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
//#define DEBUG_CHARTING_DAILY_VALUES

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes.ShareObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Double;

namespace SharePortfolioManager.Classes
{
    public static class ChartingDailyValues
    {
        #region Variables

        private static bool _marketValueOverviewTabSelected;

        private static ShareObjectMarketValue _shareObjectMarketValue;

        private static ShareObjectFinalValue _shareObjectFinalValue;

        private static Chart _chartDailyValues;

        private static ChartValues _chartValues;

        private static Legend _legendChart;

        // TODO: (thomas:2020-08-10) Currently not used and disabled
        //private static Logger _logger;

        private static string _languageName;

        private static Language _language;

        private static Color _colorBuyInformation;

        private static Color _colorSaleInformation;

        #endregion Variables

        #region Properties

        #endregion Properties

        #region Charting

        public static void Charting(
            out string text,
            bool marketValueOverviewTabSelected,
            ShareObjectFinalValue shareObjectFinalValue, ShareObjectMarketValue shareObjectMarketValue,
            Logger logger, string languageName, Language language,
            DateTime startDate,
            Chart chartDailyValues,
            ChartValues chartValues,
            System.Windows.Forms.Label lblBoxNoDataMessage,
            bool showTitle,
            Color buyInformationColor,
            Color saleInformationColor
            )
        {
            #region Set private values

            // First reset title then add new title
            text = marketValueOverviewTabSelected ? shareObjectMarketValue.Name : shareObjectFinalValue.Name;

            _marketValueOverviewTabSelected = marketValueOverviewTabSelected;

            _shareObjectMarketValue = shareObjectMarketValue;
            _shareObjectFinalValue = shareObjectFinalValue;

            // TODO: (thomas:2020-08-10) Currently not used and disabled
            //_logger = logger;
            _languageName = languageName;
            _language = language;

            _chartDailyValues = chartDailyValues;

            _chartValues = chartValues;

            _colorBuyInformation = buyInformationColor;
            _colorSaleInformation = saleInformationColor;

            #endregion Set private values

            // Clear chart
            foreach (var chartArea in _chartDailyValues.ChartAreas)
            {
                chartArea.AxisX.StripLines.Clear();
            }
            _chartDailyValues.ChartAreas.Clear();
            _chartDailyValues.Series.Clear();

            // Clear legend
            _chartDailyValues.Legends.Clear();

            // Check if no graph values have been given
            if (_chartValues?.GraphValuesList == null || _chartValues.GraphValuesList.Count <= 0)
            {
                // Check if a no "no data message label" has been given
                if (lblBoxNoDataMessage == null) return;

                // Set text to the no data message label
                lblBoxNoDataMessage.Text = _language.GetLanguageTextByXPath(@"/Chart/Errors/NoGraphDataGiven", _languageName);

                // Show no data message label
                lblBoxNoDataMessage.Visible = true;

                return;
            }

            if (_marketValueOverviewTabSelected && _shareObjectMarketValue.DailyValuesList.Entries.Count == 0 ||
                !_marketValueOverviewTabSelected && _shareObjectFinalValue.DailyValuesList.Entries.Count == 0
            )
            {
                // Check if a no "no data message label" has been given
                if (lblBoxNoDataMessage == null) return;

                // Set text to the no data message label
                lblBoxNoDataMessage.Text = Helper.BuildNewLineTextFromStringList(
                    _language.GetLanguageTextListByXPath(@"/Chart/Errors/NoData/Lines/*", _languageName));

                // Show no data message label
                lblBoxNoDataMessage.Visible = true;

                return;
            }

            _chartDailyValues.DataSource = null;

            #region Selection

            decimal decMinValueY = 0;
            decimal decMaxValueY = 0;
            decimal decMinValueY2 = 0;
            decimal decMaxValueY2= 0;

            // Set interval to the daily values list
            shareObjectMarketValue.DailyValuesList.Interval = chartValues.Interval;
            shareObjectFinalValue.DailyValuesList.Interval = chartValues.Interval;

            // Check if an Interval is selected and the amount is greater than 0
            var dailyValuesList = marketValueOverviewTabSelected
                ? shareObjectMarketValue.DailyValuesList.GetDailyValuesOfInterval(startDate, _chartValues.Amount)
                : shareObjectFinalValue.DailyValuesList.GetDailyValuesOfInterval(startDate, _chartValues.Amount);

            // Check if daily values are found for the given timespan
            if (dailyValuesList.Count <= 0)
            {
                // Check if a no "no data message label" has been given
                if (lblBoxNoDataMessage == null) return;

                // Set text to the no data message label
                lblBoxNoDataMessage.Text = Helper.BuildNewLineTextFromStringList(
                    _language.GetLanguageTextListByXPath(@"/Chart/Errors/NoData/Lines/*", _languageName));

                // Show no data message label
                lblBoxNoDataMessage.Visible = true;

                // Hide chart graph
                chartDailyValues.Visible = false;

                return;
            }

            if (dailyValuesList.Count > 0)
                GetMinMax(dailyValuesList, _chartValues.UseAxisY2 , out decMinValueY, out decMaxValueY, out decMinValueY2, out decMaxValueY2);

            _chartDailyValues.DataSource = dailyValuesList;

            #region Sale information

            var bShowSaleInformation = false;
            var bShowSaleStripLine = false;
            decimal decLastSalePrice = 0;
            var dateLastSalePrice = string.Empty;
            var iIndexSale = -1.0;
            var strSaleDate = "";
            var saleObject = marketValueOverviewTabSelected ? shareObjectMarketValue.GetLastAddedSale() : shareObjectFinalValue.GetLastAddedSale();

            if (saleObject != null)
            {
                bShowSaleInformation = true;
                decLastSalePrice = saleObject.SalePrice;
                dateLastSalePrice = saleObject.Date;

                // Check if the sale is in the daily values list
                if (dailyValuesList.Any(x => x.Date.ToShortDateString() == DateTime.Parse(dateLastSalePrice).ToShortDateString()))
                    bShowSaleStripLine = true;
            }

            if (dateLastSalePrice != @"")
            {
                strSaleDate = DateTime.Parse(DateTime.Parse(dateLastSalePrice).ToShortDateString()).ToShortDateString();
                iIndexSale =
                    dailyValuesList.FindIndex(x =>
                        x.Date == DateTime.Parse(DateTime.Parse(dateLastSalePrice).ToShortDateString())) +
                    1.009;
            }

            #endregion Sale information

            #region Buy information

            var bShowBuyInformation = false;
            var bShowBuyStripLine = false;
            decimal decLastBuyPrice = 0;
            var dateLastBuyPrice = string.Empty;
            var iIndexBuy = -1.0;
            var strBuyDate = "";
            var buyObject = marketValueOverviewTabSelected ? shareObjectMarketValue.GetLastAddedBuy() : shareObjectFinalValue.GetLastAddedBuy();

            if (buyObject != null)
            {
                bShowBuyInformation = true;
                decLastBuyPrice = buyObject.Price;
                dateLastBuyPrice = buyObject.Date;

                // Check if the sale is in the daily values list
                if (dailyValuesList.Any(x => x.Date.ToShortDateString() == DateTime.Parse(dateLastBuyPrice).ToShortDateString()))
                    bShowBuyStripLine = true;
            }

            if (dateLastBuyPrice != @"")
            {
                strBuyDate = DateTime.Parse(DateTime.Parse(dateLastBuyPrice).ToShortDateString()).ToShortDateString();
                iIndexBuy =
                    dailyValuesList.FindIndex(x =>
                        x.Date == DateTime.Parse(DateTime.Parse(dateLastBuyPrice).ToShortDateString())) +
                    1.009;
            }

            #endregion Buy information

            #endregion Selection

#if DEBUG_CHARTING_DAILY_VALUES
            if (dailyValuesList.Count > 0)
            {
                var startDateTime = dailyValuesList.First().Date;
                var endDateTime = dailyValuesList.Last().Date;
                var diffDatetime = startDateTime - endDateTime;

                Console.WriteLine(@"Start: {0}", startDateTime.ToShortDateString());
                Console.WriteLine(@"End:   {0}", endDateTime.ToShortDateString());
                Console.WriteLine(@"Diff:  {0}", diffDatetime.Days);
            }
#endif
            _chartDailyValues.ChartAreas.Add("ChosenValues");

            // Primary axis Y
            _chartDailyValues.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 9.00F);
            _chartDailyValues.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 10.00F, FontStyle.Bold);

