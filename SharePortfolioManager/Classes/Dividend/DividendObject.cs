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
//#define DEBUG_DIVIDEND_OBJECT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using SharePortfolioManager.Classes.ShareObjects;
using static System.Decimal;

namespace SharePortfolioManager.Classes.Dividend
{
    [Serializable]
    public class DividendObject
    {
        #region Variables

        /// <summary>
        /// Stores if the payout is done in a foreign currency
        /// </summary>
        private CheckState _enableFc;

        /// <summary>
        /// Stores the exchange ratio for the foreign currency
        /// </summary>
        private decimal _exchangeRatio = 1;

        /// <summary>
        /// Stores the exchange ratio for the foreign currency as string
        /// </summary>
        private string _exchangeRatioAsStr = "";

        /// <summary>
        /// Stores the volume of the share at the dividend pay day
        /// </summary>
        private decimal _volume;

        /// <summary>
        /// Stores the volume of the share at the dividend pay day as string
        /// </summary>
        private string _volumeAsStr = "";

        /// <summary>
        /// Stores the paid dividend of one piece of the share
        /// </summary>
        private decimal _dividend= -1;

        /// <summary>
        /// Stores the paid dividend of one piece of the share as string
        /// </summary>
        private string _dividendAsStr = "";

        /// <summary>
        /// Stores the paid tax at source
        /// </summary>
        private decimal _taxAtSource;

        /// <summary>
        /// Stores the paid tax at source as string
        /// </summary>
        private string _taxAtSourceAsStr = "";

        /// <summary>
        /// Stores the paid capital gains tax
        /// </summary>
        private decimal _capitalGainsTax;

        /// <summary>
        /// Stores the paid capital gains tax as string
        /// </summary>
        private string _capitalGainsTaxAsStr = "";

        /// <summary>
        /// Stores the paid solidarity tax
        /// </summary>
        private decimal _solidarityTax;

        /// <summary>
        /// Stores the paid solidarity tax as string
        /// </summary>
        private string _solidarityTaxAsStr = "";

        /// <summary>
        /// Stores the paid taxes sum
        /// </summary>
        private decimal _taxSum;

        /// <summary>
        /// Stores the paid taxes sum as string
        /// </summary>
        private string _taxSumAsStr;

        /// <summary>
        /// Stores the paid dividend value
        /// </summary>
        private decimal _dividendPayout;

        /// <summary>
        /// Stores the paid dividend value in the foreign currency
        /// </summary>
        private decimal _dividendPayoutFc;

        /// <summary>
        /// Stores the paid dividend value with taxes
        /// </summary>
        private decimal _dividendPayoutWithTaxes;

        /// <summary>
        /// Stores the dividend yield
        /// </summary>
        private decimal _yield;

        /// <summary>
        /// Stores the dividend yield as string
        /// </summary>
        private string _yieldAsStr = "";

        /// <summary>
        /// Stores the price of the share at the dividend pay day
        /// </summary>
        private decimal _priceAtPayday = -1;

        /// <summary>
        /// Stores the price of the share at the dividend pay day as string
        /// </summary>
        private string _priceAtPaydayAsStr = "";

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public bool UpdateView { get; set; }

        [Browsable(false)]
        public CultureInfo DividendCultureInfo { get; set; }

        [Browsable(false)]
        public CultureInfo CultureInfoFc { get; set; }

