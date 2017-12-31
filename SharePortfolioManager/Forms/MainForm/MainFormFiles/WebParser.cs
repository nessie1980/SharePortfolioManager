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
using WebParser;

namespace SharePortfolioManager
{
    partial class FrmMain
    {
        #region WebParser

        /// <summary>
        /// Set up the WebParser object by
        /// attaching event handlers.
        /// </summary>
        private void InitializeWebParser()
        {
            if (InitFlag && WebParser != null)
                WebParser.OnWebParserUpdate += WebParser_UpdateGUI;
        }

        /// <summary>
        /// This function starts the update all shares process
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void RefreshAll(object sender, EventArgs e)
        {
            try
            {
                // Check if the WebParser is in idle mode
                if (WebParser != null && WebParser.WebParserInfoState.State == WebParserState.Idle)
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
                    EnableDisableControlNames.Add(@"dataGridViewSharePortfolio");
                    EnableDisableControlNames.Add(@"dataGridViewSharePortfolioFooter");
                    EnableDisableControlNames.Add(@"tabCtrlDetails");
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

                        // Start the asynchronous operation of the WebParser
                        WebParser.WebSite = ShareObjectMarketValue.WebSite;
                        WebParser.RegexList = ShareObjectMarketValue.RegexList;
                        WebParser.EncodingType = ShareObjectMarketValue.WebSiteEncodingType;
                        WebParser.StartParsing();
                    }
                    else
                    {
                        // Deselect current row
                        dgvPortfolioFinalValue.ClearSelection();

                        // Select new row
                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);

                        // Start the asynchronous operation of the WebParser
                        WebParser.WebSite = ShareObjectFinalValue.WebSite;
                        WebParser.RegexList = ShareObjectFinalValue.RegexList;
                        WebParser.EncodingType = ShareObjectFinalValue.WebSiteEncodingType;
                        WebParser.StartParsing();
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
                MessageBox.Show("RefreshAll()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void Refresh(object sender, EventArgs e)
        {
            try
            {
                // Check if the WebParser is in idle mode
                if (WebParser != null && WebParser.WebParserInfoState.State == WebParserState.Idle)
                {
                    // Check if a share is selected
                    if (MarketValueOverviewTabSelected == false && dgvPortfolioFinalValue.SelectedCells.Count != 0 && dgvPortfolioFinalValue.SelectedCells[0].Value.ToString() != "" ||
                        MarketValueOverviewTabSelected == true && dgvPortfolioMarketValue.SelectedCells.Count != 0 && dgvPortfolioMarketValue.SelectedCells[0].Value.ToString() != ""
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
                            WebParser.WebSite = ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].WebSite;
                            WebParser.RegexList = ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].RegexList;
                            WebParser.EncodingType = ShareObjectListMarketValue[dgvPortfolioMarketValue.SelectedCells[0].RowIndex].WebSiteEncodingType;
                            WebParser.StartParsing();
                        }
                        else
                        {
                            // Start the asynchronous operation.
                            WebParser.WebSite = ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].WebSite;
                            WebParser.RegexList = ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].RegexList;
                            WebParser.EncodingType = ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedCells[0].RowIndex].WebSiteEncodingType;
                            WebParser.StartParsing();
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
                MessageBox.Show("Refresh()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void CancelWebParser(object sender, EventArgs e)
        {
            if (WebParser != null)
                WebParser.CancelThread = true;
        }

        /// <summary>
        /// This event handler updates the progress.
        /// </summary>
        /// <param name="sender">BackBroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void WebParser_UpdateGUI(object sender, OnWebParserUpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => WebParser_UpdateGUI(sender, e)));
            }
            else
            {
                try
                {
                    string ShareName = @"";

                    if (MarketValueOverviewTabSelected)
                        ShareName = ShareObjectMarketValue.Name;
                    else
                        ShareName = ShareObjectFinalValue.Name;

                    switch (e.WebParserInfoState.LastErrorCode)
                    {
                        case WebParserErrorCodes.Finished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Finish1", LanguageName) +
                                    ShareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Finish2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);

                                // Check if a share is selected
                                if (ShareObjectFinalValue != null && ShareObjectMarketValue != null)
                                {
                                    DateTime dtTryParse;
                                    ShareObjectFinalValue.LastUpdateInternet = DateTime.Now;
                                    ShareObjectMarketValue.LastUpdateInternet = DateTime.Now;

                                    if (e.WebParserInfoState.SearchResult.ContainsKey("LastDate"))
                                        if (DateTime.TryParse(e.WebParserInfoState.SearchResult["LastDate"][0], out dtTryParse))
                                        {
                                            ShareObjectMarketValue.LastUpdateDate = dtTryParse;
                                            ShareObjectFinalValue.LastUpdateDate = dtTryParse;
                                        }
                                        else
                                        {
                                            ShareObjectMarketValue.LastUpdateDate = DateTime.Parse(e.WebParserInfoState.SearchResult["LastDate"][0]);
                                            ShareObjectFinalValue.LastUpdateDate = DateTime.Parse(e.WebParserInfoState.SearchResult["LastDate"][0]);
                                        }

                                    if (e.WebParserInfoState.SearchResult.ContainsKey("LastTime"))
                                        if (DateTime.TryParse(e.WebParserInfoState.SearchResult["LastTime"][0], out dtTryParse))
                                        {
                                            ShareObjectMarketValue.LastUpdateTime = new DateTime(1970, 1, 1, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Second);
                                            ShareObjectFinalValue.LastUpdateTime = new DateTime(1970, 1, 1, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Second);
                                        }
                                        else
                                        {
                                            ShareObjectMarketValue.LastUpdateTime = new DateTime(0, 1, 1, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Second);
                                            ShareObjectFinalValue.LastUpdateTime = new DateTime(0, 1, 1, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Second);
                                        }

                                    if (e.WebParserInfoState.SearchResult.ContainsKey("Price"))
                                    {
                                        ShareObjectMarketValue.CurPrice = Convert.ToDecimal(e.WebParserInfoState.SearchResult["Price"][0]);
                                        ShareObjectFinalValue.CurPrice = Convert.ToDecimal(e.WebParserInfoState.SearchResult["Price"][0]);
                                    }

                                    if (e.WebParserInfoState.SearchResult.ContainsKey("PriceBefore"))
                                    {
                                        ShareObjectMarketValue.PrevDayPrice = Convert.ToDecimal(e.WebParserInfoState.SearchResult["PriceBefore"][0]);
                                        ShareObjectFinalValue.PrevDayPrice = Convert.ToDecimal(e.WebParserInfoState.SearchResult["PriceBefore"][0]);
                                    }

                                    // Save the share values to the XML
                                    Exception exception = null;
                                    if (!ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue, ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, PortfolioFileName, out exception))
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

                                if (UpdateAllFlag == true)
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
                                            WebParser.WebSite = ShareObjectMarketValue.WebSite;
                                            WebParser.RegexList = ShareObjectMarketValue.RegexList;
                                            WebParser.EncodingType = ShareObjectMarketValue.WebSiteEncodingType;
                                            WebParser.StartParsing();
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
                                            WebParser.WebSite = ShareObjectFinalValue.WebSite;
                                            WebParser.RegexList = ShareObjectFinalValue.RegexList;
                                            WebParser.EncodingType = ShareObjectFinalValue.WebSiteEncodingType;
                                            WebParser.StartParsing();
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
                        case WebParserErrorCodes.SearchFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchFinish", LanguageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.SearchRunning:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchRunning", LanguageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.SearchStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchStarted", LanguageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.ContentLoadFinished:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/ContentLoaded", LanguageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                   Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.ContentLoadStarted:
                            {
                                // Add status message
                                Helper.AddStatusMessage(lblWebParserState,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/ContentLoadStarted", LanguageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.Started:
                            {
                                lblShareNameWebParser.Text = ShareObjectFinalValue.Name;
                                progressBarWebParser.Value = e.WebParserInfoState.Percentage;
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Start1", LanguageName) +
                                    ShareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Start2", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.Starting:
                            {
                                lblShareNameWebParser.Text = @"";
                                lblWebParserState.Text = @"";
                                progressBarWebParser.Value = 0;
                                break;
                            }
                        case WebParserErrorCodes.NoError:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    "",
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.StartFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.BusyFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/BusyFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.InvalidWebSiteGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/InvalidWebSiteGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.NoRegexListGiven:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/NoRegexListGiven", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.NoWebContentLoaded:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ContentLoadedFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.ParsingFailed:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ParsingFailed1", LanguageName) +
                                    e.WebParserInfoState.LastRegexListKey +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ParsingFailed2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.CancelThread:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/CancelThread1", LanguageName) +
                                    ShareName +
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/CancelThread2", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.WebExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/Failure", LanguageName) +
                                    e.WebParserInfoState.Exception.Message,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.WebParser);
                                break;
                            }
                        case WebParserErrorCodes.ExceptionOccured:
                            {
                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/Failure", LanguageName) +
                                    e.WebParserInfoState.Exception.Message,
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.WebParser);
                                break;
                            }
                    }

                    if (WebParser.LastErrorCode > 0)
                        Thread.Sleep(100);

                    // Check if a error occurred or the process has been finished
                    if (e.WebParserInfoState.LastErrorCode < 0 || e.WebParserInfoState.LastErrorCode == WebParserErrorCodes.Finished)
                    {
                        Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                        if (e.WebParserInfoState.LastErrorCode < 0)
                        {
                            // Reset labels
                            lblShareNameWebParser.Text = @"";
                            lblWebParserState.Text = @"";
                        }

                        btnRefreshAll.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                        btnRefresh.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                    }

                    progressBarWebParser.Value = e.WebParserInfoState.Percentage;
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("webParser_UpdateGUI()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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

        #endregion WebParser
    }
}
