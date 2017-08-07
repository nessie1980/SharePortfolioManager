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

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.DividendForm.View
{
    public enum DividendErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        InputValuesInvalid,
        DateExists,
        DateWrongFormat,
        ExchangeRatioEmpty,
        ExchangeRatioWrongFormat,
        ExchangeRatioWrongValue,
        RateEmpty,
        RateWrongFormat,
        RateWrongValue,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        TaxAtSourceWrongFormat,
        TaxAtSourceWrongValue,
        CapitalGainsTaxWrongFormat,
        CapitalGainsTaxWrongValue,
        SolidarityTaxWrongFormat,
        SolidarityTaxWrongValue,
        TaxWrongFormat,
        TaxWrongValue,
        PriceEmpty,
        PriceWrongFormat,
        PriceWrongValue,
        DocumentBrowseFailed,
        DocumentDoesNotExists
    }

    /// <summary>
    /// Interface of the ShareAdd view
    /// </summary>
    public interface IViewDividendEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValues;
        event EventHandler AddDividend;
        event EventHandler EditDividend;
        event EventHandler DeleteDividend;
        event EventHandler DocumentBrowse;

        DividendErrorCode ErrorCode { get; set; }

        bool UpdateDividend { get; set; }
        string SelectedDate { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        string Date { get; set; }
        string Time { get; set; }
        CheckState EnableFC { get; set; }
        string ExchangeRatio { get; set; }
        CultureInfo CultureInfoFC { get; }
        string Rate { get; set; }
        string Volume { get; set; }
        string Payout { get; set; }
        string PayoutFC { get; set; }
        string TaxAtSource { get; set; }
        string CapitalGainsTax { get; set; }
        string SolidarityTax { get; set; }
        string Tax { get; set; }
        string PayoutAfterTax { get; set; }
        string Yield { get; set; }
        string Price { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
        void DocumentBrowseFinish();
    }

    public partial class ViewDividendEdit : Form, IViewDividendEdit
    {
        #region Fields

        #region Transfer parameter

        /// <summary>
        /// Stores the chosen market share object
        /// </summary>
        ShareObjectMarketValue _shareObjectMarketValue = null;

        /// <summary>
        /// Stores the chosen final share object
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
        /// Stores if the share object dividend values should be taken or not
        /// </summary>
        bool _bLoadShareObjectDividendTaxValues;

        /// <summary>
        /// Stores if a dividend has been deleted or added
        /// and so a save must be done in the lower dialog
        /// </summary>
        private bool _bSave;

        /// <summary>
        /// Stores if a dividend should be load from the portfolio
        /// </summary>
        private bool _bLoadGridSelectionFlag;

        /// <summary>
        /// Stores if a dividend should be updated
        /// </summary>
        bool _bUpdateDividend;

        #endregion Flags

        #region Input values

        /// <summary>
        /// Stores if a foreign should be entered
        /// </summary>
        bool _bForeignFlag = false;

        /// <summary>
        /// Stores the exchange ratio value for the foreign currency
        /// </summary>
        decimal _decExchangeRatio = 1;

        /// <summary>
        /// Stores the culture info for the foreign currency
        /// </summary>
        CultureInfo _cultureInfoFC;

        /// <summary>
        /// Stores the dividend rate
        /// </summary>
        decimal _decRate = 0;

        /// <summary>
        /// Stores the volume of the shares
        /// </summary>
        decimal _decVolume = 0;

        /// <summary>
        /// Stores the dividend payout
        /// </summary>
        decimal _decDividendPayout = 0;
        decimal _decDividendPayoutFC = 0;

        /// <summary>
        /// Stores the tax at source pay value
        /// </summary>
        decimal _decTaxAtSource = 0;

        /// <summary>
        /// Stores the capital gains tax pay value
        /// </summary>
        decimal _decCapitalGainsTax = 0;

        /// <summary>
        /// Stores t he solidarity tax pay value
        /// </summary>
        decimal _decSolidarityTax = 0;

        /// <summary>
        /// Stores the dividend yield
        /// </summary>
        decimal _decDividendYield = 0;

        /// <summary>
        /// Stores the share price at the payout day
        /// </summary>
        decimal _decSharePriceAtThePayoutDay = 0;

        /// <summary>
        /// Stores the document for the dividend payout
        /// </summary>
        string _strDocument = @"";

        #endregion Input values

        /// <summary>
        /// Stores the DataGridView of the selected row
        /// </summary>
        DataGridView _selectedDataGridView = null;

        /// <summary>
        /// Stores the date of a selected dividend row
        /// </summary>
        string _selectedDate;

        /// <summary>
        /// Stores the current error code of the form
        /// </summary>
        private DividendErrorCode _errorCode;

        #endregion Fields

        #region IViewMember

        new

        DialogResult ShowDialog
        {
            get { return base.ShowDialog(); }
        }

        public DividendErrorCode ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public bool UpdateDividend
        {
            get { return _bUpdateDividend; }
            set
            {
                _bUpdateDividend = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UpdateDividend"));
            }
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

        public CheckState EnableFC
        {
            get { return chkBoxEnableFC.CheckState; }
            set
            {
                if (chkBoxEnableFC.CheckState == value)
                    return;
                chkBoxEnableFC.CheckState = value;
            }
        }

        public CultureInfo CultureInfoFC
        {
            get
            {
                if (cbxBoxDividendFCUnit.SelectedItem != null)
                {
                    CultureInformation ciTemp;
                    if (Helper.DictionaryListNameCultureInfoCurrencySymbol.TryGetValue(cbxBoxDividendFCUnit.SelectedItem.ToString().Split('/')[0].Trim(), out ciTemp))
                        return ciTemp.CultureInfo;
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public string ExchangeRatio
        {
            get { return txtBoxExchangeRatio.Text; }
            set
            {
                if (txtBoxExchangeRatio.Text == value)
                    return;
                txtBoxExchangeRatio.Text = value;
            }
        }

        public string Rate
        {
            get { return txtBoxRate.Text; }
            set
            {
                if (txtBoxRate.Text == value)
                    return;
                txtBoxRate.Text = value;
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

        public string PayoutFC
        {
            get { return txtBoxPayoutFC.Text; }
            set
            {
                if (txtBoxPayoutFC.Text == value)
                    return;
                txtBoxPayoutFC.Text = value;
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
            get { return txtBoxCapitalGainsTax.Text; }
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

        public string Tax
        {
            get { return txtBoxTax.Text; }
            set
            {
                if (txtBoxTax.Text == value)
                    return;
                txtBoxTax.Text = value;
            }
        }

        public string PayoutAfterTax
        {
            get { return txtBoxPayoutAfterTax.Text; }
            set
            {
                if (txtBoxPayoutAfterTax.Text == value)
                    return;
                txtBoxPayoutAfterTax.Text = value;
            }
        }

        public string Yield
        {
            get { return txtBoxYield.Text; }
            set
            {
                if (txtBoxYield.Text == value)
                    return;
                txtBoxYield.Text = value;
            }
        }

        public string Price
        {
            get { return txtBoxPrice.Text; }
            set
            {
                if (txtBoxPrice.Text == value)
                    return;
                txtBoxPrice.Text = value;
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
                case DividendErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        _bSave = true;
                        // Reset values
                        ResetValues();
                        // Refresh the buy list
                        ShowDividends();
                        break;
                    }
                case DividendErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add",
                                LanguageName);
                        btnAddSave.Image = Resources.black_add;
                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAddDividend.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/EditSuccess", LanguageName);
                        // Set flag to save the share object.
                        _bSave = true;
                        // Reset values
                        ResetValues();
                        // Refresh the buy list
                        ShowDividends();
                        break;
                    }
                case DividendErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        _bSave = true;
                        // Reset values
                        ResetValues();

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.black_add;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the buy list
                        ShowDividends();
                        break;
                    }
                case DividendErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
                case DividendErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
                case DividendErrorCode.DateExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DateExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        datePickerDate.Focus();
                        break;
                    }
                case DividendErrorCode.DateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        datePickerDate.Focus();
                        break;
                    }
                case DividendErrorCode.ExchangeRatioEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendExchangeRatioEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendExchangeRatioWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendExchangeRatioWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.RateEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxRate.Focus();
                        break;
                    }
                case DividendErrorCode.RateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxRate.Focus();
                        break;
                    }
                case DividendErrorCode.RateWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxRate.Focus();
                        break;
                    }
                case DividendErrorCode.PriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxPrice.Focus();
                        break;
                    }
                case DividendErrorCode.PriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxPrice.Focus();
                        break;
                    }
                case DividendErrorCode.PriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxPrice.Focus();
                        break;
                    }
                case DividendErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.DocumentDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
            }

            // Enable controls
            this.Enabled = true;

            Helper.AddStatusMessage(toolStripStatusLabelMessage,
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
            string strMessage = @"";
            Color clrMessage = Color.Black;
            FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case DividendErrorCode.DocumentBrowseFailed:
                    {
                        txtBoxDocument.Text = @"-";

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ChoseDocumentFailed", LanguageName);
                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessage,
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
        public event EventHandler FormatInputValues;
        public event EventHandler AddDividend;
        public event EventHandler EditDividend;
        public event EventHandler DeleteDividend;
        public event EventHandler EditTax;
        public event EventHandler DocumentBrowse;

        #endregion Event members

        #region Properties

        #region Transfer parameter

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

        #endregion Transfer parameter

        #region Flags

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

        #endregion Flags

        #endregion Properties

        #region Methods

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObject">Current chosen share object</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public ViewDividendEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue, Logger logger, Language xmlLanguage, String language)
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
        private void ShareDividendEdit_Load(object sender, EventArgs e)
        {
            try
            {
                _bLoadShareObjectDividendTaxValues = false;

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Caption", LanguageName);
                grpBoxAddDividend.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxDividends.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/Caption",
                        LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                lblEnableForeignCurrency.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/EnableForeignCurrency",
                    LanguageName);
                lblDividendExchangeRatio.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/ExchangeRatio",
                    LanguageName);
                lblDividendRate.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/DividendRate",
                        LanguageName);
                lblVolume.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Volume",
                        LanguageName);
                lblPayout.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Payout",
                        LanguageName);
                lblTaxAtSource.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/TaxAtSource",
                        LanguageName);
                lblCapitalGainsTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/CapitalGainsTax",
                        LanguageName);
                lblSolidarityTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/SolidarityTax",
                        LanguageName);
                lblAddTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Tax",
                        LanguageName);
                lblAddYield.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Yield",
                        LanguageName);
                txtBoxYield.Text = @"-";
                lblAddPayoutAfterTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/PayoutAfterTax",
                        LanguageName);
                lblAddPrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Price",
                        LanguageName);
                lblAddDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add",
                    LanguageName);
                btnReset.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Reset",
                    LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                // Load button images
                btnAddSave.Image = Resources.black_add;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                // Set dividend units to the edit boxes
                lblDividendRateUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblVolumeUnit.Text = ShareObjectFinalValue.PieceUnit;
                lblPayoutUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxAtSourceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCapitalGainsTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSolidarityTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPayoutAfterTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblYieldUnit.Text = ShareObjectFinalValue.PercentageUnit;
                lblPriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                // Set currency to ComboBox
                foreach (var temp in Helper.ListNameCultureInfoCurrencySymbol)
                {
                    cbxBoxDividendFCUnit.Items.Add(string.Format("{0} / {1}", temp.Key, temp.Value.CurrencySymbol));
                }

                CheckForeignCurrencyCalulationShouldBeDone();

                // Chose USD item
                int iIndex = cbxBoxDividendFCUnit.FindString("en-US");
                cbxBoxDividendFCUnit.SelectedIndex = iIndex;

                ShowDividends();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShareDividendEdit_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function set the focus on the volume edit box
        /// when the form is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareDividendEdit_Shown(object sender, EventArgs e)
        {
            txtBoxRate.Focus();
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a dividend has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareDividendEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SaveFlag)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date
        /// It also reset the background colors
        /// </summary>
        private void ResetValues()
        {
            // Reset date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            // Reset text boxes
            chkBoxEnableFC.CheckState = CheckState.Unchecked;
            txtBoxExchangeRatio.Text = @"";
            txtBoxVolume.Text = @"";
            txtBoxRate.Text = @"";
            txtBoxPayout.Text = @"";
            txtBoxPayoutFC.Text = @"";
            txtBoxTaxAtSource.Text = @"";
            txtBoxCapitalGainsTax.Text = @"";
            txtBoxSolidarityTax.Text = @"";
            txtBoxTax.Text = @"";
            txtBoxPayoutAfterTax.Text = @"";
            txtBoxYield.Text = @"";
            txtBoxPrice.Text = @"";
            txtBoxDocument.Text = @"";

            txtBoxRate.Focus();
        }

        #endregion Form

        #region DataGridView

        /// <summary>
        /// This function paints the dividend list of the share
        /// </summary>
        private void ShowDividends()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        DataGridView view = control as DataGridView;
                        if (view != null)
                        {
                            view.SelectionChanged -= DataGridViewDividendsOfAYear_SelectionChanged;
                            view.SelectionChanged -= DataGridViewDividendsOfYears_SelectionChanged;
                            view.DataBindingComplete -= DataGridViewDividensOfAYear_DataBindingComplete;
                        }
                    }
                    tabPage.Controls.Clear();
                    tabCtrlDividends.TabPages.Remove(tabPage);
                }

                tabCtrlDividends.TabPages.Clear();

                // Create TabPage for the dividends of the years
                TabPage newTabPageOverviewYears = new TabPage();
                // Set TabPage name
                newTabPageOverviewYears.Name =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview",
                        LanguageName);
                newTabPageOverviewYears.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview", LanguageName)
                                          + @" (" + ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsString + @")";

                // Create Binding source for the dividend data
                BindingSource bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues();

                // Create DataGridView
                DataGridView dataGridViewDividendsOverviewOfAYears = new DataGridView();
                dataGridViewDividendsOverviewOfAYears.Dock = DockStyle.Fill;
                // Bind source with dividend values to the DataGridView
                dataGridViewDividendsOverviewOfAYears.DataSource = bindingSourceOverview;
                // Set the delegate for the DataBindingComplete event
                dataGridViewDividendsOverviewOfAYears.DataBindingComplete += DataGridViewDividensOfAYear_DataBindingComplete;

                // Set row select event
                dataGridViewDividendsOverviewOfAYears.SelectionChanged += DataGridViewDividendsOfYears_SelectionChanged;

                // Advanced configuration DataGridView dividends
                DataGridViewCellStyle styleOverviewOfYears = dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                dataGridViewDividendsOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                dataGridViewDividendsOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewDividendsOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dataGridViewDividendsOverviewOfAYears.MultiSelect = false;

                dataGridViewDividendsOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewDividendsOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewDividendsOverviewOfAYears.AllowUserToResizeRows = false;

                dataGridViewDividendsOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                newTabPageOverviewYears.Controls.Add(dataGridViewDividendsOverviewOfAYears);
                dataGridViewDividendsOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlDividends.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlDividends;      

                // Check if dividend pays exists
                if (ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Count > 0)
                {
                    // Loop through the years of the dividend pays
                    foreach (
                        var keyName in ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Keys.Reverse()
                        )
                    {
                        // Create TabPage
                        TabPage newTabPage = new TabPage();
                        // Set TabPage name
                        newTabPage.Name = keyName;
                        newTabPage.Text = keyName + @" (" +
                                              ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                                                  .DividendValueYearWithUnitAsString
                                                  + @")";

                        // Create Binding source for the dividend data
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource =
                            ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName].DividendListYear;

                        // Create DataGridView
                        DataGridView dataGridViewDividendsOfAYear = new DataGridView();
                        dataGridViewDividendsOfAYear.Dock = DockStyle.Fill;
                        // Bind source with dividend values to the DataGridView
                        dataGridViewDividendsOfAYear.DataSource = bindingSource;
                        // Set the delegate for the DataBindingComplete event
                        dataGridViewDividendsOfAYear.DataBindingComplete += DataGridViewDividensOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewDividendsOfAYear.SelectionChanged += DataGridViewDividendsOfAYear_SelectionChanged;
                        // Set cell decimal click event
                        dataGridViewDividendsOfAYear.CellContentDoubleClick += DataGridViewDividendsOfAYear_CellContentdecimalClick;

                        // Advanced configuration DataGridView dividends
                        DataGridViewCellStyle style = dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle;
                        style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                        dataGridViewDividendsOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        dataGridViewDividendsOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                        dataGridViewDividendsOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                        dataGridViewDividendsOfAYear.RowHeadersVisible = false;
                        dataGridViewDividendsOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                        dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                        dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                        dataGridViewDividendsOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        dataGridViewDividendsOfAYear.MultiSelect = false;

                        dataGridViewDividendsOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewDividendsOfAYear.AllowUserToResizeColumns = false;
                        dataGridViewDividendsOfAYear.AllowUserToResizeRows = false;

                        dataGridViewDividendsOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        newTabPage.Controls.Add(dataGridViewDividendsOfAYear);
                        dataGridViewDividendsOfAYear.Parent = newTabPage;
                        tabCtrlDividends.Controls.Add(newTabPage);
                        newTabPage.Parent = tabCtrlDividends;
                    }
                }

                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;

                _bLoadShareObjectDividendTaxValues = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShowDividends()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the dividend document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void DataGridViewDividendsOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
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
                        // Get date and time of the selected buy item
                        string strDateTime = curItem[0].Cells[0].Value.ToString();

                        // Check if a document is set
                        if (curItem[0].Cells[iColumnCount - 1].Value.ToString() != @"-")
                        {
                            // Get doc from the dividend with the strDateTime
                            foreach (var temp in ShareObjectFinalValue.AllDividendEntries.GetAllDividendsOfTheShare())
                            {
                                // Check if the dividend date and time is the same as the date and time of the clicked buy item
                                if (temp.DateTime == strDateTime)
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
                                            // Remove dividend object and add it with no document
                                            if (ShareObjectFinalValue.RemoveDividend(temp.DateTime) &&
                                                ShareObjectFinalValue.AddDividend(temp.CultureInfoFC, temp.EnableFC, temp.ExchangeRatioDec, strDateTime, temp.RateDec, temp.VolumeDec,
                                                   temp.TaxAtSourceDec, temp.CapitalGainsTaxDec, temp.SolidarityTaxDec, temp.PriceDec))
                                            {
                                                // Set flag to save the share object.
                                                SaveFlag = true;

                                                ResetValues();
                                                ShowDividends();

                                                // Add status message
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    Language.GetLanguageTextByXPath(
                                                        @"/AddEditFormDividend/StateMessages/EditSuccess", LanguageName),
                                                    Language, LanguageName,
                                                    Color.Black, Logger, (int) FrmMain.EStateLevels.Info,
                                                    (int) FrmMain.EComponentLevels.Application);
                                            }
                                            else
                                            {
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    Language.GetLanguageTextByXPath(
                                                        @"/AddEditFormDividend/Errors/EditFailed", LanguageName),
                                                    Language, LanguageName,
                                                    Color.Red, Logger, (int) FrmMain.EStateLevels.Error,
                                                    (int) FrmMain.EComponentLevels.Application);
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
                MessageBox.Show("dataGridViewDividendsOfAYear_CellContentdecimalClick()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewDividendsOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlDividends.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlDividends.SelectedTab.Controls.Contains((DataGridView) sender))
                        DeselectRowsOfDataGridViews((DataGridView) sender);
                }

                if (((DataGridView) sender).SelectedRows.Count == 1)
                {
                    ResetValues();

                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView) sender).SelectedRows;

                    // Set selected date
                    SelectedDate = curItem[0].Cells[0].Value.ToString();

                    // Get DividendObject of the selected DataGridView row
                    DividendObject selectedDividendObject = ShareObjectFinalValue.AllDividendEntries.GetDividendObjectByDateTime(SelectedDate);
                    if (selectedDividendObject != null)
                    {
                        if (selectedDividendObject.EnableFC == CheckState.Checked)
                        {
                            LoadGridSelectionFlag = true;

                            // Set CheckBox for the exchange ratio
                            chkBoxEnableFC.CheckState = CheckState.Checked;

                            // Set foreign currency values
                            txtBoxExchangeRatio.Text = selectedDividendObject.ExchangeRatio;

                            // TODO find correct currency
                            //if (selectedDividendObject.DividendTaxes.FCUnit != null)
                            //    cbxBoxDividendFCUnit.SelectedIndex = cbxBoxDividendFCUnit.FindString(new RegionInfo(selectedDividendObject.DividendTaxes.CiShareFC.LCID).ISOCurrencySymbol); // ?NAME

                            cbxBoxDividendFCUnit.SelectedIndex = cbxBoxDividendFCUnit.FindString(selectedDividendObject.CultureInfoFC.Name);

                            LoadGridSelectionFlag = false;
                        }
                        else
                        {
                            txtBoxRate.Text = selectedDividendObject.Rate;

                            // Chose USD item
                            int iIndex = cbxBoxDividendFCUnit.FindString("en-US");
                            cbxBoxDividendFCUnit.SelectedIndex = iIndex;
                        }

                        datePickerDate.Value = Convert.ToDateTime(selectedDividendObject.DateTime);
                        datePickerTime.Value = Convert.ToDateTime(selectedDividendObject.DateTime);
                        txtBoxRate.Text = selectedDividendObject.Rate;
                        txtBoxVolume.Text = selectedDividendObject.Volume;
                        txtBoxTaxAtSource.Text = selectedDividendObject.TaxAtSource;
                        txtBoxCapitalGainsTax.Text = selectedDividendObject.CapitalGainsTax;
                        txtBoxSolidarityTax.Text = selectedDividendObject.SolidarityTax;
                        txtBoxYield.Text = selectedDividendObject.Yield;
                        txtBoxPrice.Text = selectedDividendObject.Price;
                        txtBoxDocument.Text = selectedDividendObject.Document;
                    }
                    else
                    {
                        datePickerDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        datePickerTime.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        txtBoxPayout.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxRate.Text = curItem[0].Cells[2].Value.ToString();
                        txtBoxYield.Text = curItem[0].Cells[3].Value.ToString();
                        txtBoxPrice.Text = curItem[0].Cells[4].Value.ToString();
                        txtBoxVolume.Text = curItem[0].Cells[5].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[6].Value.ToString();
                    }

                    // Enable button(s)
                    //btnReset.Enabled = true;
                    btnDelete.Enabled = true;
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;
                    // Disable button(s)
                    //btnReset.Enabled = false;
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewDividendsOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewDividendsOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView) sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView) sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlDividends.SelectTab(tabPage);
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
                MessageBox.Show("dataGridViewDividendsOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DataGridViewDividensOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (int i = 0; i < ((DataGridView) sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView) sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Date",
                                    LanguageName);
                            break;
                        case 1:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Payout",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 2:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Dividend",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 3:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Yield",
                                    LanguageName) + @" (" + ShareObject.PercentageUnit + @")";
                            break;
                        case 4:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Price",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 5:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Volume",
                                    LanguageName) + @" (" + ShareObject.PieceUnit + @")";
                            ;
                            break;
                        case 6:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Document",
                                    LanguageName);
                            ;
                            break;
                    }
                }

                if (((DataGridView) sender).Rows.Count > 0)
                    ((DataGridView) sender).Rows[0].Selected = false;

                // Reset the text box values
                ResetValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewDividensOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RenameColHeaderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }

        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        private void DeselectRowsOfDataGridViews(DataGridView DataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage TabPage in tabCtrlDividends.TabPages)
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

                ResetValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("DeselectRowsOfDataGridViews()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeselectFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion DataGridView

        #region Buttons

        /// <summary>
        /// This function adds a new dividend entry to the share object
        /// or edit a dividend entry
        /// It also checks if an entry already exists for the given date
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="arg">EventArgs</param>
        private void BtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                // Disable controls
                this.Enabled = false;

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    UpdateDividend = false;

                    if (AddDividend != null)
                        AddDividend(this, null);
                }
                else
                {
                    UpdateDividend = true;

                    if (EditDividend != null)
                        EditDividend(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnAdd_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/AddFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the dividend.
        /// If the user press "Ok" the dividend will be deleted and the
        /// list of the dividends will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">EventArgs</param>
        private void BtnDividendDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                string strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                string strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/DividendDelete",
                    LanguageName);
                string strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                string strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                DialogResult dlgResult = messageBox.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Check if a row is selected
                    if (SelectedDataGridView != null && SelectedDataGridView.SelectedRows.Count == 1)
                    {
                        if (DeleteDividend != null)
                            DeleteDividend(this, null);
                    }

                    // Reset values
                    ResetValues();

                    // Enable button(s)
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;

                    // Disable button(s)
                    //btnReset.Enabled = false;
                    //btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Do not load share object dividend tax values
                    _bLoadShareObjectDividendTaxValues = false;

                    // Refresh the dividend list
                    ShowDividends();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnDividendDelete_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeleteFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a dividend
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                // Enable button(s)
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.black_add;

                // Rename group box
                grpBoxAddDividend.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                SelectedDataGridView = null;

                // Select overview tab
                if (
                    tabCtrlDividends.TabPages.ContainsKey(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview", LanguageName)))
                    tabCtrlDividends.SelectTab(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview", LanguageName));
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnReset_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CancelFailure", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function closes the dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the dividend pay of the share
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void BtnAddDividendDocumentBrowse_Click(object sender, EventArgs e)
        {
            if (DocumentBrowse != null)
                DocumentBrowse(this, new EventArgs());
        }

        #endregion Buttons

        #region CheckBoxes

        /// <summary>
        /// This function checks the CheckBox enable foreign currency
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">EventArgs</param>
        private void ChkBoxAddDividendForeignCurrency_CheckedChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("EnableFC"));

            CheckForeignCurrencyCalulationShouldBeDone();
        }

        #endregion CheckBoxes

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the exchange ratio has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void TxtBoxAddDividendExchangeRatio_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("ExchangeRatio"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxExchangeRatio_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function update the model if the rate has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void TxtBoxAddDividendRate_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Rate"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxRate_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updated the model if the foreign rate has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void TxtBoxAddDividendRateFC_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("RateFC"));
        }

        /// <summary>
        /// This function updates the model if volume has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void TxtBoxAddVolume_TextChanged(object sender, EventArgs e)
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
        /// This function updates the model if tax at source value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxTaxAtSource_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("TaxAtSource"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxTaxAtSource_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if capital gains tax value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxCapitalGainsTax_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("CapitalGainsTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxCapitalGainsTax_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if solidarity tax value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxSolidarityTax_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("SolidarityTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxSolidarityTax_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void txtBoxPrice_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the price has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void TxtBoxAddPrice_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Price"));
        }

        #endregion TextBoxes

        /// <summary>
        /// This function check is a foreign current calculation should be done.
        /// The function shows or hides, resize and reposition controls
        /// </summary>
        private void CheckForeignCurrencyCalulationShouldBeDone()
        {
            // Reset date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            if (chkBoxEnableFC.CheckState == CheckState.Checked)
            {
                _bForeignFlag = true;

                // Enable controls
                txtBoxExchangeRatio.Enabled = true;
                txtBoxExchangeRatio.ReadOnly = false;
                txtBoxTaxAtSource.Enabled = true;
                txtBoxTaxAtSource.ReadOnly = false;
                cbxBoxDividendFCUnit.Enabled = true;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxExchangeRatio.Text = @"";
                    txtBoxVolume.Text = @"";
                    txtBoxRate.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPayoutFC.Text = @"";
                    txtBoxTaxAtSource.Text = @"";
                    txtBoxCapitalGainsTax.Text = @"";
                    txtBoxSolidarityTax.Text = @"";
                    txtBoxTax.Text = @"";
                    txtBoxPayoutAfterTax.Text = @"";
                    txtBoxPrice.Text = @"";
                    txtBoxDocument.Text = @"";

                    // Reset status message
                    toolStripStatusLabelMessage.Text = @"";
                }

                txtBoxExchangeRatio.Focus();
            }
            else
            {
                _bForeignFlag = false;

                // Disable controls
                txtBoxExchangeRatio.Enabled = false;
                txtBoxExchangeRatio.ReadOnly = true;
                cbxBoxDividendFCUnit.Enabled = false;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxExchangeRatio.Text = @"";
                    txtBoxVolume.Text = @"";
                    txtBoxRate.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPayoutFC.Text = @"";
                    txtBoxTaxAtSource.Text = @"";
                    txtBoxCapitalGainsTax.Text = @"";
                    txtBoxSolidarityTax.Text = @"";
                    txtBoxTax.Text = @"";
                    txtBoxPayoutAfterTax.Text = @"";
                    txtBoxPrice.Text = @"";
                    txtBoxDocument.Text = @"";

                    // Reset status message
                    toolStripStatusLabelMessage.Text = @"";
                }

                txtBoxRate.Focus();
            }
        }

        /// <summary>
        /// This function changes the currency unit when the currency
        /// has been changed and changes the currency unit to the taxes
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">EventArgs</param>
        private void CbxBoxAddDividendForeignCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPayoutFCUnit.Text = ((ComboBox)sender).SelectedItem.ToString().Split('/')[1].Trim();

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("CultureInfoFC"));
        }

        #endregion Methods
    }
}
