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

using SharePortfolioManager.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using WebParser;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Load website configurations

        /// <summary>
        /// This function loads the website configurations from the WebSites.XML
        /// This configuration is used by the WebParse for parsing the given websites
        /// </summary>
        private void LoadWebSiteConfigurations()
        {
            if (!InitFlag) return;

            // Load websites file
            try
            {
                //// Create the validating reader and specify DTD validation.
                //ReaderSettings = new XmlReaderSettings();
                //ReaderSettings.DtdProcessing = DtdProcessing.Parse;
                //ReaderSettings.ValidationType = ValidationType.DTD;
                //ReaderSettings.ValidationEventHandler += eventHandler;

                ReaderWebSites = XmlReader.Create(WebSitesFileName, ReaderSettingsWebSites);

                // Pass the validating reader to the XML document.
                // Validation fails due to an undefined attribute, but the 
                // data is still loaded into the document.
                WebSites = new XmlDocument();
                WebSites.Load(ReaderWebSites);

                // Read the website configurations
                var nodeListShares = WebSites.SelectNodes("/WebSites/WebSite");

                // Check if website configurations exists if not cancel 
                if (nodeListShares == null || nodeListShares.Count == 0)
                {
                    // Set initialization flag
                    InitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteConfigurationListEmpty",
                            LanguageName),
                        Language,
                        LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError, (int) EComponentLevels.Application);
                }
                else
                {
                    // Flag if the web site configuration load was successful
                    var loadSettings = true;

                    // Loop through the website configurations
                    foreach (XmlNode nodeElement in nodeListShares)
                    {
                        if (nodeElement != null)
                        {
                            // Create a regexList object for the various parsing elements of the website
                            var regexList = new RegExList();

                            // Check if all elements are available
                            if (nodeElement.Attributes?["Id"] == null || nodeElement.Attributes["Encoding"] == null)
                                loadSettings = false;
                            else
                            {
                                var webSiteName = nodeElement.Attributes["Id"].Value;
                                var webSiteEncoding = nodeElement.Attributes["Encoding"].Value;

                                if (!nodeElement.HasChildNodes || nodeElement.ChildNodes.Count != WebSiteTagCount)
                                    loadSettings = false;
                                else
                                {
                                    // Check if all attributes are available
                                    for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                                    {
                                        if (nodeElement.ChildNodes[i].Attributes == null
                                            || nodeElement.ChildNodes[i].Attributes["Name"] == null
                                            || nodeElement.ChildNodes[i].Attributes["FoundIndex"] == null
                                            || nodeElement.ChildNodes[i].Attributes["ResultEmpty"] == null
                                            || nodeElement.ChildNodes[i].Attributes["DownloadResult"] == null
                                            || nodeElement.ChildNodes[i].Attributes["RegexOptions"] == null)
                                            loadSettings = false;
                                        else
                                        {
                                            var regexName = nodeElement.ChildNodes[i].Attributes["Name"].Value;
                                            var iFoundIndex =
                                                Convert.ToInt16(
                                                    nodeElement.ChildNodes[i].Attributes["FoundIndex"].Value);
                                            var bResultEmpty =
                                                Convert.ToBoolean(
                                                    nodeElement.ChildNodes[i].Attributes["ResultEmpty"].Value);
                                            var bDownloadResult =
                                                Convert.ToBoolean(
                                                    nodeElement.ChildNodes[i].Attributes["DownloadResult"].Value);
                                            var regexOptionsList =
                                                Helper.GetRegexOptions(
                                                    nodeElement.ChildNodes[i].Attributes["RegexOptions"].Value);

                                            // Parsing expression
                                            var regexExpression = nodeElement.ChildNodes[i].InnerText;

                                            regexList.Add(regexName,
                                                new RegexElement(regexExpression,
                                                    iFoundIndex,
                                                    bResultEmpty,
                                                    bDownloadResult,
                                                    regexOptionsList));
                                        }
                                    }

                                    // Add website configuration to the global list
                                    if (loadSettings)
                                        WebSiteRegexList.Add(new WebSiteRegex(webSiteName, webSiteEncoding,
                                            regexList));
                                }
                            }
                        }
                        else
                            loadSettings = false;

                        if (loadSettings) continue;

                        // Close website reader
                        ReaderWebSites?.Close();

                        // Set initialization flag
                        InitFlag = false;

                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1",
                                LanguageName)
                            + WebSitesFileName
                            +
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2",
                                LanguageName)
                            + " " +
                            Language.GetLanguageTextByXPath(
                                @"/MainForm/Errors/WebSiteConfigurationListLoadFailed", LanguageName),
                            Language,
                            LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                            (int) EComponentLevels.Application);

                        // Stop loading more website configurations
                        break;
                    }
                }
            }
            catch (XmlException ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Close website reader
                ReaderWebSites?.Close();

                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", LanguageName)
                    + WebSitesFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", LanguageName)
                    + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure1", LanguageName)
                    + WebSitesFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure2", LanguageName),
                    Language,
                    LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);                    
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = $"{Helper.GetMyMethodName()}\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif

                // Close website reader
                ReaderWebSites?.Close();

                // Set initialization flag
                InitFlag = false;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", LanguageName)
                    + WebSitesFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", LanguageName)
                    + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteConfigurationListLoadFailed", LanguageName),
                    Language,
                    LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Load website configurations
    }
}
