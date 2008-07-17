using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Commands;
using DSLFactory.Candle.SystemModel.Repository;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using DSLFactory.Candle.SystemModel.Designer;
using Microsoft.VisualStudio.Shell.Interop;
using DSLFactory.Candle.SystemModel.Configuration;
using Microsoft.VisualStudio.Modeling;
using DSLFactory.Candle.SystemModel.Strategies;
using System.Collections;

namespace DSLFactory.Candle.SystemModel
{
    partial class CandleCommandSet
    {
        #region Constantes
        // TIPS ajout de menu
        // Ajouter dans customcmd.ctc aprés GENERATED_BUTTONS
        // 		  guidCmdSet:0x1000, guidMenu:grpidContextMain, 0x0200, OI_NOID, BUTTON, DIS_DEF, "Execute All";
        // 0x1000 correspond à la valeur de la constante ci-dessous
        // On prendra des valeurs > 0x1000
        // Ne pas oublier d'incrémenter ProvideMenuResource dans Package.cs

        private const int cmdIDImplementAllT = 0x1600; // Dans la toolbox
        private const int cmdIDImplementAllG = 0x1500; // Dans le menu principal
        private const int cmdIDImplementAll = 0x1000;
        private const int cmdIDImplement    = 0x1001;
        private const int cmdIDImportModels = 0x1002;
        private const int cmdIDArrangeShape = 0x1003;
        private const int cmdIDPropertyToAssociation = 0x1004;
        private const int cmdIDAssociationToProperty = 0x1005;
        private const int cmdIDExportDiagramAsBitmap = 0x1006;

        private const int cmdIDPublishModelT = 0x1607; // Dans la toolbox
        private const int cmdIDPublishModelG = 0x1507; // Dans le menu principal
        private const int cmdIDPublishModel = 0x1007;

        private const int cmdIDImportAssemblies = 0x1008;
        private const int cmdIDImportInterfaces = 0x1022;
        private const int cmdIDModelAffectation = 0x1009;
        private const int cmdIDShowStrategies   = 0x1010;
        private const int cmdIDManageArtifacts  = 0x1011;

        private const int cmdIDShowDataLayer = 0x1020;
        
        private const int cmdIDPublishAsTemplate = 0x1021;
        private const int cmdIDGetLastVersion = 0x1023;
        private const int cmdIDImportWSDL = 0x1024;
        private const int cmdIDShowProperties = 0x1025;
        private const int cmdIDManageConfigurations = 0x1026;
        private const int cmdIDImportXmi = 0x1027;
        private const int cmdIDPropagatesOperations = 0x1028; 
        private const int cmdIDShowDocumentation = 0x1029;
        private const int cmdIDShowRelationShips = 0x102A;
        private const int cmdIDUpdateAssembly = 0x102B;

        private const int cmdIDZoomIn = 0x1200;
        private const int cmdIDZoomOut = 0x1201;
  
        private const int cmdIDCopy             = 0x1100;
        private const int cmdIDPaste            = 0x1101;

        private const int cmdIDShowDependencies = 0x2000;
        private const int cmdIDShowRepositoryToolWindow = 0x3000;

        private const int cmdIDShowModel = 0x9100; 
        #endregion

