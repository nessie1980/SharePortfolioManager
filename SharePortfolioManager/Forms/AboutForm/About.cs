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
using System;
using System.Reflection;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class FrmAbout : Form
    {
        #region Variables

        /// <summary>
        /// Stores the parent window
        /// </summary>
        private FrmMain _parentWindow;

        /// <summary>
        /// Stores the language file
        /// </summary>
        private Language XmlLanguage;

        /// <summary>
        /// Stores language
        /// </summary>
        private String _strLanguage;

        /// <summary>
        /// Flag if the form should be closed
        /// </summary>
        private bool _stopFomClosing;

        #endregion Variables

        #region Form

        /// <summary>
        /// Constructor
        /// </summary>
        public FrmAbout(FrmMain parentWindow, Language xmlLanguage, String strLanguage)
        {
            InitializeComponent();

            _parentWindow = parentWindow;
            XmlLanguage = xmlLanguage;
            _strLanguage = strLanguage;
            _stopFomClosing = false;
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            #region Language configuration

            Text = XmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Caption", _strLanguage);
            grpBoxVersions.Text = XmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/Version", _strLanguage);
            lblApplicationVersion.Text = XmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/ApplicationVersion", _strLanguage);
            lblWebParserDllVersion.Text = XmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/WebParserDLLVersion", _strLanguage);
            lblLanguageDllVersion.Text = XmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/LanguageDLLVersion", _strLanguage);
            lblLoggerDllVersion.Text = XmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/LoggerDLLVersion", _strLanguage);

            btnOk.Text = XmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Buttons/Ok", _strLanguage);

            #endregion Language configuration

            Assembly assApp = typeof(FrmMain).Assembly;
            AssemblyName assAppName = assApp.GetName();
            Version verApp = assAppName.Version;

            lblApplicationVersionValue.Text = verApp.ToString();

            Assembly assWebParser = typeof(WebParser.WebParser).Assembly;
            AssemblyName assWebParserName = assWebParser.GetName();
            Version verWebParser = assWebParserName.Version;

            lblWebParserDllVersionValue.Text = verWebParser.ToString();

            Assembly assLanguage = typeof(LanguageHandler.Language).Assembly;
            AssemblyName assLanguageName = assLanguage.GetName();
            Version verLanguage = assLanguageName.Version;

            lblLanguageDllVersionValue.Text = verLanguage.ToString();

            Assembly assLogger = typeof(Logging.Logger).Assembly;
            AssemblyName assLoggerName = assLogger.GetName();
            Version verLogger = assLoggerName.Version;

            lblLoggerDllVersionValue.Text = verLogger.ToString();
        }

        /// <summary>
        /// This function is called when the form is closing
        /// Here we check if the form really should be closed
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void FrmAbout_FormClosing(object sender, FormClosingEventArgs e)
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

        #endregion Form

        #region Button

        /// <summary>
        /// This function close the form
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnOk_Click(object sender, EventArgs e)
        {
            _stopFomClosing = false;
            Close();
        }

        #endregion Button
    }
}
