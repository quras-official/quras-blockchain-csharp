namespace Quras_gui_SP.MainPans
{
    partial class ReceivecoinsPan
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_overview_title = new System.Windows.Forms.Label();
            this.btn_make_qr_code = new System.Windows.Forms.Button();
            this.pan_send = new System.Windows.Forms.Panel();
            this.lbl_cmt_asset = new System.Windows.Forms.Label();
            this.cmb_assets = new System.Windows.Forms.ComboBox();
            this.lbl_cmt_amount = new System.Windows.Forms.Label();
            this.lbl_balance = new System.Windows.Forms.Label();
            this.lbl_cmt_balance = new System.Windows.Forms.Label();
            this.pan_amount = new System.Windows.Forms.Panel();
            this.txb_amount = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_addr_comment = new System.Windows.Forms.Label();
            this.btn_add_addr = new System.Windows.Forms.Button();
            this.btn_addr_book = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txb_recv_addr = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.lbl_reciepent_address = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pan_send.SuspendLayout();
            this.pan_amount.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(47, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 28);
            this.label1.TabIndex = 5;
            this.label1.Text = "You can make a QR code for receiving coins.";
            // 
            // lbl_overview_title
            // 
            this.lbl_overview_title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_overview_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_overview_title.ForeColor = System.Drawing.Color.White;
            this.lbl_overview_title.Location = new System.Drawing.Point(47, 40);
            this.lbl_overview_title.Name = "lbl_overview_title";
            this.lbl_overview_title.Size = new System.Drawing.Size(349, 31);
            this.lbl_overview_title.TabIndex = 4;
            this.lbl_overview_title.Text = "Hello, receive coins here.";
            // 
            // btn_make_qr_code
            // 
            this.btn_make_qr_code.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_make_qr_code.FlatAppearance.BorderSize = 0;
            this.btn_make_qr_code.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_make_qr_code.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_make_qr_code.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btn_make_qr_code.Location = new System.Drawing.Point(458, 53);
            this.btn_make_qr_code.Name = "btn_make_qr_code";
            this.btn_make_qr_code.Size = new System.Drawing.Size(151, 40);
            this.btn_make_qr_code.TabIndex = 6;
            this.btn_make_qr_code.Text = "Make QR Code";
            this.btn_make_qr_code.UseVisualStyleBackColor = false;
            // 
            // pan_send
            // 
            this.pan_send.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.pan_send.Controls.Add(this.lbl_cmt_asset);
            this.pan_send.Controls.Add(this.cmb_assets);
            this.pan_send.Controls.Add(this.lbl_cmt_amount);
            this.pan_send.Controls.Add(this.lbl_balance);
            this.pan_send.Controls.Add(this.lbl_cmt_balance);
            this.pan_send.Controls.Add(this.pan_amount);
            this.pan_send.Controls.Add(this.label2);
            this.pan_send.Controls.Add(this.lbl_addr_comment);
            this.pan_send.Controls.Add(this.btn_add_addr);
            this.pan_send.Controls.Add(this.btn_addr_book);
            this.pan_send.Controls.Add(this.panel1);
            this.pan_send.Controls.Add(this.lbl_reciepent_address);
            this.pan_send.Location = new System.Drawing.Point(52, 313);
            this.pan_send.Name = "pan_send";
            this.pan_send.Size = new System.Drawing.Size(558, 291);
            this.pan_send.TabIndex = 7;
            // 
            // lbl_cmt_asset
            // 
            this.lbl_cmt_asset.AutoSize = true;
            this.lbl_cmt_asset.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_asset.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_asset.Location = new System.Drawing.Point(22, 189);
            this.lbl_cmt_asset.Name = "lbl_cmt_asset";
            this.lbl_cmt_asset.Size = new System.Drawing.Size(146, 13);
            this.lbl_cmt_asset.TabIndex = 11;
            this.lbl_cmt_asset.Text = "Please check the assets type.";
            // 
            // cmb_assets
            // 
            this.cmb_assets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.cmb_assets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_assets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_assets.ForeColor = System.Drawing.Color.White;
            this.cmb_assets.FormattingEnabled = true;
            this.cmb_assets.Location = new System.Drawing.Point(22, 156);
            this.cmb_assets.Name = "cmb_assets";
            this.cmb_assets.Size = new System.Drawing.Size(128, 24);
            this.cmb_assets.TabIndex = 10;
            // 
            // lbl_cmt_amount
            // 
            this.lbl_cmt_amount.AutoSize = true;
            this.lbl_cmt_amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_amount.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_amount.Location = new System.Drawing.Point(22, 262);
            this.lbl_cmt_amount.Name = "lbl_cmt_amount";
            this.lbl_cmt_amount.Size = new System.Drawing.Size(128, 13);
            this.lbl_cmt_amount.TabIndex = 9;
            this.lbl_cmt_amount.Text = "Please check the amount.";
            // 
            // lbl_balance
            // 
            this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.lbl_balance.Location = new System.Drawing.Point(320, 237);
            this.lbl_balance.Name = "lbl_balance";
            this.lbl_balance.Size = new System.Drawing.Size(226, 17);
            this.lbl_balance.TabIndex = 8;
            // 
            // lbl_cmt_balance
            // 
            this.lbl_cmt_balance.AutoSize = true;
            this.lbl_cmt_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_balance.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_balance.Location = new System.Drawing.Point(320, 220);
            this.lbl_cmt_balance.Name = "lbl_cmt_balance";
            this.lbl_cmt_balance.Size = new System.Drawing.Size(0, 13);
            this.lbl_cmt_balance.TabIndex = 7;
            // 
            // pan_amount
            // 
            this.pan_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_amount.Controls.Add(this.txb_amount);
            this.pan_amount.Location = new System.Drawing.Point(22, 220);
            this.pan_amount.Name = "pan_amount";
            this.pan_amount.Size = new System.Drawing.Size(292, 34);
            this.pan_amount.TabIndex = 3;
            // 
            // txb_amount
            // 
            this.txb_amount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_amount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_amount.ForeColor = System.Drawing.Color.White;
            this.txb_amount.Location = new System.Drawing.Point(10, 9);
            this.txb_amount.Name = "txb_amount";
            this.txb_amount.Size = new System.Drawing.Size(266, 15);
            this.txb_amount.TabIndex = 1;
            this.txb_amount.WaterMark = "Enter quras amount";
            this.txb_amount.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_amount.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_amount.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(19, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Amount && Assets";
            // 
            // lbl_addr_comment
            // 
            this.lbl_addr_comment.AutoSize = true;
            this.lbl_addr_comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_addr_comment.ForeColor = System.Drawing.Color.Gray;
            this.lbl_addr_comment.Location = new System.Drawing.Point(22, 87);
            this.lbl_addr_comment.Name = "lbl_addr_comment";
            this.lbl_addr_comment.Size = new System.Drawing.Size(305, 13);
            this.lbl_addr_comment.TabIndex = 5;
            this.lbl_addr_comment.Text = "Please only enter an QRS address. Funds will be lost otherwise.";
            // 
            // btn_add_addr
            // 
            this.btn_add_addr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_add_addr.BackgroundImage = global::Quras_gui_SP.Properties.Resources.add;
            this.btn_add_addr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_add_addr.FlatAppearance.BorderSize = 0;
            this.btn_add_addr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add_addr.Location = new System.Drawing.Point(512, 46);
            this.btn_add_addr.Name = "btn_add_addr";
            this.btn_add_addr.Size = new System.Drawing.Size(34, 34);
            this.btn_add_addr.TabIndex = 4;
            this.btn_add_addr.UseVisualStyleBackColor = false;
            // 
            // btn_addr_book
            // 
            this.btn_addr_book.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_addr_book.BackgroundImage = global::Quras_gui_SP.Properties.Resources.addrbook;
            this.btn_addr_book.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_addr_book.FlatAppearance.BorderSize = 0;
            this.btn_addr_book.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_addr_book.Location = new System.Drawing.Point(472, 46);
            this.btn_addr_book.Name = "btn_addr_book";
            this.btn_addr_book.Size = new System.Drawing.Size(34, 34);
            this.btn_addr_book.TabIndex = 3;
            this.btn_addr_book.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.panel1.Controls.Add(this.txb_recv_addr);
            this.panel1.Location = new System.Drawing.Point(22, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(443, 34);
            this.panel1.TabIndex = 2;
            // 
            // txb_recv_addr
            // 
            this.txb_recv_addr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_recv_addr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_recv_addr.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_recv_addr.ForeColor = System.Drawing.Color.White;
            this.txb_recv_addr.Location = new System.Drawing.Point(10, 9);
            this.txb_recv_addr.Name = "txb_recv_addr";
            this.txb_recv_addr.Size = new System.Drawing.Size(424, 15);
            this.txb_recv_addr.TabIndex = 1;
            this.txb_recv_addr.WaterMark = "Enter quras wallet address";
            this.txb_recv_addr.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_recv_addr.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_recv_addr.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // lbl_reciepent_address
            // 
            this.lbl_reciepent_address.AutoSize = true;
            this.lbl_reciepent_address.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_reciepent_address.ForeColor = System.Drawing.Color.White;
            this.lbl_reciepent_address.Location = new System.Drawing.Point(19, 18);
            this.lbl_reciepent_address.Name = "lbl_reciepent_address";
            this.lbl_reciepent_address.Size = new System.Drawing.Size(145, 17);
            this.lbl_reciepent_address.TabIndex = 0;
            this.lbl_reciepent_address.Text = "Reciepent Address";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Quras_gui_SP.Properties.Resources.qr_code;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(236, 116);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 180);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // ReceivecoinsPan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pan_send);
            this.Controls.Add(this.btn_make_qr_code);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_overview_title);
            this.Name = "ReceivecoinsPan";
            this.Size = new System.Drawing.Size(665, 631);
            this.pan_send.ResumeLayout(false);
            this.pan_send.PerformLayout();
            this.pan_amount.ResumeLayout(false);
            this.pan_amount.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_overview_title;
        private System.Windows.Forms.Button btn_make_qr_code;
        private System.Windows.Forms.Panel pan_send;
        private System.Windows.Forms.Label lbl_cmt_asset;
        private System.Windows.Forms.ComboBox cmb_assets;
        private System.Windows.Forms.Label lbl_cmt_amount;
        private System.Windows.Forms.Label lbl_balance;
        private System.Windows.Forms.Label lbl_cmt_balance;
        private System.Windows.Forms.Panel pan_amount;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_amount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_addr_comment;
        private System.Windows.Forms.Button btn_add_addr;
        private System.Windows.Forms.Button btn_addr_book;
        private System.Windows.Forms.Panel panel1;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_recv_addr;
        private System.Windows.Forms.Label lbl_reciepent_address;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
