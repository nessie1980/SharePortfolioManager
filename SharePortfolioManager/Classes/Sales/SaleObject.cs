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
//#define DEBUG_SALE_OBJECT

using SharePortfolioManager.Classes.Brokerage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Classes.Sales
{
    [Serializable]
    public class SaleObject
    {
        #region Variables

        /// <summary>
        /// Stores the sale volume
        /// </summary>
        private decimal _volume = -1;

        /// <summary>
        /// Stores the sale price of the sale
        /// </summary>
        private decimal _salePrice = -1;

        /// <summary>
        /// Stores the tax at source
        /// </summary>
        private decimal _taxAtSource = -1;

        /// <summary>
        /// Stores the capital gains tax
        /// </summary>
        private decimal _capitalGainsTax = -1;

        /// <summary>
        /// Stores the solidarity tax
        /// </summary>
        private decimal _solidarityTax = -1;

        /// <summary>
        /// Stores the sum of all paid taxes
        /// </summary>
        private decimal _taxSum;

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

        #endregion Brokerage values

        #endregion Variables

        #region Properties

        [Browsable(false)]
        public CultureInfo SaleCultureInfo { get; internal set; }

        [Browsable(false)]
        public string Guid { get; internal set; }

        [Browsable(false)]
        public string Date { get; internal set; }

        [Browsable(false)]
        public string DateAsStr => DateTime.Parse(Date).Date.ToShortDateString();

        [Browsable(false)]
        public string OrderNumber { get; internal set; }

        [Browsable(false)]
        public string OrderNumberAsStr => OrderNumber;

        [Browsable(false)]
        public decimal Volume
        {
            get => _volume;
            internal set
            {
                _volume = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string VolumeAsStr => Volume > 0
            ? Helper.FormatDecimal(_volume, Helper.CurrencyFiveLength, false,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)] public List<SaleBuyDetails> SaleBuyDetails = new List<SaleBuyDetails>();

        [Browsable(false)]
        public decimal SalePrice
        {
            get => _salePrice;
            internal set
            {
                _salePrice = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string SalePriceAsStr => SalePrice > 0
            ? Helper.FormatDecimal(_salePrice, Helper.CurrencyFiveLength, false,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal TaxAtSource
        {
            get => _taxAtSource;
            internal set
            {
                if (_taxAtSource > -1)
                    TaxSum -= _taxAtSource;

                _taxAtSource = value;

                if (_taxAtSource > -1)
                    TaxSum += _taxAtSource;
            }
        }

        [Browsable(false)]
        public string TaxAtSourceAsStr => TaxAtSource > 0
            ? Helper.FormatDecimal(_taxAtSource, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal CapitalGainsTax
        {
            get => _capitalGainsTax;
            internal set
            {
                if (_capitalGainsTax > -1)
                    TaxSum -= _capitalGainsTax;

                _capitalGainsTax = value;

                if (_capitalGainsTax > -1)
                    TaxSum += _capitalGainsTax;
            }
        }

        [Browsable(false)]
        public string CapitalGainsTaxAsStr => CapitalGainsTax > 0
            ? Helper.FormatDecimal(_capitalGainsTax, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal SolidarityTax
        {
            get => _solidarityTax;
            internal set
            {
                if (_solidarityTax > -1)
                    TaxSum -= _solidarityTax;

                _solidarityTax = value;

                if (_solidarityTax > -1)
                    TaxSum += _solidarityTax;
            }
        }

        [Browsable(false)]
        public string SolidarityTaxAsStr => SolidarityTax > 0
            ? Helper.FormatDecimal(_solidarityTax, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal TaxSum
        {
            get => _taxSum;
            internal set
            {
                _taxSum = value;

                // Calculate profit / loss and payout
                CalculateProfitLossAndPayout();
            }
        }

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
        public string ProvisionAsStr => Provision > 0
            ? Helper.FormatDecimal(Provision, Helper.CurrencyFiveLength, false,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

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
        public string BrokerFeeAsStr => BrokerFee > 0
            ? Helper.FormatDecimal(BrokerFee, Helper.CurrencyFiveLength, false,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

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
        public string TraderPlaceFeeAsStr => TraderPlaceFee > 0
            ? Helper.FormatDecimal(TraderPlaceFee, Helper.CurrencyFiveLength, false,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

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
        public string ReductionAsStr => Reduction > 0
            ? Helper.FormatDecimal(Reduction, Helper.CurrencyFiveLength, false,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal Brokerage
        {
            get => _brokerage;
            internal set
            {
                if (_brokerage.Equals(value))
                    return;
                _brokerage = value;

                CalculateProfitLossAndPayout();
            }
        }

        [Browsable(false)]
        public string BrokerageAsStr => Brokerage > 0
            ? Helper.FormatDecimal(Brokerage, Helper.CurrencyFiveLength, false,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        #endregion Brokerage values

        #region Buy values

        [Browsable(false)]
        public decimal BuyValue { get; internal set; } = -1;

        [Browsable(false)]
        public string BuyValueAsStr => BuyValue > 0
            ? Helper.FormatDecimal(BuyValue, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal BuyValueReduction { get; internal set; } = -1;

        [Browsable(false)]
        public string BuyValueReductionAsStr => BuyValueReduction > 0
            ? Helper.FormatDecimal(BuyValueReduction, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal BuyValueBrokerage { get; internal set; } = -1;

        [Browsable(false)]
        public string BuyValueBrokerageAsStr => BuyValueBrokerage > 0
            ? Helper.FormatDecimal(BuyValueBrokerage, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal BuyValueBrokerageReduction { get; internal set; } = -1;

        [Browsable(false)]
        public string BuyValueBrokerageReductionAsStr => BuyValueBrokerageReduction > 0
            ? Helper.FormatDecimal(BuyValueBrokerageReduction, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        #endregion Buy values

        #region Profit / loss / payout values

        [Browsable(false)] 
        public decimal ProfitLoss { get; internal set; }

        [Browsable(false)]
        public string ProfitLossAsStr => ProfitLoss > 0
            ? Helper.FormatDecimal(ProfitLoss, Helper.CurrencyTwoLength,
                true, Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal ProfitLossBrokerage { get; internal set; }

        [Browsable(false)]
        public string ProfitLossBrokerageAsStr => ProfitLossBrokerage > 0
            ? Helper.FormatDecimal(ProfitLossBrokerage, Helper.CurrencyTwoLength,
                true, Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal ProfitLossReduction { get; internal set; }

        [Browsable(false)]
        public string ProfitLossReductionAsStr => ProfitLossReduction > 0
            ? Helper.FormatDecimal(ProfitLossReduction, Helper.CurrencyTwoLength,
                true, Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal ProfitLossBrokerageReduction { get; internal set; }

        [Browsable(false)]
        public string ProfitLossBrokerageReductionAsStr => ProfitLossBrokerageReduction > 0
            ? Helper.FormatDecimal(ProfitLossBrokerageReduction, Helper.CurrencyTwoLength,
                true, Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal Payout { get; internal set; }

        [Browsable(false)]
        public string PayoutAsStr => Payout > 0
            ? Helper.FormatDecimal(Payout, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal PayoutBrokerage { get; internal set; }

        [Browsable(false)]
        public string PayoutBrokerageAsStr => PayoutBrokerage > 0
            ? Helper.FormatDecimal(PayoutBrokerage, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal PayoutReduction { get; internal set; }

        [Browsable(false)]
        public string PayoutReductionAsStr => PayoutReduction > 0
            ? Helper.FormatDecimal(PayoutReduction, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        [Browsable(false)]
        public decimal PayoutBrokerageReduction { get; internal set; }

        [Browsable(false)]
        public string PayoutBrokerageReductionAsStr => PayoutBrokerageReduction > 0
            ? Helper.FormatDecimal(PayoutBrokerageReduction, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength)
            : @"0,0";

        #endregion Profit / loss / payout values

        [Browsable(false)]
        public string DocumentAsStr { get; internal set; }

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
        public string DgvDateAsStr => DateAsStr;

        [Browsable(true)]
        [DisplayName(@"Purchase")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPurchaseValueAsStr => BuyValue > 0
            ? Helper.FormatDecimal(BuyValue, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"ProfitLoss")]
        // ReSharper disable once UnusedMember.Global
        public string DgvProfitLossAsStr => ProfitLossBrokerageReduction > 0
            ? Helper.FormatDecimal(ProfitLossBrokerageReduction, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"Payout")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPayoutAsStr => PayoutBrokerageReduction > 0
            ? Helper.FormatDecimal(PayoutBrokerageReduction, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength, true, @"", SaleCultureInfo)
            : @"-";

        [Browsable(true)]
        [DisplayName(@"Document")]
        // ReSharper disable once UnusedMember.Global
        public Image DgvDocumentGridImage => Helper.GetImageForFile(DocumentAsStr);

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strGuid">Guid of the share sale</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="strOrderNumber">Order number of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="saleBuyDetails">Buys which are sold for this sale</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="brokerageObject">Brokerage of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        public SaleObject(CultureInfo cultureInfo, string strGuid, string strDate, string strOrderNumber,
            decimal decVolume, decimal decSalePrice, IEnumerable<SaleBuyDetails> saleBuyDetails, decimal decTaxAtSource,
            decimal decCapitalGainsTax,
            decimal decSolidarityTax, BrokerageReductionObject brokerageObject, string strDoc = "")
        {
            Guid = strGuid;
            OrderNumber = strOrderNumber;
            SaleCultureInfo = cultureInfo;
            Date = strDate;
            Volume = decVolume;

            foreach (var saleBuyDetail in saleBuyDetails)
            {
                saleBuyDetail.SaleBuyDetailsAdded = true;
                SaleBuyDetails.Add(saleBuyDetail);
            }

            SalePrice = decSalePrice;
            TaxAtSource = decTaxAtSource;
            CapitalGainsTax = decCapitalGainsTax;
            SolidarityTax = decSolidarityTax;
            if (brokerageObject != null)
            {
                BrokerageGuid = brokerageObject.Guid;
                Provision = brokerageObject.ProvisionValue;
                BrokerFee = brokerageObject.BrokerFeeValue;
                TraderPlaceFee = brokerageObject.TraderPlaceFeeValue;
                Reduction = brokerageObject.ReductionValue;
                Brokerage = brokerageObject.BrokerageValue;
            }
            else
            {
                BrokerageGuid = @"";
            }

            DocumentAsStr = strDoc;

#if DEBUG_SALE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"New sale created");
            Console.WriteLine(@"Date: {0}", Date);
            Console.WriteLine(@"OrderNumber: {0}", OrderNumber);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"BuyPrice: {0}", BuyPrice);
            Console.WriteLine(@"SalePrice: {0}", SalePrice);
            Console.WriteLine(@"TaxAtSource: {0}", TaxAtSource);
            Console.WriteLine(@"CapitalGainsTax: {0}", CapitalGainsTax);
            Console.WriteLine(@"SolidarityTax: {0}", SolidarityTax);
            Console.WriteLine(@"Reduction: {0}", Reduction);
            Console.WriteLine(@"Brokerage: {0}", Brokerage);
            Console.WriteLine(@"BrokerageReduction: {0}", Brokerage);
            Console.WriteLine(@"Document: {0}", Document);
            Console.WriteLine(@"");
#endif
        }

        /// <summary>
        /// This function calculates the profit / loss and payout of the sale
        /// </summary>
        private void CalculateProfitLossAndPayout()
        {
            if (Volume <= -1 || SalePrice <= -1) return;

            var decSaleValue = Volume * SalePrice;
            var decSaleValueBrokerage = decSaleValue - Brokerage;
            var decSaleValueReduction = decSaleValue + Reduction;
            var decSaleValueBrokerageReduction = decSaleValue - Brokerage + Reduction;

            BuyValue = SaleBuyDetails.Sum(saleDetail => saleDetail.SaleBuyValue);
            BuyValueReduction = SaleBuyDetails.Sum(saleDetail => saleDetail.SaleBuyValueReduction);
            BuyValueBrokerage = SaleBuyDetails.Sum(saleDetail => saleDetail.SaleBuyValueBrokerage);
            BuyValueBrokerageReduction = SaleBuyDetails.Sum(saleDetail => saleDetail.SaleBuyValueBrokerageReduction);

            // Calculate profit / loss values
            ProfitLoss = decSaleValue - BuyValue - TaxSum;
            ProfitLossBrokerage = decSaleValueBrokerage - BuyValueBrokerage - TaxSum;
            ProfitLossReduction = decSaleValueReduction - BuyValueReduction - TaxSum;
            ProfitLossBrokerageReduction = decSaleValueBrokerageReduction - BuyValueBrokerageReduction - TaxSum;

            // Calculate payout values
            Payout = decSaleValue - TaxSum;
            PayoutBrokerage = decSaleValueBrokerage - TaxSum;
            PayoutReduction = decSaleValueReduction - TaxSum;
            PayoutBrokerageReduction = decSaleValueBrokerageReduction - TaxSum;

#if DEBUG_SALE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateProfitLossAndPayout() / MarketValue");

            Console.WriteLine(@"TaxSum: {0}", TaxSum);
            Console.WriteLine(@"Brokerage: {0}", Brokerage);
            Console.WriteLine(@"Reduction: {0}", Reduction);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"SalePrice: {0}", SalePrice);
            Console.WriteLine(@"decSaleValue: {0}", decSaleValue);
            Console.WriteLine(@"decSaleValueBrokerage: {0}", decSaleValueBrokerage);
            Console.WriteLine(@"decSaleValueReduction: {0}", decSaleValueReduction);
            Console.WriteLine(@"decSaleValueBrokerageReduction: {0}", decSaleValueBrokerageReduction);
            Console.WriteLine(@"BuyValue: {0}", BuyValue);
            Console.WriteLine(@"BuyValueBrokerage: {0}", BuyValueBrokerage);
            Console.WriteLine(@"BuyValueReduction: {0}", BuyValueReduction);
            Console.WriteLine(@"BuyValueBrokerageReduction: {0}", BuyValueBrokerageReduction);
            Console.WriteLine(@"ProfitLoss: {0}", ProfitLoss);
            Console.WriteLine(@"ProfitLossBrokerage: {0}", ProfitLossBrokerage);
            Console.WriteLine(@"ProfitLossReduction: {0}", ProfitLossReduction);
            Console.WriteLine(@"ProfitLossBrokerageReduction: {0}", ProfitLossBrokerageReduction);
            Console.WriteLine(@"Payout: {0}", Payout);
            Console.WriteLine(@"PayoutBrokerage: {0}", PayoutBrokerage);
            Console.WriteLine(@"PayoutReduction: {0}", PayoutReduction);
            Console.WriteLine(@"PayoutBrokerageReduction: {0}", PayoutBrokerageReduction);
#endif
        }

        #endregion Methods
    }

    /// <inheritdoc />
        /// <summary>
        /// This is the comparer class for the SaleObject.
        /// It is used for the sort for the sales lists.
        /// </summary>
        public class SaleObjectComparer : IComparer<SaleObject>
    {
        #region Methods

        public int Compare(SaleObject object1, SaleObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }

    [Serializable]
    public class SaleBuyDetails
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo SaleBuyDetailsCultureInfo { get; internal set; }

        [Browsable(false)]
        public bool SaleBuyDetailsAdded { get; internal set; }

        [Browsable(false)]
        public string StrDateTime { get; internal set; }

        [Browsable(false)]
        public decimal DecBuyPrice { get; internal set; }

        [Browsable(false)]
        public string SaleBuyPriceAsStr => Helper.FormatDecimal(DecBuyPrice, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)]
        public decimal DecVolume { get; internal set; }

        [Browsable(false)]
        public string SaleBuyVolumeAsStr => Helper.FormatDecimal(DecVolume, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)]
        public decimal ReductionPart { get; }

        [Browsable(false)]
        public string ReductionPartAsStr => Helper.FormatDecimal(ReductionPart, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)]
        public decimal BrokeragePart { get; }

        [Browsable(false)]
        public string BrokeragePartAsStr => Helper.FormatDecimal(BrokeragePart, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)]
        public decimal SaleBuyValue { get; }

        [Browsable(false)]
        public string SaleBuyValueAsStrWithUnit => Helper.FormatDecimal(SaleBuyValue, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)]
        public decimal SaleBuyValueReduction { get; }

        [Browsable(false)]
        public string SaleBuyValueReductionAsStrWithUnit => Helper.FormatDecimal(SaleBuyValueReduction, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)]
        public decimal SaleBuyValueBrokerage { get; }

        [Browsable(false)]
        public string SaleBuyValueBrokerageAsStrWithUnit => Helper.FormatDecimal(SaleBuyValueBrokerage, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)]
        public decimal SaleBuyValueBrokerageReduction { get; }

        [Browsable(false)]
        public string SaleBuyValueBrokerageReductionAsStrWithUnit => Helper.FormatDecimal(SaleBuyValueBrokerageReduction, Helper.CurrencyFiveLength, false,
            Helper.CurrencyTwoFixLength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(true)]
        [DisplayName("Data")]
        public string SaleBuyDetailsData {

            get
            {
                const string format = "  {0,10} : {1,15:0.00000} * {2,15:0.00000} - {3,10:0.00000} + {4,10:0.00000} = {5,15:0.00}";
                return  string.Format(format,
                    StrDateTime,
                    DecVolume,
                    DecBuyPrice,
                    ReductionPart,
                    BrokeragePart,
                    SaleBuyValueBrokerageReduction
                    );
            }
        }

        [Browsable(false)]
        public string BuyGuid { get; internal set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="cultureInfo">Culture info for the sale object</param>
        /// <param name="strDateTime">Date and time of the sale object</param>
        /// <param name="decVolume">Volume of the sale object</param>
        /// <param name="decBuyPrice">Buy price of the sale object which will be sold</param>
        /// <param name="decReductionPart">Reduction part of the used buy object</param>
        /// <param name="decBrokeragePart">Brokerage part of the used buy object</param>
        /// <param name="buyGuid">Guid of the sold buy</param>
        public SaleBuyDetails(CultureInfo cultureInfo, string strDateTime, decimal decVolume, decimal decBuyPrice, decimal decReductionPart, decimal decBrokeragePart, string buyGuid)
        {
            SaleBuyDetailsCultureInfo = cultureInfo;
            StrDateTime = strDateTime;
            DecVolume = decVolume;
            DecBuyPrice = decBuyPrice;
            ReductionPart = decReductionPart;
            BrokeragePart = decBrokeragePart;
            BuyGuid = buyGuid;

            // Calculate sale buy values
            SaleBuyValue = decVolume * decBuyPrice;
            SaleBuyValueReduction = decVolume * decBuyPrice - decReductionPart;
            SaleBuyValueBrokerage = decVolume * decBuyPrice + decBrokeragePart;
            SaleBuyValueBrokerageReduction = decVolume * decBuyPrice + decBrokeragePart - ReductionPart;
        }

        #endregion Methods
    }

    [Serializable]
    public class ProfitLossObject
    {
        #region Properties

        [Browsable(false)]
        public CultureInfo ProfitLossCultureInfo { get; internal set; }

        [Browsable(false)]
        public string SaleGuid { get; internal set; }

        [Browsable(false)]
        public string Date { get; internal set; }

        [Browsable(false)]
        public string DateAsStr => DateTime.Parse(Date).Date.ToShortDateString();

        [Browsable(false)]
        public decimal Volume { get; internal set; }

        [Browsable(false)]
        public string VolumeAsStr => Volume > 0
            ? Helper.FormatDecimal(Volume, Helper.CurrencyTwoLength, true,
                Helper.CurrencyFiveFixLength)
            : @"";

        [Browsable(false)]
        public decimal ProfitLoss { get; internal set; }

        [Browsable(false)]
        public decimal ProfitLossBrokerage { get; internal set; }

        [Browsable(false)]
        public decimal ProfitLossReduction { get; internal set; }

        [Browsable(false)]
        public decimal ProfitLossBrokerageReduction { get; internal set; }

        [Browsable(false)]
        public string DocumentAsStr { get; internal set; }

        #endregion Properties

        #region Data grid view properties

        [Browsable(true)]
        [DisplayName(@"Date")]
        // ReSharper disable once UnusedMember.Global
        public string DgvDateAsStr => DateAsStr;

        [Browsable(true)]
        [DisplayName(@"Volume")]
        // ReSharper disable once UnusedMember.Global
        public string DgvVolumeAsStr => Volume > 0
            ? Helper.FormatDecimal(Volume, Helper.CurrencyTwoLength, true,
                Helper.CurrencyFiveFixLength, true, ShareObject.PieceUnit, ProfitLossCultureInfo)
            : @"";

        [Browsable(true)]
        [DisplayName(@"ProfitLoss")]
        // ReSharper disable once UnusedMember.Global
        public string DgvProfitLossAsStr => ProfitLossBrokerageReduction > 0
            ? Helper.FormatDecimal(ProfitLossBrokerageReduction, Helper.CurrencyTwoLength, true,
                Helper.CurrencyTwoFixLength, true, @"", ProfitLossCultureInfo)
            : @"";

        [Browsable(true)]
        [DisplayName(@"Document")]
        // ReSharper disable once UnusedMember.Global
        public Image DgvDocumentGridImage => Helper.GetImageForFile(DocumentAsStr);

        #endregion Data grid view properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strSaleGuid">Guid of the share sale</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decVolume">Volume of the share sale</param>
        /// <param name="decProfitLoss">Value of the profit or loss</param>
        /// <param name="decProfitLossBrokerage">Value of the profit or loss with brokerage</param>
        /// <param name="decProfitLossReduction">Value of the profit or loss with reduction</param>
        /// <param name="decProfitLossBrokerageReduction">Value of the profit or loss with brokerage and reduction</param>
        /// <param name="strDoc">Document of the sale</param>
        public ProfitLossObject(CultureInfo cultureInfo, string strSaleGuid, string strDate, decimal decVolume,
            decimal decProfitLoss, decimal decProfitLossBrokerage, decimal decProfitLossReduction, decimal decProfitLossBrokerageReduction,
        string strDoc = "")
        {
            ProfitLossCultureInfo = cultureInfo;
            SaleGuid = strSaleGuid;
            Date = strDate;
            Volume = decVolume;
            ProfitLoss = decProfitLoss;
            ProfitLossBrokerage = decProfitLossBrokerage;
            ProfitLossReduction = decProfitLossReduction;
            ProfitLossBrokerageReduction = decProfitLossBrokerageReduction;
            DocumentAsStr = strDoc;

#if DEBUG_SALE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"New sale created");
            Console.WriteLine(@"SaleGuid: {0}", SaleGuid);
            Console.WriteLine(@"Date: {0}", Date);
            Console.WriteLine(@"ProfitLoss: {0}", ProfitLoss);
            Console.WriteLine(@"Document: {0}", Document);
            Console.WriteLine(@"");
#endif

        }

        #endregion Methods
    }

    /// <inheritdoc />
    /// <summary>
    /// This is the comparer class for the ProfitLossObject.
    /// It is used for the sort for the profit or loss lists.
    /// </summary>
    public class ProfitLossObjectComparer : IComparer<ProfitLossObject>
    {
        #region Methods

        public int Compare(ProfitLossObject object1, ProfitLossObject object2)
        {
            if (object1 == null) return 0;
            if (object2 == null) return 0;

            return DateTime.Compare(Convert.ToDateTime(object1.Date), Convert.ToDateTime(object2.Date));
        }

        #endregion Methods
    }
}

