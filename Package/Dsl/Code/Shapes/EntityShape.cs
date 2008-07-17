using System;
using System.Drawing;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands;
using DSLFactory.Candle.SystemModel.Properties;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    partial class EntityShape : ISupportArrangeShapes
    {
        #region Affichage au dessus

        /// <summary>
        /// Sets value for the <see cref="P:Microsoft.VisualStudio.Modeling.Diagrams.NodeShape.IsExpanded"></see> property.
        /// </summary>
        /// <param name="newValue">The new value for the value for the <see cref="P:Microsoft.VisualStudio.Modeling.Diagrams.NodeShape.IsExpanded"></see> property.</param>
        protected override void SetIsExpandedValue(bool newValue)
        {
            // On place le shape devant tous les autres
            if (newValue && !Store.InUndoRedoOrRollback)
            {
                ParentShape.NestedChildShapes.Move(this, ParentShape.NestedChildShapes.Count - 1);
            }
            base.SetIsExpandedValue(newValue);
        }

        #endregion

        #region AdjustSize

        /// <summary>
        /// Arranges the shapes.
        /// </summary>
        public void ArrangeShapes()
        {
            ShapeHelper.ResizeToContent(this);
        }

        /// <summary>
        /// Alerts listeners when the mouse is double-clicked over the shape.
        /// </summary>
        /// <param name="e">The diagram point event arguments.</param>
        public override void OnDoubleClick(DiagramPointEventArgs e)
        {
            base.OnDoubleClick(e);

            if (Utils.IsKeyPressed(Keys.Shift))
            {
                using (Transaction transaction = Store.TransactionManager.BeginTransaction("Adjust size"))
                {
                    ShapeHelper.ResizeToContent(this);
                    transaction.Commit();
                }
                return;
            }

            Entity entity = ModelElement as Entity;
            if (entity != null) Mapper.Instance.ShowCode(entity.Id, entity.Name, null);
        }

        /// <summary>
        /// Alerts listeners that the shape has been assigned as a child shape to a parent shape.
        /// </summary>
        public override void OnShapeInserted()
        {
            base.OnShapeInserted();
            if (!Store.InUndoRedoOrRollback)
            {
                using (Transaction transaction = Store.TransactionManager.BeginTransaction("Adjust size"))
                {
                    ShapeHelper.ResizeToContent(this);
                    transaction.Commit();
                }
            }
        }

        #endregion

        /// <summary>
        /// Creates an area field for the background gradient.
        /// </summary>
        /// <param name="fieldName">The name of the area field.</param>
        /// <returns>
        /// The area field for the background gradient.
        /// </returns>
        protected override AreaField CreateBackgroundGradientField(string fieldName)
        {
            return new MyAreaField(fieldName);
        }

        /// <summary>
        /// Initializes style set resources for this shape type
        /// </summary>
        /// <param name="classStyleSet">The style set for this shape class</param>
        protected override void InitializeResources(StyleSet classStyleSet)
        {
            base.InitializeResources(classStyleSet);

            PenSettings settings = new PenSettings();
            settings.Color = Color.FromArgb(0xff, 0, 0, 0);
            settings.Width = 0.01f;
            classStyleSet.OverridePen(DiagramPens.ShapeOutline, settings);

            BrushSettings settings2 = new BrushSettings();
            settings2.Color = Color.FromArgb(0xff, 159, 183, 215);
            classStyleSet.OverrideBrush(DiagramBrushes.ShapeBackground, settings2);

            FontSettings settings3 = new FontSettings();
            settings3.Style = FontStyle.Regular;
            settings3.Size = 0.09722222f;
            classStyleSet.AddFont(new StyleSetResourceId(string.Empty, "ShapeTextRegular7"), DiagramFonts.ShapeText,
                                  settings3);
        }

        /// <summary>
        /// TIPS modification de l'affichage des éléments dans la liste
        /// </summary>
        /// <param name="melType">The type of the DomainClass that this shape is mapped to</param>
        /// <returns></returns>
        protected override CompartmentMapping[] GetCompartmentMappings(Type melType)
        {
            CompartmentMapping[] mappings = base.GetCompartmentMappings(melType);
            foreach (CompartmentMapping map in mappings)
            {
                if (map is ElementListCompartmentMapping)
                {
                    ElementListCompartmentMapping elmMap = map as ElementListCompartmentMapping;
                    if (elmMap.CompartmentId == "Properties")
                    {
                        elmMap.ImageGetter = GetImageForElement;
                    }
                }
            }
            return mappings;
        }

        /// <summary>
        /// Gets the image for element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private Image GetImageForElement(ModelElement element)
        {
            if (element is Property && ((Property) element).IsPrimaryKey)
                return Resources.PrimaryKey;

            if (element is Property && ((Property) element).IsForeignKey)
                return Resources.ForeignKey;
            return Resources.Property;
        }

        #region Nested type: MyAreaField

        /// <summary>
        /// 
        /// </summary>
        public class MyAreaField : AreaField
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MyAreaField"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public MyAreaField(string name) : base(name)
            {
            }
        }

        #endregion
    }
}