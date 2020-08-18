//MIT License
//
//Copyright(c) 2020 nessie1980(nessie1980 @gmx.de)
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
//#define DEBUG_DOCUMENTS_CONFIGURATIONS

using System;
using System.Collections.Generic;
#if DEBUG_DOCUMENTS_CONFIGURATIONS
using System.Windows.Forms;
#endif
using System.Xml;
using Parser;
using SharePortfolioManager.Classes.ParserRegex;

namespace SharePortfolioManager.Classes
{
    public static class DocumentParsingConfiguration
    {
        #region Error codes

        // Error codes of the DocumentParsingConfiguration class
        public enum DocumentParsingErrorCode
        {
            ConfigurationLoadSuccessful = 0,
            ConfigurationEmpty = -1,
            ConfigurationBankAttributesError = -2,
            ConfigurationBankElementsError = -3,
            ConfigurationDocumentElementsError = -4,
            ConfigurationDocumentElementAttributeError = -5,
            ConfigurationIdentifierAttributeError = -6,
            ConfigurationSyntaxError = -7,
            ConfigurationLoadFailed = -8
        };

        #endregion Error codes

        #region Document types

        // Types of the DocumentParsingConfiguration class
        public enum DocumentTypes
        {
            BuyDocument = 0,
            SaleDocument = 1,
            DividendDocument = 2,
            BrokerageDocument = 3
        };

        #endregion Document types

        #region Properties

        /// <summary>
        /// Flag if the configuration load was successful
        /// </summary>
        public static bool InitFlag { internal set; get; }

        /// <summary>
        /// Error code of the document parsing configuration load
        /// </summary>
        public static DocumentParsingErrorCode ErrorCode { internal set; get; }

        /// <summary>
        /// Last exception of the document parsing configuration load
        /// </summary>
        public static Exception LastException { internal set; get; }

        /// <summary>
        /// XML file with the document parsing configuration
        /// </summary>
        private const string FileName = @"Settings\Documents.XML";

        public static XmlReaderSettings ReaderSettings { get; set; }

        public static XmlDocument XmlDocument { get; set; }

        public static XmlReader XmlReader { get; set; }

        /// <summary>
        /// Web site regular expressions list
        /// </summary>
        public static List<BankRegex> BankRegexList { get; set; } = new List<BankRegex>();

        #region XML attribute names

        #region Bank sections

        public const short
            BankTagCount =
                9; // BankIdentifier / BuyIdentifier / SaleIdentifier / DividendIdentifier / BrokerageIdentifier + 4 x documents (BuyIdentifier / SaleIdentifier / DividendIdentifier / BrokerageIdentifier)

        public const string BankNameAttrName = "Name";
        public const string BankIdentifierValueAttrName = "BankIdentifierValue";
        public const string BankEncodingAttrName = "Encoding";

        public const string BankIdentifierTagName = "BankIdentifier";
        public const string BuyIdentifierTagName = "BuyIdentifier";
        public const string SaleIdentifierTagName = "SaleIdentifier";
        public const string DividendIdentifierTagName = "DividendIdentifier";
        public const string BrokerageIdentifierTagName = "BrokerageIdentifier";

        #endregion Bank sections

        #region Document sections

        public const string DocumentTagName = "Document";
        public const string DocumentTypeAttrName = "Type";
        public const string DocumentIdentifierValueAttrName = "TypeIdentifierValue";
        public const string DocumentEncodingAttrName = "Encoding";

        #region Buy section values

        public const string DocumentTypeBuy = "Buy";
        public const string DocumentTypeBuyWkn = "Wkn";
        public const string DocumentTypeBuyDepotNumber = "DepotNumber";
        public const string DocumentTypeBuyOrderNumber = "OrderNumber";
        public const string DocumentTypeBuyName = "Name";
        public const string DocumentTypeBuyDate = "Date";
        public const string DocumentTypeBuyTime = "Time";
        public const string DocumentTypeBuyVolume = "Volume";
        public const string DocumentTypeBuyPrice = "Price";
        public const string DocumentTypeBuyProvision = "Provision";
        public const string DocumentTypeBuyBrokerFee = "BrokerFee";
        public const string DocumentTypeBuyTraderPlaceFee = "TraderPlaceFee";
        public const string DocumentTypeBuyReduction = "Reduction";

