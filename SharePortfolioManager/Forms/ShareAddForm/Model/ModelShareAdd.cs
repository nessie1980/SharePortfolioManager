﻿//MIT License
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
        string Document { get; set; }
    }

    /// <summary>
    /// Model class of the ShareAdd
    /// </summary>
    public class ModelShareAdd : IModelShareAdd
    {
        #region Fields

        bool _updateView;
        bool _updateViewFormatted;

        ShareAddErrorCode _errorCode;

        ShareObjectMarketValue _shareObjectMarketValue;
        List<ShareObjectMarketValue> _shareObjectListMarketValue;
        ShareObjectFinalValue _shareObjectFinalValue;
        List<ShareObjectFinalValue> _shareObjectListFinalValue;

        List<Image> _imageList;

        List<WebSiteRegex> _webSiteRegexList;

        string _wkn;
        string _date;
        string _time;
        string _name;
        string _volume;
        decimal _volumedec;
        string _sharePrice;
        decimal _sharePricedec;
        string _marketValue;
        decimal _marketValuedec;
        string _costs;
        decimal _costsdec;
        string _reduction;
        decimal _reductiondec;
        string _finalValue;
        decimal _finalValuedec;
        string _webSite;
        CultureInfo _cultureInfo;
        int _dividendPayoutInterval;
        string _document;

        #endregion Fields

        #region IModel Members

        public bool UpdateView
        {
            get { return _updateView; }
        }

        public bool UpdateViewFormatted
        {
            get { return _updateViewFormatted; }
            set { _updateViewFormatted = value; }
        }

        public ShareAddErrorCode ErrorCode
        {
            get { return _errorCode; }

            set
            {
                _errorCode = value;
            }
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

        public List<Image> ImageList
        {
            get { return _imageList; }
            set { _imageList = value; }
        }

        public List<WebSiteRegex> WebSiteRegexList
        {
            get { return _webSiteRegexList; }
            set { _webSiteRegexList = value; }
        }

        public string Wkn
        {
            get { return _wkn; }
            set
            {
                if (_wkn == value)
                    return;
                _wkn = value;
            }
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

        public string Name
        {
            get { return _name; }
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
                    if (_volumedec > 0)
                        return Helper.FormatDecimal(_volumedec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);
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
                if (!decimal.TryParse(_volume, out _volumedec))
                    _volumedec = 0;
            }
        }

        public decimal Volumedec
        {
            get { return _volumedec; }
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
                    if (_sharePricedec > 0)
                        return Helper.FormatDecimal(_sharePricedec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength);
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
                if (!decimal.TryParse(_sharePrice, out _sharePricedec))
                    _sharePricedec = 0;
            }
        }

        public decimal SharePricedec
        {
            get { return _sharePricedec; }
            set
            {

                if (_sharePricedec == value)
                    return;
                _sharePricedec = value;

                SharePrice = _sharePricedec.ToString();
            }
        }

        public string MarketValue
        {
            get
            {
                // Only return the value if the value is greater than '0'
                if (_marketValuedec > 0)
                    return Helper.FormatDecimal(_marketValuedec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
            }

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
            get { return _marketValuedec; }
            set
            {

                if (_marketValuedec == value)
                    return;
                _marketValuedec = value;

                MarketValue = _marketValuedec.ToString();

                _updateView = true;
            }
        }

        public string Costs
        {
            get
            {
               if (UpdateViewFormatted)
                {
                    // Only return the value if the costs is greater than '0'
                    if (_costsdec > 0)
                        return Helper.FormatDecimal(_costsdec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
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
                if (!decimal.TryParse(_costs, out _costsdec))
                    _costsdec = 0;
            }
        }

        public decimal Costsdec
        {
            get { return _costsdec; }
            set
            {

                if (_costsdec == value)
                    return;
                _costsdec = value;

                Costs = _costsdec.ToString();
            }
        }

        public string Reduction
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the reduction is greater than '0'
                    if (_reductiondec > 0)
                        return Helper.FormatDecimal(_reductiondec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
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
                if (!decimal.TryParse(_reduction, out _reductiondec))
                    _reductiondec = 0;
            }
        }

        public decimal Reductiondec
        {
            get { return _reductiondec; }
            set
            {

                if (_reductiondec == value)
                    return;
                _reductiondec = value;

                Reduction = _reductiondec.ToString();
            }
        }

        public string FinalValue
        {
            get
            {
                // Only return the value if the value is greater than '0'
                if (_finalValuedec > 0)
                    return Helper.FormatDecimal(_finalValuedec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                return @"";
            }

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
            get { return _finalValuedec; }
            set
            {
                if (_finalValuedec == value)
                    return;
                _finalValuedec = value;
            }
        }

        public string WebSite
        {
            get { return _webSite; }
            set
            {
                if (_webSite == value)
                    return;
                _webSite = value;
            }
        }

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set
            {
                if (_cultureInfo == value)
                    return;
                _cultureInfo = value;
            }
        }

        public int DividendPayoutInterval
        {
            get { return _dividendPayoutInterval; }
            set
            {
                if (_dividendPayoutInterval == value)
                    return;
                _dividendPayoutInterval = value;
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

        #endregion IModel Members
    }
}
