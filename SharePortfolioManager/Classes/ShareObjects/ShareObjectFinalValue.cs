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

using Parser;
using SharePortfolioManager.Classes.Costs;
using SharePortfolioManager.Classes.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;

namespace SharePortfolioManager.Classes.ShareObjects
{
    /// <inheritdoc>
    ///     <cref>ShareObject</cref>
    /// </inheritdoc>
    /// <summary>
    /// This class stores the final value of the share portfolio.
    /// It includes the dividends and brokerage.
    /// </summary>
    [Serializable]
    public class ShareObjectFinalValue : ShareObject, ICloneable
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Stores the current share value with dividends, brokerage, profits and loss (final value)
        /// </summary>
        private decimal _finalValue = decimal.MinValue / 2;

        /// <summary>
        /// Purchase value of the current share value without dividends, brokerage, profits and loss (market value)
        /// </summary>
        private decimal _purchaseValue = decimal.MinValue / 2;

        #endregion General variables

        #region Portfolio value variables

        /// <summary>
        /// Stores the purchase price of the portfolio (all shares) without dividends, brokerage, profits and loss (market value)
        /// </summary>
        private static decimal _portfolioPurchasePrice;

        /// <summary>
        /// Stores the purchase of sales of the portfolio (all shares)
        /// </summary>
        private static decimal _portfolioSalePurchaseValue;

        /// <summary>
        /// Stores the current value of the portfolio (all shares) with dividends, brokerage, profits and loss (final value)
        /// </summary>
        private static decimal _portfolioFinalValue;

        /// <summary>
        /// Stores the brokerage of the portfolio (all shares)
        /// </summary>
        private static decimal _portfolioBrokerage;

        /// <summary>
        /// Stores the dividends of the portfolio (all shares)
        /// </summary>
        private static decimal _portfolioDividend;

        /// <summary>
        /// Stores the performance of the portfolio (all shares) with dividends, brokerage, profits and loss
        /// </summary>
        private static decimal _portfolioPerformanceValue;

        /// <summary>
        /// Stores the profit or lose of the portfolio (all shares) with dividends, brokerage, profits and loss
        /// </summary>
        private static decimal _portfolioProfitLossValue;

        #endregion Portfolio value variables

        #endregion Variables

        #region Properties

        #region General properties

        /// <summary>
        /// Flag if the object is disposed
        /// </summary>
        [Browsable(false)]
        public new bool Disposed { get; internal set; }

