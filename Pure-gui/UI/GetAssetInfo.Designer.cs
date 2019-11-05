namespace Pure.UI
{
    partial class GetAssetInfo
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
            this.btn_close = new System.Windows.Forms.Button();
            this.txb_quras_gas_asset = new System.Windows.Forms.TextBox();
            this.lbl_quras_gas_asset = new System.Windows.Forms.Label();
            this.txb_quras_asset = new System.Windows.Forms.TextBox();
            this.lbl_quras_asset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_close
            // 
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Location = new System.Drawing.Point(339, 139);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 9;
            this.btn_close.Text = "close";
            this.btn_close.UseVisualStyleBackColor = true;
            // 
            // txb_quras_gas_asset
            // 
            this.txb_quras_gas_asset.Location = new System.Drawing.Point(38, 104);
            this.txb_quras_gas_asset.Name = "txb_quras_gas_asset";
            this.txb_quras_gas_asset.ReadOnly = true;
            this.txb_quras_gas_asset.Size = new System.Drawing.Size(680, 22);
            this.txb_quras_gas_asset.TabIndex = 8;
            this.txb_quras_gas_asset.Text = "0";
            // 
            // lbl_quras_gas_asset
            // 
            this.lbl_quras_gas_asset.AutoSize = true;
            this.lbl_quras_gas_asset.Location = new System.Drawing.Point(35, 83);
            this.lbl_quras_gas_asset.Name = "lbl_quras_gas_asset";
            this.lbl_quras_gas_asset.Size = new System.Drawing.Size(39, 17);
            this.lbl_quras_gas_asset.TabIndex = 7;
            this.lbl_quras_gas_asset.Text = "XQG";
            // 
            // txb_quras_asset
            // 
            this.txb_quras_asset.Location = new System.Drawing.Point(38, 52);
            this.txb_quras_asset.Name = "txb_quras_asset";
            this.txb_quras_asset.ReadOnly = true;
            this.txb_quras_asset.Size = new System.Drawing.Size(680, 22);
            this.txb_quras_asset.TabIndex = 6;
            this.txb_quras_asset.Text = "0";
            // 
            // lbl_quras_asset
            // 
            this.lbl_quras_asset.AutoSize = true;
            this.lbl_quras_asset.Location = new System.Drawing.Point(35, 31);
            this.lbl_quras_asset.Name = "lbl_quras_asset";
            this.lbl_quras_asset.Size = new System.Drawing.Size(37, 17);
            this.lbl_quras_asset.TabIndex = 5;
            this.lbl_quras_asset.Text = "XQC";
            // 
            // GetAssetInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 177);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.txb_quras_gas_asset);
            this.Controls.Add(this.lbl_quras_gas_asset);
            this.Controls.Add(this.txb_quras_asset);
            this.Controls.Add(this.lbl_quras_asset);
            this.Name = "GetAssetInfo";
            this.Text = "GetAssetInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.TextBox txb_quras_gas_asset;
        private System.Windows.Forms.Label lbl_quras_gas_asset;
        private System.Windows.Forms.TextBox txb_quras_asset;
        private System.Windows.Forms.Label lbl_quras_asset;
    }
}