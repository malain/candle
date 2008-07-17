using System.Drawing;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewIsCollectionColumn : DataGridViewCheckBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewIsCollectionColumn"/> class.
        /// </summary>
        public DataGridViewIsCollectionColumn()
        {
            CellTemplate = new DataGridViewIsCollectionCell();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewIsCollectionCell : DataGridViewCheckBoxCell
    {
        /// <summary>
        /// Paints the specified graphics.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="clipBounds">The clip bounds.</param>
        /// <param name="cellBounds">The cell bounds.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="elementState">State of the element.</param>
        /// <param name="value">The value.</param>
        /// <param name="formattedValue">The formatted value.</param>
        /// <param name="errorText">The error text.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="advancedBorderStyle">The advanced border style.</param>
        /// <param name="paintParts">The paint parts.</param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                                      DataGridViewElementStates elementState, object value, object formattedValue,
                                      string errorText, DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            VirtualTreeGridItem item = OwningRow.DataBoundItem as VirtualTreeGridItem;
            // On n'affiche pas la case à cocher
            if (item != null && (item.Kind == ModelKind.Root || item.Kind == ModelKind.Category))
                paintParts &= ~DataGridViewPaintParts.ContentForeground;

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText,
                       cellStyle, advancedBorderStyle, paintParts);
        }
    }
}