namespace Quras_gui.Controls.Items
{
    partial class HistoryItem
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
            this.lbl_month = new System.Windows.Forms.Label();
            this.lbl_year = new System.Windows.Forms.Label();
            this.lbl_arrow = new System.Windows.Forms.Label();
            this.lbl_balance = new System.Windows.Forms.Label();
            this.lbl_status = new System.Windows.Forms.Label();
            this.txb_address = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pan_color
            // 
            this.pan_color.BackColor = System.Drawing.Color.Green;
            this.pan_color.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_color.Location = new System.Drawing.Point(0, 0);
            this.pan_color.Name = "pan_color";
            this.pan_color.Size = new System.Drawing.Size(5, 82);
            this.pan_color.TabIndex = 0;
            // 
            // lbl_month
            // 
            this.lbl_month.AutoSize = true;
            this.lbl_month.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_month.Location = new System.Drawing.Point(30, 20);
            this.lbl_month.Name = "lbl_month";
            this.lbl_month.Size = new System.Drawing.Size(64, 20);
            this.lbl_month.TabIndex = 1;
            this.lbl_month.Text = "JAN 25";
            // 
            // lbl_year
            // 
            this.lbl_year.AutoSize = true;
            this.lbl_year.Location = new System.Drawing.Point(31, 44);
            this.lbl_year.Name = "lbl_year";
            this.lbl_year.Size = new System.Drawing.Size(40, 17);
            this.lbl_year.TabIndex = 2;
            this.lbl_year.Text = "2018";
            // 
            // lbl_arrow
            // 
            this.lbl_arrow.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_arrow.AutoSize = true;
            this.lbl_arrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_arrow.Location = new System.Drawing.Point(131, 20);
            this.lbl_arrow.Name = "lbl_arrow";
            this.lbl_arrow.Size = new System.Drawing.Size(48, 20);
            this.lbl_arrow.TabIndex = 3;
            this.lbl_arrow.Text = "From";
            // 
            // lbl_balance
            // 
            this.lbl_balance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_balance.Location = new System.Drawing.Point(469, 20);
            this.lbl_balance.Name = "lbl_balance";
            this.lbl_balance.Size = new System.Drawing.Size(154, 20);
            this.lbl_balance.TabIndex = 5;
            this.lbl_balance.Text = "QRS 1,325";
            this.lbl_balance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_status
            // 
            this.lbl_status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_status.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_status.Location = new System.Drawing.Point(489, 44);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(134, 17);
            this.lbl_status.TabIndex = 6;
            this.lbl_status.Text = "Complet";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txb_address
            // 
            this.txb_address.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txb_address.BackColor = System.Drawing.Color.White;
            this.txb_address.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_address.Location = new System.Drawing.Point(135, 44);
            this.txb_address.Name = "txb_address";
            this.txb_address.ReadOnly = true;
            this.txb_address.Size = new System.Drawing.Size(327, 15);
            this.txb_address.TabIndex = 7;
            // 
            // HistoryItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txb_address);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.lbl_balance);
            this.Controls.Add(this.lbl_arrow);
            this.Controls.Add(this.lbl_year);
            this.Controls.Add(this.lbl_month);
            this.Controls.Add(this.pan_color);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "HistoryItem";
            this.Size = new System.Drawing.Size(641, 82);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HistoryItem_MouseDown);
            this.MouseHover += new System.EventHandler(this.HistoryItem_MouseHover);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pan_color;
        private System.Windows.Forms.Label lbl_month;
        private System.Windows.Forms.Label lbl_year;
        private System.Windows.Forms.Label lbl_arrow;
        private System.Windows.Forms.Label lbl_balance;
        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.TextBox txb_address;
    }
}
