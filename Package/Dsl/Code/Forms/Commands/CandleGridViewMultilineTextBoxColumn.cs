using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CandleGridViewMultilineTextBoxColumn : DataGridViewTextBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CandleGridViewMultilineTextBoxColumn"/> class.
        /// </summary>
        public CandleGridViewMultilineTextBoxColumn()
        {
            CellTemplate = new CandleGridViewMultilineTextBoxCell();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CandleGridViewMultilineTextBoxCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// Attaches and initializes the hosted editing control.
        /// </summary>
        /// <param name="rowIndex">The index of the row being edited.</param>
        /// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
        /// <param name="dataGridViewCellStyle">A cell style that is used to determine the appearance of the hosted control.</param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
                                                      DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            TextBox editingControl = base.DataGridView.EditingControl as TextBox;
            if (editingControl != null)
            {
                editingControl.AcceptsReturn = true;
                editingControl.Multiline = true;
            }
        }
    }
}