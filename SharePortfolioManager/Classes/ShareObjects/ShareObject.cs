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
using WebParser;

namespace SharePortfolioManager
{
    public class ShareObject : IDisposable
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Flag if the object is already disposed
        /// </summary>
        private bool _bDisposed = false;

        /// <summary>
        /// Counter of the created share objects
        /// </summary>
        static private int _iObjectCounter = 0;

        /// <summary>
        /// Stores the count of the share object tags in the XML
        /// </summary>
        private const short _ShareObjectTagCount = 11;

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
        /// Stores the total share market value (buy and sales)
        /// </summary>
        private decimal _purchaseValue = decimal.MinValue / 2;
        
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

        #endregion General variables

        #region Value units variables

        /// <summary>
        /// Stores the value unit for percentage values
        /// </summary>
        static private string _percentageUnit = @"%";

        /// <summary>
        /// Stores the value unit for piece values
        /// </summary>
        static private string _pieceUnit = @"stk.";

        #endregion Value units variables

        #region Buy variables

        /// <summary>
        /// Stores the total buy value of the share without reduction and costs
        /// </summary>
        private decimal _buyMarketValueTotal = 0;

        /// <summary>
        /// Stores the average buy price of all buys
        /// </summary>
        private decimal _averageBuyPrice = 0;

        /// <summary>
        /// Stores the buys of the share
        /// </summary>
        private AllBuysOfTheShare _allBuyEntries = new AllBuysOfTheShare();

        #endregion Buy variables

        #region Buy XML variables

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

        #endregion Buy XML variables

        #region Sale variables

        /// <summary>
        /// Stores the purchase value of the sale of the share
        /// </summary>
        private decimal _salePurchaseValueTotal = decimal.MinValue / 2;

        /// <summary>
        /// Stores the sales of the share
        /// </summary>
        private AllSalesOfTheShare _allSaleEntries = new AllSalesOfTheShare();

        #endregion Sale variables

        #region Sale XML variables

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
        /// Stores the XML attribute name for the buy price of one share of a sale
        /// </summary>
        private const string _saleBuyPriceAttrName = "BuyPrice";

        /// <summary>
        /// Stores the XML attribute name for the sale price of one share of a sale
        /// </summary>
        private const string _saleSalePriceAttrName = "SalePrice";

        /// <summary>
        /// Stores the XML attribute name for the tax at source of a sale
        /// </summary>
        private const string _saleTaxAtSourceAttrName = "TaxAtSource";

        /// <summary>
        /// Stores the XML attribute name for the capital gains tax of a sale
        /// </summary>
        private const string _saleCapitalGainsTaxAttrName = "CapitalGainsTax";

        /// <summary>
        /// Stores the XML attribute name for the solidarity tax of a sale
        /// </summary>
        private const string _saleSolidarityTaxAttrName = "SolidarityTax";

        /// <summary>
        /// Stores the XML attribute name for the document of a sale
        /// </summary>
        private const string _saleDocumentAttrName = "Doc";

        /// <summary>
        /// Stores the attribute count for the dividend
        /// </summary>
        private const short _saleAttrCount = 8;

        #endregion Sale XML variables

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
        static public int ObjectCounter
        {
            get { return _iObjectCounter / 2; } // Divided by two because FinalValue and MarketValue
        }

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

        [Browsable(false)]
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

        [Browsable(false)]
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
        public virtual CultureInfo CultureInfo
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

        #endregion General properties

        #region Value units properties

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

        #endregion Value units properties

        #region Volume properties

        [Browsable(false)]
        public virtual decimal Volume
        {
            get { return _volume; }
            set
            {
                // Set the new share volume
                if (value != _volume)
                {
                    _volume = value;
                }
            }
        }

        [Browsable(false)]
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

        #endregion Volume properties

        #region Buy properties

        [Browsable(false)]
        public decimal BuyMarketValueTotal
        {
            get { return _buyMarketValueTotal; }
            internal set
            {
                _buyMarketValueTotal = value;
            }
        }