        #endregion Buy section values

        #region Sale section values

        public const string DocumentTypeSale = "Sale";
        public const string DocumentTypeSaleWkn = "Wkn";
        public const string DocumentTypeSaleDepotNumber = "DepotNumber";
        public const string DocumentTypeSaleOrderNumber = "OrderNumber";
        public const string DocumentTypeSaleDate = "Date";
        public const string DocumentTypeSaleTime = "Time";
        public const string DocumentTypeSaleVolume = "Volume";
        public const string DocumentTypeSalePrice = "Price";
        public const string DocumentTypeSaleTaxAtSource = "TaxAtSource";
        public const string DocumentTypeSaleCapitalGainTax = "CapitalGainTax";
        public const string DocumentTypeSaleSolidarity = "SolidarityTax";
        public const string DocumentTypeSaleProvision = "Provision";
        public const string DocumentTypeSaleBrokerFee = "BrokerFee";
        public const string DocumentTypeSaleTraderPlaceFee = "TraderPlaceFee";
        public const string DocumentTypeSaleReduction = "Reduction";

        #endregion Sale section values

        #region Dividend section values

        public const string DocumentTypeDividend = "Dividend";
        public const string DocumentTypeDividendWkn = "Wkn";
        public const string DocumentTypeDividendDate = "Date";
        public const string DocumentTypeDividendTime = "Time";
        public const string DocumentTypeDividendExchangeRate = "ExchangeRate";
        public const string DocumentTypeDividendDividendRate = "DividendRate";
        public const string DocumentTypeDividendVolume = "Volume";
        public const string DocumentTypeDividendTaxAtSource = "TaxAtSource";
        public const string DocumentTypeDividendCapitalGainTax = "CapitalGainTax";
        public const string DocumentTypeDividendSolidarityTax = "SolidarityTax";

        #endregion Dividend section values

        #region Brokerage section values

        // TODO: (thomas:2020-07-29) This implementation may not be complete and must be corrected 
        public const string DocumentTypeBrokerage = "Brokerage";
        public const string DocumentTypeBrokerageWkn = "Wkn";
        public const string DocumentTypeBrokerageDate = "Date";
        public const string DocumentTypeBrokerageTime = "Time";
        public const string DocumentTypeBrokerageProvision = "Provision";
        public const string DocumentTypeBrokerageBrokerFee = "BrokerFee";
        public const string DocumentTypeBrokerageTraderPlaceFee = "TraderPlaceFee";
        public const string DocumentTypeBrokerageReduction = "Reduction";

        #endregion Brokerage section values

        #endregion Document sections

        #region Typ sections

        public const string NameAttrName = "Name";

        public const string FoundIndexAttrName = "FoundIndex";

        public const string ResultEmptyAttrName = "ResultEmpty";

        public const string RegexOptionsAttrName = "RegexOptions";

        #endregion Typ sections

        #endregion XML attribute names

        #endregion Properties

        #region Load document configurations

