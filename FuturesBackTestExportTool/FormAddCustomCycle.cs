using FuturesBackTestExportTool.Model;
using System;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    public partial class FormAddCustomCycle : Form
    {
        private TimeCycle period;

        public FormAddCustomCycle()
        {
            InitializeComponent();
            comboBoxUnit.SelectedIndex = 0;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int value = (int)this.numericUpDownPeriod.Value;
            int index = this.comboBoxUnit.SelectedIndex;
            string unit = "秒";
            switch (index)
            {
                case 0:
                    unit = "秒";
                    break;
                case 1:
                    unit = "分钟";
                    break;
                case 2:
                    unit = "小时";
                    break;
                case 3:
                    unit = "日";
                    break;
            }

            period = new TimeCycle();
            period.value = value;
            period.unit = unit;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public TimeCycle getResult()
        {
            return period;
        }
    }
}
