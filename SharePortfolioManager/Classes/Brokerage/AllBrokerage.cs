//MIT License
//
//Copyright(c) 2017 - 2022 nessie1980(nessie1980@gmx.de)
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

// Define for DEBUGGING
//#define DEBUG_ALL_BROKERAGE

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharePortfolioManager.Classes.Brokerage
{
    [Serializable]
    public class AllBrokerageReductionOfTheShare
    {
        #region Properties

        public CultureInfo CultureInfo { get; internal set; }

        public decimal BrokerageValueTotal { get; internal set; }

        public string BrokerageValueTotalAsStr => Helper.FormatDecimal(BrokerageValueTotal, Helper.CurrencyFiveLength,
            false, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        public string BrokerageValueTotalAsStrUnit => Helper.FormatDecimal(BrokerageValueTotal,
            Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        public decimal ReductionValueTotal { get; internal set; }

        public string ReductionValueTotalAsStr => Helper.FormatDecimal(ReductionValueTotal, Helper.CurrencyFiveLength,
            false, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        public string ReductionValueTotalAsStrUnit => Helper.FormatDecimal(ReductionValueTotal,
            Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        public decimal BrokerageWithReductionValueTotal { get; internal set; }

        public string BrokerageWithReductionValueTotalAsStr => Helper.FormatDecimal(BrokerageWithReductionValueTotal,
            Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        public string BrokerageWithReductionValueTotalAsStrUnit => Helper.FormatDecimal(
            BrokerageWithReductionValueTotal, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"",
            CultureInfo);

        public SortedDictionary<string, BrokerageReductionYearOfTheShare>
            AllBrokerageReductionOfTheShareDictionary { get; } =
            new SortedDictionary<string, BrokerageReductionYearOfTheShare>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function sets the culture info to the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a brokerage to the list
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="bPartOfABuy">Flag if the brokerage is part of a buy</param>
        /// <param name="bPartOfASale">Flag if the brokerage is part of a Sale</param>
        /// <param name="strGuidBuySale">Guid of the buy or sale</param>
        /// <param name="strDateTime">Date and time of the brokerage</param>
        /// <param name="decProvisionValue">Provision value</param>
        /// <param name="decBrokerFeeValue">Broker fee value</param>
        /// <param name="decTraderPlaceFeeValue">Trader place fee value</param>
        /// <param name="decReductionValue">Reduction value</param>
        /// <param name="strDoc">Document of the brokerage</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBrokerageReduction(string strGuid, bool bPartOfABuy, bool bPartOfASale, string strGuidBuySale,
            string strDateTime, decimal decProvisionValue, decimal decBrokerFeeValue, decimal decTraderPlaceFeeValue, decimal decReductionValue, string strDoc = "")
        {
#if DEBUG_ALL_BROKERAGE
            Console.WriteLine(@"AddBrokerageReduction()");
#endif
            try
            {
                // Get year of the date of the new brokerage
                GetYearOfDate(strDateTime, out var year);
                if (year == null)
                    return false;

                // Search if a brokerage for the given year already exists if not add it
                if (AllBrokerageReductionOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddBrokerageReductionObject(strGuid, bPartOfABuy, bPartOfASale, CultureInfo, strGuidBuySale,
                        strDateTime, decProvisionValue, decBrokerFeeValue, decTraderPlaceFeeValue, decReductionValue, strDoc))
                        return false;
                }
                else
                {
                    // Add new year brokerage object for the brokerage with a new year
                    var addObject = new BrokerageReductionYearOfTheShare();
                    // Add brokerage with the new year to the brokerage year list
                    if (addObject.AddBrokerageReductionObject(strGuid, bPartOfABuy, bPartOfASale, CultureInfo, strGuidBuySale,
                        strDateTime, decProvisionValue, decBrokerFeeValue, decTraderPlaceFeeValue, decReductionValue, strDoc))
                    {
                        AllBrokerageReductionOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total brokerage and reduction values
                // Reset values
                BrokerageValueTotal = 0;
                ReductionValueTotal = 0;
                BrokerageWithReductionValueTotal = 0;

                // Calculate the new total brokerage value
                foreach (var calcObject in AllBrokerageReductionOfTheShareDictionary.Values)
                {
                    BrokerageValueTotal += calcObject.BrokerageValueYear;
                    ReductionValueTotal += calcObject.ReductionValueYear;
                    BrokerageWithReductionValueTotal += calcObject.BrokerageWithReductionValueYear;
                }
#if DEBUG_ALL_BROKERAGE
                Console.WriteLine(@"BrokerageValueTotal:{0}", BrokerageValueTotal);
                Console.WriteLine(@"ReductionValueTotal:{0}", ReductionValueTotal);
                Console.WriteLine(@"BrokerageWithReductionValueTotal:{0}", BrokerageWithReductionValueTotal);
#endif
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

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
        public bool RemoveBrokerageReduction(string strGuid, string strDate)
        {
#if DEBUG_ALL_BROKERAGE
            Console.WriteLine(@"RemoveBrokerageReduction()");
#endif
            try
            {
                // Get year of the date of the new brokerage
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a brokerage for the given year already exists if not no remove will be made
                if (AllBrokerageReductionOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveBrokerageReductionObject(strGuid))
                        return false;

                    if (searchObject.BrokerageReductionListYear.Count == 0)
                    {
                        if (!AllBrokerageReductionOfTheShareDictionary.Remove(year))
                            return false;
                    }
                }
                else
                {
                    return false;
                }

                // Calculate the total brokerage and reduction values
                // Reset values
                BrokerageValueTotal = 0;
                ReductionValueTotal = 0;
                BrokerageWithReductionValueTotal = 0;

                // Calculate the new total brokerage value
                foreach (var calcObject in AllBrokerageReductionOfTheShareDictionary.Values)
                {
                    BrokerageValueTotal += calcObject.BrokerageValueYear;
                    ReductionValueTotal += calcObject.ReductionValueYear;
                    BrokerageWithReductionValueTotal += calcObject.BrokerageWithReductionValueYear;
                }
#if DEBUG_ALL_BROKERAGE
                Console.WriteLine(@"BrokerageValueTotal:{0}", BrokerageValueTotal);
                Console.WriteLine(@"ReductionValueTotal:{0}", ReductionValueTotal);
                Console.WriteLine(@"BrokerageWithReductionValueTotal:{0}", BrokerageWithReductionValueTotal);
#endif
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function creates a list of all brokerage objects of the share
        /// </summary>
        /// <returns>List of BrokerageObjects or a empty list if no BrokerageObjects exists</returns>
        public List<BrokerageReductionObject> GetAllBrokerageOfTheShare()
        {
            var allBrokerageOfTheShare = new List<BrokerageReductionObject>();

            foreach(var brokerageYearOfTheShareObject in AllBrokerageReductionOfTheShareDictionary.Values)
            {
                foreach(var brokerageObject in brokerageYearOfTheShareObject.BrokerageReductionListYear)
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
        public List<BrokerageReductionYearOfTheShare> GetAllBrokerageTotalValues()
        {
            var allBrokerageOfTheShare = new List<BrokerageReductionYearOfTheShare>();

            foreach (var key in AllBrokerageReductionOfTheShareDictionary.Keys)
            {
                allBrokerageOfTheShare.Add(AllBrokerageReductionOfTheShareDictionary[key]);
            }
            return allBrokerageOfTheShare;
        }

        /// <summary>
        /// This function returns the brokerage object of the given brokerage GUID and date and time
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <returns>BrokerageObject or null if the search failed</returns>
        public BrokerageReductionObject GetBrokerageObjectByGuidDate(string strGuid, string strDateTime)
        {
            GetYearOfDate(strDateTime, out var year);

            if (year == null) return null;

            if (!AllBrokerageReductionOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var brokerageObject in AllBrokerageReductionOfTheShareDictionary[year].BrokerageReductionListYear)
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
        public BrokerageReductionObject GetBrokerageObjectByBuyGuid(string strGuid, string strDateTime)
        {
            GetYearOfDate(strDateTime, out var year);

            if (year == null) return null;

            if (!AllBrokerageReductionOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var brokerageObject in AllBrokerageReductionOfTheShareDictionary[year].BrokerageReductionListYear)
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
