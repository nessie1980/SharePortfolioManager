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
using System.Reflection;
using Logging;
using LanguageHandler;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Net;

namespace SharePortfolioManager.Classes
{
    static class Helper
    {
        #region Constants formatting

        /// <summary>
        /// Stores the max precision for a value
        /// </summary>
        public const int Maxprecision = 6;

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
        public const int Currencysixlength = 6;
        public const int Currencyfivelength = 5;
        public const int Currencyfourlength = 4;
        public const int Currencythreelength = 3;
        public const int Currencytwolength = 2;
        public const int Currencyonelength = 1;

        public const int Currencyfivefixlength = 5;
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
        /// Stores the list of the names and the corresponding culture info and currency symbol
        /// </summary>
        static private IEnumerable<KeyValuePair<string, CultureInformation>> _listNameCultureInfoCurrencySymbol;

        #endregion Variables

        #region Properties

        static public IEnumerable<KeyValuePair<string, CultureInformation>> ListNameCultureInfoCurrencySymbol
        {
            get { return _listNameCultureInfoCurrencySymbol; }
        }

        static public Dictionary<string, CultureInformation> DictionaryListNameCultureInfoCurrencySymbol
        {
            get { return _listNameCultureInfoCurrencySymbol.ToDictionary(x => x.Key, x => x.Value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        static Helper()
        {
            CreateNameCultureInfoCurrencySymbolList();
        }

        /// <summary>
        /// This function returns the version of the application
        /// </summary>
        /// <returns>Version of the application</returns>
        public static Version GetApplicationVersion()
        {
            Assembly assApp = typeof(FrmMain).Assembly;
            AssemblyName assAppName = assApp.GetName();
            Version verApp = assAppName.Version;

            return verApp;
        }

        #region Get name of current method

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetMyMethodName()
        {
            var st = new StackTrace(new StackFrame(1));
            return st.GetFrame(0).GetMethod().Name;
        }

        #endregion Get name of current method

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
        public static string FormatDouble(double dblValue, int iPrecision, bool bFixed, int iFixedPrecision = 0, bool bUnit = false, string strUnit = "", CultureInfo cultureInfo = null)
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
        public static string FormatDecimal(decimal decValue, int iPrecision, bool bFixed, int iFixedPrecision = 0, bool bUnit = false, string strUnit = "", CultureInfo cultureInfo = null)
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
        public static void CreateNameCultureInfoCurrencySymbolList()
        {
            _listNameCultureInfoCurrencySymbol = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(c => !c.IsNeutralCulture)
            .Select(culture =>
            {
                try
                {
                    return new KeyValuePair<String, CultureInformation>(culture.Name, new CultureInformation(culture, new RegionInfo(culture.LCID).CurrencySymbol));
                }
                catch
                {
                    return new KeyValuePair<String, CultureInformation>(null, null);
                }
            })
            .Where(ci => ci.Key != null)
            .OrderBy(ci => ci.Key)
            .ToDictionary(x => x.Key, x => x.Value);

            _listNameCultureInfoCurrencySymbol.OrderBy(ci => ci.Key);
        }

        #endregion Get currency list

        #region Get culture info by name

        public static CultureInfo GetCultureByName(string name)
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            foreach (var temp in cultures)
            {
                if (temp.Name == name)
                    return temp;
            }

            return null;
        }

        #endregion Get culture info by name

        #region Calculate market value, market value minus reduction and market value minus reduction and plus costs

        static public void CalcBuyValues(decimal decVolume, decimal decSharePrice, decimal decCosts, decimal decReduction, out decimal decMarketValue, out decimal decMarketValueReduction, out decimal decMarketValueReductionCosts)
        {
            if (decVolume > 0 && decSharePrice > 0)
            {
                decMarketValue = Math.Round(decVolume * decSharePrice, 2);
                decMarketValueReduction = decMarketValue;
                if (decReduction > 0)
                    decMarketValueReduction -= decReduction;

                decMarketValueReductionCosts = decMarketValueReduction;
                if (decCosts > 0)
                    decMarketValueReductionCosts += decCosts;
            }
            else
            {
                decMarketValue = 0;
                decMarketValueReduction = 0;
                decMarketValueReductionCosts = 0;
            }
        }
        #endregion  Calculate market value, market value minus reduction and market value minus reduction and plus costs

        #region URL checker

        static public bool UrlChecker(ref string url, int timeout)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            HttpWebResponse response;
            try
            {
                Uri urlCheck = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
                request.Timeout = timeout;
                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; de; rv:1.9.0.13) Gecko/2009073022 Firefox/3.0.13";

                response = (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                return false; //could not connect to the inter net (maybe) 
            }

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found)
                return true;
            else
                return false;
        }

        #endregion URL checker

        #region Get combo box items

        public static string[] GetComboBoxItmes(string xPath, string languageName, Language language)
        {
            return language.GetLanguageTextListByXPath(xPath, languageName).ToArray();
        }

        #endregion  Get combo box items

        #endregion Methods
    }

    // This class stores the informations of a culture info
    public class CultureInformation
    {
        private string _currencySymbol;
        private CultureInfo _cultureInfo;

        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { _cultureInfo = value; }
        }

        public CultureInformation(string currencySymbol)
        {
            _currencySymbol = currencySymbol;
        }

        public CultureInformation(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
        }

        public CultureInformation(CultureInfo cultureInfo, string currencySymbol)
        {
            _currencySymbol = currencySymbol;
            _cultureInfo = cultureInfo;
        }
    }

    //class CultureInformationTest <String, CultureInformation>
    //{
    //    public string _cultureISOCurrencySymbol;
    //    public CultureInformation _cultureInformation;

    //    public CultureInformationTest(string cultureISOCurrencySymbol, CultureInformation cultureInformation)
    //    {
    //        _cultureISOCurrencySymbol = cultureISOCurrencySymbol;
    //        _cultureInformation = cultureInformation;
    //    }

    //}
}
