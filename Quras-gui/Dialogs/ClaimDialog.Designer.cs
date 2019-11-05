namespace Quras_gui.Dialogs
{
    partial class ClaimDialog
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
            this.lbl_available = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_claim_max = new MaterialSkin.Controls.MaterialLabel();
            this.btn_claim = new MaterialSkin.Controls.MaterialRaisedButton();
            this.txb_available = new System.Windows.Forms.TextBox();
            this.txb_max = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_available
            // 
            this.lbl_available.AutoSize = true;
            this.lbl_available.Depth = 0;
            this.lbl_available.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_available.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_available.Location = new System.Drawing.Point(26, 102);
            this.lbl_available.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_available.Name = "lbl_available";
            this.lbl_available.Size = new System.Drawing.Size(156, 24);
            this.lbl_available.TabIndex = 0;
            this.lbl_available.Text = "Claim Available : ";
            // 
            // lbl_claim_max
            // 
            this.lbl_claim_max.AutoSize = true;
            this.lbl_claim_max.Depth = 0;
            this.lbl_claim_max.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_claim_max.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_claim_max.Location = new System.Drawing.Point(67, 148);
            this.lbl_claim_max.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_claim_max.Name = "lbl_claim_max";
            this.lbl_claim_max.Size = new System.Drawing.Size(115, 24);
            this.lbl_claim_max.TabIndex = 1;
            this.lbl_claim_max.Text = "Claim Max : ";
            // 
            // btn_claim
            // 
            this.btn_claim.Depth = 0;
            this.btn_claim.Location = new System.Drawing.Point(118, 205);
            this.btn_claim.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_claim.Name = "btn_claim";
            this.btn_claim.Primary = true;
            this.btn_claim.Size = new System.Drawing.Size(212, 23);
            this.btn_claim.TabIndex = 2;
            this.btn_claim.Text = "Claim";
            this.btn_claim.UseVisualStyleBackColor = true;
            this.btn_claim.Click += new System.EventHandler(this.btn_claim_Click);
            // 
            // txb_available
            // 
            this.txb_available.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_available.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_available.Location = new System.Drawing.Point(188, 104);
            this.txb_available.Name = "txb_available";
            this.txb_available.Size = new System.Drawing.Size(193, 23);
            this.txb_available.TabIndex = 3;
            // 
            // txb_max
            // 
            this.txb_max.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_max.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_max.Location = new System.Drawing.Point(188, 149);
            this.txb_max.Name = "txb_max";
            this.txb_max.Size = new System.Drawing.Size(193, 23);
            this.txb_max.TabIndex = 4;
            // 
            // ClaimDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(450, 249);
            this.Controls.Add(this.txb_max);
            this.Controls.Add(this.txb_available);
            this.Controls.Add(this.btn_claim);
            this.Controls.Add(this.lbl_claim_max);
            this.Controls.Add(this.lbl_available);
            this.MaximizeBox = false;
            this.Name = "ClaimDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Claim";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClaimDialog_FormClosing);
            this.Load += new System.EventHandler(this.ClaimDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel lbl_available;
        private MaterialSkin.Controls.MaterialLabel lbl_claim_max;
        private MaterialSkin.Controls.MaterialRaisedButton btn_claim;
        private System.Windows.Forms.TextBox txb_available;
        private System.Windows.Forms.TextBox txb_max;
    }
}