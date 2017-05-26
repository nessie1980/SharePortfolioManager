
using SharePortfolioManager.Properties;

namespace SharePortfolioManager.Forms.SalesForm
{
    partial class FrmShareSalesEdit
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
            this.btnSalesDelete = new System.Windows.Forms.Button();
            this.btnSalesCancel = new System.Windows.Forms.Button();
            this.btnSalesReset = new System.Windows.Forms.Button();
            this.btnSalesAddSave = new System.Windows.Forms.Button();
            this.grpBoxAddSales = new System.Windows.Forms.GroupBox();
            this.lblAddSalesLossBalanceUnit = new System.Windows.Forms.Label();
            this.txtBoxAddSalesLossBalance = new System.Windows.Forms.TextBox();
            this.lblSalesLossBalance = new System.Windows.Forms.Label();
            this.lblAddSalesBuyPriceUnit = new System.Windows.Forms.Label();
            this.txtBoxAddSalesBuyPrice = new System.Windows.Forms.TextBox();
            this.lblSalesBuyPrice = new System.Windows.Forms.Label();
            this.lblAddSalesTaxUnit = new System.Windows.Forms.Label();
            this.txtBoxAddSalesTax = new System.Windows.Forms.TextBox();
            this.lblSalesTax = new System.Windows.Forms.Label();
            this.lblAddSalesSumUnit = new System.Windows.Forms.Label();
            this.txtBoxAddSalesSum = new System.Windows.Forms.TextBox();
            this.lblSalesSum = new System.Windows.Forms.Label();
            this.lblAddSalesPriceUnit = new System.Windows.Forms.Label();
            this.txtBoxAddSalesExchangeRatioFC = new System.Windows.Forms.TextBox();
            this.lblAddSaleExchangeRatioFC = new System.Windows.Forms.Label();
            this.cbxBoxAddSalesFC = new System.Windows.Forms.ComboBox();
            this.chkBoxAddSalesFC = new System.Windows.Forms.CheckBox();
            this.lblAddSaleEnableFC = new System.Windows.Forms.Label();
            this.datePickerAddSaleTime = new System.Windows.Forms.DateTimePicker();
            this.txtBoxAddSalesSalesPrice = new System.Windows.Forms.TextBox();
            this.lblSalesPrice = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAddSalesCostsFCUnit = new System.Windows.Forms.Label();
            this.txtBoxAddSalesCosts = new System.Windows.Forms.TextBox();
            this.lblSalesCosts = new System.Windows.Forms.Label();
            this.btnSalesDocumentBrowse = new System.Windows.Forms.Button();
            this.txtBoxAddSalesDocument = new System.Windows.Forms.TextBox();
            this.lblSalesDocument = new System.Windows.Forms.Label();
            this.lblAddSalesVolumeUnit = new System.Windows.Forms.Label();
            this.txtBoxAddSalesVolume = new System.Windows.Forms.TextBox();
            this.lblSalesVolume = new System.Windows.Forms.Label();
            this.datePickerAddSaleDate = new System.Windows.Forms.DateTimePicker();
            this.lblAddSaleDate = new System.Windows.Forms.Label();
            this.grpBoxSales.SuspendLayout();
            this.grpBoxAddSales.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxSales
            // 
            this.grpBoxSales.Controls.Add(this.tabCtrlSales);
            this.grpBoxSales.Location = new System.Drawing.Point(8, 364);
            this.grpBoxSales.Name = "grpBoxSales";
            this.grpBoxSales.Size = new System.Drawing.Size(815, 157);
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
            this.tabCtrlSales.Size = new System.Drawing.Size(794, 125);
            this.tabCtrlSales.TabIndex = 0;
            // 
            // btnSalesDelete
            // 
            this.btnSalesDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalesDelete.Enabled = false;
            this.btnSalesDelete.Image = Resources.black_delete;
            this.btnSalesDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDelete.Location = new System.Drawing.Point(297, 293);
            this.btnSalesDelete.Name = "btnSalesDelete";
            this.btnSalesDelete.Size = new System.Drawing.Size(166, 31);
            this.btnSalesDelete.TabIndex = 19;
            this.btnSalesDelete.Text = "_Delete";
            this.btnSalesDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDelete.UseVisualStyleBackColor = true;
            this.btnSalesDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnSalesCancel
            // 
            this.btnSalesCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalesCancel.Image = Resources.black_cancel;
            this.btnSalesCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesCancel.Location = new System.Drawing.Point(641, 293);
            this.btnSalesCancel.Name = "btnSalesCancel";
            this.btnSalesCancel.Size = new System.Drawing.Size(166, 31);
            this.btnSalesCancel.TabIndex = 21;
            this.btnSalesCancel.Text = "_Cancel";
            this.btnSalesCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesCancel.UseVisualStyleBackColor = true;
            this.btnSalesCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // btnSalesReset
            // 
            this.btnSalesReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalesReset.Enabled = false;
            this.btnSalesReset.Image = Resources.black_cancel;
            this.btnSalesReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesReset.Location = new System.Drawing.Point(469, 293);
            this.btnSalesReset.Name = "btnSalesReset";
            this.btnSalesReset.Size = new System.Drawing.Size(166, 31);
            this.btnSalesReset.TabIndex = 20;
            this.btnSalesReset.Text = "_Reset";
            this.btnSalesReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesReset.UseVisualStyleBackColor = true;
            this.btnSalesReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // btnSalesAddSave
            // 
            this.btnSalesAddSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalesAddSave.Image = Resources.black_add;
            this.btnSalesAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesAddSave.Location = new System.Drawing.Point(125, 293);
            this.btnSalesAddSave.Name = "btnSalesAddSave";
            this.btnSalesAddSave.Size = new System.Drawing.Size(166, 31);
            this.btnSalesAddSave.TabIndex = 18;
            this.btnSalesAddSave.Text = "_Add/Save";
            this.btnSalesAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesAddSave.UseVisualStyleBackColor = true;
            this.btnSalesAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
            // 
            // grpBoxAddSales
            // 
            this.grpBoxAddSales.Controls.Add(this.lblAddSalesLossBalanceUnit);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesLossBalance);
            this.grpBoxAddSales.Controls.Add(this.lblSalesLossBalance);
            this.grpBoxAddSales.Controls.Add(this.lblAddSalesBuyPriceUnit);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesBuyPrice);
            this.grpBoxAddSales.Controls.Add(this.lblSalesBuyPrice);
            this.grpBoxAddSales.Controls.Add(this.lblAddSalesTaxUnit);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesTax);
            this.grpBoxAddSales.Controls.Add(this.lblSalesTax);
            this.grpBoxAddSales.Controls.Add(this.lblAddSalesSumUnit);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesSum);
            this.grpBoxAddSales.Controls.Add(this.lblSalesSum);
            this.grpBoxAddSales.Controls.Add(this.lblAddSalesPriceUnit);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesExchangeRatioFC);
            this.grpBoxAddSales.Controls.Add(this.lblAddSaleExchangeRatioFC);
            this.grpBoxAddSales.Controls.Add(this.cbxBoxAddSalesFC);
            this.grpBoxAddSales.Controls.Add(this.chkBoxAddSalesFC);
            this.grpBoxAddSales.Controls.Add(this.lblAddSaleEnableFC);
            this.grpBoxAddSales.Controls.Add(this.btnSalesDelete);
            this.grpBoxAddSales.Controls.Add(this.btnSalesCancel);
            this.grpBoxAddSales.Controls.Add(this.btnSalesReset);
            this.grpBoxAddSales.Controls.Add(this.btnSalesAddSave);
            this.grpBoxAddSales.Controls.Add(this.datePickerAddSaleTime);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesSalesPrice);
            this.grpBoxAddSales.Controls.Add(this.lblSalesPrice);
            this.grpBoxAddSales.Controls.Add(this.statusStrip1);
            this.grpBoxAddSales.Controls.Add(this.lblAddSalesCostsFCUnit);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesCosts);
            this.grpBoxAddSales.Controls.Add(this.lblSalesCosts);
            this.grpBoxAddSales.Controls.Add(this.btnSalesDocumentBrowse);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesDocument);
            this.grpBoxAddSales.Controls.Add(this.lblSalesDocument);
            this.grpBoxAddSales.Controls.Add(this.lblAddSalesVolumeUnit);
            this.grpBoxAddSales.Controls.Add(this.txtBoxAddSalesVolume);
            this.grpBoxAddSales.Controls.Add(this.lblSalesVolume);
            this.grpBoxAddSales.Controls.Add(this.datePickerAddSaleDate);
            this.grpBoxAddSales.Controls.Add(this.lblAddSaleDate);
            this.grpBoxAddSales.Location = new System.Drawing.Point(8, 5);
            this.grpBoxAddSales.Name = "grpBoxAddSales";
            this.grpBoxAddSales.Size = new System.Drawing.Size(815, 353);
            this.grpBoxAddSales.TabIndex = 2;
            this.grpBoxAddSales.TabStop = false;
            this.grpBoxAddSales.Text = "_grpBoxAddSales";
            // 
            // lblAddSalesLossBalanceUnit
            // 
            this.lblAddSalesLossBalanceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddSalesLossBalanceUnit.Location = new System.Drawing.Point(755, 157);
            this.lblAddSalesLossBalanceUnit.Name = "lblAddSalesLossBalanceUnit";
            this.lblAddSalesLossBalanceUnit.Size = new System.Drawing.Size(50, 23);
            this.lblAddSalesLossBalanceUnit.TabIndex = 58;
            this.lblAddSalesLossBalanceUnit.Text = "_lblLossBalanceUnit";
            this.lblAddSalesLossBalanceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxAddSalesLossBalance
            // 
            this.txtBoxAddSalesLossBalance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxAddSalesLossBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesLossBalance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesLossBalance.Location = new System.Drawing.Point(380, 157);
            this.txtBoxAddSalesLossBalance.Name = "txtBoxAddSalesLossBalance";
            this.txtBoxAddSalesLossBalance.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesLossBalance.TabIndex = 55;
            this.txtBoxAddSalesLossBalance.TextChanged += new System.EventHandler(this.OnTxtBoxAddSalesLossBalance_TextChanged);
            // 
            // lblSalesLossBalance
            // 
            this.lblSalesLossBalance.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesLossBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesLossBalance.Location = new System.Drawing.Point(10, 157);
            this.lblSalesLossBalance.Name = "lblSalesLossBalance";
            this.lblSalesLossBalance.Size = new System.Drawing.Size(365, 23);
            this.lblSalesLossBalance.TabIndex = 57;
            this.lblSalesLossBalance.Text = "_addSalesLossBalance";
            this.lblSalesLossBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddSalesBuyPriceUnit
            // 
            this.lblAddSalesBuyPriceUnit.Location = new System.Drawing.Point(755, 103);
            this.lblAddSalesBuyPriceUnit.Name = "lblAddSalesBuyPriceUnit";
            this.lblAddSalesBuyPriceUnit.Size = new System.Drawing.Size(50, 25);
            this.lblAddSalesBuyPriceUnit.TabIndex = 54;
            this.lblAddSalesBuyPriceUnit.Text = "_lblBuyPriceUnit";
            this.lblAddSalesBuyPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxAddSalesBuyPrice
            // 
            this.txtBoxAddSalesBuyPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesBuyPrice.Enabled = false;
            this.txtBoxAddSalesBuyPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesBuyPrice.Location = new System.Drawing.Point(380, 103);
            this.txtBoxAddSalesBuyPrice.Name = "txtBoxAddSalesBuyPrice";
            this.txtBoxAddSalesBuyPrice.ReadOnly = true;
            this.txtBoxAddSalesBuyPrice.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesBuyPrice.TabIndex = 7;
            // 
            // lblSalesBuyPrice
            // 
            this.lblSalesBuyPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesBuyPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesBuyPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesBuyPrice.Location = new System.Drawing.Point(11, 103);
            this.lblSalesBuyPrice.Name = "lblSalesBuyPrice";
            this.lblSalesBuyPrice.Size = new System.Drawing.Size(365, 23);
            this.lblSalesBuyPrice.TabIndex = 52;
            this.lblSalesBuyPrice.Text = "_addSalesBuyPrice";
            this.lblSalesBuyPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddSalesTaxUnit
            // 
            this.lblAddSalesTaxUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddSalesTaxUnit.Location = new System.Drawing.Point(755, 211);
            this.lblAddSalesTaxUnit.Name = "lblAddSalesTaxUnit";
            this.lblAddSalesTaxUnit.Size = new System.Drawing.Size(50, 23);
            this.lblAddSalesTaxUnit.TabIndex = 50;
            this.lblAddSalesTaxUnit.Text = "_lblTaxUnit";
            this.lblAddSalesTaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxAddSalesTax
            // 
            this.txtBoxAddSalesTax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxAddSalesTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesTax.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesTax.Location = new System.Drawing.Point(380, 211);
            this.txtBoxAddSalesTax.Name = "txtBoxAddSalesTax";
            this.txtBoxAddSalesTax.ReadOnly = true;
            this.txtBoxAddSalesTax.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesTax.TabIndex = 12;
            // 
            // lblSalesTax
            // 
            this.lblSalesTax.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesTax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesTax.Location = new System.Drawing.Point(10, 211);
            this.lblSalesTax.Name = "lblSalesTax";
            this.lblSalesTax.Size = new System.Drawing.Size(365, 23);
            this.lblSalesTax.TabIndex = 49;
            this.lblSalesTax.Text = "_addSalesTax";
            this.lblSalesTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddSalesSumUnit
            // 
            this.lblAddSalesSumUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddSalesSumUnit.Location = new System.Drawing.Point(755, 184);
            this.lblAddSalesSumUnit.Name = "lblAddSalesSumUnit";
            this.lblAddSalesSumUnit.Size = new System.Drawing.Size(50, 23);
            this.lblAddSalesSumUnit.TabIndex = 45;
            this.lblAddSalesSumUnit.Text = "_lblSumUnit";
            this.lblAddSalesSumUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxAddSalesSum
            // 
            this.txtBoxAddSalesSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxAddSalesSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesSum.Enabled = false;
            this.txtBoxAddSalesSum.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesSum.Location = new System.Drawing.Point(380, 184);
            this.txtBoxAddSalesSum.Name = "txtBoxAddSalesSum";
            this.txtBoxAddSalesSum.ReadOnly = true;
            this.txtBoxAddSalesSum.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesSum.TabIndex = 10;
            this.txtBoxAddSalesSum.TextChanged += new System.EventHandler(this.OnTxtBoxAddSalesSum_TextChanged);
            // 
            // lblSalesSum
            // 
            this.lblSalesSum.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesSum.Location = new System.Drawing.Point(10, 184);
            this.lblSalesSum.Name = "lblSalesSum";
            this.lblSalesSum.Size = new System.Drawing.Size(365, 23);
            this.lblSalesSum.TabIndex = 44;
            this.lblSalesSum.Text = "_addSalesSum";
            this.lblSalesSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddSalesPriceUnit
            // 
            this.lblAddSalesPriceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddSalesPriceUnit.Location = new System.Drawing.Point(755, 130);
            this.lblAddSalesPriceUnit.Name = "lblAddSalesPriceUnit";
            this.lblAddSalesPriceUnit.Size = new System.Drawing.Size(50, 23);
            this.lblAddSalesPriceUnit.TabIndex = 39;
            this.lblAddSalesPriceUnit.Text = "_lblSalePriceUnit";
            this.lblAddSalesPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxAddSalesExchangeRatioFC
            // 
            this.txtBoxAddSalesExchangeRatioFC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxAddSalesExchangeRatioFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesExchangeRatioFC.Enabled = false;
            this.txtBoxAddSalesExchangeRatioFC.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesExchangeRatioFC.Location = new System.Drawing.Point(581, 49);
            this.txtBoxAddSalesExchangeRatioFC.Name = "txtBoxAddSalesExchangeRatioFC";
            this.txtBoxAddSalesExchangeRatioFC.ReadOnly = true;
            this.txtBoxAddSalesExchangeRatioFC.Size = new System.Drawing.Size(115, 23);
            this.txtBoxAddSalesExchangeRatioFC.TabIndex = 4;
            this.txtBoxAddSalesExchangeRatioFC.TextChanged += new System.EventHandler(this.OnTxtBoxAddSalesExchangeRatioFC_TextChanged);
            // 
            // lblAddSaleExchangeRatioFC
            // 
            this.lblAddSaleExchangeRatioFC.BackColor = System.Drawing.Color.LightGray;
            this.lblAddSaleExchangeRatioFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddSaleExchangeRatioFC.Location = new System.Drawing.Point(410, 49);
            this.lblAddSaleExchangeRatioFC.Name = "lblAddSaleExchangeRatioFC";
            this.lblAddSaleExchangeRatioFC.Size = new System.Drawing.Size(165, 23);
            this.lblAddSaleExchangeRatioFC.TabIndex = 37;
            this.lblAddSaleExchangeRatioFC.Text = "_addSaleFactorFC";
            this.lblAddSaleExchangeRatioFC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxBoxAddSalesFC
            // 
            this.cbxBoxAddSalesFC.DropDownHeight = 100;
            this.cbxBoxAddSalesFC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBoxAddSalesFC.Enabled = false;
            this.cbxBoxAddSalesFC.FormattingEnabled = true;
            this.cbxBoxAddSalesFC.IntegralHeight = false;
            this.cbxBoxAddSalesFC.Location = new System.Drawing.Point(702, 49);
            this.cbxBoxAddSalesFC.Name = "cbxBoxAddSalesFC";
            this.cbxBoxAddSalesFC.Size = new System.Drawing.Size(105, 23);
            this.cbxBoxAddSalesFC.TabIndex = 5;
            this.cbxBoxAddSalesFC.SelectedIndexChanged += new System.EventHandler(this.cbxBoxAddSalesForeignCurrency_SelectedIndexChanged);
            // 
            // chkBoxAddSalesFC
            // 
            this.chkBoxAddSalesFC.AutoSize = true;
            this.chkBoxAddSalesFC.Location = new System.Drawing.Point(389, 53);
            this.chkBoxAddSalesFC.Name = "chkBoxAddSalesFC";
            this.chkBoxAddSalesFC.Size = new System.Drawing.Size(15, 14);
            this.chkBoxAddSalesFC.TabIndex = 3;
            this.chkBoxAddSalesFC.UseVisualStyleBackColor = true;
            this.chkBoxAddSalesFC.CheckedChanged += new System.EventHandler(this.chkBoxAddSalesForeignCurrency_CheckedChanged);
            // 
            // lblAddSaleEnableFC
            // 
            this.lblAddSaleEnableFC.BackColor = System.Drawing.Color.LightGray;
            this.lblAddSaleEnableFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddSaleEnableFC.Location = new System.Drawing.Point(10, 49);
            this.lblAddSaleEnableFC.Name = "lblAddSaleEnableFC";
            this.lblAddSaleEnableFC.Size = new System.Drawing.Size(365, 23);
            this.lblAddSaleEnableFC.TabIndex = 33;
            this.lblAddSaleEnableFC.Text = "_addSalesEnableFC";
            this.lblAddSaleEnableFC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerAddSaleTime
            // 
            this.datePickerAddSaleTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerAddSaleTime.Location = new System.Drawing.Point(581, 22);
            this.datePickerAddSaleTime.Name = "datePickerAddSaleTime";
            this.datePickerAddSaleTime.ShowUpDown = true;
            this.datePickerAddSaleTime.Size = new System.Drawing.Size(169, 23);
            this.datePickerAddSaleTime.TabIndex = 2;
            // 
            // txtBoxAddSalesSalesPrice
            // 
            this.txtBoxAddSalesSalesPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesSalesPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesSalesPrice.Location = new System.Drawing.Point(380, 130);
            this.txtBoxAddSalesSalesPrice.Name = "txtBoxAddSalesSalesPrice";
            this.txtBoxAddSalesSalesPrice.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesSalesPrice.TabIndex = 8;
            this.txtBoxAddSalesSalesPrice.TextChanged += new System.EventHandler(this.OnTxtBoxAddSalePrice_TextChanged);
            // 
            // lblSalesPrice
            // 
            this.lblSalesPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesPrice.Location = new System.Drawing.Point(10, 130);
            this.lblSalesPrice.Name = "lblSalesPrice";
            this.lblSalesPrice.Size = new System.Drawing.Size(365, 23);
            this.lblSalesPrice.TabIndex = 26;
            this.lblSalesPrice.Text = "_addSalesPrice";
            this.lblSalesPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(3, 328);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(809, 22);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessage.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // lblAddSalesCostsFCUnit
            // 
            this.lblAddSalesCostsFCUnit.Location = new System.Drawing.Point(755, 238);
            this.lblAddSalesCostsFCUnit.Name = "lblAddSalesCostsFCUnit";
            this.lblAddSalesCostsFCUnit.Size = new System.Drawing.Size(50, 25);
            this.lblAddSalesCostsFCUnit.TabIndex = 24;
            this.lblAddSalesCostsFCUnit.Text = "_lblCostsUnit";
            this.lblAddSalesCostsFCUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxAddSalesCosts
            // 
            this.txtBoxAddSalesCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesCosts.Enabled = false;
            this.txtBoxAddSalesCosts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesCosts.Location = new System.Drawing.Point(380, 238);
            this.txtBoxAddSalesCosts.Name = "txtBoxAddSalesCosts";
            this.txtBoxAddSalesCosts.ReadOnly = true;
            this.txtBoxAddSalesCosts.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesCosts.TabIndex = 14;
            // 
            // lblSalesCosts
            // 
            this.lblSalesCosts.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesCosts.Location = new System.Drawing.Point(11, 238);
            this.lblSalesCosts.Name = "lblSalesCosts";
            this.lblSalesCosts.Size = new System.Drawing.Size(365, 23);
            this.lblSalesCosts.TabIndex = 16;
            this.lblSalesCosts.Text = "_addSalesCosts";
            this.lblSalesCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSalesDocumentBrowse
            // 
            this.btnSalesDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalesDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.Location = new System.Drawing.Point(757, 265);
            this.btnSalesDocumentBrowse.Name = "btnSalesDocumentBrowse";
            this.btnSalesDocumentBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnSalesDocumentBrowse.TabIndex = 17;
            this.btnSalesDocumentBrowse.Text = "...";
            this.btnSalesDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnSalesDocumentBrowse.Click += new System.EventHandler(this.OnBtnSaleDocumentBrowse_Click);
            // 
            // txtBoxAddSalesDocument
            // 
            this.txtBoxAddSalesDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesDocument.Location = new System.Drawing.Point(380, 265);
            this.txtBoxAddSalesDocument.Name = "txtBoxAddSalesDocument";
            this.txtBoxAddSalesDocument.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesDocument.TabIndex = 16;
            // 
            // lblSalesDocument
            // 
            this.lblSalesDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesDocument.Location = new System.Drawing.Point(10, 265);
            this.lblSalesDocument.Name = "lblSalesDocument";
            this.lblSalesDocument.Size = new System.Drawing.Size(365, 23);
            this.lblSalesDocument.TabIndex = 25;
            this.lblSalesDocument.Text = "_salesShareDocument";
            this.lblSalesDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddSalesVolumeUnit
            // 
            this.lblAddSalesVolumeUnit.Location = new System.Drawing.Point(755, 76);
            this.lblAddSalesVolumeUnit.Name = "lblAddSalesVolumeUnit";
            this.lblAddSalesVolumeUnit.Size = new System.Drawing.Size(50, 25);
            this.lblAddSalesVolumeUnit.TabIndex = 17;
            this.lblAddSalesVolumeUnit.Text = "_lblVolumeUnit";
            this.lblAddSalesVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxAddSalesVolume
            // 
            this.txtBoxAddSalesVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxAddSalesVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxAddSalesVolume.Location = new System.Drawing.Point(380, 76);
            this.txtBoxAddSalesVolume.Name = "txtBoxAddSalesVolume";
            this.txtBoxAddSalesVolume.Size = new System.Drawing.Size(370, 23);
            this.txtBoxAddSalesVolume.TabIndex = 6;
            this.txtBoxAddSalesVolume.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolume_TextChanged);
            // 
            // lblSalesVolume
            // 
            this.lblSalesVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblSalesVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesVolume.Location = new System.Drawing.Point(10, 76);
            this.lblSalesVolume.Name = "lblSalesVolume";
            this.lblSalesVolume.Size = new System.Drawing.Size(365, 23);
            this.lblSalesVolume.TabIndex = 15;
            this.lblSalesVolume.Text = "_addSalesVolume";
            this.lblSalesVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerAddSaleDate
            // 
            this.datePickerAddSaleDate.CustomFormat = "";
            this.datePickerAddSaleDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerAddSaleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerAddSaleDate.Location = new System.Drawing.Point(380, 22);
            this.datePickerAddSaleDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerAddSaleDate.Name = "datePickerAddSaleDate";
            this.datePickerAddSaleDate.Size = new System.Drawing.Size(195, 23);
            this.datePickerAddSaleDate.TabIndex = 0;
            // 
            // lblAddSaleDate
            // 
            this.lblAddSaleDate.BackColor = System.Drawing.Color.LightGray;
            this.lblAddSaleDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddSaleDate.Location = new System.Drawing.Point(10, 22);
            this.lblAddSaleDate.Name = "lblAddSaleDate";
            this.lblAddSaleDate.Size = new System.Drawing.Size(365, 23);
            this.lblAddSaleDate.TabIndex = 2;
            this.lblAddSaleDate.Text = "_addSalesDate";
            this.lblAddSaleDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmShareSalesEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(831, 524);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxSales);
            this.Controls.Add(this.grpBoxAddSales);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(847, 562);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(847, 562);
            this.Name = "FrmShareSalesEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareSalesEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareSalesEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareSalesEdit_Load);
            this.Shown += new System.EventHandler(this.FrmShareSalesEdit_Shown);
            this.grpBoxSales.ResumeLayout(false);
            this.grpBoxAddSales.ResumeLayout(false);
            this.grpBoxAddSales.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxSales;
        private System.Windows.Forms.TabControl tabCtrlSales;
        private System.Windows.Forms.Button btnSalesDelete;
        private System.Windows.Forms.Button btnSalesCancel;
        private System.Windows.Forms.Button btnSalesReset;
        private System.Windows.Forms.Button btnSalesAddSave;
        private System.Windows.Forms.GroupBox grpBoxAddSales;
        private System.Windows.Forms.DateTimePicker datePickerAddSaleTime;
        private System.Windows.Forms.TextBox txtBoxAddSalesSalesPrice;
        private System.Windows.Forms.Label lblSalesPrice;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.Label lblAddSalesCostsFCUnit;
        private System.Windows.Forms.TextBox txtBoxAddSalesCosts;
        private System.Windows.Forms.Label lblSalesCosts;
        private System.Windows.Forms.Button btnSalesDocumentBrowse;
        private System.Windows.Forms.TextBox txtBoxAddSalesDocument;
        private System.Windows.Forms.Label lblSalesDocument;
        private System.Windows.Forms.Label lblAddSalesVolumeUnit;
        private System.Windows.Forms.TextBox txtBoxAddSalesVolume;
        private System.Windows.Forms.Label lblSalesVolume;
        private System.Windows.Forms.DateTimePicker datePickerAddSaleDate;
        private System.Windows.Forms.Label lblAddSaleDate;
        private System.Windows.Forms.Label lblAddSaleEnableFC;
        private System.Windows.Forms.TextBox txtBoxAddSalesExchangeRatioFC;
        private System.Windows.Forms.Label lblAddSaleExchangeRatioFC;
        private System.Windows.Forms.ComboBox cbxBoxAddSalesFC;
        private System.Windows.Forms.CheckBox chkBoxAddSalesFC;
        private System.Windows.Forms.Label lblAddSalesPriceUnit;
        private System.Windows.Forms.Label lblAddSalesSumUnit;
        private System.Windows.Forms.TextBox txtBoxAddSalesSum;
        private System.Windows.Forms.Label lblSalesSum;
        private System.Windows.Forms.Label lblAddSalesTaxUnit;
        private System.Windows.Forms.TextBox txtBoxAddSalesTax;
        private System.Windows.Forms.Label lblSalesTax;
        private System.Windows.Forms.Label lblSalesBuyPrice;
        private System.Windows.Forms.Label lblAddSalesBuyPriceUnit;
        private System.Windows.Forms.TextBox txtBoxAddSalesBuyPrice;
        private System.Windows.Forms.Label lblAddSalesLossBalanceUnit;
        private System.Windows.Forms.TextBox txtBoxAddSalesLossBalance;
        private System.Windows.Forms.Label lblSalesLossBalance;
    }
}