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
//#define DEBUG_DIVIDEND_EDIT_MODEL

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Dividend;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.DividendForm.View;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace SharePortfolioManager.DividendForm.Model
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
        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }
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

        public string SelectedGuid { get; set; }

        public string SelectedDate { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }

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
                if (Equals(_date, value))
                    return;
                _date = value;
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                if (Equals(_time, value))
                    return;
                _time = value;
            }
        }

        public CheckState EnableFc { get; set; }

        public string ExchangeRatio
        {
            get
            {
                if (!decimal.TryParse(_exchangeRatio, out _exchangeRatioDec))
                    _exchangeRatioDec = 0;

                if (UpdateViewFormatted)
                    return _exchangeRatioDec > 0 ? Helper.FormatDecimal(_exchangeRatioDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _exchangeRatio;

                return _exchangeRatio;
            }
            set
            {
                if (Equals(_exchangeRatio, value))
                    return;
                _exchangeRatio = value;

                if (!decimal.TryParse(_exchangeRatio, out _exchangeRatioDec))
                    _exchangeRatioDec = 0;
            }
        }

        public decimal ExchangeRatioDec
        {
            get => _exchangeRatioDec;
            set
            {
                if (Equals(_exchangeRatioDec, value))
                    return;
                _exchangeRatioDec = value;

                UpdateView = true;
            }
        }

        public CultureInfo CultureInfoFc
        {
            get => _cultureInfoFc;
            set
            {
                if (Equals(_cultureInfoFc, value))
                    return;
                _cultureInfoFc = value;
            }
        }

        public string Rate
        {
            get
            {
                if (!decimal.TryParse(_rate, out _rateDec))
                    _rateDec = 0;

                if (UpdateViewFormatted)
                    return _rateDec > 0 ? Helper.FormatDecimal(_rateDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _rate;

                return _rate;
            }
            set
            {
                if (Equals(_rate, value))
                    return;
                _rate = value;

                if (!decimal.TryParse(_rate, out _rateDec))
                    _rateDec = 0;
            }
        }

        public decimal RateDec
        {
            get => _rateDec;
            set
            {
                if (Equals(_rateDec, value))
                    return;
                _rateDec = value;

                UpdateView = true;
            }
        }

        public string Volume
        {
            get
            {
                if (!decimal.TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;

                if (UpdateViewFormatted)
                    return _volumeDec >= 0 ? Helper.FormatDecimal(_volumeDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _volume;

                return _volume;
            }
            set
            {
                if (Equals(_volume, value))
                    return;
                _volume = value;

                if (!decimal.TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;
            }
        }

        public decimal VolumeDec
        {
            get => _volumeDec;
            set
            {
                if (Equals(_volumeDec, value))
                    return;
                _volumeDec = value;
            }
        }

        public string Payout
        {
            get
            {
                if (!decimal.TryParse(_payout, out _payoutDec))
                    _payoutDec = 0;

                if (UpdateViewFormatted)
                    return _payoutDec >= 0 ? Helper.FormatDecimal(_payoutDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _payout;

                return _payout;
            }
            set
            {
                if (Equals(_payout, value))
                    return;
                _payout = value;

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
                if (Equals(_payoutDec, value))
                    return;
                _payoutDec = value;

                UpdateView = true;
            }
        }

        public string PayoutFc
        {
            get
            {
                if (!decimal.TryParse(_payoutFc, out _payoutFcDec))
                    _payoutFcDec = 0;

                if (UpdateViewFormatted)
                {
                    if(EnableFc == CheckState.Checked)
                        return _payoutFcDec >= 0 ? Helper.FormatDecimal(_payoutFcDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _payoutFc;
                    else
                        return _payoutFcDec > 0 ? Helper.FormatDecimal(_payoutFcDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _payoutFc;
                }

                return _payoutFc;
            }
            set
            {
                if (Equals(_payoutFc, value))
                    return;
                _payoutFc = value;

                if (!decimal.TryParse(_payoutFc, out _payoutFcDec))
                    _payoutFcDec = 0;
            }
        }

        public decimal PayoutFcDec
        {
            get => _payoutFcDec;
            set
            {
                if (Equals(_payoutFcDec, value))
                    return;
                _payoutFcDec = value;

                UpdateView = true;
            }
        }

        public string TaxAtSource
        {
            get
            {
                if (!decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;

                if (UpdateViewFormatted)
                    return _taxAtSourceDec > 0 ? Helper.FormatDecimal(_taxAtSourceDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _taxAtSource;

                return _taxAtSource;
            }
            set
            {
                if (Equals(_taxAtSource, value))
                    return;
                _taxAtSource = value;

                if (!decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;
            }
        }

        public decimal TaxAtSourceDec
        {
            get => _taxAtSourceDec;
            set
            {
                if (Equals(_taxAtSourceDec, value))
                    return;
                _taxAtSourceDec = value;
            }
        }

        public string CapitalGainsTax
        {
            get
            {
                if (!decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;

                if (UpdateViewFormatted)
                    return _capitalGainsTaxDec > 0 ? Helper.FormatDecimal(_capitalGainsTaxDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _capitalGainsTax;

                return _capitalGainsTax;
            }
            set
            {
                if (Equals(_capitalGainsTax, value))
                    return;
                _capitalGainsTax = value;

                if (!decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;
            }
        }

        public decimal CapitalGainsTaxDec
        {
            get => _capitalGainsTaxDec;
            set
            {
                if (Equals(_capitalGainsTaxDec, value))
                    return;
                _capitalGainsTaxDec = value;
            }
        }

        public string SolidarityTax
        {
            get
            {
                if (!decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;

                if (UpdateViewFormatted)
                    return _solidarityTaxDec > 0 ? Helper.FormatDecimal(_solidarityTaxDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _solidarityTax;

                return _solidarityTax;
            }
            set
            {
                if (Equals(_solidarityTax, value))
                    return;
                _solidarityTax = value;

                if (!decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;
            }
        }

        public decimal SolidarityTaxDec
        {
            get => _solidarityTaxDec;
            set
            {
                if (Equals(_solidarityTaxDec, value))
                    return;
                _solidarityTaxDec = value;
            }
        }

        public string Tax
        {
            get
            {
                if (!decimal.TryParse(_tax, out _taxDec))
                    _taxDec = 0;

                if (UpdateViewFormatted)
                    return _taxDec >= 0 ? Helper.FormatDecimal(_taxDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _tax;

                return _tax;
            }
            set
            {
                if (Equals(_tax, value))
                    return;
                _tax = value;

                if (!decimal.TryParse(_tax, out _taxDec))
                    _taxDec = 0;
            }
        }

        public decimal TaxDec
        {
            get => _taxDec;
            set
            {
                if (Equals(_taxDec, value))
                    return;
                _taxDec = value;
            }
        }

        public string PayoutAfterTax
        {
            get
            {
                if (!decimal.TryParse(_payoutAfterTax, out _payoutAfterTaxDec))
                    _payoutAfterTaxDec = 0;

                if (UpdateViewFormatted)
                    return _payoutAfterTaxDec >= 0 ? Helper.FormatDecimal(_payoutAfterTaxDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _payoutAfterTax;

                return _payoutAfterTax;
            }
            set
            {
                if (Equals(_payoutAfterTax, value))
                    return;
                _payoutAfterTax = value;

                if (!decimal.TryParse(_payoutAfterTax, out _payoutAfterTaxDec))
                    _payoutAfterTaxDec = 0;
            }
        }

        public decimal PayoutAfterTaxDec
        {
            get => _payoutAfterTaxDec;
            set
            {
                if (Equals(_payoutAfterTaxDec, value))
                    return;
                _payoutAfterTaxDec = value;

                UpdateView = true;
            }
        }

        public string Yield
        {
            get
            {
                if (!decimal.TryParse(_yield, out _yieldDec))
                    _yieldDec = 0;

                if (UpdateViewFormatted)
                    return _yieldDec >= 0 ? Helper.FormatDecimal(_yieldDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _yield;

                return _yield;
            }
            set
            {
                if (Equals(_yield, value))
                    return;
                _yield = value;

                if (!decimal.TryParse(_yield, out _yieldDec))
                    _yieldDec = 0;
            }
        }

        public decimal YieldDec
        {
            get => _yieldDec;
            set
            {
                if (Equals(_yieldDec, value))
                    return;
                _yieldDec = value;

                UpdateView = true;
            }
        }

        public string Price
        {
            get
            {
                if (!decimal.TryParse(_price, out _priceDec))
                    _priceDec = 0;

                if (UpdateViewFormatted)
                    return _priceDec > 0 ? Helper.FormatDecimal(_priceDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _price;

                return _price;
            }
            set
            {
                if (Equals(_price, value))
                    return;
                _price = value;

                if (!decimal.TryParse(_price, out _priceDec))
                    _priceDec = 0;
            }
        }

        public decimal PriceDec
        {
            get => _priceDec;
            set
            {
                if (Equals(_priceDec, value))
                    return;
                _priceDec = value;
            }
        }

        public string Document
        {
            get => _document;
            set
            {
                if (Equals(_document, value))
                    return;
                _document = value;
            }
        }

        #endregion IModel members
    }
}
