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
using System.Linq;
using System.Xml;
using Parser;
using SharePortfolioManager.Classes.Sales;

namespace SharePortfolioManager.Classes.ShareObjects
{
    /// <inheritdoc />
    /// <summary>
    /// This class is for the market value management of a share.
    /// The class stores the following things:
    /// - general information of the share
    /// - all buys of a share
    /// - all sales of a share
    /// - all brokerage of the sales and buys of a share
    /// </summary>
    public class ShareObjectMarketValue : ShareObject
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Stores the current share value value without dividends, brokerage, profits and loss (market value)
        /// </summary>
        private decimal _marketValue = decimal.MinValue / 2;

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
        /// Stores the current value of the portfolio (all shares) without dividends, brokerage, profits and loss (market value)
        /// </summary>
        private static decimal _portfolioMarketValue;

        /// <summary>
        /// Stores the performance of the portfolio (all shares) without dividends, brokerage, profits and loss
        /// </summary>
        private static decimal _portfolioPerformanceValue;

        /// <summary>
        /// Stores the profit or loss of the portfolio (all shares) without dividends, brokerage, profits and loss
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
                // Set the new purchase price
                if (value == _purchaseValue) return;

                // Recalculate portfolio purchase price value
                if (_purchaseValue > decimal.MinValue / 2)
                    PortfolioPurchaseValue -= _purchaseValue;
                PortfolioPurchaseValue += value;

                _purchaseValue = value;

                // Recalculate the total market value of the share
                CalculateMarketValue();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();
            }
        }

        /// <summary>
        /// Purchase value of the current share volume as string with unit
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
            set => base.CultureInfo = value;
        }

        #endregion General properties

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

#if DEBUG_SHAREOBJECT_MARKET
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

#if DEBUG_SHAREOBJECT_MARKET
                    Console.WriteLine(@"_marketValue: {0}", _marketValue);
                    Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
#endif
            }
        }

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
        /// <param name="guid">Guid of the buy</param>
        /// <param name="wkn">WKN number of the share</param>
        /// <param name="addDateTime">Date and time of the add</param>
        /// <param name="name">Name of the share</param>
        /// <param name="orderNumber">order number of the first buy</param>
        /// <param name="lastUpdateInternet">Date and time of the last update from the Internet</param>
        /// <param name="lastUpdateShareDate">Date of the last update on the Internet site of the share</param>
        /// <param name="lastUpdateShareTime">Time of the last update on the Internet site of the share</param>
        /// <param name="price">Current price of the share</param>
        /// <param name="volume">Volume of the share</param>
        /// <param name="volumeSold">Volume of the share which is already sold</param>
        /// <param name="webSite">Website address of the share</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
        /// <param name="shareType">Type of the share</param>
        /// <param name="document">Document of the first buy</param>
        public ShareObjectMarketValue(
            string guid, string wkn, string addDateTime, string name, string orderNumber,
            DateTime lastUpdateInternet, DateTime lastUpdateShareDate, DateTime lastUpdateShareTime,
            decimal price, decimal volume, decimal volumeSold, string webSite, List<Image> imageListForDayBeforePerformance,
            RegExList regexList, CultureInfo cultureInfo, int shareType, string document) 
            : base(wkn, addDateTime, name, lastUpdateInternet, lastUpdateShareDate, lastUpdateShareTime,
                    price, webSite, imageListForDayBeforePerformance, regexList,
                    cultureInfo, shareType)
        {
            AddBuy(guid, orderNumber, AddDateTime, volume, volumeSold, price, document);
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
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strGuid, string strOrderNumber, string strDateTime, decimal decVolume, decimal decVolumeSold, decimal decPrice, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT_MARKET
                Console.WriteLine(@"");
                Console.WriteLine(@"AddBuy() / MarketValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strOrderNumber: {0}", strOrderNumber);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"decVolume: {0}", decVolume);
                Console.WriteLine(@"decVolumeSold: {0}", decVolumeSold);
                Console.WriteLine(@"decPrice: {0}", decPrice);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                // Add buy without reductions and brokerage
                if (!AllBuyEntries.AddBuy(strGuid, strOrderNumber, strDateTime, decVolume, decVolumeSold, decPrice, null, strDoc))
                    return false;

                // Set buy value of the share
                PurchaseValueTotal = AllBuyEntries.BuyValueTotal;

                // Set new volume
                if (Volume == decimal.MinValue / 2)
                    Volume = 0;
                Volume += decVolume;

                // Recalculate MarketValue
                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                PurchaseValue += AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValue;

