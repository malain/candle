using System;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Forms;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Wizard
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DomainPathSelector : Form
    {
        private readonly DomainPathTreeView _treeView;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainPathSelector"/> class.
        /// </summary>
        /// <param name="domainFilter">The domain filter.</param>
        public DomainPathSelector(string domainFilter)
        {
            InitializeComponent();
            _treeView = new DomainPathTreeView(domainFilter);
            pnlBody.Controls.Add(_treeView);
            _treeView.Dock = DockStyle.Fill;
        }

        //internal void OnPathSelected(object sender, EventArgs e)
        //{
        //    this.btnOK.PerformClick();
        //}

        /// <summary>
        /// Gets the selected domain item.
        /// </summary>
        /// <value>The selected domain item.</value>
        public DomainItem SelectedDomainItem
        {
            get
            {
                if (SelectedPath != null)
                    return DomainManager.Instance.FindItem(SelectedPath);
                return null;
            }
        }

        /// <summary>
        /// Gets the selected path.
        /// </summary>
        /// <value>The selected path.</value>
        public string SelectedPath
        {
            get { return _treeView.SelectedPath; }
        }

        /// <summary>
        /// Handles the Click event of the btnName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnName_Click(object sender, EventArgs e)
        {
            PromptBox pbox = new PromptBox(GuiResources.NewName);
            if (pbox.ShowDialog() == DialogResult.OK)
            {
                bool ok = true;
                if (String.IsNullOrEmpty(pbox.Value))
                    ok = false;
                else
                {
                    foreach (char ch in pbox.Value)
                    {
                        char ch2 = Char.ToLower(ch);
                        if (!((ch2 >= 'a' && ch2 <= 'z') || (ch2 >= '0' && ch2 <= '9') || (" ._".IndexOf(ch) >= 0)))
                        {
                            ok = false;
                            break;
                        }
                    }
                }
                if (ok)
                {
                    _treeView.AddLevel(pbox.Value);
                }
                else
                    MessageBox.Show("Invalid name. (Only [a-z]|[0-9]|[ ._])");
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            _treeView.DeleteCurrentLevel();
        }
    }
}