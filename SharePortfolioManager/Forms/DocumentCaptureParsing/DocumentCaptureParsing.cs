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
#define DEBUG_DOCUMENT_CAPTURE_PARSING

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using Parser;
using SharePortfolioManager.BuysForm.Model;
using SharePortfolioManager.BuysForm.Presenter;
using SharePortfolioManager.BuysForm.View;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.DividendForm.Model;
using SharePortfolioManager.DividendForm.Presenter;
using SharePortfolioManager.DividendForm.View;
using SharePortfolioManager.Properties;
using SharePortfolioManager.SalesForm.Model;
using SharePortfolioManager.SalesForm.Presenter;
using SharePortfolioManager.SalesForm.View;
using SharePortfolioManager.ShareAddForm.Model;
using SharePortfolioManager.ShareAddForm.Presenter;
using SharePortfolioManager.ShareAddForm.View;

namespace SharePortfolioManager.DocumentCaptureParsing
{
    public partial class FrmDocumentCaptureParsing : Form
    {
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

        #region Parsing Fields

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
        /// Flag if the sale identifier type of the given document could be found in the document configurations
        /// </summary>
        private bool _saleIdentifierFound;
        
        /// <summary>
        /// Flag if the dividend identifier type of the given document could be found in the document configurations
        /// </summary>
        private bool _dividendIdentifierFound;

        /// <summary>
        /// Flag if the brokerage identifier type of the given document could be found in the document configurations
        /// </summary>
        private bool _brokerageIdentifierFound;

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

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        #region Transfer parameter

        public FrmMain ParentWindow { get; internal set; }

        public Logger Logger { get; internal set; }

        public Language Language { get; internal set; }

        public string LanguageName { get; internal set; }

        public string ParsingFileName { get; internal set; }

        #endregion Transfer parameter

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get => ParentWindow.ShareObjectMarketValue;
            set => ParentWindow.ShareObjectMarketValue = value;
        }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue
        {
            get => ParentWindow.ShareObjectListMarketValue;
            set => ParentWindow.ShareObjectListMarketValue = value;
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get => ParentWindow.ShareObjectFinalValue;
            set => ParentWindow.ShareObjectFinalValue = value;
        }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue
        {
            get => ParentWindow.ShareObjectListFinalValue;
            set => ParentWindow.ShareObjectListFinalValue = value;
        }

        #region Parsing

        public DocumentParsingConfiguration.DocumentTypes DocumentType { get; internal set; }

        public Parser.Parser DocumentTypeParser;

        public string ParsingText { get; internal set; }

        public Dictionary<string, List<string>> DictionaryParsingResult;

        #endregion Parsing

        #endregion Properties

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentWindow">Parent window</param>
        /// <param name="parsingFileName">File name of the file which should be parsed</param>
        /// <param name="logger">Logger</param>
        /// <param name="language">Language file</param>
        /// <param name="languageName">Language</param>
        public FrmDocumentCaptureParsing(FrmMain parentWindow, string parsingFileName, Logger logger, Language language, string languageName)
        {
            InitializeComponent();

            ParentWindow = parentWindow;

            ParsingFileName = parsingFileName;

            Logger = logger;
            Language = language;
            LanguageName = languageName;

            #region Language

            Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/Caption", LanguageName);

            grpBoxParsing.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/Caption", LanguageName);

            lblBankName.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/Labels/BankName", LanguageName);
            lblDocumentType.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/Labels/DocumentType", LanguageName);
            lblWkn.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/Labels/Wkn", LanguageName);

            btnClose.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/Button/Close", LanguageName);

            #endregion Language

            #region Parsing backgroundworker

            _parsingBackgroundWorker.WorkerReportsProgress = true;
            _parsingBackgroundWorker.WorkerSupportsCancellation = true;

            _parsingBackgroundWorker.DoWork += DocumentParsing;
            _parsingBackgroundWorker.ProgressChanged += OnDocumentParsingProgressChanged;
            _parsingBackgroundWorker.RunWorkerCompleted += OnDocumentParsingRunWorkerCompleted;

            if (_parsingBackgroundWorker.IsBusy)
            {
                _parsingBackgroundWorker.CancelAsync();
            }
            else
            {
                _parsingBackgroundWorker.RunWorkerAsync();
            }

            #endregion Parsing backgroundworker
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
                _buyIdentifierFound = false;
                _documentTypNotImplemented = false;
                _documentValuesRunning = false;

                ParsingText = string.Empty;

                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingStarted);

                DocumentType = DocumentParsingConfiguration.DocumentTypes.BuyDocument;
                DocumentTypeParser = null;
                DictionaryParsingResult = null;

