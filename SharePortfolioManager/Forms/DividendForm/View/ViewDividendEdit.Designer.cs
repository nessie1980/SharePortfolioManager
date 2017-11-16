namespace SharePortfolioManager.Forms.DividendForm.View
{
    partial class ViewDividendEdit
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
            this.btnAddSave = new System.Windows.Forms.Button();
            this.grpBoxAddDividend = new System.Windows.Forms.GroupBox();
            this.tblLayPnlDividendButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tblLayPnlDividendInput = new System.Windows.Forms.TableLayoutPanel();
            this.lblDate = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.lblCapitalGainsTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxTax = new System.Windows.Forms.TextBox();
            this.btnAddDocumentBrowse = new System.Windows.Forms.Button();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.chkBoxEnableFC = new System.Windows.Forms.CheckBox();
            this.lblDividendExchangeRatio = new System.Windows.Forms.Label();
            this.lblSolidarityTax = new System.Windows.Forms.Label();
            this.lblCapitalGainsTax = new System.Windows.Forms.Label();
            this.lblEnableForeignCurrency = new System.Windows.Forms.Label();
            this.lblTaxAtSource = new System.Windows.Forms.Label();
            this.lblPayout = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblDividendRate = new System.Windows.Forms.Label();
            this.txtBoxSolidarityTax = new System.Windows.Forms.TextBox();
            this.txtBoxCapitalGainsTax = new System.Windows.Forms.TextBox();
            this.txtBoxTaxAtSource = new System.Windows.Forms.TextBox();
            this.txtBoxPayout = new System.Windows.Forms.TextBox();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.txtBoxRate = new System.Windows.Forms.TextBox();
            this.txtBoxExchangeRatio = new System.Windows.Forms.TextBox();
            this.cbxBoxDividendFCUnit = new System.Windows.Forms.ComboBox();
            this.lblTaxAtSourceUnit = new System.Windows.Forms.Label();
            this.lblPayoutUnit = new System.Windows.Forms.Label();
            this.txtBoxPayoutFC = new System.Windows.Forms.TextBox();
            this.lblVolumeUnit = new System.Windows.Forms.Label();
            this.lblPayoutFCUnit = new System.Windows.Forms.Label();
            this.lblSolidarityTaxUnit = new System.Windows.Forms.Label();
            this.lblDividendRateUnit = new System.Windows.Forms.Label();
            this.lblAddDocument = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.lblAddPrice = new System.Windows.Forms.Label();
            this.txtBoxPrice = new System.Windows.Forms.TextBox();
            this.lblAddYield = new System.Windows.Forms.Label();
            this.txtBoxYield = new System.Windows.Forms.TextBox();
            this.lblPriceUnit = new System.Windows.Forms.Label();
            this.lblYieldUnit = new System.Windows.Forms.Label();
            this.lblAddPayoutAfterTax = new System.Windows.Forms.Label();
            this.txtBoxPayoutAfterTax = new System.Windows.Forms.TextBox();
            this.lblTaxUnit = new System.Windows.Forms.Label();
            this.lblPayoutAfterTaxUnit = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAddTax = new System.Windows.Forms.Label();
            this.grpBoxDividends = new System.Windows.Forms.GroupBox();
            this.tblLayPnlOverviewTabControl = new System.Windows.Forms.TableLayoutPanel();
            this.tabCtrlDividends = new System.Windows.Forms.TabControl();
            this.lblAddDate = new System.Windows.Forms.Label();
            this.lblAddDividendExchangeRatio = new System.Windows.Forms.Label();
            this.lblAddVolume = new System.Windows.Forms.Label();
            this.lblAddTaxAtSource = new System.Windows.Forms.Label();
            this.grpBoxAddDividend.SuspendLayout();
            this.tblLayPnlDividendButtons.SuspendLayout();
            this.tblLayPnlDividendInput.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.grpBoxDividends.SuspendLayout();
            this.tblLayPnlOverviewTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddSave
            // 
            this.btnAddSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(138, 3);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(165, 33);
            this.btnAddSave.TabIndex = 16;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.BtnAddSave_Click);
            // 
            // grpBoxAddDividend
            // 
            this.grpBoxAddDividend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAddDividend.Controls.Add(this.tblLayPnlDividendButtons);
            this.grpBoxAddDividend.Controls.Add(this.tblLayPnlDividendInput);
            this.grpBoxAddDividend.Controls.Add(this.statusStrip1);
            this.grpBoxAddDividend.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAddDividend.Name = "grpBoxAddDividend";
            this.grpBoxAddDividend.Size = new System.Drawing.Size(825, 478);
            this.grpBoxAddDividend.TabIndex = 2;
            this.grpBoxAddDividend.TabStop = false;
            this.grpBoxAddDividend.Text = "_grpBoxAddDividend";
            // 
            // tblLayPnlDividendButtons
            // 
            this.tblLayPnlDividendButtons.ColumnCount = 5;
            this.tblLayPnlDividendButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlDividendButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlDividendButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlDividendButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlDividendButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlDividendButtons.Controls.Add(this.btnReset, 3, 0);
            this.tblLayPnlDividendButtons.Controls.Add(this.btnDelete, 2, 0);
            this.tblLayPnlDividendButtons.Controls.Add(this.btnAddSave, 1, 0);
            this.tblLayPnlDividendButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tblLayPnlDividendButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlDividendButtons.Location = new System.Drawing.Point(3, 411);
            this.tblLayPnlDividendButtons.Name = "tblLayPnlDividendButtons";
            this.tblLayPnlDividendButtons.RowCount = 1;
            this.tblLayPnlDividendButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlDividendButtons.Size = new System.Drawing.Size(819, 39);
            this.tblLayPnlDividendButtons.TabIndex = 21;
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Enabled = false;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(480, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(165, 33);
            this.btnReset.TabIndex = 18;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(309, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(165, 33);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDividendDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(651, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(165, 33);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // tblLayPnlDividendInput
            // 
            this.tblLayPnlDividendInput.ColumnCount = 5;
            this.tblLayPnlDividendInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tblLayPnlDividendInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlDividendInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tblLayPnlDividendInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlDividendInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlDividendInput.Controls.Add(this.lblDate, 0, 0);
            this.tblLayPnlDividendInput.Controls.Add(this.datePickerDate, 1, 0);
            this.tblLayPnlDividendInput.Controls.Add(this.lblCapitalGainsTaxUnit, 4, 7);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxTax, 1, 9);
            this.tblLayPnlDividendInput.Controls.Add(this.btnAddDocumentBrowse, 4, 13);
            this.tblLayPnlDividendInput.Controls.Add(this.datePickerTime, 3, 0);
            this.tblLayPnlDividendInput.Controls.Add(this.chkBoxEnableFC, 1, 1);
            this.tblLayPnlDividendInput.Controls.Add(this.lblDividendExchangeRatio, 0, 2);
            this.tblLayPnlDividendInput.Controls.Add(this.lblSolidarityTax, 0, 8);
            this.tblLayPnlDividendInput.Controls.Add(this.lblCapitalGainsTax, 0, 7);
            this.tblLayPnlDividendInput.Controls.Add(this.lblEnableForeignCurrency, 0, 1);
            this.tblLayPnlDividendInput.Controls.Add(this.lblTaxAtSource, 0, 6);
            this.tblLayPnlDividendInput.Controls.Add(this.lblPayout, 0, 5);
            this.tblLayPnlDividendInput.Controls.Add(this.lblVolume, 0, 4);
            this.tblLayPnlDividendInput.Controls.Add(this.lblDividendRate, 0, 3);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxSolidarityTax, 1, 8);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxCapitalGainsTax, 1, 7);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxTaxAtSource, 1, 6);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxPayout, 1, 5);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxVolume, 1, 4);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxRate, 1, 3);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxExchangeRatio, 1, 2);
            this.tblLayPnlDividendInput.Controls.Add(this.cbxBoxDividendFCUnit, 3, 2);
            this.tblLayPnlDividendInput.Controls.Add(this.lblTaxAtSourceUnit, 4, 6);
            this.tblLayPnlDividendInput.Controls.Add(this.lblPayoutUnit, 2, 5);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxPayoutFC, 3, 5);
            this.tblLayPnlDividendInput.Controls.Add(this.lblVolumeUnit, 4, 4);
            this.tblLayPnlDividendInput.Controls.Add(this.lblPayoutFCUnit, 4, 5);
            this.tblLayPnlDividendInput.Controls.Add(this.lblSolidarityTaxUnit, 4, 8);
            this.tblLayPnlDividendInput.Controls.Add(this.lblDividendRateUnit, 4, 3);
            this.tblLayPnlDividendInput.Controls.Add(this.lblAddDocument, 0, 13);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxDocument, 1, 13);
            this.tblLayPnlDividendInput.Controls.Add(this.lblAddPrice, 0, 12);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxPrice, 1, 12);
            this.tblLayPnlDividendInput.Controls.Add(this.lblAddYield, 0, 11);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxYield, 1, 11);
            this.tblLayPnlDividendInput.Controls.Add(this.lblPriceUnit, 4, 12);
            this.tblLayPnlDividendInput.Controls.Add(this.lblYieldUnit, 4, 11);
            this.tblLayPnlDividendInput.Controls.Add(this.lblAddPayoutAfterTax, 0, 10);
            this.tblLayPnlDividendInput.Controls.Add(this.txtBoxPayoutAfterTax, 1, 10);
            this.tblLayPnlDividendInput.Controls.Add(this.lblTaxUnit, 4, 9);
            this.tblLayPnlDividendInput.Controls.Add(this.lblPayoutAfterTaxUnit, 4, 10);
            this.tblLayPnlDividendInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlDividendInput.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlDividendInput.Name = "tblLayPnlDividendInput";
            this.tblLayPnlDividendInput.RowCount = 14;
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlDividendInput.Size = new System.Drawing.Size(819, 392);
            this.tblLayPnlDividendInput.TabIndex = 20;
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Location = new System.Drawing.Point(3, 3);
            this.lblDate.Margin = new System.Windows.Forms.Padding(3);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(294, 22);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(303, 3);
            this.datePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(177, 23);
            this.datePickerDate.TabIndex = 0;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.datePickerDate.Leave += new System.EventHandler(this.OnDatePickerDate_Leave);
            // 
            // lblCapitalGainsTaxUnit
            // 
            this.lblCapitalGainsTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapitalGainsTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapitalGainsTaxUnit.Location = new System.Drawing.Point(745, 199);
            this.lblCapitalGainsTaxUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblCapitalGainsTaxUnit.Name = "lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.Size = new System.Drawing.Size(71, 22);
            this.lblCapitalGainsTaxUnit.TabIndex = 56;
            this.lblCapitalGainsTaxUnit.Text = "_lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTax
            // 
            this.txtBoxTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxTax, 3);
            this.txtBoxTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxTax.Enabled = false;
            this.txtBoxTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTax.Location = new System.Drawing.Point(303, 255);
            this.txtBoxTax.Name = "txtBoxTax";
            this.txtBoxTax.Size = new System.Drawing.Size(436, 23);
            this.txtBoxTax.TabIndex = 12;
            // 
            // btnAddDocumentBrowse
            // 
            this.btnAddDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddDocumentBrowse.Location = new System.Drawing.Point(745, 367);
            this.btnAddDocumentBrowse.Name = "btnAddDocumentBrowse";
            this.btnAddDocumentBrowse.Size = new System.Drawing.Size(71, 22);
            this.btnAddDocumentBrowse.TabIndex = 15;
            this.btnAddDocumentBrowse.Text = "...";
            this.btnAddDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnAddDocumentBrowse.Click += new System.EventHandler(this.BtnAddDividendDocumentBrowse_Click);
            // 
            // datePickerTime
            // 
            this.datePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(562, 3);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(177, 23);
            this.datePickerTime.TabIndex = 1;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Leave += new System.EventHandler(this.OnDatePickerTime_Leave);
            // 
            // chkBoxEnableFC
            // 
            this.chkBoxEnableFC.AutoSize = true;
            this.chkBoxEnableFC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkBoxEnableFC.Location = new System.Drawing.Point(303, 31);
            this.chkBoxEnableFC.Name = "chkBoxEnableFC";
            this.chkBoxEnableFC.Size = new System.Drawing.Size(177, 22);
            this.chkBoxEnableFC.TabIndex = 2;
            this.chkBoxEnableFC.UseVisualStyleBackColor = true;
            this.chkBoxEnableFC.CheckedChanged += new System.EventHandler(this.ChkBoxAddDividendForeignCurrency_CheckedChanged);
            // 
            // lblDividendExchangeRatio
            // 
            this.lblDividendExchangeRatio.BackColor = System.Drawing.Color.LightGray;
            this.lblDividendExchangeRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDividendExchangeRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDividendExchangeRatio.Location = new System.Drawing.Point(3, 59);
            this.lblDividendExchangeRatio.Margin = new System.Windows.Forms.Padding(3);
            this.lblDividendExchangeRatio.Name = "lblDividendExchangeRatio";
            this.lblDividendExchangeRatio.Size = new System.Drawing.Size(294, 22);
            this.lblDividendExchangeRatio.TabIndex = 31;
            this.lblDividendExchangeRatio.Text = "_addExchangeRatio";
            this.lblDividendExchangeRatio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSolidarityTax
            // 
            this.lblSolidarityTax.BackColor = System.Drawing.Color.LightGray;
            this.lblSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSolidarityTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolidarityTax.Location = new System.Drawing.Point(3, 227);
            this.lblSolidarityTax.Margin = new System.Windows.Forms.Padding(3);
            this.lblSolidarityTax.Name = "lblSolidarityTax";
            this.lblSolidarityTax.Size = new System.Drawing.Size(294, 22);
            this.lblSolidarityTax.TabIndex = 58;
            this.lblSolidarityTax.Text = "_addSolidarityTax";
            this.lblSolidarityTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCapitalGainsTax
            // 
            this.lblCapitalGainsTax.BackColor = System.Drawing.Color.LightGray;
            this.lblCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCapitalGainsTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapitalGainsTax.Location = new System.Drawing.Point(3, 199);
            this.lblCapitalGainsTax.Margin = new System.Windows.Forms.Padding(3);
            this.lblCapitalGainsTax.Name = "lblCapitalGainsTax";
            this.lblCapitalGainsTax.Size = new System.Drawing.Size(294, 22);
            this.lblCapitalGainsTax.TabIndex = 55;
            this.lblCapitalGainsTax.Text = "_addCapitalGainsTax";
            this.lblCapitalGainsTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEnableForeignCurrency
            // 
            this.lblEnableForeignCurrency.BackColor = System.Drawing.Color.LightGray;
            this.lblEnableForeignCurrency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEnableForeignCurrency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEnableForeignCurrency.Location = new System.Drawing.Point(3, 31);
            this.lblEnableForeignCurrency.Margin = new System.Windows.Forms.Padding(3);
            this.lblEnableForeignCurrency.Name = "lblEnableForeignCurrency";
            this.lblEnableForeignCurrency.Size = new System.Drawing.Size(294, 22);
            this.lblEnableForeignCurrency.TabIndex = 30;
            this.lblEnableForeignCurrency.Text = "_addEnableFC";
            this.lblEnableForeignCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxAtSource
            // 
            this.lblTaxAtSource.BackColor = System.Drawing.Color.LightGray;
            this.lblTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTaxAtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaxAtSource.Location = new System.Drawing.Point(3, 171);
            this.lblTaxAtSource.Margin = new System.Windows.Forms.Padding(3);
            this.lblTaxAtSource.Name = "lblTaxAtSource";
            this.lblTaxAtSource.Size = new System.Drawing.Size(294, 22);
            this.lblTaxAtSource.TabIndex = 52;
            this.lblTaxAtSource.Text = "_addTaxAtSource";
            this.lblTaxAtSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPayout
            // 
            this.lblPayout.BackColor = System.Drawing.Color.LightGray;
            this.lblPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayout.Location = new System.Drawing.Point(3, 143);
            this.lblPayout.Margin = new System.Windows.Forms.Padding(3);
            this.lblPayout.Name = "lblPayout";
            this.lblPayout.Size = new System.Drawing.Size(294, 22);
            this.lblPayout.TabIndex = 20;
            this.lblPayout.Text = "_addPayout";
            this.lblPayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolume.Location = new System.Drawing.Point(3, 115);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(3);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(294, 22);
            this.lblVolume.TabIndex = 5;
            this.lblVolume.Text = "_addVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDividendRate
            // 
            this.lblDividendRate.BackColor = System.Drawing.Color.LightGray;
            this.lblDividendRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDividendRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDividendRate.Location = new System.Drawing.Point(3, 87);
            this.lblDividendRate.Margin = new System.Windows.Forms.Padding(3);
            this.lblDividendRate.Name = "lblDividendRate";
            this.lblDividendRate.Size = new System.Drawing.Size(294, 22);
            this.lblDividendRate.TabIndex = 3;
            this.lblDividendRate.Text = "_addDividendRate";
            this.lblDividendRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxSolidarityTax
            // 
            this.txtBoxSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxSolidarityTax, 3);
            this.txtBoxSolidarityTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSolidarityTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSolidarityTax.Location = new System.Drawing.Point(303, 227);
            this.txtBoxSolidarityTax.Name = "txtBoxSolidarityTax";
            this.txtBoxSolidarityTax.Size = new System.Drawing.Size(436, 23);
            this.txtBoxSolidarityTax.TabIndex = 11;
            this.txtBoxSolidarityTax.TextChanged += new System.EventHandler(this.txtBoxSolidarityTax_TextChanged);
            this.txtBoxSolidarityTax.Leave += new System.EventHandler(this.txtBoxSolidarityTax_Leave);
            // 
            // txtBoxCapitalGainsTax
            // 
            this.txtBoxCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxCapitalGainsTax, 3);
            this.txtBoxCapitalGainsTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCapitalGainsTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCapitalGainsTax.Location = new System.Drawing.Point(303, 199);
            this.txtBoxCapitalGainsTax.Name = "txtBoxCapitalGainsTax";
            this.txtBoxCapitalGainsTax.Size = new System.Drawing.Size(436, 23);
            this.txtBoxCapitalGainsTax.TabIndex = 10;
            this.txtBoxCapitalGainsTax.TextChanged += new System.EventHandler(this.txtBoxCapitalGainsTax_TextChanged);
            this.txtBoxCapitalGainsTax.Leave += new System.EventHandler(this.txtBoxCapitalGainsTax_Leave);
            // 
            // txtBoxTaxAtSource
            // 
            this.txtBoxTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxTaxAtSource, 3);
            this.txtBoxTaxAtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxTaxAtSource.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTaxAtSource.Location = new System.Drawing.Point(303, 171);
            this.txtBoxTaxAtSource.Name = "txtBoxTaxAtSource";
            this.txtBoxTaxAtSource.Size = new System.Drawing.Size(436, 23);
            this.txtBoxTaxAtSource.TabIndex = 9;
            this.txtBoxTaxAtSource.TextChanged += new System.EventHandler(this.txtBoxTaxAtSource_TextChanged);
            this.txtBoxTaxAtSource.Leave += new System.EventHandler(this.txtBoxTaxAtSource_Leave);
            // 
            // txtBoxPayout
            // 
            this.txtBoxPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxPayout.Enabled = false;
            this.txtBoxPayout.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayout.Location = new System.Drawing.Point(303, 143);
            this.txtBoxPayout.Name = "txtBoxPayout";
            this.txtBoxPayout.ReadOnly = true;
            this.txtBoxPayout.Size = new System.Drawing.Size(177, 23);
            this.txtBoxPayout.TabIndex = 7;
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxVolume, 3);
            this.txtBoxVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(303, 115);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(436, 23);
            this.txtBoxVolume.TabIndex = 6;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.TxtBoxAddVolume_TextChanged);
            this.txtBoxVolume.Leave += new System.EventHandler(this.txtBoxVolume_Leave);
            // 
            // txtBoxRate
            // 
            this.txtBoxRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxRate, 3);
            this.txtBoxRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxRate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxRate.Location = new System.Drawing.Point(303, 87);
            this.txtBoxRate.Name = "txtBoxRate";
            this.txtBoxRate.Size = new System.Drawing.Size(436, 23);
            this.txtBoxRate.TabIndex = 5;
            this.txtBoxRate.TextChanged += new System.EventHandler(this.TxtBoxAddDividendRate_TextChanged);
            this.txtBoxRate.Leave += new System.EventHandler(this.txtBoxRate_Leave);
            // 
            // txtBoxExchangeRatio
            // 
            this.txtBoxExchangeRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxExchangeRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxExchangeRatio.Enabled = false;
            this.txtBoxExchangeRatio.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxExchangeRatio.Location = new System.Drawing.Point(303, 59);
            this.txtBoxExchangeRatio.Name = "txtBoxExchangeRatio";
            this.txtBoxExchangeRatio.ReadOnly = true;
            this.txtBoxExchangeRatio.Size = new System.Drawing.Size(177, 23);
            this.txtBoxExchangeRatio.TabIndex = 3;
            this.txtBoxExchangeRatio.TextChanged += new System.EventHandler(this.TxtBoxAddDividendExchangeRatio_TextChanged);
            this.txtBoxExchangeRatio.Leave += new System.EventHandler(this.txtBoxExchangeRatio_Leave);
            // 
            // cbxBoxDividendFCUnit
            // 
            this.cbxBoxDividendFCUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxBoxDividendFCUnit.DropDownHeight = 100;
            this.cbxBoxDividendFCUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBoxDividendFCUnit.Enabled = false;
            this.cbxBoxDividendFCUnit.FormattingEnabled = true;
            this.cbxBoxDividendFCUnit.IntegralHeight = false;
            this.cbxBoxDividendFCUnit.Location = new System.Drawing.Point(562, 59);
            this.cbxBoxDividendFCUnit.Name = "cbxBoxDividendFCUnit";
            this.cbxBoxDividendFCUnit.Size = new System.Drawing.Size(177, 23);
            this.cbxBoxDividendFCUnit.TabIndex = 4;
            this.cbxBoxDividendFCUnit.SelectedIndexChanged += new System.EventHandler(this.CbxBoxAddDividendForeignCurrency_SelectedIndexChanged);
            // 
            // lblTaxAtSourceUnit
            // 
            this.lblTaxAtSourceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaxAtSourceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAtSourceUnit.Location = new System.Drawing.Point(745, 171);
            this.lblTaxAtSourceUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblTaxAtSourceUnit.Name = "lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.Size = new System.Drawing.Size(71, 22);
            this.lblTaxAtSourceUnit.TabIndex = 53;
            this.lblTaxAtSourceUnit.Text = "_lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPayoutUnit
            // 
            this.lblPayoutUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayoutUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayoutUnit.Location = new System.Drawing.Point(486, 143);
            this.lblPayoutUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblPayoutUnit.Name = "lblPayoutUnit";
            this.lblPayoutUnit.Size = new System.Drawing.Size(70, 22);
            this.lblPayoutUnit.TabIndex = 22;
            this.lblPayoutUnit.Text = "_lblPayoutUnit";
            this.lblPayoutUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxPayoutFC
            // 
            this.txtBoxPayoutFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPayoutFC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxPayoutFC.Enabled = false;
            this.txtBoxPayoutFC.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayoutFC.Location = new System.Drawing.Point(562, 143);
            this.txtBoxPayoutFC.Name = "txtBoxPayoutFC";
            this.txtBoxPayoutFC.ReadOnly = true;
            this.txtBoxPayoutFC.Size = new System.Drawing.Size(177, 23);
            this.txtBoxPayoutFC.TabIndex = 8;
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolumeUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeUnit.Location = new System.Drawing.Point(745, 115);
            this.lblVolumeUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(71, 22);
            this.lblVolumeUnit.TabIndex = 14;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPayoutFCUnit
            // 
            this.lblPayoutFCUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayoutFCUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayoutFCUnit.Location = new System.Drawing.Point(745, 143);
            this.lblPayoutFCUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblPayoutFCUnit.Name = "lblPayoutFCUnit";
            this.lblPayoutFCUnit.Size = new System.Drawing.Size(71, 22);
            this.lblPayoutFCUnit.TabIndex = 49;
            this.lblPayoutFCUnit.Text = "_lblPayoutFCUnit";
            this.lblPayoutFCUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSolidarityTaxUnit
            // 
            this.lblSolidarityTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolidarityTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolidarityTaxUnit.Location = new System.Drawing.Point(745, 227);
            this.lblSolidarityTaxUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblSolidarityTaxUnit.Name = "lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.Size = new System.Drawing.Size(71, 22);
            this.lblSolidarityTaxUnit.TabIndex = 59;
            this.lblSolidarityTaxUnit.Text = "_lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDividendRateUnit
            // 
            this.lblDividendRateUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDividendRateUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDividendRateUnit.Location = new System.Drawing.Point(745, 87);
            this.lblDividendRateUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblDividendRateUnit.Name = "lblDividendRateUnit";
            this.lblDividendRateUnit.Size = new System.Drawing.Size(71, 22);
            this.lblDividendRateUnit.TabIndex = 12;
            this.lblDividendRateUnit.Text = "_lblDividendUnit";
            this.lblDividendRateUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddDocument
            // 
            this.lblAddDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblAddDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddDocument.Location = new System.Drawing.Point(3, 367);
            this.lblAddDocument.Margin = new System.Windows.Forms.Padding(3);
            this.lblAddDocument.Name = "lblAddDocument";
            this.lblAddDocument.Size = new System.Drawing.Size(294, 22);
            this.lblAddDocument.TabIndex = 25;
            this.lblAddDocument.Text = "_addDocument";
            this.lblAddDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxDocument, 3);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(303, 367);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(436, 23);
            this.txtBoxDocument.TabIndex = 14;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.txtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.txtBoxDocument_Leave);
            // 
            // lblAddPrice
            // 
            this.lblAddPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblAddPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddPrice.Location = new System.Drawing.Point(3, 339);
            this.lblAddPrice.Margin = new System.Windows.Forms.Padding(3);
            this.lblAddPrice.Name = "lblAddPrice";
            this.lblAddPrice.Size = new System.Drawing.Size(294, 22);
            this.lblAddPrice.TabIndex = 4;
            this.lblAddPrice.Text = "_addPrice";
            this.lblAddPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxPrice
            // 
            this.txtBoxPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxPrice, 3);
            this.txtBoxPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPrice.Location = new System.Drawing.Point(303, 339);
            this.txtBoxPrice.Name = "txtBoxPrice";
            this.txtBoxPrice.Size = new System.Drawing.Size(436, 23);
            this.txtBoxPrice.TabIndex = 13;
            this.txtBoxPrice.TextChanged += new System.EventHandler(this.txtBoxPrice_TextChanged);
            this.txtBoxPrice.Leave += new System.EventHandler(this.txtBoxPrice_Leave);
            // 
            // lblAddYield
            // 
            this.lblAddYield.BackColor = System.Drawing.Color.LightGray;
            this.lblAddYield.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddYield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddYield.Location = new System.Drawing.Point(3, 311);
            this.lblAddYield.Margin = new System.Windows.Forms.Padding(3);
            this.lblAddYield.Name = "lblAddYield";
            this.lblAddYield.Size = new System.Drawing.Size(294, 22);
            this.lblAddYield.TabIndex = 17;
            this.lblAddYield.Text = "_addYield";
            this.lblAddYield.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxYield
            // 
            this.txtBoxYield.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxYield, 3);
            this.txtBoxYield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxYield.Enabled = false;
            this.txtBoxYield.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxYield.Location = new System.Drawing.Point(303, 311);
            this.txtBoxYield.Name = "txtBoxYield";
            this.txtBoxYield.ReadOnly = true;
            this.txtBoxYield.Size = new System.Drawing.Size(436, 23);
            this.txtBoxYield.TabIndex = 11;
            // 
            // lblPriceUnit
            // 
            this.lblPriceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPriceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceUnit.Location = new System.Drawing.Point(745, 339);
            this.lblPriceUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblPriceUnit.Name = "lblPriceUnit";
            this.lblPriceUnit.Size = new System.Drawing.Size(71, 22);
            this.lblPriceUnit.TabIndex = 13;
            this.lblPriceUnit.Text = "_lblPriceUnit";
            this.lblPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblYieldUnit
            // 
            this.lblYieldUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblYieldUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYieldUnit.Location = new System.Drawing.Point(745, 308);
            this.lblYieldUnit.Name = "lblYieldUnit";
            this.lblYieldUnit.Size = new System.Drawing.Size(71, 28);
            this.lblYieldUnit.TabIndex = 19;
            this.lblYieldUnit.Text = "_lblYieldUnit";
            this.lblYieldUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddPayoutAfterTax
            // 
            this.lblAddPayoutAfterTax.BackColor = System.Drawing.Color.LightGray;
            this.lblAddPayoutAfterTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddPayoutAfterTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddPayoutAfterTax.Location = new System.Drawing.Point(3, 283);
            this.lblAddPayoutAfterTax.Margin = new System.Windows.Forms.Padding(3);
            this.lblAddPayoutAfterTax.Name = "lblAddPayoutAfterTax";
            this.lblAddPayoutAfterTax.Size = new System.Drawing.Size(294, 22);
            this.lblAddPayoutAfterTax.TabIndex = 38;
            this.lblAddPayoutAfterTax.Text = "_addPayoutAfterTax";
            this.lblAddPayoutAfterTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxPayoutAfterTax
            // 
            this.txtBoxPayoutAfterTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlDividendInput.SetColumnSpan(this.txtBoxPayoutAfterTax, 3);
            this.txtBoxPayoutAfterTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxPayoutAfterTax.Enabled = false;
            this.txtBoxPayoutAfterTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayoutAfterTax.Location = new System.Drawing.Point(303, 283);
            this.txtBoxPayoutAfterTax.Name = "txtBoxPayoutAfterTax";
            this.txtBoxPayoutAfterTax.ReadOnly = true;
            this.txtBoxPayoutAfterTax.Size = new System.Drawing.Size(436, 23);
            this.txtBoxPayoutAfterTax.TabIndex = 10;
            // 
            // lblTaxUnit
            // 
            this.lblTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxUnit.Location = new System.Drawing.Point(745, 252);
            this.lblTaxUnit.Name = "lblTaxUnit";
            this.lblTaxUnit.Size = new System.Drawing.Size(71, 28);
            this.lblTaxUnit.TabIndex = 48;
            this.lblTaxUnit.Text = "_lblTaxUnit";
            this.lblTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPayoutAfterTaxUnit
            // 
            this.lblPayoutAfterTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayoutAfterTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayoutAfterTaxUnit.Location = new System.Drawing.Point(745, 280);
            this.lblPayoutAfterTaxUnit.Name = "lblPayoutAfterTaxUnit";
            this.lblPayoutAfterTaxUnit.Size = new System.Drawing.Size(71, 28);
            this.lblPayoutAfterTaxUnit.TabIndex = 40;
            this.lblPayoutAfterTaxUnit.Text = "_lblPayoutAfterTaxUnit";
            this.lblPayoutAfterTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(3, 453);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
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
            // lblAddTax
            // 
            this.lblAddTax.BackColor = System.Drawing.Color.LightGray;
            this.lblAddTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddTax.Location = new System.Drawing.Point(17, 162);
            this.lblAddTax.Name = "lblAddTax";
            this.lblAddTax.Size = new System.Drawing.Size(300, 23);
            this.lblAddTax.TabIndex = 47;
            this.lblAddTax.Text = "_addTax";
            this.lblAddTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpBoxDividends
            // 
            this.grpBoxDividends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxDividends.Controls.Add(this.tblLayPnlOverviewTabControl);
            this.grpBoxDividends.Controls.Add(this.lblAddTax);
            this.grpBoxDividends.Location = new System.Drawing.Point(5, 489);
            this.grpBoxDividends.Name = "grpBoxDividends";
            this.grpBoxDividends.Size = new System.Drawing.Size(824, 162);
            this.grpBoxDividends.TabIndex = 3;
            this.grpBoxDividends.TabStop = false;
            this.grpBoxDividends.Text = "_dividends";
            // 
            // tblLayPnlOverviewTabControl
            // 
            this.tblLayPnlOverviewTabControl.ColumnCount = 1;
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOverviewTabControl.Controls.Add(this.tabCtrlDividends, 0, 0);
            this.tblLayPnlOverviewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOverviewTabControl.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlOverviewTabControl.Name = "tblLayPnlOverviewTabControl";
            this.tblLayPnlOverviewTabControl.RowCount = 1;
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 143F));
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(818, 140);
            this.tblLayPnlOverviewTabControl.TabIndex = 48;
            // 
            // tabCtrlDividends
            // 
            this.tabCtrlDividends.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlDividends.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlDividends.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlDividends.Name = "tabCtrlDividends";
            this.tabCtrlDividends.SelectedIndex = 0;
            this.tabCtrlDividends.Size = new System.Drawing.Size(812, 134);
            this.tabCtrlDividends.TabIndex = 0;
            this.tabCtrlDividends.SelectedIndexChanged += new System.EventHandler(this.tabCtrlDividends_SelectedIndexChanged);
            this.tabCtrlDividends.MouseEnter += new System.EventHandler(this.tabCtrlDividends_MouseEnter);
            this.tabCtrlDividends.MouseLeave += new System.EventHandler(this.tabCtrlDividends_MouseLeave);
            // 
            // lblAddDate
            // 
            this.lblAddDate.BackColor = System.Drawing.Color.LightGray;
            this.lblAddDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddDate.Location = new System.Drawing.Point(1, 1);
            this.lblAddDate.Name = "lblAddDate";
            this.lblAddDate.Size = new System.Drawing.Size(300, 23);
            this.lblAddDate.TabIndex = 2;
            this.lblAddDate.Text = "_addDate";
            this.lblAddDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddDividendExchangeRatio
            // 
            this.lblAddDividendExchangeRatio.BackColor = System.Drawing.Color.LightGray;
            this.lblAddDividendExchangeRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddDividendExchangeRatio.Location = new System.Drawing.Point(10, 76);
            this.lblAddDividendExchangeRatio.Name = "lblAddDividendExchangeRatio";
            this.lblAddDividendExchangeRatio.Size = new System.Drawing.Size(300, 23);
            this.lblAddDividendExchangeRatio.TabIndex = 31;
            this.lblAddDividendExchangeRatio.Text = "_addExchangeRatio";
            this.lblAddDividendExchangeRatio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddVolume
            // 
            this.lblAddVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblAddVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddVolume.Location = new System.Drawing.Point(10, 131);
            this.lblAddVolume.Name = "lblAddVolume";
            this.lblAddVolume.Size = new System.Drawing.Size(300, 23);
            this.lblAddVolume.TabIndex = 5;
            this.lblAddVolume.Text = "_addVolume";
            this.lblAddVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddTaxAtSource
            // 
            this.lblAddTaxAtSource.BackColor = System.Drawing.Color.LightGray;
            this.lblAddTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddTaxAtSource.Location = new System.Drawing.Point(1, 1);
            this.lblAddTaxAtSource.Name = "lblAddTaxAtSource";
            this.lblAddTaxAtSource.Size = new System.Drawing.Size(300, 23);
            this.lblAddTaxAtSource.TabIndex = 52;
            this.lblAddTaxAtSource.Text = "_addTaxAtSource";
            this.lblAddTaxAtSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewDividendEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 652);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxDividends);
            this.Controls.Add(this.grpBoxAddDividend);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 725);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 650);
            this.Name = "ViewDividendEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareDividendEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareDividendEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareDividendEdit_Load);
            this.Shown += new System.EventHandler(this.ShareDividendEdit_Shown);
            this.grpBoxAddDividend.ResumeLayout(false);
            this.grpBoxAddDividend.PerformLayout();
            this.tblLayPnlDividendButtons.ResumeLayout(false);
            this.tblLayPnlDividendInput.ResumeLayout(false);
            this.tblLayPnlDividendInput.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpBoxDividends.ResumeLayout(false);
            this.tblLayPnlOverviewTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddSave;
        private System.Windows.Forms.GroupBox grpBoxAddDividend;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Label lblAddPrice;
        private System.Windows.Forms.Label lblDividendRate;
        private System.Windows.Forms.DateTimePicker datePickerDate;
        private System.Windows.Forms.TextBox txtBoxVolume;
        private System.Windows.Forms.TextBox txtBoxPrice;
        private System.Windows.Forms.TextBox txtBoxRate;
        private System.Windows.Forms.GroupBox grpBoxDividends;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label lblVolumeUnit;
        private System.Windows.Forms.Label lblPriceUnit;
        private System.Windows.Forms.Label lblDividendRateUnit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblYieldUnit;
        private System.Windows.Forms.TextBox txtBoxYield;
        private System.Windows.Forms.Label lblAddYield;
        private System.Windows.Forms.TabControl tabCtrlDividends;
        private System.Windows.Forms.Label lblPayoutUnit;
        private System.Windows.Forms.TextBox txtBoxPayout;
        private System.Windows.Forms.Label lblPayout;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DateTimePicker datePickerTime;
        private System.Windows.Forms.TextBox txtBoxDocument;
        private System.Windows.Forms.Label lblAddDocument;
        private System.Windows.Forms.Button btnAddDocumentBrowse;
        private System.Windows.Forms.Label lblEnableForeignCurrency;
        private System.Windows.Forms.ComboBox cbxBoxDividendFCUnit;
        private System.Windows.Forms.CheckBox chkBoxEnableFC;
        private System.Windows.Forms.TextBox txtBoxExchangeRatio;
        private System.Windows.Forms.Label lblDividendExchangeRatio;
        private System.Windows.Forms.Label lblAddPayoutAfterTax;
        private System.Windows.Forms.TextBox txtBoxPayoutAfterTax;
        private System.Windows.Forms.Label lblPayoutAfterTaxUnit;
        private System.Windows.Forms.Label lblTaxUnit;
        private System.Windows.Forms.TextBox txtBoxTax;
        private System.Windows.Forms.Label lblAddTax;
        private System.Windows.Forms.Label lblSolidarityTaxUnit;
        private System.Windows.Forms.TextBox txtBoxSolidarityTax;
        private System.Windows.Forms.Label lblSolidarityTax;
        private System.Windows.Forms.Label lblCapitalGainsTaxUnit;
        private System.Windows.Forms.TextBox txtBoxCapitalGainsTax;
        private System.Windows.Forms.Label lblCapitalGainsTax;
        private System.Windows.Forms.Label lblTaxAtSourceUnit;
        private System.Windows.Forms.TextBox txtBoxTaxAtSource;
        private System.Windows.Forms.Label lblTaxAtSource;
        private System.Windows.Forms.TextBox txtBoxPayoutFC;
        private System.Windows.Forms.Label lblPayoutFCUnit;
        private System.Windows.Forms.Label lblAddDate;
        private System.Windows.Forms.Label lblAddDividendExchangeRatio;
        private System.Windows.Forms.Label lblAddVolume;
        private System.Windows.Forms.Label lblAddTaxAtSource;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlDividendInput;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlDividendButtons;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlOverviewTabControl;
    }
}