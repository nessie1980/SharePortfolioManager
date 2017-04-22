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
            if (btnRefreshAll.Text == _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", _languageName))
            {
                _bUpdateAll = true;

                // Reset row index
                _selectedIndex = 0;

                // Start refresh
                RefreshAll(sender, e);
            }
            else
            {
                // Cancel update process
                CancelWebParser(sender, e);
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
            if (btnRefresh.Text == _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", _languageName))
            {

                // Set row index
                _selectedIndex = dgvPortfolio.SelectedRows[0].Index;

                // Refresh share
                Refresh(sender, e);
            }
            else
            {
                // Cancel update process
                CancelWebParser(sender, e);
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
                List<string> additionalButtons = new List<string>();

                // Create add share form
                IModelShareAdd model = new ModelShareAdd();
                IViewShareAdd view = new ViewShareAdd(this, _logger, _xmlLanguage, _languageName, _webSiteRegexList);
                PresenterShareAdd presenterBuyEdit = new PresenterShareAdd(view, model);

                if (view.ShowDialog() == DialogResult.OK)
                {
                    // Set add flag to true for selecting the new added share in the DataGridView portfolio
                    _bAddFlag = true;

                    // Save the share values to the XML
                    Exception exception = null;
                    if (Helper.SaveShareObject(ShareObject, ref _xmlPortfolio, ref _xmlReaderPortfolio, ref _xmlReaderSettingsPortfolio, PortfolioFileName, out exception))
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                           _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/AddSaveSuccessful", _languageName),
                           _xmlLanguage, _languageName,
                           Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                        // Enable controls
                        additionalButtons.Add("btnRefreshAll");
                        additionalButtons.Add("btnRefresh");
                        additionalButtons.Add("btnEdit");
                        additionalButtons.Add("btnDelete");
                        additionalButtons.Add("btnClearLogger");
                        Helper.EnableDisableControls(true, this, additionalButtons);
                    }
                    else
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                           _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/AddSaveFailed", _languageName),
                           _xmlLanguage, _languageName,
                           Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);

                        // Enable buttons only if a share exists in the DataGridView
                        if (dgvPortfolio.RowCount > 0)
                        {
                            // Enable controls
                            additionalButtons.Add("btnRefreshAll");
                            additionalButtons.Add("btnRefresh");
                            additionalButtons.Add("btnEdit");
                            additionalButtons.Add("btnDelete");
                            additionalButtons.Add("btnClearLogger");
                            Helper.EnableDisableControls(true, this, additionalButtons);
                        }
                    }

                    // Check if the DataBinding is already done and
                    // than set the new share to the DataGridview
                    if (_dgvPortfolioBindingSource.DataSource == null && dgvPortfolio.DataSource == null)
                    {
                        _dgvPortfolioBindingSource.DataSource = ShareObjectList;
                        dgvPortfolio.DataSource = _dgvPortfolioBindingSource;
                    }
                    else
                        _dgvPortfolioBindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnAdd_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/AddSaveErrorFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
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
                // Check if a share is selected
                if (dgvPortfolio.SelectedCells[0].Value != null && dgvPortfolio.SelectedCells[0].Value.ToString() != "")
                {
                    var editShare = new FrmShareEdit(this, _logger, XmlLanguage, dgvPortfolio.SelectedCells[0].Value.ToString(), _languageName);

                    if (editShare.ShowDialog() == DialogResult.OK)
                    {
                        // Set delete flag
                        _bEditFlag = true;

                        // Save the share values to the XML
                        Exception exception = null;

                        // Get WKN of the selected object
                        ShareObject tempShareObject = ShareObjectList[dgvPortfolio.SelectedRows[0].Index];

                        if (Helper.SaveShareObject(ShareObjectList[dgvPortfolio.SelectedRows[0].Index], ref _xmlPortfolio, ref _xmlReaderPortfolio, ref _xmlReaderSettingsPortfolio, PortfolioFileName, out exception))
                        {
                            // Sort portfolio list in order of the share names
                            ShareObjectList.Sort(new ShareObjectListComparer());

                            // Select row of the new list index
                            ShareObjectSearch searchObject = new ShareObjectSearch(tempShareObject.Wkn);
                            _selectedIndex = ShareObjectList.FindIndex(searchObject.Compare);

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/EditSaveSuccessful", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                            // Reset / refresh DataGridView portfolio BindingSource
                            _dgvPortfolioBindingSource.ResetBindings(false);

                            Console.WriteLine("Index: {0}", _selectedIndex);

                            if (_selectedIndex > 0)
                                dgvPortfolio.Rows[_selectedIndex].Selected = true;

                            // Scroll to the selected row
                            Helper.ScrollDgvToIndex(dgvPortfolio, _selectedIndex, _lastFirstDisplayedRowIndex, true);
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
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Orange, _logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnEdit_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveErrorFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deletes a share of the portfolio
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnDelete_Click(object sender, EventArgs e)
        {
            // Check if a share is selected
            if (dgvPortfolio.SelectedCells[0].Value != null && dgvPortfolio.SelectedCells[0].Value.ToString() != "")
            {
                try
                {
                    var ownDeleteMessageBox = new OwnMessageBox(_xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", _languageName), _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Content/ShareDelete", _languageName), _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes", _languageName), _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No", _languageName));
                    if (ownDeleteMessageBox.ShowDialog() == DialogResult.OK)
                    {
                        // Set delete flag
                        _bDeleteFlag = true;

                        // Delete the share from the portfolio XML file
                        XmlNode nodeDelete = _xmlPortfolio.SelectSingleNode(string.Format("//Share[@WKN='{0}']", dgvPortfolio.SelectedCells[0].Value.ToString()));
                        nodeDelete.ParentNode.RemoveChild(nodeDelete);
                        
                        // Close reader for saving
                        _xmlReaderPortfolio.Close();
                        // Save settings
                        _xmlPortfolio.Save(PortfolioFileName);
                        // Create a new reader
                        _xmlReaderPortfolio = XmlReader.Create(PortfolioFileName, _xmlReaderSettingsPortfolio);
                        _xmlPortfolio.Load(PortfolioFileName);

                        // Find index of the share which should be deleted
                        var indexRemove = -1;
                        if (_shareObject != null)
                            indexRemove = ShareObjectList.IndexOf(_shareObject);

                        // Remove share from the share list
                        if (indexRemove > -1)
                        {
                            ShareObjectList[indexRemove].Dispose();
                            ShareObjectList.RemoveAt(indexRemove);

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/DeleteSuccessful", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/DeleteFailed", _languageName),
                                _xmlLanguage, _languageName,
                                Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                        }

                        // Check if other shares exists
                        if (ShareObjectList.Count == 0)
                        {
                            grpBoxShareDetails.Text = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/GrpBoxDetails/Caption", _languageName);
                            lblShareDetailsWithDividendCostShareDateValue.Text = @"";
                            lblShareDetailsWithDividendCostShareVolumeValue.Text = @"";
                            lblShareDetailsWithDividendCostShareDividendValue.Text = @"";
                            lblShareDetailsWithDividendCostShareCostValue.Text = @"";
                            lblShareDetailsWithDividendCostSharePriceCurrentValue.Text = @"";
                            lblShareDetailsWithDividendCostShareDiffPerformancePrevValue.Text = @"";
                            lblShareDetailsWithDividendCostShareDiffSumPrevValue.Text = @"";
                            lblShareDetailsWithDividendCostSharePricePervValue.Text = @"";
                            lblShareDetailsWithDividendCostShareDepositValue.Text = @"";
                            lblShareDetailsWithDividendCostShareTotalProfitValue.Text = @"";
                            lblShareDetailsWithDividendCostShareTotalPerformanceValue.Text = @"";
                            lblShareDetailsWithDividendCostShareTotalSumValue.Text = @"";

                            lblShareDetailsWithOutDividendCostShareDateValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareVolumeValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareDividendValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareCostValue.Text = @"";
                            lblShareDetailsWithOutDividendCostSharePriceCurrentValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareDiffPerformancePrevValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareDiffSumPrevValue.Text = @"";
                            lblShareDetailsWithOutDividendCostSharePricePervValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareDepositValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareTotalProfitValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareTotalPerformanceValue.Text = @"";
                            lblShareDetailsWithOutDividendCostShareTotalSumValue.Text = @"";

                            // Reset share object portfolio values
                            ShareObject.PortfolioValuesReset();

                            // Disable buttons
                            List<string> controlList = new List<string>();
                            controlList.Add("btnRefreshAll");
                            controlList.Add("btnRefresh");
                            controlList.Add("btnEdit");
                            controlList.Add("btnDelete");
                            controlList.Add("btnClearLogger");
                            controlList.Add("tabCtrlDetails");
                            Helper.EnableDisableControls(false, this, controlList);
                        }

                        // Update DataGridView
                        _dgvPortfolioBindingSource.ResetBindings(false);
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("btnDelete_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/DeleteErrorFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                }
            }
            else
            {
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", _languageName),
                    _xmlLanguage, _languageName,
                    Color.Orange, _logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
            }
        }

        #endregion Button
    }
}
