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
//#define DEBUG_SHARE_OBJECT

using Parser;
using SharePortfolioManager.Classes.Buys;
using SharePortfolioManager.Classes.ParserRegex;
using SharePortfolioManager.Classes.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace SharePortfolioManager.Classes.ShareObjects
{
    /// <summary>
    /// This class is the base class of a share.
    /// The class stores the following things:
    /// - general information of the share
    /// - all buys of a share
    /// - all sales of a share
    /// </summary>
    [Serializable]
    public class ShareObject : IDisposable, ICloneable
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
        internal const short ShareObjectTagCount = 13;

        /// <summary>
        /// Stores a list of images for the previous day performance visualization
        /// </summary>
        private List<Image> _imageListPrevDayPerformance;

        /// <summary>
        /// Stores the website url of the share for the internet update
        /// </summary>
        private string _updateWebSiteUrl;

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        #endregion General variables

        #region Portfolio XML parts values

        /// <summary>
        /// Stores the XML tag for the portfolio
        /// </summary>
        internal const string GeneralPortfolioAttrName = @"Portfolio";

        /// <summary>
        /// Stores the XML tag for the shares
        /// </summary>
        internal const string GeneralShareAttrName = @"Share";

        #endregion Portfolio XML parts values

        #region General XML variables

        /// <summary>
        /// Stores the XML attribute name for the WKN
        /// </summary>
        internal const string GeneralWknAttrName= "WKN";

        /// <summary>
        /// Stores the XML attribute name for the share name
        /// </summary>
        internal const string GeneralNameAttrName = "Name";

        /// <summary>
        /// Stores the XML attribute name for the flag if the share should be updated
        /// </summary>
        internal const string GeneralUpdateAttrName = "Update";

        /// <summary>
        /// Stores the XML attribute name for the share stock market launch date
        /// </summary>
        internal const string GeneralStockMarketLaunchDateAttrName = "StockMarketLaunchDate";

        /// <summary>
        /// Stores the XML attribute name for the last update from the internet
        /// </summary>
        internal const string GeneralLastUpdateInternetAttrName = "LastUpdateInternet";

        /// <summary>
        /// Stores the XML attribute name for the last update of the share
        /// </summary>
        internal const string GeneralLastUpdateShareDateAttrName = "LastUpdateShareDate";

        /// <summary>
        /// Stores the XML attribute name for the current share price
        /// </summary>
        internal const string GeneralSharePriceAttrName = "SharePrice";

        /// <summary>
        /// Stores the XML attribute name for the day before share price
        /// </summary>
        internal const string GeneralSharePriceBeforeAttrName = "SharePriceBefore";

        /// <summary>
        /// Stores the XML attribute name for the website of the share update
        /// </summary>
        internal const string GeneralWebSiteAttrName = "WebSite";

        /// <summary>
        /// Stores the XML attribute name for the culture of the share
        /// </summary>
        internal const string GeneralCultureAttrName = "Culture";

        /// <summary>
        /// Stores the XML attribute name for the share type
        /// </summary>
        internal const string GeneralShareTypeAttrName = "ShareType";

        /// <summary>
        /// Stores the XML attribute name for the daily value entries
        /// </summary>
        internal const string GeneralDailyValuesAttrName = "DailyValues";

        /// <summary>
        /// Stores the XML attribute name for the buy entries
        /// </summary>
        internal const string GeneralBuysAttrName = "Buys";

        /// <summary>
        /// Stores the XML attribute name for the sale entries
        /// </summary>
        internal const string GeneralSalesAttrName = "Sales";

        /// <summary>
        /// Stores the XML attribute name for the brokerage entries
        /// </summary>
        internal const string GeneralBrokeragesAttrName = "Brokerages";

        /// <summary>
        /// Stores the XML attribute name for the dividend entries
        /// </summary>
        internal const string GeneralDividendsAttrName = "Dividends";

        #endregion General XML variables

        #region Buy XML variables

        /// <summary>
        /// Stores the XML tag name prefix of a buy entry
        /// </summary>
        internal const string BuyTagNamePre = "Buy";

        /// <summary>
        /// Stores the XML attribute name for the buy Guid
        /// </summary>
        internal const string BuyGuidAttrName = "Guid";

        /// <summary>
        /// Stores the XML attribute name for the buy order number
        /// </summary>
        internal const string BuyOrderNumberAttrName = "OrderNumber";

        /// <summary>
        /// Stores the XML attribute name for the buy date
        /// </summary>
        internal const string BuyDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the volume of a buy
        /// </summary>
        internal const string BuyVolumeAttrName = "Volume";

        /// <summary>
        /// Stores the XML attribute name for the sold volume of a buy
        /// </summary>
        internal const string BuyVolumeSoldAttrName = "VolumeSold";

        /// <summary>
        /// Stores the XML attribute name for buy price of a share of a buy
        /// </summary>
        internal const string BuyPriceAttrName = "Price";

        /// <summary>
        /// Stores the XML attribute name for the brokerage Guid of a buy
        /// </summary>
        internal const string BuyBrokerageGuidAttrName = "BrokerageGuid";

        /// <summary>
        /// Stores the XML attribute name for the document of a buy
        /// </summary>
        internal const string BuyDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the buy
        /// </summary>
        internal const short BuyAttrCount = 8;

        #endregion Buy XML variables

        #region Sale XML variables

        /// <summary>
        /// Stores the XML tag name prefix of a sale entry
        /// </summary>
        internal const string SaleTagNamePre = "Sale";

        /// <summary>
        /// Stores the XML attribute name for the sale Guid
        /// </summary>
        internal const string SaleGuidAttrName = "Guid";

        /// <summary>
        /// Stores the XML attribute name for the sale date
        /// </summary>
        internal const string SaleDateAttrName = "Date";

        /// <summary>
        /// Stores the XML attribute name for the sale date
        /// </summary>
        internal const string SaleOrderNumberAttrName = "OrderNumber";

        /// <summary>
        /// Stores the XML attribute name for the buy volume of a sale
        /// </summary>
        internal const string SaleVolumeAttrName = "Volume";

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
        /// Stores the XML attribute name for the reduction of a sale
        /// </summary>
        internal const string SaleReductionAttrName = "Reduction";

        /// <summary>
        /// Stores the XML attribute name for the brokerage Guid of a sale
        /// </summary>
        internal const string SaleBrokerageGuidAttrName = "BrokerageGuid";

        /// <summary>
        /// Stores the XML attribute name for the document of a sale
        /// </summary>
        internal const string SaleDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the XML attribute name for the used buys
        /// </summary>
        internal const string SaleUsedBuysAttrName = "UsedBuys";

        /// <summary>
        /// Stores the XML attribute name for the used buy
        /// </summary>
        internal const string SaleUsedBuyAttrName = "UsedBuy";

        /// <summary>
        /// Stores the XML attribute name for the buy date of a sale
        /// </summary>
        internal const string SaleBuyDateAttrName = "BuyDate";

        /// <summary>
        /// Stores the XML attribute name for the buy volume of a sale
        /// </summary>
        internal const string SaleBuyVolumeAttrName = "BuyVolume";

        /// <summary>
        /// Stores the XML attribute name for the brokerage of one share of a sale
        /// </summary>
        internal const string SaleUsedBuyReductionAttrName = "Reduction";

        /// <summary>
        /// Stores the XML attribute name for the brokerage of one share of a sale
        /// </summary>
        internal const string SaleUsedBuyBrokerageAttrName = "Brokerage";

        /// <summary>
        /// Stores the XML attribute name for the buy price of one share of a sale
        /// </summary>
        internal const string SaleBuyPriceAttrName = "BuyPrice";

        /// <summary>
        /// Stores the XML attribute name for the buy Guid of the sale
        /// </summary>
        internal const string SaleBuyGuidAttrName = "BuyGuid";

        /// <summary>
        /// Stores the attribute count for the sale
        /// </summary>
        internal const short SaleAttrCount = 11;

        /// <summary>
        /// Stores the used buys count for the sale
        /// </summary>
        internal const short SaleUsedBuysCount = 1;

        /// <summary>
        /// Stores the attribute count for the used buy information
        /// </summary>
        internal const short SaleAttrCountUsedBuys = 6;

        #endregion Sale XML variables

        #region Brokerage XML variables

        /// <summary>
        /// Stores the tag name prefix of a brokerage entry
        /// </summary>
        internal const string BrokerageTagNamePre = "Brokerage";

        /// <summary>
        /// Stores the XML attribute name for the brokerage Guid
        /// </summary>
        internal const string BrokerageGuidAttrName = "Guid";

        /// <summary>
        /// Stores the attribute name for the flag if the brokerage is a part of a buy
        /// </summary>
        internal const string BrokerageBuyPartAttrName = "BuyPart";

        /// <summary>
        /// Stores the attribute name for the flag if the brokerage is a part of a sale
        /// </summary>
        internal const string BrokerageSalePartAttrName = "SalePart";

        /// <summary>
        /// Stores the attribute name for the buy or sale Guid of the brokerage
        /// </summary>
        internal const string BrokerageGuidBuySaleAttrName = "GuidBuySale";

        /// <summary>
        /// Stores the attribute name for the date 
        /// </summary>
        internal const string BrokerageDateAttrName = "Date";

        /// <summary>
        /// Stores the attribute name for the provision value
        /// </summary>
        internal const string BrokerageProvisionAttrName = "Provision";

        /// <summary>
        /// Stores the attribute name for the broker fee value
        /// </summary>
        internal const string BrokerageBrokerFeeAttrName = "BrokerFee";

        /// <summary>
        /// Stores the attribute name for the trader place fee value
        /// </summary>
        internal const string BrokerageTraderPlaceFeeAttrName = "TraderPlaceFee";

        /// <summary>
        /// Stores the attribute name for the reduction value
        /// </summary>
        internal const string BrokerageReductionAttrName = "Reduction";

        /// <summary>
        /// Stores the XML attribute name for the document of a brokerage
        /// </summary>
        internal const string BrokerageDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the brokerage
        /// </summary>
        internal const short BrokerageAttrCount = 10;

        #endregion Brokerage XML variables

        #region Dividends XML variables

        /// <summary>
        /// Stores the XML attribute name for the dividend payout interval
        /// </summary>
        internal const string DividendPayoutIntervalAttrName = "PayoutInterval";

        /// <summary>
        /// Stores the XML tag name of a dividend entry
        /// </summary>
        internal const string DividendTagName = "Dividend";

        /// <summary>
        /// Stores the XML attribute name for the dividend Guid
        /// </summary>
        internal const string DividendGuidAttrName = "Guid";

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

        #region DailyValues XML variables

        /// <summary>
        /// Stores the tag name prefix of a daily values entry
        /// </summary>
        internal const string DailyValuesTagNamePre = "Entry";

        /// <summary>
        /// Stores the XML attribute name for the website for the daily values update
        /// </summary>
        internal const string DailyValuesWebSiteAttrName = "WebSite";

        /// <summary>
        /// Stores the XML attribute name for the date of the daily value
        /// </summary>
        public const string DailyValuesDateTagName = "D";

        /// <summary>
        /// Stores the XML attribute name for the closing price of the daily value
        /// </summary>
        public const string DailyValuesClosingPriceTagName = "C";

        /// <summary>
        /// Stores the XML attribute name for the top price of the daily value
        /// </summary>
        public const string DailyValuesOpeningPriceTagName = "O";

        /// <summary>
        /// Stores the XML attribute name for the opening price of the daily value
        /// </summary>
        public const string DailyValuesTopTagName = "T";

        /// <summary>
        /// Stores the XML attribute name for the bottom price of the daily value
        /// </summary>
        public const string DailyValuesBottomTagName = "B";

        /// <summary>
        /// Stores the XML attribute name for the volume price of the daily value
        /// </summary>
        public const string DailyValuesVolumeTagName = "V";

        /// <summary>
        /// Stores the attribute count for the daily values
        /// </summary>
        public const short DailyValuesAttrCount = 6; // Date / ClosingPrice / OpeningPrice / Top / Bottom / Volume

        #endregion DailyValues XML variables

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
        /// Date and time of share add ( first buy )
        /// </summary>
        [Browsable(false)]
        public string AddDateTime { get; set; }

        /// <summary>
        /// Date and time of the stock market launch
        /// </summary>
        [Browsable(false)]
        public string StockMarketLaunchDate { get; set; }

        /// <summary>
        /// Name of the share
        /// </summary>
        [Browsable(false)]
        public string Name { get; set; }

        /// <summary>
        /// Flag if the share should be updated via internet
        /// </summary>
        [Browsable(false)]
        public bool DoInternetUpdate { get; set; } = true;

        /// <summary>
        /// Flag if the share should be updated via internet as string
        /// </summary>
        [Browsable(false)]
        public string DoInternetUpdateAsStr => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DoInternetUpdate.ToString());

        /// <summary>
        /// Flag if a website configuration for the share website has been found or not
        /// </summary>
        [Browsable(false)]
        public bool WebSiteConfigurationFound { get; set; }

        /// <summary>
        /// Date and time of the last share update via internet
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateViaInternet { get; set; }

        /// <summary>
        /// Date and time of the last update share of the parsed internet side
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateShare { get; set; }

        /// <summary>
        /// Website url for the share update value parsing
        /// </summary>
        [Browsable(false)]
        public string UpdateWebSiteUrl
        {
            get => _updateWebSiteUrl;
            set
            {
                // Check if "http://" is in front of the website 
                if (DoInternetUpdate && value.Substring(0, 7) != "http://" && value.Substring(0, 8) != "https://")
                    value = "http://" + value;
                _updateWebSiteUrl = value;
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

        #region Daily values

        /// <summary>
        /// Website url of the share for the update of the daily values
        /// </summary>
        [Browsable(false)]
        public string DailyValuesUpdateWebSiteUrl { get; set; }

        /// <summary>
        /// List of the daily values of the share ( Date / ClosingPrice / OpeningPrice / Top / Bottom / Volume )
        /// </summary>
        [Browsable(false)]
        public List<Parser.DailyValues> DailyValues { get; } = new List<Parser.DailyValues>();

        #endregion Daily values

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
        public string VolumeAsStr => Helper.FormatDecimal(Volume, Helper.VolumeFiveLength, false, Helper.VolumeTwoFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Current share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string VolumeAsStrUnit => Helper.FormatDecimal(Volume, Helper.VolumeFiveLength, false, Helper.VolumeTwoFixLength, true, PieceUnit, CultureInfo);

        #endregion Volume properties

        #region Price properties

        /// <summary>
        /// Current price of the share. Will be updated via the internet
        /// </summary>
        [Browsable(false)]
        public virtual decimal CurPrice { get; set; } = decimal.MinValue / 2;

        /// <summary>
        /// Current price of the share as string
        /// </summary>
        [Browsable(false)]
        public string CurPriceAsStr => Helper.FormatDecimal(CurPrice, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Current price of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string CurPriceAsStrUnit => Helper.FormatDecimal(CurPrice, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Previous day price of the share. Will be updated via the internet
        /// </summary>
        [Browsable(false)]
        public virtual decimal PrevPrice { get; set; } = decimal.MinValue / 2;

        /// <summary>
        /// Previous day price of the share as string
        /// </summary>
        [Browsable(false)]
        public string PrevPriceAsStr => Helper.FormatDecimal(PrevPrice, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Previous day price of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string PrevPriceAsStrUnit => Helper.FormatDecimal(PrevPrice, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Current and previous day price of the share as string with unit and a line break
        /// </summary>
        [Browsable(false)]
        public string CurPrevPriceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CurPrice, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PrevPrice, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Price properties

        #region Current to previous day performance properties

        /// <summary>
        /// Price difference between the current and the previous day of the share
        /// </summary>
        [Browsable(false)]
        public decimal CurPrevDayPriceDifference { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Price difference between the current and the previous day of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string CurPrevDayPriceDifferenceAsStrUnit => Helper.FormatDecimal(CurPrevDayPriceDifference, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Performance of the price in percent from the current price to the previous day price of the share
        /// </summary>
        [Browsable(false)]
        public decimal CurPrevDayPricePerformance { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance of the price in percent from the current price to the previous day price of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string CurPrevDayPricePerformanceAsStrUnit => Helper.FormatDecimal(CurPrevDayPricePerformance, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);

        /// <summary>
        /// Difference and performance from the current price to the previous day price of the share as string with unit
        /// </summary>
        [Browsable(false)]
        public string CurPrevDayPriceDifferencePerformanceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CurPrevDayPriceDifference, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(CurPrevDayPricePerformance, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Profit or loss to the previous day of the hole share volume
        /// </summary>
        [Browsable(false)]
        public decimal CurPrevDayProfitLoss { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Profit or loss to the previous day of the hole share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string CurPrevDayProfitLossAsStrUnit => Helper.FormatDecimal(CurPrevDayProfitLoss, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, @"", CultureInfo);

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

        #endregion Current to previous day performance properties

        #region Buy value properties

        /// <summary>
        /// List of all buys of this share
        /// </summary>
        [Browsable(false)]
        public AllBuysOfTheShare AllBuyEntries { get; set; } = new AllBuysOfTheShare();

        #endregion Buy value properties

        #region Sale properties

        /// <summary>
        /// List of all sales of this share
        /// </summary>
        [Browsable(false)]
        public AllSalesOfTheShare AllSaleEntries { get; set; } = new AllSalesOfTheShare();

        /// <summary>
        /// Stores the value of the sold purchase used by the sales of the share
        /// </summary>
        [Browsable(false)]
        public virtual decimal SoldPurchaseValue { get; set; } = decimal.MinValue / 2;

        #endregion Sale properties

        #endregion Properties

        #region Methods

        #region Constructors

        /// <summary>
        /// Standard constructor
        /// </summary>
        public ShareObject(List<Image> imageList, string percentageUnit, string pieceUnit)
        {
            _imageListPrevDayPerformance = imageList;
            ImagePrevDayPerformance = _imageListPrevDayPerformance[0];

            PercentageUnit = percentageUnit;
            PieceUnit = pieceUnit;

            _iObjectCounter++;
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="wkn">WKN number of the share</param>
        /// <param name="addDateTime">Date and time of the add</param>
        /// <param name="stockMarketLaunchDate">Date of the stock market launch</param>
        /// <param name="name">Name of the share</param>
        /// <param name="lastUpdateInternet">Date and time of the last update from the Internet</param>
        /// <param name="lastUpdateShare">Date and time of the last update on the Internet site of the share</param>
        /// <param name="price">Current price of the share</param>
        /// <param name="webSite">Website address of the share</param>
        /// <param name="dailyValuesWebSite">WebSite for the daily values update</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
        /// <param name="shareType">Type of the share</param>
        public ShareObject(
            string wkn, string addDateTime, string stockMarketLaunchDate, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShare,
            decimal price, string webSite, string dailyValuesWebSite, List<Image> imageListForDayBeforePerformance,
            RegExList regexList, CultureInfo cultureInfo,
            int shareType)
        {
            Wkn = wkn;
            AddDateTime = addDateTime;
            StockMarketLaunchDate = stockMarketLaunchDate;
            Name = name;
            LastUpdateViaInternet = lastUpdateInternet;
            LastUpdateShare = lastUpdateShare;
            // ReSharper disable once VirtualMemberCallInConstructor
            CurPrice = price;
            UpdateWebSiteUrl = webSite;
            DailyValuesUpdateWebSiteUrl = dailyValuesWebSite;
            ImageListPrevDayPerformance = imageListForDayBeforePerformance;
            ImagePrevDayPerformance = ImageListPrevDayPerformance[0];
            RegexList = regexList;
            // ReSharper disable once VirtualMemberCallInConstructor
            CultureInfo = cultureInfo;
            ShareType = shareType;

            _iObjectCounter++;
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

        #region Daily values methods

        /// <summary>
        /// Add the new daily values to the existing daily values list
        /// </summary>
        /// <param name="newDailyValues">List with the new daily values</param>
        /// <returns>Flag if the add of the new values was successful</returns>
        public bool AddNewDailyValues(List<Parser.DailyValues> newDailyValues)
        {
            try
            {
                // Create a list which only contains the new daily values which does not exists already exists in the existing list
                var addList = newDailyValues.Except(DailyValues);

                // Add new daily values to the list
                foreach (var item in addList)
                {
                    DailyValues.Add(item);
                }

                DailyValues.Sort();

                return true;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }
        }

        #endregion Daily values methods

        #region Buy methods

        /// <summary>
        /// Get the buy object of the last added buy
        /// </summary>
        /// <returns>Last added buy object or null</returns>
        public BuyObject GetLastAddedBuy()
        {
            if (AllBuyEntries != null &&
                AllBuyEntries.AllBuysOfTheShareDictionary.Count > 0 &&
                AllBuyEntries.AllBuysOfTheShareDictionary.Last().Value.BuyListYear.Count > 0
            )
                return AllBuyEntries.AllBuysOfTheShareDictionary.Last().Value.BuyListYear.Last();

            return null;
        }

        #endregion Buy methods

        #region Sale methods

        /// <summary>
        /// Get sale object of the last added sale
        /// </summary>
        /// <returns>Last added sale object or null</returns>
        public SaleObject GetLastAddedSale()
        {
            if (AllSaleEntries != null &&
                AllSaleEntries.AllSalesOfTheShareDictionary.Count > 0 &&
                AllSaleEntries.AllSalesOfTheShareDictionary.Last().Value.SaleListYear.Count > 0
            )
                return AllSaleEntries.AllSalesOfTheShareDictionary.Last().Value.SaleListYear.Last();

            return null;
        }

        #endregion Sale methods

        #region Performance methods

        /// <summary>
        /// Calculates the profit or loss to the previous day of the current stock volume
        /// </summary>
        public void CalculateCurPrevDayProfitLoss()
        {
            if (CurPrice > decimal.MinValue / 2
                && PrevPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                )
            {
                CurPrevDayProfitLoss = (CurPrice - PrevPrice) * Volume;
            }
        }

        /// <summary>
        /// Calculates the performance to the previous day of the current stock volume
        /// Also the image for the calculated performance value will be set to the class property value
        /// </summary>
        public void CalculateCurPrevDayPerformance()
        {
            if (CurPrice > decimal.MinValue / 2
                && PrevPrice > decimal.MinValue / 2
                )
            {
                CurPrevDayPricePerformance = (CurPrice * 100 / PrevPrice) - 100;
                CurPrevDayPriceDifference = CurPrice - PrevPrice;
            }

            if (ImageListPrevDayPerformance == null || ImageListPrevDayPerformance.Count <= 0) return;

            if (CurPrevDayPricePerformance < 0)
            {
                ImagePrevDayPerformance = ImageListPrevDayPerformance[1];
            }
            else if (CurPrevDayPricePerformance == 0)
            {
                ImagePrevDayPerformance = ImageListPrevDayPerformance[2];

            }
            else
                ImagePrevDayPerformance = ImageListPrevDayPerformance[3];
        }

        #endregion Performance methods

        /// <summary>
        /// Search for the correct website RegEx and sets the RegEx list.
        /// Also set the website encoding.
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
                if (!UpdateWebSiteUrl.Contains(webSiteRegexElement.WebSiteName)) continue;

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

        /// <inheritdoc />
        /// <summary>
        /// This function allows to clone this object
        /// </summary>
        /// <returns>New clone of the object</returns>
        public object Clone()
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

            return string.CompareOrdinal(object1.Name, object2.Name);
        }
    }

    /// <summary>
    /// Search object class for the share object
    /// It compares if the search string is equal to the WKN of a given share object.
    /// </summary>
    public class ShareObjectSearch
    {
        #region Variables

        /// <summary>
        /// Stores the search string
        /// </summary>
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
