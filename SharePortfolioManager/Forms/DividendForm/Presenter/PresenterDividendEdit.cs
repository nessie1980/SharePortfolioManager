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
            //view.EditDividend += OnEditDividend;
            //view.DeleteDividend += OnDeleteDividend;
            view.EditTax += OnEditTax;
            view.DocumentBrowse += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ShareObject = _model.ShareObject;
            _view.ErrorCode = _model.ErrorCode;

            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.EnableFC = _model.EnableFC;
            _view.CultureInfoFC = _model.CultureInfoFC;
            _view.Rate = _model.Rate;
            _view.RateFC = _model.RateFC;
            _view.Volume = _model.Volume;
            _view.LossBalance = _model.LossBalance;
            _view.Payout = _model.Payout;
            _view.TaxValuesCurrent = _model.TaxValuesCurrent;
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
            _model.ShareObject = _view.ShareObject;
            _model.ErrorCode = _view.ErrorCode;
            _model.UpdateDividend = _view.UpdateDividend;

            _model.TaxValuesCurrent = _view.TaxValuesCurrent;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.ExchangeRatio = _view.ExchangeRatio;
            _model.EnableFC = _view.EnableFC;
            _model.CultureInfoFC = _view.CultureInfoFC;
            _model.Rate = _view.Rate;
            _model.RateFC = _view.RateFC;
            _model.Volume = _view.Volume;
            _model.LossBalance = _view.LossBalance;
            _model.Payout = _view.Payout;
            _model.Tax = _view.Tax;
            _model.PayoutAfterTax = _view.PayoutAfterTax;
            _model.Yield = _view.Yield;
            _model.Price = _view.Price;
            _model.Document = _view.Document;

            // Calculations
            CalculateLossBalanceForeignCurrency();
            CalculateDividendPayOut();
            CalculateDividendPayOutFC();
            CalculateDividendYield();
            CaluclateDividendRateFromDividendRateFC();
            CalculateTaxes();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        /// <summary>
        /// This function checks the input values and then adds the new dividend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    bAddFlag = _model.ShareObject.AddDividend(strDateTime, _model.TaxValuesCurrent, _model.RateDec,
                        _model.LossBalanceDec, _model.PriceDec, _model.VolumeDec, _model.Document);

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

            MessageBox.Show(_model.ErrorCode.ToString(), @"Info", MessageBoxButtons.OK);
        }

        /// <summary>
        /// This function opens the edit dialog for the taxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditTax(object sender, EventArgs e)
        {
            // Check if all inputs are made for entering taxes
            if ( _model.PayoutDec > 0 && _model.EnableFC == CheckState.Unchecked
                ||
                _model.PayoutDec > 0 && _model.ExchangeRatioDec > 0 && _model.EnableFC == CheckState.Checked)
            {
                _model.TaxValuesCurrent.ValueWithoutTaxes = _model.PayoutDec;
                _model.TaxValuesCurrent.ExchangeRatio = _model.ExchangeRatioDec;

                ShareDividendTaxesEdit dlgShareDividendTaxesEdit = new ShareDividendTaxesEdit(_model.TaxValuesCurrent, _model.Logger, _model.Language, _model.LanguageName);
                dlgShareDividendTaxesEdit.ShowDialog();

                _model.TaxDec = _model.TaxValuesCurrent.TaxesValue;
                _model.TaxFCDec = _model.TaxValuesCurrent.TaxesValueFC;
            }
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
                _model.Document = Helper.SetDocument(_model.Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName), strFilter, _model.Document);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnAddDividendDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.ErrorCode = DividendErrorCode.DocumentBrowseFailed;

                //// Add status message
                //Helper.AddStatusMessage(toolStripStatusLabelMessage,
                //    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ChoseDocumentFailed", LanguageName),
                //    Language, LanguageName,
                //    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }

        }

        #region Calculation functions

        /// <summary>
        /// This function calculates the loss balance foreign currency
        /// </summary>
        private void CalculateLossBalanceForeignCurrency()
        {
            try
            {
                if (Math.Abs(_model.ExchangeRatioDec) > 0
                    && Math.Abs(_model.LossBalanceDec) > 0)
                {
                    _model.LossBalanceFCDec = _model.ExchangeRatioDec * _model.LossBalanceDec;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculateLossBalanceForeignCurrency()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.LossBalanceFCDec = 0;
            }
        }

        /// <summary>
        /// This function calculates the dividend payout
        /// </summary>
        private void CalculateDividendPayOut()
        {
            try
            {
                if (Math.Abs(_model.RateDec) > 0
                    && Math.Abs(_model.VolumeDec) > 0)
                {
                    _model.PayoutDec = Math.Round(_model.RateDec * _model.VolumeDec, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    _model.Payout = @"";
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculateDividendPayOut()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.PayoutDec = 0;
            }
        }

        /// <summary>
        /// This function calculates the dividend payout in the foreign currency
        /// </summary>
        private void CalculateDividendPayOutFC()
        {
            try
            {
                if (Math.Abs(_model.RateFCDec) > 0
                    && Math.Abs(_model.VolumeDec) > 0)
                {
                    _model.PayoutFCDec = Math.Round(_model.RateFCDec * _model.VolumeDec, 2, MidpointRounding.AwayFromZero);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculateDividendPayOutForeignCurrency()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.PayoutFCDec = 0;
            }
        }

        /// <summary>
        /// This function calculates the dividend yield 
        /// </summary>
        private void CalculateDividendYield()
        {
            try
            {
                if (Math.Abs(_model.PriceDec) > 0
                    && Math.Abs(_model.RateDec) > 0)
                {
                    _model.YieldDec = _model.RateDec / _model.PriceDec * 100;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculateDividendYield()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.YieldDec = 0;
            }
        }

        /// <summary>
        /// This function calculates from the foreign currency price the normal payout
        /// </summary>
        private void CaluclateDividendRateFromDividendRateFC()
        {
            try
            {
                if (Math.Abs(_model.ExchangeRatioDec) > 0
                    && Math.Abs(_model.RateFCDec) > 0)
                {
                    _model.RateDec = _model.RateFCDec / _model.ExchangeRatioDec;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CaluclatePayoutFromForeignCurrencyPrice()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#endif
                _model.RateDec = 0;
            }
        }

        private void CalculateTaxes()
        {
//            if (Math.Abs(_model.Payoutdec) > 0)
//            {
                _model.TaxValuesCurrent.ValueWithoutTaxes = _model.PayoutDec;
                _model.TaxValuesCurrent.ValueWithoutTaxesFC = _model.PayoutFCDec;

                //_model.Taxdec = _model.TaxValuesCurrent.ValueWithoutTaxes;
                //_model.TaxFCdec = _model.TaxValuesCurrent.ValueWithoutTaxesFC;

                _model.TaxDec = _model.TaxValuesCurrent.TaxesValue;
                _model.TaxFCDec = _model.TaxValuesCurrent.TaxesValueFC;

                _model.PayoutAfterTaxDec = _model.TaxValuesCurrent.ValueWithTaxes;
                _model.PayoutAfterTaxFCDec = _model.TaxValuesCurrent.ValueWithTaxesFC;
//            }
        }

        #endregion Calculation functions

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

            _model.CultureInfoFC = _model.ShareObject.CultureInfo;

            string strDateTime = _model.Date + " " + _model.Time;

            try
            {
                // Check if a dividend with the given date and time already exists
                foreach (var dividend in _model.ShareObject.AllDividendEntries.GetAllDividendsOfTheShare())
                {
                    if (bFlagEdit)
                    {
                        // By an Add all dates and times must be checked
                        if (dividend.DividendDate == strDateTime)
                        {
                            _model.ErrorCode = DividendErrorCode.DateExists;
                            bErrorFlag = true;
                            break;
                        }
                    }
                    else
                    {
                        // By an Edit all dividends without the edit entry date and time must be checked
                        if (dividend.DividendDate == strDateTime
                            && _model.SelectedDate != null
                            && dividend.DividendDate != _model.SelectedDate)
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

                    // Check if a dividend rate is given (foreign currency)
                    decimal decRateFC = -1;
                    if (_model.RateFC == @"" && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.RateEmpty;
                        bErrorFlag = true;
                    }
                    else if (!decimal.TryParse(_model.RateFC, out decRateFC) && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.RateWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decRateFC <= 0 && bErrorFlag == false)
                    {
                        _model.ErrorCode = DividendErrorCode.RateWrongValue;
                        bErrorFlag = true;
                    }
                }
                else
                {
                    // Check if a dividend rate is given
                    decimal decRate = -1;
                    if (_model.Rate == @"")
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
                }

                // Loss balance 
                decimal decLossBalance = -1;
                if (_model.LossBalance != @"" &&
                    !decimal.TryParse(_model.LossBalance, out decLossBalance) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.LossBalanceWrongFormat;
                    bErrorFlag = true;
                }
                else if (_model.LossBalance != @"" && decLossBalance <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.LossBalanceWrongValue;
                    bErrorFlag = true;
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

                // Check if a given document exists
                if (_model.Document != @"" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.ErrorCode = DividendErrorCode.DocumentDoesNotExists;
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

                _model.ErrorCode = DividendErrorCode.InputeValuesInvalid;
                return false;
            }
        }
    }
}
