using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static FuturesBackTestExportTool.WindowsApi;

namespace FuturesBackTestExportTool
{
    public partial class FormChooseClient : Form
    {
        private IntPtr handle = IntPtr.Zero;

        public FormChooseClient(List<IntPtr> wndHandles)
        {
            InitializeComponent();
            List<string> allPaths = new List<string>();
            foreach (IntPtr wnd in wndHandles)
            {
                int processId;
                GetWindowThreadProcessId(wnd, out processId);
                Process process = Process.GetProcessById(processId);
                string fileName = process.MainModule.FileName;
                allPaths.Add(Utils.cutDirName(fileName));
            }
            for (int i = 0; i < allPaths.Count; i++)
            {
                string path = allPaths[i];
                RadioButton rb = new RadioButton();
                rb.Parent = this.panelChooseClient;
                rb.AutoSize = true;
                rb.Text = path;
                rb.Tag = wndHandles[i];
                Font font = new Font("宋体", 12);
                rb.Font = font;
                Point point = new Point(16, 10 + 25 * i);
                rb.Location = point;
                this.panelChooseClient.Controls.Add(rb);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            bool choosed = false;
            foreach (Control c in this.panelChooseClient.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Checked)
                    {
                        choosed = true;
                        handle = (IntPtr)rb.Tag;
                        break;
                    }
                }
            }
            if (choosed)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择要操作的客户端");
            }
        }

        public IntPtr getResult()
        {
            return handle;
        }
    }
}
