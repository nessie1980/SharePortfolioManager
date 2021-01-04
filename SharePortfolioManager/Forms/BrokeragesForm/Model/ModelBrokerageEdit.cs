//MIT License
//
//Copyright(c) 2017 - 2021 nessie1980(nessie1980 @gmx.de)
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
//#define DEBUG_BROKERAGE_EDIT_MODEL

using LanguageHandler;
using Logging;
using SharePortfolioManager.BrokeragesForm.View;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.BrokeragesForm.Model
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
        string BrokerageReduction { get; set; }
        decimal BrokerageReductionDec { get; set; }
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
        private string _brokerage;
        private decimal _brokerageDec;
        private string _reduction;
        private decimal _reductionDec;
        private string _brokerageReduction;
        private decimal _brokerageReductionDec;
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
                    return _provisionDec > 0 ? Helper.FormatDecimal(_provisionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : @"";
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
                if (Equals(_provisionDec, value))
                    return;
                _provisionDec = value;

                UpdateView = true;
            }
        }

        public string BrokerFee
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the broker free is greater than '0'
                    return _brokerFeeDec > 0 ? Helper.FormatDecimal(_brokerFeeDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : @"";
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
                if (Equals(_brokerFeeDec, value))
                    return;
                _brokerFeeDec = value;

                UpdateView = true;
            }
        }

        public string TraderPlaceFee
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the trader place fee is greater than '0'
                    return _traderPlaceFeeDec > 0 ? Helper.FormatDecimal(_traderPlaceFeeDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : @"";
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
                if (Equals(_traderPlaceFeeDec, value))
                    return;
                _traderPlaceFeeDec = value;

                UpdateView = true;
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

        public string Reduction
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the brokerage is greater than '0'
                    return _reductionDec > 0 ? Helper.FormatDecimal(_reductionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : @"";
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
                if (Equals(_reductionDec, value))
                    return;
                _reductionDec = value;

                UpdateView = true;
            }
        }

        public string BrokerageReduction
        {
            get => _brokerageReductionDec >= 0 ? Helper.FormatDecimal(_brokerageReductionDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _brokerageReduction;
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
