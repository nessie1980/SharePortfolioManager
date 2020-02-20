using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.ShareDetailsForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SharePortfolioManager.Chart
{
    public partial class FrmChart : Form
    {
        #region Properties

        public bool MarketValueOverviewTabSelected;

        public RichTextBox RichTxtBoxStateMessage;

        public Logger Logger;

        public string LanguageName;

        public Language Language;

        public ChartingInterval ChartingInterval;

        public int ChartingAmount;

        #endregion Properties

        #region Share objects

        public ShareObjectMarketValue ShareObjectMarketValue;

        public ShareObjectFinalValue ShareObjectFinalValue;

        #endregion Share objects

        public FrmChart(bool marketValueOverviewTabSelected,
            ShareObjectFinalValue shareObjectFinalValue, ShareObjectMarketValue shareObjectMarketValue,
            RichTextBox rchTxtBoxStateMessage, Logger logger,
            Language language, string languageName, ChartingInterval iChartingInterval, int iChartingAmount)
        {
            InitializeComponent();

            MarketValueOverviewTabSelected = marketValueOverviewTabSelected;

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;

            RichTxtBoxStateMessage = rchTxtBoxStateMessage;
            Logger = logger;

            Language = language;
            LanguageName = languageName;

            ChartingInterval = iChartingInterval;
            ChartingAmount = iChartingAmount;

            Charting();
        }

        #region Charting

        private void Charting()
        {
            var graphValuesList = new List<ChartingDailyValues.GraphValues>();

            var graphValueClosingPrice = new ChartingDailyValues.GraphValues(
                true,
                SeriesChartType.Line,
                2,
                Color.Black,
                DailyValues.DateName,
                DailyValues.ClosingPriceName
            );

            graphValuesList.Add(graphValueClosingPrice);

            var chartValues =
                new ChartingDailyValues.ChartValues(
                    // TODO Radiobox or Settings.xml
                    ChartingInterval.Month,
                    1,
                    @"dd.MM.yy",
                    DateTimeIntervalType.Days,
                    graphValuesList);

            ChartingDailyValues.Charting(
                MarketValueOverviewTabSelected,
                ShareObjectFinalValue, ShareObjectMarketValue,
                Logger, LanguageName, Language,
                DateTime.Now,
                chartDailyValues,
                chartValues,
                lblNoDataMessage
            );

        }

        #endregion Charting

        private void OnChartDailyValues_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
        {
            var customItems = ((System.Windows.Forms.DataVisualization.Charting.Chart)sender).Legends[0].CustomItems.Count;

            if (customItems > 0)
            {
                var numberOfAutoItems = e.LegendItems.Count - customItems;
                for (var i = 0; i < numberOfAutoItems; i++)
                {
                    e.LegendItems.RemoveAt(0);
                }
            }
        }

        private void OnLblNoDataMessage_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnChartDailyValues_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
