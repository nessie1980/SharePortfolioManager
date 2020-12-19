//MIT License
//
//Copyright(c) 2020 nessie1980(nessie1980 @gmx.de)
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

// Define for DEBUGGING
//#define DEBUG_MAIN_FORM

using LanguageHandler;
using Logging;
using Parser;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Properties;
using SharePortfolioManager.ShareDetailsForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using SharePortfolioManager.ChartForm;

namespace SharePortfolioManager
{
    public partial class FrmMain : Form
    {
        #region Variables

        #region Form

        /// <summary>
        /// Stores the flag if the form is shown or still loading
        /// </summary>
        private bool _formIsShown;

        /// <summary>
        /// Stores the notify icon of the application
        /// </summary>
        private NotifyIcon _notifyIcon;

        /// <summary>
        /// Stores the context menu for the notify icon
        /// </summary>
        private ContextMenuStrip _notifyContextMenuStrip;

        #endregion From

        #region Logger

        /// <summary>
        /// State levels for the logging (e.g. Info)
        /// </summary>
        public enum EStateLevels
        {
            Start = Logger.ELoggerStateLevels.State1,
            Info = Logger.ELoggerStateLevels.State2,
            Warning = Logger.ELoggerStateLevels.State3,
            Error = Logger.ELoggerStateLevels.State4,
            FatalError = Logger.ELoggerStateLevels.State5,
        }

        /// <summary>
        /// Component levels for the logging (e.g Application)
        /// </summary>
        public enum EComponentLevels
        {
            Application = Logger.ELoggerComponentLevels.Component1,
            Parser = Logger.ELoggerComponentLevels.Component2,
            LanguageHandler = Logger.ELoggerComponentLevels.Component3
        }

        /// <summary>
        /// Stores the value which states should be logged (e.g. Info)
        /// </summary>
        private int _loggerStateLevel;

        /// <summary>
        /// Stores the value which component should be logged
        /// </summary>
        private int _loggerComponentLevel;

        /// <summary>
        /// Stores the value how much log files should be stored
        /// </summary>
        private int _loggerStoredLogFiles = 10;

        #endregion Logger

        #region XML files settings

        private const string LanguageFileName = @"Settings\Language.XML";

        private const string SettingsFileName = @"Settings\Settings.XML";

        private XmlReaderSettings _readerSettingsPortfolio;
        private XmlDocument _portfolio;
        private XmlReader _readerPortfolio;

        private string _portfolioFileName = @"Portfolios\Portfolio.XML";

        #endregion XML files settings

        #endregion Variables

        #region Properties

        #region Form

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        public Point NormalWindowPosition { get; set; }

        public Size NormalWindowSize { get; set; }

        public FormWindowState MyWindowState { get; set; }

        public string LanguageName { get; set; } = @"English";

        public int StatusMessageClearTimerValue { get; set; } = 5000;

        public int StartNextShareUpdateTimerValue { get; set; } = 5000;

        #endregion Form

        #region Logger

        public int LoggerGuiEntriesSize { get; set; } = 25;

        public List<string> LoggerStateList { get; set; } = new List<string>();

        public List<string> LoggerComponentNamesList { get; set; } = new List<string>();

        public List<Color> LoggerConsoleColorList { get; set; } = new List<Color>()
            {Color.Black, Color.Black, Color.OrangeRed, Color.Red, Color.DarkRed};

        public int LoggerStateLevel
        {
            get => _loggerStateLevel;
            set
            {
                if (value < 32 && value > -1)
                    _loggerStateLevel = value;
                else
                    _loggerStateLevel = 0;
            }
        }

        public int LoggerComponentLevel
        {
            get => _loggerComponentLevel;
            set
            {
                if (value < 8 && value > -1)
                    _loggerComponentLevel = value;
                else
                    _loggerComponentLevel = 0;
            }
        }

        public int LoggerStoredLogFiles
        {
            get => _loggerStoredLogFiles;
            set => _loggerStoredLogFiles = value > 1 ? value : 10;
        }

        public bool LoggerLogToFileEnabled { get; set; } = false;

        public bool LoggerLogCleanUpAtStartUpEnabled { get; set; } = false;

        public string LoggerPathFileName { get; set; } = Application.StartupPath + @"\Logs\" +
                                                         $"{DateTime.Now.Year}_{DateTime.Now.Month:00}_{DateTime.Now.Day:00}_Log.txt";

