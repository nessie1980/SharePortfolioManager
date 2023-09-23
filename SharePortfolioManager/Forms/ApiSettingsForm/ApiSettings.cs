//MIT License
//
//Copyright(c) 2017 - 2022 nessie1980(nessie1980@gmx.de)
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
//#define DEBUG_API_SETTINGS

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SharePortfolioManager.Classes.Configurations;

namespace SharePortfolioManager.ApiSettingsForm
{
    public partial class FrmApiSettings : Form
    {
        #region Properties

        public FrmMain ParentWindow { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

        public string ApiKeyName { get; set; }

        public bool StopFomClosingFlag { get; set; }

        #endregion Properties

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmApiSettings(FrmMain parentWindow, Logger logger, Language xmlLanguage, string strLanguage, string strApiKeyName)
        {
            InitializeComponent();

            ParentWindow = parentWindow;
            Logger = logger;
            Language = xmlLanguage;
            LanguageName = strLanguage;
            ApiKeyName = strApiKeyName;

            StopFomClosingFlag = false;
        }

        /// <summary>
        /// This function is called when the form is closing
        /// Here we check if the form really should be closed
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void FrmApiSettings_FormClosing(object sender, FormClosingEventArgs e)
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
        /// This function loads the api settings value to the form
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">EventArgs</param>
        private void FrmApiSettings_Load(object sender, EventArgs e)
        {
            try
            {
                #region API key configuration

                Text = Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Caption", SettingsConfiguration.LanguageName) + "\"" + ApiKeyName + "\"";

                if (SettingsConfiguration.ApiKeysDictionary.ContainsKey(ApiKeyName))
                {
                    txtBoxApiKey.Text = SettingsConfiguration.ApiKeysDictionary[ApiKeyName];
                }
                else
                {
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Errors/ApiSettingsApiKeyLoadFailed",
                            LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                        (int)FrmMain.EComponentLevels.Application);
                }

                grpBoxApiKey.Text = Language.GetLanguageTextByXPath(@"/ApiSettingsForm/GrpBoxApiKey/Caption", SettingsConfiguration.LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Buttons/Save", SettingsConfiguration.LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Buttons/Cancel", SettingsConfiguration.LanguageName);

                #endregion API key configuration
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Errors/ApiSettingsShowFailed",
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
                    #region Set API key

                    // Save update finished file name
                    SettingsConfiguration.ApiKeysDictionary[ApiKeyName] = txtBoxApiKey.Text;

                    #endregion Set API key

                    if (SettingsConfiguration.SaveSettingsConfiguration()) return;

                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Errors/SaveSettingsFailed",
                            LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                        (int)FrmMain.EComponentLevels.Application);
                }
                catch (Exception ex)
                {
                    StopFomClosingFlag = true;

                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Errors/SaveSettingsFailed",
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
                   Language.GetLanguageTextByXPath(@"/ApiSettingsForm/Errors/SaveSettingsFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);               
            }
        }

        #endregion Buttons
    }
}

