using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SharePortfolioManager.ShareDetailsForm
{
    // Interval for charting values
    public enum ChartingInterval
    {
        Week = 0,
        Month = 1,
        Quarter = 2,
        Year = 3
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

        #region Charting

        internal int ChartingIntervalValue = (int) ChartingInterval.Month;

        internal int ChartingAmount = 1;

        internal ChartingDailyValues.ChartValues ChartValues;
        
        #endregion Charting

        #endregion Properties

        public ShareDetailsForm( bool marketValueOverviewTabSelected,
            ShareObjectFinalValue shareObjectFinalValue, ShareObjectMarketValue shareObjectMarketValue,
            RichTextBox rchTxtBoxStateMessage, Logger logger,
            Language language, string languageName)
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

            chkClosingPrice.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/ClosingPrice",
                LanguageName);
            chkOpeningPrice.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/OpeningPrice",
                LanguageName);
            chkTop.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Top",
                LanguageName);
            chkBottom.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Bottom",
                LanguageName);
            chkVolume.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Volume",
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
                chkClosingPrice.Enabled = false;
                chkOpeningPrice.Enabled = false;
                chkTop.Enabled = false;
                chkBottom.Enabled = false;
                chkVolume.Enabled = false;
                dateTimePickerStartDate.Enabled = false;
                cbxIntervalSelection.Enabled = false;
                numDrpDwnAmount.Enabled = false;
            }

            btnOpenWebSite.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Buttons/OpenWebSite", LanguageName);
            btnOk.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Buttons/Ok", LanguageName);

            #region Settings

            // Select first tab control
            tabCtrlDetails.SelectedIndex = 0;

            // Select first interval
            cbxIntervalSelection.SelectedIndex = ChartingIntervalValue;
            numDrpDwnAmount.Value = ChartingAmount;

            #endregion Settings
        }

        private void ShareDetailsForm_Shown(object sender, EventArgs e)
        {
            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0);

            var bFlag = false;

            // Check if the current day is a weekend day
            while (date.DayOfWeek == DayOfWeek.Sunday ||
                   date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(-1);
                bFlag = true;
            }

            if (!bFlag)
                date = date.AddDays(-1);

            // Check if no update is necessary
            // TODO Add check if the share is still active or update is activated
            if (dateTimePickerStartDate.Value >= date && 
                ( ShareObjectFinalValue.DailyValues.Count > 0 || ShareObjectMarketValue.DailyValues.Count > 0) ) return;

            toolStripStatusLabelUpdate.ForeColor = Color.Red;
            toolStripStatusLabelUpdate.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Messages/UpdatePossible", LanguageName);
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        #region Buttons

        /// <summary>
        /// This function opens the website of the share
        /// </summary>
        /// <param name="sender">Open button</param>
        /// <param name="e">Eventargs</param>
        private void OnBtnOpenWebSite_Click(object sender, EventArgs e)
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

        /// <summary>
        /// This function close the details window
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Eventargs</param>
        private void OnBtnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion Buttons

        #region Charting

        private void Charting()
        {
            #region Graph values creation

            // Create charting values settings
            var graphValuesList = new List<ChartingDailyValues.GraphValues>();

            // Check if the ClosingPrice is selected
            if (chkClosingPrice.CheckState == CheckState.Checked)
            {
                var graphValueClosingPrice = new ChartingDailyValues.GraphValues(
                    true,
                    SeriesChartType.Line,
                    2,
                    Color.Black,
                    DailyValues.DateName,
                    DailyValues.ClosingPriceName
                );
                graphValuesList.Add(graphValueClosingPrice);
            }

            // Check if the OpeningPrice is selected
            if (chkOpeningPrice.CheckState == CheckState.Checked)
            {
                var graphValueOpeningPrice = new ChartingDailyValues.GraphValues(
                    true,
                    SeriesChartType.Line,
                    2,
                    Color.DarkGreen,
                    DailyValues.DateName,
                    DailyValues.OpeningPriceName
                );
                graphValuesList.Add(graphValueOpeningPrice);
            }

            // Check if the Top is selected
            if (chkTop.CheckState == CheckState.Checked)
            {
                var graphValueTop = new ChartingDailyValues.GraphValues(
                    true,
                    SeriesChartType.Line,
                    2,
                    Color.DarkBlue,
                    DailyValues.DateName,
                    DailyValues.TopName
                );
                graphValuesList.Add(graphValueTop);
            }

            // Check if the Top is selected
            if (chkBottom.CheckState == CheckState.Checked)
            {
                var graphValueBottom = new ChartingDailyValues.GraphValues(
                    true,
                    SeriesChartType.Line,
                    2,
                    Color.DarkRed,
                    DailyValues.DateName,
                    DailyValues.BottomName
                );
                graphValuesList.Add(graphValueBottom);
            }

            // Check if the Volume is selected
            if (chkVolume.CheckState == CheckState.Checked)
            {
                var graphValueVolume = new ChartingDailyValues.GraphValues(
                    true,
                    SeriesChartType.Line,
                    2,
                    Color.Goldenrod,
                    DailyValues.DateName,
                    DailyValues.VolumeName
                );
                graphValuesList.Add(graphValueVolume);
            }

            #endregion Graph values creation

            // Check which interval is chosen
            ChartingInterval chartingInterval;
            switch (cbxIntervalSelection.SelectedIndex)
            {
                case (int)ChartingInterval.Week:
                {
                    chartingInterval = ChartingInterval.Week;
                } break;
                case (int)ChartingInterval.Month:
                {
                    chartingInterval = ChartingInterval.Month;
                } break;
                case (int)ChartingInterval.Quarter:
                {
                        chartingInterval = ChartingInterval.Quarter;
                } break;
                case (int)ChartingInterval.Year:
                {
                    chartingInterval = ChartingInterval.Year;
                } break;
                default:
                {
                    chartingInterval = ChartingInterval.Week;
                } break;
            }

            ChartValues =
                new ChartingDailyValues.ChartValues(
                    chartingInterval,
                    (int)numDrpDwnAmount.Value,
                    @"dd.MM.yy",
                    DateTimeIntervalType.Days,
                    graphValuesList);

            // Draw chart with the given values
            ChartingDailyValues.Charting(
                MarketValueOverviewTabSelected,
                ShareObjectFinalValue, ShareObjectMarketValue,
                Logger, LanguageName, Language,
                dateTimePickerStartDate.Value,
                chartDailyValues,
                ChartValues,
                null
            );
        }

        private void SetStartEndDateAtDateTimePicker()
        {
            if (MarketValueOverviewTabSelected)
            {
                if (ShareObjectMarketValue.DailyValues == null || ShareObjectMarketValue.DailyValues.Count <= 0) return;

                dateTimePickerStartDate.MinDate = ShareObjectMarketValue.DailyValues.First().Date;
                dateTimePickerStartDate.MaxDate = ShareObjectMarketValue.DailyValues.Last().Date;
                dateTimePickerStartDate.Value = dateTimePickerStartDate.MaxDate;
            }
            else
            {
                if (ShareObjectFinalValue.DailyValues == null || ShareObjectFinalValue.DailyValues.Count <= 0) return;

                dateTimePickerStartDate.MinDate = ShareObjectFinalValue.DailyValues.First().Date;
                dateTimePickerStartDate.MaxDate = ShareObjectFinalValue.DailyValues.Last().Date;
                dateTimePickerStartDate.Value = dateTimePickerStartDate.MaxDate;
            }
        }

        #endregion Charting

        #region Selection change

        private void OnChkClosingPrice_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnChkOpeningPrice_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnChkTop_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnChkBottom_CheckedChanged(object sender, EventArgs e)
        {
            Charting();
        }

        private void OnChkVolume_CheckedChanged(object sender, EventArgs e)
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
    }
}
