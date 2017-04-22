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
using SharePortfolioManager.Classes.Taxes;
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
        DeleteFailedUnerasable,
        InputeValuesInvalid,
        DateExists,
        DateWrongFormat,
        ExchangeRatioEmpty,
        ExchangeRatioWrongFormat,
        ExchangeRatioWrongValue,
        RateEmpty,
        RateWrongFormat,
        RateWrongValue,
        RateFCEmpty,
        RateFCWrongFormat,
        RateFCWrongValue,
        LossBalanceWrongFormat,
        LossBalanceWrongValue,
        PriceEmpty,
        PriceWrongFormat,
        PriceWrongValue,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
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
        event EventHandler EditTax;
        event EventHandler DocumentBrowse;

        DividendErrorCode ErrorCode { get; set; }

        bool UpdateDividend { get; set; }
        string SelectedDate { get; set; }
        ShareObject ShareObject { get; set; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        string Date { get; set; }
        string Time { get; set; }
        CheckState EnableFC { get; set; }
        string ExchangeRatio { get; set; }
        CultureInfo CultureInfoFC { get; set; }
        string Rate { get; set; }
        string RateFC { get; set; }
        string Volume { get; set; }
        string LossBalance { get; set; }
        string Payout { get; set; }
        Taxes TaxValuesCurrent { get; set; }
        string Tax { get; set; }
        string PayoutAfterTax { get; set; }
        string Yield { get; set; }
        string Price { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
    }

    public partial class ViewDividendEdit : Form, IViewDividendEdit
    {
        #region Fields

        #region Transfer parameter

        /// <summary>
        /// Stores the chosen share object
        /// </summary>
        ShareObject _shareObject = null;

        /// <summary>
        /// Stores the logger
        /// </summary>
        Logger _logger;

        /// <summary>
        /// Stores the given language file
        /// </summary>
        Language _xmlLanguage;

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

        #region Tax values

        /// <summary>
        /// Stores the values for the current tax values
        /// </summary>
        Taxes _taxValusCurrent;

        /// <summary>
        /// Stores the values for the usual tax values
        /// </summary>
        Taxes _taxValuesNormal;

        /// <summary>
        /// Stores the values for the edit tax values
        /// </summary>
        Taxes _taxValueDividend;

        #endregion Tax values

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
        /// Stores the volume of the shares
        /// </summary>
        decimal _decVolume = 0;

        /// <summary>
        /// Stores the dividend rate
        /// </summary>
        decimal _decRate = 0;
        decimal _decRateFC = 0;

        /// <summary>
        /// Stores the loss balance
        /// </summary>
        decimal _decLossBalance = 0;
        decimal _decLossBalanceFC = 0;

        /// <summary>
        /// Stores the dividend payout
        /// </summary>
        decimal _decDividendPayout = 0;
        decimal _decDividendPayoutFC = 0;

        /// <summary>
        /// Stores the dividend tax
        /// </summary>
        decimal _decTax = 0;
        decimal _decTaxFC = 0;

        /// <summary>
        /// Stores the dividend yield
        /// </summary>
        decimal _decDividendYield = 0;

        /// <summary>
        /// Stores the share price at the payout day
        /// </summary>
        decimal _decSharePriceAtThePayoutDay = 0;
        decimal _decSharePriceAtThePayoutDayFC = 0;

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

        public ShareObject ShareObject
        {
            get { return _shareObject; }
            set { _shareObject = value; }
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

        public Taxes TaxesCurrent
        {
            get { return _taxValusCurrent; }
            set { _taxValusCurrent = value; }
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
            get { return _cultureInfoFC; }
            set { _cultureInfoFC = value; }
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

        public string RateFC
        {
            get { return txtBoxRateFC.Text; }
            set
            {
                if (txtBoxRateFC.Text == value)
                    return;
                txtBoxRateFC.Text = value;
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
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/AddSuccess", LanguageName);
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
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                //    case BuyErrorCode.EditSuccessful:
                //        {
                //            // Enable button(s)
                //            btnAddSave.Text =
                //                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add",
                //                    _strLanguage);
                //            btnAddSave.Image = Resources.black_add;
                //            // Disable button(s)
                //            btnReset.Enabled = false;
                //            btnDelete.Enabled = false;

                //            // Rename group box
                //            grpBoxAdd.Text =
                //                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption",
                //                    _strLanguage);

                //            strMessage =
                //                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/EditSuccess", _strLanguage);
                //            // Set flag to save the share object.
                //            _bSave = true;
                //            // Reset values
                //            ResetValues();
                //            // Refresh the buy list
                //            ShowBuys();
                //            break;
                //        }
                //    case BuyErrorCode.EditFailed:
                //        {
                //            strMessage =
                //                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/EditFailed", _strLanguage);
                //            clrMessage = Color.Red;
                //            stateLevel = FrmMain.EStateLevels.Error;
                //            txtBoxVolume.Focus();
                //            break;
                //        }
                //    case BuyErrorCode.DeleteSuccessful:
                //        {
                //            strMessage =
                //                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/DeleteSuccess", _strLanguage);
                //            // Set flag to save the share object.
                //            _bSave = true;
                //            // Reset values
                //            ResetValues();

                //            // Enable button(s)
                //            btnAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", _strLanguage);
                //            btnAddSave.Image = Resources.black_add;

                //            // Disable button(s)
                //            btnReset.Enabled = false;
                //            btnDelete.Enabled = false;

                //            // Rename group box
                //            grpBoxAdd.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", _strLanguage);

                //            // Refresh the buy list
                //            ShowBuys();
                //            break;
                //        }
                //    case BuyErrorCode.DeleteFailed:
                //        {
                //            strMessage =
                //                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", _strLanguage);
                //            clrMessage = Color.Red;
                //            stateLevel = FrmMain.EStateLevels.Error;
                //            break;
                //        }
                //    case BuyErrorCode.DeleteFailedUnerasable:
                //        {
                //            strMessage =
                //                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailedUnerasable", _strLanguage);
                //            clrMessage = Color.Red;
                //            stateLevel = FrmMain.EStateLevels.Error;
                //            break;
                //        }
                case DividendErrorCode.DateExists:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DateExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        datePickerDate.Focus();
                        break;
                    }
                case DividendErrorCode.DateWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        datePickerDate.Focus();
                        break;
                    }
                case DividendErrorCode.ExchangeRatioEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendForeignCurrencyFactorEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendForeignCurrencyFactorWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendCurrencyFactorWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.RateFCEmpty:
                    {
                        txtBoxRateFC.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendFCEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.RateFCWrongFormat:
                    {
                        txtBoxRateFC.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendFCWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.RateFCWrongValue:
                    {
                        txtBoxRateFC.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendFCWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.RateEmpty:
                    {
                        txtBoxRate.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.RateWrongFormat:
                    {
                        txtBoxRate.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.RateWrongValue:
                    {
                        txtBoxRate.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DividendWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.LossBalanceWrongFormat:
                    {
                        txtBoxLossBalance.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/LossBalanceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.LossBalanceWrongValue:
                    {
                        txtBoxLossBalance.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/LossBalanceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.PriceEmpty:
                    {
                        txtBoxPrice.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.PriceWrongFormat:
                    {
                        txtBoxPrice.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.PriceWrongValue:
                    {
                        txtBoxPrice.Focus();

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxExchangeRatio.Focus();
                        break;
                    }
                case DividendErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case DividendErrorCode.DocumentDoesNotExists:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongValue", LanguageName);
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
               _xmlLanguage,
               _languageName,
               clrMessage,
               _logger,
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
            get { return _xmlLanguage; }
            internal set { _xmlLanguage = value; }
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

        #region Tax values

        public Taxes TaxValuesCurrent
        {
            get { return _taxValusCurrent; }
            set { _taxValusCurrent = value; }
        }

        public Taxes TaxValuesNormal
        {
            get { return _taxValuesNormal; }
            internal set { _taxValuesNormal = value; }
        }

        public Taxes TaxValuesEditDividend
        {
            get { return _taxValueDividend; }
            set { _taxValueDividend = value; }
        }

        #endregion Tax values

        #endregion Properties

        #region Methods

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObject">Current chosen share object</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public ViewDividendEdit(ShareObject shareObject, Logger logger, Language xmlLanguage, String language)
        {
            InitializeComponent();

            ShareObject = shareObject;
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

                // Set tax values
                if (TaxValuesNormal == null)
                    TaxValuesNormal = new Taxes();

                if (TaxValuesCurrent == null)
                    TaxValuesCurrent = new Taxes();

                TaxValuesNormal.TaxAtSourceFlag = ShareObject.TaxTaxAtSourceFlag;
                TaxValuesNormal.TaxAtSourcePercentage = ShareObject.TaxTaxAtSourcePercentage;
                TaxValuesNormal.CapitalGainsTaxFlag = ShareObject.TaxCapitalGainsFlag;
                TaxValuesNormal.CapitalGainsTaxPercentage = ShareObject.TaxCapitalGainsPercentage;
                TaxValuesNormal.SolidarityTaxFlag = ShareObject.TaxSolidarityFlag;
                TaxValuesNormal.SolidarityTaxPercentage = ShareObject.TaxSolidarityPercentage;
                TaxValuesNormal.CiShareCurrency = ShareObject.CultureInfo;

                TaxValuesCurrent.DeepCopy(TaxValuesNormal);

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Caption", LanguageName);
                grpBoxAddDividend.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxDividends.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/Caption",
                        LanguageName);
                lblAddDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                lblAddEnableForeignCurrency.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/EnableForeignCurrency",
                    LanguageName);
                lblAddDividendExchangeRatio.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/ExchangeRatio",
                    LanguageName);
                lblAddDividendRate.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/DividendRate",
                        LanguageName);
                lblAddVolume.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Volume",
                        LanguageName);
                lblAddLossBalance.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/LossBalance",
                        LanguageName);
                lblAddPayout.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Payout",
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
                btnAddSave.Image = Resources.black_save;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                // Set dividend units to the edit boxes
                lblDividendRateUnit.Text = _shareObject.CurrencyUnit;
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblLossBalanceUnit.Text = _shareObject.CurrencyUnit;
                lblPayoutUnit.Text = _shareObject.CurrencyUnit;
                lblTaxUnit.Text = _shareObject.CurrencyUnit;
                lblPayoutAfterTaxUnit.Text = _shareObject.CurrencyUnit;
                lblYieldUnit.Text = ShareObject.PercentageUnit;
                lblPriceUnit.Text = _shareObject.CurrencyUnit;

                // Set currency to ComboBox
                foreach (var temp in Helper.ListNameUnitCurrency)
                {
                    cbxBoxDividendFCUnit.Items.Add(string.Format("{0} - {1}", temp.Key, temp.Value));
                }

                CheckForeignCurrencyCalulationShouldBeDone();

                // Chose USD item
                int iIndex = cbxBoxDividendFCUnit.FindString("USD");
                cbxBoxDividendFCUnit.SelectedIndex = iIndex;

                // Set currency units
                TaxValuesCurrent.CurrencyUnit = lblPayoutUnit.Text;
                TaxValuesCurrent.FCUnit = cbxBoxDividendFCUnit.SelectedItem.ToString().Split('-')[1].Trim();
                TaxValuesCurrent.CiShareFC = Helper.GetCultureByISOCurrencySymbol(cbxBoxDividendFCUnit.SelectedItem.ToString().Split('-')[0].Trim());

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
            txtBoxLossBalance.Text = @"";
            txtBoxPayout.Text = @"";
            txtBoxTax.Text = @"";
            txtBoxPayoutAfterTax.Text = @"";
            txtBoxRate.Text = @"";
            txtBoxRateFC.Text = @"";
            txtBoxPrice.Text = @"";
            txtBoxDocument.Text = @"";

            // Set normal tax values
            TaxValuesCurrent.DeepCopy(TaxValuesNormal);

            // Set currency units
            TaxValuesCurrent.CurrencyUnit = lblPayoutUnit.Text;
            if (cbxBoxDividendFCUnit.SelectedItem != null)
            {
                TaxValuesCurrent.FCUnit = cbxBoxDividendFCUnit.SelectedItem.ToString().Split('-')[1].Trim();
                TaxValuesCurrent.CiShareFC = Helper.GetCultureByISOCurrencySymbol(cbxBoxDividendFCUnit.SelectedItem.ToString().Split('-')[0].Trim());
            }

            // Reset status message
            toolStripStatusLabelMessage.Text = @"";

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
                // Reset tax values to normal tax values
                TaxValuesCurrent.DeepCopy(TaxValuesNormal);

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
                                          + @" (" + ShareObject.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsString + @")";

                // Create Binding source for the dividend data
                BindingSource bindingSourceOverview = new BindingSource();
                if (ShareObject.AllDividendEntries.GetAllDividendsTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObject.AllDividendEntries.GetAllDividendsTotalValues();

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
                if (ShareObject.AllDividendEntries.AllDividendsOfTheShareDictionary.Count > 0)
                {
                    // Loop through the years of the dividend pays
                    foreach (
                        var keyName in ShareObject.AllDividendEntries.AllDividendsOfTheShareDictionary.Keys.Reverse()
                        )
                    {
                        // Create TabPage
                        TabPage newTabPage = new TabPage();
                        // Set TabPage name
                        newTabPage.Name = keyName;
                        newTabPage.Text = keyName + @" (" +
                                              ShareObject.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                                                  .DividendValueYearWithUnitAsString
                                                  + @")";

                        // Create Binding source for the dividend data
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource =
                            ShareObject.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName].DividendListYear;

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
                            foreach (var temp in ShareObject.AllDividendEntries.GetAllDividendsOfTheShare())
                            {
                                // Check if the dividend date and time is the same as the date and time of the clicked buy item
                                if (temp.DividendDate == strDateTime)
                                {
                                    // Check if the file still exists
                                    if (File.Exists(temp.DividendDocument))
                                        // Open the file
                                        Process.Start(temp.DividendDocument);
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
                                            if (ShareObject.RemoveDividend(temp.DividendDate) &&
                                                ShareObject.AddDividend(strDateTime, temp.DividendTaxes, temp.DividendRate, temp.LossBalance,
                                                    temp.SharePrice, temp.ShareVolume))
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

                    // Get DividendObject of the selected DataGridView row
                    DividendObject selectedDividendObject = ShareObject.AllDividendEntries.GetDividendObjectByDateTime(curItem[0].Cells[0].Value.ToString());
                    if (selectedDividendObject != null)
                    {
                        if (selectedDividendObject.DividendTaxes.FCFlag)
                        {
                            LoadGridSelectionFlag = true;

                            // Set CheckBox for the exchange ratio
                            chkBoxEnableFC.CheckState = CheckState.Checked;

                            // Set foreign currency values
                            txtBoxExchangeRatio.Text = selectedDividendObject.DividendTaxes.ExchangeRatio.ToString();
                            txtBoxRateFC.Text = selectedDividendObject.DividendRateAsString;

                            // Calculate the values in the normal currency from the given foreign currency values
                            //txtBoxAddDividendRate.Text = Helper.FormatDecimal((selectedDividendObject.DividendRate / selectedDividendObject.DividendTaxes.ExchangeRatio),
                            //    Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObject.ShareCulture);
                            // TODO find correct currency
                            if (selectedDividendObject.DividendTaxes.FCUnit != null)
                                cbxBoxDividendFCUnit.SelectedIndex = cbxBoxDividendFCUnit.FindString(new RegionInfo(selectedDividendObject.DividendTaxes.CiShareFC.LCID).ISOCurrencySymbol); // ?NAME

                            LoadGridSelectionFlag = false;
                        }
                        else
                        {
                            txtBoxRate.Text = selectedDividendObject.DividendRateAsString;

                            // Chose USD item
                            int iIndex = cbxBoxDividendFCUnit.FindString("USD");
                            cbxBoxDividendFCUnit.SelectedIndex = iIndex;
                        }

                        // Set tax values to the edit dividend values only if the form is already opened
                        if (_bLoadShareObjectDividendTaxValues)
                            TaxValuesCurrent.DeepCopy(selectedDividendObject.DividendTaxes);

                        datePickerDate.Value = Convert.ToDateTime(selectedDividendObject.DividendDate);
                        datePickerTime.Value = Convert.ToDateTime(selectedDividendObject.DividendDate);
                        txtBoxVolume.Text = selectedDividendObject.ShareVolumeAsString;
                        txtBoxLossBalance.Text = selectedDividendObject.LossBalanceAsString;
                        //txtBoxAddPayout.Text = selectedDividendObject.DividendPayOutAsString;
                        txtBoxYield.Text = selectedDividendObject.DividendYieldAsString;
                        txtBoxPrice.Text = selectedDividendObject.SharePriceAsString;
                        txtBoxDocument.Text = selectedDividendObject.DividendDocument;
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
                    btnReset.Enabled = true;
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
                    btnReset.Enabled = false;
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
                                    LanguageName) + @" (" + _shareObject.CurrencyUnit + @")";
                            break;
                        case 2:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Dividend",
                                    LanguageName) + @" (" + _shareObject.CurrencyUnit + @")";
                            break;
                        case 3:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Yield",
                                    LanguageName) + @" (" + ShareObject.PercentageUnit + @")";
                            break;
                        case 4:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Price",
                                    LanguageName) + @" (" + _shareObject.CurrencyUnit + @")";
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
                // Disable controls
                this.Enabled = false;

                if (btnAddSave.Text == _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", _languageName))
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
                        if (
                            !ShareObject.RemoveDividend(SelectedDataGridView.SelectedRows[0].Cells[0].Value.ToString()))
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeleteFailed", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/DeleteSuccess", LanguageName),
                                Language, LanguageName,
                                Color.Black, Logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
                        }
                    }

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

                // Disable button(s)
                btnReset.Enabled = false;
                btnDelete.Enabled = false;

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

                // Reset tax values to normal tax values
                TaxValuesCurrent.DeepCopy(TaxValuesNormal);
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
        /// This function updates the model if the loss balance has been changed
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddLossBalance_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("LossBalance"));
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

        /// <summary>
        /// This function opens the dividend taxes edit dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTxtBoxAddTax_decimalClick(object sender, EventArgs e)
        {
            if (EditTax != null)
                EditTax(this, new EventArgs());
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
                cbxBoxDividendFCUnit.Enabled = true;

                txtBoxRateFC.Enabled = true;
                txtBoxRateFC.ReadOnly = false;

                // Disable controls
                txtBoxRate.Enabled = false;
                txtBoxRate.ReadOnly = true;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxExchangeRatio.Text = @"";
                    txtBoxVolume.Text = @"";
                    txtBoxRate.Text = @"";
                    txtBoxRateFC.Text = @"";
                    txtBoxLossBalance.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPrice.Text = @"";
                    txtBoxDocument.Text = @"";
                }
                txtBoxExchangeRatio.Focus();

                // Set edit values variable
                TaxValuesCurrent.FCFlag = true;
            }
            else
            {
                _bForeignFlag = false;

                // Disable controls
                txtBoxExchangeRatio.Enabled = false;
                txtBoxExchangeRatio.ReadOnly = true;
                cbxBoxDividendFCUnit.Enabled = false;

                txtBoxRateFC.Enabled = false;
                txtBoxRateFC.ReadOnly = true;

                // Enable controls
                txtBoxRate.Enabled = true;
                txtBoxRate.ReadOnly = false;
                txtBoxPrice.Enabled = true;
                txtBoxPrice.ReadOnly = false;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxExchangeRatio.Text = @"";
                    txtBoxVolume.Text = @"";
                    txtBoxRate.Text = @"";
                    txtBoxRateFC.Text = @"";
                    txtBoxLossBalance.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPrice.Text = @"";
                    txtBoxDocument.Text = @"";
                }

                txtBoxExchangeRatio.Focus();

                // Set edit values variable
                TaxValuesCurrent.FCFlag = false;
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
            lblRateFCUnit.Text = ((ComboBox)sender).SelectedItem.ToString().Split('-')[1].Trim();

            // Set currency units
            TaxValuesCurrent.CurrencyUnit = lblPayoutUnit.Text;
            if (((ComboBox)sender).SelectedItem != null)
            {
                TaxValuesCurrent.FCUnit = ((ComboBox)sender).SelectedItem.ToString().Split('-')[1].Trim();
                TaxValuesCurrent.CiShareFC = Helper.GetCultureByISOCurrencySymbol(((ComboBox)sender).SelectedItem.ToString().Split('-')[0].Trim());
                CultureInfoFC = TaxValuesCurrent.CiShareFC;
            }

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("TaxValuesCurrent"));
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("CultureInfoFC"));
        }

        #endregion Methods
    }
}
