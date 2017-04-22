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
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using LanguageHandler;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Taxes;
using SharePortfolioManager.Properties;

namespace SharePortfolioManager.Forms.DividendForm
{
    public partial class ShareDividendTaxesEdit : Form
    {
        #region Variables

        /// <summary>
        /// Stores the values for the taxes
        /// </summary>
        private Taxes _taxValues;

        /// <summary>
        /// Stores the values for the taxes for the cancel
        /// </summary>
        private Taxes _taxValuesOld;

        /// <summary>
        /// Stores the reference of the tax values
        /// </summary>
        private Taxes _taxValuesRef;

        /// <summary>
        /// Stores the logger
        /// </summary>
        private Logger _logger;

        /// <summary>
        /// Stores the given language file
        /// </summary>
        private Language _xmlLanguage;

        /// <summary>
        /// Stores the given language
        /// </summary>
        private string _languageName;

        /// <summary>
        /// Stores if a tax values should be load from a dividend
        /// </summary>
        private bool _bLoadTaxValuesFlag;

        /// <summary>
        /// Stores if the tax at source percentage value has been changed
        /// </summary>
        private bool _bEditTaxAtSourcePercentage;

        /// <summary>
        /// Stores if the capital gains tax percentage value has been changed
        /// </summary>
        private bool _bEditCapitalGainsTaxPercentage;

        /// <summary>
        /// Stores if the solidarity tax percentage value has been changed
        /// </summary>
        private bool _bEditSolidarityTaxPercentage;

        #endregion Variables

        #region Properties

        public Taxes TaxValues
        {
            get { return _taxValues; }
            internal set { _taxValues = value; }
        }

        public Taxes TaxValuesOld
        {
            get { return _taxValuesOld; }
            internal set { _taxValuesOld = value; }
        }

