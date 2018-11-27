using System;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    //是否支持逐笔回测界面
    public partial class FormChooseGraduallyBackTest : Form
    {
        private bool isGraduallyBackTest = true;
        public FormChooseGraduallyBackTest()
        {
            InitializeComponent();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (this.radioButtonYes.Checked)
            {
                isGraduallyBackTest = true;
            }
            else if (this.radioButtonNo.Checked)
            {
                isGraduallyBackTest = false;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public bool getResult()
        {
            return isGraduallyBackTest;
        }
    }
}
