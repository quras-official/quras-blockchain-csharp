namespace Quras_gui_SP.Dialogs
{
    partial class FromAddrDlg
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
            this.btn_confirm = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.pan_bottom = new System.Windows.Forms.Panel();
            this.pan_right = new System.Windows.Forms.Panel();
            this.pan_top = new System.Windows.Forms.Panel();
            this.pan_left = new System.Windows.Forms.Panel();
            this.cmb_address = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pan_password = new System.Windows.Forms.Panel();
            this.txb_password = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.pan_password.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_confirm
            // 
            this.btn_confirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_confirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_confirm.FlatAppearance.BorderSize = 0;
            this.btn_confirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_confirm.ForeColor = System.Drawing.Color.White;
            this.btn_confirm.Location = new System.Drawing.Point(495, 165);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(99, 31);
            this.btn_confirm.TabIndex = 55;
            this.btn_confirm.Text = "Confirm";
            this.btn_confirm.UseVisualStyleBackColor = false;
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
            this.btn_close.Location = new System.Drawing.Point(575, 1);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(30, 30);
            this.btn_close.TabIndex = 56;
            this.btn_close.Text = "×";
            this.btn_close.UseVisualStyleBackColor = false;
            // 
            // pan_bottom
            // 
            this.pan_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_bottom.Location = new System.Drawing.Point(1, 207);
            this.pan_bottom.Name = "pan_bottom";
            this.pan_bottom.Size = new System.Drawing.Size(604, 1);
            this.pan_bottom.TabIndex = 59;
            // 
            // pan_right
            // 
            this.pan_right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.pan_right.Location = new System.Drawing.Point(605, 1);
            this.pan_right.Name = "pan_right";
            this.pan_right.Size = new System.Drawing.Size(1, 207);
            this.pan_right.TabIndex = 58;
            // 
            // pan_top
            // 
            this.pan_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan_top.Location = new System.Drawing.Point(1, 0);
            this.pan_top.Name = "pan_top";
            this.pan_top.Size = new System.Drawing.Size(605, 1);
            this.pan_top.TabIndex = 57;
            // 
            // pan_left
            // 
            this.pan_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_left.Location = new System.Drawing.Point(0, 0);
            this.pan_left.Name = "pan_left";
            this.pan_left.Size = new System.Drawing.Size(1, 208);
            this.pan_left.TabIndex = 60;
            // 
            // cmb_address
            // 
            this.cmb_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.cmb_address.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_address.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_address.ForeColor = System.Drawing.Color.White;
            this.cmb_address.FormattingEnabled = true;
            this.cmb_address.Location = new System.Drawing.Point(43, 70);
            this.cmb_address.Name = "cmb_address";
            this.cmb_address.Size = new System.Drawing.Size(523, 24);
            this.cmb_address.TabIndex = 64;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(40, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 17);
            this.label2.TabIndex = 63;
            this.label2.Text = "Select the address to export.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 17);
            this.label1.TabIndex = 62;
            this.label1.Text = "Please Input the password.";
            // 
            // pan_password
            // 
            this.pan_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_password.Controls.Add(this.txb_password);
            this.pan_password.Location = new System.Drawing.Point(42, 134);
            this.pan_password.Name = "pan_password";
            this.pan_password.Size = new System.Drawing.Size(229, 34);
            this.pan_password.TabIndex = 61;
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
            this.txb_password.ReadOnly = true;
            this.txb_password.Size = new System.Drawing.Size(210, 15);
            this.txb_password.TabIndex = 3;
            this.txb_password.WaterMark = "Password";
            this.txb_password.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_password.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_password.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // FromAddrDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(606, 208);
            this.Controls.Add(this.cmb_address);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pan_password);
            this.Controls.Add(this.btn_confirm);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.pan_bottom);
            this.Controls.Add(this.pan_right);
            this.Controls.Add(this.pan_top);
            this.Controls.Add(this.pan_left);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FromAddrDlg";
            this.Text = "FromAddrDlg";
            this.pan_password.ResumeLayout(false);
            this.pan_password.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_confirm;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Panel pan_bottom;
        private System.Windows.Forms.Panel pan_right;
        private System.Windows.Forms.Panel pan_top;
        private System.Windows.Forms.Panel pan_left;
        private System.Windows.Forms.ComboBox cmb_address;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pan_password;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_password;
    }
}