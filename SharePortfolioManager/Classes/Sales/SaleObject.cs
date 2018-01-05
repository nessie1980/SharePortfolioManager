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
using System.Globalization;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
    public class SaleObject
    {
        #region Variables

        /// <summary>
        /// Stores the sale volume
        /// </summary>
        private decimal _volume = -1;

        /// <summary>
        /// Stores the buy price of the share
        /// </summary>
        private decimal _buyPrice = -1;

        /// <summary>
        /// Stores the sale price of the sale
        /// </summary>
        private decimal _salePrice = -1;

        /// <summary>
        /// Stores the tax at source
        /// </summary>
        private decimal _taxAtSource = -1;

        /// <summary>
        /// Stores the capital gains tax
        /// </summary>
        private decimal _capitalGainsTax = -1;

        /// <summary>
        /// Stores the solidarity tax
        /// </summary>
        private decimal _solidarityTax = -1;

        /// <summary>
        /// Stores the sum of all paid taxes
        /// </summary>
        private decimal _taxSum;

        /// <summary>
        /// Stores the costs of the sale
        /// </summary>
        private decimal _costs = -1;

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo SaleCultureInfo { get; internal set; }

        [Browsable(false)]
        public string Date { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string DateAsStr => Date;

        [Browsable(false)]
        public decimal Volume
        {
            get => _volume;
            internal set
            {
                _volume = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string VolumeAsStr => Helper.FormatDecimal(_volume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string VolumeWithUnitAsStr => Helper.FormatDecimal(_volume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PieceUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal BuyPrice
        {
            get => _buyPrice;
            internal set
            {
                _buyPrice = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string BuyPriceAsStr => BuyPrice.ToString("G");

        [Browsable(false)]
        public string BuyPriceWithUnitAsStr => Helper.FormatDecimal(_buyPrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal SalePrice
        {
            get => _salePrice;
            internal set
            {
                _salePrice = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string SalePriceAsStr => Helper.FormatDecimal(_salePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string SalePriceWithUnitAsStr => Helper.FormatDecimal(_salePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal TaxAtSource
        {
            get => _taxAtSource;
            internal set
            {
                if (_taxAtSource > -1)
                    _taxSum -= _taxAtSource;

                _taxAtSource = value;

                if (_taxAtSource > -1)
                    _taxSum += _taxAtSource;
            }
        }

        [Browsable(false)]
        public string TaxAtSourceAsStr => Helper.FormatDecimal(_taxAtSource, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string TaxAtSourceWithUnitAsStr => Helper.FormatDecimal(_taxAtSource, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal CapitalGainsTax
        {
            get => _capitalGainsTax;
            internal set
            {
                if (_capitalGainsTax > -1)
                    _taxSum -= _capitalGainsTax;

                _capitalGainsTax = value;

                if (_capitalGainsTax > -1)
                    _taxSum += _capitalGainsTax;
            }
        }

        [Browsable(false)]
        public string CapitalGainsTaxAsStr => Helper.FormatDecimal(_capitalGainsTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string CapitalGainsTaxWithUnitAsStr => Helper.FormatDecimal(_capitalGainsTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal SolidarityTax
        {
            get => _solidarityTax;
            internal set
            {
                if (_solidarityTax > -1)
                    _taxSum -= _solidarityTax;

                _solidarityTax = value;

                if (_solidarityTax > -1)
                    _taxSum += _solidarityTax;
            }
       }

        [Browsable(false)]
        public string SolidarityTaxAsStr => Helper.FormatDecimal(_solidarityTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string SolidarityTaxWithUnitAsStr => Helper.FormatDecimal(_solidarityTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal TaxSum
        {
            get => _taxSum;
            internal set
            {
                _taxSum = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string TaxSumTaxAsStr => Helper.FormatDecimal(_taxSum, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string TaxSumWithUnitAsStr => Helper.FormatDecimal(_taxSum, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal Costs
        {
            get => _costs;
            internal set
            {
                _costs = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string CostsAsStr => Helper.FormatDecimal(_costs, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string CostsWithUnitAsStr => Helper.FormatDecimal(_costs, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal PurchaseValue { get; internal set; } = -1;

        [Browsable(true)]
        [DisplayName("PurchaseValue")]
        public string PurchaseValueAsStr => Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string PurchaseValueWithUnitAsStr => Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal ProfitLoss { get; internal set; }

        [Browsable(true)]
        [DisplayName("ProfitLoss")]
        public string ProfitLossAsStr => Helper.FormatDecimal(ProfitLoss, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string ProfitLossWithUnitAsStr => Helper.FormatDecimal(ProfitLoss, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal ProfitLossWithoutCosts { get; internal set; }

        [Browsable(false)]
        public string ProfitLossWithoutCostsAsStr => Helper.FormatDecimal(ProfitLossWithoutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string ProfitLossWithoutCostsWithUnitAsStr => Helper.FormatDecimal(ProfitLossWithoutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal Payout { get; internal set; }

        [Browsable(true)]
        [DisplayName("Payout")]
        public string PayoutAsStr => Helper.FormatDecimal(Payout, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string PayoutWithUnitAsStr => Helper.FormatDecimal(Payout, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public decimal PayoutWithoutCosts { get; internal set; }

        [Browsable(false)]
        public string PayoutWithoutCostsAsStr => Helper.FormatDecimal(PayoutWithoutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string PayoutWithoutCostsWithUnitAsStr => Helper.FormatDecimal(PayoutWithoutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo);

        [Browsable(false)]
        public string Document { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string DocumentFileName => Helper.GetFileName(Document);

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decBuyPrice">Buy price of the share</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="decCosts">Costs of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        public SaleObject(CultureInfo cultureInfo, string strDate, decimal decVolume, decimal decBuyPrice, decimal decSalePrice, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, decimal decCosts, string strDoc = "")
        {
            SaleCultureInfo = cultureInfo;
            Date = strDate;
            Volume = decVolume;
            BuyPrice = decBuyPrice;
            SalePrice = decSalePrice;
            TaxAtSource = decTaxAtSource;
            CapitalGainsTax = decCapitalGainsTax;
            SolidarityTax = decSolidarityTax;
            Costs = decCosts;
            Document = strDoc;

#if DEBUG_SALE
            Console.WriteLine(@"");
            Console.WriteLine(@"New sale created");
            Console.WriteLine(@"Date: {0}", Date);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"BuyPrice: {0}", BuyPrice);
            Console.WriteLine(@"SalePrice: {0}", SalePrice);
            Console.WriteLine(@"TaxAtSource: {0}", TaxAtSource);
            Console.WriteLine(@"CapitalGainsTax: {0}", CapitalGainsTax);
            Console.WriteLine(@"SolidarityTax: {0}", SolidarityTax);
            Console.WriteLine(@"Costs: {0}", Costs);
            Console.WriteLine(@"Document: {0}", Document);
            Console.WriteLine(@"");
#endif
        }

        /// <summary>
        /// This function calculates the profit / loss and payout of the sale
        /// </summary>
        private void CalculateProfitLossAndPayout()
        {
            if (Volume <= -1 || SalePrice <= -1) return;

            var decSaleValue = Volume * SalePrice;
            PurchaseValue = Volume * BuyPrice;

            ProfitLossWithoutCosts = decSaleValue - PurchaseValue - TaxSum;
            ProfitLoss = ProfitLossWithoutCosts - Costs;
            PayoutWithoutCosts = decSaleValue - TaxSum;
            Payout = PayoutWithoutCosts - Costs;
        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This is the comparer class for the SaleObject.
    /// It is used for the sort for the sales lists.
    /// </summary>
    public class SaleObjectComparer : IComparer<SaleObject>
    {
        #region Methods

        public int Compare(SaleObject object1, SaleObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }

    public class ProfitLossObject
    {
        #region Variables

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo ProfitLossCultureInfo { get; internal set; }

        [Browsable(false)]
        public string Date { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string DateFormatted => Date;

        [Browsable(false)]
        public decimal ProfitLoss { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"ProfitLoss")]
        public string ProfitLossAsStr => Helper.FormatDecimal(ProfitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);

        [Browsable(false)]
        public string ProfitLossWithUnitAsStr => Helper.FormatDecimal(ProfitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit);

        [Browsable(false)]
        public string Document { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string DocumentFormatted => Helper.GetFileName(Document);

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decProftiLoss">Value of the profit or loss</param>
        /// <param name="strDoc">Document of the sale</param>
        public ProfitLossObject(CultureInfo cultureInfo, string strDate, decimal decProftiLoss, string strDoc = "")
        {
            ProfitLossCultureInfo = cultureInfo;
            Date = strDate;
            ProfitLoss = decProftiLoss;
            Document = strDoc;

#if DEBUG_SALE
            Console.WriteLine(@"");
            Console.WriteLine(@"New sale created");
            Console.WriteLine(@"Date: {0}", Date);
            Console.WriteLine(@"ProfitLoss: {0}", ProfitLoss);
            Console.WriteLine(@"Document: {0}", Document);
            Console.WriteLine(@"");
#endif

        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This is the comparer class for the ProfitLossObject.
    /// It is used for the sort for the profit or loss lists.
    /// </summary>
    public class ProfitLossObjectComparer : IComparer<ProfitLossObject>
    {
        #region Methods

        public int Compare(ProfitLossObject object1, ProfitLossObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }
}

