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
//#define DEBUG_MARKET_SHARE_OBJECT

using Parser;
using SharePortfolioManager.Classes.Brokerage;
using SharePortfolioManager.Classes.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace SharePortfolioManager.Classes.ShareObjects
{
    /// <inheritdoc />
    /// <summary>
    /// This class is for the market value management of a share.
    /// The class stores the following things:
    /// - general information of the share
    /// - all buys of a share
    /// - all sales of a share
    /// - all brokerage of the sales and buys of a share
    /// </summary>
    public class ShareObjectMarketValue : ShareObject
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Stores the current market value of the current stock volume (market value)
        /// </summary>
        private decimal _marketValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the current market value of the current stock volume with profit / loss (market value)
        /// </summary>
        private decimal _marketValueWithProfitLoss = decimal.MinValue / 2;

        /// <summary>
        /// Stores the purchase value of the current stock volume (market value)
        /// </summary>
        private decimal _purchaseValue = decimal.MinValue / 2;

        #endregion General variables

        #region Portfolio value variables

        /// <summary>
        /// Stores the current purchase value of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioPurchaseValue;

        /// <summary>
        /// Stores the complete purchase value of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioCompletePurchaseValue;

        /// <summary>
        /// Stores the current market value of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioMarketValue;

        /// <summary>
        /// Stores the current market value of the current stock volume with profit / loss of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioMarketValueWithProfitLoss;

        /// <summary>
        /// Stores the complete market value with profit / loss of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioCompleteMarketValueWithProfitLoss;

        /// <summary>
        /// Stores the sold purchase value used by the sales of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioSoldPurchaseValue;

        /// <summary>
        /// Stores the performance of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioPerformanceValue;

        /// <summary>
        /// Stores the performance value of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioCompletePerformanceValue;

        /// <summary>
        /// Stores the profit or loss of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioProfitLossValue;

        /// <summary>
        /// Stores the profit or loss of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        private static decimal _portfolioCompleteProfitLossValue;

        #endregion Portfolio value variables

        #endregion Variables

        #region Properties

        #region General properties

        /// <summary>
        /// Flag if the object is disposed
        /// </summary>
        [Browsable(false)]
        public new bool Disposed { get; internal set; }

        /// <inheritdoc />
        /// <summary>
        /// Current share volume
        /// </summary>
        [Browsable(false)]
        public override decimal Volume
        {
            get => base.Volume;
            set
            {
                // Set the new share volume
                if (value == base.Volume) return;

                base.Volume = value;

                // Recalculate the performance to the previous day
                CalculateCurPrevDayPerformance();

                // Recalculate the profit or loss to the previous day
                CalculateCurPrevDayProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the total sum of the share
                CalculateMarketValue();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Current price of the share. Will be updated via the Internet
        /// </summary>
        [Browsable(false)]
        public override decimal CurPrice
        {
            get => base.CurPrice;
            set
            {
                // Set the new share price
                if (value == base.CurPrice) return;

                base.CurPrice = value;

                // Recalculate the performance to the previous day
                CalculateCurPrevDayPerformance();

                // Recalculate the profit or loss to the previous day
                CalculateCurPrevDayProfitLoss();

                // Recalculate the total sum of the share
                CalculateMarketValue();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Previous day price of the share. Will be updated via the Internet
        /// </summary>
        [Browsable(false)]
        public override decimal PrevPrice
        {
            get => base.PrevPrice;
            set
            {
                // Set the new share price
                if (value == base.PrevPrice) return;

                base.PrevPrice = value;

                // Recalculate the performance to the previous day
                CalculateCurPrevDayPerformance();

                // Recalculate the profit or loss to the previous day
                CalculateCurPrevDayProfitLoss();

                // Recalculate the total sum of the share
                CalculateMarketValue();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();
            }
        }

        #region Purchase value properties

        /// <summary>
        /// Purchase value of the current stock volume (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PurchaseValue
        {
            get => _purchaseValue;
            set
            {
                // Set the new purchase price
                if (value == _purchaseValue) return;

                // Recalculate portfolio purchase value
                if (_purchaseValue > decimal.MinValue / 2)
                    PortfolioPurchaseValue -= _purchaseValue;
                PortfolioPurchaseValue += value;

                _purchaseValue = value;

                // Recalculate the total market value of the share
                CalculateMarketValue();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();
            }
        }

        /// <summary>
        /// Purchase value of the current stock volume as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueAsStrUnit => Helper.FormatDecimal(PurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Purchase value properties

        /// <inheritdoc />
        /// <summary>
        /// Stores the value of the sold purchase used by the sales of the share
        /// </summary>
        [Browsable(false)]
        public override decimal SoldPurchaseValue
        {
            get => base.SoldPurchaseValue;
            set
            {
                // Set the new purchase value
                if (value == base.SoldPurchaseValue) return;

                // Recalculate portfolio purchase value
                if (base.SoldPurchaseValue > decimal.MinValue / 2)
                    PortfolioSoldPurchaseValue -= base.SoldPurchaseValue;
                PortfolioSoldPurchaseValue += value;

                base.SoldPurchaseValue = value;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Stores the culture info of the share
        /// </summary>
        [Browsable(false)]
        public override CultureInfo CultureInfo
        {
            get => base.CultureInfo;
            set => base.CultureInfo = value;
        }

        #endregion General properties

        #region Market value properties

        /// <summary>
        /// Stores the current market value of the current stock volume (market value)
        /// </summary>
        [Browsable(false)]
        public decimal MarketValue
        {
            get => _marketValue;
            set
            {
                if (value.Equals(_marketValue)) return;

                // Recalculate the portfolio market value of all shares
                if (_marketValue > decimal.MinValue / 2)
                {
                    PortfolioMarketValue -= _marketValue;
                    PortfolioCompleteMarketValueWithProfitLoss -= _marketValue;
                }

                PortfolioMarketValue += value;
                PortfolioCompleteMarketValueWithProfitLoss += value;

                _marketValue = value;

#if DEBUG_MARKET_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"_marketValue: {0}", _marketValue);
                Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
                Console.WriteLine(@"PortfolioCompleteMarketValue: {0}", PortfolioCompleteMarketValue);
#endif
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume as string (market value)
        /// </summary>
        [Browsable(false)]
        public string MarketValueAsStr => Helper.FormatDecimal(MarketValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Stores the current market value of the current stock volume as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string MarketValueAsStrUnit => Helper.FormatDecimal(MarketValue, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the current market value of the current stock volume with profit / loss (market value)
        /// </summary>
        [Browsable(false)]
        public decimal MarketValueWithProfitLoss
        {
            get => _marketValueWithProfitLoss;
            set
            {
                if (value.Equals(_marketValueWithProfitLoss)) return;

                // Recalculate the portfolio market value of all shares
                if (_marketValueWithProfitLoss > decimal.MinValue / 2)
                    PortfolioMarketValueWithProfitLoss -= _marketValueWithProfitLoss;
                PortfolioMarketValueWithProfitLoss += value;

                _marketValueWithProfitLoss = value;
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume with profit / loss as string (market value)
        /// </summary>
        [Browsable(false)]
        public string MarketValueWithProfitLossAsStr => Helper.FormatDecimal(MarketValueWithProfitLoss, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Stores the current market value of the current stock volume with profit / loss as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string MarketValueWithProfitLossAsStrUnit => Helper.FormatDecimal(MarketValueWithProfitLoss, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete market value with profit / loss of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal CompleteMarketValue => SalePayoutBrokerageReduction + MarketValue;

        /// <summary>
        /// Complete market value with profit / loss of all transactions as string (market value)
        /// </summary>
        [Browsable(false)]
        public string CompleteMarketValueAsStr => Helper.FormatDecimal(CompleteMarketValue, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Complete market value with profit / loss of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string CompleteMarketValueAsStrUnit => Helper.FormatDecimal(CompleteMarketValue, Helper.CurrencyTwoLength, false, Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        #endregion Market value properties

        #region Buy value properties

        /// <summary>
        /// Complete buy volume of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyVolume => AllBuyEntries.BuyVolumeTotal;

        /// <summary>
        /// Complete buy volume of all transactions as (market value)
        /// </summary>
        [Browsable(false)]
        public string BuyVolumeAsStrUnit => Helper.FormatDecimal(BuyVolume, Helper.VolumeTwoLength, true, Helper.VolumeNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions without brokerage and reduction (market value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValue => AllBuyEntries.BuyValueTotal;

        /// <summary>
        /// Complete buy value of all transactions without brokerage and reduction as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueAsStrUnit => Helper.FormatDecimal(BuyValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions without brokerage and with reduction (market value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueReduction => AllBuyEntries.BuyValueReductionTotal;

        /// <summary>
        /// Complete buy value of all transactions without brokerage and with reduction as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueReductionAsStrUnit => Helper.FormatDecimal(BuyValueReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions with brokerage and without reduction (market value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueBrokerage => AllBuyEntries.BuyValueBrokerageTotal;

        /// <summary>
        /// Complete buy value of all transactions with brokerage and without reduction as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueBrokerageAsStrUnit => Helper.FormatDecimal(BuyValueBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions with brokerage and reduction (market value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueBrokerageReduction => AllBuyEntries.BuyValueBrokerageReductionTotal;

        /// <summary>
        /// Complete buy value of all transactions with brokerage and reduction as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueBrokerageReductionAsStrUnit => Helper.FormatDecimal(BuyValueBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Buy value properties

        #region Sale properties

        /// <summary>
        /// Complete sale volume value of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleVolume => AllSaleEntries.SaleVolumeTotal;

        /// <summary>
        /// Complete sale volume value of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SaleVolumeAsStrUnit => Helper.FormatDecimal(SaleVolume, Helper.VolumeTwoFixLength, true, Helper.VolumeNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value without brokerage and reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValue => AllSaleEntries.SalePurchaseValueTotal;

        /// <summary>
        /// Complete sale purchase value without brokerage and reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueAsStrUnit => Helper.FormatDecimal(SalePurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value with brokerage and without reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValueBrokerage => AllSaleEntries.SalePurchaseValueBrokerageTotal;

        /// <summary>
        /// Complete sale purchase value with brokerage and without reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueBrokerageAsStrUnit => Helper.FormatDecimal(SalePurchaseValueBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value without brokerage and with reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValueReduction => AllSaleEntries.SalePurchaseValueReductionTotal;

        /// <summary>
        /// Complete sale purchase value without brokerage and with reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueReductionAsStrUnit => Helper.FormatDecimal(SalePurchaseValueReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value with brokerage and reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValueBrokerageReduction => AllSaleEntries.SalePurchaseValueBrokerageReductionTotal;

        /// <summary>
        /// Complete sale purchase value with brokerage and reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueBrokerageReductionAsStrUnit => Helper.FormatDecimal(SalePurchaseValueBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value without brokerage and reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayout => AllSaleEntries.SalePayoutTotal;

        /// <summary>
        /// Complete sale payout value without brokerage and reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutAsStrUnit => Helper.FormatDecimal(SalePayout, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value with brokerage and without reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayoutBrokerage => AllSaleEntries.SalePayoutBrokerageTotal;

        /// <summary>
        /// Complete sale payout value with brokerage and without reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutBrokerageAsStrUnit => Helper.FormatDecimal(SalePayoutBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value without brokerage and with reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayoutReduction => AllSaleEntries.SalePayoutReductionTotal;

        /// <summary>
        /// Complete sale payout value without brokerage and with reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutReductionAsStrUnit => Helper.FormatDecimal(SalePayoutReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value with brokerage and reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayoutBrokerageReduction => AllSaleEntries.SalePayoutBrokerageReductionTotal;

        /// <summary>
        /// Complete sale payout value with brokerage and reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutBrokerageReductionAsStrUnit => Helper.FormatDecimal(SalePayoutBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value without brokerage and reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLoss => AllSaleEntries.SaleProfitLossTotal;

        /// <summary>
        /// Complete sale profit / loss value without brokerage and reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossAsStrUnit => Helper.FormatDecimal(SaleProfitLoss, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value with brokerage and without reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLossBrokerage => AllSaleEntries.SaleProfitLossBrokerageTotal;

        /// <summary>
        /// Complete sale profit / loss value with brokerage and without reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossBrokerageAsStrUnit => Helper.FormatDecimal(SaleProfitLossBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value without brokerage and with reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLossReduction => AllSaleEntries.SaleProfitLossReductionTotal;

        /// <summary>
        /// Complete sale profit / loss value without brokerage and with reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossReductionAsStrUnit => Helper.FormatDecimal(SaleProfitLossReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value with brokerage and reduction of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLossBrokerageReduction => AllSaleEntries.SaleProfitLossBrokerageReductionTotal;

        /// <summary>
        /// Complete sale profit / loss value with brokerage and reduction of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossBrokerageReductionAsStrUnit => Helper.FormatDecimal(SaleProfitLossBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Sale properties

        #region Performance value properties

        /// <summary>
        /// Performance value of the market value of the current stock volume without profit / loss (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PerformanceValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance value of the market value of the current stock volume without profit / loss as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string PerformanceValueAsStrUnit => Helper.FormatDecimal(PerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);

        /// <summary>
        /// Performance value of all transactions (market value)
        /// </summary>
        [Browsable(false)]
        public decimal CompletePerformanceValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance value of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string CompletePerformanceValueAsStrUnit => Helper.FormatDecimal(CompletePerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);

        #endregion Performance value properties

        #region Profit / loss value properties

        /// <summary>
        /// Profit or loss value of the market value of the current stock volume (market value)
        /// </summary>
        [Browsable(false)]
        public decimal ProfitLossValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Profit or loss value of the market value of the current stock volume as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string ProfitLossValueAsStrUnit => Helper.FormatDecimal(ProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Profit or loss value of the market value of the current stock volume with sale profit / loss (market value)
        /// </summary>
        [Browsable(false)]
        public decimal CompleteProfitLossValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Profit or loss value of the market value of the current stock volume with sale profit / loss as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string CompleteProfitLossValueAsStrUnit => Helper.FormatDecimal(CompleteProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Profit / loss value properties

        #region Portfolio value properties

        /// <summary>
        /// Stores the current purchase value of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioPurchaseValue
        {
            get => _portfolioPurchaseValue;
            internal set
            {
                if (_portfolioPurchaseValue.Equals(value)) return;

                _portfolioPurchaseValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Purchase value of the hole portfolio (all share in the portfolio) as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioPurchaseValueAsStrUnit => Helper.FormatDecimal(PortfolioPurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the complete purchase value of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompletePurchaseValue
        {
            get => _portfolioCompletePurchaseValue;
            internal set
            {
                if (_portfolioCompletePurchaseValue.Equals(value)) return;

                _portfolioCompletePurchaseValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Purchase value of the hole portfolio (all share in the portfolio) as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompletePurchaseValueAsStrUnit => Helper.FormatDecimal(PortfolioCompletePurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the sold purchase value used by the sales of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioSoldPurchaseValue
        {
            get => _portfolioSoldPurchaseValue;
            internal set
            {
                if (_portfolioSoldPurchaseValue.Equals(value)) return;

                _portfolioSoldPurchaseValue = value;

                // Recalculate sale the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioMarketValue
        {
            get => _portfolioMarketValue;
            internal set
            {
                if (_portfolioMarketValue.Equals(value)) return;

                _portfolioMarketValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume of the portfolio as string with unit (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioMarketValueAsStrUnit => Helper.FormatDecimal(PortfolioMarketValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the current market value of the current stock volume with profit / loss of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioMarketValueWithProfitLoss
        {
            get => _portfolioMarketValueWithProfitLoss;
            internal set
            {
                if (_portfolioMarketValueWithProfitLoss.Equals(value)) return;

                _portfolioMarketValueWithProfitLoss = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume with profit / loss of the portfolio as string with unit (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioMarketValueWithProfitLossAsStrUnit => Helper.FormatDecimal(PortfolioMarketValueWithProfitLoss, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the complete market value with profit / loss of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompleteMarketValueWithProfitLoss
        {
            get => _portfolioCompleteMarketValueWithProfitLoss;
            internal set
            {
                if (_portfolioCompleteMarketValueWithProfitLoss.Equals(value)) return;

                _portfolioCompleteMarketValueWithProfitLoss = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

            }
        }

        /// <summary>
        /// Complete market value with profit / loss of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompleteMarketValueWithProfitLossAsStrUnit => Helper.FormatDecimal(PortfolioCompleteMarketValueWithProfitLoss, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the performance of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioPerformanceValue
        {
            get => _portfolioPerformanceValue;
            internal set => _portfolioPerformanceValue = value;
        }

        /// <summary>
        /// Performance value of the hole portfolio as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioPerformanceValueAsStrUnit => Helper.FormatDecimal(PortfolioPerformanceValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the performance value of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompletePerformanceValue
        {
            get => _portfolioCompletePerformanceValue;
            internal set => _portfolioCompletePerformanceValue = value;
        }

        /// <summary>
        /// Stores the performance value of all transactions of the portfolio as string with unit (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompletePerformanceValueAsStrUnit => Helper.FormatDecimal(PortfolioCompletePerformanceValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the profit or loss of the current stock volume of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioProfitLossValue
        {
            get => _portfolioProfitLossValue;
            internal set => _portfolioProfitLossValue = value;
        }

        /// <summary>
        /// Profit or loss value of the hole portfolio as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioProfitLossValueAsStrUnit => Helper.FormatDecimal(PortfolioProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the profit or loss of all transactions of the portfolio (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompleteProfitLossValue
        {
            get => _portfolioCompleteProfitLossValue;
            internal set => _portfolioCompleteProfitLossValue = value;
        }

        /// <summary>
        /// Profit or loss value of all transactions as string with unit (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompleteProfitLossValueAsStrUnit => Helper.FormatDecimal(PortfolioCompleteProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Portfolio value properties

        #region Data grid view value properties

        /// <summary>
        /// Profit / loss and performance value of the market value of the current stock volume as string with unit and a line break (market value)
        /// </summary>
        [Browsable(false)]
        public string ProfitLossPerformanceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(ProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Complete profit / loss and performance value of the market value of all transactions as string with unit and a line break (market value)
        /// </summary>
        [Browsable(false)]
        public string CompleteProfitLossPerformanceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CompleteProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(CompletePerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Purchase value and market value of the current stock volume as string with unit and a line break (market value)
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueMarketValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(MarketValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Complete purchase value and complete market value with profit / loss of all transactions of the share as string with unit and a line break (market value)
        /// </summary>
        [Browsable(false)]
        public string CompletePurchaseValueMarketValueWithProfitLossAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(BuyValueReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(CompleteMarketValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Profit / loss and performance of the portfolio as string with unit and a line break (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioProfitLossPerformanceValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioPerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Complete profit / loss and performance of all transactions of the portfolio as string with unit and a line break (all shares) (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompleteProfitLossPerformanceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioCompleteProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioCompletePerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Complete purchase and market value of the current stock volume of all transactions as string with unit and a line break (all share) (market value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompletePurchaseValueMarketValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioCompletePurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioCompleteMarketValueWithProfitLoss, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Data grid view value properties

        #region Data grid view show properties

        /// <summary>
        /// WKN of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Wkn")]
        // ReSharper disable once UnusedMember.Global
        public string DgvWkn => Wkn;

        /// <summary>
        /// Name of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Name")]
        // ReSharper disable once UnusedMember.Global
        public string DgvNameAsStr => Name;

        /// <summary>
        /// Volume of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"Volume")]
        // ReSharper disable once UnusedMember.Global
        public string DgvVolumeAsStr => VolumeAsStr;

        /// <summary>
        /// Previous day price of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"CurPrevPrice")]
        // ReSharper disable once UnusedMember.Global
        public string DgvCurPrevPriceAsStrUnit => CurPrevPriceAsStrUnit;

        /// <summary>
        /// Difference between the current and the previous day of the share and the performance in percent
        /// of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"PrevDayPerformance")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPrevDayDifferencePerformanceAsStrUnit => CurPrevDayPriceDifferencePerformanceAsStrUnit;

        /// <summary>
        /// Profit or loss and performance value of the market value of the share volume as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"ProfitLossPerformanceMarketValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvProfitLossPerformanceValueAsStrUnit => ProfitLossPerformanceAsStrUnit;

        /// <summary>
        /// Purchase value and market value of the share volume as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"PurchaseValueMarketValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPurchaseValueFinalValueAsStrUnit => PurchaseValueMarketValueAsStrUnit;

        /// <summary>
        /// Profit or loss and performance value of all transactions as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"CompleteProfitLossPerformanceMarketValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvCompleteProfitLossPerformanceValueAsStrUnit => CompleteProfitLossPerformanceAsStrUnit;

        /// <summary>
        /// Purchase value and market value of all transactions as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"CompletePurchaseValueMarketValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvCompletePurchaseValueMarketValueAsStrUnit => CompletePurchaseValueMarketValueWithProfitLossAsStrUnit;

        #endregion Data grid view show properties

        #endregion Properties

        #region Methods

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Standard constructor
        /// </summary>
        public ShareObjectMarketValue(List<Image> imageList, string percentageUnit, string pieceUnit) : base(imageList, percentageUnit, pieceUnit)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="guid">Guid of the buy</param>
        /// <param name="wkn">WKN number of the share</param>
        /// <param name="depotNumber">Depot number where the buy has been done</param>
        /// <param name="orderNumber">order number of the first buy</param>
        /// <param name="addDateTime">Date and time of the add</param>
        /// <param name="stockMarketLaunchDate">Date of the stock market launch</param>
        /// <param name="name">Name of the share</param>
        /// <param name="lastUpdateInternet">Date and time of the last update from the Internet</param>
        /// <param name="lastUpdateShare">Date and time of the last update on the Internet site of the share</param>
        /// <param name="price">Current price of the share</param>
        /// <param name="volume">Volume of the share</param>
        /// <param name="volumeSold">Volume of the share which is already sold</param>
        /// <param name="provision">Provision of the buy</param>
        /// <param name="brokerFee">Broker fee of the buy</param>
        /// <param name="traderPlaceFee">Trader place fee of the buy</param>
        /// <param name="reduction">Reduction of the share</param>
        /// <param name="webSite">Website address of the share</param>
        /// <param name="dailyValuesWebSite">>Website address for the daily values of the share</param>
        /// <param name="imageListForDayBeforePerformance">Images for the performance indication</param>
        /// <param name="regexList">RegEx list for the share</param>
        /// <param name="cultureInfo">Culture of the share</param>
        /// <param name="shareType">Type of the share</param>
        /// <param name="document">Document of the first buy</param>
        public ShareObjectMarketValue(
            string guid, string wkn, string depotNumber, string orderNumber, string addDateTime, string stockMarketLaunchDate, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShare,
            decimal price, decimal volume, decimal volumeSold, decimal provision, decimal brokerFee, decimal traderPlaceFee, decimal reduction,
            string webSite, string dailyValuesWebSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            int shareType, string document) 
            : base(wkn, addDateTime, stockMarketLaunchDate, name, lastUpdateInternet, lastUpdateShare,
                    price, webSite, dailyValuesWebSite, imageListForDayBeforePerformance, regexList,
                    cultureInfo, shareType)
        {
            BrokerageReductionObject tempBrokerageObject = null;
            // Check if a brokerage must be added
            if (provision > 0 || brokerFee > 0 || traderPlaceFee > 0 || reduction > 0)
            {
                // Generate Guid
                var strGuidBrokerage = Guid.NewGuid().ToString();

                tempBrokerageObject = new BrokerageReductionObject(strGuidBrokerage, true, false, cultureInfo, guid,
                    addDateTime,
                    provision, brokerFee, traderPlaceFee, reduction, document);
            }

            AddBuy(guid, depotNumber, orderNumber, AddDateTime, volume, volumeSold, price,
                tempBrokerageObject, document);
        }

        #endregion Constructors

        #region Destructor

        /// <inheritdoc />
        /// <summary>
        /// Destructor
        /// </summary>
        ~ShareObjectMarketValue()
        {
            Dispose(false);
        }

        #endregion Destructor

        #region Buy methods

        /// <summary>
        /// This function adds new buy to the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDepotNumber">Depot number where the buy has been done</param>
        /// <param name="strOrderNumber">Order number of the buy</param>
        /// <param name="strDateTime">Date and time of the buy</param>
        /// <param name="decVolume">Buy volume</param>
        /// <param name="decVolumeSold">Buy volume which is already sold</param>
        /// <param name="decPrice">Price for one share</param>
        /// <param name="brokerageObject">Brokerage object of the buy</param>
        /// <param name="strDoc">Document of the buy</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBuy(string strGuid, string strDepotNumber, string strOrderNumber, string strDateTime, decimal decVolume, decimal decVolumeSold, decimal decPrice,
            BrokerageReductionObject brokerageObject, string strDoc = "")
        {
            try
            {
#if DEBUG_MARKET_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"AddBuy());
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDepotNumber: {0}", strDepotNumber);
                Console.WriteLine(@"strOrderNumber: {0}", strOrderNumber);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"decVolume: {0}", decVolume);
                Console.WriteLine(@"decVolumeSold: {0}", decVolumeSold);
                Console.WriteLine(@"decPrice: {0}", decPrice);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                // Add buy without reductions and brokerage
                if (!AllBuyEntries.AddBuy(strGuid, strDepotNumber, strOrderNumber, strDateTime, decVolume, decVolumeSold, decPrice,
                    brokerageObject, strDoc))
                    return false;

                // Set current stock volume
                Volume = BuyVolume - SaleVolume;

                // Recalculate current purchase value
                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                PurchaseValue += AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValueReduction;
                PortfolioCompletePurchaseValue +=
                    AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValueReduction;

#if DEBUG_MARKET_SHARE_OBJECT
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                Console.WriteLine(@"BuyValueReduction: {0}", AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValueReduction);
                Console.WriteLine(@"");
#endif
                return true;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }
        }

        /// <summary>
        /// This function removes a buy from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the buy remove</param>
        /// <param name="strDateTime">Date and time of the buy remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveBuy(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_MARKET_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveBuy() / MarketValue");
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get BuyObject by Guid and date and time and add the sale PurchaseValue and value to the share
                var buyObject = AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime);
                if (buyObject != null)
                {
                    // Remove buy by date and time
                    if (!AllBuyEntries.RemoveBuy(strGuid, strDateTime))
                        return false;

                    // Set current stock volume
                    Volume = BuyVolume - SaleVolume;

                    // Recalculate current purchase value
                    PurchaseValue -= buyObject.BuyValueReduction;
                    PortfolioCompletePurchaseValue -= buyObject.BuyValueReduction;

#if DEBUG_MARKET_SHARE_OBJECT
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                    Console.WriteLine(@"BuyValueReduction: {0}", buyObject.BuyValueReduction);
                    Console.WriteLine(@"");
#endif
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }
        }

        /// <summary>
        /// This function sets the document of the buy of the given Guid and datetime
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be modified</param>
        /// <param name="strDateTime">Date time of the buy which should be modified</param>
        /// <param name="strDocument">Document which should be set</param>
        /// <returns></returns>
        public bool SetBuyDocument(string strGuid, string strDateTime, string strDocument)
        {
            try
            {
                // Get BuyObject by Guid and date and time and set the new document
                var buyObject = AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime);
                if (buyObject != null)
                {
                    // Set document of the buy by Guid and date and time
                    if (!AllBuyEntries.SetDocumentBuy(strGuid, strDateTime, strDocument))
                        return false;
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }
        }

        #endregion Buy methods

        #region Sale methods

        /// <summary>
        /// This function adds a new sale to the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the share sale</param>
        /// <param name="strDate">Date of the share sale</param>
        /// <param name="strDepotNumber">Depot number where the sale has been done</param>
        /// <param name="strOrderNumber">Order number of the share sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decSalePrice">Sale price of the share</param>
        /// <param name="usedBuyDetails">Details of the used buys for the sale</param>
        /// <param name="decTaxAtSource">Tax at source of the sale</param>
        /// <param name="decCapitalGainsTax">Capital gains tax of the sale</param>
        /// <param name="decSolidarityTax">Solidarity tax of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strGuid, string strDate, string strDepotNumber, string strOrderNumber, decimal decVolume, decimal decSalePrice, List<SaleBuyDetails> usedBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, string strDoc = "")
        {
#if DEBUG_MARKET_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"AddSale()");
#endif
            try
            {
                if (!AllSaleEntries.AddSale(strGuid, strDate, strDepotNumber, strOrderNumber, decVolume, decSalePrice, usedBuyDetails, decTaxAtSource, decCapitalGainsTax,
                                            decSolidarityTax, null, strDoc))
                    return false;

                if (PortfolioCompleteMarketValueWithProfitLoss == decimal.MinValue / 2)
                    PortfolioCompleteMarketValueWithProfitLoss = 0;

                // Set current stock volume
                Volume = BuyVolume - SaleVolume;

                // Recalculate purchase and sold purchase value
                if (SoldPurchaseValue == decimal.MinValue / 2)
                    SoldPurchaseValue = 0;

                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                else
                {
                    if (Volume > 0 && PurchaseValue > 0)
                    {
                        SoldPurchaseValue += usedBuyDetails.Sum(x => x.SaleBuyValueReduction);
                        PurchaseValue -= usedBuyDetails.Sum(x => x.SaleBuyValueReduction);
                    }
                    else
                    {
                        SoldPurchaseValue += PurchaseValue;
                        PurchaseValue = 0;
                    }

                    // Calculate portfolio value
                    if (AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDate).PayoutBrokerageReduction > decimal.MinValue / 2 && MarketValue > decimal.MinValue / 2)
                        PortfolioCompleteMarketValueWithProfitLoss += (AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDate).PayoutBrokerageReduction);
                }

#if DEBUG_MARKET_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"SoldPurchaseValue: {0}", SoldPurchaseValue);
                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                Console.WriteLine(@"SalePayoutBrokerageReduction: {0}", SalePayoutBrokerageReduction);
                Console.WriteLine(@"MarketValue: {0}", MarketValue);
                Console.WriteLine(@"PortfolioCompleteMarketValue: {0}", PortfolioCompleteMarketValueWithProfitLoss);
#endif
                return true;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }
        }

        /// <summary>
        /// This function removes a sale from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the sale</param>
        /// <param name="strDateTime">Date and time of the sale remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveSale(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_MARKET_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveSale() / MarketValue");
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get SaleObject by date and time and add the sale deposit and value to the share
                var saleObject = AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDateTime);
                if (saleObject != null)
                {
                    // Remove sale by date and time
                    if (!AllSaleEntries.RemoveSale(strGuid, strDateTime))
                        return false;

                    // Set current stock volume
                    Volume = BuyVolume;

                    // Recalculate current purchase and sold purchase value
                    PurchaseValue += saleObject.BuyValue;
                    SoldPurchaseValue -= saleObject.BuyValue;

                    // Calculate portfolio value
                    if (saleObject.PayoutBrokerageReduction > decimal.MinValue / 2 && MarketValue > decimal.MinValue / 2)
                        PortfolioCompleteMarketValueWithProfitLoss -= saleObject.PayoutBrokerageReduction;

#if DEBUG_MARKET_SHARE_OBJECT
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"BuyValue: {0}", saleObject.BuyValue);
                    Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                    Console.WriteLine(@"SoldPurchaseValue: {0}", SoldPurchaseValue);
                    Console.WriteLine(@"PayoutBrokerageReduction: {0}", AllSaleEntries.GetAllSalesOfTheShare().Last().PayoutBrokerageReduction);
                    Console.WriteLine(@"PortfolioCompleteMarketValue: {0}", PortfolioCompleteMarketValueWithProfitLoss);
#endif
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }
        }

        /// <summary>
        /// This function sets the document of the sale of the given Guid and datetime
        /// </summary>
        /// <param name="strGuid">Guid of the buy which should be modified</param>
        /// <param name="strDateTime">Date time of the buy which should be modified</param>
        /// <param name="strDocument">Document which should be set</param>
        /// <returns></returns>
        public bool SetSaleDocument(string strGuid, string strDateTime, string strDocument)
        {
            try
            {
#if DEBUG_MARKET_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"SetSaleDocument() / MarketValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"strDocument: {0}", strDocument);
#endif
                // Get SaleObject by Guid and date and time and set the new document
                var saleObject = AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDateTime);
                if (saleObject != null)
                {
                    // Set document of the sale by Guid and date and time
                    if (!AllSaleEntries.SetDocumentSale(strGuid, strDateTime, strDocument))
                        return false;
                }
                else
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                return false;
            }
        }

        #endregion Sale methods

        #region Performance methods

        /// <summary>
        /// This function calculates the new current market value with the profit / loss
        /// of all sales.
        /// </summary>
        private void CalculateMarketValue()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && SaleProfitLoss > decimal.MinValue / 2
                )
            {
                if (Volume > 0)
                {
                    MarketValue = CurPrice * Volume;
                    MarketValueWithProfitLoss = CurPrice * Volume + SaleProfitLossReduction;
                }
                else
                {
                    MarketValue = 0;
                    MarketValueWithProfitLoss = SaleProfitLossReduction;
                }
            }

#if DEBUG_MARKET_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateFinalValue() / MarketValue");
            Console.WriteLine(@"CurrentPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"MarketValue: {0}", MarketValue);
#endif
        }

        /// <summary>
        /// This function calculates the new profit / loss value of the current market value.
        /// Also the new profit / loss value of all transactions.
        /// </summary>
        private void CalculateProfitLoss()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                )
            {
                if (Volume > 0)
                {
                    ProfitLossValue = CurPrice * Volume
                                      - PurchaseValue;
                    CompleteProfitLossValue = CurPrice * Volume
                                      - PurchaseValue
                                      + SaleProfitLossReduction;
                }
                else
                {
                    ProfitLossValue = 0;
                    CompleteProfitLossValue = SaleProfitLossReduction;
                }
            }

#if DEBUG_MARKET_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateProfitLoss() / MarketValue");
            Console.WriteLine(@"CurPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine(@"SaleProfitLossReduction: {0}", SaleProfitLossReduction);
            Console.WriteLine(@"ProfitLossValue: {0}", ProfitLossValue);
#endif
        }

        /// <summary>
        /// This function calculates the new performance.
        /// It calculates two values:
        /// - PerformanceValue: performance value without profit / loss
        /// - CompletePerformanceValue: performance value with profit / loss
        /// </summary>
        private void CalculatePerformance()
        {
            if (MarketValue <= decimal.MinValue / 2 || PurchaseValue <= decimal.MinValue / 2
            || CompleteMarketValue <= decimal.MinValue / 2 || BuyValueReduction <= decimal.MinValue / 2) return;

            if (PurchaseValue != 0 && BuyValueReduction != 0)
            {
                PerformanceValue = (MarketValue/ PurchaseValue * 100) - 100;
                CompletePerformanceValue = (CompleteMarketValue / BuyValueReduction * 100) - 100;
            }
            else
            {
                PerformanceValue = 0;
                if (BuyValueReduction != 0)
                    CompletePerformanceValue = (CompleteMarketValue / BuyValueReduction * 100) - 100;
            }

#if DEBUG_MARKET_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformance() / MarketValue");
            Console.WriteLine(@"MarketValue: {0}", MarketValue);
            Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine(@"PerformanceValue: {0}", PerformanceValue);
            Console.WriteLine(@"BuyValueReduction: {0}", BuyValueReduction);
            Console.WriteLine(@"CompleteMarketValue: {0}", CompleteMarketValue);
            Console.WriteLine(@"CompletePerformanceValue: {0}", CompletePerformanceValue);
#endif
        }

        #endregion Performance methods

        #region Save share object

        /// <summary>
        /// This function saves a share object to the XML document
        /// </summary>
        /// <param name="shareObject">ShareObject</param>
        /// <param name="xmlPortfolio">XML with the portfolio</param>
        /// <param name="xmlReaderPortfolio">XML reader for the portfolio</param>
        /// <param name="xmlReaderSettingsPortfolio">XML reader settings for the portfolio</param>
        /// <param name="strPortfolioFileName">Name of the portfolio XML</param>
        /// <param name="exception">Exception which may occur. If no exception occurs the value is null</param>
        /// <returns>Flag if the save was successful</returns>
        // ReSharper disable once UnusedMember.Global
        public static bool SaveShareObject(ShareObjectFinalValue shareObject, ref XmlDocument xmlPortfolio, ref XmlReader xmlReaderPortfolio, ref XmlReaderSettings xmlReaderSettingsPortfolio, string strPortfolioFileName, out Exception exception)
        {
            try
            {
                // Update existing share
                var nodeListShares = xmlPortfolio.SelectNodes($"/Portfolio/Share [@WKN = \"{shareObject.Wkn}\"]");
                if (nodeListShares != null)
                {
                    foreach (XmlNode nodeElement in nodeListShares)
                    {
                        if (!nodeElement.HasChildNodes || nodeElement.ChildNodes.Count != ShareObjectTagCount)
                        {
                            exception = null;
                            return false;
                        }

                        nodeElement.Attributes[GeneralWknAttrName].InnerText = shareObject.Wkn;
                        nodeElement.Attributes[GeneralNameAttrName].InnerText = shareObject.Name;
                        nodeElement.Attributes[GeneralUpdateAttrName].InnerText = shareObject.DoInternetUpdateAsStr;

                        for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                        {
                            switch (i)
                            {
                                #region General

                                case (int)FrmMain.PortfolioParts.StockMarketLaunchDate:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.StockMarketLaunchDate}";
                                    break;
                                case (int)FrmMain.PortfolioParts.LastUpdateInternet:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateViaInternet.ToShortDateString()} {
                                                shareObject.LastUpdateViaInternet.ToShortTimeString()
                                            }";
                                    break;
                                case (int)FrmMain.PortfolioParts.LastUpdateShare:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateShare.ToShortDateString()} {
                                                shareObject.LastUpdateShare.ToShortTimeString()
                                            }";
                                    break;
                                case (int)FrmMain.PortfolioParts.SharePrice:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.CurPriceAsStr;
                                    break;
                                case (int)FrmMain.PortfolioParts.SharePriceBefore:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.PrevPriceAsStr;
                                    break;
                                case (int)FrmMain.PortfolioParts.WebSite:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.UpdateWebSiteUrl;
                                    break;
                                case (int)FrmMain.PortfolioParts.Culture:
                                    nodeElement.ChildNodes[i].InnerXml = shareObject.CultureInfoAsStr;
                                    break;
                                case (int)FrmMain.PortfolioParts.ShareType:
                                    nodeElement.ChildNodes[i].InnerXml = shareObject.ShareType.ToString();
                                    break;

                                #endregion General

                                #region Daily values

                                case (int)FrmMain.PortfolioParts.DailyValues:
                                    // Remove old daily values
                                    while (nodeElement.ChildNodes[i].FirstChild != null)
                                        nodeElement.ChildNodes[i].RemoveChild(nodeElement.ChildNodes[i].FirstChild);
                                    nodeElement.ChildNodes[i].Attributes[DailyValuesWebSiteAttrName].InnerText = shareObject.DailyValuesUpdateWebSiteUrl;

                                    foreach (var dailyValue in shareObject.DailyValues)
                                    {
                                        var newDailyValuesElement =
                                            xmlPortfolio.CreateElement(DailyValuesTagNamePre);
                                        newDailyValuesElement.SetAttribute(DailyValuesDateTagName,
                                            dailyValue.Date.ToShortDateString());
                                        newDailyValuesElement.SetAttribute(DailyValuesClosingPriceTagName,
                                            dailyValue.ClosingPrice.ToString(CultureInfo.CurrentCulture));
                                        newDailyValuesElement.SetAttribute(DailyValuesOpeningPriceTagName,
                                            dailyValue.OpeningPrice.ToString(CultureInfo.CurrentCulture));
                                        newDailyValuesElement.SetAttribute(DailyValuesTopTagName,
                                            dailyValue.Top.ToString(CultureInfo.CurrentCulture));
                                        newDailyValuesElement.SetAttribute(DailyValuesBottomTagName,
                                            dailyValue.Bottom.ToString(CultureInfo.CurrentCulture));
                                        newDailyValuesElement.SetAttribute(DailyValuesVolumeTagName,
                                           dailyValue.Volume.ToString(CultureInfo.CurrentCulture));
                                        nodeElement.ChildNodes[i].AppendChild(newDailyValuesElement);
                                    }
                                    break;

                                #endregion Daily values

                                #region Brokerage

                                case (int)FrmMain.PortfolioParts.Brokerages:
                                    // Remove old brokerage
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var brokerageElementYear in shareObject.AllBrokerageEntries
                                        .GetAllBrokerageOfTheShare())
                                    {
                                        var newBrokerageElement = xmlPortfolio.CreateElement(BrokerageTagNamePre);
                                        newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName, brokerageElementYear.PartOfABuyAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageDateAttrName, brokerageElementYear.DateAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageProvisionAttrName, brokerageElementYear.ProvisionValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageBrokerFeeAttrName, brokerageElementYear.BrokerFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageTraderPlaceFeeAttrName, brokerageElementYear.TraderPlaceFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageReductionAttrName, brokerageElementYear.ReductionValueAsStr);
                                        newBrokerageElement.SetAttribute(BuyDocumentAttrName, brokerageElementYear.DocumentAsStr);
                                        nodeElement.ChildNodes[i].AppendChild(newBrokerageElement);
                                    }
                                    break;

                                #endregion Brokerage

                                #region Buys

                                case (int)FrmMain.PortfolioParts.Buys:
                                    // Remove old buys
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var buyElementYear in shareObject.AllBuyEntries
                                        .GetAllBuysOfTheShare())
                                    {
                                        var newBuyElement = xmlPortfolio.CreateElement(BuyTagNamePre);
                                        newBuyElement.SetAttribute(BuyGuidAttrName,
                                            buyElementYear.Guid);
                                        newBuyElement.SetAttribute(BuyDepotNumberAttrName,
                                            buyElementYear.DepotNumberAsStr);
                                        newBuyElement.SetAttribute(BuyDateAttrName,
                                            buyElementYear.DateAsStr);
                                        newBuyElement.SetAttribute(BuyOrderNumberAttrName,
                                            buyElementYear.OrderNumberAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeAttrName,
                                            buyElementYear.VolumeAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeSoldAttrName,
                                            buyElementYear.VolumeSoldAsStr);
                                        newBuyElement.SetAttribute(BuyPriceAttrName,
                                            buyElementYear.PriceAsStr);
                                        newBuyElement.SetAttribute(BuyBrokerageGuidAttrName,
                                            buyElementYear.ReductionAsStr);
                                        newBuyElement.SetAttribute(BuyDocumentAttrName,
                                            buyElementYear.DocumentAsStr);
                                        nodeElement.ChildNodes[i].AppendChild(newBuyElement);
                                    }
                                    break;

                                #endregion Buys

                                #region Sales

                                case (int)FrmMain.PortfolioParts.Sales:
                                    // Remove old sales
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var saleElementYear in shareObject.AllSaleEntries
                                        .GetAllSalesOfTheShare())
                                    {
                                        var newSaleElement =
                                            xmlPortfolio.CreateElement(SaleTagNamePre);
                                        newSaleElement.SetAttribute(SaleDateAttrName,
                                            saleElementYear.DateAsStr);
                                        newSaleElement.SetAttribute(SaleDepotNumberAttrName,
                                            saleElementYear.DepotNumberAsStr);
                                        newSaleElement.SetAttribute(SaleOrderNumberAttrName,
                                            saleElementYear.OrderNumberAsStr);
                                        newSaleElement.SetAttribute(SaleSalePriceAttrName,
                                            saleElementYear.SalePriceAsStr);
                                        newSaleElement.SetAttribute(SaleTaxAtSourceAttrName,
                                            saleElementYear.TaxAtSourceAsStr);
                                        newSaleElement.SetAttribute(SaleCapitalGainsTaxAttrName,
                                            saleElementYear.CapitalGainsTaxAsStr);
                                        newSaleElement.SetAttribute(SaleSolidarityTaxAttrName,
                                            saleElementYear.SolidarityTaxAsStr);
                                        newSaleElement.SetAttribute(SaleDocumentAttrName,
                                            saleElementYear.DocumentAsStr);

                                        // Used buy details
                                        var newUsedBuysElement =
                                            xmlPortfolio.CreateElement(SaleUsedBuysAttrName);

                                        if (saleElementYear.SaleBuyDetails.Count > 0)
                                        {
                                            foreach (var usedBuys in saleElementYear.SaleBuyDetails)
                                            {
                                                var newUsedBuyElement =
                                                    xmlPortfolio.CreateElement(SaleUsedBuyAttrName);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyDateAttrName,
                                                    usedBuys.StrDateTime);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyVolumeAttrName,
                                                    usedBuys.SaleBuyVolumeAsStr);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyPriceAttrName,
                                                    usedBuys.SaleBuyPriceAsStr);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleUsedBuyReductionAttrName,
                                                    usedBuys.ReductionPartAsStr);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleUsedBuyBrokerageAttrName,
                                                    usedBuys.BrokeragePartAsStr);
                                                newUsedBuyElement.SetAttribute(
                                                    SaleBuyGuidAttrName,
                                                    usedBuys.BuyGuid);

                                                // Add buy of the sale to the used buys
                                                newUsedBuysElement.AppendChild(newUsedBuyElement);
                                            }
                                        }

                                        // Add used buys to the sale
                                        newSaleElement.AppendChild(newUsedBuysElement);

                                        // Add new sale to the share
                                        nodeElement.ChildNodes[i].AppendChild(newSaleElement);
                                    }
                                    break;

                                #endregion Sales

                                default:
                                    break;
                            }
                        }
                    }

                    // Add a new share
                    if (nodeListShares.Count == 0)
                    {
                        #region General

                        // Get root element
                        var rootPortfolio = xmlPortfolio.SelectSingleNode(GeneralPortfolioAttrName);

                        // Add new share
                        var newShareNode = xmlPortfolio.CreateNode(XmlNodeType.Element, GeneralShareAttrName, null);

                        // Add attributes (WKN)
                        var xmlAttributeWkn = xmlPortfolio.CreateAttribute(GeneralWknAttrName);
                        xmlAttributeWkn.Value = shareObject.Wkn;
                        newShareNode.Attributes.Append(xmlAttributeWkn);

                        // Add attributes ShareName)
                        var xmlAttributeShareName = xmlPortfolio.CreateAttribute(GeneralNameAttrName);
                        xmlAttributeShareName.Value = shareObject.Name;
                        newShareNode.Attributes.Append(xmlAttributeShareName);

                        // Add attributes (Update)
                        var xmlAttributeUpdateFlag = xmlPortfolio.CreateAttribute(GeneralUpdateAttrName);
                        xmlAttributeUpdateFlag.Value = shareObject.DoInternetUpdateAsStr;
                        newShareNode.Attributes.Append(xmlAttributeUpdateFlag);

                        // Add child nodes (last update Internet)
                        var newStockMarketLaunchDate = xmlPortfolio.CreateElement(GeneralStockMarketLaunchDateAttrName);
                        // Add child inner text
                        var stockMarketLaunchDateValue = xmlPortfolio.CreateTextNode(
                            shareObject.StockMarketLaunchDate);
                        newShareNode.AppendChild(newStockMarketLaunchDate);
                        newShareNode.LastChild.AppendChild(stockMarketLaunchDateValue);

                        // Add child nodes (last update Internet)
                        var newLastUpdateInternet = xmlPortfolio.CreateElement(GeneralLastUpdateInternetAttrName);
                        // Add child inner text
                        var lastUpdateInternetValue = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateViaInternet.ToShortDateString() + " " +
                            shareObject.LastUpdateViaInternet.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateInternet);
                        newShareNode.LastChild.AppendChild(lastUpdateInternetValue);

                        // Add child nodes (last update share)
                        var newLastUpdateDate = xmlPortfolio.CreateElement(GeneralLastUpdateShareDateAttrName);
                        // Add child inner text
                        var lastUpdateValueDate = xmlPortfolio.CreateTextNode(
                            shareObject.LastUpdateShare.ToShortDateString() + " " +
                            shareObject.LastUpdateShare.ToShortTimeString());
                        newShareNode.AppendChild(newLastUpdateDate);
                        newShareNode.LastChild.AppendChild(lastUpdateValueDate);

                        // Add child nodes (share price)
                        var newSharePrice = xmlPortfolio.CreateElement(GeneralSharePriceAttrName);
                        // Add child inner text
                        var sharePrice = xmlPortfolio.CreateTextNode(shareObject.CurPriceAsStr);
                        newShareNode.AppendChild(newSharePrice);
                        newShareNode.LastChild.AppendChild(sharePrice);

                        // Add child nodes (share price before)
                        var newSharePriceBefore = xmlPortfolio.CreateElement(GeneralSharePriceBeforeAttrName);
                        // Add child inner text
                        var sharePriceBefore = xmlPortfolio.CreateTextNode(shareObject.PrevPriceAsStr);
                        newShareNode.AppendChild(newSharePriceBefore);
                        newShareNode.LastChild.AppendChild(sharePriceBefore);

                        // Add child nodes (website)
                        var newWebsite = xmlPortfolio.CreateElement(GeneralWebSiteAttrName);
                        // Add child inner text
                        var webSite = xmlPortfolio.CreateTextNode(shareObject.UpdateWebSiteUrl);
                        newShareNode.AppendChild(newWebsite);
                        newShareNode.LastChild.AppendChild(webSite);

                        // Add child nodes (culture)
                        var newCulture = xmlPortfolio.CreateElement(GeneralCultureAttrName);
                        // Add child inner text
                        var culture = xmlPortfolio.CreateTextNode(shareObject.CultureInfo.Name);
                        newShareNode.AppendChild(newCulture);
                        newShareNode.LastChild.AppendChild(culture);

                        // Add child nodes (share type)
                        var newShareType = xmlPortfolio.CreateElement(GeneralShareTypeAttrName);
                        // Add child inner text
                        var shareType = xmlPortfolio.CreateTextNode(shareObject.ShareType.ToString());
                        newShareNode.AppendChild(newShareType);
                        newShareNode.LastChild.AppendChild(shareType);

                        #endregion General

                        #region DailyValues / Brokerage / Buys / Sales / Dividends

                        // Add child nodes (daily values)
                        var newDailyValues = xmlPortfolio.CreateElement(GeneralDailyValuesAttrName);
                        newDailyValues.SetAttribute(DailyValuesWebSiteAttrName,shareObject.DailyValuesUpdateWebSiteUrl);
                        newShareNode.AppendChild(newDailyValues);

                        // Add child nodes (brokerage)
                        var newBrokerage = xmlPortfolio.CreateElement(GeneralBrokeragesAttrName);
                        newShareNode.AppendChild(newBrokerage);
                        foreach (var brokerageElementYear in shareObject.AllBrokerageEntries.GetAllBrokerageOfTheShare())
                        {
                            var newBrokerageElement = xmlPortfolio.CreateElement(BrokerageTagNamePre);
                            newBrokerageElement.SetAttribute(BrokerageGuidAttrName, brokerageElementYear.Guid);
                            newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName, brokerageElementYear.PartOfABuyAsStr);
                            newBrokerageElement.SetAttribute(BrokerageDateAttrName, brokerageElementYear.DateAsStr);
                            newBrokerageElement.SetAttribute(BrokerageProvisionAttrName, brokerageElementYear.ProvisionValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageBrokerFeeAttrName, brokerageElementYear.BrokerFeeValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageTraderPlaceFeeAttrName, brokerageElementYear.TraderPlaceFeeValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageReductionAttrName, brokerageElementYear.ReductionValueAsStr);
                            newBrokerageElement.SetAttribute(BuyDocumentAttrName, brokerageElementYear.DocumentAsStr);
                            newBrokerage.AppendChild(newBrokerageElement);
                        }

                        // Add child nodes (buys)
                        var newBuys = xmlPortfolio.CreateElement(GeneralBuysAttrName);
                        newShareNode.AppendChild(newBuys);
                        foreach (var buyElementYear in shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                        {
                            var newBuyElement = xmlPortfolio.CreateElement(BuyTagNamePre);
                            newBuyElement.SetAttribute(BuyGuidAttrName, buyElementYear.Guid);
                            newBuyElement.SetAttribute(BuyOrderNumberAttrName, buyElementYear.OrderNumber);
                            newBuyElement.SetAttribute(BuyDateAttrName, buyElementYear.DateAsStr);
                            newBuyElement.SetAttribute(BuyVolumeAttrName, buyElementYear.VolumeAsStr);
                            newBuyElement.SetAttribute(BuyVolumeSoldAttrName, @"0");
                            newBuyElement.SetAttribute(BuyPriceAttrName, buyElementYear.PriceAsStr);
                            newBuyElement.SetAttribute(BuyBrokerageGuidAttrName, buyElementYear.BrokerageGuid);
                            newBuyElement.SetAttribute(BuyDocumentAttrName, buyElementYear.DocumentAsStr);
                            newBuys.AppendChild(newBuyElement);
                        }

                        // Add child nodes (sales)
                        var newSales = xmlPortfolio.CreateElement(GeneralSalesAttrName);
                        newShareNode.AppendChild(newSales);

                        // Add child nodes (dividend)
                        var newDividend = xmlPortfolio.CreateElement(GeneralDividendsAttrName);
                        newDividend.SetAttribute(DividendPayoutIntervalAttrName,
                            shareObject.DividendPayoutIntervalAsStr);
                        newShareNode.AppendChild(newDividend);

                        #endregion DailyValues / Brokerage / Buys / Sales / Dividends

                        // Add share name to XML
                        rootPortfolio.AppendChild(newShareNode);
                    }
                }
                else
                {
                    exception = null;
                    return false;
                }
                // Close reader for saving
                xmlReaderPortfolio.Close();
                // Save settings
                xmlPortfolio.Save(strPortfolioFileName);
                // Create a new reader
                xmlReaderPortfolio = XmlReader.Create(strPortfolioFileName, xmlReaderSettingsPortfolio);
                xmlPortfolio.Load(strPortfolioFileName);
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }

            exception = null;
            return true;
        }

        #endregion Save share object

        #region Portfolio values 

        #region Portfolio performance values

        /// <summary>
        /// This function calculates the performance of the portfolio
        /// </summary>
        private void CalculatePortfolioPerformance()
        {
            if (PortfolioPurchaseValue != 0)
                PortfolioPerformanceValue = PortfolioMarketValue * 100 / PortfolioPurchaseValue - 100;
            else
                PortfolioPerformanceValue = 0;

            if (PortfolioCompletePurchaseValue != 0)
                PortfolioCompletePerformanceValue =
                    PortfolioCompleteMarketValueWithProfitLoss * 100 / PortfolioCompletePurchaseValue - 100;
            else
                PortfolioCompletePerformanceValue = 0;

#if DEBUG_MARKET_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformancePortfolio() / MarketValue");
            Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
            Console.WriteLine(@"PortfolioPurchaseValue: {0}", PortfolioPurchaseValue);
            Console.WriteLine(@"PortfolioPerformanceValue: {0}", PortfolioPerformanceValue);
            Console.WriteLine(@"PortfolioCompleteMarketValue: {0}", PortfolioCompleteMarketValue);
            Console.WriteLine(@"PortfolioPurchaseValue: {0}", PortfolioPurchaseValue);
            Console.WriteLine(@"PortfolioCompletePerformanceValue: {0}", PortfolioCompletePerformanceValue);
#endif
        }

        /// <summary>
        /// This function calculates the profit or loss of the portfolio
        /// </summary>
        private void CalculatePortfolioProfitLoss()
        {
            PortfolioProfitLossValue = PortfolioMarketValue - PortfolioPurchaseValue;
            PortfolioCompleteProfitLossValue = PortfolioCompleteMarketValueWithProfitLoss - PortfolioCompletePurchaseValue;

#if DEBUG_MARKET_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePortfolioProfitLoss() / MarketValue");
            Console.WriteLine(@"PortfolioMarketValue: {0}", PortfolioMarketValue);
            Console.WriteLine(@"PortfolioPurchaseValue: {0}", PortfolioPurchaseValue);
            Console.WriteLine(@"PortfolioProfitLossFinalValue: {0}", PortfolioProfitLossValue);
            Console.WriteLine(@"PortfolioCompleteMarketValueWithProfitLoss: {0}", PortfolioCompleteMarketValueWithProfitLoss);
            Console.WriteLine(@"PortfolioCompletePurchaseValue: {0}", PortfolioCompletePurchaseValue);
            Console.WriteLine(@"PortfolioCompleteProfitLossValue: {0}", PortfolioCompleteProfitLossValue);
#endif
        }

        #endregion Portfolio performance values

        /// <summary>
        /// This function resets the portfolio values of the share objects
        /// </summary>
        public static void PortfolioValuesReset()
        {
            _portfolioMarketValue = 0;
            _portfolioMarketValueWithProfitLoss = 0;
            _portfolioPurchaseValue = 0;
            _portfolioSoldPurchaseValue = 0;
            _portfolioPerformanceValue = 0;
            _portfolioProfitLossValue = 0;
        }

        #endregion Portfolio values

        /// <inheritdoc />
        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="bDisposing">Flag if the dispose is called</param>
        protected override void Dispose(bool bDisposing)
        {
            if (Disposed)
                return;

            if (bDisposing)
            {
                // Free any other managed objects here.
                // -
            }

            // Free any unmanaged objects here.

#if DEBUG_MARKET_SHARE_OBJECT
            Console.WriteLine(@"ShareObjectMarketValue destructor...");
#endif
            //if (MarketValue > decimal.MinValue / 2)
            //    PortfolioMarketValue -= MarketValue;
            if (PurchaseValue > decimal.MinValue / 2)
                PortfolioPurchaseValue -= PurchaseValue;
            Disposed = true;

            // Call base class implementation.
            base.Dispose(bDisposing);
        }

        #endregion Methods
    }
}
