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
using System.Xml;
using WebParser;

namespace SharePortfolioManager
{
    public class ShareObjectMarketValue : ShareObject
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Flag if the object is already disposed
        /// </summary>
        private bool _bDisposed = false;

        /// <summary>
        /// Stores the current total share market value without dividends, costs, profits and loss
        /// </summary>
        private decimal _marketValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the performance of the share without dividends, costs, profits and loss
        /// </summary>
        private decimal _performanceValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the profit or loss of the share without dividends, costs, profits and loss
        /// </summary>
        private decimal _profitLossValue = decimal.MinValue / 2;

        #endregion General variables

        #region Portfolio value variables

        /// <summary>
        /// Stores the value of the portfolio (all shares) without dividends, costs, profits and loss
        /// </summary>
        static private decimal _portfolioMPurchaseValue = 0;

        /// <summary>
        /// Stores the current value of the portfolio (all shares) without dividends, costs, profits and loss
        /// </summary>
        static private decimal _portfolioMarketValue = 0;

        /// <summary>
        /// Stores the performance of the portfolio (all shares) without dividends, costs, profits and loss
        /// </summary>
        static private decimal _portfolioPerformanceValue = 0;

        /// <summary>
        /// Stores the profit or loss of the portfolio (all shares) without dividends, costs, profits and loss
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

                    // Recalculate the appreciation
                    CalculatePerformance();

                    // Recalculate the profit or loss of the share
                    CalculateProfitLoss();

                    // Recalculate the total sum of the share
                    CalculateMarketValue();
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

                    // Recalculate the appreciation
                    CalculatePerformance();

                    // Recalculate the profit or loss of the share
                    CalculateProfitLoss();

