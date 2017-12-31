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
using System.Globalization;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
    public class BuyObject
    {
        #region Variables

        /// <summary>
        /// Stores the buy volume
        /// </summary>
        private decimal _volume = -1;

        /// <summary>
        /// Stores the price of the share
        /// </summary>
        private decimal _sharePrice = -1;

        /// <summary>
        /// Stores the discount value of the buy
        /// </summary>
        private decimal _reduction = -1;

        /// <summary>
        /// Stores the costs value of the buy
        /// </summary>
        private decimal _costs = -1;

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo BuyCultureInfo { get; internal set; }

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
                if (_volume == value)
                    return;
                _volume = value;

                // Calculate the values
                CalculateMarketValueAndMarketValueReduction();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string VolumeAsStr => Helper.FormatDecimal(Volume, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string VolumeAsStrUnit => Helper.FormatDecimal(Volume, Helper.Volumefivelength, false, Helper.Volumetwofixlength, true, ShareObject.PieceUnit, BuyCultureInfo);

        [Browsable(false)]
        public decimal SharePrice
        {
            get => _sharePrice;
            internal set
            {
                if (_sharePrice == value)
                    return;
                _sharePrice = value;

                // Calculate the values
                CalculateMarketValueAndMarketValueReduction();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Price")]
        public string SharePriceAsStr => Helper.FormatDecimal(SharePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string SharePriceAsStrUnit => Helper.FormatDecimal(SharePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal Reduction
        {
            get => _reduction;
            internal set
            {
                if (_reduction == value)
                    return;
                _reduction = value;

                // Calculate the values
                CalculateMarketValueAndMarketValueReduction();
            }
        }

        [Browsable(false)]
        public string ReductionAsStr => Helper.FormatDecimal(Reduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string ReductionAsStrUnit => Helper.FormatDecimal(Reduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal Costs
        {
            get => _costs;
            internal set
            {
                if (_costs == value)
                    return;
                _costs = value;

                // Calculate the values
                CalculateMarketValueAndMarketValueReduction();
            }
        }

        [Browsable(false)]
        public string CostsAsStr => Helper.FormatDecimal(Costs, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string CostsAsStrUnit => Helper.FormatDecimal(Costs, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal MarketValue { get; internal set; } = -1;

        [Browsable(true)]
        [DisplayName(@"Value")]
        public string MarketValueAsStr => Helper.FormatDecimal(MarketValue, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string MarketValueAsStrUnit => Helper.FormatDecimal(MarketValue, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal MarketValueReduction { get; internal set; } = -1;

        [Browsable(false)]
        public string MarketValueReductionAsStr => Helper.FormatDecimal(MarketValueReduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string MarketValueReductionAsStrUnit => Helper.FormatDecimal(MarketValueReduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal MarketValueReductionCosts { get; internal set; } = -1;

        [Browsable(false)]
        public string MarketValueReductionCostsAsStr => Helper.FormatDecimal(MarketValueReductionCosts, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string MarketValueReductionCostsAsStrUnit => Helper.FormatDecimal(MarketValueReductionCosts, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public string Document { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string DocumentAsStr => Helper.GetFileName(Document);

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="buyDate">Date of the share buy</param>
        /// <param name="decVolume">Volume of the bought shares</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="decReduction">Reduction of the buy</param>
        /// <param name="decCosts">Costs of the buy</param>
        /// <param name="strDoc">Document of the share buy</param>
        public BuyObject(CultureInfo cultureInfo, string buyDate, decimal decVolume, decimal decSharePrice, decimal decReduction, decimal decCosts, string strDoc = "")
        {
            BuyCultureInfo = cultureInfo;
            Date = buyDate;
            Volume = decVolume;
            Reduction = decReduction;
            Costs = decCosts;
            SharePrice = decSharePrice;
            Document = strDoc;

#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"New buy created");
            Console.WriteLine(@"Date: {0}", Date);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"SharePrice: {0}", SharePrice);
            Console.WriteLine(@"MarketValue: {0}", MarketValue);
            Console.WriteLine(@"Reduction: {0}", Reduction);
            Console.WriteLine(@"Costs: {0}", Costs);
            Console.WriteLine(@"Document: {0}", Document);
            Console.WriteLine(@"");
#endif
        }

        /// <summary>
        /// This function calculates the market value with the reduction
        /// with the given volume and share price of the buy
        /// </summary>
        private void CalculateMarketValueAndMarketValueReduction()
        {
            Helper.CalcBuyValues( Volume, SharePrice, Costs,
                Reduction, out var decMarketValue, out var decMarketValueReduction, out var decMarketValueReductionCosts);

            MarketValue = decMarketValue;
            MarketValueReduction = decMarketValueReduction;
            MarketValueReductionCosts = decMarketValueReductionCosts;
        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This is the comparer class for the BuyObject.
    /// It is used for the sort for the buys lists.
    /// </summary>
    public class BuyObjectComparer : IComparer<BuyObject>
    {
        #region Methods

        public int Compare(BuyObject object1, BuyObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }

}
