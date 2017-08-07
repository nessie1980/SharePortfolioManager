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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WebParser;

namespace SharePortfolioManager
{
    public class ShareObject
    {
        #region Variables

        #region General share variables

        /// <summary>
        /// Stores the count of the share object tags in the XML
        /// </summary>
        private const short _ShareObjectTagCount = 12;

        /// <summary>
        /// Stores the WKN of a share
        /// </summary>
        private string _wkn;

        /// <summary>
        /// Stores the add date and time of a share
        /// </summary>
        private string _addDateTime;

        /// <summary>
        /// Stores the name of the share
        /// </summary>
        private string _name;

        /// <summary>
        /// Stores the date of the last update from the Internet
        /// </summary>
        private DateTime _lastUpdateInternet;

        /// <summary>
        /// Stores the date of the last update of the share
        /// </summary>
        private DateTime _lastUpdateDate;

        /// <summary>
        /// Stores the time of the last update of the share
        /// </summary>
        private DateTime _lastUpdateTime;

        /// <summary>
        /// Stores the current price of one share
        /// </summary>
        private decimal _curPrice = decimal.MinValue / 2;

        /// <summary>
        /// Stores the previous day price of one share
        /// </summary>
        private decimal _prevDayPrice = decimal.MinValue / 2;

        /// <summary>
        /// Stores the volume of the shares
        /// </summary>
        private decimal _volume = decimal.MinValue / 2;

        /// <summary>
        /// Stores the profit or loss to the previous day of a share
        /// </summary>
        private decimal _prevDayProfitLoss = decimal.MinValue / 2;

        /// <summary>
        /// Stores the performance of the share without dividends, costs, profits and loss (market value performance)
        /// </summary>
        private decimal _performanceMarketValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the performance of the share with dividends, costs, profits and loss (final value performance)
        /// </summary>
        private decimal _performanceFinalValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the performance of the share to the previous day
        /// </summary>
        private decimal _prevDayPerformance = decimal.MinValue / 2;

        /// <summary>
        /// Stores the difference of the share to the previous day
        /// </summary>
        private decimal _prevDayDifference = decimal.MinValue / 2;

        /// <summary>
        /// Stores the image for the previous day performance visualization
        /// </summary>
        private Image _imagePrevDayPerformance = null;

        /// <summary>
        /// Stores a list of images for the previous day performance visualization
        /// </summary>
        private List<Image> _imageListPrevDayPerformance = null;

        /// <summary>
        /// Stores the total share value without dividends, costs, profits and loss (market value)
        /// </summary>
        private decimal _marketValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the share value with dividends, costs, profits and loss (final value)
        /// </summary>
        private decimal _finalValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the profit or loss of the share without dividends, costs, profits and loss (market value)
        /// </summary>
        private decimal _profitLossMarketValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the profit or loss of the share with dividends, costs, profits and loss (final value)
        /// </summary>
        private decimal _profitLossFinalValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the deposit value of all buys and sales
        /// </summary>
        private decimal _deposit = decimal.MinValue / 2;

        /// <summary>
        /// Stores the website link of the share
        /// </summary>
        private string _webSite;

        /// <summary>
        /// Stores the encoding type for the content
        /// </summary>
        private string _encodingType = Encoding.Default.ToString();

        /// <summary>
        /// Stores the RegEx object for the share
        /// </summary>
        private RegExList _regexList;

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the currency unit of the share
        /// </summary>
        private string _currencyUnit;

        #endregion General share variables

        #region Buy share variables

        /// <summary>
        /// Stores the buys of the share
        /// </summary>
        private AllBuysOfTheShare _allBuyEntries = new AllBuysOfTheShare();

        /// <summary>
        /// Stores the total buy value of the share
        /// </summary>
        private decimal _buyValueTotal = 0;

        /// <summary>
        /// Stores the XML tag name prefix of a buy entry
        /// </summary>
        private const string _buyTagNamePre = "Buy";

        /// <summary>
        /// Stores the XML attribute name for the buy date
        /// </summary>
        private const string _buyDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the volume of a buy
        /// </summary>
        private const string _buyVolumeAttrName = "Volume";

        /// <summary>
        /// Stores the XML attribute name for the reduction of a buy
        /// </summary>
        private const string _buyReductionAttrName = "Reduction";

        /// <summary>
        /// Stores the XML attribute name for buy price of a share of a buy
        /// </summary>
        private const string _buyPriceAttrName = "Price";

        /// <summary>
        /// Stores the XML attribute name for the document of a buy
        /// </summary>
        private const string _buyDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the dividend
        /// </summary>
        private const short _buyAttrCount = 5;

        #endregion Buy share variables

        #region Sale share variables

        /// <summary>
        /// Stores the sales of the share
        /// </summary>
        private AllSalesOfTheShare _allSaleEntries = new AllSalesOfTheShare();

        /// <summary>
        /// Stores the total sale value of the share
        /// </summary>
        private decimal _saleValueTotal = 0;

        /// <summary>
        /// Stores the XML tag name prefix of a sale entry
        /// </summary>
        private const string _saleTagNamePre = "Sale";

        /// <summary>
        /// Stores the XML attribute name for the sale date
        /// </summary>
        private const string _saleDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the volume of a sale
        /// </summary>
        private const string _saleVolumeAttrName = "Volume";

        /// <summary>
        /// Stores the XML attribute name for price of one share of a sale
        /// </summary>
        private const string _salePriceAttrName = "Price";

        /// <summary>
        /// Stores the XML attribute name for profit or loss of a sale
        /// </summary>
        private const string _saleProfitLossAttrName = "ProfitLoss";

        /// <summary>
        /// Stores the XML attribute name for the document of a sale
        /// </summary>
        private const string _saleDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the dividend
        /// </summary>
        private const short _saleAttrCount = 5;

        #endregion Sale share variables

        #region Costs share variables

        /// <summary>
        /// Stores the cost pays of the share
        /// </summary>
        private AllCostsOfTheShare _allCostsEntries = new AllCostsOfTheShare();

        /// <summary>
        /// Stores the total costs of the share
        /// </summary>
        private decimal _costsValueTotal = 0;

        /// <summary>
        /// Stores the tag name prefix of a cost entry
        /// </summary>
        private const string _costsTagNamePre = "Cost";

        /// <summary>
        /// Stores the attribute name for the flag if the cost is a part of a buy
        /// </summary>
        private const string _costsBuyPartAttrName = "BuyPart";

        /// <summary>
        /// Stores the attribute name for the date 
        /// </summary>
        private const string _costsDateAttrName = "Date";

        /// <summary>
        /// Stores the attribute name for the value
        /// </summary>
        private const string _costsValueAttrName = "Value";

        /// <summary>
        /// Stores the XML attribute name for the document of a cost
        /// </summary>
        private const string _costsDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the costs
        /// </summary>
        private const short _costsAttrCount = 4;

        #endregion Costs share variables

        #region Dividends share variables

        /// <summary>
        /// Stores the payout interval of the dividend
        /// </summary>
        private int _dividendPayoutInterval;

        /// <summary>
        /// Stores the dividend payouts of the share
        /// </summary>
        private AllDividendsOfTheShare _allDividendEntries = new AllDividendsOfTheShare();

        /// <summary>
        /// Stores the total dividend value of the share
        /// </summary>
        private decimal _dividendValueTotal = 0;

        /// <summary>
        /// Stores the total dividend of the share minus the taxes
        /// </summary>
        private decimal _dividendValueTotalWithTaxes = 0;

