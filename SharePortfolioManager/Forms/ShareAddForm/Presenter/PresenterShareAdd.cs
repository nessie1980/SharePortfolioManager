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
using SharePortfolioManager.Forms.ShareAddForm.Model;
using SharePortfolioManager.Forms.ShareAddForm.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.ShareAddForm.Presenter
{
    public class PresenterShareAdd
    {
        private readonly IModelShareAdd _model;
        private readonly IViewShareAdd _view;

        public PresenterShareAdd(IViewShareAdd view, IModelShareAdd model)
        {
            this._view = view;
            this._model = model;

            view.PropertyChanged += OnViewCchange;
            view.ShareAdd += OnShareAdd;
            view.FormatInputValues += OnViewFormatInputValues;
        }

        private void UpdateViewWithModel()
        {
            _view.ShareObjectMarketValue = _model.ShareObjectMarketValue;
            _view.ShareObjectFinalValue = _model.ShareObjectFinalValue;

            _view.ErrorCode = _model.ErrorCode;

            _view.Wkn = _model.Wkn;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.ShareName = _model.Name;
            _view.Volume = _model.Volume;
            _view.SharePrice = _model.SharePrice;
            _view.MarketValue = _model.MarketValue;
            _view.Costs = _model.Costs;
            _view.Reduction = _model.Reduction;
            _view.GrandTotal = _model.FinalValue;
            _view.WebSite = _model.WebSite;
            _view.Document = _model.Document;
        }

        private void OnViewFormatInputValues(object sender, EventArgs e)
        {
            _model.UpdateViewFormatted = true;

            UpdateViewWithModel();

            _model.UpdateViewFormatted = false;
        }

        private void OnViewCchange(object sender, EventArgs e)
        {
            UpdateModelwithView();
        }

        private void UpdateModelwithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectListMarketValue = _view.ShareObjectListMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ShareObjectListFinalValue = _view.ShareObjectListFinalValue;

            _model.ImageList = _view.ImageList;
            _model.WebSiteRegexList = _view.WebSiteRegexList;
            _model.ErrorCode = _view.ErrorCode;

            _model.Wkn = _view.Wkn;
            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Name = _view.ShareName;
            _model.Volume = _view.Volume;
            _model.SharePrice = _view.SharePrice;
            _model.MarketValue = _view.MarketValue;
            _model.Costs = _view.Costs;
            _model.Reduction = _view.Reduction;
            _model.FinalValue = _view.GrandTotal;
            _model.WebSite = _view.WebSite;
            _model.CultureInfo = _view.CultureInfo;
            _model.DividendPayoutInterval = _view.DividendPayoutInterval;
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
                Helper.CalcBuyValues(_model.Volumedec, _model.SharePricedec, _model.Costsdec,
                    _model.Reductiondec, out decimal decMarketValue, out decimal decPurchaseValue, out decimal decFinalValue);

                _model.MarketValuedec = decMarketValue;
                _model.FinalValuedec = decFinalValue;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculateMarketValueAndFinalValue()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.MarketValuedec = 0;
                _model.FinalValuedec = 0;
            }
        }

        private void OnShareAdd(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues())
            {
                string strDateTime = _model.Date + " " + _model.Time;

                // Create a temporary share object with the new values of the new share object market value
                List<ShareObjectMarketValue> tempShareObjectMarketValue = new List<ShareObjectMarketValue>();
                tempShareObjectMarketValue.Add(new ShareObjectMarketValue(
                            _model.Wkn,
                            strDateTime,
                            _model.Name,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            0,
                            _model.Volumedec,
                            _model.Reductiondec,
                            _model.Costsdec,
                            _model.MarketValuedec,
                            _model.WebSite,
                            _model.ImageList,
                            null,
                            _model.CultureInfo,
                            0,
                            _model.Document
                            ));

                // Create a temporary share object with the new values of the new share object final value
                List<ShareObjectFinalValue> tempShareObjectFinalValue = new List<ShareObjectFinalValue>();
                tempShareObjectFinalValue.Add(new ShareObjectFinalValue(
                            _model.Wkn,
                            strDateTime,
                            _model.Name,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            0,
                            _model.Volumedec,
                            _model.Reductiondec,
                            _model.Costsdec,
                            _model.MarketValuedec,
                            _model.WebSite,
                            _model.ImageList,
                            null,
                            _model.CultureInfo,
                            0,
                            _model.Document
                            ));

                // Check if for the given shares a website configuration exists
                if (tempShareObjectMarketValue[0].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList) &&
                    tempShareObjectFinalValue[0].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList)
                    )
                {
                    // Add market value share
                    _model.ShareObjectListMarketValue.Add(new ShareObjectMarketValue(
                            _model.Wkn,
                            strDateTime,
                            _model.Name,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            _model.SharePricedec,
                            _model.Volumedec,
                            _model.Reductiondec,
                            _model.Costsdec,
                            _model.MarketValuedec,
                            _model.WebSite,
                            _model.ImageList,
                            null,
                            _model.CultureInfo,
                            _model.DividendPayoutInterval,
                            _model.Document
                            ));

                    // Add final value share
                    _model.ShareObjectListFinalValue.Add(new ShareObjectFinalValue(
                            _model.Wkn,
                            strDateTime,
                            _model.Name,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            DateTime.MinValue,
                            _model.SharePricedec,
                            _model.Volumedec,
                            _model.Reductiondec,
                            _model.Costsdec,
                            _model.MarketValuedec,
                            _model.WebSite,
                            _model.ImageList,
                            null,
                            _model.CultureInfo,
                            _model.DividendPayoutInterval,
                            _model.Document
                            ));

                    // Set parsing expression to the market value share list
                    _model.ShareObjectListMarketValue[_model.ShareObjectListMarketValue.Count - 1].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList);
                    _model.ShareObjectMarketValue = _model.ShareObjectListMarketValue[_model.ShareObjectListMarketValue.Count - 1];

                    // Sort portfolio list in order of the share names
                    _model.ShareObjectListMarketValue.Sort(new ShareObjectListComparer());

                    // Set parsing expression to the  final value share list
                    _model.ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList);
                    _model.ShareObjectFinalValue = _model.ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1];

                    // Cost entry if the costs value is not 0 and add it to the final value list
                    if (_model.Costsdec > 0)
                        _model.ShareObjectListFinalValue[_model.ShareObjectListFinalValue.Count - 1].AddCost(true, strDateTime, _model.Costsdec, _model.Document);

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

            MessageBox.Show(_model.ErrorCode.ToString(), @"Info", MessageBoxButtons.OK);
        }

        /// <summary>
        /// This function checks if the input values are correct
        /// for and share add.
        /// </summary>
        /// <returns>Flag if the input values are correct or not</returns>
        private bool CheckInputValues()
        {
            bool bErrorFlag = false;

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
                    if (shareObjectMarketValue.Wkn == _model.Wkn)
                    {
                        _model.ErrorCode = ShareAddErrorCode.WknExists;
                        bErrorFlag = true;
                        break;
                    }
                }
            }

            // Check if final value share with the given WKN number already exists
            if (!bErrorFlag)
            {
                foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                {
                    if (shareObjectFinalValue.Wkn == _model.Wkn)
                    {
                        _model.ErrorCode = ShareAddErrorCode.WknExists;
                        bErrorFlag = true;
                        break;
                    }
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
                    if (shareObjectMarketValue.Name == _model.Name)
                    {
                        _model.ErrorCode = ShareAddErrorCode.NameExists;
                        bErrorFlag = true;
                        break;
                    }
                }
            }

            if (!bErrorFlag)
            {
                // Check if a final value share with the given share name already exists
                foreach (var shareObjectFinalValue in _model.ShareObjectListFinalValue)
                {
                    if (shareObjectFinalValue.Name == _model.Name)
                    {
                        _model.ErrorCode = ShareAddErrorCode.NameExists;
                        bErrorFlag = true;
                        break;
                    }
                }
            }

            // Check if a correct volume for the add is given
            decimal decVolume = -1;
            if (_model.Volume == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.VolumeEmpty;
                bErrorFlag = true;
            }
            else if (!decimal.TryParse(_model.Volume, out decVolume) && bErrorFlag == false)
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
                _model.Volumedec = decVolume;

            // Check if a correct price for the buy is given
            decimal decPrice = -1;
            if (_model.SharePrice == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.SharePriceEmpty;
                bErrorFlag = true;
            }
            else if (!decimal.TryParse(_model.SharePrice, out decPrice) && bErrorFlag == false)
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
                _model.SharePricedec = decPrice;

            // Costs input check
            if (_model.Costs != "" && bErrorFlag == false)
            {
                decimal decCosts = 0;
                if (!decimal.TryParse(_model.Costs, out decCosts) && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.CostsWrongFormat;
                    bErrorFlag = true;
                }
                else if (decCosts < 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.CostsWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.Costsdec = decCosts;
            }

            // Reduction input check
            if (_model.Reduction != "" && bErrorFlag == false)
            {
                decimal decReduction = 0;
                if (!decimal.TryParse(_model.Reduction, out decReduction) && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.ReductionWrongFormat;
                    bErrorFlag = true;
                }
                else if (decReduction < 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.ReductionWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.Reductiondec = decReduction;
            }

            // Check website input
            if (_model.WebSite == @"" && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.WebSiteEmpty;
                bErrorFlag = true;
            }

            // TODO Web address validation

            // Check document input
            else if (_model.Document != @"" && !File.Exists(_model.Document) && bErrorFlag == false)
            {
                _model.ErrorCode = ShareAddErrorCode.DocumentDoesNotExists;
                bErrorFlag = true;
            }

            return bErrorFlag;
        }
    }
}
