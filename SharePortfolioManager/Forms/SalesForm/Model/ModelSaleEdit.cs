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
using SharePortfolioManager.Forms.SalesForm.View;
using System.Collections.Generic;

namespace SharePortfolioManager.Forms.SalesForm.Model
{
    /// <summary>
    /// Interface of the SaleEdit model
    /// </summary>
    public interface IModelSaleEdit
    {
        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<WebSiteRegex> WebSiteRegexList { get; set; }
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }
        bool UpdateSale { get; set; }
        SaleErrorCode ErrorCode { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        decimal VolumeDec { get; set; }
        string BuyPrice { get; set; }
        decimal BuyPriceDec { get; set; }
        string SalePrice { get; set; }
        decimal SalePriceDec { get; set; }
        string TaxAtSource { get; set; }
        decimal TaxAtSourceDec { get; set; }
        string CapitalGainsTax { get; set; }
        decimal CapitalGainsTaxDec { get; set; }
        string SolidarityTax { get; set; }
        decimal SolidarityTaxDec { get; set; }
        string Costs { get; set; }
        decimal CostsDec { get; set; }
        string ProfitLoss { get; set; }
        decimal ProfitLossDec { get; set; }
        string Payout { get; set; }
        decimal PayoutDec { get; set; }
        string Document { get; set; }
    }

    /// <summary>
    /// Model class of the SaleEdit
    /// </summary>
    public class ModelSaleEdit : IModelSaleEdit
    {
        #region Fields

        bool _updateView;
        bool _updateViewFormatted;
        bool _updateSale;

        ShareObjectMarketValue _shareObjectMarketValue;
        ShareObjectFinalValue _shareObjectFinalValue;
        List<WebSiteRegex> _webSiteRegexList;
        SaleErrorCode _errorCode;

        string _selectedDate;
        string _date;
        string _time;
        string _volume;
        decimal _volumeDec;
        string _buyPrice;
        decimal _buyPriceDec;
        string _salePrice;
        decimal _salePriceDec;
        string _taxAtSource;
        decimal _taxAtSourceDec;
        string _capitalGainsTax;
        decimal _capitalGainsTaxDec;
        string _solidarityTax;
        decimal _solidarityTaxDec;
        string _costs;
        decimal _costsDec;
        string _profitLoss;
        decimal _profitLossDec;
        string _payout;
        decimal _payoutDec;
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

        public bool UpdateSale
        {
            get { return _updateSale; }
            set { _updateSale = value; }
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

        public SaleErrorCode ErrorCode
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

        public string BuyPrice
        {
            get
            {
                // Only return the value if the value is greater than '0'
                if (_buyPriceDec > 0)
                    return Helper.FormatDecimal(_buyPriceDec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength);
                return @"";
            }

            set
            {
                if (_buyPrice == value)
                    return;
                _buyPrice = value;

                // Try to parse
                if (!decimal.TryParse(_buyPrice, out _buyPriceDec))
                    _buyPriceDec = 0;
            }
        }

        public decimal BuyPriceDec
        {
            get { return _buyPriceDec; }
            set
            {
                if (_buyPriceDec == value)
                    return;
                _buyPriceDec = value;

                BuyPrice = _buyPriceDec.ToString();
            }
        }

        public string SalePrice
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the sale price is greater than '0'
                    if (_salePriceDec > 0)
                        return Helper.FormatDecimal(_salePriceDec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _salePrice;
                }
            }
            set
            {
                if (_salePrice == value)
                    return;
                _salePrice = value;

                // Try to parse
                if (!decimal.TryParse(_salePrice, out _salePriceDec))
                    _salePriceDec = 0;
            }
        }

        public decimal SalePriceDec
        {
            get { return _salePriceDec; }
            set
            {
                if (_salePriceDec == value)
                    return;
                _salePriceDec = value;

                SalePrice = _salePriceDec.ToString();
            }
        }

        public string TaxAtSource
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the tax at source is greater than '0'
                    if (_taxAtSourceDec > 0)
                        return Helper.FormatDecimal(_taxAtSourceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _taxAtSource;
                }
            }
            set
            {
                if (_taxAtSource == value)
                    return;
                _taxAtSource = value;

                // Try to parse
                if (!decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;
            }
        }

        public decimal TaxAtSourceDec
        {
            get { return _taxAtSourceDec; }
            set
            {
                if (_taxAtSourceDec == value)
                    return;
                _taxAtSourceDec = value;

                SalePrice = _taxAtSourceDec.ToString();
            }
        }

        public string CapitalGainsTax
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the capital gains tax is greater than '0'
                    if (_capitalGainsTaxDec > 0)
                        return Helper.FormatDecimal(_capitalGainsTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _capitalGainsTax;
                }
            }
            set
            {
                if (_capitalGainsTax == value)
                    return;
                _capitalGainsTax = value;

                // Try to parse
                if (!decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;
            }
        }

        public decimal CapitalGainsTaxDec
        {
            get { return _capitalGainsTaxDec; }
            set
            {
                if (_capitalGainsTaxDec == value)
                    return;
                _capitalGainsTaxDec = value;

                CapitalGainsTax = _capitalGainsTaxDec.ToString();
            }
        }

        public string SolidarityTax
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the solidarity tax is greater than '0'
                    if (_solidarityTaxDec > 0)
                        return Helper.FormatDecimal(_solidarityTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _solidarityTax;
                }
            }
            set
            {
                if (_solidarityTax == value)
                    return;
                _solidarityTax = value;

                // Try to parse
                if (!decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;
            }
        }

        public decimal SolidarityTaxDec
        {
            get { return _solidarityTaxDec; }
            set
            {
                if (_solidarityTaxDec == value)
                    return;
                _solidarityTaxDec = value;

                SolidarityTax = _solidarityTaxDec.ToString();
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

        public string ProfitLoss
        {
            get
            {
                return Helper.FormatDecimal(_profitLossDec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength);
            }

            set
            {
                if (_profitLoss == value)
                    return;
                _profitLoss = value;

                // Try to parse
                if (!decimal.TryParse(_profitLoss, out _profitLossDec))
                    ProfitLossDec = 0;
            }
        }

        public decimal ProfitLossDec
        {
            get { return _profitLossDec; }
            set
            {
                if (_profitLossDec == value)
                    return;
                _profitLossDec = value;

                ProfitLoss = _profitLossDec.ToString();

                UpdateView = true;
            }
        }

        public string Payout
        {
            get
            {
               return Helper.FormatDecimal(_payoutDec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength);
            }
            set
            {
                if (_payout == value)
                    return;
                _payout = value;

                // Try to parse
                if (!decimal.TryParse(_payout, out _payoutDec))
                    _payoutDec = 0;
            }
        }

        public decimal PayoutDec
        {
            get { return _payoutDec; }
            set
            {
                if (_payoutDec == value)
                    return;
                _payoutDec = value;

                Payout = _payoutDec.ToString();

                UpdateView = true;
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

