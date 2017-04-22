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
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Properties;
using System.Linq;
using System.IO;
using SharePortfolioManager.Classes.Taxes;

namespace SharePortfolioManager.Forms.SalesForm
{
    public partial class FrmShareSalesEdit : Form
    {
        #region Variables

        #region Transfer parameter

        /// <summary>
        /// Stores the choosen share object
        /// </summary>
        ShareObject _shareObject = null;

        /// <summary>
        /// Stores the logger
        /// </summary>
        Logger _logger;

        /// <summary>
        /// Stores the given language file
        /// </summary>
        Language _xmlLanguage;

        /// <summary>
        /// Stores the given language
        /// </summary>
        string _languageName;

        #endregion Transfer paramter

        #region Flags

        /// <summary>
        /// Stores if the share object sales values should be taken or not
        /// </summary>
        bool _bLoadShareObjectSalesTaxValues;

        /// <summary>
        /// Stores if a sale has been deleted or added
        /// and so a save must be done in the lower dialog
        /// </summary>
        bool _bSaveFlag;

        /// <summary>
        /// Stores if a dividend should be load from the portfolio
        /// </summary>
        private bool _bLoadGridSelectionFlag;

        /// <summary>
        /// Stores the flag if the selection of a gridview has been changed
        /// </summary>
        bool _bSelectionGridViewChanged = false;

        #endregion Flags

        /// <summary>
        /// Stores the DataGridView of the selected row
        /// </summary>
        private DataGridView _selectedDataGridView = null;

        /// <summary>
        /// Stores the current selected sale object
        /// </summary>
        private SaleObject _currentSelectedSaleObject;

        #region Taxes

        /// <summary>
        /// Stores the values for the current tax values
        /// </summary>
        Taxes _taxValusCurrent;

        /// <summary>
        /// Stores the values for the usual tax values
        /// </summary>
        Taxes _taxValuesNormal;

        /// <summary>
        /// Stores the values for the edit tax values
        /// </summary>
        Taxes _taxValueDividend;

        #endregion Taxes


        #region Input values

        /// <summary>
        /// Stores if a foreign should be entered
        /// </summary>
        bool _bForeignFlag = false;

        /// <summary>
        /// Stores the exchange ratio value for the foreign currency
        /// </summary>
        decimal _decExchangeRatio = 1;

        /// <summary>
        /// Stores the volume of the shares
        /// </summary>
        decimal _decVolume = 0;

        /// <summary>
        /// Stores the buy price of the share
        /// </summary>
        decimal _decBuyPrice = 0;

        /// <summary>
        /// Stores the sales price of one share
        /// </summary>
        decimal _decSalePrice = 0;
        decimal _decSalePriceFC = 0;

        /// <summary>
        /// Stores the loss balance
        /// </summary>
        decimal _decLossBalance = 0;
        decimal _decLossBalanceFC = 0;

        /// <summary>
        /// Stores the sale payout
        /// </summary>
        decimal _decSalePayout = 0;
        decimal _decSalePayoutFC = 0;

        /// <summary>
        /// Stores the dividend tax
        /// </summary>
        decimal _decTax = 0;
        decimal _decTaxFC = 0;

        /// <summary>
        /// Stores the loss or profit of the sale
        /// </summary>
        decimal _decLossProfit = 0;

        /// <summary>
        /// Stores the loss or profit value as percentage value
        /// </summary>
        decimal _decLossProfitPercentage = 0;

        /// <summary>
        /// Stores the document for the sale
        /// </summary>
        string _strDocument = @"";

        #endregion Input values

        #endregion Variables

        #region Properties

        public ShareObject ShareObject
        {
            get { return _shareObject; }
            internal set { _shareObject = value; }
        }

        public Logger Logger
        {
            get { return _logger; }
            internal set { _logger = value; }
        }

        public Language XmlLanguage
        {
            get { return _xmlLanguage; }
            internal set { _xmlLanguage = value; }
        }

        public string LanguageName
        {
            get { return _languageName; }
            internal set { _languageName = value; }
        }

        public bool SaveFlag
        {
            get { return _bSaveFlag; }
            internal set { _bSaveFlag = value; }
        }

        public bool LoadGridSelectionFlag
        {
            get { return _bLoadGridSelectionFlag; }
            internal set { _bLoadGridSelectionFlag = value; }
        }

        public DataGridView SelectedDataGridView
        {
            get { return _selectedDataGridView; }
            internal set { _selectedDataGridView = value; }
        }

        public Taxes TaxValuesCurrent
        {
            get { return _taxValusCurrent; }
            set { _taxValusCurrent = value; }
        }

        public Taxes TaxValuesNormal
        {
            get { return _taxValuesNormal; }
            internal set { _taxValuesNormal = value; }
        }

