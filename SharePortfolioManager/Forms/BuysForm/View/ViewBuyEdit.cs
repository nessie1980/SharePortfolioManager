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
//#define DEBUG_BUY_EDIT_VIEW

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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SharePortfolioManager.BuysForm.View
{
    // Error codes of the BuyEdit
    public enum BuyErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        DeleteFailedUnerasable,
        InputValuesInvalid,
        OrderNumberEmpty,
        OrderNumberExists,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        SharePriceEmpty,
        SharePriceWrongFormat,
        SharePriceWrongValue,
        ProvisionWrongFormat,
        ProvisionWrongValue,
        BrokerFeeWrongFormat,
        BrokerFeeWrongValue,
        TraderPlaceFeeWrongValue,
        TraderPlaceFeeWrongFormat,
        ReductionWrongValue,
        ReductionWrongFormat,
        BrokerageEmpty,
        BrokerageWrongValue,
        BrokerageWrongFormat,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExists,
        DocumentFileDoesNotExists
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

    /// <inheritdoc />
    /// <summary>
    /// Interface of the BuyEdit view
    /// </summary>
    public interface IViewBuyEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler AddBuyEventHandler;
        event EventHandler EditBuyEventHandler;
        event EventHandler DeleteBuyEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        BuyErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool UpdateBuy { get; set; }

        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string OrderNumber { get; set; }
        string Volume { get; set; }
        string VolumeSold { get; set; }
        string Price { get; set; }
        string Reduction { get; set; }
        string Provision { get; set; }
        string BrokerFee { get; set; }
        string TraderPlaceFee { get; set; }
        string Brokerage { get; set; }
        string BuyValue { get; set; }
        string BuyValueBrokerageReduction { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
        void DocumentBrowseFinish();
    }

    public partial class ViewBuyEdit : Form, IViewBuyEdit
    {
        #region Fields

        /// <summary>
        /// Stores the Guid of a selected buy row
        /// </summary>
        private string _selectedGuid;

        /// <summary>
        /// Stores the date of a selected buy row
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
        private bool _buyIdentifierFound;

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

        public bool UpdateBuyFlag { get; set; }

        public bool SaveFlag { get; set; }

        #endregion Flags

        public DataGridView SelectedDataGridView { get; internal set; }

        #region Parsing

        public DocumentParsingConfiguration.DocumentTypes DocumentType { get; internal set; }

        public Parser.Parser DocumentTypeParser;

        public string ParsingText { get; internal set; }

        public Dictionary<string, List<string>> DictionaryParsingResult;

        #endregion Parsing

        #endregion Properties

        #region IView members

        public bool UpdateBuy
        {
            get => UpdateBuyFlag;
            set
            {
                UpdateBuyFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateBuy"));
            }
        }

        public BuyErrorCode ErrorCode { get; set; }

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

        public string VolumeSold
        {
            get => txtBoxVolumeSold.Text;
            set
            {
                if (txtBoxVolumeSold.Text == value)
                    return;
                txtBoxVolumeSold.Text = value;
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

        public string BuyValue
        {
            get => txtBoxBuyValue.Text;
            set
            {
                if (txtBoxBuyValue.Text == value)
                    return;
                txtBoxBuyValue.Text = value;
            }
        }

        public string BuyValueBrokerageReduction
        {
            get => txtBoxBuyValueBrokerageReduction.Text;
            set
            {
                if (txtBoxBuyValueBrokerageReduction.Text == value)
                    return;
                txtBoxBuyValueBrokerageReduction.Text = value;
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

        #endregion Input values

        public void AddEditDeleteFinish()
        {
            // Set messages
            var strMessage = string.Empty;
            var clrMessage = Color.Black;
            var stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case BuyErrorCode.AddSuccessful:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/AddSuccess", LanguageName);
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Check if a direct document parsing is done
                    if (ParsingFileName != null)
                        Close();

                    // Refresh the buy list
                    OnShowBuys();

                    break;
                }
                case BuyErrorCode.AddFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case BuyErrorCode.EditSuccessful:
                {
                    // Enable button(s)
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add",
                            LanguageName);
                    btnAddSave.Image = Resources.button_add_24;
                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption",
                            LanguageName);

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/EditSuccess", LanguageName);
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Refresh the buy list
                    OnShowBuys();

                    break;
                }
                case BuyErrorCode.EditFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/EditFailed", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case BuyErrorCode.DeleteSuccessful:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/DeleteSuccess", LanguageName);
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Enable button(s)
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Refresh the buy list
                    OnShowBuys();

                    break;
                }
                case BuyErrorCode.DeleteFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case BuyErrorCode.DeleteFailedUnerasable:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailedUnErasable", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case BuyErrorCode.InputValuesInvalid:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CheckInputFailure", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case BuyErrorCode.OrderNumberEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/OrderNumberEmpty", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case BuyErrorCode.OrderNumberExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/OrderNumberExists", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxOrderNumber.Focus();

                    break;
                }
                case BuyErrorCode.VolumeEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeEmpty", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.VolumeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.VolumeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.SharePriceEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceEmpty", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.SharePriceWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.SharePriceWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.ProvisionWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ProvisionWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.ProvisionWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ProvisionWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.BrokerFeeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/BrokerFeeWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.BrokerFeeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/BrokerFeeWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.TraderPlaceFeeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/TraderPlaceFeeWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.TraderPlaceFeeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/TraderPlaceFeeWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxSharePrice.Focus();

                    break;
                }
                case BuyErrorCode.ReductionWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxReduction.Focus();

                    break;
                }
                case BuyErrorCode.ReductionWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxReduction.Focus();

                    break;
                }
                case BuyErrorCode.BrokerageEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/BrokerageEmpty", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case BuyErrorCode.BrokerageWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/BrokerageWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case BuyErrorCode.BrokerageWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/BrokerageWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case BuyErrorCode.DocumentDirectoryDoesNotExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DocumentDirectoryDoesNotExists", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxDocument.Focus();

                    break;
                }
                case BuyErrorCode.DocumentFileDoesNotExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DocumentFileDoesNotExists", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    break;
                }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
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
                case BuyErrorCode.DocumentBrowseFailed:
                {
                    txtBoxDocument.Text = @"-";

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ChoseDocumentFailed",
                            LanguageName);
                    break;
                }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                strMessage,
                Language,
                LanguageName,
                clrMessage,
                Logger,
                (int) stateLevel,
                (int) FrmMain.EComponentLevels.Application);
        }

        #endregion IView members

        #region Event members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValuesEventHandler;
        public event EventHandler AddBuyEventHandler;
        public event EventHandler EditBuyEventHandler;
        public event EventHandler DeleteBuyEventHandler;
        public event EventHandler DocumentBrowseEventHandler;

        #endregion

        #region Methods

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market value share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final value share object</param>
        /// <param name="logger">Logger</param>
        /// <param name="language">Language file</param>
        /// <param name="languageName">Language</param>
        /// <param name="parsingFileName">File name of the parsing file which is given via document capture</param>
        public ViewBuyEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue,
            Logger logger, Language language, string languageName,
            string parsingFileName = null)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;

            Logger = logger;
            Language = language;
            LanguageName = languageName;

            ParsingFileName = parsingFileName;

            _focusedControl = txtBoxOrderNumber;

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
        private void ShareBuysEdit_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Caption", LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxBuys.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/Caption",
                        LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                lblOrderNumber.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/OrderNumber",
                    LanguageName);
                lblVolume.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Volume",
                        LanguageName);
                lblVolumeSold.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/VolumeSold",
                        LanguageName);
                lblBuyPrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Price",
                        LanguageName);
                lblBuyValue.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/MarketValue",
                        LanguageName);
                lblProvision.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Provision",
                        LanguageName);
                lblBrokerFee.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/BrokerFee",
                        LanguageName);
                lblTraderPlaceFee.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/TraderPlaceFee",
                        LanguageName);
                lblReduction.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Reduction",
                        LanguageName);
                lblBrokerage.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Brokerage",
                        LanguageName);
                lblBuyValueBrokerageReduction.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/FinalValue",
                        LanguageName);
                lblDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add",
                    LanguageName);
                btnReset.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Reset",
                    LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                #endregion Language configuration

                #region Unit configuration

                // Set buy units to the edit boxes
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblVolumeSoldUnit.Text = ShareObject.PieceUnit;
                lblDepositUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblProvisionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblBrokerFeeUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTraderPlaceFeeUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblReductionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblBrokerageUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblBuyValueUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.button_save_24;
                btnDelete.Image = Resources.button_recycle_bin_24;
                btnReset.Image = Resources.button_reset_24;
                btnCancel.Image = Resources.button_back_24;

                #endregion Image configuration

                // Load buys of the share
                OnShowBuys();

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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", LanguageName),
                    Language, LanguageName, Color.DarkRed, Logger,
                    (int) FrmMain.EStateLevels.FatalError, (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a buy has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareBuysEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if a buy change must be saved
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date.
        /// </summary>
        private void ResetValues()
        {
            // Reset state pictures
            picBoxDateParseState.Image = Resources.empty_arrow;
            picBoxTimeParseState.Image = Resources.empty_arrow;
            picBoxOrderNumberParseState.Image = Resources.empty_arrow;
            picBoxVolumeParseState.Image = Resources.empty_arrow;
            picBoxPriceParseState.Image = Resources.empty_arrow;
            picBoxProvisionParseState.Image = Resources.empty_arrow;
            picBoxBrokerFeeParseState.Image = Resources.empty_arrow;
            picBoxTraderPlaceFeeParseState.Image = Resources.empty_arrow;
            picBoxReductionParseState.Image = Resources.empty_arrow;

            // Reset and enable date time picker
            dateTimePickerDate.Value = DateTime.Now;
            dateTimePickerDate.Enabled = true;
            dateTimePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePickerTime.Enabled = true;

            // Reset and enable text boxes
            txtBoxOrderNumber.Text = string.Empty;
            txtBoxOrderNumber.Enabled = true;
            txtBoxVolume.Text = string.Empty;
            txtBoxVolume.Enabled = true;
            txtBoxVolumeSold.Text = string.Empty;
            txtBoxSharePrice.Text = string.Empty;
            txtBoxSharePrice.Enabled = true;
            txtBoxProvision.Text = string.Empty;
            txtBoxProvision.Enabled = true;
            txtBoxBrokerFee.Text = string.Empty;
            txtBoxBrokerFee.Enabled = true;
            txtBoxTraderPlaceFee.Text = string.Empty;
            txtBoxTraderPlaceFee.Enabled = true;
            txtBoxReduction.Text = string.Empty;
            txtBoxReduction.Enabled = true;
            // Do not reset document value if a parsing is running
            if (!_parsingStartAllow)
            {
                txtBoxDocument.Text = string.Empty;
                txtBoxDocument.Enabled = true;
            }

            // Reset status label message text
            toolStripStatusLabelMessageBuyEdit.Text = string.Empty;
            toolStripStatusLabelMessageBuyDocumentParsing.Text = string.Empty;
            toolStripProgressBarBuyDocumentParsing.Visible = false;

            // Enable button(s)
            btnAddSave.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
            btnAddSave.Image = Resources.button_add_24;
            btnDocumentBrowse.Enabled = true;

            // Disable button(s)
            btnDelete.Enabled = false;

            // Rename group box
            grpBoxAdd.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

            // Deselect rows
            OnDeselectRowsOfDataGridViews(null);

            // Reset stored DataGridView instance
            SelectedDataGridView = null;

            // Select overview tab
            if (tabCtrlBuys.TabPages.Count > 0)
                tabCtrlBuys.SelectTab(0);

            txtBoxOrderNumber.Focus();

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

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxOrderNumber_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OrderNumber"));
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
        private void OnTxtBoxAddVolumeSold_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VolumeSold"));
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxVolumeSold_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SharePrice"));
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
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerFee_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxBrokerFee;
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
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TraderPlaceFee"));
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
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
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
        private void OnTxtBoxBrokerage_Leave(object sender, EventArgs e)
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
        private void OnTxtBoxReduction_Leave(object sender, EventArgs e)
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
        /// This function shows the Drop sign
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// This function allows to sets via Drag and Drop a document for this buy
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                _parsingStartAllow = true;

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
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                toolStripStatusLabelMessageBuyDocumentParsing.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingFailed", LanguageName);

                toolStripProgressBarBuyDocumentParsing.Visible = false;
                grpBoxAdd.Enabled = true;
                grpBoxBuys.Enabled = true;

                DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                _parsingThreadFinished = true;
                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);
            }
        }

        #endregion TextBoxes

        #region Buttons

        /// <summary>
        /// This function adds a new buy entry to the share object
        /// or edit a buy entry
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
            toolStripStatusLabelMessageBuyEdit.Text = string.Empty;
            toolStripStatusLabelMessageBuyDocumentParsing.Text = string.Empty;
            toolStripProgressBarBuyDocumentParsing.Visible = false;

            if (btnAddSave.Text ==
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName))
            {
                UpdateBuy = false;

                AddBuyEventHandler?.Invoke(this, null);
            }
            else
            {
                UpdateBuy = true;

                EditBuyEventHandler?.Invoke(this, null);
            }
        }
        catch (Exception ex)
        {
            Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", LanguageName),
                Language, LanguageName,
                Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                (int) FrmMain.EComponentLevels.Application,
                ex);
        }
    }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the buy.
        /// If the user press "Ok" the buy will be deleted and the
        /// list of the buys will be updated.
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
                toolStripStatusLabelMessageBuyEdit.Text = string.Empty;
                toolStripStatusLabelMessageBuyDocumentParsing.Text = string.Empty;
                toolStripProgressBarBuyDocumentParsing.Visible = false;

                var strCaption = Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", LanguageName)[
                    (int) EOwnMessageBoxInfoType.Info];
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/BuyDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok",
                    LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel",
                    LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult != DialogResult.OK) return;

                // Set flag to save the share object.
                SaveFlag = true;

                // Check if a row is selected
                if (SelectedDataGridView != null && SelectedDataGridView.SelectedRows.Count == 1)
                {
                    DeleteBuyEventHandler?.Invoke(this, null);
                }

                // Reset values
                ResetValues();

                // Enable button(s)
                btnAddSave.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.button_add_24;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Refresh the dividend list
                OnShowBuys();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a buy
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset values
                ResetValues();
            }
            catch (Exception ex)
            {

                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CancelFailure", LanguageName),
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
            toolStripStatusLabelMessageBuyEdit.Text = string.Empty;
            toolStripStatusLabelMessageBuyDocumentParsing.Text = string.Empty;

            _parsingStartAllow = true;

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
        /// This function paints the buy list of the share
        /// </summary>
        private void OnShowBuys()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlBuys.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBuysOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBuysOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBuysOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBuysOfAYear_MouseEnter;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewBuys_DataBindingComplete;
                    }

                    tabPage.Controls.Clear();
                    tabCtrlBuys.TabPages.Remove(tabPage);
                }

                tabCtrlBuys.TabPages.Clear();

                // Enable controls
                Enabled = true;

                #region Add page

                // Create TabPage for the buys of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", LanguageName)
                           + @" (" + ShareObjectFinalValue.AllBuyEntries.BuyValueBrokerageReductionTotalAsStrUnit +
                           @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Check if buys exists
                if (ShareObjectFinalValue.AllBuyEntries.GetAllBuysTotalValues().Count <= 0) return;

                // Reverse list so the latest is a top of the data grid view
                var reserveDataSourceOverview = ShareObjectFinalValue.AllBuyEntries.GetAllBuysTotalValues()
                    .OrderByDescending((x) => x.BuyYearAsStr).ToList();

                var bindingSourceOverview = new BindingSource
                {
                    DataSource = reserveDataSourceOverview
                };

                // Create DataGridView
                var dataGridViewBuysOverviewOfAYears = new DataGridView
                {
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with buy values to the DataGridView
                    DataSource = bindingSourceOverview,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewBuysOverviewOfAYears.DataBindingComplete += OnDataGridViewBuys_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewBuysOverviewOfAYears.MouseEnter += OnDataGridViewBuysOfYears_MouseEnter;
                // Set row select event
                dataGridViewBuysOverviewOfAYears.SelectionChanged += OnDataGridViewBuysOfYears_SelectionChanged;

                #endregion Events

                #region Style

                DataGridViewHelper.DataGridViewConfiguration(dataGridViewBuysOverviewOfAYears);

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewBuysOverviewOfAYears);
                dataGridViewBuysOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlBuys.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlBuys;

                #endregion Control add

                // Check if buys exists
                if (ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Count <= 0) return;

                // Loop through the years of the buys
                foreach (
                    var keyName in ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Keys.Reverse()
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
                               ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary[keyName]
                                   .BuyValueBrokerageReductionYearAsStrUnit
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Reverse list so the latest is a top of the data grid view
                    var reversDataSource =
                        ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary[keyName]
                            .BuyListYear.OrderByDescending(x=>DateTime.Parse(x.Date)).ToList();

                    // Create Binding source for the buy data
                    var bindingSource = new BindingSource
                    {
                        DataSource = reversDataSource

                    };

                    // Create DataGridView
                    var dataGridViewBuysOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with buy values to the DataGridView
                        DataSource = bindingSource,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewBuysOfAYear.DataBindingComplete += OnDataGridViewBuys_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewBuysOfAYear.MouseEnter += OnDataGridViewBuysOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewBuysOfAYear.SelectionChanged += OnDataGridViewBuysOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewBuysOfAYear.CellContentDoubleClick += OnDataGridViewBuysOfAYear_CellContentClick;

                    #endregion Events

                    #region Style

                    DataGridViewHelper.DataGridViewConfiguration(dataGridViewBuysOfAYear);

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewBuysOfAYear);
                    dataGridViewBuysOfAYear.Parent = newTabPage;
                    tabCtrlBuys.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlBuys;

                    #endregion Control add
                }

                tabCtrlBuys.TabPages[0].Select();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", LanguageName),
                    Language, LanguageName, Color.DarkRed, Logger,
                    (int) FrmMain.EStateLevels.FatalError, (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewBuys_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (var i = 0; i < ((DataGridView) sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView) sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    // Disable sorting of the columns ( remove sort arrow )
                    ((DataGridView) sender).Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    switch (i)
                    {
                        case 0:
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Year",
                                        LanguageName);
                            }

                            
                            break;
                        case 1:
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Volume",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Date",
                                        LanguageName);
                            }

                            break;
                        case 2:
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Purchase",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Volume",
                                        LanguageName);
                            }

                            break;
                        case 3:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Price",
                                    LanguageName);
                            break;
                        case 4:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Brokerage",
                                    LanguageName);
                            break;
                        case 5:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Purchase",
                                    LanguageName);
                            break;
                        case 6:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Document",
                                    LanguageName);
                            break;
                    }
                }

                if (((DataGridView) sender).Rows.Count > 0)
                {
                    ((DataGridView) sender).Rows[0].Selected = false;
                    ((DataGridView) sender).ScrollBars = ScrollBars.Both;
                }

                if (((DataGridView) sender).Name != @"Overview")
                    ((DataGridView) sender).Columns[0].Visible = false;

                // Reset the text box values
                ResetValues();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/RenameColHeaderFailed", LanguageName),
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
                foreach (TabPage tabPage in tabCtrlBuys.TabPages)
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeselectFailed", LanguageName),
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
        private void OnTabCtrlBuys_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabCtrlBuys.SelectedTab == null) return;

                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlBuys.SelectedTab.Controls)
                {
                    if (!(control is DataGridView view)) continue;
                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                    {
                        if (view.Rows[0].Cells.Count > 0)
                            view.Rows[0].Selected = true;
                    }

                    view.Focus();

                    if (view.Name == @"Overview")
                        ResetValues();
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function sets the focus back to the last focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlBuys_MouseLeave(object sender, EventArgs e)
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
        private void OnTabCtrlBuys_KeyDown(object sender, KeyEventArgs e)
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
        private void OnTabCtrlBuys_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (_focusedControl)
            {
                // Check if the last focused control was a text box so set the
                // key value ( char ) to the text box and then set the cursor behind the text
                case TextBox textBox:
                    textBox.Text = e.KeyChar.ToString();
                    textBox.Select(textBox.Text.Length, 0);
                    break;
                // Check if the last focused control was a date time picker
                case DateTimePicker dateTimePicker:
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
                    break;
                }
            }
        }

        #endregion Tab control delegates

        #region Buys of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewBuysOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView) sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlBuys.TabPages)
                {
                    if (curItem[0].Cells[1].Value != null &&
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()
                    ) continue;

                    tabCtrlBuys.SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
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
        private static void OnDataGridViewBuysOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
        }

        #endregion Buys of years

        #region Buys of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewBuysOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlBuys.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlBuys.SelectedTab.Controls.Contains((DataGridView) sender))
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

                    // Get selected buy object by Guid
                    var selectedBuyObject = ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare()
                        .Find(x => x.Guid == SelectedGuid);

                    // Set buy values
                    if (selectedBuyObject != null)
                    {
                        dateTimePickerDate.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        dateTimePickerTime.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        txtBoxOrderNumber.Text = selectedBuyObject.OrderNumber;
                        txtBoxVolume.Text = selectedBuyObject.VolumeAsStr;
                        txtBoxVolumeSold.Text = selectedBuyObject.VolumeSoldAsStr;
                        txtBoxSharePrice.Text = selectedBuyObject.PriceAsStr;
                        txtBoxProvision.Text = selectedBuyObject.ProvisionAsStr;
                        txtBoxBrokerFee.Text = selectedBuyObject.BrokerFeeAsStr;
                        txtBoxTraderPlaceFee.Text = selectedBuyObject.TraderPlaceFeeAsStr;
                        txtBoxReduction.Text = selectedBuyObject.ReductionAsStr;
                        txtBoxDocument.Text = selectedBuyObject.DocumentAsStr;

                        if (ShareObjectFinalValue.AllBuyEntries.IsLastBuy(SelectedGuid) &&
                            !ShareObjectFinalValue.AllBuyEntries.IsPartOfASale(SelectedGuid)
                        )
                        {
                            // Check if the delete button should be enabled or not
                            btnDelete.Enabled = ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare().Count > 1;

                            // Enable TextBox(es)
                            dateTimePickerDate.Enabled = true;
                            dateTimePickerTime.Enabled = true;
                            txtBoxOrderNumber.Enabled = true;
                            txtBoxVolume.Enabled = true;
                            txtBoxVolumeSold.Enabled = true;
                            txtBoxSharePrice.Enabled = true;
                            txtBoxBuyValue.Enabled = true;
                            txtBoxProvision.Enabled = true;
                            txtBoxBrokerFee.Enabled = true;
                            txtBoxTraderPlaceFee.Enabled = true;
                            txtBoxBrokerage.Enabled = true;
                            txtBoxReduction.Enabled = true;
                            txtBoxBuyValueBrokerageReduction.Enabled = true;
                        }
                        else
                        {
                            // Disable Button(s)
                            btnDelete.Enabled = false;

                            // Disable TextBox(es)
                            dateTimePickerDate.Enabled = false;
                            dateTimePickerTime.Enabled = false;
                            txtBoxOrderNumber.Enabled = false;
                            txtBoxVolume.Enabled = false;
                            txtBoxVolumeSold.Enabled = false;
                            txtBoxSharePrice.Enabled = false;
                            txtBoxBuyValue.Enabled = false;
                            txtBoxProvision.Enabled = false;
                            txtBoxBrokerFee.Enabled = false;
                            txtBoxTraderPlaceFee.Enabled = false;
                            txtBoxBrokerage.Enabled = false;
                            txtBoxReduction.Enabled = false;
                            txtBoxBuyValueBrokerageReduction.Enabled = false;
                        }

                        // Rename button
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Save",
                                LanguageName);
                        btnAddSave.Image = Resources.button_pencil_24;

                        // Rename group box
                        grpBoxAdd.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Edit_Caption",
                                LanguageName);

                        // Store DataGridView instance
                        SelectedDataGridView = (DataGridView) sender;

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
                        txtBoxVolumeSold.Text = string.Empty;
                        txtBoxSharePrice.Text = string.Empty;
                        txtBoxSharePrice.Enabled = false;
                        txtBoxProvision.Text = string.Empty;
                        txtBoxProvision.Enabled = false;
                        txtBoxBrokerFee.Text = string.Empty;
                        txtBoxBrokerFee.Enabled = false;
                        txtBoxTraderPlaceFee.Text = string.Empty;
                        txtBoxTraderPlaceFee.Enabled = false;
                        txtBoxReduction.Text = string.Empty;
                        txtBoxReduction.Enabled = false;
                        txtBoxDocument.Text = string.Empty;
                        txtBoxDocument.Enabled = false;

                        // Reset status label message text
                        toolStripStatusLabelMessageBuyEdit.Text = string.Empty;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text = string.Empty;
                        toolStripProgressBarBuyDocumentParsing.Visible = false;

                        // Disable Button(s)
                        btnDocumentBrowse.Enabled = false;
                        btnAddSave.Enabled = false;
                        btnDelete.Enabled = false;

                        Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                            (int)FrmMain.EComponentLevels.Application);
                    }
                }
                else
                {
                    // Rename button
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Enable Button(s)
                    btnAddSave.Enabled = true;
                    btnDocumentBrowse.Enabled = true;

                    // Enable date time picker
                    dateTimePickerDate.Enabled = true;
                    dateTimePickerTime.Enabled = true;

                    // Enabled TextBox(es)
                    txtBoxOrderNumber.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxVolumeSold.Enabled = true;
                    txtBoxSharePrice.Enabled = true;
                    txtBoxProvision.Enabled = true;
                    txtBoxBrokerFee.Enabled = true;
                    txtBoxTraderPlaceFee.Enabled = true;
                    txtBoxBuyValue.Enabled = true;
                    txtBoxBrokerage.Enabled = true;
                    txtBoxReduction.Enabled = true;
                    txtBoxBuyValueBrokerageReduction.Enabled = true;
                    txtBoxDocument.Enabled = true;

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
                tabCtrlBuys.SelectedIndex = 0;

                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
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
        private static void OnDataGridViewBuysOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
        }

        /// <summary>
        /// This function opens the buy document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewBuysOfAYear_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                var iColumnCount = ((DataGridView) sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex != iColumnCount - 1) return;

                // Check if a row is selected
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the current selected row
                var curItem = ((DataGridView) sender).SelectedRows;
                // Get Guid of the selected buy item
                var strGuid = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value == null) return;

                // Get doc from the buy with the Guid
                foreach (var temp in ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare())
                {
                    // Check if the buy Guid is the same as the Guid of the clicked buy item
                    if (temp.Guid != strGuid) continue;

                    // Check if a file is set for this buy
                    if (temp.DocumentAsStr != "")
                    {
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
                                // Remove move document from the buy objects
                                if (ShareObjectFinalValue.SetBuyDocument(strGuid, temp.Date, string.Empty) &&
                                    ShareObjectMarketValue.SetBuyDocument(strGuid, temp.Date, string.Empty))
                                {
                                    // Set flag to save the share object.
                                    SaveFlag = true;

                                    OnShowBuys();

                                    Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                                        Language.GetLanguageTextByXPath(
                                            @"/AddEditFormBuy/StateMessages/EditSuccess", LanguageName),
                                        Language, LanguageName,
                                        Color.Black, Logger, (int) FrmMain.EStateLevels.Info,
                                        (int) FrmMain.EComponentLevels.Application);
                                }
                                else
                                {
                                    Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                                        Language.GetLanguageTextByXPath(
                                            @"/AddEditFormBuy/Errors/EditFailed", LanguageName),
                                        Language, LanguageName,
                                        Color.Red, Logger, (int) FrmMain.EStateLevels.Error,
                                        (int) FrmMain.EComponentLevels.Application);
                                }
                            }
                        }
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Buys of a year

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
                _buyIdentifierFound = false;
                _documentTypNotImplemented = false;
                _documentValuesRunning = false;

                ParsingText = string.Empty;

                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingStarted);

                DocumentType = DocumentParsingConfiguration.DocumentTypes.BuyDocument;
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
                    if (DocumentTypeParser != null && DocumentTypeParser.ParserInfoState.State == DataTypes.ParserState.Idle)
                    {
                        DocumentTypeParser.OnParserUpdate += DocumentTypeParser_UpdateGUI;

                        // Reset flags
                        _bankIdentifierFound = false;
                        _buyIdentifierFound = false;

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
                            case DataTypes.ParserErrorCodes.WebExceptionOccured:
                                {
                                    break;
                                }
                            case DataTypes.ParserErrorCodes.ExceptionOccured:
                                {
                                    break;
                                }
                        }

                        if (DocumentTypeParser.ParserErrorCode > 0)
                            Thread.Sleep(100);

                        // Check if a error occurred or the process has been finished
                        if (e.ParserInfoState.LastErrorCode < 0 || e.ParserInfoState.LastErrorCode == DataTypes.ParserErrorCodes.Finished)
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
                                            .BuyIdentifierTagName))
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
                                            .BuyIdentifierTagName))
                                        _buyIdentifierFound = true;

                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName) &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BuyIdentifierTagName))
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
                                    else if (_buyIdentifierFound == false)
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
                case (int)ParsingErrorCode.ParsingStarted:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingStateMessages/ParsingStarted",
                            LanguageName);

                    toolStripProgressBarBuyDocumentParsing.Visible = true;
                    grpBoxAdd.Enabled = false;
                    grpBoxBuys.Enabled = false;
                    break;
                }
                case (int)ParsingErrorCode.ParsingParsingDocumentError:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingParsingDocumentError",
                            LanguageName);

                    toolStripProgressBarBuyDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxBuys.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingFailed:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingFailed",
                            LanguageName);

                    toolStripProgressBarBuyDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxBuys.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingDocumentNotImplemented:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingDocumentNotImplemented",
                            LanguageName);

                    toolStripProgressBarBuyDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxBuys.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingBankIdentifierFailed:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingBankIdentifierFailed",
                            LanguageName);

                    toolStripProgressBarBuyDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxBuys.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingDocumentTypeIdentifierFailed:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormBuy/ParsingErrors/ParsingDocumentTypeIdentifierFailed",
                            LanguageName);

                    toolStripProgressBarBuyDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxBuys.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingDocumentFailed:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingDocumentFailed",
                            LanguageName);

                    toolStripProgressBarBuyDocumentParsing.Visible = false;
                    grpBoxAdd.Enabled = true;
                    grpBoxBuys.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingIdentifierValuesFound:
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingIdentifierValuesFound",
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
                    .DocumentTypeBuyWkn) || DictionaryParsingResult[DocumentParsingConfiguration
                        .DocumentTypeBuyWkn][0] != ShareObjectFinalValue.Wkn)
                {
                    toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageBuyDocumentParsing.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingWrongWkn",
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
                            case DocumentParsingConfiguration.DocumentTypeBuyOrderNumber:
                                picBoxOrderNumberParseState.Image = Resources.search_ok_24;
                                txtBoxOrderNumber.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyDate:
                                picBoxDateParseState.Image = Resources.search_ok_24;
                                dateTimePickerDate.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyTime:
                                picBoxTimeParseState.Image = Resources.search_ok_24;
                                dateTimePickerTime.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyVolume:
                                picBoxVolumeParseState.Image = Resources.search_ok_24;
                                txtBoxVolume.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyPrice:
                                picBoxPriceParseState.Image = Resources.search_ok_24;
                                txtBoxSharePrice.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyProvision:
                                picBoxProvisionParseState.Image = Resources.search_ok_24;
                                txtBoxProvision.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyBrokerFee:
                                picBoxBrokerFeeParseState.Image = Resources.search_ok_24;
                                txtBoxBrokerFee.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee:
                                picBoxTraderPlaceFeeParseState.Image = Resources.search_ok_24;
                                txtBoxTraderPlaceFee.Text = resultEntry.Value[0].Trim();
                                break;
                            case DocumentParsingConfiguration.DocumentTypeBuyReduction:
                                picBoxReductionParseState.Image = Resources.search_ok_24;
                                txtBoxReduction.Text = resultEntry.Value[0].Trim();
                                break;
                        }
                    }

                    #endregion Check which values are found

                    #region Not found values

                    // Which values are not found
                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyOrderNumber) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyOrderNumber) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyOrderNumber].Count == 0
                    )
                    {
                        picBoxOrderNumberParseState.Image = Resources.search_failed_24;
                        txtBoxOrderNumber.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyDate) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyDate) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyDate].Count == 0
                    )
                    {
                        picBoxDateParseState.Image = Resources.search_failed_24;
                        dateTimePickerDate.Value = DateTime.Now;
                        dateTimePickerTime.Value =
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        picBoxDateParseState.Image = Resources.search_failed_24;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTime) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTime) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyTime].Count == 0
                    )
                    {
                        dateTimePickerTime.Value =
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        picBoxTimeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyVolume) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyVolume) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyVolume].Count == 0
                    )
                    {
                        picBoxVolumeParseState.Image = Resources.search_failed_24;
                        txtBoxVolume.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyPrice) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyPrice) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyPrice].Count == 0
                    )
                    {
                        picBoxVolumeParseState.Image = Resources.search_failed_24;
                        txtBoxSharePrice.Text = string.Empty;
                        _parsingResult = false;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyProvision) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyProvision) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyProvision].Count == 0
                    )
                    {
                        picBoxProvisionParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyBrokerFee) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyBrokerFee) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyBrokerFee].Count == 0
                    )
                    {
                        picBoxBrokerFeeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyBrokerFee) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyBrokerFee) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyBrokerFee].Count == 0
                    )
                    {
                        picBoxBrokerFeeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee].Count == 0
                    )
                    {
                        picBoxTraderPlaceFeeParseState.Image = Resources.search_info_24;
                    }

                    if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyReduction) ||
                        DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyReduction) &&
                        DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyReduction].Count == 0
                    )
                    {
                        picBoxReductionParseState.Image = Resources.search_info_24;
                    }

                    #endregion Not found values

                    if (!_parsingResult)
                    {
                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingFailed",
                                LanguageName);
                    }
                    else
                    {
                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Black;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingStateMessages/ParsingDocumentSuccessful",
                                LanguageName);
                    }
                }
            }

            toolStripProgressBarBuyDocumentParsing.Visible = false;
            grpBoxAdd.Enabled = true;
            grpBoxBuys.Enabled = true;
        }

        #endregion Parsing

        #endregion Methods
    }
}