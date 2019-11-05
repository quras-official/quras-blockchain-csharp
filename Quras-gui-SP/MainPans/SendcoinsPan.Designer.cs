namespace Quras_gui_SP.MainPans
{
    partial class SendcoinsPan
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
            this.btn_send_coins = new System.Windows.Forms.Button();
            this.pan_send = new System.Windows.Forms.Panel();
            this.btn_my_address = new System.Windows.Forms.Button();
            this.pan_from_address = new System.Windows.Forms.Panel();
            this.txb_from_address = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.lbl_from_address = new System.Windows.Forms.Label();
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_qrs_btc = new System.Windows.Forms.Label();
            this.lbl_qrs_usd = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbl_cmt_live_data = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_cmt_price = new System.Windows.Forms.Label();
            this.pan_send.SuspendLayout();
            this.pan_from_address.SuspendLayout();
            this.pan_amount.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.label1.TabIndex = 3;
            this.label1.Text = "Send coins to another quras wallet.";
            // 
            // lbl_overview_title
            // 
            this.lbl_overview_title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_overview_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_overview_title.ForeColor = System.Drawing.Color.White;
            this.lbl_overview_title.Location = new System.Drawing.Point(47, 40);
            this.lbl_overview_title.Name = "lbl_overview_title";
            this.lbl_overview_title.Size = new System.Drawing.Size(349, 31);
            this.lbl_overview_title.TabIndex = 2;
            this.lbl_overview_title.Text = "Hello, send coins here.";
            // 
            // btn_send_coins
            // 
            this.btn_send_coins.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_send_coins.FlatAppearance.BorderSize = 0;
            this.btn_send_coins.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_send_coins.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_send_coins.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btn_send_coins.Location = new System.Drawing.Point(458, 53);
            this.btn_send_coins.Name = "btn_send_coins";
            this.btn_send_coins.Size = new System.Drawing.Size(151, 40);
            this.btn_send_coins.TabIndex = 4;
            this.btn_send_coins.Text = "Send coins";
            this.btn_send_coins.UseVisualStyleBackColor = false;
            this.btn_send_coins.Click += new System.EventHandler(this.btn_send_coins_Click);
            // 
            // pan_send
            // 
            this.pan_send.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.pan_send.Controls.Add(this.btn_my_address);
            this.pan_send.Controls.Add(this.pan_from_address);
            this.pan_send.Controls.Add(this.lbl_from_address);
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
            this.pan_send.Location = new System.Drawing.Point(51, 123);
            this.pan_send.Name = "pan_send";
            this.pan_send.Size = new System.Drawing.Size(558, 339);
            this.pan_send.TabIndex = 5;
            // 
            // btn_my_address
            // 
            this.btn_my_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_my_address.BackgroundImage = global::Quras_gui_SP.Properties.Resources.add;
            this.btn_my_address.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_my_address.FlatAppearance.BorderSize = 0;
            this.btn_my_address.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_my_address.Location = new System.Drawing.Point(472, 36);
            this.btn_my_address.Name = "btn_my_address";
            this.btn_my_address.Size = new System.Drawing.Size(74, 34);
            this.btn_my_address.TabIndex = 14;
            this.btn_my_address.UseVisualStyleBackColor = false;
            // 
            // pan_from_address
            // 
            this.pan_from_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_from_address.Controls.Add(this.txb_from_address);
            this.pan_from_address.Location = new System.Drawing.Point(22, 36);
            this.pan_from_address.Name = "pan_from_address";
            this.pan_from_address.Size = new System.Drawing.Size(443, 34);
            this.pan_from_address.TabIndex = 13;
            // 
            // txb_from_address
            // 
            this.txb_from_address.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_from_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_from_address.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_from_address.ForeColor = System.Drawing.Color.White;
            this.txb_from_address.Location = new System.Drawing.Point(10, 9);
            this.txb_from_address.Name = "txb_from_address";
            this.txb_from_address.Size = new System.Drawing.Size(424, 15);
            this.txb_from_address.TabIndex = 1;
            this.txb_from_address.WaterMark = "Enter quras wallet address";
            this.txb_from_address.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_from_address.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_from_address.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // lbl_from_address
            // 
            this.lbl_from_address.AutoSize = true;
            this.lbl_from_address.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_from_address.ForeColor = System.Drawing.Color.White;
            this.lbl_from_address.Location = new System.Drawing.Point(19, 11);
            this.lbl_from_address.Name = "lbl_from_address";
            this.lbl_from_address.Size = new System.Drawing.Size(108, 17);
            this.lbl_from_address.TabIndex = 12;
            this.lbl_from_address.Text = "From Address";
            // 
            // lbl_cmt_asset
            // 
            this.lbl_cmt_asset.AutoSize = true;
            this.lbl_cmt_asset.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_asset.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_asset.Location = new System.Drawing.Point(22, 241);
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
            this.cmb_assets.Location = new System.Drawing.Point(22, 208);
            this.cmb_assets.Name = "cmb_assets";
            this.cmb_assets.Size = new System.Drawing.Size(128, 24);
            this.cmb_assets.TabIndex = 10;
            this.cmb_assets.SelectedIndexChanged += new System.EventHandler(this.cmb_assets_SelectedIndexChanged);
            // 
            // lbl_cmt_amount
            // 
            this.lbl_cmt_amount.AutoSize = true;
            this.lbl_cmt_amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_amount.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_amount.Location = new System.Drawing.Point(22, 314);
            this.lbl_cmt_amount.Name = "lbl_cmt_amount";
            this.lbl_cmt_amount.Size = new System.Drawing.Size(128, 13);
            this.lbl_cmt_amount.TabIndex = 9;
            this.lbl_cmt_amount.Text = "Please check the amount.";
            // 
            // lbl_balance
            // 
            this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.lbl_balance.Location = new System.Drawing.Point(320, 289);
            this.lbl_balance.Name = "lbl_balance";
            this.lbl_balance.Size = new System.Drawing.Size(226, 17);
            this.lbl_balance.TabIndex = 8;
            this.lbl_balance.Text = "0 QRS";
            // 
            // lbl_cmt_balance
            // 
            this.lbl_cmt_balance.AutoSize = true;
            this.lbl_cmt_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_balance.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_balance.Location = new System.Drawing.Point(320, 272);
            this.lbl_cmt_balance.Name = "lbl_cmt_balance";
            this.lbl_cmt_balance.Size = new System.Drawing.Size(45, 13);
            this.lbl_cmt_balance.TabIndex = 7;
            this.lbl_cmt_balance.Text = "Balance";
            // 
            // pan_amount
            // 
            this.pan_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_amount.Controls.Add(this.txb_amount);
            this.pan_amount.Location = new System.Drawing.Point(22, 272);
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
            this.label2.Location = new System.Drawing.Point(19, 177);
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
            this.lbl_addr_comment.Location = new System.Drawing.Point(22, 154);
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
            this.btn_add_addr.Location = new System.Drawing.Point(512, 113);
            this.btn_add_addr.Name = "btn_add_addr";
            this.btn_add_addr.Size = new System.Drawing.Size(34, 34);
            this.btn_add_addr.TabIndex = 4;
            this.btn_add_addr.UseVisualStyleBackColor = false;
            this.btn_add_addr.Click += new System.EventHandler(this.btn_add_addr_Click);
            // 
            // btn_addr_book
            // 
            this.btn_addr_book.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_addr_book.BackgroundImage = global::Quras_gui_SP.Properties.Resources.addrbook;
            this.btn_addr_book.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_addr_book.FlatAppearance.BorderSize = 0;
            this.btn_addr_book.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_addr_book.Location = new System.Drawing.Point(472, 113);
            this.btn_addr_book.Name = "btn_addr_book";
            this.btn_addr_book.Size = new System.Drawing.Size(34, 34);
            this.btn_addr_book.TabIndex = 3;
            this.btn_addr_book.UseVisualStyleBackColor = false;
            this.btn_addr_book.Click += new System.EventHandler(this.btn_addr_book_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.panel1.Controls.Add(this.txb_recv_addr);
            this.panel1.Location = new System.Drawing.Point(22, 113);
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
            this.lbl_reciepent_address.Location = new System.Drawing.Point(19, 85);
            this.lbl_reciepent_address.Name = "lbl_reciepent_address";
            this.lbl_reciepent_address.Size = new System.Drawing.Size(145, 17);
            this.lbl_reciepent_address.TabIndex = 0;
            this.lbl_reciepent_address.Text = "Reciepent Address";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lbl_qrs_btc);
            this.panel2.Controls.Add(this.lbl_qrs_usd);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.lbl_cmt_live_data);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lbl_cmt_price);
            this.panel2.Location = new System.Drawing.Point(52, 479);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(557, 123);
            this.panel2.TabIndex = 6;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(408, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 18);
            this.label7.TabIndex = 16;
            this.label7.Text = "Market Cap";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(166, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(222, 18);
            this.label8.TabIndex = 15;
            this.label8.Text = "QRG BTC";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(11, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 18);
            this.label9.TabIndex = 14;
            this.label9.Text = "QRG USD";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(412, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 23);
            this.label10.TabIndex = 13;
            this.label10.Text = "$ 240 M";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(165, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(222, 23);
            this.label11.TabIndex = 12;
            this.label11.Text = "0.0001343 BTC";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(18, 76);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(124, 23);
            this.label12.TabIndex = 11;
            this.label12.Text = "233 $";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(408, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "Market Cap";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(166, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(222, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "QRS BTC";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(11, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "QRS USD";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(412, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "$ 1,240 M";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_qrs_btc
            // 
            this.lbl_qrs_btc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_qrs_btc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qrs_btc.ForeColor = System.Drawing.Color.White;
            this.lbl_qrs_btc.Location = new System.Drawing.Point(165, 30);
            this.lbl_qrs_btc.Name = "lbl_qrs_btc";
            this.lbl_qrs_btc.Size = new System.Drawing.Size(222, 23);
            this.lbl_qrs_btc.TabIndex = 6;
            this.lbl_qrs_btc.Text = "0.0002343 BTC";
            this.lbl_qrs_btc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_qrs_usd
            // 
            this.lbl_qrs_usd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qrs_usd.ForeColor = System.Drawing.Color.White;
            this.lbl_qrs_usd.Location = new System.Drawing.Point(18, 30);
            this.lbl_qrs_usd.Name = "lbl_qrs_usd";
            this.lbl_qrs_usd.Size = new System.Drawing.Size(124, 23);
            this.lbl_qrs_usd.TabIndex = 5;
            this.lbl_qrs_usd.Text = "1,233 $";
            this.lbl_qrs_usd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.panel5.Location = new System.Drawing.Point(393, 42);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(2, 61);
            this.panel5.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.panel4.Location = new System.Drawing.Point(157, 42);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 61);
            this.panel4.TabIndex = 3;
            // 
            // lbl_cmt_live_data
            // 
            this.lbl_cmt_live_data.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_live_data.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_live_data.Location = new System.Drawing.Point(418, 5);
            this.lbl_cmt_live_data.Name = "lbl_cmt_live_data";
            this.lbl_cmt_live_data.Size = new System.Drawing.Size(108, 13);
            this.lbl_cmt_live_data.TabIndex = 2;
            this.lbl_cmt_live_data.Text = "Live Data";
            this.lbl_cmt_live_data.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gray;
            this.panel3.Location = new System.Drawing.Point(11, 22);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(537, 1);
            this.panel3.TabIndex = 1;
            // 
            // lbl_cmt_price
            // 
            this.lbl_cmt_price.AutoSize = true;
            this.lbl_cmt_price.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_price.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_price.Location = new System.Drawing.Point(23, 5);
            this.lbl_cmt_price.Name = "lbl_cmt_price";
            this.lbl_cmt_price.Size = new System.Drawing.Size(108, 13);
            this.lbl_cmt_price.TabIndex = 0;
            this.lbl_cmt_price.Text = "Quras Verge price";
            // 
            // SendcoinsPan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pan_send);
            this.Controls.Add(this.btn_send_coins);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_overview_title);
            this.Name = "SendcoinsPan";
            this.Size = new System.Drawing.Size(665, 631);
            this.pan_send.ResumeLayout(false);
            this.pan_send.PerformLayout();
            this.pan_from_address.ResumeLayout(false);
            this.pan_from_address.PerformLayout();
            this.pan_amount.ResumeLayout(false);
            this.pan_amount.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_overview_title;
        private System.Windows.Forms.Button btn_send_coins;
        private System.Windows.Forms.Panel pan_send;
        private System.Windows.Forms.Label lbl_reciepent_address;
        private System.Windows.Forms.Panel panel1;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_recv_addr;
        private System.Windows.Forms.Label lbl_addr_comment;
        private System.Windows.Forms.Button btn_add_addr;
        private System.Windows.Forms.Button btn_addr_book;
        private System.Windows.Forms.Panel pan_amount;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_amount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_cmt_amount;
        private System.Windows.Forms.Label lbl_balance;
        private System.Windows.Forms.Label lbl_cmt_balance;
        private System.Windows.Forms.Label lbl_cmt_asset;
        private System.Windows.Forms.ComboBox cmb_assets;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbl_cmt_price;
        private System.Windows.Forms.Label lbl_cmt_live_data;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_qrs_btc;
        private System.Windows.Forms.Label lbl_qrs_usd;
        private System.Windows.Forms.Label lbl_from_address;
        private System.Windows.Forms.Panel pan_from_address;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_from_address;
        private System.Windows.Forms.Button btn_my_address;
    }
}
