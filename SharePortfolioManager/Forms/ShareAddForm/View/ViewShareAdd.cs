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
        /// Stores language
        /// </summary>
        private String _languageName;

        /// <summary>
        /// Stores the RegEx list
        /// </summary>
        private List<WebSiteRegex> _webSiteRegexList;

        /// <summary>
        /// Flag if the form should be closed
        /// </summary>
        private bool _stopFomClosing;

        /// <summary>
        /// Stores the current error code of the form
        /// </summary>
        private ShareAddErrorCode _errorCode;

        #endregion Fields

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

        public bool StopFomClosingFlag
        {
            get { return _stopFomClosing; }
            set { _stopFomClosing = value; }
        }

        #endregion Properties

        #region IViewMember

        new

        DialogResult ShowDialog
        {
            get
            {
                return base.ShowDialog();
            }
        }

        public ShareAddErrorCode ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get { return ParentWindow.ShareObjectMarketValue; }
            set { ParentWindow.ShareObjectMarketValue = value; }
        }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue
        {
            get { return ParentWindow.ShareObjectListMarketValue; }
            set { ParentWindow.ShareObjectListMarketValue = value; }
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get { return ParentWindow.ShareObjectFinalValue; }
            set { ParentWindow.ShareObjectFinalValue = value; }
        }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue
        {
            get { return ParentWindow.ShareObjectListFinalValue; }
            set { ParentWindow.ShareObjectListFinalValue = value; }
        }

        public List<Image> ImageList
        {
            get { return ParentWindow.ImageList; }
        }

        public List<WebSiteRegex> WebSiteRegexList
        {
            get { return _webSiteRegexList; }
            set { _webSiteRegexList = value; }
        }

        #region Input values

        public string Wkn
        {
            get { return txtBoxWkn.Text; }
            set
            {
                if (txtBoxWkn.Text == value)
                    return;
                txtBoxWkn.Text = value;
            }
        }

        public string Date
        {
            get { return datePickerDate.Text; }
            set
            {
                if (datePickerDate.Text == value)
                    return;
                datePickerDate.Text = value;
            }
        }

        public string Time
        {
            get { return datePickerTime.Text; }
            set
            {
                if (datePickerTime.Text == value)
                    return;
                datePickerTime.Text = value;
            }
        }

        public string ShareName
        {
            get { return txtBoxName.Text; }
            set
            {
                if (txtBoxName.Text == value)
                    return;
                txtBoxName.Text = value;
            }
        }

        public string Volume
        {
            get { return txtBoxVolume.Text; }
            set
            {
                if (txtBoxVolume.Text == value)
                    return;
                txtBoxVolume.Text = value;
            }
        }

        public string SharePrice
        {
            get { return txtBoxSharePrice.Text; }
            set
            {
                if (txtBoxSharePrice.Text == value)
                    return;
                txtBoxSharePrice.Text = value;
            }
        }

        public string MarketValue
        {
            get { return txtBoxMarketValue.Text; }
            set
            {
                if (txtBoxMarketValue.Text == value)
                    return;
                txtBoxMarketValue.Text = value;
            }
        }

        public string Costs
        {
            get { return txtBoxCosts.Text; }
            set
            {
                if (txtBoxCosts.Text == value)
                    return;
                txtBoxCosts.Text = value;
            }
        }

        public string Reduction
        {
            get { return txtBoxReduction.Text; }
            set
            {
                if (txtBoxReduction.Text == value)
                    return;
                txtBoxReduction.Text = value;
            }
        }

        public string GrandTotal
        {
            get { return txtBoxFinalValue.Text; }
            set
            {
                if (txtBoxFinalValue.Text == value)
                    return;
                txtBoxFinalValue.Text = value;
            }
        }

        public string WebSite
        {
            get { return txtBoxWebSite.Text; }
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
                string cultureName = cboBoxCultureInfo.SelectedItem.ToString();
                return Helper.GetCultureByName(cultureName);
            }
        }

        public int DividendPayoutInterval
        {
            get { return cbxDividendPayoutInterval.SelectedIndex; }
            set
            {
                if (cbxDividendPayoutInterval.SelectedIndex == value)
                    return;
                cbxDividendPayoutInterval.SelectedIndex = value;
            }
        }

        public int ShareType
        {
            get { return cbxShareType.SelectedIndex; }
            set
            {
                if (cbxShareType.SelectedIndex == value)
                    return;
                cbxShareType.SelectedIndex = value;
            }
        }

        public string Document
        {
            get { return txtBoxDocument.Text; }
            set
            {
                if (txtBoxDocument.Text == value)
                    return;
                txtBoxDocument.Text = value;
            }
        }

        #endregion Input values

        // TODO
        public void AddFinish()
        {
            // Set messages
            string strMessage = @"";
            Color clrMessage = Color.Black;
            FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            StopFomClosingFlag = true;

            // Enable controls
            this.Enabled = true;

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
                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item0",
                        LanguageName));
                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item1",
                        LanguageName));
                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item2",
                        LanguageName));
                cbxDividendPayoutInterval.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item3",
                        LanguageName));

                lblShareType.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/ShareType", LanguageName);
                cbxShareType.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsShareType/Item0",
                        LanguageName));
                cbxShareType.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsShareType/Item1",
                        LanguageName));
                cbxShareType.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsShareType/Item2",
                        LanguageName));
                cbxShareType.Items.Add(
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsShareType/Item3",
                        LanguageName));

                lblWebSite.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/WebSite", LanguageName);
                lblCultureInfo.Text =
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/CultureInfo", LanguageName);
                lblDocument.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Document", LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Save", LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Cancel", LanguageName);

                #endregion Language configuration

                #region Get culture info

                List<string> list = new List<string>();
                foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    list.Add(string.Format("{0}", ci.Name));
                }

                list.Sort();

                foreach (var value in list)
                {
                    if (value != "")
                        cboBoxCultureInfo.Items.Add(value);
                }

                CultureInfo cultureInfo = CultureInfo.CurrentCulture;

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
#if DEBUG
                MessageBox.Show("FrmShareAdd_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
                string strFilter = "pdf (*.pdf)|*.pdf|txt (*.txt)|.txt|doc (*.doc)|.doc|docx (*.docx)|.docx";
                txtBoxDocument.Text = Helper.SetDocument(Language.GetLanguageTextByXPath(@"/AddFormShare/OpenFileDialog/Title", LanguageName), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnShareDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
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
                this.Enabled = false;

                if (ShareAdd != null)
                    ShareAdd(this, new EventArgs());
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnSave_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                StopFomClosingFlag = true;
                // Add status message
                Helper.AddStatusMessage(addShareStatusLabelMessage,
                    Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/AddSaveFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

                // Enable controls
                this.Enabled = true;
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

        private void datePickerDate_ValueChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Date"));
        }

        private void datePickerTime_ValueChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Time"));
        }

        #endregion Date / Time

        #region TextBoxes

        private void OnTxtBoxWkn_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Wkn"));
        }

        private void OnTxtBoxName_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
        }

        private void OnTxtBoxVolume_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Volume"));
        }

        private void OnTxtBoxVolume_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        private void OnTxtBoxSharePrice_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("SharePrice"));
        }

        private void OnTxtBoxSharePrice_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        private void OnTxtBoxCosts_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Costs"));
        }

        private void OnTxtBoxCosts_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        private void OnTxtBoxReduction_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Reduction"));
        }

        private void OnTxtBoxReduction_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        private void OnTxtBoxWebSite_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("WebSite"));
        }

        private void OnTxtBoxDocument_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Document"));
        }

        private void OnTxtBoxDocument_Leave(object sender, EventArgs e)
        {
            if (FormatInputValues != null)
                FormatInputValues(this, new EventArgs());
        }

        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in files)
                {
                    txtBoxDocument.Text = filePath.ToString();
                }
            }
        }

        #endregion TextBoxes

        #region ComboBoxes

        private void cboBoxCultureInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("CultureInfo"));
        }

        private void cbxDividendPayoutInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("DividendPayoutInterval"));
        }

        private void cbxShareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("ShareType"));
        }

        #endregion Comboboxes

    }
}
