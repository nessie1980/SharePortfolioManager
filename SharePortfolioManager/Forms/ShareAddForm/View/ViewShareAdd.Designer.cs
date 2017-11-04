namespace SharePortfolioManager.Forms.ShareAddForm.View
{
    partial class ViewShareAdd
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
            this.lblWkn = new System.Windows.Forms.Label();
            this.txtBoxName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblWebSite = new System.Windows.Forms.Label();
            this.txtBoxWebSite = new System.Windows.Forms.TextBox();
            this.lblMarketValue = new System.Windows.Forms.Label();
            this.txtBoxMarketValue = new System.Windows.Forms.TextBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.txtBoxVolume = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.addShareStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtBoxWkn = new System.Windows.Forms.TextBox();
            this.lblCultureInfo = new System.Windows.Forms.Label();
            this.cboBoxCultureInfo = new System.Windows.Forms.ComboBox();
            this.lblVolumeUnit = new System.Windows.Forms.Label();
            this.lblMarketValueUnit = new System.Windows.Forms.Label();
            this.lblDocument = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.btnDocumentBrowse = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.grpBoxGeneral = new System.Windows.Forms.GroupBox();
            this.lblFinalValue = new System.Windows.Forms.Label();
            this.txtBoxFinalValue = new System.Windows.Forms.TextBox();
            this.lblFinalValueUnit = new System.Windows.Forms.Label();
            this.lblSharePrice = new System.Windows.Forms.Label();
            this.txtBoxSharePrice = new System.Windows.Forms.TextBox();
            this.lblSharePriceUnit = new System.Windows.Forms.Label();
            this.lblReductionUnit = new System.Windows.Forms.Label();
            this.txtBoxReduction = new System.Windows.Forms.TextBox();
            this.lblReduction = new System.Windows.Forms.Label();
            this.lblCostsUnit = new System.Windows.Forms.Label();
            this.txtBoxCosts = new System.Windows.Forms.TextBox();
            this.lblCosts = new System.Windows.Forms.Label();
            this.cbxDividendPayoutInterval = new System.Windows.Forms.ComboBox();
            this.lblDividendPayoutInterval = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.grpBoxGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWkn
            // 
            this.lblWkn.BackColor = System.Drawing.Color.LightGray;
            this.lblWkn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWkn.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWkn.Location = new System.Drawing.Point(10, 22);
            this.lblWkn.Name = "lblWkn";
            this.lblWkn.Size = new System.Drawing.Size(365, 23);
            this.lblWkn.TabIndex = 21;
            this.lblWkn.Text = "_lblWkn";
            this.lblWkn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxName
            // 
            this.txtBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxName.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxName.Location = new System.Drawing.Point(380, 73);
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(370, 23);
            this.txtBoxName.TabIndex = 3;
            this.txtBoxName.TextChanged += new System.EventHandler(this.OnTxtBoxName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.LightGray;
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblName.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(10, 73);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(365, 23);
            this.lblName.TabIndex = 23;
            this.lblName.Text = "_lblName";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWebSite
            // 
            this.lblWebSite.BackColor = System.Drawing.Color.LightGray;
            this.lblWebSite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWebSite.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebSite.Location = new System.Drawing.Point(10, 253);
            this.lblWebSite.Name = "lblWebSite";
            this.lblWebSite.Size = new System.Drawing.Size(365, 23);
            this.lblWebSite.TabIndex = 27;
            this.lblWebSite.Text = "_lblWebSite";
            this.lblWebSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxWebSite
            // 
            this.txtBoxWebSite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxWebSite.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxWebSite.Location = new System.Drawing.Point(380, 253);
            this.txtBoxWebSite.Name = "txtBoxWebSite";
            this.txtBoxWebSite.Size = new System.Drawing.Size(370, 23);
            this.txtBoxWebSite.TabIndex = 8;
            this.txtBoxWebSite.TextChanged += new System.EventHandler(this.OnTxtBoxWebSite_TextChanged);
            // 
            // lblMarketValue
            // 
            this.lblMarketValue.BackColor = System.Drawing.Color.LightGray;
            this.lblMarketValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMarketValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarketValue.Location = new System.Drawing.Point(10, 150);
            this.lblMarketValue.Name = "lblMarketValue";
            this.lblMarketValue.Size = new System.Drawing.Size(365, 23);
            this.lblMarketValue.TabIndex = 26;
            this.lblMarketValue.Text = "_lblMarketValue";
            this.lblMarketValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxMarketValue
            // 
            this.txtBoxMarketValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxMarketValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxMarketValue.Location = new System.Drawing.Point(380, 150);
            this.txtBoxMarketValue.Name = "txtBoxMarketValue";
            this.txtBoxMarketValue.ReadOnly = true;
            this.txtBoxMarketValue.Size = new System.Drawing.Size(370, 23);
            this.txtBoxMarketValue.TabIndex = 40;
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.Location = new System.Drawing.Point(10, 98);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(365, 23);
            this.lblVolume.TabIndex = 24;
            this.lblVolume.Text = "_lblVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(380, 98);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(370, 23);
            this.txtBoxVolume.TabIndex = 4;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.OnTxtBoxVolume_TextChanged);
            this.txtBoxVolume.Leave += new System.EventHandler(this.OnTxtBoxVolume_Leave);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(486, 373);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(166, 31);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "_Add";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnBtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(658, 373);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(166, 31);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addShareStatusLabelMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 410);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(834, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // addShareStatusLabelMessage
            // 
            this.addShareStatusLabelMessage.Name = "addShareStatusLabelMessage";
            this.addShareStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // txtBoxWkn
            // 
            this.txtBoxWkn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxWkn.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxWkn.Location = new System.Drawing.Point(380, 22);
            this.txtBoxWkn.Name = "txtBoxWkn";
            this.txtBoxWkn.Size = new System.Drawing.Size(370, 23);
            this.txtBoxWkn.TabIndex = 0;
            this.txtBoxWkn.TextChanged += new System.EventHandler(this.OnTxtBoxWkn_TextChanged);
            // 
            // lblCultureInfo
            // 
            this.lblCultureInfo.BackColor = System.Drawing.Color.LightGray;
            this.lblCultureInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCultureInfo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCultureInfo.Location = new System.Drawing.Point(10, 279);
            this.lblCultureInfo.Name = "lblCultureInfo";
            this.lblCultureInfo.Size = new System.Drawing.Size(365, 23);
            this.lblCultureInfo.TabIndex = 28;
            this.lblCultureInfo.Text = "_lblCultureInfo";
            this.lblCultureInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboBoxCultureInfo
            // 
            this.cboBoxCultureInfo.BackColor = System.Drawing.Color.White;
            this.cboBoxCultureInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBoxCultureInfo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBoxCultureInfo.FormattingEnabled = true;
            this.cboBoxCultureInfo.Location = new System.Drawing.Point(380, 279);
            this.cboBoxCultureInfo.Name = "cboBoxCultureInfo";
            this.cboBoxCultureInfo.Size = new System.Drawing.Size(370, 23);
            this.cboBoxCultureInfo.TabIndex = 9;
            this.cboBoxCultureInfo.SelectedIndexChanged += new System.EventHandler(this.cboBoxCultureInfo_SelectedIndexChanged);
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeUnit.Location = new System.Drawing.Point(755, 98);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(50, 23);
            this.lblVolumeUnit.TabIndex = 31;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMarketValueUnit
            // 
            this.lblMarketValueUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarketValueUnit.Location = new System.Drawing.Point(755, 150);
            this.lblMarketValueUnit.Name = "lblMarketValueUnit";
            this.lblMarketValueUnit.Size = new System.Drawing.Size(50, 23);
            this.lblMarketValueUnit.TabIndex = 33;
            this.lblMarketValueUnit.Text = "_lblMarketValueUnit";
            this.lblMarketValueUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDocument
            // 
            this.lblDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocument.Location = new System.Drawing.Point(10, 331);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(365, 23);
            this.lblDocument.TabIndex = 30;
            this.lblDocument.Text = "_lblDocument";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(380, 331);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(370, 23);
            this.txtBoxDocument.TabIndex = 11;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // btnDocumentBrowse
            // 
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(757, 331);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnDocumentBrowse.TabIndex = 12;
            this.btnDocumentBrowse.Text = "...";
            this.btnDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnDocumentBrowse.Click += new System.EventHandler(this.OnBtnShareDocumentBrowse_Click);
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(10, 47);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(365, 23);
            this.lblDate.TabIndex = 22;
            this.lblDate.Text = "_lblDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(380, 47);
            this.datePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.ShowUpDown = true;
            this.datePickerDate.Size = new System.Drawing.Size(195, 23);
            this.datePickerDate.TabIndex = 1;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.datePickerDate_ValueChanged);
            // 
            // datePickerTime
            // 
            this.datePickerTime.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(581, 47);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(169, 23);
            this.datePickerTime.TabIndex = 2;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.datePickerTime_ValueChanged);
            // 
            // grpBoxGeneral
            // 
            this.grpBoxGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxGeneral.Controls.Add(this.lblFinalValue);
            this.grpBoxGeneral.Controls.Add(this.txtBoxFinalValue);
            this.grpBoxGeneral.Controls.Add(this.lblFinalValueUnit);
            this.grpBoxGeneral.Controls.Add(this.lblSharePrice);
            this.grpBoxGeneral.Controls.Add(this.txtBoxSharePrice);
            this.grpBoxGeneral.Controls.Add(this.lblSharePriceUnit);
            this.grpBoxGeneral.Controls.Add(this.lblReductionUnit);
            this.grpBoxGeneral.Controls.Add(this.txtBoxReduction);
            this.grpBoxGeneral.Controls.Add(this.lblReduction);
            this.grpBoxGeneral.Controls.Add(this.lblCostsUnit);
            this.grpBoxGeneral.Controls.Add(this.txtBoxCosts);
            this.grpBoxGeneral.Controls.Add(this.lblCosts);
            this.grpBoxGeneral.Controls.Add(this.cbxDividendPayoutInterval);
            this.grpBoxGeneral.Controls.Add(this.lblDividendPayoutInterval);
            this.grpBoxGeneral.Controls.Add(this.lblWkn);
            this.grpBoxGeneral.Controls.Add(this.datePickerTime);
            this.grpBoxGeneral.Controls.Add(this.txtBoxName);
            this.grpBoxGeneral.Controls.Add(this.datePickerDate);
            this.grpBoxGeneral.Controls.Add(this.lblName);
            this.grpBoxGeneral.Controls.Add(this.lblDate);
            this.grpBoxGeneral.Controls.Add(this.lblWebSite);
            this.grpBoxGeneral.Controls.Add(this.btnDocumentBrowse);
            this.grpBoxGeneral.Controls.Add(this.txtBoxWebSite);
            this.grpBoxGeneral.Controls.Add(this.txtBoxDocument);
            this.grpBoxGeneral.Controls.Add(this.lblMarketValue);
            this.grpBoxGeneral.Controls.Add(this.lblDocument);
            this.grpBoxGeneral.Controls.Add(this.txtBoxMarketValue);
            this.grpBoxGeneral.Controls.Add(this.lblVolumeUnit);
            this.grpBoxGeneral.Controls.Add(this.lblVolume);
            this.grpBoxGeneral.Controls.Add(this.lblMarketValueUnit);
            this.grpBoxGeneral.Controls.Add(this.txtBoxVolume);
            this.grpBoxGeneral.Controls.Add(this.cboBoxCultureInfo);
            this.grpBoxGeneral.Controls.Add(this.txtBoxWkn);
            this.grpBoxGeneral.Controls.Add(this.lblCultureInfo);
            this.grpBoxGeneral.Location = new System.Drawing.Point(8, 5);
            this.grpBoxGeneral.Name = "grpBoxGeneral";
            this.grpBoxGeneral.Size = new System.Drawing.Size(816, 362);
            this.grpBoxGeneral.TabIndex = 40;
            this.grpBoxGeneral.TabStop = false;
            this.grpBoxGeneral.Text = "grpBoxGeneral";
            // 
            // lblFinalValue
            // 
            this.lblFinalValue.BackColor = System.Drawing.Color.LightGray;
            this.lblFinalValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFinalValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalValue.Location = new System.Drawing.Point(10, 227);
            this.lblFinalValue.Name = "lblFinalValue";
            this.lblFinalValue.Size = new System.Drawing.Size(365, 23);
            this.lblFinalValue.TabIndex = 41;
            this.lblFinalValue.Text = "_lblFinalValue";
            this.lblFinalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxFinalValue
            // 
            this.txtBoxFinalValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxFinalValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxFinalValue.Location = new System.Drawing.Point(380, 227);
            this.txtBoxFinalValue.Name = "txtBoxFinalValue";
            this.txtBoxFinalValue.ReadOnly = true;
            this.txtBoxFinalValue.Size = new System.Drawing.Size(370, 23);
            this.txtBoxFinalValue.TabIndex = 41;
            // 
            // lblFinalValueUnit
            // 
            this.lblFinalValueUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalValueUnit.Location = new System.Drawing.Point(755, 227);
            this.lblFinalValueUnit.Name = "lblFinalValueUnit";
            this.lblFinalValueUnit.Size = new System.Drawing.Size(50, 23);
            this.lblFinalValueUnit.TabIndex = 42;
            this.lblFinalValueUnit.Text = "_lblFinalValueUnit";
            this.lblFinalValueUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSharePrice
            // 
            this.lblSharePrice.BackColor = System.Drawing.Color.LightGray;
            this.lblSharePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSharePrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharePrice.Location = new System.Drawing.Point(10, 124);
            this.lblSharePrice.Name = "lblSharePrice";
            this.lblSharePrice.Size = new System.Drawing.Size(365, 23);
            this.lblSharePrice.TabIndex = 38;
            this.lblSharePrice.Text = "_lblSharePrice";
            this.lblSharePrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxSharePrice
            // 
            this.txtBoxSharePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxSharePrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSharePrice.Location = new System.Drawing.Point(380, 124);
            this.txtBoxSharePrice.Name = "txtBoxSharePrice";
            this.txtBoxSharePrice.Size = new System.Drawing.Size(370, 23);
            this.txtBoxSharePrice.TabIndex = 5;
            this.txtBoxSharePrice.TextChanged += new System.EventHandler(this.OnTxtBoxSharePrice_TextChanged);
            this.txtBoxSharePrice.Leave += new System.EventHandler(this.OnTxtBoxSharePrice_Leave);
            // 
            // lblSharePriceUnit
            // 
            this.lblSharePriceUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharePriceUnit.Location = new System.Drawing.Point(755, 124);
            this.lblSharePriceUnit.Name = "lblSharePriceUnit";
            this.lblSharePriceUnit.Size = new System.Drawing.Size(50, 23);
            this.lblSharePriceUnit.TabIndex = 39;
            this.lblSharePriceUnit.Text = "_lblMarketValueUnit";
            this.lblSharePriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReductionUnit
            // 
            this.lblReductionUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReductionUnit.Location = new System.Drawing.Point(755, 202);
            this.lblReductionUnit.Name = "lblReductionUnit";
            this.lblReductionUnit.Size = new System.Drawing.Size(50, 23);
            this.lblReductionUnit.TabIndex = 36;
            this.lblReductionUnit.Text = "_lblReductionUnit";
            this.lblReductionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxReduction
            // 
            this.txtBoxReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxReduction.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxReduction.Location = new System.Drawing.Point(380, 202);
            this.txtBoxReduction.Name = "txtBoxReduction";
            this.txtBoxReduction.Size = new System.Drawing.Size(370, 23);
            this.txtBoxReduction.TabIndex = 7;
            this.txtBoxReduction.TextChanged += new System.EventHandler(this.OnTxtBoxReduction_TextChanged);
            this.txtBoxReduction.Leave += new System.EventHandler(this.OnTxtBoxReduction_Leave);
            // 
            // lblReduction
            // 
            this.lblReduction.BackColor = System.Drawing.Color.LightGray;
            this.lblReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReduction.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReduction.Location = new System.Drawing.Point(10, 202);
            this.lblReduction.Name = "lblReduction";
            this.lblReduction.Size = new System.Drawing.Size(365, 23);
            this.lblReduction.TabIndex = 34;
            this.lblReduction.Text = "_lblReduction";
            this.lblReduction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCostsUnit
            // 
            this.lblCostsUnit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostsUnit.Location = new System.Drawing.Point(755, 176);
            this.lblCostsUnit.Name = "lblCostsUnit";
            this.lblCostsUnit.Size = new System.Drawing.Size(50, 23);
            this.lblCostsUnit.TabIndex = 32;
            this.lblCostsUnit.Text = "_lblCostsUnit";
            this.lblCostsUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxCosts
            // 
            this.txtBoxCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxCosts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCosts.Location = new System.Drawing.Point(380, 176);
            this.txtBoxCosts.Name = "txtBoxCosts";
            this.txtBoxCosts.Size = new System.Drawing.Size(370, 23);
            this.txtBoxCosts.TabIndex = 6;
            this.txtBoxCosts.TextChanged += new System.EventHandler(this.OnTxtBoxCosts_TextChanged);
            this.txtBoxCosts.Leave += new System.EventHandler(this.OnTxtBoxCosts_Leave);
            // 
            // lblCosts
            // 
            this.lblCosts.BackColor = System.Drawing.Color.LightGray;
            this.lblCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCosts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCosts.Location = new System.Drawing.Point(10, 176);
            this.lblCosts.Name = "lblCosts";
            this.lblCosts.Size = new System.Drawing.Size(365, 23);
            this.lblCosts.TabIndex = 25;
            this.lblCosts.Text = "_lblCosts";
            this.lblCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxDividendPayoutInterval
            // 
            this.cbxDividendPayoutInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDividendPayoutInterval.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxDividendPayoutInterval.FormattingEnabled = true;
            this.cbxDividendPayoutInterval.Location = new System.Drawing.Point(380, 305);
            this.cbxDividendPayoutInterval.Name = "cbxDividendPayoutInterval";
            this.cbxDividendPayoutInterval.Size = new System.Drawing.Size(370, 23);
            this.cbxDividendPayoutInterval.TabIndex = 10;
            this.cbxDividendPayoutInterval.SelectedIndexChanged += new System.EventHandler(this.cbxDividendPayoutInterval_SelectedIndexChanged);
            // 
            // lblDividendPayoutInterval
            // 
            this.lblDividendPayoutInterval.BackColor = System.Drawing.Color.LightGray;
            this.lblDividendPayoutInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDividendPayoutInterval.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDividendPayoutInterval.Location = new System.Drawing.Point(10, 305);
            this.lblDividendPayoutInterval.Name = "lblDividendPayoutInterval";
            this.lblDividendPayoutInterval.Size = new System.Drawing.Size(365, 23);
            this.lblDividendPayoutInterval.TabIndex = 29;
            this.lblDividendPayoutInterval.Text = "_lblDividendPayoutInterval";
            this.lblDividendPayoutInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewShareAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 432);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxGeneral);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(850, 470);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 470);
            this.Name = "ViewShareAdd";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ShareAdd";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmShareAdd_FormClosing);
            this.Load += new System.EventHandler(this.FrmShareAdd_Load);
            this.Shown += new System.EventHandler(this.FrmShareAdd_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpBoxGeneral.ResumeLayout(false);
            this.grpBoxGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWkn;
        private System.Windows.Forms.TextBox txtBoxName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblWebSite;
        private System.Windows.Forms.Label lblMarketValue;
        private System.Windows.Forms.TextBox txtBoxMarketValue;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TextBox txtBoxVolume;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox txtBoxWkn;
        private System.Windows.Forms.Label lblCultureInfo;
        private System.Windows.Forms.ComboBox cboBoxCultureInfo;
        private System.Windows.Forms.Label lblVolumeUnit;
        private System.Windows.Forms.Label lblMarketValueUnit;
        private System.Windows.Forms.Label lblDocument;
        private System.Windows.Forms.TextBox txtBoxDocument;
        private System.Windows.Forms.Button btnDocumentBrowse;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker datePickerDate;
        private System.Windows.Forms.DateTimePicker datePickerTime;
        private System.Windows.Forms.TextBox txtBoxWebSite;
        private System.Windows.Forms.GroupBox grpBoxGeneral;
        private System.Windows.Forms.ComboBox cbxDividendPayoutInterval;
        private System.Windows.Forms.Label lblDividendPayoutInterval;
        private System.Windows.Forms.Label lblCostsUnit;
        private System.Windows.Forms.TextBox txtBoxCosts;
        private System.Windows.Forms.Label lblCosts;
        private System.Windows.Forms.Label lblReductionUnit;
        private System.Windows.Forms.TextBox txtBoxReduction;
        private System.Windows.Forms.Label lblReduction;
        private System.Windows.Forms.Label lblSharePrice;
        private System.Windows.Forms.TextBox txtBoxSharePrice;
        private System.Windows.Forms.Label lblSharePriceUnit;
        private System.Windows.Forms.Label lblFinalValue;
        private System.Windows.Forms.TextBox txtBoxFinalValue;
        private System.Windows.Forms.Label lblFinalValueUnit;
        private System.Windows.Forms.ToolStripStatusLabel addShareStatusLabelMessage;
    }
}