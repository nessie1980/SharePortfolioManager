//MIT License
//
//Copyright(c) 2017 - 2022 nessie1980(nessie1980@gmx.de)
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
//#define DEBUG_DIVIDEND_DETAILS

using SharePortfolioManager.Classes;
using SharePortfolioManager.OwnMessageBoxForm;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharePortfolioManager.Classes.Configurations;

// TODO: (thomas:2022-08-19) Correct namespace
namespace SharePortfolioManager.ShareDetailsForm
{
    public partial class FrmShareDetails
    {
        // Column indices values of the overview DataGridView
        public enum DataGridViewOverViewIndices
        {
            ColumnIndicesYear,
            ColumnIndicesDividendValue,
            ColumnIndicesScrollBar
        }

        // Column indices values of the years DataGridView
        public enum DataGridViewYearIndices
        {
            ColumnIndicesGuid,
            ColumnIndicesDate,
            ColumnIndicesVolume,
            ColumnIndicesDividend,
            ColumnIndicesYield,
            ColumnIndicesPayout,
            ColumnIndicesDocument,
            ColumnIndicesScrollBar
        }

        /// <summary>
        /// Postfix for the DataGridView footer name
        /// </summary>
        private readonly string _dataGridViewFooterPostfix = "_Footer";

        /// <summary>
        /// If the row count is greater then 4 the scrollbar will be shown
        /// </summary>
        private const int DataGridViewRowsScrollbar = 4;

        /// <summary>
        /// Height of the DataGridView footer
        /// </summary>
        private const int DataGridViewFooterHeight = 24;

        #region Column width for the dgvContentOverView and dgvContentOverViewFooter

        private const int OverViewDateColumnSize = 85;

        private const int YearDateColumnSize = 85;
        private const int YearVolumeColumnSize = 155;
        private const int YearDividendColumnSize = 145;
        private const int YearYieldColumnSize = 100;
        private const int YearPayoutColumnSize = 150;

        #endregion Column width for the dgvPortfolio and dgvPortfolioFooter

        #region Dividend details

        /// <summary>
        /// This function updates the dividend information of a share
        /// </summary>
        private void UpdateDividendDetails()
        {
            try
            {
                // Check if a share is selected
                if (ShareObjectFinalValue == null)
                {
                    ResetShareDetails();
                    return;
                }

                // Set tab page caption
                tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Text =
                    $@"{
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", SettingsConfiguration.LanguageName)
                        } ({ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesAsStrUnit})";

                #region Reset tab control

                // Disconnect delegates and remove pages
                //tabPgDetailsDividendValues.SelectedIndexChanged -= OnTabCtrlDividend_SelectedIndexChanged;
                foreach (TabPage tabPage in tabPgDetailsDividendValues.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewDividendOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewDividendOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewDividendOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewDividendOfAYear_MouseEnter;
//                            dataGridView.CellContentDoubleClick -=
//                                OnDataGridViewDividendOfAYear_CellContentDecimalClick;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewDividend_DataBindingComplete;
                    }

                    tabPage.Controls.Clear();
                    tabPgDetailsDividendValues.TabPages.Remove(tabPage);
                }

                tabPgDetailsDividendValues.TabPages.Clear();

                #endregion Reset tab control

                #region Add years overview page

                //tabPgDetailsDividendValues.SelectedIndexChanged += OnTabCtrlDividend_SelectedIndexChanged;

