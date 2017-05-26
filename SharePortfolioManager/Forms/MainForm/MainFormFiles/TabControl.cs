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
        #region Tab control details

        /// <summary>
        /// This function updates the dividend, costs and profit / loss values in the tab controls
        /// if one of them two pages are chosen.
        /// </summary>
        /// <param name="sender">TabControl</param>
        /// <param name="e">EventArgs</param>
        private void tabCtrlDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the dividend page
            if (((TabControl)sender).SelectedIndex == 2)
                UpdateDividendDetails();

            // Update the costs page
            if (((TabControl)sender).SelectedIndex == 3)
                UpdateCostsDetails();

            // Update the profit or loss page
            if (((TabControl)sender).SelectedIndex == 4)
                UpdateProfitLossDetails();
        }

        #region Reset group box details

        void ResetShareDetails()
        {
            // Group box caption
            grpBoxShareDetails.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/Caption",
                _languageName);

            // Tab captions
            tabCtrlDetails.TabPages["tabPgShareDetailsWithDividendCosts"].Text =
                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendCosts/Caption",
                    _languageName);
            tabCtrlDetails.TabPages["tabPgShareDetailsWithOutDividendCosts"].Text =
                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendCosts/Caption",
                    _languageName);
            tabCtrlDetails.TabPages["tabPgDividend"].Text =
                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", _languageName);
            tabCtrlDetails.TabPages["tabPgCosts"].Text =
                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Caption", _languageName);
            tabCtrlDetails.TabPages["tabPgProfitLoss"].Text =
                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", _languageName);

            // Label values
            lblShareDetailsWithDividendCostShareDateValue.Text = @"";
            lblShareDetailsWithDividendCostShareVolumeValue.Text = @"";
            lblShareDetailsWithDividendCostShareDividendValue.Text = @"";
            lblShareDetailsWithDividendCostShareCostValue.Text = @"";
            lblShareDetailsWithDividendCostSharePriceCurrentValue.Text = @"";
            lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Text = @"";
            lblShareDetailsWithDividendCostShareDiffSumPrevValue.Text = @"";
            lblShareDetailsWithDividendCostSharePricePervValue.Text = @"";
            lblShareDetailsWithDividendCostShareDepositValue.Text = @"";
            lblShareDetailsWithDividendCostShareTotalPerformanceValue.Text = @"";
            lblShareDetailsWithDividendCostShareTotalProfitValue.Text = @"";
            lblShareDetailsWithDividendCostShareTotalSumValue.Text = @"";
        }

        #endregion Reset group box details

        #region Share details with or without dividends and costs

        /// <summary>
        /// This function updates the details information of a share
        /// </summary>
        private void UpdateShareDetails()
        {
            try
            {
                // Set GroupBox caption
                // Check if an update has already done
                if (_shareObject.LastUpdateInternet == DateTime.MinValue)
                {
                    grpBoxShareDetails.Text = _shareObject.Name + @" ( " +
                                              _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareWKN",
                                                  _languageName) +
                                              @": " + _shareObject.Wkn + @" / " +
                                              _xmlLanguage.GetLanguageTextByXPath(
                                                  @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                  _languageName) + @" " +
                                              _xmlLanguage.GetLanguageTextByXPath(
                                                  @"/MainForm/GrpBoxDetails/ShareUpdateNotDone", _languageName) +
                                              @" )";
                }
                else
                {
                    grpBoxShareDetails.Text = _shareObject.Name + @" ( " +
                                              _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareWKN",
                                                  _languageName) +
                                              @": " + _shareObject.Wkn + @" / " +
                                              _xmlLanguage.GetLanguageTextByXPath(
                                                  @"/MainForm/GrpBoxDetails/ShareUpdate",
                                                  _languageName) + @" " +
                                              string.Format(Helper.Datefulltimeshortformat, _shareObject.LastUpdateInternet)
                                                   + " )";
                }

                if (_shareObject.LastUpdateDate == DateTime.MinValue &&
                    _shareObject.LastUpdateTime == DateTime.MinValue)
                {
                    // Set the share update date
                    lblShareDetailsWithDividendCostShareDateValue.Text =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/ShareUpdateNotDone", _languageName);
                }
                else
                {
                    // Set the share update date
                    lblShareDetailsWithDividendCostShareDateValue.Text =
                        string.Format(Helper.Datefullformat, _shareObject.LastUpdateDate) + " " +
                        string.Format(Helper.Timeshortformat, _shareObject.LastUpdateTime);
                    lblShareDetailsWithOutDividendCostShareDateValue.Text =
                        string.Format(Helper.Datefullformat, _shareObject.LastUpdateDate) + " " +
                        string.Format(Helper.Timeshortformat, _shareObject.LastUpdateTime);
                }
                // Set share volume
                lblShareDetailsWithDividendCostShareVolumeValue.Text = _shareObject.VolumeAsStrUnit;
                lblShareDetailsWithOutDividendCostShareVolumeValue.Text = _shareObject.VolumeAsStrUnit;
                // Set dividend value
                lblShareDetailsWithDividendCostShareDividendValue.Text = 
                    _shareObject.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsString;
                lblShareDetailsWithOutDividendCostShareDividendValue.Text =
                    _shareObject.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsString;
                // Disable the dividend labels 
                lblShareDetailsWithOutDividendCostShareDividend.Enabled = false;
                lblShareDetailsWithOutDividendCostShareDividendValue.Enabled = false;
                // Set cost value
                lblShareDetailsWithDividendCostShareCostValue.Text =
                    _shareObject.AllCostsEntries.CostValueTotalWithUnitAsString;
                lblShareDetailsWithOutDividendCostShareCostValue.Text =
                    _shareObject.AllCostsEntries.CostValueTotalWithUnitAsString;
                // Disable the cost labels 
                lblShareDetailsWithOutDividendCostShareCost.Enabled = false;
                lblShareDetailsWithOutDividendCostShareCostValue.Enabled = false;
                // Set the current price value
                lblShareDetailsWithDividendCostSharePriceCurrentValue.Text = 
                    _shareObject.CurPriceAsStrUnit;
                lblShareDetailsWithOutDividendCostSharePriceCurrentValue.Text = 
                    _shareObject.CurPriceAsStrUnit;
                // Set performance previous value
                lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Text = 
                    _shareObject.PrevDayPerformanceAsStrUnit;
                lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.Text = 
                    _shareObject.PrevDayPerformanceAsStrUnit;
                // Set difference previous value
                lblShareDetailsWithDividendCostShareDiffSumPrevValue.Text = 
                    _shareObject.PrevDayProfitLossAsStrUnit;
                lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.Text = 
                    _shareObject.PrevDayProfitLossAsStrUnit;
                // Set previous price value
                lblShareDetailsWithDividendCostSharePricePervValue.Text = 
                    _shareObject.PrevDayPriceAsStrUnit;
                lblShareDetailsWithOutDividendCostSharePricePervValue.Text = 
                    _shareObject.PrevDayPriceAsStrUnit;
                // Set deposit value
                lblShareDetailsWithDividendCostShareDepositValue.Text = 
                    _shareObject.MarketValueAsStrUnit;
                lblShareDetailsWithOutDividendCostShareDepositValue.Text =
                    _shareObject.MarketValueAsStrUnit;
                // Set performance of the share
                lblShareDetailsWithDividendCostShareTotalPerformanceValue.Text =
                    _shareObject.PerformanceFinalValueAsStrUnit;
                lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.Text = 
                    _shareObject.PerformanceFinalValueAsStrUnit;
                // Set profit or lose of the share
                lblShareDetailsWithDividendCostShareTotalProfitValue.Text =
                    _shareObject.ProfitLossFinalValueAsStrUnit;
                lblShareDetailsWithOutDividendCostShareTotalProfitValue.Text =
                    _shareObject.ProfitLossFinalValueAsStrUnit;
                // Set total value of the share
                // TODO
                //lblShareDetailsWithDividendCostShareTotalSumValue.Text =
                //    _shareObject.ValueWithDividendsCostsProfitsLossAsStrUnit;
                //lblShareDetailsWithOutDividendCostShareTotalSumValue.Text =
                //    _shareObject.ValueWithDividendsCostsProfitsLossAsStrUnit;

                lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Font = new Font("Trebuchet MS", 10,
                    FontStyle.Bold);
                lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.Font = new Font("Trebuchet MS", 10,
                    FontStyle.Bold);
                lblShareDetailsWithDividendCostShareDiffSumPrevValue.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.Font = new Font("Trebuchet MS", 10,
                    FontStyle.Bold);
                lblShareDetailsWithDividendCostShareTotalPerformanceValue.Font = new Font("Trebuchet MS", 10,
                    FontStyle.Bold);
                lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.Font = new Font("Trebuchet MS", 10,
                    FontStyle.Bold);
                lblShareDetailsWithDividendCostShareTotalProfitValue.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                lblShareDetailsWithOutDividendCostShareTotalProfitValue.Font = new Font("Trebuchet MS", 10,
                    FontStyle.Bold);

                // Format performance value
                if (_shareObject.PerformanceFinalValue >= 0)
                {
                    lblShareDetailsWithDividendCostShareTotalPerformanceValue.ForeColor = Color.Green;
                    lblShareDetailsWithDividendCostShareTotalProfitValue.ForeColor = Color.Green;
                }
                else
                {
                    lblShareDetailsWithDividendCostShareTotalPerformanceValue.ForeColor = Color.Red;
                    lblShareDetailsWithDividendCostShareTotalProfitValue.ForeColor = Color.Red;
                }
                if (_shareObject.PerformanceFinalValue >= 0)
                {
                    lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.ForeColor = Color.Green;
                    lblShareDetailsWithOutDividendCostShareTotalProfitValue.ForeColor = Color.Green;
                }
                else
                {
                    lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.ForeColor = Color.Red;
                    lblShareDetailsWithOutDividendCostShareTotalProfitValue.ForeColor = Color.Red;
                }

                if (_shareObject.PrevDayPerformance >= 0)
                {
                    lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.ForeColor = Color.Green;
                }
                else
                {
                    lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.ForeColor = Color.Red;
                }
                if (_shareObject.PrevDayPerformance >= 0)
                {
                    lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.ForeColor = Color.Green;
                }
                else
                {
                    lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.ForeColor = Color.Red;
                }

                if (_shareObject.PrevDayProfitLoss >= 0)
                {
                    lblShareDetailsWithDividendCostShareDiffSumPrevValue.ForeColor = Color.Green;
                }
                else
                {
                    lblShareDetailsWithDividendCostShareDiffSumPrevValue.ForeColor = Color.Red;
                }
                if (_shareObject.PrevDayProfitLoss >= 0)
                {
                    lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.ForeColor = Color.Green;
                }
                else
                {
                    lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.ForeColor = Color.Red;
                }

                int iLastUpdateDay =
                    DateTime.Parse(_shareObject.LastUpdateDate.ToString(), _shareObject.CultureInfo).Day;
                int iLastUpdateMonth =
                    DateTime.Parse(_shareObject.LastUpdateDate.ToString(), _shareObject.CultureInfo).Month;
                int iLastUpdateYear =
                    DateTime.Parse(_shareObject.LastUpdateDate.ToString(), _shareObject.CultureInfo).Year;
                int iLastUpdateHour =
                    DateTime.Parse(_shareObject.LastUpdateTime.ToString(), _shareObject.CultureInfo).Hour;
                int iLastUpdateMinute =
                    DateTime.Parse(_shareObject.LastUpdateTime.ToString(), _shareObject.CultureInfo).Minute;
                int iLastUpdateSecond =
                    DateTime.Parse(_shareObject.LastUpdateTime.ToString(), _shareObject.CultureInfo).Second;

            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateShareDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgWithWithOutDividendCostsErrors/ShowFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Share details with or without dividends and costs

        #region Dividend details

        /// <summary>
        /// This function updates the dividend information of a share
        /// </summary>
        private void UpdateDividendDetails()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = _shareObject.CultureInfo;

                tabCtrlDetails.TabPages["tabPgDividend"].Text = string.Format("{0} ({1:C2})", _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", _languageName), _shareObject.AllDividendEntries.DividendValueTotalWithTaxes);

                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                if (tabCtrlDetails.SelectedIndex == 2)
                {
                    Thread.CurrentThread.CurrentCulture = _shareObject.CultureInfo;

                    // Reset tab control
                    foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                    {
                        foreach (var control in tabPage.Controls)
                        {
                            DataGridView view = control as DataGridView;
                            if (view != null)
                            {
                                view.SelectionChanged -= dataGridViewDividendsOfYears_SelectionChanged;
                                view.SelectionChanged -= dataGridViewDividendsOfAYear_SelectionChanged;
                                view.DataBindingComplete -= dataGridViewDividensOfAYear_DataBindingComplete;
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
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Overview",
                            _languageName);
                    newTabPageOverviewYears.Text =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Overview",
                            _languageName) +
                        string.Format(" ({0:C2})", _shareObject.AllDividendEntries.DividendValueTotalWithTaxes);

                    // Create Binding source for the dividend data
                    BindingSource bindingSourceOverview = new BindingSource();
                    if (_shareObject.AllDividendEntries.GetAllDividendsTotalValues().Count > 0)
                        bindingSourceOverview.DataSource =
                            _shareObject.AllDividendEntries.GetAllDividendsTotalValues();

                    // Create DataGridView
                    DataGridView dataGridViewDividendsOverviewOfAYears = new DataGridView();
                    dataGridViewDividendsOverviewOfAYears.Dock = DockStyle.Fill;
                    // Bind source with dividend values to the DataGridView
                    dataGridViewDividendsOverviewOfAYears.DataSource = bindingSourceOverview;

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewDividendsOverviewOfAYears.DataBindingComplete +=
                        dataGridViewDividensOfAYear_DataBindingComplete;

                    // Set row select event
                    dataGridViewDividendsOverviewOfAYears.SelectionChanged +=
                        dataGridViewDividendsOfYears_SelectionChanged;

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

                    // Check if dividend pays exists
                    if (_shareObject.AllDividendEntries.AllDividendsOfTheShareDictionary.Count > 0)
                    {
                        // Loop through the years of the dividend pays
                        foreach (
                            var keyName in
                                _shareObject.AllDividendEntries.AllDividendsOfTheShareDictionary.Keys.Reverse()
                            )
                        {
                            // Create TabPage
                            TabPage newTabPage = new TabPage();
                            // Set TabPage name
                            newTabPage.Name = keyName;
                            newTabPage.Text = keyName +
                                              string.Format(" ({0:C2})",
                                                  _shareObject.AllDividendEntries.AllDividendsOfTheShareDictionary[
                                                      keyName
                                                      ]
                                                      .DividendValueYear);

                            // Create Binding source for the dividend data
                            BindingSource bindingSource = new BindingSource();
                            bindingSource.DataSource =
                                _shareObject.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                                    .DividendListYear;

                            // Create DataGridView
                            DataGridView dataGridViewDividendsOfAYear = new DataGridView();
                            dataGridViewDividendsOfAYear.Dock = DockStyle.Fill;
                            // Bind source with dividend values to the DataGridView
                            dataGridViewDividendsOfAYear.DataSource = bindingSource;
                            // Set the delegate for the DataBindingComplete event
                            dataGridViewDividendsOfAYear.DataBindingComplete +=
                                dataGridViewDividensOfAYear_DataBindingComplete;

                            // Set row select event
                            dataGridViewDividendsOfAYear.SelectionChanged +=
                                dataGridViewDividendsOfAYear_SelectionChanged;

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
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateDividendDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/ShowFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewDividendsOfAYear_SelectionChanged(object sender, EventArgs args)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewDividendsOfYears_SelectionChanged(object sender, EventArgs args)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void dataGridViewDividensOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                            if (tabCtrlDividends.TabPages.Count == 1)
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Year",
                                        _languageName);
                            }
                            else
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Date",
                                        _languageName);
                            }
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Payout",
                                    _languageName) +
                                string.Format(@" ({0})",
                                    new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Dividend",
                                    _languageName) +
                                string.Format(@" ({0})",
                                    new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Yield",
                                    _languageName) +
                                string.Format(@" ({0})", ShareObject.PercentageUnit);
                            break;
                        case 4:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Price",
                                    _languageName) +
                                string.Format(@" ({0})",
                                    new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                            break;
                        case 5:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Volume",
                                    _languageName) +
                                    ShareObject.PieceUnit;
                            ;
                            break;
                        case 6:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Document",
                                    _languageName)
                            ;
                            break;
                        default:
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                {
                    ((DataGridView)sender).Rows[0].Selected = false;
                }
                //else
                //{
                //    // Update the new share values
                //    btnRefresh.PerformClick();
                //}
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewDividensOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/RenameHeaderFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all rows when the page index of the tab control has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCtrlDividends_SelectedIndexChanged(object sender, EventArgs e)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
                        DataGridView view = control as DataGridView;
                        if (view != null && view != DataGridView)
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
                        DataGridView view = control as DataGridView;
                        if (view != null && view != DataGridView)
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
        private void UpdateCostsDetails()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = _shareObject.CultureInfo;

                tabCtrlDetails.TabPages["tabPgCosts"].Text = string.Format("{0} ({1:C2})", _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Caption", _languageName), _shareObject.AllCostsEntries.CostValueTotal);

                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                if (tabCtrlDetails.SelectedIndex == 3)
                {

                    // Reset tab control
                    foreach (TabPage tabPage in tabCtrlCosts.TabPages)
                    {
                        foreach (var control in tabPage.Controls)
                        {
                            DataGridView view = control as DataGridView;
                            if (view != null)
                            {
                                view.SelectionChanged -= dataGridViewCostsOfAYear_SelectionChanged;
                                view.SelectionChanged -= dataGridViewCostsOfYears_SelectionChanged;
                                view.DataBindingComplete -= dataGridViewCostsOfAYear_DataBindingComplete;
                            }
                        }
                        tabPage.Controls.Clear();
                        tabCtrlCosts.TabPages.Remove(tabPage);
                    }

                    tabCtrlCosts.TabPages.Clear();

                    // Create TabPage for the costs of the years
                    TabPage newTabPageOverviewYears = new TabPage();
                    // Set TabPage name
                    newTabPageOverviewYears.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/Overview", _languageName) +
                                              string.Format(" ({0:C2})", _shareObject.AllCostsEntries.CostValueTotal);

                    // Create Binding source for the costs data
                    BindingSource bindingSourceOverview = new BindingSource();
                    if (_shareObject.AllCostsEntries.GetAllCostsTotalValues().Count > 0)
                        bindingSourceOverview.DataSource =
                            _shareObject.AllCostsEntries.GetAllCostsTotalValues();

                    // Create DataGridView
                    DataGridView dataGridViewCostsOverviewOfAYears = new DataGridView();
                    dataGridViewCostsOverviewOfAYears.Dock = DockStyle.Fill;
                    // Bind source with costs values to the DataGridView
                    dataGridViewCostsOverviewOfAYears.DataSource = bindingSourceOverview;
                    // Set the delegate for the DataBindingComplete event
                    dataGridViewCostsOverviewOfAYears.DataBindingComplete +=
                        dataGridViewCostsOfAYear_DataBindingComplete;

                    // Set row select event
                    dataGridViewCostsOverviewOfAYears.SelectionChanged += dataGridViewCostsOfYears_SelectionChanged;

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
                    if (_shareObject.AllCostsEntries.AllCostsOfTheShareDictionary.Count > 0)
                    {
                        // Loop through the years of the costs pays
                        foreach (
                            var keyName in _shareObject.AllCostsEntries.AllCostsOfTheShareDictionary.Keys.Reverse()
                            )
                        {
                            // Create TabPage
                            TabPage newTabPage = new TabPage();
                            // Set TabPage name
                            newTabPage.Name = keyName;
                            newTabPage.Text = keyName +
                                              string.Format(" ({0:C2})",
                                                  _shareObject.AllCostsEntries.AllCostsOfTheShareDictionary[keyName
                                                      ]
                                                      .CostValueYear);

                            // Create Binding source for the costs data
                            BindingSource bindingSource = new BindingSource();
                            bindingSource.DataSource =
                                _shareObject.AllCostsEntries.AllCostsOfTheShareDictionary[keyName].CostListYear;

                            // Create DataGridView
                            DataGridView dataGridViewCostsOfAYear = new DataGridView();
                            dataGridViewCostsOfAYear.Dock = DockStyle.Fill;
                            // Bind source with dividend values to the DataGridView
                            dataGridViewCostsOfAYear.DataSource = bindingSource;
                            // Set the delegate for the DataBindingComplete event
                            dataGridViewCostsOfAYear.DataBindingComplete +=
                                dataGridViewCostsOfAYear_DataBindingComplete;

                            // Set row select event
                            dataGridViewCostsOfAYear.SelectionChanged += dataGridViewCostsOfAYear_SelectionChanged;

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
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateCostsDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts_Error/ShowFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void dataGridViewCostsOfAYear_SelectionChanged(object sender, EventArgs args)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the choosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewCostsOfYears_SelectionChanged(object sender, EventArgs args)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the databinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void dataGridViewCostsOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                            if (tabCtrlCosts.TabPages.Count == 1)
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Year",
                                        _languageName);
                            }
                            else
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Date",
                                        _languageName);
                            }
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Costs",
                                    _languageName) +
                                string.Format(@" ({0})",
                                    new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts/DgvCostsOverview/ColHeader_Document",
                                    _languageName) +
                                string.Format(@" ({0})",
                                    new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                            break;
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgCosts_Error/RenameHeaderFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all rows when the page index of the tab control has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCtrlCosts_SelectedIndexChanged(object sender, EventArgs e)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Costs details

        #region Profit loss details

        /// <summary>
        /// This function updates the profit or loss information of a share
        /// </summary>
        private void UpdateProfitLossDetails()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = _shareObject.CultureInfo;

                tabCtrlDetails.TabPages["tabPgProfitLoss"].Text = string.Format("{0} ({1:C2})", _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", _languageName), _shareObject.AllSaleEntries.SaleProfitLossTotal);

                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                if (tabCtrlDetails.SelectedIndex == 4)
                {
                    Thread.CurrentThread.CurrentCulture = _shareObject.CultureInfo;

                    // Reset tab control
                    foreach (TabPage tabPage in tabCtrlProfitLoss.TabPages)
                    {
                        foreach (var control in tabPage.Controls)
                        {
                            DataGridView view = control as DataGridView;
                            if (view != null)
                            {
                                view.SelectionChanged -= dataGridViewProfitLossOfYears_SelectionChanged;
                                view.SelectionChanged -= dataGridViewProfitLossOfAYear_SelectionChanged;
                                view.DataBindingComplete -= dataGridViewProfitLossOfAYear_DataBindingComplete;
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
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                            _languageName);
                    newTabPageOverviewYears.Text =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                            _languageName) +
                        string.Format(" ({0:C2})", _shareObject.AllSaleEntries.SaleProfitLossTotal);

                    // Create Binding source for the profit or loss data
                    BindingSource bindingSourceOverview = new BindingSource();
                    if (_shareObject.AllSaleEntries.GetAllSalesTotalValues().Count > 0)
                        bindingSourceOverview.DataSource =
                            _shareObject.AllSaleEntries.GetAllProfitLossTotalValues();

                    // Create DataGridView
                    DataGridView dataGridViewProfitLossOverviewOfAYears = new DataGridView();
                    dataGridViewProfitLossOverviewOfAYears.Dock = DockStyle.Fill;
                    // Bind source with profit or loss values to the DataGridView
                    dataGridViewProfitLossOverviewOfAYears.DataSource = bindingSourceOverview;

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewProfitLossOverviewOfAYears.DataBindingComplete +=
                        dataGridViewProfitLossOfAYear_DataBindingComplete;

                    // Set row select event
                    dataGridViewProfitLossOverviewOfAYears.SelectionChanged +=
                        dataGridViewProfitLossOfYears_SelectionChanged;

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
                    if (_shareObject.AllSaleEntries.AllSalesOfTheShareDictionary.Count > 0)
                    {
                        // Loop through the years of the profit or loss
                        foreach (
                            var keyName in
                                _shareObject.AllSaleEntries.AllSalesOfTheShareDictionary.Keys.Reverse()
                            )
                        {
                            // Create TabPage
                            TabPage newTabPage = new TabPage();
                            // Set TabPage name
                            newTabPage.Name = keyName;
                            newTabPage.Text = keyName +
                                              string.Format(" ({0:C2})",
                                                  _shareObject.AllSaleEntries.AllSalesOfTheShareDictionary[
                                                      keyName
                                                      ]
                                                      .SaleProfitLossYear);

                            // Create Binding source for the dividend data
                            BindingSource bindingSource = new BindingSource();
                            bindingSource.DataSource =
                                _shareObject.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                    .ProfitLossListYear;

                            // Create DataGridView
                            DataGridView dataGridViewProfitLossOfAYear = new DataGridView();
                            dataGridViewProfitLossOfAYear.Dock = DockStyle.Fill;
                            // Bind source with profit or loss values to the DataGridView
                            dataGridViewProfitLossOfAYear.DataSource = bindingSource;
                            // Set the delegate for the DataBindingComplete event
                            dataGridViewProfitLossOfAYear.DataBindingComplete +=
                                dataGridViewProfitLossOfAYear_DataBindingComplete;

                            // Set row select event
                            dataGridViewProfitLossOfAYear.SelectionChanged +=
                                dataGridViewProfitLossOfAYear_SelectionChanged;
                            // Set cell decimal click event
                            dataGridViewProfitLossOfAYear.CellContentDoubleClick +=
                                dataGridViewProfitLossOfAYear_CellContentdecimalClick;

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
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("UpdateProfitLossDetails()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss_Error/ShowFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewProfitLossOfAYear_SelectionChanged(object sender, EventArgs args)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the choosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewProfitLossOfYears_SelectionChanged(object sender, EventArgs args)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the sale document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void dataGridViewProfitLossOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
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
                            foreach (var temp in _shareObject.AllSaleEntries.GetAllSalesOfTheShare())
                            {
                                // Check if the buy date and time is the same as the date and time of the clicked buy item
                                if (temp.SaleDate == strDateTime)
                                {
                                    // Check if the file still exists
                                    if (File.Exists(temp.SaleDocument))
                                        // Open the file
                                        Process.Start(temp.SaleDocument);
                                    else
                                    {
                                        string strCaption =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                                _languageName);
                                        string strMessage =
                                            _xmlLanguage.GetLanguageTextByXPath(
                                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                                _languageName);
                                        string strOk =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                                _languageName);
                                        string strCancel =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                                _languageName);

                                        OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                                            strCancel);
                                        if (messageBox.ShowDialog() == DialogResult.OK)
                                        {
                                            // Remove sale object and add it with no document
                                            if (_shareObject.RemoveSale(temp.SaleDate) &&
                                                _shareObject.AddSale(false, strDateTime, temp.SaleVolume, temp.SaleValue, temp.SaleProfitLoss, 0))
                                            {
                                                // TODO Refresh profit or loss
                                                //Show();

                                                // Add status message
                                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/EditSuccess", _languageName),
                                                    _xmlLanguage, _languageName,
                                                    Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                                                // Save the share values to the XML
                                                Exception exception = null;
                                                if (Helper.SaveShareObject(ShareObjectList[dgvPortfolio.SelectedRows[0].Index], ref _xmlPortfolio, ref _xmlReaderPortfolio, ref _xmlReaderSettingsPortfolio, PortfolioFileName, out exception))
                                                {
                                                    // Add status message
                                                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/EditSaveSuccessful", _languageName),
                                                        _xmlLanguage, _languageName,
                                                        Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                                                    // Reset / refresh DataGridView profolio bindingsource
                                                    _dgvPortfolioBindingSource.ResetBindings(false);
                                                }
                                                else
                                                {
                                                    // Add status message
                                                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveFailed", _languageName),
                                                        _xmlLanguage, _languageName,
                                                        Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                                                }
                                            }
                                            else
                                            {
                                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/EditFailed", _languageName),
                                                    _xmlLanguage, _languageName,
                                                    Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                                            }
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DocumentShowFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the databinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void dataGridViewProfitLossOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Year",
                                        _languageName);
                            }
                            else
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Date",
                                        _languageName);
                            }
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_ProfitLoss",
                                    _languageName) +
                                string.Format(@" ({0})",
                                    new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol);
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Document",
                                    _languageName)
                            ;
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                {
                    ((DataGridView)sender).Rows[0].Selected = false;
                }
                //else
                //{
                //    // Update the new share values
                //    btnRefresh.PerformClick();
                //}
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewDividensOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/RenameHeaderFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all rows when the page index of the tab control has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCtrlProfitLoss_SelectedIndexChanged(object sender, EventArgs e)
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
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Profit or loss details

        #endregion Tab control details 
    }
}
