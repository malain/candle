using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ShapeHelper
    {
        /// <summary>
        /// Gets the desktop window.
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        internal static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// Measures the display text.
        /// </summary>
        /// <param name="displayText">The display text.</param>
        /// <param name="font">The font.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        private static SizeD MeasureDisplayText(string displayText, Font font, StringFormat format)
        {
            using (Graphics graphics1 = Graphics.FromHwnd(GetDesktopWindow()))
            {
                SizeF ef1 = graphics1.MeasureString(displayText, font, new SizeF(1000f, 1000f), format);
                return new SizeD(ef1.Width, ef1.Height);
            }
        }

        /// <summary>
        /// Measures the size of the text field.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="textField">The text field.</param>
        /// <returns></returns>
        internal static SizeD MeasureTextFieldSize(ShapeElement shape, TextField textField)
        {
            string text = textField.GetAccessibleValue(shape);
            return MeasureDisplayText(text, textField.GetFont(shape), textField.GetStringFormat(shape));
        }

        /// <summary>
        /// Resizes to content.
        /// </summary>
        /// <param name="shape">The shape.</param>
        internal static void ResizeToContent(NodeShape shape)
        {
            double num1 = 0.27;

            foreach (Decorator decorator in shape.Decorators)
            {
                if (decorator.Field != null)
                {
                    if (decorator.Field is TextField)
                    {
                        SizeD sz = MeasureTextFieldSize(shape, (TextField) decorator.Field);
                        num1 += Math.Max(0.7, sz.Width);
                    }
                    else //if( decorator1 is ExpandCollapseDecorator  )
                    {
                        RectangleD rec = decorator.Field.GetBounds(shape);
                        num1 += (0.05 + rec.Width) + 0.08;
                    }
                }
            }
            using (Transaction transaction = shape.Store.TransactionManager.BeginTransaction("Resize contents"))
            {
                shape.Size = new SizeD(num1, shape.Size.Height);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Arrange les shapes enfants
        /// </summary>
        /// <param name="container">Shape container</param>
        /// <param name="childs">The childs.</param>
        /// <param name="maxWidth">Largeur maxi alloué aux enfants ou 0 si toute la place</param>
        /// <param name="maxItemByRow">Nbre maxi de shapes par ligne ou 0 pour toute la largeur</param>
        /// <param name="startPoint">Point de départ relatif au container</param>
        /// <param name="horizontalSpacing">Espace horizontal entre les shapes</param>
        /// <param name="verticalSpacing">Espace vertical entre les shapes</param>
        /// <returns></returns>
        internal static double ArrangeChildShapes(NodeShape container, IList<ShapeElement> childs, double maxWidth,
                                                  int maxItemByRow, PointD startPoint, double horizontalSpacing,
                                                  double verticalSpacing)
        {
            double width = maxWidth;

            using (Transaction transaction = container.Store.TransactionManager.BeginTransaction("Arrange child shapes")
                )
            {
                double rowHeight = 0;
                int item = 0;

                // Il en faut au moins un des deux de renseigné
                Debug.Assert(maxWidth != 0 || maxItemByRow != 0);

                // Position de départ du premier shape
                RectangleD bounds = new RectangleD(startPoint, new SizeD());
                if (maxWidth > 0)
                    maxWidth += startPoint.X;

                foreach (ShapeElement element in childs)
                {
                    NodeShape child = element as NodeShape;
                    if (child == null)
                        continue;

                    if (child is ISupportArrangeShapes)
                    {
                        ((ISupportArrangeShapes) child).ArrangeShapes();
                    }

                    // Nouvelle position
                    bounds.Width = child.AbsoluteBounds.Width;

                    // Saut de ligne
                    if ((maxWidth > 0 && bounds.Right > maxWidth && item > 0) ||
                        (maxItemByRow > 0 && item == maxItemByRow))
                    {
                        bounds.X = startPoint.X;
                        bounds.Y += rowHeight + verticalSpacing;
                        rowHeight = child.AbsoluteBounds.Height;
                        item = 0;
                    }
                    else
                    {
                        // Hauteur du shape le plus haut
                        if (child.AbsoluteBounds.Height > rowHeight)
                            rowHeight = child.AbsoluteBounds.Height;
                    }

                    if (child.BoundsRules != null)
                        bounds = child.BoundsRules.GetCompliantBounds(child, bounds);

                    child.Location = new PointD(bounds.X, bounds.Y);

                    if (bounds.Width > width)
                        width = bounds.Width;

                    bounds.X += (bounds.Width + horizontalSpacing);
                    item++;
                }
                transaction.Commit();
            }
            return width;
        }
    }
}