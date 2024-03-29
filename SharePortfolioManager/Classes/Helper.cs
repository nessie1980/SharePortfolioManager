﻿//MIT License
//
//Copyright(c) 2017 - 2022 nessie1980(nessie1980@gmx.de)
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
//#define DEBUG_HELPER

using LanguageHandler;
using Logging;
using SharePortfolioManager.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharePortfolioManager.Classes.Configurations;
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager.Classes
{
    internal static class Helper
    {
        #region Constants formatting

        /// <summary>
        /// Stores the max precision for a value
        /// </summary>
        public const int MaxPrecision = 6;

        #region Date

        /// <summary>
        /// Stores the date time format
        /// </summary>
        public const string DateTimeFullFormat = "{0:dd/MM/yyyy HH:mm:ss}";

        public const string DateTimeFullFormat2 = "{0:dd/MM/yyyy 1:HH:mm:ss}";
        public const string DateFullTimeShortFormat = "{0:dd/MM/yyyy HH:mm}";
        public const string TimeFullFormat = "{0:HH:mm:ss}";
        public const string TimeShortFormat = "{0:HH:mm}";
        public const string DateFullFormat = "{0:dd/MM/yyyy}";

        #endregion Date

        #region Currency

        /// <summary>
        /// Stores currency format (price, deposit, brokerage, buys...)
        /// </summary>
        public const int CurrencySixLength = 6;

        public const int CurrencyFiveLength = 5;
        public const int CurrencyFourLength = 4;
        public const int CurrencyThreeLength = 3;
        public const int CurrencyTwoLength = 2;
        public const int CurrencyOneLength = 1;

        public const int CurrencyFiveFixLength = 5;
        public const int CurrencyFourFixLength = 4;
        public const int CurrencyThreeFixLength = 3;
        public const int CurrencyTwoFixLength = 2;
        public const int CurrencyOneFixLength = 1;
        public const int CurrencyNoneFixLength = 0;

        #endregion Currency

        #region Percentage

        /// <summary>
        /// Stores percentage format (performance...)
        /// </summary>
        public const int PercentageFiveLength = 5;

        public const int PercentageFourLength = 4;
        public const int PercentageThreeLength = 3;
        public const int PercentageTwoLength = 2;
        public const int PercentageOneLength = 1;

        public const int PercentageFourFixLength = 4;
        public const int PercentageThreeFixLength = 3;
        public const int PercentageTwoFixLength = 2;
        public const int PercentageOneFixLength = 1;
        public const int PercentageNoneFixLength = 0;

        #endregion Percentage

        #region Volume

        /// <summary>
        /// Stores volume format (pcs.)
        /// </summary>
        public const int VolumeFiveLength = 5;

        public const int VolumeFourLength = 4;
        public const int VolumeThreeLength = 3;
        public const int VolumeTwoLength = 2;
        public const int VolumeOneLength = 1;

        public const int VolumeFiveFixLength = 5;
        public const int VolumeFourFixLength = 4;
        public const int VolumeThreeFixLength = 3;
        public const int VolumeTwoFixLength = 2;
        public const int VolumeOneFixLength = 1;
        public const int VolumeNoneFixLength = 0;

        #endregion Volume

        #region Dividend

        /// <summary>
        /// Stores dividend format
        /// </summary>
        public const int DividendFiveLength = 5;

        public const int DividendFourLength = 4;
        public const int DividendThreeLength = 3;
        public const int DividendTwoLength = 2;
        public const int DividendOneLength = 1;

        public const int DividendFiveFixLength = 5;
        public const int DividendFourFixLength = 4;
        public const int DividendThreeFixLength = 3;
        public const int DividendTwoFixLength = 2;
        public const int DividendOneFixLength = 1;
        public const int DividendNoneFixLength = 0;

        #endregion Dividend

        #endregion Constants formatting

        #region Variables

        #region Convert PDF to text

        public static string PdfToTextApplication =
            Path.GetDirectoryName(Application.ExecutablePath) + @"\Tools\pdftotext.exe";

        public static string ParsingDocumentFileName =
            Path.GetDirectoryName(Application.ExecutablePath) + @"\Tools\Parsing.txt";

        public static bool PdfParserInstalled() => File.Exists(PdfToTextApplication);

        #endregion Convert PDF to text

        public static bool ShowExceptionMessageFlag;

        public static Form FrmMain;

        // Name of the charting color XML tag for buy information
        public const string BuyInformationName = "Buy";

        // Name of the charting color XML tag for sale information
        public const string SaleInformationName = "Sale";

        #endregion Variables

        #region Properties

        public static IEnumerable<KeyValuePair<string, CultureInformation>> ListNameCultureInfoCurrencySymbol
        {
            get;
            private set;
        }

        public static Dictionary<string, CultureInformation> DictionaryListNameCultureInfoCurrencySymbol
        {
            get { return ListNameCultureInfoCurrencySymbol.ToDictionary(x => x.Key, x => x.Value); }
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
            var assApp = typeof(FrmMain).Assembly;
            var assAppName = assApp.GetName();
            var verApp = assAppName.Version;

            return verApp;
        }

        #region Text new line builder

        public static string BuildNewLineTextFromStringList(List<string> stringList)
        {
            if (stringList == null || stringList.Count <= 0)
                return @"null";

            var stringResult = @"";
            for (var i = 0; i < stringList.Count; i++)
            {
                if (i == 0)
                {
                    stringResult += stringList[i];
                }
                else
                    stringResult += Environment.NewLine + stringList[i];
            }

            return stringResult;
        }

        #endregion Text new line builder

        #region Functions for the logging and debugging

        /// <summary>
        /// This functions determines the name of the methods where it is called
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        // ReSharper disable once UnusedMember.Global
        public static string GetMethodName()
        {
            var st = new StackTrace(new StackFrame(1));
            return st.GetFrame(0).GetMethod().Name;
        }

        /// <summary>
        /// This functions shows a message box with the given caption and some exception information
        /// </summary>
        /// <param name="ex">Exception which should be shown</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        /// <param name="bInnerException">Flag if an inner exception is given</param>
        /// <param name="iInnerExceptionCounter">Counter which counts the amount of given inner exceptions</param>
        public static void ShowExceptionMessage(Exception ex, Language xmlLanguage = null, string language = null, bool bInnerException = false, int iInnerExceptionCounter = 0)
        {
            // Check if not all necessary values for the show are given
            if (!ShowExceptionMessageFlag || ex == null) return;

            string message;
            string caption;

            if (xmlLanguage == null || language == null)
            {
                if (!bInnerException)
                {
                    caption = @"An exception occurred!";
                }
                else
                {
                    caption = @"An inner exception (" + iInnerExceptionCounter + ") occurred!";
                }

                message = @"Source: "
                    + Environment.NewLine + ex.Source
                    + Environment.NewLine + Environment.NewLine
                    + @"Function: "
                    + Environment.NewLine + ex.TargetSite
                    + Environment.NewLine + Environment.NewLine
                    + @"Message: "
                    + Environment.NewLine
                    + ex.Message
                    + Environment.NewLine + Environment.NewLine
                    + @"StackTrace: "
                    + Environment.NewLine
                    + ex.StackTrace;

                if (ex.Data.Count > 0)
                {
                    message += @"Data: "
                        + Environment.NewLine;
                }

                foreach (DictionaryEntry entry in ex.Data)
                {
                    message += entry.Value
                        + Environment.NewLine + Environment.NewLine;
                }

                if (ex.HelpLink != null)
                {
                    message += Environment.NewLine
                        + Environment.NewLine
                        + @"HelpLink: "
                        + Environment.NewLine
                        + ex.HelpLink;
                }
            }
            else
            {
                // Get caption for inner or normal exception
                if (!bInnerException)
                {
                    caption = xmlLanguage.GetLanguageTextByXPath(@"/ExceptionMessageBoxForm/Caption", language);
                }
                else
                {
                    caption = xmlLanguage.GetLanguageTextByXPath(@"/ExceptionMessageBoxForm/CaptionInner", language);
                }

                message = xmlLanguage.GetLanguageTextByXPath(
                        @"/ExceptionMessageBoxForm/Content/Source",
                        language) + " "
                    + Environment.NewLine + ex.Source
                    + Environment.NewLine + Environment.NewLine
                    + xmlLanguage.GetLanguageTextByXPath(
                        @"/ExceptionMessageBoxForm/Content/Function",
                        language) + " "
                    + Environment.NewLine + ex.TargetSite
                    + Environment.NewLine + Environment.NewLine
                    + xmlLanguage.GetLanguageTextByXPath(
                        @"/ExceptionMessageBoxForm/Content/Message",
                        language) + " "
                    + Environment.NewLine
                    + ex.Message
                    + Environment.NewLine + Environment.NewLine
                    + xmlLanguage.GetLanguageTextByXPath(
                        @"/ExceptionMessageBoxForm/Content/StackTrace",
                        language) + " "
                    + Environment.NewLine
                    + ex.StackTrace;

                if (ex.Data.Count > 0)
                {
                    message += xmlLanguage.GetLanguageTextByXPath(
                            @"/ExceptionMessageBoxForm/Content/Data",
                            language)
                        + " " + Environment.NewLine;
                }

                foreach (DictionaryEntry entry in ex.Data)
                {
                    message += entry.Value
                        + Environment.NewLine + Environment.NewLine;
                }

                if (ex.HelpLink != null)
                {
                    message += Environment.NewLine
                        + Environment.NewLine
                        + xmlLanguage.GetLanguageTextByXPath(
                            @"/ExceptionMessageBoxForm/Content/HelpLink",
                            language) + " "
                        + Environment.NewLine
                        + ex.HelpLink;
                }
            }

            // TODO: (thomas:2022-08-18) It looks like centering of the message box does no more work
            if (FrmMain != null)
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(FrmMain);

            if (ex.InnerException != null)
            {
                ShowExceptionMessage(ex.InnerException, xmlLanguage, language, true, ++iInnerExceptionCounter);
            }

            MessageBox.Show(message, caption, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// This function adds a state message to the top of the status message RichEditBox
        /// and to the logger if the logger is initialized
        /// </summary>
        /// <param name="showControl">Control where the message should be shown (RichTextBox / Label / ToolStripStatusLabel)</param>
        /// <param name="stateMessage">State message which should be added</param>
        /// <param name="xmlLanguage">Language file</param>
        /// <param name="language">Language</param>
        /// <param name="color">Color of the message in the RichEditBox if the logger is not initialized or the color could not be determine</param>
        /// <param name="logger">Logger for the logging</param>
        /// <param name="stateLevel">Level of the state (e.g. Info)</param>
        /// <param name="componentLevel">Level of the component (e.g. Application)</param>
        /// <param name="exception">Given exception which should be shown</param>
        /// <returns>Flag if the add was successful</returns>
        public static bool AddStatusMessage(object showControl, string stateMessage, Language xmlLanguage,
            string language, Color color, Logger logger, int stateLevel, int componentLevel, Exception exception = null)
        {
            try
            {
                var result = false;
                var stateMessageError = "";

                if (stateLevel == (int) SharePortfolioManager.FrmMain.EStateLevels.Error ||
                    stateLevel == (int) SharePortfolioManager.FrmMain.EStateLevels.FatalError
                )
                    Sound.PlayErrorSound();

                // Check if the given logger is initialized
                if (logger != null && logger.InitState == Logger.EInitState.Initialized)
                {
                    try
                    {
                        // Get color for the message
                        color = logger.GetColorOfStateLevel((Logger.ELoggerStateLevels) stateLevel);

                        // Add state message to the log entry list
                        logger.AddEntry(stateMessage, (Logger.ELoggerStateLevels) stateLevel,
                            (Logger.ELoggerComponentLevels) componentLevel);
                    }
                    catch (LoggerException ex)
                    {
                        if (ex.LoggerState == Logger.ELoggerState.CleanUpLogFilesFailed)
                            stateMessageError =
                                xmlLanguage.GetLanguageTextByXPath(@"/Logger/LoggerStateMessages/CleanUpLogFilesFailed",
                                    language);
                    }
                }

                // RichTextBox
                if (logger != null && showControl.GetType() == typeof(RichTextBox))
                {
                    var castControl = (RichTextBox) showControl;

                    // Create a temporary rich text box with no word wrap
                    var tempControl = castControl;
                    tempControl.WordWrap = false;

                    // Add time stamp
                    stateMessage = string.Format(TimeFullFormat, DateTime.Now) + @" " + stateMessage +
                                   Environment.NewLine;

                    // Check if the logger add failed
                    if (stateMessageError != "")
                        stateMessage += string.Format(TimeFullFormat, DateTime.Now) + @" " + stateMessageError +
                                        Environment.NewLine;

                    // Check if the maximum of lines is reached and delete last line
                    if (tempControl.Lines.Any() && tempControl.Lines.Length > logger.LoggerSize &&
                        logger.LoggerSize > -1)
                    {
                        tempControl.SelectionStart =
                            tempControl.GetFirstCharIndexFromLine(tempControl.Lines.Length - 2);
                        tempControl.SelectionLength = tempControl.Lines[tempControl.Lines.Length - 2].Length + 1;
                        tempControl.SelectedText = string.Empty;
                    }

                    // Add new state message
                    tempControl.SelectionStart = 0;
                    tempControl.SelectionLength = 0;
                    tempControl.SelectionColor = color;
                    tempControl.SelectedText = stateMessage;

                    tempControl.WordWrap = true;

                    result = true;
                }

                // Label
                if (showControl.GetType() == typeof(Label))
                {
                    var castControl = (Label) showControl;

                    // Set color
                    var oldColor = castControl.ForeColor;
                    castControl.ForeColor = color;

                    // Check if the logger add failed
                    if (stateMessageError != "")
                        stateMessage += @" " + stateMessageError + Environment.NewLine;

                    // Set state message
                    castControl.Text = stateMessage;
                    // Reset color
                    castControl.ForeColor = oldColor;

                    result = true;
                }

                // ToolStripStatusLabel
                if (showControl.GetType() == typeof(ToolStripStatusLabel))
                {
                    var castControl = (ToolStripStatusLabel) showControl;

                    // Set color
                    // var oldColor = castControl.ForeColor;
                    castControl.ForeColor = color;

                    // Check if the logger add failed
                    if (stateMessageError != "")
                        stateMessage += @" " + stateMessageError + Environment.NewLine;

                    // Set state message
                    castControl.Text = stateMessage;
                    // Reset color
                    //castControl.ForeColor = oldColor;

                    result = true;
                }

                // Check show given exception 
                ShowExceptionMessage(exception, xmlLanguage, language);

                return result;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, xmlLanguage, language);

                return false;
            }
        }

        #endregion Functions for the logging and debugging

        #region Update of a share or all shares are finished

        /// <summary>
        /// This functions updates the caption of the groupbox of the shareportfolio and plays the finish sound when the update is done
        /// </summary>
        /// <param name="grpBoxSharePortfolio">GroupBox with the shares of the portfolio where the caption should be updated</param>
        /// <param name="ShareObjectList">List with the final share objects list</param>
        public static void UpdateDone(GroupBox grpBoxSharePortfolio, List<ShareObjectFinalValue> ShareObjectList)
        {
            UpdateGroupBoxPortfolioCaption(grpBoxSharePortfolio, ShareObjectList);

            Sound.PlayUpdateFinishedSound();
        }

        #endregion Update of a share or all shares are finished

        #region Gridview SharePortfolio caption update

        /// <summary>
        /// This functions updates the caption of the groupbox of the shareportfolio
        /// </summary>
        /// <param name="grpBoxSharePortfolio">GroupBox with the shares of the portfolio where the caption should be updated</param>
        /// <param name="ShareObjectList">List with the final share objects list</param>
        public static void UpdateGroupBoxPortfolioCaption(GroupBox grpBoxSharePortfolio, List<ShareObjectFinalValue> ShareObjectList)
        {
            grpBoxSharePortfolio.Text =
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Caption", SettingsConfiguration.LanguageName) +
                @" ( " +
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/Entries", SettingsConfiguration.LanguageName) +
                @": " +
                ShareObjectList.Count + @" ) / " +
                LanguageConfiguration.Language.GetLanguageTextByXPath(@"/MainForm/GrpBoxPortfolio/LastUpdate",
                    SettingsConfiguration.LanguageName) + " " +
                GetLastShareUpdate(ShareObjectList);
        }

        #endregion Gridview SharePortfolio caption update

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
        public static void EnableDisableControls(bool flag, Control givenControl, List<string> listControlNames)
        {
            try
            {
                // Menu strip
                if (givenControl.GetType() == typeof(MenuStrip))
                {
                    foreach (var control in ((MenuStrip) givenControl).Items)
                    {
                        // MenuStrip item
                        if (control.GetType() == typeof(ToolStripMenuItem))
                        {
                            var castControl = (ToolStripMenuItem) control;
#if DEBUG_HELPER
                            Console.WriteLine(castControl.Name);
#endif

                            if (listControlNames.Contains(castControl.Name))
                                castControl.Enabled = flag;

                            // Loop through the dropdown items
                            foreach (var controlDropDropDownItem in castControl.DropDown.Items)
                            {
                                if (controlDropDropDownItem.GetType() == typeof(ToolStripMenuItem))
                                {
                                    var castControlDropDropDownItem = (ToolStripMenuItem) controlDropDropDownItem;
                                    if (listControlNames.Contains(castControlDropDropDownItem.Name))
                                        castControlDropDropDownItem.Enabled = flag;

                                }
                            }
                        }
                    }
                }

                foreach (var control in givenControl.Controls)
                {
                    // MenuStrip
                    if (control.GetType() == typeof(MenuStrip))
                    {
                        var castControl = (MenuStrip) control;
#if DEBUG_HELPER
                        Console.WriteLine(castControl.Name);
#endif

                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;

                        if (castControl.Controls.Count > 0)
                            EnableDisableControls(flag, castControl, listControlNames);
                    }

                    // GroupBox
                    if (control.GetType() == typeof(GroupBox))
                    {
                        var castControl = (GroupBox) control;
#if DEBUG_HELPER
                        Console.WriteLine(castControl.Name);
#endif

                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;

                        if (castControl.Controls.Count > 0)
                            EnableDisableControls(flag, castControl, listControlNames);
                    }

                    // Button
                    if (control.GetType() == typeof(Button))
                    {
                        var castControl = (Button) control;
#if DEBUG_HELPER
                        Console.WriteLine(castControl.Name);
#endif

                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;

                        if (castControl.Controls.Count > 0)
                            EnableDisableControls(flag, castControl, listControlNames);
                    }

                    // DataGridView
                    if (control.GetType() == typeof(DataGridView))
                    {
                        var castControl = (DataGridView) control;
#if DEBUG_HELPER
                        Console.WriteLine(castControl.Name);
#endif

                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;

                        if (castControl.Controls.Count > 0)
                            EnableDisableControls(flag, castControl, listControlNames);
                    }

                    // DataGridView
                    if (control.GetType() == typeof(TableLayoutPanel))
                    {
                        var castControl = (TableLayoutPanel) control;
#if DEBUG_HELPER
                        Console.WriteLine(castControl.Name);
#endif

                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;

                        if (castControl.Controls.Count > 0)
                            EnableDisableControls(flag, castControl, listControlNames);
                    }

                    // TabControl
                    if (control.GetType() == typeof(TabControl))
                    {
                        var castControl = (TabControl) control;
#if DEBUG_HELPER
                        Console.WriteLine(castControl.Name);
#endif
                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;

                        if (castControl.Controls.Count > 0)
                            EnableDisableControls(flag, castControl, listControlNames);
                    }

                    // Textbox
                    if (control.GetType() != typeof(TextBox)) continue;
                    {
                        var castControl = (TextBox) control;
#if DEBUG_HELPER
                        Console.WriteLine(castControl.Name);
#endif
                        if (listControlNames.Contains(castControl.Name))
                            castControl.Enabled = flag;

                        if (castControl.Controls.Count > 0)
                            EnableDisableControls(flag, castControl, listControlNames);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }
        }

        #endregion Enable or disable controls

        #region Get image for the given file

        public static Image GetImageForFile(string strFile)
        {
            // Get extension of the given file
            var strExtension = Path.GetExtension(strFile);

            if (string.Compare(strExtension, @".pdf", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return Resources.doc_pdf_image_24;
            }

            if (string.Compare(strExtension, @".xlsx", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(strExtension, @".xls", StringComparison.OrdinalIgnoreCase) == 0
            )
            {
                return Resources.doc_excel_image_24;
            }

            if (string.Compare(strExtension, @".docx", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(strExtension, @".doc", StringComparison.OrdinalIgnoreCase) == 0
            )
            {
                return Resources.doc_excel_image_24;
            }

            return Resources.empty_arrow;
        }

        #endregion Get image for the given file

        #region Scroll DataGridView to given index

        /// <summary>
        /// This function scrolls in the given DataGridView to the given index.
        /// The row with the given index would be the last seen row in the DataGridView.
        /// </summary>
        /// <param name="dataGridView">DataGridView</param>
        /// <param name="index">Index of the row where to scroll</param>
        /// <param name="lastDisplayedRowIndex">Index of the last displayed row of the data grid view</param>
        /// <param name="bAllowScrollUp">Flag if a scroll to the top is allowed</param>
        public static void ScrollDgvToIndex(DataGridView dataGridView, int index, int lastDisplayedRowIndex,
            bool bAllowScrollUp = false)
        {
            // Check if DataGridView is valid
            if (dataGridView == null || dataGridView.RowCount <= 0) return;

            // Check if index is valid
            if (index < 0 || index >= dataGridView.RowCount) return;

            // Check if the row is already in the displayed area of the DataGridView
            var iDisplayedRows = dataGridView.DisplayedRowCount(false);

            if (dataGridView.FirstDisplayedCell == null) return;

            var iFirstDisplayedRowIndex = 0;

            if (lastDisplayedRowIndex < dataGridView.RowCount)
                iFirstDisplayedRowIndex = lastDisplayedRowIndex;

#if DEBUG_HELPER
            Console.WriteLine(@"iFirstDisplayedRowIndex_1: {0}", iFirstDisplayedRowIndex);
            Console.WriteLine(@"iDisplayedRows_1: {0}", iDisplayedRows);
#endif

            if (bAllowScrollUp && index <= iFirstDisplayedRowIndex)
            {
                dataGridView.FirstDisplayedScrollingRowIndex = index;
//                lastDisplayedRowIndex = index;
            }
            else if (index + 1 > iFirstDisplayedRowIndex + iDisplayedRows && iDisplayedRows > 0)
            {
                if (iDisplayedRows < index + 1)
                    dataGridView.FirstDisplayedScrollingRowIndex = index - (iDisplayedRows - 1);
            }
            else
                dataGridView.FirstDisplayedScrollingRowIndex = lastDisplayedRowIndex;

#if DEBUG_HELPER
            Console.WriteLine(@"iFirstDisplayedRowIndex_2: {0}", iFirstDisplayedRowIndex);
            Console.WriteLine(@"iDisplayedRows_2: {0}", iDisplayedRows);
#endif
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
        /// <returns>Formatted string value of the given double</returns>
        public static string FormatDouble(double dblValue, int iPrecision, bool bFixed, int iFixedPrecision = 0,
            bool bUnit = false, string strUnit = "", CultureInfo cultureInfo = null)
        {
            var strFormatString = "";

            // Check if the given precision is greater than the maximum
            if (iPrecision > MaxPrecision)
                iPrecision = MaxPrecision;

            // Check if the given fixed precision is greater than the maximum
            if (iFixedPrecision > iPrecision)
                iFixedPrecision = iPrecision;

            // Calculate no fixed precision value
            var iNoFixedPrecision = iPrecision - iFixedPrecision - 1;

            if (iPrecision > 0)
            {
                // Create format string
                if (bFixed)
                {
                    for (var i = 1; i <= iPrecision; i++)
                    {
                        if (i == 1)
                            strFormatString += "0.0";
                        else
                            strFormatString += "0";
                    }
                }
                else
                {
                    for (var i = 1; i <= iPrecision; i++)
                    {
                        if (i > iNoFixedPrecision)
                        {
                            if (i == 1)
                                strFormatString += "0.#";
                            else
                                strFormatString += "#";
                        }
                        else
                        {
                            if (i == 1)
                                strFormatString += "0.0";
                            else
                                strFormatString += "0";
                        }
                    }
                }
            }
            else
            {
                strFormatString += "0";
            }

            // Format with the given culture info
            var strResult = cultureInfo != null
                ? dblValue.ToString(strFormatString, cultureInfo)
                : dblValue.ToString(strFormatString);

            // Check if a unit is given or should be set
            if (!bUnit)
                return strResult;

            if (!string.IsNullOrEmpty(strUnit))
                strResult = strResult + " " + strUnit;
            else
            {
                if (cultureInfo == null)
                    return strResult;

                var ri = new RegionInfo(cultureInfo.LCID);
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
        /// <returns>Formatted string of the given decimal</returns>
        public static string FormatDecimal(decimal decValue, int iPrecision, bool bFixed, int iFixedPrecision = 0,
            bool bUnit = false, string strUnit = "", CultureInfo cultureInfo = null)
        {
            var strFormatString = "";

            // Check if the given precision is greater than the maximum
            if (iPrecision > MaxPrecision)
                iPrecision = MaxPrecision;

            // Check if the given fixed precision is greater than the maximum
            if (iFixedPrecision > iPrecision)
                iFixedPrecision = iPrecision;

            // Calculate no fixed precision value
            var iNoFixedPrecision = iPrecision - iFixedPrecision - 1;

            if (iPrecision > 0)
            {
                // Create format string
                if (bFixed)
                {
                    for (var i = 1; i <= iPrecision; i++)
                    {
                        if (i == 1)
                            strFormatString += "0.0";
                        else
                            strFormatString += "0";
                    }
                }
                else
                {
                    for (var i = 1; i <= iPrecision; i++)
                    {
                        if (i > iNoFixedPrecision)
                        {
                            if (i == 1)
                                strFormatString += "0.#";
                            else
                                strFormatString += "#";
                        }
                        else
                        {
                            if (i == 1)
                                strFormatString += "0.0";
                            else
                                strFormatString += "0";
                        }
                    }
                }
            }
            else
            {
                strFormatString += "0";
            }

            // Format with the given culture info
            var strResult = cultureInfo != null
                ? decValue.ToString(strFormatString, cultureInfo)
                : decValue.ToString(strFormatString);

            // Check if a unit is given or should be set
            if (!bUnit)
                return strResult;

            if (!string.IsNullOrEmpty(strUnit))
                strResult = strResult + " " + strUnit;
            else
            {
                if (cultureInfo == null)
                    return strResult;

                var ri = new RegionInfo(cultureInfo.LCID);
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
        public static List<RegexOptions> GetRegexOptions(string strRegexOptions)
        {
            var regexOptionsList = new List<RegexOptions>();

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
                    default:
                        regexOptionsList.Add(RegexOptions.Singleline);
                        break;
                }
            }

            return regexOptionsList.Count > 0 ? regexOptionsList : null;
        }

        #endregion Get RegEx options

        #region Open file dialog for setting document

        /// <summary>
        /// This function show a open file dialog
        /// and returns the chosen file
        /// </summary>
        /// <param name="strTitle">Title for the open file dialog</param>
        /// <param name="strFilter">Filter for the open file dialog</param>
        /// <param name="strCurrentDocument">Given current document</param>
        /// <returns></returns>
        public static DialogResult SetDocument(string strTitle, string strFilter, ref string strCurrentDocument)
        {
            // Open file dialog
            OpenFileDialog openFileDlg;

            // Save old document
            var strOldDocument = strCurrentDocument;

            if (strCurrentDocument != "")
            {
                openFileDlg = new OpenFileDialog
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
                openFileDlg = new OpenFileDialog
                {
                    Title = strTitle,
                    ValidateNames = true,
                    SupportMultiDottedExtensions = false,
                    Multiselect = false,
                    RestoreDirectory = true,
                    Filter = strFilter
                };
            }

            var dlgResult = openFileDlg.ShowDialog();

            strCurrentDocument = dlgResult == DialogResult.OK ? openFileDlg.FileName : strOldDocument;

            return dlgResult;
        }

        #endregion Open file dialog for setting document

        #region Open file dialog for creating a new portfolio

        /// <summary>
        /// This function show a save file dialog
        /// and returns the chosen file
        /// </summary>
        /// <param name="strTitle">Title for the save file dialog</param>
        /// <param name="strFilter">Filter for the save file dialog</param>
        /// <param name="strCurrentPortfolio">Current loaded portfolio file</param>
        /// <returns>Chosen portfolio file</returns>
        public static DialogResult NewPortfolio(string strTitle, string strFilter, ref string strCurrentPortfolio)
        {
            // Save old portfolio file name
            var strOldPortfolioName = strCurrentPortfolio;

            var initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                   @"\SharePortfolioManager\Portfolios\";
            var restoreDirectory = false;

            if (strCurrentPortfolio != "")
            {
                initialDirectory = strCurrentPortfolio;
                restoreDirectory = true;
            }

            var saveFileDlg = new SaveFileDialog
            {
                Title = strTitle,
                InitialDirectory = initialDirectory,
                RestoreDirectory = restoreDirectory,
                FileName = "Portfolio.xml",
                ValidateNames = true,
                DefaultExt = @"xml",
                AddExtension = true,
                Filter = strFilter,
                FilterIndex = 1,
                SupportMultiDottedExtensions = false,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                OverwritePrompt = true,
            };
            var dlgResult = saveFileDlg.ShowDialog();

            strCurrentPortfolio = dlgResult == DialogResult.OK ? saveFileDlg.FileName : strOldPortfolioName;

            return dlgResult;
        }

        #endregion Open file dialog for creating a new portfolio

        #region Save file dialog for saving portfolio

        /// <summary>
        /// This function show a save file dialog
        /// and returns the chosen file
        /// </summary>
        /// <param name="strTitle">Title for the save file dialog</param>
        /// <param name="strFilter">Filter for the save file dialog</param>
        /// <param name="strCurrentPortfolio">Current loaded portfolio file</param>
        /// <returns>Chosen portfolio file</returns>
        public static DialogResult SaveAsPortfolio(string strTitle, string strFilter, ref string strCurrentPortfolio)
        {
            // Save old portfolio file name
            var strOldPortfolioName = strCurrentPortfolio;

            var initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                   @"\SharePortfolioManager\Portfolios\";
            var fileName = @"Portfolio.xml";
            var restoreDirectory = false;

            if (strCurrentPortfolio != "")
            {
                initialDirectory = strCurrentPortfolio;
                fileName = Path.GetFileName(strCurrentPortfolio);
                restoreDirectory = true;
            }

            var saveFileDlg = new SaveFileDialog
            {
                Title = strTitle,
                InitialDirectory = initialDirectory,
                RestoreDirectory = restoreDirectory,
                FileName = fileName,
                ValidateNames = true,
                DefaultExt = @"xml",
                AddExtension = true,
                Filter = strFilter,
                FilterIndex = 1,
                SupportMultiDottedExtensions = false,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                OverwritePrompt = true,
            };
            var dlgResult = saveFileDlg.ShowDialog();

            strCurrentPortfolio = dlgResult == DialogResult.OK ? saveFileDlg.FileName : strOldPortfolioName;

            return dlgResult;
        }

        #endregion Open file dialog for saving portfolio

        #region Open file dialog for loading portfolio

        /// <summary>
        /// This function show a open file dialog
        /// and returns the chosen file
        /// </summary>
        /// <param name="strTitle">Title for the open file dialog</param>
        /// <param name="strFilter">Filter for the open file dialog</param>
        /// <param name="strCurrentPortfolio">Current loaded portfolio file</param>
        /// <returns>Chosen portfolio file</returns>
        public static string LoadPortfolio(string strTitle, string strFilter, string strCurrentPortfolio)
        {
            // Save old portfolio file name
            var strOldPortfolioName = strCurrentPortfolio;

            var openFileDlg = new OpenFileDialog
            {
                Title = strTitle,
                ValidateNames = true,
                SupportMultiDottedExtensions = false,
                Multiselect = false,
                InitialDirectory = strCurrentPortfolio,
                RestoreDirectory = true,
                Filter = strFilter
            };
            var dlgResult = openFileDlg.ShowDialog();

            strCurrentPortfolio = dlgResult == DialogResult.OK ? openFileDlg.FileName : strOldPortfolioName;

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
            if (Path.GetFileName(strPath) != null && Path.GetFileName(strPath) != string.Empty)
                return Path.GetFileName(strPath);

            return @"-";
        }

        #endregion Get file name of the given path

        #region Get currency list

        /// <summary>
        /// This function returns a list of all currency
        /// in three letter ISO name and the ISO currency unit
        /// </summary>
        /// <returns>IEnumerable with a KeyValuePair list</returns>
        public static void CreateNameCultureInfoCurrencySymbolList()
        {
            ListNameCultureInfoCurrencySymbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new KeyValuePair<string, CultureInformation>(culture.Name,
                            new CultureInformation(culture, new RegionInfo(culture.LCID).CurrencySymbol));
                    }
                    catch
                    {
                        return new KeyValuePair<string, CultureInformation>(null, null);
                    }
                })
                .Where(ci => ci.Key != null)
                .OrderBy(ci => ci.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            ListNameCultureInfoCurrencySymbol = ListNameCultureInfoCurrencySymbol.OrderBy(ci => ci.Key);
        }

        #endregion Get currency list

        #region Get culture info by name

        public static CultureInfo GetCultureByName(string name)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            foreach (var temp in cultures)
            {
                if (temp.Name == name)
                    return temp;
            }

            return null;
        }

        #endregion Get culture info by name

        #region Calculate market value, market value plus brokerage

        public static void CalcBuyValues(decimal decVolume, decimal decSharePrice,
            decimal decProvision, decimal decBrokerFee, decimal decTraderPlaceFee, decimal decReduction,
            out decimal decBuyValue, out decimal decBuyValueReduction, out decimal decBuyValueBrokerage,
            out decimal decBuyValueBrokerageReduction, out decimal decBrokerage, out decimal decBrokerageReduction)
        {
            // Calculate brokerage
            CalcBrokerageValues(decProvision, decBrokerFee, decTraderPlaceFee, decReduction, out decBrokerage,
                out decBrokerageReduction);

            // Calculate market value and deposit ( market value + brokerage )
            if (decVolume > 0 && decSharePrice > 0)
            {
                decBuyValue = Math.Round(decVolume * decSharePrice, 2, MidpointRounding.AwayFromZero);
                decBuyValueBrokerage = decBuyValue + decBrokerage;
                decBuyValueReduction = decBuyValue - decReduction;
                decBuyValueBrokerageReduction = decBuyValue + decBrokerage - decReduction;
            }
            else
            {
                decBuyValue = 0;
                decBuyValueReduction = 0;
                decBuyValueBrokerage = 0;
                decBuyValueBrokerageReduction = 0;
            }
        }

        #endregion Calculate market value, market value plus brokerage

        #region Calculate brokerage value

        public static void CalcBrokerageValues(decimal decProvision, decimal decBrokerFee, decimal decTraderPlaceFee,
            decimal decReduction,
            out decimal decBrokerage, out decimal decBrokerageReduction)
        {
            // Calculate brokerage
            decBrokerage = decProvision + decBrokerFee + decTraderPlaceFee;
            decBrokerageReduction = decBrokerage - decReduction;
        }

        #endregion Calculate brokerage value

        #region URL checker

        public static bool UrlChecker(ref string url, string apiKey, int timeout)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            try
            {
                var urlCheck = new Uri(url);
                var request = (HttpWebRequest) WebRequest.Create(urlCheck);
                request.Timeout = timeout;
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0";
                request.Method = "GET";
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 1;

                // Check if an API key is given
                if (apiKey != string.Empty)
                {
                    request.Headers["X-API-KEY"] = apiKey;
                }

                // Using is necessary here, because after various response the connection fails with timeout
                HttpWebResponse response;
                using (response = (HttpWebResponse) request.GetResponse())
                {
#if DEBUG_HELPER
                    Console.WriteLine(@"Uri:" + response.ResponseUri);
                    Console.WriteLine(@"Status:" + response.StatusDescription);
                    Console.WriteLine(@"Response:" + response.StatusCode);
#endif

                    return (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found) &&
                           url == response.ResponseUri.ToString();
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);

                return false; //could not connect to the inter net (maybe) 
            }
        }

        public static string RegexReplaceStartDateAndInterval(string strWebSite, ShareObject.ParsingTypes parsingType)
        {
            if (parsingType == ShareObject.ParsingTypes.ApiOnvista)
            {
                // Replace date for example 01.01.1979
                strWebSite = Regex.Replace(strWebSite,
                    "[=]([0-9][0-9].[0-9][0-9].[0-9][0-9][0-9][0-9])[&]", "={0}&");

                // Replace date for example 01-01-1979
                strWebSite = Regex.Replace(strWebSite,
                    "[=]([0-9][0-9]-[0-9][0-9]-[0-9][0-9][0-9][0-9])[&]", "={0}&");

                // Replace date for example 1979.01.01
                strWebSite = Regex.Replace(strWebSite,
                    "[=]([0-9][0-9][0-9][0-9].[0-9][0-9].[0-9][0-9])[&]", "={0}&");

                // Replace date for example 1979-01-01
                strWebSite = Regex.Replace(strWebSite,
                    "[=]([0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9])[&]", "={0}&");

                // Replace ranges
                strWebSite = Regex.Replace(strWebSite,
                    "[e][=]([M,Y][1,3,5,6])[&]", "e={1}&");

                strWebSite = Regex.Replace(strWebSite,
                    "[e][=]([M,Y][1,3,5,6])", "e={1}");

                strWebSite = Regex.Replace(strWebSite,
                    "[e][=]([1,3,5,6][M,Y])[&]", "e={1}&");

                strWebSite = Regex.Replace(strWebSite,
                    "[e][=]([1,3,5,6][M,Y])[&]", "e={1}&");
            }
            else if (parsingType == ShareObject.ParsingTypes.ApiYahoo)
            {
                // Replace ranges for 1d / 5d
                strWebSite = Regex.Replace(strWebSite,
                    "[e][=]([1,5][d])[&]", "e={0}&");
                // Replace ranges for 1m / 3m / 6m
                strWebSite = Regex.Replace(strWebSite,
                    "[e][=]([1,3,6][m][o])[&]", "e={0}&");
                // Replace ranges for 1y / 5y / 10y / 20y
                strWebSite = Regex.Replace(strWebSite,
                    "[e][=]([1,5][y]|[1][0][y]|[2][0][y])[&]", "e={0}&");
            }
            else
            {
            }

            return strWebSite;
        }

        #endregion URL checker

        #region Remove double whitespaces and line feeds ( \n \r )

        public static string RemoveDoubleWhiteSpaces(string strInput)
        {
            if (strInput == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            var ignoreWhitespace = false;
            foreach (var c in strInput)
            {
                if ((!ignoreWhitespace || c != ' ') && c != '\n' && c != '\r')
                {
                    builder.Append(c);
                }

                ignoreWhitespace = c == ' ';
            }

            return builder.ToString();
        }

        #endregion Remove double whitespaces and line feeds ( \n \r )

        #region Async process

        public static string ProcessStdOutput { internal set; get; }
        public static string ProcessErrOutput { internal set; get; }

        public static int RunProcess(string fileName, string args)
        {
            try
            {
                // Reset output strings
                ProcessStdOutput = string.Empty;
                ProcessErrOutput = string.Empty;

                using (var process = new Process
                {
                    StartInfo =
                    {
                        FileName = fileName,
                        Arguments = args,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    },
                    EnableRaisingEvents = true
                })
                {
                    return RunProcess(process);
                }
            }
            catch (OperationCanceledException ex)
            {
                ShowExceptionMessage(ex);

                throw;
            }
            catch (InvalidOperationException ex)
            {
                ShowExceptionMessage(ex);

                throw;
            }
        }

        private static int RunProcess(Process process)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, ea) => tcs.SetResult(process.ExitCode);
            process.OutputDataReceived += (s, ea) => ProcessStdOutput += ea.Data;
            process.ErrorDataReceived += (s, ea) => ProcessErrOutput += ea.Data;

            var started = process.Start();
            if (!started)
            {
                //you may allow for the process to be re-used (started = false) 
                //but I'm not sure about the guarantees of the Exited event in such a case
                throw new InvalidOperationException("Could not start process: " + process);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task.Result;
        }

        #endregion Async process

        #region Time function

        public static string BuildDailyValuesUrl(List<Parser.DailyValues> dailyValues, string webSiteUrl,
            ShareObject.ShareTypes shareType, ShareObject.ParsingTypes parsingType)
        {
            var strDailyValuesWebSite = "";

            try
            {
                // Check if any daily values already exists
                if (dailyValues.Count == 0)
                {
                    switch (parsingType)
                    {
                        case ShareObject.ParsingTypes.Regex:
                            {
                                var date = DateTime.Now;

                                // Go five years back
                                date = date.AddYears(-5);

                                switch (shareType)
                                {
                                    // Share type "Share"
                                    case ShareObject.ShareTypes.Share:
                                        {
                                            strDailyValuesWebSite = string.Format(webSiteUrl,
                                                date.ToString("dd.MM.yyyy"), "Y5");
                                        }
                                        break;
                                    // Share type "Fond"
                                    case ShareObject.ShareTypes.Fond:
                                        {
                                            strDailyValuesWebSite = string.Format(webSiteUrl,
                                                date.ToString("dd.MM.yyyy"), "5Y");
                                        }
                                        break;
                                    // Share type "ETF"
                                    case ShareObject.ShareTypes.Etf:
                                        {
                                            strDailyValuesWebSite = string.Format(webSiteUrl,
                                                date.ToString("dd.MM.yyyy"), "5Y");
                                        }
                                        break;
                                    default:
                                        {
                                            throw new NotImplementedException();
                                        }
                                }
                            }
                            break;
                        case ShareObject.ParsingTypes.ApiOnvista:
                            {
                                var date = DateTime.Now;

                                // Go five years back
                                date = date.AddYears(-5);

                                switch (shareType)
                                {
                                    // Share type "Share"
                                    case ShareObject.ShareTypes.Share:
                                        {
                                            strDailyValuesWebSite = string.Format(webSiteUrl,
                                                date.ToString("yyyy-MM-dd"), "Y5");
                                        }
                                        break;
                                    // Share type "Fond"
                                    case ShareObject.ShareTypes.Fond:
                                        {
                                            strDailyValuesWebSite = string.Format(webSiteUrl,
                                                date.ToString("yyyy-MM-dd"), "Y5");
                                        }
                                        break;
                                    // Share type "ETF"
                                    case ShareObject.ShareTypes.Etf:
                                        {
                                            strDailyValuesWebSite = string.Format(webSiteUrl,
                                                date.ToString("yyyy-MM-dd"), "Y5");
                                        }
                                        break;
                                    default:
                                        {
                                            throw new NotImplementedException();
                                        }
                                }
                            }
                            break;
                        case ShareObject.ParsingTypes.ApiYahoo:
                            {
                                strDailyValuesWebSite = string.Format(webSiteUrl, "20y");
                            }
                            break;
                        default:
                            {
                                throw new NotImplementedException();
                            }
                    }
                }
                else
                {
                    // Get date of last daily values entry
                    var lastDate = dailyValues.Last().Date;

                    // Check if the days are less or equal than 27 days
                    var diffMonth = GetTotalMonthsFrom(DateTime.Now, lastDate);

                    switch (parsingType)
                    {
                        case ShareObject.ParsingTypes.Regex:
                        {
                            switch (shareType)
                            {
                                // Share type "Share"
                                case ShareObject.ShareTypes.Share:
                                    {
                                            if (diffMonth < 1)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy"), "M1");
                                            }
                                            else if (diffMonth < 3)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-3).ToString("dd.MM.yyyy"), "M3");
                                            }
                                            else if (diffMonth < 6)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-6).ToString("dd.MM.yyyy"), "M6");
                                            }
                                            else if (diffMonth < 12)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-12).ToString("dd.MM.yyyy"), "Y1");
                                            }
                                            else if (diffMonth < 36)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-36).ToString("dd.MM.yyyy"), "Y3");
                                            }
                                            else
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-60).ToString("dd.MM.yyyy"), "Y5");
                                            }
                                    }
                                    break;
                                // Share type "Fond"
                                case ShareObject.ShareTypes.Fond:
                                    {
                                            if (diffMonth < 1)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy"), "1M");
                                            }
                                            else if (diffMonth < 3)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-3).ToString("dd.MM.yyyy"), "3M");
                                            }
                                            else if (diffMonth < 6)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-6).ToString("dd.MM.yyyy"), "6M");
                                            }
                                            else if (diffMonth < 12)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                         DateTime.Now.AddMonths(-12).ToString("dd.MM.yyyy"), "1Y");
                                            }
                                            else if (diffMonth < 36)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-36).ToString("dd.MM.yyyy"), "3Y");
                                            }
                                            else
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-60).ToString("dd.MM.yyyy"), "5Y");
                                            }
                                    }
                                    break;
                                // Share type "ETF"
                                case ShareObject.ShareTypes.Etf:
                                    {
                                            if (diffMonth < 1)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-1).ToString("dd.MM.yyyy"), "1M");
                                            }
                                            else if (diffMonth < 3)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                         DateTime.Now.AddMonths(-3).ToString("dd.MM.yyyy"), "3M");
                                            }
                                            else if (diffMonth < 6)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-6).ToString("dd.MM.yyyy"), "6M");
                                            }
                                            else if (diffMonth < 12)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-12).ToString("dd.MM.yyyy"), "1Y");
                                            }
                                            else if (diffMonth < 36)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-36).ToString("dd.MM.yyyy"), "3Y");
                                            }
                                            else
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                        DateTime.Now.AddMonths(-60).ToString("dd.MM.yyyy"), "5Y");
                                            }
                                    }
                                    break;
                                default:
                                    {
                                        throw new NotImplementedException();
                                    }
                            }
                        }
                        break;
                        case ShareObject.ParsingTypes.ApiOnvista:
                            {
                                switch (shareType)
                                {
                                    // Share type "Share"
                                    case ShareObject.ShareTypes.Share:
                                        {
                                            if (diffMonth < 1)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"), "M1");
                                            }
                                            else if (diffMonth < 3)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd"), "M3");
                                            }
                                            else if (diffMonth < 6)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd"), "M6");
                                            }
                                            else if (diffMonth < 12)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"), "Y1");
                                            }
                                            else if (diffMonth < 36)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-36).ToString("yyyy-MM-dd"), "Y3");
                                            }
                                            else
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-60).ToString("yyyy-MM-dd"), "Y5");
                                            }
                                        }
                                        break;
                                    // Share type "Fond"
                                    case ShareObject.ShareTypes.Fond:
                                        {
                                            if (diffMonth < 1)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"), "M1");
                                            }
                                            else if (diffMonth < 3)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd"), "M3");
                                            }
                                            else if (diffMonth < 6)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd"), "M6");
                                            }
                                            else if (diffMonth < 12)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"), "Y1");
                                            }
                                            else if (diffMonth < 36)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-36).ToString("yyyy-MM-dd"), "Y3");
                                            }
                                            else
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-60).ToString("yyyy-MM-dd"), "Y5");
                                            }
                                        }
                                        break;
                                    // Share type "ETF"
                                    case ShareObject.ShareTypes.Etf:
                                        {
                                            if (diffMonth < 1)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"), "M1");
                                            }
                                            else if (diffMonth < 3)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd"), "M3");
                                            }
                                            else if (diffMonth < 6)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd"), "M6");
                                            }
                                            else if (diffMonth < 12)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"), "Y1");
                                            }
                                            else if (diffMonth < 36)
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-36).ToString("yyyy-MM-dd"), "Y3");
                                            }
                                            else
                                            {
                                                strDailyValuesWebSite = string.Format(webSiteUrl,
                                                    DateTime.Now.AddMonths(-60).ToString("yyyy-MM-dd"), "Y5");
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            throw new NotImplementedException();
                                        }
                                }
                            }
                            break;
                        case ShareObject.ParsingTypes.ApiYahoo:
                            {
                                if (diffMonth < 1)
                                {
                                    strDailyValuesWebSite = string.Format(webSiteUrl, "1mo");
                                }
                                else if (diffMonth < 3)
                                {
                                    strDailyValuesWebSite = string.Format(webSiteUrl, "3mo");
                                }
                                else if (diffMonth < 6)
                                {
                                    strDailyValuesWebSite = string.Format(webSiteUrl, "6mo");
                                }
                                else if (diffMonth < 12)
                                {
                                    strDailyValuesWebSite = string.Format(webSiteUrl, "1y");
                                }
                                else if (diffMonth < 36)
                                {
                                    strDailyValuesWebSite = string.Format(webSiteUrl, "3y");
                                }
                                else
                                {
                                    strDailyValuesWebSite = string.Format(webSiteUrl, "5y");
                                }
                            }
                            break;
                        default:
                            {
                                throw new NotImplementedException();
                            }
                    }
                }

                // Replace possbile "&amp;" with "&"
                strDailyValuesWebSite = strDailyValuesWebSite.Replace("&amp;", "&");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }

