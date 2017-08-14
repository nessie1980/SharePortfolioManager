﻿//MIT License
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

namespace SharePortfolioManager
{
    public class AllSalesOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the total sale payout of the share
        /// </summary>
        private decimal _salePayoutTotal = 0;

        /// <summary>
        /// Stores the total sale volume of the share
        /// </summary>
        private decimal _saleVolumeTotal = 0;

        /// <summary>
        /// Stores the total sale profit or loss of the share
        /// </summary>
        private decimal _saleProfitLossTotal = 0;

        /// <summary>
        /// Stores the sales of a year of the share
        /// </summary>
        private SortedDictionary<string, SalesYearOfTheShare> _allSalesOfTheShareDictionary = new SortedDictionary<string, SalesYearOfTheShare>();

        #endregion Variables

        #region Properties

        public CultureInfo SaleCultureInfo
        {
            get { return _cultureInfo; }
            internal set { _cultureInfo = value; }
        }

        public decimal SalePayoutTotal
        {
            get { return _salePayoutTotal; }
            internal set { _salePayoutTotal = value; }
        }

        public string SalePayoutTotalAsString
        {
            get { return Helper.FormatDecimal(_salePayoutTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        public string SalePayoutTotalWithUnitAsString
        {
            get { return Helper.FormatDecimal(_salePayoutTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo); }
        }

        public decimal SaleVolumeTotal
        {
            get { return _saleVolumeTotal; }
            internal set { _saleVolumeTotal = value; }
        }

        public string SaleVolumeTotalAsString
        {
            get { return Helper.FormatDecimal(_saleVolumeTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        public string SaleVolumeTotalWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleVolumeTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PieceUnit, SaleCultureInfo); }
        }

        public decimal SaleProfitLossTotal
        {
            get { return _saleProfitLossTotal; }
            internal set { _saleProfitLossTotal = value; }
        }

        public string SaleProfitLossTotalAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLossTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        public string SaleProfitLossTotalWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLossTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo); }
        }

