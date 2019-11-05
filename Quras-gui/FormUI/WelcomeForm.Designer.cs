using DT_GUI_Modules.Controls;
namespace Quras_gui.FormUI
{
    partial class WelcomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            this.pan_background = new System.Windows.Forms.Panel();
            this.btn_restore = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btn_new_wallet = new MaterialSkin.Controls.MaterialRaisedButton();
            this.bunifuFlatButton1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_small_info = new System.Windows.Forms.Label();
            this.lbl_description_bold = new System.Windows.Forms.Label();
            this.lbl_quras_title = new System.Windows.Forms.Label();
            this.pan_background.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pan_background
            // 
            this.pan_background.BackColor = System.Drawing.Color.Transparent;
            this.pan_background.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pan_background.BackgroundImage")));
            this.pan_background.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pan_background.Controls.Add(this.btn_restore);
            this.pan_background.Controls.Add(this.btn_new_wallet);
            this.pan_background.Controls.Add(this.bunifuFlatButton1);
            this.pan_background.Controls.Add(this.pictureBox1);
            this.pan_background.Controls.Add(this.lbl_small_info);
            this.pan_background.Controls.Add(this.lbl_description_bold);
            this.pan_background.Controls.Add(this.lbl_quras_title);
            this.pan_background.Location = new System.Drawing.Point(0, 0);
            this.pan_background.Name = "pan_background";
            this.pan_background.Size = new System.Drawing.Size(1192, 733);
            this.pan_background.TabIndex = 0;
            // 
            // btn_restore
            // 
            this.btn_restore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_restore.Depth = 0;
            this.btn_restore.Location = new System.Drawing.Point(636, 550);
            this.btn_restore.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_restore.Name = "btn_restore";
            this.btn_restore.Primary = true;
            this.btn_restore.Size = new System.Drawing.Size(151, 36);
            this.btn_restore.TabIndex = 10;
            this.btn_restore.Text = "Restore";
            this.btn_restore.UseVisualStyleBackColor = true;
            this.btn_restore.Click += new System.EventHandler(this.btn_resotre_Click);
            // 
            // btn_new_wallet
            // 
            this.btn_new_wallet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_new_wallet.Depth = 0;
            this.btn_new_wallet.Location = new System.Drawing.Point(405, 550);
            this.btn_new_wallet.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_new_wallet.Name = "btn_new_wallet";
            this.btn_new_wallet.Primary = true;
            this.btn_new_wallet.Size = new System.Drawing.Size(151, 36);
            this.btn_new_wallet.TabIndex = 9;
            this.btn_new_wallet.Text = "New Wallet";
            this.btn_new_wallet.UseVisualStyleBackColor = true;
            this.btn_new_wallet.Click += new System.EventHandler(this.btn_new_wallet_Click);
            // 
            // bunifuFlatButton1
            // 
            this.bunifuFlatButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.bunifuFlatButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuFlatButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuFlatButton1.Location = new System.Drawing.Point(-28, -57);
            this.bunifuFlatButton1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuFlatButton1.Name = "bunifuFlatButton1";
            this.bunifuFlatButton1.Size = new System.Drawing.Size(321, 59);
            this.bunifuFlatButton1.TabIndex = 6;
            this.bunifuFlatButton1.Text = "bunifuFlatButton1";
            this.bunifuFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bunifuFlatButton1.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::Quras_gui.Properties.Resources.welcome_mark;
            this.pictureBox1.Location = new System.Drawing.Point(85, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(230, 230);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_small_info
            // 
            this.lbl_small_info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_small_info.BackColor = System.Drawing.Color.Transparent;
            this.lbl_small_info.Font = new System.Drawing.Font("Lucida Sans Unicode", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_small_info.ForeColor = System.Drawing.Color.White;
            this.lbl_small_info.Location = new System.Drawing.Point(3, 368);
            this.lbl_small_info.Name = "lbl_small_info";
            this.lbl_small_info.Size = new System.Drawing.Size(1186, 30);
            this.lbl_small_info.TabIndex = 2;
            this.lbl_small_info.Text = "REVOLUTIONARY SOLUTION FOR IOT AND BIG DATA";
            this.lbl_small_info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_description_bold
            // 
            this.lbl_description_bold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_description_bold.BackColor = System.Drawing.Color.Transparent;
            this.lbl_description_bold.Font = new System.Drawing.Font("Lucida Sans Unicode", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_description_bold.ForeColor = System.Drawing.Color.White;
            this.lbl_description_bold.Location = new System.Drawing.Point(3, 313);
            this.lbl_description_bold.Name = "lbl_description_bold";
            this.lbl_description_bold.Size = new System.Drawing.Size(1186, 55);
            this.lbl_description_bold.TabIndex = 1;
            this.lbl_description_bold.Text = "WELLCOME TO QURAS COIN WALLET";
            this.lbl_description_bold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_quras_title
            // 
            this.lbl_quras_title.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_quras_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_quras_title.Font = new System.Drawing.Font("Lucida Sans Unicode", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_quras_title.ForeColor = System.Drawing.Color.White;
            this.lbl_quras_title.Location = new System.Drawing.Point(338, 127);
            this.lbl_quras_title.Name = "lbl_quras_title";
            this.lbl_quras_title.Size = new System.Drawing.Size(521, 65);
            this.lbl_quras_title.TabIndex = 0;
            this.lbl_quras_title.Text = "QUARS";
            this.lbl_quras_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1192, 733);
            this.Controls.Add(this.pan_background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quras Welcome";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.pan_background.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_background;
        private System.Windows.Forms.Label lbl_quras_title;
        private System.Windows.Forms.Label lbl_small_info;
        private System.Windows.Forms.Label lbl_description_bold;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button bunifuFlatButton1;
        private MaterialSkin.Controls.MaterialRaisedButton btn_restore;
        private MaterialSkin.Controls.MaterialRaisedButton btn_new_wallet;
    }
}