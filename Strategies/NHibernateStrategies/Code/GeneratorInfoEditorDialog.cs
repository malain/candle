using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Strategies.NHibernate
{
    public partial class GeneratorInfoEditorDialog : Form
    {
        private GeneratorInfo generatorInfo;
        private List<GeneratorInfo.GeneratorParm> parms;

        public GeneratorInfoEditorDialog(GeneratorInfo generatorInfo)
        {
            this.generatorInfo = generatorInfo;
            InitializeComponent();
        }

        public GeneratorInfo Value
        {
            get { return generatorInfo; }
        }

        private void GeneratorInfoEditorDialog_Load( object sender, EventArgs e )
        {
            parms = new List<GeneratorInfo.GeneratorParm>( generatorInfo.Parms );
            if( parms.Count > 0 )
                dgParams.Rows.Add(parms.Count);

            if( generatorInfo.Name != null )
            {
                int i = cbName.Items.IndexOf( generatorInfo.Name );
                if( i >= 0 )
                    cbName.SelectedIndex = i;
            }
        }

        private void btnOK_Click( object sender, EventArgs e )
        {
            generatorInfo = new GeneratorInfo();
            if( cbName.SelectedItem == null )
                generatorInfo.Name = string.Empty;
            else
                generatorInfo.Name = cbName.SelectedItem.ToString();
            generatorInfo.Parms = new List<GeneratorInfo.GeneratorParm>( parms );
        }

        private void btnAdd_Click( object sender, EventArgs e )
        {
            parms.Add( new GeneratorInfo.GeneratorParm() );
            dgParams.Rows.Add();
        }

        private void dgParams_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < parms.Count)
            {
                GeneratorInfo.GeneratorParm parm = (GeneratorInfo.GeneratorParm)parms[e.RowIndex];
                DataGridViewCell cell = dgParams.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == 0)
                    parm.Name = (string)cell.EditedFormattedValue;
                else
                    parm.Value = (string)cell.EditedFormattedValue;
            }
        }

        private void dgParams_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < parms.Count)
            {
                GeneratorInfo.GeneratorParm parm = (GeneratorInfo.GeneratorParm)parms[e.RowIndex];
                if (e.ColumnIndex == 0)
                    e.Value = parm.Name;
                else
                    e.Value = parm.Value;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgParams.SelectedRows.Count > 0)
            {
                parms.RemoveAt(dgParams.SelectedRows[0].Index);
                dgParams.Rows.Remove(dgParams.SelectedRows[0]);
            }
        }
    }
}