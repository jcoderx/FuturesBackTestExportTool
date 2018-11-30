namespace FuturesBackTestExportTool
{
    partial class FormStockChooseVariety
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStockChooseVariety));
            this.buttonNext = new System.Windows.Forms.Button();
            this.treeviewStockExchange = new FuturesBackTestExportTool.ThreeStateTreeview();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonNext.Location = new System.Drawing.Point(216, 292);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = "下一步";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // treeviewStockExchange
            // 
            this.treeviewStockExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeviewStockExchange.CheckBoxes = true;
            this.treeviewStockExchange.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeviewStockExchange.Font = new System.Drawing.Font("宋体", 11F);
            this.treeviewStockExchange.Location = new System.Drawing.Point(0, 0);
            this.treeviewStockExchange.Name = "treeviewStockExchange";
            this.treeviewStockExchange.Size = new System.Drawing.Size(492, 284);
            this.treeviewStockExchange.TabIndex = 0;
            // 
            // FormStockChooseVariety
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 320);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.treeviewStockExchange);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormStockChooseVariety";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择品种（股票）";
            this.Load += new System.EventHandler(this.FormStockChooseVariety_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ThreeStateTreeview treeviewStockExchange;
        private System.Windows.Forms.Button buttonNext;
    }
}