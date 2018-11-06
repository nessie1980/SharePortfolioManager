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
using System.Drawing;
using System.Globalization;
using SharePortfolioManager.Properties;

namespace SharePortfolioManager
{
    [Serializable]
    public class BuyObject
    {
        #region Variables

        /// <summary>
        /// Stores the buy volume
        /// </summary>
        private decimal _volume = -1;

        /// <summary>
        /// Stores the alreday sold volume of the share
        /// </summary>
        private decimal _volumeAlreadySold = -1;

        /// <summary>
        /// Stores the price of the share
        /// </summary>
        private decimal _sharePrice = -1;

        /// <summary>
        /// Stores the discount value of the buy
        /// </summary>
        private decimal _reduction = -1;

        /// <summary>
        /// Stores the brokerage value of the buy
        /// </summary>
        private decimal _brokerage = -1;

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo BuyCultureInfo { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Guid")]
        public string Guid { get; internal set; }

        [Browsable(false)]
        public string Date { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Date")]
        public string DateAsStr => DateTime.Parse(Date).Date.ToShortDateString();

        [Browsable(false)]
        public decimal Volume
        {
            get => _volume;
            internal set
            {
                if (_volume == value)
                    return;
                _volume = value;

                // Calculate the values
                CalculateMarketValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Volume")]
        public string VolumeAsStr => Helper.FormatDecimal(Volume, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal VolumeSold
        {
            get => _volumeAlreadySold;
            internal set
            {
                if (_volumeAlreadySold == value)
                    return;
                _volumeAlreadySold = value;

                // Calculate the values
                CalculateMarketValues();
            }
        }

        [Browsable(false)]
        public string VolumeSoldAsStr => Helper.FormatDecimal(VolumeSold, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal SharePrice
        {
            get => _sharePrice;
            internal set
            {
                if (_sharePrice == value)
                    return;
                _sharePrice = value;

                // Calculate the values
                CalculateMarketValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Price")]
        public string SharePriceAsStr => Helper.FormatDecimal(SharePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal Brokerage
        {
            get => _brokerage;
            internal set
            {
                if (_brokerage == value)
                    return;
                _brokerage = value;

                // Calculate the market values
                CalculateMarketValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Brokerage")]
        public string BrokerageAsStr => Helper.FormatDecimal(Brokerage, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal Reduction
        {
            get => _reduction;
            internal set
            {
                if (_reduction == value)
                    return;
                _reduction = value;

                // Calculate the values
                CalculateMarketValues();
            }
        }

        [Browsable(true)]
        [DisplayName(@"Reduction")]
        public string ReductionAsStr => Helper.FormatDecimal(Reduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal BuyValue { get; internal set; } = -1;

        [Browsable(false)]
        public decimal BuyValueReduction { get; internal set; } = -1;

        [Browsable(false)]
        public decimal BuyValueReductionBrokerage { get; internal set; } = -1;

        [Browsable(false)]
        public string MarketValueReductionBrokerageAsStr => Helper.FormatDecimal(BuyValueReductionBrokerage, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string Document { get; internal set; }

        [Browsable(true)]
        [DisplayName(@"Document")]
        public Image DocumentGrid => Document == @"-" ? null : Resources.black_logger;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strGuid">Guid of the share buy</param>
        /// <param name="strDate">Date of the share buy</param>
        /// <param name="decVolume">Volume of the bought shares</param>
        /// <param name="decVolumeSold">Volume of the bought shares which already sold</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="decReduction">Reduction of the buy</param>
        /// <param name="decBrokerage">Brokerage of the buy</param>
        /// <param name="strDoc">Document of the share buy</param>
        public BuyObject(CultureInfo cultureInfo, string strGuid, string strDate, decimal decVolume, decimal decVolumeSold, decimal decSharePrice, decimal decReduction, decimal decBrokerage, string strDoc = "")
        {
            Guid = strGuid;
            BuyCultureInfo = cultureInfo;
            Date = strDate;
            Volume = decVolume;
            VolumeSold = decVolumeSold;
            Reduction = decReduction;
            Brokerage = decBrokerage;
            SharePrice = decSharePrice;
            Document = strDoc;

#if DEBUG_BUY
            Console.WriteLine(@"");
            Console.WriteLine(@"New buy created");
            Console.WriteLine(@"Guid: {0}", Guid);
            Console.WriteLine(@"Date: {0}", Date);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"SharePrice: {0}", SharePrice);
            Console.WriteLine(@"MarketValue: {0}", BuyValue);
            Console.WriteLine(@"Reduction: {0}", Reduction);
            Console.WriteLine(@"Brokerage: {0}", Brokerage);
            Console.WriteLine(@"BrokerageReduction: {0}", Brokerage);
            Console.WriteLine(@"Document: {0}", Document);
            Console.WriteLine(@"");
#endif
        }

        /// <summary>
        /// This function calculates four values.
        /// - the market value
        /// - the market value with reduction
        /// - the market value with reduction and brokerage
        /// - the brokerage value
        /// with the given volume and share price of the buy
        /// </summary>
        private void CalculateMarketValues()
        {
            Helper.CalcBuyValues( Volume, SharePrice, Brokerage,
                Reduction, out var decMarketValue, out var decMarketValueReduction, out var decMarketValueReductionBrokerage, out var decBrokerageReduction);

            BuyValue = decMarketValue;
            BuyValueReduction = decMarketValueReduction;
            BuyValueReductionBrokerage = decMarketValueReductionBrokerage;
        }

        /// <summary>
        /// This function checks if the buy is sold complete or not
        /// </summary>
        /// <returns>Flag if all shares have been sold</returns>
        public bool HasSharesForSale()
        {
            return _volume > _volumeAlreadySold;
        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This is the comparer class for the BuyObject.
    /// It is used for the sort for the buys lists.
    /// </summary>
    public class BuyObjectComparer : IComparer<BuyObject>
    {
        #region Methods

        public int Compare(BuyObject object1, BuyObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }

}
