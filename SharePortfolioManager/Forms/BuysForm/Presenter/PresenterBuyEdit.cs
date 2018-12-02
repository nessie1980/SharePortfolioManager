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

using System;
using System.IO;
#if DEBUG
using System.Windows.Forms;
#endif
using SharePortfolioManager.Classes;
using SharePortfolioManager.Forms.BuysForm.Model;
using SharePortfolioManager.Forms.BuysForm.View;

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
            view.FormatInputValuesEventHandler += OnViewFormatInputValues;
            view.AddBuyEventHandler += OnAddBuy;
            view.EditBuyEventHandler += OnEditBuy;
            view.DeleteBuyEventHandler += OnDeleteBuy;
            view.DocumentBrowseEventHandler += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.Volume = _model.Volume;
            _view.VolumeSold = _model.VolumeSold;
            _view.Price = _model.SharePrice;
            _view.MarketValue = _model.MarketValue;
            _view.Brokerage = _model.Brokerage;
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
            UpdateModelWithView();
        }

        private void UpdateModelWithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ErrorCode = _view.ErrorCode;
            _model.UpdateBuy = _view.UpdateBuy;
            _model.SelectedGuid = _view.SelectedGuid;
            _model.SelectedDate = _view.SelectedDate;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Volume = _view.Volume;
            _model.VolumeSold = _view.VolumeSold;
            _model.SharePrice = _view.Price;
            _model.MarketValue = _view.MarketValue;
            _model.Brokerage = _view.Brokerage;
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

                // Set date
                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuidBuy = Guid.NewGuid().ToString();

                // Brokerage entry if the brokerage value is not 0
                if (_model.BrokerageDec > 0)
                {
                    // Generate Guid
                    var strGuidBrokerage = Guid.NewGuid().ToString();

                    // Calculate brokerage
                    var brokerage = _model.BrokerageDec - _model.ReductionDec;
                    bErrorFlag = !_model.ShareObjectFinalValue.AddBrokerage(strGuidBrokerage, true, false, strGuidBuy, strDateTime, brokerage,
                        _model.Document);
                }
                
                if (_model.ShareObjectFinalValue.AddBuy(strGuidBuy, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec, _model.ReductionDec, _model.BrokerageDec, _model.Document) &&
                    _model.ShareObjectMarketValue.AddBuy(strGuidBuy, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec, _model.Document)
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
                // Set date
                var strDateTime = _model.Date + " " + _model.Time;

                if (_model.ShareObjectFinalValue.RemoveBuy(_model.SelectedGuid, _model.SelectedDate) &&
                    _model.ShareObjectFinalValue.AddBuy(_model.SelectedGuid, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec, _model.ReductionDec, _model.BrokerageDec, _model.Document) &&
                    _model.ShareObjectMarketValue.RemoveBuy(_model.SelectedGuid, _model.SelectedDate) &&
                    _model.ShareObjectMarketValue.AddBuy(_model.SelectedGuid, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec, _model.Document)
                    )
                {
                    var bFlagBrokerageEdit = true;

                    var guidBrokerage = @"";

                    // Check if an old brokerage entry must be deleted
                    if (_model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByBuyGuid(_model.SelectedGuid,
                            strDateTime) != null)
                    {
                        // Get brokerage GUID
                        guidBrokerage = _model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByBuyGuid(
                            _model.SelectedGuid,
                            strDateTime).Guid;
                        bFlagBrokerageEdit = _model.ShareObjectFinalValue.RemoveBrokerage(guidBrokerage, strDateTime);
                    }

                    // Check if a new brokerage entry must be made
                    if (bFlagBrokerageEdit && _model.BrokerageDec > 0)
                    {
                        // Calculate brokerage
                        var brokerage = _model.BrokerageDec - _model.ReductionDec;
                        bFlagBrokerageEdit = _model.ShareObjectFinalValue.AddBrokerage(guidBrokerage, true, false, _model.SelectedGuid, strDateTime,
                            brokerage, _model.Document);
                    }

                    if (bFlagBrokerageEdit)
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
                if (_model.ShareObjectFinalValue.RemoveBuy(_model.SelectedGuid, _model.SelectedDate) &&
                    _model.ShareObjectMarketValue.RemoveBuy(_model.SelectedGuid, _model.SelectedDate))
                {
                    // Check if a brokerage object exists
                    if (_model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByBuyGuid(_model.SelectedGuid, _model.SelectedDate) != null)
                    {
                        var guidBrokerage = _model.ShareObjectFinalValue.AllBrokerageEntries
                            .GetBrokerageObjectByBuyGuid(_model.SelectedGuid, _model.SelectedDate).Guid;
                        _model.ErrorCode = _model.ShareObjectFinalValue.RemoveBrokerage(guidBrokerage, _model.SelectedDate) ? BuyErrorCode.DeleteSuccessful : BuyErrorCode.DeleteFailed;
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
                Helper.CalcBuyValues(_model.VolumeDec, _model.SharePriceDec, _model.BrokerageDec,
                    _model.ReductionDec, out var decMarketValue, out var decPurchaseValue, out var decFinalValue, out var decBrokerageReduction);

                _model.MarketValueDec = decMarketValue;
                _model.PurchaseValueDec = decPurchaseValue;
                _model.FinalValueDec = decFinalValue;
                _model.BrokerageDec = decBrokerageReduction;
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

                // Check if a correct volume for the buy is given
                if (_model.Volume == @"")
                {
                    _model.ErrorCode = BuyErrorCode.VolumeEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Volume, out var decVolume))
                {
                    _model.ErrorCode = BuyErrorCode.VolumeWrongFormat;
                    bErrorFlag = true;
                }
                else if (decVolume <= 0)
                {
                    _model.ErrorCode = BuyErrorCode.VolumeWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.VolumeDec = decVolume;

                // Check if a correct price for the buy is given
                if (_model.SharePrice == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = BuyErrorCode.SharePriceEmpty;
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
                    _model.SharePriceDec = decPrice;

                // Brokerage input check
                if (_model.Brokerage != "" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.Brokerage, out var decBrokerage))
                    {
                        _model.ErrorCode = BuyErrorCode.BrokerageWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decBrokerage < 0)
                    {
                        _model.ErrorCode = BuyErrorCode.BrokerageWrongValue;
                        bErrorFlag = true;
                    }
                    else
                        _model.BrokerageDec = decBrokerage;
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
