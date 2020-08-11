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
#define DEBUG_BUY_OBJECT

using SharePortfolioManager.Classes.Brokerage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Classes.Buys
{
    /// <summary>
    /// This class stores values of one buy.
    /// </summary>
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
        private decimal _price;

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
        /// Stores the brokerage - reduction value of the buy
        /// </summary>
        private decimal _brokerageReduction;

        #endregion Brokerage values

        #endregion Variables

        #region Properties

        /// <summary>
        /// Culture info of the buy
        /// </summary>
        [Browsable(false)]
        public CultureInfo BuyCultureInfo { get; internal set; }

        /// <summary>
        /// Guid of the buy which is stored in the XML
        /// </summary>
        [Browsable(false)]
        public string Guid { get; internal set; }

        /// <summary>
        /// Order number of the buy
        /// </summary>
        [Browsable(false)]
        public string OrderNumber { get; internal set; }

        /// <summary>
        /// Date of the buy
        /// </summary>
        [Browsable(false)]
        public string Date { get; internal set; }

        /// <summary>
        /// Date of the buy as string
        /// </summary>
        [Browsable(false)]
        public string DateAsStr => DateTime.Parse(Date).Date.ToShortDateString();

        /// <summary>
        /// Volume of the buy
        /// </summary>
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

        /// <summary>
        /// Volume of the buy as string
        /// </summary>
        [Browsable(false)]
        public string VolumeAsStr => Volume > 0
            ? Helper.FormatDecimal(Volume, Helper.VolumeFiveLength, false, Helper.VolumeTwoFixLength)
            : Helper.FormatDecimal(0, Helper.VolumeFiveLength, false, Helper.VolumeTwoFixLength);

        /// <summary>
        /// Volume which is already sold of this buy
        /// </summary>
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

        /// <summary>
        /// Volume which is already sold if this buy as string
        /// </summary>
        [Browsable(false)]
        public string VolumeSoldAsStr => VolumeSold > 0
            ? Helper.FormatDecimal(VolumeSold, Helper.VolumeFiveLength, false, Helper.VolumeTwoFixLength)
            : Helper.FormatDecimal(0, Helper.VolumeFiveLength, false, Helper.VolumeTwoFixLength);
        /// <summary>
        /// Price of one share of this share
        /// </summary>
        [Browsable(false)]
        public decimal Price
        {
            get => _price;
            internal set
            {
                if (_price.Equals(value))
                    return;
                _price = value;
            }
        }

        /// <summary>
        /// Price of one share of this share as string
        /// </summary>
        [Browsable(false)]
        public string PriceAsStr => Price > 0
            ? Helper.FormatDecimal(Price, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        #region Brokerage values

        /// <summary>
        /// Guid of the brokerage values of this buy
        /// </summary>
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

        /// <summary>
        /// Provision of this buy
        /// </summary>
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

        /// <summary>
        /// Provision of this buy as string
        /// </summary>
        [Browsable(false)]
        public string ProvisionAsStr => Provision > 0
            ? Helper.FormatDecimal(Provision, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Broker fee of this buy
        /// </summary>
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

        /// <summary>
        /// Broker fee of this buy as string
        /// </summary>
        [Browsable(false)]
        public string BrokerFeeAsStr => BrokerFee > 0
            ? Helper.FormatDecimal(BrokerFee, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Trader place fee of this buy
        /// </summary>
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

        /// <summary>
        /// Trader place fee of this buy as string
        /// </summary>
        [Browsable(false)]
        public string TraderPlaceFeeAsStr => TraderPlaceFee > 0
            ? Helper.FormatDecimal(TraderPlaceFee, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Reduction of this buy
        /// </summary>
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

        /// <summary>
        /// Reduction of this buy as string
        /// </summary>
        [Browsable(false)]
        public string ReductionAsStr => Reduction > 0
            ? Helper.FormatDecimal(Reduction, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Complete brokerage of this buy
        /// Brokerage = provision + broker fee + trader place fee
        /// </summary>
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

        /// <summary>
        /// Complete brokerage of this buy as string
        /// </summary>
        [Browsable(false)]
        public string BrokerageAsStr => Brokerage > 0
            ? Helper.FormatDecimal(Brokerage, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Complete brokerage - reduction of this buy
        /// Brokerage = provision + broker fee + trader place fee - reduction
        /// </summary>
        [Browsable(false)]
        public decimal BrokerageReduction
        {
            get => _brokerageReduction;
            internal set
            {
                if (_brokerageReduction.Equals(value))
                    return;
                _brokerageReduction = value;
            }
        }

        /// <summary>
        /// Complete brokerage - reduction of this buy as string
        /// </summary>
        [Browsable(false)]
        public string BrokerageReductionAsStr => BrokerageReduction > 0
            ? Helper.FormatDecimal(BrokerageReduction, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        #endregion Brokerage values

        /// <summary>
        /// Value of this buy
        /// </summary>
        [Browsable(false)]
        public decimal BuyValue { get; internal set; } = -1;

        /// <summary>
        /// Value - minus reduction of this buy
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueReduction { get; internal set; } = -1;

        /// <summary>
        /// Value - minus reduction of this buy as string
        /// </summary>
        [Browsable(false)]
        public string BuyValueReductionAsStr => BuyValueReduction > 0
            ? Helper.FormatDecimal(BuyValueReduction, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength)
            : Helper.FormatDecimal(0, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength);

        /// <summary>
        /// Value + brokerage of this buy
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueBrokerage { get; internal set; } = -1;

        /// <summary>
        /// Value + brokerage - reduction of this buy 
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueBrokerageReduction { get; internal set; } = -1;

        /// <summary>
        /// Document of this buy
        /// </summary>
        [Browsable(false)]
        public string DocumentAsStr { get; internal set; }

        [Browsable(false)]
        public Image DocumentGridImage => Helper.GetImageForFile(DocumentAsStr);
        #endregion Properties

        #region Data grid view properties

        /// <summary>
        /// Guid of the buy for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Guid")]
        // ReSharper disable once UnusedMember.Global
        public string DgvGuid => Guid;

        /// <summary>
        /// Date of the buy for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Date")]
        // ReSharper disable once UnusedMember.Global
        public string DgvDateAsStr => DateAsStr;

        /// <summary>
        /// Volume of the buy for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Volume")]
        // ReSharper disable once UnusedMember.Global
        public string DgvVolumeAsStr => Volume > 0
            ? Helper.FormatDecimal(Volume, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true,
                ShareObject.PieceUnit, BuyCultureInfo)
            : @"";

        /// <summary>
        /// Price of the buy for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Price")]
        // ReSharper disable once UnusedMember.Global
        public string DgvSharePriceAsStr => Price > 0
            ? Helper.FormatDecimal(Price, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true,
                @"", BuyCultureInfo)
            : @"";

        /// <summary>
        /// Brokerage of the buy for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"BrokerageReduction")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBrokerageReductionAsStr => BrokerageReduction > 0
            ? Helper.FormatDecimal(BrokerageReduction, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true,
                @"", BuyCultureInfo)
            : @"";

        /// <summary>
        /// Buy value - reduction of the buy for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"BuyValueBrokerageReduction")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBuyValueBrokerageReductionAsStr => BuyValueBrokerageReduction > 0
            ? Helper.FormatDecimal(BuyValueBrokerageReduction, Helper.CurrencyFiveLength, false, Helper.CurrencyTwoFixLength, true,
                @"", BuyCultureInfo)
            : @"";

        /// <summary>
        /// Document of the buy for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Document")]
        // ReSharper disable once UnusedMember.Global
        public Image DgvDocumentGridImage => DocumentGridImage;

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
            Price = decSharePrice;
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

            DocumentAsStr = strDoc;

#if DEBUG_BUY_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"New BuyObject created");
            Console.WriteLine(@"Guid: {0}", strGuid);
            Console.WriteLine(@"OrderNumber: {0}", OrderNumber);
            Console.WriteLine(@"BuyCultureInfo: {0}", BuyCultureInfo);
            Console.WriteLine(@"Date: {0}", strDate);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"VolumeSold: {0}", VolumeSold);
            Console.WriteLine(@"Price: {0}", Price);
            Console.WriteLine(@"BrokerageGuid: {0}", BrokerageGuid);
            Console.WriteLine(@"Provision: {0}", Provision);
            Console.WriteLine(@"BrokerFee: {0}", BrokerFee);
            Console.WriteLine(@"TraderPlaceFee: {0}", TraderPlaceFee);
            Console.WriteLine(@"Reduction: {0}", Reduction);
            Console.WriteLine(@"Document: {0}", strDoc);
            Console.WriteLine(@"");
#endif
            // Calculate the market values and brokerage
            CalculateValues();
        }

        /// <summary>
        /// This function calculates six values.
        /// - the buy value
        /// - the buy value with reduction
        /// - the buy value with brokerage
        /// - the buy value with brokerage and reduction
        /// - the brokerage value
        /// - the brokerage value - reduction
        /// with the given volume and share price of the buy
        /// </summary>
        private void CalculateValues()
        {
            Helper.CalcBuyValues(Volume, Price,
                Provision, BrokerFee, TraderPlaceFee, Reduction,
                out var decBuyValue, out var decBuyValueReduction, out var decBuyValueBrokerage,
                out var decBuyValueWithBrokerageWithReduction, out var decBrokerage, out var decBrokerageReduction);

            Brokerage = decBrokerage;
            BrokerageReduction = decBrokerageReduction;
            BuyValue = decBuyValue;
            BuyValueReduction = decBuyValueReduction;
            BuyValueBrokerage = decBuyValueBrokerage;
            BuyValueBrokerageReduction = decBuyValueWithBrokerageWithReduction;

#if DEBUG_BUY_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateValues()");
            Console.WriteLine(@"Brokerage: {0}", Brokerage);
            Console.WriteLine(@"BrokerageReduction: {0}", BrokerageReduction);
            Console.WriteLine(@"BuyValue: {0}", BuyValue);
            Console.WriteLine(@"BuyValueReduction: {0}", BuyValueReduction);
            Console.WriteLine(@"BuyValueBrokerage: {0}", BuyValueBrokerage);
            Console.WriteLine(@"BuyValueBrokerageReduction: {0}", BuyValueBrokerageReduction);
            Console.WriteLine(@"");
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

        /// <inheritdoc />
        /// <summary>
        /// This function compares the to given BuyObjects by the date
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns></returns>
        public int Compare(BuyObject object1, BuyObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }
}
