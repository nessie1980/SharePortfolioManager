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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Xml;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using WebParser;

namespace SharePortfolioManager
{
    /// <inheritdoc />
    /// <summary>
    /// This class is for the market value management of a share.
    /// The class stores the following things:
    /// - general information of the share
    /// - all buys of a share
    /// - all sales of a share
    /// - all costs of the sales and buys of a share
    /// </summary>
    public class ShareObjectMarketValue : ShareObject
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Stores the current total share market value without dividends, costs, profits and loss
        /// </summary>
        private decimal _marketValue = decimal.MinValue / 2;

        #endregion General variables

        #region Costs variables

        #endregion Costs variables

        #region Portfolio value variables

        /// <summary>
        /// Stores the value of the portfolio (all shares) without dividends, costs, profits and loss
        /// </summary>
        private static decimal _portfolioPurchaseValue;

        /// <summary>
        /// Stores the purchase of sales of the portfolio (all shares)
        /// </summary>
        private static decimal _portfolioSalePurchaseValue;

        /// <summary>
        /// Stores the current value of the portfolio (all shares) without dividends, costs, profits and loss
        /// </summary>
        private static decimal _portfolioMarketValue;

        /// <summary>
        /// Stores the costs of the portfolio (all shares)
        /// </summary>
        private static decimal _portfolioCosts;

        /// <summary>
        /// Stores the performance of the portfolio (all shares) without dividends, costs, profits and loss
        /// </summary>
        private static decimal _portfolioPerformanceValue;

        /// <summary>
        /// Stores the profit or loss of the portfolio (all shares) without dividends, costs, profits and loss
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

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the total sum of the share
                CalculateMarketValue();
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
                CalculateMarketValue();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();
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
                // Set the new share price
                if (value == base.PrevDayPrice) return;

                base.PrevDayPrice = value;

                // Recalculate the performance to the previous day
                CalculatePrevDayPerformance();

                // Recalculate the profit or loss to the previous day
                CalculatePrevDayProfitLoss();

