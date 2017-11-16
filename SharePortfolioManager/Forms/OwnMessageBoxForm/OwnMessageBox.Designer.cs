namespace SharePortfolioManager
{
    partial class OwnMessageBox
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tblLayPnlOnwMessageBox = new System.Windows.Forms.TableLayoutPanel();
            this.tblLayPnlOnwMessageBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(294, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(165, 33);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(123, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(165, 33);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "_Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnBtnOk_Click);
            // 
            // tblLayPnlOnwMessageBox
            // 
            this.tblLayPnlOnwMessageBox.ColumnCount = 3;
            this.tblLayPnlOnwMessageBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOnwMessageBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlOnwMessageBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlOnwMessageBox.Controls.Add(this.btnOk, 1, 0);
            this.tblLayPnlOnwMessageBox.Controls.Add(this.btnCancel, 2, 0);
            this.tblLayPnlOnwMessageBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tblLayPnlOnwMessageBox.Location = new System.Drawing.Point(0, 170);
            this.tblLayPnlOnwMessageBox.Name = "tblLayPnlOnwMessageBox";
            this.tblLayPnlOnwMessageBox.RowCount = 1;
            this.tblLayPnlOnwMessageBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOnwMessageBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOnwMessageBox.Size = new System.Drawing.Size(462, 39);
            this.tblLayPnlOnwMessageBox.TabIndex = 3;
            // 
            // OwnMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(462, 209);
            this.Controls.Add(this.tblLayPnlOnwMessageBox);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OwnMessageBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_MessageBox";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OwnMessageBox_Load);
            this.Shown += new System.EventHandler(this.OwnMessageBox_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OwnMessageBox_Paint);
            this.tblLayPnlOnwMessageBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlOnwMessageBox;
    }
}