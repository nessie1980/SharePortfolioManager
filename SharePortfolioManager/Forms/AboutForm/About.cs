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
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class FrmAbout : Form
    {
        #region Variables

        /// <summary>
        /// Stores the language file
        /// </summary>
        private readonly Language _xmlLanguage;

        /// <summary>
        /// Stores language
        /// </summary>
        private readonly string _strLanguage;

        /// <summary>
        /// Flag if the form should be closed
        /// </summary>
        private bool _stopFomClosing;

        #endregion Variables

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmAbout(Language xmlLanguage, string strLanguage)
        {
            InitializeComponent();

            _xmlLanguage = xmlLanguage;
            _strLanguage = strLanguage;
            _stopFomClosing = false;
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            #region Language configuration

            Text = _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Caption", _strLanguage);
            grpBoxVersions.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/Version", _strLanguage);
            lblApplicationVersion.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/ApplicationVersion", _strLanguage);
            lblWebParserDllVersion.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/WebParserDLLVersion", _strLanguage);
            lblLanguageDllVersion.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/LanguageDLLVersion", _strLanguage);
            lblLoggerDllVersion.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Labels/LoggerDLLVersion", _strLanguage);

            btnOk.Text = _xmlLanguage.GetLanguageTextByXPath(@"/AboutForm/Buttons/Ok", _strLanguage);

            #endregion Language configuration

            var assApp = typeof(FrmMain).Assembly;
            var assAppName = assApp.GetName();
            var verApp = assAppName.Version;

            lblApplicationVersionValue.Text = verApp.ToString();

            var assWebParser = typeof(WebParser.WebParser).Assembly;
            var assWebParserName = assWebParser.GetName();
            var verWebParser = assWebParserName.Version;

            lblWebParserDllVersionValue.Text = verWebParser.ToString();

            var assLanguage = typeof(Language).Assembly;
            var assLanguageName = assLanguage.GetName();
            var verLanguage = assLanguageName.Version;

            lblLanguageDllVersionValue.Text = verLanguage.ToString();

            var assLogger = typeof(Logging.Logger).Assembly;
            var assLoggerName = assLogger.GetName();
            var verLogger = assLoggerName.Version;

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
