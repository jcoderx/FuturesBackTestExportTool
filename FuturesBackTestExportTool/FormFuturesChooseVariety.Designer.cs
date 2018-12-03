namespace FuturesBackTestExportTool
{
    partial class FormFuturesChooseVariety
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFuturesChooseVariety));
            this.treeviewExchange = new FuturesBackTestExportTool.ThreeStateTreeview();
            this.buttonNext = new System.Windows.Forms.Button();
            this.checkBoxIndex = new System.Windows.Forms.CheckBox();
            this.checkBoxDominant = new System.Windows.Forms.CheckBox();
            this.checkBoxContinuous = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // treeviewExchange
            // 
            this.treeviewExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeviewExchange.CheckBoxes = true;
            this.treeviewExchange.Font = new System.Drawing.Font("宋体", 11F);
            this.treeviewExchange.Location = new System.Drawing.Point(0, 38);
            this.treeviewExchange.Name = "treeviewExchange";
            this.treeviewExchange.Size = new System.Drawing.Size(492, 243);
            this.treeviewExchange.TabIndex = 0;
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonNext.Location = new System.Drawing.Point(208, 289);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "下一步";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // checkBoxIndex
            // 
            this.checkBoxIndex.AutoSize = true;
            this.checkBoxIndex.Location = new System.Drawing.Point(86, 12);
            this.checkBoxIndex.Name = "checkBoxIndex";
            this.checkBoxIndex.Size = new System.Drawing.Size(48, 16);
            this.checkBoxIndex.TabIndex = 3;
            this.checkBoxIndex.Text = "指数";
            this.checkBoxIndex.UseVisualStyleBackColor = true;
            this.checkBoxIndex.CheckedChanged += new System.EventHandler(this.checkBoxIndex_CheckedChanged);
            // 
            // checkBoxDominant
            // 
            this.checkBoxDominant.AutoSize = true;
            this.checkBoxDominant.Location = new System.Drawing.Point(199, 12);
            this.checkBoxDominant.Name = "checkBoxDominant";
            this.checkBoxDominant.Size = new System.Drawing.Size(48, 16);
            this.checkBoxDominant.TabIndex = 3;
            this.checkBoxDominant.Text = "主力";
            this.checkBoxDominant.UseVisualStyleBackColor = true;
            this.checkBoxDominant.CheckedChanged += new System.EventHandler(this.checkBoxDominant_CheckedChanged);
            // 
            // checkBoxContinuous
            // 
            this.checkBoxContinuous.AutoSize = true;
            this.checkBoxContinuous.Location = new System.Drawing.Point(312, 12);
            this.checkBoxContinuous.Name = "checkBoxContinuous";
            this.checkBoxContinuous.Size = new System.Drawing.Size(48, 16);
            this.checkBoxContinuous.TabIndex = 3;
            this.checkBoxContinuous.Text = "连续";
            this.checkBoxContinuous.UseVisualStyleBackColor = true;
            this.checkBoxContinuous.CheckedChanged += new System.EventHandler(this.checkBoxContinuous_CheckedChanged);
            // 
            // FormFuturesChooseVariety
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 320);
            this.Controls.Add(this.checkBoxContinuous);
            this.Controls.Add(this.checkBoxDominant);
            this.Controls.Add(this.checkBoxIndex);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.treeviewExchange);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFuturesChooseVariety";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择品种（期货）";
            this.Load += new System.EventHandler(this.FormFuturesChooseVariety_Load);
            this.Shown += new System.EventHandler(this.FormFuturesChooseVariety_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ThreeStateTreeview treeviewExchange;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.CheckBox checkBoxIndex;
        private System.Windows.Forms.CheckBox checkBoxDominant;
        private System.Windows.Forms.CheckBox checkBoxContinuous;
    }
}