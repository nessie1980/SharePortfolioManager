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
//#define DEBUG_BUY_EDIT_PRESENTER

using SharePortfolioManager.BuysForm.Model;
using SharePortfolioManager.BuysForm.View;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Brokerage;
using System;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.BuysForm.Presenter
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
            _view.OrderNumber = _model.OrderNumber;
            _view.Volume = _model.Volume;
            _view.VolumeSold = _model.VolumeSold;
            _view.Price = _model.SharePrice;
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
            _model.OrderNumber = _view.OrderNumber;
            _model.Volume = _view.Volume;
            _model.VolumeSold = _view.VolumeSold;
            _model.SharePrice = _view.Price;
            _model.Provision = _view.Provision;
            _model.BrokerFee = _view.BrokerFee;
            _model.TraderPlaceFee = _view.TraderPlaceFee;
            _model.Reduction = _view.Reduction;
            _model.Brokerage = _view.Brokerage;
            _model.BuyValue = _view.BuyValue;
            _model.BuyValueBrokerageReduction = _view.BuyValueBrokerageReduction;
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
                BrokerageReductionObject brokerage = null;

                // Set date
                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuidBuy = Guid.NewGuid().ToString();

                // Brokerage entry if any brokerage value is not 0
                if (_model.ProvisionDec != 0 || _model.BrokerFeeDec != 0 || _model.TraderPlaceFeeDec != 0 || _model.ReductionDec != 0)
                {
                    // Generate Guid
                    var strGuidBrokerage = Guid.NewGuid().ToString();

                    // Add brokerage
                    bErrorFlag = !_model.ShareObjectFinalValue.AddBrokerage(strGuidBrokerage, true, false, strGuidBuy,
                        strDateTime,
                        _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec,
                        _model.Document);

                    // Get brokerage object
                    brokerage =
                        _model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuidBrokerage, strDateTime);
                }
                
                // Add buy
                if (bErrorFlag == false &&
                    _model.ShareObjectFinalValue.AddBuy(strGuidBuy, _model.OrderNumber, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec,
                        brokerage, _model.Document) &&
                    _model.ShareObjectMarketValue.AddBuy(strGuidBuy, _model.OrderNumber, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec,
                        brokerage,_model.Document)
                    )
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
                if (bFlagBrokerageEdit && (_model.ProvisionDec != 0 || _model.BrokerFeeDec != 0 || _model.TraderPlaceFeeDec != 0 || _model.ReductionDec != 0))
                {
                    bFlagBrokerageEdit = _model.ShareObjectFinalValue.AddBrokerage(guidBrokerage, true, false, _model.SelectedGuid, strDateTime,
                        _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec, _model.Document);
                }

                // Get brokerage object
                var brokerage = _model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByGuidDate(guidBrokerage, strDateTime);

                if (bFlagBrokerageEdit)
                {
                    if (_model.ShareObjectFinalValue.RemoveBuy(_model.SelectedGuid, _model.SelectedDate) &&
                    _model.ShareObjectFinalValue.AddBuy(_model.SelectedGuid, _model.OrderNumber, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec,
                        brokerage, _model.Document) &&
                    _model.ShareObjectMarketValue.RemoveBuy(_model.SelectedGuid, _model.SelectedDate) &&
                    _model.ShareObjectMarketValue.AddBuy(_model.SelectedGuid, _model.OrderNumber, strDateTime, _model.VolumeDec, _model.VolumeSoldDec, _model.SharePriceDec,
                        brokerage, _model.Document)
                    )
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
                var strCurrentFile = _model.Document;

                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                if (Helper.SetDocument(
                        _model.Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title",
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
                Helper.CalcBuyValues(_model.VolumeDec, _model.SharePriceDec,
                    _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec,
                    out var decBuyValue, out var decBuyValueReduction, out var decBuyValueBrokerage, out var decBuyValueBrokerageReduction,
                    out var decBrokerage, out var decBrokerageReduction);

                _model.BrokerageReductionDec = decBrokerageReduction;
                _model.BrokerageDec = decBrokerage;
                _model.BuyValueDec = decBuyValue;
                _model.BuyValueReductionDec = decBuyValueReduction;
                _model.BuyValueBrokerageDec = decBuyValueBrokerage;
                _model.BuyValueBrokerageReductionDec = decBuyValueBrokerageReduction;

#if DEBUG_BUY_EDIT_PRESENTER
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

                // Check if a order number for the buy is given and the order number does not exits already if a new buy should be added
                if (_model.OrderNumber == @"")
                {
                    _model.ErrorCode = BuyErrorCode.OrderNumberEmpty;
                    bErrorFlag = true;
                }
                else if (_model.ShareObjectFinalValue.AllBuyEntries.OrderNumberAlreadyExists(_model.OrderNumber) && bFlagEdit == false)
                {
                    _model.ErrorCode = BuyErrorCode.OrderNumberExists;
                    bErrorFlag = true;
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

                // Provision input check
                if (_model.Provision != "" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.Provision, out var decProvision))
                    {
                        _model.ErrorCode = BuyErrorCode.ProvisionWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decProvision < 0)
                    {
                        _model.ErrorCode = BuyErrorCode.ProvisionWrongValue;
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
                        _model.ErrorCode = BuyErrorCode.BrokerFeeWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decBrokerFee < 0)
                    {
                        _model.ErrorCode = BuyErrorCode.BrokerFeeWrongValue;
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
                        _model.ErrorCode = BuyErrorCode.TraderPlaceFeeWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decTraderPlaceFee < 0)
                    {
                        _model.ErrorCode = BuyErrorCode.TraderPlaceFeeWrongValue;
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

                // Brokerage input check
                if (_model.Brokerage == @"")
                {
                    _model.ErrorCode = BuyErrorCode.BrokerageEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Brokerage, out var decBrokerage))
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

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if(_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
                {
                    _model.ErrorCode = BuyErrorCode.DocumentDirectoryDoesNotExists;
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
            catch(Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _model.ErrorCode = BuyErrorCode.InputValuesInvalid;
                return true;
            }
        }
    }
}
