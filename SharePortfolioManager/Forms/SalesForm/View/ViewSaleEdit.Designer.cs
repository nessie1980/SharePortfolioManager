
using SharePortfolioManager.Properties;

namespace SharePortfolioManager.Forms.SalesForm.View
{
    partial class ViewSaleEdit
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
            this.grpBoxSales = new System.Windows.Forms.GroupBox();
            this.tblLayPnlOverviewTabControl = new System.Windows.Forms.TableLayoutPanel();
            this.tabCtrlSales = new System.Windows.Forms.TabControl();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.grpBoxAdd = new System.Windows.Forms.GroupBox();
            this.tblLayPnlSaleButtons = new System.Windows.Forms.TableLayoutPanel();
            this.tblLayPnlSaleInput = new System.Windows.Forms.TableLayoutPanel();
            this.lblAddSaleDate = new System.Windows.Forms.Label();
            this.btnSalesDocumentBrowse = new System.Windows.Forms.Button();
            this.lblPayoutUnit = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.lblSalesDocument = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.txtBoxSolidarityTax = new System.Windows.Forms.TextBox();
            this.txtBoxPayout = new System.Windows.Forms.TextBox();
            this.lblPayout = new System.Windows.Forms.Label();
            this.lblProfitLossUnit = new System.Windows.Forms.Label();
            this.lblCapitalGainsTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxProfitLoss = new System.Windows.Forms.TextBox();
            this.lblCosts = new System.Windows.Forms.Label();
            this.txtBoxCosts = new System.Windows.Forms.TextBox();
            this.lblCostsUnit = new System.Windows.Forms.Label();
            this.lblProfitLoss = new System.Windows.Forms.Label();
            this.lblSolidarityTax = new System.Windows.Forms.Label();
            this.lblSolidarityTaxUnit = new System.Windows.Forms.Label();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.lblVolume = new System.Windows.Forms.Label();
            this.txtBoxCapitalGainsTax = new System.Windows.Forms.TextBox();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.lblVolumeUnit = new System.Windows.Forms.Label();
            this.lblBuyPrice = new System.Windows.Forms.Label();
            this.lblCapitalGainsTax = new System.Windows.Forms.Label();
            this.txtBoxBuyPrice = new System.Windows.Forms.TextBox();
            this.lblBuyPriceUnit = new System.Windows.Forms.Label();
            this.lblSalePrice = new System.Windows.Forms.Label();
            this.txtBoxSalePrice = new System.Windows.Forms.TextBox();
            this.lblTaxAtSourceUnit = new System.Windows.Forms.Label();
            this.txtBoxTaxAtSource = new System.Windows.Forms.TextBox();
            this.lblSalePriceUnit = new System.Windows.Forms.Label();
            this.lblTaxAtSource = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageSaleEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxSales.SuspendLayout();
            this.tblLayPnlOverviewTabControl.SuspendLayout();
            this.grpBoxAdd.SuspendLayout();
            this.tblLayPnlSaleButtons.SuspendLayout();
            this.tblLayPnlSaleInput.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxSales
            // 
            this.grpBoxSales.Controls.Add(this.tblLayPnlOverviewTabControl);
            this.grpBoxSales.Location = new System.Drawing.Point(5, 405);
            this.grpBoxSales.Name = "grpBoxSales";
            this.grpBoxSales.Size = new System.Drawing.Size(825, 162);
            this.grpBoxSales.TabIndex = 3;
            this.grpBoxSales.TabStop = false;
            this.grpBoxSales.Text = "_sales";
            // 
            // tblLayPnlOverviewTabControl
            // 
            this.tblLayPnlOverviewTabControl.ColumnCount = 1;
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOverviewTabControl.Controls.Add(this.tabCtrlSales, 0, 0);
            this.tblLayPnlOverviewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOverviewTabControl.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlOverviewTabControl.Name = "tblLayPnlOverviewTabControl";
            this.tblLayPnlOverviewTabControl.RowCount = 1;
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 143F));
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(819, 140);
            this.tblLayPnlOverviewTabControl.TabIndex = 0;
            // 
            // tabCtrlSales
            // 
            this.tabCtrlSales.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlSales.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlSales.Name = "tabCtrlSales";
            this.tabCtrlSales.SelectedIndex = 0;
            this.tabCtrlSales.Size = new System.Drawing.Size(813, 134);
            this.tabCtrlSales.TabIndex = 0;
            this.tabCtrlSales.SelectedIndexChanged += new System.EventHandler(this.tabCtrlSales_SelectedIndexChanged);
            this.tabCtrlSales.MouseEnter += new System.EventHandler(this.tabCtrlSales_MouseEnter);
            this.tabCtrlSales.MouseLeave += new System.EventHandler(this.tabCtrlSales_MouseLeave);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = global::SharePortfolioManager.Properties.Resources.black_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(309, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(165, 33);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Image = global::SharePortfolioManager.Properties.Resources.black_cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(651, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(165, 33);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Enabled = false;
            this.btnReset.Image = global::SharePortfolioManager.Properties.Resources.black_cancel;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(480, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(165, 33);
            this.btnReset.TabIndex = 20;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // btnAddSave
            // 
            this.btnAddSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSave.Image = global::SharePortfolioManager.Properties.Resources.black_add;
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(138, 3);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(165, 33);
            this.btnAddSave.TabIndex = 18;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Controls.Add(this.tblLayPnlSaleButtons);
            this.grpBoxAdd.Controls.Add(this.tblLayPnlSaleInput);
            this.grpBoxAdd.Controls.Add(this.statusStrip1);
            this.grpBoxAdd.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(825, 394);
            this.grpBoxAdd.TabIndex = 2;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAddSales";
            // 
            // tblLayPnlSaleButtons
            // 
            this.tblLayPnlSaleButtons.ColumnCount = 5;
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tblLayPnlSaleButtons.Controls.Add(this.btnAddSave, 1, 0);
            this.tblLayPnlSaleButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tblLayPnlSaleButtons.Controls.Add(this.btnDelete, 2, 0);
            this.tblLayPnlSaleButtons.Controls.Add(this.btnReset, 3, 0);
            this.tblLayPnlSaleButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlSaleButtons.Location = new System.Drawing.Point(3, 327);
            this.tblLayPnlSaleButtons.Name = "tblLayPnlSaleButtons";
            this.tblLayPnlSaleButtons.RowCount = 1;
            this.tblLayPnlSaleButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlSaleButtons.Size = new System.Drawing.Size(819, 39);
            this.tblLayPnlSaleButtons.TabIndex = 70;
            // 
            // tblLayPnlSaleInput
            // 
            this.tblLayPnlSaleInput.ColumnCount = 5;
            this.tblLayPnlSaleInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tblLayPnlSaleInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlSaleInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tblLayPnlSaleInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlSaleInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlSaleInput.Controls.Add(this.lblAddSaleDate, 0, 0);
            this.tblLayPnlSaleInput.Controls.Add(this.btnSalesDocumentBrowse, 4, 10);
            this.tblLayPnlSaleInput.Controls.Add(this.lblPayoutUnit, 4, 9);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxDocument, 1, 10);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSalesDocument, 0, 10);
            this.tblLayPnlSaleInput.Controls.Add(this.datePickerDate, 1, 0);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxSolidarityTax, 1, 6);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxPayout, 1, 9);
            this.tblLayPnlSaleInput.Controls.Add(this.lblPayout, 0, 9);
            this.tblLayPnlSaleInput.Controls.Add(this.lblProfitLossUnit, 4, 8);
            this.tblLayPnlSaleInput.Controls.Add(this.lblCapitalGainsTaxUnit, 4, 5);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxProfitLoss, 1, 8);
            this.tblLayPnlSaleInput.Controls.Add(this.lblCosts, 0, 7);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxCosts, 1, 7);
            this.tblLayPnlSaleInput.Controls.Add(this.lblCostsUnit, 4, 7);
            this.tblLayPnlSaleInput.Controls.Add(this.lblProfitLoss, 0, 8);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSolidarityTax, 0, 6);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSolidarityTaxUnit, 4, 6);
            this.tblLayPnlSaleInput.Controls.Add(this.datePickerTime, 3, 0);
            this.tblLayPnlSaleInput.Controls.Add(this.lblVolume, 0, 1);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxCapitalGainsTax, 1, 5);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxVolume, 1, 1);
            this.tblLayPnlSaleInput.Controls.Add(this.lblVolumeUnit, 4, 1);
            this.tblLayPnlSaleInput.Controls.Add(this.lblBuyPrice, 0, 2);
            this.tblLayPnlSaleInput.Controls.Add(this.lblCapitalGainsTax, 0, 5);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxBuyPrice, 1, 2);
            this.tblLayPnlSaleInput.Controls.Add(this.lblBuyPriceUnit, 4, 2);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSalePrice, 0, 3);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxSalePrice, 1, 3);
            this.tblLayPnlSaleInput.Controls.Add(this.lblTaxAtSourceUnit, 4, 4);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxTaxAtSource, 1, 4);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSalePriceUnit, 4, 3);
            this.tblLayPnlSaleInput.Controls.Add(this.lblTaxAtSource, 0, 4);
            this.tblLayPnlSaleInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlSaleInput.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlSaleInput.Name = "tblLayPnlSaleInput";
            this.tblLayPnlSaleInput.RowCount = 11;
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblLayPnlSaleInput.Size = new System.Drawing.Size(819, 308);
            this.tblLayPnlSaleInput.TabIndex = 69;
            // 
            // lblAddSaleDate
            // 
            this.lblAddSaleDate.BackColor = System.Drawing.Color.LightGray;
            this.lblAddSaleDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddSaleDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddSaleDate.Location = new System.Drawing.Point(3, 3);
            this.lblAddSaleDate.Margin = new System.Windows.Forms.Padding(3);
            this.lblAddSaleDate.Name = "lblAddSaleDate";
            this.lblAddSaleDate.Size = new System.Drawing.Size(204, 22);
            this.lblAddSaleDate.TabIndex = 2;
            this.lblAddSaleDate.Text = "_addDate";
            this.lblAddSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSalesDocumentBrowse
            // 
            this.btnSalesDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSalesDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalesDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.Location = new System.Drawing.Point(745, 283);
            this.btnSalesDocumentBrowse.Name = "btnSalesDocumentBrowse";
            this.btnSalesDocumentBrowse.Size = new System.Drawing.Size(71, 22);
            this.btnSalesDocumentBrowse.TabIndex = 17;
            this.btnSalesDocumentBrowse.Text = "...";
            this.btnSalesDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnSalesDocumentBrowse.Click += new System.EventHandler(this.OnBtnSaleDocumentBrowse_Click);
            // 
            // lblPayoutUnit
            // 
            this.lblPayoutUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayoutUnit.Location = new System.Drawing.Point(745, 255);
            this.lblPayoutUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblPayoutUnit.Name = "lblPayoutUnit";
            this.lblPayoutUnit.Size = new System.Drawing.Size(71, 22);
            this.lblPayoutUnit.TabIndex = 68;
            this.lblPayoutUnit.Text = "_lblPayoutUnit";
            this.lblPayoutUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxDocument, 3);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(213, 283);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(526, 23);
            this.txtBoxDocument.TabIndex = 16;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // lblSalesDocument
            // 
            this.lblSalesDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalesDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesDocument.Location = new System.Drawing.Point(3, 283);
            this.lblSalesDocument.Margin = new System.Windows.Forms.Padding(3);
            this.lblSalesDocument.Name = "lblSalesDocument";
            this.lblSalesDocument.Size = new System.Drawing.Size(204, 22);
            this.lblSalesDocument.TabIndex = 25;
            this.lblSalesDocument.Text = "_salesShareDocument";
            this.lblSalesDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(213, 3);
            this.datePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(222, 23);
            this.datePickerDate.TabIndex = 0;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.datePickerDate.Leave += new System.EventHandler(this.OnDatePickerDate_Leave);
            // 
            // txtBoxSolidarityTax
            // 
            this.txtBoxSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxSolidarityTax, 3);
            this.txtBoxSolidarityTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSolidarityTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSolidarityTax.Location = new System.Drawing.Point(213, 171);
            this.txtBoxSolidarityTax.Name = "txtBoxSolidarityTax";
            this.txtBoxSolidarityTax.Size = new System.Drawing.Size(526, 23);
            this.txtBoxSolidarityTax.TabIndex = 64;
            this.txtBoxSolidarityTax.TextChanged += new System.EventHandler(this.OnTxtBoxSolidarityTax_TextChanged);
            this.txtBoxSolidarityTax.Leave += new System.EventHandler(this.OnTxtBoxSolidarityTaxLeave);
            // 
            // txtBoxPayout
            // 
            this.txtBoxPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxPayout, 3);
            this.txtBoxPayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxPayout.Enabled = false;
            this.txtBoxPayout.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayout.Location = new System.Drawing.Point(213, 255);
            this.txtBoxPayout.Name = "txtBoxPayout";
            this.txtBoxPayout.ReadOnly = true;
            this.txtBoxPayout.Size = new System.Drawing.Size(526, 23);
            this.txtBoxPayout.TabIndex = 10;
            // 
            // lblPayout
            // 
            this.lblPayout.BackColor = System.Drawing.Color.LightGray;
            this.lblPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayout.Location = new System.Drawing.Point(3, 255);
            this.lblPayout.Margin = new System.Windows.Forms.Padding(3);
            this.lblPayout.Name = "lblPayout";
            this.lblPayout.Size = new System.Drawing.Size(204, 22);
            this.lblPayout.TabIndex = 62;
            this.lblPayout.Text = "_addPayout";
            this.lblPayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProfitLossUnit
            // 
            this.lblProfitLossUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfitLossUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfitLossUnit.Location = new System.Drawing.Point(745, 227);
            this.lblProfitLossUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblProfitLossUnit.Name = "lblProfitLossUnit";
            this.lblProfitLossUnit.Size = new System.Drawing.Size(71, 22);
            this.lblProfitLossUnit.TabIndex = 67;
            this.lblProfitLossUnit.Text = "_lblProfitLossUnit";
            this.lblProfitLossUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCapitalGainsTaxUnit
            // 
            this.lblCapitalGainsTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapitalGainsTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapitalGainsTaxUnit.Location = new System.Drawing.Point(745, 143);
            this.lblCapitalGainsTaxUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblCapitalGainsTaxUnit.Name = "lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.Size = new System.Drawing.Size(71, 22);
            this.lblCapitalGainsTaxUnit.TabIndex = 66;
            this.lblCapitalGainsTaxUnit.Text = "_lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxProfitLoss
            // 
            this.txtBoxProfitLoss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxProfitLoss, 3);
            this.txtBoxProfitLoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxProfitLoss.Enabled = false;
            this.txtBoxProfitLoss.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxProfitLoss.Location = new System.Drawing.Point(213, 227);
            this.txtBoxProfitLoss.Name = "txtBoxProfitLoss";
            this.txtBoxProfitLoss.ReadOnly = true;
            this.txtBoxProfitLoss.Size = new System.Drawing.Size(526, 23);
            this.txtBoxProfitLoss.TabIndex = 65;
            // 
            // lblCosts
            // 
            this.lblCosts.BackColor = System.Drawing.Color.LightGray;
            this.lblCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCosts.Location = new System.Drawing.Point(3, 199);
            this.lblCosts.Margin = new System.Windows.Forms.Padding(3);
            this.lblCosts.Name = "lblCosts";
            this.lblCosts.Size = new System.Drawing.Size(204, 22);
            this.lblCosts.TabIndex = 16;
            this.lblCosts.Text = "_addCosts";
            this.lblCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxCosts
            // 
            this.txtBoxCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxCosts, 3);
            this.txtBoxCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCosts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCosts.Location = new System.Drawing.Point(213, 199);
            this.txtBoxCosts.Name = "txtBoxCosts";
            this.txtBoxCosts.Size = new System.Drawing.Size(526, 23);
            this.txtBoxCosts.TabIndex = 14;
            this.txtBoxCosts.TextChanged += new System.EventHandler(this.OnTxtBoxCosts_TextChanged);
            this.txtBoxCosts.Leave += new System.EventHandler(this.OnTxtBoxCostsTaxLeave);
            // 
            // lblCostsUnit
            // 
            this.lblCostsUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCostsUnit.Location = new System.Drawing.Point(745, 199);
            this.lblCostsUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblCostsUnit.Name = "lblCostsUnit";
            this.lblCostsUnit.Size = new System.Drawing.Size(71, 22);
            this.lblCostsUnit.TabIndex = 24;
            this.lblCostsUnit.Text = "_lblCostsUnit";
            this.lblCostsUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProfitLoss
            // 
            this.lblProfitLoss.BackColor = System.Drawing.Color.LightGray;
            this.lblProfitLoss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProfitLoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfitLoss.Location = new System.Drawing.Point(3, 227);
            this.lblProfitLoss.Margin = new System.Windows.Forms.Padding(3);
            this.lblProfitLoss.Name = "lblProfitLoss";
            this.lblProfitLoss.Size = new System.Drawing.Size(204, 22);
            this.lblProfitLoss.TabIndex = 44;
            this.lblProfitLoss.Text = "_addProfitLoss";
            this.lblProfitLoss.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSolidarityTax
            // 
            this.lblSolidarityTax.BackColor = System.Drawing.Color.LightGray;
            this.lblSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSolidarityTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolidarityTax.Location = new System.Drawing.Point(3, 171);
            this.lblSolidarityTax.Margin = new System.Windows.Forms.Padding(3);
            this.lblSolidarityTax.Name = "lblSolidarityTax";
            this.lblSolidarityTax.Size = new System.Drawing.Size(204, 22);
            this.lblSolidarityTax.TabIndex = 60;
            this.lblSolidarityTax.Text = "_addSolidarityTax";
            this.lblSolidarityTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSolidarityTaxUnit
            // 
            this.lblSolidarityTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolidarityTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolidarityTaxUnit.Location = new System.Drawing.Point(745, 171);
            this.lblSolidarityTaxUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblSolidarityTaxUnit.Name = "lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.Size = new System.Drawing.Size(71, 22);
            this.lblSolidarityTaxUnit.TabIndex = 50;
            this.lblSolidarityTaxUnit.Text = "_lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datePickerTime
            // 
            this.datePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(517, 3);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(222, 23);
            this.datePickerTime.TabIndex = 2;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Leave += new System.EventHandler(this.OnDatePickerTime_Leave);
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolume.Location = new System.Drawing.Point(3, 31);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(3);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(204, 22);
            this.lblVolume.TabIndex = 15;
            this.lblVolume.Text = "_addVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxCapitalGainsTax
            // 
            this.txtBoxCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxCapitalGainsTax, 3);
            this.txtBoxCapitalGainsTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCapitalGainsTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCapitalGainsTax.Location = new System.Drawing.Point(213, 143);
            this.txtBoxCapitalGainsTax.Name = "txtBoxCapitalGainsTax";
            this.txtBoxCapitalGainsTax.Size = new System.Drawing.Size(526, 23);
            this.txtBoxCapitalGainsTax.TabIndex = 63;
            this.txtBoxCapitalGainsTax.TextChanged += new System.EventHandler(this.OnTxtBoxCapitalGainsTax_TextChanged);
            this.txtBoxCapitalGainsTax.Leave += new System.EventHandler(this.OnTxtBoxCapitalGAinsTaxLeave);
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxVolume, 3);
            this.txtBoxVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(213, 31);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(526, 23);
            this.txtBoxVolume.TabIndex = 6;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolume_TextChanged);
            this.txtBoxVolume.Leave += new System.EventHandler(this.OnTxtBoxVolume_Leave);
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolumeUnit.Location = new System.Drawing.Point(745, 31);
            this.lblVolumeUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(71, 22);
            this.lblVolumeUnit.TabIndex = 17;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBuyPrice
            // 
            this.lblBuyPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblBuyPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBuyPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBuyPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuyPrice.Location = new System.Drawing.Point(3, 59);
            this.lblBuyPrice.Margin = new System.Windows.Forms.Padding(3);
            this.lblBuyPrice.Name = "lblBuyPrice";
            this.lblBuyPrice.Size = new System.Drawing.Size(204, 22);
            this.lblBuyPrice.TabIndex = 52;
            this.lblBuyPrice.Text = "_addBuyPrice";
            this.lblBuyPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCapitalGainsTax
            // 
            this.lblCapitalGainsTax.BackColor = System.Drawing.Color.LightGray;
            this.lblCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCapitalGainsTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapitalGainsTax.Location = new System.Drawing.Point(3, 143);
            this.lblCapitalGainsTax.Margin = new System.Windows.Forms.Padding(3);
            this.lblCapitalGainsTax.Name = "lblCapitalGainsTax";
            this.lblCapitalGainsTax.Size = new System.Drawing.Size(204, 22);
            this.lblCapitalGainsTax.TabIndex = 59;
            this.lblCapitalGainsTax.Text = "_addCaptialGainsTax";
            this.lblCapitalGainsTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxBuyPrice
            // 
            this.txtBoxBuyPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxBuyPrice, 3);
            this.txtBoxBuyPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBuyPrice.Enabled = false;
            this.txtBoxBuyPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBuyPrice.Location = new System.Drawing.Point(213, 59);
            this.txtBoxBuyPrice.Name = "txtBoxBuyPrice";
            this.txtBoxBuyPrice.ReadOnly = true;
            this.txtBoxBuyPrice.Size = new System.Drawing.Size(526, 23);
            this.txtBoxBuyPrice.TabIndex = 7;
            this.txtBoxBuyPrice.TextChanged += new System.EventHandler(this.OnTxtBoxAddBuyPrice_TextChanged);
            this.txtBoxBuyPrice.Leave += new System.EventHandler(this.OnTxtBoxBuyPrice_Leave);
            // 
            // lblBuyPriceUnit
            // 
            this.lblBuyPriceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBuyPriceUnit.Location = new System.Drawing.Point(745, 59);
            this.lblBuyPriceUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblBuyPriceUnit.Name = "lblBuyPriceUnit";
            this.lblBuyPriceUnit.Size = new System.Drawing.Size(71, 22);
            this.lblBuyPriceUnit.TabIndex = 54;
            this.lblBuyPriceUnit.Text = "_lblBuyPriceUnit";
            this.lblBuyPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSalePrice
            // 
            this.lblSalePrice.BackColor = System.Drawing.Color.LightGray;
            this.lblSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalePrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalePrice.Location = new System.Drawing.Point(3, 87);
            this.lblSalePrice.Margin = new System.Windows.Forms.Padding(3);
            this.lblSalePrice.Name = "lblSalePrice";
            this.lblSalePrice.Size = new System.Drawing.Size(204, 22);
            this.lblSalePrice.TabIndex = 26;
            this.lblSalePrice.Text = "_addSalePrice";
            this.lblSalePrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxSalePrice
            // 
            this.txtBoxSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxSalePrice, 3);
            this.txtBoxSalePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSalePrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSalePrice.Location = new System.Drawing.Point(213, 87);
            this.txtBoxSalePrice.Name = "txtBoxSalePrice";
            this.txtBoxSalePrice.Size = new System.Drawing.Size(526, 23);
            this.txtBoxSalePrice.TabIndex = 8;
            this.txtBoxSalePrice.TextChanged += new System.EventHandler(this.OnTxtBoxSalePrice_TextChanged);
            this.txtBoxSalePrice.Leave += new System.EventHandler(this.OnTxtBoxSalePrice_Leave);
            // 
            // lblTaxAtSourceUnit
            // 
            this.lblTaxAtSourceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaxAtSourceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAtSourceUnit.Location = new System.Drawing.Point(745, 115);
            this.lblTaxAtSourceUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblTaxAtSourceUnit.Name = "lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.Size = new System.Drawing.Size(71, 22);
            this.lblTaxAtSourceUnit.TabIndex = 45;
            this.lblTaxAtSourceUnit.Text = "_lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTaxAtSource
            // 
            this.txtBoxTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxTaxAtSource, 3);
            this.txtBoxTaxAtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxTaxAtSource.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTaxAtSource.Location = new System.Drawing.Point(213, 115);
            this.txtBoxTaxAtSource.Name = "txtBoxTaxAtSource";
            this.txtBoxTaxAtSource.Size = new System.Drawing.Size(526, 23);
            this.txtBoxTaxAtSource.TabIndex = 12;
            this.txtBoxTaxAtSource.TextChanged += new System.EventHandler(this.OnTxtBoxTaxAtSource_TextChanged);
            this.txtBoxTaxAtSource.Leave += new System.EventHandler(this.OnTxtBoxTaxAtSourceLeave);
            // 
            // lblSalePriceUnit
            // 
            this.lblSalePriceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalePriceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalePriceUnit.Location = new System.Drawing.Point(745, 87);
            this.lblSalePriceUnit.Margin = new System.Windows.Forms.Padding(3);
            this.lblSalePriceUnit.Name = "lblSalePriceUnit";
            this.lblSalePriceUnit.Size = new System.Drawing.Size(71, 22);
            this.lblSalePriceUnit.TabIndex = 39;
            this.lblSalePriceUnit.Text = "_lblSalePriceUnit";
            this.lblSalePriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTaxAtSource
            // 
            this.lblTaxAtSource.BackColor = System.Drawing.Color.LightGray;
            this.lblTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTaxAtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaxAtSource.Location = new System.Drawing.Point(3, 115);
            this.lblTaxAtSource.Margin = new System.Windows.Forms.Padding(3);
            this.lblTaxAtSource.Name = "lblTaxAtSource";
            this.lblTaxAtSource.Size = new System.Drawing.Size(204, 22);
            this.lblTaxAtSource.TabIndex = 49;
            this.lblTaxAtSource.Text = "_addTaxAtSource";
            this.lblTaxAtSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageSaleEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 369);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessageSaleEdit
            // 
            this.toolStripStatusLabelMessageSaleEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageSaleEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessageSaleEdit.Name = "toolStripStatusLabelMessageSaleEdit";
            this.toolStripStatusLabelMessageSaleEdit.Size = new System.Drawing.Size(0, 17);
            // 
            // ViewSaleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 584);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxSales);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 650);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ViewSaleEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareSalesEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareSalesEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareSalesEdit_Load);
            this.grpBoxSales.ResumeLayout(false);
            this.tblLayPnlOverviewTabControl.ResumeLayout(false);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
            this.tblLayPnlSaleButtons.ResumeLayout(false);
            this.tblLayPnlSaleInput.ResumeLayout(false);
            this.tblLayPnlSaleInput.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxSales;
        private System.Windows.Forms.TabControl tabCtrlSales;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnAddSave;
        private System.Windows.Forms.GroupBox grpBoxAdd;
        private System.Windows.Forms.DateTimePicker datePickerTime;
        private System.Windows.Forms.TextBox txtBoxSalePrice;
        private System.Windows.Forms.Label lblSalePrice;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessageSaleEdit;
        private System.Windows.Forms.Label lblCostsUnit;
        private System.Windows.Forms.TextBox txtBoxCosts;
        private System.Windows.Forms.Label lblCosts;
        private System.Windows.Forms.Button btnSalesDocumentBrowse;
        private System.Windows.Forms.TextBox txtBoxDocument;
        private System.Windows.Forms.Label lblSalesDocument;
        private System.Windows.Forms.Label lblVolumeUnit;
        private System.Windows.Forms.TextBox txtBoxVolume;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.DateTimePicker datePickerDate;
        private System.Windows.Forms.Label lblAddSaleDate;
        private System.Windows.Forms.Label lblSalePriceUnit;
        private System.Windows.Forms.Label lblTaxAtSourceUnit;
        private System.Windows.Forms.TextBox txtBoxPayout;
        private System.Windows.Forms.Label lblProfitLoss;
        private System.Windows.Forms.Label lblSolidarityTaxUnit;
        private System.Windows.Forms.TextBox txtBoxTaxAtSource;
        private System.Windows.Forms.Label lblTaxAtSource;
        private System.Windows.Forms.Label lblBuyPrice;
        private System.Windows.Forms.Label lblBuyPriceUnit;
        private System.Windows.Forms.TextBox txtBoxBuyPrice;
        private System.Windows.Forms.Label lblCapitalGainsTax;
        private System.Windows.Forms.Label lblSolidarityTax;
        private System.Windows.Forms.Label lblPayout;
        private System.Windows.Forms.TextBox txtBoxProfitLoss;
        private System.Windows.Forms.TextBox txtBoxSolidarityTax;
        private System.Windows.Forms.TextBox txtBoxCapitalGainsTax;
        private System.Windows.Forms.Label lblCapitalGainsTaxUnit;
        private System.Windows.Forms.Label lblProfitLossUnit;
        private System.Windows.Forms.Label lblPayoutUnit;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlSaleInput;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlSaleButtons;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlOverviewTabControl;
    }
}