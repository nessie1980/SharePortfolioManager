using System.ComponentModel;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    partial class FrmShareCostEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.grpBoxCosts = new System.Windows.Forms.GroupBox();
            this.tabCtrlCosts = new System.Windows.Forms.TabControl();
            this.grpBoxAddCost = new System.Windows.Forms.GroupBox();
            this.btnCostDocumentBrowse = new System.Windows.Forms.Button();
            this.txtBoxAddCostDoc = new System.Windows.Forms.TextBox();
            this.lblCostDoc = new System.Windows.Forms.Label();
            this.datePickerAddCostTime = new System.Windows.Forms.DateTimePicker();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblCostUnit = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.datePickerAddCostDate = new System.Windows.Forms.DateTimePicker();
            this.txtBoxAddCostValue = new System.Windows.Forms.TextBox();
            this.lblCostValue = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.grpBoxCosts.SuspendLayout();
            this.grpBoxAddCost.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxCosts
            // 
            this.grpBoxCosts.Controls.Add(this.tabCtrlCosts);
            this.grpBoxCosts.Location = new System.Drawing.Point(8, 175);
            this.grpBoxCosts.Name = "grpBoxCosts";
            this.grpBoxCosts.Size = new System.Drawing.Size(815, 155);
            this.grpBoxCosts.TabIndex = 5;
            this.grpBoxCosts.TabStop = false;
            this.grpBoxCosts.Text = "_costs";
            // 
            // tabCtrlCosts
            // 
            this.tabCtrlCosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlCosts.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlCosts.Location = new System.Drawing.Point(10, 22);
            this.tabCtrlCosts.Name = "tabCtrlCosts";
            this.tabCtrlCosts.SelectedIndex = 0;
            this.tabCtrlCosts.Size = new System.Drawing.Size(795, 122);
            this.tabCtrlCosts.TabIndex = 0;
            // 
            // grpBoxAddCost
            // 
            this.grpBoxAddCost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAddCost.Controls.Add(this.btnCostDocumentBrowse);
            this.grpBoxAddCost.Controls.Add(this.txtBoxAddCostDoc);
            this.grpBoxAddCost.Controls.Add(this.lblCostDoc);
            this.grpBoxAddCost.Controls.Add(this.datePickerAddCostTime);
            this.grpBoxAddCost.Controls.Add(this.btnDelete);
            this.grpBoxAddCost.Controls.Add(this.btnCancel);
            this.grpBoxAddCost.Controls.Add(this.btnReset);
            this.grpBoxAddCost.Controls.Add(this.lblCostUnit);
            this.grpBoxAddCost.Controls.Add(this.statusStrip1);
            this.grpBoxAddCost.Controls.Add(this.datePickerAddCostDate);
            this.grpBoxAddCost.Controls.Add(this.txtBoxAddCostValue);
            this.grpBoxAddCost.Controls.Add(this.lblCostValue);
            this.grpBoxAddCost.Controls.Add(this.lblDate);
            this.grpBoxAddCost.Controls.Add(this.btnAddSave);
            this.grpBoxAddCost.Location = new System.Drawing.Point(8, 5);
            this.grpBoxAddCost.Name = "grpBoxAddCost";
            this.grpBoxAddCost.Size = new System.Drawing.Size(815, 164);
            this.grpBoxAddCost.TabIndex = 4;
            this.grpBoxAddCost.TabStop = false;
            this.grpBoxAddCost.Text = "_grpBoxAddCosts";
            // 
            // btnCostDocumentBrowse
            // 
            this.btnCostDocumentBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCostDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCostDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCostDocumentBrowse.Location = new System.Drawing.Point(757, 76);
            this.btnCostDocumentBrowse.Name = "btnCostDocumentBrowse";
            this.btnCostDocumentBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnCostDocumentBrowse.TabIndex = 7;
            this.btnCostDocumentBrowse.Text = "...";
            this.btnCostDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCostDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnCostDocumentBrowse.Click += new System.EventHandler(this.OnBtnCostDocumentBrowse_Click);
            // 
            // txtBoxAddCostDoc
            // 
            this.txtBoxAddCostDoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxAddCostDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddCostDoc.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddCostDoc.Location = new System.Drawing.Point(380, 76);
            this.txtBoxAddCostDoc.Name = "txtBoxAddCostDoc";
            this.txtBoxAddCostDoc.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddCostDoc.TabIndex = 2;
            // 
            // lblCostDoc
            // 
            this.lblCostDoc.BackColor = System.Drawing.Color.LightGray;
            this.lblCostDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCostDoc.Location = new System.Drawing.Point(10, 76);
            this.lblCostDoc.Name = "lblCostDoc";
            this.lblCostDoc.Size = new System.Drawing.Size(365, 23);
            this.lblCostDoc.TabIndex = 19;
            this.lblCostDoc.Text = "_addCostDoc";
            this.lblCostDoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerAddCostTime
            // 
            this.datePickerAddCostTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.datePickerAddCostTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerAddCostTime.Location = new System.Drawing.Point(581, 22);
            this.datePickerAddCostTime.Name = "datePickerAddCostTime";
            this.datePickerAddCostTime.ShowUpDown = true;
            this.datePickerAddCostTime.Size = new System.Drawing.Size(169, 23);
            this.datePickerAddCostTime.TabIndex = 18;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(297, 104);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(166, 31);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(641, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(166, 31);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(469, 104);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(166, 31);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // lblCostUnit
            // 
            this.lblCostUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCostUnit.Location = new System.Drawing.Point(755, 49);
            this.lblCostUnit.Name = "lblCostUnit";
            this.lblCostUnit.Size = new System.Drawing.Size(50, 25);
            this.lblCostUnit.TabIndex = 12;
            this.lblCostUnit.Text = "_lblCostUnit";
            this.lblCostUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(3, 139);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(809, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessage.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // datePickerAddCostDate
            // 
            this.datePickerAddCostDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datePickerAddCostDate.CustomFormat = "";
            this.datePickerAddCostDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerAddCostDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerAddCostDate.Location = new System.Drawing.Point(380, 22);
            this.datePickerAddCostDate.Name = "datePickerAddCostDate";
            this.datePickerAddCostDate.Size = new System.Drawing.Size(195, 23);
            this.datePickerAddCostDate.TabIndex = 10;
            // 
            // txtBoxAddCostValue
            // 
            this.txtBoxAddCostValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxAddCostValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddCostValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddCostValue.Location = new System.Drawing.Point(380, 49);
            this.txtBoxAddCostValue.Name = "txtBoxAddCostValue";
            this.txtBoxAddCostValue.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddCostValue.TabIndex = 0;
            // 
            // lblCostValue
            // 
            this.lblCostValue.BackColor = System.Drawing.Color.LightGray;
            this.lblCostValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCostValue.Location = new System.Drawing.Point(10, 49);
            this.lblCostValue.Name = "lblCostValue";
            this.lblCostValue.Size = new System.Drawing.Size(365, 23);
            this.lblCostValue.TabIndex = 3;
            this.lblCostValue.Text = "_addCostValue";
            this.lblCostValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Location = new System.Drawing.Point(10, 22);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(365, 23);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddSave
            // 
            this.btnAddSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(125, 104);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(166, 31);
            this.btnAddSave.TabIndex = 3;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
            // 
            // FrmShareCostEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(831, 336);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxCosts);
            this.Controls.Add(this.grpBoxAddCost);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(847, 374);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(847, 374);
            this.Name = "FrmShareCostEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareCostEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareCostEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareCostEdit_Load);
            this.grpBoxCosts.ResumeLayout(false);
            this.grpBoxAddCost.ResumeLayout(false);
            this.grpBoxAddCost.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox grpBoxCosts;
        private GroupBox grpBoxAddCost;
        private Button btnCancel;
        private Button btnReset;
        private Label lblCostUnit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelMessage;
        private DateTimePicker datePickerAddCostDate;
        private TextBox txtBoxAddCostValue;
        private Label lblCostValue;
        private Label lblDate;
        private Button btnAddSave;
        private TabControl tabCtrlCosts;
        private Button btnDelete;
        private DateTimePicker datePickerAddCostTime;
        private TextBox txtBoxAddCostDoc;
        private Label lblCostDoc;
        private Button btnCostDocumentBrowse;
    }
}