        /// <summary>
        /// Stores an instance of the Logger
        /// </summary>
        public Logger Logger { get; } = new Logger();

        #endregion Logger

        #region XML files settings

        public Language Language { get; set; }

        public XmlReaderSettings ReaderSettingsSettings { get; set; }

        public XmlDocument Settings { get; set; }

        public XmlReader ReaderSettings { get; set; }

        // ReSharper disable once ConvertToAutoProperty
        public XmlReaderSettings ReaderSettingsPortfolio
        {
            get => _readerSettingsPortfolio;
            set => _readerSettingsPortfolio = value;
        }

        // ReSharper disable once ConvertToAutoProperty
        public XmlDocument Portfolio
        {
            get => _portfolio;
            set => _portfolio = value;
        }

        // ReSharper disable once ConvertToAutoProperty
        public XmlReader ReaderPortfolio
        {
            get => _readerPortfolio;
            set => _readerPortfolio = value;
        }

        #endregion XML files settings

        #region Portfolio load

        /// <summary>
        /// State of the portfolio load
        /// </summary>
        public enum EStatePortfolioLoad
        {
            FileDoesNotExit = -4,
            PortfolioListEmpty = -3,
            PortfolioXmlError = -2,
            LoadFailed = -1,
            LoadSuccessful = 0,

        }

        public EStatePortfolioLoad PortfolioLoadState;

        #endregion Portofolio load

        #region Flags

        public bool InitFlag { get; set; }

        public bool UpdateAllFlag { get; set; }

        public bool AddFlagMarketValue { get; set; }

        public bool AddFlagFinalValue { get; set; }

        public bool EditFlagMarketValue { get; set; }

        public bool EditFlagFinalValue { get; set; }

        public bool DeleteFlagMarketValue { get; set; }

        public bool DeleteFlagFinalValue { get; set; }

        public bool PortfolioEmptyFlag { get; set; }

        public bool MarketValueOverviewTabSelected { get; set; }

        #endregion Flags

        #region Parser

        // Debugging of the parser for the market value
        public bool ParserMarketValuesDebuggingEnable { internal set; get; }

        // Parser for the market values
        public Parser.Parser ParserMarketValues { internal set; get; }

        // Debugging of the parser for the daily value enabling
        public bool ParserDailyValuesDebuggingEnable { internal set; get; }

        // Parser for the daily values
        public Parser.Parser ParserDailyValues { internal set; get; }

        #endregion Parser

        #region Charting

        public FrmChart FrmChart;

        public ChartingInterval ChartingIntervalValue = ChartingInterval.Week;

        public int ChartingAmount = 1;

        public Dictionary<string, Color> ChartingColorDictionary = new Dictionary<string, Color>();

        #endregion Charting

        #region Share objects

        public ShareObjectMarketValue ShareObjectMarketValue { get; internal set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; internal set; }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue { get; internal set; } =
            new List<ShareObjectMarketValue>();

        public List<ShareObjectFinalValue> ShareObjectListFinalValue { get; internal set; } =
            new List<ShareObjectFinalValue>();

        #endregion Share objects

        public List<InvalidRegexConfigurations> RegexSearchFailedList { get; set; } = new List<InvalidRegexConfigurations>();

        #region Selected indices's

        public int SelectedDataGridViewShareIndex { get; set; }

        public int LastFirstDisplayedRowIndex { get; set; }

        #endregion Selected indices's

        public List<string> EnableDisableControlNames { get; } = new List<string>();

        #endregion Properties

        #region MainFrom

        #region MainForm initialization

