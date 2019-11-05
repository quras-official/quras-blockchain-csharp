namespace Quras_gui.Dialogs
{
    partial class ReceiveDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiveDialog));
            this.lbl_address = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_asset_type = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_balance = new MaterialSkin.Controls.MaterialLabel();
            this.txb_balance = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btn_make_qr_code = new MaterialSkin.Controls.MaterialFlatButton();
            this.pic_qr_code = new System.Windows.Forms.PictureBox();
            this.lbl_addr_comment = new System.Windows.Forms.Label();
            this.lbl_asset_comment = new System.Windows.Forms.Label();
            this.cmb_from = new System.Windows.Forms.ComboBox();
            this.cmb_assets = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_qr_code)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_address
            // 
            this.lbl_address.AutoSize = true;
            this.lbl_address.Depth = 0;
            this.lbl_address.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_address.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_address.Location = new System.Drawing.Point(32, 84);
            this.lbl_address.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_address.Name = "lbl_address";
            this.lbl_address.Size = new System.Drawing.Size(80, 24);
            this.lbl_address.TabIndex = 0;
            this.lbl_address.Text = "Address";
            // 
            // lbl_asset_type
            // 
            this.lbl_asset_type.AutoSize = true;
            this.lbl_asset_type.Depth = 0;
            this.lbl_asset_type.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_asset_type.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_asset_type.Location = new System.Drawing.Point(32, 172);
            this.lbl_asset_type.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_asset_type.Name = "lbl_asset_type";
            this.lbl_asset_type.Size = new System.Drawing.Size(114, 24);
            this.lbl_asset_type.TabIndex = 2;
            this.lbl_asset_type.Text = "Assets Type";
            // 
            // lbl_balance
            // 
            this.lbl_balance.AutoSize = true;
            this.lbl_balance.Depth = 0;
            this.lbl_balance.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_balance.Location = new System.Drawing.Point(32, 247);
            this.lbl_balance.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_balance.Name = "lbl_balance";
            this.lbl_balance.Size = new System.Drawing.Size(77, 24);
            this.lbl_balance.TabIndex = 4;
            this.lbl_balance.Text = "Balance";
            // 
            // txb_balance
            // 
            this.txb_balance.Depth = 0;
            this.txb_balance.Hint = "";
            this.txb_balance.Location = new System.Drawing.Point(36, 274);
            this.txb_balance.MouseState = MaterialSkin.MouseState.HOVER;
            this.txb_balance.Name = "txb_balance";
            this.txb_balance.PasswordChar = '\0';
            this.txb_balance.SelectedText = "";
            this.txb_balance.SelectionLength = 0;
            this.txb_balance.SelectionStart = 0;
            this.txb_balance.Size = new System.Drawing.Size(238, 28);
            this.txb_balance.TabIndex = 5;
            this.txb_balance.Text = "0";
            this.txb_balance.UseSystemPasswordChar = false;
            // 
            // btn_make_qr_code
            // 
            this.btn_make_qr_code.AutoSize = true;
            this.btn_make_qr_code.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_make_qr_code.Depth = 0;
            this.btn_make_qr_code.Location = new System.Drawing.Point(224, 561);
            this.btn_make_qr_code.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_make_qr_code.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_make_qr_code.Name = "btn_make_qr_code";
            this.btn_make_qr_code.Primary = false;
            this.btn_make_qr_code.Size = new System.Drawing.Size(141, 36);
            this.btn_make_qr_code.TabIndex = 6;
            this.btn_make_qr_code.Text = " Make QR-Code";
            this.btn_make_qr_code.UseVisualStyleBackColor = true;
            this.btn_make_qr_code.Click += new System.EventHandler(this.btn_make_qr_code_Click);
            // 
            // pic_qr_code
            // 
            this.pic_qr_code.BackColor = System.Drawing.Color.Transparent;
            this.pic_qr_code.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_qr_code.Location = new System.Drawing.Point(173, 317);
            this.pic_qr_code.Name = "pic_qr_code";
            this.pic_qr_code.Size = new System.Drawing.Size(253, 235);
            this.pic_qr_code.TabIndex = 7;
            this.pic_qr_code.TabStop = false;
            // 
            // lbl_addr_comment
            // 
            this.lbl_addr_comment.AutoSize = true;
            this.lbl_addr_comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_addr_comment.ForeColor = System.Drawing.Color.Gray;
            this.lbl_addr_comment.Location = new System.Drawing.Point(33, 140);
            this.lbl_addr_comment.Name = "lbl_addr_comment";
            this.lbl_addr_comment.Size = new System.Drawing.Size(280, 13);
            this.lbl_addr_comment.TabIndex = 8;
            this.lbl_addr_comment.Text = "Please input the address that you want to make a qr code.";
            // 
            // lbl_asset_comment
            // 
            this.lbl_asset_comment.AutoSize = true;
            this.lbl_asset_comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_asset_comment.ForeColor = System.Drawing.Color.Gray;
            this.lbl_asset_comment.Location = new System.Drawing.Point(33, 226);
            this.lbl_asset_comment.Name = "lbl_asset_comment";
            this.lbl_asset_comment.Size = new System.Drawing.Size(108, 13);
            this.lbl_asset_comment.TabIndex = 9;
            this.lbl_asset_comment.Text = "Select the asset type.";
            // 
            // cmb_from
            // 
            this.cmb_from.BackColor = System.Drawing.Color.White;
            this.cmb_from.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_from.ForeColor = System.Drawing.Color.Black;
            this.cmb_from.FormattingEnabled = true;
            this.cmb_from.Location = new System.Drawing.Point(36, 111);
            this.cmb_from.Name = "cmb_from";
            this.cmb_from.Size = new System.Drawing.Size(524, 24);
            this.cmb_from.TabIndex = 17;
            // 
            // cmb_assets
            // 
            this.cmb_assets.BackColor = System.Drawing.Color.White;
            this.cmb_assets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_assets.ForeColor = System.Drawing.Color.Black;
            this.cmb_assets.FormattingEnabled = true;
            this.cmb_assets.Location = new System.Drawing.Point(36, 199);
            this.cmb_assets.Name = "cmb_assets";
            this.cmb_assets.Size = new System.Drawing.Size(128, 24);
            this.cmb_assets.TabIndex = 18;
            // 
            // ReceiveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(591, 610);
            this.Controls.Add(this.cmb_assets);
            this.Controls.Add(this.cmb_from);
            this.Controls.Add(this.lbl_asset_comment);
            this.Controls.Add(this.lbl_addr_comment);
            this.Controls.Add(this.pic_qr_code);
            this.Controls.Add(this.btn_make_qr_code);
            this.Controls.Add(this.txb_balance);
            this.Controls.Add(this.lbl_balance);
            this.Controls.Add(this.lbl_asset_type);
            this.Controls.Add(this.lbl_address);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(591, 610);
            this.MinimumSize = new System.Drawing.Size(591, 610);
            this.Name = "ReceiveDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Receive";
            ((System.ComponentModel.ISupportInitialize)(this.pic_qr_code)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel lbl_address;
        private MaterialSkin.Controls.MaterialLabel lbl_asset_type;
        private MaterialSkin.Controls.MaterialLabel lbl_balance;
        private MaterialSkin.Controls.MaterialSingleLineTextField txb_balance;
        private MaterialSkin.Controls.MaterialFlatButton btn_make_qr_code;
        private System.Windows.Forms.PictureBox pic_qr_code;
        private System.Windows.Forms.Label lbl_addr_comment;
        private System.Windows.Forms.Label lbl_asset_comment;
        private System.Windows.Forms.ComboBox cmb_from;
        private System.Windows.Forms.ComboBox cmb_assets;
    }
}