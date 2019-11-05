namespace Quras_gui_SP.MainPans
{
    partial class AddrBookPan
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
            this.lbl_addrbook_status = new System.Windows.Forms.Label();
            this.lbl_addrbook_title = new System.Windows.Forms.Label();
            this.pan_addrbook = new System.Windows.Forms.Panel();
            this.lbl_no_history = new System.Windows.Forms.Label();
            this.vsb_addrbook = new System.Windows.Forms.VScrollBar();
            this.btn_add = new System.Windows.Forms.Button();
            this.pan_addrbook.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_addrbook_status
            // 
            this.lbl_addrbook_status.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_addrbook_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_addrbook_status.ForeColor = System.Drawing.Color.White;
            this.lbl_addrbook_status.Location = new System.Drawing.Point(48, 80);
            this.lbl_addrbook_status.Name = "lbl_addrbook_status";
            this.lbl_addrbook_status.Size = new System.Drawing.Size(573, 28);
            this.lbl_addrbook_status.TabIndex = 3;
            this.lbl_addrbook_status.Text = "You can add or romove your address.";
            // 
            // lbl_addrbook_title
            // 
            this.lbl_addrbook_title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_addrbook_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_addrbook_title.ForeColor = System.Drawing.Color.White;
            this.lbl_addrbook_title.Location = new System.Drawing.Point(48, 40);
            this.lbl_addrbook_title.Name = "lbl_addrbook_title";
            this.lbl_addrbook_title.Size = new System.Drawing.Size(573, 31);
            this.lbl_addrbook_title.TabIndex = 2;
            this.lbl_addrbook_title.Text = "Hello, this is your Address book.";
            // 
            // pan_addrbook
            // 
            this.pan_addrbook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.pan_addrbook.Controls.Add(this.lbl_no_history);
            this.pan_addrbook.Controls.Add(this.vsb_addrbook);
            this.pan_addrbook.Location = new System.Drawing.Point(53, 127);
            this.pan_addrbook.Name = "pan_addrbook";
            this.pan_addrbook.Size = new System.Drawing.Size(568, 448);
            this.pan_addrbook.TabIndex = 4;
            // 
            // lbl_no_history
            // 
            this.lbl_no_history.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_no_history.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_no_history.ForeColor = System.Drawing.Color.Gray;
            this.lbl_no_history.Location = new System.Drawing.Point(3, 18);
            this.lbl_no_history.Name = "lbl_no_history";
            this.lbl_no_history.Size = new System.Drawing.Size(547, 23);
            this.lbl_no_history.TabIndex = 2;
            this.lbl_no_history.Text = "There is no contact.";
            this.lbl_no_history.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // vsb_addrbook
            // 
            this.vsb_addrbook.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsb_addrbook.Location = new System.Drawing.Point(553, 0);
            this.vsb_addrbook.Name = "vsb_addrbook";
            this.vsb_addrbook.Size = new System.Drawing.Size(15, 448);
            this.vsb_addrbook.TabIndex = 1;
            this.vsb_addrbook.ValueChanged += new System.EventHandler(this.vsb_addrbook_ValueChanged);
            // 
            // btn_add
            // 
            this.btn_add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_add.FlatAppearance.BorderSize = 0;
            this.btn_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add.ForeColor = System.Drawing.Color.White;
            this.btn_add.Image = global::Quras_gui_SP.Properties.Resources.add;
            this.btn_add.Location = new System.Drawing.Point(532, 587);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(89, 31);
            this.btn_add.TabIndex = 5;
            this.btn_add.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_add.UseVisualStyleBackColor = false;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // AddrBookPan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.pan_addrbook);
            this.Controls.Add(this.lbl_addrbook_status);
            this.Controls.Add(this.lbl_addrbook_title);
            this.Name = "AddrBookPan";
            this.Size = new System.Drawing.Size(665, 631);
            this.pan_addrbook.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_addrbook_status;
        private System.Windows.Forms.Label lbl_addrbook_title;
        private System.Windows.Forms.Panel pan_addrbook;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.VScrollBar vsb_addrbook;
        private System.Windows.Forms.Label lbl_no_history;
    }
}
