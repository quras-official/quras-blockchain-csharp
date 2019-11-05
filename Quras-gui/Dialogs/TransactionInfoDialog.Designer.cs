namespace Quras_gui.Dialogs
{
    partial class TransactionInfoDialog
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
            this.lbl_comment = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_blocknumber = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_timestamp = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_txhash = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_txtype = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_from = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_to = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_txversion = new MaterialSkin.Controls.MaterialLabel();
            this.lbl_fee = new MaterialSkin.Controls.MaterialLabel();
            this.txb_fee = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.txb_txversion = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.txb_to = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.txb_from = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.txb_txtype = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.txb_txhash = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.txb_time = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.txb_blocknumber = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.txb_amount = new Quras_gui.Controls.CustomizeControl.TextBox.TransparentTextBox();
            this.lbl_amount = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // lbl_comment
            // 
            this.lbl_comment.AutoSize = true;
            this.lbl_comment.Depth = 0;
            this.lbl_comment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbl_comment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_comment.Location = new System.Drawing.Point(12, 75);
            this.lbl_comment.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_comment.Name = "lbl_comment";
            this.lbl_comment.Size = new System.Drawing.Size(309, 17);
            this.lbl_comment.TabIndex = 0;
            this.lbl_comment.Text = "Bellow, you can see the transaction information.";
            // 
            // lbl_blocknumber
            // 
            this.lbl_blocknumber.Depth = 0;
            this.lbl_blocknumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_blocknumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_blocknumber.Location = new System.Drawing.Point(13, 117);
            this.lbl_blocknumber.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_blocknumber.Name = "lbl_blocknumber";
            this.lbl_blocknumber.Size = new System.Drawing.Size(185, 26);
            this.lbl_blocknumber.TabIndex = 1;
            this.lbl_blocknumber.Text = "Block Number ";
            this.lbl_blocknumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_timestamp
            // 
            this.lbl_timestamp.Depth = 0;
            this.lbl_timestamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_timestamp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_timestamp.Location = new System.Drawing.Point(13, 164);
            this.lbl_timestamp.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_timestamp.Name = "lbl_timestamp";
            this.lbl_timestamp.Size = new System.Drawing.Size(185, 24);
            this.lbl_timestamp.TabIndex = 2;
            this.lbl_timestamp.Text = "Time ";
            this.lbl_timestamp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_txhash
            // 
            this.lbl_txhash.Depth = 0;
            this.lbl_txhash.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_txhash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_txhash.Location = new System.Drawing.Point(13, 208);
            this.lbl_txhash.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_txhash.Name = "lbl_txhash";
            this.lbl_txhash.Size = new System.Drawing.Size(185, 24);
            this.lbl_txhash.TabIndex = 3;
            this.lbl_txhash.Text = "Tx Hash ";
            this.lbl_txhash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_txtype
            // 
            this.lbl_txtype.Depth = 0;
            this.lbl_txtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_txtype.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_txtype.Location = new System.Drawing.Point(13, 252);
            this.lbl_txtype.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_txtype.Name = "lbl_txtype";
            this.lbl_txtype.Size = new System.Drawing.Size(185, 24);
            this.lbl_txtype.TabIndex = 4;
            this.lbl_txtype.Text = "Tx Type ";
            this.lbl_txtype.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_from
            // 
            this.lbl_from.Depth = 0;
            this.lbl_from.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_from.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_from.Location = new System.Drawing.Point(13, 297);
            this.lbl_from.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_from.Name = "lbl_from";
            this.lbl_from.Size = new System.Drawing.Size(185, 24);
            this.lbl_from.TabIndex = 5;
            this.lbl_from.Text = "From ";
            this.lbl_from.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_to
            // 
            this.lbl_to.Depth = 0;
            this.lbl_to.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_to.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_to.Location = new System.Drawing.Point(13, 343);
            this.lbl_to.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_to.Name = "lbl_to";
            this.lbl_to.Size = new System.Drawing.Size(185, 24);
            this.lbl_to.TabIndex = 6;
            this.lbl_to.Text = "To ";
            this.lbl_to.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_txversion
            // 
            this.lbl_txversion.Depth = 0;
            this.lbl_txversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_txversion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_txversion.Location = new System.Drawing.Point(13, 430);
            this.lbl_txversion.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_txversion.Name = "lbl_txversion";
            this.lbl_txversion.Size = new System.Drawing.Size(185, 24);
            this.lbl_txversion.TabIndex = 7;
            this.lbl_txversion.Text = "Tx Version ";
            this.lbl_txversion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_fee
            // 
            this.lbl_fee.Depth = 0;
            this.lbl_fee.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbl_fee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_fee.Location = new System.Drawing.Point(13, 473);
            this.lbl_fee.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_fee.Name = "lbl_fee";
            this.lbl_fee.Size = new System.Drawing.Size(185, 24);
            this.lbl_fee.TabIndex = 8;
            this.lbl_fee.Text = "Fee ";
            this.lbl_fee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txb_fee
            // 
            this.txb_fee.BackColor = System.Drawing.Color.Transparent;
            this.txb_fee.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_fee.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_fee.Location = new System.Drawing.Point(204, 474);
            this.txb_fee.Name = "txb_fee";
            this.txb_fee.ReadOnly = true;
            this.txb_fee.Size = new System.Drawing.Size(415, 21);
            this.txb_fee.TabIndex = 18;
            // 
            // txb_txversion
            // 
            this.txb_txversion.BackColor = System.Drawing.Color.Transparent;
            this.txb_txversion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_txversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_txversion.Location = new System.Drawing.Point(204, 431);
            this.txb_txversion.Name = "txb_txversion";
            this.txb_txversion.ReadOnly = true;
            this.txb_txversion.Size = new System.Drawing.Size(415, 21);
            this.txb_txversion.TabIndex = 17;
            // 
            // txb_to
            // 
            this.txb_to.BackColor = System.Drawing.Color.Transparent;
            this.txb_to.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_to.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_to.Location = new System.Drawing.Point(204, 344);
            this.txb_to.Name = "txb_to";
            this.txb_to.ReadOnly = true;
            this.txb_to.Size = new System.Drawing.Size(415, 21);
            this.txb_to.TabIndex = 16;
            // 
            // txb_from
            // 
            this.txb_from.BackColor = System.Drawing.Color.Transparent;
            this.txb_from.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_from.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_from.Location = new System.Drawing.Point(204, 298);
            this.txb_from.Name = "txb_from";
            this.txb_from.ReadOnly = true;
            this.txb_from.Size = new System.Drawing.Size(415, 21);
            this.txb_from.TabIndex = 15;
            // 
            // txb_txtype
            // 
            this.txb_txtype.BackColor = System.Drawing.Color.Transparent;
            this.txb_txtype.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_txtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_txtype.Location = new System.Drawing.Point(204, 253);
            this.txb_txtype.Name = "txb_txtype";
            this.txb_txtype.ReadOnly = true;
            this.txb_txtype.Size = new System.Drawing.Size(415, 21);
            this.txb_txtype.TabIndex = 14;
            // 
            // txb_txhash
            // 
            this.txb_txhash.BackColor = System.Drawing.Color.Transparent;
            this.txb_txhash.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_txhash.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_txhash.Location = new System.Drawing.Point(204, 209);
            this.txb_txhash.Name = "txb_txhash";
            this.txb_txhash.ReadOnly = true;
            this.txb_txhash.Size = new System.Drawing.Size(415, 21);
            this.txb_txhash.TabIndex = 13;
            // 
            // txb_time
            // 
            this.txb_time.BackColor = System.Drawing.Color.Transparent;
            this.txb_time.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_time.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_time.Location = new System.Drawing.Point(204, 165);
            this.txb_time.Name = "txb_time";
            this.txb_time.ReadOnly = true;
            this.txb_time.Size = new System.Drawing.Size(415, 21);
            this.txb_time.TabIndex = 12;
            // 
            // txb_blocknumber
            // 
            this.txb_blocknumber.BackColor = System.Drawing.Color.Transparent;
            this.txb_blocknumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_blocknumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_blocknumber.Location = new System.Drawing.Point(204, 119);
            this.txb_blocknumber.Name = "txb_blocknumber";
            this.txb_blocknumber.ReadOnly = true;
            this.txb_blocknumber.Size = new System.Drawing.Size(415, 21);
            this.txb_blocknumber.TabIndex = 11;
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(196, 117);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(2, 381);
            this.materialDivider1.TabIndex = 19;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // txb_amount
            // 
            this.txb_amount.BackColor = System.Drawing.Color.Transparent;
            this.txb_amount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txb_amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_amount.Location = new System.Drawing.Point(204, 388);
            this.txb_amount.Name = "txb_amount";
            this.txb_amount.ReadOnly = true;
            this.txb_amount.Size = new System.Drawing.Size(415, 21);
            this.txb_amount.TabIndex = 21;
            // 
            // lbl_amount
            // 
            this.lbl_amount.Depth = 0;
            this.lbl_amount.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_amount.Location = new System.Drawing.Point(11, 387);
            this.lbl_amount.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_amount.Name = "lbl_amount";
            this.lbl_amount.Size = new System.Drawing.Size(185, 24);
            this.lbl_amount.TabIndex = 20;
            this.lbl_amount.Text = "Amount";
            this.lbl_amount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TransactionInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(631, 529);
            this.Controls.Add(this.txb_amount);
            this.Controls.Add(this.lbl_amount);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.txb_fee);
            this.Controls.Add(this.txb_txversion);
            this.Controls.Add(this.txb_to);
            this.Controls.Add(this.txb_from);
            this.Controls.Add(this.txb_txtype);
            this.Controls.Add(this.txb_txhash);
            this.Controls.Add(this.txb_time);
            this.Controls.Add(this.txb_blocknumber);
            this.Controls.Add(this.lbl_fee);
            this.Controls.Add(this.lbl_txversion);
            this.Controls.Add(this.lbl_to);
            this.Controls.Add(this.lbl_from);
            this.Controls.Add(this.lbl_txtype);
            this.Controls.Add(this.lbl_txhash);
            this.Controls.Add(this.lbl_timestamp);
            this.Controls.Add(this.lbl_blocknumber);
            this.Controls.Add(this.lbl_comment);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionInfoDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel lbl_comment;
        private MaterialSkin.Controls.MaterialLabel lbl_blocknumber;
        private MaterialSkin.Controls.MaterialLabel lbl_timestamp;
        private MaterialSkin.Controls.MaterialLabel lbl_txhash;
        private MaterialSkin.Controls.MaterialLabel lbl_txtype;
        private MaterialSkin.Controls.MaterialLabel lbl_from;
        private MaterialSkin.Controls.MaterialLabel lbl_to;
        private MaterialSkin.Controls.MaterialLabel lbl_txversion;
        private MaterialSkin.Controls.MaterialLabel lbl_fee;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_blocknumber;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_time;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_txhash;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_txtype;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_from;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_to;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_txversion;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_fee;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private Controls.CustomizeControl.TextBox.TransparentTextBox txb_amount;
        private MaterialSkin.Controls.MaterialLabel lbl_amount;
    }
}