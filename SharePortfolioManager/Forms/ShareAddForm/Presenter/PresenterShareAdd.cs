﻿//MIT License
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
//#define DEBUG_SHARE_ADD_PRESENTER

using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.ShareAddForm.Model;
using SharePortfolioManager.ShareAddForm.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharePortfolioManager.Classes.Configurations;

namespace SharePortfolioManager.ShareAddForm.Presenter
{
    public class PresenterShareAdd
    {
        private readonly IModelShareAdd _model;
        private readonly IViewShareAdd _view;

        public PresenterShareAdd(IViewShareAdd view, IModelShareAdd model)
        {
            _view = view;
            _model = model;

            view.PropertyChanged += OnViewChange;
            view.ShareAddEventHandler += OnShareAdd;
            view.FormatInputValuesEventHandler += OnViewFormatInputValues;
            view.DocumentBrowseEventHandler += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ShareObjectMarketValue = _model.ShareObjectMarketValue;
            _view.ShareObjectFinalValue = _model.ShareObjectFinalValue;

            _view.ErrorCode = _model.ErrorCode;

            _view.Wkn = _model.Wkn;
            _view.Isin = _model.Isin;
            _view.ShareName = _model.Name;
            _view.StockMarketLaunchDate = _model.StockMarketLaunchDate;
            _view.DetailsWebSite = _model.DetailsWebSite;
            _view.MarketValuesWebSite = _model.MarketValuesWebSite;
            _view.MarketValuesParsingOption = _model.MarketValuesParsingOption;
            _view.DailyValuesWebSite = _model.DailyValuesWebSite;
            _view.DailyValuesParsingOption = _model.DailyValuesParsingOption;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.DepotNumber = _model.DepotNumber;
            _view.OrderNumber = _model.OrderNumber;
            _view.Volume = _model.Volume;
            _view.SharePrice = _model.SharePrice;
            _view.Provision = _model.Provision;
            _view.BrokerFee = _model.BrokerFee;
            _view.TraderPlaceFee = _model.TraderPlaceFee;
            _view.Reduction = _model.Reduction;
            _view.Brokerage = _model.Brokerage;
            _view.BuyValue = _model.BuyValue;
            _view.BuyValueBrokerageReduction = _model.BuyValueBrokerageReduction;
            _view.Document = _model.Document;
        }

        private void OnViewFormatInputValues(object sender, EventArgs e)
        {
            _model.UpdateViewFormatted = true;

            UpdateViewWithModel();

            _model.UpdateViewFormatted = false;
        }

        private void OnViewChange(object sender, EventArgs e)
        {
            UpdateModelWithView();
        }

        private void UpdateModelWithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectListMarketValue = _view.ShareObjectListMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ShareObjectListFinalValue = _view.ShareObjectListFinalValue;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.ImageListPrevDayPerformance = _view.ImageListPrevDayPerformance;
            _model.ImageListCompletePerformance = _view.ImageListCompletePerformance;
            _model.WebSiteRegexList = _view.WebSiteRegexList;
            _model.ErrorCode = _view.ErrorCode;

            _model.Wkn = _view.Wkn;
            _model.Isin = _view.Isin;
            _model.Name = _view.ShareName;
            _model.StockMarketLaunchDate = _view.StockMarketLaunchDate;
            _model.ShareType = _view.ShareType;
            _model.DividendPayoutInterval = _view.DividendPayoutInterval;
            _model.CultureInfo = _view.CultureInfo;
            _model.DetailsWebSite = _view.DetailsWebSite;
            _model.MarketValuesWebSite = _view.MarketValuesWebSite;
            _model.MarketValuesParsingOption = _view.MarketValuesParsingOption;
            _model.DailyValuesWebSite = _view.DailyValuesWebSite;
            _model.DailyValuesParsingOption = _view.DailyValuesParsingOption;
            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.DepotNumber = _view.DepotNumber;
            _model.OrderNumber = _view.OrderNumber;
            _model.Volume = _view.Volume;
            _model.SharePrice = _view.SharePrice;
            _model.BuyValue = _view.BuyValue;
            _model.Provision = _view.Provision;
            _model.BrokerFee = _view.BrokerFee;
            _model.TraderPlaceFee = _view.TraderPlaceFee;
            _model.Reduction = _view.Reduction;
            _model.Brokerage = _view.Brokerage;
            _model.BuyValueBrokerageReduction = _view.BuyValueBrokerageReduction;
            _model.Document = _view.Document;

