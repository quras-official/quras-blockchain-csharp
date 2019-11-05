namespace DT_GUI_Modules.Modules
{
    partial class DashboardControl
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
            this.pan_main = new System.Windows.Forms.Panel();
            this.pan_flow = new System.Windows.Forms.Panel();
            this.pan_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan_main
            // 
            this.pan_main.BackgroundImage = global::DT_GUI_Modules.Properties.Resources.background_1;
            this.pan_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pan_main.Controls.Add(this.pan_flow);
            this.pan_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_main.Location = new System.Drawing.Point(0, 0);
            this.pan_main.Name = "pan_main";
            this.pan_main.Size = new System.Drawing.Size(471, 627);
            this.pan_main.TabIndex = 0;
            // 
            // pan_flow
            // 
            this.pan_flow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pan_flow.BackColor = System.Drawing.Color.Transparent;
            this.pan_flow.Location = new System.Drawing.Point(4, 4);
            this.pan_flow.Name = "pan_flow";
            this.pan_flow.Size = new System.Drawing.Size(450, 620);
            this.pan_flow.TabIndex = 0;
            // 
            // DashboardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pan_main);
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(471, 627);
            this.pan_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_main;
        private System.Windows.Forms.Panel pan_flow;
    }
}
