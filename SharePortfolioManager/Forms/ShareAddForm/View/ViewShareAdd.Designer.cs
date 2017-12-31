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
            this.tblLayPnlAddShareButtons = new System.Windows.Forms.TableLayoutPanel();
            this.tblLayPnlAddShareInput = new System.Windows.Forms.TableLayoutPanel();
            this.cbxShareType = new System.Windows.Forms.ComboBox();
            this.lblShareType = new System.Windows.Forms.Label();
            this.txtBoxFinalValue = new System.Windows.Forms.TextBox();
            this.lblFinalValueUnit = new System.Windows.Forms.Label();
            this.cbxDividendPayoutInterval = new System.Windows.Forms.ComboBox();
            this.lblFinalValue = new System.Windows.Forms.Label();
            this.lblDividendPayoutInterval = new System.Windows.Forms.Label();
            this.lblReductionUnit = new System.Windows.Forms.Label();
            this.lblSharePriceUnit = new System.Windows.Forms.Label();
            this.txtBoxReduction = new System.Windows.Forms.TextBox();
            this.txtBoxSharePrice = new System.Windows.Forms.TextBox();
            this.lblReduction = new System.Windows.Forms.Label();
            this.lblSharePrice = new System.Windows.Forms.Label();
            this.lblCostsUnit = new System.Windows.Forms.Label();
            this.txtBoxCosts = new System.Windows.Forms.TextBox();
            this.lblCosts = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.grpBoxGeneral.SuspendLayout();
            this.tblLayPnlAddShareButtons.SuspendLayout();
            this.tblLayPnlAddShareInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWkn
            // 
            this.lblWkn.BackColor = System.Drawing.Color.LightGray;
            this.lblWkn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWkn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWkn.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWkn.Location = new System.Drawing.Point(1, 1);
            this.lblWkn.Margin = new System.Windows.Forms.Padding(1);
            this.lblWkn.Name = "lblWkn";
            this.lblWkn.Size = new System.Drawing.Size(208, 22);
            this.lblWkn.TabIndex = 21;
            this.lblWkn.Text = "_lblWkn";
            this.lblWkn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxName
            // 
            this.txtBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxName, 2);
            this.txtBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxName.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxName.Location = new System.Drawing.Point(211, 49);
            this.txtBoxName.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(530, 22);
            this.txtBoxName.TabIndex = 3;
            this.txtBoxName.TextChanged += new System.EventHandler(this.OnTxtBoxName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.LightGray;
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(1, 49);
            this.lblName.Margin = new System.Windows.Forms.Padding(1);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(208, 22);
            this.lblName.TabIndex = 23;
            this.lblName.Text = "_lblName";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWebSite
            // 
            this.lblWebSite.BackColor = System.Drawing.Color.LightGray;
            this.lblWebSite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWebSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebSite.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebSite.Location = new System.Drawing.Point(1, 217);
            this.lblWebSite.Margin = new System.Windows.Forms.Padding(1);
            this.lblWebSite.Name = "lblWebSite";
            this.lblWebSite.Size = new System.Drawing.Size(208, 22);
            this.lblWebSite.TabIndex = 27;
            this.lblWebSite.Text = "_lblWebSite";
            this.lblWebSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxWebSite
            // 
            this.txtBoxWebSite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxWebSite, 2);
            this.txtBoxWebSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxWebSite.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxWebSite.Location = new System.Drawing.Point(211, 217);
            this.txtBoxWebSite.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxWebSite.Name = "txtBoxWebSite";
            this.txtBoxWebSite.Size = new System.Drawing.Size(530, 22);
            this.txtBoxWebSite.TabIndex = 8;
            this.txtBoxWebSite.TextChanged += new System.EventHandler(this.OnTxtBoxWebSite_TextChanged);
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
            this.lblMarketValue.Size = new System.Drawing.Size(208, 22);
            this.lblMarketValue.TabIndex = 26;
            this.lblMarketValue.Text = "_lblMarketValue";
            this.lblMarketValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxMarketValue
            // 
            this.txtBoxMarketValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxMarketValue, 2);
            this.txtBoxMarketValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxMarketValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxMarketValue.Location = new System.Drawing.Point(211, 121);
            this.txtBoxMarketValue.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxMarketValue.Name = "txtBoxMarketValue";
            this.txtBoxMarketValue.ReadOnly = true;
            this.txtBoxMarketValue.Size = new System.Drawing.Size(530, 22);
            this.txtBoxMarketValue.TabIndex = 40;
            // 
            // lblVolume
            // 
            this.lblVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.Location = new System.Drawing.Point(1, 73);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(208, 22);
            this.lblVolume.TabIndex = 24;
            this.lblVolume.Text = "_lblVolume";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxVolume
            // 
            this.txtBoxVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxVolume, 2);
            this.txtBoxVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxVolume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxVolume.Location = new System.Drawing.Point(211, 73);
            this.txtBoxVolume.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxVolume.Name = "txtBoxVolume";
            this.txtBoxVolume.Size = new System.Drawing.Size(530, 22);
            this.txtBoxVolume.TabIndex = 4;
            this.txtBoxVolume.TextChanged += new System.EventHandler(this.OnTxtBoxVolume_TextChanged);
            this.txtBoxVolume.Leave += new System.EventHandler(this.OnTxtBoxVolume_Leave);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(460, 1);
            this.btnSave.Margin = new System.Windows.Forms.Padding(1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(178, 31);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "_Add";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnBtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(640, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(178, 31);
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
            this.statusStrip1.Location = new System.Drawing.Point(3, 390);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
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
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxWkn, 2);
            this.txtBoxWkn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxWkn.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxWkn.Location = new System.Drawing.Point(211, 1);
            this.txtBoxWkn.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxWkn.Name = "txtBoxWkn";
            this.txtBoxWkn.Size = new System.Drawing.Size(530, 22);
            this.txtBoxWkn.TabIndex = 0;
            this.txtBoxWkn.TextChanged += new System.EventHandler(this.OnTxtBoxWkn_TextChanged);
            // 
            // lblCultureInfo
            // 
            this.lblCultureInfo.BackColor = System.Drawing.Color.LightGray;
            this.lblCultureInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCultureInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCultureInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCultureInfo.Location = new System.Drawing.Point(1, 241);
            this.lblCultureInfo.Margin = new System.Windows.Forms.Padding(1);
            this.lblCultureInfo.Name = "lblCultureInfo";
            this.lblCultureInfo.Size = new System.Drawing.Size(208, 22);
            this.lblCultureInfo.TabIndex = 28;
            this.lblCultureInfo.Text = "_lblCultureInfo";
            this.lblCultureInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboBoxCultureInfo
            // 
            this.cboBoxCultureInfo.BackColor = System.Drawing.Color.White;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.cboBoxCultureInfo, 2);
            this.cboBoxCultureInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboBoxCultureInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBoxCultureInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBoxCultureInfo.FormattingEnabled = true;
            this.cboBoxCultureInfo.Location = new System.Drawing.Point(211, 241);
            this.cboBoxCultureInfo.Margin = new System.Windows.Forms.Padding(1);
            this.cboBoxCultureInfo.Name = "cboBoxCultureInfo";
            this.cboBoxCultureInfo.Size = new System.Drawing.Size(530, 22);
            this.cboBoxCultureInfo.TabIndex = 9;
            this.cboBoxCultureInfo.SelectedIndexChanged += new System.EventHandler(this.CboBoxCultureInfo_SelectedIndexChanged);
            // 
            // lblVolumeUnit
            // 
            this.lblVolumeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVolumeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeUnit.Location = new System.Drawing.Point(743, 73);
            this.lblVolumeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblVolumeUnit.Name = "lblVolumeUnit";
            this.lblVolumeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblVolumeUnit.TabIndex = 31;
            this.lblVolumeUnit.Text = "_lblVolumeUnit";
            this.lblVolumeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMarketValueUnit
            // 
            this.lblMarketValueUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarketValueUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarketValueUnit.Location = new System.Drawing.Point(743, 121);
            this.lblMarketValueUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblMarketValueUnit.Name = "lblMarketValueUnit";
            this.lblMarketValueUnit.Size = new System.Drawing.Size(75, 22);
            this.lblMarketValueUnit.TabIndex = 33;
            this.lblMarketValueUnit.Text = "_lblMarketValueUnit";
            this.lblMarketValueUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDocument
            // 
            this.lblDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocument.Location = new System.Drawing.Point(1, 313);
            this.lblDocument.Margin = new System.Windows.Forms.Padding(1);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(208, 22);
            this.lblDocument.TabIndex = 30;
            this.lblDocument.Text = "_lblDocument";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxDocument, 2);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(211, 313);
            this.txtBoxDocument.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(530, 22);
            this.txtBoxDocument.TabIndex = 11;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // btnDocumentBrowse
            // 
            this.btnDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(743, 313);
            this.btnDocumentBrowse.Margin = new System.Windows.Forms.Padding(1);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(75, 22);
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
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(1, 25);
            this.lblDate.Margin = new System.Windows.Forms.Padding(1);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(208, 22);
            this.lblDate.TabIndex = 22;
            this.lblDate.Text = "_lblDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(211, 25);
            this.datePickerDate.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerDate.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.ShowUpDown = true;
            this.datePickerDate.Size = new System.Drawing.Size(264, 22);
            this.datePickerDate.TabIndex = 1;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.DatePickerDate_ValueChanged);
            // 
            // datePickerTime
            // 
            this.datePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerTime.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.datePickerTime.Location = new System.Drawing.Point(477, 25);
            this.datePickerTime.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerTime.Name = "datePickerTime";
            this.datePickerTime.ShowUpDown = true;
            this.datePickerTime.Size = new System.Drawing.Size(264, 22);
            this.datePickerTime.TabIndex = 2;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.DatePickerTime_ValueChanged);
            // 
            // grpBoxGeneral
            // 
            this.grpBoxGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxGeneral.Controls.Add(this.tblLayPnlAddShareButtons);
            this.grpBoxGeneral.Controls.Add(this.statusStrip1);
            this.grpBoxGeneral.Controls.Add(this.tblLayPnlAddShareInput);
            this.grpBoxGeneral.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxGeneral.Location = new System.Drawing.Point(5, 5);
            this.grpBoxGeneral.Name = "grpBoxGeneral";
            this.grpBoxGeneral.Size = new System.Drawing.Size(825, 415);
            this.grpBoxGeneral.TabIndex = 40;
            this.grpBoxGeneral.TabStop = false;
            this.grpBoxGeneral.Text = "_grpBoxGeneral";
            // 
            // tblLayPnlAddShareButtons
            // 
            this.tblLayPnlAddShareButtons.ColumnCount = 3;
            this.tblLayPnlAddShareButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlAddShareButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlAddShareButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlAddShareButtons.Controls.Add(this.btnSave, 1, 0);
            this.tblLayPnlAddShareButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tblLayPnlAddShareButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlAddShareButtons.Location = new System.Drawing.Point(3, 354);
            this.tblLayPnlAddShareButtons.Name = "tblLayPnlAddShareButtons";
            this.tblLayPnlAddShareButtons.RowCount = 1;
            this.tblLayPnlAddShareButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlAddShareButtons.Size = new System.Drawing.Size(819, 33);
            this.tblLayPnlAddShareButtons.TabIndex = 44;
            // 
            // tblLayPnlAddShareInput
            // 
            this.tblLayPnlAddShareInput.ColumnCount = 4;
            this.tblLayPnlAddShareInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tblLayPnlAddShareInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlAddShareInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlAddShareInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlAddShareInput.Controls.Add(this.cbxShareType, 1, 12);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblShareType, 0, 12);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblWkn, 0, 0);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxFinalValue, 1, 8);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblFinalValueUnit, 3, 8);
            this.tblLayPnlAddShareInput.Controls.Add(this.cbxDividendPayoutInterval, 1, 11);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblFinalValue, 0, 8);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblDividendPayoutInterval, 0, 11);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxWkn, 1, 0);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblWebSite, 0, 9);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxWebSite, 1, 9);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblDate, 0, 1);
            this.tblLayPnlAddShareInput.Controls.Add(this.cboBoxCultureInfo, 1, 10);
            this.tblLayPnlAddShareInput.Controls.Add(this.datePickerDate, 1, 1);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblCultureInfo, 0, 10);
            this.tblLayPnlAddShareInput.Controls.Add(this.datePickerTime, 2, 1);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblName, 0, 2);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblReductionUnit, 3, 7);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblSharePriceUnit, 3, 4);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxReduction, 1, 7);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxSharePrice, 1, 4);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblReduction, 0, 7);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblSharePrice, 0, 4);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblCostsUnit, 3, 6);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxName, 1, 2);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxCosts, 1, 6);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblVolume, 0, 3);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblCosts, 0, 6);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxVolume, 1, 3);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblVolumeUnit, 3, 3);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblMarketValue, 0, 5);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxMarketValue, 1, 5);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblMarketValueUnit, 3, 5);
            this.tblLayPnlAddShareInput.Controls.Add(this.lblDocument, 0, 13);
            this.tblLayPnlAddShareInput.Controls.Add(this.txtBoxDocument, 1, 13);
            this.tblLayPnlAddShareInput.Controls.Add(this.btnDocumentBrowse, 3, 13);
            this.tblLayPnlAddShareInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlAddShareInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblLayPnlAddShareInput.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlAddShareInput.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlAddShareInput.Name = "tblLayPnlAddShareInput";
            this.tblLayPnlAddShareInput.RowCount = 14;
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlAddShareInput.Size = new System.Drawing.Size(819, 336);
            this.tblLayPnlAddShareInput.TabIndex = 43;
            // 
            // cbxShareType
            // 
            this.tblLayPnlAddShareInput.SetColumnSpan(this.cbxShareType, 2);
            this.cbxShareType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxShareType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxShareType.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxShareType.FormattingEnabled = true;
            this.cbxShareType.Location = new System.Drawing.Point(211, 289);
            this.cbxShareType.Margin = new System.Windows.Forms.Padding(1);
            this.cbxShareType.Name = "cbxShareType";
            this.cbxShareType.Size = new System.Drawing.Size(530, 22);
            this.cbxShareType.TabIndex = 44;
            this.cbxShareType.SelectedIndexChanged += new System.EventHandler(this.CbxShareType_SelectedIndexChanged);
            // 
            // lblShareType
            // 
            this.lblShareType.BackColor = System.Drawing.Color.LightGray;
            this.lblShareType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShareType.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareType.Location = new System.Drawing.Point(1, 289);
            this.lblShareType.Margin = new System.Windows.Forms.Padding(1);
            this.lblShareType.Name = "lblShareType";
            this.lblShareType.Size = new System.Drawing.Size(208, 22);
            this.lblShareType.TabIndex = 43;
            this.lblShareType.Text = "_lblShareType";
            this.lblShareType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxFinalValue
            // 
            this.txtBoxFinalValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxFinalValue, 2);
            this.txtBoxFinalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxFinalValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxFinalValue.Location = new System.Drawing.Point(211, 193);
            this.txtBoxFinalValue.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxFinalValue.Name = "txtBoxFinalValue";
            this.txtBoxFinalValue.ReadOnly = true;
            this.txtBoxFinalValue.Size = new System.Drawing.Size(530, 22);
            this.txtBoxFinalValue.TabIndex = 41;
            // 
            // lblFinalValueUnit
            // 
            this.lblFinalValueUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinalValueUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalValueUnit.Location = new System.Drawing.Point(743, 193);
            this.lblFinalValueUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblFinalValueUnit.Name = "lblFinalValueUnit";
            this.lblFinalValueUnit.Size = new System.Drawing.Size(75, 22);
            this.lblFinalValueUnit.TabIndex = 42;
            this.lblFinalValueUnit.Text = "_lblFinalValueUnit";
            this.lblFinalValueUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxDividendPayoutInterval
            // 
            this.tblLayPnlAddShareInput.SetColumnSpan(this.cbxDividendPayoutInterval, 2);
            this.cbxDividendPayoutInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxDividendPayoutInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDividendPayoutInterval.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxDividendPayoutInterval.FormattingEnabled = true;
            this.cbxDividendPayoutInterval.Location = new System.Drawing.Point(211, 265);
            this.cbxDividendPayoutInterval.Margin = new System.Windows.Forms.Padding(1);
            this.cbxDividendPayoutInterval.Name = "cbxDividendPayoutInterval";
            this.cbxDividendPayoutInterval.Size = new System.Drawing.Size(530, 22);
            this.cbxDividendPayoutInterval.TabIndex = 10;
            this.cbxDividendPayoutInterval.SelectedIndexChanged += new System.EventHandler(this.CbxDividendPayoutInterval_SelectedIndexChanged);
            // 
            // lblFinalValue
            // 
            this.lblFinalValue.BackColor = System.Drawing.Color.LightGray;
            this.lblFinalValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFinalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinalValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalValue.Location = new System.Drawing.Point(1, 193);
            this.lblFinalValue.Margin = new System.Windows.Forms.Padding(1);
            this.lblFinalValue.Name = "lblFinalValue";
            this.lblFinalValue.Size = new System.Drawing.Size(208, 22);
            this.lblFinalValue.TabIndex = 41;
            this.lblFinalValue.Text = "_lblFinalValue";
            this.lblFinalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDividendPayoutInterval
            // 
            this.lblDividendPayoutInterval.BackColor = System.Drawing.Color.LightGray;
            this.lblDividendPayoutInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDividendPayoutInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDividendPayoutInterval.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDividendPayoutInterval.Location = new System.Drawing.Point(1, 265);
            this.lblDividendPayoutInterval.Margin = new System.Windows.Forms.Padding(1);
            this.lblDividendPayoutInterval.Name = "lblDividendPayoutInterval";
            this.lblDividendPayoutInterval.Size = new System.Drawing.Size(208, 22);
            this.lblDividendPayoutInterval.TabIndex = 29;
            this.lblDividendPayoutInterval.Text = "_lblDividendPayoutInterval";
            this.lblDividendPayoutInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReductionUnit
            // 
            this.lblReductionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReductionUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReductionUnit.Location = new System.Drawing.Point(743, 169);
            this.lblReductionUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblReductionUnit.Name = "lblReductionUnit";
            this.lblReductionUnit.Size = new System.Drawing.Size(75, 22);
            this.lblReductionUnit.TabIndex = 36;
            this.lblReductionUnit.Text = "_lblReductionUnit";
            this.lblReductionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSharePriceUnit
            // 
            this.lblSharePriceUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSharePriceUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharePriceUnit.Location = new System.Drawing.Point(743, 97);
            this.lblSharePriceUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblSharePriceUnit.Name = "lblSharePriceUnit";
            this.lblSharePriceUnit.Size = new System.Drawing.Size(75, 22);
            this.lblSharePriceUnit.TabIndex = 39;
            this.lblSharePriceUnit.Text = "_lblMarketValueUnit";
            this.lblSharePriceUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxReduction
            // 
            this.txtBoxReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxReduction, 2);
            this.txtBoxReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxReduction.Location = new System.Drawing.Point(211, 169);
            this.txtBoxReduction.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxReduction.Name = "txtBoxReduction";
            this.txtBoxReduction.Size = new System.Drawing.Size(530, 22);
            this.txtBoxReduction.TabIndex = 7;
            this.txtBoxReduction.TextChanged += new System.EventHandler(this.OnTxtBoxReduction_TextChanged);
            this.txtBoxReduction.Leave += new System.EventHandler(this.OnTxtBoxReduction_Leave);
            // 
            // txtBoxSharePrice
            // 
            this.txtBoxSharePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxSharePrice, 2);
            this.txtBoxSharePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxSharePrice.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSharePrice.Location = new System.Drawing.Point(211, 97);
            this.txtBoxSharePrice.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxSharePrice.Name = "txtBoxSharePrice";
            this.txtBoxSharePrice.Size = new System.Drawing.Size(530, 22);
            this.txtBoxSharePrice.TabIndex = 5;
            this.txtBoxSharePrice.TextChanged += new System.EventHandler(this.OnTxtBoxSharePrice_TextChanged);
            this.txtBoxSharePrice.Leave += new System.EventHandler(this.OnTxtBoxSharePrice_Leave);
            // 
            // lblReduction
            // 
            this.lblReduction.BackColor = System.Drawing.Color.LightGray;
            this.lblReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReduction.Location = new System.Drawing.Point(1, 169);
            this.lblReduction.Margin = new System.Windows.Forms.Padding(1);
            this.lblReduction.Name = "lblReduction";
            this.lblReduction.Size = new System.Drawing.Size(208, 22);
            this.lblReduction.TabIndex = 34;
            this.lblReduction.Text = "_lblReduction";
            this.lblReduction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSharePrice
            // 
            this.lblSharePrice.BackColor = System.Drawing.Color.LightGray;
            this.lblSharePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSharePrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSharePrice.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharePrice.Location = new System.Drawing.Point(1, 97);
            this.lblSharePrice.Margin = new System.Windows.Forms.Padding(1);
            this.lblSharePrice.Name = "lblSharePrice";
            this.lblSharePrice.Size = new System.Drawing.Size(208, 22);
            this.lblSharePrice.TabIndex = 38;
            this.lblSharePrice.Text = "_lblSharePrice";
            this.lblSharePrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCostsUnit
            // 
            this.lblCostsUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCostsUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostsUnit.Location = new System.Drawing.Point(743, 145);
            this.lblCostsUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblCostsUnit.Name = "lblCostsUnit";
            this.lblCostsUnit.Size = new System.Drawing.Size(75, 22);
            this.lblCostsUnit.TabIndex = 32;
            this.lblCostsUnit.Text = "_lblCostsUnit";
            this.lblCostsUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxCosts
            // 
            this.txtBoxCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlAddShareInput.SetColumnSpan(this.txtBoxCosts, 2);
            this.txtBoxCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxCosts.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxCosts.Location = new System.Drawing.Point(211, 145);
            this.txtBoxCosts.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxCosts.Name = "txtBoxCosts";
            this.txtBoxCosts.Size = new System.Drawing.Size(530, 22);
            this.txtBoxCosts.TabIndex = 6;
            this.txtBoxCosts.TextChanged += new System.EventHandler(this.OnTxtBoxCosts_TextChanged);
            this.txtBoxCosts.Leave += new System.EventHandler(this.OnTxtBoxCosts_Leave);
            // 
            // lblCosts
            // 
            this.lblCosts.BackColor = System.Drawing.Color.LightGray;
            this.lblCosts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCosts.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCosts.Location = new System.Drawing.Point(1, 145);
            this.lblCosts.Margin = new System.Windows.Forms.Padding(1);
            this.lblCosts.Name = "lblCosts";
            this.lblCosts.Size = new System.Drawing.Size(208, 22);
            this.lblCosts.TabIndex = 25;
            this.lblCosts.Text = "_lblCosts";
            this.lblCosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewShareAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 424);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxGeneral);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 462);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 462);
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
            this.tblLayPnlAddShareButtons.ResumeLayout(false);
            this.tblLayPnlAddShareInput.ResumeLayout(false);
            this.tblLayPnlAddShareInput.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TableLayoutPanel tblLayPnlAddShareInput;
        private System.Windows.Forms.TableLayoutPanel tblLayPnlAddShareButtons;
        private System.Windows.Forms.ComboBox cbxShareType;
        private System.Windows.Forms.Label lblShareType;
    }
}