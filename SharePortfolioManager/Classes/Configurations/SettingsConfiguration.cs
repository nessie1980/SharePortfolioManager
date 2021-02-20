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
//#define DEBUG_SETTINGS

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace SharePortfolioManager.Classes.Configurations
{
    public class ChartingColors
    {
        #region Variablen

        private const string ClosingPriceKeyName = Parser.DailyValues.ClosingPriceName;
        private readonly Color _closingPriceColor = Color.Black;

        private const string OpeningPriceKeyName = Parser.DailyValues.OpeningPriceName;
        private readonly Color _openingPriceColor = Color.DarkGreen;

        private const string TopPriceKeyName = Parser.DailyValues.TopName;
        private readonly Color _topPriceColor = Color.DarkBlue;

        private const string BottomPriceKeyName = Parser.DailyValues.BottomName;
        private readonly Color _bottomPriceColor = Color.DarkRed;

        private const string VolumeKeyName = Parser.DailyValues.VolumeName;
        private readonly Color _volumeColor = Color.Goldenrod;

        private const string LastBuyKeyName = "LastBuy";
        private readonly Color _lastBuyColor = Color.Blue;

        private const string AllBuysWithoutLastBuyKeyName = "RestOfBuys";
        private readonly Color _allBuysWithoutLastBuyColor = Color.Red;

        private const string LastSaleKeyName = "LastSale";
        private readonly Color _lastSaleColor = Color.Aqua;

        private const string AllSalesWithoutLastSaleKeyName = "RstOfSales";
        private readonly Color _allSalesWithoutLastSaleColor = Color.OrangeRed;

        private readonly Dictionary<string, Color> _chartingColors = new Dictionary<string, Color>();

        #endregion Variablen

        #region Methods

        public ChartingColors()
        {
            // Set default colors
            SetDefaultColors();
        }

        public void SetDefaultColors()
        {
            _chartingColors.Clear();

            _chartingColors.Add(ClosingPriceKeyName, _closingPriceColor);
            _chartingColors.Add(OpeningPriceKeyName, _openingPriceColor);
            _chartingColors.Add(TopPriceKeyName, _topPriceColor);
            _chartingColors.Add(BottomPriceKeyName, _bottomPriceColor);
            _chartingColors.Add(VolumeKeyName, _volumeColor);

            _chartingColors.Add(LastBuyKeyName, _lastBuyColor);
            _chartingColors.Add(LastSaleKeyName, _allBuysWithoutLastBuyColor);
            _chartingColors.Add(AllBuysWithoutLastBuyKeyName, _lastSaleColor);
            _chartingColors.Add(AllSalesWithoutLastSaleKeyName, _allSalesWithoutLastSaleColor);
        }

        public bool SetColorViaKeyName(string keyName, Color color)
        {
            if (!_chartingColors.ContainsKey(keyName)) return false;

            _chartingColors[keyName] = color;
            return true;
        }

        public bool GetColorViaKeyName(string keyName, out Color color)
        {
            if (!_chartingColors.ContainsKey(keyName))
            {
                color = Color.Black;
                return false;
            }

            color = _chartingColors[keyName];
            return true;
        }

        public bool SetClosingPriceColor( Color color)
        {
            if (!_chartingColors.ContainsKey(ClosingPriceKeyName)) return false;

            _chartingColors[ClosingPriceKeyName] = color;
            return true;
        }

        public Color GetClosingPriceColor()
        {
            return _chartingColors.ContainsKey(ClosingPriceKeyName) ? _chartingColors[ClosingPriceKeyName] : _closingPriceColor;
        }

        public bool SetOpeningPriceColor(Color color)
        {
            if (!_chartingColors.ContainsKey(OpeningPriceKeyName)) return false;

            _chartingColors[OpeningPriceKeyName] = color;
            return true;
        }

        public Color GetOpeningPriceColor()
        {
            return _chartingColors.ContainsKey(OpeningPriceKeyName) ? _chartingColors[OpeningPriceKeyName] : _openingPriceColor;
        }

        public bool SetTopPriceColor(Color color)
        {
            if (!_chartingColors.ContainsKey(TopPriceKeyName)) return false;

            _chartingColors[TopPriceKeyName] = color;
            return true;
        }

        public Color GetTopPriceColor()
        {
            return _chartingColors.ContainsKey(TopPriceKeyName) ? _chartingColors[TopPriceKeyName] : _topPriceColor;
        }

        public bool SetBottomPriceColor(Color color)
        {
            if (!_chartingColors.ContainsKey(BottomPriceKeyName)) return false;

            _chartingColors[BottomPriceKeyName] = color;
            return true;
        }

        public Color GetBottomPriceColor()
        {
            return _chartingColors.ContainsKey(BottomPriceKeyName) ? _chartingColors[BottomPriceKeyName] : _bottomPriceColor;
        }

        public bool SetVolumeColor(Color color)
        {
            if (!_chartingColors.ContainsKey(VolumeKeyName)) return false;

            _chartingColors[VolumeKeyName] = color;
            return true;
        }

        public Color GetVolumeColor()
        {
            return _chartingColors.ContainsKey(VolumeKeyName) ? _chartingColors[VolumeKeyName] : _volumeColor;
        }

        public Color GetLastBuyColor()
        {
            return _chartingColors.ContainsKey(LastBuyKeyName) ? _chartingColors[LastBuyKeyName] : _lastBuyColor;
        }

        public Color GetLastSaleColor()
        {
            return _chartingColors.ContainsKey(LastSaleKeyName) ? _chartingColors[LastSaleKeyName] : _lastSaleColor;
        }

        public Color GetAllBuysColor()
        {
            return _chartingColors.ContainsKey(AllBuysWithoutLastBuyKeyName) ? _chartingColors[AllBuysWithoutLastBuyKeyName] : _allBuysWithoutLastBuyColor;
        }

        public Color GetAllSalesColor()
        {
            return _chartingColors.ContainsKey(AllSalesWithoutLastSaleKeyName) ? _chartingColors[AllSalesWithoutLastSaleKeyName] : _allSalesWithoutLastSaleColor;
        }

        #endregion Methods
    }

    public static class SettingsConfiguration
    {
        #region Error codes

        // Error codes of the WebSiteConfiguration class
        public enum ESettingsErrorCode
        {
            ConfigurationSaveSuccessful = 1,
            ConfigurationLoadSuccessful = 0,
            FileDoesNotExit = -1,
            ConfigurationXmlError = -2,
            ConfigurationLoadFailed = -3,
            ConfigurationSaveFailed = -4
        };

        #endregion Error codes

        #region Properties

        /// <summary>
        /// Flag if the configuration load was successful
        /// </summary>
        public static bool InitFlag { internal set; get; }

        /// <summary>
        /// Error code of the settings configuration load
        /// </summary>
        public static ESettingsErrorCode ErrorCode { internal set; get; }

        /// <summary>
        /// Last exception of the web site configuration load
        /// </summary>
        public static Exception LastException { internal set; get; }

        /// <summary>
        /// XML file with the settings configuration
        /// </summary>
        public const string FileName = @"Settings\Settings.xml";

#pragma warning disable 649
        private static readonly XmlReaderSettings ReaderSettings;
#pragma warning restore 649

        public static XmlDocument XmlDocument;

        private static XmlReader _xmlReader;

        #endregion Properties

        #region Settings Properties

        public static string PortfolioName { internal set; get; } = @"Portfolios\Portfolio.XML";

        public static string LanguageName { internal set; get; } = @"English";

        #region Window

        public static Point NormalWindowPosition { internal set; get; }

        public static Size NormalWindowSize { internal set; get; }

        public static FormWindowState MyWindowState { internal set; get; }

        public static int MinimumSizeHeight { internal set; get; }

        public static int MinimumSizeWidth { internal get; set; }

        #endregion Window

        #region Timers

        public static int StatusMessageClearTimerValue { internal set; get; } = 5000;

        public static int StartNextShareUpdateTimerValue { internal set; get; } = 5000;

        public static int TimerStartNextShareUpdateInterval { internal set; get; }

        public static int TimerStatusMessageClearInterval { internal set; get; }

        #endregion Timers

        #region Charting

        public static ChartingInterval ChartingIntervalValue { internal set; get; } = ChartingInterval.Week;

        public static int ChartingAmount { internal set; get; } = 1;

        public static ChartingColors ChartingColors { internal set; get; } = new ChartingColors();

        #endregion Charting

        #region Parser

        // Debugging of the parser for the market value
        public static bool ParserMarketValuesDebuggingEnable { internal set; get; }

        // Debugging of the parser for the daily value enabling
        public static bool ParserDailyValuesDebuggingEnable { internal set; get; }

        // Parser for the daily values

        #endregion Parser

        #region Logger

        private static int _loggerStateLevel;

        private static int _loggerComponentLevel;

        private static int _loggerStoredLogFiles;

        public static int LoggerGuiEntriesSize { internal set; get; } = 25;

        public static List<Color> LoggerConsoleColorList { internal set; get; } = new List<Color>()
            {Color.Black, Color.Black, Color.OrangeRed, Color.Red, Color.DarkRed};

        public static Dictionary<string, Color> LoggerColorDictionary { internal set; get; } =
            new Dictionary<string, Color>();

        public static int LoggerStateLevel
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

        public static int LoggerComponentLevel
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

        public static int LoggerStoredLogFiles
        {
            get => _loggerStoredLogFiles;
            set => _loggerStoredLogFiles = value > 1 ? value : 10;
        }

        public static bool LoggerLogToFileEnabled { get; set; }

        public static bool LoggerLogCleanUpAtStartUpEnabled { get; set; }

        #endregion Logger

        #endregion Settings Properties

        #region Read settings

        /// <summary>
        /// This function reads the setting from the Settings.xml
        /// and sets the values to the member variables
        /// </summary>
        public static bool LoadSettingsConfiguration(bool initFlag, Size minimumSize)
        {
            // Set minimum size of the application window
            MinimumSizeHeight = minimumSize.Height;
            MinimumSizeWidth = minimumSize.Width;

            InitFlag = initFlag;

            // Flag if the settings load was successful
            var loadSettings = true;

            try
            {
                // Check if the website configuration file exists
                if (!File.Exists(FileName))
                {
                    ErrorCode = ESettingsErrorCode.FileDoesNotExit;

                    return false;
                }

                //// Create the validating reader and specify DTD validation.
                //ReaderSettings = new XmlReaderSettings();
                //ReaderSettings.DtdProcessing = DtdProcessing.Parse;
                //ReaderSettings.ValidationType = ValidationType.DTD;
                //ReaderSettings.ValidationEventHandler += eventHandler;

                _xmlReader = XmlReader.Create(FileName, ReaderSettings);

                // Pass the validating reader to the XML document.
                // Validation fails due to an undefined attribute, but the 
                // data is still loaded into the document.
                XmlDocument = new XmlDocument();
                XmlDocument.Load(_xmlReader);

                #region Portfolio

                // Read portfolio name
                var nodePortfolioFile = XmlDocument.SelectSingleNode("/Settings/Portfolio");
                if (nodePortfolioFile != null)
                    PortfolioName = nodePortfolioFile.InnerXml;

                #endregion Portfolio

                #region Position / Size / State

                // Read last application window position and window size
                // Default values
                var iPosX = 0;
                var iPosY = 0;
                var iWidth = MinimumSizeWidth;
                var iHeight = MinimumSizeHeight;

                // Read position
                var nodePosX = XmlDocument.SelectSingleNode("/Settings/Window/PosX");
                var nodePosY = XmlDocument.SelectSingleNode("/Settings/Window/PosY");

                // Convert to int values
                if (nodePosX != null)
                {
                    if (!int.TryParse(nodePosX.InnerXml, out iPosX))
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                if (nodePosY != null)
                {
                    if (!int.TryParse(nodePosY.InnerXml, out iPosY))
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                // Set position
                NormalWindowPosition = new Point(iPosX, iPosY);

                // Read size
                var nodeWidth = XmlDocument.SelectSingleNode("/Settings/Window/Width");
                var nodeHeight = XmlDocument.SelectSingleNode("/Settings/Window/Height");

                // Convert to int values
                if (nodeWidth != null)
                {
                    if (!int.TryParse(nodeWidth.InnerXml, out iWidth))
                        loadSettings = false;
                    else
                    {
                        if (iWidth < MinimumSizeWidth)
                            iWidth = MinimumSizeWidth;
                    }
                }
                else
                    loadSettings = false;

                if (nodeHeight != null)
                {
                    if (!int.TryParse(nodeHeight.InnerXml, out iHeight))
                        loadSettings = false;
                    else
                    {
                        if (iHeight < MinimumSizeHeight)
                            iHeight = MinimumSizeHeight;
                    }
                }
                else
                    loadSettings = false;

                // Set size
                NormalWindowSize = new Size(iWidth, iHeight);

                // Read state
                var nodeWindowState = XmlDocument.SelectSingleNode("/Settings/Window/State");

                // Set right window state
                if (nodeWindowState != null)
                {
                    switch (nodeWindowState.InnerXml)
                    {
                        case "Normal":
                            MyWindowState = FormWindowState.Normal;
                            break;
                        case "Minimized":
                            MyWindowState = FormWindowState.Minimized;
                            break;
                        case "Maximized":
                            MyWindowState = FormWindowState.Maximized;
                            break;
                        default:
                            MyWindowState = FormWindowState.Normal;
                            break;
                    }
                }
                else
                    loadSettings = false;

                #endregion Position / Size / State

                #region Language

                // Read the language settings
                var nodeListLanguage = XmlDocument.SelectNodes("/Settings/Language");
                if (nodeListLanguage != null && nodeListLanguage.Count > 0)
                {
                    foreach (XmlNode nodeElement in nodeListLanguage)
                    {
                        if (nodeElement != null)
                        {
                            LanguageName = nodeElement.InnerText;
                        }
                    }
                }
                else
                {
                    loadSettings = false;
                }

                #endregion Language

                #region Start next share update timer

                // Read the time value for starting the next share update
                var nodeStartNextShareUpdate = XmlDocument.SelectSingleNode("/Settings/StartNextShareUpdate");
                if (nodeStartNextShareUpdate != null)
                {
                    if (int.TryParse(nodeStartNextShareUpdate.InnerText, out var iOutResult))
                        StartNextShareUpdateTimerValue = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                // Set timerStartNextShareUpdate value
                TimerStartNextShareUpdateInterval = StartNextShareUpdateTimerValue;

                #endregion Start next share update timer

                #region State clear timer

                // Read the time value for clearing the status message
                var nodeStatusMessageClear = XmlDocument.SelectSingleNode("/Settings/StatusMessageClear");
                if (nodeStatusMessageClear != null)
                {
                    if (int.TryParse(nodeStatusMessageClear.InnerText, out var iOutResult))
                        StatusMessageClearTimerValue = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                // Set timerStatusMessageClear value
                TimerStatusMessageClearInterval = StatusMessageClearTimerValue;

                #endregion State clear timer

                #region Sounds

                var nodeUpdateFinishSoundEnable = XmlDocument.SelectSingleNode("/Settings/Sounds/UpdateFinishedEnabled");
                if (nodeUpdateFinishSoundEnable != null)
                {
                    if (bool.TryParse(nodeUpdateFinishSoundEnable.InnerText, out var bOutResult))
                        Sound.UpdateFinishedEnable = bOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                var nodeUpdateFinishSound = XmlDocument.SelectSingleNode("/Settings/Sounds/UpdateFinished");
                if (nodeUpdateFinishSound != null)
                {
                    Sound.UpdateFinishedFileName = Path.GetDirectoryName(Application.ExecutablePath) + Sound.SoundFilesDirectory + nodeUpdateFinishSound.InnerText;
                }
                else
                    loadSettings = false;

                var nodeErrorSoundEnable = XmlDocument.SelectSingleNode("/Settings/Sounds/ErrorEnabled");
                if (nodeErrorSoundEnable != null)
                {
                    if (bool.TryParse(nodeErrorSoundEnable.InnerText, out var bOutResult))
                        Sound.ErrorEnable = bOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                var nodeErrorSound = XmlDocument.SelectSingleNode("/Settings/Sounds/Error");
                if (nodeErrorSound != null)
                {
                    Sound.ErrorFileName = Path.GetDirectoryName(Application.ExecutablePath) + Sound.SoundFilesDirectory + nodeErrorSound.InnerText;
                }
                else
                    loadSettings = false;

                #endregion Sounds

                #region Charting values

                // Read the charting values
                var nodeChartingInterval = XmlDocument.SelectSingleNode("/Settings/Charting/Interval");
                if (nodeChartingInterval != null)
                {
                    switch (nodeChartingInterval.InnerText)
                    {
                        case "Week":
                            ChartingIntervalValue = ChartingInterval.Week;
                            break;
                        case "Month":
                            ChartingIntervalValue = ChartingInterval.Month;
                            break;
                        case "Quarter":
                            ChartingIntervalValue = ChartingInterval.Quarter;
                            break;
                        case "Year":
                            ChartingIntervalValue = ChartingInterval.Year;
                            break;
                        default:
                            loadSettings = false;
                            break;
                    }
                }
                else
                    loadSettings = false;

                // Read the charting values
                var nodeChartingAmount = XmlDocument.SelectSingleNode("/Settings/Charting/Amount");
                if (nodeChartingAmount != null)
                {
                    if (int.TryParse(nodeChartingAmount.InnerText, out var iOutResult))
                        ChartingAmount = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #region Charting colors

                // Read the colors for the various charting lines
                var nodeChartingColorsDailyValues = XmlDocument.SelectSingleNode("/Settings/Charting/Colors/DailyValues");
                if (nodeChartingColorsDailyValues != null)
                {
                    foreach (XmlNode color in nodeChartingColorsDailyValues.ChildNodes)
                    {
                        if (!ChartingColors.SetColorViaKeyName(color.Name, Color.FromName(color.InnerText)))
                        {
                            ChartingColors.SetDefaultColors();

                            loadSettings = false;
                            break;
                        }
                    }
                }
                else
                {
                    ChartingColors.SetDefaultColors();

                    loadSettings = false;
                }

                // Read the colors for the various charting lines
                if (loadSettings)
                {
                    var nodeChartingColors = XmlDocument.SelectSingleNode("/Settings/Charting/Colors/Information");
                    if (nodeChartingColors != null)
                    {
                        foreach (XmlNode color in nodeChartingColors.ChildNodes)
                        {
                            ChartingColors.SetColorViaKeyName(color.Name, Color.FromName(color.InnerText));
                        }
                    }
                    else
                    {
                        ChartingColors.SetDefaultColors();

                        loadSettings = false;
                    }
                }

                #endregion Charting colors

                #endregion Charting values

                #region Logger GUI stores size

                // Read the GUI entries size
                var nodeGuiEntriesSize = XmlDocument.SelectSingleNode("/Settings/Logger/GUIEntries");
                if (nodeGuiEntriesSize != null)
                {
                    if (int.TryParse(nodeGuiEntriesSize.InnerText, out var iOutResult))
                        LoggerGuiEntriesSize = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger GUI stores size

                #region Logger enable to log file

                // Read the flag if the logger should be used
                var nodeLoggerLogToFileEnabled = XmlDocument.SelectSingleNode("/Settings/Logger/LogToFileEnable");
                if (nodeLoggerLogToFileEnabled != null)
                {
                    if (bool.TryParse(nodeLoggerLogToFileEnabled.InnerText, out var bOutResult))
                        LoggerLogToFileEnabled = bOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger enable to log file

                #region Logger stored log files

                // Read the time value for clearing the status message
                var nodeStoredLogFiles = XmlDocument.SelectSingleNode("/Settings/Logger/StoredLogFiles");
                if (nodeStoredLogFiles != null)
                {
                    if (int.TryParse(nodeStoredLogFiles.InnerText, out var iOutResult))
                        LoggerStoredLogFiles = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger stored log files

                #region Logger cleanup at startup enable

                // Read the flag if the logger should be used
                var nodeLoggerCleanUpAtStartUpEnabled =
                    XmlDocument.SelectSingleNode("/Settings/Logger/LogCleanUpAtStartUpEnable");
                if (nodeLoggerCleanUpAtStartUpEnabled != null)
                {
                    if (bool.TryParse(nodeLoggerCleanUpAtStartUpEnabled.InnerText, out var bOutResult))
                        LoggerLogCleanUpAtStartUpEnabled = bOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger cleanup at startup enable

                #region Logger components level

                // Read the value which logger components should be logged
                var nodeLogComponents = XmlDocument.SelectSingleNode("/Settings/Logger/LogComponents");
                if (nodeLogComponents != null)
                {
                    if (int.TryParse(nodeLogComponents.InnerText, out var iOutResult))
                        LoggerComponentLevel = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger components level

                #region Logger log level

                // Read the value which logger levels should be logged
                var nodeLogLevels = XmlDocument.SelectSingleNode("/Settings/Logger/LogLevels");
                if (nodeLogLevels != null)
                {
                    if (int.TryParse(nodeLogLevels.InnerText, out var iOutResult))
                        LoggerStateLevel = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger log level

                #region Logger colors

                // Read the colors for the various logger levels
                var nodeLogColors = XmlDocument.SelectSingleNode("/Settings/Logger/LogColors");
                if (nodeLogColors != null)
                {
                    for (var index = 0; index < nodeLogColors.ChildNodes.Count; index++)
                    {
                        var color = nodeLogColors.ChildNodes[index];
                        LoggerColorDictionary.Add(color.Name, Color.FromName(color.InnerText));
                    }
                }
                else
                {
                    LoggerColorDictionary.Clear();
                    LoggerColorDictionary.Add(@"Start", LoggerConsoleColorList[0]);
                    LoggerColorDictionary.Add(@"Info", LoggerConsoleColorList[1]);
                    LoggerColorDictionary.Add(@"Warning", LoggerConsoleColorList[2]);
                    LoggerColorDictionary.Add(@"Error", LoggerConsoleColorList[3]);
                    LoggerColorDictionary.Add(@"FatalError", LoggerConsoleColorList[4]);

                    loadSettings = false;
                }

                // Get color list from settings.
                // Remove Control colors.
                var loggerConsoleColorList =
                    LoggerColorDictionary.Select(color => color.Value).ToList();

                // Clear list and then add the read settings colors
                LoggerConsoleColorList.Clear();
                LoggerConsoleColorList = loggerConsoleColorList;

                #endregion Logger colors

                #region Show exception messages

                // Read the flag if exception messages should be shown
                var nodeShowExceptionMessages = XmlDocument.SelectSingleNode("/Settings/ShowExceptionMessages");
                if (nodeShowExceptionMessages != null)
                {
                    if (bool.TryParse(nodeShowExceptionMessages.InnerText, out var bOutResult))
                        Helper.ShowExceptionMessageFlag = bOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Show exception messages

                #region Read parser debugging flag

                // Read the flag if parser for the market value should be enabled
                var nodeMarketValuesParserDebugging = XmlDocument.SelectSingleNode("/Settings/Parser/MarketValuesDebuggingEnable");
                if (nodeMarketValuesParserDebugging != null)
                {
                    if (bool.TryParse(nodeMarketValuesParserDebugging.InnerText, out var bOutResult))
                        ParserMarketValuesDebuggingEnable = bOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                // Read the flag if parser for the market value should be enabled
                var nodeDailyValuesParserDebugging = XmlDocument.SelectSingleNode("/Settings/Parser/DailyValuesDebuggingEnable");
                if (nodeDailyValuesParserDebugging != null)
                {
                    if (bool.TryParse(nodeDailyValuesParserDebugging.InnerText, out var bOutResult))
                        ParserDailyValuesDebuggingEnable = bOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Read parser debugging flag

                InitFlag = loadSettings;

                ErrorCode = InitFlag ? ESettingsErrorCode.ConfigurationLoadSuccessful : ESettingsErrorCode.ConfigurationLoadFailed;

                return InitFlag;
            }
            catch (XmlException ex)
            {
                // Set last exception 
                LastException = ex;

                // Close website reader
                _xmlReader?.Close();

                // Set error code
                ErrorCode = ESettingsErrorCode.ConfigurationXmlError;

                // Set initialization flag
                InitFlag = false;
                return InitFlag;
            }
            catch (Exception ex)
            {
                // Set last exception 
                LastException = ex;

                // Close website reader
                _xmlReader?.Close();

                // Set error code
                ErrorCode = ESettingsErrorCode.ConfigurationLoadFailed;

                // Set initialization flag
                InitFlag = false;
                return InitFlag;
            }
        }

        #endregion Read settings

        public static bool SaveSettingsConfiguration()
        {
            try
            {
                // Flag if the settings save was successful
                var saveSettings = true;

                // Check if the load was successful
                if (XmlDocument != null)
                {
                    #region Portfolio

                    // Set portfolio name
                    var nodePortfolioFile = XmlDocument.SelectSingleNode("/Settings/Portfolio");
                    if (nodePortfolioFile != null)
                    {
                        nodePortfolioFile.InnerXml = PortfolioName;
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Portfolio

                    #region Position / Size / State

                    // Save last application window position and window size
                    // Save current window position
                    var nodePosX = XmlDocument.SelectSingleNode("/Settings/Window/PosX");
                    var nodePosY = XmlDocument.SelectSingleNode("/Settings/Window/PosY");

                    if (nodePosX != null)
                    {
                        nodePosX.InnerXml = NormalWindowPosition.X.ToString();
                    }
                    else
                    {
                        saveSettings = true;
                    }

                    if (nodePosY != null)
                    {
                        nodePosY.InnerXml = NormalWindowPosition.Y.ToString();
                    }
                    else
                    {
                        saveSettings = true;
                    }

                    // Save current window size
                    var nodeWidth = XmlDocument.SelectSingleNode("/Settings/Window/Width");
                    var nodeHeight = XmlDocument.SelectSingleNode("/Settings/Window/Height");

                    if (nodeWidth != null)
                    {
                        nodeWidth.InnerXml = NormalWindowSize.Width.ToString();
                    }
                    else
                    {
                        saveSettings = true;
                    }

                    if (nodeHeight != null)
                    {
                        nodeHeight.InnerXml = NormalWindowSize.Height.ToString();
                    }
                    else
                    {
                        saveSettings = true;
                    }

                    // Save window state
                    var nodeWindowState = XmlDocument.SelectSingleNode("/Settings/Window/State");

                    if (nodeWindowState != null)
                    {
                        nodeWindowState.InnerXml = MyWindowState.ToString();
                    }
                    else
                    {
                        saveSettings = true;
                    }

                    #endregion Position / Size / State

                    #region Language

                    // Save language settings
                    var nodeListLanguage = XmlDocument.SelectSingleNode("/Settings/Language");
                    if (nodeListLanguage != null )
                    {
                        nodeListLanguage.InnerXml = LanguageName;
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Language

                    #region Start next share update timer

                    // Save the time value for starting the next share update
                    var nodeStartNextShareUpdate = XmlDocument.SelectSingleNode("/Settings/StartNextShareUpdate");
                    if (nodeStartNextShareUpdate != null)
                    {
                        nodeStartNextShareUpdate.InnerXml = TimerStartNextShareUpdateInterval.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Start next share update timer

                    #region State clear timer

                    // Save the time value for clearing the status message
                    var nodeStatusMessageClear = XmlDocument.SelectSingleNode("/Settings/StatusMessageClear");
                    if (nodeStatusMessageClear != null)
                    {
                        nodeStatusMessageClear.InnerText = TimerStatusMessageClearInterval.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion State clear timer

                    #region Sounds

                    // Save sounds
                    var nodeUpdateFinishSoundEnable = XmlDocument.SelectSingleNode("/Settings/Sounds/UpdateFinishedEnabled");
                    if (nodeUpdateFinishSoundEnable != null)
                    {
                        nodeUpdateFinishSoundEnable.InnerText = Sound.UpdateFinishedEnable.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    var nodeUpdateFinishSound = XmlDocument.SelectSingleNode("/Settings/Sounds/UpdateFinished");
                    if (nodeUpdateFinishSound != null)
                    {
                        nodeUpdateFinishSound.InnerText = Path.GetFileName(Sound.UpdateFinishedFileName);
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    var nodeErrorSoundEnable = XmlDocument.SelectSingleNode("/Settings/Sounds/ErrorEnabled");
                    if (nodeErrorSoundEnable != null)
                    {
                        nodeErrorSoundEnable.InnerText = Sound.ErrorEnable.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    var nodeErrorSound = XmlDocument.SelectSingleNode("/Settings/Sounds/Error");
                    if (nodeErrorSound != null)
                    {
                        nodeErrorSound.InnerText = Path.GetFileName(Sound.ErrorFileName);
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Sounds

                    #region Charting values

                    // Save the charting values
                    var nodeChartingInterval = XmlDocument.SelectSingleNode("/Settings/Charting/Interval");
                    if (nodeChartingInterval != null)
                    {
                        var chartingIntervalValue = @"Week";

                        switch (ChartingIntervalValue)
                        {
                            case ChartingInterval.Week:
                            {
                                chartingIntervalValue = @"Week";
                            }
                                break;
                            case ChartingInterval.Month:
                            {
                                chartingIntervalValue = @"Month";
                            }
                                break;
                            case ChartingInterval.Quarter:
                            {
                                chartingIntervalValue = @"Quarter";
                            }
                                break;
                            case ChartingInterval.Year:
                            {
                                chartingIntervalValue = @"Year";
                            }
                                break;
                            default:
                            {
                                saveSettings = false;
                            }
                                break;
                        }

                        nodeChartingInterval.InnerText = chartingIntervalValue;
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    // Save the charting values
                    var nodeChartingAmount = XmlDocument.SelectSingleNode("/Settings/Charting/Amount");
                    if (nodeChartingAmount != null)
                    {
                        nodeChartingAmount.InnerText = ChartingAmount.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #region Charting colors

                    // Save the colors for the various charting lines
                    var nodeChartingColorsDailyValues = XmlDocument.SelectSingleNode("/Settings/Charting/Colors/DailyValues");
                    if (nodeChartingColorsDailyValues != null)
                    {
                        foreach (XmlNode color in nodeChartingColorsDailyValues.ChildNodes)
                        {
                            if (ChartingColors.GetColorViaKeyName(color.Name, out var getColor))
                            {
                                color.InnerXml = getColor.Name;
                            }
                            else
                            {
                                saveSettings = false;
                            }
                        }
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    // Save the colors for the various logger levels
                    var nodeChartingColors = XmlDocument.SelectSingleNode("/Settings/Charting/Colors/Information");
                    if (nodeChartingColors != null)
                    {
                        foreach (XmlNode color in nodeChartingColors.ChildNodes)
                        {
                            if (ChartingColors.GetColorViaKeyName(color.Name, out var getColor))
                            {
                                color.InnerXml = getColor.Name;
                            }
                            else
                            {
                                saveSettings = false;
                            }
                        }
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Charting colors

                    #endregion Charting values

                    #region Logger GUI stores size

                    // Save the GUI entries size
                    var nodeGuiEntriesSize = XmlDocument.SelectSingleNode("/Settings/Logger/GUIEntries");
                    if (nodeGuiEntriesSize != null)
                    {
                        nodeGuiEntriesSize.InnerText = LoggerGuiEntriesSize.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Logger GUI stores size

                    #region Logger enable to log file

                    // Save the flag if the logger should be used
                    var nodeLoggerLogToFileEnabled = XmlDocument.SelectSingleNode("/Settings/Logger/LogToFileEnable");
                    if (nodeLoggerLogToFileEnabled != null)
                    {
                        nodeLoggerLogToFileEnabled.InnerText = LoggerLogToFileEnabled.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Logger enable to log file

                    #region Logger stored log files

                    // Save the time value for clearing the status message
                    var nodeStoredLogFiles = XmlDocument.SelectSingleNode("/Settings/Logger/StoredLogFiles");
                    if (nodeStoredLogFiles != null)
                    {
                        nodeStoredLogFiles.InnerText = LoggerStoredLogFiles.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Logger stored log files

                    #region Logger cleanup at startup enable

                    // Save the flag if the logger should be used
                    var nodeLoggerCleanUpAtStartUpEnabled =
                        XmlDocument.SelectSingleNode("/Settings/Logger/LogCleanUpAtStartUpEnable");
                    if (nodeLoggerCleanUpAtStartUpEnabled != null)
                    {
                        nodeLoggerCleanUpAtStartUpEnabled.InnerText = LoggerLogCleanUpAtStartUpEnabled.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Logger cleanup at startup enable

                    #region Logger components level

                    // Save the value which logger components should be logged
                    var nodeLogComponents = XmlDocument.SelectSingleNode("/Settings/Logger/LogComponents");
                    if (nodeLogComponents != null)
                    {
                        nodeLogComponents.InnerText = LoggerComponentLevel.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Logger components level

                    #region Logger log level

                    // Save the value which logger levels should be logged
                    var nodeLogLevels = XmlDocument.SelectSingleNode("/Settings/Logger/LogLevels");
                    if (nodeLogLevels != null)
                    {  
                        nodeLogLevels.InnerText = LoggerStateLevel.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Logger log level

                    #region Logger colors

                    // Save the colors for the various logger levels
                    var nodeLogColors = XmlDocument.SelectSingleNode("/Settings/Logger/LogColors");
                    if (nodeLogColors != null)
                    {
                        foreach (XmlNode color in nodeLogColors.ChildNodes)
                        {
                            if (LoggerColorDictionary.ContainsKey(color.Name))
                            {
                                color.InnerXml = LoggerColorDictionary[color.Name].Name;
                            }
                            else
                            {
                                saveSettings = false;
                            }
                        }
                    }
                    else
                        saveSettings = false;

                    #endregion Logger colors

                    #region Show exception messages

                    // Save the flag if exception messages should be shown
                    var nodeShowExceptionMessages = XmlDocument.SelectSingleNode("/Settings/ShowExceptionMessages");
                    if (nodeShowExceptionMessages != null)
                    {
                        nodeShowExceptionMessages.InnerText = Helper.ShowExceptionMessageFlag.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Show exception messages

                    #region Read parser debugging flag

                    // Save the flag if parser for the market value should be enabled
                    var nodeMarketValuesParserDebugging = XmlDocument.SelectSingleNode("/Settings/Parser/MarketValuesDebuggingEnable");
                    if (nodeMarketValuesParserDebugging != null)
                    {
                        nodeMarketValuesParserDebugging.InnerText = ParserMarketValuesDebuggingEnable.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    // Save the flag if parser for the market value should be enabled
                    var nodeDailyValuesParserDebugging = XmlDocument.SelectSingleNode("/Settings/Parser/DailyValuesDebuggingEnable");
                    if (nodeDailyValuesParserDebugging != null)
                    {
                        nodeDailyValuesParserDebugging.InnerText = ParserDailyValuesDebuggingEnable.ToString();
                    }
                    else
                    {
                        saveSettings = false;
                    }

                    #endregion Read parser debugging flag

                    // Close reader for saving
                    _xmlReader.Close();
                    // Save settings
                    XmlDocument.Save(FileName);
                    // Create a new reader to test if the saved values could be loaded
                    _xmlReader = XmlReader.Create(FileName, ReaderSettings);
                    XmlDocument.Load(_xmlReader);

                    ErrorCode = ESettingsErrorCode.ConfigurationSaveSuccessful;

                    return saveSettings;
                }

                // Close website reader
                _xmlReader?.Close();

                // Set error code
                ErrorCode = ESettingsErrorCode.ConfigurationSaveFailed;

                return false;
            }
            catch (Exception ex)
            {
                // Set last exception 
                LastException = ex;

                // Close website reader
                _xmlReader?.Close();

                // Set error code
                ErrorCode = ESettingsErrorCode.ConfigurationSaveFailed;

                // Set initialization flag
                return false;
            }
        }
    }
}