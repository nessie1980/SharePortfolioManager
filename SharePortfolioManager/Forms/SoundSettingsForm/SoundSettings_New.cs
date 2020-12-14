﻿//MIT License
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
//#define DEBUG_LOGGER_SETTINGS

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.OwnMessageBoxForm;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager.SoundSettingsForm
{
    public partial class FrmSoundSettings : Form
    {
        #region Properties

        public FrmMain ParentWindow { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

        public bool StopFomClosingFlag { get; set; }

        #endregion Properties

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmSoundSettings(FrmMain parentWindow, Logger logger, Language xmlLanguage, String strLanguage)
        {
            InitializeComponent();

            ParentWindow = parentWindow;
            Logger = logger;
            Language = xmlLanguage;
            LanguageName = strLanguage;
            StopFomClosingFlag = false;
        }

        /// <summary>
        /// This function is called when the form is closing
        /// Here we check if the form really should be closed
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void FrmSoundSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the form should be closed
            if (StopFomClosingFlag)
            {
                // Stop closing the form
                e.Cancel = true;
            }

            // Reset closing flag
            StopFomClosingFlag = false;
        }

        /// <summary>
        /// This function loads the logger settings value to the form
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">EventArgs</param>
        private void FrmSoundSettings_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Caption", LanguageName);

                grpBoxGUIEntries.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxLogGUIEntriesSize/Caption", LanguageName);
                lblGUIEntriesSize.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxLogGUIEntriesSize/Labels/GUIEntriesSize", LanguageName);

                grpBoxEnableFileLogging.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxFileLogging/Caption", LanguageName);
                chkBoxEnableFileLogging.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxFileLogging/States/EnableFileLogging", LanguageName);

                grpBoxStoredLogFiles.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxStoredFiles/Caption", LanguageName);
                lblStoredLogFiles.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxStoredFiles/Labels/StoredLogFiles", LanguageName);
                btnLogFileCleanUp.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Buttons/CleanUp", LanguageName);

                grpBoxCleanUpAtStartUp.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxCleanUpAtStartUp/Caption", LanguageName);
                chkBoxEnableCleanUpAtStartUp.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxCleanUpAtStartUp/States/EnableCleanUpAtStartUp", LanguageName);

                grpBoxComponents.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxLogComponents/Caption", LanguageName);
                chkBoxApplication.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", LanguageName);
                chkBoxParser.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Parser", LanguageName);
                chkBoxLanguageHander.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", LanguageName);

                grpBoxLogLevel.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxLogLevels/Caption", LanguageName);
                chkBoxStart.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Start", LanguageName);
                chkBoxInfo.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Info", LanguageName);
                chkBoxWarning.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Warning", LanguageName);
                chkBoxError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Error", LanguageName);
                chkBoxFatalError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", LanguageName);

                grpBoxLogColors.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxLogLevelColors/Caption", LanguageName);
                lblColorStart.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Start", LanguageName);
                lblColorInfo.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Info", LanguageName);
                lblColorWarning.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Warning", LanguageName);
                lblColorError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Error", LanguageName);
                lblColorFatalError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Buttons/Save", LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Buttons/Cancel", LanguageName);

                #endregion Language configuration

                #region GUI log entries

                var iIndexGui = cbxGUIEntriesList.FindStringExact(ParentWindow.LoggerGuiEntriesSize.ToString());
                cbxGUIEntriesList.SelectedIndex = iIndexGui;

                #endregion GUI log entries

                #region Logging to file

                bool.TryParse(ParentWindow.LoggerLogToFileEnabled.ToString(), out var bFlagLoggingToFile);
                chkBoxEnableFileLogging.Checked = bFlagLoggingToFile;

                #endregion Logging to file

                #region Stored log files

                var iIndexStored = cbxStoredLogFiles.FindStringExact(ParentWindow.LoggerStoredLogFiles.ToString());
                cbxStoredLogFiles.SelectedIndex = iIndexStored;

                #endregion Stored log files

                #region Logging to file

                bool.TryParse(ParentWindow.LoggerLogCleanUpAtStartUpEnabled.ToString(), out var bFlagCleanUpAtStartUp);
                chkBoxEnableCleanUpAtStartUp.Checked = bFlagCleanUpAtStartUp;

                #endregion Logging to file

                #region Log components

                chkBoxApplication.Checked = (ParentWindow.LoggerComponentLevel & 1) == 1;

                chkBoxParser.Checked = (ParentWindow.LoggerComponentLevel & 2) == 2;

                chkBoxLanguageHander.Checked = (ParentWindow.LoggerComponentLevel & 4) == 4;

                #endregion Log components

                #region Log state levels

                chkBoxStart.Checked = (ParentWindow.LoggerStateLevel & 1) == 1;

                chkBoxInfo.Checked = (ParentWindow.LoggerStateLevel & 2) == 2;

                chkBoxWarning.Checked = (ParentWindow.LoggerStateLevel & 4) == 4;

                chkBoxError.Checked = (ParentWindow.LoggerStateLevel & 8) == 8;

                chkBoxFatalError.Checked = (ParentWindow.LoggerStateLevel & 16) == 16;

                #endregion Log state levels

                #region Color configuration

                var cbxSize = new Size(172, 20);
                const int cbxDropDownHeight = 125;
                const int iLocationX = 148;
                const int iLocationYAdd = 29;

                // ComboBox for the start color
                var cbxColorStart = new ComboBoxCustom
                {
                    Location = new Point(iLocationX, 21 + (iLocationYAdd * 0)),
                    Size = cbxSize,
                    DropDownHeight = cbxDropDownHeight
                };
                cbxColorStart.Show();
                grpBoxLogColors.Controls.Add(cbxColorStart);

                // ComboBox for the info color
                var cbxColorInfo = new ComboBoxCustom
                {
                    Location = new Point(iLocationX, 21 + (iLocationYAdd * 1)),
                    Size = cbxSize,
                    DropDownHeight = cbxDropDownHeight
                };
                grpBoxLogColors.Controls.Add(cbxColorInfo);

                // ComboBox for the warning color
                var cbxColorWarning = new ComboBoxCustom
                {
                    Location = new Point(iLocationX, 21 + (iLocationYAdd * 2)),
                    Size = cbxSize,
                    DropDownHeight = cbxDropDownHeight
                };
                grpBoxLogColors.Controls.Add(cbxColorWarning);

                // ComboBox for the error color
                var cbxColorError = new ComboBoxCustom
                {
                    Location = new Point(iLocationX, 21 + (iLocationYAdd * 3)),
                    Size = cbxSize,
                    DropDownHeight = cbxDropDownHeight
                };
                grpBoxLogColors.Controls.Add(cbxColorError);

                // ComboBox for the fatal error color
                var cboBoxColorFatalError =
                    new ComboBoxCustom
                    {
                        Location = new Point(iLocationX, 21 + (iLocationYAdd * 4)),
                        Size = cbxSize,
                        DropDownHeight = cbxDropDownHeight
                    };
                grpBoxLogColors.Controls.Add(cboBoxColorFatalError);

                // Get enums strings and order them by name.
                // Remove Control colors.
                var iIndex = 0;
                foreach (var c in Enum.GetNames(typeof(KnownColor)).Where(
                    item => !item.StartsWith("Control") ).OrderBy(item => item))
                {
                    cbxColorStart.Items.Add(new ComboBoxItem(c, iIndex, Color.FromName(c)));
                    cbxColorInfo.Items.Add(new ComboBoxItem(c, iIndex, Color.FromName(c)));
                    cbxColorWarning.Items.Add(new ComboBoxItem(c, iIndex, Color.FromName(c)));
                    cbxColorError.Items.Add(new ComboBoxItem(c, iIndex, Color.FromName(c)));
                    cboBoxColorFatalError.Items.Add(new ComboBoxItem(c, iIndex, Color.FromName(c)));

                    iIndex++;
                }

                cbxColorStart.SelectedIndex = cbxColorStart.FindStringExact(Logger.LoggerColorList[0].Name);
                cbxColorInfo.SelectedIndex = cbxColorInfo.FindStringExact(Logger.LoggerColorList[1].Name);
                cbxColorWarning.SelectedIndex = cbxColorWarning.FindStringExact(Logger.LoggerColorList[2].Name);
                cbxColorError.SelectedIndex = cbxColorError.FindStringExact(Logger.LoggerColorList[3].Name);
                cboBoxColorFatalError.SelectedIndex = cboBoxColorFatalError.FindStringExact(Logger.LoggerColorList[4].Name);

                #endregion Color configuration
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/LoggerSettingsShowFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);

            }
        }

        #endregion Form

        #region Buttons

        /// <summary>
        /// This function does a log file clean up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnLogFileCleanUp_Click(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(cbxStoredLogFiles.SelectedItem.ToString(), out var iStoredLogFiles);
                Logger.CleanUpLogFiles(iStoredLogFiles);

                if(Logger.LoggerState == Logger.ELoggerState.CleanUpLogFilesSuccessful)
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesSuccessful", LanguageName),
                        Language, LanguageName,
                        Color.Black, Logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
                if (Logger.LoggerState == Logger.ELoggerState.CleanUpLogFilesFailed)
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesFailed", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
            }
            catch (LoggerException loggerException)
            {
                var message = @"invalid";

                // Check the logger initialization state
                switch (Logger.InitState)
                    {
                    case Logger.EInitState.NotInitialized:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized",
                            LanguageName);
                        break;
                    case Logger.EInitState.WrongSize:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WrongSize",
                            LanguageName);
                        break;
                    case Logger.EInitState.ComponentLevelInvalid:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentLevelInvalid",
                            LanguageName);
                        break;
                    case Logger.EInitState.ComponentNamesMaxCount:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentNamesMaxCount",
                            LanguageName);
                        break;
                    case Logger.EInitState.StateLevelInvalid:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StateLevelInvalid",
                            LanguageName);
                        break;
                    case Logger.EInitState.StatesMaxCount:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StatesMaxCount",
                            LanguageName);
                        break;
                    case Logger.EInitState.ColorsMaxCount:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ColorsMaxCount",
                            LanguageName);
                        break;
                    case Logger.EInitState.WriteStartupFailed:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WriteStartupFailed",
                            LanguageName);
                        break;
                    case Logger.EInitState.LogPathCreationFailed:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/LogPathCreationFailed",
                            LanguageName);
                        break;
                    case Logger.EInitState.InitializationFailed:
                        message = Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/InitializationFailed",
                            LanguageName);
                        break;
                }

                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    message, Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.Error, (int) FrmMain.EComponentLevels.Application,
                    loggerException);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/LogFileCleanUpFailed", LanguageName), Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function close the form
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            StopFomClosingFlag = false;
            Close();
        }

        /// <summary>
        /// This function save the changed settings and closes the dialog
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnSave_Click(object sender, EventArgs e)
        {
            // Set window start position and window size
            if (ParentWindow.Settings != null)
            {
                try
                {
                    #region GUI entries size

                    // Save logger GUI entries size
                    var nodeLogGuiEntriesSize = ParentWindow.Settings.SelectSingleNode("/Settings/Logger/GUIEntries");

                    if (nodeLogGuiEntriesSize != null)
                    {
                        int.TryParse(cbxGUIEntriesList.SelectedItem.ToString(), out var iGuiEntriesSize);
                        // Set GUI entries size value to FrmMain member variable
                        ParentWindow.LoggerGuiEntriesSize = iGuiEntriesSize;
                        // Set GUI entries size value to the XML
                        nodeLogGuiEntriesSize.InnerXml = iGuiEntriesSize.ToString();
                    }

                    #endregion GUI entries size

                    #region Write to file flag

                    // Save logger flag write to file
                    var nodeLogFlagWriteToFile = ParentWindow.Settings.SelectSingleNode("/Settings/Logger/LogToFileEnable");

                    if (nodeLogFlagWriteToFile != null)
                    {
                        // Set GUI entries size value to FrmMain member variable
                        ParentWindow.LoggerLogToFileEnabled = chkBoxEnableFileLogging.Checked;
                        // Set GUI entries size value to the XML
                        nodeLogFlagWriteToFile.InnerXml = chkBoxEnableFileLogging.Checked.ToString();
                    }

                    #endregion Write to file flag

                    #region Stored log files

                    // Save logger stored log files
                    var nodeLogStoredLogFiles = ParentWindow.Settings.SelectSingleNode("/Settings/Logger/StoredLogFiles");

                    if (nodeLogStoredLogFiles != null)
                    {
                        int.TryParse(cbxStoredLogFiles.SelectedItem.ToString(), out var iStoredLogFiles);
                        // Set GUI entries size value to FrmMain member variable
                        ParentWindow.LoggerStoredLogFiles = iStoredLogFiles;
                        // Set GUI entries size value to the XML
                        nodeLogStoredLogFiles.InnerXml = iStoredLogFiles.ToString();
                    }

                    #endregion Stored log files

                    #region Cleanup log files at startup flag

                    // Save logger flag write to file
                    var nodeLogFlagCleanUpAtStartUp = ParentWindow.Settings.SelectSingleNode("/Settings/Logger/LogCleanUpAtStartUpEnable");

                    if (nodeLogFlagCleanUpAtStartUp != null)
                    {
                        // Set GUI entries size value to FrmMain member variable
                        ParentWindow.LoggerLogCleanUpAtStartUpEnabled = chkBoxEnableCleanUpAtStartUp.Checked;
                        // Set GUI entries size value to the XML
                        nodeLogFlagCleanUpAtStartUp.InnerXml = chkBoxEnableCleanUpAtStartUp.Checked.ToString();
                    }

                    #endregion Cleanup log files at startup flag

                    #region Component levels

                    // Save logger component level 
                    var nodeLogComponents = ParentWindow.Settings.SelectSingleNode("/Settings/Logger/LogComponents");

                    if (nodeLogComponents != null)
                    {
                        // Calculate log components value
                        var iLogComponents = 0;
                        if (chkBoxApplication.Checked)
                            iLogComponents += 1;
                        if (chkBoxParser.Checked)
                            iLogComponents += 2;
                        if (chkBoxLanguageHander.Checked)
                            iLogComponents += 4;

                        // Set calculated log components value to FrmMain member variable
                        ParentWindow.LoggerComponentLevel = iLogComponents;
                        // Set calculated log components value to the XML
                        nodeLogComponents.InnerXml = iLogComponents.ToString();
                    }

                    #endregion Component levels

                    #region State levels

                    // Save logger log state level
                    var nodeLogStateLevels = ParentWindow.Settings.SelectSingleNode("/Settings/Logger/LogLevels");

                    if (nodeLogStateLevels != null)
                    {
                        // Calculate log components value
                        var iLogStateLevel = 0;
                        if (chkBoxStart.Checked)
                            iLogStateLevel += 1;
                        if (chkBoxInfo.Checked)
                            iLogStateLevel += 2;
                        if (chkBoxWarning.Checked)
                            iLogStateLevel += 4;
                        if (chkBoxError.Checked)
                            iLogStateLevel += 8;
                        if (chkBoxFatalError.Checked)
                            iLogStateLevel += 16;

                        // Set calculated log components value to FrmMain member variable
                        ParentWindow.LoggerStateLevel = iLogStateLevel;
                        // Set calculated log components value to the XML
                        nodeLogStateLevels.InnerXml = iLogStateLevel.ToString();
                    }

                    #endregion State levels

                    // Show own message box that a restart of the software should be done
                    var ombReboot = new OwnMessageBox(Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", LanguageName)[
                        (int)EOwnMessageBoxInfoType.Info],
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/LoggerSettingsReboot", LanguageName),
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName),
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName));
                    ombReboot.ShowDialog();
                }
                catch (Exception ex)
                {
                    StopFomClosingFlag = true;

                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                        (int) FrmMain.EComponentLevels.Application,
                        ex);
                }
            }
            else
            {
                StopFomClosingFlag = true;

                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);               
            }
        }

        #endregion Buttons

        #region Custom ComboBox

        public class ComboBoxCustom : ComboBox
        {
            public ComboBoxCustom()
            {
                DrawMode = DrawMode.OwnerDrawFixed;
            }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                base.OnDrawItem(e);
                if (e.Index < 0) { return; }

                //Create A Rectangle To Fit New Item
                var colorSize = new Rectangle(0, e.Bounds.Top,
                   e.Bounds.Width, e.Bounds.Height);

                var item = (ComboBoxItem)Items[e.Index];
                    Brush brush = new SolidBrush(item.ForeColor);

                e.Graphics.FillRectangle(brush, colorSize);

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                { brush = Brushes.Yellow; }
                e.Graphics.DrawString(item.Text,
                    Font, brush, e.Bounds.X, e.Bounds.Y);
            }

            public new object SelectedValue
            {
                get
                {
                    object ret = null;
                    if (SelectedIndex >= 0)
                    {
                        ret = ((ComboBoxItem)SelectedItem).Value;
                    }
                    return ret;
                }
            }

            public new string SelectedText => ((ComboBoxItem)SelectedItem).Text;
        }

        public class ComboBoxItem
        {
            public ComboBoxItem(string pText, object pValue, Color pColor)
            {
                Text = pText; Value = pValue; ForeColor = pColor;
            }

            public string Text { get; set; }

            public object Value { get; set; }

            public Color ForeColor { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        #endregion Custom ComboBox
    }
}