namespace Quras_gui_SP.Controls
{
    partial class TxItem
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
            this.pan_color = new System.Windows.Forms.Panel();
            this.lbl_date = new System.Windows.Forms.Label();
            this.lbl_date_year = new System.Windows.Forms.Label();
            this.lbl_cmt_from = new System.Windows.Forms.Label();
            this.lbl_balance = new System.Windows.Forms.Label();
            this.lbl_result = new System.Windows.Forms.Label();
            this.txb_from = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pan_color
            // 
            this.pan_color.BackColor = System.Drawing.Color.Maroon;
            this.pan_color.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_color.Location = new System.Drawing.Point(0, 0);
            this.pan_color.Name = "pan_color";
            this.pan_color.Size = new System.Drawing.Size(5, 70);
            this.pan_color.TabIndex = 0;
            // 
            // lbl_date
            // 
            this.lbl_date.AutoSize = true;
            this.lbl_date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_date.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_date.Location = new System.Drawing.Point(22, 16);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Size = new System.Drawing.Size(62, 18);
            this.lbl_date.TabIndex = 1;
            this.lbl_date.Text = "JAN 25";
            // 
            // lbl_date_year
            // 
            this.lbl_date_year.AutoSize = true;
            this.lbl_date_year.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_date_year.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_date_year.Location = new System.Drawing.Point(23, 36);
            this.lbl_date_year.Name = "lbl_date_year";
            this.lbl_date_year.Size = new System.Drawing.Size(35, 13);
            this.lbl_date_year.TabIndex = 2;
            this.lbl_date_year.Text = "2018";
            // 
            // lbl_cmt_from
            // 
            this.lbl_cmt_from.AutoSize = true;
            this.lbl_cmt_from.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cmt_from.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_cmt_from.Location = new System.Drawing.Point(124, 16);
            this.lbl_cmt_from.Name = "lbl_cmt_from";
            this.lbl_cmt_from.Size = new System.Drawing.Size(48, 18);
            this.lbl_cmt_from.TabIndex = 3;
            this.lbl_cmt_from.Text = "From";
            // 
            // lbl_balance
            // 
            this.lbl_balance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_balance.Location = new System.Drawing.Point(393, 16);
            this.lbl_balance.Name = "lbl_balance";
            this.lbl_balance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl_balance.Size = new System.Drawing.Size(91, 20);
            this.lbl_balance.TabIndex = 5;
            this.lbl_balance.Text = "QRS 10";
            // 
            // lbl_result
            // 
            this.lbl_result.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_result.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_result.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.lbl_result.Location = new System.Drawing.Point(396, 36);
            this.lbl_result.Name = "lbl_result";
            this.lbl_result.Size = new System.Drawing.Size(87, 16);
            this.lbl_result.TabIndex = 6;
            this.lbl_result.Text = "complete";
            this.lbl_result.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txb_from
            // 
            this.txb_from.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.txb_from.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_from.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_from.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.txb_from.Location = new System.Drawing.Point(128, 37);
            this.txb_from.Name = "txb_from";
            this.txb_from.Size = new System.Drawing.Size(259, 12);
            this.txb_from.TabIndex = 7;
            this.txb_from.Text = "0x234234234234234234234234234234";
            // 
            // TxItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.txb_from);
            this.Controls.Add(this.lbl_result);
            this.Controls.Add(this.lbl_balance);
            this.Controls.Add(this.lbl_cmt_from);
            this.Controls.Add(this.lbl_date_year);
            this.Controls.Add(this.lbl_date);
            this.Controls.Add(this.pan_color);
            this.Name = "TxItem";
            this.Size = new System.Drawing.Size(500, 70);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pan_color;
        private System.Windows.Forms.Label lbl_date;
        private System.Windows.Forms.Label lbl_date_year;
        private System.Windows.Forms.Label lbl_cmt_from;
        private System.Windows.Forms.Label lbl_balance;
        private System.Windows.Forms.Label lbl_result;
        private System.Windows.Forms.TextBox txb_from;
    }
}
