
using SharePortfolioManager.Properties;

namespace SharePortfolioManager.SoundSettingsForm
{
    partial class FrmSoundSettings
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
            this.btnSave = new System.Windows.Forms.Button();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxLogLevel = new System.Windows.Forms.GroupBox();
            this.chkBoxFatalError = new System.Windows.Forms.CheckBox();
            this.chkBoxError = new System.Windows.Forms.CheckBox();
            this.chkBoxWarning = new System.Windows.Forms.CheckBox();
            this.chkBoxInfo = new System.Windows.Forms.CheckBox();
            this.chkBoxStart = new System.Windows.Forms.CheckBox();
            this.grpBoxLogColors = new System.Windows.Forms.GroupBox();
            this.lblColorFatalError = new System.Windows.Forms.Label();
            this.lblColorError = new System.Windows.Forms.Label();
            this.lblColorWarning = new System.Windows.Forms.Label();
            this.lblColorInfo = new System.Windows.Forms.Label();
            this.lblColorStart = new System.Windows.Forms.Label();
            this.grpBoxComponents = new System.Windows.Forms.GroupBox();
            this.chkBoxLanguageHander = new System.Windows.Forms.CheckBox();
            this.chkBoxParser = new System.Windows.Forms.CheckBox();
            this.chkBoxApplication = new System.Windows.Forms.CheckBox();
            this.grpBoxEnableFileLogging = new System.Windows.Forms.GroupBox();
            this.chkBoxEnableFileLogging = new System.Windows.Forms.CheckBox();
            this.grpBoxGUIEntries = new System.Windows.Forms.GroupBox();
            this.cbxGUIEntriesList = new System.Windows.Forms.ComboBox();
            this.lblGUIEntriesSize = new System.Windows.Forms.Label();
            this.grpBoxStoredLogFiles = new System.Windows.Forms.GroupBox();
            this.btnLogFileCleanUp = new System.Windows.Forms.Button();
            this.cbxStoredLogFiles = new System.Windows.Forms.ComboBox();
            this.lblStoredLogFiles = new System.Windows.Forms.Label();
            this.grpBoxCleanUpAtStartUp = new System.Windows.Forms.GroupBox();
            this.chkBoxEnableCleanUpAtStartUp = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.grpBoxLogLevel.SuspendLayout();
            this.grpBoxLogColors.SuspendLayout();
            this.grpBoxComponents.SuspendLayout();
            this.grpBoxEnableFileLogging.SuspendLayout();
            this.grpBoxGUIEntries.SuspendLayout();
            this.grpBoxStoredLogFiles.SuspendLayout();
            this.grpBoxCleanUpAtStartUp.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::SharePortfolioManager.Properties.Resources.button_cancel_24;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(559, 297);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(166, 31);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Image = global::SharePortfolioManager.Properties.Resources.button_save_24;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(387, 297);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(166, 31);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "_Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnBtnSave_Click);
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // grpBoxLogLevel
            // 
            this.grpBoxLogLevel.Controls.Add(this.chkBoxFatalError);
            this.grpBoxLogLevel.Controls.Add(this.chkBoxError);
            this.grpBoxLogLevel.Controls.Add(this.chkBoxWarning);
            this.grpBoxLogLevel.Controls.Add(this.chkBoxInfo);
            this.grpBoxLogLevel.Controls.Add(this.chkBoxStart);
            this.grpBoxLogLevel.Location = new System.Drawing.Point(214, 125);
            this.grpBoxLogLevel.Name = "grpBoxLogLevel";
            this.grpBoxLogLevel.Size = new System.Drawing.Size(172, 168);
            this.grpBoxLogLevel.TabIndex = 3;
            this.grpBoxLogLevel.TabStop = false;
            this.grpBoxLogLevel.Text = "_grpBoxLogLevel";
            // 
            // chkBoxFatalError
            // 
            this.chkBoxFatalError.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.chkBoxFatalError.Location = new System.Drawing.Point(10, 139);
            this.chkBoxFatalError.Name = "chkBoxFatalError";
            this.chkBoxFatalError.Size = new System.Drawing.Size(147, 19);
            this.chkBoxFatalError.TabIndex = 4;
            this.chkBoxFatalError.Text = "_chkBoxFatalError";
            this.chkBoxFatalError.UseVisualStyleBackColor = false;
            // 
            // chkBoxError
            // 
            this.chkBoxError.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.chkBoxError.Location = new System.Drawing.Point(10, 109);
            this.chkBoxError.Name = "chkBoxError";
            this.chkBoxError.Size = new System.Drawing.Size(147, 19);
            this.chkBoxError.TabIndex = 3;
            this.chkBoxError.Text = "_chkBoxError";
            this.chkBoxError.UseVisualStyleBackColor = false;
            // 
            // chkBoxWarning
            // 
            this.chkBoxWarning.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.chkBoxWarning.Location = new System.Drawing.Point(10, 79);
            this.chkBoxWarning.Name = "chkBoxWarning";
            this.chkBoxWarning.Size = new System.Drawing.Size(147, 19);
            this.chkBoxWarning.TabIndex = 2;
            this.chkBoxWarning.Text = "_chkBoxWarning";
            this.chkBoxWarning.UseVisualStyleBackColor = false;
            // 
            // chkBoxInfo
            // 
            this.chkBoxInfo.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.chkBoxInfo.Location = new System.Drawing.Point(10, 49);
            this.chkBoxInfo.Name = "chkBoxInfo";
            this.chkBoxInfo.Size = new System.Drawing.Size(147, 19);
            this.chkBoxInfo.TabIndex = 1;
            this.chkBoxInfo.Text = "_chkBoxInfo";
            this.chkBoxInfo.UseVisualStyleBackColor = false;
            // 
            // chkBoxStart
            // 
            this.chkBoxStart.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.chkBoxStart.Location = new System.Drawing.Point(10, 22);
            this.chkBoxStart.Name = "chkBoxStart";
            this.chkBoxStart.Size = new System.Drawing.Size(147, 19);
            this.chkBoxStart.TabIndex = 0;
            this.chkBoxStart.Text = "_chkBoxStart";
            this.chkBoxStart.UseVisualStyleBackColor = false;
            // 
            // grpBoxLogColors
            // 
            this.grpBoxLogColors.Controls.Add(this.lblColorFatalError);
            this.grpBoxLogColors.Controls.Add(this.lblColorError);
            this.grpBoxLogColors.Controls.Add(this.lblColorWarning);
            this.grpBoxLogColors.Controls.Add(this.lblColorInfo);
            this.grpBoxLogColors.Controls.Add(this.lblColorStart);
            this.grpBoxLogColors.Location = new System.Drawing.Point(392, 125);
            this.grpBoxLogColors.Name = "grpBoxLogColors";
            this.grpBoxLogColors.Size = new System.Drawing.Size(333, 168);
            this.grpBoxLogColors.TabIndex = 4;
            this.grpBoxLogColors.TabStop = false;
            this.grpBoxLogColors.Text = "_grpBoxLogColors";
            // 
            // lblColorFatalError
            // 
            this.lblColorFatalError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorFatalError.BackColor = System.Drawing.Color.LightGray;
            this.lblColorFatalError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColorFatalError.Location = new System.Drawing.Point(10, 138);
            this.lblColorFatalError.Name = "lblColorFatalError";
            this.lblColorFatalError.Size = new System.Drawing.Size(131, 23);
            this.lblColorFatalError.TabIndex = 9;
            this.lblColorFatalError.Text = "_colorFatalError";
            this.lblColorFatalError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblColorError
            // 
            this.lblColorError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorError.BackColor = System.Drawing.Color.LightGray;
            this.lblColorError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColorError.Location = new System.Drawing.Point(10, 109);
            this.lblColorError.Name = "lblColorError";
            this.lblColorError.Size = new System.Drawing.Size(131, 23);
            this.lblColorError.TabIndex = 8;
            this.lblColorError.Text = "_colorError";
            this.lblColorError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblColorWarning
            // 
            this.lblColorWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorWarning.BackColor = System.Drawing.Color.LightGray;
            this.lblColorWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColorWarning.Location = new System.Drawing.Point(10, 80);
            this.lblColorWarning.Name = "lblColorWarning";
            this.lblColorWarning.Size = new System.Drawing.Size(131, 23);
            this.lblColorWarning.TabIndex = 7;
            this.lblColorWarning.Text = "_colorWarning";
            this.lblColorWarning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblColorInfo
            // 
            this.lblColorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorInfo.BackColor = System.Drawing.Color.LightGray;
            this.lblColorInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColorInfo.Location = new System.Drawing.Point(10, 51);
            this.lblColorInfo.Name = "lblColorInfo";
            this.lblColorInfo.Size = new System.Drawing.Size(131, 23);
            this.lblColorInfo.TabIndex = 6;
            this.lblColorInfo.Text = "_colorInfo";
            this.lblColorInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblColorStart
            // 
            this.lblColorStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColorStart.BackColor = System.Drawing.Color.LightGray;
            this.lblColorStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColorStart.Location = new System.Drawing.Point(10, 22);
            this.lblColorStart.Name = "lblColorStart";
            this.lblColorStart.Size = new System.Drawing.Size(131, 23);
            this.lblColorStart.TabIndex = 5;
            this.lblColorStart.Text = "_colorStart";
            this.lblColorStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpBoxComponents
            // 
            this.grpBoxComponents.Controls.Add(this.chkBoxLanguageHander);
            this.grpBoxComponents.Controls.Add(this.chkBoxParser);
            this.grpBoxComponents.Controls.Add(this.chkBoxApplication);
            this.grpBoxComponents.Location = new System.Drawing.Point(8, 125);
            this.grpBoxComponents.Name = "grpBoxComponents";
            this.grpBoxComponents.Size = new System.Drawing.Size(200, 168);
            this.grpBoxComponents.TabIndex = 5;
            this.grpBoxComponents.TabStop = false;
            this.grpBoxComponents.Text = "_grpBoxComponents";
            // 
            // chkBoxLanguageHander
            // 
            this.chkBoxLanguageHander.Location = new System.Drawing.Point(10, 79);
            this.chkBoxLanguageHander.Name = "chkBoxLanguageHander";
            this.chkBoxLanguageHander.Size = new System.Drawing.Size(175, 19);
            this.chkBoxLanguageHander.TabIndex = 7;
            this.chkBoxLanguageHander.Text = "_chkBoxLanguageHandler";
            this.chkBoxLanguageHander.UseVisualStyleBackColor = true;
            // 
            // chkBoxParser
            // 
            this.chkBoxParser.Location = new System.Drawing.Point(10, 49);
            this.chkBoxParser.Name = "chkBoxParser";
            this.chkBoxParser.Size = new System.Drawing.Size(175, 19);
            this.chkBoxParser.TabIndex = 6;
            this.chkBoxParser.Text = "_chkBoxParser";
            this.chkBoxParser.UseVisualStyleBackColor = true;
            // 
            // chkBoxApplication
            // 
            this.chkBoxApplication.Location = new System.Drawing.Point(10, 22);
            this.chkBoxApplication.Name = "chkBoxApplication";
            this.chkBoxApplication.Size = new System.Drawing.Size(175, 19);
            this.chkBoxApplication.TabIndex = 5;
            this.chkBoxApplication.Text = "_chkBoxApplication";
            this.chkBoxApplication.UseVisualStyleBackColor = true;
            // 
            // grpBoxEnableFileLogging
            // 
            this.grpBoxEnableFileLogging.Controls.Add(this.chkBoxEnableFileLogging);
            this.grpBoxEnableFileLogging.Location = new System.Drawing.Point(512, 5);
            this.grpBoxEnableFileLogging.Name = "grpBoxEnableFileLogging";
            this.grpBoxEnableFileLogging.Size = new System.Drawing.Size(213, 54);
            this.grpBoxEnableFileLogging.TabIndex = 6;
            this.grpBoxEnableFileLogging.TabStop = false;
            this.grpBoxEnableFileLogging.Text = "_grpBoxEnableFileLogging";
            // 
            // chkBoxEnableFileLogging
            // 
            this.chkBoxEnableFileLogging.AutoSize = true;
            this.chkBoxEnableFileLogging.Location = new System.Drawing.Point(10, 25);
            this.chkBoxEnableFileLogging.Name = "chkBoxEnableFileLogging";
            this.chkBoxEnableFileLogging.Size = new System.Drawing.Size(194, 19);
            this.chkBoxEnableFileLogging.TabIndex = 0;
            this.chkBoxEnableFileLogging.Text = "_chkBoxEnableFileLogging";
            this.chkBoxEnableFileLogging.UseVisualStyleBackColor = true;
            // 
            // grpBoxGUIEntries
            // 
            this.grpBoxGUIEntries.Controls.Add(this.cbxGUIEntriesList);
            this.grpBoxGUIEntries.Controls.Add(this.lblGUIEntriesSize);
            this.grpBoxGUIEntries.Location = new System.Drawing.Point(8, 5);
            this.grpBoxGUIEntries.Name = "grpBoxGUIEntries";
            this.grpBoxGUIEntries.Size = new System.Drawing.Size(498, 54);
            this.grpBoxGUIEntries.TabIndex = 7;
            this.grpBoxGUIEntries.TabStop = false;
            this.grpBoxGUIEntries.Text = "_grpBoxGUIEntries";
            // 
            // cbxGUIEntriesList
            // 
            this.cbxGUIEntriesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGUIEntriesList.FormattingEnabled = true;
            this.cbxGUIEntriesList.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50"});
            this.cbxGUIEntriesList.Location = new System.Drawing.Point(398, 22);
            this.cbxGUIEntriesList.Name = "cbxGUIEntriesList";
            this.cbxGUIEntriesList.Size = new System.Drawing.Size(94, 23);
            this.cbxGUIEntriesList.TabIndex = 4;
            // 
            // lblGUIEntriesSize
            // 
            this.lblGUIEntriesSize.BackColor = System.Drawing.Color.LightGray;
            this.lblGUIEntriesSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGUIEntriesSize.Location = new System.Drawing.Point(10, 22);
            this.lblGUIEntriesSize.Name = "lblGUIEntriesSize";
            this.lblGUIEntriesSize.Size = new System.Drawing.Size(382, 23);
            this.lblGUIEntriesSize.TabIndex = 3;
            this.lblGUIEntriesSize.Text = "_GUIEntriesSize";
            this.lblGUIEntriesSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpBoxStoredLogFiles
            // 
            this.grpBoxStoredLogFiles.Controls.Add(this.btnLogFileCleanUp);
            this.grpBoxStoredLogFiles.Controls.Add(this.cbxStoredLogFiles);
            this.grpBoxStoredLogFiles.Controls.Add(this.lblStoredLogFiles);
            this.grpBoxStoredLogFiles.Location = new System.Drawing.Point(8, 65);
            this.grpBoxStoredLogFiles.Name = "grpBoxStoredLogFiles";
            this.grpBoxStoredLogFiles.Size = new System.Drawing.Size(498, 54);
            this.grpBoxStoredLogFiles.TabIndex = 8;
            this.grpBoxStoredLogFiles.TabStop = false;
            this.grpBoxStoredLogFiles.Text = "_grpBoxStoredLogFiles";
            // 
            // btnLogFileCleanUp
            // 
            this.btnLogFileCleanUp.Location = new System.Drawing.Point(377, 21);
            this.btnLogFileCleanUp.Name = "btnLogFileCleanUp";
            this.btnLogFileCleanUp.Size = new System.Drawing.Size(114, 25);
            this.btnLogFileCleanUp.TabIndex = 5;
            this.btnLogFileCleanUp.Text = "_cleanUp";
            this.btnLogFileCleanUp.UseVisualStyleBackColor = true;
            this.btnLogFileCleanUp.Click += new System.EventHandler(this.OnBtnLogFileCleanUp_Click);
            // 
            // cbxStoredLogFiles
            // 
            this.cbxStoredLogFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStoredLogFiles.FormattingEnabled = true;
            this.cbxStoredLogFiles.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50"});
            this.cbxStoredLogFiles.Location = new System.Drawing.Point(277, 22);
            this.cbxStoredLogFiles.Name = "cbxStoredLogFiles";
            this.cbxStoredLogFiles.Size = new System.Drawing.Size(94, 23);
            this.cbxStoredLogFiles.TabIndex = 4;
            // 
            // lblStoredLogFiles
            // 
            this.lblStoredLogFiles.BackColor = System.Drawing.Color.LightGray;
            this.lblStoredLogFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStoredLogFiles.Location = new System.Drawing.Point(10, 22);
            this.lblStoredLogFiles.Name = "lblStoredLogFiles";
            this.lblStoredLogFiles.Size = new System.Drawing.Size(261, 22);
            this.lblStoredLogFiles.TabIndex = 3;
            this.lblStoredLogFiles.Text = "_storedLogFiles";
            this.lblStoredLogFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpBoxCleanUpAtStartUp
            // 
            this.grpBoxCleanUpAtStartUp.Controls.Add(this.chkBoxEnableCleanUpAtStartUp);
            this.grpBoxCleanUpAtStartUp.Location = new System.Drawing.Point(512, 65);
            this.grpBoxCleanUpAtStartUp.Name = "grpBoxCleanUpAtStartUp";
            this.grpBoxCleanUpAtStartUp.Size = new System.Drawing.Size(213, 54);
            this.grpBoxCleanUpAtStartUp.TabIndex = 7;
            this.grpBoxCleanUpAtStartUp.TabStop = false;
            this.grpBoxCleanUpAtStartUp.Text = "_grpBoxEnableCleanUpAtStartUp";
            // 
            // chkBoxEnableCleanUpAtStartUp
            // 
            this.chkBoxEnableCleanUpAtStartUp.AutoSize = true;
            this.chkBoxEnableCleanUpAtStartUp.Location = new System.Drawing.Point(10, 26);
            this.chkBoxEnableCleanUpAtStartUp.Name = "chkBoxEnableCleanUpAtStartUp";
            this.chkBoxEnableCleanUpAtStartUp.Size = new System.Drawing.Size(229, 19);
            this.chkBoxEnableCleanUpAtStartUp.TabIndex = 0;
            this.chkBoxEnableCleanUpAtStartUp.Text = "_chkBoxEnableCleanUpAtStartUp";
            this.chkBoxEnableCleanUpAtStartUp.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 334);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(734, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip";
            // 
            // FrmSoundSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(734, 356);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grpBoxCleanUpAtStartUp);
            this.Controls.Add(this.grpBoxStoredLogFiles);
            this.Controls.Add(this.grpBoxGUIEntries);
            this.Controls.Add(this.grpBoxEnableFileLogging);
            this.Controls.Add(this.grpBoxComponents);
            this.Controls.Add(this.grpBoxLogColors);
            this.Controls.Add(this.grpBoxLogLevel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(750, 395);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(750, 395);
            this.Name = "FrmSoundSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LoggerSettings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSoundSettings_FormClosing);
            this.Load += new System.EventHandler(this.FrmSoundSettings_Load);
            this.grpBoxLogLevel.ResumeLayout(false);
            this.grpBoxLogColors.ResumeLayout(false);
            this.grpBoxComponents.ResumeLayout(false);
            this.grpBoxEnableFileLogging.ResumeLayout(false);
            this.grpBoxEnableFileLogging.PerformLayout();
            this.grpBoxGUIEntries.ResumeLayout(false);
            this.grpBoxStoredLogFiles.ResumeLayout(false);
            this.grpBoxCleanUpAtStartUp.ResumeLayout(false);
            this.grpBoxCleanUpAtStartUp.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.GroupBox grpBoxLogLevel;
        private System.Windows.Forms.CheckBox chkBoxFatalError;
        private System.Windows.Forms.CheckBox chkBoxError;
        private System.Windows.Forms.CheckBox chkBoxWarning;
        private System.Windows.Forms.CheckBox chkBoxInfo;
        private System.Windows.Forms.CheckBox chkBoxStart;
        private System.Windows.Forms.GroupBox grpBoxLogColors;
        private System.Windows.Forms.Label lblColorFatalError;
        private System.Windows.Forms.Label lblColorError;
        private System.Windows.Forms.Label lblColorWarning;
        private System.Windows.Forms.Label lblColorInfo;
        private System.Windows.Forms.Label lblColorStart;
        private System.Windows.Forms.GroupBox grpBoxComponents;
        private System.Windows.Forms.CheckBox chkBoxLanguageHander;
        private System.Windows.Forms.CheckBox chkBoxParser;
        private System.Windows.Forms.CheckBox chkBoxApplication;
        private System.Windows.Forms.GroupBox grpBoxEnableFileLogging;
        private System.Windows.Forms.CheckBox chkBoxEnableFileLogging;
        private System.Windows.Forms.GroupBox grpBoxGUIEntries;
        private System.Windows.Forms.Label lblGUIEntriesSize;
        private System.Windows.Forms.ComboBox cbxGUIEntriesList;
        private System.Windows.Forms.GroupBox grpBoxStoredLogFiles;
        private System.Windows.Forms.ComboBox cbxStoredLogFiles;
        private System.Windows.Forms.Label lblStoredLogFiles;
        private System.Windows.Forms.Button btnLogFileCleanUp;
        private System.Windows.Forms.GroupBox grpBoxCleanUpAtStartUp;
        private System.Windows.Forms.CheckBox chkBoxEnableCleanUpAtStartUp;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}