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

namespace SharePortfolioManager
{
    public class CostYearOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the paid cost value in a year
        /// </summary>
        private decimal _costValueYear = -1;

        /// <summary>
        /// Stores the single paid costs of a year
        /// </summary>
        private List<CostObject> _costListYear = new List<CostObject>();

        #endregion Variables

        #region Properties

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            internal set { _cultureInfo = value; }
        }

        public decimal CostValueYear
        {
            get { return _costValueYear; }
            internal set { _costValueYear = value; }
        }

        public string CostValueYearAsString
        {
            get { return Helper.FormatDecimal(_costValueYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        public string CostValueYearWithUnitAsString
        {
            get { return Helper.FormatDecimal(_costValueYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        public List<CostObject> CostListYear
        {
            get { return _costListYear; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        public CostYearOfTheShare()
        { }

        /// <summary>
        /// This functions adds a new cost object to the year list with the given values
        /// It also recalculates the cost value
        /// </summary>
        /// <param name="bCostOfABuy">Flag if the cost is a part of a share buy</param>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strDate">Pay date of the new cost list entry</param>
        /// <param name="decValue">Paid cost value</param>
        /// <param name="strDoc">Document of the cost</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddCostObject(bool bCostOfABuy, CultureInfo cultureInfo, string strDate, decimal decValue, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"AddCostObject");
#endif
            try
            {
                // Set culture info of the share
                CultureInfo = cultureInfo;

                // Create new CostObject
                CostObject addObject = new CostObject(bCostOfABuy, cultureInfo, strDate, decValue, strDoc);

                // Add object to the list
                CostListYear.Add(addObject);
                CostListYear.Sort(new CostObjectComparer());

                // Calculate cost value
                if (CostValueYear == -1)
                    CostValueYear = 0;
                CostValueYear += addObject.CostValue;

#if DEBUG
                Console.WriteLine(@"CostValueYear: {0}", CostValueYear);
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
        /// This function removes the cost object with the given pay date from the list
        /// It also recalculates the cost value
        /// </summary>
        /// <param name="strDate">Pay date of the cost object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveCostObject(string strDate)
        {
#if DEBUG
            Console.WriteLine(@"RemoveCostObject");
#endif
            try
            {
                // Search for the remove object
                int iFoundIndex = -1;
                foreach (CostObject costObject in CostListYear)
                {
                    if (costObject.CostDate == strDate)
                    {
                        iFoundIndex = CostListYear.IndexOf(costObject);
                        break;
                    }
                }
                // Save remove object
                CostObject removeObject = CostListYear[iFoundIndex];

                // Remove object from the list
                CostListYear.Remove(removeObject);

                // Calculate cost value
                CostValueYear -= removeObject.CostValue;

#if DEBUG
                Console.WriteLine(@"CostValueYear: {0}", CostValueYear);
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

    public class CostYearOfTheShareComparer : IComparer<CostYearOfTheShare>
    {
        public int Compare(CostYearOfTheShare object1, CostYearOfTheShare object2)
        {
            if (Convert.ToInt16(object2.CostListYear) == Convert.ToInt16(object1.CostListYear))
                return 0;
            else if (Convert.ToInt16(object2.CostListYear) > Convert.ToInt16(object1.CostListYear))
                return 1;
            else 
                return -1;
        }
    }
}
