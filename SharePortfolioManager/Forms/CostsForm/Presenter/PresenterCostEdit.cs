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
using SharePortfolioManager.Forms.CostsForm.Model;
using SharePortfolioManager.Forms.CostsForm.View;
using System;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.CostsForm.Presenter
{
    class PresenterCostEdit
    {
        private readonly IModelCostEdit _model;
        private readonly IViewCostEdit _view;

        public PresenterCostEdit(IViewCostEdit view, IModelCostEdit model)
        {
            this._view = view;
            this._model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValues += OnViewFormatInputValues;
            view.AddCost += OnAddCost;
            view.EditCost += OnEditCost;
            view.DeleteCost += OnDeleteCost;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.Costs = _model.Costs;
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
            _model.UpdateCost = _view.UpdateCost;
            _model.SelectedDate = _view.SelectedDate;
            _model.PartOfABuy = _view.PartOfABuy;
            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Costs = _view.Costs;
            _model.Document = _view.Document;

            // TODO
            //CalculateMarketValueAndFinalValue();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        private void OnAddCost(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateCost))
            {
                string strDateTime = _model.Date + " " + _model.Time;
                if (_model.ShareObjectFinalValue.AddCost(false, strDateTime, _model.CostsDec, _model.Document))
                {
                    _model.ErrorCode = CostErrorCode.AddSuccessful;
                }
                else
                {
                    _model.ErrorCode = CostErrorCode.AddFailed;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditCost(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateCost))
            {
                string strDateTime = _model.Date + " " + _model.Time;

                if (_model.ShareObjectFinalValue.RemoveCost(_model.SelectedDate) && _model.ShareObjectFinalValue.AddCost(_model.PartOfABuy, strDateTime, _model.CostsDec, _model.Document))
                {
                    _model.ErrorCode = CostErrorCode.EditSuccessful;

                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteCost(object sender, EventArgs e)
        {
            // Delete the cost of the selected date
            if (_model.ShareObjectFinalValue.RemoveCost(_model.SelectedDate))
            {
               _model.ErrorCode = CostErrorCode.DeleteSuccessful;
            }
            else
            {
                _model.ErrorCode = CostErrorCode.DeleteFailed;
            }

            // Update error code
            _view.ErrorCode = _model.ErrorCode;

            _view.AddEditDeleteFinish();
        }

// TODO Is this function necessary
//        /// <summary>
//        /// This function calculates the market value and purchase price of the given values
//        /// If a given values are not valid the market value and final value is set to "0"
//        /// </summary>
//        private void CalculateMarketValueAndFinalValue()
//        {
//            try
//            {
//                decimal decMarketValue = 0;
//                decimal decPurchaseValue = 0;
//                decimal decFinalValue = 0;

//                Helper.CalcBuyValues(_model.VolumeDec, _model.SharePricedec, _model.CostsDec,
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
//        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is chosen if
        /// a cost should be add or edit.
        /// </summary>
        /// <param name="bFlagEdit">Flag if a cost should be add (true) or edit (false)</param>
        /// <returns>Flag if the input values are correct or not</returns>
        bool CheckInputValues(bool bFlagEdit)
        {
            try
            {
                bool bErrorFlag = false;

                if (bFlagEdit)
                    _model.ErrorCode = CostErrorCode.EditSuccessful;
                else
                    _model.ErrorCode = CostErrorCode.AddSuccessful;

                string strDate = _model.Date + " " + _model.Time;

                // Check if a cost with the given date and time already exists
                foreach (var costObject in _model.ShareObjectFinalValue.AllCostsEntries.GetAllCostsOfTheShare())
                {
                    // Check if a cost should be added or a cost should be edit
                    if (!bFlagEdit)
                    {
                        // By an Add all dates must be checked
                        if (costObject.CostDate == strDate)
                        {
                            _model.ErrorCode = CostErrorCode.DateExists;
                            bErrorFlag = true;
                            break;
                        }
                    }
                    else
                    {
                        // By an Edit all costs without the edit entry date and time must be checked
                        if (costObject.CostDate == strDate
                            && _model.SelectedDate != null
                            && costObject.CostDate != _model.SelectedDate)
                        {
                            _model.ErrorCode = CostErrorCode.DateExists;
                            bErrorFlag = true;
                            break;
                        }
                    }
                }

                // Costs input check
                if (_model.Costs == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = CostErrorCode.CostsEmpty;
                    bErrorFlag = true;
                }

                if (_model.Costs != @"" && bErrorFlag == false)
                {
                    decimal decCosts = 0;
                    if (!decimal.TryParse(_model.Costs, out decCosts) && bErrorFlag == false)
                    {
                        _model.ErrorCode = CostErrorCode.CostsWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decCosts < 0 && bErrorFlag == false)
                    {
                        _model.ErrorCode = CostErrorCode.CostsWrongValue;
                        bErrorFlag = true;
                    }
                    else if (bErrorFlag == false)
                        _model.CostsDec = decCosts;
                }

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if (_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
                {
                    _model.ErrorCode = CostErrorCode.DocumentDirectoryDoesNotExits;
                    bErrorFlag = true;
                }
                else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.Document = @"";
                    _model.ErrorCode = CostErrorCode.DocumentFileDoesNotExists;
                    bErrorFlag = true;
                }

                return bErrorFlag;
            }
            catch
            {
                _model.ErrorCode = CostErrorCode.InputeValuesInvalid;
                return true;
            }
        }
    }
}
