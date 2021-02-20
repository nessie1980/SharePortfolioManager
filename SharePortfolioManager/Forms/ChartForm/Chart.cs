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
//#define DEBUG_CHART

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Configurations;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.ChartForm
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

        public ChartingColors ChartingColorsDictionary;

        public string Title;

        #endregion Properties

        #region Share objects

        public ShareObjectMarketValue ShareObjectMarketValue;

        public ShareObjectFinalValue ShareObjectFinalValue;

        #endregion Share objects

        public FrmChart(bool marketValueOverviewTabSelected,
            ShareObjectFinalValue shareObjectFinalValue, ShareObjectMarketValue shareObjectMarketValue,
            RichTextBox rchTxtBoxStateMessage, Logger logger,
            Language language, string languageName,
            ChartingInterval iChartingInterval, int iChartingAmount, ChartingColors chartingColorsDictionary)
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
            ChartingColorsDictionary = chartingColorsDictionary;

            chartDailyValues.MouseWheel += OnChartDailyValues_MouseWheel;
        }

        private void FrmChart_Shown(object sender, EventArgs e)
        {
            Charting();

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
                (ShareObjectFinalValue.DailyValuesList.Entries.Count > 0 || ShareObjectMarketValue.DailyValuesList.Entries.Count > 0) &&
                ShareObjectFinalValue.DailyValuesList.Entries.Last().Date >= date) return;

            toolStripLabelMessage.ForeColor = Color.Red;
            toolStripLabelMessage.Text = Language.GetLanguageTextByXPath(@"/Chart/Messages/UpdatePossible", SettingsConfiguration.LanguageName);
        }

        #region Charting

        private void Charting()
        {
            try
            {
                var graphValuesList = new List<ChartingDailyValues.GraphValues>();

                var graphValueClosingPrice = new ChartingDailyValues.GraphValues(
                    true,
                    SeriesChartType.Line,
                    2,
                    ChartingColorsDictionary.GetClosingPriceColor(),
                    Parser.DailyValues.DateName,
                    Parser.DailyValues.ClosingPriceName
                );

                graphValuesList.Add(graphValueClosingPrice);

                var intervalType = DateTimeIntervalType.Days;

                if (ChartingAmount > 24)
                    intervalType = DateTimeIntervalType.Months;
                if (ChartingAmount > 48)
                    intervalType = DateTimeIntervalType.Years;

                var chartValues =
                    new ChartingDailyValues.ChartValues(
                        ChartingInterval.Month,
                        ChartingAmount,
                        @"dd.MM.yy",
                        intervalType,
                        graphValuesList);

                ChartingDailyValues.Charting(
                    out Title,
                    MarketValueOverviewTabSelected,
                    ShareObjectFinalValue, ShareObjectMarketValue,
                    Logger, SettingsConfiguration.LanguageName, Language,
                    DateTime.Now,
                    chartDailyValues,
                    chartValues,
                    lblNoDataMessage,
                    true,
                    ChartingColorsDictionary
                );
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripLabelMessage,
                    Language.GetLanguageTextByXPath(@"/Chart/Errors/ShowFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName, Color.DarkRed, Logger,
                    (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #endregion Charting

        private void OnChartDailyValues_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
        {
            try
            {
                var customItems = ((Chart) sender).Legends[0].CustomItems.Count;

                if (customItems <= 0) return;

                var numberOfAutoItems = e.LegendItems.Count - customItems;
                for (var i = 0; i < numberOfAutoItems; i++)
                {
                    e.LegendItems.RemoveAt(0);
                }

            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripLabelMessage,
                    Language.GetLanguageTextByXPath(@"/Chart/Errors/LegendCustomize", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName, Color.DarkRed, Logger,
                    (int) FrmMain.EStateLevels.FatalError, (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        #region Mouse wheel action

        private void OnChartDailyValues_MouseWheel(object sender, MouseEventArgs e)
        {
            // Calculate the scroll interval
            var iDelta = e.Delta / SystemInformation.MouseWheelScrollDelta;

            var iMonthAmount = MarketValueOverviewTabSelected
                ? ShareObjectMarketValue.DailyValuesList.MonthAmount
                : ShareObjectFinalValue.DailyValuesList.MonthAmount;

            // Check if the new amount is lower than 1
            if (ChartingAmount + iDelta < 2 || (ChartingAmount + iDelta) > iMonthAmount)
                return;
            
            // Calculate new chart interval
            ChartingAmount += iDelta;
             
            // Redraw chart
            Charting();
        }

        private void OnChartDailyValues_MouseLeave(object sender, EventArgs e)
        {
            chartDailyValues.MouseWheel -= OnChartDailyValues_MouseWheel;

            Close();
        }

        private void OnLblNoDataMessage_MouseLeave(object sender, EventArgs e)
        {
            chartDailyValues.MouseWheel -= OnChartDailyValues_MouseWheel;

            Close();
        }

        #endregion Mouse wheel action
    }
}
