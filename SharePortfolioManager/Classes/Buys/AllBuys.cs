//MIT License
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
//#define DEBUG_ALL_BUYS

using SharePortfolioManager.Classes.Brokerage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharePortfolioManager.Classes.Buys
{
    /// <summary>
    /// This class handles all buys of a share.
    /// It calculates all necessary values of the buys of the share:
    /// - buy volume
    /// - buy value
    /// - buy value minus reduction
    /// - buy value plus brokerage
    /// - buy value plus brokerage minus reduction
    /// Dictionary with all buys sorted by the buy years
    /// </summary>
    [Serializable]
    public class AllBuysOfTheShare
    {
        #region Variables

        #endregion Variables

        #region Properties

        /// <summary>
        /// Culture info of the buys
        /// </summary>
        public CultureInfo BuyCultureInfo { get; internal set; }

        /// <summary>
        /// Total value of all buys
        /// </summary>
        public decimal BuyVolumeTotal { get; internal set; }

        /// <summary>
        /// Total buy value of all buys
        /// </summary>
        public decimal BuyValueTotal { get; internal set; }

        /// <summary>
        /// Total buy value of all buys as string with unit
        /// </summary>
        public string BuyValueTotalAsStrUnit => Helper.FormatDecimal(BuyValueTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", BuyCultureInfo);

        /// <summary>
        /// Total buy value minus reduction of all buys
        /// </summary>
        public decimal BuyValueReductionTotal { get; internal set; }

        /// <summary>
        /// Total buy value minus reduction of all buys as string
        /// </summary>
        public string BuyValueReductionTotalAsStr => Helper.FormatDecimal(BuyValueReductionTotal, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Total buy value minus reduction of all buys as string with unit
        /// </summary>
        public string BuyValueReductionTotalAsStrUnit => Helper.FormatDecimal(BuyValueReductionTotal, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", BuyCultureInfo);

        /// <summary>
        /// Total buy value plus brokerage of all buys
        /// </summary>
        public decimal BuyValueBrokerageTotal { get; internal set; }

        /// <summary>
        /// Total buy value plus brokerage and minus reduction of all buys
        /// </summary>
        public decimal BuyValueBrokerageReductionTotal { get; internal set; }

        /// <summary>
        /// Total buy value plus brokerage and minus reduction of all buys as string
        /// </summary>
        public string BuyValueBrokerageReductionTotalAsStr => Helper.FormatDecimal(BuyValueBrokerageReductionTotal, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Total buy value plus brokerage and minus reduction of all buys as string with unit
        /// </summary>
        public string BuyValueBrokerageReductionTotalAsStrUnit => Helper.FormatDecimal(BuyValueBrokerageReductionTotal, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true, @"", BuyCultureInfo);

        /// <summary>
        /// Sorted dictionary of all buy. The dictionary is sorted by the buy year.
        /// </summary>
        public SortedDictionary<string, BuysYearOfTheShare> AllBuysOfTheShareDictionary { get; } = new SortedDictionary<string, BuysYearOfTheShare>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function sets the culture info of the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            BuyCultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a buy to the list
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDepotNumber">Depot number where the buy has been done</param>
        /// <param name="strOrderNumber">Order number of the buy</param>
        /// <param name="strDate">Date and time of the buy</param>
        /// <param name="decVolume">Volume of the buy</param>
        /// <param name="decVolumeSold">Volume of the buy which is already sold</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="brokerageObject">Brokerage of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strGuid, string strDepotNumber, string strOrderNumber, string strDate, decimal decVolume, decimal decVolumeSold, decimal decSharePrice,
            BrokerageReductionObject brokerageObject, string strDoc = "")
        {
#if DEBUG_ALL_BUYS
            Console.WriteLine(@"AddBuy()");
#endif
            try
            {
                // Get year of the date of the new buy
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a buy for the given year already exists if not add it
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddBuyObject(BuyCultureInfo, strGuid, strDepotNumber, strOrderNumber, strDate, decVolume, decVolumeSold, decSharePrice,
                        brokerageObject, strDoc))
                        return false;
                }
                else
                {
                    // Add new year buy object for the buy with a new year
                    var addObject = new BuysYearOfTheShare();
                    // Add buy with the new year to the buy year list
                    if (addObject.AddBuyObject(BuyCultureInfo, strGuid, strDepotNumber, strOrderNumber, strDate, decVolume, decVolumeSold, decSharePrice,
                        brokerageObject, strDoc))
                    {
                        AllBuysOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total buy value and total buy volume
                // Reset total buy value and buy volume
                BuyVolumeTotal = 0;
                BuyValueTotal = 0;
                BuyValueReductionTotal = 0;
                BuyValueBrokerageTotal = 0;
                BuyValueBrokerageReductionTotal = 0;

                // Calculate the new total buy value and buy volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                    BuyValueTotal += calcObject.BuyValueYear;
                    BuyValueReductionTotal += calcObject.BuyValueReductionYear;
                    BuyValueBrokerageTotal += calcObject.BuyValueBrokerageYear;
                    BuyValueBrokerageReductionTotal += calcObject.BuyValueBrokerageReductionYear;
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }

#if DEBUG_ALL_BUYS
            Console.WriteLine(@"BuyVolumeTotal: {0}", BuyVolumeTotal);
            Console.WriteLine(@"BuyValueTotal: {0}", BuyValueTotal);
            Console.WriteLine(@"BuyValueReductionTotal: {0}", BuyValueReductionTotal);
            Console.WriteLine(@"BuyValueBrokerageTotal: {0}", BuyValueBrokerageTotal);
            Console.WriteLine(@"BuyValueBrokerageReductionTotal: {0}", BuyValueBrokerageReductionTotal);
#endif

            return true;
        }

        /// <summary>
        /// This function remove a buy with the given Guid and date and time
        /// from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be removed</param>
        /// <param name="strDate">Date and time of the buy which should be removed</param>
        /// <returns></returns>
        public bool RemoveBuy(string strGuid, string strDate)
        {
            try
            {
                // Get year of the date of the buy which should be removed
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if the buy for the given year exists
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveBuyObject(strGuid))
                        return false;

                    if (searchObject.BuyListYear.Count == 0)
                    {
                        if (!AllBuysOfTheShareDictionary.Remove(year))
                            return false;
                    }
                }
                else
                {
                    return false;
                }

                // Calculate the total buy value and volume
                // Reset total buy value and volume
                BuyVolumeTotal = 0;
                BuyValueTotal = 0;
                BuyValueReductionTotal = 0;
                BuyValueBrokerageTotal = 0;
                BuyValueBrokerageReductionTotal = 0;

                // Calculate the new total buy value and volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                    BuyValueTotal += calcObject.BuyValueYear;
                    BuyValueReductionTotal += calcObject.BuyValueReductionYear;
                    BuyValueBrokerageTotal += calcObject.BuyValueBrokerageYear;
                    BuyValueBrokerageReductionTotal += calcObject.BuyValueBrokerageReductionYear;
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function sets the document of a buy with the given Guid and date and time
        /// from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be modified</param>
        /// <param name="strDate">Date and time of the buy which should be modified</param>
        /// <param name="strDocument">Document of the buy</param>
        /// <returns></returns>
        public bool SetDocumentBuy(string strGuid, string strDate, string strDocument)
        {
            try
            {
                // Get year of the date of the buy which should be modified
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if the buy for the given year exists
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.SetDocumentBuyObject(strGuid, strDocument))
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function creates a list of all buy objects of the share
        /// </summary>
        /// <returns>List of BuyObjects or a empty list if no BuyObjects exists</returns>
        public List<BuyObject> GetAllBuysOfTheShare()
        {
            var allBuysOfTheShare = new List<BuyObject>();

            foreach (var buysYearOfTheShareObject in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buysYearOfTheShareObject.BuyListYear)
                {
                    allBuysOfTheShare.Add(buyObject);
                }
            }
            return allBuysOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total buy values of the years
        /// </summary>
        /// <returns>Dictionary with the years and the buys values of the year or empty dictionary if no year exist.</returns>
        public List<BuysYearOfTheShare> GetAllBuysTotalValues()
        {
            var allBuysOfTheShare = new List<BuysYearOfTheShare>();

            foreach (var key in AllBuysOfTheShareDictionary.Keys)
            {
                allBuysOfTheShare.Add(AllBuysOfTheShareDictionary[key]);
            }
            return allBuysOfTheShare;
        }

        /// <summary>
        /// This function returns the buy object of the given date and time
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDate">Date and time of the buy</param>
        /// <returns>BuyObject or null if the search failed</returns>
        public BuyObject GetBuyObjectByGuidDate(string strGuid, string strDate)
        {
            GetYearOfDate(strDate, out var year);

            if (year == null) return null;

            if (!AllBuysOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var buyObject in AllBuysOfTheShareDictionary[year].BuyListYear)
            {
                if (buyObject.Guid == strGuid)
                {
                    return buyObject;
                }
            }

            return null;
        }

        /// <summary>
        /// This function adds a volume to the sold volume
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="decSaleVolume">Sale volume</param>
        /// <returns>Flag if the sale volume add has been successful</returns>
        public bool AddSaleVolumeByGuid(string strGuid, decimal decSaleVolume)
        {
            foreach (var buyObjectList in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buyObjectList.BuyListYear)
                {
                    if (buyObject.Guid != strGuid ) continue;

                    buyObject.VolumeSold += decSaleVolume;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function removes a volume of the sold volume
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="decSaleVolume">Sale volume</param>
        /// <returns>Flag if the sale volume remove has been successful</returns>
        public bool RemoveSaleVolumeByGuid(string strGuid, decimal decSaleVolume)
        {
            foreach (var buyObjectList in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buyObjectList.BuyListYear)
                {
                    if (buyObject.Guid != strGuid) continue;

                    if (decSaleVolume <= buyObject.VolumeSold)
                        buyObject.VolumeSold -= decSaleVolume;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function checks if the buy with the given Guid is the last buy
        /// </summary>
        /// <param name="strGuid">Given GUID</param>
        /// <returns>Flag if this is the last buy</returns>
        public bool IsLastBuy(string strGuid)
        {
            if (GetAllBuysOfTheShare().Count <= 0) return false;

            // Get current selected buy object
            var buyObject = GetAllBuysOfTheShare().Find(x => x.Guid == strGuid);

            if (buyObject == null)
                return false;

            // Get date of the selected buy object
            var selectedDate = DateTime.Parse(buyObject.Date);

            // Check if the selected buy object is the last one
            foreach (var buyObjectCompare in GetAllBuysOfTheShare())
            {
                var compareDate = DateTime.Parse(buyObjectCompare.Date);

                // Check if the selected buy object is not the last one
                if (DateTime.Compare(selectedDate, compareDate) < 0)
                    return false;
            }

            // Selected object is the last one
            return true;
        }

        /// <summary>
        /// This function checks if the buy with the given order number already exists
        /// </summary>
        /// <param name="strOrderNumber">Given order number</param>
        /// <returns></returns>
        public bool OrderNumberAlreadyExists(string strOrderNumber)
        {
            foreach (var buyList in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buy in buyList.BuyListYear)
                {
                    if (strOrderNumber == buy.OrderNumber)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function checks if the buy is already part of a sale
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <returns>Flag if the buy is part of a sale.</returns>
        public bool IsPartOfASale(string strGuid)
        {
            if (AllBuysOfTheShareDictionary.Count <= 0) return false;

            foreach (var buysYearOfTheShare in AllBuysOfTheShareDictionary.Values)
            {
                foreach (var buyObject in buysYearOfTheShare.BuyListYear)
                {
                    if (buyObject.Guid != strGuid) continue;
                    return buyObject.VolumeSold != 0;
                }
            }

            return false;
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
