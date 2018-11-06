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
using SharePortfolioManager.Forms.SalesForm.View;
using System.Collections.Generic;
using SharePortfolioManager.Classes.ShareObjects;
using Logging;
using LanguageHandler;

namespace SharePortfolioManager.Forms.SalesForm.Model
{
    /// <summary>
    /// Interface of the SaleEdit model
    /// </summary>
    public interface IModelSaleEdit
    {
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }

        bool ShowSales { get; set; }
        bool AddSale { get; set; }
        bool UpdateSale { get; set; }

        SaleErrorCode ErrorCode { get; set; }
        string SelectedGuid { get; set; }
        string SelectedGuidLast { get; set; }
        string SelectedDate { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        ShareObjectFinalValue ShareObjectCalculation { get; set; }

        List<WebSiteRegex> WebSiteRegexList { get; set; }

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string SalePrice { get; set; }
        decimal SalePriceDec { get; set; }
        decimal SaleBuyValueDec { get; set; }
        string SaleBuyValue { get; set; }
        List<SaleBuyDetails> UsedBuyDetails { get; set; }
        string TaxAtSource { get; set; }
        decimal TaxAtSourceDec { get; set; }
        string CapitalGainsTax { get; set; }
        decimal CapitalGainsTaxDec { get; set; }
        string SolidarityTax { get; set; }
        decimal SolidarityTaxDec { get; set; }
        string Brokerage { get; set; }
        decimal BrokerageDec { get; set; }
        string Reduction { get; set; }
        decimal ReductionDec { get; set; }
        string ProfitLoss { get; set; }
        decimal ProfitLossDec { get; set; }
        string Payout { get; set; }
        decimal PayoutDec { get; set; }
        string Document { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Model class of the SaleEdit
    /// </summary>
    public class ModelSaleEdit : IModelSaleEdit
    {
        #region Fields

        private string _date;
        private string _time;
        private string _volume;
        private decimal _volumeDec;
        private string _salePrice;
        private decimal _salePriceDec;
        private string _saleBuyValue;
        private decimal _saleBuyValueDec;
        private string _taxAtSource;
        private decimal _taxAtSourceDec;
        private string _capitalGainsTax;
        private decimal _capitalGainsTaxDec;
        private string _solidarityTax;
        private decimal _solidarityTaxDec;
        private string _brokerage;
        private decimal _brokerageDec;
        private string _reduction;
        private decimal _reductionDec;
        private string _profitLoss;
        private decimal _profitLossDec;
        private string _payout;
        private decimal _payoutDec;
        private string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView { get; set; }

        public bool UpdateViewFormatted { get; set; }

        public bool ShowSales { get; set; }

        public bool AddSale { get; set; }

        public bool UpdateSale { get; set; }

        public SaleErrorCode ErrorCode { get; set; }

        public string SelectedGuid { set; get; }

        public string SelectedGuidLast { set; get; }

        public string SelectedDate { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public ShareObjectFinalValue ShareObjectCalculation{ get; set; }

        public List<WebSiteRegex> WebSiteRegexList { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

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

        public string Volume
        {
            get
            {
                if (!decimal.TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;

                if (UpdateViewFormatted)
                    return _volumeDec > 0 ? Helper.FormatDecimal(_volumeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _volume;

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

        public string SalePrice
        {
            get
            {
                if (!decimal.TryParse(_salePrice, out _salePriceDec))
                    _salePriceDec = 0;

                if (UpdateViewFormatted)
                {
                    return _salePriceDec > 0 ? Helper.FormatDecimal(_salePriceDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _salePrice;
                }

                return _salePrice;
            }
            set
            {
                if (Equals(_salePrice, value))
                    return;
                _salePrice = value;

                if (!decimal.TryParse(_salePrice, out _salePriceDec))
                    _salePriceDec = 0;
            }
        }

        public decimal SalePriceDec
        {
            get => _salePriceDec;
            set
            {
                if (_salePriceDec == value)
                    return;
                _salePriceDec = value;

                SalePrice = _salePriceDec.ToString("G");
            }
        }

        public string SaleBuyValue
        {
            get => Helper.FormatDecimal(_saleBuyValueDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
            set
            {
                if (Equals(_saleBuyValue, value))
                    return;
                _saleBuyValue = value;

                if (!decimal.TryParse(_saleBuyValue, out _saleBuyValueDec))
                    _saleBuyValueDec = 0;
            }
        }

        public decimal SaleBuyValueDec
        {
            get => _saleBuyValueDec;
            set
            {
                if (Equals(_saleBuyValueDec, value))
                    return;
                _saleBuyValueDec = value;

                SaleBuyValue = _saleBuyValueDec.ToString("G");
            }
        }

        public string ProfitLoss
        {
            get => Helper.FormatDecimal(_profitLossDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
            set
            {
                if (Equals(_profitLoss, value))
                    return;
                _profitLoss = value;

                if (!decimal.TryParse(_profitLoss, out _profitLossDec))
                    ProfitLossDec = 0;
            }
        }

        public decimal ProfitLossDec
        {
            get => _profitLossDec;
            set
            {
                if (Equals(_profitLossDec, value))
                    return;
                _profitLossDec = value;

                ProfitLoss = _profitLossDec.ToString("G");

                UpdateView = true;
            }
        }

        public List<SaleBuyDetails> UsedBuyDetails { get; set; }

        public string TaxAtSource
        {
            get
            {
                if (!decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;

                if (UpdateViewFormatted)
                    return _taxAtSourceDec > 0 ? Helper.FormatDecimal(_taxAtSourceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : _taxAtSource;

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

                SalePrice = _taxAtSourceDec.ToString("G");
            }
        }

        public string CapitalGainsTax
        {
            get
            {
                if (!decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;

                if (UpdateViewFormatted)
                    return _capitalGainsTaxDec > 0 ? Helper.FormatDecimal(_capitalGainsTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : _capitalGainsTax;

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

                CapitalGainsTax = _capitalGainsTaxDec.ToString("G");
            }
        }

        public string SolidarityTax
        {
            get
            {
                if (!decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;

                if (UpdateViewFormatted)
                    return _solidarityTaxDec > 0 ? Helper.FormatDecimal(_solidarityTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : _solidarityTax;

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

                SolidarityTax = _solidarityTaxDec.ToString("G");
            }
        }

        public string Brokerage
        {
            get
            {
                if (!decimal.TryParse(_brokerage, out _brokerageDec))
                    _brokerageDec = 0;

                if (UpdateViewFormatted)
                    return _brokerageDec > 0 ? Helper.FormatDecimal(_brokerageDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : _brokerage;

                return _brokerage;
            }
            set
            {
                if (Equals(_brokerage, value))
                    return;
                _brokerage = value;

                if (!decimal.TryParse(_brokerage, out _brokerageDec))
                    _brokerageDec = 0;
            }
        }

        public decimal BrokerageDec
        {
            get => _brokerageDec;
            set
            {
                if (Equals(_brokerageDec, value))
                    return;
                _brokerageDec = value;

                Brokerage = _brokerageDec.ToString("G");
            }
        }

        public string Reduction
        {
            get
            {
                if (!decimal.TryParse(_reduction, out _reductionDec))
                    _reductionDec = 0;

                if (UpdateViewFormatted)
                    return _reductionDec > 0 ? Helper.FormatDecimal(_reductionDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : _reduction;

                return _reduction;
            }
            set
            {
                if (Equals(_reduction, value))
                    return;
                _reduction = value;

                if (!decimal.TryParse(_reduction, out _reductionDec))
                    _reductionDec = 0;
            }
        }

        public decimal ReductionDec
        {
            get => _reductionDec;
            set
            {
                if (Equals(_reductionDec, value))
                    return;
                _reductionDec = value;

                Reduction = _reductionDec.ToString("G");
            }
        }

        public string Payout
        {
            get => Helper.FormatDecimal(_payoutDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
            set
            {
                if (Equals(_payout, value))
                    return;
                _payout = value;

                if (!decimal.TryParse(_payout, out _payoutDec))
                    _payoutDec = 0;
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

                Payout = _payoutDec.ToString("G");

                UpdateView = true;
            }
        }

        public string Document
        {
            get => _document;
            set
            {
                if (_document != null && _document == value)
                    return;
                _document = value;
            }
        }

        #endregion IModel members
    }
}

