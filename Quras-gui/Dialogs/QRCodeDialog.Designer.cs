namespace Quras_gui.Dialogs
{
    partial class QRCodeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QRCodeDialog));
            this.pic_qr_code = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbl_comment = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_qr_code)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_qr_code
            // 
            this.pic_qr_code.Location = new System.Drawing.Point(2, 2);
            this.pic_qr_code.Name = "pic_qr_code";
            this.pic_qr_code.Size = new System.Drawing.Size(246, 246);
            this.pic_qr_code.TabIndex = 0;
            this.pic_qr_code.TabStop = false;
            this.pic_qr_code.Click += new System.EventHandler(this.pic_qr_code_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 1);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(249, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 273);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1, 273);
            this.panel3.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(1, 273);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(248, 1);
            this.panel4.TabIndex = 4;
            // 
            // lbl_comment
            // 
            this.lbl_comment.ForeColor = System.Drawing.Color.Gray;
            this.lbl_comment.Location = new System.Drawing.Point(2, 251);
            this.lbl_comment.Name = "lbl_comment";
            this.lbl_comment.Size = new System.Drawing.Size(246, 17);
            this.lbl_comment.TabIndex = 5;
            this.lbl_comment.Text = "This is your QR Code of address.";
            this.lbl_comment.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbl_comment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // QRCodeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(250, 274);
            this.Controls.Add(this.lbl_comment);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pic_qr_code);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QRCodeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QRCodeDialog";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pic_qr_code)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_qr_code;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbl_comment;
    }
}