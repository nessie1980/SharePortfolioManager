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
using SharePortfolioManager.Forms.SalesForm.Model;
using SharePortfolioManager.Forms.SalesForm.View;
using System;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.SalesForm.Presenter
{
    class PresenterSaleEdit
    {
        private readonly IModelSaleEdit _model;
        private readonly IViewSaleEdit _view;

        public PresenterSaleEdit(IViewSaleEdit view, IModelSaleEdit model)
        {
            this._view = view;
            this._model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValues += OnViewFormatInputValues;
            view.AddSale += OnAddSale;
            view.EditSale += OnEditSale;
            view.DeleteSale += OnDeleteSale;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.Volume = _model.Volume;
            _view.BuyPrice = _model.BuyPrice;
            _view.SalePrice = _model.SalePrice;
            _view.LossBalance = _model.LossBalance;
            _view.TaxAtSource = _model.TaxAtSource;
            _view.CapitalGainsTax = _model.CapitalGainsTax;
            _view.SolidarityTax = _model.SolidarityTax;
            _view.Costs = _model.Costs;
            _view.ProfitLoss = _model.ProfitLoss;
            _view.Payout = _model.Payout;
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
            UpdateModelwithView();
        }

        private void UpdateModelwithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ErrorCode = _view.ErrorCode;
            _model.UpdateSale = _view.UpdateSale;
            _model.SelectedDate = _view.SelectedDate;
            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Volume = _view.Volume;
            _model.BuyPrice = _view.BuyPrice;
            _model.SalePrice = _view.SalePrice;
            _model.LossBalance = _view.LossBalance;
            _model.TaxAtSource = _view.TaxAtSource;
            _model.CapitalGainsTax = _view.CapitalGainsTax;
            _model.SolidarityTax = _view.SolidarityTax;
            _model.Costs = _view.Costs;
            _model.ProfitLoss = _view.ProfitLoss;
            _model.Payout = _view.Payout;
            _model.Document = _view.Document;

            CalculateMarketValueAndFinalValue();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        private void OnAddSale(object sender, EventArgs e)
        {
            // TODO
            // Check the input values
            if (!CheckInputValues(_model.UpdateSale))
            {
                //bool bErrorFlag = false;
                //string strDateTime = _model.Date + " " + _model.Time;

                //// Cost entry if the costs value is not 0
                //if (_model.CostsDec > 0)
                //    bErrorFlag = !_model.ShareObjectFinalValue.AddCost(true, strDateTime, _model.CostsDec, _model.Document);

                //if (_model.ShareObjectFinalValue.AddSale(strDateTime, _model.VolumeDec, _model.SharePricedec, _model.ReductionDec, _model.CostsDec, _model.Document)
                //    && bErrorFlag == false)
                //{
                //    _model.ErrorCode = SaleErrorCode.AddSuccessful;
                //}
                //else
                //{
                //    _model.ErrorCode = SaleErrorCode.AddFailed;
                //}
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditSale(object sender, EventArgs e)
        {
            // TODO
            // Check the input values
            if (!CheckInputValues(_model.UpdateSale))
            {
                //string strDateTime = _model.Date + " " + _model.Time;

                //if (_model.ShareObjectFinalValue.RemoveSale(_model.SelectedDate) && _model.ShareObjectFinalValue.AddSale(strDateTime, _model.VolumeDec, _model.SharePricedec, _model.ReductionDec, _model.CostsDec, _model.Document))
                //{
                //    bool bFlagCostEdit = true;

                //    // Check if an old cost entry must be deleted
                //    if (_model.ShareObjectFinalValue.AllCostsEntries.GetCostObjectByDateTime(strDateTime) != null)
                //        bFlagCostEdit = _model.ShareObjectFinalValue.RemoveCost(strDateTime);

                //    // Check if a new cost entry must be made
                //    if (bFlagCostEdit && _model.CostsDec > 0)
                //        bFlagCostEdit = _model.ShareObjectFinalValue.AddCost(true, strDateTime, _model.CostsDec, _model.Document);

                //    if (bFlagCostEdit)
                //    {
                //        _model.ErrorCode = SaleErrorCode.EditSuccessful;
                //    }

                //    UpdateViewWithModel();

                //    _view.AddEditDeleteFinish();

                //    return;
                //}
            }

            //_model.ErrorCode = SaleErrorCode.EditFailed;
            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteSale(object sender, EventArgs e)
        {
            // If this is the first sale of all. This sale can´t be deleted. Only if the whole share will be deleted
            if (_model.ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare().Count > 1)
            {
                // Delete the sale of the selected date
                if (_model.ShareObjectFinalValue.RemoveSale(_model.SelectedDate))
                {
                    // Check if a cost object exists
                    if (_model.ShareObjectFinalValue.AllCostsEntries.GetCostObjectByDateTime(_model.SelectedDate) != null)
                    {
                        if (_model.ShareObjectFinalValue.RemoveCost(_model.SelectedDate))
                        {
                            _model.ErrorCode = SaleErrorCode.DeleteSuccessful;
                        }
                        else
                        {
                            _model.ErrorCode = SaleErrorCode.DeleteFailed;
                        }
                    }
                    else
                    {
                        _model.ErrorCode = SaleErrorCode.DeleteSuccessful;
                    }
                }
                else
                {
                    _model.ErrorCode = SaleErrorCode.DeleteFailed;
                }
            }
            else
                _model.ErrorCode = SaleErrorCode.DeleteFailedUnerasable;

            // Update error code
            _view.ErrorCode = _model.ErrorCode;

            _view.AddEditDeleteFinish();
        }

        /// <summary>
        /// This function calculates the market value and purchase price of the given values
        /// If a given values are not valid the market value and final value is set to "0"
        /// </summary>
        private void CalculateMarketValueAndFinalValue()
        {
//            try
//            {
//                decimal decMarketValue = 0;
//                decimal decPurchaseValue = 0;
//                decimal decFinalValue = 0;

//                Helper.CalcSaleValues(_model.VolumeDec, _model.SharePricedec, _model.CostsDec,
//                    _model.ReductionDec, out decMarketValue, out decPurchaseValue, out decFinalValue);

//                _model.MarketValueDec = decMarketValue;
//                _model.PurchaseValueDec = decPurchaseValue;
//                _model.FinalValueDec = decFinalValue;
//            }
//            catch (Exception ex)
//            {
//#if DEBUG
//                MessageBox.Show("CalculateMarketValueAndFinalValue()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
//                    MessageBoxIcon.Error);
//#endif
//                _model.MarketValueDec = 0;
//                _model.FinalValueDec = 0;
//            }
        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is chosen if
        /// a sale should be add or edit.
        /// </summary>
        /// <param name="bFlagEdit">Flag if a sale should be add (true) or edit (false)</param>
        /// <returns>Flag if the input values are correct or not</returns>
        bool CheckInputValues(bool bFlagEdit)
        {
            //try
            //{
            //    bool bErrorFlag = false;

            //    if (bFlagEdit)
            //        _model.ErrorCode = SaleErrorCode.EditSuccessful;
            //    else
            //        _model.ErrorCode = SaleErrorCode.AddSuccessful;

            //    string strDate = _model.Date + " " + _model.Time;

            //    // Check if a sale with the given date and time already exists
            //    foreach (var saleObject in _model.ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
            //    {
            //        // Check if a sale should be added or a sale should be edit
            //        if (!bFlagEdit)
            //        {
            //            // By an Add all dates must be checked
            //            if (saleObject.Date == strDate)
            //            {
            //                _model.ErrorCode = SaleErrorCode.DateExists;
            //                bErrorFlag = true;
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            // By an Edit all sales without the edit entry date and time must be checked
            //            if (saleObject.Date == strDate
            //                && _model.SelectedDate != null
            //                && saleObject.Date != _model.SelectedDate)
            //            {
            //                _model.ErrorCode = SaleErrorCode.DateExists;
            //                bErrorFlag = true;
            //                break;
            //            }
            //        }
            //    }

            //    // Check if a correct volume for the sale is given
            //    decimal decVolume = -1;
            //    if (_model.Volume == @"" && bErrorFlag == false)
            //    {
            //        _model.ErrorCode = SaleErrorCode.VolumeEmpty;
            //        bErrorFlag = true;
            //    }
            //    else if (!decimal.TryParse(_model.Volume, out decVolume) && bErrorFlag == false)
            //    {
            //        _model.ErrorCode = SaleErrorCode.VolumeWrongFormat;
            //        bErrorFlag = true;
            //    }
            //    else if (decVolume <= 0 && bErrorFlag == false)
            //    {
            //        _model.ErrorCode = SaleErrorCode.VolumeWrongValue;
            //        bErrorFlag = true;
            //    }
            //    else if (bErrorFlag == false)
            //        _model.VolumeDec = decVolume;

            //    // Check if a correct price for the sale is given
            //    decimal decPrice = -1;
            //    if (_model.SharePrice == @"" && bErrorFlag == false)
            //    {
            //        _model.ErrorCode = SaleErrorCode.SharePricEmpty;
            //        bErrorFlag = true;
            //    }
            //    else if (!decimal.TryParse(_model.SharePrice, out decPrice) && bErrorFlag == false)
            //    {
            //        _model.ErrorCode = SaleErrorCode.SharePriceWrongFormat;
            //        bErrorFlag = true;
            //    }
            //    else if (decPrice <= 0 && bErrorFlag == false)
            //    {
            //        _model.ErrorCode = SaleErrorCode.SharePriceWrongValue;
            //        bErrorFlag = true;
            //    }
            //    else if (bErrorFlag == false)
            //        _model.SharePricedec = decPrice;

            //    // Costs input check
            //    if (_model.Costs != "" && bErrorFlag == false)
            //    {
            //        decimal decCosts = 0;
            //        if (!decimal.TryParse(_model.Costs, out decCosts) && bErrorFlag == false)
            //        {
            //            _model.ErrorCode = SaleErrorCode.CostsWrongFormat;
            //            bErrorFlag = true;
            //        }
            //        else if (decCosts < 0 && bErrorFlag == false)
            //        {
            //            _model.ErrorCode = SaleErrorCode.CostsWrongValue;
            //            bErrorFlag = true;
            //        }
            //        else if (bErrorFlag == false)
            //            _model.CostsDec = decCosts;
            //    }

            //    // Reduction input check
            //    if (_model.Reduction != "" && bErrorFlag == false)
            //    {
            //        decimal decReduction = 0;
            //        if (!decimal.TryParse(_model.Reduction, out decReduction) && bErrorFlag == false)
            //        {
            //            _model.ErrorCode = SaleErrorCode.ReductionWrongFormat;
            //            bErrorFlag = true;
            //        }
            //        else if (decReduction < 0 && bErrorFlag == false)
            //        {
            //            _model.ErrorCode = SaleErrorCode.ReductionWrongValue;
            //            bErrorFlag = true;
            //        }
            //        else if (bErrorFlag == false)
            //            _model.ReductionDec = decReduction;
            //    }

            //    // Check if a given document exists
            //    if (_model.Document == null)
            //        _model.Document = @"";
            //    else if (_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
            //    {
            //        _model.ErrorCode = SaleErrorCode.DocumentDirectoryDoesNotExits;
            //        bErrorFlag = true;
            //    }
            //    else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
            //    {
            //        _model.Document = @"";
            //        _model.ErrorCode = SaleErrorCode.DocumentFileDoesNotExists;
            //        bErrorFlag = true;
            //    }

            //    return bErrorFlag;
            //}
            //catch
            //{
            //    _model.ErrorCode = SaleErrorCode.InputValuesInvalid;
            //    return true;
            //}

            // TODO remove
            return true;
        }
    }
}
