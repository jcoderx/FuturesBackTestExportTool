using FuturesBackTestExportTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    public partial class FormChooseCycle : Form
    {
        private List<TimeCycle> periods = new List<TimeCycle>();
        private List<TimeCycle> customPeriods = new List<TimeCycle>();
        public FormChooseCycle()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormAddCustomCycle formAddCustomPeriod = new FormAddCustomCycle();
            if (formAddCustomPeriod.ShowDialog() == DialogResult.OK)
            {
                TimeCycle period = formAddCustomPeriod.getResult();
                customPeriods.Add(period);
                refreshCustomPeriod();
            }
        }

        private void refreshCustomPeriod()
        {
            this.listBoxCustomPeriod.Items.Clear();
            foreach (TimeCycle period in customPeriods)
            {
                this.listBoxCustomPeriod.Items.Add(period.value + period.unit);
            }
        }

        private void buttonDelCustomPeriod_Click(object sender, EventArgs e)
        {
            int index = this.listBoxCustomPeriod.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("请选择要删除的自定义周期");
                return;
            }
            if (index < customPeriods.Count)
            {
                customPeriods.RemoveAt(index);
                refreshCustomPeriod();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            periods.Clear();

            List<CheckBox> checkBoxes = new List<CheckBox>();
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    checkBoxes.Add(cb);
                }
            }

            checkBoxes = checkBoxes.OrderBy(x => x.Tag).ToList();

            foreach (CheckBox cb in checkBoxes)
            {
                if (cb.Checked)
                {
                    TimeCycle period = new TimeCycle();
                    switch (cb.Text)
                    {
                        case "5秒":
                            period.value = 5;
                            period.unit = "秒";
                            periods.Add(period);
                            break;
                        case "10秒":
                            period.value = 10;
                            period.unit = "秒";
                            periods.Add(period);
                            break;
                        case "15秒":
                            period.value = 15;
                            period.unit = "秒";
                            periods.Add(period);
                            break;
                        case "30秒":
                            period.value = 30;
                            period.unit = "秒";
                            periods.Add(period);
                            break;
                        case "1分钟":
                            period.value = 1;
                            period.unit = "分钟";
                            periods.Add(period);
                            break;
                        case "3分钟":
                            period.value = 3;
                            period.unit = "分钟";
                            periods.Add(period);
                            break;
                        case "5分钟":
                            period.value = 5;
                            period.unit = "分钟";
                            periods.Add(period);
                            break;
                        case "10分钟":
                            period.value = 10;
                            period.unit = "分钟";
                            periods.Add(period);
                            break;
                        case "15分钟":
                            period.value = 15;
                            period.unit = "分钟";
                            periods.Add(period);
                            break;
                        case "30分钟":
                            period.value = 30;
                            period.unit = "分钟";
                            periods.Add(period);
                            break;
                        case "1小时":
                            period.value = 1;
                            period.unit = "小时";
                            periods.Add(period);
                            break;
                        case "2小时":
                            period.value = 2;
                            period.unit = "小时";
                            periods.Add(period);
                            break;
                        case "3小时":
                            period.value = 3;
                            period.unit = "小时";
                            periods.Add(period);
                            break;
                        case "4小时":
                            period.value = 4;
                            period.unit = "小时";
                            periods.Add(period);
                            break;
                        case "日线":
                            period.value = 0;
                            period.unit = "日线";
                            periods.Add(period);
                            break;
                        case "周线":
                            period.value = 0;
                            period.unit = "周线";
                            periods.Add(period);
                            break;
                        case "月线":
                            period.value = 0;
                            period.unit = "月线";
                            periods.Add(period);
                            break;
                        case "季线":
                            period.value = 0;
                            period.unit = "季线";
                            periods.Add(period);
                            break;
                        case "年线":
                            period.value = 0;
                            period.unit = "年线";
                            periods.Add(period);
                            break;
                    }
                }
            }
            periods.AddRange(customPeriods);
            if (periods.Count == 0)
            {
                MessageBox.Show("请选择周期");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<TimeCycle> getResult()
        {
            return periods;
        }
    }
}