        public Taxes TaxValuesEditDividend
        {
            get { return _taxValueDividend; }
            set { _taxValueDividend = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObject">Current chosen share object</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public FrmShareSalesEdit(ShareObject shareObject, Logger logger, Language xmlLanguage, String language)
        {
            InitializeComponent();

            _shareObject = shareObject;
            _logger = logger;
            _xmlLanguage = xmlLanguage;
            _languageName = language;
            _bSaveFlag = false;
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
                _bLoadShareObjectSalesTaxValues = false;

                // Set tax values
                if (TaxValuesNormal == null)
                    TaxValuesNormal = new Taxes();

                if (TaxValuesCurrent == null)
                    TaxValuesCurrent = new Taxes();

                TaxValuesNormal.TaxAtSourceFlag = ShareObject.TaxTaxAtSourceFlag;
                TaxValuesNormal.TaxAtSourcePercentage = ShareObject.TaxTaxAtSourcePercentage;
                TaxValuesNormal.CapitalGainsTaxFlag = ShareObject.TaxCapitalGainsFlag;
                TaxValuesNormal.CapitalGainsTaxPercentage = ShareObject.TaxCapitalGainsPercentage;
                TaxValuesNormal.SolidarityTaxFlag = ShareObject.TaxSolidarityFlag;
                TaxValuesNormal.SolidarityTaxPercentage = ShareObject.TaxSolidarityPercentage;
                TaxValuesNormal.CiShareCurrency = ShareObject.CultureInfo;

                TaxValuesCurrent.DeepCopy(TaxValuesNormal);

                Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Caption", _languageName);
                grpBoxAddSales.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", _languageName);
                grpBoxSales.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/Caption",
                        _languageName);
                lblAddSaleDate.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Date",
                    _languageName);
                lblAddSaleEnableFC.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/ForeignCurrencyActivation",
                    _languageName);
                lblAddSaleExchangeRatioFC.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/ForeignCurrencyFactor",
                    _languageName);
                lblSalesVolume.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Volume",
                        _languageName);
                lblSalesBuyPrice.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/BuyPrice",
                        _languageName);
                lblSalesPrice.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SalesPrice",
                        _languageName);
                lblSalesLossBalance.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SalesLossBalance",
                        _languageName);
                lblSalesSum.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SalesSum",
                        _languageName);
                lblSalesTax.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/SalesSumAfterTax",
                        _languageName);
                lblSalesCosts.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Costs",
                        _languageName);
                lblSalesDocument.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Labels/Document",
                        _languageName);
                btnSalesAddSave.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add",
                    _languageName);
                btnSalesReset.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Reset",
                    _languageName);
                btnSalesDelete.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Delete",
                        _languageName);
                btnSalesCancel.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Cancel",
                        _languageName);

                // Load button images
                btnSalesAddSave.Image = Resources.black_save;
                btnSalesDelete.Image = Resources.black_delete;
                btnSalesReset.Image = Resources.black_cancel;
                btnSalesCancel.Image = Resources.black_cancel;

                // Set sale units to the edit boxes
                lblAddSalesVolumeUnit.Text = ShareObject.PieceUnit;
                lblAddSalesBuyPriceUnit.Text = _shareObject.CurrencyUnit;
                lblAddSalesPriceUnit.Text = _shareObject.CurrencyUnit;
                lblAddSalesLossBalanceUnit.Text = _shareObject.CurrencyUnit;
                lblAddSalesSumUnit.Text = _shareObject.CurrencyUnit;
                lblAddSalesTaxUnit.Text = _shareObject.CurrencyUnit;
                lblAddSalesCostsFCUnit.Text = _shareObject.CurrencyUnit;

                // Set current share price
                txtBoxAddSalesSalesPrice.Text = _shareObject.CurPriceAsStr;

                // Set current share value
                txtBoxAddSalesVolume.Text = _shareObject.VolumeAsStr;

                // Set currency to combobox
                foreach (var temp in Helper.ListNameUnitCurrency)
                {
                    cbxBoxAddSalesFC.Items.Add(string.Format("{0} - {1}", temp.Key, temp.Value));
                }

                CheckForeignCurrencyCalulationShouldBeDone();

                // Chose USD item
                int iIndex = cbxBoxAddSalesFC.FindString("USD");
                cbxBoxAddSalesFC.SelectedIndex = iIndex;

                // Set currency units
                TaxValuesCurrent.CurrencyUnit = lblAddSalesBuyPriceUnit.Text;
                TaxValuesCurrent.FCUnit = cbxBoxAddSalesFC.SelectedItem.ToString().Split('-')[1].Trim();
                TaxValuesCurrent.CiShareFC = Helper.GetCultureByISOCurrencySymbol(cbxBoxAddSalesFC.SelectedItem.ToString().Split('-')[0].Trim());

                ShowSales();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShareSalesEdit_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function set the focus on the volume edit box
        /// when the form is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmShareSalesEdit_Shown(object sender, EventArgs e)
        {
            txtBoxAddSalesVolume.Focus();
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a buy has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareSalesEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_bSaveFlag)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }

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
                        DataGridView view = control as DataGridView;
                        if (view != null)
                        {
                            view.SelectionChanged -= dataGridViewSalesOfAYear_SelectionChanged;
                            view.SelectionChanged -= dataGridViewSalesOfYears_SelectionChanged;
                            view.DataBindingComplete -= dataGridViewSalesOfAYear_DataBindingComplete;
                        }
                    }
                    tabPage.Controls.Clear();
                    tabCtrlSales.TabPages.Remove(tabPage);
                }

                tabCtrlSales.TabPages.Clear();

                // Create TabPage for the sales of the years
                TabPage newTabPageOverviewYears = new TabPage();
                // Set TabPage name
                newTabPageOverviewYears.Name =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview",
                        _languageName);
                newTabPageOverviewYears.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", _languageName)
                                         + @" ("
                                         + _shareObject.AllSaleEntries.SaleValueTotalWithUnitAsString
                                         + @" / "
                                         + _shareObject.AllSaleEntries.SaleProfitLossTotalWithUnitAsString
                                         + @")";

                // Create Binding source for the sale data
                BindingSource bindingSourceOverview = new BindingSource();
                if (_shareObject.AllSaleEntries.GetAllSalesTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        _shareObject.AllSaleEntries.GetAllSalesTotalValues();

                // Create DataGridView
                DataGridView dataGridViewSalesOverviewOfAYears = new DataGridView();
                dataGridViewSalesOverviewOfAYears.Dock = DockStyle.Fill;
                // Bind source with buy values to the DataGridView
                dataGridViewSalesOverviewOfAYears.DataSource = bindingSourceOverview;
                // Set the delegate for the DataBindingComplete event
                dataGridViewSalesOverviewOfAYears.DataBindingComplete += dataGridViewSalesOfAYear_DataBindingComplete;

                // Set row select event
                dataGridViewSalesOverviewOfAYears.SelectionChanged += dataGridViewSalesOfYears_SelectionChanged;

                // Advanced configuration DataGridView buys
                DataGridViewCellStyle styleOverviewOfYears = dataGridViewSalesOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

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

                newTabPageOverviewYears.Controls.Add(dataGridViewSalesOverviewOfAYears);
                dataGridViewSalesOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlSales.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlSales;


                // Check if sales exists
                if (_shareObject.AllSaleEntries.AllSalesOfTheShareDictionary.Count > 0)
                {
                    // Loop through the years of the sales
                    foreach (
                        var keyName in _shareObject.AllSaleEntries.AllSalesOfTheShareDictionary.Keys.Reverse()
                        )
                    {
                        // Create TabPage
                        TabPage newTabPage = new TabPage();
                        // Set TabPage name
                        newTabPage.Name = keyName;
                        newTabPage.Text = keyName
                                            + @" ("
                                            + _shareObject.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                              .SaleValueYearWithUnitAsString
                                            + @" / "
                                            + _shareObject.AllSaleEntries.AllSalesOfTheShareDictionary[keyName]
                                              .SaleProfitLossYearWithUnitAsString
                                            + @")";

                        // Create Binding source for the sale data
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource =
                            _shareObject.AllSaleEntries.AllSalesOfTheShareDictionary[keyName].SaleListYear;

                        // Create DataGridView
                        DataGridView dataGridViewSalesOfAYear = new DataGridView();
                        dataGridViewSalesOfAYear.Dock = DockStyle.Fill;
                        // Bind source with buy values to the DataGridView
                        dataGridViewSalesOfAYear.DataSource = bindingSource;
                        // Set the delegate for the DataBindingComplete event
                        dataGridViewSalesOfAYear.DataBindingComplete += dataGridViewSalesOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewSalesOfAYear.SelectionChanged += dataGridViewSalesOfAYear_SelectionChanged;
                        // Set cell decimal click event
                        dataGridViewSalesOfAYear.CellContentDoubleClick += dataGridViewSalesOfAYear_CellContentdecimalClick;

                        // Advanced configuration DataGridView buys
                        DataGridViewCellStyle style = dataGridViewSalesOfAYear.ColumnHeadersDefaultCellStyle;
                        style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

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

                        newTabPage.Controls.Add(dataGridViewSalesOfAYear);
                        dataGridViewSalesOfAYear.Parent = newTabPage;
                        tabCtrlSales.Controls.Add(newTabPage);
                        newTabPage.Parent = tabCtrlSales;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShowSales()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ShowFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the sale document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void dataGridViewSalesOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
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
                            // Get doc from the sale with the strDateTime
                            foreach (var temp in _shareObject.AllSaleEntries.GetAllSalesOfTheShare())
                            {
                                // Check if the sale date and time is the same as the date and time of the clicked sale item
                                if (temp.SaleDate == strDateTime)
                                {
                                    // Check if the file still exists
                                    if (File.Exists(temp.SaleDocument))
                                        // Open the file
                                        Process.Start(temp.SaleDocument);
                                    else
                                    {
                                        string strCaption =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                                _languageName);
                                        string strMessage =
                                            _xmlLanguage.GetLanguageTextByXPath(
                                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                                _languageName);
                                        string strOk =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                                _languageName);
                                        string strCancel =
                                            _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                                _languageName);

                                        OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                                            strCancel);
                                        if (messageBox.ShowDialog() == DialogResult.OK)
                                        {
                                            // Remove sale object and add it with no document
                                            if (_shareObject.RemoveSale(temp.SaleDate) &&
                                                _shareObject.AddSale(false, strDateTime, temp.SaleVolume, temp.SaleValue, temp.SaleProfitLoss, 0))
                                            {
                                                // Set flag to save the share object.
                                                _bSaveFlag = true;

                                                ResetValues();
                                                ShowSales();

                                                // Add status message
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    _xmlLanguage.GetLanguageTextByXPath(
                                                        @"/AddEditFormSale/StateMessages/EditSuccess", _languageName),
                                                    _xmlLanguage, _languageName,
                                                    Color.Black, _logger, (int)FrmMain.EStateLevels.Info,
                                                    (int)FrmMain.EComponentLevels.Application);
                                            }
                                            else
                                            {
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    _xmlLanguage.GetLanguageTextByXPath(
                                                        @"/AddEditFormSale/Errors/EditFailed", _languageName),
                                                    _xmlLanguage, _languageName,
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
                MessageBox.Show("dataGridViewSalesOfAYear_CellContentdecimalClick()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DocumentShowFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewSalesOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                // Set selection changed flag
                _bSelectionGridViewChanged = true;
                
                if (tabCtrlSales.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlSales.SelectedTab.Controls.Contains((DataGridView)sender))
                        DeselectRowsOfDataGridViews((DataGridView)sender);
                }

                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    // Get SaleObject of the selected DataGridView row
                    _currentSelectedSaleObject = _shareObject.AllSaleEntries.GetSaleObjectByDateTime(curItem[0].Cells[0].Value.ToString());
                    if (_currentSelectedSaleObject != null)
                    {
                        datePickerAddSaleDate.Value = Convert.ToDateTime(_currentSelectedSaleObject.SaleDate);
                        datePickerAddSaleTime.Value = Convert.ToDateTime(_currentSelectedSaleObject.SaleDate);
                        txtBoxAddSalesCosts.Text = _currentSelectedSaleObject.SaleValueAsString;
                        txtBoxAddSalesSalesPrice.Text = _currentSelectedSaleObject.SalePriceAsString;
                        txtBoxAddSalesVolume.Text = _currentSelectedSaleObject.SaleVolumeAsString;
                        txtBoxAddSalesDocument.Text = _currentSelectedSaleObject.SaleDocument;
                    }
                    else
                    {
                        datePickerAddSaleDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        datePickerAddSaleTime.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        txtBoxAddSalesCosts.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxAddSalesSalesPrice.Text = curItem[0].Cells[2].Value.ToString();
                        txtBoxAddSalesVolume.Text = curItem[0].Cells[4].Value.ToString();
                        txtBoxAddSalesDocument.Text = curItem[0].Cells[5].Value.ToString();
                    }

                    if (_shareObject.AllBuyEntries.IsDateLastDate(curItem[0].Cells[0].Value.ToString()) &&
                        _shareObject.AllSaleEntries.IsDateLastDate(curItem[0].Cells[0].Value.ToString()))
                    {
                        // Enable button(s)
                        btnSalesDelete.Enabled = true;
                        // Enable text boxe(s)
                        txtBoxAddSalesSalesPrice.Enabled = true;
                        txtBoxAddSalesCosts.Enabled = true;
                        txtBoxAddSalesVolume.Enabled = true;
                    }
                    else
                    {
                        // Disable button(s)
                        btnSalesDelete.Enabled = false;
                        // Disable text boxe(s)
                        txtBoxAddSalesSalesPrice.Enabled = false;
                        txtBoxAddSalesCosts.Enabled = false;
                        txtBoxAddSalesVolume.Enabled = false;
                    }

                    // Enable button(s)
                    btnSalesReset.Enabled = true;

                    // Rename button
                    btnSalesAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Save", _languageName);
                    btnSalesAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAddSales.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Edit_Caption", _languageName);

                    // Store DataGridView instance
                    _selectedDataGridView = (DataGridView)sender;
                }
                else
                {
                    // Rename button
                    btnSalesAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", _languageName);
                    btnSalesAddSave.Image = Resources.black_add;
                    // Disable button(s)
                    btnSalesReset.Enabled = false;
                    btnSalesDelete.Enabled = false;
                    // Enable text boxe(s)
                    txtBoxAddSalesSalesPrice.Enabled = true;
                    txtBoxAddSalesCosts.Enabled = true;
                    txtBoxAddSalesVolume.Enabled = true;

                    // Rename group box
                    grpBoxAddSales.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", _languageName);

                    // Reset stored DataGridView instance
                    _selectedDataGridView = null;
                }

                // Set selection changed flag
                _bSelectionGridViewChanged = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("dataGridViewSalesOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                // Set selection changed flag
                _bSelectionGridViewChanged = false;

                // Reset selected sale object
                _currentSelectedSaleObject = null;
            }
        }

        /// <summary>
        /// This functions selects the tab page of the choosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridViewSalesOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlSales.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlSales.SelectTab(tabPage);
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
                MessageBox.Show("dataGridViewSalesOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/SelectionChangeFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the databinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void dataGridViewSalesOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Date",
                                    _languageName);
                            break;
                        case 1:
                            ((DataGridView) sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(
                                    @"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Value",
                                    _languageName) + @" (" + _shareObject.CurrencyUnit + ")";
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Price",
                                    _languageName) + @" (" + _shareObject.CurrencyUnit + ")";
                            break;
                        case 3:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_ProfitLoss",
                                    _languageName) + @" (" + _shareObject.CurrencyUnit + ")";
                            break;
                        case 4:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Volume",
                                    _languageName) + ShareObject.PieceUnit;
                            break;
                        case 5:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxSale/TabCtrl/DgvSaleOverview/ColHeader_Document",
                                    _languageName);
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
                MessageBox.Show("dataGridViewSalesOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/RenameColHeaderFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                foreach (TabPage TabPage in tabCtrlSales.TabPages)
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
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeselectFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

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
                bool errorFlag = false;
                string strDateTime;
                decimal decVolume;
                decimal decValue;
                decimal decProfitLoss;
                string strDoc;

                if (btnSalesAddSave.Text == _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", _languageName))
                {
                    // Check the input values
                    errorFlag = CheckSaleInput(true, out strDateTime, out decVolume, _shareObject.Volume, out decValue, out decProfitLoss, out strDoc);

                    // If no error occured add the new sale to the share
                    if (errorFlag == false)
                    {
                        if (_shareObject.AddSale(true, strDateTime, decVolume, decValue, decProfitLoss, _shareObject.Volume, strDoc))
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/AddSuccess", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Black, _logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);

                            // Set flag to save the share object.
                            _bSaveFlag = true;

                            // Reset values
                            ResetValues();

                            // Refresh the sale list
                            ShowSales();
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Red, _logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
                        }
                    }
                }
                else
                {
                    // Get current sale object
                    SaleObject currentslSaleObject = _shareObject.AllSaleEntries.GetSaleObjectByDateTime(_selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString());
                    // Check the input values
                    errorFlag = CheckSaleInput(false, out strDateTime, out decVolume, _shareObject.Volume + currentslSaleObject.SaleVolume, out decValue, out decProfitLoss, out strDoc);

                    // If no error occured add the new sale to the share
                    if (errorFlag == false)
                    {
                        if (_shareObject.RemoveSale(_selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString()) && _shareObject.AddSale(true, strDateTime, decVolume, decValue, decProfitLoss, _shareObject.Volume, strDoc))
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/EditSuccess", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Black, _logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);

                            // Set flag to save the share object.
                            _bSaveFlag = true;

                            // Reset values
                            ResetValues();

                            // Enable button(s)
                            btnSalesAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", _languageName);
                            btnSalesAddSave.Image = Resources.black_add;
                            // Disable button(s)
                            btnSalesReset.Enabled = false;
                            btnSalesDelete.Enabled = false;

                            // Rename group box
                            grpBoxAddSales.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", _languageName);

                            // Refresh the sale list
                            ShowSales();
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/EditFailed", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        }
                    }
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
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/AddFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he realy wants to delete the sale.
        /// If the user press "Ok" the sale will be deleted and the
        /// list of the sales will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">Event args</param>
        void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                string strCaption = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", _languageName);
                string strMessage = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Content/SaleDelete",
                    _languageName);
                string strOk = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", _languageName);
                string strCancel = _xmlLanguage.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", _languageName);

                OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                DialogResult dlgResult = messageBox.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    // Set flag to save the share object.
                    _bSaveFlag = true;

                    // Check if a row is selected
                    if (_selectedDataGridView != null && _selectedDataGridView.SelectedRows.Count == 1)
                    {
                        if (
                            !_shareObject.RemoveSale(_selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString()))
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/StateMessages/DeleteSuccess", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Black, _logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
                        }
                    }

                    // Reset values
                    ResetValues();

                    // Enable button(s)
                    btnSalesAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", _languageName);
                    btnSalesAddSave.Image = Resources.black_add;

                    // Disable button(s)
                    btnSalesReset.Enabled = false;
                    btnSalesDelete.Enabled = false;

                    // Rename group box
                    grpBoxAddSales.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", _languageName);

                    // Refresh the sale list
                    ShowSales();
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
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DeleteFailed", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                toolStripStatusLabelMessage.Text = @"";

                // Enable button(s)
                btnSalesAddSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Buttons/Add", _languageName);
                btnSalesAddSave.Image = Resources.black_add;

                // Disable button(s)
                btnSalesReset.Enabled = false;
                btnSalesDelete.Enabled = false;

                // Rename group box
                grpBoxAddSales.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/Add_Caption", _languageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                _selectedDataGridView = null;

                // Select overview tab
                if (
                    tabCtrlSales.TabPages.ContainsKey(
                        _xmlLanguage.GetLanguageTextByXPath(
                            @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", _languageName)))
                    tabCtrlSales.SelectTab(
                        _xmlLanguage.GetLanguageTextByXPath(
                            @"/AddEditFormSale/GrpBoxSale/TabCtrl/TabPgOverview/Overview", _languageName));
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnReset_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CancelFailure", _languageName),
                   _xmlLanguage, _languageName,
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
        /// This function calls the function
        /// - CaluclatePayoutFromForeignCurrencyPrice
        /// - CaluclatePriceFromForeignCurrencyPrice
        /// - CalculateDividendPayOutForeignCurrency if a foreign currency should be entered
        /// if the text of the exchange ratio has been changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddSalesExchangeRatioFC_TextChanged(object sender, EventArgs e)
        {
            if (_bForeignFlag)
            {
                Decimal.TryParse(((TextBox)sender).Text, out _decExchangeRatio);

                TaxValuesCurrent.ExchangeRatio = _decExchangeRatio;
            }

            // Calculate the original payout
            //if (_bForeignFlag)
            //    txtBoxAddSalesSum.Text = CaluclateSalePayoutFromForeignCurrency();

            // Calculate the original price
            if (_bForeignFlag)
                txtBoxAddSalesSalesPrice.Text = CaluclateSalePriceFromForeignCurrencyPrice();

            //// Calculate payout foreign currency of the share
            //if (_bForeignFlag || LoadGridSelectionFlag)
            //    txtBoxAddSalesSumFC.Text = CalculateSalePayOutForeignCurrency();
        }

        /// <summary>
        /// This function calls the CalculateValue function if
        /// the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddVolume_TextChanged(object sender, EventArgs e)
        {
            Decimal.TryParse(txtBoxAddSalesVolume.Text, out _decVolume);

            // Calculate the sales value
            txtBoxAddSalesSum.Text = CalculateSalePayoutValue();

            // Check the gridview selection has changed or not
            if (!_bSelectionGridViewChanged)
            {
                if (_currentSelectedSaleObject != null)
                {
                    // Calculate the profit or loss value
                    txtBoxAddSalesCosts.Text = CalculateProfitLoss(txtBoxAddSalesVolume.Text, txtBoxAddSalesSalesPrice.Text,
                        _shareObject.MarketValue + _currentSelectedSaleObject.SaleValue - _currentSelectedSaleObject.SaleProfitLoss, _shareObject.Volume + _currentSelectedSaleObject.SaleVolume);                                        
                }
                else
                {
                    // Calculate the profit or loss value
                    txtBoxAddSalesCosts.Text = CalculateProfitLoss(txtBoxAddSalesVolume.Text, txtBoxAddSalesSalesPrice.Text,
                        _shareObject.MarketValue, _shareObject.Volume);                    
                }
            }
        }

        /// <summary>
        /// This function calls the CalculateValue function if
        /// the text has changed
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddSalePrice_TextChanged(object sender, EventArgs e)
        {
            Decimal.TryParse(((TextBox)sender).Text, out _decSalePrice);

            // Calculate the sales value
            txtBoxAddSalesSum.Text = CalculateSalePayoutValue();

            // Calculate the original payout
            //if (_bForeignFlag)
            //    txtBoxAddSalesSum.Text = CaluclateSalePayoutFromForeignCurrency();

            // Calculate the original price
            if (_bForeignFlag)
                txtBoxAddSalesSalesPrice.Text = CaluclateSalePriceFromForeignCurrencyPrice();

            //// Calculate payout foreign currency of the share
            //if (_bForeignFlag || LoadGridSelectionFlag)
            //    txtBoxAddSalesSumFC.Text = CalculateSalePayOutForeignCurrency();

            // Check the gridview selection has changed or not
            if (!_bSelectionGridViewChanged)
            {
                if (_currentSelectedSaleObject != null)
                {
                    // Calculate the profit or loss value
                    txtBoxAddSalesCosts.Text = CalculateProfitLoss(txtBoxAddSalesVolume.Text, txtBoxAddSalesSalesPrice.Text,
                        _shareObject.MarketValue + _currentSelectedSaleObject.SaleValue - _currentSelectedSaleObject.SaleProfitLoss, _shareObject.Volume + _currentSelectedSaleObject.SaleVolume);
                }
                else
                {
                    // Calculate the profit or loss value
                    txtBoxAddSalesCosts.Text = CalculateProfitLoss(txtBoxAddSalesVolume.Text, txtBoxAddSalesSalesPrice.Text,
                        _shareObject.MarketValue, _shareObject.Volume);
                }
            }
        }

        private void OnTxtBoxAddSalesSalesPriceFC_TextChanged(object sender, EventArgs e)
        {
            Decimal.TryParse(((TextBox)sender).Text, out _decSalePriceFC);

            // Calculate the original payout
            if (_bForeignFlag)
                txtBoxAddSalesSalesPrice.Text = CaluclateSalePriceFromForeignCurrencyPrice();
        }

        // TODO
        private void OnTxtBoxAddSalesLossBalance_TextChanged(object sender, EventArgs e)
        {
            //    decimal.TryParse(((TextBox)sender).Text, out _decLossBalance);
            //    TaxValuesCurrent.LossBalance = _decLossBalance;

            //    if (((TextBox)sender).Text != "")
            //    {
            //        // Check if the textbox is enabled than disabled the foreign currency textbox
            //        if (((TextBox)sender).Enabled == true)
            //        {
            //            txtBoxAddDividendLossBalanceFC.Enabled = false;
            //            txtBoxAddDividendLossBalanceFC.ReadOnly = true;
            //        }

            //        // Check if the value for the foreign currency should be calculated and set
            //        if (chkBoxAddDividendExchangeRatio.CheckState == CheckState.Checked)
            //        {
            //            // Only set this value when the text box is disabled so the input comes from the normal currency
            //            if (txtBoxAddDividendLossBalanceFC.Enabled == false)
            //                txtBoxAddDividendLossBalanceFC.Text = CalculateLossBalanceForeignCurrency();
            //        }
            //        else
            //        {
            //            // If no foreign currency should be calculated disable the text box
            //            txtBoxAddDividendLossBalanceFC.Enabled = false;
            //            txtBoxAddDividendLossBalanceFC.ReadOnly = true;
            //            txtBoxAddDividendLossBalanceFC.Text = @"-";
            //        }
            //    }
            //    else
            //    {
            //        // If the text box is empty enable it
            //        ((TextBox)sender).Enabled = true;
            //        ((TextBox)sender).ReadOnly = false;

            //        // Check if a foreign currency should be calculated
            //        if (chkBoxAddDividendExchangeRatio.CheckState == CheckState.Checked)
            //        {
            //            // Enable the foreign currency text box and delete the content
            //            txtBoxAddDividendLossBalanceFC.Enabled = true;
            //            txtBoxAddDividendLossBalanceFC.ReadOnly = false;
            //            txtBoxAddDividendLossBalanceFC.Text = @"";
            //        }
            //    }
        }

        // TODO
        private void OnTxtBoxAddSalesLossBalanceFC_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This function sets the taxes values when the sale sum has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTxtBoxAddSalesSum_TextChanged(object sender, EventArgs e)
        {
            Decimal.TryParse(txtBoxAddSalesSum.Text, out _decSalePayout);

            TaxValuesCurrent.ValueWithoutTaxes = _decSalePayout;

            _decTax = TaxValuesCurrent.ValueWithoutTaxes;
            _decTaxFC = TaxValuesCurrent.ValueWithoutTaxesFC;

            // TODO
            //txtBoxAddSalesTax.Text = TaxValuesCurrent.ValueWithTaxesAsString;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date
        /// It also reset the background colors
        /// </summary>
        private void ResetValues()
        {
            // Reset date time picker
            datePickerAddSaleDate.Value = DateTime.Now;
            datePickerAddSaleTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            // Reset text boxes
            chkBoxAddSalesFC.CheckState = CheckState.Unchecked;
            txtBoxAddSalesExchangeRatioFC.Text = @"-";
            txtBoxAddSalesVolume.Text = @"";
            txtBoxAddSalesBuyPrice.Text = @"";
            txtBoxAddSalesSalesPrice.Text = @"";
            txtBoxAddSalesLossBalance.Text = @"";
            txtBoxAddSalesSum.Text = @"-";
            txtBoxAddSalesTax.Text = @"-";
            txtBoxAddSalesCosts.Text = @"-";
            txtBoxAddSalesDocument.Text = @"";
            txtBoxAddSalesVolume.Focus();
        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit" it is choosen if
        /// a buy should be add or edit.
        /// </summary>
        /// <param name="bFlagAddEdit">Flag if a sale should be add (true) or edit (false)</param>
        /// <param name="strDateTime">Date and time of the sale</param>
        /// <param name="decVolume">Volume of the sale</param>
        /// <param name="decMaxVolume"></param>
        /// <param name="decValue">Value of the sale</param>
        /// <param name="strDoc">Document of the sale</param>
        /// <returns>Flag if the input values are correct or not</returns>
        bool CheckSaleInput(bool bFlagAddEdit, out string strDateTime, out decimal decVolume, decimal decMaxVolume, out decimal decValue, out decimal decProfitLoss, out string strDoc)
        {
            bool errorFlag = false;
            decVolume = 0;
            decValue = 0;
            decProfitLoss = 0;
            strDateTime = datePickerAddSaleDate.Text + " " + datePickerAddSaleTime.Text;
            strDoc = txtBoxAddSalesDocument.Text;

            try
            {
                toolStripStatusLabelMessage.ForeColor = Color.Red;
                toolStripStatusLabelMessage.Text = "";

                // Check if a sale with the given date and time already exists
                foreach (var sale in _shareObject.AllSaleEntries.GetAllSalesOfTheShare())
                {
                    if (bFlagAddEdit)
                    {
                        // By an Add all dates must be checked
                        if (sale.SaleDate == strDateTime)
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateExists", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                            errorFlag = true;
                        }
                    }
                    else
                    {
                        // By an Edit all sales without the edit entry date and time must be checked
                        if (sale.SaleDate == strDateTime
                            && _selectedDataGridView != null
                            && _selectedDataGridView.SelectedRows.Count == 1
                            && sale.SaleDate != _selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString())
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                               _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/DateWrongFormat", _languageName),
                               _xmlLanguage, _languageName,
                               Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                            errorFlag = true;
                        }
                    }
                }

                // Check if a volume for the sale is given
                if (txtBoxAddSalesVolume.Text == @"" && errorFlag == false)
                {
                    txtBoxAddSalesVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeEmpty", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (!decimal.TryParse(txtBoxAddSalesVolume.Text, out decVolume) && errorFlag == false)
                {
                    txtBoxAddSalesVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongFormat", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (decVolume <= 0 && errorFlag == false)
                {
                    txtBoxAddSalesVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeWrongValue", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (decVolume > decMaxVolume && errorFlag == false)
                {
                    txtBoxAddSalesVolume.Text = String.Format("{0:N2}", decMaxVolume);
                    txtBoxAddSalesVolume.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/VolumeMaxValue", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }

                // Value
                if (txtBoxAddSalesCosts.Text == @"" && errorFlag == false)
                {
                    txtBoxAddSalesCosts.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ValueEmpty", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (!decimal.TryParse(txtBoxAddSalesCosts.Text, out decValue) && errorFlag == false)
                {
                    txtBoxAddSalesCosts.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ValueWrongFormat", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (decValue <= 0 && errorFlag == false)
                {
                    txtBoxAddSalesCosts.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ValueWrongValue", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }

                //// Profit or loss
                //if (txtBoxAddSalesProfitLossPercentage.Text == @"" && errorFlag == false)
                //{
                //    txtBoxAddSalesProfitLossPercentage.Focus();

                //    // Add status message
                //    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                //       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ProfitLossEmpty", _languageName),
                //       _xmlLanguage, _languageName,
                //       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                //    errorFlag = true;
                //}
                //else if (!decimal.TryParse(txtBoxAddSalesProfitLossPercentage.Text, out decProfitLoss) && errorFlag == false)
                //{
                //    txtBoxAddSalesProfitLossPercentage.Focus();

                //    // Add status message
                //    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                //       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ProfitLossWrongFormat", _languageName),
                //       _xmlLanguage, _languageName,
                //       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                //    errorFlag = true;
                //}

                // Check if a given document exists
                if (strDoc != @"" && strDoc != @"-" && !File.Exists(strDoc) && errorFlag == false)
                {
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                       _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/FileDoesNotExist", _languageName),
                       _xmlLanguage, _languageName,
                       Color.Red, _logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }

                // Reset string values
                if (errorFlag)
                {
                    strDateTime = @"";
                    strDoc = @"";
                }

                return errorFlag;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CheckSaleInput()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/CheckInputFailure", _languageName),
                   _xmlLanguage, _languageName,
                   Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                // Reset string values
                strDateTime = @"";
                strDoc = @"";

                return true;
            }
        }

        /// <summary>
        /// This function caluclates the value of the given values
        /// If a given value is not valid the value text box is set to "-"
        /// <returns>String with the value or "-" if the calculation failed</returns>>
        /// </summary>
        private string CalculateSalePayoutValue()
        {
            try
            {
                if (Math.Abs(_decSalePrice) > 0
                    && Math.Abs(_decVolume) > 0)
                {
                    return Helper.FormatDecimal((_decSalePrice * _decVolume), Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObject.CultureInfo);
                }
                else
                {
                    return @"-";
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculateValue()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                return @"-";
            }
        }

        /// <summary>
        /// This function caluclates the profit / loss of the given values
        /// If a given value is not valid the profit / loss text box is set to "-"
        /// <param name="strSaleVolume">Sale volume of the share</param>
        /// <param name="strSalePrice">Current price of the share</param>
        /// <param name="decShareDeposit">Current deposit of the share</param>
        /// <param name="decShareVolume">Current share volume</param>
        /// <returns>String with the profit / loss or "-" if the calculation failed</returns>>
        /// </summary>
        private string CalculateProfitLoss(string strSaleVolume, string strSalePrice, decimal decShareDeposit, decimal decShareVolume)
        {
            try
            {
                decimal volume = 0;

                if (decimal.TryParse(strSaleVolume, out volume) && volume > 0)
                {
                    decimal price = 0;

                    if (decimal.TryParse(strSalePrice, out price))
                    {
                        decimal decSalePrice = price * volume;

                        // Check if the share deposit and share volume is not "0"
                        decimal decDeposit = 0;
                        if (decShareDeposit > 0 && decShareVolume > 0)
                        {
                            decDeposit = volume * decShareDeposit / decShareVolume;
                        }
                        
                        return Helper.FormatDecimal((decSalePrice - decDeposit), Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObject.CultureInfo);
                    }
                    else
                    {
                        return @"-";
                    }
                }
                else
                {
                    return @"-";
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculatePrice()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                return @"-";
            }
        }

        /// <summary>
        /// This function caluclates the dividend payout foreign currency
        /// <returns>String with the payout or "-" if the calculation failed</returns>>
        /// </summary>
        private string CalculateSalePayoutForeignCurrency()
        {
            try
            {
                if (Math.Abs(_decSalePriceFC) > 0
                    && Math.Abs(_decVolume) > 0)
                {
                    _decSalePayoutFC = _decSalePriceFC * _decVolume;
                    return Helper.FormatDecimal(_decSalePayoutFC,
                            Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObject.CultureInfo);
                }
                else
                {
                    return @"-";
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CalculateSalePayoutForeignCurrency()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                return @"-";
            }
        }

        /// <summary>
        /// This function caluclates from the foreign currency price the normal price
        /// <returns>String with the normal payout of one share of the share or "-" if the calculation failed</returns>>
        /// </summary>
        private string CaluclateSalePriceFromForeignCurrencyPrice()
        {
            try
            {
                if (Math.Abs(_decExchangeRatio) > 0
                    && Math.Abs(_decSalePriceFC) > 0)
                {
                    _decSalePrice = _decSalePriceFC / _decExchangeRatio;
                    return
                        Helper.FormatDecimal(_decSalePrice,
                            Helper.Currencyfivelength, false, Helper.Currencytwofixlength, false, @"", _shareObject.CultureInfo);
                }
                else
                {
                    return @"-";
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("CaluclateSalePriceFromForeignCurrencyPrice()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#endif
                return @"-";
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
                txtBoxAddSalesDocument.Text = Helper.SetDocument(_xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/GrpBoxAddEdit/OpenFileDialog/Title", _languageName), strFilter, txtBoxAddSalesDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnSaleDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormSale/Errors/ChoseDocumentFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function check is a foreign current calucation should be done.
        /// The function shows or hides, resize and reposition controls
        /// </summary>
        private void CheckForeignCurrencyCalulationShouldBeDone()
        {
            if (chkBoxAddSalesFC.CheckState == CheckState.Checked)
            {
                _bForeignFlag = true;

                // Enable controls
                txtBoxAddSalesExchangeRatioFC.Enabled = true;
                txtBoxAddSalesExchangeRatioFC.ReadOnly = false;
                cbxBoxAddSalesFC.Enabled = true;

                // Disable controls
                txtBoxAddSalesSalesPrice.Enabled = false;
                txtBoxAddSalesSum.Enabled = false;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxAddSalesExchangeRatioFC.Text = @"";
                    txtBoxAddSalesVolume.Text = @"";
                    txtBoxAddSalesBuyPrice.Text = @"";
                    txtBoxAddSalesSalesPrice.Text = @"-";
                    txtBoxAddSalesSum.Text = @"-";
                    txtBoxAddSalesTax.Text = @"-";
                }
                txtBoxAddSalesExchangeRatioFC.Focus();

                // Set edit values variable
                TaxValuesCurrent.FCFlag = true;
            }
            else
            {
                _bForeignFlag = false;

                // Disable controls
                txtBoxAddSalesExchangeRatioFC.Enabled = false;
                txtBoxAddSalesExchangeRatioFC.ReadOnly = true;
                cbxBoxAddSalesFC.Enabled = false;

                // Enable controls
                txtBoxAddSalesSalesPrice.Enabled = true;

                // Set values
                if (!LoadGridSelectionFlag)
                {
                    txtBoxAddSalesExchangeRatioFC.Text = @"-";
                    txtBoxAddSalesVolume.Text = @"";
                    txtBoxAddSalesBuyPrice.Text = @"";
                    txtBoxAddSalesSalesPrice.Text = @"";
                    txtBoxAddSalesSum.Text = @"-";
                    txtBoxAddSalesTax.Text = @"-";
                }

                txtBoxAddSalesVolume.Focus();

                // Set edit values variable
                TaxValuesCurrent.FCFlag = false;
            }
        }

        /// <summary>
        /// This function changes the currency unit when the currency
        /// has been changed
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">EventArgs</param>
        private void cbxBoxAddSalesForeignCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
//            lblAddSalesPriceShareFCUnit.Text = ((ComboBox)sender).SelectedItem.ToString().Split('-')[1].Trim();

            // Set currency units
//            TaxValuesCurrent.CurrencyUnit = lblAddSalesPriceShareFCUnit.Text;
            if (((ComboBox)sender).SelectedItem != null)
            {
                TaxValuesCurrent.FCUnit = ((ComboBox)sender).SelectedItem.ToString().Split('-')[1].Trim();
                TaxValuesCurrent.CiShareFC = Helper.GetCultureByISOCurrencySymbol(((ComboBox)sender).SelectedItem.ToString().Split('-')[0].Trim());
            }
        }

        /// <summary>
        /// This function checks the CheckBox change
        /// </summary>
        /// <param name="sender">Checkbox</param>
        /// <param name="e">EventArgs</param>
        private void chkBoxAddSalesForeignCurrency_CheckedChanged(object sender, EventArgs e)
        {
            CheckForeignCurrencyCalulationShouldBeDone();
        }

        #endregion Methods
    }
}