        /// <inheritdoc />
        /// <summary>
        /// This is the constructor of the main form
        /// It does all the initialization from the various components
        /// - load settings
        /// - load language
        /// - initialize logger
        /// - initialize Parser
        /// - configure DataGridView´s
        /// - load website configuration
        /// - load share configuration
        /// - load shares to the DataGridView portfolio
        /// - select the first share
        /// - set language to controls
        /// - enable / disable controls
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            try
            {
                // Set main form window to the Helper class.
                // This class use this variable for centering message box windows in center of the main from
                Helper.FrmMain = this;

                #region Set controls names for the "enable / disable" list

                EnableDisableControlNames.Add("menuStrip1");
                EnableDisableControlNames.Add("grpBoxSharePortfolio");
                EnableDisableControlNames.Add("grpBoxShareDetails");
                EnableDisableControlNames.Add("grpBoxStatusMessage");
                EnableDisableControlNames.Add("grpBoxUpdateState");

                // Disable all controls
                Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                #endregion Enable / disable controls names

                #region Load settings

                LoadSettings();

                #endregion Load settings

                #region Logger

                if (InitFlag)
                {
                    // Initialize logger
                    Logger.LoggerInitialize(LoggerStateLevel, LoggerComponentLevel, LoggerStateList,
                        LoggerComponentNamesList, LoggerConsoleColorList, LoggerLogToFileEnabled, LoggerGuiEntriesSize,
                        LoggerPathFileName, null, true);

                    // Check if the logger initialization was not successful
                    if (Logger.InitState != Logger.EInitState.Initialized)
                        InitFlag = false;
                    else
                    {
                        // Check if the startup cleanup of the log files should be done
                        if (LoggerLogCleanUpAtStartUpEnabled)
                        {
                            Logger.CleanUpLogFiles(LoggerStoredLogFiles);
                        }
                    }
                }

                #endregion Logger

                #region Load language

                LoadLanguage();

                #endregion Load language

                #region Create notify icon

                CreateNotifyIcon();

                #endregion Create notify icon

                #region Parser

                InitializeParser();

                #endregion Parser

                #region dgvPortfolio configuration (like row style, header style, font, colors)

                DgvPortfolioConfiguration();

                #endregion dgvPortfolio configuration (like row style, header style, font, colors)

                #region dgvPortfolioFooter configuration (like row style, header style, font, colors)

                DgvPortfolioFooterConfiguration();

                #endregion dgvPortfolioFooter configuration (like row style, header style, font, colors)

                #region Load website RegEx configuration from XML

                WebSiteConfiguration.LoadWebSiteConfigurations(InitFlag);

                #endregion Load website RegEx configuration from XML

                #region Load document RegEx configuration from XML

                DocumentParsingConfiguration.LoadDocumentParsingConfigurations(InitFlag);

                #endregion Load document RegEx configuration from XML

                #region Read shares from XML / Load portfolio

                InitializeBackgroundWorkerLoadPortfolio();

                #endregion Read shares from XML / Load portfolio

                #region Set language values to the control

                SetLanguage();

                #endregion Set language values to the control
            }
            catch (LoggerException loggerException)
            {
                Helper.ShowExceptionMessage(loggerException);

                // Check the logger initialization state
                switch (Logger.InitState)
                {
                    case Logger.EInitState.NotInitialized:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.WrongSize:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WrongSize", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ComponentLevelInvalid:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentLevelInvalid",
                                LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ComponentNamesMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentNamesMaxCount",
                                LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.StateLevelInvalid:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StateLevelInvalid", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.StatesMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StatesMaxCount", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ColorsMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ColorsMaxCount", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.WriteStartupFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WriteStartupFailed", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.LogPathCreationFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/LogPathCreationFailed",
                                LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.InitializationFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/InitializationFailed", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                        break;
                    case Logger.EInitState.Initialized:
                        switch (Logger.LoggerState)
                        {
                            case Logger.ELoggerState.CleanUpLogFilesFailed:
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(
                                        @"/Logger/LoggerStateMessages/CleanUpLogFilesFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                                    (int) EComponentLevels.Application);
                                break;
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);
            }
        }

        #endregion MainForm initialization

        #region MainForm shown

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            Size = NormalWindowSize;
            Location = NormalWindowPosition;
            WindowState = MyWindowState;

            _formIsShown = true;

            #region Read shares from XML / Load portfolio

            // Only load portfolio if a portfolio file is set in the Settings.xml
            if (_portfolioFileName != "")
            {
                // Load portfolio via background worker
                if (!BgwLoadPortfolio.IsBusy)
                {
                    // Show loading portfolio controls
                    tblLayPnlLoadingPortfolio.Visible = true;

                    // Start portfolio load
                    BgwLoadPortfolio.RunWorkerAsync();
                }
            }
            else
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioNotSet", LanguageName),
                    Language, LanguageName,
                    Color.OrangeRed, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);

                // Disable menu strip menu point "Save as..."
                saveAsToolStripMenuItem.Enabled = false;

