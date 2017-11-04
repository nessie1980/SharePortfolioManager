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
using SharePortfolioManager.Classes;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public class DividendYearOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _dividendCultureInfo;

        /// <summary>
        /// Stores the paid dividend value in a year
        /// </summary>
        private decimal _dividendValueYear = -1;

        ///// <summary>
        ///// Stores the percent value of the paid dividend in a year
        ///// </summary>
        //private decimal _dividendPercentValueYear = -1;

        /// <summary>
        /// Stores the single paid dividends of a year
        /// </summary>
        private List<DividendObject> _dividendListYear = new List<DividendObject>();

        #endregion Variables
        
        #region Properties

        public CultureInfo DividendCultureInfo
        {
            get { return _dividendCultureInfo; }
            internal set { _dividendCultureInfo = value; }
        }

        public decimal DividendValueYear
        {
            get { return _dividendValueYear; }
            internal set { _dividendValueYear = value; }
        }

        public string DividendValueYearAsStr
        {
            get { return Helper.FormatDecimal(_dividendValueYear, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", DividendCultureInfo); }
        }

        public string DividendValueYearWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_dividendValueYear, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", DividendCultureInfo); }
        }

        public List<DividendObject> DividendListYear
        {
            get { return _dividendListYear; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        public DividendYearOfTheShare()
        {
        }

        /// <summary>
        /// This functions adds a new dividend object to the year list with the given values
        /// It also recalculates the dividend value and the dividend percent value of the hole year
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="cultureInfoFC">Culture info of the share for the foreign currency</param>
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
        /// <returns>Flag if the add was successful</returns>
        public bool AddDividendObject(CultureInfo cultureInfo, CultureInfo cultureInfoFC, CheckState csEnableFC, decimal decExchangeRatio, string strDateTime, decimal decRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax, decimal decSharePrice, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"AddDividendObject");
#endif
            try
            {
                // Set culture info of the share
                DividendCultureInfo = cultureInfo;

                // Create new DividendObject
                DividendObject addObject = new DividendObject(cultureInfo, cultureInfoFC, csEnableFC, decExchangeRatio, strDateTime, decRate, decVolume,
                    decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc);

                // Add object to the list
                DividendListYear.Add(addObject);
                DividendListYear.Sort(new DividendObjectComparer());

                // Calculate dividend value
                if (DividendValueYear == -1)
                    DividendValueYear = 0;

                DividendValueYear += addObject.PayoutWithTaxesDec;
#if DEBUG
                Console.WriteLine(@"DividendValueYear: {0}", DividendValueYear);
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
        /// This function removes the dividend object with the given pay date from the list
        /// It also recalculates the dividend value and the dividend percent value of the hole year
        /// </summary>
        /// <param name="strDate">Pay date of the dividend object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveDividendObject(string strDate)
        {
#if DEBUG
            Console.WriteLine(@"RemoveDividendObject");
#endif
            try
            {
                // Search for the remove object
                int iFoundIndex = -1;
                foreach (DividendObject dividendObject in DividendListYear)
                {
                    if (dividendObject.DateTime == strDate)
                    {
                        iFoundIndex =  DividendListYear.IndexOf(dividendObject);
                        break;
                    }
                }
                // Save remove object
                DividendObject removeObject = DividendListYear[iFoundIndex];

                // Remove object from the list
                DividendListYear.Remove(removeObject);

                // Calculate dividend value
                DividendValueYear -= removeObject.PayoutWithTaxesDec;

                //// Calculate dividend percent value
                //decimal tempDividendPercentValueSum = 0;
                //foreach (DividendObject dividendObject in DividendListYear)
                //{
                //    tempDividendPercentValueSum += dividendObject.DividendPercent;
                //}
                //DividendPercentValueYear = tempDividendPercentValueSum / DividendListYear.Count;

#if DEBUG
                Console.WriteLine(@"DividendValueYear: {0}", DividendValueYear);
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

    public class DividendYearOfTheShareComparer : IComparer<DividendYearOfTheShare>
    {
        public int Compare(DividendYearOfTheShare object1, DividendYearOfTheShare object2)
        {
            if (Convert.ToInt16(object2.DividendListYear) == Convert.ToInt16(object1.DividendListYear))
                return 0;
            else if (Convert.ToInt16(object2.DividendListYear) > Convert.ToInt16(object1.DividendListYear))
                return 1;
            else 
                return -1;
        }
    }
}
