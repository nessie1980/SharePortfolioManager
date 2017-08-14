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

using System;
using System.Collections.Generic;
using System.Globalization;
using SharePortfolioManager.Classes;

namespace SharePortfolioManager
{
    public class SalesYearOfTheShare
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _saleCultureInfo;

        /// <summary>
        /// Stores the sale payout in a year
        /// </summary>
        private decimal _salePayoutYear = -1;

        /// <summary>
        /// Stores the sale volume in a year
        /// </summary>
        private decimal _saleVolumeYear = -1;

        /// <summary>
        /// Stores the sale profit or loss in a year
        /// </summary>
        private decimal _saleProfitLossYear = -1;

        /// <summary>
        /// Stores the single sales of a year
        /// </summary>
        private List<SaleObject> _SaleListYear = new List<SaleObject>();

        /// <summary>
        /// Stores the single profit or loss of a year
        /// </summary>
        private List<ProfitLossObject> _ProfitLossListYear = new List<ProfitLossObject>();

        #endregion Variables

        #region Properties

        public CultureInfo SaleCultureInfo
        {
            get { return _saleCultureInfo; }
            internal set { _saleCultureInfo = value; }
        }

        public decimal SalePayoutYear
        {
            get { return _salePayoutYear; }
            internal set { _salePayoutYear = value; }
        }

        public string SalePayoutYearAsString
        {
            get { return Helper.FormatDecimal(_salePayoutYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        public string SalePayoutYearWithUnitAsString
        {
            get { return Helper.FormatDecimal(_salePayoutYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo); }
        }

        public decimal SaleVolumeYear
        {
            get { return _saleVolumeYear; }
            internal set { _saleVolumeYear = value; }
        }

        public string SaleVolumeYearAsString
        {
            get { return Helper.FormatDecimal(_saleVolumeYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        public string SaleVolumeYearWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleVolumeYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PieceUnit, SaleCultureInfo); }
        }

        public decimal SaleProfitLossYear
        {
            get { return _saleProfitLossYear; }
            internal set { _saleProfitLossYear = value; }
        }

        public string SaleProfitLossYearAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLossYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo); }
        }

        public string SaleProfitLossYearWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLossYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo); }
        }

        public List<SaleObject> SaleListYear
        {
            get { return _SaleListYear; }
        }

        public List<ProfitLossObject> ProfitLossListYear
        {
            get { return _ProfitLossListYear; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Standard constructor
        /// </summary>
        public SalesYearOfTheShare()
        { }

        /// <summary>
        /// This functions adds a new sale object to the year list with the given values
        /// It also recalculates the value of the sale shares in the year
        /// </summary>
        /// <param name="cultureInfo">Culture info of the buy</param>
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
        public bool AddSaleObject(CultureInfo cultureInfo, string strDate, decimal decVolume, decimal decBuyPrice, decimal decSalePrice, decimal decLossBalance, decimal decTaxAtSource, decimal decCapitalGains,
             decimal decSolidarityTax, decimal decCosts, string strDoc = "")
        {
#if DEBUG
            Console.WriteLine(@"AddSaleObject");
#endif
            try
            {
                // Set culture info of the share
                SaleCultureInfo = cultureInfo;

                // Create new SaleObject
                SaleObject addObject = new SaleObject(cultureInfo, strDate, decVolume, decBuyPrice, decSalePrice, decLossBalance, decTaxAtSource, decCapitalGains, decSolidarityTax, decCosts, strDoc = "");
                ProfitLossObject addPorfitLossObject = new ProfitLossObject(cultureInfo, strDate, addObject.ProfitLoss, strDoc);

                // Add object to the list
                SaleListYear.Add(addObject);
                SaleListYear.Sort(new SaleObjectComparer());

                // Add profit or loss object to the list
                ProfitLossListYear.Add(addPorfitLossObject);
                ProfitLossListYear.Sort(new ProfitLossObjectComparer());

                // Calculate sale value
                if (SalePayoutYear == -1)
                    SalePayoutYear = 0;
                SalePayoutYear += addObject.Payout;

                // Calculate sale volume
                if (SaleVolumeYear == -1)
                    SaleVolumeYear = 0;
                SaleVolumeYear += addObject.Volume;

                // Calculate sale profit or loss
                if (SaleProfitLossYear == -1)
                    SaleProfitLossYear = 0;
                SaleProfitLossYear += addObject.ProfitLoss;

#if DEBUG
                Console.WriteLine(@"SalePayoutYear: {0}", SalePayoutYear);
                Console.WriteLine(@"SaleVolumeYear: {0}", SaleVolumeYear);
                Console.WriteLine(@"SaleProfitLossYear: {0}", SaleProfitLossYear);
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
        /// This function removes the sale object with the given date and time from the list
        /// It also recalculates the sale value, volume and profit and loss
        /// </summary>
        /// <param name="SaleDateTime">Date and time of the sale object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveSaleObject(string strDateTime)
        {
#if DEBUG
            Console.WriteLine(@"RemoveSaleObject");
#endif
            try
            {
                // Search for the remove object
                int iFoundIndex = -1;
                foreach (SaleObject saleObject in SaleListYear)
                {
                    if (saleObject.Date == strDateTime)
                    {
                        iFoundIndex = SaleListYear.IndexOf(saleObject);
                        break;
                    }
                }
                // Save remove object
                SaleObject removeObject = SaleListYear[iFoundIndex];

                // Remove object from the list
                SaleListYear.Remove(removeObject);

                // Calculate sale value
                SalePayoutYear -= removeObject.Payout;
                // Calculate sale volume
                SaleVolumeYear -= removeObject.Volume;
                // Calculate sale profit or loss
                SaleProfitLossYear -= removeObject.ProfitLoss;

                // Search for the remove object
                iFoundIndex = -1;
                foreach (ProfitLossObject profitLossObject in ProfitLossListYear)
                {
                    if (profitLossObject.Date == strDateTime)
                    {
                        iFoundIndex = ProfitLossListYear.IndexOf(profitLossObject);
                        break;
                    }
                }
                // Save remove object
                ProfitLossObject removeProfitLossObject = ProfitLossListYear[iFoundIndex];

                // Remove object from the list
                ProfitLossListYear.Remove(removeProfitLossObject);

#if DEBUG
                Console.WriteLine(@"SalePayoutYear: {0}", SalePayoutYear);
                Console.WriteLine(@"SaleVolumeYear: {0}", SaleVolumeYear);
                Console.WriteLine(@"SaleProfitLossYear: {0}", SaleProfitLossYear);
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

    public class SalesYearOfTheShareComparer : IComparer<SalesYearOfTheShare>
    {
        public int Compare(SalesYearOfTheShare object1, SalesYearOfTheShare object2)
        {
            if (Convert.ToInt16(object2.SaleListYear) == Convert.ToInt16(object1.SaleListYear))
                return 0;
            else if (Convert.ToInt16(object2.SaleListYear) > Convert.ToInt16(object1.SaleListYear))
                return 1;
            else
                return -1;
        }
    }
}

