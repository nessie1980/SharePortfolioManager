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
using SharePortfolioManager.Forms.DividendForm.View;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.DividendForm.Model
{
    /// <summary>
    /// Interface of the DividendEdit model
    /// </summary>
    internal interface IModelDividendEdit
    {
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }
        bool UpdateDividend { get; set; }

        DividendErrorCode ErrorCode { get; set; }
        string SelectedDate { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<ShareObjectFinalValue> ShareObjectListFinalValue { get; set; }

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        DividendObject DividendObject { get; set; }

        string Date { get; set; }
        string Time { get; set; }
        CheckState EnableFc { get; set; }
        string ExchangeRatio { get; set; }
        decimal ExchangeRatioDec { get; set; }
        CultureInfo CultureInfoFc { get; set; }
        string Rate { get; set; }
        decimal RateDec { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string Payout { get; set; }
        decimal PayoutDec { get; set; }
        string PayoutFc { get; set; }
        decimal PayoutFcDec { get; set; }
        string TaxAtSource { get; set; }
        decimal TaxAtSourceDec { get; set; }
        string CapitalGainsTax { get; set; }
        decimal CapitalGainsTaxDec { get; set; }
        string SolidarityTax { get; set; }
        decimal SolidarityTaxDec { get; set; }
        string Tax { get; set; }
        decimal TaxDec { get; set; }
        string PayoutAfterTax { get; set; }
        decimal PayoutAfterTaxDec { get; set; }
        string Yield { get; set; }
        decimal YieldDec { get; set; }
        string Price { get; set; }
        decimal PriceDec { get; set; }
        string Document { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Model class of the DividendEdit
    /// </summary>
    public class ModelDividendEdit : IModelDividendEdit
    {
        #region Fields

        private string _date;
        private string _time;
        private string _exchangeRatio;
        private decimal _exchangeRatioDec;
        private CultureInfo _cultureInfoFc;
        private string _rate;
        private decimal _rateDec;
        private string _volume;
        private decimal _volumeDec;
        private string _payout;
        private decimal _payoutDec;
        private string _payoutFc;
        private decimal _payoutFcDec;
        private string _taxAtSource;
        private decimal _taxAtSourceDec;
        private string _capitalGainsTax;
        private decimal _capitalGainsTaxDec;
        private string _solidarityTax;
        private decimal _solidarityTaxDec;
        private string _tax;
        private decimal _taxDec;
        private string _payoutAfterTax;
        private decimal _payoutAfterTaxDec;
        private string _yield;
        private decimal _yieldDec;
        private string _price;
        private decimal _priceDec;
        private string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView { get; set; }

        public bool UpdateViewFormatted { get; set; }

        public bool UpdateDividend { get; set; }

        public DividendErrorCode ErrorCode { get; set; }

        public string SelectedDate { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

        public DividendObject DividendObject { get; set; }

        public string Date
        {
            get => _date;
            set
            {
                if (_date != null && _date == value)
                    return;

                _date = value;
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                if (_time != null && _time == value)
                    return;

                _time = value;
            }
        }

        public CheckState EnableFc { get; set; }

        public string ExchangeRatio
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the exchange ratio is greater than '0'
                    return _exchangeRatioDec > 0 ? Helper.FormatDecimal(_exchangeRatioDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";
                }

                return _exchangeRatio;
            }
            set
            {
                if (_exchangeRatio == value)
                    return;
                _exchangeRatio = value;

                // Try to parse
                if (!decimal.TryParse(_exchangeRatio, out _exchangeRatioDec))
                    _exchangeRatioDec = 0;
            }
        }

        public decimal ExchangeRatioDec
        {
            get => _exchangeRatioDec;
            set
            {
                if (_exchangeRatioDec == value)
                    return;
                _exchangeRatioDec = value;

                ExchangeRatio = _exchangeRatioDec.ToString("G");

                UpdateView = true;
            }
        }

        public CultureInfo CultureInfoFc
        {
            get => _cultureInfoFc;
            set
            {
                if (_cultureInfoFc != null && Equals(_cultureInfoFc, value))
                    return;
                _cultureInfoFc = value;
            }
        }

        public string Rate
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the rate is greater than '0'
                    return _rateDec > 0 ? Helper.FormatDecimal(_rateDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";
                }

                return _rate;
            }
            set
            {
                if (_rate == value)
                    return;
                _rate = value;

                // Try to parse
                if (!decimal.TryParse(_rate, out _rateDec))
                    _rateDec = 0;
            }
        }

        public decimal RateDec
        {
            get => _rateDec;
            set
            {
                if (_rateDec == value)
                    return;
                _rateDec = value;

                Rate = _rateDec.ToString("G");

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
                    return _volumeDec > 0 ? Helper.FormatDecimal(_volumeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";
                }

                return _volume;
            }
            set
            {
                if (_volume == value)
                    return;
                _volume = value;

                // Try to parse
                if (!decimal.TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;
            }
        }

        public decimal VolumeDec
        {
            get => _volumeDec;
            set
            {
                if (_volumeDec == value)
                {
                    return;
                }

                _volumeDec = value;
            }
        }

        public string Payout
        {
            get => _payoutDec > 0 ? Helper.FormatDecimal(_payoutDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
            set
            {
                if (_payout == value)
                    return;
                _payout = value;

                // Try to parse
                if (!decimal.TryParse(_payout, out _payoutDec))
                    _payoutDec = 0;

                UpdateView = true;
            }
        }

        public decimal PayoutDec
        {
            get => _payoutDec;
            set
            {
                if (_payoutDec == value)
                    return;
                _payoutDec = value;

                Payout = _payoutDec.ToString("G");

                UpdateView = true;
            }
        }

        public string PayoutFc
        {
            get => _payoutFcDec > 0 ? Helper.FormatDecimal(_payoutFcDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
            set
            {
                if (_payoutFc == value)
                    return;
                _payoutFc = value;

                // Try to parse
                if (!decimal.TryParse(_payoutFc, out _payoutFcDec))
                    _payoutFcDec = 0;
            }
        }

        public decimal PayoutFcDec
        {
            get => _payoutFcDec;
            set
            {
                if (_payoutFcDec == value)
                    return;
                _payoutFcDec = value;

                PayoutFc = _payoutFcDec.ToString("G");

                UpdateView = true;
            }
        }

        public string TaxAtSource
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the tax is greater than '0'
                    return _taxAtSourceDec > 0 ? Helper.FormatDecimal(_taxAtSourceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _taxAtSource;
            }
            set
            {
                if (_taxAtSource == value)
                    return;
                _taxAtSource = value;

                // Try to parse
                if (!decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;
            }
        }

        public decimal TaxAtSourceDec
        {
            get => _taxAtSourceDec;
            set
            {
                if (_taxAtSourceDec == value)
                    return;

                TaxAtSource = TaxAtSourceDec.ToString("G");

                _taxAtSourceDec = value;
            }
        }

        public string CapitalGainsTax
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the tax is greater than '0'
                    return _capitalGainsTaxDec > 0 ? Helper.FormatDecimal(_capitalGainsTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _capitalGainsTax;
            }
            set
            {
                if (_capitalGainsTax == value)
                    return;
                _capitalGainsTax = value;

                // Try to parse
                if (!decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;
            }
        }

        public decimal CapitalGainsTaxDec
        {
            get => _capitalGainsTaxDec;
            set
            {
                if (_capitalGainsTaxDec == value)
                    return;

                CapitalGainsTax = CapitalGainsTaxDec.ToString("G");

                _capitalGainsTaxDec = value;
            }
        }

        public string SolidarityTax
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the tax is greater than '0'
                    return _solidarityTaxDec > 0 ? Helper.FormatDecimal(_solidarityTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _solidarityTax;
            }
            set
            {
                if (_solidarityTax == value)
                    return;
                _solidarityTax = value;

                // Try to parse
                if (!decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;
            }
        }

        public decimal SolidarityTaxDec
        {
            get => _solidarityTaxDec;
            set
            {
                if (_solidarityTaxDec == value)
                    return;

                SolidarityTax = SolidarityTaxDec.ToString("G");

                _solidarityTaxDec = value;
            }
        }

        public string Tax
        {
            get => _taxDec > 0 ? Helper.FormatDecimal(_taxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
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
            get => _taxDec;
            set
            {
                if (_taxDec == value)
                    return;

                Tax = TaxDec.ToString("G");

                _taxDec = value;
            }
        }

        public string PayoutAfterTax
        {
            get => _payoutAfterTaxDec > 0 ? Helper.FormatDecimal(_payoutAfterTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
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
            get => _payoutAfterTaxDec;
            set
            {
                if (_payoutAfterTaxDec == value)
                    return;
                _payoutAfterTaxDec = value;

                PayoutAfterTax = _payoutAfterTaxDec.ToString("G");

                UpdateView = true;
            }
        }

        public string Yield
        {
            get => _yieldDec > 0 ? Helper.FormatDecimal(_yieldDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";
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
            get => _yieldDec;
            set
            {
                if (_yieldDec == value)
                    return;
                _yieldDec = value;

                Yield = _yieldDec.ToString("G");

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
                    return _priceDec > 0 ? Helper.FormatDecimal(_priceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _price;
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
            get => _priceDec;
            set
            {
                if (_priceDec == value)
                    return;
                _priceDec = value;
            }
        }

        public string Document
        {
            get => _document;
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
