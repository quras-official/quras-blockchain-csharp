namespace Quras_gui.FormUI
{
    partial class UpdateWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateWindow));
            this.pan_percent_full = new System.Windows.Forms.Panel();
            this.pan_percent = new System.Windows.Forms.Panel();
            this.lbl_percent = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txb_update_logs = new System.Windows.Forms.TextBox();
            this.lbl_update_version = new MaterialSkin.Controls.MaterialLabel();
            this.txb_update_version = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_change_log = new MaterialSkin.Controls.MaterialLabel();
            this.btn_update = new MaterialSkin.Controls.MaterialFlatButton();
            this.btn_cancel = new MaterialSkin.Controls.MaterialFlatButton();
            this.pan_percent_full.SuspendLayout();
            this.pan_percent.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan_percent_full
            // 
            this.pan_percent_full.BackColor = System.Drawing.Color.White;
            this.pan_percent_full.Controls.Add(this.pan_percent);
            this.pan_percent_full.Controls.Add(this.panel6);
            this.pan_percent_full.Controls.Add(this.panel5);
            this.pan_percent_full.Controls.Add(this.panel4);
            this.pan_percent_full.Controls.Add(this.panel3);
            this.pan_percent_full.Location = new System.Drawing.Point(40, 355);
            this.pan_percent_full.Name = "pan_percent_full";
            this.pan_percent_full.Size = new System.Drawing.Size(547, 21);
            this.pan_percent_full.TabIndex = 78;
            // 
            // pan_percent
            // 
            this.pan_percent.BackColor = System.Drawing.Color.Blue;
            this.pan_percent.Controls.Add(this.lbl_percent);
            this.pan_percent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_percent.Location = new System.Drawing.Point(1, 1);
            this.pan_percent.Name = "pan_percent";
            this.pan_percent.Size = new System.Drawing.Size(392, 19);
            this.pan_percent.TabIndex = 62;
            // 
            // lbl_percent
            // 
            this.lbl_percent.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_percent.BackColor = System.Drawing.Color.Transparent;
            this.lbl_percent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_percent.ForeColor = System.Drawing.Color.White;
            this.lbl_percent.Location = new System.Drawing.Point(303, 1);
            this.lbl_percent.Name = "lbl_percent";
            this.lbl_percent.Size = new System.Drawing.Size(86, 16);
            this.lbl_percent.TabIndex = 71;
            this.lbl_percent.Text = "50%";
            this.lbl_percent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.panel5.Size = new System.Drawing.Size(546, 1);
            this.panel5.TabIndex = 60;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(546, 1);
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
            this.panel3.Size = new System.Drawing.Size(547, 1);
            this.panel3.TabIndex = 58;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txb_update_logs);
            this.panel1.Location = new System.Drawing.Point(40, 170);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(547, 178);
            this.panel1.TabIndex = 76;
            // 
            // txb_update_logs
            // 
            this.txb_update_logs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_update_logs.BackColor = System.Drawing.Color.White;
            this.txb_update_logs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_update_logs.ForeColor = System.Drawing.Color.Black;
            this.txb_update_logs.Location = new System.Drawing.Point(16, 15);
            this.txb_update_logs.Margin = new System.Windows.Forms.Padding(20);
            this.txb_update_logs.Multiline = true;
            this.txb_update_logs.Name = "txb_update_logs";
            this.txb_update_logs.Size = new System.Drawing.Size(512, 145);
            this.txb_update_logs.TabIndex = 66;
            this.txb_update_logs.Text = "Init Commit.\r\n * Updating the bugs\r\n";
            // 
            // lbl_update_version
            // 
            this.lbl_update_version.AutoSize = true;
            this.lbl_update_version.Depth = 0;
            this.lbl_update_version.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_update_version.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_update_version.Location = new System.Drawing.Point(37, 91);
            this.lbl_update_version.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_update_version.Name = "lbl_update_version";
            this.lbl_update_version.Size = new System.Drawing.Size(153, 24);
            this.lbl_update_version.TabIndex = 79;
            this.lbl_update_version.Text = "Newest Version :";
            // 
            // txb_update_version
            // 
            this.txb_update_version.AutoSize = true;
            this.txb_update_version.Depth = 0;
            this.txb_update_version.Font = new System.Drawing.Font("Roboto", 11F);
            this.txb_update_version.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txb_update_version.Location = new System.Drawing.Point(196, 91);
            this.txb_update_version.MouseState = MaterialSkin.MouseState.HOVER;
            this.txb_update_version.Name = "txb_update_version";
            this.txb_update_version.Size = new System.Drawing.Size(173, 24);
            this.txb_update_version.TabIndex = 80;
            this.txb_update_version.Text = "txb_update_version";
            // 
            // lbl_change_log
            // 
            this.lbl_change_log.AutoSize = true;
            this.lbl_change_log.Depth = 0;
            this.lbl_change_log.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_change_log.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_change_log.Location = new System.Drawing.Point(37, 137);
            this.lbl_change_log.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_change_log.Name = "lbl_change_log";
            this.lbl_change_log.Size = new System.Drawing.Size(112, 24);
            this.lbl_change_log.TabIndex = 81;
            this.lbl_change_log.Text = "Update logs";
            // 
            // btn_update
            // 
            this.btn_update.AutoSize = true;
            this.btn_update.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_update.Depth = 0;
            this.btn_update.Location = new System.Drawing.Point(436, 396);
            this.btn_update.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_update.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_update.Name = "btn_update";
            this.btn_update.Primary = false;
            this.btn_update.Size = new System.Drawing.Size(78, 36);
            this.btn_update.TabIndex = 82;
            this.btn_update.Text = "Update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.AutoSize = true;
            this.btn_cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_cancel.Depth = 0;
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(522, 396);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_cancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Primary = false;
            this.btn_cancel.Size = new System.Drawing.Size(65, 36);
            this.btn_cancel.TabIndex = 83;
            this.btn_cancel.Text = "Close";
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // UpdateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(622, 447);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_update);
            this.Controls.Add(this.lbl_change_log);
            this.Controls.Add(this.txb_update_version);
            this.Controls.Add(this.lbl_update_version);
            this.Controls.Add(this.pan_percent_full);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateWindow";
            this.pan_percent_full.ResumeLayout(false);
            this.pan_percent.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pan_percent_full;
        private System.Windows.Forms.Panel pan_percent;
        private System.Windows.Forms.Label lbl_percent;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txb_update_logs;
        private MaterialSkin.Controls.MaterialLabel lbl_update_version;
        private MaterialSkin.Controls.MaterialLabel txb_update_version;
        private MaterialSkin.Controls.MaterialLabel lbl_change_log;
        private MaterialSkin.Controls.MaterialFlatButton btn_update;
        private MaterialSkin.Controls.MaterialFlatButton btn_cancel;
    }
}