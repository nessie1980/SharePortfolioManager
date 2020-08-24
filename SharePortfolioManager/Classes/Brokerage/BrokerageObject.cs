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
//#define DEBUG_BROKERAGE_OBJECT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace SharePortfolioManager.Classes.Brokerage
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
        public string DateAsStr => DateTime.Parse(Date).Date.ToShortDateString();

        [Browsable(false)]
        public decimal ProvisionValue { get; set; }

        [Browsable(false)]
        public string ProvisionValueAsStr => ProvisionValue > 0
            ? Helper.FormatDecimal(ProvisionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength);

        [Browsable(false)]
        public decimal BrokerFeeValue { get; set; }

        [Browsable(false)]
        public string BrokerFeeValueAsStr => BrokerFeeValue > 0
            ? Helper.FormatDecimal(BrokerFeeValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength);

        [Browsable(false)]
        public decimal TraderPlaceFeeValue { get; set; }

        [Browsable(false)]
        public string TraderPlaceFeeValueAsStr => TraderPlaceFeeValue > 0
            ? Helper.FormatDecimal(TraderPlaceFeeValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength);

        [Browsable(false)]
        public decimal ReductionValue { get; set; }

        [Browsable(false)]
        public string ReductionValueAsStr => ReductionValue > 0
            ? Helper.FormatDecimal(ReductionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength);

        [Browsable(false)]
        public decimal BrokerageValue { get; set; }

        [Browsable(false)]
        public string BrokerageValueAsStr => BrokerageValue > 0
            ? Helper.FormatDecimal(BrokerageValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength);

        [Browsable(false)]
        public decimal BrokerageReductionValue { get; set; }

        [Browsable(false)]
        public string BrokerageReductionValueAsStr => BrokerageReductionValue > 0
            ? Helper.FormatDecimal(BrokerageReductionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength);

        [Browsable(false)]
        public string DocumentAsStr { get; set; }

        [Browsable(false)]
        public string BrokerageDocumentFileName => Helper.GetFileName(DocumentAsStr);

        [Browsable(false)]
        public Image DocumentGridImage => Helper.GetImageForFile(DocumentAsStr);

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Guid")]
        // ReSharper disable once UnusedMember.Global
        public string DgvGuid => Guid;

        [Browsable(true)]
        [DisplayName(@"Date")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBrokerageDate => DateAsStr;

        [Browsable(true)]
        [DisplayName(@"Brokerage")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBrokerageValueAsStr => BrokerageValue > 0
            ? Helper.FormatDecimal(BrokerageValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true,
                @"", CultureInfo)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true,
                @"", CultureInfo);

        [Browsable(true)]
        [DisplayName(@"Reduction")]
        // ReSharper disable once UnusedMember.Global
        public string DgvReductionValueAsStr => ReductionValue > 0
            ? Helper.FormatDecimal(ReductionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true,
                @"", CultureInfo)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true,
                @"", CultureInfo);

        [Browsable(true)]
        [DisplayName(@"Broker. - Red.")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBrokerageReductionValueAsStr => BrokerageReductionValue > 0
            ? Helper.FormatDecimal(BrokerageReductionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true,
                @"", CultureInfo)
            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true,
                @"", CultureInfo);

        [Browsable(true)]
        [DisplayName(@"Document")]
        // ReSharper disable once UnusedMember.Global
        public Image DgvDocumentGrid => DocumentGridImage;


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
            DocumentAsStr = strDoc;

            // Calculate and set brokerage value
            Helper.CalcBrokerageValues(decProvisionValue, decBrokerFeeValue, decTraderPlaceFeeValue, decReductionValue,
                out var brokerageValue, out var brokerageReduction);
            BrokerageValue = brokerageValue;
            BrokerageReductionValue = brokerageReduction;

#if DEBUG_BROKERAGE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"BrokerageReductionObject()");
            Console.WriteLine(@"Guid: {0}", strGuid);
            Console.WriteLine(@"bBrokerageOfABuy: {0}", bBrokerageOfABuy);
            Console.WriteLine(@"bBrokerageOfASale: {0}", bBrokerageOfASale);
            Console.WriteLine(@"Date: {0}", strDate);
            Console.WriteLine(@"Provision: {0}", decProvisionValue);
            Console.WriteLine(@"BrokerFeeValue: {0}", decBrokerFeeValue);
            Console.WriteLine(@"TraderPlaceFeeValue: {0}", decTraderPlaceFeeValue);
            Console.WriteLine(@"ReductionValue: {0}", decReductionValue);
            Console.WriteLine(@"Brokerage: {0}", BrokerageValue);
            Console.WriteLine(@"BrokerageReductionValue: {0}", BrokerageReductionValue);
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
