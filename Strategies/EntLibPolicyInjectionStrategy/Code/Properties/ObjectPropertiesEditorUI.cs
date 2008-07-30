using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    public partial class ObjectPropertiesEditorUI : UserControl
    {
        private bool canceled;
        private IWindowsFormsEditorService service;

        public ObjectPropertiesEditorUI(IWindowsFormsEditorService service, object value)
        {
            this.service = service;
            InitializeComponent();
            propertyGrid1.SelectedObject = value;
        }

        private void ObjectPropertiesEditorUI_Load(object sender, EventArgs e)
        {
        }

        public object ModifiedValue
        {
            get
            {
                if (canceled) return null;
                return propertyGrid1.SelectedObject;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            service.CloseDropDown();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            canceled = true;
            service.CloseDropDown();
        }
    }
}
