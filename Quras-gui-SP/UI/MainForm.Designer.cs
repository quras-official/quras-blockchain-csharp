namespace Quras_gui_SP.UI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.pan_header = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_minimize = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.pan_left_side = new System.Windows.Forms.Panel();
            this.lbl_status = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_version = new System.Windows.Forms.Label();
            this.btn_tab_settings = new System.Windows.Forms.Button();
            this.btn_tab_addr_book = new System.Windows.Forms.Button();
            this.btn_tab_transactions = new System.Windows.Forms.Button();
            this.btn_tab_receive_coins = new System.Windows.Forms.Button();
            this.btn_tab_send_coins = new System.Windows.Forms.Button();
            this.btn_overview = new System.Windows.Forms.Button();
            this.pan_tab_select = new System.Windows.Forms.Panel();
            this.pan_right_side = new System.Windows.Forms.Panel();
            this.transactionRightSidePan1 = new Quras_gui_SP.RightSidePans.TransactionRightSidePan();
            this.pan_main = new System.Windows.Forms.Panel();
            this.settingsPan1 = new Quras_gui_SP.MainPans.SettingsPan();
            this.addrBookPan1 = new Quras_gui_SP.MainPans.AddrBookPan();
            this.overviewPan1 = new Quras_gui_SP.MainPans.OverviewPan();
            this.transactionsPan1 = new Quras_gui_SP.MainPans.TransactionsPan();
            this.receivecoinsPan1 = new Quras_gui_SP.MainPans.ReceivecoinsPan();
            this.sendcoinsPan1 = new Quras_gui_SP.MainPans.SendcoinsPan();
            this.timer_blockchain = new System.Windows.Forms.Timer(this.components);
            this.pan_header.SuspendLayout();
            this.pan_left_side.SuspendLayout();
            this.pan_right_side.SuspendLayout();
            this.pan_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan_header
            // 
            this.pan_header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(50)))));
            this.pan_header.Controls.Add(this.label1);
            this.pan_header.Controls.Add(this.btn_minimize);
            this.pan_header.Controls.Add(this.btn_close);
            this.pan_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan_header.Location = new System.Drawing.Point(0, 0);
            this.pan_header.Name = "pan_header";
            this.pan_header.Size = new System.Drawing.Size(1062, 39);
            this.pan_header.TabIndex = 0;
            this.pan_header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 35);
            this.label1.TabIndex = 2;
            this.label1.Text = "Quras Wallet";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            // 
            // btn_minimize
            // 
            this.btn_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_minimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(50)))));
            this.btn_minimize.FlatAppearance.BorderSize = 0;
            this.btn_minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_minimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_minimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_minimize.Location = new System.Drawing.Point(982, 0);
            this.btn_minimize.Name = "btn_minimize";
            this.btn_minimize.Size = new System.Drawing.Size(39, 39);
            this.btn_minimize.TabIndex = 1;
            this.btn_minimize.Text = "−";
            this.btn_minimize.UseVisualStyleBackColor = false;
            this.btn_minimize.Click += new System.EventHandler(this.btn_minimize_Click);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(50)))));
            this.btn_close.FlatAppearance.BorderSize = 0;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_close.ForeColor = System.Drawing.Color.Red;
            this.btn_close.Location = new System.Drawing.Point(1023, 0);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(39, 39);
            this.btn_close.TabIndex = 0;
            this.btn_close.Text = "×";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // pan_left_side
            // 
            this.pan_left_side.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(164)))), ((int)(((byte)(202)))));
            this.pan_left_side.Controls.Add(this.lbl_status);
            this.pan_left_side.Controls.Add(this.label2);
            this.pan_left_side.Controls.Add(this.lbl_version);
            this.pan_left_side.Controls.Add(this.btn_tab_settings);
            this.pan_left_side.Controls.Add(this.btn_tab_addr_book);
            this.pan_left_side.Controls.Add(this.btn_tab_transactions);
            this.pan_left_side.Controls.Add(this.btn_tab_receive_coins);
            this.pan_left_side.Controls.Add(this.btn_tab_send_coins);
            this.pan_left_side.Controls.Add(this.btn_overview);
            this.pan_left_side.Controls.Add(this.pan_tab_select);
            this.pan_left_side.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_left_side.Location = new System.Drawing.Point(0, 39);
            this.pan_left_side.Name = "pan_left_side";
            this.pan_left_side.Size = new System.Drawing.Size(204, 631);
            this.pan_left_side.TabIndex = 1;
            // 
            // lbl_status
            // 
            this.lbl_status.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbl_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_status.ForeColor = System.Drawing.Color.White;
            this.lbl_status.Location = new System.Drawing.Point(0, 600);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(204, 23);
            this.lbl_status.TabIndex = 10;
            this.lbl_status.Text = "ZkSnarks Module Loading...";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.Font = new System.Drawing.Font("Segoe Print", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 556);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Quras";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_version
            // 
            this.lbl_version.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbl_version.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_version.ForeColor = System.Drawing.Color.White;
            this.lbl_version.Location = new System.Drawing.Point(0, 581);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(204, 23);
            this.lbl_version.TabIndex = 7;
            this.lbl_version.Text = "Version 1.0.0.1, IT Company";
            this.lbl_version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_tab_settings
            // 
            this.btn_tab_settings.FlatAppearance.BorderSize = 0;
            this.btn_tab_settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tab_settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tab_settings.ForeColor = System.Drawing.Color.White;
            this.btn_tab_settings.Location = new System.Drawing.Point(9, 250);
            this.btn_tab_settings.Name = "btn_tab_settings";
            this.btn_tab_settings.Size = new System.Drawing.Size(195, 50);
            this.btn_tab_settings.TabIndex = 6;
            this.btn_tab_settings.Text = "Settings";
            this.btn_tab_settings.UseVisualStyleBackColor = true;
            this.btn_tab_settings.Click += new System.EventHandler(this.btn_tab_settings_Click);
            // 
            // btn_tab_addr_book
            // 
            this.btn_tab_addr_book.FlatAppearance.BorderSize = 0;
            this.btn_tab_addr_book.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tab_addr_book.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tab_addr_book.ForeColor = System.Drawing.Color.White;
            this.btn_tab_addr_book.Location = new System.Drawing.Point(9, 200);
            this.btn_tab_addr_book.Name = "btn_tab_addr_book";
            this.btn_tab_addr_book.Size = new System.Drawing.Size(195, 50);
            this.btn_tab_addr_book.TabIndex = 5;
            this.btn_tab_addr_book.Text = "Address book";
            this.btn_tab_addr_book.UseVisualStyleBackColor = true;
            this.btn_tab_addr_book.Click += new System.EventHandler(this.btn_tab_addr_book_Click);
            // 
            // btn_tab_transactions
            // 
            this.btn_tab_transactions.FlatAppearance.BorderSize = 0;
            this.btn_tab_transactions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tab_transactions.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tab_transactions.ForeColor = System.Drawing.Color.White;
            this.btn_tab_transactions.Location = new System.Drawing.Point(9, 150);
            this.btn_tab_transactions.Name = "btn_tab_transactions";
            this.btn_tab_transactions.Size = new System.Drawing.Size(195, 50);
            this.btn_tab_transactions.TabIndex = 4;
            this.btn_tab_transactions.Text = "Transactions";
            this.btn_tab_transactions.UseVisualStyleBackColor = true;
            this.btn_tab_transactions.Click += new System.EventHandler(this.btn_tab_transactions_Click);
            // 
            // btn_tab_receive_coins
            // 
            this.btn_tab_receive_coins.FlatAppearance.BorderSize = 0;
            this.btn_tab_receive_coins.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tab_receive_coins.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tab_receive_coins.ForeColor = System.Drawing.Color.White;
            this.btn_tab_receive_coins.Location = new System.Drawing.Point(9, 100);
            this.btn_tab_receive_coins.Name = "btn_tab_receive_coins";
            this.btn_tab_receive_coins.Size = new System.Drawing.Size(195, 50);
            this.btn_tab_receive_coins.TabIndex = 3;
            this.btn_tab_receive_coins.Text = "Receive coins";
            this.btn_tab_receive_coins.UseVisualStyleBackColor = true;
            this.btn_tab_receive_coins.Click += new System.EventHandler(this.btn_tab_receive_coins_Click);
            // 
            // btn_tab_send_coins
            // 
            this.btn_tab_send_coins.FlatAppearance.BorderSize = 0;
            this.btn_tab_send_coins.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_tab_send_coins.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tab_send_coins.ForeColor = System.Drawing.Color.White;
            this.btn_tab_send_coins.Location = new System.Drawing.Point(9, 50);
            this.btn_tab_send_coins.Name = "btn_tab_send_coins";
            this.btn_tab_send_coins.Size = new System.Drawing.Size(195, 50);
            this.btn_tab_send_coins.TabIndex = 2;
            this.btn_tab_send_coins.Text = "Send coins";
            this.btn_tab_send_coins.UseVisualStyleBackColor = true;
            this.btn_tab_send_coins.Click += new System.EventHandler(this.btn_tab_send_coins_Click);
            // 
            // btn_overview
            // 
            this.btn_overview.FlatAppearance.BorderSize = 0;
            this.btn_overview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_overview.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_overview.ForeColor = System.Drawing.Color.White;
            this.btn_overview.Location = new System.Drawing.Point(9, 0);
            this.btn_overview.Name = "btn_overview";
            this.btn_overview.Size = new System.Drawing.Size(195, 50);
            this.btn_overview.TabIndex = 1;
            this.btn_overview.Text = "Overview";
            this.btn_overview.UseVisualStyleBackColor = true;
            this.btn_overview.Click += new System.EventHandler(this.btn_overview_Click);
            // 
            // pan_tab_select
            // 
            this.pan_tab_select.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(163)))));
            this.pan_tab_select.Location = new System.Drawing.Point(0, 0);
            this.pan_tab_select.Name = "pan_tab_select";
            this.pan_tab_select.Size = new System.Drawing.Size(10, 50);
            this.pan_tab_select.TabIndex = 0;
            // 
            // pan_right_side
            // 
            this.pan_right_side.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(31)))), ((int)(((byte)(37)))));
            this.pan_right_side.Controls.Add(this.transactionRightSidePan1);
            this.pan_right_side.Dock = System.Windows.Forms.DockStyle.Right;
            this.pan_right_side.Location = new System.Drawing.Point(869, 39);
            this.pan_right_side.Name = "pan_right_side";
            this.pan_right_side.Size = new System.Drawing.Size(193, 631);
            this.pan_right_side.TabIndex = 2;
            // 
            // transactionRightSidePan1
            // 
            this.transactionRightSidePan1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(31)))), ((int)(((byte)(37)))));
            this.transactionRightSidePan1.Location = new System.Drawing.Point(0, 0);
            this.transactionRightSidePan1.Name = "transactionRightSidePan1";
            this.transactionRightSidePan1.Size = new System.Drawing.Size(193, 631);
            this.transactionRightSidePan1.TabIndex = 0;
            // 
            // pan_main
            // 
            this.pan_main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.pan_main.Controls.Add(this.settingsPan1);
            this.pan_main.Controls.Add(this.addrBookPan1);
            this.pan_main.Controls.Add(this.overviewPan1);
            this.pan_main.Controls.Add(this.transactionsPan1);
            this.pan_main.Controls.Add(this.receivecoinsPan1);
            this.pan_main.Controls.Add(this.sendcoinsPan1);
            this.pan_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_main.Location = new System.Drawing.Point(204, 39);
            this.pan_main.Name = "pan_main";
            this.pan_main.Size = new System.Drawing.Size(665, 631);
            this.pan_main.TabIndex = 3;
            // 
            // settingsPan1
            // 
            this.settingsPan1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.settingsPan1.Location = new System.Drawing.Point(6, 5);
            this.settingsPan1.Name = "settingsPan1";
            this.settingsPan1.Size = new System.Drawing.Size(665, 631);
            this.settingsPan1.TabIndex = 6;
            // 
            // addrBookPan1
            // 
            this.addrBookPan1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.addrBookPan1.Location = new System.Drawing.Point(27, 19);
            this.addrBookPan1.Name = "addrBookPan1";
            this.addrBookPan1.Size = new System.Drawing.Size(665, 631);
            this.addrBookPan1.TabIndex = 5;
            // 
            // overviewPan1
            // 
            this.overviewPan1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.overviewPan1.Location = new System.Drawing.Point(27, 91);
            this.overviewPan1.Name = "overviewPan1";
            this.overviewPan1.Size = new System.Drawing.Size(665, 631);
            this.overviewPan1.TabIndex = 4;
            // 
            // transactionsPan1
            // 
            this.transactionsPan1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.transactionsPan1.Location = new System.Drawing.Point(27, 125);
            this.transactionsPan1.Name = "transactionsPan1";
            this.transactionsPan1.Size = new System.Drawing.Size(665, 631);
            this.transactionsPan1.TabIndex = 3;
            // 
            // receivecoinsPan1
            // 
            this.receivecoinsPan1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.receivecoinsPan1.Location = new System.Drawing.Point(79, 64);
            this.receivecoinsPan1.Name = "receivecoinsPan1";
            this.receivecoinsPan1.Size = new System.Drawing.Size(665, 631);
            this.receivecoinsPan1.TabIndex = 2;
            // 
            // sendcoinsPan1
            // 
            this.sendcoinsPan1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.sendcoinsPan1.Location = new System.Drawing.Point(190, 40);
            this.sendcoinsPan1.Name = "sendcoinsPan1";
            this.sendcoinsPan1.Size = new System.Drawing.Size(665, 631);
            this.sendcoinsPan1.TabIndex = 1;
            // 
            // timer_blockchain
            // 
            this.timer_blockchain.Enabled = true;
            this.timer_blockchain.Interval = 500;
            this.timer_blockchain.Tick += new System.EventHandler(this.timer_blockchain_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1062, 670);
            this.Controls.Add(this.pan_main);
            this.Controls.Add(this.pan_right_side);
            this.Controls.Add(this.pan_left_side);
            this.Controls.Add(this.pan_header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pan_header.ResumeLayout(false);
            this.pan_header.PerformLayout();
            this.pan_left_side.ResumeLayout(false);
            this.pan_right_side.ResumeLayout(false);
            this.pan_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_header;
        private System.Windows.Forms.Panel pan_left_side;
        private System.Windows.Forms.Panel pan_right_side;
        private System.Windows.Forms.Panel pan_main;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_minimize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_tab_send_coins;
        private System.Windows.Forms.Button btn_overview;
        private System.Windows.Forms.Panel pan_tab_select;
        private System.Windows.Forms.Button btn_tab_transactions;
        private System.Windows.Forms.Button btn_tab_receive_coins;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_version;
        private System.Windows.Forms.Button btn_tab_settings;
        private System.Windows.Forms.Button btn_tab_addr_book;
        private MainPans.OverviewPan overviewPan1;
        private RightSidePans.TransactionRightSidePan transactionRightSidePan1;
        private MainPans.SendcoinsPan sendcoinsPan1;
        private MainPans.ReceivecoinsPan receivecoinsPan1;
        private MainPans.TransactionsPan transactionsPan1;
        private System.Windows.Forms.Timer timer_blockchain;
        private MainPans.AddrBookPan addrBookPan1;
        private MainPans.SettingsPan settingsPan1;
        private System.Windows.Forms.Label lbl_status;
    }
}