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
using SharePortfolioManager.Forms.BrokerageForm.View;
using System.Collections.Generic;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.BrokerageForm.Model
{
    /// <summary>
    /// Interface of the BrokerageEdit model
    /// </summary>
    public interface IModelBrokerageEdit
    {
        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<WebSiteRegex> WebSiteRegexList { get; set; }
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }
        bool UpdateBrokerage { get; set; }
        BrokerageErrorCode ErrorCode { get; set; }
        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }
        bool PartOfABuy { get; set; }
        bool PartOfASale { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Brokerage { get; set; }
        decimal BrokerageDec { get; set; }
        string Document { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Model class of the BrokerageEdit
    /// </summary>
    public class ModelBrokerageEdit : IModelBrokerageEdit
    {
        #region Fields

        private string _date;
        private string _time;
        private string _brokerage;
        private decimal _brokerageDec;
        private string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView { get; set; }

        public bool UpdateViewFormatted { get; set; }

        public bool UpdateBrokerage { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public List<WebSiteRegex> WebSiteRegexList { get; set; }

        public BrokerageErrorCode ErrorCode { get; set; }

        public string SelectedGuid { get; set; }

        public string SelectedDate { get; set; }

        public bool PartOfABuy { get; set; }

        public bool PartOfASale { get; set; }

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

        public string Brokerage
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the brokerage is greater than '0'
                    return _brokerageDec > 0 ? Helper.FormatDecimal(_brokerageDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _brokerage;
            }
            set
            {
                if (_brokerage == value)
                    return;
                _brokerage = value;

                // Try to parse
                if (!decimal.TryParse(_brokerage, out _brokerageDec))
                    _brokerageDec = 0;
            }
        }

        public decimal BrokerageDec
        {
            get => _brokerageDec;
            set
            {
                if (_brokerageDec == value)
                    return;
                _brokerageDec = value;

                Brokerage = _brokerageDec.ToString("G");
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
