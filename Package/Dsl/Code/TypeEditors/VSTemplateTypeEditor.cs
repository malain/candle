using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE80;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// Editeur permettant de sélectionner un template visual studio
    /// </summary>
    internal class VSTemplateTypeEditor : UITypeEditor
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
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            _edSvc = (IWindowsFormsEditorService) sp.GetService(typeof (IWindowsFormsEditorService));
            if (_edSvc != null)
            {
                _comboBox = new ListBox();

                Solution2 sln = ServiceLocator.Instance.ShellHelper.Solution as Solution2;
                if (sln == null)
                    return value;
                ModelElement mel = context.Instance as ModelElement;
                if (mel != null)
                {
                    string languageName = StrategyManager.GetInstance(mel.Store).TargetLanguage.Name;
                    string templateFolder = Path.Combine(FileLocationHelper.VSInstallDir, "ProjectTemplates");
                    int maxSize = 0;
                    // TODO if( !(mel is Layer ) || ((Layer)mel).HostingContext != HostingContext.Web )
                    PopulateListBoxItems(templateFolder, languageName, ref maxSize);
                    PopulateListBoxItems(templateFolder, "web", ref maxSize);
                    if (_comboBox.Items.Count == 0)
                        return value;
                    _comboBox.Size = new Size(maxSize + 10, 120);
                    _comboBox.KeyDown += KeyDown;
                    _comboBox.Leave += ValueChanged;
                    _comboBox.DoubleClick += ValueChanged;
                    _comboBox.Click += ValueChanged;
                    _edSvc.DropDownControl(_comboBox);

                    if (_comboBox.SelectedItem != null)
                        return _comboBox.SelectedItem.ToString();
                }
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFolder"></param>
        /// <param name="languageName"></param>
        /// <param name="maxSize"></param>
        private void PopulateListBoxItems(string templateFolder, string languageName, ref int maxSize)
        {
            using (Graphics graphics = _comboBox.CreateGraphics())
            {
                Font font = _comboBox.Font;
                string folder = Path.Combine(templateFolder, languageName);

                foreach (string file in Utils.SearchFile(folder, "*.zip"))
                {
                    string item = String.Concat(languageName, '/', Path.GetFileName(file));
                    _comboBox.Items.Add(item);
                    SizeF ef = graphics.MeasureString(item, font);
                    int num = (int) ef.Width;
                    if (num > maxSize)
                    {
                        maxSize = num;
                    }
                }
            }
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (((e != null) && e.Control) && ((e.KeyCode == Keys.Return) && (_edSvc != null)))
            {
                _edSvc.CloseDropDown();
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            if (_edSvc != null)
            {
                _edSvc.CloseDropDown();
            }
        }
    }
}