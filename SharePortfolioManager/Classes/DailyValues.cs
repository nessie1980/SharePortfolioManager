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
//#define DEBUG_DAILY_VALUES

using System;

namespace SharePortfolioManager.Classes
{
    public class DailyValues : IComparable<DailyValues>
    {
        public DateTime Date { get; set; }
        public const string DateName = "Date";
        public decimal ClosingPrice { get; set; }
        public const string ClosingPriceName = "Closingprice";
        public decimal OpeningPrice { get; set; }
        public const string OpeningPriceName  = "Openingprice";
        public decimal Top { get; set; }
        public const string TopName = "Top";
        public decimal Bottom { get; set; }
        public const string BottomName = "Bottom";
        public decimal Volume { get; set; }
        public const string VolumeName = "Volume";

        // Default comparer for DailiyValues type.
        public int CompareTo(DailyValues dailyValue)
        {
            // A null value means that this object is greater.
            return dailyValue == null ? 1 : Date.CompareTo(dailyValue.Date);
        }
    }
}
