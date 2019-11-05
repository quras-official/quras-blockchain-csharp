namespace Quras_gui.Controls.Items
{
    partial class PasswordItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordItem));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btn_show_password = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txb_password = new Quras_gui.Controls.CustomizeControl.TextBox.AlphaBlendTextBox();
            this.txb_wallet_path = new Quras_gui.Controls.CustomizeControl.TextBox.AlphaBlendTextBox();
            this.btn_browser = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Location = new System.Drawing.Point(67, 142);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 1);
            this.panel1.TabIndex = 47;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.Location = new System.Drawing.Point(67, 79);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(461, 1);
            this.panel2.TabIndex = 48;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::Quras_gui.Properties.Resources.wallet;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(67, 43);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 30);
            this.pictureBox2.TabIndex = 49;
            this.pictureBox2.TabStop = false;
            // 
            // btn_show_password
            // 
            this.btn_show_password.BackColor = System.Drawing.Color.Transparent;
            this.btn_show_password.BackgroundImage = global::Quras_gui.Properties.Resources.show_password;
            this.btn_show_password.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_show_password.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_show_password.FlatAppearance.BorderSize = 0;
            this.btn_show_password.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_show_password.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_show_password.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_show_password.Location = new System.Drawing.Point(495, 105);
            this.btn_show_password.Name = "btn_show_password";
            this.btn_show_password.Size = new System.Drawing.Size(30, 30);
            this.btn_show_password.TabIndex = 46;
            this.btn_show_password.UseVisualStyleBackColor = false;
            this.btn_show_password.Click += new System.EventHandler(this.btn_show_password_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::Quras_gui.Properties.Resources.password_b;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(67, 105);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.TabIndex = 45;
            this.pictureBox1.TabStop = false;
            // 
            // txb_password
            // 
            this.txb_password.BackAlpha = 0;
            this.txb_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.txb_password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_password.ForeColor = System.Drawing.Color.White;
            this.txb_password.Location = new System.Drawing.Point(111, 109);
            this.txb_password.Name = "txb_password";
            this.txb_password.PasswordChar = '*';
            this.txb_password.Size = new System.Drawing.Size(370, 23);
            this.txb_password.TabIndex = 44;
            // 
            // txb_wallet_path
            // 
            this.txb_wallet_path.BackAlpha = 0;
            this.txb_wallet_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.txb_wallet_path.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_wallet_path.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_wallet_path.ForeColor = System.Drawing.Color.White;
            this.txb_wallet_path.Location = new System.Drawing.Point(111, 50);
            this.txb_wallet_path.Name = "txb_wallet_path";
            this.txb_wallet_path.Size = new System.Drawing.Size(370, 17);
            this.txb_wallet_path.TabIndex = 43;
            // 
            // btn_browser
            // 
            this.btn_browser.BackColor = System.Drawing.Color.Transparent;
            this.btn_browser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_browser.BackgroundImage")));
            this.btn_browser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_browser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_browser.FlatAppearance.BorderSize = 0;
            this.btn_browser.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_browser.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_browser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_browser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_browser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_browser.Location = new System.Drawing.Point(493, 43);
            this.btn_browser.Margin = new System.Windows.Forms.Padding(4);
            this.btn_browser.Name = "btn_browser";
            this.btn_browser.Size = new System.Drawing.Size(33, 30);
            this.btn_browser.TabIndex = 50;
            this.btn_browser.Text = "...";
            this.btn_browser.UseVisualStyleBackColor = false;
            this.btn_browser.Click += new System.EventHandler(this.bunifuThinButton21_Click);
            // 
            // PasswordItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btn_browser);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_show_password);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txb_password);
            this.Controls.Add(this.txb_wallet_path);
            this.Name = "PasswordItem";
            this.Size = new System.Drawing.Size(578, 197);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private CustomizeControl.TextBox.AlphaBlendTextBox txb_password;
        private CustomizeControl.TextBox.AlphaBlendTextBox txb_wallet_path;
        private System.Windows.Forms.Button btn_show_password;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btn_browser;
    }
}
