using System;
using System.Drawing;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewNameColumn : DataGridViewTextBoxColumn
    {
        private string headerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewNameColumn"/> class.
        /// </summary>
        public DataGridViewNameColumn()
        {
            CellTemplate = new DataGridViewNameCell();
        }

        /// <summary>
        /// Gets or sets the name of the header.
        /// </summary>
        /// <value>The name of the header.</value>
        public string HeaderName
        {
            get { return headerName; }
            set { headerName = value; }
        }

        internal event DataGridViewCellEventHandler ExpandCollapse;

        /// <summary>
        /// Raises the <see cref="E:ExpandCollapseCell"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        internal void OnExpandCollapseCell(DataGridViewCellEventArgs e)
        {
            if (ExpandCollapse != null)
            {
                ExpandCollapse(this, e);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewNameCell : DataGridViewTextBoxCell
    {
        private Rectangle hitRect = Rectangle.Empty;

        /// <summary>
        /// Gets the type of the cell's hosted editing control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see> representing the <see cref="T:System.Windows.Forms.DataGridViewTextBoxEditingControl"></see> type.</returns>
        public override Type EditType
        {
            get { return typeof (DataGridViewNameEditingControl); }
        }

        /// <summary>
        /// Called when the cell is clicked.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs"></see> that contains the event data.</param>
        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);

            Point mousePosition = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            mousePosition = DataGridView.PointToClient(mousePosition);

            if (hitRect != Rectangle.Empty && hitRect.Contains(mousePosition))
            {
                ((DataGridViewNameColumn) OwningColumn).OnExpandCollapseCell(e);
            }
        }

        /// <summary>
        /// Sets the location and size of the editing panel hosted by the cell, and returns the normal bounds of the editing control within the editing panel.
        /// </summary>
        /// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle"></see> that defines the cell bounds.</param>
        /// <param name="cellClip">The area that will be used to paint the editing panel.</param>
        /// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> that represents the style of the cell being edited.</param>
        /// <param name="singleVerticalBorderAdded">true to add a vertical border to the cell; otherwise, false.</param>
        /// <param name="singleHorizontalBorderAdded">true to add a horizontal border to the cell; otherwise, false.</param>
        /// <param name="isFirstDisplayedColumn">true if the cell is in the first column currently displayed in the control; otherwise, false.</param>
        /// <param name="isFirstDisplayedRow">true if the cell is in the first row currently displayed in the control; otherwise, false.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Rectangle"></see> that represents the normal bounds of the editing control within the editing panel.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The cell has not been added to a <see cref="T:System.Windows.Forms.DataGridView"></see> control.</exception>
        public override Rectangle PositionEditingPanel(Rectangle cellBounds, Rectangle cellClip,
                                                       DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded,
                                                       bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn,
                                                       bool isFirstDisplayedRow)
        {
            Rectangle rect =
                base.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded,
                                          singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
            VirtualTreeGridItem value = OwningRow.DataBoundItem as VirtualTreeGridItem;

            Point pt = DataGridView.EditingPanel.Location;
            int decalage = 43;
            if (value != null)
                if (value.Kind == ModelKind.Child)
                {
                    decalage = 58;
                }

            rect.Width -= decalage;
            pt.X += decalage;
            rect.X += decalage;
            DataGridView.EditingPanel.BackColor = Color.Transparent;

            //DataGridView.EditingPanel.Location = pt;
            //DataGridView.EditingPanel.Width -= decalage;

            return rect;
        }

        /// <summary>
        /// Affichage d'une ligne.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="clipBounds">The clip bounds.</param>
        /// <param name="cellBounds">The cell bounds.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="cellState">State of the cell.</param>
        /// <param name="value">The value.</param>
        /// <param name="formattedValue">The formatted value.</param>
        /// <param name="errorText">The error text.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="advancedBorderStyle">The advanced border style.</param>
        /// <param name="paintParts">The paint parts.</param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                                      DataGridViewElementStates cellState, object value, object formattedValue,
                                      string errorText, DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            // Une ligne peut contenir des icones en début de ligne
            VirtualTreeGridItem item = DataGridView.Rows[rowIndex].DataBoundItem as VirtualTreeGridItem;
            if (item == null)
                return;

            // Décalage de la zone d'affichage pour libérer de la place pour les icones
            if (item.Kind == ModelKind.Member)
            {
                cellStyle.Padding = new Padding(43, 0, 0, 0);
            }
            if (item.Kind == ModelKind.Child)
            {
                cellStyle.Padding = new Padding(58, 0, 0, 0);
            }

            Color oldForeColor = cellStyle.ForeColor;
            // Entete en gras
            if (item.Kind == ModelKind.Root)
            {
                cellStyle.ForeColor = Color.Black;
                cellStyle.Font = new Font(cellStyle.Font, FontStyle.Bold);
                formattedValue = ((DataGridViewNameColumn) OwningColumn).HeaderName;
            }
            else if (item.Kind == ModelKind.Category)
            {
                cellStyle.ForeColor = Color.Gray;
                cellStyle.Font = new Font(cellStyle.Font, FontStyle.Bold | FontStyle.Italic);
                cellStyle.Padding = new Padding(20, 0, 0, 0);
            }
            else if (Utils.StringCompareEquals((string) formattedValue, item.EmptyName))
            {
                cellStyle.ForeColor = Color.Black;
                cellStyle.Font = new Font(cellStyle.Font, FontStyle.Italic);
            }
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Affichagedu texte
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                       cellStyle, advancedBorderStyle, paintParts);
            cellStyle.ForeColor = oldForeColor;

            // Zone de click
            hitRect = Rectangle.Empty;

            VirtualTreeGridItem nextItem = null;
            try
            {
                nextItem = rowIndex == DataGridView.Rows.Count - 1
                               ? null
                               : DataGridView.Rows[rowIndex + 1].DataBoundItem as VirtualTreeGridItem;
            }
            catch
            {
                // Dans le cas ou la dernière ligne ne contient pas de données
            }
            bool lastCategoryRow = nextItem == null || nextItem.Kind != item.Kind;

            // Affichage des icones
            if (item.Kind == ModelKind.Category)
            {
                if (item.Category.Icon != null)
                {
                    Point pt = cellBounds.Location;
                    pt.X += 2;
                    Bitmap bmp = item.Category.Icon;
                    bmp.MakeTransparent();
                    graphics.DrawImage(bmp, pt);
                }
            }
            else if (item.Kind == ModelKind.Member)
            {
                Point pt = cellBounds.Location;
                pt.X += 6;
                Bitmap bmp;

                if (item.DataItem is IHasChildren)
                {
                    bmp = lastCategoryRow ? VirtualTreeGridResource.CollapsedEnd : VirtualTreeGridResource.Collapsed;
                    if (!item.Collapse)
                        bmp = VirtualTreeGridResource.Expanded;
                    bmp.MakeTransparent();
                    graphics.DrawImage(bmp, pt);

                    // Sauvegarde de la zone de click
                    hitRect = new Rectangle(pt, bmp.Size);

                    pt.X += bmp.Width;
                }
                else
                {
                    pt.X += 16;
                }
                bmp = VirtualTreeGridResource.VSObject_Method;
                pt.Y += (cellBounds.Height - bmp.Height)/2;
                bmp.MakeTransparent();
                graphics.DrawImage(bmp, pt);
            }
            else if (item.Kind == ModelKind.Child)
            {
                Point pt = cellBounds.Location;
                pt.X += 6;
                Bitmap bmp = VirtualTreeGridResource.VerticalLine;
                bmp.MakeTransparent();
                graphics.DrawImage(bmp, pt);

                bmp = lastCategoryRow
                          ? VirtualTreeGridResource.ArgumentVerticalLineEnd
                          : VirtualTreeGridResource.ArgumentVerticalLine;
                bmp.MakeTransparent();
                pt.X += 22;
                graphics.DrawImage(bmp, pt);

                Rectangle rect = cellBounds;
                rect.X = pt.X + 20;
                rect.Width = 16;

                VirtualTreeGridItem prevItem = DataGridView.Rows[rowIndex - 1].DataBoundItem as VirtualTreeGridItem;
                if (prevItem != null)
                {
                    bool firstCategoryRow = prevItem.Kind != item.Kind;

                    string prefix = ",";
                    if (lastCategoryRow && firstCategoryRow)
                    {
                        prefix = "()";
                    }
                    else if (lastCategoryRow)
                    {
                        prefix = ")";
                    }
                    else if (firstCategoryRow)
                    {
                        prefix = "(";
                    }

                    Font font = new Font(cellStyle.Font, FontStyle.Bold | FontStyle.Regular);
                    TextRenderer.DrawText(graphics, prefix, font, rect, cellStyle.ForeColor);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class DataGridViewNameEditingControl : DataGridViewTextBoxEditingControl
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyPress"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs"></see> that contains the event data.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            VirtualTreeGridItem item =
                EditingControlDataGridView.Rows[EditingControlRowIndex].DataBoundItem as VirtualTreeGridItem;
            if (item != null)
                if (item.Kind == ModelKind.Member)
                {
                    string separators = ((VirtualTreeGrid) EditingControlDataGridView).MemberSeparators;
                    if (separators.IndexOf(e.KeyChar) >= 0)
                    {
                        ((VirtualTreeGrid) EditingControlDataGridView).NavigateNext(EditingControlRowIndex, false);
                        e.Handled = true;
                        return;
                    }
                }

            if (item != null)
                if (item.Kind == ModelKind.Child)
                {
                    string separators = ((VirtualTreeGrid) EditingControlDataGridView).ChildSeparators;
                    if (separators.IndexOf(e.KeyChar) >= 0)
                    {
                        ((VirtualTreeGrid) EditingControlDataGridView).NavigateNext(EditingControlRowIndex, e.KeyChar == ')');
                        e.Handled = true;
                        return;
                    }
                }

            // La 1ère lettre ne peut pas être un chiffre
            if (Text.Length == 0 && Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Caractères valides
            if (Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '_' || Char.IsControl(e.KeyChar))
                return;

            // Tous les autres sont incorrects
            e.Handled = true;
        }
    }
}