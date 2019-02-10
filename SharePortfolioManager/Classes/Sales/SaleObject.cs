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
using System.Linq;
using SharePortfolioManager.Classes.Costs;

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

        /// <summary>
        /// Stores the brokerage minus reduction value of the buy
        /// </summary>
        private decimal _brokerageWithReduction;

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
        public string DateAsStr => Date;

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
        public string VolumeAsStr => Helper.FormatDecimal(_volume, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public List<SaleBuyDetails> SaleBuyDetails = new List<SaleBuyDetails>();

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
        public string SalePriceAsStr => Helper.FormatDecimal(_salePrice, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal TaxAtSource
        {
            get => _taxAtSource;
            internal set
            {
                if (_taxAtSource > -1)
                    _taxSum -= _taxAtSource;

                _taxAtSource = value;

                if (_taxAtSource > -1)
                    _taxSum += _taxAtSource;
            }
        }

        [Browsable(false)]
        public string TaxAtSourceAsStr => Helper.FormatDecimal(_taxAtSource, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal CapitalGainsTax
        {
            get => _capitalGainsTax;
            internal set
            {
                if (_capitalGainsTax > -1)
                    _taxSum -= _capitalGainsTax;

                _capitalGainsTax = value;

                if (_capitalGainsTax > -1)
                    _taxSum += _capitalGainsTax;
            }
        }

        [Browsable(false)]
        public string CapitalGainsTaxAsStr => Helper.FormatDecimal(_capitalGainsTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal SolidarityTax
        {
            get => _solidarityTax;
            internal set
            {
                if (_solidarityTax > -1)
                    _taxSum -= _solidarityTax;

                _solidarityTax = value;

                if (_solidarityTax > -1)
                    _taxSum += _solidarityTax;
            }
       }

        [Browsable(false)]
        public string SolidarityTaxAsStr => Helper.FormatDecimal(_solidarityTax, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

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
        public string ProvisionAsStr => Helper.FormatDecimal(Provision, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

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
        public string BrokerFeeAsStr => Helper.FormatDecimal(BrokerFee, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

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
        public string TraderPlaceFeeAsStr => Helper.FormatDecimal(TraderPlaceFee, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

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
        public string ReductionAsStr => Helper.FormatDecimal(Reduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

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
        public string BrokerageAsStr => Helper.FormatDecimal(Brokerage, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

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
        public string BrokerageWithReductionAsStr => Helper.FormatDecimal(BrokerageWithReduction, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        #endregion Brokerage values

        [Browsable(false)]
        public decimal PurchaseValue { get; internal set; } = -1;

        [Browsable(false)]
        public string PurchaseValueAsStr => Helper.FormatDecimal(PurchaseValue, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal ProfitLoss { get; internal set; }

        [Browsable(false)]
        public string ProfitLossAsStr => Helper.FormatDecimal(ProfitLoss, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal ProfitLossWithoutBrokerage { get; internal set; }

        [Browsable(false)]
        public decimal Payout { get; internal set; }

        [Browsable(false)]
        public string PayoutAsStr => Helper.FormatDecimal(Payout, Helper.Currencytwolength, true, Helper.Currencytwofixlength, false, @"", SaleCultureInfo);

        [Browsable(false)]
        public decimal PayoutWithoutBrokerage { get; internal set; }

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
        [DisplayName(@"Purchase")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPurchaseValueAsStr => PurchaseValueAsStr;

        [Browsable(true)]
        [DisplayName(@"ProfitLoss")]
        // ReSharper disable once UnusedMember.Global
        public string DgvProfitLossAsStr => ProfitLossAsStr;

        [Browsable(true)]
        [DisplayName(@"Payout")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPayoutAsStr => PayoutAsStr;

        [Browsable(true)]
        [DisplayName(@"Document")]
        // ReSharper disable once UnusedMember.Global
        public Image DocumentGrid => Helper.GetImageForFile(Document);

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
        public SaleObject(CultureInfo cultureInfo, string strGuid, string strDate, string strOrderNumber, decimal decVolume, decimal decSalePrice, IEnumerable<SaleBuyDetails> saleBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
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
            }
            else
            {
                BrokerageGuid = @"";
            }

            Document = strDoc;

#if DEBUG_SALE
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

            PurchaseValue = SaleBuyDetails.Sum(saleDetail => saleDetail.DecVolume * saleDetail.DecBuyPrice);

            ProfitLossWithoutBrokerage = decSaleValue - PurchaseValue - TaxSum;
            ProfitLoss = ProfitLossWithoutBrokerage - Brokerage + Reduction;
            PayoutWithoutBrokerage = decSaleValue - TaxSum;
            Payout = PayoutWithoutBrokerage - Brokerage + Reduction;
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

        [Browsable(false)] public CultureInfo SaleBuyDetailsCultureInfo { get; internal set; }

        [Browsable(false)] public bool SaleBuyDetailsAdded { get; internal set; }

        [Browsable(false)] public string StrDateTime { get; internal set; }

        [Browsable(false)] public decimal DecBuyPrice { get; internal set; }

        [Browsable(false)]
        public string SaleBuyPriceAsStr => Helper.FormatDecimal(DecBuyPrice, Helper.Currencyfivelength, false,
            Helper.Currencytwofixlength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)] public decimal DecVolume { get; internal set; }

        [Browsable(false)]
        public string SaleBuyVolumeAsStr => Helper.FormatDecimal(DecVolume, Helper.Currencyfivelength, false,
            Helper.Currencytwofixlength, false, @"", SaleBuyDetailsCultureInfo);

        [Browsable(false)] public decimal SaleBuyValue { get; }

        [Browsable(true)]
        [DisplayName("Data")]
        public string SaleBuyDetailsData {

            get
            {
                const string format = "  {0,10} : {1,20:0.00000} * {2,20:0.00000} = {3,18:0.00}";
                return  string.Format(format,
                    StrDateTime,
                    DecVolume,
                    DecBuyPrice,
                    DecVolume * DecBuyPrice
                    );
            }
        }

        [Browsable(false)]
        public string BuyGuid { get; internal set; }

        #endregion Properties

        #region Methods

        public SaleBuyDetails(CultureInfo cultureInfo, string strDateTime, decimal decVolume, decimal decBuyPrice, string buyGuid)
        {
            SaleBuyDetailsCultureInfo = cultureInfo;
            StrDateTime = strDateTime;
            DecVolume = decVolume;
            DecBuyPrice = decBuyPrice;
            BuyGuid = buyGuid;

            // Calculate buy value
            SaleBuyValue = decVolume * decBuyPrice;
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
        public decimal ProfitLoss { get; internal set; }

        [Browsable(false)]
        public string Document { get; internal set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="cultureInfo">Culture info of the share</param>
        /// <param name="strSaleGuid">Guid of the share sale</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="decProfitLoss">Value of the profit or loss</param>
        /// <param name="strDoc">Document of the sale</param>
        public ProfitLossObject(CultureInfo cultureInfo, string strSaleGuid, string strDate, decimal decProfitLoss, string strDoc = "")
        {
            ProfitLossCultureInfo = cultureInfo;
            SaleGuid = strSaleGuid;
            Date = strDate;
            ProfitLoss = decProfitLoss;
            Document = strDoc;

#if DEBUG_SALE
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

