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
using SharePortfolioManager.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Column enums for dgvPortfolioMarketValue and dgvPortfolioFooterMarketValue

        enum ColumnIndicesPortfolioMarketValue
        {
            EWknNumberColumnIndex = 0,
            EShareNameColumnIndex,
            EShareVolumeColumnIndex,
            ESharePriceColumnIndex,
            ESharePerformanceDayBeforeColumnIndex,
            EImagePerformanceColumnIndex,
            ESharePerformanceColumnIndex,
            EShareSumColumnIndex
        }

        enum ColumnIndicesPortfolioFooterMarketValue
        {
            ELabelTotalColumnIndex = 0,
            EPerformanceColumnIndex,
            ESumColumnIndex
        }

        #endregion Column enums for dgvPortfolioMarketValue and dgvPortfolioFooterMarketValue

        #region Column enums for dgvPortfolioFinalValue and dgvPortfolioFooterFinalValue

        enum ColumnIndicesPortfolioFinalValue
        {
            EWknNumberColumnIndex = 0,
            EShareNameColumnIndex,
            EShareVolumeColumnIndex,
            EShareCostsDividendColumnIndex,
            ESharePriceColumnIndex,
            ESharePerformanceDayBeforeColumnIndex,
            EImagePerformanceColumnIndex,
            ESharePerformanceColumnIndex,
            EShareSumColumnIndex
        }

        enum ColumnIndicesPortfolioFooterFinalValue
        {
            ELabelCostsDividendIndex = 0,
            ECostsDividendIndex,
            ELabelTotalColumnIndex,
            EPerformanceColumnIndex,
            ESumColumnIndex
        }

        #endregion Column enums for dgvPortfolioFinalValue and dgvPortfolioFooterFinalValue

        #region Variables

        #region Column width for the dgvPortfolio and dgvPortfolioFooter

        private const int WKNNumber = 70;
        private const int SharePrice = 110;
        private const int ShareVolume = 125;
        private const int ShareCostsDividend = 100;
        private const int SharePerformanceDayBefore = 110;
        private const int ImagePerformance = 25;
        private const int SharePerformance = 100;
        private const int ShareSum = 125;

        #endregion Column width for the dgvPortfolio and dgvPortfolioFooter

        #region Data binding sources for the data grid views

        /// <summary>
        /// Stores the BindingSource for the dgvPortfolioMarketValue
        /// </summary>
        private readonly BindingSource _dgvPortfolioBindingSourceMarketValue = new BindingSource();

        /// <summary>
        /// Stores the BindingSource for the dgvPortfolioFinalValue
        /// </summary>
        private readonly BindingSource _dgvPortfolioBindingSourceFinalValue = new BindingSource();

        #endregion Data binding sources for the data grid views

        #region ImageList with the images for the performance visualization in the data gird views

        private readonly List<Image> _imageList = new List<Image>
        {
            Resources.empty_arrow,
            Resources.red_down_arrow,
            Resources.gray_neutral_arrow,
            Resources.green_up_arrow
        };

        #endregion Images for the performance visualization in the data grid views

        #endregion Variables

        #region Properties

        public BindingSource DgvPortfolioBindingSourceMarketValue
        {
            get { return _dgvPortfolioBindingSourceMarketValue; }
        }

        public BindingSource DgvPortfolioBindingSourceFinalValue
        {
            get { return _dgvPortfolioBindingSourceFinalValue; }
        }

        public List<Image> ImageList
        {
            get { return _imageList; }
        }
        #endregion Properties

        #region Methods

        #region Data grid view configuration

        /// <summary>
        /// This function configures the dgvPortfolios (like row style)
        /// </summary>
        private void DgvPortfolioConfiguration()
        {
            try
            {
                #region dgvProtfolioMarketValue

                // Advanced configuration DataGridView share portfolio
                DataGridViewCellStyle styleMarketValue = dgvPortfolioMarketValue.ColumnHeadersDefaultCellStyle;
                styleMarketValue.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleMarketValue.Font = new Font("Consolas", 9, FontStyle.Bold);


                dgvPortfolioMarketValue.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvPortfolioMarketValue.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                // Does resize the row to display the complete content
                dgvPortfolioMarketValue.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                dgvPortfolioMarketValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                dgvPortfolioMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                dgvPortfolioMarketValue.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dgvPortfolioMarketValue.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dgvPortfolioMarketValue.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                #endregion dgvProtfolioMarketValue

                #region dgvProtfolioFinalValue

                // Advanced configuration DataGridView share portfolio
                DataGridViewCellStyle styleFinaleValue = dgvPortfolioFinalValue.ColumnHeadersDefaultCellStyle;
                styleFinaleValue.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleFinaleValue.Font = new Font("Consolas", 9, FontStyle.Bold);

                dgvPortfolioFinalValue.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvPortfolioFinalValue.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                // Does resize the row to display the complete content
                dgvPortfolioFinalValue.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                dgvPortfolioFinalValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                dgvPortfolioFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                dgvPortfolioFinalValue.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dgvPortfolioFinalValue.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dgvPortfolioFinalValue.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                #endregion dgvProtfolioFinalValue
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/ConfigurationFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Data grid view configuration

        #region Data grid view portfolio footer configuration

        /// <summary>
        /// This function configures the dgvPortfolioFooter (like row style)
        /// </summary>
        private void DgvPortfolioFooterConfiguration()
        {
            try
            {
                #region dgvPortfolioFooterMarketValue

                DataGridViewTextBoxColumn dgvscLabelTotalMarketValue = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn dgvscPerformanceMarketValue = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn dgvscTotalMarketValue = new DataGridViewTextBoxColumn();

                dgvPortfolioFooterMarketValue.Columns.Add(dgvscLabelTotalMarketValue);
                dgvPortfolioFooterMarketValue.Columns.Add(dgvscPerformanceMarketValue);
                dgvPortfolioFooterMarketValue.Columns.Add(dgvscTotalMarketValue);

                dgvPortfolioFooterMarketValue.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

                // Does resize the row to display the complete content
                dgvPortfolioFooterMarketValue.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                dgvPortfolioFooterMarketValue.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dgvPortfolioFooterMarketValue.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dgvPortfolioFooterMarketValue.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                #endregion dgvPortfolioFooterMarketValue

                #region dgvPortfolioFooterMarketValue

                DataGridViewTextBoxColumn dgvscLabelCostDividendFinalValue = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn dgvscCostsDividendFinalValue = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn dgvscLabelTotalFinalValue = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn dgvscPerformanceFinalValue = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn dgvscTotalFinalValue = new DataGridViewTextBoxColumn();

                dgvPortfolioFooterFinalValue.Columns.Add(dgvscLabelCostDividendFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvscCostsDividendFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvscLabelTotalFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvscPerformanceFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvscTotalFinalValue);

                dgvPortfolioFooterFinalValue.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

                // Does resize the row to display the complete content
                dgvPortfolioFooterFinalValue.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                dgvPortfolioFooterFinalValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                dgvPortfolioFooterFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                dgvPortfolioFooterFinalValue.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dgvPortfolioFooterFinalValue.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dgvPortfolioFooterFinalValue.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                #endregion dgvPortfolioFooterFinalValue
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add statue message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter_Error/ConfigurationFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Data grid view portfolio footer configuration

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
                //ShareObjectMarketValue = null;
                //ShareObjectFinalValue = null;

                // Save current selected ShareObject via the selected WKN number
                if (dgvPortfolioMarketValue.SelectedRows.Count == 1 &&
                    dgvPortfolioMarketValue.SelectedRows[0].Cells.Count > 0 &&
                    dgvPortfolioMarketValue.SelectedRows[0].Cells[0].Value != null &&
                    AddFlagMarketValue == false)
                {
                    // Get selected WKN number
                    string selectedWKNNumber = dgvPortfolioMarketValue.SelectedRows[0].Cells[0].Value.ToString();

                    // Search for the selected ShareObject in the market value list
                    foreach (var shareObjectMarketValue in ShareObjectListMarketValue)
                    {
                        if (shareObjectMarketValue.Wkn == selectedWKNNumber)
                        {
                            ShareObjectMarketValue = shareObjectMarketValue;
                            break;
                        }
                    }

                    // Search for the selected ShareObject in the final value list
                    foreach (var shareObjectFinalValue in ShareObjectListFinalValue)
                    {
                        if (shareObjectFinalValue.Wkn == selectedWKNNumber)
                        {
                            ShareObjectFinalValue = shareObjectFinalValue;
                            break;
                        }
                    }

                    if (ShareObjectMarketValue != null && ShareObjectFinalValue != null)
                    {
                        UpdateShareDetails(MarketValueOverviewTabSelected);
                        UpdateProfitLossDetails(MarketValueOverviewTabSelected);
                        UpdateDividendDetails(MarketValueOverviewTabSelected);
                        UpdateCostsDetails(MarketValueOverviewTabSelected);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
                //ShareObjectMarketValue = null;
                //ShareObjectFinalValue = null;

                // Save current selected ShareObject via the selected WKN number
                if (dgvPortfolioFinalValue.SelectedRows.Count == 1 &&
                    dgvPortfolioFinalValue.SelectedRows[0].Cells.Count > 0 &&
                    dgvPortfolioFinalValue.SelectedRows[0].Cells[0].Value != null &&
                    AddFlagFinalValue == false)
                {
                    // Get selected WKN number
                    string selectedWKNNumber = dgvPortfolioFinalValue.SelectedRows[0].Cells[0].Value.ToString();

                    // Search for the selected ShareObject in the final value list
                    foreach (var shareObjectFinalValue in ShareObjectListFinalValue)
                    {
                        if (shareObjectFinalValue.Wkn == selectedWKNNumber)
                        {
                            ShareObjectFinalValue = shareObjectFinalValue;
                            break;
                        }
                    }

                    // Search for the selected ShareObject in the market value list
                    foreach (var shareObjectMarketValue in ShareObjectListMarketValue)
                    {
                        if (shareObjectMarketValue.Wkn == selectedWKNNumber)
                        {
                            ShareObjectMarketValue = shareObjectMarketValue;
                            break;
                        }
                    }

                    if (ShareObjectFinalValue != null && ShareObjectListMarketValue != null)
                    {
                        UpdateShareDetails(MarketValueOverviewTabSelected);
                        UpdateProfitLossDetails(MarketValueOverviewTabSelected);
                        UpdateDividendDetails(MarketValueOverviewTabSelected);
                        UpdateCostsDetails(MarketValueOverviewTabSelected);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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

                if (dgvPortfolioFooterMarketValue.ColumnCount >= 3)
                {
                    var vScrollbarPortfolio = dgvPortfolioMarketValue.Controls.OfType<VScrollBar>().First();
                    var vScrollbarWidth = 0;
                    if (vScrollbarPortfolio.Visible)
                    {
                        vScrollbarWidth = vScrollbarPortfolio.Width;
                        dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.ESumColumnIndex].Width = ShareSum + vScrollbarWidth;
                    }
                    else
                    {
                        dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.ESumColumnIndex].Width =
                            ShareSum;
                    }
                }

                #endregion dgvPortfolioFooterFinalValue

                #region dgvPortfolioFooterFinalValue

                if (dgvPortfolioFooterFinalValue.ColumnCount >= 5)
                {
                    var _columnLabelTotalWidth = SharePrice + SharePerformanceDayBefore + ImagePerformance;

                    var vScrollbarPortfolio = dgvPortfolioFinalValue.Controls.OfType<VScrollBar>().First();
                    var vScrollbarWidth = 0;
                    if (vScrollbarPortfolio.Visible)
                    {
                        vScrollbarWidth = vScrollbarPortfolio.Width;
                        dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ESumColumnIndex].Width = ShareSum + vScrollbarWidth;
                    }
                    else
                    {
                        dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ESumColumnIndex].Width =
                            ShareSum;
                    }

                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ECostsDividendIndex].Width = ShareCostsDividend;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].Width = _columnLabelTotalWidth;
                }

                #endregion dgvPortfolioFooterFinalValue
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
            }
        }

        #endregion DataGridView portfolio and portfolio footer resize

        #region Add shares to data grid view portfolio and configure data grid view footer 

        /// <summary>
        /// This function add the shares to the DataGridView portfolio.
        /// </summary>
        void AddSharesToDataGridViews()
        {
            if (InitFlag)
            {
                try
                {
                    #region dgvPortfolioMarketValue

                    // Check if the share object list has items
                    if (ShareObjectListMarketValue != null && ShareObjectListMarketValue.Count > 0)
                    {
                        DgvPortfolioBindingSourceMarketValue.ResetBindings(false);
                        DgvPortfolioBindingSourceMarketValue.DataSource = ShareObjectListMarketValue;
                        dgvPortfolioMarketValue.DataSource = DgvPortfolioBindingSourceMarketValue;
                    }

                    #endregion dgvPortfolioMarketValue

                    #region dgvPortfolioFinalValue

                    // Check if the share object list has items
                    if (ShareObjectListFinalValue != null && ShareObjectListFinalValue.Count > 0)
                    {
                        DgvPortfolioBindingSourceFinalValue.ResetBindings(false);
                        DgvPortfolioBindingSourceFinalValue.DataSource = ShareObjectListFinalValue;
                        dgvPortfolioFinalValue.DataSource = DgvPortfolioBindingSourceFinalValue;
                    }

                    #endregion dgvPortfolioFinalValue
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    InitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/DisplayFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                }
            }
        }

        /// <summary>
        /// This function configures and adds the footer rows in the DataGridView portfolio footer.
        /// </summary>
        private void AddShareFooters()
        {
            if (InitFlag)
            {
                try
                {
                    // Clear footers
                    dgvPortfolioFooterMarketValue.Rows.Clear();
                    dgvPortfolioFooterFinalValue.Rows.Clear();

                    #region dgvPortfolioFooterMarketValue

                    // Check if the share object list has items
                    if (ShareObjectListMarketValue != null && ShareObject.ObjectCounter > 0 && dgvPortfolioMarketValue.Rows.Count > 0)
                    {
                        // Set row colors
                        if (dgvPortfolioMarketValue.RowsDefaultCellStyle.BackColor == Color.LightGray && dgvPortfolioMarketValue.RowCount % 2 == 0)
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
                        object[] newFooterPurchaseValueOfAllShares = new object[]
                        {
                            string.Format(@"{0}", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalPurchaseValue", LanguageName)),
                            @"",
                            ShareObjectListMarketValue[0].PortfolioPurchaseValueAsStrUnit
                        };

                        // Create row content with total performance of the market value portfolio
                        object[] newFooterDepotDevelopment = new object[]
                        {
                            string.Format(@"{0}", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalPerformance", LanguageName)),
                            ShareObjectListMarketValue[0].ProfitLossPerformancePortfolioValueAsStr,
                            @""
                        };

                        // Create row content with total market value of the market value portfolio
                        object[] newFooterTotalDepotVolume = new object[]
                        {
                            string.Format(@"{0}", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolioFooter/RowContent_TotalDepotValue", LanguageName)),
                            @"",
                            ShareObjectListMarketValue[0].PortfolioMarketValueAsStrUnit
                        };

                        // Add rows to the data grid view market value footer
                        dgvPortfolioFooterMarketValue.Rows.Add(newFooterPurchaseValueOfAllShares);
                        dgvPortfolioFooterMarketValue.Rows.Add(newFooterDepotDevelopment);
                        dgvPortfolioFooterMarketValue.Rows.Add(newFooterTotalDepotVolume);
                    }

                    #endregion dgvPortfolioFooterMarketValue

                    #region dgvPortfolioFooterFinalValue

                    // Check if the share object list has items
                    if (ShareObjectListFinalValue != null && ShareObject.ObjectCounter > 0 && dgvPortfolioFinalValue.Rows.Count > 0)
                    {
                        // Set row colors
                        if (dgvPortfolioFinalValue.RowsDefaultCellStyle.BackColor == Color.LightGray && dgvPortfolioFinalValue.RowCount % 2 == 0)
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
                        object[] newFooterPurchaseFinalOfAllShares = new object[]
                        {
                            @"",
                            @"",
                            string.Format(@"{0}", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPurchaseValue", LanguageName)),
                            @"",
                            ShareObjectListFinalValue[0].PortfolioPurchaseValueAsStrUnit
                        };

                        // Create row content with total performance of the final value portfolio
                        object[] newFooterDepotDevelopment = new object[]
                        {
                            string.Format(@"{0}", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalCostDividend", LanguageName)),
                            ShareObjectListFinalValue[0].CostsDividendPortfolioValueAsStr,
                            string.Format(@"{0}", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalPerformance", LanguageName)),
                            ShareObjectListFinalValue[0].ProfitLossPerformancePortfolioValueAsStr,
                            @""
                        };

                        // Create row content with total market value of the final value portfolio
                        object[] newFooterTotalDepotVolume = new object[]
                        {
                            @"",
                            @"",
                            string.Format(@"{0}", Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter/RowContent_TotalDepotValue", LanguageName)),
                            @"",
                            ShareObjectListFinalValue[0].PortfolioFinalValueAsStrUnit
                        };

                        // Add rows to the data grid view final value footer
                        dgvPortfolioFooterFinalValue.Rows.Add(newFooterPurchaseFinalOfAllShares);
                        dgvPortfolioFooterFinalValue.Rows.Add(newFooterDepotDevelopment);
                        dgvPortfolioFooterFinalValue.Rows.Add(newFooterTotalDepotVolume);

                    }

                    #endregion dgvPortfolioFooterFinalValue
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    InitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter_Error/ConfigurationFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                }
            }
        }

        #endregion Add shares to data grid view portfolio and configure data grid view footer 

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
                if (dgvPortfolioMarketValue.Columns.Count == (int)ColumnIndicesPortfolioMarketValue.EShareSumColumnIndex + 1)
                {
                    #region Captions

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EWknNumberColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_WKN", LanguageName);
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EWknNumberColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_WKN", LanguageName);

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareNameColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Name", LanguageName);
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareNameColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Name", LanguageName);

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareVolumeColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Volume", LanguageName);
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareVolumeColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Volume", LanguageName);

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePriceColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Price", LanguageName);
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePriceColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Price", LanguageName);

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePerformanceDayBeforeColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_PrevDay", LanguageName);

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EImagePerformanceColumnIndex].HeaderText = @"";

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePerformanceColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Performance", LanguageName);
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePerformanceColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Performance", LanguageName);

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareSumColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue", LanguageName);
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareSumColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue", LanguageName);

                    #endregion Captions

                    #region Width configuration

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EWknNumberColumnIndex].Width = WKNNumber;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePriceColumnIndex].Width = SharePrice;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareVolumeColumnIndex].Width = ShareVolume;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePerformanceDayBeforeColumnIndex].Width = SharePerformanceDayBefore;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EImagePerformanceColumnIndex].Width = ImagePerformance;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePerformanceColumnIndex].Width = SharePerformance;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareSumColumnIndex].Width = ShareSum;

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareNameColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareNameColumnIndex].FillWeight = 10;

                    #endregion Width configuration

                    #region Alignment configuration

                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EWknNumberColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareNameColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePriceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareVolumeColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePerformanceDayBeforeColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EImagePerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.ESharePerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioMarketValue.Columns[(int)ColumnIndicesPortfolioMarketValue.EShareSumColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    #endregion Alignment configuration

                    #region Footer width configuration

                    dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex].Width = SharePerformance;
                    dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.ESumColumnIndex].Width = ShareSum;
                    dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.ESumColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvPortfolioFooterMarketValue.Columns[(int)ColumnIndicesPortfolioFooterMarketValue.ELabelTotalColumnIndex].FillWeight = 10;

                    #endregion Footer width configuration

                    // Resize data grid view market value
                    OnResizeDataGridView();

                    if (AddFlagMarketValue == false && DeleteFlagMarketValue == false)
                    {
                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                    }

                    // When a share has been added clear the data grid view market value selection
                    // and select the last row. At least make an update of the added share
                    if (AddFlagMarketValue == true)
                    {
                        // Reset flag
                        AddFlagMarketValue = false;

                        //if (dgvPortfolioMarketValue.Rows.Count > 1)
                        //    dgvPortfolioMarketValue.ClearSelection();

                        SelectedDataGridViewShareIndex = ShareObjectListMarketValue.IndexOf(ShareObjectMarketValue);
                        dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

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

                        // Update the new share
                        //btnRefresh.PerformClick();
                    }

                    // Make an update of the footer
                    if (EditFlagMarketValue == true)
                    {
                        // Reset flag
                        EditFlagMarketValue = false;

                        // Refresh the footers
                        RefreshFooters();
                    }

                    // When a share has been deleted clear the DataGridView portfolio selection
                    // and select the first row if rows still present.
                    if (DeleteFlagMarketValue == true)
                    {
                        // Reset flag
                        DeleteFlagMarketValue = false;

                        dgvPortfolioMarketValue.ClearSelection();
                        if (dgvPortfolioMarketValue.Rows.Count > 0)
                        {
                            SelectedDataGridViewShareIndex = 0;
                            dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioMarketValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                        }
                        else
                        {
                            // Clear portfolio footer
                            dgvPortfolioFooterMarketValue.Rows.Clear();
                        }

                        // Refresh the footers
                        RefreshFooters();
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/DisplayFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
                if (dgvPortfolioFinalValue.Columns.Count == (int)ColumnIndicesPortfolioFinalValue.EShareSumColumnIndex + 1)
                {
                    #region Captions

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EWknNumberColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_WKN", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EWknNumberColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_WKN", LanguageName);

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareNameColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Name", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareNameColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Name", LanguageName);

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareVolumeColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Volume", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareVolumeColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Volume", LanguageName);

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareCostsDividendColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_CostsDividend", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareCostsDividendColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_CostsDividend", LanguageName);

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePriceColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Price", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePriceColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Price", LanguageName);

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceDayBeforeColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PrevDay", LanguageName);

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EImagePerformanceColumnIndex].HeaderText = @"";
                    
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Performance", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Performance", LanguageName);

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareSumColumnIndex].Name =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue", LanguageName);
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareSumColumnIndex].HeaderText =
                        Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue", LanguageName);

                    #endregion Captions

                    #region Width configuration

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EWknNumberColumnIndex].Width = WKNNumber;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePriceColumnIndex].Width = SharePrice;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareVolumeColumnIndex].Width = ShareVolume;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareCostsDividendColumnIndex].Width = ShareCostsDividend;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceDayBeforeColumnIndex].Width = SharePerformanceDayBefore;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EImagePerformanceColumnIndex].Width = ImagePerformance;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceColumnIndex].Width = SharePerformance;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareSumColumnIndex].Width = ShareSum;

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareNameColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareNameColumnIndex].FillWeight = 10;

                    #endregion With configuration

                    #region Alignment configuration

                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EWknNumberColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareNameColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePriceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareVolumeColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceDayBeforeColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EImagePerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareCostsDividendColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.ESharePerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFinalValue.Columns[(int)ColumnIndicesPortfolioFinalValue.EShareSumColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    #endregion Alignment configuration

                    #region Footer configuration

                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ECostsDividendIndex].Width = ShareCostsDividend;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex].Width = SharePerformance;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ESumColumnIndex].Width = ShareSum;

                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelCostsDividendIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ECostsDividendIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelTotalColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ESumColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelCostsDividendIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvPortfolioFooterFinalValue.Columns[(int)ColumnIndicesPortfolioFooterFinalValue.ELabelCostsDividendIndex].FillWeight = 10;

                    #endregion Footer configuration

                    // Resize data grid view final value
                    OnResizeDataGridView();

                    if (AddFlagFinalValue == false && DeleteFlagFinalValue == false)
                    {
                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);
                    }

                    // When a share has been added clear the data grid view final value selection
                    // and select the last row. At least make an update of the added share
                    if (AddFlagFinalValue == true)
                    {
                        // Reset flag
                        AddFlagFinalValue = false;

                        //if (dgvPortfolioFinalValue.Rows.Count > 1)
                        //    dgvPortfolioFinalValue.ClearSelection();

                        SelectedDataGridViewShareIndex = ShareObjectListFinalValue.IndexOf(ShareObjectFinalValue);
                        dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex);

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
                    if (EditFlagFinalValue == true)
                    {
                        // Reset flag
                        EditFlagFinalValue = false;

                        // Refresh the footers
                        RefreshFooters();
                    }

                    // When a share has been deleted clear the data grid view final value selection
                    // and select the first row if rows still present.
                    if (DeleteFlagFinalValue == true)
                    {
                        // Reset flag
                        DeleteFlagFinalValue = false;

                        dgvPortfolioFinalValue.ClearSelection();
                        if (dgvPortfolioFinalValue.Rows.Count > 0)
                        {
                            SelectedDataGridViewShareIndex = 0;
                            dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolioFinalValue, SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                        }
                        else
                        {
                            // Clear portfolio footer
                            dgvPortfolioFooterFinalValue.Rows.Clear();
                        }

                        // Refresh the footers
                        RefreshFooters();
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/DisplayFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Databinding DataGridView portfolio

        #region Cell formatting and painting for the data grid views and footers

        /// <summary>
        /// This function formats the cells in the data grid view market value
        /// </summary>
        /// <param name="sender">Data grid view market value</param>
        /// <param name="e">DataGridViewCellFormattingEventArgs</param>
        private void DgvPortfolioMarketValue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                var splitString = e.Value.ToString().Split(' ');
                if (e.ColumnIndex == (int)ColumnIndicesPortfolioMarketValue.ESharePerformanceDayBeforeColumnIndex || e.ColumnIndex == (int)ColumnIndicesPortfolioMarketValue.ESharePerformanceColumnIndex)
                {
                    if ((Convert.ToDecimal(splitString[0])) > 0)
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else if ((Convert.ToDecimal(splitString[0])) == 0)
                    {
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/CellFormattingFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function formats the cells in the data grid view final value
        /// </summary>
        /// <param name="sender">Data grid view final value</param>
        /// <param name="e">DataGridViewCellFormattingEventArgs</param>
        private void DgvPortfolioFinalValue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                var splitString = e.Value.ToString().Split(' ');
                if (e.ColumnIndex == (int)ColumnIndicesPortfolioFinalValue.ESharePerformanceDayBeforeColumnIndex || e.ColumnIndex == (int)ColumnIndicesPortfolioFinalValue.ESharePerformanceColumnIndex)
                {
                    if ((Convert.ToDecimal(splitString[0])) > 0)
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else if ((Convert.ToDecimal(splitString[0])) == 0)
                    {
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/CellFormattingFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function formats the cells in the data grid view market value footer
        /// </summary>
        /// <param name="sender">Data grid view market value footer</param>
        /// <param name="e">DataGridViewCellFormattingEventArgs</param>
        private void DgvPortfolioFooterMarketValue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex == 1)
                {
                    var splitString = e.Value.ToString().Split(' ');

                    if (e.ColumnIndex == (int)ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex)
                    {
                        // Remove '%' or 'invalid' from the cell value for converting it into a decimal
                        // 'invalid' only appears when the 
                        if ((Convert.ToDecimal(splitString[0])) > 0)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                        }
                        else if ((Convert.ToDecimal(splitString[0])) == 0)
                        {
                            e.CellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter_Error/CellFormattingFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function formats the cells in the data grid view final value footer
        /// </summary>
        /// <param name="sender">Data grid view final value footer</param>
        /// <param name="e">DataGridViewCellFormattingEventArgs</param>
        private void DgvPortfolioFooterFinalValue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex == 1)
                {
                    var splitString = e.Value.ToString().Split(' ');

                    if (e.ColumnIndex == (int)ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex)
                    {
                        // Remove '%' or 'invalid' from the cell value for converting it into a decimal
                        // 'invalid' only appears when the 
                        if ((Convert.ToDecimal(splitString[0])) > 0)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                        }
                        else if ((Convert.ToDecimal(splitString[0])) == 0)
                        {
                            e.CellStyle.ForeColor = Color.Black;
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter_Error/CellFormattingFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function draws vertical column borders in the data grid view market value
        /// </summary>
        /// <param name="sender">Cell of the data grid view market value</param>
        /// <param name="e">DataGridViewCellPaintingEventArgs</param>
        private void DgvPortfolioMarketValue_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != (int)ColumnIndicesPortfolioMarketValue.ESharePerformanceDayBeforeColumnIndex && e.ColumnIndex < dgvPortfolioMarketValue.Columns.Count - 1)
                {
                    SolidBrush brush = new SolidBrush(dgvPortfolioMarketValue.ColumnHeadersDefaultCellStyle.BackColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    brush.Dispose();
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioMarketValue.GridColor, 0, ButtonBorderStyle.None,
                        dgvPortfolioMarketValue.GridColor, 0,
                        ButtonBorderStyle.None, dgvPortfolioMarketValue.GridColor, 1,
                        ButtonBorderStyle.Solid, dgvPortfolioMarketValue.GridColor, 1,
                        ButtonBorderStyle.Solid);

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/CellPaintingBorderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function draws vertical column borders in the data grid view final value
        /// </summary>
        /// <param name="sender">Cell of the data grid view final value</param>
        /// <param name="e">DataGridViewCellPaintingEventArgs</param>
        private void DgvPortfolioFinalValue_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != (int)ColumnIndicesPortfolioFinalValue.ESharePerformanceDayBeforeColumnIndex && e.ColumnIndex < dgvPortfolioFinalValue.Columns.Count - 1)
                {
                    SolidBrush brush = new SolidBrush(dgvPortfolioFinalValue.ColumnHeadersDefaultCellStyle.BackColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    brush.Dispose();
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFinalValue.GridColor, 0, ButtonBorderStyle.None,
                        dgvPortfolioFinalValue.GridColor, 0,
                        ButtonBorderStyle.None, dgvPortfolioFinalValue.GridColor, 1,
                        ButtonBorderStyle.Solid, dgvPortfolioFinalValue.GridColor, 1,
                        ButtonBorderStyle.Solid);

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio_Error/CellPaintingBorderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function draws vertical column borders in the data grid view final value footer
        /// </summary>
        /// <param name="sender">Cell of the data grid view final value footer</param>
        /// <param name="e">DataGridViewCellPaintingEventArgs</param>
        private void DgvPortfolioFooterMarketValue_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == 2)
                {
                    if (e.ColumnIndex < dgvPortfolioFooterMarketValue.Columns.Count - 1)
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooterMarketValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterMarketValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, 2, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterMarketValue.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterMarketValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                    else
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooterMarketValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterMarketValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, 2, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
                else
                {
                    if (e.ColumnIndex < dgvPortfolioFooterMarketValue.Columns.Count - 1)
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooterMarketValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterMarketValue.GridColor, 0,
                            ButtonBorderStyle.None, dgvPortfolioFooterMarketValue.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooterMarketValue.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterMarketValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter_Error/CellPaintingBorderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }

        }

        /// <summary>
        /// This function draws vertical column borders in the data grid view final value footer
        /// </summary>
        /// <param name="sender">Cell of the data grid view final value footer</param>
        /// <param name="e">DataGridViewCellPaintingEventArgs</param>
        private void DgvPortfolioFooterFinalValue_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == 2)
                {
                    if (e.ColumnIndex < dgvPortfolioFooterFinalValue.Columns.Count - 1)
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooterFinalValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterFinalValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, 2, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                    else
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooterFinalValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterFinalValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, 2, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
                else
                {
                    if (e.ColumnIndex < dgvPortfolioFooterFinalValue.Columns.Count - 1)
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooterFinalValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterFinalValue.GridColor, 0,
                            ButtonBorderStyle.None, dgvPortfolioFooterFinalValue.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter_Error/CellPaintingBorderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Cell formatting and painting for the data grid views and footers

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
                if (dgvPortfolioMarketValue.RowsDefaultCellStyle.BackColor == Color.LightGray && dgvPortfolioMarketValue.RowCount % 2 == 0)
                {
                    dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.White;
                    dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                }

                if (dgvPortfolioFooterMarketValue.Rows.Count == 3)
                {
                    // Set deposit of all shares
                    dgvPortfolioFooterMarketValue.Rows[0].Cells[
                        (int)ColumnIndicesPortfolioFooterMarketValue.ESumColumnIndex].Value = ShareObjectListMarketValue[0].PortfolioPurchaseValueAsStrUnit;

                    // Set performance of all shares
                    dgvPortfolioFooterMarketValue.Rows[1].Cells[
                        (int)ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex].Value = ShareObjectListMarketValue[0].ProfitLossPerformancePortfolioValueAsStr;

                    // Set value of the shares
                    dgvPortfolioFooterMarketValue.Rows[2].Cells[
                        (int)ColumnIndicesPortfolioFooterMarketValue.ESumColumnIndex].Value = ShareObjectListMarketValue[0].PortfolioMarketValueAsStrUnit;
                }

                #endregion dgvPortfolioMarketValue

                #region dgvPortfolioFinalValue

                // Set colors correctly corresponding to the data grid view final value row colors
                if (dgvPortfolioFinalValue.RowsDefaultCellStyle.BackColor == Color.LightGray && dgvPortfolioFinalValue.RowCount % 2 == 0)
                {
                    dgvPortfolioFooterFinalValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dgvPortfolioFooterFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dgvPortfolioFooterFinalValue.RowsDefaultCellStyle.BackColor = Color.White;
                    dgvPortfolioFooterFinalValue.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                }

                if (dgvPortfolioFooterFinalValue.Rows.Count == 3)
                {
                    // Set deposit of all shares
                    dgvPortfolioFooterFinalValue.Rows[0].Cells[
                        (int)ColumnIndicesPortfolioFooterFinalValue.ESumColumnIndex].Value = ShareObjectListFinalValue[0].PortfolioPurchaseValueAsStrUnit;

                    // Set costs and dividend of all shares
                    dgvPortfolioFooterFinalValue.Rows[1].Cells[
                        (int)ColumnIndicesPortfolioFooterFinalValue.ECostsDividendIndex].Value = ShareObjectListFinalValue[0].CostsDividendPortfolioValueAsStr;

                    // Set performance of all shares
                    dgvPortfolioFooterFinalValue.Rows[1].Cells[
                        (int)ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex].Value = ShareObjectListFinalValue[0].ProfitLossPerformancePortfolioValueAsStr;

                    // Set value of the shares
                    dgvPortfolioFooterFinalValue.Rows[2].Cells[
                        (int)ColumnIndicesPortfolioFooterFinalValue.ESumColumnIndex].Value = ShareObjectListFinalValue[0].PortfolioFinalValueAsStrUnit;
                }

                #endregion dgvPortfolioFinalValue
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(Helper.GetMyMethodName() + "\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolioFooter_Error/RefreshFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Refresh data grid view footers

        #region Data grid view enter

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="e">EventArgs</param>
        private void dgvPortfolioFinalValue_MouseEnter(object sender, EventArgs e)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="e">EventArgs</param>
        private void dgvPortfolioMarketValue_MouseEnter(object sender, EventArgs e)
        {
            ((DataGridView)sender).Focus();
        }

        #endregion Data grid view enter

        #endregion Methods
    }
}