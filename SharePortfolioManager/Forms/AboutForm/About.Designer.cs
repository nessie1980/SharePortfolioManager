namespace SharePortfolioManager
{
    partial class FrmAbout
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
            this.btnOk = new System.Windows.Forms.Button();
            this.lblApplicationVersion = new System.Windows.Forms.Label();
            this.lblWebParserDllVersion = new System.Windows.Forms.Label();
            this.lblApplicationVersionValue = new System.Windows.Forms.Label();
            this.lblWebParserDllVersionValue = new System.Windows.Forms.Label();
            this.grpBoxVersions = new System.Windows.Forms.GroupBox();
            this.tblLayPnlButton = new System.Windows.Forms.TableLayoutPanel();
            this.tblLayPnlVersions = new System.Windows.Forms.TableLayoutPanel();
            this.lblLoggerDllVersionValue = new System.Windows.Forms.Label();
            this.lblLoggerDllVersion = new System.Windows.Forms.Label();
            this.lblLanguageDllVersionValue = new System.Windows.Forms.Label();
            this.lblLanguageDllVersion = new System.Windows.Forms.Label();
            this.grpBoxVersions.SuspendLayout();
            this.tblLayPnlButton.SuspendLayout();
            this.tblLayPnlVersions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(137, 1);
            this.btnOk.Margin = new System.Windows.Forms.Padding(1);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(178, 31);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "_Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnBtnOk_Click);
            // 
            // lblApplicationVersion
            // 
            this.lblApplicationVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblApplicationVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblApplicationVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationVersion.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationVersion.Location = new System.Drawing.Point(1, 1);
            this.lblApplicationVersion.Margin = new System.Windows.Forms.Padding(1);
            this.lblApplicationVersion.Name = "lblApplicationVersion";
            this.lblApplicationVersion.Size = new System.Drawing.Size(203, 22);
            this.lblApplicationVersion.TabIndex = 2;
            this.lblApplicationVersion.Text = "_Application version:";
            this.lblApplicationVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWebParserDllVersion
            // 
            this.lblWebParserDllVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblWebParserDllVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWebParserDllVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebParserDllVersion.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebParserDllVersion.Location = new System.Drawing.Point(1, 25);
            this.lblWebParserDllVersion.Margin = new System.Windows.Forms.Padding(1);
            this.lblWebParserDllVersion.Name = "lblWebParserDllVersion";
            this.lblWebParserDllVersion.Size = new System.Drawing.Size(203, 22);
            this.lblWebParserDllVersion.TabIndex = 3;
            this.lblWebParserDllVersion.Text = "_Webparser dll:";
            this.lblWebParserDllVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblApplicationVersionValue
            // 
            this.lblApplicationVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblApplicationVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblApplicationVersionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationVersionValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationVersionValue.Location = new System.Drawing.Point(206, 1);
            this.lblApplicationVersionValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblApplicationVersionValue.Name = "lblApplicationVersionValue";
            this.lblApplicationVersionValue.Size = new System.Drawing.Size(109, 22);
            this.lblApplicationVersionValue.TabIndex = 4;
            this.lblApplicationVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWebParserDllVersionValue
            // 
            this.lblWebParserDllVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblWebParserDllVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWebParserDllVersionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebParserDllVersionValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebParserDllVersionValue.Location = new System.Drawing.Point(206, 25);
            this.lblWebParserDllVersionValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblWebParserDllVersionValue.Name = "lblWebParserDllVersionValue";
            this.lblWebParserDllVersionValue.Size = new System.Drawing.Size(109, 22);
            this.lblWebParserDllVersionValue.TabIndex = 5;
            this.lblWebParserDllVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpBoxVersions
            // 
            this.grpBoxVersions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxVersions.Controls.Add(this.tblLayPnlButton);
            this.grpBoxVersions.Controls.Add(this.tblLayPnlVersions);
            this.grpBoxVersions.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxVersions.Location = new System.Drawing.Point(5, 5);
            this.grpBoxVersions.Name = "grpBoxVersions";
            this.grpBoxVersions.Size = new System.Drawing.Size(322, 149);
            this.grpBoxVersions.TabIndex = 6;
            this.grpBoxVersions.TabStop = false;
            this.grpBoxVersions.Text = "_grpBoxVersions";
            // 
            // tblLayPnlButton
            // 
            this.tblLayPnlButton.ColumnCount = 2;
            this.tblLayPnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlButton.Controls.Add(this.btnOk, 1, 0);
            this.tblLayPnlButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlButton.Location = new System.Drawing.Point(3, 114);
            this.tblLayPnlButton.Name = "tblLayPnlButton";
            this.tblLayPnlButton.RowCount = 1;
            this.tblLayPnlButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlButton.Size = new System.Drawing.Size(316, 33);
            this.tblLayPnlButton.TabIndex = 1;
            // 
            // tblLayPnlVersions
            // 
            this.tblLayPnlVersions.ColumnCount = 2;
            this.tblLayPnlVersions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tblLayPnlVersions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tblLayPnlVersions.Controls.Add(this.lblLoggerDllVersionValue, 1, 3);
            this.tblLayPnlVersions.Controls.Add(this.lblApplicationVersion, 0, 0);
            this.tblLayPnlVersions.Controls.Add(this.lblApplicationVersionValue, 1, 0);
            this.tblLayPnlVersions.Controls.Add(this.lblLoggerDllVersion, 0, 3);
            this.tblLayPnlVersions.Controls.Add(this.lblWebParserDllVersion, 0, 1);
            this.tblLayPnlVersions.Controls.Add(this.lblWebParserDllVersionValue, 1, 1);
            this.tblLayPnlVersions.Controls.Add(this.lblLanguageDllVersionValue, 1, 2);
            this.tblLayPnlVersions.Controls.Add(this.lblLanguageDllVersion, 0, 2);
            this.tblLayPnlVersions.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlVersions.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlVersions.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlVersions.Name = "tblLayPnlVersions";
            this.tblLayPnlVersions.RowCount = 4;
            this.tblLayPnlVersions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlVersions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlVersions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlVersions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlVersions.Size = new System.Drawing.Size(316, 96);
            this.tblLayPnlVersions.TabIndex = 0;
            // 
            // lblLoggerDllVersionValue
            // 
            this.lblLoggerDllVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblLoggerDllVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLoggerDllVersionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoggerDllVersionValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggerDllVersionValue.Location = new System.Drawing.Point(206, 73);
            this.lblLoggerDllVersionValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblLoggerDllVersionValue.Name = "lblLoggerDllVersionValue";
            this.lblLoggerDllVersionValue.Size = new System.Drawing.Size(109, 22);
            this.lblLoggerDllVersionValue.TabIndex = 9;
            this.lblLoggerDllVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoggerDllVersion
            // 
            this.lblLoggerDllVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblLoggerDllVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLoggerDllVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoggerDllVersion.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggerDllVersion.Location = new System.Drawing.Point(1, 73);
            this.lblLoggerDllVersion.Margin = new System.Windows.Forms.Padding(1);
            this.lblLoggerDllVersion.Name = "lblLoggerDllVersion";
            this.lblLoggerDllVersion.Size = new System.Drawing.Size(203, 22);
            this.lblLoggerDllVersion.TabIndex = 8;
            this.lblLoggerDllVersion.Text = "_Logger dll:";
            this.lblLoggerDllVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLanguageDllVersionValue
            // 
            this.lblLanguageDllVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblLanguageDllVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLanguageDllVersionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLanguageDllVersionValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLanguageDllVersionValue.Location = new System.Drawing.Point(206, 49);
            this.lblLanguageDllVersionValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblLanguageDllVersionValue.Name = "lblLanguageDllVersionValue";
            this.lblLanguageDllVersionValue.Size = new System.Drawing.Size(109, 22);
            this.lblLanguageDllVersionValue.TabIndex = 7;
            this.lblLanguageDllVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLanguageDllVersion
            // 
            this.lblLanguageDllVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblLanguageDllVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLanguageDllVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLanguageDllVersion.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLanguageDllVersion.Location = new System.Drawing.Point(1, 49);
            this.lblLanguageDllVersion.Margin = new System.Windows.Forms.Padding(1);
            this.lblLanguageDllVersion.Name = "lblLanguageDllVersion";
            this.lblLanguageDllVersion.Size = new System.Drawing.Size(203, 22);
            this.lblLanguageDllVersion.TabIndex = 6;
            this.lblLanguageDllVersion.Text = "_Language dll:";
            this.lblLanguageDllVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(330, 157);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxVersions);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAbout_FormClosing);
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            this.grpBoxVersions.ResumeLayout(false);
            this.tblLayPnlButton.ResumeLayout(false);
            this.tblLayPnlVersions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblApplicationVersion;
        private System.Windows.Forms.Label lblWebParserDllVersion;
        private System.Windows.Forms.Label lblApplicationVersionValue;
        private System.Windows.Forms.Label lblWebParserDllVersionValue;
        private System.Windows.Forms.GroupBox grpBoxVersions;
        private System.Windows.Forms.Label lblLanguageDllVersion;
        private System.Windows.Forms.Label lblLanguageDllVersionValue;
        private System.Windows.Forms.Label lblLoggerDllVersionValue;
        private System.Windows.Forms.Label lblLoggerDllVersion;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlButton;
        public System.Windows.Forms.TableLayoutPanel tblLayPnlVersions;
    }
}