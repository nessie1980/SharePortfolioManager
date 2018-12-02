using System.ComponentModel;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.BrokeragesForm.View
{
    partial class ViewBrokerageEdit
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
            this.tabCtrlBrokerage = new System.Windows.Forms.TabControl();
            this.grpBoxAdd = new System.Windows.Forms.GroupBox();
            this.tblLayPnlBrokerageButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tblLayPnlBrokerageInput = new System.Windows.Forms.TableLayoutPanel();
            this.btnDocumentBrowse = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblBrokerage = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.lblDocument = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.txtBoxBrokerage = new System.Windows.Forms.TextBox();
            this.lblAddBrokerageUnit = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageBrokerageEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxOverview.SuspendLayout();
            this.tblLayPnlOverviewTabControl.SuspendLayout();
            this.grpBoxAdd.SuspendLayout();
            this.tblLayPnlBrokerageButtons.SuspendLayout();
            this.tblLayPnlBrokerageInput.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxOverview
            // 
            this.grpBoxOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxOverview.Controls.Add(this.tblLayPnlOverviewTabControl);
            this.grpBoxOverview.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxOverview.Location = new System.Drawing.Point(5, 160);
            this.grpBoxOverview.Name = "grpBoxOverview";
            this.grpBoxOverview.Size = new System.Drawing.Size(825, 183);
            this.grpBoxOverview.TabIndex = 5;
            this.grpBoxOverview.TabStop = false;
            this.grpBoxOverview.Text = "_brokerage";
            this.grpBoxOverview.MouseLeave += new System.EventHandler(this.OnGrpBoxOverview_MouseLeave);
            // 
            // tblLayPnlOverviewTabControl
            // 
            this.tblLayPnlOverviewTabControl.ColumnCount = 1;
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOverviewTabControl.Controls.Add(this.tabCtrlBrokerage, 0, 0);
            this.tblLayPnlOverviewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOverviewTabControl.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlOverviewTabControl.Name = "tblLayPnlOverviewTabControl";
            this.tblLayPnlOverviewTabControl.RowCount = 1;
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(819, 162);
            this.tblLayPnlOverviewTabControl.TabIndex = 0;
            // 
            // tabCtrlBrokerage
            // 
            this.tabCtrlBrokerage.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrlBrokerage.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlBrokerage.Name = "tabCtrlBrokerage";
            this.tabCtrlBrokerage.SelectedIndex = 0;
            this.tabCtrlBrokerage.Size = new System.Drawing.Size(813, 156);
            this.tabCtrlBrokerage.TabIndex = 0;
            this.tabCtrlBrokerage.SelectedIndexChanged += new System.EventHandler(this.TabCtrlBrokerage_SelectedIndexChanged);
            this.tabCtrlBrokerage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTabCtrlBrokerage_KeyDown);
            this.tabCtrlBrokerage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTabCtrlBrokerage_KeyPress);
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBrokerageButtons);
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBrokerageInput);
            this.grpBoxAdd.Controls.Add(this.statusStrip1);
            this.grpBoxAdd.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxAdd.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(825, 149);
            this.grpBoxAdd.TabIndex = 4;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAddBrokerage";
            // 
            // tblLayPnlBrokerageButtons
            // 
            this.tblLayPnlBrokerageButtons.ColumnCount = 5;
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnAddSave, 1, 0);
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnDelete, 2, 0);
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnReset, 3, 0);
            this.tblLayPnlBrokerageButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlBrokerageButtons.Location = new System.Drawing.Point(3, 90);
            this.tblLayPnlBrokerageButtons.Name = "tblLayPnlBrokerageButtons";
            this.tblLayPnlBrokerageButtons.RowCount = 1;
            this.tblLayPnlBrokerageButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlBrokerageButtons.Size = new System.Drawing.Size(819, 33);
            this.tblLayPnlBrokerageButtons.TabIndex = 13;
            // 
            // btnAddSave
            // 
            this.btnAddSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSave.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(100, 1);
            this.btnAddSave.Margin = new System.Windows.Forms.Padding(1);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(178, 31);
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
            this.btnDelete.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(280, 1);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(178, 31);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(640, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(178, 31);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(460, 1);
            this.btnReset.Margin = new System.Windows.Forms.Padding(1);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(178, 31);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // tblLayPnlBrokerageInput
            // 
            this.tblLayPnlBrokerageInput.ColumnCount = 4;
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlBrokerageInput.Controls.Add(this.btnDocumentBrowse, 3, 2);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblDate, 0, 0);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblBrokerage, 0, 1);
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxDocument, 1, 2);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblDocument, 0, 2);
            this.tblLayPnlBrokerageInput.Controls.Add(this.datePickerDate, 1, 0);
            this.tblLayPnlBrokerageInput.Controls.Add(this.datePickerTime, 2, 0);
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxBrokerage, 1, 1);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblAddBrokerageUnit, 3, 1);
            this.tblLayPnlBrokerageInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlBrokerageInput.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlBrokerageInput.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlBrokerageInput.Name = "tblLayPnlBrokerageInput";
            this.tblLayPnlBrokerageInput.RowCount = 3;
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.Size = new System.Drawing.Size(819, 72);
            this.tblLayPnlBrokerageInput.TabIndex = 12;
            // 
            // btnDocumentBrowse
            // 
            this.btnDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(743, 49);
            this.btnDocumentBrowse.Margin = new System.Windows.Forms.Padding(1);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(75, 22);
            this.btnDocumentBrowse.TabIndex = 7;
            this.btnDocumentBrowse.Text = "...";
            this.btnDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnDocumentBrowse.Click += new System.EventHandler(this.OnBtnBrokerageDocumentBrowse_Click);
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(1, 1);
            this.lblDate.Margin = new System.Windows.Forms.Padding(1);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(248, 22);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrokerage
            // 
            this.lblBrokerage.BackColor = System.Drawing.Color.LightGray;
            this.lblBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerage.Location = new System.Drawing.Point(1, 25);
            this.lblBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerage.Name = "lblBrokerage";
            this.lblBrokerage.Size = new System.Drawing.Size(248, 22);
            this.lblBrokerage.TabIndex = 3;
            this.lblBrokerage.Text = "_addBrokerageValue";
            this.lblBrokerage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxDocument, 2);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(251, 49);
            this.txtBoxDocument.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(490, 22);
            this.txtBoxDocument.TabIndex = 2;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Enter += new System.EventHandler(this.OnTxtBoxDocument_Enter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // lblDocument
            // 
            this.lblDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocument.Location = new System.Drawing.Point(1, 49);
            this.lblDocument.Margin = new System.Windows.Forms.Padding(1);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(248, 22);
            this.lblDocument.TabIndex = 19;
            this.lblDocument.Text = "_addBrokerageDoc";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(251, 1);
            this.datePickerDate.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(244, 22);
            this.datePickerDate.TabIndex = 10;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.datePickerDate.Enter += new System.EventHandler(this.OnDatePickerDate_Enter);
            this.datePickerDate.Leave += new System.EventHandler(this.OnDatePickerDate_Leave);
            // 
            // datePickerTime
            // 
            this.datePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerTime.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(497, 1);
            this.datePickerTime.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(244, 22);
            this.datePickerTime.TabIndex = 18;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Enter += new System.EventHandler(this.OnDatePickerTime_Enter);
            this.datePickerTime.Leave += new System.EventHandler(this.datePickerTime_Leave);
            // 
            // txtBoxBrokerage
            // 
            this.txtBoxBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxBrokerage, 2);
            this.txtBoxBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBrokerage.Location = new System.Drawing.Point(251, 25);
            this.txtBoxBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxBrokerage.Name = "txtBoxBrokerage";
            this.txtBoxBrokerage.Size = new System.Drawing.Size(490, 22);
            this.txtBoxBrokerage.TabIndex = 0;
            this.txtBoxBrokerage.TextChanged += new System.EventHandler(this.OnTxtBoxBrokerage_TextChanged);
            this.txtBoxBrokerage.Enter += new System.EventHandler(this.OnTxtBoxBrokerage_Enter);
            this.txtBoxBrokerage.Leave += new System.EventHandler(this.OnTxtBoxBrokerage_Leave);
            // 
            // lblAddBrokerageUnit
            // 
            this.lblAddBrokerageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddBrokerageUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddBrokerageUnit.Location = new System.Drawing.Point(743, 25);
            this.lblAddBrokerageUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddBrokerageUnit.Name = "lblAddBrokerageUnit";
            this.lblAddBrokerageUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddBrokerageUnit.TabIndex = 12;
            this.lblAddBrokerageUnit.Text = "_lblBrokerageUnit";
            this.lblAddBrokerageUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageBrokerageEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 124);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessageBrokerageEdit
            // 
            this.toolStripStatusLabelMessageBrokerageEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageBrokerageEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessageBrokerageEdit.Name = "toolStripStatusLabelMessageBrokerageEdit";
            this.toolStripStatusLabelMessageBrokerageEdit.Size = new System.Drawing.Size(0, 17);
            // 
            // ViewBrokerageEdit
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
            this.Name = "ViewBrokerageEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareBrokerageEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareBrokerageEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareBrokerageEdit_Load);
            this.grpBoxOverview.ResumeLayout(false);
            this.tblLayPnlOverviewTabControl.ResumeLayout(false);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
            this.tblLayPnlBrokerageButtons.ResumeLayout(false);
            this.tblLayPnlBrokerageInput.ResumeLayout(false);
            this.tblLayPnlBrokerageInput.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox grpBoxOverview;
        private GroupBox grpBoxAdd;
        private Button btnCancel;
        private Button btnReset;
        private Label lblAddBrokerageUnit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelMessageBrokerageEdit;
        private DateTimePicker datePickerDate;
        private TextBox txtBoxBrokerage;
        private Label lblBrokerage;
        private Label lblDate;
        private Button btnAddSave;
        private TabControl tabCtrlBrokerage;
        private Button btnDelete;
        private DateTimePicker datePickerTime;
        private TextBox txtBoxDocument;
        private Label lblDocument;
        private Button btnDocumentBrowse;
        private TableLayoutPanel tblLayPnlBrokerageInput;
        private TableLayoutPanel tblLayPnlBrokerageButtons;
        private TableLayoutPanel tblLayPnlOverviewTabControl;
    }
}