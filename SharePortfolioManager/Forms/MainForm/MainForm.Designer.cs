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
            this.btnClearLogger = new System.Windows.Forms.Button();
            this.dgvPortfolioFooter = new System.Windows.Forms.DataGridView();
            this.dgvPortfolio = new System.Windows.Forms.DataGridView();
            this.btnRefreshAll = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grpBoxShareDetails = new System.Windows.Forms.GroupBox();
            this.tabCtrlDetails = new System.Windows.Forms.TabControl();
            this.tabPgShareDetailsWithDividendCosts = new System.Windows.Forms.TabPage();
            this.lblShareDetailsWithDividendCostShareDateValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDate = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareCostValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDividendValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostSharePriceCurrent = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareCost = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDeposit = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDividend = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareVolume = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareTotalSum = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDiffSumPrev = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareTotalPerformance = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareVolumeValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareDepositValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareTotalSumValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareTotalProfitValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostSharePricePrev = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostShareTotalProfit = new System.Windows.Forms.Label();
            this.lblShareDetailsWithDividendCostSharePricePervValue = new System.Windows.Forms.Label();
            this.tabPgShareDetailsWithOutDividendCosts = new System.Windows.Forms.TabPage();
            this.lblShareDetailsWithOutDividendCostShareDateValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDate = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareCostValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDividendValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareCost = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDeposit = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDividend = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareVolume = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareTotalSum = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareVolumeValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareDepositValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostSharePricePrev = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostShareTotalProfit = new System.Windows.Forms.Label();
            this.lblShareDetailsWithOutDividendCostSharePricePervValue = new System.Windows.Forms.Label();
            this.tabPgDividend = new System.Windows.Forms.TabPage();
            this.tabCtrlDividends = new System.Windows.Forms.TabControl();
            this.tabPgCosts = new System.Windows.Forms.TabPage();
            this.tabCtrlCosts = new System.Windows.Forms.TabControl();
            this.tabPgProfitLoss = new System.Windows.Forms.TabPage();
            this.tabCtrlProfitLoss = new System.Windows.Forms.TabControl();
            this.grpBoxStatusMessage = new System.Windows.Forms.GroupBox();
            this.rchTxtBoxStateMessage = new System.Windows.Forms.RichTextBox();
            this.lblWebParserState = new System.Windows.Forms.Label();
            this.progressBarWebParser = new System.Windows.Forms.ProgressBar();
            this.grpBoxUpdateState = new System.Windows.Forms.GroupBox();
            this.lblShareNameWebParser = new System.Windows.Forms.Label();
            this.timerStatusMessageClear = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1.SuspendLayout();
            this.grpBoxSharePortfolio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolio)).BeginInit();
            this.grpBoxShareDetails.SuspendLayout();
            this.tabCtrlDetails.SuspendLayout();
            this.tabPgShareDetailsWithDividendCosts.SuspendLayout();
            this.tabPgShareDetailsWithOutDividendCosts.SuspendLayout();
            this.tabPgDividend.SuspendLayout();
            this.tabPgCosts.SuspendLayout();
            this.tabPgProfitLoss.SuspendLayout();
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
            this.grpBoxSharePortfolio.Controls.Add(this.btnClearLogger);
            this.grpBoxSharePortfolio.Controls.Add(this.dgvPortfolioFooter);
            this.grpBoxSharePortfolio.Controls.Add(this.dgvPortfolio);
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
            // dgvPortfolioFooter
            // 
            this.dgvPortfolioFooter.AllowUserToAddRows = false;
            this.dgvPortfolioFooter.AllowUserToDeleteRows = false;
            this.dgvPortfolioFooter.AllowUserToResizeColumns = false;
            this.dgvPortfolioFooter.AllowUserToResizeRows = false;
            this.dgvPortfolioFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPortfolioFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfolioFooter.ColumnHeadersVisible = false;
            this.dgvPortfolioFooter.Enabled = false;
            this.dgvPortfolioFooter.EnableHeadersVisualStyles = false;
            this.dgvPortfolioFooter.Location = new System.Drawing.Point(10, 235);
            this.dgvPortfolioFooter.MultiSelect = false;
            this.dgvPortfolioFooter.Name = "dgvPortfolioFooter";
            this.dgvPortfolioFooter.ReadOnly = true;
            this.dgvPortfolioFooter.RowHeadersVisible = false;
            this.dgvPortfolioFooter.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPortfolioFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPortfolioFooter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioFooter.Size = new System.Drawing.Size(1197, 82);
            this.dgvPortfolioFooter.TabIndex = 10;
            this.dgvPortfolioFooter.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridViewSharePortfolioFooter_CellFormatting);
            this.dgvPortfolioFooter.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridViewSharePortfolioFooter_CellPainting);
            this.dgvPortfolioFooter.SelectionChanged += new System.EventHandler(this.DataGridViewSharePortfolioFooter_SelectionChanged);
            this.dgvPortfolioFooter.Resize += new System.EventHandler(this.DgvPortfolioFooter_Resize);
            // 
            // dgvPortfolio
            // 
            this.dgvPortfolio.AllowUserToAddRows = false;
            this.dgvPortfolio.AllowUserToDeleteRows = false;
            this.dgvPortfolio.AllowUserToResizeColumns = false;
            this.dgvPortfolio.AllowUserToResizeRows = false;
            this.dgvPortfolio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPortfolio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfolio.Location = new System.Drawing.Point(10, 24);
            this.dgvPortfolio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPortfolio.MultiSelect = false;
            this.dgvPortfolio.Name = "dgvPortfolio";
            this.dgvPortfolio.ReadOnly = true;
            this.dgvPortfolio.RowHeadersVisible = false;
            this.dgvPortfolio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolio.Size = new System.Drawing.Size(1197, 211);
            this.dgvPortfolio.TabIndex = 9;
            this.dgvPortfolio.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridViewSharePortfolio_CellFormatting);
            this.dgvPortfolio.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridViewSharePortfolio_CellPainting);
            this.dgvPortfolio.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DataGridViewSharePortfolio_DataBindingComplete);
            this.dgvPortfolio.SelectionChanged += new System.EventHandler(this.DgvPortfolio_SelectionChanged);
            this.dgvPortfolio.Resize += new System.EventHandler(this.DgvPortfolio_Resize);
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
            this.tabCtrlDetails.Controls.Add(this.tabPgShareDetailsWithDividendCosts);
            this.tabCtrlDetails.Controls.Add(this.tabPgShareDetailsWithOutDividendCosts);
            this.tabCtrlDetails.Controls.Add(this.tabPgDividend);
            this.tabCtrlDetails.Controls.Add(this.tabPgCosts);
            this.tabCtrlDetails.Controls.Add(this.tabPgProfitLoss);
            this.tabCtrlDetails.Location = new System.Drawing.Point(10, 20);
            this.tabCtrlDetails.Margin = new System.Windows.Forms.Padding(0);
            this.tabCtrlDetails.Name = "tabCtrlDetails";
            this.tabCtrlDetails.SelectedIndex = 0;
            this.tabCtrlDetails.Size = new System.Drawing.Size(1197, 148);
            this.tabCtrlDetails.TabIndex = 10;
            this.tabCtrlDetails.SelectedIndexChanged += new System.EventHandler(this.tabCtrlDetails_SelectedIndexChanged);
            // 
            // tabPgShareDetailsWithDividendCosts
            // 
            this.tabPgShareDetailsWithDividendCosts.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDateValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDate);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareCostValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDividendValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostSharePriceCurrent);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareCost);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDeposit);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDividend);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareVolume);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDiffSumPrevValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareTotalSum);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDiffSumPrev);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareTotalPerformance);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareTotalPerformanceValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDiffPerformancePrev);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareVolumeValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostSharePriceCurrentValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareDepositValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareTotalSumValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareTotalProfitValue);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostSharePricePrev);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostShareTotalProfit);
            this.tabPgShareDetailsWithDividendCosts.Controls.Add(this.lblShareDetailsWithDividendCostSharePricePervValue);
            this.tabPgShareDetailsWithDividendCosts.Location = new System.Drawing.Point(4, 26);
            this.tabPgShareDetailsWithDividendCosts.Margin = new System.Windows.Forms.Padding(0);
            this.tabPgShareDetailsWithDividendCosts.Name = "tabPgShareDetailsWithDividendCosts";
            this.tabPgShareDetailsWithDividendCosts.Size = new System.Drawing.Size(1189, 118);
            this.tabPgShareDetailsWithDividendCosts.TabIndex = 0;
            this.tabPgShareDetailsWithDividendCosts.Text = "DetailsWithDividendCost_";
            // 
            // lblShareDetailsWithDividendCostShareDateValue
            // 
            this.lblShareDetailsWithDividendCostShareDateValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDateValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDateValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDateValue.Location = new System.Drawing.Point(199, 9);
            this.lblShareDetailsWithDividendCostShareDateValue.Name = "lblShareDetailsWithDividendCostShareDateValue";
            this.lblShareDetailsWithDividendCostShareDateValue.Size = new System.Drawing.Size(184, 21);
            this.lblShareDetailsWithDividendCostShareDateValue.TabIndex = 36;
            this.lblShareDetailsWithDividendCostShareDateValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDate
            // 
            this.lblShareDetailsWithDividendCostShareDate.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDate.Location = new System.Drawing.Point(8, 9);
            this.lblShareDetailsWithDividendCostShareDate.Name = "lblShareDetailsWithDividendCostShareDate";
            this.lblShareDetailsWithDividendCostShareDate.Size = new System.Drawing.Size(187, 21);
            this.lblShareDetailsWithDividendCostShareDate.TabIndex = 35;
            this.lblShareDetailsWithDividendCostShareDate.Text = "ShareDate_";
            this.lblShareDetailsWithDividendCostShareDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareCostValue
            // 
            this.lblShareDetailsWithDividendCostShareCostValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareCostValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareCostValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareCostValue.Location = new System.Drawing.Point(240, 86);
            this.lblShareDetailsWithDividendCostShareCostValue.Name = "lblShareDetailsWithDividendCostShareCostValue";
            this.lblShareDetailsWithDividendCostShareCostValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareCostValue.TabIndex = 34;
            this.lblShareDetailsWithDividendCostShareCostValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDividendValue
            // 
            this.lblShareDetailsWithDividendCostShareDividendValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDividendValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDividendValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDividendValue.Location = new System.Drawing.Point(240, 60);
            this.lblShareDetailsWithDividendCostShareDividendValue.Name = "lblShareDetailsWithDividendCostShareDividendValue";
            this.lblShareDetailsWithDividendCostShareDividendValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareDividendValue.TabIndex = 33;
            this.lblShareDetailsWithDividendCostShareDividendValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostSharePriceCurrent
            // 
            this.lblShareDetailsWithDividendCostSharePriceCurrent.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostSharePriceCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostSharePriceCurrent.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostSharePriceCurrent.Location = new System.Drawing.Point(404, 9);
            this.lblShareDetailsWithDividendCostSharePriceCurrent.Name = "lblShareDetailsWithDividendCostSharePriceCurrent";
            this.lblShareDetailsWithDividendCostSharePriceCurrent.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostSharePriceCurrent.TabIndex = 2;
            this.lblShareDetailsWithDividendCostSharePriceCurrent.Text = "SharePriceCurrent_";
            this.lblShareDetailsWithDividendCostSharePriceCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareCost
            // 
            this.lblShareDetailsWithDividendCostShareCost.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareCost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareCost.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareCost.Location = new System.Drawing.Point(8, 86);
            this.lblShareDetailsWithDividendCostShareCost.Name = "lblShareDetailsWithDividendCostShareCost";
            this.lblShareDetailsWithDividendCostShareCost.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareCost.TabIndex = 32;
            this.lblShareDetailsWithDividendCostShareCost.Text = "ShareTotalCost_";
            this.lblShareDetailsWithDividendCostShareCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDeposit
            // 
            this.lblShareDetailsWithDividendCostShareDeposit.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDeposit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDeposit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDeposit.Location = new System.Drawing.Point(799, 9);
            this.lblShareDetailsWithDividendCostShareDeposit.Name = "lblShareDetailsWithDividendCostShareDeposit";
            this.lblShareDetailsWithDividendCostShareDeposit.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareDeposit.TabIndex = 4;
            this.lblShareDetailsWithDividendCostShareDeposit.Text = "ShareDeposit_";
            this.lblShareDetailsWithDividendCostShareDeposit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDividend
            // 
            this.lblShareDetailsWithDividendCostShareDividend.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDividend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDividend.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDividend.Location = new System.Drawing.Point(8, 60);
            this.lblShareDetailsWithDividendCostShareDividend.Name = "lblShareDetailsWithDividendCostShareDividend";
            this.lblShareDetailsWithDividendCostShareDividend.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareDividend.TabIndex = 31;
            this.lblShareDetailsWithDividendCostShareDividend.Text = "ShareTotalDividend_";
            this.lblShareDetailsWithDividendCostShareDividend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareVolume
            // 
            this.lblShareDetailsWithDividendCostShareVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareVolume.Location = new System.Drawing.Point(8, 34);
            this.lblShareDetailsWithDividendCostShareVolume.Name = "lblShareDetailsWithDividendCostShareVolume";
            this.lblShareDetailsWithDividendCostShareVolume.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareVolume.TabIndex = 6;
            this.lblShareDetailsWithDividendCostShareVolume.Text = "ShareVolume_";
            this.lblShareDetailsWithDividendCostShareVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDiffSumPrevValue
            // 
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.Location = new System.Drawing.Point(636, 60);
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.Name = "lblShareDetailsWithDividendCostShareDiffSumPrevValue";
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.TabIndex = 30;
            this.lblShareDetailsWithDividendCostShareDiffSumPrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareTotalSum
            // 
            this.lblShareDetailsWithDividendCostShareTotalSum.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareTotalSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareTotalSum.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareTotalSum.Location = new System.Drawing.Point(799, 86);
            this.lblShareDetailsWithDividendCostShareTotalSum.Name = "lblShareDetailsWithDividendCostShareTotalSum";
            this.lblShareDetailsWithDividendCostShareTotalSum.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareTotalSum.TabIndex = 7;
            this.lblShareDetailsWithDividendCostShareTotalSum.Text = "ShareTotalSum_";
            this.lblShareDetailsWithDividendCostShareTotalSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDiffSumPrev
            // 
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.Location = new System.Drawing.Point(404, 60);
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.Name = "lblShareDetailsWithDividendCostShareDiffSumPrev";
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.TabIndex = 29;
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.Text = "ShareDiffSumPrev_";
            this.lblShareDetailsWithDividendCostShareDiffSumPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareTotalPerformance
            // 
            this.lblShareDetailsWithDividendCostShareTotalPerformance.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareTotalPerformance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareTotalPerformance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareTotalPerformance.Location = new System.Drawing.Point(799, 34);
            this.lblShareDetailsWithDividendCostShareTotalPerformance.Name = "lblShareDetailsWithDividendCostShareTotalPerformance";
            this.lblShareDetailsWithDividendCostShareTotalPerformance.Size = new System.Drawing.Size(228, 22);
            this.lblShareDetailsWithDividendCostShareTotalPerformance.TabIndex = 8;
            this.lblShareDetailsWithDividendCostShareTotalPerformance.Text = "ShareTotalPerformance_";
            this.lblShareDetailsWithDividendCostShareTotalPerformance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDiffPerformancePrevValue
            // 
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Location = new System.Drawing.Point(636, 34);
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Name = "lblShareDetailsWithDividendCostShareDiffPerformancePrevValue";
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.TabIndex = 28;
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareTotalPerformanceValue
            // 
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.Location = new System.Drawing.Point(1032, 34);
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.Name = "lblShareDetailsWithDividendCostShareTotalPerformanceValue";
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.Size = new System.Drawing.Size(143, 22);
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.TabIndex = 11;
            this.lblShareDetailsWithDividendCostShareTotalPerformanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDiffPerformancePrev
            // 
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.Location = new System.Drawing.Point(404, 34);
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.Name = "lblShareDetailsWithDividendCostShareDiffPerformancePrev";
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.TabIndex = 26;
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.Text = "ShareDiffPerformancePrev_";
            this.lblShareDetailsWithDividendCostShareDiffPerformancePrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareVolumeValue
            // 
            this.lblShareDetailsWithDividendCostShareVolumeValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareVolumeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareVolumeValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareVolumeValue.Location = new System.Drawing.Point(240, 34);
            this.lblShareDetailsWithDividendCostShareVolumeValue.Name = "lblShareDetailsWithDividendCostShareVolumeValue";
            this.lblShareDetailsWithDividendCostShareVolumeValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareVolumeValue.TabIndex = 24;
            this.lblShareDetailsWithDividendCostShareVolumeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostSharePriceCurrentValue
            // 
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.Location = new System.Drawing.Point(636, 9);
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.Name = "lblShareDetailsWithDividendCostSharePriceCurrentValue";
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.TabIndex = 13;
            this.lblShareDetailsWithDividendCostSharePriceCurrentValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareDepositValue
            // 
            this.lblShareDetailsWithDividendCostShareDepositValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareDepositValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareDepositValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareDepositValue.Location = new System.Drawing.Point(1032, 9);
            this.lblShareDetailsWithDividendCostShareDepositValue.Name = "lblShareDetailsWithDividendCostShareDepositValue";
            this.lblShareDetailsWithDividendCostShareDepositValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareDepositValue.TabIndex = 23;
            this.lblShareDetailsWithDividendCostShareDepositValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareTotalSumValue
            // 
            this.lblShareDetailsWithDividendCostShareTotalSumValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareTotalSumValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareTotalSumValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareTotalSumValue.Location = new System.Drawing.Point(1032, 86);
            this.lblShareDetailsWithDividendCostShareTotalSumValue.Name = "lblShareDetailsWithDividendCostShareTotalSumValue";
            this.lblShareDetailsWithDividendCostShareTotalSumValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareTotalSumValue.TabIndex = 16;
            this.lblShareDetailsWithDividendCostShareTotalSumValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareTotalProfitValue
            // 
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.Location = new System.Drawing.Point(1032, 60);
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.Name = "lblShareDetailsWithDividendCostShareTotalProfitValue";
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.TabIndex = 22;
            this.lblShareDetailsWithDividendCostShareTotalProfitValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostSharePricePrev
            // 
            this.lblShareDetailsWithDividendCostSharePricePrev.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostSharePricePrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostSharePricePrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostSharePricePrev.Location = new System.Drawing.Point(404, 86);
            this.lblShareDetailsWithDividendCostSharePricePrev.Name = "lblShareDetailsWithDividendCostSharePricePrev";
            this.lblShareDetailsWithDividendCostSharePricePrev.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostSharePricePrev.TabIndex = 17;
            this.lblShareDetailsWithDividendCostSharePricePrev.Text = "SharePricePrev_";
            this.lblShareDetailsWithDividendCostSharePricePrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostShareTotalProfit
            // 
            this.lblShareDetailsWithDividendCostShareTotalProfit.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostShareTotalProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostShareTotalProfit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostShareTotalProfit.Location = new System.Drawing.Point(799, 60);
            this.lblShareDetailsWithDividendCostShareTotalProfit.Name = "lblShareDetailsWithDividendCostShareTotalProfit";
            this.lblShareDetailsWithDividendCostShareTotalProfit.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithDividendCostShareTotalProfit.TabIndex = 21;
            this.lblShareDetailsWithDividendCostShareTotalProfit.Text = "ShareTotalProfit_";
            this.lblShareDetailsWithDividendCostShareTotalProfit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithDividendCostSharePricePervValue
            // 
            this.lblShareDetailsWithDividendCostSharePricePervValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithDividendCostSharePricePervValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithDividendCostSharePricePervValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithDividendCostSharePricePervValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithDividendCostSharePricePervValue.Location = new System.Drawing.Point(636, 86);
            this.lblShareDetailsWithDividendCostSharePricePervValue.Name = "lblShareDetailsWithDividendCostSharePricePervValue";
            this.lblShareDetailsWithDividendCostSharePricePervValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithDividendCostSharePricePervValue.TabIndex = 20;
            this.lblShareDetailsWithDividendCostSharePricePervValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPgShareDetailsWithOutDividendCosts
            // 
            this.tabPgShareDetailsWithOutDividendCosts.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDateValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDate);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareCostValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDividendValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostSharePriceCurrent);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareCost);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDeposit);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDividend);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareVolume);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareTotalSum);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDiffSumPrev);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareTotalPerformance);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareVolumeValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostSharePriceCurrentValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareDepositValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareTotalSumValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareTotalProfitValue);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostSharePricePrev);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostShareTotalProfit);
            this.tabPgShareDetailsWithOutDividendCosts.Controls.Add(this.lblShareDetailsWithOutDividendCostSharePricePervValue);
            this.tabPgShareDetailsWithOutDividendCosts.Location = new System.Drawing.Point(4, 26);
            this.tabPgShareDetailsWithOutDividendCosts.Name = "tabPgShareDetailsWithOutDividendCosts";
            this.tabPgShareDetailsWithOutDividendCosts.Size = new System.Drawing.Size(1189, 118);
            this.tabPgShareDetailsWithOutDividendCosts.TabIndex = 3;
            this.tabPgShareDetailsWithOutDividendCosts.Text = "DetailsWithOutDividendCosts";
            // 
            // lblShareDetailsWithOutDividendCostShareDateValue
            // 
            this.lblShareDetailsWithOutDividendCostShareDateValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDateValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDateValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDateValue.Location = new System.Drawing.Point(199, 9);
            this.lblShareDetailsWithOutDividendCostShareDateValue.Name = "lblShareDetailsWithOutDividendCostShareDateValue";
            this.lblShareDetailsWithOutDividendCostShareDateValue.Size = new System.Drawing.Size(184, 21);
            this.lblShareDetailsWithOutDividendCostShareDateValue.TabIndex = 60;
            this.lblShareDetailsWithOutDividendCostShareDateValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDate
            // 
            this.lblShareDetailsWithOutDividendCostShareDate.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDate.Location = new System.Drawing.Point(8, 9);
            this.lblShareDetailsWithOutDividendCostShareDate.Name = "lblShareDetailsWithOutDividendCostShareDate";
            this.lblShareDetailsWithOutDividendCostShareDate.Size = new System.Drawing.Size(187, 21);
            this.lblShareDetailsWithOutDividendCostShareDate.TabIndex = 59;
            this.lblShareDetailsWithOutDividendCostShareDate.Text = "ShareDate_";
            this.lblShareDetailsWithOutDividendCostShareDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareCostValue
            // 
            this.lblShareDetailsWithOutDividendCostShareCostValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareCostValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareCostValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareCostValue.Location = new System.Drawing.Point(240, 86);
            this.lblShareDetailsWithOutDividendCostShareCostValue.Name = "lblShareDetailsWithOutDividendCostShareCostValue";
            this.lblShareDetailsWithOutDividendCostShareCostValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareCostValue.TabIndex = 58;
            this.lblShareDetailsWithOutDividendCostShareCostValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDividendValue
            // 
            this.lblShareDetailsWithOutDividendCostShareDividendValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDividendValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDividendValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDividendValue.Location = new System.Drawing.Point(240, 60);
            this.lblShareDetailsWithOutDividendCostShareDividendValue.Name = "lblShareDetailsWithOutDividendCostShareDividendValue";
            this.lblShareDetailsWithOutDividendCostShareDividendValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareDividendValue.TabIndex = 57;
            this.lblShareDetailsWithOutDividendCostShareDividendValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostSharePriceCurrent
            // 
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.Location = new System.Drawing.Point(404, 9);
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.Name = "lblShareDetailsWithOutDividendCostSharePriceCurrent";
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.TabIndex = 37;
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.Text = "SharePriceCurrent_";
            this.lblShareDetailsWithOutDividendCostSharePriceCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareCost
            // 
            this.lblShareDetailsWithOutDividendCostShareCost.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareCost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareCost.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareCost.Location = new System.Drawing.Point(8, 86);
            this.lblShareDetailsWithOutDividendCostShareCost.Name = "lblShareDetailsWithOutDividendCostShareCost";
            this.lblShareDetailsWithOutDividendCostShareCost.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareCost.TabIndex = 56;
            this.lblShareDetailsWithOutDividendCostShareCost.Text = "ShareTotalCost_";
            this.lblShareDetailsWithOutDividendCostShareCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDeposit
            // 
            this.lblShareDetailsWithOutDividendCostShareDeposit.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDeposit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDeposit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDeposit.Location = new System.Drawing.Point(799, 9);
            this.lblShareDetailsWithOutDividendCostShareDeposit.Name = "lblShareDetailsWithOutDividendCostShareDeposit";
            this.lblShareDetailsWithOutDividendCostShareDeposit.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareDeposit.TabIndex = 38;
            this.lblShareDetailsWithOutDividendCostShareDeposit.Text = "ShareDeposit_";
            this.lblShareDetailsWithOutDividendCostShareDeposit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDividend
            // 
            this.lblShareDetailsWithOutDividendCostShareDividend.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDividend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDividend.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDividend.Location = new System.Drawing.Point(8, 60);
            this.lblShareDetailsWithOutDividendCostShareDividend.Name = "lblShareDetailsWithOutDividendCostShareDividend";
            this.lblShareDetailsWithOutDividendCostShareDividend.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareDividend.TabIndex = 55;
            this.lblShareDetailsWithOutDividendCostShareDividend.Text = "ShareTotalDividend_";
            this.lblShareDetailsWithOutDividendCostShareDividend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareVolume
            // 
            this.lblShareDetailsWithOutDividendCostShareVolume.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareVolume.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareVolume.Location = new System.Drawing.Point(8, 34);
            this.lblShareDetailsWithOutDividendCostShareVolume.Name = "lblShareDetailsWithOutDividendCostShareVolume";
            this.lblShareDetailsWithOutDividendCostShareVolume.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareVolume.TabIndex = 39;
            this.lblShareDetailsWithOutDividendCostShareVolume.Text = "ShareVolume_";
            this.lblShareDetailsWithOutDividendCostShareVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDiffSumPrevValue
            // 
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.Location = new System.Drawing.Point(636, 60);
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.Name = "lblShareDetailsWithOutDividendCostShareDiffSumPrevValue";
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.TabIndex = 54;
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareTotalSum
            // 
            this.lblShareDetailsWithOutDividendCostShareTotalSum.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareTotalSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareTotalSum.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareTotalSum.Location = new System.Drawing.Point(799, 86);
            this.lblShareDetailsWithOutDividendCostShareTotalSum.Name = "lblShareDetailsWithOutDividendCostShareTotalSum";
            this.lblShareDetailsWithOutDividendCostShareTotalSum.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareTotalSum.TabIndex = 40;
            this.lblShareDetailsWithOutDividendCostShareTotalSum.Text = "ShareTotalSum_";
            this.lblShareDetailsWithOutDividendCostShareTotalSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDiffSumPrev
            // 
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.Location = new System.Drawing.Point(404, 60);
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.Name = "lblShareDetailsWithOutDividendCostShareDiffSumPrev";
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.TabIndex = 53;
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.Text = "ShareDiffSumPrev_";
            this.lblShareDetailsWithOutDividendCostShareDiffSumPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareTotalPerformance
            // 
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.Location = new System.Drawing.Point(799, 34);
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.Name = "lblShareDetailsWithOutDividendCostShareTotalPerformance";
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.Size = new System.Drawing.Size(228, 22);
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.TabIndex = 41;
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.Text = "ShareTotalPerformance_";
            this.lblShareDetailsWithOutDividendCostShareTotalPerformance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue
            // 
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.Location = new System.Drawing.Point(636, 34);
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.Name = "lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue";
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.TabIndex = 52;
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareTotalPerformanceValue
            // 
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.Location = new System.Drawing.Point(1032, 34);
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.Name = "lblShareDetailsWithOutDividendCostShareTotalPerformanceValue";
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.Size = new System.Drawing.Size(143, 22);
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.TabIndex = 42;
            this.lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareDiffPerformancePrev
            // 
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.Location = new System.Drawing.Point(404, 34);
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.Name = "lblShareDetailsWithOutDividendCostShareDiffPerformancePrev";
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.TabIndex = 51;
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.Text = "ShareDiffPerformancePrev_";
            this.lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareVolumeValue
            // 
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.Location = new System.Drawing.Point(240, 34);
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.Name = "lblShareDetailsWithOutDividendCostShareVolumeValue";
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.TabIndex = 50;
            this.lblShareDetailsWithOutDividendCostShareVolumeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // lblShareDetailsWithOutDividendCostShareDepositValue
            // 
            this.lblShareDetailsWithOutDividendCostShareDepositValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareDepositValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareDepositValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareDepositValue.Location = new System.Drawing.Point(1032, 9);
            this.lblShareDetailsWithOutDividendCostShareDepositValue.Name = "lblShareDetailsWithOutDividendCostShareDepositValue";
            this.lblShareDetailsWithOutDividendCostShareDepositValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareDepositValue.TabIndex = 49;
            this.lblShareDetailsWithOutDividendCostShareDepositValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareTotalSumValue
            // 
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.Location = new System.Drawing.Point(1032, 86);
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.Name = "lblShareDetailsWithOutDividendCostShareTotalSumValue";
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.TabIndex = 44;
            this.lblShareDetailsWithOutDividendCostShareTotalSumValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareTotalProfitValue
            // 
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.Location = new System.Drawing.Point(1032, 60);
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.Name = "lblShareDetailsWithOutDividendCostShareTotalProfitValue";
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.TabIndex = 48;
            this.lblShareDetailsWithOutDividendCostShareTotalProfitValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostSharePricePrev
            // 
            this.lblShareDetailsWithOutDividendCostSharePricePrev.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostSharePricePrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostSharePricePrev.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostSharePricePrev.Location = new System.Drawing.Point(404, 86);
            this.lblShareDetailsWithOutDividendCostSharePricePrev.Name = "lblShareDetailsWithOutDividendCostSharePricePrev";
            this.lblShareDetailsWithOutDividendCostSharePricePrev.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostSharePricePrev.TabIndex = 45;
            this.lblShareDetailsWithOutDividendCostSharePricePrev.Text = "SharePricePrev_";
            this.lblShareDetailsWithOutDividendCostSharePricePrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostShareTotalProfit
            // 
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.Location = new System.Drawing.Point(799, 60);
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.Name = "lblShareDetailsWithOutDividendCostShareTotalProfit";
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.Size = new System.Drawing.Size(228, 21);
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.TabIndex = 47;
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.Text = "ShareTotalProfit_";
            this.lblShareDetailsWithOutDividendCostShareTotalProfit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShareDetailsWithOutDividendCostSharePricePervValue
            // 
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.Location = new System.Drawing.Point(636, 86);
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.Name = "lblShareDetailsWithOutDividendCostSharePricePervValue";
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.TabIndex = 46;
            this.lblShareDetailsWithOutDividendCostSharePricePervValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPgDividend
            // 
            this.tabPgDividend.AutoScroll = true;
            this.tabPgDividend.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.tabPgDividend.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tabPgDividend.Controls.Add(this.tabCtrlDividends);
            this.tabPgDividend.Location = new System.Drawing.Point(4, 26);
            this.tabPgDividend.Margin = new System.Windows.Forms.Padding(0);
            this.tabPgDividend.Name = "tabPgDividend";
            this.tabPgDividend.Size = new System.Drawing.Size(1189, 118);
            this.tabPgDividend.TabIndex = 1;
            this.tabPgDividend.Text = "DividendPayOuts_";
            // 
            // tabCtrlDividends
            // 
            this.tabCtrlDividends.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCtrlDividends.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlDividends.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlDividends.Name = "tabCtrlDividends";
            this.tabCtrlDividends.SelectedIndex = 0;
            this.tabCtrlDividends.Size = new System.Drawing.Size(1188, 115);
            this.tabCtrlDividends.TabIndex = 0;
            this.tabCtrlDividends.SelectedIndexChanged += new System.EventHandler(this.tabCtrlDividends_SelectedIndexChanged);
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
            this.tabCtrlCosts.Size = new System.Drawing.Size(1188, 115);
            this.tabCtrlCosts.TabIndex = 0;
            this.tabCtrlCosts.SelectedIndexChanged += new System.EventHandler(this.tabCtrlCosts_SelectedIndexChanged);
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
            this.tabCtrlProfitLoss.Size = new System.Drawing.Size(1188, 115);
            this.tabCtrlProfitLoss.TabIndex = 0;
            this.tabCtrlProfitLoss.SelectedIndexChanged += new System.EventHandler(this.tabCtrlProfitLoss_SelectedIndexChanged);
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
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolio)).EndInit();
            this.grpBoxShareDetails.ResumeLayout(false);
            this.tabCtrlDetails.ResumeLayout(false);
            this.tabPgShareDetailsWithDividendCosts.ResumeLayout(false);
            this.tabPgShareDetailsWithOutDividendCosts.ResumeLayout(false);
            this.tabPgDividend.ResumeLayout(false);
            this.tabPgCosts.ResumeLayout(false);
            this.tabPgProfitLoss.ResumeLayout(false);
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
        private Label lblShareDetailsWithDividendCostSharePriceCurrent;
        private Label lblShareDetailsWithDividendCostShareTotalPerformance;
        private Label lblShareDetailsWithDividendCostShareTotalSum;
        private Label lblShareDetailsWithDividendCostShareVolume;
        private Label lblShareDetailsWithDividendCostShareDeposit;
        private Label lblShareDetailsWithDividendCostShareTotalPerformanceValue;
        private Label lblShareDetailsWithDividendCostSharePriceCurrentValue;
        private Label lblShareDetailsWithDividendCostShareTotalSumValue;
        private Button btnRefreshAll;
        private Label lblShareDetailsWithDividendCostSharePricePrev;
        private Label lblShareDetailsWithDividendCostSharePricePervValue;
        private DataGridView dgvPortfolio;
        private Label lblShareDetailsWithDividendCostShareTotalProfit;
        private Label lblShareDetailsWithDividendCostShareTotalProfitValue;
        private Label lblShareDetailsWithDividendCostShareDepositValue;
        private Label lblShareDetailsWithDividendCostShareVolumeValue;
        private Label lblShareDetailsWithDividendCostShareDiffPerformancePrevValue;
        private Label lblShareDetailsWithDividendCostShareDiffPerformancePrev;
        private Label lblShareDetailsWithDividendCostShareDiffSumPrevValue;
        private Label lblShareDetailsWithDividendCostShareDiffSumPrev;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem languageToolStripMenuItem;
        private GroupBox grpBoxStatusMessage;
        private ProgressBar progressBarWebParser;
        private GroupBox grpBoxUpdateState;
        private Label lblShareNameWebParser;
        private Timer timerStatusMessageClear;
        private Label lblShareDetailsWithDividendCostShareCost;
        private Label lblShareDetailsWithDividendCostShareDividend;
        private Label lblShareDetailsWithDividendCostShareCostValue;
        private Label lblShareDetailsWithDividendCostShareDividendValue;
        private TabControl tabCtrlDetails;
        private TabPage tabPgShareDetailsWithDividendCosts;
        private TabPage tabPgCosts;
        private Label lblShareDetailsWithDividendCostShareDateValue;
        private Label lblShareDetailsWithDividendCostShareDate;
        private TabPage tabPgShareDetailsWithOutDividendCosts;
        private Label lblShareDetailsWithOutDividendCostShareDateValue;
        private Label lblShareDetailsWithOutDividendCostShareDate;
        private Label lblShareDetailsWithOutDividendCostShareCostValue;
        private Label lblShareDetailsWithOutDividendCostShareDividendValue;
        private Label lblShareDetailsWithOutDividendCostSharePriceCurrent;
        private Label lblShareDetailsWithOutDividendCostShareCost;
        private Label lblShareDetailsWithOutDividendCostShareDeposit;
        private Label lblShareDetailsWithOutDividendCostShareDividend;
        private Label lblShareDetailsWithOutDividendCostShareVolume;
        private Label lblShareDetailsWithOutDividendCostShareDiffSumPrevValue;
        private Label lblShareDetailsWithOutDividendCostShareTotalSum;
        private Label lblShareDetailsWithOutDividendCostShareDiffSumPrev;
        private Label lblShareDetailsWithOutDividendCostShareTotalPerformance;
        private Label lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue;
        private Label lblShareDetailsWithOutDividendCostShareTotalPerformanceValue;
        private Label lblShareDetailsWithOutDividendCostShareDiffPerformancePrev;
        private Label lblShareDetailsWithOutDividendCostShareVolumeValue;
        private Label lblShareDetailsWithOutDividendCostSharePriceCurrentValue;
        private Label lblShareDetailsWithOutDividendCostShareDepositValue;
        private Label lblShareDetailsWithOutDividendCostShareTotalSumValue;
        private Label lblShareDetailsWithOutDividendCostShareTotalProfitValue;
        private Label lblShareDetailsWithOutDividendCostSharePricePrev;
        private Label lblShareDetailsWithOutDividendCostShareTotalProfit;
        private Label lblShareDetailsWithOutDividendCostSharePricePervValue;
        private TabPage tabPgDividend;
        private TabControl tabCtrlDividends;
        private TabControl tabCtrlCosts;
        private TabControl tabCtrlProfitLoss;
        private DataGridView dgvPortfolioFooter;
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
    }
}

