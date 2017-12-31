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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using WebParser;

namespace SharePortfolioManager.Classes.ShareObjects
{
    /// <inheritdoc />
    /// <summary>
    /// This class is the base class of a share.
    /// The class stores the following things:
    /// - general information of the share
    /// - all buys of a share
    /// - all sales of a share
    /// </summary>
    public class ShareObject : IDisposable
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Counter of the created share objects
        /// </summary>
        private static int _iObjectCounter;

        /// <summary>
        /// Stores the count of the share object tags in the XML
        /// </summary>
        internal const short ShareObjectTagCount = 12;

        /// <summary>
        /// Stores a list of images for the previous day performance visualization
        /// </summary>
        private List<Image> _imageListPrevDayPerformance;

        /// <summary>
        /// Stores the website link of the share
        /// </summary>
        private string _webSite;

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        #endregion General variables

        #region Value units variables

        #endregion Value units variables

        #region Buy variables

        #endregion Buy variables

        #region Buy XML variables

        /// <summary>
        /// Stores the XML tag name prefix of a buy entry
        /// </summary>
        internal const string BuyTagNamePre = "Buy";

        /// <summary>
        /// Stores the XML attribute name for the buy date
        /// </summary>
        internal const string BuyDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the volume of a buy
        /// </summary>
        internal const string BuyVolumeAttrName = "Volume";

        /// <summary>
        /// Stores the XML attribute name for the reduction of a buy
        /// </summary>
        internal const string BuyReductionAttrName = "Reduction";

        /// <summary>
        /// Stores the XML attribute name for buy price of a share of a buy
        /// </summary>
        internal const string BuyPriceAttrName = "Price";

        /// <summary>
        /// Stores the XML attribute name for the document of a buy
        /// </summary>
        internal const string BuyDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the dividend
        /// </summary>
        internal const short BuyAttrCount = 5;

        #endregion Buy XML variables

        #region Sale variables

        #endregion Sale variables

        #region Sale XML variables

        /// <summary>
        /// Stores the XML tag name prefix of a sale entry
        /// </summary>
        internal const string SaleTagNamePre = "Sale";

        /// <summary>
        /// Stores the XML attribute name for the sale date
        /// </summary>
        internal const string SaleDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the volume of a sale
        /// </summary>
        internal const string SaleVolumeAttrName = "Volume";

        /// <summary>
        /// Stores the XML attribute name for the buy price of one share of a sale
        /// </summary>
        internal const string SaleBuyPriceAttrName = "BuyPrice";

        /// <summary>
        /// Stores the XML attribute name for the sale price of one share of a sale
        /// </summary>
        internal const string SaleSalePriceAttrName = "SalePrice";

        /// <summary>
        /// Stores the XML attribute name for the tax at source of a sale
        /// </summary>
        internal const string SaleTaxAtSourceAttrName = "TaxAtSource";

        /// <summary>
        /// Stores the XML attribute name for the capital gains tax of a sale
        /// </summary>
        internal const string SaleCapitalGainsTaxAttrName = "CapitalGainsTax";

        /// <summary>
        /// Stores the XML attribute name for the solidarity tax of a sale
        /// </summary>
        internal const string SaleSolidarityTaxAttrName = "SolidarityTax";

        /// <summary>
        /// Stores the XML attribute name for the document of a sale
        /// </summary>
        internal const string SaleDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the dividend
        /// </summary>
        internal const short SaleAttrCount = 8;

        #endregion Sale XML variables

        #region Dividends XML variables

        /// <summary>
        /// Stores the XML attribute name for the dividend payout interval
        /// </summary>
        internal const string DividendPayoutIntervalAttrName = "PayoutInterval";

        #endregion Dividends XML variables

        #region Costs XML variables

        /// <summary>
        /// Stores the tag name prefix of a cost entry
        /// </summary>
        internal const string CostsTagNamePre = "Cost";

        /// <summary>
        /// Stores the attribute name for the flag if the cost is a part of a buy
        /// </summary>
        internal const string CostsBuyPartAttrName = "BuyPart";

        /// <summary>
        /// Stores the attribute name for the flag if the cost is a part of a sale
        /// </summary>
        internal const string CostsSalePartAttrName = "SalePart";

