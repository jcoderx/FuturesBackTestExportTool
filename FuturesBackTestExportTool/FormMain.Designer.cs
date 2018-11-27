namespace FuturesBackTestExportTool
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonBackTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSupplyData = new System.Windows.Forms.Button();
            this.buttonOpenExcelDir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AllowUserToAddRows = false;
            this.dataGridViewResult.AllowUserToDeleteRows = false;
            this.dataGridViewResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataGridViewResult.Location = new System.Drawing.Point(0, 132);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.ReadOnly = true;
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.RowTemplate.Height = 23;
            this.dataGridViewResult.Size = new System.Drawing.Size(800, 317);
            this.dataGridViewResult.TabIndex = 8;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "行号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "第一列";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "第二列";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 400;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "第三列";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 200;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "第四列";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 200;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "第五列";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 500;
            // 
            // buttonBackTest
            // 
            this.buttonBackTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBackTest.Location = new System.Drawing.Point(665, 50);
            this.buttonBackTest.Name = "buttonBackTest";
            this.buttonBackTest.Size = new System.Drawing.Size(121, 30);
            this.buttonBackTest.TabIndex = 6;
            this.buttonBackTest.Text = "回测";
            this.buttonBackTest.UseVisualStyleBackColor = true;
            this.buttonBackTest.Click += new System.EventHandler(this.buttonBackTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(587, 80);
            this.label1.TabIndex = 9;
            this.label1.Text = "1.补充数据和回测过程较长，避免电脑进入睡眠或休眠状态。\r\n2.回测之前将智赢程序化客户端在屏幕前台显示，仅保留智赢程序化主界面。\r\n3.回测之前请补充数据，补充" +
    "数据时保证网络畅通。\r\n4.开始回测后，不要再操作电脑，尽量避免其它软件抢占焦点。\r\n5.回测过程比较消耗内存，尽可能少的打开软件。";
            // 
            // buttonSupplyData
            // 
            this.buttonSupplyData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSupplyData.Location = new System.Drawing.Point(665, 9);
            this.buttonSupplyData.Name = "buttonSupplyData";
            this.buttonSupplyData.Size = new System.Drawing.Size(121, 30);
            this.buttonSupplyData.TabIndex = 5;
            this.buttonSupplyData.Text = "补充数据";
            this.buttonSupplyData.UseVisualStyleBackColor = true;
            this.buttonSupplyData.Click += new System.EventHandler(this.buttonSupplyData_Click);
            // 
            // buttonOpenExcelDir
            // 
            this.buttonOpenExcelDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenExcelDir.Location = new System.Drawing.Point(665, 91);
            this.buttonOpenExcelDir.Name = "buttonOpenExcelDir";
            this.buttonOpenExcelDir.Size = new System.Drawing.Size(121, 30);
            this.buttonOpenExcelDir.TabIndex = 7;
            this.buttonOpenExcelDir.Text = "打开excel目录";
            this.buttonOpenExcelDir.UseVisualStyleBackColor = true;
            this.buttonOpenExcelDir.Click += new System.EventHandler(this.buttonOpenExcelDir_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewResult);
            this.Controls.Add(this.buttonBackTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSupplyData);
            this.Controls.Add(this.buttonOpenExcelDir);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "期货回测导出工具";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Button buttonBackTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSupplyData;
        private System.Windows.Forms.Button buttonOpenExcelDir;
    }
}