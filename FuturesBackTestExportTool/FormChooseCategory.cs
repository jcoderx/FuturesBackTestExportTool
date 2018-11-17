using System;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    public partial class FormChooseCategory : Form
    {
        private int category = 0;

        public FormChooseCategory()
        {
            InitializeComponent();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (this.radioButtonFutures.Checked)
            {
                category = 0;
            }
            else if (this.radioButtonStock.Checked)
            {
                category = 1;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int getResult()
        {
            return category;
        }
    }
}
