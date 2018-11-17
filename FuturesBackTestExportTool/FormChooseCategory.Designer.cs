namespace FuturesBackTestExportTool
{
    partial class FormChooseCategory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChooseCategory));
            this.buttonNext = new System.Windows.Forms.Button();
            this.radioButtonStock = new System.Windows.Forms.RadioButton();
            this.radioButtonFutures = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(77, 46);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 5;
            this.buttonNext.Text = "下一步";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // radioButtonStock
            // 
            this.radioButtonStock.AutoSize = true;
            this.radioButtonStock.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButtonStock.Location = new System.Drawing.Point(137, 12);
            this.radioButtonStock.Name = "radioButtonStock";
            this.radioButtonStock.Size = new System.Drawing.Size(58, 20);
            this.radioButtonStock.TabIndex = 4;
            this.radioButtonStock.TabStop = true;
            this.radioButtonStock.Text = "股票";
            this.radioButtonStock.UseVisualStyleBackColor = true;
            // 
            // radioButtonFutures
            // 
            this.radioButtonFutures.AutoSize = true;
            this.radioButtonFutures.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButtonFutures.Location = new System.Drawing.Point(30, 12);
            this.radioButtonFutures.Name = "radioButtonFutures";
            this.radioButtonFutures.Size = new System.Drawing.Size(58, 20);
            this.radioButtonFutures.TabIndex = 3;
            this.radioButtonFutures.TabStop = true;
            this.radioButtonFutures.Text = "期货";
            this.radioButtonFutures.UseVisualStyleBackColor = true;
            // 
            // FormChooseCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 74);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.radioButtonStock);
            this.Controls.Add(this.radioButtonFutures);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChooseCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择品种大类";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.RadioButton radioButtonStock;
        private System.Windows.Forms.RadioButton radioButtonFutures;
    }
}