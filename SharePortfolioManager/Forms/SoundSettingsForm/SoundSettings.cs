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
//#define DEBUG_SOUNDS_SETTINGS

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using System;
using System.Drawing;
using System.IO;
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

                Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/Caption", LanguageName);

                grpBoxUpdateFinishedSound.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxUpdateFinishedSound/Caption", LanguageName);
                lblUpdateFinishedSound.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxUpdateFinishedSound/Labels/UpdateFinishSound", LanguageName);
                btnUpdateFinishedSound.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxUpdateFinishedSound/Buttons/Browse", LanguageName);

                chkBoxUpdateFinishedSoundPlay.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxUpdateFinishedSound/CheckBoxes/EnableUpdateFinishSound", LanguageName);

                grpBoxErrorSound.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxErrorSound/Caption", LanguageName);
                lblErrorSound.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxErrorSound/Labels/ErrorSound", LanguageName);
                btnErrorSound.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxErrorSound/Buttons/Browse", LanguageName);

                chkBoxErrorSoundPlay.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/GrpBoxErrorSound/CheckBoxes/EnableErrorSound", LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/Buttons/Save", LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/Buttons/Cancel", LanguageName);

                #endregion Language configuration

                #region Sounds enable

                chkBoxUpdateFinishedSoundPlay.Checked = Sound.UpdateFinishedEnable;
                chkBoxErrorSoundPlay.Checked = Sound.ErrorEnable;

                #endregion Sounds enable

                #region Sounds filename

                lblUpdateFinishedSound.Text = Path.GetFileName(Sound.UpdateFinishedFileName);
                lblErrorSound.Text = Path.GetFileName(Sound.ErrorFileName);

                #endregion Sounds filename
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/SoundsSettingsForm/Errors/SoundSettingsShowFailed",
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
        /// This function allows the user to open a file dialog to change the update finished sound.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnUpdateFinishedSoundBrowse_Click(object sender, EventArgs e)
        {
            var strCurrentFile = Sound.UpdateFinishedFileName;

            const string strFilter = "wav (*.wav)|*.wav";
            if (Helper.SetDocument(
                Language.GetLanguageTextByXPath(
                    @"/SoundsSettingsForm/GrpBoxUpdateFinishedSound/OpenFileDialog/Title", LanguageName),
                strFilter, ref strCurrentFile) != DialogResult.OK)
                return;

            Sound.UpdateFinishedFileName = strCurrentFile;
            lblUpdateFinishedSound.Text = Path.GetFileName(strCurrentFile);

        }

        /// <summary>
        /// This function allows the user to open a file dialog to change the error sound.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnErrorSoundBrowse_Click(object sender, EventArgs e)
        {
            var strCurrentFile = Sound.ErrorFileName;

            const string strFilter = "wav (*.wav)|*.wav";
            if (Helper.SetDocument(
                Language.GetLanguageTextByXPath(
                    @"/SoundsSettingsForm/GrpBoxErrorSound/OpenFileDialog/Title", LanguageName),
                strFilter, ref strCurrentFile) != DialogResult.OK)
                return;

            lblErrorSound.Text = Path.GetFileName(strCurrentFile);
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
                    #region Set update finished sound settings

                    // Save update finished file name
                    var nodeUpdateFinishedSoundFileName =
                        ParentWindow.Settings.SelectSingleNode("/Settings/Sounds/UpdateFinished");

                    if (nodeUpdateFinishedSoundFileName != null)
                    {
                        Sound.UpdateFinishedFileName = Path.GetDirectoryName(Sound.UpdateFinishedFileName) + @"\" +
                                                       lblUpdateFinishedSound.Text;

                        // Set sound file name to the XML
                        nodeUpdateFinishedSoundFileName.InnerXml = lblUpdateFinishedSound.Text;
                    }

                    // Save update finished enable flag
                    var nodeUpdateFinishedSoundEnable =
                        ParentWindow.Settings.SelectSingleNode("/Settings/Sounds/UpdateFinishedEnabled");

                    if (nodeUpdateFinishedSoundEnable != null)
                    {
                        Sound.UpdateFinishedEnable = chkBoxUpdateFinishedSoundPlay.Checked;

                        // Set sound enable to the XML
                        nodeUpdateFinishedSoundEnable.InnerXml = chkBoxUpdateFinishedSoundPlay.Checked.ToString();
                    }

                    #endregion Set update finished sound settings

                    #region Set error sound settings

                    // Save error file name
                    var nodeErrorSoundFileName =
                        ParentWindow.Settings.SelectSingleNode("/Settings/Sounds/Error");

                    if (nodeErrorSoundFileName != null)
                    {
                        Sound.ErrorFileName = Path.GetDirectoryName(Sound.ErrorFileName) + @"\" +
                                                       lblErrorSound.Text;

                        // Set sound file name to the XML
                        nodeErrorSoundFileName.InnerXml = lblErrorSound.Text;
                    }

                    // Save error enable flag
                    var nodeErrorSoundEnable =
                        ParentWindow.Settings.SelectSingleNode("/Settings/Sounds/ErrorEnabled");

                    if (nodeErrorSoundEnable != null)
                    {
                        // Set sound enable to the XML
                        nodeErrorSoundEnable.InnerXml = chkBoxErrorSoundPlay.Checked.ToString();
                    }

                    #endregion Set update finished sound settings
                }
                catch (Exception ex)
                {
                    StopFomClosingFlag = true;

                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsFailed",
                            LanguageName),
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
    }
}