        [Browsable(false)]
        public CheckState EnableFc
        {
            get => _enableFc;
            set
            {
                if (_enableFc == value)
                    return;
                _enableFc = value;

                // Calculate the dividend values
                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string EnableFcAsStr => _enableFc.ToString();

        [Browsable(false)]
        public decimal ExchangeRatio
        {
            get => _exchangeRatio;
            set
            {
                if (_exchangeRatio == value)
                    return;
                _exchangeRatio = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string ExchangeRatioAsStr
        {
            get => _exchangeRatio > 0
                ? Helper.FormatDecimal(_exchangeRatio, Helper.CurrencyFiveLength, true,
                    Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_exchangeRatioAsStr == value)
                    return;
                _exchangeRatioAsStr = value;

                // Try to parse
                if (!TryParse(_exchangeRatioAsStr, out _exchangeRatio))
                    _exchangeRatio = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string Guid { get; internal set; }

        [Browsable(false)]
        public string Date { get; set; }

        [Browsable(false)]
        public string DateAsStr => DateTime.Parse(Date).ToShortDateString();

        [Browsable(false)]
        public decimal Dividend
        {
            get => _dividend;
            set
            {
                if (_dividend == value)
                    return;
                _dividend = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string DividendAsStr
        {
            get => _dividend > 0
                ? Helper.FormatDecimal(_dividend, Helper.CurrencyFiveLength, true, Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_dividendAsStr == value)
                    return;
                _dividendAsStr = value;

                // Try to parse
                if (!TryParse(_dividendAsStr, out _dividend))
                    _dividend = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal Volume
        {
            get => _volume;
            set
            {
                if (_volume == value)
                    return;
                _volume = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string VolumeAsStr
        {
            get => _volume > 0
                ? Helper.FormatDecimal(_volume, Helper.CurrencyFiveLength, true, Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_volumeAsStr == value)
                    return;
                _volumeAsStr = value;

                // Try to parse
                if (!TryParse(_volumeAsStr, out _volume))
                    _volume = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal TaxAtSource
        {
            get => _taxAtSource;
            internal set
            {
                if (_taxAtSource == value)
                    return;
                _taxAtSource = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string TaxAtSourceAsStr
        {
            get => _taxAtSource > 0
                ? Helper.FormatDecimal(_taxAtSource, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_taxAtSourceAsStr == value)
                    return;
                _taxAtSourceAsStr = value;

                // Try to parse
                if (!TryParse(_taxAtSourceAsStr, out _taxAtSource))
                    _taxAtSource = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal CapitalGainsTax
        {
            get => _capitalGainsTax;
            internal set
            {
                if (_capitalGainsTax == value)
                    return;
                _capitalGainsTax = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string CapitalGainsTaxAsStr
        {
            get => _capitalGainsTax > 0
                ? Helper.FormatDecimal(_capitalGainsTax, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_capitalGainsTaxAsStr == value)
                    return;
                _capitalGainsTaxAsStr = value;

                // Try to parse
                if (!TryParse(_capitalGainsTaxAsStr, out _capitalGainsTax))
                    _capitalGainsTax = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal SolidarityTax
        {
            get => _solidarityTax;
            internal set
            {
                if (_solidarityTax == value)
                    return;
                _solidarityTax = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string SolidarityTaxAsStr
        {
            get => _solidarityTax > 0
                ? Helper.FormatDecimal(_solidarityTax, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_solidarityTaxAsStr == value)
                    return;
                _solidarityTaxAsStr = value;

                // Try to parse
                if (!TryParse(_solidarityTaxAsStr, out _solidarityTax))
                    _solidarityTax = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal TaxesSum
        {
            get => _taxSum;
            set
            {
                if (_taxSum == value)
                    return;
                _taxSum = value;
                if (_dividend > -1 && _priceAtPayday > 0)
                {
                    CalculateDividendValues();
                }
            }
        }

        [Browsable(false)]
        public string TaxesSumAsStr
        {
            get => _taxSum > 0
                ? Helper.FormatDecimal(_taxSum, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_taxSumAsStr == value)
                    return;
                _taxSumAsStr = value;

                // Try to parse
                if (!TryParse(_taxSumAsStr, out _taxSum))
                    _taxSum = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal DividendPayout
        {
            get => _dividendPayout;
            internal set
            {
                if (_dividendPayout == value)
                    return;
                _dividendPayout = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string DividendPayoutAsStr => _dividendPayout > 0
            ? Helper.FormatDecimal(_dividendPayout, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal DividendPayoutFc
        {
            get => _dividendPayoutFc;
            internal set
            {
                if (_dividendPayoutFc == value)
                    return;
                _dividendPayoutFc = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string DividendPayoutFcAsStr => _dividendPayoutFc > 0
            ? Helper.FormatDecimal(_dividendPayoutFc, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal DividendPayoutWithTaxes
        {
            get => _dividendPayoutWithTaxes;
            internal set
            {
                if (Equals(_dividendPayoutWithTaxes, value))
                    return;

                _dividendPayoutWithTaxes = value;
            }
        }

        [Browsable(false)]
        public string DividendPayoutWithTaxesAsStr => _dividendPayoutWithTaxes > 0
            ? Helper.FormatDecimal(_dividendPayoutWithTaxes, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal Yield
        {
            get => _yield;
            internal set
            {
                if (_yield == value)
                    return;
                _yield = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string YieldAsStr
        {
            get => _yield > 0
                    ? Helper.FormatDecimal(_yield, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength)
                    : @"0,0";
            set
            {
                if (_yieldAsStr == value)
                    return;
                _yieldAsStr = value;

                // Try to parse
                if (!TryParse(_yieldAsStr, out _yield))
                    _yield = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal PriceAtPayday
        {
            get => _priceAtPayday;
            set
            {
                if (_priceAtPayday == value)
                    return;
                _priceAtPayday = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string PriceAtPaydayAsStr
        {
            get => _priceAtPayday > 0
                ? Helper.FormatDecimal(_priceAtPayday, Helper.CurrencyFiveLength, true, Helper.CurrencyTwoFixLength)
                : @"0,0";
            set
            {
                if (_priceAtPaydayAsStr == value)
                    return;
                _priceAtPaydayAsStr = value;

                // Try to parse
                if (!TryParse(_priceAtPaydayAsStr, out _priceAtPayday))
                    _priceAtPayday = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string DocumentAsStr { get; set; }

        [Browsable(false)]
        public Image DocumentGridImage => Helper.GetImageForFile(DocumentAsStr);

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Guid")]
        // ReSharper disable once UnusedMember.Global
        public string DgvGuid => Guid;

        [Browsable(true)]
        [DisplayName(@"Date")]
        // ReSharper disable once UnusedMember.Global
        public string DgvDate => DateAsStr;

        [Browsable(true)]
        [DisplayName(@"Dividend")]
        // ReSharper disable once UnusedMember.Global
        public string DgvRate
        {
            get
            {
                if (EnableFc == CheckState.Checked && ExchangeRatio != 0)
                {
                    return _dividend / ExchangeRatio > 0
                        ? Helper.FormatDecimal(_dividend / ExchangeRatio, Helper.CurrencyFiveLength, true, Helper.CurrencyTwoFixLength, true,
                            @"",
                            DividendCultureInfo)
                        : @"-";

                }
                else
                {
                    return _dividend > 0
                        ? Helper.FormatDecimal(_dividend, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true,
                            @"",
                            DividendCultureInfo)
                        : @"-";

                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        // ReSharper disable once UnusedMember.Global
        public string DgvVolume => _volume >= 0
            ? Helper.FormatDecimal(_volume, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true,
                ShareObject.PieceUnit, DividendCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"TaxSum")]
        // ReSharper disable once UnusedMember.Global
        public string DgvTaxSum => _taxSum >= 0
            ? Helper.FormatDecimal(_taxSum, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true,
                @"", DividendCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"PayoutWithTaxes")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPayoutWithTaxes => _dividendPayoutWithTaxes > 0
            ? Helper.FormatDecimal(_dividendPayoutWithTaxes, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength,
                true, @"", DividendCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"Yield")]
        // ReSharper disable once UnusedMember.Global
        public string DgvYield => _yield > 0
            ? Helper.FormatDecimal(_yield, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength,
                true,
                @"%", DividendCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"Document")]
        // ReSharper disable once UnusedMember.Global
        public Image DgvDocumentGrid => DocumentGridImage;

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="cultureInfoFc">Foreign culture info of the share</param>
        /// <param name="csEnableFc">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency calculation</param>
        /// <param name="strGuid">Guid of the share buy</param>
        /// <param name="strDate">Date of the dividend pay</param>
        /// <param name="decDividendRate">Dividend of one piece</param>
        /// <param name="decVolume">Volume of all shares</param>
        /// <param name="decTaxAtSource"> Paid tax at source value</param>
        /// <param name="decCapitalGainsTax"> Paid capital gains tax value</param>
        /// <param name="decSolidarityTax"> Paid solidarity tax value</param>
        /// <param name="decPrice">Price of one piece</param>
        /// <param name="strDoc">Document of the dividend</param>
        public DividendObject(CultureInfo cultureInfo, CultureInfo cultureInfoFc, CheckState csEnableFc, decimal decExchangeRatio,
            string strGuid, string strDate, decimal decDividendRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax,
            decimal decPrice, string strDoc = "")
        {
            DividendCultureInfo = cultureInfo;
            CultureInfoFc = cultureInfoFc;

            Guid = strGuid;
            Date = strDate;
            EnableFc = csEnableFc;
            ExchangeRatio = decExchangeRatio;
            Dividend = decDividendRate;
            Volume = decVolume;
            TaxAtSource = decTaxAtSource;
            CapitalGainsTax = decCapitalGainsTax;
            SolidarityTax = decSolidarityTax;
            PriceAtPayday = decPrice;
            DocumentAsStr = strDoc;

#if DEBUG_DIVIDEND_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"New DividendObject created");
            Console.WriteLine(@"CultureInfo: {0}", DividendCultureInfo.Name);
            Console.WriteLine(@"EnableFC: {0}", EnableFc);
            if(EnableFc == CheckState.Checked)
                Console.WriteLine(@"CultureInfoFC: {0}", CultureInfoFc.Name);
            Console.WriteLine(@"ExchangeRatio: {0}", ExchangeRatioDec);
            Console.WriteLine(@"Guid: {0}", Guid);
            Console.WriteLine(@"Date: {0}", Date);
            Console.WriteLine(@"DividendOfAShare: {0}", RateDec);
            Console.WriteLine(@"Volume: {0}", VolumeDec);
            Console.WriteLine(@"TaxAtSource: {0}", TaxAtSourceDec);
            Console.WriteLine(@"CapitalGainsTax: {0}", CapitalGainsTaxDec);
            Console.WriteLine(@"SolidarityTax: {0}", SolidarityTaxDec);
            Console.WriteLine(@"SharePrice: {0}", PriceDec);
            Console.WriteLine(@"DividendDocument: {0}", Document);
            Console.WriteLine(@"");
#endif
        }

        /// <summary>
        /// This function calculates the dividend percent value
        /// and the dividend value for the complete volume of shares
        /// The values are stored in the member variables
        /// </summary>
        private void CalculateDividendValues()
        {
#if DEBUG_DIVIDEND_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateDividendValues");
            Console.WriteLine(@"RateDec: {0}", RateDec);
            Console.WriteLine(@"PriceDec: {0}", PriceDec);
#endif

            if (EnableFc == CheckState.Checked)
            {
                // Calculate the payout
                DividendPayoutFc = Math.Round(Dividend * Volume, 2, MidpointRounding.AwayFromZero);

                DividendPayout = ExchangeRatio != 0
                    ? Math.Round(DividendPayoutFc / ExchangeRatio, 2, MidpointRounding.AwayFromZero)
                    : 0;
            }
            else
            {
                // Calculate the payout
                DividendPayout = Math.Round(Dividend * Volume, 2, MidpointRounding.AwayFromZero);
                DividendPayoutFc = 0;
            }

            // Calculate the percent value of dividend paid of a share
            if (Dividend > 0 && PriceAtPayday > 0)
                Yield = Dividend / PriceAtPayday * 100;
            else
                Yield = 0;

            // Calculate taxes sum
            TaxesSum = TaxAtSource + CapitalGainsTax + SolidarityTax;

            // Calculate payout with taxes
            if (DividendPayout > 0)
                DividendPayoutWithTaxes = DividendPayout - TaxesSum;
            else
                DividendPayoutWithTaxes = 0;

            // Set flag for the view update
            UpdateView = true;
        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This is the comparer class for the DividendObject.
    /// It is used for the sort for the dividend lists.
    /// </summary>
    public class DividendObjectComparer : IComparer<DividendObject>
    {
#region Methods

        public int Compare(DividendObject dividendObject1, DividendObject dividendObject2)
        {
            if (dividendObject1 == null) return 0;
            if (dividendObject2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(dividendObject1.Date), Convert.ToDateTime(dividendObject2.Date));
        }

#endregion Methods
    }

}
