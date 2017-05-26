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
            this.lblSolidarityTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxSolidarityTax = new System.Windows.Forms.TextBox();
            this.lblSolidarityTax = new System.Windows.Forms.Label();
            this.lblCapitalGainsTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxCapitalGainsTax = new System.Windows.Forms.TextBox();
            this.lblCapitalGainsTax = new System.Windows.Forms.Label();
            this.lblTaxAtSourceUnit = new System.Windows.Forms.Label();
            this.txtBoxTaxAtSource = new System.Windows.Forms.TextBox();
            this.lblTaxAtSource = new System.Windows.Forms.Label();
            this.txtBoxPayoutFC = new System.Windows.Forms.TextBox();
            this.lblPayoutFCUnit = new System.Windows.Forms.Label();
            this.lblTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxTax = new System.Windows.Forms.TextBox();
            this.lblAddTax = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.lblVolumeUnit = new System.Windows.Forms.Label();
            this.lblPayoutAfterTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxPayoutAfterTax = new System.Windows.Forms.TextBox();
            this.lblAddPayoutAfterTax = new System.Windows.Forms.Label();
            this.txtBoxExchangeRatio = new System.Windows.Forms.TextBox();
            this.lblDividendExchangeRatio = new System.Windows.Forms.Label();
            this.lblEnableForeignCurrency = new System.Windows.Forms.Label();
            this.cbxBoxDividendFCUnit = new System.Windows.Forms.ComboBox();
            this.chkBoxEnableFC = new System.Windows.Forms.CheckBox();
            this.btnAddDocumentBrowse = new System.Windows.Forms.Button();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.lblAddDocument = new System.Windows.Forms.Label();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblPayoutUnit = new System.Windows.Forms.Label();
            this.txtBoxPayout = new System.Windows.Forms.TextBox();
            this.lblPayout = new System.Windows.Forms.Label();
            this.lblYieldUnit = new System.Windows.Forms.Label();
            this.txtBoxYield = new System.Windows.Forms.TextBox();
            this.lblAddYield = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblPriceUnit = new System.Windows.Forms.Label();
            this.lblDividendRateUnit = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.txtBoxPrice = new System.Windows.Forms.TextBox();
            this.txtBoxRate = new System.Windows.Forms.TextBox();
            this.lblAddPrice = new System.Windows.Forms.Label();
            this.lblDividendRate = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.grpBoxDividends = new System.Windows.Forms.GroupBox();
            this.tabCtrlDividends = new System.Windows.Forms.TabControl();
            this.lblAddDate = new System.Windows.Forms.Label();
            this.lblAddDividendExchangeRatio = new System.Windows.Forms.Label();
            this.lblAddVolume = new System.Windows.Forms.Label();
            this.lblAddTaxAtSource = new System.Windows.Forms.Label();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxAddDividend.SuspendLayout();
            this.grpBoxDividends.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddSave
            // 
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(54, 402);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(166, 31);
            this.btnAddSave.TabIndex = 16;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.BtnAddSave_Click);
            // 
            // grpBoxAddDividend
            // 
            this.grpBoxAddDividend.Controls.Add(this.lblSolidarityTaxUnit);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxSolidarityTax);
            this.grpBoxAddDividend.Controls.Add(this.lblSolidarityTax);
            this.grpBoxAddDividend.Controls.Add(this.lblCapitalGainsTaxUnit);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxCapitalGainsTax);
            this.grpBoxAddDividend.Controls.Add(this.lblCapitalGainsTax);
            this.grpBoxAddDividend.Controls.Add(this.lblTaxAtSourceUnit);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxTaxAtSource);
            this.grpBoxAddDividend.Controls.Add(this.lblTaxAtSource);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxPayoutFC);
            this.grpBoxAddDividend.Controls.Add(this.lblPayoutFCUnit);
            this.grpBoxAddDividend.Controls.Add(this.lblTaxUnit);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxTax);
            this.grpBoxAddDividend.Controls.Add(this.lblAddTax);
            this.grpBoxAddDividend.Controls.Add(this.lblVolume);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxVolume);
            this.grpBoxAddDividend.Controls.Add(this.lblVolumeUnit);
            this.grpBoxAddDividend.Controls.Add(this.lblPayoutAfterTaxUnit);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxPayoutAfterTax);
            this.grpBoxAddDividend.Controls.Add(this.lblAddPayoutAfterTax);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxExchangeRatio);
            this.grpBoxAddDividend.Controls.Add(this.lblDividendExchangeRatio);
            this.grpBoxAddDividend.Controls.Add(this.lblEnableForeignCurrency);
            this.grpBoxAddDividend.Controls.Add(this.cbxBoxDividendFCUnit);
            this.grpBoxAddDividend.Controls.Add(this.chkBoxEnableFC);
            this.grpBoxAddDividend.Controls.Add(this.btnAddDocumentBrowse);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxDocument);
            this.grpBoxAddDividend.Controls.Add(this.lblAddDocument);
            this.grpBoxAddDividend.Controls.Add(this.datePickerTime);
            this.grpBoxAddDividend.Controls.Add(this.btnDelete);
            this.grpBoxAddDividend.Controls.Add(this.lblPayoutUnit);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxPayout);
            this.grpBoxAddDividend.Controls.Add(this.lblPayout);
            this.grpBoxAddDividend.Controls.Add(this.lblYieldUnit);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxYield);
            this.grpBoxAddDividend.Controls.Add(this.lblAddYield);
            this.grpBoxAddDividend.Controls.Add(this.btnCancel);
            this.grpBoxAddDividend.Controls.Add(this.btnReset);
            this.grpBoxAddDividend.Controls.Add(this.lblPriceUnit);
            this.grpBoxAddDividend.Controls.Add(this.lblDividendRateUnit);
            this.grpBoxAddDividend.Controls.Add(this.statusStrip1);
            this.grpBoxAddDividend.Controls.Add(this.datePickerDate);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxPrice);
            this.grpBoxAddDividend.Controls.Add(this.txtBoxRate);
            this.grpBoxAddDividend.Controls.Add(this.lblAddPrice);
            this.grpBoxAddDividend.Controls.Add(this.lblDividendRate);
            this.grpBoxAddDividend.Controls.Add(this.lblDate);
            this.grpBoxAddDividend.Controls.Add(this.btnAddSave);
            this.grpBoxAddDividend.Location = new System.Drawing.Point(8, 5);
            this.grpBoxAddDividend.Name = "grpBoxAddDividend";
            this.grpBoxAddDividend.Size = new System.Drawing.Size(744, 462);
            this.grpBoxAddDividend.TabIndex = 2;
            this.grpBoxAddDividend.TabStop = false;
            this.grpBoxAddDividend.Text = "_grpBoxAddDividend";
            // 
            // lblSolidarityTaxUnit
            // 
            this.lblSolidarityTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolidarityTaxUnit.Location = new System.Drawing.Point(658, 239);
            this.lblSolidarityTaxUnit.Name = "lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.Size = new System.Drawing.Size(77, 23);
            this.lblSolidarityTaxUnit.TabIndex = 59;
            this.lblSolidarityTaxUnit.Text = "_lblSolidarityTaxUnit";
            this.lblSolidarityTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxSolidarityTax
            // 
            this.txtBoxSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxSolidarityTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSolidarityTax.Location = new System.Drawing.Point(314, 239);
            this.txtBoxSolidarityTax.Name = "txtBoxSolidarityTax";
            this.txtBoxSolidarityTax.Size = new System.Drawing.Size(338, 23);
            this.txtBoxSolidarityTax.TabIndex = 11;
            this.txtBoxSolidarityTax.TextChanged += new System.EventHandler(this.txtBoxSolidarityTax_TextChanged);
            this.txtBoxSolidarityTax.Leave += new System.EventHandler(this.txtBoxSolidarityTax_Leave);
            // 
            // lblSolidarityTax
            // 
            this.lblSolidarityTax.BackColor = System.Drawing.Color.LightGray;
            this.lblSolidarityTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSolidarityTax.Location = new System.Drawing.Point(9, 239);
            this.lblSolidarityTax.Name = "lblSolidarityTax";
            this.lblSolidarityTax.Size = new System.Drawing.Size(300, 23);
            this.lblSolidarityTax.TabIndex = 58;
            this.lblSolidarityTax.Text = "_addSolidarityTax";
            this.lblSolidarityTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCapitalGainsTaxUnit
            // 
            this.lblCapitalGainsTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapitalGainsTaxUnit.Location = new System.Drawing.Point(658, 212);
            this.lblCapitalGainsTaxUnit.Name = "lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.Size = new System.Drawing.Size(77, 23);
            this.lblCapitalGainsTaxUnit.TabIndex = 56;
            this.lblCapitalGainsTaxUnit.Text = "_lblCapitalGainsTaxUnit";
            this.lblCapitalGainsTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxCapitalGainsTax
            // 
            this.txtBoxCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxCapitalGainsTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCapitalGainsTax.Location = new System.Drawing.Point(314, 212);
            this.txtBoxCapitalGainsTax.Name = "txtBoxCapitalGainsTax";
            this.txtBoxCapitalGainsTax.Size = new System.Drawing.Size(338, 23);
            this.txtBoxCapitalGainsTax.TabIndex = 10;
            this.txtBoxCapitalGainsTax.TextChanged += new System.EventHandler(this.txtBoxCapitalGainsTax_TextChanged);
            this.txtBoxCapitalGainsTax.Leave += new System.EventHandler(this.txtBoxCapitalGainsTax_Leave);
            // 
            // lblCapitalGainsTax
            // 
            this.lblCapitalGainsTax.BackColor = System.Drawing.Color.LightGray;
            this.lblCapitalGainsTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCapitalGainsTax.Location = new System.Drawing.Point(9, 212);
            this.lblCapitalGainsTax.Name = "lblCapitalGainsTax";
            this.lblCapitalGainsTax.Size = new System.Drawing.Size(300, 23);
            this.lblCapitalGainsTax.TabIndex = 55;
            this.lblCapitalGainsTax.Text = "_addCapitalGainsTax";
            this.lblCapitalGainsTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxAtSourceUnit
            // 
            this.lblTaxAtSourceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAtSourceUnit.Location = new System.Drawing.Point(658, 185);
            this.lblTaxAtSourceUnit.Name = "lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.Size = new System.Drawing.Size(77, 23);
            this.lblTaxAtSourceUnit.TabIndex = 53;
            this.lblTaxAtSourceUnit.Text = "_lblTaxAtSourceUnit";
            this.lblTaxAtSourceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTaxAtSource
            // 
            this.txtBoxTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxTaxAtSource.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTaxAtSource.Location = new System.Drawing.Point(314, 185);
            this.txtBoxTaxAtSource.Name = "txtBoxTaxAtSource";
            this.txtBoxTaxAtSource.Size = new System.Drawing.Size(338, 23);
            this.txtBoxTaxAtSource.TabIndex = 9;
            this.txtBoxTaxAtSource.TextChanged += new System.EventHandler(this.txtBoxTaxAtSource_TextChanged);
            this.txtBoxTaxAtSource.Leave += new System.EventHandler(this.txtBoxTaxAtSource_Leave);
            // 
            // lblTaxAtSource
            // 
            this.lblTaxAtSource.BackColor = System.Drawing.Color.LightGray;
            this.lblTaxAtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTaxAtSource.Location = new System.Drawing.Point(9, 185);
            this.lblTaxAtSource.Name = "lblTaxAtSource";
            this.lblTaxAtSource.Size = new System.Drawing.Size(300, 23);
            this.lblTaxAtSource.TabIndex = 52;
            this.lblTaxAtSource.Text = "_addTaxAtSource";
            this.lblTaxAtSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxPayoutFC
            // 
            this.txtBoxPayoutFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPayoutFC.Enabled = false;
            this.txtBoxPayoutFC.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayoutFC.Location = new System.Drawing.Point(532, 158);
            this.txtBoxPayoutFC.Name = "txtBoxPayoutFC";
            this.txtBoxPayoutFC.ReadOnly = true;
            this.txtBoxPayoutFC.Size = new System.Drawing.Size(121, 23);
            this.txtBoxPayoutFC.TabIndex = 8;
            // 
            // lblPayoutFCUnit
            // 
            this.lblPayoutFCUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayoutFCUnit.Location = new System.Drawing.Point(656, 158);
            this.lblPayoutFCUnit.Name = "lblPayoutFCUnit";
            this.lblPayoutFCUnit.Size = new System.Drawing.Size(77, 23);
            this.lblPayoutFCUnit.TabIndex = 49;
            this.lblPayoutFCUnit.Text = "_lblPayoutFCUnit";
            this.lblPayoutFCUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTaxUnit
            // 
            this.lblTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxUnit.Location = new System.Drawing.Point(659, 266);
            this.lblTaxUnit.Name = "lblTaxUnit";
            this.lblTaxUnit.Size = new System.Drawing.Size(77, 23);
            this.lblTaxUnit.TabIndex = 48;
            this.lblTaxUnit.Text = "_lblTaxUnit";
            this.lblTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTax
            // 
            this.txtBoxTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxTax.Enabled = false;
            this.txtBoxTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTax.Location = new System.Drawing.Point(315, 266);
            this.txtBoxTax.Name = "txtBoxTax";
            this.txtBoxTax.Size = new System.Drawing.Size(338, 23);
            this.txtBoxTax.TabIndex = 12;
            // 
            // lblAddTax
            // 
            this.lblAddTax.BackColor = System.Drawing.Color.LightGray;
            this.lblAddTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddTax.Location = new System.Drawing.Point(10, 266);
            this.lblAddTax.Name = "lblAddTax";
            this.lblAddTax.Size = new System.Drawing.Size(300, 23);
            this.lblAddTax.TabIndex = 47;
            this.lblAddTax.Text = "_addTax";
            this.lblAddTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Location = new System.Drawing.Point(10, 131);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(300, 23);
            this.lblVolume.TabIndex = 5;
            this.lblVolume.Text = "_addVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(315, 131);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(338, 23);
            this.txtBoxVolume.TabIndex = 6;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.TxtBoxAddVolume_TextChanged);
            this.txtBoxVolume.Leave += new System.EventHandler(this.txtBoxVolume_Leave);
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeUnit.Location = new System.Drawing.Point(659, 131);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(77, 23);
            this.lblVolumeUnit.TabIndex = 14;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPayoutAfterTaxUnit
            // 
            this.lblPayoutAfterTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayoutAfterTaxUnit.Location = new System.Drawing.Point(659, 293);
            this.lblPayoutAfterTaxUnit.Name = "lblPayoutAfterTaxUnit";
            this.lblPayoutAfterTaxUnit.Size = new System.Drawing.Size(77, 23);
            this.lblPayoutAfterTaxUnit.TabIndex = 40;
            this.lblPayoutAfterTaxUnit.Text = "_lblPayoutAfterTaxUnit";
            this.lblPayoutAfterTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxPayoutAfterTax
            // 
            this.txtBoxPayoutAfterTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPayoutAfterTax.Enabled = false;
            this.txtBoxPayoutAfterTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayoutAfterTax.Location = new System.Drawing.Point(315, 293);
            this.txtBoxPayoutAfterTax.Name = "txtBoxPayoutAfterTax";
            this.txtBoxPayoutAfterTax.ReadOnly = true;
            this.txtBoxPayoutAfterTax.Size = new System.Drawing.Size(338, 23);
            this.txtBoxPayoutAfterTax.TabIndex = 10;
            // 
            // lblAddPayoutAfterTax
            // 
            this.lblAddPayoutAfterTax.BackColor = System.Drawing.Color.LightGray;
            this.lblAddPayoutAfterTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddPayoutAfterTax.Location = new System.Drawing.Point(10, 293);
            this.lblAddPayoutAfterTax.Name = "lblAddPayoutAfterTax";
            this.lblAddPayoutAfterTax.Size = new System.Drawing.Size(300, 23);
            this.lblAddPayoutAfterTax.TabIndex = 38;
            this.lblAddPayoutAfterTax.Text = "_addPayoutAfterTax";
            this.lblAddPayoutAfterTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxExchangeRatio
            // 
            this.txtBoxExchangeRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxExchangeRatio.Enabled = false;
            this.txtBoxExchangeRatio.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxExchangeRatio.Location = new System.Drawing.Point(314, 76);
            this.txtBoxExchangeRatio.Name = "txtBoxExchangeRatio";
            this.txtBoxExchangeRatio.ReadOnly = true;
            this.txtBoxExchangeRatio.Size = new System.Drawing.Size(122, 23);
            this.txtBoxExchangeRatio.TabIndex = 3;
            this.txtBoxExchangeRatio.TextChanged += new System.EventHandler(this.TxtBoxAddDividendExchangeRatio_TextChanged);
            this.txtBoxExchangeRatio.Leave += new System.EventHandler(this.txtBoxExchangeRatio_Leave);
            // 
            // lblDividendExchangeRatio
            // 
            this.lblDividendExchangeRatio.BackColor = System.Drawing.Color.LightGray;
            this.lblDividendExchangeRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDividendExchangeRatio.Location = new System.Drawing.Point(10, 76);
            this.lblDividendExchangeRatio.Name = "lblDividendExchangeRatio";
            this.lblDividendExchangeRatio.Size = new System.Drawing.Size(300, 23);
            this.lblDividendExchangeRatio.TabIndex = 31;
            this.lblDividendExchangeRatio.Text = "_addExchangeRatio";
            this.lblDividendExchangeRatio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEnableForeignCurrency
            // 
            this.lblEnableForeignCurrency.BackColor = System.Drawing.Color.LightGray;
            this.lblEnableForeignCurrency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEnableForeignCurrency.Location = new System.Drawing.Point(10, 49);
            this.lblEnableForeignCurrency.Name = "lblEnableForeignCurrency";
            this.lblEnableForeignCurrency.Size = new System.Drawing.Size(300, 23);
            this.lblEnableForeignCurrency.TabIndex = 30;
            this.lblEnableForeignCurrency.Text = "_addEnableFC";
            this.lblEnableForeignCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxBoxDividendFCUnit
            // 
            this.cbxBoxDividendFCUnit.DropDownHeight = 100;
            this.cbxBoxDividendFCUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBoxDividendFCUnit.Enabled = false;
            this.cbxBoxDividendFCUnit.FormattingEnabled = true;
            this.cbxBoxDividendFCUnit.IntegralHeight = false;
            this.cbxBoxDividendFCUnit.Location = new System.Drawing.Point(447, 76);
            this.cbxBoxDividendFCUnit.Name = "cbxBoxDividendFCUnit";
            this.cbxBoxDividendFCUnit.Size = new System.Drawing.Size(206, 23);
            this.cbxBoxDividendFCUnit.TabIndex = 4;
            this.cbxBoxDividendFCUnit.SelectedIndexChanged += new System.EventHandler(this.CbxBoxAddDividendForeignCurrency_SelectedIndexChanged);
            // 
            // chkBoxEnableFC
            // 
            this.chkBoxEnableFC.AutoSize = true;
            this.chkBoxEnableFC.Location = new System.Drawing.Point(317, 54);
            this.chkBoxEnableFC.Name = "chkBoxEnableFC";
            this.chkBoxEnableFC.Size = new System.Drawing.Size(15, 14);
            this.chkBoxEnableFC.TabIndex = 2;
            this.chkBoxEnableFC.UseVisualStyleBackColor = true;
            this.chkBoxEnableFC.CheckedChanged += new System.EventHandler(this.ChkBoxAddDividendForeignCurrency_CheckedChanged);
            // 
            // btnAddDocumentBrowse
            // 
            this.btnAddDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddDocumentBrowse.Location = new System.Drawing.Point(659, 374);
            this.btnAddDocumentBrowse.Name = "btnAddDocumentBrowse";
            this.btnAddDocumentBrowse.Size = new System.Drawing.Size(77, 23);
            this.btnAddDocumentBrowse.TabIndex = 15;
            this.btnAddDocumentBrowse.Text = "...";
            this.btnAddDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnAddDocumentBrowse.Click += new System.EventHandler(this.BtnAddDividendDocumentBrowse_Click);
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(315, 374);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(338, 23);
            this.txtBoxDocument.TabIndex = 14;
            // 
            // lblAddDocument
            // 
            this.lblAddDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblAddDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddDocument.Location = new System.Drawing.Point(10, 374);
            this.lblAddDocument.Name = "lblAddDocument";
            this.lblAddDocument.Size = new System.Drawing.Size(300, 23);
            this.lblAddDocument.TabIndex = 25;
            this.lblAddDocument.Text = "_addDocument";
            this.lblAddDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerTime
            // 
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(528, 22);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(125, 23);
            this.datePickerTime.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(226, 402);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(166, 31);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDividendDelete_Click);
            // 
            // lblPayoutUnit
            // 
            this.lblPayoutUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayoutUnit.Location = new System.Drawing.Point(444, 158);
            this.lblPayoutUnit.Name = "lblPayoutUnit";
            this.lblPayoutUnit.Size = new System.Drawing.Size(77, 23);
            this.lblPayoutUnit.TabIndex = 22;
            this.lblPayoutUnit.Text = "_lblPayoutUnit";
            this.lblPayoutUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxPayout
            // 
            this.txtBoxPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPayout.Enabled = false;
            this.txtBoxPayout.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPayout.Location = new System.Drawing.Point(315, 158);
            this.txtBoxPayout.Name = "txtBoxPayout";
            this.txtBoxPayout.ReadOnly = true;
            this.txtBoxPayout.Size = new System.Drawing.Size(121, 23);
            this.txtBoxPayout.TabIndex = 7;
            // 
            // lblPayout
            // 
            this.lblPayout.BackColor = System.Drawing.Color.LightGray;
            this.lblPayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPayout.Location = new System.Drawing.Point(10, 158);
            this.lblPayout.Name = "lblPayout";
            this.lblPayout.Size = new System.Drawing.Size(300, 23);
            this.lblPayout.TabIndex = 20;
            this.lblPayout.Text = "_addPayout";
            this.lblPayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblYieldUnit
            // 
            this.lblYieldUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYieldUnit.Location = new System.Drawing.Point(659, 320);
            this.lblYieldUnit.Name = "lblYieldUnit";
            this.lblYieldUnit.Size = new System.Drawing.Size(74, 23);
            this.lblYieldUnit.TabIndex = 19;
            this.lblYieldUnit.Text = "_lblYieldUnit";
            this.lblYieldUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxYield
            // 
            this.txtBoxYield.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxYield.Enabled = false;
            this.txtBoxYield.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxYield.Location = new System.Drawing.Point(315, 320);
            this.txtBoxYield.Name = "txtBoxYield";
            this.txtBoxYield.ReadOnly = true;
            this.txtBoxYield.Size = new System.Drawing.Size(338, 23);
            this.txtBoxYield.TabIndex = 11;
            // 
            // lblAddYield
            // 
            this.lblAddYield.BackColor = System.Drawing.Color.LightGray;
            this.lblAddYield.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddYield.Location = new System.Drawing.Point(10, 320);
            this.lblAddYield.Name = "lblAddYield";
            this.lblAddYield.Size = new System.Drawing.Size(300, 23);
            this.lblAddYield.TabIndex = 17;
            this.lblAddYield.Text = "_addYield";
            this.lblAddYield.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(570, 402);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(166, 31);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(398, 402);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(166, 31);
            this.btnReset.TabIndex = 18;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // lblPriceUnit
            // 
            this.lblPriceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceUnit.Location = new System.Drawing.Point(659, 347);
            this.lblPriceUnit.Name = "lblPriceUnit";
            this.lblPriceUnit.Size = new System.Drawing.Size(75, 23);
            this.lblPriceUnit.TabIndex = 13;
            this.lblPriceUnit.Text = "_lblPriceUnit";
            this.lblPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDividendRateUnit
            // 
            this.lblDividendRateUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDividendRateUnit.Location = new System.Drawing.Point(659, 104);
            this.lblDividendRateUnit.Name = "lblDividendRateUnit";
            this.lblDividendRateUnit.Size = new System.Drawing.Size(74, 23);
            this.lblDividendRateUnit.TabIndex = 12;
            this.lblDividendRateUnit.Text = "_lblDividendUnit";
            this.lblDividendRateUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(3, 437);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(738, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
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
            // txtBoxPrice
            // 
            this.txtBoxPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPrice.Location = new System.Drawing.Point(315, 347);
            this.txtBoxPrice.Name = "txtBoxPrice";
            this.txtBoxPrice.Size = new System.Drawing.Size(338, 23);
            this.txtBoxPrice.TabIndex = 13;
            this.txtBoxPrice.TextChanged += new System.EventHandler(this.TxtBoxAddPrice_TextChanged);
            this.txtBoxPrice.Leave += new System.EventHandler(this.txtBoxPrice_Leave);
            // 
            // txtBoxRate
            // 
            this.txtBoxRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxRate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxRate.Location = new System.Drawing.Point(315, 104);
            this.txtBoxRate.Name = "txtBoxRate";
            this.txtBoxRate.Size = new System.Drawing.Size(338, 23);
            this.txtBoxRate.TabIndex = 5;
            this.txtBoxRate.TextChanged += new System.EventHandler(this.TxtBoxAddDividendRate_TextChanged);
            this.txtBoxRate.Leave += new System.EventHandler(this.txtBoxRate_Leave);
            // 
            // lblAddPrice
            // 
            this.lblAddPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblAddPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddPrice.Location = new System.Drawing.Point(10, 347);
            this.lblAddPrice.Name = "lblAddPrice";
            this.lblAddPrice.Size = new System.Drawing.Size(300, 23);
            this.lblAddPrice.TabIndex = 4;
            this.lblAddPrice.Text = "_addPrice";
            this.lblAddPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDividendRate
            // 
            this.lblDividendRate.BackColor = System.Drawing.Color.LightGray;
            this.lblDividendRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDividendRate.Location = new System.Drawing.Point(10, 104);
            this.lblDividendRate.Name = "lblDividendRate";
            this.lblDividendRate.Size = new System.Drawing.Size(300, 23);
            this.lblDividendRate.TabIndex = 3;
            this.lblDividendRate.Text = "_addDividendRate";
            this.lblDividendRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Location = new System.Drawing.Point(10, 22);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(300, 23);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpBoxDividends
            // 
            this.grpBoxDividends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxDividends.Controls.Add(this.tabCtrlDividends);
            this.grpBoxDividends.Location = new System.Drawing.Point(8, 473);
            this.grpBoxDividends.Name = "grpBoxDividends";
            this.grpBoxDividends.Size = new System.Drawing.Size(744, 136);
            this.grpBoxDividends.TabIndex = 3;
            this.grpBoxDividends.TabStop = false;
            this.grpBoxDividends.Text = "_dividends";
            // 
            // tabCtrlDividends
            // 
            this.tabCtrlDividends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlDividends.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlDividends.Location = new System.Drawing.Point(9, 22);
            this.tabCtrlDividends.Name = "tabCtrlDividends";
            this.tabCtrlDividends.SelectedIndex = 0;
            this.tabCtrlDividends.Size = new System.Drawing.Size(726, 105);
            this.tabCtrlDividends.TabIndex = 0;
            // 
            // lblAddDate
            // 
            this.lblAddDate.BackColor = System.Drawing.Color.LightGray;
            this.lblAddDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddDate.Location = new System.Drawing.Point(10, 22);
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
            this.lblAddTaxAtSource.Location = new System.Drawing.Point(9, 185);
            this.lblAddTaxAtSource.Name = "lblAddTaxAtSource";
            this.lblAddTaxAtSource.Size = new System.Drawing.Size(300, 23);
            this.lblAddTaxAtSource.TabIndex = 52;
            this.lblAddTaxAtSource.Text = "_addTaxAtSource";
            this.lblAddTaxAtSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessage.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // ViewDividendEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(759, 612);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxDividends);
            this.Controls.Add(this.grpBoxAddDividend);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(775, 650);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(775, 650);
            this.Name = "ViewDividendEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareDividendEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareDividendEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareDividendEdit_Load);
            this.Shown += new System.EventHandler(this.ShareDividendEdit_Shown);
            this.grpBoxAddDividend.ResumeLayout(false);
            this.grpBoxAddDividend.PerformLayout();
            this.grpBoxDividends.ResumeLayout(false);
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
    }
}