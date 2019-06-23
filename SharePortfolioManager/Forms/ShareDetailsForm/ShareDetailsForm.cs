using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SharePortfolioManager.Forms.ShareDetailsForm
{
    // Error codes for the document parsing
    public enum ParsingErrorCodes
    {
        ParsingFailed = -2,
        UpdateFailed = -1,
        ParsingStarted = 0,
        ParsingProcess = 1,
        ParsingFinished = 2
    }

    public partial class ShareDetailsForm : Form
    {
        #region Variables

        /// <summary>
        /// State levels for the logging (e.g. Info)
        /// </summary>
        public enum EDetailsPageNumber
        {
            Chart = 0,
            FinalMarketValue = 1,
            ProfitLoss = 2,
            Dividend = 3,
            Brokerage = 4
        }

        /// <summary>
        /// Stores the name of the chart tab control
        /// </summary>
        private readonly string _tabPageDetailsChart = "tabPgDetailsChart";
        
        /// <summary>
        /// Stores the name of the final value tab control
        /// </summary>
        private readonly string _tabPageDetailsFinalValue = "tabPgDetailsFinalValue";

        /// <summary>
        /// Stores the name of the market value tab control
        /// </summary>
        private readonly string _tabPageDetailsMarketValue = "tabPgDetailsMarketValue";

        /// <summary>
        /// Stores the name of the dividends tab control
        /// </summary>
        private readonly string _tabPageDetailsDividendValue = "tabPgDividends";

        /// <summary>
        /// Stores the name of the brokerage tab control
        /// </summary>
        private readonly string _tabPageDetailsBrokerageValue = "tabPgBrokerage";

        /// <summary>
        /// Stores the name of the profit / loss value tab control
        /// </summary>
        private readonly string _tabPageDetailsProfitLossValue = "tabPgProfitLoss";

        private TabPage _tempFinalValues;
        private TabPage _tempMarketValues;
        private TabPage _tempProfitLoss;
        private TabPage _tempDividends;
        private TabPage _tempBrokerage;

        // Tool tip shows the X value of the mouse hovers
        private readonly ToolTip _tooltip = new ToolTip();
        private string _strUnit;

        #region Parsing Fields

        /// <summary>
        /// BackGroundWorker for the document parsing
        /// </summary>
        private readonly BackgroundWorker _parsingBackgroundWorker = new BackgroundWorker();

        #endregion Parsing Fields

        #endregion Variables

        #region Properties

        public bool MarketValueOverviewTabSelected;

        public RichTextBox RichTxtBoxStateMessage;

        public Logger Logger;

        public string LanguageName;

        public Language Language;

        #region Share objects

        public ShareObjectMarketValue ShareObjectMarketValue;

        public ShareObjectFinalValue ShareObjectFinalValue;

        #endregion Share objects

        #region Update

        public WebBrowser WebBrowser = new WebBrowser();

        #endregion Update

        #region Charting

        internal Point? PrevPosition { get; set; }
        
        #endregion Charting

        #endregion Properties

        public ShareDetailsForm( bool marketValueOverviewTabSelected,
            ShareObjectFinalValue shareObjectFinalValue, ShareObjectMarketValue shareObjectMarketValue,
            RichTextBox rchTxtBoxStateMessage, Logger logger,
            Language language, string languageName  )
        {
            InitializeComponent();

            // Set properties
            MarketValueOverviewTabSelected = marketValueOverviewTabSelected;

            #region ShareObjects 

            ShareObjectFinalValue = shareObjectFinalValue;
            ShareObjectMarketValue = shareObjectMarketValue;

            #endregion ShareObjets

            #region Logger

            RichTxtBoxStateMessage = rchTxtBoxStateMessage;
            Logger = logger;

            #endregion Logger

            #region Language

            Language = language;
            LanguageName = languageName;

            #endregion Language

            #region GrpBox for the details

            grpBoxShareDetails.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/Caption",
                LanguageName);

            tabCtrlDetails.TabPages[_tabPageDetailsChart].Text =
                Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Caption",
                LanguageName);
            lblDailyValuesSelection.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Selection",
                LanguageName);
            lblStartDate.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/StartDate",
                LanguageName);
            lblIntervalSelection.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Interval",
                LanguageName);
            lblAmount.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Amount",
                LanguageName);

            rdbClosingPrice.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/ClosingPrice",
                LanguageName);
            rdbOpeningPrice.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/OpeningPrice",
                LanguageName);
            rdbTop.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Top",
                LanguageName);
            rdbBottom.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Bottom",
                LanguageName);
            rdbVolume.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Volume",
                LanguageName);

            if (tabCtrlDetails.TabPages[_tabPageDetailsFinalValue] != null)
            {
                tabCtrlDetails.TabPages[_tabPageDetailsFinalValue].Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Caption",
                        LanguageName);
                lblDetailsFinalValueDate.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/Date",
                        LanguageName);
                lblDetailsFinalValueVolume.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/Volume",
                        LanguageName);
                lblDetailsFinalValueDividend.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/TotalDividend",
                        LanguageName);
                lblDetailsFinalValueBrokerage.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/TotalBrokerage",
                        LanguageName);
                lblDetailsFinaValueCurPrice.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/CurrentPrice",
                        LanguageName);
                lblDetailsFinalValueDiffPerformancePrev.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/DiffPerformancePrevDay", LanguageName);
                lblDetailsFinalValueDiffSumPrev.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/DiffSumPrevDay",
                        LanguageName);
                lblDetailsFinalValuePrevPrice.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/PricePrevDay",
                        LanguageName);
                lblDetailsPurchase.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/Purchase",
                        LanguageName);
                lblDetailsFinalValuePerformance.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/TotalPerformance",
                        LanguageName);
                lblDetailsFinalVinalTotalProfit.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/TotalProfitLoss",
                        LanguageName);
                lblDetailsFinalValueTotalSum.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithDividendBrokerage/Labels/TotalSum",
                        LanguageName);
            }

            if (tabCtrlDetails.TabPages[_tabPageDetailsMarketValue] != null)
            {
                tabCtrlDetails.TabPages[_tabPageDetailsMarketValue].Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Caption",
                        LanguageName);
                lblDetailsMarketValueDate.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/Date",
                        LanguageName);
                lblDetailsMarketValueVolume.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/Volume",
                        LanguageName);
                lblDetailsMarketValueDividend.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/TotalDividend",
                        LanguageName);
                lblDetailsMarketValueBrokerage.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/TotalBrokerage",
                        LanguageName);
                lblDetailsMarketValueCurPrice.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/CurrentPrice",
                        LanguageName);
                lblDetailsMarketValueDiffPerformancePrev.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/DiffPerformancePrevDay", LanguageName);
                lblDetailsMarketValueDiffSumPrev.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/DiffSumPrevDay",
                        LanguageName);
                lblDetailsMarketValuePrevPrice.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/PricePrevDay",
                        LanguageName);
                lblDetailsMarketValuePurchase.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/Purchase",
                        LanguageName);
                lblDetailsMarketValueTotalPerformance.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/TotalPerformance", LanguageName);
                lblDetailsMarketValueTotalProfit.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/TotalProfitLoss",
                        LanguageName);
                lblDetailsMarketValueTotalSum.Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgWithOutDividendBrokerage/Labels/TotalSum",
                        LanguageName);
            }

            if (tabCtrlDetails.TabPages[_tabPageDetailsDividendValue] != null)
            {
                tabCtrlDetails.TabPages[_tabPageDetailsDividendValue].Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", LanguageName);
                tabCtrlDetails.TabPages[_tabPageDetailsBrokerageValue].Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Caption", LanguageName);
                tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue].Text =
                    Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", LanguageName);
            }

            #endregion GrpBox for the details

            #region Set tab controls names

            _tabPageDetailsChart = tabCtrlDetails.TabPages[0].Name;
            _tabPageDetailsFinalValue = tabCtrlDetails.TabPages[1].Name;
            _tabPageDetailsMarketValue = tabCtrlDetails.TabPages[2].Name;
            _tabPageDetailsProfitLossValue = tabCtrlDetails.TabPages[3].Name;
            _tabPageDetailsDividendValue = tabCtrlDetails.TabPages[4].Name;
            _tabPageDetailsBrokerageValue = tabCtrlDetails.TabPages[5].Name;

            _tempFinalValues = tabCtrlDetails.TabPages[_tabPageDetailsFinalValue];
            _tempMarketValues = tabCtrlDetails.TabPages[_tabPageDetailsMarketValue];
            _tempDividends = tabCtrlDetails.TabPages[_tabPageDetailsDividendValue];
            _tempBrokerage = tabCtrlDetails.TabPages[_tabPageDetailsBrokerageValue];
            _tempProfitLoss = tabCtrlDetails.TabPages[_tabPageDetailsProfitLossValue];

            #endregion Set tab controls names

            #region Backgroundworker

            _parsingBackgroundWorker.WorkerReportsProgress = true;
            _parsingBackgroundWorker.WorkerSupportsCancellation = true;

            _parsingBackgroundWorker.DoWork += DailyValuesParsing;
            _parsingBackgroundWorker.ProgressChanged += OnDocumentParsingProgressChanged;
            _parsingBackgroundWorker.RunWorkerCompleted += OnDocumentParsingRunWorkerCompleted;

            #endregion Backgroundworker

            #region Web browser

            WebBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
            WebBrowser.ProgressChanged += WebBrowser_ProgressChanged;

            #endregion Web browser

            UpdateShareDetails(MarketValueOverviewTabSelected);
            UpdateProfitLossDetails(MarketValueOverviewTabSelected);
            UpdateBrokerageDetails(MarketValueOverviewTabSelected);
            UpdateDividendDetails(MarketValueOverviewTabSelected);

            // Check if daily values already exist
            int iAmount;
            if (MarketValueOverviewTabSelected)
            {
                Text = ShareObjectMarketValue.Name;
                iAmount = ShareObjectMarketValue.DailyValues.Count;
            }
            else
            {
                Text = ShareObjectFinalValue.Name;
                iAmount = ShareObjectFinalValue.DailyValues.Count;
            }

            // Set form and date time picker min / max values set
            if (iAmount > 0)
            {
                SetStartEndDateAtDateTimePicker();
                dateTimePickerStartDate.Enabled = true;
            }
            else
            {
                rdbClosingPrice.Enabled = false;
                rdbOpeningPrice.Enabled = false;
                rdbTop.Enabled = false;
                rdbBottom.Enabled = false;
                rdbVolume.Enabled = false;
                dateTimePickerStartDate.Enabled = false;
                cbxIntervalSelection.Enabled = false;
                numDrpDwnAmount.Enabled = false;
            }

            btnOpenWebSite.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Buttons/OpenWebSite", LanguageName);
            btnUpdateDailyValues.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Buttons/Update", LanguageName);
            btnOk.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Buttons/Ok", LanguageName);

            #region Settings

            // Select first tab control
            tabCtrlDetails.SelectedIndex = 0;

            // Select first interval
            cbxIntervalSelection.SelectedIndex = 0;

            #endregion Settings
        }

        private void ShareDetailsForm_Shown(object sender, EventArgs e)
        {
            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0);

            while (date.DayOfWeek == DayOfWeek.Sunday ||
                   date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(-1);
            }

            // Check if no update is necessary
            if (dateTimePickerStartDate.Value >= date && 
                ( ShareObjectFinalValue.DailyValues.Count > 0 || ShareObjectMarketValue.DailyValues.Count > 0) ) return;

            var strCaption = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/UpdateMsgBox/Caption",
                LanguageName);
            var strMessage = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/UpdateMsgBox/Message",
                LanguageName);
            if (ShareObjectFinalValue.DailyValues.Count == 0 || ShareObjectMarketValue.DailyValues.Count == 0)
            {
                strMessage += " ";
                strMessage += Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/UpdateMsgBox/FirstUpdate",
                    LanguageName);
            }

            var strOk = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/UpdateMsgBox/Yes",
                LanguageName);
            var strCancel = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/UpdateMsgBox/No",
                LanguageName);

            var updateOwnMessageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);
            var dlgResult = updateOwnMessageBox.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                btnUpdateDailyValues.PerformClick();
            }
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        #region Buttons

        /// <summary>
        /// This function starts the daily values update
        /// </summary>
        /// <param name="sender">Update button</param>
        /// <param name="e">Eventargs</param>
        private void BtnUpdateDailyValues_Click(object sender, EventArgs e)
        {
            // Disable controls
            rdbClosingPrice.Enabled = false;
            rdbOpeningPrice.Enabled = false;
            rdbTop.Enabled = false;
            rdbBottom.Enabled = false;
            rdbVolume.Enabled = false;
            dateTimePickerStartDate.Enabled = false;
            cbxIntervalSelection.Enabled = false;
            numDrpDwnAmount.Enabled = false;
            btnUpdateDailyValues.Enabled = false;
            btnOk.Enabled = false;

            // Check if the BackGroundWorker already is running
            if (_parsingBackgroundWorker.IsBusy)
            {
                _parsingBackgroundWorker.CancelAsync();

                rdbClosingPrice.Enabled = true;
                rdbOpeningPrice.Enabled = true;
                rdbTop.Enabled = true;
                rdbBottom.Enabled = true;
                rdbVolume.Enabled = true;
                dateTimePickerStartDate.Enabled = true;
                cbxIntervalSelection.Enabled = true;
                numDrpDwnAmount.Enabled = true;
                btnUpdateDailyValues.Enabled = true;
                btnOk.Enabled = true;
            }
            else
            {
                toolStripStatusLabelUpdate.ForeColor = Color.Black;
                toolStripStatusLabelUpdate.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartStateMessages/UpdateStarted", LanguageName);
                toolStripProgressBarUpdate.Visible = true;

                var strDailyValuesWebSite = "";

                // Check if any daily values already exists
                if (ShareObjectFinalValue.DailyValues.Count == 0)
                {
                    var date = DateTime.Now;
                    // Go five years back
                    date = date.AddYears(-5);
                    strDailyValuesWebSite = string.Format(ShareObjectFinalValue.DailyValuesWebSite, date.ToString("dd.MM.yyyy"), "5Y");
                }
                else
                {
                    // Get date of last daily values entry
                    var lastDate = ShareObjectFinalValue.DailyValues.Last().Date;

                    // Check if the days are less or equal than 27 days
                    var diffDays = DateTime.Now - lastDate;

                    if (diffDays.Days <= 27 )
                        strDailyValuesWebSite = string.Format(ShareObjectFinalValue.DailyValuesWebSite, DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy"), "1M");
                    else
                    {
                        var years = DateTime.Now.Year - lastDate.Year + 1;
                        if (years > 5)
                            years = 5;
                        
                        strDailyValuesWebSite = string.Format(ShareObjectFinalValue.DailyValuesWebSite, DateTime.Now.AddYears(-years).ToString("dd.MM.yyyy"), $"{years}Y");
                    }
                }

#if DEBUG
                Console.WriteLine(@"WebSite: {0}", strDailyValuesWebSite);
#endif
                // Navigate / load the daily values website
                WebBrowser.Navigate(strDailyValuesWebSite);
            }
        }

        /// <summary>
        /// This function close the details window
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Eventargs</param>
        private void OnBtnOk_Click(object sender, EventArgs e)
        {
            // Disconnect delegates
            _parsingBackgroundWorker.DoWork -= DailyValuesParsing;
            _parsingBackgroundWorker.ProgressChanged -= OnDocumentParsingProgressChanged;
            _parsingBackgroundWorker.RunWorkerCompleted -= OnDocumentParsingRunWorkerCompleted;

            WebBrowser.DocumentCompleted -= WebBrowser_DocumentCompleted;
            WebBrowser.ProgressChanged -= WebBrowser_ProgressChanged;

            Close();
        }

        #endregion Buttons

        #region Parsing

        /// <summary>
        /// This function reports the daily values website load process
        /// </summary>
        /// <param name="sender">WebBrowser</param>
        /// <param name="e">WebBrowser process change eventargs</param>
        private void WebBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            toolStripStatusLabelUpdate.ForeColor = Color.Black;
            toolStripStatusLabelUpdate.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartStateMessages/DownloadRunning", LanguageName);
        }

        /// <summary>
        /// This function reports the daily values website has been loaded and starts the parsing process
        /// </summary>
        /// <param name="sender">WebBrowser</param>
        /// <param name="e">WebBrowser document completed eventargs</param>
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                // Check if the BackGroundWorker is already running
                if (!_parsingBackgroundWorker.IsBusy)
                { 
                    // Start BackGroundWorker with the parsing process
                    _parsingBackgroundWorker.RunWorkerAsync(WebBrowser.Document);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error 1", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.UpdateFailed);
            }
        }

        /// <summary>
        /// This function parse the daily values from the loaded website
        /// </summary>
        /// <param name="sender">BackGroundWorker</param>
        /// <param name="e">Eventargs with the loaded website content</param>
        private void DailyValuesParsing(object sender, DoWorkEventArgs e)
        {
            try
            {
                toolStripStatusLabelUpdate.ForeColor = Color.Black;
                toolStripStatusLabelUpdate.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartStateMessages/DownloadDone", LanguageName);

                Thread.Sleep(2000);

                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingStarted);

                var htmlDocument = (HtmlDocument)e.Argument;
                // Get tables with the daily values which are tagged with "tbody"
                var tables = htmlDocument.GetElementsByTagName("tbody");

                // Check if the "tbody" tag has been found
                if (tables.Count > 0)
                {
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingProcess);

                    // Loop through the found tables
                    foreach (HtmlElement table in tables)
                    {
                        // Check if the current table has childs
                        if (table.All.Count > 0)
                        {
                            // Count the rows
                            for (var index = 0; index < table.All.Count; index++)
                            {
                                var row = table.All[index];
                                if (row.TagName == "TR")
                                {
                                }
                            }

                            // Loop through the childs
                            foreach (HtmlElement row in table.All)
                            {
                                var iCellCounter = 0;
                                var dailyValues = new DailyValues();

                                // Check if the child is a table row
                                if (row.TagName != "TR") continue;

                                // Check if the row has five elements
                                if (row.All.Count == 6)
                                {
                                    foreach (HtmlElement cell in row.All)
                                    {
                                        switch (iCellCounter)
                                        {
                                            case 0:
                                                dailyValues.Date = DateTime.Parse(cell.InnerHtml);
                                                break;
                                            case 1:
                                                dailyValues.OpeningPrice = decimal.Parse(cell.InnerHtml);
                                                break;
                                            case 2:
                                                dailyValues.Top = decimal.Parse(cell.InnerHtml);
                                                break;
                                            case 3:
                                                dailyValues.Bottom = decimal.Parse(cell.InnerHtml);
                                                break;
                                            case 4:
                                                dailyValues.ClosingPrice = decimal.Parse(cell.InnerHtml);
                                                break;
                                            case 5:
                                                dailyValues.Volume = decimal.Parse(cell.InnerHtml);
                                                break;
                                        }

                                        iCellCounter++;
                                    }

                                    // Only add if the date not exists already
                                    if (!ShareObjectFinalValue.DailyValues.Exists(x => x.Date.ToString() == dailyValues.Date.ToString()))
                                    {
                                        // Add new daily values to the list
                                        ShareObjectFinalValue.DailyValues.Add(dailyValues);
                                        ShareObjectFinalValue.DailyValues.Sort();
                                    }
                                }
                                else
                                {
                                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingFailed);
                                    return;
                                }

                                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingProcess);
                            }
                        }
                        else
                        {
                            _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingFailed);
                            return;
                        }
                    }

                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingFinished);
                }
                else
                {
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingFailed);
                }
            }
            catch (OperationCanceledException ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error 1", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingFailed);
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error 2", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCodes.ParsingFailed);
            }
        }

        private void OnDocumentParsingProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case (int)ParsingErrorCodes.ParsingStarted:
                    {

                        toolStripStatusLabelUpdate.ForeColor = Color.Black;
                        toolStripStatusLabelUpdate.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartStateMessages/ParsingStarted",
                                LanguageName);

                        toolStripProgressBarUpdate.Visible = true;

                        break;
                    }
                case (int)ParsingErrorCodes.ParsingProcess:
                    {
                        toolStripStatusLabelUpdate.ForeColor = Color.Black;
                        toolStripStatusLabelUpdate.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartStateMessages/ParsingProcess",
                                LanguageName);
                        break;
                    }
                case (int)ParsingErrorCodes.UpdateFailed:
                    {
                        toolStripStatusLabelUpdate.ForeColor = Color.Red;
                        toolStripStatusLabelUpdate.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartErrors/UpdateFailed",
                                LanguageName);

                        chartDailyValues.DataBind();

                        toolStripProgressBarUpdate.Visible = false;

                        btnUpdateDailyValues.Enabled = true;
                        btnOk.Enabled = true;

                        break;
                    }
                case (int)ParsingErrorCodes.ParsingFailed:
                    {
                        toolStripStatusLabelUpdate.ForeColor = Color.Red;
                        toolStripStatusLabelUpdate.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartErrors/ParsingFailed",
                                LanguageName);

                        chartDailyValues.DataBind();

                        toolStripProgressBarUpdate.Visible = false;

                        btnUpdateDailyValues.Enabled = true;
                        btnOk.Enabled = true;

                        break;
                    }
                case (int)ParsingErrorCodes.ParsingFinished:
                    {
                        toolStripStatusLabelUpdate.ForeColor = Color.Black;
                        toolStripStatusLabelUpdate.Text =
                            Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartStateMessages/ParsingFinished",
                                LanguageName);

                        chartDailyValues.DataBind();

                        toolStripProgressBarUpdate.Visible = false;

                        btnUpdateDailyValues.Enabled = true;
                        btnOk.Enabled = true;

                        break;
                    }
            }
        }

        private void OnDocumentParsingRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetStartEndDateAtDateTimePicker();

            rdbClosingPrice.Enabled = true;
            rdbOpeningPrice.Enabled = true;
            rdbTop.Enabled = true;
            rdbBottom.Enabled = true;
            rdbVolume.Enabled = true;
            dateTimePickerStartDate.Enabled = true;
            cbxIntervalSelection.Enabled = true;
            numDrpDwnAmount.Enabled = true;
            btnUpdateDailyValues.Enabled = true;
            btnOk.Enabled = true;
        }

        #endregion Parsing

        #region Charting

        private void Charting()
        {
            if (MarketValueOverviewTabSelected)
            {
                if (ShareObjectMarketValue.DailyValues.Count == 0)
                    return;
            }
            else
            {
                if (ShareObjectFinalValue.DailyValues.Count == 0)
                    return;
            }

            chartDailyValues.DataSource = null;
            decimal decMinValue = 0;
            decimal decMaxValue = 0;

            var dailyValuesList = new List<DailyValues>();

            #region Selection

            // Week
            if (cbxIntervalSelection.SelectedIndex == 0)
            {
                dailyValuesList = GetDailyValuesOfWeeks(dateTimePickerStartDate.Value, (int)numDrpDwnAmount.Value);
                GetMinMax(dailyValuesList, out decMinValue, out decMaxValue);
                chartDailyValues.DataSource = dailyValuesList;
            }

            // Month
            if (cbxIntervalSelection.SelectedIndex == 1)
            {
                dailyValuesList = GetDailyValuesOfWeeks(dateTimePickerStartDate.Value, (int)numDrpDwnAmount.Value * 4);
                GetMinMax(dailyValuesList, out decMinValue, out decMaxValue);
                chartDailyValues.DataSource = dailyValuesList;
            }

            // Quarter
            if (cbxIntervalSelection.SelectedIndex == 2)
            {
                dailyValuesList = GetDailyValuesOfWeeks(dateTimePickerStartDate.Value, (int)numDrpDwnAmount.Value * 4 * 3);
                GetMinMax(dailyValuesList, out decMinValue, out decMaxValue);
                chartDailyValues.DataSource = dailyValuesList;
            }

            // Year
            if (cbxIntervalSelection.SelectedIndex == 3)
            {
                dailyValuesList = GetDailyValuesOfWeeks(dateTimePickerStartDate.Value, (int)numDrpDwnAmount.Value * 52);
                GetMinMax(dailyValuesList, out decMinValue, out decMaxValue);
                chartDailyValues.DataSource = dailyValuesList;
            }

            #endregion Selection

#if DEBUG
            if (dailyValuesList.Count != 0)
            {
                var startDateTime = dailyValuesList.First().Date;
                var endDateTime = dailyValuesList.Last().Date;
                var diffDatetime = startDateTime - endDateTime;

                Console.WriteLine(@"Start: {0}", startDateTime.ToShortDateString());
                Console.WriteLine(@"End:   {0}", endDateTime.ToShortDateString());
                Console.WriteLine(@"Diff:  {0}", diffDatetime.Days);
            }
#endif
            // Clear chart
            chartDailyValues.ChartAreas.Clear();
            chartDailyValues.Series.Clear();

            if (dailyValuesList.Count <= 0) return;

            chartDailyValues.ChartAreas.Add("ChosenVales");
            chartDailyValues.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 9.00F);
            chartDailyValues.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 10.00F, FontStyle.Bold);

            chartDailyValues.ChartAreas[0].AxisY.Minimum = (int)decMinValue;
            chartDailyValues.ChartAreas[0].AxisY.Maximum = (int)decMaxValue;
            chartDailyValues.ChartAreas[0].AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.No;


            chartDailyValues.ChartAreas[0].AxisX.LabelStyle.Format = "dd.MM.yy";
            chartDailyValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chartDailyValues.ChartAreas[0].AxisX.IntervalOffset = 0;

            if (rdbClosingPrice.Checked)
            {
                _strUnit = ShareObjectFinalValue.CurrencyUnit;

                chartDailyValues.ChartAreas[0].AxisY.Title = "Price ( " + ShareObjectFinalValue.CurrencyUnit + " )";

                chartDailyValues.Series.Add("Series");
                chartDailyValues.Series["Series"].ChartType = SeriesChartType.Line;
                chartDailyValues.Series["Series"].XValueMember = DailyValues.DateName;
                chartDailyValues.Series["Series"].XValueType = ChartValueType.DateTime;
                chartDailyValues.Series["Series"].YValueMembers = DailyValues.ClosingPriceName;
                chartDailyValues.Series["Series"].YValueType = ChartValueType.Double;
            }

            if (rdbOpeningPrice.Checked)
            {
                _strUnit = ShareObjectFinalValue.CurrencyUnit;

                chartDailyValues.ChartAreas[0].AxisY.Title = "Price ( " + ShareObjectFinalValue.CurrencyUnit + " )";

                chartDailyValues.Series.Add("Series");
                chartDailyValues.Series["Series"].ChartType = SeriesChartType.Line;
                chartDailyValues.Series["Series"].XValueMember = DailyValues.DateName;
                chartDailyValues.Series["Series"].XValueType = ChartValueType.DateTime;
                chartDailyValues.Series["Series"].YValueMembers = DailyValues.OpeningPriceName;
                chartDailyValues.Series["Series"].YValueType = ChartValueType.Double;
            }

            if (rdbTop.Checked)
            {
                _strUnit = ShareObjectFinalValue.CurrencyUnit;

                chartDailyValues.ChartAreas[0].AxisY.Title = "Price ( " + ShareObjectFinalValue.CurrencyUnit + " )";

                chartDailyValues.Series.Add("Series");
                chartDailyValues.Series["Series"].ChartType = SeriesChartType.Line;
                chartDailyValues.Series["Series"].XValueMember = DailyValues.DateName;
                chartDailyValues.Series["Series"].XValueType = ChartValueType.DateTime;
                chartDailyValues.Series["Series"].YValueMembers = DailyValues.TopName;
                chartDailyValues.Series["Series"].YValueType = ChartValueType.Double;
            }

            if (rdbBottom.Checked)
            {
                _strUnit = ShareObjectFinalValue.CurrencyUnit;

                chartDailyValues.ChartAreas[0].AxisY.Title = "Price ( " + ShareObjectFinalValue.CurrencyUnit + " )";

                chartDailyValues.Series.Add("Series");
                chartDailyValues.Series["Series"].ChartType = SeriesChartType.Line;
                chartDailyValues.Series["Series"].XValueMember = DailyValues.DateName;
                chartDailyValues.Series["Series"].XValueType = ChartValueType.DateTime;
                chartDailyValues.Series["Series"].YValueMembers = DailyValues.BottomName;
                chartDailyValues.Series["Series"].YValueType = ChartValueType.Double;
            }

            if (rdbVolume.Checked)
            {
                _strUnit = ShareObject.PieceUnit;

                chartDailyValues.ChartAreas[0].AxisY.Title = "Volume ( " + ShareObject.PieceUnit + " )";

                chartDailyValues.Series.Add("Series");
                chartDailyValues.Series["Series"].ChartType = SeriesChartType.Line;
                chartDailyValues.Series["Series"].XValueMember = DailyValues.DateName;
                chartDailyValues.Series["Series"].XValueType = ChartValueType.DateTime;
                chartDailyValues.Series["Series"].YValueMembers = DailyValues.VolumeName;
                chartDailyValues.Series["Series"].YValueType = ChartValueType.Double;
            }
        }

        private void SetStartEndDateAtDateTimePicker()
        {
            if (MarketValueOverviewTabSelected)
            {
                dateTimePickerStartDate.MinDate = ShareObjectMarketValue.DailyValues.First().Date;
                dateTimePickerStartDate.MaxDate = ShareObjectMarketValue.DailyValues.Last().Date;
                dateTimePickerStartDate.Value = dateTimePickerStartDate.MaxDate;
            }
            else
            {
                dateTimePickerStartDate.MinDate = ShareObjectFinalValue.DailyValues.First().Date;
                dateTimePickerStartDate.MaxDate = ShareObjectFinalValue.DailyValues.Last().Date;
                dateTimePickerStartDate.Value = dateTimePickerStartDate.MaxDate;
            }
        }

        private List<DailyValues> GetDailyValuesOfWeeks(DateTime givenDateTime, int iAmount)
        {
            var iDays = 7;
            var calcDateTime = givenDateTime;
            var dateTimes = new List<DateTime>();

            // Fill with random int values
            var dailyValues = new List<DailyValues>();
            var dailyValuesResult = new List<DailyValues>();

            dailyValues.AddRange(MarketValueOverviewTabSelected
                ? ShareObjectMarketValue.DailyValues
                : ShareObjectFinalValue.DailyValues);

            do
            {
                //if (calcDateTime.DayOfWeek != DayOfWeek.Saturday && calcDateTime.DayOfWeek != DayOfWeek.Sunday )
                //{
                    dateTimes.Add(calcDateTime);
                //}

                calcDateTime = calcDateTime.AddDays(-1);
            } while (dateTimes.Count <= iDays * iAmount);

            foreach (var dateTime in dateTimes)
            {
                foreach (var dailyValue in dailyValues)
                {
                    if (dailyValue.Date == dateTime.Date)
                        dailyValuesResult.Add(dailyValue);

                    if (dailyValuesResult.Count >= iDays * iAmount) break;
                }
            }

            return dailyValuesResult;
        }

        private void GetMinMax(List<DailyValues> dailyValuesList, out decimal decMinValue, out decimal decMaxValue)
        {
            decMinValue = 0;
            decMaxValue = 0;

            #region Check which value should be shown

            if (rdbClosingPrice.Checked)
            {
                decMinValue = dailyValuesList.Min(x => x.ClosingPrice);
                decMaxValue = dailyValuesList.Max(x => x.ClosingPrice);
            }

            if (rdbOpeningPrice.Checked)
            {
                decMinValue = dailyValuesList.Min(x => x.OpeningPrice);
                decMaxValue = dailyValuesList.Max(x => x.OpeningPrice);
            }

            if (rdbTop.Checked)
            {
                decMinValue = dailyValuesList.Min(x => x.Top);
                decMaxValue = dailyValuesList.Max(x => x.Top);
            }

            if (rdbBottom.Checked)
            {
                decMinValue = dailyValuesList.Min(x => x.Bottom);
                decMaxValue = dailyValuesList.Max(x => x.Bottom);
            }

            if (rdbVolume.Checked)
            {
                decMinValue = dailyValuesList.Min(x => x.Volume);
                decMaxValue = dailyValuesList.Max(x => x.Volume);
            }

            #endregion Check which value should be shown

            // Round min value down and round max value up
            decMinValue = decimal.Floor(decMinValue);
            decMaxValue = decimal.Ceiling(decMaxValue);

            // Calculate difference between max and min value
            var minMaxDifference = (int)(decMaxValue - decMinValue);
            var iPlace = 0;

            // Get length of the difference value
            if (minMaxDifference == 0)
                iPlace = 1;
            else
            {
                while (minMaxDifference > 0)
                {
                    minMaxDifference /= 10;
                    iPlace++;
                }
            }

#if DEBUG
            Console.WriteLine(@"Place: {0}", iPlace);
            Console.WriteLine(@"Min: {0}", decMinValue);
            Console.WriteLine(@"Max: {0}", decMaxValue);
#endif

            if (iPlace < 3)
            {
                decMinValue -= 1;
                decMaxValue += 1;
            }
            else
            {
                var j = (int) Math.Pow(10, iPlace - 2);

                decMinValue -= j;
                decMaxValue += j;
            }

            // Check if the min value is lower than 0 so set it to 0
            if (decMinValue < 0) decMinValue = 0;

#if DEBUG
            Console.WriteLine(@"Min: {0}", decMinValue);
            Console.WriteLine(@"Max: {0}", decMaxValue);
#endif
        }

        public void OnChartMouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            const int tolerance = 5;

            if (PrevPosition.HasValue && pos == PrevPosition.Value) return;

            _tooltip.RemoveAll();

            PrevPosition = pos;

            var results = chartDailyValues.HitTest(pos.X, pos.Y, false,
                ChartElementType.DataPoint);

            foreach (var result in results)
            {
                if (result.ChartElementType != ChartElementType.DataPoint) continue;

                if (!(result.Object is DataPoint prop)) continue;

                var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                // check if the cursor is really close to the point (2 pixels around the point)
                if (Math.Abs(pos.X - pointXPixel) < tolerance &&
                    Math.Abs(pos.Y - pointYPixel) < tolerance)
                {
                    _tooltip.Show(prop.YValues[0] + " " + _strUnit, this.chartDailyValues,
                        pos.X, pos.Y - 15);
                }
            }
        }

        #endregion Charting

        #region Selection change

        private void OnRdbClosingPrice_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnRdbOpeningPrice_CheckedChanged(object sender, EventArgs e)
        {
            Charting();

        }

        private void OnRdbTop_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnRdbBottom_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnRdbVolume_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnDateTimePickerStartDate_ValueChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnCbxIntervalSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnNumDrpDwnAmount_ValueChanged(object sender, EventArgs e)
        {
            Charting();
        }

        #endregion Selection change

        private void btnOpenWebSite_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(MarketValueOverviewTabSelected
                    ? ShareObjectMarketValue.WebSite
                    : ShareObjectFinalValue.WebSite);
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
#if DEBUG
                    var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + noBrowser.Message;
                    MessageBox.Show(message, @"Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelUpdate,
                        Language.GetLanguageTextByXPath(
                            @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartErrors/NoBrowserInstalled", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelUpdate,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartErrors/OpenWebSiteFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }
    }
}
