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
using System.Drawing;
using System.Globalization;
using SharePortfolioManager.Classes.Costs;

namespace SharePortfolioManager.Classes.Buys
{
    [Serializable]
    public class BuyObject
    {
        #region Variables

        /// <summary>
        /// Stores the buy volume
        /// </summary>
        private decimal _volume;

        /// <summary>
        /// Stores the already sold volume of the share
        /// </summary>
        private decimal _volumeAlreadySold;

        /// <summary>
        /// Stores the price of the share
        /// </summary>
        private decimal _sharePrice;

        #region Brokerage values

        /// <summary>
        /// Stores the Guid of the brokerage value of the brokerage object
        /// </summary>
        private string _brokerageGuid = @"";

        /// <summary>
        /// Stores the provision value of the buy
        /// </summary>
        private decimal _provision;

        /// <summary>
        /// Stores the broker fee value of the buy
        /// </summary>
        private decimal _brokerFee;

        /// <summary>
        /// Stores the trader place fee value of the buy
        /// </summary>
        private decimal _traderPlaceFee;

        /// <summary>
        /// Stores the reduction value of the buy
        /// </summary>
        private decimal _reduction;

        /// <summary>
        /// Stores the brokerage value of the buy
        /// </summary>
        private decimal _brokerage;

        /// <summary>
        /// Stores the brokerage minus reduction value of the buy
        /// </summary>
        private decimal _brokerageWithReduction;

        #endregion Brokerage values

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo BuyCultureInfo { get; internal set; }

        [Browsable(false)]
        public string Guid { get; internal set; }

        [Browsable(false)]
        public string OrderNumber { get; internal set; }

        [Browsable(false)]
        public string Date { get; internal set; }

        [Browsable(false)]
        public string DateAsStr => DateTime.Parse(Date).Date.ToShortDateString();

        [Browsable(false)]
        public decimal Volume
        {
            get => _volume;
            internal set
            {
                if (_volume.Equals(value))
                    return;
                _volume = value;
            }
        }