                    // Recalculate the total sum of the share
                    CalculateMarketValue();
                }
            }
        }

        [Browsable(false)]
        public override decimal PrevDayPrice
        {
            get { return base.PrevDayPrice; }
            set
            {
                // Set the new share price
                if (value != base.PrevDayPrice)
                {
                    base.PrevDayPrice = value;

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
        }

        [Browsable(false)]
        public override decimal PurchaseValue
        {
            get { return base.PurchaseValue; }
            set
            {
                // Set the new share price
                if (value != base.PurchaseValue)
                {
                    // Recalculate portfolio purchase value
                    if (base.PurchaseValue > decimal.MinValue / 2)
                        PortfolioPurchaseValue -= base.PurchaseValue;
                    PortfolioPurchaseValue += value;

                    base.PurchaseValue = value;

                    // Recalculate the appreciation
                    CalculatePerformance();

                    // Recalculate the profit or loss of the share
                    CalculateProfitLoss();

                    // Recalculate the total sum of the share
                    CalculateMarketValue();
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
            }
        }

        #endregion General properties

        #region Market value properties

        [Browsable(false)]
        public decimal MarketValue
        {
            get { return _marketValue; }
            set
            {
                if (value != _marketValue)
                {
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
                value += "\n" + Helper.FormatDecimal(MarketValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Market value properties

        #region Portfolio value properties

        [Browsable(false)]
        public decimal PortfolioPurchaseValue
        {
            get { return _portfolioMPurchaseValue; }
            internal set
            {
                if (_portfolioMPurchaseValue != value)
                {
                    _portfolioMPurchaseValue = value;

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
        public decimal PortfolioMarketValue
        {
            get { return _portfolioMarketValue; }
            internal set
            {
                if (_portfolioMarketValue != value)
                {
                    _portfolioMarketValue = value;

                    // Recalculate the performance of all shares
                    CalculatePortfolioPerformance();

                    // Recalculate the profit or lose of all shares
                    CalculatePortfolioProfitLoss();
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
            get { return VolumeAsStr; }
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
        public ShareObjectMarketValue(List<Image> imageList, string percentageUnit, string pieceUnit) : base(imageList, percentageUnit, pieceUnit)
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
        public ShareObjectMarketValue(
            string wkn, string addDateTime, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShareDate, DateTime lastUpdateShareTime,
            decimal price, decimal volume, decimal reduction, decimal costs, decimal purchaseValue,
            string webSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            int dividendPayoutInterval, string document) : base(wkn, addDateTime, name, lastUpdateInternet, lastUpdateShareDate, lastUpdateShareTime,
                                                                price, volume, reduction, costs, purchaseValue,
                                                                webSite, imageListForDayBeforePerformance, regexList, cultureInfo,
                                                                document)
        {
        }

        #endregion Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~ShareObjectMarketValue()
        {
            Dispose(false);
        }

        #endregion Destructor

        #region Performance methods

        /// <summary>
        /// This function calculates the new current market value of this share
        /// without dividends and costs and sales profit or loss
        /// </summary>
        private void CalculateMarketValue()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
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
                ProfitLossValue = CurPrice * Volume - PurchaseValue;
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
            if (MarketValue > decimal.MinValue / 2
                && PurchaseValue > decimal.MinValue / 2
                && PurchaseValue != 0
                )
            {
                PerformanceValue = (MarketValue * 100) / PurchaseValue - 100;
            }

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
                                        // TODO
                                        //nodeElement.ChildNodes[i].RemoveAll();
                                        //foreach (var saleElementYear in shareObject.AllSaleEntries.GetAllSalesOfTheShare())
                                        //{
                                        //    XmlElement newSaleElement = xmlPortfolio.CreateElement(shareObject.SaleTagNamePre);
                                        //    newSaleElement.SetAttribute(shareObject.SaleDateAttrName, saleElementYear.SaleDateAsString);
                                        //    newSaleElement.SetAttribute(shareObject.SaleVolumeAttrName, saleElementYear.SaleVolumeAsString);
                                        //    newSaleElement.SetAttribute(shareObject.SalePriceAttrName, saleElementYear.SaleValueAsString);
                                        //    newSaleElement.SetAttribute(shareObject.SaleProfitLossAttrName, saleElementYear.SaleProfitLossAsString);
                                        //    newSaleElement.SetAttribute(shareObject.SaleDocumentAttrName, saleElementYear.SaleDocument);
                                        //    nodeElement.ChildNodes[i].AppendChild(newSaleElement);
                                        //}
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
                                            newCostElement.SetAttribute(shareObject.CostsBuyPartAttrName, costElementYear.CostOfABuyAsString);
                                            newCostElement.SetAttribute(shareObject.CostsDateAttrName, costElementYear.CostDateAsString);
                                            newCostElement.SetAttribute(shareObject.CostsValueAttrName, costElementYear.CostValueAsString);
                                            newCostElement.SetAttribute(shareObject.BuyDocumentAttrName, costElementYear.CostDocument);
                                            nodeElement.ChildNodes[i].AppendChild(newCostElement);
                                        }
                                        break;

                                    #endregion Costs

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
                        newCostElement.SetAttribute(shareObject.CostsBuyPartAttrName, costElementYear.CostOfABuyAsString);
                        newCostElement.SetAttribute(shareObject.CostsDateAttrName, costElementYear.CostDateAsString);
                        newCostElement.SetAttribute(shareObject.CostsValueAttrName, costElementYear.CostValueAsString);
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

        #region Portfolio values 

        #region Portfolio performance values

        /// <summary>
        /// This function calculates the performance of the portfolio
        /// </summary>
        private void CalculatePortfolioPerformance()
        {
            if (PortfolioPurchaseValue != 0)
            {
                PortfolioPerformanceValue = PortfolioMarketValue * 100 / PortfolioPurchaseValue - 100;
            }
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
        static public void PortfolioValuesReset()
        {
            _portfolioMarketValue = 0;
            _portfolioPerformanceValue = 0;
            _portfolioProfitLossValue = 0;
        }

        #endregion Portfolio values

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
            Console.WriteLine("ShareObjectMarketValue destructor...");
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
