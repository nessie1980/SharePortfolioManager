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

namespace SharePortfolioManager
{
    public partial class FrmMain : Form
    {
        #region Variables

        #region Form

        /// <summary>
        /// Stores the position of the application window (start position)
        /// </summary>
        private Point _windowPosition;

        /// <summary>
        /// Stores the size of the application window
        /// </summary>
        private Size _windowSize;

        /// <summary>
        /// Stores the loaded language setting
        /// </summary>
        private string _languageName = @"English";

        /// <summary>
        /// Stores the time for showing a status message.
        /// </summary>
        private int _statusMessageClearTimerValue = 5000;

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
        private string _loggerPathFileName = Application.StartupPath + @"\Logs\" + string.Format("{0}_{1:00}_{2:00}_Log.txt", DateTime.Now.Year, DateTime.Now.Month ,DateTime.Now.Day);

        /// <summary>
        /// Stores an instance of the Logger
        /// </summary>
        private readonly Logger _logger = new Logger();

        #endregion Logger

        #region XML files settings

        private Language _xmlLanguage;
        private const string LanguageFileName = @"Settings\Language.XML";

        private XmlReaderSettings _xmlReaderSettingsSettings;
        private XmlDocument _xmlSettings;
        private XmlReader _xmlReaderSettings;
        private const string SettingsFileName = @"Settings\Settings.XML";


        private XmlReaderSettings _xmlReaderSettingsWebSites;
        private XmlDocument _xmlWebSites;
        private XmlReader _xmlReaderWebSites;
        private const string WebSitesFileName = @"Settings\WebSites.XML";

        private XmlReaderSettings _xmlReaderSettingsPortfolio;
        private XmlDocument _xmlPortfolio;
        private XmlReader _xmlReaderPortfolio;
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
        private bool _bAddFlag;

        /// <summary>
        /// Stores if a share has been edit and if the DataGridView portfolio must be refreshed
        /// </summary>
        private bool _bEditFlag;

        /// <summary>
        /// Stores if a share has been deleted and if the DataGridView portfolio must be refreshed
        /// </summary>
        private bool _bDeleteFlag;

        /// <summary>
        /// Stores if a portfolio file has no shares listed
        /// </summary>
        private bool _bPortfolioEmpty;

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
        /// Stores the share objects of the XML
        /// </summary>
        private List<ShareObject> _shareObjectsList = new List<ShareObject>();

        /// <summary>
        /// Stores the WKN of the shares with an invalid website configuration
        /// </summary>
        private List<string> _regexSearchFailedList = new List<string>(); 

        /// <summary>
        /// Stores the share object of the current selected share in the DataGridView portfolio
        /// </summary>
        private ShareObject _shareObject = null;

        /// <summary>
        /// Stores the index of the currently updating share when all shares will be updated.
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// Stores the index of the last displayed row index in the DataGridView portfolio;
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
        private readonly List<string> _enableDisableControlNames;

        #endregion Control names list

        #endregion Variables

        #region Properties

