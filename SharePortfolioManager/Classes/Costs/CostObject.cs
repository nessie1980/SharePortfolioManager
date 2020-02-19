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
using System.Drawing;
using System.Globalization;

namespace SharePortfolioManager.Classes.Costs
{
    [Serializable]
    public class BrokerageReductionObject
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo CultureInfo { get; internal set; }

        [Browsable(false)]
        public string Guid { get; internal set; }

        [Browsable(false)]
        public string GuidBuySale { get; internal set; }

        [Browsable(false)]
        public bool PartOfABuy { get; set; }

        [Browsable(false)]
        public bool PartOfASale { get; set; }

        [Browsable(false)]
        public string PartOfABuyAsStr => PartOfABuy.ToString();

        [Browsable(false)]
        public string PartOfASaleAsStr => PartOfASale.ToString();

        [Browsable(false)]
        public string Date { get; set; }

        [Browsable(false)]
        public string DateAsStr => Date;

        [Browsable(false)]
        public decimal ProvisionValue { get; set; }

        [Browsable(false)]
        public string ProvisionValueAsStr => Helper.FormatDecimal(ProvisionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        [Browsable(false)]
        public decimal BrokerFeeValue { get; set; }

        [Browsable(false)]
        public string BrokerFeeValueAsStr => Helper.FormatDecimal(BrokerFeeValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        [Browsable(false)]
        public decimal TraderPlaceFeeValue { get; set; }

        [Browsable(false)]
        public string TraderPlaceFeeValueAsStr => Helper.FormatDecimal(TraderPlaceFeeValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        [Browsable(false)]
        public decimal ReductionValue { get; set; }

        [Browsable(false)]
        public string ReductionValueAsStr => Helper.FormatDecimal(ReductionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        [Browsable(false)]
        public decimal BrokerageValue { get; set; }

        [Browsable(false)]
        public string BrokerageValueAsStr => Helper.FormatDecimal(BrokerageValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        [Browsable(false)]
        public decimal BrokerageReductionValue { get; set; }

        [Browsable(false)]
        public string BrokerageReductionValueAsStr => Helper.FormatDecimal(BrokerageReductionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        [Browsable(false)]
        public string BrokerageDocument { get; set; }

        [Browsable(false)]
        public string BrokerageDocumentFileName => Helper.GetFileName(BrokerageDocument);

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        public string DgvGuid => Guid;

        [Browsable(true)]
        public string DgvBrokerageDate => DateAsStr;

        [Browsable(true)]
        public string DgvBrokerageReductionValueAsStr => BrokerageReductionValueAsStr;

        [Browsable(true)]
        public Image DocumentGrid => Helper.GetImageForFile(BrokerageDocument);


        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="strGuid">Guid of the share brokerage</param>
        /// <param name="bBrokerageOfABuy">Flag if the brokerage is part of a share buy</param>
        /// <param name="bBrokerageOfASale">Flag if the brokerage is part of a share sale</param>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strGuidBuySale">Guid of the buy or sale</param>
        /// <param name="strDate">Date of the brokerage pay</param>
        /// <param name="decProvisionValue">Provision of the brokerage</param>
        /// <param name="decBrokerFeeValue">Broker fee of the brokerage</param>
        /// <param name="decTraderPlaceFeeValue">Trader place fee of the brokerage</param>
        /// <param name="decReductionValue">Reduction of the brokerage</param>
        /// <param name="strDoc">Document of the brokerage</param>
        public BrokerageReductionObject(string strGuid, bool bBrokerageOfABuy, bool bBrokerageOfASale, CultureInfo cultureInfo, string strGuidBuySale,
            string strDate, decimal decProvisionValue, decimal decBrokerFeeValue, decimal decTraderPlaceFeeValue, decimal decReductionValue, string strDoc = "")
        {
            Guid = strGuid;
            GuidBuySale = strGuidBuySale;
            PartOfABuy = bBrokerageOfABuy;
            PartOfASale = bBrokerageOfASale;
            CultureInfo = cultureInfo;
            Date = strDate;
            ProvisionValue = decProvisionValue;
            BrokerFeeValue = decBrokerFeeValue;
            TraderPlaceFeeValue = decTraderPlaceFeeValue;
            ReductionValue = decReductionValue;
            BrokerageDocument = strDoc;

            // Calculate and set brokerage value
            Helper.CalcBrokerageValues(decProvisionValue, decBrokerFeeValue, decTraderPlaceFeeValue, ReductionValue, out var brokerageValue, out var brokerageWithReductionValue);
            BrokerageValue = brokerageValue;
            BrokerageReductionValue = brokerageWithReductionValue;

#if DEBUG_BROKERAGE
            Console.WriteLine(@"");
            Console.WriteLine(@"New BrokerageObject created");
            Console.WriteLine(@"Guid: {0}", strGuid);
            Console.WriteLine(@"BrokerageOfABuy: {0}", BrokerageOfABuy);
            Console.WriteLine(@"bBrokerageOfASale: {0}", bBrokerageOfASale);
            Console.WriteLine(@"Date: {0}", strDate);
            Console.WriteLine(@"Provision: {0}", decProvisionValue);
            Console.WriteLine(@"BrokerFeeValue: {0}", decBrokerFeeValue);
            Console.WriteLine(@"TraderPlaceFeeValue: {0}", decTraderPlaceFeeValue);
            Console.WriteLine(@"ReductionValue: {0}", decReductionValue);
            Console.WriteLine(@"Brokerage: {0}", BrokerageValue);
            Console.WriteLine(@"brokerageWithReductionValue: {0}", brokerageWithReductionValue);
            Console.WriteLine(@"Document: {0}", strDoc);
            Console.WriteLine(@"");
#endif
        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This is the comparer class for the BrokerageObject.
    /// It is used for the sort for the brokerage lists.
    /// </summary>
    public class BrokerageReductionObjectComparer : IComparer<BrokerageReductionObject>
    {
        #region Methods

        public int Compare(BrokerageReductionObject brokerageReductionObject1, BrokerageReductionObject brokerageReductionObject2)
        {
            if (brokerageReductionObject1 == null) return 0;
            if (brokerageReductionObject2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(brokerageReductionObject1.Date), Convert.ToDateTime(brokerageReductionObject2.Date));
        }

        #endregion Methods
    }

}
