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
using SharePortfolioManager.Classes.Buys;
using SharePortfolioManager.Classes.Costs;

// ReSharper disable once CheckNamespace
namespace SharePortfolioManager
{
    [Serializable]
    public class BuysYearOfTheShare
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo BuyCultureInfo { get; internal set; }

        [Browsable(false)]
        public string BuyYear { get; internal set; } = @"-";

        [Browsable(false)]
        public decimal BuyVolumeYear { get; internal set; } = -1;

        [Browsable(false)]
        public decimal BuyValueYear { get; internal set; } = -1;

        [Browsable(false)]
        public string BuyValueYearAsStrUnit => Helper.FormatDecimal(BuyValueYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal BuyValueBrokerageYear { get; internal set; } = -1;

        [Browsable(false)]
        public string BuyValueBrokerageYearAsStrUnit => Helper.FormatDecimal(BuyValueBrokerageYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo);

        [Browsable(false)]
        public List<BuyObject> BuyListYear { get; } = new List<BuyObject>();

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        // ReSharper disable once UnusedMember.Global
        public string DgvBuyYear => BuyYear;

        [Browsable(true)]
        // ReSharper disable once UnusedMember.Global
        public decimal DgvBuyVolumeYear => BuyVolumeYear;

        [Browsable(true)]
        // ReSharper disable once UnusedMember.Global
        public decimal DgvBuyValueBrokerageYear => BuyValueBrokerageYear;

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// This functions adds a new buy object to the year list with the given values
        /// It also recalculates the value of the bought shares in the year
        /// </summary>
        /// <param name="cultureInfo">Culture info of the buy</param>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strOrderNumber">Order number of the buy</param>
        /// <param name="strDate">Buy date of the new buy list entry</param>
        /// <param name="decVolume">Volume of the buy</param>
        /// <param name="decVolumeSold">Volume of the buy which is already sold</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="brokerageObject">Brokerage of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuyObject(CultureInfo cultureInfo, string strGuid, string strOrderNumber, string strDate, decimal decVolume, decimal decVolumeSold, decimal decSharePrice,
            BrokerageReductionObject brokerageObject, string strDoc = "")
        {
#if DEBUG_BUY
            Console.WriteLine(@"");
            Console.WriteLine(@"AddBuyObject");
#endif
            try
            {
                // Set culture info of the share
                BuyCultureInfo = cultureInfo;

                // Create new BuyObject
                var addObject = new BuyObject(cultureInfo, strGuid, strOrderNumber, strDate, decVolume, decVolumeSold, decSharePrice,
                    brokerageObject, strDoc);

                // Add object to the list
                BuyListYear.Add(addObject);
                BuyListYear.Sort(new BuyObjectComparer());

                // Set year
                DateTime.TryParse(strDate, out var dateTime);
                BuyYear = dateTime.Year.ToString();

                // Calculate buy value without reductions and brokerage
                if (BuyValueYear == -1)
                    BuyValueYear = 0;
                BuyValueYear += addObject.BuyValue;

                // Calculate buy value with brokerage
                if (BuyValueBrokerageYear == -1)
                    BuyValueBrokerageYear = 0;
                BuyValueBrokerageYear += addObject.BuyValueBrokerage;

                // Calculate buy volume
                if (BuyVolumeYear == -1)
                    BuyVolumeYear = 0;
                BuyVolumeYear += addObject.Volume;

#if DEBUG_BUY
                Console.WriteLine(@"BuyValueYear: {0}", BuyValueYear);
                Console.WriteLine(@"BuyValueReductionYear: {0}", BuyValueReductionYear);
                Console.WriteLine(@"BuyValueReductionBrokerageYear: {0}", BuyValueReductionBrokerageYear);
                Console.WriteLine(@"VolumeYear: {0}", BuyVolumeYear);
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
        /// This function removes the buy object with the given Guid from the list
        /// It also recalculates the buy value and volume
        /// </summary>
        /// <param name="strGuid">Guid of the buy object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveBuyObject(string strGuid)
        {
#if DEBUG_BUY
            Console.WriteLine(@"RemoveBuyObject");
#endif
            try
            {
                // Search for the remove object
                var iFoundIndex = -1;
                foreach (var buyObject in BuyListYear)
                {
                    if (buyObject.Guid != strGuid) continue;

                    iFoundIndex =  BuyListYear.IndexOf(buyObject);
                    break;
                }
                // Save remove object
                var removeObject = BuyListYear[iFoundIndex];

                // Remove object from the list
                BuyListYear.Remove(removeObject);

                // Calculate buy value with reduction and brokerage
                BuyValueBrokerageYear -= removeObject.BuyValueBrokerage;
                // Calculate buy value without reduction and brokerage
                BuyValueYear -= removeObject.BuyValue;
                // Calculate buy volume
                BuyVolumeYear -= removeObject.Volume;

#if DEBUG_BUY
                Console.WriteLine(@"BuyValueYear: {0}", BuyValueYear);
                Console.WriteLine(@"BuyValueBrokerageYear: {0}", BuyValueBrokerageYear);
                Console.WriteLine(@"VolumeYear: {0}", BuyVolumeYear);
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
        /// This function sets the document the buy object with the given Guid
        /// </summary>
        /// <param name="strGuid">Guid of the buy object which should be removed</param>
        /// <param name="strDocument">Document of the buy object</param>
        /// <returns>Flag if the set was successfully</returns>
        public bool SetDocumentBuyObject(string strGuid, string strDocument)
        {
#if DEBUG_BUY
            Console.WriteLine(@"SetDocumentBuyObject");
#endif
            try
            {
                // Search for the buy object
                foreach (var buyObject in BuyListYear)
                {
                    if (buyObject.Guid != strGuid) continue;

                    buyObject.Document = strDocument;

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion Methods
    }

    public class BuysYearOfTheShareComparer : IComparer<BuysYearOfTheShare>
    {
        public int Compare(BuysYearOfTheShare object1, BuysYearOfTheShare object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            if (Convert.ToInt16(object2.BuyListYear) == Convert.ToInt16(object1.BuyListYear))
                return 0;
            if (Convert.ToInt16(object2.BuyListYear) > Convert.ToInt16(object1.BuyListYear))
                return 1;

            return -1;
        }
    }
}
