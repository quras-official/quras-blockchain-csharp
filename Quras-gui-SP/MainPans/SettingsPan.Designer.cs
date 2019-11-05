namespace Quras_gui_SP.MainPans
{
    partial class SettingsPan
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
            this.lbl_overview_title = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_engine = new System.Windows.Forms.Button();
            this.btn_others = new System.Windows.Forms.Button();
            this.pan_settings = new System.Windows.Forms.Panel();
            this.pan_select = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_overview_title
            // 
            this.lbl_overview_title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_overview_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_overview_title.ForeColor = System.Drawing.Color.White;
            this.lbl_overview_title.Location = new System.Drawing.Point(47, 40);
            this.lbl_overview_title.Name = "lbl_overview_title";
            this.lbl_overview_title.Size = new System.Drawing.Size(535, 31);
            this.lbl_overview_title.TabIndex = 5;
            this.lbl_overview_title.Text = "Settings";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Quras_gui_SP.Properties.Resources.settings;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(570, 61);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // btn_engine
            // 
            this.btn_engine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_engine.FlatAppearance.BorderSize = 0;
            this.btn_engine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_engine.ForeColor = System.Drawing.Color.White;
            this.btn_engine.Location = new System.Drawing.Point(52, 84);
            this.btn_engine.Name = "btn_engine";
            this.btn_engine.Size = new System.Drawing.Size(78, 27);
            this.btn_engine.TabIndex = 7;
            this.btn_engine.Text = "Engine";
            this.btn_engine.UseVisualStyleBackColor = false;
            this.btn_engine.Click += new System.EventHandler(this.btn_engine_Click);
            // 
            // btn_others
            // 
            this.btn_others.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(86)))), ((int)(((byte)(99)))));
            this.btn_others.FlatAppearance.BorderSize = 0;
            this.btn_others.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_others.ForeColor = System.Drawing.Color.White;
            this.btn_others.Location = new System.Drawing.Point(131, 84);
            this.btn_others.Name = "btn_others";
            this.btn_others.Size = new System.Drawing.Size(78, 27);
            this.btn_others.TabIndex = 8;
            this.btn_others.Text = "Others";
            this.btn_others.UseVisualStyleBackColor = false;
            this.btn_others.Click += new System.EventHandler(this.btn_others_Click);
            // 
            // pan_settings
            // 
            this.pan_settings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.pan_settings.Location = new System.Drawing.Point(52, 117);
            this.pan_settings.Name = "pan_settings";
            this.pan_settings.Size = new System.Drawing.Size(568, 477);
            this.pan_settings.TabIndex = 9;
            // 
            // pan_select
            // 
            this.pan_select.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(46)))), ((int)(((byte)(55)))));
            this.pan_select.Location = new System.Drawing.Point(52, 112);
            this.pan_select.Name = "pan_select";
            this.pan_select.Size = new System.Drawing.Size(78, 5);
            this.pan_select.TabIndex = 10;
            // 
            // SettingsPan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.pan_select);
            this.Controls.Add(this.pan_settings);
            this.Controls.Add(this.btn_others);
            this.Controls.Add(this.btn_engine);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbl_overview_title);
            this.Name = "SettingsPan";
            this.Size = new System.Drawing.Size(665, 631);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_overview_title;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_engine;
        private System.Windows.Forms.Button btn_others;
        private System.Windows.Forms.Panel pan_settings;
        private System.Windows.Forms.Panel pan_select;
    }
}
