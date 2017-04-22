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
using SharePortfolioManager.Classes.Taxes;
using System.Globalization;
using SharePortfolioManager.Classes;

namespace SharePortfolioManager
{
    public class DividendYearOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

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

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            internal set { _cultureInfo = value; }
        }

        public decimal DividendValueYear
        {
            get { return _dividendValueYear; }
            internal set { _dividendValueYear = value; }
        }

        public string DividendValueYearAsString
        {
            get { return Helper.FormatDecimal(_dividendValueYear, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        public string DividendValueYearWithUnitAsString
        {
            get { return Helper.FormatDecimal(_dividendValueYear, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CultureInfo); }
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
        /// <param name="strDate">Pay date of the new dividend list entry</param>
        /// <param name="taxesValues">Taxes which must be paid</param>
        /// <param name="decDividendRate">Paid dividend of one share</param>
        /// <param name="decLossBalance">Loss balance of the share</param>
        /// <param name="decSharePrice">Share price at the pay date</param>
        /// <param name="decVolume">Share volume at the pay date</param>
        /// <param name="strDoc">Document of the dividend</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddDividendObject(CultureInfo cultureInfo, string strDate, Taxes taxesValues, decimal decDividendRate, decimal decLossBalance, decimal decSharePrice, decimal decVolume, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"AddDividendObject");
#endif
            try
            {
                // Set culture info of the share
                CultureInfo = cultureInfo;

                // Create new DividendObject
                DividendObject addObject = new DividendObject(cultureInfo, strDate, taxesValues, decDividendRate, decLossBalance, decSharePrice, decVolume, strDoc);

                // Add object to the list
                DividendListYear.Add(addObject);
                DividendListYear.Sort(new DividendObjectComparer());

                // Calculate dividend value
                if (DividendValueYear == -1)
                    DividendValueYear = 0;

                // TODO With or without taxes!!!
                //DividendValueYear += addObject.DividendPayOut;
                DividendValueYear += addObject.DividendPayOutWithTaxes;

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
                    if (dividendObject.DividendDate == strDate)
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
                DividendValueYear -= removeObject.DividendPayOutWithTaxes;

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
