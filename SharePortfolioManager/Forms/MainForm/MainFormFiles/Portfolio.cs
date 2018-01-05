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
using SharePortfolioManager.Classes.ShareObjects;

namespace SharePortfolioManager
{
    /// <inheritdoc />
    /// <summary>
    /// The FrmMain is the start form of the application.
    /// </summary>
    public partial class FrmMain
    {
        public enum PortfolioParts
        {
            LastInternetUpdate,
            LastUpdateShareDate,
            LastUpdateTime,
            SharePrice,
            SharePriceBefore,
            WebSite,
            Culture,
            ShareType,
            Buys,
            Sales,
            Costs,
            Dividends
        };

        #region Change portfolio

        /// <summary>
        /// This function does all the GUI changes
        /// which must be done while the portfolio
        /// has been changed
        /// </summary>
        private void ChangePortfolio()
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

                // If the LoadPortfolio failed the InitFlag is "false"
                if (!InitFlag) return;

                // Add shares to the DataGridViews
                AddSharesToDataGridViews();
                AddShareFooters();

                // Select first item if an item exists
                if (dgvPortfolioFinalValue.Rows.Count > 0)
                {
                    dgvPortfolioFinalValue.Rows[0].Selected = true;
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
                if (_portfolioFileName != "")
                    Text = Language.GetLanguageTextByXPath(@"/Application/Name", LanguageName)
                           + @" " + Helper.GetApplicationVersion() + @" - (" + _portfolioFileName + @")";

                // Enable controls
                Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangingPortfolioSuccessful", LanguageName),
                    Language, LanguageName,
                    Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                // If the portfolio is empty leave function
                if (!PortfolioEmptyFlag) return;

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
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
#if DEBUG
                var message = $"ChangePortfolio()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Close portfolio reader
                ReaderPortfolio?.Close();

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
        private void LoadPortfolio()
        {
            if (!InitFlag) return;

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
                if (!File.Exists(_portfolioFileName))
                {
                    // Add status message
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists1", LanguageName)
                        + _portfolioFileName
                        + Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists2", LanguageName),
                        Language, LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                    _portfolioFileName = "";
                    // Set portfolio name value to the XML
                    if (nodePortfolioName != null)
                        nodePortfolioName.InnerXml = _portfolioFileName;

                    return;
                }

                //// Create the validating reader and specify DTD validation.
                //ReaderSettings = new XmlReaderSettings();
                //ReaderSettings.DtdProcessing = DtdProcessing.Parse;
                //ReaderSettings.ValidationType = ValidationType.DTD;
                //ReaderSettings.ValidationEventHandler += eventHandler;

                ReaderPortfolio = XmlReader.Create(_portfolioFileName, ReaderSettingsPortfolio);

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
                    var bLoadPortfolio = true;

                    // Clear list
                    RegexSearchFailedList.Clear();

                    // Loop through the portfolio configuration
                    foreach (XmlNode nodeElement in nodeListShares)
                    {
                        if (nodeElement != null)
                        {
                            // Add a new share to the market list
                            ShareObjectListMarketValue.Add(new ShareObjectMarketValue(
                                ImageList,
                                Language.GetLanguageTextByXPath(@"/PercentageUnit", LanguageName),
                                Language.GetLanguageTextByXPath(@"/PieceUnit", LanguageName)
                            ));

                            // Add a new share to the final list
                            ShareObjectListFinalValue.Add(new ShareObjectFinalValue(
                                ImageList,
                                Language.GetLanguageTextByXPath(@"/PercentageUnit", LanguageName),
                                Language.GetLanguageTextByXPath(@"/PieceUnit", LanguageName)
                            ));

                            // Check if the node has the right tag count and the right attributes
                            if (!nodeElement.HasChildNodes ||
                                nodeElement.ChildNodes.Count != ShareObject.ShareObjectTagCount || nodeElement.Attributes?["WKN"] == null)
                                bLoadPortfolio = false;
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
                                for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                                {
                                    switch (i)
                                    {
                                        #region General

                                        case (int)PortfolioParts.LastInternetUpdate:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateInternet =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateInternet =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            break;
                                        case (int)PortfolioParts.LastUpdateShareDate:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateDate =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateDate =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            break;
                                        case (int)PortfolioParts.LastUpdateTime:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateTime =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateTime =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            break;
                                        case (int)PortfolioParts.SharePrice:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CurPrice =
                                                Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CurPrice =
                                                Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                            break;
                                        case (int)PortfolioParts.SharePriceBefore:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].PrevDayPrice =
                                                Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].PrevDayPrice =
                                                Convert.ToDecimal(nodeElement.ChildNodes[i].InnerText);
                                            break;
                                        case (int)PortfolioParts.WebSite:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].WebSite =
                                                nodeElement.ChildNodes[i].InnerText;
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].WebSite =
                                                nodeElement.ChildNodes[i].InnerText;
                                            break;
                                        case (int)PortfolioParts.Culture:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].CultureInfo =
                                                new CultureInfo(nodeElement.ChildNodes[i].InnerXml);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].CultureInfo =
                                                new CultureInfo(nodeElement.ChildNodes[i].InnerXml);
                                            break;
                                        case (int)PortfolioParts.ShareType:
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].ShareType =
                                                Convert.ToInt16(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].ShareType =
                                                Convert.ToInt16(nodeElement.ChildNodes[i].InnerText);
                                            break;

