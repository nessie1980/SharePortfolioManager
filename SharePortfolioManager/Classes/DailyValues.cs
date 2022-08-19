//MIT License
//
//Copyright(c) 2017 - 2022 nessie1980(nessie1980@gmx.de)
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
//#define DEBUG_LIST_DAILY_VALUES

using System;
using System.Collections.Generic;
using System.Linq;

namespace SharePortfolioManager.Classes
{
    // Interval for charting values
    public enum ChartingInterval
    {
        Week = 0,
        Month = 1,
        Quarter = 2,
        Year = 3
    }

    public class DailyValuesList
    {
        #region Properties

        public ChartingInterval Interval { get; internal set; }

        public int WeekAmount { get; internal set; }

        public int MonthAmount { get; internal set; }

        public int QuarterAmount { get; internal set; }

        public int YearAmount { get; internal set; }

        public List<Parser.DailyValues> Entries { get; internal set; } = new List<Parser.DailyValues>();

        #endregion Properties

        #region Methodes

        public void AddItem(Parser.DailyValues item)
        {
            Entries.Add(item);

            Entries.Sort();

            UpdateAmounts(Entries.First().Date, Entries.Last().Date);
        }
        
        #region Helper functions

        public void UpdateAmounts(DateTime startDateTime, DateTime endDateTime)
        {
            GetWeekAmount(startDateTime, endDateTime);
            GetMonthAmount(startDateTime, endDateTime);
            GetQuarterAmount(startDateTime, endDateTime);
            GetYearAmount(startDateTime, endDateTime);
        }
        public List<Parser.DailyValues> GetDailyValuesOfInterval(DateTime givenDateTime, int iAmount)
        {
            DateTime startDate;
            var dailyValues = new List<Parser.DailyValues>();

            dailyValues.AddRange(Entries);

            // Calculate the start date for the given interval and amount
            switch (Interval)
            {
                case ChartingInterval.Week:
                    {
                        startDate = givenDateTime.AddDays(-7 * iAmount);
                    }
                    break;
                case ChartingInterval.Month:
                    {
                        startDate = givenDateTime.AddMonths(-iAmount);
                    }
                    break;
                case ChartingInterval.Quarter:
                    {
                        startDate = givenDateTime.AddMonths(-3 * iAmount);
                    }
                    break;
                case ChartingInterval.Year:
                    {
                        startDate = givenDateTime.AddYears(-iAmount);
                    }
                    break;
                default:
                    {
                        startDate = givenDateTime.AddDays(-7 * iAmount);
                    }
                    break;
            }

            // Get daily values for the timespan
            var dailyValuesResult = dailyValues.Where(x => x.Date >= startDate).Where(x => x.Date <= givenDateTime).ToList();

            return dailyValuesResult;
        }

        public int GetWeekAmount(DateTime startDate, DateTime endDate)
        {
            WeekAmount = (startDate < endDate ? (endDate - startDate).Days : (startDate - endDate).Days) / 7;

#if DEBUG_LIST_DAILY_VALUES
            Console.WriteLine(@"Weeks: {0}", WeekAmount);
#endif
            return WeekAmount;
        }

        public int GetMonthAmount(DateTime startDate, DateTime endDate)
        {
            int month1;
            int month2;

            if (startDate < endDate)
            {
                month1 = (endDate.Month - startDate.Month);     // Years
                month2 = (endDate.Year - startDate.Year) * 12;  // Months
            }
            else
            {
                month1 = (startDate.Month - endDate.Month);     // Years
                month2 = (startDate.Year - endDate.Year) * 12;  // Months
            }

            MonthAmount = month1 + month2;

#if DEBUG_LIST_DAILY_VALUES
            Console.WriteLine(@"Month: {0}", MonthAmount);
#endif
            return MonthAmount;
        }

        public int GetQuarterAmount(DateTime startDate, DateTime endDate)
        {
            QuarterAmount = GetMonthAmount(startDate, endDate) / 4;

#if DEBUG_LIST_DAILY_VALUES
            Console.WriteLine(@"Quarter: {0}", QuarterAmount);
#endif
            return QuarterAmount;
        }

        public int GetYearAmount(DateTime startDate, DateTime endDate)
        {
            YearAmount = GetMonthAmount(startDate, endDate) / 12;

#if DEBUG_LIST_DAILY_VALUES
            Console.WriteLine(@"Year: {0}", YearAmount);
#endif
            return YearAmount;
        }

        #endregion Helper functions

        #endregion Methodes
    }
}