        /// <summary>
        /// Stores the XML tag name of a dividend entry
        /// </summary>
        private const string _dividendTagName = "Dividend";

        /// <summary>
        /// Stores the XML attribute name for the dividend payout interval
        /// </summary>
        private const string _dividendPayoutIntervalAttrName = "PayoutInterval";

        /// <summary>
        /// Stores the XML attribute name for the dividend pay date
        /// </summary>
        private const string _dividendDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the dividend pay for one share
        /// </summary>
        private const string _dividendRateAttrName = "Rate";

        /// <summary>
        /// Stores the XML attribute name for the share volume at the pay date
        /// </summary>
        private const string _dividendVolumeAttrName = "Volume";

        /// <summary>
        /// Stores the XML attribute name for the dividend tax at source value
        /// </summary>
        private const string _dividendTaxAtSourceAttrName = "TaxAtSource";

        /// <summary>
        /// Stores the XML attribute name for the dividend capital gains tax value
        /// </summary>
        private const string _dividendCapitalGainsTaxAttrName = "CapitalGainsTax";

        /// <summary>
        /// Stores the XML attribute name for the dividend capital gains tax value
        /// </summary>
        private const string _dividendSolidarityTaxAttrName = "SolidarityTax";

        /// <summary>
        /// Stores the XML attribute name for the share price at the pay date
        /// </summary>
        private const string _dividendPriceAttrName = "Price";

        /// <summary>
        /// Stores the XML attribute name for the document of a dividend
        /// </summary>
        private const string _dividendDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the XML tag name of the foreign currency information
        /// </summary>
        private const string _dividendTagNameForeignCu = "ForeignCurrency";

        /// <summary>
        /// Stores the XML attribute name for the flag if the dividend is paid in a foreign currency
        /// </summary>
        private const string _dividendForeignCuFlagAttrName = "Flag";

        /// <summary>
        /// Stores the XML attribute name for the factor of the foreign currency
        /// </summary>
        private const string _dividendExchangeRatioAttrName = "ExchangeRatio";

        /// <summary>
        /// Stores the XML attribute name for the name of the foreign currency
        /// </summary>
        private const string _dividendNameAttrName = "FCName";

        /// <summary>
        /// Stores the attribute count for the foreign currency information
        /// </summary>
        private const short _dividendAttrCountForeignCu = 3;

        /// <summary>
        /// Stores the attribute count for the dividend
        /// </summary>
        private const short _dividendAttrCount = 9;

        /// <summary>
        /// Stores the child node count for the dividend
        /// </summary>
        private const short _dividendChildNodeCount = 1;

        #endregion Dividends share variables

        #region Value units

        /// <summary>
        /// Stores the value unit for percentage values
        /// </summary>
        static private string _percentageUnit = @"%";

        /// <summary>
        /// Stores the value unit for piece values
        /// </summary>
        static private string _pieceUnit = @"stk.";

        #endregion Value units

        #region Portfolio value variables

        /// <summary>
        /// Stores the deposit of the portfolio (all shares)
        /// </summary>
        static private decimal _portfolioDeposit = 0;

        /// <summary>
        /// Stores the costs of the portfolio (all shares)
        /// </summary>
        static private decimal _portfolioCosts = 0;

        /// <summary>
        /// Stores the dividends of the portfolio (all shares)
        /// </summary>
        static private decimal _portfolioDividend = 0;

        /// <summary>
        /// Stores the performance of the portfolio (all shares) without dividends, costs, profits and loss (market value)
        /// </summary>
        static private decimal _portfolioPerformanceMarketValue = 0;

        /// <summary>
        /// Stores the performance of the portfolio (all shares) with dividends, costs, profits and loss (final value)
        /// </summary>
        static private decimal _portfolioPerformanceFinalValue = 0;

        /// <summary>
        /// Stores the profit or lose of the portfolio (all shares) without dividends, costs, profits and loss (market value)
        /// </summary>
        static private decimal _portfolioProfitLossMarketValue = 0;

        /// <summary>
        /// Stores the profit or lose of the portfolio (all shares) with dividends, costs, profits and loss (final value)
        /// </summary>
        static private decimal _portfolioProfitLossFinalValue = 0;

        /// <summary>
        /// Stores the value of the portfolio (all shares) without dividends, costs, profits and loss (market value)
        /// </summary>
        static private decimal _portfolioMarketValue = 0;

        /// <summary>
        /// Stores the value of the portfolio (all shares) with dividends, costs, profits and loss (final value)
        /// </summary>
        static private decimal _portfolioFinalValue = 0;

        #endregion Portfolio value variables

        #endregion Variables

        #region Properties

        #region Value units

        [Browsable(false)]
        public static string PercentageUnit
        {
            set { _percentageUnit = value; }
            get { return _percentageUnit; }
        }

        [Browsable(false)]
        public static string PieceUnit
        {
            set { _pieceUnit = value; }
            get { return _pieceUnit; }
        }

        #endregion Value units

        #region General share properties

        [Browsable(false)]
        public short ShareObjectTagCount
        {
            get { return _ShareObjectTagCount; }
        }

