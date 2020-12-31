using System;
using System.Drawing;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Configurations;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Set language

        /// <summary>
        /// This function loads the language key values to the Main form dialog
        /// </summary>
        private void SetLanguage()
        {
            try
            {
                #region Application name

                Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Application/Name", SettingsConfiguration.LanguageName)
                       + @" " + Helper.GetApplicationVersion();

                if (SettingsConfiguration.PortfolioName != "")
                {
                    Text += @" - (" + SettingsConfiguration.PortfolioName + @")";
                }

                #endregion Application name

                #region Notify icon

                if (_notifyIcon != null)
                {
                    _notifyIcon.Text =
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Application/Name", SettingsConfiguration.LanguageName)
                        + @" " + Helper.GetApplicationVersion();

                    if (_notifyContextMenuStrip != null)
                    {
                        if (_notifyContextMenuStrip.Items[0] != null)
                            _notifyContextMenuStrip.Items[0].Text =
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/NotifyIcon/Show",
                                    SettingsConfiguration.LanguageName);
                        if (_notifyContextMenuStrip.Items[1] != null)
                            _notifyContextMenuStrip.Items[1].Text =
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/NotifyIcon/Exit",
                                    SettingsConfiguration.LanguageName);
                    }
                }

                #endregion Notify icon

                #region Menu

                fileToolStripMenuItem.Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/Header", SettingsConfiguration.LanguageName);
                newToolStripMenuItem.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/New", SettingsConfiguration.LanguageName);
                saveAsToolStripMenuItem.Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/SaveAs", SettingsConfiguration.LanguageName);
                openToolStripMenuItem.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/Open", SettingsConfiguration.LanguageName);
                exitToolStripMenuItem.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/File/Quit", SettingsConfiguration.LanguageName);

                settingsToolStripMenuItem.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Header",
                    SettingsConfiguration.LanguageName);
                languageToolStripMenuItem.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Language",
                    SettingsConfiguration.LanguageName);
                loggerToolStripMenuItem.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Logger",
                    SettingsConfiguration.LanguageName);
                soundsToolStripMenuItem.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/Settings/Sounds",
                    SettingsConfiguration.LanguageName);

                helpToolStripMenuItem.Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/Help/Header", SettingsConfiguration.LanguageName);
                aboutToolStripMenuItem.Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Menu/Help/About", SettingsConfiguration.LanguageName);

                #endregion Menu

                #region GrpBox overviews

                // Set group box caption
                grpBoxSharePortfolio.Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Caption", SettingsConfiguration.LanguageName) +
                    @" ( " +
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Entries", SettingsConfiguration.LanguageName) +
                    @": " +
                    ShareObjectListFinalValue.Count + @" )";

                #region TabControl overviews

                tabCtrlShareOverviews.TabPages[0].Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/Caption",
                        SettingsConfiguration.LanguageName);

                tabCtrlShareOverviews.TabPages[1].Text =
                    LanguageConfiguration.Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/Caption",
                        SettingsConfiguration.LanguageName);

                #region DataGirdView for the market value / complete depot

                if (dgvPortfolioMarketValue.Columns.Count ==
                    (int)ColumnIndicesPortfolioMarketValue.EPortfolioMarketValueColumnCount)
                {
                    OnSetDgvPortfolioMarketValueColumnHeaderCaptions();
                }

                if (dgvPortfolioFinalValue.Columns.Count ==
                    (int)ColumnIndicesPortfolioFinalValue.EPortfolioFinalValueColumnCount)
                {
                    OnSetDgvPortfolioFinalValueColumnHeaderTexts();
                }

                #endregion DataGirdView for the market value / complete depot

                #region DataGirdView footer market value / complete depot

                if (dgvPortfolioFooterMarketValue.RowCount > 0)
                {
                    dgvPortfolioFooterMarketValue.Rows[0]
                            .Cells[(int)ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex].Value =
                        $@"{
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalPurchaseValue",
                                    SettingsConfiguration.LanguageName)
                            }";
                    dgvPortfolioFooterMarketValue.Rows[1]
                            .Cells[(int)ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex].Value =
                        $@"{
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalPerformance",
                                    SettingsConfiguration.LanguageName)
                            }";
                    dgvPortfolioFooterMarketValue.Rows[2]
                            .Cells[(int)ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex].Value =
                        $@"{
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalDepotValue",
                                    SettingsConfiguration.LanguageName)
                            }";
                }

                if (dgvPortfolioFooterFinalValue.RowCount > 0)
                {
                    dgvPortfolioFooterFinalValue.Rows[0]
                            .Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Value =
                        $@"{
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPurchaseValue",
                                    SettingsConfiguration.LanguageName)
                            }";
                    dgvPortfolioFooterFinalValue.Rows[1]
                            .Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelBrokerageDividendIndex].Value =
                        $@"{
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalBrokerageDividend",
                                    SettingsConfiguration.LanguageName)
                            }";
                    dgvPortfolioFooterFinalValue.Rows[1]
                            .Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Value =
                        $@"{
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPerformance",
                                    SettingsConfiguration.LanguageName)
                            }";
                    dgvPortfolioFooterFinalValue.Rows[2]
                            .Cells[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Value =
                        $@"{
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalDepotValue",
                                    SettingsConfiguration.LanguageName)
                            }";
                }

                #endregion DataGirdView footer market value / complete depot

                #endregion TabControl overviews

                #region Buttons

                btnRefreshAll.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll",
                    SettingsConfiguration.LanguageName);
                btnRefresh.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh",
                    SettingsConfiguration.LanguageName);
                btnAdd.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Add", SettingsConfiguration.LanguageName);
                btnEdit.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Edit", SettingsConfiguration.LanguageName);
                btnDelete.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Delete",
                    SettingsConfiguration.LanguageName);
                btnClearLogger.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/ResetLogger",
                    SettingsConfiguration.LanguageName);

                #endregion Buttons

                #endregion GrpBox overviews

                #region GrpBox status message

                grpBoxStatusMessage.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(
                    @"/MainForm/GrpBoxStatusMessage/Caption", SettingsConfiguration.LanguageName);

                #endregion GrpBox status message

                #region GrpBox document capture

                grpBoxDocumentCapture.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(
                    @"/MainForm/GrpBoxDocumentCapture/Caption", SettingsConfiguration.LanguageName);

                #endregion GrpBox document capture

                #region GrpBox update state

                grpBoxUpdateState.Text = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxUpdateState/Caption",
                    SettingsConfiguration.LanguageName);

                #endregion GrpBox update state

                #region Logger language

                // Clear state list
                LoggerStateList.Clear();

                // Add state names
                LoggerStateList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/States/Start", SettingsConfiguration.LanguageName));
                LoggerStateList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/States/Info", SettingsConfiguration.LanguageName));
                LoggerStateList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/States/Warning", SettingsConfiguration.LanguageName));
                LoggerStateList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/States/Error", SettingsConfiguration.LanguageName));
                LoggerStateList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", SettingsConfiguration.LanguageName));

                // Clear component names
                LoggerComponentNamesList.Clear();

                // Add component names
                LoggerComponentNamesList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application",
                    SettingsConfiguration.LanguageName));
                LoggerComponentNamesList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Parser",
                    SettingsConfiguration.LanguageName));
                LoggerComponentNamesList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler",
                    SettingsConfiguration.LanguageName));
                LoggerComponentNamesList.Add(LanguageConfiguration.Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Logger",
                    SettingsConfiguration.LanguageName));

                #endregion Logger language

                #region Set share object unit and percentage unit

                ShareObject.PercentageUnit = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/PercentageUnit", SettingsConfiguration.LanguageName);
                ShareObject.PieceUnit = LanguageConfiguration.Language.GetLanguageTextByXPath(@"/PieceUnit", SettingsConfiguration.LanguageName);

                #endregion Set share object unit and percentage unit
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", SettingsConfiguration.LanguageName) !=
                    LanguageConfiguration.Language.InvalidLanguageKeyValue
                        ? LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadAllLanguageKeys", SettingsConfiguration.LanguageName)
                        : @"Could not set all language keys to the controls!",
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.LanguageHandler,
                    ex);

                // Update control list
                EnableDisableControlNames.Add("btnRefreshAll");
                EnableDisableControlNames.Add("btnRefresh");
                EnableDisableControlNames.Add("menuStrip1");

                // Disable controls
                Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                // Set initialization flag
                InitFlag = false;

                Helper.ShowExceptionMessage(ex);
            }
        }

        #endregion Set language
    }
}
