namespace DT_GUI_Modules.Modules
{
    partial class SendControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_send = new DT_GUI_Modules.Controls.GlowButton();
            this.txb_amount = new System.Windows.Forms.TextBox();
            this.lbl_amount = new System.Windows.Forms.Label();
            this.cmb_type = new System.Windows.Forms.ComboBox();
            this.lbl_currency_type = new System.Windows.Forms.Label();
            this.cmb_from_address = new System.Windows.Forms.ComboBox();
            this.lbl_to = new System.Windows.Forms.Label();
            this.lbl_from = new System.Windows.Forms.Label();
            this.txb_to_address = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::DT_GUI_Modules.Properties.Resources.background_1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.txb_to_address);
            this.panel1.Controls.Add(this.btn_send);
            this.panel1.Controls.Add(this.txb_amount);
            this.panel1.Controls.Add(this.lbl_amount);
            this.panel1.Controls.Add(this.cmb_type);
            this.panel1.Controls.Add(this.lbl_currency_type);
            this.panel1.Controls.Add(this.cmb_from_address);
            this.panel1.Controls.Add(this.lbl_to);
            this.panel1.Controls.Add(this.lbl_from);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 240);
            this.panel1.TabIndex = 0;
            // 
            // btn_send
            // 
            this.btn_send.BackColor = System.Drawing.Color.White;
            this.btn_send.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_send.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_send.GlowColor = System.Drawing.Color.Silver;
            this.btn_send.Location = new System.Drawing.Point(387, 192);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(99, 28);
            this.btn_send.TabIndex = 8;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = false;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // txb_amount
            // 
            this.txb_amount.Location = new System.Drawing.Point(108, 137);
            this.txb_amount.Name = "txb_amount";
            this.txb_amount.Size = new System.Drawing.Size(181, 22);
            this.txb_amount.TabIndex = 7;
            // 
            // lbl_amount
            // 
            this.lbl_amount.AutoSize = true;
            this.lbl_amount.BackColor = System.Drawing.Color.Transparent;
            this.lbl_amount.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lbl_amount.Location = new System.Drawing.Point(33, 139);
            this.lbl_amount.Name = "lbl_amount";
            this.lbl_amount.Size = new System.Drawing.Size(64, 17);
            this.lbl_amount.TabIndex = 6;
            this.lbl_amount.Text = "Amount :";
            // 
            // cmb_type
            // 
            this.cmb_type.BackColor = System.Drawing.Color.White;
            this.cmb_type.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_type.FormattingEnabled = true;
            this.cmb_type.Location = new System.Drawing.Point(108, 104);
            this.cmb_type.Name = "cmb_type";
            this.cmb_type.Size = new System.Drawing.Size(181, 24);
            this.cmb_type.TabIndex = 5;
            // 
            // lbl_currency_type
            // 
            this.lbl_currency_type.AutoSize = true;
            this.lbl_currency_type.BackColor = System.Drawing.Color.Transparent;
            this.lbl_currency_type.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lbl_currency_type.Location = new System.Drawing.Point(49, 106);
            this.lbl_currency_type.Name = "lbl_currency_type";
            this.lbl_currency_type.Size = new System.Drawing.Size(48, 17);
            this.lbl_currency_type.TabIndex = 4;
            this.lbl_currency_type.Text = "Type :";
            // 
            // cmb_from_address
            // 
            this.cmb_from_address.BackColor = System.Drawing.Color.White;
            this.cmb_from_address.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_from_address.FormattingEnabled = true;
            this.cmb_from_address.Location = new System.Drawing.Point(108, 38);
            this.cmb_from_address.Name = "cmb_from_address";
            this.cmb_from_address.Size = new System.Drawing.Size(378, 24);
            this.cmb_from_address.TabIndex = 2;
            // 
            // lbl_to
            // 
            this.lbl_to.AutoSize = true;
            this.lbl_to.BackColor = System.Drawing.Color.Transparent;
            this.lbl_to.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lbl_to.Location = new System.Drawing.Point(64, 73);
            this.lbl_to.Name = "lbl_to";
            this.lbl_to.Size = new System.Drawing.Size(33, 17);
            this.lbl_to.TabIndex = 1;
            this.lbl_to.Text = "To :";
            // 
            // lbl_from
            // 
            this.lbl_from.AutoSize = true;
            this.lbl_from.BackColor = System.Drawing.Color.Transparent;
            this.lbl_from.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lbl_from.Location = new System.Drawing.Point(49, 41);
            this.lbl_from.Name = "lbl_from";
            this.lbl_from.Size = new System.Drawing.Size(48, 17);
            this.lbl_from.TabIndex = 0;
            this.lbl_from.Text = "From :";
            // 
            // txb_to_address
            // 
            this.txb_to_address.Location = new System.Drawing.Point(107, 72);
            this.txb_to_address.Name = "txb_to_address";
            this.txb_to_address.Size = new System.Drawing.Size(379, 22);
            this.txb_to_address.TabIndex = 9;
            // 
            // SendControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "SendControl";
            this.Size = new System.Drawing.Size(542, 240);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_from;
        private System.Windows.Forms.ComboBox cmb_from_address;
        private System.Windows.Forms.Label lbl_to;
        private Controls.GlowButton btn_send;
        private System.Windows.Forms.TextBox txb_amount;
        private System.Windows.Forms.Label lbl_amount;
        private System.Windows.Forms.ComboBox cmb_type;
        private System.Windows.Forms.Label lbl_currency_type;
        private System.Windows.Forms.TextBox txb_to_address;
    }
}
