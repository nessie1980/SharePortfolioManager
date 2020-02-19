//MIT License
//
//Copyright(c) 2020 nessie1980(nessie1980 @gmx.de)
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
using System.Windows.Forms;

namespace SharePortfolioManager.Classes.Dividend
{
    [Serializable]
    public class AllDividendsOfTheShare
    {
        #region Properties

        public CultureInfo DividendCultureInfo { get; internal set; }

        public decimal DividendValueTotalWithTaxes { get; internal set; }

        public string DividendValueTotalWithTaxesAsStr => Helper.FormatDecimal(DividendValueTotalWithTaxes, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", DividendCultureInfo);

        public string DividendValueTotalWithTaxesWithUnitAsStr => Helper.FormatDecimal(DividendValueTotalWithTaxes, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", DividendCultureInfo);

        public SortedDictionary<string, DividendYearOfTheShare> AllDividendsOfTheShareDictionary { get; } = new SortedDictionary<string, DividendYearOfTheShare>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function sets the culture info to the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            DividendCultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a dividend to the list of the dividends of the share
        /// </summary>
        /// <param name="cultureInfoFc">Culture info of the payout for the foreign currency</param>
        /// <param name="csEnableFc">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency</param>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDate">Pay date of the new dividend list entry</param>
        /// <param name="decRate">Paid dividend of one share</param>
        /// <param name="decVolume">Share volume at the pay date</param>
        /// <param name="decTaxAtSource">Tax at source value</param>
        /// <param name="decCapitalGainsTax">Capital gains tax value</param>
        /// <param name="decSolidarityTax">Solidarity tax value</param>
        /// <param name="decSharePrice">Share price at the pay date</param>
        /// <param name="strDoc">Document of the dividend</param>
        /// <returns></returns>
        public bool AddDividend(CultureInfo cultureInfoFc, CheckState csEnableFc, decimal decExchangeRatio, string strGuid, string strDate, decimal decRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax, decimal decSharePrice, string strDoc = "")
        {
#if DEBUG_DIVIDEND
            Console.WriteLine(@"Add DividendYearOfTheShare");
#endif
            try
            {
                // Get year of the date of the new dividend
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a dividend for the given year already exists if not add it
                if (AllDividendsOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddDividendObject(DividendCultureInfo, cultureInfoFc, csEnableFc, decExchangeRatio, strGuid, strDate, decRate, decVolume,
                        decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc))
                        return false;
                }
                else
                {
                    // Add new year dividend object for the dividend with a new year
                    var addObject = new DividendYearOfTheShare();
                    // Add dividend with the new year to the dividend year list
                    if (addObject.AddDividendObject(DividendCultureInfo, cultureInfoFc, csEnableFc, decExchangeRatio, strGuid, strDate, decRate, decVolume,
                        decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc))
                    {
                        AllDividendsOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }
                
                // Calculate the total dividend values
                // Reset total dividend values
                DividendValueTotalWithTaxes = 0;

                //decimal tempDividendPercentValueTotalSum = 0;
                // Calculate the new total dividend value
                foreach (var calcObject in AllDividendsOfTheShareDictionary.Values)
                {
                    DividendValueTotalWithTaxes += calcObject.DividendValueYear;
                }
#if DEBUG_DIVIDEND
                Console.WriteLine(@"DividendValueTotalWithTaxes:{0}", DividendValueTotalWithTaxes);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function removes a dividend entry from the list by the given date
        /// </summary>
        /// <param name="strGuid">Guid of the dividend which should be removed</param>
        /// <param name="strDate">Date of the dividend entry which should be removed</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveDividend(string strGuid, string strDate)
        {
            try
            {
                // Get year of the date of the new dividend
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a dividend for the given year already exists if not no remove will be made
                if (AllDividendsOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveDividendObject(strGuid))
                        return false;
                    if (searchObject.DividendListYear.Count == 0)
                    {
                        if (!AllDividendsOfTheShareDictionary.Remove(year))
                            return false;
                    }
                }
                else
                {
                    return false;
                }

                // Calculate the total dividend values
                // Reset total dividend values
                DividendValueTotalWithTaxes = 0;
                //DividendPercentValueTotal = 0;

                //decimal tempDividendPercentValueTotalSum = 0;
                // Calculate the new total dividend value
                foreach (var calcObject in AllDividendsOfTheShareDictionary.Values)
                {
                    DividendValueTotalWithTaxes += calcObject.DividendValueYear;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function creates a list of all dividend objects of the share
        /// </summary>
        /// <returns>List of DividendObjects or a empty list if no dividend objects exist</returns>
        public List<DividendObject> GetAllDividendsOfTheShare()
        {
            var allDividendsOfTheShare = new List<DividendObject>();

            foreach(var dividendYearOfTheShareObject in AllDividendsOfTheShareDictionary.Values)
            {
                foreach(var dividendObject in dividendYearOfTheShareObject.DividendListYear)
                {
                    allDividendsOfTheShare.Add(dividendObject);
                }
            }
            return allDividendsOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total dividend of the years
        /// </summary>
        /// <returns>Dictionary with the years and the dividend values of the year or empty dictionary if no year exist.</returns>
        public List<DividendYearOfTheShare> GetAllDividendsTotalValues()
        {
            var allDividendsOfTheShare = new List<DividendYearOfTheShare>();

            foreach (var key in AllDividendsOfTheShareDictionary.Keys)
            {
                allDividendsOfTheShare.Add(AllDividendsOfTheShareDictionary[key]);
            }
            return allDividendsOfTheShare;
        }

        /// <summary>
        /// This function returns the dividend object of the given date and time
        /// </summary>
        /// <param name="strGuid">Guid of the dividend</param>
        /// <param name="strDate">Date and time of the dividend</param>
        /// <returns>DividendObject or null if the search failed</returns>
        public DividendObject GetDividendObjectByGuidDate(string strGuid, string strDate)
        {
            GetYearOfDate(strDate, out var year);

            if (year == null) return null;

            if (!AllDividendsOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var dividendObject in AllDividendsOfTheShareDictionary[year].DividendListYear)
            {
                if (dividendObject.Guid == strGuid)
                {
                    return dividendObject;
                }
            }

            return null;
        }

        /// <summary>
        /// This function tries to the year of the given date string
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
