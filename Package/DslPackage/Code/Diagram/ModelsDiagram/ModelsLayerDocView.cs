
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
    internal class DataLayerDocView : DslShell::SingleDiagramDocView
    {
        /// <summary>
        /// Constructs a new SystemModelDocView.
        /// </summary>
        public DataLayerDocView( DslShell::ModelingDocData docData, global::System.IServiceProvider serviceProvider )
            : base( docData, serviceProvider )
        {
        }

        /// <summary>
        /// Called to initialize the view after the corresponding document has been loaded.
        /// </summary>
        protected override bool LoadView()
        {
            base.LoadView();

            ReadOnlyCollection<DataLayer> models = this.DocData.Store.ElementDirectory.FindElements<global::DSLFactory.Candle.SystemModel.DataLayer>();
            if( models.Count == 0 )
                return false;

            DataLayer model = models[0];

            global::System.Diagnostics.Debug.Assert( this.DocData.RootElement!=null );
            if( this.DocData.RootElement == null )
            {
                return false;
            }

            // Penser à rajouter le diagramme dans GetCustomDomainModelTypes de SystemModel
            global::System.Collections.ObjectModel.ReadOnlyCollection<global::DSLFactory.Candle.SystemModel.DataLayerDiagram> diagrams = this.DocData.Store.ElementDirectory.FindElements<global::DSLFactory.Candle.SystemModel.DataLayerDiagram>();
            if( diagrams.Count > 0 )
            {
                //global::System.Diagnostics.Debug.Assert( diagrams.Count == 1, "Found more than one diagram, using the first one found." );
                this.Diagram = (DslDiagrams::Diagram)diagrams[0];
            }
            else
            {
                // Sinon on le crée
                DataLayerDiagram diagram;
                using( Transaction transaction = this.DocData.Store.TransactionManager.BeginTransaction( "Load models view" ) )
                {
                    diagram = new DataLayerDiagram( DocData.Store );
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

        private void SynchronizeDiagram( DataLayer model )
        {
            bool isDirty = false;
            using( Transaction transaction = model.Store.TransactionManager.BeginTransaction( "Synchro models" ) )
            {
                foreach( Package package in model.Packages )
                {
                    PackageShape packageShape=null;
                    foreach( ShapeElement shape in base.Diagram.NestedChildShapes )
                    {
                        if( shape.ModelElement.Id == package.Id )
                        {
                            packageShape = (PackageShape)shape;
                            break;
                        }
                    }

                    if( packageShape==null )
                    {
                        packageShape = new PackageShape( model.Store );
                        packageShape.Associate( package );
                        base.Diagram.NestedChildShapes.Add( packageShape );
                        isDirty = true;
                    }

                    foreach( DataType clazz in package.Types )
                    {
                        NodeShape classShape=null;
                        foreach( NodeShape shape in packageShape.NestedChildShapes )
                        {
                            if( shape.ModelElement.Id == clazz.Id )
                            {
                                classShape=shape;
                                break;
                            }
                        }
                        if( classShape == null )
                        {
                            if( clazz is Enumeration )
                                classShape = new EnumTypeShape( model.Store );
                            else
                                classShape = new EntityShape( model.Store );

                            classShape.Associate( clazz );
                            packageShape.NestedChildShapes.Add( classShape );
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
                return "DataLayerDiagram";
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

