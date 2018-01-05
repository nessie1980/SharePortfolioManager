﻿//MIT License
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
using System.Drawing;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Classes.ShareObjects;

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
            if (!InitFlag) return;

            try
            {
                // Load language XML file
                Language = new Language(LanguageFileName);

                // Check if the language file has been loaded
                if (Language.InitFlag)
                {
                    // ONLY in DEBUG mode
                    // Check if an language key is not defined in the Language.XML file and then create a
                    // a dialog with the undefined language keys
#if DEBUG_LANGUAGE
                        string strProjectPath =
                            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
                        Language.CheckLanguageKeysOfProject(strProjectPath);
                        Language.CheckLanguageKeysOfXML(strProjectPath);

                        if (Language.InvalidLanguageKeysOfProject.Count != 0 || Language.InvalidLanguageKeysOfXml.Count != 0)
                        {
                            string strInvalidKeys = @"";

                            if (Language.InvalidLanguageKeysOfProject.Count != 0)
                            {
                                strInvalidKeys = Language.InvalidLanguageKeysOfProject.Count + " invalid language keys in the project files.\n";

                                foreach (var invalidkeyProject in Language.InvalidLanguageKeysOfProject)
                                {
                                    strInvalidKeys += invalidkeyProject + "\n";
                                }

                                strInvalidKeys += "\n";
                            }

                            if (Language.InvalidLanguageKeysOfXml.Count != 0)
                            {
                                strInvalidKeys += Language.InvalidLanguageKeysOfXml.Count + " unused XML language keys in file \"" + LanguageFileName + "\"\n";
                                foreach (var invalidKeyXML in Language.InvalidLanguageKeysOfXml)
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
                    LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Start", LanguageName));
                    LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Info", LanguageName));
                    LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Warning", LanguageName));
                    LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Error", LanguageName));
                    LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", LanguageName));

                    // Add component names
                    LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", LanguageName));
                    LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/WebParser", LanguageName));
                    LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", LanguageName));
                    LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Logger", LanguageName));

                    #endregion Load logger language

                    #region Add language menu items for the available languages

                    // Get settings menu item
                    var tmiSettings =
                        (ToolStripMenuItem)menuStrip1.Items["settingsToolStripMenuItem"];
                    // Get language menu item
                    var tmiLanguage =
                        (ToolStripMenuItem)tmiSettings.DropDownItems["languageToolStripMenuItem"];
                    // Get possible languages and add them to the menu
                    var strLanguages = Language.GetAvailableLanguages();
                    // Add available to the menu
                    foreach (var strLanguage in strLanguages)
                    {
                        var tmiLanguageAdd = new ToolStripMenuItem(strLanguage, null, LanguageClick,
                            $"languageToolStripMenuItem{strLanguage}");

                        if (strLanguage == LanguageName)
                            tmiLanguageAdd.Checked = true;

                        tmiLanguage.DropDownItems.Add(tmiLanguageAdd);
                    }

                    #endregion Add language menu items for the available languages

                    #region Set share object unit and percentage unit

                    ShareObject.PercentageUnit = Language.GetLanguageTextByXPath(@"/PercentageUnit", LanguageName);
                    ShareObject.PieceUnit = Language.GetLanguageTextByXPath(@"/PieceUnit", LanguageName);

                    #endregion Set share object unit and percentage unit
                }
                else
                {
#if DEBUG
                    var message = $"{Helper.GetMyMethodName()}\n\n{Language.LastException.Message}";
                    MessageBox.Show(message,
                        @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    InitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage, @"Could not load '" + LanguageFileName + @"' file!",
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message,
                    @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage, @"Could not load '" + LanguageFileName + @"' file!",
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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

                Text = Language.GetLanguageTextByXPath(@"/Application/Name", LanguageName)
                    + @" " + Helper.GetApplicationVersion();

                if (_portfolioFileName != "")
                {
                    Text += @" - (" + _portfolioFileName + @")";
                }

                #endregion Application name

                #region Notify icon

                _notifyIcon.Text = Language.GetLanguageTextByXPath(@"/Application/Name", LanguageName)
                                   + @" " + Helper.GetApplicationVersion();

                if (_notifyContextMenuStríp != null)
                {
                    if (_notifyContextMenuStríp.Items[0] != null)
                        _notifyContextMenuStríp.Items[0].Text =
                            Language.GetLanguageTextByXPath(@"/NotifyIcon/Show", LanguageName);
                    if (_notifyContextMenuStríp.Items[1] != null)
                        _notifyContextMenuStríp.Items[1].Text =
                            Language.GetLanguageTextByXPath(@"/NotifyIcon/Exit", LanguageName);
                }

                #endregion Notify icon

                #region Menu

                fileToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/Header", LanguageName);
                newToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/New", LanguageName);
                saveAsToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/SaveAs", LanguageName);
                openToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/Open", LanguageName);
                exitToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/Quit", LanguageName);

                settingsToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Header",
                    LanguageName);
                languageToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Language",
                    LanguageName);
                loggerToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Logger",
                    LanguageName);

                helpToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/Help/Header", LanguageName);
                aboutToolStripMenuItem.Text = Language.GetLanguageTextByXPath(@"/MainForm/Menu/Help/About", LanguageName);

                #endregion Menu

                #region GrpBox overviews

                grpBoxSharePortfolio.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Caption", LanguageName);

                #region TabControl overviews

                tabCtrlShareOverviews.TabPages[0].Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/Caption", LanguageName);

                #region  DataGirdView for the complete depot value

                if (dgvPortfolioFinalValue.Columns.Count == (int)ColumnIndicesPortfolioFinalValue.EShareSumColumnIndex + 1)
                {
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EWknNumberColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_WKN", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareNameColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Name", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePriceColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Price", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareVolumeColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Volume", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceDayBeforeColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PrevDay", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareCostsDividendColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_CostsDividend", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Performance", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareSumColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue", LanguageName);
                }

                #endregion  DataGirdView for the complete depot value

                #region DataGirdView footer complete depot values

                if (dgvPortfolioFooterFinalValue.RowCount > 0)
                {
                    dgvPortfolioFooterFinalValue.Rows[0].Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Value =
                        $@"{
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPurchaseValue",
                                LanguageName)
                        }";
                    dgvPortfolioFooterFinalValue.Rows[1].Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelCostsDividendIndex].Value =
                        $@"{
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalCostDividend",
                                LanguageName)
                        }";
                    dgvPortfolioFooterFinalValue.Rows[1].Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Value =
                        $@"{
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPerformance",
                                LanguageName)
                        }";
                    dgvPortfolioFooterFinalValue.Rows[2].Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Value =
                        $@"{
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalDepotValue",
                                LanguageName)
                        }";
                }

                #endregion DataGirdView footer complete depot values

                tabCtrlShareOverviews.TabPages[1].Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/Caption", LanguageName);

                #region  DataGirdView for the market values

                // TODO

                #endregion DataGridView for the market values

                #region  DataGirdView footer market values

                // TODO

                #endregion DataGridView footer market values

                #endregion TabControl overviews

                #region Buttons

                btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll",
                    LanguageName);
                btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh",
                    LanguageName);
                btnAdd.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Add", LanguageName);
                btnEdit.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Edit", LanguageName);
                btnDelete.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Delete",
                    LanguageName);
                btnClearLogger.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/ResetLogger",
                    LanguageName);

                #endregion Buttons

                #endregion GrpBox overviews

                #region GrpBox for the details

                grpBoxShareDetails.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/Caption",
                    LanguageName);

                if (tabCtrlDetails.TabPages[_tabPageDetailsFinalValue] != null)
                {
                    tabCtrlDetails.TabPages[_tabPageDetailsFinalValue].Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Caption",
                            LanguageName);
                    lblDetailsFinalValueDate.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/Date",
                            LanguageName);
                    lblDetailsFinalValueVolume.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/Volume",
                            LanguageName);
                    lblDetailsFinalValueDividend.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalDividend",
                            LanguageName);
                    lblDetailsFinalValueCost.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalCosts",
                            LanguageName);
                    lblDetailsFinaValueCurPrice.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/CurrentPrice",
                            LanguageName);
                    lblDetailsFinalValueDiffPerformancePrev.Text =
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/DiffPerformancePrevDay", LanguageName);
                    lblDetailsFinalValueDiffSumPrev.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/DiffSumPrevDay",
                            LanguageName);
                    lblDetailsFinalValuePrevPrice.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/PricePrevDay",
                            LanguageName);
                    lblDetailsPurchase.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/Purchase",
                            LanguageName);
                    lblDetailsFinalValuePerformance.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalPerformance",
                            LanguageName);
                    lblDetailsFinalVinalTotalProfit.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalProfitLoss",
                            LanguageName);
                    lblDetailsFinalValueTotalSum.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Labels/TotalSum",
                            LanguageName);
                }

                if (tabCtrlDetails.TabPages[_tabPageDetailsMarketValue] != null)
                {
                    tabCtrlDetails.TabPages[_tabPageDetailsMarketValue].Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Caption",
                            LanguageName);
                    lblDetailsMarketValueDate.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/Date",
                            LanguageName);
                    lblDetailsMarketValueVolume.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/Volume",
                            LanguageName);
                    lblDetailsMarketValueDividend.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalDividend",
                            LanguageName);
                    lblDetailsMarketValueCost.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalCosts",
                            LanguageName);
                    lblDetailsMarketValueCurPrice.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/CurrentPrice",
                            LanguageName);
                    lblDetailsMarketValueDiffPerformancePrev.Text =
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/DiffPerformancePrevDay", LanguageName);
                    lblDetailsMarketValueDiffSumPrev.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/DiffSumPrevDay",
                            LanguageName);
                    lblDetailsMarketValuePrevPrice.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/PricePrevDay",
                            LanguageName);
                    lblDetailsMarketValuePurchase.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/Purchase",
                            LanguageName);
                    lblDetailsMarketValueTotalPerformance.Text =
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalPerformance", LanguageName);
                    lblDetailsMarketValueTotalProfit.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalProfitLoss",
                            LanguageName);
                    lblDetailsMarketValueTotalSum.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Labels/TotalSum",
                            LanguageName);
                }

                if (tabCtrlDetails.TabPages[_tabPageDetailsDividendValue] != null)
                {
                    tabCtrlDetails.TabPages[_tabPageDetailsDividendValue].Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", LanguageName);
                    tabCtrlDetails.TabPages[_tabPageDetailsCostsValue].Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Caption", LanguageName);
                    tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue].Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", LanguageName);
                }

                #endregion GrpBox for the details

                #region GrpBox status message

                grpBoxStatusMessage.Text = Language.GetLanguageTextByXPath(
                    @"/MainForm/GrpBoxStatusMessage/Caption", LanguageName);

                #endregion GrpBox status message

                #region GrpBox update state

                grpBoxUpdateState.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxUpdateState/Caption",
                    LanguageName);

                #endregion GrpBox update state

                #region Logger language

                // Clear state list
                LoggerStatelList.Clear();

                // Add state names
                LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Start", LanguageName));
                LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Info", LanguageName));
                LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Warning", LanguageName));
                LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Error", LanguageName));
                LoggerStatelList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", LanguageName));

                // Clear component names
                LoggerComponentNamesList.Clear();

                // Add component names
                LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application", LanguageName));
                LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/WebParser", LanguageName));
                LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", LanguageName));
                LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Logger", LanguageName));

                #endregion Logger language

                #region Set share object unit and percentage unit

                ShareObject.PercentageUnit = Language.GetLanguageTextByXPath(@"/PercentageUnit", LanguageName);
                ShareObject.PieceUnit = Language.GetLanguageTextByXPath(@"/PieceUnit", LanguageName);

                #endregion Set share object unit and percentage unit
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                if (Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", LanguageName) !=
                    Language.InvalidLanguageKeyValue)
                {
                    // Set status message
                    lblWebParserState.Text = Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", LanguageName);
                    //Helper.AddStatusMessage(richTextBox1, statusMessage, Color.Red);

                    // Write log
                    Logger.AddEntry(Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", LanguageName), (Logger.ELoggerStateLevels)EStateLevels.Start, (Logger.ELoggerComponentLevels)EComponentLevels.LanguageHandler);
                }
                else
                {
                    // Set status message
                    lblWebParserState.Text = lblWebParserState.Text = @"Could not set all language keys to the controls!";
                    //Helper.AddStatusMessage(richTextBox1, statusMessage, Color.Red);

                    // Write log
                    Logger.AddEntry(@"Could not set all language keys to the controls!", (Logger.ELoggerStateLevels)EStateLevels.Start, (Logger.ELoggerComponentLevels)EComponentLevels.LanguageHandler);
                }

                // Update control list
                EnableDisableControlNames.Add("btnRefreshAll");
                EnableDisableControlNames.Add("btnRefresh");
                EnableDisableControlNames.Add("menuStrip1");

                // Disable controls
                Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                // Set initialization flag
                InitFlag = false;
            }
        }

        #endregion Set language
    }
}