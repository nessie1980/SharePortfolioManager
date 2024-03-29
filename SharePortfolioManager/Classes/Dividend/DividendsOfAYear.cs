﻿//MIT License
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
//#define DEBUG_DIVIDEND_YEARS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace SharePortfolioManager.Classes.Dividend
{
    [Serializable]
    public class DividendYearOfTheShare
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo DividendCultureInfo { get; internal set; }

        [Browsable(false)]
        public string DividendYearAsStr { get; internal set; } = @"-";

        [Browsable(false)]
        public decimal DividendValueYear { get; internal set; } = -1;

        /// HINT: If any format is changed here it must also be changed in file "ViewDividendEdit.cs" at the variable "payoutFormatted"
        /// in the function "OnDataGridViewDividends_DataBindingComplete"
        [Browsable(false)]
        public string DividendValueYearAsStrUnit => DividendValueYear > 0
            ? Helper.FormatDecimal(DividendValueYear, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength, true, @"", DividendCultureInfo)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", DividendCultureInfo);

        [Browsable(false)]
        public List<DividendObject> DividendListYear { get; } = new List<DividendObject>();

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Year")]
        // ReSharper disable once UnusedMember.Global
        public string DgvDividendYear => DividendYearAsStr;

        [Browsable(true)]
        [DisplayName(@"Dividend")]
        public string DgvDividendValueYearAsStrUnit => DividendValueYearAsStrUnit;

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// This functions adds a new dividend object to the year list with the given values
        /// It also recalculates the dividend value and the dividend percent value of the hole year
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="cultureInfoFc">Culture info of the share for the foreign currency</param>
        /// <param name="csEnableFc">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency</param>
        /// <param name="strGuid">Guid of the dividend</param>
        /// <param name="strDate">Pay date of the new dividend list entry</param>
        /// <param name="decRate">Paid dividend of one share</param>
        /// <param name="decVolume">Share volume at the pay date</param>
        /// <param name="decTaxAtSource">Tax at source value</param>
        /// <param name="decCapitalGainsTax">Capital gains tax value</param>
        /// <param name="decSolidarityTax">Solidarity tax value</param>
        /// <param name="decSharePrice">Share price at the pay date</param>
        /// <param name="strDoc">Document of the dividend</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddDividendObject(CultureInfo cultureInfo, CultureInfo cultureInfoFc, CheckState csEnableFc, decimal decExchangeRatio, string strGuid, string strDate, decimal decRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax, decimal decSharePrice, string strDoc = "")
        {
#if DEBUG_DIVIDEND_YEARS
            Console.WriteLine(@"AddDividendObject");
#endif
            try
            {
                // Set culture info of the share
                DividendCultureInfo = cultureInfo;

                // Create new DividendObject
                var addObject = new DividendObject(cultureInfo, cultureInfoFc, csEnableFc, decExchangeRatio, strGuid, strDate, decRate, decVolume,
                    decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc);

                // Add object to the list
                DividendListYear.Add(addObject);
                DividendListYear.Sort(new DividendObjectComparer());

                // Set year
                DateTime.TryParse(strDate, out var dateTime);
                DividendYearAsStr = dateTime.Year.ToString();

                // Calculate dividend value
                if (DividendValueYear == -1)
                    DividendValueYear = 0;

                DividendValueYear += addObject.DividendPayoutWithTaxes;
#if DEBUG_DIVIDEND_YEARS
                Console.WriteLine(@"DividendValueYear: {0}", DividendValueYear);
                Console.WriteLine(@"");
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
        /// This function removes the dividend object with the given pay date from the list
        /// It also recalculates the dividend value and the dividend percent value of the hole year
        /// </summary>
        /// <param name="strGuid">Guid of the dividend object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveDividendObject(string strGuid)
        {
#if DEBUG_DIVIDEND_YEARS
            Console.WriteLine(@"RemoveDividendObject");
#endif
            try
            {
                // Search for the remove object
                var iFoundIndex = -1;
                foreach (var dividendObject in DividendListYear)
                {
                    if (dividendObject.Guid != strGuid) continue;

                    iFoundIndex =  DividendListYear.IndexOf(dividendObject);
                    break;
                }
                // Save remove object
                var removeObject = DividendListYear[iFoundIndex];

                // Remove object from the list
                DividendListYear.Remove(removeObject);

                // Calculate dividend value
                DividendValueYear -= removeObject.DividendPayoutWithTaxes;
#if DEBUG_DIVIDEND_YEARS
                Console.WriteLine(@"DividendValueYear: {0}", DividendValueYear);
                Console.WriteLine(@"");
#endif
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

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
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            if (Convert.ToInt16(object2.DividendListYear) == Convert.ToInt16(object1.DividendListYear))
                return 0;
            if (Convert.ToInt16(object2.DividendListYear) > Convert.ToInt16(object1.DividendListYear))
                return 1;
            
            return -1;
        }
    }
}
