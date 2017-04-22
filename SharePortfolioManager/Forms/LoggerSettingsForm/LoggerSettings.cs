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
        private Language _xmlLanguage;

        /// <summary>
        /// Stores language
        /// </summary>
        private String _strLanguage;

        /// <summary>
        /// Flag if the form should be closed
        /// </summary>
        private bool _stopFomClosing;

        #endregion Variables

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        public FrmLoggerSettings(FrmMain parentWindow, Logger logger, Language xmlLanguage, String strLanguage)
        {
            InitializeComponent();

            _parentWindow = parentWindow;
            _logger = logger;
            _xmlLanguage = xmlLanguage;
            _strLanguage = strLanguage;
            _stopFomClosing = false;
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
            if (_stopFomClosing)
            {
                // Stop closing the form
                e.Cancel = true;
            }

            // Reset closing flag
            _stopFomClosing = false;
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

                Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Caption", _strLanguage);

                grpBoxGUIEntries.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogGUIEntriesSize/Caption", _strLanguage);
                lblGUIEntriesSize.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogGUIEntriesSize/Labels/GUIEntriesSize", _strLanguage);

                grpBoxEnableFileLogging.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxFileLogging/Caption", _strLanguage);
                chkBoxEnableFileLogging.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxFileLogging/States/EnableFileLogging", _strLanguage);

                grpBoxStoredLogFiles.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxStoredFiles/Caption", _strLanguage);
                lblStoredLogFiles.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxStoredFiles/Labels/StoredLogFiles", _strLanguage);
                btnLogFileCleanUp.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/CleanUp", _strLanguage);

                grpBoxCleanUpAtStartUp.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxCleanUpAtStartUp/Caption", _strLanguage);
                chkBoxEnableCleanUpAtStartUp.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxCleanUpAtStartUp/States/EnableCleanUpAtStartUp", _strLanguage);

                grpBoxComponents.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogComponents/Caption", _strLanguage);
                chkBoxApplication.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", _strLanguage);
                chkBoxWebParser.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/WebParser", _strLanguage);
                chkBoxLanguageHander.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", _strLanguage);

                grpBoxLogLevel.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogLevels/Caption", _strLanguage);
                chkBoxStart.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Start", _strLanguage);
                chkBoxInfo.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Info", _strLanguage);
                chkBoxWarning.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Warning", _strLanguage);
                chkBoxError.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Error", _strLanguage);
                chkBoxFatalError.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/FatalError", _strLanguage);

                grpBoxLogColors.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/GrpBoxLogLevelColors/Caption", _strLanguage);
                lblColorStart.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Start", _strLanguage);
                lblColorInfo.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Info", _strLanguage);
                lblColorWarning.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Warning", _strLanguage);
                lblColorError.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Error", _strLanguage);
                lblColorFatalError.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/FatalError", _strLanguage);

                btnSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/Save", _strLanguage);
                btnCancel.Text = _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Buttons/Cancel", _strLanguage);

                #endregion Language configuration

                #region GUI log entries

                int iIndexGUI = cbxGUIEntriesList.FindStringExact(_parentWindow.LoggerGUIEntriesSize.ToString());
                cbxGUIEntriesList.SelectedIndex = iIndexGUI;

                #endregion GUI log entries

                #region Logging to file

                bool bFlagLoggingToFile = false;
                Boolean.TryParse(_parentWindow.LoggerLogToFileEnabled.ToString(), out bFlagLoggingToFile);
                chkBoxEnableFileLogging.Checked = bFlagLoggingToFile;

                #endregion Logging to file

                #region Stored log files

                int iIndexStored = cbxStoredLogFiles.FindStringExact(_parentWindow.LoggerStoredLogFiles.ToString());
                cbxStoredLogFiles.SelectedIndex = iIndexStored;

                #endregion Stored log files

                #region Logging to file

                bool bFlagCleanUpAtStartUp = false;
                Boolean.TryParse(_parentWindow.LoggerLogCleanUpAtStartUpEnabled.ToString(), out bFlagCleanUpAtStartUp);
                chkBoxEnableCleanUpAtStartUp.Checked = bFlagCleanUpAtStartUp;

                #endregion Logging to file

                #region Log components

                if ((_parentWindow.LoggerComponentLevel & 1) == 1)
                    chkBoxApplication.Checked = true;
                else
                    chkBoxApplication.Checked = false;

                if ((_parentWindow.LoggerComponentLevel & 2) == 2)
                    chkBoxWebParser.Checked = true;
                else
                    chkBoxWebParser.Checked = false;

                if ((_parentWindow.LoggerComponentLevel & 4) == 4)
                    chkBoxLanguageHander.Checked = true;
                else
                    chkBoxLanguageHander.Checked = false;

                #endregion Log components

                #region Log state levels

                if ((_parentWindow.LoggerStateLevel & 1) == 1)
                    chkBoxStart.Checked = true;
                else
                    chkBoxStart.Checked = false;

                if ((_parentWindow.LoggerStateLevel & 2) == 2)
                    chkBoxInfo.Checked = true;
                else
                    chkBoxInfo.Checked = false;

                if ((_parentWindow.LoggerStateLevel & 4) == 4)
                    chkBoxWarning.Checked = true;
                else
                    chkBoxWarning.Checked = false;

                if ((_parentWindow.LoggerStateLevel & 8) == 8)
                    chkBoxError.Checked = true;
                else
                    chkBoxError.Checked = false;

                if ((_parentWindow.LoggerStateLevel & 16) == 16)
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

                cbxColorStart.SelectedIndex = cbxColorStart.FindStringExact(_logger.LoggerColorList[0].Name);
                cbxColorInfo.SelectedIndex = cbxColorInfo.FindStringExact(_logger.LoggerColorList[1].Name);
                cbxColorWarning.SelectedIndex = cbxColorWarning.FindStringExact(_logger.LoggerColorList[2].Name);
                cbxColorError.SelectedIndex = cbxColorError.FindStringExact(_logger.LoggerColorList[3].Name);
                cboBoxColorFatalError.SelectedIndex = cboBoxColorFatalError.FindStringExact(_logger.LoggerColorList[4].Name);

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
                   _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/LoggerSettingsShowFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                iDeletedFiles =  _logger.CleanUpLogFiles(iStoredLogFiles);

                Helper.AddStatusMessage(toolStripStatusLabel1,
                    _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.Black, _logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
            }
            catch (LoggerException loggerException) // TODO
            {
#if DEBUG
                MessageBox.Show(loggerException.Message, @"Error - FrmMain()", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                //// Check the logger initialization state
                //switch (_logger.InitState)
                //{
                //    case Logger.EInitState.NotInitialized:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/NotInitialized", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.WrongSize:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WrongSize", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.ComponentLevelInvalid:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentLevelInvalid", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.ComponentNamesMaxCount:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ComponentNamesMaxCount", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.StateLevelInvalid:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StateLevelInvalid", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.StatesMaxCount:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/StatesMaxCount", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.ColorsMaxCount:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/ColorsMaxCount", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.WriteStartupFailed:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/WriteStartupFailed", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.LogPathCreationFailed:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/LogPathCreationFailed", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                //        break;
                //    case Logger.EInitState.InitializationFailed:
                //        Helper.AddStatusMessage(rchTxtBoxStateMessage, _xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerErrors/InitializationFailed", _languageName), Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
            _stopFomClosing = false;
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
            if (_parentWindow.XmlSettings != null)
            {
                try
                {
                    #region GUI entries size

                    // Save logger GUI entries size
                    var nodeLogGUIEntriesSize = _parentWindow.XmlSettings.SelectSingleNode("/Settings/Logger/GUIEntries");

                    if (nodeLogGUIEntriesSize != null)
                    {
                        int iGUIEntriesSize = 0;
                        Int32.TryParse(cbxGUIEntriesList.SelectedItem.ToString(), out iGUIEntriesSize);
                        // Set GUI entries size value to FrmMain member variable
                        _parentWindow.LoggerGUIEntriesSize = iGUIEntriesSize;
                        // Set GUI entries size value to the XML
                        nodeLogGUIEntriesSize.InnerXml = iGUIEntriesSize.ToString();
                    }

                    #endregion GUI entries size

                    #region Write to file flag

                    // Save logger flag write to file
                    var nodeLogFlagWriteToFile = _parentWindow.XmlSettings.SelectSingleNode("/Settings/Logger/LogToFileEnable");

                    if (nodeLogFlagWriteToFile != null)
                    {
                        // Set GUI entries size value to FrmMain member variable
                        _parentWindow.LoggerLogToFileEnabled = chkBoxEnableFileLogging.Checked;
                        // Set GUI entries size value to the XML
                        nodeLogFlagWriteToFile.InnerXml = chkBoxEnableFileLogging.Checked.ToString();
                    }

                    #endregion Write to file flag

                    #region Stored log files

                    // Save logger stored log files
                    var nodeLogStoredLogFiles = _parentWindow.XmlSettings.SelectSingleNode("/Settings/Logger/StoredLogFiles");

                    if (nodeLogStoredLogFiles != null)
                    {
                        int iStoredLogFiles = 0;
                        Int32.TryParse(cbxStoredLogFiles.SelectedItem.ToString(), out iStoredLogFiles);
                        // Set GUI entries size value to FrmMain member variable
                        _parentWindow.LoggerStoredLogFiles = iStoredLogFiles;
                        // Set GUI entries size value to the XML
                        nodeLogStoredLogFiles.InnerXml = iStoredLogFiles.ToString();
                    }

                    #endregion Stored log files

                    #region Cleanup log files at startup flag

                    // Save logger flag write to file
                    var nodeLogFlagCleanUpAtStartUp = _parentWindow.XmlSettings.SelectSingleNode("/Settings/Logger/LogCleanUpAtStartUpEnable");

                    if (nodeLogFlagCleanUpAtStartUp != null)
                    {
                        // Set GUI entries size value to FrmMain member variable
                        _parentWindow.LoggerLogCleanUpAtStartUpEnabled = chkBoxEnableCleanUpAtStartUp.Checked;
                        // Set GUI entries size value to the XML
                        nodeLogFlagCleanUpAtStartUp.InnerXml = chkBoxEnableCleanUpAtStartUp.Checked.ToString();
                    }

                    #endregion Cleanup log files at startup flag

                    #region Component levels

                    // Save logger component level 
                    var nodeLogComponents = _parentWindow.XmlSettings.SelectSingleNode("/Settings/Logger/LogComponents");

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
                        _parentWindow.LoggerComponentLevel = iLogComponents;
                        // Set calculated log components value to the XML
                        nodeLogComponents.InnerXml = iLogComponents.ToString();
                    }

                    #endregion Component levels

                    #region State levels

                    // Save logger log state level
                    var nodeLogStateLevels = _parentWindow.XmlSettings.SelectSingleNode("/Settings/Logger/LogLevels");

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
                        _parentWindow.LoggerStateLevel = iLogStateLevel;
                        // Set calculated log components value to the XML
                        nodeLogStateLevels.InnerXml = iLogStateLevel.ToString();
                    }

                    #endregion State levels

                    // Show own message box that a restart of the software should be done
                    OwnMessageBox ombReboot = new OwnMessageBox(_xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", _strLanguage),
                        _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Content/LoggerSetttingsReboot", _strLanguage),
                        _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", _strLanguage),
                        _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", _strLanguage));
                    ombReboot.ShowDialog();
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("btnSave_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    _stopFomClosing = true;

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", _strLanguage),
                       _xmlLanguage, _strLanguage,
                       Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
                }
            }
            else
            {
                _stopFomClosing = true;

                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);               
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
