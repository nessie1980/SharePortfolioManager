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
            this.tblLayPnlShareOverviews = new System.Windows.Forms.TableLayoutPanel();
            this.btnRefreshAll = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabCtrlShareOverviews = new System.Windows.Forms.TabControl();
            this.tabPgFinalValue = new System.Windows.Forms.TabPage();
            this.dgvPortfolioFinalValue = new System.Windows.Forms.DataGridView();
            this.dgvPortfolioFooterFinalValue = new System.Windows.Forms.DataGridView();
            this.tabPgMarketValue = new System.Windows.Forms.TabPage();
            this.dgvPortfolioMarketValue = new System.Windows.Forms.DataGridView();
            this.dgvPortfolioFooterMarketValue = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClearLogger = new System.Windows.Forms.Button();
            this.grpBoxStatusMessage = new System.Windows.Forms.GroupBox();
            this.tblLayPnlStatusMessages = new System.Windows.Forms.TableLayoutPanel();
            this.rchTxtBoxStateMessage = new System.Windows.Forms.RichTextBox();
            this.lblWebParserDailyValuesState = new System.Windows.Forms.Label();
            this.progressBarWebParserMarketValues = new System.Windows.Forms.ProgressBar();
            this.lblWebParserMarketValuesState = new System.Windows.Forms.Label();
            this.grpBoxUpdateState = new System.Windows.Forms.GroupBox();
            this.tblLayPnlUpdateState = new System.Windows.Forms.TableLayoutPanel();
            this.progressBarWebParserDailyValues = new System.Windows.Forms.ProgressBar();
            this.timerStatusMessageClear = new System.Windows.Forms.Timer(this.components);
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue = new System.Windows.Forms.Label();
            this.timerStartNextShareUpdate = new System.Windows.Forms.Timer(this.components);
            this.timerMouseCellDownDoubleClick = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.grpBoxSharePortfolio.SuspendLayout();
            this.tblLayPnlShareOverviews.SuspendLayout();
            this.tabCtrlShareOverviews.SuspendLayout();
            this.tabPgFinalValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFinalValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterFinalValue)).BeginInit();
            this.tabPgMarketValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioMarketValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterMarketValue)).BeginInit();
            this.grpBoxStatusMessage.SuspendLayout();
            this.tblLayPnlStatusMessages.SuspendLayout();
            this.grpBoxUpdateState.SuspendLayout();
            this.tblLayPnlUpdateState.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.newToolStripMenuItem.Image = global::SharePortfolioManager.Properties.Resources.menu_file_add2_24;
            this.newToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(131, 30);
            this.newToolStripMenuItem.Text = "New_";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.OnNewToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::SharePortfolioManager.Properties.Resources.menu_folder_open_24;
            this.openToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(131, 30);
            this.openToolStripMenuItem.Text = "Open_";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OnOpenToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::SharePortfolioManager.Properties.Resources.button_save_as_24;
            this.saveAsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(131, 30);
            this.saveAsToolStripMenuItem.Text = "SaveAs_";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(128, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::SharePortfolioManager.Properties.Resources.button_exit_24;
            this.exitToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(131, 30);
            this.exitToolStripMenuItem.Text = "&Quit_";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExitToolStripMenuItem_Click);
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
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(145, 30);
            this.languageToolStripMenuItem.Text = "_Language";
            // 
            // loggerToolStripMenuItem
            // 
            this.loggerToolStripMenuItem.Image = global::SharePortfolioManager.Properties.Resources.menu_eventlog_24;
            this.loggerToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.loggerToolStripMenuItem.Name = "loggerToolStripMenuItem";
            this.loggerToolStripMenuItem.Size = new System.Drawing.Size(145, 30);
            this.loggerToolStripMenuItem.Text = "_Logger";
            this.loggerToolStripMenuItem.Click += new System.EventHandler(this.OnLoggerToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.helpToolStripMenuItem.Text = "&Help_";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::SharePortfolioManager.Properties.Resources.menu_about_24;
            this.aboutToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(124, 30);
            this.aboutToolStripMenuItem.Text = "&About_";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutToolStripMenuItem_Click);
            // 
            // grpBoxSharePortfolio
            // 
            this.grpBoxSharePortfolio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxSharePortfolio.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.grpBoxSharePortfolio.Controls.Add(this.tblLayPnlShareOverviews);
            this.grpBoxSharePortfolio.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxSharePortfolio.Location = new System.Drawing.Point(5, 25);
            this.grpBoxSharePortfolio.Name = "grpBoxSharePortfolio";
            this.grpBoxSharePortfolio.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxSharePortfolio.Size = new System.Drawing.Size(1225, 558);
            this.grpBoxSharePortfolio.TabIndex = 3;
            this.grpBoxSharePortfolio.TabStop = false;
            this.grpBoxSharePortfolio.Text = "Share portfolio_";
            // 
            // tblLayPnlShareOverviews
            // 
            this.tblLayPnlShareOverviews.ColumnCount = 7;
            this.tblLayPnlShareOverviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlShareOverviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlShareOverviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlShareOverviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlShareOverviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlShareOverviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlShareOverviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlShareOverviews.Controls.Add(this.btnRefreshAll, 0, 1);
            this.tblLayPnlShareOverviews.Controls.Add(this.btnEdit, 3, 1);
            this.tblLayPnlShareOverviews.Controls.Add(this.btnAdd, 2, 1);
            this.tblLayPnlShareOverviews.Controls.Add(this.btnRefresh, 1, 1);
            this.tblLayPnlShareOverviews.Controls.Add(this.tabCtrlShareOverviews, 0, 0);
            this.tblLayPnlShareOverviews.Controls.Add(this.btnDelete, 4, 1);
            this.tblLayPnlShareOverviews.Controls.Add(this.btnClearLogger, 5, 1);
            this.tblLayPnlShareOverviews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlShareOverviews.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlShareOverviews.Margin = new System.Windows.Forms.Padding(1);
            this.tblLayPnlShareOverviews.Name = "tblLayPnlShareOverviews";
            this.tblLayPnlShareOverviews.RowCount = 2;
            this.tblLayPnlShareOverviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlShareOverviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tblLayPnlShareOverviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlShareOverviews.Size = new System.Drawing.Size(1219, 535);
            this.tblLayPnlShareOverviews.TabIndex = 13;
            // 
            // btnRefreshAll
            // 
            this.btnRefreshAll.AutoSize = true;
            this.btnRefreshAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefreshAll.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshAll.Image = global::SharePortfolioManager.Properties.Resources.button_update_all_24;
            this.btnRefreshAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshAll.Location = new System.Drawing.Point(1, 501);
            this.btnRefreshAll.Margin = new System.Windows.Forms.Padding(1);
            this.btnRefreshAll.Name = "btnRefreshAll";
            this.btnRefreshAll.Size = new System.Drawing.Size(178, 33);
            this.btnRefreshAll.TabIndex = 8;
            this.btnRefreshAll.Text = "&Update all_";
            this.btnRefreshAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefreshAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefreshAll.UseVisualStyleBackColor = true;
            this.btnRefreshAll.Click += new System.EventHandler(this.OnBtnRefreshAll_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Image = global::SharePortfolioManager.Properties.Resources.button_pencil_24;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(541, 501);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(1);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(178, 33);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "&Edit_";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.OnBtnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = global::SharePortfolioManager.Properties.Resources.button_add_24;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(361, 501);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(178, 33);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "&Add_";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.OnBtnAdd_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Image = global::SharePortfolioManager.Properties.Resources.button_update_24;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(181, 501);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(178, 33);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "&Update_";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.OnBtnRefresh_Click);
            // 
            // tabCtrlShareOverviews
            // 
            this.tabCtrlShareOverviews.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tblLayPnlShareOverviews.SetColumnSpan(this.tabCtrlShareOverviews, 7);
            this.tabCtrlShareOverviews.Controls.Add(this.tabPgFinalValue);
            this.tabCtrlShareOverviews.Controls.Add(this.tabPgMarketValue);
            this.tabCtrlShareOverviews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlShareOverviews.Location = new System.Drawing.Point(3, 0);
            this.tabCtrlShareOverviews.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tabCtrlShareOverviews.Name = "tabCtrlShareOverviews";
            this.tabCtrlShareOverviews.Padding = new System.Drawing.Point(0, 0);
            this.tabCtrlShareOverviews.SelectedIndex = 0;
            this.tabCtrlShareOverviews.Size = new System.Drawing.Size(1213, 500);
            this.tabCtrlShareOverviews.TabIndex = 12;
            // 
            // tabPgFinalValue
            // 
            this.tabPgFinalValue.Controls.Add(this.dgvPortfolioFinalValue);
            this.tabPgFinalValue.Controls.Add(this.dgvPortfolioFooterFinalValue);
            this.tabPgFinalValue.Location = new System.Drawing.Point(4, 26);
            this.tabPgFinalValue.Name = "tabPgFinalValue";
            this.tabPgFinalValue.Padding = new System.Windows.Forms.Padding(3);
            this.tabPgFinalValue.Size = new System.Drawing.Size(1205, 470);
            this.tabPgFinalValue.TabIndex = 0;
            this.tabPgFinalValue.Text = "_tabPageCompleteDepotValues";
            this.tabPgFinalValue.UseVisualStyleBackColor = true;
            // 
            // dgvPortfolioFinalValue
            // 
            this.dgvPortfolioFinalValue.AllowUserToAddRows = false;
            this.dgvPortfolioFinalValue.AllowUserToDeleteRows = false;
            this.dgvPortfolioFinalValue.AllowUserToResizeColumns = false;
            this.dgvPortfolioFinalValue.AllowUserToResizeRows = false;
            this.dgvPortfolioFinalValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfolioFinalValue.Location = new System.Drawing.Point(0, 0);
            this.dgvPortfolioFinalValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPortfolioFinalValue.MultiSelect = false;
            this.dgvPortfolioFinalValue.Name = "dgvPortfolioFinalValue";
            this.dgvPortfolioFinalValue.ReadOnly = true;
            this.dgvPortfolioFinalValue.RowHeadersVisible = false;
            this.dgvPortfolioFinalValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioFinalValue.Size = new System.Drawing.Size(1204, 396);
            this.dgvPortfolioFinalValue.TabIndex = 9;
            this.dgvPortfolioFinalValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioFinalValue_CellFormatting);
            this.dgvPortfolioFinalValue.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnDgvPortfolioFinalValue_CellMouseDown);
            this.dgvPortfolioFinalValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioFinalValue_CellPainting);
            this.dgvPortfolioFinalValue.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvPortfolioFinalValue_DataBindingComplete);
            this.dgvPortfolioFinalValue.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.OnDgvPortfolioFinalValue_RowsAdded);
            this.dgvPortfolioFinalValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioFinalValue_SelectionChanged);
            this.dgvPortfolioFinalValue.MouseEnter += new System.EventHandler(this.DgvPortfolioFinalValue_MouseEnter);
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
            this.dgvPortfolioFooterFinalValue.Location = new System.Drawing.Point(0, 396);
            this.dgvPortfolioFooterFinalValue.MultiSelect = false;
            this.dgvPortfolioFooterFinalValue.Name = "dgvPortfolioFooterFinalValue";
            this.dgvPortfolioFooterFinalValue.ReadOnly = true;
            this.dgvPortfolioFooterFinalValue.RowHeadersVisible = false;
            this.dgvPortfolioFooterFinalValue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPortfolioFooterFinalValue.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPortfolioFooterFinalValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioFooterFinalValue.Size = new System.Drawing.Size(1204, 74);
            this.dgvPortfolioFooterFinalValue.TabIndex = 10;
            this.dgvPortfolioFooterFinalValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioFooterFinalValue_CellFormatting);
            this.dgvPortfolioFooterFinalValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioFooterFinalValue_CellPainting);
            this.dgvPortfolioFooterFinalValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioFooterFinalValue_SelectionChanged);
            this.dgvPortfolioFooterFinalValue.Resize += new System.EventHandler(this.DgvPortfolioFooterFinalValue_Resize);
            // 
            // tabPgMarketValue
            // 
            this.tabPgMarketValue.Controls.Add(this.dgvPortfolioMarketValue);
            this.tabPgMarketValue.Controls.Add(this.dgvPortfolioFooterMarketValue);
            this.tabPgMarketValue.Location = new System.Drawing.Point(4, 26);
            this.tabPgMarketValue.Name = "tabPgMarketValue";
            this.tabPgMarketValue.Padding = new System.Windows.Forms.Padding(3);
            this.tabPgMarketValue.Size = new System.Drawing.Size(1205, 470);
            this.tabPgMarketValue.TabIndex = 1;
            this.tabPgMarketValue.Text = "_tabPageMarketValues";
            this.tabPgMarketValue.UseVisualStyleBackColor = true;
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
            this.dgvPortfolioMarketValue.Location = new System.Drawing.Point(0, 0);
            this.dgvPortfolioMarketValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPortfolioMarketValue.MultiSelect = false;
            this.dgvPortfolioMarketValue.Name = "dgvPortfolioMarketValue";
            this.dgvPortfolioMarketValue.ReadOnly = true;
            this.dgvPortfolioMarketValue.RowHeadersVisible = false;
            this.dgvPortfolioMarketValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioMarketValue.Size = new System.Drawing.Size(1204, 396);
            this.dgvPortfolioMarketValue.TabIndex = 11;
            this.dgvPortfolioMarketValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioMarketValue_CellFormatting);
            this.dgvPortfolioMarketValue.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnDgvPortfolioMarketValue_CellMouseDown);
            this.dgvPortfolioMarketValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioMarketValue_CellPainting);
            this.dgvPortfolioMarketValue.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvPortfolioMarketValue_DataBindingComplete);
            this.dgvPortfolioMarketValue.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.OnDgvPortfolioMarketValue_RowsAdded);
            this.dgvPortfolioMarketValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioMarketValue_SelectionChanged);
            this.dgvPortfolioMarketValue.MouseEnter += new System.EventHandler(this.DgvPortfolioMarketValue_MouseEnter);
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
            this.dgvPortfolioFooterMarketValue.Location = new System.Drawing.Point(0, 396);
            this.dgvPortfolioFooterMarketValue.MultiSelect = false;
            this.dgvPortfolioFooterMarketValue.Name = "dgvPortfolioFooterMarketValue";
            this.dgvPortfolioFooterMarketValue.ReadOnly = true;
            this.dgvPortfolioFooterMarketValue.RowHeadersVisible = false;
            this.dgvPortfolioFooterMarketValue.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPortfolioFooterMarketValue.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPortfolioFooterMarketValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPortfolioFooterMarketValue.Size = new System.Drawing.Size(1204, 74);
            this.dgvPortfolioFooterMarketValue.TabIndex = 12;
            this.dgvPortfolioFooterMarketValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvPortfolioFooterMarketValue_CellFormatting);
            this.dgvPortfolioFooterMarketValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvPortfolioFooterMarketValue_CellPainting);
            this.dgvPortfolioFooterMarketValue.SelectionChanged += new System.EventHandler(this.DgvPortfolioFooterMarketValue_SelectionChanged);
            this.dgvPortfolioFooterMarketValue.Resize += new System.EventHandler(this.DgvPortfolioFooterMarketValue_Resize);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::SharePortfolioManager.Properties.Resources.button_recycle_bin_24;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(721, 501);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(178, 33);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "&Remove_";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnBtnDelete_Click);
            // 
            // btnClearLogger
            // 
            this.btnClearLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearLogger.Enabled = false;
            this.btnClearLogger.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLogger.Image = ((System.Drawing.Image)(resources.GetObject("btnClearLogger.Image")));
            this.btnClearLogger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearLogger.Location = new System.Drawing.Point(901, 501);
            this.btnClearLogger.Margin = new System.Windows.Forms.Padding(1);
            this.btnClearLogger.Name = "btnClearLogger";
            this.btnClearLogger.Size = new System.Drawing.Size(178, 33);
            this.btnClearLogger.TabIndex = 11;
            this.btnClearLogger.Text = "&Clear_";
            this.btnClearLogger.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearLogger.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearLogger.UseVisualStyleBackColor = true;
            this.btnClearLogger.Visible = false;
            // 
            // grpBoxStatusMessage
            // 
            this.grpBoxStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxStatusMessage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpBoxStatusMessage.Controls.Add(this.tblLayPnlStatusMessages);
            this.grpBoxStatusMessage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxStatusMessage.Location = new System.Drawing.Point(5, 590);
            this.grpBoxStatusMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxStatusMessage.Name = "grpBoxStatusMessage";
            this.grpBoxStatusMessage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxStatusMessage.Size = new System.Drawing.Size(770, 115);
            this.grpBoxStatusMessage.TabIndex = 5;
            this.grpBoxStatusMessage.TabStop = false;
            this.grpBoxStatusMessage.Text = "Status message_";
            // 
            // tblLayPnlStatusMessages
            // 
            this.tblLayPnlStatusMessages.ColumnCount = 1;
            this.tblLayPnlStatusMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlStatusMessages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlStatusMessages.Controls.Add(this.rchTxtBoxStateMessage, 0, 0);
            this.tblLayPnlStatusMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlStatusMessages.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlStatusMessages.Name = "tblLayPnlStatusMessages";
            this.tblLayPnlStatusMessages.RowCount = 1;
            this.tblLayPnlStatusMessages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlStatusMessages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tblLayPnlStatusMessages.Size = new System.Drawing.Size(764, 92);
            this.tblLayPnlStatusMessages.TabIndex = 0;
            // 
            // rchTxtBoxStateMessage
            // 
            this.rchTxtBoxStateMessage.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.rchTxtBoxStateMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rchTxtBoxStateMessage.Location = new System.Drawing.Point(3, 3);
            this.rchTxtBoxStateMessage.Name = "rchTxtBoxStateMessage";
            this.rchTxtBoxStateMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rchTxtBoxStateMessage.ShowSelectionMargin = true;
            this.rchTxtBoxStateMessage.Size = new System.Drawing.Size(758, 86);
            this.rchTxtBoxStateMessage.TabIndex = 0;
            this.rchTxtBoxStateMessage.Text = "";
            // 
            // lblWebParserDailyValuesState
            // 
            this.lblWebParserDailyValuesState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebParserDailyValuesState.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebParserDailyValuesState.Location = new System.Drawing.Point(2, 48);
            this.lblWebParserDailyValuesState.Margin = new System.Windows.Forms.Padding(1);
            this.lblWebParserDailyValuesState.Name = "lblWebParserDailyValuesState";
            this.lblWebParserDailyValuesState.Size = new System.Drawing.Size(439, 21);
            this.lblWebParserDailyValuesState.TabIndex = 1;
            this.lblWebParserDailyValuesState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBarWebParserMarketValues
            // 
            this.progressBarWebParserMarketValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarWebParserMarketValues.Location = new System.Drawing.Point(4, 28);
            this.progressBarWebParserMarketValues.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBarWebParserMarketValues.Name = "progressBarWebParserMarketValues";
            this.progressBarWebParserMarketValues.Size = new System.Drawing.Size(435, 15);
            this.progressBarWebParserMarketValues.TabIndex = 0;
            // 
            // lblWebParserMarketValuesState
            // 
            this.lblWebParserMarketValuesState.BackColor = System.Drawing.Color.Transparent;
            this.lblWebParserMarketValuesState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWebParserMarketValuesState.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebParserMarketValuesState.Location = new System.Drawing.Point(4, 4);
            this.lblWebParserMarketValuesState.Margin = new System.Windows.Forms.Padding(3);
            this.lblWebParserMarketValuesState.Name = "lblWebParserMarketValuesState";
            this.lblWebParserMarketValuesState.Size = new System.Drawing.Size(435, 17);
            this.lblWebParserMarketValuesState.TabIndex = 1;
            this.lblWebParserMarketValuesState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpBoxUpdateState
            // 
            this.grpBoxUpdateState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxUpdateState.Controls.Add(this.tblLayPnlUpdateState);
            this.grpBoxUpdateState.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxUpdateState.Location = new System.Drawing.Point(781, 590);
            this.grpBoxUpdateState.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxUpdateState.Name = "grpBoxUpdateState";
            this.grpBoxUpdateState.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxUpdateState.Size = new System.Drawing.Size(449, 115);
            this.grpBoxUpdateState.TabIndex = 6;
            this.grpBoxUpdateState.TabStop = false;
            this.grpBoxUpdateState.Text = "Update state_";
            // 
            // tblLayPnlUpdateState
            // 
            this.tblLayPnlUpdateState.ColumnCount = 1;
            this.tblLayPnlUpdateState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlUpdateState.Controls.Add(this.progressBarWebParserDailyValues, 0, 3);
            this.tblLayPnlUpdateState.Controls.Add(this.lblWebParserDailyValuesState, 0, 2);
            this.tblLayPnlUpdateState.Controls.Add(this.progressBarWebParserMarketValues, 0, 1);
            this.tblLayPnlUpdateState.Controls.Add(this.lblWebParserMarketValuesState, 0, 0);
            this.tblLayPnlUpdateState.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tblLayPnlUpdateState.Location = new System.Drawing.Point(3, 19);
            this.tblLayPnlUpdateState.MaximumSize = new System.Drawing.Size(443, 92);
            this.tblLayPnlUpdateState.MinimumSize = new System.Drawing.Size(443, 92);
            this.tblLayPnlUpdateState.Name = "tblLayPnlUpdateState";
            this.tblLayPnlUpdateState.Padding = new System.Windows.Forms.Padding(1);
            this.tblLayPnlUpdateState.RowCount = 4;
            this.tblLayPnlUpdateState.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tblLayPnlUpdateState.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tblLayPnlUpdateState.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tblLayPnlUpdateState.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tblLayPnlUpdateState.Size = new System.Drawing.Size(443, 92);
            this.tblLayPnlUpdateState.TabIndex = 2;
            // 
            // progressBarWebParserDailyValues
            // 
            this.progressBarWebParserDailyValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarWebParserDailyValues.Location = new System.Drawing.Point(4, 74);
            this.progressBarWebParserDailyValues.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBarWebParserDailyValues.Name = "progressBarWebParserDailyValues";
            this.progressBarWebParserDailyValues.Size = new System.Drawing.Size(435, 15);
            this.progressBarWebParserDailyValues.TabIndex = 2;
            // 
            // timerStatusMessageClear
            // 
            this.timerStatusMessageClear.Interval = 2000;
            this.timerStatusMessageClear.Tick += new System.EventHandler(this.TimerStatusMessageDelete_Tick);
            // 
            // lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue
            // 
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.BackColor = System.Drawing.Color.LightGray;
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.Location = new System.Drawing.Point(636, 9);
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.Name = "lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue";
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.Size = new System.Drawing.Size(143, 21);
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.TabIndex = 43;
            this.lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timerStartNextShareUpdate
            // 
            this.timerStartNextShareUpdate.Interval = 1000;
            this.timerStartNextShareUpdate.Tick += new System.EventHandler(this.TimerStartNextShareUpdate_Tick);
            // 
            // timerMouseCellDownDoubleClick
            // 
            this.timerMouseCellDownDoubleClick.Interval = 500;
            this.timerMouseCellDownDoubleClick.Tick += new System.EventHandler(this.TimerMouseCellDownDoubleClick_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1235, 708);
            this.Controls.Add(this.grpBoxUpdateState);
            this.Controls.Add(this.grpBoxStatusMessage);
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
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.LocationChanged += new System.EventHandler(this.FrmMain_LocationChanged);
            this.VisibleChanged += new System.EventHandler(this.FrmMain_VisibleChanged);
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpBoxSharePortfolio.ResumeLayout(false);
            this.tblLayPnlShareOverviews.ResumeLayout(false);
            this.tblLayPnlShareOverviews.PerformLayout();
            this.tabCtrlShareOverviews.ResumeLayout(false);
            this.tabPgFinalValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFinalValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterFinalValue)).EndInit();
            this.tabPgMarketValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioMarketValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfolioFooterMarketValue)).EndInit();
            this.grpBoxStatusMessage.ResumeLayout(false);
            this.tblLayPnlStatusMessages.ResumeLayout(false);
            this.grpBoxUpdateState.ResumeLayout(false);
            this.tblLayPnlUpdateState.ResumeLayout(false);
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
        private Button btnRefresh;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnAdd;
        private Button btnRefreshAll;
        private DataGridView dgvPortfolioFinalValue;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem languageToolStripMenuItem;
        private GroupBox grpBoxStatusMessage;
        private ProgressBar progressBarWebParserMarketValues;
        private GroupBox grpBoxUpdateState;
        private Label lblWebParserDailyValuesState;
        private Timer timerStatusMessageClear;
        private DataGridView dgvPortfolioFooterFinalValue;
        private RichTextBox rchTxtBoxStateMessage;
        private Label lblWebParserMarketValuesState;
        private ToolStripMenuItem loggerToolStripMenuItem;
        private Button btnClearLogger;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private TabControl tabCtrlShareOverviews;
        private TabPage tabPgFinalValue;
        private TabPage tabPgMarketValue;
        private DataGridView dgvPortfolioMarketValue;
        private DataGridView dgvPortfolioFooterMarketValue;
        private Label lblShareDetailsWithOutDividendBrokerageSharePriceCurrentValue;
        private TableLayoutPanel tblLayPnlShareOverviews;
        private TableLayoutPanel tblLayPnlStatusMessages;
        private TableLayoutPanel tblLayPnlUpdateState;
        private Timer timerStartNextShareUpdate;
        private ProgressBar progressBarWebParserDailyValues;
        private Timer timerMouseCellDownDoubleClick;
    }
}

