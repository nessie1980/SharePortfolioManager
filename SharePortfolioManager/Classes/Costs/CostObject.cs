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
using System.ComponentModel;
using System.Globalization;

namespace SharePortfolioManager
{
    [Serializable]
    public class BrokerageObject
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo BrokerageCultureInfo { get; internal set; }

        [Browsable(false)]
        [DisplayName(@"Guid")]
        public string Guid { get; internal set; }

        [Browsable(false)]
        [DisplayName(@"GuidBuySale")]
        public string GuidBuySale { get; internal set; }

        [Browsable(false)]
        public bool BrokerageOfABuy { get; set; }

        [Browsable(false)]
        public bool BrokerageOfASale { get; set; }

        [Browsable(false)]
        public string BrokerageOfABuyAsStr => BrokerageOfABuy.ToString();

        [Browsable(false)]
        public string BrokerageOfASaleAsStr => BrokerageOfASale.ToString();

        [Browsable(false)]
        public string BrokerageDate { get; set; }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string BrokerageDateAsStr => BrokerageDate;

        [Browsable(false)]
        public decimal BrokerageValue { get; set; }

        [Browsable(true)]
        [DisplayName(@"Value")]
        public string BrokerageValueAsStr => Helper.FormatDecimal(BrokerageValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", BrokerageCultureInfo);

        [Browsable(false)]
        public string BrokerageDocument { get; set; }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string BrokerageDocumentFileName => Helper.GetFileName(BrokerageDocument);

        #endregion Properties

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
        /// <param name="decValue">Value of the brokerage</param>
        /// <param name="strDoc">Document of the brokerage</param>
        public BrokerageObject(string strGuid, bool bBrokerageOfABuy, bool bBrokerageOfASale, CultureInfo cultureInfo, string strGuidBuySale, string strDate, decimal decValue, string strDoc = "")
        {
            Guid = strGuid;
            GuidBuySale = strGuidBuySale;
            BrokerageOfABuy = bBrokerageOfABuy;
            BrokerageOfASale = bBrokerageOfASale;
            BrokerageCultureInfo = cultureInfo;
            BrokerageDate = strDate;
            BrokerageValue = decValue;
            BrokerageDocument = strDoc;

#if DEBUG_BROKERAGE
            Console.WriteLine(@"");
            Console.WriteLine(@"New BrokerageObject created");
            Console.WriteLine(@"Guid: {0}", strGuid);
            Console.WriteLine(@"BrokerageOfABuy: {0}", BrokerageOfABuy);
            Console.WriteLine(@"bBrokerageOfASale: {0}", bBrokerageOfASale);
            Console.WriteLine(@"Date: {0}", strDate);
            Console.WriteLine(@"Value: {0}", decValue);
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
    public class BrokerageObjectComparer : IComparer<BrokerageObject>
    {
        #region Methods

        public int Compare(BrokerageObject brokerageObject1, BrokerageObject brokerageObject2)
        {
            if (brokerageObject1 == null) return 0;
            if (brokerageObject2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(brokerageObject1.BrokerageDate), Convert.ToDateTime(brokerageObject2.BrokerageDate));
        }

        #endregion Methods
    }

}
