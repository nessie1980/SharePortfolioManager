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

using SharePortfolioManager.Forms.BrokerageForm.Model;
using SharePortfolioManager.Forms.BrokerageForm.View;
using System;
using System.IO;

namespace SharePortfolioManager.Forms.BrokerageForm.Presenter
{
    internal class PresenterBrokerageEdit
    {
        private readonly IModelBrokerageEdit _model;
        private readonly IViewBrokerageEdit _view;

        public PresenterBrokerageEdit(IViewBrokerageEdit view, IModelBrokerageEdit model)
        {
            _view = view;
            _model = model;

            view.PropertyChanged += OnViewChange;
            view.FormatInputValues += OnViewFormatInputValues;
            view.AddBrokerage += OnAddBrokerage;
            view.EditBrokerage += OnEditBrokerage;
            view.DeleteBrokerage += OnDeleteBrokerage;
        }

        private void UpdateViewWithModel()
        {
            _view.ErrorCode = _model.ErrorCode;
            _view.Date = _model.Date;
            _view.Time = _model.Time;
            _view.Brokerage = _model.Brokerage;
            _view.Document = _model.Document;
        }

        private void OnViewFormatInputValues(object sender, EventArgs e)
        {
            _model.UpdateViewFormatted = true;

            UpdateViewWithModel();

            _model.UpdateViewFormatted = false;
        }

        private void OnViewChange(object sender, EventArgs e)
        {
            UpdateModelwithView();
        }

        private void UpdateModelwithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ErrorCode = _view.ErrorCode;
            _model.UpdateBrokerage = _view.UpdateBrokerage;
            _model.SelectedGuid = _view.SelectedGuid;
            _model.SelectedDate = _view.SelectedDate;
            _model.PartOfABuy = _view.PartOfABuy;
            _model.PartOfASale = _view.PartOfASale;
            _model.Date = _view.Date;
            _model.Time = _view.Time;
            _model.Brokerage = _view.Brokerage;
            _model.Document = _view.Document;

            // TODO
            //CalculateMarketValueAndFinalValue();

            if (_model.UpdateView)
                UpdateViewWithModel();
        }

        private void OnAddBrokerage(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateBrokerage))
            {
                // Set date 
                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuid = Guid.NewGuid().ToString();

                _model.ErrorCode = _model.ShareObjectFinalValue.AddBrokerage(_model.SelectedGuid, false, false, strGuid, strDateTime, _model.BrokerageDec, _model.Document) ? BrokerageErrorCode.AddSuccessful : BrokerageErrorCode.AddFailed;
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnEditBrokerage(object sender, EventArgs e)
        {
            // Check the input values
            if (!CheckInputValues(_model.UpdateBrokerage))
            {
                // Set date
                var strDateTime = _model.Date + " " + _model.Time;

                // Generate Guid
                var strGuid = Guid.NewGuid().ToString();

                if (_model.ShareObjectFinalValue.RemoveBrokerage(_model.SelectedGuid, _model.SelectedDate) && _model.ShareObjectFinalValue.AddBrokerage(_model.SelectedGuid, _model.PartOfABuy, _model.PartOfASale, strGuid, strDateTime, _model.BrokerageDec, _model.Document))
                {
                    _model.ErrorCode = BrokerageErrorCode.EditSuccessful;

                    UpdateViewWithModel();

                    _view.AddEditDeleteFinish();

                    return;
                }
            }

            UpdateViewWithModel();

            _view.AddEditDeleteFinish();
        }

        private void OnDeleteBrokerage(object sender, EventArgs e)
        {
            // Delete the brokerage of the selected date
            _model.ErrorCode = _model.ShareObjectFinalValue.RemoveBrokerage(_model.SelectedGuid, _model.SelectedDate) ? BrokerageErrorCode.DeleteSuccessful : BrokerageErrorCode.DeleteFailed;

            // Update error code
            _view.ErrorCode = _model.ErrorCode;

            _view.AddEditDeleteFinish();
        }
        
        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is chosen if
        /// a brokerage should be add or edit.
        /// </summary>
        /// <param name="bFlagEdit">Flag if a brokerage should be add (true) or edit (false)</param>
        /// <returns>Flag if the input values are correct or not</returns>
        private bool CheckInputValues(bool bFlagEdit)
        {
            try
            {
                var bErrorFlag = false;

                _model.ErrorCode = bFlagEdit ? BrokerageErrorCode.EditSuccessful : BrokerageErrorCode.AddSuccessful;

                // Brokerage input check
                if (_model.Brokerage == @"")
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageEmpty;
                    bErrorFlag = true;
                }
                else if (!decimal.TryParse(_model.Brokerage, out var decBrokerage) && bErrorFlag == false)
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageWrongFormat;
                    bErrorFlag = true;
                }
                else if (decBrokerage < 0 && bErrorFlag == false)
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageWrongValue;
                    bErrorFlag = true;
                }
                else if (bErrorFlag == false)
                    _model.BrokerageDec = decBrokerage;

                // Check if a given document exists
                if (_model.Document == null)
                    _model.Document = @"";
                else if (_model.Document != @"" && _model.Document != @"-" && !Directory.Exists(Path.GetDirectoryName(_model.Document)))
                {
                    _model.ErrorCode = BrokerageErrorCode.DocumentDirectoryDoesNotExits;
                    bErrorFlag = true;
                }
                else if (_model.Document != @"" && _model.Document != @"-" && !File.Exists(_model.Document) && bErrorFlag == false)
                {
                    _model.Document = @"";
                    _model.ErrorCode = BrokerageErrorCode.DocumentFileDoesNotExists;
                    bErrorFlag = true;
                }

                return bErrorFlag;
            }
            catch
            {
                _model.ErrorCode = BrokerageErrorCode.InputeValuesInvalid;
                return true;
            }
        }
    }
}
