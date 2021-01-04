//MIT License
//
//Copyright(c) 2017 - 2021 nessie1980(nessie1980 @gmx.de)
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
//#define DEBUG_BROKERAGE_EDIT_VIEW

using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.OwnMessageBoxForm;
using SharePortfolioManager.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager.BrokeragesForm.View
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
        string Reduction { get; set; }
        string BrokerageReduction { get; set; }
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

        /// <summary>
        /// Flag if the show of the buy is running ( true ) or finished ( false )
        /// </summary>
        public bool ShowBrokerageFlag;

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
            get => txtBoxProvision.Text;
            set
            {
                if (txtBoxProvision.Text == value)
                    return;
                txtBoxProvision.Text = value;
            }
        }

        public string BrokerFee
        {
            get => txtBoxBrokerFee.Text;
            set
            {
                if (txtBoxBrokerFee.Text == value)
                    return;
                txtBoxBrokerFee.Text = value;
            }
        }

        public string TraderPlaceFee
        {
            get => txtBoxTraderPlaceFee.Text;
            set
            {
                if (txtBoxTraderPlaceFee.Text == value)
                    return;
                txtBoxTraderPlaceFee.Text = value;
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

        public string BrokerageReduction
        {
            get => txtBoxBrokerageReduction.Text;
            set
            {
                if (txtBoxBrokerageReduction.Text == value)
                    return;
                txtBoxBrokerageReduction.Text = value;
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
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/StateMessages/AddSuccess", SettingsConfiguration.LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the brokerage list
                        OnShowBrokerage();

                        break;
                    }
                case BrokerageErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/AddFailed", SettingsConfiguration.LanguageName);
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

                        // Reset dialog icon to add
                        Icon = Resources.add;

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/StateMessages/EditSuccess", SettingsConfiguration.LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the brokerage list
                        OnShowBrokerage();

                        break;
                    }
                case BrokerageErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/EditFailed", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/StateMessages/DeleteSuccess", SettingsConfiguration.LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName);
                        btnAddSave.Image = Resources.button_add_24;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);

                        // Reset dialog icon to add
                        Icon = Resources.add;

                        // Refresh the brokerage list
                        OnShowBrokerage();

                        break;
                    }
                case BrokerageErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DeleteFailed", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/CheckInputFailure", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.ProvisionWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ProvisionWrongFormat", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.ProvisionWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ProvisionWrongValue", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxProvision.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerFeeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerFeeWrongFormat", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerFeeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerFeeWrongValue", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.TraderPlaceFeeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/TraderPlaceFeeWrongFormat", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTraderPlaceFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.TraderPlaceFeeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/TraderPlaceFeeWrongValue", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTraderPlaceFee.Focus();

                        break;
                    }
                case BrokerageErrorCode.ReductionWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ReductionWrongFormat", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxReduction.Focus();

                        break;
                    }
                case BrokerageErrorCode.ReductionWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ReductionWrongValue", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxReduction.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerageEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerageEmpty", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerage.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerageWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerageWrongFormat", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxBrokerage.Focus();

                        break;
                    }
                case BrokerageErrorCode.BrokerageWrongValue:
                    {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/BrokerageWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case BrokerageErrorCode.DocumentDirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DocumentDirectoryDoesNotExists", SettingsConfiguration.LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
                case BrokerageErrorCode.DocumentFileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DocumentFileDoesNotExists", SettingsConfiguration.LanguageName);
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
                        Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ChoseDocumentFailed", SettingsConfiguration.LanguageName);
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

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Caption", SettingsConfiguration.LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);
                grpBoxDocumentPreview.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxDocumentPreview/Caption", SettingsConfiguration.LanguageName);
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
                lblBrokerageReduction.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Labels/BrokerageMinusReduction",
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
                lblAddBrokerageReductionUnit.Text = ShareObjectFinalValue.CurrencyUnit;

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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ShowFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName, Color.DarkRed, Logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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
            // Cleanup web browser
            Helper.WebBrowserPdf.CleanUp(webBrowser1);

            // Check if a brokerage change must be saved
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date.
        /// </summary>
        private void ResetValues()
        {
            // Reset and enable date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerDate.Enabled = true;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            datePickerTime.Enabled = true;

            // Reset check boxes
            chkBoxBuyPart.CheckState = CheckState.Unchecked;
            chkBoxSalePart.CheckState = CheckState.Unchecked;

            // Reset and enable text boxes
            txtBoxProvision.Text = string.Empty;
            txtBoxProvision.Enabled = true;
            txtBoxBrokerFee.Text = string.Empty;
            txtBoxBrokerFee.Enabled = true;
            txtBoxTraderPlaceFee.Text = string.Empty;
            txtBoxTraderPlaceFee.Enabled = true;
            txtBoxBrokerage.Text = string.Empty;
            txtBoxBrokerage.Enabled = true;
            txtBoxReduction.Text = string.Empty;
            txtBoxReduction.Enabled = true;
            txtBoxBrokerageReduction.Text = string.Empty;
            txtBoxBrokerageReduction.Enabled = true;
            txtBoxDocument.Text = string.Empty;
            txtBoxDocument.Enabled = true;

            // Reset status label message text
            toolStripStatusLabelMessageBrokerageEdit.Text = string.Empty;

            // Enable button(s)
            btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName);
            btnAddSave.Image = Resources.button_add_24;

            // Disable button(s)
            btnDelete.Enabled = false;

            // Rename group box
            grpBoxAdd.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption", SettingsConfiguration.LanguageName);

            // Reset dialog icon to add
            Icon = Resources.add;

            // Deselect rows
            OnDeselectRowsOfDataGridViews(null);

            // Reset stored DataGridView instance
            SelectedDataGridView = null;

            // Select overview tab
            if (tabCtrlBrokerage.TabPages.Count > 0)
                tabCtrlBrokerage.SelectTab(0);

            // Reset document web browser
            Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);

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

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add", SettingsConfiguration.LanguageName))
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/AddFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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

                var strCaption = Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                    (int)EOwnMessageBoxInfoType.Info];
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/BrokerageDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", SettingsConfiguration.LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", SettingsConfiguration.LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel, EOwnMessageBoxInfoType.Info);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult != DialogResult.OK) return;

                DeleteBrokerageEventHandler?.Invoke(this, null);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DeleteFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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
                ResetValues();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/CancelFailure", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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
                // Set flag that the show brokerage is running
                ShowBrokerageFlag = true;

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
                           + @" ("
                           + ShareObjectFinalValue.AllBrokerageEntries.BrokerageValueTotalAsStr
                           + @" / "
                           + ShareObjectFinalValue.AllBrokerageEntries.ReductionValueTotalAsStr
                           + @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Check if brokerage exists
                if (ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues().Count <= 0) return;

                // Reverse list so the latest is a top of the data grid view
                var reversDataSourceOverview = ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageTotalValues()
                    .OrderByDescending(x => x.BrokerageValueYear).ToList();

                var bindingSourceOverview = new BindingSource
                {
                    DataSource = reversDataSourceOverview
                };

                // Create DataGridView
                var dataGridViewBrokerageOverviewOfAYears = new DataGridView
                {
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with brokerage values to the DataGridView
                    DataSource = bindingSourceOverview,

                    // Disable column header resize
                    ColumnHeadersHeightSizeMode =
                        DataGridViewColumnHeadersHeightSizeMode.DisableResizing
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
                    DataGridViewHelper.DataGridViewHeaderColors;
                dataGridViewBrokerageOverviewOfAYears.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                    DataGridViewHelper.DataGridViewHeaderColors;
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
                        Text = keyName 
                               + @" ("
                               +  ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName]
                                   .BrokerageValueYearAsStrUnit
                               + @" / "
                               + ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName]
                                   .ReductionValueYearAsStrUnit
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Reverse list so the latest is a top of the data grid view
                    var reversDataSource =
                        ShareObjectFinalValue.AllBrokerageEntries.AllBrokerageReductionOfTheShareDictionary[keyName]
                            .BrokerageReductionListYear.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();

                    // Create Binding source for the brokerage data
                    var bindingSource = new BindingSource
                    {
                        DataSource = reversDataSource
                    };

                    // Create DataGridView
                    var dataGridViewBrokerageOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with brokerage values to the DataGridView
                        DataSource = bindingSource,

                        // Disable column header resize
                        ColumnHeadersHeightSizeMode =
                            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewBrokerageOfAYear.DataBindingComplete += OnDataGridViewBrokerage_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewBrokerageOfAYear.MouseEnter += OnDataGridViewBrokerageOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewBrokerageOfAYear.SelectionChanged += OnDataGridViewBrokerageOfAYear_SelectionChanged;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView brokerage
                    dataGridViewBrokerageOfAYear.EnableHeadersVisualStyles = false;
                    // Column header styling
                    dataGridViewBrokerageOfAYear.ColumnHeadersDefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewBrokerageOfAYear.ColumnHeadersDefaultCellStyle.BackColor =
                        DataGridViewHelper.DataGridViewHeaderColors;
                    dataGridViewBrokerageOfAYear.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                        DataGridViewHelper.DataGridViewHeaderColors;
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

                tabCtrlBrokerage.TabPages[0].Select();

                // Set flag that the show brokerage is finished
                ShowBrokerageFlag = false;

            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/ShowFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName, Color.DarkRed, Logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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

                    // Disable sorting of the columns ( remove sort arrow )
                    ((DataGridView)sender).Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

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
                        } break;
                        case 1:
                        {
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Brokerage",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Date",
                                        LanguageName);
                            }
                        } break;
                        case 2:
                        {
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Reduction",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Brokerage",
                                        LanguageName);
                            }
                        } break;
                        case 3:
                        {
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_BrokerageReduction",
                                        LanguageName);
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Reduction",
                                        LanguageName);

                            }
                        } break;
                        case 4:
                        {
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_BrokerageReduction",
                                    LanguageName);
                        } break;
                        case 5:
                        {
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBrokerage/GrpBoxBrokerage/TabCtrl/DgvBrokerageOverview/ColHeader_Document",
                                    LanguageName);
                        } break;
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
                ResetValues();
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/RenameColHeaderFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/DeselectFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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
                        ResetValues();
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
                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/SelectionChangeFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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

                // If it is "1" a selection change has been made
                // else an deselection has been made ( switch to the overview tab )
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    var curItem = ((DataGridView)sender).SelectedRows;

                    // Set selected date
                    if (curItem[0].Cells[1].Value != null)
                        SelectedDate = curItem[0].Cells[1].Value.ToString();
                    else
                        return;

                    // Set selected Guid
                    if (curItem[0].Cells[1].Value != null)
                        SelectedGuid = curItem[0].Cells[0].Value.ToString();
                    else
                        return;

                    // Get selected brokerage object by Guid
                    var selectedBrokerageObject = ShareObjectFinalValue.AllBrokerageEntries.GetAllBrokerageOfTheShare()
                        .Find(x => x.Guid == SelectedGuid);

                    // Set brokerage values
                    if (selectedBrokerageObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(selectedBrokerageObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(selectedBrokerageObject.Date);
                        chkBoxBuyPart.CheckState = PartOfABuy ? CheckState.Checked : CheckState.Unchecked;
                        chkBoxSalePart.CheckState = PartOfASale ? CheckState.Checked : CheckState.Unchecked;
                        txtBoxProvision.Text = selectedBrokerageObject.ProvisionValueAsStr;
                        txtBoxBrokerFee.Text = selectedBrokerageObject.BrokerFeeValueAsStr;
                        txtBoxTraderPlaceFee.Text = selectedBrokerageObject.TraderPlaceFeeValueAsStr;
                        txtBoxBrokerage.Text = selectedBrokerageObject.BrokerageValueAsStr;
                        txtBoxReduction.Text = selectedBrokerageObject.ReductionValueAsStr;
                        txtBoxBrokerageReduction.Text = selectedBrokerageObject.BrokerageReductionValueAsStr;
                        txtBoxDocument.Text = selectedBrokerageObject.DocumentAsStr;

                        // Set brokerage values
                        if (selectedBrokerageObject.PartOfABuy || selectedBrokerageObject.PartOfASale)
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

                        // Rename button
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Save",
                                LanguageName);
                        btnAddSave.Image = Resources.button_pencil_24;

                        // Rename group box
                        grpBoxAdd.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Edit_Caption",
                                LanguageName);

                        // Reset dialog icon to add
                        Icon = Resources.edit;

                        // Store DataGridView instance
                        SelectedDataGridView = (DataGridView)sender;

                        // Format the input value
                        FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
                    }
                    else
                    {
                        // Reset and disable date time picker
                        datePickerDate.Value = DateTime.Now;
                        datePickerDate.Enabled = false;
                        datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        datePickerTime.Enabled = false;

                        // Reset check boxes
                        chkBoxBuyPart.CheckState = CheckState.Unchecked;
                        chkBoxBuyPart.Enabled = false;
                        chkBoxSalePart.CheckState = CheckState.Unchecked;
                        chkBoxSalePart.Enabled = false;

                        // Reset and disable text boxes
                        txtBoxProvision.Text = string.Empty;
                        txtBoxProvision.Enabled = false;
                        txtBoxBrokerFee.Text = string.Empty;
                        txtBoxBrokerFee.Enabled = false;
                        txtBoxTraderPlaceFee.Text = string.Empty;
                        txtBoxTraderPlaceFee.Enabled = false;
                        txtBoxBrokerage.Text = string.Empty;
                        txtBoxReduction.Text = string.Empty;
                        txtBoxReduction.Enabled = false;
                        txtBoxBrokerageReduction.Text = string.Empty;
                        txtBoxDocument.Text = string.Empty;
                        txtBoxDocument.Enabled = false;

                        // Reset status label message text
                        toolStripStatusLabelMessageBrokerageEdit.Text = string.Empty;

                        // Disable Button(s)
                        btnDocumentBrowse.Enabled = false;
                        btnAddSave.Enabled = false;
                        btnDelete.Enabled = false;

                        Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                            Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/SelectionChangeFailed", SettingsConfiguration.LanguageName),
                            Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                            (int)FrmMain.EComponentLevels.Application);
                    }
                }
                else
                {
                    // Rename button
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Buttons/Add",
                            LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/GrpBoxAddEdit/Add_Caption",
                        LanguageName);

                    // Reset dialog icon to add
                    Icon = Resources.add;

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Enable Button(s)
                    btnAddSave.Enabled = true;
                    btnDocumentBrowse.Enabled = true;

                    // Enable date time picker
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;

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


                // Check if the file still exists or no document is set
                if (File.Exists(txtBoxDocument.Text) || txtBoxDocument.Text == @"")
                {
                    Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);
                }
                else
                {
                    if (ShowBrokerageFlag) return;

                    var strCaption =
                        Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", SettingsConfiguration.LanguageName)[
                            (int)EOwnMessageBoxInfoType.Error];
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
                        strCancel, EOwnMessageBoxInfoType.Error);

                    // Check if the user pressed cancel
                    if (messageBox.ShowDialog() == DialogResult.Cancel) return;

                    // Get the current selected row
                    var curItem = ((DataGridView)sender).SelectedRows;
                    // Get Guid of the selected buy item
                    var strGuid = curItem[0].Cells[0].Value.ToString();

                    // Check if a document is set
                    if (curItem[0].Cells[((DataGridView)sender).ColumnCount - 1].Value == null) return;

                    // Get doc from the buy with the Guid
                    foreach (var temp in ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare())
                    {
                        // Check if the buy Guid is the same as the Guid of the clicked buy item
                        if (temp.Guid != strGuid) continue;

                        // Remove move document from the buy objects
                        if (ShareObjectFinalValue.SetBuyDocument(strGuid, temp.Date, string.Empty) &&
                            ShareObjectMarketValue.SetBuyDocument(strGuid, temp.Date, string.Empty))
                        {
                            // Set flag to save the share object.
                            SaveFlag = true;

                            OnShowBrokerage();

                            Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBrokerage/StateMessages/EditSuccess", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                (int)FrmMain.EComponentLevels.Application);
                        }
                        else
                        {
                            Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBrokerage/Errors/EditFailed", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error,
                                (int)FrmMain.EComponentLevels.Application);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tabCtrlBrokerage.SelectedIndex = 0;

                Helper.AddStatusMessage(toolStripStatusLabelMessageBrokerageEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormBrokerage/Errors/SelectionChangeFailed", SettingsConfiguration.LanguageName),
                   Language, SettingsConfiguration.LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                   ex);
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

        #endregion Brokerage of a year

        #endregion Data grid view

        #endregion Methods
    }
}
