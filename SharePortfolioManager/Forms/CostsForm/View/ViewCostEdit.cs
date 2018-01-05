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

namespace SharePortfolioManager.Forms.CostsForm.View
{
    public enum CostErrorCode
    {
        AddSuccessful,
        EditSuccessful,
        DeleteSuccessful,
        AddFailed,
        EditFailed,
        DeleteFailed,
        InputeValuesInvalid,
        DateExists,
        DateWrongFormat,
        CostsEmpty,
        CostsWrongFormat,
        CostsWrongValue,
        DocumentDirectoryDoesNotExits,
        DocumentFileDoesNotExists
    };

    /// <inheritdoc />
    /// <summary>
    /// Interface of the CostEdit view
    /// </summary>
    public interface IViewCostEdit : INotifyPropertyChanged
    {
        event EventHandler FormatInputValues;
        event EventHandler AddCost;
        event EventHandler EditCost;
        event EventHandler DeleteCost;

        CostErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; }
        ShareObjectFinalValue ShareObjectFinalValue { get; }

        bool UpdateCost { get; set; }
        string SelectedDate { get; set; }
        bool PartOfABuy { get; set; }
        bool PartOfASale { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string Costs { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddEditDeleteFinish();
    }

    public partial class ViewCostEdit : Form, IViewCostEdit
    {
        #region Fields

        /// <summary>
        /// Stores the date of a selected cost row
        /// </summary>
        private string _selectedDate;

        #endregion Fields

        #region Properties

        public Logger Logger { get; }

        public Language Language { get; }

        public string LanguageName { get; }

        public bool UpdateCostFlag { get; set; }

        public bool SaveFlag { get; set; }

        public DataGridView SelectedDataGridView { get; set; }

        #endregion Properties

        #region IView members

        public bool UpdateCost
        {
            get => UpdateCostFlag;
            set
            {
                UpdateCostFlag = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UpdateCost"));
            }
        }

        public bool PartOfABuy { get; set; }

        public bool PartOfASale { get; set; }

        public CostErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue { get; }

        public ShareObjectFinalValue ShareObjectFinalValue { get; }

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
                case CostErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/StateMessages/AddSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the cost list
                        ShowCosts();

                        // Reset values
                        Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case CostErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case CostErrorCode.EditSuccessful:
                    {
                        // Enable button(s)
                        btnAddSave.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Add",
                                LanguageName);
                        btnAddSave.Image = Resources.black_add;
                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Add_Caption",
                                LanguageName);

                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/StateMessages/EditSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Refresh the cost list
                        ShowCosts();

                        // Reset values
                        Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case CostErrorCode.EditFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/EditFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case CostErrorCode.DeleteSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/StateMessages/DeleteSuccess", LanguageName);
                        // Set flag to save the share object.
                        SaveFlag = true;

                        // Enable button(s)
                        btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Add", LanguageName);
                        btnAddSave.Image = Resources.black_add;

                        // Disable button(s)
                        btnReset.Enabled = false;
                        btnDelete.Enabled = false;

                        // Rename group box
                        grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Add_Caption", LanguageName);

                        // Refresh the cost list
                        ShowCosts();

                        // Reset values
                        Enabled = true;
                        ResetInputValues();

                        break;
                    }
                case CostErrorCode.DeleteFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/DeleteFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case CostErrorCode.InputeValuesInvalid:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/CheckInputFailure", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case CostErrorCode.DateExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/DateExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case CostErrorCode.DateWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/DateWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        datePickerDate.Focus();

