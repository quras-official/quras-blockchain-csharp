namespace Quras_gui_SP.Controls
{
    partial class AssetItem
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
            this.btn_delete = new System.Windows.Forms.Button();
            this.pan_color = new System.Windows.Forms.Panel();
            this.btn_edit = new System.Windows.Forms.Button();
            this.txb_assets_address = new System.Windows.Forms.MaskedTextBox();
            this.txb_assets_name = new System.Windows.Forms.TextBox();
            this.lbl_asset_type = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_delete
            // 
            this.btn_delete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_delete.FlatAppearance.BorderSize = 0;
            this.btn_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_delete.Image = global::Quras_gui_SP.Properties.Resources.remove;
            this.btn_delete.Location = new System.Drawing.Point(446, 38);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(25, 25);
            this.btn_delete.TabIndex = 9;
            this.btn_delete.UseVisualStyleBackColor = false;
            // 
            // pan_color
            // 
            this.pan_color.BackColor = System.Drawing.Color.Green;
            this.pan_color.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_color.Location = new System.Drawing.Point(0, 0);
            this.pan_color.Name = "pan_color";
            this.pan_color.Size = new System.Drawing.Size(5, 70);
            this.pan_color.TabIndex = 8;
            // 
            // btn_edit
            // 
            this.btn_edit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_edit.FlatAppearance.BorderSize = 0;
            this.btn_edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_edit.Image = global::Quras_gui_SP.Properties.Resources.edit;
            this.btn_edit.Location = new System.Drawing.Point(446, 7);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(25, 25);
            this.btn_edit.TabIndex = 7;
            this.btn_edit.UseVisualStyleBackColor = false;
            // 
            // txb_assets_address
            // 
            this.txb_assets_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.txb_assets_address.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_assets_address.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.txb_assets_address.Location = new System.Drawing.Point(31, 41);
            this.txb_assets_address.Name = "txb_assets_address";
            this.txb_assets_address.ReadOnly = true;
            this.txb_assets_address.Size = new System.Drawing.Size(397, 15);
            this.txb_assets_address.TabIndex = 6;
            this.txb_assets_address.Text = "0x322399u78970";
            // 
            // txb_assets_name
            // 
            this.txb_assets_name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.txb_assets_name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_assets_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_assets_name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.txb_assets_name.Location = new System.Drawing.Point(31, 11);
            this.txb_assets_name.Name = "txb_assets_name";
            this.txb_assets_name.ReadOnly = true;
            this.txb_assets_name.Size = new System.Drawing.Size(197, 21);
            this.txb_assets_name.TabIndex = 5;
            this.txb_assets_name.Text = "QRS";
            // 
            // lbl_asset_type
            // 
            this.lbl_asset_type.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_asset_type.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lbl_asset_type.Location = new System.Drawing.Point(234, 9);
            this.lbl_asset_type.Name = "lbl_asset_type";
            this.lbl_asset_type.Size = new System.Drawing.Size(206, 23);
            this.lbl_asset_type.TabIndex = 10;
            this.lbl_asset_type.Text = "Government Token";
            this.lbl_asset_type.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AssetItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.lbl_asset_type);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.pan_color);
            this.Controls.Add(this.btn_edit);
            this.Controls.Add(this.txb_assets_address);
            this.Controls.Add(this.txb_assets_name);
            this.Name = "AssetItem";
            this.Size = new System.Drawing.Size(476, 70);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Panel pan_color;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.MaskedTextBox txb_assets_address;
        private System.Windows.Forms.TextBox txb_assets_name;
        private System.Windows.Forms.Label lbl_asset_type;
    }
}
