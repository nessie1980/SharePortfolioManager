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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class FrmLoggerSettings : Form
    {
        #region Variables

        /// <summary>
        /// Stores the parent window
        /// </summary>
        private FrmMain _parentWindow;

        /// <summary>
        /// Stores the logger
        /// </summary>
        private Logger _logger;

        /// <summary>
        /// Stores the language file
        /// </summary>
        private Language _language;

        /// <summary>
        /// Stores language
        /// </summary>
        private String _languageName;

        /// <summary>
        /// Flag if the form should be closed
        /// </summary>
        private bool _stopFomClosing;

        #endregion Variables

        #region Properties

        public FrmMain ParentWindow
        {
            get { return _parentWindow; }
            set { _parentWindow = value; }
        }

        public Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public Language Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string LanguageName
        {
            get { return _languageName; }
            set { _languageName = value; }
        }

        public bool StopFomClosingFlag
        {
            get { return _stopFomClosing; }
            set { _stopFomClosing = value; }
        }

        #endregion Properties

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        public FrmLoggerSettings(FrmMain parentWindow, Logger logger, Language xmlLanguage, String strLanguage)
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

                Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Caption", LanguageName);

                grpBoxGUIEntries.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogGUIEntriesSize/Caption", LanguageName);
                lblGUIEntriesSize.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogGUIEntriesSize/Labels/GUIEntriesSize", LanguageName);

                grpBoxEnableFileLogging.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxFileLogging/Caption", LanguageName);
                chkBoxEnableFileLogging.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxFileLogging/States/EnableFileLogging", LanguageName);

                grpBoxStoredLogFiles.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxStoredFiles/Caption", LanguageName);
                lblStoredLogFiles.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxStoredFiles/Labels/StoredLogFiles", LanguageName);
                btnLogFileCleanUp.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/CleanUp", LanguageName);

                grpBoxCleanUpAtStartUp.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxCleanUpAtStartUp/Caption", LanguageName);
                chkBoxEnableCleanUpAtStartUp.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxCleanUpAtStartUp/States/EnableCleanUpAtStartUp", LanguageName);

                grpBoxComponents.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogComponents/Caption", LanguageName);
                chkBoxApplication.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", LanguageName);
                chkBoxWebParser.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/WebParser", LanguageName);
                chkBoxLanguageHander.Text = Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", LanguageName);

                grpBoxLogLevel.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogLevels/Caption", LanguageName);
                chkBoxStart.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Start", LanguageName);
                chkBoxInfo.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Info", LanguageName);
                chkBoxWarning.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Warning", LanguageName);
                chkBoxError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Error", LanguageName);
                chkBoxFatalError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", LanguageName);

                grpBoxLogColors.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogLevelColors/Caption", LanguageName);
                lblColorStart.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Start", LanguageName);
                lblColorInfo.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Info", LanguageName);
                lblColorWarning.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Warning", LanguageName);
                lblColorError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/Error", LanguageName);
                lblColorFatalError.Text = Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/Save", LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/Cancel", LanguageName);

                #endregion Language configuration

                #region GUI log entries

                int iIndexGUI = cbxGUIEntriesList.FindStringExact(ParentWindow.LoggerGUIEntriesSize.ToString());
                cbxGUIEntriesList.SelectedIndex = iIndexGUI;

                #endregion GUI log entries

                #region Logging to file

                bool bFlagLoggingToFile = false;
                Boolean.TryParse(ParentWindow.LoggerLogToFileEnabled.ToString(), out bFlagLoggingToFile);
                chkBoxEnableFileLogging.Checked = bFlagLoggingToFile;

                #endregion Logging to file

                #region Stored log files

                int iIndexStored = cbxStoredLogFiles.FindStringExact(ParentWindow.LoggerStoredLogFiles.ToString());
                cbxStoredLogFiles.SelectedIndex = iIndexStored;

                #endregion Stored log files

                #region Logging to file

                bool bFlagCleanUpAtStartUp = false;
                Boolean.TryParse(ParentWindow.LoggerLogCleanUpAtStartUpEnabled.ToString(), out bFlagCleanUpAtStartUp);
                chkBoxEnableCleanUpAtStartUp.Checked = bFlagCleanUpAtStartUp;

                #endregion Logging to file

                #region Log components

                if ((ParentWindow.LoggerComponentLevel & 1) == 1)
                    chkBoxApplication.Checked = true;
                else
                    chkBoxApplication.Checked = false;

                if ((ParentWindow.LoggerComponentLevel & 2) == 2)
                    chkBoxWebParser.Checked = true;
                else
                    chkBoxWebParser.Checked = false;

                if ((ParentWindow.LoggerComponentLevel & 4) == 4)
                    chkBoxLanguageHander.Checked = true;
                else
                    chkBoxLanguageHander.Checked = false;

                #endregion Log components

                #region Log state levels

                if ((ParentWindow.LoggerStateLevel & 1) == 1)
                    chkBoxStart.Checked = true;
                else
                    chkBoxStart.Checked = false;

                if ((ParentWindow.LoggerStateLevel & 2) == 2)
                    chkBoxInfo.Checked = true;
                else
                    chkBoxInfo.Checked = false;

                if ((ParentWindow.LoggerStateLevel & 4) == 4)
                    chkBoxWarning.Checked = true;
                else
                    chkBoxWarning.Checked = false;

                if ((ParentWindow.LoggerStateLevel & 8) == 8)
                    chkBoxError.Checked = true;
                else
                    chkBoxError.Checked = false;

                if ((ParentWindow.LoggerStateLevel & 16) == 16)
                    chkBoxFatalError.Checked = true;
                else
                    chkBoxFatalError.Checked = false;

                #endregion Log state levels

                #region Color configuration

                Size cbxSize = new Size(172, 20);
                int cbxDropDownHeight = 125;
                int iLocationX = 148;
                int iLocationYAdd = 30;

                // ComboBox for the start color
                ComboBoxCustom cbxColorStart = new ComboBoxCustom();
                cbxColorStart.Location = new Point(iLocationX, 21 + (iLocationYAdd * 0));
                cbxColorStart.Size = cbxSize;
                cbxColorStart.DropDownHeight = cbxDropDownHeight;
                cbxColorStart.Show();
                grpBoxLogColors.Controls.Add(cbxColorStart);

                // ComboBox for the info color
                ComboBoxCustom cbxColorInfo = new ComboBoxCustom();
                cbxColorInfo.Location = new Point(iLocationX, 21 + (iLocationYAdd * 1));
                cbxColorInfo.Size = cbxSize;
                cbxColorInfo.DropDownHeight = cbxDropDownHeight;
                grpBoxLogColors.Controls.Add(cbxColorInfo);

                // ComboBox for the warning color
                ComboBoxCustom cbxColorWarning = new ComboBoxCustom();
                cbxColorWarning.Location = new Point(iLocationX, 21 + (iLocationYAdd * 2));
                cbxColorWarning.Size = cbxSize;
                cbxColorWarning.DropDownHeight = cbxDropDownHeight;
                grpBoxLogColors.Controls.Add(cbxColorWarning);

                // ComboBox for the error color
                ComboBoxCustom cbxColorError = new ComboBoxCustom();
                cbxColorError.Location = new Point(iLocationX, 21 + (iLocationYAdd * 3));
                cbxColorError.Size = cbxSize;
                cbxColorError.DropDownHeight = cbxDropDownHeight;
                grpBoxLogColors.Controls.Add(cbxColorError);

                // ComboBox for the fatal error color
                ComboBoxCustom cboBoxColorFatalError = new ComboBoxCustom();
                cboBoxColorFatalError.Location = new Point(iLocationX, 21 + (iLocationYAdd * 4));
                cboBoxColorFatalError.Size = cbxSize;
                cboBoxColorFatalError.DropDownHeight = cbxDropDownHeight;
                grpBoxLogColors.Controls.Add(cboBoxColorFatalError);

                // Get enum strings and order them by name.
                // Remove Control colors.
                int iIndex = 0;
                foreach (string c in Enum.GetNames(typeof(KnownColor)).Where(
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
#if DEBUG
                MessageBox.Show("FrmLoggerSettings_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/LoggerSettingsShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                int iStoredLogFiles = 0;
                int iDeletedFiles = 0;
                Int32.TryParse(cbxStoredLogFiles.SelectedItem.ToString(), out iStoredLogFiles);
                iDeletedFiles = Logger.CleanUpLogFiles(iStoredLogFiles);

                Helper.AddStatusMessage(toolStripStatusLabel1,
                    Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized", LanguageName),
                    Language, LanguageName,
                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
            }
            catch (LoggerException loggerException) // TODO
            {
#if DEBUG
                MessageBox.Show(loggerException.Message, @"Error - FrmMain()", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                //// Check the logger initialization state
                //switch (Logger.InitState)
                //{
                //    case Logger.EInitState.NotInitialized:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.WrongSize:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WrongSize", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.ComponentLevelInvalid:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentLevelInvalid", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.ComponentNamesMaxCount:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentNamesMaxCount", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.StateLevelInvalid:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StateLevelInvalid", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.StatesMaxCount:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StatesMaxCount", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.ColorsMaxCount:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ColorsMaxCount", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.WriteStartupFailed:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WriteStartupFailed", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.LogPathCreationFailed:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/LogPathCreationFailed", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.InitializationFailed:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, Language.GetLanguageTextByXPath(@"/Logger/LoggerErrors/InitializationFailed", LanguageName), Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //}
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
                    var nodeLogGUIEntriesSize = ParentWindow.Settings.SelectSingleNode("/Settings/Logger/GUIEntries");

                    if (nodeLogGUIEntriesSize != null)
                    {
                        int iGUIEntriesSize = 0;
                        Int32.TryParse(cbxGUIEntriesList.SelectedItem.ToString(), out iGUIEntriesSize);
                        // Set GUI entries size value to FrmMain member variable
                        ParentWindow.LoggerGUIEntriesSize = iGUIEntriesSize;
                        // Set GUI entries size value to the XML
                        nodeLogGUIEntriesSize.InnerXml = iGUIEntriesSize.ToString();
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
                        int iStoredLogFiles = 0;
                        Int32.TryParse(cbxStoredLogFiles.SelectedItem.ToString(), out iStoredLogFiles);
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
                        int iLogComponents = 0;
                        if (chkBoxApplication.Checked)
                            iLogComponents += 1;
                        if (chkBoxWebParser.Checked)
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
                        int iLogStateLevel = 0;
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
                    OwnMessageBox ombReboot = new OwnMessageBox(Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName),
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/LoggerSetttingsReboot", LanguageName),
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName),
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName));
                    ombReboot.ShowDialog();
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("btnSave_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    StopFomClosingFlag = true;

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", LanguageName),
                       Language, LanguageName,
                       Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
                }
            }
            else
            {
                StopFomClosingFlag = true;

                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", LanguageName),
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
                this.DrawMode = DrawMode.OwnerDrawFixed;
            }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                base.OnDrawItem(e);
                if (e.Index < 0) { return; }

                //Create A Rectangle To Fit New Item
                Rectangle ColourSize = new Rectangle(0, e.Bounds.Top,
                   e.Bounds.Width, e.Bounds.Height);

//                e.DrawBackground();
                ComboBoxItem item = (ComboBoxItem)this.Items[e.Index];
                    Brush brush = new SolidBrush(item.ForeColor);

                e.Graphics.FillRectangle(brush, ColourSize);

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                { brush = Brushes.Yellow; }
                e.Graphics.DrawString(item.Text,
                    this.Font, brush, e.Bounds.X, e.Bounds.Y);
            }
            object selectedValue = null;
            public new Object SelectedValue
            {
                get
                {
                    object ret = null;
                    if (this.SelectedIndex >= 0)
                    {
                        ret = ((ComboBoxItem)this.SelectedItem).Value;
                    }
                    return ret;
                }
                set { selectedValue = value; }
            }
            string selectedText = "";
            public new String SelectedText
            {
                get
                {
                    return ((ComboBoxItem)this.SelectedItem).Text;
                }
                set { selectedText = value; }
            }
        }

        public class ComboBoxItem
        {
            public ComboBoxItem() { }

            public ComboBoxItem(string pText, object pValue)
            {
                text = pText; val = pValue;
            }

            public ComboBoxItem(string pText, object pValue, Color pColor)
            {
                text = pText; val = pValue; foreColor = pColor;
            }

            string text = "";
            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            object val;
            public object Value
            {
                get { return val; }
                set { val = value; }
            }

            Color foreColor = Color.Black;
            public Color ForeColor
            {
                get { return foreColor; }
                set { foreColor = value; }
            }

            public override string ToString()
            {
                return text;
            }
        }

        #endregion Costum ComboBox
    }
}
