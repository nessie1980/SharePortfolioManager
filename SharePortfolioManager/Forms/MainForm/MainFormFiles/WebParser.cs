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
using System.Threading;
using System.Windows.Forms;
using SharePortfolioManager.Classes.ShareObjects;
using Parser;

namespace SharePortfolioManager
{
    partial class FrmMain
    {
        #region WebParsing

        /// <summary>
        /// Set up the Parser object for the web parsing by
        /// attaching event handlers.
        /// </summary>
        private void InitializeParser()
        {
            if (InitFlag && Parser != null)
                Parser.OnParserUpdate += WebParser_UpdateGUI;
        }

        /// <summary>
        /// This function starts the update all shares process
        /// </summary>
        private void RefreshAll()
        {
            try
            {
                // Check if the Parser is in idle mode
                if (Parser != null && Parser.ParserInfoState.State == ParserState.Idle)
                {
                    // Set flag for updating all shares
                    UpdateAllFlag = true;

                    // Rename the button
                    btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAllCancel", LanguageName);

                    // Disable controls
                    EnableDisableControlNames.Clear();
                    EnableDisableControlNames.Add(@"menuStrip1");
                    EnableDisableControlNames.Add(@"btnRefresh");
                    EnableDisableControlNames.Add(@"btnAdd");
                    EnableDisableControlNames.Add(@"btnEdit");
                    EnableDisableControlNames.Add(@"btnDelete");
                    EnableDisableControlNames.Add(@"btnClearLogger");
                    EnableDisableControlNames.Add(@"grpBoxShareDetails");
                    EnableDisableControlNames.Add(@"grpBoxSharePortfolio");
                    Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                    // Check which share overview is selected
                    if (MarketValueOverviewTabSelected)
                    {
                        // Deselect current row
                        dgvPortfolioMarketValue.ClearSelection();

                        // Select new row
                        dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                        // Start the asynchronous operation of the Parser
                        Parser.WebParsing = true;
                        Parser.WebSiteUrl = ShareObjectMarketValue.WebSite;
                        Parser.RegexList = ShareObjectMarketValue.RegexList;
                        Parser.EncodingType = ShareObjectMarketValue.WebSiteEncodingType;
                        Parser.StartParsing();
                    }
                    else
                    {
                        // Deselect current row
                        dgvPortfolioFinalValue.ClearSelection();

                        // Select new row
                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                        // Start the asynchronous operation of the Parser
                        Parser.WebParsing = true;
                        Parser.WebSiteUrl = ShareObjectFinalValue.WebSite;
                        Parser.RegexList = ShareObjectFinalValue.RegexList;
                        Parser.EncodingType = ShareObjectFinalValue.WebSiteEncodingType;
                        Parser.StartParsing();
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
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
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
                if (Parser != null && Parser.ParserInfoState.State == ParserState.Idle)
                {
                    // Check if a share is selected
                    if (MarketValueOverviewTabSelected == false && dgvPortfolioFinalValue.SelectedCells.Count != 0 && dgvPortfolioFinalValue.SelectedCells[0].Value.ToString() != "" ||
                        MarketValueOverviewTabSelected && dgvPortfolioMarketValue.SelectedCells.Count != 0 && dgvPortfolioMarketValue.SelectedCells[0].Value.ToString() != ""
                       )
                    {
                        // Reset flag for updating all shares
                        UpdateAllFlag = false;

                        btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshCancel", LanguageName);

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
                            // Start the asynchronous operation.
                            Parser.WebParsing = true;
                            Parser.WebSiteUrl = ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].WebSite;
                            Parser.RegexList = ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].RegexList;
                            Parser.EncodingType = ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].WebSiteEncodingType;
                            Parser.StartParsing();
                        }
                        else
                        {
                            // Start the asynchronous operation.
                            Parser.WebParsing = true;
                            Parser.WebSiteUrl = ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].WebSite;
                            Parser.RegexList = ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].RegexList;
                            Parser.EncodingType = ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].WebSiteEncodingType;
                            Parser.StartParsing();
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
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
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
            if (Parser != null)
                Parser.CancelThread = true;
        }