        public int LoggerGUIEntriesSize
        {
            get { return _loggerEntrySize; }
            set { _loggerEntrySize = value; }
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

        public Language XmlLanguage
        {
            get { return _xmlLanguage; }
        }

        public XmlDocument XmlSettings
        {
            get { return _xmlSettings; }    
        }

        public List<ShareObject> ShareObjectList
        {
            get { return _shareObjectsList; }
            internal set { _shareObjectsList = value; }
        }

        public ShareObject ShareObject
        {
            get { return _shareObject; }           
            internal  set { _shareObject = value; }
        }

        public List<Image> ImageList
        {
            get { return _imageList; }
        }

        public List<WebSiteRegex> WebSiteRegexList
        {
            get { return _webSiteRegexList; }
            set { _webSiteRegexList = value; }
        }

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

                _enableDisableControlNames = new List<string>()
                {
                    "menuStrip1",
                    "btnRefreshAll",
                    "btnRefresh",
                    "btnAdd",
                    "btnEdit",
                    "btnDelete",
                    "btnClearLogger",
                    "dataGridViewSharePortfolio",
                    "dataGridViewSharePortfolioFooter",
                    "tabCtrlDetails"
                };

                // Disable all controls
                Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                #endregion Enable / disable controls names

                #region Load settings

                LoadSettings();

                #endregion Load settings

                #region Load language

                LoadLanguage();

                #endregion Load language

                #region Logger

                if (_bInitFlag)
                {
                    // Initialize logger
                    _logger.LoggerInitialize(_loggerStateLevel, _loggerComponentLevel, _loggerStatelList, _loggerComponentNamesList, _loggerConsoleColorList, _loggerLogToFileEnabled, _loggerEntrySize, _loggerPathFileName, null, true);

                    // Check if the logger initialization was not successful
                    if (_logger.InitState != Logger.EInitState.Initialized)
                        _bInitFlag = false;
                    else
                    {
                        // Check if the startup cleanup of the log files should be done
                        if (LoggerLogCleanUpAtStartUpEnabled)
                        {
                            _logger.CleanUpLogFiles(LoggerStoredLogFiles);
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

                Text = _xmlLanguage.GetLanguageTextByXPath(@"/Application/Name", _languageName)
                    + @" " + Helper.GetApplicationVersion().ToString();

                // Only load portfolio if a portfolio is set in the settings
                if (PortfolioFileName != "")
                {
                    Text += @" (" + Path.GetFileName(PortfolioFileName) + @")";

                    LoadPortfolio();
                }

                #endregion Read shares from XML

                #region Add items to DataGridView portfolio and set DataGridView portfolio footer

                AddSharesToDataGridView();

                AddShareFooter();

                #endregion Add items to DataGridView portfolio and set DataGridView portfolio footer

                #region Select first item

                if (dgvPortfolio.Rows.Count > 0)
                {
                    dgvPortfolio.Rows[0].Selected = true;
                }

                #endregion Select first item

                #region Message if portfolio is empty

                if (_bPortfolioEmpty)
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListEmpty", _languageName),
                        _xmlLanguage, _languageName,
                        Color.OrangeRed, _logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                }

                #endregion Message if portfolio is empty

                #region Enable / disable controls

                // Enable controls if the initialization was correct and a portfolio is set
                if (ShareObjectList.Count != 0 && PortfolioFileName != "")
                {
                    // Enable controls
                    _enableDisableControlNames.Add(@"btnRefreshAll");
                    _enableDisableControlNames.Add(@"btnRefresh");
                    _enableDisableControlNames.Add(@"btnEdit");
                    _enableDisableControlNames.Add(@"btnDelete");
                    _enableDisableControlNames.Add(@"btnClearLogger");
                    _enableDisableControlNames.Add(@"dgvPortfolio");
                    _enableDisableControlNames.Add(@"dgvPortfolioFooter");
                    _enableDisableControlNames.Add(@"tabCtrlDetails");

                    // Enable controls
                    Helper.EnableDisableControls(true, this, _enableDisableControlNames);
                }

                // Enable controls if the initialization was correct and a portfolio is set
                if (_bInitFlag && PortfolioFileName == "")
                {
                    // Enable controls
                    _enableDisableControlNames.Clear();
                    _enableDisableControlNames.Add(@"menuStrip1");

                    Helper.EnableDisableControls(true, this, _enableDisableControlNames);
                }

                #endregion Enable / disable controls
            }
            catch (LoggerException loggerException)
            {
#if DEBUG
                MessageBox.Show(loggerException.Message, @"Error - FrmMain()", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Check the logger initialization state
                switch (_logger.InitState)
                {
                    case Logger.EInitState.NotInitialized:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.WrongSize:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WrongSize", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ComponentLevelInvalid:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentLevelInvalid", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ComponentNamesMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentNamesMaxCount", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.StateLevelInvalid:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StateLevelInvalid", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.StatesMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StatesMaxCount", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.ColorsMaxCount:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ColorsMaxCount", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.WriteStartupFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WriteStartupFailed", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.LogPathCreationFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/LogPathCreationFailed", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.InitializationFailed:
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/InitializationFailed", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                        break;
                    case Logger.EInitState.Initialized:
                        switch (_logger.LoggerState)
                        {
                            case Logger.ELoggerState.CleanUpLogFilesFailed:
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesFailed", _languageName),
                                    _xmlLanguage, _languageName,
                                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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

        #region MainForm load

        /// <summary>
        /// This function sets the window position and the window size of the MainForm
        /// </summary>
        /// <param name="sender">MainForm</param>
        /// <param name="e">EventArgs</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Location = _windowPosition;
            Size = _windowSize;
        }

        #endregion MainForm load

        #region MainForm closing

        /// <summary>
        /// This function saves the settings when the MainForm is closing
        /// </summary>
        /// <param name="sender">MainFrom</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Set window start position and window size
            if (_xmlSettings != null)
            {
                try
                {
                    // Save current window position
                    var nodePosX = _xmlSettings.SelectSingleNode("/Settings/Window/PosX");
                    var nodePosY = _xmlSettings.SelectSingleNode("/Settings/Window/PosY");
                    
                    if (nodePosX != null)
                        nodePosX.InnerXml = Location.X.ToString();
                    if (nodePosY != null)
                        nodePosY.InnerXml = Location.Y.ToString();

                    // Save current window size
                    var nodeWidth = _xmlSettings.SelectSingleNode("/Settings/Window/Width");
                    var nodeHeigth = _xmlSettings.SelectSingleNode("/Settings/Window/Height");
                    
                    if (nodeWidth != null)
                        nodeWidth.InnerXml = Size.Width.ToString();
                    if (nodeHeigth != null)
                        nodeHeigth.InnerXml = Size.Height.ToString();

                    // Close reader for saving
                    _xmlReaderSettings.Close();
                    // Save settings
                    _xmlSettings.Save(SettingsFileName);
                    // Create a new reader to test if the saved values could be loaded
                    _xmlReaderSettings = XmlReader.Create(SettingsFileName, _xmlReaderSettingsSettings);
                    _xmlSettings.Load(_xmlReaderSettings);
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("MainForm_FormClosing()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    _logger.AddEntry(_xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SaveSettingsFailed",
                    _languageName), (Logger.ELoggerStateLevels)EStateLevels.FatalError, (Logger.ELoggerComponentLevels)EComponentLevels.Application);
                }
            }
        }

        #endregion MainFrom closing

        #region MainForm resize

        // TODO here we check if the user minimized window, we hide the form
        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }

        #endregion MainFrom resize

        #region MainFrom visibility changed

        // TODO when the form is hidden, we show notify icon and when the form is visible we hide it
        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = !this.Visible;
        }

        #endregion MainForm visibility changed

        #region NotifyIcon

        // TODO When click on notify icon, we bring the form to front
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
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
