using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Commands.Reverse
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ReverseModelsForm : Form
    {
        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        public delegate bool FilterTypeHandler(Type t);

        #endregion

        private readonly FilterTypeHandler filterCallback;

        private string nameSpace;
        private List<Assembly> referencedAssemblies = new List<Assembly>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseModelsForm"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public ReverseModelsForm(FilterTypeHandler callback)
        {
            filterCallback = callback;
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the referenced assemblies.
        /// </summary>
        /// <value>The referenced assemblies.</value>
        public List<Assembly> ReferencedAssemblies
        {
            get { return referencedAssemblies; }
            set { referencedAssemblies = value; }
        }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath
        {
            get { return Path.GetDirectoryName(textBox1.Text); }
        }

        /// <summary>
        /// Gets the selected classes.
        /// </summary>
        /// <value>The selected classes.</value>
        public IList SelectedClasses
        {
            get
            {
                List<Type> results = new List<Type>();
                RetrieveSelectedTypes(treeView1.Nodes, results);
                return results;
            }
        }

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                btnImport.Visible = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the button4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                panel3.Visible = false;
                ShowClasses(textBox1.Text);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                lstMessages.Items.Clear();
                foreach (Exception ex in rtle.LoaderExceptions)
                    lstMessages.Items.Add(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the selected types.
        /// </summary>
        /// <param name="childs">The childs.</param>
        /// <param name="results">The results.</param>
        private static void RetrieveSelectedTypes(TreeNodeCollection childs, List<Type> results)
        {
            foreach (TreeNode node in childs)
            {
                if (node.Tag != null && node.Checked)
                    results.Add((Type) node.Tag);
                if (node.Nodes.Count > 0)
                    RetrieveSelectedTypes(node.Nodes, results);
            }
        }

        /// <summary>
        /// Affichage des classes
        /// </summary>
        /// <param name="assemblyFullName">Full name of the assembly.</param>
        private void ShowClasses(string assemblyFullName)
        {
            treeView1.Nodes.Clear();

            try
            {
                Assembly assembly = Assembly.ReflectionOnlyLoadFrom(assemblyFullName);
                GetTypesRecursive(assembly, true);
                btnOK.Visible = true;
                treeView1.Visible = true;
            }
            catch
            {
                btnOK.Visible = false;
                panel3.Visible = true;
                MessageBox.Show("Unable to load assembly " + assemblyFullName);
                throw;
            }
        }

        /// <summary>
        /// Récupération des types d'un assembly
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="mainAssembly">if set to <c>true</c> [main assembly].</param>
        private void GetTypesRecursive(Assembly assembly, bool mainAssembly)
        {
            TreeNode root = new TreeNode(assembly.GetName().Name);
            root.Checked = mainAssembly;
            treeView1.Nodes.Add(root);
            using (AssemblyResolver resolver = new AssemblyResolver(Path.GetDirectoryName(assembly.Location)))
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (filterCallback(type))
                    {
                        nameSpace = type.Namespace;
                        TreeNode typeNode = new TreeNode(type.ToString());
                        typeNode.Checked = mainAssembly;
                        typeNode.Tag = type;
                        root.Nodes.Add(typeNode);
                    }
                }

                referencedAssemblies.AddRange(resolver.ReferencedAssemblies);

                foreach (Assembly asm in resolver.ReferencedAssemblies)
                    GetTypesRecursive(asm, false);
            }
            if (mainAssembly)
                root.Expand();
        }

        /// <summary>
        /// Inits the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        internal void Init(string path)
        {
            textBox1.Text = path;
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnImport.Visible = textBox1.Text.Length > 0;
        }

        /// <summary>
        /// Handles the AfterCheck event of the treeView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode child in e.Node.Nodes)
            {
                child.Checked = e.Node.Checked;
            }
        }
    }
}