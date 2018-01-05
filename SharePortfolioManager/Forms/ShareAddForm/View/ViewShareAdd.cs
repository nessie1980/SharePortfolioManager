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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Forms.ShareAddForm.View
{
    public enum ShareAddErrorCode
    {
        AddSuccessful,
        AddFailed,
        InputeValuesInvalid,
        WknEmpty,
        WknExists,
        NameEmpty,
        NameExists,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        SharePriceEmpty,
        SharePriceWrongFormat,
        SharePriceWrongValue,
        CostsWrongFormat,
        CostsWrongValue,
        ReductionWrongFormat,
        ReductionWrongValue,
        WebSiteEmpty,
        WebSiteWrongFormat,
        WebSiteExists,
        DocumentDirectoryDoesNotExists,
        DocumentFileDoesNotExists,
        WebSiteRegexNotFound
    };

    /// <inheritdoc />
    /// <summary>
    /// Interface of the ShareAdd view
    /// </summary>
    public interface IViewShareAdd : INotifyPropertyChanged
    {
        event EventHandler ShareAdd;
        event EventHandler FormatInputValues;

        ShareAddErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<ShareObjectFinalValue> ShareObjectListFinalValue { get; set; }

        List<Image> ImageList { get; }

        List<WebSiteRegex> WebSiteRegexList { get; }

        string Wkn { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string ShareName { get; set; }
        string Volume { get; set; }
        string SharePrice { get; set; }
        string MarketValue { get; set; }
        string Costs { get; set; }
        string Reduction { get; set; }
        string GrandTotal { get; set; }
        string WebSite { get; set; }
        CultureInfo CultureInfo { get; }
        int DividendPayoutInterval { get; set; }
        int ShareType { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddFinish();
    }

    public partial class ViewShareAdd : Form, IViewShareAdd
    {
        #region Fields

        #endregion Fields

        #region Properties

        public FrmMain ParentWindow { get; set; }

        public Logger Logger { get; set; }

        public Language Language { get; set; }

        public string LanguageName { get; set; }

        public bool StopFomClosingFlag { get; set; }

        #endregion Properties

        #region IViewMember

        public ShareAddErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get => ParentWindow.ShareObjectMarketValue;
            set => ParentWindow.ShareObjectMarketValue = value;
        }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue
        {
            get => ParentWindow.ShareObjectListMarketValue;
            set => ParentWindow.ShareObjectListMarketValue = value;
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get => ParentWindow.ShareObjectFinalValue;
            set => ParentWindow.ShareObjectFinalValue = value;
        }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue
        {
            get => ParentWindow.ShareObjectListFinalValue;
            set => ParentWindow.ShareObjectListFinalValue = value;
        }

        public List<Image> ImageList => ParentWindow.ImageList;

        public List<WebSiteRegex> WebSiteRegexList { get; set; }

        #region Input values

        public string Wkn
        {
            get => txtBoxWkn.Text;
            set
            {
                if (txtBoxWkn.Text == value)
                    return;
                txtBoxWkn.Text = value;
            }
        }

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

        public string ShareName
        {
            get => txtBoxName.Text;
            set
            {
                if (txtBoxName.Text == value)
                    return;
                txtBoxName.Text = value;
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

        public string SharePrice
        {
            get => txtBoxSharePrice.Text;
            set
            {
                if (txtBoxSharePrice.Text == value)
                    return;
                txtBoxSharePrice.Text = value;
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

        public string GrandTotal
        {
            get => txtBoxFinalValue.Text;
            set
            {
                if (txtBoxFinalValue.Text == value)
                    return;
                txtBoxFinalValue.Text = value;
            }
        }

        public string WebSite
        {
            get => txtBoxWebSite.Text;
            set
            {
                if (txtBoxWebSite.Text == value)
                    return;
                txtBoxWebSite.Text = value;
            }
        }

        public CultureInfo CultureInfo
        {
            get
            {
                var cultureName = cboBoxCultureInfo.SelectedItem.ToString();
                return Helper.GetCultureByName(cultureName);
            }
        }

        public int DividendPayoutInterval
        {
            get => cbxDividendPayoutInterval.SelectedIndex;
            set
            {
                if (cbxDividendPayoutInterval.SelectedIndex == value)
                    return;
                cbxDividendPayoutInterval.SelectedIndex = value;
            }
        }

        public int ShareType
        {
            get => cbxShareType.SelectedIndex;
            set
            {
                if (cbxShareType.SelectedIndex == value)
                    return;
                cbxShareType.SelectedIndex = value;
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

        public void AddFinish()
        {
            // Set messages
            var strMessage = @"";
            var clrMessage = Color.Black;
            var stateLevel = FrmMain.EStateLevels.Info;

            StopFomClosingFlag = true;

            // Enable controls
            Enabled = true;

            switch (ErrorCode)
            {
                case ShareAddErrorCode.AddSuccessful:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/AddSuccess", LanguageName);

                        StopFomClosingFlag = false;
                        break;
                    }
                case ShareAddErrorCode.AddFailed:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.WknEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WKNEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWkn.Focus();
                        break;
                    }
                case ShareAddErrorCode.WknExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WKNExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWkn.Focus();
                        break;
                    }
                case ShareAddErrorCode.NameEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/NameEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxName.Focus();
                        break;
                    }
                case ShareAddErrorCode.NameExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/NameExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxName.Focus();
                        break;
                    }
                case ShareAddErrorCode.VolumeEmpty:
                    {
                        strMessage =
                             Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.SharePriceEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxSharePrice.Focus();
                        break;
                    }
                case ShareAddErrorCode.SharePriceWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxSharePrice.Focus();
                        break;
                    }
                case ShareAddErrorCode.SharePriceWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxSharePrice.Focus();
                        break;
                    }
                case ShareAddErrorCode.CostsWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/CostsWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxCosts.Focus();
                        break;
                    }
                case ShareAddErrorCode.CostsWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/CostsWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxCosts.Focus();
                        break;
                    }
                case ShareAddErrorCode.ReductionWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ReductionWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxReduction.Focus();
                        break;
                    }
                case ShareAddErrorCode.ReductionWrongValue:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ReductionWrongValue", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxReduction.Focus();
                        break;
                    }
                case ShareAddErrorCode.WebSiteEmpty:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WebSiteEmpty", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWebSite.Focus();
                        break;
                    }
                case ShareAddErrorCode.WebSiteWrongFormat:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WebSiteWrongFormat", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWebSite.Focus();
                        break;
                    }
                case ShareAddErrorCode.WebSiteExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WebSiteExists", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWebSite.Focus();
                        break;
                    }
                case ShareAddErrorCode.DocumentDirectoryDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DirectoryDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
                case ShareAddErrorCode.DocumentFileDoesNotExists:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/FileDoesNotExist", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
                case ShareAddErrorCode.WebSiteRegexNotFound:
                    {
                        strMessage =
                            Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WebSiteRegexNotFound", LanguageName);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
            }

            Helper.AddStatusMessage(addShareStatusLabelMessage,
               strMessage,
               Language,
               LanguageName,
               clrMessage,
               Logger,
               (int)stateLevel,
               (int)FrmMain.EComponentLevels.Application);
        }

        #endregion IViewMember

        #region Event Members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValues;
        public event EventHandler ShareAdd;

        #endregion Event Members

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewShareAdd(FrmMain parentWindow, Logger logger, Language language, String strLanguage, List<WebSiteRegex> webSiteRegexList)
        {
            InitializeComponent();

            ParentWindow = parentWindow;
            Logger = logger;
            Language = language;
            LanguageName = strLanguage;
            WebSiteRegexList = webSiteRegexList;
            StopFomClosingFlag = false;
        }

        /// <summary>
        /// This function is called when the form is closing
        /// Here we check if the form really should be closed
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void FrmShareAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the form should be closed
            if (StopFomClosingFlag)
            {
                // Stop closing the form
                e.Cancel = true;
            }

            // Reset closing flag
            StopFomClosingFlag = false;
        }

        /// <summary>
        /// This function creates the share add form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmShareAdd_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Caption", LanguageName);
                grpBoxGeneral.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Caption", LanguageName);
                lblWkn.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/WKN", LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Date", LanguageName);
                lblName.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Name", LanguageName);
                lblVolume.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Volume", LanguageName);
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblSharePrice.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/SharePrice", LanguageName);
                lblSharePriceUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblMarketValue.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/MarketValue", LanguageName);
                lblMarketValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblCosts.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Costs", LanguageName);
                lblCostsUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblReduction.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Reduction", LanguageName);
                lblReductionUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblFinalValue.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/FinalValue", LanguageName);
                lblFinalValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;

                lblDividendPayoutInterval.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/PayoutInterval", LanguageName);
                // Add dividend payout interval values
                cbxDividendPayoutInterval.Items.AddRange(Helper.GetComboBoxItmes(@"/ComboBoxItemsPayout/*", LanguageName, Language));
                lblShareType.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/ShareType", LanguageName);
                // Add share type values
                cbxShareType.Items.AddRange(Helper.GetComboBoxItmes(@"/ComboBoxItemsShareType/*", LanguageName, Language));

                lblWebSite.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/WebSite", LanguageName);
                lblCultureInfo.Text =
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/CultureInfo", LanguageName);
                lblDocument.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Document", LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Save", LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Cancel", LanguageName);

                #endregion Language configuration

                #region Get culture info

                var list = new List<string>();
                foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    list.Add($"{ci.Name}");
                }

                list.Sort();

                foreach (var value in list)
                {
                    if (value != "")
                        cboBoxCultureInfo.Items.Add(value);
                }

                var cultureInfo = CultureInfo.CurrentCulture;

                cboBoxCultureInfo.SelectedIndex = cboBoxCultureInfo.FindStringExact(cultureInfo.Name);

                #endregion Get culture info

                // Load button images
                btnSave.Image = Resources.black_save;
                btnCancel.Image = Resources.black_cancel;

                cbxDividendPayoutInterval.SelectedIndex = 0;
                cbxShareType.SelectedIndex = 0;

                datePickerDate.Value = DateTime.Now;
                datePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            }
            catch (Exception ex)
            {
#if DEBUG_ADDSHARE || DEBUG
                var message = $"FrmShareAdd_Load()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(addShareStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/AddShowFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function set the focus on the WKN number edit box
        /// when the form is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmShareAdd_Shown(object sender, EventArgs e)
        {
            txtBoxWkn.Focus();
        }

        #endregion Form

        #region Button

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the buy of the share
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareDocumentBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                const string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                txtBoxDocument.Text = Helper.SetDocument(Language.GetLanguageTextByXPath(@"/AddFormShare/OpenFileDialog/Title", LanguageName), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG_ADDSHARE || DEBUG
                var message = $"btnShareDocumentBrowse_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(addShareStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ChoseDocumentFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function stores the values to the share object
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls
                Enabled = false;

                ShareAdd?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
#if DEBUG_ADDSHARE || DEBUG
                var message = $"btnSave_Click()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                StopFomClosingFlag = true;
                // Add status message
                Helper.AddStatusMessage(addShareStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/AddSaveFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                // Enable controls
                Enabled = true;
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

        #endregion Button

        #region Date / Time

        private void DatePickerDate_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
        }

        private void DatePickerTime_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
        }

        #endregion Date / Time

        #region TextBoxes

        private void OnTxtBoxWkn_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wkn"));
        }

        private void OnTxtBoxName_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
        }

        private void OnTxtBoxVolume_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Volume"));
        }

        private void OnTxtBoxVolume_Leave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        private void OnTxtBoxSharePrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SharePrice"));
        }

        private void OnTxtBoxSharePrice_Leave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        private void OnTxtBoxCosts_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Costs"));
        }

        private void OnTxtBoxCosts_Leave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        private void OnTxtBoxReduction_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reduction"));
        }

        private void OnTxtBoxReduction_Leave(object sender, EventArgs e)
        {
            FormatInputValues?.Invoke(this, new EventArgs());
        }

        private void OnTxtBoxWebSite_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WebSite"));
        }

        private void OnTxtBoxDocument_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Document"));
        }

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

        #region ComboBoxes

        private void CboBoxCultureInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CultureInfo"));
        }

        private void CbxDividendPayoutInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DividendPayoutInterval"));
        }

        private void CbxShareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShareType"));
        }

        #endregion ComboBoxes

    }
}