                        break;
                    }
                case CostErrorCode.CostsEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/CostsEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case CostErrorCode.CostsWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/CostsWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case CostErrorCode.CostsWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/CostsWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        Enabled = true;
                        txtBoxCosts.Focus();

                        break;
                    }
                case CostErrorCode.DocumentDirectoryDoesNotExits:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/DirectoryDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
                case CostErrorCode.DocumentFileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/FileDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;

                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
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
        public event EventHandler FormatInputValues;
        public event EventHandler AddCost;
        public event EventHandler EditCost;
        public event EventHandler DeleteCost;

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
        public ViewCostEdit(ShareObjectMarketValue shareObjectMarketValue, ShareObjectFinalValue shareObjectFinalValue, Logger logger, Language language, string languageName)
        {
            InitializeComponent();

            ShareObjectMarketValue = shareObjectMarketValue;
            ShareObjectFinalValue = shareObjectFinalValue;
            Logger = logger;
            Language = language;
            LanguageName = languageName;
            SaveFlag = false;
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
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/Caption", LanguageName);
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Add_Caption", LanguageName);
                grpBoxOverview.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxCost/Caption",
                        LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Labels/Date",
                    LanguageName);
                lblCosts.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Labels/Cost",
                        LanguageName);
                lblDocument.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Labels/Document",
                        LanguageName);
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Add",
                    LanguageName);
                btnReset.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Reset",
                    LanguageName);
                btnDelete.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Delete",
                        LanguageName);
                btnCancel.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Cancel",
                        LanguageName);

                #endregion Language configuration

                #region Unit configuration

                // Set cost unit to the edit box
                lblAddCostsUnit.Text = ShareObjectFinalValue.CurrencyUnit;

                #endregion Unit configuration

                #region Image configuration

                // Load button images
                btnAddSave.Image = Resources.black_save;
                btnDelete.Image = Resources.black_delete;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                #endregion Image configuration

                // Load costs of the share
                ShowCosts();
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"ShareCostEdit_Load()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/ShowFailed", LanguageName),
                   Language, LanguageName, Color.DarkRed, Logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
            // Check if a cost change must be saved
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
            datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            // Reset text boxes
            txtBoxCosts.Text = @"";
            txtBoxDocument.Text = @"";
            txtBoxCosts.Focus();
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
        private void datePickerTime_Leave(object sender, EventArgs e)
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
        private void OnTxtBoxCosts_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Costs"));
        }

        /// <summary>
        /// This function updates the view with the formatted value
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxCosts_Leave(object sender, EventArgs e)
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
        private void OntxtBoxDocument_Leave(object sender, EventArgs e)
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
        /// This function adds a new cost entry to the share object
        /// or edit a cost entry
        /// It also checks if an entry already exists for the given date and time
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls
                //this.Enabled = false;

                if (btnAddSave.Text == Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Add", LanguageName))
                {
                    UpdateCost = false;

                    AddCost?.Invoke(this, null);
                }
                else
                {
                    UpdateCost = true;

                    EditCost?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"btnAdd_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/AddFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
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
                // Disable controls
                Enabled = false;

                toolStripStatusLabelMessageCostEdit.Text = @"";

                var strCaption = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Captions/Info", LanguageName);
                var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/CostDelete",
                    LanguageName);
                var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok", LanguageName);
                var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel", LanguageName);

                var messageBox = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);

                var dlgResult = messageBox.ShowDialog();

                if (dlgResult != DialogResult.OK) return;

                DeleteCost?.Invoke(this, null);
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"btnDelete_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/DeleteFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function cancel the edit process of a cost
        /// </summary>
        /// <param name="sender">Clicked button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabelMessageCostEdit.Text = @"";

                // Enable button(s)
                btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Add", LanguageName);
                btnAddSave.Image = Resources.black_add;

                // Disable button(s)
                btnReset.Enabled = false;
                btnDelete.Enabled = false;

                // Rename group box
                grpBoxAdd.Text =
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Add_Caption", LanguageName);

                // Deselect rows
                DeselectRowsOfDataGridViews(null);

                // Reset stored DataGridView instance
                SelectedDataGridView = null;

                // Select overview tab
                if (
                    tabCtrlCosts.TabPages.ContainsKey(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormCost/GrpBoxCost/TabCtrl/TabPgOverview/Overview", LanguageName)))
                    tabCtrlCosts.SelectTab(
                        Language.GetLanguageTextByXPath(
                            @"/AddEditFormCost/GrpBoxCost/TabCtrl/TabPgOverview/Overview", LanguageName));
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"btnReset_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/CancelFailure", LanguageName),
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
        /// can chose a file which documents the cost
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnDocumentBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                txtBoxDocument.Text = Helper.SetDocument(Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/OpenFileDialog/Title", LanguageName), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"btnCostsDocumentBrowse_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/ChoseDocumentFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Buttons

        #region Data grid view

        /// <summary>
        /// This function paints the cost list of the share
        /// </summary>
        private void ShowCosts()
        {
            try
            {
                // Reset tab control
                foreach (TabPage tabPage in tabCtrlCosts.TabPages)
                {
                    foreach (var control in tabPage.Controls)
                    {
                        if (!(control is DataGridView dataGridView)) continue;

                        if (tabPage.Name == "Overview")
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewCostsOfYears_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewCostsOfYears_MouseEnter;
                            dataGridView.MouseLeave -= OnDataGridViewCostsOfYears_MouseLeave;
                        }
                        else
                        {
                            dataGridView.SelectionChanged -= OnDataGridViewCostsOfAYear_SelectionChanged;
                            dataGridView.MouseEnter -= OnDataGridViewCostsOfAYear_MouseEnter;
                            dataGridView.MouseLeave -= OnDataGridViewCostsOfAYear_MouseEnter;
                            dataGridView.CellContentDoubleClick -= OnDataGridViewCostsOfAYear_CellContentdecimalClick;
                        }

                        dataGridView.DataBindingComplete -= OnDataGridViewCosts_DataBindingComplete;
                    }
                    tabPage.Controls.Clear();
                    tabCtrlCosts.TabPages.Remove(tabPage);
                }

                tabCtrlCosts.TabPages.Clear();

                #region Add page

                // Create TabPage for the costs of the years
                var newTabPageOverviewYears = new TabPage
                {
                    // Set TabPage name
                    Name = Language.GetLanguageTextByXPath(
                        @"/AddEditFormCost/GrpBoxCost/TabCtrl/TabPgOverview/Overview",
                        LanguageName),

                    // Set TabPage caption
                    Text = Language.GetLanguageTextByXPath(
                               @"/AddEditFormCost/GrpBoxCost/TabCtrl/TabPgOverview/Overview", LanguageName) +
                           @" (" + ShareObjectFinalValue.AllCostsEntries.CostValueTotalWithUnitAsStr + @")"
                };

                #endregion Add page

                #region Data source, data binding and data grid view

                // Create Binding source for the cost data
                var bindingSourceOverview = new BindingSource();
                if (ShareObjectFinalValue.AllCostsEntries.GetAllCostsTotalValues().Count > 0)
                    bindingSourceOverview.DataSource =
                        ShareObjectFinalValue.AllCostsEntries.GetAllCostsTotalValues();

                // Create DataGridView
                var dataGridViewCostsOverviewOfAYears = new DataGridView
                {
                    Dock = DockStyle.Fill,

                    // Bind source with cost values to the DataGridView
                    DataSource = bindingSourceOverview
                };

                #endregion Data source, data binding and data grid view

                #region Events

                // Set the delegate for the DataBindingComplete event
                dataGridViewCostsOverviewOfAYears.DataBindingComplete += OnDataGridViewCosts_DataBindingComplete;
                // Set the delegate for the mouse enter event
                dataGridViewCostsOverviewOfAYears.MouseEnter += OnDataGridViewCostsOfYears_MouseEnter;
                // Set the delegate for the mouse leave event
                dataGridViewCostsOverviewOfAYears.MouseLeave += OnDataGridViewCostsOfYears_MouseLeave;
                // Set row select event
                dataGridViewCostsOverviewOfAYears.SelectionChanged += OnDataGridViewCostsOfYears_SelectionChanged;

                #endregion Events

                #region Style 

                // Advanced configuration DataGridView costs
                var styleOverviewOfYears = dataGridViewCostsOverviewOfAYears.ColumnHeadersDefaultCellStyle;
                styleOverviewOfYears.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

                #endregion Style

                #region Control add

                newTabPageOverviewYears.Controls.Add(dataGridViewCostsOverviewOfAYears);
                dataGridViewCostsOverviewOfAYears.Parent = newTabPageOverviewYears;
                tabCtrlCosts.Controls.Add(newTabPageOverviewYears);
                newTabPageOverviewYears.Parent = tabCtrlCosts;

                #endregion Control add

                // Check if costs exists
                if (ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary.Count <= 0) return;

                // Loop through the years of the costs
                foreach (
                    var keyName in ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary.Keys.Reverse()
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
                               ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary[keyName]
                                   .CostValueYearWithUnitAsStr
                               + @")"
                    };

                    #endregion Add page

                    #region Data source, data binding and data grid view

                    // Create Binding source for the cost data
                    var bindingSource = new BindingSource
                    {
                        DataSource = ShareObjectFinalValue.AllCostsEntries.AllCostsOfTheShareDictionary[keyName]
                            .CostListYear
                    };

                    // Create DataGridView
                    var dataGridViewCostsOfAYear = new DataGridView
                    {
                        Dock = DockStyle.Fill,

                        // Bind source with cost values to the DataGridView
                        DataSource = bindingSource
                    };

                    #endregion Data source, data binding and data grid view

                    #region Events

                    // Set the delegate for the DataBindingComplete event
                    dataGridViewCostsOfAYear.DataBindingComplete += OnDataGridViewCosts_DataBindingComplete;
                    // Set the delegate for the mouse enter event
                    dataGridViewCostsOfAYear.MouseEnter += OnDataGridViewCostsOfAYear_MouseEnter;
                    // Set the delegate for the mouse leave event
                    dataGridViewCostsOfAYear.MouseLeave += OnDataGridViewCostsOfAYear_MouseLeave;
                    // Set row select event
                    dataGridViewCostsOfAYear.SelectionChanged += OnDataGridViewCostsOfAYear_SelectionChanged;
                    // Set cell decimal click event
                    dataGridViewCostsOfAYear.CellContentDoubleClick += OnDataGridViewCostsOfAYear_CellContentdecimalClick;

                    #endregion Events

                    #region Style

                    // Advanced configuration DataGridView costs
                    var style = dataGridViewCostsOfAYear.ColumnHeadersDefaultCellStyle;
                    style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

                    #endregion Style

                    #region Control add

                    newTabPage.Controls.Add(dataGridViewCostsOfAYear);
                    dataGridViewCostsOfAYear.Parent = newTabPage;
                    tabCtrlCosts.Controls.Add(newTabPage);
                    newTabPage.Parent = tabCtrlCosts;

                    #endregion Control add
                }
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"ShowCosts()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/ShowFailed", LanguageName),
                   Language, LanguageName, Color.DarkRed, Logger,
                   (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function deselects the select row after the DataBinding is complete
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewBindingCompleteEventArgs</param>
        private void OnDataGridViewCosts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                                Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxCost/TabCtrl/DgvCostOverview/ColHeader_Date",
                                    LanguageName);
                            break;
                        case 1:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxCost/TabCtrl/DgvCostOverview/ColHeader_Costs",
                                    LanguageName) + @" (" + ShareObjectFinalValue.CurrencyUnit + @")";
                            break;
                        case 2:
                            ((DataGridView)sender).Columns[i].HeaderText =
                                Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxCost/TabCtrl/DgvCostOverview/ColHeader_Document",
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
#if DEBUG_COST || DEBUG
                var message = $"dataGridViewCostsOfAYear_DataBindingComplete()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/RenameColHeaderFailed", LanguageName),
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
                foreach (TabPage tabPage in tabCtrlCosts.TabPages)
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
#if DEBUG_COST || DEBUG
                var message = $"DeselectRowsOfDataGridViews()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/DeselectFailed", LanguageName),
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
        private void TabCtrlCosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlCosts.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabCtrlCosts.SelectedTab.Controls)
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
        private void TabCtrlCosts_MouseEnter(object sender, EventArgs e)
        {
            if (tabCtrlCosts.SelectedTab == null) return;

            // Loop trough the controls of the selected tab page and set focus on the data grid view
            foreach (Control control in tabCtrlCosts.SelectedTab.Controls)
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
        private void TabCtrlCosts_MouseLeave(object sender, EventArgs e)
        {
            grpBoxAdd.Focus();
        }

        #endregion Tab control delegates

        #region Costs of years

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
                if (((DataGridView) sender).SelectedRows.Count != 1) return;

                // Get the currently selected item in the ListBox
                var curItem = ((DataGridView)sender).SelectedRows;

                foreach (TabPage tabPage in tabCtrlCosts.TabPages)
                {
                    if (tabPage.Name != curItem[0].Cells[0].Value.ToString()) continue;

                    tabCtrlCosts.SelectTab(tabPage);
                    tabPage.Focus();

                    // Deselect rows
                    DeselectRowsOfDataGridViews(null);

                    break;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"dataGridViewCostsOfYears_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewCostsOfYears_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewCostsOfYears_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
        }

        #endregion Costs of years

        #region Costs of a year

        /// <summary>
        /// This function deselects the other selected rows of any other DataGridViews
        /// Write the values of the selected row to the text boxes
        /// and enables or disables the buttons
        /// </summary>
        /// <param name="sender">Data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewCostsOfAYear_SelectionChanged(object sender, EventArgs args)
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
                    var curItem = ((DataGridView)sender).SelectedRows;

                    // Set selected date
                    SelectedDate = curItem[0].Cells[0].Value.ToString();

                    // Get CostObject of the selected DataGridView row
                    var selectedCostObject = ShareObjectFinalValue.AllCostsEntries.GetCostObjectByDateTime(SelectedDate);

                    // Set cost values
                    if (selectedCostObject != null)
                    {
                        datePickerDate.Value = Convert.ToDateTime(selectedCostObject.CostDate);
                        datePickerTime.Value = Convert.ToDateTime(selectedCostObject.CostDate);
                        txtBoxCosts.Text = selectedCostObject.CostValueAsStr;
                        txtBoxDocument.Text = selectedCostObject.CostDocument;
                    }
                    else
                    {
                        datePickerDate.Value = Convert.ToDateTime(SelectedDate);
                        datePickerTime.Value = Convert.ToDateTime(SelectedDate);
                        txtBoxCosts.Text = curItem[0].Cells[1].Value.ToString();
                        txtBoxDocument.Text = curItem[0].Cells[2].Value.ToString();
                    }

                    // Set cost values
                    if (selectedCostObject != null && (selectedCostObject.CostOfABuy || selectedCostObject.CostOfASale))
                    {
                        // Set flag if cost is part of a buy
                        PartOfABuy = selectedCostObject.CostOfABuy;

                        // Set flag if cost is part of a sale
                        PartOfASale = selectedCostObject.CostOfASale;

                        // Enable TextBoxe(s)
                        datePickerDate.Enabled = false;
                        datePickerTime.Enabled = false;
                        txtBoxCosts.Enabled = false;

                        // Enable Button(s)
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        if (selectedCostObject != null)
                        {
                            // Set flag if cost is part of a buy
                            PartOfABuy = selectedCostObject.CostOfABuy;

                            // Set flag if cost is part of a sale
                            PartOfASale = selectedCostObject.CostOfASale;

                            // Enable TextBoxe(s)
                            datePickerDate.Enabled = true;
                            datePickerTime.Enabled = true;
                            txtBoxCosts.Enabled = true;

                            // Enable Button(s)
                            btnDelete.Enabled = true;
                        }
                    }

                    // Enable button(s)
                    btnReset.Enabled = true;
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Save", LanguageName);
                    btnAddSave.Image = Resources.black_edit;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Edit_Caption", LanguageName);

                    // Store DataGridView instance
                    SelectedDataGridView = (DataGridView)sender;

                    // Format the input value
                    FormatInputValues?.Invoke(this, new EventArgs());
                }
                else
                {
                    // Rename button
                    btnAddSave.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Buttons/Add", LanguageName);
                    btnAddSave.Image = Resources.black_add;
                    // Disable button(s)
                    btnReset.Enabled = false;
                    btnDelete.Enabled = false;
                    // Enable Button(s)
                    btnAddSave.Enabled = true;
                    // Enabled TextBoxe(s)
                    datePickerDate.Enabled = true;
                    datePickerTime.Enabled = true;
                    txtBoxCosts.Enabled = true;

                    // Rename group box
                    grpBoxAdd.Text = Language.GetLanguageTextByXPath(@"/AddEditFormCost/GrpBoxAddEdit/Add_Caption", LanguageName);

                    // Reset stored DataGridView instance
                    SelectedDataGridView = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG_COST || DEBUG
                var message = $"dataGridViewCostsOfAYear_SelectionChanged()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                   Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/SelectionChangeFailed", LanguageName),
                   Language, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                ResetInputValues();

                ShowCosts();
            }
        }

        /// <summary>
        /// This function sets the focus to the entered data grid view
        /// </summary>
        /// <param name="sender">Entered data grid view</param>
        /// <param name="args">EventArgs</param>
        private static void OnDataGridViewCostsOfAYear_MouseEnter(object sender, EventArgs args)
        {
            ((DataGridView)sender).Focus();
        }

        /// <summary>
        /// This function sets the focus to the left data grid view
        /// </summary>
        /// <param name="sender">Left data grid view</param>
        /// <param name="args">EventArgs</param>
        private void OnDataGridViewCostsOfAYear_MouseLeave(object sender, EventArgs args)
        {
            grpBoxAdd.Focus();
        }

        /// <summary>
        /// This function opens the cost document if a document is present
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void OnDataGridViewCostsOfAYear_CellContentdecimalClick(object sender, DataGridViewCellEventArgs e)
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
                // Get date and time of the selected cost item
                var strDateTime = curItem[0].Cells[0].Value.ToString();

                // Check if a document is set
                if (curItem[0].Cells[iColumnCount - 1].Value.ToString() == @"-") return;

                // Get doc from the cost with the strDateTime
                foreach (var temp in ShareObjectFinalValue.AllCostsEntries.GetAllCostsOfTheShare())
                {
                    // Check if the cost date and time is the same as the date and time of the clicked cost item
                    if (temp.CostDate != strDateTime) continue;

                    // Check if the file still exists
                    if (File.Exists(temp.CostDocument))
                        // Open the file
                        Process.Start(temp.CostDocument);
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
                            // Remove cost object and add it with no document
                            if (ShareObjectFinalValue.RemoveCost(temp.CostDate) &&
                                ShareObjectFinalValue.AddCost(false, false, strDateTime, temp.CostValue))
                            {
                                // Set flag to save the share object.
                                SaveFlag = true;

                                ResetInputValues();
                                ShowCosts();

                                // Add status message
                                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormCost/StateMessages/EditSuccess", LanguageName),
                                    Language, LanguageName,
                                    Color.Black, Logger, (int)FrmMain.EStateLevels.Info,
                                    (int)FrmMain.EComponentLevels.Application);
                            }
                            else
                            {
                                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                                    Language.GetLanguageTextByXPath(
                                        @"/AddEditFormCost/Errors/EditFailed", LanguageName),
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
#if DEBUG_COST || DEBUG
                var message = $"dataGridViewCostsOfAYear_CellContentdecimalClick()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessageCostEdit,
                    Language.GetLanguageTextByXPath(@"/AddEditFormCost/Errors/DocumentShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError,
                    (int)FrmMain.EComponentLevels.Application);
            }
        }

        #endregion Costs of a year

        #endregion Data grid view

        #endregion Methods

    }
}