        #region Initialisation
        /// <summary>
        /// Provide the menu commands that this command set handles
        /// </summary>
        /// <returns></returns>
        protected override IList<MenuCommand> GetMenuCommands()
        {
            IList<MenuCommand> commandList = base.GetMenuCommands();

            // Ajout d'une ligne dans le menu show other windows pour afficher le repository
            MenuCommand menuCommand = new CommandContextBoundMenuCommand( this.ServiceProvider,
                new EventHandler( OnShowRepositoryToolWindow ),
                new CommandID( new global::System.Guid( Constants.CandleCommandSetId ), cmdIDShowRepositoryToolWindow ),
                typeof( CandleEditorFactory ).GUID );
            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusShowModel ),
                new EventHandler( OnShowModel ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDShowModel ) );
            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusShowStrategies ),
                new EventHandler( OnShowStrategies ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDShowStrategies ) );
            commandList.Add( menuCommand );


            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusImplementAll ),
                new EventHandler( OnImplementAll ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDImplementAll ) );
            commandList.Add( menuCommand );
            
            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusImplementAll),
                new EventHandler(OnImplementAll),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDImplementAllG));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusImplementAll),
                new EventHandler(OnImplementAll),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDImplementAllT));
            commandList.Add(menuCommand);


            menuCommand = new DynamicStatusMenuCommand(
               new EventHandler( OnStatusImplement ),
               new EventHandler( OnImplement ),
               new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDImplement ) );

            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusImportModels ),
                new EventHandler( OnImportModels ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDImportModels ) );

            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusImportInterfaces),
                new EventHandler(OnImportInterfaces),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDImportInterfaces));

            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusArrangeShapes ),
                new EventHandler( OnArrangeShapes ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDArrangeShape ) );

            commandList.Add( menuCommand );

            //menuCommand = new DynamicStatusMenuCommand(
            //    new EventHandler(OnStatusShowDataLayer),
            //     new EventHandler(OnShowDataLayer),
            //      new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDShowDataLayer));

            //commandList.Add(menuCommand);

            //menuCommand = new DynamicStatusMenuCommand(
            //    new EventHandler( OnStatusPropertyToAssociation ),
            //    new EventHandler( OnPropertyToAssociation ),
            //    new CommandID( new Guid(Constants.CandleCommandSetId), cmdIDPropertyToAssociation ) );

            //commandList.Add( menuCommand );

            //menuCommand = new DynamicStatusMenuCommand(
            //    new EventHandler( OnStatusAssociationToProperty ),
            //    new EventHandler( OnAssociationToProperty ),
            //    new CommandID( new Guid(Constants.CandleCommandSetId), cmdIDAssociationToProperty ) );

            //commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusExportDiagramAsBitmap ),
                new EventHandler( OnExportDiagramAsBitmap ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDExportDiagramAsBitmap ) );

            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusPublishModel ),
                new EventHandler( OnPublishModel ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDPublishModel ) );

            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusPublishModel),
                new EventHandler(OnPublishModel),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDPublishModelG));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusPublishModel),
                new EventHandler(OnPublishModel),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDPublishModelT));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusPublishAsTemplate),
                new EventHandler(OnPublishAsTemplate),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDPublishAsTemplate));

            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusImportAssemblies ),
                new EventHandler( OnImportAssemblies ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDImportAssemblies ) );

            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusShowDependencies ),
                new EventHandler( OnShowDependencies ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDShowDependencies ) );

            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusModelAffectation ),
                new EventHandler( OnModelAffectation ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDModelAffectation ) );

            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler( OnStatusManageArtifacts ),
                new EventHandler( OnManageArtifacts ),
                new CommandID( new Guid( Constants.CandleCommandSetId ), cmdIDManageArtifacts ) );
            commandList.Add( menuCommand );

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusManageConfiguration),
                new EventHandler(OnManageConfiguration),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDManageConfigurations));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusImportWSDL),
                new EventHandler(OnImportWSDL),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDImportWSDL));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusCopy),
                new EventHandler(OnCopy),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDCopy));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusPaste),
                new EventHandler(OnPaste),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDPaste));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusGetLastVersion),
                new EventHandler(OnGetLastVersion),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDGetLastVersion));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusShowProperties),
                new EventHandler(OnShowProperties),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDShowProperties));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusImportXmi),
                new EventHandler(OnImportXmi),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDImportXmi));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusPropagateOperations),
                new EventHandler(OnPropagateOperations),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDPropagatesOperations));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusShowDocumentation),
                new EventHandler(OnShowDocumentation),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDShowDocumentation));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusShowRelationShips),
                new EventHandler(OnShowRelationShips),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDShowRelationShips));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusZoomIn),
                new EventHandler(OnZoomIn),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDZoomIn));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(
                new EventHandler(OnStatusZoomOut),
                new EventHandler(OnZoomOut),
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDZoomOut));
            commandList.Add(menuCommand);

            menuCommand = new DynamicStatusMenuCommand(OnStatusUpdateDotNetAssembly,
                OnUpdateDotNetAssembly,
                new CommandID(new Guid(Constants.CandleCommandSetId), cmdIDUpdateAssembly));
            commandList.Add(menuCommand);
            //// Copy/Paste
            //AddPasteCommand(commandList);
            //AddCopyCommand(commandList);

            return commandList;
        } 
        #endregion

        #region Copy
        ///// <summary>
        ///// Add the support for the standard Copy Command
        ///// </summary>
        ///// <param name="commands">CommandList, we want to add the Copy command to</param>
        ///// <typeparam name="T">A ModelElement derived type which is used to restrict the kind of model element
        ///// that can be copied to the clipboard</typeparam>
        //public void AddCopyCommand(IList<MenuCommand> commands) 
        //{
        //    commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusCopy),
        //                                              new EventHandler(OnMenuCopy),
        //                                              StandardCommands.Copy));
        //}

        /// <summary>
        /// Status for the Copy command
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnStatusCopy(object sender, EventArgs e)
        {
            MenuCommand command = sender as MenuCommand;
            Diagram diagram = null;
            if (CurrentDocView != null)
                diagram = this.CurrentDocView.CurrentDiagram;
            ICollection selection = this.CurrentSelection;

            CopyCommand copy = new CopyCommand(diagram.Store, selection);
            command.Enabled = copy.Enabled;
            command.Visible = copy.Visible();
        }

        /// <summary>
        /// Copy command
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCopy(object sender, EventArgs e)
        {
            Diagram diagram = this.CurrentDocView.CurrentDiagram;
            ICollection selection = this.CurrentSelection;

            CopyCommand copy = new CopyCommand(diagram.Store, selection);
            copy.Exec();

        }

        #endregion

        #region Paste
        ///// <summary>
        ///// Add a command to Paste the content of model elements from the clipboard.
        ///// </summary>
        ///// <param name="commands">List of command to add the paste command to.</param>
        //public void AddPasteCommand(IList<MenuCommand> commands)
        //{
        //    commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusPaste),
        //                                              new EventHandler(OnMenuPaste),
        //                                              StandardCommands.Paste));
        //}

        /// <summary>
        /// Status for the Paste Command
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnStatusPaste(object sender, EventArgs e)
        {
            bool flag = false;
            MenuCommand command = sender as MenuCommand;
            Diagram diagram = null;
            if( CurrentDocView != null)
                diagram = this.CurrentDocView.CurrentDiagram;
            if (diagram != null && this.SingleSelection != null)
            {
                System.Windows.Forms.IDataObject data = System.Windows.Forms.Clipboard.GetDataObject();
                if( data != null )
                {
                    DesignSurfaceElementOperations op = diagram.ElementOperations;
                    flag = op.CanMerge(((PresentationElement)this.SingleSelection).ModelElement, data);
                }
            }
            command.Visible = command.Enabled = flag;
        }

        /// <summary>
        /// Paste command processing
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnPaste(object sender, EventArgs e)
        {
            Diagram diagram = this.CurrentDocView.CurrentDiagram;
            if (diagram == null && this.SingleSelection != null) return;

            System.Windows.Forms.IDataObject data =
                            System.Windows.Forms.Clipboard.GetDataObject();
            DesignSurfaceElementOperations op = diagram.ElementOperations;
            if (op.CanMerge(((PresentationElement)this.SingleSelection).ModelElement, data))
            {
                // Find a suitable place to position the new shape.
                PointD place = new PointD(0, 0);
                foreach (object item in this.CurrentSelection)
                {
                    ShapeElement shape = item as ShapeElement;
                    if (shape != null)
                    {
                        place = shape.AbsoluteBoundingBox.Center;
                        break;
                    }
                }
                using (Transaction t = diagram.Store.TransactionManager.BeginTransaction("paste"))
                {
                    // Do the business.
                    op.Merge((PresentationElement)this.SingleSelection, data, PointD.ToPointF(place));
                    t.Commit();
                    System.Windows.Forms.Clipboard.Clear();
                }
            }
        }
        #endregion

        #region Update dotnet assembly
        protected void OnStatusUpdateDotNetAssembly(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            if (SingleSelection != null)
            {
                UpdateDotNetAssembly command = new UpdateDotNetAssembly(SingleSelection);
                cmd.Visible = command.Visible();
                cmd.Enabled = command.Enabled;
                return;
            }
            cmd.Enabled = cmd.Visible = false;
        }

        protected void OnUpdateDotNetAssembly(object sender, EventArgs e)
        {
            UpdateDotNetAssembly command = new UpdateDotNetAssembly(SingleSelection);
            command.Exec();
        }
        #endregion


        #region Zoom in
        protected void OnStatusZoomIn(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            VSDiagramView designer = this.CurrentDocView != null && this.CurrentDocView.CurrentDesigner != null ? this.CurrentDocView.CurrentDesigner : null;
            cmd.Enabled = cmd.Visible = designer != null && designer.ZoomFactor < designer.DiagramClientView.MaximumZoom;
        }

        protected void OnZoomIn(object sender, EventArgs e)
        {
            base.CurrentCandleDocView.CurrentDesigner.ZoomIn();
        }
        #endregion

        #region Zoom Out
        protected void OnStatusZoomOut(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            VSDiagramView designer = this.CurrentDocView != null && this.CurrentDocView.CurrentDesigner != null ? this.CurrentDocView.CurrentDesigner : null;
            cmd.Enabled = cmd.Visible = designer != null && designer.ZoomFactor > designer.DiagramClientView.MinimumZoom;
        }

        protected void OnZoomOut(object sender, EventArgs e)
        {
            base.CurrentCandleDocView.CurrentDesigner.ZoomOut();
        }
        #endregion

        #region Show Relationships
        protected void OnStatusShowRelationShips(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = base.CurrentCandleDocView != null && base.CurrentCandleDocView.CurrentDiagram != null;
            if( cmd.Enabled )
                cmd.Checked = ((ComponentModelDiagram)base.CurrentCandleDocView.CurrentDiagram).ShowRelationShips;
        }

        protected void OnShowRelationShips(object sender, EventArgs e)
        {
            if ((base.CurrentCandleDocView != null) && (base.CurrentCandleDocView.CurrentDiagram != null))
            {
                ComponentModelDiagram currentDiagram = base.CurrentCandleDocView.CurrentDiagram as ComponentModelDiagram;
                if (currentDiagram != null)
                {
                    currentDiagram.ShowRelationShips = !currentDiagram.ShowRelationShips;
                }
            }
        }
        #endregion

        #region Manage Configurations
        protected void OnStatusManageConfiguration(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            ManageConfigurationCommand command = new ManageConfigurationCommand(this.SingleSelection);
            cmd.Enabled = command.Enabled;
            cmd.Visible = command.Visible();
        }

        protected void OnManageConfiguration(object sender, EventArgs e)
        {
            ManageConfigurationCommand command = new ManageConfigurationCommand(this.SingleSelection);
            command.Exec();
        }
        #endregion

        #region Manage Artifacts
        protected void OnStatusManageArtifacts( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;

            ManageArtifactsCommand command = new ManageArtifactsCommand( this.SingleSelection );
            cmd.Enabled = cmd.Visible = command.Visible();
        }

        protected void OnManageArtifacts( object sender, EventArgs e )
        {
            ManageArtifactsCommand command = new ManageArtifactsCommand( this.SingleSelection );
            command.Exec();
        } 
        #endregion

        #region Show properties
        protected void OnStatusShowProperties(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            if (this.SingleSelection != null)
            {
                ShowPropertiesCommand command = new ShowPropertiesCommand(SingleSelection);
                cmd.Visible = command.Visible();
                cmd.Enabled = command.Enabled;
                return;
            }
            cmd.Enabled = cmd.Visible = false;
        }

        protected void OnShowProperties(object sender, EventArgs e)
        {
            ShowPropertiesCommand command = new ShowPropertiesCommand(SingleSelection);
            command.Exec();
        }
        #endregion

        #region Get Last Version
        protected void OnStatusGetLastVersion(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            if (this.CurrentDocView.PrimarySelection != null)
            {
                GetLastVersionCommand command = new GetLastVersionCommand(SingleSelection);
                cmd.Visible = command.Visible();
                cmd.Enabled = command.Enabled;
                return;
            }
            cmd.Enabled = cmd.Visible = false;
        }

        protected void OnGetLastVersion(object sender, EventArgs e)
        {
            GetLastVersionCommand command = new GetLastVersionCommand(SingleSelection);
            command.Exec();
        }
        #endregion

        #region Show strategies
        protected void OnStatusShowStrategies( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            if( this.CurrentDocView.PrimarySelection != null )
            {
                ShowStrategiesCommand command = new ShowStrategiesCommand(CandleModel.GetInstance(this.CurrentCandleDocData.Store).SoftwareComponent, this.CurrentDocView.PrimarySelection, this.CurrentDocData.FileName);
                cmd.Visible = command.Visible();
                cmd.Enabled = command.Enabled;                
                return;
            }
            cmd.Enabled = cmd.Visible = false;
        }

        protected void OnShowStrategies( object sender, EventArgs e )
        {
            ShowStrategiesCommand command = new ShowStrategiesCommand(CandleModel.GetInstance(this.CurrentCandleDocData.Store).SoftwareComponent, this.CurrentDocView.PrimarySelection, this.CurrentDocData.FileName);
            command.Exec();
        } 
        #endregion

        #region Show Models layer
        protected void OnStatusShowDataLayer(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            if (this.CurrentCandleDocView != null)
            {
                ShowDataLayerCommand command = new ShowDataLayerCommand(this.ServiceProvider, this.CurrentDocView.CurrentDiagram.ModelElement, this.CurrentDocView.CurrentDiagram);
                cmd.Enabled = cmd.Visible = command.Visible();
            }
            else
                cmd.Enabled = cmd.Visible = false;
        }

        protected void OnShowDataLayer(object sender, EventArgs e)
        {
            ShowDataLayerCommand command = new ShowDataLayerCommand(this.ServiceProvider, this.CurrentDocView.CurrentDiagram.ModelElement, this.CurrentDocView.CurrentDiagram);
            command.Exec();
        }
        #endregion

        #region Show Model
        protected void OnStatusShowModel( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = true;
        }

        protected void OnShowModel( object sender, EventArgs e )
        {
            ShowModelCommand command = new ShowModelCommand( this.ServiceProvider );
            command.Exec();
        } 
        #endregion

        #region Show repository window
        protected void OnShowRepositoryToolWindow( object sender, EventArgs e )
        {
            RepositoryWindowPane wnd = RepositoryWindowPane;
            if( wnd != null )
                wnd.Show();
        }

        /// <summary>
        /// Returns the repository explorer tool window.
        /// </summary>
        protected RepositoryWindowPane RepositoryWindowPane
        {
            get
            {
                RepositoryWindowPane repositoryExplorer = null;
                ModelingPackage package = this.ServiceProvider.GetService( typeof( CandlePackage ) ) as ModelingPackage;

                if( package != null )
                {
                    repositoryExplorer = package.GetToolWindow( typeof( RepositoryWindowPane ), true ) as RepositoryWindowPane;
                }

                return repositoryExplorer;
            }
        }

        #endregion

        #region Copy
        //internal void OnCopy( object sender, EventArgs e )
        //{
        //    if( this.CurrentSelection.Count > 0 )
        //    {
        //        IDataObject data = new DataObject();
        //        Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram = this.CurrentDocView.CurrentDiagram;
        //        ICollection<Microsoft.VisualStudio.Modeling.ModelElement> elements = new List<Microsoft.VisualStudio.Modeling.ModelElement>();

        //        foreach( object elem in CurrentSelection )
        //        {
        //            if( elem is EnumTypeShape || elem is EntityShape )
        //            {
        //                elements.Add( ( (PresentationElement)elem ).ModelElement );
        //            }
        //        }
        //        diagram.ElementOperations.Copy( data, elements );
        //        Clipboard.SetDataObject( data, false, 100, 50 );
        //    }
        //}

        //internal void OnStatusCopy( object sender, EventArgs e )
        //{
        //    bool flag=false;

        //    if( this.CurrentSelection.Count > 0 )
        //    {
        //        IDataObject data = new DataObject();
        //        Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram = this.CurrentDocView.CurrentDiagram;

        //        foreach( object elem in this.CurrentSelection )
        //        {
        //            if( elem is EnumTypeShape || elem is EntityShape )
        //            {
        //                flag=true;
        //                break;
        //            }
        //        }
        //    }

        //    MenuCommand cmd = sender as MenuCommand;
        //    cmd.Enabled = cmd.Visible = flag;
        //}

        #endregion

        #region Paste
        //internal void OnStatusPaste( object sender, EventArgs e )
        //{
        //    bool flag = false;
        //    if( this.SingleSelection is PackageShape )
        //    {
        //        IDataObject data = Clipboard.GetDataObject();
        //        Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram = this.CurrentDocView.CurrentDiagram;
        //        flag = diagram.ElementOperations.CanMerge( ( (PackageShape)this.SingleSelection ).ModelElement, data );
        //    }
        //    MenuCommand cmd = sender as MenuCommand;
        //    cmd.Enabled = cmd.Visible = flag;
        //}

        //internal void OnPaste( object sender, EventArgs e )
        //{
        //    if( this.SingleSelection is PresentationElement )
        //    {
        //        PresentationElement shape = this.SingleSelection as PresentationElement;
        //        IDataObject data = Clipboard.GetDataObject();
        //        Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram = this.CurrentDocView.CurrentDiagram;

        //        if( diagram.ElementOperations.CanMerge( shape.ModelElement, data ) )
        //        {
        //            using( Transaction transaction = shape.Store.TransactionManager.BeginTransaction( "Paste" ) )
        //            {
        //                diagram.ElementOperations.Merge( shape.ModelElement, data );
        //                transaction.Commit();
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Show dependencies
        protected void OnStatusShowDependencies( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new ShowDependenciesCommand( SingleSelection, CurrentCandleDocData.FileName ).Visible();
        }

        protected void OnShowDependencies( object sender, EventArgs e )
        {
            ShowDependenciesCommand command = new ShowDependenciesCommand( SingleSelection, CurrentCandleDocData.FileName );
            command.Exec();

            //XmiImporter xi = new XmiImporter(model);
            //xi.Import();
        } 
        #endregion

        #region External model affectation
        protected void OnStatusModelAffectation( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new ModelAffectationCommand( SingleSelection ).Visible();
        }

        protected void OnModelAffectation( object sender, EventArgs e )
        {
            ModelAffectationCommand command = new ModelAffectationCommand( SingleSelection );
            command.Exec();
        }

        #endregion

        #region Export diagram as bitmap
        protected void OnStatusExportDiagramAsBitmap( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new ExportDiagramAsBitmapCommand( ServiceProvider, SingleSelection ).Visible();
        }

        protected void OnExportDiagramAsBitmap( object sender, EventArgs e )
        {
            ExportDiagramAsBitmapCommand command = new ExportDiagramAsBitmapCommand( ServiceProvider, SingleSelection );
            command.Exec();
            
            //XmiImporter xi = new XmiImporter(model);
            //xi.Import();
        } 
        #endregion

        #region Import assemblies
        protected void OnStatusImportAssemblies( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new ImportAssemblyCommand( SingleSelection ).Visible();
        }

        protected void OnImportAssemblies(object sender, EventArgs e)
        {
            ImportAssemblyCommand command = new ImportAssemblyCommand( SingleSelection );
            command.Exec();
        } 
        #endregion

        #region Import interfaces
        protected void OnStatusImportInterfaces(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new ImportInterfaceCommand(SingleSelection).Visible();
        }

        protected void OnImportInterfaces(object sender, EventArgs e)
        {
            ImportInterfaceCommand command = new ImportInterfaceCommand(SingleSelection);
            command.Exec();
        }
        #endregion

        #region Import wsdl
        protected void OnStatusImportWSDL(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            ImportWsdlCommand command = new ImportWsdlCommand(this.ServiceProvider, SingleSelection);
            cmd.Visible = command.Visible();
            cmd.Enabled = command.Enabled;
        }

        protected void OnImportWSDL(object sender, EventArgs e)
        {
            ImportWsdlCommand command = new ImportWsdlCommand(this.ServiceProvider, SingleSelection);
            command.Exec();

            //XmiImporter xi = new XmiImporter(model);
            //xi.Import();
        }
        #endregion

        #region Import models
        protected void OnStatusImportXmi(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new ImportXmiCommand(SingleSelection).Visible();
        }

        protected void OnImportXmi(object sender, EventArgs e)
        {
            ImportXmiCommand command = new ImportXmiCommand(SingleSelection);
            command.Exec();
        }
        #endregion

        #region Import models
        protected void OnStatusImportModels( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new ImportModelCommand( SingleSelection ).Visible();
        }

        protected void OnImportModels( object sender, EventArgs e )
        {
            ImportModelCommand command = new ImportModelCommand( SingleSelection );
            command.Exec();

            //XmiImporter xi = new XmiImporter(model);
            //xi.Import();
        } 
        #endregion

        #region Publish
        protected void OnStatusPublishModel( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            PublishModelCommand command = SingleSelection == null || CurrentDocData== null ? new PublishModelCommand(this.ServiceProvider) : new PublishModelCommand(SingleSelection, CurrentDocData.FileName);
            cmd.Enabled = cmd.Visible = command.Visible();
        }

        protected void OnPublishModel( object sender, EventArgs e )
        {
            PublishModelCommand command = SingleSelection == null || CurrentDocData == null ? new PublishModelCommand(this.ServiceProvider) : new PublishModelCommand(SingleSelection, CurrentDocData.FileName);
            command.Exec();
        }

        #endregion

        #region PublishAsTemplate
        protected void OnStatusPublishAsTemplate(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            PublishAsTemplateCommand command = new PublishAsTemplateCommand(SingleSelection, CurrentDocData.FileName);
            cmd.Enabled = cmd.Visible = command.Visible();
        }

        protected void OnPublishAsTemplate(object sender, EventArgs e)
        {
            PublishAsTemplateCommand cmd = new PublishAsTemplateCommand(SingleSelection, CurrentDocData.FileName);
            cmd.Exec();
        }

        #endregion

        #region Implement All
        protected void OnStatusImplementAll( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            GenerateAllCommand command = new GenerateAllCommand(this.ServiceProvider, CurrentDocData != null ? CurrentDocData.FileName : null);
            cmd.Enabled = cmd.Visible = command.Visible();
        }

        protected void OnImplementAll( object sender, EventArgs e )
        {
            GenerateAllCommand command = new GenerateAllCommand(this.ServiceProvider, CurrentDocData != null ? CurrentDocData.FileName : null);
            command.Exec();
        }
        #endregion

        #region Implement
        protected void OnStatusImplement( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            NodeShape shape = SingleSelection as NodeShape;
            bool flag = shape != null && ( shape.ModelElement is CandleElement ) && CandleModel.GetInstance(shape.ModelElement.Store).SoftwareComponent != null;
            cmd.Enabled = cmd.Visible = flag && 
                StrategyManager.GetInstance(shape.ModelElement.Store).GetStrategies(((CandleElement)shape.ModelElement ).StrategiesOwner, true).Count > 0;
        }

        protected void OnImplement( object sender, EventArgs e )
        {
            NodeShape shape = SingleSelection as NodeShape;
            Generator.Generate( this.ServiceProvider, CurrentDocData.FileName, shape.ModelElement as ICustomizableElement);
        }        
        #endregion

        #region Arrange shapes
        protected void OnStatusArrangeShapes( object sender, EventArgs e )
        {
            NodeShape shape = SingleSelection as NodeShape;
            ArrangeShapesCommand command = new ArrangeShapesCommand( shape );
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = command.Visible();
        }

        protected void OnArrangeShapes( object sender, EventArgs e )
        {
            NodeShape shape = SingleSelection as NodeShape;
            ArrangeShapesCommand command = new ArrangeShapesCommand( shape );
            command.Exec();
        } 
        #endregion

        #region PropagateOperations
        protected void OnStatusPropagateOperations(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            PropagatesOperationsCommand command = new PropagatesOperationsCommand(SingleSelection);
            cmd.Enabled = command.Enabled;
            cmd.Visible = command.Visible();
        }

        protected void OnPropagateOperations(object sender, EventArgs e)
        {
            PropagatesOperationsCommand cmd = new PropagatesOperationsCommand(SingleSelection);
            cmd.Exec();
        }
        #endregion

        #region ShowDocumentation
        protected void OnStatusShowDocumentation(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;
            ShowDocumentationCommand command = new ShowDocumentationCommand(SingleSelection);
            cmd.Enabled = command.Enabled;
            cmd.Visible = command.Visible();
        }

        protected void OnShowDocumentation(object sender, EventArgs e)
        {
            ShowDocumentationCommand cmd = new ShowDocumentationCommand(SingleSelection);
            cmd.Exec();
        }
        #endregion

        #region obsolete
        protected void OnStatusPropertyToAssociation( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new AttributeAsAssociationCommand( this.ServiceProvider, SingleSelection ).Visible();
        }

        protected void OnPropertyToAssociation( object sender, EventArgs e )
        {
            AttributeAsAssociationCommand command = new AttributeAsAssociationCommand( this.ServiceProvider, SingleSelection );
            command.Exec();
        }

        protected void OnStatusAssociationToProperty( object sender, EventArgs e )
        {
            MenuCommand cmd = sender as MenuCommand;
            cmd.Enabled = cmd.Visible = new AssociationAsAttributeCommand( this.ServiceProvider, SingleSelection ).Visible();
        }

        protected void OnAssociationToProperty( object sender, EventArgs e )
        {
            AssociationAsAttributeCommand command = new AssociationAsAttributeCommand( this.ServiceProvider, SingleSelection );
            command.Exec();
        } 
        #endregion
    }
}