            CalculateMarketValueAndFinalValue();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        /// <summary>
        /// This function calculates the market value and purchase price of the given values
        /// If a given values are not valid the market value and final value is set to "0"
        /// </summary>
        private void CalculateMarketValueAndFinalValue()
        {
            try
            {
                Helper.CalcBuyValues(_model.VolumeDec, _model.SharePriceDec,
                    _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec,
                    out var decBuyValue, out var decBuyValueReduction, out var decBuyValueBrokerage,
                    out var decBuyValueBrokerageReduction,
                    out var decBrokerage, out var decBrokerageReduction);

                _model.BrokerageDec = decBrokerage;
                _model.BrokerageReductionDec = decBrokerageReduction;
                _model.BuyValueDec = decBuyValue;
                _model.BuyValueReductionDec = decBuyValueReduction;
                _model.BuyValueBrokerageDec = decBuyValueBrokerage;
                _model.BuyValueBrokerageReductionDec = decBuyValueBrokerageReduction;

#if DEBUG_SHARE_ADD_PRESENTER
                Console.WriteLine(@"BrokerageDec: {0}", _model.BrokerageDec);
                Console.WriteLine(@"BuyValueDec: {0}", _model.BuyValueDec);
                Console.WriteLine(@"BuyValueReductionDec: {0}", _model.BuyValueReductionDec);
                Console.WriteLine(@"BuyValueBrokerageDec: {0}", _model.BuyValueBrokerageDec);
                Console.WriteLine(@"BuyValueBrokerageReductionDec: {0}", _model.BuyValueBrokerageReductionDec);
#endif
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _model.BrokerageDec = 0;
                _model.BuyValueDec = 0;
                _model.BuyValueReductionDec = 0;
                _model.BuyValueBrokerageDec = 0;
                _model.BuyValueBrokerageReductionDec = 0;
            }
        }

