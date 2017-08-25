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
        InputValuesInvalid,
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
        DocumentDirectoryDoesNotExits,
        DocumentFileDoesNotExists
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

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        bool UpdateBuy { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        string Price { get; set; }
        string MarketValue { get; set; }
        string Reduction { get; set; }
        string Costs { get; set; }
        string FinalValue { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
    }

    public partial class ViewBuyEdit : Form, IViewBuyEdit
    {
        #region Fields

        #region Transfer parameter

        /// <summary>
        /// Stores the chosen market value share object
        /// </summary>
        ShareObjectMarketValue _shareObjectMarketValue = null;

        /// <summary>
        /// Stores the chosen final value share object
        /// </summary>
        ShareObjectFinalValue _shareObjectFinalValue = null;

        /// <summary>
        /// Stores the logger
        /// </summary>
        readonly Logger _logger;

        /// <summary>
        /// Stores the given language file
        /// </summary>
        readonly Language _language;

        /// <summary>
        /// Stores the given language
        /// </summary>
        string _languageName;

        #endregion Transfer parameter

        #region Flags

        /// <summary>
        /// Stores if a buy has been deleted or added
        /// and so a save must be done in the lower dialog
        /// </summary>
        bool _bSave;

        /// <summary>
        /// Stores if a buy should be updated
        /// </summary>
        bool _bUpdateBuy;

        #endregion Flags

        /// <summary>
        /// Stores the date of a selected buy row
        /// </summary>
        string _selectedDate;

        /// <summary>
        /// Stores the current error code of the form
        /// </summary>
        BuyErrorCode _errorCode;

        /// <summary>
        /// Stores the DataGridView of the selected row
        /// </summary>
        DataGridView _selectedDataGridView = null;

        #endregion Fields

        #region Properties

        public Logger Logger
        {
            get { return _logger; }
        }

        public Language Language
        {
            get { return _language; }
        }

        public string LanguageName
        {
            get { return _languageName; }
        }

        public bool UpdateBuyFlag
        {
            get { return _bUpdateBuy; }
            set { _bUpdateBuy = value; }
        }

        public bool SaveFlag
        {
            get { return _bSave; }
            set { _bSave = value; }
        }

        public DataGridView SelectedDataGridView
        {
            get { return _selectedDataGridView; }
            set { _selectedDataGridView = value; }
        }

        #endregion Properties

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

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get { return _shareObjectMarketValue; }
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get { return _shareObjectFinalValue; }
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
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        _bSave = true;
                        
                        // Refresh the buy list
                        ShowBuys();

                        // Reset values
                        this.Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case BuyErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add",
                                LanguageName);
                        btnAddSave.Image = Resources.black_add;
                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/EditSuccess", LanguageName);
                        // Set flag to save the share object.
                        _bSave = true;
                        
                        // Refresh the buy list
                        ShowBuys();

                        // Reset values
                        this.Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case BuyErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        _bSave = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.black_add;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the buy list
                        ShowBuys();

                        // Reset values
                        this.Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case BuyErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.DeleteFailedUnerasable:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailedUnerasable", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.DateExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DateExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case BuyErrorCode.DateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case BuyErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case BuyErrorCode.SharePricEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxPrice.Focus();

                        break;
                    }
                case BuyErrorCode.SharePriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxPrice.Focus();

                        break;
                    }
                case BuyErrorCode.SharePriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxPrice.Focus();

                        break;
                    }
                case BuyErrorCode.CostsWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CostsWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case BuyErrorCode.CostsWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CostsWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case BuyErrorCode.ReductionWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxReduction.Focus();

                        break;
                    }
                case BuyErrorCode.ReductionWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxReduction.Focus();

                        break;
                    }
                case BuyErrorCode.DocumentDirectoryDoesNotExits:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DirectoryDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        this.Enabled = true;
                        txtBoxDocument.Focus();

                        break;
                    }
                case BuyErrorCode.DocumentFileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/FileDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
               strMessage,
               Language,
               LanguageName,
               clrMessage,
               Logger,
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
        /// <param name="shareObjectMarketValue">Current chosen market value share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final value share object</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public ViewBuyEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue, Logger logger, Language language, String languageName)
        {
            InitializeComponent();

            _shareObjectMarketValue = shareObjectMarketValue;
            _shareObjectFinalValue = shareObjectFinalValue;
            _logger = logger;
            _language = language;
            _languageName = languageName;
            SaveFlag = false;
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

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Caption", LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxOverview.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/Caption",
                        LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                lblVolume.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Volume",
                        LanguageName);
                lblFinalValue.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/FinalValue",
                        LanguageName);
                lblCosts.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Costs",
                        LanguageName);
                lblReduction.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Reduction",
                        LanguageName);
                lblMarketValue.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/MarketValue",
                        LanguageName);
                lblBuyPrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Price",
                        LanguageName);
                lblDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add",
                    LanguageName);
                btnReset.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Reset",
                    LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                #endregion Language configuration

                #region Unit configuration

                // Set buy units to the edit boxes
                lblAddVolumeUnit.Text = ShareObject.PieceUnit;
                lblAddFinalValueUnit.Text = _shareObjectFinalValue.CurrencyUnit;
                lblAddCostsUnit.Text = _shareObjectFinalValue.CurrencyUnit;
                lblAddReductionUnit.Text = _shareObjectFinalValue.CurrencyUnit;
                lblAddMarketValueUnit.Text = _shareObjectFinalValue.CurrencyUnit;
                lblAddPriceUnit.Text = _shareObjectFinalValue.CurrencyUnit;

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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", LanguageName),
                   Language, LanguageName, Color.DarkRed, Logger,
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

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date.
        /// </summary>
        private void ResetInputValues()
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

        #region Buttons

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

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName))
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
        private void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                // Disable controls
                this.Enabled = false;

                toolStripStatusLabelMessageBuyEdit.Text = @"";

                string strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                string strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/BuyDelete",
                    LanguageName);
                string strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                string strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                toolStripStatusLabelMessageBuyEdit.Text = @"";

                // Enable button(s)
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.black_add;

                // Disable button(s)
                btnReset.Enabled = false;
                btnDelete.Enabled = false;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                _selectedDataGridView = null;

                // Select overview tab
                if (
                    tabCtrlBuys.TabPages.ContainsKey(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", LanguageName)))
                    tabCtrlBuys.SelectTab(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", LanguageName));
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnReset_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CancelFailure", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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

        #endregion Buttons

        #region Data grid view

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
                            if (tabPage.Name == "Overview")
                            {
                                dataGridView.SelectionChanged -= OnDataGridViewBuysOfYears_SelectionChanged;
                                dataGridView.MouseEnter -= OnDataGridViewBuysOfYears_MouseEnter;
                                dataGridView.MouseLeave -= OnDataGridViewBuysOfYears_MouseLeave;
                            }
                            else
                            {
                                dataGridView.SelectionChanged -= OnDataGridViewBuysOfAYear_SelectionChanged;
                                dataGridView.MouseEnter -= OnDataGridViewBuysOfAYear_MouseEnter;
                                dataGridView.MouseLeave -= OnDataGridViewBuysOfAYear_MouseEnter;
                            }

                            dataGridView.DataBindingComplete -= OnDataGridViewBuys_DataBindingComplete;
                        }
                    }
                    tabPage.Controls.Clear();
                    tabCtrlBuys.TabPages.Remove(tabPage);
                }

                tabCtrlBuys.TabPages.Clear();

                #region Add page

                // Create TabPage for the buys of the years
                TabPage newTabPageOverviewYears = new TabPage();
                // Set TabPage name
                newTabPageOverviewYears.Name =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview",
                        LanguageName);
                newTabPageOverviewYears.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", LanguageName) +
                                          @" (" + ShareObjectFinalValue.AllBuyEntries.BuyMarketValueReductionTotalAsStrUnit + @")";

                #endregion Add page

                #region Data source, data binding and data grid view

                // Create Binding source for the buy data
                BindingSource bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllBuyEntries.GetAllBuysTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllBuyEntries.GetAllBuysTotalValues();

                // Create DataGridView
                DataGridView dataGridViewBuysOverviewOfAYears = new DataGridView();
                dataGridViewBuysOverviewOfAYears.Dock = DockStyle.Fill;
                // Bind source with buy values to the DataGridView
                dataGridViewBuysOverviewOfAYears.DataSource = bindingSourceOverview;

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewBuysOverviewOfAYears.DataBindingComplete += OnDataGridViewBuys_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewBuysOverviewOfAYears.MouseEnter += OnDataGridViewBuysOfYears_MouseEnter;
                // Set the delegate for the mouse leave event
                dataGridViewBuysOverviewOfAYears.MouseLeave += OnDataGridViewBuysOfYears_MouseLeave;
                // Set row select event
                dataGridViewBuysOverviewOfAYears.SelectionChanged += OnDataGridViewBuysOfYears_SelectionChanged;

                #endregion Events

                #region Style 

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

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewBuysOverviewOfAYears);
                dataGridViewBuysOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlBuys.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlBuys;

                #endregion Control add

                // Check if buys exists
                if (ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Count > 0)
                {
                    // Loop through the years of the buys
                    foreach (
                        var keyName in ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Keys.Reverse()
                        )
                    {
                        #region Add page

                        // Create TabPage
                        TabPage newTabPage = new TabPage();
                        // Set TabPage name
                        newTabPage.Name = keyName;
                        newTabPage.Text = keyName + " (" +
                                          ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary[keyName]
                                          .BuyMarketValueReductionYearAsStrUnit
                                          + ")";

                        #endregion Add page

                        #region Data source, data binding and data grid view

                        // Create Binding source for the buy data
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource =
                            ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary[keyName].BuyListYear;

                        // Create DataGridView
                        DataGridView dataGridViewBuysOfAYear = new DataGridView();
                        dataGridViewBuysOfAYear.Dock = DockStyle.Fill;
                        // Bind source with buy values to the DataGridView
                        dataGridViewBuysOfAYear.DataSource = bindingSource;

                        #endregion Data source, data binding and data grid view

                        #region Events

                        // Set the delegate for the DataBindingComplete event
                        dataGridViewBuysOfAYear.DataBindingComplete += OnDataGridViewBuys_DataBindingComplete;
                        // Set the delegate for the mouse enter event
                        dataGridViewBuysOfAYear.MouseEnter += OnDataGridViewBuysOfAYear_MouseEnter;
                        // Set the delegate for the mouse leave event
                        dataGridViewBuysOfAYear.MouseLeave += OnDataGridViewBuysOfAYear_MouseLeave;
                        // Set row select event
                        dataGridViewBuysOfAYear.SelectionChanged += OnDataGridViewBuysOfAYear_SelectionChanged;
                        // Set cell decimal click event
                        dataGridViewBuysOfAYear.CellContentDoubleClick += OnDataGridViewBuysOfAYear_CellContentdecimalClick;

                        #endregion Events

                        #region Style

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

                        #endregion Style

                        #region Control add

                        newTabPage.Controls.Add(dataGridViewBuysOfAYear);
                        dataGridViewBuysOfAYear.Parent = newTabPage;
                        tabCtrlBuys.Controls.Add(newTabPage);
                        newTabPage.Parent = tabCtrlBuys;

                        #endregion Control add
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", LanguageName),
                   Language, LanguageName, Color.DarkRed, Logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewBuys_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Date",
                                    LanguageName);
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Volume",
                                    LanguageName) + @" (" + ShareObject.PieceUnit + @")";
                            ;
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Price",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Value",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 4:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Document",
                                    LanguageName);
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                    ((DataGridView)sender).Rows[0].Selected = false;

                // Reset the text box values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewBuysOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/RenameColHeaderFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        private void DeselectRowsOfDataGridViews(DataGridView DataGridView)
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

                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("DeselectRowsOfDataGridViews()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeselectFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
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
                txtBoxDocument.Text = Helper.SetDocument(Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/OpenFileDialog/Title", LanguageName), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnBuyDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ChoseDocumentFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        #region Tab control delegates

        /// <summary>
        /// This function sets the focus on the data grid view of the new selected tab page of the tab control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void tabCtrlBuys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlBuys.SelectedTab != null)
            {
                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlBuys.SelectedTab.Controls)
                {
                    if (control is DataGridView)
                        ((DataGridView)control).Focus();
                }
            }
        }

        /// <summary>
        /// This function sets the focus on the data grid view of the current selected tab page of the tab control
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
        private void tabCtrlBuys_MouseEnter(object sender, EventArgs e)
        {
            if (tabCtrlBuys.SelectedTab != null)
            {
                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlBuys.SelectedTab.Controls)
                {
                    if (control is DataGridView)
                        ((DataGridView)control).Focus();
                }
            }
        }

        /// <summary>
        /// This function sets the focus to the group box add / edit when the mouse leaves the data grid view
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void tabCtrlBuys_MouseLeave(object sender, EventArgs e)
        {
            grpBoxAdd.Focus();
        }

        #endregion Tab control delegates

        #region Buys of years

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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewBuysOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewBuysOfYears_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
        }

        #endregion Buys of years

        #region Buys of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
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
                    BuyObject selectedBuyObject = ShareObjectFinalValue.AllBuyEntries.GetBuyObjectByDateTime(SelectedDate);

                    // Get CostObject of the selected DataGridView row if a cost is set by this buy
                    CostObject selectedCostObject = ShareObjectFinalValue.AllCostsEntries.GetCostObjectByDateTime(SelectedDate);

                    // Set buy values
                    if (selectedBuyObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        txtBoxVolume.Text = selectedBuyObject.VolumeAsStr;
                        txtBoxFinalValue.Text = selectedBuyObject.MarketValueReductionCostsAsStr;
                        txtBoxReduction.Text = selectedBuyObject.ReductionAsStr;
                        txtBoxPrice.Text = selectedBuyObject.SharePriceAsStr;
                        txtBoxDocument.Text = selectedBuyObject.Document;
                    }
                    else
                    {
                        datePickerDate.Value = Convert.ToDateTime(SelectedDate);
                        datePickerTime.Value = Convert.ToDateTime(SelectedDate);
                        txtBoxVolume.Text = curItem[0].Cells[1].Value.ToString();
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

                    if (ShareObjectFinalValue.AllBuyEntries.IsDateLastDate(SelectedDate) &&
                        ShareObjectFinalValue.AllSaleEntries.IsDateLastDate(SelectedDate)
                        )
                    {
                        if (ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare().Count > 1)
                            // Enable Button(s)
                            btnDelete.Enabled = true;
                        else
                            // Enable Button(s)
                            btnDelete.Enabled = false;

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
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;

                    // Format the input value
                    if (FormatInputValues != null)
                        FormatInputValues(this, new EventArgs());
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
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
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                ResetInputValues();

                ShowBuys();
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewBuysOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewBuysOfAYear_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
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
                            foreach (var temp in ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare())
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
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                                LanguageName);
                                        string strMessage =
                                            Language.GetLanguageTextByXPath(
                                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                                LanguageName);
                                        string strOk =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                                LanguageName);
                                        string strCancel =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                                LanguageName);

                                        OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                                            strCancel);
                                        if (messageBox.ShowDialog() == DialogResult.OK)
                                        {
                                            // Remove buy object and add it with no document
                                            if (ShareObjectFinalValue.RemoveBuy(temp.Date) &&
                                                ShareObjectFinalValue.AddBuy(strDateTime, temp.Volume, temp.Reduction, temp.Costs, temp.MarketValue))
                                            {
                                                // Set flag to save the share object.
                                                _bSave = true;

                                                ResetInputValues();
                                                ShowBuys();

                                                // Add status message
                                                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                                                    Language.GetLanguageTextByXPath(
                                                        @"/AddEditFormBuy/StateMessages/EditSuccess", LanguageName),
                                                    Language, LanguageName,
                                                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                                    (int)FrmMain.EComponentLevels.Application);
                                            }
                                            else
                                            {
                                                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                                                    Language.GetLanguageTextByXPath(
                                                        @"/AddEditFormBuy/Errors/EditFailed", LanguageName),
                                                    Language, LanguageName,
                                                    Color.Red, Logger, (int)FrmMain.EStateLevels.Error,
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Buys of a year

        #endregion Data grid view

        #endregion Methods
    }
}