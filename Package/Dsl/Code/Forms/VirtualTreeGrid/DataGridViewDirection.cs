using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewDirectionColumn : DataGridViewComboBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewDirectionColumn"/> class.
        /// </summary>
        public DataGridViewDirectionColumn()
        {
            CellTemplate = new DataGridViewDirectionCell();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewDirectionCell : DataGridViewComboBoxCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewDirectionCell"/> class.
        /// </summary>
        public DataGridViewDirectionCell()
        {
        }

        /// <summary>
        /// Attaches and initializes the hosted editing control.
        /// </summary>
        /// <param name="rowIndex">The index of the cell's parent row.</param>
        /// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
        /// <param name="dataGridViewCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> that determines the appearance of the hosted control.</param>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
                                                      DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            VirtualTreeGridItem item = DataGridView.Rows[rowIndex].DataBoundItem as VirtualTreeGridItem;
            DataGridViewComboBoxEditingControl ctrl = DataGridView.EditingControl as DataGridViewComboBoxEditingControl;
            ctrl.SelectedText = string.Empty;
            if (item != null && item.DataItem is IArgument)
            {
                DataGridView.EditingControl.Enabled = true;
                ctrl.SelectedIndex = ctrl.FindStringExact(item.Direction);
            }
            else
                DataGridView.EditingControl.Enabled = false;
        }
    }
}