        private void OnShareAdd(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues())
            {
                var strDateTime = _model.Date + " " + _model.Time;

                // Create a temporary share object with the new values of the new share object market value
                var tempShareObjectMarketValue = new List<ShareObjectMarketValue>
                {
                    new ShareObjectMarketValue(
                        @"",
                        _model.Wkn,
                        _model.Isin,
                        _model.DepotNumber,
                        _model.OrderNumber,
                        strDateTime,
                        _model.StockMarketLaunchDate,
                        _model.Name,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        0,
                        _model.VolumeDec,
                        0,
                        _model.ProvisionDec,
                        _model.BrokerFeeDec,
                        _model.TraderPlaceFeeDec,
                        _model.ReductionDec,
                        _model.DetailsWebSite,
                        _model.MarketValuesWebSite,
                        _model.MarketValuesParsingOption,
                        _model.DailyValuesWebSite,
                        _model.DailyValuesParsingOption,
                        _model.ImageListPrevDayPerformance,
                        _model.ImageListCompletePerformance,
                        null,
                        _model.CultureInfo,
                        0,
                        _model.Document
                    )
                };

                // Create a temporary share object with the new values of the new share object final value
                var tempShareObjectFinalValue = new List<ShareObjectFinalValue>
                {
                    new ShareObjectFinalValue(
                        @"",
                        _model.Wkn,
                        _model.Isin,
                        _model.DepotNumber,
                        _model.OrderNumber,
                        strDateTime,
                        _model.StockMarketLaunchDate,
                        _model.Name,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        0,
                        _model.VolumeDec,
                        0,
                        _model.ProvisionDec,
                        _model.BrokerFeeDec,
                        _model.TraderPlaceFeeDec,
                        _model.ReductionDec,
                        _model.DetailsWebSite,
                        _model.MarketValuesWebSite,
                        _model.MarketValuesParsingOption,
                        _model.DailyValuesWebSite,
                        _model.DailyValuesParsingOption,
                        _model.ImageListPrevDayPerformance,
                        _model.ImageListCompletePerformance,
                        null,
                        _model.CultureInfo,
                        0,
                        0,
                        _model.Document
                    )
                };

                // Check if for the given shares a website configuration exists
                if (tempShareObjectMarketValue[0].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList, _model.MarketValuesParsingOption) &&
                    tempShareObjectFinalValue[0].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList, _model.MarketValuesParsingOption)
                )
                {
                    // Generate Guid
                    var guid = Guid.NewGuid().ToString();

                    // Add market value share
                    _model.ShareObjectListMarketValue.Add(new ShareObjectMarketValue(
                        guid,
                        _model.Wkn,
                        _model.Isin,
                        _model.DepotNumber,
                        _model.OrderNumber,
                        strDateTime,
                        _model.StockMarketLaunchDate,
                        _model.Name,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        _model.SharePriceDec,
                        _model.VolumeDec,
                        0,
                        _model.ProvisionDec,
                        _model.BrokerFeeDec,
                        _model.TraderPlaceFeeDec,
                        _model.ReductionDec,
                        _model.DetailsWebSite,
                        _model.MarketValuesWebSite,
                        _model.MarketValuesParsingOption,
                        _model.DailyValuesWebSite,
                        _model.DailyValuesParsingOption,
                        _model.ImageListPrevDayPerformance,
                        _model.ImageListCompletePerformance,
                        null,
                        _model.CultureInfo,
                        _model.ShareType,
                        _model.Document
                    ));

                    // Add final value share
                    _model.ShareObjectListFinalValue.Add(new ShareObjectFinalValue(
                        guid,
                        _model.Wkn,
                        _model.Isin,
                        _model.DepotNumber,
                        _model.OrderNumber,
                        strDateTime,
                        _model.StockMarketLaunchDate,
                        _model.Name,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        _model.SharePriceDec,
                        _model.VolumeDec,
                        0,
                        _model.ProvisionDec,
                        _model.BrokerFeeDec,
                        _model.TraderPlaceFeeDec,
                        _model.ReductionDec,
                        _model.DetailsWebSite,
                        _model.MarketValuesWebSite,
                        _model.MarketValuesParsingOption,
                        _model.DailyValuesWebSite,
                        _model.DailyValuesParsingOption,
                        _model.ImageListPrevDayPerformance,
                        _model.ImageListCompletePerformance,
                        null,
                        _model.CultureInfo,
                        _model.DividendPayoutInterval,
                        _model.ShareType,
                        _model.Document
                    ));

                    // Set parsing expression to the market value share list
                    _model.ShareObjectListMarketValue[_model.ShareObjectListMarketValue.Count - 1]
                        .SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList, _model.MarketValuesParsingOption);
                    _model.ShareObjectMarketValue =
                        _model.ShareObjectListMarketValue[_model.ShareObjectListMarketValue.Count - 1];