                // Create TabPage for the dividend of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = @"Overview",

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Overview",
                               LanguageName) +
                           $@" ({ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesAsStrUnit})"
                };

                #region Data source, data binding and data grid view

                // Check if dividend exists
                if (ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues().Count <= 0) return;

                // Reverse list so the latest is a top of the data grid view
                var reversDataSourceOverview = ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues()
                    .OrderByDescending(x => x.DgvDividendYear).ToList();

                var bindingSourceOverview = new BindingSource
                {
                    DataSource = reversDataSourceOverview
                };

                // Create DataGridView
                var dataGridViewDividendOverviewOfYears = new DataGridView
                {
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with dividend values to the DataGridView
                    DataSource = bindingSourceOverview,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                };

                // Create DataGridView
                var dataGridViewDividendsOverviewOfAYearsFooter = new DataGridView
                {
                    Name = @"Overview" + _dataGridViewFooterPostfix,
                    Dock = DockStyle.Bottom,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing,

                    // Set height via Size height
                    Size = new Size(1, DataGridViewFooterHeight)
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewDividendOverviewOfYears.DataBindingComplete +=
                    OnDataGridViewDividend_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewDividendOverviewOfYears.MouseEnter +=
                    OnDataGridViewDividendOfYears_MouseEnter;
                // Set row select event
                dataGridViewDividendOverviewOfYears.SelectionChanged +=
                    OnDataGridViewDividendOfYears_SelectionChanged;

                #endregion Events

                #region Style

                DataGridViewHelper.DataGridViewConfiguration(dataGridViewDividendOverviewOfYears);

                dataGridViewDividendsOverviewOfAYearsFooter.ColumnHeadersVisible = false;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewDividendOverviewOfYears);
                newTabPageOverviewYears.Controls.Add(dataGridViewDividendsOverviewOfAYearsFooter);
                dataGridViewDividendOverviewOfYears.Parent = newTabPageOverviewYears;
                ((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls[0]).TabPages.Add(
                    newTabPageOverviewYears);
                newTabPageOverviewYears.Parent =
                    ((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls[0]);

                #endregion Control add

                #endregion Add years overview page

                // Check if dividends exists
                if (ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Count <=
                    0) return;

                // Loop through the years of the dividend payouts
                foreach (
                    var keyName in ShareObjectFinalValue.AllDividendEntries
                        .AllDividendsOfTheShareDictionary.Keys.Reverse()
                )
                {
                    #region Add page

                    // Create TabPage
                    var newTabPage = new TabPage
                    {
                        // Set TabPage name
                        Name = keyName,

                        // Set TabPage caption
                        Text = keyName + @" (" +
                               ShareObjectFinalValue.AllDividendEntries
                                   .AllDividendsOfTheShareDictionary[keyName]
                                   .DgvDividendValueYearAsStrUnit
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Reverse list so the latest is a top of the data grid view
                    var reversDataSource =
                        ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                            .DividendListYear.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();

                    // Create Binding source for the dividend data
                    var bindingSource = new BindingSource
                    {
                        DataSource = reversDataSource
                    };

                    // Create DataGridView
                    var dataGridViewDividendOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with brokerage values to the DataGridView
                        DataSource = bindingSource,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    };

                    // Create DataGridView
                    var dataGridViewDividendsOfAYearFooter = new DataGridView
                    {
                        Name = keyName + _dataGridViewFooterPostfix,
                        Dock = DockStyle.Bottom,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing,

                        // Set height via Size height
                        Size = new Size(1, DataGridViewFooterHeight)
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewDividendOfAYear.DataBindingComplete +=
                        OnDataGridViewDividend_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewDividendOfAYear.MouseEnter +=
                        OnDataGridViewDividendOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewDividendOfAYear.SelectionChanged +=
                        OnDataGridViewDividendOfAYear_SelectionChanged;

                    #endregion Events

                    #region Style

                    DataGridViewHelper.DataGridViewConfiguration(dataGridViewDividendOfAYear);

                    dataGridViewDividendsOverviewOfAYearsFooter.ColumnHeadersVisible = false;

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewDividendOfAYear);
                    newTabPage.Controls.Add(dataGridViewDividendsOfAYearFooter);
                    dataGridViewDividendOfAYear.Parent = newTabPage;
                    ((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls[0]).TabPages
                        .Add(newTabPage);
                    newTabPage.Parent =
                        (TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls[0];

                    #endregion Control add
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelDividend,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/ShowFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewDividend_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Local variables of the DataGridView
                var dgvContent = (DataGridView)sender;

                // Set column headers
                for (var i = 0; i < dgvContent.ColumnCount; i++)
                {
                    // Disable sorting of the columns ( remove sort arrow )
                    dgvContent.Columns[i].SortMode =
                        DataGridViewColumnSortMode.NotSortable;

                    // Get index of the current added column
                    var index = ((DataGridView) ((TabControl) tabCtrlShareDetails
                                .TabPages[TabPageDetailsDividendValueName].Controls[0]).TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name + _dataGridViewFooterPostfix]).Columns
                        .Add(dgvContent.Name + _dataGridViewFooterPostfix, "Header " + i);

                    // Disable sorting of the columns ( remove sort arrow )
                    ((DataGridView) ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName]
                                .Controls[0]).TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name + _dataGridViewFooterPostfix]).Columns[index].SortMode =
                        DataGridViewColumnSortMode.NotSortable;

                    // Set alignment of the column
                    dgvContent.Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            {
                                if (dgvContent.Name == @"Overview")
                                {
                                    dgvContent.Columns[i].HeaderText =
                                        Language.GetLanguageTextByXPath(
                                            @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Year",
                                            LanguageName);
                                }
                            }
                            break;
                        case 1:
                            {
                                if (dgvContent.Name == @"Overview")
                                {
                                    dgvContent.Columns[i].HeaderText =
                                        Language.GetLanguageTextByXPath(
                                            @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Dividend",
                                            LanguageName);
                                }
                                else
                                {
                                    dgvContent.Columns[i].HeaderText =
                                        Language.GetLanguageTextByXPath(
                                            @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Date",
                                            LanguageName);
                                }
                            }
                            break;
                        case 2:
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Dividend",
                                        LanguageName);
                            }
                            break;
                        case 3:
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Volume",
                                        LanguageName);
                            }
                            break;
                        case 4:
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Taxes",
                                        LanguageName);
                            }
                            break;
                        case 5:
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Payout",
                                        LanguageName);
                            }
                            break;
                            case 6:
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Yield",
                                        LanguageName);
                            }
                            break;
                        case 7:
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/DgvDividendOverview/ColHeader_Document",
                                        LanguageName);
                            }
                            break;
                    }
                }

                if (dgvContent.Rows.Count > 0)
                {
                    dgvContent.Rows[0].Selected = false;
                    dgvContent.ScrollBars = ScrollBars.Both;
                }

                //if (dgvContent.Name != @"Overview")
                //    dgvContent.Columns[0].Visible = false;

                if (dgvContent.Name != @"Overview")
                {
                    // Hide first column with the GUID
                    ((DataGridView)((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName]
                                .Controls[0]).TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name]).Columns[0].Visible =
                        false;

                    ((DataGridView)((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName]
                                .Controls[0]).TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name + _dataGridViewFooterPostfix]).Columns[0].Visible =
                        false;

                    #region Column size content

                    // Content DataGridView column width resize
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesDate].Width =
                        YearDateColumnSize;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesVolume].Width =
                        YearVolumeColumnSize;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesDividend].Width =
                        YearDividendColumnSize;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesYield].Width =
                        YearYieldColumnSize;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesPayout].Width =
                        YearPayoutColumnSize;

                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].FillWeight = 10;

                    #endregion Column size content

                    #region Column size footer

                    // Get footer DataGridView
                    var dgvFooter = (DataGridView)((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName]
                            .Controls[0]).TabPages[dgvContent.Name]
                        .Controls[dgvContent.Name + _dataGridViewFooterPostfix];

                    // Footer DataGridView column width resize
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesDate].Width =
                        YearDateColumnSize;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesVolume].Width =
                        YearVolumeColumnSize;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesDividend].Width =
                        YearDividendColumnSize;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesYield].Width =
                        YearYieldColumnSize;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesPayout].Width =
                        YearPayoutColumnSize;

                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].FillWeight = 10;

                    #endregion Column size footer

                    // Get culture info
                    var cultureInfo = ShareObjectFinalValue.AllDividendEntries
                        .AllDividendsOfTheShareDictionary[dgvContent.Name]
                        .DividendCultureInfo;

                    #region Payout

                    var payout =
                        ShareObjectFinalValue.AllDividendEntries
                            .AllDividendsOfTheShareDictionary[dgvContent.Name]
                            .DividendListYear.Sum(x => x.DividendPayoutWithTaxes);

                    // HINT: If any format is changed here it must also be changed in file "DividendObject.cs" at the property "DgvPayoutWithTaxes"
                    var payoutFormatted = payout > 0
                        ? Helper.FormatDecimal(payout, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength,
                            true, @"", cultureInfo)
                        : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, true, Helper.CurrencyTwoFixLength,
                            true, @"", cultureInfo);

                    #endregion Payout

                    // Footer with sum values
                    if (dgvFooter.Rows.Count == 1)
                        dgvFooter.Rows.Add("",
                            Language.GetLanguageTextByXPath(
                                @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/Footer_Totals",
                                SettingsConfiguration.LanguageName),
                            "-", "-", "-", payoutFormatted, "-");

                    // Check if the vertical scrollbar is shown
                    if (dgvContent.RowCount > DataGridViewRowsScrollbar)
                    {
                        dgvFooter.Columns.Add("Scrollbar", "Scrollbar");

                        dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesScrollBar].Width =
                            SystemInformation.VerticalScrollBarWidth;

                        // Disable sorting of the columns ( remove sort arrow )
                        dgvFooter.Columns[dgvFooter.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    DataGridViewHelper.DataGridViewConfigurationFooter(dgvFooter);
                }
                else
                {
                    #region Column size content

                    // Content DataGridView column width resize
                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesYear].Width =
                        OverViewDateColumnSize;

                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesDividendValue].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesDividendValue].FillWeight = 10;

                    #endregion Column size content

                    #region Column size footer

                    // Get footer DataGridView
                    var dgvFooter = (DataGridView)((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName]
                            .Controls[0]).TabPages[dgvContent.Name]
                        .Controls[dgvContent.Name + _dataGridViewFooterPostfix];

                    // Footer DataGridView column width resize
                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesYear].Width =
                        OverViewDateColumnSize;

                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesDividendValue].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesDividendValue].FillWeight = 10;

                    #endregion Column size footer

                    // Get culture info
                    var cultureInfo = ShareObjectFinalValue.AllDividendEntries.DividendCultureInfo;

                    #region Payout

                    var payout =
                        ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues().Sum(x => x.DividendValueYear);

                    // HINT: If any format is changed here it must also be changed in file "DividendsOfAYear.cs" at the property "DividendValueYearAsStrUnit"
                    var payoutFormatted = payout > 0
                            ? Helper.FormatDecimal(payout, Helper.CurrencyTwoLength, true,
                                Helper.CurrencyTwoFixLength, true, @"", cultureInfo)
                            : Helper.FormatDecimal(0, Helper.CurrencyTwoLength, false,
                                Helper.CurrencyTwoFixLength, true, @"", cultureInfo);

                    #endregion Payout

                    // Footer with sum values
                    if (dgvFooter.Rows.Count == 1)
                        dgvFooter.Rows.Add(
                            Language.GetLanguageTextByXPath(
                                @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/Footer_Totals",
                                SettingsConfiguration.LanguageName),
                            payoutFormatted);

                    // Check if the vertical scrollbar is shown
                    if (dgvContent.RowCount > DataGridViewRowsScrollbar)
                    {
                        dgvFooter.Columns.Add("Scrollbar", "Scrollbar");

                        dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesScrollBar].Width =
                            SystemInformation.VerticalScrollBarWidth;

                        // Disable sorting of the columns ( remove sort arrow )
                        dgvFooter.Columns[dgvFooter.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    DataGridViewHelper.DataGridViewConfigurationFooter(dgvFooter);

                }

                // Deselect rows
                //OnDeselectRowsOfDividendDetailsDataGridViews(null);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelDividend,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/RenameHeaderFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        private void OnDeselectRowsOfDividendDetailsDataGridViews(DataGridView dataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage tabPage in tabPgDetailsDividendValues.TabPages)
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
                Helper.ShowExceptionMessage(ex);
            }
        }

        #region Tab control delegates

        /// <summary>
        /// This function sets the focus on the data grid view of the new selected tab page of the tab control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlDividend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabPgDetailsDividendValues.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabPgDetailsDividendValues.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                {
                    view.Focus();

                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                        view.Rows[0].Selected = false;

                    if (view.Name == @"Overview")
                        OnDeselectRowsOfDividendDetailsDataGridViews(null);
                }
            }
        }

        #endregion Tab control delegates

        #region Dividend of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewDividendOfYears_SelectionChanged(object sender, EventArgs args)
        {
            var dgvContent = (DataGridView)sender;

            try
            {
                if (tabPgDetailsDividendValues.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabPgDetailsDividendValues.SelectedTab.Controls.Contains((DataGridView)sender))
                        OnDeselectRowsOfDividendDetailsDataGridViews((DataGridView)sender);
                }

                if (dgvContent.SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = dgvContent.SelectedRows;

                foreach (TabPage tabPage in ((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName]
                    .Controls[0]).TabPages)
                {
                    if (curItem[0].Cells[1].Value != null &&
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    ((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls[0])
                        .SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelDividend,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/SelectionChangeFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);

                OnDeselectRowsOfDividendDetailsDataGridViews(null);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewDividendOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        #endregion Dividend of years

        #region Dividend of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewDividendOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls[0]).TabPages
                    .Count <= 0) return;

                // Deselect row only of the other TabPages DataGridViews
                if (((TabControl)tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls[0])
                    .SelectedTab.Controls.Contains((DataGridView)sender))
                    OnDeselectRowsOfDividendDetailsDataGridViews((DataGridView)sender);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelDividend,
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/SelectionChangeFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application,
                    ex);

                UpdateDividendDetails();
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewDividendOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function opens the dividend document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewDividendOfAYear_CellContentDecimalClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                var iColumnCount = ((DataGridView)sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex != iColumnCount - 1) return;

                // Check if a row is selected
                if (((DataGridView)sender).SelectedRows.Count != 1) return;

                // Get the current selected row
                var curItem = ((DataGridView)sender).SelectedRows;
                // Get Guid of the selected buy item
                var strGuidBuy = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value.ToString() == @"-") return;

                // Get doc from the dividend with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllDividendEntries.GetAllDividendsOfTheShare())
                {
                    // Check if the dividend Guid is the same as the Guid of the clicked dividend item
                    if (temp.Guid != strGuidBuy) continue;

                    // Check if the file still exists
                    if (File.Exists(temp.DocumentAsStr))
                        // Open the file
                        Process.Start(temp.DocumentAsStr);
                    else
                    {
                        var strCaption =
                            Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                                (int)EOwnMessageBoxInfoType.Info];
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
                            strCancel, EOwnMessageBoxInfoType.Info);
                        if (messageBox.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }

                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelDividend,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend_Error/DocumentShowFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Dividend of a year

        #endregion Dividend details
    }
}