        public SortedDictionary<string, SalesYearOfTheShare> AllSalesOfTheShareDictionary
        {
            get { return _allSalesOfTheShareDictionary; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        public AllSalesOfTheShare()
        { }

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
        /// <param name="decLossBalance">Loss balance of the share</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="decCosts">Costs of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strDate, decimal decVolume, decimal decBuyPrice, decimal decSalePrice, decimal decLossBalance, decimal decTaxAtSource, decimal decCapitalGains,
             decimal decSolidarityTax, decimal decCosts, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"Add AllSalesOfTheShare");
#endif
            try
            {
                // Get year of the date of the new sale
                string year = "";
                GetYearOfDate(strDate, out year);
                if (year == null)
                    return false;

                // Search if a sale for the given year already exists if not add it
                SalesYearOfTheShare searchObject;
                if (AllSalesOfTheShareDictionary.TryGetValue(year, out searchObject))
                {
                    if (!searchObject.AddSaleObject(SaleCultureInfo, strDate, decVolume, decBuyPrice, decSalePrice, decLossBalance, decTaxAtSource, decCapitalGains, decSolidarityTax, decCosts, strDoc))
                        return false;
                }
                else
                {
                    // Add new year sale object for the sale with a new year
                    SalesYearOfTheShare addObject = new SalesYearOfTheShare();
                    // Add sale with the new year to the sale year list
                    if (addObject.AddSaleObject(SaleCultureInfo, strDate, decVolume, decBuyPrice, decSalePrice, decLossBalance, decTaxAtSource, decCapitalGains, decSolidarityTax, decCosts, strDoc))
                    {
                        AllSalesOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total sale value, total sale volume and sale profit or loss
                // Reset total sale value, sale volume and profit or loss
                SalePayoutTotal = 0;
                SaleVolumeTotal = 0;
                SaleProfitLossTotal = 0;

                // Calculate the new total sale value, sale volume and profit or loss
                foreach (var calcObject in AllSalesOfTheShareDictionary.Values)
                {
                    SalePayoutTotal += calcObject.SalePayoutYear;
                    SaleVolumeTotal += calcObject.SaleVolumeYear;
                    SaleProfitLossTotal += calcObject.SaleProfitLossYear;
                }
#if DEBUG
                Console.WriteLine(@"SalePayoutTotal:{0}", SalePayoutTotal);
                Console.WriteLine(@"SaleVolumeTotal:{0}", SaleVolumeTotal);
                Console.WriteLine(@"SaleProfitLossTotal:{0}", SaleProfitLossTotal);
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
#if DEBUG
            Console.WriteLine(@"Remove AllSalesOfTheShare");
#endif
            try
            {
                // Get year of the date of the sale which should be removed
                string year = "";
                GetYearOfDate(strDateTime, out year);
                if (year == null)
                    return false;

                // Search if the sale for the given year exists
                SalesYearOfTheShare searchObject;
                if (AllSalesOfTheShareDictionary.TryGetValue(year, out searchObject))
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
                SaleVolumeTotal = 0;
                SaleProfitLossTotal = 0;

                // Calculate the new total sale value, volume and profit or loss
                foreach (var calcObject in AllSalesOfTheShareDictionary.Values)
                {
                    SalePayoutTotal += calcObject.SalePayoutYear;
                    SaleVolumeTotal += calcObject.SaleVolumeYear;
                    SaleProfitLossTotal += calcObject.SaleProfitLossYear;
                }

#if DEBUG
                Console.WriteLine(@"SaleValueTotal:{0}", SalePayoutTotal);
                Console.WriteLine(@"SaleVolumeTotal:{0}", SaleVolumeTotal);
                Console.WriteLine(@"SaleProfitLossTotal:{0}", SaleProfitLossTotal);
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
            List<SaleObject> allSalesOfTheShare = new List<SaleObject>();

            foreach (SalesYearOfTheShare salesYearOfTheShareObject in AllSalesOfTheShareDictionary.Values)
            {
                foreach (SaleObject saleObject in salesYearOfTheShareObject.SaleListYear)
                {
                    allSalesOfTheShare.Add(saleObject);
                }
            }
            return allSalesOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total sales values of the years
        /// </summary>
        /// <returns>Dictionary with the years and the sales values of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllSalesTotalValues()
        {
            Dictionary<string, string> allSalesOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allSalesOfTheShare.Add(key, Helper.FormatDecimal(AllSalesOfTheShareDictionary[key].SalePayoutYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo));
            }
            return allSalesOfTheShare;
        }

        /// <summary>
        /// This function creates a dictionary with the years
        /// and the total costs of the years
        /// </summary>
        /// <returns>Dictionary with the years and the profit / loss values of the year or empty dictionary if no year exist.</returns>
        public Dictionary<string, string> GetAllProfitLossTotalValues()
        {
            Dictionary<string, string> allProfitLossOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllSalesOfTheShareDictionary.Keys)
            {
                allProfitLossOfTheShare.Add(key, Helper.FormatDecimal(AllSalesOfTheShareDictionary[key].SaleProfitLossYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo));
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
            string strYear = @"";
            GetYearOfDate(strDateTime, out strYear);

            if (strYear != null)
            {
                if (AllSalesOfTheShareDictionary.ContainsKey(strYear))
                {
                    foreach (var saleObject in AllSalesOfTheShareDictionary[strYear].SaleListYear)
                    {
                        if (saleObject.Date == strDateTime)
                        {
                            return saleObject;
                        }
                    }
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
            if (_allSalesOfTheShareDictionary.Count > 0)
            {
                SalesYearOfTheShare lastYearEntries = _allSalesOfTheShareDictionary.Last().Value;
                {
                    if (lastYearEntries.SaleListYear.Count > 0)
                    {
                        DateTime tempTimeDate = Convert.ToDateTime(lastYearEntries.SaleListYear.Last().Date);
                        DateTime givenTimeDate = Convert.ToDateTime(strDateTime);

                        if (givenTimeDate >= tempTimeDate)
                            return true;
                        else
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// This function tries to get the year of the given date string
        /// </summary>
        /// <param name="date">Date string (DD.MM.YYYY)</param>
        /// <param name="year">Year of the given date or null if the split failed</param>
        private void GetYearOfDate(string date, out string year)
        {
            string[] dateTimeElements = date.Split('.');
            if (dateTimeElements.Length != 3)
            {
                year = null;
            }
            else
            {
                string yearTime = dateTimeElements.Last();
                string[] dateElements = yearTime.Split(' ');
                year = dateElements.First();
            }
        }

        #endregion Methods
    }
}
