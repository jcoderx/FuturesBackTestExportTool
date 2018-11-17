namespace FuturesBackTestExportTool
{
    partial class FormChooseSupplyDataParams
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChooseSupplyDataParams));
            this.checkBox1D = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSupplyData = new System.Windows.Forms.Button();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox15M = new System.Windows.Forms.CheckBox();
            this.checkBox1M = new System.Windows.Forms.CheckBox();
            this.checkBox1S = new System.Windows.Forms.CheckBox();
            this.checkBoxTICK = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox1D
            // 
            this.checkBox1D.AutoSize = true;
            this.checkBox1D.Location = new System.Drawing.Point(302, 45);
            this.checkBox1D.Name = "checkBox1D";
            this.checkBox1D.Size = new System.Drawing.Size(42, 16);
            this.checkBox1D.TabIndex = 28;
            this.checkBox1D.Text = "1日";
            this.checkBox1D.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(0, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 2);
            this.label3.TabIndex = 32;
            // 
            // buttonSupplyData
            // 
            this.buttonSupplyData.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSupplyData.ForeColor = System.Drawing.Color.Red;
            this.buttonSupplyData.Location = new System.Drawing.Point(134, 140);
            this.buttonSupplyData.Name = "buttonSupplyData";
            this.buttonSupplyData.Size = new System.Drawing.Size(98, 27);
            this.buttonSupplyData.TabIndex = 31;
            this.buttonSupplyData.Text = "补充数据";
            this.buttonSupplyData.UseVisualStyleBackColor = true;
            this.buttonSupplyData.Click += new System.EventHandler(this.buttonSupplyData_Click);
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.CustomFormat = "yyyy/MM/dd";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(120, 95);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(129, 21);
            this.dateTimePickerFrom.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "历史数据起始日期";
            // 
            // checkBox15M
            // 
            this.checkBox15M.AutoSize = true;
            this.checkBox15M.Location = new System.Drawing.Point(224, 45);
            this.checkBox15M.Name = "checkBox15M";
            this.checkBox15M.Size = new System.Drawing.Size(60, 16);
            this.checkBox15M.TabIndex = 27;
            this.checkBox15M.Text = "15分钟";
            this.checkBox15M.UseVisualStyleBackColor = true;
            // 
            // checkBox1M
            // 
            this.checkBox1M.AutoSize = true;
            this.checkBox1M.Location = new System.Drawing.Point(151, 45);
            this.checkBox1M.Name = "checkBox1M";
            this.checkBox1M.Size = new System.Drawing.Size(54, 16);
            this.checkBox1M.TabIndex = 26;
            this.checkBox1M.Text = "1分钟";
            this.checkBox1M.UseVisualStyleBackColor = true;
            // 
            // checkBox1S
            // 
            this.checkBox1S.AutoSize = true;
            this.checkBox1S.Location = new System.Drawing.Point(81, 45);
            this.checkBox1S.Name = "checkBox1S";
            this.checkBox1S.Size = new System.Drawing.Size(54, 16);
            this.checkBox1S.TabIndex = 25;
            this.checkBox1S.Text = "1秒钟";
            this.checkBox1S.UseVisualStyleBackColor = true;
            // 
            // checkBoxTICK
            // 
            this.checkBoxTICK.AutoSize = true;
            this.checkBoxTICK.Location = new System.Drawing.Point(14, 45);
            this.checkBoxTICK.Name = "checkBoxTICK";
            this.checkBoxTICK.Size = new System.Drawing.Size(48, 16);
            this.checkBoxTICK.TabIndex = 24;
            this.checkBoxTICK.Text = "TICK";
            this.checkBoxTICK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "历史数据周期";
            // 
            // FormChooseSupplyDataParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 180);
            this.Controls.Add(this.checkBox1D);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSupplyData);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox15M);
            this.Controls.Add(this.checkBox1M);
            this.Controls.Add(this.checkBox1S);
            this.Controls.Add(this.checkBoxTICK);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseSupplyDataParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择补充历史数据参数";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1D;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSupplyData;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox15M;
        private System.Windows.Forms.CheckBox checkBox1M;
        private System.Windows.Forms.CheckBox checkBox1S;
        private System.Windows.Forms.CheckBox checkBoxTICK;
        private System.Windows.Forms.Label label1;
    }
}