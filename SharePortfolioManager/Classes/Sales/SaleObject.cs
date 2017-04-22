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
using System.ComponentModel;
using System.Globalization;
using SharePortfolioManager.Classes;

namespace SharePortfolioManager
{
    public class SaleObject
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the date string of the sale date
        /// </summary>
        private string _saleDate;

        /// <summary>
        /// Stores the sale volume
        /// </summary>
        private decimal _saleVolume = -1;

        /// <summary>
        /// Stores the price of the share
        /// </summary>
        private decimal _salePrice = -1;

        /// <summary>
        /// Stores the value of the sale
        /// </summary>
        private decimal _saleValue = -1;

        /// <summary>
        /// Stores the profit or loss of the sale
        /// </summary>
        private decimal _saleProfitLoss = -1;

        /// <summary>
        /// Stores the document of the sale
        /// </summary>
        private string _saleDocument = "";

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            internal set { _cultureInfo = value; }
        }

        [Browsable(false)]
        public string SaleDate
        {
            get { return _saleDate; }
            internal set { _saleDate = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string SaleDateAsString
        {
            get { return _saleDate; }
        }

        [Browsable(false)]
        public decimal SaleValue
        {
            get { return _saleValue; }
            internal set
            {
                _saleValue = value;
                if (_saleVolume > -1 && _saleValue > -1)
                {
                    // Calculate the sale price
                    CalculatePrice();
                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"Value")]
        public string SaleValueAsString
        {
            get { return Helper.FormatDecimal(_saleValue, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string SaleValueWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleValue, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal SalePrice
        {
            get { return _salePrice; }
            internal set
            {
                _salePrice = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"Price")]
        public string SalePriceAsString
        {
            get { return Helper.FormatDecimal(_salePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string SalePriceWithUnitAsString
        {
            get { return Helper.FormatDecimal(_salePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", CultureInfo); }
        }

        [Browsable(false)]
        public decimal SaleProfitLoss
        {
            get { return _saleProfitLoss; }
            internal set
            {
                _saleProfitLoss = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"ProfitLoss")]
        public string SaleProfitLossAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string SaleProfitLossWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, CultureInfo); }
        }

        [Browsable(false)]
        public decimal SaleVolume
        {
            get { return _saleVolume; }
            internal set
            {
                _saleVolume = value;
                if (_saleVolume > -1 && _saleValue > -1)
                {
                    // Calculate the sale price
                    CalculatePrice();
                }
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string SaleVolumeAsString
        {
            get { return Helper.FormatDecimal(_saleVolume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string SaleVolumeWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleVolume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PieceUnit, CultureInfo); }
        }

        [Browsable(false)]
        public string SaleDocument
        {
            get { return _saleDocument; }
            internal set
            {
                _saleDocument = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string SaleDocumentFileName
        {
            get { return Helper.GetFileName(_saleDocument); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decValue">Value of the sale</param>
        /// <param name="decProftiLoss">Value of the profit or loss</param>
        /// <param name="strDoc">Document of the sale</param>
        public SaleObject(CultureInfo cultureInfo, string strDate, decimal decVolume, decimal decValue, decimal decProftiLoss, string strDoc = "")
        {
            CultureInfo = cultureInfo;
            SaleDate = strDate;
            SaleVolume = decVolume;
            SaleValue = decValue;
            SaleProfitLoss = decProftiLoss;
            SaleDocument = strDoc;

#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"New sale created");
            Console.WriteLine(@"Date: {0}", SaleDate);
            Console.WriteLine(@"Volume: {0}", SaleVolume);
            Console.WriteLine(@"Value: {0}", SaleValue);
            Console.WriteLine(@"ProfitLoss: {0}", SaleProfitLoss);
            Console.WriteLine(@"Document: {0}", SaleDocument);
            Console.WriteLine(@"");
#endif

        }

        /// <summary>
        /// This function calculates the price at the sale day
        /// with the sale value and the volume
        /// </summary>
        private void CalculatePrice()
        {
            _salePrice = _saleValue / _saleVolume;
        }

        #endregion Methods
    }

    /// <summary>
    /// This is the comparer class for the SaleObject.
    /// It is used for the sort for the sales lists.
    /// </summary>
    public class SaleObjectComparer : IComparer<SaleObject>
    {
        #region Methods

        public int Compare(SaleObject object1, SaleObject object2)
        {
            return DateTime.Compare(Convert.ToDateTime(object1.SaleDate), Convert.ToDateTime(object2.SaleDate));
        }

        #endregion Methods
    }

    public class ProfitLossObject
    {
        #region Variables

        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Stores the date string of the sale date
        /// </summary>
        private string _saleDate;

        /// <summary>
        /// Stores the profit or loss of the sale
        /// </summary>
        private decimal _saleProfitLoss = -1;

        /// <summary>
        /// Stores the document of the sale
        /// </summary>
        private string _saleDocument = "";

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            internal set { _cultureInfo = value; }
        }

        [Browsable(false)]
        public string SaleDate
        {
            get { return _saleDate; }
            internal set { _saleDate = value; }
        }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string SaleDateFormatted
        {
            get { return _saleDate; }
        }

        [Browsable(false)]
        public decimal SaleProfitLoss
        {
            get { return _saleProfitLoss; }
            internal set
            {
                _saleProfitLoss = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"ProfitLoss")]
        public string SaleProfitLossAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", CultureInfo); }
        }

        [Browsable(false)]
        public string SaleProfitLossWithUnitAsString
        {
            get { return Helper.FormatDecimal(_saleProfitLoss, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, ShareObject.PercentageUnit, CultureInfo); }
        }

        [Browsable(false)]
        public string SaleDocument
        {
            get { return _saleDocument; }
            internal set
            {
                _saleDocument = value;
            }
        }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public string SaleDocumentFormatted
        {
            get { return Helper.GetFileName(_saleDocument); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decProftiLoss">Value of the profit or loss</param>
        /// <param name="strDoc">Document of the sale</param>
        public ProfitLossObject(CultureInfo cultureInfo, string strDate, decimal decProftiLoss, string strDoc = "")
        {
            CultureInfo = cultureInfo;
            SaleDate = strDate;
            SaleProfitLoss = decProftiLoss;
            SaleDocument = strDoc;

#if DEBUG
            Console.WriteLine(@"");
            Console.WriteLine(@"New sale created");
            Console.WriteLine(@"Date: {0}", SaleDate);
            Console.WriteLine(@"ProfitLoss: {0}", SaleProfitLoss);
            Console.WriteLine(@"Document: {0}", SaleDocument);
            Console.WriteLine(@"");
#endif

        }

        #endregion Methods
    }

    /// <summary>
    /// This is the comparer class for the ProfitLossObject.
    /// It is used for the sort for the profit or loss lists.
    /// </summary
    public class ProfitLossObjectComparer : IComparer<ProfitLossObject>
    {
        #region Methods

        public int Compare(ProfitLossObject object1, ProfitLossObject object2)
        {
            return DateTime.Compare(Convert.ToDateTime(object1.SaleDate), Convert.ToDateTime(object2.SaleDate));
        }

        #endregion Methods
    }

}

