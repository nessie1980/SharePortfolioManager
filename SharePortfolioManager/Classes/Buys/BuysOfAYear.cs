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
using System.Globalization;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
    public class BuysYearOfTheShare
    {
        #region Properties

        public CultureInfo BuyCultureInfo { get; internal set; }

        public decimal BuyMarketValueYear { get; internal set; } = -1;

        public string BuyMarketValueYearAsStr => Helper.FormatDecimal(BuyMarketValueYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        public string BuyMarketValueYearAsStrUnit => Helper.FormatDecimal(BuyMarketValueYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyMarketValueReductionYear { get; internal set; } = -1;

        public string BuyMarketValueReductionYearAsStr => Helper.FormatDecimal(BuyMarketValueReductionYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        public string BuyMarketValueReductionYearAsStrUnit => Helper.FormatDecimal(BuyMarketValueReductionYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyMarketValueReductionCostsYear { get; internal set; } = -1;

        public string BuyMarketValueReductionCostsYearAsStr => Helper.FormatDecimal(BuyMarketValueReductionCostsYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        public string BuyMarketValueReductionCostsYearAsStrUnit => Helper.FormatDecimal(BuyMarketValueReductionCostsYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyVolumeYear { get; internal set; } = -1;

        public string BuyVolumeYearAsStr => Helper.FormatDecimal(BuyVolumeYear, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", BuyCultureInfo);

        public string BuyVolumeYearAsStrUnit => Helper.FormatDecimal(BuyVolumeYear, Helper.Volumefivelength, false, Helper.Volumetwofixlength, true, ShareObject.PieceUnit, BuyCultureInfo);

        public List<BuyObject> BuyListYear { get; } = new List<BuyObject>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// This functions adds a new buy object to the year list with the given values
        /// It also recalculates the value of the bought shares in the year
        /// </summary>
        /// <param name="cultureInfo">Culture info of the buy</param>
        /// <param name="strDate">Buy date of the new buy list entry</param>
        /// <param name="decVolume">Volume of the buy</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="decReduction">Reduction of the buy</param>
        /// <param name="decCosts">Costs of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuyObject(CultureInfo cultureInfo, string strDate, decimal decVolume, decimal decSharePrice, decimal decReduction, decimal decCosts, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"AddBuyObject");
#endif
            try
            {
                // Set culture info of the share
                BuyCultureInfo = cultureInfo;

                // Create new BuyObject
                var addObject = new BuyObject(cultureInfo, strDate, decVolume, decSharePrice, decReduction, decCosts, strDoc);

                // Add object to the list
                BuyListYear.Add(addObject);
                BuyListYear.Sort(new BuyObjectComparer());

                // Calculate buy value without reductions and costs
                if (BuyMarketValueYear == -1)
                    BuyMarketValueYear = 0;
                BuyMarketValueYear += addObject.MarketValue;

                // Calculate buy value with reduction
                if (BuyMarketValueReductionYear == -1)
                    BuyMarketValueReductionYear = 0;
                BuyMarketValueReductionYear += addObject.MarketValueReduction;

                // Calculate buy value with reduction and costs
                if (BuyMarketValueReductionCostsYear == -1)
                    BuyMarketValueReductionCostsYear = 0;
                BuyMarketValueReductionCostsYear += addObject.MarketValueReductionCosts;

                // Calculate buy volume
                if (BuyVolumeYear == -1)
                    BuyVolumeYear = 0;
                BuyVolumeYear += addObject.Volume;

#if DEBUG
                Console.WriteLine(@"MarketValueYear: {0}", BuyMarketValueYear);
                Console.WriteLine(@"PurchaseValueYear: {0}", BuyMarketValueReductionYear);
                Console.WriteLine(@"FinalValueYear: {0}", BuyMarketValueReductionCostsYear);
                Console.WriteLine(@"VolumeYear: {0}", BuyVolumeYear);
                Console.WriteLine(@"");
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function removes the buy object with the given date and time from the list
        /// It also recalculates the buy value and volume
        /// </summary>
        /// <param name="buyDateTime">Date and time of the buy object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveBuyObject(string buyDateTime)
        {
#if DEBUG
            Console.WriteLine(@"RemoveBuyObject");
#endif
            try
            {
                // Search for the remove object
                var iFoundIndex = -1;
                foreach (var buyObject in BuyListYear)
                {
                    if (buyObject.Date != buyDateTime) continue;

                    iFoundIndex =  BuyListYear.IndexOf(buyObject);
                    break;
                }
                // Save remove object
                var removeObject = BuyListYear[iFoundIndex];

                // Remove object from the list
                BuyListYear.Remove(removeObject);

                // Calculate buy value with reduction and costs
                BuyMarketValueReductionCostsYear -= removeObject.MarketValueReductionCosts;
                // Calculate buy value with reduction
                BuyMarketValueReductionYear -= removeObject.MarketValueReduction;
                // Calculate buy value without reduction and costs
                BuyMarketValueYear -= removeObject.MarketValue;
                // Calculate buy volume
                BuyVolumeYear -= removeObject.Volume;

#if DEBUG
                Console.WriteLine(@"MarketValueYear: {0}", BuyMarketValueYear);
                Console.WriteLine(@"FinalValueYear: {0}", BuyMarketValueReductionCostsYear);
                Console.WriteLine(@"VolumeYear: {0}", BuyVolumeYear);
                Console.WriteLine(@"");
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }

    public class BuysYearOfTheShareComparer : IComparer<BuysYearOfTheShare>
    {
        public int Compare(BuysYearOfTheShare object1, BuysYearOfTheShare object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            if (Convert.ToInt16(object2.BuyListYear) == Convert.ToInt16(object1.BuyListYear))
                return 0;
            if (Convert.ToInt16(object2.BuyListYear) > Convert.ToInt16(object1.BuyListYear))
                return 1;

            return -1;
        }
    }
}
