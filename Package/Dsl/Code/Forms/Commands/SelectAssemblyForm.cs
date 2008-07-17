using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SelectAssemblyForm : Form
    {
        private Assembly selectedAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAssemblyForm"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public SelectAssemblyForm(List<Assembly> assemblies)
        {
            InitializeComponent();

            foreach (Assembly asm in assemblies)
            {
                lstAssembly.Items.Add(new AssemblyItem(asm));
            }
        }

        /// <summary>
        /// Gets the selected assembly.
        /// </summary>
        /// <value>The selected assembly.</value>
        public Assembly SelectedAssembly
        {
            get { return selectedAssembly; }
        }

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (lstAssembly.CheckedItems.Count != 1)
            {
                MessageBox.Show("You must select one assembly.");
                return;
            }

            selectedAssembly = ((AssemblyItem) lstAssembly.CheckedItems[0]).Assembly;
            Hide();
        }

        #region Nested type: AssemblyItem

        /// <summary>
        /// 
        /// </summary>
        private class AssemblyItem
        {
            private readonly Assembly _assembly;

            /// <summary>
            /// Initializes a new instance of the <see cref="AssemblyItem"/> class.
            /// </summary>
            /// <param name="assembly">The assembly.</param>
            public AssemblyItem(Assembly assembly)
            {
                _assembly = assembly;
            }

            /// <summary>
            /// Gets the assembly.
            /// </summary>
            /// <value>The assembly.</value>
            public Assembly Assembly
            {
                get { return _assembly; }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return _assembly.GetName().FullName;
            }
        }

        #endregion
    }
}