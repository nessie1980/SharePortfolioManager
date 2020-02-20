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

using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Costs;
using SharePortfolioManager.Classes.Sales;
using SharePortfolioManager.SalesForm.Model;
using SharePortfolioManager.SalesForm.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager.SalesForm.Presenter
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
            view.SaleChangeEventHandler += OnSaleChangeEventHandler;
            view.AddSaleEventHandler += OnAddSale;
            view.EditSaleEventHandler += OnEditSale;
            view.DeleteSaleEventHandler += OnDeleteSale;
            view.DocumentBrowseEventHandler += OnDocumentBrowse;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.OrderNumber = _model.OrderNumber;
            _view.Time = _model.Time;
            _view.Volume = _model.Volume;
            _view.SalePrice = _model.SalePrice;
            _view.SaleBuyValue = _model.SaleBuyValue;
            _view.UsedBuyDetails = _model.UsedBuyDetails;
            _view.TaxAtSource = _model.TaxAtSource;
            _view.CapitalGainsTax = _model.CapitalGainsTax;
            _view.SolidarityTax = _model.SolidarityTax;
            _view.Provision = _model.Provision;
            _view.BrokerFee = _model.BrokerFee;
            _view.TraderPlaceFee = _model.TraderPlaceFee;
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
            _model.ErrorCode = _view.ErrorCode;
            _model.ShowSalesRunning = _view.ShowSalesRunning;
            _model.LoadSaleRunning = _view.LoadSaleRunning;
            _model.ResetRunning = _view.ResetRunning;
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
            _model.OrderNumber = _view.OrderNumber;
            _model.Volume = _view.Volume;
            _model.SalePrice = _view.SalePrice;
            _model.TaxAtSource = _view.TaxAtSource;
            _model.CapitalGainsTax = _view.CapitalGainsTax;
            _model.SolidarityTax = _view.SolidarityTax;
            _model.Provision = _view.Provision;
            _model.BrokerFee = _view.BrokerFee;
            _model.TraderPlaceFee = _view.TraderPlaceFee;
            _model.Reduction = _view.Reduction;
            _model.Document = _view.Document;

            CalculateProfitLossAndPayout();
        }

        private void OnSaleChangeEventHandler(object sender, EventArgs e)
        {
            // In this case a change of the selected sale object has been done.
            // The buys of the selected sale will be added to the buys sold volume
            // and buys of the last selected sale will be removed from the buys sold volume.
            if (!_model.ShowSalesRunning &&
                _model.SelectedGuid != _model.SelectedGuidLast
                )
            {
                foreach (var salesYear in _model.ShareObjectFinalValue.AllSaleEntries
                    .AllSalesOfTheShareDictionary)
                {
                    foreach (var sale in salesYear.Value.SaleListYear)
                    {
                        if (sale.Guid == _model.SelectedGuid)
                        {
                            foreach (var saleDetail in sale.SaleBuyDetails)
                            {
                                foreach (var buysYear in _model.ShareObjectFinalValue.AllBuyEntries
                                    .AllBuysOfTheShareDictionary)
                                {
                                    foreach (var buy in buysYear.Value.BuyListYear)
                                    {
                                        if (buy.Guid == saleDetail.BuyGuid && buy.VolumeSold > 0)
                                        {
                                            Console.WriteLine(@"Removed Buy-Guid:    {0}", buy.Guid);
                                            Console.WriteLine(@"Removed Buy-Volume:  {0}", saleDetail.DecVolume);

                                            // Remove sale volumes from calculation object
                                            _model.ShareObjectFinalValue.AllBuyEntries.RemoveSaleVolumeByGuid(
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
                                foreach (var buysYear in _model.ShareObjectFinalValue.AllBuyEntries
                                    .AllBuysOfTheShareDictionary)
                                {
                                    foreach (var buy in buysYear.Value.BuyListYear)
                                    {
                                        if (buy.Guid == saleDetail.BuyGuid)
                                        {
                                            Console.WriteLine(@"Added Buy-Guid:      {0}", buy.Guid);
                                            Console.WriteLine(@"Added Buy-Volume:    {0}", saleDetail.DecVolume);

                                            // Add sale volumes from calculation object
                                            _model.ShareObjectFinalValue.AllBuyEntries.AddSaleVolumeByGuid(
                                                buy.Guid,
                                                saleDetail.DecVolume);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var salesYear in _model.ShareObjectMarketValue.AllSaleEntries
                    .AllSalesOfTheShareDictionary)
                {
                    foreach (var sale in salesYear.Value.SaleListYear)
                    {
                        if (sale.Guid == _model.SelectedGuid)
                        {
                            foreach (var saleDetail in sale.SaleBuyDetails)
                            {
                                foreach (var buysYear in _model.ShareObjectMarketValue.AllBuyEntries
                                    .AllBuysOfTheShareDictionary)
                                {
                                    foreach (var buy in buysYear.Value.BuyListYear)
                                    {
                                        if (buy.Guid == saleDetail.BuyGuid && buy.VolumeSold > 0)
                                        {
                                            Console.WriteLine(@"Removed Buy-Guid:    {0}", buy.Guid);
                                            Console.WriteLine(@"Removed Buy-Volume:  {0}", saleDetail.DecVolume);

                                            // Remove sale volumes from calculation object
                                            _model.ShareObjectMarketValue.AllBuyEntries.RemoveSaleVolumeByGuid(
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
                                foreach (var buysYear in _model.ShareObjectMarketValue.AllBuyEntries
                                    .AllBuysOfTheShareDictionary)
                                {
                                    foreach (var buy in buysYear.Value.BuyListYear)
                                    {
                                        if (buy.Guid == saleDetail.BuyGuid)
                                        {
                                            Console.WriteLine(@"Added Buy-Guid:      {0}", buy.Guid);
                                            Console.WriteLine(@"Added Buy-Volume:    {0}", saleDetail.DecVolume);

                                            // Add sale volumes from calculation object
                                            _model.ShareObjectMarketValue.AllBuyEntries.AddSaleVolumeByGuid(
                                                buy.Guid,
                                                saleDetail.DecVolume);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                CalculateProfitLossAndPayout();
            }

            // If a return to the edit dialog will be done
            // then a remove of the current used buy volumes is done
            if (_model.SelectedGuid == string.Empty &&
                _model.SelectedGuidLast == string.Empty)
            {
                foreach (var modelUsedBuyDetail in _model.UsedBuyDetails)
                {
                    if (!modelUsedBuyDetail.SaleBuyDetailsAdded)
                    {
                        // Remove sale volumes from calculation object
                        _model.ShareObjectFinalValue.AllBuyEntries.RemoveSaleVolumeByGuid(
                            modelUsedBuyDetail.BuyGuid,
                            modelUsedBuyDetail.DecVolume);
                    }
                    if (!modelUsedBuyDetail.SaleBuyDetailsAdded)
                    {
                        // Remove sale volumes from calculation object
                        _model.ShareObjectMarketValue.AllBuyEntries.RemoveSaleVolumeByGuid(
                            modelUsedBuyDetail.BuyGuid,
                            modelUsedBuyDetail.DecVolume);
                    }
                }

                _model.UsedBuyDetails.Clear();
            }
        }

        private void OnAddSale(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateSale))
            {
                var bErrorFlag = false;
                BrokerageReductionObject brokerage = null;

                // Set date
                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuidSale = Guid.NewGuid().ToString();

                // Brokerage entry if any brokerage value is not 0
                if (_model.ProvisionDec != 0 || _model.BrokerFeeDec != 0 || _model.TraderPlaceFeeDec != 0 ||
                    _model.ReductionDec != 0)
                {
                    // Generate Guid
                    var strGuidBrokerage = Guid.NewGuid().ToString();

                    bErrorFlag = !_model.ShareObjectFinalValue.AddBrokerage(strGuidBrokerage, false, true, strGuidSale,
                        strDateTime,
                        _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec,
                        _model.Document);

                    // Get brokerage object
                    brokerage =
                        _model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByGuidDate(strGuidBrokerage, strDateTime);

                }

                // When the sale volume has been changed the UseBuyDetails must be updated
                if (bErrorFlag == false &&
                    _model.ShareObjectFinalValue.AddSale(strGuidSale, strDateTime, _model.OrderNumber, _model.VolumeDec, _model.SalePriceDec, _model.UsedBuyDetails,
                        _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, brokerage, _model.Document) &&
                    _model.ShareObjectMarketValue.AddSale(strGuidSale, strDateTime, _model.OrderNumber, _model.VolumeDec, _model.SalePriceDec, _model.UsedBuyDetails,
                        _model.TaxAtSourceDec, _model.CapitalGainsTaxDec, _model.SolidarityTaxDec, _model.Document))
                {
                    _model.ErrorCode = SaleErrorCode.AddSuccessful;
                }
                else
                {
                    _model.ErrorCode = SaleErrorCode.AddFailed;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditSale(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateSale))
            {
                // Set date
                var strDateTime = _model.Date + " " + _model.Time;

                var bFlagBrokerageEdit = true;
                var guidBrokerage = @"";

                // Check if an old brokerage entry must be deleted
                if (_model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByBuyGuid(_model.SelectedGuid,
                        strDateTime) != null)
                {
                    // Get brokerage GUID
                    guidBrokerage = _model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByBuyGuid(
                        _model.SelectedGuid,
                        strDateTime).Guid;
                    bFlagBrokerageEdit = _model.ShareObjectFinalValue.RemoveBrokerage(guidBrokerage, strDateTime);
                }

                // Check if a new brokerage entry must be made
                if (bFlagBrokerageEdit && (_model.ProvisionDec != 0 || _model.BrokerFeeDec != 0 || _model.TraderPlaceFeeDec != 0 || _model.ReductionDec != 0))
                {
                    bFlagBrokerageEdit = _model.ShareObjectFinalValue.AddBrokerage(guidBrokerage, true, false, _model.SelectedGuid, strDateTime,
                        _model.ProvisionDec, _model.BrokerFeeDec, _model.TraderPlaceFeeDec, _model.ReductionDec, _model.Document);
                }

                // Get brokerage object
                var brokerage = _model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByGuidDate(guidBrokerage, strDateTime);

                if (bFlagBrokerageEdit)
                {
                    if (_model.ShareObjectFinalValue.RemoveSale(_model.SelectedGuid, _model.SelectedDate) &&
                        _model.ShareObjectFinalValue.AddSale(_model.SelectedGuid, strDateTime, _model.OrderNumber,
                            _model.VolumeDec, _model.SalePriceDec,
                            _model.UsedBuyDetails, _model.TaxAtSourceDec, _model.CapitalGainsTaxDec,
                            _model.SolidarityTaxDec, brokerage, _model.Document) &&
                        _model.ShareObjectMarketValue.RemoveSale(_model.SelectedGuid, _model.SelectedDate) &&
                        _model.ShareObjectMarketValue.AddSale(_model.SelectedGuid, strDateTime, _model.OrderNumber,
                            _model.VolumeDec, _model.SalePriceDec,
                            _model.UsedBuyDetails, _model.TaxAtSourceDec, _model.CapitalGainsTaxDec,
                            _model.SolidarityTaxDec, _model.Document)
                    )
                    {
                        _model.ErrorCode = SaleErrorCode.EditSuccessful;
                    }

                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
            }

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
            if (_model.ShareObjectFinalValue.RemoveSale(_model.SelectedGuid, _model.SelectedDate) && 
                _model.ShareObjectMarketValue.RemoveSale(_model.SelectedGuid, _model.SelectedDate))
            {
                // Check if a brokerage object exists
                if (_model.ShareObjectFinalValue.AllBrokerageEntries.GetBrokerageObjectByGuidDate(_model.SelectedGuid,
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
                var strCurrentFile = _model.Document;

                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                if (Helper.SetDocument(
                        _model.Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/OpenFileDialog/Title",
                            _model.LanguageName), strFilter, ref strCurrentFile) == DialogResult.OK)
                {
                    _model.Document = strCurrentFile;
                }

                UpdateViewWithModel();

                _view.DocumentBrowseFinish();
            }
#if! DEBUG
            catch
            {
#else
            catch (Exception ex)
            {
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
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
                var alreadySoldVolume = new decimal(0.0);

                if (_model.UsedBuyDetails == null)
                    _model.UsedBuyDetails = new List<SaleBuyDetails>();
                else
                {
                    foreach (var modelUsedBuyDetail in _model.UsedBuyDetails)
                    {
                        if (!modelUsedBuyDetail.SaleBuyDetailsAdded)
                        {
                            // Remove sale volumes from calculation object
                            _model.ShareObjectFinalValue.AllBuyEntries.RemoveSaleVolumeByGuid(
                                modelUsedBuyDetail.BuyGuid,
                                modelUsedBuyDetail.DecVolume);
                        }
                        if (!modelUsedBuyDetail.SaleBuyDetailsAdded)
                        {
                            // Remove sale volumes from calculation object
                            _model.ShareObjectMarketValue.AllBuyEntries.RemoveSaleVolumeByGuid(
                                modelUsedBuyDetail.BuyGuid,
                                modelUsedBuyDetail.DecVolume);
                        }
                    }

                    _model.UsedBuyDetails.Clear();
                }

                // Loop through the buys and check which buy should be used for this sale
                foreach (var currentBuyObject in _model.ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare())
                {
                    // Check if this buy is already completely sold
                    if (currentBuyObject.VolumeSold == currentBuyObject.Volume) continue;

                    // Calculate the already sold volume
                    if (_model.UsedBuyDetails != null && _model.UsedBuyDetails.Count > 0)
                        alreadySoldVolume = _model.UsedBuyDetails.Sum(x => x.DecVolume);

                    // Calculate the volume which has to be sold 
                    var toBeSoldVolume = _model.VolumeDec - alreadySoldVolume;

                    // Check if all sale are done
                    if (toBeSoldVolume <= 0) break;

                    // Salable volume
                    var salableVolume = currentBuyObject.Volume - currentBuyObject.VolumeSold;

                    // Check if the remaining buy volume is greater than the volume which must be sold
                    if (toBeSoldVolume >= currentBuyObject.Volume - currentBuyObject.VolumeSold)
                    {
                        // Calculate brokerage / reduction value
                        var brokerageWithReductionPart = Math.Round(currentBuyObject.Brokerage * salableVolume / currentBuyObject.Volume, 5);
                        var reductionPart = Math.Round(currentBuyObject.Reduction * salableVolume / currentBuyObject.Volume, 5);

                        decimal decAlreadyUsedBrokerageValue = 0;
                        decimal decAlreadyUsedReductionValue = 0;
                        // Get already used brokerage / reduction value ( rounding errors )
                        foreach (var saleObject in _model.ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
                        {
                            // If an edit is running only add the brokerage / reduction values of the not selected values
                            if (_model.SelectedGuid == saleObject.Guid) continue;

                            foreach (var saleObjectSaleBuyDetail in saleObject.SaleBuyDetails)
                            {
                                if (saleObjectSaleBuyDetail.BuyGuid == currentBuyObject.Guid)
                                    decAlreadyUsedBrokerageValue +=
                                        saleObjectSaleBuyDetail.BrokeragePart;
                                if (saleObjectSaleBuyDetail.BuyGuid == currentBuyObject.Guid)
                                    decAlreadyUsedReductionValue +=
                                        saleObjectSaleBuyDetail.ReductionPart;
                            }
                        }

                        // Check if the brokerage / reduction value is not equal to the rest calculation
                        if (!brokerageWithReductionPart.Equals(currentBuyObject.Brokerage - decAlreadyUsedBrokerageValue))
                            brokerageWithReductionPart = currentBuyObject.Brokerage - decAlreadyUsedBrokerageValue;

                        // Check if the reduction value is not equal to the rest calculation
                        if (!reductionPart.Equals(currentBuyObject.Reduction - decAlreadyUsedReductionValue)) 
                            reductionPart = currentBuyObject.Reduction - decAlreadyUsedReductionValue;

                        _model.UsedBuyDetails?.Add(new SaleBuyDetails(_model.ShareObjectFinalValue.CultureInfo,
                            currentBuyObject.DateAsStr, salableVolume, currentBuyObject.Price, 
                             reductionPart, brokerageWithReductionPart, currentBuyObject.Guid));

                        // Add sale volume to the sold volume of the buy
                        _model.ShareObjectFinalValue.AllBuyEntries.AddSaleVolumeByGuid(currentBuyObject.Guid,
                            salableVolume);
                        // Add sale volume to the sold volume of the buy
                        _model.ShareObjectMarketValue.AllBuyEntries.AddSaleVolumeByGuid(currentBuyObject.Guid,
                            salableVolume);
                    }
                    else
                    {
                        // Calculate brokerage / reduction value
                        var brokeragePart = Math.Round(currentBuyObject.Brokerage * toBeSoldVolume / currentBuyObject.Volume, 5);

                        // Calculate reduction value
                        var reductionPart = Math.Round(currentBuyObject.Reduction * toBeSoldVolume / currentBuyObject.Volume, 5);

                        _model.UsedBuyDetails?.Add(new SaleBuyDetails(_model.ShareObjectFinalValue.CultureInfo,
                            currentBuyObject.DateAsStr, toBeSoldVolume, currentBuyObject.Price,
                            reductionPart, brokeragePart, currentBuyObject.Guid));

                        // Add sale volume to the sold volume of the buy
                        _model.ShareObjectFinalValue.AllBuyEntries.AddSaleVolumeByGuid(currentBuyObject.Guid,
                            toBeSoldVolume);
                        // Add sale volume to the sold volume of the buy
                        _model.ShareObjectMarketValue.AllBuyEntries.AddSaleVolumeByGuid(currentBuyObject.Guid,
                            toBeSoldVolume);
                    }
                }

                decimal decProfitLoss = 0;
                if (_model.UsedBuyDetails != null)
                    decProfitLoss = _model.SalePriceDec* _model.VolumeDec -
                    _model.UsedBuyDetails.Sum(x => x.SaleBuyValueBrokerageReduction);
                else
                    if (_model.UsedBuyDetails != null)
                        decProfitLoss = _model.SalePriceDec * _model.VolumeDec;

                var decBrokerage = _model.ProvisionDec + _model.BrokerFeeDec + _model.TraderPlaceFeeDec -
                                   _model.ReductionDec;
                var decPayout = decProfitLoss - _model.TaxAtSourceDec - _model.CapitalGainsTaxDec -
                                _model.SolidarityTaxDec - decBrokerage;

                _model.SaleBuyValueDec = _model.UsedBuyDetails != null ? Math.Round(_model.UsedBuyDetails.Sum(x => x.SaleBuyValueBrokerageReduction), 2) : 0;

                _model.ProfitLossDec = decProfitLoss;
                _model.BrokerageDec = decBrokerage;
                _model.PayoutDec = decPayout;

                UpdateViewWithModel();
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
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

                // Check if a order number for the buy is given
                if (_model.OrderNumber == @"")
                {
                    _model.ErrorCode = SaleErrorCode.OrderNumberEmpty;
                    bErrorFlag = true;
                }
                else if (_model.ShareObjectFinalValue.AllSaleEntries.OrderNumberAlreadyExists(_model.OrderNumber, _model.SelectedGuid))
                {
                    _model.ErrorCode = SaleErrorCode.OrderNumberExists;
                    bErrorFlag = true;
                }

                // Check if a correct volume for the sale is given
                if (_model.Volume == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.VolumeEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Volume, out var decVolume) && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.VolumeWrongFormat;
                    bErrorFlag = true;
                }
                else if (decVolume <= 0 && bErrorFlag == false)
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
                        if (decVolume > _model.ShareObjectFinalValue.AllSaleEntries.GetSaleObjectByGuidDate(_model.SelectedGuid, strDate).Volume + _model.ShareObjectFinalValue.Volume)
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
                if (_model.Brokerage == @"" && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.BrokerageEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Brokerage, out var decBrokerage) && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.BrokerageWrongFormat;
                    bErrorFlag = true;
                }
                else if (decBrokerage < 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = SaleErrorCode.BrokerageWrongValue;
                    bErrorFlag = true;
                }
                else
                    _model.BrokerageDec = decBrokerage;

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if (_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
                {
                    _model.ErrorCode = SaleErrorCode.DocumentDirectoryDoesNotExists;
                    bErrorFlag = true;
                }
                else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.Document = @"";
                    _model.ErrorCode = SaleErrorCode.DocumentFileDoesNotExists;
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
