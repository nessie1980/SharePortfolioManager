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
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public class AllDividendsOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the total dividend value of the share with taxes
        /// </summary>
        private decimal _dividendValueTotalWithTaxes = 0;

        ///// <summary>
        ///// Stores the total dividend percent value of the share
        ///// </summary>
        //private decimal _dividendPercentValueTotal = -1;

        /// <summary>
        /// Stores the dividends of a year of the share
        /// </summary>
        private SortedDictionary<string, DividendYearOfTheShare> _allDividendsOfTheShareDictionary = new SortedDictionary<string, DividendYearOfTheShare>();

        #endregion Variables

        #region Properties

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            internal set { _cultureInfo = value; }
        }

        public decimal DividendValueTotalWithTaxes
        {
            get { return _dividendValueTotalWithTaxes; }
            internal set { _dividendValueTotalWithTaxes = value; }
        }

        public string DividendValueTotalWithTaxesAsString
        {
            get { return Helper.FormatDecimal(_dividendValueTotalWithTaxes, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        public string DividendValueTotalWithTaxesWithUnitAsString
        {
            get { return Helper.FormatDecimal(_dividendValueTotalWithTaxes, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        public SortedDictionary<string, DividendYearOfTheShare> AllDividendsOfTheShareDictionary
        {
            get { return _allDividendsOfTheShareDictionary; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        public AllDividendsOfTheShare()
        {
        }

        /// <summary>
        /// This function sets the culture info to the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a dividend to the list of the dividends of the share
        /// </summary>
        /// <param name="cultureInfoFC">Culture info of the payout for the foreign currency</param>
        /// <param name="csEnableFC">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency</param>
        /// <param name="strDateTime">Pay date of the new dividend list entry</param>
        /// <param name="decRate">Paid dividend of one share</param>
        /// <param name="decVolume">Share volume at the pay date</param>
        /// <param name="decTaxAtSource">Tax at source value</param>
        /// <param name="decCapitalGainsTax">Capital gains tax value</param>
        /// <param name="decSolidarityTax">Solidarity tax value</param>
        /// <param name="decSharePrice">Share price at the pay date</param>
        /// <param name="strDoc">Document of the dividend</param>
        /// <returns></returns>
        public bool AddDividend(CultureInfo cultureInfoFC, CheckState csEnableFC, decimal decExchangeRatio, string strDateTime, decimal decRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax, decimal decSharePrice, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"Add DividendYearOfTheShare");
#endif
            try
            {
                // Get year of the date of the new dividend
                string year = "";
                GetYearOfDate(strDateTime, out year);
                if (year == null)
                    return false;

                // Search if a dividend for the given year already exists if not add it
                DividendYearOfTheShare searchObject;
                if (AllDividendsOfTheShareDictionary.TryGetValue(year, out searchObject))
                {
                    if (!searchObject.AddDividendObject(CultureInfo, cultureInfoFC, csEnableFC, decExchangeRatio, strDateTime, decRate, decVolume,
                        decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc))
                        return false;
                }
                else
                {
                    // Add new year dividend object for the dividend with a new year
                    DividendYearOfTheShare addObject = new DividendYearOfTheShare();
                    // Add dividend with the new year to the dividend year list
                    if (addObject.AddDividendObject(CultureInfo, cultureInfoFC, csEnableFC, decExchangeRatio, strDateTime, decRate, decVolume,
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
                //DividendPercentValueTotal = 0;

                //decimal tempDividendPercentValueTotalSum = 0;
                // Calculate the new total dividend value
                foreach (var calcObject in AllDividendsOfTheShareDictionary.Values)
                {
                    DividendValueTotalWithTaxes += calcObject.DividendValueYear;
                    //tempDividendPercentValueTotalSum += calcObject.DividendPercentValueYear;
                }
                //DividendPercentValueTotal = tempDividendPercentValueTotalSum / AllDividendOfTheShare.Count;

#if DEBUG
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
        /// <param name="payDate">Date of the dividend entry which should be removed</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveDividend(string payDate)
        {
            try
            {
                // Get year of the date of the new dividend
                string year = "";
                GetYearOfDate(payDate, out year);
                if (year == null)
                    return false;

                // Search if a dividend for the given year already exists if not no remove will be made
                DividendYearOfTheShare searchObject;
                if (AllDividendsOfTheShareDictionary.TryGetValue(year, out searchObject))
                {
                    if (!searchObject.RemoveDividendObject(payDate))
                        return false;
                    else
                    {
                        if (searchObject.DividendListYear.Count == 0)
                        {
                            if (!AllDividendsOfTheShareDictionary.Remove(year))
                                return false;
                        }
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
                    //tempDividendPercentValueTotalSum += calcObject.DividendPercentValueYear;
                }
                //DividendPercentValueTotal = tempDividendPercentValueTotalSum / AllDividendOfTheShare.Count;

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
            List<DividendObject> allDividendsOfTheShare = new List<DividendObject>();

            foreach(DividendYearOfTheShare dividendYearOfTheShareObject in AllDividendsOfTheShareDictionary.Values)
            {
                foreach(DividendObject dividendObject in dividendYearOfTheShareObject.DividendListYear)
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
        public Dictionary<string, string> GetAllDividendsTotalValues()
        {
            Dictionary<string, string> allDividendsOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllDividendsOfTheShareDictionary.Keys)
            {
                allDividendsOfTheShare.Add(key, string.Format(@"{0:N2}", AllDividendsOfTheShareDictionary[key].DividendValueYear));
            }
            return allDividendsOfTheShare;
        }

        /// <summary>
        /// This function returns the dividend object of the given date and time
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <returns>DividendObject or null if the search failed</returns>
        public DividendObject GetDividendObjectByDateTime(string strDateTime)
        {
            string strYear = @"";
            GetYearOfDate(strDateTime, out strYear);

            if (strYear != null)
            {
                if (AllDividendsOfTheShareDictionary.ContainsKey(strYear))
                {
                    foreach (var dividendObject in AllDividendsOfTheShareDictionary[strYear].DividendListYear)
                    {
                        if (dividendObject.DateTime == strDateTime)
                        {
                            return dividendObject;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// This function tries to the year of the given date string
        /// </summary>
        /// <param name="date">Date string (DD.MM.YYYY)</param>
        /// <param name="year">Year of the given date or null if the split failed</param>
        private void GetYearOfDate(string date, out string year)
        {
            string[] dateTimeElements = date.Split('.');
            if (dateTimeElements.Length != 3)
            {
                year = null;
            }
            else
            {
                string yearTime = dateTimeElements.Last();
                string[] dateElements = yearTime.Split(' ');
                year = dateElements.First();
            }
        }

        #endregion Methods
    }
}
