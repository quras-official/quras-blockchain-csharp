namespace Quras_gui_SP.MainPans
{
    partial class OverviewPan
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
            this.lbl_overview_title = new System.Windows.Forms.Label();
            this.lbl_wallet_status = new System.Windows.Forms.Label();
            this.lbl_cmt_qrs_bal = new System.Windows.Forms.Label();
            this.lbl_cmt_qrg_bal = new System.Windows.Forms.Label();
            this.lbl_qrs_balance = new System.Windows.Forms.Label();
            this.lbl_qrg_balance = new System.Windows.Forms.Label();
            this.lbl_cmt_inf = new System.Windows.Forms.Label();
            this.btn_open_wallet = new System.Windows.Forms.Button();
            this.btn_add_addr = new System.Windows.Forms.Button();
            this.btn_export_wallet = new System.Windows.Forms.Button();
            this.pan_addr = new System.Windows.Forms.Panel();
            this.pan_addr_main = new System.Windows.Forms.Panel();
            this.pan_addr_header = new System.Windows.Forms.Panel();
            this.lbl_cmt_balance = new System.Windows.Forms.Label();
            this.lbl_cmt_assets = new System.Windows.Forms.Label();
            this.pan_split = new System.Windows.Forms.Panel();
            this.lbl_cmt_addr = new System.Windows.Forms.Label();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.lbl_block_height = new System.Windows.Forms.Label();
            this.lbl_connect = new System.Windows.Forms.Label();
            this.btn_new_wallet = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.pan_addr.SuspendLayout();
            this.pan_addr_header.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_overview_title
            // 
            this.lbl_overview_title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_overview_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_overview_title.ForeColor = System.Drawing.Color.White;
            this.lbl_overview_title.Location = new System.Drawing.Point(48, 40);
            this.lbl_overview_title.Name = "lbl_overview_title";
            this.lbl_overview_title.Size = new System.Drawing.Size(573, 31);
            this.lbl_overview_title.TabIndex = 0;
            this.lbl_overview_title.Text = "Hello, this is your wallet.";
            // 
            // lbl_wallet_status
            // 
            this.lbl_wallet_status.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_wallet_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_wallet_status.ForeColor = System.Drawing.Color.White;
            this.lbl_wallet_status.Location = new System.Drawing.Point(48, 80);
            this.lbl_wallet_status.Name = "lbl_wallet_status";
            this.lbl_wallet_status.Size = new System.Drawing.Size(573, 28);
            this.lbl_wallet_status.TabIndex = 1;
            this.lbl_wallet_status.Text = "Here is a summary of your wallet.";
            // 
            // lbl_cmt_qrs_bal
            // 
            this.lbl_cmt_qrs_bal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_cmt_qrs_bal.AutoSize = true;
            this.lbl_cmt_qrs_bal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(152)))), ((int)(((byte)(154)))));
            this.lbl_cmt_qrs_bal.Location = new System.Drawing.Point(53, 146);
            this.lbl_cmt_qrs_bal.Name = "lbl_cmt_qrs_bal";
            this.lbl_cmt_qrs_bal.Size = new System.Drawing.Size(93, 17);
            this.lbl_cmt_qrs_bal.TabIndex = 2;
            this.lbl_cmt_qrs_bal.Text = "QRS Balance";
            // 
            // lbl_cmt_qrg_bal
            // 
            this.lbl_cmt_qrg_bal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_cmt_qrg_bal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(152)))), ((int)(((byte)(154)))));
            this.lbl_cmt_qrg_bal.Location = new System.Drawing.Point(494, 146);
            this.lbl_cmt_qrg_bal.Name = "lbl_cmt_qrg_bal";
            this.lbl_cmt_qrg_bal.Size = new System.Drawing.Size(127, 17);
            this.lbl_cmt_qrg_bal.TabIndex = 3;
            this.lbl_cmt_qrg_bal.Text = "QRG Balance";
            this.lbl_cmt_qrg_bal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_qrs_balance
            // 
            this.lbl_qrs_balance.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_qrs_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qrs_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.lbl_qrs_balance.Location = new System.Drawing.Point(51, 178);
            this.lbl_qrs_balance.Name = "lbl_qrs_balance";
            this.lbl_qrs_balance.Size = new System.Drawing.Size(256, 29);
            this.lbl_qrs_balance.TabIndex = 4;
            this.lbl_qrs_balance.Text = "0 QRS";
            // 
            // lbl_qrg_balance
            // 
            this.lbl_qrg_balance.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_qrg_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qrg_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.lbl_qrg_balance.Location = new System.Drawing.Point(359, 178);
            this.lbl_qrg_balance.Name = "lbl_qrg_balance";
            this.lbl_qrg_balance.Size = new System.Drawing.Size(264, 29);
            this.lbl_qrg_balance.TabIndex = 5;
            this.lbl_qrg_balance.Text = "0 QRG";
            this.lbl_qrg_balance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_cmt_inf
            // 
            this.lbl_cmt_inf.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_cmt_inf.AutoSize = true;
            this.lbl_cmt_inf.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_inf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.lbl_cmt_inf.Location = new System.Drawing.Point(331, 161);
            this.lbl_cmt_inf.Name = "lbl_cmt_inf";
            this.lbl_cmt_inf.Size = new System.Drawing.Size(19, 29);
            this.lbl_cmt_inf.TabIndex = 6;
            this.lbl_cmt_inf.Text = "|";
            // 
            // btn_open_wallet
            // 
            this.btn_open_wallet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_open_wallet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_open_wallet.FlatAppearance.BorderSize = 0;
            this.btn_open_wallet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_open_wallet.ForeColor = System.Drawing.Color.White;
            this.btn_open_wallet.Location = new System.Drawing.Point(225, 578);
            this.btn_open_wallet.Name = "btn_open_wallet";
            this.btn_open_wallet.Size = new System.Drawing.Size(128, 27);
            this.btn_open_wallet.TabIndex = 7;
            this.btn_open_wallet.Text = "Open Wallet";
            this.btn_open_wallet.UseVisualStyleBackColor = false;
            this.btn_open_wallet.Click += new System.EventHandler(this.btn_open_wallet_Click);
            // 
            // btn_add_addr
            // 
            this.btn_add_addr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_add_addr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_add_addr.FlatAppearance.BorderSize = 0;
            this.btn_add_addr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add_addr.ForeColor = System.Drawing.Color.White;
            this.btn_add_addr.Location = new System.Drawing.Point(359, 578);
            this.btn_add_addr.Name = "btn_add_addr";
            this.btn_add_addr.Size = new System.Drawing.Size(128, 27);
            this.btn_add_addr.TabIndex = 8;
            this.btn_add_addr.Text = "Add Address";
            this.btn_add_addr.UseVisualStyleBackColor = false;
            // 
            // btn_export_wallet
            // 
            this.btn_export_wallet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_export_wallet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_export_wallet.FlatAppearance.BorderSize = 0;
            this.btn_export_wallet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_export_wallet.ForeColor = System.Drawing.Color.White;
            this.btn_export_wallet.Location = new System.Drawing.Point(493, 578);
            this.btn_export_wallet.Name = "btn_export_wallet";
            this.btn_export_wallet.Size = new System.Drawing.Size(128, 27);
            this.btn_export_wallet.TabIndex = 9;
            this.btn_export_wallet.Text = "Export Wallet";
            this.btn_export_wallet.UseVisualStyleBackColor = false;
            this.btn_export_wallet.Click += new System.EventHandler(this.btn_export_wallet_Click);
            // 
            // pan_addr
            // 
            this.pan_addr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pan_addr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            this.pan_addr.Controls.Add(this.pan_addr_main);
            this.pan_addr.Controls.Add(this.pan_addr_header);
            this.pan_addr.Controls.Add(this.vScrollBar1);
            this.pan_addr.Location = new System.Drawing.Point(53, 232);
            this.pan_addr.Name = "pan_addr";
            this.pan_addr.Size = new System.Drawing.Size(568, 278);
            this.pan_addr.TabIndex = 10;
            // 
            // pan_addr_main
            // 
            this.pan_addr_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_addr_main.Location = new System.Drawing.Point(0, 35);
            this.pan_addr_main.Name = "pan_addr_main";
            this.pan_addr_main.Size = new System.Drawing.Size(555, 243);
            this.pan_addr_main.TabIndex = 2;
            // 
            // pan_addr_header
            // 
            this.pan_addr_header.Controls.Add(this.lbl_cmt_balance);
            this.pan_addr_header.Controls.Add(this.lbl_cmt_assets);
            this.pan_addr_header.Controls.Add(this.pan_split);
            this.pan_addr_header.Controls.Add(this.lbl_cmt_addr);
            this.pan_addr_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan_addr_header.Location = new System.Drawing.Point(0, 0);
            this.pan_addr_header.Name = "pan_addr_header";
            this.pan_addr_header.Size = new System.Drawing.Size(555, 35);
            this.pan_addr_header.TabIndex = 1;
            // 
            // lbl_cmt_balance
            // 
            this.lbl_cmt_balance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_cmt_balance.AutoSize = true;
            this.lbl_cmt_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(152)))), ((int)(((byte)(154)))));
            this.lbl_cmt_balance.Location = new System.Drawing.Point(458, 9);
            this.lbl_cmt_balance.Name = "lbl_cmt_balance";
            this.lbl_cmt_balance.Size = new System.Drawing.Size(66, 17);
            this.lbl_cmt_balance.TabIndex = 3;
            this.lbl_cmt_balance.Text = "Balance";
            // 
            // lbl_cmt_assets
            // 
            this.lbl_cmt_assets.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_cmt_assets.AutoSize = true;
            this.lbl_cmt_assets.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_assets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(152)))), ((int)(((byte)(154)))));
            this.lbl_cmt_assets.Location = new System.Drawing.Point(333, 9);
            this.lbl_cmt_assets.Name = "lbl_cmt_assets";
            this.lbl_cmt_assets.Size = new System.Drawing.Size(48, 17);
            this.lbl_cmt_assets.TabIndex = 2;
            this.lbl_cmt_assets.Text = "Asset";
            // 
            // pan_split
            // 
            this.pan_split.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.pan_split.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_split.Location = new System.Drawing.Point(0, 34);
            this.pan_split.Name = "pan_split";
            this.pan_split.Size = new System.Drawing.Size(555, 1);
            this.pan_split.TabIndex = 1;
            // 
            // lbl_cmt_addr
            // 
            this.lbl_cmt_addr.AutoSize = true;
            this.lbl_cmt_addr.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_addr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(152)))), ((int)(((byte)(154)))));
            this.lbl_cmt_addr.Location = new System.Drawing.Point(22, 9);
            this.lbl_cmt_addr.Name = "lbl_cmt_addr";
            this.lbl_cmt_addr.Size = new System.Drawing.Size(67, 17);
            this.lbl_cmt_addr.TabIndex = 0;
            this.lbl_cmt_addr.Text = "Address";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(555, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(13, 278);
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // lbl_block_height
            // 
            this.lbl_block_height.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_block_height.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(152)))), ((int)(((byte)(154)))));
            this.lbl_block_height.Location = new System.Drawing.Point(53, 522);
            this.lbl_block_height.Name = "lbl_block_height";
            this.lbl_block_height.Size = new System.Drawing.Size(570, 23);
            this.lbl_block_height.TabIndex = 11;
            this.lbl_block_height.Text = "Height : 0/0";
            this.lbl_block_height.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_connect
            // 
            this.lbl_connect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_connect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(152)))), ((int)(((byte)(154)))));
            this.lbl_connect.Location = new System.Drawing.Point(53, 545);
            this.lbl_connect.Name = "lbl_connect";
            this.lbl_connect.Size = new System.Drawing.Size(570, 23);
            this.lbl_connect.TabIndex = 12;
            this.lbl_connect.Text = "Connect : 0";
            this.lbl_connect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_new_wallet
            // 
            this.btn_new_wallet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_new_wallet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_new_wallet.FlatAppearance.BorderSize = 0;
            this.btn_new_wallet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_new_wallet.ForeColor = System.Drawing.Color.White;
            this.btn_new_wallet.Location = new System.Drawing.Point(91, 578);
            this.btn_new_wallet.Name = "btn_new_wallet";
            this.btn_new_wallet.Size = new System.Drawing.Size(128, 27);
            this.btn_new_wallet.TabIndex = 13;
            this.btn_new_wallet.Text = "New Wallet";
            this.btn_new_wallet.UseVisualStyleBackColor = false;
            this.btn_new_wallet.Click += new System.EventHandler(this.btn_new_wallet_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_refresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(166)))), ((int)(((byte)(204)))));
            this.btn_refresh.FlatAppearance.BorderSize = 0;
            this.btn_refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_refresh.ForeColor = System.Drawing.Color.White;
            this.btn_refresh.Location = new System.Drawing.Point(495, 81);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(128, 27);
            this.btn_refresh.TabIndex = 14;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.UseVisualStyleBackColor = false;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // OverviewPan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.btn_new_wallet);
            this.Controls.Add(this.lbl_connect);
            this.Controls.Add(this.lbl_block_height);
            this.Controls.Add(this.pan_addr);
            this.Controls.Add(this.btn_export_wallet);
            this.Controls.Add(this.btn_add_addr);
            this.Controls.Add(this.btn_open_wallet);
            this.Controls.Add(this.lbl_cmt_inf);
            this.Controls.Add(this.lbl_qrg_balance);
            this.Controls.Add(this.lbl_qrs_balance);
            this.Controls.Add(this.lbl_cmt_qrg_bal);
            this.Controls.Add(this.lbl_cmt_qrs_bal);
            this.Controls.Add(this.lbl_wallet_status);
            this.Controls.Add(this.lbl_overview_title);
            this.Name = "OverviewPan";
            this.Size = new System.Drawing.Size(665, 631);
            this.pan_addr.ResumeLayout(false);
            this.pan_addr_header.ResumeLayout(false);
            this.pan_addr_header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_overview_title;
        private System.Windows.Forms.Label lbl_wallet_status;
        private System.Windows.Forms.Label lbl_cmt_qrs_bal;
        private System.Windows.Forms.Label lbl_cmt_qrg_bal;
        private System.Windows.Forms.Label lbl_qrs_balance;
        private System.Windows.Forms.Label lbl_qrg_balance;
        private System.Windows.Forms.Label lbl_cmt_inf;
        private System.Windows.Forms.Button btn_open_wallet;
        private System.Windows.Forms.Button btn_add_addr;
        private System.Windows.Forms.Button btn_export_wallet;
        private System.Windows.Forms.Panel pan_addr;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Panel pan_addr_header;
        private System.Windows.Forms.Panel pan_split;
        private System.Windows.Forms.Label lbl_cmt_addr;
        private System.Windows.Forms.Label lbl_cmt_balance;
        private System.Windows.Forms.Label lbl_cmt_assets;
        private System.Windows.Forms.Panel pan_addr_main;
        private System.Windows.Forms.Label lbl_block_height;
        private System.Windows.Forms.Label lbl_connect;
        private System.Windows.Forms.Button btn_new_wallet;
        private System.Windows.Forms.Button btn_refresh;
    }
}
