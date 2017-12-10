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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using SharePortfolioManager.Properties;

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
        /// Stores the position of the application window in the "Normal" window state
        /// </summary>
        private Point _normalWindowPosition;

        /// <summary>
        /// Stores the size of the application window in the "Normal" window state
        /// </summary>
        private Size _normalWindowSize;

        /// <summary>
        /// Stores the state of the application window but not the minimized window state
        /// </summary>
        private FormWindowState _myWindowState;

        /// <summary>
        /// Stores the notify icon of the application
        /// </summary>
        private NotifyIcon _notifyIcon;

        /// <summary>
        /// Stores the context menu for the notify icon
        /// </summary>
        private ContextMenuStrip _notifyContextMenuStríp;

        /// <summary>
        /// Stores the loaded language setting
        /// </summary>
        private string _languageName = @"English";

        /// <summary>
        /// Stores the time for showing a status message.
        /// </summary>
        private int _statusMessageClearTimerValue = 5000;

        /// <summary>
        /// Stores the name of the final value tab control
        /// </summary>
        private readonly string TabPageDetailsFinalValue = "tabPgDetailsFinalValue";

        /// <summary>
        /// Stores the name of the market value tab control
        /// </summary>
        private readonly string TabPageDetailsMarketValue = "tabPgDetailsMarketValue";

        /// <summary>
        /// Stores the name of the dividends tab control
        /// </summary>
        private readonly string TabPageDetailsDividendValue = "tabPgDividends";

        /// <summary>
        /// Stores the name of the costs tab control
        /// </summary>
        private readonly string TabPageDetailsCostsValue = "tabPgCosts";

        /// <summary>
        /// Stores the name of the profit / loss value tab control
        /// </summary>
        private readonly string TabPageDetailsProfitLossValue = "tabPgProfitLoss";

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
            WebParser = Logger.ELoggerComponentLevels.Component2,
            LanguageHandler = Logger.ELoggerComponentLevels.Component3
        }

        /// <summary>
        /// Stores the size of the log entry list (default 25)
        /// </summary>
        private int _loggerEntrySize = 25;

        /// <summary>
        /// State list with the names of the various state levels for the logger (e.g. Info)
        /// </summary>
        private List<string> _loggerStatelList = new List<string>();

        /// <summary>
        /// Component name list with the various component names for the logger (e.g. Application) 
        /// </summary>
        private List<string> _loggerComponentNamesList = new List<string>();

        /// <summary>
        /// Color list for the display in the status message GroupBox
        /// </summary>
        private List<Color> _loggerConsoleColorList = new List<Color>() { Color.Black, Color.Black, Color.OrangeRed, Color.Red, Color.DarkRed };

        /// <summary>
        /// Stores the value which states should be logged (e.g. Info)
        /// </summary>
        private int _loggerStateLevel = 0;

        /// <summary>
        /// Stores the value which component should be logged
        /// </summary>
        private int _loggerComponentLevel = 0;

        /// <summary>
        /// Stores the value how much log files should be stored
        /// </summary>
        private int _loggerStoredLogFiles = 10;

        /// <summary>
        /// Stores the value if the logger should log to a file
        /// </summary>
        private bool _loggerLogToFileEnabled = false;

        /// <summary>
        /// Stores the value if the logger should cleanup the log file at application startup
        /// </summary>
        private bool _loggerLogCleanUpAtStartUpEnabled = false;

        /// <summary>
        /// Stores the logger file name with the path
        /// </summary>
        private string _loggerPathFileName = Application.StartupPath + @"\Logs\" + string.Format("{0}_{1:00}_{2:00}_Log.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        /// <summary>
        /// Stores an instance of the Logger
        /// </summary>
        private readonly Logger _logger = new Logger();

        #endregion Logger

        #region XML files settings

        private Language _language;
        private const string LanguageFileName = @"Settings\Language.XML";

        private XmlReaderSettings _readerSettingsSettings;
        private XmlDocument _settings;
        private XmlReader _readerSettings;
        private const string SettingsFileName = @"Settings\Settings.XML";


        private XmlReaderSettings _readerSettingsWebSites;
        private XmlDocument _webSites;
        private XmlReader _readerWebSites;
        private const string WebSitesFileName = @"Settings\WebSites.XML";

        private XmlReaderSettings _readerSettingsPortfolio;
        private XmlDocument _portfolio;
        private XmlReader _readerPortfolio;
        private string PortfolioFileName = @"Portfolios\Portfolio.XML";

        #endregion XML files settings

        #region Flags

        /// <summary>
        /// Stores the state of the application initialization
        /// </summary>
        private bool _bInitFlag;

        /// <summary>
        /// Stores the flag if all shares should be updated
        /// </summary>
        private bool _bUpdateAll;

        /// <summary>
        /// Stores if a new share has been added and if the DataGridView portfolio must be refreshed
        /// </summary>
        private bool _bAddFlagMarketValue;
        private bool _bAddFlagFinalValue;

        /// <summary>
        /// Stores if a share has been edit and if the DataGridView portfolio must be refreshed
        /// </summary>
        private bool _bEditFlagMarketValue;
        private bool _bEditFlagFinalValue;

        /// <summary>
        /// Stores if a share has been deleted and if the DataGridView portfolio must be refreshed
        /// </summary>
        private bool _bDeleteFlagMarketValue;
        private bool _bDeleteFlagFinalValue;

        /// <summary>
        /// Stores if a portfolio file has no shares listed
        /// </summary>
        private bool _bPortfolioEmpty;

        /// <summary>
        /// Stores if the market values or the final values are selected (tab control share overview)
        /// </summary>
        private bool _bMarketValueOverviewTabSelected;

        #endregion Flags

        #region WebParser

        /// <summary>
        /// Stores an instance of the WebParser
        /// </summary>
        private readonly WebParser.WebParser _webParser = new WebParser.WebParser();

        /// <summary>
        /// Stores the list of the website RegEx
        /// </summary>
        private List<WebSiteRegex> _webSiteRegexList = new List<WebSiteRegex>();

        #endregion WebParser

        #region Share / share list

        /// <summary>
        /// Stores a single share object with the market value
        /// for e.g. the selected share in the data grid view or for saving and so on
        /// </summary>
        private ShareObjectMarketValue _shareObjectMarketValue = null;

        /// <summary>
        /// Stores a single share object with the final value
        /// for e.g. the selected share in the data grid view or for saving and so on
        /// </summary>
        private ShareObjectFinalValue _shareObjectFinalValue = null;

        /// <summary>
        /// Stores the share objects (market value) of the XML
        /// </summary>
        private List<ShareObjectMarketValue> _shareObjectsListMarketValue = new List<ShareObjectMarketValue>();

        /// <summary>
        /// Stores the share objects (final value) of the XML
        /// </summary>
        private List<ShareObjectFinalValue> _shareObjectsListFinalValue = new List<ShareObjectFinalValue>();

        /// <summary>
        /// Stores the WKN of the shares with an invalid website configuration
        /// </summary>
        private List<string> _regexSearchFailedList = new List<string>();

        /// <summary>
        /// Stores the index of the currently updating share object when all shares will be updated.
        /// </summary>
        private int _selectedDataGridViewShareIndex;

        /// <summary>
        /// Stores the index of the last displayed row index in the data grid view with the portfolio;
        /// </summary>
        private int _lastFirstDisplayedRowIndex;

        #endregion Share / share list

        #region WebSite configuration

        /// <summary>
        /// Stores the count of the website object tags in the XML
        /// </summary>
        private const short WebSiteTagCount = 4;

        #endregion WebSite configuration

        #region Control names list

        /// <summary>
        /// Stores the names of the controls which should be enabled or disabled
        /// </summary>
        private readonly List<string> _enableDisableControlNames = new List<string>();

        #endregion Control names list

        #endregion Variables

        #region Properties

        #region Form

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public Point NormalWindowPosition
        {
            get { return _normalWindowPosition; }
            set { _normalWindowPosition = value; }
        }

        public Size NormalWindowSize
        {
            get { return _normalWindowSize; }
            set { _normalWindowSize = value; }
        }

        public FormWindowState MyWindowState
        {
            get { return _myWindowState; }
            set { _myWindowState = value; }
        }

        public string LanguageName
        {
            get { return _languageName; }
            set { _languageName = value; }
        }

        public int StatusMessageClearTimerValue
        {
            get { return _statusMessageClearTimerValue; }
            set { _statusMessageClearTimerValue = value; }
        }

        #endregion Form

        #region Logger

        public int LoggerGUIEntriesSize
        {
            get { return _loggerEntrySize; }
            set { _loggerEntrySize = value; }
        }

        public List<string> LoggerStatelList
        {
            get { return _loggerStatelList; }
            set { _loggerStatelList = value; }
        }

        public List<string> LoggerComponentNamesList
        {
            get { return _loggerComponentNamesList; }
            set { _loggerComponentNamesList = value; }
        }

        public List<Color> LoggerConsoleColorList
        {
            get { return _loggerConsoleColorList; }
            set { _loggerConsoleColorList = value; }
        }

        public int LoggerStateLevel
        {
            get { return _loggerStateLevel; }
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
            get { return _loggerComponentLevel; }
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
            get { return _loggerStoredLogFiles; }
            set
            {
                if (value > 1)
                    _loggerStoredLogFiles = value;
                else
                    _loggerStoredLogFiles = 10;
            }

        }

        public bool LoggerLogToFileEnabled
        {
            get { return _loggerLogToFileEnabled; }
            set { _loggerLogToFileEnabled = value; }
        }

        public bool LoggerLogCleanUpAtStartUpEnabled
        {
            get { return _loggerLogCleanUpAtStartUpEnabled; }
            set { _loggerLogCleanUpAtStartUpEnabled = value; }
        }

        public string LoggerPathFileName
        {
            get { return _loggerPathFileName; }
            set { _loggerPathFileName = value; }
        }

        /// <summary>
        /// Stores an instance of the Logger
        /// </summary>
        public Logger Logger
        {
            get { return _logger; }
        }

        #endregion Logger

        #region XML files settings

        public Language Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public XmlReaderSettings ReaderSettingsSettings
        {
            get { return _readerSettingsSettings; }
            set { _readerSettingsSettings = value; }
        }

        public XmlDocument Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public XmlReader ReaderSettings
        {
            get { return _readerSettings; }
            set { _readerSettings = value; }
        }

        public XmlReaderSettings ReaderSettingsWebSites
        {
            get { return _readerSettingsWebSites; }
            set { _readerSettingsWebSites = value; }
        }

        public XmlDocument WebSites
        {
            get { return _webSites; }
            set { _webSites = value; }
        }

        public XmlReader ReaderWebSites
        {
            get { return _readerWebSites; }
            set { _readerWebSites = value; }
        }

        public XmlReaderSettings ReaderSettingsPortfolio
        {
            get { return _readerSettingsPortfolio; }
            set { _readerSettingsPortfolio = value; }
        }

        public XmlDocument Portfolio
        {
            get { return _portfolio; }
            set { _portfolio = value; }
        }

        public XmlReader ReaderPortfolio
        {
            get { return _readerPortfolio; }
            set { _readerPortfolio = value; }
        }

        #endregion XML files settings

        #region Flags

        public bool InitFlag
        {
            get { return _bInitFlag; }
            set { _bInitFlag = value; }
        }

        public bool UpdateAllFlag
        {
            get { return _bUpdateAll; }
            set { _bUpdateAll = value; }
        }

        public bool AddFlagMarketValue
        {
            get { return _bAddFlagMarketValue; }
            set { _bAddFlagMarketValue = value; }
        }

        public bool AddFlagFinalValue
        {
            get { return _bAddFlagFinalValue; }
            set { _bAddFlagFinalValue = value; }
        }

        public bool EditFlagMarketValue
        {
            get { return _bEditFlagMarketValue; }
            set { _bEditFlagMarketValue = value; }
        }

        public bool EditFlagFinalValue
        {
            get { return _bEditFlagFinalValue; }
            set { _bEditFlagFinalValue = value; }
        }

        public bool DeleteFlagMarketValue
        {
            get { return _bDeleteFlagMarketValue; }
            set { _bDeleteFlagMarketValue = value; }
        }

        public bool DeleteFlagFinalValue
        {
            get { return _bDeleteFlagFinalValue; }
            set { _bDeleteFlagFinalValue = value; }
        }

        public bool PortfolioEmptyFlag
        {
            get { return _bPortfolioEmpty; }
            set { _bPortfolioEmpty = value; }
        }

        public bool MarketValueOverviewTabSelected
        {
            get { return _bMarketValueOverviewTabSelected; }
            set { _bMarketValueOverviewTabSelected = value; }
        }

        #endregion Flags

        #region WebParser

        public WebParser.WebParser WebParser
        {
            get { return _webParser; }
        }

        public List<WebSiteRegex> WebSiteRegexList
        {
            get { return _webSiteRegexList; }
            set { _webSiteRegexList = value; }
        }

        #endregion WebParser

        #region Share objects

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get { return _shareObjectMarketValue; }
            internal set { _shareObjectMarketValue = value; }
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get { return _shareObjectFinalValue; }
            internal set { _shareObjectFinalValue = value; }
        }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue
        {
            get { return _shareObjectsListMarketValue; }
            internal set { _shareObjectsListMarketValue = value; }
        }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue
        {
            get { return _shareObjectsListFinalValue; }
            internal set { _shareObjectsListFinalValue = value; }
        }

        #endregion Share objects

        public List<string> RegexSearchFailedList
        {
            get { return _regexSearchFailedList; }
            set { _regexSearchFailedList = value; }
        }

        #region Selected indices's

        public int SelectedDataGridViewShareIndex
        {
            get { return _selectedDataGridViewShareIndex; }
            set { _selectedDataGridViewShareIndex = value; }
        }

        public int LastFirstDisplayedRowIndex
        {
            get { return _lastFirstDisplayedRowIndex; }
            set { _lastFirstDisplayedRowIndex = value; }
        }

        #endregion Selected indices's

        public List<string> EnableDisableControlNames
        {
            get { return _enableDisableControlNames; }
        }

        TabPage tempFinalValues = null;
        TabPage tempMarketValues = null;
        TabPage tempProfitLoss = null;
        TabPage tempDividends = null;
        TabPage tempCosts = null;

        #endregion Properties

        #region MainFrom

        #region MainForm initialization

        /// <summary>
        /// This is the constructor of the main form
        /// It does all the initialization from the various components
        /// - load settings
        /// - load language
        /// - initialize logger
        /// - initialize WebParser
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
                EnableDisableControlNames.Add("btnRefreshAll");
                EnableDisableControlNames.Add("btnRefresh");
                EnableDisableControlNames.Add("btnAdd");
                EnableDisableControlNames.Add("btnEdit");
                EnableDisableControlNames.Add("btnDelete");
                EnableDisableControlNames.Add("btnClearLogger");
                EnableDisableControlNames.Add("dataGridViewSharePortfolio");
                EnableDisableControlNames.Add("dataGridViewSharePortfolioFooter");
                EnableDisableControlNames.Add("tabCtrlDetails");

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
                    Logger.LoggerInitialize(LoggerStateLevel, LoggerComponentLevel, LoggerStatelList, LoggerComponentNamesList, LoggerConsoleColorList, LoggerLogToFileEnabled, LoggerGUIEntriesSize, LoggerPathFileName, null, true);

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

                #region WebParser

                InitializeWebParser();

                #endregion WebParser

                #region dgvPortfolio configuration (like row style, header style, font, colors)

                DgvPortfolioConfiguration();

                #endregion dgvPortfolio configuration (like row style, header style, font, colors)

                #region dgvPortfolioFooter configuration (like row style, header style, font, colors)

                DgvPortfolioFooterConfiguration();

                #endregion dgvPortfolioFooter configuration (like row style, header style, font, colors)

                #region Load website RegEx configuration from XML

                LoadWebSiteConfigurations();

                #endregion Load website RegEx configuration from XML

                #region Set language values to the control

                SetLanguage();

                #endregion Set language values to the control

                #region Read shares from XML

                Text = Language.GetLanguageTextByXPath(@"/Application/Name", LanguageName)
                       + @" " + Helper.GetApplicationVersion();

                // Only load portfolio if a portfolio is set in the settings
                if (PortfolioFileName != "")
                {
                    Text += @" - (" + PortfolioFileName + @")";

                    LoadPortfolio();
                }

                #endregion Read shares from XML

                #region Add items to DataGridView portfolio and set DataGridView portfolio footer

                AddSharesToDataGridViews();

                AddShareFooters();

                #endregion Add items to DataGridView portfolio and set DataGridView portfolio footer

                #region Message if portfolio is empty

                if (PortfolioEmptyFlag && InitFlag)
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListEmpty", LanguageName),
                        Language, LanguageName,
                        Color.OrangeRed, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                }

                #endregion Message if portfolio is empty

                #region Enable / disable controls

                // Enable controls if the initialization was correct and a portfolio is set
                if (ShareObjectListMarketValue.Count != 0 && ShareObjectListFinalValue.Count != 0 && PortfolioFileName != "")
                {
                    // Enable controls
                    EnableDisableControlNames.Add(@"btnRefreshAll");
                    EnableDisableControlNames.Add(@"btnRefresh");
                    EnableDisableControlNames.Add(@"btnEdit");
                    EnableDisableControlNames.Add(@"btnDelete");
                    EnableDisableControlNames.Add(@"btnClearLogger");
                    EnableDisableControlNames.Add(@"dgvPortfolio");
                    EnableDisableControlNames.Add(@"dgvPortfolioFooter");
                    EnableDisableControlNames.Add(@"tabCtrlDetails");

                    // Enable controls
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);
                }

                // Enable controls if the initialization was correct and a portfolio is set
                //if (InitFlag && PortfolioFileName == "")
                //{
                    // Enable controls
                    EnableDisableControlNames.Clear();
                    EnableDisableControlNames.Add(@"menuStrip1");

                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);
                //}

                #endregion Enable / disable controls

                #region Set tab controls names

                TabPageDetailsFinalValue = tabCtrlDetails.TabPages[0].Name;
                TabPageDetailsMarketValue = tabCtrlDetails.TabPages[1].Name;
                TabPageDetailsProfitLossValue = tabCtrlDetails.TabPages[2].Name;
                TabPageDetailsDividendValue = tabCtrlDetails.TabPages[3].Name;
                TabPageDetailsCostsValue = tabCtrlDetails.TabPages[4].Name;

                tempFinalValues = tabCtrlDetails.TabPages[TabPageDetailsFinalValue];
                tempMarketValues = tabCtrlDetails.TabPages[TabPageDetailsMarketValue];
                tempDividends = tabCtrlDetails.TabPages[TabPageDetailsDividendValue];
                tempCosts = tabCtrlDetails.TabPages[TabPageDetailsCostsValue];
                tempProfitLoss = tabCtrlDetails.TabPages[TabPageDetailsProfitLossValue];

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
                MessageBox.Show(string.Format("Error occurred\r\n\r\nMessage: {0}", ex.Message), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion MainForm initialization

        #region MainForm shown

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            _formIsShown = true;

            Size = NormalWindowSize;
            Location = NormalWindowPosition;
            WindowState = MyWindowState;
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
            if (Settings != null)
            {
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
                    MessageBox.Show("MainForm_FormClosing()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    Logger.AddEntry(Language.GetLanguageTextByXPath(@"/MainForm/Errors/SaveSettingsFailed",
                    LanguageName), (Logger.ELoggerStateLevels)EStateLevels.FatalError, (Logger.ELoggerComponentLevels)EComponentLevels.Application);
                }
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
            this._notifyIcon.Visible = !this.Visible;
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
            _notifyContextMenuStríp = new ContextMenuStrip();
            _notifyContextMenuStríp.Items.Add(
                Language.GetLanguageTextByXPath(@"/NotifyIcon/Show", LanguageName),
                Resources.black_show, Show_Click);
            _notifyContextMenuStríp.Items.Add(
                Language.GetLanguageTextByXPath(@"/NotifyIcon/Exit", LanguageName),
                Resources.black_exit, Exit_Click);

            //_notifyContextMenuStríp.MenuItems.Add(0,
            //    new MenuItem(Language.GetLanguageTextByXPath(@"/NotifyIcon/Show",
            //        LanguageName), Show_Click));
            //_notifyContextMenuStríp.MenuItems.Add(1,
            //    new MenuItem(Language.GetLanguageTextByXPath(@"/NotifyIcon/Exit",
            //        LanguageName), (Exit_Click)));

            // Set created context menu to the notify icon
            _notifyIcon.ContextMenuStrip = _notifyContextMenuStríp;
        }

        // When click on notify icon, we bring the form to front
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = MyWindowState;

            this.Activate();
        }

        /// <summary>
        /// This function shows the application via the notify icon if it is minimized
        /// </summary>
        /// <param name="sender">Notify icon</param>
        /// <param name="e">EventArgs</param>
        protected void Show_Click(object sender, System.EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = MyWindowState;

            this.Activate();
        }

        /// <summary>
        /// This function close the application via the notify icon
        /// </summary>
        /// <param name="sender">Notify icon</param>
        /// <param name="e">EventArgs</param>
        protected void Exit_Click(object sender, System.EventArgs e)
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
