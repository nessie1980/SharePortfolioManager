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

namespace SharePortfolioManager
{
    public class AllBuysOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _buyCultureInfo;

        /// <summary>
        /// Stores the total buy value of the share without the reduction and costs
        /// </summary>
        private decimal _buyMarketValueTotal = 0;

        /// <summary>
        /// Stores the total buy value with the discount and costs
        /// </summary>
        private decimal _buyFinalValueTotal = 0;

        /// <summary>
        /// Stores the total buy volume of the share
        /// </summary>
        private decimal _buyVolumeTotal = 0;

        /// <summary>
        /// Stores the buys of a year of the share
        /// The string stores the different buy years
        /// The BuysOfAYearOfTheShare stores the buys of each year
        /// </summary>
        private SortedDictionary<string, BuysYearOfTheShare> _allBuysOfTheShareDictionary = new SortedDictionary<string, BuysYearOfTheShare>();

        #endregion Variables

        #region Properties

        public CultureInfo BuyCultureInfo
        {
            get { return _buyCultureInfo; }
            internal set { _buyCultureInfo = value; }
        }

        public decimal BuyMarketValueTotal
        {
            get { return _buyMarketValueTotal; }
            internal set { _buyMarketValueTotal = value; }
        }

        public string BuyMarketValueTotalAsStr
        {
            get { return Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo); }
        }

        public string BuyMarketValueTotalAsStrUnit
        {
            get { return Helper.FormatDecimal(BuyMarketValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo); }
        }

        public decimal BuyFinalValueTotal
        {
            get { return _buyFinalValueTotal; }
            internal set { _buyFinalValueTotal = value; }
        }

        public string BuyFinalValueTotalAsStr
        {
            get { return Helper.FormatDecimal(BuyFinalValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo); }
        }

        public string BuyFinalValueTotalAsStrUnit
        {
            get { return Helper.FormatDecimal(BuyFinalValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", BuyCultureInfo); }
        }

        public decimal BuyVolumeTotal
        {
            get { return _buyVolumeTotal; }
            internal set { _buyVolumeTotal = value; }
        }

        public string BuyVolumeTotalAsStr
        {
            get { return Helper.FormatDecimal(BuyVolumeTotal, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", BuyCultureInfo); }
        }

        public string BuyVolumeTotalAsStrUnit
        {
            get { return Helper.FormatDecimal(BuyVolumeTotal, Helper.Volumefivelength, false, Helper.Volumetwofixlength, true, ShareObject.PieceUnit, BuyCultureInfo); }
        }

        public SortedDictionary<string, BuysYearOfTheShare> AllBuysOfTheShareDictionary
        {
            get { return _allBuysOfTheShareDictionary; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        public AllBuysOfTheShare()
        { }

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
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <param name="decVolume">Volume of the buy</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="decReduction">Reduction value of the buy</param>
        /// <param name="decCosts">Costs of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strDateTime, decimal decVolume, decimal decSharePrice, decimal decReduction, decimal decCosts, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"Add AllBuysOfTheShare");
#endif
            try
            {
                // Get year of the date of the new buy
                string year = "";
                GetYearOfDate(strDateTime, out year);
                if (year == null)
                    return false;

                // Search if a buy for the given year already exists if not add it
                BuysYearOfTheShare searchObject;
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out searchObject))
                {
                    if (!searchObject.AddBuyObject(BuyCultureInfo, strDateTime, decVolume, decSharePrice, decReduction, decCosts, strDoc))
                        return false;
                }
                else
                {
                    // Add new year buy object for the buy with a new year
                    BuysYearOfTheShare addObject = new BuysYearOfTheShare();
                    // Add buy with the new year to the buy year list
                    if (addObject.AddBuyObject(BuyCultureInfo, strDateTime, decVolume, decSharePrice, decReduction, decCosts, strDoc))
                    {
                        AllBuysOfTheShareDictionary.Add(year, addObject);
                    }
                    else
                        return false;
                }

                // Calculate the total buy value and total buy volume
                // Reset total buy value and buy volume
                BuyFinalValueTotal = 0;
                BuyMarketValueTotal = 0;
                BuyVolumeTotal = 0;

                // Calculate the new total buy value and buy volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyFinalValueTotal += calcObject.BuyFinalValueYear;
                    BuyMarketValueTotal += calcObject.BuyMarketValueYear;
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                }
#if DEBUG
                Console.WriteLine(@"MarketValueTotal:{0}", BuyMarketValueTotal);
                Console.WriteLine(@"FinalValueTotal:{0}", BuyFinalValueTotal);
                Console.WriteLine(@"VolumeTotal:{0}", BuyVolumeTotal);
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function remove a buy with the given date and time
        /// from the dictionary
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy which should be removed</param>
        /// <returns></returns>
        public bool RemoveBuy(string strDateTime)
        {
#if DEBUG
            Console.WriteLine(@"Remove AllBuysOfTheShare");
#endif
            try
            {
                // Get year of the date of the buy which should be removed
                string year = "";
                GetYearOfDate(strDateTime, out year);
                if (year == null)
                    return false;

                // Search if the buy for the given year exists
                BuysYearOfTheShare searchObject;
                if (AllBuysOfTheShareDictionary.TryGetValue(year, out searchObject))
                {
                    if (!searchObject.RemoveBuyObject(strDateTime))
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
                BuyFinalValueTotal = 0;
                BuyMarketValueTotal = 0;
                BuyVolumeTotal = 0;

                // Calculate the new total buy value and volume
                foreach (var calcObject in AllBuysOfTheShareDictionary.Values)
                {
                    BuyFinalValueTotal += calcObject.BuyFinalValueYear;
                    BuyMarketValueTotal += calcObject.BuyMarketValueYear;
                    BuyVolumeTotal += calcObject.BuyVolumeYear;
                }

#if DEBUG
                Console.WriteLine(@"MarketValueTotal:{0}", BuyMarketValueTotal);
                Console.WriteLine(@"FinalValueTotal:{0}", BuyFinalValueTotal);
                Console.WriteLine(@"VolumeTotal:{0}", BuyVolumeTotal);
#endif
            }
            catch
            {
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
            List<BuyObject> allBuysOfTheShare = new List<BuyObject>();

            foreach (BuysYearOfTheShare buysYearOfTheShareObject in AllBuysOfTheShareDictionary.Values)
            {
                foreach (BuyObject buyObject in buysYearOfTheShareObject.BuyListYear)
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
        public Dictionary<string, string> GetAllBuysTotalValues()
        {
            Dictionary<string, string> allBuysOfTheShare = new Dictionary<string, string>();

            foreach (var key in AllBuysOfTheShareDictionary.Keys)
            {
                allBuysOfTheShare.Add(key, string.Format(@"{0:N2}", AllBuysOfTheShareDictionary[key].BuyMarketValueYear));
            }
            return allBuysOfTheShare;
        }

        /// <summary>
        /// This function returns the buy object of the given date and time
        /// </summary>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <returns>BuyObject or null if the search failed</returns>
        public BuyObject GetBuyObjectByDateTime(string strDateTime)
        {
            string strYear = @"";
            GetYearOfDate(strDateTime, out strYear);

            if (strYear != null)
            {
                if (AllBuysOfTheShareDictionary.ContainsKey(strYear))
                {
                    foreach (var buyObject in AllBuysOfTheShareDictionary[strYear].BuyListYear)
                    {
                        if (buyObject.Date == strDateTime)
                        {
                            return buyObject;
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
            if (_allBuysOfTheShareDictionary.Count > 0)
            {
                BuysYearOfTheShare lastYearEntries = _allBuysOfTheShareDictionary.Last().Value;
                {
                    if (lastYearEntries.BuyListYear.Count > 0)
                    {
                        DateTime tempTimeDate = Convert.ToDateTime(lastYearEntries.BuyListYear.Last().Date);
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