                    // Set parsing expression to the  final value share list
                    _model.ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1]
                        .SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList, _model.MarketValuesParsingOption);
                    _model.ShareObjectFinalValue =
                        _model.ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1];

                    // Brokerage entry if any brokerage value is not 0
                    if (_model.ProvisionDec != 0 || _model.BrokerFeeDec != 0 || _model.TraderPlaceFeeDec != 0 ||
                        _model.ReductionDec != 0)
                    {
                        // Generate Guid
                        var strGuidBrokerage = _model
                            .ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1].AllBuyEntries
                            .AllBuysOfTheShareDictionary.Values.Last().BuyListYear.Last().BrokerageGuid;

                        _model.ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1].AddBrokerage(
                            strGuidBrokerage, true,
                            false, guid, strDateTime, _model.ProvisionDec, _model.BrokerFeeDec,
                            _model.TraderPlaceFeeDec, _model.ReductionDec, _model.Document);
                    }

                    // Set website configuration and encoding to the share object.
                    // The encoding is necessary for the Parser for encoding the download result.
                    _model.ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1]
                        .WebSiteConfigurationFound = _model
                        .ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1]
                        .SetWebSiteRegexListAndEncoding(WebSiteConfiguration.WebSiteRegexList, _model.MarketValuesParsingOption);

                    _model.ShareObjectListMarketValue[_model.ShareObjectListMarketValue.Count - 1]
                        .WebSiteConfigurationFound = _model
                        .ShareObjectListMarketValue[_model.ShareObjectListMarketValue.Count - 1]
                        .SetWebSiteRegexListAndEncoding(WebSiteConfiguration.WebSiteRegexList, _model.MarketValuesParsingOption);

                    // Sort portfolio list in order of the share names
                    _model.ShareObjectListMarketValue.Sort(new ShareObjectListComparer());

                    // Sort portfolio list in order of the share names
                    _model.ShareObjectListFinalValue.Sort(new ShareObjectListComparer());
                }
                else
                    _model.ErrorCode = ShareAddErrorCode.WebSiteRegexNotFound;

                // Delete temp objects
                if (tempShareObjectMarketValue.Count > 0)
                    tempShareObjectMarketValue[0].Dispose();
                if (tempShareObjectFinalValue.Count > 0)
                    tempShareObjectFinalValue[0].Dispose();
            }

            UpdateViewWithModel();

            _view.AddFinish();
        }

        /// <summary>
        /// This function opens the document browse dialog and set the chosen document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentBrowse(object sender, EventArgs e)
        {
            try
            {
                var strCurrentFile = _model.Document;

                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                if (Helper.SetDocument(
                    _model.Language.GetLanguageTextByXPath(@"/AddFormShare/OpenFileDialog/Title",
                        _model.LanguageName), strFilter, ref strCurrentFile) == DialogResult.OK)
                {
                    _model.Document = strCurrentFile;

                    UpdateViewWithModel();
                }

                _view.DocumentBrowseFinish();
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _model.ErrorCode = ShareAddErrorCode.DocumentBrowseFailed;
            }
        }

        /// <summary>
        /// This function checks if the input values are correct
        /// for and share add.
        /// </summary>
        /// <returns>Flag if the input values are correct or not</returns>
        private bool CheckInputValues()
        {
            var bErrorFlag = false;
            var decodedUrlDetailsWebSite = _model.DetailsWebSite;
            var decodedUrlMarketValuesWebSite = _model.MarketValuesWebSite;
            var decodeUrlDailyValuesWebSite = Helper.RegexReplaceStartDateAndInterval(_model.DailyValuesWebSite, _model.DailyValuesParsingOption);

            _model.ErrorCode = ShareAddErrorCode.AddSuccessful;

            // Check WKN number input
            if (_model.Wkn == @"")
            {
                _model.ErrorCode = ShareAddErrorCode.WknEmpty;
                bErrorFlag = true;
            }

            // Check if a market value share with the given WKN number already exists
            if (!bErrorFlag)
            {
                foreach (var shareObjectMarketValue in _model.ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.Wkn != _model.Wkn) continue;

                    _model.ErrorCode = ShareAddErrorCode.WknExists;
                    bErrorFlag = true;
                    break;
                }
            }

            // Check if final value share with the given WKN number already exists
            if (!bErrorFlag)
            {
                foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                {
                    if (shareObjectFinalValue.Wkn != _model.Wkn) continue;

                    _model.ErrorCode = ShareAddErrorCode.WknExists;
                    bErrorFlag = true;
                    break;
                }
            }

            // Check ISIN number input
            if (_model.Isin == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.IsinEmpty;
                bErrorFlag = true;
            }

            // Check if a market value share with the given ISIN number already exists
            if (!bErrorFlag)
            {
                foreach (var shareObjectMarketValue in _model.ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.Isin != _model.Isin) continue;

                    _model.ErrorCode = ShareAddErrorCode.IsinExists;
                    bErrorFlag = true;
                    break;
                }
            }

            // Check if final value share with the given ISIN number already exists
            if (!bErrorFlag)
            {
                foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                {
                    if (shareObjectFinalValue.Isin != _model.Isin) continue;

                    _model.ErrorCode = ShareAddErrorCode.IsinExists;
                    bErrorFlag = true;
                    break;
                }
            }

            // Check name input
            if (_model.Name == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.NameEmpty;
                bErrorFlag = true;
            }

            if (!bErrorFlag)
            {
                // Check if a market value share with the given share name already exists
                foreach (var shareObjectMarketValue in _model.ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.Name != _model.Name) continue;

                    _model.ErrorCode = ShareAddErrorCode.NameExists;
                    bErrorFlag = true;
                    break;
                }
            }

            if (!bErrorFlag)
            {
                // Check if a final value share with the given share name already exists
                foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                {
                    if (shareObjectFinalValue.Name != _model.Name) continue;

                    _model.ErrorCode = ShareAddErrorCode.NameExists;
                    bErrorFlag = true;
                    break;
                }
            }

            // Check if the stock market launch date has been modified
            var dummyDateTimePicker = new DateTimePicker
            {
                MinDate = DateTime.MinValue
            };

            if (_model.StockMarketLaunchDate == dummyDateTimePicker.MinDate.ToShortDateString() && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.StockMarketLaunchDateNotModified;
                bErrorFlag = true;
            }

            // Check details website input
            if (_model.DetailsWebSite == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.DetailsWebSiteEmpty;
                bErrorFlag = true;
            }
            // Check details website format
            else if (bErrorFlag == false
                &&
                !Helper.UrlChecker(
                    ref decodedUrlDetailsWebSite,
                    Helper.GetApiKey(
                        SettingsConfiguration.ApiKeysDictionary,
                        _model.DailyValuesParsingOption),
                        10000)
                )
            {
                _model.ErrorCode = ShareAddErrorCode.DetailsWebSiteWrongFormat;
                bErrorFlag = true;
            }
            // Check if the details website is already used
            else if (bErrorFlag == false)
            {

                // Check if a market value share with the given website already exists
                foreach (var shareObjectMarketValue in _model.ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.DetailsWebSiteUrl != _model.DetailsWebSite ||
                        shareObjectMarketValue == _model.ShareObjectMarketValue) continue;

                    _model.ErrorCode = ShareAddErrorCode.DetailsWebSiteExists;
                    bErrorFlag = true;
                    break;
                }

                if (bErrorFlag == false)
                {
                    // Check if a final value share with the given website already exists
                    foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                    {
                        if (shareObjectFinalValue.DetailsWebSiteUrl != _model.DetailsWebSite ||
                            shareObjectFinalValue == _model.ShareObjectFinalValue) continue;

                        _model.ErrorCode = ShareAddErrorCode.DetailsWebSiteExists;
                        bErrorFlag = true;
                        break;
                    }
                }
            }
            // Check market values website input
            if (_model.MarketValuesWebSite == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.MarketValuesWebSiteEmpty;
                bErrorFlag = true;
            }
            // Check market values website format
            else if (bErrorFlag == false
                &&
                !Helper.UrlChecker(
                    ref decodedUrlMarketValuesWebSite,
                    Helper.GetApiKey(
                        SettingsConfiguration.ApiKeysDictionary,
                        _model.MarketValuesParsingOption),
                    10000)
                )
            {
                _model.ErrorCode = ShareAddErrorCode.MarketWebSiteWrongFormat;
                bErrorFlag = true;
            }
            // Check if the market values website is already used
            else if (bErrorFlag == false)
            {

                // Check if a market value share with the given website already exists
                foreach (var shareObjectMarketValue in _model.ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.MarketValuesUpdateWebSiteUrl != _model.MarketValuesWebSite ||
                        shareObjectMarketValue == _model.ShareObjectMarketValue) continue;

                    _model.ErrorCode = ShareAddErrorCode.MarketWebSiteExists;
                    bErrorFlag = true;
                    break;
                }

                if (bErrorFlag == false)
                {
                    // Check if a final value share with the given website already exists
                    foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                    {
                        if (shareObjectFinalValue.MarketValuesUpdateWebSiteUrl != _model.MarketValuesWebSite ||
                            shareObjectFinalValue == _model.ShareObjectFinalValue) continue;

                        _model.ErrorCode = ShareAddErrorCode.MarketWebSiteExists;
                        bErrorFlag = true;
                        break;
                    }
                }
            }

            // Check daily values website input
            var dummyDailyValues = new List<Parser.DailyValues>();
            var dummyUrl = Helper.BuildDailyValuesUrl(dummyDailyValues, decodeUrlDailyValuesWebSite, _model.ShareType, _model.DailyValuesParsingOption);
            if (_model.DailyValuesWebSite == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.DailyValuesWebSiteEmpty;
                bErrorFlag = true;
            }
            // Check daily values website format
            else if (bErrorFlag == false
                &&
                !Helper.UrlChecker(
                    ref dummyUrl,
                    Helper.GetApiKey(
                        SettingsConfiguration.ApiKeysDictionary,
                        _model.DailyValuesParsingOption),
                    10000)
                )
            {
                _model.ErrorCode = ShareAddErrorCode.DailyValuesWebSiteWrongFormat;
                bErrorFlag = true;
            }
            // Check if the daily values website is already used
            else if (bErrorFlag == false)
            {

                // Check if a market value share with the given daily values website already exists
                foreach (var shareObjectMarketValue in _model.ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.DailyValuesUpdateWebSiteUrl != _model.DailyValuesWebSite ||
                        shareObjectMarketValue == _model.ShareObjectMarketValue) continue;

                    _model.ErrorCode = ShareAddErrorCode.DailyValuesWebSiteExists;
                    bErrorFlag = true;
                    break;
                }

                if (bErrorFlag == false)
                {
                    // Check if a final value share with the given daily values website already exists
                    foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                    {
                        if (shareObjectFinalValue.DailyValuesUpdateWebSiteUrl != _model.DailyValuesWebSite ||
                            shareObjectFinalValue == _model.ShareObjectFinalValue) continue;

                        _model.ErrorCode = ShareAddErrorCode.DailyValuesWebSiteExists;
                        bErrorFlag = true;
                        break;
                    }
                }
            }

            // Check if a depot number for the buy is given
            if ((_model.DepotNumber == @"" || _model.DepotNumber == @"-") && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.DepotNumberEmpty;
                bErrorFlag = true;
            }

            // Check if a order number for the buy is given and the order number does not exits already if a new buy should be added
            if (_model.OrderNumber == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.OrderNumberEmpty;
                bErrorFlag = true;
            }
            else if (_model.ShareObjectFinalValue != null &&
                     _model.ShareObjectFinalValue.AllBuyEntries.OrderNumberAlreadyExists(_model.OrderNumber) &&
                     bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.OrderNumberExists;
                bErrorFlag = true;
            }

            // Check if a correct volume for the add is given
            if (_model.Volume == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.VolumeEmpty;
                bErrorFlag = true;
            }
            else if (!decimal.TryParse(_model.Volume, out var decVolume) && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.VolumeWrongFormat;
                bErrorFlag = true;
            }
            else if (decVolume <= 0 && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.VolumeWrongValue;
                bErrorFlag = true;
            }
            else if (bErrorFlag == false)
                _model.VolumeDec = decVolume;

            // Check if a correct price for the buy is given
            if (_model.SharePrice == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.SharePriceEmpty;
                bErrorFlag = true;
            }
            else if (!decimal.TryParse(_model.SharePrice, out var decPrice) && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.SharePriceWrongFormat;
                bErrorFlag = true;
            }
            else if (decPrice <= 0 && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.SharePriceWrongValue;
                bErrorFlag = true;
            }
            else if (bErrorFlag == false)
                _model.SharePriceDec = decPrice;

            // Provision input check
            if (_model.Provision != "" && bErrorFlag == false)
            {
                if (!decimal.TryParse(_model.Provision, out var decProvision))
                {
                    _model.ErrorCode = ShareAddErrorCode.ProvisionWrongFormat;
                    bErrorFlag = true;
                }
                else if (decProvision < 0)
                {
                    _model.ErrorCode = ShareAddErrorCode.ProvisionWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.ProvisionDec = decProvision;
            }

            // Broker fee input check
            if (_model.BrokerFee != "" && bErrorFlag == false)
            {
                if (!decimal.TryParse(_model.BrokerFee, out var decBrokerFee))
                {
                    _model.ErrorCode = ShareAddErrorCode.BrokerFeeWrongFormat;
                    bErrorFlag = true;
                }
                else if (decBrokerFee < 0)
                {
                    _model.ErrorCode = ShareAddErrorCode.BrokerFeeWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.BrokerFeeDec = decBrokerFee;
            }

            // Trader place fee input check
            if (_model.TraderPlaceFee != "" && bErrorFlag == false)
            {
                if (!decimal.TryParse(_model.TraderPlaceFee, out var decTraderPlaceFee))
                {
                    _model.ErrorCode = ShareAddErrorCode.TraderPlaceFeeWrongFormat;
                    bErrorFlag = true;
                }
                else if (decTraderPlaceFee < 0)
                {
                    _model.ErrorCode = ShareAddErrorCode.TraderPlaceFeeWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.TraderPlaceFeeDec = decTraderPlaceFee;
            }

            // Reduction input check
            if (_model.Reduction != "" && bErrorFlag == false)
            {
                if (!decimal.TryParse(_model.Reduction, out var decReduction))
                {
                    _model.ErrorCode = ShareAddErrorCode.ReductionWrongFormat;
                    bErrorFlag = true;
                }
                else if (decReduction < 0)
                {
                    _model.ErrorCode = ShareAddErrorCode.ReductionWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.ReductionDec = decReduction;
            }

            // Brokerage input check
            if (_model.Brokerage == @"")
            {
                _model.ErrorCode = ShareAddErrorCode.BrokerageEmpty;
                bErrorFlag = true;
            }
            else if (!decimal.TryParse(_model.Brokerage, out var decBrokerage))
            {
                _model.ErrorCode = ShareAddErrorCode.BrokerageWrongFormat;
                bErrorFlag = true;
            }
            else if (decBrokerage < 0)
            {
                _model.ErrorCode = ShareAddErrorCode.BrokerageWrongValue;
                bErrorFlag = true;
            }
            else
                _model.BrokerageDec = decBrokerage;

            // Check document input
            if (_model.Document != @"" && !Directory.Exists(Path.GetDirectoryName(_model.Document)) && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.DocumentDirectoryDoesNotExists;
            }
            // Check document input
            else if (_model.Document != @"" && !File.Exists(_model.Document))
            {
                _model.ErrorCode = ShareAddErrorCode.DocumentFileDoesNotExists;
            }

            _model.DetailsWebSite = decodedUrlDetailsWebSite;
            _model.MarketValuesWebSite = decodedUrlMarketValuesWebSite;
            _model.DailyValuesWebSite = Helper.RegexReplaceStartDateAndInterval(decodeUrlDailyValuesWebSite, _model.DailyValuesParsingOption);

            return bErrorFlag;
        }
    }
}
