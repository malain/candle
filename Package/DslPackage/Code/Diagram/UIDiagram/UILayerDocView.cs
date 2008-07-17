
using DslModeling=Microsoft.VisualStudio.Modeling;
using DslDiagrams=Microsoft.VisualStudio.Modeling.Diagrams;
using DslShell=Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Class that hosts the diagram surface in the Visual Studio document area.
    /// </summary>
    internal class UIWorkflowLayerDocView : DslShell::SingleDiagramDocView
    {
        /// <summary>
        /// Constructs a new SystemModelDocView.
        /// </summary>
        public UIWorkflowLayerDocView( DslShell::ModelingDocData docData, global::System.IServiceProvider serviceProvider )
            : base( docData, serviceProvider )
        {
        }

        /// <summary>
        /// Called to initialize the view after the corresponding document has been loaded.
        /// </summary>
        protected override bool LoadView()
        {
            base.LoadView();

            ReadOnlyCollection<UIWorkflowLayer> models = this.DocData.Store.ElementDirectory.FindElements<global::DSLFactory.Candle.SystemModel.UIWorkflowLayer>();
            if( models.Count == 0 )
                return false;

            UIWorkflowLayer model = models[0];

            global::System.Diagnostics.Debug.Assert( this.DocData.RootElement!=null );
            if( this.DocData.RootElement == null )
            {
                return false;
            }

            // Penser à rajouter le diagramme dans GetCustomDomainModelTypes de SystemModel
            global::System.Collections.ObjectModel.ReadOnlyCollection<global::DSLFactory.Candle.SystemModel.UIWorkflowLayerDiagram> diagrams = this.DocData.Store.ElementDirectory.FindElements<global::DSLFactory.Candle.SystemModel.UIWorkflowLayerDiagram>();
            if( diagrams.Count > 0 )
            {
                this.Diagram = (DslDiagrams::Diagram)diagrams[0];
            }
            else
            {
                // Sinon on le crée
                UIWorkflowLayerDiagram diagram;
                using( Transaction transaction = this.DocData.Store.TransactionManager.BeginTransaction( "Load ui view" ) )
                {
                    diagram  = new UIWorkflowLayerDiagram( DocData.Store );
                    diagram.Associate( model );
                    transaction.Commit();
                }

                if( diagram != null )
                {
                    base.Diagram = diagram;
                }
            }
            SynchronizeDiagram( model );

            return base.Diagram != null;
        }

        private void SynchronizeDiagram( UIWorkflowLayer model )
        {
            bool isDirty = false;
            using( Transaction transaction = model.Store.TransactionManager.BeginTransaction( "Synchro diagram" ) )
            {
                foreach( Scenario scenario in model.Scenarios )
                {
                    ScenarioShape scenarioShape=null;
                    foreach( ShapeElement shape in base.Diagram.NestedChildShapes )
                    {
                        if( shape.ModelElement.Id == scenario.Id )
                        {
                            scenarioShape = (ScenarioShape)shape;
                            break;
                        }
                    }

                    if( scenarioShape==null )
                    {
                        scenarioShape = new ScenarioShape( model.Store );
                        scenarioShape.Associate( scenario );
                        base.Diagram.NestedChildShapes.Add( scenarioShape );
                        isDirty = true;
                    }

                    foreach( UIView clazz in scenario.Views)
                    {
                        UiViewShape classShape=null;
                        foreach( ShapeElement shape in scenarioShape.NestedChildShapes )
                        {
                            if( shape.ModelElement.Id == clazz.Id )
                            {
                                classShape=(UiViewShape)shape;
                                break;
                            }
                        }
                        if( classShape == null )
                        {
                            classShape = new UiViewShape( model.Store );
                            classShape.Associate( clazz );
                            scenarioShape.NestedChildShapes.Add( classShape );
                            isDirty = true;
                        }
                    }
                }

                //ArrangeShapesCommand command2 = new ArrangeShapesCommand( base.Diagram );
                //command2.Exec();
                if( isDirty )
                    transaction.Commit();
            }
        }

        /// <summary>
        /// Name of the toolbox tab that should be displayed when the diagram is opened.
        /// </summary>
        protected override string DefaultToolboxTabName
        {
            get
            {
                return "UILayerDiagram";
            }
        }

        /// <summary>
        /// Context menu displayed when the user right-clicks on the design surface.
        /// </summary>
        protected override global::System.ComponentModel.Design.CommandID ContextMenuId
        {
            get
            {
                return Constants.CandleDiagramMenu;
            }
        }
    }
}

