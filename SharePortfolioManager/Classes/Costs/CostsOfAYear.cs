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
using System.ComponentModel;
using System.Globalization;

namespace SharePortfolioManager.Classes.Costs
{
    [Serializable]
    public class BrokerageReductionYearOfTheShare
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo BrokerageReductionCultureInfo { get; internal set; }

        [Browsable(false)]
        public string BrokerageYear { get; internal set; } = @"-";

        [Browsable(false)]
        public decimal BrokerageValueYear { get; internal set; } = -1;

        [Browsable(false)]
        public string BrokerageValueYearWithUnitAsStr => Helper.FormatDecimal(BrokerageValueYear, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", BrokerageReductionCultureInfo);

        [Browsable(false)]
        public decimal BrokerageWithReductionValueYear { get; internal set; } = -1;

        [Browsable(false)]
        public string BrokerageWithReductionValueYearWithUnitAsStr => Helper.FormatDecimal(BrokerageWithReductionValueYear, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", BrokerageReductionCultureInfo);

        [Browsable(false)]
        public List<BrokerageReductionObject> BrokerageReductionListYear { get; } = new List<BrokerageReductionObject>();

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        public string DgvBrokerageYear => BrokerageYear;

        [Browsable(true)]
        public decimal DgvBrokerageWithReductionValueYear => BrokerageWithReductionValueYear;

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// This functions adds a new brokerage object to the year list with the given values
        /// It also recalculates the brokerage value
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="bPartOfABuy">Flag if the brokerage is a part of a share buy</param>
        /// <param name="bPartOfASale">Flag if the brokerage is a part of a share sale</param>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strGuidBuySale">Guid of the buy or sale</param>
        /// <param name="strDate">Pay date of the new brokerage list entry</param>
        /// <param name="decProvisionValue">Provision value</param>
        /// <param name="decBrokerFreeValue">Broker fee value</param>
        /// <param name="decTraderPlaceFeeValue">Trader place fee value</param>
        /// <param name="decReductionValue">Reduction value</param>
        /// <param name="strDoc">Document of the brokerage</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBrokerageReductionObject(string strGuid, bool bPartOfABuy, bool bPartOfASale, CultureInfo cultureInfo, string strGuidBuySale,
            string strDate, decimal decProvisionValue, decimal decBrokerFreeValue, decimal decTraderPlaceFeeValue, decimal decReductionValue, string strDoc = "")
        {
#if DEBUG_BROKERAGE
            Console.WriteLine(@"");
            Console.WriteLine(@"AddBrokerageReductionObject");
#endif
            try
            {
                // Set culture info of the share
                BrokerageReductionCultureInfo = cultureInfo;

                // Create new BrokerageObject
                var addObject = new BrokerageReductionObject(strGuid, bPartOfABuy, bPartOfASale, cultureInfo, strGuidBuySale, strDate, decProvisionValue, decBrokerFreeValue, decTraderPlaceFeeValue, decReductionValue, strDoc);

                // Add object to the list
                BrokerageReductionListYear.Add(addObject);
                BrokerageReductionListYear.Sort(new BrokerageReductionObjectComparer());

                // Set year
                DateTime.TryParse(strDate, out var dateTime);
                BrokerageYear = dateTime.Year.ToString();

                // Calculate brokerage value
                if (BrokerageValueYear == -1)
                    BrokerageValueYear = 0;
                BrokerageValueYear += addObject.BrokerageValue;

                // Calculate brokerage minus reduction value
                if (BrokerageWithReductionValueYear == -1)
                    BrokerageWithReductionValueYear = 0;
                BrokerageWithReductionValueYear += addObject.BrokerageReductionValue;

#if DEBUG_BROKERAGE
                Console.WriteLine(@"BrokerageValueYear: {0}", BrokerageValueYear);
                Console.WriteLine(@"BrokerageWithReductionValueYear: {0}", BrokerageWithReductionValueYear);
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
        public bool RemoveBrokerageReductionObject(string strGuid)
        {
#if DEBUG_BROKERAGE
            Console.WriteLine(@"RemoveBrokerageReductionObject");
#endif
            try
            {
                // Search for the remove object
                var iFoundIndex = -1;
                foreach (var brokerageReductionObject in BrokerageReductionListYear)
                {
                    if (brokerageReductionObject.Guid != strGuid) continue;

                    iFoundIndex = BrokerageReductionListYear.IndexOf(brokerageReductionObject);
                    break;
                }

                // Set remove object
                var removeObject = BrokerageReductionListYear[iFoundIndex];

                // Remove object from the list
                BrokerageReductionListYear.Remove(removeObject);

                // Calculate brokerage value
                BrokerageValueYear -= removeObject.BrokerageValue;

                // Calculate brokerage minus reduction value
                BrokerageWithReductionValueYear -= removeObject.BrokerageReductionValue;

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

    public class BrokerageWithReductionYearOfTheShareComparer : IComparer<BrokerageReductionYearOfTheShare>
    {
        public int Compare(BrokerageReductionYearOfTheShare object1, BrokerageReductionYearOfTheShare object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            if (Convert.ToInt16(object2.BrokerageReductionListYear) == Convert.ToInt16(object1.BrokerageReductionListYear))
                return 0;
            if (Convert.ToInt16(object2.BrokerageReductionListYear) > Convert.ToInt16(object1.BrokerageReductionListYear))
                return 1;
            
            return -1;
        }
    }
}