        /// <summary>
        /// Stores the attribute name for the date 
        /// </summary>
        internal const string CostsDateAttrName = "Date";

        /// <summary>
        /// Stores the attribute name for the value
        /// </summary>
        internal const string CostsValueAttrName = "Value";

        /// <summary>
        /// Stores the XML attribute name for the document of a cost
        /// </summary>
        internal const string CostsDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the costs
        /// </summary>
        internal const short CostsAttrCount = 5;

        #endregion Costs XML variables

        #region Dividends XML variables

        /// <summary>
        /// Stores the XML tag name of a dividend entry
        /// </summary>
        internal const string DividendTagName = "Dividend";

        /// <summary>
        /// Stores the XML attribute name for the dividend pay date
        /// </summary>
        internal const string DividendDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the dividend pay for one share
        /// </summary>
        internal const string DividendRateAttrName = "Rate";

        /// <summary>
        /// Stores the XML attribute name for the share volume at the pay date
        /// </summary>
        internal const string DividendVolumeAttrName = "Volume";

        /// <summary>
        /// Stores the XML attribute name for the dividend tax at source value
        /// </summary>
        internal const string DividendTaxAtSourceAttrName = "TaxAtSource";

        /// <summary>
        /// Stores the XML attribute name for the dividend capital gains tax value
        /// </summary>
        internal const string DividendCapitalGainsTaxAttrName = "CapitalGainsTax";

        /// <summary>
        /// Stores the XML attribute name for the dividend capital gains tax value
        /// </summary>
        internal const string DividendSolidarityTaxAttrName = "SolidarityTax";

        /// <summary>
        /// Stores the XML attribute name for the share price at the pay date
        /// </summary>
        internal const string DividendPriceAttrName = "Price";

        /// <summary>
        /// Stores the XML attribute name for the document of a dividend
        /// </summary>
        internal const string DividendDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the XML tag name of the foreign currency information
        /// </summary>
        internal const string DividendTagNameForeignCu = "ForeignCurrency";

        /// <summary>
        /// Stores the XML attribute name for the flag if the dividend is paid in a foreign currency
        /// </summary>
        internal const string DividendForeignCuFlagAttrName = "Flag";

        /// <summary>
        /// Stores the XML attribute name for the factor of the foreign currency
        /// </summary>
        internal const string DividendExchangeRatioAttrName = "ExchangeRatio";

        /// <summary>
        /// Stores the XML attribute name for the name of the foreign currency
        /// </summary>
        internal const string DividendNameAttrName = "FCName";

        /// <summary>
        /// Stores the attribute count for the foreign currency information
        /// </summary>
        internal const short DividendAttrCountForeignCu = 3;

        /// <summary>
        /// Stores the attribute count for the dividend
        /// </summary>
        internal const short DividendAttrCount = 9;

        /// <summary>
        /// Stores the child node count for the dividend
        /// </summary>
        internal const short DividendChildNodeCount = 1;

        #endregion Dividends XML variables

        #endregion Variables

        #region Properties

        #region General properties

        /// <summary>
        /// Flag if the object is disposed
        /// </summary>
        [Browsable(false)]
        public bool Disposed { get; internal set; }

        /// <summary>
        /// Counter of the created objects
        /// </summary>
        [Browsable(false)]
        public static int ObjectCounter => _iObjectCounter / 2;

        /// <summary>
        /// WKN of the share
        /// </summary>
        [Browsable(false)]
        public string Wkn { get; set; }

        /// <summary>
        /// WKN of the share as string
        /// </summary>
        [Browsable(false)]
        public string WknAsStr => Wkn;

        /// <summary>
        /// DateTime of a buy add
        /// </summary>
        [Browsable(false)]
        public string AddDateTime { get; set; }

        /// <summary>
        /// Name of the share
        /// </summary>
        [Browsable(false)]
        public string Name { get; set; }

        /// <summary>
        /// Name of the share as string
        /// </summary>
        [Browsable(false)]
        public string NameAsStr => Name;

        /// <summary>
        /// DateTime of the last share update via Internet
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateInternet { get; set; }

