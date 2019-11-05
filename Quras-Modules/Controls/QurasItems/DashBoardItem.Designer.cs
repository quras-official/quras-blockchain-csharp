namespace DT_GUI_Modules.Controls.QurasItems
{
    partial class DashBoardItem
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
            this.lbl_address = new System.Windows.Forms.Label();
            this.lbl_qrs_amount = new System.Windows.Forms.Label();
            this.lbl_usd_amount = new System.Windows.Forms.Label();
            this.btn_addr_copy = new DT_GUI_Modules.Controls.GlowButton();
            this.btn_qr_code = new DT_GUI_Modules.Controls.GlowButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_address
            // 
            this.lbl_address.BackColor = System.Drawing.Color.Transparent;
            this.lbl_address.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_address.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_address.ForeColor = System.Drawing.Color.White;
            this.lbl_address.Location = new System.Drawing.Point(17, 10);
            this.lbl_address.Name = "lbl_address";
            this.lbl_address.Size = new System.Drawing.Size(304, 31);
            this.lbl_address.TabIndex = 0;
            this.lbl_address.Text = "dmwe8ewfh394hf9rhf930f983h93";
            // 
            // lbl_qrs_amount
            // 
            this.lbl_qrs_amount.BackColor = System.Drawing.Color.Transparent;
            this.lbl_qrs_amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qrs_amount.ForeColor = System.Drawing.Color.White;
            this.lbl_qrs_amount.Location = new System.Drawing.Point(16, 41);
            this.lbl_qrs_amount.Name = "lbl_qrs_amount";
            this.lbl_qrs_amount.Size = new System.Drawing.Size(247, 34);
            this.lbl_qrs_amount.TabIndex = 1;
            this.lbl_qrs_amount.Text = "QRS 13";
            // 
            // lbl_usd_amount
            // 
            this.lbl_usd_amount.AutoSize = true;
            this.lbl_usd_amount.BackColor = System.Drawing.Color.Transparent;
            this.lbl_usd_amount.ForeColor = System.Drawing.Color.White;
            this.lbl_usd_amount.Location = new System.Drawing.Point(18, 85);
            this.lbl_usd_amount.Name = "lbl_usd_amount";
            this.lbl_usd_amount.Size = new System.Drawing.Size(40, 17);
            this.lbl_usd_amount.TabIndex = 2;
            this.lbl_usd_amount.Text = "$260";
            // 
            // btn_addr_copy
            // 
            this.btn_addr_copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_addr_copy.BackgroundImage = global::DT_GUI_Modules.Properties.Resources.copy;
            this.btn_addr_copy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_addr_copy.FlatAppearance.BorderSize = 0;
            this.btn_addr_copy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_addr_copy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_addr_copy.GlowColor = System.Drawing.Color.Silver;
            this.btn_addr_copy.Location = new System.Drawing.Point(379, 11);
            this.btn_addr_copy.Name = "btn_addr_copy";
            this.btn_addr_copy.Size = new System.Drawing.Size(30, 30);
            this.btn_addr_copy.TabIndex = 5;
            this.btn_addr_copy.UseVisualStyleBackColor = true;
            this.btn_addr_copy.Click += new System.EventHandler(this.btn_addr_copy_Click);
            // 
            // btn_qr_code
            // 
            this.btn_qr_code.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_qr_code.BackgroundImage = global::DT_GUI_Modules.Properties.Resources.qr_code_button;
            this.btn_qr_code.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_qr_code.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_qr_code.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_qr_code.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_qr_code.GlowColor = System.Drawing.Color.Silver;
            this.btn_qr_code.Location = new System.Drawing.Point(379, 75);
            this.btn_qr_code.Name = "btn_qr_code";
            this.btn_qr_code.Size = new System.Drawing.Size(30, 30);
            this.btn_qr_code.TabIndex = 3;
            this.btn_qr_code.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(418, 116);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // DashBoardItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.btn_addr_copy);
            this.Controls.Add(this.btn_qr_code);
            this.Controls.Add(this.lbl_usd_amount);
            this.Controls.Add(this.lbl_qrs_amount);
            this.Controls.Add(this.lbl_address);
            this.Controls.Add(this.pictureBox1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "DashBoardItem";
            this.Size = new System.Drawing.Size(418, 116);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_address;
        private System.Windows.Forms.Label lbl_qrs_amount;
        private System.Windows.Forms.Label lbl_usd_amount;
        private GlowButton btn_qr_code;
        private System.Windows.Forms.PictureBox pictureBox1;
        private GlowButton btn_addr_copy;
    }
}
