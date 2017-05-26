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
        private Language _xmlLanguage;

        /// <summary>
        /// Stores the language
        /// </summary>
        private String _strLanguage;

        /// <summary>
        /// Stores the WKN number of the share which should be edited
        /// </summary>
        private String _shareWkn;

        /// <summary>
        /// Stores if the share must be saved because the dividends has been modified
        /// </summary>
        private Boolean _bSave;

        /// <summary>
        /// Stores the share object which should be edited
        /// </summary>
        private ShareObject _shareObject;
        /// <summary>
        /// Flag if the form should be closed
        /// </summary>
        private bool _stopFomClosing;

        #endregion Variables

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        public FrmShareEdit(FrmMain parentWindow, Logger logger, Language xmlLanguage, String shareWkn, String language )
        {
            InitializeComponent();

            _parentWindow = parentWindow;
            _logger = logger;
            _xmlLanguage = xmlLanguage;
            _strLanguage = language;
            _shareWkn = shareWkn;
            _bSave = false;
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

                if (_parentWindow.ShareObject != null)
                {
                    _shareObject = _parentWindow.ShareObject;

                    Thread.CurrentThread.CurrentCulture = _shareObject.CultureInfo;

                    #region GroupBox General 

                    lblWknValue.Text = _shareObject.Wkn;
                    lblDateValue.Text = _shareObject.AllBuyEntries.AllBuysOfTheShareDictionary.Values.First().BuyListYear.First().Date;
                    txtBoxName.Text = _shareObject.Name;
                    lblDepositValue.Text = _shareObject.MarketValueAsStr;
                    lblDepositUnit.Text = _shareObject.CurrencyUnit;
                    lblVolumeValue.Text = _shareObject.VolumeAsStr;
                    lblVolumeUnit.Text = ShareObject.PieceUnit;
                    txtBoxWebSite.Text = _shareObject.WebSite;

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

                    cboBoxCultureInfo.SelectedIndex = cboBoxCultureInfo.FindStringExact(_shareObject.CultureInfo.Name);

                    #endregion Get culture info

                    #endregion GroupBox General
                    
                    #region GroupBox EarningsExpenditure

                    lblBuysValue.Text = _shareObject.AllBuyEntries.BuyMarketValueTotalAsStr;
                    lblBuysUnit.Text = _shareObject.CurrencyUnit;
                    lblSalesValue.Text = _shareObject.AllSaleEntries.SaleValueTotalAsString;
                    lblSalesUnit.Text = _shareObject.CurrencyUnit;
                    lblProfitLossValue.Text = _shareObject.AllSaleEntries.SaleProfitLossTotalAsString;
                    lblProfitLossUnit.Text = _shareObject.CurrencyUnit;
                    lblDividendValue.Text = _shareObject.AllDividendEntries.DividendValueTotalWithTaxesAsString;
                    lblDividendUnit.Text = _shareObject.CurrencyUnit;
                    lblCostValue.Text = _shareObject.AllCostsEntries.CostValueTotalAsString;
                    lblCostUnit.Text = _shareObject.CurrencyUnit;

                    #endregion GroupBox EarningsExpenditure

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
                }

                #endregion Fill in the values of the share object
                
                #region Language configuration

                #region GroupBox General

                Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Caption", _strLanguage);
                grpBoxGeneral.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Caption", _strLanguage);
                lblWkn.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/WKN", _strLanguage);
                lblDate.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Date", _strLanguage);
                lblName.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Name", _strLanguage);
                lblDeposit.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Deposit", _strLanguage);
                lblVolume.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Volume", _strLanguage);
                lblWebSite.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/WebSite", _strLanguage);
                lblCultureInfo.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/CultureInfo", _strLanguage);
                lblDividendPayoutInterval.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/PayoutInterval", _strLanguage);

                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item0",
                        _strLanguage));
                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item1",
                        _strLanguage));
                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item2",
                        _strLanguage));
                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item3",
                        _strLanguage));

                // Select the payout interval for the dividend
                cbxDividendPayoutInterval.SelectedIndex = _shareObject.DividendPayoutInterval;

                #endregion GroupBox General

                #region GroupBox EarningsExpenditure

                grpBoxEarningsExpenditure.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Caption", _strLanguage);
                lblBuys.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Buys", _strLanguage);
                btnShareBuysEdit.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Buys",
                    _strLanguage);
                lblSales.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Sales",
                    _strLanguage);
                btnShareSalesEdit.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Sales",
                    _strLanguage);
                if (_shareObject.AllSaleEntries.SaleProfitLossTotal < 0)
                    lblProfitLoss.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Loss",
                        _strLanguage);
                else
                    lblProfitLoss.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Profit",
                        _strLanguage);
                lblDividend.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Dividend",
                    _strLanguage);
                btnShareDividendsEdit.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Dividend",
                    _strLanguage);
                lblCost.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Costs", _strLanguage);
                btnShareCostsEdit.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Costs",
                    _strLanguage);

                // Load button images
                btnShareBuysEdit.Image = Resources.black_edit;
                btnShareSalesEdit.Image = Resources.black_edit;
                btnShareCostsEdit.Image = Resources.black_edit;
                btnShareDividendsEdit.Image = Resources.black_edit;

                #endregion GroupBox EarningsExpenditure

                btnSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Buttons/Save", _strLanguage);
                btnCancel.Text = _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Buttons/Cancel", _strLanguage);

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
                    _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/ShowFailed", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
            if (_stopFomClosing)
            {
                // Stop closing the form
                e.Cancel = true;
            }

            // Reset closing flag
            _stopFomClosing = false;

            // Set the dialog result correct of saving or not saving the share
            // Share must be saved because a dividend has been added or deleted
            if (_bSave)
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
                _stopFomClosing = true;
                var errorFlag = false;
                decimal volume = 0;
                decimal deposit = 0;

                statusStrip1.ForeColor = Color.Red;

                if (txtBoxName.Text == @"")
                {
                    txtBoxName.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameEmpty", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else
                {
                    // Check if an share with the given share name already exists
                    foreach (var shareObject in _parentWindow.ShareObjectList)
                    {
                        if (shareObject.Name == txtBoxName.Text && shareObject != _shareObject)
                        {
                            errorFlag = true;
                            _stopFomClosing = true;
                            txtBoxName.Focus();
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameExists", _strLanguage),
                                _xmlLanguage, _strLanguage,
                                Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                            break;
                        }

                    }
                }

                if (lblDepositValue.Text == @"" && errorFlag == false)
                {
                    lblDepositValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/DepositEmpty", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if(!Decimal.TryParse(lblDepositValue.Text, out deposit) && errorFlag == false)
                {
                    lblDepositValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/DepositWrongFormat", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (deposit <= 0 && errorFlag == false)
                {
                    lblDepositValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/DepositWrongValue", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (lblVolumeValue.Text == @"" && errorFlag == false)
                {
                    lblVolumeValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/VolumeEmpty", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (!Decimal.TryParse(lblVolumeValue.Text, out volume) && errorFlag == false)
                {
                    lblVolumeValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/VolumeWrongFormat", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (volume <= 0 && errorFlag == false)
                {
                    lblVolumeValue.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/VolumeWrongValue", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (txtBoxWebSite.Text == @"" && errorFlag == false)
                {
                    txtBoxWebSite.Focus();
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteEmpty", _strLanguage),
                        _xmlLanguage, _strLanguage,
                        Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }
                else if (errorFlag == false)
                {
                    // Check if an share with the given WKN number already exists
                    foreach (var shareObject in _parentWindow.ShareObjectList)
                    {
                        if (shareObject.WebSite == txtBoxWebSite.Text && shareObject != _shareObject)
                        {
                            errorFlag = true;
                            _stopFomClosing = true;
                            txtBoxWebSite.Focus();
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteExists", _strLanguage),
                                _xmlLanguage, _strLanguage,
                                Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                            break;
                        }

                    }
                }

                if (errorFlag == false)
                {
                    _stopFomClosing = false;
                    _bSave = true;

                    _shareObject.Wkn = lblWknValue.Text;
                    _shareObject.Name = txtBoxName.Text;
                    _shareObject.Volume = volume;
                    _shareObject.MarketValue = deposit;
                    _shareObject.WebSite = txtBoxWebSite.Text;

                    CultureInfo cultureInfo = new CultureInfo(cboBoxCultureInfo.GetItemText(cboBoxCultureInfo.SelectedItem));
                    _shareObject.CultureInfo = cultureInfo;

                    _shareObject.DividendPayoutInterval = cbxDividendPayoutInterval.SelectedIndex;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnSave_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                _stopFomClosing = true;
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/EditFormShare/Errors/EditSaveFailed", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
                _stopFomClosing = true;
            }
        }

        /// <summary>
        /// This function close the form
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            _stopFomClosing = false;
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
            IViewBuyEdit view = new ViewBuyEdit(_shareObject, _logger, _xmlLanguage, _strLanguage);
            PresenterBuyEdit presenterBuyEdit = new PresenterBuyEdit (view, model);

            DialogResult dlgResult = view.ShowDialog();
            if (dlgResult == DialogResult.OK)
                _bSave = true;
            else
                _bSave = false;

            lblVolumeValue.Text = string.Format("{0:N2}", _shareObject.Volume);
            lblDepositValue.Text = string.Format("{0:N2}", _shareObject.MarketValue);
            lblBuysValue.Text = string.Format("{0:N2}", _shareObject.AllBuyEntries.BuyMarketValueTotal);
            lblSalesValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleValueTotal);
            lblCostValue.Text = string.Format("{0:N2}", _shareObject.AllCostsEntries.CostValueTotal);
            lblProfitLossValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleProfitLossTotal);
            lblDividendValue.Text = string.Format("{0:N2}", _shareObject.AllDividendEntries.DividendValueTotalWithTaxes);
        }

        /// <summary>
        /// This function opens the sale add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareSalesEdit_Click(object sender, EventArgs e)
        {
            FrmShareSalesEdit shareSalesEdit = new FrmShareSalesEdit(_shareObject, _logger, _xmlLanguage, _strLanguage);

            DialogResult dlgResult = shareSalesEdit.ShowDialog();
            if (dlgResult == DialogResult.OK)
                _bSave = true;
            else
                _bSave = false;

            lblVolumeValue.Text = string.Format("{0:N2}", _shareObject.Volume);
            lblDepositValue.Text = string.Format("{0:N2}", _shareObject.MarketValue);
            lblBuysValue.Text = string.Format("{0:N2}", _shareObject.AllBuyEntries.BuyMarketValueTotal);
            lblSalesValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleValueTotal);
            lblCostValue.Text = string.Format("{0:N2}", _shareObject.AllCostsEntries.CostValueTotal);
            lblProfitLossValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleProfitLossTotal);
            lblDividendValue.Text = string.Format("{0:N2}", _shareObject.AllDividendEntries.DividendValueTotalWithTaxes);
        }

        /// <summary>
        /// This function opens the share dividend add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareDividendsEdit_Click(object sender, EventArgs e)
        {
            IModelDividendEdit model = new ModelDividendEdit();
            IViewDividendEdit view = new ViewDividendEdit(_shareObject, _logger, _xmlLanguage, _strLanguage);
            PresenterDividendEdit presenterDividendEdit = new PresenterDividendEdit(view, model);

            DialogResult dlgResult = view.ShowDialog();
            if (dlgResult == DialogResult.OK)
                _bSave = true;
            else
                _bSave = false;

            lblVolumeValue.Text = string.Format("{0:N2}", _shareObject.Volume);
            lblDepositValue.Text = string.Format("{0:N2}", _shareObject.MarketValue);
            lblBuysValue.Text = string.Format("{0:N2}", _shareObject.AllBuyEntries.BuyMarketValueTotal);
            lblSalesValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleValueTotal);
            lblCostValue.Text = string.Format("{0:N2}", _shareObject.AllCostsEntries.CostValueTotal);
            lblProfitLossValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleProfitLossTotal);
            lblDividendValue.Text = string.Format("{0:N2}", _shareObject.AllDividendEntries.DividendValueTotalWithTaxes);
        }

        /// <summary>
        /// This function opens the share cost add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareCostsEdit_Click(object sender, EventArgs e)
        {
            FrmShareCostEdit shareCostEdit = new FrmShareCostEdit(_shareObject, _logger, _xmlLanguage, _strLanguage);

            DialogResult dlgResult = shareCostEdit.ShowDialog();
            if (dlgResult == DialogResult.OK)
                _bSave = true;
            else
                _bSave = false;

            lblVolumeValue.Text = string.Format("{0:N2}", _shareObject.Volume);
            lblDepositValue.Text = string.Format("{0:N2}", _shareObject.MarketValue);
            lblBuysValue.Text = string.Format("{0:N2}", _shareObject.AllBuyEntries.BuyMarketValueTotal);
            lblSalesValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleValueTotal);
            lblCostValue.Text = string.Format("{0:N2}", _shareObject.AllCostsEntries.CostValueTotal);
            lblProfitLossValue.Text = string.Format("{0:N2}", _shareObject.AllSaleEntries.SaleProfitLossTotal);
            lblDividendValue.Text = string.Format("{0:N2}", _shareObject.AllDividendEntries.DividendValueTotalWithTaxes);
        }

        #endregion Button
    }
}
