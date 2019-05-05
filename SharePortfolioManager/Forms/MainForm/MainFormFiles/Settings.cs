//MIT License
//
//Copyright(c) 2019 nessie1980(nessie1980 @gmx.de)
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

using SharePortfolioManager.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Read settings

        /// <summary>
        /// This function reads the setting from the Settings.XML
        /// and sets the values to the member variables
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                // Flag if the settings load was successful
                var loadSettings = true;

                InitFlag = false;

                //// Create the validating reader and specify DTD validation.
                //ReaderSettings = new XmlReaderSettings();
                //ReaderSettings.DtdProcessing = DtdProcessing.Parse;
                //ReaderSettings.ValidationType = ValidationType.DTD;
                //ReaderSettings.ValidationEventHandler += eventHandler;

                ReaderSettings = XmlReader.Create(SettingsFileName, ReaderSettingsSettings);

                // Pass the validating reader to the XML document.
                // Validation fails due to an undefined attribute, but the 
                // data is still loaded into the document.
                Settings = new XmlDocument();
                Settings.Load(ReaderSettings);

                #region Portfolio

                // Read position
                var nodePortfolioFile = Settings.SelectSingleNode("/Settings/Portfolio");
                if (nodePortfolioFile != null)
                    _portfolioFileName = nodePortfolioFile.InnerXml;

                #endregion Portfolio

                #region Position / Size / State

                // Read last application window position and window size
                // Default values
                var iPosX = 0;
                var iPosY = 0;
                var iWidth = MinimumSize.Width;
                var iHeight = MinimumSize.Height;

                // Read position
                var nodePosX = Settings.SelectSingleNode("/Settings/Window/PosX");
                var nodePosY = Settings.SelectSingleNode("/Settings/Window/PosY");

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
                var nodeWidth = Settings.SelectSingleNode("/Settings/Window/Width");
                var nodeHeight = Settings.SelectSingleNode("/Settings/Window/Height");

                // Convert to int values
                if (nodeWidth != null)
                {
                    if (!int.TryParse(nodeWidth.InnerXml, out iWidth))
                        loadSettings = false;
                    else
                    {
                        if (iWidth < MinimumSize.Width)
                            iWidth = MinimumSize.Width;
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
                        if (iHeight < MinimumSize.Height)
                            iHeight = MinimumSize.Height;
                    }
                }
                else
                    loadSettings = false;

                // Set size
                NormalWindowSize = new Size(iWidth, iHeight);

                // Read state
                var nodeWindowState = Settings.SelectSingleNode("/Settings/Window/State");

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
                var nodeListLanguage = Settings.SelectNodes("/Settings/Language");
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
                var nodeStartNextShareUpdate = Settings.SelectSingleNode("/Settings/StartNextShareUpdate");
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
                timerStartNextShareUpdate.Interval = StartNextShareUpdateTimerValue;

                #endregion Start next share update timer

                #region State clear timer

                // Read the time value for clearing the status message
                var nodeStatusMessageClear = Settings.SelectSingleNode("/Settings/StatusMessageClear");
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
                timerStatusMessageClear.Interval = StatusMessageClearTimerValue;

                #endregion State clear timer

                #region Logger stored log files

                // Read the GUI entries size
                var nodeGuiEntriesSize = Settings.SelectSingleNode("/Settings/Logger/GUIEntries");
                if (nodeGuiEntriesSize != null)
                {
                    if (int.TryParse(nodeGuiEntriesSize.InnerText, out var iOutResult))
                        LoggerGuiEntriesSize = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger stored log files

                #region Logger enable to log file

                // Read the flag if the logger should be used
                var nodeLoggerLogToFileEnabled = Settings.SelectSingleNode("/Settings/Logger/LogToFileEnable");
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
                var nodeStoredLogFiles = Settings.SelectSingleNode("/Settings/Logger/StoredLogFiles");
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
                var nodeLoggerCleanUpAtStartUpEnabled = Settings.SelectSingleNode("/Settings/Logger/LogCleanUpAtStartUpEnable");
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
                var nodeLogComponents = Settings.SelectSingleNode("/Settings/Logger/LogComponents");
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
                var nodeLogLevels = Settings.SelectSingleNode("/Settings/Logger/LogLevels");
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

                var loggerConsoleColorList = new List<Color>();
                var loggerConsoleColorListString = new List<string>();

                // Read the colors for the various logger levels
                var nodeLogColors = Settings.SelectSingleNode("/Settings/Logger/LogColors");
                if (nodeLogColors != null)
                {
                    foreach(XmlNode color in nodeLogColors.ChildNodes)
                    {
                        loggerConsoleColorListString.Add(color.InnerText);
                    }
                }
                else
                    loadSettings = false;

                // Get color list from settings.
                // Remove Control colors.
                foreach (var colorName in loggerConsoleColorListString)
                {
                    foreach (var colors in Enum.GetNames(typeof(KnownColor)).Where(
                    item => !item.StartsWith("Control")).OrderBy(item => item))
                    {
                        if (colors != colorName) continue;

                        loggerConsoleColorList.Add(Color.FromName(colors));
                        break;
                    }
                }

                // Clear list and then add the read settings colors
                LoggerConsoleColorList.Clear();
                LoggerConsoleColorList = loggerConsoleColorList;

                #endregion Logger colors

                // Check if a settings value could not be load and add status message
                if (loadSettings == false)
                    Helper.AddStatusMessage(rchTxtBoxStateMessage, "Settings file incomplete or corrupt!",
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                InitFlag = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage, "Could not load '" + SettingsFileName + @"' file!",
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                // Close reader
                ReaderSettings?.Close();
            }
        }

        #endregion Read settings
    }
}