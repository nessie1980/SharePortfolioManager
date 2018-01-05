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
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
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

        public decimal SalePayoutTotal { get; internal set; }

        public string SalePayoutTotalAsStr => Helper.FormatDecimal(SalePayoutTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        public string SalePayoutTotalWithUnitAsStr => Helper.FormatDecimal(SalePayoutTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

        public decimal SalePayoutWithoutCostsTotal { get; internal set; }

        public string SalePayoutWithoutCostsTotalAsStr => Helper.FormatDecimal(SalePayoutWithoutCostsTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        public string SalePayoutWithoutCostsTotalWithUnitAsStr => Helper.FormatDecimal(SalePayoutWithoutCostsTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

        public decimal SaleVolumeTotal { get; internal set; }

        public string SaleVolumeTotalAsStr => Helper.FormatDecimal(SaleVolumeTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        public string SaleVolumeTotalWithUnitAsStr => Helper.FormatDecimal(SaleVolumeTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, ShareObject.PieceUnit, SaleCultureInfo);

        public decimal SalePurchaseValueTotal { get; internal set; }

        public string SalePurchaseValueTotalAsStr => Helper.FormatDecimal(SalePurchaseValueTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        public string SalePurchaseValueTotalWithUnitAsStr => Helper.FormatDecimal(SalePurchaseValueTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, ShareObject.PieceUnit, SaleCultureInfo);

        public decimal SaleProfitLossTotal { get; internal set; }

        public string SaleProfitLossTotalAsStr => Helper.FormatDecimal(SaleProfitLossTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        public string SaleProfitLossTotalWithUnitAsStr => Helper.FormatDecimal(SaleProfitLossTotal, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

        public decimal SaleProfitLossTotalWithoutCosts { get; internal set; }

        public string SaleProfitLossWithoutCostsTotalAsStr => Helper.FormatDecimal(SaleProfitLossTotalWithoutCosts, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        public string SaleProfitLossTotalWithoutCostsWithUnitAsStr => Helper.FormatDecimal(SaleProfitLossTotalWithoutCosts, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

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
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decBuyPrice">Buy price of the share</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="decCosts">Costs of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strDate, decimal decVolume, decimal decBuyPrice, decimal decSalePrice, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, decimal decCosts, string strDoc = "")
        {
#if DEBUG_SALE
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
                    if (!searchObject.AddSaleObject(SaleCultureInfo, strDate, decVolume, decBuyPrice, decSalePrice, decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decCosts, strDoc))
                        return false;
                }
                else
                {
                    // Add new year sale object for the sale with a new year
                    var addObject = new SalesYearOfTheShare();
                    // Add sale with the new year to the sale year list
                    if (addObject.AddSaleObject(SaleCultureInfo, strDate, decVolume, decBuyPrice, decSalePrice, decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decCosts, strDoc))
                    {
                        AllSalesOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total sale value, total sale volume and sale profit or loss
                // Reset total sale value, sale volume and profit or loss
                SalePayoutTotal = 0;
                SalePayoutWithoutCostsTotal = 0;
                SaleVolumeTotal = 0;
                SalePurchaseValueTotal = 0;
                SaleProfitLossTotal = 0;
                SaleProfitLossTotalWithoutCosts = 0;

                // Calculate the new total sale value, sale volume and profit or loss
                foreach (var calcObject in AllSalesOfTheShareDictionary.Values)
                {
                    SalePayoutTotal += calcObject.SalePayoutYear;
                    SalePayoutWithoutCostsTotal += calcObject.SalePayoutWithoutCostsYear;
                    SaleVolumeTotal += calcObject.SaleVolumeYear;
                    SalePurchaseValueTotal += calcObject.SalePurchaseValueYear;
                    SaleProfitLossTotal += calcObject.SaleProfitLossYear;
                    SaleProfitLossTotalWithoutCosts += calcObject.SaleProfitLossWithoutCostsYear;
                }
#if DEBUG_SALE
                Console.WriteLine(@"SalePayoutTotal:{0}", SalePayoutTotal);
                Console.WriteLine(@"SalePayoutWithoutCostsTotal:{0}", SalePayoutWithoutCostsTotal);
                Console.WriteLine(@"SaleVolumeTotal:{0}", SaleVolumeTotal);
                Console.WriteLine(@"SalePurchaseValueTotal:{0}", SalePurchaseValueTotal);
                Console.WriteLine(@"SaleProfitLossTotal:{0}", SaleProfitLossTotal);
                Console.WriteLine(@"SaleProfitLossTotalWithoutCosts:{0}", SaleProfitLossTotalWithoutCosts);
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
        /// <param name="strDateTime">Date and time of the sale which should be removed</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveSale(string strDateTime)
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
                    if (!searchObject.RemoveSaleObject(strDateTime))
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
                SalePayoutTotal = 0;
                SalePayoutWithoutCostsTotal = 0;
                SaleVolumeTotal = 0;
                SaleProfitLossTotal = 0;
                SaleProfitLossTotalWithoutCosts = 0;

                // Calculate the new total sale value, volume and profit or loss
                foreach (var calcObject in AllSalesOfTheShareDictionary.Values)
                {
                    SalePayoutTotal += calcObject.SalePayoutYear;
                    SalePayoutWithoutCostsTotal += calcObject.SalePayoutWithoutCostsYear;
                    SaleVolumeTotal += calcObject.SaleVolumeYear;
                    SalePurchaseValueTotal += calcObject.SalePurchaseValueYear;
                    SaleProfitLossTotal += calcObject.SaleProfitLossYear;
                    SaleProfitLossTotalWithoutCosts += calcObject.SaleProfitLossWithoutCostsYear;
                }

#if DEBUG_SALE
                Console.WriteLine(@"SaleValueTotal:{0}", SalePayoutTotal);
                Console.WriteLine(@"SalePayoutWithoutCostsYear:{0}", SalePayoutWithoutCostsTotal);
                Console.WriteLine(@"SaleVolumeTotal:{0}", SaleVolumeTotal);
                Console.WriteLine(@"SalePurchaseValueTotal:{0}", SalePurchaseValueTotal);
                Console.WriteLine(@"SaleProfitLossTotal:{0}", SaleProfitLossTotal);
                Console.WriteLine(@"SaleProfitLossTotalWithoutCosts:{0}", SaleProfitLossTotalWithoutCosts);
#endif
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
        /// and the total sales values with costs of the years
        /// </summary>
        /// <returns>Dictionary with the years and the sales values with costs of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllSalesTotalValues()
        {
            var allSalesOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allSalesOfTheShare.Add(key, Helper.FormatDecimal(AllSalesOfTheShareDictionary[key].SalePayoutYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo));
            }
            return allSalesOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total sales values without costs of the years
        /// </summary>
        /// <returns>Dictionary with the years and the sales values without costs of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllSalesWithoutCostsTotalValues()
        {
            var allSalesOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allSalesOfTheShare.Add(key, Helper.FormatDecimal(AllSalesOfTheShareDictionary[key].SalePayoutWithoutCostsYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo));
            }
            return allSalesOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total profit / loss value with costs of the years
        /// </summary>
        /// <returns>Dictionary with the years and the profit / loss values with costs of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllProfitLossTotalValues()
        {
            var allProfitLossOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allProfitLossOfTheShare.Add(key, Helper.FormatDecimal(AllSalesOfTheShareDictionary[key].SaleProfitLossYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo));
            }
            return allProfitLossOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total profit / loss value without costs of the years
        /// </summary>
        /// <returns>Dictionary with the years and the profit / loss values of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllProfitLossWithoutCostsTotalValues()
        {
            var allProfitLossOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allProfitLossOfTheShare.Add(key, Helper.FormatDecimal(AllSalesOfTheShareDictionary[key].SaleProfitLossWithoutCostsYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo));
            }
            return allProfitLossOfTheShare;
        }

        /// <summary>
        /// This function returns the sale object of the given date and time
        /// </summary>
        /// <param name="strDateTime">Date and time of the sale</param>
        /// <returns>SaleObject or null if the search failed</returns>
        public SaleObject GetSaleObjectByDateTime(string strDateTime)
        {
            GetYearOfDate(strDateTime, out var year);

            if (year == null) return null;

            if (!AllSalesOfTheShareDictionary.ContainsKey(year)) return null;

            foreach (var saleObject in AllSalesOfTheShareDictionary[year].SaleListYear)
            {
                if (saleObject.Date == strDateTime)
                {
                    return saleObject;
                }
            }

            return null;
        }

        /// <summary>
        /// This function checks if the given date is the last date of the entries
        /// </summary>
        /// <param name="strDateTime">Given date and time</param>
        /// <returns></returns>
        public bool IsDateLastDate(string strDateTime)
        {
            if (_allSalesOfTheShareDictionary.Count <= 0) return true;

            var lastYearEntries = _allSalesOfTheShareDictionary.Last().Value;
            {
                if (lastYearEntries.SaleListYear.Count <= 0) return true;

                var tempTimeDate = Convert.ToDateTime(lastYearEntries.SaleListYear.Last().Date);
                var givenTimeDate = Convert.ToDateTime(strDateTime);

                return givenTimeDate >= tempTimeDate;
            }
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