        [Browsable(false)]
        public string VolumeAsStr => Helper.FormatDecimal(Volume, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal VolumeSold
        {
            get => _volumeAlreadySold;
            internal set
            {
                if (_volumeAlreadySold.Equals(value))
                    return;
                _volumeAlreadySold = value;
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
                if (_sharePrice.Equals(value))
                    return;
                _sharePrice = value;
            }
        }

        [Browsable(false)]
        public string SharePriceAsStr => Helper.FormatDecimal(SharePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        #region Brokerage values

        [Browsable(false)]
        public string BrokerageGuid
        {
            get => _brokerageGuid;
            internal set
            {
                if (_brokerageGuid.Equals(value))
                    return;
                _brokerageGuid = value;
            }
        }

        [Browsable(false)]
        public decimal Provision
        {
            get => _provision;
            internal set
            {
                if (_provision.Equals(value))
                    return;
                _provision = value;
            }
        }

        [Browsable(false)]
        public string ProvisionAsStr => Helper.FormatDecimal(Provision, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal BrokerFee
        {
            get => _brokerFee;
            internal set
            {
                if (_brokerFee.Equals(value))
                    return;
                _brokerFee = value;
            }
        }

        [Browsable(false)]
        public string BrokerFeeAsStr => Helper.FormatDecimal(BrokerFee, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal TraderPlaceFee
        {
            get => _traderPlaceFee;
            internal set
            {
                if (_traderPlaceFee.Equals(value))
                    return;
                _traderPlaceFee = value;
            }
        }

        [Browsable(false)]
        public string TraderPlaceFeeAsStr => Helper.FormatDecimal(TraderPlaceFee, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal Reduction
        {
            get => _reduction;
            internal set
            {
                if (_reduction.Equals(value))
                    return;
                _reduction = value;
            }
        }

        [Browsable(false)]
        public string ReductionAsStr => Helper.FormatDecimal(Reduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal Brokerage
        {
            get => _brokerage;
            internal set
            {
                if (_brokerage.Equals(value))
                    return;
                _brokerage = value;
            }
        }

        [Browsable(false)]
        public string BrokerageAsStr => Helper.FormatDecimal(Brokerage, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public decimal BrokerageWithReduction
        {
            get => _brokerageWithReduction;
            internal set
            {
                if (_brokerageWithReduction.Equals(value))
                    return;
                _brokerageWithReduction = value;
            }
        }

        [Browsable(false)]
        public string BrokerageWithReductionAsStr => Helper.FormatDecimal(BrokerageWithReduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        #endregion Brokerage values

        [Browsable(false)]
        public decimal BuyValue { get; internal set; } = -1;

        [Browsable(false)]
        public decimal BuyValueBrokerage { get; internal set; } = -1;

        [Browsable(false)]
        public string BuyValueBrokerageAsStr => Helper.FormatDecimal(BuyValueBrokerage, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", BuyCultureInfo);

        [Browsable(false)]
        public string Document { get; internal set; }

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Guid")]
        // ReSharper disable once UnusedMember.Global
        public string DgvGuid => Guid;

        [Browsable(true)]
        [DisplayName(@"Date")]
        // ReSharper disable once UnusedMember.Global
        public string DgvDateAsStr => DateAsStr;

        [Browsable(true)]
        [DisplayName(@"Volume")]
        // ReSharper disable once UnusedMember.Global
        public string DgvVolumeAsStr => VolumeAsStr;

        [Browsable(true)]
        [DisplayName(@"Price")]
        // ReSharper disable once UnusedMember.Global
        public string DgvSharePriceAsStr => SharePriceAsStr;

        [Browsable(true)]
        [DisplayName(@"Brokerage")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBrokerageWithReductionAsStr => BrokerageWithReductionAsStr;

        [Browsable(true)]
        [DisplayName(@"Deposit")]
        // ReSharper disable once UnusedMember.Global
        public string DgvDepositAsStr => BuyValueBrokerageAsStr;

        [Browsable(true)]
        [DisplayName(@"Document")]
        // ReSharper disable once UnusedMember.Global
        public Image DocumentGrid => Helper.GetImageForFile( Document);

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strGuid">Guid of the share buy</param>
        /// <param name="strOrderNumber">Order number of the share buy</param>
        /// <param name="strDate">Date of the share buy</param>
        /// <param name="decVolume">Volume of the bought shares</param>
        /// <param name="decVolumeSold">Volume of the bought shares which already sold</param>
        /// <param name="decSharePrice">Price for one share</param>
        /// <param name="brokerageObject">Brokerage of the buy</param>
        /// <param name="strDoc">Document of the share buy</param>
        public BuyObject(CultureInfo cultureInfo, string strGuid, string strOrderNumber, string strDate, decimal decVolume, decimal decVolumeSold, decimal decSharePrice,
            BrokerageReductionObject brokerageObject, string strDoc = "")
        {
            Guid = strGuid;
            OrderNumber = strOrderNumber;
            BuyCultureInfo = cultureInfo;
            Date = strDate;
            Volume = decVolume;
            VolumeSold = decVolumeSold;
            SharePrice = decSharePrice;
            if (brokerageObject != null)
            {
                BrokerageGuid = brokerageObject.Guid;
                Provision = brokerageObject.ProvisionValue;
                BrokerFee = brokerageObject.BrokerFeeValue;
                TraderPlaceFee = brokerageObject.TraderPlaceFeeValue;
                Reduction = brokerageObject.ReductionValue;
            }
            else
            {
                BrokerageGuid = @"";
            }

            Document = strDoc;

            // Calculate the market values and brokerage
            CalculateValues();

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
        private void CalculateValues()
        {
            Helper.CalcBuyValues( Volume, SharePrice,
                Provision, BrokerFee, TraderPlaceFee, Reduction,
                out var decMarketValue, out var decDeposit, out var decBrokerage, out var decBrokerageWithReduction);

            BuyValue = decMarketValue;
            BuyValueBrokerage = decDeposit;
            Brokerage = decBrokerage;
            BrokerageWithReduction = decBrokerageWithReduction;

#if DEBUG_BUY
            Console.WriteLine(@"BuyValue: {0}", BuyValue);
            Console.WriteLine(@"BuyValueBrokerage: {0}", BuyValueBrokerage);
            Console.WriteLine(@"Brokerage: {0}", Brokerage);
            Console.WriteLine(@"BrokerageWithReduction: {0}", BrokerageWithReduction);
#endif
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