                                        #endregion General

                                        #region Buys

                                        case (int)PortfolioParts.Buys:
                                            foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                            {
                                               // Check if the node has the right count of attributes
                                                if (nodeList != null &&
                                                    nodeList.Attributes.Count ==
                                                    ShareObject.BuyAttrCount)
                                                {
                                                    if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddBuy(
                                                        nodeList.Attributes[ShareObject.BuyDateAttrName].Value,             // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyVolumeAttrName].Value),      // Volume
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyPriceAttrName].Value),       // Price
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyReductionAttrName].Value),   // Reduction
                                                        GetCostOfShareObjectByDate(                                         // Costs
                                                            nodeElement.Attributes["WKN"].InnerText,
                                                            nodeList.Attributes[ShareObject.BuyDateAttrName].Value,
                                                            false,
                                                            true
                                                            ),
                                                        nodeList.Attributes[ShareObject.BuyDocumentAttrName].Value))        // Document
                                                        bLoadPortfolio = false;

                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddBuy(
                                                        nodeList.Attributes[ShareObject.BuyDateAttrName].Value,             // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyVolumeAttrName].Value),      // Volume
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyPriceAttrName].Value),       // Price
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyReductionAttrName].Value),   // Reduction
                                                        GetCostOfShareObjectByDate(                                         // Costs
                                                            nodeElement.Attributes["WKN"].InnerText,
                                                            nodeList.Attributes[ShareObject.BuyDateAttrName].Value,
                                                            false,
                                                            true
                                                        ),
                                                        nodeList.Attributes[ShareObject.BuyDocumentAttrName].Value))        // Document
                                                        bLoadPortfolio = false;
                                                }
                                                else
                                                    bLoadPortfolio = false;
                                            }
                                            break;

                                        #endregion Buys

                                        #region Sales

                                        case (int)PortfolioParts.Sales:
                                            foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                            {
                                                // Check if the node has the right count of attributes
                                                if (nodeList != null &&
                                                    nodeList.Attributes.Count ==
                                                    ShareObject.SaleAttrCount)
                                                {
                                                    if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddSale(
                                                        nodeList.Attributes[ShareObject.SaleDateAttrName].Value,                    // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleVolumeAttrName].Value),             // Volume
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleBuyPriceAttrName].Value),           // Buy price of a share
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleSalePriceAttrName].Value),          // Sale price of a share
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleTaxAtSourceAttrName].Value),        // Tax at source
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleCapitalGainsTaxAttrName].Value),    // Capital gains tax
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleSolidarityTaxAttrName].Value),      // Solidarity tax
                                                        GetCostOfShareObjectByDate(                                                 // Costs
                                                            nodeElement.Attributes["WKN"].InnerText,
                                                            nodeList.Attributes[ShareObject.BuyDateAttrName].Value,
                                                            true,
                                                            false
                                                        ),
                                                        nodeList.Attributes[ShareObject.SaleDocumentAttrName].Value))               // Document
                                                        bLoadPortfolio = false;

                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddSale(
                                                        nodeList.Attributes[ShareObject.SaleDateAttrName].Value,                    // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleVolumeAttrName].Value),             // Volume
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleBuyPriceAttrName].Value),           // Buy price of a share
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleSalePriceAttrName].Value),          // Sale price of a share
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleTaxAtSourceAttrName].Value),        // Tax at source
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleCapitalGainsTaxAttrName].Value),    // Capital gains tax
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.SaleSolidarityTaxAttrName].Value),      // Solidarity tax
                                                        GetCostOfShareObjectByDate(                                                 // Costs
                                                            nodeElement.Attributes["WKN"].InnerText,
                                                            nodeList.Attributes[ShareObject.BuyDateAttrName].Value,
                                                            true,
                                                            false
                                                        ),
                                                        nodeList.Attributes[ShareObject.SaleDocumentAttrName].Value))               // Document
                                                        bLoadPortfolio = false;
                                                }
                                                else
                                                    bLoadPortfolio = false;
                                            }
                                            break;

                                        #endregion Sales

                                        #region Costs

                                        case (int)PortfolioParts.Costs:
                                            foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                            {
                                                // Check if the node has the right count of attributes
                                                if (nodeList != null &&
                                                    nodeList.Attributes.Count ==
                                                    ShareObject.CostsAttrCount)
                                                {
                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddCost(
                                                        Convert.ToBoolean(
                                                            nodeList.Attributes[ShareObject.CostsBuyPartAttrName].Value // Flag if part of a buy
                                                        ),
                                                        Convert.ToBoolean(
                                                            nodeList.Attributes[ShareObject.CostsSalePartAttrName].Value // Flag if part of a sale
                                                        ),
                                                        nodeList.Attributes[ShareObject.CostsDateAttrName].Value,       // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.CostsValueAttrName].Value   // Value
                                                        ),                                                                                                  
                                                        nodeList.Attributes[ShareObject.CostsDocumentAttrName].Value))  // Document                                                               // Document
                                                        bLoadPortfolio = false;

                                                    //// Check if the costs is part of a buy
                                                    //if (bLoadPortfolio &&
                                                    //    Convert.ToBoolean(
                                                    //        nodeList.Attributes[ShareObject.CostsBuyPartAttrName].Value // Flag if part of a buy
                                                    //    )
                                                    //)
                                                    //{
                                                    //    var tempBuyObject = ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AllBuyEntries.GetBuyObjectByDateTime(nodeList.Attributes[ShareObject.CostsDateAttrName].Value);

                                                    //    // Check if a buy object has been found
                                                    //    if (tempBuyObject != null)
                                                    //    {
                                                    //        if (ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].RemoveBuy(nodeList.Attributes[ShareObject.CostsDateAttrName].Value))
                                                    //        {
                                                    //            if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddBuy(
                                                    //                nodeList.Attributes[ShareObject.CostsDateAttrName].Value,
                                                    //                tempBuyObject.Volume,
                                                    //                tempBuyObject.SharePrice,
                                                    //                tempBuyObject.Reduction,
                                                    //                Convert.ToDecimal(
                                                    //                    nodeList.Attributes[ShareObject.CostsValueAttrName].Value   // Value
                                                    //                ),
                                                    //                tempBuyObject.Document))
                                                    //                bLoadPortfolio = false;
                                                    //        }
                                                    //        else
                                                    //            bLoadPortfolio = false;

                                                    //        if (ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].RemoveBuy(nodeList.Attributes[ShareObject.CostsDateAttrName].Value))
                                                    //        {
                                                    //            if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddBuy(
                                                    //                nodeList.Attributes[ShareObject.CostsDateAttrName].Value,
                                                    //                tempBuyObject.Volume,
                                                    //                tempBuyObject.SharePrice,
                                                    //                tempBuyObject.Reduction,
                                                    //                Convert.ToDecimal(
                                                    //                    nodeList.Attributes[ShareObject.CostsValueAttrName].Value   // Value
                                                    //                ),
                                                    //                tempBuyObject.Document))
                                                    //                bLoadPortfolio = false;
                                                    //        }
                                                    //        else
                                                    //            bLoadPortfolio = false;
                                                    //    }
                                                    //}

                                                    //// Check if the costs is part of a sale
                                                    //if (bLoadPortfolio &&
                                                    //    Convert.ToBoolean(
                                                    //        nodeList.Attributes[ShareObject.CostsSalePartAttrName].Value // Flag if part of a sale
                                                    //    )
                                                    //)
                                                    //{
                                                    //    var tempSaleObject = ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AllSaleEntries.GetSaleObjectByDateTime(nodeList.Attributes[ShareObject.CostsDateAttrName].Value);

                                                    //    // Check if a sale object has been found
                                                    //    if (tempSaleObject != null)
                                                    //    {
                                                    //        if (ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].RemoveSale(nodeList.Attributes[ShareObject.CostsDateAttrName].Value))
                                                    //        {
                                                    //            if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddSale(
                                                    //                nodeList.Attributes[ShareObject.CostsDateAttrName].Value,
                                                    //                tempSaleObject.Volume,
                                                    //                tempSaleObject.BuyPrice,
                                                    //                tempSaleObject.SalePrice,
                                                    //                tempSaleObject.TaxAtSource,
                                                    //                tempSaleObject.CapitalGainsTax,
                                                    //                tempSaleObject.SolidarityTax,
                                                    //                Convert.ToDecimal(
                                                    //                    nodeList.Attributes[ShareObject.CostsValueAttrName].Value   // Value
                                                    //                ),
                                                    //                tempSaleObject.Document))
                                                    //                bLoadPortfolio = false;
                                                    //        }
                                                    //        else
                                                    //            bLoadPortfolio = false;

                                                    //        if (ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].RemoveSale(nodeList.Attributes[ShareObject.CostsDateAttrName].Value))
                                                    //        {
                                                    //            if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddSale(
                                                    //                nodeList.Attributes[ShareObject.CostsDateAttrName].Value,
                                                    //                tempSaleObject.Volume,
                                                    //                tempSaleObject.BuyPrice,
                                                    //                tempSaleObject.SalePrice,
                                                    //                tempSaleObject.TaxAtSource,
                                                    //                tempSaleObject.CapitalGainsTax,
                                                    //                tempSaleObject.SolidarityTax,
                                                    //                Convert.ToDecimal(
                                                    //                    nodeList.Attributes[ShareObject.CostsValueAttrName].Value   // Value
                                                    //                ),
                                                    //                tempSaleObject.Document))
                                                    //                bLoadPortfolio = false;
                                                    //        }
                                                    //        else
                                                    //            bLoadPortfolio = false;
                                                    //    }
                                                    //}
                                                }
                                                else
                                                    bLoadPortfolio = false;
                                            }
                                            break;

                                        #endregion Costs

                                        #region Dividends

                                        case (int)PortfolioParts.Dividends:
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
                                                bLoadPortfolio = false;

                                            foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                            {
                                                // Check if the dividend values are correct
                                                // and if the foreign currency value are correct
                                                if (nodeList != null &&
                                                    nodeList.ChildNodes.Count ==
                                                    ShareObject.DividendChildNodeCount &&
                                                    nodeList.ChildNodes[0].Attributes != null &&
                                                    nodeList.ChildNodes[0].Attributes.Count ==
                                                    ShareObject.DividendAttrCountForeignCu)
                                                {
                                                    // Convert string of the check state to the windows forms check state
                                                    var enableFc = CheckState.Unchecked;
                                                    if (nodeList.ChildNodes[0].Attributes[0].Value == @"Checked")
                                                        enableFc = CheckState.Checked;

                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                        .AddDividend(
                                                            Helper.GetCultureByName(
                                                                nodeList.ChildNodes[0].Attributes[2].Value),        //CultureInfo FC
                                                            enableFc,                                               // FC enabled
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
                                                        bLoadPortfolio = false;                                                    
                                                }
                                                else
                                                    bLoadPortfolio = false;
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
                            bLoadPortfolio = false;

                        // Check if the read to the share was not successful 
                        if (bLoadPortfolio == false)
                        {
                            // Close portfolio reader
                            ReaderPortfolio?.Close();

                            // Remove added share object
                            ShareObjectListMarketValue.RemoveAt(ShareObjectListMarketValue.Count - 1);
                            ShareObjectListFinalValue.RemoveAt(ShareObjectListFinalValue.Count - 1);

                            // Set initialization flag
                            InitFlag = false;

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile1", LanguageName)
                                + _portfolioFileName
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
                nodePortfolioName.InnerXml = _portfolioFileName;

                // Sort portfolio list in order of the share names
                ShareObjectListMarketValue.Sort(new ShareObjectListComparer());
                ShareObjectListFinalValue.Sort(new ShareObjectListComparer());
            }
#pragma warning disable 168
            catch (XmlException ex)
#pragma warning restore 168
            {
#if DEBUG
                var message = $"LoadPortfolio()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
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
                    + _portfolioFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", LanguageName)
                    + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure1", LanguageName)
                    + _portfolioFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure2", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                ShareObjectListMarketValue.Clear();
                ShareObjectListFinalValue.Clear();

                // Close portfolio reader
                ReaderPortfolio?.Close();
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
#if DEBUG
                var message = $"LoadWebSiteConfigurations()\n\n{ex.Message}";
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Close portfolio reader
                ReaderPortfolio?.Close();

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
                    + _portfolioFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile2", LanguageName)
                    + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteConfigurationListLoadFailed", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                ShareObjectListMarketValue.Clear();
                ShareObjectListFinalValue.Clear();
            }
        }

        #endregion Load portfolio

        #region Get costs of a share object by date

        private decimal GetCostOfShareObjectByDate(string wkn, string dateTime, bool isSalePart, bool isBuyPart)
        {
            try
            {
                // Read the portfolio
                var nodeElement = Portfolio.SelectSingleNode($"/Portfolio/Share[@WKN=\"{wkn}\"]");
                if (nodeElement == null) return 0;

                foreach (XmlElement nodeList in nodeElement.ChildNodes[(int) PortfolioParts.Costs].ChildNodes)
                {
                    // Check if the node has the right count of attributes
                    if (nodeList == null || nodeList.Attributes.Count != ShareObject.CostsAttrCount) return 0;

                    // Read XML date
                    var xmlDateTime = nodeList.Attributes[ShareObject.CostsDateAttrName].Value;
                    var xmlSalePart = Convert.ToBoolean(nodeList.Attributes[ShareObject.CostsSalePartAttrName].Value);
                    var xmlBuyPart = Convert.ToBoolean(nodeList.Attributes[ShareObject.CostsBuyPartAttrName].Value);

                    if (xmlDateTime == dateTime && (xmlBuyPart == isBuyPart || xmlSalePart == isSalePart))
                        return Convert.ToDecimal(nodeList.Attributes[ShareObject.CostsValueAttrName].Value);
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        #endregion Get costs of a share object by date

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
                    Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application);
            }
            else if (regexSearchFailed.Count != 0 && regexSearchFailed.Count > 1)
            {
                // Build the status message string
                var statusMessage =
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound2_1", LanguageName);
                for (var i = 0; i < regexSearchFailed.Count; i++)
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
                statusMessage +=
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/WebSiteRegexNotFound2_2", LanguageName);

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage, statusMessage,
                    Language, LanguageName,
                    Color.Red, Logger, (int) EStateLevels.Error, (int) EComponentLevels.Application);
            }
        }

        #endregion Show invalid website configuration(s)
    }
}