                Helper.RunProcess($"{Helper.PdfConverterApplication}", $"-simple \"{ParsingFileName}\" {Helper.ParsingDocumentFileName}");

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
                        _saleIdentifierFound = false;
                        _dividendIdentifierFound = false;
                        _brokerageIdentifierFound = false;

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
                        //Console.WriteLine(@"Percentage: {0}", e.ParserInfoState.Percentage);
                        switch (e.ParserInfoState.LastErrorCode)
                        {
                            case DataTypes.ParserErrorCodes.Finished:
                                {
#if DEBUG_DOCUMENT_CAPTURE_PARSING
                                    if (e.ParserInfoState.SearchResult != null)
                                    {
                                        foreach (var result in e.ParserInfoState.SearchResult)
                                        {
                                            Console.Write(@"{0}:", result.Key);
                                            if (result.Value != null && result.Value.Count > 0)
                                                Console.WriteLine(@"{0}", result.Value[0]);
                                            else
                                                Console.WriteLine(@"-");
                                        }
                                    }
#endif
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
                                // Check if the bank identifier has been found
                                if (e.ParserInfoState.SearchResult != null &&
                                    e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                        .BankIdentifierTagName)
                                    &&
                                    e.ParserInfoState.SearchResult[DocumentParsingConfiguration.BankIdentifierTagName][0] == DocumentParsingConfiguration.BankRegexList[_bankCounter].BankIdentifier
                                    )
                                {
                                    _bankIdentifierFound = true;
                                    picBoxBank.Image = Resources.search_ok_24;
                                    txtBoxBank.Text = DocumentParsingConfiguration
                                        .BankRegexList[_bankCounter].BankName;
                                }

                                //Check if the correct bank identifier and document identifier may have been found so search for the document values
                                if (e.ParserInfoState.SearchResult != null &&
                                    e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                        .BankIdentifierTagName) &&
                                    (
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BuyIdentifierTagName)
                                        ||
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .SaleIdentifierTagName)
                                        ||
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .DividendIdentifierTagName)
                                        ||
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BrokerageIdentifierTagName)
                                        )
                                    )
                                {
                                    // Check if the buy identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BuyIdentifierTagName)
                                        &&
                                        e.ParserInfoState.SearchResult[DocumentParsingConfiguration.BuyIdentifierTagName].Count > 0
                                        )
                                    {
                                        _buyIdentifierFound = true;
                                        DocumentType = DocumentParsingConfiguration.DocumentTypes.BuyDocument;
                                        picBoxDocumentType.Image = Resources.search_ok_24;
                                        txtBoxDocumentType.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/DocumentTypes/Buy", LanguageName);
                                    }

                                    // Check if the sale identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .SaleIdentifierTagName)
                                        &&
                                        e.ParserInfoState.SearchResult[DocumentParsingConfiguration.SaleIdentifierTagName].Count > 0
                                        )
                                    {
                                        _saleIdentifierFound = true;
                                        DocumentType = DocumentParsingConfiguration.DocumentTypes.SaleDocument;
                                        picBoxDocumentType.Image = Resources.search_ok_24;
                                        txtBoxDocumentType.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/DocumentTypes/Sale", LanguageName);
                                    }

                                    // Check if the dividend identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .DividendIdentifierTagName)
                                        &&
                                        e.ParserInfoState.SearchResult[DocumentParsingConfiguration.DividendIdentifierTagName].Count > 0
                                        )
                                    {
                                        _dividendIdentifierFound = true;
                                        DocumentType = DocumentParsingConfiguration.DocumentTypes.DividendDocument;
                                        picBoxDocumentType.Image = Resources.search_ok_24;
                                        txtBoxDocumentType.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/DocumentTypes/Dividend", LanguageName);
                                    }

                                    // Check if the brokerage identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BrokerageIdentifierTagName)
                                        &&
                                        e.ParserInfoState.SearchResult[DocumentParsingConfiguration.BrokerageIdentifierTagName].Count > 0
                                        )
                                    {
                                        _brokerageIdentifierFound = true;
                                        DocumentType = DocumentParsingConfiguration.DocumentTypes.BrokerageDocument;
                                        picBoxDocumentType.Image = Resources.search_ok_24;
                                        txtBoxDocumentType.Text = Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/GrpBoxDocumentCapture/DocumentTypes/Brokerage", LanguageName);
                                    }

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
                                    else if (_buyIdentifierFound == false && _saleIdentifierFound == false && _dividendIdentifierFound == false && _brokerageIdentifierFound == false)
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
                            Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingStateMessages/ParsingStarted",
                                LanguageName);

                        toolStripProgressBarParsing.Visible = true;
                        grpBoxParsing.Enabled = false;
                        break;
                    }
                case (int)ParsingErrorCode.ParsingParsingDocumentError:
                    {
                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingErrors/ParsingParsingDocumentError",
                                LanguageName);

                        toolStripProgressBarParsing.Visible = false;
                        grpBoxParsing.Enabled = true;
                        break;
                    }
                case (int)ParsingErrorCode.ParsingFailed:
                    {
                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingErrors/ParsingFailed",
                                LanguageName);

                        toolStripProgressBarParsing.Visible = false;
                        grpBoxParsing.Enabled = true;
                        break;
                    }
                case (int)ParsingErrorCode.ParsingDocumentNotImplemented:
                    {
                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingErrors/ParsingDocumentNotImplemented",
                                LanguageName);

                        toolStripProgressBarParsing.Visible = false;
                        grpBoxParsing.Enabled = true;
                        break;
                    }
                case (int)ParsingErrorCode.ParsingBankIdentifierFailed:
                    {
                        picBoxBank.Image = Resources.search_failed_24;

                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingErrors/ParsingBankIdentifierFailed",
                                LanguageName);

                        toolStripProgressBarParsing.Visible = false;
                        grpBoxParsing.Enabled = true;
                        break;
                    }
                case (int)ParsingErrorCode.ParsingDocumentTypeIdentifierFailed:
                    {
                        picBoxDocumentType.Image = Resources.search_failed_24;

                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(
                                @"/DocumentCaptureParsing/ParsingErrors/ParsingDocumentTypeIdentifierFailed",
                                LanguageName);

                        toolStripProgressBarParsing.Visible = false;
                        grpBoxParsing.Enabled = true;
                        break;
                    }
                case (int)ParsingErrorCode.ParsingDocumentFailed:
                    {
                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingErrors/ParsingDocumentFailed",
                                LanguageName);

                        toolStripProgressBarParsing.Visible = false;
                        grpBoxParsing.Enabled = true;
                        break;
                    }
                case (int)ParsingErrorCode.ParsingIdentifierValuesFound:
                    {
                        toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Black;
                        toolStripStatusLabelMessageBuyDocumentParsing.Text =
                            Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingErrors/ParsingIdentifierValuesFound",
                                LanguageName);
                        break;
                    }
            }
        }

        private void OnDocumentParsingRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (DictionaryParsingResult != null)
                {
                    _parsingResult = true;

                    if (ShareObjectListMarketValue != null && ShareObjectListFinalValue != null)
                    {

                        // Check if the WKN has been found
                        if (_buyIdentifierFound &&
                            DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyWkn))
                        {
                            txtBoxWkn.Text =
                                DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyWkn][0];

                            if (ShareObjectListMarketValue.Count == 0 && ShareObjectListFinalValue.Count == 0
                                ||
                                !ShareObjectListMarketValue.Exists(x => x.Wkn == txtBoxWkn.Text) &&
                                !ShareObjectListFinalValue.Exists(x => x.Wkn == txtBoxWkn.Text)
                            )
                            {
                                // Create add share form
                                IModelShareAdd model = new ModelShareAdd();
                                IViewShareAdd view = new ViewShareAdd(ParentWindow, Logger, Language, LanguageName,
                                    WebSiteConfiguration.WebSiteRegexList, ParsingFileName);
                                // ReSharper disable once UnusedVariable
                                var presenterShareAdd = new PresenterShareAdd(view, model);

                                if (view.ShowDialog() == DialogResult.OK)
                                {
                                    ParentWindow.OnAddShare();
                                }
                            }
                            else
                            {
                                if (ShareObjectListFinalValue.Find(x => x.Wkn == txtBoxWkn.Text) != null)
                                    ShareObjectFinalValue =
                                        ShareObjectListFinalValue.Find(x => x.Wkn == txtBoxWkn.Text);
                                if (ShareObjectListMarketValue.Find(x => x.Wkn == txtBoxWkn.Text) != null)
                                    ShareObjectMarketValue =
                                        ShareObjectListMarketValue.Find(x => x.Wkn == txtBoxWkn.Text);

                                IModelBuyEdit model = new ModelBuyEdit();
                                IViewBuyEdit view = new ViewBuyEdit(ShareObjectMarketValue, ShareObjectFinalValue,
                                    Logger,
                                    Language, LanguageName,
                                    ParsingFileName);
                                // ReSharper disable once UnusedVariable
                                var presenterBuyEdit = new PresenterBuyEdit(view, model);

                                if (view.ShowDialog() == DialogResult.OK)
                                {
                                    // Get index of share object
                                    var index = ShareObjectListFinalValue.FindIndex(x => x.Wkn == txtBoxWkn.Text);
                                    ParentWindow.OnEditShare(index);
                                }
                            }
                        }
                        else if (_saleIdentifierFound &&
                                 DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                                     .DocumentTypeSaleWkn))
                        {
                            txtBoxWkn.Text =
                                DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeSaleWkn][0];


                            if (ShareObjectListFinalValue.Find(x => x.Wkn == txtBoxWkn.Text) != null)
                                ShareObjectFinalValue =
                                    ShareObjectListFinalValue.Find(x => x.Wkn == txtBoxWkn.Text);
                            if (ShareObjectListMarketValue.Find(x => x.Wkn == txtBoxWkn.Text) != null)
                                ShareObjectMarketValue =
                                    ShareObjectListMarketValue.Find(x => x.Wkn == txtBoxWkn.Text);

                            IModelSaleEdit model = new ModelSaleEdit();
                            IViewSaleEdit view = new ViewSaleEdit(ShareObjectMarketValue, ShareObjectFinalValue,
                                Logger,
                                Language, LanguageName,
                                ParsingFileName);
                            // ReSharper disable once UnusedVariable
                            var presenterSaleEdit = new PresenterSaleEdit(view, model);

                            if (view.ShowDialog() == DialogResult.OK)
                            {
                                // Get index of share object
                                var index = ShareObjectListFinalValue.FindIndex(x => x.Wkn == txtBoxWkn.Text);
                                ParentWindow.OnEditShare(index);
                            }
                        }
                        else if (_dividendIdentifierFound &&
                                 DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                                     .DocumentTypeDividendWkn))
                        {
                            txtBoxWkn.Text =
                                DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeDividendWkn][0];

                            if (ShareObjectListFinalValue.Find(x => x.Wkn == txtBoxWkn.Text) != null)
                                ShareObjectFinalValue =
                                    ShareObjectListFinalValue.Find(x => x.Wkn == txtBoxWkn.Text);
                            if (ShareObjectListMarketValue.Find(x => x.Wkn == txtBoxWkn.Text) != null)
                                ShareObjectMarketValue =
                                    ShareObjectListMarketValue.Find(x => x.Wkn == txtBoxWkn.Text);

                            IModelDividendEdit model = new ModelDividendEdit();
                            IViewDividendEdit view = new ViewDividendEdit(ShareObjectMarketValue, ShareObjectFinalValue,
                                Logger,
                                Language, LanguageName,
                                ParsingFileName);
                            // ReSharper disable once UnusedVariable
                            var presenterDividendEdit = new PresenterDividendEdit(view, model);

                            if (view.ShowDialog() == DialogResult.OK)
                            {
                                // Get index of share object
                                var index = ShareObjectListFinalValue.FindIndex(x => x.Wkn == txtBoxWkn.Text);
                                ParentWindow.OnEditShare(index);
                            }
                        }
                        else if (_brokerageIdentifierFound &&
                                 DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration
                                     .DocumentTypeBrokerageWkn))
                            txtBoxWkn.Text =
                                DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBrokerageWkn][0];
                        else
                            _parsingResult = false;

                        if (!_parsingResult)
                        {
                            picBoxWkn.Image = Resources.search_failed_24;

                            toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                            toolStripStatusLabelMessageBuyDocumentParsing.Text =
                                Language.GetLanguageTextByXPath(
                                    @"/DocumentCaptureParsing/ParsingErrors/ParsingFailed",
                                    LanguageName);
                        }
                        else
                        {
                            picBoxWkn.Image = Resources.search_ok_24;

                            toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Black;
                            toolStripStatusLabelMessageBuyDocumentParsing.Text =
                                Language.GetLanguageTextByXPath(
                                    @"/DocumentCaptureParsing/ParsingStateMessages/ParsingDocumentSuccessful",
                                    LanguageName);

                            Close();
                        }
                    }
                    else
                        _parsingResult = false;

                    toolStripProgressBarParsing.Visible = false;
                    grpBoxParsing.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);

                toolStripStatusLabelMessageBuyDocumentParsing.ForeColor = Color.Red;
                toolStripStatusLabelMessageBuyDocumentParsing.Text =
                    Language.GetLanguageTextByXPath(@"/DocumentCaptureParsing/ParsingErrors/ParsingFailed",
                        LanguageName);
            }
        }

        #endregion Parsing
    }
}
