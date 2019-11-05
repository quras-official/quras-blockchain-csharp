namespace DT_GUI_Modules.Modules
{
    partial class SendPanControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendPanControl));
            this.lbl_from = new System.Windows.Forms.Label();
            this.cmb_assets = new System.Windows.Forms.ComboBox();
            this.bunifuCustomLabel1 = new System.Windows.Forms.Label();
            this.txb_to_address = new System.Windows.Forms.TextBox();
            this.bunifuCustomLabel2 = new System.Windows.Forms.Label();
            this.bunifuCustomLabel3 = new System.Windows.Forms.Label();
            this.txb_amount = new System.Windows.Forms.TextBox();
            this.bunifuThinButton21 = new System.Windows.Forms.Button();
            this.txb_balance = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_from
            // 
            this.lbl_from.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_from.AutoSize = true;
            this.lbl_from.BackColor = System.Drawing.Color.Transparent;
            this.lbl_from.ForeColor = System.Drawing.Color.White;
            this.lbl_from.Location = new System.Drawing.Point(32, 36);
            this.lbl_from.Name = "lbl_from";
            this.lbl_from.Size = new System.Drawing.Size(51, 17);
            this.lbl_from.TabIndex = 0;
            this.lbl_from.Text = "Asset :";
            // 
            // cmb_assets
            // 
            this.cmb_assets.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmb_assets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_assets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_assets.FormattingEnabled = true;
            this.cmb_assets.Location = new System.Drawing.Point(90, 35);
            this.cmb_assets.Name = "cmb_assets";
            this.cmb_assets.Size = new System.Drawing.Size(355, 24);
            this.cmb_assets.TabIndex = 1;
            this.cmb_assets.SelectedIndexChanged += new System.EventHandler(this.cmb_assets_SelectedIndexChanged);
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuCustomLabel1.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(50, 115);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(33, 17);
            this.bunifuCustomLabel1.TabIndex = 2;
            this.bunifuCustomLabel1.Text = "To :";
            // 
            // txb_to_address
            // 
            this.txb_to_address.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txb_to_address.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txb_to_address.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_to_address.Location = new System.Drawing.Point(90, 112);
            this.txb_to_address.Name = "txb_to_address";
            this.txb_to_address.Size = new System.Drawing.Size(355, 24);
            this.txb_to_address.TabIndex = 3;
            // 
            // bunifuCustomLabel2
            // 
            this.bunifuCustomLabel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bunifuCustomLabel2.AutoSize = true;
            this.bunifuCustomLabel2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuCustomLabel2.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel2.Location = new System.Drawing.Point(16, 76);
            this.bunifuCustomLabel2.Name = "bunifuCustomLabel2";
            this.bunifuCustomLabel2.Size = new System.Drawing.Size(67, 17);
            this.bunifuCustomLabel2.TabIndex = 4;
            this.bunifuCustomLabel2.Text = "Balance :";
            // 
            // bunifuCustomLabel3
            // 
            this.bunifuCustomLabel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bunifuCustomLabel3.AutoSize = true;
            this.bunifuCustomLabel3.BackColor = System.Drawing.Color.Transparent;
            this.bunifuCustomLabel3.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel3.Location = new System.Drawing.Point(19, 153);
            this.bunifuCustomLabel3.Name = "bunifuCustomLabel3";
            this.bunifuCustomLabel3.Size = new System.Drawing.Size(64, 17);
            this.bunifuCustomLabel3.TabIndex = 6;
            this.bunifuCustomLabel3.Text = "Amount :";
            // 
            // txb_amount
            // 
            this.txb_amount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txb_amount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txb_amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_amount.Location = new System.Drawing.Point(90, 152);
            this.txb_amount.Name = "txb_amount";
            this.txb_amount.Size = new System.Drawing.Size(355, 24);
            this.txb_amount.TabIndex = 7;
            // 
            // bunifuThinButton21
            // 
            this.bunifuThinButton21.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bunifuThinButton21.BackColor = System.Drawing.Color.Transparent;
            this.bunifuThinButton21.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuThinButton21.BackgroundImage")));
            this.bunifuThinButton21.Text = "Send";
            this.bunifuThinButton21.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuThinButton21.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuThinButton21.ForeColor = System.Drawing.Color.SeaGreen;
            this.bunifuThinButton21.Location = new System.Drawing.Point(189, 186);
            this.bunifuThinButton21.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuThinButton21.Name = "bunifuThinButton21";
            this.bunifuThinButton21.Size = new System.Drawing.Size(107, 39);
            this.bunifuThinButton21.TabIndex = 8;
            this.bunifuThinButton21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuThinButton21.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // txb_balance
            // 
            this.txb_balance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txb_balance.BackColor = System.Drawing.Color.White;
            this.txb_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txb_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_balance.Location = new System.Drawing.Point(90, 74);
            this.txb_balance.Name = "txb_balance";
            this.txb_balance.ReadOnly = true;
            this.txb_balance.Size = new System.Drawing.Size(355, 24);
            this.txb_balance.TabIndex = 9;
            // 
            // SendPanControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::DT_GUI_Modules.Properties.Resources.background_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.txb_balance);
            this.Controls.Add(this.bunifuThinButton21);
            this.Controls.Add(this.txb_amount);
            this.Controls.Add(this.bunifuCustomLabel3);
            this.Controls.Add(this.bunifuCustomLabel2);
            this.Controls.Add(this.txb_to_address);
            this.Controls.Add(this.bunifuCustomLabel1);
            this.Controls.Add(this.cmb_assets);
            this.Controls.Add(this.lbl_from);
            this.Name = "SendPanControl";
            this.Size = new System.Drawing.Size(481, 248);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_from;
        private System.Windows.Forms.ComboBox cmb_assets;
        private System.Windows.Forms.Label bunifuCustomLabel1;
        private System.Windows.Forms.TextBox txb_to_address;
        private System.Windows.Forms.Label bunifuCustomLabel2;
        private System.Windows.Forms.Label bunifuCustomLabel3;
        private System.Windows.Forms.TextBox txb_amount;
        private System.Windows.Forms.Button bunifuThinButton21;
        private System.Windows.Forms.TextBox txb_balance;
    }
}
