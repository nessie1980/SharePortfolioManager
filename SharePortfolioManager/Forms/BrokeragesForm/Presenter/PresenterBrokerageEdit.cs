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
//#define DEBUG_BROKERAGE_EDIT_PRESENTER

using SharePortfolioManager.BrokeragesForm.Model;
using SharePortfolioManager.BrokeragesForm.View;
using SharePortfolioManager.Classes;
using System;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.BrokeragesForm.Presenter
{
    internal class PresenterBrokerageEdit
    {
        private readonly IModelBrokerageEdit _model;
        private readonly IViewBrokerageEdit _view;

        public PresenterBrokerageEdit(IViewBrokerageEdit view, IModelBrokerageEdit model)
        {
            _view = view;
            _model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValuesEventHandler += OnViewFormatInputValues;
            view.AddBrokerageEventHandler += OnAddBrokerage;
            view.EditBrokerageEventHandler += OnEditBrokerage;
            view.DeleteBrokerageEventHandler += OnDeleteBrokerage;
            view.DocumentBrowseEventHandler += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.Provision = _model.Provision;
            _view.BrokerFee = _model.BrokerFee;
            _view.TraderPlaceFee = _model.TraderPlaceFee;
            _view.Reduction = _model.Reduction;
            _view.Brokerage = _model.Brokerage;
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
            _model.UpdateBrokerage = _view.UpdateBrokerage;
            _model.SelectedGuid = _view.SelectedGuid;
            _model.SelectedDate = _view.SelectedDate;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.PartOfABuy = _view.PartOfABuy;
            _model.PartOfASale = _view.PartOfASale;
            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Provision = _view.Provision;
            _model.BrokerFee = _view.BrokerFee;
            _model.TraderPlaceFee = _view.TraderPlaceFee;
            _model.Reduction = _view.Reduction;
            _model.Brokerage = _view.Brokerage;
            _model.Document = _view.Document;

            CalculateBrokerageValue();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        private void OnAddBrokerage(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateBrokerage))
            {
                // Set date 
                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuid = Guid.NewGuid().ToString();

                _model.ErrorCode = _model.ShareObjectFinalValue.AddBrokerage(strGuid, false, false, @"-",
                    strDateTime, _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec, _model.Document) ? BrokerageErrorCode.AddSuccessful : BrokerageErrorCode.AddFailed;
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditBrokerage(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateBrokerage))
            {
                // Set date
                var strDateTime = _model.Date + " " + _model.Time;

                if (_model.ShareObjectFinalValue.RemoveBrokerage(_model.SelectedGuid, _model.SelectedDate) && _model.ShareObjectFinalValue.AddBrokerage(_model.SelectedGuid, _model.PartOfABuy, _model.PartOfASale, @"-",
                        strDateTime, _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec, _model.Document))
                {
                    _model.ErrorCode = BrokerageErrorCode.EditSuccessful;

                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteBrokerage(object sender, EventArgs e)
        {
            // Delete the brokerage of the selected date
            _model.ErrorCode = _model.ShareObjectFinalValue.RemoveBrokerage(_model.SelectedGuid, _model.SelectedDate) ? BrokerageErrorCode.DeleteSuccessful : BrokerageErrorCode.DeleteFailed;

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
                        _model.Language.GetLanguageTextByXPath(
                            @"/AddEditFormBrokerage/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName),
                        strFilter, ref strCurrentFile) == DialogResult.OK)
                {
                    _model.Document = strCurrentFile;
                }

                UpdateViewWithModel();

                _view.DocumentBrowseFinish();
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _model.ErrorCode = BrokerageErrorCode.DocumentBrowseFailed;
            }

        }

        /// <summary>
        /// This function calculates the brokerage value of the given values
        /// If a given values are not valid the brokerage value is set to "0"
        /// </summary>
        private void CalculateBrokerageValue()
        {
            try
            {
                Helper.CalcBrokerageValues(_model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, out var decBrokerage);
                
                _model.BrokerageDec = decBrokerage;

#if DEBUG_BROKERAGE_EDIT_PRESENTER
                Console.WriteLine(@"BrokerageDec: {0}", _model.BrokerageDec);
#endif
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _model.BrokerageDec = 0;
            }
        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is chosen if
        /// a brokerage should be add or edit.
        /// </summary>
        /// <param name="bFlagEdit">Flag if a brokerage should be add (true) or edit (false)</param>
        /// <returns>Flag if the input values are correct or not</returns>
        private bool CheckInputValues(bool bFlagEdit)
        {
            try
            {
                var bErrorFlag = false;

                _model.ErrorCode = bFlagEdit ? BrokerageErrorCode.EditSuccessful : BrokerageErrorCode.AddSuccessful;

                // Provision input check
                if (!decimal.TryParse(_model.Provision, out var decProvision))
                {
                    _model.ErrorCode = BrokerageErrorCode.ProvisionWrongFormat;
                    bErrorFlag = true;
                }
                else if (decProvision < 0)
                {
                    _model.ErrorCode = BrokerageErrorCode.ProvisionWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.ProvisionDec = decProvision;

                // Broker fee input check
                if (_model.BrokerFee != string.Empty)
                {
                    if (!decimal.TryParse(_model.BrokerFee, out var decBrokerFee))
                    {
                        _model.ErrorCode = BrokerageErrorCode.BrokerFeeWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decBrokerFee < 0)
                    {
                        _model.ErrorCode = BrokerageErrorCode.BrokerFeeWrongValue;
                        bErrorFlag = true;
                    }
                    else
                        _model.BrokerFeeDec = decBrokerFee;
                }

                // Trader place fee input check
                if (_model.TraderPlaceFee != string.Empty)
                {
                    if (!decimal.TryParse(_model.TraderPlaceFee, out var decTraderPlaceFee))
                    {
                        _model.ErrorCode = BrokerageErrorCode.TraderPlaceFeeWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decTraderPlaceFee < 0)
                    {
                        _model.ErrorCode = BrokerageErrorCode.TraderPlaceFeeWrongValue;
                        bErrorFlag = true;
                    }
                    else
                        _model.TraderPlaceFeeDec = decTraderPlaceFee;
                }
                
                // Reduction input check
                if (_model.Reduction != string.Empty)
                {
                    if (!decimal.TryParse(_model.Reduction, out var decReduction))
                    {
                        _model.ErrorCode = BrokerageErrorCode.ReductionWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decReduction < 0)
                    {
                        _model.ErrorCode = BrokerageErrorCode.ReductionWrongValue;
                        bErrorFlag = true;
                    }
                    else
                        _model.ReductionDec = decReduction;
                }

                // Brokerage input check
                if (_model.Brokerage == string.Empty)
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Brokerage, out var decBrokerageWithReduction))
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageWrongFormat;
                    bErrorFlag = true;
                }
                else if (decBrokerageWithReduction < 0)
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.BrokerageDec = decBrokerageWithReduction;

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if (_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
                {
                    _model.ErrorCode = BrokerageErrorCode.DocumentDirectoryDoesNotExists;
                    bErrorFlag = true;
                }
                else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.Document = @"";
                    _model.ErrorCode = BrokerageErrorCode.DocumentFileDoesNotExists;
                    bErrorFlag = true;
                }

                return bErrorFlag;
            }
            catch(Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _model.ErrorCode = BrokerageErrorCode.InputValuesInvalid;
                return false;
            }
        }
    }
}
