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
using SharePortfolioManager.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharePortfolioManager
{
    [Serializable]
    public class AllBrokerageOfTheShare
    {
        #region Properties

        public CultureInfo BrokerageCultureInfo { get; internal set; }

        public decimal BrokerageValueTotal { get; internal set; }

        public string BrokerageValueTotalAsStr => Helper.FormatDecimal(BrokerageValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BrokerageCultureInfo);

        public string BrokerageValueTotalWithUnitAsStr => Helper.FormatDecimal(BrokerageValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BrokerageCultureInfo);

        public SortedDictionary<string, BrokerageYearOfTheShare> AllBrokerageOfTheShareDictionary { get; } = new SortedDictionary<string, BrokerageYearOfTheShare>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function sets the culture info to the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            BrokerageCultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a brokerage to the list
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="bBrokerageOfABuy">Flag if the brokerage is part of a buy</param>
        /// <param name="bBrokerageOfASale">Flag if the brokerage is part of a Sale</param>
        /// <param name="strGuidBuySale">Guid of the buy or sale</param>
        /// <param name="strDateTime">Date and time of the brokerage</param>
        /// <param name="decValue">Value of the brokerage</param>
        /// <param name="strDoc">Document of the brokerage</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBrokerage(string strGuid, bool bBrokerageOfABuy, bool bBrokerageOfASale, string strGuidBuySale, string strDateTime, decimal decValue, string strDoc = "")
        {
#if DEBUG_BROKERAGE
            Console.WriteLine(@"Add BrokerageYearOfTheShare");
#endif
            try
            {
                // Get year of the date of the new brokerage
                GetYearOfDate(strDateTime, out var year);
                if (year == null)
                    return false;

                // Search if a brokerage for the given year already exists if not add it
                if (AllBrokerageOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddBrokerageObject(strGuid, bBrokerageOfABuy, bBrokerageOfASale, BrokerageCultureInfo, strGuidBuySale, strDateTime, decValue, strDoc))
                        return false;
                }
                else
                {
                    // Add new year brokerage object for the brokerage with a new year
                    var addObject = new BrokerageYearOfTheShare();
                    // Add brokerage with the new year to the brokerage year list
                    if (addObject.AddBrokerageObject(strGuid, bBrokerageOfABuy, bBrokerageOfASale, BrokerageCultureInfo, strGuidBuySale, strDateTime, decValue, strDoc))
                    {
                        AllBrokerageOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total brokerage values
                // Reset total brokerage values
                BrokerageValueTotal = 0;

                // Calculate the new total brokerage value
                foreach (var calcObject in AllBrokerageOfTheShareDictionary.Values)
                {
                    BrokerageValueTotal += calcObject.BrokerageValueYear;
                }
#if DEBUG_BROKERAGE
                Console.WriteLine(@"BrokerageValueTotal:{0}", BrokerageValueTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function removes a brokerage from the brokerage lists by the given date
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage which should be removed</param>
        /// <param name="strDate">Date of the brokerage entry which should be removed</param>
        /// <returns></returns>
        public bool RemoveBrokerage(string strGuid, string strDate)
        {
#if DEBUG_BROKERAGE
            Console.WriteLine(@"Remove BrokerageYearOfTheShare");
#endif
            try
            {
                // Get year of the date of the new brokerage
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a brokerage for the given year already exists if not no remove will be made
                if (AllBrokerageOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveBrokerageObject(strGuid))
                        return false;

                    if (searchObject.BrokerageListYear.Count == 0)
                    {
                        if (!AllBrokerageOfTheShareDictionary.Remove(year))
                            return false;
                    }
                }
                else
                {
                    return false;
                }

                // Calculate the total brokerage values
                // Reset total brokerage values
                BrokerageValueTotal = 0;

                // Calculate the new total brokerage value
                foreach (var calcObject in AllBrokerageOfTheShareDictionary.Values)
                {
                    BrokerageValueTotal += calcObject.BrokerageValueYear;
                }

#if DEBUG_BROKERAGE
                Console.WriteLine(@"BrokerageValueTotal:{0}", BrokerageValueTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function creates a list of all brokerage objects of the share
        /// </summary>
        /// <returns>List of BrokerageObjects or a empty list if no BrokerageObjects exists</returns>
        public List<BrokerageObject> GetAllBrokerageOfTheShare()
        {
            var allBrokerageOfTheShare = new List<BrokerageObject>();

            foreach(var brokerageYearOfTheShareObject in AllBrokerageOfTheShareDictionary.Values)
            {
                foreach(var brokerageObject in brokerageYearOfTheShareObject.BrokerageListYear)
                {
                    allBrokerageOfTheShare.Add(brokerageObject);
                }
            }
            return allBrokerageOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total brokerage of the years
        /// </summary>
        /// <returns>Dictionary with the years and the brokerage values of the year or empty dictionary if no year exist.</returns>
        public List<BrokerageYearOfTheShare> GetAllBrokerageTotalValues()
        {
            var allBrokerageOfTheShare = new List<BrokerageYearOfTheShare>();

            foreach (var key in AllBrokerageOfTheShareDictionary.Keys)
            {
                allBrokerageOfTheShare.Add(AllBrokerageOfTheShareDictionary[key]);
            }
            return allBrokerageOfTheShare;
        }

        /// <summary>
        /// This function returns the brokerage object of the given brokerage GUID and date and time
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <returns>BrokerageObject or null if the search failed</returns>
        public BrokerageObject GetBrokerageObjectByGuid(string strGuid, string strDateTime)
        {
            GetYearOfDate(strDateTime, out var year);

            if (year == null) return null;

            if (!AllBrokerageOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var brokerageObject in AllBrokerageOfTheShareDictionary[year].BrokerageListYear)
            {
                if (brokerageObject.Guid == strGuid)
                {
                    return brokerageObject;
                }
            }

            return null;
        }

        /// <summary>
        /// This function returns the brokerage object of the buy GUID and given date and time
        /// </summary>
        /// <param name="strGuid">Guid of the buy of the brokerage</param>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <returns>BrokerageObject or null if the search failed</returns>
        public BrokerageObject GetBrokerageObjectByBuyGuid(string strGuid, string strDateTime)
        {
            GetYearOfDate(strDateTime, out var year);

            if (year == null) return null;

            if (!AllBrokerageOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var brokerageObject in AllBrokerageOfTheShareDictionary[year].BrokerageListYear)
            {
                if (brokerageObject.GuidBuySale == strGuid)
                {
                    return brokerageObject;
                }
            }

            return null;
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
