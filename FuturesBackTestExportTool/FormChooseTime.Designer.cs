namespace FuturesBackTestExportTool
{
    partial class FormChooseTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChooseTime));
            this.buttonBackTest = new System.Windows.Forms.Button();
            this.dateTimePickerEnding = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStarting = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonBackTest
            // 
            this.buttonBackTest.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonBackTest.ForeColor = System.Drawing.Color.Red;
            this.buttonBackTest.Location = new System.Drawing.Point(183, 74);
            this.buttonBackTest.Name = "buttonBackTest";
            this.buttonBackTest.Size = new System.Drawing.Size(87, 31);
            this.buttonBackTest.TabIndex = 9;
            this.buttonBackTest.Text = "开始回测";
            this.buttonBackTest.UseVisualStyleBackColor = true;
            this.buttonBackTest.Click += new System.EventHandler(this.buttonBackTest_Click);
            // 
            // dateTimePickerEnding
            // 
            this.dateTimePickerEnding.CustomFormat = "yyyy/MM/dd";
            this.dateTimePickerEnding.Font = new System.Drawing.Font("宋体", 11F);
            this.dateTimePickerEnding.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnding.Location = new System.Drawing.Point(139, 41);
            this.dateTimePickerEnding.Name = "dateTimePickerEnding";
            this.dateTimePickerEnding.Size = new System.Drawing.Size(131, 24);
            this.dateTimePickerEnding.TabIndex = 8;
            // 
            // dateTimePickerStarting
            // 
            this.dateTimePickerStarting.CustomFormat = "yyyy/MM/dd";
            this.dateTimePickerStarting.Font = new System.Drawing.Font("宋体", 11F);
            this.dateTimePickerStarting.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStarting.Location = new System.Drawing.Point(139, 11);
            this.dateTimePickerStarting.Name = "dateTimePickerStarting";
            this.dateTimePickerStarting.Size = new System.Drawing.Size(131, 24);
            this.dateTimePickerStarting.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11F);
            this.label2.Location = new System.Drawing.Point(9, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "信号计算结束时间";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "信号计算开始时间";
            // 
            // FormChooseTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 112);
            this.Controls.Add(this.buttonBackTest);
            this.Controls.Add(this.dateTimePickerEnding);
            this.Controls.Add(this.dateTimePickerStarting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择起止时间";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBackTest;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnding;
        private System.Windows.Forms.DateTimePicker dateTimePickerStarting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}