        /// <inheritdoc />
        /// <summary>
        /// Current share volume
        /// </summary>
        [Browsable(false)]
        public override decimal Volume
        {
            get => base.Volume;
            set
            {
                // Set the new share volume
                if (value == base.Volume) return;

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

        /// <inheritdoc />
        /// <summary>
        /// Current price of the share. Will be updated via the Internet
        /// </summary>
        [Browsable(false)]
        public override decimal CurPrice
        {
            get => base.CurPrice;
            set
            {
                // Set the new share price
                if (value == base.CurPrice) return;

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

        /// <inheritdoc />
        /// <summary>
        /// Previous day price of the share. Will be updated via the Internet
        /// </summary>
        [Browsable(false)]
        public override decimal PrevDayPrice
        {
            get => base.PrevDayPrice;
            set
            {
                // Set the previous day share price
                if (value == base.PrevDayPrice) return;

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

        #region Purchase value properties

        /// <summary>
        /// Purchase value of the current share value without dividends, brokerage, profits and loss (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PurchaseValue
        {
            get => _purchaseValue;
            set
            {
                // Set the new purchase price value
                if (value == _purchaseValue) return;

                // Recalculate portfolio purchase price value
                if (_purchaseValue > decimal.MinValue / 2)
                    PortfolioPurchasePrice -= _purchaseValue;
                PortfolioPurchasePrice += value;

                _purchaseValue = value;

                // Recalculate the total final value of the share
                CalculateFinalValue();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();
            }
        }

        /// <summary>
        /// Purchase price of the current share volume as string
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueAsStr => Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Purchase price of the current share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueAsStrUnit => Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        #endregion Purchase value properties

        /// <inheritdoc />
        /// <summary>
        /// Total sale value of the share
        /// </summary>
        [Browsable(false)]
        public override decimal SalePurchaseValueTotal
        {
            get => base.SalePurchaseValueTotal;
            set
            {
                // Set the new purchase value
                if (value == base.SalePurchaseValueTotal) return;

                // Recalculate portfolio purchase value
                if (base.SalePurchaseValueTotal > decimal.MinValue / 2)
                    PortfolioSalePurchaseValue -= base.SalePurchaseValueTotal;
                PortfolioSalePurchaseValue += value;

                base.SalePurchaseValueTotal = value;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        [Browsable(false)]
        public override CultureInfo CultureInfo
        {
            get => base.CultureInfo;
            set
            {
                base.CultureInfo = value;

                // Set culture info to the lists
                AllBrokerageEntries?.SetCultureInfo(base.CultureInfo);
                AllDividendEntries?.SetCultureInfo(base.CultureInfo);
            }
        }

        #endregion General properties

        #region Brokerage properties

        /// <summary>
        /// Brokerage value of the final value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal BrokerageValueTotal { get; internal set; }

        /// <summary>
        /// Brokerage value of the final value of the share volume as string
        /// </summary>
        [Browsable(false)]
        public string BrokerageValueTotalAsStr => Helper.FormatDecimal(BrokerageValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// List of all brokerages of this share
        /// </summary>
        [Browsable(false)]
        public AllBrokerageReductionOfTheShare AllBrokerageEntries { get; set; } = new AllBrokerageReductionOfTheShare();

        #endregion Brokerage properties

        #region Dividends properties

        /// <summary>
        /// Dividend value of the final value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal DividendValueTotal { get; internal set; }

        /// <summary>
        ///  Dividend payout interval
        /// </summary>
        [Browsable(false)]
        public int DividendPayoutInterval { get; set; }

        /// <summary>
        ///  Dividend payout interval as string
        /// </summary>
        [Browsable(false)]
        public string DividendPayoutIntervalAsStr => DividendPayoutInterval.ToString();

        /// <summary>
        /// List of all dividends of this share
        /// </summary>
        [Browsable(false)]
        public AllDividendsOfTheShare AllDividendEntries { get; set; } = new AllDividendsOfTheShare();

        #endregion Dividend properties

        #region Brokerage and dividends with taxes

        /// <summary>
        /// Brokerage and dividends minus taxes of this share as string with unit and a line break
        /// </summary>
        [Browsable(false)]
        public string BrokerageDividendWithTaxesAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(BrokerageValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(DividendValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Brokerage and dividends with taxes

        #region Final value properties

        /// <summary>
        /// Current final value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal FinalValue
        {
            get => _finalValue;
            set
            {
                if (value == _finalValue) return;

#if DEBUG_SHAREOBJECT_FINAL
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

#if DEBUG_SHAREOBJECT_FINAL
                    Console.WriteLine(@"_finalValue: {0}", _finalValue);
                    Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
#endif
            }
        }

        /// <summary>
        /// Current final value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string FinalValueAsStrUnit => Helper.FormatDecimal(FinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Performance value of the final value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal PerformanceValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance value of the final value of the share volume as string with string
        /// </summary>
        [Browsable(false)]
        public string PerformanceValueAsStrUnit => Helper.FormatDecimal(PerformanceValue, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);

        /// <summary>
        /// Profit or loss value of the final value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal ProfitLossValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Profit or loss value of the final value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string ProfitLossValueAsStrUnit => Helper.FormatDecimal(ProfitLossValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Profit or loss and performance value of the final value of the share volume as sting with unit
        /// </summary>
        [Browsable(false)]
        public string ProfitLossPerformanceValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(ProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PerformanceValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Purchase value and final value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueFinalValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(FinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Final value properties

        #region Portfolio properties

        /// <summary>
        /// Purchase price of the hole portfolio (all share in the portfolio without dividends, brokerage, profits and loss (market value) )
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioPurchasePrice
        {
            get => _portfolioPurchasePrice;
            internal set
            {
                if (_portfolioPurchasePrice == value) return;

                _portfolioPurchasePrice = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Purchase value of the hole portfolio (all share in the portfolio without dividends, brokerage, profits and loss (market value) ) as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioPurchaseValueAsStrUnit => Helper.FormatDecimal(PortfolioPurchasePrice, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// The portfolio sale purchase value stores the
        /// purchase value which has been sold again.
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioSalePurchaseValue
        {
            get => _portfolioSalePurchaseValue;
            internal set
            {
                if (_portfolioSalePurchaseValue == value) return;

                _portfolioSalePurchaseValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Brokerage of the hole portfolio
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioBrokerage
        {
            get => _portfolioBrokerage;
            internal set => _portfolioBrokerage = value;
        }

        /// <summary>
        /// Dividend of the hole portfolio
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioDividend
        {
            get => _portfolioDividend;
            internal set => _portfolioDividend = value;
        }

        /// <summary>
        /// Brokerage and dividend of the hole portfolio as string with unit and line break
        /// </summary>
        [Browsable(false)]
        public string BrokerageDividendPortfolioValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioBrokerage, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioDividend, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Performance value of the hole portfolio
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioPerformanceValue
        {
            get => _portfolioPerformanceValue;
            internal set => _portfolioPerformanceValue = value;
        }

        /// <summary>
        /// Profit or loss value of the hole portfolio
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioProfitLossValue
        {
            get => _portfolioProfitLossValue;
            internal set => _portfolioProfitLossValue = value;
        }

        /// <summary>
        /// Profit or loss value and performance of the hole portfolio as string with unit and a line break
        /// </summary>
        [Browsable(false)]
        public string ProfitLossPerformancePortfolioValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioPerformanceValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Final value of the hole portfolio (all share in the portfolio with dividends, brokerage, profits and loss (final value) )
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioFinalValue
        {
            get => _portfolioFinalValue;
            internal set
            {
                if (_portfolioFinalValue == value) return;

                _portfolioFinalValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Final value of the hole portfolio (all share in the portfolio with dividends, brokerage, profits and loss (final value) ) as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioFinalValueAsStrUnit => Helper.FormatDecimal(PortfolioFinalValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        #endregion Portfolio properties

        #region Data grid view properties

        /// <summary>
        /// WKN of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Wkn")]
        // ReSharper disable once UnusedMember.Global
        public string DgvWkn => WknAsStr;

        /// <summary>
        /// Name of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Name")]
        // ReSharper disable once UnusedMember.Global
        public string DgvNameAsStr => NameAsStr;

        /// <summary>
        /// Volume of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Volume")]
        // ReSharper disable once UnusedMember.Global
        public string DgvVolumeAsStr => VolumeAsStr;

        /// <summary>
        /// Brokerage and dividend minus taxes of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"BrokerageDividendWithTaxes")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBrokerageDividendWithTaxesAsStrUnit => BrokerageDividendWithTaxesAsStrUnit;

        /// <summary>
        /// Previous day price of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"CurPrevPrice")]
        // ReSharper disable once UnusedMember.Global
        public string DgvCurPrevPriceAsStrUnit => CurPrevPriceAsStrUnit;

        /// <summary>
        /// Difference between the current and the previous day of the share and the performance in percent
        /// of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"PrevDayPerformance")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPrevDayDifferencePerformanceAsStrUnit => PrevDayDifferencePerformanceAsStrUnit;

        /// <summary>
        /// Image which indicates the performance of the share to the previous day for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"")]
        // ReSharper disable once UnusedMember.Global
        public Image DgvImagePrevDayPerformance => ImagePrevDayPerformance;

        /// <summary>
        /// Profit or loss and performance value of the market value of the share volume as sting with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"ProfitLossPerformanceFinalValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvProfitLossPerformanceValueAsStrUnit => ProfitLossPerformanceValueAsStrUnit;

        /// <summary>
        /// Purchase value and final value of the share volume as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"PurchaseValueFinalValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPurchaseValueFinalValueAsStrUnit => PurchaseValueFinalValueAsStrUnit;

        #endregion Data grid properties

        #endregion Properties

        #region Methods

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Standard constructor
        /// </summary>
        public ShareObjectFinalValue(List<Image> imageList, string percentageUnit, string pieceUnit) : base(imageList, percentageUnit, pieceUnit)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="guid">Guid of the buy</param>
        /// <param name="orderNumber">Order number of the buy</param>
        /// <param name="wkn">WKN number of the share</param>
        /// <param name="addDateTime">Date and time of the add</param>
        /// <param name="name">Name of the share</param>
        /// <param name="lastUpdateInternet">Date and time of the last update from the Internet</param>
        /// <param name="lastUpdateShareDate">Date of the last update on the Internet site of the share</param>
        /// <param name="lastUpdateShareTime">Time of the last update on the Internet site of the share</param>
        /// <param name="price">Current price of the share</param>
        /// <param name="volume">Volume of the share</param>
        /// <param name="volumeSold">Volume of the share which is already sold</param>
        /// <param name="provision">Provision of the buy</param>
        /// <param name="brokerFee">Broker fee of the buy</param>
        /// <param name="traderPlaceFee">Trader place fee of the buy</param>
        /// <param name="reduction">Reduction of the share</param>
        /// <param name="webSite">Website address of the share</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
        /// <param name="dividendPayoutInterval">Interval of the dividend payout</param>
        /// <param name="shareType">Type of the share</param>
        /// <param name="document">Document of the first buy</param>
        public ShareObjectFinalValue(
            string guid, string wkn, string orderNumber, string addDateTime, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShareDate, DateTime lastUpdateShareTime,
            decimal price, decimal volume, decimal volumeSold, decimal provision, decimal brokerFee, decimal traderPlaceFee, decimal reduction,
            string webSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            int dividendPayoutInterval, int shareType, string document)
            : base(wkn, addDateTime, name, lastUpdateInternet, lastUpdateShareDate, lastUpdateShareTime,
                    price, webSite, imageListForDayBeforePerformance,
                    regexList, cultureInfo, shareType)
        {
            BrokerageReductionObject tempBrokerageObject = null;
            // Check if a brokerage must be added
            if (provision > 0 || brokerFee > 0 || traderPlaceFee > 0 || reduction > 0)
            {
                // Generate Guid
                var strGuidBrokerage = Guid.NewGuid().ToString();

                tempBrokerageObject = new BrokerageReductionObject(strGuidBrokerage, true, false, cultureInfo, guid,
                    addDateTime,
                    provision, brokerFee, traderPlaceFee, reduction, document);
            }

            AddBuy(guid, orderNumber, AddDateTime, volume, volumeSold, price,
                tempBrokerageObject, document);

            DividendPayoutInterval = dividendPayoutInterval;
        }

        #endregion Constructors

        #region Destructor

        /// <inheritdoc />
        /// <summary>
        /// Destructor
        /// </summary>
        ~ShareObjectFinalValue()
        {
            Dispose(false);
        }

        #endregion Destructor

        #region Brokerage methods

        /// <summary>
        /// This function adds the brokerage for the share to the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="bBrokerageOfABuy">Flag if the brokerage is part of a buy</param>
        /// <param name="bBrokerageOfASale">Flag if the brokerage is part of a Sale</param>
        /// <param name="strGuidBuySale">Guid of the buy or sale</param>
        /// <param name="strDateTime">Date and time of the brokerage</param>
        /// <param name="decProvisionValue">Provision value</param>
        /// <param name="decBrokerFeeValue">Broker fee value</param>
        /// <param name="decTraderPlaceFeeValue">Trader place fee value</param>
        /// <param name="decReductionValue">Reduction value</param>
        /// <param name="strDoc">Doc of the brokerage</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBrokerage(string strGuid, bool bBrokerageOfABuy, bool bBrokerageOfASale, string strGuidBuySale,
            string strDateTime, decimal decProvisionValue, decimal decBrokerFeeValue, decimal decTraderPlaceFeeValue, decimal decReductionValue, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"");
                Console.WriteLine(@"AddBrokerage() / FinalValue");
                Console.WriteLine(@"bBrokerageOfABuy: {0}", bBrokerageOfABuy);
                Console.WriteLine(@"bBrokerageOfASale: {0}", bBrokerageOfASale);
                Console.WriteLine(@"strGuidBuySale: {0}", strGuidBuySale);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"decProvision: {0}", decProvisionValue);
                Console.WriteLine(@"decBrokerFee: {0}", decBrokerFeeValue);
                Console.WriteLine(@"decTraderPlaceFee: {0}", decTraderPlaceFeeValue);
                Console.WriteLine(@"decReduction: {0}", decReductionValue);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                // Remove current brokerage of the share from the brokerage of all shares
                PortfolioBrokerage -= BrokerageValueTotal;

                if (!AllBrokerageEntries.AddBrokerageReduction(strGuid, bBrokerageOfABuy, bBrokerageOfASale, strGuidBuySale,
                    strDateTime, decProvisionValue, decBrokerFeeValue, decTraderPlaceFeeValue, decReductionValue, strDoc))
                    return false;

                // Set brokerage of the share
                BrokerageValueTotal = AllBrokerageEntries.BrokerageWithReductionValueTotal /*.BrokerageValueTotal*/;

                // Add new brokerage of the share to the brokerage of all shares
                PortfolioBrokerage += BrokerageValueTotal;

                // Recalculate purchase price
                if (bBrokerageOfABuy == false && bBrokerageOfASale == false)
                {
                    if (PurchaseValue == decimal.MinValue / 2)
                        PurchaseValue = 0;
                    PurchaseValue += AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime).BrokerageReductionValue;
                }

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

#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"BrokerageValueTotal: {0}", BrokerageValueTotal);
                Console.WriteLine(@"");
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This function removes a brokerage for the share from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="strDateTime">Date of the brokerage remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveBrokerage(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveBrokerage() / FinalValue");
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get flag if brokerage is part of a buy
                bool bPartOfABuy = AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime).PartOfABuy;

                // Get flag if brokerage is part of a buy
                bool bPartOfASale = AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime).PartOfASale;

                // Get brokerage with reduction value
                decimal decBrokerageReductionValue = AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime).BrokerageReductionValue;

                // Remove current brokerage the share to the brokerage of all shares
                PortfolioBrokerage -= AllBrokerageEntries.BrokerageValueTotal;

                // Remove brokerage by date
                if (!AllBrokerageEntries.RemoveBrokerageReduction(strGuid, strDateTime))
                    return false;

                // Set brokerage of the share
                BrokerageValueTotal = AllBrokerageEntries.BrokerageValueTotal;

                // Add new brokerage of the share to the brokerage of all shares
                PortfolioBrokerage += AllBrokerageEntries.BrokerageValueTotal;


                // Recalculate purchase price
                if (bPartOfABuy == false && bPartOfASale == false)
                {
                    if (PurchaseValue == decimal.MinValue / 2)
                        PurchaseValue = 0;
                    PurchaseValue -= decBrokerageReductionValue;
                }

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

#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"BrokerageValueTotal: {0}", BrokerageValueTotal);
                Console.WriteLine(@"");
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Brokerage methods

        #region Buy methods

        /// <summary>
        /// This function adds the buy for the share to the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strOrderNumber">Order number of the buy</param>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <param name="decVolume">Buy volume</param>
        /// <param name="decVolumeSold">Buy volume which is already sold</param>
        /// <param name="decPrice">Price for one share</param>
        /// <param name="brokerageObject">Brokerage object of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strGuid, string strOrderNumber,  string strDateTime, decimal decVolume, decimal decVolumeSold, decimal decPrice,
            BrokerageReductionObject brokerageObject, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"");
                Console.WriteLine(@"AddBuy() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"decVolume: {0}", decVolume);
                Console.WriteLine(@"decPrice: {0}", decPrice);
                if (brokerageObject != null)
                {
                    Console.WriteLine(@"decProvision: {0}", brokerageObject.ProvisionValue);
                    Console.WriteLine(@"decBrokerFee: {0}", brokerageObject.BrokerFeeValue);
                    Console.WriteLine(@"decTraderPlaceFee: {0}", brokerageObject.TraderPlaceFeeValue);
                    Console.WriteLine(@"decReduction: {0}", brokerageObject.ReductionValue);
                }
                else
                {
                    Console.WriteLine(@"decProvision: {0}", 0);
                    Console.WriteLine(@"decBrokerFee: {0}", 0);
                    Console.WriteLine(@"decTraderPlaceFee: {0}", 0);
                    Console.WriteLine(@"decReduction: {0}", 0);
                }
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                // Add buy with reductions and brokerage
                if (!AllBuyEntries.AddBuy(strGuid, strOrderNumber, strDateTime, decVolume, decVolumeSold, decPrice,
                    brokerageObject, strDoc))
                    return false;

                // Set new volume
                Volume = AllBuyEntries.BuyVolumeTotal - AllSaleEntries.SaleVolumeTotal;

                // Recalculate purchase price
                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                PurchaseValue += AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValueBrokerageReduction;

#if DEBUG_SHAREOBJECT_FINAL || DEBUG_BUY
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"BuyValueTotal: {0}", PurchaseValueTotal);
                Console.WriteLine(@"PurchasePrice: {0}", PurchaseValue);
                Console.WriteLine(@"");
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
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDateTime">Date and time of the buy remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveBuy(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL || DEBUG_BUY
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveBuy() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get BuyObject by Guid and date and time and add the sale PurchaseValue and value to the share
                var buyObject = AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime);
                if (buyObject != null)
                {
                    // Remove buy by Guid and date and time
                    if (!AllBuyEntries.RemoveBuy(strGuid, strDateTime))
                        return false;

                    // Set volume
                    Volume = AllBuyEntries.BuyVolumeTotal - AllSaleEntries.SaleVolumeTotal;

                    PurchaseValue -= buyObject.BuyValueBrokerageReduction;

#if DEBUG_SHAREOBJECT_FINAL || DEBUG_BUY
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"BuyValueTotal: {0}", PurchaseValueTotal);
                    Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                    Console.WriteLine(@"");
#endif
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// This function sets the document of the buy of the given Guid and datetime
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be modified</param>
        /// <param name="strDateTime">Date time of the buy which should be modified</param>
        /// <param name="strDocument">Document which should be set</param>
        /// <returns></returns>
        public bool SetBuyDocument(string strGuid, string strDateTime, string strDocument)
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL || DEBUG_BUY
                Console.WriteLine(@"");
                Console.WriteLine(@"SetBuyDocument() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"strDocument: {0}", strDocument);
#endif
                // Get BuyObject by Guid and date and time and set the new document
                var buyObject = AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime);
                if (buyObject != null)
                {
                    // Set document of the buy by Guid and date and time
                    if (!AllBuyEntries.SetDocumentBuy(strGuid, strDateTime, strDocument))
                        return false;
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion Buy methods

        #region Sale methods

        /// <summary>
        /// This function adds the sale for the share to the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the share sale</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="strOrderNumber">Order number of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="usedBuyDetails">Details of the used buys for the sale</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="brokerageObject">Brokerage object of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strGuid, string strDate, string strOrderNumber, decimal decVolume, decimal decSalePrice, List<SaleBuyDetails> usedBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, BrokerageReductionObject brokerageObject, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"");
                Console.WriteLine(@"AddSale() / FinalValue");
                Console.WriteLine(@"strDateTime: {0}", strDate);
                Console.WriteLine(@"decVolume: {0}", decVolume);
                Console.WriteLine(@"decSalePrice: {0}", decSalePrice);
                Console.WriteLine(@"decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine(@"decCapitalGainsTax: {0}", decCapitalGainsTax);
                Console.WriteLine(@"decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine(@"decBrokerage: {0}", brokerageObject?.BrokerageValue ?? 0);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                if (!AllSaleEntries.AddSale(strGuid, strDate, strOrderNumber, decVolume, decSalePrice, usedBuyDetails, decTaxAtSource, decCapitalGainsTax,
                                            decSolidarityTax, brokerageObject, strDoc))
                    return false;
                
                // Set volume
                Volume = AllBuyEntries.BuyVolumeTotal - AllSaleEntries.SaleVolumeTotal;

                // Recalculate PurchaseValue and SalePurchaseValueTotal
                if (SalePurchaseValueTotal == decimal.MinValue / 2)
                    SalePurchaseValueTotal = 0;

                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                else
                {
                    if (Volume > 0 && PurchaseValue > 0)
                    {
                        SalePurchaseValueTotal += usedBuyDetails.Sum(x => x.SaleBuyValue);
                        PurchaseValue -= usedBuyDetails.Sum(x => x.SaleBuyValue);
                    }
                    else
                    {
                        SalePurchaseValueTotal += PurchaseValue;
                        PurchaseValue = 0;
                    }
                }

#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"SalePurchaseValueTotal: {0}", SalePurchaseValueTotal);
                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
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
        /// <param name="strGuid">Guid of the sale</param>
        /// <param name="strDateTime">Date and time of the sale remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveSale(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveSale() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get SaleObject by date and time and add the sale deposit and value to the share
                var saleObject = AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDateTime);
                if (saleObject != null)
                {
                    // Remove sale by Guid and DateTime
                    if (!AllSaleEntries.RemoveSale(strGuid, strDateTime))
                        return false;

                    // Set volume
                    Volume = AllBuyEntries.BuyVolumeTotal - AllSaleEntries.SaleVolumeTotal;

                    PurchaseValue += saleObject.BuyValue;
                    SalePurchaseValueTotal -= saleObject.BuyValue;

#if DEBUG_SHAREOBJECT_FINAL
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"SalePurchaseValueTotal: {0}", SalePurchaseValueTotal);
                    Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
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
        /// This function sets the document of the sale of the given Guid and datetime
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be modified</param>
        /// <param name="strDateTime">Date time of the buy which should be modified</param>
        /// <param name="strDocument">Document which should be set</param>
        /// <returns></returns>
        public bool SetSaleDocument(string strGuid, string strDateTime, string strDocument)
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL || DEBUG_SALE
                Console.WriteLine(@"");
                Console.WriteLine(@"SetSaleDocument() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"strDocument: {0}", strDocument);
#endif
                // Get SaleObject by Guid and date and time and set the new document
                var saleObject = AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDateTime);
                if (saleObject != null)
                {
                    // Set document of the sale by Guid and date and time
                    if (!AllSaleEntries.SetDocumentSale(strGuid, strDateTime, strDocument))
                        return false;
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion Sale methods

        #region Dividend methods

        /// <summary>
        /// This function adds the dividend payouts for the share to the dictionary
        /// </summary>
        /// <param name="cultureInfoFc">CultureInfo of the share for the foreign currency</param>
        /// <param name="csEnableFc">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency</param>
        /// <param name="strGuid">Guid of the dividend</param>
        /// <param name="strDate">Pay date of the new dividend list entry</param>
        /// <param name="decRate">Paid dividend of one share</param>
        /// <param name="decVolume">Share volume at the pay date</param>
        /// <param name="decTaxAtSource">Tax at source value</param>
        /// <param name="decCapitalGainsTax">Capital gains tax value</param>
        /// <param name="decSolidarityTax">Solidarity tax value</param>
        /// <param name="decSharePrice">Share price at the pay date</param>
        /// <param name="strDoc">Document of the dividend</param>
        /// <returns>Flag if the add was successful</returns>  
        public bool AddDividend(CultureInfo cultureInfoFc, CheckState csEnableFc, decimal decExchangeRatio, string strGuid, string strDate, decimal decRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax, decimal decSharePrice, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"");
                Console.WriteLine(@"AddDividend() / FinalValue");
                Console.WriteLine(@"EnableFC: {0}", csEnableFc);
                if (csEnableFc == CheckState.Checked)
                    Console.WriteLine(@"cultureInfoFC: {0}", cultureInfoFc);
                Console.WriteLine(@"decExchangeRatio: {0}", decExchangeRatio);
                Console.WriteLine(@"strDateTime: {0}", strDate);
                Console.WriteLine(@"decDividendRate: {0}", decRate);
                Console.WriteLine(@"decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine(@"decCapitalGainsTax: {0}", decCapitalGainsTax);
                Console.WriteLine(@"decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine(@"decShareVolume: {0}", decVolume);
                Console.WriteLine(@"decSharePrice: {0}", decSharePrice);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                if (!AllDividendEntries.AddDividend(cultureInfoFc, csEnableFc, decExchangeRatio, strGuid, strDate, decRate, decVolume,
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

#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"DividendValueTotal: {0}", DividendValueTotal);
                Console.WriteLine(@"");
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
        /// <param name="strGuid">Guid of the dividend</param>
        /// <param name="strDate">Date of the dividend pay date</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveDividend(string strGuid, string strDate)
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveDividend() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDate: {0}", strDate);
#endif
                // Set dividend of all shares
                PortfolioDividend -= DividendValueTotal;

                // Remove dividend by date
                if (!AllDividendEntries.RemoveDividend(strGuid, strDate))
                    return false;

                // Set dividend of the share
                DividendValueTotal = AllDividendEntries.DividendValueTotalWithTaxes;

                // Set dividend of all shares
                PortfolioDividend += DividendValueTotal;

                // Add new brokerage of the share to the brokerage of all shares
                PortfolioBrokerage += BrokerageValueTotal;

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

#if DEBUG_SHAREOBJECT_FINAL
                Console.WriteLine(@"DividendValueTotal: {0}", DividendValueTotal);
                Console.WriteLine(@"");
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
        /// with dividends and brokerage and sales profit or loss
        /// </summary>
        private void CalculateFinalValue()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                if (Volume > 0)
                {
                    FinalValue = CurPrice * Volume
                                 + AllDividendEntries.DividendValueTotalWithTaxes
                                 + AllSaleEntries.SaleProfitLossTotal;
                }
                else
                {
                    FinalValue = AllDividendEntries.DividendValueTotalWithTaxes
                                 + AllSaleEntries.SaleProfitLossTotal;
                }
            }

#if DEBUG_SHAREOBJECT_FINAL
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateFinalValue() / FinalValue");
            Console.WriteLine(@"CurrentPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"DividendValueTotal: {0}", AllDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine(@"SaleProfitLossTotal: {0}", AllSaleEntries.SaleProfitLossTotal);
            Console.WriteLine(@"FinalValue: {0}", FinalValue);
#endif
        }

        /// <summary>
        /// This function calculates the new profit or loss value of this share
        /// with dividends and brokerage and sales profit or loss
        /// </summary>
        private void CalculateProfitLoss()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllBrokerageEntries.BrokerageValueTotal > decimal.MinValue / 2
                && AllDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                if (Volume > 0)
                {
                    ProfitLossValue = CurPrice * Volume
                                      - PurchaseValue
                                      + AllDividendEntries.DividendValueTotalWithTaxes
                                      + AllSaleEntries.SaleProfitLossTotal;
                }
                else
                {
                    ProfitLossValue = AllDividendEntries.DividendValueTotalWithTaxes
                                      + AllSaleEntries.SaleProfitLossTotal;
                }
            }

#if DEBUG_SHAREOBJECT_FINAL
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateProfitLoss() / FinalValue");
            Console.WriteLine(@"CurPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine(@"BrokerageValueTotal: {0}", AllBrokerageEntries.BrokerageValueTotal);
            Console.WriteLine(@"DividendValueTotal: {0}", AllDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine(@"SaleProfitLossTotal: {0}", AllSaleEntries.SaleProfitLossTotal);
            Console.WriteLine(@"ProfitLossValue: {0}", ProfitLossValue);
#endif
        }

        /// <summary>
        /// This function calculates the new performance of this share
        /// with dividends and brokerage and sales profit or loss
        /// </summary>
        private void CalculatePerformance()
        {
            if (FinalValue <= decimal.MinValue / 2 || PurchaseValue <= decimal.MinValue / 2) return;

            if ((PurchaseValue + AllSaleEntries.SalePurchaseValueTotal) != 0)
                PerformanceValue = ( (FinalValue + AllSaleEntries.SalePurchaseValueTotal) * 100) / (PurchaseValue + AllSaleEntries.SalePurchaseValueTotal) - 100;

#if DEBUG_SHAREOBJECT_FINAL
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformance() / FinalValue");
            Console.WriteLine(@"FinalValue: {0}", FinalValue);
            Console.WriteLine(@"SalePurchaseValueTotal: {0}",  AllSaleEntries.SalePurchaseValueTotal);
            Console.WriteLine(@"PurchaseValue: {0}",  PurchaseValue);
            Console.WriteLine(@"PerformanceValue: {0}", PerformanceValue);
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
                var nodeListShares = xmlPortfolio.SelectNodes($"/Portfolio/Share [@WKN = \"{shareObject.Wkn}\"]");
                if (nodeListShares != null)
                {
                    foreach (XmlNode nodeElement in nodeListShares)
                    {
                        if (!nodeElement.HasChildNodes || nodeElement.ChildNodes.Count != ShareObjectTagCount)
                        {
                            exception = null;
                            return false;
                        }

                        nodeElement.Attributes[GeneralWknAttrName].InnerText = shareObject.Wkn;
                        nodeElement.Attributes[GeneralNameAttrName].InnerText = shareObject.NameAsStr;
                        nodeElement.Attributes[GeneralUpdateAttrName].InnerText = shareObject.UpdateAsStr;

                        for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                        {
                            switch (i)
                            {
                                #region General

                                case (int)FrmMain.PortfolioParts.LastInternetUpdate:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateInternet.ToShortDateString()} {
                                            shareObject.LastUpdateInternet.ToShortTimeString()
                                        }";
                                    break;
                                case (int)FrmMain.PortfolioParts.LastUpdateShareDate:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateDate.ToShortDateString()} {
                                            shareObject.LastUpdateDate.ToShortTimeString()
                                        }";
                                    break;
                                case (int)FrmMain.PortfolioParts.LastUpdateTime:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateTime.ToShortDateString()} {
                                            shareObject.LastUpdateTime.ToShortTimeString()
                                        }";
                                    break;
                                case (int)FrmMain.PortfolioParts.SharePrice:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.CurPriceAsStr;
                                    break;
                                case (int)FrmMain.PortfolioParts.SharePriceBefore:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.PrevDayPriceAsStr;
                                    break;
                                case (int)FrmMain.PortfolioParts.WebSite:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.WebSite;
                                    break;
                                case (int)FrmMain.PortfolioParts.Culture:
                                    nodeElement.ChildNodes[i].InnerXml = shareObject.CultureInfoAsStr;
                                    break;
                                case (int)FrmMain.PortfolioParts.ShareType:
                                    nodeElement.ChildNodes[i].InnerXml = shareObject.ShareType.ToString();
                                    break;

                                #endregion General

                                #region Brokerage

                                case (int)FrmMain.PortfolioParts.Brokerages:
                                    // Remove old brokerage
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    // Remove old brokerage
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var brokerageElementYear in shareObject.AllBrokerageEntries
                                        .GetAllBrokerageOfTheShare())
                                    {
                                        var newBrokerageElement =
                                            xmlPortfolio.CreateElement(BrokerageTagNamePre);
                                        newBrokerageElement.SetAttribute(BrokerageGuidAttrName,
                                            brokerageElementYear.Guid);
                                        newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName,
                                            brokerageElementYear.PartOfABuyAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageSalePartAttrName,
                                            brokerageElementYear.PartOfASaleAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageGuidBuySaleAttrName,
                                            brokerageElementYear.GuidBuySale);
                                        newBrokerageElement.SetAttribute(BrokerageDateAttrName,
                                            brokerageElementYear.DateAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageProvisionAttrName,
                                            brokerageElementYear.ProvisionValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageBrokerFeeAttrName,
                                            brokerageElementYear.BrokerFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageTraderPlaceFeeAttrName,
                                            brokerageElementYear.TraderPlaceFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageReductionAttrName,
                                            brokerageElementYear.ReductionValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageDocumentAttrName,
                                            brokerageElementYear.BrokerageDocument);
                                        nodeElement.ChildNodes[i].AppendChild(newBrokerageElement);
                                    }
                                    break;

                                #endregion Brokerage

                                #region Buys

                                case (int)FrmMain.PortfolioParts.Buys:
                                    // Remove old buys
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var buyElementYear in shareObject.AllBuyEntries
                                        .GetAllBuysOfTheShare())
                                    {
                                        var newBuyElement =
                                            xmlPortfolio.CreateElement(BuyTagNamePre);
                                        newBuyElement.SetAttribute(BuyGuidAttrName,
                                            buyElementYear.Guid);
                                        newBuyElement.SetAttribute(BuyOrderNumberAttrName,
                                            buyElementYear.OrderNumber);
                                        newBuyElement.SetAttribute(BuyDateAttrName,
                                            buyElementYear.DateAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeAttrName,
                                            buyElementYear.VolumeAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeSoldAttrName,
                                            buyElementYear.VolumeSoldAsStr);
                                        newBuyElement.SetAttribute(BuyPriceAttrName,
                                            buyElementYear.PriceAsStr);
                                        newBuyElement.SetAttribute(BuyBrokerageGuidAttrName,
                                            buyElementYear.BrokerageGuid);
                                        newBuyElement.SetAttribute(BuyDocumentAttrName,
                                            buyElementYear.Document);
                                        nodeElement.ChildNodes[i].AppendChild(newBuyElement);
                                    }
                                    break;

                                #endregion Buys

                                #region Sales

                                case (int)FrmMain.PortfolioParts.Sales:
                                    // Remove old sales
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var saleElementYear in shareObject.AllSaleEntries
                                        .GetAllSalesOfTheShare())
                                    {
                                        var newSaleElement =
                                            xmlPortfolio.CreateElement(SaleTagNamePre);
                                        newSaleElement.SetAttribute(SaleGuidAttrName,
                                            saleElementYear.Guid);
                                        newSaleElement.SetAttribute(SaleDateAttrName,
                                            saleElementYear.DateAsStr);
                                        newSaleElement.SetAttribute(SaleOrderNumberAttrName,
                                            saleElementYear.OrderNumberAsStr);
                                        newSaleElement.SetAttribute(SaleVolumeAttrName,
                                            saleElementYear.VolumeAsStr);
                                        newSaleElement.SetAttribute(SaleSalePriceAttrName,
                                            saleElementYear.SalePriceAsStr);
                                        newSaleElement.SetAttribute(SaleTaxAtSourceAttrName,
                                            saleElementYear.TaxAtSourceAsStr);
                                        newSaleElement.SetAttribute(SaleCapitalGainsTaxAttrName,
                                            saleElementYear.CapitalGainsTaxAsStr);
                                        newSaleElement.SetAttribute(SaleSolidarityTaxAttrName,
                                            saleElementYear.SolidarityTaxAsStr);
                                        newSaleElement.SetAttribute(SaleReductionAttrName,
                                            saleElementYear.ReductionAsStr);
                                        newSaleElement.SetAttribute(SaleBrokerageGuidAttrName,
                                            saleElementYear.BrokerageGuid);
                                        newSaleElement.SetAttribute(SaleDocumentAttrName,
                                            saleElementYear.Document);

                                        // Used buy details
                                        XmlElement newUsedBuysElement =
                                            xmlPortfolio.CreateElement(SaleUsedBuysAttrName);

                                        if (saleElementYear.SaleBuyDetails.Count > 0)
                                        {
                                            foreach (var usedBuys in saleElementYear.SaleBuyDetails)
                                            {
                                                XmlElement newUsedBuyElement =
                                                    xmlPortfolio.CreateElement(SaleUsedBuyAttrName);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyDateAttrName,
                                                    usedBuys.StrDateTime);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyVolumeAttrName,
                                                    usedBuys.SaleBuyVolumeAsStr);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyPriceAttrName,
                                                    usedBuys.SaleBuyPriceAsStr);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBrokerageReductionAttrName,
                                                    usedBuys.BrokerageReductionPartAsStr);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyGuidAttrName,
                                                    usedBuys.BuyGuid);
                                                
                                                // Add buy of the sale to the used buys
                                                newUsedBuysElement.AppendChild(newUsedBuyElement);
                                            }
                                        }

                                        // Add used buys to the sale
                                        newSaleElement.AppendChild(newUsedBuysElement);

                                        // Add new sale to the share
                                        nodeElement.ChildNodes[i].AppendChild(newSaleElement);
                                    }
                                    break;

                                #endregion Sales

                                #region Dividends

                                case (int)FrmMain.PortfolioParts.Dividends:
                                    // Remove old dividends
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    ((XmlElement) nodeElement.ChildNodes[i]).SetAttribute(
                                        DividendPayoutIntervalAttrName,
                                        shareObject.DividendPayoutIntervalAsStr);

                                    foreach (var dividendObject in shareObject.AllDividendEntries
                                        .GetAllDividendsOfTheShare())
                                    {
                                        var newDividendElement =
                                            xmlPortfolio.CreateElement(DividendTagName);
                                        newDividendElement.SetAttribute(DividendGuidAttrName,
                                            dividendObject.Guid);
                                        newDividendElement.SetAttribute(DividendDateAttrName,
                                            dividendObject.DateStr);
                                        newDividendElement.SetAttribute(DividendRateAttrName,
                                            dividendObject.Rate);
                                        newDividendElement.SetAttribute(DividendVolumeAttrName,
                                            dividendObject.Volume);

                                        newDividendElement.SetAttribute(DividendTaxAtSourceAttrName,
                                            dividendObject.TaxAtSource);
                                        newDividendElement.SetAttribute(
                                            DividendCapitalGainsTaxAttrName,
                                            dividendObject.CapitalGainsTax);
                                        newDividendElement.SetAttribute(
                                            DividendSolidarityTaxAttrName,
                                            dividendObject.SolidarityTax);

                                        newDividendElement.SetAttribute(DividendPriceAttrName,
                                            dividendObject.Price);
                                        newDividendElement.SetAttribute(DividendDocumentAttrName,
                                            dividendObject.Document);

                                        // Foreign currency information
                                        XmlElement newForeignCurrencyElement =
                                            xmlPortfolio.CreateElement(DividendTagNameForeignCu);

                                        newForeignCurrencyElement.SetAttribute(
                                            DividendForeignCuFlagAttrName,
                                            dividendObject.EnableFcStr);

                                        if (dividendObject.EnableFc == CheckState.Checked)
                                        {
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendExchangeRatioAttrName,
                                                dividendObject.ExchangeRatio);
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendNameAttrName,
                                                dividendObject.CultureInfoFc.Name);
                                        }
                                        else
                                        {
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendExchangeRatioAttrName, 0.ToString());
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendNameAttrName,
                                                dividendObject.DividendCultureInfo.Name);
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

                    // Add a new share
                    if (nodeListShares.Count == 0)
                    {
                        #region General

                        // Get root element
                        var rootPortfolio = xmlPortfolio.SelectSingleNode(@"Portfolio");

                        // Add new share
                        var newShareNode = xmlPortfolio.CreateNode(XmlNodeType.Element, "Share", null);

                        // Add attributes (WKN)
                        var xmlAttributeWkn = xmlPortfolio.CreateAttribute(@"WKN");
                        xmlAttributeWkn.Value = shareObject.WknAsStr;
                        newShareNode.Attributes.Append(xmlAttributeWkn);

                        // Add attributes (ShareName)
                        var xmlAttributeShareName = xmlPortfolio.CreateAttribute(@"Name");
                        xmlAttributeShareName.Value = shareObject.NameAsStr;
                        newShareNode.Attributes.Append(xmlAttributeShareName);

                        // Add attributes (Update)
                        var xmlAttributeUpdateFlag = xmlPortfolio.CreateAttribute(@"Update");
                        xmlAttributeUpdateFlag.Value = shareObject.UpdateAsStr;
                        newShareNode.Attributes.Append(xmlAttributeUpdateFlag);

                        // Add child nodes (last update Internet)
                        var newLastUpdateInternet = xmlPortfolio.CreateElement(@"LastUpdateInternet");
                        // Add child inner text
                        var lastUpdateInternetValue = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateInternet.ToShortDateString() + " " +
                            shareObject.LastUpdateInternet.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateInternet);
                        newShareNode.LastChild.AppendChild(lastUpdateInternetValue);

                        // Add child nodes (last update date)
                        var newLastUpdateDate = xmlPortfolio.CreateElement(@"LastUpdateShareDate");
                        // Add child inner text
                        var lastUpdateValueDate = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateDate.ToShortDateString() + " " +
                            shareObject.LastUpdateDate.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateDate);
                        newShareNode.LastChild.AppendChild(lastUpdateValueDate);

                        // Add child nodes (last update time)
                        var newLastUpdateTime = xmlPortfolio.CreateElement(@"LastUpdateTime");
                        // Add child inner text
                        var lastUpdateValueTime = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateTime.ToShortDateString() + " " +
                            shareObject.LastUpdateTime.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateTime);
                        newShareNode.LastChild.AppendChild(lastUpdateValueTime);

                        // Add child nodes (share price)
                        var newSharePrice = xmlPortfolio.CreateElement(@"SharePrice");
                        // Add child inner text
                        var sharePrice = xmlPortfolio.CreateTextNode(shareObject.CurPriceAsStr);
                        newShareNode.AppendChild(newSharePrice);
                        newShareNode.LastChild.AppendChild(sharePrice);

                        // Add child nodes (share price before)
                        var newSharePriceBefore = xmlPortfolio.CreateElement(@"SharePriceBefore");
                        // Add child inner text
                        var sharePriceBefore = xmlPortfolio.CreateTextNode(shareObject.PrevDayPriceAsStr);
                        newShareNode.AppendChild(newSharePriceBefore);
                        newShareNode.LastChild.AppendChild(sharePriceBefore);

                        // Add child nodes (website)
                        var newWebsite = xmlPortfolio.CreateElement(@"WebSite");
                        // Add child inner text
                        var webSite = xmlPortfolio.CreateTextNode(shareObject.WebSite);
                        newShareNode.AppendChild(newWebsite);
                        newShareNode.LastChild.AppendChild(webSite);

                        // Add child nodes (culture)
                        var newCulture = xmlPortfolio.CreateElement(@"Culture");
                        // Add child inner text
                        var culture = xmlPortfolio.CreateTextNode(shareObject.CultureInfo.Name);
                        newShareNode.AppendChild(newCulture);
                        newShareNode.LastChild.AppendChild(culture);

                        // Add child nodes (share type)
                        var newShareType = xmlPortfolio.CreateElement(@"ShareType");
                        // Add child inner text
                        var shareType = xmlPortfolio.CreateTextNode(shareObject.ShareType.ToString());
                        newShareNode.AppendChild(newShareType);
                        newShareNode.LastChild.AppendChild(shareType);

                        #endregion General

                        #region Buys / Sales / Brokerage / Dividends

                        // Add child nodes (brokerage)
                        var newBrokerage = xmlPortfolio.CreateElement(@"Brokerage");
                        newShareNode.AppendChild(newBrokerage);
                        foreach (var brokerageElementYear in shareObject.AllBrokerageEntries.GetAllBrokerageOfTheShare())
                        {
                            var newBrokerageElement = xmlPortfolio.CreateElement(BrokerageTagNamePre);
                            newBrokerageElement.SetAttribute(BrokerageGuidAttrName, brokerageElementYear.Guid);
                            newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName,
                                brokerageElementYear.PartOfABuyAsStr);
                            newBrokerageElement.SetAttribute(BrokerageDateAttrName, brokerageElementYear.DateAsStr);
                            newBrokerageElement.SetAttribute(BrokerageProvisionAttrName, brokerageElementYear.ProvisionValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageBrokerFeeAttrName, brokerageElementYear.BrokerFeeValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageTraderPlaceFeeAttrName, brokerageElementYear.TraderPlaceFeeValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageReductionAttrName, brokerageElementYear.ReductionValueAsStr);
                            newBrokerageElement.SetAttribute(BuyDocumentAttrName, brokerageElementYear.BrokerageDocument);
                            newBrokerage.AppendChild(newBrokerageElement);
                        }

                        // Add child nodes (buys)
                        var newBuys = xmlPortfolio.CreateElement(@"Buys");
                        newShareNode.AppendChild(newBuys);
                        foreach (var buyElementYear in shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                        {
                            var newBuyElement = xmlPortfolio.CreateElement(BuyTagNamePre);
                            newBuyElement.SetAttribute(BuyGuidAttrName, buyElementYear.Guid);
                            newBuyElement.SetAttribute(BuyOrderNumberAttrName, buyElementYear.OrderNumber);
                            newBuyElement.SetAttribute(BuyDateAttrName, buyElementYear.DateAsStr);
                            newBuyElement.SetAttribute(BuyVolumeAttrName, buyElementYear.VolumeAsStr);
                            newBuyElement.SetAttribute(BuyVolumeSoldAttrName, @"0");
                            newBuyElement.SetAttribute(BuyPriceAttrName, buyElementYear.PriceAsStr);
                            newBuyElement.SetAttribute(BuyBrokerageGuidAttrName, buyElementYear.BrokerageGuid);
                            newBuyElement.SetAttribute(BuyDocumentAttrName, buyElementYear.Document);
                            newBuys.AppendChild(newBuyElement);
                        }

                        // Add child nodes (sales)
                        var newSales = xmlPortfolio.CreateElement(@"Sales");
                        newShareNode.AppendChild(newSales);

                        // Add child nodes (dividend)
                        var newDividend = xmlPortfolio.CreateElement(@"Dividends");
                        newDividend.SetAttribute(DividendPayoutIntervalAttrName,
                            shareObject.DividendPayoutIntervalAsStr);
                        newShareNode.AppendChild(newDividend);

                        #endregion Buys / Sales / Brokerage / Dividends

                        // Add share name to XML
                        rootPortfolio.AppendChild(newShareNode);
                    }
                }
                else
                {
                    exception = null;
                    return false;
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
            if (PortfolioPurchasePrice != 0)
            {
                PortfolioPerformanceValue = (PortfolioFinalValue) * 100 / (PortfolioPurchasePrice) - 100;
            }
            
#if DEBUG_SHAREOBJECT_FINAL
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformancePortfolio() / FinalValue");
            Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine(@"PortfolioSalePurchaseValue: {0}", PortfolioSalePurchaseValue);
            Console.WriteLine(@"PortfolioPurchasePrice: {0}", PortfolioPurchasePrice);
            Console.WriteLine(@"PortfolioPerformanceValue: {0}", PortfolioPerformanceValue);
#endif
        }

        /// <summary>
        /// This function calculates the profit or loss of the portfolio
        /// </summary>
        private void CalculatePortfolioProfitLoss()
        {
            PortfolioProfitLossValue = PortfolioFinalValue - PortfolioPurchasePrice;

#if DEBUG_SHAREOBJECT_FINAL
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePortfolioProfitLoss() / FinalValue");
            Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine(@"PortfolioSalePurchaseValue: {0}", PortfolioSalePurchaseValue);
            Console.WriteLine(@"PortfolioPurchasePrice: {0}", PortfolioPurchasePrice);
            Console.WriteLine(@"PortfolioProfitLossFinalValue: {0}", PortfolioProfitLossValue);
#endif
        }

        #endregion Portfolio performance values

        /// <summary>
        /// This function resets the portfolio values of the share objects
        /// </summary>
        public static void PortfolioValuesReset()
        {
            _portfolioPurchasePrice = 0;
            _portfolioSalePurchaseValue = 0;
            _portfolioFinalValue = 0;
            _portfolioBrokerage = 0;
            _portfolioDividend = 0;
            _portfolioPerformanceValue = 0;
            _portfolioProfitLossValue = 0;
        }

        #endregion Portfolio methods

        /// <inheritdoc />
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

#if SHAREOBJECT_DEBUG
            Console.WriteLine(@"ShareObjectFinalValue destructor...");
#endif

            if (PurchaseValue > decimal.MinValue / 2)
                PortfolioPurchasePrice -= PurchaseValue;
            if (FinalValue > decimal.MinValue / 2)
                PortfolioFinalValue -= FinalValue;
            if (DividendValueTotal > decimal.MinValue / 2)
                PortfolioDividend -= DividendValueTotal;
            if (BrokerageValueTotal > decimal.MinValue / 2)
                PortfolioBrokerage -= BrokerageValueTotal;

            Disposed = true;

            // Call base class implementation.
            base.Dispose(bDisposing);
        }

        /// <inheritdoc />
        /// <summary>
        /// This function allows to clone this object
        /// </summary>
        /// <returns>New clone of the object</returns>
        public new object Clone()
        {
            using (var stream = new MemoryStream())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                if (!GetType().IsSerializable) return null;

                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        #endregion Methods
    }
}
