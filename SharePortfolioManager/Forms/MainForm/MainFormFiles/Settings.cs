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
                bool loadSettings = true;

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
                XmlNode nodePortfolioFile = Settings.SelectSingleNode("/Settings/Portfolio");
                PortfolioFileName = nodePortfolioFile.InnerXml;

                #endregion Portfolio

                #region Position and size

                // Read last application window position and window size
                // Default values
                int iPosX = 0;
                int iPosY = 0;
                int iWidth = MinimumSize.Width;
                int iHeigth = MinimumSize.Height;

                // Read position
                XmlNode nodePosX = Settings.SelectSingleNode("/Settings/Window/PosX");
                XmlNode nodePosY = Settings.SelectSingleNode("/Settings/Window/PosY");

                // Convert to int values
                if (nodePosX != null)
                {
                    if (!(Int32.TryParse(nodePosX.InnerXml, out iPosX)))
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                if (nodePosY != null)
                {
                    if (!(Int32.TryParse(nodePosY.InnerXml, out iPosY)))
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                // Set position
                MyWindowPosition = new Point(iPosX, iPosY);

                // Read size
                XmlNode nodeWidth = Settings.SelectSingleNode("/Settings/Window/Width");
                XmlNode nodeHeigth = Settings.SelectSingleNode("/Settings/Window/Height");

                // Convert to int values
                if (nodeWidth != null)
                {
                    if (!(Int32.TryParse(nodeWidth.InnerXml, out iWidth)))
                        loadSettings = false;
                    else
                    {
                        if (iWidth < MinimumSize.Width)
                            iWidth = MinimumSize.Width;
                    }
                }
                else
                    loadSettings = false;

                if (nodeHeigth != null)
                {
                    if (!(Int32.TryParse(nodeHeigth.InnerXml, out iHeigth)))
                        loadSettings = false;
                    else
                    {
                        if (iHeigth < MinimumSize.Height)
                            iHeigth = MinimumSize.Height;
                    }
                }
                else
                    loadSettings = false;

                // Set size
                MyWindowSize = new Size(iWidth, iHeigth);

                #endregion Position and size

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

                #region State clear timer

                // Read the time value for clearing the status message
                var nodeStatusMessageClear = Settings.SelectSingleNode("/Settings/StatusMessageClear");
                if (nodeStatusMessageClear != null)
                {
                    int iOutResult;
                    if (Int32.TryParse(nodeStatusMessageClear.InnerText, out iOutResult))
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
                var nodeGUIEntriesSize = Settings.SelectSingleNode("/Settings/Logger/GUIEntries");
                if (nodeGUIEntriesSize != null)
                {
                    int iOutResult;
                    if (Int32.TryParse(nodeGUIEntriesSize.InnerText, out iOutResult))
                        LoggerGUIEntriesSize = iOutResult;
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
                    bool bOutResult;
                    if (Boolean.TryParse(nodeLoggerLogToFileEnabled.InnerText, out bOutResult))
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
                    int iOutResult;
                    if (Int32.TryParse(nodeStoredLogFiles.InnerText, out iOutResult))
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
                    bool bOutResult;
                    if (Boolean.TryParse(nodeLoggerCleanUpAtStartUpEnabled.InnerText, out bOutResult))
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
                    int iOutResult;
                    if (Int32.TryParse(nodeLogComponents.InnerText, out iOutResult))
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
                    int iOutResult;
                    if (Int32.TryParse(nodeLogLevels.InnerText, out iOutResult))
                        LoggerStateLevel = iOutResult;
                    else
                        loadSettings = false;
                }
                else
                    loadSettings = false;

                #endregion Logger log level

                #region Logger colors

                List<Color> loggerConsoleColorList = new List<Color>() {};
                List<string> loggerConsoleColorListString = new List<string> {};

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
                foreach (string colorName in loggerConsoleColorListString)
                {
                    foreach (string c in Enum.GetNames(typeof(KnownColor)).Where(
                    item => !item.StartsWith("Control")).OrderBy(item => item))
                    {
                        if (c == colorName)
                        {
                            loggerConsoleColorList.Add(Color.FromName(c));
                            break;
                        }
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
                MessageBox.Show("LoadSettings()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage, "Could not load '" + SettingsFileName + @"' file!",
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                // Close reader
                if (ReaderSettings != null)
                    ReaderSettings.Close();
            }
        }

        #endregion Read settings
    }
}