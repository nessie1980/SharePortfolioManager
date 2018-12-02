
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
            this.lblTaxAtSourceUnit = new System.Windows.Forms.Label();
            this.txtBoxTaxAtSource = new System.Windows.Forms.TextBox();
            this.lblTaxAtSource = new System.Windows.Forms.Label();
            this.lblBuyValue = new System.Windows.Forms.Label();
            this.txtBoxSalePrice = new System.Windows.Forms.TextBox();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.lblVolume = new System.Windows.Forms.Label();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.lblVolumeUnit = new System.Windows.Forms.Label();
            this.lblSalesDocument = new System.Windows.Forms.Label();
            this.lblPayout = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.txtBoxPayout = new System.Windows.Forms.TextBox();
            this.btnSalesDocumentBrowse = new System.Windows.Forms.Button();
            this.lblPayoutUnit = new System.Windows.Forms.Label();
            this.lblSalePrice = new System.Windows.Forms.Label();
            this.lblSalePriceUnit = new System.Windows.Forms.Label();
            this.btnSalesBuyDetails = new System.Windows.Forms.Button();
            this.txtBoxSaleBuyValue = new System.Windows.Forms.TextBox();
            this.lblProfitLoss = new System.Windows.Forms.Label();
            this.lblReduction = new System.Windows.Forms.Label();
            this.lblBrokerage = new System.Windows.Forms.Label();
            this.lblSolidarityTax = new System.Windows.Forms.Label();
            this.lblCapitalGainsTax = new System.Windows.Forms.Label();
            this.txtBoxProfitLoss = new System.Windows.Forms.TextBox();
            this.txtBoxReduction = new System.Windows.Forms.TextBox();
            this.txtBoxBrokerage = new System.Windows.Forms.TextBox();
            this.txtBoxSolidarityTax = new System.Windows.Forms.TextBox();
            this.txtBoxCapitalGainsTax = new System.Windows.Forms.TextBox();
            this.lblReductionUnit = new System.Windows.Forms.Label();
            this.lblBrokerageUnit = new System.Windows.Forms.Label();
            this.lblSolidarityTaxUnit = new System.Windows.Forms.Label();
            this.lblCapitalGainsTaxUnit = new System.Windows.Forms.Label();
            this.lblProfitLossUnit = new System.Windows.Forms.Label();
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
            this.grpBoxSales.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxSales.Location = new System.Drawing.Point(5, 380);
            this.grpBoxSales.Name = "grpBoxSales";
            this.grpBoxSales.Size = new System.Drawing.Size(825, 174);
            this.grpBoxSales.TabIndex = 3;
            this.grpBoxSales.TabStop = false;
            this.grpBoxSales.Text = "_grpBoxSales";
            // 
            // tblLayPnlOverviewTabControl
            // 
            this.tblLayPnlOverviewTabControl.ColumnCount = 1;
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOverviewTabControl.Controls.Add(this.tabCtrlSales, 0, 0);
            this.tblLayPnlOverviewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOverviewTabControl.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblLayPnlOverviewTabControl.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlOverviewTabControl.Name = "tblLayPnlOverviewTabControl";
            this.tblLayPnlOverviewTabControl.RowCount = 1;
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(819, 153);
            this.tblLayPnlOverviewTabControl.TabIndex = 0;
            // 
            // tabCtrlSales
            // 
            this.tabCtrlSales.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlSales.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrlSales.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlSales.Name = "tabCtrlSales";
            this.tabCtrlSales.SelectedIndex = 0;
            this.tabCtrlSales.Size = new System.Drawing.Size(813, 147);
            this.tabCtrlSales.TabIndex = 0;
            this.tabCtrlSales.SelectedIndexChanged += new System.EventHandler(this.TabCtrlSales_SelectedIndexChanged);
            this.tabCtrlSales.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTabCtrlSales_KeyDown);
            this.tabCtrlSales.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTabCtrlSales_KeyPress);
            this.tabCtrlSales.MouseLeave += new System.EventHandler(this.TabCtrlSales_MouseLeave);
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
            this.btnDelete.Size = new System.Drawing.Size(178, 34);
            this.btnDelete.TabIndex = 12;
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
            this.btnCancel.Size = new System.Drawing.Size(178, 34);
            this.btnCancel.TabIndex = 14;
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
            this.btnReset.Size = new System.Drawing.Size(178, 34);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // btnAddSave
            // 
            this.btnAddSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSave.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(100, 1);
            this.btnAddSave.Margin = new System.Windows.Forms.Padding(1);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(178, 34);
            this.btnAddSave.TabIndex = 11;
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
            this.grpBoxAdd.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxAdd.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(825, 369);
            this.grpBoxAdd.TabIndex = 2;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAddSales";
            // 
            // tblLayPnlSaleButtons
            // 
            this.tblLayPnlSaleButtons.ColumnCount = 5;
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlSaleButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlSaleButtons.Controls.Add(this.btnAddSave, 1, 0);
            this.tblLayPnlSaleButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tblLayPnlSaleButtons.Controls.Add(this.btnDelete, 2, 0);
            this.tblLayPnlSaleButtons.Controls.Add(this.btnReset, 3, 0);
            this.tblLayPnlSaleButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlSaleButtons.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblLayPnlSaleButtons.Location = new System.Drawing.Point(3, 306);
            this.tblLayPnlSaleButtons.Name = "tblLayPnlSaleButtons";
            this.tblLayPnlSaleButtons.RowCount = 1;
            this.tblLayPnlSaleButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlSaleButtons.Size = new System.Drawing.Size(819, 36);
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
            this.tblLayPnlSaleInput.Controls.Add(this.lblTaxAtSourceUnit, 4, 5);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxTaxAtSource, 1, 5);
            this.tblLayPnlSaleInput.Controls.Add(this.lblTaxAtSource, 0, 5);
            this.tblLayPnlSaleInput.Controls.Add(this.lblBuyValue, 0, 3);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxSalePrice, 1, 2);
            this.tblLayPnlSaleInput.Controls.Add(this.datePickerDate, 1, 0);
            this.tblLayPnlSaleInput.Controls.Add(this.datePickerTime, 3, 0);
            this.tblLayPnlSaleInput.Controls.Add(this.lblVolume, 0, 1);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxVolume, 1, 1);
            this.tblLayPnlSaleInput.Controls.Add(this.lblVolumeUnit, 4, 1);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSalesDocument, 0, 11);
            this.tblLayPnlSaleInput.Controls.Add(this.lblPayout, 0, 10);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxDocument, 1, 11);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxPayout, 1, 10);
            this.tblLayPnlSaleInput.Controls.Add(this.btnSalesDocumentBrowse, 4, 11);
            this.tblLayPnlSaleInput.Controls.Add(this.lblPayoutUnit, 4, 10);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSalePrice, 0, 2);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSalePriceUnit, 4, 2);
            this.tblLayPnlSaleInput.Controls.Add(this.btnSalesBuyDetails, 4, 3);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxSaleBuyValue, 1, 3);
            this.tblLayPnlSaleInput.Controls.Add(this.lblProfitLoss, 0, 4);
            this.tblLayPnlSaleInput.Controls.Add(this.lblReduction, 0, 9);
            this.tblLayPnlSaleInput.Controls.Add(this.lblBrokerage, 0, 8);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSolidarityTax, 0, 7);
            this.tblLayPnlSaleInput.Controls.Add(this.lblCapitalGainsTax, 0, 6);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxProfitLoss, 1, 4);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxReduction, 1, 9);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxBrokerage, 1, 8);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxSolidarityTax, 1, 7);
            this.tblLayPnlSaleInput.Controls.Add(this.txtBoxCapitalGainsTax, 1, 6);
            this.tblLayPnlSaleInput.Controls.Add(this.lblReductionUnit, 4, 9);
            this.tblLayPnlSaleInput.Controls.Add(this.lblBrokerageUnit, 4, 8);
            this.tblLayPnlSaleInput.Controls.Add(this.lblSolidarityTaxUnit, 4, 7);
            this.tblLayPnlSaleInput.Controls.Add(this.lblCapitalGainsTaxUnit, 4, 6);
            this.tblLayPnlSaleInput.Controls.Add(this.lblProfitLossUnit, 4, 4);
            this.tblLayPnlSaleInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlSaleInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblLayPnlSaleInput.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlSaleInput.Name = "tblLayPnlSaleInput";
            this.tblLayPnlSaleInput.RowCount = 12;
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlSaleInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlSaleInput.Size = new System.Drawing.Size(819, 288);
            this.tblLayPnlSaleInput.TabIndex = 69;
            // 
            // lblAddSaleDate
            // 
            this.lblAddSaleDate.BackColor = System.Drawing.Color.LightGray;
            this.lblAddSaleDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddSaleDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddSaleDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddSaleDate.Location = new System.Drawing.Point(1, 1);
            this.lblAddSaleDate.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddSaleDate.Name = "lblAddSaleDate";
            this.lblAddSaleDate.Size = new System.Drawing.Size(208, 22);
            this.lblAddSaleDate.TabIndex = 2;
            this.lblAddSaleDate.Text = "_addDate";
            this.lblAddSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxAtSourceUnit
            // 
            this.lblTaxAtSourceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaxAtSourceUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAtSourceUnit.Location = new System.Drawing.Point(743, 121);
            this.lblTaxAtSourceUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblTaxAtSourceUnit.Name = "lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.Size = new System.Drawing.Size(75, 22);
            this.lblTaxAtSourceUnit.TabIndex = 45;
            this.lblTaxAtSourceUnit.Text = "_lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTaxAtSource
            // 
            this.txtBoxTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxTaxAtSource, 3);
            this.txtBoxTaxAtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxTaxAtSource.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTaxAtSource.Location = new System.Drawing.Point(211, 121);
            this.txtBoxTaxAtSource.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxTaxAtSource.Name = "txtBoxTaxAtSource";
            this.txtBoxTaxAtSource.Size = new System.Drawing.Size(530, 22);
            this.txtBoxTaxAtSource.TabIndex = 4;
            this.txtBoxTaxAtSource.TextChanged += new System.EventHandler(this.OnTxtBoxTaxAtSource_TextChanged);
            this.txtBoxTaxAtSource.Enter += new System.EventHandler(this.OnTxtBoxTaxAtSource_Enter);
            this.txtBoxTaxAtSource.Leave += new System.EventHandler(this.OnTxtBoxTaxAtSourceLeave);
            // 
            // lblTaxAtSource
            // 
            this.lblTaxAtSource.BackColor = System.Drawing.Color.LightGray;
            this.lblTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTaxAtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaxAtSource.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAtSource.Location = new System.Drawing.Point(1, 121);
            this.lblTaxAtSource.Margin = new System.Windows.Forms.Padding(1);
            this.lblTaxAtSource.Name = "lblTaxAtSource";
            this.lblTaxAtSource.Size = new System.Drawing.Size(208, 22);
            this.lblTaxAtSource.TabIndex = 49;
            this.lblTaxAtSource.Text = "_addTaxAtSource";
            this.lblTaxAtSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBuyValue
            // 
            this.lblBuyValue.BackColor = System.Drawing.Color.LightGray;
            this.lblBuyValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBuyValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuyValue.Location = new System.Drawing.Point(1, 73);
            this.lblBuyValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblBuyValue.Name = "lblBuyValue";
            this.lblBuyValue.Size = new System.Drawing.Size(208, 22);
            this.lblBuyValue.TabIndex = 52;
            this.lblBuyValue.Text = "_addBuyValue";
            this.lblBuyValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxSalePrice
            // 
            this.txtBoxSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxSalePrice, 3);
            this.txtBoxSalePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSalePrice.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSalePrice.Location = new System.Drawing.Point(211, 49);
            this.txtBoxSalePrice.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxSalePrice.Name = "txtBoxSalePrice";
            this.txtBoxSalePrice.Size = new System.Drawing.Size(530, 22);
            this.txtBoxSalePrice.TabIndex = 3;
            this.txtBoxSalePrice.TextChanged += new System.EventHandler(this.OnTxtBoxSalePrice_TextChanged);
            this.txtBoxSalePrice.Enter += new System.EventHandler(this.OnTxtBoxSalePrice_Enter);
            this.txtBoxSalePrice.Leave += new System.EventHandler(this.OnTxtBoxSalePrice_Leave);
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(211, 1);
            this.datePickerDate.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(226, 22);
            this.datePickerDate.TabIndex = 0;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.datePickerDate.Enter += new System.EventHandler(this.OnDatePickerDate_Enter);
            this.datePickerDate.Leave += new System.EventHandler(this.OnDatePickerDate_Leave);
            // 
            // datePickerTime
            // 
            this.datePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerTime.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(515, 1);
            this.datePickerTime.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(226, 22);
            this.datePickerTime.TabIndex = 1;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Enter += new System.EventHandler(this.OnDatePickerTime_Enter);
            this.datePickerTime.Leave += new System.EventHandler(this.OnDatePickerTime_Leave);
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.Location = new System.Drawing.Point(1, 25);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(208, 22);
            this.lblVolume.TabIndex = 15;
            this.lblVolume.Text = "_addVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxVolume, 3);
            this.txtBoxVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(211, 25);
            this.txtBoxVolume.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(530, 22);
            this.txtBoxVolume.TabIndex = 2;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolume_TextChanged);
            this.txtBoxVolume.Enter += new System.EventHandler(this.OnTxtBoxVolume_Enter);
            this.txtBoxVolume.Leave += new System.EventHandler(this.OnTxtBoxVolume_Leave);
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolumeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeUnit.Location = new System.Drawing.Point(743, 25);
            this.lblVolumeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblVolumeUnit.TabIndex = 17;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSalesDocument
            // 
            this.lblSalesDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalesDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesDocument.Location = new System.Drawing.Point(1, 265);
            this.lblSalesDocument.Margin = new System.Windows.Forms.Padding(1);
            this.lblSalesDocument.Name = "lblSalesDocument";
            this.lblSalesDocument.Size = new System.Drawing.Size(208, 22);
            this.lblSalesDocument.TabIndex = 25;
            this.lblSalesDocument.Text = "_salesShareDocument";
            this.lblSalesDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPayout
            // 
            this.lblPayout.BackColor = System.Drawing.Color.LightGray;
            this.lblPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayout.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayout.Location = new System.Drawing.Point(1, 241);
            this.lblPayout.Margin = new System.Windows.Forms.Padding(1);
            this.lblPayout.Name = "lblPayout";
            this.lblPayout.Size = new System.Drawing.Size(208, 22);
            this.lblPayout.TabIndex = 62;
            this.lblPayout.Text = "_addPayout";
            this.lblPayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxDocument, 3);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(211, 265);
            this.txtBoxDocument.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(530, 22);
            this.txtBoxDocument.TabIndex = 9;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Enter += new System.EventHandler(this.txtBoxDocument_Enter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // txtBoxPayout
            // 
            this.txtBoxPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxPayout, 3);
            this.txtBoxPayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxPayout.Enabled = false;
            this.txtBoxPayout.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayout.Location = new System.Drawing.Point(211, 241);
            this.txtBoxPayout.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxPayout.Name = "txtBoxPayout";
            this.txtBoxPayout.ReadOnly = true;
            this.txtBoxPayout.Size = new System.Drawing.Size(530, 22);
            this.txtBoxPayout.TabIndex = 10;
            // 
            // btnSalesDocumentBrowse
            // 
            this.btnSalesDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSalesDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalesDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.Location = new System.Drawing.Point(743, 265);
            this.btnSalesDocumentBrowse.Margin = new System.Windows.Forms.Padding(1);
            this.btnSalesDocumentBrowse.Name = "btnSalesDocumentBrowse";
            this.btnSalesDocumentBrowse.Size = new System.Drawing.Size(75, 22);
            this.btnSalesDocumentBrowse.TabIndex = 10;
            this.btnSalesDocumentBrowse.Text = "...";
            this.btnSalesDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.UseVisualStyleBackColor = true;
            // 
            // lblPayoutUnit
            // 
            this.lblPayoutUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPayoutUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayoutUnit.Location = new System.Drawing.Point(743, 241);
            this.lblPayoutUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblPayoutUnit.Name = "lblPayoutUnit";
            this.lblPayoutUnit.Size = new System.Drawing.Size(75, 22);
            this.lblPayoutUnit.TabIndex = 68;
            this.lblPayoutUnit.Text = "_lblPayoutUnit";
            this.lblPayoutUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSalePrice
            // 
            this.lblSalePrice.BackColor = System.Drawing.Color.LightGray;
            this.lblSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalePrice.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalePrice.Location = new System.Drawing.Point(1, 49);
            this.lblSalePrice.Margin = new System.Windows.Forms.Padding(1);
            this.lblSalePrice.Name = "lblSalePrice";
            this.lblSalePrice.Size = new System.Drawing.Size(208, 22);
            this.lblSalePrice.TabIndex = 26;
            this.lblSalePrice.Text = "_addSalePrice";
            this.lblSalePrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSalePriceUnit
            // 
            this.lblSalePriceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSalePriceUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalePriceUnit.Location = new System.Drawing.Point(743, 49);
            this.lblSalePriceUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblSalePriceUnit.Name = "lblSalePriceUnit";
            this.lblSalePriceUnit.Size = new System.Drawing.Size(75, 22);
            this.lblSalePriceUnit.TabIndex = 39;
            this.lblSalePriceUnit.Text = "_lblSalePriceUnit";
            this.lblSalePriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSalesBuyDetails
            // 
            this.btnSalesBuyDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSalesBuyDetails.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalesBuyDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesBuyDetails.Location = new System.Drawing.Point(743, 73);
            this.btnSalesBuyDetails.Margin = new System.Windows.Forms.Padding(1);
            this.btnSalesBuyDetails.Name = "btnSalesBuyDetails";
            this.btnSalesBuyDetails.Size = new System.Drawing.Size(75, 22);
            this.btnSalesBuyDetails.TabIndex = 73;
            this.btnSalesBuyDetails.Text = "_details";
            this.btnSalesBuyDetails.UseVisualStyleBackColor = true;
            this.btnSalesBuyDetails.Click += new System.EventHandler(this.OnBtnSalesBuyDetails_Click);
            // 
            // txtBoxSaleBuyValue
            // 
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxSaleBuyValue, 3);
            this.txtBoxSaleBuyValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSaleBuyValue.Enabled = false;
            this.txtBoxSaleBuyValue.Location = new System.Drawing.Point(211, 73);
            this.txtBoxSaleBuyValue.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxSaleBuyValue.Name = "txtBoxSaleBuyValue";
            this.txtBoxSaleBuyValue.ReadOnly = true;
            this.txtBoxSaleBuyValue.Size = new System.Drawing.Size(530, 22);
            this.txtBoxSaleBuyValue.TabIndex = 74;
            // 
            // lblProfitLoss
            // 
            this.lblProfitLoss.BackColor = System.Drawing.Color.LightGray;
            this.lblProfitLoss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProfitLoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfitLoss.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfitLoss.Location = new System.Drawing.Point(1, 97);
            this.lblProfitLoss.Margin = new System.Windows.Forms.Padding(1);
            this.lblProfitLoss.Name = "lblProfitLoss";
            this.lblProfitLoss.Size = new System.Drawing.Size(208, 22);
            this.lblProfitLoss.TabIndex = 44;
            this.lblProfitLoss.Text = "_addProfitLoss";
            this.lblProfitLoss.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReduction
            // 
            this.lblReduction.BackColor = System.Drawing.Color.LightGray;
            this.lblReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReduction.Location = new System.Drawing.Point(1, 217);
            this.lblReduction.Margin = new System.Windows.Forms.Padding(1);
            this.lblReduction.Name = "lblReduction";
            this.lblReduction.Size = new System.Drawing.Size(208, 22);
            this.lblReduction.TabIndex = 69;
            this.lblReduction.Text = "_addReduction";
            this.lblReduction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrokerage
            // 
            this.lblBrokerage.BackColor = System.Drawing.Color.LightGray;
            this.lblBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerage.Location = new System.Drawing.Point(1, 193);
            this.lblBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerage.Name = "lblBrokerage";
            this.lblBrokerage.Size = new System.Drawing.Size(208, 22);
            this.lblBrokerage.TabIndex = 16;
            this.lblBrokerage.Text = "_addBrokerage";
            this.lblBrokerage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSolidarityTax
            // 
            this.lblSolidarityTax.BackColor = System.Drawing.Color.LightGray;
            this.lblSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSolidarityTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolidarityTax.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolidarityTax.Location = new System.Drawing.Point(1, 169);
            this.lblSolidarityTax.Margin = new System.Windows.Forms.Padding(1);
            this.lblSolidarityTax.Name = "lblSolidarityTax";
            this.lblSolidarityTax.Size = new System.Drawing.Size(208, 22);
            this.lblSolidarityTax.TabIndex = 60;
            this.lblSolidarityTax.Text = "_addSolidarityTax";
            this.lblSolidarityTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCapitalGainsTax
            // 
            this.lblCapitalGainsTax.BackColor = System.Drawing.Color.LightGray;
            this.lblCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCapitalGainsTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapitalGainsTax.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapitalGainsTax.Location = new System.Drawing.Point(1, 145);
            this.lblCapitalGainsTax.Margin = new System.Windows.Forms.Padding(1);
            this.lblCapitalGainsTax.Name = "lblCapitalGainsTax";
            this.lblCapitalGainsTax.Size = new System.Drawing.Size(208, 22);
            this.lblCapitalGainsTax.TabIndex = 59;
            this.lblCapitalGainsTax.Text = "_addCaptialGainsTax";
            this.lblCapitalGainsTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxProfitLoss
            // 
            this.txtBoxProfitLoss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxProfitLoss, 3);
            this.txtBoxProfitLoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxProfitLoss.Enabled = false;
            this.txtBoxProfitLoss.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxProfitLoss.Location = new System.Drawing.Point(211, 97);
            this.txtBoxProfitLoss.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxProfitLoss.Name = "txtBoxProfitLoss";
            this.txtBoxProfitLoss.ReadOnly = true;
            this.txtBoxProfitLoss.Size = new System.Drawing.Size(530, 22);
            this.txtBoxProfitLoss.TabIndex = 65;
            // 
            // txtBoxReduction
            // 
            this.txtBoxReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxReduction, 3);
            this.txtBoxReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxReduction.Location = new System.Drawing.Point(211, 217);
            this.txtBoxReduction.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxReduction.Name = "txtBoxReduction";
            this.txtBoxReduction.Size = new System.Drawing.Size(530, 22);
            this.txtBoxReduction.TabIndex = 8;
            this.txtBoxReduction.TextChanged += new System.EventHandler(this.OnTxtBoxReduction_TextChanged);
            this.txtBoxReduction.Enter += new System.EventHandler(this.OnTxtBoxReduction_Enter);
            this.txtBoxReduction.Leave += new System.EventHandler(this.OnTxtBoxReductionLeave);
            // 
            // txtBoxBrokerage
            // 
            this.txtBoxBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxBrokerage, 3);
            this.txtBoxBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBrokerage.Location = new System.Drawing.Point(211, 193);
            this.txtBoxBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxBrokerage.Name = "txtBoxBrokerage";
            this.txtBoxBrokerage.Size = new System.Drawing.Size(530, 22);
            this.txtBoxBrokerage.TabIndex = 7;
            this.txtBoxBrokerage.TextChanged += new System.EventHandler(this.OnTxtBoxBrokerage_TextChanged);
            this.txtBoxBrokerage.Enter += new System.EventHandler(this.OnTxtBoxBrokerage_Enter);
            this.txtBoxBrokerage.Leave += new System.EventHandler(this.OnTxtBoxBrokerageLeave);
            // 
            // txtBoxSolidarityTax
            // 
            this.txtBoxSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxSolidarityTax, 3);
            this.txtBoxSolidarityTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSolidarityTax.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSolidarityTax.Location = new System.Drawing.Point(211, 169);
            this.txtBoxSolidarityTax.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxSolidarityTax.Name = "txtBoxSolidarityTax";
            this.txtBoxSolidarityTax.Size = new System.Drawing.Size(530, 22);
            this.txtBoxSolidarityTax.TabIndex = 6;
            this.txtBoxSolidarityTax.TextChanged += new System.EventHandler(this.OnTxtBoxSolidarityTax_TextChanged);
            this.txtBoxSolidarityTax.Enter += new System.EventHandler(this.OnTxtBoxSolidarityTax_Enter);
            this.txtBoxSolidarityTax.Leave += new System.EventHandler(this.OnTxtBoxSolidarityTaxLeave);
            // 
            // txtBoxCapitalGainsTax
            // 
            this.txtBoxCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlSaleInput.SetColumnSpan(this.txtBoxCapitalGainsTax, 3);
            this.txtBoxCapitalGainsTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCapitalGainsTax.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCapitalGainsTax.Location = new System.Drawing.Point(211, 145);
            this.txtBoxCapitalGainsTax.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxCapitalGainsTax.Name = "txtBoxCapitalGainsTax";
            this.txtBoxCapitalGainsTax.Size = new System.Drawing.Size(530, 22);
            this.txtBoxCapitalGainsTax.TabIndex = 5;
            this.txtBoxCapitalGainsTax.TextChanged += new System.EventHandler(this.OnTxtBoxCapitalGainsTax_TextChanged);
            this.txtBoxCapitalGainsTax.Enter += new System.EventHandler(this.OnTxtBoxCapitalGainsTax_Enter);
            this.txtBoxCapitalGainsTax.Leave += new System.EventHandler(this.OnTxtBoxCapitalGainsTaxLeave);
            // 
            // lblReductionUnit
            // 
            this.lblReductionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReductionUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReductionUnit.Location = new System.Drawing.Point(743, 217);
            this.lblReductionUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblReductionUnit.Name = "lblReductionUnit";
            this.lblReductionUnit.Size = new System.Drawing.Size(75, 22);
            this.lblReductionUnit.TabIndex = 71;
            this.lblReductionUnit.Text = "_lblReductionUnit";
            this.lblReductionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBrokerageUnit
            // 
            this.lblBrokerageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerageUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerageUnit.Location = new System.Drawing.Point(743, 193);
            this.lblBrokerageUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerageUnit.Name = "lblBrokerageUnit";
            this.lblBrokerageUnit.Size = new System.Drawing.Size(75, 22);
            this.lblBrokerageUnit.TabIndex = 24;
            this.lblBrokerageUnit.Text = "_lblBrokerageUnit";
            this.lblBrokerageUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSolidarityTaxUnit
            // 
            this.lblSolidarityTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolidarityTaxUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolidarityTaxUnit.Location = new System.Drawing.Point(743, 169);
            this.lblSolidarityTaxUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblSolidarityTaxUnit.Name = "lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.Size = new System.Drawing.Size(75, 22);
            this.lblSolidarityTaxUnit.TabIndex = 50;
            this.lblSolidarityTaxUnit.Text = "_lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCapitalGainsTaxUnit
            // 
            this.lblCapitalGainsTaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCapitalGainsTaxUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapitalGainsTaxUnit.Location = new System.Drawing.Point(743, 145);
            this.lblCapitalGainsTaxUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblCapitalGainsTaxUnit.Name = "lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.Size = new System.Drawing.Size(75, 22);
            this.lblCapitalGainsTaxUnit.TabIndex = 66;
            this.lblCapitalGainsTaxUnit.Text = "_lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProfitLossUnit
            // 
            this.lblProfitLossUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfitLossUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfitLossUnit.Location = new System.Drawing.Point(743, 97);
            this.lblProfitLossUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblProfitLossUnit.Name = "lblProfitLossUnit";
            this.lblProfitLossUnit.Size = new System.Drawing.Size(75, 22);
            this.lblProfitLossUnit.TabIndex = 67;
            this.lblProfitLossUnit.Text = "_lblProfitLossUnit";
            this.lblProfitLossUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageSaleEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 344);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessageSaleEdit
            // 
            this.toolStripStatusLabelMessageSaleEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageSaleEdit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessageSaleEdit.Name = "toolStripStatusLabelMessageSaleEdit";
            this.toolStripStatusLabelMessageSaleEdit.Size = new System.Drawing.Size(0, 17);
            // 
            // ViewSaleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 556);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxSales);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 609);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 563);
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
        private System.Windows.Forms.Label lblBrokerageUnit;
        private System.Windows.Forms.TextBox txtBoxBrokerage;
        private System.Windows.Forms.Label lblBrokerage;
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
        private System.Windows.Forms.Label lblBuyValue;
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
        private System.Windows.Forms.Label lblReduction;
        private System.Windows.Forms.TextBox txtBoxReduction;
        private System.Windows.Forms.Label lblReductionUnit;
        private System.Windows.Forms.Button btnSalesBuyDetails;
        private System.Windows.Forms.TextBox txtBoxSaleBuyValue;
    }
}