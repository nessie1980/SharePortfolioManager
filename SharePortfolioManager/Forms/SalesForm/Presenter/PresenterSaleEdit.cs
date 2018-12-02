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
using SharePortfolioManager.Classes.Sales;
using SharePortfolioManager.Forms.SalesForm.Model;
using SharePortfolioManager.Forms.SalesForm.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if DEBUG
using System.Windows.Forms;
#endif

namespace SharePortfolioManager.Forms.SalesForm.Presenter
{
    internal class PresenterSaleEdit
    {
        private readonly IModelSaleEdit _model;
        private readonly IViewSaleEdit _view;

        public PresenterSaleEdit(IViewSaleEdit view, IModelSaleEdit model)
        {
            _view = view;
            _model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValuesEventHandler += OnViewFormatInputValues;
            view.AddSaleEventHandler += OnAddSale;
            view.EditSaleEventHandler += OnEditSale;
            view.DeleteSaleEventHandler += OnDeleteSale;
            view.DocumentBrowseEventHandler += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.SelectedGuidLast = _model.SelectedGuidLast;
            _view.Time = _model.Time;
            _view.Volume = _model.Volume;
            _view.SalePrice = _model.SalePrice;
            _view.SaleBuyValue = _model.SaleBuyValue;
            _view.UsedBuyDetails = _model.UsedBuyDetails;
            _view.TaxAtSource = _model.TaxAtSource;
            _view.CapitalGainsTax = _model.CapitalGainsTax;
            _view.SolidarityTax = _model.SolidarityTax;
            _view.Brokerage = _model.Brokerage;
            _view.Reduction = _model.Reduction;
            _view.ProfitLoss = _model.ProfitLoss;
            _view.Payout = _model.Payout;
            _view.Document = _model.Document;
        }

        private void OnViewFormatInputValues(object sender, EventArgs e)
        {
            _model.UpdateViewFormatted = true;

            UpdateViewWithModel();

            _model.UpdateViewFormatted = false;
        }

        private void OnViewChange(object sender, EventArgs e)
        {
            UpdateModelWithView();
        }

        private void UpdateModelWithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ShareObjectCalculation = _view.ShareObjectCalculation;
            _model.ErrorCode = _view.ErrorCode;
            _model.ShowSales = _view.ShowSales;
            _model.AddSale = _view.AddSale;
            _model.UpdateSale = _view.UpdateSale;
            _model.SelectedGuid = _view.SelectedGuid;
            _model.SelectedGuidLast = _view.SelectedGuidLast;
            _model.SelectedDate = _view.SelectedDate;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Volume = _view.Volume;
            _model.SalePrice = _view.SalePrice;
            _model.TaxAtSource = _view.TaxAtSource;
            _model.CapitalGainsTax = _view.CapitalGainsTax;
            _model.SolidarityTax = _view.SolidarityTax;
            _model.Brokerage = _view.Brokerage;
            _model.Reduction = _view.Reduction;
            _model.ProfitLoss = _view.ProfitLoss;
            _model.Payout = _view.Payout;
            _model.Document = _view.Document;

            CalculateProfitLossAndPayout();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        private void OnAddSale(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateSale))
            {
                var bErrorFlag = false;

                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuidBrokerage = Guid.NewGuid().ToString();

                // Generate Guid
                var strGuidSale = Guid.NewGuid().ToString();

                // Brokerage entry if the brokerage value is not 0
                if (_model.BrokerageDec > 0)
                    bErrorFlag = !_model.ShareObjectFinalValue.AddBrokerage(strGuidBrokerage, false, true, strGuidSale, strDateTime, _model.BrokerageDec, _model.Document);

                // Update buys with the sale volumes
                foreach (var usedBuyDetail in _model.UsedBuyDetails)
                {
                    // Add sale volume to the sold volume of the buy
                    _model.ShareObjectMarketValue.AllBuyEntries.AddSaleVolumeByGuid(usedBuyDetail.BuyGuid,
                        usedBuyDetail.DecVolume);
                    _model.ShareObjectFinalValue.AllBuyEntries.AddSaleVolumeByGuid(usedBuyDetail.BuyGuid,
                        usedBuyDetail.DecVolume);
                }

                // When the sale volume has been changed the UseBuyDetails must be updated
                if (_model.ShareObjectFinalValue.AddSale(strGuidSale, strDateTime, _model.VolumeDec, _model.SalePriceDec, _model.UsedBuyDetails,
                        _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, _model.BrokerageDec, _model.ReductionDec, _model.Document) &&
                    _model.ShareObjectMarketValue.AddSale(strGuidSale, strDateTime, _model.VolumeDec, _model.SalePriceDec, _model.UsedBuyDetails,
                        _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, _model.BrokerageDec, _model.ReductionDec, _model.Document) &&
                    bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.AddSuccessful;
                }
                else
                {
                    _model.ErrorCode = SaleErrorCode.AddFailed;
                }
            }

