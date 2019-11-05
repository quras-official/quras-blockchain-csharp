namespace Quras_gui.FormUI
{
    partial class RestoreWalletForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreWalletForm));
            this.lbl_comment = new System.Windows.Forms.Label();
            this.lbl_warning = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_continue = new MaterialSkin.Controls.MaterialRaisedButton();
            this.passwordItem1 = new Quras_gui.Controls.Items.PasswordItem();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_comment
            // 
            this.lbl_comment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_comment.BackColor = System.Drawing.Color.Transparent;
            this.lbl_comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_comment.ForeColor = System.Drawing.Color.White;
            this.lbl_comment.Location = new System.Drawing.Point(12, 159);
            this.lbl_comment.Name = "lbl_comment";
            this.lbl_comment.Size = new System.Drawing.Size(1168, 57);
            this.lbl_comment.TabIndex = 0;
            this.lbl_comment.Text = "ENTER YOUR BACKUP PHRASE";
            this.lbl_comment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_warning
            // 
            this.lbl_warning.BackColor = System.Drawing.Color.Transparent;
            this.lbl_warning.ForeColor = System.Drawing.Color.Red;
            this.lbl_warning.Location = new System.Drawing.Point(17, 11);
            this.lbl_warning.Name = "lbl_warning";
            this.lbl_warning.Size = new System.Drawing.Size(1160, 23);
            this.lbl_warning.TabIndex = 10;
            this.lbl_warning.Text = "Password is not correct!";
            this.lbl_warning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btn_continue);
            this.panel1.Controls.Add(this.lbl_warning);
            this.panel1.Location = new System.Drawing.Point(-1, 449);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1195, 157);
            this.panel1.TabIndex = 12;
            // 
            // btn_continue
            // 
            this.btn_continue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_continue.Depth = 0;
            this.btn_continue.Location = new System.Drawing.Point(520, 72);
            this.btn_continue.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_continue.Name = "btn_continue";
            this.btn_continue.Primary = true;
            this.btn_continue.Size = new System.Drawing.Size(151, 33);
            this.btn_continue.TabIndex = 12;
            this.btn_continue.Text = "Continue";
            this.btn_continue.UseVisualStyleBackColor = true;
            this.btn_continue.Click += new System.EventHandler(this.btn_continue_Click);
            // 
            // passwordItem1
            // 
            this.passwordItem1.BackColor = System.Drawing.Color.Transparent;
            this.passwordItem1.Location = new System.Drawing.Point(296, 248);
            this.passwordItem1.Name = "passwordItem1";
            this.passwordItem1.Size = new System.Drawing.Size(578, 197);
            this.passwordItem1.TabIndex = 13;
            // 
            // RestoreWalletForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1192, 733);
            this.Controls.Add(this.passwordItem1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbl_comment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RestoreWalletForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open Wallet";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RestoreWalletForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_comment;
        private System.Windows.Forms.Label lbl_warning;
        private System.Windows.Forms.Panel panel1;
        private Controls.Items.PasswordItem passwordItem1;
        private MaterialSkin.Controls.MaterialRaisedButton btn_continue;
    }
}