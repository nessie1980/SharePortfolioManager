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
//#define DEBUG_FINAL_SHARE_OBJECT

using Parser;
using SharePortfolioManager.Classes.Brokerage;
using SharePortfolioManager.Classes.Dividend;
using SharePortfolioManager.Classes.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;

namespace SharePortfolioManager.Classes.ShareObjects
{
    /// <inheritdoc>
    ///     <cref>ShareObject</cref>
    /// </inheritdoc>
    /// <summary>
    /// This class stores the final value of the share portfolio.
    /// It includes the dividends and brokerage.
    /// </summary>
    [Serializable]
    public class ShareObjectFinalValue : ShareObject, ICloneable
    {
        #region Variables

        #region General variables

        /// <summary>
        /// Stores the current market vale of the current stock volume (final value)
        /// </summary>
        private decimal _finalValue = decimal.MinValue / 2;

        /// <summary>
        /// Stores the current market value of the current stock volume with dividends, brokerage, profits and loss (final value)
        /// </summary>
        private decimal _finalValueWithProfitLoss = decimal.MinValue / 2;

        /// <summary>
        /// Stores the purchase value of the current stock volume with brokerage (final value)
        /// </summary>
        private decimal _purchaseValue = decimal.MinValue / 2;

        #endregion General variables

        #region Portfolio value variables

        /// <summary>
        /// Stores the current purchase value of the current stock volume with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioPurchaseValue;

        /// <summary>
        /// Stores the complete purchase value of all transactions with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioCompletePurchaseValue;

        /// <summary>
        /// Stores the current market value of the current stock volume with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioFinalValue;

        /// <summary>
        /// Stores the current market value of the current stock volume with brokerage and profits / loss of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioFinalValueWithProfitLoss;

        /// <summary>
        /// Stores the complete market value with brokerage, dividends and profit / loss of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioCompleteFinalValue;

        /// <summary>
        /// Stores the sold purchase value used by the sales of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioSoldPurchaseValue;

        /// <summary>
        /// Stores the brokerage of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioCompleteBrokerage;

        /// <summary>
        /// Stores the brokerage - reduction of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioCompleteBrokerageReduction;

        /// <summary>
        /// Stores the dividends of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioCompleteDividends;

        /// <summary>
        /// Stores the performance value of the current stock volume with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioPerformanceValue;

        /// <summary>
        /// Stores the performance value with brokerage, dividends and profit / loss of all transactions (all shares) (final value)
        /// </summary>
        private static decimal _portfolioCompletePerformanceValue;

        /// <summary>
        /// Stores the profit / loss of the current stock volume with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        private static decimal _portfolioProfitLossValue;

        /// <summary>
        /// Stores the profit / loss of all transactions of the portfolio (all shares) (final value)
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

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();
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
                CalculateFinalValue();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();
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
                // Set the previous day share price
                if (value == base.PrevPrice) return;

                base.PrevPrice = value;

                // Recalculate the performance to the previous day
                CalculateCurPrevDayPerformance();

