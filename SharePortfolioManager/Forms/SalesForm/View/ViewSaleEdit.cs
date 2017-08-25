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
using System.Globalization;

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
        DeleteFailedUnerasable,
        InputValuesInvalid,
        DateExists,
        DateWrongFormat,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        BuyPriceEmpty,
        BuyPriceWrongFormat,
        BuyPriceWrongValue,
        SalePriceEmpty,
        SalePriceWrongFormat,
        SalePriceWrongValue,
        LossBalanceWrongFormat,
        LossBalanceWrongValue,
        TaxAtSourceWrongFormat,
        TaxAtSourceWrongValue,
        CapitalGainsTaxWrongFormat,
        CapitalGainsTaxWrongValue,
        SolidarityTaxWrongFormat,
        SolidarityTaxWrongValue,
        CostsWrongFormat,
        CostsWrongValue,
        DirectoryDoesNotExists,
        FileDoesNotExists
    };

    /// <summary>
    /// Interface of the SaleEdit view
    /// </summary>
    public interface IViewSaleEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValues;
        event EventHandler AddSale;
        event EventHandler EditSale;
        event EventHandler DeleteSale;

        SaleErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        bool UpdateSale { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        string BuyPrice { get; set; }
        string SalePrice { get; set; }
        string LossBalance { get; set; }
        string TaxAtSource { get; set; }
        string CapitalGainsTax { get; set; }
        string SolidarityTax { get; set; }
        string Costs { get; set; }
        string ProfitLoss { get; set; }
        string Payout { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
    }

    public partial class ViewSaleEdit : Form, IViewSaleEdit
    {
        #region Fields

        #region Transfer parameter

        /// <summary>
        /// Stores the chosen market value share object
        /// </summary>
        ShareObjectMarketValue _shareObjectMarketValue = null;

        /// <summary>
        /// Stores the chosen final value share object
        /// </summary>
        ShareObjectFinalValue _shareObjectFinalValue = null;

        /// <summary>
        /// Stores the logger
        /// </summary>
        Logger _logger;

        /// <summary>
        /// Stores the given language file
        /// </summary>
        Language _language;

        /// <summary>
        /// Stores the given language
        /// </summary>
        string _languageName;

        #endregion Transfer parameter

        #region Flags

        /// <summary>
        /// Stores if a sale has been deleted or added
        /// and so a save must be done in the lower dialog
        /// </summary>
        bool _bSave;

        /// <summary>
        /// Stores if a sale should be load from the portfolio
        /// </summary>
        private bool _bLoadGridSelectionFlag;

        /// <summary>
        /// Stores if a sale should be updated
        /// </summary>
        bool _bUpdateSale;

        #endregion Flags

        /// <summary>
        /// Stores the date of a selected sale row
        /// </summary>
        string _selectedDate;

        /// <summary>
        /// Stores the current error code of the form
        /// </summary>
        SaleErrorCode _errorCode;

        /// <summary>
        /// Stores the DataGridView of the selected row
        /// </summary>
        DataGridView _selectedDataGridView = null;

        #endregion Fields

        #region Properties

        public Logger Logger
        {
            get { return _logger; }
            internal set { _logger = value; }
        }

        public Language Language
        {
            get { return _language; }
            internal set { _language = value; }
        }

        public string LanguageName
        {
            get { return _languageName; }
            internal set { _languageName = value; }
        }

        public bool UpdateSaleFlag
        {
            get { return _bUpdateSale; }
            set { _bUpdateSale = value; }
        }

        public bool SaveFlag
        {
            get { return _bSave; }
            internal set { _bSave = value; }
        }

        public bool LoadGridSelectionFlag
        {
            get { return _bLoadGridSelectionFlag; }
            internal set { _bLoadGridSelectionFlag = value; }
        }

        public DataGridView SelectedDataGridView
        {
            get { return _selectedDataGridView; }
            internal set { _selectedDataGridView = value; }
        }

        #endregion Properties

        #region IViewMember

        new

        DialogResult ShowDialog
        {
            get { return base.ShowDialog(); }
        }

        public bool UpdateSale
        {
            get { return _bUpdateSale; }
            set
            {
                _bUpdateSale = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UpdateSale"));
            }
        }

        public SaleErrorCode ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get { return _shareObjectMarketValue; }
            set { _shareObjectMarketValue = value; }
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get { return _shareObjectFinalValue; }
            set { _shareObjectFinalValue = value; }
        }

        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDate"));
            }
        }

        #region Input values

        public string Date
        {
            get { return datePickerDate.Text; }
            set
            {
                if (datePickerDate.Text == value)
                    return;
                datePickerDate.Text = value;
            }
        }

        public string Time
        {
            get { return datePickerTime.Text; }
            set
            {
                if (datePickerTime.Text == value)
                    return;
                datePickerTime.Text = value;
            }
        }

        public string Volume
        {
            get { return txtBoxVolume.Text; }
            set
            {
                if (txtBoxVolume.Text == value)
                    return;
                txtBoxVolume.Text = value;
            }
        }

        public string BuyPrice
        {
            get { return txtBoxBuyPrice.Text; }
            set
            {
                if (txtBoxBuyPrice.Text == value)
                    return;
                txtBoxBuyPrice.Text = value;
            }
        }

        public string SalePrice
        {
            get { return txtBoxSalePrice.Text; }
            set
            {
                if (txtBoxSalePrice.Text == value)
                    return;
                txtBoxSalePrice.Text = value;
            }
        }

        public string LossBalance
        {
            get { return txtBoxLossBalance.Text; }
            set
            {
                if (txtBoxLossBalance.Text == value)
                    return;
                txtBoxLossBalance.Text = value;
            }
        }

        public string TaxAtSource
        {
            get { return txtBoxTaxAtSource.Text; }
            set
            {
                if (txtBoxTaxAtSource.Text == value)
                    return;
                txtBoxTaxAtSource.Text = value;
            }
        }

        public string CapitalGainsTax
        {
            get { return txtBoxPayout.Text; }
            set
            {
                if (txtBoxCapitalGainsTax.Text == value)
                    return;
                txtBoxCapitalGainsTax.Text = value;
            }
        }

        public string SolidarityTax
        {
            get { return txtBoxSolidarityTax.Text; }
            set
            {
                if (txtBoxSolidarityTax.Text == value)
                    return;
                txtBoxSolidarityTax.Text = value;
            }
        }

        public string Costs
        {
            get { return txtBoxCosts.Text; }
            set
            {
                if (txtBoxCosts.Text == value)
                    return;
                txtBoxCosts.Text = value;
            }
        }

        public string ProfitLoss
        {
            get { return txtBoxProfitLoss.Text; }
            set
            {
                if (txtBoxProfitLoss.Text == value)
                    return;
                txtBoxProfitLoss.Text = value;
            }
        }

        public string Payout
        {
            get { return txtBoxPayout.Text; }
            set
            {
                if (txtBoxPayout.Text == value)
                    return;
                txtBoxPayout.Text = value;
            }
        }

        public string Document
        {
            get { return txtBoxDocument.Text; }
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
                        _bSave = true;

                        // Refresh the sale list
                        ShowSales();

                        // Reset values
                        this.Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case SaleErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
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
                        // Set flag to save the share object.
                        _bSave = true;

                        // Refresh the sale list
                        ShowSales();

                        // Reset values
                        this.Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case SaleErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        _bSave = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.black_add;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the sale list
                        ShowSales();

                        // Reset values
                        this.Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case SaleErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.DateExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case SaleErrorCode.DateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.BuyPriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BuyPriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxBuyPrice.Focus();

                        break;
                    }
                case SaleErrorCode.BuyPriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BuyPriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxBuyPrice.Focus();

                        break;
                    }
                case SaleErrorCode.BuyPriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/BuyPriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxBuyPrice.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.LossBalanceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/LossBalanceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxLossBalance.Focus();

                        break;
                    }
                case SaleErrorCode.LossBalanceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/LossBalanceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxLossBalance.Focus();

                        break;
                    }
                case SaleErrorCode.TaxAtSourceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case SaleErrorCode.TaxAtSourceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case SaleErrorCode.CapitalGainsTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case SaleErrorCode.CapitalGainsTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case SaleErrorCode.SolidarityTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case SaleErrorCode.SolidarityTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case SaleErrorCode.CostsWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CostsWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case SaleErrorCode.CostsWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CostsWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case SaleErrorCode.DirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DirectoryDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxDocument.Focus();

                        break;
                    }
                case SaleErrorCode.FileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/FileDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
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

        // TODO in all other dialogs
        //public void DocumentBrowseFinish()
        //{
        //    // Set messages
        //    string strMessage = @"";
        //    Color clrMessage = Color.Black;
        //    FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

        //    switch (ErrorCode)
        //    {
        //        case SaleErrorCode.DocumentBrowseFailed:
        //            {
        //                txtBoxDocument.Text = @"-";

        //                strMessage =
        //                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ChoseDocumentFailed", LanguageName);
        //                break;
        //            }
        //    }

        //    Helper.AddStatusMessage(toolStripStatusLabelMessage,
        //       strMessage,
        //       Language,
        //       LanguageName,
        //       clrMessage,
        //       Logger,
        //       (int)stateLevel,
        //       (int)FrmMain.EComponentLevels.Application);
        //}

        #endregion IViewMember

        #region Event members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValues;
        public event EventHandler AddSale;
        public event EventHandler EditSale;
        public event EventHandler DeleteSale;

        // TODO in all other dialogs
        //public event EventHandler DocumentBrowse;

        #endregion Event members

        #region Methods

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final share object</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public ViewSaleEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue, Logger logger, Language xmlLanguage, String language)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;
            Logger = logger;
            Language = xmlLanguage;
            LanguageName = language;
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
                lblBuyPrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/BuyPrice",
                        LanguageName);
                lblSalePrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SalesPrice",
                        LanguageName);
                lblLossBalance.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/LossBalance",
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
                lblCosts.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Costs",
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
                lblBuyPriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSalePriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblLossBalanceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxAtSourceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCapitalGainsTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSolidarityTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCostsUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblProfitLossUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPayoutUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.black_save;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                #endregion Image configuration

                // Set current share value
                txtBoxVolume.Text = ShareObjectFinalValue.VolumeAsStr;

                // Set current share price
                txtBoxSalePrice.Text = ShareObjectFinalValue.CurPriceAsStr;

                ShowSales();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShareSalesEdit_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
            if (_bSave)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
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
            txtBoxBuyPrice.Text = @"";
            txtBoxSalePrice.Text = @"";
            txtBoxLossBalance.Text = @"";
            txtBoxTaxAtSource.Text = @"";
            txtBoxCapitalGainsTax.Text = @"";
            txtBoxSolidarityTax.Text = @"";
            txtBoxCosts.Text = @"";
            txtBoxProfitLoss.Text = @"";
            txtBoxPayout.Text = @"";
            txtBoxDocument.Text = @"";

            txtBoxVolume.Focus();
        }

        #endregion Form

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddVolume_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Volume"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxVolume_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSalePrice_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("SalePrice"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxSalePrice_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());

        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxLossBalance_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("LossBalance"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxLossBalance_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTaxAtSource_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("TaxAtSource"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxTaxAtSourceLeave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCapitalGainsTax_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("CapitalGainsTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxCapitalGAinsTaxLeave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSolidarityTax_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("SolidarityTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxSolidarityTaxLeave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCosts_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Costs"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxCostsTaxLeave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function only sets the document of the model to the view
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Document"));
        }

        #endregion TextBoxes

        #region Buttons

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
                this.Enabled = false;

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    UpdateSale = false;

                    if (AddSale != null)
                        AddSale(this, null);
                }
                else
                {
                    UpdateSale = true;

                    if (EditSale != null)
                        EditSale(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnAdd_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
        void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                // Disable controls
                this.Enabled = false;

                toolStripStatusLabelMessageSaleEdit.Text = @"";

                string strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                string strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/SaleDelete",
                    LanguageName);
                string strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                string strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                DialogResult dlgResult = messageBox.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    if (DeleteSale != null)
                        DeleteSale(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnDelete_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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

                // Disable button(s)
                btnReset.Enabled = false;
                btnDelete.Enabled = false;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                _selectedDataGridView = null;

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
#if DEBUG
                MessageBox.Show("btnReset_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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

        #endregion Buttons

        #region Data grid view

        /// <summary>
        /// This function paints the sale list of the share
        /// </summary>
        private void ShowSales()
        {
            try
            {
                // TODO refactoring

                // Reset tab control
                foreach (TabPage tabPage in tabCtrlSales.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        DataGridView view = control as DataGridView;
                        if (view != null)
                        {
                            view.SelectionChanged -= dataGridViewSalesOfAYear_SelectionChanged;
                            view.SelectionChanged -= dataGridViewSalesOfYears_SelectionChanged;
                            view.DataBindingComplete -= dataGridViewSalesOfAYear_DataBindingComplete;
                        }
                    }
                    tabPage.Controls.Clear();
                    tabCtrlSales.TabPages.Remove(tabPage);
                }

                tabCtrlSales.TabPages.Clear();

                // Create TabPage for the sales of the years
                TabPage newTabPageOverviewYears = new TabPage();
                // Set TabPage name
                newTabPageOverviewYears.Name =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview",
                        LanguageName);
                newTabPageOverviewYears.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", LanguageName)
                                         + @" ("
                                         + _shareObjectFinalValue.AllSaleEntries.SalePayoutTotalWithUnitAsString
                                         + @" / "
                                         + _shareObjectFinalValue.AllSaleEntries.SaleProfitLossTotalWithUnitAsString
                                         + @")";

                // Create Binding source for the sale data
                BindingSource bindingSourceOverview = new BindingSource();
                if (_shareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        _shareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues();

                // Create DataGridView
                DataGridView dataGridViewSalesOverviewOfAYears = new DataGridView();
                dataGridViewSalesOverviewOfAYears.Dock = DockStyle.Fill;
                // Bind source with buy values to the DataGridView
                dataGridViewSalesOverviewOfAYears.DataSource = bindingSourceOverview;
                // Set the delegate for the DataBindingComplete event
                dataGridViewSalesOverviewOfAYears.DataBindingComplete += dataGridViewSalesOfAYear_DataBindingComplete;

                // Set row select event
                dataGridViewSalesOverviewOfAYears.SelectionChanged += dataGridViewSalesOfYears_SelectionChanged;

                // Advanced configuration DataGridView buys
                DataGridViewCellStyle styleOverviewOfYears = dataGridViewSalesOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                dataGridViewSalesOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridViewSalesOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewSalesOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


                dataGridViewSalesOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewSalesOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dataGridViewSalesOverviewOfAYears.MultiSelect = false;

                dataGridViewSalesOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewSalesOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewSalesOverviewOfAYears.AllowUserToResizeRows = false;

                dataGridViewSalesOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                newTabPageOverviewYears.Controls.Add(dataGridViewSalesOverviewOfAYears);
                dataGridViewSalesOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlSales.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlSales;


                // Check if sales exists
                if (_shareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Count > 0)
                {
                    // Loop through the years of the sales
                    foreach (
                        var keyName in _shareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Keys.Reverse()
                        )
                    {
                        // Create TabPage
                        TabPage newTabPage = new TabPage();
                        // Set TabPage name
                        newTabPage.Name = keyName;
                        newTabPage.Text = keyName
                                            + @" ("
                                            + _shareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                              .SalePayoutYearWithUnitAsString
                                            + @" / "
                                            + _shareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                              .SaleProfitLossYearWithUnitAsString
                                            + @")";

                        // Create Binding source for the sale data
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource =
                            _shareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName].SaleListYear;

                        // Create DataGridView
                        DataGridView dataGridViewSalesOfAYear = new DataGridView();
                        dataGridViewSalesOfAYear.Dock = DockStyle.Fill;
                        // Bind source with buy values to the DataGridView
                        dataGridViewSalesOfAYear.DataSource = bindingSource;
                        // Set the delegate for the DataBindingComplete event
                        dataGridViewSalesOfAYear.DataBindingComplete += dataGridViewSalesOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewSalesOfAYear.SelectionChanged += dataGridViewSalesOfAYear_SelectionChanged;
                        // Set cell decimal click event
                        dataGridViewSalesOfAYear.CellContentDoubleClick += dataGridViewSalesOfAYear_CellContentdecimalClick;

                        // Advanced configuration DataGridView buys
                        DataGridViewCellStyle style = dataGridViewSalesOfAYear.ColumnHeadersDefaultCellStyle;
                        style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                        dataGridViewSalesOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        dataGridViewSalesOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                        dataGridViewSalesOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                        dataGridViewSalesOfAYear.RowHeadersVisible = false;
                        dataGridViewSalesOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                        dataGridViewSalesOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                        dataGridViewSalesOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                        dataGridViewSalesOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        dataGridViewSalesOfAYear.MultiSelect = false;

                        dataGridViewSalesOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewSalesOfAYear.AllowUserToResizeColumns = false;
                        dataGridViewSalesOfAYear.AllowUserToResizeRows = false;

                        dataGridViewSalesOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        newTabPage.Controls.Add(dataGridViewSalesOfAYear);
                        dataGridViewSalesOfAYear.Parent = newTabPage;
                        tabCtrlSales.Controls.Add(newTabPage);
                        newTabPage.Parent = tabCtrlSales;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShowSales()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
        private void dataGridViewSalesOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (int i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Date",
                                    LanguageName);
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Value",
                                    LanguageName) + @" (" + _shareObjectFinalValue.CurrencyUnit + ")";
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Price",
                                    LanguageName) + @" (" + _shareObjectFinalValue.CurrencyUnit + ")";
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_ProfitLoss",
                                    LanguageName) + @" (" + _shareObjectFinalValue.CurrencyUnit + ")";
                            break;
                        case 4:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Volume",
                                    LanguageName) + ShareObject.PieceUnit;
                            break;
                        case 5:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Document",
                                    LanguageName);
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                    ((DataGridView)sender).Rows[0].Selected = false;

                // Reset the text box values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSalesOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
        void DeselectRowsOfDataGridViews(DataGridView DataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage TabPage in tabCtrlSales.TabPages)
                {
                    foreach (Control control in TabPage.Controls)
                    {
                        DataGridView view = control as DataGridView;
                        if (view != null && view != DataGridView)
                        {
                            foreach (DataGridViewRow selectedRow in view.SelectedRows)
                            {
                                selectedRow.Selected = false;
                            }
                        }
                    }
                }

                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("DeselectRowsOfDataGridViews()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeselectFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the sale
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnSaleDocumentBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                txtBoxDocument.Text = Helper.SetDocument(Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/OpenFileDialog/Title", LanguageName), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnSaleDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ChoseDocumentFailed", LanguageName),
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
        private void tabCtrlSales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlSales.SelectedTab != null)
            {
                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlSales.SelectedTab.Controls)
                {
                    if (control is DataGridView)
                        ((DataGridView)control).Focus();
                }
            }
        }

        /// <summary>
        /// This function sets the focus on the data grid view of the current selected tab page of the tab control
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
        private void tabCtrlSales_MouseEnter(object sender, EventArgs e)
        {
            if (tabCtrlSales.SelectedTab != null)
            {
                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlSales.SelectedTab.Controls)
                {
                    if (control is DataGridView)
                        ((DataGridView)control).Focus();
                }
            }
        }

        /// <summary>
        /// This function sets the focus to the group box add / edit when the mouse leaves the data grid view
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void tabCtrlSales_MouseLeave(object sender, EventArgs e)
        {
            grpBoxAdd.Focus();
        }

        #endregion Tab control delegates

        #region Sales of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewSalesOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlSales.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlSales.SelectTab(tabPage);
                            tabPage.Focus();

                            // Deselect rows
                            DeselectRowsOfDataGridViews(null);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSalesOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
        private void OnDataGridViewSalesOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewSalesOfYears_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
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
        private void dataGridViewSalesOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    // Get SaleObject of the selected DataGridView row
                    SaleObject saleObject = ShareObjectFinalValue.AllSaleEntries.GetSaleObjectByDateTime(curItem[0].Cells[0].Value.ToString());
                    if (saleObject != null)
                    {
                        // TODO
                        //datePickerDate.Value = Convert.ToDateTime(saleObject.Date);
                        //datePickerTime.Value = Convert.ToDateTime(saleObject.Date);
                        //txtBoxCosts.Text = saleObject.ValueAsString;
                        //txtBoxSalePrice.Text = saleObject.PriceAsString;
                        //txtBoxVolume.Text = saleObject.VolumeAsString;
                        //txtBoxDocument.Text = saleObject.Document;
                    }
                    else
                    {
                        // TODO
                        //datePickerDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        //datePickerTime.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        //txtBoxCosts.Text = curItem[0].Cells[1].Value.ToString();
                        //txtBoxSalePrice.Text = curItem[0].Cells[2].Value.ToString();
                        //txtBoxVolume.Text = curItem[0].Cells[4].Value.ToString();
                        //txtBoxDocument.Text = curItem[0].Cells[5].Value.ToString();
                    }

                    if (_shareObjectFinalValue.AllBuyEntries.IsDateLastDate(curItem[0].Cells[0].Value.ToString()) &&
                        _shareObjectFinalValue.AllSaleEntries.IsDateLastDate(curItem[0].Cells[0].Value.ToString()))
                    {
                        // Enable button(s)
                        btnDelete.Enabled = true;
                        // Enable text box(es)
                        txtBoxSalePrice.Enabled = true;
                        txtBoxCosts.Enabled = true;
                        txtBoxVolume.Enabled = true;
                    }
                    else
                    {
                        // Disable button(s)
                        btnDelete.Enabled = false;
                        // Disable text box(es)
                        txtBoxSalePrice.Enabled = false;
                        txtBoxCosts.Enabled = false;
                        txtBoxVolume.Enabled = false;
                    }

                    // Enable button(s)
                    btnReset.Enabled = true;

                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    _selectedDataGridView = (DataGridView)sender;
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;
                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;
                    // Enable text box(es)
                    txtBoxSalePrice.Enabled = true;
                    txtBoxCosts.Enabled = true;
                    txtBoxVolume.Enabled = true;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Reset stored DataGridView instance
                    _selectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSalesOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
        private void OnDataGridViewSalesOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewSalesOfAYear_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
        }

        /// <summary>
        /// This function opens the sale document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void dataGridViewSalesOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                int iColumnCount = ((DataGridView)sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex == iColumnCount - 1)
                {
                    // Check if a row is selected
                    if (((DataGridView)sender).SelectedRows.Count == 1)
                    {
                        // Get the current selected row
                        DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;
                        // Get date and time of the selected sale item
                        string strDateTime = curItem[0].Cells[0].Value.ToString();

                        // Check if a document is set
                        if (curItem[0].Cells[iColumnCount - 1].Value.ToString() != @"-")
                        {
                            // Get doc from the sale with the strDateTime
                            foreach (var temp in ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare())
                            {
                                // Check if the sale date and time is the same as the date and time of the clicked sale item
                                if (temp.Date == strDateTime)
                                {
                                    // Check if the file still exists
                                    if (File.Exists(temp.Document))
                                        // Open the file
                                        Process.Start(temp.Document);
                                    else
                                    {
                                        string strCaption =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                                LanguageName);
                                        string strMessage =
                                            Language.GetLanguageTextByXPath(
                                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                                LanguageName);
                                        string strOk =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                                LanguageName);
                                        string strCancel =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                                LanguageName);

                                        OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                                            strCancel);
                                        if (messageBox.ShowDialog() == DialogResult.OK)
                                        {
                                            // Remove sale object and add it with no document
                                            if (ShareObjectFinalValue.RemoveSale(temp.Date) // Refactoring &&
                                                /*ShareObjectFinalValue.AddSale(strDateTime, temp.Volume, temp.Reduction, temp.Costs, temp.MarketValue)*/)
                                            {
                                                // Set flag to save the share object.
                                                _bSave = true;

                                                ResetInputValues();
                                                ShowSales();

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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSalesOfAYear_CellContentdecimalClick()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is chosen if
        /// a buy should be add or edit.
        /// </summary>
        /// <param name="bFlagAddEdit">Flag if a sale should be add (true) or edit (false)</param>
        /// <param name="strDateTime">Date and time of the sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decMaxVolume"></param>
        /// <param name="decValue">Value of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the input values are correct or not</returns>
        bool CheckSaleInput(bool bFlagAddEdit, out string strDateTime, out decimal decVolume, decimal decMaxVolume, out decimal decValue, out decimal decProfitLoss, out string strDoc)
        {
            bool errorFlag = false;
            decVolume = 0;
            decValue = 0;
            decProfitLoss = 0;
            strDateTime = datePickerDate.Text + " " + datePickerTime.Text;
            strDoc = txtBoxDocument.Text;

            try
            {
                toolStripStatusLabelMessageSaleEdit.ForeColor = Color.Red;
                toolStripStatusLabelMessageSaleEdit.Text = "";

                // Check if a sale with the given date and time already exists
                foreach (var sale in _shareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
                {
                    if (bFlagAddEdit)
                    {
                        // By an Add all dates must be checked
                        if (sale.Date == strDateTime)
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                               Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateExists", LanguageName),
                               Language, LanguageName,
                               Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                            errorFlag = true;
                        }
                    }
                    else
                    {
                        // By an Edit all sales without the edit entry date and time must be checked
                        if (sale.Date == strDateTime
                            && _selectedDataGridView != null
                            && _selectedDataGridView.SelectedRows.Count == 1
                            && sale.Date != _selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString())
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                               Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateWrongFormat", LanguageName),
                               Language, LanguageName,
                               Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                            errorFlag = true;
                        }
                    }
                }

                // Check if a volume for the sale is given
                if (txtBoxVolume.Text == @"" && errorFlag == false)
                {
                    txtBoxVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeEmpty", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (!decimal.TryParse(txtBoxVolume.Text, out decVolume) && errorFlag == false)
                {
                    txtBoxVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongFormat", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (decVolume <= 0 && errorFlag == false)
                {
                    txtBoxVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongValue", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (decVolume > decMaxVolume && errorFlag == false)
                {
                    txtBoxVolume.Text = String.Format("{0:N2}", decMaxVolume);
                    txtBoxVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }

                // Value
                if (txtBoxCosts.Text == @"" && errorFlag == false)
                {
                    txtBoxCosts.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ValueEmpty", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (!decimal.TryParse(txtBoxCosts.Text, out decValue) && errorFlag == false)
                {
                    txtBoxCosts.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ValueWrongFormat", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (decValue <= 0 && errorFlag == false)
                {
                    txtBoxCosts.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ValueWrongValue", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }

                //// Profit or loss
                //if (txtBoxAddSalesProfitLossPercentage.Text == @"" && errorFlag == false)
                //{
                //    txtBoxAddSalesProfitLossPercentage.Focus();

                //    // Add status message
                //    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                //       XmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ProfitLossEmpty", LanguageName),
                //       XmlLanguage, LanguageName,
                //       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                //    errorFlag = true;
                //}
                //else if (!decimal.TryParse(txtBoxAddSalesProfitLossPercentage.Text, out decProfitLoss) && errorFlag == false)
                //{
                //    txtBoxAddSalesProfitLossPercentage.Focus();

                //    // Add status message
                //    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                //       XmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ProfitLossWrongFormat", LanguageName),
                //       XmlLanguage, LanguageName,
                //       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                //    errorFlag = true;
                //}

                // Check if a given document directory exists
                if (strDoc != @"" && strDoc != @"-" && !Directory.Exists(Path.GetDirectoryName(strDoc)) && errorFlag == false)
                {
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DirectoryDoesNotExist", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }

                // Check if a given document exists
                if (strDoc != @"" && strDoc != @"-" && !File.Exists(strDoc) && errorFlag == false)
                {
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                       Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/FileDoesNotExist", LanguageName),
                       Language, LanguageName,
                       Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }

                // Reset string values
                if (errorFlag)
                {
                    strDateTime = @"";
                    strDoc = @"";
                }

                return errorFlag;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CheckSaleInput()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CheckInputFailure", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                // Reset string values
                strDateTime = @"";
                strDoc = @"";

                return true;
            }
        }

        /// <summary>
        /// This function calculates the value of the given values
        /// If a given value is not valid the value text box is set to "-"
        /// <returns>String with the value or "-" if the calculation failed</returns>>
        /// </summary>
        //private string CalculateSalePayoutValue()
        //{
//            try
//            {
//                if (Math.Abs(_decSalePrice) > 0
//                    && Math.Abs(_decVolume) > 0)
//                {
//                    return Helper.FormatDecimal((_decSalePrice * _decVolume), Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObjectFinalValue.CultureInfo);
//                }
//                else
//                {
//                    return @"-";
//                }
//            }
//            catch (Exception ex)
//            {
//#if DEBUG
//                MessageBox.Show("CalculateValue()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
//                    MessageBoxIcon.Error);
//#endif
//                return @"-";
//            }
//        }

        /// <summary>
        /// This function calculates the profit / loss of the given values
        /// If a given value is not valid the profit / loss text box is set to "-"
        /// <param name="strSaleVolume">Sale volume of the share</param>
        /// <param name="strSalePrice">Current price of the share</param>
        /// <param name="decShareDeposit">Current deposit of the share</param>
        /// <param name="decShareVolume">Current share volume</param>
        /// <returns>String with the profit / loss or "-" if the calculation failed</returns>>
        /// </summary>
        private string CalculateProfitLoss(string strSaleVolume, string strSalePrice, decimal decShareDeposit, decimal decShareVolume)
        {
            try
            {
                decimal volume = 0;

                if (decimal.TryParse(strSaleVolume, out volume) && volume > 0)
                {
                    decimal price = 0;

                    if (decimal.TryParse(strSalePrice, out price))
                    {
                        decimal decSalePrice = price * volume;

                        // Check if the share deposit and share volume is not "0"
                        decimal decDeposit = 0;
                        if (decShareDeposit > 0 && decShareVolume > 0)
                        {
                            decDeposit = volume * decShareDeposit / decShareVolume;
                        }
                        
                        return Helper.FormatDecimal((decSalePrice - decDeposit), Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
                    }
                    else
                    {
                        return @"-";
                    }
                }
                else
                {
                    return @"-";
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculatePrice()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                return @"-";
            }
        }

        /// <summary>
        /// This function calculates the dividend payout foreign currency
        /// <returns>String with the payout or "-" if the calculation failed</returns>>
        /// </summary>
        //        private string CalculateSalePayoutForeignCurrency()
        //        {
        //            try
        //            {
        //                //if (Math.Abs(_decSalePriceFC) > 0
        //                //    && Math.Abs(_decVolume) > 0)
        //                //{
        //                //    _decSalePayoutFC = _decSalePriceFC * _decVolume;
        //                //    return Helper.FormatDecimal(_decSalePayoutFC,
        //                //            Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObjectFinalValue.CultureInfo);
        //                //}
        //                //else
        //                //{
        //                //    return @"-";
        //                //}
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                MessageBox.Show("CalculateSalePayoutForeignCurrency()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //#endif
        //                return @"-";
        //            }
        //        }

        /// <summary>
        /// This function calculates from the foreign currency price the normal price
        /// <returns>String with the normal payout of one share of the share or "-" if the calculation failed</returns>>
        /// </summary>
        //        private string CaluclateSalePriceFromForeignCurrencyPrice()
        //        {
        //            try
        //            {
        //                //if (Math.Abs(_decExchangeRatio) > 0
        //                //    && Math.Abs(_decSalePriceFC) > 0)
        //                //{
        //                //    _decSalePrice = _decSalePriceFC / _decExchangeRatio;
        //                //    return
        //                //        Helper.FormatDecimal(_decSalePrice,
        //                //            Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObjectFinalValue.CultureInfo);
        //                //}
        //                //else
        //                //{
        //                //    return @"-";
        //                //}
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                MessageBox.Show("CaluclateSalePriceFromForeignCurrencyPrice()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error);
        //#endif
        //                return @"-";
        //            }
        //        }

        /// <summary>
        /// This function changes the currency unit when the currency
        /// has been changed
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">EventArgs</param>
        private void cbxBoxAddSalesForeignCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO
//            lblAddSalesPriceShareFCUnit.Text = ((ComboBox)sender).SelectedItem.ToString().Split('-')[1].Trim();

            // Set currency units
//            TaxValuesCurrent.CurrencyUnit = lblAddSalesPriceShareFCUnit.Text;
            //if (((ComboBox)sender).SelectedItem != null)
            //{
            //    TaxValuesCurrent.FCUnit = ((ComboBox)sender).SelectedItem.ToString().Split('-')[1].Trim();
            //    TaxValuesCurrent.CiShareFC = Helper.GetCultureByISOCurrencySymbol(((ComboBox)sender).SelectedItem.ToString().Split('-')[0].Trim());
            //}
        }

        #endregion Methods
    }
}

