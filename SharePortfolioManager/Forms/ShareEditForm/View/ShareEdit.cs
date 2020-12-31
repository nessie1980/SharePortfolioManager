//MIT License
//
//Copyright(c) 2020 nessie1980(nessie1980 @gmx.de)
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
//#define DEBUG_SHARE_EDIT

using LanguageHandler;
using Logging;
using SharePortfolioManager.BrokeragesForm.Model;
using SharePortfolioManager.BrokeragesForm.Presenter;
using SharePortfolioManager.BrokeragesForm.View;
using SharePortfolioManager.BuysForm.Model;
using SharePortfolioManager.BuysForm.Presenter;
using SharePortfolioManager.BuysForm.View;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.DividendForm.Model;
using SharePortfolioManager.DividendForm.Presenter;
using SharePortfolioManager.DividendForm.View;
using SharePortfolioManager.Properties;
using SharePortfolioManager.SalesForm.Model;
using SharePortfolioManager.SalesForm.Presenter;
using SharePortfolioManager.SalesForm.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace SharePortfolioManager
{
    public partial class FrmShareEdit : Form
    {
        #region Properties

        public FrmMain ParentWindow { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

        public string ShareWkn { get; set; }

        public bool Save { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; set; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; set; }

        public bool StopFomClosingFlag { get; set; }

        #endregion Properties

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmShareEdit(FrmMain parentWindow, Logger logger, Language xmlLanguage, string shareWkn, string language )
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

                    // Set values
                    lblWknValue.Text = ShareObjectFinalValue.Wkn;
                    dateTimeStockMarketLaunchDate.Value = DateTime.Parse(ShareObjectFinalValue.StockMarketLaunchDate);
                    lblDateValue.Text = ShareObjectFinalValue.AllBuyEntries.AllBuysOfTheShareDictionary.Values.First().BuyListYear.First().Date;
                    txtBoxName.Text = ShareObjectFinalValue.Name;

                    switch (ShareObjectFinalValue.InternetUpdateOption)
                    {
                        case ShareObject.ShareUpdateTypes.Both:
                        {
                            rdbBoth.Checked = true;
                        } break;
                        case ShareObject.ShareUpdateTypes.MarketPrice:
                        {
                            rdbMarketPrice.Checked = true;
                        } break;
                        case ShareObject.ShareUpdateTypes.DailyValues:
                        {
                            rdbDailyValues.Checked = true;
                        } break;
                        case ShareObject.ShareUpdateTypes.None:
                        {
                            rdbNone.Checked = true;
                        } break;
                        default:
                        {
                            rdbNone.Checked = true;
                        } break;
                    }

                    txtBoxWebSite.Text = ShareObjectFinalValue.UpdateWebSiteUrl;
                    txtBoxDailyValuesWebSite.Text = ShareObjectFinalValue.DailyValuesUpdateWebSiteUrl;

                    // Set units
                    lblPurchaseUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblVolumeUnit.Text = ShareObject.PieceUnit;
                    lblBuysUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblSalesUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblProfitLossUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblDividendUnit.Text = ShareObjectFinalValue.CurrencyUnit;
                    lblBrokerageUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                    #region Get culture info

                    var list = new List<string>();
                    foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                    {
                        try
                        {
                            // ReSharper disable once UnusedVariable
                            var specName = CultureInfo.CreateSpecificCulture(ci.Name).Name;
                        }
                        catch
                        {
                            // ignored
                        }

                        list.Add($"{ci.Name}");
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

                    SetShareValuesToTextBoxes();

                    #endregion GroupBox EarningsExpenditure

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentUICulture;
                }

                #endregion Fill in the values of the share object
                
                #region Language configuration

                #region GroupBox General

                Text = Language.GetLanguageTextByXPath(@"/EditFormShare/Caption", SettingsConfiguration.LanguageName);
                grpBoxGeneral.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Caption", SettingsConfiguration.LanguageName);
                lblWkn.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/WKN", SettingsConfiguration.LanguageName);
                lblStockMarketLaunchDate.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/StockMarketLaunchDate", SettingsConfiguration.LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Date", SettingsConfiguration.LanguageName);
                lblName.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Name", SettingsConfiguration.LanguageName);
                lblShareUpdate.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Update", SettingsConfiguration.LanguageName);

                rdbBoth.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Radio/Both", SettingsConfiguration.LanguageName);
                rdbMarketPrice.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Radio/MarketPrice", SettingsConfiguration.LanguageName);
                rdbDailyValues.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Radio/DailyValues", SettingsConfiguration.LanguageName);
                rdbNone.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Radio/None", SettingsConfiguration.LanguageName);

                lblPurchase.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Purchase", SettingsConfiguration.LanguageName);
                lblVolume.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/Volume", SettingsConfiguration.LanguageName);
                lblWebSite.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/WebSite", SettingsConfiguration.LanguageName);
                lblDailyValuesWebSite.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/DailyValuesWebSite", SettingsConfiguration.LanguageName);
                lblCultureInfo.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/CultureInfo", SettingsConfiguration.LanguageName);

                lblDividendPayoutInterval.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/PayoutInterval", SettingsConfiguration.LanguageName);
                // Add dividend payout interval values
                Language.GetLanguageTextListByXPath(@"/ComboBoxItemsPayout/*", SettingsConfiguration.LanguageName).ForEach(item => cbxDividendPayoutInterval.Items.Add(item));
                lblShareType.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxGeneral/Labels/ShareType", SettingsConfiguration.LanguageName);
                // Add share type values
                Language.GetLanguageTextListByXPath(@"/ComboBoxItemsShareType/*", SettingsConfiguration.LanguageName).ForEach(item => cbxShareType.Items.Add(item));

                // Select the payout interval for the dividend
                cbxDividendPayoutInterval.SelectedIndex = ShareObjectFinalValue.DividendPayoutInterval;
                // Select the type of the share
                cbxShareType.SelectedIndex = (int)ShareObjectFinalValue.ShareType;

                #endregion GroupBox General

                #region GroupBox EarningsExpenditure

                grpBoxEarningsExpenditure.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Caption", SettingsConfiguration.LanguageName);
                lblBuys.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Buys", SettingsConfiguration.LanguageName);
                btnShareBuysEdit.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Buys",
                    LanguageName);
                lblSales.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Sales",
                    LanguageName);
                btnShareSalesEdit.Text =
                    Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Sales",
                    LanguageName);

                if (ShareObjectFinalValue.AllSaleEntries.SaleProfitLossTotal < 0)
                {

                    lblProfitLoss.Text =
                        Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Loss",
                            LanguageName);
                }
                else
                {
                    lblProfitLoss.Text =
                        Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Profit",
                            LanguageName);
                }

                lblDividend.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Dividend",
                    LanguageName);
                btnShareDividendsEdit.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Dividend",
                    LanguageName);
                lblBrokerage.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Labels/Brokerage", SettingsConfiguration.LanguageName);
                btnShareBrokerageEdit.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/GrpBoxEarningsExpenditure/Buttons/Brokerage",
                    LanguageName);

                // Load button images
                btnShareBuysEdit.Image = Resources.button_pencil_16;
                btnShareSalesEdit.Image = Resources.button_pencil_16;
                btnShareBrokerageEdit.Image = Resources.button_pencil_16;
                btnShareDividendsEdit.Image = Resources.button_pencil_16;

                #endregion GroupBox EarningsExpenditure

                btnSave.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/Buttons/Save", SettingsConfiguration.LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/EditFormShare/Buttons/Cancel", SettingsConfiguration.LanguageName);

                // Load button images
                btnSave.Image = Resources.button_save_24;
                btnCancel.Image = Resources.button_back_24;

                #endregion Language configuration
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(editShareStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/ShowFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
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
            DialogResult = Save ? DialogResult.OK : DialogResult.Cancel;
        }

        /// <summary>
        /// This function replace the start date and the interval with {0} and {1}.
        /// This allows to replace it with other parameters
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDailyValuesWebSite_Leave(object sender, EventArgs e)
        {
            txtBoxDailyValuesWebSite.Text = Helper.RegexReplaceStartDateAndInterval(txtBoxDailyValuesWebSite.Text);
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

                statusStrip1.ForeColor = Color.Red;
                statusStrip1.Text = "";

                // Needed for the URL check
                var decodedUrlWebSite = txtBoxWebSite.Text;
                var decodeUrlDailyValuesWebSite = txtBoxDailyValuesWebSite.Text;
                // Set start date and interval for the test
                decodeUrlDailyValuesWebSite =
                    string.Format(decodeUrlDailyValuesWebSite, DateTime.Now.AddMonths(-1).ToShortDateString(), "M1");

                if (txtBoxName.Text == @"")
                {
                    txtBoxName.Focus();

                    Helper.AddStatusMessage(editShareStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameEmpty", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    
                    errorFlag = true;
                }
                else
                {
                    // Check if a market value share with the given share name already exists
                    foreach (var shareObjectMarketValue in ParentWindow.ShareObjectListMarketValue)
                    {
                        if (shareObjectMarketValue.Name != txtBoxName.Text ||
                            shareObjectMarketValue == ShareObjectMarketValue) continue;

                        errorFlag = true;
                        StopFomClosingFlag = true;
                        txtBoxName.Focus();

                        Helper.AddStatusMessage(editShareStatusLabelMessage,
                            Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameExists", SettingsConfiguration.LanguageName),
                            Language, SettingsConfiguration.LanguageName,
                            Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        
                        break;
                    }

                    if (errorFlag == false)
                    {
                        // Check if a market value share with the given share name already exists
                        foreach (var shareObjectFinalValue in ParentWindow.ShareObjectListFinalValue)
                        {
                            if (shareObjectFinalValue.Name != txtBoxName.Text ||
                                shareObjectFinalValue == ShareObjectFinalValue) continue;

                            errorFlag = true;
                            StopFomClosingFlag = true;
                            txtBoxName.Focus();

                            Helper.AddStatusMessage(editShareStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/NameExists", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                            
                            break;
                        }
                    }
                }

                // Stock market launch date
                // Check if the stock market launch date has been modified
                var dummy = new DateTimePicker
                {
                    MinDate = DateTime.MinValue
                };

                if (txtBoxDailyValuesWebSite.Text == dummy.MinDate.ToShortDateString() && errorFlag == false)
                {
                    txtBoxDailyValuesWebSite.Focus();

                    Helper.AddStatusMessage(editShareStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/StockMarketLaunchDateNotModified", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    
                    errorFlag = true;
                }

                // Update website
                if (txtBoxWebSite.Text == @"" && (rdbBoth.Checked || rdbMarketPrice.Checked) && errorFlag == false)
                {
                    txtBoxWebSite.Focus();

                    Helper.AddStatusMessage(editShareStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteEmpty", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    
                    errorFlag = true;
                }
                else if ((rdbBoth.Checked || rdbMarketPrice.Checked) && !Helper.UrlChecker(ref decodedUrlWebSite, 10000))
                {
                    txtBoxWebSite.Focus();
                    
                    Helper.AddStatusMessage(editShareStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteWrongFormat", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    
                    errorFlag = true;
                }
                else if (errorFlag == false)
                {
                    // Check if a market value share with the given website already exists
                    foreach (var shareObjectMarketValue in ParentWindow.ShareObjectListMarketValue)
                    {
                        if (txtBoxWebSite.Text == @"" ||
                            shareObjectMarketValue.UpdateWebSiteUrl != txtBoxWebSite.Text ||
                            shareObjectMarketValue == ShareObjectMarketValue) continue;

                        errorFlag = true;
                        StopFomClosingFlag = true;
                        txtBoxWebSite.Focus();

                        Helper.AddStatusMessage(editShareStatusLabelMessage,
                            Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteExists", SettingsConfiguration.LanguageName),
                            Language, SettingsConfiguration.LanguageName,
                            Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        
                        break;
                    }

                    if (errorFlag == false)
                    {
                        // Check if a final value share with the given website already exists
                        foreach (var shareObjectFinalValue in ParentWindow.ShareObjectListFinalValue)
                        {
                            if (txtBoxWebSite.Text == @"" ||
                                shareObjectFinalValue.UpdateWebSiteUrl != txtBoxWebSite.Text ||
                                shareObjectFinalValue == ShareObjectFinalValue) continue;

                            errorFlag = true;
                            StopFomClosingFlag = true;
                            txtBoxWebSite.Focus();
                        
                            Helper.AddStatusMessage(editShareStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/WebSiteExists", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                            
                            break;
                        }
                    }
                }
                
                // Daily values update website
                if (txtBoxDailyValuesWebSite.Text == @"" && (rdbBoth.Checked || rdbDailyValues.Checked) && errorFlag == false)
                {
                    txtBoxDailyValuesWebSite.Focus();

                    Helper.AddStatusMessage(editShareStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/DailyValuesWebSiteEmpty", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    
                    errorFlag = true;
                }
                else if ((rdbBoth.Checked || rdbDailyValues.Checked) && !Helper.UrlChecker(ref decodeUrlDailyValuesWebSite, 10000))
                {
                    txtBoxDailyValuesWebSite.Focus();
                    
                    Helper.AddStatusMessage(editShareStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/DailyValuesWebSiteWrongFormat", SettingsConfiguration.LanguageName),
                        Language, SettingsConfiguration.LanguageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    
                    errorFlag = true;
                }
                else if (errorFlag == false)
                {
                    // Check if a market value share with the given daily values website already exists
                    foreach (var shareObjectMarketValue in ParentWindow.ShareObjectListMarketValue)
                    {
                        if (txtBoxWebSite.Text == @"" ||
                            shareObjectMarketValue.DailyValuesUpdateWebSiteUrl != txtBoxDailyValuesWebSite.Text ||
                            shareObjectMarketValue == ShareObjectMarketValue) continue;

                        errorFlag = true;
                        StopFomClosingFlag = true;
                        txtBoxDailyValuesWebSite.Focus();
                    
                        Helper.AddStatusMessage(editShareStatusLabelMessage,
                            Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/DailyValuesWebSiteExists", SettingsConfiguration.LanguageName),
                            Language, SettingsConfiguration.LanguageName,
                            Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        
                        break;
                    }

                    if (errorFlag == false)
                    {
                        // Check if a final value share with the given daily values website already exists
                        foreach (var shareObjectFinalValue in ParentWindow.ShareObjectListFinalValue)
                        {
                            if (txtBoxWebSite.Text == @"" ||
                                shareObjectFinalValue.DailyValuesUpdateWebSiteUrl != txtBoxDailyValuesWebSite.Text ||
                                shareObjectFinalValue == ShareObjectFinalValue) continue;

                            errorFlag = true;
                            StopFomClosingFlag = true;
                            txtBoxDailyValuesWebSite.Focus();
                        
                            Helper.AddStatusMessage(editShareStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/DailyValuesWebSiteExists", SettingsConfiguration.LanguageName),
                                Language, SettingsConfiguration.LanguageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                            
                            break;
                        }
                    }
                }

                if (errorFlag) return;

                txtBoxWebSite.Text = decodedUrlWebSite;
                txtBoxDailyValuesWebSite.Text = Helper.RegexReplaceStartDateAndInterval(decodeUrlDailyValuesWebSite);

                StopFomClosingFlag = false; 
                Save = true;

                var cultureInfo = new CultureInfo(cboBoxCultureInfo.GetItemText(cboBoxCultureInfo.SelectedItem));

                // Market value share
                ShareObjectMarketValue.Name = txtBoxName.Text;
                ShareObjectMarketValue.StockMarketLaunchDate = dateTimeStockMarketLaunchDate.Value.ToShortDateString();

                if (rdbBoth.Checked)
                    ShareObjectMarketValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.Both;

                if (rdbMarketPrice.Checked)
                    ShareObjectMarketValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.MarketPrice;
                
                if (rdbDailyValues.Checked)
                    ShareObjectMarketValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.DailyValues;

                if (rdbNone.Checked)
                    ShareObjectMarketValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.None;

                ShareObjectMarketValue.UpdateWebSiteUrl = txtBoxWebSite.Text;
                ShareObjectMarketValue.DailyValuesUpdateWebSiteUrl = txtBoxDailyValuesWebSite.Text;
                ShareObjectMarketValue.CultureInfo = cultureInfo;
                ShareObjectMarketValue.ShareType = (ShareObject.ShareTypes)cbxShareType.SelectedIndex;

                // Final value share
                ShareObjectFinalValue.Name = txtBoxName.Text;
                ShareObjectFinalValue.StockMarketLaunchDate = dateTimeStockMarketLaunchDate.Value.ToShortDateString();

                if (rdbBoth.Checked)
                    ShareObjectFinalValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.Both;

                if (rdbMarketPrice.Checked)
                    ShareObjectFinalValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.MarketPrice;

                if (rdbDailyValues.Checked)
                    ShareObjectFinalValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.DailyValues;

                if (rdbNone.Checked)
                    ShareObjectFinalValue.InternetUpdateOption = ShareObject.ShareUpdateTypes.None;

                ShareObjectFinalValue.UpdateWebSiteUrl = txtBoxWebSite.Text;
                ShareObjectFinalValue.DailyValuesUpdateWebSiteUrl = txtBoxDailyValuesWebSite.Text;
                ShareObjectFinalValue.CultureInfo = cultureInfo;
                ShareObjectFinalValue.DividendPayoutInterval = cbxDividendPayoutInterval.SelectedIndex;
                ShareObjectFinalValue.ShareType = (ShareObject.ShareTypes)cbxShareType.SelectedIndex;
            }
            catch (Exception ex)
            {
                StopFomClosingFlag = true;

                Helper.AddStatusMessage(editShareStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/EditFormShare/Errors/EditSaveFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
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
            IViewBuyEdit view = new ViewBuyEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, SettingsConfiguration.LanguageName);
            // ReSharper disable once UnusedVariable
            var presenterBuyEdit = new PresenterBuyEdit (view, model);

            var dlgResult = view.ShowDialog();
            Save = dlgResult == DialogResult.OK;

            SetShareValuesToTextBoxes();
        }

        /// <summary>
        /// This function opens the sale add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareSalesEdit_Click(object sender, EventArgs e)
        {
            IModelSaleEdit model = new ModelSaleEdit();
            IViewSaleEdit view = new ViewSaleEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, SettingsConfiguration.LanguageName);
            // ReSharper disable once UnusedVariable
            var presenterSaleEdit = new PresenterSaleEdit(view, model);

            var dlgResult = view.ShowDialog();
            Save = dlgResult == DialogResult.OK;

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
            IViewDividendEdit view = new ViewDividendEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, SettingsConfiguration.LanguageName);
            // ReSharper disable once UnusedVariable
            var presenterDividendEdit = new PresenterDividendEdit(view, model);

            var dlgResult = view.ShowDialog();
            Save = dlgResult == DialogResult.OK;

            SetShareValuesToTextBoxes();
        }

        /// <summary>
        /// This function opens the share brokerage add / edit dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareBrokerageEdit_Click(object sender, EventArgs e)
        {
            IModelBrokerageEdit model = new ModelBrokerageEdit();
            IViewBrokerageEdit view = new ViewBrokerageEdit(ShareObjectMarketValue, ShareObjectFinalValue, Logger, Language, SettingsConfiguration.LanguageName);
            // ReSharper disable once UnusedVariable
            var presenterBrokerageEdit = new PresenterBrokerageEdit(view, model);

            var dlgResult = view.ShowDialog();
            Save = dlgResult == DialogResult.OK;

            SetShareValuesToTextBoxes();
        }

        /// <summary>
        /// This function sets the values to the corresponding text boxes
        /// </summary>
        private void SetShareValuesToTextBoxes()
        {
            lblVolumeValue.Text = ShareObjectFinalValue.VolumeAsStr;
            lblPurchaseValue.Text = ShareObjectFinalValue.PurchaseValueAsStr;

            lblBuysValue.Text = ShareObjectFinalValue.BuyValueBrokerageReductionAsStr;
            lblSalesValue.Text = ShareObjectFinalValue.SalePayoutBrokerageReductionAsStr;
            lblProfitLossValue.Text = ShareObjectFinalValue.SaleProfitLossBrokerageReductionAsStr;
            lblDividendValue.Text = ShareObjectFinalValue.AllDividendEntries.DividendValueTotalWithTaxesAsStr;
            lblBrokerageValue.Text = ShareObjectFinalValue.CompleteBrokerageValueAsStr;
        }

        #endregion Button
    }
}
