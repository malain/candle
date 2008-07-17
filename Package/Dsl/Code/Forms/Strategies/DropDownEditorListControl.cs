using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public class DropDownEditorListControl : UserControl
    {
        private readonly ListBox _listBox;
        private IWindowsFormsEditorService _edSvc;

        private string _selectedElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownEditorListControl"/> class.
        /// </summary>
        public DropDownEditorListControl()
        {
            _listBox = new ListBox();
            _listBox.Dock = DockStyle.Fill;
            _listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            Controls.Add(_listBox);
        }

        /// <summary>
        /// Gets or sets the selected element.
        /// </summary>
        /// <value>The selected element.</value>
        public string SelectedElement
        {
            get { return _selectedElement; }
            set { _selectedElement = value; }
        }

        /// <summary>
        /// Gets the width of the preferred.
        /// </summary>
        /// <value>The width of the preferred.</value>
        private int PreferredWidth
        {
            get
            {
                int preferredWidth = 0;
                Graphics graphics1 = _listBox.CreateGraphics();
                try
                {
                    foreach (string item in _listBox.Items)
                    {
                        Size size1 = Size.Ceiling(graphics1.MeasureString(item.ToString(), _listBox.Font));
                        preferredWidth = Math.Max(preferredWidth, size1.Width);
                    }
                }
                finally
                {
                    graphics1.Dispose();
                }
                return preferredWidth;
            }
        }

        /// <summary>
        /// Inits the items.
        /// </summary>
        /// <param name="currentValue">The current value.</param>
        /// <param name="editorService">The editor service.</param>
        /// <param name="values">The values.</param>
        internal void InitItems(string currentValue, IWindowsFormsEditorService editorService, IList<string> values)
        {
            _edSvc = editorService;
            _listBox.Items.Clear();

            foreach (string className in values)
            {
                int idx = _listBox.Items.Add(className);
                if (className == currentValue)
                    _listBox.SelectedIndex = idx;
            }

            _selectedElement = null;
            base.Width = Math.Max(base.Width, (int) (PreferredWidth + (SystemInformation.VerticalScrollBarWidth*2)));
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left"></see> property value of the control.</param>
        /// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top"></see> property value of the control.</param>
        /// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width"></see> property value of the control.</param>
        /// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height"></see> property value of the control.</param>
        /// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified"></see> values.</param>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if ((BoundsSpecified.Width & specified) == BoundsSpecified.Width)
            {
                width = Math.Max(width, 100);
            }
            if ((BoundsSpecified.Height & specified) == BoundsSpecified.Height)
            {
                height = Math.Max(height, 70);
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedElement = (string) _listBox.SelectedItem;
            _edSvc.CloseDropDown();
        }
    }
}