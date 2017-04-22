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

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Taxes;
using SharePortfolioManager.Forms.DividendForm.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.DividendForm.Model
{
    interface IModelDividendEdit
    {
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }
        bool UpdateDividend { get; set; }

        DividendErrorCode ErrorCode { get; set; }
        string SelectedDate { get; set; }

        List<ShareObject> ShareObjectList { get; set; }
        ShareObject ShareObject { get; set; }

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        string Date { get; set; }
        string Time { get; set; }
        CheckState EnableFC { get; set; }
        string ExchangeRatio { get; set; }
        decimal ExchangeRatioDec { get; set; }
        CultureInfo CultureInfoFC { get; set; }
        string Rate { get; set; }
        decimal RateDec { get; set; }
        string RateFC { get; set; }
        decimal RateFCDec { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string LossBalance { get; set; }
        decimal LossBalanceDec { get; set; }
        string LossBalanceFC { get; set; }
        decimal LossBalanceFCDec { get; set; }
        string Payout { get; set; }
        decimal PayoutDec { get; set; }
        string PayoutFC { get; set; }
        decimal PayoutFCDec { get; set; }
        Taxes TaxValuesCurrent { get; set; }
        string Tax { get; set; }
        decimal TaxDec { get; set; }
        string TaxFC { get; set; }
        decimal TaxFCDec { get; set; }
        string PayoutAfterTax { get; set; }
        decimal PayoutAfterTaxDec { get; set; }
        string PayoutAfterTaxFC { get; set; }
        decimal PayoutAfterTaxFCDec { get; set; }
        string Yield { get; set; }
        decimal YieldDec { get; set; }
        string Price { get; set; }
        decimal PriceDec { get; set; }
        string PriceFC { get; set; }
        decimal PriceFCDec { get; set; }
        string Document { get; set; }
    }

    /// <summary>
    /// Model class of the DividendEdit
    /// </summary>
    public class ModelDividendEdit : IModelDividendEdit
    {
        #region Fields

        bool _updateView;
        bool _updateViewFormatted;
        bool _updateDividend;


        DividendErrorCode _errorCode;
        string _selectedDate;

        List<ShareObject> _shareObjectList;
        ShareObject _shareObject;

        Logger _logger;
        Language _language;
        string _languageName;

        string _date;
        string _time;
        CheckState _enableFC;
        string _exchangeRatio;
        decimal _exchangeRatioDec;
        CultureInfo _cultureInfoFC;
        string _rate;
        decimal _rateDec;
        string _rateFC;
        decimal _rateFCDec;
        string _volume;
        decimal _volumeDec;
        string _lossBalance;
        decimal _lossBalanceDec;
        string _lossBalanceFC;
        decimal _lossBalanceFCDec;
        string _payout;
        decimal _payoutDec;
        string _payoutFC;
        decimal _payoutFCDec;
        Taxes _taxValuesCurrent;
        string _tax;
        decimal _taxDec;
        string _taxFC;
        decimal _taxFCDec;
        string _payoutAfterTax;
        decimal _payoutAfterTaxDec;
        string _payoutAfterTaxFC;
        decimal _payoutAfterTaxFCDec;
        string _yield;
        decimal _yieldDec;
        string _price;
        decimal _priceDec;
        string _priceFC;
        decimal _priceFCDec;
        string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView
        {
            get { return _updateView; }
            set { _updateView = value; }
        }

        public bool UpdateViewFormatted
        {
            get { return _updateViewFormatted; }
            set { _updateViewFormatted = value; }
        }

        public bool UpdateDividend
        {
            get { return _updateDividend; }
            set { _updateDividend = value; }
        }

        public DividendErrorCode ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public string SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; }
        }

        public List<ShareObject> ShareObjectList
        {
            get { return _shareObjectList; }
            set { _shareObjectList = value; }
        }

        public ShareObject ShareObject
        {
            get { return _shareObject; }
            set { _shareObject = value; }
        }

        public Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public Language Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string LanguageName
        {
            get { return _languageName; }
            set { _languageName = value; }
        }

        public string Date
        {
            get { return _date; }
            set
            {
                if (_date == value)
                    return;

                _date = value;
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                if (_time == value)
                    return;

                _time = value;
            }
        }

        public CheckState EnableFC
        {
            get { return _enableFC; }
            set { _enableFC = value; }
        }

        public string ExchangeRatio
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the exchange ratio is greater than '0'
                    if (_exchangeRatioDec > 0)
                        return Helper.FormatDecimal(_exchangeRatioDec, Helper.Currencytwolength, false, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _exchangeRatio;
                }
            }
            set
            {
                if (_exchangeRatio == value)
                    return;
                _exchangeRatio = value;

                // Try to parse
                if (!Decimal.TryParse(_exchangeRatio, out _exchangeRatioDec))
                    _exchangeRatioDec = 0;
                else
                    TaxValuesCurrent.ExchangeRatio = _exchangeRatioDec;
            }
        }

        public decimal ExchangeRatioDec
        {
            get { return _exchangeRatioDec; }
            set
            {
                if (_exchangeRatioDec == value)
                    return;
                _exchangeRatioDec = value;
                TaxValuesCurrent.ExchangeRatio = _exchangeRatioDec;
            }
        }

        public CultureInfo CultureInfoFC
        {
            get { return _cultureInfoFC; }
            set
            {
                if (_cultureInfoFC == value)
                    return;
                _cultureInfoFC = value;
            }
        }

        public string Rate
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the rate is greater than '0'
                    if (_rateDec > 0)
                        return Helper.FormatDecimal(_rateDec, Helper.Currencytwolength, false, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _rate;
                }
            }
            set
            {
                if (_rate == value)
                    return;
                _rate = value;

                // Try to parse
                if (!Decimal.TryParse(_rate, out _rateDec))
                    _rateDec = 0;
            }
        }

        public decimal RateDec
        {
            get { return _rateDec; }
            set
            {
                if (_rateDec == value)
                    return;
                _rateDec = value;
            }
        }

        public string RateFC
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the rate foreign currency is greater than '0'
                    if (_rateFCDec > 0)
                        return Helper.FormatDecimal(_rateFCDec, Helper.Currencytwolength, false, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _rateFC;
                }
            }
            set
            {
                if (_rateFC == value)
                    return;
                _rateFC = value;

                // Try to parse
                if (!Decimal.TryParse(_rateFC, out _rateFCDec))
                    _rateFCDec = 0;
                else
                    RateFCDec = _rateFCDec;
            }
        }

        public decimal RateFCDec
        {
            get { return _rateFCDec; }
            set
            {
                if (_rateFCDec == value)
                    return;
                _rateFCDec = value;

                UpdateView = true;
            }
        }

        public string Volume
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the volume is greater than '0'
                    if (_volumeDec > 0)
                        return Helper.FormatDecimal(_volumeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _volume;
                }
            }
            set
            {
                if (_volume == value)
                    return;
                _volume = value;

                // Try to parse
                if (!Decimal.TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;
            }
        }

        public decimal VolumeDec
        {
            get { return _volumeDec; }
            set
            {
                if (_volumeDec == value)
                    return;
                _volumeDec = value;
            }
        }

        public string LossBalance
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the loss balance is greater than '0'
                    if (_lossBalanceDec > 0)
                        return Helper.FormatDecimal(_lossBalanceDec, Helper.Currencytwolength, false, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _lossBalance;
                }
            }
            set
            {
                if (_lossBalance == value)
                    return;
                _lossBalance = value;

                // Try to parse
                if (!Decimal.TryParse(_lossBalance, out _lossBalanceDec))
                    _lossBalanceDec = 0;
                else
                    TaxValuesCurrent.LossBalance = _lossBalanceDec;
            }
        }

        public decimal LossBalanceDec
        {
            get { return _lossBalanceDec; }
            set
            {
                if (_lossBalanceDec == value)
                    return;
                _lossBalanceDec = value;

                LossBalance = _lossBalanceDec.ToString();

                UpdateView = true;
            }
        }

        public string LossBalanceFC
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the loss balance of the foreign currency is greater than '0'
                    if (_lossBalanceFCDec > 0)
                        return Helper.FormatDecimal(_lossBalanceFCDec, Helper.Currencytwolength, false, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _lossBalanceFC;
                }
            }
            set
            {
                if (_lossBalanceFC == value)
                    return;
                _lossBalanceFC = value;

                // Try to parse
                if (!Decimal.TryParse(_lossBalanceFC, out _lossBalanceFCDec))
                    _lossBalanceDec = 0;
            }
        }

        public decimal LossBalanceFCDec
        {
            get { return _lossBalanceFCDec; }
            set
            {
                if (_lossBalanceFCDec == value)
                    return;
                _lossBalanceFCDec = value;

                LossBalanceFC = _lossBalanceFCDec.ToString();
            }
        }

        public string Payout
        {
            get
            {
                // Only return the value if the payout is greater than '0'
                if (_payoutDec > 0)
                    return Helper.FormatDecimal(_payoutDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
            }
            set
            {
                if (_payout == value)
                    return;
                _payout = value;

                // Try to parse
                if (!decimal.TryParse(_payout, out _payoutDec))
                    _payoutDec = 0;
            }
        }

        public decimal PayoutDec
        {
            get { return _payoutDec; }
            set
            {
                if (_payoutDec == value)
                    return;
                _payoutDec = value;

                Payout = _payoutDec.ToString();

                UpdateView = true;
            }
        }

        public string PayoutFC
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the payout of the foreign currency is greater than '0'
                    if (_payoutFCDec > 0)
                        return Helper.FormatDecimal(_payoutFCDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _payoutFC;
                }
            }
            set
            {
                if (_payoutFC == value)
                    return;
                _payoutFC = value;

                // Try to parse
                if (!decimal.TryParse(_payoutFC, out _payoutFCDec))
                    _payoutFCDec = 0;
            }
        }

        public decimal PayoutFCDec
        {
            get { return _payoutFCDec; }
            set
            {
                if (_payoutFCDec == value)
                    return;
                _payoutFCDec = value;

                PayoutAfterTaxFC = _payoutFCDec.ToString();
            }
        }

        public Taxes TaxValuesCurrent
        {
            get { return _taxValuesCurrent; }
            set { _taxValuesCurrent = value; }
        }

        public string Tax
        {
            get
            {
                // Only return the value if the tax is greater than '0'
                if (_taxDec > 0)
                    return Helper.FormatDecimal(_taxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
            }
            set
            {
                if (_tax == value)
                    return;
                _tax = value;

                // Try to parse
                if (!decimal.TryParse(_tax, out _taxDec))
                    _taxDec = 0;
            }
        }

        public decimal TaxDec
        {
            get { return _taxDec; }
            set
            {
                if (_taxDec == value)
                    return;

                Tax = TaxDec.ToString();

                _taxDec = value;
            }
        }

        public string TaxFC
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the tax of the foreign currency is greater than '0'
                    if (_taxFCDec > 0)
                        return Helper.FormatDecimal(_taxFCDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _taxFC;
                }
            }
            set
            {
                if (_taxFC == value)
                    return;
                _taxFC = value;

                // Try to parse
                if (!decimal.TryParse(_taxFC, out _taxFCDec))
                    _taxFCDec = 0;
            }
        }

        public decimal TaxFCDec
        {
            get { return _taxFCDec; }
            set
            {
                if (_taxFCDec == value)
                    return;
                _taxFCDec = value;
            }
        }

        public string PayoutAfterTax
        {
            get
            {
                    // Only return the value if the payout after tax is greater than '0'
                    if (_payoutAfterTaxDec > 0)
                        return Helper.FormatDecimal(_payoutAfterTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
            }
            set
            {
                if (_payoutAfterTax == value)
                    return;
                _payoutAfterTax = value;

                // Try to parse
                if (!decimal.TryParse(_payoutAfterTax, out _payoutAfterTaxDec))
                    _payoutAfterTaxDec = 0;
            }
        }

        public decimal PayoutAfterTaxDec
        {
            get { return _payoutAfterTaxDec; }
            set
            {
                if (_payoutAfterTaxDec == value)
                    return;
                _payoutAfterTaxDec = value;

                PayoutAfterTax = _payoutAfterTaxDec.ToString();

                UpdateView = true;
            }
        }

        public string PayoutAfterTaxFC
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the payout after tax of the foreign currency is greater than '0'
                    if (_payoutAfterTaxFCDec > 0)
                        return Helper.FormatDecimal(_payoutAfterTaxFCDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _payoutAfterTaxFC;
                }
            }
            set
            {
                if (_payoutAfterTaxFC == value)
                    return;
                _payoutAfterTaxFC = value;

                // Try to parse
                if (!decimal.TryParse(_payoutAfterTaxFC, out _payoutAfterTaxFCDec))
                    _payoutAfterTaxFCDec = 0;
            }
        }

        public decimal PayoutAfterTaxFCDec
        {
            get { return _payoutAfterTaxFCDec; }
            set
            {
                if (_payoutAfterTaxFCDec == value)
                    return;
                _payoutAfterTaxFCDec = value;
            }
        }

        public string Yield
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the yield is greater than '0'
                    if (_yieldDec > 0)
                        return Helper.FormatDecimal(_yieldDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _yield;
                }
            }
            set
            {
                if (_yield == value)
                    return;
                _yield = value;

                // Try to parse
                if (!decimal.TryParse(_yield, out _yieldDec))
                    _yieldDec = 0;
            }
        }

        public decimal YieldDec
        {
            get { return _yieldDec; }
            set
            {
                if (_yieldDec == value)
                    return;
                _yieldDec = value;

                Yield = _yieldDec.ToString();

                UpdateView = true;
            }
        }

        public string Price
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the price is greater than '0'
                    if (_priceDec > 0)
                        return Helper.FormatDecimal(_priceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _price;
                }
            }
            set
            {
                if (_price == value)
                    return;
                _price = value;

                // Try to parse
                if (!decimal.TryParse(_price, out _priceDec))
                    _priceDec = 0;
            }
        }

        public decimal PriceDec
        {
            get { return _priceDec; }
            set
            {
                if (_priceDec == value)
                    return;
                _priceDec = value;
            }
        }

        public string PriceFC
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the price of the foreign currency is greater than '0'
                    if (_priceFCDec > 0)
                        return Helper.FormatDecimal(_priceFCDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _priceFC;
                }
            }
            set
            {
                if (_priceFC == value)
                    return;
                _priceFC = value;

                // Try to parse
                if (!decimal.TryParse(_priceFC, out _priceFCDec))
                    _priceFCDec = 0;
            }
        }

        public decimal PriceFCDec
        {
            get { return _priceFCDec; }
            set
            {
                if (_priceFCDec == value)
                    return;
                _priceFCDec = value;
            }
        }

        public string Document
        {
            get { return _document; }
            set
            {
                if (_document == value)
                    return;
                _document = value;
            }
        }

        #endregion IModel members
    }
}
