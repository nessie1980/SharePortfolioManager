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

namespace SharePortfolioManager
{
    public class AllCostsOfTheShare
    {
        #region Properties

        public CultureInfo CostCultureInfo { get; internal set; }

        public decimal CostValueTotal { get; internal set; }

        public string CostValueTotalAsStr => Helper.FormatDecimal(CostValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CostCultureInfo);

        public string CostValueTotalWithUnitAsStr => Helper.FormatDecimal(CostValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CostCultureInfo);

        public SortedDictionary<string, CostYearOfTheShare> AllCostsOfTheShareDictionary { get; } = new SortedDictionary<string, CostYearOfTheShare>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function sets the culture info to the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            CostCultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a cost to the list
        /// </summary>
        /// <param name="bCostOfABuy">Flag if the cost is part of a buy</param>
        /// <param name="bCostOfASale">Flag if the cost is part of a Sale</param>
        /// <param name="strDateTime">Date and time of the costs</param>
        /// <param name="decValue">Value of the costs</param>
        /// <param name="strDoc">Document of the costs</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddCost(bool bCostOfABuy, bool bCostOfASale, string strDateTime, decimal decValue, string strDoc = "")
        {
#if DEBUG_COST
            Console.WriteLine(@"Add CostYearOfTheShare");
#endif
            try
            {
                // Get year of the date of the new cost
                GetYearOfDate(strDateTime, out var year);
                if (year == null)
                    return false;

                // Search if a cost for the given year already exists if not add it
                if (AllCostsOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddCostObject(bCostOfABuy, bCostOfASale, CostCultureInfo, strDateTime, decValue, strDoc))
                        return false;
                }
                else
                {
                    // Add new year cost object for the cost with a new year
                    var addObject = new CostYearOfTheShare();
                    // Add cost with the new year to the cost year list
                    if (addObject.AddCostObject(bCostOfABuy, bCostOfASale, CostCultureInfo, strDateTime, decValue, strDoc))
                    {
                        AllCostsOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }
                
                // Calculate the total cost values
                // Reset total cost values
                CostValueTotal = 0;

                // Calculate the new total cost value
                foreach (var calcObject in AllCostsOfTheShareDictionary.Values)
                {
                    CostValueTotal += calcObject.CostValueYear;
                }
#if DEBUG_COST
                Console.WriteLine(@"CostValueTotal:{0}", CostValueTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function removes a cost from the cost lists by the given date
        /// </summary>
        /// <param name="strDate">Date of the cost entry which should be removed</param>
        /// <returns></returns>
        public bool RemoveCost(string strDate)
        {
#if DEBUG_COST
            Console.WriteLine(@"Remove CostYearOfTheShare");
#endif
            try
            {
                // Get year of the date of the new cost
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a cost for the given year already exists if not no remove will be made
                if (AllCostsOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveCostObject(strDate))
                        return false;

                    if (searchObject.CostListYear.Count == 0)
                    {
                        if (!AllCostsOfTheShareDictionary.Remove(year))
                            return false;
                    }
                }
                else
                {
                    return false;
                }

                // Calculate the total cost values
                // Reset total cost values
                CostValueTotal = 0;

                // Calculate the new total cost value
                foreach (var calcObject in AllCostsOfTheShareDictionary.Values)
                {
                    CostValueTotal += calcObject.CostValueYear;
                }

#if DEBUG_COST
                Console.WriteLine(@"CostValueTotal:{0}", CostValueTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function creates a list of all cost objects of the share
        /// </summary>
        /// <returns>List of CostObjects or a empty list if no CostObjects exists</returns>
        public List<CostObject> GetAllCostsOfTheShare()
        {
            var allCostsOfTheShare = new List<CostObject>();

            foreach(var costYearOfTheShareObject in AllCostsOfTheShareDictionary.Values)
            {
                foreach(var costObject in costYearOfTheShareObject.CostListYear)
                {
                    allCostsOfTheShare.Add(costObject);
                }
            }
            return allCostsOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total costs of the years
        /// </summary>
        /// <returns>Dictionary with the years and the costs values of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllCostsTotalValues()
        {
            var allCostsOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllCostsOfTheShareDictionary.Keys)
            {
                allCostsOfTheShare.Add(key, Helper.FormatDecimal(AllCostsOfTheShareDictionary[key].CostValueYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", CostCultureInfo));
            }
            return allCostsOfTheShare;
        }

        /// <summary>
        /// This function returns the cost object of the given date and time
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <returns>CostObject or null if the search failed</returns>
        public CostObject GetCostObjectByDateTime(string strDateTime)
        {
            GetYearOfDate(strDateTime, out var year);

            if (year == null) return null;

            if (!AllCostsOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var costObject in AllCostsOfTheShareDictionary[year].CostListYear)
            {
                if (costObject.CostDate == strDateTime)
                {
                    return costObject;
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
