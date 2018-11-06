namespace SharePortfolioManager.Forms.BuysForm.View
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
            this.tblLayPnlBuynput = new System.Windows.Forms.TableLayoutPanel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblAddVolumeUnit = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblFinalValue = new System.Windows.Forms.Label();
            this.lblReduction = new System.Windows.Forms.Label();
            this.lblBrokerage = new System.Windows.Forms.Label();
            this.lblMarketValue = new System.Windows.Forms.Label();
            this.lblBuyPrice = new System.Windows.Forms.Label();
            this.lblVolumeSold = new System.Windows.Forms.Label();
            this.lblDocument = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.txtBoxFinalValue = new System.Windows.Forms.TextBox();
            this.txtBoxReduction = new System.Windows.Forms.TextBox();
            this.txtBoxBrokerage = new System.Windows.Forms.TextBox();
            this.txtBoxMarketValue = new System.Windows.Forms.TextBox();
            this.txtBoxPrice = new System.Windows.Forms.TextBox();
            this.txtBoxVolumeSold = new System.Windows.Forms.TextBox();
            this.btnDocumentBrowse = new System.Windows.Forms.Button();
            this.lblAddFinalValueUnit = new System.Windows.Forms.Label();
            this.lblAddReductionUnit = new System.Windows.Forms.Label();
            this.lblAddBrokerageUnit = new System.Windows.Forms.Label();
            this.lblAddMarketValueUnit = new System.Windows.Forms.Label();
            this.lblAddPriceUnit = new System.Windows.Forms.Label();
            this.lblAddVolumeSoldUnit = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageBuyEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxOverview = new System.Windows.Forms.GroupBox();
            this.tblLayPnlOverviewTabControl = new System.Windows.Forms.TableLayoutPanel();
            this.tabCtrlBuys = new System.Windows.Forms.TabControl();
            this.lblPurchaseValue = new System.Windows.Forms.Label();
            this.grpBoxAdd.SuspendLayout();
            this.tblLayPnlBuyButtons.SuspendLayout();
            this.tblLayPnlBuynput.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.grpBoxOverview.SuspendLayout();
            this.tblLayPnlOverviewTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBuyButtons);
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBuynput);
            this.grpBoxAdd.Controls.Add(this.statusStrip1);
            this.grpBoxAdd.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(825, 296);
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
            this.tblLayPnlBuyButtons.Location = new System.Drawing.Point(2, 236);
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
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(460, 1);
            this.btnReset.Margin = new System.Windows.Forms.Padding(1);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(178, 31);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
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
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
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
            this.btnAddSave.TabIndex = 8;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
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
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // tblLayPnlBuynput
            // 
            this.tblLayPnlBuynput.ColumnCount = 4;
            this.tblLayPnlBuynput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tblLayPnlBuynput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBuynput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBuynput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlBuynput.Controls.Add(this.lblDate, 0, 0);
            this.tblLayPnlBuynput.Controls.Add(this.lblAddVolumeUnit, 3, 1);
            this.tblLayPnlBuynput.Controls.Add(this.datePickerDate, 1, 0);
            this.tblLayPnlBuynput.Controls.Add(this.datePickerTime, 2, 0);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxVolume, 1, 1);
            this.tblLayPnlBuynput.Controls.Add(this.lblVolume, 0, 1);
            this.tblLayPnlBuynput.Controls.Add(this.lblFinalValue, 0, 7);
            this.tblLayPnlBuynput.Controls.Add(this.lblReduction, 0, 6);
            this.tblLayPnlBuynput.Controls.Add(this.lblBrokerage, 0, 5);
            this.tblLayPnlBuynput.Controls.Add(this.lblMarketValue, 0, 4);
            this.tblLayPnlBuynput.Controls.Add(this.lblBuyPrice, 0, 3);
            this.tblLayPnlBuynput.Controls.Add(this.lblVolumeSold, 0, 2);
            this.tblLayPnlBuynput.Controls.Add(this.lblDocument, 0, 8);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxDocument, 1, 8);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxFinalValue, 1, 7);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxReduction, 1, 6);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxBrokerage, 1, 5);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxMarketValue, 1, 4);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxPrice, 1, 3);
            this.tblLayPnlBuynput.Controls.Add(this.txtBoxVolumeSold, 1, 2);
            this.tblLayPnlBuynput.Controls.Add(this.btnDocumentBrowse, 3, 8);
            this.tblLayPnlBuynput.Controls.Add(this.lblAddFinalValueUnit, 3, 7);
            this.tblLayPnlBuynput.Controls.Add(this.lblAddReductionUnit, 3, 6);
            this.tblLayPnlBuynput.Controls.Add(this.lblAddBrokerageUnit, 3, 5);
            this.tblLayPnlBuynput.Controls.Add(this.lblAddMarketValueUnit, 3, 4);
            this.tblLayPnlBuynput.Controls.Add(this.lblAddPriceUnit, 3, 3);
            this.tblLayPnlBuynput.Controls.Add(this.lblAddVolumeSoldUnit, 3, 2);
            this.tblLayPnlBuynput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlBuynput.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlBuynput.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlBuynput.Name = "tblLayPnlBuynput";
            this.tblLayPnlBuynput.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tblLayPnlBuynput.RowCount = 9;
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBuynput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlBuynput.Size = new System.Drawing.Size(819, 216);
            this.tblLayPnlBuynput.TabIndex = 15;
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(4, 1);
            this.lblDate.Margin = new System.Windows.Forms.Padding(1);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(248, 22);
            this.lblDate.TabIndex = 13;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddVolumeUnit
            // 
            this.lblAddVolumeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddVolumeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddVolumeUnit.Location = new System.Drawing.Point(740, 25);
            this.lblAddVolumeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddVolumeUnit.Name = "lblAddVolumeUnit";
            this.lblAddVolumeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddVolumeUnit.TabIndex = 20;
            this.lblAddVolumeUnit.Text = "_lblVolumeUnit";
            this.lblAddVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(254, 1);
            this.datePickerDate.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(241, 22);
            this.datePickerDate.TabIndex = 0;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
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
            this.datePickerTime.Size = new System.Drawing.Size(241, 22);
            this.datePickerTime.TabIndex = 1;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Leave += new System.EventHandler(this.OnDatePickerTime_Leave);
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxVolume, 2);
            this.txtBoxVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(254, 25);
            this.txtBoxVolume.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(484, 22);
            this.txtBoxVolume.TabIndex = 2;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolume_TextChanged);
            this.txtBoxVolume.Leave += new System.EventHandler(this.OnTxtBoxVolume_Leave);
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.Location = new System.Drawing.Point(4, 25);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(248, 22);
            this.lblVolume.TabIndex = 14;
            this.lblVolume.Text = "_addVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFinalValue
            // 
            this.lblFinalValue.BackColor = System.Drawing.Color.LightGray;
            this.lblFinalValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFinalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinalValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalValue.Location = new System.Drawing.Point(4, 169);
            this.lblFinalValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblFinalValue.Name = "lblFinalValue";
            this.lblFinalValue.Size = new System.Drawing.Size(248, 22);
            this.lblFinalValue.TabIndex = 15;
            this.lblFinalValue.Text = "_addFinalValue";
            this.lblFinalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReduction
            // 
            this.lblReduction.BackColor = System.Drawing.Color.LightGray;
            this.lblReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReduction.Location = new System.Drawing.Point(4, 145);
            this.lblReduction.Margin = new System.Windows.Forms.Padding(1);
            this.lblReduction.Name = "lblReduction";
            this.lblReduction.Size = new System.Drawing.Size(248, 22);
            this.lblReduction.TabIndex = 17;
            this.lblReduction.Text = "_addReduction";
            this.lblReduction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBrokerage
            // 
            this.lblBrokerage.BackColor = System.Drawing.Color.LightGray;
            this.lblBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerage.Location = new System.Drawing.Point(4, 121);
            this.lblBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerage.Name = "lblBrokerage";
            this.lblBrokerage.Size = new System.Drawing.Size(248, 22);
            this.lblBrokerage.TabIndex = 16;
            this.lblBrokerage.Text = "_addBrokerage";
            this.lblBrokerage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMarketValue
            // 
            this.lblMarketValue.BackColor = System.Drawing.Color.LightGray;
            this.lblMarketValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMarketValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarketValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarketValue.Location = new System.Drawing.Point(4, 97);
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
            this.lblBuyPrice.Location = new System.Drawing.Point(4, 73);
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
            this.lblVolumeSold.Location = new System.Drawing.Point(4, 49);
            this.lblVolumeSold.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolumeSold.Name = "lblVolumeSold";
            this.lblVolumeSold.Size = new System.Drawing.Size(248, 22);
            this.lblVolumeSold.TabIndex = 29;
            this.lblVolumeSold.Text = "_addVolumeSold";
            this.lblVolumeSold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDocument
            // 
            this.lblDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocument.Location = new System.Drawing.Point(4, 193);
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
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxDocument, 2);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(254, 193);
            this.txtBoxDocument.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(484, 22);
            this.txtBoxDocument.TabIndex = 6;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // txtBoxFinalValue
            // 
            this.txtBoxFinalValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxFinalValue, 2);
            this.txtBoxFinalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxFinalValue.Enabled = false;
            this.txtBoxFinalValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxFinalValue.Location = new System.Drawing.Point(254, 169);
            this.txtBoxFinalValue.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxFinalValue.Name = "txtBoxFinalValue";
            this.txtBoxFinalValue.ReadOnly = true;
            this.txtBoxFinalValue.Size = new System.Drawing.Size(484, 22);
            this.txtBoxFinalValue.TabIndex = 13;
            this.txtBoxFinalValue.TabStop = false;
            this.txtBoxFinalValue.Text = "-";
            // 
            // txtBoxReduction
            // 
            this.txtBoxReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxReduction, 2);
            this.txtBoxReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxReduction.Location = new System.Drawing.Point(254, 145);
            this.txtBoxReduction.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxReduction.Name = "txtBoxReduction";
            this.txtBoxReduction.Size = new System.Drawing.Size(484, 22);
            this.txtBoxReduction.TabIndex = 5;
            this.txtBoxReduction.TextChanged += new System.EventHandler(this.OnTxtBoxReduction_TextChanged);
            this.txtBoxReduction.Leave += new System.EventHandler(this.OnTxtBoxReduction_Leave);
            // 
            // txtBoxBrokerage
            // 
            this.txtBoxBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxBrokerage, 2);
            this.txtBoxBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBrokerage.Location = new System.Drawing.Point(254, 121);
            this.txtBoxBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxBrokerage.Name = "txtBoxBrokerage";
            this.txtBoxBrokerage.Size = new System.Drawing.Size(484, 22);
            this.txtBoxBrokerage.TabIndex = 4;
            this.txtBoxBrokerage.TextChanged += new System.EventHandler(this.OnTxtBoxBrokerage_TextChanged);
            this.txtBoxBrokerage.Leave += new System.EventHandler(this.OnTxtBoxBrokerage_Leave);
            // 
            // txtBoxMarketValue
            // 
            this.txtBoxMarketValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxMarketValue, 2);
            this.txtBoxMarketValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxMarketValue.Enabled = false;
            this.txtBoxMarketValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxMarketValue.Location = new System.Drawing.Point(254, 97);
            this.txtBoxMarketValue.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxMarketValue.Name = "txtBoxMarketValue";
            this.txtBoxMarketValue.ReadOnly = true;
            this.txtBoxMarketValue.Size = new System.Drawing.Size(484, 22);
            this.txtBoxMarketValue.TabIndex = 12;
            this.txtBoxMarketValue.TabStop = false;
            this.txtBoxMarketValue.Text = "-";
            // 
            // txtBoxPrice
            // 
            this.txtBoxPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxPrice, 2);
            this.txtBoxPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxPrice.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPrice.Location = new System.Drawing.Point(254, 73);
            this.txtBoxPrice.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxPrice.Name = "txtBoxPrice";
            this.txtBoxPrice.Size = new System.Drawing.Size(484, 22);
            this.txtBoxPrice.TabIndex = 3;
            this.txtBoxPrice.TextChanged += new System.EventHandler(this.OnTxtBoxPrice_TextChanged);
            this.txtBoxPrice.Leave += new System.EventHandler(this.OnTxtBoxPrice_Leave);
            // 
            // txtBoxVolumeSold
            // 
            this.txtBoxVolumeSold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBuynput.SetColumnSpan(this.txtBoxVolumeSold, 2);
            this.txtBoxVolumeSold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolumeSold.Enabled = false;
            this.txtBoxVolumeSold.Location = new System.Drawing.Point(254, 49);
            this.txtBoxVolumeSold.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxVolumeSold.Name = "txtBoxVolumeSold";
            this.txtBoxVolumeSold.ReadOnly = true;
            this.txtBoxVolumeSold.Size = new System.Drawing.Size(484, 22);
            this.txtBoxVolumeSold.TabIndex = 30;
            this.txtBoxVolumeSold.TabStop = false;
            this.txtBoxVolumeSold.Text = "-";
            this.txtBoxVolumeSold.TextChanged += new System.EventHandler(this.OnTxtBoxAddVolumeSold_TextChanged);
            this.txtBoxVolumeSold.Leave += new System.EventHandler(this.OnTxtBoxVolumeSold_Leave);
            // 
            // btnDocumentBrowse
            // 
            this.btnDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(740, 193);
            this.btnDocumentBrowse.Margin = new System.Windows.Forms.Padding(1);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(75, 22);
            this.btnDocumentBrowse.TabIndex = 7;
            this.btnDocumentBrowse.Text = "...";
            this.btnDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnDocumentBrowse.Click += new System.EventHandler(this.OnBtnBuyDocumentBrowse_Click);
            // 
            // lblAddFinalValueUnit
            // 
            this.lblAddFinalValueUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddFinalValueUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddFinalValueUnit.Location = new System.Drawing.Point(740, 169);
            this.lblAddFinalValueUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddFinalValueUnit.Name = "lblAddFinalValueUnit";
            this.lblAddFinalValueUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddFinalValueUnit.TabIndex = 21;
            this.lblAddFinalValueUnit.Text = "_lblFinalValueUnit";
            this.lblAddFinalValueUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddReductionUnit
            // 
            this.lblAddReductionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddReductionUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddReductionUnit.Location = new System.Drawing.Point(740, 145);
            this.lblAddReductionUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddReductionUnit.Name = "lblAddReductionUnit";
            this.lblAddReductionUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddReductionUnit.TabIndex = 23;
            this.lblAddReductionUnit.Text = "_lblReductionUnit";
            this.lblAddReductionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddBrokerageUnit
            // 
            this.lblAddBrokerageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddBrokerageUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddBrokerageUnit.Location = new System.Drawing.Point(740, 121);
            this.lblAddBrokerageUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddBrokerageUnit.Name = "lblAddBrokerageUnit";
            this.lblAddBrokerageUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddBrokerageUnit.TabIndex = 22;
            this.lblAddBrokerageUnit.Text = "_lblBrokerageUnit";
            this.lblAddBrokerageUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddMarketValueUnit
            // 
            this.lblAddMarketValueUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddMarketValueUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddMarketValueUnit.Location = new System.Drawing.Point(740, 97);
            this.lblAddMarketValueUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddMarketValueUnit.Name = "lblAddMarketValueUnit";
            this.lblAddMarketValueUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddMarketValueUnit.TabIndex = 28;
            this.lblAddMarketValueUnit.Text = "_lblMarketValueUnit";
            this.lblAddMarketValueUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddPriceUnit
            // 
            this.lblAddPriceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddPriceUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddPriceUnit.Location = new System.Drawing.Point(740, 73);
            this.lblAddPriceUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddPriceUnit.Name = "lblAddPriceUnit";
            this.lblAddPriceUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddPriceUnit.TabIndex = 24;
            this.lblAddPriceUnit.Text = "_lbPriceUnit";
            this.lblAddPriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddVolumeSoldUnit
            // 
            this.lblAddVolumeSoldUnit.AutoSize = true;
            this.lblAddVolumeSoldUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddVolumeSoldUnit.Location = new System.Drawing.Point(740, 49);
            this.lblAddVolumeSoldUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddVolumeSoldUnit.Name = "lblAddVolumeSoldUnit";
            this.lblAddVolumeSoldUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddVolumeSoldUnit.TabIndex = 31;
            this.lblAddVolumeSoldUnit.Text = "_lblVolumeSoldUnit";
            this.lblAddVolumeSoldUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageBuyEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 271);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessageBuyEdit
            // 
            this.toolStripStatusLabelMessageBuyEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageBuyEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessageBuyEdit.Name = "toolStripStatusLabelMessageBuyEdit";
            this.toolStripStatusLabelMessageBuyEdit.Size = new System.Drawing.Size(0, 17);
            // 
            // grpBoxOverview
            // 
            this.grpBoxOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxOverview.Controls.Add(this.tblLayPnlOverviewTabControl);
            this.grpBoxOverview.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxOverview.Location = new System.Drawing.Point(5, 307);
            this.grpBoxOverview.Name = "grpBoxOverview";
            this.grpBoxOverview.Size = new System.Drawing.Size(825, 209);
            this.grpBoxOverview.TabIndex = 16;
            this.grpBoxOverview.TabStop = false;
            this.grpBoxOverview.Text = "_grpBoxOverview";
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
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(819, 188);
            this.tblLayPnlOverviewTabControl.TabIndex = 0;
            // 
            // tabCtrlBuys
            // 
            this.tabCtrlBuys.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlBuys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlBuys.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlBuys.Name = "tabCtrlBuys";
            this.tabCtrlBuys.SelectedIndex = 0;
            this.tabCtrlBuys.Size = new System.Drawing.Size(813, 182);
            this.tabCtrlBuys.TabIndex = 0;
            this.tabCtrlBuys.TabStop = false;
            this.tabCtrlBuys.SelectedIndexChanged += new System.EventHandler(this.TabCtrlBuys_SelectedIndexChanged);
            this.tabCtrlBuys.MouseEnter += new System.EventHandler(this.TabCtrlBuys_MouseEnter);
            this.tabCtrlBuys.MouseLeave += new System.EventHandler(this.TabCtrlBuys_MouseLeave);
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
            this.ClientSize = new System.Drawing.Size(834, 519);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxOverview);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 563);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 469);
            this.Name = "ViewBuyEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_ShareBuysEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareBuysEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareBuysEdit_Load);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
            this.tblLayPnlBuyButtons.ResumeLayout(false);
            this.tblLayPnlBuynput.ResumeLayout(false);
            this.tblLayPnlBuynput.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpBoxOverview.ResumeLayout(false);
            this.tblLayPnlOverviewTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxAdd;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker datePickerDate;
        private System.Windows.Forms.Label lblAddVolumeUnit;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TextBox txtBoxDocument;
        private System.Windows.Forms.Label lblDocument;
        private System.Windows.Forms.Button btnDocumentBrowse;
        private System.Windows.Forms.Label lblFinalValue;
        private System.Windows.Forms.TextBox txtBoxFinalValue;
        private System.Windows.Forms.Label lblAddFinalValueUnit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnAddSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox grpBoxOverview;
        private System.Windows.Forms.TabControl tabCtrlBuys;
        private System.Windows.Forms.Label lblAddPriceUnit;
        private System.Windows.Forms.TextBox txtBoxPrice;
        private System.Windows.Forms.Label lblBuyPrice;
        private System.Windows.Forms.DateTimePicker datePickerTime;
        private System.Windows.Forms.Label lblAddBrokerageUnit;
        private System.Windows.Forms.TextBox txtBoxBrokerage;
        private System.Windows.Forms.Label lblBrokerage;
        private System.Windows.Forms.Label lblAddReductionUnit;
        private System.Windows.Forms.TextBox txtBoxReduction;
        private System.Windows.Forms.Label lblReduction;
        private System.Windows.Forms.Label lblAddMarketValueUnit;
        private System.Windows.Forms.TextBox txtBoxMarketValue;
        private System.Windows.Forms.Label lblMarketValue;
        private System.Windows.Forms.TextBox txtBoxVolume;
        private System.Windows.Forms.Label lblPurchaseValue;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessageBuyEdit;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlBuynput;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlBuyButtons;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlOverviewTabControl;
        private System.Windows.Forms.Label lblVolumeSold;
        private System.Windows.Forms.TextBox txtBoxVolumeSold;
        private System.Windows.Forms.Label lblAddVolumeSoldUnit;
    }
}