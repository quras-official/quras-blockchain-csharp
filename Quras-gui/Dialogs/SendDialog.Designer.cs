namespace Quras_gui.Dialogs
{
    partial class SendDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendDialog));
            this.lbl_from_addr = new MaterialSkin.Controls.MaterialLabel();
            this.txb_recieve_addr = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.lbl_reciepent_addr = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_amount = new MaterialSkin.Controls.MaterialLabel();
            this.btn_send = new MaterialSkin.Controls.MaterialRaisedButton();
            this.txb_amount = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.cmb_from = new System.Windows.Forms.ComboBox();
            this.lbl_cmt_asset = new System.Windows.Forms.Label();
            this.cmb_assets = new System.Windows.Forms.ComboBox();
            this.lbl_cmt_amount = new System.Windows.Forms.Label();
            this.lbl_addr_comment = new System.Windows.Forms.Label();
            this.lbl_max = new System.Windows.Forms.Label();
            this.lbl_warning = new System.Windows.Forms.Label();
            this.btn_copy = new MaterialSkin.Controls.MaterialFlatButton();
            this.txb_max_amount = new Quras_gui.Controls.CustomizeControl.TextBox.AlphaBlendTextBox();
            this.SuspendLayout();
            // 
            // lbl_from_addr
            // 
            this.lbl_from_addr.AutoSize = true;
            this.lbl_from_addr.Depth = 0;
            this.lbl_from_addr.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_from_addr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_from_addr.Location = new System.Drawing.Point(26, 81);
            this.lbl_from_addr.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_from_addr.Name = "lbl_from_addr";
            this.lbl_from_addr.Size = new System.Drawing.Size(130, 24);
            this.lbl_from_addr.TabIndex = 23;
            this.lbl_from_addr.Text = "From Address";
            // 
            // txb_recieve_addr
            // 
            this.txb_recieve_addr.Depth = 0;
            this.txb_recieve_addr.Hint = "";
            this.txb_recieve_addr.Location = new System.Drawing.Point(33, 197);
            this.txb_recieve_addr.MouseState = MaterialSkin.MouseState.HOVER;
            this.txb_recieve_addr.Name = "txb_recieve_addr";
            this.txb_recieve_addr.PasswordChar = '\0';
            this.txb_recieve_addr.SelectedText = "";
            this.txb_recieve_addr.SelectionLength = 0;
            this.txb_recieve_addr.SelectionStart = 0;
            this.txb_recieve_addr.Size = new System.Drawing.Size(521, 28);
            this.txb_recieve_addr.TabIndex = 22;
            this.txb_recieve_addr.TabStop = false;
            this.txb_recieve_addr.UseSystemPasswordChar = false;
            // 
            // lbl_reciepent_addr
            // 
            this.lbl_reciepent_addr.AutoSize = true;
            this.lbl_reciepent_addr.Depth = 0;
            this.lbl_reciepent_addr.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_reciepent_addr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_reciepent_addr.Location = new System.Drawing.Point(29, 164);
            this.lbl_reciepent_addr.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_reciepent_addr.Name = "lbl_reciepent_addr";
            this.lbl_reciepent_addr.Size = new System.Drawing.Size(169, 24);
            this.lbl_reciepent_addr.TabIndex = 21;
            this.lbl_reciepent_addr.Text = "Reciepent Address";
            // 
            // lbl_amount
            // 
            this.lbl_amount.AutoSize = true;
            this.lbl_amount.Depth = 0;
            this.lbl_amount.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_amount.Location = new System.Drawing.Point(28, 271);
            this.lbl_amount.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_amount.Name = "lbl_amount";
            this.lbl_amount.Size = new System.Drawing.Size(156, 24);
            this.lbl_amount.TabIndex = 20;
            this.lbl_amount.Text = "Amount && Assets";
            // 
            // btn_send
            // 
            this.btn_send.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_send.Depth = 0;
            this.btn_send.Location = new System.Drawing.Point(474, 421);
            this.btn_send.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_send.Name = "btn_send";
            this.btn_send.Primary = true;
            this.btn_send.Size = new System.Drawing.Size(80, 31);
            this.btn_send.TabIndex = 19;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // txb_amount
            // 
            this.txb_amount.Depth = 0;
            this.txb_amount.Hint = "";
            this.txb_amount.Location = new System.Drawing.Point(30, 363);
            this.txb_amount.MouseState = MaterialSkin.MouseState.HOVER;
            this.txb_amount.Name = "txb_amount";
            this.txb_amount.PasswordChar = '\0';
            this.txb_amount.SelectedText = "";
            this.txb_amount.SelectionLength = 0;
            this.txb_amount.SelectionStart = 0;
            this.txb_amount.Size = new System.Drawing.Size(376, 28);
            this.txb_amount.TabIndex = 18;
            this.txb_amount.TabStop = false;
            this.txb_amount.UseSystemPasswordChar = false;
            // 
            // cmb_from
            // 
            this.cmb_from.BackColor = System.Drawing.Color.White;
            this.cmb_from.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_from.ForeColor = System.Drawing.Color.Black;
            this.cmb_from.FormattingEnabled = true;
            this.cmb_from.Location = new System.Drawing.Point(30, 117);
            this.cmb_from.Name = "cmb_from";
            this.cmb_from.Size = new System.Drawing.Size(524, 24);
            this.cmb_from.TabIndex = 16;
            this.cmb_from.SelectedIndexChanged += new System.EventHandler(this.cmb_from_SelectedIndexChanged);
            // 
            // lbl_cmt_asset
            // 
            this.lbl_cmt_asset.AutoSize = true;
            this.lbl_cmt_asset.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_asset.ForeColor = System.Drawing.Color.Gray;
            this.lbl_cmt_asset.Location = new System.Drawing.Point(30, 336);
            this.lbl_cmt_asset.Name = "lbl_cmt_asset";
            this.lbl_cmt_asset.Size = new System.Drawing.Size(146, 13);
            this.lbl_cmt_asset.TabIndex = 11;
            this.lbl_cmt_asset.Text = "Please check the assets type.";
            // 
            // cmb_assets
            // 
            this.cmb_assets.BackColor = System.Drawing.Color.White;
            this.cmb_assets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_assets.ForeColor = System.Drawing.Color.Black;
            this.cmb_assets.FormattingEnabled = true;
            this.cmb_assets.Location = new System.Drawing.Point(30, 303);
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
            this.lbl_cmt_amount.Location = new System.Drawing.Point(30, 399);
            this.lbl_cmt_amount.Name = "lbl_cmt_amount";
            this.lbl_cmt_amount.Size = new System.Drawing.Size(128, 13);
            this.lbl_cmt_amount.TabIndex = 9;
            this.lbl_cmt_amount.Text = "Please check the amount.";
            // 
            // lbl_addr_comment
            // 
            this.lbl_addr_comment.AutoSize = true;
            this.lbl_addr_comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_addr_comment.ForeColor = System.Drawing.Color.Gray;
            this.lbl_addr_comment.Location = new System.Drawing.Point(30, 232);
            this.lbl_addr_comment.Name = "lbl_addr_comment";
            this.lbl_addr_comment.Size = new System.Drawing.Size(305, 13);
            this.lbl_addr_comment.TabIndex = 5;
            this.lbl_addr_comment.Text = "Please only enter an QRS address. Funds will be lost otherwise.";
            // 
            // lbl_max
            // 
            this.lbl_max.AutoSize = true;
            this.lbl_max.BackColor = System.Drawing.Color.Transparent;
            this.lbl_max.Location = new System.Drawing.Point(413, 357);
            this.lbl_max.Name = "lbl_max";
            this.lbl_max.Size = new System.Drawing.Size(33, 17);
            this.lbl_max.TabIndex = 26;
            this.lbl_max.Text = "Max";
            // 
            // lbl_warning
            // 
            this.lbl_warning.BackColor = System.Drawing.Color.Transparent;
            this.lbl_warning.ForeColor = System.Drawing.Color.Red;
            this.lbl_warning.Location = new System.Drawing.Point(30, 427);
            this.lbl_warning.Name = "lbl_warning";
            this.lbl_warning.Size = new System.Drawing.Size(435, 23);
            this.lbl_warning.TabIndex = 27;
            this.lbl_warning.Text = "Warnings";
            this.lbl_warning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_copy
            // 
            this.btn_copy.AutoSize = true;
            this.btn_copy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_copy.Depth = 0;
            this.btn_copy.Location = new System.Drawing.Point(496, 76);
            this.btn_copy.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_copy.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_copy.Name = "btn_copy";
            this.btn_copy.Primary = false;
            this.btn_copy.Size = new System.Drawing.Size(58, 36);
            this.btn_copy.TabIndex = 28;
            this.btn_copy.Text = "Copy";
            this.btn_copy.UseVisualStyleBackColor = true;
            this.btn_copy.Click += new System.EventHandler(this.btn_copy_Click);
            // 
            // txb_max_amount
            // 
            this.txb_max_amount.BackAlpha = 10;
            this.txb_max_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txb_max_amount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_max_amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_max_amount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txb_max_amount.Location = new System.Drawing.Point(413, 374);
            this.txb_max_amount.Name = "txb_max_amount";
            this.txb_max_amount.Size = new System.Drawing.Size(141, 14);
            this.txb_max_amount.TabIndex = 25;
            this.txb_max_amount.Text = "1,234 QRS";
            // 
            // SendDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(580, 470);
            this.Controls.Add(this.btn_copy);
            this.Controls.Add(this.lbl_warning);
            this.Controls.Add(this.lbl_max);
            this.Controls.Add(this.txb_max_amount);
            this.Controls.Add(this.lbl_from_addr);
            this.Controls.Add(this.txb_recieve_addr);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.lbl_reciepent_addr);
            this.Controls.Add(this.lbl_amount);
            this.Controls.Add(this.lbl_addr_comment);
            this.Controls.Add(this.txb_amount);
            this.Controls.Add(this.lbl_cmt_amount);
            this.Controls.Add(this.cmb_from);
            this.Controls.Add(this.cmb_assets);
            this.Controls.Add(this.lbl_cmt_asset);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(580, 470);
            this.MinimumSize = new System.Drawing.Size(580, 470);
            this.Name = "SendDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send Coin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_cmt_asset;
        private System.Windows.Forms.ComboBox cmb_assets;
        private System.Windows.Forms.Label lbl_cmt_amount;
        private System.Windows.Forms.Label lbl_addr_comment;
        private System.Windows.Forms.ComboBox cmb_from;
        private MaterialSkin.Controls.MaterialSingleLineTextField txb_amount;
        private MaterialSkin.Controls.MaterialRaisedButton btn_send;
        private MaterialSkin.Controls.MaterialLabel lbl_from_addr;
        private MaterialSkin.Controls.MaterialSingleLineTextField txb_recieve_addr;
        private MaterialSkin.Controls.MaterialLabel lbl_reciepent_addr;
        private MaterialSkin.Controls.MaterialLabel lbl_amount;
        private Controls.CustomizeControl.TextBox.AlphaBlendTextBox txb_max_amount;
        private System.Windows.Forms.Label lbl_max;
        private System.Windows.Forms.Label lbl_warning;
        private MaterialSkin.Controls.MaterialFlatButton btn_copy;
    }
}