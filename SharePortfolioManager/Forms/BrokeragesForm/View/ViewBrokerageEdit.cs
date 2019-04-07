//MIT License
//
//Copyright(c) 2019 nessie1980(nessie1980 @gmx.de)
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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Properties;

namespace SharePortfolioManager.Forms.BrokeragesForm.View
{
    // Error codes of the BrokerageEdit
    public enum BrokerageErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        InputValuesInvalid,
        ProvisionWrongFormat,
        ProvisionWrongValue,
        BrokerFeeWrongFormat,
        BrokerFeeWrongValue,
        TraderPlaceFeeWrongFormat,
        TraderPlaceFeeWrongValue,
        ReductionWrongFormat,
        ReductionWrongValue,
        BrokerageEmpty,
        BrokerageWrongFormat,
        BrokerageWrongValue,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExists,
        DocumentFileDoesNotExists
    };

    /// <inheritdoc />
    /// <summary>
    /// Interface of the BrokerageEdit view
    /// </summary>
    public interface IViewBrokerageEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler AddBrokerageEventHandler;
        event EventHandler EditBrokerageEventHandler;
        event EventHandler DeleteBrokerageEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        BrokerageErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool UpdateBrokerage { get; set; }
        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }
        bool PartOfABuy { get; set; }
        bool PartOfASale { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Provision { get; set; }
        string BrokerFee { get; set; }
        string TraderPlaceFee { get; set; }
        string Brokerage { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
        void DocumentBrowseFinish();
    }

    public partial class ViewBrokerageEdit : Form, IViewBrokerageEdit
    {
        #region Fields

        /// <summary>
        /// Stores the Guid of a selected brokerage row
        /// </summary>
        private string _selectedGuid;

        /// <summary>
        /// Stores the date of a selected brokerage row
        /// </summary>
        private string _selectedDate;

        /// <summary>
        /// Stores the last focused date time picker or text box
        /// </summary>
        private Control _focusedControl;

        #endregion Fields

        #region Properties

        #region Transfer parameter

        public Logger Logger { get; internal set; }

        public Language Language { get; internal set; }

        public string LanguageName { get; internal set; }

        #endregion Transfer parameter

        #region Flags

        public bool UpdateBrokerageFlag { get; set; }

        public bool SaveFlag { get; set; }

        #endregion Flags

        public DataGridView SelectedDataGridView { get; internal set; }

        #endregion Properties

        #region IView members

        public bool UpdateBrokerage
        {
            get => UpdateBrokerageFlag;
            set
            {
                UpdateBrokerageFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateBrokerage"));
            }
        }

        public bool PartOfABuy { get; set; }

        public bool PartOfASale { get; set; }

        public BrokerageErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; }

        public string SelectedGuid
        {
            get => _selectedGuid;
            set
            {
                _selectedGuid = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedGuid"));
            }
        }

        public string SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedDate"));
            }
        }

        #region Input values

        public string Date
        {
            get => datePickerDate.Text;
            set
            {
                if (datePickerDate.Text == value)
                    return;
                datePickerDate.Text = value;
            }
        }

        public string Time
        {
            get => datePickerTime.Text;
            set
            {
                if (datePickerTime.Text == value)
                    return;
                datePickerTime.Text = value;
            }
        }

        public string Provision
        {
            get => txtBoxBrokerage.Text;
            set
            {
                if (txtBoxBrokerage.Text == value)
                    return;
                txtBoxBrokerage.Text = value;
            }
        }

        public string BrokerFee
        {
            get => txtBoxBrokerage.Text;
            set
            {
                if (txtBoxBrokerage.Text == value)
                    return;
                txtBoxBrokerage.Text = value;
            }
        }

        public string TraderPlaceFee
        {
            get => txtBoxBrokerage.Text;
            set
            {
                if (txtBoxBrokerage.Text == value)
                    return;
                txtBoxBrokerage.Text = value;
            }
        }

        public string Reduction
        {
            get => txtBoxReduction.Text;
            set
            {
                if (txtBoxReduction.Text == value)
                    return;
                txtBoxReduction.Text = value;
            }
        }

        public string Brokerage
        {
            get => txtBoxBrokerage.Text;
            set
            {
                if (txtBoxBrokerage.Text == value)
                    return;
                txtBoxBrokerage.Text = value;
            }
        }

        public string Document
        {
            get => txtBoxDocument.Text;
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
            var strMessage = @"";
            var clrMessage = Color.Black;
            var stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case BrokerageErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the brokerage list
                        OnShowBrokerage();

                        break;
                    }
                case BrokerageErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add",
                                LanguageName);
                        btnAddSave.Image = Resources.button_add_24;
                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/StateMessages/EditSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the brokerage list
                        OnShowBrokerage();

                        break;
                    }
                case BrokerageErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.button_add_24;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the brokerage list
                        OnShowBrokerage();

                        break;
                    }
                case BrokerageErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.ProvisionWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ProvisionWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.ProvisionWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ProvisionWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerFeeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerFeeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerFeeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerFeeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.TraderPlaceFeeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/TraderPlaceFeeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTraderPlaceFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.TraderPlaceFeeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/TraderPlaceFeeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTraderPlaceFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.ReductionWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ReductionWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxReduction.Focus();

                        break;
                    }
                case BrokerageErrorCode.ReductionWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ReductionWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxReduction.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerageEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerageEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerage.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerageWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerageWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerage.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerageWrongValue:
                    {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerageWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case BrokerageErrorCode.DocumentDirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DocumentDirectoryDoesNotExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
                case BrokerageErrorCode.DocumentFileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DocumentFileDoesNotExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
               strMessage,
               Language,
               LanguageName,
               clrMessage,
               Logger,
               (int)stateLevel,
               (int)FrmMain.EComponentLevels.Application);
        }

        public void DocumentBrowseFinish()
        {
            // Set messages
            var strMessage = @"";
            var clrMessage = Color.Black;
            const FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case BrokerageErrorCode.DocumentBrowseFailed:
                {
                    txtBoxDocument.Text = @"-";

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ChoseDocumentFailed", LanguageName);
                    break;
                }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
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
        public event EventHandler FormatInputValuesEventHandler;
        public event EventHandler AddBrokerageEventHandler;
        public event EventHandler EditBrokerageEventHandler;
        public event EventHandler DeleteBrokerageEventHandler;
        public event EventHandler DocumentBrowseEventHandler;

        #endregion

        #region Methods

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market value share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final value share object</param>
        /// <param name="logger">Logger</param>
        /// <param name="language">Language file</param>
        /// <param name="languageName">Language</param>
        public ViewBrokerageEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue, Logger logger, Language language, string languageName)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;

            Logger = logger;
            Language = language;
            LanguageName = languageName;

            _focusedControl = txtBoxBrokerage;

            SaveFlag = false;
        }

        /// <summary>
        /// This function sets the language to the labels and buttons
        /// and sets the units behind the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareBrokerageEdit_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Caption", LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxOverview.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxBrokerage/Caption",
                        LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                chkBoxBuyPart.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/BuyPart",
                    LanguageName);
                chkBoxSalePart.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/SalePart",
                    LanguageName);
                lblProvision.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/Provision",
                    LanguageName);
                lblBrokerFee.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/BrokerFee",
                    LanguageName);
                lblTraderPlaceFee.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/TraderPlaceFee",
                    LanguageName);
                lblReduction.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/Reduction",
                    LanguageName);
                lblBrokerage.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/Brokerage",
                        LanguageName);
                lblDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add",
                    LanguageName);
                btnReset.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Reset",
                    LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                #endregion Language configuration

                #region Unit configuration

                // Set brokerage unit to the edit box
                lblAddProvisionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddBrokerFeeUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddTraderPlaceFeeUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddReductionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddBrokerageUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.button_save_24;
                btnDelete.Image = Resources.button_recycle_bin_24;
                btnReset.Image = Resources.button_reset_24;
                btnCancel.Image = Resources.button_back_24;

                #endregion Image configuration

                // Load brokerage of the share
                OnShowBrokerage();
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"ShareBrokerageEdit_Load()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ShowFailed", LanguageName),
                   Language, LanguageName, Color.DarkRed, Logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a brokerage has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareBrokerageEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if a brokerage change must be saved
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date.
        /// </summary>
        private void ResetInputValues()
        {
            // Reset date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerDate.Enabled = true;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            datePickerTime.Enabled = true;

            // Reset text boxes
            txtBoxProvision.Text = @"";
            txtBoxProvision.Enabled = true;
            txtBoxBrokerFee.Text = @"";
            txtBoxBrokerFee.Enabled = true;
            txtBoxTraderPlaceFee.Text = @"";
            txtBoxTraderPlaceFee.Enabled = true;
            txtBoxReduction.Text = @"";
            txtBoxReduction.Enabled = true;
            txtBoxBrokerage.Text = @"-";
            txtBoxBrokerage.Enabled = true;
            txtBoxDocument.Text = @"";
            txtBoxDocument.Enabled = true;

            toolStripStatusLabelMessageBrokerageEdit.Text = @"";

            // Enable button(s)
            btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add", LanguageName);
            btnAddSave.Image = Resources.button_add_24;

            // Disable button(s)
            btnDelete.Enabled = false;

            // Rename group box
            grpBoxAdd.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption", LanguageName);

            // Deselect rows
            OnDeselectRowsOfDataGridViews(null);

            // Reset stored DataGridView instance
            SelectedDataGridView = null;

            // Select overview tab
            if (tabCtrlBrokerage.TabPages.Count > 0)
                tabCtrlBrokerage.SelectTab(0);

            txtBoxProvision.Focus();

            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Form

        #region Date Time

        /// <summary>
        /// This function updates the model if the date has changed
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerDate_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerDate_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the date time picker for the date to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerDate_Enter(object sender, EventArgs e)
        {
            _focusedControl = datePickerDate;
        }

        /// <summary>
        /// This function updates the model if the time has changed
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerTime_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">DateTime picker</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerTime_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the date time picker for the time to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnDatePickerTime_Enter(object sender, EventArgs e)
        {
            _focusedControl = datePickerTime;
        }

        #endregion Date Time

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxProvision_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Provision"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxProvision_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxProvision_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxProvision;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerFee_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BrokerFee"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerFee_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerFee_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxBrokerFee;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TraderPlaceFee"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTraderPlaceFee_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxTraderPlaceFee;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReduction_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reduction"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReduction_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxReduction_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxReduction;
        }

        /// <summary>
        /// This function only sets the document of the model to the view
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Document"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxDocument;
        }

        /// <summary>
        /// This function allows to sets via Drag and Drop a document for this brokerage
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// This function shows the Drop sign
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length <= 0 || files.Length > 1) return;

            txtBoxDocument.Text = files[0];

            // TODO Parse PDF and set values to the form
            //var pdf = new PdfDocument(new PdfReader(txtBoxDocument.Text));
            //var text = PdfTextExtractor.GetTextFromPage(pdf.GetPage(1), new LocationTextExtractionStrategy());
            //pdf.Close();
            //Console.WriteLine(@"Extracted text:");
            //Console.WriteLine(text);
        }

        #endregion TextBoxes

        #region Buttons

        /// <summary>
        /// This function adds a new brokerage entry to the share object
        /// or edit a brokerage entry
        /// It also checks if an entry already exists for the given date and time
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls
                Enabled = false;

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    UpdateBrokerage = false;

                    AddBrokerageEventHandler?.Invoke(this, null);
                }
                else
                {
                    UpdateBrokerage = true;

                    EditBrokerageEventHandler?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"btnAdd_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/AddFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the brokerage.
        /// If the user press "Ok" the brokerage will be deleted and the
        /// list of the brokerage will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">EventArgs</param>
        private void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                // Disable controls
                Enabled = false;

                toolStripStatusLabelMessageBrokerageEdit.Text = @"";

                var strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/BrokerageDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult != DialogResult.OK) return;

                DeleteBrokerageEventHandler?.Invoke(this, null);
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"btnDelete_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DeleteFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a brokerage
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"btnReset_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/CancelFailure", LanguageName),
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

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the brokerage
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnBrokerageDocumentBrowse_Click(object sender, EventArgs e)
        {
            DocumentBrowseEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Buttons

        #region Group box overview

        private void OnGrpBoxOverview_MouseLeave(object sender, EventArgs e)
        {
            if (_focusedControl is TextBox box)
            {
                box.Select();
                box.Select(box.Text.Length, 0); // To set cursor at the end of TextBox
            }
            else
            {
                _focusedControl?.Focus();
            }
        }

        #endregion Group box overview

        #region Data grid view

        /// <summary>
        /// This function paints the brokerage list of the share
        /// </summary>
        private void OnShowBrokerage()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlBrokerage.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBrokerageOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBrokerageOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBrokerageOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBrokerageOfAYear_MouseEnter;
                            dataGridView.MouseLeave -= OnDataGridViewBrokerageOfAYear_MouseEnter;
                            dataGridView.CellContentDoubleClick -= OnDataGridViewBrokerageOfAYear_CellContentDecimalClick;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewBrokerage_DataBindingComplete;
                    }
                    tabPage.Controls.Clear();
                    tabCtrlBrokerage.TabPages.Remove(tabPage);
                }

                tabCtrlBrokerage.TabPages.Clear();

                // Enable controls
                Enabled = true;

                #region Add page

                // Create TabPage for the brokerage of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/TabPgOverview/Overview", 
                               LanguageName)
                           + @" (" + ShareObjectFinalValue.AllBrokerageEntries.BrokerageValueTotalWithUnitAsStr +
                           @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Create Binding source for the brokerage data
                var bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues();

                // Create DataGridView
                var dataGridViewBrokerageOverviewOfAYears = new DataGridView
                {
                    // TODO correct
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with brokerage values to the DataGridView
                    DataSource = bindingSourceOverview
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewBrokerageOverviewOfAYears.DataBindingComplete += OnDataGridViewBrokerage_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewBrokerageOverviewOfAYears.MouseEnter += OnDataGridViewBrokerageOfYears_MouseEnter;
                // Set row select event
                dataGridViewBrokerageOverviewOfAYears.SelectionChanged += OnDataGridViewBrokerageOfYears_SelectionChanged;

                #endregion Events

                #region Style 

                // Advanced configuration DataGridView brokerage
                dataGridViewBrokerageOverviewOfAYears.EnableHeadersVisualStyles = false;
                // Column header styling
                dataGridViewBrokerageOverviewOfAYears.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewBrokerageOverviewOfAYears.ColumnHeadersDefaultCellStyle.BackColor =
                    SystemColors.ControlLight;
                dataGridViewBrokerageOverviewOfAYears.ColumnHeadersHeight = 25;
                dataGridViewBrokerageOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                // Column styling
                dataGridViewBrokerageOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // Row styling
                dataGridViewBrokerageOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewBrokerageOverviewOfAYears.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewBrokerageOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewBrokerageOverviewOfAYears.MultiSelect = false;
                dataGridViewBrokerageOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // Cell styling

                dataGridViewBrokerageOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewBrokerageOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                dataGridViewBrokerageOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewBrokerageOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // Allow styling
                dataGridViewBrokerageOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewBrokerageOverviewOfAYears.AllowUserToResizeRows = false;
                dataGridViewBrokerageOverviewOfAYears.AllowUserToAddRows = false;
                dataGridViewBrokerageOverviewOfAYears.AllowUserToDeleteRows = false;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewBrokerageOverviewOfAYears);
                dataGridViewBrokerageOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlBrokerage.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlBrokerage;

                #endregion Control add

                // Check if brokerage exists
                if (ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary.Count <= 0) return;

                // Loop through the years of the brokerage
                foreach (
                    var keyName in ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary.Keys.Reverse()
                )
                {
                    #region Add page

                    // Create TabPage
                    var newTabPage = new TabPage
                    {
                        // Set TabPage name
                        Name = keyName,

                        // Set TabPage caption
                        Text = keyName + @" (" +
                               ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName]
                                   .BrokerageValueYearWithUnitAsStr
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Create Binding source for the brokerage data
                    var bindingSource = new BindingSource
                    {
                        DataSource = 
                            ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName]
                                .BrokerageReductionListYear
                    };

                    // Create DataGridView
                    var dataGridViewBrokerageOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with brokerage values to the DataGridView
                        DataSource = bindingSource
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewBrokerageOfAYear.DataBindingComplete += OnDataGridViewBrokerage_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewBrokerageOfAYear.MouseEnter += OnDataGridViewBrokerageOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewBrokerageOfAYear.SelectionChanged += OnDataGridViewBrokerageOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewBrokerageOfAYear.CellContentDoubleClick += OnDataGridViewBrokerageOfAYear_CellContentDecimalClick;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView brokerage
                    dataGridViewBrokerageOfAYear.EnableHeadersVisualStyles = false;
                    // Column header styling
                    dataGridViewBrokerageOfAYear.ColumnHeadersDefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewBrokerageOfAYear.ColumnHeadersDefaultCellStyle.BackColor =
                        SystemColors.ControlLight;
                    dataGridViewBrokerageOfAYear.ColumnHeadersHeight = 25;
                    dataGridViewBrokerageOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    // Column styling
                    dataGridViewBrokerageOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Row styling
                    dataGridViewBrokerageOfAYear.RowHeadersVisible = false;
                    dataGridViewBrokerageOfAYear.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewBrokerageOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridViewBrokerageOfAYear.MultiSelect = false;
                    dataGridViewBrokerageOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    // Cell styling

                    dataGridViewBrokerageOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dataGridViewBrokerageOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                    dataGridViewBrokerageOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    dataGridViewBrokerageOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    // Allow styling
                    dataGridViewBrokerageOfAYear.AllowUserToResizeColumns = false;
                    dataGridViewBrokerageOfAYear.AllowUserToResizeRows = false;
                    dataGridViewBrokerageOfAYear.AllowUserToAddRows = false;
                    dataGridViewBrokerageOfAYear.AllowUserToDeleteRows = false;

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewBrokerageOfAYear);
                    dataGridViewBrokerageOfAYear.Parent = newTabPage;
                    tabCtrlBrokerage.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlBrokerage;

                    #endregion Control add
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"ShowBrokerage()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ShowFailed", LanguageName),
                   Language, LanguageName, Color.DarkRed, Logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewBrokerage_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Set column headers
                for (var i = 0; i < ((DataGridView)sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView)sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                        {
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Year",
                                        LanguageName);
                            }
                        }
                        break;
                        case 1:
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Brokerage",
                                        LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            }
                            else
                            {
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Date",
                                    LanguageName);
                            }
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Brokerage",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Document",
                                    LanguageName);
                            break;
                    }
                }

                if (((DataGridView)sender).Rows.Count > 0)
                {
                    ((DataGridView)sender).Rows[0].Selected = false;
                    ((DataGridView)sender).ScrollBars = ScrollBars.Both;
                }

                if (((DataGridView)sender).Name != @"Overview")
                    ((DataGridView)sender).Columns[0].Visible = false;

                // Reset the text box values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"dataGridViewBrokerageOfAYear_DataBindingComplete()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/RenameColHeaderFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        private void OnDeselectRowsOfDataGridViews(DataGridView dataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage tabPage in tabCtrlBrokerage.TabPages)
                {
                    foreach (Control control in tabPage.Controls)
                    {
                        if (!(control is DataGridView view) || view == dataGridView) continue;

                        foreach (DataGridViewRow selectedRow in view.SelectedRows)
                        {
                            selectedRow.Selected = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"DeselectRowsOfDataGridViews()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DeselectFailed", LanguageName),
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
        private void OnTabCtrlBrokerage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlBrokerage.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabCtrlBrokerage.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                {
                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                        view.Rows[0].Selected = true;
                    view.Focus();

                    if (view.Name == @"Overview")
                        ResetInputValues();
                }
            }
        }

        /// <summary>
        /// This function sets the focus back to the last focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlBrokerage_MouseLeave(object sender, EventArgs e)
        {
            if (_focusedControl is TextBox box)
            {
                box.Select();
                box.Select(box.Text.Length, 0); // To set cursor at the end of TextBox
            }
            else
            {
                _focusedControl?.Focus();
            }
        }

        /// <summary>
        /// This function sets the focus on the last focused control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlBrokerage_KeyDown(object sender, KeyEventArgs e)
        {
            if (_focusedControl is TextBox box)
            {
                box.Select();
                box.Select(box.Text.Length, 0); // To set cursor at the end of TextBox
            }
            else
            {
                _focusedControl?.Focus();
            }
        }

        /// <summary>
        /// This function sets key value ( char ) to the last focused control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlBrokerage_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the last focused control was a text box so set the
            // key value ( char ) to the text box and then set the cursor behind the text
            if (_focusedControl is TextBox textBox)
            {
                textBox.Text = e.KeyChar.ToString();
                textBox.Select(textBox.Text.Length, 0);
            }

            // Check if the last focused control was a date time picker
            if (_focusedControl is DateTimePicker dateTimePicker)
            {
                // Check if the pressed key was a numeric key
                if (e.KeyChar == '0' ||
                    e.KeyChar == '1' ||
                    e.KeyChar == '2' ||
                    e.KeyChar == '3' ||
                    e.KeyChar == '4' ||
                    e.KeyChar == '5' ||
                    e.KeyChar == '6' ||
                    e.KeyChar == '7' ||
                    e.KeyChar == '8' ||
                    e.KeyChar == '9'
                )
                    dateTimePicker.Text = e.KeyChar.ToString();
            }
        }

        #endregion Tab control delegates

        #region Brokerage of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewBrokerageOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlBrokerage.TabPages)
                {
                    if (curItem[0].Cells[1].Value != null && 
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    tabCtrlBrokerage.SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"dataGridViewBrokerageOfYears_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewBrokerageOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        #endregion Brokerage of years

        #region Brokerage of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewBrokerageOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlBrokerage.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlBrokerage.SelectedTab.Controls.Contains((DataGridView)sender))
                        OnDeselectRowsOfDataGridViews((DataGridView)sender);
                }

                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    var curItem = ((DataGridView)sender).SelectedRows;

                    // Set selected date
                    if (curItem[0].Cells[1].Value != null)
                        SelectedDate = curItem[0].Cells[1].Value.ToString();
                    else
                        return;

                    // Get list of brokerage of a year
                    DateTime.TryParse(SelectedDate, out var dateTime);
                    var brokerageListYear =  ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[dateTime.Year.ToString()]
                        .BrokerageReductionListYear;

                    var index = ((DataGridView) sender).SelectedRows[0].Index;

                    // Set selected Guid
                    SelectedGuid = brokerageListYear[index].Guid;

                    // Get BrokerageObject of the selected DataGridView row
                    var selectedBrokerageObject = brokerageListYear[index];

                    // Set brokerage values
                    if (selectedBrokerageObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(selectedBrokerageObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(selectedBrokerageObject.Date);
                        chkBoxBuyPart.CheckState = PartOfABuy ? CheckState.Checked : CheckState.Unchecked;
                        chkBoxSalePart.CheckState = PartOfASale ? CheckState.Checked : CheckState.Unchecked;
                        txtBoxBrokerage.Text = selectedBrokerageObject.BrokerageValueAsStr;
                        txtBoxDocument.Text = selectedBrokerageObject.BrokerageDocument;
                    }
                    else
                    {
                        datePickerDate.Value = Convert.ToDateTime(SelectedDate);
                        datePickerTime.Value = Convert.ToDateTime(SelectedDate);
                        chkBoxBuyPart.CheckState = CheckState.Unchecked;
                        chkBoxSalePart.CheckState = CheckState.Unchecked;
                        txtBoxBrokerage.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[2].Value.ToString();
                    }

                    // Set brokerage values
                    if (selectedBrokerageObject != null && 
                        (selectedBrokerageObject.PartOfABuy || selectedBrokerageObject.PartOfASale)
                        )
                    {
                        // Set flag if brokerage is part of a buy
                        PartOfABuy = selectedBrokerageObject.PartOfABuy;

                        // Set flag if brokerage is part of a sale
                        PartOfASale = selectedBrokerageObject.PartOfASale;

                        // Disable TextBox(es)
                        datePickerDate.Enabled = false;
                        datePickerTime.Enabled = false;
                        txtBoxProvision.Enabled = false;
                        txtBoxBrokerFee.Enabled = false;
                        txtBoxTraderPlaceFee.Enabled = false;
                        txtBoxReduction.Enabled = false;
                        txtBoxDocument.Enabled = false;

                        // Enable Button(s)
                        btnDocumentBrowse.Enabled = false;
                        btnAddSave.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        if (selectedBrokerageObject != null)
                        {
                            // Set flag if brokerage is part of a buy
                            PartOfABuy = selectedBrokerageObject.PartOfABuy;

                            // Set flag if brokerage is part of a sale
                            PartOfASale = selectedBrokerageObject.PartOfASale;

                            // Enable TextBox(es)
                            datePickerDate.Enabled = true;
                            datePickerTime.Enabled = true;
                            txtBoxProvision.Enabled = true;
                            txtBoxBrokerFee.Enabled = true;
                            txtBoxTraderPlaceFee.Enabled = true;
                            txtBoxReduction.Enabled = true;
                            txtBoxDocument.Enabled = true;

                            // Enable Button(s)
                            btnDocumentBrowse.Enabled = true;
                            btnAddSave.Enabled = true;
                            btnDelete.Enabled = true;
                        }
                    }

                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.button_pencil_24;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;

                    // Format the input value
                    FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Enable Button(s)
                    btnAddSave.Enabled = true;
                    btnDocumentBrowse.Enabled = true;

                    // Enabled TextBox(es)
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;
                    txtBoxProvision.Enabled = true;
                    txtBoxBrokerFee.Enabled = true;
                    txtBoxTraderPlaceFee.Enabled = true;
                    txtBoxReduction.Enabled = true;

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"dataGridViewBrokerageOfAYear_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                ResetInputValues();

                OnShowBrokerage();
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewBrokerageOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function opens the brokerage document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewBrokerageOfAYear_CellContentDecimalClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                var iColumnCount = ((DataGridView)sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex != iColumnCount - 1) return;

                // Check if a row is selected
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the current selected row
                var curItem = ((DataGridView)sender).SelectedRows;
                // Get Guid of the selected buy item
                var strGuidBuy = curItem[0].Cells[0].Value.ToString();
                // Get date and time of the selected brokerage item
                var strDateTime = curItem[0].Cells[1].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value.ToString() == @"-") return;

                // Get doc from the brokerage with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageOfTheShare())
                {
                    // Check if the brokerage Guid is the same as the Guid of the clicked brokerage item
                    if (temp.Guid != strGuidBuy) continue;

                    // Check if the file still exists
                    if (File.Exists(temp.BrokerageDocument))
                        // Open the file
                        Process.Start(temp.BrokerageDocument);
                    else
                    {
                        var strCaption =
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                LanguageName);
                        var strMessage =
                            Language.GetLanguageTextByXPath(
                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                LanguageName);
                        var strOk =
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                LanguageName);
                        var strCancel =
                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                LanguageName);

                        var messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                            strCancel);
                        if (messageBox.ShowDialog() == DialogResult.OK)
                        {
                            // Generate Guid
                            var strGuidBrokerage = Guid.NewGuid().ToString();

                            // Remove brokerage object and add it with no document
                            if (ShareObjectFinalValue.RemoveBrokerage(temp.Guid ,temp.Date) &&
                                ShareObjectFinalValue.AddBrokerage(temp.Guid, false, false, strGuidBrokerage,
                                    strDateTime, temp.ProvisionValue, temp.BrokerFeeValue, temp.TraderPlaceFeeValue, temp.ReductionValue))
                            {
                                // Set flag to save the share object.
                                SaveFlag = true;

                                ResetInputValues();
                                OnShowBrokerage();

                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/StateMessages/EditSuccess", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                            else
                            {
                                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/Errors/EditFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)FrmMain.EStateLevels.Error,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                        }
                    }

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BROKERAGE || DEBUG
                var message = $"dataGridViewBrokerageOfAYear_CellContentDecimalClick()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Brokerage of a year

        #endregion Data grid view

        #endregion Methods
    }
}
