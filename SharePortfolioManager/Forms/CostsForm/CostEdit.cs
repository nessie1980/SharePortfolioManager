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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class FrmShareCostEdit : Form
    {
        #region Variables

        /// <summary>
        /// Stores the chosen market value share object
        /// </summary>
        ShareObjectMarketValue _shareObjectMarketValue = null;

        /// <summary>
        /// Stores the chosen find value share object
        /// </summary>
        ShareObjectFinalValue _shareObjectFinalValue = null;

        /// <summary>
        /// Stores the logger
        /// </summary>
        Logger _logger;

        /// <summary>
        /// Stores the given language file
        /// </summary>
        Language _language;

        /// <summary>
        /// Stores the given language
        /// </summary>
        String _languageName;

        /// <summary>
        /// Stores if a cost has been deleted or added
        /// and so a save must be done in the lower dialog
        /// </summary>
        private Boolean _bSave;

        /// <summary>
        /// Stores the flag if the cost is part of a share buy
        /// </summary>
        private bool _bPartOfABuy;

        /// <summary>
        /// Store the flag if the costs are shown
        /// </summary>
        private bool _bShowCosts;

        /// <summary>
        /// Stores the selected DataGridView
        /// </summary>
        DataGridView _selectedDataGridView = null;

        #endregion Variables

        #region Properties

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

        public bool SaveFlag
        {
            get { return _bSave; }
            set { _bSave = value; }
        }

        public bool PartOfABuyFlag
        {
            get { return _bPartOfABuy; }
            set { _bPartOfABuy = value; }
        }

        public bool ShowCostsFlag
        {
            get { return _bShowCosts; }
            set { _bShowCosts = value; }
        }

        public DataGridView SelectedDataGridView
        {
            get { return _selectedDataGridView; }
            set { _selectedDataGridView = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shareObjectMarketValue">Current chosen market share object</param>
        /// <param name="shareObjectFinalValue">Current chosen final share object</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        public FrmShareCostEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue, Logger logger, Language language, String languageName)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;
            Logger = logger;
            Language = language;
            LanguageName = languageName;
            _bSave = false;
        }

        /// <summary>
        /// This function sets the language to the labels and buttons
        /// and sets the units behind the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareCostEdit_Load(object sender, EventArgs e)
        {
            try
            {
                Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Caption", _languageName);
                grpBoxAddCost.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Add_Caption", _languageName);
                grpBoxCosts.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxCosts/Caption", _languageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Labels/Date", _languageName);
                lblCostValue.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Labels/Value", _languageName);
                lblCostDoc.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Labels/Document", _languageName);
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Add", _languageName);
                btnDelete.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Delete", _languageName);
                btnReset.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Reset", _languageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Cancel", _languageName);

                // Load button images
                btnAddSave.Image = Resources.black_save;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;
                
                // Set share values to the edit boxes
                lblCostUnit.Text = ShareObjectMarketValue.CurrencyUnit;


                ShowCosts();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShareCostEdit_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/ShowFailed", _languageName),
                   Language, _languageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function paints the cost list of the share
        /// </summary>
        private void ShowCosts()
        {
            try
            {
                _bShowCosts = true;

                // Reset tab control
                foreach (TabPage tabPage in tabCtrlCosts.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        DataGridView view = control as DataGridView;
                        if (view != null)
                        {
                            view.SelectionChanged -= OnDataGridViewCostsOfAYear_SelectionChanged;
                            view.SelectionChanged -= OnDataGridViewCostsOfYears_SelectionChanged;
                            view.DataBindingComplete -= OnDataGridViewCostsOfAYear_DataBindingComplete;
                        }
                    }
                    tabPage.Controls.Clear();
                    tabCtrlCosts.TabPages.Remove(tabPage);
                }

                tabCtrlCosts.TabPages.Clear();

                // Create TabPage for the costs of the years
                TabPage newTabPageOverviewYears = new TabPage();
                // Set TabPage name
                newTabPageOverviewYears.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxCosts/TabCtrl/TabPgOverview/Overview", _languageName) +
                                          " (" + ShareObjectFinalValue.AllCostsEntries.CostValueTotalWithUnitAsString + ")";

                // Create Binding source for the costs data
                BindingSource bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllCostsEntries.GetAllCostsTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllCostsEntries.GetAllCostsTotalValues();

                // Create DataGridView
                DataGridView dataGridViewCostsOverviewOfAYears = new DataGridView();
                dataGridViewCostsOverviewOfAYears.Dock = DockStyle.Fill;
                // Bind source with costs values to the DataGridView
                dataGridViewCostsOverviewOfAYears.DataSource = bindingSourceOverview;
                // Set the delegate for the DataBindingComplete event
                dataGridViewCostsOverviewOfAYears.DataBindingComplete += OnDataGridViewCostsOfAYear_DataBindingComplete;

                // Set row select event
                dataGridViewCostsOverviewOfAYears.SelectionChanged += OnDataGridViewCostsOfYears_SelectionChanged;

                // Advanced configuration DataGridView costs
                DataGridViewCellStyle styleOverviewOfYears = dataGridViewCostsOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;
                styleOverviewOfYears.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                dataGridViewCostsOverviewOfAYears.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridViewCostsOverviewOfAYears.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewCostsOverviewOfAYears.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                dataGridViewCostsOverviewOfAYears.RowHeadersVisible = false;
                dataGridViewCostsOverviewOfAYears.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewCostsOverviewOfAYears.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataGridViewCostsOverviewOfAYears.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                dataGridViewCostsOverviewOfAYears.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dataGridViewCostsOverviewOfAYears.MultiSelect = false;

                dataGridViewCostsOverviewOfAYears.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewCostsOverviewOfAYears.AllowUserToResizeColumns = false;
                dataGridViewCostsOverviewOfAYears.AllowUserToResizeRows = false;

                dataGridViewCostsOverviewOfAYears.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                newTabPageOverviewYears.Controls.Add(dataGridViewCostsOverviewOfAYears);
                dataGridViewCostsOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlCosts.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlCosts;

                // Check if costs pays exists
                if (ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary.Count > 0)
                {
                    // Loop through the years of the costs pays
                    foreach (
                        var keyName in ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary.Keys.Reverse()
                        )
                    {
                        // Create TabPage
                        TabPage newTabPage = new TabPage();
                        // Set TabPage name
                        newTabPage.Name = keyName;
                        newTabPage.Text = keyName + " ("
                            + ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary[keyName]
                                                  .CostValueYearWithUnitAsString + ")";

                        // Create Binding source for the costs data
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource =
                            ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary[keyName].CostListYear;

                        // Create DataGridView
                        DataGridView dataGridViewCostsOfAYear = new DataGridView();
                        dataGridViewCostsOfAYear.Dock = DockStyle.Fill;
                        // Bind source with dividend values to the DataGridView
                        dataGridViewCostsOfAYear.DataSource = bindingSource;
                        // Set the delegate for the DataBindingComplete event
                        dataGridViewCostsOfAYear.DataBindingComplete += OnDataGridViewCostsOfAYear_DataBindingComplete;

                        // Set row select event
                        dataGridViewCostsOfAYear.SelectionChanged += OnDataGridViewCostsOfAYear_SelectionChanged;
                        // Set cell decimal click event
                        dataGridViewCostsOfAYear.CellContentDoubleClick += OnDataGridViewCostsOfAYear_CellContentdecimalClick;

                        // Advanced configuration DataGridView costs
                        DataGridViewCellStyle style = dataGridViewCostsOfAYear.ColumnHeadersDefaultCellStyle;
                        style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        style.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);

                        dataGridViewCostsOfAYear.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                        dataGridViewCostsOfAYear.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                        dataGridViewCostsOfAYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                        dataGridViewCostsOfAYear.RowHeadersVisible = false;
                        dataGridViewCostsOfAYear.RowsDefaultCellStyle.BackColor = Color.White;
                        dataGridViewCostsOfAYear.DefaultCellStyle.SelectionBackColor = Color.Blue;
                        dataGridViewCostsOfAYear.DefaultCellStyle.SelectionForeColor = Color.Yellow;

                        dataGridViewCostsOfAYear.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        dataGridViewCostsOfAYear.MultiSelect = false;

                        dataGridViewCostsOfAYear.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataGridViewCostsOfAYear.AllowUserToResizeColumns = false;
                        dataGridViewCostsOfAYear.AllowUserToResizeRows = false;

                        dataGridViewCostsOfAYear.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        newTabPage.Controls.Add(dataGridViewCostsOfAYear);
                        dataGridViewCostsOfAYear.Parent = newTabPage;
                        tabCtrlCosts.Controls.Add(newTabPage);
                        newTabPage.Parent = tabCtrlCosts;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShowCosts()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/ShowFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function opens the costs document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewCostsOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
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
                            // Get doc from the dividend with the strDateTime
                            foreach (var temp in ShareObjectFinalValue.AllCostsEntries.GetAllCostsOfTheShare())
                            {
                                // Check if the dividend date and time is the same as the date and time of the clicked buy item
                                if (temp.CostDate == strDateTime)
                                {
                                    // Check if the file still exists
                                    if (File.Exists(temp.CostDocument))
                                        // Open the file
                                        Process.Start(temp.CostDocument);
                                    else
                                    {
                                        string strCaption =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Error",
                                                _languageName);
                                        string strMessage =
                                            Language.GetLanguageTextByXPath(
                                                @"/MessageBoxForm/Content/DocumentDoesNotExistDelete",
                                                _languageName);
                                        string strOk =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Yes",
                                                _languageName);
                                        string strCancel =
                                            Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/No",
                                                _languageName);

                                        OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk,
                                            strCancel);
                                        if (messageBox.ShowDialog() == DialogResult.OK)
                                        {
                                            // Remove cost object and add it with no document
                                            if (ShareObjectFinalValue.RemoveCost(temp.CostDate) &&
                                                ShareObjectFinalValue.AddCost(temp.CostOfABuy, temp.CostDate, temp.CostValue))
                                            {
                                                // Set flag to save the share object.
                                                _bSave = true;

                                                ResetValues();
                                                ShowCosts();

                                                // Add status message
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    Language.GetLanguageTextByXPath(
                                                        @"/AddEditFormCosts/StateMessages/EditSuccess", _languageName),
                                                    Language, _languageName,
                                                    Color.Black, Logger, (int) FrmMain.EStateLevels.Info,
                                                    (int) FrmMain.EComponentLevels.Application);
                                            }
                                            else
                                            {
                                                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                                    Language.GetLanguageTextByXPath(
                                                        @"/AddEditFormCosts/Errors/EditFailed", _languageName),
                                                    Language, _languageName,
                                                    Color.Red, Logger, (int) FrmMain.EStateLevels.Error,
                                                    (int) FrmMain.EComponentLevels.Application);
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
                MessageBox.Show("dataGridViewCostsOfAYear_CellContentdecimalClick()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/DocumentShowFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
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
        void OnDataGridViewCostsOfAYear_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (tabCtrlCosts.TabPages.Count > 0)
                {
                    // Deselect row only of the other TabPages DataGridViews
                    if (tabCtrlCosts.SelectedTab.Controls.Contains((DataGridView)sender))
                        DeselectRowsOfDataGridViews((DataGridView)sender);
                }

                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    // Get CostObject of the selected DataGridView row
                    CostObject selectedCostObject = ShareObjectFinalValue.AllCostsEntries.GetCostObjectByDateTime(curItem[0].Cells[0].Value.ToString());
                    if (selectedCostObject != null)
                    {
                        _bPartOfABuy = selectedCostObject.CostOfABuy;

                        datePickerAddCostDate.Value = Convert.ToDateTime(selectedCostObject.CostDate);
                        datePickerAddCostTime.Value = Convert.ToDateTime(selectedCostObject.CostDate);
                        txtBoxAddCostValue.Text = selectedCostObject.CostValueAsString;
                        txtBoxAddCostDoc.Text = selectedCostObject.CostDocument;
                    }
                    else
                    {
                        // TODO Get CostObject and do not set DataGridView values!!!
                        datePickerAddCostDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        datePickerAddCostDate.Value = Convert.ToDateTime(curItem[0].Cells[0].Value.ToString());
                        txtBoxAddCostValue.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxAddCostDoc.Text = curItem[0].Cells[2].Value.ToString();
                    }

                    if (!selectedCostObject.CostOfABuy)
                    {
                        // Enable button(s)
                        btnAddSave.Enabled = true;
                        btnReset.Enabled = true;
                        btnDelete.Enabled = true;
                        btnCostDocumentBrowse.Enabled = true;

                        // Enable input fields
                        datePickerAddCostDate.Enabled = true;
                        datePickerAddCostTime.Enabled = true;
                        txtBoxAddCostValue.Enabled = true;
                        txtBoxAddCostDoc.Enabled = true;
                
                        // Rename add / save button
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Save", _languageName);
                        btnAddSave.Image = Resources.black_save;

                        // Rename group box
                        grpBoxAddCost.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Edit_Caption",
                            _languageName);
                    }
                    else
                    {
                        if (!_bShowCosts)
                        {
                            // Disable button(s) and edit boxes
                            btnAddSave.Enabled = false;
                            btnDelete.Enabled = false;
                            datePickerAddCostDate.Enabled = false;
                            datePickerAddCostTime.Enabled = false;
                            txtBoxAddCostValue.Enabled = false;
                            txtBoxAddCostDoc.Enabled = false;
                            btnCostDocumentBrowse.Enabled = false;
                        }

                        // Enable button(s)
                        btnReset.Enabled = true;
                    }

                    // Store DataGridView instance
                    _selectedDataGridView = (DataGridView)sender;
                }
                else
                {
                    // Rename add / save button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Add", _languageName);
                    btnAddSave.Image = Resources.black_add;

                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;

                    // Rename group box
                    grpBoxAddCost.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Add_Caption", _languageName);

                    // Reset stored DataGridView instance
                    _selectedDataGridView = null;
                }

                _bShowCosts = false;
            }
            catch (Exception ex)
            {
                // Deselect row in all DataGridViews
                DeselectRowsOfDataGridViews(null);
#if DEBUG
                MessageBox.Show("dataGridViewCostsOfAYear_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/SelectionChangeFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This functions selects the tab page of the chosen year
        /// and deselects all rows in the DataGridViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataGridViewCostsOfYears_SelectionChanged(object sender, EventArgs args)
        {
            try
            {
                if (((DataGridView)sender).SelectedRows.Count == 1)
                {
                    // Get the currently selected item in the ListBox
                    DataGridViewSelectedRowCollection curItem = ((DataGridView)sender).SelectedRows;

                    foreach (TabPage tabPage in tabCtrlCosts.TabPages)
                    {
                        if (tabPage.Name == curItem[0].Cells[0].Value.ToString())
                        {
                            tabCtrlCosts.SelectTab(tabPage);
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
                MessageBox.Show("dataGridViewCostsOfYears_SelectionChanged()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/SelectionChangeFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewCostsOfAYear_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxCosts/TabCtrl/DgvCostsOverview/ColHeader_Date", _languageName);
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxCosts/TabCtrl/DgvCostsOverview/ColHeader_Costs",
                                    _languageName) + @" (" + ShareObjectMarketValue.CurrencyUnit + ")";
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxCosts/TabCtrl/DgvCostsOverview/ColHeader_Document", _languageName);
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
                MessageBox.Show("dataGridViewCostsOfAYear_DataBindingComplete()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/RenameColHeaderFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                foreach (TabPage TabPage in tabCtrlCosts.TabPages)
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
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/DeselectFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function adds a new cost entry to the share object
        /// or it saves the change values when a cost will be edited
        /// It also checks if an entry already exists for the given date
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="arg">EventArgs</param>
        private void OnBtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool errorFlag = false;
                decimal dlbCost;
                string strDateTime;
                string strDoc;

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Add", _languageName))
                {
                    // Check the input values
                    errorFlag = CheckCostInput(true, out strDateTime, out dlbCost, out strDoc);

                    // If no error occurred add the new cost to the share
                    if (errorFlag == false)
                    {
                        if (ShareObjectFinalValue.AddCost(_bPartOfABuy, strDateTime, dlbCost, strDoc))
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/StateMessages/AddSuccess", _languageName),
                                Language, _languageName,
                                Color.Black, Logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);

                            // Set flag to save the share object.
                            _bSave = true;

                            // Reset values
                            ResetValues();

                            // Refresh the cost list
                            ShowCosts();
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/AddFailed", _languageName),
                                Language, _languageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        }
                    }
                }
                else
                {
                    // Check the input values
                    errorFlag = CheckCostInput(false, out strDateTime, out dlbCost, out strDoc);

                    // If no error occurred add the new cost to the share
                    if (errorFlag == false)
                    {
                        if (ShareObjectFinalValue.RemoveCost(_selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString()) && ShareObjectFinalValue.AddCost(_bPartOfABuy, strDateTime, dlbCost, strDoc))
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/StateMessages/EditSuccess", _languageName),
                                Language, _languageName,
                                Color.Black, Logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);

                            // Set flag to save the share object.
                            _bSave = true;

                            // Reset values
                            ResetValues();

                            // Rename add / save button
                            btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Add", _languageName);
                            btnAddSave.Image = Resources.black_add;
                            
                            // Disable button(s)
                            btnReset.Enabled = false;
                            btnDelete.Enabled = false;

                            grpBoxAddCost.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Add_Caption", _languageName);

                            // Refresh the costs list
                            ShowCosts();
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/EditFailed", _languageName),
                                Language, _languageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnAddSave_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Add", _languageName))
                {
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/AddFailed", _languageName),
                        Language, _languageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                }
                else
                {
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/EditFailed", _languageName),
                        Language, _languageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                }
            }
        }

        /// <summary>
        /// This function resets the edit process of a cost
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                _bPartOfABuy = false;

                toolStripStatusLabelMessage.Text = @"";

                // Enable edit boxes
                datePickerAddCostDate.Enabled = true;
                datePickerAddCostTime.Enabled = true;
                txtBoxAddCostValue.Enabled = true;
                txtBoxAddCostDoc.Enabled = true;

                // Enable button(s)
                btnAddSave.Enabled = true;
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Add", _languageName);
                btnAddSave.Image = btnAddSave.Image = Resources.black_save;
                btnCostDocumentBrowse.Enabled = true;

                // Disable button(s)
                btnReset.Enabled = false;
                btnDelete.Enabled = false;

                // Rename group box
                grpBoxAddCost.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Add_Caption", _languageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                _selectedDataGridView = null;

                // Select overview tab
                if (tabCtrlCosts.TabPages.ContainsKey(Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxCosts/TabCtrl/TabPgOverview/Overview", _languageName)))
                    tabCtrlCosts.SelectTab(Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxCosts/TabCtrl/TabPgOverview/Overview", _languageName));

                ShowCosts();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnCancel_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/CancelFailure", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function close the dialog
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This function shows a message box which ask the user
        /// if he really wants to delete the cost.
        /// If the user press "Ok" the cost will be deleted and the
        /// list of the costs will be updated.
        /// </summary>
        /// <param name="sender">Pressed button of the user</param>
        /// <param name="arg">EventArgs</param>
        private void OnBtnDelete_Click(object sender, EventArgs arg)
        {
            try
            {
                toolStripStatusLabelMessage.Text = @"";

                string strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", _languageName);
                string strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/CostsDelete", _languageName);
                string strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", _languageName);
                string strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", _languageName);

                OwnMessageBox messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                DialogResult dlgResult = messageBox.ShowDialog();

                if (dlgResult == DialogResult.OK)
                {
                    // Set flag to save the share object.
                    _bSave = true;

                    // Check if a row is selected
                    if (_selectedDataGridView != null && _selectedDataGridView.SelectedRows.Count == 1)
                    {
                        if (!ShareObjectFinalValue.RemoveCost(_selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString()))
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/DeleteFailed", _languageName),
                                Language, _languageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                        }
                        else
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/StateMessages/DeleteSuccess", _languageName),
                                Language, _languageName,
                                Color.Black, Logger, (int)FrmMain.EStateLevels.Info, (int)FrmMain.EComponentLevels.Application);
                        }
                    }

                    // Reset values
                    ResetValues();

                    // Enable button(s)
                    btnAddSave.Enabled = true;
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Buttons/Add", _languageName);
                    btnAddSave.Image = Resources.black_add;
                    
                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;

                    grpBoxAddCost.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/Add_Caption", _languageName);

                    // Refresh the costs list
                    ShowCosts();
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
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/DeleteFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function change the dialog result if the share object must be saved
        /// because a cost has been added or deleted
        /// </summary>
        /// <param name="sender">Dialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ShareCostEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_bSave)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// This function resets the text box values
        /// and sets the date time picker to the current date
        /// It also reset the background colors
        /// </summary>
        private void ResetValues()
        {
            _bPartOfABuy = false;

            // Reset date time picker
            datePickerAddCostDate.Value = DateTime.Now;
            datePickerAddCostTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            datePickerAddCostDate.Enabled = true;
            datePickerAddCostTime.Enabled = true;

            // Reset text boxes
            txtBoxAddCostValue.Text = @"";
            txtBoxAddCostDoc.Text = @"";

            // Enable edit boxes
            txtBoxAddCostValue.Enabled = true;
            txtBoxAddCostDoc.Enabled = true;

            // Enable button(s)
            btnAddSave.Enabled = true;

            txtBoxAddCostValue.Focus();
        }

        /// <summary>
        /// This function checks if the input is correct
        /// With the flag "bFlagAddEdit it is chosen if
        /// a cost should be add or edit.
        /// </summary>
        /// <param name="bFlagAddEdit">Flag if a cost should be add (true) or edit (false)</param>
        /// <param name="strDateTime">Date and time of the cost</param>
        /// <param name="costValue">Value for the cost</param>
        /// <param name="strDoc">Document of the cost</param>
        /// <returns>Flag if the input values are correct or not</returns>
        bool CheckCostInput(bool bFlagAddEdit, out string strDateTime, out decimal costValue, out string strDoc)
        {
            costValue = 0;
            strDateTime = datePickerAddCostDate.Text + " " + datePickerAddCostTime.Text;
            strDoc = txtBoxAddCostDoc.Text;

            try
            {
                bool errorFlag = false;

                toolStripStatusLabelMessage.ForeColor = Color.Red;
                toolStripStatusLabelMessage.Text = "";

                // Check if a cost with the given date already exists
                foreach (var cost in ShareObjectFinalValue.AllCostsEntries.GetAllCostsOfTheShare())
                {
                    if (bFlagAddEdit)
                    {
                        if (cost.CostDate == strDateTime)
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/DateExists", _languageName),
                                Language, _languageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                            errorFlag = true;
                        }
                    }
                    else
                    {
                        // By an Edit all dates without the edit entry date must be checked
                        if (cost.CostDate == strDateTime
                            && _selectedDataGridView != null
                            && _selectedDataGridView.SelectedRows.Count == 1
                            && cost.CostDate != _selectedDataGridView.SelectedRows[0].Cells[0].Value.ToString())
                        {
                            // Add status message
                            Helper.AddStatusMessage(toolStripStatusLabelMessage,
                                Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/DateExists", _languageName),
                                Language, _languageName,
                                Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                            errorFlag = true;
                        }
                    }
                }

                // Check if the cost value is given
                if (txtBoxAddCostValue.Text == @"" && errorFlag == false)
                {
                    txtBoxAddCostValue.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/CostsEmpty", _languageName),
                        Language, _languageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (!decimal.TryParse(txtBoxAddCostValue.Text, out costValue) && errorFlag == false)
                {
                    txtBoxAddCostValue.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/CostsWrongFormat", _languageName),
                        Language, _languageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);

                    errorFlag = true;
                }
                else if (costValue <= 0 && errorFlag == false)
                {
                    txtBoxAddCostValue.Focus();

                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/CostsWrongValue", _languageName),
                        Language, _languageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
 
                    errorFlag = true;
                }

                // Check if a given document directory exists
                if (strDoc != @"" && !Directory.Exists(Path.GetDirectoryName(strDoc)) && errorFlag == false)
                {
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/DirectoryDoesNotExist", _languageName),
                        Language, _languageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
                    errorFlag = true;
                }

                // Check if a given document exists
                if (strDoc != @"" && !File.Exists(strDoc) && errorFlag == false)
                {
                    // Add status message
                    Helper.AddStatusMessage(toolStripStatusLabelMessage,
                        Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/FileDoesNotExist", _languageName),
                        Language, _languageName,
                        Color.Red, Logger, (int)FrmMain.EStateLevels.Error, (int)FrmMain.EComponentLevels.Application);
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
                MessageBox.Show("CheckCostInput()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/CheckInputFailure", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
                
                // Reset string values
                strDateTime = @"";
                strDoc = @"";

                return false;
            }
        }

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the cost of the share
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCostDocumentBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                txtBoxAddCostDoc.Text = Helper.SetDocument(Language.GetLanguageTextByXPath(@"/AddEditFormCosts/GrpBoxAddEdit/OpenFileDialog/Title", _languageName), strFilter, txtBoxAddCostDoc.Text);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnCostDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCosts/Errors/ChoseDocumentFailed", _languageName),
                    Language, _languageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion

    }
}
