using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DSLFactory.Candle.SystemModel.Repository;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    internal class ExternalComponentPortPlacementHelper : PortPlacementHelper
    {
        // Surchage pour pouvoir accéder à la méthode protégée de PortPlacementHelper
        /// <summary>
        /// Gets the next edge children.
        /// </summary>
        /// <param name="currentEdge">The current edge.</param>
        /// <param name="parentShape">The parent shape.</param>
        /// <param name="edgeList">The edge list.</param>
        /// <param name="center">The center.</param>
        /// <returns></returns>
        protected override PortPlacement GetNextEdgeChildren(PortPlacement currentEdge, NodeShape parentShape,
                                                             out ArrayList edgeList, out PointD center)
        {
            PortPlacement placement = PortPlacement.None;
            do
            {
                placement = base.GetNextEdgeChildren(currentEdge, parentShape, out edgeList, out center);
            } while (placement == PortPlacement.Top || placement == PortPlacement.Bottom);

            return placement;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class InnerAreaField : AreaField
    {
        private static readonly StyleSetResourceId s_background;

        /// <summary>
        /// Initializes the <see cref="InnerAreaField"/> class.
        /// </summary>
        static InnerAreaField()
        {
            s_background = new StyleSetResourceId("AM", "innerAreaBackground");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerAreaField"/> class.
        /// </summary>
        /// <param name="fieldName">The ID for the shape field.</param>
        public InnerAreaField(string fieldName)
            : base(fieldName)
        {
        }

        /// <summary>
        /// Gets the background.
        /// </summary>
        /// <value>The background.</value>
        public static StyleSetResourceId Background
        {
            get { return s_background; }
        }

        /// <summary>
        /// Gets the ID of the brush that draws the background for the shape element to which this shape field is assigned.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="parentShape">The shape element to which the shape field is assigned.</param>
        /// <returns>
        /// The ID of the brush that draws the background for the shape element to which the shape field is assigned.
        /// </returns>
        public override StyleSetResourceId GetBackgroundBrushId(DiagramClientView view, ShapeElement parentShape)
        {
            return s_background;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal sealed class ExternalComponentShapeHelper
    {
        //private void ShowFromTemporaryFolder(string physicalModelFileName, string relativeModelFileName)
        //{
        //}

        /// <summary>
        /// Tries to show.
        /// </summary>
        /// <param name="model">The model.</param>
        public static void TryToShow(ExternalComponent model)
        {
            // Si le système n'est pas encore défini, on va demander à l'utilisateur d'en choisir un 
            //  dans le référentiel ou d'en créer un nouveau.
            ComponentModelMetadata metaData = model.MetaData;

            if (metaData == null)
            {
                using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Select model"))
                {
                    if (RepositoryManager.Instance.ModelsMetadata.SelectModel(model, out metaData))
                        transaction.Commit();
                }
                return;
            }
            ShowModel(metaData);
        }

        /// <summary>
        /// Affiche d'un modèle du repository
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        public static void ShowModel(ComponentModelMetadata metaData)
        {
            string targetFileName = RepositoryManager.Instance.ModelsMetadata.CopyModelInTemporaryFolder(metaData);
            if (targetFileName != null)
            {
                ServiceLocator.Instance.ShellHelper.Solution.DTE.ItemOperations.OpenFile(targetFileName,
                                                                                         Constants.vsViewKindDesigner);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    partial class ExternalComponentShape
    {
        /// <summary>
        /// Chargement et affichage du modèle
        /// </summary>
        /// <param name="e">The diagram point event arguments.</param>
        public override void OnDoubleClick(DiagramPointEventArgs e)
        {
            base.OnDoubleClick(e);

            ExternalComponent model = ModelElement as ExternalComponent;
            if (model != null)
            {
                ExternalComponentShapeHelper.TryToShow(model);
            }
        }

        /// <summary>
        /// Updates resources, such as pens and brushes, before they are used to paint the shape.
        /// </summary>
        protected override void OnBeforePaint()
        {
            base.OnBeforePaint();

            // On s'assure que le shape est bien synchronizé avec le modèle
            ((ExternalComponent) ModelElement).UpdateFromModel();
        }

        // Pose pb quand on ouvre le modèle (Voir ActiveBoundsRule de la classe Port)
        //protected override void ConfiguredChildPortShape( Port child, bool childWasPlaced )
        //{
        //    Transaction transaction = base.Store.TransactionManager.CurrentTransaction.TopLevelTransaction;
        //    if( ( transaction == null  || !transaction.IsSerializing ) && !childWasPlaced )
        //    {
        //        ExternalComponentPortPlacementHelper pp = new ExternalComponentPortPlacementHelper();
        //        pp.PositionChildPort( child, this );
        //    }
        //}

        /// <summary>
        /// Initialize the collection of shape fields associated with this shape type.
        /// </summary>
        /// <param name="shapeFields"></param>
        protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
        {
            base.InitializeShapeFields(shapeFields);

            InnerAreaField innerArea = new InnerAreaField("InnerArea");
            innerArea.DefaultSelectable = false;
            innerArea.DrawBorder = true;
            innerArea.AnchoringBehavior.SetBottomAnchor(AnchoringBehavior.Edge.Bottom, 0.02f);
            innerArea.AnchoringBehavior.SetTopAnchor(AnchoringBehavior.Edge.Top, 0.02f);
            shapeFields.Add(innerArea);
        }

        /// <summary>
        /// Initializes style set resources for this shape type
        /// </summary>
        /// <param name="classStyleSet">The style set for this shape class</param>
        protected override void InitializeResources(StyleSet classStyleSet)
        {
            base.InitializeResources(classStyleSet);

            BrushSettings backgroundBrush = new BrushSettings();
            backgroundBrush.Color = Color.White;
            classStyleSet.AddBrush(InnerAreaField.Background, DiagramBrushes.ShapeBackground, backgroundBrush);
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //partial class ExternalBinaryComponentShape
    //{
    //    /// <summary>
    //    /// Chargement et affichage du modèle
    //    /// </summary>
    //    /// <param name="e"></param>
    //    public override void OnDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPointEventArgs e)
    //    {
    //        base.OnDoubleClick(e);

    //        ExternalComponent model = this.ModelElement as ExternalComponent;
    //        if (model != null)
    //        {
    //            ExternalComponentShapeHelper.TryToShow(model);
    //        }
    //    }

    //    //protected override void ConfiguredChildPortShape(Port child, bool childWasPlaced)
    //    //{
    //    //    Transaction transaction = base.Store.TransactionManager.CurrentTransaction.TopLevelTransaction;
    //    //    if (((transaction == null) || !transaction.IsSerializing) && !childWasPlaced)
    //    //    {
    //    //        ExternalComponentPortPlacementHelper pp = new ExternalComponentPortPlacementHelper();
    //    //        pp.PositionChildPort(child, this);
    //    //    }
    //    //}

    //    protected override void InitializeShapeFields(IList<Microsoft.VisualStudio.Modeling.Diagrams.ShapeField> shapeFields)
    //    {
    //        base.InitializeShapeFields(shapeFields);

    //        InnerAreaField innerArea = new InnerAreaField("InnerArea");
    //        innerArea.DefaultSelectable = false;
    //        innerArea.DrawBorder = true;
    //        innerArea.AnchoringBehavior.SetBottomAnchor(AnchoringBehavior.Edge.Bottom, 0.02f);
    //        innerArea.AnchoringBehavior.SetTopAnchor(AnchoringBehavior.Edge.Top, 0.02f);
    //        shapeFields.Add(innerArea);
    //    }

    //    protected override void InitializeResources(StyleSet classStyleSet)
    //    {
    //        base.InitializeResources(classStyleSet);

    //        BrushSettings backgroundBrush = new BrushSettings();
    //        backgroundBrush.Color = Color.White;
    //        classStyleSet.AddBrush(InnerAreaField.Background, DiagramBrushes.ShapeBackground, backgroundBrush);
    //    }
    //}
}