#if DEBUG_HELPER
            Console.WriteLine(@"WebSite: {0}", strDailyValuesWebSite);
#endif
            return strDailyValuesWebSite;
        }

        public static int GetTotalMonthsFrom(DateTime dt1, DateTime dt2)
        {
            var earlyDate = (dt1 > dt2) ? dt2.Date : dt1.Date;
            var lateDate = (dt1 > dt2) ? dt1.Date : dt2.Date;

            // Start with 1 month's difference and keep incrementing
            // until we overshoot the late date
            var monthsDiff = 1;
            while (earlyDate.AddMonths(monthsDiff) <= lateDate)
            {
                monthsDiff++;
            }

            return monthsDiff - 1;
        }

        public static string GetLastShareUpdate(List<ShareObjectFinalValue> listShareObjectFinalValues)
        {
            // Check if no object is in the list so retrun "-"
            if(listShareObjectFinalValues.Count == 0)
            {
                return "-";
            }   
            
            DateTime lastUpdateTime = default;

            foreach (var listShareObjectFinalValue in listShareObjectFinalValues)
            {
                if (listShareObjectFinalValue.LastUpdateViaInternet > lastUpdateTime)
                    lastUpdateTime = listShareObjectFinalValue.LastUpdateViaInternet;
            }

            return lastUpdateTime.ToShortDateString() + " " + lastUpdateTime.ToShortTimeString();
        }

        #endregion Time function

        #region Get API key

        public static string GetApiKey(Dictionary<string, string> ApiKeysDictionary, ShareObject.ParsingTypes parsingType)
        {
            if(ApiKeysDictionary.ContainsKey(parsingType.ToString()))
            { 
                return ApiKeysDictionary[parsingType.ToString()];
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Get API key

        #endregion Methods

        #region Webbrowser for the PDF show

        /// <summary>
        /// This class allows to show a PDF in a WebBrowser
        /// </summary>
        public static class WebBrowserPdf
        {
            /// <summary>
            /// This function clears the WebBrowser and loads
            /// then the given PDF file to the WebBrowser
            /// </summary>
            /// <param name="sender">WebBrowser where the PDF should be shown</param>
            /// <param name="fileName">Filename of the given PDF file which should be shown</param>
            public static void Reload(WebBrowser sender, string fileName)
            {
                if (sender == null) return;

                sender.Visible = false;
                sender.Navigate(new Uri("about:blank"));

                while (sender.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(10);
                }

                sender.Navigate(fileName);

                sender.Visible = true;
            }

            /// <summary>
            /// This function cleans up the WebBrowser
            /// </summary>
            /// <param name="sender">WebBrowser object</param>
            public static void CleanUp(WebBrowser sender)
            {
                if (sender == null) return;

                sender.Visible = false;
                sender.Navigate(new Uri("about:blank"));

                while (sender.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(10);
                }

                sender.Dispose();
                System.Threading.Thread.Sleep(100);
            }
        }

        #endregion Webbrowser for the PDF show
    }

    internal static class DataGridViewHelper
    {
        #region Constants DataGridView

        public static readonly Color DataGridViewHeaderColors = SystemColors.GrayText;

        #endregion Constatns DataGridView

        #region Methods

        public static void DataGridViewConfiguration(DataGridView dgv)
        {
            dgv.Font = new Font(@"Consolas", 9);

            // Column header styling
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 25;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            var styleColumnHeader = dgv.ColumnHeadersDefaultCellStyle;
            styleColumnHeader.Alignment = DataGridViewContentAlignment.MiddleCenter;
            styleColumnHeader.BackColor = DataGridViewHeaderColors;
            styleColumnHeader.SelectionBackColor = DataGridViewHeaderColors;

            // Row styling
            dgv.RowHeadersVisible = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;

            // Cell styling
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.Blue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Yellow;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Allow styling
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
        }

        public static void DataGridViewConfigurationFooter(DataGridView dgv)
        {
            dgv.Font = new Font(@"Consolas", 9, FontStyle.Bold);

            // Column header styling
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 25;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            var styleColumnHeader = dgv.ColumnHeadersDefaultCellStyle;
            styleColumnHeader.Alignment = DataGridViewContentAlignment.MiddleCenter;
            styleColumnHeader.BackColor = DataGridViewHeaderColors;
            styleColumnHeader.SelectionBackColor = DataGridViewHeaderColors;

            // Row styling
            dgv.RowHeadersVisible = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.DimGray;
            dgv.RowsDefaultCellStyle.BackColor = Color.DimGray;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;

            // Cell styling
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.DimGray;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Allow styling
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
        }

        #endregion Methods
    }
}