                // Recalculate the profit or loss to the previous day
                CalculateCurPrevDayProfitLoss();

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();
            }
        }

        #region Purchase value properties

        /// <summary>
        /// Purchase value of the current share value without dividends, brokerage, profits and loss (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PurchaseValue
        {
            get => _purchaseValue;
            set
            {
                // Set the new purchase price value
                if (value == _purchaseValue) return;

                // Recalculate portfolio purchase price value
                if (_purchaseValue > decimal.MinValue / 2)
                    PortfolioPurchaseValue -= _purchaseValue;
                PortfolioPurchaseValue += value;

                _purchaseValue = value;

                // Recalculate the total final value of the share
                CalculateFinalValue();

                // Recalculate the profit or loss of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();
            }
        }

        /// <summary>
        /// Purchase price of the current share volume as string
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueAsStr => Helper.FormatDecimal(PurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Purchase price of the current share volume as string with unit
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueAsStrUnit => Helper.FormatDecimal(PurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Purchase value properties

        /// <inheritdoc />
        /// <summary>
        /// Total sale value of the share
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
            set
            {
                base.CultureInfo = value;

                // Set culture info to the lists
                AllBrokerageEntries?.SetCultureInfo(base.CultureInfo);
                AllDividendEntries?.SetCultureInfo(base.CultureInfo);
            }
        }

        #endregion General properties

        #region Final value properties

        /// <summary>
        /// Stores the current final value of the current stock volume (final value)
        /// </summary>
        [Browsable(false)]
        public decimal FinalValue
        {
            get => _finalValue;
            set
            {
                if (value.Equals(_finalValue)) return;

                // Recalculate the portfolio market value of all shares
                if (_finalValue > decimal.MinValue / 2)
                {
                    PortfolioFinalValue -= _finalValue;
                    PortfolioCompleteFinalValue -= _finalValue;
                }

                PortfolioFinalValue += value;
                PortfolioCompleteFinalValue += value;

                _finalValue = value;

#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"_finalValue: {0}", _finalValue);
                Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
                Console.WriteLine(@"PortfolioCompleteFinalValue: {0}", PortfolioCompleteFinalValue);
#endif
            }
        }

        /// <summary>
        /// Stores the current final value of the current stock volume as string (final value)
        /// </summary>
        [Browsable(false)]
        public string FinalValueAsStr => Helper.FormatDecimal(FinalValue, Helper.CurrencyTwoLength, true,
            Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Stores the current final value of the current stock volume as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string FinalValueAsStrUnit => Helper.FormatDecimal(FinalValue, Helper.CurrencyTwoLength, true,
            Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the current final value of the current stock volume with profit / loss (final value)
        /// </summary>
        [Browsable(false)]
        public decimal FinalValueWithProfitLoss
        {
            get => _finalValueWithProfitLoss;
            set
            {
                if (value.Equals(_finalValueWithProfitLoss)) return;

                // Recalculate the portfolio market value of all shares
                if (_finalValueWithProfitLoss > decimal.MinValue / 2)
                    PortfolioFinalValueWithProfitLoss -= _finalValueWithProfitLoss;
                PortfolioFinalValueWithProfitLoss += value;

                _finalValueWithProfitLoss = value;
            }
        }

        /// <summary>
        /// Stores the current final value of the current stock volume with profit / loss as string (final value)
        /// </summary>
        [Browsable(false)]
        public string FinalValueWithProfitLossAsStr => Helper.FormatDecimal(FinalValueWithProfitLoss,
            Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Stores the current final value of the current stock volume with profit / loss as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string FinalValueWithProfitLossAsStrUnit => Helper.FormatDecimal(FinalValueWithProfitLoss,
            Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete final value with profit / loss of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal CompleteFinalValue => SalePayoutBrokerageReduction + FinalValue + CompleteDividendValue;

        /// <summary>
        /// Complete final value with profit / loss of all transactions as string (final value)
        /// </summary>
        [Browsable(false)]
        public string CompleteFinalValueAsStr => Helper.FormatDecimal(CompleteFinalValue, Helper.CurrencyTwoLength,
            false, Helper.CurrencyTwoFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Complete market value with profit / loss of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string CompleteFinalValueAsStrUnit => Helper.FormatDecimal(CompleteFinalValue, Helper.CurrencyTwoLength,
            false, Helper.CurrencyTwoFixLength, true, @"", CultureInfo);

        #endregion Final value properties

        #region Brokerage properties

        /// <summary>
        /// Complete brokerage value of of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal CompleteBrokerageValue { get; internal set; }

        /// <summary>
        /// Complete brokerage value of of all transactions as string (final value)
        /// </summary>
        [Browsable(false)]
        public string CompleteBrokerageValueAsStr => Helper.FormatDecimal(CompleteBrokerageValue, Helper.CurrencyTwoLength,
            true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Complete brokerage value - reduction value of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal CompleteBrokerageReductionValue { get; internal set; }

        /// <summary>
        /// Complete brokerage value - reduction value of all transactions as string (final value)
        /// </summary>
        [Browsable(false)]
        public string CompleteBrokerageReductionValueAsStr => Helper.FormatDecimal(CompleteBrokerageReductionValue,
            Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// List of all brokerages of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public AllBrokerageReductionOfTheShare AllBrokerageEntries { get; set; } = new AllBrokerageReductionOfTheShare();

        #endregion Brokerage properties

        #region Dividends properties

        /// <summary>
        /// Complete dividend value of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal CompleteDividendValue { get; internal set; }

        /// <summary>
        /// Complete dividend value of all transactions as string (final value)
        /// </summary>
        [Browsable(false)]
        public string CompleteDividendValueAsStrUnit => Helper.FormatDecimal(CompleteDividendValue, Helper.DividendTwoLength, true, Helper.DividendNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Dividend payout interval (final value)
        /// </summary>
        [Browsable(false)]
        public int DividendPayoutInterval { get; set; }

        /// <summary>
        /// Dividend payout interval as string (final value)
        /// </summary>
        [Browsable(false)]
        public string DividendPayoutIntervalAsStr => DividendPayoutInterval.ToString();

        /// <summary>
        /// List of all dividends of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public AllDividendsOfTheShare AllDividendEntries { get; set; } = new AllDividendsOfTheShare();

        #endregion Dividend properties

        #region Brokerage and dividends with taxes

        /// <summary>
        /// Complete brokerage and dividend of all transactions as string with unit and line break (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string CompleteBrokerageDividendTotalAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CompleteBrokerageValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(CompleteDividendValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Complete brokerage - reduction and dividend of all transactions as string with unit and line break (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string CompleteBrokerageReductionDividendTotalAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(CompleteBrokerageReductionValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(CompleteDividendValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Brokerage and dividends with taxes

        #region Buy value properties

        /// <summary>
        /// Complete buy volume of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyVolume => AllBuyEntries.BuyVolumeTotal;

        /// <summary>
        /// Complete buy volume of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string BuyVolumeAsStrUnit => Helper.FormatDecimal(BuyVolume, Helper.VolumeTwoLength, true, Helper.VolumeNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions without brokerage and reduction (final value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValue => AllBuyEntries.BuyValueTotal;

        /// <summary>
        /// Complete buy value of all transactions without brokerage and reduction as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueAsStrUnit => Helper.FormatDecimal(BuyValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions without brokerage and with reduction (final value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueReduction => AllBuyEntries.BuyValueReductionTotal;

        /// <summary>
        /// Complete buy value of all transactions without brokerage and with reduction as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueReductionAsStrUnit => Helper.FormatDecimal(BuyValueReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions with brokerage and without reduction (final value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueBrokerage => AllBuyEntries.BuyValueBrokerageTotal;

        /// <summary>
        /// Complete buy value of all transactions with brokerage and without reduction as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueBrokerageAsStrUnit => Helper.FormatDecimal(BuyValueBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions with brokerage and reduction (final value)
        /// </summary>
        [Browsable(false)]
        public decimal BuyValueBrokerageReduction => AllBuyEntries.BuyValueBrokerageReductionTotal;

        /// <summary>
        /// Complete buy value of all transactions with brokerage and reduction as string (final value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueBrokerageReductionAsStr => Helper.FormatDecimal(BuyValueBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Complete buy value of all transactions with brokerage and reduction as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string BuyValueBrokerageReductionAsStrUnit => Helper.FormatDecimal(BuyValueBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Buy value properties

        #region Sale properties

        /// <summary>
        /// Complete sale volume value of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleVolume => AllSaleEntries.SaleVolumeTotal;

        /// <summary>
        /// Complete sale volume value of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SaleVolumeAsStrUnit => Helper.FormatDecimal(SaleVolume, Helper.VolumeTwoFixLength, true, Helper.VolumeNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value without brokerage and reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValue => AllSaleEntries.SalePurchaseValueTotal;

        /// <summary>
        /// Complete sale purchase value without brokerage and reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueAsStrUnit => Helper.FormatDecimal(SalePurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value with brokerage and without reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValueBrokerage => AllSaleEntries.SalePurchaseValueBrokerageTotal;

        /// <summary>
        /// Complete sale purchase value with brokerage and without reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueBrokerageAsStrUnit => Helper.FormatDecimal(SalePurchaseValueBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value without brokerage and with reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValueReduction => AllSaleEntries.SalePurchaseValueReductionTotal;

        /// <summary>
        /// Complete sale purchase value without brokerage and with reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueReductionAsStrUnit => Helper.FormatDecimal(SalePurchaseValueReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale purchase value with brokerage and reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePurchaseValueBrokerageReduction => AllSaleEntries.SalePurchaseValueBrokerageReductionTotal;

        /// <summary>
        /// Complete sale purchase value with brokerage and reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePurchaseValueBrokerageReductionAsStrUnit => Helper.FormatDecimal(SalePurchaseValueBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value without brokerage and reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayout => AllSaleEntries.SalePayoutTotal;

        /// <summary>
        /// Complete sale payout value without brokerage and reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutAsStrUnit => Helper.FormatDecimal(SalePayout, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value with brokerage and without reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayoutBrokerage => AllSaleEntries.SalePayoutBrokerageTotal;

        /// <summary>
        /// Complete sale payout value with brokerage and without reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutBrokerageAsStrUnit => Helper.FormatDecimal(SalePayoutBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value without brokerage and with reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayoutReduction => AllSaleEntries.SalePayoutReductionTotal;

        /// <summary>
        /// Complete sale payout value without brokerage and with reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutReductionAsStrUnit => Helper.FormatDecimal(SalePayoutReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value with brokerage and reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SalePayoutBrokerageReduction => AllSaleEntries.SalePayoutBrokerageReductionTotal;

        /// <summary>
        /// Complete sale payout value with brokerage and reduction of all transactions as string (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutBrokerageReductionAsStr => Helper.FormatDecimal(SalePayoutBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Complete sale payout value with brokerage and reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SalePayoutBrokerageReductionAsStrUnit => Helper.FormatDecimal(SalePayoutBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value without brokerage and reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLoss => AllSaleEntries.SaleProfitLossTotal;

        /// <summary>
        /// Complete sale profit / loss value without brokerage and reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossAsStrUnit => Helper.FormatDecimal(SaleProfitLoss, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value with brokerage and without reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLossBrokerage => AllSaleEntries.SaleProfitLossBrokerageTotal;

        /// <summary>
        /// Complete sale profit / loss value with brokerage and without reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossBrokerageAsStrUnit => Helper.FormatDecimal(SaleProfitLossBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value without brokerage and with reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLossReduction => AllSaleEntries.SaleProfitLossReductionTotal;

        /// <summary>
        /// Complete sale profit / loss value without brokerage and with reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossReductionAsStrUnit => Helper.FormatDecimal(SaleProfitLossReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value with brokerage and reduction of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal SaleProfitLossBrokerageReduction => AllSaleEntries.SaleProfitLossBrokerageReductionTotal;

        /// <summary>
        /// Complete sale profit / loss value with brokerage and reduction of all transactions as string (final value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossBrokerageReductionAsStr => Helper.FormatDecimal(SaleProfitLossBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, false, @"", CultureInfo);

        /// <summary>
        /// Complete sale profit / loss value with brokerage and reduction of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string SaleProfitLossBrokerageReductionAsStrUnit => Helper.FormatDecimal(SaleProfitLossBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Sale properties

        #region Performance value properties

        /// <summary>
        /// Performance value of the market value of the current stock volume without profit / loss (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PerformanceValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance value of the market value of the current stock volume without profit / loss as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string PerformanceValueAsStrUnit => Helper.FormatDecimal(PerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);

        /// <summary>
        /// Performance value of all transactions (final value)
        /// </summary>
        [Browsable(false)]
        public decimal CompletePerformanceValue { get; internal set; } = decimal.MinValue / 2;

        /// <summary>
        /// Performance value of all transactions as string with unit (final value)
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

        #region Portfolio values properties

        /// <summary>
        /// Stores the current purchase value of the current stock volume with brokerage of the portfolio (all shares) (final value)
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
        /// Purchase value of the hole portfolio (all share in the portfolio without dividends, brokerage, profits and loss (final value) ) as string with unit
        /// </summary>
        [Browsable(false)]
        public string PortfolioPurchaseValueAsStrUnit => Helper.FormatDecimal(PortfolioPurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the complete purchase value of all transactions with brokerage of the portfolio (all shares) (final value)
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
        /// Stores the sold purchase value used by the sales of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioSoldPurchaseValue
        {
            get => _portfolioSoldPurchaseValue;
            internal set
            {
                if (_portfolioSoldPurchaseValue.Equals(value)) return;

                _portfolioSoldPurchaseValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioFinalValue
        {
            get => _portfolioFinalValue;
            internal set
            {
                if (_portfolioFinalValue.Equals(value)) return;

                _portfolioFinalValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume with brokerage of the portfolio as string with unit (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioFinalValueAsStrUnit => Helper.FormatDecimal(PortfolioFinalValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the current market value of the current stock volume with brokerage and profits / loss of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioFinalValueWithProfitLoss
        {
            get => _portfolioFinalValueWithProfitLoss;
            internal set
            {
                if (_portfolioFinalValueWithProfitLoss.Equals(value)) return;

                _portfolioFinalValueWithProfitLoss = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();
            }
        }

        /// <summary>
        /// Stores the current market value of the current stock volume with brokerage and profits / loss of the portfolio as string with unit (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioFinalValueWithProfitLossAsStrUnit => Helper.FormatDecimal(PortfolioFinalValueWithProfitLoss, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the complete market value with brokerage, dividends and profit / loss of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompleteFinalValue
        {
            get => _portfolioCompleteFinalValue;
            internal set
            {
                if (_portfolioCompleteFinalValue.Equals(value)) return;

                _portfolioCompleteFinalValue = value;

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

            }
        }

        /// <summary>
        /// Stores the complete market value with brokerage, dividends and profit / loss of all transactions of the portfolio as string with unit (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompleteFinalValueAsStrUnit => Helper.FormatDecimal(PortfolioCompleteFinalValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the brokerage of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompleteBrokerage
        {
            get => _portfolioCompleteBrokerage;
            internal set => _portfolioCompleteBrokerage = value;
        }

        /// <summary>
        /// Stores the brokerage - reduction of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompleteBrokerageReduction
        {
            get => _portfolioCompleteBrokerageReduction;
            internal set => _portfolioCompleteBrokerageReduction = value;
        }

        /// <summary>
        /// Stores the dividends of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompleteDividends
        {
            get => _portfolioCompleteDividends;
            internal set => _portfolioCompleteDividends = value;
        }

        /// <summary>
        /// Stores the performance value of the current stock volume with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioPerformanceValue
        {
            get => _portfolioPerformanceValue;
            internal set => _portfolioPerformanceValue = value;
        }

        /// <summary>
        /// Stores the performance value of the current stock volume with brokerage of the portfolio as string with unit (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioPerformanceValueAsStrUnit => Helper.FormatDecimal(PortfolioPerformanceValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the performance value with brokerage, dividends and profit / loss of all transactions (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompletePerformanceValue
        {
            get => _portfolioCompletePerformanceValue;
            internal set => _portfolioCompletePerformanceValue = value;
        }

        /// <summary>
        /// Stores the performance value with brokerage, dividends and profit / loss of all transactions as string with unit (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompletePerformanceValueAsStrUnit => Helper.FormatDecimal(PortfolioCompletePerformanceValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the profit / loss of the current stock volume with brokerage of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioProfitLossValue
        {
            get => _portfolioProfitLossValue;
            internal set => _portfolioProfitLossValue = value;
        }

        /// <summary>
        /// Stores the profit / loss of the current stock volume with brokerage of the portfolio as string with unit (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioProfitLossValueAsStrUnit => Helper.FormatDecimal(PortfolioProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        /// <summary>
        /// Stores the profit / loss of all transactions of the portfolio (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public decimal PortfolioCompleteProfitLossValue
        {
            get => _portfolioCompleteProfitLossValue;
            internal set => _portfolioCompleteProfitLossValue = value;
        }

        /// <summary>
        /// Stores the profit / loss of all transactions of the portfolio as string with unit(all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompleteProfitLossValueAsStrUnit => Helper.FormatDecimal(PortfolioCompleteProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);

        #endregion Portfolio values properties

        #region Data grid view value properties

        /// <summary>
        /// Profit / loss and performance value of the final value of the current stock volume as string with unit (final value)
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
        /// Complete profit / loss and complete performance value of the final value of all transactions as string with unit (final value)
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
        /// Purchase value and final value of the current stock volume without profit / loss as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string PurchaseValueFinalValueAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(FinalValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Complete purchase value and complete final value with profit / loss of all transactions as string with unit (final value)
        /// </summary>
        [Browsable(false)]
        public string CompletePurchaseValueFinalValueWithProfitLossAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(BuyValueBrokerageReduction, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(CompleteFinalValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Brokerage and dividend of all transactions of the portfolio as string with unit and line break (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompleteBrokerageDividendsAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioCompleteBrokerage, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioCompleteDividends, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Profit / loss value and performance of the portfolio as string with unit and a line break (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioProfitLossPerformanceAsStrUnit
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioProfitLossValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioPerformanceValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, PercentageUnit, CultureInfo);
                return value;
            }
        }

        /// <summary>
        /// Profit / loss and performance of all transactions of the portfolio as string with unit and a line break (all shares) (final value)
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
        /// Purchase and final value of all transactions of the portfolio as string with unit and a line break (all shares) (final value)
        /// </summary>
        [Browsable(false)]
        public string PortfolioCompletePurchaseValueMarketValueAsStr
        {
            get
            {
                var value = Helper.FormatDecimal(PortfolioCompletePurchaseValue, Helper.CurrencyTwoLength, true, Helper.CurrencyNoneFixLength, true, @"", CultureInfo);
                value += Environment.NewLine + Helper.FormatDecimal(PortfolioCompleteFinalValue, Helper.PercentageTwoLength, true, Helper.PercentageNoneFixLength, true, @"", CultureInfo);
                return value;
            }
        }

        #endregion Data grid view value properties

        #region Data grid view properties

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
        /// Brokerage and dividend of the share as string for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"BrokerageDividendTotal")]
        // ReSharper disable once UnusedMember.Global
        public string DgvBrokerageDividendWithTaxesAsStrUnit => CompleteBrokerageDividendTotalAsStrUnit;

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
        /// Image which indicates the performance of the share to the previous day for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"")]
        // ReSharper disable once UnusedMember.Global
        public Image DgvImagePrevDayPerformance => ImagePrevDayPerformance;

        /// <summary>
        /// Profit or loss and performance value of the market value of the share volume as sting with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"ProfitLossPerformanceFinalValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvProfitLossPerformanceValueAsStrUnit => ProfitLossPerformanceAsStrUnit;

        /// <summary>
        /// Purchase value and final value of the share volume as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"PurchaseValueFinalValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvPurchaseValueFinalValueAsStrUnit => PurchaseValueFinalValueAsStrUnit;

        /// <summary>
        /// Profit or loss and performance value of all transactions as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"CompleteProfitLossPerformanceFinalValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvCompleteProfitLossPerformanceValueAsStrUnit => CompleteProfitLossPerformanceAsStrUnit;

        /// <summary>
        /// Purchase value and market value of all transactions as string with unit
        /// for the DataGridView display
        /// </summary>
        [Browsable(true)]
        [DisplayName(@"CompletePurchaseValueFinalValue")]
        // ReSharper disable once UnusedMember.Global
        public string DgvCompletePurchaseValueMarketValueAsStrUnit => CompletePurchaseValueFinalValueWithProfitLossAsStrUnit;

        #endregion Data grid properties

        #endregion Properties

        #region Methods

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Standard constructor
        /// </summary>
        public ShareObjectFinalValue(List<Image> imageList, string percentageUnit, string pieceUnit) : base(imageList, percentageUnit, pieceUnit)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="guid">Guid of the buy</param>
        /// <param name="wkn">WKN number of the share</param>
        /// <param name="bank">Bank name where the buy has been done</param>
        /// <param name="orderNumber">Order number of the buy</param>
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
        /// <param name="dividendPayoutInterval">Interval of the dividend payout</param>
        /// <param name="shareType">Type of the share</param>
        /// <param name="document">Document of the first buy</param>
        public ShareObjectFinalValue(
            string guid, string wkn, string bank, string orderNumber, string addDateTime, string stockMarketLaunchDate, string name,
            DateTime lastUpdateInternet, DateTime lastUpdateShare,
            decimal price, decimal volume, decimal volumeSold, decimal provision, decimal brokerFee, decimal traderPlaceFee, decimal reduction,
            string webSite, string dailyValuesWebSite, List<Image> imageListForDayBeforePerformance, RegExList regexList, CultureInfo cultureInfo,
            int dividendPayoutInterval, ShareTypes shareType, string document)
            : base(wkn, addDateTime, stockMarketLaunchDate, name, lastUpdateInternet, lastUpdateShare,
                    price, webSite, dailyValuesWebSite, imageListForDayBeforePerformance,
                    regexList, cultureInfo, shareType)
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

            AddBuy(guid, bank, orderNumber, AddDateTime, volume, volumeSold, price,
                tempBrokerageObject, document);

            DividendPayoutInterval = dividendPayoutInterval;
        }

        #endregion Constructors

        #region Destructor

        /// <inheritdoc />
        /// <summary>
        /// Destructor
        /// </summary>
        ~ShareObjectFinalValue()
        {
            Dispose(false);
        }

        #endregion Destructor

        #region Brokerage methods

        /// <summary>
        /// This function adds the brokerage for the share to the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="bBrokerageOfABuy">Flag if the brokerage is part of a buy</param>
        /// <param name="bBrokerageOfASale">Flag if the brokerage is part of a Sale</param>
        /// <param name="strGuidBuySale">Guid of the buy or sale</param>
        /// <param name="strDateTime">Date and time of the brokerage</param>
        /// <param name="decProvisionValue">Provision value</param>
        /// <param name="decBrokerFeeValue">Broker fee value</param>
        /// <param name="decTraderPlaceFeeValue">Trader place fee value</param>
        /// <param name="decReductionValue">Reduction value</param>
        /// <param name="strDoc">Doc of the brokerage</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddBrokerage(string strGuid, bool bBrokerageOfABuy, bool bBrokerageOfASale, string strGuidBuySale,
            string strDateTime, decimal decProvisionValue, decimal decBrokerFeeValue, decimal decTraderPlaceFeeValue, decimal decReductionValue, string strDoc = "")
        {
            try
            {
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"AddBrokerage() / FinalValue");
                Console.WriteLine(@"bBrokerageOfABuy: {0}", bBrokerageOfABuy);
                Console.WriteLine(@"bBrokerageOfASale: {0}", bBrokerageOfASale);
                Console.WriteLine(@"strGuidBuySale: {0}", strGuidBuySale);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"decProvision: {0}", decProvisionValue);
                Console.WriteLine(@"decBrokerFee: {0}", decBrokerFeeValue);
                Console.WriteLine(@"decTraderPlaceFee: {0}", decTraderPlaceFeeValue);
                Console.WriteLine(@"decReduction: {0}", decReductionValue);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                // Remove current brokerage of the share from the brokerage of all shares
                PortfolioCompleteBrokerage -= CompleteBrokerageValue;
                PortfolioCompleteBrokerageReduction -= CompleteBrokerageReductionValue;

                if (!AllBrokerageEntries.AddBrokerageReduction(strGuid, bBrokerageOfABuy, bBrokerageOfASale, strGuidBuySale,
                    strDateTime, decProvisionValue, decBrokerFeeValue, decTraderPlaceFeeValue, decReductionValue, strDoc))
                    return false;

                // Set brokerage of the share
                CompleteBrokerageValue = AllBrokerageEntries.BrokerageValueTotal;
                CompleteBrokerageReductionValue = AllBrokerageEntries.BrokerageWithReductionValueTotal;

                // Add new brokerage of the share to the brokerage of all shares
                PortfolioCompleteBrokerage += CompleteBrokerageValue;
                PortfolioCompleteBrokerageReduction += CompleteBrokerageReductionValue;

                // Recalculate purchase price
                if (bBrokerageOfABuy == false && bBrokerageOfASale == false)
                {
                    if (PurchaseValue == decimal.MinValue / 2)
                        PurchaseValue = 0;
                    PurchaseValue += AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime)
                                         .BrokerageValue
                                     - AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime)
                                         .ReductionValue;
                }

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"CompleteBrokerageValue: {0}", CompleteBrokerageValue);
                Console.WriteLine(@"CompleteBrokerageReductionValue: {0}", CompleteBrokerageReductionValue);
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
        /// This function removes a brokerage for the share from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the brokerage</param>
        /// <param name="strDateTime">Date of the brokerage remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveBrokerage(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveBrokerage() / FinalValue");
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get flag if brokerage is part of a buy
                var bPartOfABuy = AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime).PartOfABuy;

                // Get flag if brokerage is part of a buy
                var bPartOfASale = AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime).PartOfASale;

                // Remove current brokerage the share to the brokerage of all shares
                PortfolioCompleteBrokerage -= AllBrokerageEntries.BrokerageValueTotal;
                PortfolioCompleteBrokerageReduction -= AllBrokerageEntries.BrokerageWithReductionValueTotal;

                // Remove brokerage by date
                if (!AllBrokerageEntries.RemoveBrokerageReduction(strGuid, strDateTime))
                    return false;

                // Set brokerage of the share
                CompleteBrokerageValue = AllBrokerageEntries.BrokerageValueTotal;
                CompleteBrokerageReductionValue = AllBrokerageEntries.BrokerageWithReductionValueTotal;

                // Add new brokerage of the share to the brokerage of all shares
                PortfolioCompleteBrokerage += AllBrokerageEntries.BrokerageValueTotal;
                PortfolioCompleteBrokerageReduction += AllBrokerageEntries.BrokerageWithReductionValueTotal;

                // Recalculate purchase price
                if (bPartOfABuy == false && bPartOfASale == false)
                {
                    if (PurchaseValue == decimal.MinValue / 2)
                        PurchaseValue = 0;
                    PurchaseValue -= AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime)
                                         .BrokerageValue
                                     + AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuid, strDateTime)
                                         .ReductionValue;
                }

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"CompleteBrokerageValue: {0}", CompleteBrokerageValue);
                Console.WriteLine(@"CompleteBrokerageReductionValue: {0}", CompleteBrokerageReductionValue);
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

        #endregion Brokerage methods

        #region Buy methods

        /// <summary>
        /// This function adds the buy for the share to the dictionary
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
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"AddBuy() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strBank: {0}", strBank);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"decVolume: {0}", decVolume);
                Console.WriteLine(@"decPrice: {0}", decPrice);
                if (brokerageObject != null)
                {
                    Console.WriteLine(@"decProvision: {0}", brokerageObject.ProvisionValue);
                    Console.WriteLine(@"decBrokerFee: {0}", brokerageObject.BrokerFeeValue);
                    Console.WriteLine(@"decTraderPlaceFee: {0}", brokerageObject.TraderPlaceFeeValue);
                    Console.WriteLine(@"decReduction: {0}", brokerageObject.ReductionValue);
                }
                else
                {
                    Console.WriteLine(@"decProvision: {0}", 0);
                    Console.WriteLine(@"decBrokerFee: {0}", 0);
                    Console.WriteLine(@"decTraderPlaceFee: {0}", 0);
                    Console.WriteLine(@"decReduction: {0}", 0);
                }
                Console.WriteLine(@"strDoc: {0}", strDoc);

                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                Console.WriteLine(@"PortfolioCompletePurchaseValue: {0}", PortfolioCompletePurchaseValue);
#endif
                // Add buy with reductions and brokerage
                if (!AllBuyEntries.AddBuy(strGuid, strDepotNumber, strOrderNumber, strDateTime, decVolume, decVolumeSold, decPrice,
                    brokerageObject, strDoc))
                    return false;

                // Set current stock volume
                Volume = BuyVolume - SaleVolume;

                // Recalculate current purchase value
                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                PurchaseValue += AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValueBrokerageReduction;
                PortfolioCompletePurchaseValue +=
                    AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValueBrokerageReduction;

#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"BuyValueReduction: {0}", AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime).BuyValueReduction);
                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                Console.WriteLine(@"PortfolioCompletePurchaseValue: {0}", PortfolioCompletePurchaseValue);
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
        /// This function removes a buy for the share from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the buy</param>
        /// <param name="strDateTime">Date and time of the buy remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveBuy(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveBuy() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get BuyObject by Guid and date and time and add the sale PurchaseValue and value to the share
                var buyObject = AllBuyEntries.GetBuyObjectByGuidDate(strGuid, strDateTime);
                if (buyObject != null)
                {
                    // Remove buy by Guid and date and time
                    if (!AllBuyEntries.RemoveBuy(strGuid, strDateTime))
                        return false;

                    // Set volume
                    Volume = AllBuyEntries.BuyVolumeTotal - AllSaleEntries.SaleVolumeTotal;

                    PurchaseValue -= buyObject.BuyValueBrokerageReduction;
                    PortfolioCompletePurchaseValue -= buyObject.BuyValueBrokerageReduction;

#if DEBUG_FINAL_SHARE_OBJECT
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
                    Console.WriteLine(@"BuyValueBrokerageReduction: {0}", buyObject.BuyValueBrokerageReduction);
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
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"SetBuyDocument() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
                Console.WriteLine(@"strDocument: {0}", strDocument);
#endif
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
        /// This function adds the sale for the share to the dictionary
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
        /// <param name="brokerageObject">Brokerage object of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the add was successful</returns>
        public bool AddSale(string strGuid, string strDate, string strDepotNumber, string strOrderNumber, decimal decVolume, decimal decSalePrice, List<SaleBuyDetails> usedBuyDetails, decimal decTaxAtSource, decimal decCapitalGainsTax,
             decimal decSolidarityTax, BrokerageReductionObject brokerageObject, string strDoc = "")
        {
            try
            {
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"AddSale() / FinalValue");
                Console.WriteLine(@"strDateTime: {0}", strDate);
                Console.WriteLine(@"decVolume: {0}", decVolume);
                Console.WriteLine(@"decSalePrice: {0}", decSalePrice);
                Console.WriteLine(@"decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine(@"decCapitalGainsTax: {0}", decCapitalGainsTax);
                Console.WriteLine(@"decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine(@"decBrokerage: {0}", brokerageObject?.BrokerageValue ?? 0);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                if (!AllSaleEntries.AddSale(strGuid, strDate, strDepotNumber, strOrderNumber, decVolume, decSalePrice, usedBuyDetails, decTaxAtSource, decCapitalGainsTax,
                                            decSolidarityTax, brokerageObject, strDoc))
                    return false;
                
                // Set volume
                Volume = AllBuyEntries.BuyVolumeTotal - AllSaleEntries.SaleVolumeTotal;

                // Recalculate PurchaseValue and SalePurchaseValueTotal
                if (SoldPurchaseValue == decimal.MinValue / 2)
                    SoldPurchaseValue = 0;

                if (PurchaseValue == decimal.MinValue / 2)
                    PurchaseValue = 0;
                else
                {
                    if (Volume > 0 && PurchaseValue > 0)
                    {
                        SoldPurchaseValue += usedBuyDetails.Sum(x => x.SaleBuyValueBrokerageReduction);
                        PurchaseValue -= usedBuyDetails.Sum(x => x.SaleBuyValueBrokerageReduction);
                    }
                    else
                    {
                        SoldPurchaseValue += PurchaseValue;
                        PurchaseValue = 0;
                    }

                    // Calculate portfolio value
                    if (AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDate).PayoutBrokerageReduction > decimal.MinValue / 2 && FinalValue > decimal.MinValue / 2)
                        PortfolioCompleteFinalValue += (AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDate).PayoutBrokerageReduction);
                }

#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"Volume: {0}", Volume);
                Console.WriteLine(@"SoldPurchaseValue: {0}", SoldPurchaseValue);
                Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
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
        /// This function removes a sale for the share from the dictionary
        /// </summary>
        /// <param name="strGuid">Guid of the sale</param>
        /// <param name="strDateTime">Date and time of the sale remove</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveSale(string strGuid, string strDateTime)
        {
            try
            {
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveSale() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDateTime: {0}", strDateTime);
#endif
                // Get SaleObject by date and time and add the sale deposit and value to the share
                var saleObject = AllSaleEntries.GetSaleObjectByGuidDate(strGuid, strDateTime);
                if (saleObject != null)
                {
                    // Remove sale by Guid and DateTime
                    if (!AllSaleEntries.RemoveSale(strGuid, strDateTime))
                        return false;

                    // Set volume
                    Volume = AllBuyEntries.BuyVolumeTotal - AllSaleEntries.SaleVolumeTotal;

                    PurchaseValue += saleObject.BuyValue;
                    SoldPurchaseValue -= saleObject.BuyValue;

                    // Calculate portfolio value
                    if (saleObject.PayoutBrokerageReduction > decimal.MinValue / 2 && FinalValue > decimal.MinValue / 2)
                        PortfolioCompleteFinalValue -= saleObject.PayoutBrokerageReduction;

#if DEBUG_FINAL_SHARE_OBJECT
                    Console.WriteLine(@"Volume: {0}", Volume);
                    Console.WriteLine(@"SoldPurchaseValue: {0}", SoldPurchaseValue);
                    Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
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
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"SetSaleDocument() / FinalValue");
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

        #region Dividend methods

        /// <summary>
        /// This function adds the dividend payouts for the share to the dictionary
        /// </summary>
        /// <param name="cultureInfoFc">CultureInfo of the share for the foreign currency</param>
        /// <param name="csEnableFc">Flag if the payout is in a foreign currency</param>
        /// <param name="decExchangeRatio">Exchange ratio for the foreign currency</param>
        /// <param name="strGuid">Guid of the dividend</param>
        /// <param name="strDate">Pay date of the new dividend list entry</param>
        /// <param name="decRate">Paid dividend of one share</param>
        /// <param name="decVolume">Share volume at the pay date</param>
        /// <param name="decTaxAtSource">Tax at source value</param>
        /// <param name="decCapitalGainsTax">Capital gains tax value</param>
        /// <param name="decSolidarityTax">Solidarity tax value</param>
        /// <param name="decSharePrice">Share price at the pay date</param>
        /// <param name="strDoc">Document of the dividend</param>
        /// <returns>Flag if the add was successful</returns>  
        public bool AddDividend(CultureInfo cultureInfoFc, CheckState csEnableFc, decimal decExchangeRatio, string strGuid, string strDate, decimal decRate, decimal decVolume,
            decimal decTaxAtSource, decimal decCapitalGainsTax, decimal decSolidarityTax, decimal decSharePrice, string strDoc = "")
        {
            try
            {
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"AddDividend() / FinalValue");
                Console.WriteLine(@"EnableFC: {0}", csEnableFc);
                if (csEnableFc == CheckState.Checked)
                    Console.WriteLine(@"cultureInfoFC: {0}", cultureInfoFc);
                Console.WriteLine(@"decExchangeRatio: {0}", decExchangeRatio);
                Console.WriteLine(@"strDateTime: {0}", strDate);
                Console.WriteLine(@"decDividendRate: {0}", decRate);
                Console.WriteLine(@"decTaxAtSource: {0}", decTaxAtSource);
                Console.WriteLine(@"decCapitalGainsTax: {0}", decCapitalGainsTax);
                Console.WriteLine(@"decSolidarityTax: {0}", decSolidarityTax);
                Console.WriteLine(@"decShareVolume: {0}", decVolume);
                Console.WriteLine(@"decSharePrice: {0}", decSharePrice);
                Console.WriteLine(@"strDoc: {0}", strDoc);
#endif
                if (!AllDividendEntries.AddDividend(cultureInfoFc, csEnableFc, decExchangeRatio, strGuid, strDate, decRate, decVolume,
                    decTaxAtSource, decCapitalGainsTax, decSolidarityTax, decSharePrice, strDoc))
                    return false;

                // Remove current dividend of the share from the dividend of all shares
                PortfolioCompleteDividends -= CompleteDividendValue;
                PortfolioCompleteFinalValue -= CompleteDividendValue;

                // Set dividend of the share
                CompleteDividendValue = AllDividendEntries.DividendValueTotalWithTaxes;

                // Add new dividend of the share to the dividend of all shares
                PortfolioCompleteDividends += CompleteDividendValue;
                PortfolioCompleteFinalValue += CompleteDividendValue;

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"CompleteDividendValue: {0}", CompleteDividendValue);
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
        /// This function removes a dividend payout for the share from the list
        /// </summary>
        /// <param name="strGuid">Guid of the dividend</param>
        /// <param name="strDate">Date of the dividend pay date</param>
        /// <returns>Flag if the remove was successful</returns>
        public bool RemoveDividend(string strGuid, string strDate)
        {
            try
            {
#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"");
                Console.WriteLine(@"RemoveDividend() / FinalValue");
                Console.WriteLine(@"strGuid: {0}", strGuid);
                Console.WriteLine(@"strDate: {0}", strDate);
#endif
                // Remove dividend of all shares
                PortfolioCompleteDividends -= CompleteDividendValue;
                PortfolioCompleteFinalValue -= CompleteDividendValue;

                // Remove dividend by date
                if (!AllDividendEntries.RemoveDividend(strGuid, strDate))
                    return false;

                // Set dividend of the share
                CompleteDividendValue = AllDividendEntries.DividendValueTotalWithTaxes;

                // Add dividend of all shares
                PortfolioCompleteDividends += CompleteDividendValue;
                PortfolioCompleteFinalValue += CompleteDividendValue;

                // Recalculate the total sum of the share
                CalculateFinalValue();

                // Recalculate the profit or lose of the share
                CalculateProfitLoss();

                // Recalculate the appreciation
                CalculatePerformance();

                // Recalculate the performance of all shares
                CalculatePortfolioPerformance();

                // Recalculate the profit or lose of all shares
                CalculatePortfolioProfitLoss();

#if DEBUG_FINAL_SHARE_OBJECT
                Console.WriteLine(@"CompleteDividendValue: {0}", CompleteDividendValue);
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

        #endregion Dividend methods

        #region Performance methods

        /// <summary>
        /// This function calculates the new final value of this share
        /// with dividends and brokerage and sales profit or loss
        /// </summary>
        private void CalculateFinalValue()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossTotal > decimal.MinValue / 2
                )
            {
                if (Volume > 0)
                {
                    FinalValue = CurPrice * Volume;
                    FinalValueWithProfitLoss = CurPrice * Volume
                                 + AllDividendEntries.DividendValueTotalWithTaxes
                                 + AllSaleEntries.SaleProfitLossBrokerageReductionTotal;
                }
                else
                {
                    FinalValue = 0;
                    FinalValueWithProfitLoss = AllDividendEntries.DividendValueTotalWithTaxes
                                 + AllSaleEntries.SaleProfitLossBrokerageReductionTotal;
                }
            }

#if DEBUG_FINAL_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateFinalValue() / FinalValue");
            Console.WriteLine(@"CurrentPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"DividendValueTotal: {0}", AllDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine(@"SaleProfitLossBrokerageReductionTotal: {0}", AllSaleEntries.SaleProfitLossBrokerageReductionTotal);
            Console.WriteLine(@"FinalValue: {0}", FinalValue);
#endif
        }

        /// <summary>
        /// This function calculates the new profit or loss value of this share
        /// with dividends and brokerage and sales profit or loss
        /// </summary>
        private void CalculateProfitLoss()
        {
            if (CurPrice > decimal.MinValue / 2
                && Volume > decimal.MinValue / 2
                && AllDividendEntries.DividendValueTotalWithTaxes > decimal.MinValue / 2
                && AllSaleEntries.SaleProfitLossBrokerageReductionTotal > decimal.MinValue / 2
                )
            {
                if (Volume > 0)
                {
                    ProfitLossValue = CurPrice * Volume
                                      - PurchaseValue;
                    CompleteProfitLossValue = CurPrice * Volume
                                               - PurchaseValue
                                               + AllDividendEntries.DividendValueTotalWithTaxes
                                               + AllSaleEntries.SaleProfitLossBrokerageReductionTotal;
                }
                else
                {
                    ProfitLossValue = 0;
                    CompleteProfitLossValue = AllDividendEntries.DividendValueTotalWithTaxes
                                              + AllSaleEntries.SaleProfitLossBrokerageReductionTotal;
                }
            }

#if DEBUG_FINAL_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculateProfitLoss() / FinalValue");
            Console.WriteLine(@"CurPrice: {0}", CurPrice);
            Console.WriteLine(@"Volume: {0}", Volume);
            Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine(@"DividendValueTotal: {0}", AllDividendEntries.DividendValueTotalWithTaxes);
            Console.WriteLine(@"SaleProfitLossBrokerageReductionTotal: {0}", AllSaleEntries.SaleProfitLossBrokerageReductionTotal);
            Console.WriteLine(@"ProfitLossValue: {0}", ProfitLossValue);
            Console.WriteLine(@"CompleteProfitLossValue: {0}", CompleteProfitLossValue);
#endif
        }

        /// <summary>
        /// This function calculates the new performance of this share.
        /// It calculates two values:
        /// - PerformanceValue: performance value without profit / loss
        /// - PerformanceValueWithProfitLoss: performance value with profit / loss
        /// </summary>
        private void CalculatePerformance()
        {
            if (FinalValue <= decimal.MinValue / 2 || PurchaseValue <= decimal.MinValue / 2
            || CompleteFinalValue <= decimal.MinValue / 2 || BuyValueBrokerageReduction <= decimal.MinValue / 2) return;

            if (PurchaseValue != 0 && BuyValueBrokerageReduction != 0)
            {
                PerformanceValue = (FinalValue / PurchaseValue * 100) - 100;
                CompletePerformanceValue = (CompleteFinalValue / BuyValueBrokerageReduction * 100) - 100;
            }
            else
            {
                PerformanceValue = 0;
                if (BuyValueBrokerageReduction != 0)
                    CompletePerformanceValue = (CompleteFinalValue / BuyValueBrokerageReduction * 100) - 100;
            }

#if DEBUG_FINAL_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformance() / FinalValue");
            Console.WriteLine(@"FinalValue: {0}", FinalValue);
            Console.WriteLine(@"PurchaseValue: {0}", PurchaseValue);
            Console.WriteLine(@"PerformanceValue: {0}", PerformanceValue);
            Console.WriteLine(@"BuyValueReduction: {0}", BuyValueReduction);
            Console.WriteLine(@"CompleteFinalValue: {0}", CompleteFinalValue);
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
        public static bool SaveShareObject(ShareObjectFinalValue shareObject, ref XmlDocument xmlPortfolio,
            ref XmlReader xmlReaderPortfolio, ref XmlReaderSettings xmlReaderSettingsPortfolio,
            string strPortfolioFileName, out Exception exception)
        {
            try
            {
                // Update existing share
                var nodeListShares = xmlPortfolio.SelectNodes(
                    $"/{GeneralPortfolioAttrName}/{GeneralShareAttrName} [@{GeneralWknAttrName} = \"{shareObject.Wkn}\"]");
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

                                case (int) FrmMain.PortfolioParts.StockMarketLaunchDate:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.StockMarketLaunchDate}";
                                    break;
                                case (int) FrmMain.PortfolioParts.LastUpdateInternet:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateViaInternet.ToShortDateString()} {
                                                shareObject.LastUpdateViaInternet.ToShortTimeString()
                                            }";
                                    break;
                                case (int) FrmMain.PortfolioParts.LastUpdateShare:
                                    nodeElement.ChildNodes[i].InnerText =
                                        $@"{shareObject.LastUpdateShare.ToShortDateString()} {
                                                shareObject.LastUpdateShare.ToShortTimeString()
                                            }";
                                    break;
                                case (int) FrmMain.PortfolioParts.SharePrice:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.CurPriceAsStr;
                                    break;
                                case (int) FrmMain.PortfolioParts.SharePriceBefore:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.PrevPriceAsStr;
                                    break;
                                case (int) FrmMain.PortfolioParts.WebSite:
                                    nodeElement.ChildNodes[i].InnerText = shareObject.UpdateWebSiteUrl;
                                    break;
                                case (int) FrmMain.PortfolioParts.Culture:
                                    nodeElement.ChildNodes[i].InnerXml = shareObject.CultureInfoAsStr;
                                    break;
                                case (int) FrmMain.PortfolioParts.ShareType:
                                    nodeElement.ChildNodes[i].InnerXml = ((int)shareObject.ShareType).ToString();
                                    break;

                                #endregion General

                                #region Daily values

                                case (int) FrmMain.PortfolioParts.DailyValues:
                                    // Remove old daily values
                                    while (nodeElement.ChildNodes[i].FirstChild != null)
                                        nodeElement.ChildNodes[i].RemoveChild(nodeElement.ChildNodes[i].FirstChild);
                                    nodeElement.ChildNodes[i].Attributes[DailyValuesWebSiteAttrName].InnerText =
                                        shareObject.DailyValuesUpdateWebSiteUrl;

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

                                case (int) FrmMain.PortfolioParts.Brokerages:
                                    // Remove old brokerage
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var brokerageElementYear in shareObject.AllBrokerageEntries
                                        .GetAllBrokerageOfTheShare())
                                    {
                                        var newBrokerageElement =
                                            xmlPortfolio.CreateElement(BrokerageTagNamePre);
                                        newBrokerageElement.SetAttribute(BrokerageGuidAttrName,
                                            brokerageElementYear.Guid);
                                        newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName,
                                            brokerageElementYear.PartOfABuyAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageSalePartAttrName,
                                            brokerageElementYear.PartOfASaleAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageGuidBuySaleAttrName,
                                            brokerageElementYear.GuidBuySale);
                                        newBrokerageElement.SetAttribute(BrokerageDateAttrName,
                                            brokerageElementYear.DateAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageProvisionAttrName,
                                            brokerageElementYear.ProvisionValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageBrokerFeeAttrName,
                                            brokerageElementYear.BrokerFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageTraderPlaceFeeAttrName,
                                            brokerageElementYear.TraderPlaceFeeValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageReductionAttrName,
                                            brokerageElementYear.ReductionValueAsStr);
                                        newBrokerageElement.SetAttribute(BrokerageDocumentAttrName,
                                            brokerageElementYear.DocumentAsStr);
                                        nodeElement.ChildNodes[i].AppendChild(newBrokerageElement);
                                    }

                                    break;

                                #endregion Brokerage

                                #region Buys

                                case (int) FrmMain.PortfolioParts.Buys:
                                    // Remove old buys
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var buyElementYear in shareObject.AllBuyEntries
                                        .GetAllBuysOfTheShare())
                                    {
                                        var newBuyElement =
                                            xmlPortfolio.CreateElement(BuyTagNamePre);
                                        newBuyElement.SetAttribute(BuyGuidAttrName,
                                            buyElementYear.Guid);
                                        newBuyElement.SetAttribute(BuyDepotNumberAttrName,
                                            buyElementYear.DepotNumberAsStr);
                                        newBuyElement.SetAttribute(BuyOrderNumberAttrName,
                                            buyElementYear.OrderNumberAsStr);
                                        newBuyElement.SetAttribute(BuyDateAttrName,
                                            buyElementYear.DateAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeAttrName,
                                            buyElementYear.VolumeAsStr);
                                        newBuyElement.SetAttribute(BuyVolumeSoldAttrName,
                                            buyElementYear.VolumeSoldAsStr);
                                        newBuyElement.SetAttribute(BuyPriceAttrName,
                                            buyElementYear.PriceAsStr);
                                        newBuyElement.SetAttribute(BuyBrokerageGuidAttrName,
                                            buyElementYear.BrokerageGuid);
                                        newBuyElement.SetAttribute(BuyDocumentAttrName,
                                            buyElementYear.DocumentAsStr);
                                        nodeElement.ChildNodes[i].AppendChild(newBuyElement);
                                    }

                                    break;

                                #endregion Buys

                                #region Sales

                                case (int) FrmMain.PortfolioParts.Sales:
                                    // Remove old sales
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    foreach (var saleElementYear in shareObject.AllSaleEntries
                                        .GetAllSalesOfTheShare())
                                    {
                                        var newSaleElement =
                                            xmlPortfolio.CreateElement(SaleTagNamePre);
                                        newSaleElement.SetAttribute(SaleGuidAttrName,
                                            saleElementYear.Guid);
                                        newSaleElement.SetAttribute(SaleDateAttrName,
                                            saleElementYear.DateAsStr);
                                        newSaleElement.SetAttribute(SaleDepotNumberAttrName,
                                            saleElementYear.DepotNumberAsStr);
                                        newSaleElement.SetAttribute(SaleOrderNumberAttrName,
                                            saleElementYear.OrderNumberAsStr);
                                        newSaleElement.SetAttribute(SaleVolumeAttrName,
                                            saleElementYear.VolumeAsStr);
                                        newSaleElement.SetAttribute(SaleSalePriceAttrName,
                                            saleElementYear.SalePriceAsStr);
                                        newSaleElement.SetAttribute(SaleTaxAtSourceAttrName,
                                            saleElementYear.TaxAtSourceAsStr);
                                        newSaleElement.SetAttribute(SaleCapitalGainsTaxAttrName,
                                            saleElementYear.CapitalGainsTaxAsStr);
                                        newSaleElement.SetAttribute(SaleSolidarityTaxAttrName,
                                            saleElementYear.SolidarityTaxAsStr);
                                        newSaleElement.SetAttribute(SaleReductionAttrName,
                                            saleElementYear.ReductionAsStr);
                                        newSaleElement.SetAttribute(SaleBrokerageGuidAttrName,
                                            saleElementYear.BrokerageGuid);
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

                                #region Dividends

                                case (int) FrmMain.PortfolioParts.Dividends:
                                    // Remove old dividends
                                    nodeElement.ChildNodes[i].RemoveAll();
                                    ((XmlElement) nodeElement.ChildNodes[i]).SetAttribute(
                                        DividendPayoutIntervalAttrName,
                                        shareObject.DividendPayoutIntervalAsStr);

                                    foreach (var dividendObject in shareObject.AllDividendEntries
                                        .GetAllDividendsOfTheShare())
                                    {
                                        var newDividendElement =
                                            xmlPortfolio.CreateElement(DividendTagName);
                                        newDividendElement.SetAttribute(DividendGuidAttrName,
                                            dividendObject.Guid);
                                        newDividendElement.SetAttribute(DividendDateAttrName,
                                            dividendObject.DateAsStr);
                                        newDividendElement.SetAttribute(DividendRateAttrName,
                                            dividendObject.DividendAsStr);
                                        newDividendElement.SetAttribute(DividendVolumeAttrName,
                                            dividendObject.VolumeAsStr);

                                        newDividendElement.SetAttribute(DividendTaxAtSourceAttrName,
                                            dividendObject.TaxAtSourceAsStr);
                                        newDividendElement.SetAttribute(
                                            DividendCapitalGainsTaxAttrName,
                                            dividendObject.CapitalGainsTaxAsStr);
                                        newDividendElement.SetAttribute(
                                            DividendSolidarityTaxAttrName,
                                            dividendObject.SolidarityTaxAsStr);

                                        newDividendElement.SetAttribute(DividendPriceAttrName,
                                            dividendObject.PriceAtPaydayAsStr);
                                        newDividendElement.SetAttribute(DividendDocumentAttrName,
                                            dividendObject.DocumentAsStr);

                                        // Foreign currency information
                                        XmlElement newForeignCurrencyElement =
                                            xmlPortfolio.CreateElement(DividendTagNameForeignCu);

                                        newForeignCurrencyElement.SetAttribute(
                                            DividendForeignCuFlagAttrName,
                                            dividendObject.EnableFcAsStr);

                                        if (dividendObject.EnableFc == CheckState.Checked)
                                        {
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendExchangeRatioAttrName,
                                                dividendObject.ExchangeRatioAsStr);
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendNameAttrName,
                                                dividendObject.CultureInfoFc.Name);
                                        }
                                        else
                                        {
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendExchangeRatioAttrName, 0.ToString());
                                            newForeignCurrencyElement.SetAttribute(
                                                DividendNameAttrName,
                                                dividendObject.DividendCultureInfo.Name);
                                        }

                                        newDividendElement.AppendChild(newForeignCurrencyElement);

                                        nodeElement.ChildNodes[i].AppendChild(newDividendElement);
                                    }

                                    break;

                                #endregion Dividends

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

                        // Add attributes (ShareName)
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

                        // Add child nodes (last update share )
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
                        var shareType = xmlPortfolio.CreateTextNode(((int)shareObject.ShareType).ToString());
                        newShareNode.AppendChild(newShareType);
                        newShareNode.LastChild.AppendChild(shareType);

                        #endregion General

                        #region DailyValues / Brokerage / Buys / Sales / Dividends

                        // Add child nodes (daily values)
                        var newDailyValues = xmlPortfolio.CreateElement(GeneralDailyValuesAttrName);
                        newDailyValues.SetAttribute(DailyValuesWebSiteAttrName,
                            shareObject.DailyValuesUpdateWebSiteUrl);
                        newShareNode.AppendChild(newDailyValues);

                        // Add child nodes (brokerage)
                        var newBrokerage = xmlPortfolio.CreateElement(GeneralBrokeragesAttrName);
                        newShareNode.AppendChild(newBrokerage);
                        foreach (var brokerageElementYear in shareObject.AllBrokerageEntries.GetAllBrokerageOfTheShare()
                        )
                        {
                            var newBrokerageElement = xmlPortfolio.CreateElement(BrokerageTagNamePre);
                            newBrokerageElement.SetAttribute(BrokerageGuidAttrName, brokerageElementYear.Guid);
                            newBrokerageElement.SetAttribute(BrokerageBuyPartAttrName,
                                brokerageElementYear.PartOfABuyAsStr);
                            newBrokerageElement.SetAttribute(BrokerageSalePartAttrName,
                                brokerageElementYear.PartOfASaleAsStr);
                            newBrokerageElement.SetAttribute(BrokerageGuidBuySaleAttrName,
                                brokerageElementYear.GuidBuySale);
                            newBrokerageElement.SetAttribute(BrokerageDateAttrName, brokerageElementYear.DateAsStr);
                            newBrokerageElement.SetAttribute(BrokerageProvisionAttrName,
                                brokerageElementYear.ProvisionValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageBrokerFeeAttrName,
                                brokerageElementYear.BrokerFeeValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageTraderPlaceFeeAttrName,
                                brokerageElementYear.TraderPlaceFeeValueAsStr);
                            newBrokerageElement.SetAttribute(BrokerageReductionAttrName,
                                brokerageElementYear.ReductionValueAsStr);
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

        #region Portfolio methods

        #region Portfolio performance values

        /// <summary>
        /// This function calculates the performance of the portfolio
        /// </summary>
        private void CalculatePortfolioPerformance()
        {
            if (PortfolioPurchaseValue != 0)
                PortfolioPerformanceValue = (PortfolioFinalValue) * 100 / (PortfolioPurchaseValue) - 100;
            else
                PortfolioPerformanceValue = 0;

            if (PortfolioCompletePurchaseValue != 0)
                PortfolioCompletePerformanceValue = (PortfolioCompleteFinalValue) * 100 / (PortfolioCompletePurchaseValue) - 100;
            else
                PortfolioCompletePerformanceValue = 0;

#if DEBUG_FINAL_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePerformancePortfolio() / FinalValue");
            Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine(@"PortfolioSalePurchaseValue: {0}", PortfolioPurchaseValue);
            Console.WriteLine(@"PortfolioPerformanceValue: {0}", PortfolioPerformanceValue);
#endif
        }

        /// <summary>
        /// This function calculates the profit or loss of the portfolio
        /// </summary>
        private void CalculatePortfolioProfitLoss()
        {
            PortfolioProfitLossValue = PortfolioFinalValue - PortfolioPurchaseValue;
            PortfolioCompleteProfitLossValue = PortfolioCompleteFinalValue - PortfolioCompletePurchaseValue;

#if DEBUG_FINAL_SHARE_OBJECT
            Console.WriteLine(@"");
            Console.WriteLine(@"CalculatePortfolioProfitLoss() / FinalValue");
            Console.WriteLine(@"PortfolioFinalValue: {0}", PortfolioFinalValue);
            Console.WriteLine(@"PortfolioPurchaseValue: {0}", PortfolioPurchaseValue);
            Console.WriteLine(@"PortfolioProfitLossValue: {0}", PortfolioProfitLossValue);
            Console.WriteLine(@"PortfolioCompleteFinalValue: {0}", PortfolioCompleteFinalValue);
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
            _portfolioPurchaseValue = 0;
            _portfolioSoldPurchaseValue = 0;
            _portfolioFinalValue = 0;
            _portfolioCompleteBrokerage = 0;
            _portfolioCompleteDividends = 0;
            _portfolioPerformanceValue = 0;
            _portfolioProfitLossValue = 0;
        }

        #endregion Portfolio methods

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

#if DEBUG_FINAL_SHARE_OBJECT
            Console.WriteLine(@"ShareObjectFinalValue destructor...");
#endif

            if (PurchaseValue > decimal.MinValue / 2)
                PortfolioPurchaseValue -= PurchaseValue;
            if (FinalValue > decimal.MinValue / 2)
                PortfolioFinalValue -= FinalValue;
            if (CompleteDividendValue > decimal.MinValue / 2)
                PortfolioCompleteDividends -= CompleteDividendValue;
            if (CompleteBrokerageValue > decimal.MinValue / 2)
                PortfolioCompleteBrokerage -= CompleteBrokerageValue;
            if (CompleteBrokerageReductionValue > decimal.MinValue / 2)
                PortfolioCompleteBrokerageReduction -= CompleteBrokerageReductionValue;

            Disposed = true;

            // Call base class implementation.
            base.Dispose(bDisposing);
        }

        /// <inheritdoc />
        /// <summary>
        /// This function allows to clone this object
        /// </summary>
        /// <returns>New clone of the object</returns>
        public new object Clone()
        {
            using (var stream = new MemoryStream())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                if (!GetType().IsSerializable) return null;

                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        #endregion Methods
    }
}
