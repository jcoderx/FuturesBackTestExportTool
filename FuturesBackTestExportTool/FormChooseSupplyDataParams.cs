using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    public partial class FormChooseSupplyDataParams : Form
    {
        private List<string> cycles;
        private DateTime dtFrom;

        public FormChooseSupplyDataParams()
        {
            InitializeComponent();
        }

        private void buttonSupplyData_Click(object sender, EventArgs e)
        {
            cycles = new List<string>();
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    if (cb.Checked)
                    {
                        cycles.Add(cb.Text);
                    }
                }
            }
            if (cycles.Count == 0)
            {
                MessageBox.Show("请选择周期");
                return;
            }
            dtFrom = this.dateTimePickerFrom.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public object[] getResult()
        {
            object[] result = new object[2];
            result[0] = cycles;
            result[1] = dtFrom;
            return result;
        }
    }
}
