
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
            this.tabCtrlSales = new System.Windows.Forms.TabControl();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.grpBoxAdd = new System.Windows.Forms.GroupBox();
            this.lblPayoutUnit = new System.Windows.Forms.Label();
            this.lblProfitLossUnit = new System.Windows.Forms.Label();
            this.lblCapitalGainsTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxProfitLoss = new System.Windows.Forms.TextBox();
            this.txtBoxSolidarityTax = new System.Windows.Forms.TextBox();
            this.txtBoxCapitalGainsTax = new System.Windows.Forms.TextBox();
            this.lblPayout = new System.Windows.Forms.Label();
            this.lblSolidarityTax = new System.Windows.Forms.Label();
            this.lblCapitalGainsTax = new System.Windows.Forms.Label();
            this.lblBuyPriceUnit = new System.Windows.Forms.Label();
            this.txtBoxBuyPrice = new System.Windows.Forms.TextBox();
            this.lblBuyPrice = new System.Windows.Forms.Label();
            this.lblSolidarityTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxTaxAtSource = new System.Windows.Forms.TextBox();
            this.lblTaxAtSource = new System.Windows.Forms.Label();
            this.lblTaxAtSourceUnit = new System.Windows.Forms.Label();
            this.txtBoxPayout = new System.Windows.Forms.TextBox();
            this.lblProfitLoss = new System.Windows.Forms.Label();
            this.lblSalePriceUnit = new System.Windows.Forms.Label();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.txtBoxSalePrice = new System.Windows.Forms.TextBox();
            this.lblSalePrice = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageSaleEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCostsUnit = new System.Windows.Forms.Label();
            this.txtBoxCosts = new System.Windows.Forms.TextBox();
            this.lblCosts = new System.Windows.Forms.Label();
            this.btnSalesDocumentBrowse = new System.Windows.Forms.Button();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.lblSalesDocument = new System.Windows.Forms.Label();
            this.lblVolumeUnit = new System.Windows.Forms.Label();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.lblAddSaleDate = new System.Windows.Forms.Label();
            this.grpBoxSales.SuspendLayout();
            this.grpBoxAdd.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxSales
            // 
            this.grpBoxSales.Controls.Add(this.tabCtrlSales);
            this.grpBoxSales.Location = new System.Drawing.Point(8, 395);
            this.grpBoxSales.Name = "grpBoxSales";
            this.grpBoxSales.Size = new System.Drawing.Size(744, 215);
            this.grpBoxSales.TabIndex = 3;
            this.grpBoxSales.TabStop = false;
            this.grpBoxSales.Text = "_sales";
            // 
            // tabCtrlSales
            // 
            this.tabCtrlSales.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlSales.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlSales.Location = new System.Drawing.Point(10, 22);
            this.tabCtrlSales.Name = "tabCtrlSales";
            this.tabCtrlSales.SelectedIndex = 0;
            this.tabCtrlSales.Size = new System.Drawing.Size(726, 183);
            this.tabCtrlSales.TabIndex = 0;
            this.tabCtrlSales.SelectedIndexChanged += new System.EventHandler(this.tabCtrlSales_SelectedIndexChanged);
            this.tabCtrlSales.MouseEnter += new System.EventHandler(this.tabCtrlSales_MouseEnter);
            this.tabCtrlSales.MouseLeave += new System.EventHandler(this.tabCtrlSales_MouseLeave);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = global::SharePortfolioManager.Properties.Resources.black_delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(226, 324);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(166, 31);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::SharePortfolioManager.Properties.Resources.black_cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(570, 324);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(166, 31);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.Image = global::SharePortfolioManager.Properties.Resources.black_cancel;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(398, 324);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(166, 31);
            this.btnReset.TabIndex = 20;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // btnAddSave
            // 
            this.btnAddSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSave.Image = global::SharePortfolioManager.Properties.Resources.black_add;
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(54, 324);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(166, 31);
            this.btnAddSave.TabIndex = 18;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Controls.Add(this.lblPayoutUnit);
            this.grpBoxAdd.Controls.Add(this.lblProfitLossUnit);
            this.grpBoxAdd.Controls.Add(this.lblCapitalGainsTaxUnit);
            this.grpBoxAdd.Controls.Add(this.txtBoxProfitLoss);
            this.grpBoxAdd.Controls.Add(this.txtBoxSolidarityTax);
            this.grpBoxAdd.Controls.Add(this.txtBoxCapitalGainsTax);
            this.grpBoxAdd.Controls.Add(this.lblPayout);
            this.grpBoxAdd.Controls.Add(this.lblSolidarityTax);
            this.grpBoxAdd.Controls.Add(this.lblCapitalGainsTax);
            this.grpBoxAdd.Controls.Add(this.lblBuyPriceUnit);
            this.grpBoxAdd.Controls.Add(this.txtBoxBuyPrice);
            this.grpBoxAdd.Controls.Add(this.lblBuyPrice);
            this.grpBoxAdd.Controls.Add(this.lblSolidarityTaxUnit);
            this.grpBoxAdd.Controls.Add(this.txtBoxTaxAtSource);
            this.grpBoxAdd.Controls.Add(this.lblTaxAtSource);
            this.grpBoxAdd.Controls.Add(this.lblTaxAtSourceUnit);
            this.grpBoxAdd.Controls.Add(this.txtBoxPayout);
            this.grpBoxAdd.Controls.Add(this.lblProfitLoss);
            this.grpBoxAdd.Controls.Add(this.lblSalePriceUnit);
            this.grpBoxAdd.Controls.Add(this.btnDelete);
            this.grpBoxAdd.Controls.Add(this.btnCancel);
            this.grpBoxAdd.Controls.Add(this.btnReset);
            this.grpBoxAdd.Controls.Add(this.btnAddSave);
            this.grpBoxAdd.Controls.Add(this.datePickerTime);
            this.grpBoxAdd.Controls.Add(this.txtBoxSalePrice);
            this.grpBoxAdd.Controls.Add(this.lblSalePrice);
            this.grpBoxAdd.Controls.Add(this.statusStrip1);
            this.grpBoxAdd.Controls.Add(this.lblCostsUnit);
            this.grpBoxAdd.Controls.Add(this.txtBoxCosts);
            this.grpBoxAdd.Controls.Add(this.lblCosts);
            this.grpBoxAdd.Controls.Add(this.btnSalesDocumentBrowse);
            this.grpBoxAdd.Controls.Add(this.txtBoxDocument);
            this.grpBoxAdd.Controls.Add(this.lblSalesDocument);
            this.grpBoxAdd.Controls.Add(this.lblVolumeUnit);
            this.grpBoxAdd.Controls.Add(this.txtBoxVolume);
            this.grpBoxAdd.Controls.Add(this.lblVolume);
            this.grpBoxAdd.Controls.Add(this.datePickerDate);
            this.grpBoxAdd.Controls.Add(this.lblAddSaleDate);
            this.grpBoxAdd.Location = new System.Drawing.Point(8, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(744, 384);
            this.grpBoxAdd.TabIndex = 2;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAddSales";
            // 
            // lblPayoutUnit
            // 
            this.lblPayoutUnit.Location = new System.Drawing.Point(659, 266);
            this.lblPayoutUnit.Name = "lblPayoutUnit";
            this.lblPayoutUnit.Size = new System.Drawing.Size(74, 23);
            this.lblPayoutUnit.TabIndex = 68;
            this.lblPayoutUnit.Text = "_lblPayoutUnit";
            this.lblPayoutUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProfitLossUnit
            // 
            this.lblProfitLossUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfitLossUnit.Location = new System.Drawing.Point(659, 239);
            this.lblProfitLossUnit.Name = "lblProfitLossUnit";
            this.lblProfitLossUnit.Size = new System.Drawing.Size(74, 23);
            this.lblProfitLossUnit.TabIndex = 67;
            this.lblProfitLossUnit.Text = "_lblProfitLossUnit";
            this.lblProfitLossUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCapitalGainsTaxUnit
            // 
            this.lblCapitalGainsTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapitalGainsTaxUnit.Location = new System.Drawing.Point(658, 158);
            this.lblCapitalGainsTaxUnit.Name = "lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.Size = new System.Drawing.Size(74, 23);
            this.lblCapitalGainsTaxUnit.TabIndex = 66;
            this.lblCapitalGainsTaxUnit.Text = "_lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxProfitLoss
            // 
            this.txtBoxProfitLoss.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxProfitLoss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxProfitLoss.Enabled = false;
            this.txtBoxProfitLoss.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxProfitLoss.Location = new System.Drawing.Point(315, 239);
            this.txtBoxProfitLoss.Name = "txtBoxProfitLoss";
            this.txtBoxProfitLoss.ReadOnly = true;
            this.txtBoxProfitLoss.Size = new System.Drawing.Size(337, 23);
            this.txtBoxProfitLoss.TabIndex = 65;
            // 
            // txtBoxSolidarityTax
            // 
            this.txtBoxSolidarityTax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxSolidarityTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSolidarityTax.Location = new System.Drawing.Point(315, 185);
            this.txtBoxSolidarityTax.Name = "txtBoxSolidarityTax";
            this.txtBoxSolidarityTax.Size = new System.Drawing.Size(337, 23);
            this.txtBoxSolidarityTax.TabIndex = 64;
            this.txtBoxSolidarityTax.TextChanged += new System.EventHandler(this.OnTxtBoxSolidarityTax_TextChanged);
            this.txtBoxSolidarityTax.Leave += new System.EventHandler(this.txtBoxSolidarityTaxLeave);
            // 
            // txtBoxCapitalGainsTax
            // 
            this.txtBoxCapitalGainsTax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxCapitalGainsTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCapitalGainsTax.Location = new System.Drawing.Point(315, 158);
            this.txtBoxCapitalGainsTax.Name = "txtBoxCapitalGainsTax";
            this.txtBoxCapitalGainsTax.Size = new System.Drawing.Size(337, 23);
            this.txtBoxCapitalGainsTax.TabIndex = 63;
            this.txtBoxCapitalGainsTax.TextChanged += new System.EventHandler(this.OnTxtBoxCapitalGainsTax_TextChanged);
            this.txtBoxCapitalGainsTax.Leave += new System.EventHandler(this.txtBoxCapitalGAinsTaxLeave);
            // 
            // lblPayout
            // 
            this.lblPayout.BackColor = System.Drawing.Color.LightGray;
            this.lblPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPayout.Location = new System.Drawing.Point(10, 266);
            this.lblPayout.Name = "lblPayout";
            this.lblPayout.Size = new System.Drawing.Size(300, 23);
            this.lblPayout.TabIndex = 62;
            this.lblPayout.Text = "_addPayout";
            this.lblPayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSolidarityTax
            // 
            this.lblSolidarityTax.BackColor = System.Drawing.Color.LightGray;
            this.lblSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSolidarityTax.Location = new System.Drawing.Point(10, 185);
            this.lblSolidarityTax.Name = "lblSolidarityTax";
            this.lblSolidarityTax.Size = new System.Drawing.Size(300, 23);
            this.lblSolidarityTax.TabIndex = 60;
            this.lblSolidarityTax.Text = "_addSolidarityTax";
            this.lblSolidarityTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCapitalGainsTax
            // 
            this.lblCapitalGainsTax.BackColor = System.Drawing.Color.LightGray;
            this.lblCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCapitalGainsTax.Location = new System.Drawing.Point(10, 158);
            this.lblCapitalGainsTax.Name = "lblCapitalGainsTax";
            this.lblCapitalGainsTax.Size = new System.Drawing.Size(300, 23);
            this.lblCapitalGainsTax.TabIndex = 59;
            this.lblCapitalGainsTax.Text = "_addCaptialGainsTax";
            this.lblCapitalGainsTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBuyPriceUnit
            // 
            this.lblBuyPriceUnit.Location = new System.Drawing.Point(659, 77);
            this.lblBuyPriceUnit.Name = "lblBuyPriceUnit";
            this.lblBuyPriceUnit.Size = new System.Drawing.Size(74, 23);
            this.lblBuyPriceUnit.TabIndex = 54;
            this.lblBuyPriceUnit.Text = "_lblBuyPriceUnit";
            this.lblBuyPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxBuyPrice
            // 
            this.txtBoxBuyPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxBuyPrice.Enabled = false;
            this.txtBoxBuyPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBuyPrice.Location = new System.Drawing.Point(315, 77);
            this.txtBoxBuyPrice.Name = "txtBoxBuyPrice";
            this.txtBoxBuyPrice.ReadOnly = true;
            this.txtBoxBuyPrice.Size = new System.Drawing.Size(338, 23);
            this.txtBoxBuyPrice.TabIndex = 7;
            this.txtBoxBuyPrice.TextChanged += new System.EventHandler(this.OnTxtBoxAddBuyPrice_TextChanged);
            this.txtBoxBuyPrice.Leave += new System.EventHandler(this.txtBoxBuyPrice_Leave);
            // 
            // lblBuyPrice
            // 
            this.lblBuyPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblBuyPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBuyPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuyPrice.Location = new System.Drawing.Point(10, 77);
            this.lblBuyPrice.Name = "lblBuyPrice";
            this.lblBuyPrice.Size = new System.Drawing.Size(300, 23);
            this.lblBuyPrice.TabIndex = 52;
            this.lblBuyPrice.Text = "_addBuyPrice";
            this.lblBuyPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSolidarityTaxUnit
            // 
            this.lblSolidarityTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolidarityTaxUnit.Location = new System.Drawing.Point(659, 185);
            this.lblSolidarityTaxUnit.Name = "lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.Size = new System.Drawing.Size(74, 23);
            this.lblSolidarityTaxUnit.TabIndex = 50;
            this.lblSolidarityTaxUnit.Text = "_lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTaxAtSource
            // 
            this.txtBoxTaxAtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxTaxAtSource.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTaxAtSource.Location = new System.Drawing.Point(315, 131);
            this.txtBoxTaxAtSource.Name = "txtBoxTaxAtSource";
            this.txtBoxTaxAtSource.Size = new System.Drawing.Size(337, 23);
            this.txtBoxTaxAtSource.TabIndex = 12;
            this.txtBoxTaxAtSource.TextChanged += new System.EventHandler(this.OnTxtBoxTaxAtSource_TextChanged);
            this.txtBoxTaxAtSource.Leave += new System.EventHandler(this.txtBoxTaxAtSourceLeave);
            // 
            // lblTaxAtSource
            // 
            this.lblTaxAtSource.BackColor = System.Drawing.Color.LightGray;
            this.lblTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTaxAtSource.Location = new System.Drawing.Point(10, 131);
            this.lblTaxAtSource.Name = "lblTaxAtSource";
            this.lblTaxAtSource.Size = new System.Drawing.Size(300, 23);
            this.lblTaxAtSource.TabIndex = 49;
            this.lblTaxAtSource.Text = "_addTaxAtSource";
            this.lblTaxAtSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxAtSourceUnit
            // 
            this.lblTaxAtSourceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAtSourceUnit.Location = new System.Drawing.Point(658, 131);
            this.lblTaxAtSourceUnit.Name = "lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.Size = new System.Drawing.Size(74, 23);
            this.lblTaxAtSourceUnit.TabIndex = 45;
            this.lblTaxAtSourceUnit.Text = "_lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxPayout
            // 
            this.txtBoxPayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPayout.Enabled = false;
            this.txtBoxPayout.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayout.Location = new System.Drawing.Point(315, 266);
            this.txtBoxPayout.Name = "txtBoxPayout";
            this.txtBoxPayout.ReadOnly = true;
            this.txtBoxPayout.Size = new System.Drawing.Size(337, 23);
            this.txtBoxPayout.TabIndex = 10;
            // 
            // lblProfitLoss
            // 
            this.lblProfitLoss.BackColor = System.Drawing.Color.LightGray;
            this.lblProfitLoss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProfitLoss.Location = new System.Drawing.Point(10, 239);
            this.lblProfitLoss.Name = "lblProfitLoss";
            this.lblProfitLoss.Size = new System.Drawing.Size(300, 23);
            this.lblProfitLoss.TabIndex = 44;
            this.lblProfitLoss.Text = "_addProfitLoss";
            this.lblProfitLoss.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSalePriceUnit
            // 
            this.lblSalePriceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalePriceUnit.Location = new System.Drawing.Point(656, 104);
            this.lblSalePriceUnit.Name = "lblSalePriceUnit";
            this.lblSalePriceUnit.Size = new System.Drawing.Size(74, 23);
            this.lblSalePriceUnit.TabIndex = 39;
            this.lblSalePriceUnit.Text = "_lblSalePriceUnit";
            this.lblSalePriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datePickerTime
            // 
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(528, 22);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(125, 23);
            this.datePickerTime.TabIndex = 2;
            // 
            // txtBoxSalePrice
            // 
            this.txtBoxSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxSalePrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSalePrice.Location = new System.Drawing.Point(315, 104);
            this.txtBoxSalePrice.Name = "txtBoxSalePrice";
            this.txtBoxSalePrice.Size = new System.Drawing.Size(338, 23);
            this.txtBoxSalePrice.TabIndex = 8;
            this.txtBoxSalePrice.TextChanged += new System.EventHandler(this.OnTxtBoxSalePrice_TextChanged);
            this.txtBoxSalePrice.Leave += new System.EventHandler(this.txtBoxSalePrice_Leave);
            // 
            // lblSalePrice
            // 
            this.lblSalePrice.BackColor = System.Drawing.Color.LightGray;
            this.lblSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalePrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalePrice.Location = new System.Drawing.Point(10, 104);
            this.lblSalePrice.Name = "lblSalePrice";
            this.lblSalePrice.Size = new System.Drawing.Size(300, 23);
            this.lblSalePrice.TabIndex = 26;
            this.lblSalePrice.Text = "_addSalePrice";
            this.lblSalePrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageSaleEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 359);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(738, 22);
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
            // lblCostsUnit
            // 
            this.lblCostsUnit.Location = new System.Drawing.Point(659, 212);
            this.lblCostsUnit.Name = "lblCostsUnit";
            this.lblCostsUnit.Size = new System.Drawing.Size(74, 23);
            this.lblCostsUnit.TabIndex = 24;
            this.lblCostsUnit.Text = "_lblCostsUnit";
            this.lblCostsUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxCosts
            // 
            this.txtBoxCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxCosts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCosts.Location = new System.Drawing.Point(315, 212);
            this.txtBoxCosts.Name = "txtBoxCosts";
            this.txtBoxCosts.Size = new System.Drawing.Size(338, 23);
            this.txtBoxCosts.TabIndex = 14;
            this.txtBoxCosts.TextChanged += new System.EventHandler(this.OnTxtBoxCosts_TextChanged);
            this.txtBoxCosts.Leave += new System.EventHandler(this.txtBoxCostsTaxLeave);
            // 
            // lblCosts
            // 
            this.lblCosts.BackColor = System.Drawing.Color.LightGray;
            this.lblCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCosts.Location = new System.Drawing.Point(10, 212);
            this.lblCosts.Name = "lblCosts";
            this.lblCosts.Size = new System.Drawing.Size(300, 23);
            this.lblCosts.TabIndex = 16;
            this.lblCosts.Text = "_addCosts";
            this.lblCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSalesDocumentBrowse
            // 
            this.btnSalesDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalesDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.Location = new System.Drawing.Point(659, 293);
            this.btnSalesDocumentBrowse.Name = "btnSalesDocumentBrowse";
            this.btnSalesDocumentBrowse.Size = new System.Drawing.Size(77, 23);
            this.btnSalesDocumentBrowse.TabIndex = 17;
            this.btnSalesDocumentBrowse.Text = "...";
            this.btnSalesDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnSalesDocumentBrowse.Click += new System.EventHandler(this.OnBtnSaleDocumentBrowse_Click);
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(315, 293);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(338, 23);
            this.txtBoxDocument.TabIndex = 16;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            // 
            // lblSalesDocument
            // 
            this.lblSalesDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesDocument.Location = new System.Drawing.Point(10, 293);
            this.lblSalesDocument.Name = "lblSalesDocument";
            this.lblSalesDocument.Size = new System.Drawing.Size(300, 23);
            this.lblSalesDocument.TabIndex = 25;
            this.lblSalesDocument.Text = "_salesShareDocument";
            this.lblSalesDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Location = new System.Drawing.Point(659, 50);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(74, 23);
            this.lblVolumeUnit.TabIndex = 17;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(315, 50);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(338, 23);
            this.txtBoxVolume.TabIndex = 6;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolume_TextChanged);
            this.txtBoxVolume.Leave += new System.EventHandler(this.txtBoxVolume_Leave);
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Location = new System.Drawing.Point(10, 50);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(300, 23);
            this.lblVolume.TabIndex = 15;
            this.lblVolume.Text = "_addVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(315, 22);
            this.datePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(121, 23);
            this.datePickerDate.TabIndex = 0;
            // 
            // lblAddSaleDate
            // 
            this.lblAddSaleDate.BackColor = System.Drawing.Color.LightGray;
            this.lblAddSaleDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddSaleDate.Location = new System.Drawing.Point(10, 22);
            this.lblAddSaleDate.Name = "lblAddSaleDate";
            this.lblAddSaleDate.Size = new System.Drawing.Size(300, 23);
            this.lblAddSaleDate.TabIndex = 2;
            this.lblAddSaleDate.Text = "_addDate";
            this.lblAddSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewSaleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(759, 612);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxSales);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(775, 650);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(775, 650);
            this.Name = "ViewSaleEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareSalesEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareSalesEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareSalesEdit_Load);
            this.grpBoxSales.ResumeLayout(false);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
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
    }
}