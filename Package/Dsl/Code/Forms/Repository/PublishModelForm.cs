using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PublishModelForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishModelForm"/> class.
        /// </summary>
        /// <param name="serverAvailable">if set to <c>true</c> [server available].</param>
        public PublishModelForm(bool serverAvailable)
        {
            InitializeComponent();
            chkServer.Visible = serverAvailable;
        }

        /// <summary>
        /// Gets a value indicating whether [publish on server].
        /// </summary>
        /// <value><c>true</c> if [publish on server]; otherwise, <c>false</c>.</value>
        public bool PublishOnServer
        {
            get { return chkServer.Checked; }
        }
    }
}