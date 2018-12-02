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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Properties;
using System.Linq;
using System.IO;
using System.ComponentModel;
using SharePortfolioManager.Classes.Sales;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.SalesForm.View
{
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
        BrokerageWrongFormat,
        BrokerageWrongValue,
        DirectoryDoesNotExists,
        FileDoesNotExists,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExits,
        DocumentFileDoesNotExists
    };

    /// <inheritdoc />
    /// <summary>
    /// Interface of the SaleEdit view
    /// </summary>
    public interface IViewSaleEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler AddSaleEventHandler;
        event EventHandler EditSaleEventHandler;
        event EventHandler DeleteSaleEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        SaleErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }
        ShareObjectFinalValue ShareObjectCalculation { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool ShowSales { get; set; }
        bool AddSale { get; set; }
        bool UpdateSale { get; set; }

        string SelectedGuid { get; set; }
        string SelectedGuidLast { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        string SalePrice { get; set; }
        string SaleBuyValue { get; set; }
        List<SaleBuyDetails> UsedBuyDetails { set; }
        string TaxAtSource { get; set; }
        string CapitalGainsTax { get; set; }
        string SolidarityTax { get; set; }
        string Brokerage { get; set; }
        string Reduction { get; set; }
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
        private string _selectedGuidLast = @"";

        /// <summary>
        /// Stores the date of a selected sale row
        /// </summary>
        private string _selectedDate;

        /// <summary>
        /// Stores the last focused date time picker or text box
        /// </summary>
        private Control _focusedControl;

        #endregion Fields

        #region Properties

        #region Transfer parameter

        public Logger Logger { get; internal set; }

        public Language Language { get; internal set; }

        public string LanguageName { get; internal set; }

        #endregion Transfer parameter

        #region Flags

        public bool ShowSalesFlag { get; set; }

        public bool AddSaleFlag { get; set; }

        public bool UpdateSaleFlag { get; set; }

        public bool SaveFlag { get; internal set; }

        #endregion Flags

        public DataGridView SelectedDataGridView { get; internal set; }

        #endregion Properties

        #region IViewMember

        public bool ShowSales
        {
            get => ShowSalesFlag;
            set
            {
                ShowSalesFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowSales"));
            }
        }

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

        public SaleErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public ShareObjectFinalValue ShareObjectCalculation { get; set; }

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
            get => datePickerDate.Text;
            set
            {
                if (datePickerDate.Text == value)
                    return;
                datePickerDate.Text = value;
            }
        }

        public string Time
        {
            get => datePickerTime.Text;
            set
            {
                if (datePickerTime.Text == value)
                    return;
                datePickerTime.Text = value;
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
            string strMessage = @"";
            Color clrMessage = Color.Black;
            FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case SaleErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Reset add flag
                        AddSale = false;

                        // Refresh the sale list
                        OnSalesShow();

                        break;
                    }
                case SaleErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", LanguageName);
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
                        btnAddSave.Image = Resources.black_add;
                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/EditSuccess", LanguageName);

                        // Reset update flag
                        UpdateSale = false;

                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the sale list
                        OnSalesShow();

                        break;
                    }
                case SaleErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/DeleteSuccess", LanguageName);

                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.black_add;

                        // Reset add flag
                        AddSale = false;

                        // Reset update flag
                        UpdateSale = false;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the sale list
                        OnSalesShow();

                        break;
                    }
                case SaleErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeMaxValue:
                    {
                        if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName))
                        {
                            strMessage =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_1", LanguageName) +
                                ShareObjectFinalValue.Volume +
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_2", LanguageName);
                        }
                        else
                        {
                            strMessage =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_1", LanguageName) +
                                (ShareObjectFinalValue.Volume + ShareObjectFinalValue.AllSaleEntries.GetSaleObjectByDateTime(datePickerDate.Text + " " + datePickerTime.Text).Volume) +
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_2", LanguageName);
                        }
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.TaxAtSourceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case SaleErrorCode.TaxAtSourceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case SaleErrorCode.CapitalGainsTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case SaleErrorCode.CapitalGainsTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case SaleErrorCode.SolidarityTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case SaleErrorCode.SolidarityTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case SaleErrorCode.BrokerageWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BrokerageWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerage.Focus();

                        break;
                    }
                case SaleErrorCode.BrokerageWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BrokerageWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerage.Focus();

                        break;
                    }
                case SaleErrorCode.DirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DirectoryDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDocument.Focus();

                        break;
                    }
                case SaleErrorCode.FileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/FileDoesNotExist", LanguageName);
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
               (int)stateLevel,
               (int)FrmMain.EComponentLevels.Application);
        }

        public void DocumentBrowseFinish()
        {
            // Set messages
            var strMessage = @"";
            var clrMessage = Color.Black;
            const FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case SaleErrorCode.DocumentBrowseFailed:
                    {
                        txtBoxDocument.Text = @"-";

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ChoseDocumentFailed", LanguageName);
                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
               strMessage,
               Language,
               LanguageName,
               clrMessage,
               Logger,
               (int)stateLevel,
               (int)FrmMain.EComponentLevels.Application);
        }

        #endregion IViewMember

        #region Event members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValuesEventHandler;
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
        public ViewSaleEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue,
            Logger logger, Language xmlLanguage, string language)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;
            ShareObjectCalculation = (ShareObjectFinalValue)ShareObjectFinalValue.Clone();

            Logger = logger;
            Language = xmlLanguage;
            LanguageName = language;

            _focusedControl = txtBoxVolume;

            SaveFlag = false;
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

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/Caption", LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxSales.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/Caption",
                        LanguageName);
                lblAddSaleDate.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Date",
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
                lblBrokerage.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Brokerage",
                        LanguageName);
                lblReduction.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Reduction",
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
                lblBrokerageUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblReductionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblProfitLossUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPayoutUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.black_add;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                #endregion Image configuration

                OnSalesShow();
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"ShareSalesEdit_Load()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date
        /// </summary>
        private void ResetInputValues()
        {
            // Reset date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            // Reset text boxes
            txtBoxVolume.Text = @"";
            txtBoxSalePrice.Text = @"";
            txtBoxSaleBuyValue.Text = @"";
            txtBoxProfitLoss.Text = @"";

            txtBoxTaxAtSource.Text = @"";
            txtBoxCapitalGainsTax.Text = @"";
            txtBoxSolidarityTax.Text = @"";

            txtBoxBrokerage.Text = @"";
            txtBoxReduction.Text = @""; 
            
            txtBoxPayout.Text = @"";
            txtBoxDocument.Text = @"";

            // Check if any volume is present
            if (ShareObjectFinalValue.Volume > 0)
            {
                // Set current share value
                txtBoxVolume.Text = ShareObjectFinalValue.VolumeAsStr;

                // Set current share price
                txtBoxSalePrice.Text = ShareObjectFinalValue.CurPriceAsStr;

                datePickerDate.Enabled = true;
                datePickerTime.Enabled = true;

                txtBoxVolume.Enabled = true;
                txtBoxSalePrice.Enabled = true;

                btnSalesBuyDetails.Enabled = true;

                txtBoxTaxAtSource.Enabled = true;
                txtBoxCapitalGainsTax.Enabled = true;
                txtBoxSolidarityTax.Enabled = true;

                txtBoxBrokerage.Enabled = true;
                txtBoxReduction.Enabled = true;

                txtBoxDocument.Enabled = true;
                btnSalesDocumentBrowse.Enabled = true;

                btnAddSave.Enabled = true;
            }
            else
            {
                datePickerDate.Enabled = false;
                datePickerTime.Enabled = false;

                txtBoxVolume.Enabled = false;
                txtBoxSalePrice.Enabled = false;

                btnSalesBuyDetails.Enabled = false;

                txtBoxTaxAtSource.Enabled = false;
                txtBoxCapitalGainsTax.Enabled = false;
                txtBoxSolidarityTax.Enabled = false;

                txtBoxBrokerage.Enabled = false;
                txtBoxReduction.Enabled = false;

                txtBoxDocument.Enabled = false;
                btnSalesDocumentBrowse.Enabled = false;

                btnAddSave.Enabled = false;
            }

            // TODO Renaming of the buttons and so on

            // Select overview tab
            if (tabCtrlSales.TabPages.Count > 0)
                tabCtrlSales.SelectTab(0);

            txtBoxVolume.Focus();

            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());

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
            _focusedControl = datePickerDate;
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
            _focusedControl = datePickerTime;

        }

        #endregion Date Time

        #region TextBoxes

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
        private void OnTxtBoxBrokerage_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Brokerage"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerageLeave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerage_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxBrokerage;
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
        private void txtBoxDocument_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxDocument;
        }

        /// <summary>
        /// This function shows the Drop sign
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// This function allows to sets via Drag and Drop a document for this brokerage
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length <= 0 || files.Length > 1) return;

            txtBoxDocument.Text = files[0];

            // TODO Parse PDF and set values to the form
            //var pdf = new PdfDocument(new PdfReader(txtBoxDocument.Text));
            //var text = PdfTextExtractor.GetTextFromPage(pdf.GetPage(1), new LocationTextExtractionStrategy());
            //pdf.Close();
            //Console.WriteLine(@"Extracted text:");
            //Console.WriteLine(text);
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
            var strMessage = @"";

            // Loop through the buy details which are used for this sale
            foreach (var saleBuyDetails in UsedBuyDetails)
            {
                strMessage += saleBuyDetails.SaleBuyDetailsData;
                strMessage += "\r\n";
            }

            // Result of the used buy details
            strMessage += " ===============================================================================\r\n";
            const string format = "               {0,20:0.00000} * {1,20:0.00000} = {2,18:0.00}";
            var strMessageResult = string.Format(format,
                UsedBuyDetails.Sum(x => x.DecVolume ),
                UsedBuyDetails.Sum(x => x.DecBuyPrice * x.DecVolume) / UsedBuyDetails.Sum(x => x.DecVolume),
                UsedBuyDetails.Sum(x => x.DecBuyPrice * x.DecVolume)
            );

            strMessage += strMessageResult;

            // Create form with the used buy details and the result
            var showOwnMessageBox =
                new UsedBuyDetailsList.UsedBuyDetailsList(
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/UsedBuyDetailsForm/Caption", LanguageName),
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/UsedBuyDetailsForm/GroupBox", LanguageName),
                    strMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/UsedBuyDetailsForm/Button", LanguageName)
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

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    AddSale = true;
                    UpdateSale = false;

                    AddSaleEventHandler?.Invoke(this, null);
                }
                else
                {
                    AddSale = true;
                    UpdateSale = true;

                    EditSaleEventHandler?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"btnAdd_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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

                toolStripStatusLabelMessageSaleEdit.Text = @"";

                var strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/SaleDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    DeleteSaleEventHandler?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"btnDelete_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                toolStripStatusLabelMessageSaleEdit.Text = @"";

                // Enable button(s)
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.black_add;

                // Reset add flag
                AddSale = false;

                // Reset update flag
                UpdateSale = false;

                // Disable button(s)
                btnDelete.Enabled = false;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                SelectedDataGridView = null;

                // Select overview tab
                if (
                    tabCtrlSales.TabPages.ContainsKey(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", LanguageName)))
                    tabCtrlSales.SelectTab(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", LanguageName));
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"btnReset_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CancelFailure", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function closes the dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
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
            _focusedControl?.Focus();
        }

        #endregion Group box overview

        #region Data grid view

        /// <summary>
        /// This function paints the sale list of the share
        /// </summary>
        private void OnSalesShow()
        {
            try
            {
                // Set show flag
                ShowSales = true;

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
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", LanguageName)
                           + @" ("
                           + ShareObjectFinalValue.AllSaleEntries.SalePayoutTotalWithUnitAsStr
                           + @" / "
                           + ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotalWithUnitAsStr
                           + @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Create Binding source for the sale data
                var bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues();

                // Create DataGridView
                var dataGridViewSalesOverviewOfAYears = new DataGridView
                {
                    // TODO correct
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with buy values to the DataGridView
                    DataSource = bindingSourceOverview
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

                // Advanced configuration DataGridView sales
                dataGridViewSalesOverviewOfAYears.EnableHeadersVisualStyles = false;
                // Column header styling
                dataGridViewSalesOverviewOfAYears.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewSalesOverviewOfAYears.ColumnHeadersDefaultCellStyle.BackColor =
                    SystemColors.ControlLight;
                dataGridViewSalesOverviewOfAYears.ColumnHeadersHeight = 25;
                dataGridViewSalesOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                // Column styling
                dataGridViewSalesOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // Row styling
                dataGridViewSalesOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewSalesOverviewOfAYears.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewSalesOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewSalesOverviewOfAYears.MultiSelect = false;
                dataGridViewSalesOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // Cell styling
                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                dataGridViewSalesOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // Allow styling
                dataGridViewSalesOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewSalesOverviewOfAYears.AllowUserToResizeRows = false;
                dataGridViewSalesOverviewOfAYears.AllowUserToAddRows = false;
                dataGridViewSalesOverviewOfAYears.AllowUserToDeleteRows = false;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewSalesOverviewOfAYears);
                dataGridViewSalesOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlSales.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlSales;

                #endregion Control add

                // Check if sales exists
                if (ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Count <= 0) return;

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
                                   .SalePayoutYearWithUnitAsStr
                               + @" / "
                               + ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                   .SaleProfitLossYearWithUnitAsStr
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Create Binding source for the sale data
                    var bindingSource = new BindingSource
                    {
                        DataSource = ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                            .SaleListYear
                    };

                    // Create DataGridView
                    var dataGridViewSalesOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with buy values to the DataGridView
                        DataSource = bindingSource
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewSalesOfAYear.DataBindingComplete += OnDataGridViewSalesOfAYear_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewSalesOfAYear.MouseEnter += OnDataGridViewSalesOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewSalesOfAYear.SelectionChanged += OnDataGridViewSalesOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewSalesOfAYear.CellContentDoubleClick += OnDataGridViewSalesOfAYear_CellContentDecimalClick;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView sales
                    dataGridViewSalesOfAYear.EnableHeadersVisualStyles = false;
                    // Column header styling
                    dataGridViewSalesOfAYear.ColumnHeadersDefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewSalesOfAYear.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlLight;
                    dataGridViewSalesOfAYear.ColumnHeadersHeight = 25;
                    dataGridViewSalesOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    // Column styling
                    dataGridViewSalesOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Row styling
                    dataGridViewSalesOfAYear.RowHeadersVisible = false;
                    dataGridViewSalesOfAYear.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewSalesOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridViewSalesOfAYear.MultiSelect = false;
                    dataGridViewSalesOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    // Cell styling
                    dataGridViewSalesOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dataGridViewSalesOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                    dataGridViewSalesOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    dataGridViewSalesOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    // Allow styling
                    dataGridViewSalesOfAYear.AllowUserToResizeColumns = false;
                    dataGridViewSalesOfAYear.AllowUserToResizeRows = false;
                    dataGridViewSalesOfAYear.AllowUserToAddRows = false;
                    dataGridViewSalesOfAYear.AllowUserToDeleteRows = false;

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewSalesOfAYear);
                    dataGridViewSalesOfAYear.Parent = newTabPage;
                    tabCtrlSales.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlSales;

                    #endregion Control add
                }

                tabCtrlSales.TabPages[0].Select();

                // Reset show flag
                ShowSales = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"ShowSales()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the data binding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewSalesOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (var i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                        {
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Year",
                                        LanguageName);
                            }
                        }
                        break;
                        case 1:
                        {
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Volume",
                                        LanguageName) + @" (" + ShareObject.PieceUnit + @")";
                            }
                            else
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Date",
                                        LanguageName);
                            }
                        }
                        break;
                        case 2:
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Payout",
                                        LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Purchase",
                                        LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            }

                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_ProfitLoss",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 4:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Sale",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 5:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Document",
                                    LanguageName);
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                {
                    ((DataGridView)sender).Rows[0].Selected = false;
                    ((DataGridView)sender).ScrollBars = ScrollBars.Both;
                }

                if (((DataGridView)sender).Name != @"Overview")
                    ((DataGridView)sender).Columns[0].Visible = false;

                // Reset the text box values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfAYear_DataBindingComplete()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/RenameColHeaderFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }

        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        private void DeselectRowsOfDataGridViews(DataGridView dataGridView)
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
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"DeselectRowsOfDataGridViews()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeselectFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                // Reset last selected Guid
                SelectedGuidLast = @"";

                if (tabCtrlSales.SelectedTab == null) return;

                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlSales.SelectedTab.Controls)
                {
                    if (control is DataGridView view)
                    {
                        if (view.Rows.Count > 0 && view.Name != @"Overview")
                        {
                            if (view.Rows[0].Cells.Count > 0)
                            {
                                view.Rows[0].Selected = true;

                                // Set update flag
                                UpdateSale = true;
                            }

                            view.Focus();

                            if (view.Name == @"Overview")
                                ResetInputValues();
                        }
                        else
                        {
                            UpdateSale = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"TabCtrlBuys_SelectedIndexChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", LanguageName),
                Language, LanguageName,
                Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                (int) FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the group box add / edit when the mouse leaves the data grid view
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlSales_MouseLeave(object sender, EventArgs e)
        {
            _focusedControl?.Focus();
        }

        /// <summary>
        /// This function sets the focus on the last focused control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlSales_KeyDown(object sender, KeyEventArgs e)
        {
            _focusedControl?.Focus();
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
            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

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
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfYears_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewSalesOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
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
            try
            {
                if (tabCtrlSales.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlSales.SelectedTab.Controls.Contains((DataGridView)sender))
                        DeselectRowsOfDataGridViews((DataGridView)sender);
                }

                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    var curItem = ((DataGridView)sender).SelectedRows;

                    // Set selected date
                    if (curItem[0].Cells[1].Value != null)
                        SelectedDate = curItem[0].Cells[1].Value.ToString();
                    else
                        return;

                    // Get list of sales of a year
                    DateTime.TryParse(SelectedDate, out var dateTime);
                    var saleListYear = ShareObjectFinalValue.AllSaleEntries
                        .AllSalesOfTheShareDictionary[dateTime.Year.ToString()]
                        .SaleListYear;

                    var index = ((DataGridView)sender).SelectedRows[0].Index;

                    // Set selected Guid
                    SelectedGuid = saleListYear[index].Guid;

                    // Get BrokerageObject of the selected DataGridView row
                    var selectedSaleObject = saleListYear[index];

                    if (selectedSaleObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(selectedSaleObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(selectedSaleObject.Date);
                        txtBoxVolume.Text = selectedSaleObject.VolumeAsStr;
                        txtBoxSalePrice.Text = selectedSaleObject.SalePriceAsStr;
                        txtBoxTaxAtSource.Text = selectedSaleObject.TaxAtSourceAsStr;
                        txtBoxCapitalGainsTax.Text = selectedSaleObject.CapitalGainsTaxAsStr;
                        txtBoxSolidarityTax.Text = selectedSaleObject.SolidarityTaxAsStr;
                        txtBoxBrokerage.Text = selectedSaleObject.BrokerageAsStr;
                        txtBoxReduction.Text = selectedSaleObject.ReductionAsStr;
                        txtBoxProfitLoss.Text = selectedSaleObject.ProfitLossAsStr;
                        txtBoxPayout.Text = selectedSaleObject.PayoutAsStr;
                        txtBoxDocument.Text = selectedSaleObject.Document;
                    }
                    else
                    {
                        // TODO
                        datePickerDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        datePickerTime.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        txtBoxVolume.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxProfitLoss.Text = curItem[0].Cells[3].Value.ToString();
                        txtBoxPayout.Text = curItem[0].Cells[4].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[5].Value.ToString();
                    }

                    if (ShareObjectFinalValue.AllSaleEntries.IsDateLastDate(SelectedDate))
                    {
                        btnDelete.Enabled = ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare().Count > 1;

                        // Enable text box(es)
                        datePickerDate.Enabled = true;
                        datePickerTime.Enabled = true;
                        txtBoxVolume.Enabled = true;
                        txtBoxSalePrice.Enabled = true;
                        txtBoxTaxAtSource.Enabled = true;
                        txtBoxCapitalGainsTax.Enabled = true;
                        txtBoxSolidarityTax.Enabled = true;
                        txtBoxBrokerage.Enabled = true;
                        txtBoxReduction.Enabled = true;
                    }
                    else
                    {
                        // Disable Button(s)
                        btnDelete.Enabled = false;
                        // Disable TextBox(es)
                        datePickerDate.Enabled = false;
                        datePickerTime.Enabled = false;
                        txtBoxVolume.Enabled = false;
                        txtBoxSalePrice.Enabled = false;
                        txtBoxTaxAtSource.Enabled = false;
                        txtBoxCapitalGainsTax.Enabled = false;
                        txtBoxSolidarityTax.Enabled = false;
                        txtBoxBrokerage.Enabled = false;
                        txtBoxReduction.Enabled = false;
                    }

                    // Rename button
                    btnAddSave.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAdd.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;

                    // Format the input value
                    FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;

                    // Rename group box
                    grpBoxAdd.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Enable Button(s)
                    btnAddSave.Enabled = true;

                    // Enable text box(es)
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxSalePrice.Enabled = true;
                    txtBoxTaxAtSource.Enabled = true;
                    txtBoxCapitalGainsTax.Enabled = true;
                    txtBoxSolidarityTax.Enabled = true;
                    txtBoxBrokerage.Enabled = true;

                    // Reset update flag
                    UpdateSale = false;

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfAYear_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewSalesOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function opens the sale document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewSalesOfAYear_CellContentDecimalClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                var iColumnCount = ((DataGridView)sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex != iColumnCount - 1) return;

                // Check if a row is selected
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the current selected row
                var curItem = ((DataGridView)sender).SelectedRows;
                // Get Guid of the selected sale item
                var strGuid = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value == null) return;

                // Get doc from the sale with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
                {
                    // Check if the sale Guid is the same as the Guid of the clicked sale item
                    if (temp.Guid != strGuid) continue;

                    // Check if the file still exists
                    if (File.Exists(temp.Document))
                        // Open the file
                        Process.Start(temp.Document);
                    else
                    {
                        var strCaption =
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                LanguageName);
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
                            strCancel);
                        if (messageBox.ShowDialog() == DialogResult.OK)
                        {
                            // Remove sale object and add it with no document
                            if (ShareObjectFinalValue.RemoveSale(temp.Guid, temp.Date) &&
                                ShareObjectFinalValue.AddSale(strGuid, temp.Date, temp.Volume,
                                    temp.SalePrice, temp.SaleBuyDetails, temp.TaxAtSource, temp.CapitalGainsTax,
                                    temp.SolidarityTax, temp.Brokerage, temp.Reduction))
                            {
                                // Set flag to save the share object.
                                SaveFlag = true;

                                OnSalesShow();

                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/StateMessages/EditSuccess", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                            else
                            {
                                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/Errors/EditFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)FrmMain.EStateLevels.Error,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                        }
                    }

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfAYear_CellContentdecimalClick()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Sales of a year

        #endregion Data grid view

        #endregion Methods
    }
}

