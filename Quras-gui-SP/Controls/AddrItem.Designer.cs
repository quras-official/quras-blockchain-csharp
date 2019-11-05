namespace Quras_gui_SP.Controls
{
    partial class AddrItem
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
            this.txb_name = new System.Windows.Forms.TextBox();
            this.txb_address = new System.Windows.Forms.MaskedTextBox();
            this.btn_edit = new System.Windows.Forms.Button();
            this.pan_color = new System.Windows.Forms.Panel();
            this.btn_delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txb_name
            // 
            this.txb_name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.txb_name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.txb_name.Location = new System.Drawing.Point(28, 11);
            this.txb_name.Name = "txb_name";
            this.txb_name.ReadOnly = true;
            this.txb_name.Size = new System.Drawing.Size(432, 21);
            this.txb_name.TabIndex = 0;
            this.txb_name.Text = "Wang XiaoMing";
            // 
            // txb_address
            // 
            this.txb_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.txb_address.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_address.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(146)))), ((int)(((byte)(146)))));
            this.txb_address.Location = new System.Drawing.Point(28, 41);
            this.txb_address.Name = "txb_address";
            this.txb_address.ReadOnly = true;
            this.txb_address.Size = new System.Drawing.Size(432, 15);
            this.txb_address.TabIndex = 1;
            this.txb_address.Text = "0x234234234234234234234234234234";
            // 
            // btn_edit
            // 
            this.btn_edit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_edit.FlatAppearance.BorderSize = 0;
            this.btn_edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_edit.Image = global::Quras_gui_SP.Properties.Resources.edit;
            this.btn_edit.Location = new System.Drawing.Point(485, 7);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(25, 25);
            this.btn_edit.TabIndex = 2;
            this.btn_edit.UseVisualStyleBackColor = false;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // pan_color
            // 
            this.pan_color.BackColor = System.Drawing.Color.Green;
            this.pan_color.Dock = System.Windows.Forms.DockStyle.Left;
            this.pan_color.Location = new System.Drawing.Point(0, 0);
            this.pan_color.Name = "pan_color";
            this.pan_color.Size = new System.Drawing.Size(5, 70);
            this.pan_color.TabIndex = 3;
            // 
            // btn_delete
            // 
            this.btn_delete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_delete.FlatAppearance.BorderSize = 0;
            this.btn_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_delete.Image = global::Quras_gui_SP.Properties.Resources.remove;
            this.btn_delete.Location = new System.Drawing.Point(485, 38);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(25, 25);
            this.btn_delete.TabIndex = 4;
            this.btn_delete.UseVisualStyleBackColor = false;
            this.btn_delete.Click += new System.EventHandler(this.button2_Click);
            // 
            // AddrItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.pan_color);
            this.Controls.Add(this.btn_edit);
            this.Controls.Add(this.txb_address);
            this.Controls.Add(this.txb_name);
            this.Name = "AddrItem";
            this.Size = new System.Drawing.Size(518, 70);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txb_name;
        private System.Windows.Forms.MaskedTextBox txb_address;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Panel pan_color;
        private System.Windows.Forms.Button btn_delete;
    }
}
