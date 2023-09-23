//MIT License
//
//Copyright(c) 2017 - 2022 nessie1980(nessie1980@gmx.de)
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
using System.Globalization;
using System.Threading;
using SharePortfolioManager.Classes.Configurations;
using SharePortfolioManager.Properties;
using System.Net;

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
            ParserMarketValues = new Parser.Parser(SettingsConfiguration.ParserMarketValuesDebuggingEnable);
            ParserDailyValues = new Parser.Parser(SettingsConfiguration.ParserDailyValuesDebuggingEnable);

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
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAllCancel",
                            SettingsConfiguration.LanguageName);
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
                                // Build parsing values for the market values update
                                if (ShareObjectMarketValue.MarketValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectMarketValue.MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectMarketValue.MarketValuesParsingOption),
                                        ShareObjectMarketValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaRealTime
                                    );
                                }
                                else if (ShareObjectMarketValue.MarketValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectMarketValue.MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectMarketValue.MarketValuesParsingOption),
                                        ShareObjectMarketValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooRealTime
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the market values
                                ParserMarketValues.StartParsing();
                            }

                            if (ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectMarketValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.DailyValues)
                            {
                                // Build parsing values for the daily values update
                                if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(ShareObjectMarketValue.DailyValuesList.Entries,
                                        ShareObjectMarketValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectMarketValue.ShareType,
                                        ShareObjectMarketValue.DailyValuesParsingOption)),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectMarketValue.DailyValuesParsingOption),
                                        ShareObjectMarketValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaHistoryData
                                    );
                                }
                                else if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(ShareObjectMarketValue.DailyValuesList.Entries,
                                        ShareObjectMarketValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectMarketValue.ShareType,
                                        ShareObjectMarketValue.DailyValuesParsingOption)),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectMarketValue.DailyValuesParsingOption),
                                        ShareObjectMarketValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooHistoryData
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the daily market values
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
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll",
                                    SettingsConfiguration.LanguageName);
                            btnRefresh.Text =
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh",
                                    SettingsConfiguration.LanguageName);
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
                            if (ShareObjectFinalValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectFinalValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.MarketPrice)
                            {
                                // Build parsing values for the market values update
                                if (ShareObjectFinalValue.MarketValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectFinalValue.MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectFinalValue.MarketValuesParsingOption),
                                        ShareObjectFinalValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaRealTime
                                    );
                                }
                                else if (ShareObjectFinalValue.MarketValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectFinalValue.MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectFinalValue.MarketValuesParsingOption),
                                        ShareObjectMarketValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooRealTime
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the market values
                                ParserMarketValues.StartParsing();
                            }

                            if (ShareObjectFinalValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.Both ||
                                ShareObjectFinalValue.InternetUpdateOption ==
                                ShareObject.ShareUpdateTypes.DailyValues)
                            {
                                // Build parsing values for the daily values update
                                if (ShareObjectFinalValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(ShareObjectFinalValue.DailyValuesList.Entries,
                                        ShareObjectFinalValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectFinalValue.ShareType,
                                        ShareObjectFinalValue.DailyValuesParsingOption)),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectFinalValue.DailyValuesParsingOption),
                                        ShareObjectFinalValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaHistoryData
                                    );
                                }
                                else if (ShareObjectFinalValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(ShareObjectFinalValue.DailyValuesList.Entries,
                                        ShareObjectFinalValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectFinalValue.ShareType,
                                        ShareObjectFinalValue.DailyValuesParsingOption)),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectFinalValue.DailyValuesParsingOption),
                                        ShareObjectFinalValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooHistoryData
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the daily market values
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
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll",
                                    SettingsConfiguration.LanguageName);
                            btnRefresh.Text =
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh",
                                    SettingsConfiguration.LanguageName);
                            btnRefreshAll.Image = Resources.button_update_all_24;
                            btnRefresh.Image = Resources.button_update_24;
                        }
                    }
                }
                else
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
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
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshCancel",
                                SettingsConfiguration.LanguageName);
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
                                // Build parsing values the market values update
                                if (ShareObjectMarketValue.MarketValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListMarketValue[
                                            dgvPortfolioMarketValue
                                                .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                .RowIndex]
                                                .MarketValuesParsingOption),
                                        ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaRealTime
                                    );
                                }
                                else if (ShareObjectMarketValue.MarketValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListMarketValue[
                                            dgvPortfolioMarketValue
                                                .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                .RowIndex]
                                                .MarketValuesParsingOption),
                                        ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooRealTime
                                    );
                                }
                                 
                                // Start the asynchronous operation of the Parser for the market values
                                ParserMarketValues.StartParsing();

                                // Build parsing values for the market values update
                                if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(ShareObjectMarketValue.DailyValuesList.Entries,
                                        ShareObjectMarketValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectMarketValue.ShareType,
                                        ShareObjectMarketValue.DailyValuesParsingOption)),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectMarketValue.DailyValuesParsingOption),
                                        ShareObjectMarketValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaHistoryData
                                    );
                                }
                                else if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(ShareObjectMarketValue.DailyValuesList.Entries,
                                        ShareObjectMarketValue.DailyValuesUpdateWebSiteUrl,
                                        ShareObjectMarketValue.ShareType,
                                        ShareObjectMarketValue.DailyValuesParsingOption)),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectMarketValue.DailyValuesParsingOption),
                                        ShareObjectMarketValue.WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooHistoryData
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the daily market values
                                ParserDailyValues.StartParsing();
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
                                // Build parsing values for the daily values update
                                if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesList.Entries,
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesUpdateWebSiteUrl,
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .ShareType,
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesParsingOption

                                        )),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesParsingOption),
                                        ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaHistoryData
                                    );
                                }
                                else if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesList.Entries,
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesUpdateWebSiteUrl,
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .ShareType,
                                            ShareObjectListMarketValue[
                                                    dgvPortfolioMarketValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesParsingOption

                                        )),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[
                                                        (int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                                .DailyValuesParsingOption),
                                        ShareObjectListMarketValue[
                                                dgvPortfolioMarketValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooHistoryData
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the daily market values
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
                                // Build parsing values for the market values update
                                if (ShareObjectMarketValue.MarketValuesParsingOption ==  ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .MarketValuesParsingOption),
                                        ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaRealTime
                                    );
                                }
                                else if (ShareObjectMarketValue.MarketValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserMarketValues.ParsingValues = new ParsingValues(
                                        new Uri(ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .MarketValuesUpdateWebSiteUrl),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                                .MarketValuesParsingOption),
                                        ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooRealTime
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the market values
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
                                // Build parsing values for the market values update
                                if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiOnvista)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesList.Entries,
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesUpdateWebSiteUrl,
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .ShareType,
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesParsingOption
                                        )),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .DailyValuesParsingOption),
                                        ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.OnVistaHistoryData
                                    );
                                }
                                else if (ShareObjectMarketValue.DailyValuesParsingOption == ShareObject.ParsingTypes.ApiYahoo)
                                {
                                    ParserDailyValues.ParsingValues = new ParsingValues(
                                        new Uri(Helper.BuildDailyValuesUrl(
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesList.Entries,
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesUpdateWebSiteUrl,
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .ShareType,
                                            ShareObjectListFinalValue[
                                                    dgvPortfolioFinalValue
                                                        .SelectedCells[
                                                            (int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                        .RowIndex]
                                                .DailyValuesParsingOption
                                        )),
                                        Helper.GetApiKey(
                                            SettingsConfiguration.ApiKeysDictionary,
                                            ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .DailyValuesParsingOption),
                                        ShareObjectListFinalValue[
                                                dgvPortfolioFinalValue
                                                    .SelectedCells[(int)ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                                                    .RowIndex]
                                            .WebSiteEncodingType,
                                        DataTypes.ParsingType.YahooHistoryData
                                    );
                                }

                                // Start the asynchronous operation of the Parser for the daily market values
                                ParserDailyValues.StartParsing();
                            }
                        }
                    }
                    else
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.Orange, Logger, (int) EStateLevels.Warning, (int) EComponentLevels.Application);
                    }
                }
                else
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/StartFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
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
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Finish_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Finish_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);

                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchFinish",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);

                            // Check if a share is selected
                            if (ShareObjectFinalValue != null && ShareObjectMarketValue != null)
                            {
                                var ci = new RegionInfo(ShareObjectFinalValue.CultureInfo.LCID);

                                // Check if the parsed value have the right currency
                                if (e.ParserInfoState.SearchResult.ContainsKey("Currency") &&
                                    e.ParserInfoState.SearchResult["Currency"][0] == ci.ISOCurrencySymbol)
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
                                            SettingsConfiguration.PortfolioName, out var exception))
                                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                                    @"/MainForm/UpdateErrors/MarketValues/SaveFailed_1",
                                                    SettingsConfiguration.LanguageName) +
                                                shareName +
                                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                                    @"/MainForm/UpdateErrors/MarketValues/SaveFailed_2",
                                                    SettingsConfiguration.LanguageName),
                                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
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
                                else
                                {
                                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                                            @"/MainForm/UpdateErrors/MarketValues/CurrencyError_1",
                                            SettingsConfiguration.LanguageName) +
                                        e.ParserInfoState.SearchResult["Currency"][0] +
                                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                                            @"/MainForm/UpdateErrors/MarketValues/CurrencyError_2",
                                            SettingsConfiguration.LanguageName) +
                                        ci.ISOCurrencySymbol +
                                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                                            @"/MainForm/UpdateErrors/MarketValues/CurrencyError_3",
                                            SettingsConfiguration.LanguageName),
                                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                        Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
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
                                if (ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished &&
                                    (
                                        ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                        ParserDailyValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                                    )
                                )
                                {
                                    var openUpdates = false;

                                    // Check if in the next shares still is a share which should be updated
                                    for (var i = SelectedDataGridViewShareIndex + 1; i < ShareObject.ObjectCounter; i++)
                                    {
                                        if (ShareObjectListFinalValue[i].InternetUpdateOption !=
                                            ShareObject.ShareUpdateTypes.None)
                                            openUpdates = true;
                                    }

                                    // Start next share update if still shares exists which must be updated
                                    if (SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1 && openUpdates)
                                        timerStartNextShareUpdate.Enabled = true;

                                    // Do not start the next share update because the last share has been reached
                                    if (SelectedDataGridViewShareIndex == ShareObject.ObjectCounter - 1 || !openUpdates)
                                    {
                                        // Update caption of the groupbox and play sound
                                        Helper.UpdateDone(grpBoxSharePortfolio, ShareObjectListFinalValue);

                                        // Check which share overview is selected
                                        if (MarketValueOverviewTabSelected)
                                        {
                                            // This is done for the data grid view scrolling
                                            // Clear current selection
                                            dgvPortfolioMarketValue.ClearSelection();

                                            if (ShareObject.ObjectCounter > 1)
                                            {
                                                SelectedDataGridViewShareIndex--;

                                                // Scroll to the selected row
                                                Helper.ScrollDgvToIndex(dgvPortfolioMarketValue,
                                                    SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

                                                SelectedDataGridViewShareIndex++;
                                            }

                                            // Select the new share update
                                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected =
                                                true;
                                        }
                                        else
                                        {
                                            // This is done for the data grid view scrolling
                                            // Clear current selection
                                            dgvPortfolioFinalValue.ClearSelection();

                                            if (ShareObject.ObjectCounter > 1)
                                            {
                                                SelectedDataGridViewShareIndex--;

                                                // Scroll to the selected row
                                                Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                                                    SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

                                                SelectedDataGridViewShareIndex++;
                                            }

                                            // Select the new share update
                                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected =
                                                true;
                                        }

                                        timerStatusMessageClear.Enabled = true;
                                    }
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
                                {
                                    // Update caption of the groupbox and play sound
                                    Helper.UpdateDone(grpBoxSharePortfolio, ShareObjectListFinalValue);

                                    timerStatusMessageClear.Enabled = true;
                                }
                            }

                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchFinish",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchRunning:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchRunning",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/SearchStarted",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/ContentLoaded",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserMarketValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateMessages/MarketValues/ContentLoadStarted", SettingsConfiguration.LanguageName) + " ( " +
                                e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.Started:
                        {
                            // Just wait some time before the update begins
                            Thread.Sleep(250);
                            progressBarWebParserMarketValues.Value = e.ParserInfoState.Percentage;

                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Start_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/MarketValues/Start_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
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
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.StartFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/StartFailed",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.BusyFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/BusyFailed",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.InvalidWebSiteGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/InvalidWebSiteGiven_1", SettingsConfiguration.LanguageName) +
                            ParserMarketValues.ParsingValues.GivenWebSiteUrl +
                            LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/InvalidWebSiteGiven_2", SettingsConfiguration.LanguageName) +
                            shareName +
                            LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/InvalidWebSiteGiven_3", SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoRegexListGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/NoRegexListGiven_1", SettingsConfiguration.LanguageName) +
                            shareName +
                            LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/NoRegexListGiven_2", SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoWebContentLoaded:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_1", SettingsConfiguration.LanguageName) +
                            shareName +
                            LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_2", SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.JsonError:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/JsonError_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/JsonError_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ParsingFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_1",
                                    SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.LastRegexListKey +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_2",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/ParsingFailed_3",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.CancelThread:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/CancelThread_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/CancelThread_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.FileExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/FileFailed_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/FileFailed_2", SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.JsonExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/JsonFailed_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/JsonFailed_2", SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.WebExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/ContentLoadedFailed_2", SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/Failure_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/MarketValues/Failure_2", SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                    }

                    // Check if an error occurred or the process has been finished
                    if (e.ParserInfoState.LastErrorCode < DataTypes.ParserErrorCodes.NoError)
                    {
                        UpdateAllFlag = false;

                        var shareUpdateTypes = ShareObject.ShareUpdateTypes.None;

                        if (MarketValueOverviewTabSelected)
                        {
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            if (ShareObjectMarketValue != null)
                                shareUpdateTypes = ShareObjectMarketValue.InternetUpdateOption;
                        }
                        else
                        {
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            if (ShareObjectFinalValue != null)
                                shareUpdateTypes = ShareObjectFinalValue.InternetUpdateOption;
                        }

                        // Check if the daily values parser has finished his work or has no error so far
                        if (
                            (
                                shareUpdateTypes == ShareObject.ShareUpdateTypes.Both ||
                                shareUpdateTypes == ShareObject.ShareUpdateTypes.MarketPrice ||
                                ParserDailyValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                ParserDailyValues.ParserErrorCode < DataTypes.ParserErrorCodes.NoError
                             )
                            &&
                            ParserMarketValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                        )
                            timerStatusMessageClear.Enabled = true;
                    }

                    if (e.ParserInfoState.Exception != null)
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/UpdateFailed_1",
                            SettingsConfiguration.LanguageName) +
                        shareName +
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/UpdateFailed_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application,
                        e.ParserInfoState.Exception);
                    }

                    progressBarWebParserMarketValues.Value = e.ParserInfoState.Percentage;

                    // Check if an exception is given
                    if (e.ParserInfoState.Exception != null)
                    {
                        if (e.ParserInfoState.Exception.GetType() == typeof(WebException))
                        {
                            Helper.AddStatusMessage(
                                rchTxtBoxStateMessage,
                               LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/GeneralErrors/CaptionParser", 
                                    SettingsConfiguration.LanguageName) +
                                    @" HttpStatus: " +
                                    e.ParserInfoState.HttpStatus.ToString(),
                                LanguageConfiguration.Language,
                                SettingsConfiguration.LanguageName,
                                Color.DarkRed, Logger,
                                (int)EStateLevels.FatalError,
                                (int)EComponentLevels.Parser,
                                e.ParserInfoState.Exception
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/UpdateFailed_1",
                            SettingsConfiguration.LanguageName) +
                        shareName +
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/MarketValues/UpdateFailed_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                        ex);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Reset labels
                    lblWebParserMarketValuesState.Text = @"";

                    btnRefreshAll.Text =
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", SettingsConfiguration.LanguageName);
                    btnRefresh.Text =
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", SettingsConfiguration.LanguageName);
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
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Finish_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Finish_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);

                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchFinish",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Parser);

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
                                        SettingsConfiguration.PortfolioName, out var exception))
                                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/SaveFailed_1",
                                                SettingsConfiguration.LanguageName) +
                                            shareName +
                                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/SaveFailed_2",
                                                SettingsConfiguration.LanguageName),
                                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
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
                                    var openUpdates = false;

                                    // Check if in the next shares still is a share which should be updated
                                    for (var i = SelectedDataGridViewShareIndex + 1; i < ShareObject.ObjectCounter; i++)
                                    {
                                        if (ShareObjectListFinalValue[i].InternetUpdateOption !=
                                            ShareObject.ShareUpdateTypes.None)
                                            openUpdates = true;
                                    }

                                    // Start next share update if still shares exists which must be updated
                                    if (SelectedDataGridViewShareIndex < ShareObject.ObjectCounter - 1 && openUpdates)
                                        timerStartNextShareUpdate.Enabled = true;

                                    if (SelectedDataGridViewShareIndex == ShareObject.ObjectCounter - 1 || !openUpdates)
                                    {
                                        // Update caption of the groupbox and play sound
                                        Helper.UpdateDone(grpBoxSharePortfolio, ShareObjectListFinalValue);

                                        // Check which share overview is selected
                                        if (MarketValueOverviewTabSelected)
                                        {
                                            // This is done for the data grid view scrolling
                                            // Clear current selection
                                            dgvPortfolioMarketValue.ClearSelection();

                                            if (ShareObject.ObjectCounter > 1)
                                            {
                                                SelectedDataGridViewShareIndex--;

                                                // Scroll to the selected row
                                                Helper.ScrollDgvToIndex(dgvPortfolioMarketValue,
                                                    SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

                                                SelectedDataGridViewShareIndex++;
                                            }

                                            // Select the new share update
                                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected =
                                                true;
                                        }
                                        else
                                        {
                                            // This is done for the data grid view scrolling
                                            // Clear current selection
                                            dgvPortfolioFinalValue.ClearSelection();

                                            if (ShareObject.ObjectCounter > 1)
                                            {
                                                SelectedDataGridViewShareIndex--;

                                                // Scroll to the selected row
                                                Helper.ScrollDgvToIndex(dgvPortfolioFinalValue,
                                                    SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

                                                SelectedDataGridViewShareIndex++;
                                            }

                                            // Select the new share update
                                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected =
                                                true;
                                        }

                                        timerStatusMessageClear.Enabled = true;
                                    }
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
                                {
                                    // Update caption of the groupbox and play sound
                                    Helper.UpdateDone(grpBoxSharePortfolio, ShareObjectListFinalValue);

                                    timerStatusMessageClear.Enabled = true;
                                }
                            }

                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchFinish",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchRunning:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchRunning",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.SearchStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/SearchStarted",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadFinished:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/ContentLoaded",
                                    SettingsConfiguration.LanguageName) + " ( " + e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ContentLoadStarted:
                        {
                            Helper.AddStatusMessage(lblWebParserDailyValuesState,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateMessages/DailyValues/ContentLoadStarted", SettingsConfiguration.LanguageName) + " ( " +
                                e.ParserInfoState.Percentage + " % )",
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.Started:
                        {
                            // Just wait some time before the update begins
                            Thread.Sleep(250);
                            progressBarWebParserDailyValues.Value = e.ParserInfoState.Percentage;

                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Start_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateMessages/DailyValues/Start_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
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
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.StartFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/StartFailed",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.BusyFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/BusyFailed",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.InvalidWebSiteGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/InvalidWebSiteGiven_1", SettingsConfiguration.LanguageName) +
                                ParserDailyValues.ParsingValues.GivenWebSiteUrl +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/InvalidWebSiteGiven_2", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/InvalidWebSiteGiven_3", SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoRegexListGiven:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/NoRegexListGiven_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/NoRegexListGiven_2", SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.NoWebContentLoaded:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/ContentLoadedFailed_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/ContentLoadedFailed_2", SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.JsonError:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/JsonError_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/JsonError_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ParsingFailed:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_1",
                                    SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.LastRegexListKey +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_2",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/ParsingFailed_3",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.CancelThread:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/CancelThread_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/CancelThread_2",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.FileExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/FileFailed_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/FileFailed_2",
                                    SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.JsonExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/JsonFailed_1",
                                    SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/JsonFailed_2",
                                    SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception.Message,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.WebExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/Failure_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/DailyValues/Failure_2", SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                        case DataTypes.ParserErrorCodes.ExceptionOccurred:
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/MainForm/UpdateErrors/DailyValues/Failure_1", SettingsConfiguration.LanguageName) +
                                shareName +
                                LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/MainForm/UpdateErrors/DailyValues/Failure_2", SettingsConfiguration.LanguageName) +
                                e.ParserInfoState.Exception,
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Parser,
                                e.ParserInfoState.Exception);
                            break;
                        }
                    }

                    // Check if an error occurred or the process has been finished
                    if (e.ParserInfoState.LastErrorCode < DataTypes.ParserErrorCodes.NoError)
                    {
                        UpdateAllFlag = false;

                        var shareUpdateTypes = ShareObject.ShareUpdateTypes.None;

                        if (MarketValueOverviewTabSelected)
                        {
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            if (ShareObjectMarketValue != null)
                                shareUpdateTypes = ShareObjectMarketValue.InternetUpdateOption;
                        }
                        else
                        {
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                                LastFirstDisplayedRowIndex, true);

                            if (ShareObjectFinalValue != null)
                                shareUpdateTypes = ShareObjectFinalValue.InternetUpdateOption;
                        }

                        if (
                            (
                                shareUpdateTypes == ShareObject.ShareUpdateTypes.Both ||
                                shareUpdateTypes == ShareObject.ShareUpdateTypes.DailyValues ||
                                ParserMarketValues.ParserErrorCode == DataTypes.ParserErrorCodes.Finished ||
                                ParserMarketValues.ParserErrorCode < DataTypes.ParserErrorCodes.NoError
                            )
                            &&
                            ParserDailyValues.ParserErrorCode <= DataTypes.ParserErrorCodes.NoError
                        )
                            timerStatusMessageClear.Enabled = true;
                    }

                    progressBarWebParserDailyValues.Value = e.ParserInfoState.Percentage;

                    // Check if an exception is given
                    if (e.ParserInfoState.Exception != null)
                    {
                        if (e.ParserInfoState.Exception.GetType() == typeof(WebException))
                        {
                            Helper.AddStatusMessage(
                                rchTxtBoxStateMessage,
                               LanguageConfiguration.Language.GetLanguageTextByXPath(
                                    @"/GeneralErrors/CaptionParser",
                                    SettingsConfiguration.LanguageName) +
                                    @" HttpStatus: " +
                                    e.ParserInfoState.HttpStatus.ToString(),
                                LanguageConfiguration.Language,
                                SettingsConfiguration.LanguageName,
                                Color.DarkRed, Logger,
                                (int)EStateLevels.FatalError,
                                (int)EComponentLevels.Parser,
                                e.ParserInfoState.Exception
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/UpdateFailed_1",
                            SettingsConfiguration.LanguageName) +
                        shareName +
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/UpdateErrors/DailyValues/UpdateFailed_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                        ex);

                    Thread.Sleep(500);
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Reset labels
                    lblWebParserDailyValuesState.Text = @"";

                    btnRefreshAll.Text =
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", SettingsConfiguration.LanguageName);
                    btnRefresh.Text =
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", SettingsConfiguration.LanguageName);
                    btnRefreshAll.Image = Resources.button_update_all_24;
                    btnRefresh.Image = Resources.button_update_24;
                }
            }
        }
    }
}
