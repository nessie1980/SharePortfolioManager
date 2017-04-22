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
using SharePortfolioManager.Classes.Taxes;
using System.Globalization;

namespace SharePortfolioManager
{
    public class DividendObject
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the date string of the pay date
        /// </summary>
        private string _dividendDate;

        /// <summary>
        /// Stores the paid dividend of one piece of the share
        /// </summary>
        private decimal _dividendRate = -1;

        /// <summary>
        /// Stores the loss balance of the share
        /// </summary>
        private decimal _lossBalance = 0;

        /// <summary>
        /// Stores the price of the share at the dividend pay day
        /// </summary>
        private decimal _sharePrice = -1;

        /// <summary>
        /// Stores the volume of the share at the dividend pay day
        /// </summary>
        private decimal _shareVolume = 0;

        /// <summary>
        /// Stores the dividend yield
        /// </summary>
        private decimal _dividendYield = 0;

        /// <summary>
        /// Stores the taxes of the dividend
        /// </summary>
        private Taxes _dividendTaxes;

        /// <summary>
        /// Stores the paid dividend value
        /// </summary>
        private decimal _dividendPayOut = 0;

        /// <summary>
        /// Stores the paid dividend value with taxes
        /// </summary>
        private decimal _dividendPayOutWithTaxes = 0;

