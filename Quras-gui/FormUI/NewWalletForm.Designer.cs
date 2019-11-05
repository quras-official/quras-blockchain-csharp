using DT_GUI_Modules.Controls;

namespace Quras_gui.FormUI
{
    partial class NewWalletForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewWalletForm));
            this.pan_background = new System.Windows.Forms.Panel();
            this.chk_anonymous = new System.Windows.Forms.CheckBox();
            this.lbl_password_cmt = new System.Windows.Forms.Label();
            this.lbl_reciepent_address = new System.Windows.Forms.Label();
            this.pan_confirm_password = new System.Windows.Forms.Panel();
            this.txb_confirm_password = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.pan_password = new System.Windows.Forms.Panel();
            this.txb_password = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.btn_browser = new System.Windows.Forms.Button();
            this.pan_wallet_path = new System.Windows.Forms.Panel();
            this.txb_wallet_path = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.lbl_warning = new System.Windows.Forms.Label();
            this.btn_new_wallet_yes = new DT_GUI_Modules.Controls.GlowButton();
            this.lbl_comment = new System.Windows.Forms.Label();
            this.pic_mark = new System.Windows.Forms.PictureBox();
            this.pan_background.SuspendLayout();
            this.pan_confirm_password.SuspendLayout();
            this.pan_password.SuspendLayout();
            this.pan_wallet_path.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_mark)).BeginInit();
            this.SuspendLayout();
            // 
            // pan_background
            // 
            this.pan_background.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pan_background.BackgroundImage")));
            this.pan_background.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pan_background.Controls.Add(this.chk_anonymous);
            this.pan_background.Controls.Add(this.lbl_password_cmt);
            this.pan_background.Controls.Add(this.lbl_reciepent_address);
            this.pan_background.Controls.Add(this.pan_confirm_password);
            this.pan_background.Controls.Add(this.pan_password);
            this.pan_background.Controls.Add(this.btn_browser);
            this.pan_background.Controls.Add(this.pan_wallet_path);
            this.pan_background.Controls.Add(this.lbl_warning);
            this.pan_background.Controls.Add(this.btn_new_wallet_yes);
            this.pan_background.Controls.Add(this.lbl_comment);
            this.pan_background.Controls.Add(this.pic_mark);
            this.pan_background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_background.Location = new System.Drawing.Point(0, 0);
            this.pan_background.Name = "pan_background";
            this.pan_background.Size = new System.Drawing.Size(1192, 733);
            this.pan_background.TabIndex = 0;
            this.pan_background.Paint += new System.Windows.Forms.PaintEventHandler(this.pan_background_Paint);
            // 
            // chk_anonymous
            // 
            this.chk_anonymous.AutoSize = true;
            this.chk_anonymous.BackColor = System.Drawing.Color.Transparent;
            this.chk_anonymous.ForeColor = System.Drawing.Color.White;
            this.chk_anonymous.Location = new System.Drawing.Point(763, 508);
            this.chk_anonymous.Name = "chk_anonymous";
            this.chk_anonymous.Size = new System.Drawing.Size(104, 21);
            this.chk_anonymous.TabIndex = 44;
            this.chk_anonymous.Text = "Anonymous";
            this.chk_anonymous.UseVisualStyleBackColor = false;
            // 
            // lbl_password_cmt
            // 
            this.lbl_password_cmt.AutoSize = true;
            this.lbl_password_cmt.BackColor = System.Drawing.Color.Transparent;
            this.lbl_password_cmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_password_cmt.ForeColor = System.Drawing.Color.White;
            this.lbl_password_cmt.Location = new System.Drawing.Point(343, 435);
            this.lbl_password_cmt.Name = "lbl_password_cmt";
            this.lbl_password_cmt.Size = new System.Drawing.Size(204, 17);
            this.lbl_password_cmt.TabIndex = 42;
            this.lbl_password_cmt.Text = "Please Input the password.";
            // 
            // lbl_reciepent_address
            // 
            this.lbl_reciepent_address.AutoSize = true;
            this.lbl_reciepent_address.BackColor = System.Drawing.Color.Transparent;
            this.lbl_reciepent_address.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_reciepent_address.ForeColor = System.Drawing.Color.White;
            this.lbl_reciepent_address.Location = new System.Drawing.Point(343, 359);
            this.lbl_reciepent_address.Name = "lbl_reciepent_address";
            this.lbl_reciepent_address.Size = new System.Drawing.Size(214, 17);
            this.lbl_reciepent_address.TabIndex = 41;
            this.lbl_reciepent_address.Text = "Please Input the wallet path.";
            // 
            // pan_confirm_password
            // 
            this.pan_confirm_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_confirm_password.Controls.Add(this.txb_confirm_password);
            this.pan_confirm_password.Location = new System.Drawing.Point(344, 495);
            this.pan_confirm_password.Name = "pan_confirm_password";
            this.pan_confirm_password.Size = new System.Drawing.Size(370, 34);
            this.pan_confirm_password.TabIndex = 38;
            // 
            // txb_confirm_password
            // 
            this.txb_confirm_password.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_confirm_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_confirm_password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_confirm_password.ForeColor = System.Drawing.Color.White;
            this.txb_confirm_password.Location = new System.Drawing.Point(10, 9);
            this.txb_confirm_password.Name = "txb_confirm_password";
            this.txb_confirm_password.PasswordChar = '*';
            this.txb_confirm_password.Size = new System.Drawing.Size(351, 15);
            this.txb_confirm_password.TabIndex = 4;
            this.txb_confirm_password.WaterMark = "Confirm password";
            this.txb_confirm_password.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_confirm_password.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_confirm_password.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // pan_password
            // 
            this.pan_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_password.Controls.Add(this.txb_password);
            this.pan_password.Location = new System.Drawing.Point(344, 455);
            this.pan_password.Name = "pan_password";
            this.pan_password.Size = new System.Drawing.Size(370, 34);
            this.pan_password.TabIndex = 37;
            // 
            // txb_password
            // 
            this.txb_password.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_password.ForeColor = System.Drawing.Color.White;
            this.txb_password.Location = new System.Drawing.Point(10, 9);
            this.txb_password.Name = "txb_password";
            this.txb_password.PasswordChar = '*';
            this.txb_password.Size = new System.Drawing.Size(351, 15);
            this.txb_password.TabIndex = 3;
            this.txb_password.WaterMark = "Password";
            this.txb_password.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_password.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_password.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // btn_browser
            // 
            this.btn_browser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_browser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_browser.FlatAppearance.BorderSize = 0;
            this.btn_browser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_browser.ForeColor = System.Drawing.Color.White;
            this.btn_browser.Location = new System.Drawing.Point(833, 381);
            this.btn_browser.Name = "btn_browser";
            this.btn_browser.Size = new System.Drawing.Size(34, 34);
            this.btn_browser.TabIndex = 35;
            this.btn_browser.Text = "...";
            this.btn_browser.UseVisualStyleBackColor = false;
            this.btn_browser.Click += new System.EventHandler(this.btn_browser_Click);
            // 
            // pan_wallet_path
            // 
            this.pan_wallet_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_wallet_path.Controls.Add(this.txb_wallet_path);
            this.pan_wallet_path.Location = new System.Drawing.Point(344, 381);
            this.pan_wallet_path.Name = "pan_wallet_path";
            this.pan_wallet_path.Size = new System.Drawing.Size(483, 34);
            this.pan_wallet_path.TabIndex = 36;
            // 
            // txb_wallet_path
            // 
            this.txb_wallet_path.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_wallet_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_wallet_path.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_wallet_path.ForeColor = System.Drawing.Color.White;
            this.txb_wallet_path.Location = new System.Drawing.Point(10, 9);
            this.txb_wallet_path.Name = "txb_wallet_path";
            this.txb_wallet_path.ReadOnly = true;
            this.txb_wallet_path.Size = new System.Drawing.Size(464, 15);
            this.txb_wallet_path.TabIndex = 2;
            this.txb_wallet_path.WaterMark = "Quras wallet path";
            this.txb_wallet_path.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_wallet_path.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_wallet_path.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // lbl_warning
            // 
            this.lbl_warning.BackColor = System.Drawing.Color.Transparent;
            this.lbl_warning.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_warning.ForeColor = System.Drawing.Color.Red;
            this.lbl_warning.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lbl_warning.Location = new System.Drawing.Point(793, 660);
            this.lbl_warning.Name = "lbl_warning";
            this.lbl_warning.Size = new System.Drawing.Size(396, 64);
            this.lbl_warning.TabIndex = 12;
            this.lbl_warning.Text = "Your password is not strong\r\nPassword have to involve number, Uppercase character" +
    "";
            this.lbl_warning.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btn_new_wallet_yes
            // 
            this.btn_new_wallet_yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_new_wallet_yes.FlatAppearance.BorderSize = 0;
            this.btn_new_wallet_yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_new_wallet_yes.ForeColor = System.Drawing.Color.White;
            this.btn_new_wallet_yes.GlowColor = System.Drawing.Color.Silver;
            this.btn_new_wallet_yes.Location = new System.Drawing.Point(546, 594);
            this.btn_new_wallet_yes.Name = "btn_new_wallet_yes";
            this.btn_new_wallet_yes.Size = new System.Drawing.Size(142, 41);
            this.btn_new_wallet_yes.TabIndex = 2;
            this.btn_new_wallet_yes.Text = "Next";
            this.btn_new_wallet_yes.UseVisualStyleBackColor = false;
            this.btn_new_wallet_yes.Click += new System.EventHandler(this.btn_new_wallet_yes_Click);
            // 
            // lbl_comment
            // 
            this.lbl_comment.BackColor = System.Drawing.Color.Transparent;
            this.lbl_comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_comment.ForeColor = System.Drawing.Color.White;
            this.lbl_comment.Location = new System.Drawing.Point(3, 227);
            this.lbl_comment.Name = "lbl_comment";
            this.lbl_comment.Size = new System.Drawing.Size(1189, 83);
            this.lbl_comment.TabIndex = 1;
            this.lbl_comment.Text = "WOULD YOU LIKE TO MANUALY SET UP YOUR WALLET?";
            this.lbl_comment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pic_mark
            // 
            this.pic_mark.BackColor = System.Drawing.Color.Transparent;
            this.pic_mark.BackgroundImage = global::Quras_gui.Properties.Resources.new_wallet_mark;
            this.pic_mark.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_mark.Location = new System.Drawing.Point(528, 70);
            this.pic_mark.Name = "pic_mark";
            this.pic_mark.Size = new System.Drawing.Size(150, 150);
            this.pic_mark.TabIndex = 0;
            this.pic_mark.TabStop = false;
            // 
            // NewWalletForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1192, 733);
            this.Controls.Add(this.pan_background);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NewWalletForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Wallet";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewWalletForm_FormClosed);
            this.pan_background.ResumeLayout(false);
            this.pan_background.PerformLayout();
            this.pan_confirm_password.ResumeLayout(false);
            this.pan_confirm_password.PerformLayout();
            this.pan_password.ResumeLayout(false);
            this.pan_password.PerformLayout();
            this.pan_wallet_path.ResumeLayout(false);
            this.pan_wallet_path.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_mark)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_background;
        private System.Windows.Forms.PictureBox pic_mark;
        private System.Windows.Forms.Label lbl_comment;
        private GlowButton btn_new_wallet_yes;
        private System.Windows.Forms.Label lbl_warning;
        private System.Windows.Forms.Button btn_browser;
        private System.Windows.Forms.Panel pan_wallet_path;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_wallet_path;
        private System.Windows.Forms.Panel pan_confirm_password;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_confirm_password;
        private System.Windows.Forms.Panel pan_password;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_password;
        private System.Windows.Forms.Label lbl_password_cmt;
        private System.Windows.Forms.Label lbl_reciepent_address;
        private System.Windows.Forms.CheckBox chk_anonymous;
    }
}