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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSoundSettings));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkBoxUpdateFinishedSoundPlay = new System.Windows.Forms.CheckBox();
            this.grpBoxUpdateFinishedSound = new System.Windows.Forms.GroupBox();
            this.btnUpdateFinishedSound = new System.Windows.Forms.Button();
            this.lblUpdateFinishedSound = new System.Windows.Forms.Label();
            this.grpBoxErrorSound = new System.Windows.Forms.GroupBox();
            this.chkBoxErrorSoundPlay = new System.Windows.Forms.CheckBox();
            this.btnErrorSound = new System.Windows.Forms.Button();
            this.lblErrorSound = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.grpBoxUpdateFinishedSound.SuspendLayout();
            this.grpBoxErrorSound.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::SharePortfolioManager.Properties.Resources.button_cancel_24;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(559, 129);
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
            this.btnSave.Location = new System.Drawing.Point(387, 129);
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
            // chkBoxUpdateFinishedSoundPlay
            // 
            this.chkBoxUpdateFinishedSoundPlay.AutoSize = true;
            this.chkBoxUpdateFinishedSoundPlay.Location = new System.Drawing.Point(587, 25);
            this.chkBoxUpdateFinishedSoundPlay.Name = "chkBoxUpdateFinishedSoundPlay";
            this.chkBoxUpdateFinishedSoundPlay.Size = new System.Drawing.Size(124, 19);
            this.chkBoxUpdateFinishedSoundPlay.TabIndex = 0;
            this.chkBoxUpdateFinishedSoundPlay.Text = "_chkBoxUfsPlay";
            this.chkBoxUpdateFinishedSoundPlay.UseVisualStyleBackColor = true;
            // 
            // grpBoxUpdateFinishedSound
            // 
            this.grpBoxUpdateFinishedSound.Controls.Add(this.chkBoxUpdateFinishedSoundPlay);
            this.grpBoxUpdateFinishedSound.Controls.Add(this.btnUpdateFinishedSound);
            this.grpBoxUpdateFinishedSound.Controls.Add(this.lblUpdateFinishedSound);
            this.grpBoxUpdateFinishedSound.Location = new System.Drawing.Point(8, 5);
            this.grpBoxUpdateFinishedSound.Name = "grpBoxUpdateFinishedSound";
            this.grpBoxUpdateFinishedSound.Size = new System.Drawing.Size(717, 54);
            this.grpBoxUpdateFinishedSound.TabIndex = 7;
            this.grpBoxUpdateFinishedSound.TabStop = false;
            this.grpBoxUpdateFinishedSound.Text = "_grpBoxUpdateFinishedSound";
            // 
            // btnUpdateFinishedSound
            // 
            this.btnUpdateFinishedSound.Location = new System.Drawing.Point(467, 21);
            this.btnUpdateFinishedSound.Name = "btnUpdateFinishedSound";
            this.btnUpdateFinishedSound.Size = new System.Drawing.Size(114, 25);
            this.btnUpdateFinishedSound.TabIndex = 6;
            this.btnUpdateFinishedSound.Text = "_browse";
            this.btnUpdateFinishedSound.UseVisualStyleBackColor = true;
            this.btnUpdateFinishedSound.Click += new System.EventHandler(this.OnBtnUpdateFinishedSoundBrowse_Click);
            // 
            // lblUpdateFinishedSound
            // 
            this.lblUpdateFinishedSound.BackColor = System.Drawing.Color.LightGray;
            this.lblUpdateFinishedSound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUpdateFinishedSound.Location = new System.Drawing.Point(10, 22);
            this.lblUpdateFinishedSound.Name = "lblUpdateFinishedSound";
            this.lblUpdateFinishedSound.Size = new System.Drawing.Size(451, 23);
            this.lblUpdateFinishedSound.TabIndex = 3;
            this.lblUpdateFinishedSound.Text = "_UpdateFinishedSound";
            this.lblUpdateFinishedSound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpBoxErrorSound
            // 
            this.grpBoxErrorSound.Controls.Add(this.chkBoxErrorSoundPlay);
            this.grpBoxErrorSound.Controls.Add(this.btnErrorSound);
            this.grpBoxErrorSound.Controls.Add(this.lblErrorSound);
            this.grpBoxErrorSound.Location = new System.Drawing.Point(8, 65);
            this.grpBoxErrorSound.Name = "grpBoxErrorSound";
            this.grpBoxErrorSound.Size = new System.Drawing.Size(717, 54);
            this.grpBoxErrorSound.TabIndex = 8;
            this.grpBoxErrorSound.TabStop = false;
            this.grpBoxErrorSound.Text = "_grpBoxErrorSound";
            // 
            // chkBoxErrorSoundPlay
            // 
            this.chkBoxErrorSoundPlay.AutoSize = true;
            this.chkBoxErrorSoundPlay.Location = new System.Drawing.Point(587, 25);
            this.chkBoxErrorSoundPlay.Name = "chkBoxErrorSoundPlay";
            this.chkBoxErrorSoundPlay.Size = new System.Drawing.Size(117, 19);
            this.chkBoxErrorSoundPlay.TabIndex = 0;
            this.chkBoxErrorSoundPlay.Text = "_chkBoxEsPlay";
            this.chkBoxErrorSoundPlay.UseVisualStyleBackColor = true;
            // 
            // btnErrorSound
            // 
            this.btnErrorSound.Location = new System.Drawing.Point(467, 21);
            this.btnErrorSound.Name = "btnErrorSound";
            this.btnErrorSound.Size = new System.Drawing.Size(114, 25);
            this.btnErrorSound.TabIndex = 5;
            this.btnErrorSound.Text = "_browse";
            this.btnErrorSound.UseVisualStyleBackColor = true;
            this.btnErrorSound.Click += new System.EventHandler(this.OnBtnErrorSoundBrowse_Click);
            // 
            // lblErrorSound
            // 
            this.lblErrorSound.BackColor = System.Drawing.Color.LightGray;
            this.lblErrorSound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblErrorSound.Location = new System.Drawing.Point(10, 22);
            this.lblErrorSound.Name = "lblErrorSound";
            this.lblErrorSound.Size = new System.Drawing.Size(451, 22);
            this.lblErrorSound.TabIndex = 3;
            this.lblErrorSound.Text = "_ErrorSound";
            this.lblErrorSound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 166);
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
            this.ClientSize = new System.Drawing.Size(734, 188);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grpBoxErrorSound);
            this.Controls.Add(this.grpBoxUpdateFinishedSound);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(750, 227);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(750, 227);
            this.Name = "FrmSoundSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SoundSettings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSoundSettings_FormClosing);
            this.Load += new System.EventHandler(this.FrmSoundSettings_Load);
            this.grpBoxUpdateFinishedSound.ResumeLayout(false);
            this.grpBoxUpdateFinishedSound.PerformLayout();
            this.grpBoxErrorSound.ResumeLayout(false);
            this.grpBoxErrorSound.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.CheckBox chkBoxUpdateFinishedSoundPlay;
        private System.Windows.Forms.GroupBox grpBoxUpdateFinishedSound;
        private System.Windows.Forms.Label lblUpdateFinishedSound;
        private System.Windows.Forms.GroupBox grpBoxErrorSound;
        private System.Windows.Forms.Label lblErrorSound;
        private System.Windows.Forms.Button btnErrorSound;
        private System.Windows.Forms.CheckBox chkBoxErrorSoundPlay;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnUpdateFinishedSound;
    }
}