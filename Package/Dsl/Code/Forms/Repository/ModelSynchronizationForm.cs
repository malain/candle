using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ModelSynchronizationForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSynchronizationForm"/> class.
        /// </summary>
        public ModelSynchronizationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets a value indicating whether [server version selected].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [server version selected]; otherwise, <c>false</c>.
        /// </value>
        public bool ServerVersionSelected
        {
            get { return rbServer.Checked; }
        }
    }
}