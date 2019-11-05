namespace Quras_gui_SP.Controls
{
    partial class SideTxItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pan_split = new System.Windows.Forms.Panel();
            this.lbl_month = new System.Windows.Forms.Label();
            this.lbl_day = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_balance = new System.Windows.Forms.Label();
            this.lbl_asset = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pan_split
            // 
            this.pan_split.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(26)))), ((int)(((byte)(31)))));
            this.pan_split.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_split.Location = new System.Drawing.Point(0, 64);
            this.pan_split.Name = "pan_split";
            this.pan_split.Size = new System.Drawing.Size(193, 1);
            this.pan_split.TabIndex = 0;
            // 
            // lbl_month
            // 
            this.lbl_month.AutoSize = true;
            this.lbl_month.Font = new System.Drawing.Font("Microsoft Sans Serif", 4.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_month.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_month.Location = new System.Drawing.Point(13, 15);
            this.lbl_month.Name = "lbl_month";
            this.lbl_month.Size = new System.Drawing.Size(28, 12);
            this.lbl_month.TabIndex = 1;
            this.lbl_month.Text = "DEC";
            // 
            // lbl_day
            // 
            this.lbl_day.AutoSize = true;
            this.lbl_day.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_day.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_day.Location = new System.Drawing.Point(10, 27);
            this.lbl_day.Name = "lbl_day";
            this.lbl_day.Size = new System.Drawing.Size(36, 25);
            this.lbl_day.TabIndex = 2;
            this.lbl_day.Text = "20";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(45, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 33);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_balance
            // 
            this.lbl_balance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(111)))), ((int)(((byte)(84)))));
            this.lbl_balance.Location = new System.Drawing.Point(86, 15);
            this.lbl_balance.Name = "lbl_balance";
            this.lbl_balance.Size = new System.Drawing.Size(95, 18);
            this.lbl_balance.TabIndex = 4;
            this.lbl_balance.Text = "+1,104,345";
            this.lbl_balance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_asset
            // 
            this.lbl_asset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_asset.Font = new System.Drawing.Font("Microsoft Sans Serif", 4.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_asset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_asset.Location = new System.Drawing.Point(110, 33);
            this.lbl_asset.Name = "lbl_asset";
            this.lbl_asset.Size = new System.Drawing.Size(71, 15);
            this.lbl_asset.TabIndex = 5;
            this.lbl_asset.Text = "QRS";
            this.lbl_asset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SideTxItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(31)))), ((int)(((byte)(37)))));
            this.Controls.Add(this.lbl_asset);
            this.Controls.Add(this.lbl_balance);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbl_day);
            this.Controls.Add(this.lbl_month);
            this.Controls.Add(this.pan_split);
            this.Name = "SideTxItem";
            this.Size = new System.Drawing.Size(193, 65);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pan_split;
        private System.Windows.Forms.Label lbl_month;
        private System.Windows.Forms.Label lbl_day;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_balance;
        private System.Windows.Forms.Label lbl_asset;
    }
}
