namespace CodingProject1
{
    partial class FRMInventory
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
            this.LBXInventory = new System.Windows.Forms.ListBox();
            this.BTNRefill = new System.Windows.Forms.Button();
            this.BTNClose = new System.Windows.Forms.Button();
            this.LBLTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LBXInventory
            // 
            this.LBXInventory.FormattingEnabled = true;
            this.LBXInventory.ItemHeight = 16;
            this.LBXInventory.Location = new System.Drawing.Point(11, 87);
            this.LBXInventory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LBXInventory.Name = "LBXInventory";
            this.LBXInventory.Size = new System.Drawing.Size(596, 308);
            this.LBXInventory.TabIndex = 0;
            // 
            // BTNRefill
            // 
            this.BTNRefill.Location = new System.Drawing.Point(11, 428);
            this.BTNRefill.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BTNRefill.Name = "BTNRefill";
            this.BTNRefill.Size = new System.Drawing.Size(241, 46);
            this.BTNRefill.TabIndex = 1;
            this.BTNRefill.Text = "Refill Inventory";
            this.BTNRefill.UseVisualStyleBackColor = true;
            this.BTNRefill.Click += new System.EventHandler(this.BTNRefill_Click);
            // 
            // BTNClose
            // 
            this.BTNClose.Location = new System.Drawing.Point(356, 428);
            this.BTNClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BTNClose.Name = "BTNClose";
            this.BTNClose.Size = new System.Drawing.Size(252, 46);
            this.BTNClose.TabIndex = 2;
            this.BTNClose.Text = "Close";
            this.BTNClose.UseVisualStyleBackColor = true;
            this.BTNClose.Click += new System.EventHandler(this.BTNClose_Click);
            // 
            // LBLTime
            // 
            this.LBLTime.AutoSize = true;
            this.LBLTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBLTime.Location = new System.Drawing.Point(16, 38);
            this.LBLTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LBLTime.Name = "LBLTime";
            this.LBLTime.Size = new System.Drawing.Size(64, 25);
            this.LBLTime.TabIndex = 3;
            this.LBLTime.Text = "label1";
            // 
            // FRMInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 549);
            this.Controls.Add(this.LBLTime);
            this.Controls.Add(this.BTNClose);
            this.Controls.Add(this.BTNRefill);
            this.Controls.Add(this.LBXInventory);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FRMInventory";
            this.Text = "Inventory";
            this.Load += new System.EventHandler(this.FRMInventory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LBXInventory;
        private System.Windows.Forms.Button BTNRefill;
        private System.Windows.Forms.Button BTNClose;
        private System.Windows.Forms.Label LBLTime;
    }
}