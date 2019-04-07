//MIT License
//
//Copyright(c) 2019 nessie1980(nessie1980 @gmx.de)
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
using SharePortfolioManager.Classes.ShareObjects;
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
        #region Tab control overview

        /// <summary>
        /// This function calls the details update function
        /// </summary>
        /// <param name="sender">TabControl</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlShareOverviews_Enter(object sender, EventArgs e)
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
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    MarketValueOverviewTabSelected = false;
                    break;
                case 1:
                    MarketValueOverviewTabSelected = true;
                    break;
            }

            ResetShareDetails();

            UpdateShareDetails(MarketValueOverviewTabSelected);
            UpdateProfitLossDetails(MarketValueOverviewTabSelected);
            UpdateBrokerageDetails(MarketValueOverviewTabSelected);
            UpdateDividendDetails(MarketValueOverviewTabSelected);

            tabCtrlDetails.SelectedIndex = 0;

            if (MarketValueOverviewTabSelected)
                dgvPortfolioMarketValue.Focus();
            else
                dgvPortfolioFinalValue.Focus();

            OnResizeDataGridView();
        }

        #endregion Tab control overview

        #region Tab control details

        /// <summary>
        /// This function updates the dividend, brokerage and profit / loss values in the tab controls
        /// if one of them three pages are chosen.
        /// </summary>
        /// <param name="sender">TabControl</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the profit or loss page
            switch (((TabControl)sender).SelectedIndex)
            {
                case 1:
                    UpdateProfitLossDetails(MarketValueOverviewTabSelected);
                    break;
                case 2:
                    UpdateDividendDetails(MarketValueOverviewTabSelected);
                    break;
                case 3:
                    UpdateBrokerageDetails(MarketValueOverviewTabSelected);
                    break;
            }

            // Update the dividend page

            // Update the brokerage page
        }

        #region Reset group box details

        private void ResetShareDetails()
        {
            // Group box caption
            grpBoxShareDetails.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/Caption",
                LanguageName);

            // Tab captions
            if (tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsFinalValue))
                tabCtrlDetails.TabPages[_tabPageDetailsFinalValue].Text =
                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Caption",
                    LanguageName);

            if (tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsMarketValue))
                tabCtrlDetails.TabPages[_tabPageDetailsMarketValue].Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Caption",
                        LanguageName);

            if (tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsProfitLossValue))
                tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue].Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", LanguageName);

            if (tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsDividendValue))
                tabCtrlDetails.TabPages[_tabPageDetailsDividendValue].Text =
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", LanguageName);

            if (tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsBrokerageValue))
                tabCtrlDetails.TabPages[_tabPageDetailsBrokerageValue].Text =
                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Caption", LanguageName);

            // Label values of the market value
            lblDetailsMarketValueDateValue.Text = @"";
            lblDetailsMarketValueVolumeValue.Text = @"";
            lblDetailsMarketValueDividendValue.Text = @"";
            lblDetailsMarketValueBrokerageValue.Text = @"";
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
            lblDetailsFinalValueBrokerageValue.Text = @"";
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

        #region Share details with or without dividends and brokerage

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
                    if (tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsFinalValue))
                    {
                        // Save page for later adding when the final value overview is selected again
                        _tempFinalValues = tabCtrlDetails.TabPages[_tabPageDetailsFinalValue];
                        tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[_tabPageDetailsFinalValue]);
                    }

                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsMarketValue))
                        tabCtrlDetails.TabPages.Insert(0, _tempMarketValues);

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
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareType",
                                                      LanguageName) +
                                                  @" " +
                                                  Helper.GetComboBoxItems(@" / ComboBoxItemsShareType/*", LanguageName,
                                                      Language)[ShareObjectMarketValue.ShareType] +
                                                  @" / " +
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
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareType",
                                                      LanguageName) +
                                                  @" " +
                                                  Helper.GetComboBoxItems(@" / ComboBoxItemsShareType/*", LanguageName,
                                                      Language)[ShareObjectMarketValue.ShareType] +
                                                  @" / " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                      LanguageName) + @" " +
                                                  string.Format(Helper.Datefulltimeshortformat, ShareObjectMarketValue.LastUpdateInternet)
                                                       + @" )";
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
                            string.Format(Helper.Datefullformat, ShareObjectMarketValue.LastUpdateDate) + @" " +
                            string.Format(Helper.Timeshortformat, ShareObjectMarketValue.LastUpdateTime);
                        lblDetailsMarketValueDateValue.Text =
                            string.Format(Helper.Datefullformat, ShareObjectMarketValue.LastUpdateDate) + @" " +
                            string.Format(Helper.Timeshortformat, ShareObjectMarketValue.LastUpdateTime);
                    }
                    // Set share volume
                    lblDetailsMarketValueVolumeValue.Text = ShareObjectMarketValue.VolumeAsStrUnit;

                    // Set dividend value
                    lblDetailsMarketValueDividendValue.Text = @"-";
                    // Disable the dividend labels 
                    lblDetailsMarketValueDividend.Enabled = false;
                    lblDetailsMarketValueDividendValue.Enabled = false;

                    // Set brokerage value
                    lblDetailsMarketValueBrokerageValue.Text = @"-";
                    // Disable the Brokerage labels 
                    lblDetailsMarketValueBrokerage.Enabled = false;
                    lblDetailsMarketValueBrokerageValue.Enabled = false;

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

                    lblDetailsMarketValueDiffPerformancePrevValue.ForeColor = ShareObjectMarketValue.PrevDayPerformance >= 0 ? Color.Green : Color.Red;

                    lblDetailsMarketValueDiffSumPrevValue.ForeColor = ShareObjectMarketValue.PrevDayProfitLoss >= 0 ? Color.Green : Color.Red;

                    var iLastUpdateDay =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateDate.ToString("G"), ShareObjectMarketValue.CultureInfo).Day;
                    var iLastUpdateMonth =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateDate.ToString("G"), ShareObjectMarketValue.CultureInfo).Month;
                    var iLastUpdateYear =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateDate.ToString("G"), ShareObjectMarketValue.CultureInfo).Year;
                    var iLastUpdateHour =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateTime.ToString("G"), ShareObjectMarketValue.CultureInfo).Hour;
                    var iLastUpdateMinute =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateTime.ToString("G"), ShareObjectMarketValue.CultureInfo).Minute;
                    var iLastUpdateSecond =
                        DateTime.Parse(ShareObjectMarketValue.LastUpdateTime.ToString("G"), ShareObjectMarketValue.CultureInfo).Second;
                }
                else
                {
                    // Remove page
                    if (tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsMarketValue))
                    {
                        // Save page for later adding when the final value overview is selected again
                        _tempMarketValues = tabCtrlDetails.TabPages[_tabPageDetailsMarketValue];
                        tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[_tabPageDetailsMarketValue]);
                    }

                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsFinalValue))
                        tabCtrlDetails.TabPages.Insert(0, _tempFinalValues);
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsDividendValue))
                        tabCtrlDetails.TabPages.Insert(1, _tempDividends);
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsProfitLossValue))
                        tabCtrlDetails.TabPages.Insert(2, _tempProfitLoss);
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsBrokerageValue))
                        tabCtrlDetails.TabPages.Insert(3, _tempBrokerage);

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
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareType",
                                                      LanguageName) +
                                                  @" " +
                                                  Helper.GetComboBoxItems(@" / ComboBoxItemsShareType/*", LanguageName,
                                                      Language)[ShareObjectFinalValue.ShareType] +
                                                  @" / " +
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
                                                  Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareType",
                                                      LanguageName) +
                                                  @" " +
                                                  Helper.GetComboBoxItems(@" / ComboBoxItemsShareType/*", LanguageName,
                                                      Language)[ShareObjectFinalValue.ShareType] +
                                                  @" / " +
                                                  Language.GetLanguageTextByXPath(
                                                      @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                      LanguageName) + @" " +
                                                  string.Format(Helper.Datefulltimeshortformat, ShareObjectFinalValue.LastUpdateInternet)
                                                       + @" )";
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
                            string.Format(Helper.Datefullformat, ShareObjectFinalValue.LastUpdateDate) + @" " +
                            string.Format(Helper.Timeshortformat, ShareObjectFinalValue.LastUpdateTime);
                        lblDetailsMarketValueDateValue.Text =
                            string.Format(Helper.Datefullformat, ShareObjectFinalValue.LastUpdateDate) + @" " +
                            string.Format(Helper.Timeshortformat, ShareObjectFinalValue.LastUpdateTime);
                    }

                    // Set share volume
                    lblDetailsFinalValueVolumeValue.Text = ShareObjectFinalValue.VolumeAsStrUnit;

                    // Set dividend value
                    lblDetailsFinalValueDividendValue.Text =
                        ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsStr;
                    // Disable the dividend labels 
                    lblDetailsMarketValueDividend.Enabled = false;
                    lblDetailsMarketValueDividendValue.Enabled = false;
                    
                    // Set brokerage value
                    lblDetailsFinalValueBrokerageValue.Text =
                        ShareObjectFinalValue.AllBrokerageEntries.BrokerageValueTotalWithUnitAsStr;
                    // Disable the brokerage labels 
                    lblDetailsMarketValueBrokerage.Enabled = false;
                    lblDetailsMarketValueBrokerageValue.Enabled = false;

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

                    lblDetailsFinalValueDiffPerformancePrevValue.ForeColor = ShareObjectFinalValue.PrevDayPerformance >= 0 ? Color.Green : Color.Red;

                    lblDetailsFinalValueDiffSumPrevValue.ForeColor = ShareObjectFinalValue.PrevDayProfitLoss >= 0 ? Color.Green : Color.Red;

                    var iLastUpdateDay =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateDate.ToString("G"), ShareObjectFinalValue.CultureInfo).Day;
                    var iLastUpdateMonth =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateDate.ToString("G"), ShareObjectFinalValue.CultureInfo).Month;
                    var iLastUpdateYear =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateDate.ToString("G"), ShareObjectFinalValue.CultureInfo).Year;
                    var iLastUpdateHour =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateTime.ToString("G"), ShareObjectFinalValue.CultureInfo).Hour;
                    var iLastUpdateMinute =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateTime.ToString("G"), ShareObjectFinalValue.CultureInfo).Minute;
                    var iLastUpdateSecond =
                        DateTime.Parse(ShareObjectFinalValue.LastUpdateTime.ToString("G"), ShareObjectFinalValue.CultureInfo).Second;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithWithOutDividendBrokerageErrors/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Share details with or without dividends and brokerage

        #region Profit loss details

        /// <summary>
        /// This function updates the profit or loss information of a share
        /// </summary>
        private void UpdateProfitLossDetails(bool bShowMarketValue)
        {
            try
            {
                if (bShowMarketValue)
                {
                    // Remove page
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsProfitLossValue)) return;

                    // Save page for later adding when the final value overview is selected again
                    _tempProfitLoss = tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue];
                    tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue]);
                }
                else
                {
                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsProfitLossValue))
                        tabCtrlDetails.TabPages.Insert(1, _tempProfitLoss);

                    // Check if a share is selected
                    if (ShareObjectFinalValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                    tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue].Text =
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", LanguageName)
                            } ({
                                Helper.FormatDecimal(ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotal,
                                    Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"",
                                    ShareObjectFinalValue.CultureInfo)
                            })";

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                    if (tabCtrlDetails.SelectedIndex != 1) return;

                    Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                    // Reset tab control
                    foreach (TabPage tabPage in tabCtrlProfitLoss.TabPages)
                    {
                        foreach (var control in tabPage.Controls)
                        {
                            if (!(control is DataGridView view)) continue;

                            view.SelectionChanged -= DataGridViewProfitLossOfYears_SelectionChanged;
                            view.SelectionChanged -= DataGridViewProfitLossOfAYear_SelectionChanged;
                            view.DataBindingComplete -= DataGridViewProfitLossOfAYear_DataBindingComplete;
                        }
                        tabPage.Controls.Clear();
                        tabCtrlProfitLoss.TabPages.Remove(tabPage);
                    }

                    tabCtrlProfitLoss.TabPages.Clear();

                    // Create TabPage for the profit or loss of the years
                    var newTabPageOverviewYears = new TabPage
                    {
                        Name = Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                            LanguageName),
                        Text = Language.GetLanguageTextByXPath(
                                   @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                                   LanguageName) +
                               $@" ({ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotal:C2})"
                    };
                    // Set TabPage name

                    // Create Binding source for the profit or loss data
                    var bindingSourceOverview = new BindingSource();
                    if (ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Count > 0)
                        bindingSourceOverview.DataSource =
                            ShareObjectFinalValue.AllSaleEntries.GetAllProfitLossTotalValues();

                    // Create DataGridView
                    var dataGridViewProfitLossOverviewOfAYears = new DataGridView
                    {
                        Dock = DockStyle.Fill,

                        // Bind source with profit or loss values to the DataGridView
                        DataSource = bindingSourceOverview
                    };

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewProfitLossOverviewOfAYears.DataBindingComplete +=
                        DataGridViewProfitLossOfAYear_DataBindingComplete;

                    // Set row select event
                    dataGridViewProfitLossOverviewOfAYears.SelectionChanged +=
                        DataGridViewProfitLossOfYears_SelectionChanged;

                    // Advanced configuration DataGridView profit or loss
                    var styleOverviewOfYears =
                        dataGridViewProfitLossOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                    styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
                            var newTabPage = new TabPage
                            {
                                Name = keyName,
                                Text = keyName +
                                       $@" ({Helper.FormatDecimal(ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName].SaleProfitLossYear, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, true, @"", ShareObjectFinalValue.CultureInfo)})"
                            };
                            // Set TabPage name

                            // Create Binding source for the dividend data
                            var bindingSource = new BindingSource
                            {
                                DataSource = ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                    .ProfitLossListYear
                            };

                            // Create DataGridView
                            var dataGridViewProfitLossOfAYear = new DataGridView
                            {
                                Dock = DockStyle.Fill,

                                // Bind source with profit or loss values to the DataGridView
                                DataSource = bindingSource
                            };

                            // Set the delegate for the DataBindingComplete event
                            dataGridViewProfitLossOfAYear.DataBindingComplete +=
                                DataGridViewProfitLossOfAYear_DataBindingComplete;

                            // Set row select event
                            dataGridViewProfitLossOfAYear.SelectionChanged +=
                                DataGridViewProfitLossOfAYear_SelectionChanged;
                            // Set cell decimal click event
                            dataGridViewProfitLossOfAYear.CellContentDoubleClick +=
                                DataGridViewProfitLossOfAYear_CellContentDoubleClick;

                            // Advanced configuration DataGridView profit or loss
                            var style = dataGridViewProfitLossOfAYear.ColumnHeadersDefaultCellStyle;
                            style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
                if (tabCtrlProfitLoss.TabPages.Count <= 0) return;

                // Deselect row only of the other TabPages DataGridViews
                if (tabCtrlProfitLoss.SelectedTab.Controls.Contains((DataGridView)sender))
                    DeselectRowsOfDataGridViews((DataGridView)sender);
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlProfitLoss.TabPages)
                {
                    if (tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    tabCtrlProfitLoss.SelectTab(tabPage);
                    tabPage.Focus();

                    // Deselect rows
                    DeselectRowsOfDataGridViews(null);

                    break;
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
        private void DataGridViewProfitLossOfAYear_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                var iColumnCount = ((DataGridView)sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex != iColumnCount - 1) return;

                // Check if a row is selected
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the current selected row
                var curItem = ((DataGridView)sender).SelectedRows;
                // Get date and time of the selected buy item
                var strDateTime = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value.ToString() == @"-") return;

                // Get doc from the profit or loss with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
                {
                    // Check if the buy date and time is the same as the date and time of the clicked buy item
                    if (temp.Date != strDateTime) continue;

                    // Check if the file still exists
                    if (File.Exists(temp.Document))
                        // Open the file
                        Process.Start(temp.Document);
                    else
                    {
                        var strCaption =
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                LanguageName);
                        var strMessage =
                            Language.GetLanguageTextByXPath(
                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                LanguageName);
                        var strOk =
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                LanguageName);
                        var strCancel =
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                LanguageName);

                        var messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                            strCancel);
                        if (messageBox.ShowDialog() == DialogResult.OK)
                        {
                            //// Remove sale object and add it with no document
                            //if (ShareObjectFinalValue.RemoveSale(temp.Guid, temp.Date) &&
                            //    ShareObjectFinalValue.AddSale(temp.Guid, strDateTime, temp.OrderNumber, temp.Volume, temp.SalePrice, temp.SaleBuyDetails, temp.TaxAtSource, temp.CapitalGainsTax, temp.SolidarityTax, temp.Brokerage, temp.Reduction))
                            //{
                            //    // TODO Refresh profit or loss
                            //    //Show();

                            //    // Add status message
                            //    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            //        Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/EditSuccess", LanguageName),
                            //        Language, LanguageName,
                            //        Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                            //    // Save the share values to the XML
                            //    if (ShareObjectFinalValue.SaveShareObject(ShareObjectListFinalValue[dgvPortfolioFinalValue.SelectedRows[0].Index], ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, _portfolioFileName, out var exception))
                            //    {
                            //        // Add status message
                            //        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            //            Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/EditSaveSuccessful", LanguageName),
                            //            Language, LanguageName,
                            //            Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                            //        // Reset / refresh DataGridView portfolio binding source
                            //        DgvPortfolioBindingSourceFinalValue.ResetBindings(false);
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
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
                for (var i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            {
                                if (tabCtrlProfitLoss.TabPages.Count == 1)
                                    ((DataGridView) sender).Columns[i].HeaderText = Language.GetLanguageTextByXPath(
                                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Year",
                                        LanguageName
                                    );
                                else
                                    ((DataGridView)sender).Columns[i].HeaderText = Language.GetLanguageTextByXPath(
                                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Date",
                                        LanguageName
                                    );

                                break;
                            }
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_ProfitLoss",
                                    LanguageName) +
                                $@" ({new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol})";
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
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
            try
            {
                // Deselect all rows
                DeselectRowsOfDataGridViews(null);
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsDividendValue)) return;

                    // Save page for later adding when the final value overview is selected again
                    _tempDividends = tabCtrlDetails.TabPages[_tabPageDetailsDividendValue];
                    tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[_tabPageDetailsDividendValue]);
                }
                else
                {
                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsDividendValue))
                        tabCtrlDetails.TabPages.Insert(2, _tempDividends);

                    // Check if a share is selected
                    if (ShareObjectFinalValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                    tabCtrlDetails.TabPages[_tabPageDetailsDividendValue].Text =
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", LanguageName)
                            } ({ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsStr})";

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                    if (tabCtrlDetails.SelectedIndex != 2) return;

                    // Reset tab control
                    foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                    {
                        foreach (var control in tabPage.Controls)
                        {
                            if (!(control is DataGridView view)) continue;

                            view.SelectionChanged -= DataGridViewDividendsOfYears_SelectionChanged;
                            view.SelectionChanged -= DataGridViewDividendsOfAYear_SelectionChanged;
                            view.DataBindingComplete -= DataGridViewDividendsOfAYear_DataBindingComplete;
                        }
                        tabPage.Controls.Clear();
                        tabCtrlDividends.TabPages.Remove(tabPage);
                    }

                    tabCtrlDividends.TabPages.Clear();

                    // Create TabPage for the dividends of the years
                    var newTabPageOverviewYears = new TabPage
                    {
                        // Set TabPage name
                        Name = Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Overview",
                            LanguageName),

                        // Set TabPage caption
                        Text = Language.GetLanguageTextByXPath(
                                   @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Overview",
                                   LanguageName) +
                               $@" ({ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsStr})"
                    };

                    // Create Binding source for the dividend data
                    var bindingSourceOverview = new BindingSource();
                    if (ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues().Count > 0)
                        bindingSourceOverview.DataSource =
                            ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues();

                    // Create DataGridView
                    var dataGridViewDividendsOverviewOfAYears = new DataGridView
                    {
                        Dock = DockStyle.Fill,

                        // Bind source with dividend values to the DataGridView
                        DataSource = bindingSourceOverview
                    };

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewDividendsOverviewOfAYears.DataBindingComplete +=
                        DataGridViewDividendsOfAYear_DataBindingComplete;

                    // Set row select event
                    dataGridViewDividendsOverviewOfAYears.SelectionChanged +=
                        DataGridViewDividendsOfYears_SelectionChanged;

                    // Advanced configuration DataGridView dividends
                    var styleOverviewOfYears =
                        dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                    styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
                            var newTabPage = new TabPage
                            {
                                // Set TabPage name
                                Name = keyName,

                                // Set TabPage caption
                                Text = keyName +
                                       $@" ({ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName].DividendValueYearWithUnitAsStr})"
                            };

                            // Create Binding source for the dividend data
                            var bindingSource = new BindingSource
                            {
                                DataSource = ShareObjectFinalValue.AllDividendEntries
                                    .AllDividendsOfTheShareDictionary[keyName]
                                    .DividendListYear
                            };

                            // Create DataGridView
                            var dataGridViewDividendsOfAYear = new DataGridView
                            {
                                Dock = DockStyle.Fill,

                                // Bind source with dividend values to the DataGridView
                                DataSource = bindingSource
                            };

                            // Set the delegate for the DataBindingComplete event
                            dataGridViewDividendsOfAYear.DataBindingComplete +=
                                DataGridViewDividendsOfAYear_DataBindingComplete;

                            // Set row select event
                            dataGridViewDividendsOfAYear.SelectionChanged +=
                                DataGridViewDividendsOfAYear_SelectionChanged;

                            // Advanced configuration DataGridView dividends
                            var style = dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle;
                            style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
                if (tabCtrlDividends.TabPages.Count <= 0) return;

                // Deselect row only of the other TabPages DataGridViews
                if (tabCtrlDividends.SelectedTab.Controls.Contains((DataGridView)sender))
                    DeselectRowsOfDataGridViews((DataGridView)sender);
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                {
                    if (tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    tabCtrlDividends.SelectTab(tabPage);
                    tabPage.Focus();

                    // Deselect rows
                    DeselectRowsOfDataGridViews(null);

                    break;
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
        private void DataGridViewDividendsOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (var i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            {
                                if (tabCtrlDividends.TabPages.Count == 1)
                                    ((DataGridView)sender).Columns[i].HeaderText = Language.GetLanguageTextByXPath(
                                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Year",
                                        LanguageName);
                                else
                                    ((DataGridView)sender).Columns[i].HeaderText = Language.GetLanguageTextByXPath(
                                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Date",
                                        LanguageName);
                                break;                    
                            }
                        case 1:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Payout",
                                        LanguageName) +
                                    $@" ({new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol})";
                                break;
                            }
                        case 2:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Dividend",
                                        LanguageName) +
                                    $@" ({new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol})";
                                break;
                            }
                        case 3:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Yield",
                                        LanguageName) +
                                    $@" ({ShareObject.PercentageUnit})";
                                break;
                            }
                        case 4:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Price",
                                        LanguageName) +
                                    $@" ({new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol})";
                                break;
                            }
                        case 5:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Volume",
                                        LanguageName) +
                                        ShareObject.PieceUnit;
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
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                    ((DataGridView)sender).Rows[0].Selected = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
            try
            {
                // Deselect all rows
                DeselectRowsOfDataGridViews(null);
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Dividend details

        #region Brokerage details

        /// <summary>
        /// This function updates the brokerage information of a share
        /// </summary>
        private void UpdateBrokerageDetails(bool bShowMarketValue)
        {
            try
            {
                if (bShowMarketValue)
                {

                    // Remove page
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsBrokerageValue)) return;

                    // Save page for later adding when the final value overview is selected again
                    _tempBrokerage = tabCtrlDetails.TabPages[_tabPageDetailsBrokerageValue];
                    tabCtrlDetails.TabPages.Remove(tabCtrlDetails.TabPages[_tabPageDetailsBrokerageValue]);
                }
                else
                {
                    // Add page
                    if (!tabCtrlDetails.TabPages.ContainsKey(_tabPageDetailsBrokerageValue))
                        tabCtrlDetails.TabPages.Insert(3, _tempBrokerage);

                    // Check if a share is selected
                    if (ShareObjectFinalValue == null)
                    {
                        ResetShareDetails();
                        return;
                    }

                    Thread.CurrentThread.CurrentCulture = ShareObjectFinalValue.CultureInfo;

                    tabCtrlDetails.TabPages[_tabPageDetailsBrokerageValue].Text =
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Caption", LanguageName)
                            } ({ShareObjectFinalValue.AllBrokerageEntries.BrokerageValueTotalWithUnitAsStr})";

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                    if (tabCtrlDetails.SelectedIndex != 3) return;

                    // Reset tab control
                    foreach (TabPage tabPage in tabCtrlBrokerage.TabPages)
                    {
                        foreach (var control in tabPage.Controls)
                        {
                            if (!(control is DataGridView view)) continue;

                            view.SelectionChanged -= DataGridViewBrokerageOfAYear_SelectionChanged;
                            view.SelectionChanged -= DataGridViewBrokerageOfYears_SelectionChanged;
                            view.DataBindingComplete -= DataGridViewBrokerageOfAYear_DataBindingComplete;
                        }
                        tabPage.Controls.Clear();
                        tabCtrlBrokerage.TabPages.Remove(tabPage);
                    }

                    tabCtrlBrokerage.TabPages.Clear();

                    // Create TabPage for the brokerage of the years
                    var newTabPageOverviewYears = new TabPage
                    {
                        Name = Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Overview",
                            LanguageName),
                        Text = Language.GetLanguageTextByXPath(
                                   @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Overview", LanguageName) +
                               $@" ({ShareObjectFinalValue.AllBrokerageEntries.BrokerageValueTotalWithUnitAsStr})"
                    };
                    // Set TabPage name

                    // Create Binding source for the brokerage data
                    var bindingSourceOverview = new BindingSource();
                    if (ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues().Count > 0)
                        bindingSourceOverview.DataSource =
                            ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues();

                    // Create DataGridView
                    var dataGridViewBrokerageOverviewOfAYears = new DataGridView
                    {
                        Dock = DockStyle.Fill,

                        // Bind source with brokerage values to the DataGridView
                        DataSource = bindingSourceOverview
                    };
                    // Set the delegate for the DataBindingComplete event
                    dataGridViewBrokerageOverviewOfAYears.DataBindingComplete +=
                        DataGridViewBrokerageOfAYear_DataBindingComplete;

                    // Set row select event
                    dataGridViewBrokerageOverviewOfAYears.SelectionChanged += DataGridViewBrokerageOfYears_SelectionChanged;

                    // Advanced configuration DataGridView brokerage
                    var styleOverviewOfYears =
                        dataGridViewBrokerageOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                    styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dataGridViewBrokerageOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    dataGridViewBrokerageOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    dataGridViewBrokerageOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                    dataGridViewBrokerageOverviewOfAYears.RowHeadersVisible = false;
                    dataGridViewBrokerageOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridViewBrokerageOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dataGridViewBrokerageOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                    dataGridViewBrokerageOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    dataGridViewBrokerageOverviewOfAYears.MultiSelect = false;

                    dataGridViewBrokerageOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewBrokerageOverviewOfAYears.AllowUserToResizeColumns = false;
                    dataGridViewBrokerageOverviewOfAYears.AllowUserToResizeRows = false;

                    dataGridViewBrokerageOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    newTabPageOverviewYears.Controls.Add(dataGridViewBrokerageOverviewOfAYears);
                    dataGridViewBrokerageOverviewOfAYears.Parent = newTabPageOverviewYears;
                    tabCtrlBrokerage.Controls.Add(newTabPageOverviewYears);
                    newTabPageOverviewYears.Parent = tabCtrlBrokerage;

                    // Check if brokerage pays exists
                    if (ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary.Count > 0)
                    {
                        // Loop through the years of the brokerage pays
                        foreach (
                            var keyName in ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary.Keys.Reverse()
                        )
                        {
                            // Create TabPage
                            var newTabPage = new TabPage
                            {
                                Name = keyName,
                                Text = keyName +
                                       $@" ({ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName].BrokerageValueYearWithUnitAsStr})"
                            };
                            // Set TabPage name

                            // Create Binding source for the brokerage data
                            var bindingSource = new BindingSource
                            {
                                DataSource =
                                    ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName]
                                        .BrokerageReductionListYear
                            };

                            // Create DataGridView
                            var dataGridViewBrokerageOfAYear = new DataGridView
                            {
                                Dock = DockStyle.Fill,

                                // Bind source with dividend values to the DataGridView
                                DataSource = bindingSource
                            };

                            // Set the delegate for the DataBindingComplete event
                            dataGridViewBrokerageOfAYear.DataBindingComplete +=
                                DataGridViewBrokerageOfAYear_DataBindingComplete;

                            // Set row select event
                            dataGridViewBrokerageOfAYear.SelectionChanged += DataGridViewBrokerageOfAYear_SelectionChanged;

                            // Advanced configuration DataGridView brokerage
                            var style = dataGridViewBrokerageOfAYear.ColumnHeadersDefaultCellStyle;
                            style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                            dataGridViewBrokerageOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                            dataGridViewBrokerageOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                            dataGridViewBrokerageOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                            dataGridViewBrokerageOfAYear.RowHeadersVisible = false;
                            dataGridViewBrokerageOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                            dataGridViewBrokerageOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                            dataGridViewBrokerageOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                            dataGridViewBrokerageOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                            dataGridViewBrokerageOfAYear.MultiSelect = false;

                            dataGridViewBrokerageOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            dataGridViewBrokerageOfAYear.AllowUserToResizeColumns = false;
                            dataGridViewBrokerageOfAYear.AllowUserToResizeRows = false;

                            dataGridViewBrokerageOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            newTabPage.Controls.Add(dataGridViewBrokerageOfAYear);
                            dataGridViewBrokerageOfAYear.Parent = newTabPage;
                            tabCtrlBrokerage.Controls.Add(newTabPage);
                            newTabPage.Parent = tabCtrlBrokerage;
                        }
                    }

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage_Error/ShowFailed", LanguageName),
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
        private void DataGridViewBrokerageOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlBrokerage.TabPages.Count <= 0) return;

                // Deselect row only of the other TabPages DataGridViews
                if (tabCtrlBrokerage.SelectedTab.Controls.Contains((DataGridView)sender))
                    DeselectRowsOfDataGridViews((DataGridView)sender);
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
        private void DataGridViewBrokerageOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlBrokerage.TabPages)
                {
                    if (tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    tabCtrlBrokerage.SelectTab(tabPage);
                    tabPage.Focus();

                    // Deselect rows
                    DeselectRowsOfDataGridViews(null);

                    break;
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);

#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
        private void DataGridViewBrokerageOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (var i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            {
                                if (tabCtrlBrokerage.TabPages.Count == 1)
                                    ((DataGridView) sender).Columns[i].HeaderText = Language.GetLanguageTextByXPath(
                                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Year",
                                        LanguageName
                                    );
                                else
                                    ((DataGridView)sender).Columns[i].HeaderText = Language.GetLanguageTextByXPath(
                                        @"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Date",
                                        LanguageName);
                                break;
                            }
                        case 1:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Brokerage",
                                        LanguageName) +
                                    $@" ({new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol})";
                                break;
                            }
                        case 2:
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Document",
                                        LanguageName) +
                                    $@" ({new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol})";
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
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage_Error/RenameHeaderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all rows when the page index of the tab control has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCtrlBrokerage_SelectedIndexChanged(object sender, EventArgs e)
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
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Brokerage details

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        private void DeselectRowsOfDataGridViews(DataGridView dataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                {
                    foreach (Control control in tabPage.Controls)
                    {
                        if (!(control is DataGridView view) || view == dataGridView) continue;

                        foreach (DataGridViewRow selectedRow in view.SelectedRows)
                        {
                            selectedRow.Selected = false;
                        }
                    }
                }

                // Deselect the row
                foreach (TabPage tabPage in tabCtrlBrokerage.TabPages)
                {
                    foreach (Control control in tabPage.Controls)
                    {
                        if (!(control is DataGridView view) || view == dataGridView) continue;

                        foreach (DataGridViewRow selectedRow in view.SelectedRows)
                        {
                            selectedRow.Selected = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
            }
        }

        #endregion Tab control details 
    }
}
