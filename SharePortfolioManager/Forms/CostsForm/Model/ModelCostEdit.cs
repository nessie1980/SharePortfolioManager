//MIT License
//
//Copyright(c) 2017 nessie1980(nessie1980 @gmx.de)
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

using SharePortfolioManager.Classes;
using SharePortfolioManager.Forms.CostsForm.View;
using System.Collections.Generic;

namespace SharePortfolioManager.Forms.CostsForm.Model
{
    /// <summary>
    /// Interface of the CostEdit model
    /// </summary>
    public interface IModelCostEdit
    {
        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<WebSiteRegex> WebSiteRegexList { get; set; }
        bool UpdateView { get; set; }
        bool UpdateViewFormatted { get; set; }
        bool UpdateCost { get; set; }
        CostErrorCode ErrorCode { get; set; }
        string SelectedDate { get; set; }
        bool PartOfABuy { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Costs { get; set; }
        decimal CostsDec { get; set; }
        string Document { get; set; }
    }

    /// <summary>
    /// Model class of the CostEdit
    /// </summary>
    public class ModelCostEdit : IModelCostEdit
    {
        #region Fields

        bool _updateView;
        bool _updateViewFormatted;
        bool _updateCost;

        ShareObjectMarketValue _shareObjectMarketValue;
        ShareObjectFinalValue _shareObjectFinalValue;
        List<WebSiteRegex> _webSiteRegexList;
        CostErrorCode _errorCode;

        string _selectedDate;
        bool _partOfABuy;
        string _date;
        string _time;
        string _costs;
        decimal _costsDec;
        string _document;

        #endregion Fields

        #region IModel members

        public bool UpdateView
        {
            get { return _updateView; }
            set { _updateView = value; }
        }

        public bool UpdateViewFormatted
        {
            get { return _updateViewFormatted; }
            set { _updateViewFormatted = value; }
        }

        public bool UpdateCost
        {
            get { return _updateCost; }
            set { _updateCost = value; }
        }

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get { return _shareObjectMarketValue; }
            set { _shareObjectMarketValue = value; }
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get { return _shareObjectFinalValue; }
            set { _shareObjectFinalValue = value; }
        }

        public List<WebSiteRegex> WebSiteRegexList
        {
            get { return _webSiteRegexList; }
            set { _webSiteRegexList = value; }
        }

        public CostErrorCode ErrorCode
        {
            get { return _errorCode; }

            set
            {
                _errorCode = value;
            }
        }

        public string SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; }
        }

        public bool PartOfABuy
        {
            get { return _partOfABuy; }
            set { _partOfABuy = value; }
        }

        public string Date
        {
            get { return _date; }
            set
            {
                if (_date == value)
                    return;

                _date = value;
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                if (_time == value)
                    return;

                _time = value;
            }
        }

        public string Costs
        {
            get
            {
                if (UpdateViewFormatted)
                {
                    // Only return the value if the costs is greater than '0'
                    if (_costsDec > 0)
                        return Helper.FormatDecimal(_costsDec, Helper.Currencytwolength, true, Helper.Currencytwofixlength);
                    return @"";
                }
                else
                {
                    return _costs;
                }
            }
            set
            {
                if (_costs == value)
                    return;
                _costs = value;

                // Try to parse
                if (!decimal.TryParse(_costs, out _costsDec))
                    _costsDec = 0;
            }
        }

        public decimal CostsDec
        {
            get { return _costsDec; }
            set
            {
                if (_costsDec == value)
                    return;
                _costsDec = value;

                Costs = _costsDec.ToString();
            }
        }

        public string Document
        {
            get { return _document; }
            set
            {
                if (_document == value)
                    return;
                _document = value;
            }
        }

        #endregion IModel members
    }
}
