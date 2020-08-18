//MIT License
//
//Copyright(c) 2020 nessie1980(nessie1980 @gmx.de)
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
//#define DEBUG_SALE_EDIT_MODEL

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Sales;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.SalesForm.View;
using System.Collections.Generic;

namespace SharePortfolioManager.SalesForm.Model
{
    /// <summary>
    /// Interface of the SaleEdit model
    /// </summary>
    public interface IModelSaleEdit
    {
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }

        SaleErrorCode ErrorCode { get; set; }
        string SelectedGuid { get; set; }
        string SelectedGuidLast { get; set; }
        string SelectedDate { get; set; }

        bool AddSale { get; set; }
        bool UpdateSale { get; set; }
        bool ShowSalesRunning { get; set; }
        bool LoadSaleRunning { get; set; }
        bool ResetRunning { get; set; }
        bool DepotNumberChangeRunning { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        Logger Logger { get; set; }
        Language Language { get; set; }
        string LanguageName { get; set; }

        string Date { get; set; }
        string Time { get; set; }
        string DepotNumber { get; set; }
        string OrderNumber { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string SalePrice { get; set; }
        decimal SalePriceDec { get; set; }
        decimal SaleBuyValueDec { get; set; }
        string SaleBuyValue { get; set; }
        List<SaleBuyDetails> UsedBuyDetails { get; set; }
        string TaxAtSource { get; set; }
        decimal TaxAtSourceDec { get; set; }
        string CapitalGainsTax { get; set; }
        decimal CapitalGainsTaxDec { get; set; }
        string SolidarityTax { get; set; }
        decimal SolidarityTaxDec { get; set; }
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
        string ProfitLoss { get; set; }
        decimal ProfitLossDec { get; set; }
        string Payout { get; set; }
        decimal PayoutDec { get; set; }
        string Document { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Model class of the SaleEdit
    /// </summary>
    public class ModelSaleEdit : IModelSaleEdit
    {
        #region Fields

        private string _date;
        private string _time;
        private string _depotNumber;
        private string _orderNumber;
        private string _volume;
        private decimal _volumeDec;
        private string _salePrice;
        private decimal _salePriceDec;
        private string _saleBuyValue;
        private decimal _saleBuyValueDec;
        private string _taxAtSource;
        private decimal _taxAtSourceDec;
        private string _capitalGainsTax;
        private decimal _capitalGainsTaxDec;
        private string _solidarityTax;
        private decimal _solidarityTaxDec;
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
        private string _profitLoss;
        private decimal _profitLossDec;
        private string _payout;
        private decimal _payoutDec;
        private string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView { get; set; }

        public bool UpdateViewFormatted { get; set; }

        public bool UpdateSale { get; set; }

        public bool ShowSalesRunning { get; set; }

        public bool LoadSaleRunning { get; set; }

        public bool ResetRunning { get; set; }

        public bool DepotNumberChangeRunning { get; set; }

        public bool AddSale { get; set; }

        public SaleErrorCode ErrorCode { get; set; }

        public string SelectedGuid { set; get; }

        public string SelectedGuidLast { set; get; }

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
                if (_orderNumber != null && _orderNumber == value)
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
                    return _volumeDec > 0 ? Helper.FormatDecimal(_volumeDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _volume;

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

        public string SalePrice
        {
            get
            {
                if (!decimal.TryParse(_salePrice, out _salePriceDec))
                    _salePriceDec = 0;

                if (UpdateViewFormatted)
                {
                    return _salePriceDec > 0 ? Helper.FormatDecimal(_salePriceDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength) : _salePrice;
                }

                return _salePrice;
            }
            set
            {
                if (Equals(_salePrice, value))
                    return;
                _salePrice = value;

                if (!decimal.TryParse(_salePrice, out _salePriceDec))
                    _salePriceDec = 0;
            }
        }

        public decimal SalePriceDec
        {
            get => _salePriceDec;
            set
            {
                if (_salePriceDec == value)
                    return;
                _salePriceDec = value;

                SalePrice = _salePriceDec.ToString("G");
            }
        }

        public string SaleBuyValue
        {
            get => Helper.FormatDecimal(_saleBuyValueDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);
            set
            {
                if (Equals(_saleBuyValue, value))
                    return;
                _saleBuyValue = value;

                if (!decimal.TryParse(_saleBuyValue, out _saleBuyValueDec))
                    _saleBuyValueDec = 0;
            }
        }

        public decimal SaleBuyValueDec
        {
            get => _saleBuyValueDec;
            set
            {
                if (Equals(_saleBuyValueDec, value))
                    return;
                _saleBuyValueDec = value;

                SaleBuyValue = _saleBuyValueDec.ToString("G");
            }
        }

        public string ProfitLoss
        {
            get => Helper.FormatDecimal(_profitLossDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);
            set
            {
                if (Equals(_profitLoss, value))
                    return;
                _profitLoss = value;

                if (!decimal.TryParse(_profitLoss, out _profitLossDec))
                    ProfitLossDec = 0;
            }
        }

        public decimal ProfitLossDec
        {
            get => _profitLossDec;
            set
            {
                if (Equals(_profitLossDec, value))
                    return;
                _profitLossDec = value;

                ProfitLoss = _profitLossDec.ToString("G");
            }
        }

        public List<SaleBuyDetails> UsedBuyDetails { get; set; }

        public string TaxAtSource
        {
            get
            {
                if (!decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;

                if (UpdateViewFormatted)
                    return _taxAtSourceDec > 0 ? Helper.FormatDecimal(_taxAtSourceDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _taxAtSource;

                return _taxAtSource;
            }
            set
            {
                if (Equals(_taxAtSource, value))
                    return;
                _taxAtSource = value;

                if (!decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;
            }
        }

        public decimal TaxAtSourceDec
        {
            get => _taxAtSourceDec;
            set
            {
                if (Equals(_taxAtSourceDec, value))
                    return;
                _taxAtSourceDec = value;

                SalePrice = _taxAtSourceDec.ToString("G");
            }
        }

        public string CapitalGainsTax
        {
            get
            {
                if (!decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;

                if (UpdateViewFormatted)
                    return _capitalGainsTaxDec > 0 ? Helper.FormatDecimal(_capitalGainsTaxDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _capitalGainsTax;

                return _capitalGainsTax;
            }
            set
            {
                if (Equals(_capitalGainsTax, value))
                    return;
                _capitalGainsTax = value;

                if (!decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;
            }
        }

        public decimal CapitalGainsTaxDec
        {
            get => _capitalGainsTaxDec;
            set
            {
                if (Equals(_capitalGainsTaxDec, value))
                    return;
                _capitalGainsTaxDec = value;

                CapitalGainsTax = _capitalGainsTaxDec.ToString("G");
            }
        }

        public string SolidarityTax
        {
            get
            {
                if (!decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;

                if (UpdateViewFormatted)
                    return _solidarityTaxDec > 0 ? Helper.FormatDecimal(_solidarityTaxDec, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength) : _solidarityTax;

                return _solidarityTax;
            }
            set
            {
                if (Equals(_solidarityTax, value))
                    return;
                _solidarityTax = value;

                if (!decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;
            }
        }

        public decimal SolidarityTaxDec
        {
            get => _solidarityTaxDec;
            set
            {
                if (Equals(_solidarityTaxDec, value))
                    return;
                _solidarityTaxDec = value;

                SolidarityTax = _solidarityTaxDec.ToString("G");
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

                Provision = _provisionDec.ToString("G");
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

                BrokerFee = _brokerFeeDec.ToString("G");
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

                TraderPlaceFee = _traderPlaceFeeDec.ToString("G");
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

                Reduction = _reductionDec.ToString("G");
            }
        }

        public string Brokerage
        {
            get => Helper.FormatDecimal(_brokerageDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);
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

                Brokerage = _brokerageDec.ToString("G");
            }
        }

        public string Payout
        {
            get => Helper.FormatDecimal(_payoutDec, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);
            set
            {
                if (Equals(_payout, value))
                    return;
                _payout = value;

                if (!decimal.TryParse(_payout, out _payoutDec))
                    _payoutDec = 0;
            }
        }

        public decimal PayoutDec
        {
            get => _payoutDec;
            set
            {
                if (Equals(_payoutDec, value))
                    return;
                _payoutDec = value;

                Payout = _payoutDec.ToString("G");
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

