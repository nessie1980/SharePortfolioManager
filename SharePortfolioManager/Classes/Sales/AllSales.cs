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

using SharePortfolioManager.Classes.Costs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharePortfolioManager.Classes.Sales
{
    [Serializable]
    public class AllSalesOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the sales of a year of the share
        /// </summary>
        private readonly SortedDictionary<string, SalesYearOfTheShare> _allSalesOfTheShareDictionary = new SortedDictionary<string, SalesYearOfTheShare>();

        #endregion Variables

        #region Properties

        public CultureInfo SaleCultureInfo { get; internal set; }

        public decimal SaleVolumeTotal { get; internal set; }

        public decimal SalePurchaseValueTotal { get; internal set; }

        public decimal SalePurchaseValueBrokerageTotal { get; internal set; }

        public decimal SalePurchaseValueReductionTotal { get; internal set; }

        public decimal SalePurchaseValueBrokerageReductionTotal { get; internal set; }

        public decimal SalePayoutTotal { get; internal set; }

        public string SalePayoutTotalAsStr => Helper.FormatDecimal(SalePayoutTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SalePayoutTotalWithUnitAsStr => Helper.FormatDecimal(SalePayoutTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public decimal SalePayoutBrokerageTotal { get; internal set; }
        
        public string SalePayoutBrokerageTotalAsStr => Helper.FormatDecimal(SalePayoutBrokerageTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SalePayoutBrokerageTotalWithUnitAsStr => Helper.FormatDecimal(SalePayoutBrokerageTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public decimal SalePayoutReductionTotal { get; internal set; }

        public string SalePayoutReductionTotalAsStr => Helper.FormatDecimal(SalePayoutReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SalePayoutReductionTotalWithUnitAsStr => Helper.FormatDecimal(SalePayoutReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public decimal SalePayoutBrokerageReductionTotal { get; internal set; }

        public string SalePayoutBrokerageReductionTotalAsStr => Helper.FormatDecimal(SalePayoutBrokerageReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SalePayoutBrokerageReductionTotalWithUnitAsStr => Helper.FormatDecimal(SalePayoutBrokerageReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public decimal SaleProfitLossTotal { get; internal set; }

        public string SaleProfitLossTotalAsStr => Helper.FormatDecimal(SaleProfitLossTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SaleProfitLossTotalWithUnitAsStr => Helper.FormatDecimal(SaleProfitLossTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public decimal SaleProfitLossBrokerageTotal { get; internal set; }

        public string SaleProfitLossBrokerageTotalAsStr => Helper.FormatDecimal(SaleProfitLossBrokerageTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SaleProfitLossBrokerageTotalWithUnitAsStr => Helper.FormatDecimal(SaleProfitLossBrokerageTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public decimal SaleProfitLossReductionTotal { get; internal set; }

        public string SaleProfitLossReductionTotalAsStr => Helper.FormatDecimal(SaleProfitLossReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SaleProfitLossReductionTotalWithUnitAsStr => Helper.FormatDecimal(SaleProfitLossReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public decimal SaleProfitLossBrokerageReductionTotal { get; internal set; }

        public string SaleProfitLossBrokerageReductionTotalAsStr => Helper.FormatDecimal(SaleProfitLossBrokerageReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo);

        public string SaleProfitLossBrokerageReductionTotalWithUnitAsStr => Helper.FormatDecimal(SaleProfitLossBrokerageReductionTotal, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo);

        public SortedDictionary<string, SalesYearOfTheShare> AllSalesOfTheShareDictionary => _allSalesOfTheShareDictionary;

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function sets the culture info to the list
        /// </summary>
        /// <param name="cultureInfo"></param>
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            SaleCultureInfo = cultureInfo;
        }

        /// <summary>
        /// This function adds a sale to the list
        /// </summary>
        /// <param name="strGuid">Guid of the share sale</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="strOrderNumber">Order number of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="usedBuyDetails">Details of the used buys for the sale</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="brokerageObject">Brokerage of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strGuid, string strDate, string strOrderNumber, decimal decVolume, decimal decSalePrice, List<SaleBuyDetails> usedBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, BrokerageReductionObject brokerageObject, string strDoc = "")
        {
#if false
            Console.WriteLine(@"Add AllSalesOfTheShare");
#endif
            try
            {
                // Get year of the date of the new sale
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if a sale for the given year already exists if not add it
                if (AllSalesOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.AddSaleObject(SaleCultureInfo, strGuid, strDate, strOrderNumber, decVolume, decSalePrice, usedBuyDetails, decTaxAtSource, decCapitalGainsTax, decSolidarityTax, brokerageObject, strDoc))
                        return false;
                }
                else
                {
                    // Add new year sale object for the sale with a new year
                    var addObject = new SalesYearOfTheShare();
                    // Add sale with the new year to the sale year list
                    if (addObject.AddSaleObject(SaleCultureInfo, strGuid, strDate, strOrderNumber, decVolume, decSalePrice, usedBuyDetails ,decTaxAtSource, decCapitalGainsTax, decSolidarityTax, brokerageObject, strDoc))
                    {
                        AllSalesOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total sale value, total sale volume and sale profit or loss
                // Reset total sale value, sale volume and profit or loss
                SaleVolumeTotal = 0;

                SalePurchaseValueTotal = 0;
                SalePurchaseValueBrokerageTotal = 0;
                SalePurchaseValueReductionTotal = 0;
                SalePurchaseValueBrokerageReductionTotal = 0;

                SalePayoutTotal = 0;
                SalePayoutBrokerageTotal = 0;
                SalePayoutReductionTotal = 0;
                SalePayoutBrokerageReductionTotal = 0;

                SaleProfitLossTotal = 0;
                SaleProfitLossBrokerageTotal = 0;
                SaleProfitLossReductionTotal = 0;
                SaleProfitLossBrokerageReductionTotal = 0;

                // Calculate the new total sale value, sale volume and profit or loss
                foreach (var calcObject in AllSalesOfTheShareDictionary.Values)
                {
                    SaleVolumeTotal += calcObject.SaleVolumeYear;

                    SalePurchaseValueTotal += calcObject.SalePurchaseValueYear;
                    SalePurchaseValueBrokerageTotal += calcObject.SalePurchaseValueBrokerageYear;
                    SalePurchaseValueReductionTotal += calcObject.SalePurchaseValueReductionYear;
                    SalePurchaseValueBrokerageReductionTotal += calcObject.SalePurchaseValueBrokerageReductionYear;

                    SalePayoutTotal += calcObject.SalePayoutYear;
                    SalePayoutBrokerageTotal += calcObject.SalePayoutBrokerageYear;
                    SalePayoutReductionTotal += calcObject.SalePayoutReductionYear;
                    SalePayoutBrokerageReductionTotal += calcObject.SalePayoutBrokerageReductionYear;

                    SaleProfitLossTotal += calcObject.SaleProfitLossYear;
                    SaleProfitLossBrokerageTotal += calcObject.SaleProfitLossBrokerageYear;
                    SaleProfitLossReductionTotal += calcObject.SaleProfitLossReductionYear;
                    SaleProfitLossBrokerageReductionTotal += calcObject.SaleProfitLossBrokerageReductionYear;
                }
#if false
                Console.WriteLine(@"SaleVolumeTotal:{0}", SaleVolumeTotal);
                Console.WriteLine(@"SalePurchaseValueTotal:{0}", SalePurchaseValueTotal);
                Console.WriteLine(@"SalePayoutTotal:{0}", SalePayoutTotal);
                Console.WriteLine(@"SalePayoutBrokerageTotal:{0}", SalePayoutBrokerageTotal);
                Console.WriteLine(@"SalePayoutReductionTotal:{0}", SalePayoutReductionTotal);
                Console.WriteLine(@"SalePayoutBrokerageReductionTotal:{0}", SalePayoutBrokerageReductionTotal);
                Console.WriteLine(@"SaleProfitLossTotal:{0}", SaleProfitLossTotal);
                Console.WriteLine(@"SaleProfitLossBrokerageTotal:{0}", SaleProfitLossBrokerageTotal);
                Console.WriteLine(@"SaleProfitLossReductionTotal:{0}", SaleProfitLossReductionTotal);
                Console.WriteLine(@"SaleProfitLossBrokerageReductionTotal:{0}", SaleProfitLossBrokerageReductionTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function remove a sale with the given date and time
        /// from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the sale which should be removed</param>
        /// <param name="strDateTime">Date and time of the sale which should be removed</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveSale(string strGuid, string strDateTime)
        {
#if DEBUG_SALE
            Console.WriteLine(@"Remove AllSalesOfTheShare");
#endif
            try
            {
                // Get year of the date of the sale which should be removed
                GetYearOfDate(strDateTime, out var year);
                if (year == null)
                    return false;

                // Search if the sale for the given year exists
                if (AllSalesOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.RemoveSaleObject(strGuid))
                        return false;

                    if (searchObject.SaleListYear.Count == 0)
                    {
                        if (!AllSalesOfTheShareDictionary.Remove(year))
                            return false;
                    }
                }
                else
                {
                    return false;
                }

                // Calculate the total sale value, volume and profit or loss
                // Reset total sale value, volume and profit or loss
                SaleVolumeTotal = 0;

                SalePurchaseValueTotal = 0;
                SalePurchaseValueBrokerageTotal = 0;
                SalePurchaseValueReductionTotal = 0;
                SalePurchaseValueBrokerageReductionTotal = 0;

                SalePayoutTotal = 0;
                SalePayoutBrokerageTotal = 0;
                SalePayoutReductionTotal = 0;
                SalePayoutBrokerageReductionTotal = 0;

                SaleProfitLossTotal = 0;
                SaleProfitLossBrokerageTotal = 0;
                SaleProfitLossReductionTotal = 0;
                SaleProfitLossBrokerageReductionTotal = 0;

                // Calculate the new total sale value, volume and profit or loss
                foreach (var calcObject in AllSalesOfTheShareDictionary.Values)
                {
                    SaleVolumeTotal += calcObject.SaleVolumeYear;

                    SalePurchaseValueTotal += calcObject.SalePurchaseValueYear;
                    SalePurchaseValueBrokerageTotal += calcObject.SalePurchaseValueBrokerageYear;
                    SalePurchaseValueReductionTotal += calcObject.SalePurchaseValueReductionYear;
                    SalePurchaseValueBrokerageReductionTotal += calcObject.SalePurchaseValueBrokerageReductionYear;

                    SalePayoutTotal += calcObject.SalePayoutYear;
                    SalePayoutBrokerageTotal += calcObject.SalePayoutBrokerageYear;
                    SalePayoutReductionTotal += calcObject.SalePayoutReductionYear;
                    SalePayoutBrokerageReductionTotal += calcObject.SalePayoutBrokerageReductionYear;

                    SaleProfitLossTotal += calcObject.SaleProfitLossYear;
                    SaleProfitLossBrokerageTotal += calcObject.SaleProfitLossBrokerageYear;
                    SaleProfitLossReductionTotal += calcObject.SaleProfitLossReductionYear;
                    SaleProfitLossBrokerageReductionTotal += calcObject.SaleProfitLossBrokerageReductionYear;
                }

#if false
                Console.WriteLine(@"SaleVolumeTotal:{0}", SaleVolumeTotal);
                Console.WriteLine(@"SalePurchaseValueTotal:{0}", SalePurchaseValueTotal);
                Console.WriteLine(@"SalePayoutTotal:{0}", SalePayoutTotal);
                Console.WriteLine(@"SalePayoutBrokerageTotal:{0}", SalePayoutBrokerageTotal);
                Console.WriteLine(@"SalePayoutReductionTotal:{0}", SalePayoutReductionTotal);
                Console.WriteLine(@"SalePayoutBrokerageReductionTotal:{0}", SalePayoutBrokerageReductionTotal);
                Console.WriteLine(@"SaleProfitLossTotal:{0}", SaleProfitLossTotal);
                Console.WriteLine(@"SaleProfitLossBrokerageTotal:{0}", SaleProfitLossBrokerageTotal);
                Console.WriteLine(@"SaleProfitLossReductionTotal:{0}", SaleProfitLossReductionTotal);
                Console.WriteLine(@"SaleProfitLossBrokerageReductionTotal:{0}", SaleProfitLossBrokerageReductionTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function sets the document of a sale with the given Guid and date and time
        /// from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the sale which should be modified</param>
        /// <param name="strDate">Date and time of the sale which should be modified</param>
        /// <param name="strDocument">Document of the sale</param>
        /// <returns></returns>
        public bool SetDocumentSale(string strGuid, string strDate, string strDocument)
        {
#if DEBUG_BUY
            Console.WriteLine(@"Remove SetDocumentSale");
#endif
            try
            {
                // Get year of the date of the sale which should be modified
                GetYearOfDate(strDate, out var year);
                if (year == null)
                    return false;

                // Search if the sale for the given year exists
                if (AllSalesOfTheShareDictionary.TryGetValue(year, out var searchObject))
                {
                    if (!searchObject.SetDocumentSaleObject(strGuid, strDocument))
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function creates a list of all sale objects of the share
        /// </summary>
        /// <returns>List of SaleObjects or a empty list if no SaleObjects exists</returns>
        public List<SaleObject> GetAllSalesOfTheShare()
        {
            var allSalesOfTheShare = new List<SaleObject>();

            foreach (var salesYearOfTheShareObject in AllSalesOfTheShareDictionary.Values)
            {
                foreach (var saleObject in salesYearOfTheShareObject.SaleListYear)
                {
                    allSalesOfTheShare.Add(saleObject);
                }
            }
            return allSalesOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total sales values with brokerage of the years
        /// </summary>
        /// <returns>Dictionary with the years and the sales values with brokerage of the year or empty dictionary if no year exist.</returns>
        public List<SalesYearOfTheShare> GetAllSalesTotalValues()
        {
            var allSalesOfTheShare = new List<SalesYearOfTheShare>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allSalesOfTheShare.Add(AllSalesOfTheShareDictionary[key]);
            }

            return allSalesOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total profit / loss value with brokerage of the years
        /// </summary>
        /// <returns>Dictionary with the years and the profit / loss values with brokerage of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllProfitLossTotalValues()
        {
            var allProfitLossOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allProfitLossOfTheShare.Add(key, Helper.FormatDecimal(AllSalesOfTheShareDictionary[key].SaleProfitLossYear, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength, false, @"", SaleCultureInfo));
            }
            return allProfitLossOfTheShare;
        }

        /// <summary>
        /// This function returns the sale object of the given date and time
        /// </summary>
        /// <param name="strGuid">Guid of the sale</param>
        /// <param name="strDate">Date and time of the sale</param>
        /// <returns>SaleObject or null if the search failed</returns>
        public SaleObject GetSaleObjectByGuidDate(string strGuid, string strDate)
        {
            GetYearOfDate(strDate, out var year);

            if (year == null) return null;

            if (!AllSalesOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var saleObject in AllSalesOfTheShareDictionary[year].SaleListYear)
            {
                if (saleObject.Guid == strGuid)
                {
                    return saleObject;
                }
            }

            return null;
        }

        /// <summary>
        /// This function checks if the sale with the given order number already exists
        /// </summary>
        /// <param name="strOrderNumber">Given order number</param>
        /// <param name="strGuidSale">Guid of the current selected sale</param>
        /// <returns></returns>
        public bool OrderNumberAlreadyExists(string strOrderNumber, string strGuidSale)
        {
            foreach (var saleList in _allSalesOfTheShareDictionary.Values)
            {
                foreach (var sale in saleList.SaleListYear)
                {
                    if (strOrderNumber == sale.OrderNumber && sale.Guid != strGuidSale)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function checks if the sale with the given Guid is the last sale of the entries
        /// </summary>
        /// <param name="strGuid">Given GUID</param>
        /// <returns></returns>
        public bool IsLastSale(string strGuid)
        {
            if (_allSalesOfTheShareDictionary.Count <= 0) return false;

            var lastYearEntries = _allSalesOfTheShareDictionary.Last().Value;

            if (lastYearEntries.SaleListYear.Count <= 0) return false;

            return lastYearEntries.SaleListYear.Last().Guid == strGuid;
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
