namespace Quras_gui_SP.Controls
{
    partial class SettingsEngineItem
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
            this.btn_data_directory_path = new System.Windows.Forms.Button();
            this.pan_data_directory = new System.Windows.Forms.Panel();
            this.txb_data_directory_path = new DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox();
            this.lbl_data_directory_path = new System.Windows.Forms.Label();
            this.pan_data_directory.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_data_directory_path
            // 
            this.btn_data_directory_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_data_directory_path.BackgroundImage = global::Quras_gui_SP.Properties.Resources.add;
            this.btn_data_directory_path.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_data_directory_path.FlatAppearance.BorderSize = 0;
            this.btn_data_directory_path.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_data_directory_path.Location = new System.Drawing.Point(480, 48);
            this.btn_data_directory_path.Name = "btn_data_directory_path";
            this.btn_data_directory_path.Size = new System.Drawing.Size(74, 34);
            this.btn_data_directory_path.TabIndex = 17;
            this.btn_data_directory_path.UseVisualStyleBackColor = false;
            // 
            // pan_data_directory
            // 
            this.pan_data_directory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.pan_data_directory.Controls.Add(this.txb_data_directory_path);
            this.pan_data_directory.Location = new System.Drawing.Point(30, 48);
            this.pan_data_directory.Name = "pan_data_directory";
            this.pan_data_directory.Size = new System.Drawing.Size(443, 34);
            this.pan_data_directory.TabIndex = 16;
            // 
            // txb_data_directory_path
            // 
            this.txb_data_directory_path.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txb_data_directory_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.txb_data_directory_path.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_data_directory_path.ForeColor = System.Drawing.Color.White;
            this.txb_data_directory_path.Location = new System.Drawing.Point(10, 9);
            this.txb_data_directory_path.Name = "txb_data_directory_path";
            this.txb_data_directory_path.Size = new System.Drawing.Size(424, 15);
            this.txb_data_directory_path.TabIndex = 1;
            this.txb_data_directory_path.WaterMark = "Enter data directory path.";
            this.txb_data_directory_path.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txb_data_directory_path.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_data_directory_path.WaterMarkForeColor = System.Drawing.Color.Gray;
            // 
            // lbl_data_directory_path
            // 
            this.lbl_data_directory_path.AutoSize = true;
            this.lbl_data_directory_path.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_data_directory_path.ForeColor = System.Drawing.Color.White;
            this.lbl_data_directory_path.Location = new System.Drawing.Point(27, 23);
            this.lbl_data_directory_path.Name = "lbl_data_directory_path";
            this.lbl_data_directory_path.Size = new System.Drawing.Size(151, 17);
            this.lbl_data_directory_path.TabIndex = 15;
            this.lbl_data_directory_path.Text = "Data Directory Path";
            // 
            // SettingsEngineItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.Controls.Add(this.btn_data_directory_path);
            this.Controls.Add(this.pan_data_directory);
            this.Controls.Add(this.lbl_data_directory_path);
            this.Name = "SettingsEngineItem";
            this.Size = new System.Drawing.Size(568, 477);
            this.pan_data_directory.ResumeLayout(false);
            this.pan_data_directory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_data_directory_path;
        private System.Windows.Forms.Panel pan_data_directory;
        private DT_GUI_Modules.Controls.TextBox.WaterMarkTextBox txb_data_directory_path;
        private System.Windows.Forms.Label lbl_data_directory_path;
    }
}
