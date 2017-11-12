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
using System.Windows.Forms;
using System.Xml;
using WebParser;

namespace SharePortfolioManager
{
    public class ShareObjectFinalValue : ShareObject
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Flag if the object is already disposed
        /// </summary>
        private bool _bDisposed = false;

        /// <summary>
        /// Stores the current share value with dividends, costs, profits and loss (final value)
        /// </summary>
        private decimal _finalValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the performance of the share with dividends, costs, profits and loss
        /// </summary>
        private decimal _performanceValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the profit or loss of the share with dividends, costs, profits and loss
        /// </summary>
        private decimal _profitLossValue = decimal.MinValue / 2;

        #endregion General variables

        #region Costs variables

        /// <summary>
        /// Stores the total costs of the share
        /// </summary>
        private decimal _costsValueTotal = 0;

        /// <summary>
        /// Stores the cost pays of the share
        /// </summary>
        private AllCostsOfTheShare _allCostsEntries = new AllCostsOfTheShare();

        #endregion Costs variables

        #region Costs XML variables

        /// <summary>
        /// Stores the tag name prefix of a cost entry
        /// </summary>
        private const string _costsTagNamePre = "Cost";

        /// <summary>
        /// Stores the attribute name for the flag if the cost is a part of a buy
        /// </summary>
        private const string _costsBuyPartAttrName = "BuyPart";

        /// <summary>
        /// Stores the attribute name for the flag if the cost is a part of a sale
        /// </summary>
        private const string _costsSalePartAttrName = "SalePart";

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
        private const short _costsAttrCount = 5;

        #endregion Costs XML variables

        #region Dividends variables

        /// <summary>
        /// Stores the payout interval of the dividend
        /// </summary>
        private int _dividendPayoutInterval;

        /// <summary>
        /// Stores the total dividend value of the share
        /// </summary>
        private decimal _dividendValueTotal = 0;

        /// <summary>
        /// Stores the total dividend of the share minus the taxes
        /// </summary>
        private decimal _dividendValueTotalWithTaxes = 0;

        /// <summary>
        /// Stores the dividend payouts of the share
        /// </summary>
        private AllDividendsOfTheShare _allDividendEntries = new AllDividendsOfTheShare();

        #endregion Dividend values

        #region Dividends XML variables

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

        #endregion Dividends XML variables

        #region Portfolio value variables

        /// <summary>
        /// Stores the value of the portfolio (all shares) without dividends, costs, profits and loss (market value)
        /// </summary>
        static private decimal _portfolioPurchaseValue = 0;

        /// <summary>
        /// Stores the purchase of sales of the portfolio (all shares)
        /// </summary>
        static private decimal _portfolioSalePurchaseValue = 0;

        /// <summary>
        /// Stores the current value of the portfolio (all shares) with dividends, costs, profits and loss (final value)
        /// </summary>
        static private decimal _portfolioFinalValue = 0;

        /// <summary>
        /// Stores the costs of the portfolio (all shares)
        /// </summary>
        static private decimal _portfolioCosts = 0;

        /// <summary>
        /// Stores the dividends of the portfolio (all shares)
        /// </summary>
        static private decimal _portfolioDividend = 0;

        /// <summary>
        /// Stores the performance of the portfolio (all shares) with dividends, costs, profits and loss
        /// </summary>
        static private decimal _portfolioPerformanceValue = 0;

        /// <summary>
        /// Stores the profit or lose of the portfolio (all shares) with dividends, costs, profits and loss
        /// </summary>
        static private decimal _portfolioProfitLossValue = 0;

        #endregion Portfolio value variables

        #endregion Variables

        #region Properties

        #region General properties

        [Browsable(false)]
        public bool Disposed
        {
            get { return _bDisposed; }
            internal set { _bDisposed = value; }
        }

        [Browsable(false)]
        public override decimal Volume
        {
            get { return base.Volume; }
            set
            {
                // Set the new share volume
                if (value != base.Volume)
                {
                    base.Volume = value;

                    // Recalculate the performance to the previous day
                    CalculatePrevDayPerformance();

                    // Recalculate the profit or loss to the previous day
                    CalculatePrevDayProfitLoss();

                    // Recalculate the total sum of the share
                    CalculateFinalValue();

                    // Recalculate the profit or loss of the share
                    CalculateProfitLoss();

                    // Recalculate the appreciation
                    CalculatePerformance();
                }
            }
        }

        [Browsable(false)]
        public override decimal CurPrice
        {
            get { return base.CurPrice; }
            set
            {
                // Set the new share price
                if (value != base.CurPrice)
                {
                    base.CurPrice = value;

                    // Recalculate the performance to the previous day
                    CalculatePrevDayPerformance();

                    // Recalculate the profit or loss to the previous day
                    CalculatePrevDayProfitLoss();

                    // Recalculate the total sum of the share
                    CalculateFinalValue();

                    // Recalculate the profit or loss of the share
                    CalculateProfitLoss();

                    // Recalculate the appreciation
                    CalculatePerformance();
                }
            }
        }

