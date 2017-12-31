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
using SharePortfolioManager.Forms.BuysForm.Model;
using SharePortfolioManager.Forms.BuysForm.View;
using System;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.BuysForm.Presenter
{
    internal class PresenterBuyEdit  
    {
        private readonly IModelBuyEdit _model;
        private readonly IViewBuyEdit _view;

        public PresenterBuyEdit(IViewBuyEdit view, IModelBuyEdit model)
        {
            _view = view;
            _model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValues += OnViewFormatInputValues;
            view.AddBuy += OnAddBuy;
            view.EditBuy += OnEditBuy;
            view.DeleteBuy += OnDeleteBuy;
            view.DocumentBrowse += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.Volume = _model.Volume;
            _view.Price = _model.SharePrice;
            _view.MarketValue = _model.MarketValue;
            _view.Costs = _model.Costs;
            _view.Reduction = _model.Reduction;
            _view.FinalValue = _model.FinalValue;
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
            _model.UpdateBuy = _view.UpdateBuy;
            _model.SelectedDate = _view.SelectedDate;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Volume = _view.Volume;
            _model.SharePrice = _view.Price;
            _model.MarketValue = _view.MarketValue;
            _model.Costs = _view.Costs;
            _model.Reduction = _view.Reduction;
            _model.FinalValue = _view.FinalValue;
            _model.Document = _view.Document;

            CalculateMarketValueAndFinalValue();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        private void OnAddBuy(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateBuy))
            {
                var bErrorFlag = false;
                var strDateTime = _model.Date + " " + _model.Time;

                // Cost entry if the costs value is not 0
                if (_model.CostsDec > 0)
                    bErrorFlag = !_model.ShareObjectFinalValue.AddCost(true, false, strDateTime, _model.CostsDec, _model.Document);

                if (_model.ShareObjectFinalValue.AddBuy(strDateTime, _model.VolumeDec, _model.SharePricedec, _model.ReductionDec, _model.CostsDec, _model.Document)
                    && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.AddSuccessful;
                }
                else
                {
                    _model.ErrorCode = BuyErrorCode.AddFailed;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditBuy(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateBuy))
            {
                var strDateTime = _model.Date + " " + _model.Time;

                if (_model.ShareObjectFinalValue.RemoveBuy(_model.SelectedDate) && _model.ShareObjectFinalValue.AddBuy(strDateTime, _model.VolumeDec, _model.SharePricedec, _model.ReductionDec, _model.CostsDec, _model.Document))
                {
                    var bFlagCostEdit = true;

                    // Check if an old cost entry must be deleted
                    if (_model.ShareObjectFinalValue.AllCostsEntries.GetCostObjectByDateTime(strDateTime) != null)
                        bFlagCostEdit = _model.ShareObjectFinalValue.RemoveCost(strDateTime);

                    // Check if a new cost entry must be made
                    if (bFlagCostEdit && _model.CostsDec > 0)
                        bFlagCostEdit = _model.ShareObjectFinalValue.AddCost(true, false, strDateTime, _model.CostsDec, _model.Document);

                    if (bFlagCostEdit)
                    {
                        _model.ErrorCode = BuyErrorCode.EditSuccessful;
                    }

                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
            }

            //_model.ErrorCode = BuyErrorCode.EditFailed;
            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteBuy(object sender, EventArgs e)
        {
            // If this is the first buy of all. This buy can´t be deleted. Only if the whole share will be deleted
            if (_model.ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare().Count > 1)
            {
                // Delete the buy of the selected date
                if (_model.ShareObjectFinalValue.RemoveBuy(_model.SelectedDate))
                {
                    // Check if a cost object exists
                    if (_model.ShareObjectFinalValue.AllCostsEntries.GetCostObjectByDateTime(_model.SelectedDate) != null)
                    {
                        _model.ErrorCode = _model.ShareObjectFinalValue.RemoveCost(_model.SelectedDate) ? BuyErrorCode.DeleteSuccessful : BuyErrorCode.DeleteFailed;
                    }
                    else
                    {
                        _model.ErrorCode = BuyErrorCode.DeleteSuccessful;
                    }
                }
                else
                {
                    _model.ErrorCode = BuyErrorCode.DeleteFailed;
                }
            }
            else
                _model.ErrorCode = BuyErrorCode.DeleteFailedUnerasable;

            // Update error code
            _view.ErrorCode = _model.ErrorCode;

            _view.AddEditDeleteFinish();
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
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                _model.Document = Helper.SetDocument(_model.Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName), strFilter, _model.Document);

                UpdateViewWithModel();

                _view.DocumentBrowseFinish();
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"OnDocumentBrowse()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.ErrorCode = BuyErrorCode.DocumentBrowseFailed;
            }

        }

        /// <summary>
        /// This function calculates the market value and purchase price of the given values
        /// If a given values are not valid the market value and final value is set to "0"
        /// </summary>
        private void CalculateMarketValueAndFinalValue()
        {
            try
            {
                Helper.CalcBuyValues(_model.VolumeDec, _model.SharePricedec, _model.CostsDec,
                    _model.ReductionDec, out var decMarketValue, out var decPurchaseValue, out var decFinalValue);

                _model.MarketValueDec = decMarketValue;
                _model.PurchaseValueDec = decPurchaseValue;
                _model.FinalValueDec = decFinalValue;
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"CalculateMarketValueAndFinalValue()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.MarketValueDec = 0;
                _model.FinalValueDec = 0;
            }
        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is chosen if
        /// a buy should be add or edit.
        /// </summary>
        /// <param name="bFlagEdit">Flag if a buy should be add (true) or edit (false)</param>
        /// <returns>Flag if the input values are correct or not</returns>
        private bool CheckInputValues(bool bFlagEdit)
        {
            try
            {
                var bErrorFlag = false;

                _model.ErrorCode = bFlagEdit ? BuyErrorCode.EditSuccessful : BuyErrorCode.AddSuccessful;

                var strDate = _model.Date + " " + _model.Time;

                // Check if a buy with the given date and time already exists
                foreach (var buyObject in _model.ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare())
                {
                    // Check if a buy should be added or a buy should be edit
                    if (!bFlagEdit)
                    {
                        // By an Add all dates must be checked
                        if (buyObject.Date != strDate) continue;

                        _model.ErrorCode = BuyErrorCode.DateExists;
                        bErrorFlag = true;
                        break;
                    }

                    // By an Edit all buys without the edit entry date and time must be checked
                    if (buyObject.Date != strDate || _model.SelectedDate == null ||
                        buyObject.Date == _model.SelectedDate) continue;

                    _model.ErrorCode = BuyErrorCode.DateExists;
                    bErrorFlag = true;
                    break;
                }

                // Check if a correct volume for the buy is given
                if (_model.Volume == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.VolumeEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Volume, out var decVolume) && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.VolumeWrongFormat;
                    bErrorFlag = true;
                }
                else if (decVolume <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.VolumeWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.VolumeDec = decVolume;

                // Check if a correct price for the buy is given
                if (_model.SharePrice == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.SharePricEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.SharePrice, out var decPrice) && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.SharePriceWrongFormat;
                    bErrorFlag = true;
                }
                else if (decPrice <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.SharePriceWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.SharePricedec = decPrice;

                // Costs input check
                if (_model.Costs != "" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.Costs, out var decCosts))
                    {
                        _model.ErrorCode = BuyErrorCode.CostsWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decCosts < 0)
                    {
                        _model.ErrorCode = BuyErrorCode.CostsWrongValue;
                        bErrorFlag = true;
                    }
                    else
                        _model.CostsDec = decCosts;
                }

                // Reduction input check
                if (_model.Reduction != "" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.Reduction, out var decReduction))
                    {
                        _model.ErrorCode = BuyErrorCode.ReductionWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decReduction < 0)
                    {
                        _model.ErrorCode = BuyErrorCode.ReductionWrongValue;
                        bErrorFlag = true;
                    }
                    else
                        _model.ReductionDec = decReduction;
                }

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if(_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
                {
                    _model.ErrorCode = BuyErrorCode.DocumentDirectoryDoesNotExits;
                    bErrorFlag = true;
                }
                else if(_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.Document = @"";
                    _model.ErrorCode = BuyErrorCode.DocumentFileDoesNotExists;
                    bErrorFlag = true;
                }

                return bErrorFlag;
            }
            catch
            {
                _model.ErrorCode = BuyErrorCode.InputValuesInvalid;
                return true;
            }
        }
    }
}
