//MIT License
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

using Parser;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using System;
using System.Drawing;
using System.Threading;
#if DEBUG
using System.Windows.Forms;
#endif
using SharePortfolioManager.Properties;

namespace SharePortfolioManager
{
    partial class FrmMain
    {
        /// <summary>
        /// Set up the Parser object for the web parsing by
        /// attaching event handlers.
        /// </summary>
        private void InitializeParser()
        {
            // Set market values delegate
            if (InitFlag && ParserMarketValues != null)
                ParserMarketValues.OnParserUpdate += ParserMarketValues_UpdateGUI;

            // Set daily values delegate
            if (InitFlag && ParserDailyValues != null)
                ParserDailyValues.OnParserUpdate += ParserDailyValues_UpdateGUI;
        }

        /// <summary>
        /// This function starts the update all shares process
        /// </summary>
        private void RefreshAll()
        {
            try
            {
                // Check if the Parser is in idle mode
                if (ParserMarketValues != null && ParserMarketValues.ParserInfoState.State == ParserState.Idle)
                {
                    // Set flag for updating all shares
                    UpdateAllFlag = true;

                    // Rename the button
                    btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAllCancel", LanguageName);
                    btnRefreshAll.Image = Resources.button_cancel_24;

                    // Disable controls
                    EnableDisableControlNames.Clear();
                    EnableDisableControlNames.Add(@"menuStrip1");
                    EnableDisableControlNames.Add(@"btnRefresh");
                    EnableDisableControlNames.Add(@"btnAdd");
                    EnableDisableControlNames.Add(@"btnEdit");
                    EnableDisableControlNames.Add(@"btnDelete");
                    EnableDisableControlNames.Add(@"btnClearLogger");
                    EnableDisableControlNames.Add(@"grpBoxShareDetails");
                    EnableDisableControlNames.Add(@"tabCtrlShareOverviews");
                    Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                    // Check which share overview is selected
                    if (MarketValueOverviewTabSelected)
                    {
                        do
                        {
                            // Deselect current row
                            dgvPortfolioMarketValue.ClearSelection();

                            // Select new row
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            // Check if the current share should not be updated so check the next share
                            if (!ShareObjectMarketValue.Update || !ShareObjectMarketValue.WebSiteConfigurationValid)
                                SelectedDataGridViewShareIndex++;

                        } while (!ShareObjectMarketValue.Update ||
                                 !ShareObjectMarketValue.WebSiteConfigurationValid &&
                                 SelectedDataGridViewShareIndex < ShareObject.ObjectCounter);

                        // Check if the share should be update
                        if (ShareObjectMarketValue.Update && ShareObjectMarketValue.WebSiteConfigurationValid)
                        {
                            // Start the asynchronous operation of the Parser for the market values
                            ParserMarketValues.ParsingValues = new ParsingValues(
                                new Uri(ShareObjectMarketValue.WebSite),
                                ShareObjectMarketValue.WebSiteEncodingType,
                                ShareObjectMarketValue.RegexList
                                );
                            ParserMarketValues.StartParsing();

                            // Start the asynchronous operation of the Parser for the daily market values
                            ParserDailyValues.ParsingValues = new ParsingValues(
                                new Uri(Helper.BuildDailyValuesUrl(ShareObjectMarketValue.DailyValues, ShareObjectMarketValue.DailyValuesWebSite, ShareObjectMarketValue.ShareType)),
                                ShareObjectMarketValue.WebSiteEncodingType
                            );
                            ParserDailyValues.StartParsing();
                        }
                        else
                        {
                            EnableDisableControlNames.Remove(@"btnRefreshAll");
                            EnableDisableControlNames.Remove(@"btnRefresh");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            UpdateAllFlag = false;

                            // Reset index
                            SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex]
                                .Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue,
                                SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex,
                                true);

                            btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                            btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                            btnRefreshAll.Image = Resources.button_update_all_24;
                            btnRefresh.Image = Resources.button_update_24;
                        }
                    }
                    else
                    {
                        do
                        {
                            // Deselect current row
                            dgvPortfolioFinalValue.ClearSelection();

                            // Select new row
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                            // Check if the current share should not be updated so check the next share
                            if (!ShareObjectFinalValue.Update || !ShareObjectFinalValue.WebSiteConfigurationValid)
                                SelectedDataGridViewShareIndex++;

                        } while (!ShareObjectFinalValue.Update ||
                                 !ShareObjectFinalValue.WebSiteConfigurationValid &&
                                 SelectedDataGridViewShareIndex < ShareObject.ObjectCounter);

                        // Check if the share should be update
                        if (ShareObjectFinalValue.Update && ShareObjectFinalValue.WebSiteConfigurationValid)
                        {
                            // Start the asynchronous operation of the Parser for the market values
                            ParserMarketValues.ParsingValues = new ParsingValues(
                                new Uri(ShareObjectFinalValue.WebSite),
                                ShareObjectFinalValue.WebSiteEncodingType,
                                ShareObjectFinalValue.RegexList
                                );
                            ParserMarketValues.StartParsing();

                            // Start the asynchronous operation of the Parser for the daily market values
                            ParserDailyValues.ParsingValues = new ParsingValues(
                                new Uri(Helper.BuildDailyValuesUrl(ShareObjectFinalValue.DailyValues, ShareObjectFinalValue.DailyValuesWebSite, ShareObjectFinalValue.ShareType)),
                                ShareObjectFinalValue.WebSiteEncodingType
                            );
                            ParserDailyValues.StartParsing();
                        }
                        else
                        {
                            EnableDisableControlNames.Remove(@"btnRefreshAll");
                            EnableDisableControlNames.Remove(@"btnRefresh");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            UpdateAllFlag = false;

                            // Reset index
                            SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex]
                                .Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                                SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex,
                                true);

                            btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                            btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                            btnRefreshAll.Image = Resources.button_update_all_24;
                            btnRefresh.Image = Resources.button_update_24;
                        }
                    }
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                }
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
                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function starts the update single share process
        /// </summary>
        public void RefreshOne()
        {
            try
            {
                // Check if the Parser is in idle mode
                if (ParserMarketValues != null && ParserMarketValues.ParserInfoState.State == ParserState.Idle)
                {
                    // Check if a share is selected
                    if (MarketValueOverviewTabSelected == false && dgvPortfolioFinalValue.SelectedCells.Count != 0 && dgvPortfolioFinalValue.SelectedCells[0].Value.ToString() != "" ||
                        MarketValueOverviewTabSelected && dgvPortfolioMarketValue.SelectedCells.Count != 0 && dgvPortfolioMarketValue.SelectedCells[0].Value.ToString() != ""
                       )
                    {
                        // Reset flag for updating all shares
                        UpdateAllFlag = false;

                        btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshCancel", LanguageName);
                        btnRefresh.Image = Resources.button_cancel_24;

                        // Disable controls
                        EnableDisableControlNames.Clear();
                        EnableDisableControlNames.Add(@"menuStrip1");
                        EnableDisableControlNames.Add(@"btnRefreshAll");
                        EnableDisableControlNames.Add(@"btnAdd");
                        EnableDisableControlNames.Add(@"btnEdit");
                        EnableDisableControlNames.Add(@"btnDelete");
                        EnableDisableControlNames.Add(@"btnClearLogger");
                        EnableDisableControlNames.Add(@"dataGridViewSharePortfolio");
                        EnableDisableControlNames.Add(@"dataGridViewSharePortfolioFooter");
                        EnableDisableControlNames.Add(@"tabCtrlDetails");
                        Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                        // Check which share overview is selected
                        if (MarketValueOverviewTabSelected)
                        {
                            // Start the asynchronous operation of the Parser for the market values
                            ParserMarketValues.ParsingValues = new ParsingValues(
                                new Uri(ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].WebSite),
                                ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].WebSiteEncodingType,
                                ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].RegexList
                                );
                            ParserMarketValues.StartParsing();

                            // Start the asynchronous operation of the Parser for the daily market values
                            ParserDailyValues.ParsingValues = new ParsingValues(
                                new Uri(Helper.BuildDailyValuesUrl(
                                    ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].DailyValues, 
                                    ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].DailyValuesWebSite,
                                    ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].ShareType
                                    )),
                                ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].WebSiteEncodingType
                                );
                            ParserDailyValues.StartParsing();
                        }
                        else
                        {
                            // Start the asynchronous operation of the Parser for the market values
                            ParserMarketValues.ParsingValues = new ParsingValues(
                                new Uri(ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].WebSite),
                                ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].WebSiteEncodingType,
                                ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].RegexList
                                );
                            ParserMarketValues.StartParsing();

                            // Start the asynchronous operation of the Parser for the daily market values
                            ParserDailyValues.ParsingValues = new ParsingValues(
                                new Uri(Helper.BuildDailyValuesUrl(
                                    ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].DailyValues,
                                    ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].DailyValuesWebSite,
                            ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].ShareType
                                    )),
                                ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].WebSiteEncodingType
                            );
                            ParserDailyValues.StartParsing();
                        }
                    }
                    else
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", LanguageName),
                            Language, LanguageName,
                            Color.Orange, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                    }
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                }
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
                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function stops the update process
        /// </summary>
        private void CancelWebParser()
        {
            if (ParserMarketValues != null)
                ParserMarketValues.CancelThread = true;

            if (ParserDailyValues != null)
                ParserDailyValues.CancelThread = true;
        }

        /// <summary>
        /// This event handler updates the progress of the market values.
        /// </summary>
        /// <param name="sender">BackGroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void ParserMarketValues_UpdateGUI(object sender, OnParserUpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ParserMarketValues_UpdateGUI(sender, e)));
            }
            else
            {
                try
                {
                    var shareName = MarketValueOverviewTabSelected ? ShareObjectMarketValue.Name : ShareObjectFinalValue.Name;

                    switch (e.ParserInfoState.LastErrorCode)
                    {
                        case ParserErrorCodes.Finished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Finish_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Finish_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);

                                // Check if a share is selected
                                if (ShareObjectFinalValue != null && ShareObjectMarketValue != null)
                                {
                                    ShareObjectFinalValue.LastUpdateInternet = DateTime.Now;
                                    ShareObjectMarketValue.LastUpdateInternet = DateTime.Now;

                                    if (e.ParserInfoState.SearchResult.ContainsKey("LastDate") &&
                                        e.ParserInfoState.SearchResult.ContainsKey("LastTime"))
                                    {
                                        var dateTime =
                                            $@"{e.ParserInfoState.SearchResult["LastDate"][0]} {e.ParserInfoState.SearchResult["LastTime"][0]}";

                                        if (DateTime.TryParse(dateTime, out var dtTryParse))
                                        {
                                            ShareObjectMarketValue.LastUpdateShare = dtTryParse;
                                            ShareObjectFinalValue.LastUpdateShare = dtTryParse;
                                        }
                                        else
                                        {
                                            ShareObjectMarketValue.LastUpdateShare =
                                                DateTime.Parse(dateTime);
                                            ShareObjectFinalValue.LastUpdateShare =
                                                DateTime.Parse(dateTime);
                                        }
                                    }

                                    if (e.ParserInfoState.SearchResult.ContainsKey("Price"))
                                    {
                                        ShareObjectMarketValue.CurPrice = Convert.ToDecimal(e.ParserInfoState.SearchResult["Price"][0]);
                                        ShareObjectFinalValue.CurPrice = Convert.ToDecimal(e.ParserInfoState.SearchResult["Price"][0]);
                                    }

                                    if (e.ParserInfoState.SearchResult.ContainsKey("PriceBefore"))
                                    {
                                        ShareObjectMarketValue.PrevDayPrice = Convert.ToDecimal(e.ParserInfoState.SearchResult["PriceBefore"][0]);
                                        ShareObjectFinalValue.PrevDayPrice = Convert.ToDecimal(e.ParserInfoState.SearchResult["PriceBefore"][0]);
                                    }

                                    // Save the share values to the XML
                                    if (ParserDailyValues.ParserErrorCode == ParserErrorCodes.Finished ||
                                        ParserDailyValues.ParserErrorCode <= ParserErrorCodes.NoError
                                        )
                                    {
                                        if (!ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue,
                                            ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio,
                                            _portfolioFileName, out var exception))
                                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                                exception.Message,
                                                Language, LanguageName,
                                                Color.Black, Logger, (int) EStateLevels.Info,
                                                (int) EComponentLevels.Application);
                                    }

                                    // Save last displayed DataGridView row
                                    if (MarketValueOverviewTabSelected)
                                    {
                                        if (dgvPortfolioMarketValue.FirstDisplayedCell != null)
                                            LastFirstDisplayedRowIndex = dgvPortfolioMarketValue.FirstDisplayedCell.RowIndex;
                                    }
                                    else
                                    {
                                        if (dgvPortfolioFinalValue.FirstDisplayedCell != null)
                                            LastFirstDisplayedRowIndex = dgvPortfolioFinalValue.FirstDisplayedCell.RowIndex;
                                    }

                                    // Update row with the new ShareObject values
                                    dgvPortfolioFinalValue.Refresh();
                                    dgvPortfolioMarketValue.Refresh();

                                    // Refresh the footer
                                    RefreshFooters();

                                    // Reset share add flag
                                    AddFlagMarketValue = false;
                                    AddFlagFinalValue = false;
                                }

                                if (UpdateAllFlag)
                                {
                                    // Check which share overview is selected
                                    if (MarketValueOverviewTabSelected)
                                    {
                                        // Select the new share update
                                        dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                        // Scroll to the selected row
                                        Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                                    }
                                    else
                                    {
                                        // Select the new share update
                                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                        // Scroll to the selected row
                                        Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                                            SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                                    }

                                    // Check if another share object should be updated
                                    if (ParserDailyValues.ParserErrorCode == ParserErrorCodes.Finished ||
                                        ParserDailyValues.ParserErrorCode <= ParserErrorCodes.NoError
                                        )
                                    {

                                        if (SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1)
                                            timerStartNextShareUpdate.Enabled = true;

                                        if (SelectedDataGridViewShareIndex == ShareObject.ObjectCounter - 1)
                                            timerStatusMessageClear.Enabled = true;
                                    }
                                }
                                else
                                {
                                    if (MarketValueOverviewTabSelected)
                                    {
                                        // Clear current selection
                                        dgvPortfolioMarketValue.ClearSelection();

                                        // Select the new share update
                                        dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;
                                    }
                                    else
                                    {
                                        // Clear current selection
                                        dgvPortfolioFinalValue.ClearSelection();

                                        // Select the new share update
                                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;
                                    }

                                    // Check if all parsing is done
                                    if (ParserDailyValues.ParserErrorCode == ParserErrorCodes.Finished ||
                                        ParserDailyValues.ParserErrorCode <= ParserErrorCodes.NoError
                                        )
                                        timerStatusMessageClear.Enabled = true;
                                }
                                break;
                            }
                        case ParserErrorCodes.SearchFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchFinish", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.SearchRunning:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchRunning", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.SearchStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchStarted", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ContentLoadFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/ContentLoaded", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                   Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);

                                Console.WriteLine(@"Content: {0}", e.ParserInfoState.WebSiteContentAsString);
                                break;
                            }
                        case ParserErrorCodes.ContentLoadStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/ContentLoadStarted", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.Started:
                            {
                                // Just wait some time before the update begins
                                Thread.Sleep(250);
                                progressBarWebParserMarketValues.Value = e.ParserInfoState.Percentage;
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Start_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Start_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.Starting:
                            {
                                lblWebParserMarketValuesState.Text = @"";
                                progressBarWebParserMarketValues.Value = 0;
                                break;
                            }
                        case ParserErrorCodes.NoError:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    "",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.StartFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/StartFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.BusyFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/BusyFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.InvalidWebSiteGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/InvalidWebSiteGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.NoRegexListGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/NoRegexListGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.NoWebContentLoaded:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ParsingFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_1", LanguageName) +
                                    e.ParserInfoState.LastRegexListKey +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.CancelThread:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/CancelThread_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/CancelThread_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.WebExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/Failure", LanguageName) +
                                    e.ParserInfoState.Exception.Message,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/Failure", LanguageName) +
                                    e.ParserInfoState.Exception.Message +
                                    e.ParserInfoState.Exception.Source + 
                                    e.ParserInfoState.Exception.Data,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser);
                                break;
                            }
                    }

                    // Check if an error occurred or the process has been finished
                    if (e.ParserInfoState.LastErrorCode < ParserErrorCodes.NoError)
                    {
                        UpdateAllFlag = false;

                        if (MarketValueOverviewTabSelected)
                        {
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                            // Reset index
                            SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                        }
                        else
                        {
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                            // Reset index
                            SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                        }

                        if(ParserDailyValues.ParserErrorCode == ParserErrorCodes.Finished ||
                           ParserDailyValues.ParserErrorCode <= ParserErrorCodes.NoError
                           )
                            timerStatusMessageClear.Enabled = true;
                    }

                    progressBarWebParserMarketValues.Value = e.ParserInfoState.Percentage;
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
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/UpdateFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Reset labels
                    lblWebParserMarketValuesState.Text = @"";

                    btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                    btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                    btnRefreshAll.Image = Resources.button_update_all_24;
                    btnRefresh.Image = Resources.button_update_24;
                }
            }
        }

        /// <summary>
        /// This event handler updates the progress of the daily values.
        /// </summary>
        /// <param name="sender">BackGroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void ParserDailyValues_UpdateGUI(object sender, OnParserUpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ParserDailyValues_UpdateGUI(sender, e)));
            }
            else
            {
                try
                {
                    var shareName = MarketValueOverviewTabSelected ? ShareObjectMarketValue.Name : ShareObjectFinalValue.Name;

                    switch (e.ParserInfoState.LastErrorCode)
                    {
                        case ParserErrorCodes.Finished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Finish_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Finish_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);

                                // Check if a share is selected
                                if (ShareObjectFinalValue != null && ShareObjectMarketValue != null)
                                {
                                    // Only add if the date not exists already
                                    ShareObjectFinalValue.AddNewDailyValues(ParserDailyValues.ParserInfoState
                                        .DailyValuesList);

                                    // Save the share values to the XML
                                    if (ParserMarketValues.ParserErrorCode == ParserErrorCodes.Finished ||
                                        ParserMarketValues.ParserErrorCode <= ParserErrorCodes.NoError
                                        )
                                    {
                                        // Save the share values to the XML
                                        if (!ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue,
                                            ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio,
                                            _portfolioFileName, out var exception))
                                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                                exception.Message,
                                                Language, LanguageName,
                                                Color.Black, Logger, (int) EStateLevels.Info,
                                                (int) EComponentLevels.Application);
                                    }
                                }

                                if (UpdateAllFlag)
                                {
                                    // Check which share overview is selected
                                    if (MarketValueOverviewTabSelected)
                                    {
                                        // Select the new share update
                                        dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                        // Scroll to the selected row
                                        Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                                    }
                                    else
                                    {
                                        // Select the new share update
                                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                        // Scroll to the selected row
                                        Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                                            SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                                    }

                                    // Check if another share object should be updated
                                    if (ParserMarketValues.ParserErrorCode == ParserErrorCodes.Finished ||
                                        ParserMarketValues.ParserErrorCode <= ParserErrorCodes.NoError
                                    )
                                    {
                                        if (SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1)
                                            timerStartNextShareUpdate.Enabled = true;

                                        if (SelectedDataGridViewShareIndex == ShareObject.ObjectCounter - 1)
                                            timerStatusMessageClear.Enabled = true;
                                    }
                                }
                                else
                                {
                                    if (MarketValueOverviewTabSelected)
                                    {
                                        // Clear current selection
                                        dgvPortfolioMarketValue.ClearSelection();

                                        // Select the new share update
                                        dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;
                                    }
                                    else
                                    {
                                        // Clear current selection
                                        dgvPortfolioFinalValue.ClearSelection();

                                        // Select the new share update
                                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;
                                    }

                                    if (ParserMarketValues.ParserErrorCode == ParserErrorCodes.Finished ||
                                        ParserMarketValues.ParserErrorCode <= ParserErrorCodes.NoError
                                        )
                                        timerStatusMessageClear.Enabled = true;
                                }
                                break;
                            }
                        case ParserErrorCodes.SearchFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchFinish", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.SearchRunning:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchRunning", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.SearchStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchStarted", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ContentLoadFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/ContentLoaded", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                   Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);

                                Console.WriteLine(@"Content: {0}", e.ParserInfoState.WebSiteContentAsString);
                                break;
                            }
                        case ParserErrorCodes.ContentLoadStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/ContentLoadStarted", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.Started:
                            {
                                // Just wait some time before the update begins
                                Thread.Sleep(250);
                                progressBarWebParserDailyValues.Value = e.ParserInfoState.Percentage;
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Start_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Start_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.Starting:
                            {
                                lblWebParserDailyValuesState.Text = @"";
                                progressBarWebParserDailyValues.Value = 0;
                                break;
                            }
                        case ParserErrorCodes.NoError:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    "",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.StartFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/StartFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.BusyFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/BusyFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.InvalidWebSiteGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/InvalidWebSiteGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.NoRegexListGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/NoRegexListGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.NoWebContentLoaded:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ContentLoadedFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ParsingFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_1", LanguageName) +
                                    e.ParserInfoState.LastRegexListKey +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.CancelThread:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/CancelThread_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/CancelThread_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.WebExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/Failure", LanguageName) +
                                    e.ParserInfoState.Exception.Message,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/Failure", LanguageName) +
                                    e.ParserInfoState.Exception.Message,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser);
                                break;
                            }
                    }

                    // Check if an error occurred or the process has been finished
                    if (e.ParserInfoState.LastErrorCode < ParserErrorCodes.NoError)
                    {
                        UpdateAllFlag = false;

                        if (MarketValueOverviewTabSelected)
                        {
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                            // Reset index
                            SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                        }
                        else
                        {
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                            // Reset index
                            SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                        }

                        if (ParserMarketValues.ParserErrorCode == ParserErrorCodes.Finished ||
                            ParserMarketValues.ParserErrorCode <= ParserErrorCodes.NoError
                            )
                            timerStatusMessageClear.Enabled = true;
                    }

                    progressBarWebParserDailyValues.Value = e.ParserInfoState.Percentage;
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
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/UpdateFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Reset labels
                    lblWebParserDailyValuesState.Text = @"";

                    btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                    btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                    btnRefreshAll.Image = Resources.button_update_all_24;
                    btnRefresh.Image = Resources.button_update_24;
                }
            }
        }
    }
}
