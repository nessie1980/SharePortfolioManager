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
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using Logging;
using LanguageHandler;

namespace SharePortfolioManager.Classes
{
    static class Helper
    {
        #region Constants formatting

        /// <summary>
        /// Stores the max precision for a value
        /// </summary>
        public const int Maxprecision = 5;

        #region Date

        /// <summary>
        /// Stores the date time format
        /// </summary>
        public const string Datetimefullformat = "{0:dd/MM/yyyy HH:mm:ss}";
        public const string Datetimefullformat2 = "{0:dd/MM/yyyy 1:HH:mm:ss}";
        public const string Datefulltimeshortformat = "{0:dd/MM/yyyy HH:mm}";
        public const string Timefullformat = "{0:HH:mm:ss}";
        public const string Timeshortformat = "{0:HH:mm}";
        public const string Datefullformat = "{0:dd/MM/yyyy}";

        #endregion Date

        #region Currency

        /// <summary>
        /// Stores currency format (price, deposit, costs, buys...)
        /// </summary>
        public const int Currencyfivelength = 5;
        public const int Currencyfourlength = 4;
        public const int Currencythreelength = 3;
        public const int Currencytwolength = 2;
        public const int Currencyonelength = 1;

        public const int Currencyfourfixlength = 4;
        public const int Currencythreefixlength = 3;
        public const int Currencytwofixlength = 2;
        public const int Currencyonefixlength = 1;
        public const int Currencynonefixlength = 0;

        #endregion Currency

        #region Percentage

        /// <summary>
        /// Stores percentage format (performance...)
        /// </summary>
        public const int Percentagefivelength = 5;
        public const int Percentagefourlength = 4;
        public const int Percentagethreelength = 3;
        public const int Percentagetwolength = 2;
        public const int Percentageonelength = 1;

        public const int Percentagefourfixlength = 4;
        public const int Percentagethreefixlength = 3;
        public const int Percentagetwofixlength = 2;
        public const int Percentageonefixlength = 1;
        public const int Percentagenonefixlength = 0;

        #endregion Percentage

        #region Volume

        /// <summary>
        /// Stores volume format (pcs.)
        /// </summary>
        public const int Volumefivelength = 5;
        public const int Volumefourlength = 4;
        public const int Volumethreelength = 3;
        public const int Volumetwolength = 2;
        public const int Volumeonelength = 1;

        public const int Volumefourfixlength = 4;
        public const int Volumethreefixlength = 3;
        public const int Volumetwofixlength = 2;
        public const int Volumeonefixlength = 1;
        public const int Volumenonefixlength = 0;

        #endregion Volume

        #endregion Constants formatting

        #region Variables

        /// <summary>
        /// Stores the list of the names and the units of the currency's
        /// </summary>
        static private IEnumerable<KeyValuePair<string, string>> _listNameUnitCurrency;

        #endregion Variables

        #region Properties

