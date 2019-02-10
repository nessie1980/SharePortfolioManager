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

using SharePortfolioManager.Classes.Costs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace SharePortfolioManager.Classes.Sales
{
    [Serializable]
    public class SalesYearOfTheShare
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo SaleCultureInfo { get; internal set; }

        [Browsable(false)]
        public string SaleYear { get; internal set; } = @"-";

        [Browsable(false)]
        public decimal SalePayoutYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SalePayoutYearAsStr => Helper.FormatDecimal(SalePayoutYear, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public string SalePayoutYearWithUnitAsStr => Helper.FormatDecimal(SalePayoutYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal SalePayoutWithoutBrokerageYear { get; internal set; } = -1;

        [Browsable(false)]
        public decimal SaleVolumeYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SaleVolumeYearAsStr => Helper.FormatDecimal(SaleVolumeYear, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal SalePurchaseValueYear { get; internal set; } = -1;

        [Browsable(false)]
        public decimal SaleProfitLossYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SaleProfitLossYearWithUnitAsStr => Helper.FormatDecimal(SaleProfitLossYear, Helper.Currencytwolength, false, Helper.Currencytwofixlength, true, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal SaleProfitLossWithoutBrokerageYear { get; internal set; } = -1;

        [Browsable(false)]
        public List<SaleObject> SaleListYear { get; } = new List<SaleObject>();

        [Browsable(false)]
        public List<ProfitLossObject> ProfitLossListYear { get; } = new List<ProfitLossObject>();

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Year")]
        public string DgvSaleYear => SaleYear;

        [Browsable(true)]
        [DisplayName(@"YearVolume")]
        public string DgvSaleVolumeYear => SaleVolumeYearAsStr;
        
        [Browsable(true)]
        [DisplayName(@"YearPayout")]
        public string DgvSalePayoutYearAsStr => SalePayoutYearAsStr;

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// This functions adds a new sale object to the year list with the given values
        /// It also recalculates the value of the sale shares in the year
        /// </summary>
        /// <param name="cultureInfo">Culture info of the sale</param>
        /// <param name="strGuid">Guid of the sale</param>
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
        public bool AddSaleObject(CultureInfo cultureInfo,  string strGuid, string strDate, string strOrderNumber, decimal decVolume, decimal decSalePrice, List<SaleBuyDetails> usedBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, BrokerageReductionObject brokerageObject, string strDoc = "")
        {
#if DEBUG_SALE
            Console.WriteLine(@"AddSaleObject");
#endif
            try
            {
                // Set culture info of the share
                SaleCultureInfo = cultureInfo;

                // Create new SaleObject
                var addObject = new SaleObject(cultureInfo, strGuid, strDate, strOrderNumber, decVolume, decSalePrice, usedBuyDetails, decTaxAtSource, decCapitalGainsTax, decSolidarityTax, brokerageObject, strDoc);

                var addProfitLossObject = new ProfitLossObject(cultureInfo, strGuid, strDate, addObject.ProfitLoss, strDoc);

                // Add object to the list
                SaleListYear.Add(addObject);
                SaleListYear.Sort(new SaleObjectComparer());

                // Add profit or loss object to the list
                ProfitLossListYear.Add(addProfitLossObject);
                ProfitLossListYear.Sort(new ProfitLossObjectComparer());

                // Set year
                DateTime.TryParse(strDate, out var dateTime);
                SaleYear = dateTime.Year.ToString();

                // Calculate sale value
                if (SalePayoutYear == -1)
                    SalePayoutYear = 0;
                SalePayoutYear += addObject.Payout;
                if (SalePayoutWithoutBrokerageYear == -1)
                    SalePayoutWithoutBrokerageYear = 0;
                SalePayoutWithoutBrokerageYear += addObject.PayoutWithoutBrokerage;

                // Calculate sale volume
                if (SaleVolumeYear == -1)
                    SaleVolumeYear = 0;
                SaleVolumeYear += addObject.Volume;

                // Calculate sale purchase value
                if (SalePurchaseValueYear == -1)
                    SalePurchaseValueYear = 0;
                SalePurchaseValueYear += addObject.PurchaseValue;

                // Calculate sale profit or loss
                if (SaleProfitLossYear == -1)
                    SaleProfitLossYear = 0;
                SaleProfitLossYear += addObject.ProfitLoss;
                if (SaleProfitLossWithoutBrokerageYear == -1)
                    SaleProfitLossWithoutBrokerageYear = 0;
                SaleProfitLossWithoutBrokerageYear += addObject.ProfitLossWithoutBrokerage;

#if DEBUG_SALE
                Console.WriteLine(@"SalePayoutYear: {0}", SalePayoutYear);
                Console.WriteLine(@"SalePayoutWithoutBrokerageYear: {0}", SalePayoutWithoutBrokerageYear);
                Console.WriteLine(@"SaleVolumeYear: {0}", SaleVolumeYear);
                Console.WriteLine(@"SalePurchaseValueYear: {0}", SalePurchaseValueYear);
                Console.WriteLine(@"SaleProfitLossYear: {0}", SaleProfitLossYear);
                Console.WriteLine(@"SaleProfitLossWithoutBrokerageYear: {0}", SaleProfitLossWithoutBrokerageYear);
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
        /// <param name="strGuid">Guid of the sale object which should be removed</param>
        /// <returns>Flag if the remove was successfully</returns>
        public bool RemoveSaleObject(string strGuid)
        {
#if DEBUG_SALE
            Console.WriteLine(@"RemoveSaleObject");
#endif
            try
            {
                // Search for the remove object
                var iFoundIndex = -1;
                foreach (var saleObject in SaleListYear)
                {
                    if (saleObject.Guid != strGuid) continue;

                    iFoundIndex = SaleListYear.IndexOf(saleObject);
                    break;
                }
                // Save remove object
                var removeObject = SaleListYear[iFoundIndex];

                // Remove object from the list
                SaleListYear.Remove(removeObject);

                // Calculate sale value
                SalePayoutYear -= removeObject.Payout;
                SalePayoutWithoutBrokerageYear -= removeObject.PayoutWithoutBrokerage;
                // Calculate sale volume
                SaleVolumeYear -= removeObject.Volume;
                // Calculate purchase vale
                SalePurchaseValueYear -= removeObject.PurchaseValue;
                // Calculate sale profit or loss
                SaleProfitLossYear -= removeObject.ProfitLoss;
                SaleProfitLossWithoutBrokerageYear -= removeObject.ProfitLossWithoutBrokerage;

                // Search for the remove object
                iFoundIndex = -1;
                foreach (var profitLossObject in ProfitLossListYear)
                {
                    if (profitLossObject.SaleGuid != strGuid) continue;

                    iFoundIndex = ProfitLossListYear.IndexOf(profitLossObject);
                    break;
                }
                // Save remove object
                var removeProfitLossObject = ProfitLossListYear[iFoundIndex];

                // Remove object from the list
                ProfitLossListYear.Remove(removeProfitLossObject);

#if DEBUG_SALE
                Console.WriteLine(@"SalePayoutYear: {0}", SalePayoutYear);
                Console.WriteLine(@"SalePayoutWithoutBrokerageYear: {0}", SalePayoutWithoutBrokerageYear);
                Console.WriteLine(@"SaleVolumeYear: {0}", SaleVolumeYear);
                Console.WriteLine(@"SaleProfitLossWithoutBrokerageYear: {0}", SaleProfitLossWithoutBrokerageYear);
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
        /// This function sets the document the sale object with the given Guid
        /// </summary>
        /// <param name="strGuid">Guid of the sale object which should be removed</param>
        /// <param name="strDocument">Document of the sale object</param>
        /// <returns>Flag if the set was successfully</returns>
        public bool SetDocumentSaleObject(string strGuid, string strDocument)
        {
#if DEBUG_BUY
            Console.WriteLine(@"SetDocumentSaleObject");
#endif
            try
            {
                // Search for the buy object
                foreach (var saleObject in SaleListYear)
                {
                    if (saleObject.Guid != strGuid) continue;

                    saleObject.Document = strDocument;

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

    public class SalesYearOfTheShareComparer : IComparer<SalesYearOfTheShare>
    {
        public int Compare(SalesYearOfTheShare object1, SalesYearOfTheShare object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            if (Convert.ToInt16(object2.SaleListYear) == Convert.ToInt16(object1.SaleListYear))
                return 0;
            if (Convert.ToInt16(object2.SaleListYear) > Convert.ToInt16(object1.SaleListYear))
                return 1;

            return -1;
        }
    }
}

