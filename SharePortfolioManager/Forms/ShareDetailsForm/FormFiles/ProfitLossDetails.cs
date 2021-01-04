//MIT License
//
//Copyright(c) 2017 - 2021 nessie1980(nessie1980 @gmx.de)
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
//#define DEBUG_PROFIT_LOSS_DETAILS

using SharePortfolioManager.Classes;
using SharePortfolioManager.OwnMessageBoxForm;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager.ShareDetailsForm
{
    public partial class FrmShareDetails
    {
        #region Profit loss details

        /// <summary>
        /// This function updates the profit or loss information of a share
        /// </summary>
        private void UpdateProfitLossDetails()
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
                tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Text =
                    $@"{
                            Language.GetLanguageTextByXPath(
                                @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", SettingsConfiguration.LanguageName)
                        } ({ShareObjectFinalValue.AllSaleEntries.SaleProfitLossBrokerageReductionTotalAsStrUnit})";

                #region Reset tab control

                // Disconnect delegates and remove pages
                tabPgDetailsProfitLossValues.SelectedIndexChanged -= OnTabCtrlProfitLoss_SelectedIndexChanged;
                foreach (TabPage tabPage in tabPgDetailsProfitLossValues.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewProfitLossOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewProfitLossOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewProfitLossOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewProfitLossOfAYear_MouseEnter;
                            dataGridView.MouseLeave -= OnDataGridViewProfitLossOfAYear_MouseEnter;
                            dataGridView.CellContentDoubleClick -=
                                OnDataGridViewProfitLossOfAYear_CellContentDoubleClick;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewProfitLossOfAYear_DataBindingComplete;
                    }

                    tabPage.Controls.Clear();
                    tabPgDetailsProfitLossValues.TabPages.Remove(tabPage);
                }

                tabPgDetailsProfitLossValues.TabPages.Clear();

                #endregion Reset tab control

                #region Add years overview page

                tabPgDetailsProfitLossValues.SelectedIndexChanged += OnTabCtrlProfitLoss_SelectedIndexChanged;

                // Create TabPage for the profit or loss of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Overview",
                               LanguageName) +
                           $@" ({ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotalAsStrUnit})"
                };

                #region Data source, data binding and data grid view

                // Check if profit / loss exists
                if (ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Count <= 0) return;

                // Reverse list so the latest is a top of the data grid view

                // Create Binding source for the profit or loss data
                var reversDataSourceOverview = ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues();
                reversDataSourceOverview.Reverse();

                var bindingSourceOverview = new BindingSource
                {
                    DataSource = reversDataSourceOverview
                };

                // Create DataGridView
                var dataGridViewProfitLossOverviewOfYears = new DataGridView
                {
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with profit or loss values to the DataGridView
                    DataSource = bindingSourceOverview,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewProfitLossOverviewOfYears.DataBindingComplete +=
                    OnDataGridViewProfitLossOfAYear_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewProfitLossOverviewOfYears.MouseEnter += OnDataGridViewProfitLossOfYears_MouseEnter;
                // Set row select event
                dataGridViewProfitLossOverviewOfYears.SelectionChanged +=
                    OnDataGridViewProfitLossOfYears_SelectionChanged;

                #endregion Events

                #region Style

                DataGridViewHelper.DataGridViewConfiguration(dataGridViewProfitLossOverviewOfYears);

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewProfitLossOverviewOfYears);
                dataGridViewProfitLossOverviewOfYears.Parent = newTabPageOverviewYears;
                ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Controls[0]).TabPages.Add(
                    newTabPageOverviewYears);
                newTabPageOverviewYears.Parent =
                    ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Controls[0]);

                #endregion Control add

                #endregion Add page

                // Check if profit / loss exists
                if (ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Count <=
                    0) return;

                // Loop through the years of the profit / loss
                foreach (
                    var keyName in
                    ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Keys.Reverse()
                )
                {
                    #region Add page

                    // Create TabPage
                    var newTabPage = new TabPage
                    {
                        Name = keyName,

                        Text = keyName +
                               @" (" + ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                   .SaleProfitLossYearWithUnitAsStr
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Reverse list so the latest is a top of the data grid view
                    var reversDataSource =
                        ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                            .ProfitLossListYear;
                    reversDataSource.Reverse();

                    // Create Binding source for the profit / loss data
                    var bindingSource = new BindingSource
                    {
                        DataSource = reversDataSource
                    };

                    // Create DataGridView
                    var dataGridViewProfitLossOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with profit / loss values to the DataGridView
                        DataSource = bindingSource,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewProfitLossOfAYear.DataBindingComplete +=
                        OnDataGridViewProfitLossOfAYear_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewProfitLossOfAYear.MouseEnter +=
                        OnDataGridViewProfitLossOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewProfitLossOfAYear.SelectionChanged +=
                        OnDataGridViewProfitLossOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewProfitLossOfAYear.CellContentDoubleClick +=
                        OnDataGridViewProfitLossOfAYear_CellContentDoubleClick;

                    #endregion Events

                    #region Style

                    DataGridViewHelper.DataGridViewConfiguration(dataGridViewProfitLossOfAYear);

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewProfitLossOfAYear);
                    dataGridViewProfitLossOfAYear.Parent = newTabPage;
                    ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Controls[0]).TabPages
                        .Add(newTabPage);
                    newTabPage.Parent =
                        ((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Controls[0]);

                    #endregion Control add
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelProfitLoss,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss_Error/ShowFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function deselects the select row after the data binding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewProfitLossOfAYear_DataBindingComplete(object sender,
            DataGridViewBindingCompleteEventArgs e)
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
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Year",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Date",
                                        LanguageName);
                            }

                            break;
                        }
                        case 1:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Volume",
                                    LanguageName);
                            break;
                        case 2:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_ProfitLoss",
                                    LanguageName);
                            break;
                        case 3:
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_ProfitLoss",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/DgvProfitLossOverview/ColHeader_Document",
                                        LanguageName)
                                    ;
                            }

                            break;
                    }
                }

                if (((DataGridView) sender).Rows.Count > 0)
                {
                    ((DataGridView) sender).Rows[0].Selected = false;
                    ((DataGridView) sender).ScrollBars = ScrollBars.Both;
                }

                if (((DataGridView) sender).Name == @"Overview")
                    ((DataGridView) sender).Columns[2].Visible = false;

                // Deselect rows
                OnDeselectRowsOfProfitLossDetailsDataGridViews(null);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelProfitLoss,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss_Error/RenameHeaderFailed",
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
        private void OnDeselectRowsOfProfitLossDetailsDataGridViews(DataGridView dataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage tabPage in tabPgDetailsProfitLossValues.TabPages)
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
        private void OnTabCtrlProfitLoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabPgDetailsProfitLossValues.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabPgDetailsProfitLossValues.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                {
                    view.Focus();

                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                        view.Rows[0].Selected = false;

                    if (view.Name == @"Overview")
                        OnDeselectRowsOfProfitLossDetailsDataGridViews(null);
                }
            }
        }

        #endregion Tab control delegates

        #region Profit / loss of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewProfitLossOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView) sender).SelectedRows;

                foreach (TabPage tabPage in tabPgDetailsProfitLossValues.TabPages)
                {
                    if (tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    tabPgDetailsProfitLossValues.SelectTab(tabPage);
                    tabPage.Focus();

                    // Deselect rows
                    OnDeselectRowsOfProfitLossDetailsDataGridViews(null);

                    break;
                }
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                OnDeselectRowsOfProfitLossDetailsDataGridViews(null);

                Helper.AddStatusMessage(_toolStripStatusLabelProfitLoss,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss_Error/SelectionChangeFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewProfitLossOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        #endregion Profit / loss of years

        #region Profit / loss of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewProfitLossOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((TabControl) tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Controls[0]).TabPages
                    .Count <= 0) return;

                // Deselect row only of the other TabPages DataGridViews
                if (tabPgDetailsProfitLossValues.SelectedTab.Controls.Contains((DataGridView) sender))
                    OnDeselectRowsOfProfitLossDetailsDataGridViews((DataGridView) sender);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(_toolStripStatusLabelProfitLoss,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss_Error/SelectionChangeFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);

                UpdateProfitLossDetails();
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewProfitLossOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function opens the sale document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewProfitLossOfAYear_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                // Get date and time of the selected buy item
                var strDateTime = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value.ToString() == @"-") return;

                // Get doc from the profit or loss with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllSaleEntries.GetAllProfitLossTotalValues())
                {
                    // Check if the buy date and time is the same as the date and time of the clicked buy item
                    if (temp.DateAsStr != strDateTime) continue;

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
                                @"/MessageBoxForm/Content/DocumentDoesNotExist",
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
                Helper.AddStatusMessage(_toolStripStatusLabelProfitLoss,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss_Error/DocumentShowFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Profit / loss of a year

        #endregion Profit / loss details
    }
}