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
//#define DEBUG_SHARE_DETAILS_FRM

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
using SharePortfolioManager.Classes.Configurations;

namespace SharePortfolioManager.ShareDetailsForm
{
    public partial class FrmShareDetails : Form
    {
        #region Variables

        /// <summary>
        /// State levels for the logging (e.g. Info)
        /// </summary>
        public enum EDetailsPageNumber
        {
            Chart = 0,
            ProfitLoss = 1,
            Dividend = 2,
            Brokerage = 3
        }

        /// <summary>
        /// Stores the name of the chart tab control
        /// </summary>
        private const string TabPageDetailsChartName = "tabPgDetailsChart";

        /// <summary>
        /// Stores the name of the complete depot value
        /// </summary>
        private const string TabPageDetailsFinalValueName = "tabPgDetailsFinalValue";

        /// <summary>
        /// Stores the name of the market value tab page control
        /// </summary>
        private const string TabPageDetailsMarketValueName = "tabPgDetailsMarketValue";

        /// <summary>
        /// Stores the name of the profit / loss value tab control
        /// </summary>
        private const string TabPageDetailsProfitLossValueName = "tabPgDetailsProfitLoss";

        /// <summary>
        /// Stores the name of the dividends tab control
        /// </summary>
        private const string TabPageDetailsDividendValueName = "tabPgDetailsDividends";

        /// <summary>
        /// Stores the name of the brokerage tab control
        /// </summary>
        private const string TabPageDetailsBrokerageValueName = "tabPgDetailsBrokerages";

        #region Status strips

        private readonly ToolStripStatusLabel _toolStripStatusLabelProfitLoss = new ToolStripStatusLabel()
        {
            Name = @"toolStripStatusLabelMessage"
        };

        private readonly ToolStripStatusLabel _toolStripStatusLabelDividend = new ToolStripStatusLabel()
        {
            Name = @"toolStripStatusLabelMessage"
        };

        private readonly ToolStripStatusLabel _toolStripStatusLabelBrokerage = new ToolStripStatusLabel()
        {
            Name = @"toolStripStatusLabelMessage"
        };

        #endregion Status strips

        #endregion Variables

        #region Properties

        public bool MarketValueOverviewTabSelected;

        public RichTextBox RichTxtBoxStateMessage;

        public Logger Logger;

        public ChartingColors ChartingColorsDictionary;

        public string LanguageName;

        public Language Language;

        #region Share objects

        public ShareObjectMarketValue ShareObjectMarketValue;

        public ShareObjectFinalValue ShareObjectFinalValue;

        #endregion Share objects

        #region Charting

        internal ChartingInterval ChartingIntervalValue;

        internal int ChartingAmount;

        internal ChartingDailyValues.ChartValues ChartValues;

        internal string Title;

        #endregion Charting

        #endregion Properties

