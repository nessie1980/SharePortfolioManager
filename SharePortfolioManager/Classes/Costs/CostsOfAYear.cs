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
using System.ComponentModel;
using System.Globalization;

namespace SharePortfolioManager
{
    [Serializable]
    public class BrokerageYearOfTheShare
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo BrokerageCultureInfo { get; internal set; }

        [Browsable(true)]
        public string BrokerageYear { get; internal set; } = @"-";

        [Browsable(true)]
        public decimal BrokerageValueYear { get; internal set; } = -1;

        [Browsable(false)]
        public string BrokerageValueYearWithUnitAsStr => Helper.FormatDecimal(BrokerageValueYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BrokerageCultureInfo);

        [Browsable(false)]
        public List<BrokerageObject> BrokerageListYear { get; } = new List<BrokerageObject>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// This functions adds a new brokerage object to the year list with the given values
        /// It also recalculates the brokerage value
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="bBrokerageOfABuy">Flag if the brokerage is a part of a share buy</param>
        /// <param name="bBrokerageOfASale">Flag if the brokerage is a part of a share sale</param>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strGuidBuySale">Guid of the buy or sale</param>
        /// <param name="strDate">Pay date of the new brokerage list entry</param>
        /// <param name="decValue">Paid brokerage value</param>
        /// <param name="strDoc">Document of the brokerage</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBrokerageObject(string strGuid, bool bBrokerageOfABuy, bool bBrokerageOfASale, CultureInfo cultureInfo, string strGuidBuySale, string strDate, decimal decValue, string strDoc = "")
        {
#if DEBUG_BROKERAGE
            Console.WriteLine(@"");
            Console.WriteLine(@"AddBrokerageObject");
#endif
            try
            {
                // Set culture info of the share
                BrokerageCultureInfo = cultureInfo;

                // Create new BrokerageObject
                var addObject = new BrokerageObject(strGuid, bBrokerageOfABuy, bBrokerageOfASale, cultureInfo, strGuidBuySale, strDate, decValue, strDoc);

                // Add object to the list
                BrokerageListYear.Add(addObject);
                BrokerageListYear.Sort(new BrokerageObjectComparer());

                // Set year
                DateTime.TryParse(strDate, out var dateTime);
                BrokerageYear = dateTime.Year.ToString();

                // Calculate brokerage value
                if (BrokerageValueYear == -1)
                    BrokerageValueYear = 0;
                BrokerageValueYear += addObject.BrokerageValue;

#if DEBUG_BROKERAGE
                Console.WriteLine(@"BrokerageValueYear: {0}", BrokerageValueYear);
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
        /// This function removes the brokerage object with the given Guid from the list
        /// It also recalculates the brokerage value
        /// </summary>
        /// <param name="strGuid">Pay date of the brokerage object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveBrokerageObject(string strGuid)
        {
#if DEBUG_BROKERAGE
            Console.WriteLine(@"RemoveBrokerageObject");
#endif
            try
            {
                // Search for the remove object
                var iFoundIndex = -1;
                foreach (var brokerageObject in BrokerageListYear)
                {
                    if (brokerageObject.Guid != strGuid) continue;

                    iFoundIndex = BrokerageListYear.IndexOf(brokerageObject);
                    break;
                }
                // Save remove object
                var removeObject = BrokerageListYear[iFoundIndex];

                // Remove object from the list
                BrokerageListYear.Remove(removeObject);

                // Calculate brokerage value
                BrokerageValueYear -= removeObject.BrokerageValue;

#if DEBUG_BROKERAGE
                Console.WriteLine(@"BrokerageValueYear: {0}", BrokerageValueYear);
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

    public class BrokerageYearOfTheShareComparer : IComparer<BrokerageYearOfTheShare>
    {
        public int Compare(BrokerageYearOfTheShare object1, BrokerageYearOfTheShare object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            if (Convert.ToInt16(object2.BrokerageListYear) == Convert.ToInt16(object1.BrokerageListYear))
                return 0;
            if (Convert.ToInt16(object2.BrokerageListYear) > Convert.ToInt16(object1.BrokerageListYear))
                return 1;
            
            return -1;
        }
    }
}
