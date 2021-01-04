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
//#define DEBUG_SOUNDS_SETTINGS

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SharePortfolioManager.Classes.Configurations;

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
        public FrmSoundSettings(FrmMain parentWindow, Logger logger, Language xmlLanguage, string strLanguage)
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

                Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Caption", SettingsConfiguration.LanguageName);

                grpBoxUpdateFinishedSound.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxUpdateFinishedSound/Caption", SettingsConfiguration.LanguageName);
                chkBoxUpdateFinishedSoundPlay.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxUpdateFinishedSound/CheckBoxes/EnableUpdateFinishSound", SettingsConfiguration.LanguageName);

                grpBoxErrorSound.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxErrorSound/Caption", SettingsConfiguration.LanguageName);
                chkBoxErrorSoundPlay.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/GrpBoxErrorSound/CheckBoxes/EnableErrorSound", SettingsConfiguration.LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Buttons/Save", SettingsConfiguration.LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Buttons/Cancel", SettingsConfiguration.LanguageName);

                #endregion Language configuration

                #region Sounds enable

                chkBoxUpdateFinishedSoundPlay.Checked = Sound.UpdateFinishedEnable;
                chkBoxErrorSoundPlay.Checked = Sound.ErrorEnable;

                #endregion Sounds enable

                #region Sounds filename

                var files = Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath) + Sound.SoundFilesDirectory);

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    cbxUpdateFinishedSound.Items.Add(fileName);
                }

                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    cbxErrorSound.Items.Add(fileName);
                }

                cbxUpdateFinishedSound.SelectedIndex = cbxUpdateFinishedSound.Items.IndexOf(Path.GetFileName(Sound.UpdateFinishedFileName));
                cbxErrorSound.SelectedIndex = cbxErrorSound.Items.IndexOf(Path.GetFileName(Sound.ErrorFileName));

                #endregion Sounds filename
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SoundSettingsShowFailed",
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
            if (SettingsConfiguration.XmlDocument != null)
            {
                try
                {
                    #region Set update finished sound settings

                    // Save update finished file name
                    Sound.UpdateFinishedFileName =
                        Path.GetDirectoryName(Application.ExecutablePath) + Sound.SoundFilesDirectory +
                        cbxUpdateFinishedSound.SelectedItem;

                    // Save update finished enable flag
                    Sound.UpdateFinishedEnable = chkBoxUpdateFinishedSoundPlay.Checked;

                    #endregion Set update finished sound settings

                    #region Set error sound settings

                    // Save error file name
                    Sound.ErrorFileName =
                        Path.GetDirectoryName(Application.ExecutablePath) + Sound.SoundFilesDirectory +
                        cbxErrorSound.SelectedItem;

                    // Save error enable flag
                    Sound.ErrorEnable = chkBoxErrorSoundPlay.Checked;

                    if (SettingsConfiguration.SaveSettingsConfiguration()) return;

                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsFailed",
                            LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                        (int)FrmMain.EComponentLevels.Application);

                    #endregion Set update finished sound settings
                }
                catch (Exception ex)
                {
                    StopFomClosingFlag = true;

                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsFailed",
                            LanguageName),
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
                   Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);               
            }
        }

        #endregion Buttons
    }
}

