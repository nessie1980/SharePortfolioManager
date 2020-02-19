using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.ShareDetailsForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace SharePortfolioManager.Classes
{
    public static class ChartingDailyValues
    {
        #region Variables

        private static bool _marketValueOverviewTabSelected;

        private static ShareObjectMarketValue _shareObjectMarketValue;

        private static ShareObjectFinalValue _shareObjectFinalValue;

        private static System.Windows.Forms.DataVisualization.Charting.Chart _chartDailyValues;

        private static ChartValues _chartValues;

        private static Legend _legend;

        private static Logger _logger;

        private static string _languageName;

        private static Language _language;

        #endregion Variables

        #region Charting

        public static void Charting(
            bool marketValueOverviewTabSelected,
            ShareObjectFinalValue shareObjectFinalValue, ShareObjectMarketValue shareObjectMarketValue,
            Logger logger, string languageName, Language language,
            DateTime startDate,
            System.Windows.Forms.DataVisualization.Charting.Chart chartDailyValues,
            ChartValues chartValues
            )
        {
            #region Set private values

            _marketValueOverviewTabSelected = marketValueOverviewTabSelected;

            _shareObjectMarketValue = shareObjectMarketValue;
            _shareObjectFinalValue = shareObjectFinalValue;

            _logger = logger;
            _languageName = languageName;
            _language = language;

            _chartDailyValues = chartDailyValues;

            _chartValues = chartValues;

            #endregion Set private values

            // Clear chart
            _chartDailyValues.ChartAreas.Clear();
            _chartDailyValues.Series.Clear();

            // Clear legend
            _chartDailyValues.Legends.Clear();

            // Check if no graph values have been given
            if ( _chartValues.GraphValuesList.Count <= 0) return;

            if (_marketValueOverviewTabSelected)
            {
                if (_shareObjectMarketValue.DailyValues.Count == 0)
                    return;
            }
            else
            {
                if (_shareObjectFinalValue.DailyValues.Count == 0)
                    return;
            }

            _chartDailyValues.DataSource = null;

            #region Selection

            decimal decMinValueY = 0;
            decimal decMaxValueY = 0;
            decimal decMinValueY2 = 0;
            decimal decMaxValueY2= 0;

            // Check if an Interval is selected and the amount is greater than 0
            var dailyValuesList = GetDailyValuesOfInterval(startDate, _chartValues.Amount);

            // Check if daily values are found for the given timespan
            if (dailyValuesList.Count <= 0) return;

            if (dailyValuesList.Count > 0)
                GetMinMax(dailyValuesList, _chartValues.UseAxisY2 , out decMinValueY, out decMaxValueY, out decMinValueY2, out decMaxValueY2);

            _chartDailyValues.DataSource = dailyValuesList;

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
            _chartDailyValues.ChartAreas[0].AxisX.IntervalOffset = 0;

            _legend = new Legend(@"Chart");
            _chartDailyValues.Legends.Add(_legend);
            _chartDailyValues.Legends["Chart"].LegendStyle = LegendStyle.Table;
            _chartDailyValues.Legends[0].Title = _language.GetLanguageTextByXPath(@"/Chart/Legend/Caption", _languageName);
            _chartDailyValues.Legends[0].Font = new Font("Microsoft Sans Serif", 8.00F, FontStyle.Regular);

            foreach (var graphValues in _chartValues.GraphValuesList)
            {
                if (!graphValues.Show) continue;

                switch (graphValues.YValueMemberName)
                {
                    case DailyValues.ClosingPriceName:
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

                        _chartDailyValues.Series.Add(DailyValues.ClosingPriceName);
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].Color = graphValues.Color;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].XValueMember = DailyValues.DateName;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].YValueMembers = DailyValues.ClosingPriceName;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].MarkerSize = 6;
                        _chartDailyValues.Series[DailyValues.ClosingPriceName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/ClosingPrice", _languageName) + "\n" + toolTipFormat;

                        #endregion Series

                        #region Legend

                        // Legend item creation
                        var legendItem = new LegendItem
                        {
                            SeriesName = DailyValues.ClosingPriceName,
                            Color = graphValues.Color,
                            Name = DailyValues.ClosingPriceName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                            DailyValues.ClosingPriceName + "_FirstCell", ContentAlignment.MiddleRight);

                        // Legend second cell creation
                        var secondCell = new LegendCell(LegendCellType.Text,
                        string.Format(_language.GetLanguageTextByXPath(@"/Chart/Legend/ClosingPrice", _languageName) + "( " + _shareObjectFinalValue.CurrencyUnit + " )\n" +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Minimum", _languageName) +
                                      "{0} / " +
                                      _language.GetLanguageTextByXPath(@"/Chart/ToolTip/Maximum", _languageName) +
                                      "{1}", dailyValuesList.Aggregate((min, x) => x.ClosingPrice < min.ClosingPrice ? x : min).ClosingPrice, dailyValuesList.Aggregate((max, x) => x.ClosingPrice > max.ClosingPrice ? x : max).ClosingPrice),
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
                    case DailyValues.OpeningPriceName:
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

                        _chartDailyValues.Series.Add(DailyValues.OpeningPriceName);
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].Color = graphValues.Color;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].XValueMember = DailyValues.DateName;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].YValueMembers = DailyValues.OpeningPriceName;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].MarkerSize = 6;
                        _chartDailyValues.Series[DailyValues.OpeningPriceName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/OpeningPrice", _languageName) + "\n" + toolTipFormat;

                        #endregion Series

                        #region Legend

                        // Legend item creation
                        var legendItem = new LegendItem
                        {
                            SeriesName = DailyValues.OpeningPriceName,
                            Color = graphValues.Color,
                            Name = DailyValues.OpeningPriceName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                            DailyValues.OpeningPriceName + "_FirstCell", ContentAlignment.MiddleRight);

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
                    case DailyValues.TopName:
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

                        _chartDailyValues.Series.Add(DailyValues.TopName);
                        _chartDailyValues.Series[DailyValues.TopName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[DailyValues.TopName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[DailyValues.TopName].Color = graphValues.Color;
                        _chartDailyValues.Series[DailyValues.TopName].XValueMember = DailyValues.DateName;
                        _chartDailyValues.Series[DailyValues.TopName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[DailyValues.TopName].YValueMembers = DailyValues.TopName;
                        _chartDailyValues.Series[DailyValues.TopName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[DailyValues.TopName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[DailyValues.TopName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[DailyValues.TopName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[DailyValues.TopName].MarkerSize = 6;
                        _chartDailyValues.Series[DailyValues.TopName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Top", _languageName) + "\n" + toolTipFormat;

                        #endregion Series

                        #region Legend

                        // Legend item creation
                        var legendItem = new LegendItem
                        {
                            SeriesName = DailyValues.TopName,
                            Color = graphValues.Color,
                            Name = DailyValues.TopName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                            DailyValues.TopName + "_FirstCell", ContentAlignment.MiddleRight);

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
                    case DailyValues.BottomName:
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

                        _chartDailyValues.Series.Add(DailyValues.BottomName);
                        _chartDailyValues.Series[DailyValues.BottomName].ChartType = graphValues.Type;
                        _chartDailyValues.Series[DailyValues.BottomName].BorderWidth = graphValues.BorderWidth;
                        _chartDailyValues.Series[DailyValues.BottomName].Color = graphValues.Color;
                        _chartDailyValues.Series[DailyValues.BottomName].XValueMember = DailyValues.DateName;
                        _chartDailyValues.Series[DailyValues.BottomName].XValueType = ChartValueType.DateTime;
                        _chartDailyValues.Series[DailyValues.BottomName].YValueMembers = DailyValues.BottomName;
                        _chartDailyValues.Series[DailyValues.BottomName].YValueType = ChartValueType.Double;
                        _chartDailyValues.Series[DailyValues.BottomName].YAxisType = AxisType.Primary;
                        _chartDailyValues.Series[DailyValues.BottomName].IsVisibleInLegend = false;
                        _chartDailyValues.Series[DailyValues.BottomName].MarkerStyle = MarkerStyle.Square;
                        _chartDailyValues.Series[DailyValues.BottomName].MarkerSize = 6;
                        _chartDailyValues.Series[DailyValues.BottomName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Bottom", _languageName) + "\n" + toolTipFormat;

                        #endregion Series

                        #region Legend

                        // Legend item creation
                        var legendItem = new LegendItem
                        {
                            SeriesName = DailyValues.BottomName,
                            Color = graphValues.Color,
                            Name = DailyValues.BottomName + "_Item"
                        };

                        // Legend first cell creation
                        var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                            DailyValues.BottomName + "_FirstCell", ContentAlignment.MiddleRight);

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
                    case DailyValues.VolumeName:
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

                            _chartDailyValues.Series.Add(DailyValues.VolumeName);
                            _chartDailyValues.Series[DailyValues.VolumeName].ChartType = graphValues.Type;
                            _chartDailyValues.Series[DailyValues.VolumeName].BorderWidth = graphValues.BorderWidth;
                            _chartDailyValues.Series[DailyValues.VolumeName].Color = graphValues.Color;
                            _chartDailyValues.Series[DailyValues.VolumeName].XValueMember = DailyValues.DateName;
                            _chartDailyValues.Series[DailyValues.VolumeName].XValueType = ChartValueType.DateTime;
                            _chartDailyValues.Series[DailyValues.VolumeName].YValueMembers = DailyValues.VolumeName;
                            _chartDailyValues.Series[DailyValues.VolumeName].YValueType = ChartValueType.Double;
                            _chartDailyValues.Series[DailyValues.VolumeName].YAxisType = AxisType.Secondary;
                            _chartDailyValues.Series[DailyValues.VolumeName].IsVisibleInLegend = false;
                            _chartDailyValues.Series[DailyValues.VolumeName].MarkerStyle = MarkerStyle.Square;
                            _chartDailyValues.Series[DailyValues.VolumeName].MarkerSize = 6;
                            _chartDailyValues.Series[DailyValues.VolumeName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Volume", _languageName) + "\n" + toolTipFormat;

                                #endregion Series

                            #region Legend

                            // Legend item creation
                            var legendItem = new LegendItem
                            {
                                SeriesName = DailyValues.VolumeName,
                                Color = graphValues.Color,
                                Name = DailyValues.VolumeName + "_Item"
                            };

                            // Legend first cell creation
                            var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                                DailyValues.VolumeName + "_FirstCell", ContentAlignment.MiddleRight);

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

                            _chartDailyValues.Series.Add(DailyValues.VolumeName);
                            _chartDailyValues.Series[DailyValues.VolumeName].ChartType = graphValues.Type;
                            _chartDailyValues.Series[DailyValues.VolumeName].BorderWidth = graphValues.BorderWidth;
                            _chartDailyValues.Series[DailyValues.VolumeName].Color = graphValues.Color;
                            _chartDailyValues.Series[DailyValues.VolumeName].XValueMember = DailyValues.DateName;
                            _chartDailyValues.Series[DailyValues.VolumeName].XValueType = ChartValueType.DateTime;
                            _chartDailyValues.Series[DailyValues.VolumeName].YValueMembers = DailyValues.VolumeName;
                            _chartDailyValues.Series[DailyValues.VolumeName].YValueType = ChartValueType.Double;
                            _chartDailyValues.Series[DailyValues.VolumeName].YAxisType = AxisType.Primary;
                            _chartDailyValues.Series[DailyValues.VolumeName].IsVisibleInLegend = false;
                            _chartDailyValues.Series[DailyValues.VolumeName].MarkerStyle = MarkerStyle.Square;
                            _chartDailyValues.Series[DailyValues.VolumeName].MarkerSize = 6;
                            _chartDailyValues.Series[DailyValues.VolumeName].ToolTip = _language.GetLanguageTextByXPath(@"/Chart/Legend/Volume", _languageName) + "\n" + toolTipFormat;

                            #endregion Series

                            #region Legend

                            // Legend item creation
                            var legendItem = new LegendItem
                            {
                                SeriesName = DailyValues.VolumeName,
                                Color = graphValues.Color,
                                Name = DailyValues.VolumeName + "_Item"
                            };

                            // Legend first cell creation
                            var firstCell = new LegendCell(LegendCellType.SeriesSymbol,
                                DailyValues.VolumeName + "_FirstCell", ContentAlignment.MiddleRight);

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
        }

        #endregion Charting

        #region Helper functions

        private static List<Parser.DailyValues> GetDailyValuesOfInterval(DateTime givenDateTime, int iAmount)
        {
            DateTime startDate;
            var dailyValues = new List<Parser.DailyValues>();

            dailyValues.AddRange(_marketValueOverviewTabSelected
                ? _shareObjectMarketValue.DailyValues
                : _shareObjectFinalValue.DailyValues);

            // Calculate the start date for the given interval and amount
            switch (_chartValues.Interval)
            {
                case ChartingInterval.Week:
                {
                    startDate = givenDateTime.AddDays(-7 * iAmount);
                } break;
                case ChartingInterval.Month:
                {
                    startDate = givenDateTime.AddMonths(-iAmount);
                } break;
                case ChartingInterval.Quarter:
                {
                    startDate = givenDateTime.AddMonths(-3 * iAmount);
                } break;
                case ChartingInterval.Year:
                {
                    startDate = givenDateTime.AddYears(-iAmount);
                } break;
                default:
                {
                    startDate = givenDateTime.AddDays(-7 * iAmount);
                } break;
            }

            // Get daily values for the timespan
            var dailyValuesResult = dailyValues.Where(x => x.Date >= startDate).Where(x => x.Date <= givenDateTime).ToList();

            return dailyValuesResult;
        }

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
                    case DailyValues.ClosingPriceName:
                    {
                        decMin = dailyValuesList.Min(x => x.ClosingPrice);
                        decMax = dailyValuesList.Max(x => x.ClosingPrice);
                    } break;
                    case DailyValues.OpeningPriceName:
                    {
                        decMin = dailyValuesList.Min(x => x.OpeningPrice);
                        decMax = dailyValuesList.Max(x => x.OpeningPrice);
                    } break;
                    case DailyValues.TopName:
                    {
                        decMin = dailyValuesList.Min(x => x.Top);
                        decMax = dailyValuesList.Max(x => x.Top);
                    } break;
                    case DailyValues.BottomName:
                    {
                        decMin = dailyValuesList.Min(x => x.Bottom);
                        decMax = dailyValuesList.Max(x => x.Bottom);
                    } break;
                    case DailyValues.VolumeName:
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
                    if (graphValues.YValueMemberName == DailyValues.VolumeName)
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

#if DEBUG
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

#if DEBUG
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
                if (GraphValuesList.Count(x => x.Show) > 1 && GraphValuesList.Any(x => x.YValueMemberName == DailyValues.VolumeName))
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