// This class stores the information of a culture info
public class CultureInformation
    {
        public string CurrencySymbol { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public CultureInformation(string currencySymbol)
        {
            CurrencySymbol = currencySymbol;
        }

        public CultureInformation(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        public CultureInformation(CultureInfo cultureInfo, string currencySymbol)
        {
            CurrencySymbol = currencySymbol;
            CultureInfo = cultureInfo;
        }
    }

    /// <summary>
    /// This class allows to center a message box to the center of the parent dialog
    /// Code is taken from the following web site
    /// http://www.jasoncarr.com/technology/centering-a-message-box-on-the-active-window-in-csharp
    /// </summary>
    internal static class MessageBoxHelper
    {
        internal static void PrepToCenterMessageBoxOnForm(Form form)
        {
            var helper = new MessageBoxCenterHelper();
            helper.Prep(form);
        }

        private class MessageBoxCenterHelper
        {
            private int _messageHook;
            private IntPtr _parentFormHandle;

            public void Prep(IWin32Window form)
            {
                var callBackDelegate = new NativeMethods.CenterMessageCallBackDelegate(CenterMessageCallBack);
                GCHandle.Alloc(callBackDelegate);

                _parentFormHandle = form.Handle;
                _messageHook = NativeMethods.SetWindowsHookEx(5, callBackDelegate,
                        new IntPtr(NativeMethods.GetWindowLong(_parentFormHandle, -6)),
                        NativeMethods.GetCurrentThreadId())
                    .ToInt32();
            }

            private int CenterMessageCallBack(int message, int wParam, int lParam)
            {
                if (message != 5) return 0;

                NativeMethods.GetWindowRect(_parentFormHandle, out var formRect);
                NativeMethods.GetWindowRect(new IntPtr(wParam), out var messageBoxRect);

                var xPos = (formRect.Left + (formRect.Right - formRect.Left) / 2) -
                           ((messageBoxRect.Right - messageBoxRect.Left) / 2);
                var yPos = (formRect.Top + (formRect.Bottom - formRect.Top) / 2) -
                           ((messageBoxRect.Bottom - messageBoxRect.Top) / 2);

                NativeMethods.SetWindowPos(wParam, 0, xPos, yPos, 0, 0, 0x1 | 0x4 | 0x10);
                NativeMethods.UnhookWindowsHookEx(_messageHook);

                return 0;
            }
        }

        private static class NativeMethods
        {
            internal readonly struct Rect
            {
                //public static Rect CreateInstance(int left, int top, int right, int bottom)
                //{
                //    return new Rect(left, top, right, bottom);
                //}

                public readonly int Left;
                public readonly int Top;
                public readonly int Right;
                public readonly int Bottom;

                // ReSharper disable once UnusedMember.Local
                private Rect(int left, int top, int right, int bottom)
                {
                    Left = left;
                    Top = top;
                    Right = right;
                    Bottom = bottom;
                }
            }

            internal delegate int CenterMessageCallBackDelegate(int message, int wParam, int lParam);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool UnhookWindowsHookEx(int hhk);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("kernel32.dll")]
            internal static extern int GetCurrentThreadId();

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr SetWindowsHookEx(int hook, CenterMessageCallBackDelegate callback,
                IntPtr hMod, int dwThreadId);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
                int uFlags);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        }
    }
