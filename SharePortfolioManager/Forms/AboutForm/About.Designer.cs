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
            this.lblLoggerDllVersionValue = new System.Windows.Forms.Label();
            this.lblLoggerDllVersion = new System.Windows.Forms.Label();
            this.lblLanguageDllVersionValue = new System.Windows.Forms.Label();
            this.lblLanguageDllVersion = new System.Windows.Forms.Label();
            this.grpBoxVersions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(115, 149);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 31);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "_Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnBtnOk_Click);
            // 
            // lblApplicationVersion
            // 
            this.lblApplicationVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblApplicationVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblApplicationVersion.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationVersion.Location = new System.Drawing.Point(10, 22);
            this.lblApplicationVersion.Name = "lblApplicationVersion";
            this.lblApplicationVersion.Size = new System.Drawing.Size(203, 23);
            this.lblApplicationVersion.TabIndex = 2;
            this.lblApplicationVersion.Text = "_Application version:";
            this.lblApplicationVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWebParserDllVersion
            // 
            this.lblWebParserDllVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblWebParserDllVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWebParserDllVersion.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebParserDllVersion.Location = new System.Drawing.Point(10, 49);
            this.lblWebParserDllVersion.Name = "lblWebParserDllVersion";
            this.lblWebParserDllVersion.Size = new System.Drawing.Size(203, 23);
            this.lblWebParserDllVersion.TabIndex = 3;
            this.lblWebParserDllVersion.Text = "_Webparser dll:";
            this.lblWebParserDllVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblApplicationVersionValue
            // 
            this.lblApplicationVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblApplicationVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblApplicationVersionValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationVersionValue.Location = new System.Drawing.Point(217, 22);
            this.lblApplicationVersionValue.Name = "lblApplicationVersionValue";
            this.lblApplicationVersionValue.Size = new System.Drawing.Size(79, 23);
            this.lblApplicationVersionValue.TabIndex = 4;
            this.lblApplicationVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWebParserDllVersionValue
            // 
            this.lblWebParserDllVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblWebParserDllVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWebParserDllVersionValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebParserDllVersionValue.Location = new System.Drawing.Point(217, 49);
            this.lblWebParserDllVersionValue.Name = "lblWebParserDllVersionValue";
            this.lblWebParserDllVersionValue.Size = new System.Drawing.Size(79, 23);
            this.lblWebParserDllVersionValue.TabIndex = 5;
            this.lblWebParserDllVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpBoxVersions
            // 
            this.grpBoxVersions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxVersions.Controls.Add(this.lblLoggerDllVersionValue);
            this.grpBoxVersions.Controls.Add(this.lblLoggerDllVersion);
            this.grpBoxVersions.Controls.Add(this.lblLanguageDllVersionValue);
            this.grpBoxVersions.Controls.Add(this.lblLanguageDllVersion);
            this.grpBoxVersions.Controls.Add(this.lblApplicationVersion);
            this.grpBoxVersions.Controls.Add(this.lblWebParserDllVersionValue);
            this.grpBoxVersions.Controls.Add(this.lblApplicationVersionValue);
            this.grpBoxVersions.Controls.Add(this.lblWebParserDllVersion);
            this.grpBoxVersions.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxVersions.Location = new System.Drawing.Point(12, 8);
            this.grpBoxVersions.Name = "grpBoxVersions";
            this.grpBoxVersions.Size = new System.Drawing.Size(306, 135);
            this.grpBoxVersions.TabIndex = 6;
            this.grpBoxVersions.TabStop = false;
            this.grpBoxVersions.Text = "_grpBoxVersions";
            // 
            // lblLoggerDllVersionValue
            // 
            this.lblLoggerDllVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblLoggerDllVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLoggerDllVersionValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggerDllVersionValue.Location = new System.Drawing.Point(217, 103);
            this.lblLoggerDllVersionValue.Name = "lblLoggerDllVersionValue";
            this.lblLoggerDllVersionValue.Size = new System.Drawing.Size(79, 23);
            this.lblLoggerDllVersionValue.TabIndex = 9;
            this.lblLoggerDllVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoggerDllVersion
            // 
            this.lblLoggerDllVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblLoggerDllVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLoggerDllVersion.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggerDllVersion.Location = new System.Drawing.Point(10, 103);
            this.lblLoggerDllVersion.Name = "lblLoggerDllVersion";
            this.lblLoggerDllVersion.Size = new System.Drawing.Size(203, 23);
            this.lblLoggerDllVersion.TabIndex = 8;
            this.lblLoggerDllVersion.Text = "_Logger dll:";
            this.lblLoggerDllVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLanguageDllVersionValue
            // 
            this.lblLanguageDllVersionValue.BackColor = System.Drawing.Color.LightGray;
            this.lblLanguageDllVersionValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLanguageDllVersionValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLanguageDllVersionValue.Location = new System.Drawing.Point(217, 76);
            this.lblLanguageDllVersionValue.Name = "lblLanguageDllVersionValue";
            this.lblLanguageDllVersionValue.Size = new System.Drawing.Size(79, 23);
            this.lblLanguageDllVersionValue.TabIndex = 7;
            this.lblLanguageDllVersionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLanguageDllVersion
            // 
            this.lblLanguageDllVersion.BackColor = System.Drawing.Color.LightGray;
            this.lblLanguageDllVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLanguageDllVersion.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLanguageDllVersion.Location = new System.Drawing.Point(10, 76);
            this.lblLanguageDllVersion.Name = "lblLanguageDllVersion";
            this.lblLanguageDllVersion.Size = new System.Drawing.Size(203, 23);
            this.lblLanguageDllVersion.TabIndex = 6;
            this.lblLanguageDllVersion.Text = "_Language dll:";
            this.lblLanguageDllVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(330, 185);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxVersions);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
    }
}