        static public IEnumerable<KeyValuePair<string, string>> ListNameUnitCurrency
        {
            get { return _listNameUnitCurrency; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        static Helper()
        {
            // Create the name and currency unit list
            _listNameUnitCurrency = GetCurrencyList();
        }

        /// <summary>
        /// This function returns the version of the application
        /// </summary>
        /// <returns>Version of the application</returns>
        static public Version GetApplicationVersion()
        {
            Assembly assApp = typeof(FrmMain).Assembly;
            AssemblyName assAppName = assApp.GetName();
            Version verApp = assAppName.Version;

            return verApp;
        }

        #region Add status message to the text box and to the logger

        /// <summary>
        /// This function adds a state message to the top of the status message RichEditBox
        /// and to the logger if the logger is initialized
        /// </summary>
        /// <param name="showObject">Object where the message should be shown (RichTextBox / Label)</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="stateMessage">State message which should be added</param>
        /// <param name="language">Language</param>
        /// <param name="color">Color of the message in the RichEditBox if the logger is not initialized</param>
        /// <param name="logger">Logger for the logging</param>
        /// <param name="stateLevel">Level of the state (e.g. Info)</param>
        /// <param name="componentLevel">Level of the component (e.g. Application)</param>
        /// <returns>Flag if the add was successful</returns>
        static public bool AddStatusMessage(object showObject, string stateMessage, Language xmlLanguage, string strLanguage, Color color, Logger logger, int stateLevel, int componentLevel)
        {
            try
            {
                string stateMessageError = "";

                // Get type of the given show object
                var type = showObject.GetType();

                // Check if the given logger is initialized
                if (logger != null && logger.InitState == Logger.EInitState.Initialized)
                {
                    try
                    {
                        // Get color for the message
                        color = logger.GetColorOfStateLevel((Logger.ELoggerStateLevels)stateLevel);

                        // Add state message to the log entry list
                        logger.AddEntry(stateMessage, (Logger.ELoggerStateLevels)stateLevel, (Logger.ELoggerComponentLevels)componentLevel);
                    }
                    catch (LoggerException ex)
                    {
                        if (ex.LoggerState == Logger.ELoggerState.CleanUpLogFilesFailed)
                            stateMessageError = xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesFailed", strLanguage);
                    }   
                }

                // RichTextBox
                if (showObject.GetType() == typeof(RichTextBox))
                {
                    RichTextBox castControl = (RichTextBox)showObject;

                    // Create a temporary rich text box with no word wrap
                    RichTextBox tempControl = castControl;
                    tempControl.WordWrap = false;

                    // Add time stamp
                    stateMessage = string.Format(Timefullformat, DateTime.Now) + @" " + stateMessage + "\n";

                    // Check if the logger add failed
                    if (stateMessageError != "")
                        stateMessage += string.Format(Timefullformat, DateTime.Now) + @" " + stateMessageError + "\n";

                    // Get length of the state message
                    int lengthStateMessage = stateMessage.Length;

                    // Remove line break at the end
                    //tempControl.Text = tempControl.Text.TrimEnd('\n');

                    // Check if the maximum of lines is reached and delete last line
                    if (tempControl.Lines.Any() && tempControl.Lines.Count() > logger.LoggerSize)
                    {
                        tempControl.SelectionStart = tempControl.GetFirstCharIndexFromLine(tempControl.Lines.Count() - 2);
                        tempControl.SelectionLength = tempControl.Lines[tempControl.Lines.Count() - 2].Length + 1;
                        tempControl.SelectedText = string.Empty;
                    }

                    // Add new state message
                    tempControl.SelectionStart = 0;
                    tempControl.SelectionLength = 0;
                    tempControl.SelectionColor = color;
                    tempControl.SelectedText = stateMessage;

                    tempControl.WordWrap = true;
                    castControl = tempControl;

                    return true;
                }

                // Label
                if (showObject.GetType() == typeof(Label))
                {
                    Label castControl = (Label)showObject;

                    // Set color
                    Color oldColor = castControl.ForeColor;
                    castControl.ForeColor = color;

                    // Check if the logger add failed
                    if (stateMessageError != "")
                        stateMessage += @" " + stateMessageError + "\n";

                    // Set state message
                    castControl.Text = stateMessage;
                    // Reset color
                    castControl.ForeColor = oldColor;

                    return true;
                }

                // ToolStripStatusLabel
                if (showObject.GetType() == typeof(ToolStripStatusLabel))
                {
                    ToolStripStatusLabel castControl = (ToolStripStatusLabel)showObject;

                    // Set color
                    Color oldColor = castControl.ForeColor;
                    castControl.ForeColor = color;

                    // Check if the logger add failed
                    if (stateMessageError != "")
                        stateMessage += @" " + stateMessageError + "\n";
                    
                    // Set state message
                    castControl.Text = stateMessage;

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("AddStatusMessage()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                return false;
            }
        }

        #endregion Add status message to the text box and to the logger

        #region Enable or disable controls

        /// <summary>
        /// This function allows to enable or disable controls
        /// The controls are
        /// GroupBox
        /// Button
        /// MenuStrip
        /// DataGridView
        /// TabControl
        /// </summary>
        /// <param name="flag">true = enable / false = disable</param>
        /// <param name="givenControl">Control for the start</param>
        /// <param name="listControlNames">List with the control names which should be enabled or disabled</param>
        static public void EnableDisableControls(bool flag, Control givenControl, List<string> listControlNames)
        {
            try
            {
                foreach (var control in givenControl.Controls)
                {
                    Control castControl = null;
                    var type = control.GetType();

                    // GroupBox
                    if (control.GetType() == typeof(GroupBox))
                    {
                        castControl = (GroupBox)control;

                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;
                        if (castControl.Controls.Count > 0)
                        {
                            EnableDisableControls(flag, castControl, listControlNames);
                        }
                    }

                    // Button
                    if (control.GetType() == typeof(Button))
                    {
                        castControl = (Button)control;

                        if (listControlNames.Contains(castControl.Name))
                        {
                            castControl.Enabled = flag;
                        }
                        if (castControl.Controls.Count > 0)
                        {
                            EnableDisableControls(flag, castControl, listControlNames);
                        }
                    }

                    // MenuStrip
                    if (control.GetType() == typeof(MenuStrip))
                    {
                        castControl = (MenuStrip)control;

                        if (listControlNames.Contains(castControl.Name))
                        {
                            castControl.Enabled = flag;
                        }
                        if (castControl.Controls.Count > 0)
                        {
                            EnableDisableControls(flag, castControl, listControlNames);
                        }
                    }

                    // DataGridView
                    if (control.GetType() == typeof(DataGridView))
                    {
                        castControl = (DataGridView)control;

                        if (listControlNames.Contains(castControl.Name))
                        {
                            castControl.Enabled = flag;
                        }
                        if (castControl.Controls.Count > 0)
                        {
                            EnableDisableControls(flag, castControl, listControlNames);
                        }
                    }

                    // TabControl
                    if (control.GetType() == typeof(TabControl))
                    {
                        castControl = (TabControl)control;

                        if (listControlNames.Contains(castControl.Name))
                        {
                            castControl.Enabled = flag;
                        }
                        if (castControl.Controls.Count > 0)
                        {
                            EnableDisableControls(flag, castControl, listControlNames);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("EnableDisableControls()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            }
        }

        #endregion Enable or disable controls

        #region Scroll DataGridView to given index

        /// <summary>
        /// This function scrolls in the given DataGridView to the given index.
        /// The row with the given index would be the last seen row in the DataGridView.
        /// </summary>
        /// <param name="DataGridView">DataGridView</param>
        /// <param name="index">Index of the row where to scroll</param>
        /// <param name="bAllowScrollUp">Flag if a scroll to the top is allowed</param>
        static public void ScrollDgvToIndex(DataGridView DataGridView, int index, int lastDisplayedRowIndex, bool bAllowScrollUp = false)
        {
            // Check if DataGridView is valid
            if (DataGridView != null && DataGridView.RowCount > 0)
            {
                // Check if index is valid
                if (index >= 0 && index < DataGridView.RowCount)
                {
                    // Check if the row is already in the displayed area of the DataGridView
                    int iDisplayedRows = DataGridView.DisplayedRowCount(false);

                    if (DataGridView.FirstDisplayedCell != null)
                    {
                        int iFirstDisplayedRowIndex = 0;

                        if (lastDisplayedRowIndex < DataGridView.RowCount)
                            iFirstDisplayedRowIndex = lastDisplayedRowIndex;

                        Console.WriteLine("iFirstDisplayedRowIndex_1: {0}", iFirstDisplayedRowIndex);
                        Console.WriteLine("iDisplayedRows_1: {0}", iDisplayedRows);

                        if (bAllowScrollUp && index <= iFirstDisplayedRowIndex)
                        {
                            DataGridView.FirstDisplayedScrollingRowIndex = index;
                            lastDisplayedRowIndex = index;
                        }
                        else if (index + 1 > iFirstDisplayedRowIndex + iDisplayedRows && iDisplayedRows > 0)
                        {
                            if (iDisplayedRows < index + 1)
                                DataGridView.FirstDisplayedScrollingRowIndex = index - (iDisplayedRows - 1);
                        }
                        else
                            DataGridView.FirstDisplayedScrollingRowIndex = lastDisplayedRowIndex;

                        Console.WriteLine("iFirstDisplayedRowIndex_2: {0}", iFirstDisplayedRowIndex);
                        Console.WriteLine("iDisplayedRows_2: {0}", iDisplayedRows);
                    }
                }
            }
        }

        #endregion Scroll DataGridView to given index

        #region Formatting functions

        /// <summary>
        /// This function formats the share price
        /// </summary>
        /// <param name="decSharePrice"></param>
        static public string FormatSharePrice(decimal decSharePrice)
        {
            string strSharePrice = string.Format("{0:#.000}", decSharePrice);

            if (strSharePrice[strSharePrice.Length - 1] == '0')
                strSharePrice = string.Format("{0:C2}", decSharePrice);
            else
                strSharePrice = string.Format("{0:C3}", decSharePrice);

            return strSharePrice;
        }

        /// <summary>
        /// This function formats the given decimal value
        /// with the given format options.
        /// </summary>
        /// <param name="dblValue">Value for the formatting</param>
        /// <param name="iPrecision">Precision for the formatting</param>
        /// <param name="bFixed">Flag if the formatting should be fixed</param>
        /// <param name="iFixedPrecision">Length of the fixed precision</param>
        /// <param name="bUnit">Flag if a unit should be set</param>
        /// <param name="strUnit">String with the given unit</param>
        /// <param name="cultureInfo">Culture info for the formatting</param>
        /// <returns></returns>
        static public string FormatDouble(double dblValue, int iPrecision, bool bFixed, int iFixedPrecision = 0, bool bUnit = false, string strUnit = "", CultureInfo cultureInfo = null)
        {
            string strFormatString = "";
            string strResult = "";
            int iNoFixedPrecision = 0;

            // Check if the given precision is greater than the maximum
            if (iPrecision > Maxprecision)
                iPrecision = Maxprecision;

            // Check if the given fixed precision is greater than the maximum
            if (iFixedPrecision > iPrecision)
                iFixedPrecision = iPrecision;

            // Calculate no fixed precision value
            iNoFixedPrecision = iPrecision - iFixedPrecision - 1;

            if (iPrecision > 0)
            {
                // Create format string
                if (bFixed)
                {
                    for (int i = 1; i <= iPrecision; i++)
                    {
                        if (i == 1)
                            strFormatString = strFormatString + "0.0";
                        else
                            strFormatString = strFormatString + "0";
                    }
                }
                else
                {
                    for (int i = 1; i <= iPrecision; i++)
                    {
                        if (i > iNoFixedPrecision)
                        {
                            if (i == 1)
                                strFormatString = strFormatString + "0.#";
                            else
                                strFormatString = strFormatString + "#";
                        }
                        else
                        {
                            if (i == 1)
                                strFormatString = strFormatString + "0.0";
                            else
                                strFormatString = strFormatString + "0";
                        }
                    }
                }
            }
            else
            {
                strFormatString = strFormatString + "0";
            }

            // Format with the given culture info
            if (cultureInfo != null)
                strResult = dblValue.ToString(strFormatString, cultureInfo);
            else
                strResult = dblValue.ToString(strFormatString);

            // Check if a unit is given or should be set
            if (!bUnit)
                return strResult;

            if (!string.IsNullOrEmpty(strUnit))
                strResult = strResult + " " + strUnit;
            else
            {
                if (cultureInfo == null)
                    return strResult;

                RegionInfo ri = new RegionInfo(cultureInfo.LCID);
                strResult = strResult + " " + ri.CurrencySymbol;
            }

            return strResult;
        }

        /// <summary>
        /// This function formats the given decimal value
        /// with the given format options.
        /// </summary>
        /// <param name="decValue">Value for the formatting</param>
        /// <param name="iPrecision">Precision for the formatting</param>
        /// <param name="bFixed">Flag if the formatting should be fixed</param>
        /// <param name="iFixedPrecision">Length of the fixed precision</param>
        /// <param name="bUnit">Flag if a unit should be set</param>
        /// <param name="strUnit">String with the given unit</param>
        /// <param name="cultureInfo">Culture info for the formatting</param>
        /// <returns></returns>
        static public string FormatDecimal(decimal decValue, int iPrecision, bool bFixed, int iFixedPrecision = 0, bool bUnit = false, string strUnit = "", CultureInfo cultureInfo = null)
        {
            string strFormatString = "";
            string strResult = "";
            int iNoFixedPrecision = 0;

            // Check if the given precision is greater than the maximum
            if (iPrecision > Maxprecision)
                iPrecision = Maxprecision;

            // Check if the given fixed precision is greater than the maximum
            if (iFixedPrecision > iPrecision)
                iFixedPrecision = iPrecision;

            // Calculate no fixed precision value
            iNoFixedPrecision = iPrecision - iFixedPrecision - 1;

            if (iPrecision > 0)
            {
                // Create format string
                if (bFixed)
                {
                    for (int i = 1; i <= iPrecision; i++)
                    {
                        if (i == 1)
                            strFormatString = strFormatString + "0.0";
                        else
                            strFormatString = strFormatString + "0";
                    }
                }
                else
                {
                    for (int i = 1; i <= iPrecision; i++)
                    {
                        if (i > iNoFixedPrecision)
                        {
                            if (i == 1)
                                strFormatString = strFormatString + "0.#";
                            else
                                strFormatString = strFormatString + "#";
                        }
                        else
                        {
                            if (i == 1)
                                strFormatString = strFormatString + "0.0";
                            else
                                strFormatString = strFormatString + "0";
                        }
                    }
                }
            }
            else
            {
                strFormatString = strFormatString + "0";
            }

            // Format with the given culture info
            if (cultureInfo != null)
                strResult = decValue.ToString(strFormatString, cultureInfo);
            else
                strResult = decValue.ToString(strFormatString);

            // Check if a unit is given or should be set
            if (!bUnit)
                return strResult;

            if (!string.IsNullOrEmpty(strUnit))
                strResult = strResult + " " + strUnit;
            else
            {
                if (cultureInfo == null)
                    return strResult;

                RegionInfo ri = new RegionInfo(cultureInfo.LCID);
                strResult = strResult + " " + ri.CurrencySymbol;
            }

            return strResult;
        }

        #endregion Formatting functions

        #region Get RegEx options

        /// <summary>
        /// This function returns a list of "RegexOptions" of a given string
        /// </summary>
        /// <param name="strRegexOptions">The RegEx options string. Options must be separated by ","</param>
        /// <returns>List with the RegexOptions</returns>
        static public List<RegexOptions> GetRegexOptions(string strRegexOptions)
            {
                List<RegexOptions> regexOptionsList = new List<RegexOptions>();

                // Remove white space
                strRegexOptions = strRegexOptions.Replace(" ", "");
                foreach (var regexOption in strRegexOptions.Split(','))
                {
                    switch (regexOption)
                    {
                        case "Compiled":
                            regexOptionsList.Add(RegexOptions.Compiled);
                            break;
                        case "CultureInvariant":
                            regexOptionsList.Add(RegexOptions.CultureInvariant);
                            break;
                        case "ECMAScript":
                            regexOptionsList.Add(RegexOptions.ECMAScript);
                            break;
                        case "ExplicitCapture":
                            regexOptionsList.Add(RegexOptions.ExplicitCapture);
                            break;
                        case "IgnoreCase":
                            regexOptionsList.Add(RegexOptions.IgnoreCase);
                            break;
                        case "IgnorePatternWhitespace":
                            regexOptionsList.Add(RegexOptions.IgnorePatternWhitespace);
                            break;
                        case "Multiline":
                            regexOptionsList.Add(RegexOptions.Multiline);
                            break;
                        case "None":
                            regexOptionsList.Add(RegexOptions.None);
                            break;
                        case "RightToLeft":
                            regexOptionsList.Add(RegexOptions.RightToLeft);
                            break;
                        case "Singleline":
                            regexOptionsList.Add(RegexOptions.Singleline);
                            break;
                    }
                }

                if (regexOptionsList.Count > 0)
                    return regexOptionsList;

                return null;
            }

        #endregion Get RegEx options

        #region Open file dialog for setting document

        /// <summary>
        /// This function show a open file dialog
        /// and returns the chosen file
        /// </summary>
        /// <param name="xmlLanguage"></param>
        /// <param name="strLanguage"></param>
        /// <param name="strTitleLanguagePath"></param>
        /// <param name="strFilter"></param>
        /// <param name="strCurrentDocument"></param>
        /// <returns></returns>
        static public string SetDocument(string strTitle, string strFilter, string strCurrentDocument)
        {
            // Open file dialog
            OpenFileDialog openFileDlg = null;

            // Save old document
            string strOldDocument = strCurrentDocument;

            if (strCurrentDocument != "")
            {
                openFileDlg = new OpenFileDialog()
                {
                    Title = strTitle,
                    ValidateNames = true,
                    SupportMultiDottedExtensions = false,
                    Multiselect = false,
                    RestoreDirectory = true,
                    InitialDirectory = Path.GetDirectoryName(strCurrentDocument),
                    FileName = Path.GetFileName(strCurrentDocument),
                    Filter = strFilter
                };
            }
            else
            {
                openFileDlg = new OpenFileDialog()
                {
                    Title = strTitle,
                    ValidateNames = true,
                    SupportMultiDottedExtensions = false,
                    Multiselect = false,
                    RestoreDirectory = true,
                    Filter = strFilter
                };
            }

            DialogResult dlgResult = openFileDlg.ShowDialog();

            if (dlgResult == DialogResult.OK)
                strCurrentDocument = openFileDlg.FileName;
            else
                strCurrentDocument = strOldDocument;

            return strCurrentDocument;
        }

        #endregion Open file dialog for setting document

        #region Open file dialog for loading portfolio

        /// <summary>
        /// This function show a open file dialog
        /// and returns the chosen file
        /// </summary>
        /// <param name="xmlLanguage"></param>
        /// <param name="strLanguage"></param>
        /// <param name="strTitleLanguagePath"></param>
        /// <param name="strFilter"></param>
        /// <param name="strCurrentPortfolio"></param>
        /// <returns></returns>
        static public string LoadPortfolio(string strTitle, string strFilter, string strCurrentPortfolio)
        {
            // Save old portfolio file name
            string strOldPortfolioName = strCurrentPortfolio;

            OpenFileDialog openFileDlg = new OpenFileDialog()
            {
                Title = strTitle,
                ValidateNames = true,
                SupportMultiDottedExtensions = false,
                Multiselect = false,
                InitialDirectory = strCurrentPortfolio,
                RestoreDirectory = true,
                Filter = strFilter
            };
            DialogResult dlgResult = openFileDlg.ShowDialog();

            if (dlgResult == DialogResult.OK)
                strCurrentPortfolio = openFileDlg.FileName;
            else
                strCurrentPortfolio = strOldPortfolioName;

            return strCurrentPortfolio;
        }
        #endregion Open file dialog for loading portfolio

        #region Get file name of the given path

        /// <summary>
        /// This function return the file name of the given path
        /// </summary>
        /// <param name="strPath">Given path</param>
        /// <returns>String with the file name</returns>
        public static string GetFileName(string strPath)
        {
            if (Path.GetFileName(strPath) != null && Path.GetFileName(strPath) != String.Empty)
                return Path.GetFileName(strPath);

            return @"-";
        }

        #endregion Get file name of the given path

        #region Get currency list

        /// <summary>
        /// This function returns a list of all currency
        /// in three letter ISO name and the ISO currency unit
        /// </summary>
        /// <returns>IEnumberable with a KeyValuePair list</returns>
        public static IEnumerable<KeyValuePair<string, string>> GetCurrencyList()
        {
            IEnumerable<KeyValuePair<string, string>> currencyMap;
            currencyMap = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(c => !c.IsNeutralCulture)
            .Select(culture => {
                try
                {
                    return new RegionInfo(culture.LCID);
                }
                catch
                {
                    return null;
                }
            })
            .Where(ri => ri != null)
            .GroupBy(ri => ri.ISOCurrencySymbol)
            .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);

            IEnumerable<KeyValuePair<string, string>> result = currencyMap.OrderBy(ci => ci.Key);

            return result;
        }

        #endregion Get currency list

        #region Get culture info by name

        public static CultureInfo GetCultureByName(string name)
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            foreach(var temp in cultures)
            {
                if (temp.Name == name)
                    return temp;
            }

            return null;
        }

        public static CultureInfo GetCultureByISOCurrencySymbol(string ISOCurrencySymbol)
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            foreach (var temp in cultures)
            {
                RegionInfo ri = null;
                try
                {
                    ri = new RegionInfo(temp.LCID);
                }
                catch
                {
                    ri = null;
                }

                if (ri != null)
                    if (ri.ISOCurrencySymbol == ISOCurrencySymbol)
                        return temp;
            }

            return null;
        }

        #endregion Get culture info by name

        #region Calculate market value and purchase price

        static public void CalcMarketValueAndFinalValue(decimal decVolume, decimal decSharePrice, decimal decCosts, decimal decReduction, out decimal decMarketValue, out decimal decFinalValue)
        {
            if (decVolume > 0 && decSharePrice > 0)
            {
                decMarketValue = Math.Round(decVolume * decSharePrice, 2);
                decFinalValue = decMarketValue;

                if (decCosts > 0)
                    decFinalValue += decCosts;

                if (decReduction > 0)
                    decFinalValue -= decReduction;
            }
            else
            {
                decMarketValue = 0;
                decFinalValue = 0;
            }
        }
        #endregion Calculate makret value and purchase price

        #region Save share object

        /// <summary>
        /// This function saves a share object to the XML document
        /// </summary>
        /// <param name="shareObject">ShareObject</param>
        /// <param name="xmlPortfolio">XML with the portfolio</param>
        /// <param name="xmlReaderPortfolio">XML reader for the portfolio</param>
        /// <param name="xmlReaderSettingsPortfolio">XML reader settings for the portfolio</param>
        /// <param name="strPortfolioFileName">Name of the portfolio XML</param>
        /// <param name="exception">Exception which may occur. If no exception occurs the value is null</param>
        /// <returns>Flag if the save was successful</returns>
        public static bool SaveShareObject(ShareObject shareObject, ref XmlDocument xmlPortfolio, ref XmlReader xmlReaderPortfolio, ref XmlReaderSettings xmlReaderSettingsPortfolio, string strPortfolioFileName, out Exception exception)
        {
            try
            {
                // Update existing share
                var nodeListShares = xmlPortfolio.SelectNodes(string.Format("/Portfolio/Share [@WKN = \"{0}\"]", shareObject.Wkn));
                foreach (XmlNode nodeElement in nodeListShares)
                {
                    if (nodeElement != null)
                    {
                        if (nodeElement.HasChildNodes && nodeElement.ChildNodes.Count == shareObject.ShareObjectTagCount)
                        {
                            nodeElement.Attributes["WKN"].InnerText = shareObject.Wkn;
                            nodeElement.Attributes["Name"].InnerText = shareObject.NameAsStr;

                            for (int i = 0; i < nodeElement.ChildNodes.Count; i++)
                            {
                                switch (i)
                                {
                                    #region General
                                    case 0:
                                        nodeElement.ChildNodes[i].InnerText = string.Format(@"{0} {1}", shareObject.LastUpdateInternet.ToShortDateString(), shareObject.LastUpdateInternet.ToShortTimeString());
                                        break;
                                    case 1:
                                        nodeElement.ChildNodes[i].InnerText = string.Format(@"{0} {1}", shareObject.LastUpdateDate.ToShortDateString(), shareObject.LastUpdateDate.ToShortTimeString());
                                        break;
                                    case 2:
                                        nodeElement.ChildNodes[i].InnerText = string.Format(@"{0} {1}", shareObject.LastUpdateTime.ToShortDateString(), shareObject.LastUpdateTime.ToShortTimeString());
                                        break;
                                    case 3:
                                        nodeElement.ChildNodes[i].InnerText = shareObject.CurPriceAsStr;
                                        break;
                                    case 4:
                                        nodeElement.ChildNodes[i].InnerText = shareObject.PrevDayPriceAsStr;
                                        break;
                                    case 5:
                                        nodeElement.ChildNodes[i].InnerText = shareObject.DepositAsStr;
                                        break;
                                    case 6:
                                        nodeElement.ChildNodes[i].InnerText = shareObject.WebSite;
                                        break;
                                    case 7:
                                        nodeElement.ChildNodes[i].InnerXml = shareObject.CultureInfoAsStr;
                                        break;

                                    #endregion General

                                    #region Buys

                                    case 8:
                                        // Remove old buys
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        foreach (var buyElementYear in shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                                        {
                                            XmlElement newBuyElement = xmlPortfolio.CreateElement(shareObject.BuyTagNamePre);
                                            newBuyElement.SetAttribute(shareObject.BuyDateAttrName, buyElementYear.DateAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyVolumeAttrName, buyElementYear.VolumeAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyPriceAttrName, buyElementYear.SharePriceAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyReductionAttrName, buyElementYear.ReductionAsStr);
                                            newBuyElement.SetAttribute(shareObject.BuyDocumentAttrName, buyElementYear.Document);
                                            nodeElement.ChildNodes[i].AppendChild(newBuyElement);
                                        }
                                        break;

                                    #endregion Buys

                                    #region Sales

                                    case 9:
                                        // Remove old sales
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        foreach (var saleElementYear in shareObject.AllSaleEntries.GetAllSalesOfTheShare())
                                        {
                                            XmlElement newSaleElement = xmlPortfolio.CreateElement(shareObject.SaleTagNamePre);
                                            newSaleElement.SetAttribute(shareObject.SaleDateAttrName, saleElementYear.SaleDateAsString);
                                            newSaleElement.SetAttribute(shareObject.SaleVolumeAttrName, saleElementYear.SaleVolumeAsString);
                                            newSaleElement.SetAttribute(shareObject.SalePriceAttrName, saleElementYear.SaleValueAsString);
                                            newSaleElement.SetAttribute(shareObject.SaleProfitLossAttrName, saleElementYear.SaleProfitLossAsString);
                                            newSaleElement.SetAttribute(shareObject.SaleDocumentAttrName, saleElementYear.SaleDocument);
                                            nodeElement.ChildNodes[i].AppendChild(newSaleElement);
                                        }
                                        break;

                                    #endregion Sales

                                    #region Costs

                                    case 10:
                                        // Remove old costs
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        // Remove old costs
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        foreach (var costElementYear in shareObject.AllCostsEntries.GetAllCostsOfTheShare())
                                        {
                                            XmlElement newCostElement = xmlPortfolio.CreateElement(shareObject.CostsTagNamePre);
                                            newCostElement.SetAttribute(shareObject.CostsBuyPartAttrName, costElementYear.CostOfABuyAsString);
                                            newCostElement.SetAttribute(shareObject.CostsDateAttrName, costElementYear.CostDateAsString);
                                            newCostElement.SetAttribute(shareObject.CostsValueAttrName, costElementYear.CostValueAsString);
                                            newCostElement.SetAttribute(shareObject.BuyDocumentAttrName, costElementYear.CostDocument);
                                            nodeElement.ChildNodes[i].AppendChild(newCostElement);
                                        }
                                        break;

                                    #endregion Costs

                                    #region Dividends

                                    case 11:
                                        // Remove old dividends
                                        nodeElement.ChildNodes[i].RemoveAll();
                                        ((XmlElement)nodeElement.ChildNodes[i]).SetAttribute(shareObject.DividendPayoutIntervalAttrName, shareObject.DividendPayoutIntervalAsStr);

                                        foreach (var dividendObject in shareObject.AllDividendEntries.GetAllDividendsOfTheShare())
                                        {
                                            XmlElement newDividendElement = xmlPortfolio.CreateElement(shareObject.DividendTagName);
                                            newDividendElement.SetAttribute(shareObject.DividendDateAttrName, dividendObject.DividendDateAsString);
                                            newDividendElement.SetAttribute(shareObject.DividendVolumeAttrName, dividendObject.ShareVolumeAsString);
                                            newDividendElement.SetAttribute(shareObject.DividendLossBalanceAttrName, dividendObject.LossBalanceAsString);
                                            newDividendElement.SetAttribute(shareObject.DividendPriceAttrName, dividendObject.SharePriceAsString);
                                            newDividendElement.SetAttribute(shareObject.DividendRateAttrName, dividendObject.DividendRateAsString);
                                            newDividendElement.SetAttribute(shareObject.DividendDocumentAttrName, dividendObject.DividendDocument);
                                            
                                            // Foreign currency information
                                            XmlElement newForeignCurrencyElement = xmlPortfolio.CreateElement(shareObject.DividendTagNameForeignCu);

                                            newForeignCurrencyElement.SetAttribute(shareObject.DividendForeignCuFlagAttrName, dividendObject.DividendTaxes.FCFlag.ToString());

                                            if (dividendObject.DividendTaxes.FCFlag)
                                            {
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendExchangeRatioAttrName, dividendObject.DividendTaxes.ExchangeRatio.ToString());
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendNameAttrName, dividendObject.DividendTaxes.CiShareFC.Name);
                                            }
                                            else
                                            {
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendExchangeRatioAttrName, 0.ToString());
                                                newForeignCurrencyElement.SetAttribute(shareObject.DividendNameAttrName, dividendObject.DividendTaxes.CiShareFC.Name);

                                            }
                                            newDividendElement.AppendChild(newForeignCurrencyElement);


                                            // Add child nodes (taxes)
                                            XmlElement newTaxesElement = xmlPortfolio.CreateElement(shareObject.TaxTagName);

                                            XmlElement newTaxAtSourceElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameTaxAtSource);
                                            newTaxAtSourceElement.SetAttribute(shareObject.TaxTaxAtSourceFlagAttrName, dividendObject.DividendTaxes.TaxAtSourceFlag.ToString());
                                            newTaxAtSourceElement.SetAttribute(shareObject.TaxTaxAtSourcePercentageAttrName, dividendObject.DividendTaxes.TaxAtSourcePercentage.ToString());
                                            newTaxesElement.AppendChild(newTaxAtSourceElement);

                                            XmlElement newCapitalGainsTaxElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameCapitalGains);
                                            newCapitalGainsTaxElement.SetAttribute(shareObject.TaxCapitalGainsFlagAttrName, dividendObject.DividendTaxes.CapitalGainsTaxFlag.ToString());
                                            newCapitalGainsTaxElement.SetAttribute(shareObject.TaxCapitalGainsPercentageAttrName, dividendObject.DividendTaxes.CapitalGainsTaxPercentage.ToString());
                                            newTaxesElement.AppendChild(newCapitalGainsTaxElement);

                                            XmlElement newSolidarityTaxElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameSolidarity);
                                            newSolidarityTaxElement.SetAttribute(shareObject.TaxSolidarityFlagAttrName, dividendObject.DividendTaxes.SolidarityTaxFlag.ToString());
                                            newSolidarityTaxElement.SetAttribute(shareObject.TaxSolidarityPercentageAttrName, dividendObject.DividendTaxes.SolidarityTaxPercentage.ToString());
                                            newTaxesElement.AppendChild(newSolidarityTaxElement);

                                            newDividendElement.AppendChild(newTaxesElement);

                                            nodeElement.ChildNodes[i].AppendChild(newDividendElement);
                                        }
                                        break;

                                    #endregion Dividends

                                    #region Taxes
                                    
                                    case 12:
                                        // Remove old taxes
                                        nodeElement.ChildNodes[i].RemoveAll();

                                        XmlElement newRegularTaxAtSourceElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameTaxAtSource);
                                        newRegularTaxAtSourceElement.SetAttribute(shareObject.TaxTaxAtSourceFlagAttrName, shareObject.TaxTaxAtSourceFlagAsStr);
                                        newRegularTaxAtSourceElement.SetAttribute(shareObject.TaxTaxAtSourcePercentageAttrName, shareObject.TaxTaxAtSourcePercentageAsStr);
                                        nodeElement.ChildNodes[i].AppendChild(newRegularTaxAtSourceElement);

                                        XmlElement newRegularCapitalGainsTaxElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameCapitalGains);
                                        newRegularCapitalGainsTaxElement.SetAttribute(shareObject.TaxCapitalGainsFlagAttrName, shareObject.CapitalGainsTaxFlagAsStr);
                                        newRegularCapitalGainsTaxElement.SetAttribute(shareObject.TaxCapitalGainsPercentageAttrName, shareObject.TaxCapitalGainsPercentageAsStr);
                                        nodeElement.ChildNodes[i].AppendChild(newRegularCapitalGainsTaxElement);

                                        XmlElement newRegularSolidarityTaxElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameSolidarity);
                                        newRegularSolidarityTaxElement.SetAttribute(shareObject.TaxSolidarityFlagAttrName, shareObject.TaxSolidarityFlagAsStr);
                                        newRegularSolidarityTaxElement.SetAttribute(shareObject.TaxSolidarityPercentageAttrName, shareObject.TaxSolidarityPercentageAsStr);
                                        nodeElement.ChildNodes[i].AppendChild(newRegularSolidarityTaxElement);

                                        break;

                                    #endregion Taxes 

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                // Add a new share
                if (nodeListShares.Count == 0)
                {
                    #region General

                    // Get root element
                    XmlNode rootPortfolio = xmlPortfolio.SelectSingleNode("Portfolio");

                    // Add new share
                    XmlNode newShareNode = xmlPortfolio.CreateNode(XmlNodeType.Element, "Share", null);

                    // Add attributes (WKN)
                    XmlAttribute xmlAttributeWKN = xmlPortfolio.CreateAttribute("WKN");
                    xmlAttributeWKN.Value = shareObject.WknAsStr;
                    newShareNode.Attributes.Append(xmlAttributeWKN);

                    // Add attributes (ShareName)
                    XmlAttribute xmlAttributeShareName = xmlPortfolio.CreateAttribute("Name");
                    xmlAttributeShareName.Value = shareObject.NameAsStr;
                    newShareNode.Attributes.Append(xmlAttributeShareName);

                    // Add child nodes (last update Internet)
                    XmlElement newLastUpdateInternet = xmlPortfolio.CreateElement("LastUpdateInternet");
                    // Add child inner text
                    XmlText lastUpdateInternetValue = xmlPortfolio.CreateTextNode(shareObject.LastUpdateInternet.ToShortDateString() + " " + shareObject.LastUpdateInternet.ToShortTimeString());
                    newShareNode.AppendChild(newLastUpdateInternet);
                    newShareNode.LastChild.AppendChild(lastUpdateInternetValue);

                    // Add child nodes (last update date)
                    XmlElement newLastUpdateDate = xmlPortfolio.CreateElement("LastUpdateShareDate");
                    // Add child inner text
                    XmlText lastUpdateValueDate = xmlPortfolio.CreateTextNode(shareObject.LastUpdateDate.ToShortDateString() + " " + shareObject.LastUpdateDate.ToShortTimeString());
                    newShareNode.AppendChild(newLastUpdateDate);
                    newShareNode.LastChild.AppendChild(lastUpdateValueDate);

                    // Add child nodes (last update time)
                    XmlElement newLastUpdateTime = xmlPortfolio.CreateElement("LastUpdateTime");
                    // Add child inner text
                    XmlText lastUpdateValueTime = xmlPortfolio.CreateTextNode(shareObject.LastUpdateTime.ToShortDateString() + " " + shareObject.LastUpdateTime.ToShortTimeString());
                    newShareNode.AppendChild(newLastUpdateTime);
                    newShareNode.LastChild.AppendChild(lastUpdateValueTime);

                    // Add child nodes (share price)
                    XmlElement newSharePrice = xmlPortfolio.CreateElement("SharePrice");
                    // Add child inner text
                    XmlText SharePrice = xmlPortfolio.CreateTextNode(shareObject.CurPriceAsStr);
                    newShareNode.AppendChild(newSharePrice);
                    newShareNode.LastChild.AppendChild(SharePrice);

                    // Add child nodes (share price before)
                    XmlElement newSharePriceBefore = xmlPortfolio.CreateElement("SharePriceBefore");
                    // Add child inner text
                    XmlText SharePriceBefore = xmlPortfolio.CreateTextNode(shareObject.PrevDayPriceAsStr);
                    newShareNode.AppendChild(newSharePriceBefore);
                    newShareNode.LastChild.AppendChild(SharePriceBefore);

                    // Add child nodes (deposit)
                    XmlElement newDeposit = xmlPortfolio.CreateElement("Deposit");
                    // Add child inner text
                    XmlText Deposit = xmlPortfolio.CreateTextNode(shareObject.MarketValueAsStr);
                    newShareNode.AppendChild(newDeposit);
                    newShareNode.LastChild.AppendChild(Deposit);

                    // Add child nodes (website)
                    XmlElement newWebsite = xmlPortfolio.CreateElement("WebSite");
                    // Add child inner text
                    XmlText WebSite = xmlPortfolio.CreateTextNode(shareObject.WebSite);
                    newShareNode.AppendChild(newWebsite);
                    newShareNode.LastChild.AppendChild(WebSite);

                    // Add child nodes (culture)
                    XmlElement newCulture = xmlPortfolio.CreateElement("Culture");
                    // Add child inner text
                    XmlText Culture = xmlPortfolio.CreateTextNode(shareObject.CultureInfo.Name);
                    newShareNode.AppendChild(newCulture);
                    newShareNode.LastChild.AppendChild(Culture);

                    #endregion General

                    #region Buys / Sales / Costs / Dividends

                    // Add child nodes (buys)
                    XmlElement newBuys = xmlPortfolio.CreateElement("Buys");
                    newShareNode.AppendChild(newBuys);
                    foreach (var buyElementYear in shareObject.AllBuyEntries.GetAllBuysOfTheShare())
                    {
                        XmlElement newBuyElement = xmlPortfolio.CreateElement(shareObject.BuyTagNamePre);
                        newBuyElement.SetAttribute(shareObject.BuyDateAttrName, buyElementYear.DateAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyVolumeAttrName, buyElementYear.VolumeAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyPriceAttrName, buyElementYear.SharePriceAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyReductionAttrName, buyElementYear.ReductionAsStr);
                        newBuyElement.SetAttribute(shareObject.BuyDocumentAttrName, buyElementYear.Document);
                        newBuys.AppendChild(newBuyElement);
                    }

                    // Add child nodes (sales)
                    XmlElement newSales = xmlPortfolio.CreateElement("Sales");
                    newShareNode.AppendChild(newSales);

                    // Add child nodes (costs)
                    XmlElement newCosts = xmlPortfolio.CreateElement("Costs");
                    newShareNode.AppendChild(newCosts);
                    foreach (var costElementYear in shareObject.AllCostsEntries.GetAllCostsOfTheShare())
                    {
                        XmlElement newCostElement = xmlPortfolio.CreateElement(shareObject.CostsTagNamePre);
                        newCostElement.SetAttribute(shareObject.CostsBuyPartAttrName, costElementYear.CostOfABuyAsString);
                        newCostElement.SetAttribute(shareObject.CostsDateAttrName, costElementYear.CostDateAsString);
                        newCostElement.SetAttribute(shareObject.CostsValueAttrName, costElementYear.CostValueAsString);
                        newCostElement.SetAttribute(shareObject.BuyDocumentAttrName, costElementYear.CostDocument);
                        newCosts.AppendChild(newCostElement);
                    }

                    // Add child nodes (dividend)
                    XmlElement newDividend = xmlPortfolio.CreateElement("Dividends");
                    newDividend.SetAttribute(shareObject.DividendPayoutIntervalAttrName, shareObject.DividendPayoutIntervalAsStr);
                    newShareNode.AppendChild(newDividend);

                    #endregion Buys / Sales / Costs / Dividends

                    #region Taxes

                    // Add child nodes (taxes)
                    XmlElement newRegularTaxesElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameRegularTaxes);

                    XmlElement newRegularTaxAtSourceElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameTaxAtSource);
                    newRegularTaxAtSourceElement.SetAttribute(shareObject.TaxTaxAtSourceFlagAttrName, shareObject.TaxTaxAtSourceFlagAsStr);
                    newRegularTaxAtSourceElement.SetAttribute(shareObject.TaxTaxAtSourcePercentageAttrName, shareObject.TaxTaxAtSourcePercentageAsStr);
                    newRegularTaxesElement.AppendChild(newRegularTaxAtSourceElement);

                    XmlElement newRegularCapitalGainsTaxElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameCapitalGains);
                    newRegularCapitalGainsTaxElement.SetAttribute(shareObject.TaxCapitalGainsFlagAttrName, shareObject.CapitalGainsTaxFlagAsStr);
                    newRegularCapitalGainsTaxElement.SetAttribute(shareObject.TaxCapitalGainsPercentageAttrName, shareObject.TaxCapitalGainsPercentageAsStr);
                    newRegularTaxesElement.AppendChild(newRegularCapitalGainsTaxElement);

                    XmlElement newRegularSolidarityTaxElement = xmlPortfolio.CreateElement(shareObject.TaxTagNameSolidarity);
                    newRegularSolidarityTaxElement.SetAttribute(shareObject.TaxSolidarityFlagAttrName, shareObject.TaxSolidarityFlagAsStr);
                    newRegularSolidarityTaxElement.SetAttribute(shareObject.TaxSolidarityPercentageAttrName, shareObject.TaxSolidarityPercentageAsStr);
                    newRegularTaxesElement.AppendChild(newRegularSolidarityTaxElement);

                    newShareNode.AppendChild(newRegularTaxesElement);

                    #endregion Taxes

                    // Add share name to XML
                    rootPortfolio.AppendChild(newShareNode);
                }

                // Close reader for saving
                xmlReaderPortfolio.Close();
                // Save settings
                xmlPortfolio.Save(strPortfolioFileName);
                // Create a new reader
                xmlReaderPortfolio = XmlReader.Create(strPortfolioFileName, xmlReaderSettingsPortfolio);
                xmlPortfolio.Load(strPortfolioFileName);
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }

            exception = null;
            return true;
        }

        #endregion Save share object

        #endregion Methods
    }
}
