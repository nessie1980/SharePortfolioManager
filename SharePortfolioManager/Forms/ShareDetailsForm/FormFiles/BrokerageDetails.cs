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
//#define DEBUG_BROKERAGE_DETAILS

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

        #region Brokerage details

        /// <summary>
        /// This function updates the brokerage information of a share
        /// </summary>
        private void UpdateBrokerageDetails()
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
                tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Text =
                    $@"{
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Caption", SettingsConfiguration.LanguageName)
                        } ({ShareObjectFinalValue.AllBrokerageEntries.BrokerageValueTotalAsStrUnit})";

                #region Reset tab control

                // Disconnect delegates and remove pages
                tabPgDetailsBrokerageValues.SelectedIndexChanged -= OnTabCtrlBrokerage_SelectedIndexChanged;
                foreach (TabPage tabPage in tabPgDetailsBrokerageValues.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBrokerageOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBrokerageOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBrokerageOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBrokerageOfAYear_MouseEnter;
                            dataGridView.MouseLeave -= OnDataGridViewBrokerageOfAYear_MouseEnter;
                            dataGridView.CellContentDoubleClick -=
                                OnDataGridViewBrokerageOfAYear_CellContentDecimalClick;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewBrokerage_DataBindingComplete;
                    }

                    tabPage.Controls.Clear();
                    tabPgDetailsBrokerageValues.TabPages.Remove(tabPage);
                }

                tabPgDetailsBrokerageValues.TabPages.Clear();

                #endregion Reset tab control

                #region Add years overview page

                tabPgDetailsBrokerageValues.SelectedIndexChanged += OnTabCtrlBrokerage_SelectedIndexChanged;

                // Create TabPage for the brokerage of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Overview",
                               LanguageName) +
                           $@" ({ShareObjectFinalValue.AllBrokerageEntries.BrokerageValueTotalAsStrUnit})"
                };

                #region Data source, data binding and data grid view

                // Check if brokerage exists
                if (ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues().Count <= 0) return;

                // Reverse list so the latest is a top of the data grid view
                var reversDataSourceOverview = ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues();
                reversDataSourceOverview.Reverse();

                var bindingSourceOverview = new BindingSource
                {
                    DataSource = reversDataSourceOverview
                };

                // Create DataGridView
                var dataGridViewBrokerageOverviewOfAYears = new DataGridView
                {
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with brokerage values to the DataGridView
                    DataSource = bindingSourceOverview,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewBrokerageOverviewOfAYears.DataBindingComplete +=
                    OnDataGridViewBrokerage_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewBrokerageOverviewOfAYears.MouseEnter += OnDataGridViewBrokerageOfYears_MouseEnter;
                // Set row select event
                dataGridViewBrokerageOverviewOfAYears.SelectionChanged +=
                    OnDataGridViewBrokerageOfYears_SelectionChanged;

                #endregion Events

                #region Style

                DataGridViewHelper.DataGridViewConfiguration(dataGridViewBrokerageOverviewOfAYears);

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewBrokerageOverviewOfAYears);
                dataGridViewBrokerageOverviewOfAYears.Parent = newTabPageOverviewYears;
                ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls[0]).TabPages.Add(
                    newTabPageOverviewYears);
                newTabPageOverviewYears.Parent =
                    ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls[0]);

                #endregion Control add

                #endregion Add page

                // Check if brokerage exists
                if (ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary.Count <=
                    0) return;

                // Loop through the years of the brokerage pays
                foreach (
                    var keyName in ShareObjectFinalValue.AllBrokerageEntries
                        .AllBrokerageReductionOfTheShareDictionary.Keys.Reverse()
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
                               ShareObjectFinalValue.AllBrokerageEntries
                                   .AllBrokerageReductionOfTheShareDictionary[keyName]
                                   .BrokerageValueYearAsStrUnit
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Reverse list so the latest is a top of the data grid view
                    var reversDataSource =
                        ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName]
                            .BrokerageReductionListYear;
                    reversDataSource.Reverse();

                    // Create Binding source for the brokerage data
                    var bindingSource = new BindingSource
                    {
                        DataSource = reversDataSource
                    };

                    // Create DataGridView
                    var dataGridViewBrokerageOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with brokerage values to the DataGridView
                        DataSource = bindingSource,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewBrokerageOfAYear.DataBindingComplete +=
                        OnDataGridViewBrokerage_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewBrokerageOfAYear.MouseEnter +=
                        OnDataGridViewBrokerageOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewBrokerageOfAYear.SelectionChanged +=
                        OnDataGridViewBrokerageOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewBrokerageOfAYear.CellContentDoubleClick +=
                        OnDataGridViewBrokerageOfAYear_CellContentDecimalClick;

                    #endregion Events

                    #region Style

                    DataGridViewHelper.DataGridViewConfiguration(dataGridViewBrokerageOfAYear);

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewBrokerageOfAYear);
                    dataGridViewBrokerageOfAYear.Parent = newTabPage;
                    ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls[0]).TabPages
                        .Add(newTabPage);
                    newTabPage.Parent =
                        ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls[0]);

                    #endregion Control add
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelBrokerage,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage_Error/ShowFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewBrokerage_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (var i = 0; i < ((DataGridView) sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView) sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    // Disable sorting of the columns ( remove sort arrow )
                    ((DataGridView) sender).Columns[i].SortMode =
                        DataGridViewColumnSortMode.NotSortable;

                    switch (i)
                    {
                        case 0:
                        {
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Year",
                                        LanguageName);
                            }
                        }
                            break;
                        case 1:
                        {
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Brokerage",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Date",
                                        LanguageName);
                            }
                        }
                            break;
                        case 2:
                        {
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Brokerage",
                                    LanguageName);
                        }
                            break;
                        case 3:
                        {
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Reduction",
                                    LanguageName);
                        }
                            break;
                        case 4:
                        {
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_BrokerageReduction",
                                    LanguageName);
                        }
                            break;
                        case 5:
                        {
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/DgvBrokerageOverview/ColHeader_Document",
                                    LanguageName);
                        }
                            break;
                    }
                }

                if (((DataGridView) sender).Rows.Count > 0)
                {
                    ((DataGridView) sender).Rows[0].Selected = false;
                    ((DataGridView) sender).ScrollBars = ScrollBars.Both;
                }

                if (((DataGridView) sender).Name != @"Overview")
                    ((DataGridView) sender).Columns[0].Visible = false;

                // Deselect rows
                OnDeselectRowsOfBrokerageDetailsDataGridViews(null);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelBrokerage,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage_Error/RenameHeaderFailed",
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
        private void OnDeselectRowsOfBrokerageDetailsDataGridViews(DataGridView dataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage tabPage in tabPgDetailsBrokerageValues.TabPages)
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
        private void OnTabCtrlBrokerage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabPgDetailsBrokerageValues.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabPgDetailsBrokerageValues.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                {
                    view.Focus();

                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                        view.Rows[0].Selected = false;

                    if (view.Name == @"Overview")
                        OnDeselectRowsOfBrokerageDetailsDataGridViews(null);
                }
            }
        }

        #endregion Tab control delegates

        #region Brokerage of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewBrokerageOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabPgDetailsBrokerageValues.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabPgDetailsBrokerageValues.SelectedTab.Controls.Contains((DataGridView) sender))
                        OnDeselectRowsOfBrokerageDetailsDataGridViews((DataGridView) sender);
                }

                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView) sender).SelectedRows;

                foreach (TabPage tabPage in ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName]
                    .Controls[0]).TabPages)
                {
                    if (curItem[0].Cells[1].Value != null &&
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls[0])
                        .SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelBrokerage,
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage_Error/SelectionChangeFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);

                OnDeselectRowsOfBrokerageDetailsDataGridViews(null);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewBrokerageOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
        }

        #endregion Brokerage of years

        #region Brokerage of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewBrokerageOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls[0]).TabPages
                    .Count <= 0) return;

                // Deselect row only of the other TabPages DataGridViews
                if (((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls[0])
                    .SelectedTab.Controls.Contains((DataGridView) sender))
                    OnDeselectRowsOfBrokerageDetailsDataGridViews((DataGridView) sender);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelBrokerage,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage_Error/SelectionChangeFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);

                UpdateBrokerageDetails();
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewBrokerageOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
        }

        /// <summary>
        /// This function opens the brokerage document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewBrokerageOfAYear_CellContentDecimalClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                var iColumnCount = ((DataGridView) sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex != iColumnCount - 1) return;

                // Check if a row is selected
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the current selected row
                var curItem = ((DataGridView) sender).SelectedRows;
                // Get Guid of the selected buy item
                var strGuidBuy = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value.ToString() == @"-") return;

                // Get doc from the brokerage with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageOfTheShare())
                {
                    // Check if the brokerage Guid is the same as the Guid of the clicked brokerage item
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
                Helper.AddStatusMessage(_toolStripStatusLabelBrokerage,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage_Error/DocumentShowFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Brokerage of a year

        #endregion Brokerage details
    }
}