        /// <summary>
        /// Date of the last update share of the parsed Internet side
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Time of the last update share of the parsed Internet side
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// Website for the share update value parsing
        /// </summary>
        [Browsable(false)]
        public string WebSite
        {
            get => _webSite;
            set
            {
                // Check if "http://" is in front of the website
                if (value.Substring(0, 7) != "http://" && value.Substring(0, 8) != "https://")
                    value = "http://" + value;
                _webSite = value;
            }
        }

        /// <summary>
        /// Encoding type for the website
        /// </summary>
        [Browsable(false)]
        public string WebSiteEncodingType { get; set; } = Encoding.Default.ToString();

        /// <summary>
        /// Reg expression list for the website parsing of the share update values
        /// </summary>
        [Browsable(false)]
        public RegExList RegexList { get; set; }

        /// <summary>
        /// Culture info for the share
        /// </summary>
        [Browsable(false)]
        public virtual CultureInfo CultureInfo
        {
            get => _cultureInfo;
            set
            {
                _cultureInfo = value;

                // Set culture info to the lists
                AllBuyEntries?.SetCultureInfo(_cultureInfo);
                AllSaleEntries?.SetCultureInfo(_cultureInfo);

                Thread.CurrentThread.CurrentCulture = CultureInfo;
                CurrencyUnit = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
            }
        }

        /// <summary>
        /// Culture info as string
        /// </summary>
        [Browsable(false)]
        public string CultureInfoAsStr => CultureInfo.ToString();

        /// <summary>
        /// Currency unit of the share
        /// </summary>
        [Browsable(false)]
        public string CurrencyUnit { get; private set; }

        /// <summary>
        /// Type of the share (Fond, ETF....)
        /// </summary>
        [Browsable(false)]
        public int ShareType { get; set; }

        #endregion General properties

        #region Value units properties

        /// <summary>
        /// Unit for percentage values
        /// </summary>
        [Browsable(false)]
        public static string PercentageUnit { set; get; } = @"%";

        /// <summary>
        /// Unit for the piece values
        /// </summary>
        [Browsable(false)]
        public static string PieceUnit { set; get; } = @"stk.";

        #endregion Value units properties

        #region Volume properties

        /// <summary>
        /// Current share volume
        /// </summary>
        [Browsable(false)]
        public virtual decimal Volume { get; set; } = decimal.MinValue / 2;

