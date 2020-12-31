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
//#define DEBUG_MAIN_FORM_MENU

using SharePortfolioManager.AboutForm;
using SharePortfolioManager.Classes;
using SharePortfolioManager.LoggerSettingsForm;
using SharePortfolioManager.OwnMessageBoxForm;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SharePortfolioManager.Classes.Configurations;
using SharePortfolioManager.SoundSettingsForm;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Menu

        /// <summary>
        /// This function creates a new portfolio file
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">EventArgs</param>
        private void OnNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save old portfolio filename
            var oldPortfolioFileName = SettingsConfiguration.PortfolioName;

            var dlgPortfolioFileName = new OwnMessageBox(
                LanguageConfiguration.Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                    (int) EOwnMessageBoxInfoType.InputFileName],
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/InputPortfolioName", SettingsConfiguration.LanguageName),
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", SettingsConfiguration.LanguageName),
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", SettingsConfiguration.LanguageName),
                EOwnMessageBoxInfoType.InputFileName,
                true);
            var dlgResult = dlgPortfolioFileName.ShowDialog();

            if (dlgResult != DialogResult.OK) return;

            SettingsConfiguration.PortfolioName = Application.StartupPath + "\\Portfolios\\" + dlgPortfolioFileName.InputString + ".xml";

            // Check if the portfolio file already exists
            if (File.Exists(SettingsConfiguration.PortfolioName))
            {
                var dlgPortfolioFileExists = new OwnMessageBox(
                    LanguageConfiguration.Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                        (int) EOwnMessageBoxInfoType.Info],
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/PortfolioNameExists", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", SettingsConfiguration.LanguageName),
                    EOwnMessageBoxInfoType.Info);

                var dlgResultFileExists = dlgPortfolioFileExists.ShowDialog();

                if (dlgResultFileExists == DialogResult.Cancel)
                    SettingsConfiguration.PortfolioName = "";
            }

            // Cancel has been pressed so do nothing
            if (SettingsConfiguration.PortfolioName == "")
            {
                SettingsConfiguration.PortfolioName = oldPortfolioFileName;
                return;
            }

            // Check if the portfolio directory does not exist so create it
            var path = Path.GetDirectoryName(SettingsConfiguration.PortfolioName);
            if (path != null && !Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Check if the portfolio directory creation was successful
            if (Directory.Exists(Path.GetDirectoryName(SettingsConfiguration.PortfolioName)))
            {
                using (var writer = XmlWriter.Create(SettingsConfiguration.PortfolioName))
                {
                    writer.WriteStartElement("Portfolio");
                    writer.WriteEndElement();
                    writer.Flush();
                }

                // Do GUI changes for the portfolio change
                ChangePortfolio();
            }
            else
            {
                var dlgPortfolioDirectoryCreationFailed = new OwnMessageBox(
                    LanguageConfiguration.Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                        (int)EOwnMessageBoxInfoType.Info],
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/PortfolioDirectoryCreationFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", SettingsConfiguration.LanguageName),
                    EOwnMessageBoxInfoType.Info);

                dlgPortfolioDirectoryCreationFailed.ShowDialog();
            }
        }

        /// <summary>
        /// This function allows the user to open a existing portfolio file
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">EventArgs</param>
        private void OnOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var strOldPortfolioFileName = SettingsConfiguration.PortfolioName;

            const string strFilter = "XML (*.XML)|*.XML";
            SettingsConfiguration.PortfolioName =
                Helper.LoadPortfolio(
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title",
                        SettingsConfiguration.LanguageName), strFilter, SettingsConfiguration.PortfolioName);

            // Check if the portfolio file name has been changed
            if (strOldPortfolioFileName != SettingsConfiguration.PortfolioName)
            {
                // Do GUI changes for the portfolio change
                ChangePortfolio();
            }
        }

        /// <summary>
        /// This function closes the application
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">EventArgs</param>
        private void OnExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This function changes the language
        /// </summary>
        /// <param name="sender">MenuStrip language</param>
        /// <param name="e">EventArgs</param>
        public void OnLanguageClick(object sender, EventArgs e)
        {
            try
            {
                // Old language
                var oldLanguage = SettingsConfiguration.LanguageName;

                // Set new language to the global language variable
                SettingsConfiguration.LanguageName = (sender).ToString();
                SettingsConfiguration.SaveSettingsConfiguration();

                // Get settings menu item
                var tmiSettings = (ToolStripMenuItem) menuStrip1.Items["settingsToolStripMenuItem"];
                // Get language menu item
                var tmiLanguage = (ToolStripMenuItem) tmiSettings.DropDownItems["languageToolStripMenuItem"];

                // Loop through the menu language items and select or deselect the language entries
                foreach (ToolStripMenuItem item in tmiLanguage.DropDownItems)
                {
                    item.Checked = item.Name == ((ToolStripMenuItem) sender).Name;
                }

                // Check if the language really change
                if (oldLanguage == SettingsConfiguration.LanguageName) return;

                // Reload language keys of the controls
                SetLanguage();

                // Select the first item in the DataGridView portfolio
                if (dgvPortfolioFinalValue.SelectedRows.Count > 0)
                {
                    if (dgvPortfolioFinalValue.Rows.Count > 0 && dgvPortfolioFinalValue.Rows.Count >=
                        dgvPortfolioFinalValue.SelectedRows[0].Index)
                    {
                        dgvPortfolioFinalValue.Rows[dgvPortfolioFinalValue.SelectedRows[0].Index].Selected = true;
                    }
                }

                // Reset update GroupBox update state
                progressBarWebParserMarketValues.Value = 0;
                progressBarWebParserDailyValues.Value = 0;
                lblWebParserDailyValuesState.Text = @"";
                lblWebParserMarketValuesState.Text = @"";

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangeLanguageSuccessful", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.Orange, Logger, (int) EStateLevels.Warning, (int) EComponentLevels.Application);
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/ChangeLanguageFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function opens the logger settings dialog
        /// </summary>
        /// <param name="sender">MenuStrip logger</param>
        /// <param name="e">EventArgs</param>
        private void OnLoggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var loggerSettings = new FrmLoggerSettings(this, Logger, LanguageConfiguration.Language, SettingsConfiguration.LanguageName);

                if (loggerSettings.ShowDialog() != DialogResult.OK) return;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsSuccessful", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Application);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function opens the sound settings dialog
        /// </summary>
        /// <param name="sender">MenuStrip sound</param>
        /// <param name="e">EventArgs</param>
        private void OnSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var soundSettings = new FrmSoundSettings(this, Logger, LanguageConfiguration.Language, SettingsConfiguration.LanguageName);

                if (soundSettings.ShowDialog() != DialogResult.OK) return;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsSuccessful", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/SoundSettingsForm/Errors/SaveSettingsFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function opens the about form
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">EventArgs</param>
        private void OnAboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var frmAbout = new FrmAbout(LanguageConfiguration.Language, SettingsConfiguration.LanguageName);
                frmAbout.ShowDialog();
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/AboutForm/Errors/ShowAboutBoxFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Menu
    }
}
