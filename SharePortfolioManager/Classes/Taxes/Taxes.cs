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
using System.Globalization;

namespace SharePortfolioManager.Classes.Taxes
{
    public class Taxes
    {
        #region Variables

        /// <summary>
        /// Stores the culture info for the currency of the share
        /// </summary>
        private CultureInfo _ciShareCurrency;

        /// <summary>
        /// Stores the culture info for the foreign currency of the share
        /// </summary>
        private CultureInfo _ciShareFC;

        /// <summary>
        /// Stores the normal currency unit of the share
        /// </summary>
        private string _strCurrencyUnit;

        /// <summary>
        /// Stores the foreign currency unit of the share
        /// </summary>
        private string _strFCUnit;

        /// <summary>
        /// Stores the flag if the share taxes are paid in a foreign currency
        /// </summary>
        private bool _bFCFlag;

        /// <summary>
        /// Stores the exchange ratio from the foreign currency to the normal share currency
        /// </summary>
        private decimal _decExchangeRatio;

        /// <summary>
        /// Stores the value in the normal currency of the share
        /// </summary>
        private decimal _decValueWithoutTaxes;

        /// <summary>
        /// Stores the value in the foreign currency of the share
        /// </summary>
        private decimal _decValueWithoutTaxesFC;

        /// <summary>
        /// Stores the value of the capital tax liable capital gains
        /// </summary>
        private decimal _decValueCapitalGains;

        /// <summary>
        /// Stores the value of the capital tax liable capital gains in the foreign currency
        /// </summary>
        private decimal _decValueCapitalGainsFC;

        /// <summary>
        /// Stores the loss balance value of the normal currency
        /// </summary>
        private decimal _decLossBalance;

        /// <summary>
        /// Stores the loss balance value of the foreign currency
        /// </summary>
        private decimal _decLossBalanceFC;

        /// <summary>
        /// Stores if the tax at source must be paid
        /// </summary>
        private bool _bTaxAtSource;

        /// <summary>
        /// Stores the percentage value of the tax at source
        /// </summary>
        private decimal _decTaxAtSourcePercentage;
        
        /// <summary>
        /// Stores the tax at source value
        /// </summary>
        private decimal _decTaxAtSourceValue;

        /// <summary>
        /// Stores the tax at source value in the foreign currency
        /// </summary>
        private decimal _decTaxAtSourceValueFC;

        /// <summary>
        /// Stores if the capital gains tax must be paid
        /// </summary>
        private bool _bCapitalGainsTax;

        /// <summary>
        /// Stores the percentage value of the capital gains tax
        /// </summary>
        private decimal _decCapitalGainsTaxPercentage;

        /// <summary>
        /// Stores the capital gains tax value
        /// </summary>
        private decimal _decCapitalGainsTaxValue;

        /// <summary>
        /// Stores the capital gains tax value in the foreign currency
        /// </summary>
        private decimal _decCapitalGainsTaxValueFC;

        /// <summary>
        /// Stores if the solidarity tax must be paid
        /// </summary>
        private bool _bSolidarityTax;

        /// <summary>
        /// Stores the percentage value of the solidarity tax
        /// </summary>
        private decimal _decSolidarityTaxPercentage;

        /// <summary>
        /// Stores the solidarity tax value
        /// </summary>
        private decimal _decSolidarityTaxValue;

        /// <summary>
        /// Stores the solidarity tax value in the foreign currency
        /// </summary>
        private decimal _decSolidarityTaxValueFC;

        /// <summary>
        /// Stores the value tax in the normal currency of the share
        /// </summary>
        private decimal _decValueTaxes;

        /// <summary>
        /// Stores the value taxes in the foreign currency of the share
        /// </summary>
        private decimal _decValueTaxesFC;

        /// <summary>
        /// Stores the value minus taxes in the normal currency of the share
        /// </summary>
        private decimal _decValueWithTaxes;

        /// <summary>
        /// Stores the value minus taxes in the foreign currency of the share
        /// </summary>
        private decimal _decValueWithTaxesFC;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Stores the culture info for the currency of the share
        /// </summary>
        public CultureInfo CiShareCurrency
        {
            get
            {
                if (_ciShareCurrency != null)
                    return _ciShareCurrency;
                else
                    return CultureInfo.CurrentCulture;
            }
            set
            {
                _ciShareCurrency = value;
                if (_ciShareCurrency != null)
                    _strCurrencyUnit = new RegionInfo(_ciShareCurrency.LCID).CurrencySymbol;
            }
        }

