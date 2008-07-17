using System.Collections.Generic;
using System.Diagnostics;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using DslModeling=Microsoft.VisualStudio.Modeling;
using DslDiagrams=Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Builder générique pour toutes les références du modèle
    /// </summary>
    /// <remarks>
    /// </remarks>
    partial class ReferenceBuilder
    {
        /// <summary>
        /// Sert lors de la création des liens pour propager le contrat
        /// </summary>
        private static ServiceContract s_initialContract;

        #region Accept Connection Methods
        /// <summary>
        /// Test whether a given model element is acceptable to this ConnectionBuilder as the source of a connection.
        /// </summary>
        /// <param name="candidate">The model element to test.</param>
        /// <returns>Whether the element can be used as the source of a connection.</returns>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1800:DoNotCastUnnecessarily" )]
        public static bool CanAcceptSource( DslModeling::ModelElement candidate )
        {
            if( candidate == null )
                return false;
            return  candidate is ServiceContract ||
                    candidate is ClassImplementation ||
                    candidate is SoftwareLayer ||
                    candidate is ExternalServiceContract || 
                    candidate is ExternalPublicPort ||
                    candidate is Scenario ||
                    candidate is DotNetAssembly;
        }

        /// <summary>
        /// Test whether a given model element is acceptable to this ConnectionBuilder as the target of a connection.
        /// </summary>
        /// <param name="candidate">The model element to test.</param>
        /// <returns>Whether the element can be used as the target of a connection.</returns>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1800:DoNotCastUnnecessarily" )]
        public static bool CanAcceptTarget( DslModeling::ModelElement candidate )
        {
            if( candidate == null )
                return false;

            return candidate is ServiceContract ||
                    candidate is ClassImplementation ||
                    candidate is SoftwareLayer ||
                    candidate is ExternalServiceContract ||
                    candidate is DotNetAssembly ||
                    candidate is Scenario ||
                    candidate is ExternalPublicPort ||
                    candidate is ExternalComponent;
        }

        /// <summary>
        /// Test whether a given pair of model elements are acceptable to this ConnectionBuilder as the source and target of a connection
        /// </summary>
        /// <param name="candidateSource">The model element to test as a source</param>
        /// <param name="candidateTarget">The model element to test as a target</param>
        /// <returns>Whether the elements can be used as the source and target of a connection</returns>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1800:DoNotCastUnnecessarily" )]
        public static bool CanAcceptSourceAndTarget( DslModeling::ModelElement candidateSource, DslModeling::ModelElement candidateTarget )
        {
            if( candidateSource == candidateTarget )
                return false;

            if( candidateSource == null )
            {
                if( candidateTarget != null )
                {
                    throw new global::System.ArgumentNullException( "candidateSource" );
                }
                else // Both null
                {
                    return false;
                }
            }

            bool acceptSource = CanAcceptSource( candidateSource );
            // If the source wasn't accepted then there's no point checking targets.
            // If there is no target then the source controls the accept.
            if( !acceptSource || candidateTarget == null )
            {
                return acceptSource;
            }

            if( !CanAcceptTarget( candidateTarget ) )
                return false;

            if (candidateTarget is ExternalComponent)
            {
                if (candidateSource is DataLayer)
                    return DataLayerReferencesExternalComponent.GetLink((DataLayer)candidateSource, (ExternalComponent)candidateTarget) == null;
                return false;
            }

            //     Assembly    ->      Assembly                AssemblyReferencesAssemblies
            //     Assembly    ->      Model                   DotnetPublication
            if( candidateSource is DotNetAssembly )
            {
                if (candidateTarget is DotNetAssembly)
                    return AssemblyReferencesAssemblies.GetLink((DotNetAssembly)candidateSource, (DotNetAssembly)candidateTarget) == null;

                if( candidateTarget is ExternalPublicPort )
                {
                    return ExternalServiceReference.GetLink((AbstractLayer)candidateSource, (ExternalPublicPort)candidateTarget) == null;
                }
            }

            // Important - Comme ExternalServiceContract hérite de ExternalPublicPort, il faut faire ce test avant
            if (candidateSource is ExternalServiceContract || candidateTarget is ExternalServiceContract)
            {
                if (!(candidateSource is ExternalServiceContract))
                    Utils.Swap<ModelElement>(ref candidateSource, ref candidateTarget);

                if (candidateTarget is ClassImplementation || candidateTarget is Scenario)
                {
                    return ClassUsesOperations.GetLinks((TypeWithOperations)candidateTarget, (ExternalServiceContract)candidateSource).Count == 0;
                }

                return candidateTarget is SoftwareLayer;
            }

            if (candidateSource is ExternalPublicPort || candidateTarget is ExternalPublicPort)
            {
                // Contract tjs en temps que source
                if (!(candidateSource is ExternalPublicPort))
                    Utils.Swap<ModelElement>(ref candidateSource, ref candidateTarget);

                if (candidateTarget is AbstractLayer)
                    return ExternalServiceReference.GetLink((AbstractLayer)candidateTarget, (ExternalPublicPort)candidateSource) == null;
            }

            if (candidateSource is ServiceContract || candidateTarget is ServiceContract)
            {
                // Contract tjs en temps que source
                if (!(candidateSource is ServiceContract))
                    Utils.Swap<ModelElement>(ref candidateSource, ref candidateTarget);

                ServiceContract contract = candidateSource as ServiceContract;
                if (candidateTarget is ClassImplementation)
                {
                    ClassImplementation clazz = candidateTarget as ClassImplementation;
                    if (contract.Layer.Level > clazz.Layer.Level)
                        return clazz.Contract == null;
                    else
                        return ClassUsesOperations.GetLinks(clazz, contract).Count == 0;
                }

                if (candidateTarget is Scenario)
                {
                    SoftwareLayer iLayer = GetDownestLayer(((Scenario)candidateTarget).Layer, null);
                    return ((ServiceContract)candidateSource).Layer == iLayer && ScenarioUsesContracts.GetLinks((Scenario)candidateTarget, (ServiceContract)candidateSource).Count == 0;
                }

                if (candidateTarget is SoftwareLayer)
                {
                    int layerLevel;
                    if (candidateTarget is Layer)
                        layerLevel = ((Layer)candidateTarget).Level;
                    else if (candidateTarget is InterfaceLayer)
                        layerLevel = ((InterfaceLayer)candidateTarget).Level;
                    else
                        return false;

                    // Implementation 
                    if (contract.Layer.Level > layerLevel)
                    {
                        // Est ce qu'il y a dèjà une implémentation avec cette couche ?
                        foreach(Implementation impl in Implementation.GetLinksToImplementations(contract))
                        {
                            if( impl.ClassImplementation.Layer == ((SoftwareLayer)candidateTarget))
                                return false; // Si oui, c'est pas bon
                        }
                        return true;
                    }
                    // Utilise service
                    else if (contract.Layer.Level < layerLevel)
                    {
                        foreach (ClassUsesOperations link in ClassUsesOperations.GetLinksToSources(contract))
                        {
                           // if (link.Source.Layer == (Layer)candidateTarget)
                                return false;
                        }
                        return true;
                    }
                }
            }

            if (candidateSource is ClassImplementation)
            {
                if (candidateTarget is ClassImplementation)
                {
                    //// Dans le même layer
                    //Layer sourceLayer = ((ClassImplementation)candidateSource).Layer;
                    //Layer targetLayer = ((ClassImplementation)candidateTarget).Layer;
                    //if (targetLayer.LayerPackage  == sourceLayer.LayerPackage)
                    //{
                    //    return true;
                    //}

                    //// Sinon il faut que ce soit la couche immédiatement en dessous
                    //SoftwareLayer downestLayer=null;
                    //if (sourceLayer.Level > targetLayer.Level)
                    //{
                    //    downestLayer = GetDownestLayer(sourceLayer, null);
                    //    return downestLayer == targetLayer;
                    //}
                    //else
                    //{
                    //    downestLayer = GetDownestLayer(targetLayer, null);
                    //    return downestLayer == sourceLayer;
                    //}
                    return true;
                }

                //if (candidateTarget is Scenario)
                //{
                //    SoftwareLayer iLayer = GetDownestLayer(((Scenario)candidateTarget).Layer, null);
                //    return ((ServiceContract)candidateSource).Layer == iLayer && ScenarioUsesContracts.GetLinks((Scenario)candidateTarget, (ClassImplementation)candidateSource).Count == 0;
                //}

                return candidateTarget is InterfaceLayer;
            }

            return false;
        }

        /// <summary>
        /// Make a connection between the given pair of source and target elements
        /// </summary>
        /// <param name="source">The model element to use as the source of the connection</param>
        /// <param name="target">The model element to use as the target of the connection</param>
        /// <returns>A link representing the created connection</returns>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1800:DoNotCastUnnecessarily" )]
        public static DslModeling::ElementLink Connect( DslModeling::ModelElement source, DslModeling::ModelElement target )
        {
            if( source == null )
            {
                throw new global::System.ArgumentNullException( "source" );
            }
            if( target == null )
            {
                throw new global::System.ArgumentNullException( "target" );
            }

            if( CanAcceptSourceAndTarget( source, target ) )
            {
                if (target is ExternalComponent)
                {
                    if (source is DataLayer)
                        return new DataLayerReferencesExternalComponent((DataLayer)source, (ExternalComponent)target);
                }
                
                // Enregistrement de la position initiale de l'élément initial pour aligner
                // les élements verticalement dans le cas d'une référence multicouches.
                IList<PresentationElement> shapes = null;
                // On privilégie la source
                if (source is ClassImplementation || source is ServiceContract)
                {
                    shapes = PresentationViewsSubject.GetPresentation(source);
                }
                else if (target is ClassImplementation || target is ServiceContract)
                {
                    shapes = PresentationViewsSubject.GetPresentation(target);
                }

                if( shapes!= null && shapes.Count > 0 )
                {
                    UnplacedModelHelper.RegisterInitialPosition(source.Store, ((NodeShape)shapes[0]).AbsoluteBoundingBox.X);
                }                

                ElementLink link = CreateLink( source, target );
                if( link != null )
                    return link;
            }

            global::System.Diagnostics.Debug.Fail( "Having agreed that the connection can be accepted we should never fail to make one." );
            throw new global::System.InvalidOperationException();
        }
        #endregion

        #region Connection Methods

        /// <summary>
        /// Creates the link.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private static ElementLink CreateLink( ModelElement source, ModelElement target )
        {
            s_initialContract = null;

            //     Assembly    ->      Assembly                AssemblyReferencesAssemblies
            //     Assembly    ->      Model                   DotnetPublication
            //     Assembly    ->      ExternalPort      ExternalServiceReference
            if( source is DotNetAssembly || target is DotNetAssembly)
            {
                if (target is DotNetAssembly && !(source is DotNetAssembly))
                    Utils.Swap<ModelElement>(ref source, ref target);
                return CreateLinkFromDotNetAssembly(source, target);
            }

            // Important - Comme ExternalServiceContract hérite de ExternalPublicPort, il faut faire ce test avant
            if (source is ExternalServiceContract || target is ExternalServiceContract)
            {
                // Contract tjs en temps que source
                if (!(source is ExternalServiceContract))
                    Utils.Swap<ModelElement>(ref source, ref target);
                ServiceContract contract = null;
                if (target is InterfaceLayer)
                {
                    contract = CreateContract(((ExternalServiceContract)source).ReferencedServiceContract, (InterfaceLayer)target);
                    target = GetDownestLayer((InterfaceLayer)target, null);
                }
                if (target is UIWorkflowLayer)
                {
                    Scenario scenario = CreateScenario(((ExternalServiceContract)source).ReferencedServiceContract, (UIWorkflowLayer)target);
                    return new ClassUsesOperations(scenario, (ExternalServiceContract)source);
                }

                if (target is Layer)
                {
                    ClassImplementation clazz = CreateClass(((ExternalServiceContract)source).ReferencedServiceContract, (Layer)target);
                    if (contract != null)
                        clazz.Contract = contract;
                    target = clazz;
                }

                if (target is ClassImplementation || target is Scenario)
                    return new ClassUsesOperations((TypeWithOperations)target, (ExternalServiceContract)source);
            }

            if( source is ExternalPublicPort || target is ExternalPublicPort )
            {
                // Contract tjs en temps que source
                if( !(source is ExternalPublicPort) )
                    Utils.Swap<ModelElement>(ref source, ref target);

                if (target is AbstractLayer)
                    return new ExternalServiceReference((AbstractLayer)target, (ExternalPublicPort)source);
            }

            if (source is ServiceContract || target is ServiceContract)
            {
                if (!(source is ServiceContract))
                    Utils.Swap<ModelElement>(ref source, ref target);

                s_initialContract = source as ServiceContract;
                if (target is ClassImplementation)
                {
                    return CreateLink(s_initialContract, (ClassImplementation)target);
                }

                if (target is Scenario)
                {
                    return new ScenarioUsesContracts((Scenario)target, (ServiceContract)source);
                }

                if (target is SoftwareLayer)
                {
                    return CreateLink(s_initialContract, (SoftwareLayer)target);
                }

            }

            if (source is ClassImplementation)
            {
                if( target is ClassImplementation )
                    return CreateLink((ClassImplementation)source, (ClassImplementation)target);
                if (target is InterfaceLayer )
                    return CreateLink((ClassImplementation)source, (SoftwareLayer)target);
                if (target is ExternalServiceContract)
                {
                    return new ClassUsesOperations((ClassImplementation)source, (ExternalServiceContract)target);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the link.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        private static ElementLink CreateLink(ClassImplementation clazz, SoftwareLayer layer)
        {
            if (clazz.Layer.Level > ((ISortedLayer)layer).Level)
            {
                InterfaceLayer iLayer = (InterfaceLayer)GetDownestLayer(clazz.Layer, layer);
                ServiceContract contract = CreateContract(clazz, iLayer);
                ClassUsesOperations link = new ClassUsesOperations(clazz, contract);
                if (iLayer == layer)
                {
                    return link;
                }
                return CreateLink(contract, layer);
            }
            else // On monte
            {
                if (layer is InterfaceLayer)
                {
                    ServiceContract contract = CreateContract(clazz, (InterfaceLayer)layer);
                    return CreateLink(contract, clazz);
                }
                ClassImplementation clazz2 = CreateClass(clazz, (Layer)layer);
                return CreateLink(clazz2, clazz);
            }
        }

        /// <summary>
        /// Creates the link.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="targetLayer">The target layer.</param>
        /// <returns></returns>
        private static ElementLink CreateLink(ServiceContract contract, SoftwareLayer targetLayer)
        {
            InterfaceLayer iLayer = targetLayer as InterfaceLayer;
            Layer layer = targetLayer as Layer;
            
            Debug.Assert(iLayer != null || layer != null);
            int layerLevel = 0;
            if( layer != null )
                layerLevel = layer.Level;
            else if (iLayer != null )
                layerLevel = iLayer.Level;


            // On descend 
            if (contract.Layer.Level > layerLevel)
            {
                // Implémentation
                Layer downLayer = GetDownestLayer(contract.Layer, targetLayer) as Layer;
                Debug.Assert(downLayer != null);
                ClassImplementation impl = CreateClass(contract, downLayer);
                Implementation link = new Implementation(impl, contract);
                if (downLayer == targetLayer)
                {
                    return link;
                }
                return CreateLink(impl, targetLayer);
            }
            else 
            {
                // On monte
                if (layer != null)
                {
                    ClassImplementation impl = CreateClass(contract, layer);
                    return CreateLink(contract, impl);
                }
                // Sinon la couche destinatrice est une couche d'interface
                ServiceContract contract2 = CreateContract(contract, iLayer);
                return CreateLink(contract, contract2);
            }
        }

        /// <summary>
        /// Creates the link from dot net assembly.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private static ElementLink CreateLinkFromDotNetAssembly(ModelElement source, ModelElement target)
        {
            if (target is DotNetAssembly)
                return new AssemblyReferencesAssemblies((DotNetAssembly)source, (DotNetAssembly)target);

            //     Assembly       ->      ExternalPort      ExternalServiceReference
            if( target is ExternalPublicPort )
            {
                return new ExternalServiceReference((AbstractLayer)source, (ExternalPublicPort)target);
            }

            return null;
        }

        /// <summary>
        /// Creates the link.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="contract2">The contract2.</param>
        /// <returns></returns>
        private static ElementLink CreateLink(ServiceContract contract, ServiceContract contract2)
        {
            if (contract.Layer.Level < contract2.Layer.Level)
                Utils.Swap<ServiceContract>(ref contract, ref contract2);

            Layer nextLayer = GetDownestLayer(contract.Layer, contract2.Layer) as Layer;

            ClassImplementation clazz = CreateClass(contract, nextLayer);
            new Implementation(clazz,contract);
            return CreateLink(contract2, clazz);
        }

        /// <summary>
        /// Creates the link.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        /// <param name="clazz2">The clazz2.</param>
        /// <returns></returns>
        private static ElementLink CreateLink(ClassImplementation clazz, ClassImplementation clazz2)
        {
            if (clazz.Layer.Level < clazz2.Layer.Level)
                Utils.Swap<ClassImplementation>(ref clazz, ref clazz2);

            if (clazz.Layer.LayerPackage == clazz2.Layer.LayerPackage)
                return new ClassUsesOperations(clazz, clazz2);

            SoftwareLayer nextLayer = GetDownestLayer(clazz.Layer, clazz2.Layer);
            if (nextLayer is InterfaceLayer)
            {
                ServiceContract contract = CreateContract(clazz2, (InterfaceLayer)nextLayer);
                new ClassUsesOperations(clazz, contract);
                return CreateLink(contract, clazz2);
            }
            
            ClassImplementation clazz3 = CreateClass(clazz2, (Layer)nextLayer);
            ClassUsesOperations link = new ClassUsesOperations(clazz, clazz3);
            link.Scope |= ReferenceScope.Compilation;
            if (nextLayer == clazz2.Layer)
                return link;
            return CreateLink(clazz3, clazz2);
        }

        /// <summary>
        /// Creates the link.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="clazz">The clazz.</param>
        /// <returns></returns>
        private static ElementLink CreateLink(ServiceContract contract, ClassImplementation clazz)
        {
            // Le contrat est en dessous
            if (contract.Layer.Level < clazz.Layer.Level)
            {
                InterfaceLayer iLayer = GetDownestLayer(clazz.Layer, contract.Layer) as InterfaceLayer;
                if (iLayer == contract.Layer)
                {
                    return new ClassUsesOperations(clazz, contract);
                }

                ServiceContract contract2 = CreateContract(contract, iLayer);
                new ClassUsesOperations(clazz, contract2);
                return CreateLink(contract2, contract);
                //// Sélection de l'opération
                //OperationSelectorDlg dlg = new OperationSelectorDlg(contract);
                //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    Operation op = dlg.SelectedOperation;
                //    if (op != null)
                //    {
                //        ClassUsesOperations link = new ClassUsesOperations(clazz, contract);
                //        link.Operation = op;
                //        return link;
                //    }
                //}
                //throw new CanceledByUser();
            }
            // Le contrat est au dessus
            else
            {
                Layer nextLayer = GetDownestLayer(contract.Layer, clazz.Layer) as Layer;
                if (nextLayer == null)
                    return null;
                if (nextLayer == clazz.Layer)
                {
                    // Si c'est la couche du dessous, on donne le bon nom à l'interface
                    //contract.Name = StrategyManager.GetInstance(clazz.Store).NamingStrategy.CreateElementName(contract.Layer, clazz.Name);
                    return new Implementation(clazz, contract);
                }
                ClassImplementation clazz2 = CreateClass(contract, nextLayer);
                return CreateLink(clazz2, clazz);
            }
        }

        /// <summary>
        /// Recherche de la couche immédiatement en dessous
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="potentialLayer">The potential layer.</param>
        /// <returns></returns>
        private static SoftwareLayer GetDownestLayer(SoftwareLayer layer, SoftwareLayer potentialLayer)
        {
            // Recherche du layerPackage en dessous
            if( layer is InterfaceLayer )
            {
                LayerPackage layerPackage = null;
                foreach (LayerPackage lp in layer.SoftwareComponent.LayerPackages)
                {
                    if (lp.Level < ((InterfaceLayer)layer).Level)
                    {
                        if (layerPackage == null || layerPackage.Level < lp.Level)
                            layerPackage = lp;
                    }
                }

                if (layerPackage != null)
                {
                    List<SoftwareLayer> layers = new List<SoftwareLayer>();
                    foreach (SoftwareLayer sl in layerPackage.Layers)
                        layers.Add(sl);
                    // TODO a voir - On retourne tjs le 1er layer
                    if (layers.Count == 0)
                        return null;
                    foreach (SoftwareLayer sl in layers)
                    {
                        if (sl == potentialLayer)
                            return potentialLayer;
                    }
                    return layers[0];
                }
            }
            else
            {

                // Recherche de la couche d'interface
                ISortedLayer theLayer = null;
                foreach (AbstractLayer al in layer.SoftwareComponent.Layers)
                {
                    ISortedLayer sl = al as ISortedLayer;
                    if (sl != null && sl.Level < ((Layer)layer).Level)
                    {
                        if (theLayer == null || theLayer.Level < sl.Level)
                            theLayer = sl;
                    }
                }

                return theLayer as SoftwareLayer;
            }

            return null;
        }

        /// <summary>
        /// Création d'un scenario
        /// </summary>
        /// <param name="elem">The elem.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        private static Scenario CreateScenario(CandleElement elem, UIWorkflowLayer layer)
        {
            string name = StrategyManager.GetInstance(elem.Store).NamingStrategy.CreateElementName(layer, elem.RootName);

            // On regarde d'abord si il n'existe pas une classe du même nom
            foreach (Scenario cl in layer.Scenarios)
            {
                if (cl.Name == name)
                    return cl;
            }

            // Sinon création d'une nouvelle
            Scenario scenario = new Scenario(layer.Store);
            scenario.RootName = elem.RootName;
            scenario.Name = name;
            scenario.Comment = elem.Comment;
            layer.Scenarios.Add(scenario);

            UnplacedModelHelper.RegisterNewModel(layer.Store, scenario);
            return scenario;
        }

        /// <summary>
        /// Création d'une nouvelle classe
        /// </summary>
        /// <param name="elem">The elem.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        private static ClassImplementation CreateClass(CandleElement elem, Layer layer)
        {
            string name = StrategyManager.GetInstance(elem.Store).NamingStrategy.CreateElementName(layer, elem.RootName);

            // On regarde d'abord si il n'existe pas une classe du même nom
            foreach (ClassImplementation cl in layer.Classes)
            {
                if (cl.Name == name)
                    return cl;
            }

            // Sinon création d'une nouvelle
            ClassImplementation clazz = new ClassImplementation(layer.Store);
            clazz.RootName = elem.RootName;
            clazz.Name = name;
            clazz.Comment = elem.Comment;
            layer.Classes.Add(clazz);

            UnplacedModelHelper.RegisterNewModel(layer.Store, clazz);
            return clazz;
        }

        /// <summary>
        /// Création d'un nouveau contrat
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        private static ServiceContract CreateContract(CandleElement port, InterfaceLayer layer)
        {
            string name = StrategyManager.GetInstance(port.Store).NamingStrategy.CreateElementName(layer, port.RootName);
            // Vérification de l'existence
            foreach (ServiceContract sc in layer.ServiceContracts)
            {
                if (sc.Name == name)
                    return sc;
            }

            // Sinon création avec copie des opérations
            ServiceContract contract = new ServiceContract(layer.Store);
            layer.ServiceContracts.Add(contract);
            contract.RootName = port.RootName;
            UnplacedModelHelper.RegisterNewModel(layer.Store, contract);

            TypeWithOperations copiedContract = null;
            if (port != null)
            {
                if (port is ServiceContract || port is ExternalServiceContract)
                {
                    copiedContract = port as TypeWithOperations;
                    contract.Name = name;
                }
                else
                {
                    ClassImplementation clazz = port as ClassImplementation;
                    contract.Name = StrategyManager.GetInstance(clazz.Store).NamingStrategy.CreateElementName(layer, clazz.Name);
                    if (clazz.Contract != null)
                        copiedContract = clazz.Contract;
                    contract.Comment = clazz.Comment;
                }
            }

            if (copiedContract != null)
                TypeWithOperations.CopyOperations(copiedContract, contract);

            return contract;
        }
        #endregion
    }
}
