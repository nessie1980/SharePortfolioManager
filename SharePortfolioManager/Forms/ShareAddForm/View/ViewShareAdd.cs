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
        DocumentDoesNotExists,
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

        List<ShareObject> ShareObjectList { get; set; }
        ShareObject ShareObject { get; set; }

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
        private Language _xmlLanguage;

        /// <summary>
        /// Stores language
        /// </summary>
        private String _strLanguage;

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

        public List<ShareObject> ShareObjectList
        {
            get { return _parentWindow.ShareObjectList; }
            set { _parentWindow.ShareObjectList = value; }
        }

        public ShareObject ShareObject
        {
            get { return _parentWindow.ShareObject; }
            set { _parentWindow.ShareObject = value; }
        }

        public List<Image> ImageList
        {
            get { return _parentWindow.ImageList; }
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

            _stopFomClosing = true;

            // Enable controls
            this.Enabled = true;

            switch (ErrorCode)
            {
                case ShareAddErrorCode.AddSuccessful:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/StateMessages/AddSuccess", _strLanguage);

                        _stopFomClosing = false;
                        break;
                    }
                case ShareAddErrorCode.AddFailed:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddEditFormBuy/Errors/AddFailed", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.WknEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/WKNEmpty", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWkn.Focus();
                        break;
                    }
                case ShareAddErrorCode.WknExists:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/WKNExists", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWkn.Focus();
                        break;
                    }
                case ShareAddErrorCode.NameEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/NameEmpty", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxName.Focus();
                        break;
                    }
                case ShareAddErrorCode.NameExists:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/NameExists", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxName.Focus();
                        break;
                    }
                case ShareAddErrorCode.VolumeEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeEmpty", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.VolumeWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.VolumeWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxVolume.Focus();
                        break;
                    }
                case ShareAddErrorCode.SharePriceEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceEmpty", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxSharePrice.Focus();
                        break;
                    }
                case ShareAddErrorCode.SharePriceWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxSharePrice.Focus();
                        break;
                    }
                case ShareAddErrorCode.SharePriceWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxSharePrice.Focus();
                        break;
                    }
                case ShareAddErrorCode.CostsWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/CostsWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxCosts.Focus();
                        break;
                    }
                case ShareAddErrorCode.CostsWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/CostsWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxCosts.Focus();
                        break;
                    }
                case ShareAddErrorCode.ReductionWrongFormat:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/ReductionWrongFormat", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxReduction.Focus();
                        break;
                    }
                case ShareAddErrorCode.ReductionWrongValue:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/ReductionWrongValue", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxReduction.Focus();
                        break;
                    }
                case ShareAddErrorCode.WebSiteEmpty:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/WebSiteEmpty", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        txtBoxWebSite.Focus();
                        break;
                    }
                case ShareAddErrorCode.DocumentDoesNotExists:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/FileDoesNotExist", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
                case ShareAddErrorCode.WebSiteRegexNotFound:
                    {
                        strMessage =
                            _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/WebSiteRegexNotFound", _strLanguage);
                        clrMessage = Color.Red;
                        stateLevel = FrmMain.EStateLevels.Error;
                        break;
                    }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessage,
               strMessage,
               _xmlLanguage,
               _strLanguage,
               clrMessage,
               _logger,
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
        public ViewShareAdd(FrmMain parentWindow, Logger logger, Language xmlLanguage, String strLanguage, List<WebSiteRegex> webSiteRegexList)
        {
            InitializeComponent();

            _parentWindow = parentWindow;
            _logger = logger;
            _xmlLanguage = xmlLanguage;
            _strLanguage = strLanguage;
            _webSiteRegexList = webSiteRegexList;
            _stopFomClosing = false;
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
            if (_stopFomClosing)
            {
                // Stop closing the form
                e.Cancel = true;
            }

            // Reset closing flag
            _stopFomClosing = false;
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

                Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Caption", _strLanguage);
                grpBoxGeneral.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Caption", _strLanguage);
                lblWkn.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/WKN", _strLanguage);
                lblDate.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Date", _strLanguage);
                lblName.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Name", _strLanguage);
                lblVolume.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Volume", _strLanguage);
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblSharePrice.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/SharePrice", _strLanguage);
                lblSharePriceUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblMarketValue.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/MarketValue", _strLanguage);
                lblMarketValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblCosts.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Costs", _strLanguage);
                lblCostsUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblReduction.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Reduction", _strLanguage);
                lblReductionUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblFinalValue.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/FinalValue", _strLanguage);
                lblFinalValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblDividendPayoutInterval.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/PayoutInterval", _strLanguage);
                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item0",
                        _strLanguage));
                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item1",
                        _strLanguage));
                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item2",
                        _strLanguage));
                cbxDividendPayoutInterval.Items.Add(
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/ComboBoxItemsPayout/Item3",
                        _strLanguage));

                lblWebSite.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/WebSite", _strLanguage);
                lblCultureInfo.Text =
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/CultureInfo", _strLanguage);
                lblDocument.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Document", _strLanguage);

                btnSave.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Save", _strLanguage);
                btnCancel.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Cancel", _strLanguage);

                #endregion Language configuration

                #region Get culture info

                List<string> list = new List<string>();
                foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    //string specName = "(none)";
                    //try
                    //{
                    //    specName = CultureInfo.CreateSpecificCulture(ci.Name).Name;
                    //}
                    //catch
                    //{ }

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
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/AddShowFailed", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                txtBoxDocument.Text = Helper.SetDocument(_xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/OpenFileDialog/Title", _strLanguage), strFilter, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("btnShareDocumentBrowse_Click()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/ChoseDocumentFailed", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
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
                _stopFomClosing = true;
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/AddFormShare/Errors/AddSaveFailed", _strLanguage),
                    _xmlLanguage, _strLanguage,
                    Color.DarkRed, _logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);

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
            _stopFomClosing = false;
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

        #endregion Comboboxes
    }
}
