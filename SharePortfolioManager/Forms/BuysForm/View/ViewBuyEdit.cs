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
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.BuysForm.View
{
    // Error codes of the BuyEdit
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
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        SharePriceEmpty,
        SharePriceWrongFormat,
        SharePriceWrongValue,
        BrokerageWrongFormat,
        BrokerageWrongValue,
        ReductionWrongFormat,
        ReductionWrongValue,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExits,
        DocumentFileDoesNotExists
    };

    /// <inheritdoc />
    /// <summary>
    /// Interface of the BuyEdit view
    /// </summary>
    public interface IViewBuyEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler AddBuyEventHandler;
        event EventHandler EditBuyEventHandler;
        event EventHandler DeleteBuyEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        BuyErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool UpdateBuy { get; set; }

        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        string VolumeSold { get; set; }
        string Price { get; set; }
        string MarketValue { get; set; }
        string Reduction { get; set; }
        string Brokerage { get; set; }
        string FinalValue { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
        void DocumentBrowseFinish();
    }

    public partial class ViewBuyEdit : Form, IViewBuyEdit
    {
        #region Fields

        /// <summary>
        /// Stores the Guid of a selected buy row
        /// </summary>
        private string _selectedGuid;

        /// <summary>
        /// Stores the date of a selected buy row
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

        public bool UpdateBuyFlag { get; set; }

        public bool SaveFlag { get; set; }

        #endregion Flags

        public DataGridView SelectedDataGridView { get; internal set; }

        #endregion Properties

        #region IView members

        public bool UpdateBuy
        {
            get => UpdateBuyFlag;
            set
            {
                UpdateBuyFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateBuy"));
            }
        }

        public BuyErrorCode ErrorCode { get; set; }

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

        public string Volume
        {
            get => txtBoxVolume.Text;
            set
            {
                if (txtBoxVolume.Text == value)
                    return;
                txtBoxVolume.Text = value;
            }
        }

        public string VolumeSold
        {
            get => txtBoxVolumeSold.Text;
            set
            {
                if (txtBoxVolumeSold.Text == value)
                    return;
                txtBoxVolumeSold.Text = value;
            }
        }

        public string Price
        {
            get => txtBoxPrice.Text;
            set
            {
                if (txtBoxPrice.Text == value)
                    return;
                txtBoxPrice.Text = value;
            }
        }

        public string MarketValue
        {
            get => txtBoxMarketValue.Text;
            set
            {
                if (txtBoxMarketValue.Text == value)
                    return;
                txtBoxMarketValue.Text = value;
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

        public string FinalValue
        {
            get => txtBoxFinalValue.Text;
            set
            {
                if (txtBoxFinalValue.Text == value)
                    return;
                txtBoxFinalValue.Text = value;
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
                case BuyErrorCode.AddSuccessful:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/AddSuccess", LanguageName);
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Refresh the buy list
                    OnShowBuys();

                    break;
                }
                case BuyErrorCode.AddFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
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
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption",
                            LanguageName);

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/EditSuccess", LanguageName);
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Refresh the buy list
                    OnShowBuys();

                    break;
                }
                case BuyErrorCode.EditFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/EditFailed", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.DeleteSuccessful:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/DeleteSuccess", LanguageName);
                    // Set flag to save the share object.
                    SaveFlag = true;

                    // Enable button(s)
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Refresh the buy list
                    OnShowBuys();

                    break;
                }
                case BuyErrorCode.DeleteFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.DeleteFailedUnerasable:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailedUnErasable", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.InputValuesInvalid:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CheckInputFailure", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.VolumeEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeEmpty", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.VolumeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.VolumeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/VolumeWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxVolume.Focus();

                    break;
                }
                case BuyErrorCode.SharePriceEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceEmpty", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxPrice.Focus();

                    break;
                }
                case BuyErrorCode.SharePriceWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxPrice.Focus();

                    break;
                }
                case BuyErrorCode.SharePriceWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SharePriceWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxPrice.Focus();

                    break;
                }
                case BuyErrorCode.BrokerageWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/BrokerageWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case BuyErrorCode.BrokerageWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/BrokerageWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxBrokerage.Focus();

                    break;
                }
                case BuyErrorCode.ReductionWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongFormat", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxReduction.Focus();

                    break;
                }
                case BuyErrorCode.ReductionWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ReductionWrongValue", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
                    txtBoxReduction.Focus();

                    break;
                }
                case BuyErrorCode.DocumentDirectoryDoesNotExits:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DirectoryDoesNotExist", LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;

                    Enabled = true;
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
                (int) stateLevel,
                (int) FrmMain.EComponentLevels.Application);
        }

        public void DocumentBrowseFinish()
        {
            // Set messages
            var strMessage = @"";
            var clrMessage = Color.Black;
            const FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case BuyErrorCode.DocumentBrowseFailed:
                {
                    txtBoxDocument.Text = @"-";

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ChoseDocumentFailed",
                            LanguageName);
                    break;
                }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                strMessage,
                Language,
                LanguageName,
                clrMessage,
                Logger,
                (int) stateLevel,
                (int) FrmMain.EComponentLevels.Application);
        }

        #endregion IView members

        #region Event members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValuesEventHandler;
        public event EventHandler AddBuyEventHandler;
        public event EventHandler EditBuyEventHandler;
        public event EventHandler DeleteBuyEventHandler;
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
        public ViewBuyEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue,
            Logger logger, Language language, string languageName)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;

            Logger = logger;
            Language = language;
            LanguageName = languageName;

            _focusedControl = txtBoxVolume;

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
                lblVolumeSold.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/VolumeSold",
                        LanguageName);
                lblFinalValue.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/FinalValue",
                        LanguageName);
                lblBrokerage.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Labels/Brokerage",
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
                lblAddVolumeSoldUnit.Text = ShareObject.PieceUnit;
                lblAddFinalValueUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddBrokerageUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddReductionUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddMarketValueUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblAddPriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.black_save;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                #endregion Image configuration

                // Load buys of the share
                OnShowBuys();

                txtBoxVolume.Focus();
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"ShareBuysEdit_Load()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", LanguageName),
                    Language, LanguageName, Color.DarkRed, Logger,
                    (int) FrmMain.EStateLevels.FatalError, (int) FrmMain.EComponentLevels.Application);
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
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date.
        /// </summary>
        private void ResetInputValues()
        {
            // Reset and enable date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerDate.Enabled = true;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            datePickerTime.Enabled = true;

            // Reset and enable text boxes
            txtBoxVolume.Text = @"";
            txtBoxVolume.Enabled = true;
            txtBoxVolumeSold.Text = @"";
            txtBoxPrice.Text = @"";
            txtBoxPrice.Enabled = true;
            txtBoxBrokerage.Text = @"";
            txtBoxBrokerage.Enabled = true;
            txtBoxReduction.Text = @"";
            txtBoxReduction.Enabled = true;
            txtBoxDocument.Text = @"";

            toolStripStatusLabelMessageBuyEdit.Text = @"";

            // Enable button(s)
            btnAddSave.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
            btnAddSave.Image = Resources.black_add;

            // Disable button(s)
            btnDelete.Enabled = false;

            // Rename group box
            grpBoxAdd.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

            // Deselect rows
            OnDeselectRowsOfDataGridViews(null);

            // Reset stored DataGridView instance
            SelectedDataGridView = null;

            // Select overview tab
            if (tabCtrlBuys.TabPages.Count > 0)
                tabCtrlBuys.SelectTab(0);

            txtBoxVolume.Focus();

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
        private void OnTxtBoxAddVolume_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Volume"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxVolume_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxVolume_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxVolume;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddVolumeSold_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VolumeSold"));
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxVolumeSold_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SharePrice"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxPrice;
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerage_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Brokerage"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerage_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBrokerage_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxBrokerage;
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
        /// This function shows the Drop sign
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// This function allows to sets via Drag and Drop a document for this brokerage
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
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
            Enabled = false;

            if (btnAddSave.Text ==
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName))
            {
                UpdateBuy = false;

                AddBuyEventHandler?.Invoke(this, null);
            }
            else
            {
                UpdateBuy = true;

                EditBuyEventHandler?.Invoke(this, null);
            }
        }
        catch (Exception ex)
        {
#if DEBUG_BUY || DEBUG
            var message = $"btnAdd_Click()\n\n{ex.Message}";
            MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
#endif
            // Add status message
            Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", LanguageName),
                Language, LanguageName,
                Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                (int) FrmMain.EComponentLevels.Application);
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
                Enabled = false;

                toolStripStatusLabelMessageBuyEdit.Text = @"";

                var strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/BuyDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult != DialogResult.OK) return;

                // Set flag to save the share object.
                SaveFlag = true;

                // Check if a row is selected
                if (SelectedDataGridView != null && SelectedDataGridView.SelectedRows.Count == 1)
                {
                    DeleteBuyEventHandler?.Invoke(this, null);
                }

                // Reset values
                ResetInputValues();

                // Enable button(s)
                btnAddSave.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.black_add;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Refresh the dividend list
                OnShowBuys();

            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"btnDelete_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeleteFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);
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
                // Reset values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"btnReset_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/CancelFailure", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);
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
        /// can chose a file which documents the buy
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnBuyDocumentBrowse_Click(object sender, EventArgs e)
        {
            DocumentBrowseEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Buttons

        #region Group box overview

        /// <summary>
        /// This function sets the focus back to the last focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnGrpBoxOverview_MouseLeave(object sender, EventArgs e)
        {
            _focusedControl?.Focus();
        }

        #endregion Group box overview

        #region Data grid view

        /// <summary>
        /// This function paints the buy list of the share
        /// </summary>
        private void OnShowBuys()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlBuys.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBuysOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBuysOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewBuysOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewBuysOfAYear_MouseEnter;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewBuys_DataBindingComplete;
                    }

                    tabPage.Controls.Clear();
                    tabCtrlBuys.TabPages.Remove(tabPage);
                }

                tabCtrlBuys.TabPages.Clear();

                // Enable controls
                Enabled = true;

                #region Add page

                // Create TabPage for the buys of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/TabPgOverview/Overview", LanguageName)
                           + @" (" + ShareObjectFinalValue.AllBuyEntries.BuyValueTotalAsStrUnit +
                           @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Create Binding source for the buy data
                var bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllBuyEntries.GetAllBuysTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllBuyEntries.GetAllBuysTotalValues();

                // Create DataGridView
                var dataGridViewBuysOverviewOfAYears = new DataGridView
                {
                    // TODO correct
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with buy values to the DataGridView
                    DataSource = bindingSourceOverview
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewBuysOverviewOfAYears.DataBindingComplete += OnDataGridViewBuys_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewBuysOverviewOfAYears.MouseEnter += OnDataGridViewBuysOfYears_MouseEnter;
                // Set row select event
                dataGridViewBuysOverviewOfAYears.SelectionChanged += OnDataGridViewBuysOfYears_SelectionChanged;

                #endregion Events

                #region Style 

                // Advanced configuration DataGridView buys
                dataGridViewBuysOverviewOfAYears.EnableHeadersVisualStyles = false;
                // Column header styling
                dataGridViewBuysOverviewOfAYears.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewBuysOverviewOfAYears.ColumnHeadersDefaultCellStyle.BackColor =
                    SystemColors.ControlLight;
                dataGridViewBuysOverviewOfAYears.ColumnHeadersHeight = 25;
                dataGridViewBuysOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                // Column styling
                dataGridViewBuysOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // Row styling
                dataGridViewBuysOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewBuysOverviewOfAYears.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewBuysOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewBuysOverviewOfAYears.MultiSelect = false;
                dataGridViewBuysOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // Cell styling
                dataGridViewBuysOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewBuysOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                dataGridViewBuysOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewBuysOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // Allow styling
                dataGridViewBuysOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewBuysOverviewOfAYears.AllowUserToResizeRows = false;
                dataGridViewBuysOverviewOfAYears.AllowUserToAddRows = false;
                dataGridViewBuysOverviewOfAYears.AllowUserToDeleteRows = false;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewBuysOverviewOfAYears);
                dataGridViewBuysOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlBuys.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlBuys;

                #endregion Control add

                // Check if buys exists
                if (ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Count <= 0) return;

                // Loop through the years of the buys
                foreach (
                    var keyName in ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Keys.Reverse()
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
                               ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary[keyName]
                                   .BuyValueYearAsStrUnit
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Create Binding source for the buy data
                    var bindingSource = new BindingSource
                    {
                        DataSource =
                            ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary[keyName]
                                .BuyListYear
                    };

                    // Create DataGridView
                    var dataGridViewBuysOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with buy values to the DataGridView
                        DataSource = bindingSource
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewBuysOfAYear.DataBindingComplete += OnDataGridViewBuys_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewBuysOfAYear.MouseEnter += OnDataGridViewBuysOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewBuysOfAYear.SelectionChanged += OnDataGridViewBuysOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewBuysOfAYear.CellContentDoubleClick += OnDataGridViewBuysOfAYear_CellContentClick;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView buys
                    dataGridViewBuysOfAYear.EnableHeadersVisualStyles = false;
                    // Column header styling
                    dataGridViewBuysOfAYear.ColumnHeadersDefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewBuysOfAYear.ColumnHeadersDefaultCellStyle.BackColor =
                        SystemColors.ControlLight;
                    dataGridViewBuysOfAYear.ColumnHeadersHeight = 25;
                    dataGridViewBuysOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    // Column styling
                    dataGridViewBuysOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Row styling
                    dataGridViewBuysOfAYear.RowHeadersVisible = false;
                    dataGridViewBuysOfAYear.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewBuysOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridViewBuysOfAYear.MultiSelect = false;
                    dataGridViewBuysOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    // Cell styling
                    dataGridViewBuysOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dataGridViewBuysOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                    dataGridViewBuysOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    dataGridViewBuysOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    // Allow styling
                    dataGridViewBuysOfAYear.AllowUserToResizeColumns = false;
                    dataGridViewBuysOfAYear.AllowUserToResizeRows = false;
                    dataGridViewBuysOfAYear.AllowUserToAddRows = false;
                    dataGridViewBuysOfAYear.AllowUserToDeleteRows = false;

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewBuysOfAYear);
                    dataGridViewBuysOfAYear.Parent = newTabPage;
                    tabCtrlBuys.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlBuys;

                    #endregion Control add
                }

                tabCtrlBuys.TabPages[0].Select();
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"ShowBuys()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/ShowFailed", LanguageName),
                    Language, LanguageName, Color.DarkRed, Logger,
                    (int) FrmMain.EStateLevels.FatalError, (int) FrmMain.EComponentLevels.Application);
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
                for (var i = 0; i < ((DataGridView) sender).ColumnCount; i++)
                {
                    // Set alignment of the column
                    ((DataGridView) sender).Columns[i].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    switch (i)
                    {
                        case 0:
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Year",
                                        LanguageName);
                            }

                            break;
                        case 1:
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Volume",
                                        LanguageName) + @" (" + ShareObject.PieceUnit + @")";
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Date",
                                        LanguageName);
                            }

                            break;
                        case 2:
                            if (((DataGridView) sender).Name == @"Overview")
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Price",
                                        LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            }
                            else
                            {
                                ((DataGridView) sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Volume",
                                        LanguageName) + @" (" + ShareObject.PieceUnit + @")";
                            }

                            break;
                        case 3:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Price",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 4:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Brokerage",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 5:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Reduction",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 6:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Document",
                                    LanguageName);
                            break;
                    }
                }

                if (((DataGridView) sender).Rows.Count > 0)
                {
                    ((DataGridView) sender).Rows[0].Selected = false;
                    ((DataGridView) sender).ScrollBars = ScrollBars.Both;
                }

                if (((DataGridView) sender).Name != @"Overview")
                    ((DataGridView) sender).Columns[0].Visible = false;

                // Reset the text box values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"dataGridViewBuysOfAYear_DataBindingComplete()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/RenameColHeaderFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);
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
                foreach (TabPage tabPage in tabCtrlBuys.TabPages)
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
#if DEBUG_BUY || DEBUG
                var message = $"DeselectRowsOfDataGridViews()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DeselectFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);
            }
        }

        #region Tab control delegates

        /// <summary>
        /// This function sets the focus on the data grid view of the new selected tab page of the tab control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlBuys_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabCtrlBuys.SelectedTab == null) return;

                // Loop trough the controls of the selected tab page and set focus on the data grid view
                foreach (Control control in tabCtrlBuys.SelectedTab.Controls)
                {
                    if (control is DataGridView view)
                    {
                        if (view.Rows.Count > 0 && view.Name != @"Overview")
                        {
                            if (view.Rows[0].Cells.Count > 0)
                                view.Rows[0].Selected = true;
                        }

                        view.Focus();

                        if (view.Name == @"Overview")
                            ResetInputValues();
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"TabCtrlBuys_SelectedIndexChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus back to the last focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlBuys_MouseLeave(object sender, EventArgs e)
        {
            _focusedControl?.Focus();
        }

        /// <summary>
        /// This function sets the focus on the last focused control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlBuys_KeyDown(object sender, KeyEventArgs e)
        {
            _focusedControl?.Focus();
        }

        /// <summary>
        /// This function sets key value ( char ) to the last focused control
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void OnTabCtrlBuys_KeyPress(object sender, KeyPressEventArgs e)
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
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView) sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlBuys.TabPages)
                {
                    if (curItem[0].Cells[1].Value != null &&
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()
                    ) continue;

                    tabCtrlBuys.SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"OnDataGridViewBuysOfYears_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewBuysOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
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
                    if (tabCtrlBuys.SelectedTab.Controls.Contains((DataGridView) sender))
                        OnDeselectRowsOfDataGridViews((DataGridView) sender);
                }

                if (((DataGridView) sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    var curItem = ((DataGridView) sender).SelectedRows;

                    // Set selected date
                    if (curItem[0].Cells[1].Value != null)
                        SelectedDate = curItem[0].Cells[1].Value.ToString();
                    else
                        return;

                    // Get list of buys of a year
                    DateTime.TryParse(SelectedDate, out var dateTime);
                    var buyListYear = ShareObjectFinalValue.AllBuyEntries
                        .AllBuysOfTheShareDictionary[dateTime.Year.ToString()]
                        .BuyListYear;

                    var index = ((DataGridView) sender).SelectedRows[0].Index;

                    // Set selected Guid
                    SelectedGuid = buyListYear[index].Guid;

                    // Get BrokerageObject of the selected DataGridView row
                    var selectedBuyObject = buyListYear[index];

                    // Set buy values
                    if (selectedBuyObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(selectedBuyObject.Date);
                        txtBoxVolume.Text = selectedBuyObject.VolumeAsStr;
                        txtBoxVolumeSold.Text = selectedBuyObject.VolumeSoldAsStr;
                        txtBoxFinalValue.Text = selectedBuyObject.MarketValueReductionBrokerageAsStr;
                        txtBoxBrokerage.Text = selectedBuyObject.BrokerageAsStr;
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

                    if (ShareObjectFinalValue.AllBuyEntries.IsLastBuy(SelectedGuid) &&
                        !ShareObjectFinalValue.AllBuyEntries.IsPartOfASale(SelectedGuid)
                    )
                    {
                        btnDelete.Enabled = ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare().Count > 1;

                        // Enable TextBox(es)
                        datePickerDate.Enabled = true;
                        datePickerTime.Enabled = true;
                        txtBoxVolume.Enabled = true;
                        txtBoxVolumeSold.Enabled = true;
                        txtBoxPrice.Enabled = true;
                        txtBoxMarketValue.Enabled = true;
                        txtBoxBrokerage.Enabled = true;
                        txtBoxReduction.Enabled = true;
                        txtBoxFinalValue.Enabled = true;
                    }
                    else
                    {
                        // Disable Button(s)
                        btnDelete.Enabled = false;
                        // Disable TextBox(es)
                        datePickerDate.Enabled = false;
                        datePickerTime.Enabled = false;
                        txtBoxVolume.Enabled = false;
                        txtBoxVolumeSold.Enabled = false;
                        txtBoxPrice.Enabled = false;
                        txtBoxMarketValue.Enabled = false;
                        txtBoxBrokerage.Enabled = false;
                        txtBoxReduction.Enabled = false;
                        txtBoxFinalValue.Enabled = false;
                    }

                    // Rename button
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView) sender;

                    // Format the input value
                    FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
                }
                else
                {
                    // Rename button
                    btnAddSave.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;

                    // Rename group box
                    grpBoxAdd.Text =
                        Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // Enable Button(s)
                    btnAddSave.Enabled = true;

                    // Enabled TextBox(es)
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxVolumeSold.Enabled = true;
                    txtBoxPrice.Enabled = true;
                    txtBoxMarketValue.Enabled = true;
                    txtBoxBrokerage.Enabled = true;
                    txtBoxReduction.Enabled = true;
                    txtBoxFinalValue.Enabled = true;

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"OnDataGridViewBuysOfAYear_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);

                OnShowBuys();
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewBuysOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView) sender).Focus();
        }

        /// <summary>
        /// This function opens the buy document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewBuysOfAYear_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get column count of the given DataGridView
                var iColumnCount = ((DataGridView) sender).ColumnCount;

                // Check if the last column (document column) has been clicked
                if (e.ColumnIndex != iColumnCount - 1) return;

                // Check if a row is selected
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the current selected row
                var curItem = ((DataGridView) sender).SelectedRows;
                // Get Guid of the selected buy item
                var strGuid = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value == null) return;

                // Get doc from the buy with the Guid
                foreach (var temp in ShareObjectFinalValue.AllBuyEntries.GetAllBuysOfTheShare())
                {
                    // Check if the buy Guid is the same as the Guid of the clicked buy item
                    if (temp.Guid != strGuid) continue;

                    // Check if the file still exists
                    if (File.Exists(temp.Document))
                        // Open the file
                        Process.Start(temp.Document);
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
                            // Remove buy object and add it with no document
                            if (ShareObjectFinalValue.RemoveBuy(temp.Guid, temp.Date) &&
                                ShareObjectFinalValue.AddBuy(strGuid, temp.Date, temp.Volume, temp.VolumeSold,
                                    temp.Reduction, temp.Brokerage, temp.BuyValue))
                            {
                                // Set flag to save the share object.
                                SaveFlag = true;

                                OnShowBuys();

                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/StateMessages/EditSuccess", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int) FrmMain.EStateLevels.Info,
                                    (int) FrmMain.EComponentLevels.Application);
                            }
                            else
                            {
                                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/Errors/EditFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int) FrmMain.EStateLevels.Error,
                                    (int) FrmMain.EComponentLevels.Application);
                            }
                        }
                    }

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_BUY || DEBUG
                var message = $"OnDataGridViewBuysOfAYear_CellContentClick()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageBuyEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Buys of a year

        #endregion Data grid view

        #endregion Methods
    }
}