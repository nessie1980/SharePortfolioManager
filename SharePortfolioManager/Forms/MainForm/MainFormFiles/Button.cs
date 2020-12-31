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
//#define DEBUG_MAIN_FORM_BUTTON

using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.OwnMessageBoxForm;
using SharePortfolioManager.ShareAddForm.Model;
using SharePortfolioManager.ShareAddForm.Presenter;
using SharePortfolioManager.ShareAddForm.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using SharePortfolioManager.Classes.Configurations;

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
            if (btnRefreshAll.Text ==
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/RefreshAll", SettingsConfiguration.LanguageName))
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
            if (btnRefresh.Text ==
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Buttons/Refresh", SettingsConfiguration.LanguageName))
            {
                // Set row index
                SelectedDataGridViewShareIndex = MarketValueOverviewTabSelected
                    ? dgvPortfolioMarketValue.SelectedRows[0].Index
                    : dgvPortfolioFinalValue.SelectedRows[0].Index;

                // Refresh share
                RefreshOne();
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
                // Create add share form
                IModelShareAdd model = new ModelShareAdd();
                IViewShareAdd view = new ViewShareAdd(this, Logger, LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    WebSiteConfiguration.WebSiteRegexList);
                // ReSharper disable once UnusedVariable
                var presenterBuyEdit = new PresenterShareAdd(view, model);

                if (view.ShowDialog() != DialogResult.OK) return;

                // Add share and update UI
                OnAddShare();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/AddSaveErrorFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
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
                    var editShare = new FrmShareEdit(this, Logger, LanguageConfiguration.Language, wkn, SettingsConfiguration.LanguageName);

                    if (editShare.ShowDialog() != DialogResult.OK) return;

                    // Set flag for edit / update flag
                    EditFlagMarketValue = true;

                    var tempShareObjectFinalValue = ShareObjectListFinalValue[SelectedDataGridViewShareIndex];

                    // Save the share values to the XML
                    if (ShareObjectFinalValue.SaveShareObject(ShareObjectListFinalValue[SelectedDataGridViewShareIndex],
                        ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, SettingsConfiguration.PortfolioName, 
                        out var exception))
                    {
                        // Sort portfolio list in order of the share names
                        ShareObjectListFinalValue.Sort(new ShareObjectListComparer());
                        ShareObjectListMarketValue.Sort(new ShareObjectListComparer());

                        // Select row of the new list index
                        var searchObject = new ShareObjectSearch(tempShareObjectFinalValue.Wkn);
                        SelectedDataGridViewShareIndex = ShareObjectListFinalValue.FindIndex(searchObject.Compare);

                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/EditSaveSuccessful",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Application);

                        // Reset / refresh DataGridView portfolio BindingSource
                        DgvPortfolioBindingSourceMarketValue.ResetBindings(false);
                        DgvPortfolioBindingSourceFinalValue.ResetBindings(false);

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
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveFailed", SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application,
                            exception);
                    }

                    // Check if any share set for updating so enable the refresh all button
                    btnRefreshAll.Enabled =
                        ShareObjectListFinalValue.Count(p => p.InternetUpdateOption != ShareObject.ShareUpdateTypes.None && p.WebSiteConfigurationFound) >= 1;

                    // Throw exception which is thrown in the SaveShareObject function
                    if (exception != null)
                        throw exception;
                }
                else
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.Orange, Logger, (int) EStateLevels.Warning, (int) EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveErrorFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function saves the changed share data. It also updates the data grid views
        /// </summary>
        public void OnEditShare(int index)
        {
            // Set delete flag
            EditFlagMarketValue = true;

            // Save the share values to the XML
            // Get WKN of the selected object
            var tempShareObjectFinalValue = ShareObjectListFinalValue[index];

            if (ShareObjectFinalValue.SaveShareObject(ShareObjectListFinalValue[index],
                ref _portfolio, ref _readerPortfolio, ref _readerSettingsPortfolio, SettingsConfiguration.PortfolioName,
                out var exception))
            {
                // Sort portfolio list in order of the share names
                ShareObjectListFinalValue.Sort(new ShareObjectListComparer());
                ShareObjectListMarketValue.Sort(new ShareObjectListComparer());

                // Select row of the new list index
                var searchObject = new ShareObjectSearch(tempShareObjectFinalValue.Wkn);
                index = ShareObjectListFinalValue.FindIndex(searchObject.Compare);

                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/EditSaveSuccessful",
                        SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                // Reset / refresh DataGridView portfolio BindingSource
                DgvPortfolioBindingSourceMarketValue.ResetBindings(false);
                DgvPortfolioBindingSourceFinalValue.ResetBindings(false);

                if (index > 0)
                {
                    if (MarketValueOverviewTabSelected)
                        dgvPortfolioMarketValue.Rows[index].Selected = true;
                    else
                        dgvPortfolioFinalValue.Rows[index].Selected = true;
                }

                // Scroll to the selected row
                Helper.ScrollDgvToIndex(
                    MarketValueOverviewTabSelected ? dgvPortfolioMarketValue : dgvPortfolioFinalValue,
                    index, LastFirstDisplayedRowIndex, true);
            }
            else
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/EditSaveFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application,
                    exception);
            }

            // Check if any share set for updating so enable the refresh all button
            btnRefreshAll.Enabled =
                ShareObjectListFinalValue.Count(p => p.InternetUpdateOption != ShareObject.ShareUpdateTypes.None && p.WebSiteConfigurationFound) >= 1;

            // Throw exception which is thrown in the SaveShareObject function
            if (exception != null)
                throw exception;
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
                    var deleteFlag = true;

                    var ownDeleteMessageBox = new OwnMessageBox(
                        LanguageConfiguration.Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                            (int) EOwnMessageBoxInfoType.Info],
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/ShareDelete", SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes", SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No", SettingsConfiguration.LanguageName),
                        EOwnMessageBoxInfoType.Info);
                    if (ownDeleteMessageBox.ShowDialog() != DialogResult.OK) return;

                    // Set delete flag
                    DeleteFlagMarketValue = true;
                    DeleteFlagFinalValue = true;
                    
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
                        #region Remove share and all objects from this share from the market list 

                        // Delete sale objects in the market list
                        for (var j = ShareObjectListMarketValue[indexRemove].AllSaleEntries.AllSalesOfTheShareDictionary
                                .Count - 1;
                            j >= 0;
                            j--)
                        {
                            var saleObjectSalesYearOfTheShare = ShareObjectListMarketValue[indexRemove].AllSaleEntries
                                .AllSalesOfTheShareDictionary.ElementAt(j);
                         
                            for (var i = saleObjectSalesYearOfTheShare.Value.SaleListYear.Count - 1; i >= 0; i--)
                            {
                                var saleObject = saleObjectSalesYearOfTheShare.Value.SaleListYear[i];

                                if (!ShareObjectListMarketValue[indexRemove]
                                    .RemoveSale(saleObject.Guid, saleObject.DateAsStr))
                                {
                                    deleteFlag = false;
                                    break;
                                }
                            }
                        }

                        // Delete buy objects in the market list
                        if (deleteFlag)
                        {
                            for (var j = ShareObjectListMarketValue[indexRemove].AllBuyEntries
                                    .AllBuysOfTheShareDictionary.Count - 1;
                                j >= 0;
                                j--)
                            {
                                var buyObjectBuysYearOfTheShare = ShareObjectListMarketValue[indexRemove]
                                    .AllBuyEntries.AllBuysOfTheShareDictionary.ElementAt(j);

                                for (var i = buyObjectBuysYearOfTheShare.Value.BuyListYear.Count - 1; i >= 0; i--)
                                {
                                    var buyObject = buyObjectBuysYearOfTheShare.Value.BuyListYear[i];

                                    if (!ShareObjectListMarketValue[indexRemove]
                                        .RemoveBuy(buyObject.Guid, buyObject.DateAsStr))
                                    {
                                        deleteFlag = false;
                                        break;
                                    }
                                }
                            }
                        }

                        ShareObjectListMarketValue[indexRemove].Dispose();
                        ShareObjectListMarketValue.RemoveAt(indexRemove);
                        ShareObjectMarketValue = null;

                        #endregion Remove share and all objects from this share from the market list 

                        #region Remove share and all objects from this share from the final list 

                        // Delete dividend objects in the final list
                        if (deleteFlag)
                        {
                            for (var j = ShareObjectListFinalValue[indexRemove].AllDividendEntries
                                    .AllDividendsOfTheShareDictionary.Count - 1;
                                j >= 0;
                                j--)
                            {
                                var dividendObjectDividendsYearOfTheShare = ShareObjectListFinalValue[indexRemove]
                                    .AllDividendEntries
                                    .AllDividendsOfTheShareDictionary.ElementAt(j);

                                for (var i =
                                        dividendObjectDividendsYearOfTheShare.Value.DividendListYear.Count - 1;
                                    i >= 0;
                                    i--)
                                {
                                    var dividendObject =
                                        dividendObjectDividendsYearOfTheShare.Value.DividendListYear[i];

                                    if (!ShareObjectListFinalValue[indexRemove]
                                        .RemoveDividend(dividendObject.Guid, dividendObject.DateAsStr))
                                    {
                                        deleteFlag = false;
                                        break;
                                    }
                                }
                            }
                        }

                        // Delete sale objects in the final list
                        if (deleteFlag)
                        {
                            for (var j = ShareObjectListFinalValue[indexRemove].AllSaleEntries
                                    .AllSalesOfTheShareDictionary.Count - 1;
                                j >= 0;
                                j--)
                            {
                                var saleObjectSalesYearOfTheShare = ShareObjectListFinalValue[indexRemove]
                                    .AllSaleEntries
                                    .AllSalesOfTheShareDictionary.ElementAt(j);

                                for (var i = saleObjectSalesYearOfTheShare.Value.SaleListYear.Count - 1; i >= 0; i--)
                                {
                                    var saleObject = saleObjectSalesYearOfTheShare.Value.SaleListYear[i];

                                    if (!ShareObjectListFinalValue[indexRemove]
                                        .RemoveSale(saleObject.Guid, saleObject.DateAsStr))
                                    {
                                        deleteFlag = false;
                                        break;
                                    }
                                }
                            }
                        }

                        // Delete buy objects in the final list
                        if (deleteFlag)
                        {
                            for (var j = ShareObjectListFinalValue[indexRemove].AllBuyEntries
                                    .AllBuysOfTheShareDictionary.Count - 1;
                                j >= 0;
                                j--)
                            {
                                var buyObjectBuysYearOfTheShare = ShareObjectListFinalValue[indexRemove]
                                    .AllBuyEntries.AllBuysOfTheShareDictionary.ElementAt(j);

                                for (var i = buyObjectBuysYearOfTheShare.Value.BuyListYear.Count - 1; i >= 0; i--)
                                {
                                    var buyObject = buyObjectBuysYearOfTheShare.Value.BuyListYear[i];

                                    if (!ShareObjectListFinalValue[indexRemove]
                                        .RemoveBuy(buyObject.Guid, buyObject.DateAsStr))
                                    {
                                        deleteFlag = false;
                                        break;
                                    }
                                }
                            }
                        }

                        // Delete brokerage objects in the final list
                        if (deleteFlag)
                        {
                            for (var j = ShareObjectListFinalValue[indexRemove].AllBrokerageEntries
                                    .AllBrokerageReductionOfTheShareDictionary.Count - 1;
                                j >= 0;
                                j--)
                            {
                                var brokerageObjectBrokeragesYearOfTheShare = ShareObjectListFinalValue[indexRemove]
                                    .AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary.ElementAt(j);

                                for (var i = brokerageObjectBrokeragesYearOfTheShare.Value
                                        .BrokerageReductionListYear.Count - 1;
                                    i >= 0;
                                    i--)
                                {
                                    var brokerageObject = brokerageObjectBrokeragesYearOfTheShare.Value
                                        .BrokerageReductionListYear[i];

                                    if (!ShareObjectListFinalValue[indexRemove]
                                        .RemoveBrokerage(brokerageObject.Guid, brokerageObject.DateAsStr))
                                    {
                                        deleteFlag = false;
                                        break;
                                    }
                                }
                            }
                        }

                        ShareObjectListFinalValue[indexRemove].Dispose();
                        ShareObjectListFinalValue.RemoveAt(indexRemove);
                        ShareObjectFinalValue = null;

                        #endregion Remove share and all objects from this share from the final list 

                        #region Delete from XML file

                        if (deleteFlag)
                        {
                            // Delete the share from the portfolio XML file
                            var nodeDelete = Portfolio.SelectSingleNode($"//Share[@WKN='{wkn}']");
                            nodeDelete?.ParentNode?.RemoveChild(nodeDelete);

                            // Close reader for saving
                            ReaderPortfolio.Close();
                            // Save settings
                            Portfolio.Save(SettingsConfiguration.PortfolioName);
                            // Create a new reader
                            ReaderPortfolio = XmlReader.Create(SettingsConfiguration.PortfolioName, ReaderSettingsPortfolio);
                            Portfolio.Load(SettingsConfiguration.PortfolioName);
                        }

                        #endregion Delete from XML file

                        // Check if the delete was successful
                        if (deleteFlag)
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/DeleteSuccessful",
                                    SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Application);
                        }
                        else
                        {
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/DeleteFailed", SettingsConfiguration.LanguageName),
                                LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
                        }
                    }
                    else
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/DeleteFailed", SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application);
                    }

                    #endregion Delete from share lists

                    // Check if no other shares exists
                    if (ShareObjectListFinalValue.Count == 0 || ShareObjectListMarketValue.Count == 0)
                    {
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

                    // Check if any share set for updating so enable the refresh all button
                    btnRefreshAll.Enabled =
                        ShareObjectListFinalValue.Count(p => p.InternetUpdateOption != ShareObject.ShareUpdateTypes.None && p.WebSiteConfigurationFound) >= 1;
                }
                catch (Exception ex)
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/DeleteErrorFailed", SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application,
                        ex);
                }
            }
            else
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/SelectAShare", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.Orange, Logger, (int) EStateLevels.Warning, (int) EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function saves the added share. It also updates the buttons and the data grid views
        /// </summary>
        public void OnAddShare()
        {
            // Set add flag to true for selecting the new added share in the DataGridView portfolio
            AddFlagMarketValue = true;
            AddFlagFinalValue = true;

            var additionalButtons = new List<string>();

            // Save the share values only of the final value object to the XML (The market value object contains the same values)
            if (ShareObjectFinalValue.SaveShareObject(ShareObjectFinalValue, ref _portfolio, ref _readerPortfolio,
                ref _readerSettingsPortfolio, SettingsConfiguration.PortfolioName, out var exception))
            {
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/AddSaveSuccessful", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
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
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/Errors/AddSaveFailed", SettingsConfiguration.LanguageName),
                    LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application,
                    exception);

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

            // Check if any share set for updating so enable the refresh all button
            btnRefreshAll.Enabled =
                ShareObjectListFinalValue.Count(p => p.InternetUpdateOption != ShareObject.ShareUpdateTypes.None && p.WebSiteConfigurationFound) >= 1;

            // Throw exception which is thrown in the SaveShareObject function
            if (exception != null)
                throw exception;

            AddSharesToDataGridViews();
        }
        #endregion Button
    }
}
