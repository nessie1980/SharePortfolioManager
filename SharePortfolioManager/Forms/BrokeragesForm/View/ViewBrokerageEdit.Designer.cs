using System.ComponentModel;
using System.Windows.Forms;

namespace SharePortfolioManager.BrokeragesForm.View
{
    partial class ViewBrokerageEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.grpBoxOverview = new System.Windows.Forms.GroupBox();
            this.tblLayPnlOverviewTabControl = new System.Windows.Forms.TableLayoutPanel();
            this.tabCtrlBrokerage = new System.Windows.Forms.TabControl();
            this.grpBoxAdd = new System.Windows.Forms.GroupBox();
            this.tblLayPnlBrokerageButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tblLayPnlBrokerageInput = new System.Windows.Forms.TableLayoutPanel();
            this.txtBoxReduction = new System.Windows.Forms.TextBox();
            this.lblAddReductionUnit = new System.Windows.Forms.Label();
            this.lblReduction = new System.Windows.Forms.Label();
            this.lblAddTraderPlaceFeeUnit = new System.Windows.Forms.Label();
            this.txtBoxTraderPlaceFee = new System.Windows.Forms.TextBox();
            this.lblTraderPlaceFee = new System.Windows.Forms.Label();
            this.lblAddProvisionUnit = new System.Windows.Forms.Label();
            this.txtBoxProvision = new System.Windows.Forms.TextBox();
            this.lblProvision = new System.Windows.Forms.Label();
            this.lblAddBrokerFeeUnit = new System.Windows.Forms.Label();
            this.txtBoxBrokerFee = new System.Windows.Forms.TextBox();
            this.lblBrokerFee = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.datePickerDate = new System.Windows.Forms.DateTimePicker();
            this.datePickerTime = new System.Windows.Forms.DateTimePicker();
            this.chkBoxBuyPart = new System.Windows.Forms.CheckBox();
            this.chkBoxSalePart = new System.Windows.Forms.CheckBox();
            this.lblDocument = new System.Windows.Forms.Label();
            this.txtBoxDocument = new System.Windows.Forms.TextBox();
            this.btnDocumentBrowse = new System.Windows.Forms.Button();
            this.lblAddBrokerageUnit = new System.Windows.Forms.Label();
            this.txtBoxBrokerage = new System.Windows.Forms.TextBox();
            this.lblBrokerage = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessageBrokerageEdit = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxOverview.SuspendLayout();
            this.tblLayPnlOverviewTabControl.SuspendLayout();
            this.grpBoxAdd.SuspendLayout();
            this.tblLayPnlBrokerageButtons.SuspendLayout();
            this.tblLayPnlBrokerageInput.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxOverview
            // 
            this.grpBoxOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxOverview.Controls.Add(this.tblLayPnlOverviewTabControl);
            this.grpBoxOverview.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxOverview.Location = new System.Drawing.Point(5, 282);
            this.grpBoxOverview.Name = "grpBoxOverview";
            this.grpBoxOverview.Size = new System.Drawing.Size(825, 150);
            this.grpBoxOverview.TabIndex = 5;
            this.grpBoxOverview.TabStop = false;
            this.grpBoxOverview.Text = "_brokerage";
            this.grpBoxOverview.MouseLeave += new System.EventHandler(this.OnGrpBoxOverview_MouseLeave);
            // 
            // tblLayPnlOverviewTabControl
            // 
            this.tblLayPnlOverviewTabControl.ColumnCount = 1;
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOverviewTabControl.Controls.Add(this.tabCtrlBrokerage, 0, 0);
            this.tblLayPnlOverviewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOverviewTabControl.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlOverviewTabControl.Name = "tblLayPnlOverviewTabControl";
            this.tblLayPnlOverviewTabControl.RowCount = 1;
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOverviewTabControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tblLayPnlOverviewTabControl.Size = new System.Drawing.Size(819, 129);
            this.tblLayPnlOverviewTabControl.TabIndex = 0;
            // 
            // tabCtrlBrokerage
            // 
            this.tabCtrlBrokerage.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrlBrokerage.Location = new System.Drawing.Point(3, 3);
            this.tabCtrlBrokerage.Name = "tabCtrlBrokerage";
            this.tabCtrlBrokerage.SelectedIndex = 0;
            this.tabCtrlBrokerage.Size = new System.Drawing.Size(813, 123);
            this.tabCtrlBrokerage.TabIndex = 0;
            this.tabCtrlBrokerage.SelectedIndexChanged += new System.EventHandler(this.OnTabCtrlBrokerage_SelectedIndexChanged);
            this.tabCtrlBrokerage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTabCtrlBrokerage_KeyDown);
            this.tabCtrlBrokerage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTabCtrlBrokerage_KeyPress);
            this.tabCtrlBrokerage.MouseLeave += new System.EventHandler(this.OnTabCtrlBrokerage_MouseLeave);
            // 
            // grpBoxAdd
            // 
            this.grpBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBrokerageButtons);
            this.grpBoxAdd.Controls.Add(this.tblLayPnlBrokerageInput);
            this.grpBoxAdd.Controls.Add(this.statusStrip1);
            this.grpBoxAdd.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxAdd.Location = new System.Drawing.Point(5, 5);
            this.grpBoxAdd.Name = "grpBoxAdd";
            this.grpBoxAdd.Size = new System.Drawing.Size(825, 271);
            this.grpBoxAdd.TabIndex = 4;
            this.grpBoxAdd.TabStop = false;
            this.grpBoxAdd.Text = "_grpBoxAddBrokerage";
            // 
            // tblLayPnlBrokerageButtons
            // 
            this.tblLayPnlBrokerageButtons.ColumnCount = 5;
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnAddSave, 1, 0);
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnDelete, 2, 0);
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnCancel, 4, 0);
            this.tblLayPnlBrokerageButtons.Controls.Add(this.btnReset, 3, 0);
            this.tblLayPnlBrokerageButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlBrokerageButtons.Location = new System.Drawing.Point(3, 210);
            this.tblLayPnlBrokerageButtons.Name = "tblLayPnlBrokerageButtons";
            this.tblLayPnlBrokerageButtons.RowCount = 1;
            this.tblLayPnlBrokerageButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlBrokerageButtons.Size = new System.Drawing.Size(819, 33);
            this.tblLayPnlBrokerageButtons.TabIndex = 13;
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
            this.btnAddSave.TabIndex = 12;
            this.btnAddSave.Text = "_Add/Save";
            this.btnAddSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.OnBtnAddSave_Click);
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
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "_Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
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
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "_Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
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
            this.btnReset.TabIndex = 14;
            this.btnReset.Text = "_Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.OnBtnReset_Click);
            // 
            // tblLayPnlBrokerageInput
            // 
            this.tblLayPnlBrokerageInput.ColumnCount = 4;
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayPnlBrokerageInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxReduction, 1, 5);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblAddReductionUnit, 3, 5);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblReduction, 0, 5);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblAddTraderPlaceFeeUnit, 3, 4);
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxTraderPlaceFee, 1, 4);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblTraderPlaceFee, 0, 4);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblAddProvisionUnit, 3, 2);
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxProvision, 1, 2);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblProvision, 0, 2);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblAddBrokerFeeUnit, 3, 3);
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxBrokerFee, 1, 3);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblBrokerFee, 0, 3);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblDate, 0, 0);
            this.tblLayPnlBrokerageInput.Controls.Add(this.datePickerDate, 1, 0);
            this.tblLayPnlBrokerageInput.Controls.Add(this.datePickerTime, 2, 0);
            this.tblLayPnlBrokerageInput.Controls.Add(this.chkBoxBuyPart, 1, 1);
            this.tblLayPnlBrokerageInput.Controls.Add(this.chkBoxSalePart, 2, 1);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblDocument, 0, 7);
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxDocument, 1, 7);
            this.tblLayPnlBrokerageInput.Controls.Add(this.btnDocumentBrowse, 3, 7);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblAddBrokerageUnit, 3, 6);
            this.tblLayPnlBrokerageInput.Controls.Add(this.txtBoxBrokerage, 1, 6);
            this.tblLayPnlBrokerageInput.Controls.Add(this.lblBrokerage, 0, 6);
            this.tblLayPnlBrokerageInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblLayPnlBrokerageInput.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlBrokerageInput.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlBrokerageInput.Name = "tblLayPnlBrokerageInput";
            this.tblLayPnlBrokerageInput.RowCount = 8;
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblLayPnlBrokerageInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlBrokerageInput.Size = new System.Drawing.Size(819, 192);
            this.tblLayPnlBrokerageInput.TabIndex = 12;
            // 
            // txtBoxReduction
            // 
            this.txtBoxReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxReduction, 2);
            this.txtBoxReduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxReduction.Location = new System.Drawing.Point(251, 121);
            this.txtBoxReduction.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxReduction.Name = "txtBoxReduction";
            this.txtBoxReduction.Size = new System.Drawing.Size(490, 22);
            this.txtBoxReduction.TabIndex = 8;
            this.txtBoxReduction.TextChanged += new System.EventHandler(this.OnTxtBoxReduction_TextChanged);
            this.txtBoxReduction.Enter += new System.EventHandler(this.OnTxtBoxReduction_Enter);
            this.txtBoxReduction.Leave += new System.EventHandler(this.OnTxtBoxReduction_Leave);
            // 
            // lblAddReductionUnit
            // 
            this.lblAddReductionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddReductionUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddReductionUnit.Location = new System.Drawing.Point(743, 121);
            this.lblAddReductionUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddReductionUnit.Name = "lblAddReductionUnit";
            this.lblAddReductionUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddReductionUnit.TabIndex = 32;
            this.lblAddReductionUnit.Text = "_lblReductionUnit";
            this.lblAddReductionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReduction
            // 
            this.lblReduction.BackColor = System.Drawing.Color.LightGray;
            this.lblReduction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReduction.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReduction.Location = new System.Drawing.Point(1, 121);
            this.lblReduction.Margin = new System.Windows.Forms.Padding(1);
            this.lblReduction.Name = "lblReduction";
            this.lblReduction.Size = new System.Drawing.Size(248, 22);
            this.lblReduction.TabIndex = 31;
            this.lblReduction.Text = "_addReductionValue";
            this.lblReduction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddTraderPlaceFeeUnit
            // 
            this.lblAddTraderPlaceFeeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddTraderPlaceFeeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddTraderPlaceFeeUnit.Location = new System.Drawing.Point(743, 97);
            this.lblAddTraderPlaceFeeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddTraderPlaceFeeUnit.Name = "lblAddTraderPlaceFeeUnit";
            this.lblAddTraderPlaceFeeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddTraderPlaceFeeUnit.TabIndex = 30;
            this.lblAddTraderPlaceFeeUnit.Text = "_lblTraderPlaceFeeUnit";
            this.lblAddTraderPlaceFeeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxTraderPlaceFee
            // 
            this.txtBoxTraderPlaceFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxTraderPlaceFee, 2);
            this.txtBoxTraderPlaceFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxTraderPlaceFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxTraderPlaceFee.Location = new System.Drawing.Point(251, 97);
            this.txtBoxTraderPlaceFee.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxTraderPlaceFee.Name = "txtBoxTraderPlaceFee";
            this.txtBoxTraderPlaceFee.Size = new System.Drawing.Size(490, 22);
            this.txtBoxTraderPlaceFee.TabIndex = 7;
            this.txtBoxTraderPlaceFee.TextChanged += new System.EventHandler(this.OnTxtBoxTraderPlaceFee_TextChanged);
            this.txtBoxTraderPlaceFee.Enter += new System.EventHandler(this.OnTxtBoxTraderPlaceFee_Enter);
            this.txtBoxTraderPlaceFee.Leave += new System.EventHandler(this.OnTxtBoxTraderPlaceFee_Leave);
            // 
            // lblTraderPlaceFee
            // 
            this.lblTraderPlaceFee.BackColor = System.Drawing.Color.LightGray;
            this.lblTraderPlaceFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTraderPlaceFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTraderPlaceFee.Location = new System.Drawing.Point(1, 97);
            this.lblTraderPlaceFee.Margin = new System.Windows.Forms.Padding(1);
            this.lblTraderPlaceFee.Name = "lblTraderPlaceFee";
            this.lblTraderPlaceFee.Size = new System.Drawing.Size(248, 22);
            this.lblTraderPlaceFee.TabIndex = 28;
            this.lblTraderPlaceFee.Text = "_addTraderPlaceFeeValue";
            this.lblTraderPlaceFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddProvisionUnit
            // 
            this.lblAddProvisionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddProvisionUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddProvisionUnit.Location = new System.Drawing.Point(743, 49);
            this.lblAddProvisionUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddProvisionUnit.Name = "lblAddProvisionUnit";
            this.lblAddProvisionUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddProvisionUnit.TabIndex = 27;
            this.lblAddProvisionUnit.Text = "_lblProvisionUnit";
            this.lblAddProvisionUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxProvision
            // 
            this.txtBoxProvision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxProvision, 2);
            this.txtBoxProvision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxProvision.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxProvision.Location = new System.Drawing.Point(251, 49);
            this.txtBoxProvision.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxProvision.Name = "txtBoxProvision";
            this.txtBoxProvision.Size = new System.Drawing.Size(490, 22);
            this.txtBoxProvision.TabIndex = 5;
            this.txtBoxProvision.TextChanged += new System.EventHandler(this.OnTxtBoxProvision_TextChanged);
            this.txtBoxProvision.Enter += new System.EventHandler(this.OnTxtBoxProvision_Enter);
            this.txtBoxProvision.Leave += new System.EventHandler(this.OnTxtBoxProvision_Leave);
            // 
            // lblProvision
            // 
            this.lblProvision.BackColor = System.Drawing.Color.LightGray;
            this.lblProvision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProvision.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProvision.Location = new System.Drawing.Point(1, 49);
            this.lblProvision.Margin = new System.Windows.Forms.Padding(1);
            this.lblProvision.Name = "lblProvision";
            this.lblProvision.Size = new System.Drawing.Size(248, 22);
            this.lblProvision.TabIndex = 25;
            this.lblProvision.Text = "_addProvisionValue";
            this.lblProvision.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddBrokerFeeUnit
            // 
            this.lblAddBrokerFeeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddBrokerFeeUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddBrokerFeeUnit.Location = new System.Drawing.Point(743, 73);
            this.lblAddBrokerFeeUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddBrokerFeeUnit.Name = "lblAddBrokerFeeUnit";
            this.lblAddBrokerFeeUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddBrokerFeeUnit.TabIndex = 24;
            this.lblAddBrokerFeeUnit.Text = "_lblBrokerFeeUnit";
            this.lblAddBrokerFeeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxBrokerFee
            // 
            this.txtBoxBrokerFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxBrokerFee, 2);
            this.txtBoxBrokerFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBrokerFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBrokerFee.Location = new System.Drawing.Point(251, 73);
            this.txtBoxBrokerFee.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxBrokerFee.Name = "txtBoxBrokerFee";
            this.txtBoxBrokerFee.Size = new System.Drawing.Size(490, 22);
            this.txtBoxBrokerFee.TabIndex = 6;
            this.txtBoxBrokerFee.TextChanged += new System.EventHandler(this.OnTxtBoxBrokerFee_TextChanged);
            this.txtBoxBrokerFee.Enter += new System.EventHandler(this.OnTxtBoxBrokerFee_Enter);
            this.txtBoxBrokerFee.Leave += new System.EventHandler(this.OnTxtBoxBrokerFee_Leave);
            // 
            // lblBrokerFee
            // 
            this.lblBrokerFee.BackColor = System.Drawing.Color.LightGray;
            this.lblBrokerFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBrokerFee.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerFee.Location = new System.Drawing.Point(1, 73);
            this.lblBrokerFee.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerFee.Name = "lblBrokerFee";
            this.lblBrokerFee.Size = new System.Drawing.Size(248, 22);
            this.lblBrokerFee.TabIndex = 22;
            this.lblBrokerFee.Text = "_addBrokerFeeValue";
            this.lblBrokerFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "_addDate";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePickerDate
            // 
            this.datePickerDate.CustomFormat = "";
            this.datePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datePickerDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerDate.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerDate.Location = new System.Drawing.Point(251, 1);
            this.datePickerDate.Margin = new System.Windows.Forms.Padding(1);
            this.datePickerDate.Name = "datePickerDate";
            this.datePickerDate.Size = new System.Drawing.Size(244, 22);
            this.datePickerDate.TabIndex = 1;
            this.datePickerDate.ValueChanged += new System.EventHandler(this.OnDatePickerDate_ValueChanged);
            this.datePickerDate.Enter += new System.EventHandler(this.OnDatePickerDate_Enter);
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
            this.datePickerTime.Size = new System.Drawing.Size(244, 22);
            this.datePickerTime.TabIndex = 2;
            this.datePickerTime.ValueChanged += new System.EventHandler(this.OnDatePickerTime_ValueChanged);
            this.datePickerTime.Enter += new System.EventHandler(this.OnDatePickerTime_Enter);
            this.datePickerTime.Leave += new System.EventHandler(this.OnDatePickerTime_Leave);
            // 
            // chkBoxBuyPart
            // 
            this.chkBoxBuyPart.AutoSize = true;
            this.chkBoxBuyPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkBoxBuyPart.Enabled = false;
            this.chkBoxBuyPart.Location = new System.Drawing.Point(253, 27);
            this.chkBoxBuyPart.Name = "chkBoxBuyPart";
            this.chkBoxBuyPart.Size = new System.Drawing.Size(240, 18);
            this.chkBoxBuyPart.TabIndex = 3;
            this.chkBoxBuyPart.TabStop = false;
            this.chkBoxBuyPart.Text = "_BuyPart";
            this.chkBoxBuyPart.UseVisualStyleBackColor = true;
            // 
            // chkBoxSalePart
            // 
            this.chkBoxSalePart.AutoSize = true;
            this.chkBoxSalePart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkBoxSalePart.Enabled = false;
            this.chkBoxSalePart.Location = new System.Drawing.Point(499, 27);
            this.chkBoxSalePart.Name = "chkBoxSalePart";
            this.chkBoxSalePart.Size = new System.Drawing.Size(240, 18);
            this.chkBoxSalePart.TabIndex = 4;
            this.chkBoxSalePart.TabStop = false;
            this.chkBoxSalePart.Text = "_SalePart";
            this.chkBoxSalePart.UseVisualStyleBackColor = true;
            // 
            // lblDocument
            // 
            this.lblDocument.BackColor = System.Drawing.Color.LightGray;
            this.lblDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocument.Location = new System.Drawing.Point(1, 169);
            this.lblDocument.Margin = new System.Windows.Forms.Padding(1);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(248, 22);
            this.lblDocument.TabIndex = 19;
            this.lblDocument.Text = "_addBrokerageDoc";
            this.lblDocument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBoxDocument
            // 
            this.txtBoxDocument.AllowDrop = true;
            this.txtBoxDocument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxDocument, 2);
            this.txtBoxDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxDocument.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDocument.Location = new System.Drawing.Point(251, 169);
            this.txtBoxDocument.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxDocument.Name = "txtBoxDocument";
            this.txtBoxDocument.Size = new System.Drawing.Size(490, 22);
            this.txtBoxDocument.TabIndex = 10;
            this.txtBoxDocument.TextChanged += new System.EventHandler(this.OnTxtBoxDocument_TextChanged);
            this.txtBoxDocument.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragDrop);
            this.txtBoxDocument.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTxtBoxDocument_DragEnter);
            this.txtBoxDocument.Enter += new System.EventHandler(this.OnTxtBoxDocument_Enter);
            this.txtBoxDocument.Leave += new System.EventHandler(this.OnTxtBoxDocument_Leave);
            // 
            // btnDocumentBrowse
            // 
            this.btnDocumentBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDocumentBrowse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDocumentBrowse.Image = global::SharePortfolioManager.Properties.Resources.menu_folder_open_16;
            this.btnDocumentBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.Location = new System.Drawing.Point(743, 169);
            this.btnDocumentBrowse.Margin = new System.Windows.Forms.Padding(1);
            this.btnDocumentBrowse.Name = "btnDocumentBrowse";
            this.btnDocumentBrowse.Size = new System.Drawing.Size(75, 22);
            this.btnDocumentBrowse.TabIndex = 11;
            this.btnDocumentBrowse.Text = "...";
            this.btnDocumentBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocumentBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDocumentBrowse.UseVisualStyleBackColor = true;
            this.btnDocumentBrowse.Click += new System.EventHandler(this.OnBtnBrokerageDocumentBrowse_Click);
            // 
            // lblAddBrokerageUnit
            // 
            this.lblAddBrokerageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAddBrokerageUnit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddBrokerageUnit.Location = new System.Drawing.Point(743, 145);
            this.lblAddBrokerageUnit.Margin = new System.Windows.Forms.Padding(1);
            this.lblAddBrokerageUnit.Name = "lblAddBrokerageUnit";
            this.lblAddBrokerageUnit.Size = new System.Drawing.Size(75, 22);
            this.lblAddBrokerageUnit.TabIndex = 12;
            this.lblAddBrokerageUnit.Text = "_lblBrokerageUnit";
            this.lblAddBrokerageUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBoxBrokerage
            // 
            this.txtBoxBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tblLayPnlBrokerageInput.SetColumnSpan(this.txtBoxBrokerage, 2);
            this.txtBoxBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxBrokerage.Enabled = false;
            this.txtBoxBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxBrokerage.Location = new System.Drawing.Point(251, 145);
            this.txtBoxBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.txtBoxBrokerage.Name = "txtBoxBrokerage";
            this.txtBoxBrokerage.ReadOnly = true;
            this.txtBoxBrokerage.Size = new System.Drawing.Size(490, 22);
            this.txtBoxBrokerage.TabIndex = 9;
            this.txtBoxBrokerage.TabStop = false;
            this.txtBoxBrokerage.Text = "-";
            // 
            // lblBrokerage
            // 
            this.lblBrokerage.BackColor = System.Drawing.Color.LightGray;
            this.lblBrokerage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBrokerage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrokerage.Location = new System.Drawing.Point(1, 145);
            this.lblBrokerage.Margin = new System.Windows.Forms.Padding(1);
            this.lblBrokerage.Name = "lblBrokerage";
            this.lblBrokerage.Size = new System.Drawing.Size(248, 22);
            this.lblBrokerage.TabIndex = 3;
            this.lblBrokerage.Text = "_addBrokerageValue";
            this.lblBrokerage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessageBrokerageEdit});
            this.statusStrip1.Location = new System.Drawing.Point(3, 246);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessageBrokerageEdit
            // 
            this.toolStripStatusLabelMessageBrokerageEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelMessageBrokerageEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelMessageBrokerageEdit.Name = "toolStripStatusLabelMessageBrokerageEdit";
            this.toolStripStatusLabelMessageBrokerageEdit.Size = new System.Drawing.Size(0, 17);
            // 
            // ViewBrokerageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(834, 434);
            this.ControlBox = false;
            this.Controls.Add(this.grpBoxOverview);
            this.Controls.Add(this.grpBoxAdd);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 550);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 450);
            this.Name = "ViewBrokerageEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_shareBrokerageEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShareBrokerageEdit_FormClosing);
            this.Load += new System.EventHandler(this.ShareBrokerageEdit_Load);
            this.grpBoxOverview.ResumeLayout(false);
            this.tblLayPnlOverviewTabControl.ResumeLayout(false);
            this.grpBoxAdd.ResumeLayout(false);
            this.grpBoxAdd.PerformLayout();
            this.tblLayPnlBrokerageButtons.ResumeLayout(false);
            this.tblLayPnlBrokerageInput.ResumeLayout(false);
            this.tblLayPnlBrokerageInput.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox grpBoxOverview;
        private GroupBox grpBoxAdd;
        private Button btnCancel;
        private Button btnReset;
        private Label lblAddBrokerageUnit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelMessageBrokerageEdit;
        private DateTimePicker datePickerDate;
        private TextBox txtBoxBrokerage;
        private Label lblBrokerage;
        private Label lblDate;
        private Button btnAddSave;
        private TabControl tabCtrlBrokerage;
        private Button btnDelete;
        private DateTimePicker datePickerTime;
        private TextBox txtBoxDocument;
        private Label lblDocument;
        private Button btnDocumentBrowse;
        private TableLayoutPanel tblLayPnlBrokerageInput;
        private TableLayoutPanel tblLayPnlBrokerageButtons;
        private TableLayoutPanel tblLayPnlOverviewTabControl;
        private CheckBox chkBoxBuyPart;
        private CheckBox chkBoxSalePart;
        private Label lblAddTraderPlaceFeeUnit;
        private TextBox txtBoxTraderPlaceFee;
        private Label lblTraderPlaceFee;
        private Label lblAddProvisionUnit;
        private TextBox txtBoxProvision;
        private Label lblProvision;
        private Label lblAddBrokerFeeUnit;
        private TextBox txtBoxBrokerFee;
        private Label lblBrokerFee;
        private Label lblAddReductionUnit;
        private Label lblReduction;
        private TextBox txtBoxReduction;
    }
}