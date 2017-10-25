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

namespace SharePortfolioManager
{
    public class SaleObject
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the sale
        /// </summary>
        private CultureInfo _saleCultureInfo;

        /// <summary>
        /// Stores the date string of the sale date
        /// </summary>
        private string _date;

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
        private decimal _taxSum = 0;

        /// <summary>
        /// Stores the costs of the sale
        /// </summary>
        private decimal _costs = -1;

        /// <summary>
        /// Stores the sold purchase value of the bought purchase value
        /// </summary>
        private decimal _purchaseValue = -1;

        /// <summary>
        /// Stores the profit / loss with costs of the sale
        /// </summary>
        private decimal _profitLoss = 0;

        /// <summary>
        /// Stores the profit / loss without costs of the sale
        /// </summary>
        private decimal _profitLossWithoutCosts = 0;

        /// <summary>
        /// Stores the payout with costs of the sale
        /// </summary>
        private decimal _payout = 0;

        /// <summary>
        /// Stores the payout without costs of the sale
        /// </summary>
        private decimal _payoutWithOutCosts = 0;

        /// <summary>
        /// Stores the document of the sale
        /// </summary>
        private string _document = "";

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo SaleCultureInfo
        {
            get { return _saleCultureInfo; }
            internal set { _saleCultureInfo = value; }
        }

