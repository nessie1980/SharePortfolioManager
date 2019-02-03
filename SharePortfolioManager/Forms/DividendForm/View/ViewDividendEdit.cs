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
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SharePortfolioManager.Forms.DividendForm.View
{
    // Error codes of the DividendEdit
    public enum DividendErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        InputValuesInvalid,
        ExchangeRatioEmpty,
        ExchangeRatioWrongFormat,
        ExchangeRatioWrongValue,
        RateEmpty,
        RateWrongFormat,
        RateWrongValue,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        VolumeMaxValue,
        TaxAtSourceWrongFormat,
        TaxAtSourceWrongValue,
        CapitalGainsTaxWrongFormat,
        CapitalGainsTaxWrongValue,
        SolidarityTaxWrongFormat,
        SolidarityTaxWrongValue,
        PriceEmpty,
        PriceWrongFormat,
        PriceWrongValue,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExists,
        DocumentFileDoesNotExists
    }

    /// <inheritdoc />
    /// <summary>
    /// Interface of the DividendEdit view
    /// </summary>
    public interface IViewDividendEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler AddDividendEventHandler;
        event EventHandler EditDividendEventHandler;
        event EventHandler DeleteDividendEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        DividendErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool UpdateDividend { get; set; }
        string SelectedGuid { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        CheckState EnableFc { get; set; }
        string ExchangeRatio { get; set; }
        CultureInfo CultureInfoFc { get; }
        string Rate { get; set; }
        string Volume { get; set; }
        string Payout { get; set; }
        string PayoutFc { get; set; }
        string TaxAtSource { get; set; }
        string CapitalGainsTax { get; set; }
        string SolidarityTax { get; set; }
        string Tax { get; set; }
        string PayoutAfterTax { get; set; }
        string Yield { get; set; }
        string Price { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
        void DocumentBrowseFinish();
    }

    public partial class ViewDividendEdit : Form, IViewDividendEdit
    {
        #region Fields

        /// <summary>
        /// Stores the Guid of a selected dividend row
        /// </summary>
        private string _selectedGuid;

        /// <summary>
        /// Stores the date of a selected dividend row
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

        public bool UpdateDividendFlag { get; set; }

        public bool SaveFlag { get; internal set; }

        public bool LoadGridSelectionFlag { get; internal set; }

        #endregion Flags

        public DataGridView SelectedDataGridView { get; internal set; }

        #endregion Properties

        #region IViewMember

        public bool UpdateDividend
        {
            get => UpdateDividendFlag;
            set
            {
                UpdateDividendFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateDividend"));
            }
        }

        public DividendErrorCode ErrorCode { get; set; }

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

        public CheckState EnableFc
        {
            get => chkBoxEnableFC.CheckState;
            set
            {
                if (chkBoxEnableFC.CheckState == value)
                    return;
                chkBoxEnableFC.CheckState = value;
            }
        }

        public CultureInfo CultureInfoFc
        {
            get
            {
                if (cbxBoxDividendFCUnit.SelectedItem != null)
                {
                    return Helper.DictionaryListNameCultureInfoCurrencySymbol.TryGetValue(cbxBoxDividendFCUnit.SelectedItem.ToString().Split('/')[0].Trim(), out var ciTemp) ? ciTemp.CultureInfo : null;
                }

                return null;
            }
        }

        public string ExchangeRatio
        {
            get => txtBoxExchangeRatio.Text;
            set
            {
                if (txtBoxExchangeRatio.Text == value)
                    return;
                txtBoxExchangeRatio.Text = value;
            }
        }

        public string Rate
        {
            get => txtBoxRate.Text;
            set
            {
                if (txtBoxRate.Text == value)
                    return;
                txtBoxRate.Text = value;
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

        public string Payout
        {
            get => txtBoxPayout.Text;
            set
            {
                if (txtBoxPayout.Text == value)
                    return;
                txtBoxPayout.Text = value;
            }
        }

        public string PayoutFc
        {
            get => txtBoxPayoutFC.Text;
            set
            {
                if (txtBoxPayoutFC.Text == value)
                    return;
                txtBoxPayoutFC.Text = value;
            }
        }

        public string TaxAtSource
        {
            get => txtBoxTaxAtSource.Text;
            set
            {
                if (txtBoxTaxAtSource.Text == value)
                    return;
                txtBoxTaxAtSource.Text = value;
            }
        }

        public string CapitalGainsTax
        {
            get => txtBoxCapitalGainsTax.Text;
            set
            {
                if (txtBoxCapitalGainsTax.Text == value)
                    return;
                txtBoxCapitalGainsTax.Text = value;
            }
        }

        public string SolidarityTax
        {
            get => txtBoxSolidarityTax.Text;
            set
            {
                if (txtBoxSolidarityTax.Text == value)
                    return;
                txtBoxSolidarityTax.Text = value;
            }
        }

        public string Tax
        {
            get => txtBoxTax.Text;
            set
            {
                if (txtBoxTax.Text == value)
                    return;
                txtBoxTax.Text = value;
            }
        }

        public string PayoutAfterTax
        {
            get => txtBoxPayoutAfterTax.Text;
            set
            {
                if (txtBoxPayoutAfterTax.Text == value)
                    return;
                txtBoxPayoutAfterTax.Text = value;
            }
        }

        public string Yield
        {
            get => txtBoxYield.Text;
            set
            {
                if (txtBoxYield.Text == value)
                    return;
                txtBoxYield.Text = value;
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

        #endregion InputValues

        public void AddEditDeleteFinish()
        {
            // Set messages
            var strMessage = @"";
            var clrMessage = Color.Black;
            var stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case DividendErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the buy list
                        OnShowDividends();

                        break;
                    }
                case DividendErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add",
                                LanguageName);
                        btnAddSave.Image = Resources.button_add_24;
                        // Disable button(s)
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAddDividend.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/EditSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the buy list
                        OnShowDividends();

                        break;
                    }
                case DividendErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.button_add_24;

                        // Disable button(s)
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the buy list
                        OnShowDividends();

                        break;
                    }
                case DividendErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.ExchangeRatioEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ExchangeRatioEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxExchangeRatio.Focus();

                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ExchangeRatioWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxExchangeRatio.Focus();

                        break;
                    }
                case DividendErrorCode.ExchangeRatioWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ExchangeRatioWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxExchangeRatio.Focus();

                        break;
                    }
                case DividendErrorCode.RateEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RateEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxRate.Focus();

                        break;
                    }
                case DividendErrorCode.RateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxRate.Focus();

                        break;
                    }
                case DividendErrorCode.RateWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RateWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxRate.Focus();

                        break;
                    }
                case DividendErrorCode.TaxAtSourceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/TaxAtSourceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case DividendErrorCode.TaxAtSourceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/TaxAtSourceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case DividendErrorCode.CapitalGainsTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CapitalGainsTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case DividendErrorCode.CapitalGainsTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CapitalGainsTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case DividendErrorCode.SolidarityTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SolidarityTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case DividendErrorCode.SolidarityTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SolidarityTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case DividendErrorCode.PriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxPrice.Focus();

                        break;
                    }
                case DividendErrorCode.PriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxPrice.Focus();

                        break;
                    }
                case DividendErrorCode.PriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/PriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxPrice.Focus();

                        break;
                    }
                case DividendErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.VolumeMaxValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeMaxValue_1", LanguageName) +
                            ShareObjectFinalValue.Volume +
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/VolumeMaxValue_2", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case DividendErrorCode.DocumentDirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DirectoryDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDocument.Focus();

                        break;
                    }
                case DividendErrorCode.DocumentFileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/FileDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessage,
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
                case DividendErrorCode.DocumentBrowseFailed:
                    {
                        txtBoxDocument.Text = @"-";

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ChoseDocumentFailed", LanguageName);
                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessage,
               strMessage,
               Language,
               LanguageName,
               clrMessage,
               Logger,
               (int)stateLevel,
               (int)FrmMain.EComponentLevels.Application);
        }

        #endregion IViewMember

        #region Event members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValuesEventHandler;
        public event EventHandler AddDividendEventHandler;
        public event EventHandler EditDividendEventHandler;
        public event EventHandler DeleteDividendEventHandler;
        public event EventHandler DocumentBrowseEventHandler;

        #endregion Event members

        #region Methods

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market value share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final value share object</param>
        /// <param name="logger">Logger</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public ViewDividendEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue,
            Logger logger, Language xmlLanguage, string language)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;
            Logger = logger;
            Language = xmlLanguage;
            LanguageName = language;
            SaveFlag = false;
        }

        /// <summary>
        /// This function sets the language to the labels and buttons
        /// and sets the units behind the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareDividendEdit_Load(object sender, EventArgs e)
        {
            try
            {
                Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Caption", LanguageName);
                grpBoxAddDividend.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxDividends.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxDividend/Caption",
                        LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                lblTime.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Time",
                    LanguageName);
                lblEnableForeignCurrency.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/EnableForeignCurrency",
                    LanguageName);
                lblDividendExchangeRatio.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/ExchangeRatio",
                    LanguageName);
                lblDividendRate.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/DividendRate",
                        LanguageName);
                lblVolume.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Volume",
                        LanguageName);
                lblPayout.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Payout",
                        LanguageName);
                lblTaxAtSource.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/TaxAtSource",
                        LanguageName);
                lblCapitalGainsTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/CapitalGainsTax",
                        LanguageName);
                lblSolidarityTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/SolidarityTax",
                        LanguageName);
                lblAddTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Tax",
                        LanguageName);
                lblAddYield.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Yield",
                        LanguageName);
                txtBoxYield.Text = @"-";
                lblAddPayoutAfterTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/PayoutAfterTax",
                        LanguageName);
                lblAddPrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Price",
                        LanguageName);
                lblAddDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add",
                    LanguageName);
                btnReset.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Reset",
                    LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                // Load button images
                btnAddSave.Image = Resources.button_add_24;
                btnDelete.Image = Resources.button_recycle_bin_24;
                btnReset.Image = Resources.button_reset_24;
                btnCancel.Image = Resources.button_back_24;

                // Set dividend units to the edit boxes
                lblDividendRateUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblPayoutUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxAtSourceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCapitalGainsTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSolidarityTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPayoutAfterTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblYieldUnit.Text = ShareObject.PercentageUnit;
                lblPriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                // Set currency to ComboBox
                foreach (var temp in Helper.ListNameCultureInfoCurrencySymbol)
                {
                    cbxBoxDividendFCUnit.Items.Add($"{temp.Key} / {temp.Value.CurrencySymbol}");
                }

                CheckForeignCurrencyCalculationShouldBeDone();

                // Chose USD item
                var iIndex = cbxBoxDividendFCUnit.FindString("en-US");
                cbxBoxDividendFCUnit.SelectedIndex = iIndex;

                OnShowDividends();
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND || DEBUG
                var message = $"ShareDividendEdit_Load()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function set the focus on the volume edit box
        /// when the form is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareDividendEdit_Shown(object sender, EventArgs e)
        {
            txtBoxRate.Focus();
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a dividend has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareDividendEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            chkBoxEnableFC.CheckState = CheckState.Unchecked;
            chkBoxEnableFC.Enabled = true;
            txtBoxExchangeRatio.Text = @"";
            txtBoxVolume.Text = @"";
            txtBoxVolume.Enabled = true;
            txtBoxRate.Text = @"";
            txtBoxRate.Enabled = true;
            txtBoxPayout.Text = @"";
            txtBoxPayoutFC.Text = @"";
            txtBoxTaxAtSource.Text = @"";
            txtBoxTaxAtSource.Enabled = true;
            txtBoxCapitalGainsTax.Text = @"";
            txtBoxCapitalGainsTax.Enabled = true;
            txtBoxSolidarityTax.Text = @"";
            txtBoxSolidarityTax.Enabled = true;
            txtBoxTax.Text = @"";
            txtBoxPayoutAfterTax.Text = @"";
            txtBoxYield.Text = @"";
            txtBoxPrice.Text = @"";
            txtBoxDocument.Text = @"";

            toolStripStatusLabelMessage.Text = @"";

            // Enable button(s)
            btnAddSave.Text = 
                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
            btnAddSave.Image = Resources.button_add_24;

            // Disable button(s)
            btnDelete.Enabled = false;

            // Rename group box
            grpBoxAddDividend.Text =
                Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

            // Deselect rows
            OnDeselectRowsOfDataGridViews(null);

            // Reset stored DataGridView instance
            SelectedDataGridView = null;

            // Check if any volume is present
            if (ShareObjectFinalValue.Volume > 0)
            {
                // Set current share value
                txtBoxVolume.Text = ShareObjectFinalValue.VolumeAsStr;
            }

            if (tabCtrlDividends.TabPages.Count > 0)
                tabCtrlDividends.SelectTab(0);

            txtBoxRate.Focus();

            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Form

        #region CheckBoxes

        /// <summary>
        /// This function checks the CheckBox enable foreign currency
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">EventArgs</param>
        private void OnChkBoxAddDividendForeignCurrency_CheckedChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnableFC"));

            CheckForeignCurrencyCalculationShouldBeDone();
        }

        /// <summary>
        /// This function stores the check box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnChkBoxEnableFC_Enter(object sender, EventArgs e)
        {
            _focusedControl = chkBoxEnableFC;
        }

        #endregion CheckBoxes

        #region Combo box foreign currency

        /// <summary>
        /// This function changes the currency unit when the currency
        /// has been changed and changes the currency unit to the taxes
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">EventArgs</param>
        private void CbxBoxAddDividendForeignCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPayoutFCUnit.Text = ((ComboBox)sender).SelectedItem.ToString().Split('/')[1].Trim();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CultureInfoFC"));
        }

        /// <summary>
        /// This function stores the combo box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnCbxBoxDividendFCUnit_Enter(object sender, EventArgs e)
        {
            _focusedControl = cbxBoxDividendFCUnit;
        }

        #endregion Combo box foreign currency

        #region Date / Time

        /// <summary>
        /// This function updates the model if the date has been changed
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
        /// This function updates the model if the time has been changed
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

        #endregion Date / Time

        #region TextBoxes

        /// <summary>
        /// This function updates the model if the exchange ratio has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void TxtBoxAddDividendExchangeRatio_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExchangeRatio"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxExchangeRatio_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxExchangeRatio_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxExchangeRatio;
        }

        /// <summary>
        /// This function update the model if the rate has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddDividendRate_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rate"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxRate_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxRate_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxRate;
        }

        /// <summary>
        /// This function updates the model if volume has been changed
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
        /// This function updates the model if tax at source value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTaxAtSource_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaxAtSource"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTaxAtSource_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxTaxAtSource_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxTaxAtSource;
        }

        /// <summary>
        /// This function updates the model if capital gains tax value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCapitalGainsTax_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapitalGainsTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCapitalGainsTax_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCapitalGainsTax_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxCapitalGainsTax;
        }

        /// <summary>
        /// This function updates the model if solidarity tax value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSolidarityTax_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SolidarityTax"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSolidarityTax_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function stores the text box to the focused control
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSolidarityTax_Enter(object sender, EventArgs e)
        {
            _focusedControl = txtBoxSolidarityTax;
        }

        /// <summary>
        /// This function updates the model if price value has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxPrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
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
        /// This function updates the model if document value has been changed
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
        /// This function adds a new dividend entry to the share object
        /// or edit a dividend entry
        /// It also checks if an entry already exists for the given date
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void BtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                // Disable controls
                Enabled = false;

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    UpdateDividend = false;

                    AddDividendEventHandler?.Invoke(this, null);
                }
                else
                {
                    UpdateDividend = true;

                    EditDividendEventHandler?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND || DEBUG
                var message = $"btnAdd_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/AddFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the dividend.
        /// If the user press "Ok" the dividend will be deleted and the
        /// list of the dividends will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">EventArgs</param>
        private void BtnDividendDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                var strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/DividendDelete",
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
                    DeleteDividendEventHandler?.Invoke(this, null);
                }

                // Reset values
                ResetInputValues();

                // Enable button(s)
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.button_add_24;

                // Rename group box
                grpBoxAddDividend.Text = Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Refresh the dividend list
                OnShowDividends();
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND || DEBUG
                var message = $"btnDividendDelete_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeleteFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a dividend
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset values
                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND || DEBUG
                var message = $"btnReset_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/CancelFailure", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function closes the dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the dividend pay of the share
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void BtnDividendDocumentBrowse_Click(object sender, EventArgs e)
        {
            DocumentBrowseEventHandler?.Invoke(this, new EventArgs());
        }

        #endregion Buttons

        #region Group box overview

        private void OnGrpBoxDividends_MouseLeave(object sender, EventArgs e)
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
        /// This function paints the dividend list of the share
        /// </summary>
        private void OnShowDividends()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= DataGridViewDividendsOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewDividendsOfYears_MouseEnter;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= DataGridViewDividendsOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewDividendsOfAYear_MouseEnter;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewDividends_DataBindingComplete;
                    }
                    tabPage.Controls.Clear();
                    tabCtrlDividends.TabPages.Remove(tabPage);
                }

                tabCtrlDividends.TabPages.Clear();

                // Enable controls
                Enabled = true;

                #region Add page

                // Create TabPage for the dividends of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/TabPgOverview/Overview", LanguageName)
                           + @" (" + ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesWithUnitAsStr +
                           @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Create Binding source for the dividend data
                var bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllDividendEntries.GetAllDividendsTotalValues();

                // Create DataGridView
                var dataGridViewDividendsOverviewOfAYears = new DataGridView
                {
                    // TODO correct
                    Name = @"Overview",
                    Dock = DockStyle.Fill,

                    // Bind source with dividend values to the DataGridView
                    DataSource = bindingSourceOverview
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewDividendsOverviewOfAYears.DataBindingComplete += OnDataGridViewDividends_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewDividendsOverviewOfAYears.MouseEnter += OnDataGridViewDividendsOfYears_MouseEnter;
                // Set row select event
                dataGridViewDividendsOverviewOfAYears.SelectionChanged += DataGridViewDividendsOfYears_SelectionChanged;

                #endregion Events

                #region Style

                // Advanced configuration DataGridView dividends
                dataGridViewDividendsOverviewOfAYears.EnableHeadersVisualStyles = false;
                // Column header styling
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersDefaultCellStyle.BackColor =
                    SystemColors.ControlLight;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersHeight = 25;
                dataGridViewDividendsOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                // Column styling
                dataGridViewDividendsOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // Row styling
                dataGridViewDividendsOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewDividendsOverviewOfAYears.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewDividendsOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewDividendsOverviewOfAYears.MultiSelect = false;
                dataGridViewDividendsOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                // Cell styling
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                dataGridViewDividendsOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewDividendsOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // Allow styling
                dataGridViewDividendsOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewDividendsOverviewOfAYears.AllowUserToResizeRows = false;
                dataGridViewDividendsOverviewOfAYears.AllowUserToAddRows = false;
                dataGridViewDividendsOverviewOfAYears.AllowUserToDeleteRows = false;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewDividendsOverviewOfAYears);
                dataGridViewDividendsOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlDividends.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlDividends;

                #endregion Control add

                // Check if dividend pays exists
                if (ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Count <= 0) return;

                // Loop through the years of the dividend pays
                foreach (
                    var keyName in ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary.Keys.Reverse()
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
                               ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                                   .DividendValueYearWithUnitAsStr
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Create Binding source for the dividend data
                    var bindingSource = new BindingSource
                    {
                        DataSource =
                            ShareObjectFinalValue.AllDividendEntries.AllDividendsOfTheShareDictionary[keyName]
                                .DividendListYear
                    };

                    // Create DataGridView
                    var dataGridViewDividendsOfAYear = new DataGridView
                    {
                        Name = keyName,
                        Dock = DockStyle.Fill,

                        // Bind source with dividend values to the DataGridView
                        DataSource = bindingSource
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewDividendsOfAYear.DataBindingComplete += OnDataGridViewDividends_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewDividendsOfAYear.MouseEnter += OnDataGridViewDividendsOfAYear_MouseEnter;
                    // Set row select event
                    dataGridViewDividendsOfAYear.SelectionChanged += DataGridViewDividendsOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewDividendsOfAYear.CellContentDoubleClick += DataGridViewDividendsOfAYear_CellContentClick;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView dividends
                    dataGridViewDividendsOfAYear.EnableHeadersVisualStyles = false;
                    // Column header styling
                    dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewDividendsOfAYear.ColumnHeadersDefaultCellStyle.BackColor =
                        SystemColors.ControlLight;
                    dataGridViewDividendsOfAYear.ColumnHeadersHeight = 25;
                    dataGridViewDividendsOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    // Column styling
                    dataGridViewDividendsOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Row styling
                    dataGridViewDividendsOfAYear.RowHeadersVisible = false;
                    dataGridViewDividendsOfAYear.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewDividendsOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridViewDividendsOfAYear.MultiSelect = false;
                    dataGridViewDividendsOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    // Cell styling
                    dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dataGridViewDividendsOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;
                    dataGridViewDividendsOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    dataGridViewDividendsOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    // Allow styling
                    dataGridViewDividendsOfAYear.AllowUserToResizeColumns = false;
                    dataGridViewDividendsOfAYear.AllowUserToResizeRows = false;
                    dataGridViewDividendsOfAYear.AllowUserToAddRows = false;
                    dataGridViewDividendsOfAYear.AllowUserToDeleteRows = false;

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewDividendsOfAYear);
                    dataGridViewDividendsOfAYear.Parent = newTabPage;
                    tabCtrlDividends.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlDividends;

                    #endregion Control add
                }

                tabCtrlDividends.TabPages[0].Select();

                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND || DEBUG
                var message = $"ShowDividends()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewDividends_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Date",
                                        LanguageName);
                            }
                            break;
                        case 1:
                            if (((DataGridView)sender).Name == @"Overview")
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Dividend",
                                        LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            }
                            else
                            {
                                ((DataGridView)sender).Columns[i].HeaderText =
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormBuy/GrpBoxBuy/TabCtrl/DgvBuyOverview/ColHeader_Date",
                                        LanguageName);
                            }
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Dividend",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Volume",
                                    LanguageName) + @" (" + ShareObject.PieceUnit + @")";
                            break;
                        case 4:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_TaxSum",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 5:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Yield",
                                    LanguageName) + @" (" + ShareObject.PercentageUnit + @")";
                            break;
                        case 6:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(
                                    @"/AddEditFormDividend/GrpBoxDividend/TabCtrl/DgvDividendOverview/ColHeader_Document",
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
#if DEBUG_DIVIDEND || DEBUG
                var message = $"dataGridViewDividendsOfAYear_DataBindingComplete()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/RenameColHeaderFailed", LanguageName),
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
                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
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
#if DEBUG_DIVIDEND || DEBUG
                var message = $"DeselectRowsOfDataGridViews()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DeselectFailed", LanguageName),
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
        private void OnTabCtrlDividends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlDividends.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabCtrlDividends.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                {
                    if (view.Rows.Count > 0 && view.Name != @"Overview")
                    {
                        if(view.Rows[0].Cells.Count > 0)
                            view.Rows[0].Selected = true;
                    }

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
        private void OnTabCtrlDividends_MouseLeave(object sender, EventArgs e)
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
        private void OnTabCtrlDividends_KeyDown(object sender, KeyEventArgs e)
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
        private void OnTabCtrlDividends_KeyPress(object sender, KeyPressEventArgs e)
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

        #region Dividends of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewDividendsOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlDividends.TabPages)
                {
                    if (curItem[0].Cells[1].Value != null && 
                        tabPage.Name != curItem[0].Cells[0].Value.ToString()
                        ) continue;

                    tabCtrlDividends.SelectTab(tabPage);
                    tabPage.Focus();

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND || DEBUG
                var message = $"dataGridViewDividendsOfYears_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewDividendsOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        #endregion Dividends of years

        #region Dividends of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewDividendsOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlDividends.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlDividends.SelectedTab.Controls.Contains((DataGridView) sender))
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
                    var dividendListYear = ShareObjectFinalValue.AllDividendEntries
                        .AllDividendsOfTheShareDictionary[dateTime.Year.ToString()]
                        .DividendListYear;

                    var index = ((DataGridView)sender).SelectedRows[0].Index;

                    // Set selected Guid
                    SelectedGuid = dividendListYear[index].Guid;

                    // Get BrokerageObject of the selected DataGridView row
                    var selectedDividendObject = dividendListYear[index];

                    // Set dividend values
                    if (selectedDividendObject != null)
                    {
                        if (selectedDividendObject.EnableFc == CheckState.Checked)
                        {
                            LoadGridSelectionFlag = true;

                            // Set CheckBox for the exchange ratio
                            chkBoxEnableFC.CheckState = CheckState.Checked;

                            // Set foreign currency values
                            txtBoxExchangeRatio.Text = selectedDividendObject.ExchangeRatio;

                            // Find correct currency
                            cbxBoxDividendFCUnit.SelectedIndex = cbxBoxDividendFCUnit.FindString(selectedDividendObject.CultureInfoFc.Name);

                            LoadGridSelectionFlag = false;
                        }
                        else
                        {
                            txtBoxRate.Text = selectedDividendObject.Rate;

                            // Chose USD item
                            var iIndex = cbxBoxDividendFCUnit.FindString("en-US");
                            cbxBoxDividendFCUnit.SelectedIndex = iIndex;
                        }

                        datePickerDate.Value = Convert.ToDateTime(selectedDividendObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(selectedDividendObject.Date);
                        txtBoxRate.Text = selectedDividendObject.Rate;
                        txtBoxVolume.Text = selectedDividendObject.Volume;
                        txtBoxTaxAtSource.Text = selectedDividendObject.TaxAtSource;
                        txtBoxCapitalGainsTax.Text = selectedDividendObject.CapitalGainsTax;
                        txtBoxSolidarityTax.Text = selectedDividendObject.SolidarityTax;
                        txtBoxYield.Text = selectedDividendObject.Yield;
                        txtBoxPrice.Text = selectedDividendObject.Price;
                        txtBoxDocument.Text = selectedDividendObject.Document;
                    }
                    else
                    {
                        datePickerDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        datePickerTime.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        txtBoxPayout.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxRate.Text = curItem[0].Cells[2].Value.ToString();
                        txtBoxYield.Text = curItem[0].Cells[3].Value.ToString();
                        txtBoxPrice.Text = curItem[0].Cells[4].Value.ToString();
                        txtBoxVolume.Text = curItem[0].Cells[5].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[6].Value.ToString();
                    }

                    // Rename button
                    btnAddSave.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.button_pencil_24;

                    // Rename group box
                    grpBoxAddDividend.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;

                    // Format the input value
                    FormatInputValuesEventHandler?.Invoke(this, new EventArgs());
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.button_add_24;

                    // Rename group box
                    grpBoxAddDividend.Text = 
                        Language.GetLanguageTextByXPath(@"/AddEditFormDividend/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Disable button(s)
                    btnDelete.Enabled = false;

                    // TODO correct
                    // Enabled TextBox(es)
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;
                    txtBoxRate.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxTaxAtSource.Enabled = true;
                    txtBoxCapitalGainsTax.Enabled = true;
                    txtBoxSolidarityTax.Enabled = true;
                    txtBoxPrice.Enabled = true;

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_DIVIDEND || DEBUG
                var message = $"dataGridViewDividendsOfAYear_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/SelectionChangeFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewDividendsOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function opens the dividend document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void DataGridViewDividendsOfAYear_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                var strGuid = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value == null) return;

                // Get doc from the dividend with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllDividendEntries.GetAllDividendsOfTheShare())
                {
                    // Check if the dividend date and time is the same as the date and time of the clicked buy item
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
                            // Remove dividend object and add it with no document
                            if (ShareObjectFinalValue.RemoveDividend(temp.Guid, temp.Date) &&
                                ShareObjectFinalValue.AddDividend(temp.CultureInfoFc, temp.EnableFc, temp.ExchangeRatioDec, temp.Guid, temp.Date, temp.RateDec, temp.VolumeDec,
                                    temp.TaxAtSourceDec, temp.CapitalGainsTaxDec, temp.SolidarityTaxDec, temp.PriceDec))
                            {
                                // Set flag to save the share object.
                                SaveFlag = true;

                                OnShowDividends();

                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/StateMessages/EditSuccess", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                            else
                            {
                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormDividend/Errors/EditFailed", LanguageName),
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
#if DEBUG_DIVIDEND || DEBUG
                var message = $"dataGridViewDividendsOfAYear_CellContentDecimalClick()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormDividend/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Dividends of a year

        #endregion Data grid view

        /// <summary>
        /// This function check is a foreign current calculation should be done.
        /// The function shows or hides, resize and reposition controls
        /// </summary>
        private void CheckForeignCurrencyCalculationShouldBeDone()
        {
            if (chkBoxEnableFC.CheckState == CheckState.Checked)
            {
                // Enable controls
                txtBoxExchangeRatio.Enabled = true;
                txtBoxExchangeRatio.ReadOnly = false;
                txtBoxTaxAtSource.Enabled = true;
                txtBoxTaxAtSource.ReadOnly = false;
                cbxBoxDividendFCUnit.Enabled = true;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxExchangeRatio.Text = @"";
                    txtBoxVolume.Text = @"";
                    txtBoxRate.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPayoutFC.Text = @"";
                    txtBoxTaxAtSource.Text = @"";
                    txtBoxCapitalGainsTax.Text = @"";
                    txtBoxSolidarityTax.Text = @"";
                    txtBoxTax.Text = @"";
                    txtBoxPayoutAfterTax.Text = @"";
                    txtBoxPrice.Text = @"";
                    txtBoxDocument.Text = @"";

                    // Reset status message
                    toolStripStatusLabelMessage.Text = @"";
                }

                txtBoxExchangeRatio.Focus();
            }
            else
            {
                // Disable controls
                txtBoxExchangeRatio.Enabled = false;
                txtBoxExchangeRatio.ReadOnly = true;
                cbxBoxDividendFCUnit.Enabled = false;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxExchangeRatio.Text = @"";
                    txtBoxVolume.Text = @"";
                    txtBoxRate.Text = @"";
                    txtBoxPayout.Text = @"";
                    txtBoxPayoutFC.Text = @"";
                    txtBoxTaxAtSource.Text = @"";
                    txtBoxCapitalGainsTax.Text = @"";
                    txtBoxSolidarityTax.Text = @"";
                    txtBoxTax.Text = @"";
                    txtBoxPayoutAfterTax.Text = @"";
                    txtBoxPrice.Text = @"";
                    txtBoxDocument.Text = @"";

                    // Reset status message
                    toolStripStatusLabelMessage.Text = @"";
                }

                txtBoxRate.Focus();
            }

            // Check if any volume is present
            if (ShareObjectFinalValue.Volume > 0)
            {
                // Set current share value
                txtBoxVolume.Text = ShareObjectFinalValue.VolumeAsStr;
            }
        }

        #endregion Methods
    }
}
