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
using SharePortfolioManager.Forms.DividendForm.Model;
using SharePortfolioManager.Forms.DividendForm.View;
using System;
using System.IO;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.DividendForm.Presenter
{
    class PresenterDividendEdit
    {
        private readonly IModelDividendEdit _model;
        private readonly IViewDividendEdit _view;

        public PresenterDividendEdit(IViewDividendEdit view, IModelDividendEdit model)
        {
            this._view = view;
            this._model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValues += OnViewFormatInputValues;
            view.AddDividend += OnAddDividend;
            view.EditDividend += OnEditDividend;
            view.DeleteDividend += OnDeleteDividend;
            view.DocumentBrowse += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;

            // Copy decimal values from the dividend object to the model
            if (_model.DividendObject != null)
            {
                _model.Date = Convert.ToDateTime(_model.DividendObject.DateTime).ToShortDateString();
                _model.Time = Convert.ToDateTime(_model.DividendObject.DateTime).ToShortTimeString();
                _model.EnableFC = _model.DividendObject.EnableFC;
                _model.ExchangeRatioDec = _model.DividendObject.ExchangeRatioDec;
                _model.CultureInfoFC = _model.DividendObject.CultureInfoFC;
                _model.RateDec = _model.DividendObject.RateDec;
                _model.VolumeDec = _model.DividendObject.VolumeDec;
                _model.PayoutDec = _model.DividendObject.PayoutDec;
                _model.PayoutFCDec = _model.DividendObject.PayoutFCDec;
                _model.TaxAtSourceDec = _model.DividendObject.TaxAtSourceDec;
                _model.CapitalGainsTaxDec = _model.DividendObject.CapitalGainsTaxDec;
                _model.SolidarityTaxDec = _model.DividendObject.SolidarityTaxDec;
                _model.TaxDec = _model.DividendObject.TaxesSumDec;
                _model.PayoutAfterTaxDec = _model.DividendObject.PayoutWithTaxesDec;
                _model.YieldDec = _model.DividendObject.YieldDec;
                _model.PriceDec = _model.DividendObject.PriceDec;
                _model.Document = _model.DividendObject.Document;
            }

            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.EnableFC = _model.DividendObject.EnableFC;
            _view.ExchangeRatio = _model.ExchangeRatio;
            _view.Rate = _model.Rate;
            _view.Volume = _model.Volume;
            _view.Payout = _model.Payout;
            _view.PayoutFC = _model.PayoutFC;
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
            UpdateModelwithView();
        }

        private void UpdateModelwithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ErrorCode = _view.ErrorCode;
            _model.UpdateDividend = _view.UpdateDividend;
            _model.SelectedDate = _view.SelectedDate;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.ExchangeRatio = _view.ExchangeRatio;
            _model.EnableFC = _view.EnableFC;
            _model.CultureInfoFC = _view.CultureInfoFC;
            _model.Rate = _view.Rate;
            _model.Volume = _view.Volume;
            _model.TaxAtSource = _view.TaxAtSource;
            _model.CapitalGainsTax = _view.CapitalGainsTax;
            _model.SolidarityTax = _view.SolidarityTax;
            _model.PayoutAfterTax = _view.PayoutAfterTax;
            _model.Price = _view.Price;
            _model.Document = _view.Document;

            // Copy model values to the dividend object
            if (_model.DividendObject == null)
            {
                _model.DividendObject = new DividendObject(
                    _model.ShareObjectFinalValue.CultureInfo,
                    _model.CultureInfoFC,
                    _model.EnableFC,
                    _model.ExchangeRatioDec,
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
                _model.DividendObject.DateTime = _model.Date + " " + _model.Time;
                _model.DividendObject.EnableFC = _model.EnableFC;
                _model.DividendObject.ExchangeRatio = _model.ExchangeRatio;
                _model.DividendObject.CultureInfoFC = _model.CultureInfoFC;
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
                bool bErrorFlag = false;
                string strDateTime = _model.Date + " " + _model.Time;

                // If no error occurred add the new dividend to the share
                if (bErrorFlag == false)
                {
                    bool bAddFlag = false;

                    bAddFlag = _model.ShareObjectFinalValue.AddDividend(_model.CultureInfoFC, _model.EnableFC, _model.ExchangeRatioDec, strDateTime, _model.RateDec, _model.VolumeDec,
                        _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec,
                        _model.PriceDec, _model.Document);

                    if (bAddFlag)
                    {
                        _model.ErrorCode = DividendErrorCode.AddSuccessful;
                    }
                    else
                    {
                        _model.ErrorCode = DividendErrorCode.AddFailed;
                    }
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditDividend(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateDividend))
            {
                string strDateTime = _model.Date + " " + _model.Time;

                if (_model.ShareObjectFinalValue.RemoveDividend(_model.SelectedDate) &&
                    _model.ShareObjectFinalValue.AddDividend(
                        _model.CultureInfoFC, _model.EnableFC, _model.ExchangeRatioDec, _model.Date, _model.RateDec, _model.VolumeDec,
                        _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, _model.PriceDec, _model.Document)
                        )
                {
                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteDividend(object sender, EventArgs e)
        {
           // Delete the buy of the selected date
            if (_model.ShareObjectFinalValue.RemoveDividend(_model.SelectedDate))
            {
                _model.ErrorCode = DividendErrorCode.DeleteSuccessful;
            }
            else
            {
                _model.ErrorCode = DividendErrorCode.DeleteFailed;
            }

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
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                if (_model.DividendObject != null)
                    _model.DividendObject.Document = Helper.SetDocument(_model.Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName), strFilter, _model.DividendObject.Document);
                else
                    _model.Document = Helper.SetDocument(_model.Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName), strFilter, _model.Document);

                UpdateViewWithModel();

                _view.DocumentBrowseFinish();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("OnDocumentBrowse()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
            bool bErrorFlag = false;

            if (bFlagEdit)
                _model.ErrorCode = DividendErrorCode.EditSuccessful;
            else
                _model.ErrorCode = DividendErrorCode.AddSuccessful;

            string strDateTime = _model.Date + " " + _model.Time;

            try
            {
                // Check if a dividend with the given date and time already exists
                foreach (var dividend in _model.ShareObjectFinalValue.AllDividendEntries.GetAllDividendsOfTheShare())
                {
                    if (!bFlagEdit)
                    {
                        // By an Add all dates and times must be checked
                        if (dividend.DateTime == strDateTime)
                        {
                            _model.ErrorCode = DividendErrorCode.DateExists;
                            bErrorFlag = true;
                            break;
                        }
                    }
                    else
                    {
                        // By an Edit all dividends without the edit entry date and time must be checked
                        if (dividend.DateTime == strDateTime
                            && _model.SelectedDate != null
                            && dividend.DateTime != _model.SelectedDate)
                        {
                            _model.ErrorCode = DividendErrorCode.DateWrongFormat;
                            bErrorFlag = true;
                            break;
                        }
                    }
                }

                // Check foreign CheckBox
                if (_model.EnableFC == CheckState.Checked && bErrorFlag == false)
                {
                    // Check if a foreign currency factor is given
                    decimal decExchangeRatio = -1;
                    if (_model.ExchangeRatio == @"")
                    {
                        _model.ErrorCode = DividendErrorCode.ExchangeRatioEmpty;
                        bErrorFlag = true;
                    }
                    else if (!decimal.TryParse(_model.ExchangeRatio, out decExchangeRatio) && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.ExchangeRatioWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decExchangeRatio <= 0 && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.ExchangeRatioWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Dividend rate
                decimal decRate = -1;
                if (_model.Rate == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.RateEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Rate, out decRate) && bErrorFlag == false)
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
                decimal decVolume = -1;
                if (_model.Volume == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.VolumeEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Volume, out decVolume) && bErrorFlag == false)
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
                decimal decTaxAtSource = -1;
                if (_model.TaxAtSource != "" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.TaxAtSource, out decTaxAtSource) && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.TaxAtSourceWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decTaxAtSource <= 0 && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.TaxAtSourceWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Capital gains tax
                decimal decCapitalGainsTax = -1;
                if (_model.CapitalGainsTax != @"" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.CapitalGainsTax, out decCapitalGainsTax) && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.CapitalGainsTaxWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decCapitalGainsTax <= 0 && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.CapitalGainsTaxWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Solidarity tax
                decimal decSolidarityTax = -1;
                if (_model.SolidarityTax != @"" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.SolidarityTax, out decSolidarityTax) && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.SolidarityTaxWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decSolidarityTax <= 0 && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.SolidarityTaxWrongValue;
                        bErrorFlag = true;
                    }
                }

                // Price
                decimal decPrice = -1;
                if (_model.Price == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.PriceEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Price, out decPrice) && bErrorFlag == false)
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
                    _model.ErrorCode = DividendErrorCode.DirectoryDoesNotExists;
                    bErrorFlag = true;
                }
                else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.FileDoesNotExists;
                    bErrorFlag = true;
                }

                if (bErrorFlag)
                {
                    // Reset string values
                    _model.Date = @"";
                    _model.Time = @"";
                    _model.Document = @"";
                }

                return bErrorFlag;
            }
            catch (Exception ex)
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
