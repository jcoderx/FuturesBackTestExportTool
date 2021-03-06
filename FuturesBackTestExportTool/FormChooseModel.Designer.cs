﻿namespace FuturesBackTestExportTool
{
    partial class FormChooseModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChooseModel));
            this.treeviewModel = new FuturesBackTestExportTool.ThreeStateTreeview();
            this.buttonNext = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeviewModel
            // 
            this.treeviewModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeviewModel.CheckBoxes = true;
            this.treeviewModel.Font = new System.Drawing.Font("宋体", 11F);
            this.treeviewModel.Location = new System.Drawing.Point(0, 0);
            this.treeviewModel.Name = "treeviewModel";
            this.treeviewModel.Size = new System.Drawing.Size(400, 240);
            this.treeviewModel.TabIndex = 0;
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonNext.Location = new System.Drawing.Point(161, 248);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(85, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "下一步";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // FormChooseModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 279);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.treeviewModel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormChooseModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择模型";
            this.Load += new System.EventHandler(this.FormChooseModel_Load);
            this.Shown += new System.EventHandler(this.FormChooseModel_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private ThreeStateTreeview treeviewModel;
        private System.Windows.Forms.Button buttonNext;
    }
}