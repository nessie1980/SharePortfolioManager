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
            if (_bInitFlag && _webParser != null)
                _webParser.OnWebParserUpdate += webParser_UpdateGUI;
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
                if (_webParser != null && _webParser.WebParserInfoState.State == WebParserState.Idle)
                {
                    // Set flag for updating all shares
                    _bUpdateAll = true;

                    // Rename the button
                    btnRefreshAll.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAllCancel", _languageName);

                    // Disable controls
                    _enableDisableControlNames.Clear();
                    _enableDisableControlNames.Add(@"menuStrip1");
                    _enableDisableControlNames.Add(@"btnRefresh");
                    _enableDisableControlNames.Add(@"btnAdd");
                    _enableDisableControlNames.Add(@"btnEdit");
                    _enableDisableControlNames.Add(@"btnDelete");
                    _enableDisableControlNames.Add(@"btnClearLogger");
                    _enableDisableControlNames.Add(@"dataGridViewSharePortfolio");
                    _enableDisableControlNames.Add(@"dataGridViewSharePortfolioFooter");
                    _enableDisableControlNames.Add(@"tabCtrlDetails");
                    Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                    // Deselect current row
                    dgvPortfolio.ClearSelection();

                    // Select new row
                    dgvPortfolio.Rows[_selectedIndex].Selected = true;

                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolio, _selectedIndex, _lastFirstDisplayedRowIndex, true);

                    // Start the asynchronous operation of the WebParser
                    _webParser.WebSite = _shareObject.WebSite;
                    _webParser.RegexList = _shareObject.RegexList;
                    _webParser.EncodingType = _shareObject.EncodingType;
                    _webParser.StartParsing();
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
                if (_webParser != null && _webParser.WebParserInfoState.State == WebParserState.Idle)
                {
                    // Check if a share is selected
                    if (dgvPortfolio.SelectedCells.Count != 0 && dgvPortfolio.SelectedCells[0].Value.ToString() != "")
                    {
                        // Reset flag for updating all shares
                        _bUpdateAll = false;

                        btnRefresh.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshCancel", _languageName);

                        // Disable controls
                        _enableDisableControlNames.Clear();
                        _enableDisableControlNames.Add(@"menuStrip1");
                        _enableDisableControlNames.Add(@"btnRefreshAll");
                        _enableDisableControlNames.Add(@"btnAdd");
                        _enableDisableControlNames.Add(@"btnEdit");
                        _enableDisableControlNames.Add(@"btnDelete");
                        _enableDisableControlNames.Add(@"btnClearLogger");
                        _enableDisableControlNames.Add(@"dataGridViewSharePortfolio");
                        _enableDisableControlNames.Add(@"dataGridViewSharePortfolioFooter");
                        _enableDisableControlNames.Add(@"tabCtrlDetails");
                        Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                        // Start the asynchronous operation.
                        _webParser.WebSite = ShareObjectList[dgvPortfolio.SelectedCells[0].RowIndex].WebSite;
                        _webParser.RegexList = ShareObjectList[dgvPortfolio.SelectedCells[0].RowIndex].RegexList;
                        _webParser.EncodingType = ShareObjectList[dgvPortfolio.SelectedCells[0].RowIndex].EncodingType;
                        _webParser.StartParsing();
                    }
                    else
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", _languageName),
                            _xmlLanguage, _languageName,
                            Color.Orange, _logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                    }
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function stops the update process
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void CancelWebParser(object sender, EventArgs e)
        {
            if (_webParser != null)
                _webParser.CancelThread = true;
        }

        /// <summary>
        /// This event handler updates the progress.
        /// </summary>
        /// <param name="sender">BackBroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void webParser_UpdateGUI(object sender, OnWebParserUpdateEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => webParser_UpdateGUI(sender, e)));
            }
            else
            {
                try
                {
                    switch (e.WebParserInfoState.LastErrorCode)
                    {
                        case WebParserErrorCodes.Finished:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Finish1", _languageName) +
                                _shareObject.Name +
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Finish2", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);

                            // Check if a share is selected
                            if (_shareObject != null)
                            {
                                DateTime dtTryParse;
                                _shareObject.LastUpdateInternet = DateTime.Now;
                                if (e.WebParserInfoState.SearchResult.ContainsKey("LastDate"))
                                    if (DateTime.TryParse(e.WebParserInfoState.SearchResult["LastDate"][0], out dtTryParse))
                                        _shareObject.LastUpdateDate = dtTryParse;
                                    else
                                        _shareObject.LastUpdateDate = DateTime.Parse(e.WebParserInfoState.SearchResult["LastDate"][0]);

                                if (e.WebParserInfoState.SearchResult.ContainsKey("LastTime"))
                                    if (DateTime.TryParse(e.WebParserInfoState.SearchResult["LastTime"][0], out dtTryParse))
                                        _shareObject.LastUpdateTime = new DateTime(1970, 1, 1, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Second);
                                    else
                                        _shareObject.LastUpdateTime = new DateTime(0, 1, 1, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Hour, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Minute, DateTime.Parse(e.WebParserInfoState.SearchResult["LastTime"][0]).Second);

                                if (e.WebParserInfoState.SearchResult.ContainsKey("Price"))
                                    _shareObject.CurPrice = Convert.ToDecimal(e.WebParserInfoState.SearchResult["Price"][0]);

                                if (e.WebParserInfoState.SearchResult.ContainsKey("PriceBefore"))
                                    _shareObject.PrevDayPrice = Convert.ToDecimal(e.WebParserInfoState.SearchResult["PriceBefore"][0]);

                                // TODO Exception handling
                                // Save the share values to the XML
                                Exception exception = null;
                                if (!Helper.SaveShareObject(_shareObject, ref _xmlPortfolio, ref _xmlReaderPortfolio, ref _xmlReaderSettingsPortfolio, PortfolioFileName, out exception))
                                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                        exception.Message,
                                        _xmlLanguage, _languageName,
                                        Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                                // Save last displayed DataGridView row
                                if (dgvPortfolio.FirstDisplayedCell != null)
                                    _lastFirstDisplayedRowIndex = dgvPortfolio.FirstDisplayedCell.RowIndex;

                                // Update row with the new ShareObject values
                                _dgvPortfolioBindingSource.ResetBindings(false);

                                // Refresh the footer
                                RefreshFooter();

                                // Reset share add flag
                                _bAddFlag = false;
                            }

                            if (_bUpdateAll == true)
                            {
                                // Check if another share object should be updated
                                if (_selectedIndex < ShareObjectList.Count - 1)
                                {
                                    // Increase index to get the next share
                                    _selectedIndex++;

                                    Thread.Sleep(100);

                                    // Clear current selection
                                    dgvPortfolio.ClearSelection();

                                    // Select the new share update
                                    dgvPortfolio.Rows[_selectedIndex].Selected = true;

                                    // Scroll to the selected row
                                    Helper.ScrollDgvToIndex(dgvPortfolio, _selectedIndex, _lastFirstDisplayedRowIndex);

                                    // Start the asynchronous parsing operation.
                                    _webParser.WebSite = _shareObject.WebSite;
                                    _webParser.RegexList = _shareObject.RegexList;
                                    _webParser.EncodingType = _shareObject.EncodingType;
                                    _webParser.StartParsing();
                                }
                                else
                                {
                                    // Reset index
                                    _selectedIndex = 0;
                                    dgvPortfolio.Rows[_selectedIndex].Selected = true;

                                    // Scroll to the selected row
                                    Helper.ScrollDgvToIndex(dgvPortfolio, _selectedIndex, _lastFirstDisplayedRowIndex, true);

                                    timerStatusMessageClear.Enabled = true;
                                }
                            }
                            else
                            {
                                // Clear current selection
                                dgvPortfolio.ClearSelection();

                                // Select the new share update
                                dgvPortfolio.Rows[_selectedIndex].Selected = true;

                                timerStatusMessageClear.Enabled = true;
                            }
                            break;
                        case WebParserErrorCodes.SearchFinished:
                            // Add status message
                            Helper.AddStatusMessage(lblWebParserState,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchFinish", _languageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                _xmlLanguage, _languageName,
                                Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.SearchRunning:
                            // Add status message
                            Helper.AddStatusMessage(lblWebParserState,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchRunning", _languageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                _xmlLanguage, _languageName,
                                Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.SearchStarted:
                            // Add status message
                            Helper.AddStatusMessage(lblWebParserState,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/SearchStarted", _languageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                _xmlLanguage, _languageName,
                                Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.ContentLoadFinished:
                            // Add status message
                            Helper.AddStatusMessage(lblWebParserState,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/ContentLoaded", _languageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                _xmlLanguage, _languageName,
                               Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.ContentLoadStarted:
                            // Add status message
                            Helper.AddStatusMessage(lblWebParserState,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/ContentLoadStarted", _languageName) + " ( " + e.WebParserInfoState.Percentage.ToString() + " % )",
                                _xmlLanguage, _languageName,
                                Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.Started:
                            lblShareNameWebParser.Text = _shareObject.Name;
                            progressBarWebParser.Value = e.WebParserInfoState.Percentage;
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Start1", _languageName) +
                                _shareObject.Name +
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/Start2", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.Starting:
                            lblShareNameWebParser.Text = @"";
                            lblWebParserState.Text = @"";
                            progressBarWebParser.Value = 0;
                            break;
                        case WebParserErrorCodes.NoError:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                "",
                                _xmlLanguage, _languageName,
                                Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.StartFailed:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.BusyFailed:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/BusyFailed", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.InvalidWebSiteGiven:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/InvalidWebSiteGiven", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.NoRegexListGiven:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/NoRegexListGiven", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.NoWebContentLoaded:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ContentLoadedFailed", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.ParsingFailed:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ParsingFailed1", _languageName) +
                                e.WebParserInfoState.LastRegexListKey +
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/ParsingFailed2", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.CancelThread:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/CancelThread1", _languageName) +
                                _shareObject.Name +
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/CancelThread2", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.WebExceptionOccured:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/Failure", _languageName) +
                                e.WebParserInfoState.Exception.Message,
                                _xmlLanguage, _languageName,
                                Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.WebParser);
                            break;
                        case WebParserErrorCodes.ExceptionOccured:
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/Failure", _languageName) +
                                e.WebParserInfoState.Exception.Message,
                                _xmlLanguage, _languageName,
                                Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.WebParser);
                            break;
                    }

                    if (_webParser.LastErrorCode > 0)
                        Thread.Sleep(100);

                    // Check if a error occurred or the process has been finished
                    if (e.WebParserInfoState.LastErrorCode < 0 || e.WebParserInfoState.LastErrorCode == WebParserErrorCodes.Finished)
                    {
                        Helper.EnableDisableControls(true, this, _enableDisableControlNames);

                        if (e.WebParserInfoState.LastErrorCode < 0)
                        {
                            // Reset labels
                            lblShareNameWebParser.Text = @"";
                            lblWebParserState.Text = @"";
                        }

                        btnRefreshAll.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", _languageName);
                        btnRefresh.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", _languageName);
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
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/UpdateFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, _enableDisableControlNames);

                    // Reset labels
                    lblShareNameWebParser.Text = @"";
                    lblWebParserState.Text = @"";

                    btnRefreshAll.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", _languageName);
                    btnRefresh.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", _languageName);
                }
            }
        }

        #endregion WebParser
    }
}
