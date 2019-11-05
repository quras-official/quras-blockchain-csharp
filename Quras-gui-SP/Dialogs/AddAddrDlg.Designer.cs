namespace Quras_gui_SP.Dialogs
{
    partial class AddAddrDlg
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
            this.txb_contact_name = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.txb_address = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.lbl_warning = new System.Windows.Forms.Label();
            this.lbl_reciepent_address = new System.Windows.Forms.Label();
            this.btn_confirm = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.pan_bottom = new System.Windows.Forms.Panel();
            this.pan_right = new System.Windows.Forms.Panel();
            this.pan_top = new System.Windows.Forms.Panel();
            this.pan_password = new System.Windows.Forms.Panel();
            this.pan_wallet_path = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pan_left = new System.Windows.Forms.Panel();
            this.pan_password.SuspendLayout();
            this.pan_wallet_path.SuspendLayout();
            this.SuspendLayout();
            // 
            // txb_contact_name
            // 
            this.txb_contact_name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_contact_name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_contact_name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_contact_name.ForeColor = System.Drawing.Color.White;
            this.txb_contact_name.Location = new System.Drawing.Point(10, 9);
            this.txb_contact_name.Name = "txb_contact_name";
            this.txb_contact_name.Size = new System.Drawing.Size(496, 15);
            this.txb_contact_name.TabIndex = 2;
            this.txb_contact_name.WaterMark = "Input a name";
            this.txb_contact_name.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_contact_name.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_contact_name.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // txb_address
            // 
            this.txb_address.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_address.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_address.ForeColor = System.Drawing.Color.White;
            this.txb_address.Location = new System.Drawing.Point(10, 9);
            this.txb_address.Name = "txb_address";
            this.txb_address.Size = new System.Drawing.Size(496, 15);
            this.txb_address.TabIndex = 3;
            this.txb_address.WaterMark = "Input a address";
            this.txb_address.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_address.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_address.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // lbl_warning
            // 
            this.lbl_warning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lbl_warning.Location = new System.Drawing.Point(99, 182);
            this.lbl_warning.Name = "lbl_warning";
            this.lbl_warning.Size = new System.Drawing.Size(394, 44);
            this.lbl_warning.TabIndex = 54;
            this.lbl_warning.Text = "Input a name!";
            this.lbl_warning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_reciepent_address
            // 
            this.lbl_reciepent_address.AutoSize = true;
            this.lbl_reciepent_address.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_reciepent_address.ForeColor = System.Drawing.Color.White;
            this.lbl_reciepent_address.Location = new System.Drawing.Point(45, 48);
            this.lbl_reciepent_address.Name = "lbl_reciepent_address";
            this.lbl_reciepent_address.Size = new System.Drawing.Size(163, 17);
            this.lbl_reciepent_address.TabIndex = 52;
            this.lbl_reciepent_address.Text = "Please input a Name.";
            // 
            // btn_confirm
            // 
            this.btn_confirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_confirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_confirm.FlatAppearance.BorderSize = 0;
            this.btn_confirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_confirm.ForeColor = System.Drawing.Color.White;
            this.btn_confirm.Location = new System.Drawing.Point(499, 189);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(99, 31);
            this.btn_confirm.TabIndex = 42;
            this.btn_confirm.Text = "Confirm";
            this.btn_confirm.UseVisualStyleBackColor = false;
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.FlatAppearance.BorderSize = 0;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_close.ForeColor = System.Drawing.Color.Silver;
            this.btn_close.Location = new System.Drawing.Point(578, 1);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(30, 30);
            this.btn_close.TabIndex = 44;
            this.btn_close.Text = "×";
            this.btn_close.UseVisualStyleBackColor = false;
            // 
            // pan_bottom
            // 
            this.pan_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_bottom.Location = new System.Drawing.Point(1, 231);
            this.pan_bottom.Name = "pan_bottom";
            this.pan_bottom.Size = new System.Drawing.Size(607, 1);
            this.pan_bottom.TabIndex = 47;
            // 
            // pan_right
            // 
            this.pan_right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.pan_right.Location = new System.Drawing.Point(608, 1);
            this.pan_right.Name = "pan_right";
            this.pan_right.Size = new System.Drawing.Size(1, 231);
            this.pan_right.TabIndex = 46;
            // 
            // pan_top
            // 
            this.pan_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan_top.Location = new System.Drawing.Point(1, 0);
            this.pan_top.Name = "pan_top";
            this.pan_top.Size = new System.Drawing.Size(608, 1);
            this.pan_top.TabIndex = 45;
            // 
            // pan_password
            // 
            this.pan_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_password.Controls.Add(this.txb_address);
            this.pan_password.Location = new System.Drawing.Point(47, 142);
            this.pan_password.Name = "pan_password";
            this.pan_password.Size = new System.Drawing.Size(515, 34);
            this.pan_password.TabIndex = 50;
            // 
            // pan_wallet_path
            // 
            this.pan_wallet_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_wallet_path.Controls.Add(this.txb_contact_name);
            this.pan_wallet_path.Location = new System.Drawing.Point(47, 70);
            this.pan_wallet_path.Name = "pan_wallet_path";
            this.pan_wallet_path.Size = new System.Drawing.Size(515, 34);
            this.pan_wallet_path.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(45, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 17);
            this.label1.TabIndex = 53;
            this.label1.Text = "Please input the Address.";
            // 
            // pan_left
            // 
            this.pan_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_left.Location = new System.Drawing.Point(0, 0);
            this.pan_left.Name = "pan_left";
            this.pan_left.Size = new System.Drawing.Size(1, 232);
            this.pan_left.TabIndex = 48;
            // 
            // AddAddrDlg
            // 
            this.AcceptButton = this.btn_confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(609, 232);
            this.Controls.Add(this.lbl_warning);
            this.Controls.Add(this.lbl_reciepent_address);
            this.Controls.Add(this.btn_confirm);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.pan_bottom);
            this.Controls.Add(this.pan_right);
            this.Controls.Add(this.pan_top);
            this.Controls.Add(this.pan_password);
            this.Controls.Add(this.pan_wallet_path);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pan_left);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddAddrDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddAddrDlg";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.pan_password.ResumeLayout(false);
            this.pan_password.PerformLayout();
            this.pan_wallet_path.ResumeLayout(false);
            this.pan_wallet_path.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_contact_name;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_address;
        private System.Windows.Forms.Label lbl_warning;
        private System.Windows.Forms.Label lbl_reciepent_address;
        private System.Windows.Forms.Button btn_confirm;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Panel pan_bottom;
        private System.Windows.Forms.Panel pan_right;
        private System.Windows.Forms.Panel pan_top;
        private System.Windows.Forms.Panel pan_password;
        private System.Windows.Forms.Panel pan_wallet_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pan_left;
    }
}