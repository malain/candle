using System;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Dependencies;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ShowDependenciesForm : Form
    {
        private readonly ModelElement _system;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowDependenciesForm"/> class.
        /// </summary>
        /// <param name="system">The system.</param>
        public ShowDependenciesForm(ModelElement system)
        {
            InitializeComponent();
            _system = system;
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ReferenceScope scope = GetReferenceScope();
                lstArtefacts.Items.Clear();

                ConfigurationMode mode = new ConfigurationMode(cbMode.Text);
                ReferenceWalker w = new ReferenceWalker(scope, mode);
                string folder = null;
                if ((string) cbAction.SelectedItem == "current solution")
                    folder = @"c:\[sln]\";
                ReferenceVisitor rv = new ReferenceVisitor(scope, folder);
                w.Traverse(rv, _system);
//                ReferenceContext context = new ReferenceContext(mode, scope, (ReferenceContext.ReferenceSource)cbAction.SelectedItem, CandleModel.GetInstance(((ModelElement)system).Store), "project folder\\..\\");
                foreach (string item in rv.References)
                {
                    lstArtefacts.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Gets the reference scope.
        /// </summary>
        /// <returns></returns>
        private ReferenceScope GetReferenceScope()
        {
            if (rbCompilation.Checked)
                return ReferenceScope.Compilation;
            if (rbRuntime.Checked)
                return ReferenceScope.Runtime;

            return ReferenceScope.Publish;
        }

        /// <summary>
        /// Handles the Load event of the ShowDependenciesForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ShowDependenciesForm_Load(object sender, EventArgs e)
        {
            SolutionConfigurations configurations = null;
            if (ServiceLocator.Instance.ShellHelper.Solution != null &&
                ServiceLocator.Instance.ShellHelper.Solution.SolutionBuild != null)
            {
                configurations = ServiceLocator.Instance.ShellHelper.Solution.SolutionBuild.SolutionConfigurations;
            }

            if (configurations != null && configurations.Count > 0)
            {
                for (int i = 0; i < configurations.Count; i++)
                {
                    cbMode.Items.Add(configurations.Item(i + 1).Name);
                }
            }
            else
            {
                cbMode.Items.Add("*");
            }
            cbMode.SelectedIndex = 0;

            //   cbAction.Items.Add( ReferenceContext.Action.FromLatest );
            cbAction.Items.Add("Repository");
            cbAction.Items.Add("current solution");
            cbAction.SelectedIndex = 1;
        }
    }
}