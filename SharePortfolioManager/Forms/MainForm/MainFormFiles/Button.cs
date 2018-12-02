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
using SharePortfolioManager.Forms.ShareAddForm.Model;
using SharePortfolioManager.Forms.ShareAddForm.Presenter;
using SharePortfolioManager.Forms.ShareAddForm.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Button

        /// <summary>
        /// This function starts the update all shares process
        /// or cancels the update process
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnRefreshAll_Click(object sender, EventArgs e)
        {
            if (btnRefreshAll.Text == Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", LanguageName))
            {
                UpdateAllFlag = true;

                // Reset row index
                SelectedDataGridViewShareIndex = 0;

                // Start refresh
                RefreshAll();
            }
            else
            {
                // Cancel update process
                CancelWebParser();
            }
        }

        /// <summary>
        /// This function starts the update single share process
        /// or cancels the update process
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnRefresh_Click(object sender, EventArgs e)
        {
            if (btnRefresh.Text == Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", LanguageName))
            {
                // Set row index
                 SelectedDataGridViewShareIndex = MarketValueOverviewTabSelected ? dgvPortfolioMarketValue.SelectedRows[0].Index : dgvPortfolioFinalValue.SelectedRows[0].Index;

                // Refresh share
                Refresh();
            }
            else
            {
                // Cancel update process
                CancelWebParser();
            }
        }

        /// <summary>
        /// This function adds a new share
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var additionalButtons = new List<string>();

                // Create add share form
                IModelShareAdd model = new ModelShareAdd();
                IViewShareAdd view = new ViewShareAdd(this, Logger, Language, LanguageName, WebSiteRegexList);
                var presenterBuyEdit = new PresenterShareAdd(view, model);

                if (view.ShowDialog() != DialogResult.OK) return;

                // Set add flag to true for selecting the new added share in the DataGridView portfolio
                AddFlagMarketValue = true;
                AddFlagFinalValue = true;

                // Save the share values only of the final value object to the XML (The market value object contains the same values)
                if (ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue, ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, _portfolioFileName, out var exception))
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/AddSaveSuccessful", LanguageName),
                        Language, LanguageName,
                        Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                    // Enable controls
                    additionalButtons.Add("btnRefreshAll");
                    additionalButtons.Add("btnRefresh");
                    additionalButtons.Add("btnEdit");
                    additionalButtons.Add("btnDelete");
                    additionalButtons.Add("btnClearLogger");
                    Helper.EnableDisableControls(true, tblLayPnlShareOverviews, additionalButtons);
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/AddSaveFailed", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);

                    // Enable buttons only if a share exists in the DataGridView
                    if (dgvPortfolioFinalValue.RowCount > 0 && dgvPortfolioMarketValue.RowCount > 0)
                    {
                        // Enable controls
                        additionalButtons.Add("btnRefreshAll");
                        additionalButtons.Add("btnRefresh");
                        additionalButtons.Add("btnEdit");
                        additionalButtons.Add("btnDelete");
                        additionalButtons.Add("btnClearLogger");
                        Helper.EnableDisableControls(true, tblLayPnlShareOverviews, additionalButtons);
                    }
                }

                // Check if the DataBinding is already done and
                // than set the new share to the DataGridvVew
                if (DgvPortfolioBindingSourceFinalValue.DataSource == null && dgvPortfolioFinalValue.DataSource == null)
                {
                    DgvPortfolioBindingSourceFinalValue.DataSource = ShareObjectListFinalValue;
                    dgvPortfolioFinalValue.DataSource = DgvPortfolioBindingSourceFinalValue;
                }
                else
                    DgvPortfolioBindingSourceFinalValue.ResetBindings(false);

                if (DgvPortfolioBindingSourceMarketValue.DataSource == null && dgvPortfolioMarketValue.DataSource == null)
                {
                    DgvPortfolioBindingSourceMarketValue.DataSource = ShareObjectListMarketValue;
                    dgvPortfolioMarketValue.DataSource = DgvPortfolioBindingSourceMarketValue;
                }
                else
                    DgvPortfolioBindingSourceMarketValue.ResetBindings(false);
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"btnAdd_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                   Language.GetLanguageTextByXPath(@"/MainForm/Errors/AddSaveErrorFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the edit window for the selected share
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var wkn = @"";

                // Check if a share is selected
                if (MarketValueOverviewTabSelected &&
                    dgvPortfolioMarketValue.SelectedCells[0].Value != null &&
                    dgvPortfolioMarketValue.SelectedCells[0].Value.ToString() != ""
                   )
                {
                    wkn = dgvPortfolioMarketValue.SelectedCells[0].Value.ToString();
                    SelectedDataGridViewShareIndex = dgvPortfolioMarketValue.SelectedRows[0].Index;
                }

                if (MarketValueOverviewTabSelected == false &&
                    dgvPortfolioFinalValue.SelectedCells[0].Value != null &&
                    dgvPortfolioFinalValue.SelectedCells[0].Value.ToString() != ""
                   )
                {
                    wkn = dgvPortfolioFinalValue.SelectedCells[0].Value.ToString();
                    SelectedDataGridViewShareIndex = dgvPortfolioFinalValue.SelectedRows[0].Index;
                }

                if (wkn != @"")
                {
                    var editShare = new FrmShareEdit(this, Logger, Language, wkn, LanguageName);

                    if (editShare.ShowDialog() != DialogResult.OK) return;

                    // Set delete flag
                    EditFlagMarketValue = true;

                    // Save the share values to the XML
                    // Get WKN of the selected object
                    var tempShareObjectFinalValue = ShareObjectListFinalValue[SelectedDataGridViewShareIndex];

                    if (ShareObjectFinalValue.SaveShareObject(ShareObjectListFinalValue[SelectedDataGridViewShareIndex], ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, _portfolioFileName, out var exception))
                    {
                        // Sort portfolio list in order of the share names
                        ShareObjectListFinalValue.Sort(new ShareObjectListComparer());
                        ShareObjectListMarketValue.Sort(new ShareObjectListComparer());

                        // Select row of the new list index
                        var searchObject = new ShareObjectSearch(tempShareObjectFinalValue.Wkn);
                        SelectedDataGridViewShareIndex = ShareObjectListFinalValue.FindIndex(searchObject.Compare);

                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/EditSaveSuccessful", LanguageName),
                            Language, LanguageName,
                            Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                        // Reset / refresh DataGridView portfolio BindingSource
                        DgvPortfolioBindingSourceMarketValue.ResetBindings(false);
                        DgvPortfolioBindingSourceFinalValue.ResetBindings(false);

#if DEBUG
                        Console.WriteLine(@"Index: {0}", SelectedDataGridViewShareIndex);
#endif
                        if (SelectedDataGridViewShareIndex > 0)
                        {
                            if (MarketValueOverviewTabSelected)
                                dgvPortfolioMarketValue.Rows[SelectedDataGridViewShareIndex].Selected = true;
                            else
                                dgvPortfolioFinalValue.Rows[SelectedDataGridViewShareIndex].Selected = true;
                        }

                        // Scroll to the selected row
                        Helper.ScrollDgvToIndex(
                            MarketValueOverviewTabSelected ? dgvPortfolioMarketValue : dgvPortfolioFinalValue,
                            SelectedDataGridViewShareIndex, LastFirstDisplayedRowIndex, true);
                    }
                    else
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveFailed", LanguageName),
                            Language, LanguageName,
                            Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                    }
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                       Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", LanguageName),
                       Language, LanguageName,
                       Color.Orange, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"btnEdit_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                   Language.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveErrorFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deletes a share of the portfolio
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnDelete_Click(object sender, EventArgs e)
        {
            var wkn = @"";

            // Check if a share is selected
            if (MarketValueOverviewTabSelected == false &&
                dgvPortfolioFinalValue.SelectedCells.Count > 0 &&
                dgvPortfolioFinalValue.SelectedCells[0].Value != null &&
                dgvPortfolioFinalValue.SelectedCells[0].Value.ToString() != ""
               )
                wkn = dgvPortfolioFinalValue.SelectedCells[0].Value.ToString();

            if (MarketValueOverviewTabSelected &&
                dgvPortfolioMarketValue.SelectedCells.Count > 0 &&
                dgvPortfolioMarketValue.SelectedCells[0].Value != null &&
                dgvPortfolioMarketValue.SelectedCells[0].Value.ToString() != ""
               )
                wkn = dgvPortfolioMarketValue.SelectedCells[0].Value.ToString();

            if (wkn != @"")
            {
                try
                {
                    var ownDeleteMessageBox = new OwnMessageBox(Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName), Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/ShareDelete", LanguageName), Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes", LanguageName), Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No", LanguageName));
                    if (ownDeleteMessageBox.ShowDialog() != DialogResult.OK) return;

                    // Set delete flag
                    DeleteFlagMarketValue = true;
                    DeleteFlagFinalValue = true;

                    #region Delete from XML file

                    // Delete the share from the portfolio XML file
                    var nodeDelete = Portfolio.SelectSingleNode($"//Share[@WKN='{wkn}']");
                    nodeDelete?.ParentNode?.RemoveChild(nodeDelete);

                    // Close reader for saving
                    ReaderPortfolio.Close();
                    // Save settings
                    Portfolio.Save(_portfolioFileName);
                    // Create a new reader
                    ReaderPortfolio = XmlReader.Create(_portfolioFileName, ReaderSettingsPortfolio);
                    Portfolio.Load(_portfolioFileName);

                    #endregion Delete from XML file

                    #region Delete from share lists
                        
                    // Find index of the share which should be deleted
                    var indexRemove = -1;
                    if (MarketValueOverviewTabSelected)
                    {
                        if (ShareObjectMarketValue != null)
                            indexRemove = ShareObjectListMarketValue.IndexOf(ShareObjectMarketValue);
                    }
                    else
                    {
                        if (ShareObjectFinalValue != null)
                            indexRemove = ShareObjectListFinalValue.IndexOf(ShareObjectFinalValue);
                    }

                    // Remove share from the share list
                    if (indexRemove > -1)
                    {
                        ShareObjectListMarketValue[indexRemove].Dispose();
                        ShareObjectListMarketValue.RemoveAt(indexRemove);
                        ShareObjectMarketValue = null;

                        ShareObjectListFinalValue[indexRemove].Dispose();
                        ShareObjectListFinalValue.RemoveAt(indexRemove);
                        ShareObjectFinalValue = null;

                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/DeleteSuccessful", LanguageName),
                            Language, LanguageName,
                            Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
                    }
                    else
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/DeleteFailed", LanguageName),
                            Language, LanguageName,
                            Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                    }

                    #endregion Delete from share lists

                    // Check if other shares exists
                    if (ShareObjectListFinalValue.Count == 0 || ShareObjectListMarketValue.Count == 0)
                    {
                        grpBoxShareDetails.Text = Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/Caption", LanguageName);
                        lblDetailsFinalValueDateValue.Text = @"";
                        lblDetailsFinalValueVolumeValue.Text = @"";
                        lblDetailsFinalValueDividendValue.Text = @"";
                        lblDetailsFinalValueBrokerageValue.Text = @"";
                        lblDetailsFinalValueCurPriceValue.Text = @"";
                        lblDetailsFinalValueDiffPerformancePrevValue.Text = @"";
                        lblDetailsFinalValueDiffSumPrevValue.Text = @"";
                        lblDetailsFinalValuePrevPriceValue.Text = @"";
                        lblDetailsFinalValuePurchaseValue.Text = @"";
                        lblDetailsFinalValueTotalProfitValue.Text = @"";
                        lblDetailsFinalValueTotalPerformanceValue.Text = @"";
                        lblDetailsFinalValueTotalSumValue.Text = @"";

                        lblDetailsMarketValueDateValue.Text = @"";
                        lblDetailsMarketValueVolumeValue.Text = @"";
                        lblDetailsMarketValueDividendValue.Text = @"";
                        lblDetailsMarketValueBrokerageValue.Text = @"";
                        lblDetailsMarketValueCurPriceValue.Text = @"";
                        lblDetailsMarketValueDiffPerformancePrevValue.Text = @"";
                        lblDetailsMarketValueDiffSumPrevValue.Text = @"";
                        lblDetailsMarketValuePrevPriceValue.Text = @"";
                        lblDetailsMarketValuePurchaseValue.Text = @"";
                        lblDetailsMarketValueTotalProfitValue.Text = @"";
                        lblDetailsMarketValueTotalPerformanceValue.Text = @"";
                        lblDetailsMarketValueTotalSumValue.Text = @"";

                        // Reset share object portfolio values
                        ShareObjectFinalValue.PortfolioValuesReset();
                        ShareObjectMarketValue.PortfolioValuesReset();

                        // Disable buttons
                        var controlList = new List<string>
                        {
                            "btnRefreshAll",
                            "btnRefresh",
                            "btnEdit",
                            "btnDelete",
                            "btnClearLogger",
                            "tabCtrlDetails"
                        };
                        Helper.EnableDisableControls(false, tblLayPnlShareOverviews, controlList);
                    }

                    // Update DataGridView
                    DgvPortfolioBindingSourceMarketValue.ResetBindings(false);
                    DgvPortfolioBindingSourceFinalValue.ResetBindings(false);
                }
                catch (Exception ex)
                {
#if DEBUG
                    var message = $"btnDelete_Click()\n\n{ex.Message}";
                    MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/DeleteErrorFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                }
            }
            else
            {
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", LanguageName),
                    Language, LanguageName,
                    Color.Orange, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
            }
        }

        #endregion Button
    }
}