        [Browsable(false)]
        public string Wkn
        {
            get { return _wkn; }
            set { _wkn = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Wkn")]
        public string WknAsStr
        {
            get { return _wkn; }
        }

        [Browsable(false)]
        public string AddDateTime
        {
            get { return _addDateTime; }
            set { _addDateTime = value; }
        }

        [Browsable(false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Name")]
        public string NameAsStr
        {
            get { return _name; }
        }

        [Browsable(false)]
        public DateTime LastUpdateInternet
        {
            get { return _lastUpdateInternet; }
            set { _lastUpdateInternet = value; }
        }

        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get { return _lastUpdateDate; }
            set { _lastUpdateDate = value; }
        }

        [Browsable(false)]
        public DateTime LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }

        [Browsable(false)]
        public decimal Volume
        {
            get { return _volume; }
            set
            {
                // Set the new share volume
                if (value != _volume)
                {
                    _volume = value;

                    // Recalculate the total sum of the share
                    CalculateTotalShareValues();

                    // Recalculate the appreciation
                    CalculateTotalPerformancesOfShare();

                    // Recalculate the profit or lose to the previous day
                    CalculateDayBeforeProfitLoseOfShare();

                    // Recalculate the profit or lose of the share
                    CalculateProfitLoseShareValues();
                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string VolumeAsStr
        {
            get
            {
                return Helper.FormatDecimal(_volume, Helper.Volumefivelength, true, Helper.Volumenonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string VolumeAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(_volume, Helper.Volumefivelength, true, Helper.Volumenonefixlength, true, PieceUnit, CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal BuyValueTotal
        {
            get { return _buyValueTotal; }
            internal set
            {
                _buyValueTotal = value;
            }
        }

        [Browsable(false)]
        public string BuyValueTotalAsStr
        {
            get
            {
                return Helper.FormatDecimal(BuyValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string BuyValueTotalAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(BuyValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal SaleValueTotal
        {
            get { return _saleValueTotal; }
            internal set
            {
                _saleValueTotal = value;
            }
        }

        [Browsable(false)]
        public string SaleValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(SaleValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string SaleValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(SaleValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal CostsValueTotal
        {
            get { return _costsValueTotal; }
            internal set
            {
                _costsValueTotal = value;
            }
        }

        [Browsable(false)]
        public string CostsValueTotalAsStr
        {
            get
            {
                return Helper.FormatDecimal(CostsValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string CostsValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(CostsValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal DividendValueTotal
        {
            get { return _dividendValueTotal; }
            internal set
            {
                _dividendValueTotal = value;
            }
        }

        [Browsable(false)]
        public string DividendValueTotalAsStr
        {
            get
            {
                return Helper.FormatDecimal(DividendValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string DividendValueTotalAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(DividendValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal DividendValueTotalWithTaxes
        {
            get { return _dividendValueTotalWithTaxes; }
            internal set
            {
                _dividendValueTotalWithTaxes = value;
            }
        }

        [Browsable(false)]
        public string DividendValueTotalWithTaxesAsStr
        {
            get
            {
                return Helper.FormatDecimal(DividendValueTotalWithTaxes, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string DividendValueTotalWithTaxesAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(DividendValueTotalWithTaxes, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(true)]
        [DisplayName(@"CostsDividendWithTaxes")]
        public string CostsDividendWithTaxesAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CostsValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(DividendValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public decimal CurPrice
        {
            get { return _curPrice; }
            set
            {
                // Set the new price
                if (value != _curPrice)
                {
                    _curPrice = value;

                    // Recalculate the total sum of the share
                    CalculateTotalShareValues();

                    // Recalculate the appreciation
                    CalculateTotalPerformancesOfShare();

                    // Recalculate the day before appreciation
                    CalculateDayBeforePerformanceOfShare();

                    // Recalculate the profit or lose to the previous day
                    CalculateDayBeforeProfitLoseOfShare();

                    // Recalculate the profit or lose of the share
                    CalculateProfitLoseShareValues();
                }
            }
        }

        [Browsable(false)]
        public string CurPriceAsStr
        {
            get
            {
                return Helper.FormatDecimal(CurPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string CurPriceAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(CurPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PrevDayPrice
        {
            get { return _prevDayPrice; }
            set
            {
                // Set new price day before
                if (value != _prevDayPrice)
                {
                    _prevDayPrice = value;

                    // Recalculate the day before appreciation
                    CalculateDayBeforePerformanceOfShare();

                    // Recalculate the profit or lose to the previous day
                    CalculateDayBeforeProfitLoseOfShare();
                }
            }
        }

        [Browsable(false)]
        public string PrevDayPriceAsStr
        {
            get
            {
                return Helper.FormatDecimal(PrevDayPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PrevDayPriceAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PrevDayPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(true)]
        [DisplayName(@"CurPrevPrice")]
        public string CurPrevPriceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CurPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PrevDayPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public decimal PerformanceFinalValue
        {
            get { return _performanceFinalValue; }
            internal set { _performanceFinalValue = value; }
        }

        [Browsable(false)]
        public string PerformanceFinalValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PerformanceFinalValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PerformanceFinalValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PerformanceFinalValue, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PerformanceMarketValue
        {
            get { return _performanceMarketValue; }
            internal set { _performanceMarketValue = value; }
        }

        [Browsable(false)]
        public string PerformanceMarketValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PerformanceMarketValue, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PerformanceMarketValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PerformanceMarketValue, Helper.Percentagefourlength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PrevDayProfitLoss
        {
            get { return _prevDayProfitLoss; }
            internal set
            {
                if (_prevDayProfitLoss != value)
                    _prevDayProfitLoss = value;
            }
        }

        [Browsable(false)]
        public string PrevDayProfitLossAsStr
        {
            get
            {
                return Helper.FormatDecimal(PrevDayProfitLoss, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PrevDayProfitLossAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PrevDayProfitLoss, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PrevDayPerformance
        {
            get { return _prevDayPerformance; }
            internal set
            {
                _prevDayPerformance = value;
            }
        }

        [Browsable(false)]
        public string PrevDayPerformanceAsStr
        {
            get
            {
                return Helper.FormatDecimal(PrevDayPerformance, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PrevDayPerformanceAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PrevDayPerformance, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PrevDayDifference
        {
            get { return _prevDayDifference; }
            internal set
            {
                _prevDayDifference = value;
            }
        }

        [Browsable(false)]
        public string PrevDayDifferenceAsStr
        {
            get
            {
                return Helper.FormatDecimal(PrevDayDifference, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PrevDayDifferenceAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PrevDayDifference, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(true)]
        [DisplayName(@"PrevDayPerformance")]
        public string PrevDayDifferencePerformanceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PrevDayDifference, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PrevDayPerformance, Helper.Percentagefourlength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"")]
        public Image ImagePrevDayPerformance
        {
            get { return _imagePrevDayPerformance; }
            internal set { _imagePrevDayPerformance = value; }
        }

        [Browsable(false)]
        public List<Image> ImageListPrevDayPerformance
        {
            get { return _imageListPrevDayPerformance; }
            set
            {
                if (_imageListPrevDayPerformance == null)
                {
                    _imageListPrevDayPerformance = new List<Image>();
                }
                else
                {
                    _imageListPrevDayPerformance.Clear();
                }

                _imageListPrevDayPerformance = value;
            }
        }

        [Browsable(false)]
        public decimal ProfitLossFinalValue
        {
            get { return _profitLossFinalValue; }
            internal set
            {
                _profitLossFinalValue = value;
            }
        }

        [Browsable(false)]
        public string ProfitLossFinalValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(ProfitLossFinalValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string ProfitLossFinalValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(ProfitLossFinalValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(true)]
        [DisplayName(@"ProfitLossPerformanceFinalValue")]
        public string ProfitLossPerformanceFinalValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(ProfitLossFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PerformanceFinalValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public decimal FinalValue
        {
            get { return _finalValue; }
            set
            {
                if (value != _finalValue)
                {
#if DEBUG_SHAREOBJECT
                    Console.WriteLine(@"");
                    Console.WriteLine(@"_finalValue: {0}", _finalValue);
                    Console.WriteLine(@"ValuePortfolio: {0}", ValuePortfolioWithDividendsCostsProfitsLoss);
#endif
                    // Recalculate the total sum of all shares
                    // by subtracting the old total share value and then add the new value
                    if (_finalValue > decimal.MinValue / 2)
                        PortfolioFinalValue -= _finalValue;
                    PortfolioFinalValue += value;

                    // Set the total share volume
                    _finalValue = value;

#if DEBUG_SHAREOBJECT
                    Console.WriteLine(@"_valueWithDividendsCosts: {0}", _valueWithDividendsCostsProfitsLoss);
                    Console.WriteLine(@"ValuePortfolio: {0}", ValuePortfolioWithDividendsCostsProfitsLoss);
#endif
                }
            }
        }

        [Browsable(false)]
        public string FinalValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(FinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string FinalValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(FinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(true)]
        [DisplayName(@"DepositFinalValue")]
        public string DepositFinalValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(Deposit, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(FinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public decimal ProfitLossMarketValue
        {
            get { return _profitLossMarketValue; }
            internal set
            {
                _profitLossMarketValue = value;
            }
        }

        [Browsable(false)]
        public string ProfitLossMarketValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(ProfitLossMarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string ProfitLossMarketValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(ProfitLossMarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal MarketValue
        {
            get { return _marketValue; }
            set
            {
                if (value != _marketValue)
                {
                    // Set the total share volume
                    _marketValue = value;
                }
            }
        }

        [Browsable(false)]
        public string MarketValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(MarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string MarketValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(MarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal Deposit
        {
            get { return _deposit; }
            set
            {
                if (value != _deposit)
                {
                    // Recalculate the total sum of all shares
                    if (_deposit > 0)
                    {
                        PortfolioDeposit -= _deposit;
                        Console.WriteLine("DepositPortfolio (-1): {0}", PortfolioDeposit);
                    }

                    PortfolioDeposit += value;
                    Console.WriteLine("DepositPortfolio ({0}): {1}", value, PortfolioDeposit);

                    // Set new deposit value
                    _deposit = value;

                    // Recalculate the appreciation
                    CalculateTotalPerformancesOfShare();

                    // Recalculate the profit or lose of the share
                    CalculateProfitLoseShareValues();
                }
            }
        }

        [Browsable(false)]
        public string DepositAsStr
        {
            get
            {
                return Helper.FormatDecimal(Deposit, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string DepositAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(Deposit, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string WebSite
        {
            get { return _webSite; }
            set
            {
                // Check if "http://" is in front of the website
                if (value.Substring(0, 7) != "http://" && value.Substring(0, 8) != "https://")
                    value = "http://" + value;
                _webSite = value;
            }
        }

        [Browsable(false)]
        public string EncodingType
        {
            get { return _encodingType; }
            set { _encodingType = value; }
        }

        [Browsable(false)]
        public RegExList RegexList
        {
            get { return _regexList; }
            set { _regexList = value; }
        }

        [Browsable(false)]
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set
            {
                _cultureInfo = value;

                // Set culture info to the lists
                if (AllBuyEntries != null)
                    AllBuyEntries.SetCultureInfo(_cultureInfo);
                if (AllSaleEntries != null)
                    AllSaleEntries.SetCultureInfo(_cultureInfo);
                if (AllCostsEntries != null)
                    AllCostsEntries.SetCultureInfo(_cultureInfo);
                if (AllDividendEntries != null)
                    AllDividendEntries.SetCultureInfo(_cultureInfo);

                Thread.CurrentThread.CurrentCulture = CultureInfo;
                _currencyUnit = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
            }
        }

        [Browsable(false)]
        public string CultureInfoAsStr
        {
            get { return CultureInfo.ToString(); }
        }

        [Browsable(false)]
        public string CurrencyUnit
        {
            get { return _currencyUnit; }
        }

        #endregion General share properites

        #region Buy share properties

        [Browsable(false)]
        public AllBuysOfTheShare AllBuyEntries
        {
            get { return _allBuyEntries; }
            set { _allBuyEntries = value; }
        }

        [Browsable(false)]
        public string BuyTagNamePre
        {
            get { return _buyTagNamePre; }
        }

        [Browsable(false)]
        public string BuyDateAttrName
        {
            get { return _buyDateAttrName; }
        }

        [Browsable(false)]
        public string BuyVolumeAttrName
        {
            get { return _buyVolumeAttrName; }
        }

        [Browsable(false)]
        public string BuyPriceAttrName
        {
            get { return _buyPriceAttrName; }
        }

        [Browsable(false)]
        public string BuyReductionAttrName
        {
            get { return _buyReductionAttrName; }
        }

        [Browsable(false)]
        public string BuyDocumentAttrName
        {
            get { return _buyDocumentAttrName; }
        }

        [Browsable(false)]
        public short BuyAttrCount
        {
            get { return _buyAttrCount; }
        }

        #endregion Buy share properties

        #region Sale share properties

        [Browsable(false)]
        public AllSalesOfTheShare AllSaleEntries
        {
            get { return _allSaleEntries; }
            set { _allSaleEntries = value; }
        }

        [Browsable(false)]
        public string SaleTagNamePre
        {
            get { return _saleTagNamePre; }
        }

        [Browsable(false)]
        public string SaleDateAttrName
        {
            get { return _saleDateAttrName; }
        }

        [Browsable(false)]
        public string SaleVolumeAttrName
        {
            get { return _saleVolumeAttrName; }
        }

        [Browsable(false)]
        public string SalePriceAttrName
        {
            get { return _salePriceAttrName; }
        }

        [Browsable(false)]
        public string SaleProfitLossAttrName
        {
            get { return _saleProfitLossAttrName; }
        }

        [Browsable(false)]
        public string SaleDocumentAttrName
        {
            get { return _saleDocumentAttrName; }
        }

        [Browsable(false)]
        public short SaleAttrCount
        {
            get { return _saleAttrCount; }
        }

        #endregion Sale share properties

        #region Costs share properties

        [Browsable(false)]
        public AllCostsOfTheShare AllCostsEntries
        {
            get { return _allCostsEntries; }
            set { _allCostsEntries = value; }
        }

        [Browsable(false)]
        public string CostsTagNamePre
        {
            get { return _costsTagNamePre; }
        }

        [Browsable(false)]
        public string CostsBuyPartAttrName
        {
            get { return _costsBuyPartAttrName; }
        }

        [Browsable(false)]
        public string CostsDateAttrName
        {
            get { return _costsDateAttrName; }
        }

        [Browsable(false)]
        public string CostsValueAttrName
        {
            get { return _costsValueAttrName; }
        }

        [Browsable(false)]
        public string CostsDocumentAttrName
        {
            get { return _costsDocumentAttrName; }
        }

        [Browsable(false)]
        public short CostsAttrCount
        {
            get { return _costsAttrCount; }
        }

        #endregion Costs share properties

        #region Dividends share properties

        [Browsable(false)]
        public int DividendPayoutInterval
        {
            get { return _dividendPayoutInterval; }
            set { _dividendPayoutInterval = value; }
        }

        [Browsable(false)]
        public string DividendPayoutIntervalAsStr
        {
            get { return DividendPayoutInterval.ToString(); }
        }

        [Browsable(false)]
        public AllDividendsOfTheShare AllDividendEntries
        {
            get { return _allDividendEntries; }
            set { _allDividendEntries = value; }
        }

        [Browsable(false)]
        public string DividendTagName
        {
            get { return _dividendTagName; }
        }

        [Browsable(false)]
        public string DividendPayoutIntervalAttrName
        {
            get { return _dividendPayoutIntervalAttrName; }
        }

        [Browsable(false)]
        public string DividendDateAttrName
        {
            get { return _dividendDateAttrName; }
        }

        [Browsable(false)]
        public string DividendRateAttrName
        {
            get { return _dividendRateAttrName; }
        }

        [Browsable(false)]
        public string DividendVolumeAttrName
        {
            get { return _dividendVolumeAttrName; }
        }

        [Browsable(false)]
        public string DividendTaxAtSourceAttrName
        {
            get { return _dividendTaxAtSourceAttrName; }
        }

        [Browsable(false)]
        public string DividendCapitalGainsTaxAttrName
        {
            get { return _dividendCapitalGainsTaxAttrName; }
        }

        [Browsable(false)]
        public string DividendSolidarityTaxAttrName
        {
            get { return _dividendSolidarityTaxAttrName; }
        }

        [Browsable(false)]
        public string DividendPriceAttrName
        {
            get { return _dividendPriceAttrName; }
        }

        [Browsable(false)]
        public string DividendDocumentAttrName
        {
            get { return _dividendDocumentAttrName; }
        }

        [Browsable(false)]
        public string DividendTagNameForeignCu
        {
            get { return _dividendTagNameForeignCu; }
        }

        [Browsable(false)]
        public string DividendForeignCuFlagAttrName
        {
            get { return _dividendForeignCuFlagAttrName; }
        }

        [Browsable(false)]
        public string DividendNameAttrName
        {
            get { return _dividendNameAttrName; }
        }

        [Browsable(false)]
        public string DividendExchangeRatioAttrName
        {
            get { return _dividendExchangeRatioAttrName; }
        }

        [Browsable(false)]
        public short DividendAttrCountForeignCu
        {
            get { return _dividendAttrCountForeignCu; }
        }

        [Browsable(false)]
        public short DividendAttrCount
        {
            get { return _dividendAttrCount; }
        }

        [Browsable(false)]
        public short DividendChildNodeCount
        {
            get { return _dividendChildNodeCount; }
        }

        #endregion Dividends share properties

        #region Portfolio value properties

        [Browsable(false)]
        public decimal PortfolioDeposit
        {
            get { return _portfolioDeposit; }
            internal set
            {
                if (_portfolioDeposit != value)
                {
                    _portfolioDeposit = value;

                    // Recalculate the performance of all shares
                    CalculatePerformanceOfAllShares();

                    // Recalculate the profit or lose of all shares
                    CalculateProfitLoseOfAllShares();
                }
            }
        }

        [Browsable(false)]
        public string PortfolioDepositAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioDeposit, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioDepositAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(_portfolioDeposit, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }
        
        [Browsable(false)]
        public decimal PortfolioCosts
        {
            get { return _portfolioCosts; }
            internal set { _portfolioCosts = value; }
        }

        [Browsable(false)]
        public string PortfolioCostsAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioCosts, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioCostsAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioCosts, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PortfolioDividend
        {
            get { return _portfolioDividend; }
            internal set { _portfolioDividend = value; }
        }

        [Browsable(false)]
        public string PortfolioDividendAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioDividend, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioDividendAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioDividend, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PortfolioPerformanceFinalValue
        {
            get { return _portfolioPerformanceFinalValue; }
            internal set { _portfolioPerformanceFinalValue = value; }
        }

        [Browsable(false)]
        public string PortfolioPerformanceFinalValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioPerformanceFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioPerformanceFinalValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioPerformanceFinalValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PortfolioProfitLossFinalValue
        {
            get { return _portfolioProfitLossFinalValue; }
            internal set { _portfolioProfitLossFinalValue = value; }
        }

        [Browsable(false)]
        public string PortfolioProfitLossFinalValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioProfitLossFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioProfitLossFinalValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioProfitLossFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string ProfitLosePerformancePortfolioFinalValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioProfitLossFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PortfolioPerformanceFinalValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public string CostsDividendPortfolioFinalValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioCosts, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PortfolioDividend, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public decimal PortfolioFinalValue
        {
            get { return _portfolioFinalValue; }
            internal set
            {
                if (_portfolioFinalValue != value)
                {
                    _portfolioFinalValue = value;

                    // Recalculate the performance of all shares
                    CalculatePerformanceOfAllShares();

                    // Recalculate the profit or lose of all shares
                    CalculateProfitLoseOfAllShares();
                }
            }
        }

        [Browsable(false)]
        public string PortfolioFinalValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioFinalValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PortfolioMarketValue
        {
            get { return _portfolioMarketValue; }
            internal set
            {
                if (_portfolioMarketValue != value)
                {
                    _portfolioMarketValue = value;

                    // Recalculate the performance of all shares
                    CalculatePerformanceOfAllShares();

                    // Recalculate the profit or lose of all shares
                    CalculateProfitLoseOfAllShares();
                }
            }
        }

        [Browsable(false)]
        public string PortfolioMarketValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioMarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioMarketValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioMarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        #endregion Portfolio value properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        public ShareObject(List<Image> imageList, string percentageUnit, string pieceUnit)
        {
#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("ShareObject(ImageList)");
            Console.WriteLine("percentageUnit: {0}", percentageUnit);
            Console.WriteLine("pieceUnit: {0}", pieceUnit);
#endif
            _imageListPrevDayPerformance = imageList;
            _imagePrevDayPerformance = _imageListPrevDayPerformance[0];

            PercentageUnit = percentageUnit;
            PieceUnit = pieceUnit;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="wkn">WKN number of the share</param>
        /// <param name="addDateTime">Date and time of the add</param>
        /// <param name="name">Name of the share</param>
        /// <param name="lastUpdateInternet">Date and time of the last update from the Internet</param>
        /// <param name="lastUpdateShareDate">Date of the last update on the Internet site of the share</param>
        /// <param name="lastUpdateShareTime">Time of the last update on the Internet site of the share</param>
        /// <param name="price">Current price of the share</param>
        /// <param name="volume">Volume of the share</param>
        /// <param name="reduction">Reduction of the share</param>
        /// <param name="costs">Costs of the buy</param>
        /// <param name="deposit">Deposit of the share</param>
        /// <param name="webSite">Website address of the share</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
        /// <param name="dividendPayoutInterval">Interval of the dividend payout</param>
        /// <param name="document">Document of the first buy</param>
        /// <param name="taxAtSourceFlag">General flag if a tax at source must be paid</param>
        /// <param name="taxAtSourcePercentage">General value for the tax at source</param>
        /// <param name="capitalGainsTaxFlag">General flag if a capital gains tax must be paid </param>
        /// <param name="capitalGainsTaxPercentage">General value for the capital gains tax</param>
        /// <param name="solidarityTaxFlag">General flag if a solidarity tax must be paid </param>
        /// <param name="solidarityTaxPercentage">General value for the solidarity tax</param>
        public ShareObject(
            string wkn, string addDateTime, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShareDate, DateTime lastUpdateShareTime,
            decimal price, decimal volume, decimal reduction, decimal costs, decimal deposit,
            string webSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            int dividendPayoutInterval, string document)
        {
#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("ShareObject()");
            Console.WriteLine("wkn: {0}", wkn);
            Console.WriteLine("addDateTime: {0}", addDateTime);
            Console.WriteLine("name: {0}", name);
            Console.WriteLine("lastUpdateInternet: {0}", lastUpdateInternet);
            Console.WriteLine("lastUpdateShareDate: {0}", lastUpdateShareDate);
            Console.WriteLine("lastUpdateShareTime: {0}", lastUpdateShareTime);
            Console.WriteLine("price: {0}", price);
            Console.WriteLine("volume: {0}", volume);
            Console.WriteLine("costs: {0}", costs);
            Console.WriteLine("deposit: {0}", deposit);
            Console.WriteLine("webSite: {0}", webSite);
            Console.WriteLine("cultureInfo.Name: {0}", cultureInfo.Name);
            Console.WriteLine("dividendPayoutInterval: {0}", dividendPayoutInterval);
            Console.WriteLine("document: {0}", document);
            Console.WriteLine("taxAtSourceFlag: {0}", taxAtSourceFlag);
            Console.WriteLine("taxAtSourcePercentage: {0}", taxAtSourcePercentage);
            Console.WriteLine("capitalGainsTaxFlag: {0}", capitalGainsTaxFlag);
            Console.WriteLine("capitalGainsTaxPercentage: {0}", capitalGainsTaxPercentage);
            Console.WriteLine("solidarityTaxFlag: {0}", solidarityTaxFlag);
            Console.WriteLine("solidarityTaxPercentage: {0}", solidarityTaxPercentage);
#endif
            Wkn = wkn;
            AddDateTime = addDateTime;
            Name = name;
            LastUpdateInternet = lastUpdateInternet;
            LastUpdateDate = lastUpdateShareDate;
            LastUpdateTime = lastUpdateShareTime;
            CurPrice = price;

            // TODO RegEx check!

            // Check if "http://" is in front of the website
            if ((webSite.Length >= 7 && webSite.Substring(0, 7) != "http://") &&
                (webSite.Length >= 8 && webSite.Substring(0, 8) != "https://"))
                webSite = "http://" + webSite;

            WebSite = webSite;
            ImageListPrevDayPerformance = imageListForDayBeforePerformance;
            ImagePrevDayPerformance = ImageListPrevDayPerformance[0];
            RegexList = regexList;
            CultureInfo = cultureInfo;
            DividendPayoutInterval = dividendPayoutInterval;

            AddBuy(true, AddDateTime, volume, price, reduction, costs, document);
        }

        /// <summary>
        /// This function resets the portfolio values of the share objects
        /// </summary>
        static public void PortfolioValuesReset()
        {
            _portfolioDeposit = 0;
            _portfolioCosts = 0;
            _portfolioDividend = 0;
            _portfolioPerformanceFinalValue = 0;
            _portfolioPerformanceMarketValue = 0;
            _portfolioProfitLossFinalValue = 0;
            _portfolioProfitLossMarketValue = 0;
            _portfolioFinalValue = 0;
            _portfolioMarketValue = 0;
        }

        /// <summary>
        /// This function search for the correct website RegEx and
        /// sets the RegEx list and encoding to the share
        /// </summary>
        /// <param name="webSiteRegexList">List of the websites and their RegEx list</param>
        /// <returns>Flag if a website configuration for share exists</returns>
        public bool SetWebSiteRegexListAndEncoding(List<WebSiteRegex> webSiteRegexList)
        {
            // Flag if the website of the current share exists in the website configuration list
            bool bRegexFound = false;

            // Loop through the given website configuration list
            foreach (var webSiteRegexElement in webSiteRegexList)
            {
                // Check if the current share object use the current website configuration
                if (WebSite.Contains(webSiteRegexElement.WebSiteName))
                {
                    // Set the website configuration to the share object
                    RegexList = webSiteRegexElement.WebSiteRegexList;
                    EncodingType = webSiteRegexElement.WebSiteEncodingType;
                    bRegexFound = true;
                    break;
                }
            }

            return bRegexFound;
        }

        /// <summary>
        /// This function calculates the new total share value of this share
        /// with and without dividends and costs
        /// </summary>
        private void CalculateTotalShareValues()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && _allCostsEntries.CostValueTotal > decimal.MinValue / 2
                && _allDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && _allSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                // Calculation of the final value
                // TODO Remove cast
                FinalValue = CurPrice * (decimal)Volume
                    - _allCostsEntries.CostValueTotal
                    // TODO Remove cast
                    + (decimal)_allDividendEntries.DividendValueTotalWithTaxes
                    + _allSaleEntries.SaleProfitLossTotal;

                // Calculation of the market value
                // TODO Remove cast
                MarketValue = CurPrice * (decimal)Volume;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateTotalShareValues()");
            Console.WriteLine("CurrentPrice: {0}", CurPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("CostValueTotal: {0}", _allCostsEntries.CostValueTotal);
            Console.WriteLine("DividendValueTotal: {0}", _allDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine("SaleProfitLossTotal: {0}", _allSaleEntries.SaleProfitLossTotal);
            Console.WriteLine("MarketValue: {0}", MarketValue);
            Console.WriteLine("FinalValue: {0}", FinalValue);
#endif
        }

        /// <summary>
        /// This function calculates the new profit or lose value of this share
        /// with and without dividends and costs
        /// </summary>
        private void CalculateProfitLoseShareValues()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && Deposit > decimal.MinValue / 2
                && _allCostsEntries.CostValueTotal > decimal.MinValue / 2
                && _allDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && _allSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                // Calculation of the profit or loss for the final value
                // TODO Remove cast
                ProfitLossFinalValue = CurPrice * (decimal)Volume
                    - Deposit
                    - _allCostsEntries.CostValueTotal
                    // TODO Remove Cast
                    + (decimal)_allDividendEntries.DividendValueTotalWithTaxes
                    + _allSaleEntries.SaleProfitLossTotal;

                // Calculation of the profit or loss  for the market value
                // TODO Remove cast
                ProfitLossMarketValue = CurPrice * (decimal)Volume - Deposit;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateProfitLoseShareValues()");
            Console.WriteLine("CurrentPrice: {0}", CurrentPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("Deposit: {0}", Deposit);
            Console.WriteLine("CostValueTotal: {0}", _allCostsEntries.CostValueTotal);
            Console.WriteLine("DividendValueTotal: {0}", _allDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine("SaleProfitLossTotal: {0}", _allSaleEntries.SaleProfitLossTotal);
            Console.WriteLine("ProfitLossMarketValue: {0}", ProfitLossMarketValue);
            Console.WriteLine("ProfitLossFinalValue: {0}", ProfitLossFinalValue);
#endif
        }

        /// <summary>
        /// This function calculates the new total performance of this share
        /// with and without dividends and costs
        /// </summary>
        private void CalculateTotalPerformancesOfShare()
        {
            if (FinalValue > decimal.MinValue / 2
                && MarketValue > decimal.MinValue / 2
                && Deposit > decimal.MinValue / 2 
                && Deposit != 0
                )
            {
                PerformanceFinalValue = (FinalValue * 100) / Deposit - 100;
                PerformanceMarketValue = (MarketValue * 100) / Deposit - 100;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateTotalPerformancesOfShare()");
            Console.WriteLine("Deposit: {0}", Deposit);
            Console.WriteLine("MarketValue: {0}", MarketValue);
            Console.WriteLine("FinalValue: {0}", FinalValue);
            Console.WriteLine("PerformanceMarketValue: {0}", PerformanceMarketValue);
            Console.WriteLine("PerformanceFinalValue: {0}", PerformanceFinalValue);
#endif
        }

        /// <summary>
        /// This function caluclates the performance of all shares
        /// </summary>
        private void CalculatePerformanceOfAllShares()
        {
            if (PortfolioDeposit != 0)
                PortfolioPerformanceFinalValue = PortfolioFinalValue * 100 / PortfolioDeposit - 100;
#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculatePerformanceOfAllShares()");
            Console.WriteLine("PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine("PortfolioDeposit: {0}", PortfolioDeposit);
            Console.WriteLine("PortfolioPerformanceFinalValue: {0}", PortfolioPerformanceFinalValue);
#endif
        }

        /// <summary>
        /// This function calculates the profit or lose of all shares
        /// </summary>
        private void CalculateProfitLoseOfAllShares()
        {
            PortfolioProfitLossFinalValue = PortfolioFinalValue - PortfolioDeposit;

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateProfitLoseOfAllShares()");
            Console.WriteLine("PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine("PortfolioDeposit: {0}", PortfolioDeposit);
            Console.WriteLine("PortfolioProfitLossFinalValue: {0}", PortfolioProfitLossFinalValue);
#endif
        }

        /// <summary>
        /// This function caluclates the new profit or lose to the previous day of the share
        /// </summary>
        private void CalculateDayBeforeProfitLoseOfShare()
        {
            if (CurPrice > decimal.MinValue / 2
                && PrevDayPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                )
            {
                // TODO Remove cast
                PrevDayProfitLoss = (CurPrice - PrevDayPrice) * (decimal)Volume;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateDayBeforeProfitLoseOfShare()");
            Console.WriteLine("CurPrice: {0}", CurPrice);
            Console.WriteLine("PrevDayPrice: {0}", PrevDayPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("PrevDayProfitLoss: {0}", PrevDayProfitLoss);
            Console.WriteLine("");
#endif
        }

        /// <summary>
        /// This function calculates the performance to the previous day of this share
        /// </summary>
        private void CalculateDayBeforePerformanceOfShare()
        {
            if (CurPrice > decimal.MinValue / 2
                && PrevDayPrice > decimal.MinValue / 2
                )
            {
                PrevDayPerformance = (CurPrice * 100) / PrevDayPrice - 100;
                PrevDayDifference = CurPrice - PrevDayPrice;
            }

            if (ImageListPrevDayPerformance != null && ImageListPrevDayPerformance.Count > 0)
            {
                if (PrevDayPerformance < 0)
                {
                    ImagePrevDayPerformance = ImageListPrevDayPerformance[1];
                }
                else if (PrevDayPerformance == 0)
                {
                    ImagePrevDayPerformance = ImageListPrevDayPerformance[2];

                }
                else
                    ImagePrevDayPerformance = ImageListPrevDayPerformance[3];
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateDayBeforePerformanceOfShare()");
            Console.WriteLine("CurPrice: {0}", CurPrice);
            Console.WriteLine("PrevDayPrice: {0}", PrevDayPrice);
            Console.WriteLine("PrevDayPerformance: {0}", PrevDayPerformance);
            Console.WriteLine("PrevDayDifference: {0}", PrevDayDifference);
#endif
        }

        ///// <summary>
        ///// This function calculates the dividend with taxes
        ///// </summary>
        //private void CalculateDividendMinusTaxes()
        //{
        //    DividendValueWithTaxes = DividendValue;

        //    if (TaxAtSourceFlag)
        //    {
        //        DividendValueWithTaxes = DividendValue / 100 * TaxAtSourcePercentage;
        //    }
        //}

        /// <summary>
        /// This function adds the buy for the share to the dictionary
        /// </summary>
        /// <param name="bDepositCalc">Flag if the deposit should be recalculated</param>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <param name="decVolume">Buy volume</param>
        /// <param name="decPrice">Price for one share</param>
        /// <param name="decReduction">Reduction of the buy</param>
        /// <param name="decCosts">Costs of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(bool bDepositCalc, string strDateTime, decimal decVolume, decimal decPrice, decimal decReduction, decimal decCosts, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("AddBuy()");
                Console.WriteLine("bDepositCalc: {0}", bDepositCalc);
                Console.WriteLine("strDateTime: {0}", strDateTime);
                Console.WriteLine("decVolume: {0}", decVolume);
                Console.WriteLine("decReduction: {0}", decReduction);
                Console.WriteLine("decCosts: {0}", decCosts);
                Console.WriteLine("decValueWithoutReduction: {0}", decValueWithoutReduction);
                Console.WriteLine("strDoc: {0}", strDoc);
#endif
                if (!AllBuyEntries.AddBuy(strDateTime, decVolume, decPrice, decReduction, decCosts, strDoc))
                    return false;

                // Set buy value of the share
                BuyValueTotal = AllBuyEntries.BuyMarketValueTotal;

                // Set new volume
                if (Volume == decimal.MinValue / 2)
                    Volume = 0;
                Volume += decVolume;

                // Recalculate deposit
                if (Deposit == decimal.MinValue / 2)
                    Deposit = 0;
                if (bDepositCalc)
                {
                    Deposit += AllBuyEntries.GetBuyObjectByDateTime(strDateTime).MarketValue;
                }
                //Deposit += decValue;

#if DEBUG_SHAREOBJECT
                Console.WriteLine("BuyValueTotal: {0}", BuyValueTotal);
                Console.WriteLine("Volume: {0}", Volume);
                Console.WriteLine("Deposit: {0}", Deposit);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function removes a buy for the share from the dictionary
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveBuy(string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("RemoveBuy()");
                Console.WriteLine("strDateTime: {0}", strDateTime);
#endif
                // Get BuyObject by date and time and add the sale depsoit and value to the share
                BuyObject buyObject = AllBuyEntries.GetBuyObjectByDateTime(strDateTime);
                if (buyObject != null)
                {
                    Volume -= buyObject.Volume;
                    Deposit -= buyObject.MarketValue;
                    BuyValueTotal = AllBuyEntries.BuyMarketValueTotal;
                    // TODO Why

#if DEBUG_SHAREOBJECT
                    Console.WriteLine("Volume: {0}", Volume);
                    Console.WriteLine("Deposit: {0}", Deposit);
                    Console.WriteLine("BuyValueTotal: {0}", BuyValueTotal);
#endif
                    // Remove buy by date and time
                    if (!_allBuyEntries.RemoveBuy(strDateTime))
                        return false;
                }
                else
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function adds the sale for the share to the dictionary
        /// </summary>
        /// <param name="bDepositCalc">Flag if the deposit should be recalculated</param>
        /// <param name="strDateTime">Date and time of the sale</param>
        /// <param name="decVolume">Sale volume</param>
        /// <param name="decValue">Sale value</param>
        /// <param name="decProfitLoss">Sale profit or loss</param>
        /// <param name="decCurrentVolume">Current share volume</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(bool bDepositCalc, string strDateTime, decimal decVolume, decimal decValue, decimal decProfitLoss, decimal decCurrentVolume, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("AddSale()");
                Console.WriteLine("bDepositCalc: {0}", bDepositCalc);
                Console.WriteLine("strDateTime: {0}", strDateTime);
                Console.WriteLine("decVolume: {0}", decVolume);
                Console.WriteLine("decValue: {0}", decValue);
                Console.WriteLine("decProfitLoss: {0}", decProfitLoss);
                Console.WriteLine("decCurrentVolume: {0}", decCurrentVolume);
                Console.WriteLine("strDoc: {0}", strDoc);
#endif
                if (!AllSaleEntries.AddSale(strDateTime, decVolume, decValue, decProfitLoss, strDoc))
                    return false;

                // Set sale value of the share
                //SaleValue = AllSaleEntries.SaleValueTotal;

                // Set new volume
                if (Volume == decimal.MinValue / 2)
                    Volume = 0;
                Volume -= decVolume;

                // Recalculate deposit
                if (Deposit == decimal.MinValue / 2)
                    Deposit = 0;
                if (bDepositCalc)
                {
                    Deposit -= (decValue - decProfitLoss);
                    //Deposit = Volume * Deposit / decCurrentVolume ;
                }

#if DEBUG_SHAREOBJECT
                Console.WriteLine("Volume: {0}", Volume);
                Console.WriteLine("Deposit: {0}", Deposit);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function removes a sale for the share from the dictionary
        /// </summary>
        /// <param name="strDateTime">Date and time of the sale remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveSale(string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("RemoveSale()");
                Console.WriteLine("strDateTime: {0}", strDateTime);
#endif
                // Get SaleObject by date and time and add the sale depsoit and value to the share
                SaleObject saleObject = AllSaleEntries.GetSaleObjectByDateTime(strDateTime);
                if (saleObject != null)
                {
                    // Remove sale by date and time
                    if (!_allSaleEntries.RemoveSale(strDateTime))
                        return false;

                    Volume += saleObject.SaleVolume;
                    Deposit += (saleObject.SaleValue - saleObject.SaleProfitLoss);
                    //Deposit = Volume * (AllBuyEntries.BuyValueTotal / AllBuyEntries.BuyVolumeTotal);
#if DEBUG_SHAREOBJECT
                    Console.WriteLine("Volume: {0}", Volume);
                    Console.WriteLine("Deposit: {0}", Deposit);
#endif
                }
                else
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function adds the costs for the share to the dictionary
        /// </summary>
        /// <param name="bCostOfABuy">Flag if the cost is part of a buy</param>
        /// <param name="strDateTime">Date and time of the cost</param>
        /// <param name="decValue">Cost value</param>
        /// <param name="strDoc">Doc of the cost</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddCost(bool bCostOfABuy, string strDateTime, decimal decValue, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("AddCost()");
                Console.WriteLine("bCostOfABuy: {0}", bCostOfABuy);
                Console.WriteLine("strDateTime: {0}", strDateTime);
                Console.WriteLine("decValue: {0}", decValue);
                Console.WriteLine("strDoc: {0}", strDoc);
#endif
                // Remove current costs of the share from the costs of all shares
                PortfolioCosts -= CostsValueTotal;

                if (!AllCostsEntries.AddCost(bCostOfABuy, strDateTime, decValue, strDoc))
                    return false;

                // Set costs of the share
                CostsValueTotal = AllCostsEntries.CostValueTotal;

                // Add new costs of the share to the costs of all shares
                PortfolioCosts += CostsValueTotal;

                // Recalculate the total sum of the share
                CalculateTotalShareValues();

                // Recalculate the apperciation
                CalculateTotalPerformancesOfShare();

                // Recalculate the profit or lose of the share
                CalculateProfitLoseShareValues();

                // Realculate the performance of all shares
                CalculatePerformanceOfAllShares();

                // Recalulate the profit or lose of all shares
                CalculateProfitLoseOfAllShares();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("CostsValueTotal: {0}", CostsValueTotal);
                Console.WriteLine("PortfolioCosts: {0}", PortfolioCosts);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function removes a cost for the share from the dictionary
        /// </summary>
        /// <param name="strDateTime">Date of the cost remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveCost(string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("RemoveCost()");
                Console.WriteLine("strDateTime: {0}", strDateTime);
#endif
                // Remove current costs the share to the costs of all shares
                PortfolioCosts -= AllCostsEntries.CostValueTotal;

                // Add remove cost to deposit of the share
                //Deposit += AllCostsEntries.GetCostObjectByDateTime(strDateTime).CostValue;

                // Remove cost by date
                if (!_allCostsEntries.RemoveCost(strDateTime))
                    return false;

                // Set costs of the share
                CostsValueTotal = AllCostsEntries.CostValueTotal;

                // Add new costs of the share to the costs of all shares
                PortfolioCosts += AllCostsEntries.CostValueTotal;

                // Recalculate the total sum of the share
                CalculateTotalShareValues();

                // Recalculate the apperciation
                CalculateTotalPerformancesOfShare();

                // Recalculate the profit or lose of the share
                CalculateProfitLoseShareValues();

                // Realculate the performance of all shares
                CalculatePerformanceOfAllShares();

                // Recalulate the profit or lose of all shares
                CalculateProfitLoseOfAllShares();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("CostsValueTotal: {0}", CostsValueTotal);
                Console.WriteLine("PortfolioCosts: {0}", PortfolioCosts);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function adds the dividend payouts for the share to the dictionary
        /// </summary>
        /// <param name="cultureInfoFC">CultureInfo of the share for the foreign currency</param>
        /// <param name="csEnableFC">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency</param>
        /// <param name="strDate">Pay date of the new dividend list entry</param>
        /// <param name="decRate">Paid dividend of one share</param>
        /// <param name="decVolume">Share volume at the pay date</param>
        /// <param name="decTaxAtSource">Tax at source value</param>
        /// <param name="decCapitalGainsTax">Capital gains tax value</param>
        /// <param name="decSolidarityTax">Solidarity tax value</param>
        /// <param name="decSharePrice">Share price at the pay date</param>
        /// <param name="strDoc">Document of the dividend</param>
        /// <returns>Flag if the add was successful</returns>  
        public bool AddDividend(CultureInfo cultureInfoFC, CheckState csEnableFC, decimal decExchangeRatio, string strDate, decimal decRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax, decimal decSharePrice, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("AddDividend()");
                Console.WriteLine("EnableFC: {0}", csEnableFC);
                if (csEnableFC == CheckState.Checked)
                Console.WriteLine("cultureInfoFC: {0}", culturInfoFC);
                Console.WriteLine("decExchangeRatio: {0}", decExchangeRatio);
                Console.WriteLine("strDateTime: {0}", strDateTime);
                Console.WriteLine("decDividendRate: {0}", decDividendRate);
                Console.WriteLine("decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine("decCapitalGainsTax: {0}", decCapitalGainsTax);
                Console.WriteLine("decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine("decShareVolume: {0}", decShareVolume);
                Console.WriteLine("decSharePrice: {0}", decSharePrice);
                Console.WriteLine("strDoc: {0}", strDoc);
#endif
                // Remove current dividend of the share from the dividend of all shares
                PortfolioDividend -= DividendValueTotal;

                if (!AllDividendEntries.AddDividend(cultureInfoFC, csEnableFC, decExchangeRatio, strDate, decRate, decVolume,
                    decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc))
                    return false;

                // TODO With or without taxes!!!
                // Set dividend of the share
                DividendValueTotal = AllDividendEntries.DividendValueTotalWithTaxes;

                // Add new dividend of the share to the dividend of all shares
                PortfolioDividend += DividendValueTotal;

                // Recalculate the total sum of the share
                CalculateTotalShareValues();

                // Recalculate the apperciation
                CalculateTotalPerformancesOfShare();

                // Recalculate the profit or lose of the share
                CalculateProfitLoseShareValues();

                // Recalculate the performance of all shares
                CalculatePerformanceOfAllShares();

                // Recalculate the profit or lose of all shares
                CalculateProfitLoseOfAllShares();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("DividendValueTotal: {0}", DividendValueTotal);
                Console.WriteLine("PortfolioDividend: {0}", PortfolioDividend);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function removes a dividend payout for the share from the list
        /// </summary>
        /// <param name="strDateTime">Date of the dividend pay date</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveDividend(string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("RemoveDividend()");
                Console.WriteLine("strDateTime: {0}", strDateTime);
#endif
                // Set dividend of all shares
                PortfolioDividend -= DividendValueTotal;

                // Remove dividend by date
                if (!_allDividendEntries.RemoveDividend(strDateTime))
                    return false;

                // Set dividend of the share
                DividendValueTotal = AllDividendEntries.DividendValueTotalWithTaxes;

                // Set dividend of all shares
                PortfolioDividend += DividendValueTotal;
                
                // Recalculate the total sum of the share
                CalculateTotalShareValues();

                // Recalculate the appreciation
                CalculateTotalPerformancesOfShare();

                // Recalculate the profit or lose of the share
                CalculateProfitLoseShareValues();

                // Recalculate the performance of all shares
                CalculatePerformanceOfAllShares();

                // Recalulate the profit or lose of all shares
                CalculateProfitLoseOfAllShares();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("DividendValueTotal: {0}", DividendValueTotal);
                Console.WriteLine("PortfolioDividend: {0}", PortfolioDividend);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            if (Deposit > decimal.MinValue / 2)
                PortfolioDeposit -= _deposit;

            if (FinalValue > decimal.MinValue / 2)
                PortfolioFinalValue -= _finalValue;

            //            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        ~ShareObject()
        {
        }

        #endregion Methods
    }

    public class ShareObjectListComparer : IComparer<ShareObject>
    {
        public int Compare(ShareObject object1, ShareObject object2)
        {
            return string.Compare(object1.NameAsStr, object2.NameAsStr);
        }
    }

    public class ShareObjectSearch
    {
        #region Variables

        private string _searchString;

        #endregion Variables

        public ShareObjectSearch(string searchString)
        {
            _searchString = searchString;
        }

        public bool Compare(ShareObject shareObject)
        {
            return shareObject.Wkn == _searchString;
        }
    }
}