        /// <summary>
        /// This event handler updates the progress.
        /// </summary>
        /// <param name="sender">BackGroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void WebParser_UpdateGUI(object sender, OnParserUpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => WebParser_UpdateGUI(sender, e)));
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
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Finish_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Finish_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);

                                // Check if a share is selected
                                if (ShareObjectFinalValue != null && ShareObjectMarketValue != null)
                                {
                                    DateTime dtTryParse;
                                    ShareObjectFinalValue.LastUpdateInternet = DateTime.Now;
                                    ShareObjectMarketValue.LastUpdateInternet = DateTime.Now;

                                    if (e.ParserInfoState.SearchResult.ContainsKey("LastDate"))
                                        if (DateTime.TryParse(e.ParserInfoState.SearchResult["LastDate"][0], out dtTryParse))
                                        {
                                            ShareObjectMarketValue.LastUpdateDate = dtTryParse;
                                            ShareObjectFinalValue.LastUpdateDate = dtTryParse;
                                        }
                                        else
                                        {
                                            ShareObjectMarketValue.LastUpdateDate = DateTime.Parse(e.ParserInfoState.SearchResult["LastDate"][0]);
                                            ShareObjectFinalValue.LastUpdateDate = DateTime.Parse(e.ParserInfoState.SearchResult["LastDate"][0]);
                                        }

                                    if (e.ParserInfoState.SearchResult.ContainsKey("LastTime"))
                                        if (DateTime.TryParse(e.ParserInfoState.SearchResult["LastTime"][0], out dtTryParse))
                                        {
                                            ShareObjectMarketValue.LastUpdateTime = new DateTime(1970, 1, 1, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Second);
                                            ShareObjectFinalValue.LastUpdateTime = new DateTime(1970, 1, 1, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Second);
                                        }
                                        else
                                        {
                                            ShareObjectMarketValue.LastUpdateTime = new DateTime(0, 1, 1, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Second);
                                            ShareObjectFinalValue.LastUpdateTime = new DateTime(0, 1, 1, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.ParserInfoState.SearchResult["LastTime"][0]).Second);
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
                                    if (!ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue, ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, _portfolioFileName, out var exception))
                                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            exception.Message,
                                            Language, LanguageName,
                                            Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

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
                                    DgvPortfolioBindingSourceFinalValue.ResetBindings(false);
                                    DgvPortfolioBindingSourceMarketValue.ResetBindings(false);

                                    // Refresh the footer
                                    RefreshFooters();

                                    // Reset share add flag
                                    AddFlagMarketValue = false;
                                    AddFlagFinalValue = false;
                                }

                                if (UpdateAllFlag)
                                {
                                    // Check if another share object should be updated
                                    if (SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1)
                                    {
                                        // Increase index to get the next share
                                        SelectedDataGridViewShareIndex++;

                                        Thread.Sleep(100);

                                        // Check which share overview is selected
                                        if (MarketValueOverviewTabSelected)
                                        {
                                            // Clear current selection
                                            dgvPortfolioMarketValue.ClearSelection();

                                            // Select the new share update
                                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                            // Scroll to the selected row
                                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

                                            // Start the asynchronous parsing operation.
                                            if (ShareObjectMarketValue != null)
                                            {
                                                Parser.WebParsing = true;
                                                Parser.WebSiteUrl = ShareObjectMarketValue.WebSite;
                                                Parser.RegexList = ShareObjectMarketValue.RegexList;
                                                Parser.EncodingType = ShareObjectMarketValue.WebSiteEncodingType;
                                            }
                                            Parser.StartParsing();
                                        }
                                        else
                                        {
                                            // Clear current selection
                                            dgvPortfolioFinalValue.ClearSelection();

                                            // Select the new share update
                                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                            // Scroll to the selected row
                                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

                                            // Start the asynchronous parsing operation.
                                            if (ShareObjectFinalValue != null)
                                            {
                                                Parser.WebParsing = true;
                                                Parser.WebSiteUrl = ShareObjectFinalValue.WebSite;
                                                Parser.RegexList = ShareObjectFinalValue.RegexList;
                                                Parser.EncodingType = ShareObjectFinalValue.WebSiteEncodingType;
                                            }
                                            Parser.StartParsing();
                                        }
                                    }
                                    else
                                    {
                                        // Reset index
                                        SelectedDataGridViewShareIndex = 0;

                                        if (MarketValueOverviewTabSelected)
                                        {
                                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                            // Scroll to the selected row
                                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                                        }
                                        else
                                        {
                                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                            // Scroll to the selected row
                                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                                        }

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
                                    timerStatusMessageClear.Enabled = true;
                                }
                                break;
                            }
                        case ParserErrorCodes.SearchFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchFinish", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.SearchRunning:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchRunning", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.SearchStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchStarted", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ContentLoadFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/ContentLoaded", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                   Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ContentLoadStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/ContentLoadStarted", LanguageName) + " ( " + e.ParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.Started:
                            {
                                lblShareNameWebParser.Text = ShareObjectFinalValue.Name;
                                progressBarWebParser.Value = e.ParserInfoState.Percentage;
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Start_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Start_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.Starting:
                            {
                                lblShareNameWebParser.Text = @"";
                                lblWebParserState.Text = @"";
                                progressBarWebParser.Value = 0;
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
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.BusyFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/BusyFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.InvalidWebSiteGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/InvalidWebSiteGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.NoRegexListGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/NoRegexListGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.NoWebContentLoaded:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ContentLoadedFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ParsingFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ParsingFailed_1", LanguageName) +
                                    e.ParserInfoState.LastRegexListKey +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ParsingFailed_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.CancelThread:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/CancelThread_1", LanguageName) +
                                    shareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/CancelThread_2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.WebExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/Failure", LanguageName) +
                                    e.ParserInfoState.Exception.Message,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser);
                                break;
                            }
                        case ParserErrorCodes.ExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/Failure", LanguageName) +
                                    e.ParserInfoState.Exception.Message,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser);
                                break;
                            }
                    }

                    if (Parser.ParserErrorCode > 0)
                        Thread.Sleep(100);

                    // Check if a error occurred or the process has been finished
                    if (e.ParserInfoState.LastErrorCode < 0 || e.ParserInfoState.LastErrorCode == ParserErrorCodes.Finished)
                    {
                        Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                        if (e.ParserInfoState.LastErrorCode < 0)
                        {
                            // Reset labels
                            lblShareNameWebParser.Text = @"";
                            lblWebParserState.Text = @"";
                        }

                        btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                        btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                    }

                    progressBarWebParser.Value = e.ParserInfoState.Percentage;
                }
                catch (Exception ex)
                {
#if DEBUG
                    var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                    MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/UpdateFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Reset labels
                    lblShareNameWebParser.Text = @"";
                    lblWebParserState.Text = @"";

                    btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                    btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                }
            }
        }

        #endregion WebParsing
    }
}