        [Browsable(false)]
        public override decimal PrevDayPrice
        {
            get { return base.PrevDayPrice; }
            set
            {
                // Set the previous day share price
                if (value != base.PrevDayPrice)
                {
                    base.PrevDayPrice = value;

                    // Recalculate the performance to the previous day
                    CalculatePrevDayPerformance();

                    // Recalculate the profit or loss to the previous day
                    CalculatePrevDayProfitLoss();

                    // Recalculate the total sum of the share
                    CalculateFinalValue();

                    // Recalculate the profit or loss of the share
                    CalculateProfitLoss();

                    // Recalculate the appreciation
                    CalculatePerformance();
                }
            }
        }

        [Browsable(false)]
        public override decimal PurchaseValue
        {
            get { return base.PurchaseValue; }
            set
            {
                // Set the new purchase value
                if (value != base.PurchaseValue)
                {
                    // Recalculate portfolio purchase value
                    if (base.PurchaseValue > decimal.MinValue / 2)
                        PortfolioPurchaseValue -= base.PurchaseValue;
                    PortfolioPurchaseValue += value;

                    base.PurchaseValue = value;

                    // Recalculate the total sum of the share
                    CalculateFinalValue();

                    // Recalculate the profit or loss of the share
                    CalculateProfitLoss();

                    // Recalculate the appreciation
                    CalculatePerformance();
                }
            }
        }

        [Browsable(false)]
        public override decimal SalePurchaseValueTotal
        {
            get { return base.SalePurchaseValueTotal; }
            set
            {
                // Set the new purchase value
                if (value != base.SalePurchaseValueTotal)
                {
                    // Recalculate portfolio purchase value
                    if (base.SalePurchaseValueTotal > decimal.MinValue / 2)
                        PortfolioSalePurchaseValue -= base.SalePurchaseValueTotal;
                    PortfolioSalePurchaseValue += value;

                    base.SalePurchaseValueTotal = value;
                }
            }
        }

        [Browsable(false)]
        public override CultureInfo CultureInfo
        {
            get { return base.CultureInfo; }
            set
            {
                base.CultureInfo = value;

                // Set culture info to the lists
                if (AllCostsEntries != null)
                    AllCostsEntries.SetCultureInfo(base.CultureInfo);
                if (AllDividendEntries != null)
                    AllDividendEntries.SetCultureInfo(base.CultureInfo);
            }
        }

        #endregion General properties

        #region Costs properties

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
        public AllCostsOfTheShare AllCostsEntries
        {
            get { return _allCostsEntries; }
            set { _allCostsEntries = value; }
        }

        #endregion Costs properties

