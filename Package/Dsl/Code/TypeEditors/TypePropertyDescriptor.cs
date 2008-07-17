using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    internal class TypePropertyEditor : UITypeEditor
    {
        private ComboBox _comboBox;
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
            ModelElement model = context.Instance as ModelElement;
            if (model == null)
                return value;

            _edSvc = (IWindowsFormsEditorService) sp.GetService(typeof (IWindowsFormsEditorService));
            if (_edSvc != null)
            {
                _comboBox = new ComboBox();
                _comboBox.DropDownStyle = ComboBoxStyle.Simple;

                int num1 = 0;
                SoftwareComponent component = CandleModel.GetInstance(model.Store).SoftwareComponent;
                string item1 = PopulateListBoxItems(component.GetDefinedTypeNames(), out num1);
                _comboBox.Size = new Size(num1 + 10, 120);
                _comboBox.KeyDown += KeyDown;
                _comboBox.Leave += ValueChanged;
                _comboBox.DoubleClick += ValueChanged;
                _comboBox.Click += ValueChanged;
                _edSvc.DropDownControl(_comboBox);
                if (_comboBox.Text.Length == 0)
                {
                    return value;
                }
                string item2 = _comboBox.Text;
                if ((item2 == null) || (item1 == item2))
                {
                    return value;
                }

                if (!String.IsNullOrEmpty(item2))
                {
                    bool shouldCreateModel = false;
                    ClrTypeParser.Parse(item2, delegate(string typeName)
                                                   {
                                                       shouldCreateModel = true;
                                                       return typeName;
                                                   });

                    // Si il y a des types à créer, il faut que la couche modèle existe
                    if (shouldCreateModel && component.DataLayer == null)
                    {
                        IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                        if (ide != null)
                        {
                            ide.ShowMessage(
                                String.Format("Can't create user type '{0}' because the models layer does not exist.",
                                              item2));
                        }
                    }
                    else
                        return item2;
                }
            }
            return value;
        }


        /// <summary>
        /// Populates the list box items.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="maxLengInPixel">The max leng in pixel.</param>
        /// <returns></returns>
        private string PopulateListBoxItems(IList<string> types, out int maxLengInPixel)
        {
            string text1 = null;


            string item1 = null;
            maxLengInPixel = 0;
            Graphics graphics1 = _comboBox.CreateGraphics();
            using (Graphics graphics2 = graphics1)
            {
                Font font1 = _comboBox.Font;
                foreach (string item2 in types)
                {
                    _comboBox.Items.Add(item2);
                    if ((item1 == null) && (Utils.StringCompareEquals(text1, item2)))
                    {
                        item1 = item2;
                    }
                    SizeF ef1 = graphics1.MeasureString(item2.ToString(), font1);
                    int num1 = (int) ef1.Width;
                    if (num1 > maxLengInPixel)
                    {
                        maxLengInPixel = num1;
                    }
                }
            }
            if (item1 != null)
            {
                _comboBox.SelectedItem = item1;
                return item1;
            }
            if (_comboBox.Items.Count != 0)
            {
                _comboBox.SelectedIndex = 0;
                item1 = _comboBox.SelectedItem as string;
            }
            return item1;
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