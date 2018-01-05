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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Properties;
using System.Linq;
using System.IO;
using System.ComponentModel;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.SalesForm.View
{
    public enum SaleErrorCode
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
        VolumeMaxValue,
        SalePriceEmpty,
        SalePriceWrongFormat,
        SalePriceWrongValue,
        TaxAtSourceWrongFormat,
        TaxAtSourceWrongValue,
        CapitalGainsTaxWrongFormat,
        CapitalGainsTaxWrongValue,
        SolidarityTaxWrongFormat,
        SolidarityTaxWrongValue,
        CostsWrongFormat,
        CostsWrongValue,
        DirectoryDoesNotExists,
        FileDoesNotExists,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExits,
        DocumentFileDoesNotExists
    };

    /// <inheritdoc />
    /// <summary>
    /// Interface of the SaleEdit view
    /// </summary>
    public interface IViewSaleEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValues;
        event EventHandler AddSale;
        event EventHandler EditSale;
        event EventHandler DeleteSale;
        event EventHandler DocumentBrowse;

        SaleErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        bool UpdateSale { get; set; }
        string SelectedDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Volume { get; set; }
        string BuyPrice { get; set; }
        string SalePrice { get; set; }
        string TaxAtSource { get; set; }
        string CapitalGainsTax { get; set; }
        string SolidarityTax { get; set; }
        string Costs { get; set; }
        string ProfitLoss { get; set; }
        string Payout { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
        void DocumentBrowseFinish();
    }

    public partial class ViewSaleEdit : Form, IViewSaleEdit
    {
        #region Fields

        /// <summary>
        /// Stores the date of a selected sale row
        /// </summary>
        private string _selectedDate;

        #endregion Fields

        #region Properties

        #region Transfer parameter

        public Logger Logger { get; internal set; }

        public Language Language { get; internal set; }

        public string LanguageName { get; internal set; }

        #endregion Transfer parameter

        #region Flags

        public bool UpdateSaleFlag { get; set; }

        public bool SaveFlag { get; internal set; }

        public bool LoadGridSelectionFlag { get; internal set; }

        #endregion Flags

        public DataGridView SelectedDataGridView { get; internal set; }

        #endregion Properties

        #region IViewMember

        public bool UpdateSale
        {
            get => UpdateSaleFlag;
            set
            {
                UpdateSaleFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateSale"));
            }
        }

        public SaleErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

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

        public string BuyPrice
        {
            get => txtBoxBuyPrice.Text;
            set
            {
                if (txtBoxBuyPrice.Text == value)
                    return;
                txtBoxBuyPrice.Text = value;
            }
        }

        public string SalePrice
        {
            get => txtBoxSalePrice.Text;
            set
            {
                if (txtBoxSalePrice.Text == value)
                    return;
                txtBoxSalePrice.Text = value;
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

        public string Costs
        {
            get => txtBoxCosts.Text;
            set
            {
                if (txtBoxCosts.Text == value)
                    return;
                txtBoxCosts.Text = value;
            }
        }

        public string ProfitLoss
        {
            get => txtBoxProfitLoss.Text;
            set
            {
                if (txtBoxProfitLoss.Text == value)
                    return;
                txtBoxProfitLoss.Text = value;
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
            string strMessage = @"";
            Color clrMessage = Color.Black;
            FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case SaleErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the sale list
                        ShowSales();

                        // Reset values
                        Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case SaleErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add",
                                LanguageName);
                        btnAddSave.Image = Resources.black_add;
                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/EditSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the sale list
                        ShowSales();

                        // Reset values
                        Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case SaleErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.black_add;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the sale list
                        ShowSales();

                        // Reset values
                        Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case SaleErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.InputValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.DateExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case SaleErrorCode.DateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.VolumeMaxValue:
                    {
                        if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName))
                        {
                            strMessage =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_1", LanguageName) +
                                ShareObjectFinalValue.Volume +
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_2", LanguageName);
                        }
                        else
                        {
                            strMessage =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_1", LanguageName) +
                                (ShareObjectFinalValue.Volume + ShareObjectFinalValue.AllSaleEntries.GetSaleObjectByDateTime(datePickerDate.Text + " " + datePickerTime.Text).Volume) +
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue_2", LanguageName);
                        }
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxVolume.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.SalePriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SalePriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSalePrice.Focus();

                        break;
                    }
                case SaleErrorCode.TaxAtSourceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case SaleErrorCode.TaxAtSourceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/TaxAtSourceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxTaxAtSource.Focus();

                        break;
                    }
                case SaleErrorCode.CapitalGainsTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case SaleErrorCode.CapitalGainsTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CapitalGainsTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCapitalGainsTax.Focus();

                        break;
                    }
                case SaleErrorCode.SolidarityTaxWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case SaleErrorCode.SolidarityTaxWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SolidarityTaxWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxSolidarityTax.Focus();

                        break;
                    }
                case SaleErrorCode.CostsWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CostsWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case SaleErrorCode.CostsWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CostsWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case SaleErrorCode.DirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DirectoryDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDocument.Focus();

                        break;
                    }
                case SaleErrorCode.FileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/FileDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxDocument.Focus();

                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
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
                case SaleErrorCode.DocumentBrowseFailed:
                    {
                        txtBoxDocument.Text = @"-";

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ChoseDocumentFailed", LanguageName);
                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
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
        public event EventHandler FormatInputValues;
        public event EventHandler AddSale;
        public event EventHandler EditSale;
        public event EventHandler DeleteSale;
        public event EventHandler DocumentBrowse;

        #endregion Event members

        #region Methods

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final share object</param>
        /// <param name="logger">Logger</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public ViewSaleEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue, Logger logger, Language xmlLanguage, string language)
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
        private void ShareSalesEdit_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/Caption", LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxSales.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/Caption",
                        LanguageName);
                lblAddSaleDate.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                lblVolume.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Volume",
                        LanguageName);
                lblBuyPrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/BuyPrice",
                        LanguageName);
                lblSalePrice.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SalesPrice",
                        LanguageName);
                lblTaxAtSource.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/TaxAtSource",
                        LanguageName);
                lblCapitalGainsTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/CapitalGainsTax",
                        LanguageName);
                lblSolidarityTax.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SolidarityTax",
                        LanguageName);
                lblCosts.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Costs",
                        LanguageName);
                lblProfitLoss.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/ProfitLoss",
                        LanguageName);
                lblPayout.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Payout",
                        LanguageName);
                lblSalesDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add",
                    LanguageName);
                btnReset.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Reset",
                    LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                #endregion Language configuration

                #region Unit configuration

                // Set sale units to the edit boxes
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblBuyPriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSalePriceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblTaxAtSourceUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCapitalGainsTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblSolidarityTaxUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblCostsUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblProfitLossUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                lblPayoutUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.black_add;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                #endregion Image configuration

                ShowSales();
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"ShareSalesEdit_Load()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a buy has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareSalesEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = SaveFlag ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date
        /// </summary>
        private void ResetInputValues()
        {
            // Reset date time picker
            datePickerDate.Value = DateTime.Now;
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            // Reset text boxes
            txtBoxVolume.Text = @"";
            txtBoxBuyPrice.Text = @"";
            txtBoxSalePrice.Text = @"";
            txtBoxTaxAtSource.Text = @"";
            txtBoxCapitalGainsTax.Text = @"";
            txtBoxSolidarityTax.Text = @"";
            txtBoxCosts.Text = @"";
            txtBoxProfitLoss.Text = @"";
            txtBoxPayout.Text = @"";
            txtBoxDocument.Text = @"";

            // Check if any volume is present
            if (ShareObjectFinalValue.Volume > 0)
            {
                // Set current share value
                txtBoxVolume.Text = ShareObjectFinalValue.VolumeAsStr;

                // Set current share price
                txtBoxSalePrice.Text = ShareObjectFinalValue.CurPriceAsStr;

                // Set current buy price
                txtBoxBuyPrice.Text = ShareObjectFinalValue.AverageBuyPriceAsStr;
            }
            else
            {
                datePickerDate.Enabled = false;
                datePickerTime.Enabled = false;

                txtBoxVolume.Enabled = false;
                txtBoxTaxAtSource.Enabled = false;
                txtBoxCapitalGainsTax.Enabled = false;
                txtBoxSolidarityTax.Enabled = false;

                txtBoxSalePrice.Enabled = false;
                txtBoxCosts.Enabled = false;
                txtBoxDocument.Enabled = false;
                btnSalesDocumentBrowse.Enabled = false;

                btnAddSave.Enabled = false;
            }

            txtBoxVolume.Focus();
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
            FormatInputValues?.Invoke(this, new EventArgs());
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
            FormatInputValues?.Invoke(this, new EventArgs());
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
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddBuyPrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BuyPrice"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxBuyPrice_Leave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSalePrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SalePrice"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxSalePrice_Leave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());

        }

        /// <summary>
        /// This function updates the model if the text has changed
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
        private void OnTxtBoxTaxAtSourceLeave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
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
        private void OnTxtBoxCapitalGAinsTaxLeave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
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
        private void OnTxtBoxSolidarityTaxLeave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This function updates the model if the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCosts_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Costs"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCostsTaxLeave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
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
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var filePath in files)
            {
                txtBoxDocument.Text = filePath;
            }
        }

        #endregion TextBoxes

        #region Buttons

        /// <summary>
        /// This function adds a new sale entry to the share object
        /// or edit a sale entry
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

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormBuy/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    UpdateSale = false;

                    AddSale?.Invoke(this, null);
                }
                else
                {
                    UpdateSale = true;

                    EditSale?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"btnAdd_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the sale.
        /// If the user press "Ok" the sale will be deleted and the
        /// list of the sales will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">EventArgs</param>
        private void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                // Disable controls
                Enabled = false;

                toolStripStatusLabelMessageSaleEdit.Text = @"";

                var strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/SaleDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    DeleteSale?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"btnDelete_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a sale
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelMessageSaleEdit.Text = @"";

                // Enable button(s)
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.black_add;

                // Disable button(s)
                btnReset.Enabled = false;
                btnDelete.Enabled = false;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                SelectedDataGridView = null;

                // Select overview tab
                if (
                    tabCtrlSales.TabPages.ContainsKey(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", LanguageName)))
                    tabCtrlSales.SelectTab(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", LanguageName));
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"btnReset_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CancelFailure", LanguageName),
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
        /// This function paints the sale list of the share
        /// </summary>
        private void ShowSales()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlSales.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView view)) continue;

                        view.SelectionChanged -= DataGridViewSalesOfAYear_SelectionChanged;
                        view.SelectionChanged -= DataGridViewSalesOfYears_SelectionChanged;
                        view.DataBindingComplete -= DataGridViewSalesOfAYear_DataBindingComplete;
                    }
                    tabPage.Controls.Clear();
                    tabCtrlSales.TabPages.Remove(tabPage);
                }

                tabCtrlSales.TabPages.Clear();

                #region Add page

                // Create TabPage for the sales of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", LanguageName)
                           + @" ("
                           + ShareObjectFinalValue.AllSaleEntries.SalePayoutTotalWithUnitAsStr
                           + @" / "
                           + ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotalWithUnitAsStr
                           + @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Create Binding source for the sale data
                var bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllSaleEntries.GetAllSalesTotalValues();

                // Create DataGridView
                var dataGridViewSalesOverviewOfAYears = new DataGridView
                {
                    Dock = DockStyle.Fill,

                    // Bind source with buy values to the DataGridView
                    DataSource = bindingSourceOverview
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewSalesOverviewOfAYears.DataBindingComplete += DataGridViewSalesOfAYear_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewSalesOverviewOfAYears.MouseEnter += OnDataGridViewSalesOfYears_MouseEnter;
                // Set the delegate for the mouse leave event
                dataGridViewSalesOverviewOfAYears.MouseLeave += OnDataGridViewSalesOfYears_MouseLeave;
                // Set row select event
                dataGridViewSalesOverviewOfAYears.SelectionChanged += DataGridViewSalesOfYears_SelectionChanged;

                #endregion Events

                #region Style

                // Advanced configuration DataGridView buys
                var styleOverviewOfYears = dataGridViewSalesOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dataGridViewSalesOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridViewSalesOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewSalesOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


                dataGridViewSalesOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewSalesOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dataGridViewSalesOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dataGridViewSalesOverviewOfAYears.MultiSelect = false;

                dataGridViewSalesOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewSalesOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewSalesOverviewOfAYears.AllowUserToResizeRows = false;

                dataGridViewSalesOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewSalesOverviewOfAYears);
                dataGridViewSalesOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlSales.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlSales;

                #endregion Control add

                // Check if sales exists
                if (ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Count <= 0) return;

                // Loop through the years of the sales
                foreach (
                    var keyName in ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary.Keys.Reverse()
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
                               + ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                   .SalePayoutYearWithUnitAsStr
                               + @" / "
                               + ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                   .SaleProfitLossYearWithUnitAsStr
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Create Binding source for the sale data
                    var bindingSource = new BindingSource
                    {
                        DataSource = ShareObjectFinalValue.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                            .SaleListYear
                    };

                    // Create DataGridView
                    var dataGridViewSalesOfAYear = new DataGridView
                    {
                        Dock = DockStyle.Fill,

                        // Bind source with buy values to the DataGridView
                        DataSource = bindingSource
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewSalesOfAYear.DataBindingComplete += DataGridViewSalesOfAYear_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewSalesOfAYear.MouseEnter += OnDataGridViewSalesOfAYear_MouseEnter;
                    // Set the delegate for the mouse leave event
                    dataGridViewSalesOfAYear.MouseLeave += OnDataGridViewSalesOfAYear_MouseLeave;
                    // Set row select event
                    dataGridViewSalesOfAYear.SelectionChanged += DataGridViewSalesOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewSalesOfAYear.CellContentDoubleClick += DataGridViewSalesOfAYear_CellContentdecimalClick;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView buys
                    var style = dataGridViewSalesOfAYear.ColumnHeadersDefaultCellStyle;
                    style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dataGridViewSalesOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    dataGridViewSalesOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    dataGridViewSalesOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                    dataGridViewSalesOfAYear.RowHeadersVisible = false;
                    dataGridViewSalesOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridViewSalesOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    dataGridViewSalesOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                    dataGridViewSalesOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    dataGridViewSalesOfAYear.MultiSelect = false;

                    dataGridViewSalesOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewSalesOfAYear.AllowUserToResizeColumns = false;
                    dataGridViewSalesOfAYear.AllowUserToResizeRows = false;

                    dataGridViewSalesOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewSalesOfAYear);
                    dataGridViewSalesOfAYear.Parent = newTabPage;
                    tabCtrlSales.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlSales;

                    #endregion Control add
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"ShowSales()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the data binding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void DataGridViewSalesOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Date",
                                    LanguageName);
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Volume",
                                    LanguageName) + @" (" + ShareObject.PieceUnit + @")";
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Purchase",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_ProfitLoss",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 4:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Sale",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 5:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Document",
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
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfAYear_DataBindingComplete()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/RenameColHeaderFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }

        }

        /// <summary>
        /// This function deselects all selected rows of the
        /// DataGridViews in the TabPages
        /// </summary>
        private void DeselectRowsOfDataGridViews(DataGridView dataGridView)
        {
            try
            {
                // Deselect the row
                foreach (TabPage tabPage in tabCtrlSales.TabPages)
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

                ResetInputValues();
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"DeselectRowsOfDataGridViews()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeselectFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the sale
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnSaleDocumentBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                txtBoxDocument.Text = Helper.SetDocument(Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/OpenFileDialog/Title", LanguageName), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"btnSaleDocumentBrowse_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ChoseDocumentFailed", LanguageName),
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
        private void TabCtrlSales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlSales.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabCtrlSales.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                    view.Focus();
            }
        }

        /// <summary>
        /// This function sets the focus on the data grid view of the current selected tab page of the tab control
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlSales_MouseEnter(object sender, EventArgs e)
        {
            if (tabCtrlSales.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabCtrlSales.SelectedTab.Controls)
            {
                if (control is DataGridView view)
                    view.Focus();
            }
        }

        /// <summary>
        /// This function sets the focus to the group box add / edit when the mouse leaves the data grid view
        /// </summary>
        /// <param name="sender">Tab control</param>
        /// <param name="e">EventArgs</param>
        private void TabCtrlSales_MouseLeave(object sender, EventArgs e)
        {
            grpBoxAdd.Focus();
        }

        #endregion Tab control delegates

        #region Sales of years

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewSalesOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlSales.TabPages)
                {
                    if (tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    tabCtrlSales.SelectTab(tabPage);
                    tabPage.Focus();

                    // Deselect rows
                    DeselectRowsOfDataGridViews(null);

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfYears_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewSalesOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewSalesOfYears_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
        }

        #endregion Sales of years

        #region Sales of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataGridViewSalesOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                btnAddSave.Enabled = true;
                btnSalesDocumentBrowse.Enabled = true;
                txtBoxDocument.Enabled = true;

                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    var curItem = ((DataGridView)sender).SelectedRows;

                    // Set selected date
                    SelectedDate = curItem[0].Cells[0].Value.ToString();

                    // Get SaleObject of the selected DataGridView row
                    var saleObject = ShareObjectFinalValue.AllSaleEntries.GetSaleObjectByDateTime(SelectedDate);
                    if (saleObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(saleObject.Date);
                        datePickerTime.Value = Convert.ToDateTime(saleObject.Date);
                        txtBoxVolume.Text = saleObject.VolumeAsStr;
                        txtBoxBuyPrice.Text = saleObject.BuyPriceAsStr;
                        txtBoxSalePrice.Text = saleObject.SalePriceAsStr;
                        txtBoxTaxAtSource.Text = saleObject.TaxAtSourceAsStr;
                        txtBoxCapitalGainsTax.Text = saleObject.CapitalGainsTaxAsStr;
                        txtBoxSolidarityTax.Text = saleObject.SolidarityTaxAsStr;
                        txtBoxCosts.Text = saleObject.CostsAsStr;
                        txtBoxProfitLoss.Text = saleObject.ProfitLossAsStr;
                        txtBoxPayout.Text = saleObject.PayoutAsStr;
                        txtBoxDocument.Text = saleObject.Document;
                    }
                    else
                    {
                        // TODO
                        datePickerDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        datePickerTime.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        txtBoxVolume.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxProfitLoss.Text = curItem[0].Cells[3].Value.ToString();
                        txtBoxPayout.Text = curItem[0].Cells[4].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[5].Value.ToString();
                    }

                    if (ShareObjectFinalValue.AllBuyEntries.IsDateLastDate(SelectedDate) &&
                        ShareObjectFinalValue.AllSaleEntries.IsDateLastDate(SelectedDate)
                        )
                    {
                        // Check if a sale exists
                        btnDelete.Enabled = ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare().Count > 1;

                        // Enable text box(es)
                        datePickerDate.Enabled = true;
                        datePickerTime.Enabled = true;
                        txtBoxVolume.Enabled = true;
                        txtBoxSalePrice.Enabled = true;
                        txtBoxTaxAtSource.Enabled = true;
                        txtBoxCapitalGainsTax.Enabled = true;
                        txtBoxSolidarityTax.Enabled = true;
                        txtBoxCosts.Enabled = true;
                    }
                    else
                    {
                        // Disable Button(s)
                        btnDelete.Enabled = false;
                        // Disable text box(es)
                        datePickerDate.Enabled = false;
                        datePickerTime.Enabled = false;
                        txtBoxVolume.Enabled = false;
                        txtBoxSalePrice.Enabled = false;
                        txtBoxTaxAtSource.Enabled = false;
                        txtBoxCapitalGainsTax.Enabled = false;
                        txtBoxSolidarityTax.Enabled = false;
                        txtBoxCosts.Enabled = false;
                    }

                    // Enable button(s)
                    btnReset.Enabled = true;

                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;
                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;
                    // Enable Button(s)
                    btnAddSave.Enabled = true;
                    // Enable text box(es)
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;
                    txtBoxVolume.Enabled = true;
                    txtBoxSalePrice.Enabled = true;
                    txtBoxTaxAtSource.Enabled = true;
                    txtBoxCapitalGainsTax.Enabled = true;
                    txtBoxSolidarityTax.Enabled = true;
                    txtBoxCosts.Enabled = true;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfAYear_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewSalesOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewSalesOfAYear_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
        }

        /// <summary>
        /// This function opens the sale document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void DataGridViewSalesOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
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
                // Get date and time of the selected sale item
                var strDateTime = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value.ToString() == @"-") return;

                // Get doc from the sale with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllSaleEntries.GetAllSalesOfTheShare())
                {
                    // Check if the sale date and time is the same as the date and time of the clicked sale item
                    if (temp.Date != strDateTime) continue;

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
                            // Remove sale object and add it with no document
                            if (ShareObjectFinalValue.RemoveSale(temp.Date) &&
                                ShareObjectFinalValue.AddSale(strDateTime, temp.Volume, temp.BuyPrice, temp.SalePrice, temp.TaxAtSource, temp.CapitalGainsTax, temp.SolidarityTax, temp.Costs))
                            {
                                // Set flag to save the share object.
                                SaveFlag = true;

                                ResetInputValues();
                                ShowSales();

                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/StateMessages/EditSuccess", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                            else
                            {
                                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormSale/Errors/EditFailed", LanguageName),
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
#if DEBUG_SALE || DEBUG
                var message = $"dataGridViewSalesOfAYear_CellContentdecimalClick()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageSaleEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Sales of a year

        #endregion Data grid view

//        /// <summary>
//        /// This function calculates the profit / loss of the given values
//        /// If a given value is not valid the profit / loss text box is set to "-"
//        /// <param name="strSaleVolume">Sale volume of the share</param>
//        /// <param name="strSalePrice">Current price of the share</param>
//        /// <param name="decShareDeposit">Current deposit of the share</param>
//        /// <param name="decShareVolume">Current share volume</param>
//        /// <returns>String with the profit / loss or "-" if the calculation failed</returns>>
//        /// </summary>
//        private string CalculateProfitLoss(string strSaleVolume, string strSalePrice, decimal decShareDeposit, decimal decShareVolume)
//        {
//            try
//            {
//                decimal volume = 0;

//                if (decimal.TryParse(strSaleVolume, out volume) && volume > 0)
//                {
//                    decimal price = 0;

//                    if (decimal.TryParse(strSalePrice, out price))
//                    {
//                        decimal decSalePrice = price * volume;

//                        // Check if the share deposit and share volume is not "0"
//                        decimal decDeposit = 0;
//                        if (decShareDeposit > 0 && decShareVolume > 0)
//                        {
//                            decDeposit = volume * decShareDeposit / decShareVolume;
//                        }
                        
//                        return Helper.FormatDecimal((decSalePrice - decDeposit), Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
//                    }
//                    else
//                    {
//                        return @"-";
//                    }
//                }
//                else
//                {
//                    return @"-";
//                }
//            }
//            catch (Exception ex)
//            {
//#if DEBUG_SALE || DEBUG
//                var message = $"CalculatePrice()\n\n{ex.Message}";
//                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
//                    MessageBoxIcon.Error);
//#endif
//                return @"-";
//            }
//        }

        #endregion Methods
    }
}

