//MIT License
//
//Copyright(c) 2019 nessie1980(nessie1980 @gmx.de)
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
using SharePortfolioManager.Forms.DividendForm.Model;
using SharePortfolioManager.Forms.DividendForm.View;
using System;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.DividendForm.Presenter
{
    internal class PresenterDividendEdit
    {
        private readonly IModelDividendEdit _model;
        private readonly IViewDividendEdit _view;

        public PresenterDividendEdit(IViewDividendEdit view, IModelDividendEdit model)
        {
            _view = view;
            _model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValuesEventHandler += OnViewFormatInputValues;
            view.AddDividendEventHandler += OnAddDividend;
            view.EditDividendEventHandler += OnEditDividend;
            view.DeleteDividendEventHandler += OnDeleteDividend;
            view.DocumentBrowseEventHandler += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;

            // Copy decimal values from the dividend object to the model
            if (_model.DividendObject != null)
            {
                _model.Date = Convert.ToDateTime(_model.DividendObject.Date).ToShortDateString();
                _model.Time = Convert.ToDateTime(_model.DividendObject.Date).ToShortTimeString();
                _model.EnableFc = _model.DividendObject.EnableFc;
                _model.ExchangeRatioDec = _model.DividendObject.ExchangeRatioDec;
                _model.CultureInfoFc = _model.DividendObject.CultureInfoFc;
                _model.RateDec = _model.DividendObject.RateDec;
                _model.VolumeDec = _model.DividendObject.VolumeDec;
                _model.PayoutDec = _model.DividendObject.PayoutDec;
                _model.Payout = _model.DividendObject.Payout;
                _model.PayoutFcDec = _model.DividendObject.PayoutFcDec;
                _model.PayoutFc = _model.DividendObject.PayoutFc;
                _model.TaxAtSourceDec = _model.DividendObject.TaxAtSourceDec;
                _model.CapitalGainsTaxDec = _model.DividendObject.CapitalGainsTaxDec;
                _model.SolidarityTaxDec = _model.DividendObject.SolidarityTaxDec;
                _model.TaxDec = _model.DividendObject.TaxesSumDec;
                _model.Tax = _model.DividendObject.TaxesSum;
                _model.PayoutAfterTaxDec = _model.DividendObject.PayoutWithTaxesDec;
                _model.PayoutAfterTax = _model.DividendObject.PayoutWithTaxes;
                _model.PriceDec = _model.DividendObject.PriceDec;
                _model.YieldDec = _model.DividendObject.YieldDec;
                _model.Yield = _model.DividendObject.Yield;
                _model.Document = _model.DividendObject.Document;
            }

            _view.Date = _model.Date;
            _view.Time = _model.Time;

            if (_model.DividendObject != null)
                _view.EnableFc = _model.DividendObject.EnableFc;

            _view.ExchangeRatio = _model.ExchangeRatio;
            _view.Rate = _model.Rate;
            _view.Volume = _model.Volume;
            _view.Payout = _model.Payout;
            _view.PayoutFc = _model.PayoutFc;
            _view.TaxAtSource = _model.TaxAtSource;
            _view.CapitalGainsTax = _model.CapitalGainsTax;
            _view.SolidarityTax = _model.SolidarityTax;
            _view.Tax = _model.Tax;
            _view.PayoutAfterTax = _model.PayoutAfterTax;
            _view.Yield = _model.Yield;
            _view.Price = _model.Price;
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
            _model.UpdateDividend = _view.UpdateDividend;
            _model.SelectedGuid = _view.SelectedGuid;
            _model.SelectedDate = _view.SelectedDate;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.ExchangeRatio = _view.ExchangeRatio;
            _model.EnableFc = _view.EnableFc;
            _model.CultureInfoFc = _view.CultureInfoFc;
            _model.Rate = _view.Rate;
            _model.Volume = _view.Volume;
            _model.TaxAtSource = _view.TaxAtSource;
            _model.CapitalGainsTax = _view.CapitalGainsTax;
            _model.SolidarityTax = _view.SolidarityTax;
            _model.Tax = _view.Tax;
            _model.PayoutAfterTax = _view.PayoutAfterTax;
            _model.Yield = _view.Yield;
            _model.Price = _view.Price;
            _model.Document = _view.Document;

            // Copy model values to the dividend object
            if (_model.DividendObject == null)
            {
                _model.DividendObject = new DividendObject(
                    _model.ShareObjectFinalValue.CultureInfo,
                    _model.CultureInfoFc,
                    _model.EnableFc,
                    _model.ExchangeRatioDec,
                    _model.SelectedGuid,
                    _model.Date + " " + _model.Time,
                    _model.RateDec,
                    _model.VolumeDec,
                    _model.TaxAtSourceDec,
                    _model.CapitalGainsTaxDec,
                    _model.SolidarityTaxDec,
                    _model.PriceDec,
                    _model.Document
                    );
            }
            else
            {
                _model.DividendObject.Guid = _model.SelectedGuid;
                _model.DividendObject.Date = _model.Date + " " + _model.Time;
                _model.DividendObject.EnableFc = _model.EnableFc;
                _model.DividendObject.ExchangeRatio = _model.ExchangeRatio;
                _model.DividendObject.CultureInfoFc = _model.CultureInfoFc;
                _model.DividendObject.Rate = _model.Rate;
                _model.DividendObject.Volume = _model.Volume;
                _model.DividendObject.TaxAtSource = _model.TaxAtSource;
                _model.DividendObject.CapitalGainsTax = _model.CapitalGainsTax;
                _model.DividendObject.SolidarityTax = _model.SolidarityTax;
                _model.DividendObject.Price = _model.Price;
                _model.DividendObject.Document = _model.Document;
            }

            _model.UpdateView = _model.DividendObject.UpdateView;

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        private void OnAddDividend(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateDividend))
            {
                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuid = Guid.NewGuid().ToString();

                // If no error occurred add the new dividend to the share
                var bAddFlag = _model.ShareObjectFinalValue.AddDividend(_model.CultureInfoFc, _model.EnableFc, _model.ExchangeRatioDec, strGuid, strDateTime, _model.RateDec, _model.VolumeDec,
                    _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec,
                    _model.PriceDec, _model.Document);

                _model.ErrorCode = bAddFlag ? DividendErrorCode.AddSuccessful : DividendErrorCode.AddFailed;
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditDividend(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateDividend))
            {
                var strDateTime = _model.Date + " " + _model.Time;

                if (_model.ShareObjectFinalValue.RemoveDividend(_model.SelectedGuid, _model.SelectedDate) &&
                    _model.ShareObjectFinalValue.AddDividend(
                        _model.CultureInfoFc, _model.EnableFc, _model.ExchangeRatioDec, _model.SelectedGuid, strDateTime, _model.RateDec, _model.VolumeDec,
                        _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, _model.PriceDec, _model.Document)
                        )
                {
                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
                else
                {
                    _model.ErrorCode = DividendErrorCode.EditFailed;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteDividend(object sender, EventArgs e)
        {
           // Delete the buy of the selected date
            _model.ErrorCode = _model.ShareObjectFinalValue.RemoveDividend(_model.SelectedGuid, _model.SelectedDate) ? DividendErrorCode.DeleteSuccessful : DividendErrorCode.DeleteFailed;

            // Update error code
            _view.ErrorCode = _model.ErrorCode;

            _view.AddEditDeleteFinish();
        }

        /// <summary>
        /// This function opens the document browse dialog and set the chosen document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentBrowse (object sender, EventArgs e)
        {
            try
            {
                var strCurrentFile = string.Empty;

                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                if (_model.DividendObject != null)
                {
                    strCurrentFile = _model.DividendObject.Document;

                    if (Helper.SetDocument(
                            _model.Language.GetLanguageTextByXPath(
                                @"/AddEditFormDividend/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName),
                            strFilter,
                            ref strCurrentFile) == DialogResult.OK)
                        _model.DividendObject.Document = strCurrentFile;
                }
                else
                {
                    strCurrentFile = _model.Document;

                    if (Helper.SetDocument(
                            _model.Language.GetLanguageTextByXPath(
                                @"/AddEditFormDividend/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName),
                            strFilter, ref strCurrentFile) == DialogResult.OK)
                        _model.Document = strCurrentFile;
                }

                UpdateViewWithModel();

                _view.DocumentBrowseFinish();
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND
                var message = $"OnDocumentBrowse()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.ErrorCode = DividendErrorCode.DocumentBrowseFailed;
            }

        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagEdit" it is chosen if
        /// a dividend should be edit or add.
        /// </summary>
        /// <param name="bFlagEdit">Flag if a dividend should be edit</param>
        /// <returns>Flag if the input values are correct or not</returns>
        private bool CheckInputValues(bool bFlagEdit)
        {
            var bErrorFlag = false;

            _model.ErrorCode = bFlagEdit ? DividendErrorCode.EditSuccessful : DividendErrorCode.AddSuccessful;

            try
            {
                // Check foreign CheckBox
                if (_model.EnableFc == CheckState.Checked)
                {
                    // Check if a foreign currency factor is given
                    if (_model.ExchangeRatio == @"")
                    {
                        _model.ErrorCode = DividendErrorCode.ExchangeRatioEmpty;
                        bErrorFlag = true;
                    }
                    else if (!decimal.TryParse(_model.ExchangeRatio, out var decExchangeRatio))
                    {
                        _model.ErrorCode = DividendErrorCode.ExchangeRatioWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decExchangeRatio <= 0)
                    {
                        _model.ErrorCode = DividendErrorCode.ExchangeRatioWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Dividend rate
                if (_model.Rate == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.RateEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Rate, out var decRate) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.RateWrongFormat;
                    bErrorFlag = true;
                }
                else if (decRate <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.RateWrongValue;
                    bErrorFlag = true;
                }

                // Volume
                if (_model.Volume == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.VolumeEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Volume, out var decVolume) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.VolumeWrongFormat;
                    bErrorFlag = true;
                }
                else if (decVolume <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.VolumeWrongValue;
                    bErrorFlag = true;
                }
                else if (decVolume > _model.ShareObjectFinalValue.Volume && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.VolumeMaxValue;
                    bErrorFlag = true;
                }

                // Tax at source
                if (_model.TaxAtSource != "" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.TaxAtSource, out var decTaxAtSource))
                    {
                        _model.ErrorCode = DividendErrorCode.TaxAtSourceWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decTaxAtSource <= 0)
                    {
                        _model.ErrorCode = DividendErrorCode.TaxAtSourceWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Capital gains tax
                if (_model.CapitalGainsTax != @"" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.CapitalGainsTax, out var decCapitalGainsTax))
                    {
                        _model.ErrorCode = DividendErrorCode.CapitalGainsTaxWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decCapitalGainsTax <= 0)
                    {
                        _model.ErrorCode = DividendErrorCode.CapitalGainsTaxWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Solidarity tax
                if (_model.SolidarityTax != @"" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.SolidarityTax, out var decSolidarityTax))
                    {
                        _model.ErrorCode = DividendErrorCode.SolidarityTaxWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decSolidarityTax <= 0)
                    {
                        _model.ErrorCode = DividendErrorCode.SolidarityTaxWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Price
                if (_model.Price == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.PriceEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Price, out var decPrice) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.PriceWrongFormat;
                    bErrorFlag = true;
                }
                else if (decPrice <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.PriceWrongValue;
                    bErrorFlag = true;
                }

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if (_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.DocumentDirectoryDoesNotExists;
                    bErrorFlag = true;
                }
                else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.DocumentFileDoesNotExists;
                    bErrorFlag = true;
                }

                if (!bErrorFlag) return false;

                // Reset string values
                _model.Date = @"";
                _model.Time = @"";
                _model.Document = @"";

                return true;
            }
            catch
            {
                // Reset string values
                _model.Date = @"";
                _model.Time = @"";
                _model.Document = @"";

                _model.ErrorCode = DividendErrorCode.InputValuesInvalid;
                return false;
            }
        }
    }
}
