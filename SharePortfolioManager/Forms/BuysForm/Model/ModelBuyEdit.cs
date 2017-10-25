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

    /// <summary>
    /// Model class of the BuyEdit
    /// </summary>
    public class ModelBuyEdit : IModelBuyEdit
    {
        #region Fields

        bool _updateView;
        bool _updateViewFormatted;
        bool _updateBuy;

        BuyErrorCode _errorCode;
        string _selectedDate;

        ShareObjectMarketValue _shareObjectMarketValue;
        ShareObjectFinalValue _shareObjectFinalValue;
        List<WebSiteRegex> _webSiteRegexList;

        Logger _logger;
        Language _language;
        string _languageName;

        string _date;
        string _time;
        string _volume;
        decimal _volumeDec;
        string _marketValue;
        decimal _marketValueDec;
        string _reduction;
        decimal _reductionDec;
        string _purchaseValue;
        decimal _purchaseValueDec;
        string _costs;
        decimal _costsDec;
        string _finalValue;
        decimal _finalValueDec;
        string _sharePrice;
        decimal _sharePriceDec;
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
  
        public bool UpdateBuy
        {
            get { return _updateBuy; }
            set { _updateBuy = value; }
        }

        public BuyErrorCode ErrorCode
        {
            get { return _errorCode; }

            set
            {
                _errorCode = value;
            }
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

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get { return _shareObjectFinalValue; }
            set { _shareObjectFinalValue = value; }
        }

        public List<WebSiteRegex> WebSiteRegexList
        {
            get { return _webSiteRegexList; }
            set { _webSiteRegexList = value; }
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
                if (!decimal.TryParse(_volume, out _volumeDec))
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

        public string MarketValue
        {
            get
            {
                // Only return the value if the value is greater than '0'
                if (_marketValueDec > 0)
                    return Helper.FormatDecimal(_marketValueDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
            }

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
            get { return _marketValueDec; }
            set
            {
                if (_marketValueDec == value)
                    return;
                _marketValueDec = value;

                MarketValue = _marketValueDec.ToString();

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
                    if (_reductionDec > 0)
                        return Helper.FormatDecimal(_reductionDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _reduction;
                }
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
            get { return _reductionDec; }
            set
            {
                if (_reductionDec == value)
                    return;
                _reductionDec = value;

                Reduction = _reductionDec.ToString();
            }
        }

        public string PurchaseValue
        {
            get
            {
                // Only return the value if the value is greater than '0'
                if (PurchaseValueDec > 0)
                    return Helper.FormatDecimal(PurchaseValueDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
            }

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
            get { return _purchaseValueDec; }
            set
            {
                if (_purchaseValueDec == value)
                    return;
                _purchaseValueDec = value;

                PurchaseValue = _purchaseValueDec.ToString();
            }
        }

        public string Costs
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the costs is greater than '0'
                    if (_costsDec > 0)
                        return Helper.FormatDecimal(_costsDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _costs;
                }
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
            get { return _costsDec; }
            set
            {
                if (_costsDec == value)
                    return;
                _costsDec = value;

                Costs = _costsDec.ToString();
            }
        }

        public string FinalValue
        {
            get
            {
                // Only return the value if the value is greater than '0'
                if (_finalValueDec > 0)
                    return Helper.FormatDecimal(_finalValueDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
            }

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
            get { return _finalValueDec; }
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
                    if (_sharePriceDec > 0)
                        return Helper.FormatDecimal(_sharePriceDec, Helper.Currencysixlength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _sharePrice;
                }
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
            get { return _sharePriceDec; }
            set
            {
                if (_sharePriceDec == value)
                    return;
                _sharePriceDec = value;

                SharePrice = _sharePriceDec.ToString();
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
