using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// Permet d'éditer le nom des couches pour la stratégie de nommage
    /// </summary>
    internal class LayerNameEditor : UITypeEditor
    {
        private ListBox _comboBox;
        private IWindowsFormsEditorService _edSvc;

        /// <summary>
        /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Edits the value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sp">The sp.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            _edSvc = (IWindowsFormsEditorService) sp.GetService(typeof (IWindowsFormsEditorService));
            if (_edSvc != null)
            {
                _comboBox = new ListBox();

                foreach (Type type in GetType().Assembly.GetTypes())
                {
                    if (type.IsClass && type.IsSubclassOf(typeof (SoftwareLayer)))
                    {
                        _comboBox.Items.Add(type.Name);
                    }
                }

                _comboBox.KeyDown += KeyDown;
                _comboBox.Leave += ValueChanged;
                _comboBox.DoubleClick += ValueChanged;
                _comboBox.Click += ValueChanged;
                _edSvc.DropDownControl(_comboBox);

                if (_comboBox.SelectedItem != null)
                    return _comboBox.SelectedItem.ToString();
            }
            return value;
        }

        /// <summary>
        /// Keys down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (((e != null) && e.Control) && ((e.KeyCode == Keys.Return) && (_edSvc != null)))
            {
                _edSvc.CloseDropDown();
            }
        }

        /// <summary>
        /// Values the changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ValueChanged(object sender, EventArgs e)
        {
            if (_edSvc != null)
            {
                _edSvc.CloseDropDown();
            }
        }
    }
}