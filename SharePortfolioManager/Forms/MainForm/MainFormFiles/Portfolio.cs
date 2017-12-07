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
                // Set InitFlag to "true" for loading other portfolio
                InitFlag = true;

                // Reset market value share object
                ShareObjectMarketValue = null;

                // Reset the datagridview market value
                dgvPortfolioMarketValue.Rows.Clear();
                dgvPortfolioMarketValue.Refresh();
                dgvPortfolioMarketValue.ColumnHeadersVisible = false;

                // Reset the datagridview market value footer
                dgvPortfolioFooterMarketValue.Rows.Clear();
                dgvPortfolioFooterMarketValue.Refresh();
                dgvPortfolioFooterMarketValue.ColumnHeadersVisible = false;

                // Reset final value share object
                ShareObjectFinalValue = null;

                // Reset the datagridview final value
                dgvPortfolioFinalValue.Rows.Clear();
                dgvPortfolioFinalValue.Refresh();
                dgvPortfolioFinalValue.ColumnHeadersVisible = false;

                // Reset the datagridview final value footer
                dgvPortfolioFooterFinalValue.Rows.Clear();
                dgvPortfolioFooterFinalValue.Refresh();
                dgvPortfolioFooterFinalValue.ColumnHeadersVisible = false;

                // Reset group box share details
                ResetShareDetails();

                // Load new portfolio
                LoadPortfolio();

                if (InitFlag == true)
                {
                    // Add shares to the DataGridViews
                    AddSharesToDataGridViews();
                    AddShareFooters();

                    // Select first item if an item exists and show header again
                    if (dgvPortfolioFinalValue.Rows.Count > 0)
                    {
                        dgvPortfolioFinalValue.Rows[0].Selected = true;
                        dgvPortfolioFooterFinalValue.ColumnHeadersVisible = true;
                        dgvPortfolioFooterMarketValue.ColumnHeadersVisible = true;
                    }

                    // Enable / disable controls
                    EnableDisableControlNames.Remove(@"menuStrip1");
                    EnableDisableControlNames.Remove(@"btnAdd");
                    EnableDisableControlNames.Add(@"btnRefreshAll");
                    EnableDisableControlNames.Add(@"btnRefresh");
                    EnableDisableControlNames.Add(@"btnEdit");
                    EnableDisableControlNames.Add(@"btnDelete");
                    EnableDisableControlNames.Add(@"btnClearLogger");
                    EnableDisableControlNames.Add(@"dgvPortfolio");
                    EnableDisableControlNames.Add(@"dgvPortfolioFooter");
                    EnableDisableControlNames.Add(@"tabCtrlDetails");

                    // Set application title with portfolio file name
                    if (PortfolioFileName != "")
                        Text = Language.GetLanguageTextByXPath(@"/Application/Name", LanguageName)
                            + @" " + Helper.GetApplicationVersion() + @" - (" + PortfolioFileName + @")";

                    // Enable controls
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangingPortfolioSuccessful", LanguageName),
                        Language, LanguageName,
                        Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                    if (PortfolioEmptyFlag)
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListEmpty", LanguageName),
                            Language, LanguageName,
                            Color.OrangeRed, Logger, (int)EStateLevels.Warning, (int)EComponentLevels.Application);

                        // Update control list
                        EnableDisableControlNames.Add(@"btnRefreshAll");
                        EnableDisableControlNames.Add(@"btnRefresh");
                        EnableDisableControlNames.Add(@"btnEdit");
                        EnableDisableControlNames.Add(@"btnDelete");
                        EnableDisableControlNames.Add(@"btnClearLogger");
                        EnableDisableControlNames.Add(@"dgvPortfolio");
                        EnableDisableControlNames.Add(@"dgvPortfolioFooter");
                        EnableDisableControlNames.Add(@"tabCtrlDetails");

                        // Disable controls
                        Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                        // Update control list
                        EnableDisableControlNames.Clear();
                        EnableDisableControlNames.Add(@"btnAdd");

                        // Disable controls
                        Helper.EnableDisableControls(true, this, EnableDisableControlNames);
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
                if (ReaderPortfolio != null)
                    ReaderPortfolio.Close();

                // Update control list
                EnableDisableControlNames.Add(@"btnRefreshAll");
                EnableDisableControlNames.Add(@"btnRefresh");
                EnableDisableControlNames.Add(@"btnAdd");
                EnableDisableControlNames.Add(@"btnEdit");
                EnableDisableControlNames.Add(@"btnDelete");
                EnableDisableControlNames.Add(@"btnClearLogger");
                EnableDisableControlNames.Add(@"dgvPortfolio");
                EnableDisableControlNames.Add(@"dgvPortfolioFooter");
                EnableDisableControlNames.Add(@"tabCtrlDetails");

                // Disable controls
                Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/ChangingPortfolioFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);
            }
        }

        #endregion Change portfolio

        #region Load portfolio

        /// <summary>
        /// This function loads the portfolio from the portfolio file
        /// </summary>
        void LoadPortfolio()
        {
            if (InitFlag)
            {
                // Load portfolio file
                try
                {
                    // Reset share object list
                    ShareObjectListMarketValue.Clear();
                    ShareObjectListFinalValue.Clear();
                    ShareObjectFinalValue.PortfolioValuesReset();
                    ShareObjectMarketValue.PortfolioValuesReset();
                    PortfolioEmptyFlag = true;

                    // Set portfolio to the Settings.XML file
                    var nodePortfolioName = Settings.SelectSingleNode("/Settings/Portfolio");

                    // Check if the portfolio file exists
                    if (!File.Exists(PortfolioFileName))
                    {
                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists1", LanguageName)
                            + PortfolioFileName
                            + Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists2", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                        PortfolioFileName = "";
                        // Set portfolio name value to the XML
                        nodePortfolioName.InnerXml = PortfolioFileName;

                        return;
                    }

                    //// Create the validating reader and specify DTD validation.
                    //ReaderSettings = new XmlReaderSettings();
                    //ReaderSettings.DtdProcessing = DtdProcessing.Parse;
                    //ReaderSettings.ValidationType = ValidationType.DTD;
                    //ReaderSettings.ValidationEventHandler += eventHandler;

                    ReaderPortfolio = XmlReader.Create(PortfolioFileName, ReaderSettingsPortfolio);

                    // Pass the validating reader to the XML document.
                    // Validation fails due to an undefined attribute, but the 
                    // data is still loaded into the document.
                    Portfolio = new XmlDocument();
                    Portfolio.Load(ReaderPortfolio);

                    // Read the portfolio
                    var nodeListShares = Portfolio.SelectNodes("/Portfolio/Share");
                    if (nodeListShares != null && nodeListShares.Count > 0)
                    {
                        // Set portfolio content flag
                        PortfolioEmptyFlag = false;

                        // Flag if the portfolio load was successful
                        bool loadPortfolio = true;

                        // Clear list
                        RegexSearchFailedList.Clear();

                        // Loop through the portfolio configuration
                        foreach (XmlNode nodeElement in nodeListShares)
                        {
                            if (nodeElement != null)
                            {
                                // Add a new share to the market list
                                ShareObjectListMarketValue.Add(new ShareObjectMarketValue(
                                    _imageList,
                                    Language.GetLanguageTextByXPath(@"/PercentageUnit", LanguageName),
                                    Language.GetLanguageTextByXPath(@"/PieceUnit", LanguageName)
                                    ));

                                // Add a new share to the final list
                                ShareObjectListFinalValue.Add(new ShareObjectFinalValue(
                                    _imageList,
                                    Language.GetLanguageTextByXPath(@"/PercentageUnit", LanguageName),
                                    Language.GetLanguageTextByXPath(@"/PieceUnit", LanguageName)
                                    ));

                                // Check if the node has the right tag count and the right attributes
                                if (!nodeElement.HasChildNodes ||
                                    nodeElement.ChildNodes.Count != ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].ShareObjectTagCount ||
                                    nodeElement.Attributes == null ||
                                    nodeElement.Attributes["WKN"] == null)
                                    loadPortfolio = false;
                                else
                                {
                                    ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Wkn =
                                        nodeElement.Attributes["WKN"].InnerText;
                                    ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Wkn =
                                        nodeElement.Attributes["WKN"].InnerText;

                                    ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Name =
                                        nodeElement.Attributes["Name"].InnerText;
                                    ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Name =
                                        nodeElement.Attributes["Name"].InnerText;

                                    // Loop through the tags and set values to the ShareObject
                                    for (int i = 0; i < nodeElement.ChildNodes.Count; i++)
                                    {
                                        switch (i)
                                        {
                                            #region General

                                            case 0:
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateInternet =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateInternet =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 1:
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateDate =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateDate =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 2:
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateTime =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateTime =
                                                    Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 3:
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CurPrice =
                                                    Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CurPrice =
                                                    Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 4:
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].PrevDayPrice =
                                                    Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].PrevDayPrice =
                                                    Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                                break;
                                            case 5:
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].WebSite =
                                                    nodeElement.ChildNodes[i].InnerText;
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].WebSite =
                                                    nodeElement.ChildNodes[i].InnerText;
                                                break;
                                            case 6:
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CultureInfo =
                                                    new CultureInfo(nodeElement.ChildNodes[i].InnerXml);
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CultureInfo =
                                                    new CultureInfo(nodeElement.ChildNodes[i].InnerXml);
                                                break;
                                            case 7:
                                                    ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].ShareType =
                                                    Convert.ToInt16(nodeElement.ChildNodes[i].InnerText);
                                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].ShareType =
                                                    Convert.ToInt16(nodeElement.ChildNodes[i].InnerText);
                                                break;

                                            #endregion General

                                            #region Buys

                                            case 8:
                                                foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                                {
                                                    // Check if the node has the right count of attributes
                                                    if (nodeList != null &&
                                                        nodeList.Attributes.Count ==
                                                        ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                            .BuyAttrCount)
                                                    {
                                                        if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddBuy(
                                                            nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].BuyDateAttrName].Value,            // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].BuyVolumeAttrName].Value),     // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].BuyPriceAttrName].Value),      // Price
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].BuyReductionAttrName].Value),  // Reduction
                                                            0,                                                                                                                      // Costs
                                                            nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].BuyDocumentAttrName].Value))       // Document
                                                            loadPortfolio = false;

                                                        if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddBuy(
                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].BuyDateAttrName].Value,              // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].BuyVolumeAttrName].Value),       // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].BuyPriceAttrName].Value),        // Price
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].BuyReductionAttrName].Value),    // Reduction
                                                            0,                                                                                                                      // Costs
                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].BuyDocumentAttrName].Value))         // Document
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
                                                        ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                            .SaleAttrCount)
                                                    {
                                                        if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddSale(
                                                            nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleDateAttrName].Value,                   // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleVolumeAttrName].Value),            // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleBuyPriceAttrName].Value),          // Buy price of a share
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SalePriceAttrName].Value),             // Sale price of a share
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleTaxAtSourceAttrName].Value),       // Tax at source
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleCapitalGainsTaxAttrName].Value),   // Capital gains tax
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleSolidarityTaxAttrName].Value),     // Solidarity tax
                                                            //Convert.ToDecimal(
                                                            //    nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleCostsAttrName].Value),             // Costs
                                                            0,
                                                            nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SaleDocumentAttrName].Value))              // Document
                                                            loadPortfolio = false;

                                                        if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddSale(
                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleDateAttrName].Value,                   // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleVolumeAttrName].Value),            // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleBuyPriceAttrName].Value),          // Buy price of a share
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SalePriceAttrName].Value),             // Sale price of a share
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleTaxAtSourceAttrName].Value),       // Tax at source
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleCapitalGainsTaxAttrName].Value),   // Capital gains tax
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleSolidarityTaxAttrName].Value),     // Solidarity tax
                                                            //Convert.ToDecimal(
                                                            //    nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleCostsAttrName].Value),             // Costs
                                                            0,
                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SaleDocumentAttrName].Value))              // Document
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
                                                        ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                            .CostsAttrCount)
                                                    {
                                                        if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddCost(
                                                            Convert.ToBoolean(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsBuyPartAttrName].Value // Flag if part of a buy
                                                                ),
                                                            Convert.ToBoolean(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsSalePartAttrName].Value // Flag if part of a sale
                                                                ),
                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDateAttrName].Value,       // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsValueAttrName].Value   // Value
                                                                ),                                                                                                  
                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDocumentAttrName].Value))  // Document                                                               // Document
                                                            loadPortfolio = false;

                                                        // Check if the costs is part of a buy
                                                        if (loadPortfolio &&
                                                            Convert.ToBoolean(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsBuyPartAttrName].Value // Flag if part of a buy
                                                                )
                                                           )
                                                        {
                                                            BuyObject tempBuyObject = ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AllBuyEntries.GetBuyObjectByDateTime(nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDateAttrName].Value);

                                                            // Check if a buy object has been found
                                                            if (tempBuyObject != null)
                                                            {
                                                                if (ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].RemoveBuy(nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDateAttrName].Value))
                                                                {
                                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddBuy(
                                                                        nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDateAttrName].Value,
                                                                        tempBuyObject.Volume,
                                                                        tempBuyObject.SharePrice,
                                                                        tempBuyObject.Reduction,
                                                                        Convert.ToDecimal(
                                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsValueAttrName].Value   // Value
                                                                        ),
                                                                        tempBuyObject.Document))
                                                                        loadPortfolio = false;
                                                                }
                                                                else
                                                                    loadPortfolio = false;

                                                                if (ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].RemoveBuy(nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CostsDateAttrName].Value))
                                                                {
                                                                    if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddBuy(
                                                                        nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CostsDateAttrName].Value,
                                                                        tempBuyObject.Volume,
                                                                        tempBuyObject.SharePrice,
                                                                        tempBuyObject.Reduction,
                                                                        Convert.ToDecimal(
                                                                            nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CostsValueAttrName].Value   // Value
                                                                        ),
                                                                        tempBuyObject.Document))
                                                                        loadPortfolio = false;
                                                                }
                                                                else
                                                                    loadPortfolio = false;
                                                            }
                                                        }

                                                        // Check if the costs is part of a sale
                                                        if (loadPortfolio &&
                                                            Convert.ToBoolean(
                                                                nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsSalePartAttrName].Value // Flag if part of a sale
                                                                )
                                                           )
                                                        {
                                                            SaleObject tempSaleObject = ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AllSaleEntries.GetSaleObjectByDateTime(nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDateAttrName].Value);

                                                            // Check if a sale object has been found
                                                            if (tempSaleObject != null)
                                                            {
                                                                if (ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].RemoveSale(nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDateAttrName].Value))
                                                                {
                                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddSale(
                                                                        nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsDateAttrName].Value,
                                                                        tempSaleObject.Volume,
                                                                        tempSaleObject.BuyPrice,
                                                                        tempSaleObject.SalePrice,
                                                                        tempSaleObject.TaxAtSource,
                                                                        tempSaleObject.CapitalGainsTax,
                                                                        tempSaleObject.SolidarityTax,
                                                                        Convert.ToDecimal(
                                                                            nodeList.Attributes[ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CostsValueAttrName].Value   // Value
                                                                        ),
                                                                        tempSaleObject.Document))
                                                                        loadPortfolio = false;
                                                                }
                                                                else
                                                                    loadPortfolio = false;

                                                                if (ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].RemoveSale(nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CostsDateAttrName].Value))
                                                                {
                                                                    if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddSale(
                                                                        nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CostsDateAttrName].Value,
                                                                        tempSaleObject.Volume,
                                                                        tempSaleObject.BuyPrice,
                                                                        tempSaleObject.SalePrice,
                                                                        tempSaleObject.TaxAtSource,
                                                                        tempSaleObject.CapitalGainsTax,
                                                                        tempSaleObject.SolidarityTax,
                                                                        Convert.ToDecimal(
                                                                            nodeList.Attributes[ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CostsValueAttrName].Value   // Value
                                                                        ),
                                                                        tempSaleObject.Document))
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
                                                    ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                        .DividendPayoutInterval =
                                                        Convert.ToInt16(
                                                            nodeElement.ChildNodes[i].Attributes[
                                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].DividendPayoutInterval].InnerText);
                                                }
                                                else
                                                    loadPortfolio = false;

                                                foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                                {
                                                    // Check if the dividend values are correct
                                                    // and if the foreign currency value are correct
                                                    if (nodeList != null &&
                                                        nodeList.ChildNodes.Count ==
                                                        ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].DividendChildNodeCount &&
                                                        nodeList.ChildNodes[0].Attributes != null &&
                                                        nodeList.ChildNodes[0].Attributes.Count ==
                                                        ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].DividendAttrCountForeignCu)
                                                    {
                                                        // Convert string of the check state to the windows forms check state
                                                        CheckState enableFC = CheckState.Unchecked;
                                                        if (nodeList.ChildNodes[0].Attributes[0].Value == @"Checked")
                                                            enableFC = CheckState.Checked;

                                                        if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                            .AddDividend(
                                                                Helper.GetCultureByName(
                                                                    nodeList.ChildNodes[0].Attributes[2].Value),        //CultureInfo FC
                                                                enableFC,                                               // FC enabled
                                                                Convert.ToDecimal(
                                                                nodeList.ChildNodes[0].Attributes[1].Value),            // Exchange ratio
                                                                nodeList.Attributes[0].Value,                           // Date
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[1].Value),                      // Rate
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[2].Value),                      // Volume     
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[3].Value),                      // Tax at source
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[4].Value),                      // Capital gains tax
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[5].Value),                      // Solidarity tax
                                                                Convert.ToDecimal(
                                                                    nodeList.Attributes[6].Value),                      // Price
                                                                nodeList.Attributes[7].Value))                          // Document
                                                            loadPortfolio = false;                                                    
                                                    }
                                                    else
                                                        loadPortfolio = false;
                                                }
                                                break;

                                            #endregion Dividends

                                            default:
                                                break;
                                        }
                                    }
                                
                                    // Set website configuration and encoding to the share object.
                                    // The encoding is necessary for the WebParser for encoding the download result.
                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].SetWebSiteRegexListAndEncoding(WebSiteRegexList))
                                        RegexSearchFailedList.Add(ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Wkn);
                                    if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].SetWebSiteRegexListAndEncoding(WebSiteRegexList))
                                        RegexSearchFailedList.Add(ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Wkn);
                                }
                            }
                            else
                                loadPortfolio = false;

                            // Check if the read to the share was not successful 
                            if (loadPortfolio == false)
                            {
                                // Close portfolio reader
                                if (ReaderPortfolio != null)
                                    ReaderPortfolio.Close();

                                // Remove added share object
                                ShareObjectListMarketValue.RemoveAt(ShareObjectListMarketValue.Count - 1);
                                ShareObjectListFinalValue.RemoveAt(ShareObjectListFinalValue.Count - 1);

                                // Set initialization flag
                                InitFlag = false;

                                // Add status message
                                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", LanguageName)
                                    + PortfolioFileName
                                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", LanguageName)
                                    + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListLoadFailed", LanguageName),
                                    Language, LanguageName,
                                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                                // Stop loading more portfolio configurations
                                break;
                            }
                        }

                        // Load was successful
                        // TODO This shares must be disable in the DataGridView!!!
                        if (InitFlag)
                        {
                            // Show the invalid website configurations
                            ShowInvalidWebSiteConfigurations(RegexSearchFailedList);
                        }
                    }

                    // Enable "Add" button, "ClearLogger" button
                    EnableDisableControlNames.Clear();
                    EnableDisableControlNames.Add("btnClearLogger");
                    EnableDisableControlNames.Add("btnAdd");
 
                    // Enable controls seen above
                    Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                    // Set calculated log components value to the XML
                    nodePortfolioName.InnerXml = PortfolioFileName;

                    // Sort portfolio list in order of the share names
                    ShareObjectListMarketValue.Sort(new ShareObjectListComparer());
                    ShareObjectListFinalValue.Sort(new ShareObjectListComparer());
                }
                catch (XmlException ex)
                {
#if DEBUG
                    MessageBox.Show("LoadPortfolio()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Update control list
                    EnableDisableControlNames.Remove("menuStrip1");
                    EnableDisableControlNames.Add("btnRefreshAll");
                    EnableDisableControlNames.Add("btnRefresh");
                    EnableDisableControlNames.Add("btnAdd");

                    // Disable controls
                    Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                    // Set initialization flag
                    InitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", LanguageName)
                        + PortfolioFileName
                        + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", LanguageName)
                        + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure1", LanguageName)
                        + PortfolioFileName
                        + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure2", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    ShareObjectListMarketValue.Clear();
                    ShareObjectListFinalValue.Clear();

                    // Close portfolio reader
                    if (ReaderPortfolio != null)
                        ReaderPortfolio.Close();
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show("LoadWebSiteConfigurations()\n\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
#endif
                    // Close portfolio reader
                    if (ReaderPortfolio != null)
                        ReaderPortfolio.Close();

                    // Update control list
                    EnableDisableControlNames.Add("btnRefreshAll");
                    EnableDisableControlNames.Add("btnRefresh");

                    // Disable controls
                    Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                    // Set initialization flag
                    InitFlag = false;

                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", LanguageName)
                        + PortfolioFileName
                        + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", LanguageName)
                        + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteConfigurationListLoadFailed", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    ShareObjectListMarketValue.Clear();
                    ShareObjectListFinalValue.Clear();
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
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound1_1", LanguageName)
                    + @"'"
                    + regexSearchFailed[0]
                    + @"'"
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound1_2", LanguageName),
                    Language, LanguageName,
                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
            }
            else if (regexSearchFailed.Count != 0 && regexSearchFailed.Count > 1)
            {
                // Build the status message string
                string statusMessage = Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound2_1", LanguageName);
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
                statusMessage += Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound2_2", LanguageName);

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage, statusMessage,
                    Language, LanguageName,
                    Color.Red, Logger, (int)EStateLevels.Error, (int)EComponentLevels.Application);
            }
        }

        #endregion Show invalid website configuration(s)
    }
}
