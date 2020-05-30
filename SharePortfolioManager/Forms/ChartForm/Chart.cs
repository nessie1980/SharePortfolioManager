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
//#define DEBUG_CHART

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.ShareDetailsForm;

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

        public string Title;

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
            try
            {
                var graphValuesList = new List<ChartingDailyValues.GraphValues>();

                var graphValueClosingPrice = new ChartingDailyValues.GraphValues(
                    true,
                    SeriesChartType.Line,
                    2,
                    // TODO use settings value
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
                    out Title,
                    MarketValueOverviewTabSelected,
                    ShareObjectFinalValue, ShareObjectMarketValue,
                    Logger, LanguageName, Language,
                    DateTime.Now,
                    chartDailyValues,
                    chartValues,
                    lblNoDataMessage,
                    true,
                    // TODO use settings value
                    Color.Blue,
                    Color.Red
                );
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripLabelMessage,
                    Language.GetLanguageTextByXPath(@"/Chart/Errors/ShowFailed", LanguageName),
                    Language, LanguageName, Color.DarkRed, Logger,
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
                    Language.GetLanguageTextByXPath(@"/Chart/Errors/LegendCustomize", LanguageName),
                    Language, LanguageName, Color.DarkRed, Logger,
                    (int) FrmMain.EStateLevels.FatalError, (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        private void OnChartDailyValues_MouseLeave(object sender, EventArgs e)
        {
            Close();
        }

        private void OnLblNoDataMessage_MouseLeave(object sender, EventArgs e)
        {
            Close();
        }
    }
}
