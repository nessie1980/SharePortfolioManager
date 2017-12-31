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
using SharePortfolioManager.Forms.ShareAddForm.View;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.ShareAddForm.Model
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

        List<Image> ImageList { get; set; }

        List<WebSiteRegex> WebSiteRegexList { get; set; }

        string Wkn { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Name { get; set; }
        string Volume { get; set; }
        decimal Volumedec { get; set; }
        string SharePrice { get; set; }
        decimal SharePricedec { get; set; }
        string MarketValue { get; set; }
        decimal MarketValuedec { get; set; }
        string Costs { get; set; }
        decimal Costsdec { get; set; }
        string Reduction { get; set; }
        decimal Reductiondec { get; set; }
        string FinalValue { get; set; }
        decimal FinalValuedec { get; set; }
        string WebSite { get; set; }
        CultureInfo CultureInfo { get; set; }
        int DividendPayoutInterval { get; set; }
        int ShareType { get; set; }
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
        private string _date;
        private string _time;
        private string _name;
        private string _volume;
        private decimal _volumedec;
        private string _sharePrice;
        private decimal _sharePricedec;
        private string _marketValue;
        private decimal _marketValuedec;
        private string _costs;
        private decimal _costsdec;
        private string _reduction;
        private decimal _reductiondec;
        private string _finalValue;
        private decimal _finalValuedec;
        private string _webSite;
        private CultureInfo _cultureInfo;
        private int _dividendPayoutInterval;
        private int _shareType;
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

        public List<Image> ImageList { get; set; }

        public List<WebSiteRegex> WebSiteRegexList { get; set; }

        public string Wkn
        {
            get => _wkn;
            set
            {
                if (_wkn == value)
                    return;
                _wkn = value;
            }
        }

        public string Date
        {
            get => _date;
            set
            {
                if (_date == value)
                    return;

                _date = value;
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                if (_time == value)
                    return;

                _time = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;
            }
        }

        public string Volume
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the volume is greater than '0'
                    return _volumedec > 0 ? Helper.FormatDecimal(_volumedec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";
                }

                return _volume;
            }
            set
            {
                if (_volume == value)
                    return;
                _volume = value;

                // Try to parse
                if (!decimal.TryParse(_volume, out _volumedec))
                    _volumedec = 0;
            }
        }

        public decimal Volumedec
        {
            get => _volumedec;
            set
            {
                if (_volumedec == value)
                    return;
                _volumedec = value;
            }
        }

        public string SharePrice
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the share price is greater than '0'
                    return _sharePricedec > 0 ? Helper.FormatDecimal(_sharePricedec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength) : @"";
                }

                return _sharePrice;
            }
            set
            {
                if (_sharePrice == value)
                    return;
                _sharePrice = value;

                // Try to parse
                if (!decimal.TryParse(_sharePrice, out _sharePricedec))
                    _sharePricedec = 0;
            }
        }

        public decimal SharePricedec
        {
            get => _sharePricedec;
            set
            {

                if (_sharePricedec == value)
                    return;
                _sharePricedec = value;

                SharePrice = _sharePricedec.ToString("G");
            }
        }

        public string MarketValue
        {
            get => _marketValuedec > 0 ? Helper.FormatDecimal(_marketValuedec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";

            set
            {
                if (_marketValue == value)
                    return;
                _marketValue = value;

                // Try to parse
                if (!decimal.TryParse(_marketValue, out _marketValuedec))
                    _marketValuedec = 0;
            }
        }

        public decimal MarketValuedec
        {
            get => _marketValuedec;
            set
            {

                if (_marketValuedec == value)
                    return;
                _marketValuedec = value;

                MarketValue = _marketValuedec.ToString("G");

                UpdateView = true;
            }
        }

        public string Costs
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the costs is greater than '0'
                    return _costsdec > 0 ? Helper.FormatDecimal(_costsdec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _costs;
            }
            set
            {
                if (_costs == value)
                    return;
                _costs = value;

                // Try to parse
                if (!decimal.TryParse(_costs, out _costsdec))
                    _costsdec = 0;
            }
        }

        public decimal Costsdec
        {
            get => _costsdec;
            set
            {

                if (_costsdec == value)
                    return;
                _costsdec = value;

                Costs = _costsdec.ToString("G");
            }
        }

        public string Reduction
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the reduction is greater than '0'
                    return _reductiondec > 0 ? Helper.FormatDecimal(_reductiondec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _reduction;
            }
            set
            {
                if (_reduction == value)
                    return;
                _reduction = value;

                // Try to parse
                if (!decimal.TryParse(_reduction, out _reductiondec))
                    _reductiondec = 0;
            }
        }

        public decimal Reductiondec
        {
            get => _reductiondec;
            set
            {

                if (_reductiondec == value)
                    return;
                _reductiondec = value;

                Reduction = _reductiondec.ToString("G");
            }
        }

        public string FinalValue
        {
            get => _finalValuedec > 0 ? Helper.FormatDecimal(_finalValuedec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";

            set
            {
                if (_finalValue == value)
                    return;
                _finalValue = value;

                // Try to parse
                if (!decimal.TryParse(_finalValue, out _finalValuedec))
                    _finalValuedec = 0;
            }
        }

        public decimal FinalValuedec
        {
            get => _finalValuedec;
            set
            {
                if (_finalValuedec == value)
                    return;
                _finalValuedec = value;
            }
        }

        public string WebSite
        {
            get => _webSite;
            set
            {
                if (_webSite == value)
                    return;
                _webSite = value;
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

        public int DividendPayoutInterval
        {
            get => _dividendPayoutInterval;
            set
            {
                if (_dividendPayoutInterval == value)
                    return;
                _dividendPayoutInterval = value;
            }
        }

        public int ShareType
        {
            get => _shareType;
            set
            {
                if (_shareType == value)
                    return;
                _shareType = value;
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

        #endregion IModel Members
    }
}
