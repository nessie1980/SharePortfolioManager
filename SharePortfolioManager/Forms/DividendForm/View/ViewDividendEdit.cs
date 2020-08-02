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
//#define DEBUG_DIVIDEND_EDIT_VIEW

using LanguageHandler;
using Logging;
using Parser;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.OwnMessageBoxForm;
using SharePortfolioManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SharePortfolioManager.DividendForm.View
{
    // Error codes of the DividendEdit
    public enum DividendErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        InputValuesInvalid,
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
        PriceEmpty,
        PriceWrongFormat,
        PriceWrongValue,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExists,
        DocumentFileDoesNotExists
    }

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

    /// <inheritdoc />
    /// <summary>
    /// Interface of the DividendEdit view
    /// </summary>
    public interface IViewDividendEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler AddDividendEventHandler;
        event EventHandler EditDividendEventHandler;
        event EventHandler DeleteDividendEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        DividendErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool UpdateDividend { get; set; }
        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        CheckState EnableFc { get; set; }
        string ExchangeRatio { get; set; }
        CultureInfo CultureInfoFc { get; }
        string Rate { get; set; }
        string Volume { get; set; }
        string Payout { get; set; }
        string PayoutFc { get; set; }
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

        /// <summary>
        /// Stores the Guid of a selected dividend row
        /// </summary>
        private string _selectedGuid;

        /// <summary>
        /// Stores the date of a selected dividend row
        /// </summary>
        private string _selectedDate;

        /// <summary>
        /// Stores the last focused date time picker or text box
        /// </summary>
        private Control _focusedControl;

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
        /// Flag if the buy identifier type of the given document could be found in the document configurations
        /// </summary>
        private bool _dividendIdentifierFound;

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

        public bool UpdateDividendFlag { get; set; }

        public bool SaveFlag { get; internal set; }

        public bool LoadGridSelectionFlag { get; internal set; }

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

        public bool UpdateDividend
        {
            get => UpdateDividendFlag;
            set
            {
                UpdateDividendFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateDividend"));
            }
        }

        public DividendErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; }

        public string SelectedGuid
        {
            get => _selectedGuid;
            set
            {
                _selectedGuid = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedGuid"));
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

        public CheckState EnableFc
        {
            get => chkBoxEnableFC.CheckState;
            set
            {
                if (chkBoxEnableFC.CheckState == value)
                    return;
                chkBoxEnableFC.CheckState = value;
            }
        }

        public CultureInfo CultureInfoFc
        {
            get
            {
                if (cbxBoxDividendFCUnit.SelectedItem != null)
                {
                    return Helper.DictionaryListNameCultureInfoCurrencySymbol.TryGetValue(cbxBoxDividendFCUnit.SelectedItem.ToString().Split('/')[0].Trim(), out var ciTemp) ? ciTemp.CultureInfo : null;
                }

                return null;
            }
        }

        public string ExchangeRatio
        {
            get => txtBoxExchangeRatio.Text;
            set
            {
                if (txtBoxExchangeRatio.Text == value)
                    return;
                txtBoxExchangeRatio.Text = value;
            }
        }

        public string Rate
        {
            get => txtBoxDividendRate.Text;
            set
            {
                if (txtBoxDividendRate.Text == value)
                    return;
                txtBoxDividendRate.Text = value;
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

        public string PayoutFc
        {
            get => txtBoxPayoutFC.Text;
            set
            {
                if (txtBoxPayoutFC.Text == value)
                    return;
                txtBoxPayoutFC.Text = value;
            }
        }

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

        public string Tax
        {
            get => txtBoxTax.Text;
            set
            {
                if (txtBoxTax.Text == value)
                    return;
                txtBoxTax.Text = value;
            }
        }

        public string PayoutAfterTax
        {
            get => txtBoxPayoutAfterTax.Text;
            set
            {
                if (txtBoxPayoutAfterTax.Text == value)
                    return;
                txtBoxPayoutAfterTax.Text = value;
            }
        }

        public string Yield
        {
            get => txtBoxYield.Text;
            set
            {
                if (txtBoxYield.Text == value)
                    return;
                txtBoxYield.Text = value;
            }
        }

        public string Price
        {
            get => txtBoxSharePrice.Text;
            set
            {
                if (txtBoxSharePrice.Text == value)
                    return;
                txtBoxSharePrice.Text = value;
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
            var strMessage = @"";
            var clrMessage = Color.Black;
            var stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case DividendErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the buy list
                        OnShowDividends();

                        break;
                    }
                case DividendErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add",
                                LanguageName);
                        btnAddSave.Image = Resources.button_add_24;
                        // Disable button(s)
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAddDividend.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/EditSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the buy list
                        OnShowDividends();

                        break;
                    }
                case DividendErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.button_add_24;

                        // Disable button(s)
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the buy list
                        OnShowDividends();

                        break;
                    }
                case DividendErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.ExchangeRatioEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ExchangeRatioEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxExchangeRatio.Focus();

                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ExchangeRatioWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxExchangeRatio.Focus();

                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ExchangeRatioWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxExchangeRatio.Focus();

                        break;
                    }
                case DividendErrorCode.RateEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RateEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDividendRate.Focus();

                        break;
                    }
                case DividendErrorCode.RateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDividendRate.Focus();

                        break;
                    }
                case DividendErrorCode.RateWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RateWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDividendRate.Focus();

                        break;
                    }
                case DividendErrorCode.TaxAtSourceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/TaxAtSourceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case DividendErrorCode.TaxAtSourceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/TaxAtSourceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case DividendErrorCode.CapitalGainsTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CapitalGainsTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case DividendErrorCode.CapitalGainsTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CapitalGainsTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case DividendErrorCode.SolidarityTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SolidarityTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case DividendErrorCode.SolidarityTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SolidarityTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case DividendErrorCode.PriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSharePrice.Focus();

                        break;
                    }
                case DividendErrorCode.PriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSharePrice.Focus();

                        break;
                    }
                case DividendErrorCode.PriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSharePrice.Focus();

                        break;
                    }
                case DividendErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.DocumentDirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DocumentDirectoryDoesNotExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDocument.Focus();

                        break;
                    }
                case DividendErrorCode.DocumentFileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DocumentFileDoesNotExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
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
                case DividendErrorCode.DocumentBrowseFailed:
                    {
                        txtBoxDocument.Text = @"-";

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ChoseDocumentFailed", LanguageName);
                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
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
        public event EventHandler AddDividendEventHandler;
        public event EventHandler EditDividendEventHandler;
        public event EventHandler DeleteDividendEventHandler;
        public event EventHandler DocumentBrowseEventHandler;

        #endregion Event members

        #region Methods

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market value share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final value share object</param>
        /// <param name="logger">Logger</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        /// <param name="parsingFileName">File name of the parsing file which is given via document capture</param>
        public ViewDividendEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue,
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

            SaveFlag = false;

            #region Parsing backgroundworker

            _parsingBackgroundWorker.WorkerReportsProgress = true;
            _parsingBackgroundWorker.WorkerSupportsCancellation = true;

            _parsingBackgroundWorker.DoWork += DocumentParsing;
            _parsingBackgroundWorker.ProgressChanged += OnDocumentParsingProgressChanged;
            _parsingBackgroundWorker.RunWorkerCompleted += OnDocumentParsingRunWorkerCompleted;

            #endregion Parsing backgroundworker

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
                #region Language configuration

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

                #endregion Language configuration

                #region Unit configuration

                // Set dividend units to the edit boxes
                // HINT: Foreign currencies are set in the function "CbxBoxAddDividendForeignCurrency_SelectedIndexChanged"
                lblDividendRateUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblPayoutUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxAtSourceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCapitalGainsTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSolidarityTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPayoutAfterTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblYieldUnit.Text = ShareObject.PercentageUnit;
                lblPriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.button_add_24;
                btnDelete.Image = Resources.button_recycle_bin_24;
                btnReset.Image = Resources.button_reset_24;
                btnCancel.Image = Resources.button_back_24;

                #endregion Image configuration

                // Set currency to ComboBox
                foreach (var temp in Helper.ListNameCultureInfoCurrencySymbol)
                {
                    cbxBoxDividendFCUnit.Items.Add($"{temp.Key} / {temp.Value.CurrencySymbol}");
                }

                CheckForeignCurrencyCalculationShouldBeDone();

                // Chose USD item
                var iIndex = cbxBoxDividendFCUnit.FindString("en-US");
                cbxBoxDividendFCUnit.SelectedIndex = iIndex;

                OnShowDividends();
            }
            catch (Exception ex)
            {

                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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
            // If a parsing file name is given the form directly starts with the document parsing
            if (ParsingFileName != null)
            {
                _parsingStartAllow = true;
                txtBoxDocument.Text = ParsingFileName;
            }

            txtBoxDividendRate.Focus();
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a dividend has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareDividendEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date.
        /// </summary>
        private void ResetInputValues()
        {
            // Reset state pictures
            picBoxDateParseState.Image = Resources.empty_arrow;
            picBoxTimeParseState.Image = Resources.empty_arrow;
            picBoxVolumeParserState.Image = Resources.empty_arrow;
            picBoxExchangeRateBoxParseState.Image = Resources.empty_arrow;
            picBoxDividendRateParserState.Image = Resources.empty_arrow;
            picBoxTaxAtSourceParserState.Image = Resources.empty_arrow;
            picBoxCapitalGainParserState.Image = Resources.empty_arrow;
            picBoxSolidarityParserState.Image = Resources.empty_arrow;
            picBoxSharePriceParserState.Image = Resources.empty_arrow;

            // Reset date time picker
            dateTimePickerDate.Value = DateTime.Now;
            dateTimePickerDate.Enabled = true;
            dateTimePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePickerTime.Enabled = true;

            // Reset text boxes
            chkBoxEnableFC.CheckState = CheckState.Unchecked;
            chkBoxEnableFC.Enabled = true;
            txtBoxExchangeRatio.Text = string.Empty;
            txtBoxExchangeRatio.Enabled = true;
            txtBoxVolume.Text = string.Empty;
            txtBoxVolume.Enabled = true;
            txtBoxDividendRate.Text = string.Empty;
            txtBoxDividendRate.Enabled = true;
            txtBoxPayout.Text = string.Empty;
            txtBoxPayoutFC.Text = string.Empty;
            txtBoxTaxAtSource.Text = string.Empty;
            txtBoxTaxAtSource.Enabled = true;
            txtBoxCapitalGainsTax.Text = string.Empty;
            txtBoxCapitalGainsTax.Enabled = true;
            txtBoxSolidarityTax.Text = string.Empty;
            txtBoxSolidarityTax.Enabled = true;
            txtBoxTax.Text = string.Empty;
            txtBoxPayoutAfterTax.Text = string.Empty;
            txtBoxYield.Text = string.Empty;
            txtBoxSharePrice.Text = string.Empty;
            txtBoxDocument.Text = string.Empty;

            toolStripStatusLabelMessageDividendEdit.Text = string.Empty;
            toolStripStatusLabelMessageDividendDocumentParsing.Text = string.Empty;
            toolStripProgressBarDividendDocumentParsing.Visible = false;

            // Enable button(s)
            btnAddSave.Text = 
                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
            btnAddSave.Image = Resources.button_add_24;

            // Disable button(s)
            btnDelete.Enabled = false;

            // Rename group box
            grpBoxAddDividend.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

            // Deselect rows
            OnDeselectRowsOfDataGridViews(null);

            // Reset stored DataGridView instance
            SelectedDataGridView = null;

            // Check if any volume is present
            if (ShareObjectFinalValue.Volume > 0)
            {
                // Set current share value
                txtBoxVolume.Text = ShareObjectFinalValue.VolumeAsStr;
            }

            if (tabCtrlDividends.TabPages.Count > 0)
                tabCtrlDividends.SelectTab(0);

            txtBoxDividendRate.Focus();

            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Form

        #region CheckBoxes

        /// <summary>
        /// This function checks the CheckBox enable foreign currency
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">EventArgs</param>
        private void OnChkBoxAddDividendForeignCurrency_CheckedChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnableFC"));

            CheckForeignCurrencyCalculationShouldBeDone();
        }

        /// <summary>
        /// This function stores the check box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnChkBoxEnableFC_Enter(object sender, EventArgs e)
        {
            _focusedControl = chkBoxEnableFC;
        }

        #endregion CheckBoxes

        #region Combo box foreign currency

        /// <summary>
        /// This function changes the currency unit when the currency
        /// has been changed and changes the currency unit to the taxes
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">EventArgs</param>
        private void CbxBoxAddDividendForeignCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPayoutFCUnit.Text = ((ComboBox)sender).SelectedItem.ToString().Split('/')[1].Trim();
            lblDividendRateUnit.Text = lblPayoutFCUnit.Text;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CultureInfoFC"));
        }

        /// <summary>
        /// This function stores the combo box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnCbxBoxDividendFCUnit_Enter(object sender, EventArgs e)
        {
            _focusedControl = cbxBoxDividendFCUnit;
        }

        #endregion Combo box foreign currency

        #region Date / Time

        /// <summary>
        /// This function updates the model if the date has been changed
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
        /// This function updates the model if the time has been changed
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

        #endregion Date / Time

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the exchange ratio has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void TxtBoxAddDividendExchangeRatio_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExchangeRatio"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxExchangeRatio_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxExchangeRatio_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxExchangeRatio;
        }

        /// <summary>
        /// This function update the model if the rate has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddDividendRate_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rate"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxRate_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxRate_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxDividendRate;
        }

        /// <summary>
        /// This function updates the model if volume has been changed
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
        /// This function updates the model if tax at source value has been changed
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
        private void OnTxtBoxTaxAtSource_Leave(object sender, EventArgs e)
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
        /// This function updates the model if capital gains tax value has been changed
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
        private void OnTxtBoxCapitalGainsTax_Leave(object sender, EventArgs e)
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
        /// This function updates the model if solidarity tax value has been changed
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
        private void OnTxtBoxSolidarityTax_Leave(object sender, EventArgs e)
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
        /// This function updates the model if price value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxSharePrice;
        }

        /// <summary>
        /// This function updates the model if document value has been changed
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

                _parsingStartAllow = true;

                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
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
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendDocumentParsing,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingErrors/ParsingFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application,
                    ex);

                toolStripProgressBarDividendDocumentParsing.Visible = false;
                grpBoxAddDividend.Enabled = true;
                grpBoxDividends.Enabled = true;

                DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                _parsingThreadFinished = true;
                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);
            }
        }

        #endregion TextBoxes

        #region Buttons

        /// <summary>
        /// This function adds a new dividend entry to the share object
        /// or edit a dividend entry
        /// It also checks if an entry already exists for the given date
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void BtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelMessageDividendEdit.Text = string.Empty;
                toolStripStatusLabelMessageDividendDocumentParsing.Text = string.Empty;
                toolStripProgressBarDividendDocumentParsing.Visible = false;

                // Disable controls
                Enabled = false;

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    UpdateDividend = false;

                    AddDividendEventHandler?.Invoke(this, null);
                }
                else
                {
                    UpdateDividend = true;

                    EditDividendEventHandler?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/AddFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
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
                toolStripStatusLabelMessageDividendEdit.Text = string.Empty;
                toolStripStatusLabelMessageDividendDocumentParsing.Text = string.Empty;
                toolStripProgressBarDividendDocumentParsing.Visible = false;

                var strCaption = Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", LanguageName)[
                    (int) EOwnMessageBoxInfoType.Info];
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/DividendDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult != DialogResult.OK) return;

                // Set flag to save the share object.
                SaveFlag = true;

                // Check if a row is selected
                if (SelectedDataGridView != null && SelectedDataGridView.SelectedRows.Count == 1)
                {
                    DeleteDividendEventHandler?.Invoke(this, null);
                }

                // Reset values
                ResetInputValues();

                // Enable button(s)
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.button_add_24;

                // Rename group box
                grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Refresh the dividend list
                OnShowDividends();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeleteFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
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
                // Reset values
                ResetInputValues();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CancelFailure", LanguageName),
                    Language, LanguageName,
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
        private void BtnDividendDocumentBrowse_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMessageDividendEdit.Text = string.Empty;
            toolStripStatusLabelMessageDividendDocumentParsing.Text = string.Empty;

            _parsingStartAllow = true;

            DocumentBrowseEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Buttons

        #region Group box overview

        private void OnGrpBoxDividends_MouseLeave(object sender, EventArgs e)
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
        /// This function paints the dividend list of the share
        /// </summary>
        private void OnShowDividends()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= DataGridViewDividendsOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewDividendsOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= DataGridViewDividendsOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewDividendsOfAYear_MouseEnter;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewDividends_DataBindingComplete;
                    }
                    tabPage.Controls.Clear();
                    tabCtrlDividends.TabPages.Remove(tabPage);
                }

                tabCtrlDividends.TabPages.Clear();

                // Enable controls
                Enabled = true;

                #region Add page

                // Create TabPage for the dividends of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview", LanguageName)
                           + @" (" + ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesAsStrUnit +
                           @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Check if dividends exists
                if (ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues().Count <= 0) return;

                // Reverse list so the latest is a top of the data grid view
                var reversDataSourceOverview = ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues();
                reversDataSourceOverview.Reverse();

                var bindingSourceOverview = new BindingSource
                {
                    DataSource = reversDataSourceOverview
                };

                // Create DataGridView
                var dataGridViewDividendsOverviewOfAYears = new DataGridView
                {
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with dividend values to the DataGridView
                    DataSource = bindingSourceOverview,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewDividendsOverviewOfAYears.DataBindingComplete +=
                    OnDataGridViewDividends_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewDividendsOverviewOfAYears.MouseEnter +=
                    OnDataGridViewDividendsOfYears_MouseEnter;
                // Set row select event
                dataGridViewDividendsOverviewOfAYears.SelectionChanged +=
                    DataGridViewDividendsOfYears_SelectionChanged;

                #endregion Events

                #region Style

                // Advanced configuration DataGridView dividends
                dataGridViewDividendsOverviewOfAYears.EnableHeadersVisualStyles = false;
                // Column header styling
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle.BackColor =
                    DataGridViewHelper.DataGridViewHeaderColors;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                    DataGridViewHelper.DataGridViewHeaderColors;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersHeight = 25;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                // Column styling
                dataGridViewDividendsOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // Row styling
                dataGridViewDividendsOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewDividendsOverviewOfAYears.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewDividendsOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewDividendsOverviewOfAYears.MultiSelect = false;
                dataGridViewDividendsOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // Cell styling
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                dataGridViewDividendsOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // Allow styling
                dataGridViewDividendsOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewDividendsOverviewOfAYears.AllowUserToResizeRows = false;
                dataGridViewDividendsOverviewOfAYears.AllowUserToAddRows = false;
                dataGridViewDividendsOverviewOfAYears.AllowUserToDeleteRows = false;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewDividendsOverviewOfAYears);
                dataGridViewDividendsOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlDividends.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlDividends;

                #endregion Control add

                // Check if dividend pays exists
                if (ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Count <= 0) return;

                // Loop through the years of the dividend pays
                foreach (
                    var keyName in ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Keys.Reverse()
                    )
                {
                    #region Add page

                    // Create TabPage
                    var newTabPage = new TabPage
                    {
                        // Set TabPage name
                        Name = keyName,

                        // Set TabPage caption
                        Text = keyName + @" (" +
                               ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                                   .DividendValueYearAsStrUnit
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Reverse list so the latest is a top of the data grid view
                    var reversDataSource =
                        ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                            .DividendListYear;
                    reversDataSource.Reverse();

                    // Create Binding source for the dividend data
                    var bindingSource = new BindingSource
                    {
                        DataSource = reversDataSource

                    };

                    // Create DataGridView
                    var dataGridViewDividendsOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with dividend values to the DataGridView
                        DataSource = bindingSource,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewDividendsOfAYear.DataBindingComplete += OnDataGridViewDividends_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewDividendsOfAYear.MouseEnter += OnDataGridViewDividendsOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewDividendsOfAYear.SelectionChanged += DataGridViewDividendsOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewDividendsOfAYear.CellContentDoubleClick += DataGridViewDividendsOfAYear_CellContentClick;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView dividends
                    dataGridViewDividendsOfAYear.EnableHeadersVisualStyles = false;
                    // Column header styling
                    dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle.BackColor =
                        DataGridViewHelper.DataGridViewHeaderColors;
                    dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        DataGridViewHelper.DataGridViewHeaderColors;
                    dataGridViewDividendsOfAYear.ColumnHeadersHeight = 25;
                    dataGridViewDividendsOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    // Column styling
                    dataGridViewDividendsOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Row styling
                    dataGridViewDividendsOfAYear.RowHeadersVisible = false;
                    dataGridViewDividendsOfAYear.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewDividendsOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridViewDividendsOfAYear.MultiSelect = false;
                    dataGridViewDividendsOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    // Cell styling
                    dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                    dataGridViewDividendsOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    dataGridViewDividendsOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    // Allow styling
                    dataGridViewDividendsOfAYear.AllowUserToResizeColumns = false;
                    dataGridViewDividendsOfAYear.AllowUserToResizeRows = false;
                    dataGridViewDividendsOfAYear.AllowUserToAddRows = false;
                    dataGridViewDividendsOfAYear.AllowUserToDeleteRows = false;

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewDividendsOfAYear);
                    dataGridViewDividendsOfAYear.Parent = newTabPage;
                    tabCtrlDividends.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlDividends;

                    #endregion Control add
                }

                tabCtrlDividends.TabPages[0].Select();

                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewDividends_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (var i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    // Disable sorting of the columns ( remove sort arrow )
                    ((DataGridView)sender).Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    switch (i)
                    {
                        case 0:
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Date",
                                        LanguageName);
                            }
                            break;
                        case 1:
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Dividend",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Date",
                                        LanguageName);
                            }
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Dividend",
                                    LanguageName);
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Volume",
                                    LanguageName);
                            break;
                        case 4:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_TaxSum",
                                    LanguageName);
                            break;
                        case 5:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_PayoutWithTaxes",
                                    LanguageName);
                            break;
                        case 6:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Yield",
                                    LanguageName);
                            break;
                        case 7:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Document",
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RenameColHeaderFailed", LanguageName),
                    Language, LanguageName,
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
                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeselectFailed", LanguageName),
                    Language, LanguageName,
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
        private void OnTabCtrlDividends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlDividends.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabCtrlDividends.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                {
                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                    {
                        if(view.Rows[0].Cells.Count > 0)
                            view.Rows[0].Selected = true;
                    }

                    view.Focus();

                    if (view.Name == @"Overview")
                        ResetInputValues();
                }
            }
        }

        /// <summary>
        /// This function sets the focus back to the last focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlDividends_MouseLeave(object sender, EventArgs e)
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
        private void OnTabCtrlDividends_KeyDown(object sender, KeyEventArgs e)
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
        private void OnTabCtrlDividends_KeyPress(object sender, KeyPressEventArgs e)
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

        #region Dividends of years

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
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                {
                    if (curItem[0].Cells[1].Value != null && 
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()
                        ) continue;

                    tabCtrlDividends.SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
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
        private static void OnDataGridViewDividendsOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        #endregion Dividends of years

        #region Dividends of a year

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
                        OnDeselectRowsOfDataGridViews((DataGridView) sender);
                }

                if (((DataGridView) sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    var curItem = ((DataGridView) sender).SelectedRows;

                    // Set selected date
                    if (curItem[0].Cells[1].Value != null)
                        SelectedDate = curItem[0].Cells[1].Value.ToString();
                    else
                        return;

                    // Get list of dividends of a year
                    DateTime.TryParse(SelectedDate, out var dateTime);
                    var dividendListYear = ShareObjectFinalValue.AllDividendEntries
                        .AllDividendsOfTheShareDictionary[dateTime.Year.ToString()]
                        .DividendListYear;

                    var index = ((DataGridView)sender).SelectedRows[0].Index;

                    // Set selected Guid
                    SelectedGuid = dividendListYear[index].Guid;

                    // Get BrokerageObject of the selected DataGridView row
                    var selectedDividendObject = dividendListYear[index];

                    // Set dividend values
                    if (selectedDividendObject != null)
                    {
                        if (selectedDividendObject.EnableFc == CheckState.Checked)
                        {
                            LoadGridSelectionFlag = true;

                            // Set CheckBox for the exchange ratio
                            chkBoxEnableFC.CheckState = CheckState.Checked;

                            // Set foreign currency values
                            txtBoxExchangeRatio.Text = selectedDividendObject.ExchangeRatioAsStr;

                            // Find correct currency
                            cbxBoxDividendFCUnit.SelectedIndex = cbxBoxDividendFCUnit.FindString(selectedDividendObject.CultureInfoFc.Name);

                            LoadGridSelectionFlag = false;
                        }
                        else
                        {
                            txtBoxDividendRate.Text = selectedDividendObject.DividendAsStr;

                            // Chose USD item
                            var iIndex = cbxBoxDividendFCUnit.FindString("en-US");
                            cbxBoxDividendFCUnit.SelectedIndex = iIndex;
                        }

                        dateTimePickerDate.Value = Convert.ToDateTime(selectedDividendObject.Date);
                        dateTimePickerTime.Value = Convert.ToDateTime(selectedDividendObject.Date);
                        txtBoxDividendRate.Text = selectedDividendObject.DividendAsStr;
                        txtBoxVolume.Text = selectedDividendObject.VolumeAsStr;
                        txtBoxTaxAtSource.Text = selectedDividendObject.TaxAtSourceAsStr;
                        txtBoxCapitalGainsTax.Text = selectedDividendObject.CapitalGainsTaxAsStr;
                        txtBoxSolidarityTax.Text = selectedDividendObject.SolidarityTaxAsStr;
                        txtBoxYield.Text = selectedDividendObject.YieldAsStr;
                        txtBoxSharePrice.Text = selectedDividendObject.PriceAtPaydayAsStr;
                        txtBoxDocument.Text = selectedDividendObject.DocumentAsStr;
                    }
                    else
                    {
                        dateTimePickerDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        dateTimePickerTime.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        txtBoxPayout.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxDividendRate.Text = curItem[0].Cells[2].Value.ToString();
                        txtBoxYield.Text = curItem[0].Cells[3].Value.ToString();
                        txtBoxSharePrice.Text = curItem[0].Cells[4].Value.ToString();
                        txtBoxVolume.Text = curItem[0].Cells[5].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[6].Value.ToString();
                    }

                    // Rename button
                    btnAddSave.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.button_pencil_24;

                    // Rename group box
                    grpBoxAddDividend.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;

                    // Format the input value
                    FormatInputValuesEventHandler?.Invoke(this, new EventArgs());

                    // Enabled delete button
                    btnDelete.Enabled = true;
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Rename group box
                    grpBoxAddDividend.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // TODO correct
                    // Enabled TextBox(es)
                    dateTimePickerDate.Enabled = true;
                    dateTimePickerTime.Enabled = true;
                    txtBoxDividendRate.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxTaxAtSource.Enabled = true;
                    txtBoxCapitalGainsTax.Enabled = true;
                    txtBoxSolidarityTax.Enabled = true;
                    txtBoxSharePrice.Enabled = true;

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
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
        private static void OnDataGridViewDividendsOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function opens the dividend document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void DataGridViewDividendsOfAYear_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                // Get Guid of the selected dividend item
                var strGuid = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value == null) return;

                // Get doc from the dividend with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllDividendEntries.GetAllDividendsOfTheShare())
                {
                    // Check if the dividend date and time is the same as the date and time of the clicked buy item
                    if (temp.Guid != strGuid) continue;

                    // Check if the file still exists
                    if (File.Exists(temp.DocumentAsStr))
                        // Open the file
                        Process.Start(temp.DocumentAsStr);
                    else
                    {
                        var strCaption =
                            Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", LanguageName)[
                                (int) EOwnMessageBoxInfoType.Error];
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
                            // Remove dividend object and add it with no document
                            if (ShareObjectFinalValue.RemoveDividend(temp.Guid, temp.Date) &&
                                ShareObjectFinalValue.AddDividend(temp.CultureInfoFc, temp.EnableFc, temp.ExchangeRatio, temp.Guid, temp.Date, temp.Dividend, temp.Volume,
                                    temp.TaxAtSource, temp.CapitalGainsTax, temp.SolidarityTax, temp.PriceAtPayday))
                            {
                                // Set flag to save the share object.
                                SaveFlag = true;

                                OnShowDividends();

                                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/StateMessages/EditSuccess", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                            else
                            {
                                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/Errors/EditFailed", LanguageName),
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageDividendEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Dividends of a year

        #endregion Data grid view

        /// <summary>
        /// This function check is a foreign current calculation should be done.
        /// The function shows or hides, resize and reposition controls
        /// </summary>
        private void CheckForeignCurrencyCalculationShouldBeDone()
        {
            if (chkBoxEnableFC.CheckState == CheckState.Checked)
            {
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
                    txtBoxDividendRate.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPayoutFC.Text = @"";
                    txtBoxTaxAtSource.Text = @"";
                    txtBoxCapitalGainsTax.Text = @"";
                    txtBoxSolidarityTax.Text = @"";
                    txtBoxTax.Text = @"";
                    txtBoxPayoutAfterTax.Text = @"";
                    txtBoxSharePrice.Text = @"";
                    txtBoxDocument.Text = @"";

                    // Reset status message
                    toolStripStatusLabelMessageDividendEdit.Text = @"";
                }

                txtBoxExchangeRatio.Focus();
            }
            else
            {
                // Disable controls
                txtBoxExchangeRatio.Enabled = false;
                txtBoxExchangeRatio.ReadOnly = true;
                cbxBoxDividendFCUnit.Enabled = false;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxExchangeRatio.Text = @"";
                    txtBoxVolume.Text = @"";
                    txtBoxDividendRate.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPayoutFC.Text = @"";
                    txtBoxTaxAtSource.Text = @"";
                    txtBoxCapitalGainsTax.Text = @"";
                    txtBoxSolidarityTax.Text = @"";
                    txtBoxTax.Text = @"";
                    txtBoxPayoutAfterTax.Text = @"";
                    txtBoxSharePrice.Text = @"";
                    txtBoxDocument.Text = @"";

                    // Reset status message
                    toolStripStatusLabelMessageDividendEdit.Text = @"";
                }

                txtBoxDividendRate.Focus();
            }

            // Check if any volume is present
            if (ShareObjectFinalValue.Volume > 0)
            {
                // Set current share value
                txtBoxVolume.Text = ShareObjectFinalValue.VolumeAsStr;
            }
        }

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
                _dividendIdentifierFound = false;
                _documentTypNotImplemented = false;
                _documentValuesRunning = false;

                ParsingText = string.Empty;

                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingStarted);

                DocumentType = DocumentParsingConfiguration.DocumentTypes.DividendDocument;
                DocumentTypeParser = null;
                DictionaryParsingResult = null;

                Helper.RunProcess($"{Helper.PdfConverterApplication}", $"-simple \"{txtBoxDocument.Text}\" {Helper.ParsingDocumentFileName}");

                // This text is added only once to the file.
                if (File.Exists(Helper.ParsingDocumentFileName))
                {
                    ParsingText = File.ReadAllText(Helper.ParsingDocumentFileName, Encoding.Default);

                    DocumentTypeParsing();
                }

                while (!_parsingThreadFinished)
                {
                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException ex)
            {
                Helper.ShowExceptionMessage(ex);

                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);
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
                    DocumentTypeParser.ParsingValues = new ParsingValues(ParsingText,
                        DocumentParsingConfiguration.BankRegexList[_bankCounter].BankEncodingType,
                        DocumentParsingConfiguration.BankRegexList[_bankCounter].BankRegexList
                    );

                    // Check if the Parser is in idle mode
                    if (DocumentTypeParser != null && DocumentTypeParser.ParserInfoState.State == ParserState.Idle)
                    {
                        DocumentTypeParser.OnParserUpdate += DocumentTypeParser_UpdateGUI;

                        // Reset flags
                        _bankIdentifierFound = false;
                        _dividendIdentifierFound = false;

                        // Start document parsing
                        DocumentTypeParser.StartParsing();
                    }
                    else
                    {
                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                        _parsingThreadFinished = true;
                        _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingFailed);
                    }
                }
                else
                {
                    _parsingThreadFinished = true;
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingParsingDocumentError);
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                _parsingThreadFinished = true;
                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingFailed);
            }
        }

        /// <summary>
        /// This event handler updates the progress while checking the document type
        /// </summary>
        /// <param name="sender">BackGroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void DocumentTypeParser_UpdateGUI(object sender, OnParserUpdateEventArgs e)
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
                        //Console.WriteLine(@"Percentage: {0}", e.ParserInfoState.Percentage);
                        switch (e.ParserInfoState.LastErrorCode)
                        {
                            case ParserErrorCodes.Finished:
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
                            case ParserErrorCodes.SearchFinished:
                                {
                                    break;
                                }
                            case ParserErrorCodes.SearchRunning:
                                {
                                    break;
                                }
                            case ParserErrorCodes.SearchStarted:
                                {
                                    break;
                                }
                            case ParserErrorCodes.ContentLoadFinished:
                                {
                                    break;
                                }
                            case ParserErrorCodes.ContentLoadStarted:
                                {
                                    break;
                                }
                            case ParserErrorCodes.Started:
                                {
                                    break;
                                }
                            case ParserErrorCodes.Starting:
                                {
                                    break;
                                }
                            case ParserErrorCodes.NoError:
                                {
                                    break;
                                }
                            case ParserErrorCodes.StartFailed:
                                {
                                    break;
                                }
                            case ParserErrorCodes.BusyFailed:
                                {
                                    break;
                                }
                            case ParserErrorCodes.InvalidWebSiteGiven:
                                {
                                    break;
                                }
                            case ParserErrorCodes.NoRegexListGiven:
                                {
                                    break;
                                }
                            case ParserErrorCodes.NoWebContentLoaded:
                                {
                                    break;
                                }
                            case ParserErrorCodes.ParsingFailed:
                                {
                                    break;
                                }
                            case ParserErrorCodes.CancelThread:
                                {
                                    break;
                                }
                            case ParserErrorCodes.WebExceptionOccured:
                                {
                                    break;
                                }
                            case ParserErrorCodes.ExceptionOccured:
                                {
                                    break;
                                }
                        }

                        if (DocumentTypeParser.ParserErrorCode > 0)
                            Thread.Sleep(100);

                        // Check if a error occurred or the process has been finished
                        if (e.ParserInfoState.LastErrorCode < 0 || e.ParserInfoState.LastErrorCode == ParserErrorCodes.Finished)
                        {
                            if (e.ParserInfoState.LastErrorCode < 0)
                            {
                                // Set fail message
                                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingFailed);
                            }
                            else
                            {
                                //Check if the correct bank identifier and document identifier may have been found so search for the document values
                                if (e.ParserInfoState.SearchResult != null &&
                                    (
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName) ||
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .DividendIdentifierTagName))
                                )
                                {
                                    // Check if the bank identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName))
                                        _bankIdentifierFound = true;
                                    // Check if the dividend identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .DividendIdentifierTagName))
                                        _dividendIdentifierFound = true;

                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName) &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .DividendIdentifierTagName))
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int)ParsingErrorCode.ParsingIdentifierValuesFound);

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
                                        DocumentParsingConfiguration
                                            .BankRegexList[_bankCounter].BankRegexList
                                    );
                                    DocumentTypeParser.StartParsing();
                                }
                                else
                                {
                                    if (_bankIdentifierFound == false)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int)ParsingErrorCode.ParsingBankIdentifierFailed);

                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else if (_dividendIdentifierFound == false)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int)ParsingErrorCode.ParsingDocumentTypeIdentifierFailed);

                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else if (_documentTypNotImplemented)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int)ParsingErrorCode.ParsingDocumentNotImplemented);

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
                    catch (Exception ex)
                    {
                        Helper.ShowExceptionMessage(ex);

                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                        _parsingThreadFinished = true;
                        _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                _parsingThreadFinished = true;
                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);
            }
        }

        private void OnDocumentParsingProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case (int) ParsingErrorCode.ParsingStarted:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingStateMessages/ParsingStarted",
                            LanguageName);

                    toolStripProgressBarDividendDocumentParsing.Visible = true;
                    grpBoxAddDividend.Enabled = false;
                    grpBoxDividends.Enabled = false;
                    break;
                }
                case (int)ParsingErrorCode.ParsingParsingDocumentError:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingErrors/ParsingParsingDocumentError",
                            LanguageName);

                    toolStripProgressBarDividendDocumentParsing.Visible = false;
                    grpBoxAddDividend.Enabled = true;
                    grpBoxDividends.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingFailed:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingErrors/ParsingFailed",
                            LanguageName);

                    toolStripProgressBarDividendDocumentParsing.Visible = false;
                    grpBoxAddDividend.Enabled = true;
                    grpBoxDividends.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingDocumentNotImplemented:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormDividend/ParsingErrors/ParsingDocumentNotImplemented",
                            LanguageName);

                    toolStripProgressBarDividendDocumentParsing.Visible = false;
                    grpBoxAddDividend.Enabled = true;
                    grpBoxDividends.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingBankIdentifierFailed:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormDividend/ParsingErrors/ParsingBankIdentifierFailed",
                            LanguageName);

                    toolStripProgressBarDividendDocumentParsing.Visible = false;
                    grpBoxAddDividend.Enabled = true;
                    grpBoxDividends.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingDocumentTypeIdentifierFailed:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormDividend/ParsingErrors/ParsingDocumentTypeIdentifierFailed",
                            LanguageName);

                    toolStripProgressBarDividendDocumentParsing.Visible = false;
                    grpBoxAddDividend.Enabled = true;
                    grpBoxDividends.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingDocumentFailed:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingErrors/ParsingDocumentFailed",
                            LanguageName);

                    toolStripProgressBarDividendDocumentParsing.Visible = false;
                    grpBoxAddDividend.Enabled = true;
                    grpBoxDividends.Enabled = true;
                    break;
                }
                case (int) ParsingErrorCode.ParsingIdentifierValuesFound:
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormDividend/ParsingErrors/ParsingIdentifierValuesFound",
                            LanguageName);
                    break;
                }
            }
        }

        private void OnDocumentParsingRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (DictionaryParsingResult != null)
            {
                // Check if the WKN has been found and if the WKN is the right one
                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                    .DocumentTypeDividendWkn) || DictionaryParsingResult[DocumentParsingConfiguration
                        .DocumentTypeDividendWkn][0] != ShareObjectFinalValue.Wkn)
                {
                    toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageDividendDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingErrors/ParsingWrongWkn",
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
                            case DocumentParsingConfiguration.DocumentTypeDividendExchangeRate:
                                string strDocument = txtBoxDocument.Text;
                                chkBoxEnableFC.CheckState = CheckState.Checked;                                
                                picBoxExchangeRateBoxParseState.Image = Resources.search_ok_24;
                                txtBoxExchangeRatio.Text = resultEntry.Value[0].Trim();
                                txtBoxDocument.Text = strDocument;
                                break;
                            case DocumentParsingConfiguration.DocumentTypeDividendDate:
                                picBoxDateParseState.Image = Resources.search_ok_24;
                                dateTimePickerDate.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeDividendTime:
                                picBoxTimeParseState.Image = Resources.search_ok_24;
                                dateTimePickerTime.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeDividendVolume:
                                picBoxVolumeParserState.Image = Resources.search_ok_24;
                                txtBoxVolume.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeDividendDividendRate:
                                picBoxDividendRateParserState.Image = Resources.search_ok_24;
                                txtBoxDividendRate.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeDividendTaxAtSource:
                                picBoxTaxAtSourceParserState.Image = Resources.search_ok_24;
                                txtBoxTaxAtSource.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeDividendCapitalGainTax:
                                picBoxCapitalGainParserState.Image = Resources.search_ok_24;
                                txtBoxCapitalGainsTax.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeDividendSolidarityTax:
                                picBoxSolidarityParserState.Image = Resources.search_ok_24;
                                txtBoxSolidarityTax.Text = resultEntry.Value[0].Trim();
                                break;
                        }
                    }

                    #endregion Check which values are found

                    #region Not found values

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendDate) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendDate) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendDate].Count == 0
                    )
                    {
                        picBoxDateParseState.Image = Resources.search_failed_24;
                        dateTimePickerDate.Value = DateTime.Now;
                        dateTimePickerTime.Value =
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        picBoxDateParseState.Image = Resources.search_failed_24;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendTime) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendTime) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendTime].Count == 0
                    )
                    {
                        dateTimePickerTime.Value =
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        picBoxTimeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendExchangeRate) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendExchangeRate) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendExchangeRate].Count == 0
                    )
                    {
                        picBoxExchangeRateBoxParseState.Image = Resources.search_ok_24;
                        chkBoxEnableFC.CheckState = CheckState.Unchecked;
                        txtBoxExchangeRatio.Text = string.Empty;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendVolume) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendVolume) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendVolume].Count == 0
                    )
                    {
                        picBoxVolumeParserState.Image = Resources.search_failed_24;
                        txtBoxVolume.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendDividendRate) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendDividendRate) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendDividendRate].Count == 0
                    )
                    {
                        picBoxDividendRateParserState.Image = Resources.search_failed_24;
                        txtBoxDividendRate.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendTaxAtSource) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendTaxAtSource) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendTaxAtSource].Count == 0
                    )
                    {
                        picBoxTaxAtSourceParserState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendCapitalGainTax) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendCapitalGainTax) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendCapitalGainTax].Count == 0
                    )
                    {
                        picBoxCapitalGainParserState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendSolidarityTax) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeDividendSolidarityTax) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendSolidarityTax].Count == 0
                    )
                    {
                        picBoxSolidarityParserState.Image = Resources.search_info_24;
                    }

                    #endregion Not found values

                    if (!_parsingResult)
                    {
                        toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageDividendDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingErrors/ParsingFailed",
                                LanguageName);
                    }
                    else
                    {
                        toolStripStatusLabelMessageDividendDocumentParsing.ForeColor = Color.Black;
                        toolStripStatusLabelMessageDividendDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/ParsingStateMessages/ParsingDocumentSuccessful",
                                LanguageName);
                    }
                }
            }

            toolStripProgressBarDividendDocumentParsing.Visible = false;
            grpBoxAddDividend.Enabled = true;
            grpBoxDividends.Enabled = true;
        }

        private void ResetValues()
        {
            // Reset state pictures
            // TODO
            //picBoxDateParseState.Image = Resources.empty_arrow;
            //picBoxTimeParseState.Image = Resources.empty_arrow;
            //picBoxOrderNumberParseState.Image = Resources.empty_arrow;
            //picBoxVolumeParseState.Image = Resources.empty_arrow;
            //picBoxPriceParseState.Image = Resources.empty_arrow;
            //picBoxProvisionParseState.Image = Resources.empty_arrow;
            //picBoxBrokerFeeParseState.Image = Resources.empty_arrow;
            //picBoxTraderPlaceFeeParseState.Image = Resources.empty_arrow;
            //picBoxReductionParseState.Image = Resources.empty_arrow;

            //// Reset textboxes
            //dateTimePickerDate.Value = DateTime.Now;
            //dateTimePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            //txtBoxOrderNumber.Text = string.Empty;
            //txtBoxVolume.Text = string.Empty;
            //txtBoxSharePrice.Text = string.Empty;
            //txtBoxProvision.Text = string.Empty;
            //txtBoxBrokerFee.Text = string.Empty;
            //txtBoxTraderPlaceFee.Text = string.Empty;
            //txtBoxReduction.Text = string.Empty;

            // Reset status strip
            toolStripStatusLabelMessageDividendEdit.Text = string.Empty;
            toolStripStatusLabelMessageDividendDocumentParsing.Text = string.Empty;
            toolStripProgressBarDividendDocumentParsing.Visible = false;
        }

        #endregion Parsing

        #endregion Methods
    }
}
