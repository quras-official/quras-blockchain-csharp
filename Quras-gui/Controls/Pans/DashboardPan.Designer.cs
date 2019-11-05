namespace Quras_gui.Controls.Pans
{
    partial class DashboardPan
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
            this.lbl_qrs = new System.Windows.Forms.Label();
            this.lbl_qrg = new System.Windows.Forms.Label();
            this.btn_qrcode = new Quras_gui.Controls.CustomizeControl.Buttons.GlowButton();
            this.txb_qrg_balance = new Quras_gui.Controls.CustomizeControl.TextBox.AlphaBlendTextBox();
            this.txb_qrs_balance = new Quras_gui.Controls.CustomizeControl.TextBox.AlphaBlendTextBox();
            this.SuspendLayout();
            // 
            // lbl_qrs
            // 
            this.lbl_qrs.AutoSize = true;
            this.lbl_qrs.BackColor = System.Drawing.Color.Transparent;
            this.lbl_qrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qrs.ForeColor = System.Drawing.Color.White;
            this.lbl_qrs.Location = new System.Drawing.Point(25, 42);
            this.lbl_qrs.Name = "lbl_qrs";
            this.lbl_qrs.Size = new System.Drawing.Size(55, 25);
            this.lbl_qrs.TabIndex = 0;
            this.lbl_qrs.Text = "QRS";
            // 
            // lbl_qrg
            // 
            this.lbl_qrg.AutoSize = true;
            this.lbl_qrg.BackColor = System.Drawing.Color.Transparent;
            this.lbl_qrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qrg.ForeColor = System.Drawing.Color.Silver;
            this.lbl_qrg.Location = new System.Drawing.Point(25, 76);
            this.lbl_qrg.Name = "lbl_qrg";
            this.lbl_qrg.Size = new System.Drawing.Size(56, 25);
            this.lbl_qrg.TabIndex = 1;
            this.lbl_qrg.Text = "QRG";
            // 
            // btn_qrcode
            // 
            this.btn_qrcode.BackgroundImage = global::Quras_gui.Properties.Resources.qr_code;
            this.btn_qrcode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_qrcode.FlatAppearance.BorderSize = 0;
            this.btn_qrcode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_qrcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_qrcode.GlowColor = System.Drawing.Color.Silver;
            this.btn_qrcode.Location = new System.Drawing.Point(337, 14);
            this.btn_qrcode.Name = "btn_qrcode";
            this.btn_qrcode.Size = new System.Drawing.Size(119, 119);
            this.btn_qrcode.TabIndex = 4;
            this.btn_qrcode.UseVisualStyleBackColor = true;
            this.btn_qrcode.Click += new System.EventHandler(this.btn_qrcode_Click);
            // 
            // txb_qrg_balance
            // 
            this.txb_qrg_balance.BackAlpha = 0;
            this.txb_qrg_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txb_qrg_balance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_qrg_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_qrg_balance.ForeColor = System.Drawing.Color.Silver;
            this.txb_qrg_balance.Location = new System.Drawing.Point(86, 76);
            this.txb_qrg_balance.Name = "txb_qrg_balance";
            this.txb_qrg_balance.Size = new System.Drawing.Size(229, 23);
            this.txb_qrg_balance.TabIndex = 3;
            this.txb_qrg_balance.Text = "0";
            // 
            // txb_qrs_balance
            // 
            this.txb_qrs_balance.BackAlpha = 0;
            this.txb_qrs_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txb_qrs_balance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_qrs_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_qrs_balance.ForeColor = System.Drawing.Color.Silver;
            this.txb_qrs_balance.Location = new System.Drawing.Point(86, 42);
            this.txb_qrs_balance.Name = "txb_qrs_balance";
            this.txb_qrs_balance.Size = new System.Drawing.Size(229, 23);
            this.txb_qrs_balance.TabIndex = 2;
            this.txb_qrs_balance.Text = "0";
            // 
            // DashboardPan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::Quras_gui.Properties.Resources.Dashboard_background;
            this.Controls.Add(this.btn_qrcode);
            this.Controls.Add(this.txb_qrg_balance);
            this.Controls.Add(this.txb_qrs_balance);
            this.Controls.Add(this.lbl_qrg);
            this.Controls.Add(this.lbl_qrs);
            this.Name = "DashboardPan";
            this.Size = new System.Drawing.Size(471, 627);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_qrs;
        private System.Windows.Forms.Label lbl_qrg;
        private CustomizeControl.TextBox.AlphaBlendTextBox txb_qrs_balance;
        private CustomizeControl.TextBox.AlphaBlendTextBox txb_qrg_balance;
        private CustomizeControl.Buttons.GlowButton btn_qrcode;
    }
}
