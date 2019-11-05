namespace Quras_gui.FormUI
{
    partial class DownloadKeyForm
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
            this.lbl_cmt_status = new MaterialSkin.Controls.MaterialLabel();
            this.btn_cancel = new MaterialSkin.Controls.MaterialFlatButton();
            this.btn_skip = new MaterialSkin.Controls.MaterialFlatButton();
            this.pan_percent_full = new System.Windows.Forms.Panel();
            this.pan_percent = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_status = new System.Windows.Forms.Label();
            this.pan_percent_full.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_cmt_status
            // 
            this.lbl_cmt_status.Depth = 0;
            this.lbl_cmt_status.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_cmt_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_cmt_status.Location = new System.Drawing.Point(12, 74);
            this.lbl_cmt_status.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_cmt_status.Name = "lbl_cmt_status";
            this.lbl_cmt_status.Size = new System.Drawing.Size(506, 58);
            this.lbl_cmt_status.TabIndex = 0;
            this.lbl_cmt_status.Text = "You have to download zk-snarks keys.\r\nWithout this, you can\'t use the anonymous t" +
    "ransaction.";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_cancel.Depth = 0;
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(452, 192);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_cancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Primary = false;
            this.btn_cancel.Size = new System.Drawing.Size(65, 36);
            this.btn_cancel.TabIndex = 86;
            this.btn_cancel.Text = "Close";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_skip
            // 
            this.btn_skip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_skip.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skip.Depth = 0;
            this.btn_skip.Location = new System.Drawing.Point(371, 192);
            this.btn_skip.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skip.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skip.Name = "btn_skip";
            this.btn_skip.Primary = false;
            this.btn_skip.Size = new System.Drawing.Size(73, 36);
            this.btn_skip.TabIndex = 85;
            this.btn_skip.Text = "Skip";
            this.btn_skip.UseVisualStyleBackColor = true;
            this.btn_skip.Click += new System.EventHandler(this.btn_skip_Click);
            // 
            // pan_percent_full
            // 
            this.pan_percent_full.BackColor = System.Drawing.Color.White;
            this.pan_percent_full.Controls.Add(this.pan_percent);
            this.pan_percent_full.Controls.Add(this.panel6);
            this.pan_percent_full.Controls.Add(this.panel5);
            this.pan_percent_full.Controls.Add(this.panel4);
            this.pan_percent_full.Controls.Add(this.panel3);
            this.pan_percent_full.Location = new System.Drawing.Point(12, 162);
            this.pan_percent_full.Name = "pan_percent_full";
            this.pan_percent_full.Size = new System.Drawing.Size(506, 21);
            this.pan_percent_full.TabIndex = 84;
            // 
            // pan_percent
            // 
            this.pan_percent.BackColor = System.Drawing.Color.Blue;
            this.pan_percent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_percent.Location = new System.Drawing.Point(1, 1);
            this.pan_percent.Name = "pan_percent";
            this.pan_percent.Size = new System.Drawing.Size(392, 19);
            this.pan_percent.TabIndex = 62;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 1);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1, 19);
            this.panel6.TabIndex = 61;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 20);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(505, 1);
            this.panel5.TabIndex = 60;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(505, 1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1, 20);
            this.panel4.TabIndex = 59;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(506, 1);
            this.panel3.TabIndex = 58;
            // 
            // lbl_status
            // 
            this.lbl_status.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_status.BackColor = System.Drawing.Color.Transparent;
            this.lbl_status.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_status.ForeColor = System.Drawing.Color.Black;
            this.lbl_status.Location = new System.Drawing.Point(14, 139);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(503, 16);
            this.lbl_status.TabIndex = 71;
            this.lbl_status.Text = "50%";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DownloadKeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 236);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_skip);
            this.Controls.Add(this.pan_percent_full);
            this.Controls.Add(this.lbl_cmt_status);
            this.Name = "DownloadKeyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download ZK-Snarks keys";
            this.pan_percent_full.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel lbl_cmt_status;
        private MaterialSkin.Controls.MaterialFlatButton btn_cancel;
        private MaterialSkin.Controls.MaterialFlatButton btn_skip;
        private System.Windows.Forms.Panel pan_percent_full;
        private System.Windows.Forms.Panel pan_percent;
        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
    }
}