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

using System;
using System.IO;
#if DEBUG
using System.Windows.Forms;
#endif
using SharePortfolioManager.Classes;
using SharePortfolioManager.Forms.BrokeragesForm.Model;
using SharePortfolioManager.Forms.BrokeragesForm.View;

namespace SharePortfolioManager.Forms.BrokeragesForm.Presenter
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
            view.FormatInputValuesEventHandler += OnViewFormatInputValues;
            view.AddBrokerageEventHandler += OnAddBrokerage;
            view.EditBrokerageEventHandler += OnEditBrokerage;
            view.DeleteBrokerageEventHandler += OnDeleteBrokerage;
            view.DocumentBrowseEventHandler += OnDocumentBrowse;
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
            UpdateModelWithView();
        }

        private void UpdateModelWithView()
        {
            _model.ShareObjectMarketValue = _view.ShareObjectMarketValue;
            _model.ShareObjectFinalValue = _view.ShareObjectFinalValue;
            _model.ErrorCode = _view.ErrorCode;
            _model.UpdateBrokerage = _view.UpdateBrokerage;
            _model.SelectedGuid = _view.SelectedGuid;
            _model.SelectedDate = _view.SelectedDate;

            _model.Logger = _view.Logger;
            _model.Language = _view.Language;
            _model.LanguageName = _view.LanguageName;

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
        /// This function opens the document browse dialog and set the chosen document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentBrowse(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                _model.Document = Helper.SetDocument(_model.Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/OpenFileDialog/Title", _model.LanguageName), strFilter, _model.Document);

                UpdateViewWithModel();

                _view.DocumentBrowseFinish();
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"OnDocumentBrowse()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _model.ErrorCode = BrokerageErrorCode.DocumentBrowseFailed;
            }

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
                else if (!decimal.TryParse(_model.Brokerage, out var decBrokerage))
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageWrongFormat;
                    bErrorFlag = true;
                }
                else if (decBrokerage < 0)
                {
                    _model.ErrorCode = BrokerageErrorCode.BrokerageWrongValue;
                    bErrorFlag = true;
                }
                else
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
                _model.ErrorCode = BrokerageErrorCode.InputValuesInvalid;
                return true;
            }
        }
    }
}