        public Taxes TaxValuesRef
        {
            get { return _taxValuesRef; }
            internal set { _taxValuesRef = value; }
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

        public bool LoadTaxValuesFlag
        {
            get { return _bLoadTaxValuesFlag; }
            internal set { _bLoadTaxValuesFlag = value; }
        }

        public bool EditTaxAtSourcePercentageFlag
        {
            get { return _bEditTaxAtSourcePercentage; }
            internal set { _bEditTaxAtSourcePercentage = value; }
        }

        public bool EditCapitalGainsTaxPercentageFlag
        {
            get { return _bEditCapitalGainsTaxPercentage; }
            internal set { _bEditCapitalGainsTaxPercentage = value; }
        }

        public bool EditSolidarityTaxPercentageFlag
        {
            get { return _bEditSolidarityTaxPercentage; }
            internal set { _bEditSolidarityTaxPercentage = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// This function is the construtor of the dialog
        /// </summary>
        /// <param name="taxValues"></param>
        /// <param name="logger"></param>
        /// <param name="xmlLanguage"></param>
        /// <param name="language"></param>
        public ShareDividendTaxesEdit(Taxes taxValues, Logger logger, Language xmlLanguage, string language)
        {
            InitializeComponent();

            TaxValues = taxValues;
            TaxValuesOld = new Taxes();
            TaxValuesOld.DeepCopy(taxValues);

            Logger = logger;
            XmlLanguage = xmlLanguage;
            LanguageName = language;
        }

        /// <summary>
        /// This function sets the language to the labels and buttons
        /// and sets the units behind the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareDividendTaxesEdit_Load(object sender, EventArgs e)
        {
            try
            {
                LoadTaxValuesFlag = true;

                EditTaxAtSourcePercentageFlag = false;
                EditCapitalGainsTaxPercentageFlag = false;
                EditSolidarityTaxPercentageFlag = false;

                Text = XmlLanguage.GetLanguageTextByXPath(@"/Taxes/Caption", LanguageName);
                grpBoxAddTaxes.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Caption", LanguageName);
                lblAddTaxPayout.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/Payout",
                        LanguageName);
                lblAddTaxAtSource.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/TaxAtSource",
                        LanguageName);
                lblAddTaxAtSourceValue.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/TaxAtSourceValue",
                        LanguageName);
                lblAddTaxAtSourcePercentage.Text = ShareObject.PercentageUnit;
                lblAddCapitalGainsValue.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/CapitalGainsValue",
                        LanguageName);
                lblAddTaxLossBalance.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/LossBalance",
                        LanguageName);
                lblAddTaxCapitalTainsTax.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/CapitalGainsTax",
                        LanguageName);
                lblAddTaxCapitalTainsTaxValues.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/CapitalGainsTaxValue",
                        LanguageName);
                lblAddTaxCapitalGainsTaxPercentage.Text = ShareObject.PercentageUnit;
                lblAddTaxSolidarityTax.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/SolidarityTax",
                        LanguageName);
                lblAddTaxSolidarityTaxValues.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/SolidarityTaxValue",
                        LanguageName);
                lblAddTaxSolidarityTaxPercentage.Text = ShareObject.PercentageUnit;
                lblAddTaxDividendPayoutWithTaxesValues.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Labels/PayoutWithTaxes",
                        LanguageName);
                btnAddOk.Text = XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Buttons/Ok",
                    LanguageName);
                btnReset.Text = XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Buttons/Reset",
                    LanguageName);
                btnCancel.Text =
                    XmlLanguage.GetLanguageTextByXPath(@"/Taxes/GrpBoxEdit/Buttons/Cancel",
                        LanguageName);

                // Load button images
                btnAddOk.Image = Resources.black_save;
                btnReset.Image = Resources.black_cancel;
                btnCancel.Image = Resources.black_cancel;

                // Set tax value to the edit boxes
                SetTaxValuesToEditBoxes();

                LoadTaxValuesFlag = false;

                // Set tax values
                UpdateTaxValues();

                // Set tax value to the edit boxes
                SetTaxValuesToEditBoxes();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ShareDividendTaxesEdit_Load()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Add status message
                Helper.AddStatusMessage(toolStripStatusLabelMessage,
                   XmlLanguage.GetLanguageTextByXPath(@"/Taxes/Errors/ShowFailed", LanguageName),
                   XmlLanguage, LanguageName,
                   Color.DarkRed, Logger, (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application);
            }
        }

        /// <summary>
        /// This function checks if the check box state has been changed
        /// and does the enable or disable controls
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">EventArgs</param>
        private void chkBoxTaxAtSource_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxTaxAtSource.CheckState == CheckState.Checked)
            {
                txtBoxAddTaxAtSourcePercentage.Enabled = true;
                txtBoxAddTaxAtSourcePercentage.ReadOnly = false;
            }
            else
            {
                txtBoxAddTaxAtSourcePercentage.Enabled = false;
                txtBoxAddTaxAtSourcePercentage.ReadOnly = true;                
            }

            //EditTaxAtSourcePercentageFlag = true;

            // Set tax values
            UpdateTaxValues();

            // Set tax value to the edit boxes
            SetTaxValuesToEditBoxes();

            //EditTaxAtSourcePercentageFlag = false;
        }

        /// <summary>
        /// This function calculates the payout with taxes values new
        /// when the text has been changed
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddTaxAtSourcePercentage_TextChanged(object sender, EventArgs e)
        {
            EditTaxAtSourcePercentageFlag = true;

            // Set tax values
            UpdateTaxValues();
            // Set tax value to the edit boxes
            SetTaxValuesToEditBoxes();

            EditTaxAtSourcePercentageFlag = false;
        }

        /// <summary>
        /// This function checks if the check box state has been changed
        /// and does the enable or disable controls
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">EventArgs</param>
        private void chkBoxCapitalGainsTax_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxCapitalGainsTax.CheckState == CheckState.Checked)
            {
                txtBoxAddTaxCapitalGainsTaxPercentage.Enabled = true;
                txtBoxAddTaxCapitalGainsTaxPercentage.ReadOnly = false;
            }
            else
            {
                txtBoxAddTaxCapitalGainsTaxPercentage.Enabled = false;
                txtBoxAddTaxCapitalGainsTaxPercentage.ReadOnly = true;
            }

            //EditCapitalGainsTaxPercentageFlag = true;

            // Set tax values
            UpdateTaxValues();

            // Set tax value to the edit boxes
            SetTaxValuesToEditBoxes();

            //EditCapitalGainsTaxPercentageFlag = false;
        }

        /// <summary>
        /// This function calculates the payout with taxes values new
        /// when the text has been changed
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddTaxCapitalGainsTaxPercentage_TextChanged(object sender, EventArgs e)
        {
            EditCapitalGainsTaxPercentageFlag = true;

            // Set tax values
            UpdateTaxValues();
            // Set tax value to the edit boxes
            SetTaxValuesToEditBoxes();

            EditCapitalGainsTaxPercentageFlag = false;
        }

        /// <summary>
        /// This function checks if the check box state has been changed
        /// and does the enable or disable controls
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">EventArgs</param>
        private void chkBoxSolidarityTax_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxSolidarityTax.CheckState == CheckState.Checked)
            {
                txtBoxAddTaxSolidarityTaxPercentage.Enabled = true;
                txtBoxAddTaxSolidarityTaxPercentage.ReadOnly = false;
            }
            else
            {
                txtBoxAddTaxSolidarityTaxPercentage.Enabled = false;
                txtBoxAddTaxSolidarityTaxPercentage.ReadOnly = true;
            }

            //EditSolidarityTaxPercentageFlag = true;

            // Set tax values
            UpdateTaxValues();

            // Set tax value to the edit boxes
            SetTaxValuesToEditBoxes();

            //EditSolidarityTaxPercentageFlag = false;
        }

        /// <summary>
        /// This function calculates the payout with taxes values new
        /// when the text has been changed
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxAddTaxSolidarityTaxPercentage_TextChanged(object sender, EventArgs e)
        {
            EditSolidarityTaxPercentageFlag = true;

            // Set tax values
            UpdateTaxValues();
            // Set tax value to the edit boxes
            SetTaxValuesToEditBoxes();

            EditSolidarityTaxPercentageFlag = false;
        }

        private void OnBtnAddOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This function close the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            TaxValues.DeepCopy(TaxValuesOld);
            Close();
        }

        /// <summary>
        /// This function updates the tax values
        /// </summary>
        private void UpdateTaxValues()
        {
            // Check if the default values should be loaded so do not update values
            if (!LoadTaxValuesFlag)
            {
                decimal decTaxAtSourcePercentage = 0;
                decimal decCapitalGainsTaxPercentage = 0;
                decimal decSolidarityTaxPercentage = 0;

                // Check tax flag
                TaxValues.TaxAtSourceFlag = chkBoxTaxAtSource.Checked;
                TaxValues.CapitalGainsTaxFlag = chkBoxCapitalGainsTax.Checked;
                TaxValues.SolidarityTaxFlag = chkBoxSolidarityTax.Checked;

                // Set percentage values
                if (TaxValues.TaxAtSourceFlag)
                {
                    if (!decimal.TryParse(txtBoxAddTaxAtSourcePercentage.Text, out decTaxAtSourcePercentage))
                        TaxValues.TaxAtSourcePercentage = 0;
                    else
                        TaxValues.TaxAtSourcePercentage = decTaxAtSourcePercentage;
                }
                else
                    TaxValues.TaxAtSourcePercentage = 0;

                if (TaxValues.CapitalGainsTaxFlag)
                {
                    if (!decimal.TryParse(txtBoxAddTaxCapitalGainsTaxPercentage.Text, out decCapitalGainsTaxPercentage))
                        TaxValues.CapitalGainsTaxPercentage = 0;
                    else
                        TaxValues.CapitalGainsTaxPercentage = decCapitalGainsTaxPercentage;
                }
                else
                    TaxValues.CapitalGainsTaxPercentage = 0;

                if (TaxValues.SolidarityTaxFlag)
                {
                    if (!decimal.TryParse(txtBoxAddTaxSolidarityTaxPercentage.Text, out decSolidarityTaxPercentage))
                        TaxValues.SolidarityTaxPercentage = 0;
                    else
                        TaxValues.SolidarityTaxPercentage = decSolidarityTaxPercentage;
                }
                else
                    TaxValues.SolidarityTaxPercentage = 0;
            }
        }

        /// <summary>
        /// This function sets the tax values to the edit boxes
        /// </summary>
        private void SetTaxValuesToEditBoxes()
        {
            // Set dividend units to the edit boxes
            lblPayoutUnit.Text = TaxValues.CurrencyUnit;
            lblTaxAtSourceUnit.Text = TaxValues.CurrencyUnit;
            lblCapitalGainsValueUnit.Text = TaxValues.CurrencyUnit;
            lblLossBalanceUnit.Text = TaxValues.CurrencyUnit;
            lblCapiatalGainsTaxUnit.Text = TaxValues.CurrencyUnit;
            lblSolidarityTaxUnit.Text = TaxValues.CurrencyUnit;
            lblPayoutWithTaxesUnit.Text = TaxValues.CurrencyUnit;

            lblPayoutFCUnit.Text = TaxValues.FCUnit;
            lblTaxAtSourceFCUnit.Text = TaxValues.FCUnit;
            lblCapitalGainsValueFCUnit.Text = TaxValues.FCUnit;
            lblCapiatalGainsTaxFCUnit.Text = TaxValues.FCUnit;
            lblSolidarityTaxFCUnit.Text = TaxValues.FCUnit;
            lblPayoutWithTaxesFCUnit.Text = TaxValues.FCUnit;

            // Set edit tax values
            if (TaxValues.ValueWithoutTaxes == decimal.MinValue / 2)
                txtBoxAddTaxPayoutValue.Text = @"-";
            else
                txtBoxAddTaxPayoutValue.Text = TaxValues.ValueWithoutTaxesAsString;

            if (TaxValues.ValueWithoutTaxesFC == decimal.MinValue / 2)
                txtBoxAddTaxPayoutValueFC.Text = @"-";
            else
                txtBoxAddTaxPayoutValueFC.Text = TaxValues.ValueWithoutTaxesFCAsString;

            // Checkbox states
            if (TaxValues.TaxAtSourceFlag)
            {
                chkBoxTaxAtSource.CheckState = CheckState.Checked;

                if (!EditTaxAtSourcePercentageFlag)
                    txtBoxAddTaxAtSourcePercentage.Text = TaxValues.TaxAtSourcePercentageAsString;

                txtBoxAddTaxAtSourceValue.Text = TaxValues.TaxAtSourceValueAsString;
                txtBoxAddTaxAtSourceValueFC.Text = TaxValues.TaxAtSourceValueFCAsString;
            }
            else
            {
                chkBoxTaxAtSource.CheckState = CheckState.Unchecked;

                if (!EditTaxAtSourcePercentageFlag)
                    txtBoxAddTaxAtSourcePercentage.Text = TaxValues.TaxAtSourcePercentageAsString;

                txtBoxAddTaxAtSourceValue.Text = TaxValues.TaxAtSourceValueAsString;
                txtBoxAddTaxAtSourceValueFC.Text = TaxValues.TaxAtSourceValueFCAsString;
            }

            txtBoxAddCapitalGainsValue.Text = TaxValues.ValueCapitalGainsAsString;
            txtBoxAddCapitalGainsValueFC.Text = TaxValues.ValueCapitalGainsFCAsString;

            // Set loss balance value
            txtBoxAddTaxLossBalanceValue.Text = TaxValues.LossBalanceAsString;

            if (TaxValues.CapitalGainsTaxFlag)
            {
                chkBoxCapitalGainsTax.CheckState = CheckState.Checked;

                chkBoxSolidarityTax.Enabled = true;

                if (!EditCapitalGainsTaxPercentageFlag)
                    txtBoxAddTaxCapitalGainsTaxPercentage.Text = TaxValues.CapitalGainsTaxPercentageAsString;

                txtBoxAddTaxCaptialGainsTaxValue.Text = TaxValues.CapitalGainsTaxValueAsString;
                txtBoxAddTaxCaptialGainsTaxValueFC.Text = TaxValues.CapitalGainsTaxValueFCAsString;
            }
            else
            {
                chkBoxCapitalGainsTax.CheckState = CheckState.Unchecked;

                chkBoxSolidarityTax.CheckState = CheckState.Unchecked;
                chkBoxSolidarityTax.Enabled = false;

                if (!EditCapitalGainsTaxPercentageFlag)
                    txtBoxAddTaxCapitalGainsTaxPercentage.Text = TaxValues.CapitalGainsTaxPercentageAsString;

                txtBoxAddTaxCaptialGainsTaxValue.Text = TaxValues.CapitalGainsTaxValueAsString;
                txtBoxAddTaxCaptialGainsTaxValueFC.Text = TaxValues.CapitalGainsTaxValueFCAsString;
            }

            if (TaxValues.SolidarityTaxFlag)
            {
                chkBoxSolidarityTax.CheckState = CheckState.Checked;

                if(!EditSolidarityTaxPercentageFlag)
                    txtBoxAddTaxSolidarityTaxPercentage.Text = TaxValues.SolidarityTaxPercentageAsString;

                txtBoxAddTaxSolidarityTaxValue.Text = TaxValues.SolidarityTaxValueAsString;
                txtBoxAddTaxSolidarityTaxValueFC.Text = TaxValues.SolidarityTaxValueFCAsString;
            }
            else
            {
                chkBoxSolidarityTax.CheckState = CheckState.Unchecked;

                if (!EditSolidarityTaxPercentageFlag)
                    txtBoxAddTaxSolidarityTaxPercentage.Text = TaxValues.SolidarityTaxPercentageAsString;

                txtBoxAddTaxSolidarityTaxValue.Text = TaxValues.SolidarityTaxValueAsString;
                txtBoxAddTaxSolidarityTaxValueFC.Text = TaxValues.SolidarityTaxValueFCAsString;
            }

            txtBoxAddTaxPayoutWithTaxes.Text = TaxValues.ValueWithTaxesAsString;
            txtBoxAddTaxPayoutWithTaxesFC.Text = TaxValues.ValueWithTaxesFCAsString;
        }

        #endregion Methods
    }
}