            _model.UsedBuyDetails.Clear();

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditSale(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateSale))
            {
                var strDateTime = _model.Date + " " + _model.Time;

                // Remove old sale volumes from the old used buys
                foreach (var sales in _model.ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Values)
                {
                    foreach (var saleYear in sales.SaleListYear)
                    {
                        foreach (var buyDetails in saleYear.SaleBuyDetails)
                        {
                            foreach (var buysYear in _model.ShareObjectFinalValue.AllBuyEntries
                                .AllBuysOfTheShareDictionary.Values)
                            {
                                foreach (var buy in buysYear.BuyListYear)
                                {
                                    if (buy.Guid == buyDetails.BuyGuid)
                                        buy.VolumeSold -= buyDetails.DecVolume;
                                }
                            }
                        }
                    }
                }
                foreach (var sales in _model.ShareObjectMarketValue.AllSaleEntries.AllSalesOfTheShareDictionary.Values)
                {
                    foreach (var saleYear in sales.SaleListYear)
                    {
                        foreach (var buyDetails in saleYear.SaleBuyDetails)
                        {
                            foreach (var buysYear in _model.ShareObjectMarketValue.AllBuyEntries
                                .AllBuysOfTheShareDictionary.Values)
                            {
                                foreach (var buy in buysYear.BuyListYear)
                                {
                                    if (buy.Guid == buyDetails.BuyGuid)
                                        buy.VolumeSold -= buyDetails.DecVolume;
                                }
                            }
                        }
                    }
                }

                // Update buys with the sale volumes
                foreach (var usedBuyDetail in _model.UsedBuyDetails)
                {
                    // Add sale volume to the sold volume of the buy
                    _model.ShareObjectMarketValue.AllBuyEntries.AddSaleVolumeByGuid(usedBuyDetail.BuyGuid,
                        usedBuyDetail.DecVolume);
                    _model.ShareObjectFinalValue.AllBuyEntries.AddSaleVolumeByGuid(usedBuyDetail.BuyGuid,
                        usedBuyDetail.DecVolume);
                }

                // Generate Guid
                var strGuidBrokerage = Guid.NewGuid().ToString();

                if (_model.ShareObjectFinalValue.RemoveSale(_model.SelectedGuid, _model.SelectedDate) && _model.ShareObjectFinalValue.AddSale(_model.SelectedGuid, strDateTime, _model.VolumeDec, _model.SalePriceDec,
                    _model.UsedBuyDetails, _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, _model.BrokerageDec, _model.ReductionDec, _model.Document) &&
                    _model.ShareObjectMarketValue.RemoveSale(_model.SelectedGuid, _model.SelectedDate) && _model.ShareObjectMarketValue.AddSale(_model.SelectedGuid, strDateTime, _model.VolumeDec, _model.SalePriceDec,
                        _model.UsedBuyDetails, _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, _model.BrokerageDec, _model.ReductionDec, _model.Document)
                    )
                {
                    var bFlagBrokerageEdit = true;

                    // Check if an old brokerage entry must be deleted
                    if (_model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByGuid(_model.SelectedGuid, strDateTime) != null)
                        bFlagBrokerageEdit = _model.ShareObjectFinalValue.RemoveBrokerage(_model.SelectedGuid, strDateTime);

                    // Check if a new brokerage entry must be made
                    if (bFlagBrokerageEdit && _model.BrokerageDec > 0)
                        bFlagBrokerageEdit = _model.ShareObjectFinalValue.AddBrokerage(strGuidBrokerage, false, true, _model.SelectedGuid, strDateTime, _model.BrokerageDec, _model.Document);

                    if (bFlagBrokerageEdit)
                    {
                        _model.ErrorCode = SaleErrorCode.EditSuccessful;
                    }

                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
            }

            //_model.ErrorCode = SaleErrorCode.EditFailed;
            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteSale(object sender, EventArgs e)
        {
            // Get selected sale
            foreach (var salesOfTheYears in _model.ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Values)
            {
                foreach (var sale in salesOfTheYears.SaleListYear)
                {
                    if (sale.Guid != _model.SelectedGuid) continue;

                    // Loop through the buys of the selected sale
                    foreach (var buySale in sale.SaleBuyDetails)
                    {
                        // Get buy and remove sale volume from ShareObjectFinalValue
                        foreach (var buyOfTheYears in _model.ShareObjectFinalValue.AllBuyEntries
                            .AllBuysOfTheShareDictionary.Values)
                        {
                            foreach (var buy in buyOfTheYears.BuyListYear)
                                if (buy.Guid == buySale.BuyGuid)
                                    buy.VolumeSold -= buySale.DecVolume;
                        }

                        // Get buy and remove sale volume from ShareObjectMarketValue
                        foreach (var buyOfTheYears in _model.ShareObjectMarketValue.AllBuyEntries
                            .AllBuysOfTheShareDictionary.Values)
                        {
                            foreach (var buy in buyOfTheYears.BuyListYear)
                                if (buy.Guid == buySale.BuyGuid)
                                    buy.VolumeSold -= buySale.DecVolume;
                        }
                    }
                }
            }

            // Delete the sale of the selected date
            if (_model.ShareObjectFinalValue.RemoveSale(_model.SelectedGuid, _model.SelectedDate))
            {
                // Check if a brokerage object exists
                if (_model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByGuid(_model.SelectedGuid,
                        _model.SelectedDate) != null)
                {
                    _model.ErrorCode =
                        _model.ShareObjectFinalValue.RemoveBrokerage(_model.SelectedGuid, _model.SelectedDate)
                            ? SaleErrorCode.DeleteSuccessful
                            : SaleErrorCode.DeleteFailed;
                }
                else
                {
                    _model.ErrorCode = SaleErrorCode.DeleteSuccessful;
                }
            }
            else
                _model.ErrorCode = SaleErrorCode.DeleteFailedUnErasable;

            // Update error code
            _view.ErrorCode = _model.ErrorCode;

            _view.AddEditDeleteFinish();
        }

        /// <summary>
        /// This function opens the document browse dialog and set the chosen document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentBrowse(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                _model.Document = Helper.SetDocument(_model.Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName), strFilter, _model.Document);

                UpdateViewWithModel();

                _view.DocumentBrowseFinish();
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"OnDocumentBrowse()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.ErrorCode = SaleErrorCode.DocumentBrowseFailed;
            }

        }

        /// <summary>
        /// This function calculates the profit or loss and the payout of the sale
        /// </summary>
        private void CalculateProfitLossAndPayout()
        {
            try
            {
                Console.WriteLine(@"UpdateSale:              {0}", _model.UpdateSale);
                Console.WriteLine(@"AddSale:                 {0}", _model.AddSale);
                Console.WriteLine(@"_model.SelectedGuid:     {0}", _model.SelectedGuid);
                Console.WriteLine(@"_model.SelectedGuidLast: {0}", _model.SelectedGuidLast);
                Console.WriteLine();

                var soldVolume = new decimal(0.0);

                // In this case a change of the TabControl has been done.
                // Only the buys of the selected sale will be removed from the buys sold volume.
                if (_model.UpdateSale && !_model.ShowSales &&
                    _model.SelectedGuidLast != _model.SelectedGuid &&
                    _model.SelectedGuidLast == @"")
                {
                    foreach (var salesYear in _model.ShareObjectCalculation.AllSaleEntries.AllSalesOfTheShareDictionary)
                    {
                        foreach (var sale in salesYear.Value.SaleListYear)
                        {
                            if (sale.Guid != _model.SelectedGuid) continue;
                            foreach (var saleDetail in sale.SaleBuyDetails)
                            {
                                foreach (var buysYear in _model.ShareObjectCalculation.AllBuyEntries
                                    .AllBuysOfTheShareDictionary)
                                {
                                    foreach (var buy in buysYear.Value.BuyListYear)
                                    {
                                        if (buy.Guid == saleDetail.BuyGuid && buy.VolumeSold > 0)
                                        {
                                            // Remove sale volumes from calculation object
                                            _model.ShareObjectCalculation.AllBuyEntries.RemoveSaleVolumeByGuid(
                                                buy.Guid,
                                                saleDetail.DecVolume);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // In this case a change of the TabControl has been done.
                // The buys of the selected sale will be removed from the buys sold volume
                // and the buys of the selected sale will be added to the buys sold volume.
                if (_model.UpdateSale && !_model.ShowSales &&
                    _model.SelectedGuidLast != _model.SelectedGuid &&
                    _model.SelectedGuidLast != @"")
                {
                    foreach (var salesYear in _model.ShareObjectCalculation.AllSaleEntries.AllSalesOfTheShareDictionary)
                    {
                        foreach (var sale in salesYear.Value.SaleListYear)
                        {
                            if (sale.Guid == _model.SelectedGuid)
                            {
                                foreach (var saleDetail in sale.SaleBuyDetails)
                                {
                                    foreach (var buysYear in _model.ShareObjectCalculation.AllBuyEntries
                                        .AllBuysOfTheShareDictionary)
                                    {
                                        foreach (var buy in buysYear.Value.BuyListYear)
                                        {
                                            if (buy.Guid == saleDetail.BuyGuid && buy.VolumeSold > 0)
                                            {
                                                // Remove sale volumes from calculation object
                                                _model.ShareObjectCalculation.AllBuyEntries.RemoveSaleVolumeByGuid(
                                                    buy.Guid,
                                                    saleDetail.DecVolume);
                                            }
                                        }
                                    }
                                }
                            }

                            if (sale.Guid == _model.SelectedGuidLast)
                            {
                                foreach (var saleDetail in sale.SaleBuyDetails)
                                {
                                    foreach (var buysYear in _model.ShareObjectCalculation.AllBuyEntries
                                        .AllBuysOfTheShareDictionary)
                                    {
                                        foreach (var buy in buysYear.Value.BuyListYear)
                                        {
                                            if (buy.Guid == saleDetail.BuyGuid)
                                            {
                                                // Add sale volumes from calculation object
                                                _model.ShareObjectCalculation.AllBuyEntries.AddSaleVolumeByGuid(
                                                    buy.Guid,
                                                    saleDetail.DecVolume);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Save current selected Guid of the selected sale
                if (_model.SelectedGuid != _model.SelectedGuidLast)
                    _model.SelectedGuidLast = _model.SelectedGuid;

                //Check if the list for the used buy details is set else clear the list
                //if (!_model.AddSale /*&& !_model.UpdateSale*/)
                //{
                    if (_model.UsedBuyDetails == null)
                        _model.UsedBuyDetails = new List<SaleBuyDetails>();
                    else
                    {
                        foreach (var modelUsedBuyDetail in _model.UsedBuyDetails)
                        {
                            if (!modelUsedBuyDetail.SaleBuyDetailsAdded)
                            {
                                // Remove sale volumes from calculation object
                                _model.ShareObjectCalculation.AllBuyEntries.RemoveSaleVolumeByGuid(
                                    modelUsedBuyDetail.BuyGuid,
                                    modelUsedBuyDetail.DecVolume);
                            }
                        }

                        _model.UsedBuyDetails.Clear();
                    }
                //}

                // Loop through the buys and check which buy should be used for this sale
                foreach (var buy in _model.ShareObjectCalculation.AllBuyEntries.GetAllBuysOfTheShare())
                {
                    // Check if this buy is already completely sold
                    if (buy.VolumeSold == buy.Volume) continue;

                    // Calculate the already sold volume
                    if (_model.UsedBuyDetails != null && _model.UsedBuyDetails.Count > 0)
                        soldVolume = _model.UsedBuyDetails.Sum(x => x.DecVolume);

                    // Calculate the volume which has to be sold 
                    var toBeSold = _model.VolumeDec - soldVolume;

                    // Check if all sale are done
                    if (toBeSold <= 0) break;

                    // Salable volume
                    var salableVolume = buy.Volume - buy.VolumeSold;

                    // Check if the remaining buy volume is greater than the volume which must be sold
                    if (toBeSold >= buy.Volume - buy.VolumeSold)
                    {
                        _model.UsedBuyDetails.Add(new SaleBuyDetails(_model.ShareObjectCalculation.CultureInfo,
                            buy.DateAsStr, salableVolume, buy.SharePrice, buy.Guid));

                        // Add sale volume to the sold volume of the buy
                        _model.ShareObjectCalculation.AllBuyEntries.AddSaleVolumeByGuid(buy.Guid,
                            salableVolume);
                    }
                    else
                    {
                        _model.UsedBuyDetails.Add(new SaleBuyDetails(_model.ShareObjectCalculation.CultureInfo,
                            buy.DateAsStr, toBeSold, buy.SharePrice, buy.Guid));

                        // Add sale volume to the sold volume of the buy
                        _model.ShareObjectCalculation.AllBuyEntries.AddSaleVolumeByGuid(buy.Guid,
                            toBeSold);
                    }
                }

                var decProfitLoss = _model.SalePriceDec * _model.VolumeDec - _model.UsedBuyDetails.Sum(x => x.SaleBuyValue);
                var decPayout = decProfitLoss - _model.TaxAtSourceDec - _model.CapitalGainsTaxDec - _model.SolidarityTaxDec - _model.BrokerageDec + _model.ReductionDec;

                _model.SaleBuyValueDec = Math.Round(_model.UsedBuyDetails.Sum(x => x.SaleBuyValue), 2);
                _model.ProfitLossDec = decProfitLoss;
                _model.PayoutDec = decPayout;
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"CalculateProfitLossAndPayout()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
            }
        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is chosen if
        /// a sale should be add or edit.
        /// </summary>
        /// <param name="bFlagEdit">Flag if a sale should be add (true) or edit (false)</param>
        /// <returns>Flag if the input values are correct or not</returns>
        private bool CheckInputValues(bool bFlagEdit)
        {
            try
            {
                var bErrorFlag = false;

                _model.ErrorCode = bFlagEdit ? SaleErrorCode.EditSuccessful : SaleErrorCode.AddSuccessful;

                var strDate = _model.Date + " " + _model.Time;

                // Check if a correct volume for the sale is given
                if (_model.Volume == @"")
                {
                    _model.ErrorCode = SaleErrorCode.VolumeEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Volume, out var decVolume))
                {
                    _model.ErrorCode = SaleErrorCode.VolumeWrongFormat;
                    bErrorFlag = true;
                }
                else if (decVolume <= 0)
                {
                    _model.ErrorCode = SaleErrorCode.VolumeWrongValue;
                    bErrorFlag = true;
                }
                else
                {
                    if (!bFlagEdit)
                    {
                        if (decVolume > _model.ShareObjectFinalValue.Volume)
                        {
                            _model.ErrorCode = SaleErrorCode.VolumeMaxValue;
                            bErrorFlag = true;
                        }
                    }
                    else
                    {
                        if (decVolume > _model.ShareObjectFinalValue.AllSaleEntries.GetSaleObjectByDateTime(strDate).Volume + _model.ShareObjectFinalValue.Volume)
                        {
                            _model.ErrorCode = SaleErrorCode.VolumeMaxValue;
                            bErrorFlag = true;
                        }
                    }
                }

                // Check if a correct price for the sale is given
                if (_model.SalePrice == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.SalePriceEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.SalePrice, out var decSalePrice) && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.SalePriceWrongFormat;
                    bErrorFlag = true;
                }
                else if (decSalePrice <= 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.SalePriceWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.SalePriceDec = decSalePrice;

                // Brokerage input check
                if (_model.Brokerage != "" && bErrorFlag == false)
                {
                    if (!decimal.TryParse(_model.Brokerage, out var decBrokerage))
                    {
                        _model.ErrorCode = SaleErrorCode.BrokerageWrongFormat;
                        bErrorFlag = true;
                    }
                    else if (decBrokerage < 0)
                    {
                        _model.ErrorCode = SaleErrorCode.BrokerageWrongValue;
                        bErrorFlag = true;
                    }
                    else
                        _model.BrokerageDec = decBrokerage;
                }

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if (_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
                {
                    _model.ErrorCode = SaleErrorCode.DirectoryDoesNotExists;
                    bErrorFlag = true;
                }
                else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.Document = @"";
                    _model.ErrorCode = SaleErrorCode.FileDoesNotExists;
                    bErrorFlag = true;
                }

                return bErrorFlag;
            }
            catch
            {
                _model.ErrorCode = SaleErrorCode.InputValuesInvalid;
                return true;
            }
        }
    }
}
