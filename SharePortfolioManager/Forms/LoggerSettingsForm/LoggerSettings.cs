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
//#define DEBUG_LOGGER_SETTINGS

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.OwnMessageBoxForm;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager.LoggerSettingsForm
{
    public partial class FrmLoggerSettings : Form
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
        public FrmLoggerSettings(FrmMain parentWindow, Logger logger, Language xmlLanguage, string strLanguage)
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
        private void FrmLoggerSettings_FormClosing(object sender, FormClosingEventArgs e)
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
        private void FrmLoggerSettings_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Caption", SettingsConfiguration.LanguageName);

                grpBoxGUIEntries.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogGUIEntriesSize/Caption", SettingsConfiguration.LanguageName);
                lblGUIEntriesSize.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogGUIEntriesSize/Labels/GUIEntriesSize", SettingsConfiguration.LanguageName);

                grpBoxEnableFileLogging.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxFileLogging/Caption", SettingsConfiguration.LanguageName);
                chkBoxEnableFileLogging.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxFileLogging/States/EnableFileLogging", SettingsConfiguration.LanguageName);

                grpBoxStoredLogFiles.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxStoredFiles/Caption", SettingsConfiguration.LanguageName);
                lblStoredLogFiles.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxStoredFiles/Labels/StoredLogFiles", SettingsConfiguration.LanguageName);
                btnLogFileCleanUp.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/CleanUp", SettingsConfiguration.LanguageName);

                grpBoxCleanUpAtStartUp.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxCleanUpAtStartUp/Caption", SettingsConfiguration.LanguageName);
                chkBoxEnableCleanUpAtStartUp.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxCleanUpAtStartUp/States/EnableCleanUpAtStartUp", SettingsConfiguration.LanguageName);

                grpBoxComponents.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogComponents/Caption", SettingsConfiguration.LanguageName);
                chkBoxApplication.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", SettingsConfiguration.LanguageName);
                chkBoxParser.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Parser", SettingsConfiguration.LanguageName);
                chkBoxLanguageHander.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", SettingsConfiguration.LanguageName);

                grpBoxLogLevel.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogLevels/Caption", SettingsConfiguration.LanguageName);
                chkBoxStart.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Start", SettingsConfiguration.LanguageName);
                chkBoxInfo.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Info", SettingsConfiguration.LanguageName);
                chkBoxWarning.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Warning", SettingsConfiguration.LanguageName);
                chkBoxError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Error", SettingsConfiguration.LanguageName);
                chkBoxFatalError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", SettingsConfiguration.LanguageName);

                grpBoxLogColors.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogLevelColors/Caption", SettingsConfiguration.LanguageName);
                lblColorStart.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Start", SettingsConfiguration.LanguageName);
                lblColorInfo.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Info", SettingsConfiguration.LanguageName);
                lblColorWarning.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Warning", SettingsConfiguration.LanguageName);
                lblColorError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Error", SettingsConfiguration.LanguageName);
                lblColorFatalError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", SettingsConfiguration.LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/Save", SettingsConfiguration.LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/Cancel", SettingsConfiguration.LanguageName);

                #endregion Language configuration

                #region GUI log entries

                var iIndexGui = cbxGUIEntriesList.FindStringExact(SettingsConfiguration.LoggerGuiEntriesSize.ToString());
                cbxGUIEntriesList.SelectedIndex = iIndexGui;

                #endregion GUI log entries

                #region Logging to file

                bool.TryParse(SettingsConfiguration.LoggerLogToFileEnabled.ToString(), out var bFlagLoggingToFile);
                chkBoxEnableFileLogging.Checked = bFlagLoggingToFile;

                #endregion Logging to file

                #region Stored log files

                var iIndexStored = cbxStoredLogFiles.FindStringExact(SettingsConfiguration.LoggerStoredLogFiles.ToString());
                cbxStoredLogFiles.SelectedIndex = iIndexStored;

                #endregion Stored log files

                #region Logging to file

                bool.TryParse(SettingsConfiguration.LoggerLogCleanUpAtStartUpEnabled.ToString(), out var bFlagCleanUpAtStartUp);
                chkBoxEnableCleanUpAtStartUp.Checked = bFlagCleanUpAtStartUp;

                #endregion Logging to file

                #region Log components

                chkBoxApplication.Checked = (SettingsConfiguration.LoggerComponentLevel & 1) == 1;

                chkBoxParser.Checked = (SettingsConfiguration.LoggerComponentLevel & 2) == 2;

                chkBoxLanguageHander.Checked = (SettingsConfiguration.LoggerComponentLevel & 4) == 4;

                #endregion Log components

                #region Log state levels

                chkBoxStart.Checked = (SettingsConfiguration.LoggerStateLevel & 1) == 1;

                chkBoxInfo.Checked = (SettingsConfiguration.LoggerStateLevel & 2) == 2;

                chkBoxWarning.Checked = (SettingsConfiguration.LoggerStateLevel & 4) == 4;

                chkBoxError.Checked = (SettingsConfiguration.LoggerStateLevel & 8) == 8;

                chkBoxFatalError.Checked = (SettingsConfiguration.LoggerStateLevel & 16) == 16;

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
                    Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/LoggerSettingsShowFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
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
                        Language.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesSuccessful", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.Black, Logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
                if (Logger.LoggerState == Logger.ELoggerState.CleanUpLogFilesFailed)
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesFailed", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
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
                    message, Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.Error, (int) FrmMain.EComponentLevels.Application,
                    loggerException);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/LogFileCleanUpFailed", SettingsConfiguration.LanguageName), Language, SettingsConfiguration.LanguageName,
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
            if (SettingsConfiguration.XmlDocument != null)
            {
                try
                {
                    #region GUI entries size
                    
                    int.TryParse(cbxGUIEntriesList.SelectedItem.ToString(), out var iGuiEntriesSize);
                    
                    // Set GUI entries size value to FrmMain member variable
                    SettingsConfiguration.LoggerGuiEntriesSize = iGuiEntriesSize;

                    #endregion GUI entries size

                    #region Write to file flag

                    // Set GUI entries size value to FrmMain member variable
                    SettingsConfiguration.LoggerLogToFileEnabled = chkBoxEnableFileLogging.Checked;
                    
                    #endregion Write to file flag

                    #region Stored log files

                    int.TryParse(cbxStoredLogFiles.SelectedItem.ToString(), out var iStoredLogFiles);
                    
                    // Set GUI entries size value to FrmMain member variable
                    SettingsConfiguration.LoggerStoredLogFiles = iStoredLogFiles;

                    #endregion Stored log files

                    #region Cleanup log files at startup flag

                    // Set GUI entries size value to FrmMain member variable
                    SettingsConfiguration.LoggerLogCleanUpAtStartUpEnabled = chkBoxEnableCleanUpAtStartUp.Checked;
                    
                    #endregion Cleanup log files at startup flag

                    #region Component levels

                    // Calculate log components value
                    var iLogComponents = 0;
                    if (chkBoxApplication.Checked)
                        iLogComponents += 1;
                    if (chkBoxParser.Checked)
                        iLogComponents += 2;
                    if (chkBoxLanguageHander.Checked)
                        iLogComponents += 4;

                    // Set calculated log components value to FrmMain member variable
                    SettingsConfiguration.LoggerComponentLevel = iLogComponents;

                    #endregion Component levels

                    #region State levels

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
                    SettingsConfiguration.LoggerStateLevel = iLogStateLevel;

                    #endregion State levels

                    if (!SettingsConfiguration.SaveSettingsConfiguration())
                    {
                        Helper.AddStatusMessage(toolStripStatusLabelMessage,
                            Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed",
                                LanguageName),
                            Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                            (int) FrmMain.EComponentLevels.Application,
                            SettingsConfiguration.LastException);
                    }
                    else
                    {
                        // Show own message box that a restart of the software should be done
                        var ombReboot = new OwnMessageBox(Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                                (int)EOwnMessageBoxInfoType.Info],
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/LoggerSettingsReboot", SettingsConfiguration.LanguageName),
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", SettingsConfiguration.LanguageName),
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", SettingsConfiguration.LanguageName),
                            EOwnMessageBoxInfoType.Info);
                        ombReboot.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    StopFomClosingFlag = true;

                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                        (int) FrmMain.EComponentLevels.Application,
                        ex);
                }
            }
            else
            {
                StopFomClosingFlag = true;

                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
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
