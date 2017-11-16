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
            this.tblLayPnlOverviewTabControl = new System.Windows.Forms.TableLayoutPanel();
            this.tabCtrlCosts = new System.Windows.Forms.TabControl();
            this.grpBoxAdd = new System.Windows.Forms.GroupBox();
            this.tblLayPnlCostsButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tblLayPnlCostsInput = new System.Windows.Forms.TableLayoutPanel();
            this.btnDocumentBrowse = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblCosts = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.lblDocument = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.txtBoxCosts = new System.Windows.Forms.TextBox();
            this.lblAddCostsUnit = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageCostEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxOverview.SuspendLayout();
            this.tblLayPnlOverviewTabControl.SuspendLayout();
            this.grpBoxAdd.SuspendLayout();
            this.tblLayPnlCostsButtons.SuspendLayout();
            this.tblLayPnlCostsInput.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxOverview
            // 
            this.grpBoxOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxOverview.Controls.Add(this.tblLayPnlOverviewTabControl);
            this.grpBoxOverview.Location = new System.Drawing.Point(5, 181);
            this.grpBoxOverview.Name = "grpBoxOverview";
            this.grpBoxOverview.Size = new System.Drawing.Size(825, 162);
            this.grpBoxOverview.TabIndex = 5;
            this.grpBoxOverview.TabStop = false;
            this.grpBoxOverview.Text = "_costs";
            // 
            // tblLayPnlOverviewTabControl
            // 
            this.tblLayPnlOverviewTabControl.ColumnCount = 1;
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOverviewTabControl.Controls.Add(this.tabCtrlCosts, 0, 0);
            this.tblLayPnlOverviewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOverviewTabControl.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlOverviewTabControl.Name = "tblLayPnlOverviewTabControl";
            this.tblLayPnlOverviewTabControl.RowCount = 1;
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 143F));
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(819, 140);
            this.tblLayPnlOverviewTabControl.TabIndex = 0;
            // 
            // tabCtrlCosts
            // 
            this.tabCtrlCosts.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlCosts.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlCosts.Name = "tabCtrlCosts";
            this.tabCtrlCosts.SelectedIndex = 0;
            this.tabCtrlCosts.Size = new System.Drawing.Size(813, 134);
            this.tabCtrlCosts.TabIndex = 0;
            this.tabCtrlCosts.SelectedIndexChanged += new System.EventHandler(this.tabCtrlCosts_SelectedIndexChanged);
            this.tabCtrlCosts.MouseEnter += new System.EventHandler(this.tabCtrlCosts_MouseEnter);
            this.tabCtrlCosts.MouseLeave += new System.EventHandler(this.tabCtrlCosts_MouseLeave);
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAdd.Controls.Add(this.tblLayPnlCostsButtons);
            this.grpBoxAdd.Controls.Add(this.tblLayPnlCostsInput);
            this.grpBoxAdd.Controls.Add(this.statusStrip1);
            this.grpBoxAdd.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(825, 170);
            this.grpBoxAdd.TabIndex = 4;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAddCosts";
            // 
            // tblLayPnlCostsButtons
            // 
            this.tblLayPnlCostsButtons.ColumnCount = 5;
            this.tblLayPnlCostsButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlCostsButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlCostsButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlCostsButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlCostsButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlCostsButtons.Controls.Add(this.btnAddSave, 1, 0);
            this.tblLayPnlCostsButtons.Controls.Add(this.btnDelete, 2, 0);
            this.tblLayPnlCostsButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tblLayPnlCostsButtons.Controls.Add(this.btnReset, 3, 0);
            this.tblLayPnlCostsButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlCostsButtons.Location = new System.Drawing.Point(3, 103);
            this.tblLayPnlCostsButtons.Name = "tblLayPnlCostsButtons";
            this.tblLayPnlCostsButtons.RowCount = 1;
            this.tblLayPnlCostsButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlCostsButtons.Size = new System.Drawing.Size(819, 39);
            this.tblLayPnlCostsButtons.TabIndex = 13;
            // 
            // btnAddSave
            // 
            this.btnAddSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(138, 3);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(165, 33);
            this.btnAddSave.TabIndex = 3;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(309, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(165, 33);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(651, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(165, 33);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Enabled = false;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(480, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(165, 33);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // tblLayPnlCostsInput
            // 
            this.tblLayPnlCostsInput.ColumnCount = 4;
            this.tblLayPnlCostsInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tblLayPnlCostsInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlCostsInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlCostsInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlCostsInput.Controls.Add(this.btnDocumentBrowse, 3, 2);
            this.tblLayPnlCostsInput.Controls.Add(this.lblDate, 0, 0);
            this.tblLayPnlCostsInput.Controls.Add(this.lblCosts, 0, 1);
            this.tblLayPnlCostsInput.Controls.Add(this.txtBoxDocument, 1, 2);
            this.tblLayPnlCostsInput.Controls.Add(this.lblDocument, 0, 2);
            this.tblLayPnlCostsInput.Controls.Add(this.datePickerDate, 1, 0);
            this.tblLayPnlCostsInput.Controls.Add(this.datePickerTime, 2, 0);
            this.tblLayPnlCostsInput.Controls.Add(this.txtBoxCosts, 1, 1);
            this.tblLayPnlCostsInput.Controls.Add(this.lblAddCostsUnit, 3, 1);
            this.tblLayPnlCostsInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlCostsInput.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlCostsInput.Name = "tblLayPnlCostsInput";
            this.tblLayPnlCostsInput.RowCount = 3;
            this.tblLayPnlCostsInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlCostsInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlCostsInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlCostsInput.Size = new System.Drawing.Size(819, 84);
            this.tblLayPnlCostsInput.TabIndex = 12;
            // 
            // btnDocumentBrowse
            // 
            this.btnDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(745, 59);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(71, 22);
            this.btnDocumentBrowse.TabIndex = 7;
            this.btnDocumentBrowse.Text = "...";
            this.btnDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnDocumentBrowse.Click += new System.EventHandler(this.OnBtnDocumentBrowse_Click);
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Location = new System.Drawing.Point(3, 3);
            this.lblDate.Margin = new System.Windows.Forms.Padding(3);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(244, 22);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCosts
            // 
            this.lblCosts.BackColor = System.Drawing.Color.LightGray;
            this.lblCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCosts.Location = new System.Drawing.Point(3, 31);
            this.lblCosts.Margin = new System.Windows.Forms.Padding(3);
            this.lblCosts.Name = "lblCosts";
            this.lblCosts.Size = new System.Drawing.Size(244, 22);
            this.lblCosts.TabIndex = 3;
            this.lblCosts.Text = "_addCostValue";
            this.lblCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlCostsInput.SetColumnSpan(this.txtBoxDocument, 2);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(253, 59);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(486, 23);
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
            this.lblDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocument.Location = new System.Drawing.Point(3, 59);
            this.lblDocument.Margin = new System.Windows.Forms.Padding(3);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(244, 22);
            this.lblDocument.TabIndex = 19;
            this.lblDocument.Text = "_addCostDoc";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(253, 3);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(240, 23);
            this.datePickerDate.TabIndex = 10;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.datePickerDate.Leave += new System.EventHandler(this.OnDatePickerDate_Leave);
            // 
            // datePickerTime
            // 
            this.datePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(499, 3);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(240, 23);
            this.datePickerTime.TabIndex = 18;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Leave += new System.EventHandler(this.datePickerTime_Leave);
            // 
            // txtBoxCosts
            // 
            this.txtBoxCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlCostsInput.SetColumnSpan(this.txtBoxCosts, 2);
            this.txtBoxCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCosts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCosts.Location = new System.Drawing.Point(253, 31);
            this.txtBoxCosts.Name = "txtBoxCosts";
            this.txtBoxCosts.Size = new System.Drawing.Size(486, 23);
            this.txtBoxCosts.TabIndex = 0;
            this.txtBoxCosts.TextChanged += new System.EventHandler(this.OnTxtBoxCosts_TextChanged);
            this.txtBoxCosts.Leave += new System.EventHandler(this.OnTxtBoxCosts_Leave);
            // 
            // lblAddCostsUnit
            // 
            this.lblAddCostsUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddCostsUnit.Location = new System.Drawing.Point(745, 31);
            this.lblAddCostsUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblAddCostsUnit.Name = "lblAddCostsUnit";
            this.lblAddCostsUnit.Size = new System.Drawing.Size(71, 22);
            this.lblAddCostsUnit.TabIndex = 12;
            this.lblAddCostsUnit.Text = "_lblCostUnit";
            this.lblAddCostsUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageCostEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 145);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
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
            // ViewCostEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 348);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxOverview);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 450);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 350);
            this.Name = "ViewCostEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareCostEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareCostEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareCostEdit_Load);
            this.grpBoxOverview.ResumeLayout(false);
            this.tblLayPnlOverviewTabControl.ResumeLayout(false);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
            this.tblLayPnlCostsButtons.ResumeLayout(false);
            this.tblLayPnlCostsInput.ResumeLayout(false);
            this.tblLayPnlCostsInput.PerformLayout();
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
        private TableLayoutPanel tblLayPnlCostsInput;
        private TableLayoutPanel tblLayPnlCostsButtons;
        private TableLayoutPanel tblLayPnlOverviewTabControl;
    }
}