            // Secondary axis Y
            _chartDailyValues.ChartAreas[0].AxisY2.TitleFont = new Font("Microsoft Sans Serif", 10.00F, FontStyle.Bold);
            _chartDailyValues.ChartAreas[0].AxisY2.TextOrientation = TextOrientation.Rotated270;

            // Set first AxisY values
            _chartDailyValues.ChartAreas[0].AxisY.Minimum = (int)decMinValueY;
            _chartDailyValues.ChartAreas[0].AxisY.Maximum = (int)decMaxValueY;

            if (_chartValues.UseAxisY2)
            {
                // Set second AxisY values
                _chartDailyValues.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                _chartDailyValues.ChartAreas[0].AxisY2.Minimum = (int) decMinValueY2;
                _chartDailyValues.ChartAreas[0].AxisY2.Maximum = (int) decMaxValueY2;
            }

            // Set X axis values
            _chartDailyValues.ChartAreas[0].AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.No;
            _chartDailyValues.ChartAreas[0].AxisX.LabelStyle.Format = _chartValues.DateTimeFormat;
            _chartDailyValues.ChartAreas[0].AxisX.IntervalType = _chartValues.IntervalType;
            _chartDailyValues.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            _chartDailyValues.ChartAreas[0].AxisX.IntervalOffset = 0;
            

            #region Legend

            _legendChart = new Legend(@"Chart");
            _chartDailyValues.Legends.Add(_legendChart);
            _chartDailyValues.Legends["Chart"].LegendStyle = LegendStyle.Table;
            _chartDailyValues.Legends["Chart"].Title = _language.GetLanguageTextByXPath(@"/Chart/Legend/Caption", _languageName);
            _chartDailyValues.Legends["Chart"].Font = new Font("Microsoft Sans Serif", 8.00F, FontStyle.Regular);

