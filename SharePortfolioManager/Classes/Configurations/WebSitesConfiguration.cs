//MIT License
//
//Copyright(c) 2017 - 2021 nessie1980(nessie1980 @gmx.de)
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
//#define DEBUG_WEBSITES_CONFIGURATIONS

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Parser;
using SharePortfolioManager.Classes.ParserRegex;

namespace SharePortfolioManager.Classes.Configurations
{
    public static class WebSiteConfiguration
    {
        #region Error codes

        // Error codes of the WebSiteConfiguration class
        public enum EWebSiteErrorCode
        {
            ConfigurationLoadSuccessful = 0,
            FileDoesNotExit = -1,
            ConfigurationEmpty = -2,
            ConfigurationXmlError = -3,
            ConfigurationAttributeError = -4,
            ConfigurationLoadFailed = -5
        };

        #endregion Error codes

        #region Properties

        /// <summary>
        /// Flag if the configuration load was successful
        /// </summary>
        public static bool InitFlag { internal set; get; }

        /// <summary>
        /// Error code of the web site configuration load
        /// </summary>
        public static EWebSiteErrorCode ErrorCode { internal set; get; }

        /// <summary>
        /// Last exception of the web site configuration load
        /// </summary>
        public static Exception LastException { internal set; get; }

        /// <summary>
        /// XML file with the web site configuration
        /// </summary>
        public const string FileName = @"Settings\WebSites.xml";

        public static XmlReaderSettings ReaderSettings { get; set; }

        public static XmlDocument XmlDocument { get; set; }

        public static XmlReader XmlReader { get; set; }

        /// <summary>
        /// Web site regular expressions list
        /// </summary>
        public static List<WebSiteRegex> WebSiteRegexList { get; set; } = new List<WebSiteRegex>();

        #region XML attribute names

        private const string IdAttrName = "Id";

        private const string EncodingAttrName = "Encoding";

        private const short WebSiteTagCount = 5;

        private const string NameAttrName = "Name";

        private const string FoundIndexAttrName = "FoundIndex";

        private const string ResultEmptyAttrName = "ResultEmpty";

        private const string RegexOptionsAttrName = "RegexOptions";

        #endregion XML attribute names

        #endregion Properties

        #region Load website configurations

        /// <summary>
        /// This function loads the website configurations from the WebSites.XML
        /// This configuration is used by the Parser for parsing the given websites
        /// </summary>
        public static bool LoadWebSiteConfigurations(bool initFlag)
        {
            InitFlag = initFlag;

            if (!InitFlag) return false;

            try
            {
                // Check if the website configuration file exists
                if (!File.Exists(FileName))
                {
                    ErrorCode = EWebSiteErrorCode.FileDoesNotExit;

                    return false;
                }

                //// Create the validating reader and specify DTD validation.
                //ReaderSettings = new XmlReaderSettings();
                //ReaderSettings.DtdProcessing = DtdProcessing.Parse;
                //ReaderSettings.ValidationType = ValidationType.DTD;
                //ReaderSettings.ValidationEventHandler += eventHandler;

                XmlReader = XmlReader.Create(FileName, ReaderSettings);

                // Pass the validating reader to the XML document.
                // Validation fails due to an undefined attribute, but the 
                // data is still loaded into the document.
                XmlDocument = new XmlDocument();
                XmlDocument.Load(XmlReader);

                // Read the website configurations
                var nodeListShares = XmlDocument.SelectNodes("/WebSites/WebSite");

                // Check if website configurations exists if not cancel 
                if (nodeListShares == null || nodeListShares.Count == 0)
                {
                    // Set initialization flag
                    InitFlag = false;

                    // Set error code
                    ErrorCode = EWebSiteErrorCode.ConfigurationEmpty;
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
                            if (nodeElement.Attributes?[IdAttrName] == null ||
                                nodeElement.Attributes[EncodingAttrName] == null)
                                loadSettings = false;
                            else
                            {
                                var webSiteName = nodeElement.Attributes[IdAttrName].Value;
                                var webSiteEncoding = nodeElement.Attributes[EncodingAttrName].Value;

                                if (!nodeElement.HasChildNodes || nodeElement.ChildNodes.Count != WebSiteTagCount)
                                    loadSettings = false;
                                else
                                {
                                    // Check if all attributes are available
                                    for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                                    {
                                        if (nodeElement.ChildNodes[i].Attributes == null
                                            || nodeElement.ChildNodes[i].Attributes[NameAttrName] == null
                                            || nodeElement.ChildNodes[i].Attributes[FoundIndexAttrName] == null
                                            || nodeElement.ChildNodes[i].Attributes[ResultEmptyAttrName] == null
                                            || nodeElement.ChildNodes[i].Attributes[RegexOptionsAttrName] == null)
                                            loadSettings = false;
                                        else
                                        {
                                            var regexName = nodeElement.ChildNodes[i].Attributes[NameAttrName].Value;
                                            var iFoundIndex =
                                                Convert.ToInt16(
                                                    nodeElement.ChildNodes[i].Attributes[FoundIndexAttrName].Value);
                                            var bResultEmpty =
                                                Convert.ToBoolean(
                                                    nodeElement.ChildNodes[i].Attributes[ResultEmptyAttrName].Value);
                                            var regexOptionsList =
                                                Helper.GetRegexOptions(
                                                    nodeElement.ChildNodes[i].Attributes[RegexOptionsAttrName].Value);

                                            // Parsing expression
                                            var regexExpression = nodeElement.ChildNodes[i].InnerText;

                                            regexList.Add(regexName,
                                                new RegexElement(regexExpression,
                                                    iFoundIndex,
                                                    bResultEmpty,
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
                        XmlReader?.Close();

                        // Set initialization flag
                        InitFlag = false;

                        // Set error code
                        ErrorCode = EWebSiteErrorCode.ConfigurationAttributeError;

                        // Stop loading more website configurations
                        break;
                    }

                    if (loadSettings)
                    {
                        // Set error code
                        ErrorCode = EWebSiteErrorCode.ConfigurationLoadSuccessful;
                    }
                }

                return InitFlag;
            }
            catch (XmlException ex)
            {
                // Set last exception 
                LastException = ex;

                // Close website reader
                XmlReader?.Close();

                // Set error code
                ErrorCode = EWebSiteErrorCode.ConfigurationXmlError;

                // Set initialization flag
                InitFlag = false;
                return InitFlag;
            }
            catch (Exception ex)
            {
                // Set last exception 
                LastException = ex;

                // Close website reader
                XmlReader?.Close();

                // Set error code
                ErrorCode = EWebSiteErrorCode.ConfigurationLoadFailed;

                // Set initialization flag
                InitFlag = false;
                return InitFlag;
            }
        }

        #endregion Load website configurations
    }
}
