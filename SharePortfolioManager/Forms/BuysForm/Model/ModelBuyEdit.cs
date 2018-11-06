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
using SharePortfolioManager.Forms.BuysForm.View;
using System.Collections.Generic;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.BuysForm.Model
{
    /// <summary>
    /// Interface of the BuyEdit model
    /// </summary>
    public interface IModelBuyEdit
    {
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }
        bool UpdateBuy { get; set; }

        BuyErrorCode ErrorCode { get; set; }
        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<WebSiteRegex> WebSiteRegexList { get; set; }

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string VolumeSold { get; set; }
        decimal VolumeSoldDec { get; set; }
        string MarketValue { get; set; }
        decimal MarketValueDec { get; set; }
        string Reduction { get; set; }
        decimal ReductionDec { get; set; }
        decimal PurchaseValueDec { get; set; }
        string Brokerage { get; set; }
        decimal BrokerageDec { get; set; }
        string FinalValue { get; set; }
        decimal FinalValueDec { get; set; }
        string SharePrice { get; set; }
        decimal SharePriceDec { get; set; }
        string Document { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Model class of the BuyEdit
    /// </summary>
    public class ModelBuyEdit : IModelBuyEdit
    {
        #region Fields

        private string _date;
        private string _time;
        private string _volume;
        private decimal _volumeDec;
        private string _volumeSold;
        private decimal _volumeSoldDec;
        private string _sharePrice;
        private decimal _sharePriceDec;
        private string _marketValue;
        private decimal _marketValueDec;
        private string _brokerage;
        private decimal _brokerageDec;
        private string _reduction;
        private decimal _reductionDec;
        private string _purchaseValue;
        private decimal _purchaseValueDec;
        private string _finalValue;
        private decimal _finalValueDec;
        private string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView { get; set; }

        public bool UpdateViewFormatted { get; set; }

        public bool UpdateBuy { get; set; }

        public BuyErrorCode ErrorCode { get; set; }

        public string SelectedGuid { get; set; }

        public string SelectedDate { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public List<WebSiteRegex> WebSiteRegexList { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

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

        public string VolumeSold
        {
            get
            {
                if (!decimal.TryParse(_volumeSold, out _volumeSoldDec))
                    _volumeSoldDec = 0;

                if (UpdateViewFormatted)
                    return _volumeSoldDec >= 0 ? Helper.FormatDecimal(_volumeSoldDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength): _volumeSold;

                return _volumeSold;
            }
            set
            {
                if (Equals(_volumeSold, value))
                    return;
                _volumeSold = value;

                if (!decimal.TryParse(_volumeSold, out _volumeSoldDec))
                    _volumeSoldDec = 0;
            }
        }

        public decimal VolumeSoldDec
        {
            get => _volumeSoldDec;
            set
            {
                if (Equals(_volumeSoldDec, value))
                    return;
                _volumeSoldDec = value;
            }
        }

        public string SharePrice
        {
            get
            {
                if (!decimal.TryParse(_sharePrice, out _sharePriceDec))
                    _sharePriceDec = 0;

                if (UpdateViewFormatted)
                    return _sharePriceDec > 0 ? Helper.FormatDecimal(_sharePriceDec, Helper.Currencysixlength, false, Helper.Currencytwofixlength) : _sharePrice;

                return _sharePrice;
            }
            set
            {
                if (Equals(_sharePrice, value))
                    return;
                _sharePrice = value;

                if (!decimal.TryParse(_sharePrice, out _sharePriceDec))
                    _sharePriceDec = 0;
            }
        }

        public decimal SharePriceDec
        {
            get => _sharePriceDec;
            set
            {
                if (Equals(_sharePriceDec, value))
                    return;
                _sharePriceDec = value;
            }
        }

        public string MarketValue
        {
            get => _marketValueDec >= 0 ? Helper.FormatDecimal(_marketValueDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _marketValue;
            set
            {
                if (Equals(_marketValue, value))
                    return;
                _marketValue = value;

                if (!decimal.TryParse(_marketValue, out _marketValueDec))
                    _marketValueDec = 0;
            }
        }

        public decimal MarketValueDec
        {
            get => _marketValueDec;
            set
            {
                if (Equals(_marketValueDec, value))
                    return;
                _marketValueDec = value;

                UpdateView = true;
            }
        }

        public string Brokerage
        {
            get
            {
                if (!decimal.TryParse(_brokerage, out _brokerageDec))
                    _brokerageDec = 0;

                if (UpdateViewFormatted)
                    return _brokerageDec > 0 ? Helper.FormatDecimal(_brokerageDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _brokerage;

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
            }
        }

        public string Reduction
        {
            get
            {
                if (!decimal.TryParse(_reduction, out _reductionDec))
                    _reductionDec = 0;

                if (UpdateViewFormatted)
                    return _reductionDec > 0 ? Helper.FormatDecimal(_reductionDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _reduction;

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
            }
        }

        public string PurchaseValue
        {
            get => PurchaseValueDec >= 0 ? Helper.FormatDecimal(_purchaseValueDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _purchaseValue;
            set
            {
                if (Equals(_purchaseValue, value))
                    return;
                _purchaseValue = value;

                if (!decimal.TryParse(_purchaseValue, out _purchaseValueDec))
                    PurchaseValueDec = 0;
            }
        }

        public decimal PurchaseValueDec
        {
            get => _purchaseValueDec;
            set
            {
                if (Equals(_purchaseValueDec, value))
                    return;
                _purchaseValueDec = value;
            }
        }

        public string FinalValue
        {
            get => _finalValueDec >= 0 ? Helper.FormatDecimal(_finalValueDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _finalValue;
            set
            {
                if (Equals(_finalValue, value))
                    return;
                _finalValue = value;

                if (!decimal.TryParse(_finalValue, out _finalValueDec))
                    FinalValueDec = 0;
            }
        }

        public decimal FinalValueDec
        {
            get => _finalValueDec;
            set
            {
                if (Equals(_finalValueDec, value))
                    return;
                _finalValueDec = value;
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
