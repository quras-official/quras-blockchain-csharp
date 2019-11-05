namespace Quras_gui.Dialogs
{
    partial class AlertDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertDialog));
            this.tm_show_dlg = new System.Windows.Forms.Timer(this.components);
            this.pan_right = new System.Windows.Forms.Panel();
            this.pan_bottom = new System.Windows.Forms.Panel();
            this.pan_top = new System.Windows.Forms.Panel();
            this.pan_left = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_body = new System.Windows.Forms.Label();
            this.pan_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // tm_show_dlg
            // 
            this.tm_show_dlg.Interval = 50;
            this.tm_show_dlg.Tick += new System.EventHandler(this.tm_show_dlg_Tick);
            // 
            // pan_right
            // 
            this.pan_right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.pan_right.Location = new System.Drawing.Point(465, 0);
            this.pan_right.Name = "pan_right";
            this.pan_right.Size = new System.Drawing.Size(1, 76);
            this.pan_right.TabIndex = 51;
            // 
            // pan_bottom
            // 
            this.pan_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pan_bottom.Location = new System.Drawing.Point(0, 75);
            this.pan_bottom.Name = "pan_bottom";
            this.pan_bottom.Size = new System.Drawing.Size(465, 1);
            this.pan_bottom.TabIndex = 52;
            // 
            // pan_top
            // 
            this.pan_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_top.Controls.Add(this.pan_left);
            this.pan_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan_top.Location = new System.Drawing.Point(0, 0);
            this.pan_top.Name = "pan_top";
            this.pan_top.Size = new System.Drawing.Size(465, 1);
            this.pan_top.TabIndex = 53;
            // 
            // pan_left
            // 
            this.pan_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.pan_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_left.Location = new System.Drawing.Point(0, 0);
            this.pan_left.Name = "pan_left";
            this.pan_left.Size = new System.Drawing.Size(1, 1);
            this.pan_left.TabIndex = 53;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(114)))), ((int)(((byte)(123)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 74);
            this.panel1.TabIndex = 54;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(13, 13);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(37, 17);
            this.lbl_title.TabIndex = 55;
            this.lbl_title.Text = "Alert";
            this.lbl_title.Click += new System.EventHandler(this.AlertDialog_Click);
            // 
            // lbl_body
            // 
            this.lbl_body.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_body.BackColor = System.Drawing.Color.Transparent;
            this.lbl_body.ForeColor = System.Drawing.Color.White;
            this.lbl_body.Location = new System.Drawing.Point(68, 30);
            this.lbl_body.Name = "lbl_body";
            this.lbl_body.Size = new System.Drawing.Size(379, 17);
            this.lbl_body.TabIndex = 56;
            this.lbl_body.Text = "Send transaction was added successfully";
            this.lbl_body.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbl_body.Click += new System.EventHandler(this.AlertDialog_Click);
            // 
            // AlertDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(466, 76);
            this.Controls.Add(this.lbl_body);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pan_top);
            this.Controls.Add(this.pan_bottom);
            this.Controls.Add(this.pan_right);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AlertDialog";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AlertDialog";
            this.Load += new System.EventHandler(this.AlertDialog_Load);
            this.Click += new System.EventHandler(this.AlertDialog_Click);
            this.pan_top.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tm_show_dlg;
        private System.Windows.Forms.Panel pan_right;
        private System.Windows.Forms.Panel pan_bottom;
        private System.Windows.Forms.Panel pan_top;
        private System.Windows.Forms.Panel pan_left;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_body;
    }
}