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
//#define DEBUG_SALE_VIEW

using LanguageHandler;
using Logging;
using Parser;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Sales;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.OwnMessageBoxForm;
using SharePortfolioManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharePortfolioManager.Classes.Configurations;

namespace SharePortfolioManager.SalesForm.View
{
    // Error codes of the SaleEdit
    public enum SaleErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        DeleteFailedUnErasable,
        InputValuesInvalid,
        DepotNumberEmpty,
        OrderNumberEmpty,
        OrderNumberExists,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        VolumeMaxValue,
        SalePriceEmpty,
        SalePriceWrongFormat,
        SalePriceWrongValue,
        TaxAtSourceWrongFormat,
        TaxAtSourceWrongValue,
        CapitalGainsTaxWrongFormat,
        CapitalGainsTaxWrongValue,
        SolidarityTaxWrongFormat,
        SolidarityTaxWrongValue,
        ProvisionWrongFormat,
        ProvisionWrongValue,
        BrokerFeeWrongFormat,
        BrokerFeeWrongValue,
        TraderPlaceFeeWrongFormat,
        TraderPlaceFeeWrongValue,
        ReductionWrongFormat,
        ReductionWrongValue,
        BrokerageEmpty,
        BrokerageWrongFormat,
        BrokerageWrongValue,
        DocumentDirectoryDoesNotExists,
        DocumentFileDoesNotExists,
        DocumentBrowseFailed,
    };

    // Error codes for the document parsing
    public enum ParsingErrorCode
    {
        ParsingFailed = -6,
        ParsingDocumentNotImplemented = -5,
        ParsingDocumentTypeIdentifierFailed = -4,
        ParsingBankIdentifierFailed = -3,
        ParsingDocumentFailed = -2,
        ParsingParsingDocumentError = -1,
        ParsingStarted = 0,
        ParsingIdentifierValuesFound = 1
    }

    // Column indices values of the overview DataGridView
    public enum DataGridViewOverViewIndices
    {
        ColumnIndicesYear,
        ColumnIndicesVolume,
        ColumnIndicesPayout,
        ColumnIndicesProfitLoss,
        ColumnIndicesScrollBar
    }

    // Column indices values of the years DataGridView
    public enum DataGridViewYearIndices
    {
        ColumnIndicesGuid,
        ColumnIndicesDate,
        ColumnIndicesBuyValue,
        ColumnIndicesProfitLoss,
        ColumnIndicesPayout,
        ColumnIndicesDocument,
        ColumnIndicesScrollBar
    }

    /// <inheritdoc />
    /// <summary>
    /// Interface of the SaleEdit view
    /// </summary>
    public interface IViewSaleEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler SaleChangeEventHandler;
        event EventHandler AddSaleEventHandler;
        event EventHandler EditSaleEventHandler;
        event EventHandler DeleteSaleEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        SaleErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool AddSale { get; set; }
        bool UpdateSale { get; set; }
        bool ShowSalesRunning { get; set; }
        bool LoadSaleRunning { get; set; }
        bool ResetRunning { get; set; }
        bool DepotNumberChangeRunning { get; set; }
        string SelectedGuid { get; set; }
        string SelectedGuidLast { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string DepotNumber { get; set; }
        string OrderNumber { get; set; }
        string Volume { get; set; }
        string SalePrice { get; set; }
        string SaleBuyValue { get; set; }
        List<SaleBuyDetails> UsedBuyDetails { set; }
        string TaxAtSource { get; set; }
        string CapitalGainsTax { get; set; }
        string SolidarityTax { get; set; }
        string Provision { get; set; }
        string BrokerFee { get; set; }
        string TraderPlaceFee { get; set; }
        string Reduction { get; set; }
        string Brokerage { get; set; }
        string ProfitLoss { get; set; }
        string Payout { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
        void DocumentBrowseFinish();
    }

    public partial class ViewSaleEdit : Form, IViewSaleEdit
    {
        #region Fields

        /// <summary>
        /// Stores the Guid of a selected sale row
        /// </summary>
        private string _selectedGuid;

        /// <summary>
        /// Stores the Guid of a selected sale row of the last select
        /// </summary>
        private string _selectedGuidLast = string.Empty;

        /// <summary>
        /// Stores the date of a selected sale row
        /// </summary>
        private string _selectedDate;

        /// <summary>
        /// Stores the last focused date time picker or text box
        /// </summary>
        private Control _focusedControl;

        /// <summary>
        /// Postfix for the DataGridView footer name
        /// </summary>
        private readonly string _dataGridViewFooterPostfix = "_Footer";

        /// <summary>
        /// If the row count is greater then 4 the scrollbar will be shown
        /// </summary>
        private const int DataGridViewRowsScrollbar = 4;

        /// <summary>
        /// Height of the DataGridView footer
        /// </summary>
        private const int DataGridViewFooterHeight = 24;

        #region Column width for the dgvContentOverView and dgvContentOverViewFooter

        private const int OverViewDateColumnSize = 85;
        private const int OverViewVolumeColumnSize = 200;
        private const int OverViewPayoutColumnSize = 200;

        private const int YearDateColumnSize = 85;
        private const int YearBuyValueColumnSize = 175;
        private const int YearProfitLossColumnSize = 175;
        private const int YearPayoutColumnSize = 175;

        #endregion Column width for the dgvPortfolio and dgvPortfolioFooter

        #endregion Fields

        #region Parsing Fields

        /// <summary>
        /// Flag if a parsing start is allows ( document browse / drag and drop )
        /// </summary>
        private bool _parsingStartAllow;

        /// <summary>
        /// Counter for the check bank type configurations
        /// </summary>
        private int _bankCounter;

        /// <summary>
        /// Flag if the bank identifier of the given document could be found in the document configurations
        /// </summary>
        private bool _bankIdentifierFound;

        /// <summary>
        /// Flag if the sale identifier type of the given document could be found in the document configurations
        /// </summary>
        private bool _saleIdentifierFound;

        /// <summary>
        /// Flag if the document type is not implemented yet
        /// </summary>
        private bool _documentTypNotImplemented;

        /// <summary>
        /// Flag if the document values parsing is running
        /// </summary>
        private bool _documentValuesRunning;

        /// <summary>
        /// Flag if the parsing was successful
        /// </summary>
        private bool _parsingResult;

        /// <summary>
        /// Flag if the parsing with the Parser.dll is done
        /// </summary>
        private bool _parsingThreadFinished;

        /// <summary>
        /// BackGroundWorker for the document parsing
        /// </summary>
        private readonly BackgroundWorker _parsingBackgroundWorker = new BackgroundWorker();

        #endregion Parsing Fields

        #region Properties

        #region Transfer parameter

        public Logger Logger { get; internal set; }

        public Language Language { get; internal set; }

        public string LanguageName { get; internal set; }

        public string ParsingFileName { get; internal set; }

        #endregion Transfer parameter

        #region Flags

        public bool AddSaleFlag { get; set; }
        public bool UpdateSaleFlag { get; set; }

        public bool ShowSalesRunningFlag { get; set; }
        public bool LoadSaleRunningFlag { get; set; }
        public bool ResetRunningFlag { get; set; }
        public bool DepotNumberChangeRunningFlag { get; set; }
        
        public bool SaveFlag { get; internal set; }

        #endregion Flags

        public DataGridView SelectedDataGridView { get; internal set; }

        #region Parsing

        public DocumentParsingConfiguration.DocumentTypes DocumentType { get; internal set; }

        public Parser.Parser DocumentTypeParser;

        public string ParsingText { get; internal set; }

        public Dictionary<string, List<string>> DictionaryParsingResult;

        #endregion Parsing

        #endregion Properties

        #region IViewMember

        public bool AddSale
        {
            get => AddSaleFlag;
            set
            {
                AddSaleFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddSale"));
            }
        }

        public bool UpdateSale
        {
            get => UpdateSaleFlag;
            set
            {
                UpdateSaleFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateSale"));
            }
        }

        public bool ShowSalesRunning
        {
            get => ShowSalesRunningFlag;
            set
            {
                ShowSalesRunningFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowSalesRunning"));
            }
        }

        public bool LoadSaleRunning
        {
            get => LoadSaleRunningFlag;
            set
            {
                LoadSaleRunningFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LoadSaleRunning"));
            }
        }

        public bool ResetRunning
        {
            get => ResetRunningFlag;
            set
            {
                ResetRunningFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResetRunning"));
            }
        }

        public bool DepotNumberChangeRunning
        {
            get => DepotNumberChangeRunningFlag;
            set
            {
                DepotNumberChangeRunningFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepotNumberChangeRunning"));
            }
        }

        public SaleErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public string SelectedGuid
        {
            get => _selectedGuid;
            set
            {
                _selectedGuid = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedGuid"));
            }
        }

        public string SelectedGuidLast
        {
            get => _selectedGuidLast;
            set
            {
                if (_selectedGuidLast != null && _selectedGuidLast == value)
                    return;
                _selectedGuidLast = value;
            }
        }

        public string SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedDate"));
            }
        }

        #region Input values

        public string Date
        {
            get => dateTimePickerDate.Text;
            set
            {
                if (dateTimePickerDate.Text == value)
                    return;
                dateTimePickerDate.Text = value;
            }
        }

        public string Time
        {
            get => dateTimePickerTime.Text;
            set
            {
                if (dateTimePickerTime.Text == value)
                    return;
                dateTimePickerTime.Text = value;
            }
        }

        public string DepotNumber
        {
            get => cbxDepotNumber.SelectedIndex > 0 ? cbxDepotNumber.SelectedItem.ToString() : @"-";
            set
            {
                var index = cbxDepotNumber.FindString(value);
                if (index > -1)
                    cbxDepotNumber.SelectedIndex = index;
            }
        }

        public string OrderNumber
        {
            get => txtBoxOrderNumber.Text;
            set
            {
                if (txtBoxOrderNumber.Text == value)
                    return;
                txtBoxOrderNumber.Text = value;
            }
        }

        public string Volume
        {
            get => txtBoxVolume.Text;
            set
            {
                if (txtBoxVolume.Text == value)
                    return;
                txtBoxVolume.Text = value;
            }
        }

        public string SalePrice
        {
            get => txtBoxSalePrice.Text;
            set
            {
                if (txtBoxSalePrice.Text == value)
                    return;
                txtBoxSalePrice.Text = value;
            }
        }

        public string SaleBuyValue
        {
            get => txtBoxSaleBuyValue.Text;
            set
            {
                if (txtBoxSaleBuyValue.Text == value)
                    return;
                txtBoxSaleBuyValue.Text = value;
            }
        }

        public List<SaleBuyDetails> UsedBuyDetails { get; set; }

        public string TaxAtSource
        {
            get => txtBoxTaxAtSource.Text;
            set
            {
                if (txtBoxTaxAtSource.Text == value)
                    return;
                txtBoxTaxAtSource.Text = value;
            }
        }

        public string CapitalGainsTax
        {
            get => txtBoxCapitalGainsTax.Text;
            set
            {
                if (txtBoxCapitalGainsTax.Text == value)
                    return;
                txtBoxCapitalGainsTax.Text = value;
            }
        }

        public string SolidarityTax
        {
            get => txtBoxSolidarityTax.Text;
            set
            {
                if (txtBoxSolidarityTax.Text == value)
                    return;
                txtBoxSolidarityTax.Text = value;
            }
        }

        public string Provision
        {
            get => txtBoxProvision.Text;
            set
            {
                if (txtBoxProvision.Text == value)
                    return;
                txtBoxProvision.Text = value;
            }
        }

        public string BrokerFee
        {
            get => txtBoxBrokerFee.Text;
            set
            {
                if (txtBoxBrokerFee.Text == value)
                    return;
                txtBoxBrokerFee.Text = value;
            }
        }

        public string TraderPlaceFee
        {
            get => txtBoxTraderPlaceFee.Text;
            set
            {
                if (txtBoxTraderPlaceFee.Text == value)
                    return;
                txtBoxTraderPlaceFee.Text = value;
            }
        }

        public string Reduction
        {
            get => txtBoxReduction.Text;
            set
            {
                if (txtBoxReduction.Text == value)
                    return;
                txtBoxReduction.Text = value;
            }
        }

        public string Brokerage
        {
            get => txtBoxBrokerage.Text;
            set
            {
                if (txtBoxBrokerage.Text == value)
                    return;
                txtBoxBrokerage.Text = value;
            }
        }

        public string ProfitLoss
        {
            get => txtBoxProfitLoss.Text;
            set
            {
                if (txtBoxProfitLoss.Text == value)
                    return;
                txtBoxProfitLoss.Text = value;
            }
        }

        public string Payout
        {
            get => txtBoxPayout.Text;
            set
            {
                if (txtBoxPayout.Text == value)
                    return;
                txtBoxPayout.Text = value;
            }
        }

        public string Document
        {
            get => txtBoxDocument.Text;
            set
            {
                if (txtBoxDocument.Text == value)
                    return;
                txtBoxDocument.Text = value;
            }
        }

        #endregion InputValues

        public void AddEditDeleteFinish()
        {
            // Set messages
            var strMessage = string.Empty;
            var clrMessage = Color.Black;
            var stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case SaleErrorCode.AddSuccessful:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/AddSuccess", SettingsConfiguration.LanguageName);
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Reset add flag
                    AddSale = false;

                    // Check if a direct document parsing is done
                    if (ParsingFileName != null)
                        Close();
                    else
                    {
                        // Refresh the sale list
                        OnShowSales();
                    }

                    break;
                }
                case SaleErrorCode.AddFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.EditSuccessful:
                {
                    // Enable button(s)
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add",
                            LanguageName);
                    btnAddSave.Image = Resources.button_add_24;
                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption",
                            LanguageName);

                    // Reset dialog icon to add
                    Icon = Resources.add;

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/EditSuccess", SettingsConfiguration.LanguageName);

                    // Reset update flag
                    UpdateSale = false;

                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Refresh the sale list
                    OnShowSales();

                    break;
                }
                case SaleErrorCode.EditFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/EditFailed", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.DeleteSuccessful:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/DeleteSuccess", SettingsConfiguration.LanguageName);

                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Enable button(s)
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Reset add flag
                    AddSale = false;

                    // Reset update flag
                    UpdateSale = false;

                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);

                    // Reset dialog icon to add
                    Icon = Resources.add;

                    // Refresh the sale list
                    OnShowSales();

                    break;
                }
                case SaleErrorCode.DeleteFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.InputValuesInvalid:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CheckInputFailure", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.DepotNumberEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DepotNumberEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    cbxDepotNumber.Focus();

                    break;
                }
                case SaleErrorCode.OrderNumberEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/OrderNumberEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case SaleErrorCode.OrderNumberExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/OrderNumberExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case SaleErrorCode.VolumeEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.VolumeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.VolumeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.VolumeMaxValue:
                {
                    if (btnAddSave.Text ==
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName))
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_1", SettingsConfiguration.LanguageName) +
                            ShareObjectFinalValue.SalableVolumeAsStr +
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_2", SettingsConfiguration.LanguageName);
                    }
                    else
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_1", SettingsConfiguration.LanguageName) +
                            ShareObjectFinalValue.SalableVolumeAsStr +
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_2", SettingsConfiguration.LanguageName);
                    }

                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Text = ShareObjectFinalValue.SalableVolumeAsStr;
                    txtBoxVolume.Focus();

                    break;
                }
                case SaleErrorCode.SalePriceEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSalePrice.Focus();

                    break;
                }
                case SaleErrorCode.SalePriceWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSalePrice.Focus();

                    break;
                }
                case SaleErrorCode.SalePriceWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSalePrice.Focus();

                    break;
                }
                case SaleErrorCode.TaxAtSourceWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongFormat",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxTaxAtSource.Focus();

                    break;
                }
                case SaleErrorCode.TaxAtSourceWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxTaxAtSource.Focus();

                    break;
                }
                case SaleErrorCode.CapitalGainsTaxWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongFormat",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxCapitalGainsTax.Focus();

                    break;
                }
                case SaleErrorCode.CapitalGainsTaxWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongValue",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxCapitalGainsTax.Focus();

                    break;
                }
                case SaleErrorCode.SolidarityTaxWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongFormat",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSolidarityTax.Focus();

                    break;
                }
                case SaleErrorCode.SolidarityTaxWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongValue",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSolidarityTax.Focus();

                    break;
                }
                case SaleErrorCode.ProvisionWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ProvisionWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxProvision.Focus();

                    break;
                }
                case SaleErrorCode.ProvisionWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ProvisionWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxProvision.Focus();

                    break;
                }
                case SaleErrorCode.BrokerFeeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BrokerFeeWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerFee.Focus();

                    break;
                }
                case SaleErrorCode.BrokerFeeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BrokerFeeWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerFee.Focus();

                    break;
                }
                case SaleErrorCode.TraderPlaceFeeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TraderPlaceFeeWrongFormat",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxTraderPlaceFee.Focus();

                    break;
                }
                case SaleErrorCode.TraderPlaceFeeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TraderPlaceFeeWrongValue",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxTraderPlaceFee.Focus();

                    break;
                }
                case SaleErrorCode.ReductionWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ReductionWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxReduction.Focus();

                    break;
                }
                case SaleErrorCode.ReductionWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ReductionWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxReduction.Focus();

                    break;
                }
                case SaleErrorCode.BrokerageEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BrokerageEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case SaleErrorCode.BrokerageWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BrokerageWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case SaleErrorCode.BrokerageWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BrokerageWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case SaleErrorCode.DocumentDirectoryDoesNotExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DocumentDirectoryDoesNotExists",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxDocument.Focus();

                    break;
                }
                case SaleErrorCode.DocumentFileDoesNotExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DocumentFileDoesNotExists",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxDocument.Focus();

                    break;
                }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                strMessage,
                Language,
                LanguageName,
                clrMessage,
                Logger,
                (int) stateLevel,
                (int) FrmMain.EComponentLevels.Application);
        }

        public void DocumentBrowseFinish()
        {
            // Set messages
            var strMessage = string.Empty;
            var clrMessage = Color.Black;
            const FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case SaleErrorCode.DocumentBrowseFailed:
                {
                    txtBoxDocument.Text = @"-";

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ChoseDocumentFailed", SettingsConfiguration.LanguageName);
                    break;
                }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                strMessage,
                Language,
                LanguageName,
                clrMessage,
                Logger,
                (int) stateLevel,
                (int) FrmMain.EComponentLevels.Application);
        }

        #endregion IViewMember

        #region Event members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValuesEventHandler;
        public event EventHandler SaleChangeEventHandler;
        public event EventHandler AddSaleEventHandler;
        public event EventHandler EditSaleEventHandler;
        public event EventHandler DeleteSaleEventHandler;
        public event EventHandler DocumentBrowseEventHandler;

        #endregion Event members

        #region Methods

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final share object</param>
        /// <param name="logger">Logger</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        /// <param name="parsingFileName">File name of the parsing file which is given via document capture</param>
        public ViewSaleEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue,
            Logger logger, Language xmlLanguage, string language,
            string parsingFileName = null)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;

            Logger = logger;
            Language = xmlLanguage;
            LanguageName = language;

            ParsingFileName = parsingFileName;

            _focusedControl = txtBoxOrderNumber;

            SaveFlag = false;

            #region Parsing backgroundworker

            _parsingBackgroundWorker.WorkerReportsProgress = true;
            _parsingBackgroundWorker.WorkerSupportsCancellation = true;

            // Check if the PDF converter is installed so initialize the background worker for the parsing
            if (Helper.PdfParserInstalled())
            {
                _parsingBackgroundWorker.DoWork += DocumentParsing;
                _parsingBackgroundWorker.ProgressChanged += OnDocumentParsingProgressChanged;
                _parsingBackgroundWorker.RunWorkerCompleted += OnDocumentParsingRunWorkerCompleted;
            }
            
            #endregion Parsing backgroundworker
        }

        /// <summary>
        /// This function sets the language to the labels and buttons
        /// and sets the units behind the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareSalesEdit_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/Caption", SettingsConfiguration.LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);
                grpBoxDocumentPreview.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxDocumentPreview/Caption", SettingsConfiguration.LanguageName);
                grpBoxSales.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/Caption",
                        LanguageName);
                lblAddSaleDate.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Date",
                        LanguageName);
                lblSaleDepotNumber.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/DepotNumber",
                        LanguageName);
                lblSaleOrderNumber.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/OrderNumber",
                        LanguageName);
                lblVolume.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Volume",
                        LanguageName);
                lblSalePrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SalesPrice",
                        LanguageName);
                lblBuyValue.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/BuyValue",
                        LanguageName);
                btnSalesBuyDetails.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/BuyValueDetails",
                        LanguageName);
                lblTaxAtSource.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/TaxAtSource",
                        LanguageName);
                lblCapitalGainsTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/CapitalGainsTax",
                        LanguageName);
                lblSolidarityTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SolidarityTax",
                        LanguageName);
                lblProvision.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Provision",
                        LanguageName);
                lblBrokerFee.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/BrokerFee",
                        LanguageName);
                lblTraderPlaceFee.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/TraderPlaceFee",
                        LanguageName);
                lblReduction.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Reduction",
                        LanguageName);
                lblBrokerage.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Brokerage",
                        LanguageName);
                lblProfitLoss.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/ProfitLoss",
                        LanguageName);
                lblPayout.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Payout",
                        LanguageName);
                lblSalesDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add",
                        LanguageName);
                btnReset.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Reset",
                        LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                #endregion Language configuration

                #region Unit configuration

                // Set sale units to the edit boxes
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblSalePriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxAtSourceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCapitalGainsTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSolidarityTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblProvisionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblBrokerFeeUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTraderPlaceFeeUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblReductionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblBrokerageUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblProfitLossUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPayoutUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.button_add_24;
                btnDelete.Image = Resources.button_recycle_bin_24;
                btnReset.Image = Resources.button_reset_24;
                btnCancel.Image = Resources.button_back_24;

                #endregion Image configuration

                #region Get depot numbers and the corresponding banks

                var listBankRegex = DocumentParsingConfiguration.BankRegexList;

                cbxDepotNumber.Items.Add(@"-");
                foreach (var bankRegex in listBankRegex)
                {
                    cbxDepotNumber.Items.Add(bankRegex.BankIdentifier + @" - " + bankRegex.BankName);
                }

                #endregion Get depot numbers and the corresponding banks

                OnShowSales();

                // If a parsing file name is given the form directly starts with the document parsing
                if (ParsingFileName != null)
                {
                    _parsingStartAllow = true;
                    txtBoxDocument.Text = ParsingFileName;
                }

                txtBoxOrderNumber.Focus();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a buy has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareSalesEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup web browser
            Helper.WebBrowserPdf.CleanUp(webBrowser1);

            // Check if a sale change must be saved
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date
        /// </summary>
        private void ResetValues()
        {
            // Set reset flag
            ResetRunning = true;

            // Reset state pictures
            picBoxDateParseState.Image = Resources.empty_arrow;
            picBoxTimeParseState.Image = Resources.empty_arrow;
            picBoxDepotNumberParseState.Image = Resources.empty_arrow;
            picBoxOrderNumberParserState.Image = Resources.empty_arrow;
            picBoxVolumeParseState.Image = Resources.empty_arrow;
            picBoxPriceParseState.Image = Resources.empty_arrow;
            picBoxTaxAtSourceParseState.Image = Resources.empty_arrow;
            picBoxCapitalGainTaxParseState.Image = Resources.empty_arrow;
            picBoxSolidarityTaxParseState.Image = Resources.empty_arrow;
            picBoxProvisionParseState.Image = Resources.empty_arrow;
            picBoxBrokerFeeParseState.Image = Resources.empty_arrow;
            picBoxTraderPlaceFeeParseState.Image = Resources.empty_arrow;
            picBoxReductionParseState.Image = Resources.empty_arrow;

            // Reset and enable date time picker
            dateTimePickerDate.Value = DateTime.Now;
            dateTimePickerDate.Enabled = true;
            dateTimePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePickerTime.Enabled = true;

            // Reset and enable combo box
            if (!DepotNumberChangeRunning)
            {
                cbxDepotNumber.SelectedIndex = 0;
                cbxDepotNumber.Enabled = true;
            }

            // Reset textboxes
            txtBoxOrderNumber.Text = string.Empty;
            txtBoxOrderNumber.Enabled = true;
            txtBoxVolume.Text = string.Empty;
            txtBoxVolume.Enabled = true;
            txtBoxSalePrice.Text = string.Empty;
            txtBoxSalePrice.Enabled = true;
            txtBoxSaleBuyValue.Text = string.Empty;
            txtBoxSaleBuyValue.Enabled = true;
            txtBoxProfitLoss.Text = string.Empty;
            txtBoxProfitLoss.Enabled = true;
            txtBoxTaxAtSource.Text = string.Empty;
            txtBoxTaxAtSource.Enabled = true;
            txtBoxCapitalGainsTax.Text = string.Empty;
            txtBoxCapitalGainsTax.Enabled = true;
            txtBoxSolidarityTax.Text = string.Empty;
            txtBoxSolidarityTax.Enabled = true;
            txtBoxProvision.Text = string.Empty;
            txtBoxProvision.Enabled = true;
            txtBoxBrokerFee.Text = string.Empty;
            txtBoxBrokerFee.Enabled = true;
            txtBoxTraderPlaceFee.Text = string.Empty;
            txtBoxTraderPlaceFee.Enabled = true;
            txtBoxReduction.Text = string.Empty;
            txtBoxReduction.Enabled = true;
            txtBoxBrokerage.Text = string.Empty;
            txtBoxBrokerage.Enabled = true;
            txtBoxPayout.Text = string.Empty;
            txtBoxPayout.Enabled = true;

            // Do not reset document value if a parsing is running
            if (!_parsingStartAllow && !DepotNumberChangeRunningFlag)
            {
                txtBoxDocument.Text = string.Empty;
                txtBoxDocument.Enabled = true;
            }

            // Reset status strip
            toolStripStatusLabelMessageSaleEdit.Text = string.Empty;
            toolStripStatusLabelMessageSaleDocumentParsing.Text = string.Empty;
            toolStripProgressBarSaleDocumentParsing.Visible = false;

            // Check if there is any volume left for the current selected depot number
            if(ShareObjectFinalValue.SalableVolume > 0)
            {
                // Set salable volume of the current selected depot number
                txtBoxVolume.Text =
                    ShareObjectFinalValue.SalableVolumeAsStr;

                // Set current share price
                txtBoxSalePrice.Text = ShareObjectFinalValue.CurPriceAsStr;

                dateTimePickerDate.Enabled = true;
                dateTimePickerTime.Enabled = true;

                txtBoxOrderNumber.Enabled = true;
                txtBoxVolume.Enabled = true;
                txtBoxSalePrice.Enabled = true;

                btnSalesBuyDetails.Enabled = true;

                txtBoxTaxAtSource.Enabled = true;
                txtBoxCapitalGainsTax.Enabled = true;
                txtBoxSolidarityTax.Enabled = true;

                txtBoxProvision.Enabled = true;
                txtBoxBrokerFee.Enabled = true;
                txtBoxTraderPlaceFee.Enabled = true;
                txtBoxReduction.Enabled = true;

                txtBoxDocument.Enabled = true;
                btnSalesDocumentBrowse.Enabled = true;

                btnAddSave.Enabled = true;
            }
            else
            {
                dateTimePickerDate.Enabled = false;
                dateTimePickerTime.Enabled = false;

                txtBoxOrderNumber.Enabled = false;
                txtBoxVolume.Enabled = false;
                txtBoxSalePrice.Enabled = false;

                btnSalesBuyDetails.Enabled = false;

                txtBoxTaxAtSource.Enabled = false;
                txtBoxCapitalGainsTax.Enabled = false;
                txtBoxSolidarityTax.Enabled = false;

                txtBoxProvision.Enabled = false;
                txtBoxBrokerFee.Enabled = false;
                txtBoxTraderPlaceFee.Enabled = false;
                txtBoxReduction.Enabled = false;

                //txtBoxDocument.Enabled = false;
                //btnSalesDocumentBrowse.Enabled = false;

                btnAddSave.Enabled = false;
            }

            // Enable button(s)
            btnAddSave.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName);
            btnAddSave.Image = Resources.button_add_24;

            // Disable button(s)
            btnDelete.Enabled = false;

            // Rename group box
            grpBoxAdd.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);

            // Reset dialog icon to add
            Icon = Resources.add;

            // Deselect rows
            OnDeselectRowsOfDataGridViews(null);

            // Reset stored DataGridView instance
            SelectedDataGridView = null;

            // Select overview tab
            if (tabCtrlSales.TabPages.Count > 0)
                tabCtrlSales.SelectTab(0);

            // Reset document web browser
            Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);

            dateTimePickerDate.Focus();

            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());

            // Reset reset running flag
            ResetRunning = false;
        }

        #endregion Form

        #region Date Time

        /// <summary>
        /// This function updates the model if the date has changed
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerDate_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerDate_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the date time picker for the date to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerDate_Enter(object sender, EventArgs e)
        {
            _focusedControl = dateTimePickerDate;
        }

        /// <summary>
        /// This function updates the model if the time has changed
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerTime_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerTime_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the date time picker for the time to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerTime_Enter(object sender, EventArgs e)
        {
            _focusedControl = dateTimePickerTime;

        }

        #endregion Date Time

        #region Combo box

        /// <summary>
        /// This function updates the model if the combo box selection has changed
        /// </summary>
        /// <param name="sender">Combo box</param>
        /// <param name="e">EventArgs</param>
        private void OnCbxDepotNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            DepotNumberChangeRunningFlag = true;

            if (!LoadSaleRunning)
                ResetValues();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepotNumber"));

            DepotNumberChangeRunningFlag = false;
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Combo box</param>
        /// <param name="e">EventArgs</param>
        private void OnCbxDepotNumber_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the combo box to the focused control
        /// </summary>
        /// <param name="sender">Combo box</param>
        /// <param name="e">EventArgs</param>
        private void OnCbxDepotNumber_Enter(object sender, EventArgs e)
        {
            _focusedControl = cbxDepotNumber;
        }

        #endregion Combo box

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddOrderNumber_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OrderNumber"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxOrderNumber_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxOrderNumber_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxOrderNumber;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddVolume_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Volume"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxVolume_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxVolume_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxVolume;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSalePrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SalePrice"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSalePrice_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());

        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSalePrice_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxSalePrice;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTaxAtSource_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaxAtSource"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTaxAtSourceLeave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTaxAtSource_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxTaxAtSource;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCapitalGainsTax_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapitalGainsTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCapitalGainsTaxLeave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCapitalGainsTax_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxCapitalGainsTax;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSolidarityTax_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolidarityTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSolidarityTaxLeave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSolidarityTax_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxSolidarityTax;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxProvision_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Provision"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxProvision_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxProvision_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxProvision;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerFee_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BrokerFee"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerFee_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerFee_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxBrokerFee;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TraderPlaceFee"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxTraderPlaceFee;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReduction_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reduction"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReductionLeave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReduction_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxReduction;
        }

        /// <summary>
        /// This function only sets the document of the model to the view
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Document"));

            if (_parsingStartAllow)
            {
                if (_parsingBackgroundWorker.IsBusy)
                {
                    _parsingBackgroundWorker.CancelAsync();
                }
                else
                {
                    ResetValues();
                    _parsingBackgroundWorker.RunWorkerAsync();
                }
            }

            _parsingStartAllow = false;
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxDocument;
        }

        /// <summary>
        /// This function allows to sets via Drag and Drop a document for this brokerage
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// This function shows the Drop sign
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                if (ShowSalesRunning) return;

                // Check if the parsing should be allowed
                if (ShareObjectFinalValue.AllSaleEntries.IsLastSale(SelectedGuid) ||
                    SelectedGuid == string.Empty
                )
                    _parsingStartAllow = true;
                else
                    _parsingStartAllow = false;

                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                if (files.Length <= 0 || files.Length > 1) return;

                txtBoxDocument.Text = files[0];

                // Check if the document is a PDF
                var extenstion = Path.GetExtension(txtBoxDocument.Text);

                if (string.Compare(extenstion, ".PDF", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (_parsingBackgroundWorker.IsBusy)
                    {
                        _parsingBackgroundWorker.CancelAsync();
                    }
                    else
                    {
                        ResetValues();
                        _parsingBackgroundWorker.RunWorkerAsync();
                    }
                }

                _parsingStartAllow = false;

                Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                toolStripStatusLabelMessageSaleDocumentParsing.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingFailed", SettingsConfiguration.LanguageName);

                toolStripProgressBarSaleDocumentParsing.Visible = false;
                grpBoxAdd.Enabled = true;
                grpBoxSales.Enabled = true;

                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (DocumentTypeParser != null)
                    DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;

                if(_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);
            }
        }

        #endregion TextBoxes

        #region Buttons

        /// <summary>
        /// This function shows the list of the used buys
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnSalesBuyDetails_Click(object sender, EventArgs e)
        {
            var strMessage = string.Empty;

            // Loop through the buy details which are used for this sale
            foreach (var saleBuyDetails in UsedBuyDetails)
            {
                strMessage += saleBuyDetails.SaleBuyDetailsData;
                strMessage += "\r\n";
            }

            // Result of the used buy details
            strMessage +=
                " ============================================================================================\r\n";
            const string format = "  {0,10}   {1,15}   {2,15}   {3,10}   {4,10} = {5,15}";
            var strMessageResult = string.Format(format,
                @"",
                @"",
                @"",
                @"",
                @"",
                Helper.FormatDecimal(UsedBuyDetails.Sum(x => x.SaleBuyValueBrokerageReduction),
                    Helper.CurrencyTwoLength, true,
                    Helper.CurrencyTwoFixLength));

            strMessage += strMessageResult;

            // Create form with the used buy details and the result
            var showOwnMessageBox =
                new UsedBuyDetailsList.UsedBuyDetailsList(
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/UsedBuyDetailsForm/Caption", SettingsConfiguration.LanguageName),
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/UsedBuyDetailsForm/GroupBox", SettingsConfiguration.LanguageName),
                    strMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/UsedBuyDetailsForm/Button", SettingsConfiguration.LanguageName)
                );

            showOwnMessageBox.ShowDialog();
        }

        /// <summary>
        /// This function adds a new sale entry to the share object
        /// or edit a sale entry
        /// It also checks if an entry already exists for the given date and time
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls
                Enabled = false;

                // Reset status strip
                toolStripStatusLabelMessageSaleEdit.Text = string.Empty;
                toolStripStatusLabelMessageSaleDocumentParsing.Text = string.Empty;
                toolStripProgressBarSaleDocumentParsing.Visible = false;

                if (btnAddSave.Text ==
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName))
                {
                    AddSale = true;
                    UpdateSale = false;

                    AddSaleEventHandler?.Invoke(this, null);
                }
                else
                {
                    AddSale = false;
                    UpdateSale = true;

                    EditSaleEventHandler?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the sale.
        /// If the user press "Ok" the sale will be deleted and the
        /// list of the sales will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">EventArgs</param>
        private void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                // Disable controls
                Enabled = false;

                // Reset status strip
                toolStripStatusLabelMessageSaleEdit.Text = string.Empty;
                toolStripStatusLabelMessageSaleDocumentParsing.Text = string.Empty;
                toolStripProgressBarSaleDocumentParsing.Text = string.Empty;

                var strCaption =
                    Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                        (int) EOwnMessageBoxInfoType.Info];
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/SaleDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok",
                    LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel",
                    LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel, EOwnMessageBoxInfoType.Info);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult != DialogResult.OK) return;

                // Set flag to save the share object.
                SaveFlag = true;

                // Check if a row is selected
                if (SelectedDataGridView != null && SelectedDataGridView.SelectedRows.Count == 1)
                {
                    DeleteSaleEventHandler?.Invoke(this, null);
                }

                // Reset values
                ResetValues();

                // Enable button(s)
                btnAddSave.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName);
                btnAddSave.Image = Resources.button_add_24;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);

                // Reset dialog icon to add
                Icon = Resources.add;

                // Refresh the dividend list
                OnShowSales();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a sale
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset add flag
                AddSale = false;
                // Reset update flag
                UpdateSale = false;

                // Reset values
                ResetValues();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CancelFailure", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function closes the dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            SaleChangeEventHandler?.Invoke(this, null);

            Close();
        }

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the buy
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnBuyDocumentBrowse_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMessageSaleEdit.Text = string.Empty;
            toolStripStatusLabelMessageSaleDocumentParsing.Text = string.Empty;

            DocumentBrowseEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Buttons

        #region Group box overview

        /// <summary>
        /// This function sets the focus back to the last focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnGrpBoxOverview_MouseLeave(object sender, EventArgs e)
        {
            if (_focusedControl is TextBox box)
            {
                box.Select();
                box.Select(box.Text.Length, 0); // To set cursor at the end of TextBox
            }
            else
            {
                _focusedControl?.Focus();
            }
        }

        #endregion Group box overview

        #region Data grid view

        /// <summary>
        /// This function paints the sale list of the share
        /// </summary>
        private void OnShowSales()
        {
            try
            {
                // Set show flag
                ShowSalesRunning = true;
                LoadSaleRunning = false;

                // Reset tab control
                foreach (TabPage tabPage in tabCtrlSales.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewSalesOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewSalesOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewSalesOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewSalesOfYears_MouseEnter;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewSalesOfAYear_DataBindingComplete;
                    }

                    tabPage.Controls.Clear();
                    tabCtrlSales.TabPages.Remove(tabPage);
                }

                tabCtrlSales.TabPages.Clear();

                // Enable controls
                Enabled = true;

                #region Add page

                // Create TabPage for the sales of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = @"Overview",

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", SettingsConfiguration.LanguageName)
                           + @" ("
                           + ShareObjectFinalValue.AllSaleEntries.SalePayoutBrokerageReductionTotalAsStrUnit
                           + @" / "
                           + ShareObjectFinalValue.AllSaleEntries.SaleProfitLossBrokerageReductionTotalAsStrUnit
                           + @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Check if sales exists
                if (ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Count <= 0)
                {
                    ResetValues();
                    return;
                }

                // Reverse list so the latest is a top of the data grid view
                var reversDataSourceOverview = ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues()
                    .OrderByDescending(x => x.DgvSaleYear).ToList();

                var bindingSourceOverview = new BindingSource
                {
                    DataSource = reversDataSourceOverview
                };

                // Create DataGridView
                var dataGridViewSalesOverviewOfAYears = new DataGridView
                {
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with sale values to the DataGridView
                    DataSource = bindingSourceOverview,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                };

                // Create DataGridView
                var dataGridViewSalesOverviewOfAYearsFooter = new DataGridView
                {
                    Name = @"Overview" + _dataGridViewFooterPostfix,
                    Dock = DockStyle.Bottom,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing,

                    // Set height via Size height
                    Size = new Size(1, DataGridViewFooterHeight)
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewSalesOverviewOfAYears.DataBindingComplete += OnDataGridViewSalesOfAYear_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewSalesOverviewOfAYears.MouseEnter += OnDataGridViewSalesOfYears_MouseEnter;
                // Set row select event
                dataGridViewSalesOverviewOfAYears.SelectionChanged += OnDataGridViewSalesOfYears_SelectionChanged;

                #endregion Events

                #region Style

                DataGridViewHelper.DataGridViewConfiguration(dataGridViewSalesOverviewOfAYears);

                dataGridViewSalesOverviewOfAYearsFooter.ColumnHeadersVisible = false;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewSalesOverviewOfAYears);
                newTabPageOverviewYears.Controls.Add(dataGridViewSalesOverviewOfAYearsFooter);
                dataGridViewSalesOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlSales.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlSales;

                #endregion Control add

                // Check if sales exists
                if (ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Count <= 0)
                {
                    ShowSalesRunning = false;
                    return;
                }

                // Loop through the years of the sales
                foreach (
                    var keyName in ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Keys.Reverse()
                )
                {
                    #region Add page

                    // Create TabPage
                    var newTabPage = new TabPage
                    {
                        // Set TabPage name
                        Name = keyName,

                        // Set TabPage caption
                        Text = keyName
                               + @" ("
                               + ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                   .SalePayoutBrokerageReductionYearUnitAsStr
                               + @" / "
                               + ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                   .SaleProfitLossBrokerageReductionYearWithUnitAsStr
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Reverse list so the latest is a top of the data grid view
                    var reversDataSource =
                        ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                            .SaleListYear.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();

                    // Create Binding source for the sale data
                    var bindingSource = new BindingSource
                    {
                        DataSource = reversDataSource
                    };

                    // Create DataGridView
                    var dataGridViewSalesOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with sale values to the DataGridView
                        DataSource = bindingSource,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    };

                    // Create DataGridView
                    var dataGridViewSalesOfAYearFooter = new DataGridView
                    {
                        Name = keyName + _dataGridViewFooterPostfix,
                        Dock = DockStyle.Bottom,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing,

                        // Set height via Size height
                        Size = new Size(1, DataGridViewFooterHeight)
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewSalesOfAYear.DataBindingComplete += OnDataGridViewSalesOfAYear_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewSalesOfAYear.MouseEnter += OnDataGridViewSalesOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewSalesOfAYear.SelectionChanged += OnDataGridViewSalesOfAYear_SelectionChanged;

                    #endregion Events

                    #region Style

                    DataGridViewHelper.DataGridViewConfiguration(dataGridViewSalesOfAYear);

                    dataGridViewSalesOfAYearFooter.ColumnHeadersVisible = false;


                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewSalesOfAYear);
                    newTabPage.Controls.Add(dataGridViewSalesOfAYearFooter);
                    dataGridViewSalesOfAYear.Parent = newTabPage;
                    tabCtrlSales.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlSales;

                    #endregion Control add
                }

                tabCtrlSales.TabPages[0].Select();

                // Reset show flag
                ShowSalesRunning = false;
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", SettingsConfiguration.LanguageName),
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
        private void OnDataGridViewSalesOfAYear_DataBindingComplete(object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Local variables of the DataGridView
                var dgvContent = (DataGridView)sender;

                // Set column headers
                for (var i = 0; i < dgvContent.ColumnCount; i++)
                {
                    // Disable sorting of the columns ( remove sort arrow )
                    dgvContent.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    // Get index of the current added column
                    var index = ((DataGridView)tabCtrlSales.TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name + _dataGridViewFooterPostfix]).Columns
                        .Add(dgvContent.Name + _dataGridViewFooterPostfix, "Header " + i);

                    // Disable sorting of the columns ( remove sort arrow )
                    ((DataGridView)tabCtrlSales.TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name + _dataGridViewFooterPostfix]).Columns[index].SortMode =
                        DataGridViewColumnSortMode.NotSortable;

                    switch (i)
                    {
                        case 0:
                        {
                            if (dgvContent.Name == @"Overview")
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Year",
                                        LanguageName);
                            }
                        }
                            break;
                        case 1:
                        {
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (dgvContent.Name == @"Overview")
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Volume",
                                        LanguageName);
                            }
                            else
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Date",
                                        LanguageName);
                            }
                        }
                            break;
                        case 2:
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (dgvContent.Name == @"Overview")
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Payout",
                                        LanguageName);
                            }
                            else
                            {
                                dgvContent.Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Purchase",
                                        LanguageName);
                            }

                            break;
                        case 3:
                            dgvContent.Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_ProfitLoss",
                                    LanguageName);
                            break;
                        case 4:
                            dgvContent.Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Sale",
                                    LanguageName);
                            break;
                        case 5:
                            dgvContent.Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Document",
                                    LanguageName);
                            break;
                    }
                }

                if (dgvContent.Rows.Count > 0)
                {
                    dgvContent.Rows[0].Selected = false;
                    dgvContent.ScrollBars = ScrollBars.Both;
                }

                if (dgvContent.Name != @"Overview")
                {
                    // Hide first column with the GUID
                    ((DataGridView)tabCtrlSales.TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name]).Columns[0].Visible =
                        false;

                    ((DataGridView)tabCtrlSales.TabPages[dgvContent.Name]
                            .Controls[dgvContent.Name + _dataGridViewFooterPostfix]).Columns[0].Visible =
                        false;

                    #region Column size content

                    // Content DataGridView column width resize
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesDate].Width =
                        YearDateColumnSize;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesBuyValue].Width =
                        YearBuyValueColumnSize;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesProfitLoss].Width =
                        YearProfitLossColumnSize;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesPayout].Width =
                        YearPayoutColumnSize;

                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvContent.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].FillWeight = 10;

                    #endregion Column size content

                    #region Column size footer

                    // Get footer DataGridView
                    var dgvFooter = (DataGridView)tabCtrlSales.TabPages[dgvContent.Name]
                        .Controls[dgvContent.Name + _dataGridViewFooterPostfix];

                    // Footer DataGridView column width resize
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesDate].Width =
                        YearDateColumnSize;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesBuyValue].Width =
                        YearBuyValueColumnSize;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesProfitLoss].Width =
                        YearProfitLossColumnSize;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesPayout].Width =
                        YearPayoutColumnSize;

                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesDocument].FillWeight = 10;

                    #endregion Column size footer

                    // Get culture info
                    var cultureInfo = ShareObjectFinalValue.AllSaleEntries
                        .AllSalesOfTheShareDictionary[dgvContent.Name]
                        .SaleCultureInfo;

                    #region Purchase

                    var buyValue =
                        ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[dgvContent.Name]
                            .SaleListYear.Sum(x => x.BuyValueBrokerageReduction);

                    // HINT: If any format is changed here it must also be changed in file "SaleObject.cs" at the property "BuyValueBrokerageReductionAsStr"
                    var buyValueFormatted = buyValue > 0
                        ? Helper.FormatDecimal(buyValue, Helper.CurrencyTwoLength, true,
                            Helper.CurrencyTwoFixLength, true, @"", cultureInfo)
                        : @"-";

                    #endregion Purchase

                    #region Profit / Loss

                    var profitLoss =
                        ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[dgvContent.Name]
                            .SaleListYear.Sum(x => x.ProfitLossBrokerageReduction);

                    // HINT: If any format is changed here it must also be changed in file "SaleObject.cs" at the property "ProfitLossBrokerageReductionAsStr"
                    var profitLossFormatted = Helper.FormatDecimal(profitLoss, Helper.CurrencyTwoLength, true,
                            Helper.CurrencyTwoFixLength, true, @"", cultureInfo);

                    #endregion Profit / Loss

                    #region SaleValue

                    var saleValue =
                        ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[dgvContent.Name]
                            .SaleListYear.Sum(x => x.PayoutBrokerageReduction);

                    // HINT: If any format is changed here it must also be changed in file "BuyObject.cs" at the property "PayoutBrokerageReductionAsStr"
                    var saleValueFormatted = Helper.FormatDecimal(saleValue, Helper.CurrencyTwoLength, true,
                        Helper.CurrencyTwoFixLength, true, @"", cultureInfo);

                    #endregion SaleValue

                    // Footer with sum values
                    if (dgvFooter.Rows.Count == 1)
                        dgvFooter.Rows.Add("",
                            Language.GetLanguageTextByXPath(
                                @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/Footer_Totals",
                                SettingsConfiguration.LanguageName),
                            buyValueFormatted, profitLossFormatted, saleValueFormatted, "-");

                    // Check if the vertical scrollbar is shown
                    if (dgvContent.RowCount > DataGridViewRowsScrollbar)
                    {
                        dgvFooter.Columns.Add("Scrollbar", "Scrollbar");

                        dgvFooter.Columns[(int)DataGridViewYearIndices.ColumnIndicesScrollBar].Width =
                            SystemInformation.VerticalScrollBarWidth;

                        // Disable sorting of the columns ( remove sort arrow )
                        dgvFooter.Columns[dgvFooter.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    DataGridViewHelper.DataGridViewConfigurationFooter(dgvFooter);
                }
                else
                {
                    #region Column size content

                    // Content DataGridView column width resize
                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesYear].Width =
                        OverViewDateColumnSize;
                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesVolume].Width =
                        OverViewVolumeColumnSize;
                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesPayout].Width =
                        OverViewPayoutColumnSize;

                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesProfitLoss].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvContent.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesProfitLoss].FillWeight = 10;

                    #endregion Column size content

                    #region Columns size footer

                    // Get footer DataGridView
                    var dgvFooter = (DataGridView)tabCtrlSales.TabPages[dgvContent.Name]
                        .Controls[dgvContent.Name + _dataGridViewFooterPostfix];

                    // Footer DataGridView column width resize
                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesYear].Width =
                        OverViewDateColumnSize;
                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesVolume].Width =
                        OverViewVolumeColumnSize;
                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesPayout].Width =
                        OverViewPayoutColumnSize;

                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesProfitLoss].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesProfitLoss].FillWeight = 10;

                    #endregion Column size footer

                    // Get culture info
                    var cultureInfo = ShareObjectFinalValue.AllSaleEntries.SaleCultureInfo;

                    #region Volume

                    var volume =
                        ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Sum(x => x.SaleVolumeYear);

                    // HINT: If any format is changed here it must also be changed in file "SalesOfAYear.cs" at the property "DgvSaleVolumeYear"
                    var volumeFormatted = volume > 0
                        ? Helper.FormatDecimal(volume, Helper.CurrencyFiveLength, false,
                            Helper.CurrencyTwoFixLength, true, ShareObject.PieceUnit, cultureInfo)
                        : @"-";

                    #endregion Voluem

                    #region Payout

                    var payout =
                        ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Sum(x => x.SalePayoutBrokerageReductionYear);

                    // HINT: If any format is changed here it must also be changed in file "SalesOfAYear.cs" at the property "DgvSalePayoutYearAsStr"
                    var payoutFormatted = Helper.FormatDecimal(payout, Helper.CurrencyTwoLength, true,
                        Helper.CurrencyTwoFixLength, true, @"", cultureInfo);

                    #endregion Payout

                    #region Profit / Loss

                    var profitLoss =
                        ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Sum(x => x.SaleProfitLossBrokerageReductionYear);

                    // HINT: If any format is changed here it must also be changed in file "SalesOfAYear.cs" at the property "DgvSaleProfitLossYearAsStr"
                    var profitLossFormatted = Helper.FormatDecimal(profitLoss, Helper.CurrencyTwoLength, true,
                        Helper.CurrencyTwoFixLength, true, @"", cultureInfo);

                    #endregion Profit / Loss

                    // Footer with sum values
                    if (dgvFooter.Rows.Count == 1)
                        dgvFooter.Rows.Add(
                            Language.GetLanguageTextByXPath(
                                @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/Footer_Totals",
                                SettingsConfiguration.LanguageName),
                            volumeFormatted, payoutFormatted, profitLossFormatted);

                    // Check if the vertical scrollbar is shown
                    if (dgvContent.RowCount > DataGridViewRowsScrollbar)
                    {
                        dgvFooter.Columns.Add("Scrollbar", "Scrollbar");

                        dgvFooter.Columns[(int)DataGridViewOverViewIndices.ColumnIndicesScrollBar].Width =
                            SystemInformation.VerticalScrollBarWidth;

                        // Disable sorting of the columns ( remove sort arrow )
                        dgvFooter.Columns[dgvFooter.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    DataGridViewHelper.DataGridViewConfigurationFooter(dgvFooter);
                }

                // Reset the text box values
                ResetValues();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/RenameColHeaderFailed", SettingsConfiguration.LanguageName),
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
        private void OnDeselectRowsOfDataGridViews(DataGridView dataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage tabPage in tabCtrlSales.TabPages)
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

                SelectedGuid = string.Empty;
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeselectFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #region Tab control delegates

        /// <summary>
        /// This function sets the focus on the data grid view of the new selected tab page of the tab control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlSales_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabCtrlSales.SelectedTab == null)
                {
                    SelectedGuid = string.Empty;
                    SelectedGuidLast = string.Empty;

                    return;
                }

                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlSales.SelectedTab.Controls)
                {
                    // Check if the control is DataGridView or not
                    if (!(control is DataGridView view)) continue;

                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                    {
                        if (view.Rows[0].Cells.Count > 0)
                        {
                            view.Rows[0].Selected = true;
                        }

                        view.Focus();
                    }

                    if (view.Name == @"Overview")
                    {
                        ResetValues();

                        SaleChangeEventHandler?.Invoke(this, null);

                        SelectedGuid = string.Empty;
                        SelectedGuidLast = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function sets the focus to the group box add / edit when the mouse leaves the data grid view
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlSales_MouseLeave(object sender, EventArgs e)
        {
            if (_focusedControl is TextBox box)
            {
                box.Select();
                box.Select(box.Text.Length, 0); // To set cursor at the end of TextBox
            }
            else
            {
                _focusedControl?.Focus();
            }
        }

        /// <summary>
        /// This function sets the focus on the last focused control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (_focusedControl is TextBox box)
            {
                box.Select();
                box.Select(box.Text.Length, 0); // To set cursor at the end of TextBox
            }
            else
            {
                _focusedControl?.Focus();
            }
        }

        /// <summary>
        /// This function sets key value ( char ) to the last focused control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlSales_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the last focused control was a text box so set the
            // key value ( char ) to the text box and then set the cursor behind the text
            if (_focusedControl is TextBox textBox)
            {
                textBox.Text = e.KeyChar.ToString();
                textBox.Select(textBox.Text.Length, 0);
            }

            // Check if the last focused control was a date time picker
            if (_focusedControl is DateTimePicker dateTimePicker)
            {
                // Check if the pressed key was a numeric key
                if (e.KeyChar == '0' ||
                    e.KeyChar == '1' ||
                    e.KeyChar == '2' ||
                    e.KeyChar == '3' ||
                    e.KeyChar == '4' ||
                    e.KeyChar == '5' ||
                    e.KeyChar == '6' ||
                    e.KeyChar == '7' ||
                    e.KeyChar == '8' ||
                    e.KeyChar == '9'
                )
                    dateTimePicker.Text = e.KeyChar.ToString();
            }

        }

        #endregion Tab control delegates

        #region Sales of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewSalesOfYears_SelectionChanged(object sender, EventArgs args)
        {
            // Check if a sale show is running break
            if (ShowSalesRunningFlag) return;

            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView) sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlSales.TabPages)
                {
                    if (curItem[0].Cells[1].Value != null &&
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()
                    ) continue;

                    tabCtrlSales.SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", SettingsConfiguration.LanguageName),
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
        private static void OnDataGridViewSalesOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
        }

        #endregion Sales of years

        #region Sales of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewSalesOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            // Check if a sale show is running break
            if (ShowSalesRunningFlag) return;

            try
            {
                // Set load running flag
                LoadSaleRunning = true;

                if (tabCtrlSales.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlSales.SelectedTab.Controls.Contains((DataGridView) sender))
                        OnDeselectRowsOfDataGridViews((DataGridView) sender);
                }

                // If it is "1" a selection change has been made
                // else an deselection has been made ( switch to the overview tab )
                if (((DataGridView) sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    var curItem = ((DataGridView) sender).SelectedRows;

                    // Set selected date
                    if (curItem[0].Cells[1].Value != null)
                        SelectedDate = curItem[0].Cells[1].Value.ToString();
                    else
                        return;

                    // Set selected Guid
                    if (curItem[0].Cells[1].Value != null)
                        SelectedGuid = curItem[0].Cells[0].Value.ToString();
                    else
                        return;

                    // Get selected sale object by Guid
                    var selectedSaleObject = ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare()
                        .Find(x => x.Guid == SelectedGuid);

                    if (!ShowSalesRunning)
                    {
                        if (selectedSaleObject != null)
                        {
                            dateTimePickerDate.Value = Convert.ToDateTime(selectedSaleObject.Date);
                            dateTimePickerTime.Value = Convert.ToDateTime(selectedSaleObject.Date);

                            var index = cbxDepotNumber.FindString(selectedSaleObject.DepotNumber);
                            cbxDepotNumber.SelectedIndex = index > -1 ? index : 0;

                            txtBoxOrderNumber.Text = selectedSaleObject.OrderNumberAsStr;
                            txtBoxVolume.Text = selectedSaleObject.VolumeAsStr;
                            txtBoxSalePrice.Text = selectedSaleObject.SalePriceAsStr;
                            txtBoxTaxAtSource.Text = selectedSaleObject.TaxAtSourceAsStr;
                            txtBoxCapitalGainsTax.Text = selectedSaleObject.CapitalGainsTaxAsStr;
                            txtBoxSolidarityTax.Text = selectedSaleObject.SolidarityTaxAsStr;
                            txtBoxProvision.Text = selectedSaleObject.ProvisionAsStr;
                            txtBoxBrokerFee.Text = selectedSaleObject.BrokerFeeAsStr;
                            txtBoxTraderPlaceFee.Text = selectedSaleObject.TraderPlaceFeeAsStr;
                            txtBoxReduction.Text = selectedSaleObject.ReductionAsStr;
                            txtBoxProfitLoss.Text = selectedSaleObject.ProfitLossBrokerageReductionAsStr;
                            txtBoxPayout.Text = selectedSaleObject.PayoutBrokerageReductionAsStr;
                            txtBoxDocument.Text = selectedSaleObject.DocumentAsStr;

                            if (ShareObjectFinalValue.AllSaleEntries.IsLastSale(SelectedGuid))
                            {
                                // Check if the delete button should be enabled or not
                                btnDelete.Enabled = ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare().Count > 0;
                                btnAddSave.Enabled = ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare().Count > 0;
                                btnSalesDocumentBrowse.Enabled = ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare().Count > 0;

                                // Disable CheckBox
                                cbxDepotNumber.Enabled = false;

                                // Enable TextBox(es)
                                dateTimePickerDate.Enabled = true;
                                dateTimePickerTime.Enabled = true;
                                txtBoxOrderNumber.Enabled = true;
                                txtBoxVolume.Enabled = true;
                                txtBoxSalePrice.Enabled = true;
                                txtBoxTaxAtSource.Enabled = true;
                                txtBoxCapitalGainsTax.Enabled = true;
                                txtBoxSolidarityTax.Enabled = true;
                                txtBoxProvision.Enabled = true;
                                txtBoxBrokerFee.Enabled = true;
                                txtBoxTraderPlaceFee.Enabled = true;
                                txtBoxReduction.Enabled = true;
                                txtBoxDocument.Enabled = true;
                            }
                            else
                            {
                                // Disable Button(s)
                                btnDelete.Enabled = false;

                                // Disable Check box
                                cbxDepotNumber.Enabled = false;

                                // Disable TextBox(es)
                                dateTimePickerDate.Enabled = false;
                                dateTimePickerTime.Enabled = false;
                                txtBoxOrderNumber.Enabled = false;
                                txtBoxVolume.Enabled = false;
                                txtBoxSalePrice.Enabled = false;
                                txtBoxTaxAtSource.Enabled = false;
                                txtBoxCapitalGainsTax.Enabled = false;
                                txtBoxSolidarityTax.Enabled = false;
                                txtBoxProvision.Enabled = false;
                                txtBoxBrokerFee.Enabled = false;
                                txtBoxTraderPlaceFee.Enabled = false;
                                txtBoxReduction.Enabled = false;
                            }

                            btnSalesBuyDetails.Enabled = true;
                            btnReset.Enabled = true;

                            // Rename button
                            btnAddSave.Text =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Save",
                                    LanguageName);
                            btnAddSave.Image = Resources.button_pencil_24;

                            // Rename group box
                            grpBoxAdd.Text =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Edit_Caption",
                                    LanguageName);

                            // Reset dialog icon to edit
                            Icon = Resources.edit;

                            // Store DataGridView instance
                            SelectedDataGridView = (DataGridView)sender;

                            // Format the input value
                            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
                        }
                        else
                        {
                            // Reset and disable date time picker
                            dateTimePickerDate.Value = DateTime.Now;
                            dateTimePickerDate.Enabled = false;
                            dateTimePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            dateTimePickerTime.Enabled = false;

                            // Reset and disable text boxes
                            txtBoxOrderNumber.Text = string.Empty;
                            txtBoxOrderNumber.Enabled = false;
                            txtBoxVolume.Text = string.Empty;
                            txtBoxVolume.Enabled = false;
                            txtBoxSalePrice.Text = string.Empty;
                            txtBoxSalePrice.Enabled = false;
                            txtBoxSaleBuyValue.Text = string.Empty;
                            txtBoxSaleBuyValue.Enabled = false;
                            txtBoxProfitLoss.Text = string.Empty;
                            txtBoxProfitLoss.Enabled = false;

                            txtBoxTaxAtSource.Text = string.Empty;
                            txtBoxTaxAtSource.Enabled = false;
                            txtBoxCapitalGainsTax.Text = string.Empty;
                            txtBoxCapitalGainsTax.Enabled = false;
                            txtBoxSolidarityTax.Text = string.Empty;
                            txtBoxSolidarityTax.Enabled = false;

                            txtBoxProvision.Text = string.Empty;
                            txtBoxProvision.Enabled = false;
                            txtBoxBrokerFee.Text = string.Empty;
                            txtBoxBrokerFee.Enabled = false;
                            txtBoxTraderPlaceFee.Text = string.Empty;
                            txtBoxTraderPlaceFee.Enabled = false;
                            txtBoxReduction.Text = string.Empty;
                            txtBoxReduction.Enabled = false;
                            txtBoxBrokerage.Text = string.Empty;
                            txtBoxBrokerage.Enabled = false;

                            txtBoxPayout.Text = string.Empty;
                            txtBoxPayout.Enabled = false;
                            txtBoxDocument.Text = string.Empty;
                            txtBoxDocument.Enabled = false;

                            // Reset status label message text
                            toolStripStatusLabelMessageSaleEdit.Text = string.Empty;
                            toolStripStatusLabelMessageSaleDocumentParsing.Text = string.Empty;
                            toolStripProgressBarSaleDocumentParsing.Visible = false;

                            // Disable Button(s)
                            btnSalesBuyDetails.Enabled = false;
                            btnSalesDocumentBrowse.Enabled = false;
                            btnAddSave.Enabled = false;
                            btnDelete.Enabled = false;

                            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                                (int)FrmMain.EComponentLevels.Application);
                        }
                    }
                }
                else
                {
                    // Rename button
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);

                    // Reset dialog icon to add
                    Icon = Resources.add;

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Enable Button(s)
                    btnAddSave.Enabled = true;
                    btnSalesDocumentBrowse.Enabled = true;

                    // Enable date time picker
                    dateTimePickerDate.Enabled = true;
                    dateTimePickerTime.Enabled = true;

                    // Enable text box(es)
                    txtBoxOrderNumber.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxSalePrice.Enabled = true;
                    txtBoxSaleBuyValue.Enabled = true;
                    txtBoxProfitLoss.Enabled = true;

                    txtBoxTaxAtSource.Enabled = true;
                    txtBoxCapitalGainsTax.Enabled = true;
                    txtBoxSolidarityTax.Enabled = true;

                    txtBoxProvision.Enabled = true;
                    txtBoxBrokerFee.Enabled = true;
                    txtBoxTraderPlaceFee.Enabled = true;
                    txtBoxReduction.Enabled = true;
                    txtBoxBrokerage.Enabled = true;

                    txtBoxPayout.Enabled = true;
                    txtBoxDocument.Enabled = true;

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }

                SaleChangeEventHandler?.Invoke(this, null);

                if (!ShowSalesRunning)
                {
                    if (SelectedGuid != SelectedGuidLast)
                        SelectedGuidLast = SelectedGuid;
                }

                // Check if the file still exists or no document is set
                if (File.Exists(txtBoxDocument.Text) || txtBoxDocument.Text == @"")
                {
                    Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);
                }
                else
                {
                    if (ShowSalesRunning) return;

                    var strCaption =
                        Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                            (int)EOwnMessageBoxInfoType.Error];
                    var strMessage =
                        Language.GetLanguageTextByXPath(
                            @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                            LanguageName);
                    var strOk =
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                            LanguageName);
                    var strCancel =
                        Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                            LanguageName);

                    var messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                        strCancel, EOwnMessageBoxInfoType.Error);

                    // Check if the user pressed cancel
                    if (messageBox.ShowDialog() == DialogResult.Cancel) return;

                    // Get the current selected row
                    var curItem = ((DataGridView)sender).SelectedRows;
                    // Get Guid of the selected buy item
                    var strGuid = curItem[0].Cells[0].Value.ToString();

                    // Check if a document is set
                    if (curItem[0].Cells[((DataGridView)sender).ColumnCount - 1].Value == null) return;

                    // Get doc from the sale with the Guid
                    foreach (var temp in ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
                    {
                        // Check if the sale Guid is the same as the Guid of the clicked buy item
                        if (temp.Guid != strGuid) continue;

                        // Remove move document from the buy objects
                        if (ShareObjectFinalValue.SetBuyDocument(strGuid, temp.Date, string.Empty) &&
                            ShareObjectMarketValue.SetBuyDocument(strGuid, temp.Date, string.Empty))
                        {
                            // Set flag to save the share object.
                            SaveFlag = true;

                            OnShowSales();

                            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormSale/StateMessages/EditSuccess", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                (int)FrmMain.EComponentLevels.Application);
                        }
                        else
                        {
                            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormSale/Errors/EditFailed", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error,
                                (int)FrmMain.EComponentLevels.Application);
                        }
                    }
                }
                
                // Reset load running flag
                LoadSaleRunning = false;
            }
            catch (Exception ex)
            {
                tabCtrlSales.SelectedIndex = 0;

                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", SettingsConfiguration.LanguageName),
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
        private static void OnDataGridViewSalesOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
        }

        #endregion Sales of a year

        #endregion Data grid view

        #region Parsing

        private void DocumentParsing(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Reset parsing variables
                _parsingResult = true;
                _parsingThreadFinished = false;
                _bankCounter = 0;
                _bankIdentifierFound = false;
                _saleIdentifierFound = false;
                _documentTypNotImplemented = false;
                _documentValuesRunning = false;

                ParsingText = string.Empty;

                _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingStarted);

                DocumentType = DocumentParsingConfiguration.DocumentTypes.SaleDocument;
                DocumentTypeParser = null;
                DictionaryParsingResult = null;

                Helper.RunProcess($"{Helper.PdfToTextApplication}",
                    $"-simple \"{txtBoxDocument.Text}\" {Helper.ParsingDocumentFileName}");

                ParsingText = File.ReadAllText(Helper.ParsingDocumentFileName, Encoding.Default);

                // Start document parsing
                DocumentTypeParsing();

                while (!_parsingThreadFinished)
                {
                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException)
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingDocumentFailed);
            }
            catch
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingDocumentFailed);
            }
        }

        /// <summary>
        /// This function starts the document parsing process
        /// which checks if the document typ is correct
        /// </summary>
        private void DocumentTypeParsing()
        {
            try
            {
                if (DocumentParsingConfiguration.InitFlag)
                {
                    if (DocumentTypeParser == null)
                        DocumentTypeParser = new Parser.Parser();
                    DocumentTypeParser.ParsingValues = new ParsingValues(
                        ParsingText,
                        DocumentParsingConfiguration.BankRegexList[_bankCounter].BankEncodingType,
                        DocumentParsingConfiguration.BankRegexList[_bankCounter].BankRegexList
                    );

                    // Check if the Parser is in idle mode
                    if (DocumentTypeParser != null && DocumentTypeParser.ParserInfoState.State == DataTypes.ParserState.Idle)
                    {
                        DocumentTypeParser.OnParserUpdate += DocumentTypeParser_UpdateGUI;

                        // Reset flags
                        _bankIdentifierFound = false;
                        _saleIdentifierFound = false;

                        // Start document parsing
                        DocumentTypeParser.StartParsing();
                    }
                    else
                    {
                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                        _parsingThreadFinished = true;
                        _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingFailed);
                    }
                }
                else
                {
                    _parsingThreadFinished = true;
                    _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingParsingDocumentError);
                }
            }
            catch (Exception)
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingFailed);
            }
        }

        /// <summary>
        /// This event handler updates the progress while checking the document type
        /// </summary>
        /// <param name="sender">BackGroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void DocumentTypeParser_UpdateGUI(object sender, DataTypes.OnParserUpdateEventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => DocumentTypeParser_UpdateGUI(sender, e)));
                }
                else
                {
                    try
                    {
                        switch (e.ParserInfoState.LastErrorCode)
                        {
                            case DataTypes.ParserErrorCodes.Finished:
                            {
                                //if (e.ParserInfoState.SearchResult != null)
                                //{
                                //    foreach (var result in e.ParserInfoState.SearchResult)
                                //    {
                                //        Console.Write(@"{0}:", result.Key);
                                //        if (result.Value != null && result.Value.Count > 0)
                                //            Console.WriteLine(@"{0}", result.Value[0]);
                                //        else
                                //            Console.WriteLine(@"-");
                                //    }
                                //}
                                break;
                            }
                            case DataTypes.ParserErrorCodes.SearchFinished:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.SearchRunning:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.SearchStarted:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ContentLoadFinished:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ContentLoadStarted:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.Started:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.Starting:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.NoError:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.StartFailed:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.BusyFailed:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.InvalidWebSiteGiven:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.NoRegexListGiven:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.NoWebContentLoaded:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ParsingFailed:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.CancelThread:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.FileExceptionOccurred:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.JsonExceptionOccurred:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.WebExceptionOccurred:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ExceptionOccurred:
                            {
                                break;
                            }
                        }

                        if (DocumentTypeParser.ParserErrorCode > 0)
                            Thread.Sleep(100);

                        // Check if a error occurred or the process has been finished
                        if (e.ParserInfoState.LastErrorCode < 0 ||
                            e.ParserInfoState.LastErrorCode == DataTypes.ParserErrorCodes.Finished)
                        {
                            if (e.ParserInfoState.LastErrorCode < 0)
                            {
                                // Set fail message
                                _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingFailed);
                            }
                            else
                            {
                                //Check if the correct bank has been found so search for the document values
                                if (e.ParserInfoState.SearchResult != null &&
                                    (
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName) ||
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .SaleIdentifierTagName))
                                )
                                {
                                    // Check if the bank identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName))
                                        _bankIdentifierFound = true;
                                    // Check if the buy identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .SaleIdentifierTagName))
                                        _saleIdentifierFound = true;

                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName) &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .SaleIdentifierTagName))
                                    {

                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingIdentifierValuesFound);

                                        _documentValuesRunning = true;

                                        // Get the correct parsing options for the given document type
                                        switch (DocumentType)
                                        {
                                            case DocumentParsingConfiguration.DocumentTypes.BuyDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeBuy]
                                                        .DocumentRegexList
                                                );
                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            case DocumentParsingConfiguration.DocumentTypes.SaleDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeSale]
                                                        .DocumentRegexList
                                                );
                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            case DocumentParsingConfiguration.DocumentTypes.DividendDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeDividend]
                                                        .DocumentRegexList
                                                );

                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            case DocumentParsingConfiguration.DocumentTypes.BrokerageDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeBrokerage]
                                                        .DocumentRegexList
                                                );

                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            default:
                                            {
                                                _documentTypNotImplemented = true;
                                            }
                                                break;
                                        }
                                    }
                                }

                                _bankCounter++;

                                // Check if another bank configuration should be checked
                                if (_bankCounter < DocumentParsingConfiguration.BankRegexList.Count &&
                                    _bankIdentifierFound == false)
                                {
                                    var parsingValues = DocumentTypeParser.ParsingValues;
                                    DocumentTypeParser.ParsingValues = new ParsingValues(
                                        parsingValues.ParsingText,
                                        parsingValues.EncodingType,
                                        DocumentParsingConfiguration.BankRegexList[_bankCounter].BankRegexList
                                    );
                                    DocumentTypeParser.StartParsing();
                                }
                                else
                                {
                                    if (_bankIdentifierFound == false)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingBankIdentifierFailed);
                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else if (_saleIdentifierFound == false)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingDocumentTypeIdentifierFailed);

                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else if (_documentTypNotImplemented)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingDocumentNotImplemented);

                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else
                                    {
                                        if (_documentValuesRunning)
                                        {
                                            if (DocumentTypeParser.ParsingResult != null)
                                            {
                                                DictionaryParsingResult =
                                                    new Dictionary<string, List<string>>(DocumentTypeParser
                                                        .ParsingResult);

                                                DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                                _parsingThreadFinished = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Show exception is not allowed here (THREAD)
                        // Only do progress report progress
                        _parsingThreadFinished = true;
                        _parsingStartAllow = false;

                        if (_parsingBackgroundWorker.WorkerReportsProgress)
                            _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingDocumentFailed);

                        if(DocumentTypeParser != null)
                            DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                    }
                }
            }
            catch (Exception)
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingDocumentFailed);

                if (DocumentTypeParser != null)
                    DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
            }
        }

        private void OnDocumentParsingProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case (int) ParsingErrorCode.ParsingStarted:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingStateMessages/ParsingStarted",
                            LanguageName);

                    toolStripProgressBarSaleDocumentParsing.Visible = true;
                    grpBoxAdd.Enabled = false;
                    grpBoxSales.Enabled = false;
                    break;
                }

                case (int) ParsingErrorCode.ParsingParsingDocumentError:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingParsingDocumentError",
                            LanguageName);

                    toolStripProgressBarSaleDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxSales.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingFailed:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingFailed",
                            LanguageName);

                    toolStripProgressBarSaleDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxSales.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingDocumentNotImplemented:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingDocumentNotImplemented",
                            LanguageName);

                    toolStripProgressBarSaleDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxSales.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingBankIdentifierFailed:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingBankIdentifierFailed",
                            LanguageName);

                    toolStripProgressBarSaleDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxSales.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingDocumentTypeIdentifierFailed:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormSale/ParsingErrors/ParsingDocumentTypeIdentifierFailed",
                            LanguageName);

                    toolStripProgressBarSaleDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxSales.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingDocumentFailed:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingDocumentFailed",
                            LanguageName);

                    toolStripProgressBarSaleDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxSales.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingIdentifierValuesFound:
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingIdentifierValuesFound",
                            LanguageName);
                    break;
                }
            }
        }

        private void OnDocumentParsingRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Delete old parsing text file
            if (File.Exists(Helper.ParsingDocumentFileName))
                File.Delete(Helper.ParsingDocumentFileName);

            if (DictionaryParsingResult != null)
            {
                // Check if the WKN has been found and if the WKN is the right one
                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                    .DocumentTypeSaleWkn) || DictionaryParsingResult[DocumentParsingConfiguration
                    .DocumentTypeSaleWkn][0] != ShareObjectFinalValue.Wkn)
                {
                    toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageSaleDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingWrongWkn",
                            LanguageName);
                }
                else
                {
                    #region Check which values are found

                    foreach (var resultEntry in DictionaryParsingResult)
                    {
                        if (resultEntry.Value.Count <= 0) continue;

                        switch (resultEntry.Key)
                        {
                            case DocumentParsingConfiguration.DocumentTypeSaleDate:
                            {
                                picBoxDateParseState.Image = Resources.search_ok_24;
                                dateTimePickerDate.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleTime:
                            {
                                picBoxTimeParseState.Image = Resources.search_ok_24;
                                dateTimePickerTime.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleDepotNumber:
                            {
                                picBoxDepotNumberParseState.Image = Resources.search_ok_24;
                                cbxDepotNumber.SelectedIndex = cbxDepotNumber.FindString(DocumentParsingConfiguration.BankRegexList[_bankCounter - 2].BankIdentifier);
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleOrderNumber:
                            {
                                picBoxOrderNumberParserState.Image = Resources.search_ok_24;
                                txtBoxOrderNumber.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleVolume:
                            {
                                picBoxVolumeParseState.Image = Resources.search_ok_24;
                                txtBoxVolume.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSalePrice:
                            {
                                picBoxPriceParseState.Image = Resources.search_ok_24;
                                txtBoxSalePrice.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleTaxAtSource:
                            {
                                picBoxTaxAtSourceParseState.Image = Resources.search_ok_24;
                                txtBoxTaxAtSource.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleCapitalGainTax:
                            {
                                picBoxCapitalGainTaxParseState.Image = Resources.search_ok_24;
                                txtBoxCapitalGainsTax.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleSolidarity:
                            {
                                picBoxSolidarityTaxParseState.Image = Resources.search_ok_24;
                                txtBoxSolidarityTax.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleProvision:
                            {
                                picBoxProvisionParseState.Image = Resources.search_ok_24;
                                txtBoxProvision.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleBrokerFee:
                            {
                                picBoxBrokerFeeParseState.Image = Resources.search_ok_24;
                                txtBoxBrokerFee.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleTraderPlaceFee:
                            {
                                picBoxTraderPlaceFeeParseState.Image = Resources.search_ok_24;
                                txtBoxTraderPlaceFee.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                            case DocumentParsingConfiguration.DocumentTypeSaleReduction:
                            {
                                picBoxReductionParseState.Image = Resources.search_ok_24;
                                txtBoxReduction.Text = resultEntry.Value[0].Trim();
                                break;
                            }
                        }
                    }

                    #endregion Check which values are found

                    #region Not found values

                    // Which values are not found
                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleDate) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleDate) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleDate].Count == 0
                    )
                    {
                        picBoxDateParseState.Image = Resources.search_failed_24;
                        dateTimePickerDate.Value = DateTime.Now;
                        dateTimePickerTime.Value =
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleTime) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleTime) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleTime].Count == 0
                    )
                    {
                        dateTimePickerTime.Value =
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        picBoxTimeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                            .DocumentTypeSaleDepotNumber) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleDepotNumber) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleDepotNumber].Count == 0
                    )
                    {
                        picBoxDepotNumberParseState.Image = Resources.search_failed_24;
                        cbxDepotNumber.SelectedIndex = 0;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                            .DocumentTypeSaleOrderNumber) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleOrderNumber) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleOrderNumber].Count == 0
                    )
                    {
                        picBoxOrderNumberParserState.Image = Resources.search_failed_24;
                        txtBoxOrderNumber.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleVolume) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleVolume) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleVolume].Count == 0
                    )
                    {
                        picBoxVolumeParseState.Image = Resources.search_failed_24;
                        txtBoxVolume.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSalePrice) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSalePrice) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSalePrice].Count == 0
                    )
                    {
                        picBoxVolumeParseState.Image = Resources.search_failed_24;
                        txtBoxSalePrice.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                            .DocumentTypeSaleTaxAtSource) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleTaxAtSource) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleTaxAtSource].Count == 0
                    )
                    {
                        picBoxTaxAtSourceParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(
                            DocumentParsingConfiguration.DocumentTypeSaleCapitalGainTax) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                            .DocumentTypeSaleCapitalGainTax) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleCapitalGainTax].Count == 0
                    )
                    {
                        picBoxCapitalGainTaxParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleSolidarity) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleSolidarity) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleSolidarity].Count == 0
                    )
                    {
                        picBoxSolidarityTaxParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleProvision) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleProvision) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleProvision].Count == 0
                    )
                    {
                        picBoxProvisionParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleBrokerFee) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleBrokerFee) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleBrokerFee].Count == 0
                    )
                    {
                        picBoxBrokerFeeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(
                            DocumentParsingConfiguration.DocumentTypeSaleTraderPlaceFee) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                            .DocumentTypeSaleTraderPlaceFee) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleTraderPlaceFee].Count == 0
                    )
                    {
                        picBoxTraderPlaceFeeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleReduction) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeSaleReduction) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleReduction].Count == 0
                    )
                    {
                        picBoxReductionParseState.Image = Resources.search_info_24;
                    }

                    #endregion Not found values

                    if (!_parsingResult)
                    {
                        toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageSaleDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/ParsingErrors/ParsingFailed",
                                LanguageName);
                    }
                    else
                    {
                        toolStripStatusLabelMessageSaleDocumentParsing.ForeColor = Color.Black;
                        toolStripStatusLabelMessageSaleDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(
                                @"/AddEditFormSale/ParsingStateMessages/ParsingDocumentSuccessful",
                                LanguageName);
                    }
                }
            }

            toolStripProgressBarSaleDocumentParsing.Visible = false;
            grpBoxAdd.Enabled = true;
            grpBoxSales.Enabled = true;
        }

        #endregion Parsing

        #endregion Methods
    }
}