        public FrmShareDetails( bool marketValueOverviewTabSelected,
            ShareObjectFinalValue shareObjectFinalValue, ShareObjectMarketValue shareObjectMarketValue,
            RichTextBox rchTxtBoxStateMessage, Logger logger,
            ChartingColors chartingColorsDictionary,
            ChartingInterval chartingInterval, int chartingAmount,
            Language language, string languageName)
        {
            InitializeComponent();

            // Set properties
            MarketValueOverviewTabSelected = marketValueOverviewTabSelected;

            #region Set tab page names

            tabPgDetailsChartValues.Name = TabPageDetailsChartName;
            tabPgDetailsFinalValue.Name = TabPageDetailsFinalValueName;
            tabPgDetailsMarketValue.Name = TabPageDetailsMarketValueName;
            tabPgDetailsProfitLossValues.Name = TabPageDetailsProfitLossValueName;
            tabPgDetailsDividendValues.Name = TabPageDetailsDividendValueName;
            tabPgDetailsBrokerageValues.Name = TabPageDetailsBrokerageValueName;

            #endregion Set tab page names

            #region ShareObjects 

            ShareObjectFinalValue = shareObjectFinalValue;
            ShareObjectMarketValue = shareObjectMarketValue;

            #endregion ShareObjets

            #region Charting

            ChartingColorsDictionary = chartingColorsDictionary;
            ChartingIntervalValue = chartingInterval;
            ChartingAmount = chartingAmount;

            #endregion Charting

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

            // Chart for the share
            if (tabCtrlShareDetails.TabPages[TabPageDetailsChartName] != null)
            {
                tabCtrlShareDetails.TabPages[TabPageDetailsChartName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Caption",
                        LanguageName);
                lblDailyValuesSelection.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Selection",
                    LanguageName);
                lblStartDate.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/StartDate",
                    LanguageName);
                lblIntervalSelection.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Interval",
                    LanguageName);
                lblAmount.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Amount",
                    LanguageName);
                chkClosingPrice.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/ClosingPrice",
                    LanguageName);
                chkOpeningPrice.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/OpeningPrice",
                    LanguageName);
                chkTop.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Top",
                    LanguageName);
                chkBottom.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Bottom",
                    LanguageName);
                chkVolume.Text = Language.GetLanguageTextByXPath(
                    @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChart/Volume",
                    LanguageName);
            }

            // Final value calculations overview
            if (tabCtrlShareDetails.TabPages[TabPageDetailsFinalValueName] != null)
            {
                tabCtrlShareDetails.TabPages[TabPageDetailsFinalValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Caption",
                        LanguageName);
                lblDetailsFinalValueDateValue.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Date",
                        LanguageName);

                #region Overall calcuation

                lblDetailsFinalValueOverallCalculation.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/Caption",
                        LanguageName);
                lblDetailsFinalValueTotalVolume.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/Volume",
                        LanguageName);
                lblDetailsFinalValueTotalCurPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/CurrentPrice",
                        LanguageName);
                lblDetailsFinalValueTotalPurchase.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/Purchase",
                        LanguageName);
                lblDetailsFinalValueTotalDividend.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/Dividend",
                        LanguageName);
                lblDetailsFinalValueTotalSale.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/Sale",
                        LanguageName);
                lblDetailsFinalValueTotalSum.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/Sum",
                        LanguageName);
                lblDetailsFinalValueTotalSalePurchase.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/SalePurchase",
                        LanguageName);
                lblDetailsFinalFinalTotalProfitLoss.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/ProfitLoss",
                        LanguageName);
                lblDetailsFinalValueTotalPerformance.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Total/Performance",
                        LanguageName);

                #endregion Overall calcuation

                #region Current calcuation

                lblDetailsFinalValueCurrentCalculation.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Current/Caption",
                        LanguageName);
                lblDetailsFinalValueCurrentVolume.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Current/Volume",
                        LanguageName);
                lblDetailsFinalValueCurrentCurPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Current/CurrentPrice",
                        LanguageName);
                lblDetailsFinalValueCurrentPurchase.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Current/Purchase",
                        LanguageName);
                lblDetailsFinalValueCurrentDividend.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Current/Dividend",
                        LanguageName);
                lblDetailsFinalValueCurrentProfitLossSale.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Current/ProfitLoss",
                        LanguageName);
                lblDetailsFinalValueCurrentSum.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/Current/Sum",
                        LanguageName);

                #endregion Current calcuation

                #region Previous day calcuation

                lblDetailsFinalValuePrevDayCalculation.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/Caption",
                        LanguageName);
                lblDetailsFinalValuePrevDayCurPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/CurrentPrice",
                        LanguageName);
                lblDetailsFinalValuePrevDayPrevPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/PrevPrice",
                        LanguageName);
                lblDetailsFinalValuePrevDayDiffPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/DiffPrice",
                        LanguageName);
                lblDetailsFinalValuePrevDayDiffPerformance.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/DiffPerformance",
                        LanguageName);
                lblDetailsFinalValuePrevDayVolume.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/Volume",
                        LanguageName);
                lblDetailsFinalValuePrevDayDiffValue.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/DiffPrice",
                        LanguageName);
                lblDetailsFinalValueDiffSumPrev.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgCompleteDepotValue/Labels/PrevDay/DiffSum",
                        LanguageName);

                #endregion Previous day calcuation
            }

            // Market value calculations overview
            if (tabCtrlShareDetails.TabPages[TabPageDetailsMarketValueName] != null)
            {
                tabCtrlShareDetails.TabPages[TabPageDetailsMarketValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Caption",
                        LanguageName);
                lblDetailsMarketValueDateValue.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Date",
                        LanguageName);

                #region Overall calcuation

                lblDetailsMarketValueOverallCalculation.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/Caption",
                        LanguageName);
                lblDetailsMarketValueTotalVolume.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/Volume",
                        LanguageName);
                lblDetailsMarketValueTotalCurPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/CurrentPrice",
                        LanguageName);
                lblDetailsMarketValueTotalPurchase.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/Purchase",
                        LanguageName);
                lblDetailsMarketValueTotalDividend.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/Dividend",
                        LanguageName);
                lblDetailsMarketValueTotalSale.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/Sale",
                        LanguageName);
                lblDetailsMarketValueTotalSum.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/Sum",
                        LanguageName);
                lblDetailsMarketValueTotalSalePurchase.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/SalePurchase",
                        LanguageName);
                lblDetailsMarketValueTotalProfitLoss.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/ProfitLoss",
                        LanguageName);
                lblDetailsMarketValueTotalPerformance.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Total/Performance",
                        LanguageName);

                #endregion Overall calcuation

                #region Current calcuation

                lblDetailsMarketValueCurrentCalculation.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Current/Caption",
                        LanguageName);
                lblDetailsMarketValueCurrentVolume.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Current/Volume",
                        LanguageName);
                lblDetailsMarketValueCurrentCurPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Current/CurrentPrice",
                        LanguageName);
                lblDetailsMarketValueCurrentPurchase.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Current/Purchase",
                        LanguageName);
                lblDetailsMarketValueCurrentDividend.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Current/Dividend",
                        LanguageName);
                lblDetailsMarketValueCurrentProfitLossSale.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Current/ProfitLoss",
                        LanguageName);
                lblDetailsMarketValueCurrentSum.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/Current/Sum",
                        LanguageName);

                #endregion Current calcuation

                #region Previous day calcuation

                lblDetailsMarketValuePrevDayCalculation.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/Caption",
                        LanguageName);
                lblDetailsMarketValuePrevDayCurPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/CurrentPrice",
                        LanguageName);
                lblDetailsMarketValuePrevDayPrevPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/PrevPrice",
                        LanguageName);
                lblDetailsMarketValuePrevDayDiffPrice.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/DiffPrice",
                        LanguageName);
                lblDetailsMarketValuePrevDayDiffPerformance.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/DiffPerformance",
                        LanguageName);
                lblDetailsMarketValuePrevDayVolume.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/Volume",
                        LanguageName);
                lblDetailsMarketValuePrevDayDiffValue.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/DiffPrice",
                        LanguageName);
                lblDetailsMarketValueDiffSumPrev.Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgMarketDepotValue/Labels/PrevDay/DiffSum",
                        LanguageName);

                #endregion Previous day calcuation
            }

            // Profit / loss overview for the share
            if (tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName] != null)
            {
                tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].BackColor = tabCtrlShareDetails.BackColor;
                tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Padding = new Padding(0, 8, 0, 0);

                tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgProfitLoss/Caption", SettingsConfiguration.LanguageName);

                tabCtrlShareDetails.TabPages[TabPageDetailsProfitLossValueName].Controls.Add(new StatusStrip()
                {
                    Name =  @"statusStripProfitLoss",
                    Dock = DockStyle.Bottom,
                    Items = { _toolStripStatusLabelProfitLoss }
                });
            }

            // Dividend overview for the share
            if (tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName] != null)
            {
                tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].BackColor = tabCtrlShareDetails.BackColor;
                tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Padding = new Padding(0, 8, 0, 0);

                tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgDividend/Caption", SettingsConfiguration.LanguageName);

                tabCtrlShareDetails.TabPages[TabPageDetailsDividendValueName].Controls.Add(new StatusStrip()
                {
                    Name = @"statusStripDividend",
                    Dock = DockStyle.Bottom,
                    Items = { _toolStripStatusLabelDividend }
                });
            }

            // Brokerage overview for the share
            if (tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName] != null)
            {
                tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].BackColor = tabCtrlShareDetails.BackColor;
                tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Padding = new Padding(0, 8, 0, 0);

                tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Text =
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgBrokerage/Caption", SettingsConfiguration.LanguageName);

                tabCtrlShareDetails.TabPages[TabPageDetailsBrokerageValueName].Controls.Add(new StatusStrip()
                {
                    Name = @"statusStripBrokerage",
                    Dock = DockStyle.Bottom,
                    Items = { _toolStripStatusLabelBrokerage }
                 });
            }

            #endregion GrpBox for the details

            #region Show or hide tab pages

            if (MarketValueOverviewTabSelected)
            {
                // Remove final value page
                if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsFinalValueName))
                    tabCtrlShareDetails.TabPages.RemoveByKey(TabPageDetailsFinalValueName);

                // Remove profit / loss value page
                if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsProfitLossValueName))
                    tabCtrlShareDetails.TabPages.RemoveByKey(TabPageDetailsProfitLossValueName);

                // Remove dividend value page
                if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsDividendValueName))
                    tabCtrlShareDetails.TabPages.RemoveByKey(TabPageDetailsDividendValueName);

                // Remove brokerage value page
                if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsBrokerageValueName))
                    tabCtrlShareDetails.TabPages.RemoveByKey(TabPageDetailsBrokerageValueName);
            }
            else
            {
                // Remove market value page
                if (tabCtrlShareDetails.TabPages.ContainsKey(TabPageDetailsMarketValueName))
                    tabCtrlShareDetails.TabPages.RemoveByKey(TabPageDetailsMarketValueName);
            }

            #endregion Show or hide tab pages

            UpdateShareDetails(MarketValueOverviewTabSelected);

            if (!MarketValueOverviewTabSelected)
            {
                UpdateProfitLossDetails();
                UpdateBrokerageDetails();
                UpdateDividendDetails();
            }

            // Check if daily values already exist
            int iAmount;
            if (MarketValueOverviewTabSelected)
            {
                Text = ShareObjectMarketValue.Name;
                iAmount = ShareObjectMarketValue.DailyValuesList.Entries.Count;
            }
            else
            {
                Text = ShareObjectFinalValue.Name;
                iAmount = ShareObjectFinalValue.DailyValuesList.Entries.Count;
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

            btnOpenWebSite.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Buttons/OpenWebSite", SettingsConfiguration.LanguageName);
            btnOk.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Buttons/Ok", SettingsConfiguration.LanguageName);

            #region Settings

            // Select first tab control
            tabCtrlShareDetails.SelectedIndex = 0;

            // Select first interval
            cbxIntervalSelection.SelectedIndex = (int)ChartingIntervalValue;
            if (chartingAmount <= numDrpDwnAmount.Maximum)
                numDrpDwnAmount.Value = ChartingAmount;

            #endregion Settings

            chartDailyValues.MouseWheel += OnChartDailyValues_MouseWheel;
        }

        private void ShareDetailsForm_Shown(object sender, EventArgs e)
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            var bFlag = false;

            // Check if the current day is a weekend day
            while (date.DayOfWeek == DayOfWeek.Sunday ||
                   date.DayOfWeek == DayOfWeek.Saturday ||
                   date.DayOfWeek == DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
                bFlag = true;
            }

            if (!bFlag)
                date = date.AddDays(-1);

            // Check if no update is necessary
            if (ShareObjectFinalValue.InternetUpdateOption == ShareObject.ShareUpdateTypes.MarketPrice &&
                 ShareObjectFinalValue.InternetUpdateOption == ShareObject.ShareUpdateTypes.None ||
                dateTimePickerStartDate.Value >= date && 
                ( ShareObjectFinalValue.DailyValuesList.Entries.Count > 0 || ShareObjectMarketValue.DailyValuesList.Entries.Count > 0) ) return;

            toolStripStatusLabelUpdate.ForeColor = Color.Red;
            toolStripStatusLabelUpdate.Text = Language.GetLanguageTextByXPath(@"/ShareDetailsForm/Messages/UpdatePossible", SettingsConfiguration.LanguageName);
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
                    ? ShareObjectMarketValue.DetailsWebSiteUrl
                    : ShareObjectFinalValue.DetailsWebSiteUrl);
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    Helper.AddStatusMessage(toolStripStatusLabelUpdate,
                        Language.GetLanguageTextByXPath(
                            @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartErrors/NoBrowserInstalled",
                            LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                        (int) FrmMain.EComponentLevels.Application,
                        noBrowser);
                }
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelUpdate,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/GrpBoxDetails/TabCtrlDetails/TabPgChartErrors/OpenWebSiteFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
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
            try
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
                        ChartingColorsDictionary.GetClosingPriceColor(),
                        Parser.DailyValues.DateName,
                        Parser.DailyValues.ClosingPriceName
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
                        ChartingColorsDictionary.GetOpeningPriceColor(),
                        Parser.DailyValues.DateName,
                        Parser.DailyValues.OpeningPriceName
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
                        ChartingColorsDictionary.GetTopPriceColor(),
                        Parser.DailyValues.DateName,
                        Parser.DailyValues.TopName
                    );
                    graphValuesList.Add(graphValueTop);
                }

                // Check if the Bottom is selected
                if (chkBottom.CheckState == CheckState.Checked)
                {
                    var graphValueBottom = new ChartingDailyValues.GraphValues(
                        true,
                        SeriesChartType.Line,
                        2,
                        ChartingColorsDictionary.GetBottomPriceColor(),
                        Parser.DailyValues.DateName,
                        Parser.DailyValues.BottomName
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
                        ChartingColorsDictionary.GetVolumeColor(),
                        Parser.DailyValues.DateName,
                        Parser.DailyValues.VolumeName
                    );
                    graphValuesList.Add(graphValueVolume);
                }

                #endregion Graph values creation

                // Check which interval is chosen
                ChartingInterval chartingInterval;
                switch (cbxIntervalSelection.SelectedIndex)
                {
                    case (int) ChartingInterval.Week:
                    {
                        chartingInterval = ChartingInterval.Week;
                    }
                        break;
                    case (int) ChartingInterval.Month:
                    {
                        chartingInterval = ChartingInterval.Month;
                    }
                        break;
                    case (int) ChartingInterval.Quarter:
                    {
                        chartingInterval = ChartingInterval.Quarter;
                    }
                        break;
                    case (int) ChartingInterval.Year:
                    {
                        chartingInterval = ChartingInterval.Year;
                    }
                        break;
                    default:
                    {
                        chartingInterval = ChartingInterval.Week;
                    }
                        break;
                }

                ChartValues =
                    new ChartingDailyValues.ChartValues(
                        chartingInterval,
                        (int) numDrpDwnAmount.Value,
                        @"dd.MM.yy",
                        DateTimeIntervalType.Days,
                        graphValuesList);

                // Draw chart with the given values
                ChartingDailyValues.Charting(
                    out Title,
                    MarketValueOverviewTabSelected,
                    ShareObjectFinalValue, ShareObjectMarketValue,
                    Logger, SettingsConfiguration.LanguageName, Language,
                    dateTimePickerStartDate.Value,
                    chartDailyValues,
                    ChartValues,
                    lblNoDataMessage,
                    false,
                    ChartingColorsDictionary
                );

                Text = Title;
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelUpdate,
                    Language.GetLanguageTextByXPath(
                        @"/ShareDetailsForm/Errors/SelectionChangeFailed",
                        LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        private void SetStartEndDateAtDateTimePicker()
        {
            if (MarketValueOverviewTabSelected)
            {
                if (ShareObjectMarketValue.DailyValuesList.Entries == null || ShareObjectMarketValue.DailyValuesList.Entries.Count <= 0) return;

                dateTimePickerStartDate.MinDate = ShareObjectMarketValue.DailyValuesList.Entries.First().Date;
                dateTimePickerStartDate.MaxDate = ShareObjectMarketValue.DailyValuesList.Entries.Last().Date;
                dateTimePickerStartDate.Value = dateTimePickerStartDate.MaxDate;
            }
            else
            {
                if (ShareObjectFinalValue.DailyValuesList.Entries == null || ShareObjectFinalValue.DailyValuesList.Entries.Count <= 0) return;

                dateTimePickerStartDate.MinDate = ShareObjectFinalValue.DailyValuesList.Entries.First().Date;
                dateTimePickerStartDate.MaxDate = ShareObjectFinalValue.DailyValuesList.Entries.Last().Date;
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
            // Recalculate the various amounts for the new start date
            if (MarketValueOverviewTabSelected)
            {
                ShareObjectMarketValue.DailyValuesList.UpdateAmounts(dateTimePickerStartDate.Value, ShareObjectMarketValue.DailyValuesList.Entries.First().Date);
            }
            else
            {
                ShareObjectFinalValue.DailyValuesList.UpdateAmounts(dateTimePickerStartDate.Value, ShareObjectFinalValue.DailyValuesList.Entries.First().Date);
            }

            Charting();
        }

        private void OnCbxIntervalSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChartingIntervalValue = (ChartingInterval)cbxIntervalSelection.SelectedIndex;
            ChartingAmount = 1;

            if(numDrpDwnAmount.Maximum > 0)
                numDrpDwnAmount.Value = 1;

            switch (ChartingIntervalValue)
            {
                case ChartingInterval.Week:
                {
                    numDrpDwnAmount.Maximum = MarketValueOverviewTabSelected ? ShareObjectMarketValue.DailyValuesList.WeekAmount : ShareObjectFinalValue.DailyValuesList.WeekAmount;
                } break;
                case ChartingInterval.Month:
                {
                    numDrpDwnAmount.Maximum = MarketValueOverviewTabSelected ? ShareObjectMarketValue.DailyValuesList.MonthAmount : ShareObjectFinalValue.DailyValuesList.MonthAmount;
                } break;
                case ChartingInterval.Quarter:
                {
                    numDrpDwnAmount.Maximum = MarketValueOverviewTabSelected ? ShareObjectMarketValue.DailyValuesList.QuarterAmount : ShareObjectFinalValue.DailyValuesList.QuarterAmount;
                } break;
                case ChartingInterval.Year:
                {
                    numDrpDwnAmount.Maximum = MarketValueOverviewTabSelected ? ShareObjectMarketValue.DailyValuesList.YearAmount : ShareObjectFinalValue.DailyValuesList.YearAmount;
                } break;
                default:
                {
                    numDrpDwnAmount.Maximum = MarketValueOverviewTabSelected ? ShareObjectMarketValue.DailyValuesList.WeekAmount : ShareObjectFinalValue.DailyValuesList.WeekAmount;
                } break;
            }

            Charting();
        }

        private void OnNumDrpDwnAmount_ValueChanged(object sender, EventArgs e)
        {
            Charting();
        }

        #endregion Selection change

        private void OnChartDailyValues_MouseWheel(object sender, MouseEventArgs e)
        {
            // Calculate the scroll interval
            var iDelta = e.Delta / SystemInformation.MouseWheelScrollDelta;

            switch (ChartingIntervalValue)
            {
                case ChartingInterval.Week:
                {
                    var iWeekAmount = MarketValueOverviewTabSelected
                        ? ShareObjectMarketValue.DailyValuesList.WeekAmount
                        : ShareObjectFinalValue.DailyValuesList.WeekAmount;

                    // Check if the new amount is lower than 1
                    if (ChartingAmount + iDelta < 1 || (ChartingAmount + iDelta) > iWeekAmount)
                        return;
                } break;
                case ChartingInterval.Month:
                {
                    var iMonthAmount = MarketValueOverviewTabSelected
                        ? ShareObjectMarketValue.DailyValuesList.MonthAmount
                        : ShareObjectFinalValue.DailyValuesList.MonthAmount;

                    // Check if the new amount is lower than 1
                    if (ChartingAmount + iDelta < 1 || (ChartingAmount + iDelta) > iMonthAmount)
                        return;

                } break;
                case ChartingInterval.Quarter:
                {
                    var iQuarterAmount = MarketValueOverviewTabSelected
                        ? ShareObjectMarketValue.DailyValuesList.QuarterAmount
                        : ShareObjectFinalValue.DailyValuesList.QuarterAmount;

                    // Check if the new amount is lower than 1
                    if (ChartingAmount + iDelta < 1 || (ChartingAmount + iDelta) > iQuarterAmount)
                        return;
                } break;
                case ChartingInterval.Year:
                {
                    var iYearAmount = MarketValueOverviewTabSelected
                        ? ShareObjectMarketValue.DailyValuesList.YearAmount
                        : ShareObjectFinalValue.DailyValuesList.YearAmount;

                    // Check if the new amount is lower than 1
                    if (ChartingAmount + iDelta < 1 || (ChartingAmount + iDelta) > iYearAmount)
                        return;
                } break;
                default:
                {
                    var iWeekAmount = MarketValueOverviewTabSelected
                        ? ShareObjectMarketValue.DailyValuesList.WeekAmount
                        : ShareObjectFinalValue.DailyValuesList.WeekAmount;

                    // Check if the new amount is lower than 1
                    if (ChartingAmount + iDelta < 1 || (ChartingAmount + iDelta) > iWeekAmount)
                        return;
                } break;
            }

            // Calculate new chart interval
            ChartingAmount += iDelta;

            // Set new amount to numeric up / down control
            numDrpDwnAmount.Value = ChartingAmount;
        }
    }
}
