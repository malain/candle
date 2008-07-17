using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Surcharge pour pouvoir instancier notre classe personnalisée
    /// </summary>
    public class DataGridViewDropDownListColumn : DataGridViewComboBoxColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewDropDownListColumn"/> class.
        /// </summary>
        public DataGridViewDropDownListColumn()
        {
            CellTemplate = new DataGridViewDropDownListCell(this);
        }
    }

    /// <summary>
    /// Cellule personnalisé permettant de gérer les types
    /// </summary>
    public class DataGridViewDropDownListCell : DataGridViewComboBoxCell
    {
        private DataGridViewDropDownListColumn _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewDropDownListCell"/> class.
        /// </summary>
        /// <param name="template">The template.</param>
        public DataGridViewDropDownListCell(DataGridViewDropDownListColumn template)
        {
            _template = template;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridViewDropDownListCell"/> class.
        /// </summary>
        public DataGridViewDropDownListCell()
        {
        }

        /// <summary>
        /// Gets the type of the cell's hosted editing control.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Type"></see> of the underlying editing control. This property always returns <see cref="T:System.Windows.Forms.DataGridViewComboBoxEditingControl"></see>.</returns>
        public override Type EditType
        {
            get { return typeof (DataGridViewDropdDownListEditingControl); }
        }

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell"></see>.
        /// </returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override object Clone()
        {
            DataGridViewDropDownListCell clone = (DataGridViewDropDownListCell) base.Clone();
            clone._template = _template;
            return clone;
        }

        /// <summary>
        /// On rentre en mode édition. 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="initialFormattedValue"></param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
                                                      DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            ComboBox comboBox = base.DataGridView.EditingControl as ComboBox;
            if (comboBox != null)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                VirtualTreeGridItem value = OwningRow.DataBoundItem as VirtualTreeGridItem;
                comboBox.Enabled = (value != null && !value.IsNewValue);
                if (comboBox.Enabled)
                {
                    //if( !comboBox.Items.Contains( value.Type ) )
                    //    comboBox.Items.Add( value.Type );
                    //comboBox.SelectedIndex = comboBox.FindString( value.Type );
                    comboBox.Text = value.Type;
                }
            }
        }

        /// <summary>
        /// Gets the formatted value of the cell's data.
        /// </summary>
        /// <param name="value">The value to be formatted.</param>
        /// <param name="rowIndex">The index of the cell's parent row.</param>
        /// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> in effect for the cell.</param>
        /// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter"></see> associated with the value type that provides custom conversion to the formatted value type, or null if no such custom conversion is needed.</param>
        /// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter"></see> associated with the formatted value type that provides custom conversion from the value type, or null if no such custom conversion is needed.</param>
        /// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts"></see> values describing the context in which the formatted value is needed.</param>
        /// <returns>
        /// The value of the cell's data after formatting has been applied or null if the cell is not part of a <see cref="T:System.Windows.Forms.DataGridView"></see> control.
        /// </returns>
        /// <exception cref="T:System.Exception">Formatting failed and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError"></see> event of the <see cref="T:System.Windows.Forms.DataGridView"></see> control or the handler set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException"></see> property to true. The exception object can typically be cast to type <see cref="T:System.FormatException"></see> for type conversion errors or to type <see cref="T:System.ArgumentException"></see> if value cannot be found in the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.DataSource"></see> or the <see cref="P:System.Windows.Forms.DataGridViewComboBoxCell.Items"></see> collection. </exception>
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle,
                                                    TypeConverter valueTypeConverter,
                                                    TypeConverter formattedValueTypeConverter,
                                                    DataGridViewDataErrorContexts context)
        {
            //try
            //{
            //    return base.GetFormattedValue( value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context );
            //}
            //catch
            //{
            return value;
            //}
        }

        ///// <summary>
        ///// Fin de l'édition. On met à jour la liste des valeurs disponibles
        ///// </summary>
        //public override void DetachEditingControl()
        //{
        //    ComboBox comboBox = base.DataGridView.EditingControl as ComboBox;
        //    if (comboBox != null)
        //    {
        //        VirtualTreeGridItem item = ((VirtualTreeGrid)base.DataGridView).GetRowValue(this.RowIndex);
        //        string text = comboBox.Text;
        //        if (item != null && !String.IsNullOrEmpty(item.Type))
        //            text = item.Type;

        //        //if (!String.IsNullOrEmpty(text))
        //        //{
        //        //    if (!template.Items.Contains(text))
        //        //    {
        //        //        template.Items.Add(text);
        //        //    }
        //        //}
        //    }
        //    base.DetachEditingControl();
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch)]
    internal class DataGridViewDropdDownListEditingControl : DataGridViewComboBoxEditingControl
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyPress"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs"></see> that contains the event data.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (ModifierKeys == Keys.None && !Char.IsControl(e.KeyChar) && e.Handled == false)
            {
                // La 1ère lettre d'un mot ne peut pas être un chiffre
                if ((Text.Length == 0 || Text[Text.Length - 1] == '.') &&
                    !(Char.IsLetter(e.KeyChar) || e.KeyChar == '_'))
                {
                    e.Handled = true;
                    return;
                }

                // Caractères valides
                if (Char.IsLetterOrDigit(e.KeyChar) || "._<>[]".IndexOf(e.KeyChar) >= 0)
                    return;

                // Tous les autres sont incorrects
                e.Handled = true;
            }
        }

        /// <summary>
        /// Determines whether the specified key is a regular input key that the editing control should process or a special key that the <see cref="T:System.Windows.Forms.DataGridView"></see> should process.
        /// </summary>
        /// <param name="keyData">A bitwise combination of <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key that was pressed.</param>
        /// <param name="dataGridViewWantsInputKey">true to indicate that the <see cref="T:System.Windows.Forms.DataGridView"></see> control can process the key; otherwise, false.</param>
        /// <returns>
        /// true if the specified key is a regular input key that should be handled by the editing control; otherwise, false.
        /// </returns>
        public override bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            bool b = base.EditingControlWantsInputKey(keyData, dataGridViewWantsInputKey);

            if (!b && !base.DroppedDown)
            {
                Keys tmp = keyData & ~Keys.Shift;
                if (tmp == Keys.Left || tmp == Keys.Right || tmp == Keys.Home || tmp == Keys.End)
                    return true;
            }
            return b;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            EditingControlValueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }
    }
}