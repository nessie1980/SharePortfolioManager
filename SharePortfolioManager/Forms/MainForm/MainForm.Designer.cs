using System.ComponentModel;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    partial class FrmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpBoxSharePortfolio = new System.Windows.Forms.GroupBox();
            this.tabCtrlShareOverviews = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvPortfolioFinalValue = new System.Windows.Forms.DataGridView();
            this.dgvPortfolioFooterFinalValue = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvPortfolioMarketValue = new System.Windows.Forms.DataGridView();
            this.dgvPortfolioFooterMarketValue = new System.Windows.Forms.DataGridView();
            this.btnClearLogger = new System.Windows.Forms.Button();
            this.btnRefreshAll = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grpBoxShareDetails = new System.Windows.Forms.GroupBox();
            this.tabCtrlDetails = new System.Windows.Forms.TabControl();
            this.tabPgDetailsFinalValue = new System.Windows.Forms.TabPage();
            this.lblDetailsFinalValueDateValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueDate = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueCostsValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueDividendValue = new System.Windows.Forms.Label();
            this.lblDetailsFinaValueCurPrice = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueCost = new System.Windows.Forms.Label();
            this.lblDetailsPurchase = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueDividend = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueVolume = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueDiffSumPrevValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueTotalSum = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueDiffSumPrev = new System.Windows.Forms.Label();
            this.lblDetailsFinalValuePerformance = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueDiffPerformancePrevValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueTotalPerformanceValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueDiffPerformancePrev = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueVolumeValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueCurPriceValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValuePurchaseValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueTotalSumValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValueTotalProfitValue = new System.Windows.Forms.Label();
            this.lblDetailsFinalValuePrevPrice = new System.Windows.Forms.Label();
            this.lblDetailsFinalVinalTotalProfit = new System.Windows.Forms.Label();
            this.lblDetailsFinalValuePrevPriceValue = new System.Windows.Forms.Label();
            this.tabPgDetailsMarketValue = new System.Windows.Forms.TabPage();
            this.lblDetailsMarketValueDateValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueDate = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueCostValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueDividendValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueCurPrice = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueCost = new System.Windows.Forms.Label();
            this.lblDetailsMarketValuePurchase = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueDividend = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueVolume = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueDiffSumPrevValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueTotalSum = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueDiffSumPrev = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueTotalPerformance = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueDiffPerformancePrevValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueTotalPerformanceValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueDiffPerformancePrev = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueVolumeValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueCurPriceValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValuePurchaseValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueTotalSumValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueTotalProfitValue = new System.Windows.Forms.Label();
            this.lblDetailsMarketValuePrevPrice = new System.Windows.Forms.Label();
            this.lblDetailsMarketValueTotalProfit = new System.Windows.Forms.Label();
            this.lblDetailsMarketValuePrevPriceValue = new System.Windows.Forms.Label();
            this.tabPgProfitLoss = new System.Windows.Forms.TabPage();
            this.tabCtrlProfitLoss = new System.Windows.Forms.TabControl();
            this.tabPgDividends = new System.Windows.Forms.TabPage();
            this.tabCtrlDividends = new System.Windows.Forms.TabControl();
            this.tabPgCosts = new System.Windows.Forms.TabPage();
            this.tabCtrlCosts = new System.Windows.Forms.TabControl();
            this.grpBoxStatusMessage = new System.Windows.Forms.GroupBox();
            this.rchTxtBoxStateMessage = new System.Windows.Forms.RichTextBox();
            this.lblWebParserState = new System.Windows.Forms.Label();
            this.progressBarWebParser = new System.Windows.Forms.ProgressBar();
            this.grpBoxUpdateState = new System.Windows.Forms.GroupBox();
            this.lblShareNameWebParser = new System.Windows.Forms.Label();
            this.timerStatusMessageClear = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.grpBoxSharePortfolio.SuspendLayout();
            this.tabCtrlShareOverviews.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFinalValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterFinalValue)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioMarketValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterMarketValue)).BeginInit();
            this.grpBoxShareDetails.SuspendLayout();
            this.tabCtrlDetails.SuspendLayout();
            this.tabPgDetailsFinalValue.SuspendLayout();
            this.tabPgDetailsMarketValue.SuspendLayout();
            this.tabPgProfitLoss.SuspendLayout();
            this.tabPgDividends.SuspendLayout();
            this.tabPgCosts.SuspendLayout();
            this.grpBoxStatusMessage.SuspendLayout();
            this.grpBoxUpdateState.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1235, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fileToolStripMenuItem.Text = "&File_";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "New_";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open_";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "SaveAs_";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "&Quit_";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem,
            this.loggerToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.settingsToolStripMenuItem.Text = "&Settings_";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.languageToolStripMenuItem.Text = "_Language";
            // 
            // loggerToolStripMenuItem
            // 
            this.loggerToolStripMenuItem.Name = "loggerToolStripMenuItem";
            this.loggerToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.loggerToolStripMenuItem.Text = "_Logger";
            this.loggerToolStripMenuItem.Click += new System.EventHandler(this.loggerToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.helpToolStripMenuItem.Text = "&Help_";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About_";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // grpBoxSharePortfolio
            // 
            this.grpBoxSharePortfolio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxSharePortfolio.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.grpBoxSharePortfolio.Controls.Add(this.tabCtrlShareOverviews);
            this.grpBoxSharePortfolio.Controls.Add(this.btnClearLogger);
            this.grpBoxSharePortfolio.Controls.Add(this.btnRefreshAll);
            this.grpBoxSharePortfolio.Controls.Add(this.btnDelete);
            this.grpBoxSharePortfolio.Controls.Add(this.btnEdit);
            this.grpBoxSharePortfolio.Controls.Add(this.btnAdd);
            this.grpBoxSharePortfolio.Controls.Add(this.btnRefresh);
            this.grpBoxSharePortfolio.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxSharePortfolio.Location = new System.Drawing.Point(7, 30);
            this.grpBoxSharePortfolio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxSharePortfolio.Name = "grpBoxSharePortfolio";
            this.grpBoxSharePortfolio.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxSharePortfolio.Size = new System.Drawing.Size(1219, 362);
            this.grpBoxSharePortfolio.TabIndex = 3;
            this.grpBoxSharePortfolio.TabStop = false;
            this.grpBoxSharePortfolio.Text = "Share portfolio_";
            // 
            // tabCtrlShareOverviews
            // 
            this.tabCtrlShareOverviews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlShareOverviews.Controls.Add(this.tabPage1);
            this.tabCtrlShareOverviews.Controls.Add(this.tabPage2);
            this.tabCtrlShareOverviews.Location = new System.Drawing.Point(10, 23);
            this.tabCtrlShareOverviews.Margin = new System.Windows.Forms.Padding(0);
            this.tabCtrlShareOverviews.Name = "tabCtrlShareOverviews";
            this.tabCtrlShareOverviews.Padding = new System.Drawing.Point(0, 0);
            this.tabCtrlShareOverviews.SelectedIndex = 0;
            this.tabCtrlShareOverviews.Size = new System.Drawing.Size(1198, 294);
            this.tabCtrlShareOverviews.TabIndex = 12;
            this.tabCtrlShareOverviews.SelectedIndexChanged += new System.EventHandler(this.TabCtrlShareOverviews_SelectedIndexChanged);
            this.tabCtrlShareOverviews.Enter += new System.EventHandler(this.tabCtrlShareOverviews_Enter);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvPortfolioFinalValue);
            this.tabPage1.Controls.Add(this.dgvPortfolioFooterFinalValue);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1190, 266);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "_tabPageCompleteDepotValues";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvPortfolioFinalValue
            // 
            this.dgvPortfolioFinalValue.AllowUserToAddRows = false;
            this.dgvPortfolioFinalValue.AllowUserToDeleteRows = false;
            this.dgvPortfolioFinalValue.AllowUserToResizeColumns = false;
            this.dgvPortfolioFinalValue.AllowUserToResizeRows = false;
            this.dgvPortfolioFinalValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPortfolioFinalValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfolioFinalValue.Location = new System.Drawing.Point(0, 1);
            this.dgvPortfolioFinalValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPortfolioFinalValue.MultiSelect = false;
            this.dgvPortfolioFinalValue.Name = "dgvPortfolioFinalValue";
            this.dgvPortfolioFinalValue.ReadOnly = true;
            this.dgvPortfolioFinalValue.RowHeadersVisible = false;
            this.dgvPortfolioFinalValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioFinalValue.Size = new System.Drawing.Size(1189, 188);
            this.dgvPortfolioFinalValue.TabIndex = 9;
            this.dgvPortfolioFinalValue.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPortfolioFinalValue_CellDoubleClick);
            this.dgvPortfolioFinalValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioFinalValue_CellFormatting);
            this.dgvPortfolioFinalValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioFinalValue_CellPainting);
            this.dgvPortfolioFinalValue.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvPortfolioFinalValue_DataBindingComplete);
            this.dgvPortfolioFinalValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioFinalValue_SelectionChanged);
            this.dgvPortfolioFinalValue.MouseEnter += new System.EventHandler(this.dgvPortfolioFinalValue_MouseEnter);
            this.dgvPortfolioFinalValue.Resize += new System.EventHandler(this.DgvPortfolioFinalValue_Resize);
            // 
            // dgvPortfolioFooterFinalValue
            // 
            this.dgvPortfolioFooterFinalValue.AllowUserToAddRows = false;
            this.dgvPortfolioFooterFinalValue.AllowUserToDeleteRows = false;
            this.dgvPortfolioFooterFinalValue.AllowUserToResizeColumns = false;
            this.dgvPortfolioFooterFinalValue.AllowUserToResizeRows = false;
            this.dgvPortfolioFooterFinalValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPortfolioFooterFinalValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfolioFooterFinalValue.ColumnHeadersVisible = false;
            this.dgvPortfolioFooterFinalValue.Enabled = false;
            this.dgvPortfolioFooterFinalValue.EnableHeadersVisualStyles = false;
            this.dgvPortfolioFooterFinalValue.Location = new System.Drawing.Point(0, 189);
            this.dgvPortfolioFooterFinalValue.MultiSelect = false;
            this.dgvPortfolioFooterFinalValue.Name = "dgvPortfolioFooterFinalValue";
            this.dgvPortfolioFooterFinalValue.ReadOnly = true;
            this.dgvPortfolioFooterFinalValue.RowHeadersVisible = false;
            this.dgvPortfolioFooterFinalValue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPortfolioFooterFinalValue.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPortfolioFooterFinalValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioFooterFinalValue.Size = new System.Drawing.Size(1189, 76);
            this.dgvPortfolioFooterFinalValue.TabIndex = 10;
            this.dgvPortfolioFooterFinalValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioFooterFinalValue_CellFormatting);
            this.dgvPortfolioFooterFinalValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioFooterFinalValue_CellPainting);
            this.dgvPortfolioFooterFinalValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioFooterFinalValue_SelectionChanged);
            this.dgvPortfolioFooterFinalValue.Resize += new System.EventHandler(this.DgvPortfolioFooterFinalValue_Resize);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvPortfolioMarketValue);
            this.tabPage2.Controls.Add(this.dgvPortfolioFooterMarketValue);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1190, 266);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "_tabPageMarketValues";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvPortfolioMarketValue
            // 
            this.dgvPortfolioMarketValue.AllowUserToAddRows = false;
            this.dgvPortfolioMarketValue.AllowUserToDeleteRows = false;
            this.dgvPortfolioMarketValue.AllowUserToResizeColumns = false;
            this.dgvPortfolioMarketValue.AllowUserToResizeRows = false;
            this.dgvPortfolioMarketValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPortfolioMarketValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfolioMarketValue.Location = new System.Drawing.Point(0, 1);
            this.dgvPortfolioMarketValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPortfolioMarketValue.MultiSelect = false;
            this.dgvPortfolioMarketValue.Name = "dgvPortfolioMarketValue";
            this.dgvPortfolioMarketValue.ReadOnly = true;
            this.dgvPortfolioMarketValue.RowHeadersVisible = false;
            this.dgvPortfolioMarketValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioMarketValue.Size = new System.Drawing.Size(1189, 188);
            this.dgvPortfolioMarketValue.TabIndex = 11;
            this.dgvPortfolioMarketValue.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPortfolioMarketValue_CellContentDoubleClick);
            this.dgvPortfolioMarketValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioMarketValue_CellFormatting);
            this.dgvPortfolioMarketValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioMarketValue_CellPainting);
            this.dgvPortfolioMarketValue.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvPortfolioMarketValue_DataBindingComplete);
            this.dgvPortfolioMarketValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioMarketValue_SelectionChanged);
            this.dgvPortfolioMarketValue.MouseEnter += new System.EventHandler(this.dgvPortfolioMarketValue_MouseEnter);
            this.dgvPortfolioMarketValue.Resize += new System.EventHandler(this.DgvPortfolioMarketValue_Resize);
            // 
            // dgvPortfolioFooterMarketValue
            // 
            this.dgvPortfolioFooterMarketValue.AllowUserToAddRows = false;
            this.dgvPortfolioFooterMarketValue.AllowUserToDeleteRows = false;
            this.dgvPortfolioFooterMarketValue.AllowUserToResizeColumns = false;
            this.dgvPortfolioFooterMarketValue.AllowUserToResizeRows = false;
            this.dgvPortfolioFooterMarketValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPortfolioFooterMarketValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfolioFooterMarketValue.ColumnHeadersVisible = false;
            this.dgvPortfolioFooterMarketValue.Enabled = false;
            this.dgvPortfolioFooterMarketValue.EnableHeadersVisualStyles = false;
            this.dgvPortfolioFooterMarketValue.Location = new System.Drawing.Point(0, 189);
            this.dgvPortfolioFooterMarketValue.MultiSelect = false;
            this.dgvPortfolioFooterMarketValue.Name = "dgvPortfolioFooterMarketValue";
            this.dgvPortfolioFooterMarketValue.ReadOnly = true;
            this.dgvPortfolioFooterMarketValue.RowHeadersVisible = false;
            this.dgvPortfolioFooterMarketValue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPortfolioFooterMarketValue.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPortfolioFooterMarketValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioFooterMarketValue.Size = new System.Drawing.Size(1189, 76);
            this.dgvPortfolioFooterMarketValue.TabIndex = 12;
            this.dgvPortfolioFooterMarketValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioFooterMarketValue_CellFormatting);
            this.dgvPortfolioFooterMarketValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioFooterMarketValue_CellPainting);
            this.dgvPortfolioFooterMarketValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioFooterMarketValue_SelectionChanged);
            this.dgvPortfolioFooterMarketValue.Resize += new System.EventHandler(this.DgvPortfolioFooterMarketValue_Resize);
            // 
            // btnClearLogger
            // 
            this.btnClearLogger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearLogger.Enabled = false;
            this.btnClearLogger.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLogger.Image = ((System.Drawing.Image)(resources.GetObject("btnClearLogger.Image")));
            this.btnClearLogger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearLogger.Location = new System.Drawing.Point(1014, 321);
            this.btnClearLogger.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClearLogger.Name = "btnClearLogger";
            this.btnClearLogger.Size = new System.Drawing.Size(194, 33);
            this.btnClearLogger.TabIndex = 11;
            this.btnClearLogger.Text = "&Clear_";
            this.btnClearLogger.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearLogger.UseVisualStyleBackColor = true;
            this.btnClearLogger.Visible = false;
            // 
            // btnRefreshAll
            // 
            this.btnRefreshAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshAll.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshAll.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshAll.Image")));
            this.btnRefreshAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshAll.Location = new System.Drawing.Point(10, 321);
            this.btnRefreshAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefreshAll.Name = "btnRefreshAll";
            this.btnRefreshAll.Size = new System.Drawing.Size(194, 33);
            this.btnRefreshAll.TabIndex = 8;
            this.btnRefreshAll.Text = "&Update all_";
            this.btnRefreshAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshAll.UseVisualStyleBackColor = true;
            this.btnRefreshAll.Click += new System.EventHandler(this.OnBtnRefreshAll_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(813, 321);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(194, 33);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "&Remove_";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(612, 321);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(194, 33);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "&Edit_";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.OnBtnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(412, 321);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(194, 33);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "&Add_";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.OnBtnAdd_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(211, 321);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(194, 33);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "&Update_";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.OnBtnRefresh_Click);
            // 
            // grpBoxShareDetails
            // 
            this.grpBoxShareDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxShareDetails.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.grpBoxShareDetails.Controls.Add(this.tabCtrlDetails);
            this.grpBoxShareDetails.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxShareDetails.Location = new System.Drawing.Point(7, 401);
            this.grpBoxShareDetails.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxShareDetails.Name = "grpBoxShareDetails";
            this.grpBoxShareDetails.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxShareDetails.Size = new System.Drawing.Size(1219, 178);
            this.grpBoxShareDetails.TabIndex = 4;
            this.grpBoxShareDetails.TabStop = false;
            this.grpBoxShareDetails.Text = "Share details_";
            // 
            // tabCtrlDetails
            // 
            this.tabCtrlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlDetails.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlDetails.Controls.Add(this.tabPgDetailsFinalValue);
            this.tabCtrlDetails.Controls.Add(this.tabPgDetailsMarketValue);
            this.tabCtrlDetails.Controls.Add(this.tabPgProfitLoss);
            this.tabCtrlDetails.Controls.Add(this.tabPgDividends);
            this.tabCtrlDetails.Controls.Add(this.tabPgCosts);
            this.tabCtrlDetails.Location = new System.Drawing.Point(10, 20);
            this.tabCtrlDetails.Margin = new System.Windows.Forms.Padding(0);
            this.tabCtrlDetails.Name = "tabCtrlDetails";
            this.tabCtrlDetails.SelectedIndex = 0;
            this.tabCtrlDetails.Size = new System.Drawing.Size(1197, 148);
            this.tabCtrlDetails.TabIndex = 10;
            this.tabCtrlDetails.SelectedIndexChanged += new System.EventHandler(this.TabCtrlDetails_SelectedIndexChanged);
            // 
            // tabPgDetailsFinalValue
            // 
            this.tabPgDetailsFinalValue.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDateValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDate);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueCostsValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDividendValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinaValueCurPrice);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueCost);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsPurchase);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDividend);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueVolume);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDiffSumPrevValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueTotalSum);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDiffSumPrev);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValuePerformance);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDiffPerformancePrevValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueTotalPerformanceValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueDiffPerformancePrev);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueVolumeValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueCurPriceValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValuePurchaseValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueTotalSumValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValueTotalProfitValue);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValuePrevPrice);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalVinalTotalProfit);
            this.tabPgDetailsFinalValue.Controls.Add(this.lblDetailsFinalValuePrevPriceValue);
            this.tabPgDetailsFinalValue.Location = new System.Drawing.Point(4, 26);
            this.tabPgDetailsFinalValue.Margin = new System.Windows.Forms.Padding(0);
            this.tabPgDetailsFinalValue.Name = "tabPgDetailsFinalValue";
            this.tabPgDetailsFinalValue.Size = new System.Drawing.Size(1189, 118);
            this.tabPgDetailsFinalValue.TabIndex = 0;
            this.tabPgDetailsFinalValue.Text = "DetailsFinalValue_";
            // 
            // lblDetailsFinalValueDateValue
            // 
            this.lblDetailsFinalValueDateValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDateValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDateValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDateValue.Location = new System.Drawing.Point(199, 9);
            this.lblDetailsFinalValueDateValue.Name = "lblDetailsFinalValueDateValue";
            this.lblDetailsFinalValueDateValue.Size = new System.Drawing.Size(184, 21);
            this.lblDetailsFinalValueDateValue.TabIndex = 36;
            this.lblDetailsFinalValueDateValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueDate
            // 
            this.lblDetailsFinalValueDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDate.Location = new System.Drawing.Point(8, 9);
            this.lblDetailsFinalValueDate.Name = "lblDetailsFinalValueDate";
            this.lblDetailsFinalValueDate.Size = new System.Drawing.Size(187, 21);
            this.lblDetailsFinalValueDate.TabIndex = 35;
            this.lblDetailsFinalValueDate.Text = "Date_";
            this.lblDetailsFinalValueDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueCostsValue
            // 
            this.lblDetailsFinalValueCostsValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueCostsValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueCostsValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueCostsValue.Location = new System.Drawing.Point(240, 86);
            this.lblDetailsFinalValueCostsValue.Name = "lblDetailsFinalValueCostsValue";
            this.lblDetailsFinalValueCostsValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueCostsValue.TabIndex = 34;
            this.lblDetailsFinalValueCostsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueDividendValue
            // 
            this.lblDetailsFinalValueDividendValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDividendValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDividendValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDividendValue.Location = new System.Drawing.Point(240, 60);
            this.lblDetailsFinalValueDividendValue.Name = "lblDetailsFinalValueDividendValue";
            this.lblDetailsFinalValueDividendValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueDividendValue.TabIndex = 33;
            this.lblDetailsFinalValueDividendValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinaValueCurPrice
            // 
            this.lblDetailsFinaValueCurPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinaValueCurPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinaValueCurPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinaValueCurPrice.Location = new System.Drawing.Point(404, 9);
            this.lblDetailsFinaValueCurPrice.Name = "lblDetailsFinaValueCurPrice";
            this.lblDetailsFinaValueCurPrice.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinaValueCurPrice.TabIndex = 2;
            this.lblDetailsFinaValueCurPrice.Text = "CurPrice_";
            this.lblDetailsFinaValueCurPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueCost
            // 
            this.lblDetailsFinalValueCost.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueCost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueCost.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueCost.Location = new System.Drawing.Point(8, 86);
            this.lblDetailsFinalValueCost.Name = "lblDetailsFinalValueCost";
            this.lblDetailsFinalValueCost.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalValueCost.TabIndex = 32;
            this.lblDetailsFinalValueCost.Text = "TotalCost_";
            this.lblDetailsFinalValueCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsPurchase
            // 
            this.lblDetailsPurchase.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsPurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsPurchase.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsPurchase.Location = new System.Drawing.Point(799, 9);
            this.lblDetailsPurchase.Name = "lblDetailsPurchase";
            this.lblDetailsPurchase.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsPurchase.TabIndex = 4;
            this.lblDetailsPurchase.Text = "Purchase_";
            this.lblDetailsPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueDividend
            // 
            this.lblDetailsFinalValueDividend.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDividend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDividend.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDividend.Location = new System.Drawing.Point(8, 60);
            this.lblDetailsFinalValueDividend.Name = "lblDetailsFinalValueDividend";
            this.lblDetailsFinalValueDividend.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalValueDividend.TabIndex = 31;
            this.lblDetailsFinalValueDividend.Text = "TotalDividend_";
            this.lblDetailsFinalValueDividend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueVolume
            // 
            this.lblDetailsFinalValueVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueVolume.Location = new System.Drawing.Point(8, 34);
            this.lblDetailsFinalValueVolume.Name = "lblDetailsFinalValueVolume";
            this.lblDetailsFinalValueVolume.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalValueVolume.TabIndex = 6;
            this.lblDetailsFinalValueVolume.Text = "Volume_";
            this.lblDetailsFinalValueVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueDiffSumPrevValue
            // 
            this.lblDetailsFinalValueDiffSumPrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDiffSumPrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDiffSumPrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDiffSumPrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsFinalValueDiffSumPrevValue.Location = new System.Drawing.Point(636, 60);
            this.lblDetailsFinalValueDiffSumPrevValue.Name = "lblDetailsFinalValueDiffSumPrevValue";
            this.lblDetailsFinalValueDiffSumPrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueDiffSumPrevValue.TabIndex = 30;
            this.lblDetailsFinalValueDiffSumPrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueTotalSum
            // 
            this.lblDetailsFinalValueTotalSum.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueTotalSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueTotalSum.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueTotalSum.Location = new System.Drawing.Point(799, 86);
            this.lblDetailsFinalValueTotalSum.Name = "lblDetailsFinalValueTotalSum";
            this.lblDetailsFinalValueTotalSum.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalValueTotalSum.TabIndex = 7;
            this.lblDetailsFinalValueTotalSum.Text = "TotalSum_";
            this.lblDetailsFinalValueTotalSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueDiffSumPrev
            // 
            this.lblDetailsFinalValueDiffSumPrev.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDiffSumPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDiffSumPrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDiffSumPrev.Location = new System.Drawing.Point(404, 60);
            this.lblDetailsFinalValueDiffSumPrev.Name = "lblDetailsFinalValueDiffSumPrev";
            this.lblDetailsFinalValueDiffSumPrev.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalValueDiffSumPrev.TabIndex = 29;
            this.lblDetailsFinalValueDiffSumPrev.Text = "DiffSumPrev_";
            this.lblDetailsFinalValueDiffSumPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValuePerformance
            // 
            this.lblDetailsFinalValuePerformance.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValuePerformance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValuePerformance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValuePerformance.Location = new System.Drawing.Point(799, 34);
            this.lblDetailsFinalValuePerformance.Name = "lblDetailsFinalValuePerformance";
            this.lblDetailsFinalValuePerformance.Size = new System.Drawing.Size(228, 22);
            this.lblDetailsFinalValuePerformance.TabIndex = 8;
            this.lblDetailsFinalValuePerformance.Text = "TotalPerformance_";
            this.lblDetailsFinalValuePerformance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueDiffPerformancePrevValue
            // 
            this.lblDetailsFinalValueDiffPerformancePrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDiffPerformancePrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDiffPerformancePrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDiffPerformancePrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsFinalValueDiffPerformancePrevValue.Location = new System.Drawing.Point(636, 34);
            this.lblDetailsFinalValueDiffPerformancePrevValue.Name = "lblDetailsFinalValueDiffPerformancePrevValue";
            this.lblDetailsFinalValueDiffPerformancePrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueDiffPerformancePrevValue.TabIndex = 28;
            this.lblDetailsFinalValueDiffPerformancePrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueTotalPerformanceValue
            // 
            this.lblDetailsFinalValueTotalPerformanceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueTotalPerformanceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueTotalPerformanceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueTotalPerformanceValue.Location = new System.Drawing.Point(1032, 34);
            this.lblDetailsFinalValueTotalPerformanceValue.Name = "lblDetailsFinalValueTotalPerformanceValue";
            this.lblDetailsFinalValueTotalPerformanceValue.Size = new System.Drawing.Size(143, 22);
            this.lblDetailsFinalValueTotalPerformanceValue.TabIndex = 11;
            this.lblDetailsFinalValueTotalPerformanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueDiffPerformancePrev
            // 
            this.lblDetailsFinalValueDiffPerformancePrev.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueDiffPerformancePrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueDiffPerformancePrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueDiffPerformancePrev.Location = new System.Drawing.Point(404, 34);
            this.lblDetailsFinalValueDiffPerformancePrev.Name = "lblDetailsFinalValueDiffPerformancePrev";
            this.lblDetailsFinalValueDiffPerformancePrev.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalValueDiffPerformancePrev.TabIndex = 26;
            this.lblDetailsFinalValueDiffPerformancePrev.Text = "DiffPerformancePrev_";
            this.lblDetailsFinalValueDiffPerformancePrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueVolumeValue
            // 
            this.lblDetailsFinalValueVolumeValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueVolumeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueVolumeValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueVolumeValue.Location = new System.Drawing.Point(240, 34);
            this.lblDetailsFinalValueVolumeValue.Name = "lblDetailsFinalValueVolumeValue";
            this.lblDetailsFinalValueVolumeValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueVolumeValue.TabIndex = 24;
            this.lblDetailsFinalValueVolumeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueCurPriceValue
            // 
            this.lblDetailsFinalValueCurPriceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueCurPriceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueCurPriceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueCurPriceValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsFinalValueCurPriceValue.Location = new System.Drawing.Point(636, 9);
            this.lblDetailsFinalValueCurPriceValue.Name = "lblDetailsFinalValueCurPriceValue";
            this.lblDetailsFinalValueCurPriceValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueCurPriceValue.TabIndex = 13;
            this.lblDetailsFinalValueCurPriceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValuePurchaseValue
            // 
            this.lblDetailsFinalValuePurchaseValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValuePurchaseValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValuePurchaseValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValuePurchaseValue.Location = new System.Drawing.Point(1032, 9);
            this.lblDetailsFinalValuePurchaseValue.Name = "lblDetailsFinalValuePurchaseValue";
            this.lblDetailsFinalValuePurchaseValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValuePurchaseValue.TabIndex = 23;
            this.lblDetailsFinalValuePurchaseValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueTotalSumValue
            // 
            this.lblDetailsFinalValueTotalSumValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueTotalSumValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueTotalSumValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueTotalSumValue.Location = new System.Drawing.Point(1032, 86);
            this.lblDetailsFinalValueTotalSumValue.Name = "lblDetailsFinalValueTotalSumValue";
            this.lblDetailsFinalValueTotalSumValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueTotalSumValue.TabIndex = 16;
            this.lblDetailsFinalValueTotalSumValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValueTotalProfitValue
            // 
            this.lblDetailsFinalValueTotalProfitValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValueTotalProfitValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValueTotalProfitValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValueTotalProfitValue.Location = new System.Drawing.Point(1032, 60);
            this.lblDetailsFinalValueTotalProfitValue.Name = "lblDetailsFinalValueTotalProfitValue";
            this.lblDetailsFinalValueTotalProfitValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValueTotalProfitValue.TabIndex = 22;
            this.lblDetailsFinalValueTotalProfitValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValuePrevPrice
            // 
            this.lblDetailsFinalValuePrevPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValuePrevPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValuePrevPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValuePrevPrice.Location = new System.Drawing.Point(404, 86);
            this.lblDetailsFinalValuePrevPrice.Name = "lblDetailsFinalValuePrevPrice";
            this.lblDetailsFinalValuePrevPrice.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalValuePrevPrice.TabIndex = 17;
            this.lblDetailsFinalValuePrevPrice.Text = "PrevPrice_";
            this.lblDetailsFinalValuePrevPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalVinalTotalProfit
            // 
            this.lblDetailsFinalVinalTotalProfit.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalVinalTotalProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalVinalTotalProfit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalVinalTotalProfit.Location = new System.Drawing.Point(799, 60);
            this.lblDetailsFinalVinalTotalProfit.Name = "lblDetailsFinalVinalTotalProfit";
            this.lblDetailsFinalVinalTotalProfit.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsFinalVinalTotalProfit.TabIndex = 21;
            this.lblDetailsFinalVinalTotalProfit.Text = "TotalProfit_";
            this.lblDetailsFinalVinalTotalProfit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsFinalValuePrevPriceValue
            // 
            this.lblDetailsFinalValuePrevPriceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsFinalValuePrevPriceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsFinalValuePrevPriceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsFinalValuePrevPriceValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsFinalValuePrevPriceValue.Location = new System.Drawing.Point(636, 86);
            this.lblDetailsFinalValuePrevPriceValue.Name = "lblDetailsFinalValuePrevPriceValue";
            this.lblDetailsFinalValuePrevPriceValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsFinalValuePrevPriceValue.TabIndex = 20;
            this.lblDetailsFinalValuePrevPriceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPgDetailsMarketValue
            // 
            this.tabPgDetailsMarketValue.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDateValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDate);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueCostValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDividendValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueCurPrice);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueCost);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValuePurchase);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDividend);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueVolume);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDiffSumPrevValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueTotalSum);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDiffSumPrev);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueTotalPerformance);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDiffPerformancePrevValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueTotalPerformanceValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueDiffPerformancePrev);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueVolumeValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueCurPriceValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValuePurchaseValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueTotalSumValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueTotalProfitValue);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValuePrevPrice);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValueTotalProfit);
            this.tabPgDetailsMarketValue.Controls.Add(this.lblDetailsMarketValuePrevPriceValue);
            this.tabPgDetailsMarketValue.Location = new System.Drawing.Point(4, 26);
            this.tabPgDetailsMarketValue.Name = "tabPgDetailsMarketValue";
            this.tabPgDetailsMarketValue.Size = new System.Drawing.Size(1189, 118);
            this.tabPgDetailsMarketValue.TabIndex = 3;
            this.tabPgDetailsMarketValue.Text = "DetailsMarketValue_";
            // 
            // lblDetailsMarketValueDateValue
            // 
            this.lblDetailsMarketValueDateValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDateValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDateValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDateValue.Location = new System.Drawing.Point(199, 9);
            this.lblDetailsMarketValueDateValue.Name = "lblDetailsMarketValueDateValue";
            this.lblDetailsMarketValueDateValue.Size = new System.Drawing.Size(184, 21);
            this.lblDetailsMarketValueDateValue.TabIndex = 60;
            this.lblDetailsMarketValueDateValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueDate
            // 
            this.lblDetailsMarketValueDate.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDate.Location = new System.Drawing.Point(8, 9);
            this.lblDetailsMarketValueDate.Name = "lblDetailsMarketValueDate";
            this.lblDetailsMarketValueDate.Size = new System.Drawing.Size(187, 21);
            this.lblDetailsMarketValueDate.TabIndex = 59;
            this.lblDetailsMarketValueDate.Text = "Date_";
            this.lblDetailsMarketValueDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueCostValue
            // 
            this.lblDetailsMarketValueCostValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueCostValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueCostValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueCostValue.Location = new System.Drawing.Point(240, 86);
            this.lblDetailsMarketValueCostValue.Name = "lblDetailsMarketValueCostValue";
            this.lblDetailsMarketValueCostValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueCostValue.TabIndex = 58;
            this.lblDetailsMarketValueCostValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueDividendValue
            // 
            this.lblDetailsMarketValueDividendValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDividendValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDividendValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDividendValue.Location = new System.Drawing.Point(240, 60);
            this.lblDetailsMarketValueDividendValue.Name = "lblDetailsMarketValueDividendValue";
            this.lblDetailsMarketValueDividendValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueDividendValue.TabIndex = 57;
            this.lblDetailsMarketValueDividendValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueCurPrice
            // 
            this.lblDetailsMarketValueCurPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueCurPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueCurPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueCurPrice.Location = new System.Drawing.Point(404, 9);
            this.lblDetailsMarketValueCurPrice.Name = "lblDetailsMarketValueCurPrice";
            this.lblDetailsMarketValueCurPrice.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueCurPrice.TabIndex = 37;
            this.lblDetailsMarketValueCurPrice.Text = "CurPrice_";
            this.lblDetailsMarketValueCurPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueCost
            // 
            this.lblDetailsMarketValueCost.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueCost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueCost.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueCost.Location = new System.Drawing.Point(8, 86);
            this.lblDetailsMarketValueCost.Name = "lblDetailsMarketValueCost";
            this.lblDetailsMarketValueCost.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueCost.TabIndex = 56;
            this.lblDetailsMarketValueCost.Text = "TotalCost_";
            this.lblDetailsMarketValueCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValuePurchase
            // 
            this.lblDetailsMarketValuePurchase.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValuePurchase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValuePurchase.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValuePurchase.Location = new System.Drawing.Point(799, 9);
            this.lblDetailsMarketValuePurchase.Name = "lblDetailsMarketValuePurchase";
            this.lblDetailsMarketValuePurchase.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValuePurchase.TabIndex = 38;
            this.lblDetailsMarketValuePurchase.Text = "Purchase_";
            this.lblDetailsMarketValuePurchase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueDividend
            // 
            this.lblDetailsMarketValueDividend.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDividend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDividend.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDividend.Location = new System.Drawing.Point(8, 60);
            this.lblDetailsMarketValueDividend.Name = "lblDetailsMarketValueDividend";
            this.lblDetailsMarketValueDividend.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueDividend.TabIndex = 55;
            this.lblDetailsMarketValueDividend.Text = "TotalDividend_";
            this.lblDetailsMarketValueDividend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueVolume
            // 
            this.lblDetailsMarketValueVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueVolume.Location = new System.Drawing.Point(8, 34);
            this.lblDetailsMarketValueVolume.Name = "lblDetailsMarketValueVolume";
            this.lblDetailsMarketValueVolume.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueVolume.TabIndex = 39;
            this.lblDetailsMarketValueVolume.Text = "Volume_";
            this.lblDetailsMarketValueVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueDiffSumPrevValue
            // 
            this.lblDetailsMarketValueDiffSumPrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDiffSumPrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDiffSumPrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDiffSumPrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsMarketValueDiffSumPrevValue.Location = new System.Drawing.Point(636, 60);
            this.lblDetailsMarketValueDiffSumPrevValue.Name = "lblDetailsMarketValueDiffSumPrevValue";
            this.lblDetailsMarketValueDiffSumPrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueDiffSumPrevValue.TabIndex = 54;
            this.lblDetailsMarketValueDiffSumPrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueTotalSum
            // 
            this.lblDetailsMarketValueTotalSum.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueTotalSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueTotalSum.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueTotalSum.Location = new System.Drawing.Point(799, 86);
            this.lblDetailsMarketValueTotalSum.Name = "lblDetailsMarketValueTotalSum";
            this.lblDetailsMarketValueTotalSum.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueTotalSum.TabIndex = 40;
            this.lblDetailsMarketValueTotalSum.Text = "TotalSum_";
            this.lblDetailsMarketValueTotalSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueDiffSumPrev
            // 
            this.lblDetailsMarketValueDiffSumPrev.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDiffSumPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDiffSumPrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDiffSumPrev.Location = new System.Drawing.Point(404, 60);
            this.lblDetailsMarketValueDiffSumPrev.Name = "lblDetailsMarketValueDiffSumPrev";
            this.lblDetailsMarketValueDiffSumPrev.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueDiffSumPrev.TabIndex = 53;
            this.lblDetailsMarketValueDiffSumPrev.Text = "DiffSumPrev_";
            this.lblDetailsMarketValueDiffSumPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueTotalPerformance
            // 
            this.lblDetailsMarketValueTotalPerformance.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueTotalPerformance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueTotalPerformance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueTotalPerformance.Location = new System.Drawing.Point(799, 34);
            this.lblDetailsMarketValueTotalPerformance.Name = "lblDetailsMarketValueTotalPerformance";
            this.lblDetailsMarketValueTotalPerformance.Size = new System.Drawing.Size(228, 22);
            this.lblDetailsMarketValueTotalPerformance.TabIndex = 41;
            this.lblDetailsMarketValueTotalPerformance.Text = "TotalPerformance_";
            this.lblDetailsMarketValueTotalPerformance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueDiffPerformancePrevValue
            // 
            this.lblDetailsMarketValueDiffPerformancePrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDiffPerformancePrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDiffPerformancePrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDiffPerformancePrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsMarketValueDiffPerformancePrevValue.Location = new System.Drawing.Point(636, 34);
            this.lblDetailsMarketValueDiffPerformancePrevValue.Name = "lblDetailsMarketValueDiffPerformancePrevValue";
            this.lblDetailsMarketValueDiffPerformancePrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueDiffPerformancePrevValue.TabIndex = 52;
            this.lblDetailsMarketValueDiffPerformancePrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueTotalPerformanceValue
            // 
            this.lblDetailsMarketValueTotalPerformanceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueTotalPerformanceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueTotalPerformanceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueTotalPerformanceValue.Location = new System.Drawing.Point(1032, 34);
            this.lblDetailsMarketValueTotalPerformanceValue.Name = "lblDetailsMarketValueTotalPerformanceValue";
            this.lblDetailsMarketValueTotalPerformanceValue.Size = new System.Drawing.Size(143, 22);
            this.lblDetailsMarketValueTotalPerformanceValue.TabIndex = 42;
            this.lblDetailsMarketValueTotalPerformanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueDiffPerformancePrev
            // 
            this.lblDetailsMarketValueDiffPerformancePrev.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueDiffPerformancePrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueDiffPerformancePrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueDiffPerformancePrev.Location = new System.Drawing.Point(404, 34);
            this.lblDetailsMarketValueDiffPerformancePrev.Name = "lblDetailsMarketValueDiffPerformancePrev";
            this.lblDetailsMarketValueDiffPerformancePrev.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueDiffPerformancePrev.TabIndex = 51;
            this.lblDetailsMarketValueDiffPerformancePrev.Text = "DiffPerformancePrev_";
            this.lblDetailsMarketValueDiffPerformancePrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueVolumeValue
            // 
            this.lblDetailsMarketValueVolumeValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueVolumeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueVolumeValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueVolumeValue.Location = new System.Drawing.Point(240, 34);
            this.lblDetailsMarketValueVolumeValue.Name = "lblDetailsMarketValueVolumeValue";
            this.lblDetailsMarketValueVolumeValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueVolumeValue.TabIndex = 50;
            this.lblDetailsMarketValueVolumeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueCurPriceValue
            // 
            this.lblDetailsMarketValueCurPriceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueCurPriceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueCurPriceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueCurPriceValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsMarketValueCurPriceValue.Location = new System.Drawing.Point(636, 9);
            this.lblDetailsMarketValueCurPriceValue.Name = "lblDetailsMarketValueCurPriceValue";
            this.lblDetailsMarketValueCurPriceValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueCurPriceValue.TabIndex = 43;
            this.lblDetailsMarketValueCurPriceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValuePurchaseValue
            // 
            this.lblDetailsMarketValuePurchaseValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValuePurchaseValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValuePurchaseValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValuePurchaseValue.Location = new System.Drawing.Point(1032, 9);
            this.lblDetailsMarketValuePurchaseValue.Name = "lblDetailsMarketValuePurchaseValue";
            this.lblDetailsMarketValuePurchaseValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValuePurchaseValue.TabIndex = 49;
            this.lblDetailsMarketValuePurchaseValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueTotalSumValue
            // 
            this.lblDetailsMarketValueTotalSumValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueTotalSumValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueTotalSumValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueTotalSumValue.Location = new System.Drawing.Point(1032, 86);
            this.lblDetailsMarketValueTotalSumValue.Name = "lblDetailsMarketValueTotalSumValue";
            this.lblDetailsMarketValueTotalSumValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueTotalSumValue.TabIndex = 44;
            this.lblDetailsMarketValueTotalSumValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueTotalProfitValue
            // 
            this.lblDetailsMarketValueTotalProfitValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueTotalProfitValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueTotalProfitValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueTotalProfitValue.Location = new System.Drawing.Point(1032, 60);
            this.lblDetailsMarketValueTotalProfitValue.Name = "lblDetailsMarketValueTotalProfitValue";
            this.lblDetailsMarketValueTotalProfitValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValueTotalProfitValue.TabIndex = 48;
            this.lblDetailsMarketValueTotalProfitValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValuePrevPrice
            // 
            this.lblDetailsMarketValuePrevPrice.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValuePrevPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValuePrevPrice.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValuePrevPrice.Location = new System.Drawing.Point(404, 86);
            this.lblDetailsMarketValuePrevPrice.Name = "lblDetailsMarketValuePrevPrice";
            this.lblDetailsMarketValuePrevPrice.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValuePrevPrice.TabIndex = 45;
            this.lblDetailsMarketValuePrevPrice.Text = "PrevPrice_";
            this.lblDetailsMarketValuePrevPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValueTotalProfit
            // 
            this.lblDetailsMarketValueTotalProfit.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValueTotalProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValueTotalProfit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValueTotalProfit.Location = new System.Drawing.Point(799, 60);
            this.lblDetailsMarketValueTotalProfit.Name = "lblDetailsMarketValueTotalProfit";
            this.lblDetailsMarketValueTotalProfit.Size = new System.Drawing.Size(228, 21);
            this.lblDetailsMarketValueTotalProfit.TabIndex = 47;
            this.lblDetailsMarketValueTotalProfit.Text = "TotalProfit_";
            this.lblDetailsMarketValueTotalProfit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailsMarketValuePrevPriceValue
            // 
            this.lblDetailsMarketValuePrevPriceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblDetailsMarketValuePrevPriceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDetailsMarketValuePrevPriceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailsMarketValuePrevPriceValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDetailsMarketValuePrevPriceValue.Location = new System.Drawing.Point(636, 86);
            this.lblDetailsMarketValuePrevPriceValue.Name = "lblDetailsMarketValuePrevPriceValue";
            this.lblDetailsMarketValuePrevPriceValue.Size = new System.Drawing.Size(143, 21);
            this.lblDetailsMarketValuePrevPriceValue.TabIndex = 46;
            this.lblDetailsMarketValuePrevPriceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPgProfitLoss
            // 
            this.tabPgProfitLoss.AutoScroll = true;
            this.tabPgProfitLoss.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.tabPgProfitLoss.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgProfitLoss.Controls.Add(this.tabCtrlProfitLoss);
            this.tabPgProfitLoss.Location = new System.Drawing.Point(4, 26);
            this.tabPgProfitLoss.Margin = new System.Windows.Forms.Padding(0);
            this.tabPgProfitLoss.Name = "tabPgProfitLoss";
            this.tabPgProfitLoss.Size = new System.Drawing.Size(1189, 118);
            this.tabPgProfitLoss.TabIndex = 4;
            this.tabPgProfitLoss.Text = "ProfitLoss_";
            // 
            // tabCtrlProfitLoss
            // 
            this.tabCtrlProfitLoss.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlProfitLoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlProfitLoss.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlProfitLoss.Name = "tabCtrlProfitLoss";
            this.tabCtrlProfitLoss.SelectedIndex = 0;
            this.tabCtrlProfitLoss.Size = new System.Drawing.Size(1189, 118);
            this.tabCtrlProfitLoss.TabIndex = 0;
            this.tabCtrlProfitLoss.SelectedIndexChanged += new System.EventHandler(this.TabCtrlProfitLoss_SelectedIndexChanged);
            // 
            // tabPgDividends
            // 
            this.tabPgDividends.AutoScroll = true;
            this.tabPgDividends.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.tabPgDividends.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgDividends.Controls.Add(this.tabCtrlDividends);
            this.tabPgDividends.Location = new System.Drawing.Point(4, 26);
            this.tabPgDividends.Margin = new System.Windows.Forms.Padding(0);
            this.tabPgDividends.Name = "tabPgDividends";
            this.tabPgDividends.Size = new System.Drawing.Size(1189, 118);
            this.tabPgDividends.TabIndex = 1;
            this.tabPgDividends.Text = "Dividends_";
            // 
            // tabCtrlDividends
            // 
            this.tabCtrlDividends.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlDividends.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlDividends.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlDividends.Name = "tabCtrlDividends";
            this.tabCtrlDividends.SelectedIndex = 0;
            this.tabCtrlDividends.Size = new System.Drawing.Size(1189, 118);
            this.tabCtrlDividends.TabIndex = 0;
            this.tabCtrlDividends.SelectedIndexChanged += new System.EventHandler(this.TabCtrlDividends_SelectedIndexChanged);
            // 
            // tabPgCosts
            // 
            this.tabPgCosts.AutoScroll = true;
            this.tabPgCosts.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.tabPgCosts.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgCosts.Controls.Add(this.tabCtrlCosts);
            this.tabPgCosts.Location = new System.Drawing.Point(4, 26);
            this.tabPgCosts.Margin = new System.Windows.Forms.Padding(0);
            this.tabPgCosts.Name = "tabPgCosts";
            this.tabPgCosts.Size = new System.Drawing.Size(1189, 118);
            this.tabPgCosts.TabIndex = 2;
            this.tabPgCosts.Text = "Costs_";
            // 
            // tabCtrlCosts
            // 
            this.tabCtrlCosts.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlCosts.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlCosts.Name = "tabCtrlCosts";
            this.tabCtrlCosts.SelectedIndex = 0;
            this.tabCtrlCosts.Size = new System.Drawing.Size(1189, 118);
            this.tabCtrlCosts.TabIndex = 0;
            this.tabCtrlCosts.SelectedIndexChanged += new System.EventHandler(this.TabCtrlCosts_SelectedIndexChanged);
            // 
            // grpBoxStatusMessage
            // 
            this.grpBoxStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxStatusMessage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpBoxStatusMessage.Controls.Add(this.rchTxtBoxStateMessage);
            this.grpBoxStatusMessage.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxStatusMessage.Location = new System.Drawing.Point(7, 587);
            this.grpBoxStatusMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxStatusMessage.Name = "grpBoxStatusMessage";
            this.grpBoxStatusMessage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxStatusMessage.Size = new System.Drawing.Size(768, 108);
            this.grpBoxStatusMessage.TabIndex = 5;
            this.grpBoxStatusMessage.TabStop = false;
            this.grpBoxStatusMessage.Text = "Status message_";
            // 
            // rchTxtBoxStateMessage
            // 
            this.rchTxtBoxStateMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rchTxtBoxStateMessage.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.rchTxtBoxStateMessage.Location = new System.Drawing.Point(10, 23);
            this.rchTxtBoxStateMessage.Name = "rchTxtBoxStateMessage";
            this.rchTxtBoxStateMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rchTxtBoxStateMessage.ShowSelectionMargin = true;
            this.rchTxtBoxStateMessage.Size = new System.Drawing.Size(746, 75);
            this.rchTxtBoxStateMessage.TabIndex = 0;
            this.rchTxtBoxStateMessage.Text = "";
            // 
            // lblWebParserState
            // 
            this.lblWebParserState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWebParserState.BackColor = System.Drawing.Color.Transparent;
            this.lblWebParserState.Font = new System.Drawing.Font("Trebuchet MS", 9.75F);
            this.lblWebParserState.Location = new System.Drawing.Point(15, 43);
            this.lblWebParserState.Name = "lblWebParserState";
            this.lblWebParserState.Size = new System.Drawing.Size(411, 18);
            this.lblWebParserState.TabIndex = 1;
            this.lblWebParserState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBarWebParser
            // 
            this.progressBarWebParser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarWebParser.Location = new System.Drawing.Point(15, 67);
            this.progressBarWebParser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBarWebParser.Name = "progressBarWebParser";
            this.progressBarWebParser.Size = new System.Drawing.Size(411, 33);
            this.progressBarWebParser.TabIndex = 0;
            // 
            // grpBoxUpdateState
            // 
            this.grpBoxUpdateState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxUpdateState.Controls.Add(this.lblWebParserState);
            this.grpBoxUpdateState.Controls.Add(this.lblShareNameWebParser);
            this.grpBoxUpdateState.Controls.Add(this.progressBarWebParser);
            this.grpBoxUpdateState.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxUpdateState.Location = new System.Drawing.Point(787, 587);
            this.grpBoxUpdateState.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxUpdateState.Name = "grpBoxUpdateState";
            this.grpBoxUpdateState.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxUpdateState.Size = new System.Drawing.Size(439, 108);
            this.grpBoxUpdateState.TabIndex = 6;
            this.grpBoxUpdateState.TabStop = false;
            this.grpBoxUpdateState.Text = "Update state_";
            // 
            // lblShareNameWebParser
            // 
            this.lblShareNameWebParser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShareNameWebParser.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareNameWebParser.Location = new System.Drawing.Point(15, 22);
            this.lblShareNameWebParser.Name = "lblShareNameWebParser";
            this.lblShareNameWebParser.Size = new System.Drawing.Size(411, 18);
            this.lblShareNameWebParser.TabIndex = 1;
            // 
            // timerStatusMessageClear
            // 
            this.timerStatusMessageClear.Interval = 2000;
            this.timerStatusMessageClear.Tick += new System.EventHandler(this.TimerStatusMessageDelete_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "SharePortfolioManagment";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.NotifyIcon_Click);
            // 
            // lblShareDetailsWithOutDividendCostSharePriceCurrentValue
            // 
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.Location = new System.Drawing.Point(636, 9);
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.Name = "lblShareDetailsWithOutDividendCostSharePriceCurrentValue";
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.TabIndex = 43;
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1235, 708);
            this.Controls.Add(this.grpBoxUpdateState);
            this.Controls.Add(this.grpBoxStatusMessage);
            this.Controls.Add(this.grpBoxShareDetails);
            this.Controls.Add(this.grpBoxSharePortfolio);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(1251, 746);
            this.Name = "FrmMain";
            this.Text = "Share administration_";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmMain_VisibleChanged);
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpBoxSharePortfolio.ResumeLayout(false);
            this.tabCtrlShareOverviews.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFinalValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterFinalValue)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioMarketValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterMarketValue)).EndInit();
            this.grpBoxShareDetails.ResumeLayout(false);
            this.tabCtrlDetails.ResumeLayout(false);
            this.tabPgDetailsFinalValue.ResumeLayout(false);
            this.tabPgDetailsMarketValue.ResumeLayout(false);
            this.tabPgProfitLoss.ResumeLayout(false);
            this.tabPgDividends.ResumeLayout(false);
            this.tabPgCosts.ResumeLayout(false);
            this.grpBoxStatusMessage.ResumeLayout(false);
            this.grpBoxUpdateState.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private GroupBox grpBoxSharePortfolio;
        private GroupBox grpBoxShareDetails;
        private Button btnRefresh;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnAdd;
        private Label lblDetailsFinaValueCurPrice;
        private Label lblDetailsFinalValuePerformance;
        private Label lblDetailsFinalValueTotalSum;
        private Label lblDetailsFinalValueVolume;
        private Label lblDetailsPurchase;
        private Label lblDetailsFinalValueTotalPerformanceValue;
        private Label lblDetailsFinalValueCurPriceValue;
        private Label lblDetailsFinalValueTotalSumValue;
        private Button btnRefreshAll;
        private Label lblDetailsFinalValuePrevPrice;
        private Label lblDetailsFinalValuePrevPriceValue;
        private DataGridView dgvPortfolioFinalValue;
        private Label lblDetailsFinalVinalTotalProfit;
        private Label lblDetailsFinalValueTotalProfitValue;
        private Label lblDetailsFinalValuePurchaseValue;
        private Label lblDetailsFinalValueVolumeValue;
        private Label lblDetailsFinalValueDiffPerformancePrevValue;
        private Label lblDetailsFinalValueDiffPerformancePrev;
        private Label lblDetailsFinalValueDiffSumPrevValue;
        private Label lblDetailsFinalValueDiffSumPrev;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem languageToolStripMenuItem;
        private GroupBox grpBoxStatusMessage;
        private ProgressBar progressBarWebParser;
        private GroupBox grpBoxUpdateState;
        private Label lblShareNameWebParser;
        private Timer timerStatusMessageClear;
        private Label lblDetailsFinalValueCost;
        private Label lblDetailsFinalValueDividend;
        private Label lblDetailsFinalValueCostsValue;
        private Label lblDetailsFinalValueDividendValue;
        private TabControl tabCtrlDetails;
        private TabPage tabPgDetailsFinalValue;
        private TabPage tabPgCosts;
        private Label lblDetailsFinalValueDateValue;
        private Label lblDetailsFinalValueDate;
        private TabPage tabPgDetailsMarketValue;
        private Label lblDetailsMarketValueDateValue;
        private Label lblDetailsMarketValueDate;
        private Label lblDetailsMarketValueCostValue;
        private Label lblDetailsMarketValueDividendValue;
        private Label lblDetailsMarketValueCurPrice;
        private Label lblDetailsMarketValueCost;
        private Label lblDetailsMarketValuePurchase;
        private Label lblDetailsMarketValueDividend;
        private Label lblDetailsMarketValueVolume;
        private Label lblDetailsMarketValueDiffSumPrevValue;
        private Label lblDetailsMarketValueTotalSum;
        private Label lblDetailsMarketValueDiffSumPrev;
        private Label lblDetailsMarketValueTotalPerformance;
        private Label lblDetailsMarketValueDiffPerformancePrevValue;
        private Label lblDetailsMarketValueTotalPerformanceValue;
        private Label lblDetailsMarketValueDiffPerformancePrev;
        private Label lblDetailsMarketValueVolumeValue;
        private Label lblDetailsMarketValueCurPriceValue;
        private Label lblDetailsMarketValuePurchaseValue;
        private Label lblDetailsMarketValueTotalSumValue;
        private Label lblDetailsMarketValueTotalProfitValue;
        private Label lblDetailsMarketValuePrevPrice;
        private Label lblDetailsMarketValueTotalProfit;
        private Label lblDetailsMarketValuePrevPriceValue;
        private TabPage tabPgDividends;
        private TabControl tabCtrlDividends;
        private TabControl tabCtrlCosts;
        private TabControl tabCtrlProfitLoss;
        private DataGridView dgvPortfolioFooterFinalValue;
        private RichTextBox rchTxtBoxStateMessage;
        private Label lblWebParserState;
        private ToolStripMenuItem loggerToolStripMenuItem;
        private Button btnCancelRefresh;
        private Button btnClearLogger;
        private TabPage tabPgProfitLoss;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private NotifyIcon notifyIcon;
        private TabControl tabCtrlShareOverviews;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView dgvPortfolioMarketValue;
        private DataGridView dgvPortfolioFooterMarketValue;
        private Label lblShareDetailsWithOutDividendCostSharePriceCurrentValue;
    }
}

