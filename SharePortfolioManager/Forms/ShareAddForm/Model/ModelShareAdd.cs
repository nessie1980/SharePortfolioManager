//MIT License
//
//Copyright(c) 2017 - 2022 nessie1980(nessie1980@gmx.de)
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
//#define DEBUG_SHARE_ADD_MODEL

using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ParserRegex;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.ShareAddForm.View;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using LanguageHandler;
using Logging;

namespace SharePortfolioManager.ShareAddForm.Model
{
    /// <summary>
    /// Interface of the ShareAdd model
    /// </summary>
    public interface IModelShareAdd
    {
        ShareAddErrorCode ErrorCode { get; set; }
        bool UpdateView { get; }
        bool UpdateViewFormatted { get; set; }

        List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }
        List<ShareObjectFinalValue> ShareObjectListFinalValue { get; set; }
        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        List<Image> ImageListPrevDayPerformance { get; set; }
        List<Image> ImageListCompletePerformance { get; set; }

        List<WebSiteRegex> WebSiteRegexList { get; set; }

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        string Wkn { get; set; }
        string Name { get; set; }
        string Isin { get; set; }
        string StockMarketLaunchDate { get; set; }
        ShareObject.ShareTypes ShareType { get; set; }
        int DividendPayoutInterval { get; set; }
        CultureInfo CultureInfo { get; set; }
        string DetailsWebSite { get; set; }
        string MarketValuesWebSite { get; set; }
        ShareObject.ParsingTypes MarketValuesParsingOption { get; set; }
        string MarketValuesParsingApiKey { get; set; }
        string DailyValuesWebSite { get; set; }
        ShareObject.ParsingTypes DailyValuesParsingOption { get; set; }
        string DailyValuesParsingApiKey { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string DepotNumber { get; set; }
        string OrderNumber { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string SharePrice { get; set; }
        decimal SharePriceDec { get; set; }
        string Provision { get; set; }
        decimal ProvisionDec { get; set; }
        string BrokerFee { get; set; }
        decimal BrokerFeeDec { get; set; }
        string TraderPlaceFee { get; set; }
        decimal TraderPlaceFeeDec { get; set; }
        string Reduction { get; set; }
        decimal ReductionDec { get; set; }
        string Brokerage { get; set; }
        decimal BrokerageDec { get; set; }
        string BrokerageReduction { get; set; }
        decimal BrokerageReductionDec { get; set; }
        string BuyValue { get; set; }
        decimal BuyValueDec { get; set; }
        string BuyValueReduction { get; set; }
        decimal BuyValueReductionDec { get; set; }
        string BuyValueBrokerage { get; set; }
        decimal BuyValueBrokerageDec { get; set; }
        string BuyValueBrokerageReduction { get; set; }
        decimal BuyValueBrokerageReductionDec { get; set; }
        string Document { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Model class of the ShareAdd
    /// </summary>
    public class ModelShareAdd : IModelShareAdd
    {
        #region Fields

        private string _wkn;
        private string _isin;
        private string _name;
        private string _stockMarketLaunchDate;
        private ShareObject.ShareTypes _shareType;
        private int _dividendPayoutInterval;
        private CultureInfo _cultureInfo;
        private string _detailsWebSite;
        private string _marketValuesWebSite;
        private ShareObject.ParsingTypes _marketValuesParsingOption;
        private string _marketValuesParsingApiKey;
        private string _dailyValuesWebSite;
        private ShareObject.ParsingTypes _dailyValuesParsingOption;
        private string _dailyValuesParsingApiKey;
        private string _date;
        private string _time;
        private string _depotNumber;
        private string _orderNumber;
        private string _volume;
        private decimal _volumeDec;
        private string _sharePrice;
        private decimal _sharePriceDec;
        private string _provision;
        private decimal _provisionDec;
        private string _brokerFee;
        private decimal _brokerFeeDec;
        private string _traderPlaceFee;
        private decimal _traderPlaceFeeDec;
        private string _reduction;
        private decimal _reductionDec;
        private string _brokerage;
        private decimal _brokerageDec;
        private string _brokerageReduction;
        private decimal _brokerageReductionDec;
        private string _buyValue;
        private decimal _buyValueDec;
        private string _buyValueReduction;
        private decimal _buyValueReductionDec;
        private string _buyValueBrokerage;
        private decimal _buyValueBrokerageDec;
        private string _buyValueBrokerageReduction;
        private decimal _buyValueBrokerageReductionDec;
        private string _document;

        #endregion Fields

        #region IModel Members

        public bool UpdateView { get; private set; }

        public bool UpdateViewFormatted { get; set; }

        public ShareAddErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue { get; set; }

        public List<Image> ImageListPrevDayPerformance { get; set; }

        public List<Image> ImageListCompletePerformance { get; set; }

        public List<WebSiteRegex> WebSiteRegexList { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

        public string Wkn
        {
            get => _wkn;
            set
            {
                if (Equals(_wkn, value))
                    return;
                _wkn = value;
            }
        }

        public string Isin
        {
            get => _isin;
            set
            {
                if (Equals(_isin, value))
                    return;
                _isin = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (Equals(_name, value))
                    return;
                _name = value;
            }
        }

        public string StockMarketLaunchDate
        {
            get => _stockMarketLaunchDate;
            set
            {
                if (Equals(_stockMarketLaunchDate, value))
                    return;
                _stockMarketLaunchDate = value;
            }
        }

        public ShareObject.ShareTypes ShareType
        {
            get => _shareType;
            set
            {
                if (Equals(_shareType, value))
                    return;
                _shareType = value;
            }
        }

        public int DividendPayoutInterval
        {
            get => _dividendPayoutInterval;
            set
            {
                if (Equals(_dividendPayoutInterval, value))
                    return;
                _dividendPayoutInterval = value;
            }
        }

        public CultureInfo CultureInfo
        {
            get => _cultureInfo;
            set
            {
                if (Equals(_cultureInfo, value))
                    return;
                _cultureInfo = value;
            }
        }

        public string DetailsWebSite
        {
            get => _detailsWebSite;
            set
            {
                if (Equals(_detailsWebSite, value))
                    return;
                _detailsWebSite = value;
            }
        }

        public string MarketValuesWebSite
        {
            get => _marketValuesWebSite;
            set
            {
                if (Equals(_marketValuesWebSite, value))
                    return;
                _marketValuesWebSite = value;
            }
        }

        public ShareObject.ParsingTypes MarketValuesParsingOption
        {
            get => _marketValuesParsingOption;
            set
            {
                if (Equals(_marketValuesParsingOption, value))
                    return;
                _marketValuesParsingOption = value;
            }
        }

        public string MarketValuesParsingApiKey
        {
            get => _marketValuesParsingApiKey;
            set
            {
                if (Equals(_marketValuesParsingApiKey, value))
                    return;
                _marketValuesParsingApiKey = value;
            }
        }

        public string DailyValuesWebSite
        {
            get => _dailyValuesWebSite;
            set
            {
                if (Equals(_dailyValuesWebSite, value))
                    return;
                _dailyValuesWebSite = value;
            }
        }

        public ShareObject.ParsingTypes DailyValuesParsingOption
        {
            get => _dailyValuesParsingOption;
            set
            {
                if (Equals(_dailyValuesParsingOption, value))
                    return;
                _dailyValuesParsingOption = value;
            }
        }

        public string DailyValuesParsingApiKey
        {
            get => _dailyValuesParsingApiKey;
            set
            {
                if (Equals(_dailyValuesParsingApiKey, value))
                    return;
                _dailyValuesParsingApiKey = value;
            }
        }

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

        public string DepotNumber
        {
            get => _depotNumber;
            set
            {
                if (Equals(_depotNumber, value))
                    return;
                _depotNumber = value;
            }
        }

        public string OrderNumber
        {
            get => _orderNumber;
            set
            {
                if (Equals(_orderNumber, value))
                    return;
                _orderNumber = value;
            }
        }

        public string Volume
        {
            get
            {
                if (!decimal.TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;

                if (UpdateViewFormatted)
                    return _volumeDec > 0 ? Helper.FormatDecimal(_volumeDec, Helper.CurrencyFiveLength, true, Helper.CurrencyTwoFixLength) : _volume;

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

        public string SharePrice
        {
            get
            {
                if (!decimal.TryParse(_sharePrice, out _sharePriceDec))
                    _sharePriceDec = 0;

                if (UpdateViewFormatted)
                    return _sharePriceDec > 0 ? Helper.FormatDecimal(_sharePriceDec, Helper.CurrencyFiveLength, true, Helper.CurrencyTwoFixLength) : _sharePrice;

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

        public string Provision
        {
            get
            {
                if (!decimal.TryParse(_provision, out _provisionDec))
                    _provisionDec = 0;

                if (UpdateViewFormatted)
                    return _provisionDec > 0 ? Helper.FormatDecimal(_provisionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _provision;

                return _provision;
            }
            set
            {
                if (Equals(_provision, value))
                    return;
                _provision = value;

                if (!decimal.TryParse(_provision, out _provisionDec))
                    _provisionDec = 0;
            }
        }

        public decimal ProvisionDec
        {
            get => _provisionDec;
            set
            {

                if (Equals(_provisionDec, value))
                    return;
                _provisionDec = value;
            }
        }

        public string BrokerFee
        {
            get
            {
                if (!decimal.TryParse(_brokerFee, out _brokerFeeDec))
                    _brokerFeeDec = 0;

                if (UpdateViewFormatted)
                    return _brokerFeeDec > 0 ? Helper.FormatDecimal(_brokerFeeDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _brokerFee;

                return _brokerFee;
            }
            set
            {
                if (Equals(_brokerFee, value))
                    return;
                _brokerFee = value;

                if (!decimal.TryParse(_brokerFee, out _brokerFeeDec))
                    _brokerFeeDec = 0;
            }
        }

        public decimal BrokerFeeDec
        {
            get => _brokerFeeDec;
            set
            {

                if (Equals(_brokerFeeDec, value))
                    return;
                _brokerFeeDec = value;
            }
        }

        public string TraderPlaceFee
        {
            get
            {
                if (!decimal.TryParse(_traderPlaceFee, out _traderPlaceFeeDec))
                    _traderPlaceFeeDec = 0;

                if (UpdateViewFormatted)
                    return _traderPlaceFeeDec > 0 ? Helper.FormatDecimal(_traderPlaceFeeDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _traderPlaceFee;

                return _traderPlaceFee;
            }
            set
            {
                if (Equals(_traderPlaceFee, value))
                    return;
                _traderPlaceFee = value;

                if (!decimal.TryParse(_traderPlaceFee, out _traderPlaceFeeDec))
                    _traderPlaceFeeDec = 0;
            }
        }

        public decimal TraderPlaceFeeDec
        {
            get => _traderPlaceFeeDec;
            set
            {

                if (Equals(_traderPlaceFeeDec, value))
                    return;
                _traderPlaceFeeDec = value;
            }
        }

        public string Reduction
        {
            get
            {
                if (!decimal.TryParse(_reduction, out _reductionDec))
                    _reductionDec = 0;

                if (UpdateViewFormatted)
                    return _reductionDec > 0 ? Helper.FormatDecimal(_reductionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _reduction;

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

        public string Brokerage
        {
            get => _brokerageDec >= 0 ? Helper.FormatDecimal(_brokerageDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _brokerage;
            set
            {
                if (Equals(_brokerage, value))
                    return;
                _brokerage = value;
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

                UpdateView = true;
            }
        }

        public string BrokerageReduction
        {
            get => _brokerageReductionDec >= 0 ? Helper.FormatDecimal(_brokerageReductionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _brokerage;
            set
            {
                if (Equals(_brokerageReduction, value))
                    return;
                _brokerageReduction = value;
            }
        }

        public decimal BrokerageReductionDec
        {
            get => _brokerageReductionDec;
            set
            {
                if (Equals(_brokerageReductionDec, value))
                    return;
                _brokerageReductionDec = value;

                UpdateView = true;
            }
        }

        public string BuyValue
        {
            get => _buyValueDec >= 0 ? Helper.FormatDecimal(_buyValueDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : @"";
            set
            {
                if (Equals(_buyValue, value))
                    return;
                _buyValue = value;
            }
        }

        public decimal BuyValueDec
        {
            get => _buyValueDec;
            set
            {
                if (Equals(_buyValueDec, value))
                    return;
                _buyValueDec = value;

                UpdateView = true;
            }
        }

        public string BuyValueReduction
        {
            get => _buyValueReductionDec >= 0 ? Helper.FormatDecimal(_buyValueReductionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : @"";
            set
            {
                if (Equals(_buyValueReduction, value))
                    return;
                _buyValueReduction = value;
            }
        }

        public decimal BuyValueReductionDec
        {
            get => _buyValueReductionDec;
            set
            {
                if (Equals(_buyValueReductionDec, value))
                    return;
                _buyValueReductionDec = value;

                UpdateView = true;
            }
        }

        public string BuyValueBrokerage
        {
            get => _buyValueBrokerageDec >= 0 ? Helper.FormatDecimal(_buyValueBrokerageDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : @"";
            set
            {
                if (Equals(_buyValueBrokerage, value))
                    return;
                _buyValueBrokerage = value;
            }
        }

        public decimal BuyValueBrokerageDec
        {
            get => _buyValueBrokerageDec;
            set
            {
                if (Equals(_buyValueBrokerageDec, value))
                    return;
                _buyValueBrokerageDec = value;

                UpdateView = true;
            }
        }

        public string BuyValueBrokerageReduction
        {
            get => _buyValueBrokerageReductionDec >= 0 ? Helper.FormatDecimal(_buyValueBrokerageReductionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _buyValueBrokerageReduction;
            set
            {
                if (Equals(_buyValueBrokerageReduction, value))
                    return;
                _buyValueBrokerageReduction = value;
            }
        }

        public decimal BuyValueBrokerageReductionDec
        {
            get => _buyValueBrokerageReductionDec;
            set
            {
                if (Equals(_buyValueBrokerageReductionDec, value))
                    return;
                _buyValueBrokerageReductionDec = value;
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

        #endregion IModel Members
    }
}
