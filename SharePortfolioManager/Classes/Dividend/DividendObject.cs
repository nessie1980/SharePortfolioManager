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
using SharePortfolioManager.Classes;
using System.Globalization;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public class DividendObject
    {
        #region Variables

        /// <summary>
        /// Stores the flag if the view must been updated
        /// </summary>
        private bool _updateView;

        /// <summary>
        /// Stores the culture info of the payout currency
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the culture info of the payout foreign currency
        /// </summary>
        private CultureInfo _cultureInfoFC;

        /// <summary>
        /// Stores if the payout is done in a foreign currency
        /// </summary>
        private CheckState _enableFC;

        /// <summary>
        /// Stores if the payout is done in a foreign currency as string
        /// </summary>
        private string _enableFCStr;

        /// <summary>
        /// Stores the exchange ratio for the foreign currency
        /// </summary>
        private decimal _exchangeRatioDec = 1;

        /// <summary>
        /// Stores the exchange ratio for the foreign currency as string
        /// </summary>
        private string _exchangeRatio = "";

        /// <summary>
        /// Stores the date string of the pay date
        /// </summary>
        private string _dateTime;

        /// <summary>
        /// Stores the paid dividend value
        /// </summary>
        private decimal _payoutDec = 0;

        /// <summary>
        /// Stores the paid dividend value as string
        /// </summary>
        private string _payout = "";

        /// <summary>
        /// Stores the paid dividend value in the foreign currency
        /// </summary>
        private decimal _payoutFCDec = 0;

        /// <summary>
        /// Stores the paid dividend value in the foreign currency as string
        /// </summary>
        private string _payoutFC = "";

        /// <summary>
        /// Stores the paid dividend value with taxes
        /// </summary>
        private decimal _payOutWithTaxesDec = 0;

        /// <summary>
        /// Stores the paid dividend value with taxes as string
        /// </summary>
        private string _payOutWithTaxes = "";

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
        private decimal _taxAtSourceDec = 0;

        /// <summary>
        /// Stores the paid tax at source as string
        /// </summary>
        private string _taxAtSource = "";

        /// <summary>
        /// Stores the paid capital gains tax
        /// </summary>
        private decimal _capitalGainsTaxDec = 0;

        /// <summary>
        /// Stores the paid capital gains tax as string
        /// </summary>
        private string _capitalGainsTax = "";

        /// <summary>
        /// Stores the paid solidarity tax
        /// </summary>
        private decimal _solidarityTaxDec = 0;

        /// <summary>
        /// Stores the paid solidarity tax as string
        /// </summary>
        private string _solidarityTax = "";

        /// <summary>
        /// Stores the paid taxes sum
        /// </summary>
        private decimal _taxesSumDec = 0;

        /// <summary>
        /// Stores the paid taxes sum as string
        /// </summary>
        private string _taxesSum = "";

        /// <summary>
        /// Stores the dividend yield
        /// </summary>
        private decimal _yieldDec = 0;

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
        private decimal _volumeDec = 0;

        /// <summary>
        /// Stores the volume of the share at the dividend pay day as string
        /// </summary>
        private string _volume = "";

        /// <summary>
        /// Stores the document of the dividend
        /// </summary>
        private string _dividendDocument;

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public bool UpdateView
        {
            get { return _updateView; }
            set { _updateView = value; }
        }

        [Browsable(false)]
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { _cultureInfo = value; }
        }

        [Browsable(false)]
        public CultureInfo CultureInfoFC
        {
            get { return _cultureInfoFC; }
            set { _cultureInfoFC = value; }
        }

        [Browsable(false)]
        public CheckState EnableFC
        {
            get { return _enableFC; }
            set
            {
                if (_enableFC == value)
                    return;
                _enableFC = value;

                // Calculate the dividend values
                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string EnableFCStr
        {
            get { return _enableFC.ToString(); }
        }

        [Browsable(false)]
        public decimal ExchangeRatioDec
        {
            get { return _exchangeRatioDec; }
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
            get { return Helper.FormatDecimal(_exchangeRatioDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _exchangeRatioDec.ToString(); }
            set
            {
                if (_exchangeRatio == value)
                    return;
                _exchangeRatio = value;

                // Try to parse
                if (!Decimal.TryParse(_exchangeRatio, out _exchangeRatioDec))
                    _exchangeRatioDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string ExchangeRatioWithUnit
        {
            get { return Helper.FormatDecimal(_exchangeRatioDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }

        [DisplayName(@"Date")]
        public string DateTimeStr
        {
            get { return _dateTime; }
        }

        [Browsable(false)]
        public decimal RateDec
        {
            get { return _rateDec; }
            set
            {
                if (_rateDec == value)
                    return;
                _rateDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public decimal PayoutDec
        {
            get { return _payoutDec; }
            internal set
            {
                if (_payoutDec == value)
                    return;
                _payoutDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string Payout
        {
            get { return Helper.FormatDecimal(_payoutDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _payoutDec.ToString(); }
            set
            {
                if (_payout == value)
                    return;
                _payout = value;

                // Try to parse
                if (!Decimal.TryParse(_payout, out _payoutDec))
                    _payoutDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string PayoutWithUnit
        {
            get { return Helper.FormatDecimal(_payoutDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal PayoutFCDec
        {
            get { return _payoutFCDec; }
            internal set
            {
                if (_payoutFCDec == value)
                    return;
                _payoutFCDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string PayoutFC
        {
            //get { return Helper.FormatDecimal(_payoutFCDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            get { return _payoutFCDec.ToString(); }
            set
            {
                if (_payoutFC == value)
                    return;
                _payoutFC = value;

                // Try to parse
                if (!Decimal.TryParse(_payoutFC, out _payoutFCDec))
                    _payoutFCDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string PayoutFCWithUnit
        {
            get { return Helper.FormatDecimal(_payoutFCDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal PayoutWithTaxesDec
        {
            get { return _payOutWithTaxesDec; }
            internal set
            {
                if (_payOutWithTaxesDec == value)
                    return;
                _payOutWithTaxesDec = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"PayoutWithTaxes")]
        public string PayoutWithTaxes
        {
            get { return Helper.FormatDecimal(_payOutWithTaxesDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _payOutWithTaxesDec.ToString(); }
            internal set
            {
                if (_payOutWithTaxes == value)
                    return;
                _payOutWithTaxes = value;

                // Try to parse
                if (!Decimal.TryParse(_payOutWithTaxes, out _payOutWithTaxesDec))
                    _payOutWithTaxesDec = 0;
            }
        }

        [Browsable(false)]
        public string PayoutWithTaxesWithUnit
        {
            get { return Helper.FormatDecimal(_payOutWithTaxesDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(true)]
        [DisplayName(@"Dividend")]
        public string Rate
        {
            get { return Helper.FormatDecimal(_rateDec, Helper.Currencysixlength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _rateDec.ToString(); }
            set
            {
                if (_rate == value)
                    return;
                _rate = value;

                // Try to parse
                if (!Decimal.TryParse(_rate, out _rateDec))
                    _rateDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string RateWithUnit
        {
            get { return Helper.FormatDecimal(_rateDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal TaxAtSourceDec
        {
            get { return _taxAtSourceDec; }
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
            get { return Helper.FormatDecimal(_taxAtSourceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _taxAtSourceDec.ToString(); }
            set
            {
                if (_taxAtSource == value)
                    return;
                _taxAtSource = value;

                // Try to parse
                if (!Decimal.TryParse(_taxAtSource, out _taxAtSourceDec))
                    _taxAtSourceDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string TaxAtSourceWithUnit
        {
            get { return Helper.FormatDecimal(_taxAtSourceDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal CapitalGainsTaxDec
        {
            get { return _capitalGainsTaxDec; }
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
            //get { return Helper.FormatDecimal(_capitalGainsTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            get { return _capitalGainsTaxDec.ToString(); }
            set
            {
                if (_capitalGainsTax == value)
                    return;
                _capitalGainsTax = value;

                // Try to parse
                if (!Decimal.TryParse(_capitalGainsTax, out _capitalGainsTaxDec))
                    _capitalGainsTaxDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string CapitalGainsTaxWithUnit
        {
            get { return Helper.FormatDecimal(_capitalGainsTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal SolidarityTaxDec
        {
            get { return _solidarityTaxDec; }
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
            get { return Helper.FormatDecimal(_solidarityTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _solidarityTaxDec.ToString(); }
            set
            {
                if (_solidarityTax == value)
                    return;
                _solidarityTax = value;

                // Try to parse
                if (!Decimal.TryParse(_solidarityTax, out _solidarityTaxDec))
                    _solidarityTaxDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string SolidarityTaxWithUnit
        {
            get { return Helper.FormatDecimal(_solidarityTaxDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal TaxesSumDec
        {
            get { return _taxesSumDec; }
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

        [Browsable(false)]
        public string TaxesSum
        {
            get { return Helper.FormatDecimal(_taxesSumDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _taxesSumDec.ToString(); }
            set
            {
                if (_taxesSum == value)
                    return;
                _taxesSum = value;

                // Try to parse
                if (!Decimal.TryParse(_taxesSum, out _taxesSumDec))
                    _taxesSumDec = 0;
            }
        }

        [Browsable(false)]
        public string TaxesSumWithUnit
        {
            get { return Helper.FormatDecimal(_taxesSumDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal YieldDec
        {
            get { return _yieldDec; }
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
            get { return Helper.FormatDecimal(_yieldDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return YieldDec.ToString(); }
            set
            {
                if (_yield == value)
                    return;
                _yield = value;

                // Try to parse
                if (!Decimal.TryParse(_yield, out _yieldDec))
                    _yieldDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string YieldWithUnit
        {
            get { return Helper.FormatDecimal(_yieldDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, CultureInfo); }
        }

        [Browsable(false)]
        public decimal PriceDec
        {
            get { return _priceDec; }
            set
            {
                if (_priceDec == value)
                    return;
                _priceDec = value;

                CalculateDividendValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Price")]
        public string Price
        {
            get { return Helper.FormatDecimal(_priceDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _priceDec.ToString(); }
            set
            {
                if (_price == value)
                    return;
                _price = value;

                // Try to parse
                if (!Decimal.TryParse(_price, out _priceDec))
                    _priceDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string PriceWithUnit
        {
            get { return Helper.FormatDecimal(_priceDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal VolumeDec
        {
            get { return _volumeDec; }
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
            get { return Helper.FormatDecimal(_volumeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
            //get { return _volumeDec.ToString(); }
            set
            {
                if (_volume == value)
                    return;
                _volume = value;

                // Try to parse
                if (!Decimal.TryParse(_volume, out _volumeDec))
                    _volumeDec = 0;

                CalculateDividendValues();
            }
        }

        [Browsable(false)]
        public string VolumeWithUnit
        {
            get { return Helper.FormatDecimal(_volumeDec, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, CultureInfo); }
        }

        [Browsable(false)]
        public string Document
        {
            get { return _dividendDocument; }
            set { _dividendDocument = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string DocumentFileName
        {
            get { return Helper.GetFileName(_dividendDocument); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        //public DividendObject()
        //{ }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="csEnableFC">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency calculation</param>
        /// <param name="strDate">Date of the dividend pay</param>
        /// <param name="decDiviendRate">Dividend of one piece</param>
        /// <param name="decVolume">Volume of all shares</param>
        /// <param name="decTaxAtSource"> Paid tax at source value</param>
        /// <param name="decCapitalGainsTax"> Paid capital gains tax value</param>
        /// <param name="decSolidarityTax"> Paid solidarity tax value</param>
        /// <param name="decPrice">Price of one piece</param>
        /// <param name="strDoc">Document of the dividend</param>
        public DividendObject(CultureInfo cultureInfo, CultureInfo cultureInfoFC, CheckState csEnableFC, decimal decExchangeRatio,
            string strDate, decimal decDiviendRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax,
            decimal decPrice, string strDoc = "")
        {
            CultureInfo = cultureInfo;
            CultureInfoFC = cultureInfoFC;

            DateTime = strDate;
            EnableFC = csEnableFC;
            ExchangeRatioDec = decExchangeRatio;
            RateDec = decDiviendRate;
            VolumeDec = decVolume;
            TaxAtSourceDec = decTaxAtSource;
            CapitalGainsTaxDec = decCapitalGainsTax;
            SolidarityTaxDec = decSolidarityTax;
            PriceDec = decPrice;
            Document = strDoc;

#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"New DividendObject created");
            Console.WriteLine(@"CultureInfo: {0}", CultureInfo.Name);
            Console.WriteLine(@"EnableFC: {0}", EnableFC);
            if(EnableFC == CheckState.Checked)
                Console.WriteLine(@"CultureInfoFC: {0}", CultureInfoFC.Name);
            Console.WriteLine(@"ExchangeRatio: {0}", ExchangeRatioDec);
            Console.WriteLine(@"Date: {0}", DateTime);
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
            if (EnableFC == CheckState.Checked)
            {
                // Calculate the payout
                PayoutFCDec = Math.Round(RateDec * VolumeDec, 2, MidpointRounding.AwayFromZero);

                if (ExchangeRatioDec != 0)
                    PayoutDec = Math.Round(PayoutFCDec / ExchangeRatioDec, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                // Calculate the payout
                PayoutDec = Math.Round(RateDec * VolumeDec, 2, MidpointRounding.AwayFromZero);
                PayoutFCDec = 0;
            }

            // Calculate the percent value of dividend paid of a share
            if (RateDec > 0 && PriceDec > 0)
                YieldDec = RateDec / PriceDec * 100;
            else
                YieldDec = 0;

            // Calculate taxes sum
            TaxesSumDec = TaxAtSourceDec + CapitalGainsTaxDec + SolidarityTaxDec;

            // Calculate payout with taxes
            PayoutWithTaxesDec = PayoutDec - TaxesSumDec;

            // Set flag for the view update
            UpdateView = true;
        }

        #endregion Methods
    }

    /// <summary>
    /// This is the comparer class for the DividendObject.
    /// It is used for the sort for the dividend lists.
    /// </summary>
    public class DividendObjectComparer : IComparer<DividendObject>
    {
        #region Methods

        public int Compare(DividendObject dividendObject1, DividendObject dividendObject2)
        {
            return DateTime.Compare(Convert.ToDateTime(dividendObject1.DateTime), Convert.ToDateTime(dividendObject2.DateTime));
        }

        #endregion Methods
    }

}
