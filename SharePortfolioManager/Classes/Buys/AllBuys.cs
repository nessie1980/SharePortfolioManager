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
using System.Linq;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
    public class AllBuysOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the buys of a year of the share
        /// The string stores the different buy years
        /// The BuysOfAYearOfTheShare stores the buys of each year
        /// </summary>
        private readonly SortedDictionary<string, BuysYearOfTheShare> _allBuysOfTheShareDictionary = new SortedDictionary<string, BuysYearOfTheShare>();

        #endregion Variables

        #region Properties

        public CultureInfo BuyCultureInfo { get; internal set; }

        public decimal BuyMarketValueTotal { get; internal set; }

        public string BuyMarketValueTotalAsStr => Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        public string BuyMarketValueTotalAsStrUnit => Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyMarketValueReductionTotal { get; internal set; }

        public string BuyMarketValueReductionTotalAsStr => Helper.FormatDecimal(BuyMarketValueReductionTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        public string BuyMarketValueReductionTotalAsStrUnit => Helper.FormatDecimal(BuyMarketValueReductionTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyMarketValueReductionCostsTotal { get; internal set; }

        public string BuyMarketValueReductionCostsTotalAsStr => Helper.FormatDecimal(BuyMarketValueReductionCostsTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        public string BuyMarketValueReductionCostsTotalAsStrUnit => Helper.FormatDecimal(BuyMarketValueReductionCostsTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyVolumeTotal { get; internal set; }

        public string BuyVolumeTotalAsStr => Helper.FormatDecimal(BuyVolumeTotal, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", BuyCultureInfo);

        public string BuyVolumeTotalAsStrUnit => Helper.FormatDecimal(BuyVolumeTotal, Helper.Volumefivelength, false, Helper.Volumetwofixlength, true, ShareObject.PieceUnit, BuyCultureInfo);

        public SortedDictionary<string, BuysYearOfTheShare> AllBuysOfTheShareDictionary => _allBuysOfTheShareDictionary;

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function sets the culture info of the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            BuyCultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a buy to the list
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <param name="decVolume">Volume of the buy</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="decReduction">Reduction value of the buy</param>
        /// <param name="decCosts">Costs of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strDateTime, decimal decVolume, decimal decSharePrice, decimal decReduction, decimal decCosts, string strDoc = "")
        {
#if DEBUG_BUY
            Console.WriteLine(@"");
            Console.WriteLine(@"Add AllBuysOfTheShare");
#endif
            try
            {
                // Get year of the date of the new buy
                GetYearOfDate(strDateTime, out var year);
                if (year == null)
                    return false;

                // Search if a buy for the given year already exists if not add it
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddBuyObject(BuyCultureInfo, strDateTime, decVolume, decSharePrice, decReduction, decCosts, strDoc))
                        return false;
                }
                else
                {
                    // Add new year buy object for the buy with a new year
                    var addObject = new BuysYearOfTheShare();
                    // Add buy with the new year to the buy year list
                    if (addObject.AddBuyObject(BuyCultureInfo, strDateTime, decVolume, decSharePrice, decReduction, decCosts, strDoc))
                    {
                        AllBuysOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total buy value and total buy volume
                // Reset total buy value and buy volume
                BuyMarketValueTotal = 0;
                BuyMarketValueReductionTotal = 0;
                BuyMarketValueReductionCostsTotal = 0;
                BuyVolumeTotal = 0;

                // Calculate the new total buy value and buy volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyMarketValueTotal += calcObject.BuyMarketValueYear;
                    BuyMarketValueReductionTotal += calcObject.BuyMarketValueReductionYear;
                    BuyMarketValueReductionCostsTotal += calcObject.BuyMarketValueReductionCostsYear;
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                }

#if DEBUG_BUY
                Console.WriteLine(@"MarketValueTotal:{0}", BuyMarketValueTotal);
                Console.WriteLine(@"PurchaseValueTotal:{0}", BuyMarketValueReductionTotal);
                Console.WriteLine(@"FinalValueTotal:{0}", BuyMarketValueReductionCostsTotal);
                Console.WriteLine(@"VolumeTotal:{0}", BuyVolumeTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function remove a buy with the given date and time
        /// from the dictionary
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy which should be removed</param>
        /// <returns></returns>
        public bool RemoveBuy(string strDateTime)
        {
#if DEBUG_BUY
            Console.WriteLine(@"Remove AllBuysOfTheShare");
#endif
            try
            {
                // Get year of the date of the buy which should be removed
                GetYearOfDate(strDateTime, out var year);
                if (year == null)
                    return false;

                // Search if the buy for the given year exists
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveBuyObject(strDateTime))
                        return false;

                    if (searchObject.BuyListYear.Count == 0)
                    {
                        if (!AllBuysOfTheShareDictionary.Remove(year))
                            return false;
                    }
                }
                else
                {
                    return false;
                }

                // Calculate the total buy value and volume
                // Reset total buy value and volume
                BuyMarketValueReductionCostsTotal = 0;
                BuyMarketValueReductionTotal = 0;
                BuyMarketValueTotal = 0;
                BuyVolumeTotal = 0;

                // Calculate the new total buy value and volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyMarketValueReductionCostsTotal += calcObject.BuyMarketValueReductionCostsYear;
                    BuyMarketValueReductionTotal += calcObject.BuyMarketValueReductionYear;
                    BuyMarketValueTotal += calcObject.BuyMarketValueYear;
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                }

#if DEBUG_BUY
                Console.WriteLine(@"MarketValueTotal:{0}", BuyMarketValueTotal);
                Console.WriteLine(@"MarketValueWithReduction:{0}", BuyMarketValueReductionTotal);
                Console.WriteLine(@"FinalValueTotal:{0}", BuyMarketValueReductionCostsTotal);
                Console.WriteLine(@"VolumeTotal:{0}", BuyVolumeTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function creates a list of all buy objects of the share
        /// </summary>
        /// <returns>List of BuyObjects or a empty list if no BuyObjects exists</returns>
        public List<BuyObject> GetAllBuysOfTheShare()
        {
            var allBuysOfTheShare = new List<BuyObject>();

            foreach (var buysYearOfTheShareObject in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buysYearOfTheShareObject.BuyListYear)
                {
                    allBuysOfTheShare.Add(buyObject);
                }
            }
            return allBuysOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total buy values of the years
        /// </summary>
        /// <returns>Dictionary with the years and the buys values of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllBuysTotalValues()
        {
            var allBuysOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllBuysOfTheShareDictionary.Keys)
            {
                allBuysOfTheShare.Add(key, Helper.FormatDecimal(AllBuysOfTheShareDictionary[key].BuyMarketValueYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo));
            }
            return allBuysOfTheShare;
        }

        /// <summary>
        /// This function returns the buy object of the given date and time
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <returns>BuyObject or null if the search failed</returns>
        public BuyObject GetBuyObjectByDateTime(string strDateTime)
        {
            GetYearOfDate(strDateTime, out var year);

            if (year == null) return null;

            if (!AllBuysOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var buyObject in AllBuysOfTheShareDictionary[year].BuyListYear)
            {
                if (buyObject.Date == strDateTime)
                {
                    return buyObject;
                }
            }

            return null;
        }

        /// <summary>
        /// This function checks if the given date is the last date of the entries
        /// </summary>
        /// <param name="strDateTime">Given date and time</param>
        /// <returns></returns>
        public bool IsDateLastDate(string strDateTime)
        {
            if (_allBuysOfTheShareDictionary.Count <= 0) return true;

            var lastYearEntries = _allBuysOfTheShareDictionary.Last().Value;
            {
                if (lastYearEntries.BuyListYear.Count <= 0) return true;

                var tempTimeDate = Convert.ToDateTime(lastYearEntries.BuyListYear.Last().Date);
                var givenTimeDate = Convert.ToDateTime(strDateTime);

                return givenTimeDate >= tempTimeDate;
            }
        }

        /// <summary>
        /// This function tries to get the year of the given date string
        /// </summary>
        /// <param name="date">Date string (DD.MM.YYYY)</param>
        /// <param name="year">Year of the given date or null if the split failed</param>
        private static void GetYearOfDate(string date, out string year)
        {
            var dateTimeElements = date.Split('.');
            if (dateTimeElements.Length != 3)
            {
                year = null;
            }
            else
            {
                var yearTime = dateTimeElements.Last();
                var dateElements = yearTime.Split(' ');
                year = dateElements.First();                   
            }
        }

        #endregion Methods
    }
}
