using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextTemplating;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Generator
    {
        private static IServiceProvider s_serviceProvider;
        private static GenerationContext s_context;

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public static GenerationContext Context
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return s_context; }
        }

        /// <summary>
        /// Notifies the publish events.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="eventType">Type of the event.</param>
        internal static void NotifyPublishEvents(CandleModel model, string fileName, PublishEventType eventType)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.BeginStep("Custom strategy publishing actions", LogType.Info);

            try
            {
                foreach (StrategyBase strategy in StrategyManager.GetStrategies(model.Component, null))
                {
                    if (!strategy.IsEnabled)
                        continue;

                    try
                    {
                        s_context = new GenerationContext(model, fileName, Guid.Empty);
                        s_context.GenerationPass = GenerationPass.Publishing;
                        strategy.InitializeContext(s_context, model.Component);

                        switch (eventType)
                        {
                            case PublishEventType.EndPublish:
                                if (strategy is IStrategyPublishEvents)
                                {
                                    ((IStrategyPublishEvents)strategy).OnPublicationEnded(model, fileName);
                                    if (logger != null)
                                        logger.Write("Custom strategy publishing actions", "Executing strategy before local custom publishing action for " + strategy.DisplayName, LogType.Info);
                                }
                                break;
                            case PublishEventType.PublishFailed:
                                if (strategy is IStrategyPublishEvents)
                                {
                                    ((IStrategyPublishEvents)strategy).OnPublicationError(model, fileName);
                                    if (logger != null)
                                        logger.Write("Custom strategy publishing actions", "Executing strategy before local custom publishing action for " + strategy.DisplayName, LogType.Info);
                                }
                                break;
                            case PublishEventType.BeforeLocal:
                                if (strategy is IStrategyPublishEvents)
                                {
                                    ((IStrategyPublishEvents)strategy).OnBeforeLocalPublication(model, fileName);
                                    if (logger != null)
                                        logger.Write("Custom strategy publishing actions", "Executing strategy before local custom publishing action for " + strategy.DisplayName, LogType.Info);
                                }
                                break;
                            case PublishEventType.AfterLocal:
                                if (strategy is IStrategyPublisher)
                                {
                                    string folder = Path.GetDirectoryName(model.MetaData.GetFileName(PathKind.Absolute));
                                    ((IStrategyPublisher)strategy).Publish(model, folder);
                                    if (logger != null)
                                        logger.Write("Custom strategy publishing actions", "Executing strategy custom publishing action for " + strategy.DisplayName, LogType.Info);
                                }
                                if (strategy is IStrategyPublishEvents)
                                {
                                    ((IStrategyPublishEvents)strategy).OnAfterLocalPublication(model, fileName);
                                    if (logger != null)
                                        logger.Write("Custom strategy publishing actions", "Executing strategy after local custom publishing action for " + strategy.DisplayName, LogType.Info);
                                }
                                break;
                            case PublishEventType.BeforeServer:
                                if (strategy is IStrategyPublishEvents)
                                {
                                    ((IStrategyPublishEvents)strategy).OnBeforeServerPublication(model, fileName);
                                    if (logger != null)
                                        logger.Write("Custom strategy publishing actions", "Executing strategy before server custom publishing action for " + strategy.DisplayName, LogType.Info);
                                }
                                break;
                            case PublishEventType.AfterServer:
                                if (strategy is IStrategyPublishEvents)
                                {
                                    ((IStrategyPublishEvents)strategy).OnAfterServerPublication(model, fileName);
                                    if (logger != null)
                                        logger.Write("Custom strategy publishing actions", "Executing strategy after server custom publishing action for " + strategy.DisplayName, LogType.Info);
                                }
                                break;
                        }
                    }
                    catch (PublishingException pe)
                    {
                        if (logger != null)
                            logger.WriteError("Custom strategy publishing actions", "Publishing stopped for strategy custom publishing action for " + strategy.DisplayName, pe);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        if (logger != null)
                            logger.WriteError("Custom strategy publishing actions", "Error for strategy custom publishing action for " + strategy.DisplayName, ex);
                    }
                    finally
                    {
                        strategy.InitializeContext(null, null);
                    }
                }
            }
            finally
            {
                if (logger != null)
                    logger.EndStep();
            }
        }

        /// <summary>
        /// Generation appelée lors de l'insertion d'un elément.
        /// Correspond à la phase GenerationPas.MetaModelElementAdded
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="model">The model.</param>
        /// <param name="selectedElement">The selected element.</param>
        public static void GenerateWhenElementAdded(IServiceProvider serviceProvider, CandleModel model, ICustomizableElement selectedElement)
        {
            Debug.Assert(model != null);
            Debug.Assert(model != null);

            if (selectedElement.StrategiesOwner == null)
                return;

            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.BeginProcess(true, false);

            Generator.s_serviceProvider = serviceProvider;

            try
            {
                s_context = new GenerationContext(model, String.Empty, selectedElement.Id);

                ServiceLocator.Instance.IDEHelper.SetWaitCursor();

                if (logger != null)
                    logger.BeginStep("Element added", LogType.Debug);

                ServiceLocator.Instance.IDEHelper.DisplayProgress("Generate...", 1, 2);
                // Mise à jour du méta modèle
                s_context.GenerationPass = GenerationPass.ElementAdded;
                using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Update metamodel"))
                {
                    selectedElement.StrategiesOwner.GenerateCode(s_context);
                    transaction.Commit();
                }
                if (logger != null)
                    logger.EndStep();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Generator", "element added " + selectedElement.Name, ex);
            }
            finally
            {
                // On s'assure de faire disparaitre la progress bar
                ServiceLocator.Instance.IDEHelper.DisplayProgress("", 2, 0);
                // [Comment] Pas d'affichage car pb quand on édite une fenetre détails
                // a chaque ajout il ne faut pas afficher la fenetre d'erreur
                //ServiceLocator.Instance.IDEHelper.ShowErrorList();
                if (logger != null)
                    logger.EndProcess();
            }
        }

        /// <summary>
        /// Generates the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        /// <param name="selectedElement">The selected element.</param>
        public static void Generate(IServiceProvider serviceProvider, string modelFileName, ICustomizableElement selectedElement)
        {
            if( modelFileName == null )
                throw new ArgumentNullException( "modelFileName" );

            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
            Generator.s_serviceProvider = serviceProvider;

            try
            {
                // Sauvegarde de tous les documents
                ServiceLocator.Instance.ShellHelper.Solution.DTE.Documents.SaveAll();

                CandleModel model = CandleModel.GetModelFromCurrentSolution(modelFileName);
                // Chargement du modèle
                //ModelLoader loader = ModelLoader.GetLoader(modelFileName, false);
                //if (loader == null || loader.Model == null)
                //{
                //    if (logger != null)
                //        logger.Write("Generator", "unable to load the model", LogType.Error);
                //    return;
                //}

                //CandleModel model = loader.Model;
                if (model.Component == null)
                {
                    if (logger != null)
                        logger.Write("Generator", "model contains no software component.", LogType.Error);
                    return;
                }
                if (StrategyManager.GetInstance(model.Store).GetStrategies(null, true).Count == 0)
                {
                    if (logger != null)
                        logger.Write("Generator", "No strategies configured.", LogType.Error);
                    return;
                }

//                CandleModel model = loader.Model;

                s_context = new GenerationContext(model, modelFileName, selectedElement != null ? selectedElement.Id : Guid.Empty);

                GenerationPass generationPassesSelected = GenerationPass.CodeGeneration | GenerationPass.MetaModelUpdate;
                try
                {
                    // Demande des stratégies à executer
                    if (selectedElement != null)
                    {
                        RunningStrategiesForm dlg = new RunningStrategiesForm(selectedElement.StrategiesOwner);
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            return;
                        s_context.SelectedStrategies = dlg.SelectedStrategies;
                        generationPassesSelected = dlg.SelectedGenerationPasses;
                    }

                    if (logger != null)
                        logger.BeginProcess(true, true);

                    // Préparation de l'IDE
                    ide.SetWaitCursor();
                    ide.DisplayProgress("Generate...", 1, 3);

                    Microsoft.VisualStudio.Modeling.Validation.ValidationContext vc = new Microsoft.VisualStudio.Modeling.Validation.ValidationContext(Microsoft.VisualStudio.Modeling.Validation.ValidationCategories.Save, model);
                    if (vc.CurrentViolations.Count > 0)
                    {
                        if (ide != null)
                        {
                            if (ide.ShowMessageBox("There is validation errors, continue generate ?", "Generation", MessageBoxButtons.YesNo) == DialogResult.No)
                                return;
                        }
                    }

                    // Au cas ou cette passe a été forcé dans la boite de dialogue
                    if ((generationPassesSelected & GenerationPass.ElementAdded) == GenerationPass.ElementAdded)
                    {
                        if (logger != null)
                            logger.BeginStep("Code Generation for the ElementAdded event", LogType.Info);
                        s_context.GenerationPass = GenerationPass.ElementAdded;
                        using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Update metamodel"))
                        {
                            model.Component.GenerateCode(s_context);
                            transaction.Commit();
                        }
                        if (logger != null)
                            logger.EndStep();
                    }

                    // Mise à jour du méta modèle
                    if ((generationPassesSelected & GenerationPass.MetaModelUpdate) == GenerationPass.MetaModelUpdate)
                    {
                        if (logger != null)
                            logger.BeginStep("1) Meta Model Update", LogType.Info);
                        s_context.GenerationPass = GenerationPass.MetaModelUpdate;
                        using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Update metamodel"))
                        {
                            // On ne veut pas que les wizards soient appelés
                            model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[StrategyManager.IgnoreStrategyWizards] = true;
                            model.Component.GenerateCode(s_context);
                            if( transaction.HasPendingChanges )
                                transaction.Commit();
                        }
                        if (logger != null)
                            logger.EndStep();
                    }

                    if (logger != null)
                        logger.BeginStep("1) Check references", LogType.Info);

                    // Vérification des dépendances
                    ReferencesHelper.CheckReferences(true, vc, new ConfigurationMode(), ReferenceScope.Compilation, model);
                    if (logger != null)
                        logger.EndStep();

                    ide.DisplayProgress("Generate...", 2, 3);

                    // Génération de code
                    if ((generationPassesSelected & GenerationPass.CodeGeneration) == GenerationPass.CodeGeneration)
                    {
                        if (logger != null)
                            logger.BeginStep("2) Code Generation", LogType.Info);
                        s_context.GenerationPass = GenerationPass.CodeGeneration;
                        model.Component.GenerateCode(s_context);
                        if (logger != null)
                            logger.EndStep();
                    }
                }
                finally
                {
                    Mapper.Instance.Save();
                }

                //if (logger != null)
                //    logger.BeginStep("Generation epilogue", LogType.Debug);


                //if (logger != null)
                //    logger.EndStep();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Generator", "Generator", ex);
            }
            finally
            {
                // On s'assure de faire disparaitre la progress bar
                ide.DisplayProgress("", 2, 0);
                if (logger != null)
                    logger.EndProcess();
                if (ide != null)
                    ide.ShowErrorList();

            }
        }

        /// <summary>
        /// Appel d'un autre template de transformation
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="T4Template">The t4 template.</param>
        /// <param name="outputFile">Nom du fichier de sortie ou null</param>
        /// <param name="properties">Dictionnaire contenant des variables de remplacement sur le code T4 avant l'exécution ou null</param>
        /// <returns></returns>
        internal static string CallT4Template(ICustomizableElement element, string T4Template, string outputFile, TemplateProperties properties )
        {
            if (element == null)
                throw new ArgumentNullException("element must not be null in CallT4Template");

            if (String.IsNullOrEmpty(T4Template))
                throw new ArgumentException("T4Template is required in CallT4Template");


            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            string data = null;
            Encoding encoding = Encoding.Default;

            try
            {
                RepositoryFile repFile = null;

                // Si le fichier n'est pas un chemin statique, on le récupére sur le repository
                if( !Path.IsPathRooted( T4Template ) )
                {
                    // On le prend dans le repository
                    repFile = new RepositoryFile( CandleSettings.GetT4TemplateFileName( T4Template ) );
                }

                string inputFileContent=null;
                try
                {
                    if( repFile != null )
                    {
                        inputFileContent = repFile.ReadContent( out encoding );
                    }
                    else
                    {
                        encoding = RepositoryFile.FindEncodingFromFile( T4Template );
                        inputFileContent = File.ReadAllText( T4Template );
                    }
                }
                catch { }

                if( String.IsNullOrEmpty( inputFileContent ) )
                    throw new ApplicationException( String.Format( "Template {0} not found or incorrect.", T4Template ) );

                // Génération du code
                CandleTemplateHost host = new CandleTemplateHost(element, properties);                
                Engine engine = new Engine();
                data = engine.ProcessTemplate( inputFileContent, host );

                if (host.Errors.HasErrors)
                {
                    StringBuilder sb = new StringBuilder(" ERRORS {");
                    sb.Append(T4Template);
                    sb.Append("} \n");

                    foreach (CompilerError error in host.Errors)
                    {
                        if (!error.IsWarning)
                        {
                            sb.Append(error.ToString());
                            sb.AppendLine();
                        }
                    }

                    if (String.IsNullOrEmpty(data))
                        data = sb.ToString();
                    else
                        data = String.Concat(data, sb.ToString());

                    foreach (CompilerError err in host.Errors)
                    {
                        err.FileName = T4Template;
                    }
                    ServiceLocator.Instance.IDEHelper.LogErrors(host.Errors);
                    if (logger != null)
                        logger.Write("Calling T4", String.Format("Run T4 template ({0}) for element naming {1} (id={2}) - Output file = {3} - See errors list.", T4Template, element.Name, element.Id, outputFile ?? " in memory"), LogType.Error);
                }
                else if (logger != null)
                    logger.Write("Calling T4", String.Format("Run T4 template ({0}) for element naming {1} (id={2}) - Output file = {3}", T4Template, element.Name, element.Id, outputFile ?? " in memory"), LogType.Info);

            }
            catch( Exception ex )
            {
                data = ex.Message;
                if (logger != null)
                    logger.WriteError("Calling T4", T4Template, ex);
            }

            // Si le fichier de sortie est null, on sort. 
            // Utilise dans le cas d'un appel de template appelant d'autres templates
            // On ignore le code de la première génération
            if( !String.IsNullOrEmpty( outputFile ) && !String.IsNullOrEmpty( data ) && s_context.GenerationPass != GenerationPass.MetaModelUpdate )
            {
                WriteSafeOutputFile( outputFile, data, encoding );
                return data;
            }
            return data;
        }

        /// <summary>
        /// Writes the safe output file.
        /// </summary>
        /// <param name="outputFile">The output file.</param>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        private static void WriteSafeOutputFile( string outputFile, string data, Encoding encoding )
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.Write("Writing generated file", String.Concat("Save template generation result in " + outputFile), LogType.Debug);

            Directory.CreateDirectory( Path.GetDirectoryName(outputFile) );

            // Ecriture dans le fichier de sortie
            ServiceLocator.Instance.ShellHelper.EnsureCheckout(outputFile);

            encoding = new UTF8Encoding( false, true );
            using( SafeStreamWriter writer = new SafeStreamWriter( outputFile, false, encoding ) )
            {
                writer.Write( data );
            }
        }

        /// <summary>
        /// Execute les stratégies de génération de projet
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="context">The context.</param>
        internal static void ApplyProjectGeneratorStrategies(ICustomizableElement element, GenerationContext context)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
            {
                logger.BeginStep("Project generation " + element.Name, LogType.Info);
                logger.Write("Project generation", String.Concat("Current layer is ", element.Name, " (id=", element.Id, ")"), LogType.Debug);
            }
            IList<StrategyBase> strategies = element.GetStrategies( true );

            // D'abord exécution des stratégies générant les projets
            bool strategyFinding = false;
            foreach (StrategyBase strategy in strategies)
            {
                if( strategy is IStrategyProjectGenerator )
                {
                    strategyFinding = true;
                    try
                    {
                        strategy.InitializeContext(context, element);
                        ((IStrategyProjectGenerator)strategy).GenerateVSProject(element);
                        if (logger != null)
                            logger.Write("Project generation", String.Concat("Strategy '", strategy.DisplayName, "' id : ", strategy.StrategyId, " on layer ", element.Name), LogType.Debug);
                    }
                    catch(Exception ex)
                    {
                        if (logger != null)
                            logger.WriteError("Project generation", String.Concat("Strategy '", strategy.DisplayName, "' id : ", strategy.StrategyId, " on layer ", element.Name, " See errors list"), ex);
                    }
                    finally
                    {
                        strategy.InitializeContext(null,null);
                    }
                }
            }
            if (logger != null)
            {
                if (!strategyFinding)
                    logger.Write("Project generation", "No project generation finding", LogType.Warning);
                logger.EndStep();
            }
        }

        /// <summary>
        /// Applique les stratégies
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="context">The context.</param>
        internal static void ApplyStrategies( ICustomizableElement element, GenerationContext context )
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
            {
                logger.BeginStep("Strategy generation", LogType.Debug);
                logger.Write("Strategy generation", String.Concat("Current element is ", element.Name, " (id=", element.Id, ")"), LogType.Debug);
            }
            IList<StrategyBase> strategies = element.GetStrategies(true);

            // Execution des stratégies
            StrategyBase cfgStrategy = null;
            foreach( StrategyBase strategy in strategies )
            {
                if( strategy is IStrategyProjectGenerator)
                    continue;

                if (strategy is IStrategyConfigGenerator)
                {
                    cfgStrategy = strategy;
                    continue;
                }

                RunStrategy(element, context, strategy);
            }
            
            if (cfgStrategy != null)
                RunStrategy(element, context, cfgStrategy);

            if (logger != null)
                logger.EndStep();
        }

        /// <summary>
        /// Execute une strategie
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="context">The context.</param>
        /// <param name="strategy">The strategy.</param>
        private static void RunStrategy(ICustomizableElement element, GenerationContext context, StrategyBase strategy)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.Write("Running strategy", String.Concat("Strategy '", strategy.DisplayName, "' id : ", strategy.StrategyId), LogType.Debug);
            if (context.SelectedStrategies != null && (context.SelectedStrategies != null && !context.SelectedStrategies.Contains(strategy)))
            {
                if (logger != null)
                    logger.Write("Running strategy", "Strategy skipped because this is not a selected strategy", LogType.Debug);
            }
            else if (context.CanGenerate(element.Id))
                RunStrategyInternal(element, strategy, context);
        }

        /// <summary>
        /// Applique les stratégies d'injection de code
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <param name="element">The element.</param>
        /// <param name="context">The context.</param>
        public static void ApplyCodeInjectionStrategies(ProjectItem projectItem, ICustomizableElement element, GenerationContext context)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
            {
                logger.BeginStep("Injection code generation", LogType.Info);
                logger.Write("Injection code generation", String.Concat("Current element is ", element.Name, " (id=", element.Id, ")"), LogType.Debug);
            }

            EnvDTE80.FileCodeModel2 fcm = null;
            if (projectItem != null)
            {
                fcm = projectItem.FileCodeModel as EnvDTE80.FileCodeModel2;
                if (fcm == null)
                {
                    if (logger != null)
                    {
                        logger.Write("Injection code generation", "No code model associated with this element - exit", LogType.Debug);
                        logger.EndStep();
                    }
                    return;
                }
                //fcm.BeginBatch();
            }

            try
            {
                CodeInjectionContext gc = new CodeInjectionContext(context);

                List<StrategyBase> strategies = new List<StrategyBase>();

                // Execution des stratégies
                foreach (StrategyBase strategy in element.GetStrategies(false))
                {
                    if (!(strategy is IStrategyCodeInjector))
                        continue;

                    if (logger != null)
                        logger.Write("Injection code generation", String.Concat("Strategy ", strategy.StrategyId), LogType.Debug);
                    if (context.SelectedStrategies != null && (context.SelectedStrategies != null && !context.SelectedStrategies.Contains(strategy)))
                    {
                        if (logger != null)
                            logger.Write("Injection code generation", "Strategy skipped because this is not a selected strategy", LogType.Debug);
                        continue;
                    }
                    strategies.Add(strategy);
                }

                // Tri dans l'ordre d'exécution
//                strategies.Sort(delegate(IStrategyCodeInjector a, IStrategyCodeInjector b) { return a.ExecutionOrder.CompareTo(b.ExecutionOrder); });

                foreach (StrategyBase injector in strategies)
                {
                    if (logger != null)
                        logger.BeginStep(String.Concat("Run strategy ", injector.DisplayName, " id=", injector.StrategyId), LogType.Debug);
                    gc.CurrentElement = element;
                    gc.Strategy = (IStrategyCodeInjector)injector;

                    try
                    {
                        if (gc.GenerationContext.GenerationPass == GenerationPass.MetaModelUpdate)
                        {
                            ((IStrategyCodeInjector)injector).OnMetaModelUpdate(gc);
                        }
                        else
                        {
                            CodeModelWalker walker = new CodeModelWalker(new CodeInjectorVisitor( gc ));
                            walker.Traverse(fcm);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (logger != null)
                            logger.WriteError(injector.DisplayName, "Code Injection error", ex);
                    }
                    finally
                    {
                        gc.Strategy = null;
                        gc.CurrentElement = null;
                        if (logger != null)
                            logger.EndStep();
                    }
                }
            }
            finally
            {
                if (fcm != null)
                {
                //    fcm.EndBatch();
                //    fcm.Synchronize();
                }
            }
            if (logger != null)
                logger.EndStep();
        }

        /// <summary>
        /// Runs the strategy internal.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="strategy">The strategy.</param>
        /// <param name="context">The context.</param>
        private static void RunStrategyInternal( ICustomizableElement element, StrategyBase strategy, GenerationContext context )
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            IStrategyCodeGenerator si = strategy as IStrategyCodeGenerator;
            if (si == null)
            {
                if (logger != null)
                    logger.Write("Running strategy", "Strategy ignored because this is not a code generation strategy", LogType.Debug);
                return;
            }

            try
            {
                if (logger != null)
                    logger.Write("Running strategy", String.Concat("Execute strategy '", strategy.DisplayName, "' id : ", strategy.StrategyId), LogType.Debug);

                strategy.InitializeContext(context, element);

                if( context.GenerationPass == GenerationPass.CodeGeneration)
                    Mapper.Instance.BeginGenerationTransaction(context, strategy.StrategyId, element.Id);

                si.Execute();

                if (context.GenerationPass == GenerationPass.CodeGeneration)
                    Mapper.Instance.CommitGenerationTransaction(context);
            }
            catch(Exception ex)
            {
                if (context.GenerationPass == GenerationPass.CodeGeneration)
                    Mapper.Instance.RollbackGenerationTransaction(context);
                if (logger != null)
                    logger.WriteError("Running strategy " + strategy.DisplayName, String.Concat("Strategy '", strategy.DisplayName, "' id : ", strategy.StrategyId), ex);
            }
            finally
            {
                strategy.InitializeContext(null,null);
            }
        }
    }
}
