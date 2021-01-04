//MIT License
//
//Copyright(c) 2017 - 2021 nessie1980(nessie1980 @gmx.de)
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

using Logging;
using Parser;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharePortfolioManager.ChartForm;
using SharePortfolioManager.Classes.Configurations;

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

        #endregion Logger

        #endregion Variables

        #region Properties

        #region Form

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        #endregion Form

        #region Logger

        public List<string> LoggerStateList { get; set; } = new List<string>();

        public List<string> LoggerComponentNamesList { get; set; } = new List<string>();

        public string LoggerPathFileName { get; set; } = Application.StartupPath + @"\Logs\" +
            $"{DateTime.Now.Year}_{DateTime.Now.Month:00}_{DateTime.Now.Day:00}_Log.txt";

        /// <summary>
        /// Stores an instance of the Logger
        /// </summary>
        public Logger Logger { get; } = new Logger();

        #endregion Logger

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

        // Parser for the market values
        public Parser.Parser ParserMarketValues { internal set; get; }

        // Parser for the daily values
        public Parser.Parser ParserDailyValues { internal set; get; }

        #endregion Parser

        #region Charting

        public FrmChart FrmChart;

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
        /// - initialize logger
        /// - load language
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

                // Load settings values
                // ReSharper disable once VirtualMemberCallInConstructor
                InitFlag = SettingsConfiguration.LoadSettingsConfiguration(InitFlag, MinimumSize);

                #endregion Load settings

                #region Load language

                InitFlag = LanguageConfiguration.LoadLanguage();

                #endregion Load language

                #region Logger

                if (InitFlag)
                {
                    // Initialize logger
                    Logger.LoggerInitialize(
                        SettingsConfiguration.LoggerStateLevel,
                        SettingsConfiguration.LoggerComponentLevel,
                        LoggerStateList,
                        LoggerComponentNamesList,
                        SettingsConfiguration.LoggerConsoleColorList,
                        SettingsConfiguration.LoggerLogToFileEnabled,
                        SettingsConfiguration.LoggerGuiEntriesSize,
                        LoggerPathFileName, null, true);

                    // Check if the logger initialization was not successful
                    if (Logger.InitState != Logger.EInitState.Initialized)
                        InitFlag = false;
                    else
                    {
                        // Check if the startup cleanup of the log files should be done
                        if (SettingsConfiguration.LoggerLogCleanUpAtStartUpEnabled)
                        {
                            Logger.CleanUpLogFiles(SettingsConfiguration.LoggerStoredLogFiles);
                        }
                    }
                }

                #endregion Logger

                #region Load website RegEx configuration from XML

                InitFlag = WebSiteConfiguration.LoadWebSiteConfigurations(InitFlag);

                #endregion Load website RegEx configuration from XML

                #region Load document RegEx configuration from XML

                InitFlag = DocumentParsingConfiguration.LoadDocumentParsingConfigurations(InitFlag);

                #endregion Load document RegEx configuration from XML

                #region Set language values to the control

                if(LanguageConfiguration.ErrorCode == LanguageConfiguration.ELanguageErrorCode.ConfigurationLoadSuccessful)
                    SetLanguage();

                #endregion Set language values to the control

                #region Create notify icon

                if (LanguageConfiguration.ErrorCode == LanguageConfiguration.ELanguageErrorCode.ConfigurationLoadSuccessful)
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

                #region Read shares from XML / Load portfolio

                InitializeBackgroundWorkerLoadPortfolio();

                #endregion Read shares from XML / Load portfolio
            }
            catch (LoggerException loggerException)
            {
                Helper.ShowExceptionMessage(loggerException);

                // Check the logger initialization state
                switch (Logger.InitState)
                {
                    case Logger.EInitState.NotInitialized:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/Logger/LoggerErrors/NotInitialized",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.WrongSize:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WrongSize",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.ComponentLevelInvalid:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentLevelInvalid",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.ComponentNamesMaxCount:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentNamesMaxCount",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.StateLevelInvalid:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StateLevelInvalid",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.StatesMaxCount:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StatesMaxCount",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.ColorsMaxCount:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ColorsMaxCount",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.WriteStartupFailed:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WriteStartupFailed",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.LogPathCreationFailed:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/LogPathCreationFailed",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.InitializationFailed:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/InitializationFailed",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                    }
                        break;
                    case Logger.EInitState.Initialized:
                    {
                        switch (Logger.LoggerState)
                        {
                            case Logger.ELoggerState.CleanUpLogFilesFailed:
                            {
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    LanguageConfiguration.Language.GetLanguageTextByXPath(
                                        @"/Logger/LoggerStateMessages/CleanUpLogFilesFailed",
                                        SettingsConfiguration.LanguageName),
                                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                    Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                                    (int) EComponentLevels.Application);
                            }
                                break;
                            default:
                            {
                                throw (new NotImplementedException());
                            }
                        }
                    }
                        break;
                    default:
                    {
                        throw(new NotImplementedException());
                    }
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
            #region Set window settings

            Size = SettingsConfiguration.NormalWindowSize;
            Location = SettingsConfiguration.NormalWindowPosition;
            WindowState = SettingsConfiguration.MyWindowState;

            #endregion Set window settings

            #region Check initialization error codes

            CheckInitErrorCodes();

            #endregion Check initialization error codes

            #region Read shares from XML / Load portfolio

            if (InitFlag)
            {
                // Only load portfolio if a portfolio file is set in the Settings.xml
                if (SettingsConfiguration.PortfolioName != "")
                {
                    // Load portfolio via background worker
                    if (!BgwLoadPortfolio.IsBusy && InitFlag)
                    {
                        // Set 
                        pgbLoadingPortfolio.Value = 0;
                        lblLoadingPortfolio.Text =
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/LoadingPortfolio/Message", SettingsConfiguration.LanguageName)
                            + @" " + 0 + @" " + ShareObject.PercentageUnit;

                        // Show loading portfolio controls
                        tblLayPnlLoadingPortfolio.Visible = true;

                        // Start portfolio load
                        BgwLoadPortfolio.RunWorkerAsync();
                    }
                }
                else
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/PortfolioErrors/PortfolioNotSet",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.OrangeRed, Logger, (int) EStateLevels.Warning, (int) EComponentLevels.Application);

                    // Disable menu strip menu point "Save as..."
                    saveAsToolStripMenuItem.Enabled = false;

                    // Hide loading portfolio controls
                    tblLayPnlLoadingPortfolio.Visible = false;

                    EnableDisableControlNames.Clear();
                    EnableDisableControlNames.Add("menuStrip1");

                    // Disable all controls
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);
                }
            }

            #endregion Read shares from XML / Load portfolio

            #region Select first item

            if (dgvPortfolioFinalValue.Rows.Count > 0)
            {
                dgvPortfolioFinalValue.Rows[0].Selected = true;
            }

            tabCtrlShareOverviews.Select();

            #endregion Select first item

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
            if (SettingsConfiguration.SaveSettingsConfiguration()) return;

            if (Logger.InitState == Logger.EInitState.Initialized)
            {
                Logger.AddEntry(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/SaveSettingsFailed",
                        SettingsConfiguration.LanguageName), (Logger.ELoggerStateLevels) EStateLevels.FatalError,
                    (Logger.ELoggerComponentLevels) EComponentLevels.Application);
            }

            Helper.ShowExceptionMessage(SettingsConfiguration.LastException);
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
                {
                    ShowInTaskbar = false;
                    Hide();
                }
                    break;
                case FormWindowState.Maximized:
                {
                    SettingsConfiguration.MyWindowState = FormWindowState.Maximized;
                }
                    break;
                case FormWindowState.Normal:
                {
                    SettingsConfiguration.MyWindowState = FormWindowState.Normal;
                    SettingsConfiguration.NormalWindowSize = Size;
                    SettingsConfiguration.NormalWindowPosition = Location;
                }
                    break;
                default:
                {
                    SettingsConfiguration.MyWindowState = FormWindowState.Maximized;
                }
                    break;
            }
        }

        #endregion MainFrom resize

        #region MainForm location changed

        private void FrmMain_LocationChanged(object sender, EventArgs e)
        {
            if (!_formIsShown) return;

            if (WindowState == FormWindowState.Normal)
                SettingsConfiguration.NormalWindowPosition = Location;
        }

        #endregion MainForm location changed

        #region MainFrom visibility changed

        // When the form is hidden, we show notify icon and when the form is visible we hide it
        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            if(_notifyIcon != null)
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
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/NotifyIcon/Show", SettingsConfiguration.LanguageName),
                Resources.show_window_24, Show_Click);
            _notifyContextMenuStrip.Items.Add(
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/NotifyIcon/Exit", SettingsConfiguration.LanguageName),
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
            WindowState = SettingsConfiguration.MyWindowState;

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
            WindowState = SettingsConfiguration.MyWindowState;

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
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", SettingsConfiguration.LanguageName);
            btnRefresh.Text =
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", SettingsConfiguration.LanguageName);
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
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", SettingsConfiguration.LanguageName);
                btnRefresh.Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", SettingsConfiguration.LanguageName);
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
