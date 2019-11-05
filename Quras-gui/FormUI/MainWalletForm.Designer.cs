namespace Quras_gui.FormUI
{
    partial class MainWalletForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWalletForm));
            this.pan_header = new System.Windows.Forms.Panel();
            this.btn_claim_qrg = new DT_GUI_Modules.Controls.GlowButton();
            this.lbl_height = new System.Windows.Forms.Label();
            this.lbl_connected = new System.Windows.Forms.Label();
            this.btn_copy_address = new DT_GUI_Modules.Controls.GlowButton();
            this.btn_receive = new DT_GUI_Modules.Controls.GlowButton();
            this.btn_send = new DT_GUI_Modules.Controls.GlowButton();
            this.pan_side = new System.Windows.Forms.Panel();
            this.dashboardPan1 = new Quras_gui.Controls.Pans.DashboardPan();
            this.pan_main = new System.Windows.Forms.Panel();
            this.vsb_history = new System.Windows.Forms.VScrollBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbl_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_obj_about = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_lang = new System.Windows.Forms.ToolStripDropDownButton();
            this.japaneseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pan_history = new System.Windows.Forms.Panel();
            this.lbl_no_history = new System.Windows.Forms.Label();
            this.lbl_transaction_history = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pan_timer = new System.Windows.Forms.Timer(this.components);
            this.pan_header.SuspendLayout();
            this.pan_side.SuspendLayout();
            this.pan_main.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pan_history.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan_header
            // 
            this.pan_header.BackColor = System.Drawing.Color.White;
            this.pan_header.Controls.Add(this.btn_claim_qrg);
            this.pan_header.Controls.Add(this.lbl_height);
            this.pan_header.Controls.Add(this.lbl_connected);
            this.pan_header.Controls.Add(this.btn_copy_address);
            this.pan_header.Controls.Add(this.btn_receive);
            this.pan_header.Controls.Add(this.btn_send);
            this.pan_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan_header.Location = new System.Drawing.Point(0, 0);
            this.pan_header.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pan_header.Name = "pan_header";
            this.pan_header.Size = new System.Drawing.Size(955, 85);
            this.pan_header.TabIndex = 0;
            // 
            // btn_claim_qrg
            // 
            this.btn_claim_qrg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_claim_qrg.BackColor = System.Drawing.Color.White;
            this.btn_claim_qrg.FlatAppearance.BorderSize = 0;
            this.btn_claim_qrg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_claim_qrg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_claim_qrg.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_claim_qrg.GlowColor = System.Drawing.Color.Silver;
            this.btn_claim_qrg.Image = global::Quras_gui.Properties.Resources.claim;
            this.btn_claim_qrg.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_claim_qrg.Location = new System.Drawing.Point(290, 11);
            this.btn_claim_qrg.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_claim_qrg.Name = "btn_claim_qrg";
            this.btn_claim_qrg.Size = new System.Drawing.Size(91, 62);
            this.btn_claim_qrg.TabIndex = 5;
            this.btn_claim_qrg.Text = "Claim";
            this.btn_claim_qrg.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_claim_qrg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_claim_qrg.UseVisualStyleBackColor = false;
            this.btn_claim_qrg.Click += new System.EventHandler(this.btn_claim_qrg_Click);
            // 
            // lbl_height
            // 
            this.lbl_height.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_height.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_height.Location = new System.Drawing.Point(715, 42);
            this.lbl_height.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_height.Name = "lbl_height";
            this.lbl_height.Size = new System.Drawing.Size(230, 14);
            this.lbl_height.TabIndex = 4;
            this.lbl_height.Text = "Height : 0/0";
            this.lbl_height.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lbl_connected
            // 
            this.lbl_connected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_connected.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_connected.Location = new System.Drawing.Point(800, 57);
            this.lbl_connected.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_connected.Name = "lbl_connected";
            this.lbl_connected.Size = new System.Drawing.Size(145, 14);
            this.lbl_connected.TabIndex = 3;
            this.lbl_connected.Text = "connected : 0";
            this.lbl_connected.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btn_copy_address
            // 
            this.btn_copy_address.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_copy_address.BackColor = System.Drawing.Color.Transparent;
            this.btn_copy_address.FlatAppearance.BorderSize = 0;
            this.btn_copy_address.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_copy_address.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_copy_address.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_copy_address.GlowColor = System.Drawing.Color.Silver;
            this.btn_copy_address.Image = global::Quras_gui.Properties.Resources.copy_address;
            this.btn_copy_address.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_copy_address.Location = new System.Drawing.Point(582, 11);
            this.btn_copy_address.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_copy_address.Name = "btn_copy_address";
            this.btn_copy_address.Size = new System.Drawing.Size(91, 62);
            this.btn_copy_address.TabIndex = 2;
            this.btn_copy_address.Text = "Copy Address";
            this.btn_copy_address.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_copy_address.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_copy_address.UseVisualStyleBackColor = false;
            this.btn_copy_address.Click += new System.EventHandler(this.btn_copy_address_Click);
            this.btn_copy_address.MouseHover += new System.EventHandler(this.btn_copy_address_MouseHover);
            // 
            // btn_receive
            // 
            this.btn_receive.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_receive.BackColor = System.Drawing.Color.Transparent;
            this.btn_receive.FlatAppearance.BorderSize = 0;
            this.btn_receive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_receive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_receive.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_receive.GlowColor = System.Drawing.Color.Silver;
            this.btn_receive.Image = global::Quras_gui.Properties.Resources.receive;
            this.btn_receive.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_receive.Location = new System.Drawing.Point(486, 11);
            this.btn_receive.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_receive.Name = "btn_receive";
            this.btn_receive.Size = new System.Drawing.Size(91, 62);
            this.btn_receive.TabIndex = 1;
            this.btn_receive.Text = "Receive";
            this.btn_receive.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_receive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_receive.UseVisualStyleBackColor = false;
            this.btn_receive.Click += new System.EventHandler(this.btn_receive_Click);
            this.btn_receive.MouseHover += new System.EventHandler(this.btn_receive_MouseHover);
            // 
            // btn_send
            // 
            this.btn_send.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_send.BackColor = System.Drawing.Color.White;
            this.btn_send.FlatAppearance.BorderSize = 0;
            this.btn_send.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.btn_send.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_send.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_send.GlowColor = System.Drawing.Color.Silver;
            this.btn_send.Image = global::Quras_gui.Properties.Resources.send;
            this.btn_send.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_send.Location = new System.Drawing.Point(387, 11);
            this.btn_send.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(91, 62);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "Send\r\n";
            this.btn_send.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_send.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_send.UseVisualStyleBackColor = false;
            this.btn_send.Click += new System.EventHandler(this.btnSend_Click);
            this.btn_send.MouseHover += new System.EventHandler(this.btn_send_MouseHover);
            // 
            // pan_side
            // 
            this.pan_side.Controls.Add(this.dashboardPan1);
            this.pan_side.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_side.Location = new System.Drawing.Point(0, 85);
            this.pan_side.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pan_side.Name = "pan_side";
            this.pan_side.Size = new System.Drawing.Size(377, 508);
            this.pan_side.TabIndex = 2;
            // 
            // dashboardPan1
            // 
            this.dashboardPan1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dashboardPan1.BackgroundImage")));
            this.dashboardPan1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboardPan1.Location = new System.Drawing.Point(0, 0);
            this.dashboardPan1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dashboardPan1.Name = "dashboardPan1";
            this.dashboardPan1.Size = new System.Drawing.Size(377, 508);
            this.dashboardPan1.TabIndex = 0;
            this.dashboardPan1.MouseHover += new System.EventHandler(this.dashboardPan1_MouseHover);
            // 
            // pan_main
            // 
            this.pan_main.Controls.Add(this.vsb_history);
            this.pan_main.Controls.Add(this.statusStrip1);
            this.pan_main.Controls.Add(this.pan_history);
            this.pan_main.Controls.Add(this.lbl_transaction_history);
            this.pan_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_main.Location = new System.Drawing.Point(377, 85);
            this.pan_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pan_main.Name = "pan_main";
            this.pan_main.Size = new System.Drawing.Size(578, 508);
            this.pan_main.TabIndex = 3;
            // 
            // vsb_history
            // 
            this.vsb_history.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsb_history.Location = new System.Drawing.Point(557, 0);
            this.vsb_history.Name = "vsb_history";
            this.vsb_history.Size = new System.Drawing.Size(21, 482);
            this.vsb_history.TabIndex = 4;
            this.vsb_history.ValueChanged += new System.EventHandler(this.vsb_history_ValueChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbl_status,
            this.lbl_obj_about,
            this.btn_lang});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 482);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStrip1.Size = new System.Drawing.Size(578, 26);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbl_status
            // 
            this.lbl_status.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lbl_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(106, 21);
            this.lbl_status.Text = "PK Key is loading...";
            // 
            // lbl_obj_about
            // 
            this.lbl_obj_about.Name = "lbl_obj_about";
            this.lbl_obj_about.Size = new System.Drawing.Size(127, 21);
            this.lbl_obj_about.Text = "This is the send button";
            // 
            // btn_lang
            // 
            this.btn_lang.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_lang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.japaneseToolStripMenuItem,
            this.englishToolStripMenuItem});
            this.btn_lang.Image = global::Quras_gui.Properties.Resources.usa;
            this.btn_lang.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_lang.Name = "btn_lang";
            this.btn_lang.Size = new System.Drawing.Size(33, 24);
            this.btn_lang.Text = "toolStripDropDownButton1";
            this.btn_lang.Click += new System.EventHandler(this.btn_lang_Click);
            // 
            // japaneseToolStripMenuItem
            // 
            this.japaneseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("japaneseToolStripMenuItem.Image")));
            this.japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
            this.japaneseToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.japaneseToolStripMenuItem.Text = "Japanese";
            this.japaneseToolStripMenuItem.Click += new System.EventHandler(this.japaneseToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("englishToolStripMenuItem.Image")));
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // pan_history
            // 
            this.pan_history.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pan_history.Controls.Add(this.lbl_no_history);
            this.pan_history.Location = new System.Drawing.Point(22, 43);
            this.pan_history.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pan_history.Name = "pan_history";
            this.pan_history.Size = new System.Drawing.Size(538, 435);
            this.pan_history.TabIndex = 2;
            // 
            // lbl_no_history
            // 
            this.lbl_no_history.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbl_no_history.Location = new System.Drawing.Point(151, 22);
            this.lbl_no_history.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_no_history.Name = "lbl_no_history";
            this.lbl_no_history.Size = new System.Drawing.Size(236, 14);
            this.lbl_no_history.TabIndex = 0;
            this.lbl_no_history.Text = "There is no history";
            this.lbl_no_history.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_transaction_history
            // 
            this.lbl_transaction_history.AutoSize = true;
            this.lbl_transaction_history.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_transaction_history.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_transaction_history.Location = new System.Drawing.Point(18, 14);
            this.lbl_transaction_history.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_transaction_history.Name = "lbl_transaction_history";
            this.lbl_transaction_history.Size = new System.Drawing.Size(171, 17);
            this.lbl_transaction_history.TabIndex = 0;
            this.lbl_transaction_history.Text = "TRANSACTION HISOTRY";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pan_timer
            // 
            this.pan_timer.Interval = 500;
            this.pan_timer.Tick += new System.EventHandler(this.pan_timer_Tick);
            // 
            // MainWalletForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(955, 593);
            this.Controls.Add(this.pan_main);
            this.Controls.Add(this.pan_side);
            this.Controls.Add(this.pan_header);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximumSize = new System.Drawing.Size(971, 632);
            this.MinimumSize = new System.Drawing.Size(971, 632);
            this.Name = "MainWalletForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quras Wallet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWalletForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWalletForm_FormClosed);
            this.Load += new System.EventHandler(this.MainWalletForm_Load);
            this.pan_header.ResumeLayout(false);
            this.pan_side.ResumeLayout(false);
            this.pan_main.ResumeLayout(false);
            this.pan_main.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pan_history.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_header;
        private System.Windows.Forms.Panel pan_side;
        private System.Windows.Forms.Panel pan_main;
        private DT_GUI_Modules.Controls.GlowButton btn_copy_address;
        private DT_GUI_Modules.Controls.GlowButton btn_receive;
        private DT_GUI_Modules.Controls.GlowButton btn_send;
        private System.Windows.Forms.Label lbl_height;
        private System.Windows.Forms.Label lbl_connected;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer pan_timer;
        private System.Windows.Forms.Panel pan_history;
        private System.Windows.Forms.Label lbl_transaction_history;
        private Controls.Pans.DashboardPan dashboardPan1;
        private System.Windows.Forms.Label lbl_no_history;
        private System.Windows.Forms.VScrollBar vsb_history;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_status;
        private System.Windows.Forms.ToolStripStatusLabel lbl_obj_about;
        private System.Windows.Forms.ToolStripDropDownButton btn_lang;
        private System.Windows.Forms.ToolStripMenuItem japaneseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private DT_GUI_Modules.Controls.GlowButton btn_claim_qrg;
    }
}