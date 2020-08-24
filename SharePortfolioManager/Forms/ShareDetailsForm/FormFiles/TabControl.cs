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
//#define DEBUG_TAB_CONTROL

using SharePortfolioManager.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharePortfolioManager.ShareDetailsForm
{
    public partial class FrmShareDetails
    {
        #region Tab control details

        /// <summary>
        /// This function updates the dividend, brokerage and profit / loss values in the tab controls
        /// if one of them three pages are chosen.
        /// </summary>
        /// <param name="sender">TabControl</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the profit or loss page / update the dividend page / update the brokerage page
            switch (((TabControl)sender).SelectedIndex)
            {
                case (int)EDetailsPageNumber.ProfitLoss:
                    UpdateProfitLossDetails();
                    break;
                case (int)EDetailsPageNumber.Dividend:
                    UpdateDividendDetails();
                    break;
                case (int)EDetailsPageNumber.Brokerage:
                    UpdateBrokerageDetails();
                    break;
            }
        }

        #region Share details

        /// <summary>
        /// This function updates the details information of a share
        /// </summary>
        private void UpdateShareDetails(bool bShowMarketValue)
        {
            try
            {
                // Check which share object should be show
                if (bShowMarketValue)
                {
                    // Check if a share is selected
                    if (ShareObjectMarketValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    // Set GroupBox caption
                    // Check if an update has already done
                    if (ShareObjectMarketValue.LastUpdateViaInternet == DateTime.MinValue)
                    {
                        grpBoxShareDetails.Text =
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/ShareUpdate",
                                LanguageName) + @" " +
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName) +
                            @" / " +
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareType",
                                LanguageName) +
                            @" " +
                            Language.GetLanguageTextListByXPath(@"/ComboBoxItemsShareType/*", LanguageName)[
                                (int)ShareObjectMarketValue.ShareType];
                    }
                    else
                    {
                        grpBoxShareDetails.Text =
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/ShareUpdate",
                                LanguageName) + @" " +
                            string.Format(Helper.DateFullTimeShortFormat, ShareObjectMarketValue.LastUpdateViaInternet) +
                            @" / " +
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareType",
                                LanguageName) +
                            @" " +
                            Language.GetLanguageTextListByXPath(@"/ComboBoxItemsShareType/*", LanguageName)[
                                (int)ShareObjectMarketValue.ShareType];
                    }

                    if (ShareObjectMarketValue.LastUpdateShare == DateTime.MinValue)
                    {
                        // Set the share update date
                        lblDetailsMarketValueDateValue.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName);
                    }
                    else
                    {
                        // Set the share update date
                        lblDetailsMarketValueDateValue.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareDateTime",
                                LanguageName) + @" " +
                            string.Format(Helper.DateFullFormat, ShareObjectMarketValue.LastUpdateShare) + @" " +
                            string.Format(Helper.TimeShortFormat, ShareObjectMarketValue.LastUpdateShare);
                    }

                    #region Overall calculation

                    // Set share volume
                    lblDetailsMarketValueTotalVolumeValue.Text =
                        ShareObjectMarketValue.VolumeAsStrUnit;

                    // Set current price
                    lblDetailsMarketValueTotalCurPriceValue.Text =
                        ShareObjectMarketValue.CurPriceAsStrUnit;

                    // Set purchase value
                    lblDetailsMarketValueTotalPurchaseValue.Text =
                        ShareObjectMarketValue.MarketValueAsStrUnit;

                    // Set sale value
                    lblDetailsMarketValueTotalSaleValue.Text =
                        ShareObjectMarketValue.SalePayoutReductionAsStrUnit;

                    // Set dividend value
                    lblDetailsMarketValueTotalDividendValue.Text = @"-";
                    // Disable the dividend labels 
                    lblDetailsMarketValueTotalDividend.Enabled = false;
                    lblDetailsMarketValueTotalDividendValue.Enabled = false;

                    // Set total share value
                    lblDetailsMarketValueTotalSumValue.Text =
                        ShareObjectMarketValue.CompleteMarketValueAsStrUnit;
                    
                    // Set total purchase value
                    lblDetailsMarketValueTotalSalePurchaseValue.Text =
                        ShareObjectMarketValue.BuyValueReductionAsStrUnit;

                    // Set profit or lose of the share
                    lblDetailsMarketValueTotalProfitLossValue.Text =
                        ShareObjectMarketValue.CompleteProfitLossValueAsStrUnit;

                    // Set performance of the share
                    lblDetailsMarketValueTotalPerformanceValue.Text =
                        ShareObjectMarketValue.CompletePerformanceValueAsStrUnit;

                    // Format performance value
                    if (ShareObjectMarketValue.PerformanceValue >= 0)
                    {
                        lblDetailsMarketValueTotalPerformanceValue.ForeColor = Color.Green;
                        lblDetailsMarketValueTotalProfitLossValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsMarketValueTotalPerformanceValue.ForeColor = Color.Red;
                        lblDetailsMarketValueTotalProfitLossValue.ForeColor = Color.Red;
                    }

                    #endregion Overall calculation

                    #region Current calculation

                    // Set share volume
                    lblDetailsMarketValueCurrentVolumeValue.Text =
                        ShareObjectMarketValue.VolumeAsStrUnit;

                    // Set the current price value
                    lblDetailsMarketValueCurrentCurPriceValue.Text =
                        ShareObjectMarketValue.CurPriceAsStrUnit;

                    // Set current value of the volume of the shares
                    lblDetailsMarketValueCurrentPurchaseValue.Text =
                        ShareObjectMarketValue.MarketValueAsStrUnit;

                    // Set dividend value
                    lblDetailsMarketValueCurrentDividendValue.Text = @"-";
                    // Disable the dividend labels 
                    lblDetailsMarketValueCurrentDividend.Enabled = false;
                    lblDetailsMarketValueCurrentDividendValue.Enabled = false;

                    // Set total share value
                    lblDetailsMarketValueCurrentProfitLossSaleValue.Text =
                        ShareObjectMarketValue.SaleProfitLossReductionAsStrUnit;

                    // Set total value of the share
                    lblDetailsMarketValueCurrentSumValue.Text =
                        ShareObjectMarketValue.MarketValueWithProfitLossAsStrUnit;

                    #endregion Current calculation

                    #region Daily calculation

                    // Set the current price value
                    lblDetailsMarketValuePrevDayCurPriceValue.Text =
                        ShareObjectMarketValue.CurPriceAsStrUnit;

                    // Set previous price value
                    lblDetailsMarketValuePrevDayPrevPriceValue.Text =
                        ShareObjectMarketValue.PrevPriceAsStrUnit;

                    // Set difference price value
                    lblDetailsMarketValuePrevDayDiffPriceValue.Text =
                        ShareObjectMarketValue.CurPrevDayPriceDifferenceAsStrUnit;

                    // Set performance previous value
                    lblDetailsMarketValuePrevDayDiffPerformanceValue.Text =
                        ShareObjectMarketValue.CurPrevDayPricePerformanceAsStrUnit;

                    // Set share volume
                    lblDetailsMarketValuePrevDayVolumeValue.Text =
                        ShareObjectMarketValue.VolumeAsStrUnit;

                    // Set difference price value
                    lblDetailsMarketValuePrevDayDiffPriceValueValue.Text =
                        ShareObjectMarketValue.CurPrevDayPriceDifferenceAsStrUnit;

                    // Set difference previous value
                    lblDetailsMarketValueDiffSumPrevValue.Text =
                        ShareObjectMarketValue.CurPrevDayProfitLossAsStrUnit;

                    lblDetailsMarketValuePrevDayDiffPriceValue.ForeColor = ShareObjectMarketValue.CurPrevDayPricePerformance >= 0 ? Color.Green : Color.Red;
                    lblDetailsMarketValuePrevDayDiffPerformanceValue.ForeColor = ShareObjectMarketValue.CurPrevDayPricePerformance >= 0 ? Color.Green : Color.Red;
                    lblDetailsFinalValueDiffSumPrevValue.ForeColor = ShareObjectMarketValue.CurPrevDayProfitLoss >= 0 ? Color.Green : Color.Red;

                    #endregion Daily calclation
                }
                else
                {
                    // Check if a share is selected
                    if (ShareObjectFinalValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    // Set GroupBox caption
                    // Check if an update has already done
                    if (ShareObjectFinalValue.LastUpdateViaInternet == DateTime.MinValue)
                    {
                        grpBoxShareDetails.Text =
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/ShareUpdate",
                                LanguageName) + @" " +
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName) +
                            @" / " +
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareType",
                                LanguageName) +
                            @" " +
                            Language.GetLanguageTextListByXPath(@"/ComboBoxItemsShareType/*", LanguageName)[
                                (int)ShareObjectFinalValue.ShareType];
                    }
                    else
                    {
                        grpBoxShareDetails.Text =
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/ShareUpdate",
                                LanguageName) + @" " +
                            string.Format(Helper.DateFullTimeShortFormat, ShareObjectFinalValue.LastUpdateViaInternet) +
                            @" / " +
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareType",
                                LanguageName) +
                            @" " +
                            Language.GetLanguageTextListByXPath(@"/ComboBoxItemsShareType/*", LanguageName)[
                                (int)ShareObjectFinalValue.ShareType];
                    }

                    if (ShareObjectFinalValue.LastUpdateShare == DateTime.MinValue)
                    {
                        // Set the share update date
                        lblDetailsFinalValueDateValue.Text += @" " +
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName);
                    }
                    else
                    {
                        // Set the share update date
                        lblDetailsFinalValueDateValue.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/ShareDateTime",
                                LanguageName) + @" " +
                            string.Format(Helper.DateFullFormat, ShareObjectFinalValue.LastUpdateShare) + @" " +
                            string.Format(Helper.TimeShortFormat, ShareObjectFinalValue.LastUpdateShare);
                    }

                    #region Overall calculation

                    // Set share volume
                    lblDetailsFinalValueTotalVolumeValue.Text =
                        ShareObjectFinalValue.VolumeAsStrUnit;

                    // Set the current price value
                    lblDetailsFinalValueTotalCurPriceValue.Text =
                        ShareObjectFinalValue.CurPriceAsStrUnit;

                    // Set current value of the volume of the shares
                    lblDetailsFinalValueTotalPurchaseValue.Text =
                        ShareObjectFinalValue.FinalValueAsStrUnit;

                    // Set dividend value
                    lblDetailsFinalValueTotalDividendValue.Text =
                        ShareObjectFinalValue.CompleteDividendValueAsStrUnit;

                    // Set sale value
                    lblDetailsFinalValueTotalSaleValue.Text =
                        ShareObjectFinalValue.SalePayoutBrokerageReductionAsStrUnit;

                    // Set total share value
                    lblDetailsFinalValueTotalSumValue.Text =
                        ShareObjectFinalValue.CompleteFinalValueAsStrUnit;

                    // Set total purchase value
                    lblDetailsFinalValueTotalSalePurchaseValue.Text =
                        ShareObjectFinalValue.BuyValueBrokerageReductionAsStrUnit;

                    // Set profit or lose of the share
                    lblDetailsFinalValueTotalProfitLossValue.Text =
                        ShareObjectFinalValue.CompleteProfitLossValueAsStrUnit;

                    // Set performance of the share
                    lblDetailsFinalValueTotalPerformanceValue.Text =
                        ShareObjectFinalValue.CompletePerformanceValueAsStrUnit;

                    // Format performance value
                    if (ShareObjectFinalValue.PerformanceValue >= 0)
                    {
                        lblDetailsFinalValueTotalPerformanceValue.ForeColor = Color.Green;
                        lblDetailsFinalValueTotalProfitLossValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsFinalValueTotalPerformanceValue.ForeColor = Color.Red;
                        lblDetailsFinalValueTotalProfitLossValue.ForeColor = Color.Red;
                    }

                    #endregion Overall calculation

                    #region Current calculation

                    // Set share volume
                    lblDetailsFinalValueCurrentVolumeValue.Text =
                        ShareObjectFinalValue.VolumeAsStrUnit;

                    // Set the current price value
                    lblDetailsFinalValueCurrentCurPriceValue.Text =
                        ShareObjectFinalValue.CurPriceAsStrUnit;

                    // Set current value of the volume of the shares
                    lblDetailsFinalValueCurrentPurchaseValue.Text =
                        ShareObjectFinalValue.FinalValueAsStrUnit;

                    // Set dividend value
                    lblDetailsFinalValueCurrentDividendValue.Text =
                        ShareObjectFinalValue.CompleteDividendValueAsStrUnit;

                    // Set total share value
                    lblDetailsFinalValueCurrentProfitLossSaleValue.Text =
                        ShareObjectFinalValue.SaleProfitLossBrokerageReductionAsStrUnit;

                    // Set total value of the share
                    lblDetailsFinalValueCurrentSumValue.Text =
                        ShareObjectFinalValue.FinalValueWithProfitLossAsStrUnit;

                    #endregion Current calculation

                    #region Previous day calculation

                    // Set the current price value
                    lblDetailsFinalValuePrevDayCurPriceValue.Text =
                        ShareObjectFinalValue.CurPriceAsStrUnit;

                    // Set previous price value
                    lblDetailsFinalValuePrevDayPrevPriceValue.Text =
                        ShareObjectFinalValue.PrevPriceAsStrUnit;

                    // Set difference price value
                    lblDetailsFinalValuePrevDayDiffPriceValue.Text =
                        ShareObjectFinalValue.CurPrevDayPriceDifferenceAsStrUnit;

                    // Set performance previous value
                    lblDetailsFinalValuePrevDayDiffPerformanceValue.Text =
                        ShareObjectFinalValue.CurPrevDayPricePerformanceAsStrUnit;

                    // Set share volume
                    lblDetailsFinalValuePrevDayVolumeValue.Text =
                        ShareObjectFinalValue.VolumeAsStrUnit;

                    // Set difference price value
                    lblDetailsFinalValuePrevDayDiffPriceValueValue.Text =
                        ShareObjectFinalValue.CurPrevDayPriceDifferenceAsStrUnit;

                    // Set difference previous value
                    lblDetailsFinalValueDiffSumPrevValue.Text =
                        ShareObjectFinalValue.CurPrevDayProfitLossAsStrUnit;

                    lblDetailsFinalValuePrevDayDiffPriceValue.ForeColor = ShareObjectFinalValue.CurPrevDayPricePerformance >= 0 ? Color.Green : Color.Red;
                    lblDetailsFinalValuePrevDayDiffPerformanceValue.ForeColor = ShareObjectFinalValue.CurPrevDayPricePerformance >= 0 ? Color.Green : Color.Red;
                    lblDetailsFinalValueDiffSumPrevValue.ForeColor = ShareObjectFinalValue.CurPrevDayProfitLoss >= 0 ? Color.Green : Color.Red;

                    #endregion Previous day calculation
                }
            }
            catch (Exception ex)
            {
                // Get correct status strip label of the selected tab page
                var statusStrip = bShowMarketValue ? toolStripStatusLabelMarketValues : toolStripStatusLabelCompleteValues;

                Helper.AddStatusMessage(statusStrip,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithCompleteMarketDepotValuesErrors/ShowFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        private void ResetShareDetails()
        {
            // Group box caption
            grpBoxShareDetails.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/Caption",
                LanguageName);

            // Tab captions
            if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsFinalValueName))
                tabCtrlShareDetails.TabPages[TabPageDetailsFinalValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Caption",
                        LanguageName);

            if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsMarketValueName))
                tabCtrlShareDetails.TabPages[TabPageDetailsMarketValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Caption",
                        LanguageName);

            if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsProfitLossValueName))
                tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", LanguageName);

            if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsDividendValueName))
                tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", LanguageName);

            if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsBrokerageValueName))
                tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Caption", LanguageName);

            // Label values of the market value
            lblDetailsMarketValueDateValue.Text = @"";

            #region Overall calculation

            lblDetailsMarketValueTotalVolumeValue.Text = @"";
            lblDetailsMarketValueTotalCurPriceValue.Text = @"";
            lblDetailsMarketValueTotalPurchaseValue.Text = @"";
            lblDetailsMarketValueTotalDividendValue.Text = @"";
            lblDetailsMarketValueTotalSaleValue.Text = @"";
            lblDetailsMarketValueTotalSumValue.Text = @"";
            lblDetailsMarketValueTotalSalePurchaseValue.Text = @"";
            lblDetailsMarketValueTotalProfitLossValue.Text = @"";
            lblDetailsMarketValueTotalPerformanceValue.Text = @"";

            #endregion Overall calculation

            #region Current calculation

            lblDetailsMarketValueCurrentVolumeValue.Text = @"";
            lblDetailsMarketValueCurrentCurPriceValue.Text = @"";
            lblDetailsMarketValueCurrentPurchaseValue.Text = @"";
            lblDetailsMarketValueCurrentDividendValue.Text = @"";
            lblDetailsMarketValueCurrentProfitLossSaleValue.Text = @"";
            lblDetailsMarketValueCurrentSumValue.Text = @"";

            #endregion Curren calculation

            #region Daily calculation

            lblDetailsMarketValuePrevDayCurPriceValue.Text = @"";
            lblDetailsMarketValuePrevDayPrevPriceValue.Text = @"";
            lblDetailsMarketValuePrevDayDiffPriceValue.Text = @"";
            lblDetailsMarketValuePrevDayDiffPerformanceValue.Text = @"";
            lblDetailsMarketValuePrevDayVolumeValue.Text = @"";
            lblDetailsMarketValuePrevDayDiffPriceValueValue.Text = @"";
            lblDetailsMarketValueDiffSumPrevValue.Text = @"";

            #endregion Daily calculation

            // Label values of the final value
            lblDetailsFinalValueDateValue.Text = @"";

            #region Overall calculation

            lblDetailsFinalValueTotalVolumeValue.Text = @"";
            lblDetailsFinalValueTotalCurPriceValue.Text = @"";
            lblDetailsFinalValueTotalPurchaseValue.Text = @"";
            lblDetailsFinalValueTotalDividendValue.Text = @"";
            lblDetailsFinalValueTotalSaleValue.Text = @"";
            lblDetailsFinalValueTotalSumValue.Text = @"";
            lblDetailsFinalValueTotalSalePurchaseValue.Text = @"";
            lblDetailsFinalValueTotalProfitLossValue.Text = @"";
            lblDetailsFinalValueTotalPerformanceValue.Text = @"";

            #endregion Overall calculation

            #region Current calculation

            lblDetailsFinalValueCurrentVolumeValue.Text = @"";
            lblDetailsFinalValueCurrentCurPriceValue.Text = @"";
            lblDetailsFinalValueCurrentPurchaseValue.Text = @"";
            lblDetailsFinalValueCurrentDividendValue.Text = @"";
            lblDetailsFinalValueCurrentProfitLossSaleValue.Text = @"";
            lblDetailsFinalValueCurrentSumValue.Text = @"";

            #endregion Current calculation

            #region Daily calculation

            lblDetailsFinalValuePrevDayCurPriceValue.Text = @"";
            lblDetailsFinalValuePrevDayPrevPriceValue.Text = @"";
            lblDetailsFinalValuePrevDayDiffPriceValue.Text = @"";
            lblDetailsFinalValuePrevDayDiffPerformanceValue.Text = @"";
            lblDetailsFinalValuePrevDayVolumeValue.Text = @"";
            lblDetailsFinalValuePrevDayDiffPriceValueValue.Text = @"";
            lblDetailsFinalValueDiffSumPrevValue.Text = @"";

            #endregion Daily calculation

        }

        #endregion Share details

        #endregion Tab control details 
    }
}
