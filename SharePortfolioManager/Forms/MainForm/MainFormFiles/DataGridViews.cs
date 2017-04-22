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
        #region Column Enums for dgvPortfolio and dgvPortfolioFooter

        enum ColumnIndicesPortfolio
        {
            EWknNumberColumnIndex = 0,
            EShareNameColumnIndex = 1,
            EShareVolumeColumnIndex = 2,
            EShareCostsDividendColumnIndex = 3,
            ESharePriceColumnIndex = 4,
            ESharePerformanceDayBeforeColumnIndex = 5,
            EImagePerformanceColumnIndex = 6,
            ESharePerformanceColumnIndex = 7,
            EShareSumColumnIndex = 8
        }

        enum ColumnIndicesPortfolioFooter
        {
            ELabelCostsDividendIndex = 0,
            ECostsDividendIndex = 1,
            ELabelTotalColumnIndex = 2,
            EPerformanceColumnIndex = 3,
            ESumColumnIndex = 4
        }

        #endregion Column Enums

        #region Variables

        /// <summary>
        /// Stores the BindingSource for the dgvPortfolio
        /// </summary>
        private readonly BindingSource _dgvPortfolioBindingSource = new BindingSource();

        #region Column width for the dgvPortfolio and dgvPortfolioFooter

        private const int WKNNumber = 70;
        private const int SharePrice = 110;
        private const int ShareVolume = 125;
        private const int SharePerformanceDayBefore = 110;
        private const int ImagePerformance = 25;
        private const int ShareCostsDividend = 100;
        private const int SharePerformance = 100;
        private const int ShareSum = 125;

        #endregion Column width for the dgvPortfolio and dgvPortfolioFooter

        #region ImageList with the images for the performance visualization in the dgvPortfolio

        private readonly List<Image> _imageList = new List<Image>
        {
            Resources.empty_arrow,
            Resources.red_down_arrow,
            Resources.gray_neutral_arrow,
            Resources.green_up_arrow
        };

        #endregion Images for the performance visualization in the dgvPortfolio

        #endregion Variables

        #region Properties
        #endregion Properties

        #region Methods

        #region DataGridView portfolio configuration

        /// <summary>
        /// This function configures the dgvPortfolio (like row style)
        /// </summary>
        private void DgvPortfolioConfiguration()
        {
            if (_bInitFlag)
            {
                try
                {
                    // Advanced configuration DataGridView share portfolio
                    DataGridViewCellStyle style = dgvPortfolio.ColumnHeadersDefaultCellStyle;
                    style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    style.Font = new Font("Consolas", 9, FontStyle.Bold);

                    dgvPortfolio.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    dgvPortfolio.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                    // Does resize the row to display the complete content
                    dgvPortfolio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                    dgvPortfolio.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dgvPortfolio.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dgvPortfolio.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dgvPortfolio.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                    dgvPortfolio.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("DgvPortfolioConfiguration()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    _bInitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio_Error/ConfigurationFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                }
            }
        }

        #endregion DataGridView portfolio configuration

        #region DataGridView portfolio footer configuration

        /// <summary>
        /// This function configures the dgvPortfolioFooter (like row style)
        /// </summary>
        private void DgvPortfolioFooterConfiguration()
        {
            if (_bInitFlag)
            {
                try
                {
                    DataGridViewTextBoxColumn dgvscLabelCostDividend = new DataGridViewTextBoxColumn();
                    DataGridViewTextBoxColumn dgvscCostsDividend = new DataGridViewTextBoxColumn();
                    DataGridViewTextBoxColumn dgvscLabelTotal = new DataGridViewTextBoxColumn();
                    DataGridViewTextBoxColumn dgvscPerformance = new DataGridViewTextBoxColumn();
                    DataGridViewTextBoxColumn dgvscTotal = new DataGridViewTextBoxColumn();

                    dgvPortfolioFooter.Columns.Add(dgvscLabelCostDividend);
                    dgvPortfolioFooter.Columns.Add(dgvscCostsDividend);
                    dgvPortfolioFooter.Columns.Add(dgvscLabelTotal);
                    dgvPortfolioFooter.Columns.Add(dgvscPerformance);
                    dgvPortfolioFooter.Columns.Add(dgvscTotal);

                    dgvPortfolioFooter.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

                    // Does resize the row to display the complete content
                    dgvPortfolioFooter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                    dgvPortfolioFooter.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dgvPortfolioFooter.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dgvPortfolioFooter.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dgvPortfolioFooter.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                    dgvPortfolioFooter.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("DgvPortfolioFooterConfiguration\n\n" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    _bInitFlag = false;

                    // Add statue message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter_Error/ConfigurationFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                }
            }
        }

        #endregion DataGridView portfolio footer configuration

        #region DataGridView portfolio and portfolio footer selection changed

        /// <summary>
        /// This function loads the selected share to the share details
        /// when the selection in the dgvPortfolio has been changed
        /// </summary>
        /// <param name="sender">Selected item</param>
        /// <param name="e">EventArgs</param>
        private void DgvPortfolio_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Save current selected ShareObject via the selected WKN number
                if (dgvPortfolio.SelectedRows.Count == 1 &&
                    dgvPortfolio.SelectedRows[0].Cells.Count > 0 &&
                    dgvPortfolio.SelectedRows[0].Cells[0].Value != null&&
                    _bAddFlag == false)
                {
                    // Get selected WKN number
                    string selectedWKNNumber = dgvPortfolio.SelectedRows[0].Cells[0].Value.ToString();

                    // Search for the selected ShareObject in the ShareObject list
                    foreach (var shareObject in ShareObjectList)
                    {
                        if (shareObject.Wkn == selectedWKNNumber)
                        {
                            _shareObject = shareObject;
                            break;
                        }
                    }

                    if (_shareObject != null)
                    {
                        UpdateShareDetails();
                        UpdateDividendDetails();
                        UpdateCostsDetails();
                        UpdateProfitLossDetails();
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dgvPortfolio_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/Errors/SelectionChangeFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        // TODO Exception
        /// <summary>
        /// Function which deselects the first row of the DataGridView footer
        /// </summary>
        /// <param name="sender">DataGirdView for the footer</param>
        /// <param name="e">EventArgs</param>
        private void DataGridViewSharePortfolioFooter_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPortfolioFooter.SelectedRows.Count > 0)
                dgvPortfolioFooter.SelectedRows[0].Selected = false;
        }

        #endregion DataGridView portfolio and portfolio footer selection changed 

        #region DataGridView portfolio and portfolio footer resize

        /// <summary>
        /// This function resizes the columns when the
        /// dgvPortfolio has been resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvPortfolio_Resize(object sender, EventArgs e)
        {
            OnResizeDataGridView();
        }

        /// <summary>
        /// This function resizes the columns when the
        /// dgvPortfolioFooter has been resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvPortfolioFooter_Resize(object sender, EventArgs e)
        {
            OnResizeDataGridView();
        }

        /// <summary>
        /// This function does a column resize of the dgvPortfolio and dgvPortfolioFooter
        /// </summary>
        private void OnResizeDataGridView()
        {
            try
            {
                #region dgvPortfolio column resize

                if (dgvPortfolio.ColumnCount >= (int)ColumnIndicesPortfolio.EShareNameColumnIndex + 1)
                {
                    var _columnSum = WKNNumber + SharePrice + ShareVolume + SharePerformance + ImagePerformance +
                                     SharePerformanceDayBefore + ShareCostsDividend + ShareSum;

                    var vScrollbar = dgvPortfolio.Controls.OfType<VScrollBar>().First();
                    var vScrollbarWidth = 0;
                    if (vScrollbar.Visible)
                    {
                        vScrollbarWidth = vScrollbar.Width;
                    }
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareNameColumnIndex].Width =
                        dgvPortfolio.Width - (_columnSum + 3 + vScrollbarWidth);
                }

                #endregion dgvPortfolio column resize

                #region dgvPortfolioFooter column resize

                if (dgvPortfolioFooter.ColumnCount >= 5)
                {
                    var _columnLabelCostsDividendWidth = ShareCostsDividend + SharePrice + SharePerformanceDayBefore + ImagePerformance + SharePerformance + ShareSum;
                    var _columnLabelTotalWidth = SharePrice + SharePerformanceDayBefore + ImagePerformance;

                    var vScrollbarPortfolio = dgvPortfolio.Controls.OfType<VScrollBar>().First();
                    var vScrollbarWidth = 0;
                    if (vScrollbarPortfolio.Visible)
                    {
                        vScrollbarWidth = vScrollbarPortfolio.Width;
                        dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ESumColumnIndex].Width = ShareSum + vScrollbarWidth;
                    }
                    else
                    {
                        dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ESumColumnIndex].Width =
                            ShareSum;
                    }

                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ELabelCostsDividendIndex].Width =
                        dgvPortfolioFooter.Width - (_columnLabelCostsDividendWidth + 3 + vScrollbarWidth);

                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ECostsDividendIndex].Width = ShareCostsDividend;

                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ELabelTotalColumnIndex].Width = _columnLabelTotalWidth;
                }

                #endregion dgvPortfolioFooter column resize
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message, @"Error - OnResizeDataGridView()", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            }
        }

        #endregion DataGridView portfolio and portfolio footer resize

        #region Add shares to DataGridView portfolio and configure DataGridView footer 

        /// <summary>
        /// This function add the shares to the DataGridView portfolio.
        /// </summary>
        void AddSharesToDataGridView()
        {
            if (_bInitFlag)
            {
                try
                {
                    // Check if the share object list has items
                    if (ShareObjectList != null && ShareObjectList.Count > 0)
                    {
                        _dgvPortfolioBindingSource.ResetBindings(false);
                        _dgvPortfolioBindingSource.DataSource = ShareObjectList;
                        dgvPortfolio.DataSource = _dgvPortfolioBindingSource;
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("AddSharesToDataGridView()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    _bInitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/DgvPortfolio_Error/DisplayFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                }
            }
        }

        /// <summary>
        /// This function configures and adds the footer rows in the DataGridView portfolio footer.
        /// </summary>
        private void AddShareFooter()
        {
            if (_bInitFlag)
            {
                try
                {
                    // Check if the share object list has items
                    if (ShareObjectList != null && ShareObjectList.Count > 0 && dgvPortfolio.Rows.Count > 0)
                    {
                        // Set row colors
                        if (dgvPortfolio.RowsDefaultCellStyle.BackColor == Color.LightGray && dgvPortfolio.RowCount % 2 == 0)
                        {
                            dgvPortfolioFooter.RowsDefaultCellStyle.BackColor = Color.LightGray;
                            dgvPortfolioFooter.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                        }
                        else
                        {
                            dgvPortfolioFooter.RowsDefaultCellStyle.BackColor = Color.White;
                            dgvPortfolioFooter.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                        }

                        // Create row content with total deposit of the portfolio
                        object[] newFooterDepositOfAllShares = new object[]
                        {
                            @"",
                            @"",
                            string.Format(@"{0}", _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalDeposit", _languageName)),
                            @"",
                            ShareObjectList[0].PortfolioDepositAsStrUnit
                        };

                        // Create row content with total performance of the portfolio
                        object[] newFooterDepotDevelopment = new object[]
                        {
                            string.Format(@"{0}", _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalCostDividend", _languageName)),
                            ShareObjectList[0].CostsDividendPortfolioFinalValueAsStr,
                            string.Format(@"{0}", _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalPerformance", _languageName)),
                            ShareObjectList[0].ProfitLosePerformancePortfolioFinalValueAsStr,
                            @""
                        };

                        // Create row content with total sum of the portfolio
                        object[] newFooterTotalDepotVolume = new object[]
                        {
                            @"",
                            @"",
                            string.Format(@"{0}", _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter/RowContent_TotalSum", _languageName)),
                            @"",
                            ShareObjectList[0].PortfolioFinalValueAsStrUnit
                        };

                        // Rows to the DataGridView portfolio footer
                        dgvPortfolioFooter.Rows.Add(newFooterDepositOfAllShares);
                        dgvPortfolioFooter.Rows.Add(newFooterDepotDevelopment);
                        dgvPortfolioFooter.Rows.Add(newFooterTotalDepotVolume);
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("AddShareFooter()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Set initialization flag
                    _bInitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter_Error/ConfigurationFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                }
            }
        }

        #endregion Add shares to DataGridView portfolio and configure DataGridView footer

        #region DataBinding DataGridView portfolio

        // TODO Exception is thrown three times...
        /// <summary>
        /// This function does DataGridView portfolio configuration when the data binding is done.
        /// </summary>
        /// <param name="sender">DataGridView portfolio</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DataGridViewSharePortfolio_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers and width
                // and resize the DataGridView portfolio
                // At least do the row selection
                if (dgvPortfolio.Columns.Count == (int)ColumnIndicesPortfolio.EShareSumColumnIndex + 1)
                {
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EWknNumberColumnIndex].Name =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_WKN", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EWknNumberColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_WKN", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareNameColumnIndex].Name =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Name", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareNameColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Name", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareVolumeColumnIndex].Name =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Volume", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareVolumeColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Volume", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareCostsDividendColumnIndex].Name =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_CostsDividend", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareCostsDividendColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_CostsDividend", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePriceColumnIndex].Name =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Price", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePriceColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Price", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceDayBeforeColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_PrevDay", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EImagePerformanceColumnIndex].HeaderText = @"";
                    
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceColumnIndex].Name =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Performance", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_Performance", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareSumColumnIndex].Name =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_DepositSum", _languageName);
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareSumColumnIndex].HeaderText =
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/DgvPortfolio/ColHeader_DepositSum", _languageName);

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EWknNumberColumnIndex].Width = WKNNumber;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePriceColumnIndex].Width = SharePrice;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareVolumeColumnIndex].Width = ShareVolume;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareCostsDividendColumnIndex].Width = ShareCostsDividend;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceDayBeforeColumnIndex].Width = SharePerformanceDayBefore;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EImagePerformanceColumnIndex].Width = ImagePerformance;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceColumnIndex].Width = SharePerformance;

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareSumColumnIndex].Width = ShareSum;

                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EWknNumberColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareNameColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePriceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareVolumeColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceDayBeforeColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EImagePerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareCostsDividendColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.ESharePerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolio.Columns[(int)ColumnIndicesPortfolio.EShareSumColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ECostsDividendIndex].Width = ShareCostsDividend;
                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.EPerformanceColumnIndex].Width = SharePerformance;
                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ESumColumnIndex].Width = ShareSum;

                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ELabelCostsDividendIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ECostsDividendIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ELabelTotalColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.EPerformanceColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvPortfolioFooter.Columns[(int)ColumnIndicesPortfolioFooter.ESumColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    // Resize DataGridView portfolio
                    OnResizeDataGridView();

                    if (_bAddFlag == false && _bDeleteFlag == false)
                    {
                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolio, _selectedIndex, _lastFirstDisplayedRowIndex);
                    }

                    // When a share has been added clear the DataGridView portfolio selection
                    // and select the last row. At least make an update of the added share
                    if (_bAddFlag == true)
                    {
                        // Reset flag
                        _bAddFlag = false;

                        dgvPortfolio.ClearSelection();
                        //_selectedIndex = dgvPortfolio.Rows.Count - 1;
                        _selectedIndex = _shareObjectsList.IndexOf(ShareObject);
                        dgvPortfolio.Rows[_selectedIndex].Selected = true;

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(dgvPortfolio, _selectedIndex, _lastFirstDisplayedRowIndex);

                        if (dgvPortfolio.RowCount == 1)
                        {
                            if (_bInitFlag == false)
                                _bInitFlag = true;
                            // Add portfolio footer
                            AddShareFooter();
                        }
                        else
                            // Refresh the footer DataGridView
                            RefreshFooter();

                        // Update the new share
                        btnRefresh.PerformClick();
                    }

                    // Make an update of the footer
                    if (_bEditFlag == true)
                    {
                        // Reset flag
                        _bEditFlag = false;

                        // Refresh the footer DataGridView
                        RefreshFooter();
                    }

                    // When a share has been deleted clear the DataGridView portfolio selection
                    // and select the first row if rows still present.
                    if (_bDeleteFlag == true)
                    {
                        // Reset flag
                        _bDeleteFlag = false;

                        dgvPortfolio.ClearSelection();
                        if (dgvPortfolio.Rows.Count > 0)
                        {
                            _selectedIndex = 0;
                            dgvPortfolio.Rows[_selectedIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolio, _selectedIndex, _lastFirstDisplayedRowIndex, true);
                        }
                        else
                        {
                            // Clear portfolio footer
                            dgvPortfolioFooter.Rows.Clear();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSharePortfolio_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/DgvPortfolio_Error/DisplayFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Databinding DataGridView portfolio

        #region Cell formatting and painting for DataGridView portfolio and portfolio footer

        /// <summary>
        /// This function formats the cells in the DataGridView portfolio
        /// </summary>
        /// <param name="sender">DataGridView portfolio</param>
        /// <param name="e">DataGridViewCellFormattingEventArgs</param>
        private void DataGridViewSharePortfolio_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                var splitString = e.Value.ToString().Split(' ');
                if (e.ColumnIndex == (int)ColumnIndicesPortfolio.ESharePerformanceDayBeforeColumnIndex || e.ColumnIndex == (int)ColumnIndicesPortfolio.ESharePerformanceColumnIndex)
                {
                    if ((Convert.ToDecimal(splitString[0])) > 0)
                    {
                        e.CellStyle.ForeColor = Color.Green;
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
                MessageBox.Show("dataGridViewSharePortfolio_CellFormatting()\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/DgvPortfolio_Error/CellFormattingFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function formats the cells in the DataGridView portfolio footer
        /// </summary>
        /// <param name="sender">DataGridView portfolio footer</param>
        /// <param name="e">DataGridViewCellFormattingEventArgs</param>
        private void DataGridViewSharePortfolioFooter_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex == 1)
                {
                    var splitString = e.Value.ToString().Split(' ');

                    if (e.ColumnIndex == (int)ColumnIndicesPortfolioFooter.EPerformanceColumnIndex)
                    {
                        // Remove '%' or 'invalid' from the cell value for converting it into a decimal
                        // 'invalid' only appears when the 
                        if ((Convert.ToDecimal(splitString[0])) > 0)
                        {
                            e.CellStyle.ForeColor = Color.Green;
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
                MessageBox.Show("dataGridViewSharePortfolioFooter_CellFormatting()\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter_Error/CellFormattingFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function draws vertical column borders in the DataGridView portfolio
        /// </summary>
        /// <param name="sender">Cell of the DataGridView portfolio</param>
        /// <param name="e">DataGridViewCellPaintingEventArgs</param>
        private void DataGridViewSharePortfolio_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != (int)ColumnIndicesPortfolio.ESharePerformanceDayBeforeColumnIndex && e.ColumnIndex < dgvPortfolio.Columns.Count - 1)
                {
                    SolidBrush brush = new SolidBrush(dgvPortfolio.ColumnHeadersDefaultCellStyle.BackColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    brush.Dispose();
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolio.GridColor, 0, ButtonBorderStyle.None,
                        dgvPortfolio.GridColor, 0,
                        ButtonBorderStyle.None, dgvPortfolio.GridColor, 1,
                        ButtonBorderStyle.Solid, dgvPortfolio.GridColor, 1,
                        ButtonBorderStyle.Solid);

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSharePortfolio_CellPainting()\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/DgvPortfolio_Error/CellPaintingBorderFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function draws vertical column borders in the DataGridView portfolio footer
        /// </summary>
        /// <param name="sender">Cell of the DataGridView portfolio footer</param>
        /// <param name="e">DataGridViewCellPaintingEventArgs</param>
        private void DataGridViewSharePortfolioFooter_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == 2)
                {
                    if (e.ColumnIndex < dgvPortfolioFooter.Columns.Count - 1)
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooter.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooter.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, 2, ButtonBorderStyle.Solid,
                            dgvPortfolioFooter.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooter.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                    else
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooter.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooter.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, 2, ButtonBorderStyle.Solid,
                            dgvPortfolioFooter.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooter.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
                else
                {
                    if (e.ColumnIndex < dgvPortfolioFooter.Columns.Count - 1)
                    {
                        SolidBrush brush =
                            new SolidBrush(dgvPortfolioFooter.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooter.GridColor, 0,
                            ButtonBorderStyle.None, dgvPortfolioFooter.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooter.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooter.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSharePortfolioFooter_CellPainting()\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter_Error/CellPaintingBorderFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Cell formatting and painting for DataGridView portfolio and portfolio footer

        #region Refresh DataGridView portfolio footer

        /// <summary>
        /// This function refreshes the DataGridView portfolio footer.
        /// </summary>
        private void RefreshFooter()
        {
            try
            {
                // Set colors correctly corresponding to the DataGridView portfolio row colors
                if (dgvPortfolio.RowsDefaultCellStyle.BackColor == Color.LightGray && dgvPortfolio.RowCount % 2 == 0)
                {
                    dgvPortfolioFooter.RowsDefaultCellStyle.BackColor = Color.LightGray;
                    dgvPortfolioFooter.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dgvPortfolioFooter.RowsDefaultCellStyle.BackColor = Color.White;
                    dgvPortfolioFooter.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                }

                if (dgvPortfolioFooter.Rows.Count == 3)
                {
                    // Set deposit of all shares
                    dgvPortfolioFooter.Rows[0].Cells[
                        (int)ColumnIndicesPortfolioFooter.ESumColumnIndex].Value = ShareObjectList[0].PortfolioDepositAsStrUnit;

                    // Set costs and dividend of all shares
                    dgvPortfolioFooter.Rows[1].Cells[
                        (int)ColumnIndicesPortfolioFooter.ECostsDividendIndex].Value = ShareObjectList[0].CostsDividendPortfolioFinalValueAsStr;

                    // Set performance of all shares
                    dgvPortfolioFooter.Rows[1].Cells[
                        (int)ColumnIndicesPortfolioFooter.EPerformanceColumnIndex].Value = ShareObjectList[0].ProfitLosePerformancePortfolioFinalValueAsStr;

                    // Set value of the shares
                    dgvPortfolioFooter.Rows[2].Cells[
                        (int)ColumnIndicesPortfolioFooter.ESumColumnIndex].Value = ShareObjectList[0].PortfolioFinalValueAsStrUnit;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("RefreshFooter()\n\n" + ex.Message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Set initialization flag
                _bInitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/DgvPortfolioFooter_Error/RefreshFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Refresh DataGridView portfolio footer

        #endregion Methods
    }
}