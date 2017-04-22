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
            _view.ShareObject = _model.ShareObject;

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

            _view.TaxAtSourceFlag = _model.TaxAtSourceFlag;
            _view.TaxAtSource = _model.TaxAtSource;
            _view.CapitalGainsTaxFlag = _model.CapitalGainsTaxFlag;
            _view.CapitalGainsTax = _model.CapitalGainsTax;
            _view.SolidarityTaxFlag = _model.SolidarityTaxFlag;
            _view.SolidarityTax = _model.SolidarityTax;
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
            _model.ShareObjectList = _view.ShareObjectList;
            _model.ShareObject = _view.ShareObject;
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

            _model.TaxAtSourceFlag = _view.TaxAtSourceFlag;
            _model.TaxAtSource = _view.TaxAtSource;
            _model.CapitalGainsTaxFlag = _view.CapitalGainsTaxFlag;
            _model.CapitalGainsTax = _view.CapitalGainsTax;
            _model.SolidarityTaxFlag = _view.SolidarityTaxFlag;
            _model.SolidarityTax = _view.SolidarityTax;

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
                Helper.CalcMarketValueAndFinalValue(_model.Volumedec, _model.SharePricedec, _model.Costsdec,
                    _model.Reductiondec, out decimal decMarketValue, out decimal decFinalValue);

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

                // Create a temporary share object with the new values of the new share object
                List<ShareObject> tempShareObject = new List<ShareObject>();
                tempShareObject.Add(new ShareObject(
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
                            _model.Document,
                            false,
                            0,
                            false,
                            0,
                            false,
                            0
                            ));

                // Check if for the given share a website configuration exists
                if (tempShareObject[0].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList))
                {
                    bool bTaxAtSourceFlag = false;
                    if (_model.TaxAtSourceFlag == CheckState.Checked)
                        bTaxAtSourceFlag = true;

                    bool bCapitalGainsTaxFlag = false;
                    if (_model.CapitalGainsTaxFlag == CheckState.Checked)
                        bCapitalGainsTaxFlag = true;

                    bool bSolidarityTaxFlag = false;
                    if (_model.SolidarityTaxFlag == CheckState.Checked)
                        bSolidarityTaxFlag = true;

                    _model.ShareObjectList.Add(new ShareObject(
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
                            _model.Document,
                            bTaxAtSourceFlag,
                            _model.TaxAtSourcedec,
                            bCapitalGainsTaxFlag,
                            _model.CapitalGainsTaxdec,
                            bSolidarityTaxFlag,
                            _model.SolidarityTaxdec
                            ));

                    // Set parsing expression to the share list
                    _model.ShareObjectList[_model.ShareObjectList.Count - 1].SetWebSiteRegexListAndEncoding(_model.WebSiteRegexList);
                    _model.ShareObject = _model.ShareObjectList[_model.ShareObjectList.Count - 1];

                    // Cost entry if the costs value is not 0
                    if (_model.Costsdec > 0)
                        _model.ShareObjectList[_model.ShareObjectList.Count - 1].AddCost(true, strDateTime, _model.Costsdec, _model.Document);

                    // Sort portfolio list in order of the share names
                    _model.ShareObjectList.Sort(new ShareObjectListComparer());
                }
                else
                    _model.ErrorCode = ShareAddErrorCode.WebSiteRegexNotFound;

                // Delete temp object
                if (tempShareObject.Count > 0)
                    tempShareObject[0].Dispose();
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

            // Check if an share with the given WKN number already exists
            if (!bErrorFlag)
            {
                foreach (var shareObject in _model.ShareObjectList)
                {
                    if (shareObject.Wkn == _model.Wkn)
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
                // Check if an share with the given share name already exists
                foreach (var shareObject in _model.ShareObjectList)
                {
                    if (shareObject.Name == _model.Name)
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

            // Check tax at source input
            if (_model.TaxAtSourceFlag == CheckState.Checked)
            {
                decimal decTaxAtSource = 0;
                if (_model.TaxAtSource == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.TaxAtSourceEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.TaxAtSource, out decTaxAtSource) && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.TaxAtSourceWrongFormat;
                    bErrorFlag = true;
                }
                else if (decTaxAtSource <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.TaxAtSourceWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.TaxAtSourcedec = decTaxAtSource;
            }

            // Check capital gains tax input
            if (_model.CapitalGainsTaxFlag == CheckState.Checked)
            {
                decimal decCapitalGainsTax = 0;
                if (_model.CapitalGainsTax == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.CapitalGainsTaxEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.CapitalGainsTax, out decCapitalGainsTax) && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.CapitalGainsTaxWrongFormat;
                    bErrorFlag = true;
                }
                else if (decCapitalGainsTax <= 0)
                {
                    _model.ErrorCode = ShareAddErrorCode.CapitalGainsTaxWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.CapitalGainsTaxdec = decCapitalGainsTax;
            }

            // Check solidarity tax input
            if (_model.SolidarityTaxFlag == CheckState.Checked)
            {
                decimal decSolidarityTax = 0;
                if (_model.SolidarityTax == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.SolidarityTaxEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.SolidarityTax, out decSolidarityTax) && bErrorFlag == false)
                {
                    _model.ErrorCode = ShareAddErrorCode.SolidarityTaxWrongFormat;
                    bErrorFlag = true;
                }
                else if (decSolidarityTax <= 0)
                {
                    _model.ErrorCode = ShareAddErrorCode.SolidarityTaxWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.SolidarityTaxdec = decSolidarityTax;
            }

            return bErrorFlag;
        }
    }
}
