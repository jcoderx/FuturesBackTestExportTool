using FuturesBackTestExportTool.Page;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using static FuturesBackTestExportTool.WindowsApi;


namespace FuturesBackTestExportTool
{
    public partial class Form1 : Form
    {
        private IntPtr mainHandle;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string m = "模型123 RUMI";
            string m2 = "模型1234";

            MessageBox.Show(m.Replace(" RUMI","")+":::::"+m2.Replace(" RUMI",""));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("SHOWN");
        }
    }
}
