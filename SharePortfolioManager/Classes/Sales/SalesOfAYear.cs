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

// Define for DEBUGGING
//#define DEBUG_SALE_YEARS

using SharePortfolioManager.Classes.Brokerage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Classes.Sales
{
    [Serializable]
    public class SalesYearOfTheShare
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo SaleCultureInfo { get; internal set; }

        [Browsable(false)]
        public string SaleYearAsStr { get; internal set; } = @"-";

        [Browsable(false)]
        public decimal SaleVolumeYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SaleVolumeYearAsStr => SaleVolumeYear > 0
            ? Helper.FormatDecimal(SaleVolumeYear, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : @"";

        [Browsable(false)]
        public decimal SalePurchaseValueYear { get; internal set; } = -1;

        [Browsable(false)]
        public decimal SalePurchaseValueBrokerageYear { get; internal set; } = -1;

        [Browsable(false)]
        public decimal SalePurchaseValueReductionYear { get; internal set; } = -1;

        [Browsable(false)]
        public decimal SalePurchaseValueBrokerageReductionYear { get; internal set; } = -1;

        [Browsable(false)]
        public decimal SalePayoutYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SalePayoutYearAsStr => SalePayoutYear > 0
            ? Helper.FormatDecimal(SalePayoutYear, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : @"";

        [Browsable(false)]
        public string SalePayoutYearWithUnitAsStr => SalePayoutYear > 0
            ? Helper.FormatDecimal(SalePayoutYear, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength, true,
                @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public decimal SalePayoutBrokerageYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SalePayoutBrokerageYearUnitAsStr => SalePayoutBrokerageYear > 0
            ? Helper.FormatDecimal(SalePayoutBrokerageYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public decimal SalePayoutReductionYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SalePayoutReductionYearUnitAsStr => SalePayoutReductionYear > 0
            ? Helper.FormatDecimal(SalePayoutReductionYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public decimal SalePayoutBrokerageReductionYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SalePayoutBrokerageReductionYearUnitAsStr => SalePayoutBrokerageReductionYear > 0
            ? Helper.FormatDecimal(SalePayoutBrokerageReductionYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public decimal SaleProfitLossYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SaleProfitLossYearWithUnitAsStr => SaleProfitLossYear > 0
            ? Helper.FormatDecimal(SaleProfitLossYear, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength,
                true, @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public decimal SaleProfitLossBrokerageYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SaleProfitLossBrokerageYearWithUnitAsStr => SaleProfitLossBrokerageYear > 0
            ? Helper.FormatDecimal(SaleProfitLossBrokerageYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public decimal SaleProfitLossReductionYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SaleProfitLossReductionYearWithUnitAsStr => SaleProfitLossReductionYear > 0
            ? Helper.FormatDecimal(SaleProfitLossReductionYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public decimal SaleProfitLossBrokerageReductionYear { get; internal set; } = -1;

        [Browsable(false)]
        public string SaleProfitLossBrokerageReductionYearWithUnitAsStr => SaleProfitLossBrokerageReductionYear > 0
            ? Helper.FormatDecimal(SaleProfitLossBrokerageReductionYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"";

        [Browsable(false)]
        public List<SaleObject> SaleListYear { get; } = new List<SaleObject>();

        [Browsable(false)]
        public List<ProfitLossObject> ProfitLossListYear { get; } = new List<ProfitLossObject>();

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Year")]
        // ReSharper disable once UnusedMember.Global
        public string DgvSaleYear => SaleYearAsStr;

        [Browsable(true)]
        [DisplayName(@"YearVolume")]
        // ReSharper disable once UnusedMember.Global
        public string DgvSaleVolumeYear => SaleVolumeYear > 0
            ? Helper.FormatDecimal(SaleVolumeYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, ShareObject.PieceUnit, SaleCultureInfo)
            : @"-";
        
        [Browsable(true)]
        [DisplayName(@"YearPayout")]
        // ReSharper disable once UnusedMember.Global
        public string DgvSalePayoutYearAsStr => SalePayoutYear > 0
            ? Helper.FormatDecimal(SalePayoutYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"YearProfitLoss")]
        // ReSharper disable once UnusedMember.Global
        public string DgvSaleProfitLossYearAsStr => SaleProfitLossBrokerageReductionYear > 0
            ? Helper.FormatDecimal(SaleProfitLossBrokerageReductionYear, Helper.CurrencyTwoLength, false,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"-";

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// This functions adds a new sale object to the year list with the given values
        /// It also recalculates the value of the sale shares in the year
        /// </summary>
        /// <param name="cultureInfo">Culture info of the sale</param>
        /// <param name="strGuid">Guid of the sale</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="strDepotNumber">Depot number where the sale has been done</param>
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
        public bool AddSaleObject(CultureInfo cultureInfo,  string strGuid, string strDate, string strDepotNumber, string strOrderNumber, decimal decVolume, decimal decSalePrice, List<SaleBuyDetails> usedBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, BrokerageReductionObject brokerageObject, string strDoc = "")
        {
#if DEBUG_SALE_YEARS
            Console.WriteLine(@"AddSaleObject");
#endif
            try
            {
                // Set culture info of the share
                SaleCultureInfo = cultureInfo;

                // Create new SaleObject
                var addObject = new SaleObject(cultureInfo, strGuid, strDate, strDepotNumber, strOrderNumber, decVolume, decSalePrice, usedBuyDetails, decTaxAtSource, decCapitalGainsTax, decSolidarityTax, brokerageObject, strDoc);

                var addProfitLossObject = new ProfitLossObject(cultureInfo, strGuid, strDate, addObject.Volume,
                    addObject.ProfitLoss, addObject.ProfitLossBrokerage, addObject.ProfitLossReduction, addObject.ProfitLossBrokerageReduction,
                    strDoc);

                // Add object to the list
                SaleListYear.Add(addObject);
                SaleListYear.Sort(new SaleObjectComparer());

                // Add profit or loss object to the list
                ProfitLossListYear.Add(addProfitLossObject);
                ProfitLossListYear.Sort(new ProfitLossObjectComparer());

                // Set year
                DateTime.TryParse(strDate, out var dateTime);
                SaleYearAsStr = dateTime.Year.ToString();

                // Calculate sale volume
                if (SaleVolumeYear == -1)
                    SaleVolumeYear = 0;
                SaleVolumeYear += addObject.Volume;

                // Calculate sale purchase value
                if (SalePurchaseValueYear == -1)
                    SalePurchaseValueYear = 0;
                SalePurchaseValueYear += addObject.BuyValue;
                if (SalePurchaseValueBrokerageYear == -1)
                    SalePurchaseValueBrokerageYear = 0;
                SalePurchaseValueBrokerageYear += addObject.BuyValueBrokerage;
                if (SalePurchaseValueReductionYear == -1)
                    SalePurchaseValueReductionYear = 0;
                SalePurchaseValueReductionYear += addObject.BuyValueReduction;
                if (SalePurchaseValueBrokerageReductionYear == -1)
                    SalePurchaseValueBrokerageReductionYear = 0;
                SalePurchaseValueBrokerageReductionYear += addObject.BuyValueBrokerageReduction;

                // Calculate sale payout value
                if (SalePayoutYear == -1)
                    SalePayoutYear = 0;
                SalePayoutYear += addObject.Payout;
                if (SalePayoutBrokerageYear == -1)
                    SalePayoutBrokerageYear = 0;
                SalePayoutBrokerageYear += addObject.PayoutBrokerage;
                if (SalePayoutReductionYear == -1)
                    SalePayoutReductionYear = 0;
                SalePayoutReductionYear += addObject.PayoutReduction;
                if (SalePayoutBrokerageReductionYear == -1)
                    SalePayoutBrokerageReductionYear = 0;
                SalePayoutBrokerageReductionYear += addObject.PayoutBrokerageReduction;

                // Calculate sale profit or loss
                if (SaleProfitLossYear == -1)
                    SaleProfitLossYear = 0;
                SaleProfitLossYear += addObject.ProfitLoss;
                if (SaleProfitLossBrokerageYear == -1)
                    SaleProfitLossBrokerageYear = 0;
                SaleProfitLossBrokerageYear += addObject.ProfitLossBrokerage;
                if (SaleProfitLossReductionYear == -1)
                    SaleProfitLossReductionYear = 0;
                SaleProfitLossReductionYear += addObject.ProfitLossReduction;
                if (SaleProfitLossBrokerageReductionYear == -1)
                    SaleProfitLossBrokerageReductionYear = 0;
                SaleProfitLossBrokerageReductionYear += addObject.ProfitLossBrokerageReduction;

#if DEBUG_SALE_YEARS
                Console.WriteLine(@"SaleVolumeYear: {0}", SaleVolumeYear);
                Console.WriteLine(@"SalePurchaseValueYear: {0}", SalePurchaseValueYear);
                Console.WriteLine(@"SalePayoutYear: {0}", SalePayoutYear);
                Console.WriteLine(@"SalePayoutBrokerageYear: {0}", SalePayoutBrokerageYear);
                Console.WriteLine(@"SalePayoutReductionYear: {0}", SalePayoutReductionYear);
                Console.WriteLine(@"SalePayoutBrokerageReductionYear: {0}", SalePayoutBrokerageReductionYear);
                Console.WriteLine(@"SaleProfitLossYear: {0}", SaleProfitLossYear);
                Console.WriteLine(@"SaleProfitLossBrokerageYear: {0}", SaleProfitLossBrokerageYear);
                Console.WriteLine(@"SaleProfitLossReductionYear: {0}", SaleProfitLossReductionYear);
                Console.WriteLine(@"SaleProfitLossBrokerageReductionYear: {0}", SaleProfitLossBrokerageReductionYear);
                Console.WriteLine(@"");
#endif
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

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
#if DEBUG_SALE_YEARS
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

                // Calculate sale volume
                SaleVolumeYear -= removeObject.Volume;

                // Calculate purchase vale
                SalePurchaseValueYear -= removeObject.BuyValue;

                // Calculate sale payout value
                SalePayoutYear -= removeObject.Payout;
                SalePayoutBrokerageYear -= removeObject.PayoutBrokerage;
                SalePayoutReductionYear -= removeObject.PayoutReduction;
                SalePayoutBrokerageReductionYear -= removeObject.PayoutBrokerageReduction;

                // Calculate sale profit or loss
                SaleProfitLossYear -= removeObject.ProfitLoss;
                SaleProfitLossBrokerageYear -= removeObject.ProfitLossBrokerage;
                SaleProfitLossReductionYear -= removeObject.ProfitLossReduction;
                SaleProfitLossBrokerageReductionYear -= removeObject.ProfitLossBrokerageReduction;

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

#if DEBUG_SALE_YEARS
                Console.WriteLine(@"SalePayoutYear: {0}", SalePayoutYear);
                Console.WriteLine(@"SalePayoutWithoutBrokerageYear: {0}", SalePayoutWithoutBrokerageYear);
                Console.WriteLine(@"SaleVolumeYear: {0}", SaleVolumeYear);
                Console.WriteLine(@"SaleProfitLossWithoutBrokerageYear: {0}", SaleProfitLossWithoutBrokerageYear);
                Console.WriteLine(@"");
#endif
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

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
#if DEBUG_SALE_YEARS
            Console.WriteLine(@"SetDocumentSaleObject");
#endif
            try
            {
                // Search for the buy object
                foreach (var saleObject in SaleListYear)
                {
                    if (saleObject.Guid != strGuid) continue;

                    saleObject.DocumentAsStr = strDocument;

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

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