                EnableDisableControlNames.Clear();
                EnableDisableControlNames.Add("menuStrip1");

                // Disable all controls
                Helper.EnableDisableControls(true, this, EnableDisableControlNames);
            }

            #endregion Read shares from XML / Load portfolio

            #region Select first item

            if (dgvPortfolioFinalValue.Rows.Count > 0)
            {
                dgvPortfolioFinalValue.Rows[0].Selected = true;
            }

            tabCtrlShareOverviews.Select();

            #endregion Select first item
        }

        #endregion MainForm shown

        #region MainForm closing

        /// <summary>
        /// This function saves the settings when the MainForm is closing
        /// </summary>
        /// <param name="sender">MainFrom</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Set window start position and window size
            if (Settings == null) return;

            try
            {
                // Save current window position
                var nodePosX = Settings.SelectSingleNode("/Settings/Window/PosX");
                var nodePosY = Settings.SelectSingleNode("/Settings/Window/PosY");

                if (nodePosX != null)
                    nodePosX.InnerXml = NormalWindowPosition.X.ToString();
                if (nodePosY != null)
                    nodePosY.InnerXml = NormalWindowPosition.Y.ToString();

                // Save current window size
                var nodeWidth = Settings.SelectSingleNode("/Settings/Window/Width");
                var nodeHeight = Settings.SelectSingleNode("/Settings/Window/Height");

                if (nodeWidth != null)
                    nodeWidth.InnerXml = NormalWindowSize.Width.ToString();
                if (nodeHeight != null)
                    nodeHeight.InnerXml = NormalWindowSize.Height.ToString();

                // Save window state
                var nodeWindowState = Settings.SelectSingleNode("/Settings/Window/State");

                if (nodeWindowState != null)
                    nodeWindowState.InnerXml = MyWindowState.ToString();

                // Close reader for saving
                ReaderSettings.Close();
                // Save settings
                Settings.Save(SettingsFileName);
                // Create a new reader to test if the saved values could be loaded
                ReaderSettings = XmlReader.Create(SettingsFileName, ReaderSettingsSettings);
                Settings.Load(ReaderSettings);
            }
            catch (Exception ex)
            {
                Logger.AddEntry(Language.GetLanguageTextByXPath(@"/MainForm/Errors/SaveSettingsFailed",
                        LanguageName), (Logger.ELoggerStateLevels) EStateLevels.FatalError,
                    (Logger.ELoggerComponentLevels) EComponentLevels.Application);

                Helper.ShowExceptionMessage(ex);
            }
        }

        #endregion MainFrom closing

        #region MainForm resize

        // Here we check if the user minimized window, we hide the form
        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (!_formIsShown) return;

            switch (WindowState)
            {
                case FormWindowState.Minimized:
                    ShowInTaskbar = false;
                    Hide();
                    break;
                case FormWindowState.Maximized:
                    MyWindowState = FormWindowState.Maximized;
                    break;
                case FormWindowState.Normal:
                    MyWindowState = FormWindowState.Normal;
                    NormalWindowSize = Size;
                    NormalWindowPosition = Location;
                    break;
                default:
                    MyWindowState = FormWindowState.Maximized;
                    break;
            }
        }

        #endregion MainFrom resize

        #region MainForm location changed

        private void FrmMain_LocationChanged(object sender, EventArgs e)
        {
            if (!_formIsShown) return;

            if (WindowState == FormWindowState.Normal)
                NormalWindowPosition = Location;
        }

        #endregion MainForm location changed

        #region MainFrom visibility changed

        // When the form is hidden, we show notify icon and when the form is visible we hide it
        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            _notifyIcon.Visible = !Visible;
        }

        #endregion MainForm visibility changed

        #region NotifyIcon

        /// <summary>
        /// This function creates the notify icon for the application
        /// </summary>
        private void CreateNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = Resources.SPM
            };

            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // Create sub menu for the notify icon
            CreateNotifyIconContextMenu();
        }

        /// <summary>
        /// This function creates the context menu for the notify icon
        /// </summary>
        private void CreateNotifyIconContextMenu()
        {
            _notifyContextMenuStrip = new ContextMenuStrip();
            _notifyContextMenuStrip.Items.Add(
                Language.GetLanguageTextByXPath(@"/NotifyIcon/Show", LanguageName),
                Resources.show_window_24, Show_Click);
            _notifyContextMenuStrip.Items.Add(
                Language.GetLanguageTextByXPath(@"/NotifyIcon/Exit", LanguageName),
                Resources.button_exit_24, Exit_Click);

            // Set created context menu to the notify icon
            _notifyIcon.ContextMenuStrip = _notifyContextMenuStrip;
        }

        // When click on notify icon, we bring the form to front
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = MyWindowState;

            Activate();
        }

        /// <summary>
        /// This function shows the application via the notify icon if it is minimized
        /// </summary>
        /// <param name="sender">Notify icon</param>
        /// <param name="e">EventArgs</param>
        protected void Show_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = MyWindowState;

            Activate();
        }

        /// <summary>
        /// This function close the application via the notify icon
        /// </summary>
        /// <param name="sender">Notify icon</param>
        /// <param name="e">EventArgs</param>
        protected void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion NotifyIcon

        #endregion Form

        #region Timer

        /// <summary>
        /// This function deletes the status message after the time out
        /// </summary>
        /// <param name="sender">Timer</param>
        /// <param name="e">EventArgs</param>
        private void TimerStatusMessageDelete_Tick(object sender, EventArgs e)
        {
            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

            if (MarketValueOverviewTabSelected)
            {
                dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                // Scroll to the selected row
                Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                    LastFirstDisplayedRowIndex, true);
            }
            else
            {
                dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                // Scroll to the selected row
                Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                    LastFirstDisplayedRowIndex, true);
            }

            btnRefreshAll.Text =
                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
            btnRefresh.Text =
                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
            btnRefreshAll.Image = Resources.button_update_all_24;
            btnRefresh.Image = Resources.button_update_24;

            // Reset labels
            lblWebParserMarketValuesState.Text = @"";
            lblWebParserDailyValuesState.Text = @"";

            // Reset progress bar
            progressBarWebParserMarketValues.Value = 0;
            progressBarWebParserDailyValues.Value = 0;

            // Disable time
            timerStatusMessageClear.Enabled = false;
        }

        /// <summary>
        /// This function starts the next share update if all shares should be updated
        /// </summary>
        /// <param name="sender">Timer</param>
        /// <param name="e">EventArgs</param>
        private void TimerStartNextShareUpdate_Tick(object sender, EventArgs e)
        {
            timerStartNextShareUpdate.Enabled = false;

            // Check if another share object should be updated
            if (SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1)
            {
                // Increase index to get the next share
                SelectedDataGridViewShareIndex++;

                // Check which share overview is selected
                if (MarketValueOverviewTabSelected)
                {
                    do
                    {
                        // Check if the current share should not be updated so check the next share
                        if (ShareObjectListMarketValue[SelectedDataGridViewShareIndex] != null &&
                            ShareObjectListMarketValue[SelectedDataGridViewShareIndex].InternetUpdateOption == ShareObject.ShareUpdateTypes.None &&
                            SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1)
                            // Increase index to get the next share
                            SelectedDataGridViewShareIndex++;

                    } while (ShareObjectListMarketValue[SelectedDataGridViewShareIndex] != null &&
                             ShareObjectListMarketValue[SelectedDataGridViewShareIndex].InternetUpdateOption == ShareObject.ShareUpdateTypes.None &&
                             SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1);

                    // Check if the share should be update
                    if (ShareObjectListMarketValue[SelectedDataGridViewShareIndex] != null &&
                        ShareObjectListMarketValue[SelectedDataGridViewShareIndex].WebSiteConfigurationFound)
                    {
                        // Select the new share update
                        dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                            LastFirstDisplayedRowIndex);

                        if (ShareObjectListMarketValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.Both ||
                            ShareObjectListMarketValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.MarketPrice)
                        {
                            // Start the asynchronous operation of the Parser for the market values
                            ParserMarketValues.ParsingValues = new ParsingValues(
                                new Uri(ShareObjectListMarketValue[SelectedDataGridViewShareIndex].UpdateWebSiteUrl),
                                ShareObjectListMarketValue[SelectedDataGridViewShareIndex].WebSiteEncodingType,
                                ShareObjectListMarketValue[SelectedDataGridViewShareIndex].RegexList
                            );
                            ParserMarketValues.StartParsing();
                        }

                        if (ShareObjectListMarketValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.Both ||
                            ShareObjectListMarketValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.DailyValues)
                        {
                            // Start the asynchronous operation of the Parser for the daily market values
                            ParserDailyValues.ParsingValues = new ParsingValues(
                                new Uri(Helper.BuildDailyValuesUrl(
                                    ShareObjectListMarketValue[SelectedDataGridViewShareIndex].DailyValuesList.Entries,
                                    ShareObjectListMarketValue[SelectedDataGridViewShareIndex]
                                        .DailyValuesUpdateWebSiteUrl,
                                    ShareObjectListMarketValue[SelectedDataGridViewShareIndex].ShareType
                                )),
                                ShareObjectListMarketValue[SelectedDataGridViewShareIndex].WebSiteEncodingType
                            );
                            ParserDailyValues.StartParsing();
                        }
                    }
                }
                else
                {
                    do
                    {

                        // Check if the current share should not be updated so check the next share
                        if (ShareObjectListFinalValue[SelectedDataGridViewShareIndex] != null &&
                            ShareObjectListFinalValue[SelectedDataGridViewShareIndex].InternetUpdateOption == ShareObject.ShareUpdateTypes.None &&
                            SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1)
                            // Increase index to get the next share
                            SelectedDataGridViewShareIndex++;

                    } while (ShareObjectListFinalValue[SelectedDataGridViewShareIndex] != null &&
                             ShareObjectListFinalValue[SelectedDataGridViewShareIndex].InternetUpdateOption == ShareObject.ShareUpdateTypes.None &&
                             SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1);

                    // Select the new share update
                    if (ShareObjectListFinalValue[SelectedDataGridViewShareIndex].WebSiteConfigurationFound
                    )
                    {
                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                        // Scroll to the selected rowShareObjectListFinalValue[SelectedDataGridViewShareIndex].Update &&
                        Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                            SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

                        if (ShareObjectListFinalValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.Both ||
                            ShareObjectListFinalValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.MarketPrice)
                        {
                            // Start the asynchronous operation of the Parser for the market values
                            ParserMarketValues.ParsingValues = new ParsingValues(
                                new Uri(ShareObjectListFinalValue[SelectedDataGridViewShareIndex].UpdateWebSiteUrl),
                                ShareObjectListFinalValue[SelectedDataGridViewShareIndex].WebSiteEncodingType,
                                ShareObjectListFinalValue[SelectedDataGridViewShareIndex].RegexList
                            );
                            ParserMarketValues.StartParsing();
                        }

                        if (ShareObjectListFinalValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.Both ||
                            ShareObjectListFinalValue[SelectedDataGridViewShareIndex].InternetUpdateOption ==
                            ShareObject.ShareUpdateTypes.DailyValues)
                        {
                            // Start the asynchronous operation of the Parser for the daily market values
                            ParserDailyValues.ParsingValues = new ParsingValues(
                                new Uri(Helper.BuildDailyValuesUrl(
                                    ShareObjectListFinalValue[SelectedDataGridViewShareIndex].DailyValuesList.Entries,
                                    ShareObjectListFinalValue[SelectedDataGridViewShareIndex]
                                        .DailyValuesUpdateWebSiteUrl,
                                    ShareObjectListFinalValue[SelectedDataGridViewShareIndex].ShareType
                                )),
                                ShareObjectListFinalValue[SelectedDataGridViewShareIndex].WebSiteEncodingType
                            );
                            ParserDailyValues.StartParsing();
                        }
                    }
                    else
                        UpdateAllFlag = false;
                }
            }

            // Check if a error occurred or the process has been finished
            if (SelectedDataGridViewShareIndex >= ShareObject.ObjectCounter - 1 && UpdateAllFlag == false)
            {
                Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                if (MarketValueOverviewTabSelected)
                {
                    dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex, true);
                }
                else
                {
                    dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex, true);
                }

                btnRefreshAll.Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                btnRefresh.Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                btnRefreshAll.Image = Resources.button_update_all_24;
                btnRefresh.Image = Resources.button_update_24;

                // Reset labels
                lblWebParserMarketValuesState.Text = @"";
                lblWebParserDailyValuesState.Text = @"";

                // Reset progress bar
                progressBarWebParserMarketValues.Value = 0;
                progressBarWebParserDailyValues.Value = 0;
            }
        }

        #endregion Timer
    }
}
