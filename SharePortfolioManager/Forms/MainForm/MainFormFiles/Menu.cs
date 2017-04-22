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
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OwnMessageBox dlgPortfolioFileName = new OwnMessageBox(
                _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/InputPortfolioName", _languageName),
                _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Content/InputPortfolioName", _languageName),
                _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", _languageName),
                _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", _languageName),
                true);
            DialogResult dlgResult = dlgPortfolioFileName.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                PortfolioFileName = Application.StartupPath + "\\Portfolios\\" + dlgPortfolioFileName.InputString + ".XML";

                // Check if the portfolio file already exists
                if (File.Exists(PortfolioFileName))
                {
                    OwnMessageBox dlgPortfolioFileExists = new OwnMessageBox(
                    _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", _languageName),
                    _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Content/PortfolioNameExists", _languageName),
                    _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", _languageName),
                    _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", _languageName));

                    DialogResult dlgResultFileExists = dlgPortfolioFileExists.ShowDialog();

                    if (dlgResultFileExists == DialogResult.Cancel)
                        PortfolioFileName = "";
                }

                // Write new file or overwrite already existing file
                if (PortfolioFileName != "")
                {
                    // Check if the portfolio directory exists
                    if (!Directory.Exists(Path.GetDirectoryName(PortfolioFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(PortfolioFileName));

                    // Check if the portfolio directory creation was successful
                    if (Directory.Exists(Path.GetDirectoryName(PortfolioFileName)))
                    {
                        using (XmlWriter writer = XmlWriter.Create(PortfolioFileName))
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
            }
        }

        /// <summary>
        /// This function allows the user to open a existing portfolio file
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">EventArgs</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strOldPortfolioFileName = PortfolioFileName;

            const string strFilter = "XML (*.XML)|*.XML";
            PortfolioFileName = Helper.LoadPortfolio(_xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title", _languageName), strFilter, PortfolioFileName);

            // Check if the portfolio file name has been changed
            if (strOldPortfolioFileName != PortfolioFileName)
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
        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This function changes the language
        /// </summary>
        /// <param name="sender">MenuStrip language</param>
        /// <param name="e">EventArgs</param>
        public void languageClick(object sender, EventArgs e)
        {
            try
            {
                // Old language
                string oldLanguage = _languageName;

                // Get the language settings node element form Settings.XML
                var nodeLanguage = _xmlSettings.SelectSingleNode("/Settings/Language");
                foreach (XmlNode nodeElement in nodeLanguage)
                {
                    if (nodeElement != null)
                    {
                        // Set new language to the node element in the Settings.XML
                        nodeElement.InnerText = (sender).ToString();

                        // Set new language to the global language variable
                        _languageName = (sender).ToString();
                    }
                }

                // Close reader for saving
                _xmlReaderSettings.Close();
                // Save settings
                _xmlSettings.Save(SettingsFileName);
                // Create a new reader and reload Settings.XML
                _xmlReaderSettings = XmlReader.Create(SettingsFileName, _xmlReaderSettingsSettings);
                _xmlSettings.Load(_xmlReaderSettings);

                // Get settings menu item
                ToolStripMenuItem tmiSettings = (ToolStripMenuItem)menuStrip1.Items["settingsToolStripMenuItem"];
                // Get language menu item
                ToolStripMenuItem tmiLanguage = (ToolStripMenuItem)tmiSettings.DropDownItems["languageToolStripMenuItem"];

                // Loop through the menu language items and select or deselect the language entries
                foreach (ToolStripMenuItem item in tmiLanguage.DropDownItems)
                {
                    if (item.Name == ((ToolStripMenuItem)sender).Name)
                        item.Checked = true;
                    else
                        item.Checked = false;
                }

                // Check if the language really change
                if (oldLanguage != _languageName)
                {
                    // Reload language keys of the controls
                    SetLanguage();

                    // Select the first item in the DataGridView portfolio
                    if (dgvPortfolio.SelectedRows.Count > 0)
                    {
                        if (dgvPortfolio.Rows.Count > 0 && dgvPortfolio.Rows.Count >= dgvPortfolio.SelectedRows[0].Index)
                        {
                            dgvPortfolio.Rows[dgvPortfolio.SelectedRows[0].Index].Selected = true;
                            UpdateShareDetails();
                            UpdateDividendDetails();
                            UpdateCostsDetails();
                            UpdateProfitLossDetails();
                        }
                    }

                    // Reset update GroupBox update state
                    progressBarWebParser.Value = 0;
                    lblShareNameWebParser.Text = @"";
                    lblWebParserState.Text = @"";

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangeLanguageSuccessful", _languageName),
                        _xmlLanguage, _languageName,
                        Color.Orange, _logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("languageClick()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/ChangeLanguageFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the logger settings dialog
        /// </summary>
        /// <param name="sender">MenuStrip logger</param>
        /// <param name="e">EventArgs</param>
        private void loggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLoggerSettings loggerSettings = new FrmLoggerSettings(this, _logger, _xmlLanguage, _languageName);

                if (loggerSettings.ShowDialog() == DialogResult.OK)
                {
                    // Close reader for saving
                    _xmlReaderSettings.Close();
                    // Save settings
                    _xmlSettings.Save(SettingsFileName);
                    // Create a new reader to test if the saved values could be loaded
                    _xmlReaderSettings = XmlReader.Create(SettingsFileName, _xmlReaderSettingsSettings);
                    _xmlSettings.Load(_xmlReaderSettings);

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsSuccessful", _languageName),
                        _xmlLanguage, _languageName,
                        Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("loggerToolStripMenuItem_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/LoggerSettingsForm/Errors/SaveSettingsFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the about form
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">EventArgs</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FrmAbout frmAbout = new FrmAbout(this, _xmlLanguage, _languageName);
                frmAbout.ShowDialog();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("aboutToolStripMenuItem_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Errors/ShowAboutBoxFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Menu
    }
}
