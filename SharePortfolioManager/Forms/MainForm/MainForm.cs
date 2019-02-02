//MIT License
//
//Copyright(c) 2017 nessie1980(nessie1980 @gmx.de)
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

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

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

        /// <summary>
        /// Stores the name of the final value tab control
        /// </summary>
        private readonly string _tabPageDetailsFinalValue = "tabPgDetailsFinalValue";

        /// <summary>
        /// Stores the name of the market value tab control
        /// </summary>
        private readonly string _tabPageDetailsMarketValue = "tabPgDetailsMarketValue";

        /// <summary>
        /// Stores the name of the dividends tab control
        /// </summary>
        private readonly string _tabPageDetailsDividendValue = "tabPgDividends";

        /// <summary>
        /// Stores the name of the brokerage tab control
        /// </summary>
        private readonly string _tabPageDetailsBrokerageValue = "tabPgBrokerage";

        /// <summary>
        /// Stores the name of the profit / loss value tab control
        /// </summary>
        private readonly string _tabPageDetailsProfitLossValue = "tabPgProfitLoss";

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


        private const string WebSitesFileName = @"Settings\WebSites.XML";

        private XmlReaderSettings _readerSettingsPortfolio;
        private XmlDocument _portfolio;
        private XmlReader _readerPortfolio;
        private string _portfolioFileName = @"Portfolios\Portfolio.XML";

        #endregion XML files settings

        #region Flags

        #endregion Flags

        #region Share / share list

        #endregion Share / share list

        #region WebSite configuration

        /// <summary>
        /// Stores the count of the website object tags in the XML
        /// </summary>
        private const short WebSiteTagCount = 4;

        #endregion WebSite configuration

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

        #endregion Form

        #region Logger

        public int LoggerGuiEntriesSize { get; set; } = 25;

        public List<string> LoggerStateList { get; set; } = new List<string>();

        public List<string> LoggerComponentNamesList { get; set; } = new List<string>();

        public List<Color> LoggerConsoleColorList { get; set; } = new List<Color>() { Color.Black, Color.Black, Color.OrangeRed, Color.Red, Color.DarkRed };

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

        public XmlReaderSettings ReaderSettingsWebSites { get; set; }

        public XmlDocument WebSites { get; set; }

        public XmlReader ReaderWebSites { get; set; }

        public XmlReaderSettings ReaderSettingsPortfolio
        {
            get => _readerSettingsPortfolio;
            set => _readerSettingsPortfolio = value;
        }

        public XmlDocument Portfolio
        {
            get => _portfolio;
            set => _portfolio = value;
        }

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
            FileDoesNotExit = -3,
            PortfolioListEmtpy = -2,
            LoadFailed = -1,
            LoadSucessful = 0,

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

        public Parser.Parser Parser { get; } = new Parser.Parser();

        public List<WebSiteRegex> WebSiteRegexList { get; set; } = new List<WebSiteRegex>();

        #endregion Parser

        #region Share objects

        public ShareObjectMarketValue ShareObjectMarketValue { get; internal set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; internal set; }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue { get; internal set; } = new List<ShareObjectMarketValue>();

        public List<ShareObjectFinalValue> ShareObjectListFinalValue { get; internal set; } = new List<ShareObjectFinalValue>();

        #endregion Share objects

        public List<string> RegexSearchFailedList { get; set; } = new List<string>();

        #region Selected indices's

        public int SelectedDataGridViewShareIndex { get; set; }

        public int LastFirstDisplayedRowIndex { get; set; }

        #endregion Selected indices's

        public List<string> EnableDisableControlNames { get; } = new List<string>();

        private TabPage _tempFinalValues;
        private TabPage _tempMarketValues;
        private TabPage _tempProfitLoss;
        private TabPage _tempDividends;
        private TabPage _tempBrokerage;

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

                #region Load language

                LoadLanguage();

                #endregion Load language

                #region Create notify icon

                CreateNotifyIcon();

                #endregion Create notify icon

                #region Logger

                if (InitFlag)
                {
                    // Initialize logger
                    Logger.LoggerInitialize(LoggerStateLevel, LoggerComponentLevel, LoggerStateList, LoggerComponentNamesList, LoggerConsoleColorList, LoggerLogToFileEnabled, LoggerGuiEntriesSize, LoggerPathFileName, null, true);

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

                #region Set language values to the control

                SetLanguage();

                #endregion Set language values to the control

                #region Read shares from XML / Load portfolio

                // Only load portfolio if a portfolio file is set in the Settings.xml
                if (_portfolioFileName != "")
                {
                    // Load portfolio
                    LoadPortfolio();

                    // Check portfolio load state
                    switch (PortfolioLoadState)
                    {
                        case EStatePortfolioLoad.LoadSucessful:
                        {
                            AddSharesToDataGridViews();
                            AddShareFooters();

                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            EnableDisableControlNames.Add("grpBoxSharePortfolio");
                            EnableDisableControlNames.Add("grpBoxShareDetails");
                            EnableDisableControlNames.Add("grpBoxStatusMessage");
                            EnableDisableControlNames.Add("grpBoxUpdateState");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);
                        } break;
                        case EStatePortfolioLoad.PortfolioListEmtpy:
                        {
                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            EnableDisableControlNames.Add("grpBoxSharePortfolio");
                            EnableDisableControlNames.Add("grpBoxShareDetails");
                            EnableDisableControlNames.Add("grpBoxStatusMessage");
                            EnableDisableControlNames.Add("grpBoxUpdateState");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            // Disable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("btnRefreshAll");
                            EnableDisableControlNames.Add("btnRefresh");
                            EnableDisableControlNames.Add("btnEdit");
                            EnableDisableControlNames.Add("btnDelete");
                            Helper.EnableDisableControls(false, tblLayPnlShareOverviews, EnableDisableControlNames);

                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListEmpty",
                                    LanguageName),
                                Language, LanguageName,
                                Color.OrangeRed, Logger, (int) EStateLevels.Warning,
                                (int) EComponentLevels.Application);
                        } break;
                        case EStatePortfolioLoad.LoadFailed:
                        {
                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            // Disable controls
                            saveAsToolStripMenuItem.Enabled = false;

                            _portfolioFileName = @"";
                        } break;
                        case EStatePortfolioLoad.FileDoesNotExit:
                        {
                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            // Disable controls
                            saveAsToolStripMenuItem.Enabled = false;

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists_1", LanguageName)
                            + _portfolioFileName
                            + Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists_2", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                            _portfolioFileName = @"";
                        }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    // Set portfolio filename to the application caption
                    Text = Language.GetLanguageTextByXPath(@"/Application/Name", LanguageName)
                           + @" " + Helper.GetApplicationVersion();
                    if ( _portfolioFileName !=  @"")
                    Text += @" - (" + _portfolioFileName + @")";
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioNotSet", LanguageName),
                        Language, LanguageName,
                        Color.OrangeRed, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);

                    // Disable menustrip menu point "Save as..."
                    saveAsToolStripMenuItem.Enabled = false;

                    EnableDisableControlNames.Clear();
                    EnableDisableControlNames.Add("menuStrip1");

                    // Disable all controls
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);
                }

                #endregion Read shares from XML / Load portfolio

                #region Set tab controls names

                _tabPageDetailsFinalValue = tabCtrlDetails.TabPages[0].Name;
                _tabPageDetailsMarketValue = tabCtrlDetails.TabPages[1].Name;
                _tabPageDetailsProfitLossValue = tabCtrlDetails.TabPages[2].Name;
                _tabPageDetailsDividendValue = tabCtrlDetails.TabPages[3].Name;
                _tabPageDetailsBrokerageValue = tabCtrlDetails.TabPages[4].Name;

                _tempFinalValues = tabCtrlDetails.TabPages[_tabPageDetailsFinalValue];
                _tempMarketValues = tabCtrlDetails.TabPages[_tabPageDetailsMarketValue];
                _tempDividends = tabCtrlDetails.TabPages[_tabPageDetailsDividendValue];
                _tempBrokerage = tabCtrlDetails.TabPages[_tabPageDetailsBrokerageValue];
                _tempProfitLoss = tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue];

                #endregion Set tab controls names

                #region Select first item

                if (dgvPortfolioFinalValue.Rows.Count > 0)
                {
                    dgvPortfolioFinalValue.Rows[0].Selected = true;
                }

                tabCtrlShareOverviews.Select();

                #endregion Select first item
            }
            catch (LoggerException loggerException)
            {
#if DEBUG
                MessageBox.Show(loggerException.Message, @"Error - FrmMain()", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Check the logger initialization state
                switch (Logger.InitState)
                {
                    case Logger.EInitState.NotInitialized:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.WrongSize:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WrongSize", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ComponentLevelInvalid:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentLevelInvalid", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ComponentNamesMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentNamesMaxCount", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.StateLevelInvalid:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StateLevelInvalid", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.StatesMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StatesMaxCount", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ColorsMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ColorsMaxCount", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.WriteStartupFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WriteStartupFailed", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.LogPathCreationFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/LogPathCreationFailed", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.InitializationFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/InitializationFailed", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.Initialized:
                        switch (Logger.LoggerState)
                        {
                            case Logger.ELoggerState.CleanUpLogFilesFailed:
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message, @"Error - FrmMain()", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                var message = $"Error occurred\r\n\r\nMessage: {ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var nodeHeigth = Settings.SelectSingleNode("/Settings/Window/Height");

                if (nodeWidth != null)
                    nodeWidth.InnerXml = NormalWindowSize.Width.ToString();
                if (nodeHeigth != null)
                    nodeHeigth.InnerXml = NormalWindowSize.Height.ToString();

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
#if DEBUG
                var message = $"MainForm_FormClosing()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                Logger.AddEntry(Language.GetLanguageTextByXPath(@"/MainForm/Errors/SaveSettingsFailed",
                    LanguageName), (Logger.ELoggerStateLevels)EStateLevels.FatalError, (Logger.ELoggerComponentLevels)EComponentLevels.Application);
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
            // Reset labels
            lblShareNameWebParser.Text = @"";
            lblWebParserState.Text = @"";

            // Reset progress bar
            progressBarWebParser.Value = 0;

            // Disable time
            timerStatusMessageClear.Enabled = false;
        }

        #endregion Timer

    }
}
