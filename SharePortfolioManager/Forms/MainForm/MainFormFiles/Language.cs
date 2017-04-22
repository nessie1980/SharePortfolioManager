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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using System.IO;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Load language

        /// <summary>
        /// This function loads the language and sets the language values
        /// to the controls (e.g. labels, buttons and so on).
        /// 
        /// REMARK: in DEBUG mode it is check if all language keys are set in the Language.XML file
        /// </summary>
        private void LoadLanguage()
        {

            // Check if the initialization was successfully so far
            if (_bInitFlag)
            {
                try
                {
                    // Load language XML file
                    _xmlLanguage = new Language(LanguageFileName);

                    // Check if the language file has been loaded
                    if (_xmlLanguage.InitFlag)
                    {
                        // ONLY in DEBUG mode
                        // Check if an language key is not defined in the Language.XML file and then create a
                        // a dialog with the undefined language keys
#if DEBUG_LANGUAGE
                        string strProjectPath =
                            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
                        _xmlLanguage.CheckLanguageKeysOfProject(strProjectPath);
                        _xmlLanguage.CheckLanguageKeysOfXML(strProjectPath);

                        if (_xmlLanguage.InvalidLanguageKeysOfProject.Count != 0 || _xmlLanguage.InvalidLanguageKeysOfXml.Count != 0)
                        {
                            string strInvalidKeys = @"";

                            if (_xmlLanguage.InvalidLanguageKeysOfProject.Count != 0)
                            {
                                strInvalidKeys = _xmlLanguage.InvalidLanguageKeysOfProject.Count + " invalid language keys in the project files.\n";

                                foreach (var invalidkeyProject in _xmlLanguage.InvalidLanguageKeysOfProject)
                                {
                                    strInvalidKeys += invalidkeyProject + "\n";
                                }

                                strInvalidKeys += "\n";
                            }

                            if (_xmlLanguage.InvalidLanguageKeysOfXml.Count != 0)
                            {
                                strInvalidKeys += _xmlLanguage.InvalidLanguageKeysOfXml.Count + " unused XML language keys in file \"" + LanguageFileName + "\"\n";
                                foreach (var invalidKeyXML in _xmlLanguage.InvalidLanguageKeysOfXml)
                                {
                                    strInvalidKeys += invalidKeyXML + "\n";
                                    
                                }
                            }
                            FrmInvalidLanguageKeys invalidLanguageKeysDlg = new FrmInvalidLanguageKeys();
                            invalidLanguageKeysDlg.Text += " - (Project path: " + strProjectPath + ")";
                            invalidLanguageKeysDlg.SetText(strInvalidKeys);
                            invalidLanguageKeysDlg.ShowDialog();
                        }
#endif
                        #region Load logger language

                        // Add state names
                        _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Start", _languageName));
                        _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Info", _languageName));
                        _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Warning", _languageName));
                        _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Error", _languageName));
                        _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/FatalError", _languageName));

                        // Add component names
                        _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", _languageName));
                        _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/WebParser", _languageName));
                        _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", _languageName));
                        _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/Logger", _languageName));

                        #endregion Load logger language

                        #region Add language menu items for the available languages

                        // Get settings menu item
                        ToolStripMenuItem tmiSettings =
                            (ToolStripMenuItem)menuStrip1.Items["settingsToolStripMenuItem"];
                        // Get language menu item
                        ToolStripMenuItem tmiLanguage =
                            (ToolStripMenuItem)tmiSettings.DropDownItems["languageToolStripMenuItem"];
                        // Get possible languages and add them to the menu
                        List<string> strLanguages = _xmlLanguage.GetAvailableLanguages();
                        // Add available to the menu
                        foreach (string strLanguage in strLanguages)
                        {
                            ToolStripMenuItem tmiLanguageAdd = new ToolStripMenuItem(strLanguage, null, languageClick,
                                string.Format("languageToolStripMenuItem{0}", strLanguage));

                            if (strLanguage == _languageName)
                                tmiLanguageAdd.Checked = true;

                            tmiLanguage.DropDownItems.Add(tmiLanguageAdd);
                        }

                        #endregion Add language menu items for the available languages

                        #region Set share object unit and percentage unit

                        ShareObject.PercentageUnit = _xmlLanguage.GetLanguageTextByXPath(@"/PercentageUnit", _languageName);
                        ShareObject.PieceUnit = _xmlLanguage.GetLanguageTextByXPath(@"/PieceUnit", _languageName);

                        #endregion Set share object unit and percentage unit
                    }
                    else
                    {
#if DEBUG
                        MessageBox.Show("LoadLanguage()\n\n" + _xmlLanguage.LastException.Message,
                            @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#endif
                        // Set initialization flag
                        _bInitFlag = false;

                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage, @"Could not load '" + LanguageFileName + @"' file!",
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("LoadLanguage()\n\n" + ex.Message,
                        @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    _bInitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage, @"Could not load '" + LanguageFileName + @"' file!",
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                }
            }

        }

        #endregion Load language

        #region Set language
        /// <summary>
        /// This function loads the language key values to the Main form dialog
        /// </summary>
        private void SetLanguage()
        {
            try
            {
                #region Application name

                Text = _xmlLanguage.GetLanguageTextByXPath(@"/Application/Name", _languageName)
                    + @" " + Helper.GetApplicationVersion().ToString();

                notifyIcon.Text = _xmlLanguage.GetLanguageTextByXPath(@"/Application/Name", _languageName)
                    + @" " + Helper.GetApplicationVersion().ToString();

                if (PortfolioFileName != "")
                {
                    Text += @" (" + Path.GetFileName(PortfolioFileName) + @")";
                }

                #endregion Application name

                #region Menu

                fileToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/File/Header", _languageName);
                newToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/File/New", _languageName);
                saveAsToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/File/SaveAs", _languageName);
                openToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/File/Open", _languageName);
                exitToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/File/Quit", _languageName);

                settingsToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Header",
                    _languageName);
                languageToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Language",
                    _languageName);
                loggerToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Logger",
                    _languageName);

                helpToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/Help/Header", _languageName);
                aboutToolStripMenuItem.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Menu/Help/About", _languageName);

                #endregion Menu

                #region  DataGirdView for the portfolio

                grpBoxSharePortfolio.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Caption",
                    _languageName);
                if (dgvPortfolio.Columns.Count == (int) ColumnIndicesPortfolio.EShareSumColumnIndex + 1)
                {
                    dgvPortfolio.Columns[(int) ColumnIndicesPortfolio.EWknNumberColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_WKN", _languageName);
                    dgvPortfolio.Columns[(int) ColumnIndicesPortfolio.EShareNameColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Name", _languageName);
                    dgvPortfolio.Columns[(int) ColumnIndicesPortfolio.ESharePriceColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Price", _languageName);
                    dgvPortfolio.Columns[(int) ColumnIndicesPortfolio.EShareVolumeColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Volume", _languageName);
                    dgvPortfolio.Columns[(int) ColumnIndicesPortfolio.ESharePerformanceDayBeforeColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_PrevDay", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareCostsDividendColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_CostsDividend", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Performance", _languageName);
                    dgvPortfolio.Columns[(int) ColumnIndicesPortfolio.EShareSumColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_DepositSum", _languageName);
                }

                #endregion  DataGirdView for the portfolio

                #region DataGirdView footer

                if (dgvPortfolioFooter.RowCount > 0)
                {
                    dgvPortfolioFooter.Rows[0].Cells[(int)ColumnIndicesPortfolioFooter.ELabelTotalColumnIndex].Value = string.Format(@"{0}",
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalDeposit", _languageName));
                    dgvPortfolioFooter.Rows[1].Cells[(int)ColumnIndicesPortfolioFooter.ELabelCostsDividendIndex].Value = string.Format(@"{0}",
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalCostDividend", _languageName));
                    dgvPortfolioFooter.Rows[1].Cells[(int)ColumnIndicesPortfolioFooter.ELabelTotalColumnIndex].Value = string.Format(@"{0}",
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalPerformance",
                            _languageName));
                    dgvPortfolioFooter.Rows[2].Cells[(int)ColumnIndicesPortfolioFooter.ELabelTotalColumnIndex].Value = string.Format(@"{0}",
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalSum", _languageName));
                }

                #endregion DataGirdView footer

                #region Buttons

                btnRefreshAll.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll",
                    _languageName);
                btnRefresh.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh",
                    _languageName);
                btnAdd.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Add", _languageName);
                btnEdit.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Edit", _languageName);
                btnDelete.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Delete",
                    _languageName);
                btnClearLogger.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/ResetLogger",
                    _languageName);

                #endregion Buttons

                #region GrpBox for the details

                grpBoxShareDetails.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/Caption",
                    _languageName);

                tabCtrlDetails.TabPages["tabPgShareDetailsWithDividendCosts"].Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Caption",
                        _languageName);
                lblShareDetailsWithDividendCostShareDate.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/Date",
                        _languageName);
                lblShareDetailsWithDividendCostShareVolume.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/Volume",
                        _languageName);
                lblShareDetailsWithDividendCostShareDividend.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalDividend",
                        _languageName);
                lblShareDetailsWithDividendCostShareCost.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalCosts",
                        _languageName);
                lblShareDetailsWithDividendCostSharePriceCurrent.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/CurrentPrice",
                        _languageName);
                lblShareDetailsWithDividendCostShareDiffPerformancePrev.Text =
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/DiffPerformancePrevDay", _languageName);
                lblShareDetailsWithDividendCostShareDiffSumPrev.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/DiffSumPrevDay",
                        _languageName);
                lblShareDetailsWithDividendCostSharePricePrev.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/PricePrevDay",
                        _languageName);
                lblShareDetailsWithDividendCostShareDeposit.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/Deposit",
                        _languageName);
                lblShareDetailsWithDividendCostShareTotalPerformance.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalPerformance",
                        _languageName);
                lblShareDetailsWithDividendCostShareTotalProfit.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalProfitLoss",
                        _languageName);
                lblShareDetailsWithDividendCostShareTotalSum.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalSum",
                        _languageName);

                tabCtrlDetails.TabPages["tabPgShareDetailsWithOutDividendCosts"].Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Caption",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareDate.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/Date",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareVolume.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/Volume",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareDividend.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalDividend",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareCost.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalCosts",
                        _languageName);
                lblShareDetailsWithOutDividendCostSharePriceCurrent.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/CurrentPrice",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareDiffPerformancePrev.Text =
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/DiffPerformancePrevDay", _languageName);
                lblShareDetailsWithOutDividendCostShareDiffSumPrev.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/DiffSumPrevDay",
                        _languageName);
                lblShareDetailsWithOutDividendCostSharePricePrev.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/PricePrevDay",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareDeposit.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/Deposit",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareTotalPerformance.Text =
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalPerformance", _languageName);
                lblShareDetailsWithOutDividendCostShareTotalProfit.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalProfitLoss",
                        _languageName);
                lblShareDetailsWithOutDividendCostShareTotalSum.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalSum",
                        _languageName);

                tabCtrlDetails.TabPages["tabPgDividend"].Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", _languageName);
                tabCtrlDetails.TabPages["tabPgCosts"].Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Caption", _languageName);
                tabCtrlDetails.TabPages["tabPgProfitLoss"].Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", _languageName);

                #endregion GrpBox for the details

                #region GrpBox status message

                grpBoxStatusMessage.Text = _xmlLanguage.GetLanguageTextByXPath(
                    @"/MainForm/GrpBoxStatusMessage/Caption", _languageName);

                #endregion GrpBox status message

                #region GrpBox update state

                grpBoxUpdateState.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxUpdateState/Caption",
                    _languageName);

                #endregion GrpBox update state

                #region Logger language

                // Clear state list
                _loggerStatelList.Clear();

                // Add state names
                _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Start", _languageName));
                _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Info", _languageName));
                _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Warning", _languageName));
                _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/Error", _languageName));
                _loggerStatelList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/States/FatalError", _languageName));

                // Clear component names
                _loggerComponentNamesList.Clear();

                // Add component names
                _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", _languageName));
                _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/WebParser", _languageName));
                _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", _languageName));
                _loggerComponentNamesList.Add(_xmlLanguage.GetLanguageTextByXPath(@"/Logger/ComponentNames/Logger", _languageName));

                #endregion Looger language

                #region Set share object unit and percentage unit

                ShareObject.PercentageUnit = _xmlLanguage.GetLanguageTextByXPath(@"/PercentageUnit", _languageName);
                ShareObject.PieceUnit = _xmlLanguage.GetLanguageTextByXPath(@"/PieceUnit", _languageName);

                #endregion Set share object unit and percentage unit
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message, @"Error - SetLanguage()", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                if (_xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", _languageName) !=
                    _xmlLanguage.InvalidLanguageKeyValue)
                {
                    // Set status message
                    lblWebParserState.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", _languageName);
                    //Helper.AddStatusMessage(richTextBox1, statusMessage, Color.Red);

                    // Write log
                    _logger.AddEntry(_xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", _languageName), (Logger.ELoggerStateLevels)EStateLevels.Start, (Logger.ELoggerComponentLevels)EComponentLevels.LanguageHandler);
                }
                else
                {
                    // Set status message
                    lblWebParserState.Text = lblWebParserState.Text = @"Could not set all language keys to the controls!";
                    //Helper.AddStatusMessage(richTextBox1, statusMessage, Color.Red);

                    // Write log
                    _logger.AddEntry(@"Could not set all language keys to the controls!", (Logger.ELoggerStateLevels)EStateLevels.Start, (Logger.ELoggerComponentLevels)EComponentLevels.LanguageHandler);
                }

                // Update control list
                _enableDisableControlNames.Add("btnRefreshAll");
                _enableDisableControlNames.Add("btnRefresh");
                _enableDisableControlNames.Add("menuStrip1");

                // Disable controls
                Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                // Set initialization flag
                _bInitFlag = false;
            }
        }

        #endregion Set language
    }
}