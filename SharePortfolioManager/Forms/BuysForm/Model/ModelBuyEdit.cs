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

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Forms.BuysForm.View;
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

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        string Date { get; set; }
        string Time { get; set; }
        string OrderNumber { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string VolumeSold { get; set; }
        decimal VolumeSoldDec { get; set; }
        string SharePrice { get; set; }
        decimal SharePriceDec { get; set; }
        string MarketValue { get; set; }
        decimal MarketValueDec { get; set; }
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
        string BrokerageWithReduction { get; set; }
        decimal BrokerageWithReductionDec { get; set; }
        string Deposit { get; set; }
        decimal DepositDec { get; set; }
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
        private string _orderNumber;
        private string _volume;
        private decimal _volumeDec;
        private string _volumeSold;
        private decimal _volumeSoldDec;
        private string _sharePrice;
        private decimal _sharePriceDec;
        private string _marketValue;
        private decimal _marketValueDec;
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
        private string _brokerageWithReduction;
        private decimal _brokerageWithReductionDec;
        private string _deposit;
        private decimal _depositDec;
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

        public string Provision
        {
            get
            {
                if (!decimal.TryParse(_provision, out _provisionDec))
                    _provisionDec = 0;

                if (UpdateViewFormatted)
                    return _provisionDec > 0 ? Helper.FormatDecimal(_provisionDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _provision;

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
                    return _brokerFeeDec > 0 ? Helper.FormatDecimal(_brokerFeeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _brokerFee;

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
                    return _traderPlaceFeeDec > 0 ? Helper.FormatDecimal(_traderPlaceFeeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _traderPlaceFee;

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

        public string Brokerage
        {
            get => _brokerageDec >= 0 ? Helper.FormatDecimal(_brokerageDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _brokerage;
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

        public string BrokerageWithReduction
        {
            get => _brokerageWithReductionDec >= 0 ? Helper.FormatDecimal(_brokerageWithReductionDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _brokerageWithReduction;
            set
            {
                if (Equals(_brokerageWithReduction, value))
                    return;
                _brokerageWithReduction = value;

                if (!decimal.TryParse(_brokerageWithReduction, out _brokerageWithReductionDec))
                    _brokerageWithReductionDec = 0;
            }
        }

        public decimal BrokerageWithReductionDec
        {
            get => _brokerageWithReductionDec;
            set
            {
                if (Equals(_brokerageWithReductionDec, value))
                    return;
                _brokerageWithReductionDec = value;
            }
        }

        public string Deposit
        {
            get => _depositDec >= 0 ? Helper.FormatDecimal(_depositDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : _deposit;
            set
            {
                if (Equals(_deposit, value))
                    return;
                _deposit = value;

                if (!decimal.TryParse(_deposit, out _depositDec))
                    DepositDec = 0;
            }
        }

        public decimal DepositDec
        {
            get => _depositDec;
            set
            {
                if (Equals(_depositDec, value))
                    return;
                _depositDec = value;
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