#if DEBUG_SHAREOBJECT_MARKET || DEBUG_BUY
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"BuyValueTotal: {0}", PurchaseValueTotal);
                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
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
        /// <param name="strGuid">Guid of the buy remove</param>
        /// <param name="strDateTime">Date and time of the buy remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveBuy(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_SHAREOBJECT_MARKET || DEBUG_BUY
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveBuy() / MarketValue");
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get BuyObject by Guid and date and time and add the sale PurchaseValue and value to the share
                var buyObject = AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime);
                if (buyObject != null)
                {
                    Volume -= buyObject.Volume;
                    PurchaseValue -= buyObject.BuyValue;
                    PurchaseValueTotal = AllBuyEntries.BuyValueTotal;

#if DEBUG_SHAREOBJECT_MARKET || DEBUG_BUY
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"BuyValueTotal: {0}", PurchaseValueTotal);
                    Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                    Console.WriteLine(@"");
#endif
                    // Remove buy by date and time
                    if (!AllBuyEntries.RemoveBuy(strGuid, strDateTime))
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

        /// <summary>
        /// This function sets the document of the buy of the given Guid and datetime
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be modified</param>
        /// <param name="strDateTime">Date time of the buy which should be modified</param>
        /// <param name="strDocument">Document which should be set</param>
        /// <returns></returns>
        public bool SetDocument(string strGuid, string strDateTime, string strDocument)
        {
            try
            {
#if DEBUG_SHAREOBJECT_FINAL || DEBUG_BUY
                Console.WriteLine(@"");
                Console.WriteLine(@"SetDocument() / FinalValue");
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
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="usedBuyDetails">Details of the used buys for the sale</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="decBrokerage">Brokerage of the sale</param>
        /// <param name="decReduction">Reduction of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strGuid, string strDate, decimal decVolume, decimal decSalePrice, List<SaleBuyDetails> usedBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, decimal decBrokerage, decimal decReduction, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT_MARKET
                Console.WriteLine(@"");
                Console.WriteLine(@"AddSale() / MarketValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDate);
                Console.WriteLine(@"decVolume: {0}", decVolume);
                Console.WriteLine(@"decBuyPrice: {0}", decBuyPrice);
                Console.WriteLine(@"decSalePrice: {0}", decSalePrice);
                Console.WriteLine(@"decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine(@"decCapitalGainsTax: {0}", decCapitalGainsTax);
                Console.WriteLine(@"decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine(@"decBrokerage: {0}", decBrokerage);
                Console.WriteLine(@"decReduction: {0}", decReduction);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                if (!AllSaleEntries.AddSale(strGuid, strDate, decVolume, decSalePrice, usedBuyDetails, decTaxAtSource, decCapitalGainsTax,
                                            decSolidarityTax, decBrokerage, decReduction, strDoc))
                    return false;

                // Set new volume
                if (Volume == decimal.MinValue / 2)
                    Volume = 0;
                Volume -= decVolume;

                // Recalculate PurchaseValue and SalePurchaseValueTotal
                if (SalePurchaseValueTotal == decimal.MinValue / 2)
                    SalePurchaseValueTotal = 0;

                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                else
                {
                    // TODO
                    if (Volume > 0 && PurchaseValue > 0)
                    {
                        SalePurchaseValueTotal += usedBuyDetails.Sum(x => x.SaleBuyValue) /*decBuyPrice * decVolume*/;
                        PurchaseValue -= usedBuyDetails.Sum(x => x.SaleBuyValue) /*decBuyPrice * decVolume*/;
                    }
                    else
                    {
                        SalePurchaseValueTotal += PurchaseValue;
                        PurchaseValue = 0;
                    }
                }

#if DEBUG_SHAREOBJECT_MARKET
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"SalePurchaseValueTotal: {0}", SalePurchaseValueTotalAsStr);
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
#if DEBUG_SHAREOBJECT_MARKET
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveSale() / MarketValue");
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get SaleObject by date and time and add the sale deposit and value to the share
                var saleObject = AllSaleEntries.GetSaleObjectByDateTime(strDateTime);
                if (saleObject != null)
                {
                    Volume += saleObject.Volume;
                    PurchaseValue += saleObject.PurchaseValue;
                    SalePurchaseValueTotal -= saleObject.PurchaseValue;

                    // Remove sale by date and time
                    if (!AllSaleEntries.RemoveSale(strGuid, strDateTime))
                        return false;

#if DEBUG_SHAREOBJECT_MARKET
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"SalePurchaseValueTotal: {0}", SalePurchaseValueTotalAsStr);
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

        #endregion Sale methods

        #region Performance methods

        /// <summary>
        /// This function calculates the new current market value of this share
        /// without dividends and brokerage and sales profit or loss
        /// </summary>
        private void CalculateMarketValue()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                MarketValue = CurPrice * Volume;
            }

#if DEBUG_SHAREOBJECT_MARKET
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateFinalValue() / MarketValue");
            Console.WriteLine(@"CurrentPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"MarketValue: {0}", MarketValue);
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
                )
            {
                ProfitLossValue = CurPrice * Volume
                    - PurchaseValue;
            }

#if DEBUG_SHAREOBJECT_MARKET
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateProfitLoss() / MarketValue");
            Console.WriteLine(@"CurPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine(@"ProfitLossValue: {0}", ProfitLossValue);
#endif
        }

        /// <summary>
        /// This function calculates the new performance of this share
        /// with dividends and brokerage and sales profit or loss
        /// </summary>
        private void CalculatePerformance()
        {
            if (MarketValue <= decimal.MinValue / 2 || PurchaseValue <= decimal.MinValue / 2) return;

            if (PurchaseValue != 0)
                PerformanceValue = (MarketValue * 100) / PurchaseValue - 100;
            else
                PerformanceValue = 0;

#if DEBUG_SHAREOBJECT_MARKET
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformance() / MarketValue");
            Console.WriteLine(@"MarketValue: {0}", MarketValue);
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

                        nodeElement.Attributes["WKN"].InnerText = shareObject.Wkn;
                        nodeElement.Attributes["Name"].InnerText = shareObject.NameAsStr;

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
                                    foreach (var brokerageElementYear in shareObject.AllBrokerageEntries
                                        .GetAllBrokerageOfTheShare())
                                    {
                                        var newBrokerageElement = xmlPortfolio.CreateElement(BrokerageTagNamePre);
                                        newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName, brokerageElementYear.PartOfABuyAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageDateAttrName, brokerageElementYear.DateAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageProvisionAttrName, brokerageElementYear.ProvisionValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageBrokerFeeAttrName, brokerageElementYear.BrokerFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageTraderPlaceFeeAttrName, brokerageElementYear.TraderPlaceFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageReductionAttrName, brokerageElementYear.ReductionValueAsStr);
                                        newBrokerageElement.SetAttribute(BuyDocumentAttrName, brokerageElementYear.BrokerageDocument);
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
                                        var newBuyElement = xmlPortfolio.CreateElement(BuyTagNamePre);
                                        newBuyElement.SetAttribute(BuyGuidAttrName,
                                            buyElementYear.Guid);
                                        newBuyElement.SetAttribute(BuyDateAttrName,
                                            buyElementYear.DateAsStr);
                                        newBuyElement.SetAttribute(BuyOrderNumberAttrName,
                                            buyElementYear.OrderNumber);
                                        newBuyElement.SetAttribute(BuyVolumeAttrName,
                                            buyElementYear.VolumeAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeSoldAttrName,
                                            buyElementYear.VolumeSoldAsStr);
                                        newBuyElement.SetAttribute(BuyPriceAttrName,
                                            buyElementYear.SharePriceAsStr);
                                        newBuyElement.SetAttribute(BuyBrokerageGuidAttrName,
                                            buyElementYear.ReductionAsStr);
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
                                        newSaleElement.SetAttribute(SaleDateAttrName,
                                            saleElementYear.DateAsStr);
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
                            newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName, brokerageElementYear.PartOfABuyAsStr);
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
                            newBuyElement.SetAttribute(BuyPriceAttrName, buyElementYear.SharePriceAsStr);
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

#if DEBUG_SHAREOBJECT_MARKET
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformancePortfolio() / MarketValue");
            Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
            Console.WriteLine(@"PortfolioPerformanceValue: {0}", PortfolioPerformanceValue);
#endif
        }

        /// <summary>
        /// This function calculates the profit or loss of the portfolio
        /// </summary>
        private void CalculatePortfolioProfitLoss()
        {
            PortfolioProfitLossValue = PortfolioMarketValue - PortfolioPurchaseValue;

#if DEBUG_SHAREOBJECT_MARKET
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePortfolioProfitLoss() / MarketValue");
            Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
            Console.WriteLine(@"PortfolioPurchaseValue: {0}", PortfolioPurchaseValue);
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

#if DEBUG_SHAREOBJECT_MARKET
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
