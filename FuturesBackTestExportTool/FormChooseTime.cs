using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    //选择起止时间界面
    public partial class FormChooseTime : Form
    {
        private DateTime dtStarting;
        private DateTime dtEnding;
        public FormChooseTime()
        {
            InitializeComponent();
        }

        private void buttonBackTest_Click(object sender, EventArgs e)
        {
            dtStarting = this.dateTimePickerStarting.Value;
            dtEnding = this.dateTimePickerEnding.Value;
            if (Utils.compareDate(dtStarting, dtEnding) > 0)
            {
                MessageBox.Show("开始时间大于结束时间，请重新设置");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public DateTime[] getResult()
        {
            DateTime[] result = new DateTime[2];
            result[0] = dtStarting;
            result[1] = dtEnding;
            return result;
        }
    }
}
