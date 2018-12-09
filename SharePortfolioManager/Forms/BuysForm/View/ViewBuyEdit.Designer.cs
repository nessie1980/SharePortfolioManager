﻿namespace SharePortfolioManager.Forms.BuysForm.View
{
    partial class ViewBuyEdit
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
            this.grpBoxAdd = new System.Windows.Forms.GroupBox();
            this.tblLayPnlBuyButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tblLayPnlBuyInput = new System.Windows.Forms.TableLayoutPanel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTraderPlaceFeeUnit = new System.Windows.Forms.Label();
            this.lblBrokerFeeUnit = new System.Windows.Forms.Label();
            this.lblProvisionUnit = new System.Windows.Forms.Label();
            this.txtBoxTraderPlaceFee = new System.Windows.Forms.TextBox();
            this.txtBoxBrokerFee = new System.Windows.Forms.TextBox();
            this.txtBoxProvision = new System.Windows.Forms.TextBox();
            this.lblTraderPlaceFee = new System.Windows.Forms.Label();
            this.lblBrokerFee = new System.Windows.Forms.Label();
            this.lblProvision = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblVolumeUnit = new System.Windows.Forms.Label();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTime = new System.Windows.Forms.DateTimePicker();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblMarketValue = new System.Windows.Forms.Label();
            this.lblBuyPrice = new System.Windows.Forms.Label();
            this.lblVolumeSold = new System.Windows.Forms.Label();
            this.txtBoxMarketValue = new System.Windows.Forms.TextBox();
            this.txtBoxSharePrice = new System.Windows.Forms.TextBox();
            this.txtBoxVolumeSold = new System.Windows.Forms.TextBox();
            this.lblMarketValueUnit = new System.Windows.Forms.Label();
            this.lblPriceUnit = new System.Windows.Forms.Label();
            this.lblVolumeSoldUnit = new System.Windows.Forms.Label();
            this.lblDocument = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.btnDocumentBrowse = new System.Windows.Forms.Button();
            this.lblDepositUnit = new System.Windows.Forms.Label();
            this.lblReductionUnit = new System.Windows.Forms.Label();
            this.lblBrokerageUnit = new System.Windows.Forms.Label();
            this.lblDeposit = new System.Windows.Forms.Label();
            this.txtBoxDeposit = new System.Windows.Forms.TextBox();
            this.lblReduction = new System.Windows.Forms.Label();
            this.txtBoxReduction = new System.Windows.Forms.TextBox();
            this.lblBrokerage = new System.Windows.Forms.Label();
            this.txtBoxBrokerage = new System.Windows.Forms.TextBox();
            this.picBoxDateParseState = new System.Windows.Forms.PictureBox();
            this.picBoxTimeParseState = new System.Windows.Forms.PictureBox();
            this.picBoxVolumeParseState = new System.Windows.Forms.PictureBox();
            this.picBoxPriceParseState = new System.Windows.Forms.PictureBox();
            this.picBoxProvisionParseState = new System.Windows.Forms.PictureBox();
            this.picBoxBrokerFeeParseState = new System.Windows.Forms.PictureBox();
            this.picBoxTraderPlaceFeeParseState = new System.Windows.Forms.PictureBox();
            this.picBoxReductionParseState = new System.Windows.Forms.PictureBox();
            this.statusStripMessages = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageBuyEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarBuyDocumentParsing = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelMessageBuyDocumentParsing = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxBuys = new System.Windows.Forms.GroupBox();
            this.tblLayPnlOverviewTabControl = new System.Windows.Forms.TableLayoutPanel();
            this.tabCtrlBuys = new System.Windows.Forms.TabControl();
            this.lblPurchaseValue = new System.Windows.Forms.Label();
            this.grpBoxAdd.SuspendLayout();
            this.tblLayPnlBuyButtons.SuspendLayout();
            this.tblLayPnlBuyInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDateParseState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTimeParseState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxVolumeParseState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxPriceParseState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxProvisionParseState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxBrokerFeeParseState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTraderPlaceFeeParseState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxReductionParseState)).BeginInit();
            this.statusStripMessages.SuspendLayout();
            this.grpBoxBuys.SuspendLayout();
            this.tblLayPnlOverviewTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBuyButtons);
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBuyInput);
            this.grpBoxAdd.Controls.Add(this.statusStripMessages);
            this.grpBoxAdd.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(825, 392);
            this.grpBoxAdd.TabIndex = 15;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAdd";
            // 
            // tblLayPnlBuyButtons
            // 
            this.tblLayPnlBuyButtons.ColumnCount = 5;
            this.tblLayPnlBuyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlBuyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBuyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBuyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBuyButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBuyButtons.Controls.Add(this.btnReset, 3, 0);
            this.tblLayPnlBuyButtons.Controls.Add(this.btnDelete, 2, 0);
            this.tblLayPnlBuyButtons.Controls.Add(this.btnAddSave, 1, 0);
            this.tblLayPnlBuyButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tblLayPnlBuyButtons.Location = new System.Drawing.Point(3, 331);
            this.tblLayPnlBuyButtons.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlBuyButtons.Name = "tblLayPnlBuyButtons";
            this.tblLayPnlBuyButtons.RowCount = 1;
            this.tblLayPnlBuyButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlBuyButtons.Size = new System.Drawing.Size(819, 33);
            this.tblLayPnlBuyButtons.TabIndex = 16;
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Image = global::SharePortfolioManager.Properties.Resources.button_reset_24;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(460, 1);
            this.btnReset.Margin = new System.Windows.Forms.Padding(1);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(178, 31);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::SharePortfolioManager.Properties.Resources.button_recycle_bin_24;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(280, 1);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(178, 31);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnAddSave
            // 
            this.btnAddSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSave.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSave.Image = global::SharePortfolioManager.Properties.Resources.button_add_24;
            this.btnAddSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.Location = new System.Drawing.Point(100, 1);
            this.btnAddSave.Margin = new System.Windows.Forms.Padding(1);
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.Size = new System.Drawing.Size(178, 31);
            this.btnAddSave.TabIndex = 8;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::SharePortfolioManager.Properties.Resources.button_back_24;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(640, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(178, 31);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // tblLayPnlBuyInput
            // 
            this.tblLayPnlBuyInput.ColumnCount = 5;
            this.tblLayPnlBuyInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tblLayPnlBuyInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBuyInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBuyInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlBuyInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.Controls.Add(this.lblTime, 0, 1);
            this.tblLayPnlBuyInput.Controls.Add(this.lblTraderPlaceFeeUnit, 3, 8);
            this.tblLayPnlBuyInput.Controls.Add(this.lblBrokerFeeUnit, 3, 7);
            this.tblLayPnlBuyInput.Controls.Add(this.lblProvisionUnit, 3, 6);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxTraderPlaceFee, 1, 8);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxBrokerFee, 1, 7);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxProvision, 1, 6);
            this.tblLayPnlBuyInput.Controls.Add(this.lblTraderPlaceFee, 0, 8);
            this.tblLayPnlBuyInput.Controls.Add(this.lblBrokerFee, 0, 7);
            this.tblLayPnlBuyInput.Controls.Add(this.lblProvision, 0, 6);
            this.tblLayPnlBuyInput.Controls.Add(this.lblDate, 0, 0);
            this.tblLayPnlBuyInput.Controls.Add(this.lblVolumeUnit, 3, 2);
            this.tblLayPnlBuyInput.Controls.Add(this.dateTimePickerDate, 1, 0);
            this.tblLayPnlBuyInput.Controls.Add(this.dateTimePickerTime, 1, 1);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxVolume, 1, 2);
            this.tblLayPnlBuyInput.Controls.Add(this.lblVolume, 0, 2);
            this.tblLayPnlBuyInput.Controls.Add(this.lblMarketValue, 0, 5);
            this.tblLayPnlBuyInput.Controls.Add(this.lblBuyPrice, 0, 4);
            this.tblLayPnlBuyInput.Controls.Add(this.lblVolumeSold, 0, 3);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxMarketValue, 1, 5);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxSharePrice, 1, 4);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxVolumeSold, 1, 3);
            this.tblLayPnlBuyInput.Controls.Add(this.lblMarketValueUnit, 3, 5);
            this.tblLayPnlBuyInput.Controls.Add(this.lblPriceUnit, 3, 4);
            this.tblLayPnlBuyInput.Controls.Add(this.lblVolumeSoldUnit, 3, 3);
            this.tblLayPnlBuyInput.Controls.Add(this.lblDocument, 0, 12);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxDocument, 1, 12);
            this.tblLayPnlBuyInput.Controls.Add(this.btnDocumentBrowse, 3, 12);
            this.tblLayPnlBuyInput.Controls.Add(this.lblDepositUnit, 3, 11);
            this.tblLayPnlBuyInput.Controls.Add(this.lblReductionUnit, 3, 9);
            this.tblLayPnlBuyInput.Controls.Add(this.lblBrokerageUnit, 3, 10);
            this.tblLayPnlBuyInput.Controls.Add(this.lblDeposit, 0, 11);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxDeposit, 1, 11);
            this.tblLayPnlBuyInput.Controls.Add(this.lblReduction, 0, 9);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxReduction, 1, 9);
            this.tblLayPnlBuyInput.Controls.Add(this.lblBrokerage, 0, 10);
            this.tblLayPnlBuyInput.Controls.Add(this.txtBoxBrokerage, 1, 10);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxDateParseState, 4, 0);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxTimeParseState, 4, 1);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxVolumeParseState, 4, 2);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxPriceParseState, 4, 4);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxProvisionParseState, 4, 6);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxBrokerFeeParseState, 4, 7);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxTraderPlaceFeeParseState, 4, 8);
            this.tblLayPnlBuyInput.Controls.Add(this.picBoxReductionParseState, 4, 9);
            this.tblLayPnlBuyInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlBuyInput.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlBuyInput.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlBuyInput.Name = "tblLayPnlBuyInput";
            this.tblLayPnlBuyInput.RowCount = 13;
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuyInput.Size = new System.Drawing.Size(819, 312);
            this.tblLayPnlBuyInput.TabIndex = 15;
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.LightGray;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(1, 25);
            this.lblTime.Margin = new System.Windows.Forms.Padding(1);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(248, 22);
            this.lblTime.TabIndex = 41;
            this.lblTime.Text = "_addTime";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTraderPlaceFeeUnit
            // 
            this.lblTraderPlaceFeeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTraderPlaceFeeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTraderPlaceFeeUnit.Location = new System.Drawing.Point(719, 193);
            this.lblTraderPlaceFeeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblTraderPlaceFeeUnit.Name = "lblTraderPlaceFeeUnit";
            this.lblTraderPlaceFeeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblTraderPlaceFeeUnit.TabIndex = 40;
            this.lblTraderPlaceFeeUnit.Text = "_lblTraderPlaceFeeUnit";
            this.lblTraderPlaceFeeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBrokerFeeUnit
            // 
            this.lblBrokerFeeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerFeeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerFeeUnit.Location = new System.Drawing.Point(719, 169);
            this.lblBrokerFeeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerFeeUnit.Name = "lblBrokerFeeUnit";
            this.lblBrokerFeeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblBrokerFeeUnit.TabIndex = 39;
            this.lblBrokerFeeUnit.Text = "_lblBrokerFeeUnit";
            this.lblBrokerFeeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProvisionUnit
            // 
            this.lblProvisionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProvisionUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProvisionUnit.Location = new System.Drawing.Point(719, 145);
            this.lblProvisionUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblProvisionUnit.Name = "lblProvisionUnit";
            this.lblProvisionUnit.Size = new System.Drawing.Size(75, 22);
            this.lblProvisionUnit.TabIndex = 38;
            this.lblProvisionUnit.Text = "_lblProvisionUnit";
            this.lblProvisionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTraderPlaceFee
            // 
            this.txtBoxTraderPlaceFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxTraderPlaceFee, 2);
            this.txtBoxTraderPlaceFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxTraderPlaceFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTraderPlaceFee.Location = new System.Drawing.Point(251, 193);
            this.txtBoxTraderPlaceFee.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxTraderPlaceFee.Name = "txtBoxTraderPlaceFee";
            this.txtBoxTraderPlaceFee.Size = new System.Drawing.Size(466, 22);
            this.txtBoxTraderPlaceFee.TabIndex = 37;
            this.txtBoxTraderPlaceFee.TextChanged += new System.EventHandler(this.OnTxtBoxTraderPlaceFee_TextChanged);
            this.txtBoxTraderPlaceFee.Enter += new System.EventHandler(this.OnTxtBoxTraderPlaceFee_Enter);
            this.txtBoxTraderPlaceFee.Leave += new System.EventHandler(this.OnTxtBoxTraderPlaceFee_Leave);
            // 
            // txtBoxBrokerFee
            // 
            this.txtBoxBrokerFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxBrokerFee, 2);
            this.txtBoxBrokerFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBrokerFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBrokerFee.Location = new System.Drawing.Point(251, 169);
            this.txtBoxBrokerFee.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxBrokerFee.Name = "txtBoxBrokerFee";
            this.txtBoxBrokerFee.Size = new System.Drawing.Size(466, 22);
            this.txtBoxBrokerFee.TabIndex = 36;
            this.txtBoxBrokerFee.TextChanged += new System.EventHandler(this.OnTxtBoxBrokerFee_TextChanged);
            this.txtBoxBrokerFee.Enter += new System.EventHandler(this.OnTxtBoxBrokerFee_Enter);
            this.txtBoxBrokerFee.Leave += new System.EventHandler(this.OnTxtBoxBrokerFee_Leave);
            // 
            // txtBoxProvision
            // 
            this.txtBoxProvision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxProvision, 2);
            this.txtBoxProvision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxProvision.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxProvision.Location = new System.Drawing.Point(251, 145);
            this.txtBoxProvision.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxProvision.Name = "txtBoxProvision";
            this.txtBoxProvision.Size = new System.Drawing.Size(466, 22);
            this.txtBoxProvision.TabIndex = 35;
            this.txtBoxProvision.TextChanged += new System.EventHandler(this.OnTxtBoxProvision_TextChanged);
            this.txtBoxProvision.Enter += new System.EventHandler(this.OnTxtBoxProvision_Enter);
            this.txtBoxProvision.Leave += new System.EventHandler(this.OnTxtBoxProvision_Leave);
            // 
            // lblTraderPlaceFee
            // 
            this.lblTraderPlaceFee.BackColor = System.Drawing.Color.LightGray;
            this.lblTraderPlaceFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTraderPlaceFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTraderPlaceFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTraderPlaceFee.Location = new System.Drawing.Point(1, 193);
            this.lblTraderPlaceFee.Margin = new System.Windows.Forms.Padding(1);
            this.lblTraderPlaceFee.Name = "lblTraderPlaceFee";
            this.lblTraderPlaceFee.Size = new System.Drawing.Size(248, 22);
            this.lblTraderPlaceFee.TabIndex = 34;
            this.lblTraderPlaceFee.Text = "_addTraderPlaceFee";
            this.lblTraderPlaceFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrokerFee
            // 
            this.lblBrokerFee.BackColor = System.Drawing.Color.LightGray;
            this.lblBrokerFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBrokerFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerFee.Location = new System.Drawing.Point(1, 169);
            this.lblBrokerFee.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerFee.Name = "lblBrokerFee";
            this.lblBrokerFee.Size = new System.Drawing.Size(248, 22);
            this.lblBrokerFee.TabIndex = 33;
            this.lblBrokerFee.Text = "_addBrokerFee";
            this.lblBrokerFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProvision
            // 
            this.lblProvision.BackColor = System.Drawing.Color.LightGray;
            this.lblProvision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProvision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProvision.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProvision.Location = new System.Drawing.Point(1, 145);
            this.lblProvision.Margin = new System.Windows.Forms.Padding(1);
            this.lblProvision.Name = "lblProvision";
            this.lblProvision.Size = new System.Drawing.Size(248, 22);
            this.lblProvision.TabIndex = 32;
            this.lblProvision.Text = "_addProvision";
            this.lblProvision.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.lblDate.TabIndex = 13;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolumeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeUnit.Location = new System.Drawing.Point(719, 49);
            this.lblVolumeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblVolumeUnit.TabIndex = 20;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerDate
            // 
            this.tblLayPnlBuyInput.SetColumnSpan(this.dateTimePickerDate, 2);
            this.dateTimePickerDate.CustomFormat = "";
            this.dateTimePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dateTimePickerDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDate.Location = new System.Drawing.Point(251, 1);
            this.dateTimePickerDate.Margin = new System.Windows.Forms.Padding(1);
            this.dateTimePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(466, 22);
            this.dateTimePickerDate.TabIndex = 0;
            this.dateTimePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.dateTimePickerDate.Enter += new System.EventHandler(this.OnDatePickerDate_Enter);
            this.dateTimePickerDate.Leave += new System.EventHandler(this.OnDatePickerDate_Leave);
            // 
            // dateTimePickerTime
            // 
            this.tblLayPnlBuyInput.SetColumnSpan(this.dateTimePickerTime, 2);
            this.dateTimePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerTime.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTime.Location = new System.Drawing.Point(251, 25);
            this.dateTimePickerTime.Margin = new System.Windows.Forms.Padding(1);
            this.dateTimePickerTime.Name = "dateTimePickerTime";
            this.dateTimePickerTime.ShowUpDown = true;
            this.dateTimePickerTime.Size = new System.Drawing.Size(466, 22);
            this.dateTimePickerTime.TabIndex = 1;
            this.dateTimePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.dateTimePickerTime.Enter += new System.EventHandler(this.OnDatePickerTime_Enter);
            this.dateTimePickerTime.Leave += new System.EventHandler(this.OnDatePickerTime_Leave);
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxVolume, 2);
            this.txtBoxVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(251, 49);
            this.txtBoxVolume.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(466, 22);
            this.txtBoxVolume.TabIndex = 2;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolume_TextChanged);
            this.txtBoxVolume.Enter += new System.EventHandler(this.OnTxtBoxVolume_Enter);
            this.txtBoxVolume.Leave += new System.EventHandler(this.OnTxtBoxVolume_Leave);
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.Location = new System.Drawing.Point(1, 49);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(248, 22);
            this.lblVolume.TabIndex = 14;
            this.lblVolume.Text = "_addVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMarketValue
            // 
            this.lblMarketValue.BackColor = System.Drawing.Color.LightGray;
            this.lblMarketValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMarketValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarketValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarketValue.Location = new System.Drawing.Point(1, 121);
            this.lblMarketValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblMarketValue.Name = "lblMarketValue";
            this.lblMarketValue.Size = new System.Drawing.Size(248, 22);
            this.lblMarketValue.TabIndex = 27;
            this.lblMarketValue.Text = "_addMarketValue";
            this.lblMarketValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBuyPrice
            // 
            this.lblBuyPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblBuyPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBuyPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBuyPrice.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuyPrice.Location = new System.Drawing.Point(1, 97);
            this.lblBuyPrice.Margin = new System.Windows.Forms.Padding(1);
            this.lblBuyPrice.Name = "lblBuyPrice";
            this.lblBuyPrice.Size = new System.Drawing.Size(248, 22);
            this.lblBuyPrice.TabIndex = 18;
            this.lblBuyPrice.Text = "_addBuyPrice";
            this.lblBuyPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVolumeSold
            // 
            this.lblVolumeSold.BackColor = System.Drawing.Color.LightGray;
            this.lblVolumeSold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolumeSold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolumeSold.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeSold.Location = new System.Drawing.Point(1, 73);
            this.lblVolumeSold.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolumeSold.Name = "lblVolumeSold";
            this.lblVolumeSold.Size = new System.Drawing.Size(248, 22);
            this.lblVolumeSold.TabIndex = 29;
            this.lblVolumeSold.Text = "_addVolumeSold";
            this.lblVolumeSold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxMarketValue
            // 
            this.txtBoxMarketValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxMarketValue, 2);
            this.txtBoxMarketValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxMarketValue.Enabled = false;
            this.txtBoxMarketValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxMarketValue.Location = new System.Drawing.Point(251, 121);
            this.txtBoxMarketValue.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxMarketValue.Name = "txtBoxMarketValue";
            this.txtBoxMarketValue.ReadOnly = true;
            this.txtBoxMarketValue.Size = new System.Drawing.Size(466, 22);
            this.txtBoxMarketValue.TabIndex = 12;
            this.txtBoxMarketValue.TabStop = false;
            this.txtBoxMarketValue.Text = "-";
            // 
            // txtBoxSharePrice
            // 
            this.txtBoxSharePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxSharePrice, 2);
            this.txtBoxSharePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSharePrice.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSharePrice.Location = new System.Drawing.Point(251, 97);
            this.txtBoxSharePrice.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxSharePrice.Name = "txtBoxSharePrice";
            this.txtBoxSharePrice.Size = new System.Drawing.Size(466, 22);
            this.txtBoxSharePrice.TabIndex = 3;
            this.txtBoxSharePrice.TextChanged += new System.EventHandler(this.OnTxtBoxPrice_TextChanged);
            this.txtBoxSharePrice.Enter += new System.EventHandler(this.OnTxtBoxPrice_Enter);
            this.txtBoxSharePrice.Leave += new System.EventHandler(this.OnTxtBoxPrice_Leave);
            // 
            // txtBoxVolumeSold
            // 
            this.txtBoxVolumeSold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxVolumeSold, 2);
            this.txtBoxVolumeSold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolumeSold.Enabled = false;
            this.txtBoxVolumeSold.Location = new System.Drawing.Point(251, 73);
            this.txtBoxVolumeSold.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxVolumeSold.Name = "txtBoxVolumeSold";
            this.txtBoxVolumeSold.ReadOnly = true;
            this.txtBoxVolumeSold.Size = new System.Drawing.Size(466, 22);
            this.txtBoxVolumeSold.TabIndex = 30;
            this.txtBoxVolumeSold.TabStop = false;
            this.txtBoxVolumeSold.Text = "-";
            this.txtBoxVolumeSold.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolumeSold_TextChanged);
            this.txtBoxVolumeSold.Leave += new System.EventHandler(this.OnTxtBoxVolumeSold_Leave);
            // 
            // lblMarketValueUnit
            // 
            this.lblMarketValueUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarketValueUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarketValueUnit.Location = new System.Drawing.Point(719, 121);
            this.lblMarketValueUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblMarketValueUnit.Name = "lblMarketValueUnit";
            this.lblMarketValueUnit.Size = new System.Drawing.Size(75, 22);
            this.lblMarketValueUnit.TabIndex = 28;
            this.lblMarketValueUnit.Text = "_lblMarketValueUnit";
            this.lblMarketValueUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPriceUnit
            // 
            this.lblPriceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPriceUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceUnit.Location = new System.Drawing.Point(719, 97);
            this.lblPriceUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblPriceUnit.Name = "lblPriceUnit";
            this.lblPriceUnit.Size = new System.Drawing.Size(75, 22);
            this.lblPriceUnit.TabIndex = 24;
            this.lblPriceUnit.Text = "_lbPriceUnit";
            this.lblPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVolumeSoldUnit
            // 
            this.lblVolumeSoldUnit.AutoSize = true;
            this.lblVolumeSoldUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolumeSoldUnit.Location = new System.Drawing.Point(719, 73);
            this.lblVolumeSoldUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolumeSoldUnit.Name = "lblVolumeSoldUnit";
            this.lblVolumeSoldUnit.Size = new System.Drawing.Size(75, 22);
            this.lblVolumeSoldUnit.TabIndex = 31;
            this.lblVolumeSoldUnit.Text = "_lblVolumeSoldUnit";
            this.lblVolumeSoldUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDocument
            // 
            this.lblDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocument.Location = new System.Drawing.Point(1, 289);
            this.lblDocument.Margin = new System.Windows.Forms.Padding(1);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(248, 22);
            this.lblDocument.TabIndex = 19;
            this.lblDocument.Text = "lblDocument";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxDocument, 2);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(251, 289);
            this.txtBoxDocument.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(466, 22);
            this.txtBoxDocument.TabIndex = 6;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Enter += new System.EventHandler(this.OnTxtBoxDocument_Enter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // btnDocumentBrowse
            // 
            this.tblLayPnlBuyInput.SetColumnSpan(this.btnDocumentBrowse, 2);
            this.btnDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.Image = global::SharePortfolioManager.Properties.Resources.menu_folder_open_16;
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(719, 289);
            this.btnDocumentBrowse.Margin = new System.Windows.Forms.Padding(1);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(99, 22);
            this.btnDocumentBrowse.TabIndex = 7;
            this.btnDocumentBrowse.Text = "...";
            this.btnDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnDocumentBrowse.Click += new System.EventHandler(this.OnBtnBuyDocumentBrowse_Click);
            // 
            // lblDepositUnit
            // 
            this.lblDepositUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDepositUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositUnit.Location = new System.Drawing.Point(719, 265);
            this.lblDepositUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblDepositUnit.Name = "lblDepositUnit";
            this.lblDepositUnit.Size = new System.Drawing.Size(75, 22);
            this.lblDepositUnit.TabIndex = 21;
            this.lblDepositUnit.Text = "_lblFinalValueUnit";
            this.lblDepositUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReductionUnit
            // 
            this.lblReductionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReductionUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReductionUnit.Location = new System.Drawing.Point(719, 217);
            this.lblReductionUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblReductionUnit.Name = "lblReductionUnit";
            this.lblReductionUnit.Size = new System.Drawing.Size(75, 22);
            this.lblReductionUnit.TabIndex = 23;
            this.lblReductionUnit.Text = "_lblReductionUnit";
            this.lblReductionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBrokerageUnit
            // 
            this.lblBrokerageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerageUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerageUnit.Location = new System.Drawing.Point(719, 241);
            this.lblBrokerageUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerageUnit.Name = "lblBrokerageUnit";
            this.lblBrokerageUnit.Size = new System.Drawing.Size(75, 22);
            this.lblBrokerageUnit.TabIndex = 22;
            this.lblBrokerageUnit.Text = "_lblBrokerageUnit";
            this.lblBrokerageUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDeposit
            // 
            this.lblDeposit.BackColor = System.Drawing.Color.LightGray;
            this.lblDeposit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDeposit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDeposit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeposit.Location = new System.Drawing.Point(1, 265);
            this.lblDeposit.Margin = new System.Windows.Forms.Padding(1);
            this.lblDeposit.Name = "lblDeposit";
            this.lblDeposit.Size = new System.Drawing.Size(248, 22);
            this.lblDeposit.TabIndex = 15;
            this.lblDeposit.Text = "_addFinalValue";
            this.lblDeposit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDeposit
            // 
            this.txtBoxDeposit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxDeposit, 2);
            this.txtBoxDeposit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDeposit.Enabled = false;
            this.txtBoxDeposit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDeposit.Location = new System.Drawing.Point(251, 265);
            this.txtBoxDeposit.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxDeposit.Name = "txtBoxDeposit";
            this.txtBoxDeposit.ReadOnly = true;
            this.txtBoxDeposit.Size = new System.Drawing.Size(466, 22);
            this.txtBoxDeposit.TabIndex = 13;
            this.txtBoxDeposit.TabStop = false;
            this.txtBoxDeposit.Text = "-";
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
            this.lblReduction.Size = new System.Drawing.Size(248, 22);
            this.lblReduction.TabIndex = 17;
            this.lblReduction.Text = "_addReduction";
            this.lblReduction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxReduction
            // 
            this.txtBoxReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxReduction, 2);
            this.txtBoxReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxReduction.Location = new System.Drawing.Point(251, 217);
            this.txtBoxReduction.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxReduction.Name = "txtBoxReduction";
            this.txtBoxReduction.Size = new System.Drawing.Size(466, 22);
            this.txtBoxReduction.TabIndex = 5;
            this.txtBoxReduction.TextChanged += new System.EventHandler(this.OnTxtBoxReduction_TextChanged);
            this.txtBoxReduction.Enter += new System.EventHandler(this.OnTxtBoxReduction_Enter);
            this.txtBoxReduction.Leave += new System.EventHandler(this.OnTxtBoxReduction_Leave);
            // 
            // lblBrokerage
            // 
            this.lblBrokerage.BackColor = System.Drawing.Color.LightGray;
            this.lblBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerage.Location = new System.Drawing.Point(1, 241);
            this.lblBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerage.Name = "lblBrokerage";
            this.lblBrokerage.Size = new System.Drawing.Size(248, 22);
            this.lblBrokerage.TabIndex = 16;
            this.lblBrokerage.Text = "_addBrokerage";
            this.lblBrokerage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxBrokerage
            // 
            this.txtBoxBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuyInput.SetColumnSpan(this.txtBoxBrokerage, 2);
            this.txtBoxBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBrokerage.Enabled = false;
            this.txtBoxBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBrokerage.Location = new System.Drawing.Point(251, 241);
            this.txtBoxBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxBrokerage.Name = "txtBoxBrokerage";
            this.txtBoxBrokerage.ReadOnly = true;
            this.txtBoxBrokerage.Size = new System.Drawing.Size(466, 22);
            this.txtBoxBrokerage.TabIndex = 4;
            this.txtBoxBrokerage.Text = "-";
            this.txtBoxBrokerage.TextChanged += new System.EventHandler(this.OnTxtBoxBrokerage_TextChanged);
            this.txtBoxBrokerage.Enter += new System.EventHandler(this.OnTxtBoxBrokerage_Enter);
            this.txtBoxBrokerage.Leave += new System.EventHandler(this.OnTxtBoxBrokerage_Leave);
            // 
            // picBoxDateParseState
            // 
            this.picBoxDateParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxDateParseState.Location = new System.Drawing.Point(798, 3);
            this.picBoxDateParseState.Name = "picBoxDateParseState";
            this.picBoxDateParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxDateParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxDateParseState.TabIndex = 42;
            this.picBoxDateParseState.TabStop = false;
            // 
            // picBoxTimeParseState
            // 
            this.picBoxTimeParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxTimeParseState.Location = new System.Drawing.Point(798, 27);
            this.picBoxTimeParseState.Name = "picBoxTimeParseState";
            this.picBoxTimeParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxTimeParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxTimeParseState.TabIndex = 43;
            this.picBoxTimeParseState.TabStop = false;
            // 
            // picBoxVolumeParseState
            // 
            this.picBoxVolumeParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxVolumeParseState.Location = new System.Drawing.Point(798, 51);
            this.picBoxVolumeParseState.Name = "picBoxVolumeParseState";
            this.picBoxVolumeParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxVolumeParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxVolumeParseState.TabIndex = 44;
            this.picBoxVolumeParseState.TabStop = false;
            // 
            // picBoxPriceParseState
            // 
            this.picBoxPriceParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxPriceParseState.Location = new System.Drawing.Point(798, 99);
            this.picBoxPriceParseState.Name = "picBoxPriceParseState";
            this.picBoxPriceParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxPriceParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxPriceParseState.TabIndex = 45;
            this.picBoxPriceParseState.TabStop = false;
            // 
            // picBoxProvisionParseState
            // 
            this.picBoxProvisionParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxProvisionParseState.Location = new System.Drawing.Point(798, 147);
            this.picBoxProvisionParseState.Name = "picBoxProvisionParseState";
            this.picBoxProvisionParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxProvisionParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxProvisionParseState.TabIndex = 46;
            this.picBoxProvisionParseState.TabStop = false;
            // 
            // picBoxBrokerFeeParseState
            // 
            this.picBoxBrokerFeeParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxBrokerFeeParseState.Location = new System.Drawing.Point(798, 171);
            this.picBoxBrokerFeeParseState.Name = "picBoxBrokerFeeParseState";
            this.picBoxBrokerFeeParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxBrokerFeeParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxBrokerFeeParseState.TabIndex = 47;
            this.picBoxBrokerFeeParseState.TabStop = false;
            // 
            // picBoxTraderPlaceFeeParseState
            // 
            this.picBoxTraderPlaceFeeParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxTraderPlaceFeeParseState.Location = new System.Drawing.Point(798, 195);
            this.picBoxTraderPlaceFeeParseState.Name = "picBoxTraderPlaceFeeParseState";
            this.picBoxTraderPlaceFeeParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxTraderPlaceFeeParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxTraderPlaceFeeParseState.TabIndex = 48;
            this.picBoxTraderPlaceFeeParseState.TabStop = false;
            // 
            // picBoxReductionParseState
            // 
            this.picBoxReductionParseState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxReductionParseState.Location = new System.Drawing.Point(798, 219);
            this.picBoxReductionParseState.Name = "picBoxReductionParseState";
            this.picBoxReductionParseState.Size = new System.Drawing.Size(18, 18);
            this.picBoxReductionParseState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxReductionParseState.TabIndex = 49;
            this.picBoxReductionParseState.TabStop = false;
            // 
            // statusStripMessages
            // 
            this.statusStripMessages.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStripMessages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageBuyEdit,
            this.toolStripProgressBarBuyDocumentParsing,
            this.toolStripStatusLabelMessageBuyDocumentParsing});
            this.statusStripMessages.Location = new System.Drawing.Point(3, 367);
            this.statusStripMessages.Name = "statusStripMessages";
            this.statusStripMessages.Size = new System.Drawing.Size(819, 22);
            this.statusStripMessages.TabIndex = 14;
            this.statusStripMessages.Text = "statusStripMessages";
            // 
            // toolStripStatusLabelMessageBuyEdit
            // 
            this.toolStripStatusLabelMessageBuyEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageBuyEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessageBuyEdit.Name = "toolStripStatusLabelMessageBuyEdit";
            this.toolStripStatusLabelMessageBuyEdit.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBarBuyDocumentParsing
            // 
            this.toolStripProgressBarBuyDocumentParsing.MarqueeAnimationSpeed = 20;
            this.toolStripProgressBarBuyDocumentParsing.Name = "toolStripProgressBarBuyDocumentParsing";
            this.toolStripProgressBarBuyDocumentParsing.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBarBuyDocumentParsing.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBarBuyDocumentParsing.Visible = false;
            // 
            // toolStripStatusLabelMessageBuyDocumentParsing
            // 
            this.toolStripStatusLabelMessageBuyDocumentParsing.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageBuyDocumentParsing.Name = "toolStripStatusLabelMessageBuyDocumentParsing";
            this.toolStripStatusLabelMessageBuyDocumentParsing.Size = new System.Drawing.Size(0, 17);
            // 
            // grpBoxBuys
            // 
            this.grpBoxBuys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxBuys.Controls.Add(this.tblLayPnlOverviewTabControl);
            this.grpBoxBuys.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxBuys.Location = new System.Drawing.Point(5, 403);
            this.grpBoxBuys.Name = "grpBoxBuys";
            this.grpBoxBuys.Size = new System.Drawing.Size(825, 126);
            this.grpBoxBuys.TabIndex = 16;
            this.grpBoxBuys.TabStop = false;
            this.grpBoxBuys.Text = "_grpBoxBuys";
            this.grpBoxBuys.MouseLeave += new System.EventHandler(this.OnGrpBoxOverview_MouseLeave);
            // 
            // tblLayPnlOverviewTabControl
            // 
            this.tblLayPnlOverviewTabControl.ColumnCount = 1;
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.Controls.Add(this.tabCtrlBuys, 0, 0);
            this.tblLayPnlOverviewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOverviewTabControl.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlOverviewTabControl.Name = "tblLayPnlOverviewTabControl";
            this.tblLayPnlOverviewTabControl.RowCount = 1;
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(819, 105);
            this.tblLayPnlOverviewTabControl.TabIndex = 0;
            // 
            // tabCtrlBuys
            // 
            this.tabCtrlBuys.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlBuys.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabCtrlBuys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlBuys.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlBuys.Name = "tabCtrlBuys";
            this.tabCtrlBuys.SelectedIndex = 0;
            this.tabCtrlBuys.Size = new System.Drawing.Size(813, 99);
            this.tabCtrlBuys.TabIndex = 0;
            this.tabCtrlBuys.TabStop = false;
            this.tabCtrlBuys.SelectedIndexChanged += new System.EventHandler(this.TabCtrlBuys_SelectedIndexChanged);
            this.tabCtrlBuys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTabCtrlBuys_KeyDown);
            this.tabCtrlBuys.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTabCtrlBuys_KeyPress);
            this.tabCtrlBuys.MouseLeave += new System.EventHandler(this.OnTabCtrlBuys_MouseLeave);
            // 
            // lblPurchaseValue
            // 
            this.lblPurchaseValue.BackColor = System.Drawing.Color.LightGray;
            this.lblPurchaseValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPurchaseValue.Location = new System.Drawing.Point(10, 177);
            this.lblPurchaseValue.Name = "lblPurchaseValue";
            this.lblPurchaseValue.Size = new System.Drawing.Size(365, 23);
            this.lblPurchaseValue.TabIndex = 15;
            this.lblPurchaseValue.Text = "_addFinalValue";
            this.lblPurchaseValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewBuyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 534);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxBuys);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 650);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 550);
            this.Name = "ViewBuyEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_ShareBuysEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareBuysEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareBuysEdit_Load);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
            this.tblLayPnlBuyButtons.ResumeLayout(false);
            this.tblLayPnlBuyInput.ResumeLayout(false);
            this.tblLayPnlBuyInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxDateParseState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTimeParseState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxVolumeParseState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxPriceParseState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxProvisionParseState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxBrokerFeeParseState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTraderPlaceFeeParseState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxReductionParseState)).EndInit();
            this.statusStripMessages.ResumeLayout(false);
            this.statusStripMessages.PerformLayout();
            this.grpBoxBuys.ResumeLayout(false);
            this.tblLayPnlOverviewTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxAdd;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.Label lblVolumeUnit;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TextBox txtBoxDocument;
        private System.Windows.Forms.Label lblDocument;
        private System.Windows.Forms.Button btnDocumentBrowse;
        private System.Windows.Forms.Label lblDeposit;
        private System.Windows.Forms.TextBox txtBoxDeposit;
        private System.Windows.Forms.Label lblDepositUnit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnAddSave;
        private System.Windows.Forms.StatusStrip statusStripMessages;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessageBuyEdit;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarBuyDocumentParsing;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessageBuyDocumentParsing;
        private System.Windows.Forms.GroupBox grpBoxBuys;
        private System.Windows.Forms.TabControl tabCtrlBuys;
        private System.Windows.Forms.Label lblPriceUnit;
        private System.Windows.Forms.TextBox txtBoxSharePrice;
        private System.Windows.Forms.Label lblBuyPrice;
        private System.Windows.Forms.DateTimePicker dateTimePickerTime;
        private System.Windows.Forms.Label lblBrokerageUnit;
        private System.Windows.Forms.TextBox txtBoxBrokerage;
        private System.Windows.Forms.Label lblBrokerage;
        private System.Windows.Forms.Label lblReductionUnit;
        private System.Windows.Forms.TextBox txtBoxReduction;
        private System.Windows.Forms.Label lblReduction;
        private System.Windows.Forms.Label lblMarketValueUnit;
        private System.Windows.Forms.TextBox txtBoxMarketValue;
        private System.Windows.Forms.Label lblMarketValue;
        private System.Windows.Forms.TextBox txtBoxVolume;
        private System.Windows.Forms.Label lblPurchaseValue;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlBuyInput;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlBuyButtons;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlOverviewTabControl;
        private System.Windows.Forms.Label lblVolumeSold;
        private System.Windows.Forms.TextBox txtBoxVolumeSold;
        private System.Windows.Forms.Label lblVolumeSoldUnit;
        private System.Windows.Forms.Label lblTraderPlaceFeeUnit;
        private System.Windows.Forms.Label lblBrokerFeeUnit;
        private System.Windows.Forms.Label lblProvisionUnit;
        private System.Windows.Forms.TextBox txtBoxTraderPlaceFee;
        private System.Windows.Forms.TextBox txtBoxBrokerFee;
        private System.Windows.Forms.TextBox txtBoxProvision;
        private System.Windows.Forms.Label lblTraderPlaceFee;
        private System.Windows.Forms.Label lblBrokerFee;
        private System.Windows.Forms.Label lblProvision;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.PictureBox picBoxDateParseState;
        private System.Windows.Forms.PictureBox picBoxTimeParseState;
        private System.Windows.Forms.PictureBox picBoxVolumeParseState;
        private System.Windows.Forms.PictureBox picBoxPriceParseState;
        private System.Windows.Forms.PictureBox picBoxProvisionParseState;
        private System.Windows.Forms.PictureBox picBoxBrokerFeeParseState;
        private System.Windows.Forms.PictureBox picBoxTraderPlaceFeeParseState;
        private System.Windows.Forms.PictureBox picBoxReductionParseState;
    }
}