            _chartDailyValues.Legends[@"Chart"].Docking = Docking.Right;
            _chartDailyValues.Legends[@"Chart"].Alignment = StringAlignment.Near;

            _chartDailyValues.Legends[@"Chart"].BorderWidth = 1;
            _chartDailyValues.Legends[@"Chart"].BorderColor = Color.Black;
            _chartDailyValues.Legends[@"Chart"].TextWrapThreshold = 50;

            #endregion Legend

            foreach (var graphValues in _chartValues.GraphValuesList)
            {
                if (!graphValues.Show) continue;

                switch (graphValues.YValueMemberName)
                {
                    case Parser.DailyValues.ClosingPriceName:
                    {
                        #region ToolTip

                        var toolTipFormat =
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Date", _languageName) +
                        " #VALX{dd.MM.yyyy}" + "\n" +
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Value", _languageName) +
                        " #VAL{N2} " + _shareObjectFinalValue.CurrencyUnit;
                        
                        _chartDailyValues.ChartAreas[0].AxisY.Title = @"" 
                                                                  + _language.GetLanguageTextByXPath(@"/Chart/Legend/ClosingPriceYTitle", _languageName) + 
                                                                  " ( "
                                                                  +  _shareObjectFinalValue.CurrencyUnit +
                                                                  " )";
                        #endregion ToolTip

                        #region Series

                        _chartDailyValues.Series.Add(Parser.DailyValues.ClosingPriceName);
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].Color = graphValues.Color;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].XValueMember = Parser.DailyValues.DateName;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].IsXValueIndexed = true;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].YValueMembers = Parser.DailyValues.ClosingPriceName;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].MarkerSize = 6;
                        _chartDailyValues.Series[Parser.DailyValues.ClosingPriceName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/ClosingPrice", _languageName) + "\n" + toolTipFormat;

                        #endregion Series
                            
                        #region Legend chart

                        // Legend item creation
                        var legendChartItem1 = new LegendItem
                        {
                            SeriesName = Parser.DailyValues.ClosingPriceName,
                            Color = graphValues.Color,
                            Name = Parser.DailyValues.ClosingPriceName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCellChart = new LegendCell(LegendCellType.SeriesSymbol,
                            Parser.DailyValues.ClosingPriceName + "_FirstCell", ContentAlignment.MiddleRight);

                        // Legend second cell creation
                        var secondCellChart = new LegendCell(LegendCellType.Text,
                        string.Format(_language.GetLanguageTextByXPath(@"/Chart/Legend/ClosingPrice", _languageName) + "( " + _shareObjectFinalValue.CurrencyUnit + " )\n" +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Minimum", _languageName) +
                                      "{0} / " +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Maximum", _languageName) +
                                      "{1}", dailyValuesList.Aggregate((min, x) => x.ClosingPrice < min.ClosingPrice ? x : min).ClosingPrice, dailyValuesList.Aggregate((max, x) => x.ClosingPrice > max.ClosingPrice ? x : max).ClosingPrice),
                        ContentAlignment.MiddleLeft);

                        // Add cells to the legend item
                        legendChartItem1.Cells.Add(firstCellChart);
                        legendChartItem1.Cells.Add(secondCellChart);
                        var cells1Chart = legendChartItem1.Cells;
                        cells1Chart[0].Margins = new Margins(0, 30, 0, 30);
                        cells1Chart[1].Margins = new Margins(0, 30, 30, 0);

                        _chartDailyValues.Legends[0].CustomItems.Add(legendChartItem1);

                        #endregion Legend chart

                        } break;
                    case Parser.DailyValues.OpeningPriceName:
                    {
                        #region ToolTip

                        var toolTipFormat =
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Date", _languageName) +
                        " #VALX{dd.MM.yyyy}" + "\n" +
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Value", _languageName) +
                        " #VAL{N2} " + _shareObjectFinalValue.CurrencyUnit;

                        _chartDailyValues.ChartAreas[0].AxisY.Title = @""
                                                                      + _language.GetLanguageTextByXPath(@"/Chart/Legend/OpeningPriceYTitle", _languageName) +
                                                                      " ( "
                                                                      + _shareObjectFinalValue.CurrencyUnit +
                                                                      " )";
                        #endregion ToolTip

                        #region Series

                        _chartDailyValues.Series.Add(Parser.DailyValues.OpeningPriceName);
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].Color = graphValues.Color;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].XValueMember = Parser.DailyValues.DateName;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].IsXValueIndexed = true;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].YValueMembers = Parser.DailyValues.OpeningPriceName;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].MarkerSize = 6;
                        _chartDailyValues.Series[Parser.DailyValues.OpeningPriceName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/OpeningPrice", _languageName) + "\n" + toolTipFormat;

                        #endregion Series

                        #region Legend

                        // Legend item creation
                        var legendItem = new LegendItem
                        {
                            SeriesName = Parser.DailyValues.OpeningPriceName,
                            Color = graphValues.Color,
                            Name = Parser.DailyValues.OpeningPriceName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                            Parser.DailyValues.OpeningPriceName + "_FirstCell", ContentAlignment.MiddleRight);

                        // Legend second cell creation
                        var secondCell = new LegendCell(LegendCellType.Text,
                        string.Format(_language.GetLanguageTextByXPath(@"/Chart/Legend/OpeningPrice", _languageName) + "( " + _shareObjectFinalValue.CurrencyUnit + " )\n" +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Minimum", _languageName) +
                                      "{0} / " +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Maximum", _languageName) +
                                      "{1}", dailyValuesList.Aggregate((min, x) => x.OpeningPrice < min.OpeningPrice ? x : min).OpeningPrice, dailyValuesList.Aggregate((max, x) => x.OpeningPrice > max.OpeningPrice ? x : max).OpeningPrice),
                        ContentAlignment.MiddleLeft);

                        // Add cells to the legend item
                        legendItem.Cells.Add(firstCell);
                        legendItem.Cells.Add(secondCell);

                        var cells = legendItem.Cells;
                        cells[0].Margins = new Margins(0, 30, 0, 30);
                        cells[1].Margins = new Margins(0, 30, 30, 0);

                        _chartDailyValues.Legends[0].CustomItems.Add(legendItem);
                        
                        #endregion Legend
                    } break;
                    case Parser.DailyValues.TopName:
                    {
                        #region ToolTip

                        var toolTipFormat =
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Date", _languageName) +
                        " #VALX{dd.MM.yyyy}" + "\n" +
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Value", _languageName) +
                        " #VAL{N2} " + _shareObjectFinalValue.CurrencyUnit;

                        _chartDailyValues.ChartAreas[0].AxisY.Title = @""
                                                              + _language.GetLanguageTextByXPath(@"/Chart/Legend/TopYTitle", _languageName) +
                                                              " ( "
                                                              + _shareObjectFinalValue.CurrencyUnit +
                                                              " )";
                        #endregion ToolTip

                        #region Series

                        _chartDailyValues.Series.Add(Parser.DailyValues.TopName);
                        _chartDailyValues.Series[Parser.DailyValues.TopName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].Color = graphValues.Color;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].XValueMember = Parser.DailyValues.DateName;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].IsXValueIndexed = true;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].YValueMembers = Parser.DailyValues.TopName;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].MarkerSize = 6;
                        _chartDailyValues.Series[Parser.DailyValues.TopName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Top", _languageName) + "\n" + toolTipFormat;

                        #endregion Series

                        #region Legend

                        // Legend item creation
                        var legendItem = new LegendItem
                        {
                            SeriesName = Parser.DailyValues.TopName,
                            Color = graphValues.Color,
                            Name = Parser.DailyValues.TopName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                            Parser.DailyValues.TopName + "_FirstCell", ContentAlignment.MiddleRight);

                        // Legend second cell creation
                        var secondCell = new LegendCell(LegendCellType.Text,
                        string.Format(_language.GetLanguageTextByXPath(@"/Chart/Legend/Top", _languageName) + "( " + _shareObjectFinalValue.CurrencyUnit + " )\n" +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Minimum", _languageName) +
                                      "{0} / " +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Maximum", _languageName) +
                                      "{1}", dailyValuesList.Aggregate((min, x) => x.Top < min.Top ? x : min).Top, dailyValuesList.Aggregate((max, x) => x.Top > max.Top ? x : max).Top),
                        ContentAlignment.MiddleLeft);

                        // Add cells to the legend item
                        legendItem.Cells.Add(firstCell);
                        legendItem.Cells.Add(secondCell);

                        var cells = legendItem.Cells;
                        cells[0].Margins = new Margins(0, 30, 0, 30);
                        cells[1].Margins = new Margins(0, 30, 30, 0);

                        _chartDailyValues.Legends[0].CustomItems.Add(legendItem);

                        #endregion Legend
                    } break;
                    case Parser.DailyValues.BottomName:
                    {
                        #region ToolTip

                        var toolTipFormat =
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Date", _languageName) +
                        " #VALX{dd.MM.yyyy}" + "\n" +
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Value", _languageName) +
                        " #VAL{N2} " + _shareObjectFinalValue.CurrencyUnit;

                        _chartDailyValues.ChartAreas[0].AxisY.Title = @""
                                                              + _language.GetLanguageTextByXPath(@"/Chart/Legend/BottomYTitle", _languageName) +
                                                              " ( "
                                                              + _shareObjectFinalValue.CurrencyUnit +
                                                              " )";
                        #endregion ToolTip

                        #region Series

                        _chartDailyValues.Series.Add(Parser.DailyValues.BottomName);
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].Color = graphValues.Color;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].XValueMember = Parser.DailyValues.DateName;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].IsXValueIndexed = true;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].YValueMembers = Parser.DailyValues.BottomName;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].MarkerSize = 6;
                        _chartDailyValues.Series[Parser.DailyValues.BottomName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Bottom", _languageName) + "\n" + toolTipFormat;

                        #endregion Series

                        #region Legend

                        // Legend item creation
                        var legendItem = new LegendItem
                        {
                            SeriesName = Parser.DailyValues.BottomName,
                            Color = graphValues.Color,
                            Name = Parser.DailyValues.BottomName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                            Parser.DailyValues.BottomName + "_FirstCell", ContentAlignment.MiddleRight);

                        // Legend second cell creation
                        var secondCell = new LegendCell(LegendCellType.Text,
                        string.Format(_language.GetLanguageTextByXPath(@"/Chart/Legend/Bottom", _languageName) + "( " + _shareObjectFinalValue.CurrencyUnit + " )\n" +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Minimum", _languageName) +
                                      "{0} / " +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Maximum", _languageName) +
                                      "{1}", dailyValuesList.Aggregate((min, x) => x.Bottom < min.Bottom ? x : min).Bottom, dailyValuesList.Aggregate((max, x) => x.Bottom > max.Bottom ? x : max).Bottom),
                        ContentAlignment.MiddleLeft);

                        // Add cells to the legend item
                        legendItem.Cells.Add(firstCell);
                        legendItem.Cells.Add(secondCell);

                        var cells = legendItem.Cells;
                        cells[0].Margins = new Margins(0, 30, 0, 30);
                        cells[1].Margins = new Margins(0, 30, 30, 0);

                        _chartDailyValues.Legends[0].CustomItems.Add(legendItem);

                        #endregion Legend
                    } break;
                    case Parser.DailyValues.VolumeName:
                    {
                        #region ToolTip

                        var toolTipFormat =
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Date", _languageName) +
                        " #VALX{dd.MM.yyyy}" + "\n" +
                        _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Value", _languageName) +
                        " #VAL{N2} " + ShareObject.PieceUnit;

                        #endregion ToolTip

                        if (_chartValues.UseAxisY2)
                        {
                            #region Axis Y

                            _chartDailyValues.ChartAreas[0].AxisY2.Title = @""
                                                                       + _language.GetLanguageTextByXPath(
                                                                           @"/Chart/Legend/VolumeYTitle",
                                                                           _languageName) +
                                                                       " ( "
                                                                       + ShareObject.PieceUnit +
                                                                       " )";
                            #endregion Axis Y

                            #region Series

                            _chartDailyValues.Series.Add(Parser.DailyValues.VolumeName);
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].ChartType = graphValues.Type;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].BorderWidth = graphValues.BorderWidth;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].Color = graphValues.Color;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].XValueMember = Parser.DailyValues.DateName;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].XValueType = ChartValueType.DateTime;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].IsXValueIndexed = true;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].YValueMembers = Parser.DailyValues.VolumeName;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].YValueType = ChartValueType.Double;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].YAxisType = AxisType.Secondary;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].IsVisibleInLegend = false;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].MarkerStyle = MarkerStyle.Square;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].MarkerSize = 6;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Volume", _languageName) + "\n" + toolTipFormat;

                                #endregion Series

                            #region Legend

                            // Legend item creation
                            var legendItem = new LegendItem
                            {
                                SeriesName = Parser.DailyValues.VolumeName,
                                Color = graphValues.Color,
                                Name = Parser.DailyValues.VolumeName + "_Item"
                            };

                            // Legend first cell creation
                            var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                                Parser.DailyValues.VolumeName + "_FirstCell", ContentAlignment.MiddleRight);

                            // Legend second cell creation
                            var secondCell = new LegendCell(LegendCellType.Text,
                                string.Format(_language.GetLanguageTextByXPath(@"/Chart/Legend/Volume", _languageName) + "( " + ShareObject.PieceUnit + " )\n" +
                                              _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Minimum", _languageName) +
                                              "{0} / " +
                                              _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Maximum", _languageName) +
                                              "{1}", dailyValuesList.Aggregate((min, x) => x.Volume < min.Volume ? x : min).Volume, dailyValuesList.Aggregate((max, x) => x.Volume > max.Volume ? x : max).Volume),
                                ContentAlignment.MiddleLeft);

                            // Add cells to the legend item
                            legendItem.Cells.Add(firstCell);
                            legendItem.Cells.Add(secondCell);

                            var cells = legendItem.Cells;
                            cells[0].Margins = new Margins(0, 30, 0, 30);
                            cells[1].Margins = new Margins(0, 30, 30, 0);

                            _chartDailyValues.Legends[0].CustomItems.Add(legendItem);

                            #endregion Legend
                        }
                        else
                        {
                            #region Axis Y
                            _chartDailyValues.ChartAreas[0].AxisY.Title = @""
                                                                      + _language.GetLanguageTextByXPath(
                                                                          @"/Chart/Legend/VolumeYTitle",
                                                                          _languageName) +
                                                                      " ( "
                                                                      + ShareObject.PieceUnit +
                                                                      " )";
                            #endregion Axis Y

                            #region Series

                            _chartDailyValues.Series.Add(Parser.DailyValues.VolumeName);
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].ChartType = graphValues.Type;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].BorderWidth = graphValues.BorderWidth;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].Color = graphValues.Color;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].XValueMember = Parser.DailyValues.DateName;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].XValueType = ChartValueType.DateTime;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].IsXValueIndexed = true;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].YValueMembers = Parser.DailyValues.VolumeName;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].YValueType = ChartValueType.Double;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].YAxisType = AxisType.Primary;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].IsVisibleInLegend = false;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].MarkerStyle = MarkerStyle.Square;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].MarkerSize = 6;
                            _chartDailyValues.Series[Parser.DailyValues.VolumeName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Volume", _languageName) + "\n" + toolTipFormat;

                            #endregion Series

                            #region Legend

                            // Legend item creation
                            var legendItem = new LegendItem
                            {
                                SeriesName = Parser.DailyValues.VolumeName,
                                Color = graphValues.Color,
                                Name = Parser.DailyValues.VolumeName + "_Item"
                            };

                            // Legend first cell creation
                            var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                                Parser.DailyValues.VolumeName + "_FirstCell", ContentAlignment.MiddleRight);

                            // Legend second cell creation
                            var secondCell = new LegendCell(LegendCellType.Text,
                                string.Format(_language.GetLanguageTextByXPath(@"/Chart/Legend/Volume", _languageName) + "( " + ShareObject.PieceUnit + " )\n" +
                                              _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Minimum", _languageName) +
                                              "{0} / " +
                                              _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Maximum", _languageName) +
                                              "{1}", dailyValuesList.Aggregate((min, x) => x.Volume < min.Volume ? x : min).Volume, dailyValuesList.Aggregate((max, x) => x.Volume > max.Volume ? x : max).Volume),
                                ContentAlignment.MiddleLeft);

                            // Add cells to the legend item
                            legendItem.Cells.Add(firstCell);
                            legendItem.Cells.Add(secondCell);

                            var cells = legendItem.Cells;
                            cells[0].Margins = new Margins(0, 30, 0, 30);
                            cells[1].Margins = new Margins(0, 30, 30, 0);

                            _chartDailyValues.Legends[0].CustomItems.Add(legendItem);

                            #endregion Legend
                        }
                    } break;
                }
            }

            #region Strip lines

            var unit = _shareObjectFinalValue.CurrencyUnit;
            var firstPrice = dailyValuesList.First().ClosingPrice;
            var firstDate = dailyValuesList.First().Date.ToShortDateString();
            var lastPrice = dailyValuesList.Last().ClosingPrice;
            var lastDate = dailyValuesList.Last().Date.ToShortDateString();

            // Check if sale information should be shown
            if (bShowSaleInformation)
            {
                // Check if the strip line should be shown
                if (bShowSaleStripLine)
                {
                    var stripSaleHorizontal = new StripLine
                    {
                        Interval = 0,
                        IntervalOffset = (double) decLastSalePrice,
                        StripWidth = 0.0,
                        BorderWidth = 2,
                        BorderColor = _colorSaleInformation,
                        BackColor = _colorSaleInformation,
                        BorderDashStyle = ChartDashStyle.Solid
                    };
                    _chartDailyValues.ChartAreas["ChosenValues"].AxisY.StripLines.Add(stripSaleHorizontal);

                    var stripSaleVertical = new StripLine
                    {
                        Interval = 0,
                        IntervalType = DateTimeIntervalType.Days,
                        IntervalOffsetType = DateTimeIntervalType.Days,
                        IntervalOffset = iIndexSale,
                        StripWidth = 0.0,
                        BorderWidth = 2,
                        BorderColor = _colorSaleInformation,
                        BackColor = _colorSaleInformation,
                        BorderDashStyle = ChartDashStyle.Solid
                    };
                    _chartDailyValues.ChartAreas["ChosenValues"].AxisX.StripLines.Add(stripSaleVertical);
                }

                // Legend item creation
                var legendSaleItem = new LegendItem
                {
                    SeriesName = "Sale",
                    Color = _colorSaleInformation,
                    Name = "Sale_Item"
                };

                // Legend first cell creation
                var firstCellSale = new LegendCell(LegendCellType.SeriesSymbol,
                    "Sale_FirstCell", ContentAlignment.MiddleRight);

                // Legend second cell creation
                var differenceSalePrice = lastPrice - decLastSalePrice;
                var percentageSalePrice = (lastPrice * 100 / decLastSalePrice - 100).ToString(@"F");
                var secondCellSale = new LegendCell(LegendCellType.Text,
                    $@"{_language.GetLanguageTextByXPath(@"/Chart/Legend/LastSale", _languageName)}:\n{strSaleDate}: {decLastSalePrice}{unit}\n{lastPrice}{unit} - {decLastSalePrice}{unit} = {differenceSalePrice}{unit} ( {percentageSalePrice} % )",
                    ContentAlignment.MiddleLeft);

                if (Parse(percentageSalePrice) > 0.0)
                    secondCellSale.ForeColor = Color.Green;
                else if (Parse(percentageSalePrice) < 0.0)
                    secondCellSale.ForeColor = Color.Red;
                else
                    secondCellSale.ForeColor = Color.Black;

                legendSaleItem.Cells.Add(firstCellSale);
                legendSaleItem.Cells.Add(secondCellSale);
                var cells1BuySale = legendSaleItem.Cells;

                cells1BuySale[0].Margins = new Margins(80, 30, 0, 30);
                cells1BuySale[1].Margins = new Margins(80, 30, 30, 0);
                _chartDailyValues.Legends[@"Chart"].CustomItems.Add(legendSaleItem);
            }

            // Check if buy information should be shown
            if (bShowBuyInformation)
            {
                // Check if the strip line should be shown
                if (bShowBuyStripLine)
                {
                    var stripBuyHorizontal = new StripLine
                    {
                        Interval = 0,
                        IntervalOffset = (double) decLastBuyPrice,
                        StripWidth = 0.0,
                        BorderWidth = 2,
                        BorderColor = _colorBuyInformation,
                        BackColor = _colorBuyInformation,
                        BorderDashStyle = ChartDashStyle.Solid
                    };
                    _chartDailyValues.ChartAreas["ChosenValues"].AxisY.StripLines.Add(stripBuyHorizontal);

                    var stripBuyVertical = new StripLine
                    {
                        Interval = 0,
                        IntervalType = DateTimeIntervalType.Days,
                        IntervalOffsetType = DateTimeIntervalType.Days,
                        IntervalOffset = iIndexBuy,
                        StripWidth = 0.0,
                        BorderWidth = 2,
                        BorderColor = _colorBuyInformation,
                        BackColor = _colorBuyInformation,
                        BorderDashStyle = ChartDashStyle.Solid
                    };
                    _chartDailyValues.ChartAreas["ChosenValues"].AxisX.StripLines.Add(stripBuyVertical);
                }

                // Legend item creation
                var legendBuyItem = new LegendItem
                {
                    SeriesName = "Buy",
                    Color = _colorBuyInformation,
                    Name = "Buy_Item"
                };

                // Legend first cell creation
                var firstCellBuy = new LegendCell(LegendCellType.SeriesSymbol,
                    "Buy_FirstCell", ContentAlignment.MiddleRight);

                // Legend second cell creation
                var differenceBuyPrice = lastPrice - decLastBuyPrice;
                var percentageBuyPrice = (lastPrice * 100 / decLastBuyPrice - 100).ToString(@"F");
                var secondCellBuy = new LegendCell(LegendCellType.Text,
                    $@"{_language.GetLanguageTextByXPath(@"/Chart/Legend/LastBuy", _languageName)}:\n{strBuyDate}: {decLastBuyPrice}{unit}\n{lastPrice}{unit} - {decLastBuyPrice}{unit} = {differenceBuyPrice}{unit} ( {percentageBuyPrice} % )",
                    ContentAlignment.MiddleLeft);

                if (Parse(percentageBuyPrice) > 0.0)
                    secondCellBuy.ForeColor = Color.Green;
                else if (Parse(percentageBuyPrice) < 0.0)
                    secondCellBuy.ForeColor = Color.Red;
                else
                    secondCellBuy.ForeColor = Color.Black;

                legendBuyItem.Cells.Add(firstCellBuy);
                legendBuyItem.Cells.Add(secondCellBuy);
                var cellsBuy = legendBuyItem.Cells;
                if (bShowSaleInformation)
                {
                    cellsBuy[0].Margins = new Margins(0, 30, 0, 30);
                    cellsBuy[1].Margins = new Margins(0, 30, 30, 0);
                }
                else
                {
                    cellsBuy[0].Margins = new Margins(100, 30, 0, 30);
                    cellsBuy[1].Margins = new Margins(100, 30, 30, 0);
                }

                _chartDailyValues.Legends[@"Chart"].CustomItems.Add(legendBuyItem);
            }

            if (showTitle)
            {
                // Remove titles
                _chartDailyValues.Titles.Clear();

                // Legend second cell creation
                var differenceDevelopment = lastPrice - firstPrice;
                var percentageDevelopment = (lastPrice * 100 / firstPrice - 100).ToString(@"F");

                if (_marketValueOverviewTabSelected)
                {
                    _chartDailyValues.Titles.Add(
                        _shareObjectMarketValue.Name +
                        $@"\n{_language.GetLanguageTextByXPath(@"/Chart/Legend/Period", _languageName)}: {firstDate} - {lastDate} / {_language.GetLanguageTextByXPath(@"/Chart/Legend/Development", _languageName)}: {differenceDevelopment}{unit} ( {percentageDevelopment} % )"
                        );
                }
                else
                {
                    _chartDailyValues.Titles.Add(
                        _shareObjectMarketValue.Name +
                        $@"\n{_language.GetLanguageTextByXPath(@"/Chart/Legend/Period", _languageName)}: {firstDate} - {lastDate} / {_language.GetLanguageTextByXPath(@"/Chart/Legend/Development", _languageName)}: {differenceDevelopment}{unit} ( {percentageDevelopment} % )"
                    );
                }

                _chartDailyValues.Titles[0].Font = new Font("Microsoft Sans Serif", 10.00F, FontStyle.Bold);
            }
            else
            {
                // Legend second cell creation
                var differenceDevelopment = lastPrice - firstPrice;
                var percentageDevelopment = (lastPrice * 100 / firstPrice - 100).ToString(@"F");

                text +=
                    $@" - {_language.GetLanguageTextByXPath(@"/Chart/Legend/Period", _languageName)}: {firstDate} - {lastDate} / {_language.GetLanguageTextByXPath(@"/Chart/Legend/Development", _languageName)}: {differenceDevelopment}{unit} ( {percentageDevelopment} % )";
            }

            #endregion Strip lines
        }

        #endregion Charting

        #region Helper functions

        private static void GetMinMax(
            IReadOnlyCollection<Parser.DailyValues> dailyValuesList,
            bool bUseAxisY2,
            out decimal decMinValueY,
            out decimal decMaxValueY,
            out decimal decMinValueY2,
            out decimal decMaxValueY2
            )
        {
            decMinValueY = decimal.MaxValue;
            decMaxValueY = decimal.MinValue;
            decMinValueY2 = decimal.MaxValue;
            decMaxValueY2 = decimal.MinValue;

            #region Check which value should be shown

            foreach (var graphValues in _chartValues.GraphValuesList)
            {
                if (!graphValues.Show) continue;

                decimal decMin;
                decimal decMax;

                switch (graphValues.YValueMemberName)
                {
                    case Parser.DailyValues.ClosingPriceName:
                    {
                        decMin = dailyValuesList.Min(x => x.ClosingPrice);
                        decMax = dailyValuesList.Max(x => x.ClosingPrice);
                    } break;
                    case Parser.DailyValues.OpeningPriceName:
                    {
                        decMin = dailyValuesList.Min(x => x.OpeningPrice);
                        decMax = dailyValuesList.Max(x => x.OpeningPrice);
                    } break;
                    case Parser.DailyValues.TopName:
                    {
                        decMin = dailyValuesList.Min(x => x.Top);
                        decMax = dailyValuesList.Max(x => x.Top);
                    } break;
                    case Parser.DailyValues.BottomName:
                    {
                        decMin = dailyValuesList.Min(x => x.Bottom);
                        decMax = dailyValuesList.Max(x => x.Bottom);
                    } break;
                    case Parser.DailyValues.VolumeName:
                    {
                        decMin = dailyValuesList.Min(x => x.Volume);
                        decMax = dailyValuesList.Max(x => x.Volume);
                    } break;
                    default:
                    {
                        decMin = dailyValuesList.Min(x => x.ClosingPrice);
                        decMax = dailyValuesList.Max(x => x.ClosingPrice);
                    } break;
                }

                // Check if two Y axis should be used
                if (bUseAxisY2)
                {
                    if (graphValues.YValueMemberName == Parser.DailyValues.VolumeName)
                    {
                        // Set Min / Max value
                        if (decMin < decMinValueY2)
                            decMinValueY2 = decMin;

                        if (decMax > decMaxValueY2)
                            decMaxValueY2 = decMax;

                        continue;
                    }
                }

                // Set Min / Max value
                if (decMin < decMinValueY)
                    decMinValueY = decMin;

                if (decMax > decMaxValueY)
                    decMaxValueY = decMax;
            }

            #endregion Check which value should be shown

            // Round min value down and round max value up
            decMinValueY = decimal.Floor(decMinValueY);
            decMaxValueY = decimal.Ceiling(decMaxValueY);

            // Calculate difference between max and min value
            var minMaxDifference = (int)(decMaxValueY - decMinValueY);
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

#if DEBUG_CHARTING_DAILY_VALUES
            Console.WriteLine(@"Place: {0}", iPlace);
            Console.WriteLine(@"Min: {0}", decMinValueY);
            Console.WriteLine(@"Max: {0}", decMaxValueY);
#endif

            if (iPlace < 3)
            {
                decMinValueY -= 1;
                decMaxValueY += 1;
            }
            else
            {
                var j = (int)Math.Pow(10, iPlace - 2);

                decMinValueY -= j;
                decMaxValueY += j;
            }

            // Check if the min value is lower than 0 so set it to 0
            if (decMinValueY < 0) decMinValueY = 0;

#if DEBUG_CHARTING_DAILY_VALUES
            Console.WriteLine(@"Min: {0}", decMinValueY);
            Console.WriteLine(@"Max: {0}", decMaxValueY);
#endif
        }

        #endregion Helper functions

        #region Graphic values structure

        /// <summary>
        /// This class stores values for the chart settings
        /// </summary>
        public class ChartValues
        {
            public ChartingInterval Interval;
            public int Amount;
            public string DateTimeFormat;
            public DateTimeIntervalType IntervalType;
            public bool UseAxisY2;
            public List<GraphValues> GraphValuesList { get; set; }

            public ChartValues(ChartingInterval interval, int amount, string dateTimeFormat, DateTimeIntervalType intervalType, List<GraphValues> graphValues)
            {
                Interval = interval;
                Amount = amount;
                DateTimeFormat = dateTimeFormat;
                IntervalType = intervalType;
                GraphValuesList = graphValues;

                // Check if more than one graph should be shown and if one of the graphs is the volume graph
                if (GraphValuesList.Count(x => x.Show) > 1 && GraphValuesList.Any(x => x.YValueMemberName == Parser.DailyValues.VolumeName))
                    UseAxisY2 = true;
            }
        }

        /// <summary>
        /// This structure stores values for a graph settings
        /// </summary>
        public struct GraphValues
        {
            public bool Show;
            public SeriesChartType Type;
            public int BorderWidth;
            public Color Color;
            public string XValueMemberName;
            public string YValueMemberName;

            public GraphValues(bool show, SeriesChartType type, int borderWidth, Color color, string xValueMemberName, string yValueMemberName)
            {
                Show = show;
                Type = type;
                BorderWidth = borderWidth;
                Color = color;
                XValueMemberName = xValueMemberName;
                YValueMemberName = yValueMemberName;
            }
        }

        #endregion Graphic values structure
    }
}