        /// <summary>
        /// Stores the document of the dividend
        /// </summary>
        private string _dividendDocument;

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { _cultureInfo = value; }
        }

        [Browsable(false)]
        public string DividendDate
        {
            get { return _dividendDate; }
            set { _dividendDate = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string DividendDateAsString
        {
            get { return _dividendDate; }
        }

        [Browsable(false)]
        public Taxes DividendTaxes
        {
            get { return _dividendTaxes; }
            internal set { _dividendTaxes = value; }
        }

        [Browsable(false)]
        public decimal DividendPayOut
        {
            get { return _dividendPayOut; }
            internal set { _dividendPayOut = value; }
        }

        [Browsable(false)]
        public string DividendPayOutAsString
        {
            get { return Helper.FormatDecimal(_dividendPayOut, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string DividendPayOutWithUnitAsString
        {
            get { return Helper.FormatDecimal(_dividendPayOut, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal DividendPayOutWithTaxes
        {
            get { return _dividendPayOutWithTaxes; }
            internal set { _dividendPayOutWithTaxes = value; }
        }

        [Browsable(true)]
        [DisplayName(@"PayoutWithTaxes")]
        public string DividendPayOutWithTaxesAsString
        {
            get { return Helper.FormatDecimal(_dividendPayOutWithTaxes, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string DividendPayOutWithTaxesWithUnitAsString
        {
            get { return Helper.FormatDecimal(_dividendPayOutWithTaxes, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal DividendRate
        {
            get { return _dividendRate; }
            set
            {
                _dividendRate = value;
                if (_dividendRate > -1 && _sharePrice > 0)
                {
                    // Calculate the dividend values "_dividendOfAShare" and "_dividendPercentValue"
                    CalculateDividendValues();
                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"Dividend")]
        public string DividendRateAsString
        {
            get { return Helper.FormatDecimal(_dividendRate, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string DividendOfAShareWithUnitAsString
        {
            get { return Helper.FormatDecimal(_dividendRate, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal LossBalance
        {
            get { return _lossBalance; }
            set
            {
                _lossBalance = value;
                if (_dividendRate > -1 && _sharePrice > 0 && _lossBalance > 0)
                {
                    // Calculate the dividend values "_dividendOfAShare" and "_dividendPercentValue"
                    CalculateDividendValues();
                }
            }
        }

        [Browsable(false)]
        public string LossBalanceAsString
        {
            get { return Helper.FormatDecimal(_lossBalance, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string LossBalanceWithUnitAsString
        {
            get { return Helper.FormatDecimal(_lossBalance, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal DividendYield
        {
            get { return _dividendYield; }
            internal set { _dividendYield = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Yield")]
        public string DividendYieldAsString
        {
            get { return Helper.FormatDecimal(_dividendYield, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string DividendYieldWithUnitAsString
        {
            get { return Helper.FormatDecimal(_dividendYield, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, CultureInfo); }
        }

        [Browsable(false)]
        public decimal SharePrice
        {
            get { return _sharePrice; }
            set
            {
                _sharePrice = value;
                if (_dividendRate > -1 && _sharePrice > 0)
                {
                    // Calculate the dividend values "_dividendOfAShare" and "_dividendPercentValue"
                    CalculateDividendValues();
                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"Price")]
        public string SharePriceAsString
        {
            get { return Helper.FormatDecimal(_sharePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string SharePriceWithUnitAsString
        {
            get { return Helper.FormatDecimal(_sharePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal ShareVolume
        {
            get { return _shareVolume; }
            set
            {
                _shareVolume = value;
                if (_dividendRate > -1 && _sharePrice > 0)
                {
                    // Calculate the dividend values "_dividendOfAShare" and "_dividendPercentValue"
                    CalculateDividendValues();
                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string ShareVolumeAsString
        {
            get { return Helper.FormatDecimal(_shareVolume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string ShareVolumeWithUnitAsString
        {
            get { return Helper.FormatDecimal(_shareVolume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, CultureInfo); }
        }

        [Browsable(false)]
        public string DividendDocument
        {
            get { return _dividendDocument; }
            set { _dividendDocument = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string DividendDocumentFileName
        {
            get { return Helper.GetFileName(_dividendDocument); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strDate">Date of the dividend pay</param>
        /// <param name="taxes">Taxes which must be paid</param>
        /// <param name="decDiviendRate">Dividend of one piece</param>
        /// <param name="decLossBalance">Loss balance of the share</param>
        /// <param name="decPrice">Price of one piece</param>
        /// <param name="decVolume">Volume of all shares</param>
        /// <param name="strDoc">Document of the dividend</param>
        public DividendObject(CultureInfo cultureInfo, string strDate, Taxes taxes, decimal decDiviendRate, decimal decLossBalance, decimal decPrice, decimal decVolume, string strDoc = "")
        {
            DividendTaxes = new Taxes();

            DividendTaxes.DeepCopy(taxes);
            // Set loss balance to the taxes object
            DividendTaxes.LossBalance = decLossBalance;

            CultureInfo = cultureInfo;
            DividendDate = strDate;
            LossBalance = decLossBalance;
            DividendRate = decDiviendRate;
            SharePrice = decPrice;
            ShareVolume = decVolume;
            DividendDocument = strDoc;

#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"New DividendObject created");
            Console.WriteLine(@"Date: {0}", DividendDate);
            Console.WriteLine(@"DividendOfAShare: {0}", DividendRate);
            Console.WriteLine(@"LossBalance: {0}", DividendTaxes.LossBalance);
            Console.WriteLine(@"SharePrice: {0}", SharePrice);
            Console.WriteLine(@"ShareVolume: {0}", ShareVolume);
            Console.WriteLine(@"DividendYield: {0}", DividendYield);
            Console.WriteLine(@"DividendDocument: {0}", DividendDocument);
            Console.WriteLine(@"");
            if (DividendTaxes.CiShareFC != null)
            Console.WriteLine(@"CiShare.Name: {0}", DividendTaxes.CiShareFC.Name);
            Console.WriteLine(@"ValueWithoutTaxes: {0}", DividendTaxes.ValueWithoutTaxes);
            Console.WriteLine(@"ValueWithoutTaxesForeignCurrency: {0}", DividendTaxes.ValueWithoutTaxesFC);
            Console.WriteLine(@"ForeignCurrencyFlag: {0}", DividendTaxes.FCFlag);
            Console.WriteLine(@"ForeignCurrencyFactor: {0}", DividendTaxes.ExchangeRatio);
            Console.WriteLine(@"ForeignCurrencyUnit: {0}", DividendTaxes.FCUnit);
            Console.WriteLine(@"TaxAtSourceFlag: {0}", DividendTaxes.TaxAtSourceFlag);
            Console.WriteLine(@"TaxAtSourcePercentage: {0}", DividendTaxes.TaxAtSourcePercentage);
            Console.WriteLine(@"TaxAtSourceValue: {0}", DividendTaxes.TaxAtSourceValue);
            Console.WriteLine(@"CapitalGainsTaxFlag: {0}", DividendTaxes.CapitalGainsTaxFlag);
            Console.WriteLine(@"CapitalGainsTaxPercentage: {0}", DividendTaxes.CapitalGainsTaxPercentage);
            Console.WriteLine(@"CapitalGainsTaxValue: {0}", DividendTaxes.CapitalGainsTaxValue);
            Console.WriteLine(@"SolidarityTaxFlag: {0}", DividendTaxes.SolidarityTaxFlag);
            Console.WriteLine(@"SolidarityTaxPercentage: {0}", DividendTaxes.SolidarityTaxPercentage);
            Console.WriteLine(@"SolidarityTaxValue: {0}", DividendTaxes.SolidarityTaxValue);
            Console.WriteLine(@"ValueWithTaxes: {0}", DividendTaxes.ValueWithTaxes);
            Console.WriteLine(@"ValueWithTaxesForeignCurrency: {0}", DividendTaxes.ValueWithTaxesFC);
            Console.WriteLine(@"");
#endif
        }

        /// <summary>
        /// This function caluclates the dividend percent value
        /// and the dividend value for the complete volume of shares
        /// The values are stored in the member variables
        /// </summary>
        private void CalculateDividendValues()
        {
            if (DividendTaxes.FCFlag)
            {
                DividendTaxes.ValueWithoutTaxesFC = Math.Round(DividendRate * ShareVolume, 2, MidpointRounding.AwayFromZero);
                DividendTaxes.ValueWithoutTaxes = Math.Round((DividendRate / DividendTaxes.ExchangeRatio) * ShareVolume, 2, MidpointRounding.AwayFromZero);

                // Calculate the percent value of dividend paid of a share
                DividendYield = (DividendRate / DividendTaxes.ExchangeRatio) / SharePrice * 100;
            }
            else
            {
                DividendTaxes.ValueWithoutTaxes = Math.Round(DividendRate * ShareVolume, 2, MidpointRounding.AwayFromZero);
                // Calculate the percent value of dividend paid of a share
                DividendYield = DividendRate / SharePrice * 100;
            }

            // Calculate the paid dividend of a share
            DividendPayOut = DividendTaxes.ValueWithoutTaxes;
            DividendPayOutWithTaxes = DividendTaxes.ValueWithTaxes;
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
            return DateTime.Compare(Convert.ToDateTime(dividendObject1.DividendDate), Convert.ToDateTime(dividendObject2.DividendDate));
        }

        #endregion Methods
    }

}