        /// <summary>
        /// Current share volume as string
        /// </summary>
        [Browsable(false)]
        public string VolumeAsStr => Helper.FormatDecimal(Volume, Helper.Volumefivelength, true, Helper.Volumenonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Current share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string VolumeAsStrUnit => Helper.FormatDecimal(Volume, Helper.Volumefivelength, true, Helper.Volumenonefixlength, true, PieceUnit, CultureInfo);

        #endregion Volume properties

        #region Buy properties

        /// <summary>
        /// Total buy value of the share without reduction and costs
        /// </summary>
        [Browsable(false)]
        public decimal BuyMarketValueTotal { get; internal set; }

        /// <summary>
        /// Total buy value of the share without reduction and costs as string
        /// </summary>
        [Browsable(false)]
        public string BuyMarketValueTotalAsStr => Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Total buy value of the share without reduction and costs as string with unit
        /// </summary>
        [Browsable(false)]
        public string BuyMarketValueTotalAsStrUnit => Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Average buy price of all buys
        /// </summary>
        [Browsable(false)]
        public decimal AverageBuyPrice { get; internal set; }

        /// <summary>
        /// Average buy price of all buys as string
        /// </summary>
        [Browsable(false)]
        public string AverageBuyPriceAsStr => Helper.FormatDecimal(AverageBuyPrice, Helper.Currencyfivelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Average buy price of all buys as string with unit
        /// </summary>
        [Browsable(false)]
        public string AverageBuyPriceAsStrUnit => Helper.FormatDecimal(AverageBuyPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// List of all buys of this share
        /// </summary>
        [Browsable(false)]
        public AllBuysOfTheShare AllBuyEntries { get; set; } = new AllBuysOfTheShare();

        #endregion Buy properties

        #region Sales properties

        /// <summary>
        /// Total sale value of the share
        /// </summary>
        [Browsable(false)]
        public virtual decimal SalePurchaseValueTotal { get; set; } = decimal.MinValue / 2;

        /// <summary>
        /// Total sale value of the share as string
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueTotalAsStr => Helper.FormatDecimal(SalePurchaseValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Total sale value of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueTotalAsStrUnit => Helper.FormatDecimal(SalePurchaseValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// List of all sales of this share
        /// </summary>
        [Browsable(false)]
        public AllSalesOfTheShare AllSaleEntries { get; set; } = new AllSalesOfTheShare();

        #endregion Sales properties

        #region Price properties

        /// <summary>
        /// Current price of the share. Will be updated via the Internet
        /// </summary>
        [Browsable(false)]
        public virtual decimal CurPrice { get; set; } = decimal.MinValue / 2;

        /// <summary>
        /// Current price of the share as string
        /// </summary>
        [Browsable(false)]
        public string CurPriceAsStr => Helper.FormatDecimal(CurPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Current price of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string CurPriceAsStrUnit => Helper.FormatDecimal(CurPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Previous day price of the share. Will be updated via the Internet
        /// </summary>
        [Browsable(false)]
        public virtual decimal PrevDayPrice { get; set; } = decimal.MinValue / 2;

        /// <summary>
        /// Previous day price of the share as string
        /// </summary>
        [Browsable(false)]
        public string PrevDayPriceAsStr => Helper.FormatDecimal(PrevDayPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Previous day price of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string PrevDayPriceAsStrUnit => Helper.FormatDecimal(PrevDayPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Current and previous day price of the share as string with unit and a line break
        /// </summary>
        [Browsable(false)]
        public string CurPrevPriceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CurPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PrevDayPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Price properties

        #region Performance properties

        /// <summary>
        /// Price difference between the current and the previous day of the share
        /// </summary>
        [Browsable(false)]
        public decimal PrevDayDifference { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Price difference between the current and the previous day of the share as string
        /// </summary>
        [Browsable(false)]
        public string PrevDayDifferenceAsStr => Helper.FormatDecimal(PrevDayDifference, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Price difference between the current and the previous day of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string PrevDayDifferenceAsStrUnit => Helper.FormatDecimal(PrevDayDifference, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Performance in percent to the previous day of the share
        /// </summary>
        [Browsable(false)]
        public decimal PrevDayPerformance { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance in percent to the previous day of the share as string
        /// </summary>
        [Browsable(false)]
        public string PrevDayPerformanceAsStr => Helper.FormatDecimal(PrevDayPerformance, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Performance in percent to the previous day of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string PrevDayPerformanceAsStrUnit => Helper.FormatDecimal(PrevDayPerformance, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);

        /// <summary>
        /// Difference between the current and the previous day of the share and the performance in percent as string
        /// </summary>
        [Browsable(false)]
        public string PrevDayDifferencePerformanceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PrevDayDifference, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
                value += "\n" + Helper.FormatDecimal(PrevDayPerformance, Helper.Percentagefourlength, true, Helper.Percentagenonefixlength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Profit or loss to the previous day of the hole share volume
        /// </summary>
        [Browsable(false)]
        public decimal PrevDayProfitLoss { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Profit or loss to the previous day of the hole share volume as string
        /// </summary>
        [Browsable(false)]
        public string PrevDayProfitLossAsStr => Helper.FormatDecimal(PrevDayProfitLoss, Helper.Currencythreelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Profit or loss to the previous day of the hole share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string PrevDayProfitLossAsStrUnit => Helper.FormatDecimal(PrevDayProfitLoss, Helper.Percentagethreelength, true, Helper.Percentagenonefixlength, true, @"", CultureInfo);

        /// <summary>
        /// Image which indicates the performance of the share to the previous day 
        /// </summary>
        [Browsable(false)]
        public Image ImagePrevDayPerformance { get; internal set; }

        /// <summary>
        /// List with the possible images for the performance to the previous day of a share
        /// </summary>
        [Browsable(false)]
        public List<Image> ImageListPrevDayPerformance
        {
            get => _imageListPrevDayPerformance;
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

        #endregion Performance properties

        #region Purchase properties
        
        /// <summary>
        /// Purchase value of the current share volume
        /// </summary>
        [Browsable(false)]
        public virtual decimal PurchaseValue { get; set; } = decimal.MinValue / 2;

        /// <summary>
        /// Purchase value of the current share volume as string
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueAsStr => Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);

        /// <summary>
        /// Purchase value of the current share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueAsStrUnit => Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);

        #endregion Purchase properties
        
        #endregion Properties

        #region Methods

        #region Constructors

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
            ImagePrevDayPerformance = _imageListPrevDayPerformance[0];

            PercentageUnit = percentageUnit;
            PieceUnit = pieceUnit;

            _iObjectCounter++;

#if DEBUG_SHAREOBJECT
            Console.WriteLine("ObjectCounter: {0}", ObjectCounter);
#endif
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
        /// <param name="webSite">Website address of the share</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
        /// <param name="shareType">Type of the share</param>
        /// <param name="document">Document of the first buy</param>
        public ShareObject(
            string wkn, string addDateTime, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShareDate, DateTime lastUpdateShareTime,
            decimal price, decimal volume, decimal reduction, decimal costs,
            string webSite, List<Image> imageListForDayBeforePerformance,
            RegExList regexList, CultureInfo cultureInfo,
            int shareType, string document)
        {
#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("ShareObject()");
            Console.WriteLine("WKN: {0}", wkn);
            Console.WriteLine("addDateTime: {0}", addDateTime);
            Console.WriteLine("name: {0}", name);
            Console.WriteLine("lastUpdateInternet: {0}", lastUpdateInternet);
            Console.WriteLine("lastUpdateShareDate: {0}", lastUpdateShareDate);
            Console.WriteLine("lastUpdateShareTime: {0}", lastUpdateShareTime);
            Console.WriteLine("price: {0}", price);
            Console.WriteLine("volume: {0}", volume);
            Console.WriteLine("reduction: {0}", reduction);
            Console.WriteLine("costs: {0}", costs);
            Console.WriteLine("purchaseValue: {0}", purchaseValue);
            Console.WriteLine("webSite: {0}", webSite);
            Console.WriteLine("cultureInfo.Name: {0}", cultureInfo.Name);
            Console.WriteLine("document: {0}", document);
#endif
            Wkn = wkn;
            AddDateTime = addDateTime;
            Name = name;
            LastUpdateInternet = lastUpdateInternet;
            LastUpdateDate = lastUpdateShareDate;
            LastUpdateTime = lastUpdateShareTime;
            CurPrice = price;
            WebSite = webSite;
            ImageListPrevDayPerformance = imageListForDayBeforePerformance;
            ImagePrevDayPerformance = ImageListPrevDayPerformance[0];
            RegexList = regexList;
            CultureInfo = cultureInfo;
            ShareType = shareType;

            AddBuy(AddDateTime, volume, price, reduction, costs, document);

            _iObjectCounter++;

#if DEBUG_SHAREOBJECT
            Console.WriteLine("ObjectCounter: {0}", ObjectCounter);
#endif
        }

        #endregion Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~ShareObject()
        {
            _iObjectCounter--;
            Dispose(false);
        }

        #endregion Destructor

        #region Buy methods

        /// <summary>
        /// This function adds the buy for the share to the dictionary
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <param name="decVolume">Buy volume</param>
        /// <param name="decPrice">Price for one share</param>
        /// <param name="decReduction">Reduction of the buy</param>
        /// <param name="decCosts">Costs of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strDateTime, decimal decVolume, decimal decPrice, decimal decReduction, decimal decCosts, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("AddBuy()");
                Console.WriteLine("strDateTime: {0}", strDateTime);
                Console.WriteLine("decVolume: {0}", decVolume);
                Console.WriteLine("decPrice: {0}", decPrice);
                Console.WriteLine("decReduction: {0}", decReduction);
                Console.WriteLine("decCosts: {0}", decCosts);
                Console.WriteLine("strDoc: {0}", strDoc);
#endif
                if (!AllBuyEntries.AddBuy(strDateTime, decVolume, decPrice, decReduction, decCosts, strDoc))
                    return false;

                // Set buy value of the share
                BuyMarketValueTotal = AllBuyEntries.BuyMarketValueTotal;

                // Set new volume
                if (Volume == decimal.MinValue / 2)
                    Volume = 0;
                Volume += decVolume;

                // Recalculate MarketValue
                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                PurchaseValue += AllBuyEntries.GetBuyObjectByDateTime(strDateTime).MarketValueReductionCosts;

                // Recalculate buy price average
                if (PurchaseValue > 0 && Volume > 0)
                    AverageBuyPrice = PurchaseValue / Volume;
                else
                    AverageBuyPrice = 0;

#if DEBUG_SHAREOBJECT
                Console.WriteLine("Volume: {0}", Volume);
                Console.WriteLine("PurchaseValue: {0}", PurchaseValue);
                Console.WriteLine("BuyValueTotal: {0}", BuyMarketValueTotal);
                Console.WriteLine("AverageBuyPrice: {0}", AverageBuyPrice);
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
                // Get BuyObject by date and time and add the sale PurchaseValue and value to the share
                var buyObject = AllBuyEntries.GetBuyObjectByDateTime(strDateTime);
                if (buyObject != null)
                {
                    Volume -= buyObject.Volume;
                    PurchaseValue -= buyObject.MarketValueReductionCosts;
                    BuyMarketValueTotal = AllBuyEntries.BuyMarketValueTotal;

                    // Recalculate buy price average
                    if (PurchaseValue > 0 && Volume > 0)
                        AverageBuyPrice = PurchaseValue / Volume;
                    else
                        AverageBuyPrice = 0;

#if DEBUG_SHAREOBJECT
                    Console.WriteLine("Volume: {0}", Volume);
                    Console.WriteLine("PurchaseValue: {0}", PurchaseValue);
                    Console.WriteLine("BuyValueTotal: {0}", BuyMarketValueTotal);
#endif
                    // Remove buy by date and time
                    if (!AllBuyEntries.RemoveBuy(strDateTime))
                        return false;
                }
                else
                    return false;

                return true;
            }
            catch(Exception ex)
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
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decBuyPrice">Buy price of the share</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="decCosts">Costs of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strDate, decimal decVolume, decimal decBuyPrice, decimal decSalePrice, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, decimal decCosts, string strDoc = "")
        {
            try
            {
#if DEBUG_SHAREOBJECT
                Console.WriteLine("");
                Console.WriteLine("AddSale()");
                Console.WriteLine("strDateTime: {0}", strDateTime);
                Console.WriteLine("decVolume: {0}", decVolume);
                Console.WriteLine("decBuyPrice: {0}", decBuyPrice);
                Console.WriteLine("decSalePrice: {0}", decSalePrice);
                Console.WriteLine("decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine("decCapitalGains: {0}", decCapitalGains);
                Console.WriteLine("decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine("decCosts: {0}", decCosts);
                Console.WriteLine("strDoc: {0}", strDoc);
#endif
                if (!AllSaleEntries.AddSale(strDate, decVolume, decBuyPrice, decSalePrice, decTaxAtSource, decCapitalGainsTax,
                                            decSolidarityTax, decCosts, strDoc))
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
                    if (Volume > 0 && PurchaseValue > 0)
                    {
                        SalePurchaseValueTotal += AverageBuyPrice * decVolume;
                        PurchaseValue -= AverageBuyPrice * decVolume;
                    }
                    else
                    {
                        SalePurchaseValueTotal += PurchaseValue;
                        PurchaseValue = 0;
                    }
                }

#if DEBUG_SHAREOBJECT
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValueAsStr);
                Console.WriteLine(@"SalePurchaseValueTotal: {0}", SalePurchaseValueTotalAsStr);
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
                // Get SaleObject by date and time and add the sale deposit and value to the share
                var saleObject = AllSaleEntries.GetSaleObjectByDateTime(strDateTime);
                if (saleObject != null)
                {
                    Volume += saleObject.Volume;
                    PurchaseValue += saleObject.PurchaseValue;
                    SalePurchaseValueTotal -= saleObject.PurchaseValue;


                    // Recalculate buy price average
                    if (PurchaseValue > 0 && Volume > 0)
                        AverageBuyPrice = PurchaseValue / Volume;
                    else
                        AverageBuyPrice = 0;

                    // Remove sale by date and time
                    if (!AllSaleEntries.RemoveSale(strDateTime))
                    return false;

#if DEBUG_SHAREOBJECT
                Console.WriteLine("Volume: {0}", Volume);
                Console.WriteLine("PurchaseValue: {0}", PurchaseValue);
                Console.WriteLine("SalePurchaseValueTotal: {0}", SalePurchaseValueTotalAsStr);
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
        /// This function calculates the profit or loss to the previous day of the hole share volume
        /// </summary>
        public void CalculatePrevDayProfitLoss()
        {
            if (CurPrice > decimal.MinValue / 2
                && PrevDayPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                )
            {
                PrevDayProfitLoss = (CurPrice - PrevDayPrice) * Volume;
            }

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateDayBeforeProfitLoss()");
            Console.WriteLine("CurPrice: {0}", CurPrice);
            Console.WriteLine("PrevDayPrice: {0}", PrevDayPrice);
            Console.WriteLine("Volume: {0}", Volume);
            Console.WriteLine("PrevDayProfitLoss: {0}", PrevDayProfitLoss);
            Console.WriteLine("");
#endif
        }

        /// <summary>
        /// This function calculates the performance to the previous day
        /// </summary>
        public void CalculatePrevDayPerformance()
        {
            if (CurPrice > decimal.MinValue / 2
                && PrevDayPrice > decimal.MinValue / 2
                )
            {
                PrevDayPerformance = 100 - (PrevDayPrice * 100 / CurPrice);
                PrevDayDifference = CurPrice - PrevDayPrice;
            }

            if (ImageListPrevDayPerformance == null || ImageListPrevDayPerformance.Count <= 0) return;

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

#if DEBUG_SHAREOBJECT
            Console.WriteLine("");
            Console.WriteLine("CalculateDayBeforePerformance()");
            Console.WriteLine("CurPrice: {0}", CurPrice);
            Console.WriteLine("PrevDayPrice: {0}", PrevDayPrice);
            Console.WriteLine("PrevDayPerformance: {0}", PrevDayPerformance);
            Console.WriteLine("PrevDayDifference: {0}", PrevDayDifference);
#endif
        }

        #endregion Performance methods

        /// <summary>
        /// This function search for the correct website RegEx and
        /// sets the RegEx list and encoding to the share
        /// </summary>
        /// <param name="webSiteRegexList">List of the websites and their RegEx list</param>
        /// <returns>Flag if a website configuration for share exists</returns>
        public bool SetWebSiteRegexListAndEncoding(List<WebSiteRegex> webSiteRegexList)
        {
            // Flag if the website of the current share exists in the website configuration list
            var bRegexFound = false;

            // Loop through the given website configuration list
            foreach (var webSiteRegexElement in webSiteRegexList)
            {
                // Check if the current share object use the current website configuration
                if (!WebSite.Contains(webSiteRegexElement.WebSiteName)) continue;

                // Set the website configuration to the share object
                RegexList = webSiteRegexElement.WebSiteRegexList;
                WebSiteEncodingType = webSiteRegexElement.WebSiteEncodingType;
                bRegexFound = true;
                break;
            }

            return bRegexFound;
        }

        /// <inheritdoc />
        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="bDisposing">Flag if dispose is called</param>
        protected virtual void Dispose(bool bDisposing)
        {
            if (Disposed)
                return;

            if (bDisposing)
            {
                // Free any other managed objects here.
                // -
            }

            // Free any unmanaged objects here.
            // -

            Disposed = true;
        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This class is a comparer class which compares to share objects.
    /// </summary>
    internal class ShareObjectListComparer : IComparer<ShareObject>
    {
        public int Compare(ShareObject object1, ShareObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return string.CompareOrdinal(object1.NameAsStr, object2.NameAsStr);
        }
    }

    /// <summary>
    /// Search object class for the share object.
    /// </summary>
    public class ShareObjectSearch
    {
        #region Variables

        private readonly string _searchString;

        #endregion Variables

        /// <summary>
        /// Constructor with the given search string
        /// </summary>
        /// <param name="searchString"></param>
        public ShareObjectSearch(string searchString)
        {
            _searchString = searchString;
        }

        /// <summary>
        /// Comparer for the share object WKN
        /// </summary>
        /// <param name="shareObject"></param>
        /// <returns></returns>
        public bool Compare(ShareObject shareObject)
        {
            return shareObject.Wkn == _searchString;
        }
    }
}
