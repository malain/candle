using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Editor
{
    internal class AssemblyReferenceEditor : UITypeEditor
    {
        private DotNetAssembly _externalAssembly;

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>
        /// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            PresentationElement shape = context.Instance as PresentationElement;
            if (shape == null)
                return value;

            _externalAssembly = shape.ModelElement as DotNetAssembly;
            if (_externalAssembly == null)
                return value;

            using (
                Transaction transaction =
                    _externalAssembly.Store.TransactionManager.BeginTransaction("Retrieve assembly infos"))
            {
                IAssemblySelectorDialog selector = ServiceLocator.Instance.GetService<IAssemblySelectorDialog>();
                if (selector.ShowDialog(1))
                {
                    _externalAssembly.InitFromAssembly(selector.SelectedAssemblies[0], true);

                    // Vérification si il n'y a pas des relations orphelines
                    // TODO
                    transaction.Commit();
                }
            }

            return value;
        }

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
    }
}