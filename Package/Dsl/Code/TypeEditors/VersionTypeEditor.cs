using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// Editeur permettant de sélectionner une nouvelle version pour un composant externe
    /// </summary>
    internal class VersionTypeEditor : UITypeEditor
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
            NodeShape shape = context.Instance as NodeShape;
            if (shape == null)
                return value;

            ExternalComponent externalComponent = shape.ModelElement as ExternalComponent;
            if (externalComponent == null)
                return value;

            _edSvc = (IWindowsFormsEditorService) sp.GetService(typeof (IWindowsFormsEditorService));
            if (_edSvc != null)
            {
                _comboBox = new ListBox();

                PopulateListBoxItems(externalComponent.ModelMoniker);
                if (_comboBox.Items.Count == 0)
                    return value;

                _comboBox.KeyDown += KeyDown;
                _comboBox.Leave += ValueChanged;
                _comboBox.DoubleClick += ValueChanged;
                _comboBox.Click += ValueChanged;
                _edSvc.DropDownControl(_comboBox);

                if (_comboBox.SelectedItem != null)
                    return new Version(_comboBox.SelectedItem.ToString());
            }
            return value;
        }


        /// <summary>
        /// Populates the list box items.
        /// </summary>
        /// <param name="id">The id.</param>
        private void PopulateListBoxItems(Guid id)
        {
            List<ComponentModelMetadata> datas = RepositoryManager.Instance.ModelsMetadata.Metadatas.GetAllVersions(id);
            foreach (ComponentModelMetadata data in datas)
            {
                _comboBox.Items.Add(data.Version.ToString());
            }
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