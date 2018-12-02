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
using System.Globalization;
using System.Linq;

namespace SharePortfolioManager.Classes.Buys
{
    [Serializable]
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

        public decimal BuyValueTotal { get; internal set; }

        public string BuyValueTotalAsStr => Helper.FormatDecimal(BuyValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);

        public string BuyValueTotalAsStrUnit => Helper.FormatDecimal(BuyValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyValueReductionTotal { get; internal set; }

        public string BuyValueReductionTotalAsStr => Helper.FormatDecimal(BuyValueReductionTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);

        public decimal BuyValueReductionBrokerageTotal { get; internal set; }

        public string BuyValueReductionBrokerageTotalAsStr => Helper.FormatDecimal(BuyValueReductionBrokerageTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength);

        public string BuyValueReductionBrokerageTotalAsStrUnit => Helper.FormatDecimal(BuyValueReductionBrokerageTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        public decimal BuyVolumeTotal { get; internal set; }

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
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDate">Date and time of the buy</param>
        /// <param name="decVolume">Volume of the buy</param>
        /// <param name="decVolumeSold">Volmue of the buy which is already sold</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="decReduction">Reduction value of the buy</param>
        /// <param name="decBrokerage">Brokerage of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strGuid, string strDate, decimal decVolume, decimal decVolumeSold, decimal decSharePrice, decimal decReduction, decimal decBrokerage, string strDoc = "")
        {
#if DEBUG_BUY
            Console.WriteLine(@"");
            Console.WriteLine(@"Add AllBuysOfTheShare");
#endif
            try
            {
                // Get year of the date of the new buy
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a buy for the given year already exists if not add it
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddBuyObject(BuyCultureInfo, strGuid, strDate, decVolume, decVolumeSold, decSharePrice, decReduction, decBrokerage, strDoc))
                        return false;
                }
                else
                {
                    // Add new year buy object for the buy with a new year
                    var addObject = new BuysYearOfTheShare();
                    // Add buy with the new year to the buy year list
                    if (addObject.AddBuyObject(BuyCultureInfo, strGuid, strDate, decVolume, decVolumeSold, decSharePrice, decReduction, decBrokerage, strDoc))
                    {
                        AllBuysOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total buy value and total buy volume
                // Reset total buy value and buy volume
                BuyValueTotal = 0;
                BuyValueReductionTotal = 0;
                BuyValueReductionBrokerageTotal = 0;
                BuyVolumeTotal = 0;

                // Calculate the new total buy value and buy volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyValueTotal += calcObject.BuyValueYear;
                    BuyValueReductionTotal += calcObject.BuyValueReductionYear;
                    BuyValueReductionBrokerageTotal += calcObject.BuyValueReductionBrokerageYear;
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                }

#if DEBUG_BUY
                Console.WriteLine(@"BuyValueTotal:{0}", BuyValueTotal);
                Console.WriteLine(@"BuyValueReductionTotal:{0}", BuyValueReductionTotal);
                Console.WriteLine(@"BuyValueReductionBrokerageTotal:{0}", BuyValueReductionBrokerageTotal);
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
        /// This function remove a buy with the given Guid and date and time
        /// from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be removed</param>
        /// <param name="strDate">Date and time of the buy which should be removed</param>
        /// <returns></returns>
        public bool RemoveBuy(string strGuid, string strDate)
        {
#if DEBUG_BUY
            Console.WriteLine(@"Remove AllBuysOfTheShare");
#endif
            try
            {
                // Get year of the date of the buy which should be removed
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if the buy for the given year exists
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveBuyObject(strGuid))
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
                BuyValueReductionBrokerageTotal = 0;
                BuyValueReductionTotal = 0;
                BuyValueTotal = 0;
                BuyVolumeTotal = 0;

                // Calculate the new total buy value and volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyValueReductionBrokerageTotal += calcObject.BuyValueReductionBrokerageYear;
                    BuyValueReductionTotal += calcObject.BuyValueReductionYear;
                    BuyValueTotal += calcObject.BuyValueYear;
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                }

#if DEBUG_BUY
                Console.WriteLine(@"BuyValueTotal:{0}", BuyValueTotal);
                Console.WriteLine(@"BuyValueReductionTotal:{0}", BuyValueReductionTotal);
                Console.WriteLine(@"BuyValueReductionBrokerageTotal:{0}", BuyValueReductionBrokerageTotal);
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
        public List<BuysYearOfTheShare> GetAllBuysTotalValues()
        {
            var allBuysOfTheShare = new List<BuysYearOfTheShare>();

            foreach (var key in AllBuysOfTheShareDictionary.Keys)
            {
                allBuysOfTheShare.Add(AllBuysOfTheShareDictionary[key]);
            }
            return allBuysOfTheShare;
        }

        /// <summary>
        /// This function returns the buy object of the given date and time
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDate">Date and time of the buy</param>
        /// <returns>BuyObject or null if the search failed</returns>
        public BuyObject GetBuyObjectByGuidDate(string strGuid, string strDate)
        {
            GetYearOfDate(strDate, out var year);

            if (year == null) return null;

            if (!AllBuysOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var buyObject in AllBuysOfTheShareDictionary[year].BuyListYear)
            {
                if (buyObject.Guid == strGuid)
                {
                    return buyObject;
                }
            }

            return null;
        }

        /// <summary>
        /// This function adds a volume to the sold volume
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="decSaleVolume">Sale volume</param>
        /// <returns>Flag if the sale volume add has been successful</returns>
        public bool AddSaleVolumeByGuid(string strGuid, decimal decSaleVolume)
        {
            foreach (var buyObjectList in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buyObjectList.BuyListYear)
                {
                    if (buyObject.Guid != strGuid ) continue;

#if DEBUG_BUY
                    Console.WriteLine(@"Add sale volume");
                    Console.WriteLine(@"Guid: {0}", strGuid);
                    Console.WriteLine(@"decSaleVolume: {0}", decSaleVolume);
                    Console.WriteLine(@"VolumeSold: {0}", buyObject.VolumeSold);
                    Console.WriteLine(@"");
#endif
                    buyObject.VolumeSold += decSaleVolume;

#if DEBUG_BUY
                    Console.WriteLine(@"After add sale volume");
                    Console.WriteLine(@"Guid: {0}", strGuid);
                    Console.WriteLine(@"decSaleVolume: {0}", decSaleVolume);
                    Console.WriteLine(@"VolumeSold: {0}", buyObject.VolumeSold);
                    Console.WriteLine(@"");
#endif

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function removes a volume of the sold volume
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="decSaleVolume">Sale volume</param>
        /// <returns>Flag if the sale volume remove has been successful</returns>
        public bool RemoveSaleVolumeByGuid(string strGuid, decimal decSaleVolume)
        {
            foreach (var buyObjectList in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buyObjectList.BuyListYear)
                {
                    if (buyObject.Guid != strGuid) continue;

#if DEBUG_BUY
                    Console.WriteLine(@"Remove sale volume");
                    Console.WriteLine(@"Guid: {0}", strGuid);
                    Console.WriteLine(@"decSaleVolume: {0}", decSaleVolume);
                    Console.WriteLine(@"VolumeSold: {0}", buyObject.VolumeSold);
                    Console.WriteLine(@"");
#endif
                    if (decSaleVolume <= buyObject.VolumeSold)
                        buyObject.VolumeSold -= decSaleVolume;

#if DEBUG_BUY
                    Console.WriteLine(@"After remove sale volume");
                    Console.WriteLine(@"Guid: {0}", strGuid);
                    Console.WriteLine(@"decSaleVolume: {0}", decSaleVolume);
                    Console.WriteLine(@"VolumeSold: {0}", buyObject.VolumeSold);
                    Console.WriteLine(@"");
#endif
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function checks if the buy with the given Guid is the last buy of the entries
        /// </summary>
        /// <param name="strGuid">Given date and time</param>
        /// <returns></returns>
        public bool IsLastBuy(string strGuid)
        {
            if (_allBuysOfTheShareDictionary.Count <= 0) return false;

            var lastYearEntries = _allBuysOfTheShareDictionary.Last().Value;

            if (lastYearEntries.BuyListYear.Count <= 0) return false;

            if (lastYearEntries.BuyListYear.Last().Guid == strGuid)
                return true;

            return false;
        }

        /// <summary>
        /// This function checks if the buy is already part of a sale
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <returns>Flag if the buy is part of a sale.</returns>
        public bool IsPartOfASale(string strGuid)
        {
            if (_allBuysOfTheShareDictionary.Count <= 0) return false;

            foreach (var buysYearOfTheShare in _allBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buysYearOfTheShare.BuyListYear)
                {
                    if (buyObject.Guid != strGuid) continue;
                    return buyObject.VolumeSold != 0;
                }
            }

            return false;
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