        /// <summary>
        /// Stores the culture info for the foreign currency of the share
        /// </summary>
        public CultureInfo CiShareFC
        {
            get
            {
                if (_ciShareFC != null)
                    return _ciShareFC;
                else
                    return CultureInfo.CurrentCulture;
            }
            set
            {
                _ciShareFC = value;
                if (_ciShareFC != null)
                    _strFCUnit = new RegionInfo(_ciShareFC.LCID).CurrencySymbol;
            }
        }

        /// <summary>
        /// Stores the normal currency unit of the share
        /// </summary>
        public string CurrencyUnit
        {
            get { return _strCurrencyUnit; }
            set { _strCurrencyUnit = value; }
        }

        /// <summary>
        /// Stores the foreign currency unit of the share
        /// </summary>
        public string FCUnit
        {
            get { return _strFCUnit; }
            set { _strFCUnit = value; }
        }

        /// <summary>
        /// Stores the flag if the share taxes are paid in a foreign currency
        /// </summary>
        public bool FCFlag
        {
            get { return _bFCFlag; }
            set { _bFCFlag = value; }
        }

        /// <summary>
        /// Stores the foreign currency flag formatted
        /// </summary>
        public string FCFlagAsString
        {
            get { return _bFCFlag.ToString(); }
        }