        #region Costs XML properties

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
        public string CostsSalePartAttrName
        {
            get { return _costsSalePartAttrName; }
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

        #endregion Costs XML properties

        #region Dividends properties

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

        #endregion Dividend properties

        #region Dividend XML properties

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

        #endregion Dividend XML properties

        #region Costs and dividends with taxes

        [Browsable(false)]
        public string CostsDividendWithTaxesAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CostsValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(DividendValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Costs and dividends with taxes

        #region Final value properties

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
                    Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
#endif
                    // Recalculate the total sum of all shares
                    // by subtracting the old total share value and then add the new value
                    if (_finalValue > decimal.MinValue / 2)
                        PortfolioFinalValue -= _finalValue;
                    PortfolioFinalValue += value;

                    // Set the total share volume
                    _finalValue = value;

#if DEBUG_SHAREOBJECT
                    Console.WriteLine(@"_finalValue: {0}", _finalValue);
                    Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
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

        [Browsable(false)]
        public decimal PerformanceValue
        {
            get { return _performanceValue; }
            internal set { _performanceValue = value; }
        }

        [Browsable(false)]
        public string PerformanceValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PerformanceValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PerformanceValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PerformanceValue, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal ProfitLossValue
        {
            get { return _profitLossValue; }
            internal set
            {
                _profitLossValue = value;
            }
        }

        [Browsable(false)]
        public string ProfitLossValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(ProfitLossValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string ProfitLossValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(ProfitLossValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string ProfitLossPerformanceValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(ProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PerformanceValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public string PurchaseValueFinalValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(FinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Final value properties

        #region Portfolio value properties

        [Browsable(false)]
        public decimal PortfolioPurchaseValue
        {
            get { return _portfolioPurchaseValue; }
            internal set
            {
                if (_portfolioPurchaseValue != value)
                {
                    _portfolioPurchaseValue = value;

                    // Recalculate the performance of all shares
                    CalculatePortfolioPerformance();

                    // Recalculate the profit or lose of all shares
                    CalculatePortfolioProfitLoss();
                }
            }
        }

        [Browsable(false)]
        public string PortfolioPurchaseValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioPurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioPurchaseValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioPurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PortfolioSalePurchaseValue
        {
            get { return _portfolioSalePurchaseValue; }
            internal set
            {
                if (_portfolioSalePurchaseValue != value)
                {
                    _portfolioSalePurchaseValue = value;

                    // Recalculate the performance of all shares
                    CalculatePortfolioPerformance();

                    // Recalculate the profit or lose of all shares
                    CalculatePortfolioProfitLoss();
                }
            }
        }

        [Browsable(false)]
        public string PortfolioSalePurchaseValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioSalePurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioSaleValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioSalePurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
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
        public string CostsDividendPortfolioValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioCosts, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PortfolioDividend, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        [Browsable(false)]
        public decimal PortfolioPerformanceValue
        {
            get { return _portfolioPerformanceValue; }
            internal set { _portfolioPerformanceValue = value; }
        }

        [Browsable(false)]
        public string PortfolioPerformanceValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioPerformanceValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioPerformanceValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioPerformanceValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal PortfolioProfitLossValue
        {
            get { return _portfolioProfitLossValue; }
            internal set { _portfolioProfitLossValue = value; }
        }

        [Browsable(false)]
        public string PortfolioProfitLossValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PortfolioProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PortfolioProfitLossValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PortfolioProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string ProfitLossPerformancePortfolioValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PortfolioPerformanceValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
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
                    CalculatePortfolioPerformance();

                    // Recalculate the profit or lose of all shares
                    CalculatePortfolioProfitLoss();
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

        #endregion Portfolio value properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Wkn")]
        public string DgvWkn
        {
            get { return WknAsStr; }
        }

        [Browsable(true)]
        [DisplayName(@"Name")]
        public string DgvNameAsStr
        {
            get { return NameAsStr; }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string DgvVolumeAsStr
        {
            get { return VolumeAsStr;  }
        }

        [Browsable(true)]
        [DisplayName(@"CostsDividendWithTaxes")]
        public string DgvCostsDividendWithTaxesAsStrUnit
        {
            get { return CostsDividendWithTaxesAsStrUnit;  } 
        }

        [Browsable(true)]
        [DisplayName(@"CurPrevPrice")]
        public string DgvCurPrevPriceAsStrUnit
        {
            get { return CurPrevPriceAsStrUnit; }
        }

        [Browsable(true)]
        [DisplayName(@"PrevDayPerformance")]
        public string DgvPrevDayDifferencePerformanceAsStrUnit
        {
            get { return PrevDayDifferencePerformanceAsStrUnit; }
        }

        [Browsable(true)]
        [DisplayName(@"")]
        public Image DgvImagePrevDayPerformance
        {
            get { return ImagePrevDayPerformance; }
        }

        [Browsable(true)]
        [DisplayName(@"ProfitLossPerformanceFinalValue")]
        public string DgvProfitLossPerformanceValueAsStrUnit
        {
            get { return ProfitLossPerformanceValueAsStrUnit; }
        }

        [Browsable(true)]
        [DisplayName(@"PurchaseValueFinalValue")]
        public string DgvPurchaseValueFinalValueAsStrUnit
        {
            get { return PurchaseValueFinalValueAsStrUnit; }
        }

        #endregion Data grid properties

        #endregion Properties

        #region Methods

        #region Constructors

        /// <summary>
        /// Standard constructor
        /// </summary>
        public ShareObjectFinalValue(List<Image> imageList, string percentageUnit, string pieceUnit) : base(imageList, percentageUnit, pieceUnit)
        {
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
        /// <param name="purchaseValue">PurchaseValue of the share</param>
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
        public ShareObjectFinalValue(
            string wkn, string addDateTime, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShareDate, DateTime lastUpdateShareTime,
            decimal price, decimal volume, decimal reduction, decimal costs, decimal purchaseValue,
            string webSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            int dividendPayoutInterval, string document) : base(wkn, addDateTime, name, lastUpdateInternet, lastUpdateShareDate, lastUpdateShareTime,
                                                                price, volume, reduction, costs, purchaseValue,
                                                                webSite, imageListForDayBeforePerformance, regexList, cultureInfo,
                                                                document)
        {
            Console.WriteLine("dividendPayoutInterval: {0}", dividendPayoutInterval);

            DividendPayoutInterval = dividendPayoutInterval;
        }

        #endregion Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~ShareObjectFinalValue()
        {
            Dispose(false);
        }

        #endregion Destructor

        #region Costs methods

        /// <summary>
        /// This function adds the costs for the share to the dictionary
        /// </summary>
        /// <param name="bCostOfABuy">Flag if the cost is part of a buy</param>
        /// <param name="bCostOfASale">Flag if the cost is part of a Sale</param>
        /// <param name="strDateTime">Date and time of the cost</param>
        /// <param name="decValue">Cost value</param>
        /// <param name="strDoc">Doc of the cost</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddCost(bool bCostOfABuy, bool bCostOfASale, string strDateTime, decimal decValue, string strDoc = "")
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

                if (!AllCostsEntries.AddCost(bCostOfABuy, bCostOfASale, strDateTime, decValue, strDoc))
                    return false;

                // Set costs of the share
                CostsValueTotal = AllCostsEntries.CostValueTotal;

                // Add new costs of the share to the costs of all shares
                PortfolioCosts += CostsValueTotal;

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("CostsValueTotal: {0}", CostsValueTotal);
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

                // Remove cost by date
                if (!_allCostsEntries.RemoveCost(strDateTime))
                    return false;

                // Set costs of the share
                CostsValueTotal = AllCostsEntries.CostValueTotal;

                // Add new costs of the share to the costs of all shares
                PortfolioCosts += AllCostsEntries.CostValueTotal;

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("CostsValueTotal: {0}", CostsValueTotal);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Costs methods

        #region Dividend methods

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
                    Console.WriteLine("cultureInfoFC: {0}", cultureInfoFC);
                Console.WriteLine("decExchangeRatio: {0}", decExchangeRatio);
                Console.WriteLine("strDateTime: {0}", strDate);
                Console.WriteLine("decDividendRate: {0}", decRate);
                Console.WriteLine("decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine("decCapitalGainsTax: {0}", decCapitalGainsTax);
                Console.WriteLine("decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine("decShareVolume: {0}", decVolume);
                Console.WriteLine("decSharePrice: {0}", decSharePrice);
                Console.WriteLine("strDoc: {0}", strDoc);
#endif
                if (!AllDividendEntries.AddDividend(cultureInfoFC, csEnableFC, decExchangeRatio, strDate, decRate, decVolume,
                    decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc))
                    return false;

                // Remove current dividend of the share from the dividend of all shares
                PortfolioDividend -= DividendValueTotal;

                // Set dividend of the share
                DividendValueTotal = AllDividendEntries.DividendValueTotalWithTaxes;

                // Add new dividend of the share to the dividend of all shares
                PortfolioDividend += DividendValueTotal;

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("DividendValueTotal: {0}", DividendValueTotal);
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

                // Add new costs of the share to the costs of all shares
                PortfolioCosts += CostsValueTotal;

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_SHAREOBJECT
                Console.WriteLine("DividendValueTotal: {0}", DividendValueTotal);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Dividend methods

        #region Performance methods

        /// <summary>
        /// This function calculates the new final value of this share
        /// with dividends and costs and sales profit or loss
        /// </summary>
        private void CalculateFinalValue()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllCostsEntries.CostValueTotal > decimal.MinValue / 2
                && AllDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                FinalValue = CurPrice * Volume
//                    - AllCostsEntries.CostValueTotal
                    + AllDividendEntries.DividendValueTotalWithTaxes
                    + AllSaleEntries.SaleProfitLossTotalWithoutCosts;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateFinalValue()");
            Console.WriteLine("CurrentPrice: {0}", CurPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("CostValueTotal: {0}", AllCostsEntries.CostValueTotal);
            Console.WriteLine("DividendValueTotal: {0}", AllDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine("SaleProfitLossTotal: {0}", AllSaleEntries.SaleProfitLossTotal);
            Console.WriteLine("FinalValue: {0}", FinalValue);
#endif
        }

        /// <summary>
        /// This function calculates the new profit or loss value of this share
        /// with dividends and costs and sales profit or loss
        /// </summary>
        private void CalculateProfitLoss()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllCostsEntries.CostValueTotal > decimal.MinValue / 2
                && AllDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                ProfitLossValue = CurPrice * Volume
                    - PurchaseValue
//                    - AllCostsEntries.CostValueTotal
                    + AllDividendEntries.DividendValueTotalWithTaxes
                    + AllSaleEntries.SaleProfitLossTotalWithoutCosts;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateProfitLoss()");
            Console.WriteLine("CurPrice: {0}", CurPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine("CostValueTotal: {0}", AllCostsEntries.CostValueTotal);
            Console.WriteLine("DividendValueTotal: {0}", AllDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine("SaleProfitLossTotal: {0}", AllSaleEntries.SaleProfitLossTotal);
            Console.WriteLine("ProfitLossValue: {0}", ProfitLossValue);
#endif
        }

        /// <summary>
        /// This function calculates the new performance of this share
        /// with dividends and costs and sales profit or loss
        /// </summary>
        private void CalculatePerformance()
        {
            if (FinalValue > decimal.MinValue / 2
                && PurchaseValue > decimal.MinValue / 2
                )
            {
                if ((PurchaseValue + AllSaleEntries.SalePurchaseValueTotal) != 0)
                    PerformanceValue = ( (FinalValue + AllSaleEntries.SalePurchaseValueTotal) * 100) / (PurchaseValue + AllSaleEntries.SalePurchaseValueTotal) - 100;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculatePerformance()");
            Console.WriteLine("FinalValue: {0}", FinalValue);
            Console.WriteLine("PerformanceValue: {0}", PerformanceValue);
#endif
        }

        #endregion Performance methods

        #region Save share object

        /// <summary>
        /// This function saves a share object to the XML document
        /// </summary>
        /// <param name="shareObject">ShareObject</param>
        /// <param name="xmlPortfolio">XML with the portfolio</param>
        /// <param name="xmlReaderPortfolio">XML reader for the portfolio</param>
        /// <param name="xmlReaderSettingsPortfolio">XML reader settings for the portfolio</param>
        /// <param name="strPortfolioFileName">Name of the portfolio XML</param>
        /// <param name="exception">Exception which may occur. If no exception occurs the value is null</param>
        /// <returns>Flag if the save was successful</returns>
        public static bool SaveShareObject(ShareObjectFinalValue shareObject, ref XmlDocument xmlPortfolio, ref XmlReader xmlReaderPortfolio, ref XmlReaderSettings xmlReaderSettingsPortfolio, string strPortfolioFileName, out Exception exception)
        {
            try
            {
                // Update existing share
                var nodeListShares = xmlPortfolio.SelectNodes(string.Format("/Portfolio/Share [@WKN = \"{0}\"]", shareObject.Wkn));
                foreach (XmlNode nodeElement in nodeListShares)
                {
                    if (nodeElement != null)
                    {
                        if (nodeElement.HasChildNodes && nodeElement.ChildNodes.Count == shareObject.ShareObjectTagCount)
                        {
                            nodeElement.Attributes["WKN"].InnerText = shareObject.Wkn;
                            nodeElement.Attributes["Name"].InnerText = shareObject.NameAsStr;

                            for (int i = 0; i < nodeElement.ChildNodes.Count; i++)
                            {
                                switch (i)
                                {
                                    #region General
                                    case 0:
                                        nodeElement.ChildNodes[i].InnerText = string.Format(@"{0} {1}", shareObject.LastUpdateInternet.ToShortDateString(), shareObject.LastUpdateInternet.ToShortTimeString());
                                        break;
                                    case 1:
                                        nodeElement.ChildNodes[i].InnerText = string.Format(@"{0} {1}", shareObject.LastUpdateDate.ToShortDateString(), shareObject.LastUpdateDate.ToShortTimeString());
                                        break;
                                    case 2:
                                        nodeElement.ChildNodes[i].InnerText = string.Format(@"{0} {1}", shareObject.LastUpdateTime.ToShortDateString(), shareObject.LastUpdateTime.ToShortTimeString());
                                        break;
                                    case 3:
                                        nodeElement.ChildNodes[i].InnerText = shareObject.CurPriceAsStr;
                                        break;
                                    case 4:
                                        nodeElement.ChildNodes[i].InnerText = shareObject.PrevDayPriceAsStr;
                                        break;
                                    case 5:
                                        nodeElement.ChildNodes[i].InnerText = shareObject.WebSite;
                                        break;
                                    case 6:
                                        nodeElement.ChildNodes[i].InnerXml = shareObject.CultureInfoAsStr;
                                        break;

                                    #endregion General

                                    #region Buys

                                    case 7:
                                        // Remove old buys
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        foreach (var buyElementYear in shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                                        {
                                            XmlElement newBuyElement = xmlPortfolio.CreateElement(shareObject.BuyTagNamePre);
                                            newBuyElement.SetAttribute(shareObject.BuyDateAttrName, buyElementYear.DateAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyVolumeAttrName, buyElementYear.VolumeAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyPriceAttrName, buyElementYear.SharePriceAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyReductionAttrName, buyElementYear.ReductionAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyDocumentAttrName, buyElementYear.Document);
                                            nodeElement.ChildNodes[i].AppendChild(newBuyElement);
                                        }
                                        break;

                                    #endregion Buys

                                    #region Sales

                                    case 8:
                                        // Remove old sales
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        foreach (var saleElementYear in shareObject.AllSaleEntries.GetAllSalesOfTheShare())
                                        {
                                            XmlElement newSaleElement = xmlPortfolio.CreateElement(shareObject.SaleTagNamePre);
                                            newSaleElement.SetAttribute(shareObject.SaleDateAttrName, saleElementYear.DateAsStr);
                                            newSaleElement.SetAttribute(shareObject.SaleVolumeAttrName, saleElementYear.VolumeAsStr);
                                            newSaleElement.SetAttribute(shareObject.SaleBuyPriceAttrName, saleElementYear.BuyPriceAsStr);
                                            newSaleElement.SetAttribute(shareObject.SalePriceAttrName, saleElementYear.SalePriceAsStr);
                                            newSaleElement.SetAttribute(shareObject.SaleTaxAtSourceAttrName, saleElementYear.TaxAtSourceAsStr);
                                            newSaleElement.SetAttribute(shareObject.SaleCapitalGainsTaxAttrName, saleElementYear.CapitalGainsTaxAsStr);
                                            newSaleElement.SetAttribute(shareObject.SaleSolidarityTaxAttrName, saleElementYear.SolidarityTaxAsStr);
                                            newSaleElement.SetAttribute(shareObject.SaleDocumentAttrName, saleElementYear.Document);
                                            nodeElement.ChildNodes[i].AppendChild(newSaleElement);
                                        }
                                        break;

                                    #endregion Sales

                                    #region Costs

                                    case 9:
                                        // Remove old costs
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        // Remove old costs
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        foreach (var costElementYear in shareObject.AllCostsEntries.GetAllCostsOfTheShare())
                                        {
                                            XmlElement newCostElement = xmlPortfolio.CreateElement(shareObject.CostsTagNamePre);
                                            newCostElement.SetAttribute(shareObject.CostsBuyPartAttrName, costElementYear.CostOfABuyAsStr);
                                            newCostElement.SetAttribute(shareObject.CostsSalePartAttrName, costElementYear.CostOfASaleAsStr);
                                            newCostElement.SetAttribute(shareObject.CostsDateAttrName, costElementYear.CostDateAsStr);
                                            newCostElement.SetAttribute(shareObject.CostsValueAttrName, costElementYear.CostValueAsStr);
                                            newCostElement.SetAttribute(shareObject.BuyDocumentAttrName, costElementYear.CostDocument);
                                            nodeElement.ChildNodes[i].AppendChild(newCostElement);
                                        }
                                        break;

                                    #endregion Costs

                                    #region Dividends

                                    case 10:
                                        // Remove old dividends
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        ((XmlElement)nodeElement.ChildNodes[i]).SetAttribute(shareObject.DividendPayoutIntervalAttrName, shareObject.DividendPayoutIntervalAsStr);

                                        foreach (var dividendObject in shareObject.AllDividendEntries.GetAllDividendsOfTheShare())
                                        {
                                            XmlElement newDividendElement = xmlPortfolio.CreateElement(shareObject.DividendTagName);
                                            newDividendElement.SetAttribute(shareObject.DividendDateAttrName, dividendObject.DateTimeStr);
                                            newDividendElement.SetAttribute(shareObject.DividendRateAttrName, dividendObject.Rate);
                                            newDividendElement.SetAttribute(shareObject.DividendVolumeAttrName, dividendObject.Volume);

                                            newDividendElement.SetAttribute(shareObject.DividendTaxAtSourceAttrName, dividendObject.TaxAtSource);
                                            newDividendElement.SetAttribute(shareObject.DividendCapitalGainsTaxAttrName, dividendObject.CapitalGainsTax);
                                            newDividendElement.SetAttribute(shareObject.DividendSolidarityTaxAttrName, dividendObject.SolidarityTax);

                                            newDividendElement.SetAttribute(shareObject.DividendPriceAttrName, dividendObject.Price);
                                            newDividendElement.SetAttribute(shareObject.DividendDocumentAttrName, dividendObject.Document);

                                            // Foreign currency information
                                            XmlElement newForeignCurrencyElement = xmlPortfolio.CreateElement(shareObject.DividendTagNameForeignCu);

                                            newForeignCurrencyElement.SetAttribute(shareObject.DividendForeignCuFlagAttrName, dividendObject.EnableFCStr);

                                            if (dividendObject.EnableFC == CheckState.Checked)
                                            {
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendExchangeRatioAttrName, dividendObject.ExchangeRatio);
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendNameAttrName, dividendObject.CultureInfoFC.Name);
                                            }
                                            else
                                            {
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendExchangeRatioAttrName, 0.ToString());
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendNameAttrName, dividendObject.DividendCultureInfo.Name);

                                            }
                                            newDividendElement.AppendChild(newForeignCurrencyElement);

                                            nodeElement.ChildNodes[i].AppendChild(newDividendElement);
                                        }
                                        break;

                                    #endregion Dividends

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                // Add a new share
                if (nodeListShares.Count == 0)
                {
                    #region General

                    // Get root element
                    XmlNode rootPortfolio = xmlPortfolio.SelectSingleNode("Portfolio");

                    // Add new share
                    XmlNode newShareNode = xmlPortfolio.CreateNode(XmlNodeType.Element, "Share", null);

                    // Add attributes (WKN)
                    XmlAttribute xmlAttributeWKN = xmlPortfolio.CreateAttribute("WKN");
                    xmlAttributeWKN.Value = shareObject.WknAsStr;
                    newShareNode.Attributes.Append(xmlAttributeWKN);

                    // Add attributes (ShareName)
                    XmlAttribute xmlAttributeShareName = xmlPortfolio.CreateAttribute("Name");
                    xmlAttributeShareName.Value = shareObject.NameAsStr;
                    newShareNode.Attributes.Append(xmlAttributeShareName);

                    // Add child nodes (last update Internet)
                    XmlElement newLastUpdateInternet = xmlPortfolio.CreateElement("LastUpdateInternet");
                    // Add child inner text
                    XmlText lastUpdateInternetValue = xmlPortfolio.CreateTextNode(shareObject.LastUpdateInternet.ToShortDateString() + " " + shareObject.LastUpdateInternet.ToShortTimeString());
                    newShareNode.AppendChild(newLastUpdateInternet);
                    newShareNode.LastChild.AppendChild(lastUpdateInternetValue);

                    // Add child nodes (last update date)
                    XmlElement newLastUpdateDate = xmlPortfolio.CreateElement("LastUpdateShareDate");
                    // Add child inner text
                    XmlText lastUpdateValueDate = xmlPortfolio.CreateTextNode(shareObject.LastUpdateDate.ToShortDateString() + " " + shareObject.LastUpdateDate.ToShortTimeString());
                    newShareNode.AppendChild(newLastUpdateDate);
                    newShareNode.LastChild.AppendChild(lastUpdateValueDate);

                    // Add child nodes (last update time)
                    XmlElement newLastUpdateTime = xmlPortfolio.CreateElement("LastUpdateTime");
                    // Add child inner text
                    XmlText lastUpdateValueTime = xmlPortfolio.CreateTextNode(shareObject.LastUpdateTime.ToShortDateString() + " " + shareObject.LastUpdateTime.ToShortTimeString());
                    newShareNode.AppendChild(newLastUpdateTime);
                    newShareNode.LastChild.AppendChild(lastUpdateValueTime);

                    // Add child nodes (share price)
                    XmlElement newSharePrice = xmlPortfolio.CreateElement("SharePrice");
                    // Add child inner text
                    XmlText SharePrice = xmlPortfolio.CreateTextNode(shareObject.CurPriceAsStr);
                    newShareNode.AppendChild(newSharePrice);
                    newShareNode.LastChild.AppendChild(SharePrice);

                    // Add child nodes (share price before)
                    XmlElement newSharePriceBefore = xmlPortfolio.CreateElement("SharePriceBefore");
                    // Add child inner text
                    XmlText SharePriceBefore = xmlPortfolio.CreateTextNode(shareObject.PrevDayPriceAsStr);
                    newShareNode.AppendChild(newSharePriceBefore);
                    newShareNode.LastChild.AppendChild(SharePriceBefore);

                    // Add child nodes (website)
                    XmlElement newWebsite = xmlPortfolio.CreateElement("WebSite");
                    // Add child inner text
                    XmlText WebSite = xmlPortfolio.CreateTextNode(shareObject.WebSite);
                    newShareNode.AppendChild(newWebsite);
                    newShareNode.LastChild.AppendChild(WebSite);

                    // Add child nodes (culture)
                    XmlElement newCulture = xmlPortfolio.CreateElement("Culture");
                    // Add child inner text
                    XmlText Culture = xmlPortfolio.CreateTextNode(shareObject.CultureInfo.Name);
                    newShareNode.AppendChild(newCulture);
                    newShareNode.LastChild.AppendChild(Culture);

                    #endregion General

                    #region Buys / Sales / Costs / Dividends

                    // Add child nodes (buys)
                    XmlElement newBuys = xmlPortfolio.CreateElement("Buys");
                    newShareNode.AppendChild(newBuys);
                    foreach (var buyElementYear in shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                    {
                        XmlElement newBuyElement = xmlPortfolio.CreateElement(shareObject.BuyTagNamePre);
                        newBuyElement.SetAttribute(shareObject.BuyDateAttrName, buyElementYear.DateAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyVolumeAttrName, buyElementYear.VolumeAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyPriceAttrName, buyElementYear.SharePriceAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyReductionAttrName, buyElementYear.ReductionAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyDocumentAttrName, buyElementYear.Document);
                        newBuys.AppendChild(newBuyElement);
                    }

                    // Add child nodes (sales)
                    XmlElement newSales = xmlPortfolio.CreateElement("Sales");
                    newShareNode.AppendChild(newSales);

                    // Add child nodes (costs)
                    XmlElement newCosts = xmlPortfolio.CreateElement("Costs");
                    newShareNode.AppendChild(newCosts);
                    foreach (var costElementYear in shareObject.AllCostsEntries.GetAllCostsOfTheShare())
                    {
                        XmlElement newCostElement = xmlPortfolio.CreateElement(shareObject.CostsTagNamePre);
                        newCostElement.SetAttribute(shareObject.CostsBuyPartAttrName, costElementYear.CostOfABuyAsStr);
                        newCostElement.SetAttribute(shareObject.CostsDateAttrName, costElementYear.CostDateAsStr);
                        newCostElement.SetAttribute(shareObject.CostsValueAttrName, costElementYear.CostValueAsStr);
                        newCostElement.SetAttribute(shareObject.BuyDocumentAttrName, costElementYear.CostDocument);
                        newCosts.AppendChild(newCostElement);
                    }

                    // Add child nodes (dividend)
                    XmlElement newDividend = xmlPortfolio.CreateElement("Dividends");
                    newDividend.SetAttribute(shareObject.DividendPayoutIntervalAttrName, shareObject.DividendPayoutIntervalAsStr);
                    newShareNode.AppendChild(newDividend);

                    #endregion Buys / Sales / Costs / Dividends

                    // Add share name to XML
                    rootPortfolio.AppendChild(newShareNode);
                }

                // Close reader for saving
                xmlReaderPortfolio.Close();
                // Save settings
                xmlPortfolio.Save(strPortfolioFileName);
                // Create a new reader
                xmlReaderPortfolio = XmlReader.Create(strPortfolioFileName, xmlReaderSettingsPortfolio);
                xmlPortfolio.Load(strPortfolioFileName);
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }

            exception = null;
            return true;
        }

        #endregion Save share object

        #region Portfolio methods

        #region Portfolio performance values

        /// <summary>
        /// This function calculates the performance of the portfolio
        /// </summary>
        private void CalculatePortfolioPerformance()
        {
            if (PortfolioPurchaseValue + PortfolioSalePurchaseValue != 0)
            {
                PortfolioPerformanceValue = ( PortfolioFinalValue + PortfolioSalePurchaseValue) * 100 / ( PortfolioPurchaseValue + PortfolioSalePurchaseValue) - 100;
            }
#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculatePerformancePortfolio()");
            Console.WriteLine("PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine("PortfolioPerformanceValue: {0}", PortfolioPerformanceValue);
#endif
        }

        /// <summary>
        /// This function calculates the profit or loss of the portfolio
        /// </summary>
        private void CalculatePortfolioProfitLoss()
        {
            PortfolioProfitLossValue = PortfolioFinalValue - PortfolioPurchaseValue;

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculatePortfolioProfitLoss()");
            Console.WriteLine("PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine("PortfolioPurchaseValue: {0}", PortfolioPurchaseValue);
            Console.WriteLine("PortfolioProfitLossFinalValue: {0}", PortfolioProfitLossValue);
#endif
        }

        #endregion Portfolio performance values

        /// <summary>
        /// This function resets the portfolio values of the share objects
        /// </summary>
        static public void PortfolioValuesReset()
        {
            _portfolioFinalValue = 0;
            _portfolioCosts = 0;
            _portfolioDividend = 0;
            _portfolioPerformanceValue = 0;
            _portfolioProfitLossValue = 0;
        }

        #endregion Portfolio methods

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="bDisposing">Flag if the dispose is called</param>
        protected override void Dispose(bool bDisposing)
        {
            if (Disposed)
                return;

            if (bDisposing)
            {
                // Free any other managed objects here.
                // -
            }

            // Free any unmanaged objects here.
            Console.WriteLine("ShareObjectFinalValue destructor...");
            if (PurchaseValue > decimal.MinValue / 2)
                PortfolioPurchaseValue -= PurchaseValue;
            if (FinalValue > decimal.MinValue / 2)
                PortfolioFinalValue -= FinalValue;
            if (DividendValueTotal > decimal.MinValue / 2)
                PortfolioDividend -= DividendValueTotal;
            if (CostsValueTotal > decimal.MinValue / 2)
                PortfolioCosts -= CostsValueTotal;

            Disposed = true;

            // Call base class implementation.
            base.Dispose(bDisposing);
        }

        #endregion Methods
    }
}
