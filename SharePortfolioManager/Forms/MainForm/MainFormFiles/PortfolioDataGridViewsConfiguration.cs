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
//#define DEBUG_MAIN_FRM_PORTFOLIO_DGV_CONFIG

using Microsoft.VisualBasic;
using SharePortfolioManager.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Portfolio data grid view

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
                dgvPortfolioMarketValue.EnableHeadersVisualStyles = false;
                dgvPortfolioMarketValue.ColumnHeadersHeight = 50;

                var styleMarketValue = dgvPortfolioMarketValue.ColumnHeadersDefaultCellStyle;
                styleMarketValue.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleMarketValue.BackColor = DataGridViewHelper.DataGridViewHeaderColors;
                styleMarketValue.SelectionBackColor = DataGridViewHelper.DataGridViewHeaderColors;


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
                dgvPortfolioFinalValue.EnableHeadersVisualStyles = false;
                dgvPortfolioFinalValue.ColumnHeadersHeight = 50;

                var styleFinaleValue = dgvPortfolioFinalValue.ColumnHeadersDefaultCellStyle;
                styleFinaleValue.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleFinaleValue.BackColor = DataGridViewHelper.DataGridViewHeaderColors;
                styleFinaleValue.SelectionBackColor = DataGridViewHelper.DataGridViewHeaderColors;

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
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/ConfigurationFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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

                var dgvLabelMarketValue = new DataGridViewTextBoxColumn();
                var dgvPerformanceMarketValue = new DataGridViewTextBoxColumn();
                var dgvMarketValue = new DataGridViewTextBoxColumn();
                var dgvCompletePerformanceMarketValue = new DataGridViewTextBoxColumn();
                var dgvCompleteMarketValue = new DataGridViewTextBoxColumn();

                dgvPortfolioFooterMarketValue.Columns.Add(dgvLabelMarketValue);
                dgvPortfolioFooterMarketValue.Columns.Add(dgvPerformanceMarketValue);
                dgvPortfolioFooterMarketValue.Columns.Add(dgvMarketValue);
                dgvPortfolioFooterMarketValue.Columns.Add(dgvCompletePerformanceMarketValue);
                dgvPortfolioFooterMarketValue.Columns.Add(dgvCompleteMarketValue);

                dgvPortfolioFooterMarketValue.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

                // Does resize the row to display the complete content
                dgvPortfolioFooterMarketValue.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

                dgvPortfolioFooterMarketValue.RowsDefaultCellStyle.BackColor = Color.LightGray;
                dgvPortfolioFooterMarketValue.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                dgvPortfolioFooterMarketValue.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dgvPortfolioFooterMarketValue.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dgvPortfolioFooterMarketValue.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                #endregion dgvPortfolioFooterMarketValue

                #region dgvPortfolioFooterFinalValue

                var dgvLabelBrokerageDividendFinalValue = new DataGridViewTextBoxColumn();
                var dgvBrokerageDividendFinalValue = new DataGridViewTextBoxColumn();
                var dgvLabelFinalValue = new DataGridViewTextBoxColumn();
                var dgvPerformanceFinalValue = new DataGridViewTextBoxColumn();
                var dgvFinalValue = new DataGridViewTextBoxColumn();
                var dgvCompletePerformanceFinalValue = new DataGridViewTextBoxColumn();
                var dgvCompleteFinalValue = new DataGridViewTextBoxColumn();

                dgvPortfolioFooterFinalValue.Columns.Add(dgvLabelBrokerageDividendFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvBrokerageDividendFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvLabelFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvPerformanceFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvCompletePerformanceFinalValue);
                dgvPortfolioFooterFinalValue.Columns.Add(dgvCompleteFinalValue);

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
                // Set initialization flag
                InitFlag = false;

                // Add statue message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolioFooter_Error/ConfigurationFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Data grid view portfolio footer configuration

        #region Set data grid view market value column headers names and texts

        /// <summary>
        /// This functions sets the column header names of the data grid view "Market value"
        /// </summary>
        private void OnSetDgvPortfolioMarketValueColumnHeaderNames()
        {
            try
            {
                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_WKN",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ENameColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Name",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EVolumeColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Volume",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPriceColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Price",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformancePrevDayColumnIndex]
                        .Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_PrevDay",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Performance",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EMarketValueColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex]
                        .Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_CompletePerformance",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ECompleteMarketValueColumnIndex]
                        .Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_CompletePurchaseMarketValue",
                        LanguageName);
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/ColumnsHeaderCaptionFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This functions sets the column header captions of the data grid view "Market value"
        /// </summary>
        private void OnSetDgvPortfolioMarketValueColumnHeaderCaptions()
        {
            try
            {

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EWknColumnIndex]
                        .HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_WKN",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.ENameColumnIndex]
                        .HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Name",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EVolumeColumnIndex]
                        .HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Volume",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPriceColumnIndex]
                        .HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Price",
                        LanguageName);

                dgvPortfolioMarketValue
                        .Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformancePrevDayColumnIndex]
                        .HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_PrevDay",
                        LanguageName);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_Performance",
                            LanguageName), "\\n", Environment.NewLine);

                dgvPortfolioMarketValue.Columns[(int) ColumnIndicesPortfolioMarketValue.EMarketValueColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue",
                            LanguageName), "\\n", Environment.NewLine);

                dgvPortfolioMarketValue
                        .Columns[(int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_CompletePerformance",
                            LanguageName), "\\n", Environment.NewLine);

                dgvPortfolioMarketValue
                        .Columns[(int) ColumnIndicesPortfolioMarketValue.ECompleteMarketValueColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgMarketDepotValue/DgvPortfolio/ColHeader_CompletePurchaseMarketValue",
                            LanguageName), "\\n", Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/ColumnsHeaderCaptionFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Set data grid view market value column headers names and texts

        #region Set data grid view complete value column headers names and texts

        /// <summary>
        /// This functions sets the column header names of the data grid view "Complete depot"
        /// </summary>
        private void OnSetDgvPortfolioFinalValueColumnHeaderNames()
        {
            try
            {
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_WKN",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ENameColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Name",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EVolumeColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Volume",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EBrokerageDividendColumnIndex]
                        .Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_BrokerageDividend",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPriceColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Price",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformancePrevDayColumnIndex]
                        .Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PrevDay",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Performance",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EFinalValueColumnIndex].Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex]
                        .Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_CompletePerformance",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompleteFinalValueColumnIndex]
                        .Name =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_CompletePurchaseMarketValue",
                        LanguageName);
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/ColumnsHeaderNameFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This functions sets the column header texts of the data grid view "Complete depot"
        /// </summary>
        private void OnSetDgvPortfolioFinalValueColumnHeaderTexts()
        {
            try
            {
                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EWknColumnIndex].HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_WKN",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ENameColumnIndex].HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Name",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EVolumeColumnIndex].HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Volume",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EBrokerageDividendColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_BrokerageDividend",
                            LanguageName), "\\n", Environment.NewLine);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPriceColumnIndex].HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Price",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformancePrevDayColumnIndex]
                        .HeaderText =
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PrevDay",
                        LanguageName);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_Performance",
                            LanguageName), "\\n", Environment.NewLine);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.EFinalValueColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_PurchaseMarketValue",
                            LanguageName), "\\n", Environment.NewLine);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_CompletePerformance",
                            LanguageName), "\\n", Environment.NewLine);

                dgvPortfolioFinalValue.Columns[(int) ColumnIndicesPortfolioFinalValue.ECompleteFinalValueColumnIndex]
                        .HeaderText =
                    Strings.Replace(
                        Language.GetLanguageTextByXPath(
                            @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/TabPgCompleteDepotValue/DgvPortfolio/ColHeader_CompletePurchaseMarketValue",
                            LanguageName), "\\n", Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/ColumnsHeaderCaptionFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Set data grid view complete value column headers names and texts

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
                if (e.Value == null) return;

                var splitString = e.Value.ToString().Split(' ');
                if (e.ColumnIndex != (int) ColumnIndicesPortfolioMarketValue.EPerformancePrevDayColumnIndex &&
                    e.ColumnIndex != (int) ColumnIndicesPortfolioMarketValue.EPerformanceColumnIndex &&
                    e.ColumnIndex != (int) ColumnIndicesPortfolioMarketValue.ECompletePerformanceColumnIndex) return;

                if (Convert.ToDecimal(splitString[0]) > 0)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (Convert.ToDecimal(splitString[0]) == 0)
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/CellFormattingFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                if (e.Value == null) return;

                var splitString = e.Value.ToString().Split(' ');
                if (e.ColumnIndex != (int) ColumnIndicesPortfolioFinalValue.EPerformancePrevDayColumnIndex &&
                    e.ColumnIndex != (int) ColumnIndicesPortfolioFinalValue.EPerformanceColumnIndex &&
                    e.ColumnIndex != (int) ColumnIndicesPortfolioFinalValue.ECompletePerformanceColumnIndex) return;

                if (Convert.ToDecimal(splitString[0]) > 0)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (Convert.ToDecimal(splitString[0]) == 0)
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/CellFormattingFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                if (e.RowIndex != 1) return;

                var splitString = e.Value.ToString().Split(' ');

                if (e.ColumnIndex != (int) ColumnIndicesPortfolioFooterMarketValue.EPerformanceColumnIndex &&
                    e.ColumnIndex != (int) ColumnIndicesPortfolioFooterMarketValue.ECompletePerformanceColumnIndex
                ) return;

                // Remove '%' or 'invalid' from the cell value for converting it into a decimal
                // 'invalid' only appears when the "%" unit is invalid
                if (Convert.ToDecimal(splitString[0]) > 0)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (Convert.ToDecimal(splitString[0]) == 0)
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolioFooter_Error/CellFormattingFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                if (e.RowIndex != 1) return;

                var splitString = e.Value.ToString().Split(' ');

                if (e.ColumnIndex != (int) ColumnIndicesPortfolioFooterFinalValue.EPerformanceColumnIndex &&
                    e.ColumnIndex != (int) ColumnIndicesPortfolioFooterFinalValue.ECompletePerformanceColumnIndex
                ) return;

                // Remove '%' or 'invalid' from the cell value for converting it into a decimal
                // 'invalid' only appears when the "%" unit is invalid
                if (Convert.ToDecimal(splitString[0]) > 0)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (Convert.ToDecimal(splitString[0]) == 0)
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolioFooter_Error/CellFormattingFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                var brush = new SolidBrush(dgvPortfolioMarketValue.ColumnHeadersDefaultCellStyle.BackColor);
                e.Graphics.FillRectangle(brush, e.CellBounds);
                brush.Dispose();
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioMarketValue.GridColor, 0,
                    ButtonBorderStyle.None,
                    dgvPortfolioMarketValue.GridColor, 0,
                    ButtonBorderStyle.None, dgvPortfolioMarketValue.GridColor, 1,
                    ButtonBorderStyle.Solid, dgvPortfolioMarketValue.GridColor, 1,
                    ButtonBorderStyle.Solid);

                e.Handled = true;
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/CellPaintingBorderFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                var brush = new SolidBrush(dgvPortfolioFinalValue.ColumnHeadersDefaultCellStyle.BackColor);
                e.Graphics.FillRectangle(brush, e.CellBounds);
                brush.Dispose();
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFinalValue.GridColor, 0,
                    ButtonBorderStyle.None,
                    dgvPortfolioFinalValue.GridColor, 0,
                    ButtonBorderStyle.None, dgvPortfolioFinalValue.GridColor, 1,
                    ButtonBorderStyle.Solid, dgvPortfolioFinalValue.GridColor, 1,
                    ButtonBorderStyle.Solid);

                e.Handled = true;
            }
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolio_Error/CellPaintingBorderFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                        var brush =
                            new SolidBrush(dgvPortfolioFooterMarketValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterMarketValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, FooterBorderWidth, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterMarketValue.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterMarketValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                    else
                    {
                        var brush =
                            new SolidBrush(dgvPortfolioFooterMarketValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterMarketValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, FooterBorderWidth, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
                else
                {
                    if (e.ColumnIndex >= dgvPortfolioFooterMarketValue.Columns.Count - 1) return;

                    var brush =
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
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolioFooter_Error/CellPaintingBorderFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                        var brush =
                            new SolidBrush(dgvPortfolioFooterFinalValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterFinalValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, FooterBorderWidth, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                    else
                    {
                        var brush =
                            new SolidBrush(dgvPortfolioFooterFinalValue.ColumnHeadersDefaultCellStyle.BackColor);
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        brush.Dispose();
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        ControlPaint.DrawBorder(e.Graphics, e.CellBounds, dgvPortfolioFooterFinalValue.GridColor, 0,
                            ButtonBorderStyle.None, Color.Black, FooterBorderWidth, ButtonBorderStyle.Solid,
                            dgvPortfolioFooterFinalValue.GridColor, 0, ButtonBorderStyle.None,
                            dgvPortfolioFooterFinalValue.GridColor, 1, ButtonBorderStyle.Solid);

                        e.Handled = true;
                    }
                }
                else
                {
                    if (e.ColumnIndex >= dgvPortfolioFooterFinalValue.Columns.Count - 1) return;

                    var brush =
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
            catch (Exception ex)
            {
                // Set initialization flag
                InitFlag = false;

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(
                        @"/MainForm/GrpBoxPortfolio/TabCtrlShareOverviews/DgvPortfolioFooter_Error/CellPaintingBorderFailed",
                        LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Cell formatting and painting for the data grid views and footers

        #endregion Portfolio data grid view
    }
}