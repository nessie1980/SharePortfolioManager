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
using System.Globalization;
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
        string MarketValue { get; set; }
        decimal MarketValueDec { get; set; }
        string Reduction { get; set; }
        decimal ReductionDec { get; set; }
        string PurchaseValue { get; set; }
        decimal PurchaseValueDec { get; set; }
        string Costs { get; set; }
        decimal CostsDec { get; set; }
        string FinalValue { get; set; }
        decimal FinalValueDec { get; set; }
        string SharePrice { get; set; }
        decimal SharePricedec { get; set; }
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
        private string _marketValue;
        private decimal _marketValueDec;
        private string _reduction;
        private decimal _reductionDec;
        private string _purchaseValue;
        private decimal _purchaseValueDec;
        private string _costs;
        private decimal _costsDec;
        private string _finalValue;
        private decimal _finalValueDec;
        private string _sharePrice;
        private decimal _sharePriceDec;
        private string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView { get; set; }

        public bool UpdateViewFormatted { get; set; }

        public bool UpdateBuy { get; set; }

        public BuyErrorCode ErrorCode { get; set; }

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
                if (UpdateViewFormatted)
                {
                    // Only return the value if the volume is greater than '0'
                    return _volumeDec <= 0 ? @"" : Helper.FormatDecimal(_volumeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
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
                    return;
                _volumeDec = value;
            }
    }

        public string MarketValue
        {
            get => _marketValueDec > 0 ? Helper.FormatDecimal(_marketValueDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";

            set
            {
                if (_marketValue == value)
                    return;
                _marketValue = value;

                // Try to parse
                if (!decimal.TryParse(_marketValue, out _marketValueDec))
                    _marketValueDec = 0;
            }
        }

        public decimal MarketValueDec
        {
            get => _marketValueDec;
            set
            {
                if (_marketValueDec == value)
                    return;
                _marketValueDec = value;

                MarketValue = _marketValueDec.ToString("G");

                UpdateView = true;
            }
        }

        public string Reduction
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the reduction is greater than '0'
                    return _reductionDec > 0 ? Helper.FormatDecimal(_reductionDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _reduction;
            }
            set
            {
                if (_reduction == value)
                    return;
                _reduction = value;

                // Try to parse
                if (!decimal.TryParse(_reduction, out _reductionDec))
                    _reductionDec = 0;
            }
        }

        public decimal ReductionDec
        {
            get => _reductionDec;
            set
            {
                if (_reductionDec == value)
                    return;
                _reductionDec = value;

                Reduction = _reductionDec.ToString(CultureInfo.CurrentCulture);
            }
        }

        public string PurchaseValue
        {
            get => PurchaseValueDec > 0 ? Helper.FormatDecimal(PurchaseValueDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";

            set
            {
                if (_purchaseValue == value)
                    return;
                _purchaseValue = value;

                // Try to parse
                if (!decimal.TryParse(_purchaseValue, out _purchaseValueDec))
                    PurchaseValueDec = 0;
            }
        }

        public decimal PurchaseValueDec
        {
            get => _purchaseValueDec;
            set
            {
                if (_purchaseValueDec == value)
                    return;
                _purchaseValueDec = value;

                PurchaseValue = _purchaseValueDec.ToString(CultureInfo.CurrentCulture);
            }
        }

        public string Costs
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the costs is greater than '0'
                    return _costsDec > 0 ? Helper.FormatDecimal(_costsDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _costs;
            }
            set
            {
                if (_costs == value)
                    return;
                _costs = value;

                // Try to parse
                if (!decimal.TryParse(_costs, out _costsDec))
                    _costsDec = 0;
            }
        }

        public decimal CostsDec
        {
            get => _costsDec;
            set
            {
                if (_costsDec == value)
                    return;
                _costsDec = value;

                Costs = _costsDec.ToString("G");
            }
        }

        public string FinalValue
        {
            get => _finalValueDec > 0 ? Helper.FormatDecimal(_finalValueDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";

            set
            {
                if (_finalValue == value)
                    return;
                _finalValue = value;

                // Try to parse
                if (!decimal.TryParse(_finalValue, out _finalValueDec))
                    FinalValueDec = 0;
            }
        }

        public decimal FinalValueDec
        {
            get => _finalValueDec;
            set
            {
                if (_finalValueDec == value)
                    return;
                _finalValueDec = value;
            }
        }

        public string SharePrice
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the share price is greater than '0'
                    return _sharePriceDec > 0 ? Helper.FormatDecimal(_sharePriceDec, Helper.Currencysixlength, true, Helper.Currencytwofixlength) : @"";
                }

                return _sharePrice;
            }
            set
            {
                if (_sharePrice == value)
                    return;
                _sharePrice = value;

                // Try to parse
                if (!decimal.TryParse(_sharePrice, out _sharePriceDec))
                    _sharePriceDec = 0;
            }
        }

        public decimal SharePricedec
        {
            get => _sharePriceDec;
            set
            {
                if (_sharePriceDec == value)
                    return;
                _sharePriceDec = value;

                SharePrice = _sharePriceDec.ToString("G");
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