        [Browsable(false)]
        public string BuyMarketValueTotalAsStr
        {
            get
            {
                return Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string BuyMarketValueTotalAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public decimal AverageBuyPrice
        {
            get { return _averageBuyPrice; }
            internal set
            {
                _averageBuyPrice = value;
            }
        }

        [Browsable(false)]
        public string AverageBuyPriceAsStr
        {
            get
            {
                return Helper.FormatDecimal(AverageBuyPrice, Helper.Currencyfivelength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string AverageBuyPriceAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(AverageBuyPrice, Helper.Currencyfourlength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public AllBuysOfTheShare AllBuyEntries
        {
            get { return _allBuyEntries; }
            set { _allBuyEntries = value; }
        }

        #endregion Buy properties

        #region Buy XML properties

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

        #endregion Buy XML properties

        #region Sales properties

        [Browsable(false)]
        public virtual decimal SalePurchaseValueTotal
        {
            get { return _salePurchaseValueTotal; }
            set
            {
                _salePurchaseValueTotal = value;
            }
        }

        [Browsable(false)]
        public string SalePurchaseValueTotalAsStr
        {
            get
            {
                return Helper.FormatDecimal(SalePurchaseValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string SalePurchaseValueTotalAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(SalePurchaseValueTotal, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public AllSalesOfTheShare AllSaleEntries
        {
            get { return _allSaleEntries; }
            set { _allSaleEntries = value; }
        }

        #endregion Sales properties

        #region Sale XML properties

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
        public string SaleBuyPriceAttrName
        {
            get { return _saleBuyPriceAttrName; }
        }

        [Browsable(false)]
        public string SalePriceAttrName
        {
            get { return _saleSalePriceAttrName; }
        }

        [Browsable(false)]
        public string SaleTaxAtSourceAttrName
        {
            get { return _saleTaxAtSourceAttrName; }
        }

        [Browsable(false)]
        public string SaleCapitalGainsTaxAttrName
        {
            get { return _saleCapitalGainsTaxAttrName; }
        }

        [Browsable(false)]
        public string SaleSolidarityTaxAttrName
        {
            get { return _saleSolidarityTaxAttrName; }
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

        #endregion Sale XML properties

        #region Price properties

        [Browsable(false)]
        public virtual decimal CurPrice
        {
            get { return _curPrice; }
            set
            {
                // Set the new price
                if (value != _curPrice)
                {
                    _curPrice = value;
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
        public virtual decimal PrevDayPrice
        {
            get { return _prevDayPrice; }
            set
            {
                // Set new price day before
                if (value != _prevDayPrice)
                {
                    _prevDayPrice = value;
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

        [Browsable(false)]
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

        #endregion Performance properties

        #region Purchase properties

        [Browsable(false)]
        public virtual decimal PurchaseValue
        {
            get { return _purchaseValue; }
            set
            {
                if (value != _purchaseValue)
                {
                    _purchaseValue = value;
#if DEBUG_SHAREOBJECT
                    Console.WriteLine(@"");
                    Console.WriteLine(@"_purchaseValue: {0}", _purchaseValue);
#endif
                }
            }
        }

        [Browsable(false)]
        public string PurchaseValueAsStr
        {
            get
            {
                return Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, false, @"", CultureInfo);
            }
        }

        [Browsable(false)]
        public string PurchaseValueAsStrUnit
        {
            get
            {
                return Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencynonefixlength, true, @"", CultureInfo);
            }
        }

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
            _imagePrevDayPerformance = _imageListPrevDayPerformance[0];

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
        /// <param name="purchaseValue">PurchaseValue of the share</param>
        /// <param name="webSite">Website address of the share</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
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
            decimal price, decimal volume, decimal reduction, decimal costs, decimal purchaseValue,
            string webSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            string document)
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
                BuyObject buyObject = AllBuyEntries.GetBuyObjectByDateTime(strDateTime);
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
                    if (!_allBuyEntries.RemoveBuy(strDateTime))
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

//#if DEBUG_SHAREOBJECT
                Console.WriteLine("Volume: {0}", Volume);
                Console.WriteLine("PurchaseValue: {0}", PurchaseValueAsStr);
                Console.WriteLine("SalePurchaseValueTotal: {0}", SalePurchaseValueTotalAsStr);
//#endif
                return true;
            }
            catch (Exception ex)
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
                SaleObject saleObject = AllSaleEntries.GetSaleObjectByDateTime(strDateTime);
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
                    if (!_allSaleEntries.RemoveSale(strDateTime))
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
        /// This function calculates the profit or loss to the previous day
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
