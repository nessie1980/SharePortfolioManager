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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using SharePortfolioManager.Classes;
using System.Globalization;
using System.Windows.Forms;
using SharePortfolioManager.Properties;
using static System.Decimal;

namespace SharePortfolioManager
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
        private decimal _exchangeRatioDec = 1;

        /// <summary>
        /// Stores the exchange ratio for the foreign currency as string
        /// </summary>
        private string _exchangeRatio = "";

        /// <summary>
        /// Stores the paid dividend value
        /// </summary>
        private decimal _payoutDec;

        /// <summary>
        /// Stores the paid dividend value in the foreign currency
        /// </summary>
        private decimal _payoutFcDec;

        /// <summary>
        /// Stores the paid dividend value with taxes
        /// </summary>
        private decimal _payOutWithTaxesDec;

        /// <summary>
        /// Stores the paid dividend of one piece of the share
        /// </summary>
        private decimal _rateDec = -1;

        /// <summary>
        /// Stores the paid dividend of one piece of the share as string
        /// </summary>
        private string _rate = "";

        /// <summary>
        /// Stores the paid tax at source
        /// </summary>
        private decimal _taxAtSourceDec;

        /// <summary>
        /// Stores the paid tax at source as string
        /// </summary>
        private string _taxAtSource = "";

        /// <summary>
        /// Stores the paid capital gains tax
        /// </summary>
        private decimal _capitalGainsTaxDec;

        /// <summary>
        /// Stores the paid capital gains tax as string
        /// </summary>
        private string _capitalGainsTax = "";

        /// <summary>
        /// Stores the paid solidarity tax
        /// </summary>
        private decimal _solidarityTaxDec;

        /// <summary>
        /// Stores the paid solidarity tax as string
        /// </summary>
        private string _solidarityTax = "";

        /// <summary>
        /// Stores the paid taxes sum
        /// </summary>
        private decimal _taxesSumDec;

        /// <summary>
        /// Stores the paid taxes sum as string
        /// </summary>
        private string _taxesSum;

        /// <summary>
        /// Stores the dividend yield
        /// </summary>
        private decimal _yieldDec;

        /// <summary>
        /// Stores the dividend yield as string
        /// </summary>
        private string _yield = "";

        /// <summary>
        /// Stores the price of the share at the dividend pay day
        /// </summary>
        private decimal _priceDec = -1;

        /// <summary>
        /// Stores the price of the share at the dividend pay day as string
        /// </summary>
        private string _price = "";

        /// <summary>
        /// Stores the volume of the share at the dividend pay day
        /// </summary>
        private decimal _volumeDec;

        /// <summary>
        /// Stores the volume of the share at the dividend pay day as string
        /// </summary>
        private string _volume = "";

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
        public string EnableFcStr => _enableFc.ToString();

        [Browsable(false)]
        public decimal ExchangeRatioDec
        {
            get => _exchangeRatioDec;
            set
            {
                if (_exchangeRatioDec == value)
                    return;
                _exchangeRatioDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string ExchangeRatio
        {
            get => _exchangeRatioDec > 0
                ? Helper.FormatDecimal(_exchangeRatioDec, Helper.Currencyfivelength, true,
                    Helper.Currencytwofixlength)
                : @"";
            set
            {
                if (_exchangeRatio == value)
                    return;
                _exchangeRatio = value;

                // Try to parse
                if (!TryParse(_exchangeRatio, out _exchangeRatioDec))
                    _exchangeRatioDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Guid")]
        public string Guid { get; internal set; }

        [Browsable(false)]
        public string Date { get; set; }

        [DisplayName(@"Date")]
        public string DateStr => DateTime.Parse(Date).ToShortDateString();

        [Browsable(false)]
        public decimal PayoutDec
        {
            get => _payoutDec;
            internal set
            {
                if (_payoutDec == value)
                    return;
                _payoutDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string Payout => _payoutDec > 0 ? Helper.FormatDecimal(_payoutDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";

        [Browsable(false)]
        public decimal PayoutFcDec
        {
            get => _payoutFcDec;
            internal set
            {
                if (_payoutFcDec == value)
                    return;
                _payoutFcDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string PayoutFc => _payoutFcDec > 0 ? Helper.FormatDecimal(_payoutFcDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";

        [Browsable(false)]
        public decimal PayoutWithTaxesDec
        {
            get => _payOutWithTaxesDec;
            internal set
            {
                if (Equals(_payOutWithTaxesDec, value))
                    return;

                _payOutWithTaxesDec = value;
            }
        }

        [Browsable(false)]
        public string PayoutWithTaxes => _payOutWithTaxesDec > 0 ? Helper.FormatDecimal(_payOutWithTaxesDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";

        [Browsable(true)]
        [DisplayName(@"Dividend")]
        public string Rate
        {
            get => _rateDec > 0 ? Helper.FormatDecimal(_rateDec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength) : @"";
            set
            {
                if (_rate == value)
                    return;
                _rate = value;

                // Try to parse
                if (!TryParse(_rate, out _rateDec))
                    _rateDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal RateDec
        {
            get => _rateDec;
            set
            {
                if (_rateDec == value)
                    return;
                _rateDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal VolumeDec
        {
            get => _volumeDec;
            set
            {
                if (_volumeDec == value)
                    return;
                _volumeDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string Volume
        {
            get => _volumeDec >= 0 ? Helper.FormatDecimal(_volumeDec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength) : @"";
            set
            {
                if (_volume == value)
                    return;
                _volume = value;

                // Try to parse
                if (!TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal TaxAtSourceDec
        {
            get => _taxAtSourceDec;
            internal set
            {
                if (_taxAtSourceDec == value)
                    return;
                _taxAtSourceDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string TaxAtSource
        {
            get => Helper.FormatDecimal(_taxAtSourceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", DividendCultureInfo);
            set
            {
                if (_taxAtSource == value)
                    return;
                _taxAtSource = value;

                // Try to parse
                if (!TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal CapitalGainsTaxDec
        {
            get => _capitalGainsTaxDec;
            internal set
            {
                if (_capitalGainsTaxDec == value)
                    return;
                _capitalGainsTaxDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string CapitalGainsTax
        {
            get => _capitalGainsTaxDec.ToString("G");
            set
            {
                if (_capitalGainsTax == value)
                    return;
                _capitalGainsTax = value;

                // Try to parse
                if (!TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal SolidarityTaxDec
        {
            get => _solidarityTaxDec;
            internal set
            {
                if (_solidarityTaxDec == value)
                    return;
                _solidarityTaxDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string SolidarityTax
        {
            get => Helper.FormatDecimal(_solidarityTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
            set
            {
                if (_solidarityTax == value)
                    return;
                _solidarityTax = value;

                // Try to parse
                if (!TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal TaxesSumDec
        {
            get => _taxesSumDec;
            set
            {
                if (_taxesSumDec == value)
                    return;
                _taxesSumDec = value;
                if (_rateDec > -1 && _priceDec > 0)
                {
                    CalculateDividendValues();
                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"TaxSum")]
        public string TaxesSum
        {
            get => _taxesSumDec >= 0 ? Helper.FormatDecimal(_taxesSumDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength) : @"";
            set
            {
                if (_taxesSum == value)
                    return;
                _taxesSum = value;

                // Try to parse
                if (!TryParse(_taxesSum, out _taxesSumDec))
                    _taxesSumDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal YieldDec
        {
            get => _yieldDec;
            internal set
            {
                if (_yieldDec == value)
                    return;
                _yieldDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Yield")]
        public string Yield
        {
            get => Helper.FormatDecimal(_yieldDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", DividendCultureInfo);
            set
            {
                if (_yield == value)
                    return;
                _yield = value;

                // Try to parse
                if (!TryParse(_yield, out _yieldDec))
                    _yieldDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal PriceDec
        {
            get => _priceDec;
            set
            {
                if (_priceDec == value)
                    return;
                _priceDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string Price
        {
            get => Helper.FormatDecimal(_priceDec, Helper.Currencyfivelength, true, Helper.Currencytwofixlength);
            set
            {
                if (_price == value)
                    return;
                _price = value;

                // Try to parse
                if (!TryParse(_price, out _priceDec))
                    _priceDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string Document { get; set; }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public Image DocumentGrid => Helper.GetImageForFile(Document);

        #endregion Properties

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
        /// <param name="decDiviendRate">Dividend of one piece</param>
        /// <param name="decVolume">Volume of all shares</param>
        /// <param name="decTaxAtSource"> Paid tax at source value</param>
        /// <param name="decCapitalGainsTax"> Paid capital gains tax value</param>
        /// <param name="decSolidarityTax"> Paid solidarity tax value</param>
        /// <param name="decPrice">Price of one piece</param>
        /// <param name="strDoc">Document of the dividend</param>
        public DividendObject(CultureInfo cultureInfo, CultureInfo cultureInfoFc, CheckState csEnableFc, decimal decExchangeRatio,
            string strGuid, string strDate, decimal decDiviendRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax,
            decimal decPrice, string strDoc = "")
        {
            DividendCultureInfo = cultureInfo;
            CultureInfoFc = cultureInfoFc;

            Guid = strGuid;
            Date = strDate;
            EnableFc = csEnableFc;
            ExchangeRatioDec = decExchangeRatio;
            RateDec = decDiviendRate;
            VolumeDec = decVolume;
            TaxAtSourceDec = decTaxAtSource;
            CapitalGainsTaxDec = decCapitalGainsTax;
            SolidarityTaxDec = decSolidarityTax;
            PriceDec = decPrice;
            Document = strDoc;

#if DEBUG_DIVIDEND
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
#if DEBUG_DIVIDEND
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateDividendValues");
            Console.WriteLine(@"RateDec: {0}", RateDec);
            Console.WriteLine(@"PriceDec: {0}", PriceDec);
#endif

        if (EnableFc == CheckState.Checked)
            {
                // Calculate the payout
                PayoutFcDec = Math.Round(RateDec * VolumeDec, 2, MidpointRounding.AwayFromZero);

                if (ExchangeRatioDec != 0)
                    PayoutDec = Math.Round(PayoutFcDec / ExchangeRatioDec, 2, MidpointRounding.AwayFromZero);
                else
                    PayoutDec = 0;
            }
            else
            {
                // Calculate the payout
                PayoutDec = Math.Round(RateDec * VolumeDec, 2, MidpointRounding.AwayFromZero);
                PayoutFcDec = 0;
            }

            // Calculate the percent value of dividend paid of a share
            if (RateDec > 0 && PriceDec > 0)
                YieldDec = RateDec / PriceDec * 100;
            else
                YieldDec = 0;

            // Calculate taxes sum
            TaxesSumDec = TaxAtSourceDec + CapitalGainsTaxDec + SolidarityTaxDec;

            // Calculate payout with taxes
            if ( PayoutDec > 0)
                PayoutWithTaxesDec = PayoutDec - TaxesSumDec;
            else
                PayoutWithTaxesDec = 0;

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
