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
        CheckState EnableFC { get; set; }
        string ExchangeRatio { get; set; }
        decimal ExchangeRatioDec { get; set; }
        CultureInfo CultureInfoFC { get; set; }
        string Rate { get; set; }
        decimal RateDec { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string Payout { get; set; }
        decimal PayoutDec { get; set; }
        string PayoutFC { get; set; }
        decimal PayoutFCDec { get; set; }
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

        ShareObjectMarketValue _shareObjectMarketValue;
        List<ShareObjectMarketValue> _shareObjectListMarketValue;
        ShareObjectFinalValue _shareObjectFinalValue;
        List<ShareObjectFinalValue> _shareObjectListFinalValue;

        Logger _logger;
        Language _language;
        string _languageName;

        DividendObject _dividendObject;
        string _date;
        string _time;
        CheckState _enableFC;
        string _exchangeRatio;
        decimal _exchangeRatioDec;
        CultureInfo _cultureInfoFC;
        string _rate;
        decimal _rateDec;
        string _volume;
        decimal _volumeDec;
        string _payout;
        decimal _payoutDec;
        string _payoutFC;
        decimal _payoutFCDec;
        string _taxAtSource;
        decimal _taxAtSourceDec;
        string _capitalGainsTax;
        decimal _capitalGainsTaxDec;
        string _solidarityTax;
        decimal _solidarityTaxDec;
        string _tax;
        decimal _taxDec;
        string _payoutAfterTax;
        decimal _payoutAfterTaxDec;
        string _yield;
        decimal _yieldDec;
        string _price;
        decimal _priceDec;
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

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get { return _shareObjectMarketValue; }
            set { _shareObjectMarketValue = value; }
        }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue
        {
            get { return _shareObjectListMarketValue; }
            set { _shareObjectListMarketValue = value; }
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get { return _shareObjectFinalValue; }
            set { _shareObjectFinalValue = value; }
        }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue
        {
            get { return _shareObjectListFinalValue; }
            set { _shareObjectListFinalValue = value; }
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

        public DividendObject DividendObject
        {
            get { return _dividendObject; }
            set { _dividendObject = value; }
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
                    return Helper.FormatDecimal(_exchangeRatioDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
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

                ExchangeRatio = _exchangeRatioDec.ToString();

                UpdateView = true;
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
                        return Helper.FormatDecimal(_rateDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
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

                Rate = _rateDec.ToString();

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

                UpdateView = true;
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
                // Only return the value if the payout of the foreign currency is greater than '0'
                if (_payoutFCDec > 0)
                    return Helper.FormatDecimal(_payoutFCDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
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

                PayoutFC = _payoutFCDec.ToString();

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
                    if (_taxAtSourceDec > 0)
                        return Helper.FormatDecimal(_taxAtSourceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
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
            get { return _taxAtSourceDec; }
            set
            {
                if (_taxAtSourceDec == value)
                    return;

                TaxAtSource = TaxAtSourceDec.ToString();

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
                    if (_capitalGainsTaxDec > 0)
                        return Helper.FormatDecimal(_capitalGainsTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
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
            get { return _capitalGainsTaxDec; }
            set
            {
                if (_capitalGainsTaxDec == value)
                    return;

                CapitalGainsTax = CapitalGainsTaxDec.ToString();

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
                    if (_solidarityTaxDec > 0)
                        return Helper.FormatDecimal(_solidarityTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
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
            get { return _solidarityTaxDec; }
            set
            {
                if (_solidarityTaxDec == value)
                    return;

                SolidarityTax = SolidarityTaxDec.ToString();

                _solidarityTaxDec = value;
            }
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

        public string Yield
        {
            get
            {
                // Only return the value if the yield is greater than '0'
                if (_yieldDec > 0)
                    return Helper.FormatDecimal(_yieldDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
                return @"";
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
