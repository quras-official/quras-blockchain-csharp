namespace Quras_gui_SP.RightSidePans
{
    partial class TransactionRightSidePan
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
            this.lbl_title = new System.Windows.Forms.Label();
            this.pan_split = new System.Windows.Forms.Panel();
            this.btn_go_to_blockchain = new System.Windows.Forms.Button();
            this.pan_side_tx = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbl_title
            // 
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(78)))));
            this.lbl_title.Location = new System.Drawing.Point(14, 21);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(166, 21);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "RECENT TRANSACTIONS";
            // 
            // pan_split
            // 
            this.pan_split.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(26)))), ((int)(((byte)(31)))));
            this.pan_split.Location = new System.Drawing.Point(0, 45);
            this.pan_split.Name = "pan_split";
            this.pan_split.Size = new System.Drawing.Size(193, 1);
            this.pan_split.TabIndex = 1;
            // 
            // btn_go_to_blockchain
            // 
            this.btn_go_to_blockchain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(187)))), ((int)(((byte)(226)))));
            this.btn_go_to_blockchain.FlatAppearance.BorderSize = 0;
            this.btn_go_to_blockchain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_go_to_blockchain.ForeColor = System.Drawing.Color.White;
            this.btn_go_to_blockchain.Location = new System.Drawing.Point(15, 584);
            this.btn_go_to_blockchain.Name = "btn_go_to_blockchain";
            this.btn_go_to_blockchain.Size = new System.Drawing.Size(163, 38);
            this.btn_go_to_blockchain.TabIndex = 3;
            this.btn_go_to_blockchain.Text = "Go to blockchain";
            this.btn_go_to_blockchain.UseVisualStyleBackColor = false;
            // 
            // pan_side_tx
            // 
            this.pan_side_tx.Location = new System.Drawing.Point(0, 48);
            this.pan_side_tx.Name = "pan_side_tx";
            this.pan_side_tx.Size = new System.Drawing.Size(193, 530);
            this.pan_side_tx.TabIndex = 4;
            // 
            // TransactionRightSidePan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(31)))), ((int)(((byte)(37)))));
            this.Controls.Add(this.pan_side_tx);
            this.Controls.Add(this.btn_go_to_blockchain);
            this.Controls.Add(this.pan_split);
            this.Controls.Add(this.lbl_title);
            this.Name = "TransactionRightSidePan";
            this.Size = new System.Drawing.Size(193, 631);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Panel pan_split;
        private System.Windows.Forms.Button btn_go_to_blockchain;
        private System.Windows.Forms.Panel pan_side_tx;
    }
}
