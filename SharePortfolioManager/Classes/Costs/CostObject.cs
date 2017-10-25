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
using System.ComponentModel;
using System.Globalization;

namespace SharePortfolioManager
{
    public class CostObject
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _costCultureInfo;

        /// <summary>
        /// Flag if the cost is part of a share buy
        /// </summary>
        private bool _costOfABuy;

        /// <summary>
        /// Flag if the cost is part of a share sale
        /// </summary>
        private bool _costOfASale;

        /// <summary>
        /// Stores the date string of the cost pay date
        /// </summary>
        private string _costDate;

        /// <summary>
        /// Stores the cost value
        /// </summary>
        private decimal _costValue = -1;

        /// <summary>
        /// Stores the document of the cost
        /// </summary>
        private string _costDocument;

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo CostCultureInfo
        {
            get { return _costCultureInfo; }
            internal set { _costCultureInfo = value; }
        }

        [Browsable(false)]
        public bool CostOfABuy
        {
            get { return _costOfABuy; }
            set { _costOfABuy = value; }
        }

        [Browsable(false)]
        public bool CostOfASale
        {
            get { return _costOfASale; }
            set { _costOfASale = value; }
        }

        [Browsable(false)]
        public string CostOfABuyAsStr
        {
            get { return _costOfABuy.ToString(); }
        }

        [Browsable(false)]
        public string CostOfASaleAsStr
        {
            get { return _costOfASale.ToString(); }
        }

        [Browsable(false)]
        public string CostDate
        {
            get { return _costDate; }
            set { _costDate = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string CostDateAsStr
        {
            get { return _costDate; }
        }

        [Browsable(false)]
        public decimal CostValue
        {
            get { return _costValue; }
            set { _costValue = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Value")]
        public string CostValueAsStr
        {
            get { return Helper.FormatDecimal(_costValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", CostCultureInfo); }
        }

        [Browsable(false)]
        public string CostValueWithUnitAsStr
        {
            get { return Helper.FormatDecimal(_costValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, true, @"", CostCultureInfo); }
        }

        [Browsable(false)]
        public string CostDocument
        {
            get { return _costDocument; }
            set { _costDocument = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string CostDocumentFileName
        {
            get { return Helper.GetFileName(_costDocument); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="bCostOfABuy">Flag if the cost is part of a share buy</param>
        /// <param name="bCostOfASale">Flag if the cost is part of a share sale</param>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strDate">Date of the cost pay</param>
        /// <param name="decValue">Value of the cost</param>
        /// <param name="strDoc">Document of the cost</param>
        public CostObject(bool bCostOfABuy, bool bCostOfASale, CultureInfo cultureInfo, string strDate, decimal decValue, string strDoc = "")
        {
            CostOfABuy = bCostOfABuy;
            CostOfASale = bCostOfASale;
            CostCultureInfo = cultureInfo;
            CostDate = strDate;
            CostValue = decValue;
            CostDocument = strDoc;

#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"New CostObject created");
            Console.WriteLine(@"CostOfABuy: {0}", bCostOfABuy);
            Console.WriteLine(@"CostOfASale: {0}", bCostOfASale);
            Console.WriteLine(@"Date: {0}", strDate);
            Console.WriteLine(@"Value: {0}", decValue);
            Console.WriteLine(@"Document: {0}", strDoc);
            Console.WriteLine(@"");
#endif

        }

        #endregion Methods
    }

    /// <summary>
    /// This is the comparer class for the CostObject.
    /// It is used for the sort for the cost lists.
    /// </summary>
    public class CostObjectComparer : IComparer<CostObject>
    {
        #region Methods

        public int Compare(CostObject costObject1, CostObject costObject2)
        {
            return DateTime.Compare(Convert.ToDateTime(costObject1.CostDate), Convert.ToDateTime(costObject2.CostDate));
        }

        #endregion Methods
    }

}
