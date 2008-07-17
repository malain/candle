using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OperationSelectorDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSelectorDlg"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public OperationSelectorDlg(TypeWithOperations type)
        {
            InitializeComponent();
            foreach (Operation op in type.Operations)
            {
                lstOperations.Items.Add(op);
            }
        }

        /// <summary>
        /// Gets the selected operation.
        /// </summary>
        /// <value>The selected operation.</value>
        public Operation SelectedOperation
        {
            get { return lstOperations.SelectedItem as Operation; }
        }
    }
}