                // Recalculate the total sum of the share
                CalculateMarketValue();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Purchase value of the current share volume
        /// </summary>
        [Browsable(false)]
        public override decimal PurchaseValue
        {
            get => base.PurchaseValue;
            set
            {
                // Set the new share price
                if (value == base.PurchaseValue) return;

                // Recalculate portfolio purchase value
                if (base.PurchaseValue > decimal.MinValue / 2)
                    PortfolioPurchaseValue -= base.PurchaseValue;
                PortfolioPurchaseValue += value;

                base.PurchaseValue = value;

                // Recalculate the total sum of the share
                CalculateMarketValue();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();
            }
        }

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
            set => base.CultureInfo = value;
        }

        #endregion General properties

        #region Costs properties

        /// <summary>
        /// Cost value of the market value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal CostsValueTotal { get; internal set; }

        /// <summary>
        /// Cost value of the market value of the share volume as string
        /// </summary>
        [Browsable(false)]
        public string CostsValueTotalAsStr => Helper.FormatDecimal(CostsValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Cost value of the market value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string CostsValueAsStrUnit => Helper.FormatDecimal(CostsValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// List of all costs of this share
        /// </summary>
        [Browsable(false)]
        public AllCostsOfTheShare AllCostsEntries { get; set; } = new AllCostsOfTheShare();

        #endregion Costs properties

        #region Market value properties

        /// <summary>
        /// Current market value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal MarketValue
        {
            get { return _marketValue; }
            set
            {
                if (value == _marketValue) return;

#if DEBUG_SHAREOBJECT
                    Console.WriteLine(@"");
                    Console.WriteLine(@"_marketValue: {0}", _marketValue);
                    Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
#endif
                // Recalculate the total sum of all shares
                // by subtracting the old total share value and then add the new value
                if (_marketValue > decimal.MinValue / 2)
                    PortfolioMarketValue -= _marketValue;
                PortfolioMarketValue += value;

                // Set the total share volume
                _marketValue = value;

#if DEBUG_SHAREOBJECT
                    Console.WriteLine(@"_marketValue: {0}", _marketValue);
                    Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
#endif
            }
        }

        /// <summary>
        /// Current market value of the share volume as string
        /// </summary>
        [Browsable(false)]
        public string MarketValueAsStr => Helper.FormatDecimal(MarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Current market value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string MarketValueAsStrUnit => Helper.FormatDecimal(MarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Performance value of the market value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal PerformanceValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance value of the market value of the share volume as string
        /// </summary>
        [Browsable(false)]
        public string PerformanceValueAsStr => Helper.FormatDecimal(PerformanceValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Performance value of the market value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string PerformanceValueAsStrUnit => Helper.FormatDecimal(PerformanceValue, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);

        /// <summary>
        /// Profit or loss value of the market value of the share volume
        /// </summary>
        [Browsable(false)]
        public decimal ProfitLossValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Profit or loss value of the market value of the share volume as string
        /// </summary>
        [Browsable(false)]
        public string ProfitLossValueAsStr => Helper.FormatDecimal(ProfitLossValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Profit or loss value of the market value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string ProfitLossValueAsStrUnit => Helper.FormatDecimal(ProfitLossValue, Helper.Currencythreelength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Profit or loss and performance value of the market value of the share volume as sting with unit
        /// </summary>
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

        /// <summary>
        /// Purchase value and market value of the share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueMarketValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(MarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Market value properties

        #region Portfolio value properties

        /// <summary>
        /// Purchase value of the hole portfolio (all share in the portfolio)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioPurchaseValue
        {
            get => _portfolioPurchaseValue;
            internal set
            {
                if (_portfolioPurchaseValue == value) return;

                _portfolioPurchaseValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Purchase value of the hole portfolio (all share in the portfolio) as string
        /// </summary>
        [Browsable(false)]
        public string PortfolioPurchaseValueAsStr => Helper.FormatDecimal(PortfolioPurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Purchase value of the hole portfolio (all share in the portfolio) as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioPurchaseValueAsStrUnit => Helper.FormatDecimal(PortfolioPurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

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

                // RecalcuSalelate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Portfolio sale purchase value as string
        /// </summary>
        [Browsable(false)]
        public string PortfolioSalePurchaseValueAsStr => Helper.FormatDecimal(PortfolioSalePurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Portfolio sale purchase value as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioSalePurchaseValueAsStrUnit => Helper.FormatDecimal(PortfolioSalePurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Costs of the hole portfolio
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCosts
        {
            get => _portfolioCosts;
            internal set => _portfolioCosts = value;
        }

        /// <summary>
        /// Costs of the portfolio as string
        /// </summary>
        [Browsable(false)]
        public string PortfolioCostsAsStr => Helper.FormatDecimal(PortfolioCosts, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Costs of the portfolio as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioCostsAsStrUnit => Helper.FormatDecimal(PortfolioCosts, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

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
        /// Performance value of the hole portfolio as string
        /// </summary>
        [Browsable(false)]
        public string PortfolioPerformanceValueAsStr => Helper.FormatDecimal(PortfolioPerformanceValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Performance value of the hole portfolio as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioPerformanceValueAsStrUnit => Helper.FormatDecimal(PortfolioPerformanceValue, Helper.Percentagetwolength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);

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
        /// Profit or loss value of the hole portfolio as string
        /// </summary>
        [Browsable(false)]
        public string PortfolioProfitLossValueAsStr => Helper.FormatDecimal(PortfolioProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Profit or loss value of the hole portfolio as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioProfitLossValueAsStrUnit => Helper.FormatDecimal(PortfolioProfitLossValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Profit or loss value and performance of the hole portfolio as string with unit and a line break
        /// </summary>
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

        /// <summary>
        /// Market value of the hole portfolio
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioMarketValue
        {
            get => _portfolioMarketValue;
            internal set
            {
                if (_portfolioMarketValue == value) return;

                _portfolioMarketValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Market value of the hole portfolio as string
        /// </summary>
        [Browsable(false)]
        public string PortfolioMarketValueAsStr => Helper.FormatDecimal(PortfolioMarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Market value of the hole portfolio as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioMarketValueAsStrUnit => Helper.FormatDecimal(PortfolioMarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        #endregion Portfolio value properties

        #region Data grid view properties

        /// <summary>
        /// WKN of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Wkn")]
        public string DgvWkn => WknAsStr;

        /// <summary>
        /// Name of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Name")]
        public string DgvNameAsStr => NameAsStr;

        /// <summary>
        /// Volume of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string DgvVolumeAsStr => VolumeAsStr;

        /// <summary>
        /// Previous day price of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"CurPrevPrice")]
        public string DgvCurPrevPriceAsStrUnit => CurPrevPriceAsStrUnit;

        /// <summary>
        /// Difference between the current and the previous day of the share and the performance in percent
        /// of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"PrevDayPerformance")]
        public string DgvPrevDayDifferencePerformanceAsStrUnit => PrevDayDifferencePerformanceAsStrUnit;

        /// <summary>
        /// Image which indicates the performance of the share to the previous day for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"")]
        public Image DgvImagePrevDayPerformance => ImagePrevDayPerformance;

        /// <summary>
        /// Profit or loss and performance value of the market value of the share volume as sting with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"ProfitLossPerformanceFinalValue")]
        public string DgvProfitLossPerformanceValueAsStrUnit => ProfitLossPerformanceValueAsStrUnit;

        /// <summary>
        /// Purchase value and market value of the share volume as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"PurchaseValueFinalValue")]
        public string DgvPurchaseValueFinalValueAsStrUnit => PurchaseValueMarketValueAsStrUnit;

        #endregion Data grid properties

        #endregion Properties

        #region Methods

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Standard constructor
        /// </summary>
        public ShareObjectMarketValue(List<Image> imageList, string percentageUnit, string pieceUnit) : base(imageList, percentageUnit, pieceUnit)
        {
        }

        /// <inheritdoc />
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
        /// <param name="webSite">Website address of the share</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
        /// <param name="shareType">Type of the share</param>
        /// <param name="document">Document of the first buy</param>
        public ShareObjectMarketValue(
            string wkn, string addDateTime, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShareDate, DateTime lastUpdateShareTime,
            decimal price, decimal volume, decimal reduction, decimal costs,
            string webSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            int shareType, string document) 
            : base(wkn, addDateTime, name, lastUpdateInternet, lastUpdateShareDate, lastUpdateShareTime,
                    price, volume, reduction, costs, webSite, imageListForDayBeforePerformance, regexList,
                    cultureInfo, shareType, document)
        {
        }

        #endregion Constructors

        #region Destructor

        /// <inheritdoc />
        /// <summary>
        /// Destructor
        /// </summary>
        ~ShareObjectMarketValue()
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
                CalculateMarketValue();

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
                if (!AllCostsEntries.RemoveCost(strDateTime))
                    return false;

                // Set costs of the share
                CostsValueTotal = AllCostsEntries.CostValueTotal;

                // Add new costs of the share to the costs of all shares
                PortfolioCosts += AllCostsEntries.CostValueTotal;

                // Recalculate the total sum of the share
                CalculateMarketValue();

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

        #region Performance methods

        /// <summary>
        /// This function calculates the new current market value of this share
        /// without dividends and costs and sales profit or loss
        /// </summary>
        private void CalculateMarketValue()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllCostsEntries.CostValueTotal > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                MarketValue = CurPrice * Volume;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateFinalValue()");
            Console.WriteLine("CurrentPrice: {0}", CurPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("MarketValue: {0}", MarketValue);
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
                )
            {
                ProfitLossValue = CurPrice * Volume
                    - PurchaseValue;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateProfitLoss()");
            Console.WriteLine("CurPrice: {0}", CurPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine("ProfitLossValue: {0}", ProfitLossValue);
#endif
        }

        /// <summary>
        /// This function calculates the new performance of this share
        /// with dividends and costs and sales profit or loss
        /// </summary>
        private void CalculatePerformance()
        {
            if (MarketValue <= decimal.MinValue / 2 || PurchaseValue <= decimal.MinValue / 2) return;

            if (PurchaseValue != 0)
                PerformanceValue = (MarketValue * 100) / PurchaseValue - 100;
            else
                PerformanceValue = 0;

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculatePerformance()");
            Console.WriteLine("MarketValue: {0}", MarketValue);
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

                        nodeElement.Attributes["WKN"].InnerText = shareObject.Wkn;
                        nodeElement.Attributes["Name"].InnerText = shareObject.NameAsStr;

                        for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                        {
                            switch (i)
                            {
                                #region General

                                case 0:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateInternet.ToShortDateString()} {
                                                shareObject.LastUpdateInternet.ToShortTimeString()
                                            }";
                                    break;
                                case 1:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateDate.ToShortDateString()} {
                                                shareObject.LastUpdateDate.ToShortTimeString()
                                            }";
                                    break;
                                case 2:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateTime.ToShortDateString()} {
                                                shareObject.LastUpdateTime.ToShortTimeString()
                                            }";
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
                                case 7:
                                    nodeElement.ChildNodes[i].InnerXml = shareObject.ShareType.ToString();
                                    break;

                                #endregion General

                                #region Buys

                                case 8:
                                    // Remove old buys
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var buyElementYear in shareObject.AllBuyEntries
                                        .GetAllBuysOfTheShare())
                                    {
                                        var newBuyElement = xmlPortfolio.CreateElement(BuyTagNamePre);
                                        newBuyElement.SetAttribute(BuyDateAttrName, buyElementYear.DateAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeAttrName,
                                            buyElementYear.VolumeAsStr);
                                        newBuyElement.SetAttribute(BuyPriceAttrName,
                                            buyElementYear.SharePriceAsStr);
                                        newBuyElement.SetAttribute(BuyReductionAttrName,
                                            buyElementYear.ReductionAsStr);
                                        newBuyElement.SetAttribute(BuyDocumentAttrName,
                                            buyElementYear.Document);
                                        nodeElement.ChildNodes[i].AppendChild(newBuyElement);
                                    }
                                    break;

                                #endregion Buys

                                #region Sales

                                case 9:
                                    // Remove old sales
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var saleElementYear in shareObject.AllSaleEntries
                                        .GetAllSalesOfTheShare())
                                    {
                                        var newSaleElement = xmlPortfolio.CreateElement(SaleTagNamePre);
                                        newSaleElement.SetAttribute(SaleDateAttrName,
                                            saleElementYear.DateAsStr);
                                        newSaleElement.SetAttribute(SaleVolumeAttrName,
                                            saleElementYear.VolumeAsStr);
                                        newSaleElement.SetAttribute(SaleBuyPriceAttrName,
                                            saleElementYear.BuyPriceAsStr);
                                        newSaleElement.SetAttribute(SaleSalePriceAttrName,
                                            saleElementYear.SalePriceAsStr);
                                        newSaleElement.SetAttribute(SaleTaxAtSourceAttrName,
                                            saleElementYear.TaxAtSourceAsStr);
                                        newSaleElement.SetAttribute(SaleCapitalGainsTaxAttrName,
                                            saleElementYear.CapitalGainsTaxAsStr);
                                        newSaleElement.SetAttribute(SaleSolidarityTaxAttrName,
                                            saleElementYear.SolidarityTaxAsStr);
                                        newSaleElement.SetAttribute(SaleDocumentAttrName,
                                            saleElementYear.Document);
                                        nodeElement.ChildNodes[i].AppendChild(newSaleElement);
                                    }
                                    break;

                                #endregion Sales

                                #region Costs

                                case 10:
                                    // Remove old costs
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var costElementYear in shareObject.AllCostsEntries
                                        .GetAllCostsOfTheShare())
                                    {
                                        var newCostElement = xmlPortfolio.CreateElement(CostsTagNamePre);
                                        newCostElement.SetAttribute(CostsBuyPartAttrName,
                                            costElementYear.CostOfABuyAsStr);
                                        newCostElement.SetAttribute(CostsDateAttrName,
                                            costElementYear.CostDateAsStr);
                                        newCostElement.SetAttribute(CostsValueAttrName,
                                            costElementYear.CostValueAsStr);
                                        newCostElement.SetAttribute(BuyDocumentAttrName,
                                            costElementYear.CostDocument);
                                        nodeElement.ChildNodes[i].AppendChild(newCostElement);
                                    }
                                    break;

                                #endregion Costs

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
                        var rootPortfolio = xmlPortfolio.SelectSingleNode("Portfolio");

                        // Add new share
                        var newShareNode = xmlPortfolio.CreateNode(XmlNodeType.Element, "Share", null);

                        // Add attributes (WKN)
                        var xmlAttributeWkn = xmlPortfolio.CreateAttribute("WKN");
                        xmlAttributeWkn.Value = shareObject.WknAsStr;
                        newShareNode.Attributes.Append(xmlAttributeWkn);

                        // Add attributes (ShareName)
                        var xmlAttributeShareName = xmlPortfolio.CreateAttribute("Name");
                        xmlAttributeShareName.Value = shareObject.NameAsStr;
                        newShareNode.Attributes.Append(xmlAttributeShareName);

                        // Add child nodes (last update Internet)
                        var newLastUpdateInternet = xmlPortfolio.CreateElement("LastUpdateInternet");
                        // Add child inner text
                        var lastUpdateInternetValue = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateInternet.ToShortDateString() + " " +
                            shareObject.LastUpdateInternet.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateInternet);
                        newShareNode.LastChild.AppendChild(lastUpdateInternetValue);

                        // Add child nodes (last update date)
                        var newLastUpdateDate = xmlPortfolio.CreateElement("LastUpdateShareDate");
                        // Add child inner text
                        var lastUpdateValueDate = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateDate.ToShortDateString() + " " +
                            shareObject.LastUpdateDate.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateDate);
                        newShareNode.LastChild.AppendChild(lastUpdateValueDate);

                        // Add child nodes (last update time)
                        var newLastUpdateTime = xmlPortfolio.CreateElement("LastUpdateTime");
                        // Add child inner text
                        var lastUpdateValueTime = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateTime.ToShortDateString() + " " +
                            shareObject.LastUpdateTime.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateTime);
                        newShareNode.LastChild.AppendChild(lastUpdateValueTime);

                        // Add child nodes (share price)
                        var newSharePrice = xmlPortfolio.CreateElement("SharePrice");
                        // Add child inner text
                        var sharePrice = xmlPortfolio.CreateTextNode(shareObject.CurPriceAsStr);
                        newShareNode.AppendChild(newSharePrice);
                        newShareNode.LastChild.AppendChild(sharePrice);

                        // Add child nodes (share price before)
                        var newSharePriceBefore = xmlPortfolio.CreateElement("SharePriceBefore");
                        // Add child inner text
                        var sharePriceBefore = xmlPortfolio.CreateTextNode(shareObject.PrevDayPriceAsStr);
                        newShareNode.AppendChild(newSharePriceBefore);
                        newShareNode.LastChild.AppendChild(sharePriceBefore);

                        // Add child nodes (website)
                        var newWebsite = xmlPortfolio.CreateElement("WebSite");
                        // Add child inner text
                        var webSite = xmlPortfolio.CreateTextNode(shareObject.WebSite);
                        newShareNode.AppendChild(newWebsite);
                        newShareNode.LastChild.AppendChild(webSite);

                        // Add child nodes (culture)
                        var newCulture = xmlPortfolio.CreateElement("Culture");
                        // Add child inner text
                        var culture = xmlPortfolio.CreateTextNode(shareObject.CultureInfo.Name);
                        newShareNode.AppendChild(newCulture);
                        newShareNode.LastChild.AppendChild(culture);

                        // Add child nodes (share type)
                        var newShareType = xmlPortfolio.CreateElement("ShareType");
                        // Add child inner text
                        var shareType = xmlPortfolio.CreateTextNode(shareObject.ShareType.ToString());
                        newShareNode.AppendChild(newShareType);
                        newShareNode.LastChild.AppendChild(shareType);

                        #endregion General

                        #region Buys / Sales / Costs / Dividends

                        // Add child nodes (buys)
                        var newBuys = xmlPortfolio.CreateElement("Buys");
                        newShareNode.AppendChild(newBuys);
                        foreach (var buyElementYear in shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                        {
                            XmlElement newBuyElement = xmlPortfolio.CreateElement(BuyTagNamePre);
                            newBuyElement.SetAttribute(BuyDateAttrName, buyElementYear.DateAsStr);
                            newBuyElement.SetAttribute(BuyVolumeAttrName, buyElementYear.VolumeAsStr);
                            newBuyElement.SetAttribute(BuyPriceAttrName, buyElementYear.SharePriceAsStr);
                            newBuyElement.SetAttribute(BuyReductionAttrName, buyElementYear.ReductionAsStr);
                            newBuyElement.SetAttribute(BuyDocumentAttrName, buyElementYear.Document);
                            newBuys.AppendChild(newBuyElement);
                        }

                        // Add child nodes (sales)
                        var newSales = xmlPortfolio.CreateElement("Sales");
                        newShareNode.AppendChild(newSales);

                        // Add child nodes (costs)
                        var newCosts = xmlPortfolio.CreateElement("Costs");
                        newShareNode.AppendChild(newCosts);
                        foreach (var costElementYear in shareObject.AllCostsEntries.GetAllCostsOfTheShare())
                        {
                            var newCostElement = xmlPortfolio.CreateElement(CostsTagNamePre);
                            newCostElement.SetAttribute(CostsBuyPartAttrName, costElementYear.CostOfABuyAsStr);
                            newCostElement.SetAttribute(CostsDateAttrName, costElementYear.CostDateAsStr);
                            newCostElement.SetAttribute(CostsValueAttrName, costElementYear.CostValueAsStr);
                            newCostElement.SetAttribute(BuyDocumentAttrName, costElementYear.CostDocument);
                            newCosts.AppendChild(newCostElement);
                        }

                        // Add child nodes (dividend)
                        var newDividend = xmlPortfolio.CreateElement("Dividends");
                        newDividend.SetAttribute(DividendPayoutIntervalAttrName,
                            shareObject.DividendPayoutIntervalAsStr);
                        newShareNode.AppendChild(newDividend);

                        #endregion Buys / Sales / Costs / Dividends

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

        #region Portfolio values 

        #region Portfolio performance values

        /// <summary>
        /// This function calculates the performance of the portfolio
        /// </summary>
        private void CalculatePortfolioPerformance()
        {
            if (PortfolioPurchaseValue != 0)
                PortfolioPerformanceValue = PortfolioMarketValue * 100 / PortfolioPurchaseValue - 100;
            else
                PortfolioPerformanceValue = 0;

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculatePerformancePortfolio()");
            Console.WriteLine("PortfolioMarketValue: {0}", PortfolioMarketValue);
            Console.WriteLine("PortfolioPerformanceValue: {0}", PortfolioPerformanceValue);
#endif
        }

        /// <summary>
        /// This function calculates the profit or loss of the portfolio
        /// </summary>
        private void CalculatePortfolioProfitLoss()
        {
            PortfolioProfitLossValue = PortfolioMarketValue - PortfolioPurchaseValue;

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculatePortfolioProfitLoss()");
            Console.WriteLine("PortfolioMarketValue: {0}", PortfolioMarketValue);
            Console.WriteLine("PortfolioPurchaseValue: {0}", PortfolioPurchaseValue);
            Console.WriteLine("PortfolioProfitLossFinalValue: {0}", PortfolioProfitLossValue);
#endif
        }

        #endregion Portfolio performance values

        /// <summary>
        /// This function resets the portfolio values of the share objects
        /// </summary>
        public static void PortfolioValuesReset()
        {
            _portfolioPurchaseValue = 0;
            _portfolioSalePurchaseValue = 0;
            _portfolioMarketValue = 0;
            _portfolioPerformanceValue = 0;
            _portfolioProfitLossValue = 0;
        }

        #endregion Portfolio values

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

#if DEBUG_SHAREOBJECT
            Console.WriteLine(@"ShareObjectMarketValue destructor...");
#endif
            //if (MarketValue > decimal.MinValue / 2)
            //    PortfolioMarketValue -= MarketValue;
            if (PurchaseValue > decimal.MinValue / 2)
                PortfolioPurchaseValue -= PurchaseValue;
            Disposed = true;

            // Call base class implementation.
            base.Dispose(bDisposing);
        }

        #endregion Methods
    }
}
