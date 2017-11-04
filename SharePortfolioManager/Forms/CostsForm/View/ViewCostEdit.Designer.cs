using System.ComponentModel;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.CostsForm.View
{
    partial class ViewCostEdit
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
            this.grpBoxOverview = new System.Windows.Forms.GroupBox();
            this.tabCtrlCosts = new System.Windows.Forms.TabControl();
            this.grpBoxAdd = new System.Windows.Forms.GroupBox();
            this.btnDocumentBrowse = new System.Windows.Forms.Button();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.lblDocument = new System.Windows.Forms.Label();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblAddCostsUnit = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageCostEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.txtBoxCosts = new System.Windows.Forms.TextBox();
            this.lblCosts = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.grpBoxOverview.SuspendLayout();
            this.grpBoxAdd.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxOverview
            // 
            this.grpBoxOverview.Controls.Add(this.tabCtrlCosts);
            this.grpBoxOverview.Location = new System.Drawing.Point(8, 175);
            this.grpBoxOverview.Name = "grpBoxOverview";
            this.grpBoxOverview.Size = new System.Drawing.Size(815, 155);
            this.grpBoxOverview.TabIndex = 5;
            this.grpBoxOverview.TabStop = false;
            this.grpBoxOverview.Text = "_costs";
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
            this.tabCtrlCosts.SelectedIndexChanged += new System.EventHandler(this.tabCtrlCosts_SelectedIndexChanged);
            this.tabCtrlCosts.MouseEnter += new System.EventHandler(this.tabCtrlCosts_MouseEnter);
            this.tabCtrlCosts.MouseLeave += new System.EventHandler(this.tabCtrlCosts_MouseLeave);
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAdd.Controls.Add(this.btnDocumentBrowse);
            this.grpBoxAdd.Controls.Add(this.txtBoxDocument);
            this.grpBoxAdd.Controls.Add(this.lblDocument);
            this.grpBoxAdd.Controls.Add(this.datePickerTime);
            this.grpBoxAdd.Controls.Add(this.btnDelete);
            this.grpBoxAdd.Controls.Add(this.btnCancel);
            this.grpBoxAdd.Controls.Add(this.btnReset);
            this.grpBoxAdd.Controls.Add(this.lblAddCostsUnit);
            this.grpBoxAdd.Controls.Add(this.statusStrip1);
            this.grpBoxAdd.Controls.Add(this.datePickerDate);
            this.grpBoxAdd.Controls.Add(this.txtBoxCosts);
            this.grpBoxAdd.Controls.Add(this.lblCosts);
            this.grpBoxAdd.Controls.Add(this.lblDate);
            this.grpBoxAdd.Controls.Add(this.btnAddSave);
            this.grpBoxAdd.Location = new System.Drawing.Point(8, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(815, 164);
            this.grpBoxAdd.TabIndex = 4;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAddCosts";
            // 
            // btnDocumentBrowse
            // 
            this.btnDocumentBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(757, 76);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnDocumentBrowse.TabIndex = 7;
            this.btnDocumentBrowse.Text = "...";
            this.btnDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnDocumentBrowse.Click += new System.EventHandler(this.OnBtnDocumentBrowse_Click);
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(380, 76);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(370, 23);
            this.txtBoxDocument.TabIndex = 2;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OntxtBoxDocument_Leave);
            // 
            // lblDocument
            // 
            this.lblDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocument.Location = new System.Drawing.Point(10, 76);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(365, 23);
            this.lblDocument.TabIndex = 19;
            this.lblDocument.Text = "_addCostDoc";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerTime
            // 
            this.datePickerTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(581, 22);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(169, 23);
            this.datePickerTime.TabIndex = 18;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Leave += new System.EventHandler(this.datePickerTime_Leave);
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
            // lblAddCostsUnit
            // 
            this.lblAddCostsUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddCostsUnit.Location = new System.Drawing.Point(755, 49);
            this.lblAddCostsUnit.Name = "lblAddCostsUnit";
            this.lblAddCostsUnit.Size = new System.Drawing.Size(50, 25);
            this.lblAddCostsUnit.TabIndex = 12;
            this.lblAddCostsUnit.Text = "_lblCostUnit";
            this.lblAddCostsUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageCostEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 139);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(809, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessageCostEdit
            // 
            this.toolStripStatusLabelMessageCostEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageCostEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessageCostEdit.Name = "toolStripStatusLabelMessageCostEdit";
            this.toolStripStatusLabelMessageCostEdit.Size = new System.Drawing.Size(0, 17);
            // 
            // datePickerDate
            // 
            this.datePickerDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(380, 22);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(195, 23);
            this.datePickerDate.TabIndex = 10;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.datePickerDate.Leave += new System.EventHandler(this.OnDatePickerDate_Leave);
            // 
            // txtBoxCosts
            // 
            this.txtBoxCosts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxCosts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCosts.Location = new System.Drawing.Point(380, 49);
            this.txtBoxCosts.Name = "txtBoxCosts";
            this.txtBoxCosts.Size = new System.Drawing.Size(370, 23);
            this.txtBoxCosts.TabIndex = 0;
            this.txtBoxCosts.TextChanged += new System.EventHandler(this.OnTxtBoxCosts_TextChanged);
            this.txtBoxCosts.Leave += new System.EventHandler(this.OnTxtBoxCosts_Leave);
            // 
            // lblCosts
            // 
            this.lblCosts.BackColor = System.Drawing.Color.LightGray;
            this.lblCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCosts.Location = new System.Drawing.Point(10, 49);
            this.lblCosts.Name = "lblCosts";
            this.lblCosts.Size = new System.Drawing.Size(365, 23);
            this.lblCosts.TabIndex = 3;
            this.lblCosts.Text = "_addCostValue";
            this.lblCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // ViewCostEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(831, 336);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxOverview);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(847, 374);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(847, 374);
            this.Name = "ViewCostEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareCostEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareCostEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareCostEdit_Load);
            this.grpBoxOverview.ResumeLayout(false);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox grpBoxOverview;
        private GroupBox grpBoxAdd;
        private Button btnCancel;
        private Button btnReset;
        private Label lblAddCostsUnit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelMessageCostEdit;
        private DateTimePicker datePickerDate;
        private TextBox txtBoxCosts;
        private Label lblCosts;
        private Label lblDate;
        private Button btnAddSave;
        private TabControl tabCtrlCosts;
        private Button btnDelete;
        private DateTimePicker datePickerTime;
        private TextBox txtBoxDocument;
        private Label lblDocument;
        private Button btnDocumentBrowse;
    }
}