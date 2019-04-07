//MIT License
//
//Copyright(c) 2019 nessie1980(nessie1980 @gmx.de)
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
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Forms.BrokeragesForm.View;

namespace SharePortfolioManager.Forms.BrokeragesForm.Model
{
    /// <summary>
    /// Interface of the BrokerageEdit model
    /// </summary>
    public interface IModelBrokerageEdit
    {
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }
        bool UpdateBrokerage { get; set; }

        BrokerageErrorCode ErrorCode { get; set; }
        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        bool PartOfABuy { get; set; }
        bool PartOfASale { get; set; }
        string Date { get; set; }
        string Time { get; set; }
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
        private string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView { get; set; }

        public bool UpdateViewFormatted { get; set; }

        public bool UpdateBrokerage { get; set; }

        public BrokerageErrorCode ErrorCode { get; set; }

        public string SelectedGuid { get; set; }

        public string SelectedDate { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

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

        public string Provision
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the provision is greater than '0'
                    return _provisionDec > 0 ? Helper.FormatDecimal(_provisionDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _provision;
            }
            set
            {
                if (_provision == value)
                    return;
                _provision = value;

                // Try to parse
                if (!decimal.TryParse(_provision, out _provisionDec))
                    _provisionDec = 0;
            }
        }

        public decimal ProvisionDec
        {
            get => _provisionDec;
            set
            {
                if (_provisionDec == value)
                    return;
                _provisionDec = value;

                Provision = _provisionDec.ToString("G");
            }
        }

        public string BrokerFee
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the broker free is greater than '0'
                    return _brokerFeeDec > 0 ? Helper.FormatDecimal(_brokerFeeDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _brokerFee;
            }
            set
            {
                if (_brokerFee == value)
                    return;
                _brokerFee = value;

                // Try to parse
                if (!decimal.TryParse(_brokerFee, out _brokerFeeDec))
                    _brokerFeeDec = 0;
            }
        }

        public decimal BrokerFeeDec
        {
            get => _brokerFeeDec;
            set
            {
                if (_brokerFeeDec == value)
                    return;
                _brokerFeeDec = value;

                BrokerFee = _brokerFeeDec.ToString("G");
            }
        }

        public string TraderPlaceFee
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the trader place fee is greater than '0'
                    return _traderPlaceFeeDec > 0 ? Helper.FormatDecimal(_traderPlaceFeeDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength) : @"";
                }

                return _traderPlaceFee;
            }
            set
            {
                if (_traderPlaceFee == value)
                    return;
                _traderPlaceFee = value;

                // Try to parse
                if (!decimal.TryParse(_traderPlaceFee, out _traderPlaceFeeDec))
                    _traderPlaceFeeDec = 0;
            }
        }

        public decimal TraderPlaceFeeDec
        {
            get => _traderPlaceFeeDec;
            set
            {
                if (_traderPlaceFeeDec == value)
                    return;
                _traderPlaceFeeDec = value;

                TraderPlaceFee = _traderPlaceFeeDec.ToString("G");
            }
        }

        public string Reduction
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the brokerage is greater than '0'
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

                Brokerage = _reductionDec.ToString("G");
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
