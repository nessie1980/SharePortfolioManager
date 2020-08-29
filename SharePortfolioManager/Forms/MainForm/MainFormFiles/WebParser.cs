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

// Define for DEBUGGING
//#define DEBUG_WEB_PARSER

using Parser;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using System;
using System.Drawing;
using System.Threading;
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
            ParserMarketValues = new Parser.Parser(ParserMarketValuesDebuggingEnable);
            ParserDailyValues = new Parser.Parser(ParserDailyValuesDebuggingEnable);

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
                if (ParserMarketValues != null && ParserMarketValues.ParserInfoState.State == DataTypes.ParserState.Idle)
                {
                    // Set flag for updating all shares
                    UpdateAllFlag = true;

                    // Rename the button
                    btnRefreshAll.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAllCancel",
                            LanguageName);
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
                            if (ShareObjectMarketValue.InternetUpdateOption == ShareObject.ShareUpdateTypes.None ||
                                !ShareObjectMarketValue.WebSiteConfigurationFound)
                                SelectedDataGridViewShareIndex++;

                        } while (ShareObjectMarketValue.InternetUpdateOption == ShareObject.ShareUpdateTypes.None ||
                                 !ShareObjectMarketValue.WebSiteConfigurationFound &&
                                 SelectedDataGridViewShareIndex < ShareObject.ObjectCounter);

                        // Check if the share should be update
                        if (ShareObjectMarketValue.InternetUpdateOption != ShareObject.ShareUpdateTypes.None &&
                            ShareObjectMarketValue.WebSiteConfigurationFound)
                        {
                            if (ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.MarketPrice)
                            {
                                // Start the asynchronous operation of the Parser for the market values
                                ParserMarketValues.ParsingValues = new ParsingValues(
                                    new Uri(ShareObjectMarketValue.UpdateWebSiteUrl),
                                    ShareObjectMarketValue.WebSiteEncodingType,
                                    ShareObjectMarketValue.RegexList
                                );
                                ParserMarketValues.StartParsing();
                            }

                            if (ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.DailyValues)
                            {
                                // Start the asynchronous operation of the Parser for the daily market values
                                ParserDailyValues.ParsingValues = new ParsingValues(
                                    new Uri(Helper.BuildDailyValuesUrl(ShareObjectMarketValue.DailyValues,
                                        ShareObjectMarketValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectMarketValue.ShareType)),
                                    ShareObjectMarketValue.WebSiteEncodingType
                                );
                                ParserDailyValues.StartParsing();
                            }
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

                            btnRefreshAll.Text =
                                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll",
                                    LanguageName);
                            btnRefresh.Text =
                                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh",
                                    LanguageName);
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
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            // Check if the current share should not be updated so check the next share
                            if (ShareObjectFinalValue.InternetUpdateOption == ShareObject.ShareUpdateTypes.None ||
                                !ShareObjectFinalValue.WebSiteConfigurationFound)
                                SelectedDataGridViewShareIndex++;

                        } while (ShareObjectFinalValue.InternetUpdateOption == ShareObject.ShareUpdateTypes.None  ||
                                 !ShareObjectFinalValue.WebSiteConfigurationFound &&
                                 SelectedDataGridViewShareIndex < ShareObject.ObjectCounter);

                        // Check if the share should be update
                        if (ShareObjectFinalValue.InternetUpdateOption != ShareObject.ShareUpdateTypes.None &&
                            ShareObjectFinalValue.WebSiteConfigurationFound)
                        {
                            if (ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.MarketPrice)
                            {
                                // Start the asynchronous operation of the Parser for the market values
                                ParserMarketValues.ParsingValues = new ParsingValues(
                                    new Uri(ShareObjectFinalValue.UpdateWebSiteUrl),
                                    ShareObjectFinalValue.WebSiteEncodingType,
                                    ShareObjectFinalValue.RegexList
                                );
                                ParserMarketValues.StartParsing();
                            }

                            if (ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.DailyValues)
                            {
                                // Start the asynchronous operation of the Parser for the daily market values
                                ParserDailyValues.ParsingValues = new ParsingValues(
                                    new Uri(Helper.BuildDailyValuesUrl(ShareObjectFinalValue.DailyValues,
                                        ShareObjectFinalValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectFinalValue.ShareType)),
                                    ShareObjectFinalValue.WebSiteEncodingType
                                );
                                ParserDailyValues.StartParsing();
                            }
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

                            btnRefreshAll.Text =
                                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll",
                                    LanguageName);
                            btnRefresh.Text =
                                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh",
                                    LanguageName);
                            btnRefreshAll.Image = Resources.button_update_all_24;
                            btnRefresh.Image = Resources.button_update_24;
                        }
                    }
                }
                else
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                if (ParserMarketValues != null && ParserMarketValues.ParserInfoState.State == DataTypes.ParserState.Idle)
                {
                    // Check if a share is selected
                    if (MarketValueOverviewTabSelected == false && dgvPortfolioFinalValue.SelectedCells.Count != 0 &&
                        dgvPortfolioFinalValue.SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex].Value.ToString() != "" ||
                        MarketValueOverviewTabSelected && dgvPortfolioMarketValue.SelectedCells.Count != 0 &&
                        dgvPortfolioMarketValue.SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex].Value.ToString() != ""
                    )
                    {
                        // Reset flag for updating all shares
                        UpdateAllFlag = false;

                        btnRefresh.Text =
                            Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshCancel",
                                LanguageName);
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
                            if (ShareObjectListMarketValue[
                                    dgvPortfolioMarketValue
                                        .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                        .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectListMarketValue[
                                    dgvPortfolioMarketValue
                                        .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                        .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.MarketPrice)
                            {

                                // Start the asynchronous operation of the Parser for the market values
                                ParserMarketValues.ParsingValues = new ParsingValues(
                                    new Uri(ShareObjectListMarketValue[
                                            dgvPortfolioMarketValue
                                                .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                .RowIndex]
                                        .UpdateWebSiteUrl),
                                    ShareObjectListMarketValue[
                                            dgvPortfolioMarketValue
                                                .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                .RowIndex]
                                        .WebSiteEncodingType,
                                    ShareObjectListMarketValue[
                                        dgvPortfolioMarketValue
                                            .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                            .RowIndex].RegexList
                                );
                                ParserMarketValues.StartParsing();
                            }

                            if (ShareObjectListMarketValue[
                                    dgvPortfolioMarketValue
                                        .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                        .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectListMarketValue[
                                    dgvPortfolioMarketValue
                                        .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                        .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.DailyValues)
                            {
                                // Start the asynchronous operation of the Parser for the daily market values
                                ParserDailyValues.ParsingValues = new ParsingValues(
                                    new Uri(Helper.BuildDailyValuesUrl(
                                        ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[
                                                        (int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .DailyValues,
                                        ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[
                                                        (int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .DailyValuesUpdateWebSiteUrl,
                                        ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[
                                                        (int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .ShareType
                                    )),
                                    ShareObjectListMarketValue[
                                            dgvPortfolioMarketValue
                                                .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                .RowIndex]
                                        .WebSiteEncodingType
                                );
                                ParserDailyValues.StartParsing();
                            }
                        }
                        else
                        {
                            if (ShareObjectListFinalValue[dgvPortfolioFinalValue
                                    .SelectedCells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                    .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectListFinalValue[
                                    dgvPortfolioFinalValue
                                        .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                        .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.MarketPrice)
                            {
                                // Start the asynchronous operation of the Parser for the market values
                                ParserMarketValues.ParsingValues = new ParsingValues(
                                    new Uri(ShareObjectListFinalValue[
                                            dgvPortfolioFinalValue
                                                .SelectedCells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                .RowIndex]
                                        .UpdateWebSiteUrl),
                                    ShareObjectListFinalValue[
                                            dgvPortfolioFinalValue
                                                .SelectedCells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                .RowIndex]
                                        .WebSiteEncodingType,
                                    ShareObjectListFinalValue[
                                            dgvPortfolioFinalValue
                                                .SelectedCells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                .RowIndex]
                                        .RegexList
                                );
                                ParserMarketValues.StartParsing();
                            }

                            if (ShareObjectListFinalValue[dgvPortfolioFinalValue
                                    .SelectedCells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                    .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectListFinalValue[
                                    dgvPortfolioFinalValue
                                        .SelectedCells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                        .RowIndex].InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.DailyValues)
                            {
                                // Start the asynchronous operation of the Parser for the daily market values
                                ParserDailyValues.ParsingValues = new ParsingValues(
                                    new Uri(Helper.BuildDailyValuesUrl(
                                        ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[
                                                        (int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .DailyValues,
                                        ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[
                                                        (int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .DailyValuesUpdateWebSiteUrl,
                                        ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[
                                                        (int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .ShareType
                                    )),
                                    ShareObjectListFinalValue[
                                            dgvPortfolioFinalValue
                                                .SelectedCells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                .RowIndex]
                                        .WebSiteEncodingType
                                );
                                ParserDailyValues.StartParsing();
                            }
                        }
                    }
                    else
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", LanguageName),
                            Language, LanguageName,
                            Color.Orange, Logger, (int) EStateLevels.Warning, (int) EComponentLevels.Application);
                    }
                }
                else
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
        private void ParserMarketValues_UpdateGUI(object sender, DataTypes.OnParserUpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ParserMarketValues_UpdateGUI(sender, e)));
            }
            else
            {
                var shareName = MarketValueOverviewTabSelected
                    ? ShareObjectMarketValue.Name
                    : ShareObjectFinalValue.Name;

                try
                {
                    switch (e.ParserInfoState.LastErrorCode)
                    {
                        case DataTypes.ParserErrorCodes.Finished:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Finish_1",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Finish_2",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);

                            // Check if a share is selected
                            if (ShareObjectFinalValue != null && ShareObjectMarketValue != null)
                            {
                                ShareObjectFinalValue.LastUpdateViaInternet = DateTime.Now;
                                ShareObjectMarketValue.LastUpdateViaInternet = DateTime.Now;

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
                                    ShareObjectMarketValue.CurPrice =
                                        Convert.ToDecimal(e.ParserInfoState.SearchResult["Price"][0]);
                                    ShareObjectFinalValue.CurPrice =
                                        Convert.ToDecimal(e.ParserInfoState.SearchResult["Price"][0]);
                                }

                                if (e.ParserInfoState.SearchResult.ContainsKey("PriceBefore"))
                                {
                                    ShareObjectMarketValue.PrevPrice =
                                        Convert.ToDecimal(e.ParserInfoState.SearchResult["PriceBefore"][0]);
                                    ShareObjectFinalValue.PrevPrice =
                                        Convert.ToDecimal(e.ParserInfoState.SearchResult["PriceBefore"][0]);
                                }

                                // Save the share values to the XML
                                if (ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                    ParserMarketValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                                )
                                {
                                    if (!ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue,
                                        ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio,
                                        _portfolioFileName, out var exception))
                                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/SaveFailed_1",
                                                LanguageName) +
                                            shareName +
                                            Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/SaveFailed_2",
                                                LanguageName),
                                            Language, LanguageName,
                                            Color.Red, Logger, (int) EStateLevels.Error,
                                            (int) EComponentLevels.Application, exception);
                                }

                                // Save last displayed DataGridView row
                                if (MarketValueOverviewTabSelected)
                                {
                                    if (dgvPortfolioMarketValue.FirstDisplayedCell != null)
                                        LastFirstDisplayedRowIndex =
                                            dgvPortfolioMarketValue.FirstDisplayedCell.RowIndex;
                                }
                                else
                                {
                                    if (dgvPortfolioFinalValue.FirstDisplayedCell != null)
                                        LastFirstDisplayedRowIndex =
                                            dgvPortfolioFinalValue.FirstDisplayedCell.RowIndex;
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
                                    Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                        LastFirstDisplayedRowIndex);
                                }
                                else
                                {
                                    // Select the new share update
                                    dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                    // Scroll to the selected row
                                    Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                                        SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                                }

                                // Check if both parsers have been finished
                                if (ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished &&
                                    (
                                        ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                        ParserDailyValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                                    )
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

                                // Check if both parsers have been finished
                                if (ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished &&
                                    (
                                        ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                        ParserDailyValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                                    )
                                )
                                    timerStatusMessageClear.Enabled = true;
                            }

                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchFinish",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchRunning:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchRunning",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchStarted",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/ContentLoaded",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateMessages/MarketValues/ContentLoadStarted", LanguageName) + " ( " +
                                e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.Started:
                        {
                            // Just wait some time before the update begins
                            Thread.Sleep(250);
                            progressBarWebParserMarketValues.Value = e.ParserInfoState.Percentage;

                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Start_1",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Start_2",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.Starting:
                        {
                            lblWebParserMarketValuesState.Text = @"";
                            progressBarWebParserMarketValues.Value = 0;
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoError:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                "",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.StartFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/StartFailed",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.BusyFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/BusyFailed",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.InvalidWebSiteGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/InvalidWebSiteGiven_1", LanguageName) +
                            ParserMarketValues.ParsingValues.GivenWebSiteUrl +
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/InvalidWebSiteGiven_2", LanguageName) +
                            shareName +
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/InvalidWebSiteGiven_3", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoRegexListGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/NoRegexListGiven_1", LanguageName) +
                            shareName +
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/NoRegexListGiven_2", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoWebContentLoaded:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_1", LanguageName) +
                            shareName +
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_2", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ParsingFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_1",
                                    LanguageName) +
                                e.ParserInfoState.LastRegexListKey +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_2",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_3",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.CancelThread:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/CancelThread_1",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/CancelThread_2",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.WebExceptionOccured:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_1", LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_2", LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ExceptionOccured:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/Failure_1", LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/Failure_2", LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                Language, LanguageName,
                                Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                    }

                    // Check if an error occurred or the process has been finished
                    if (e.ParserInfoState.LastErrorCode < DataTypes.ParserErrorCodes.NoError)
                    {
                        UpdateAllFlag = false;

                        if (MarketValueOverviewTabSelected)
                        {
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            // Reset index
                            // 07.06.2020
                            //SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);
                        }
                        else
                        {
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            // Reset index
                            // 07.06.2020
                            //SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);
                        }

                        if (
                            (ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                             ParserDailyValues.ParserErrorCode < DataTypes.ParserErrorCodes.NoError
                             )
                            &&
                            ParserMarketValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                        )
                            timerStatusMessageClear.Enabled = true;
                    }

                    progressBarWebParserMarketValues.Value = e.ParserInfoState.Percentage;
                }
                catch (Exception ex)
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/UpdateFailed_1",
                            LanguageName) +
                        shareName +
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/UpdateFailed_2",
                            LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                        ex);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Reset labels
                    lblWebParserMarketValuesState.Text = @"";

                    btnRefreshAll.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                    btnRefresh.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
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
        private void ParserDailyValues_UpdateGUI(object sender, DataTypes.OnParserUpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ParserDailyValues_UpdateGUI(sender, e)));
            }
            else
            {
                var shareName = MarketValueOverviewTabSelected
                    ? ShareObjectMarketValue.Name
                    : ShareObjectFinalValue.Name;

                try
                {
                    switch (e.ParserInfoState.LastErrorCode)
                    {
                        case DataTypes.ParserErrorCodes.Finished:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Finish_1",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Finish_2",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);

                            // Check if a share is selected
                            if (ShareObjectFinalValue != null && ShareObjectMarketValue != null)
                            {
                                // Only add if the date not exists already
                                ShareObjectFinalValue.AddNewDailyValues(ParserDailyValues.ParserInfoState
                                    .DailyValuesList);

                                // Save the share values to the XML
                                if (ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                    ParserDailyValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                                )
                                {
                                    // Save the share values to the XML
                                    if (!ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue,
                                        ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio,
                                        _portfolioFileName, out var exception))
                                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/SaveFailed_1",
                                                LanguageName) +
                                            shareName +
                                            Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/SaveFailed_2",
                                                LanguageName),
                                            Language, LanguageName,
                                            Color.Red, Logger, (int)EStateLevels.Error,
                                            (int)EComponentLevels.Application,
                                            exception);
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
                                    Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                        LastFirstDisplayedRowIndex);
                                }
                                else
                                {
                                    // Select the new share update
                                    dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                                    // Scroll to the selected row
                                    Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                                        SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                                }

                                // Check if both parsers have been finished
                                if (ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished &&
                                    (
                                        ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                        ParserMarketValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                                    )
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

                                // Check if both parsers have been finished
                                if (ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished &&
                                    (
                                        ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                        ParserMarketValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                                    )
                                )
                                    timerStatusMessageClear.Enabled = true;
                            }

                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchFinish",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchRunning:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchRunning",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchStarted",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/ContentLoaded",
                                    LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateMessages/DailyValues/ContentLoadStarted", LanguageName) + " ( " +
                                e.ParserInfoState.Percentage + " % )",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.Started:
                        {
                            // Just wait some time before the update begins
                            Thread.Sleep(250);
                            progressBarWebParserDailyValues.Value = e.ParserInfoState.Percentage;

                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Start_1",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Start_2",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.Starting:
                        {
                            lblWebParserDailyValuesState.Text = @"";
                            progressBarWebParserDailyValues.Value = 0;
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoError:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                "",
                                Language, LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.StartFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/StartFailed",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.BusyFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/BusyFailed",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.InvalidWebSiteGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/InvalidWebSiteGiven_1", LanguageName) +
                                ParserDailyValues.ParsingValues.GivenWebSiteUrl +
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/InvalidWebSiteGiven_2", LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/InvalidWebSiteGiven_3", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoRegexListGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/NoRegexListGiven_1", LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/NoRegexListGiven_2", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoWebContentLoaded:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/ContentLoadedFailed_1", LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/ContentLoadedFailed_2", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ParsingFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_1",
                                    LanguageName) +
                                e.ParserInfoState.LastRegexListKey +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_2",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_3",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.CancelThread:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/CancelThread_1",
                                    LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/CancelThread_2",
                                    LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.WebExceptionOccured:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/Failure_1", LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/DailyValues/Failure_2", LanguageName) +
                                e.ParserInfoState.Exception,
                                Language, LanguageName,
                                Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ExceptionOccured:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/Failure_1", LanguageName) +
                                shareName +
                                Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/DailyValues/Failure_2", LanguageName) +
                                e.ParserInfoState.Exception,
                                Language, LanguageName,
                                Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                    }

                    // Check if an error occurred or the process has been finished
                    if (e.ParserInfoState.LastErrorCode < DataTypes.ParserErrorCodes.NoError)
                    {
                        UpdateAllFlag = false;

                        if (MarketValueOverviewTabSelected)
                        {
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            // Reset index
                            // 07.06.2020
                            //SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);
                        }
                        else
                        {
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            // Reset index
                            //07.06.2020
                            //SelectedDataGridViewShareIndex = 0;

                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);
                        }

                        if ((
                            ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                            ParserMarketValues.ParserErrorCode < DataTypes.ParserErrorCodes.NoError
                            )
                            &&
                            ParserDailyValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                        )
                            timerStatusMessageClear.Enabled = true;
                    }

                    progressBarWebParserDailyValues.Value = e.ParserInfoState.Percentage;
                }
                catch (Exception ex)
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/UpdateFailed_1",
                            LanguageName) +
                        shareName +
                        Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/UpdateFailed_2",
                            LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                        ex);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Reset labels
                    lblWebParserDailyValuesState.Text = @"";

                    btnRefreshAll.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName);
                    btnRefresh.Text =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName);
                    btnRefreshAll.Image = Resources.button_update_all_24;
                    btnRefresh.Image = Resources.button_update_24;
                }
            }
        }
    }
}