        /// <summary>
        /// This function loads the document configurations from the Documents.XML
        /// This configuration is used by the Parser for parsing the given documents
        /// </summary>
        public static void LoadDocumentParsingConfigurations(bool initFlag)
        {
            InitFlag = initFlag;

            if (!InitFlag) return;

            // Load websites file
            try
            {
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

                // Read the document configurations of the various banks
                var nodeListBanks = XmlDocument.SelectNodes("/Documents/Bank");

                // Check if bank configurations exists if not cancel 
                if (nodeListBanks == null || nodeListBanks.Count == 0)
                {
                    // Set initialization flag
                    InitFlag = false;

                    // Set error code
                    ErrorCode = DocumentParsingErrorCode.ConfigurationEmpty;
                }
                else
                {
                    // Flag if the document configuration load was successful
                    var loadSettings = true;

                    // Loop through the document configurations
                    foreach (XmlNode nodeElement in nodeListBanks)
                    {
                        if (nodeElement != null)
                        {
                            // Create a regexList object for the various parsing elements of the bank
                            var bankRegexList = new RegExList();

                            // Check if all elements are available
                            if (nodeElement.Attributes?[BankNameAttrName] == null ||
                                nodeElement.Attributes?[BankIdentifierValueAttrName] == null ||
                                nodeElement.Attributes?[BankEncodingAttrName] == null
                            )
                            {
                                ErrorCode = DocumentParsingErrorCode.ConfigurationBankAttributesError;
                                loadSettings = false;
                            }
                            else
                            {
                                var bankName = nodeElement.Attributes[BankNameAttrName].Value;
                                var bankIdentifier = nodeElement.Attributes[BankIdentifierValueAttrName].Value;
                                var bankEncoding = nodeElement.Attributes[BankEncodingAttrName].Value;

                                if (!nodeElement.HasChildNodes || nodeElement.ChildNodes.Count != BankTagCount)
                                {
                                    ErrorCode = DocumentParsingErrorCode.ConfigurationBankElementsError;
                                    loadSettings = false;
                                }
                                else
                                {
                                    var dictionaryDocumentLists = new Dictionary<string, DocumentRegex>();

                                    // Check if all attributes are available
                                    for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                                    {

                                        // Check if a "Document" section should be read ( BuyIdentifier / SaleIdentifier / DividendIdentifier )
                                        // else read the identifier elements
                                        if (nodeElement.ChildNodes[i].Name == DocumentTagName &&
                                            nodeElement.ChildNodes[i].Attributes?[DocumentTypeAttrName] != null &&
                                            nodeElement.ChildNodes[i].Attributes?[DocumentIdentifierValueAttrName] !=
                                            null &&
                                            nodeElement.ChildNodes[i].Attributes?[DocumentEncodingAttrName] != null
                                        )
                                        {
                                            var documentRegex = new RegExList();

                                            var documentName = nodeElement.ChildNodes[i]
                                                .Attributes[DocumentTypeAttrName].Value;
                                            var typeIdentifier = nodeElement.ChildNodes[i]
                                                .Attributes[DocumentIdentifierValueAttrName].Value;
                                            var documentEncoding = nodeElement.ChildNodes[i]
                                                .Attributes[DocumentEncodingAttrName].Value;

                                            if (nodeElement.ChildNodes[i].HasChildNodes)
                                            {
                                                // Loop through all document elements
                                                for (var j = 0; j < nodeElement.ChildNodes[i].ChildNodes.Count; j++)
                                                {
                                                    // Check if the current element contains all necessary attributes
                                                    if (nodeElement.ChildNodes[i].ChildNodes[j].Attributes == null
                                                        || nodeElement.ChildNodes[i].ChildNodes[j]
                                                            .Attributes[NameAttrName] == null
                                                        || nodeElement.ChildNodes[i].ChildNodes[j]
                                                            .Attributes[FoundIndexAttrName] == null
                                                        || nodeElement.ChildNodes[i].ChildNodes[j]
                                                            .Attributes[ResultEmptyAttrName] == null
                                                        || nodeElement.ChildNodes[i].ChildNodes[j]
                                                            .Attributes[RegexOptionsAttrName] == null)
                                                    {
                                                        ErrorCode = DocumentParsingErrorCode
                                                            .ConfigurationDocumentElementAttributeError;
                                                        loadSettings = false;
                                                    }
                                                    else
                                                    {
                                                        var regexName = nodeElement.ChildNodes[i].ChildNodes[j]
                                                            .Attributes[NameAttrName]
                                                            .Value;
                                                        var iFoundIndex =
                                                            Convert.ToInt16(
                                                                nodeElement.ChildNodes[i].ChildNodes[j]
                                                                    .Attributes[FoundIndexAttrName]
                                                                    .Value);
                                                        var bResultEmpty =
                                                            Convert.ToBoolean(
                                                                nodeElement.ChildNodes[i].ChildNodes[j]
                                                                    .Attributes[ResultEmptyAttrName]
                                                                    .Value);
                                                        var regexOptionsList =
                                                            Helper.GetRegexOptions(
                                                                nodeElement.ChildNodes[i].ChildNodes[j]
                                                                    .Attributes[RegexOptionsAttrName]
                                                                    .Value);

                                                        // Parsing expression
                                                        var regexExpression = nodeElement.ChildNodes[i].ChildNodes[j]
                                                            .InnerText;

                                                        documentRegex.Add(
                                                            regexName,
                                                            new RegexElement(regexExpression,
                                                                iFoundIndex,
                                                                bResultEmpty,
                                                                regexOptionsList));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ErrorCode = DocumentParsingErrorCode.ConfigurationDocumentElementsError;
                                                loadSettings = false;
                                                break;
                                            }

                                            dictionaryDocumentLists.Add(
                                                nodeElement.ChildNodes[i].Attributes[DocumentTypeAttrName].Value,
                                                new DocumentRegex(documentName, typeIdentifier, documentEncoding,
                                                    documentRegex));
                                        }
                                        else
                                        {
                                            // Check if all identifier attributes are available
                                            if (nodeElement.ChildNodes[i].Attributes == null
                                                || nodeElement.ChildNodes[i].Attributes[NameAttrName] == null
                                                || nodeElement.ChildNodes[i].Attributes[FoundIndexAttrName] == null
                                                || nodeElement.ChildNodes[i].Attributes[ResultEmptyAttrName] == null
                                                || nodeElement.ChildNodes[i].Attributes[RegexOptionsAttrName] == null)
                                            {
                                                ErrorCode = DocumentParsingErrorCode
                                                    .ConfigurationIdentifierAttributeError;
                                                loadSettings = false;
                                            }
                                            else
                                            {
                                                var regexName = nodeElement.ChildNodes[i].Attributes[NameAttrName]
                                                    .Value;
                                                var iFoundIndex =
                                                    Convert.ToInt16(
                                                        nodeElement.ChildNodes[i].Attributes[FoundIndexAttrName].Value);
                                                var bResultEmpty =
                                                    Convert.ToBoolean(
                                                        nodeElement.ChildNodes[i].Attributes[ResultEmptyAttrName]
                                                            .Value);
                                                var regexOptionsList =
                                                    Helper.GetRegexOptions(
                                                        nodeElement.ChildNodes[i].Attributes[RegexOptionsAttrName]
                                                            .Value);

                                                // Parsing expression
                                                var regexExpression = nodeElement.ChildNodes[i].InnerText;

                                                bankRegexList.Add(regexName,
                                                    new RegexElement(regexExpression,
                                                        iFoundIndex,
                                                        bResultEmpty,
                                                        regexOptionsList));
                                            }
                                        }
                                    }

                                    // Add bank configuration to the global list
                                    if (loadSettings)
                                        BankRegexList.Add(new BankRegex(bankName, bankIdentifier, bankEncoding,
                                            bankRegexList,
                                            dictionaryDocumentLists));
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
                        ErrorCode = DocumentParsingErrorCode.ConfigurationLoadFailed;

                        // Stop loading more website configurations
                        break;
                    }

                    // Set error code
                    ErrorCode = DocumentParsingErrorCode.ConfigurationLoadSuccessful;
                }


            }
            catch (XmlException ex)
            {
                // Set last exception
                LastException = ex;

                Helper.ShowExceptionMessage(ex);

                // Close website reader
                XmlReader?.Close();

                // Set initialization flag
                InitFlag = false;

                // Set error code
                ErrorCode = DocumentParsingErrorCode.ConfigurationSyntaxError;
            }
            catch (Exception ex)
            {
                // Set last exception
                LastException = ex;

                Helper.ShowExceptionMessage(ex);

                // Close website reader
                XmlReader?.Close();

                // Set initialization flag
                InitFlag = false;

                // Set error code
                ErrorCode = DocumentParsingErrorCode.ConfigurationLoadFailed;
            }
        }

        #endregion Load website configurations
    }
}