        [Browsable(false)]
        public string Date
        {
            get { return _date; }
            internal set { _date = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string DateAsStr
        {
            get { return _date; }
        }

        [Browsable(false)]
        public decimal Volume
        {
            get { return _volume; }
            internal set
            {
                _volume = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string VolumeAsStr
        {
            get { return Helper.FormatDecimal(_volume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string VolumeWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_volume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PieceUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal BuyPrice
        {
            get { return _buyPrice; }
            internal set
            {
                _buyPrice = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string BuyPriceAsStr
        {
            get { return BuyPrice.ToString(); /*Helper.FormatDecimal(_buyPrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);*/ }
        }

        [Browsable(false)]
        public string BuyPriceWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_buyPrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal SalePrice
        {
            get { return _salePrice; }
            internal set
            {
                _salePrice = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string SalePriceAsStr
        {
            get { return Helper.FormatDecimal(_salePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string SalePriceWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_salePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal TaxAtSource
        {
            get { return _taxAtSource; }
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
        public string TaxAtSourceAsStr
        {
            get { return Helper.FormatDecimal(_taxAtSource, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string TaxAtSourceWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_taxAtSource, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal CapitalGainsTax
        {
            get { return _capitalGainsTax; }
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
        public string CapitalGainsTaxAsStr
        {
            get { return Helper.FormatDecimal(_capitalGainsTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string CapitalGainsTaxWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_capitalGainsTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal SolidarityTax
        {
            get { return _solidarityTax; }
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
        public string SolidarityTaxAsStr
        {
            get { return Helper.FormatDecimal(_solidarityTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string SolidarityTaxWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_solidarityTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal TaxSum
        {
            get { return _taxSum; }
            internal set
            {
                _taxSum = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string TaxSumTaxAsStr
        {
            get { return Helper.FormatDecimal(_taxSum, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string TaxSumWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_taxSum, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal Costs
        {
            get { return _costs; }
            internal set
            {
                _costs = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string CostsAsStr
        {
            get { return Helper.FormatDecimal(_costs, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string CostsWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_costs, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal PurchaseValue
        {
            get { return _purchaseValue; }
            internal set
            {
                _purchaseValue = value;
            }
        }

        [Browsable(true)]
        [DisplayName("PurchaseValue")]
        public string PurchaseValueAsStr
        {
            get { return Helper.FormatDecimal(_purchaseValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string PurchaseValueWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_purchaseValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal ProfitLoss
        {
            get { return _profitLoss; }
            internal set
            {
                _profitLoss = value;
            }
        }

        [Browsable(true)]
        [DisplayName("ProfitLoss")]
        public string ProfitLossAsStr
        {
            get { return Helper.FormatDecimal(_profitLoss, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string ProfitLossWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_profitLoss, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal ProfitLossWithoutCosts
        {
            get { return _profitLossWithoutCosts; }
            internal set
            {
                _profitLossWithoutCosts = value;
            }
        }

        [Browsable(false)]
        public string ProfitLossWithoutCostsAsStr
        {
            get { return Helper.FormatDecimal(_profitLossWithoutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string ProfitLossWithoutCostsWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_profitLossWithoutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal Payout
        {
            get { return _payout; }
            internal set
            {
                _payout = value;
            }
        }

        [Browsable(true)]
        [DisplayName("Payout")]
        public string PayoutAsStr
        {
            get { return Helper.FormatDecimal(_payout, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string PayoutWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_payout, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public decimal PayoutWithoutCosts
        {
            get { return _payoutWithOutCosts; }
            internal set
            {
                _payoutWithOutCosts = value;
            }
        }

        [Browsable(false)]
        public string PayoutWithoutCostsAsStr
        {
            get { return Helper.FormatDecimal(_payoutWithOutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        [Browsable(false)]
        public string PayoutWithoutCostsWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_payoutWithOutCosts, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, SaleCultureInfo); }
        }

        [Browsable(false)]
        public string Document
        {
            get { return _document; }
            internal set
            {
                _document = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string DocumentFileName
        {
            get { return Helper.GetFileName(_document); }
        }

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
        public SaleObject(CultureInfo cultureInfo, string strDate, decimal decVolume, decimal decBuyPrice, decimal decSalePrice, decimal decTaxAtSource, decimal decCapitalGains,
             decimal decSolidarityTax, decimal decCosts, string strDoc = "")
        {
            SaleCultureInfo = cultureInfo;
            Date = strDate;
            Volume = decVolume;
            BuyPrice = decBuyPrice;
            SalePrice = decSalePrice;
            TaxAtSource = decTaxAtSource;
            CapitalGainsTax = decCapitalGains;
            SolidarityTax = decSolidarityTax;
            Costs = decCosts;
            Document = strDoc;

#if DEBUG
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
            decimal decSaleValue = 0;
            if (Volume > -1 && SalePrice > -1)
            {
                PurchaseValue = Volume * BuyPrice;
                decSaleValue = Volume * SalePrice;

                ProfitLossWithoutCosts = decSaleValue - PurchaseValue - TaxSum;
                ProfitLoss = ProfitLossWithoutCosts - Costs;
                PayoutWithoutCosts = decSaleValue - TaxSum;
                Payout = PayoutWithoutCosts - Costs;
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// This is the comparer class for the SaleObject.
    /// It is used for the sort for the sales lists.
    /// </summary>
    public class SaleObjectComparer : IComparer<SaleObject>
    {
        #region Methods

        public int Compare(SaleObject object1, SaleObject object2)
        {
            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }

    public class ProfitLossObject
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the profit / loss
        /// </summary>
        private CultureInfo _profitLossCultureInfo;

        /// <summary>
        /// Stores the date string of the sale date
        /// </summary>
        private string _date;

        /// <summary>
        /// Stores the profit or loss of the sale
        /// </summary>
        private decimal _profitLoss = -1;

        /// <summary>
        /// Stores the document of the sale
        /// </summary>
        private string _document = "";

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo ProfitLossCultureInfo
        {
            get { return _profitLossCultureInfo; }
            internal set { _profitLossCultureInfo = value; }
        }

        [Browsable(false)]
        public string Date
        {
            get { return _date; }
            internal set { _date = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string DateFormatted
        {
            get { return _date; }
        }

        [Browsable(false)]
        public decimal ProfitLoss
        {
            get { return _profitLoss; }
            internal set
            {
                _profitLoss = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"ProfitLoss")]
        public string ProfitLossAsStr
        {
            get { return Helper.FormatDecimal(_profitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @""); }
        }

        [Browsable(false)]
        public string ProfitLossWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_profitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit); }
        }

        [Browsable(false)]
        public string Document
        {
            get { return _document; }
            internal set
            {
                _document = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string DocumentFormatted
        {
            get { return Helper.GetFileName(_document); }
        }

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

#if DEBUG
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

    /// <summary>
    /// This is the comparer class for the ProfitLossObject.
    /// It is used for the sort for the profit or loss lists.
    /// </summary
    public class ProfitLossObjectComparer : IComparer<ProfitLossObject>
    {
        #region Methods

        public int Compare(ProfitLossObject object1, ProfitLossObject object2)
        {
            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }

}

