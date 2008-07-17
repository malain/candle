using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Commands;
using DSLFactory.Candle.SystemModel.Repository.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    partial class ComponentModelDiagram : ISupportArrangeShapes
    {
        //private Panel m_scrollPanel;

        //protected override void OnAssociated(DiagramAssociationEventArgs e)
        //{
        //    base.OnAssociated(e);

        //    // Create and show a thumbnail form.
        //    DiagramView diagramView = e.DiagramView;
        //    m_scrollPanel = new Panel();
        //    m_scrollPanel.BackColor = System.Drawing.Color.Gainsboro;
        //    m_scrollPanel.BackgroundImage = DSLFactory.Candle.SystemModel.Properties.Resources.workflow;
        //    m_scrollPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        //    m_scrollPanel.BackgroundImageLayout = ImageLayout.Center;
        //    diagramView.Invoke(new EventHandler(delegate(object sender, EventArgs ev)
        //    {
        //        diagramView.Controls.Add(m_scrollPanel);
        //    }));

        //    m_scrollPanel.MouseDown += delegate(object sender, MouseEventArgs args)
        //    {
        //        Debug.Assert(args != null);
        //        if (args != null && args.Button == MouseButtons.Right)
        //        {
        //            foreach (ShapeElement child in diagramView.Diagram.NestedChildShapes)
        //            {
        //                if (child is ExternalServiceReferenceLink)
        //                {
        //                    child.SetShowHideState(!child.IsVisible);
        //                }
        //            }
        //            diagramView.Diagram.Invalidate(true);
        //            diagramView.DiagramClientView.Invalidate(true);
        //        }

        //    };

        //    m_scrollPanel.Width = diagramView.Controls[1].Width;
        //    m_scrollPanel.Height = diagramView.Controls[2].Height;
        //    m_scrollPanel.Left = 1;//        diagramView.Width - m_scrollPanel.Width;
        //    m_scrollPanel.Top = 1;//         diagramView.Height - m_scrollPanel.Height;
        //    m_scrollPanel.BringToFront();
        //}

        //protected override void InitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
        //{
        //    base.InitializeDecorators(shapeFields, decorators);
        //    this.SnapToGrid = false;
        //}
        private bool _showRelationShips = true;

        /// <summary>
        /// Gets or sets a value indicating whether [show relation ships].
        /// </summary>
        /// <value><c>true</c> if [show relation ships]; otherwise, <c>false</c>.</value>
        public bool ShowRelationShips
        {
            get
            {
                return this._showRelationShips;
            }
            set
            {
                if (this._showRelationShips != value)
                {
                    foreach (ShapeElement element in base.NestedChildShapes)
                    {
                        if (element is ClassUsesOperationLink || element is ExternalServiceReferenceLink )
                        {
                            element.SetShowHideState(value);
                        }
                    }
                    this.Invalidate(true);
                }
                this._showRelationShips = value;
            }
        }

        /// <summary>
        /// Gets the watermark text that is assigned to the diagram.
        /// </summary>
        /// <value></value>
        /// <returns>The watermark text that is assigned to the diagram.</returns>
        public override string WatermarkText
        {
            get
            {
                return "Drag from the toolbox a 'Software Component' or a 'Binary Component'";
            }
        }

        /// <summary>
        /// Gets the bounds rules for the shape.
        /// </summary>
        /// <value></value>
        /// <returns>The bounds rules for the shape.</returns>
        public override BoundsRules BoundsRules
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Drop d'un modèle à partir du repository tree
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragDrop( DiagramDragEventArgs e )
        {
            base.OnDragDrop( e );

            if( e.Data.GetDataPresent( typeof( RepositoryTreeControl.RepositoryDataDragDrop ) ) )
            {
                RepositoryTreeControl.RepositoryDataDragDrop data = (RepositoryTreeControl.RepositoryDataDragDrop)e.Data.GetData( typeof( RepositoryTreeControl.RepositoryDataDragDrop ) );
                if( data != null && this.ModelElement is CandleModel )
                {
                    CandleModel model = ( (CandleModel)this.ModelElement );

                    using( Transaction transaction = model.Store.TransactionManager.BeginTransaction( "Drop a model" ) )
                    {
                        // On ne le met pas si il y est dèjà
                        if (model.Id == data.MetaData.Id)
                        {
                            ServiceLocator.Instance.IDEHelper.ShowMessage("You can not drag the current model.");
                            return;
                        }
                        if( model.FindExternalComponent( data.MetaData.Id ) != null )
                        {
                            ServiceLocator.Instance.IDEHelper.ShowMessage( "This model already exists in the designer." );
                            return;
                        }

                        ExternalComponent system = data.MetaData.CreateComponent(model);
                        transaction.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Alerts listeners when the shape is dragged over its bounds.
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragOver( DiagramDragEventArgs e )
        {
            base.OnDragOver( e );
            if( e.Data.GetDataPresent( typeof( RepositoryTreeControl.RepositoryDataDragDrop ) )
                && this.ModelElement is CandleModel && ((CandleModel)this.ModelElement).Component != null )
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
        }

        //#region Mouse events for dragscroll powertoys
        //private DSLFactory.Candle.PowerToys.DragScrollAction dragScrollAction;

        //private bool SetMouseAction(DiagramMouseEventArgs e)
        //{
        //    Debug.Assert(e != null);
        //    if (e != null && e.Button == MouseButtons.Left && Form.ModifierKeys == (Keys.Control))
        //    {
        //        if (!(e.DiagramClientView.ActiveMouseAction is DSLFactory.Candle.PowerToys.DragScrollAction))
        //        {
        //            if (this.dragScrollAction == null)
        //            {
        //                this.dragScrollAction = new DSLFactory.Candle.PowerToys.DragScrollAction(this);
        //            }
        //            e.DiagramClientView.ActiveMouseAction = this.dragScrollAction;
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public override void OnMouseDown(DiagramMouseEventArgs e)
        //{
        //    base.OnMouseDown(e);

        //    if (e != null && SetMouseAction(e))
        //    {
        //        this.dragScrollAction.MouseDown(e);
        //    }
        //}

        //public override void OnMouseMove(DiagramMouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    if (e != null && SetMouseAction(e))
        //    {
        //        this.dragScrollAction.MouseDown(e);
        //    }
        //}

        //#endregion

        /// <summary>
        /// Sélection des shapes à afficher sur ce diagramme
        /// </summary>
        /// <param name="element">The shape to check.</param>
        /// <returns>
        /// true if the shape can have the child shape added to it; otherwise, false.
        /// </returns>
        protected override bool ShouldAddShapeForElement( ModelElement element )
        {
            if( 
                // Pas les éléments du diagramme DataLayerDiagram
                (element is Package || element is Association || element is DataType || element is EntityHasSubClasses )
                // Ni ceux du UIWorkflowLayerDiagram
                || (element is Action || element is UIView  ) 
                //
                //|| element is DataLayer 
                )
                return false;
            return true;
        }

        /// <summary>
        /// Creates a child shape for the parent shape.
        /// </summary>
        /// <param name="element">The parent shape.</param>
        /// <returns>The child shape for the parent shape.</returns>
        protected override ShapeElement CreateChildShape(ModelElement element)
        {
            if (element is global::DSLFactory.Candle.SystemModel.Scenario && this.GetType() != typeof(UIWorkflowLayerDiagram))
            {
                global::DSLFactory.Candle.SystemModel.ScenarioThumbnailShape newShape = new global::DSLFactory.Candle.SystemModel.ScenarioThumbnailShape(this.Partition);
                if (newShape != null) newShape.Size = newShape.DefaultSize; // set default shape size
                return newShape;
            }
            return base.CreateChildShape(element);
        }

        /// <summary>
        /// Arranges the shapes.
        /// </summary>
        public void ArrangeShapes()
        {
            List<ShapeElement> externalComponents = new List<ShapeElement>();
            List<ShapeElement> component = new List<ShapeElement>();
            foreach(PresentationElement elem in this.NestedChildShapes)
            {
                ShapeElement shape = elem as ShapeElement;
                if( shape == null )
                    continue;

                if( shape.ModelElement is SoftwareComponent)
                {
                    component.Add( shape);
                }
                else
                {
                    externalComponents.Add(shape);
                }
            }
            PointD startPoint = new PointD( 0.1, 0.1 );
            if( component.Count > 0 )
            {
                ShapeHelper.ArrangeChildShapes( this, component, 0, 1, startPoint, 0.2, 0 );
                startPoint = new PointD( component[0].AbsoluteBoundingBox.Right + 0.4, component[0].AbsoluteBoundingBox.Location.Y );
            }

            ShapeHelper.ArrangeChildShapes( this, externalComponents, 0, 1, startPoint, 0, 0.4 );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    partial class ComponentModelDiagramBase
    {
        #region Interception des erreurs d'annulation par l'utilisateur
        /// <summary>
        /// Checks to see who should report an exception that is thrown from the base design surface.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>Vrai si l'exception doit etre lancé</returns>
        public override bool UnhandledException(Exception ex)
        {
            if (ex is CanceledByUser)
                return false;

            return base.UnhandledException(ex);
        }

        /// <summary>
        /// Checks to see whether an exception that is thrown from the base design surface should be reported.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>
        /// true if the exception should be reported; otherwise, false.
        /// </returns>
        public override bool ShouldReportException(Exception ex)
        {
            if (ex is CanceledByUser)
                return false;

            return base.ShouldReportException(ex);
        }
        #endregion

        /// <summary>
        /// Gets the reference connect action.
        /// </summary>
        /// <value>The reference connect action.</value>
        public ReferenceConnectAction ReferenceConnectAction
        {
            get { return this.referenceConnectAction; }
        }

        /// <summary>
        /// Gets the target shape for class uses operation link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.Diagrams.NodeShape GetTargetShapeForClassUsesOperationLink(ClassUsesOperationLink link)
        {
            if (link != null)
            {
                return link.ToShape;
            }
            return null;
        }

        /// <summary>
        /// Gets the target role player for link mapped by class uses operation link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetTargetRolePlayerForLinkMappedByClassUsesOperationLink(ClassUsesOperationLink link)
        {
            if (link != null)
            {
                ShapeElement toShape = link.ToShape;
                if (toShape != null)
                {
                    return toShape.Subject;
                }
            }
            return null;
        }

    }
}
