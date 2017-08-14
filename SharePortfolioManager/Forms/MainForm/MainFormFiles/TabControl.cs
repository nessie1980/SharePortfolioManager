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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    partial class FrmMain
    {
        #region Variables

        private readonly string TabPageDetailsFinalValue = "tabPgDetailsFinalValue";
        private readonly string TabPageDetailsMarketValue = "tabPgDetailsMarketValue";
        private readonly string TabPageDetailsDividendValue = "tabPgDividends";
        private readonly string TabPageDetailsCostsValue = "tabPgCosts";
        private readonly string TabPageDetailsProfitLossValue = "tabPgProfitLoss";

        #endregion Variables

        #region Tab control overview

        /// <summary>
        /// This function calls the details update function
        /// </summary>
        /// <param name="sender">TabControl</param>
        /// <param name="e">EventArgs</param>
        private void tabCtrlShareOverviews_Enter(object sender, EventArgs e)
        {
            UpdateDetails((TabControl)sender);
        }

        /// <summary>
        /// This function calls the details update function
        /// </summary>
        /// <param name="sender">TabControl</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlShareOverviews_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDetails((TabControl)sender);
        }

        /// <summary>
        /// This function updates the flag if the final value or the market value should be shown
        /// </summary>
        /// <param name="tabControl">TabControl</param>
        private void UpdateDetails(TabControl tabControl)
        {
            if (tabControl.SelectedIndex == 0)
                MarketValueOverviewTabSelected = false;

            if (tabControl.SelectedIndex == 1)
                MarketValueOverviewTabSelected = true;

            ResetShareDetails();

            UpdateShareDetails(MarketValueOverviewTabSelected);
            UpdateProfitLossDetails();
            UpdateDividendDetails(MarketValueOverviewTabSelected);
            UpdateCostsDetails(MarketValueOverviewTabSelected);

            tabCtrlDetails.SelectedIndex = 0;
        }

        #endregion Tab control overview

        #region Tab control details

        TabPage tempFinalValues = null;
        TabPage tempDividends = null;
        TabPage tempCosts = null;

        /// <summary>
        /// This function updates the dividend, costs and profit / loss values in the tab controls
        /// if one of them three pages are chosen.
        /// </summary>
        /// <param name="sender">TabControl</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the profit or loss page
            if (((TabControl)sender).SelectedIndex == 2)
                UpdateProfitLossDetails();

            // Update the dividend page
            if (((TabControl)sender).SelectedIndex == 3)
                UpdateDividendDetails(MarketValueOverviewTabSelected);

            // Update the costs page
            if (((TabControl)sender).SelectedIndex == 4)
                UpdateCostsDetails(MarketValueOverviewTabSelected);
        }

        #region Reset group box details

        void ResetShareDetails()
        {
            // Group box caption
            grpBoxShareDetails.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/Caption",
                LanguageName);

            // Tab captions
            if (tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsFinalValue))
                tabCtrlDetails.TabPages[TabPageDetailsFinalValue].Text =
                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Caption",
                    LanguageName);

            tabCtrlDetails.TabPages[TabPageDetailsMarketValue].Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Caption",
                        LanguageName);

            if (tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsDividendValue))
                tabCtrlDetails.TabPages[TabPageDetailsDividendValue].Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", LanguageName);

            if (tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsCostsValue))
                tabCtrlDetails.TabPages[TabPageDetailsCostsValue].Text =
                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Caption", LanguageName);

            tabCtrlDetails.TabPages[TabPageDetailsProfitLossValue].Text =
                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", LanguageName);

            // Label values of the market value
            lblDetailsMarketValueDateValue.Text = @"";
            lblDetailsMarketValueVolumeValue.Text = @"";
            lblDetailsMarketValueDividendValue.Text = @"";
            lblDetailsMarketValueCostValue.Text = @"";
            lblDetailsMarketValueCurPriceValue.Text = @"";
            lblDetailsMarketValueDiffPerformancePrevValue.Text = @"";
            lblDetailsMarketValueDiffSumPrevValue.Text = @"";
            lblDetailsMarketValuePrevPriceValue.Text = @"";
            lblDetailsMarketValuePurchaseValue.Text = @"";
            lblDetailsMarketValueTotalPerformanceValue.Text = @"";
            lblDetailsMarketValueTotalProfitValue.Text = @"";
            lblDetailsMarketValueTotalSumValue.Text = @"";

            // Label values of the final value
            lblDetailsFinalValueDateValue.Text = @"";
            lblDetailsFinalValueVolumeValue.Text = @"";
            lblDetailsFinalValueDividendValue.Text = @"";
            lblDetailsFinalValueCostsValue.Text = @"";
            lblDetailsFinalValueCurPriceValue.Text = @"";
            lblDetailsFinalValueDiffPerformancePrevValue.Text = @"";
            lblDetailsFinalValueDiffSumPrevValue.Text = @"";
            lblDetailsFinalValuePrevPriceValue.Text = @"";
            lblDetailsFinalValuePurchaseValue.Text = @"";
            lblDetailsFinalValueTotalPerformanceValue.Text = @"";
            lblDetailsFinalValueTotalProfitValue.Text = @"";
            lblDetailsFinalValueTotalSumValue.Text = @"";
        }

        #endregion Reset group box details

        #region Share details with or without dividends and costs

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
                    // Remove page
                    if (tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsFinalValue))
                    {
                        // Save page for later adding when the final value overview is selected again
                        tempFinalValues = tabCtrlDetails.TabPages[TabPageDetailsFinalValue];
                        tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[TabPageDetailsFinalValue]);
                    }

                    // Check if a share is selected
                    if (ShareObjectMarketValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    // Set GroupBox caption
                    // Check if an update has already done
                    if (ShareObjectMarketValue.LastUpdateInternet == DateTime.MinValue)
                    {
                        grpBoxShareDetails.Text = ShareObjectMarketValue.Name + @" ( " +
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareWKN",
                                                      LanguageName) +
                                                  @": " + ShareObjectMarketValue.Wkn + @" / " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                      LanguageName) + @" " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName) +
                                                  @" )";
                    }
                    else
                    {
                        grpBoxShareDetails.Text = ShareObjectMarketValue.Name + @" ( " +
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareWKN",
                                                      LanguageName) +
                                                  @": " + ShareObjectMarketValue.Wkn + @" / " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                      LanguageName) + @" " +
                                                  string.Format(Helper.Datefulltimeshortformat, ShareObjectMarketValue.LastUpdateInternet)
                                                       + " )";
                    }

                    if (ShareObjectMarketValue.LastUpdateDate == DateTime.MinValue &&
                        ShareObjectMarketValue.LastUpdateTime == DateTime.MinValue)
                    {
                        // Set the share update date
                        lblDetailsMarketValueDateValue.Text =
                            Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName);
                    }
                    else
                    {
                        // Set the share update date
                        lblDetailsMarketValueDateValue.Text =
                            string.Format(Helper.Datefullformat, ShareObjectMarketValue.LastUpdateDate) + " " +
                            string.Format(Helper.Timeshortformat, ShareObjectMarketValue.LastUpdateTime);
                        lblDetailsMarketValueDateValue.Text =
                            string.Format(Helper.Datefullformat, ShareObjectMarketValue.LastUpdateDate) + " " +
                            string.Format(Helper.Timeshortformat, ShareObjectMarketValue.LastUpdateTime);
                    }
                    // Set share volume
                    lblDetailsMarketValueVolumeValue.Text = ShareObjectMarketValue.VolumeAsStrUnit;

                    // Set dividend value
                    lblDetailsMarketValueDividendValue.Text = @"-";
                    // Disable the dividend labels 
                    lblDetailsMarketValueDividend.Enabled = false;
                    lblDetailsMarketValueDividendValue.Enabled = false;

                    // Set cost value
                    lblDetailsMarketValueCostValue.Text = @"-";
                    // Disable the cost labels 
                    lblDetailsMarketValueCost.Enabled = false;
                    lblDetailsMarketValueCostValue.Enabled = false;

                    // Set the current price value
                    lblDetailsMarketValueCurPriceValue.Text =
                        ShareObjectMarketValue.CurPriceAsStrUnit;
                    // Set performance previous value
                    lblDetailsMarketValueDiffPerformancePrevValue.Text =
                        ShareObjectMarketValue.PrevDayPerformanceAsStrUnit;
                    // Set difference previous value
                    lblDetailsMarketValueDiffSumPrevValue.Text =
                        ShareObjectMarketValue.PrevDayProfitLossAsStrUnit;
                    // Set previous price value
                    lblDetailsMarketValuePrevPriceValue.Text =
                        ShareObjectMarketValue.PrevDayPriceAsStrUnit;

                    // Set purchase value
                    lblDetailsMarketValuePurchaseValue.Text =
                        ShareObjectMarketValue.PurchaseValueAsStrUnit;
                    // Set performance of the share
                    lblDetailsMarketValueTotalPerformanceValue.Text =
                        ShareObjectMarketValue.PerformanceValueAsStrUnit;
                    // Set profit or lose of the share
                    lblDetailsMarketValueTotalProfitValue.Text =
                        ShareObjectMarketValue.ProfitLossValueAsStrUnit;
                    // Set total value of the share
                    lblDetailsMarketValueTotalSumValue.Text =
                        ShareObjectMarketValue.MarketValueAsStrUnit;

                    lblDetailsMarketValueDiffPerformancePrevValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsMarketValueDiffPerformancePrevValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsMarketValueDiffSumPrevValue.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                    lblDetailsMarketValueDiffSumPrevValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsMarketValueTotalPerformanceValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsMarketValueTotalPerformanceValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsMarketValueTotalProfitValue.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                    lblDetailsMarketValueTotalProfitValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);

                    // Format performance value
                    if (ShareObjectMarketValue.PerformanceValue >= 0)
                    {
                        lblDetailsMarketValueTotalPerformanceValue.ForeColor = Color.Green;
                        lblDetailsMarketValueTotalProfitValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsMarketValueTotalPerformanceValue.ForeColor = Color.Red;
                        lblDetailsMarketValueTotalProfitValue.ForeColor = Color.Red;
                    }

                    if (ShareObjectMarketValue.PrevDayPerformance >= 0)
                    {
                        lblDetailsMarketValueDiffPerformancePrevValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsMarketValueDiffPerformancePrevValue.ForeColor = Color.Red;
                    }

                    if (ShareObjectMarketValue.PrevDayProfitLoss >= 0)
                    {
                        lblDetailsMarketValueDiffSumPrevValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsMarketValueDiffSumPrevValue.ForeColor = Color.Red;
                    }

                    int iLastUpdateDay =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateDate.ToString(), ShareObjectMarketValue.CultureInfo).Day;
                    int iLastUpdateMonth =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateDate.ToString(), ShareObjectMarketValue.CultureInfo).Month;
                    int iLastUpdateYear =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateDate.ToString(), ShareObjectMarketValue.CultureInfo).Year;
                    int iLastUpdateHour =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateTime.ToString(), ShareObjectMarketValue.CultureInfo).Hour;
                    int iLastUpdateMinute =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateTime.ToString(), ShareObjectMarketValue.CultureInfo).Minute;
                    int iLastUpdateSecond =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateTime.ToString(), ShareObjectMarketValue.CultureInfo).Second;
                }
                else
                {
                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsFinalValue))
                        tabCtrlDetails.TabPages.Insert(0, tempFinalValues);

                    // Check if a share is selected
                    if (ShareObjectFinalValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    // Set GroupBox caption
                    // Check if an update has already done
                    if (ShareObjectFinalValue.LastUpdateInternet == DateTime.MinValue)
                    {
                        grpBoxShareDetails.Text = ShareObjectFinalValue.Name + @" ( " +
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareWKN",
                                                      LanguageName) +
                                                  @": " + ShareObjectFinalValue.Wkn + @" / " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                      LanguageName) + @" " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName) +
                                                  @" )";
                    }
                    else
                    {
                        grpBoxShareDetails.Text = ShareObjectFinalValue.Name + @" ( " +
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareWKN",
                                                      LanguageName) +
                                                  @": " + ShareObjectFinalValue.Wkn + @" / " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                      LanguageName) + @" " +
                                                  string.Format(Helper.Datefulltimeshortformat, ShareObjectFinalValue.LastUpdateInternet)
                                                       + " )";
                    }

                    if (ShareObjectFinalValue.LastUpdateDate == DateTime.MinValue &&
                        ShareObjectFinalValue.LastUpdateTime == DateTime.MinValue)
                    {
                        // Set the share update date
                        lblDetailsFinalValueDateValue.Text =
                            Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareUpdateNotDone", LanguageName);
                    }
                    else
                    {
                        // Set the share update date
                        lblDetailsFinalValueDateValue.Text =
                            string.Format(Helper.Datefullformat, ShareObjectFinalValue.LastUpdateDate) + " " +
                            string.Format(Helper.Timeshortformat, ShareObjectFinalValue.LastUpdateTime);
                        lblDetailsMarketValueDateValue.Text =
                            string.Format(Helper.Datefullformat, ShareObjectFinalValue.LastUpdateDate) + " " +
                            string.Format(Helper.Timeshortformat, ShareObjectFinalValue.LastUpdateTime);
                    }

                    // Set share volume
                    lblDetailsFinalValueVolumeValue.Text = ShareObjectFinalValue.VolumeAsStrUnit;

                    // Set dividend value
                    lblDetailsFinalValueDividendValue.Text =
                        ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsString;
                    // Disable the dividend labels 
                    lblDetailsMarketValueDividend.Enabled = false;
                    lblDetailsMarketValueDividendValue.Enabled = false;
                    
                    // Set cost value
                    lblDetailsFinalValueCostsValue.Text =
                        ShareObjectFinalValue.AllCostsEntries.CostValueTotalWithUnitAsString;
                    // Disable the cost labels 
                    lblDetailsMarketValueCost.Enabled = false;
                    lblDetailsMarketValueCostValue.Enabled = false;

                    // Set the current price value
                    lblDetailsFinalValueCurPriceValue.Text =
                        ShareObjectFinalValue.CurPriceAsStrUnit;
                    // Set performance previous value
                    lblDetailsFinalValueDiffPerformancePrevValue.Text =
                        ShareObjectFinalValue.PrevDayPerformanceAsStrUnit;
                    // Set difference previous value
                    lblDetailsFinalValueDiffSumPrevValue.Text =
                        ShareObjectFinalValue.PrevDayProfitLossAsStrUnit;
                    // Set previous price value
                    lblDetailsFinalValuePrevPriceValue.Text =
                        ShareObjectFinalValue.PrevDayPriceAsStrUnit;

                    // Set purchase value
                    lblDetailsFinalValuePurchaseValue.Text =
                        ShareObjectFinalValue.PurchaseValueAsStrUnit;
                    // Set performance of the share
                    lblDetailsFinalValueTotalPerformanceValue.Text =
                        ShareObjectFinalValue.PerformanceValueAsStrUnit;
                    // Set profit or lose of the share
                    lblDetailsFinalValueTotalProfitValue.Text =
                        ShareObjectFinalValue.ProfitLossValueAsStrUnit;
                    // Set total value of the share
                    lblDetailsFinalValueTotalSumValue.Text =
                        ShareObjectFinalValue.FinalValueAsStrUnit;

                    lblDetailsFinalValueDiffPerformancePrevValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsMarketValueDiffPerformancePrevValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsFinalValueDiffSumPrevValue.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                    lblDetailsMarketValueDiffSumPrevValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsFinalValueTotalPerformanceValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsMarketValueTotalPerformanceValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);
                    lblDetailsFinalValueTotalProfitValue.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                    lblDetailsMarketValueTotalProfitValue.Font = new Font("Trebuchet MS", 10,
                        FontStyle.Bold);

                    // Format performance value
                    if (ShareObjectFinalValue.PerformanceValue >= 0)
                    {
                        lblDetailsFinalValueTotalPerformanceValue.ForeColor = Color.Green;
                        lblDetailsFinalValueTotalProfitValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsFinalValueTotalPerformanceValue.ForeColor = Color.Red;
                        lblDetailsFinalValueTotalProfitValue.ForeColor = Color.Red;
                    }

                    if (ShareObjectFinalValue.PrevDayPerformance >= 0)
                    {
                        lblDetailsFinalValueDiffPerformancePrevValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsFinalValueDiffPerformancePrevValue.ForeColor = Color.Red;
                    }

                    if (ShareObjectFinalValue.PrevDayProfitLoss >= 0)
                    {
                        lblDetailsFinalValueDiffSumPrevValue.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblDetailsFinalValueDiffSumPrevValue.ForeColor = Color.Red;
                    }

                    int iLastUpdateDay =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateDate.ToString(), ShareObjectFinalValue.CultureInfo).Day;
                    int iLastUpdateMonth =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateDate.ToString(), ShareObjectFinalValue.CultureInfo).Month;
                    int iLastUpdateYear =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateDate.ToString(), ShareObjectFinalValue.CultureInfo).Year;
                    int iLastUpdateHour =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateTime.ToString(), ShareObjectFinalValue.CultureInfo).Hour;
                    int iLastUpdateMinute =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateTime.ToString(), ShareObjectFinalValue.CultureInfo).Minute;
                    int iLastUpdateSecond =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateTime.ToString(), ShareObjectFinalValue.CultureInfo).Second;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateShareDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithWithOutDividendCostsErrors/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Share details with or without dividends and costs

        #region Profit loss details

        /// <summary>
        /// This function updates the profit or loss information of a share
        /// </summary>
        private void UpdateProfitLossDetails()
        {
            try
            {
                // TODO Final value or market value
                if (ShareObjectFinalValue != null)
                {
                    Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                    // TODO Replace format
                    tabCtrlDetails.TabPages[TabPageDetailsProfitLossValue].Text = string.Format("{0} ({1:C2})", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", LanguageName), ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotal);

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                    if (tabCtrlDetails.SelectedIndex == 2)
                    {
                        Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                        // Reset tab control
                        foreach (TabPage tabPage in tabCtrlProfitLoss.TabPages)
                        {
                            foreach (var control in tabPage.Controls)
                            {
                                if (control is DataGridView view)
                                {
                                    view.SelectionChanged -= DataGridViewProfitLossOfYears_SelectionChanged;
                                    view.SelectionChanged -= DataGridViewProfitLossOfAYear_SelectionChanged;
                                    view.DataBindingComplete -= DataGridViewProfitLossOfAYear_DataBindingComplete;
                                }
                            }
                            tabPage.Controls.Clear();
                            tabCtrlProfitLoss.TabPages.Remove(tabPage);
                        }

                        tabCtrlProfitLoss.TabPages.Clear();

                        // Create TabPage for the profit or loss of the years
                        TabPage newTabPageOverviewYears = new TabPage();
                        // Set TabPage name
                        newTabPageOverviewYears.Name =
                            Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                                LanguageName);
                        newTabPageOverviewYears.Text =
                            Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                                LanguageName) +
                            string.Format(" ({0:C2})", ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotal);

                        // Create Binding source for the profit or loss data
                        BindingSource bindingSourceOverview = new BindingSource();
                        if (ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Count > 0)
                            bindingSourceOverview.DataSource =
                                ShareObjectFinalValue.AllSaleEntries.GetAllProfitLossTotalValues();

                        // Create DataGridView
                        DataGridView dataGridViewProfitLossOverviewOfAYears = new DataGridView();
                        dataGridViewProfitLossOverviewOfAYears.Dock = DockStyle.Fill;
                        // Bind source with profit or loss values to the DataGridView
                        dataGridViewProfitLossOverviewOfAYears.DataSource = bindingSourceOverview;

                        // Set the delegate for the DataBindingComplete event
                        dataGridViewProfitLossOverviewOfAYears.DataBindingComplete +=
                            DataGridViewProfitLossOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewProfitLossOverviewOfAYears.SelectionChanged +=
                            DataGridViewProfitLossOfYears_SelectionChanged;

                        // Advanced configuration DataGridView profit or loss
                        DataGridViewCellStyle styleOverviewOfYears =
                            dataGridViewProfitLossOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                        styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                        dataGridViewProfitLossOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        dataGridViewProfitLossOverviewOfAYears.ColumnHeadersBorderStyle =
                            DataGridViewHeaderBorderStyle.Single;
                        dataGridViewProfitLossOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                        dataGridViewProfitLossOverviewOfAYears.RowHeadersVisible = false;
                        dataGridViewProfitLossOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                        dataGridViewProfitLossOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                        dataGridViewProfitLossOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                        dataGridViewProfitLossOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        dataGridViewProfitLossOverviewOfAYears.MultiSelect = false;

                        dataGridViewProfitLossOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewProfitLossOverviewOfAYears.AllowUserToResizeColumns = false;
                        dataGridViewProfitLossOverviewOfAYears.AllowUserToResizeRows = false;

                        dataGridViewProfitLossOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        newTabPageOverviewYears.Controls.Add(dataGridViewProfitLossOverviewOfAYears);
                        dataGridViewProfitLossOverviewOfAYears.Parent = newTabPageOverviewYears;
                        tabCtrlProfitLoss.Controls.Add(newTabPageOverviewYears);
                        newTabPageOverviewYears.Parent = tabCtrlProfitLoss;

                        // Check if profit or loss exists
                        if (ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Count > 0)
                        {
                            // Loop through the years of the profit or loss
                            foreach (
                                var keyName in
                                    ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Keys.Reverse()
                                )
                            {
                                // Create TabPage
                                TabPage newTabPage = new TabPage();
                                // Set TabPage name
                                newTabPage.Name = keyName;
                                newTabPage.Text = keyName +
                                    // TODO Replace format
                                                  string.Format(" ({0:C2})",
                                                      ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[
                                                          keyName
                                                          ]
                                                          .SaleProfitLossYear);

                                // Create Binding source for the dividend data
                                BindingSource bindingSource = new BindingSource();
                                bindingSource.DataSource =
                                    ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                        .ProfitLossListYear;

                                // Create DataGridView
                                DataGridView dataGridViewProfitLossOfAYear = new DataGridView();
                                dataGridViewProfitLossOfAYear.Dock = DockStyle.Fill;
                                // Bind source with profit or loss values to the DataGridView
                                dataGridViewProfitLossOfAYear.DataSource = bindingSource;
                                // Set the delegate for the DataBindingComplete event
                                dataGridViewProfitLossOfAYear.DataBindingComplete +=
                                    DataGridViewProfitLossOfAYear_DataBindingComplete;

                                // Set row select event
                                dataGridViewProfitLossOfAYear.SelectionChanged +=
                                    DataGridViewProfitLossOfAYear_SelectionChanged;
                                // Set cell decimal click event
                                dataGridViewProfitLossOfAYear.CellContentDoubleClick +=
                                    DataGridViewProfitLossOfAYear_CellContentdecimalClick;

                                // Advanced configuration DataGridView profit or loss
                                DataGridViewCellStyle style = dataGridViewProfitLossOfAYear.ColumnHeadersDefaultCellStyle;
                                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                                dataGridViewProfitLossOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                                dataGridViewProfitLossOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                                dataGridViewProfitLossOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                                dataGridViewProfitLossOfAYear.RowHeadersVisible = false;
                                dataGridViewProfitLossOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                                dataGridViewProfitLossOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                                dataGridViewProfitLossOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                                dataGridViewProfitLossOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                                dataGridViewProfitLossOfAYear.MultiSelect = false;

                                dataGridViewProfitLossOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                                dataGridViewProfitLossOfAYear.AllowUserToResizeColumns = false;
                                dataGridViewProfitLossOfAYear.AllowUserToResizeRows = false;

                                dataGridViewProfitLossOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                                newTabPage.Controls.Add(dataGridViewProfitLossOfAYear);
                                dataGridViewProfitLossOfAYear.Parent = newTabPage;
                                tabCtrlProfitLoss.Controls.Add(newTabPage);
                                newTabPage.Parent = tabCtrlProfitLoss;
                            }
                        }

                        Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateProfitLossDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss_Error/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewProfitLossOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlProfitLoss.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlProfitLoss.SelectedTab.Controls.Contains((DataGridView)sender))
                        DeselectRowsOfDataGridViews((DataGridView)sender);
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                MessageBox.Show("dataGridViewProfitLossOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewProfitLossOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlProfitLoss.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlProfitLoss.SelectTab(tabPage);

                            // Deselect rows
                            DeselectRowsOfDataGridViews(null);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                MessageBox.Show("dataGridViewProfitLossOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the sale document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void DataGridViewProfitLossOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                int iColumnCount = ((DataGridView)sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex == iColumnCount - 1)
                {
                    // Check if a row is selected
                    if (((DataGridView)sender).SelectedRows.Count == 1)
                    {
                        // Get the current selected row
                        DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;
                        // Get date and time of the selected buy item
                        string strDateTime = curItem[0].Cells[0].Value.ToString();

                        // Check if a document is set
                        if (curItem[0].Cells[iColumnCount - 1].Value.ToString() != @"-")
                        {
                            // Get doc from the profit or loss with the strDateTime
                            foreach (var temp in ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
                            {
                                // Check if the buy date and time is the same as the date and time of the clicked buy item
                                if (temp.Date == strDateTime)
                                {
                                    // Check if the file still exists
                                    if (File.Exists(temp.Document))
                                        // Open the file
                                        Process.Start(temp.Document);
                                    else
                                    {
                                        string strCaption =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                                LanguageName);
                                        string strMessage =
                                            Language.GetLanguageTextByXPath(
                                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                                LanguageName);
                                        string strOk =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                                LanguageName);
                                        string strCancel =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                                LanguageName);

                                        OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                                            strCancel);
                                        if (messageBox.ShowDialog() == DialogResult.OK)
                                        {
                                            // Remove sale object and add it with no document
                                            // TODO
                                            //if (ShareObjectFinalValue.RemoveSale(temp.Date) &&
                                            //    ShareObjectFinalValue.AddSale(false, strDateTime, temp.SaleVolume, temp.SaleValue, temp.SaleProfitLoss, 0))
                                            //{
                                            //    // TODO Refresh profit or loss
                                            //    //Show();

                                            //    // Add status message
                                            //    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            //        Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/EditSuccess", LanguageName),
                                            //        Language, LanguageName,
                                            //        Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                                            //    // Save the share values to the XML
                                            //    if (ShareObjectFinalValue.SaveShareObject(ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedRows[0].Index], ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, PortfolioFileName, out Exception exception))
                                            //    {
                                            //        // Add status message
                                            //        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            //            Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/EditSaveSuccessful", LanguageName),
                                            //            Language, LanguageName,
                                            //            Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                                            //        // Reset / refresh DataGridView portfolio binding source
                                            //        _dgvPortfolioBindingSourceFinalValue.ResetBindings(false);
                                            //    }
                                            //    else
                                            //    {
                                            //        // Add status message
                                            //        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            //            Language.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveFailed", LanguageName),
                                            //            Language, LanguageName,
                                            //            Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                            //        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/EditFailed", LanguageName),
                                            //        Language, LanguageName,
                                            //        Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                                            //}
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewProfitLossOfAYear_CellContentdecimalClick()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the data binding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DataGridViewProfitLossOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (int i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            if (tabCtrlProfitLoss.TabPages.Count == 1)
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Year",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Date",
                                        LanguageName);
                            }
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_ProfitLoss",
                                    LanguageName) +
                                string.Format(@" ({0})",
                                    new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Document",
                                    LanguageName)
                            ;
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                {
                    ((DataGridView)sender).Rows[0].Selected = false;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewDividensOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/RenameHeaderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all rows when the page index of the tab control has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCtrlProfitLoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO
            try
            {
                // Deselect all rows
                DeselectRowsOfDataGridViews(null);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("tabCtrlProfitLoss_SelectedIndexChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Profit or loss details

        #region Dividend details

        /// <summary>
        /// This function updates the dividend information of a share
        /// </summary>
        private void UpdateDividendDetails(bool bShowMarketValue)
        {
            try
            {
                if (bShowMarketValue)
                {
                    // Remove page
                    if (tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsDividendValue))
                    {
                        // Save page for later adding when the final value overview is selected again
                        tempDividends = tabCtrlDetails.TabPages[TabPageDetailsDividendValue];
                        tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[TabPageDetailsDividendValue]);
                    }
                }
                else
                {
                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsDividendValue))
                        tabCtrlDetails.TabPages.Insert(3, tempDividends);

                    // Check if a share is selected
                    if (ShareObjectFinalValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                    tabCtrlDetails.TabPages[TabPageDetailsDividendValue].Text = string.Format("{0} ({1})", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", LanguageName), ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsString);

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                    if (tabCtrlDetails.SelectedIndex == 3)
                    {
                        Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                        // Reset tab control
                        foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                        {
                            foreach (var control in tabPage.Controls)
                            {
                                if (control is DataGridView view)
                                {
                                    view.SelectionChanged -= DataGridViewDividendsOfYears_SelectionChanged;
                                    view.SelectionChanged -= DataGridViewDividendsOfAYear_SelectionChanged;
                                    view.DataBindingComplete -= DataGridViewDividensOfAYear_DataBindingComplete;
                                }
                            }
                            tabPage.Controls.Clear();
                            tabCtrlDividends.TabPages.Remove(tabPage);
                        }

                        tabCtrlDividends.TabPages.Clear();

                        // Create TabPage for the dividends of the years
                        TabPage newTabPageOverviewYears = new TabPage();
                        // Set TabPage name
                        newTabPageOverviewYears.Name =
                            Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Overview",
                                LanguageName);
                        newTabPageOverviewYears.Text =
                            Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Overview",
                                LanguageName) +
                            string.Format(" ({0})", ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsString);

                        // Create Binding source for the dividend data
                        BindingSource bindingSourceOverview = new BindingSource();
                        if (ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues().Count > 0)
                            bindingSourceOverview.DataSource =
                                ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues();

                        // Create DataGridView
                        DataGridView dataGridViewDividendsOverviewOfAYears = new DataGridView();
                        dataGridViewDividendsOverviewOfAYears.Dock = DockStyle.Fill;
                        // Bind source with dividend values to the DataGridView
                        dataGridViewDividendsOverviewOfAYears.DataSource = bindingSourceOverview;

                        // Set the delegate for the DataBindingComplete event
                        dataGridViewDividendsOverviewOfAYears.DataBindingComplete +=
                            DataGridViewDividensOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewDividendsOverviewOfAYears.SelectionChanged +=
                            DataGridViewDividendsOfYears_SelectionChanged;

                        // Advanced configuration DataGridView dividends
                        DataGridViewCellStyle styleOverviewOfYears =
                            dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                        styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                        dataGridViewDividendsOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        dataGridViewDividendsOverviewOfAYears.ColumnHeadersBorderStyle =
                            DataGridViewHeaderBorderStyle.Single;
                        dataGridViewDividendsOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


                        dataGridViewDividendsOverviewOfAYears.RowHeadersVisible = false;
                        dataGridViewDividendsOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                        dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                        dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                        dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        dataGridViewDividendsOverviewOfAYears.MultiSelect = false;

                        dataGridViewDividendsOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewDividendsOverviewOfAYears.AllowUserToResizeColumns = false;
                        dataGridViewDividendsOverviewOfAYears.AllowUserToResizeRows = false;


                        dataGridViewDividendsOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        newTabPageOverviewYears.Controls.Add(dataGridViewDividendsOverviewOfAYears);
                        dataGridViewDividendsOverviewOfAYears.Parent = newTabPageOverviewYears;
                        tabCtrlDividends.Controls.Add(newTabPageOverviewYears);
                        newTabPageOverviewYears.Parent = tabCtrlDividends;

                        // Check if any dividend exists
                        if (ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Count > 0)
                        {
                            // Loop through the years of the dividend pays
                            foreach (
                                var keyName in
                                    ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Keys.Reverse()
                                )
                            {
                                // Create TabPage
                                TabPage newTabPage = new TabPage();
                                // Set TabPage name
                                newTabPage.Name = keyName;
                                newTabPage.Text = keyName +
                                                  string.Format(" ({0})",
                                                      ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[
                                                          keyName
                                                          ]
                                                          .DividendValueYearWithUnitAsString);

                                // Create Binding source for the dividend data
                                BindingSource bindingSource = new BindingSource();
                                bindingSource.DataSource =
                                    ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                                        .DividendListYear;

                                // Create DataGridView
                                DataGridView dataGridViewDividendsOfAYear = new DataGridView();
                                dataGridViewDividendsOfAYear.Dock = DockStyle.Fill;
                                // Bind source with dividend values to the DataGridView
                                dataGridViewDividendsOfAYear.DataSource = bindingSource;
                                // Set the delegate for the DataBindingComplete event
                                dataGridViewDividendsOfAYear.DataBindingComplete +=
                                    DataGridViewDividensOfAYear_DataBindingComplete;

                                // Set row select event
                                dataGridViewDividendsOfAYear.SelectionChanged +=
                                    DataGridViewDividendsOfAYear_SelectionChanged;

                                // Advanced configuration DataGridView dividends
                                DataGridViewCellStyle style = dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle;
                                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                                dataGridViewDividendsOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                                dataGridViewDividendsOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                                dataGridViewDividendsOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                                dataGridViewDividendsOfAYear.RowHeadersVisible = false;
                                dataGridViewDividendsOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                                dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                                dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                                dataGridViewDividendsOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                                dataGridViewDividendsOfAYear.MultiSelect = false;

                                dataGridViewDividendsOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                                dataGridViewDividendsOfAYear.AllowUserToResizeColumns = false;
                                dataGridViewDividendsOfAYear.AllowUserToResizeRows = false;

                                dataGridViewDividendsOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                                newTabPage.Controls.Add(dataGridViewDividendsOfAYear);
                                dataGridViewDividendsOfAYear.Parent = newTabPage;
                                tabCtrlDividends.Controls.Add(newTabPage);
                                newTabPage.Parent = tabCtrlDividends;
                            }
                        }

                        Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateDividendDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewDividendsOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlDividends.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlDividends.SelectedTab.Controls.Contains((DataGridView)sender))
                        DeselectRowsOfDataGridViews((DataGridView)sender);
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                MessageBox.Show("dataGridViewDividendsOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewDividendsOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlDividends.SelectTab(tabPage);

                            // Deselect rows
                            DeselectRowsOfDataGridViews(null);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                MessageBox.Show("dataGridViewDividendsOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DataGridViewDividensOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (int i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            {
                                if (tabCtrlDividends.TabPages.Count == 1)
                                {
                                    ((DataGridView)sender).Columns[i].HeaderText =
                                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Year",
                                            LanguageName);
                                }
                                else
                                {
                                    ((DataGridView)sender).Columns[i].HeaderText =
                                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Date",
                                            LanguageName);
                                }
                                break;
                            }
                        case 1:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Payout",
                                        LanguageName) +
                                    string.Format(@" ({0})",
                                        new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                                break;
                            }
                        case 2:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Dividend",
                                        LanguageName) +
                                    string.Format(@" ({0})",
                                        new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                                break;
                            }
                        case 3:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Yield",
                                        LanguageName) +
                                    string.Format(@" ({0})", ShareObject.PercentageUnit);
                                break;
                            }
                        case 4:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Price",
                                        LanguageName) +
                                    string.Format(@" ({0})",
                                        new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                                break;
                            }
                        case 5:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Volume",
                                        LanguageName) +
                                        ShareObject.PieceUnit;
                                ;
                                break;
                            }
                        case 6:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Document",
                                        LanguageName)
                                ;
                                break;
                            }
                        default:
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                {
                    ((DataGridView)sender).Rows[0].Selected = false;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewDividensOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/RenameHeaderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all rows when the page index of the tab control has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCtrlDividends_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO
            try
            {
                // Deselect all rows
                DeselectRowsOfDataGridViews(null);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("tabCtrlDividends_SelectedIndexChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        void DeselectRowsOfDataGridViews(DataGridView DataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage TabPage in tabCtrlDividends.TabPages)
                {
                    foreach (Control control in TabPage.Controls)
                    {
                        if (control is DataGridView view && view != DataGridView)
                        {
                            foreach (DataGridViewRow selectedRow in view.SelectedRows)
                            {
                                selectedRow.Selected = false;
                            }
                        }
                    }
                }

                // Deselect the row
                foreach (TabPage TabPage in tabCtrlCosts.TabPages)
                {
                    foreach (Control control in TabPage.Controls)
                    {
                        if (control is DataGridView view && view != DataGridView)
                        {
                            foreach (DataGridViewRow selectedRow in view.SelectedRows)
                            {
                                selectedRow.Selected = false;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
#if DEBUG
                MessageBox.Show("DeselectRowsOfDataGridViews()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
            }
        }

        #endregion Dividend details

        #region Costs details

        /// <summary>
        /// This function updates the costs information of a share
        /// </summary>
        private void UpdateCostsDetails(bool bShowMarketValue)
        {
            try
            {
                if (bShowMarketValue)
                {

                    // Remove page
                    if (tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsCostsValue))
                    {
                        // Save page for later adding when the final value overview is selected again
                        tempCosts = tabCtrlDetails.TabPages[TabPageDetailsCostsValue];
                        tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[TabPageDetailsCostsValue]);
                    }
                }
                else
                {
                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(TabPageDetailsCostsValue))
                        tabCtrlDetails.TabPages.Insert(4, tempCosts);

                    // Check if a share is selected
                    if (ShareObjectFinalValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                    tabCtrlDetails.TabPages[TabPageDetailsCostsValue].Text = string.Format("{0} ({1})", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Caption", LanguageName), ShareObjectFinalValue.AllCostsEntries.CostValueTotalWithUnitAsString);

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                    if (tabCtrlDetails.SelectedIndex == 4)
                    {

                        // Reset tab control
                        foreach (TabPage tabPage in tabCtrlCosts.TabPages)
                        {
                            foreach (var control in tabPage.Controls)
                            {
                                if (control is DataGridView view)
                                {
                                    view.SelectionChanged -= DataGridViewCostsOfAYear_SelectionChanged;
                                    view.SelectionChanged -= DataGridViewCostsOfYears_SelectionChanged;
                                    view.DataBindingComplete -= DataGridViewCostsOfAYear_DataBindingComplete;
                                }
                            }
                            tabPage.Controls.Clear();
                            tabCtrlCosts.TabPages.Remove(tabPage);
                        }

                        tabCtrlCosts.TabPages.Clear();

                        // Create TabPage for the costs of the years
                        TabPage newTabPageOverviewYears = new TabPage();
                        // Set TabPage name
                        newTabPageOverviewYears.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Overview", LanguageName) +
                                                  string.Format(" ({0})", ShareObjectFinalValue.AllCostsEntries.CostValueTotalWithUnitAsString);

                        // Create Binding source for the costs data
                        BindingSource bindingSourceOverview = new BindingSource();
                        if (ShareObjectFinalValue.AllCostsEntries.GetAllCostsTotalValues().Count > 0)
                            bindingSourceOverview.DataSource =
                                ShareObjectFinalValue.AllCostsEntries.GetAllCostsTotalValues();

                        // Create DataGridView
                        DataGridView dataGridViewCostsOverviewOfAYears = new DataGridView();
                        dataGridViewCostsOverviewOfAYears.Dock = DockStyle.Fill;
                        // Bind source with costs values to the DataGridView
                        dataGridViewCostsOverviewOfAYears.DataSource = bindingSourceOverview;
                        // Set the delegate for the DataBindingComplete event
                        dataGridViewCostsOverviewOfAYears.DataBindingComplete +=
                            DataGridViewCostsOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewCostsOverviewOfAYears.SelectionChanged += DataGridViewCostsOfYears_SelectionChanged;

                        // Advanced configuration DataGridView costs
                        DataGridViewCellStyle styleOverviewOfYears =
                            dataGridViewCostsOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                        styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                        dataGridViewCostsOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        dataGridViewCostsOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                        dataGridViewCostsOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                        dataGridViewCostsOverviewOfAYears.RowHeadersVisible = false;
                        dataGridViewCostsOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                        dataGridViewCostsOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                        dataGridViewCostsOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                        dataGridViewCostsOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        dataGridViewCostsOverviewOfAYears.MultiSelect = false;

                        dataGridViewCostsOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewCostsOverviewOfAYears.AllowUserToResizeColumns = false;
                        dataGridViewCostsOverviewOfAYears.AllowUserToResizeRows = false;

                        dataGridViewCostsOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        newTabPageOverviewYears.Controls.Add(dataGridViewCostsOverviewOfAYears);
                        dataGridViewCostsOverviewOfAYears.Parent = newTabPageOverviewYears;
                        tabCtrlCosts.Controls.Add(newTabPageOverviewYears);
                        newTabPageOverviewYears.Parent = tabCtrlCosts;

                        // Check if costs pays exists
                        if (ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary.Count > 0)
                        {
                            // Loop through the years of the costs pays
                            foreach (
                                var keyName in ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary.Keys.Reverse()
                                )
                            {
                                // Create TabPage
                                TabPage newTabPage = new TabPage();
                                // Set TabPage name
                                newTabPage.Name = keyName;
                                newTabPage.Text = keyName +
                                                  string.Format(" ({0})",
                                                      ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary[keyName
                                                          ]
                                                          .CostValueYearWithUnitAsString);

                                // Create Binding source for the costs data
                                BindingSource bindingSource = new BindingSource();
                                bindingSource.DataSource =
                                    ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary[keyName].CostListYear;

                                // Create DataGridView
                                DataGridView dataGridViewCostsOfAYear = new DataGridView();
                                dataGridViewCostsOfAYear.Dock = DockStyle.Fill;
                                // Bind source with dividend values to the DataGridView
                                dataGridViewCostsOfAYear.DataSource = bindingSource;
                                // Set the delegate for the DataBindingComplete event
                                dataGridViewCostsOfAYear.DataBindingComplete +=
                                    DataGridViewCostsOfAYear_DataBindingComplete;

                                // Set row select event
                                dataGridViewCostsOfAYear.SelectionChanged += DataGridViewCostsOfAYear_SelectionChanged;

                                // Advanced configuration DataGridView costs
                                DataGridViewCellStyle style = dataGridViewCostsOfAYear.ColumnHeadersDefaultCellStyle;
                                style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                                dataGridViewCostsOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                                dataGridViewCostsOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                                dataGridViewCostsOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                                dataGridViewCostsOfAYear.RowHeadersVisible = false;
                                dataGridViewCostsOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                                dataGridViewCostsOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                                dataGridViewCostsOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                                dataGridViewCostsOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                                dataGridViewCostsOfAYear.MultiSelect = false;

                                dataGridViewCostsOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                                dataGridViewCostsOfAYear.AllowUserToResizeColumns = false;
                                dataGridViewCostsOfAYear.AllowUserToResizeRows = false;

                                dataGridViewCostsOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                                newTabPage.Controls.Add(dataGridViewCostsOfAYear);
                                dataGridViewCostsOfAYear.Parent = newTabPage;
                                tabCtrlCosts.Controls.Add(newTabPage);
                                newTabPage.Parent = tabCtrlCosts;
                            }
                        }

                        Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateCostsDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts_Error/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void DataGridViewCostsOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlCosts.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlCosts.SelectedTab.Controls.Contains((DataGridView)sender))
                        DeselectRowsOfDataGridViews((DataGridView)sender);
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                MessageBox.Show("dataGridViewCostsOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewCostsOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlCosts.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlCosts.SelectTab(tabPage);
                            tabPage.Focus();

                            // Deselect rows
                            DeselectRowsOfDataGridViews(null);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                MessageBox.Show("dataGridViewCostsOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the data binding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DataGridViewCostsOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (int i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            {
                                if (tabCtrlCosts.TabPages.Count == 1)
                                {
                                    ((DataGridView)sender).Columns[i].HeaderText =
                                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Year",
                                            LanguageName);
                                }
                                else
                                {
                                    ((DataGridView)sender).Columns[i].HeaderText =
                                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Date",
                                            LanguageName);
                                }
                                break;
                            }
                        case 1:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Costs",
                                        LanguageName) +
                                    string.Format(@" ({0})",
                                        new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                                break;
                            }
                        case 2:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Document",
                                        LanguageName) +
                                    string.Format(@" ({0})",
                                        new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                                break;
                            }
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                    ((DataGridView)sender).Rows[0].Selected = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewCostsOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts_Error/RenameHeaderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all rows when the page index of the tab control has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCtrlCosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Deselect all rows
                DeselectRowsOfDataGridViews(null);
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                MessageBox.Show("dataGridViewCostsOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Costs details

        #endregion Tab control details 
    }
}
