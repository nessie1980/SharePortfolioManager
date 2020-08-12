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
//#define DEBUG_MAIN_FRM_PORTFOLIO_DGV

using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Properties;
using SharePortfolioManager.ShareDetailsForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharePortfolioManager.ChartForm;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Column enums for dgvPortfolioMarketValue and dgvPortfolioFooterMarketValue

        private enum ColumnIndicesPortfolioMarketValue
        {
            EWknColumnIndex = 0,
            ENameColumnIndex,
            EVolumeColumnIndex,
            EPriceColumnIndex,
            EPerformancePrevDayColumnIndex,
            EPerformanceColumnIndex,
            EMarketValueColumnIndex,
            ECompletePerformanceColumnIndex,
            ECompleteMarketValueColumnIndex
        }

        private enum ColumnIndicesPortfolioFooterMarketValue
        {
            ELabelTotalColumnIndex = 0,
            EPerformanceColumnIndex,
            EMarketValueColumnIndex,
            ECompletePerformanceColumnIndex,
            ECompleteMarketValueColumnIndex
        }

        #endregion Column enums for dgvPortfolioMarketValue and dgvPortfolioFooterMarketValue

        #region Column enums for dgvPortfolioFinalValue and dgvPortfolioFooterFinalValue

        private enum ColumnIndicesPortfolioFinalValue
        {
            EWknColumnIndex = 0,
            ENameColumnIndex,
            EVolumeColumnIndex,
            EBrokerageDividendColumnIndex,
            EPriceColumnIndex,
            EPerformancePrevDayColumnIndex,
            EPerformanceColumnIndex,
            EFinalValueColumnIndex,
            ECompletePerformanceColumnIndex,
            ECompleteFinalValueColumnIndex
        }

        private enum ColumnIndicesPortfolioFooterFinalValue
        {
            ELabelBrokerageDividendIndex = 0,
            EBrokerageDividendIndex,
            ELabelTotalColumnIndex,
            EPerformanceColumnIndex,
            EFinalValueColumnIndex,
            ECompletePerformanceColumnIndex,
            ECompleteFinalValueColumnIndex
        }

        #endregion Column enums for dgvPortfolioFinalValue and dgvPortfolioFooterFinalValue

        #region Variables

        #region Column width for the dgvPortfolio and dgvPortfolioFooter

        private const int WknColumnSize = 80;
        private const int VolumeColumnSize = 100;
        private const int BrokerageDividendColumnSize = 100;
        private const int PriceColumnSize = 100;
        private const int PerformancePrevDayColumnSize = 90;
        private const int PerformanceColumnSize = 125;
        private const int MarketFinalValueColumnSize = 130;
        private const int CompletePerformanceColumnSize = 125;
        private const int CompleteMarketFinalValueColumnSize = 130;

        #endregion Column width for the dgvPortfolio and dgvPortfolioFooter

        private const int FooterBorderWidth = 2;

        #region Data grid view row click or double click detection

        /// <summary>
        /// Stores that a data grid view row is clicked the first time
        /// </summary>
        private bool _isFirstCellClick = true;

        /// <summary>
        /// Stores the index of the first clicked data grid view row
        /// </summary>
        private int _isFirstRowIndex = -1;

        /// <summary>
        /// Stores that a data grid view row is clicked the second time
        /// </summary>
        private bool _isSecondCellClick;

        /// <summary>
        /// Stores the index of the second clicked data grid view row
        /// </summary>
        private int _isDoubleRowIndex = -1;

        /// <summary>
        /// Stores the time between the first and second data grid view row click
        /// </summary>
        private int _checkDoubleClickMilliSeconds;

        #endregion Data grid view row click or double click detection

        #endregion Variables

        #region Properties

        public FrmShareDetails FrmShareDetailsForm;

        public BindingSource DgvPortfolioBindingSourceMarketValue { get; } = new BindingSource();

        public BindingSource DgvPortfolioBindingSourceFinalValue { get; } = new BindingSource();

        public List<Image> ImageList { get; } = new List<Image>
        {
            Resources.empty_arrow,
            Resources.negativ_development_24,
            Resources.neutral_development_24,
            Resources.positiv_development_24
        };

        #endregion Properties

        #region Methods

        #region TabControl for the data grid views

        private void tabCtrlShareOverviews_SelectedIndexChanged(object sender, EventArgs e)
        {
            MarketValueOverviewTabSelected = tabCtrlShareOverviews.SelectedIndex != 0;
        }

        #endregion TabControl for the data grid view

        #region Data grid view portfolio and portfolio footer selection changed

        /// <summary>
        /// This function loads the selected share to the share details
        /// when the selection in the data gird view final value has been changed
        /// </summary>
        /// <param name="sender">Selected item</param>
        /// <param name="e">EventArgs</param>
        private void DgvPortfolioMarketValue_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Save current selected ShareObject via the selected WKN number
                if (dgvPortfolioMarketValue.SelectedRows.Count != 1 ||
                    dgvPortfolioMarketValue.SelectedRows[0].Cells.Count <= 0 ||
                    dgvPortfolioMarketValue.SelectedRows[0]
                        .Cells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex].Value == null ||
                    AddFlagMarketValue) return;

                // Get selected WKN number
                var selectedWknNumber = dgvPortfolioMarketValue.SelectedRows[0]
                    .Cells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex].Value.ToString();

                // Search for the selected ShareObject in the market value list
                foreach (var shareObjectMarketValue in ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.Wkn != selectedWknNumber) continue;

                    ShareObjectMarketValue = shareObjectMarketValue;
                    break;
                }

                // Search for the selected ShareObject in the final value list
                foreach (var shareObjectFinalValue in ShareObjectListFinalValue)
                {
                    if (shareObjectFinalValue.Wkn != selectedWknNumber) continue;

                    ShareObjectFinalValue = shareObjectFinalValue;
                    break;
                }

                if (ShareObjectMarketValue == null || ShareObjectFinalValue == null) return;

                // Check if the "Update" button should be disabled or enabled
                if (!UpdateAllFlag)
                    btnRefresh.Enabled = ShareObjectMarketValue.DoInternetUpdate &&
                                         ShareObjectMarketValue.WebSiteConfigurationFound;
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function loads the selected share to the share details
        /// when the selection in the data grid view market value has been changed
        /// </summary>
        /// <param name="sender">Selected item</param>
        /// <param name="e">EventArgs</param>
        private void DgvPortfolioFinalValue_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Save current selected ShareObject via the selected WKN number
                if (dgvPortfolioFinalValue.SelectedRows.Count != 1 ||
                    dgvPortfolioFinalValue.SelectedRows[0].Cells.Count <= 0 ||
                    dgvPortfolioFinalValue.SelectedRows[0].Cells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]
                        .Value == null || AddFlagFinalValue) return;

                // Get selected WKN number
                var selectedWknNumber = dgvPortfolioFinalValue.SelectedRows[0]
                    .Cells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex].Value.ToString();

                // Search for the selected ShareObject in the final value list
                foreach (var shareObjectFinalValue in ShareObjectListFinalValue)
                {
                    if (shareObjectFinalValue.Wkn != selectedWknNumber) continue;

                    ShareObjectFinalValue = shareObjectFinalValue;
                    break;
                }

                // Search for the selected ShareObject in the market value list
                foreach (var shareObjectMarketValue in ShareObjectListMarketValue)
                {
                    if (shareObjectMarketValue.Wkn != selectedWknNumber) continue;

                    ShareObjectMarketValue = shareObjectMarketValue;
                    break;
                }

                if (ShareObjectFinalValue == null || ShareObjectListMarketValue == null) return;

                // Check if the "Update" button should be disabled or enabled
                if (!UpdateAllFlag)
                    btnRefresh.Enabled = ShareObjectFinalValue.DoInternetUpdate &&
                                         ShareObjectFinalValue.WebSiteConfigurationFound;
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// Function which deselects the first row of the data grid view market value footer
        /// </summary>
        /// <param name="sender">Data grid view for the market value footer</param>
        /// <param name="e">EventArgs</param>
        private void DgvPortfolioFooterMarketValue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPortfolioFooterMarketValue.SelectedRows.Count > 0)
                dgvPortfolioFooterMarketValue.SelectedRows[0].Selected = false;
        }

        /// <summary>
        /// Function which deselects the first row of the data gird view final footer
        /// </summary>
        /// <param name="sender">Data grid view for the final value footer</param>
        /// <param name="e">EventArgs</param>
        private void DgvPortfolioFooterFinalValue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPortfolioFooterFinalValue.SelectedRows.Count > 0)
                dgvPortfolioFooterFinalValue.SelectedRows[0].Selected = false;
        }

        #endregion Data grid view portfolio and portfolio footer selection changed

        #region Data grid view portfolio and portfolio footer resize

        /// <summary>
        /// This function resizes the columns when the
        /// data grid view market value has been resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvPortfolioMarketValue_Resize(object sender, EventArgs e)
        {
            OnResizeDataGridView();
        }

        /// <summary>
        /// This function resizes the columns when the
        /// data grid view market value footer has been resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvPortfolioFooterMarketValue_Resize(object sender, EventArgs e)
        {
            OnResizeDataGridView();
        }

        /// <summary>
        /// This function resizes the columns when the
        /// data grid view final value has been resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvPortfolioFinalValue_Resize(object sender, EventArgs e)
        {
            OnResizeDataGridView();
        }

        /// <summary>
        /// This function resizes the columns when the
        /// data grid view final value footer has been resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvPortfolioFooterFinalValue_Resize(object sender, EventArgs e)
        {
            OnResizeDataGridView();
        }

        /// <summary>
        /// This function does a column resize of the data grid views and data grid views footer
        /// </summary>
        private void OnResizeDataGridView()
        {
            try
            {
                #region dgvPortfolioFooterMarketValue

                VScrollBar vScrollbarPortfolio;

                if (dgvPortfolioFooterMarketValue.ColumnCount >= 3)
                {
                    vScrollbarPortfolio = dgvPortfolioMarketValue.Controls.OfType<VScrollBar>().First();
                    if (vScrollbarPortfolio.Visible)
                    {
                        var vScrollbarWidth = vScrollbarPortfolio.Width;
                        dgvPortfolioFooterMarketValue
                                .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.EMarketValueColumnIndex].Width =
                            MarketFinalValueColumnSize + vScrollbarWidth;
                    }
                    else
                    {
                        dgvPortfolioFooterMarketValue
                                .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.EMarketValueColumnIndex].Width =
                            MarketFinalValueColumnSize;
                    }
                }

                #endregion dgvPortfolioFooterFinalValue

                #region dgvPortfolioFooterFinalValue

                if (dgvPortfolioFooterFinalValue.ColumnCount < 5) return;

                const int columnLabelTotalWidth = PriceColumnSize + PerformancePrevDayColumnSize;

                vScrollbarPortfolio = dgvPortfolioFinalValue.Controls.OfType<VScrollBar>().First();
                if (vScrollbarPortfolio.Visible)
                {
                    var vScrollbarWidth = vScrollbarPortfolio.Width;
                    dgvPortfolioFooterFinalValue
                            .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EFinalValueColumnIndex].Width =
                        MarketFinalValueColumnSize + vScrollbarWidth;
                }
                else
                {
                    dgvPortfolioFooterFinalValue
                            .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EFinalValueColumnIndex].Width =
                        MarketFinalValueColumnSize;
                }

                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EBrokerageDividendIndex].Width =
                    BrokerageDividendColumnSize;
                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Width =
                    columnLabelTotalWidth;

                #endregion dgvPortfolioFooterFinalValue
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/Errors/ResizeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion DataGridView portfolio and portfolio footer resize

        #region Add shares to data grid view portfolio and configure data grid view footer

        /// <summary>
        /// This function add the shares to the DataGridView portfolio.
        /// </summary>
        private void AddSharesToDataGridViews()
        {
            if (!InitFlag) return;

            try
            {
                // Check if the share object list has items
                TextAndImageColumn textImageCol;
                DataGridViewTextBoxColumn textColumn;

                #region dgvPortfolioMarketValue

                if (ShareObjectListMarketValue != null && ShareObjectListMarketValue.Count > 0)
                {
                    dgvPortfolioMarketValue.Columns.Clear();

                    dgvPortfolioMarketValue.AutoGenerateColumns = false;

                    #region Add datagridview columns

                    // Wkn
                    textImageCol = new TextAndImageColumn
                    {
                        DataPropertyName = "DgvWkn",
                        Name = "Wkn",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textImageCol);
                    // Name
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvNameAsStr",
                        Name = "Name",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textColumn);
                    // Volume
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvVolumeAsStr",
                        Name = "Volume",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textColumn);
                    // CurPrevPrice
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvCurPrevPriceAsStrUnit",
                        Name = "CurPrevPrice",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textColumn);
                    // PrevDayPerformance
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvPrevDayDifferencePerformanceAsStrUnit",
                        Name = "PrevDayPerformance",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textColumn);
                    // ProfitLossPerformanceFinalValue
                    textImageCol = new TextAndImageColumn
                    {
                        DataPropertyName = "DgvProfitLossPerformanceValueAsStrUnit",
                        Name = "ProfitLossPerformanceMarketValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textImageCol);
                    // PurchaseValueFinalValue
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvPurchaseValueFinalValueAsStrUnit",
                        Name = "PurchaseValueMarketValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textColumn);
                    // Complete profitLossPerformanceFinalValue
                    textImageCol = new TextAndImageColumn
                    {
                        DataPropertyName = "DgvCompleteProfitLossPerformanceValueAsStrUnit",
                        Name = "CompleteProfitLossPerformanceMarketValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textImageCol);
                    // Complete purchaseValueFinalValue
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvCompletePurchaseValueMarketValueAsStrUnit",
                        Name = "CompletePurchaseValueMarketValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioMarketValue.Columns.Add(textColumn);

                    #endregion Add datagridview columns

                    DgvPortfolioBindingSourceMarketValue.ResetBindings(false);
                    DgvPortfolioBindingSourceMarketValue.DataSource = ShareObjectListMarketValue;
                    dgvPortfolioMarketValue.DataSource = DgvPortfolioBindingSourceMarketValue;
                }

                #endregion dgvPortfolioMarketValue

                #region dgvPortfolioFinalValue

                // Check if the share object list has items
                if (ShareObjectListFinalValue != null && ShareObjectListFinalValue.Count > 0)
                {
                    dgvPortfolioFinalValue.Columns.Clear();

                    dgvPortfolioFinalValue.AutoGenerateColumns = false;

                    #region Add datagridview columns

                    // Wkn
                    textImageCol = new TextAndImageColumn
                    {
                        DataPropertyName = "DgvWkn",
                        Name = "Wkn",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textImageCol);
                    // Name
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvNameAsStr",
                        Name = "Name",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textColumn);
                    // Volume
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvVolumeAsStr",
                        Name = "Volume",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textColumn);
                    // BrokerageDividendWithTaxes
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvBrokerageDividendWithTaxesAsStrUnit",
                        Name = "BrokerageDividendTotal",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textColumn);
                    // CurPrevPrice
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvCurPrevPriceAsStrUnit",
                        Name = "CurPrevPrice",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textColumn);
                    // PrevDayPerformance
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvPrevDayDifferencePerformanceAsStrUnit",
                        Name = "PrevDayPerformance",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textColumn);
                    // ProfitLossPerformanceFinalValue
                    textImageCol = new TextAndImageColumn
                    {
                        DataPropertyName = "DgvProfitLossPerformanceValueAsStrUnit",
                        Name = "ProfitLossPerformanceFinalValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textImageCol);
                    // PurchaseValueFinalValue
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvPurchaseValueFinalValueAsStrUnit",
                        Name = "PurchaseValueFinalValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textColumn);
                    // Complete profitLossPerformanceFinalValue
                    textImageCol = new TextAndImageColumn
                    {
                        DataPropertyName = "DgvCompleteProfitLossPerformanceValueAsStrUnit",
                        Name = "CompleteProfitLossPerformanceFinalValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textImageCol);
                    // Complete purchaseValueFinalValue
                    textColumn = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "DgvCompletePurchaseValueMarketValueAsStrUnit",
                        Name = "CompletePurchaseValueFinalValue",
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    dgvPortfolioFinalValue.Columns.Add(textColumn);

                    #endregion Add datagridview columns

                    DgvPortfolioBindingSourceFinalValue.ResetBindings(false);
                    DgvPortfolioBindingSourceFinalValue.DataSource = ShareObjectListFinalValue;
                    dgvPortfolioFinalValue.DataSource = DgvPortfolioBindingSourceFinalValue;

                }

                #endregion dgvPortfolioFinalValue
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/DisplayFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function configures and adds the footer rows in the DataGridView portfolio footer.
        /// </summary>
        private void AddShareFooters()
        {
            if (!InitFlag) return;

            try
            {
                // Clear footers
                dgvPortfolioFooterMarketValue.Rows.Clear();
                dgvPortfolioFooterFinalValue.Rows.Clear();

                #region dgvPortfolioFooterMarketValue

                // Check if the share object list has items
                if (ShareObjectListMarketValue != null && ShareObject.ObjectCounter > 0 &&
                    dgvPortfolioMarketValue.Rows.Count > 0)
                {
                    // Set row colors
                    if (dgvPortfolioMarketValue.RowsDefaultCellStyle.BackColor == Color.LightGray &&
                        dgvPortfolioMarketValue.RowCount % 2 == 0)
                    {
                        dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                        dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    }
                    else
                    {
                        dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.White;
                        dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    }

                    // Create row content with final value of the market value portfolio
                    var newFooterPurchaseValueOfAllShares = new object[]
                    {
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalPurchaseValue",
                                    LanguageName)
                            }",
                        @"",
                        ShareObjectListMarketValue[0].PortfolioPurchaseValueAsStrUnit,
                        @"",
                        ShareObjectListMarketValue[0].PortfolioCompletePurchaseValueAsStrUnit
                    };

                    // Create row content with total performance of the market value portfolio
                    var newFooterDepotDevelopment = new object[]
                    {
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalPerformance",
                                    LanguageName)
                            }",
                        ShareObjectListMarketValue[0].PortfolioProfitLossPerformanceValueAsStr,
                        @"",
                        ShareObjectListMarketValue[0].PortfolioCompleteProfitLossPerformanceAsStrUnit,
                        @""
                    };

                    // Create row content with total market value of the market value portfolio
                    var newFooterTotalDepotVolume = new object[]
                    {
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalDepotValue",
                                    LanguageName)
                            }",
                        @"",
                        ShareObjectListMarketValue[0].PortfolioMarketValueAsStrUnit,
                        @"",
                        ShareObjectListMarketValue[0].PortfolioCompleteMarketValueWithProfitLossAsStrUnit
                    };

                    // Add rows to the data grid view market value footer
                    dgvPortfolioFooterMarketValue.Rows.Add(newFooterPurchaseValueOfAllShares);
                    dgvPortfolioFooterMarketValue.Rows.Add(newFooterDepotDevelopment);
                    dgvPortfolioFooterMarketValue.Rows.Add(newFooterTotalDepotVolume);

                    // Add padding for centralize the content vertical
                    dgvPortfolioFooterMarketValue.Rows[dgvPortfolioFooterMarketValue.Rows.Count - 1].DefaultCellStyle
                        .Padding = new Padding(0, FooterBorderWidth, 0, 0);
                }

                #endregion dgvPortfolioFooterMarketValue

                #region dgvPortfolioFooterFinalValue

                // Check if the share object list has items
                if (ShareObjectListFinalValue == null || ShareObject.ObjectCounter <= 0 ||
                    dgvPortfolioFinalValue.Rows.Count <= 0) return;

                {
                    // Set row colors
                    if (dgvPortfolioFinalValue.RowsDefaultCellStyle.BackColor == Color.LightGray &&
                        dgvPortfolioFinalValue.RowCount % 2 == 0)
                    {
                        dgvPortfolioFooterFinalValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                        dgvPortfolioFooterFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    }
                    else
                    {
                        dgvPortfolioFooterFinalValue.RowsDefaultCellStyle.BackColor = Color.White;
                        dgvPortfolioFooterFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    }

                    // Create row content with final value of the final value portfolio
                    var newFooterPurchaseFinalOfAllShares = new object[]
                    {
                        @"",
                        @"",
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPurchaseValue",
                                    LanguageName)
                            }",
                        @"",
                        ShareObjectListFinalValue[0].PortfolioPurchaseValueAsStrUnit,
                        @"",
                        ShareObjectListFinalValue[0].PortfolioCompletePurchaseValueAsStrUnit
                    };

                    // Create row content with total performance of the final value portfolio
                    var newFooterDepotDevelopment = new object[]
                    {
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalBrokerageDividend",
                                    LanguageName)
                            }",
                        ShareObjectListFinalValue[0].PortfolioCompleteBrokerageDividendsAsStrUnit,
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPerformance",
                                    LanguageName)
                            }",
                        ShareObjectListFinalValue[0].PortfolioProfitLossPerformanceAsStrUnit,
                        @"",
                        ShareObjectListFinalValue[0].PortfolioCompleteProfitLossPerformanceAsStrUnit,
                        @""
                    };

                    // Create row content with total market value of the final value portfolio
                    var newFooterTotalDepotVolume = new object[]
                    {
                        @"",
                        @"",
                        $@"{
                                Language.GetLanguageTextByXPath(
                                    @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalDepotValue",
                                    LanguageName)
                            }",
                        @"",
                        ShareObjectListFinalValue[0].PortfolioFinalValueAsStrUnit,
                        @"",
                        ShareObjectListFinalValue[0].PortfolioCompleteFinalValueAsStrUnit
                    };

                    // Add rows to the data grid view final value footer
                    dgvPortfolioFooterFinalValue.Rows.Add(newFooterPurchaseFinalOfAllShares);
                    dgvPortfolioFooterFinalValue.Rows.Add(newFooterDepotDevelopment);
                    dgvPortfolioFooterFinalValue.Rows.Add(newFooterTotalDepotVolume);

                    // Add padding for centralize the content vertical
                    dgvPortfolioFooterFinalValue.Rows[dgvPortfolioFooterFinalValue.Rows.Count - 1].DefaultCellStyle
                        .Padding = new Padding(0, FooterBorderWidth, 0, 0);
                }

                #endregion dgvPortfolioFooterFinalValue
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolioFooter_Error/ConfigurationFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function adds the image if a share will be updated
        /// and also the image if the share performance is positive, neutral or negative.
        /// </summary>
        private void OnDgvPortfolioMarketValue_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Only update if all shares have been loaded
            if (dgvPortfolioMarketValue.RowCount != ShareObjectListMarketValue.Count)
                return;

            OnDgvPortfolioMarketValueImageUpdate();
        }

        /// <summary>
        /// This function adds the image if a share will be updated
        /// and also the image if the share performance is positive, neutral or negative.
        /// </summary>
        private void OnDgvPortfolioFinalValue_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Only update if all shares have been loaded
            if (dgvPortfolioFinalValue.RowCount != ShareObjectListFinalValue.Count)
                return;

            OnDgvPortfolioFinalValueImageUpdate();
        }

        #endregion Add shares to data grid view portfolio and configure data grid view footer

        #region Update images in the data grid views

        /// <summary>
        /// This function updates the images of the currently selected row of the market value data grid view
        /// </summary>
        /// <param name="givenRowIndex">Row index of the selected row. If no row index is given all rows will be updated.</param>
        private void OnDgvPortfolioMarketValueImageUpdate(int givenRowIndex = -1)
        {
            var startIndex = 0;

            // Check if a row index is given so set this as start index
            if (givenRowIndex > -1)
                startIndex = givenRowIndex;

            for (var rowIndex = startIndex; rowIndex < dgvPortfolioFinalValue.RowCount; rowIndex++)
            {
                // Return if not all columns have been added
                if ((dgvPortfolioMarketValue.Rows[rowIndex]
                        .Cells.Count - 1) != (int) ColumnIndicesPortfolioMarketValue.ECompleteMarketValueColumnIndex)
                    continue;

                // Get WKN of the current row
                var wkn = dgvPortfolioMarketValue.Rows[rowIndex]
                    .Cells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex].Value.ToString();

                // Get share object of the current WKN
                var shareObject = ShareObjectListMarketValue.Find(x => (x.Wkn == wkn));

                // Set internet update image
                ((TextAndImageCell) dgvPortfolioMarketValue.Rows[rowIndex]
                        .Cells[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]).Image =
                    shareObject.DoInternetUpdate
                        ? Resources.state_update_16
                        : Resources.state_no_update_16;

                // Current performance image
                if (shareObject.PerformanceValue > 0)
                {
                    ((TextAndImageCell) dgvPortfolioMarketValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex]).Image =
                        Resources.positiv_development_24;
                }
                else if (shareObject.PerformanceValue == 0)
                {
                    ((TextAndImageCell) dgvPortfolioMarketValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex]).Image =
                        Resources.neutral_development_24;
                }
                else
                {
                    ((TextAndImageCell) dgvPortfolioMarketValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex]).Image =
                        Resources.negativ_development_24;
                }

                // Complete performance image
                if (shareObject.CompletePerformanceValue > 0)
                {
                    ((TextAndImageCell) dgvPortfolioMarketValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex]).Image =
                        Resources.positiv_development_24;
                }
                else if (shareObject.CompletePerformanceValue == 0)
                {
                    ((TextAndImageCell) dgvPortfolioMarketValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex]).Image =
                        Resources.neutral_development_24;
                }
                else
                {
                    ((TextAndImageCell) dgvPortfolioMarketValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex]).Image =
                        Resources.negativ_development_24;
                }

                // If an index is given stop update after the update of the given index
                if (givenRowIndex > -1)
                    break;
            }
        }

        /// <summary>
        /// This function updates the images of the currently selected row of the market value data grid view
        /// <param name="givenRowIndex">Row index of the selected row. If no row index is given all rows will be updated.</param>
        /// </summary>
        private void OnDgvPortfolioFinalValueImageUpdate(int givenRowIndex = -1)
        {
            var startIndex = 0;

            // Check if a row index is given so set this as start index
            if (givenRowIndex > -1)
                startIndex = givenRowIndex;

            for (var rowIndex = startIndex; rowIndex < dgvPortfolioFinalValue.RowCount; rowIndex++)
            {
                // Return if not all columns have been added
                if ((dgvPortfolioFinalValue.Rows[rowIndex]
                    .Cells.Count - 1) != (int) ColumnIndicesPortfolioFinalValue.ECompleteFinalValueColumnIndex)
                    continue;

                // Get WKN of the current row
                var wkn = dgvPortfolioFinalValue.Rows[rowIndex]
                    .Cells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex].Value.ToString();

                // Get share object of the current WKN
                var shareObject = ShareObjectListFinalValue.Find(x => (x.Wkn == wkn));

                // Set internet update image
                ((TextAndImageCell) dgvPortfolioFinalValue.Rows[rowIndex]
                        .Cells[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex]).Image =
                    shareObject.DoInternetUpdate
                        ? Resources.state_update_16
                        : Resources.state_no_update_16;

                // Current performance image
                if (shareObject.PerformanceValue > 0)
                {
                    ((TextAndImageCell) dgvPortfolioFinalValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex]).Image =
                        Resources.positiv_development_24;
                }
                else if (shareObject.PerformanceValue == 0)
                {
                    ((TextAndImageCell) dgvPortfolioFinalValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex]).Image =
                        Resources.neutral_development_24;
                }
                else
                {
                    ((TextAndImageCell) dgvPortfolioFinalValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex]).Image =
                        Resources.negativ_development_24;
                }

                // Complete performance image
                if (shareObject.CompletePerformanceValue > 0)
                {
                    ((TextAndImageCell) dgvPortfolioFinalValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex])
                        .Image =
                        Resources.positiv_development_24;
                }
                else if (shareObject.CompletePerformanceValue == 0)
                {
                    ((TextAndImageCell) dgvPortfolioFinalValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex])
                        .Image =
                        Resources.neutral_development_24;
                }
                else
                {
                    ((TextAndImageCell) dgvPortfolioFinalValue.Rows[rowIndex]
                            .Cells[(int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex])
                        .Image =
                        Resources.negativ_development_24;
                }

                // If an index is given stop update after the update of the given index
                if (givenRowIndex > -1)
                    break;
            }
        }

        #endregion Update images in the data grid views

        #region DataBinding data grid view portfolio

        /// <summary>
        /// This function does data grid view market value configuration when the data binding is done.
        /// </summary>
        /// <param name="sender">Data grid view market value</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DgvPortfolioMarketValue_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers and width
                // and resize the data gird view market value
                // At least do the row selection
                if (dgvPortfolioMarketValue.Columns.Count !=
                    (int) ColumnIndicesPortfolioMarketValue.ECompleteMarketValueColumnIndex + 1) return;

                // Set market value data grid view column header names
                OnSetDgvPortfolioMarketValueColumnHeaderNames();

                // Set market value data grid view column header texts
                OnSetDgvPortfolioMarketValueColumnHeaderCaptions();

                #region Width configuration

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex].Width =
                    WknColumnSize;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EVolumeColumnIndex].Width =
                    VolumeColumnSize;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPriceColumnIndex].Width =
                    PriceColumnSize;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformancePrevDayColumnIndex]
                        .Width =
                    PerformancePrevDayColumnSize;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex].Width =
                    PerformanceColumnSize;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EMarketValueColumnIndex].Width =
                    MarketFinalValueColumnSize;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex]
                        .Width =
                    CompletePerformanceColumnSize;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ECompleteMarketValueColumnIndex]
                        .Width =
                    CompleteMarketFinalValueColumnSize;

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ENameColumnIndex].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ENameColumnIndex].FillWeight =
                    10;

                #endregion Width configuration

                #region Alignment configuration

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                    .DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ENameColumnIndex]
                    .DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EVolumeColumnIndex]
                    .DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPriceColumnIndex]
                    .DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformancePrevDayColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EMarketValueColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ECompleteMarketValueColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                #endregion Alignment configuration

                #region Footer width configuration

                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex]
                    .Width = PerformanceColumnSize;
                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.EMarketValueColumnIndex]
                    .Width = MarketFinalValueColumnSize;
                dgvPortfolioFooterMarketValue
                        .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.ECompletePerformanceColumnIndex].Width =
                    CompletePerformanceColumnSize;
                dgvPortfolioFooterMarketValue
                        .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.ECompleteMarketValueColumnIndex].Width =
                    CompleteMarketFinalValueColumnSize;

                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex]
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex]
                    .FillWeight = 10;

                #endregion Footer width configuration

                #region Footer aligment configuration

                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.EMarketValueColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.ECompletePerformanceColumnIndex]
                    .DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterMarketValue
                    .Columns[(int) ColumnIndicesPortfolioFooterMarketValue.ECompleteMarketValueColumnIndex]
                    .DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;

                #endregion Footer aligment configuration

                // Resize data grid view market value
                OnResizeDataGridView();

                if (AddFlagMarketValue == false && DeleteFlagMarketValue == false)
                {
                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex);
                }

                // When a share has been added clear the data grid view market value selection
                // and select the last row. At least make an update of the added share
                if (AddFlagMarketValue)
                {
                    // Reset flag
                    AddFlagMarketValue = false;

                    //if (dgvPortfolioMarketValue.Rows.Count > 1)
                    //    dgvPortfolioMarketValue.ClearSelection();

                    SelectedDataGridViewShareIndex = ShareObjectListMarketValue.IndexOf(ShareObjectMarketValue);
                    dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex);

                    if (dgvPortfolioMarketValue.RowCount == 1)
                    {
                        if (InitFlag == false)
                            InitFlag = true;
                        // Add data grid view footers
                        AddShareFooters();
                    }
                    else
                        // Refresh the footers
                        RefreshFooters();
                }

                // Make an update of the footer
                if (EditFlagMarketValue)
                {
                    // Reset flag
                    EditFlagMarketValue = false;

                    // Refresh the footers
                    RefreshFooters();
                }

                // When a share has been deleted clear the DataGridView portfolio selection
                // and select the first row if rows still present.
                if (!DeleteFlagMarketValue) return;

                // Reset flag
                DeleteFlagMarketValue = false;

                dgvPortfolioMarketValue.ClearSelection();
                if (dgvPortfolioMarketValue.Rows.Count > 0)
                {
                    SelectedDataGridViewShareIndex = 0;
                    dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex,
                        true);
                }
                else
                {
                    // Clear portfolio footer
                    dgvPortfolioFooterMarketValue.Rows.Clear();
                }

                // Refresh the footers
                RefreshFooters();
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/DisplayFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }

        }

        /// <summary>
        /// This function does DataGridView portfolio configuration when the data binding is done.
        /// </summary>
        /// <param name="sender">DataGridView portfolio</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DgvPortfolioFinalValue_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers and width
                // and resize the data grid view final value
                // At least do the row selection
                if (dgvPortfolioFinalValue.Columns.Count !=
                    (int) ColumnIndicesPortfolioFinalValue.ECompleteFinalValueColumnIndex + 1) return;

                // Set final value data grid view column header names and texts
                OnSetDgvPortfolioFinalValueColumnHeaderNames();

                // Set final value data grid view column header texts
                OnSetDgvPortfolioFinalValueColumnHeaderTexts();

                #region Width configuration

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex].Width =
                    WknColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EVolumeColumnIndex].Width =
                    VolumeColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EBrokerageDividendColumnIndex]
                    .Width = BrokerageDividendColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPriceColumnIndex].Width =
                    PriceColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformancePrevDayColumnIndex]
                    .Width = PerformancePrevDayColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex].Width =
                    PerformanceColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EFinalValueColumnIndex].Width =
                    MarketFinalValueColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex]
                    .Width = CompletePerformanceColumnSize;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompleteFinalValueColumnIndex]
                    .Width = CompleteMarketFinalValueColumnSize;

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ENameColumnIndex].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ENameColumnIndex].FillWeight = 10;

                #endregion With configuration

                #region Alignment configuration

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex].DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ENameColumnIndex].DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EVolumeColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EBrokerageDividendColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPriceColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformancePrevDayColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EFinalValueColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompleteFinalValueColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                #endregion Alignment configuration

                #region Footer configuration

                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EBrokerageDividendIndex].Width =
                    BrokerageDividendColumnSize;
                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex].Width =
                    PerformanceColumnSize;
                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EFinalValueColumnIndex].Width =
                    MarketFinalValueColumnSize;
                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ECompletePerformanceColumnIndex].Width =
                    CompletePerformanceColumnSize;
                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ECompleteFinalValueColumnIndex].Width =
                    CompleteMarketFinalValueColumnSize;

                dgvPortfolioFooterFinalValue
                        .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ELabelBrokerageDividendIndex]
                        .AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ELabelBrokerageDividendIndex].FillWeight = 10;

                #endregion Footer configuration

                #region Footer aligment configuration

                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ELabelBrokerageDividendIndex].DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EBrokerageDividendIndex].DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex].DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.EFinalValueColumnIndex].DefaultCellStyle
                    .Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ECompletePerformanceColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPortfolioFooterFinalValue
                    .Columns[(int) ColumnIndicesPortfolioFooterFinalValue.ECompleteFinalValueColumnIndex]
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                #endregion Footer aligment configuration

                // Resize data grid view final value
                OnResizeDataGridView();

                if (AddFlagFinalValue == false && DeleteFlagFinalValue == false)
                {
                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex);
                }

                // When a share has been added clear the data grid view final value selection
                // and select the last row. At least make an update of the added share
                if (AddFlagFinalValue)
                {
                    // Reset flag
                    AddFlagFinalValue = false;

                    if (dgvPortfolioFinalValue.Rows.Count > 1)
                        dgvPortfolioFinalValue.ClearSelection();

                    SelectedDataGridViewShareIndex = ShareObjectListFinalValue.IndexOf(ShareObjectFinalValue);
                    dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex);

                    if (dgvPortfolioFinalValue.RowCount == 1)
                    {
                        if (InitFlag == false)
                            InitFlag = true;
                        // Add portfolio footers
                        AddShareFooters();
                    }
                    else
                        // Refresh the footers
                        RefreshFooters();

                    // Update the new share
                    btnRefresh.PerformClick();
                }

                // Make an update of the footers
                if (EditFlagFinalValue)
                {
                    // Reset flag
                    EditFlagFinalValue = false;

                    // Refresh the footers
                    RefreshFooters();
                }

                // When a share has been deleted clear the data grid view final value selection
                // and select the first row if rows still present.
                if (!DeleteFlagFinalValue) return;

                // Reset flag
                DeleteFlagFinalValue = false;

                dgvPortfolioFinalValue.ClearSelection();
                if (dgvPortfolioFinalValue.Rows.Count > 0)
                {
                    SelectedDataGridViewShareIndex = 0;
                    dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                    // Scroll to the selected row
                    Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex,
                        LastFirstDisplayedRowIndex, true);
                }
                else
                {
                    // Clear portfolio footer
                    dgvPortfolioFooterFinalValue.Rows.Clear();
                }

                // Refresh the footers
                RefreshFooters();

                // Update images in the data grid view
                OnDgvPortfolioFinalValueImageUpdate();
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/DisplayFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion DataBinding DataGridView portfolio

        #region Refresh data grid view footers

        /// <summary>
        /// This function refreshes the footers.
        /// </summary>
        private void RefreshFooters()
        {
            try
            {
                #region dgvPortfolioMarketValue

                // Set colors correctly corresponding to the data grid view market value row colors
                if (dgvPortfolioMarketValue.RowsDefaultCellStyle.BackColor == Color.LightGray &&
                    dgvPortfolioMarketValue.RowCount % 2 == 0)
                {
                    dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.White;
                    dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                }

                if (dgvPortfolioFooterMarketValue.Rows.Count == 3 && ShareObjectListMarketValue.Count > 0 &&
                    ShareObjectListMarketValue[0] != null)
                {
                    // Set purchase value of the current stock volume
                    dgvPortfolioFooterMarketValue.Rows[0].Cells[
                            (int) ColumnIndicesPortfolioFooterMarketValue.EMarketValueColumnIndex].Value =
                        ShareObjectListMarketValue[0].PortfolioPurchaseValueAsStrUnit;

                    // Set performance value of the current stock volume
                    dgvPortfolioFooterMarketValue.Rows[1].Cells[
                            (int) ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex].Value =
                        ShareObjectListMarketValue[0].PortfolioProfitLossPerformanceValueAsStr;

                    // Set current market value of the current stock volume
                    dgvPortfolioFooterMarketValue.Rows[2].Cells[
                            (int) ColumnIndicesPortfolioFooterMarketValue.EMarketValueColumnIndex].Value =
                        ShareObjectListMarketValue[0].PortfolioMarketValueAsStrUnit;

                    // Set purchase value of all transactions
                    dgvPortfolioFooterMarketValue.Rows[0].Cells[
                            (int) ColumnIndicesPortfolioFooterMarketValue.ECompleteMarketValueColumnIndex].Value =
                        ShareObjectListMarketValue[0].PortfolioCompletePurchaseValueAsStrUnit;

                    // Set performance value of all transactions
                    dgvPortfolioFooterMarketValue.Rows[1].Cells[
                            (int) ColumnIndicesPortfolioFooterMarketValue.ECompletePerformanceColumnIndex].Value =
                        ShareObjectListMarketValue[0].PortfolioCompleteProfitLossPerformanceAsStrUnit;

                    // Set current market value of all transactions
                    dgvPortfolioFooterMarketValue.Rows[2].Cells[
                            (int) ColumnIndicesPortfolioFooterMarketValue.ECompleteMarketValueColumnIndex].Value =
                        ShareObjectListMarketValue[0].PortfolioCompleteMarketValueWithProfitLossAsStrUnit;
                }

                #endregion dgvPortfolioMarketValue

                #region dgvPortfolioFinalValue

                // Set colors correctly corresponding to the data grid view final value row colors
                if (dgvPortfolioFinalValue.RowsDefaultCellStyle.BackColor == Color.LightGray &&
                    dgvPortfolioFinalValue.RowCount % 2 == 0)
                {
                    dgvPortfolioFooterFinalValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dgvPortfolioFooterFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dgvPortfolioFooterFinalValue.RowsDefaultCellStyle.BackColor = Color.White;
                    dgvPortfolioFooterFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                }

                if (dgvPortfolioFooterFinalValue.Rows.Count == 3 && ShareObjectListFinalValue.Count > 0 &&
                    ShareObjectListFinalValue[0] != null)
                {
                    // Set purchase value of the current stock volume
                    dgvPortfolioFooterFinalValue.Rows[0].Cells[
                            (int) ColumnIndicesPortfolioFooterFinalValue.EFinalValueColumnIndex].Value =
                        ShareObjectListFinalValue[0].PortfolioPurchaseValueAsStrUnit;

                    // Set performance value of the current stock volume
                    dgvPortfolioFooterFinalValue.Rows[1].Cells[
                            (int) ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex].Value =
                        ShareObjectListFinalValue[0].PortfolioProfitLossPerformanceAsStrUnit;

                    // Set final value of the current stock volume
                    dgvPortfolioFooterFinalValue.Rows[2].Cells[
                            (int) ColumnIndicesPortfolioFooterFinalValue.EFinalValueColumnIndex].Value =
                        ShareObjectListFinalValue[0].PortfolioFinalValueAsStrUnit;

                    // Set purchase value of all transactions
                    dgvPortfolioFooterFinalValue.Rows[0].Cells[
                            (int)ColumnIndicesPortfolioFooterFinalValue.ECompleteFinalValueColumnIndex].Value =
                        ShareObjectListFinalValue[0].PortfolioCompletePurchaseValueAsStrUnit;

                    // Set performance value of all transactions
                    dgvPortfolioFooterFinalValue.Rows[1].Cells[
                            (int) ColumnIndicesPortfolioFooterFinalValue.ECompletePerformanceColumnIndex].Value =
                        ShareObjectListFinalValue[0].PortfolioCompleteProfitLossPerformanceAsStrUnit;

                    // Set final value of all transactions
                    dgvPortfolioFooterFinalValue.Rows[2].Cells[
                            (int) ColumnIndicesPortfolioFooterFinalValue.ECompleteFinalValueColumnIndex].Value =
                        ShareObjectListFinalValue[0].PortfolioCompleteFinalValueAsStrUnit;

                    // Set brokerage and dividend of all transactions
                    dgvPortfolioFooterFinalValue.Rows[1].Cells[
                            (int)ColumnIndicesPortfolioFooterFinalValue.EBrokerageDividendIndex].Value =
                        ShareObjectListFinalValue[0].PortfolioCompleteBrokerageDividendsAsStrUnit;
                }

                #endregion dgvPortfolioFinalValue
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolioFooter_Error/RefreshFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Refresh data grid view footers

        #region Data grid view enter

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="e">EventArgs</param>
        private void DgvPortfolioFinalValue_MouseEnter(object sender, EventArgs e)
        {
            ((DataGridView) sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="e">EventArgs</param>
        private void DgvPortfolioMarketValue_MouseEnter(object sender, EventArgs e)
        {
            ((DataGridView) sender).Focus();
        }

        #endregion Data grid view enter

        #region Data grid view mouse clicks

        /// <summary>
        /// This function detects if a mouse down event on the market value data grid view is made
        /// </summary>
        /// <param name="sender">Market value data grid view</param>
        /// <param name="e">DataGridViewCellMouseEventArgs</param>
        private void OnDgvPortfolioMarketValue_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if a valid row has been clicked
            if (e.RowIndex < 0 || e.RowIndex >= ShareObjectListMarketValue.Count) return;

            // Check double click
            CheckDoubleClick(e);
        }

        /// <summary>
        /// This function detects if a mouse down event on the final value data grid view is made
        /// </summary>
        /// <param name="sender">Final value data grid view</param>
        /// <param name="e">DataGridViewCellMouseEventArgs</param>
        private void OnDgvPortfolioFinalValue_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Check if a valid row has been clicked
            if (e.RowIndex < 0 || e.RowIndex >= ShareObjectListFinalValue.Count) return;

            // Check double click
            CheckDoubleClick(e);
        }

        /// <summary>
        /// This function detects if a right mouse down event on the market value data grid view is made
        /// so open the chart form
        /// </summary>
        /// <param name="sender">Market value data grid view</param>
        /// <param name="e">MouseEventArgs</param>
        private void OnDgvPortfolioMarketValue_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the click was a right click
            if (e.Button == MouseButtons.Right)
            {
                // Show chart
                ShowChart(dgvPortfolioMarketValue.HitTest(e.X, e.Y).RowIndex);
            }
        }

        /// <summary>
        /// This function detects if a right mouse down event on the final value data grid view is made
        /// so open the chart form
        /// </summary>
        /// <param name="sender">Final value data grid view</param>
        /// <param name="e">MouseEventArgs</param>
        private void OnDgvPortfolioFinalValue_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the click was a right click
            if (e.Button == MouseButtons.Right)
            {
                // Show chart
                ShowChart(dgvPortfolioFinalValue.HitTest(e.X, e.Y).RowIndex);
            }
        }

        /// <summary>
        /// This function does the detection if a single click or a double click on a data grid view row is made
        /// </summary>
        /// <param name="e">DataGridViewCellMouseEventArgs</param>
        private void CheckDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            // This is the first mouse click.
            if (_isFirstCellClick)
            {
                _isFirstRowIndex = e.RowIndex;
                _isFirstCellClick = false;

                // Start the double click timer.
                timerMouseCellDownDoubleClick.Start();
            }
            else
            {
                // Check if the second clicked row is the not same as the first clicked row
                // and if the time between the first and second click is not greater than the double click detection time
                // so no double click has been made
                if (_isFirstRowIndex != e.RowIndex ||
                    _checkDoubleClickMilliSeconds >= SystemInformation.DoubleClickTime) return;

                _isDoubleRowIndex = e.RowIndex;
                _isSecondCellClick = true;
            }
        }

        #endregion Data grid view mouse clicks

        #region Show chart

        /// <summary>
        /// This function creates the chart for the clicked data grid view index
        /// </summary>
        /// <param name="rowIndex">Clicked data grid view row index</param>
        private void ShowChart(int rowIndex)
        {
            // Check if a chart window already exist so close and delete it
            if (FrmChart != null)
            {
                FrmChart?.Close();
                FrmChart = null;
            }

            // Check if the row index is valid
            if (rowIndex < 0)
                return;

            // Check if a chart window already exists else create a new one
            if (FrmChart == null)
                FrmChart = new FrmChart(MarketValueOverviewTabSelected,
                    ShareObjectListFinalValue[rowIndex], ShareObjectListMarketValue[rowIndex],
                    rchTxtBoxStateMessage, Logger, Language, LanguageName,
                    ChartingInterval.Year, 1, ChartingColorDictionary);

            // Set window title
            FrmChart.Text = ShareObjectListFinalValue[rowIndex].Name;

            // Set location and size for the chart window
            // Offset under or above the mouse pointer
            const int offSetY = 15;
            // Offset smaller than selected data grid view
            const int offSetX = 0;
            // Width of the selected data grid view
            var dgvWidth = MarketValueOverviewTabSelected
                ? dgvPortfolioMarketValue.Width
                : dgvPortfolioFinalValue.Width;
            // Get location point of the selected data grid view
            var dgvLocation = MarketValueOverviewTabSelected
                ? dgvPortfolioMarketValue.PointToScreen(Point.Empty)
                : dgvPortfolioFinalValue.PointToScreen(Point.Empty);

            // Check if the chart form should be shown under or above the mouse pointer
            if (MousePosition.Y - Location.Y + FrmChart.Height - offSetY <= Height)
            {
                // Show under mouse pointer
                dgvLocation.Y = MousePosition.Y - offSetY;
            }
            else
            {
                // Show above mouse pointer
                dgvLocation.Y = MousePosition.Y - FrmChart.Height + offSetY;
            }

            // Move chart location to the left away from the data grid view border
            dgvLocation.X += offSetX;
            // Set chart form location
            FrmChart.Location = dgvLocation;

            // Set chart form size  ( 2 * offSetX = left offset and right offset )
            FrmChart.Size = new Size(dgvWidth - (2 * offSetX), FrmChart.Height);
            FrmChart.Show();
        }

        #endregion Show chart

        #region Timer

        /// <summary>
        /// This function detects if a double click has been made or a single click
        /// </summary>
        /// <param name="sender">Timer</param>
        /// <param name="e">EventArgs</param>
        private void TimerMouseCellDownDoubleClick_Tick(object sender, EventArgs e)
        {
            // Increase the time which detects if it is a single or double click
            _checkDoubleClickMilliSeconds += timerMouseCellDownDoubleClick.Interval;

            // Check if the timer has reached which detects a double click time limit.
            if (_checkDoubleClickMilliSeconds < SystemInformation.DoubleClickTime) return;

            timerMouseCellDownDoubleClick.Stop();

            // Check if it is the second click so a double click has been made
            // so open the ShareDetailsForm
            if (_isSecondCellClick)
            {
                try
                {
                    if (_isDoubleRowIndex < 0 || _isDoubleRowIndex >= ShareObjectListFinalValue.Count) return;

                    // Stop the double click timer
                    timerMouseCellDownDoubleClick.Enabled = false;

                    // Create ShareDetails form
                    FrmShareDetailsForm = new FrmShareDetails(MarketValueOverviewTabSelected,
                        ShareObjectFinalValue, ShareObjectMarketValue,
                        rchTxtBoxStateMessage, Logger, ChartingColorDictionary,
                        Language, LanguageName);

                    // Set location and size for the chart window
                    // Offset under or above the mouse pointer
                    const int offSetY = 15;
                    // Offset smaller than selected data grid view
                    const int offSetX = 0;
                    // Width of the selected data grid view
                    var dgvWidth = MarketValueOverviewTabSelected
                        ? dgvPortfolioMarketValue.Width
                        : dgvPortfolioFinalValue.Width;
                    // Get location point of the selected data grid view
                    var dgvLocation = MarketValueOverviewTabSelected
                        ? dgvPortfolioMarketValue.PointToScreen(Point.Empty)
                        : dgvPortfolioFinalValue.PointToScreen(Point.Empty);

                    // Check if the chart form should be shown under or above the mouse pointer
                    if (MousePosition.Y - Location.Y + FrmShareDetailsForm.Height - offSetY <= Height)
                    {
                        // Show under mouse pointer
                        dgvLocation.Y = MousePosition.Y - offSetY;
                    }
                    else
                    {
                        // Show above mouse pointer
                        dgvLocation.Y = MousePosition.Y - FrmShareDetailsForm.Height + offSetY;
                    }

                    // Move chart location to the left away from the data grid view border
                    dgvLocation.X += offSetX;
                    // Set chart form location
                    FrmShareDetailsForm.Location = dgvLocation;

                    // Set chart form size  ( 2 * offSetX = left offset and right offset )
                    FrmShareDetailsForm.Size = new Size(dgvWidth - (2 * offSetX), FrmShareDetailsForm.Height);
                    FrmShareDetailsForm.MaximumSize = new Size(dgvWidth - (2 * offSetX), FrmShareDetailsForm.Height);

                    FrmShareDetailsForm.ShowDialog();
                    FrmShareDetailsForm = null;
                }
                catch (Exception ex)
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                        ex);
                }
            }

            // Allow the MouseDown event handler to process clicks again.
            _isFirstCellClick = true;
            _isDoubleRowIndex = -1;
            _isSecondCellClick = false;
            _isDoubleRowIndex = -1;
            _checkDoubleClickMilliSeconds = 0;
        }

        #endregion Timer

        #endregion Methods
    }
}