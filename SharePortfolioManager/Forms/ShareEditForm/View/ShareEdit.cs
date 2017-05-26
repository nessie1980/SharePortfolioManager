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
using SharePortfolioManager.Forms.BuysForm.Model;
using SharePortfolioManager.Forms.BuysForm.Presenter;
using SharePortfolioManager.Forms.BuysForm.View;
using SharePortfolioManager.Forms.DividendForm.Model;
using SharePortfolioManager.Forms.DividendForm.Presenter;
using SharePortfolioManager.Forms.DividendForm.View;
using SharePortfolioManager.Forms.SalesForm;
using SharePortfolioManager.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class FrmShareEdit : Form
    {
        #region Variables

        /// <summary>
        /// Stores the parent window
        /// </summary>
        private FrmMain _parentWindow;

        /// <summary>
        /// Stores the logger
        /// </summary>
        private Logger _logger;

        /// <summary>
        /// Stores the language file
        /// </summary>
        private Language _language;

        /// <summary>
        /// Stores the language
        /// </summary>
        private string _languageName;

        /// <summary>
        /// Stores the WKN number of the share which should be edited
        /// </summary>
        private string _shareWkn;

        /// <summary>
        /// Stores if the share must be saved because the dividends has been modified
        /// </summary>
        private bool _bSave;

        /// <summary>
        /// Stores the market value share object which should be edited
        /// </summary>
        private ShareObjectMarketValue _shareObjectMarketValue;

        /// <summary>
        /// Stores the final value share object which should be edited
        /// </summary>
        private ShareObjectFinalValue _shareObjectFinalValue;

        /// <summary>
        /// Flag if the form should be closed
        /// </summary>
        private bool _stopFomClosing;

        #endregion Variables

        #region Properties

        public FrmMain ParentWindow
        {
            get { return _parentWindow; }
            set { _parentWindow = value; }
        }

        public Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public Language Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string LanguageName
        {
            get { return _languageName; }
            set { _languageName = value; }
        }

        public string ShareWkn
        {
            get { return _shareWkn; }
            set { _shareWkn = value; }
        }

        public bool Save
        {
            get { return _bSave; }
            set { _bSave = value; }
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

        public bool StopFomClosingFlag
        {
            get { return _stopFomClosing; }
            set { _stopFomClosing = value; }
        }

        #endregion Properties

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        public FrmShareEdit(FrmMain parentWindow, Logger logger, Language xmlLanguage, String shareWkn, String language )
        {
            InitializeComponent();

            ParentWindow = parentWindow;
            Logger = logger;
            Language = xmlLanguage;
            LanguageName = language;
            ShareWkn = shareWkn;
            Save = false;
        }

        /// <summary>
        /// This function loads the language keys for the form elements and
        /// then loads the share object values to the TextBoxes
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">EventArgs</param>
        private void FrmShareEdit_Load(object sender, EventArgs e)
        {
            try
            {
                #region Fill in the values of the share object

                if (ParentWindow.ShareObjectMarketValue != null && ParentWindow.ShareObjectFinalValue != null)
                {
                    ShareObjectMarketValue = ParentWindow.ShareObjectMarketValue;
                    ShareObjectFinalValue = ParentWindow.ShareObjectFinalValue;

                    Thread.CurrentThread.CurrentCulture = ShareObjectMarketValue.CultureInfo;

                    #region GroupBox General 

                    lblWknValue.Text = ShareObjectFinalValue.Wkn;
                    lblDateValue.Text = ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Values.First().BuyListYear.First().Date;
                    txtBoxName.Text = ShareObjectFinalValue.Name;
                    lblPurchaseValue.Text = ShareObjectFinalValue.PurchaseValueAsStr;
                    lblDepositUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblVolumeValue.Text = ShareObjectFinalValue.VolumeAsStr;
                    lblVolumeUnit.Text = ShareObject.PieceUnit;
                    txtBoxWebSite.Text = ShareObjectFinalValue.WebSite;

                    #region Get culture info

                    List<string> list = new List<string>();
                    foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                    {
                        string specName = "(none)";
                        try
                        {
                            specName = CultureInfo.CreateSpecificCulture(ci.Name).Name;
                        }
                        catch
                        { }

                        list.Add(string.Format("{0}", ci.Name));
                    }

                    list.Sort();

                    foreach (var value in list)
                    {
                        if (value != "")
                            cboBoxCultureInfo.Items.Add(value);
                    }

                    cboBoxCultureInfo.SelectedIndex = cboBoxCultureInfo.FindStringExact(ShareObjectMarketValue.CultureInfo.Name);

                    #endregion Get culture info

                    #endregion GroupBox General
                    
                    #region GroupBox EarningsExpenditure

                    lblBuysValue.Text = ShareObjectFinalValue.AllBuyEntries.BuyMarketValueReductionTotalAsStr;
                    lblBuysUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblSalesValue.Text = ShareObjectFinalValue.AllSaleEntries.SaleValueTotalAsString;
                    lblSalesUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblProfitLossValue.Text = ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotalAsString;
                    lblProfitLossUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblDividendValue.Text = ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesAsString;
                    lblDividendUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblCostValue.Text = ShareObjectFinalValue.AllCostsEntries.CostValueTotalAsString;
                    lblCostUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                    #endregion GroupBox EarningsExpenditure

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
                }

                #endregion Fill in the values of the share object
                
                #region Language configuration

                #region GroupBox General

                Text = Language.GetLanguageTextByXPath(@"/EditFormShare/Caption", LanguageName);
                grpBoxGeneral.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Caption", LanguageName);
                lblWkn.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/WKN", LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Date", LanguageName);
                lblName.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Name", LanguageName);
                lblPurchase.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Purchase", LanguageName);
                lblVolume.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Volume", LanguageName);
                lblWebSite.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/WebSite", LanguageName);
                lblCultureInfo.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/CultureInfo", LanguageName);
                lblDividendPayoutInterval.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/PayoutInterval", LanguageName);

                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item0",
                        LanguageName));
                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item1",
                        LanguageName));
                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item2",
                        LanguageName));
                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item3",
                        LanguageName));

                // Select the payout interval for the dividend
                cbxDividendPayoutInterval.SelectedIndex = ShareObjectFinalValue.DividendPayoutInterval;

                #endregion GroupBox General

                #region GroupBox EarningsExpenditure

                grpBoxEarningsExpenditure.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Caption", LanguageName);
                lblBuys.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Buys", LanguageName);
                btnShareBuysEdit.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Buys",
                    LanguageName);
                lblSales.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Sales",
                    LanguageName);
                btnShareSalesEdit.Text =
                    Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Sales",
                    LanguageName);
                if (ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotal < 0)
                    lblProfitLoss.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Loss",
                        LanguageName);
                else
                    lblProfitLoss.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Profit",
                        LanguageName);
                lblDividend.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Dividend",
                    LanguageName);
                btnShareDividendsEdit.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Dividend",
                    LanguageName);
                lblCost.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Costs", LanguageName);
                btnShareCostsEdit.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Costs",
                    LanguageName);

                // Load button images
                btnShareBuysEdit.Image = Resources.black_edit;
                btnShareSalesEdit.Image = Resources.black_edit;
                btnShareCostsEdit.Image = Resources.black_edit;
                btnShareDividendsEdit.Image = Resources.black_edit;

                #endregion GroupBox EarningsExpenditure

                btnSave.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/Buttons/Save", LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/Buttons/Cancel", LanguageName);

                // Load button images
                btnSave.Image = Resources.black_save;
                btnCancel.Image = Resources.black_cancel;

                #endregion Language configuration

            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("FrmShareEdit_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/ShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function is called when the form is closing
        /// Here we check if the form really should be closed
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void FrmShareEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the form should be closed
            if (StopFomClosingFlag)
            {
                // Stop closing the form
                e.Cancel = true;
            }

            // Reset closing flag
            StopFomClosingFlag = false;

            // Set the dialog result correct of saving or not saving the share
            // Share must be saved because a dividend has been added or deleted
            if (Save)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }

        #endregion Form

        #region Button

        /// <summary>
        /// This function stores the values to the share object
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                StopFomClosingFlag = true;
                var errorFlag = false;
                decimal volume = 0;
                decimal purchase = 0;

                statusStrip1.ForeColor = Color.Red;

                if (txtBoxName.Text == @"")
                {
                    txtBoxName.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameEmpty", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else
                {
                    // Check if a market value share with the given share name already exists
                    foreach (var shareObjectMarketValue in ParentWindow.ShareObjectListMarketValue)
                    {
                        if (shareObjectMarketValue.Name == txtBoxName.Text && shareObjectMarketValue != ShareObjectMarketValue)
                        {
                            errorFlag = true;
                            StopFomClosingFlag = true;
                            txtBoxName.Focus();
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameExists", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                            break;
                        }
                    }

                    if (errorFlag == false)
                    {
                        // Check if a market value share with the given share name already exists
                        foreach (var shareObjectFinalValue in ParentWindow.ShareObjectListFinalValue)
                        {
                            if (shareObjectFinalValue.Name == txtBoxName.Text && shareObjectFinalValue != ShareObjectFinalValue)
                            {
                                errorFlag = true;
                                StopFomClosingFlag = true;
                                txtBoxName.Focus();
                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                    Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameExists", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                                break;
                            }
                        }
                    }
                }

                if (lblPurchaseValue.Text == @"" && errorFlag == false)
                {
                    lblPurchaseValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/DepositEmpty", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (!Decimal.TryParse(lblPurchaseValue.Text, out purchase) && errorFlag == false)
                {
                    lblPurchaseValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/DepositWrongFormat", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (purchase <= 0 && errorFlag == false)
                {
                    lblPurchaseValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/DepositWrongValue", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (lblVolumeValue.Text == @"" && errorFlag == false)
                {
                    lblVolumeValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/VolumeEmpty", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (!Decimal.TryParse(lblVolumeValue.Text, out volume) && errorFlag == false)
                {
                    lblVolumeValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/VolumeWrongFormat", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (volume <= 0 && errorFlag == false)
                {
                    lblVolumeValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/VolumeWrongValue", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (txtBoxWebSite.Text == @"" && errorFlag == false)
                {
                    txtBoxWebSite.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteEmpty", LanguageName),
                        Language, LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (errorFlag == false)
                {
                    // Check if a market value share with the given WKN number already exists
                    foreach (var shareObjectMarketValue in ParentWindow.ShareObjectListMarketValue)
                    {
                        if (shareObjectMarketValue.WebSite == txtBoxWebSite.Text && shareObjectMarketValue != ShareObjectMarketValue)
                        {
                            errorFlag = true;
                            StopFomClosingFlag = true;
                            txtBoxWebSite.Focus();
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteExists", LanguageName),
                                Language, LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                            break;
                        }
                    }

                    if (errorFlag == false)
                    {
                        // Check if a final value share with the given WKN number already exists
                        foreach (var shareObjectFinalValue in ParentWindow.ShareObjectListFinalValue)
                        {
                            if (shareObjectFinalValue.WebSite == txtBoxWebSite.Text && shareObjectFinalValue != ShareObjectFinalValue)
                            {
                                errorFlag = true;
                                StopFomClosingFlag = true;
                                txtBoxWebSite.Focus();
                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                    Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteExists", LanguageName),
                                    Language, LanguageName,
                                    Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                                break;
                            }
                        }
                    }
                }

                if (errorFlag == false)
                {
                    StopFomClosingFlag = false;
                    Save = true;

                    CultureInfo cultureInfo = new CultureInfo(cboBoxCultureInfo.GetItemText(cboBoxCultureInfo.SelectedItem));

                    // Market value share
                    ShareObjectMarketValue.Wkn = lblWknValue.Text;
                    ShareObjectMarketValue.Name = txtBoxName.Text;
                    ShareObjectMarketValue.Volume = volume;
                    ShareObjectMarketValue.PurchaseValue = purchase;
                    ShareObjectMarketValue.WebSite = txtBoxWebSite.Text;
                    ShareObjectMarketValue.CultureInfo = cultureInfo;

                    // Final value share
                    ShareObjectFinalValue.Wkn = lblWknValue.Text;
                    ShareObjectFinalValue.Name = txtBoxName.Text;
                    ShareObjectFinalValue.Volume = volume;
                    ShareObjectFinalValue.PurchaseValue = purchase;
                    ShareObjectFinalValue.WebSite = txtBoxWebSite.Text;
                    ShareObjectFinalValue.CultureInfo = cultureInfo;
                    ShareObjectFinalValue.DividendPayoutInterval = cbxDividendPayoutInterval.SelectedIndex;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnSave_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                StopFomClosingFlag = true;
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/EditSaveFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
                StopFomClosingFlag = true;
            }
        }

        /// <summary>
        /// This function close the form
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            StopFomClosingFlag = false;
            Close();
        }

        /// <summary>
        /// This function opens the buy add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareBuysEdit_Click(object sender, EventArgs e)
        {
            IModelBuyEdit model = new ModelBuyEdit();
            IViewBuyEdit view = new ViewBuyEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, LanguageName);
            PresenterBuyEdit presenterBuyEdit = new PresenterBuyEdit (view, model);

            DialogResult dlgResult = view.ShowDialog();
            if (dlgResult == DialogResult.OK)
                Save = true;
            else
                Save = false;

            SetShareValuesToTextBoxes();
        }

        /// <summary>
        /// This function opens the sale add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareSalesEdit_Click(object sender, EventArgs e)
        {
            FrmShareSalesEdit shareSalesEdit = new FrmShareSalesEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, LanguageName);

            DialogResult dlgResult = shareSalesEdit.ShowDialog();
            if (dlgResult == DialogResult.OK)
                Save = true;
            else
                Save = false;

            SetShareValuesToTextBoxes();
        }

        /// <summary>
        /// This function opens the share dividend add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareDividendsEdit_Click(object sender, EventArgs e)
        {
            IModelDividendEdit model = new ModelDividendEdit();
            IViewDividendEdit view = new ViewDividendEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, LanguageName);
            PresenterDividendEdit presenterDividendEdit = new PresenterDividendEdit(view, model);

            DialogResult dlgResult = view.ShowDialog();
            if (dlgResult == DialogResult.OK)
                Save = true;
            else
                Save = false;

            SetShareValuesToTextBoxes();
        }

        /// <summary>
        /// This function opens the share cost add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareCostsEdit_Click(object sender, EventArgs e)
        {
            FrmShareCostEdit shareCostEdit = new FrmShareCostEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, LanguageName);

            DialogResult dlgResult = shareCostEdit.ShowDialog();
            if (dlgResult == DialogResult.OK)
                Save = true;
            else
                Save = false;

            SetShareValuesToTextBoxes();
        }

        /// <summary>
        /// This function sets the new values to the corresponding text boxes
        /// </summary>
        private void SetShareValuesToTextBoxes()
        {
            lblVolumeValue.Text = Helper.FormatDecimal(ShareObjectFinalValue.Volume, Helper.Volumefivelength, false, Helper.Volumetwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
            lblPurchaseValue.Text = Helper.FormatDecimal(ShareObjectFinalValue.PurchaseValue, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
            lblBuysValue.Text = Helper.FormatDecimal(ShareObjectFinalValue.AllBuyEntries.BuyMarketValueReductionTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
            lblSalesValue.Text = Helper.FormatDecimal(ShareObjectFinalValue.AllSaleEntries.SaleValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
            lblCostValue.Text = Helper.FormatDecimal(ShareObjectFinalValue.AllCostsEntries.CostValueTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
            lblProfitLossValue.Text = Helper.FormatDecimal(ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotal, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
            lblDividendValue.Text = Helper.FormatDecimal(ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxes, Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", ShareObjectFinalValue.CultureInfo);
        }

        #endregion Button
    }
}
