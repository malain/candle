using System;
using System.CodeDom;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Xml.Schema;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell.Interop;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ImportWsdlCommand : ICommand
    {
        private SoftwareComponent _component;
        private readonly IServiceProvider _serviceProvider;
        private CodeNamespace _codeNamespace;
        private readonly ISupportArrangeShapes _nodeShape;
        private readonly CandleModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportWsdlCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="obj">The obj.</param>
        public ImportWsdlCommand(IServiceProvider serviceProvider, object obj)
        {
            this._serviceProvider = serviceProvider;
            _nodeShape = obj as ISupportArrangeShapes;
            if (obj is SoftwareComponentShape)
            {
                this._component = ((SoftwareComponentShape)obj).ModelElement as SoftwareComponent;
                _model = _component.Model;
            }
            else if (obj is Diagram)
                this._model = ((Diagram)obj).ModelElement as CandleModel;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            try
            {
                IVsAddWebReferenceDlg dlg = (IVsAddWebReferenceDlg)_serviceProvider.GetService(typeof(SVsAddWebReferenceDlg));
                if (dlg == null)
                    return;
                int pfCanceled;
                string wsdlAddress;
                if (dlg.AddWebReferenceDlg(out wsdlAddress, out pfCanceled) != Microsoft.VisualStudio.VSConstants.S_OK || pfCanceled != 0)
                    return;
                if (String.IsNullOrEmpty(wsdlAddress))
                    return;

                if (_component == null)
                    CreateComponent();

                using (Transaction transaction = _model.Store.TransactionManager.BeginTransaction("Import wsdl"))
                {
                    _model.BaseAddress = wsdlAddress;
                    //if (wsdlAddress.EndsWith("?wsdl", StringComparison.CurrentCultureIgnoreCase))
                    //    model.BaseAddress = wsdlAddress.Substring(0, wsdlAddress.Length - 5);

                    ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                    _codeNamespace = new CodeNamespace(_component.Namespace);
                    CheckForImports(wsdlAddress, sdi);
                    sdi.Import(_codeNamespace, null);
                    if (sdi.ServiceDescriptions.Count > 0)
                        _model.Comment = sdi.ServiceDescriptions[0].Documentation;
                    
                    InterfaceLayer iLayer = EnsureInterfaceLayer();
                    EnsureDefaultPackage();

                    foreach (CodeTypeDeclaration ctDecl in _codeNamespace.Types)
                    {
                        if (ctDecl.IsClass && ctDecl.BaseTypes.Count > 0)
                        {
                            if (ctDecl.BaseTypes[0].BaseType == "System.Web.Services.Protocols.SoapHttpClientProtocol")
                            {
                                ServiceContract contract = new ServiceContract(_component.Store);
                                contract.Name = ctDecl.Name;
                                iLayer.ServiceContracts.Add(contract);

                                foreach (CodeMemberMethod method in ctDecl.Members)
                                {
                                    if (method.ReturnType.BaseType == "System.IAsyncResult" || method.Name==".ctor")
                                        continue;
                                    bool ignore = false;
                                    foreach (CodeParameterDeclarationExpression parm in method.Parameters)
                                    {
                                        if (parm.Type.BaseType == "System.IAsyncResult")
                                        {
                                            ignore = true;
                                            break;
                                        }
                                    }
                                    if (ignore)
                                        continue;

                                    Operation op = new Operation(_component.Store);
                                    contract.Operations.Add(op);
                                    op.Name = method.Name;
                                    op.Type = ValidateType(op, method.ReturnType);

                                    foreach (CodeParameterDeclarationExpression parm in method.Parameters)
                                    {
                                        Argument arg = new Argument(_component.Store);
                                        op.Arguments.Add(arg);
                                        arg.Name = parm.Name;
                                        arg.Type = ValidateType(arg, parm.Type);
                                        switch (parm.Direction)
                                        {
                                            case FieldDirection.In:
                                                arg.Direction = ArgumentDirection.In;
                                                break;
                                            case FieldDirection.Out:
                                                arg.Direction = ArgumentDirection.Out;
                                                break;
                                            case FieldDirection.Ref:
                                                arg.Direction = ArgumentDirection.InOut;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (ctDecl.IsClass)
                        {
                            Entity entity = new Entity(_component.Store);
                            entity.Name = ctDecl.Name;
                            Package package = EnsureDefaultPackage();
                            package.Types.Add(entity);

                            foreach (CodeMemberField member in ctDecl.Members)
                            {
                                Property p = new Property(_component.Store);
                                entity.Properties.Add(p);
                                p.Name = member.Name;
                                p.Type = ValidateType(p, member.Type);
                            }
                        }
                        else if (ctDecl.IsEnum)
                        {
                            Enumeration e = new Enumeration(_component.Store);
                            e.Name = ctDecl.Name;
                            Package package = EnsureDefaultPackage();
                            package.Types.Add(e);

                            foreach (CodeTypeMember member in ctDecl.Members)
                            {
                                EnumValue p = new EnumValue(_component.Store);
                                p.Type = "System.Int32";
                                e.Values.Add(p);
                                p.Name = member.Name;
                            }
                        }
                    }

                    transaction.Commit();
                }

                if (_nodeShape != null)
                {
                    using (Transaction transaction = _component.Store.TransactionManager.BeginTransaction("Arrange shapes"))
                    {
                        _nodeShape.ArrangeShapes();
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Import WSDL", "Import Canceled", ex);
            }
        }

        /// <summary>
        /// Création du composant principal initialisé à partir d'une assembly
        /// </summary>
        private void CreateComponent()
        {
            using (Transaction transaction = _model.Store.TransactionManager.BeginTransaction("Init main component"))
            {
                // Création du composant
                _component = new SoftwareComponent(_model.Store);
                _component.Name = "?"; // Pour forcer le wizard
                _model.Component = _component;
                // Force la transaction pour afficher le wizard
                transaction.Commit();
            }
        }

        /// <summary>
        /// Ensures the default package.
        /// </summary>
        /// <returns></returns>
        private Package EnsureDefaultPackage()
        {
            if (_component.DataLayer.Packages.Count > 0)
                return _component.DataLayer.Packages[0];
            Package p = new Package(_component.Store);
            p.Name = _component.DataLayer.Namespace;
            _component.DataLayer.Packages.Add(p);
            return p;
        }

        /// <summary>
        /// Ensures the interface layer.
        /// </summary>
        /// <returns></returns>
        private InterfaceLayer EnsureInterfaceLayer()
        {
            LayerPackage lp = new LayerPackage(_component.Store);
            lp.Name = "PublicContracts";
            lp.Level = 100;
            _component.LayerPackages.Add(lp);

            InterfaceLayer iLayer = new InterfaceLayer(_component.Store);
            iLayer.Level = 101;
            _component.Layers.Add(iLayer);
            lp.InterfaceLayer = iLayer;
            iLayer.Name = StrategyManager.GetInstance(lp.Store).NamingStrategy.CreateLayerName(lp, iLayer, "PublicContracts");
            iLayer.Namespace = StrategyManager.GetInstance(lp.Store).NamingStrategy.CreateNamespace(_component.Namespace, iLayer.Name, iLayer);
            return iLayer;
        }

        /// <summary>
        /// Validates the type.
        /// </summary>
        /// <param name="elem">The elem.</param>
        /// <param name="codeTypeReference">The code type reference.</param>
        /// <returns></returns>
        private string ValidateType(ITypeMember elem, CodeTypeReference codeTypeReference)
        {
            elem.IsCollection = codeTypeReference.ArrayRank > 0;

            string codeName = codeTypeReference.BaseType;
            if (codeTypeReference.ArrayRank > 0)
                codeName = codeTypeReference.ArrayElementType.BaseType;

            foreach (CodeTypeDeclaration ctd in _codeNamespace.Types)
            {
                if ((ctd.IsClass || ctd.IsEnum) && ctd.Name == codeName)
                    return String.Concat(_component.DataLayer.Packages[0].Name, ".", codeName);
            }
            return codeName;
        }

        /// <summary>
        /// Checks for imports.
        /// </summary>
        /// <param name="baseWSDLUrl">The base WSDL URL.</param>
        /// <param name="sdi">The SDI.</param>
        private static void CheckForImports(string baseWSDLUrl, ServiceDescriptionImporter sdi)
        {
            DiscoveryClientProtocol dcp = new DiscoveryClientProtocol();
            dcp.Credentials = System.Net.CredentialCache.DefaultCredentials;
            dcp.Proxy = System.Net.WebProxy.GetDefaultProxy();
            dcp.DiscoverAny(baseWSDLUrl);
            dcp.ResolveAll();

            foreach (object osd in dcp.Documents.Values)
            {
                if (osd is ServiceDescription) sdi.AddServiceDescription((ServiceDescription)osd, null, null);
                if (osd is XmlSchema)
                {
                    // store in global schemas variable
                    sdi.Schemas.Add((XmlSchema)osd);
                }
            }
        }

        #region ICommand Members

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get {
                return _model!=null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _model != null && (_component == null || ( _component.LayerPackages.Count == 0 && _component.Layers.Count == 0));
        }

        #endregion
    }
}
