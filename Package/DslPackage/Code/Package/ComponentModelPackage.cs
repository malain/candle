using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Designer;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using DSLFactory.Candle.SystemModel.Configuration.VisualStudio;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [ProvideLoadKey( "Standard", Constants.ProductVersion, Constants.ProductName, Constants.CompanyName, 105 )]
    // TIPS Chargement automatique du package lors de l'ouverture d'une solution vide (nécessaire pour le wizard)
    [Microsoft.VisualStudio.Shell.ProvideAutoLoad( UIContextGuids80.SolutionExists )]

    // TIPS Ajout d'une tool window
    [ProvideToolWindow( typeof( RepositoryWindowPane ), Width=220, Height=400)] // Window="34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3", Style=VsDockStyle.Tabbed)]// Transient=false, MultiInstances=false, Orientation=ToolWindowOrientation.Left, Window="{4a9b7e51-aa16-11d0-a8c5-00a0c921a4d2}" )]
    [ProvideToolWindowVisibility( typeof( RepositoryWindowPane ), Constants.CandleEditorFactoryId )]
    [ProvideToolWindowVisibility(typeof(RepositoryWindowPane), UIContextGuids80.NoSolution)]
    [ProvideToolWindowVisibility(typeof(RepositoryWindowPane), UIContextGuids80.EmptySolution)]

    [ProvideToolWindowVisibility(typeof(CandleExplorerToolWindow), "56AF6F2B-EF94-4297-9857-8653A0AE02D8")] // Models
    [ProvideToolWindowVisibility(typeof(CandleExplorerToolWindow), "2A9B689E-9B03-4159-8AE3-0C4D51B67614")] // UIWorkflow

    // TIPS Ajout d'une fenetre dans Visual Studio
    [ProvideToolWindow(typeof(PortWindowPane), Transient = true, MultiInstances = false, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom, Window = "{34e76e81-ee4a-11d0-ae2e-00a0c90fffc3}")]
    [ProvideToolWindowVisibility( typeof( PortWindowPane ), Constants.CandleEditorFactoryId )]
    [ProvideToolWindowVisibility( typeof( PortWindowPane ), "56AF6F2B-EF94-4297-9857-8653A0AE02D8" )] // Models
    [ProvideToolWindowVisibility( typeof( PortWindowPane ), "2A9B689E-9B03-4159-8AE3-0C4D51B67614" )] // UIWorkflow
    
    // TIPS new editor factory
    [ProvideEditorFactory( typeof( DataLayerEditorFactory ), 106, TrustLevel=__VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted )]
    [ProvideEditorFactory( typeof( UIWorkflowLayerEditorFactory ), 106, TrustLevel=__VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted )]
    
    // TIPS Option page
    [ProvideOptionPage( typeof( OptionsPage ), ModelConstants.ApplicationName, "General", 200, 201, true )]
    [ProvideOptionPage( typeof( OptionsServersPage ), ModelConstants.ApplicationName, "Servers", 200, 201, true )]

    // TIPS Extender (Permet de rajouter des propriétés sur un item)
    [ProvideExtender("8D58E6AF-ED4E-48B0-8C7B-C74EF0735451", "F3E69344-1228-46a8-B117-9DC91CC7D142", "CandleExtender")]
    [ProvideExtender("E231573C-C018-4768-A383-18B73F267E71", "F3E69344-1228-46a8-B117-9DC91CC7D142", "CandleExtender")]

 //   [ProvideCodeGenerator( typeof(DSLFactory.Candle.SystemModel.Strategies.ConfigurationFileGenerator), ConfigurationFileGenerator.Name, "Generate configuration file from a candle model", true)]
    [ProvideObject(typeof(CandleExtenderProvider))]
    
    [InstalledProductRegistration( true, null, null, null )]

    partial class CandlePackage : IVsInstalledProduct, IVsSolutionEvents
    {
        /// <summary>
        /// 
        /// </summary>
        private UInt32 cookieIVsSolutionEvents;        
        private BuildEvents buildEvents;
        private bool buildSuccess;

        /// <summary>
        /// Initialization method called by the package base class when this package is loaded.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            ServiceLocator.Instance.InitializeForVisualStudio( this );

            try
            {
                this.RegisterEditorFactory( new DataLayerEditorFactory( this ) );
                this.RegisterEditorFactory( new UIWorkflowLayerEditorFactory( this ) );

                this.AddToolWindow( typeof( PortWindowPane ) );
                this.AddToolWindow( typeof( RepositoryWindowPane ) );

                // Chargement des options
                OptionsPage.SetInstance( GetDialogPage( typeof( OptionsPage ) ) );
                OptionsServersPage.SetInstance( GetDialogPage( typeof( OptionsServersPage ) ) );

                // Suppression du répertoire temporaire
                try
                {
                    Utils.RemoveDirectory(Utils.GetCandleTempFolder());
                }
                catch { }

                InitializeSettings();

                if( buildEvents == null )
                {
                    try
                    {
                        buildEvents = ServiceLocator.Instance.IDEHelper.BuildEvents;
                        buildEvents.OnBuildDone += buildEvents_OnBuildDone;
                        buildEvents.OnBuildBegin += buildEvents_OnBuildBegin;
                        buildEvents.OnBuildProjConfigDone += buildEvents_OnBuildProjConfigDone;
                    }
                    catch (Exception ex)
                    {
                        ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                        if (logger != null)
                            logger.Write("Loading Package", ex.Message, LogType.Warning);
                    }
                }

                if (cookieIVsSolutionEvents == 0)
                {
                    IVsSolution solution = (IVsSolution)this.GetService(typeof(IVsSolution));
                    ErrorHandler.ThrowOnFailure(solution.AdviseSolutionEvents(this, out this.cookieIVsSolutionEvents));
                }
//                if( String.IsNullOrEmpty(OptionsPage.Current.LicenseId ))
//                    throw new Exception("License has expired");                        
              
            }
            catch( Exception ex )
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("Candle Package", "Initializing", ex);
#if DEBUG
                MessageBox.Show( ex.Message );
#endif
            }
        }

        /// <summary>
        /// Force l'affichage de la fenêtre d'initialisation de Candle la 1ère fois qu'il s'exécute
        /// </summary>
        private static void InitializeSettings()
        {
            // Initialisation
            if( !OptionsPage.Instance.IsInitialized )
            {
                // Valeurs par defaut
                //string baseDirectory = Path.GetFullPath( Path.Combine( FileLocationHelper.VSInstallDir,
                //                        @"..\..\" + ModelConstants.ApplicationName ) );
                string baseDirectory = @"c:\" + ModelConstants.ApplicationName;
                OptionsPage.Instance.BaseDirectory = baseDirectory;
                OptionsPage.Instance.RepositoryPath = String.Empty;
                OptionsPage.Instance.RepositoryUrl = String.Empty;
                OptionsPage.Instance.RepositoryEnabled = false;

                // Affichage pour la personnalisation (si pb de sauvegarde voir dans le constructeur de OptionsInitForm)
                using( OptionsInitForm f = new OptionsInitForm( OptionsPage.Instance ) )
                {
                    f.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Quand la compil est terminée, on copie tout dans le répertoire du projet de startup
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="action">The action.</param>
        void buildEvents_OnBuildDone( vsBuildScope scope, vsBuildAction action )
        {
            if( !buildSuccess )
                return;

            // Si le fichier modèle n'existe pas, on n'insiste pas
            string modelFileName = ServiceLocator.Instance.ShellHelper.GetSolutionAssociatedModelName();
            if (!File.Exists(modelFileName))
                return;

            // Chargement du modèle
            ModelLoader loader = ModelLoader.GetLoader(modelFileName, true);

            // Si on est pas en phase de compil, on arrete
            if( loader == null || loader.Model == null 
                || loader.Model.Component == null 
                || ( action != vsBuildAction.vsBuildActionBuild && action != vsBuildAction.vsBuildActionRebuildAll ) 
                || scope != vsBuildScope.vsBuildScopeSolution )
                return;

            // Copie des références de runtime dans le répertoire d'execution
            RepositoryManager.CopyToRuntimeFolder( loader.Model );
        }

        /// <summary>
        /// Démarrage du processus de build
        /// </summary>
        /// <param name="Scope">The scope.</param>
        /// <param name="Action">The action.</param>
        void buildEvents_OnBuildBegin( vsBuildScope Scope, vsBuildAction Action )
        {
            buildSuccess=false;
        }

        /// <summary>
        /// A chaque fin de compil d'un projet, on vérifie qu'elle s'est bien terminée
        /// </summary>
        /// <param name="Project">The project.</param>
        /// <param name="ProjectConfig">The project config.</param>
        /// <param name="Platform">The platform.</param>
        /// <param name="SolutionConfig">The solution config.</param>
        /// <param name="Success">if set to <c>true</c> [success].</param>
        void buildEvents_OnBuildProjConfigDone( string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success )
        {
            if( Success )
                buildSuccess = true;
        }

        //private object OnCreateService( IServiceContainer serviceContainer, Type serviceType )
        //{
        //    if( serviceType == typeof( IApplicationModelService ) )
        //    {
        //        return new ApplicationModelService( OptionsPage.Current );
        //    }
        //    return null;
        //}

        /// <summary>
        /// Releases the resources used by the <see cref="T:Microsoft.VisualStudio.Shell.Package"></see> object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed, false if it is being finalized.</param>
        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );

            if( disposing )
            {
                if( buildEvents != null )
                {
                    buildEvents.OnBuildDone -= new _dispBuildEvents_OnBuildDoneEventHandler( buildEvents_OnBuildDone );
                    buildEvents.OnBuildBegin -= new _dispBuildEvents_OnBuildBeginEventHandler( buildEvents_OnBuildBegin );
                    buildEvents.OnBuildProjConfigDone -=new _dispBuildEvents_OnBuildProjConfigDoneEventHandler( buildEvents_OnBuildProjConfigDone );
                    buildEvents = null;
                }

                if (cookieIVsSolutionEvents != 0)
                {
                    IVsSolution solution = (IVsSolution)this.GetService(typeof(IVsSolution));
                    if( solution != null )
                        ErrorHandler.ThrowOnFailure(solution.UnadviseSolutionEvents(this.cookieIVsSolutionEvents));
                    cookieIVsSolutionEvents = 0;
                }
            }
        }

        #region IVsInstalledProduct
        /// <summary>
        /// Ids the BMP splash.
        /// </summary>
        /// <param name="pIdBmp">The p id BMP.</param>
        /// <returns></returns>
        public int IdBmpSplash(out uint pIdBmp)
        {
            pIdBmp = 201;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        /// <summary>
        /// Officials the name.
        /// </summary>
        /// <param name="pbstrName">Name of the PBSTR.</param>
        /// <returns></returns>
        public int OfficialName(out string pbstrName)
        {
            pbstrName = ModelConstants.SoftwareName;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        /// <summary>
        /// Products the ID.
        /// </summary>
        /// <param name="pbstrPID">The PBSTR PID.</param>
        /// <returns></returns>
        public int ProductID(out string pbstrPID)
        {
            Assembly assembly1 = Assembly.GetExecutingAssembly();
            pbstrPID = string.Format( "(build #{0})", assembly1.GetName().Version.ToString(3) );
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        /// <summary>
        /// Products the details.
        /// </summary>
        /// <param name="pbstrProductDetails">The PBSTR product details.</param>
        /// <returns></returns>
        public int ProductDetails(out string pbstrProductDetails)
        {
            pbstrProductDetails = "Alain Metge 2007 (www.dslfactory.org) - Credits:Philipe Lacoste";
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        /// <summary>
        /// Ids the ico logo for aboutbox.
        /// </summary>
        /// <param name="pIdIco">The p id ico.</param>
        /// <returns></returns>
        public int IdIcoLogoForAboutbox(out uint pIdIco)
        {
            pIdIco = 201;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }
        #endregion

        #region IVsSolutionEvents Members

        /// <summary>
        /// Called when [after close solution].
        /// </summary>
        /// <param name="pUnkReserved">The p unk reserved.</param>
        /// <returns></returns>
        public int OnAfterCloseSolution(object pUnkReserved)
        {
            // PB quand on change de solution mais qu'on reste dans VS
          //  Mapper.Instance.Dispose(true); 
            return 0;
        }

        /// <summary>
        /// Called when [after load project].
        /// </summary>
        /// <param name="pStubHierarchy">The p stub hierarchy.</param>
        /// <param name="pRealHierarchy">The p real hierarchy.</param>
        /// <returns></returns>
        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return 0;
        }

        /// <summary>
        /// Called when [after open project].
        /// </summary>
        /// <param name="pHierarchy">The p hierarchy.</param>
        /// <param name="fAdded">The f added.</param>
        /// <returns></returns>
        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            //new Mapper();
            return 0;
        }

        /// <summary>
        /// Called when [after open solution].
        /// </summary>
        /// <param name="pUnkReserved">The p unk reserved.</param>
        /// <param name="fNewSolution">The f new solution.</param>
        /// <returns></returns>
        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
            if (notifier != null)
            {
                notifier.NotifySolutionOpened(this);
            }
            return 0;
        }

        /// <summary>
        /// Called when [before close project].
        /// </summary>
        /// <param name="pHierarchy">The p hierarchy.</param>
        /// <param name="fRemoved">The f removed.</param>
        /// <returns></returns>
        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return 0;
        }

        /// <summary>
        /// Called when [before close solution].
        /// </summary>
        /// <param name="pUnkReserved">The p unk reserved.</param>
        /// <returns></returns>
        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
            if (notifier != null)
            {
                notifier.NotifySolutionClosed(this);
            } 
            return 0;
        }

        /// <summary>
        /// Called when [before unload project].
        /// </summary>
        /// <param name="pRealHierarchy">The p real hierarchy.</param>
        /// <param name="pStubHierarchy">The p stub hierarchy.</param>
        /// <returns></returns>
        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query close project].
        /// </summary>
        /// <param name="pHierarchy">The p hierarchy.</param>
        /// <param name="fRemoving">The f removing.</param>
        /// <param name="pfCancel">The pf cancel.</param>
        /// <returns></returns>
        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query close solution].
        /// </summary>
        /// <param name="pUnkReserved">The p unk reserved.</param>
        /// <param name="pfCancel">The pf cancel.</param>
        /// <returns></returns>
        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query unload project].
        /// </summary>
        /// <param name="pRealHierarchy">The p real hierarchy.</param>
        /// <param name="pfCancel">The pf cancel.</param>
        /// <returns></returns>
        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return 0;
        }

        #endregion
    }
}
