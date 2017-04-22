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
using SharePortfolioManager.Classes.Taxes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        #region Change portfolio

        /// <summary>
        /// This function does all the GUI changes
        /// which must be done while the portfolio
        /// has been changed
        /// </summary>
        void ChangePortfolio()
        {
            try
            {
                // Reset the DataGridView portfolio
                dgvPortfolio.Rows.Clear();
                dgvPortfolio.Refresh();

                // Reset the DataGridView footer
                dgvPortfolioFooter.Rows.Clear();
                dgvPortfolioFooter.Refresh();

                // Reset group box share details
                ResetShareDetails();

                // Load new portfolio
                LoadPortfolio();

                // Add shares to the DataGridViews
                AddSharesToDataGridView();
                AddShareFooter();

                // Select first item if an item exists
                if (dgvPortfolio.Rows.Count > 0)
                {
                    dgvPortfolio.Rows[0].Selected = true;
                }

                // Enable / disable controls
                _enableDisableControlNames.Remove(@"menuStrip1");
                _enableDisableControlNames.Remove(@"btnAdd");
                _enableDisableControlNames.Add(@"btnRefreshAll");
                _enableDisableControlNames.Add(@"btnRefresh");
                _enableDisableControlNames.Add(@"btnEdit");
                _enableDisableControlNames.Add(@"btnDelete");
                _enableDisableControlNames.Add(@"btnClearLogger");
                _enableDisableControlNames.Add(@"dgvPortfolio");
                _enableDisableControlNames.Add(@"dgvPortfolioFooter");
                _enableDisableControlNames.Add(@"tabCtrlDetails");

                // Set application title with portfolio file name
                if (PortfolioFileName != "")
                    Text = _xmlLanguage.GetLanguageTextByXPath(@"/Application/Name", _languageName)
                        + @" " + Helper.GetApplicationVersion().ToString() + @" (" + Path.GetFileName(PortfolioFileName) + @")";
                
                // Enable controls if the initialization was correct and a portfolio is set
                if (ShareObjectList.Count != 0 && PortfolioFileName != "")
                {
                    // Enable controls
                    Helper.EnableDisableControls(true, this, _enableDisableControlNames);

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangingPortfolioSuccessful", _languageName),
                        _xmlLanguage, _languageName,
                        Color.Black, _logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
                }
                else
                {
                    // Disable controls
                    Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/ChangingPortfolioFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
                }


                if (_bPortfolioEmpty)
                {
                    if (_bPortfolioEmpty)
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListEmpty", _languageName),
                            _xmlLanguage, _languageName,
                            Color.OrangeRed, _logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);
                    }

                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("ChangePortfolio()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Close portfolio reader
                if (_xmlReaderPortfolio != null)
                    _xmlReaderPortfolio.Close();

                // Update control list
                _enableDisableControlNames.Add(@"btnRefreshAll");
                _enableDisableControlNames.Add(@"btnRefresh");
                _enableDisableControlNames.Add(@"btnAdd");
                _enableDisableControlNames.Add(@"btnEdit");
                _enableDisableControlNames.Add(@"btnDelete");
                _enableDisableControlNames.Add(@"btnClearLogger");
                _enableDisableControlNames.Add(@"dgvPortfolio");
                _enableDisableControlNames.Add(@"dgvPortfolioFooter");
                _enableDisableControlNames.Add(@"tabCtrlDetails");

                // Disable controls
                Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/ChangingPortfolioFailed", _languageName),
                    _xmlLanguage, _languageName,
                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Change portfolio

        #region Load portfolio

        /// <summary>
        /// This function loads the portfolio from the portfolio file
        /// </summary>
        void LoadPortfolio()
        {
            if (_bInitFlag)
            {
                // Load portfolio file
                try
                {
                    // Reset share object list // TODO Reset portfolio values
                    ShareObjectList.Clear();
                    ShareObject.PortfolioValuesReset();

                    // Set portfolio to the Settings.XML file
                    var nodePortfolioName = _xmlSettings.SelectSingleNode("/Settings/Portfolio");

                    // Check if the portfolio file exists
                    if (!File.Exists(PortfolioFileName))
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists1", _languageName)
                            + PortfolioFileName
                            + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists2", _languageName),
                            _xmlLanguage, _languageName,
                            Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                        PortfolioFileName = "";
                        // Set portfolio name value to the XML
                        nodePortfolioName.InnerXml = PortfolioFileName;

                        return;
                    }

                    //// Create the validating reader and specify DTD validation.
                    //_xmlReaderSettings = new XmlReaderSettings();
                    //_xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;
                    //_xmlReaderSettings.ValidationType = ValidationType.DTD;
                    //_xmlReaderSettings.ValidationEventHandler += eventHandler;

                    _xmlReaderPortfolio = XmlReader.Create(PortfolioFileName, _xmlReaderSettingsPortfolio);

                    // Pass the validating reader to the XML document.
                    // Validation fails due to an undefined attribute, but the 
                    // data is still loaded into the document.
                    _xmlPortfolio = new XmlDocument();
                    _xmlPortfolio.Load(_xmlReaderPortfolio);

                    // Read the portfolio
                    var nodeListShares = _xmlPortfolio.SelectNodes("/Portfolio/Share");
                    if (nodeListShares == null || nodeListShares.Count == 0)
                    {
                        // Set portfolio content flag
                        _bPortfolioEmpty = true;
                    }
                    else
                    {
                        // Set portfolio content flag
                        _bPortfolioEmpty = false;

                        // Flag if the portfolio load was successful
                        bool loadPortfolio = true;

                        // Clear list
                        _regexSearchFailedList.Clear();

                        // Loop through the portfolio configuration
                        foreach (XmlNode nodeElement in nodeListShares)
                        {
                            if (nodeElement != null)
                            {
                                // Add a new share to the list
                                ShareObjectList.Add(new ShareObject(
                                    _imageList,
                                    _xmlLanguage.GetLanguageTextByXPath(@"/PercentageUnit", _languageName),
                                    _xmlLanguage.GetLanguageTextByXPath(@"/PieceUnit", _languageName)
                                    ));

                                // Check if the node has the right tag count and the right attributes
                                if (!nodeElement.HasChildNodes || nodeElement.ChildNodes.Count != ShareObjectList[ShareObjectList.Count - 1].ShareObjectTagCount
                                    || nodeElement.Attributes == null || nodeElement.Attributes["WKN"] == null)
                                    loadPortfolio = false;
                                else
                                {
                                    ShareObjectList[ShareObjectList.Count - 1].Wkn =
                                        nodeElement.Attributes["WKN"].InnerText;

                                    ShareObjectList[ShareObjectList.Count - 1].Name =
                                        nodeElement.Attributes["Name"].InnerText;

                                    // Loop through the tags and set values to the ShareObject
                                    for (int i = 0; i < nodeElement.ChildNodes.Count; i++)
                                    {
                                        switch (i)
                                        {
                                            #region General

                                            case 0:
                                                ShareObjectList[ShareObjectList.Count - 1].LastUpdateInternet =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 1:
                                                ShareObjectList[ShareObjectList.Count - 1].LastUpdateDate =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 2:
                                                ShareObjectList[ShareObjectList.Count - 1].LastUpdateTime =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 3:
                                                ShareObjectList[ShareObjectList.Count - 1].CurPrice =
                                                    Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 4:
                                                ShareObjectList[ShareObjectList.Count - 1].PrevDayPrice =
                                                    Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 5:
                                                ShareObjectList[ShareObjectList.Count - 1].Deposit =
                                                    Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 6:
                                                ShareObjectList[ShareObjectList.Count - 1].WebSite =
                                                    nodeElement.ChildNodes[i].InnerText;
                                                break;
                                            case 7:
                                                ShareObjectList[ShareObjectList.Count - 1].CultureInfo =
                                                    new CultureInfo(nodeElement.ChildNodes[i].InnerXml);
                                                break;

                                            #endregion General

                                            #region Buys

                                            case 8:
                                                foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                                {
                                                    // Check if the node has the right count of attributes
                                                    if (nodeList != null &&
                                                        nodeList.Attributes.Count ==
                                                        ShareObjectList[ShareObjectList.Count - 1]
                                                            .BuyAttrCount)
                                                    {
                                                        if (!ShareObjectList[ShareObjectList.Count - 1].AddBuy(
                                                            false,
                                                            nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].BuyDateAttrName].Value,             // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].BuyVolumeAttrName].Value),      // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].BuyPriceAttrName].Value),       // Price
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].BuyReductionAttrName].Value),   // Reduction
                                                            0,                                                                                               // Costs
                                                            nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].BuyDocumentAttrName].Value))        // Document
                                                            loadPortfolio = false;
                                                    }
                                                    else
                                                        loadPortfolio = false;
                                                }
                                                break;

                                            #endregion Buys

                                            #region Sales

                                            case 9:
                                                foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                                {
                                                    // Check if the node has the right count of attributes
                                                    if (nodeList != null &&
                                                        nodeList.Attributes.Count ==
                                                        ShareObjectList[ShareObjectList.Count - 1]
                                                            .SaleAttrCount)
                                                    {
                                                        if (!ShareObjectList[ShareObjectList.Count - 1].AddSale(
                                                            false,
                                                            nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].SaleDateAttrName].Value,                         // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].SaleVolumeAttrName].Value),                  // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].SalePriceAttrName].Value),                   // Value
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].SaleProfitLossAttrName].Value),              // ProfitLoss
                                                            0,//CurrentVolume
                                                            nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].SaleDocumentAttrName].Value))                          // Document
                                                            loadPortfolio = false;
                                                    }
                                                    else
                                                        loadPortfolio = false;
                                                }
                                                break;

                                            #endregion Sales

                                            #region Costs

                                            case 10:
                                                foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                                {
                                                    // Check if the node has the right count of attributes
                                                    if (nodeList != null &&
                                                        nodeList.Attributes.Count ==
                                                        ShareObjectList[ShareObjectList.Count - 1]
                                                            .CostsAttrCount)
                                                    {
                                                        if (!ShareObjectList[ShareObjectList.Count - 1].AddCost(
                                                            Convert.ToBoolean(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsBuyPartAttrName].Value // Flag if part of a buy
                                                                ),                                                                                                  
                                                            nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsDateAttrName].Value,       // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsValueAttrName].Value   // Value
                                                                ),                                                                                                  
                                                            nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsDocumentAttrName].Value))  // Document                                                               // Document
                                                            loadPortfolio = false;

                                                        // Check if the costs is part of a buy
                                                        if (loadPortfolio &&
                                                            Convert.ToBoolean(
                                                                nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsBuyPartAttrName].Value // Flag if part of a buy
                                                                )
                                                           )
                                                        {
                                                            BuyObject tempBuyObject = ShareObjectList[ShareObjectList.Count - 1].AllBuyEntries.GetBuyObjectByDateTime(nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsDateAttrName].Value);

                                                            // Check if a buy object has been found
                                                            if (tempBuyObject != null)
                                                            {
                                                                if (ShareObjectList[ShareObjectList.Count - 1].RemoveBuy(nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsDateAttrName].Value))
                                                                {
                                                                    if (!ShareObjectList[ShareObjectList.Count - 1].AddBuy(
                                                                        true,
                                                                        nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsDateAttrName].Value,
                                                                        tempBuyObject.Volume,
                                                                        tempBuyObject.SharePrice,
                                                                        tempBuyObject.Reduction,
                                                                        Convert.ToDecimal(
                                                                            nodeList.Attributes[ShareObjectList[ShareObjectList.Count - 1].CostsValueAttrName].Value   // Value
                                                                        ),
                                                                        tempBuyObject.Document))
                                                                        loadPortfolio = false;
                                                                }
                                                                else
                                                                    loadPortfolio = false;
                                                            }
                                                        }
                                                    }
                                                    else
                                                        loadPortfolio = false;
                                                }
                                                break;

                                            #endregion Costs

                                            #region Dividends

                                            case 11:
                                                // Read dividend payout interval
                                                if (nodeElement.ChildNodes[i].Attributes != null)
                                                {
                                                    ShareObjectList[ShareObjectList.Count - 1]
                                                        .DividendPayoutInterval =
                                                        Convert.ToInt16(
                                                            nodeElement.ChildNodes[i].Attributes[
                                                                ShareObjectList[ShareObjectList.Count - 1].DividendPayoutInterval].InnerText);
                                                }
                                                else
                                                    loadPortfolio = false;

                                                foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                                {
                                                    if (nodeList != null &&
                                                        nodeList.ChildNodes.Count ==
                                                        ShareObjectList[ShareObjectList.Count - 1].DividendChildNodeCount)
                                                    {
                                                        // Create tax values object
                                                        Taxes taxValues = new Taxes();

                                                        if (nodeList.ChildNodes[0].Attributes != null &&
                                                            nodeList.ChildNodes[0].Attributes.Count ==
                                                            ShareObjectList[ShareObjectList.Count - 1].DividendAttrCountForeignCu)
                                                        {
                                                            // ForeignCurrency
                                                            taxValues.FCFlag = Convert.ToBoolean(
                                                                nodeList.ChildNodes[0].Attributes[0].Value);
                                                            // ForeignCurrencyFactor
                                                            taxValues.ExchangeRatio = Convert.ToDecimal(
                                                                nodeList.ChildNodes[0].Attributes[1].Value);
                                                            // ForeignCurrencyName
                                                            taxValues.CiShareFC =
                                                                Helper.GetCultureByName(
                                                                    nodeList.ChildNodes[0].Attributes[2].Value);
                                                            // ForeignCurrencyName
                                                            taxValues.CiShareCurrency =
                                                                ShareObjectList[ShareObjectList.Count - 1]
                                                                    .CultureInfo;
                                                        }
                                                        else
                                                            loadPortfolio = false;

                                                        // Load the tax values
                                                        if (nodeList.ChildNodes[1].ChildNodes[0] != null &&
                                                            nodeList.ChildNodes[1].ChildNodes.Count ==
                                                            ShareObjectList[ShareObjectList.Count - 1].TaxAttrCount)
                                                        {
                                                            if (nodeList.ChildNodes[1].ChildNodes[0].Attributes != null &&
                                                                nodeList.ChildNodes[1].ChildNodes[0].Attributes.Count ==
                                                                ShareObjectList[ShareObjectList.Count - 1]
                                                                    .TaxTaxAtSourceAttrCount &&
                                                                nodeList.ChildNodes[1].ChildNodes[1].Attributes != null &&
                                                                nodeList.ChildNodes[1].ChildNodes[1].Attributes.Count ==
                                                                ShareObjectList[ShareObjectList.Count - 1]
                                                                    .TaxCapitalGainsAttrCount &&
                                                                nodeList.ChildNodes[1].ChildNodes[2].Attributes != null &&
                                                                nodeList.ChildNodes[1].ChildNodes[2].Attributes.Count ==
                                                                ShareObjectList[ShareObjectList.Count - 1]
                                                                    .TaxSolidarityAttrCount
                                                                )
                                                            {
                                                                // Tax at source flag
                                                                taxValues.TaxAtSourceFlag =
                                                                    Convert.ToBoolean(
                                                                        nodeList.ChildNodes[1].ChildNodes[0].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxTaxAtSourceFlagAttrName].Value);
                                                                // Tax at source percentage
                                                                if (nodeList.ChildNodes[1].ChildNodes[0].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxTaxAtSourcePercentageAttrName]
                                                                            .Value == @"-")
                                                                    taxValues.TaxAtSourcePercentage = 0;
                                                                else
                                                                {
                                                                    taxValues.TaxAtSourcePercentage =
                                                                    Convert.ToDecimal(
                                                                        nodeList.ChildNodes[1].ChildNodes[0].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxTaxAtSourcePercentageAttrName]
                                                                            .Value);
                                                                }
                                                                // Capital gains tax flag
                                                                taxValues.CapitalGainsTaxFlag =
                                                                    Convert.ToBoolean(
                                                                        nodeList.ChildNodes[1].ChildNodes[1].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxCapitalGainsFlagAttrName].Value);
                                                                // Capital gain tax percentage
                                                                if (nodeList.ChildNodes[1].ChildNodes[1].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxCapitalGainsPercentageAttrName]
                                                                            .Value == @"-")
                                                                    taxValues.CapitalGainsTaxPercentage = 0;
                                                                else
                                                                {
                                                                    taxValues.CapitalGainsTaxPercentage =
                                                                    Convert.ToDecimal(
                                                                        nodeList.ChildNodes[1].ChildNodes[1].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxCapitalGainsPercentageAttrName]
                                                                            .Value);
                                                                }

                                                                // Solidarity tax flag
                                                                taxValues.SolidarityTaxFlag =
                                                                    Convert.ToBoolean(
                                                                        nodeList.ChildNodes[1].ChildNodes[2].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxSolidarityFlagAttrName].Value);
                                                                // Solidarity tax percentage
                                                                if (nodeList.ChildNodes[1].ChildNodes[2].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxSolidarityPercentageAttrName]
                                                                            .Value == @"-")
                                                                    taxValues.SolidarityTaxPercentage = 0;
                                                                else
                                                                {
                                                                    taxValues.SolidarityTaxPercentage =
                                                                    Convert.ToDecimal(
                                                                        nodeList.ChildNodes[1].ChildNodes[2].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxSolidarityPercentageAttrName]
                                                                            .Value);
                                                                }
                                                            }
                                                            else
                                                                loadPortfolio = false;
                                                        }
                                                        else
                                                            loadPortfolio = false;

                                                        if (!ShareObjectList[ShareObjectList.Count - 1]
                                                            .AddDividend(
                                                                nodeList.Attributes[0].Value,         // Date
                                                                taxValues,                            // Taxes
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[4].Value),    // DividendRate
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[2].Value),    // LossBalance
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[3].Value),    // Price
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[1].Value),    // Volume               
                                                                nodeList.Attributes[5].Value))        // Document
                                                            loadPortfolio = false;
                                                    }
                                                    else
                                                        loadPortfolio = false;
                                                }
                                                break;

                                            #endregion Dividends

                                            #region Taxes

                                            case 12:
                                                    if (nodeElement.ChildNodes[i].ChildNodes.Count ==
                                                        ShareObjectList[ShareObjectList.Count - 1]
                                                            .TaxAttrCount)
                                                    {
                                                        if (nodeElement.ChildNodes[i].ChildNodes[0].Attributes != null &&
                                                            nodeElement.ChildNodes[i].ChildNodes[0].Attributes.Count ==
                                                            ShareObjectList[ShareObjectList.Count - 1]
                                                                .TaxTaxAtSourceAttrCount &&
                                                            nodeElement.ChildNodes[i].ChildNodes[1].Attributes != null &&
                                                            nodeElement.ChildNodes[i].ChildNodes[1].Attributes.Count ==
                                                            ShareObjectList[ShareObjectList.Count - 1]
                                                                .TaxCapitalGainsAttrCount &&
                                                            nodeElement.ChildNodes[i].ChildNodes[2].Attributes != null &&
                                                            nodeElement.ChildNodes[i].ChildNodes[2].Attributes.Count ==
                                                            ShareObjectList[ShareObjectList.Count - 1]
                                                                .TaxSolidarityAttrCount
                                                            )
                                                        {
                                                            // Tax at source flag
                                                            ShareObjectList[ShareObjectList.Count - 1].TaxTaxAtSourceFlag =
                                                                Convert.ToBoolean(
                                                                    nodeElement.ChildNodes[i].ChildNodes[0].Attributes[
                                                                        ShareObjectList[
                                                                            ShareObjectList.Count - 1]
                                                                            .TaxTaxAtSourceFlagAttrName].Value);
                                                            // Tax at source percentage
                                                            if (nodeElement.ChildNodes[i].ChildNodes[0].Attributes[
                                                                    ShareObjectList[
                                                                        ShareObjectList.Count - 1]
                                                                        .TaxTaxAtSourcePercentageAttrName]
                                                                    .Value == @"-")
                                                                ShareObjectList[ShareObjectList.Count - 1].TaxTaxAtSourcePercentage = 0;
                                                            else
                                                            {
                                                                ShareObjectList[ShareObjectList.Count - 1].TaxTaxAtSourcePercentage =
                                                            Convert.ToDecimal(
                                                                    nodeElement.ChildNodes[i].ChildNodes[0].Attributes[
                                                                        ShareObjectList[
                                                                            ShareObjectList.Count - 1]
                                                                            .TaxTaxAtSourcePercentageAttrName]
                                                                        .Value);
                                                            }
                                                            // Capital gains tax flag
                                                            ShareObjectList[ShareObjectList.Count - 1].TaxCapitalGainsFlag =
                                                                Convert.ToBoolean(
                                                                    nodeElement.ChildNodes[i].ChildNodes[1].Attributes[
                                                                        ShareObjectList[
                                                                            ShareObjectList.Count - 1]
                                                                            .TaxCapitalGainsFlagAttrName].Value);
                                                            // Capital gain tax percentage
                                                            if (nodeElement.ChildNodes[i].ChildNodes[1].Attributes[
                                                                        ShareObjectList[
                                                                            ShareObjectList.Count - 1]
                                                                            .TaxCapitalGainsPercentageAttrName]
                                                                        .Value == @"-")
                                                                ShareObjectList[ShareObjectList.Count - 1].TaxCapitalGainsPercentage = 0;
                                                            else
                                                            {
                                                                ShareObjectList[ShareObjectList.Count - 1].TaxCapitalGainsPercentage =
                                                                    Convert.ToDecimal(
                                                                        nodeElement.ChildNodes[i].ChildNodes[1].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxCapitalGainsPercentageAttrName]
                                                                            .Value);
                                                            }
                                                            // Solidarity tax flag
                                                            ShareObjectList[ShareObjectList.Count - 1].TaxSolidarityFlag =
                                                                Convert.ToBoolean(
                                                                    nodeElement.ChildNodes[i].ChildNodes[2].Attributes[
                                                                        ShareObjectList[
                                                                            ShareObjectList.Count - 1]
                                                                            .TaxSolidarityFlagAttrName].Value);
                                                            // Solidarity tax percentage
                                                            if (nodeElement.ChildNodes[i].ChildNodes[2].Attributes[
                                                                        ShareObjectList[
                                                                            ShareObjectList.Count - 1]
                                                                            .TaxSolidarityPercentageAttrName]
                                                                        .Value == @"-")
                                                                ShareObjectList[ShareObjectList.Count - 1].TaxSolidarityPercentage = 0;
                                                            else
                                                            {
                                                                ShareObjectList[ShareObjectList.Count - 1].TaxSolidarityPercentage =
                                                                    Convert.ToDecimal(
                                                                        nodeElement.ChildNodes[i].ChildNodes[2].Attributes[
                                                                            ShareObjectList[
                                                                                ShareObjectList.Count - 1]
                                                                                .TaxSolidarityPercentageAttrName]
                                                                            .Value);
                                                            }
                                                        }
                                                        else
                                                            loadPortfolio = false;
                                                    }
                                                break;

                                            default:
                                                break;

                                            #endregion Taxes
                                        }
                                    }
                                
                                    // Set website configuration and encoding to the share object.
                                    // The encoding is necessary for the WebParser for encoding the download result.
                                    if (!ShareObjectList[ShareObjectList.Count - 1].SetWebSiteRegexListAndEncoding(_webSiteRegexList))
                                        _regexSearchFailedList.Add(ShareObjectList[ShareObjectList.Count - 1].Wkn);
                                }
                            }
                            else
                                loadPortfolio = false;

                            // Check if the read to the share was not successful 
                            if (loadPortfolio == false)
                            {
                                // Close portfolio reader
                                if (_xmlReaderPortfolio != null)
                                    _xmlReaderPortfolio.Close();

                                // Remove added share object
                                ShareObjectList.RemoveAt(ShareObjectList.Count - 1);

                                // Set initialization flag
                                _bInitFlag = false;

                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", _languageName)
                                    + PortfolioFileName
                                    + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", _languageName)
                                    + " " + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListLoadFailed", _languageName),
                                    _xmlLanguage, _languageName,
                                    Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                                // Stop loading more portfolio configurations
                                break;
                            }
                        }

                        // Load was successful
                        // TODO This shares must be disable in the DataGridView!!!
                        if (_bInitFlag)
                        {
                            // Show the invalid website configurations
                            ShowInvalidWebSiteConfigurations(_regexSearchFailedList);
                        }
                    }

                    // Enable Add button
                    _enableDisableControlNames.Clear();
                    _enableDisableControlNames.Add("menuStrip1");
                    _enableDisableControlNames.Add("btnAdd");
                    _enableDisableControlNames.Add("btnClearLogger");

                    // Enable controls seen above
                    Helper.EnableDisableControls(true, this, _enableDisableControlNames);

                    // Set calculated log components value to the XML
                    nodePortfolioName.InnerXml = PortfolioFileName;

                    // Sort portfolio list in order of the share names
                    ShareObjectList.Sort(new ShareObjectListComparer());
                }
                catch (XmlException ex)
                {
#if DEBUG
                    MessageBox.Show("LoadPortfolio()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Close portfolio reader
                    if (_xmlReaderPortfolio != null)
                        _xmlReaderPortfolio.Close();

                    // Update control list
                    _enableDisableControlNames.Add("btnRefreshAll");
                    _enableDisableControlNames.Add("btnRefresh");

                    // Disable controls
                    Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                    // Set initialization flag
                    _bInitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", _languageName)
                        + PortfolioFileName
                        + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", _languageName)
                        + " " + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure1", _languageName)
                        + PortfolioFileName
                        + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure2", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    _shareObjectsList.Clear();
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("LoadWebSiteConfigurations()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Close portfolio reader
                    if (_xmlReaderPortfolio != null)
                        _xmlReaderPortfolio.Close();

                    // Update control list
                    _enableDisableControlNames.Add("btnRefreshAll");
                    _enableDisableControlNames.Add("btnRefresh");

                    // Disable controls
                    Helper.EnableDisableControls(false, this, _enableDisableControlNames);

                    // Set initialization flag
                    _bInitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", _languageName)
                        + PortfolioFileName
                        + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", _languageName)
                        + " " + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteConfigurationListLoadFailed", _languageName),
                        _xmlLanguage, _languageName,
                        Color.DarkRed, _logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    _shareObjectsList.Clear();
                }
            }
        }

        #endregion Load portfolio

        #region Show invalid website configuration(s)

        /// <summary>
        /// This function shows the list of the mismatching website configurations of the shares
        /// </summary>
        /// <param name="regexSearchFailed">List with the mismatched website configurations</param>
        public void ShowInvalidWebSiteConfigurations(List<string> regexSearchFailed)
        {
            if (regexSearchFailed.Count != 0 && regexSearchFailed.Count == 1)
            {
                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound1_1", _languageName)
                    + @"'"
                    + regexSearchFailed[0]
                    + @"'"
                    + _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound1_2", _languageName),
                    _xmlLanguage, _languageName,
                    Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
            }
            else if (regexSearchFailed.Count != 0 && regexSearchFailed.Count > 1)
            {
                // Build the status message string
                string statusMessage = _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound2_1", _languageName);
                for (int i = 0; i < regexSearchFailed.Count; i++)
                {
                    if (i < regexSearchFailed.Count - 1)
                    {
                        statusMessage += @"'";
                        statusMessage += regexSearchFailed[i];
                        statusMessage += @"', ";
                    }
                    else
                    {
                        statusMessage += @"'";
                        statusMessage += regexSearchFailed[i];
                        statusMessage += @"' ";
                    }
                }
                statusMessage += _xmlLanguage.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound2_2", _languageName);

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage, statusMessage,
                    _xmlLanguage, _languageName,
                    Color.Red, _logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
            }
        }

        #endregion Show invalid website configuration(s)
    }
}
