namespace Quras_gui_SP.MainPans
{
    partial class TransactionsPan
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_overview_title = new System.Windows.Forms.Label();
            this.pan_history = new System.Windows.Forms.Panel();
            this.vsb_history = new System.Windows.Forms.VScrollBar();
            this.lbl_no_history = new System.Windows.Forms.Label();
            this.pan_history.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(47, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(569, 28);
            this.label1.TabIndex = 5;
            this.label1.Text = "You can check your transactions here.";
            // 
            // lbl_overview_title
            // 
            this.lbl_overview_title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_overview_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_overview_title.ForeColor = System.Drawing.Color.White;
            this.lbl_overview_title.Location = new System.Drawing.Point(47, 40);
            this.lbl_overview_title.Name = "lbl_overview_title";
            this.lbl_overview_title.Size = new System.Drawing.Size(569, 31);
            this.lbl_overview_title.TabIndex = 4;
            this.lbl_overview_title.Text = "Hello, coin transactions here.";
            // 
            // pan_history
            // 
            this.pan_history.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.pan_history.Controls.Add(this.lbl_no_history);
            this.pan_history.Controls.Add(this.vsb_history);
            this.pan_history.Location = new System.Drawing.Point(52, 123);
            this.pan_history.Name = "pan_history";
            this.pan_history.Size = new System.Drawing.Size(564, 472);
            this.pan_history.TabIndex = 6;
            // 
            // vsb_history
            // 
            this.vsb_history.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsb_history.Location = new System.Drawing.Point(549, 0);
            this.vsb_history.Name = "vsb_history";
            this.vsb_history.Size = new System.Drawing.Size(15, 472);
            this.vsb_history.TabIndex = 0;
            this.vsb_history.ValueChanged += new System.EventHandler(this.vsb_history_ValueChanged);
            // 
            // lbl_no_history
            // 
            this.lbl_no_history.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_no_history.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_no_history.ForeColor = System.Drawing.Color.Gray;
            this.lbl_no_history.Location = new System.Drawing.Point(-1, 21);
            this.lbl_no_history.Name = "lbl_no_history";
            this.lbl_no_history.Size = new System.Drawing.Size(547, 23);
            this.lbl_no_history.TabIndex = 1;
            this.lbl_no_history.Text = "There is no history.";
            this.lbl_no_history.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TransactionsPan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.pan_history);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_overview_title);
            this.Name = "TransactionsPan";
            this.Size = new System.Drawing.Size(665, 631);
            this.pan_history.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_overview_title;
        private System.Windows.Forms.Panel pan_history;
        private System.Windows.Forms.VScrollBar vsb_history;
        private System.Windows.Forms.Label lbl_no_history;
    }
}
