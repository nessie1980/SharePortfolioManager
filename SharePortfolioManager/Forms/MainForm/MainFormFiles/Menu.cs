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
using SharePortfolioManager.Forms.AboutForm;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

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
            var oldPortfolioFileName = _portfolioFileName;

            var dlgPortfolioFileName = new OwnMessageBox(
                Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/InputPortfolioName", LanguageName),
                Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/InputPortfolioName", LanguageName),
                Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName),
                Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName),
                true);
            var dlgResult = dlgPortfolioFileName.ShowDialog();

            if (dlgResult != DialogResult.OK) return;

            _portfolioFileName = Application.StartupPath + "\\Portfolios\\" + dlgPortfolioFileName.InputString + ".xml";

            // Check if the portfolio file already exists
            if (File.Exists(_portfolioFileName))
            {
                var dlgPortfolioFileExists = new OwnMessageBox(
                    Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName),
                    Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/PortfolioNameExists", LanguageName),
                    Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName),
                    Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName));

                var dlgResultFileExists = dlgPortfolioFileExists.ShowDialog();

                if (dlgResultFileExists == DialogResult.Cancel)
                    _portfolioFileName = "";
            }

            // Cancel has been pressed so do nothing
            if (_portfolioFileName == "")
            {
                _portfolioFileName = oldPortfolioFileName;
                return;
            }

            // Check if the portfolio directory does not exist so create it
            var path = Path.GetDirectoryName(_portfolioFileName);
            if (path != null && !Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Check if the portfolio directory creation was successful
            if (Directory.Exists(Path.GetDirectoryName(_portfolioFileName)))
            {
                using (var writer = XmlWriter.Create(_portfolioFileName))
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
                // TODO Error logging
            }
        }

        /// <summary>
        /// This function allows the user to open a existing portfolio file
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">EventArgs</param>
        private void OnOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var strOldPortfolioFileName = _portfolioFileName;

            const string strFilter = "XML (*.XML)|*.XML";
            _portfolioFileName = Helper.LoadPortfolio(Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title", LanguageName), strFilter, _portfolioFileName);

            // Check if the portfolio file name has been changed
            if (strOldPortfolioFileName != _portfolioFileName)
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
                var oldLanguage = LanguageName;

                // Get the language settings node element form Settings.XML
                var nodeLanguage = Settings.SelectSingleNode("/Settings/Language");
                if (nodeLanguage != null)
                {
                    foreach (XmlNode nodeElement in nodeLanguage)
                    {
                        if (nodeElement == null) continue;

                        // Set new language to the node element in the Settings.XML
                        nodeElement.InnerText = (sender).ToString();

                        // Set new language to the global language variable
                        LanguageName = (sender).ToString();
                    }
                }

                // Close reader for saving
                ReaderSettings.Close();
                // Save settings
                Settings.Save(SettingsFileName);
                // Create a new reader and reload Settings.XML
                ReaderSettings = XmlReader.Create(SettingsFileName, ReaderSettingsSettings);
                Settings.Load(ReaderSettings);

                // Get settings menu item
                var tmiSettings = (ToolStripMenuItem)menuStrip1.Items["settingsToolStripMenuItem"];
                // Get language menu item
                var tmiLanguage = (ToolStripMenuItem)tmiSettings.DropDownItems["languageToolStripMenuItem"];

                // Loop through the menu language items and select or deselect the language entries
                foreach (ToolStripMenuItem item in tmiLanguage.DropDownItems)
                {
                    item.Checked = item.Name == ((ToolStripMenuItem)sender).Name;
                }

                // Check if the language really change
                if (oldLanguage == LanguageName) return;

                // Reload language keys of the controls
                SetLanguage();

                // Select the first item in the DataGridView portfolio
                if (dgvPortfolioFinalValue.SelectedRows.Count > 0)
                {
                    if (dgvPortfolioFinalValue.Rows.Count > 0 && dgvPortfolioFinalValue.Rows.Count >= dgvPortfolioFinalValue.SelectedRows[0].Index)
                    {
                        dgvPortfolioFinalValue.Rows[dgvPortfolioFinalValue.SelectedRows[0].Index].Selected = true;
                        // TODO
                        //UpdateShareDetails();
                        //UpdateDividendDetails();
                        //UpdateBrokerageDetails();
                        //UpdateProfitLossDetails();
                    }
                }

                // Reset update GroupBox update state
                progressBarWebParser.Value = 0;
                lblShareNameWebParser.Text = @"";
                lblWebParserState.Text = @"";

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangeLanguageSuccessful", LanguageName),
                    Language, LanguageName,
                    Color.Orange, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/ChangeLanguageFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
                var loggerSettings = new FrmLoggerSettings(this, Logger, Language, LanguageName);

                if (loggerSettings.ShowDialog() != DialogResult.OK) return;

                // Close reader for saving
                ReaderSettings.Close();
                // Save settings
                Settings.Save(SettingsFileName);
                // Create a new reader to test if the saved values could be loaded
                ReaderSettings = XmlReader.Create(SettingsFileName, ReaderSettingsSettings);
                Settings.Load(ReaderSettings);

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsSuccessful", LanguageName),
                    Language, LanguageName,
                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
                var frmAbout = new FrmAbout(Language, LanguageName);
                frmAbout.ShowDialog();
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/AboutForm/Errors/ShowAboutBoxFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Menu
    }
}
