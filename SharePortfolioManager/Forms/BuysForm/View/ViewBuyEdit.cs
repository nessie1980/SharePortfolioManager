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

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.BuysForm.View
{
    public enum BuyErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        DeleteFailedUnerasable,
        InputeValuesInvalid,
        DateExists,
        DateWrongFormat,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        SharePricEmpty,
        SharePriceWrongFormat,
        SharePriceWrongValue,
        CostsWrongFormat,
        CostsWrongValue,
        ReductionWrongFormat,
        ReductionWrongValue,
        DocumentDoesNotExists
    };

    /// <summary>
    /// Interface of the BuyEdit view
    /// </summary>
    public interface IViewBuyEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValues;
        event EventHandler AddBuy;
        event EventHandler EditBuy;
        event EventHandler DeleteBuy;

        BuyErrorCode ErrorCode { get; set; }

        bool UpdateBuy { get; set; }
        string SelectedDate { get; set; }
        ShareObject ShareObject { get; }
        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        string Price { get; set; }
        string MarketValue { get; set; }
        string Costs { get; set; }
        string Reduction { get; set; }
        string FinalValue { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
    }

    public partial class ViewBuyEdit : Form, IViewBuyEdit
    {
        #region Fields

        /// <summary>
        /// Stores the chosen share object
        /// </summary>
        ShareObject _shareObject = null;

        /// <summary>
        /// Stores the logger
        /// </summary>
        readonly Logger _logger;

        /// <summary>
        /// Stores the given language file
        /// </summary>
        readonly Language _xmlLanguage;

        /// <summary>
        /// Stores the given language
        /// </summary>
        string _strLanguage;

        /// <summary>
        /// Stores if a buy should be updated
        /// </summary>
        bool _bUpdateBuy;

        /// <summary>
        /// Stores the date of a selected buy row
        /// </summary>
        string _selectedDate;

        /// <summary>
        /// Stores the current error code of the form
        /// </summary>
        private BuyErrorCode _errorCode;

        /// <summary>
        /// Stores if a buy has been deleted or added
        /// and so a save must be done in the lower dialog
        /// </summary>
        private bool _bSave;

        /// <summary>
        /// Stores the DataGridView of the selected row
        /// </summary>
        DataGridView _selectedDataGridView = null;

        #endregion Fields

        #region IView members

        new

        DialogResult ShowDialog
        {
            get { return base.ShowDialog(); }
        }

        public bool UpdateBuy
        {
            get { return _bUpdateBuy; }
            set
            {
                _bUpdateBuy = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UpdateBuy"));
            }
        }

        public BuyErrorCode ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public ShareObject ShareObject
        {
            get { return _shareObject; }
        }

        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDate"));
            }
        }

        #region Input values

        public string Date
        {
            get { return datePickerDate.Text; }
            set
            {
                if (datePickerDate.Text == value)
                    return;
                datePickerDate.Text = value;
            }
        }

        public string Time
        {
            get { return datePickerTime.Text; }
            set
            {
                if (datePickerTime.Text == value)
                    return;
                datePickerTime.Text = value;
            }
        }

        public string Volume
        {
            get { return txtBoxVolume.Text; }
            set
            {
                if (txtBoxVolume.Text == value)
                    return;
                txtBoxVolume.Text = value;
            }
        }

        public string Price
        {
            get { return txtBoxPrice.Text; }
            set
            {
                if (txtBoxPrice.Text == value)
                    return;
                txtBoxPrice.Text = value;
            }
        }

        public string MarketValue
        {
            get { return txtBoxMarketValue.Text; }
            set
            {
                if (txtBoxMarketValue.Text == value)
                    return;
                txtBoxMarketValue.Text = value;
            }
        }

        public string Costs
        {
            get { return txtBoxCosts.Text; }
            set
            {
                if (txtBoxCosts.Text == value)
                    return;
                txtBoxCosts.Text = value;
            }
        }

        public string Reduction
        {
            get { return txtBoxReduction.Text; }
            set
            {
                if (txtBoxReduction.Text == value)
                    return;
                txtBoxReduction.Text = value;
            }
        }

        public string FinalValue
        {
            get { return txtBoxFinalValue.Text; }
            set
            {
                if (txtBoxFinalValue.Text == value)
                    return;
                txtBoxFinalValue.Text = value;
            }
        }

        public string Document
        {
            get { return txtBoxDocument.Text; }
            set
            {
                if (txtBoxDocument.Text == value)
                    return;
                txtBoxDocument.Text = value;
            }
        }

        #endregion Input values

        public void AddEditDeleteFinish()
        {
            // Set messages
            string strMessage = @"";
            Color clrMessage = Color.Black;
            FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case BuyErrorCode.AddSuccessful:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/AddSuccess", _strLanguage);
                        // Set flag to save the share object.
                        _bSave = true;
                        // Reset values
                        ResetValues();
                        // Refresh the buy list
                        ShowBuys();
                        break;
                    }
                case BuyErrorCode.AddFailed:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case BuyErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add",
                                _strLanguage);
                        btnAddSave.Image = Resources.black_add;
                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption",
                                _strLanguage);

                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/EditSuccess", _strLanguage);
                        // Set flag to save the share object.
                        _bSave = true;
                        // Reset values
                        ResetValues();
                        // Refresh the buy list
                        ShowBuys();
                        break;
                    }
                case BuyErrorCode.EditFailed:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/EditFailed", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case BuyErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/DeleteSuccess", _strLanguage);
                        // Set flag to save the share object.
                        _bSave = true;
                        // Reset values
                        ResetValues();

                        // Enable button(s)
                        btnAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", _strLanguage);
                        btnAddSave.Image = Resources.black_add;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", _strLanguage);

                        // Refresh the buy list
                        ShowBuys();
                        break;
                    }
                case BuyErrorCode.DeleteFailed:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
                case BuyErrorCode.DeleteFailedUnerasable:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailedUnerasable", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
                case BuyErrorCode.DateExists:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DateExists", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        datePickerDate.Focus();
                        break;
                    }
                case BuyErrorCode.DateWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DateWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        datePickerDate.Focus();
                        break;
                    }
                case BuyErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeEmpty", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case BuyErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case BuyErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case BuyErrorCode.SharePricEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceEmpty", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxPrice.Focus();
                        break;
                    }
                case BuyErrorCode.SharePriceWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxPrice.Focus();
                        break;
                    }
                case BuyErrorCode.SharePriceWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxPrice.Focus();
                        break;
                    }
                case BuyErrorCode.CostsWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CostsWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxCosts.Focus();
                        break;
                    }
                case BuyErrorCode.CostsWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CostsWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxCosts.Focus();
                        break;
                    }
                case BuyErrorCode.ReductionWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxReduction.Focus();
                        break;
                    }
                case BuyErrorCode.ReductionWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxReduction.Focus();
                        break;
                    }
                case BuyErrorCode.DocumentDoesNotExists:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/FileDoesNotExist", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
            }

            // Enable controls
            this.Enabled = true;

            Helper.AddStatusMessage(toolStripStatusLabelMessage,
               strMessage,
               _xmlLanguage,
               _strLanguage,
               clrMessage,
               _logger,
               (int)stateLevel,
               (int)FrmMain.EComponentLevels.Application);
        }

        #endregion IView members

        #region Event members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValues;
        public event EventHandler AddBuy;
        public event EventHandler EditBuy;
        public event EventHandler DeleteBuy;

        #endregion

        #region Methods

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObject">Current chosen share object</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public ViewBuyEdit(ShareObject shareObject, Logger logger, Language xmlLanguage, String language)
        {
            InitializeComponent();

            _shareObject = shareObject;
            _logger = logger;
            _xmlLanguage = xmlLanguage;
            _strLanguage = language;
            _bSave = false;
        }

        /// <summary>
        /// This function sets the language to the labels and buttons
        /// and sets the units behind the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareBuysEdit_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Caption", _strLanguage);
                grpBoxAdd.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", _strLanguage);
                grpBoxOverview.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/Caption",
                        _strLanguage);
                lblDate.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Date",
                    _strLanguage);
                lblVolume.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Volume",
                        _strLanguage);
                lblFinalValue.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/FinalValue",
                        _strLanguage);
                lblCosts.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Costs",
                        _strLanguage);
                lblReduction.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Reduction",
                        _strLanguage);
                lblMarketValue.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/MarketValue",
                        _strLanguage);
                lblBuyPrice.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Price",
                        _strLanguage);
                lblDocument.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Document",
                        _strLanguage);
                btnAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add",
                    _strLanguage);
                btnReset.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Reset",
                    _strLanguage);
                btnDelete.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Delete",
                        _strLanguage);
                btnCancel.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Cancel",
                        _strLanguage);

                #endregion Language configuration

                #region Unit configuration

                // Set buy units to the edit boxes
                lblAddVolumeUnit.Text = ShareObject.PieceUnit;
                lblAddFinalValueUnit.Text = _shareObject.CurrencyUnit;
                lblAddCostsUnit.Text = _shareObject.CurrencyUnit;
                lblAddReductionUnit.Text = _shareObject.CurrencyUnit;
                lblAddMarketValueUnit.Text = _shareObject.CurrencyUnit;
                lblAddPriceUnit.Text = _shareObject.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.black_save;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                #endregion Image configuration

                // Load buys of the share
                ShowBuys();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShareBuysEdit_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", _strLanguage),
                   _xmlLanguage, _strLanguage, Color.DarkRed, _logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a buy has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareBuysEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if a buy change must be saved
            if (_bSave)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }

        #endregion Form

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddVolume_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Volume"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxVolume_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("SharePrice"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCosts_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Costs"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCosts_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReduction_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Reduction"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReduction_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        /// <summary>
        /// This function only sets the document of the model to the view
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Document"));
        }

        #endregion TextBoxes

        /// <summary>
        /// This function paints the buy list of the share
        /// </summary>
        private void ShowBuys()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlBuys.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        DataGridView dataGridView = control as DataGridView;
                        if (dataGridView != null)
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBuysOfAYear_SelectionChanged;
                            dataGridView.SelectionChanged -= OnDataGridViewBuysOfYears_SelectionChanged;
                            dataGridView.DataBindingComplete -= OnDataGridViewBuysOfAYear_DataBindingComplete;
                        }
                    }
                    tabPage.Controls.Clear();
                    tabCtrlBuys.TabPages.Remove(tabPage);
                }

                tabCtrlBuys.TabPages.Clear();

                // Create TabPage for the buys of the years
                TabPage newTabPageOverviewYears = new TabPage();
                // Set TabPage name
                newTabPageOverviewYears.Name =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview",
                        _strLanguage);
                newTabPageOverviewYears.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", _strLanguage) +
                                          @" (" + _shareObject.AllBuyEntries.BuyMarketValueTotalAsStrUnit + @")";

                // Create Binding source for the buy data
                BindingSource bindingSourceOverview = new BindingSource();
                if (_shareObject.AllBuyEntries.GetAllBuysTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        _shareObject.AllBuyEntries.GetAllBuysTotalValues();

                // Create DataGridView
                DataGridView dataGridViewBuysOverviewOfAYears = new DataGridView();
                dataGridViewBuysOverviewOfAYears.Dock = DockStyle.Fill;
                // Bind source with buy values to the DataGridView
                dataGridViewBuysOverviewOfAYears.DataSource = bindingSourceOverview;
                // Set the delegate for the DataBindingComplete event
                dataGridViewBuysOverviewOfAYears.DataBindingComplete += OnDataGridViewBuysOfAYear_DataBindingComplete;

                // Set row select event
                dataGridViewBuysOverviewOfAYears.SelectionChanged += OnDataGridViewBuysOfYears_SelectionChanged;

                // Advanced configuration DataGridView buys
                DataGridViewCellStyle styleOverviewOfYears = dataGridViewBuysOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                dataGridViewBuysOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridViewBuysOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewBuysOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


                dataGridViewBuysOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewBuysOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewBuysOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewBuysOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dataGridViewBuysOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dataGridViewBuysOverviewOfAYears.MultiSelect = false;

                dataGridViewBuysOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewBuysOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewBuysOverviewOfAYears.AllowUserToResizeRows = false;

                dataGridViewBuysOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                newTabPageOverviewYears.Controls.Add(dataGridViewBuysOverviewOfAYears);
                dataGridViewBuysOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlBuys.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlBuys;


                // Check if buys exists
                if (_shareObject.AllBuyEntries.AllBuysOfTheShareDictionary.Count > 0)
                {
                    // Loop through the years of the buys
                    foreach (
                        var keyName in _shareObject.AllBuyEntries.AllBuysOfTheShareDictionary.Keys.Reverse()
                        )
                    {
                        // Create TabPage
                        TabPage newTabPage = new TabPage();
                        // Set TabPage name
                        newTabPage.Name = keyName;
                        newTabPage.Text = keyName + " (" +
                                          _shareObject.AllBuyEntries.AllBuysOfTheShareDictionary[keyName]
                                          .BuyMarketValueYearAsStrUnit
                                          + ")";

                        // Create Binding source for the buy data
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource =
                            _shareObject.AllBuyEntries.AllBuysOfTheShareDictionary[keyName].BuyListYear;

                        // Create DataGridView
                        DataGridView dataGridViewBuysOfAYear = new DataGridView();
                        dataGridViewBuysOfAYear.Dock = DockStyle.Fill;
                        // Bind source with buy values to the DataGridView
                        dataGridViewBuysOfAYear.DataSource = bindingSource;
                        // Set the delegate for the DataBindingComplete event
                        dataGridViewBuysOfAYear.DataBindingComplete += OnDataGridViewBuysOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewBuysOfAYear.SelectionChanged += OnDataGridViewBuysOfAYear_SelectionChanged;
                        // Set cell decimal click event
                        dataGridViewBuysOfAYear.CellContentDoubleClick += OnDataGridViewBuysOfAYear_CellContentdecimalClick;

                        // Advanced configuration DataGridView buys
                        DataGridViewCellStyle style = dataGridViewBuysOfAYear.ColumnHeadersDefaultCellStyle;
                        style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                        dataGridViewBuysOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        dataGridViewBuysOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                        dataGridViewBuysOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                        dataGridViewBuysOfAYear.RowHeadersVisible = false;
                        dataGridViewBuysOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                        dataGridViewBuysOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                        dataGridViewBuysOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                        dataGridViewBuysOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        dataGridViewBuysOfAYear.MultiSelect = false;

                        dataGridViewBuysOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewBuysOfAYear.AllowUserToResizeColumns = false;
                        dataGridViewBuysOfAYear.AllowUserToResizeRows = false;

                        dataGridViewBuysOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        newTabPage.Controls.Add(dataGridViewBuysOfAYear);
                        dataGridViewBuysOfAYear.Parent = newTabPage;
                        tabCtrlBuys.Controls.Add(newTabPage);
                        newTabPage.Parent = tabCtrlBuys;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShowBuys()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", _strLanguage),
                   _xmlLanguage, _strLanguage, Color.DarkRed, _logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewBuysOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlBuys.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlBuys.SelectedTab.Controls.Contains((DataGridView)sender))
                        DeselectRowsOfDataGridViews((DataGridView)sender);
                }

                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    // Set selected date
                    SelectedDate = curItem[0].Cells[0].Value.ToString();

                    // Get BuyObject of the selected DataGridView row
                    BuyObject selectedBuyObject = _shareObject.AllBuyEntries.GetBuyObjectByDateTime(SelectedDate);

                    // Get CostObject of the selected DataGridView row if a cost is set by this buy
                    CostObject selectedCostObject = _shareObject.AllCostsEntries.GetCostObjectByDateTime(SelectedDate);

                    // Set buy values
                    if (selectedBuyObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        txtBoxVolume.Text = selectedBuyObject.VolumeAsStr;
                        txtBoxFinalValue.Text = selectedBuyObject.FinalValueAStr;
                        txtBoxReduction.Text = selectedBuyObject.ReductionAsStr;
                        txtBoxPrice.Text = selectedBuyObject.SharePriceAsStr;
                        txtBoxDocument.Text = selectedBuyObject.Document;
                    }
                    else
                    {
                        datePickerDate.Value = Convert.ToDateTime(SelectedDate);
                        datePickerTime.Value = Convert.ToDateTime(SelectedDate);
                        txtBoxVolume.Text =  curItem[0].Cells[1].Value.ToString();
                        txtBoxFinalValue.Text = curItem[0].Cells[3].Value.ToString();
                        txtBoxReduction.Text = @"0,00";
                        txtBoxPrice.Text = curItem[0].Cells[2].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[4].Value.ToString();
                    }

                    // Set cost values
                    if (selectedCostObject != null && selectedCostObject.CostOfABuy)
                        txtBoxCosts.Text = selectedCostObject.CostValueAsString;
                    else
                        txtBoxCosts.Text = @"";

                    if (_shareObject.AllBuyEntries.IsDateLastDate(SelectedDate) &&
                        _shareObject.AllSaleEntries.IsDateLastDate(SelectedDate) &&
                        _shareObject.AllBuyEntries.GetAllBuysOfTheShare().Count > 1
                        )
                    {
                        // Enable Button(s)
                        btnDelete.Enabled = true;
                        // Enable TextBoxe(s)
                        datePickerDate.Enabled = true;
                        datePickerTime.Enabled = true;
                        txtBoxVolume.Enabled = true;
                        txtBoxPrice.Enabled = true;
                        txtBoxMarketValue.Enabled = true;
                        txtBoxCosts.Enabled = true;
                        txtBoxReduction.Enabled = true;
                        txtBoxFinalValue.Enabled = true;
                    }
                    else
                    {
                        // Disable Button(s)
                        btnDelete.Enabled = false;
                        // Disable TextBoxe(s)
                        datePickerDate.Enabled = false;
                        datePickerTime.Enabled = false;
                        txtBoxVolume.Enabled = false;
                        txtBoxMarketValue.Enabled = false;
                        txtBoxPrice.Enabled = false;
                        txtBoxCosts.Enabled = false;
                        txtBoxReduction.Enabled = false;
                        txtBoxFinalValue.Enabled = false;
                    }

                    // Enable button(s)
                    btnReset.Enabled = true;
                    // Rename button
                    btnAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Save", _strLanguage);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAdd.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Edit_Caption", _strLanguage);

                    // Store DataGridView instance
                    _selectedDataGridView = (DataGridView)sender;

                    // Format the input value
                    if (FormatInputValues != null)
                        FormatInputValues(this, new EventArgs());
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", _strLanguage);
                    btnAddSave.Image = Resources.black_add;
                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;
                    // Enable Button(s)
                    btnAddSave.Enabled = true;
                    // Enabled TextBoxe(s)
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxPrice.Enabled = true;
                    txtBoxCosts.Enabled = true;
                    txtBoxReduction.Enabled = true;

                    // Rename group box
                    grpBoxAdd.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", _strLanguage);

                    // Reset stored DataGridView instance
                    _selectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewBuysOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                ResetValues();

                ShowBuys();
            }
        }

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewBuysOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlBuys.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlBuys.SelectTab(tabPage);
                            tabPage.Focus();

                            // Deselect rows
                            DeselectRowsOfDataGridViews(null);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewBuysOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewBuysOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (int i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Date",
                                    _strLanguage);
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Value",
                                    _strLanguage) + @" (" + _shareObject.CurrencyUnit + @")";
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Volume",
                                    _strLanguage) + @" (" + ShareObject.PieceUnit + @")";
                            ;
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Price",
                                    _strLanguage) + @" (" + _shareObject.CurrencyUnit + @")";
                            break;
                        case 4:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Document",
                                    _strLanguage);
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                    ((DataGridView)sender).Rows[0].Selected = false;

                // Reset the text box values
                ResetValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewBuysOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/RenameColHeaderFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the buy document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewBuysOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                int iColumnCount = ((DataGridView)sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex == iColumnCount - 1)
                {
                    // Check if a row is selected
                    if (((DataGridView)sender).SelectedRows.Count == 1)
                    {
                        // Get the current selected row
                        DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;
                        // Get date and time of the selected buy item
                        string strDateTime = curItem[0].Cells[0].Value.ToString();

                        // Check if a document is set
                        if (curItem[0].Cells[iColumnCount - 1].Value.ToString() != @"-")
                        {
                            // Get doc from the buy with the strDateTime
                            foreach (var temp in _shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                            {
                                // Check if the buy date and time is the same as the date and time of the clicked buy item
                                if (temp.Date == strDateTime)
                                {
                                    // Check if the file still exists
                                    if (File.Exists(temp.Document))
                                        // Open the file
                                        Process.Start(temp.Document);
                                    else
                                    {
                                        string strCaption =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                                _strLanguage);
                                        string strMessage =
                                            _xmlLanguage.GetLanguageTextByXPath(
                                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                                _strLanguage);
                                        string strOk =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                                _strLanguage);
                                        string strCancel =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                                _strLanguage);

                                        OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                                            strCancel);
                                        if (messageBox.ShowDialog() == DialogResult.OK)
                                        {
                                            // Remove buy object and add it with no document
                                            if (_shareObject.RemoveBuy(temp.Date) &&
                                                _shareObject.AddBuy(true, strDateTime, temp.Volume, temp.Reduction, temp.Costs, temp.MarketValue))
                                            {
                                                // Set flag to save the share object.
                                                _bSave = true;

                                                ResetValues();
                                                ShowBuys();

                                                // Add status message
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    _xmlLanguage.GetLanguageTextByXPath(
                                                        @"/AddEditFormBuy/StateMessages/EditSuccess", _strLanguage),
                                                    _xmlLanguage, _strLanguage,
                                                    Color.Black, _logger, (int)FrmMain.EStateLevels.Info,
                                                    (int)FrmMain.EComponentLevels.Application);
                                            }
                                            else
                                            {
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    _xmlLanguage.GetLanguageTextByXPath(
                                                        @"/AddEditFormBuy/Errors/EditFailed", _strLanguage),
                                                    _xmlLanguage, _strLanguage,
                                                    Color.Red, _logger, (int)FrmMain.EStateLevels.Error,
                                                    (int)FrmMain.EComponentLevels.Application);
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewBuysOfAYear_CellContentdecimalClick()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DocumentShowFailed", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        void DeselectRowsOfDataGridViews(DataGridView DataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage TabPage in tabCtrlBuys.TabPages)
                {
                    foreach (Control control in TabPage.Controls)
                    {
                        DataGridView view = control as DataGridView;
                        if (view != null && view != DataGridView)
                        {
                            foreach (DataGridViewRow selectedRow in view.SelectedRows)
                            {
                                selectedRow.Selected = false;
                            }
                        }
                    }
                }

                ResetValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("DeselectRowsOfDataGridViews()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeselectFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function adds a new buy entry to the share object
        /// or edit a buy entry
        /// It also checks if an entry already exists for the given date and time
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls
                this.Enabled = false;

                if (btnAddSave.Text == _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", _strLanguage))
                {
                    UpdateBuy = false;

                    if (AddBuy != null)
                        AddBuy(this, null);
                }
                else
                {
                    UpdateBuy = true;

                    if (EditBuy != null)
                        EditBuy(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnAdd_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the buy.
        /// If the user press "Ok" the buy will be deleted and the
        /// list of the buys will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">EventArgs</param>
        void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                // Disable controls
                this.Enabled = false;

                toolStripStatusLabelMessage.Text = @"";

                string strCaption = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", _strLanguage);
                string strMessage = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Content/BuyDelete",
                    _strLanguage);
                string strOk = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", _strLanguage);
                string strCancel = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", _strLanguage);

                OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                DialogResult dlgResult = messageBox.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    if (DeleteBuy != null)
                        DeleteBuy(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnDelete_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a buy
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                // Enable button(s)
                btnAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", _strLanguage);
                btnAddSave.Image = Resources.black_add;

                // Disable button(s)
                btnReset.Enabled = false;
                btnDelete.Enabled = false;

                // Rename group box
                grpBoxAdd.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", _strLanguage);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                _selectedDataGridView = null;

                // Select overview tab
                if (
                    tabCtrlBuys.TabPages.ContainsKey(
                        _xmlLanguage.GetLanguageTextByXPath(
                            @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", _strLanguage)))
                    tabCtrlBuys.SelectTab(
                        _xmlLanguage.GetLanguageTextByXPath(
                            @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", _strLanguage));
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnReset_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CancelFailure", _strLanguage),
                   _xmlLanguage, _strLanguage,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function closes the dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date
        /// It also reset the background colors
        /// </summary>
        private void ResetValues()
        {
            // Reset date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            // Reset text boxes
            txtBoxVolume.Text = @"";
            txtBoxPrice.Text = @"";
            txtBoxFinalValue.Text = @"";
            txtBoxCosts.Text = @"";
            txtBoxReduction.Text = @"";
            txtBoxDocument.Text = @"";
            txtBoxVolume.Focus();
        }

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the buy
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnBuyDocumentBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                txtBoxDocument.Text = Helper.SetDocument(_xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title", _strLanguage), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnBuyDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ChoseDocumentFailed", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Methods
    }
}