        /// <summary>
        /// Stores the exchange ratio from the foreign currency to the normal share currency
        /// </summary>
        public decimal ExchangeRatio
        {
            get { return _decExchangeRatio; }
            set
            {
                _decExchangeRatio = value;
            
                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the exchange ratio formatted
        /// </summary>
        public string ExchangeRatioAsString
        {
            get { return _decExchangeRatio.ToString(); }
        }

        /// <summary>
        /// Stores the value in the normal currency of the share
        /// </summary>
        public decimal ValueWithoutTaxes
        {
            get { return _decValueWithoutTaxes; }
            set
            {
                _decValueWithoutTaxes = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the value in the normal currency of the share
        /// </summary>
        public string ValueWithoutTaxesAsString
        {
            get
            {
                if (ValueWithoutTaxes == 0)
                    return @"-";
                else
                    return Helper.FormatDecimal(ValueWithoutTaxes, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the value capital gains value
        /// </summary>
        public decimal ValueCapitalGains
        {
            get { return _decValueCapitalGains; }
            internal set { _decValueCapitalGains = value; }
        }

        /// <summary>
        /// Stores the value capital gains value formatted
        /// </summary>
        public string ValueCapitalGainsAsString
        {
            get
            {
                if (ValueCapitalGains == 0)
                    return @"-";
                else
                    return Helper.FormatDecimal(ValueCapitalGains, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the value capital gains value in the foreign currency
        /// </summary>
        public decimal ValueCapitalGainsFC
        {
            get { return _decValueCapitalGainsFC; }
            internal set { _decValueCapitalGainsFC = value; }
        }

        /// <summary>
        /// Stores the value capital gains value in the foreign currency formatted
        /// </summary>
        public string ValueCapitalGainsFCAsString
        {
            get
            {
                if (ValueCapitalGainsFC == 0)
                    return @"-";
                else
                    return Helper.FormatDecimal(ValueCapitalGainsFC, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores loss balance value of the normal currency
        /// </summary>
        public decimal LossBalance
        {
            get { return _decLossBalance; }
            set
            {
                _decLossBalance = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the loss balance value of the normal currency formatted
        /// </summary>
        public string LossBalanceAsString
        {
            get
            {
                if (LossBalance == 0)
                    return @"-";
                else
                    return Helper.FormatDecimal(LossBalance, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the value in the foreign currency of the share minus loss balance value
        /// </summary>
        public decimal LossBalanceFC
        {
            get { return _decLossBalanceFC; }
            internal set
            {
                _decLossBalanceFC = value;
            }
        }

        /// <summary>
        /// Stores the value in the foreign currency of the share minus loss balance value formatted
        /// </summary>
        public string LossBalanceFCAsString
        {
            get
            {
                if (LossBalanceFC == 0)
                    return @"-";
                else
                    return Helper.FormatDecimal(LossBalanceFC, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CiShareFC);
            }
        }

        /// <summary>
        /// Stores the value in the foreign currency of the share
        /// </summary>
        public decimal ValueWithoutTaxesFC
        {
            get { return _decValueWithoutTaxesFC; }
            set
            {
                _decValueWithoutTaxesFC = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the value in the foreign currency of the share formatted
        /// </summary>
        public string ValueWithoutTaxesFCAsString
        {
            get
            {
                if (_decValueWithoutTaxesFC == 0)
                    return @"-";
                else
                    return Helper.FormatDecimal(_decValueWithoutTaxesFC, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CiShareFC);
            }
        }

        /// <summary>
        /// Stores if the tax at source must be paid
        /// </summary>
        public bool TaxAtSourceFlag
        {
            get { return _bTaxAtSource; }
            set
            {
                _bTaxAtSource = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the tax at source flag formatted
        /// </summary>
        public string TaxAtSourceFlagAsString
        {
            get { return _bTaxAtSource.ToString(); }
        }

        /// <summary>
        /// Stores the percentage value of the tax at source
        /// </summary>
        public decimal TaxAtSourcePercentage
        {
            get { return _decTaxAtSourcePercentage; }
            set
            {
                _decTaxAtSourcePercentage = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the percentage value of the tax at source formatted
        /// </summary>
        public string TaxAtSourcePercentageAsString
        {
            get
            {
                if (_decTaxAtSourcePercentage == 0 || _decTaxAtSourcePercentage == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(_decTaxAtSourcePercentage, Helper.Percentagefivelength, false, Helper.Percentagetwofixlength, false, @"", CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the tax at source value
        /// </summary>
        public decimal TaxAtSourceValue
        {
            get { return _decTaxAtSourceValue; }
            internal set
            {
                _decTaxAtSourceValue = value;

                // Calculate the remaining values
                //CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the tax at source value formatted
        /// </summary>
        public string TaxAtSourceValueAsString
        {
            get
            {
                if (_decTaxAtSourceValue == 0 || _decTaxAtSourceValue == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decTaxAtSourceValue, 2), Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the tax at source value in the foreign currency
        /// </summary>
        public decimal TaxAtSourceValueFC
        {
            get { return _decTaxAtSourceValueFC; }
            internal set { _decTaxAtSourceValueFC = value; }
        }

        /// <summary>
        /// Stores the tax at source value formatted
        /// </summary>
        public string TaxAtSourceValueFCAsString
        {
            get
            {
                if (_decTaxAtSourceValueFC == 0 || _decTaxAtSourceValueFC == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decTaxAtSourceValueFC, 2), Helper.Percentagefivelength, false, Helper.Percentagetwofixlength, false, @"", CiShareFC);
            }
        }

        /// <summary>
        /// Stores if the capital gains tax must be paid
        /// </summary>
        public bool CapitalGainsTaxFlag
        {
            get { return _bCapitalGainsTax; }
            set { _bCapitalGainsTax = value; }
        }

        /// <summary>
        /// Stores the capital gains tag flag formatted
        /// </summary>
        public string CapitalGainsTaxFlagAsString
        {
            get { return _bCapitalGainsTax.ToString(); }
        }

        /// <summary>
        /// Stores the percentage value of the capital gains tax
        /// </summary>
        public decimal CapitalGainsTaxPercentage
        {
            get { return _decCapitalGainsTaxPercentage; }
            set
            {
                _decCapitalGainsTaxPercentage = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the percentage value of the capital gains tax formatted
        /// </summary>
        public string CapitalGainsTaxPercentageAsString
        {
            get
            {
                if (_decCapitalGainsTaxPercentage == 0 || _decCapitalGainsTaxPercentage == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decCapitalGainsTaxPercentage, 2), Helper.Percentagetwolength, false, Helper.Percentagetwofixlength, false, ShareObject.PercentageUnit, CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the capital gains tax value
        /// </summary>
        public decimal CapitalGainsTaxValue
        {
            get { return _decCapitalGainsTaxValue; }
            internal set { _decCapitalGainsTaxValue = value; }
        }

        /// <summary>
        /// Stores the capital gains tax value formatted
        /// </summary>
        public string CapitalGainsTaxValueAsString
        {
            get
            {
                if (_decCapitalGainsTaxValue == 0 || _decCapitalGainsTaxValue == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decCapitalGainsTaxValue, 2), Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the capital gains tax value in the foreign currency
        /// </summary>
        public decimal CapitalGainsTaxValueFC
        {
            get { return _decCapitalGainsTaxValueFC; }
            internal set { _decCapitalGainsTaxValueFC = value; }
        }

        /// <summary>
        /// Stores the capital gains tax value formatted in the foreign currency
        /// </summary>
        public string CapitalGainsTaxValueFCAsString
        {
            get
            {
                if (_decCapitalGainsTaxValueFC == 0 || _decCapitalGainsTaxValueFC == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decCapitalGainsTaxValueFC, 2), Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareFC);
            }
        }

        /// <summary>
        /// Stores if the solidarity tax must be paid
        /// </summary>
        public bool SolidarityTaxFlag
        {
            get { return _bSolidarityTax; }
            set
            {
                _bSolidarityTax = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the solidarity tax flag formatted
        /// </summary>
        public string SolidarityTaxFlagAsString
        {
            get { return _bSolidarityTax.ToString(); }
        }

        /// <summary>
        /// Stores the percentage value of the solidarity tax
        /// </summary>
        public decimal SolidarityTaxPercentage
        {
            get { return _decSolidarityTaxPercentage; }
            set
            {
                _decSolidarityTaxPercentage = value;

                // Calculate the remaining values
                CalculationAndSetValues();
            }
        }

        /// <summary>
        /// Stores the percentage value of the solidarity tax formatted
        /// </summary>
        public string SolidarityTaxPercentageAsString
        {
            get
            {
                if (_decSolidarityTaxPercentage == 0 || _decSolidarityTaxPercentage == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decSolidarityTaxPercentage, 2), Helper.Percentagetwolength, false, Helper.Percentagetwofixlength, false, ShareObject.PercentageUnit, CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the solidarity tax value
        /// </summary>
        public decimal SolidarityTaxValue
        {
            get { return _decSolidarityTaxValue; }
            internal set { _decSolidarityTaxValue = value; }
        }

        /// <summary>
        /// Stores the solidarity tax value formatted
        /// </summary>
        public string SolidarityTaxValueAsString
        {
            get
            {
                if (_decSolidarityTaxValue == 0 || _decSolidarityTaxValue == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decSolidarityTaxValue, 2), Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the solidarity tax value in the foreign currency
        /// </summary>
        public decimal SolidarityTaxValueFC
        {
            get { return _decSolidarityTaxValueFC; }
            internal set { _decSolidarityTaxValueFC = value; }
        }

        /// <summary>
        /// Stores the solidarity tax value formatted in the foreign currency
        /// </summary>
        public string SolidarityTaxValueFCAsString
        {
            get
            {
                if (_decSolidarityTaxValueFC == 0 || _decSolidarityTaxValueFC == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decSolidarityTaxValueFC, 2), Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareFC);
            }
        }

        /// <summary>
        /// Stores the tax value in the normal currency
        /// </summary>
        public decimal TaxesValue
        {
            get { return _decValueTaxes; }
            internal set { _decValueTaxes = value; }
        }

        /// <summary>
        /// Stores the tax value in the foreign currency formatted
        /// </summary>
        public string TaxesValueAsString
        {
            get
            {
                if (_decValueTaxes == 0 || _decValueTaxes == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decValueTaxes, 2), Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the tax value in the foreign currency
        /// </summary>
        public decimal TaxesValueFC
        {
            get { return _decValueTaxesFC; }
            internal set { _decValueTaxesFC = value; }
        }

        /// <summary>
        /// Stores the tax value in the foreign currency formatted
        /// </summary>
        public string TaxesValueFCAsString
        {
            get
            {
                if (_decValueTaxesFC == 0 || _decValueTaxesFC == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decValueTaxesFC, 2), Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the value minus taxes in the normal currency of the share
        /// </summary>
        public decimal ValueWithTaxes
        {
            get { return _decValueWithTaxes; }
            internal set { _decValueWithTaxes = value; }
        }

        /// <summary>
        /// Stores the value minus taxes in the normal currency of the share formatted
        /// </summary>
        public string ValueWithTaxesAsString
        {
            get
            {
                if (_decValueWithTaxes == 0 || _decValueWithTaxes == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decValueWithTaxes, 2), Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareCurrency);
            }
        }

        /// <summary>
        /// Stores the value minus taxes in the foreign currency of the share
        /// </summary>
        public decimal ValueWithTaxesFC
        {
            get { return _decValueWithTaxesFC; }
            internal set { _decValueWithTaxesFC = value; }
        }

        /// <summary>
        /// Stores the value minus taxes in the foreign currency of the share formatted
        /// </summary>
        public string ValueWithTaxesFCAsString
        {
            get
            {
                if (_decValueWithTaxesFC == 0 || _decValueWithTaxesFC == decimal.MinValue / 2)
                    return @"-";
                else
                    return Helper.FormatDecimal(Math.Round(_decValueWithTaxesFC, 2), Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, ShareObject.PercentageUnit, CiShareFC);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public Taxes()
        { }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="ciShare">Culture info of the share</param>
//        /// <param name="ciShareFC">Culture info of the foreign currency</param>
//        /// <param name="foreignCurrencyFlag">Flag if the taxes is paid in a foreign currency</param>
//        /// <param name="exchangeRatio">Foreign currency factor</param>
//        /// <param name="currencyUnit">Normal currency unit</param>
//        /// <param name="foreignCurrencyUnit">Foreign currency unit</param>
//        /// <param name="valueWithoutTaxes">Value without taxes</param>
//        /// <param name="lossBalance">Value of the loss balance</param>
//        /// <param name="taxAtSourceFlag">Flag if tax at source must be paid</param>
//        /// <param name="taxAtSourcePercentage">Percentage value for the tax at source</param>
//        /// <param name="capitalGainsTaxFlag">Flag if capital gains tax must be paid</param>
//        /// <param name="capitalGainsTaxPercentage">Percentage value for the capital gains tax</param>
//        /// <param name="solidarityTaxFlag">Flag if solidarity tax must be paid</param>
//        /// <param name="solidarityTaxPercentage">Percentage value for the solidarity tax</param>
//        public Taxes (CultureInfo ciShare, CultureInfo ciShareFC,
//            bool foreignCurrencyFlag, decimal exchangeRatio,
//            string currencyUnit, string foreignCurrencyUnit,
//            decimal valueWithoutTaxes, decimal lossBalance,
//            bool taxAtSourceFlag, decimal taxAtSourcePercentage,
//            bool capitalGainsTaxFlag, decimal capitalGainsTaxPercentage,
//            bool solidarityTaxFlag, decimal solidarityTaxPercentage)
//        {
//            CiShareCurrency = ciShare;
//            CiShareFC = CiShareFC;
//            FCFlag = foreignCurrencyFlag;
//            ExchangeRatio = exchangeRatio;
//            CurrencyUnit = currencyUnit;
//            FCUnit = foreignCurrencyUnit;
//            ValueWithoutTaxes = valueWithoutTaxes;
//            LossBalance = lossBalance;
//            TaxAtSourceFlag = taxAtSourceFlag;
//            TaxAtSourcePercentage = taxAtSourcePercentage;
//            CapitalGainsTaxFlag = capitalGainsTaxFlag;
//            CapitalGainsTaxPercentage = capitalGainsTaxPercentage;
//            SolidarityTaxFlag = solidarityTaxFlag;
//            SolidarityTaxPercentage = solidarityTaxPercentage;

//#if DEBUG
//            Console.WriteLine("");
//            Console.WriteLine("TaxesObject()");
//            Console.WriteLine("CiShare: {0}", CiShareFC);
//            Console.WriteLine("FCFlag: {0}", FCFlag);
//            Console.WriteLine("CurrencyUnit: {0}", CurrencyUnit);
//            Console.WriteLine("FCUnit: {0}", FCUnit);
//            Console.WriteLine("ValueWithoutTaxes: {0}", ValueWithoutTaxes);
//            Console.WriteLine("LossBalance: {0}", LossBalance);
//            Console.WriteLine("TaxAtSourceFlag: {0}", TaxAtSourceFlag);
//            Console.WriteLine("TaxAtSourcePercentage: {0}", TaxAtSourcePercentage);
//            Console.WriteLine("CapitalGainsTaxFlag: {0}", CapitalGainsTaxFlag);
//            Console.WriteLine("CapitalGainsTaxPercentage: {0}", CapitalGainsTaxPercentage);
//            Console.WriteLine("SolidarityTaxFlag: {0}", SolidarityTaxFlag);
//            Console.WriteLine("SolidarityTaxPercentage: {0}", SolidarityTaxPercentage);

//#endif

//            // Calculate the remaining values
//            CalculationAndSetValues();
//        }

        /// <summary>
        /// This function calculates the values
        /// </summary>
        private void CalculationAndSetValues()
        {
            // Set value of the payout with taxes to the current payout value without taxes
            _decValueWithTaxes = 0;
            _decValueWithTaxesFC = 0;

            _decTaxAtSourceValue = 0;
            _decTaxAtSourceValueFC = 0;

            _decCapitalGainsTaxValue = 0;
            _decCapitalGainsTaxValueFC = 0;

            _decSolidarityTaxValue = 0;
            _decSolidarityTaxValueFC = 0;

            decimal decPayoutCalculation = 0;
            decimal decPayoutCalculationFC = 0;

            // Set values to the calculation value
            if (FCFlag)
            {
                decPayoutCalculationFC = ValueWithoutTaxesFC;
                decPayoutCalculation = ValueWithoutTaxes;
            }
            else
            {
                decPayoutCalculation = ValueWithoutTaxes;
                decPayoutCalculationFC = 0;
            }

            // Tax at source
            if (TaxAtSourceFlag)
            {
                // Check if the dividend rate is paid in a foreign currency
                if (FCFlag)
                {
                    // Calculate tax value in the foreign currency
                    TaxAtSourceValueFC = ValueWithoutTaxesFC * (TaxAtSourcePercentage / 100);

                    // Calculate the foreign currency value for the capital gains calculation
                    TaxAtSourceValueFC = Math.Round(TaxAtSourceValueFC, 2, MidpointRounding.AwayFromZero);
                    
                    // Calculate tax value in the normal currency
                    TaxAtSourceValue = TaxAtSourceValueFC / ExchangeRatio;

                    // Calculate the normal currency value for the capital gains calculation
                    TaxAtSourceValue = Math.Round(TaxAtSourceValue, 2, MidpointRounding.AwayFromZero);

                    // Calculate new payout with tax at source value in the foreign currency
                    decPayoutCalculationFC -= TaxAtSourceValueFC;
                    // Calculate new payout with tax at source value in the normal currency
                    decPayoutCalculation -= TaxAtSourceValue;

                    // Calculate the foreign currency value for the capital gains calculation
                    ValueCapitalGainsFC = Math.Round(decPayoutCalculationFC, 2, MidpointRounding.AwayFromZero);
                    // Calculate the normal currency value for the capital gains calculation
                    ValueCapitalGains = Math.Round(decPayoutCalculation, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    // Calculate tax value in the normal currency
                    TaxAtSourceValue = ValueWithoutTaxes * (TaxAtSourcePercentage / 100);
                    TaxAtSourceValueFC = 0;

                    // Calculate the foreign currency value for the capital gains calculation
                    TaxAtSourceValue = Math.Round(TaxAtSourceValue, 2, MidpointRounding.AwayFromZero);

                    // Calculate new payout with tax at source value in the normal currency
                    decPayoutCalculation -= TaxAtSourceValue;
                    decPayoutCalculationFC = 0;

                    // Calculate the normal currency value for the capital gains calculation
                    ValueCapitalGains = Math.Round(decPayoutCalculation, 2, MidpointRounding.AwayFromZero);
                    ValueCapitalGainsFC = 0;
                }
            }

            // Capital gains tax
            if (CapitalGainsTaxFlag)
            {
                decimal decCapitalGainsTaxPercentage = CapitalGainsTaxPercentage;

                // Check if tax at source is paid
                if (TaxAtSourceFlag)
                {
                    if (CapitalGainsTaxPercentage != 0)
                        decCapitalGainsTaxPercentage = CapitalGainsTaxPercentage - TaxAtSourcePercentage;
                    else
                        decCapitalGainsTaxPercentage = 0;
                }

                // Calculate capital gains tax of the normal currency
                CapitalGainsTaxValue = (ValueWithoutTaxes - LossBalance) * (decCapitalGainsTaxPercentage / 100);
                CapitalGainsTaxValue = Math.Round(CapitalGainsTaxValue, 2, MidpointRounding.AwayFromZero);

                // Calculate payout minus capital gains tax
                if (CapitalGainsTaxValue > 0)
                    decPayoutCalculation -= CapitalGainsTaxValue;
                else
                    decPayoutCalculation = 0;

                // Check if the dividend rate is paid in a foreign currency
                //if (FCFlag)
                //{
                //    // Calculate tax value foreign currency
                //    _decCapitalGainsTaxValueFC = (ValueWithoutTaxesFC - LossBalance) * (decPercentage / 100);

                //    // Calculate new payout with taxes values
                //    decPayoutCalculationFC -= Math.Round(CapitalGainsTaxValueFC, 2, MidpointRounding.AwayFromZero);

                //    // Calculate capital gains tax of the normal price
                //    _decCapitalGainsTaxValue = Math.Round((ValueWithoutTaxes - LossBalance) * (decPercentage / 100), 2, MidpointRounding.AwayFromZero);

                //    // Calculate payout minus capital gains tax
                //    if (_decCapitalGainsTaxValue > 0)
                //        decPayoutCalculation -= CapitalGainsTaxValue;
                //    else
                //        decPayoutCalculation = 0;
                //}
            }

            // Solidarity tax
            if (SolidarityTaxFlag)
            {
                // Calculate solidarity tax of the normal price
                SolidarityTaxValue = CapitalGainsTaxValue * SolidarityTaxPercentage / 100;
                SolidarityTaxValue = Math.Round(SolidarityTaxValue, 2, MidpointRounding.AwayFromZero);

                // Calculate payout minus solidarity tax
                decPayoutCalculation -= SolidarityTaxValue;

                //// Check if the dividend rate is paid in a foreign currency
                //if (FCFlag)
                //{
                //    // Calculate tax value foreign currency
                //    _decSolidarityTaxValueFC = CapitalGainsTaxValueFC * (SolidarityTaxPercentage / 100);

                //    // Calculate new payout with taxes values
                //    decPayoutCalculationFC -= Math.Round(SolidarityTaxValueFC, 2, MidpointRounding.AwayFromZero);

                //    // Calculate solidarity tax of the normal price
                //    _decSolidarityTaxValue = CapitalGainsTaxValue * SolidarityTaxPercentage / 100;
                //    // Calculate payout minus solidarity tax
                //    decPayoutCalculation -= Math.Round(SolidarityTaxValue, 2, MidpointRounding.AwayFromZero);
                //}
            }

            // Set calculated values to the struct
            TaxesValue = TaxAtSourceValue + CapitalGainsTaxValue + SolidarityTaxValue;
            TaxesValueFC = TaxAtSourceValueFC + CapitalGainsTaxValueFC + SolidarityTaxValueFC;
            ValueWithTaxes = decPayoutCalculation;
            ValueWithTaxesFC = decPayoutCalculationFC;
        }

        /// <summary>
        /// This function copies the values of the given object to this object
        /// </summary>
        /// <returns>Clone of the object</returns>
        public void DeepCopy(Taxes copyObject)
        {
            CiShareCurrency = copyObject.CiShareCurrency;
            CiShareFC = copyObject.CiShareFC;
            FCFlag = copyObject.FCFlag;
            ExchangeRatio = copyObject.ExchangeRatio;
            CurrencyUnit = copyObject.CurrencyUnit;
            FCUnit = copyObject.FCUnit;
            ValueWithoutTaxes = copyObject.ValueWithoutTaxes;
            ValueWithoutTaxesFC = copyObject.ValueWithoutTaxesFC;
            LossBalance = copyObject.LossBalance;
            LossBalanceFC = copyObject.LossBalanceFC;
            TaxAtSourceFlag = copyObject.TaxAtSourceFlag;
            TaxAtSourcePercentage = copyObject.TaxAtSourcePercentage;
            CapitalGainsTaxFlag = copyObject.CapitalGainsTaxFlag;
            CapitalGainsTaxPercentage = copyObject.CapitalGainsTaxPercentage;
            SolidarityTaxFlag = copyObject.SolidarityTaxFlag;
            SolidarityTaxPercentage = copyObject.SolidarityTaxPercentage;
        }

        #endregion Methods
    }
}
