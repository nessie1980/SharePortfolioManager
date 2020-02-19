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

using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Sales;
using SharePortfolioManager.Classes.ShareObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

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
            StockMarketLaunchDate,
            LastUpdateInternet,
            LastUpdateShare,
            SharePrice,
            SharePriceBefore,
            WebSite,
            Culture,
            ShareType,
            DailyValues,
            Brokerages,
            Buys,
            Sales,
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

                #region Reset MarketValue values

                // Reset market value share object
                ShareObjectMarketValue = null;

                // Reset the DataGridView market value
                dgvPortfolioMarketValue.Rows.Clear();
                dgvPortfolioMarketValue.Refresh();
                dgvPortfolioMarketValue.ColumnHeadersVisible = false;

                // Reset the DataGridView market value footer
                dgvPortfolioFooterMarketValue.Rows.Clear();
                dgvPortfolioFooterMarketValue.Refresh();
                dgvPortfolioFooterMarketValue.ColumnHeadersVisible = false;

                #endregion Reset MarketValue values

                #region Reset FinalValue values

                // Reset final value share object
                ShareObjectFinalValue = null;

                // Reset the DataGridView final value
                dgvPortfolioFinalValue.Rows.Clear();
                dgvPortfolioFinalValue.Refresh();
                dgvPortfolioFinalValue.ColumnHeadersVisible = false;

                // Reset the DataGridView final value footer
                dgvPortfolioFooterFinalValue.Rows.Clear();
                dgvPortfolioFooterFinalValue.Refresh();
                dgvPortfolioFooterFinalValue.ColumnHeadersVisible = false;

                #endregion Reset FinalValue values

                // Load new portfolio
                LoadPortfolio();

                // Check portfolio load state
                switch (PortfolioLoadState)
                {
                    case EStatePortfolioLoad.LoadSuccessful:
                        {
                            AddSharesToDataGridViews();
                            AddShareFooters();

                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            EnableDisableControlNames.Add("grpBoxSharePortfolio");
                            EnableDisableControlNames.Add("grpBoxShareDetails");
                            EnableDisableControlNames.Add("grpBoxStatusMessage");
                            EnableDisableControlNames.Add("grpBoxUpdateState");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("btnRefreshAll");
                            EnableDisableControlNames.Add("btnRefresh");
                            EnableDisableControlNames.Add("btnEdit");
                            EnableDisableControlNames.Add("btnDelete");
                            Helper.EnableDisableControls(true, tblLayPnlShareOverviews, EnableDisableControlNames);

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangingPortfolioSuccessful", LanguageName),
                                Language, LanguageName,
                                Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
                        }
                        break;
                    case EStatePortfolioLoad.PortfolioListEmpty:
                        {
                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            EnableDisableControlNames.Add("grpBoxSharePortfolio");
                            EnableDisableControlNames.Add("grpBoxShareDetails");
                            EnableDisableControlNames.Add("grpBoxStatusMessage");
                            EnableDisableControlNames.Add("grpBoxUpdateState");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            // Disable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("btnRefreshAll");
                            EnableDisableControlNames.Add("btnRefresh");
                            EnableDisableControlNames.Add("btnEdit");
                            EnableDisableControlNames.Add("btnDelete");
                            Helper.EnableDisableControls(false, tblLayPnlShareOverviews, EnableDisableControlNames);

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                                Language.GetLanguageTextByXPath(@"/MainForm/StatusMessages/ChangingPortfolioSuccessful", LanguageName),
                                Language, LanguageName,
                                Color.Black, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListEmpty",
                                LanguageName),
                            Language, LanguageName,
                            Color.OrangeRed, Logger, (int)EStateLevels.Warning,
                            (int)EComponentLevels.Application);
                        }
                        break;
                    case EStatePortfolioLoad.LoadFailed:
                        {
                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            // Disable controls
                            saveAsToolStripMenuItem.Enabled = false;

                            _portfolioFileName = @"";
                        }
                        break;
                    case EStatePortfolioLoad.FileDoesNotExit:
                        {
                            // Enable controls
                            EnableDisableControlNames.Clear();
                            EnableDisableControlNames.Add("menuStrip1");
                            Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                            // Disable controls
                            saveAsToolStripMenuItem.Enabled = false;

                            // Add status message
                            Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists_1", LanguageName)
                            + _portfolioFileName
                            + Language.GetLanguageTextByXPath(@"/MainForm/Errors/FileDoesNotExists_2", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                            _portfolioFileName = @"";
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // Set portfolio filename to the application caption
                Text = Language.GetLanguageTextByXPath(@"/Application/Name", LanguageName)
                       + @" " + Helper.GetApplicationVersion();
                if (_portfolioFileName != @"")
                    Text += @" - (" + _portfolioFileName + @")";
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Close portfolio reader
                ReaderPortfolio?.Close();

                // Enable controls
                EnableDisableControlNames.Clear();
                EnableDisableControlNames.Add("menuStrip1");
                EnableDisableControlNames.Add("grpBoxSharePortfolio");
                EnableDisableControlNames.Add("grpBoxShareDetails");
                EnableDisableControlNames.Add("grpBoxStatusMessage");
                EnableDisableControlNames.Add("grpBoxUpdateState");
                Helper.EnableDisableControls(false, this, EnableDisableControlNames);

                EnableDisableControlNames.Clear();
                EnableDisableControlNames.Add("btnRefreshAll");
                EnableDisableControlNames.Add("btnRefresh");
                EnableDisableControlNames.Add("btnEdit");
                EnableDisableControlNames.Add("btnDelete");
                Helper.EnableDisableControls(false, tblLayPnlShareOverviews, EnableDisableControlNames);

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
                    // Set portfolio name value to the XML
                    if (nodePortfolioName != null)
                        nodePortfolioName.InnerXml = _portfolioFileName;

                    // Set portfolio file does not exist state
                    PortfolioLoadState = EStatePortfolioLoad.FileDoesNotExit;

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

                // Read the portfolio and check if shares exist
                var nodeListShares = Portfolio.SelectNodes($"/{ShareObject.GeneralPortfolioAttrName}/{ShareObject.GeneralShareAttrName}");
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
                                nodeElement.ChildNodes.Count != ShareObject.ShareObjectTagCount ||
                                nodeElement.Attributes?[ShareObject.GeneralWknAttrName] == null
                                )
                                bLoadPortfolio = false;
                            else
                            {
                                // WKN of the share
                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Wkn =
                                    nodeElement.Attributes[ShareObject.GeneralWknAttrName].InnerText;
                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Wkn =
                                    nodeElement.Attributes[ShareObject.GeneralWknAttrName].InnerText;

                                // Name of the share
                                ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Name =
                                    nodeElement.Attributes[ShareObject.GeneralNameAttrName].InnerText;
                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Name =
                                    nodeElement.Attributes[ShareObject.GeneralNameAttrName].InnerText;

                                // Update flag of the share
                                if (nodeElement.Attributes[ShareObject.GeneralUpdateAttrName].InnerText == @"True")
                                {
                                    ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Update = true;
                                    ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Update = true;
                                }
                                else
                                {
                                    ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Update = false;
                                    ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Update = false;
                                }

                                // Loop through the tags and set values to the ShareObject
                                for (var i = 0; i < nodeElement.ChildNodes.Count; i++)
                                {
                                    switch (i)
                                    {
                                        #region General

                                        case (int)PortfolioParts.StockMarketLaunchDate:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].StockMarketLaunchDate =
                                                nodeElement.ChildNodes[i].InnerText;
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].StockMarketLaunchDate = 
                                                nodeElement.ChildNodes[i].InnerText;
                                            break;
                                        case (int)PortfolioParts.LastUpdateInternet:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateInternet =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateInternet =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            break;
                                        case (int)PortfolioParts.LastUpdateShare:
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].LastUpdateShare =
                                                Convert.ToDateTime(nodeElement.ChildNodes[i].InnerText);
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].LastUpdateShare =
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

                                        #region DailyValues

                                        case (int)PortfolioParts.DailyValues:
                                            // Read daily values website
                                            if (nodeElement.ChildNodes[i].Attributes != null)
                                            {
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                        .DailyValuesWebSite =
                                                        nodeElement.ChildNodes[i].Attributes[
                                                            ShareObject.DailyValuesWebSiteAttrName].InnerText;
                                            }
                                            else
                                                bLoadPortfolio = false;
                                            foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                            {
                                                // Check if the node has the right count of attributes
                                                if (nodeList != null &&
                                                    nodeList.Attributes.Count ==
                                                    ShareObject.DailyValuesAttrCount)
                                                {

                                                    // Try do cast date of the daily values
                                                    if (DateTime.TryParse(nodeList.Attributes[ShareObject.DailyValuesDateTagName].Value, out var dateTime)) // Date
                                                    {
                                                        var dailyValues = new Parser.DailyValues
                                                        {
                                                            Date = dateTime,
                                                            OpeningPrice = Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.DailyValuesOpeningPriceTagName].Value),                     // Opening price
                                                            ClosingPrice = Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.DailyValuesClosingPriceTagName].Value),                     // Closing price
                                                            Top = Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.DailyValuesTopTagName].Value),                              // Top
                                                            Bottom = Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.DailyValuesBottomTagName].Value),                           // Bottom
                                                            Volume = Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.DailyValuesVolumeTagName].Value)                            // Volume
                                                        };

                                                        // Add daily values item
                                                        ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].DailyValues.Add(dailyValues);
                                                        ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].DailyValues.Add(dailyValues);
                                                    }
                                                    else
                                                        bLoadPortfolio = false;
                                                }
                                                else
                                                    bLoadPortfolio = false;
                                            }
                                            break;

                                        #endregion DailyValues

                                        #region Brokerages

                                        case (int)PortfolioParts.Brokerages:
                                            foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                            {
                                                // Check if the node has the right count of attributes
                                                if (nodeList != null &&
                                                    nodeList.Attributes.Count ==
                                                    ShareObject.BrokerageAttrCount)
                                                {
                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddBrokerage(
                                                        nodeList.Attributes[ShareObject.BrokerageGuidAttrName].Value,               // Guid
                                                        Convert.ToBoolean(
                                                            nodeList.Attributes[ShareObject.BrokerageBuyPartAttrName].Value         // Flag if part of a buy
                                                        ),
                                                        Convert.ToBoolean(
                                                            nodeList.Attributes[ShareObject.BrokerageSalePartAttrName].Value        // Flag if part of a sale
                                                        ),
                                                        nodeList.Attributes[ShareObject.BrokerageGuidBuySaleAttrName].Value,        // Guid of the buy or sale
                                                        nodeList.Attributes[ShareObject.BrokerageDateAttrName].Value,               // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BrokerageProvisionAttrName].Value       // Provision
                                                        ),
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BrokerageBrokerFeeAttrName].Value       // BrokerFee
                                                        ),
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BrokerageTraderPlaceFeeAttrName].Value  // TraderPlaceFee
                                                        ),
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BrokerageReductionAttrName].Value       // Reduction
                                                        ),
                                                        nodeList.Attributes[ShareObject.BrokerageDocumentAttrName].Value))          // Document
                                                        bLoadPortfolio = false;
                                                }
                                                else
                                                    bLoadPortfolio = false;
                                            }
                                            break;

                                        #endregion Brokerages

                                        #region Buys

                                        case (int)PortfolioParts.Buys:
                                            foreach (XmlElement nodeList in nodeElement.ChildNodes[i].ChildNodes)
                                            {
                                               // Check if the node has the right count of attributes
                                                if (nodeList != null &&
                                                    nodeList.Attributes.Count ==
                                                    ShareObject.BuyAttrCount)
                                                {
                                                    // Get brokerage object
                                                    var brokerage = ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AllBrokerageEntries.GetBrokerageObjectByGuidDate(
                                                        nodeList.Attributes[ShareObject.BuyBrokerageGuidAttrName].Value,
                                                        nodeList.Attributes[ShareObject.BuyDateAttrName].Value
                                                    );

                                                    if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].AddBuy(
                                                        nodeList.Attributes[ShareObject.BuyGuidAttrName].Value,                 // Guid
                                                        nodeList.Attributes[ShareObject.BuyOrderNumberAttrName].Value,          // Order number
                                                        nodeList.Attributes[ShareObject.BuyDateAttrName].Value,                 // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyVolumeAttrName].Value),          // Volume
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyVolumeSoldAttrName].Value),      // Volume already sold
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyPriceAttrName].Value),           // Price
                                                        brokerage,                                                              // Brokerage object
                                                        nodeList.Attributes[ShareObject.BuyDocumentAttrName].Value))            // Document
                                                        bLoadPortfolio = false;

                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].AddBuy(
                                                        nodeList.Attributes[ShareObject.BuyGuidAttrName].Value,                 // Guid
                                                        nodeList.Attributes[ShareObject.BuyOrderNumberAttrName].Value,          // Order number
                                                        nodeList.Attributes[ShareObject.BuyDateAttrName].Value,                 // Date
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyVolumeAttrName].Value),          // Volume
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyVolumeSoldAttrName].Value),      // Volume already sold
                                                        Convert.ToDecimal(
                                                            nodeList.Attributes[ShareObject.BuyPriceAttrName].Value),           // Price
                                                        brokerage,                                                              // Brokerage object
                                                        nodeList.Attributes[ShareObject.BuyDocumentAttrName].Value))            // Document
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
                                                    // Check if the sales values are correct
                                                    // and if the buy values are correct
                                                    if (nodeList != null && // Sale
                                                        nodeList.ChildNodes.Count == ShareObject.SaleUsedBuysCount &&
                                                        nodeList.ChildNodes[0].ChildNodes != null && // Used buys
                                                        nodeList.ChildNodes[0].ChildNodes.Count > 0
                                                    )
                                                    {
                                                        var usedBuyDetails =
                                                            new List<SaleBuyDetails>();

                                                        // Check if the used buy entries have the correct attributes
                                                        foreach (XmlElement buyAttributes in nodeList.ChildNodes[0]
                                                            .ChildNodes)
                                                        {
                                                            // Check if the used buy values are correct
                                                            if (buyAttributes.Attributes.Count !=
                                                                ShareObject.SaleAttrCountUsedBuys
                                                            )
                                                            {
                                                                bLoadPortfolio = false;
                                                                break;
                                                            }
                                                        }

                                                        // Only if the used buy counts of the values are ok load buys
                                                        if (bLoadPortfolio)
                                                        {
                                                            // Check if the used buy entries have the correct attributes
                                                            foreach (XmlElement buyAttributes in nodeList
                                                                .ChildNodes[0].ChildNodes)
                                                            {
                                                                if (buyAttributes.Attributes.Count ==
                                                                    ShareObject.SaleAttrCountUsedBuys
                                                                )
                                                                {
                                                                    usedBuyDetails.Add(new SaleBuyDetails(
                                                                        ShareObjectListMarketValue[
                                                                                ShareObjectListMarketValue.Count - 1]
                                                                            .CultureInfo, // CultureInfo
                                                                        buyAttributes.Attributes[
                                                                                ShareObject.SaleBuyDateAttrName]
                                                                            .Value, // Volume of the used buy
                                                                        Convert.ToDecimal(
                                                                            buyAttributes.Attributes[
                                                                                    ShareObject.SaleBuyVolumeAttrName]
                                                                                .Value), // Volume of the used buy
                                                                        Convert.ToDecimal(
                                                                            buyAttributes.Attributes[
                                                                                    ShareObject.SaleBuyPriceAttrName]
                                                                                .Value), // Buy price of the used buy
                                                                        Convert.ToDecimal(
                                                                            buyAttributes.Attributes[
                                                                                    ShareObject.SaleBrokerageReductionAttrName]
                                                                                .Value), // Brokerage reduction of the used buy
                                                                        buyAttributes
                                                                            .Attributes[ShareObject.SaleBuyGuidAttrName]
                                                                            .Value // Guid of the used buy
                                                                    ));
                                                                    continue;
                                                                }

                                                                bLoadPortfolio = false;
                                                                break;
                                                            }
                                                        }

                                                        if (!bLoadPortfolio) continue;

                                                        // Get brokerage object
                                                        var brokerage =
                                                            ShareObjectListFinalValue[
                                                                    ShareObjectListFinalValue.Count - 1]
                                                                .AllBrokerageEntries
                                                                .GetBrokerageObjectByGuidDate(
                                                                    nodeList.Attributes[
                                                                        ShareObject.SaleBrokerageGuidAttrName].Value,
                                                                    nodeList.Attributes[ShareObject.SaleDateAttrName]
                                                                        .Value
                                                                );

                                                        if (!ShareObjectListMarketValue[
                                                            ShareObjectListMarketValue.Count - 1].AddSale(
                                                            nodeList.Attributes[ShareObject.SaleGuidAttrName]
                                                                .Value, // Guid
                                                            nodeList.Attributes[ShareObject.SaleDateAttrName]
                                                                .Value, // Date
                                                            nodeList
                                                                .Attributes[
                                                                    ShareObject.SaleOrderNumberAttrName] // Order number
                                                                .Value, // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.SaleVolumeAttrName]
                                                                    .Value), // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleSalePriceAttrName]
                                                                    .Value), // Sale price of a share
                                                            usedBuyDetails, // Buy which are used for this sale
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleTaxAtSourceAttrName]
                                                                    .Value), // Tax at source
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleCapitalGainsTaxAttrName]
                                                                    .Value), // Capital gains tax
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleSolidarityTaxAttrName]
                                                                    .Value), // Solidarity tax
                                                            nodeList.Attributes[ShareObject.SaleDocumentAttrName]
                                                                .Value)) // Document
                                                            bLoadPortfolio = false;

                                                        if (!ShareObjectListFinalValue[
                                                            ShareObjectListFinalValue.Count - 1].AddSale(
                                                            nodeList.Attributes[ShareObject.SaleGuidAttrName]
                                                                .Value, // Guid
                                                            nodeList.Attributes[ShareObject.SaleDateAttrName]
                                                                .Value, // Date
                                                            nodeList.Attributes[ShareObject.SaleOrderNumberAttrName]
                                                                .Value, // Order number
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.SaleVolumeAttrName]
                                                                    .Value), // Volume
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleSalePriceAttrName]
                                                                    .Value), // Sale price of a share
                                                            usedBuyDetails, // Buy which are used for this sale
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleTaxAtSourceAttrName]
                                                                    .Value), // Tax at source
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleCapitalGainsTaxAttrName]
                                                                    .Value), // Capital gains tax
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[
                                                                        ShareObject.SaleSolidarityTaxAttrName]
                                                                    .Value), // Solidarity tax
                                                            brokerage, // Brokerage
                                                            nodeList.Attributes[ShareObject.SaleDocumentAttrName]
                                                                .Value)) // Document
                                                            bLoadPortfolio = false;
                                                    }
                                                }
                                                else
                                                    bLoadPortfolio = false;
                                            }
                                            break;

                                        #endregion Sales

                                        #region Dividends

                                        case (int)PortfolioParts.Dividends:
                                            // Read dividend payout interval
                                            if (nodeElement.ChildNodes[i].Attributes != null)
                                            {
                                                ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                        .DividendPayoutInterval =
                                                    Convert.ToInt16(
                                                        nodeElement.ChildNodes[i].Attributes[
                                                            ShareObject.DividendPayoutIntervalAttrName].InnerText);
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
                                                    ShareObject.DividendAttrCountForeignCu &&
                                                    nodeList.Attributes != null &&
                                                    nodeList.Attributes.Count ==
                                                    ShareObject.DividendAttrCount)
                                                {
                                                    // Convert string of the check state to the windows forms check state
                                                    var enableFc = CheckState.Unchecked;
                                                    if (nodeList.ChildNodes[0].Attributes[0].Value == @"Checked")
                                                        enableFc = CheckState.Checked;

                                                    if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                                        .AddDividend(
                                                            Helper.GetCultureByName(
                                                                nodeList.ChildNodes[0].Attributes[ShareObject.DividendNameAttrName].Value),                 //CultureInfo FC
                                                            enableFc,                                                                                       // FC enabled
                                                            Convert.ToDecimal(
                                                                nodeList.ChildNodes[0].Attributes[ShareObject.DividendExchangeRatioAttrName].Value),        // Exchange ratio
                                                            nodeList.Attributes[ShareObject.DividendGuidAttrName].Value,                                    // Guid
                                                            nodeList.Attributes[ShareObject.DividendDateAttrName].Value,                                    // Date
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.DividendRateAttrName].Value),                               // Rate
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.DividendVolumeAttrName].Value),                             // Volume     
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.DividendTaxAtSourceAttrName].Value),                        // Tax at source
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.DividendCapitalGainsTaxAttrName].Value),                    // Capital gains tax
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.DividendSolidarityTaxAttrName].Value),                      // Solidarity tax
                                                            Convert.ToDecimal(
                                                                nodeList.Attributes[ShareObject.DividendPriceAttrName].Value),                              // Price
                                                            nodeList.Attributes[ShareObject.DividendDocumentAttrName].Value))                               // Document
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
                                // The encoding is necessary for the Parser for encoding the download result.
                                if (!ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1]
                                    .SetWebSiteRegexListAndEncoding(WebSiteConfiguration.WebSiteRegexList))
                                {
                                    if (!RegexSearchFailedList.Contains(
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Wkn) &&
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Update
                                        )
                                    {
                                        RegexSearchFailedList.Add(
                                            ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].Wkn);

                                        // Set update flag to false
                                        ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].WebSiteConfigurationValid = false;
                                    }
                                }
                                else
                                    // Set update flag to false
                                    ShareObjectListFinalValue[ShareObjectListFinalValue.Count - 1].WebSiteConfigurationValid = true;

                                if (!ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1]
                                    .SetWebSiteRegexListAndEncoding(WebSiteConfiguration.WebSiteRegexList))
                                {
                                    if (!RegexSearchFailedList.Contains(
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Wkn) &&
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Update
                                        )
                                    {
                                        RegexSearchFailedList.Add(
                                            ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].Wkn);

                                        // Set update flag to false
                                        ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].WebSiteConfigurationValid = false;
                                    }
                                }
                                else
                                    // Set update flag to false
                                    ShareObjectListMarketValue[ShareObjectListMarketValue.Count - 1].WebSiteConfigurationValid = true;
                            }
                        }
                        else
                            bLoadPortfolio = false;

                        // Check if the read of the share was successful so read next share
                        if (bLoadPortfolio) continue;

                        // Close portfolio reader
                        ReaderPortfolio?.Close();

                        // Remove added share object because the read was not successful
                        ShareObjectListMarketValue.RemoveAt(ShareObjectListMarketValue.Count - 1);
                        ShareObjectListFinalValue.RemoveAt(ShareObjectListFinalValue.Count - 1);

                        // Set initialization flag
                        InitFlag = false;
                        PortfolioLoadState = EStatePortfolioLoad.LoadFailed;

                        // Add status message
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile_1", LanguageName)
                            + _portfolioFileName
                            + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile_2", LanguageName)
                            + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/PortfolioConfigurationListLoadFailed", LanguageName),
                            Language, LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                        // Stop loading more portfolio configurations
                        break;
                    }

                    // Load was successful
                    if (InitFlag)
                    {
                        PortfolioLoadState = EStatePortfolioLoad.LoadSuccessful;

                        // Show the invalid website configurations
                        ShowInvalidWebSiteConfigurations(RegexSearchFailedList);
                    }
                }
                else
                    // Set empty portfolio list state
                    PortfolioLoadState = EStatePortfolioLoad.PortfolioListEmpty;
 
                // Set calculated log components value to the XML
                nodePortfolioName.InnerXml = _portfolioFileName;

                // Sort portfolio list in order of the share names
                ShareObjectListMarketValue.Sort(new ShareObjectListComparer());
                ShareObjectListFinalValue.Sort(new ShareObjectListComparer());

                // Check if any share set for updating so enable the refresh all button
                btnRefreshAll.Enabled = ShareObjectListFinalValue.Count(p => p.Update && p.WebSiteConfigurationValid) >= 1;
            }
            catch (XmlException ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif

                // Set initialization flag
                InitFlag = false;
                PortfolioLoadState = EStatePortfolioLoad.LoadFailed;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile_1", LanguageName)
                    + _portfolioFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile_2", LanguageName)
                    + " " + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure_1", LanguageName)
                    + _portfolioFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/XMLSyntaxFailure_2", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                ShareObjectListMarketValue.Clear();
                ShareObjectListFinalValue.Clear();

                // Close portfolio reader
                ReaderPortfolio?.Close();
            }
            catch (Exception ex)
            {
#if DEBUG
                var message = Helper.GetMyMethodName() + Environment.NewLine + Environment.NewLine + ex.Message;
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#endif
                // Close portfolio reader
                ReaderPortfolio?.Close();

                // Set initialization flag
                InitFlag = false;
                PortfolioLoadState = EStatePortfolioLoad.LoadFailed;

                // Add status message
                Helper.AddStatusMessage(rchTxtBoxStateMessage,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile_1", LanguageName)
                    + _portfolioFileName
                    + Language.GetLanguageTextByXPath(@"/MainForm/Errors/CouldNotLoadFile_2", LanguageName),
                    Language, LanguageName,
                    Color.DarkRed, Logger, (int)EStateLevels.FatalError, (int)EComponentLevels.Application);

                ShareObjectListMarketValue.Clear();
                ShareObjectListFinalValue.Clear();
            }
        }

        #endregion Load portfolio

        #region Get brokerage of a share object by date

        // TODO refactor to Guid
        private decimal GetBrokerageOfShareObjectByGuidDate(string wkn, string guid, string dateTime, bool isSalePart, bool isBuyPart)
        {
            try
            {
                // Read the portfolio
                var nodeElement = Portfolio.SelectSingleNode($"/{ShareObject.GeneralPortfolioAttrName}/{ShareObject.GeneralShareAttrName} [@{ShareObject.GeneralWknAttrName}=\"{wkn}\"]");
                if (nodeElement == null) return 0;

                foreach (XmlElement nodeList in nodeElement.ChildNodes[(int) PortfolioParts.Brokerages].ChildNodes)
                {
                    // Check if the node has the right count of attributes
                    if (nodeList == null || nodeList.Attributes.Count != ShareObject.BrokerageAttrCount) return 0;

                    // Read XML date
                    var xmlGuid = nodeList.Attributes[ShareObject.BrokerageGuidAttrName].Value;
                    var xmlDateTime = nodeList.Attributes[ShareObject.BrokerageDateAttrName].Value;
                    var xmlSalePart = Convert.ToBoolean(nodeList.Attributes[ShareObject.BrokerageSalePartAttrName].Value);
                    var xmlBuyPart = Convert.ToBoolean(nodeList.Attributes[ShareObject.BrokerageBuyPartAttrName].Value);

                    if (xmlGuid == guid &&
                        xmlDateTime == dateTime &&
                        (xmlBuyPart == isBuyPart || xmlSalePart == isSalePart)
                    )
                    {
                        var decProvision =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageProvisionAttrName].Value);
                        var decBrokerFee =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageBrokerFeeAttrName].Value);
                        var decTraderPlaceFee =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageTraderPlaceFeeAttrName].Value);
                        var decReduction =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageReductionAttrName].Value);

                        Helper.CalcBrokerageValues(decProvision, decBrokerFee, decTraderPlaceFee, decReduction,
                            out var decBrokerage, out var decBrokerageWithReduction);

                        return decBrokerage;
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }


        // TODO refactor to Guid
        private decimal GetBrokerageOfShareObjectBySaleBuyGuidDate(string wkn, string guid, string dateTime, bool isSalePart, bool isBuyPart)
        {
            try
            {
                // Read the portfolio
                var nodeElement = Portfolio.SelectSingleNode($"/{ShareObject.GeneralPortfolioAttrName}/{ShareObject.GeneralShareAttrName}[@{ShareObject.GeneralWknAttrName}=\"{wkn}\"]");
                if (nodeElement == null) return 0;

                foreach (XmlElement nodeList in nodeElement.ChildNodes[(int)PortfolioParts.Brokerages].ChildNodes)
                {
                    // Check if the node has the right count of attributes
                    if (nodeList == null || nodeList.Attributes.Count != ShareObject.BrokerageAttrCount) return 0;

                    // Read XML date
                    var xmlGuid = nodeList.Attributes[ShareObject.BrokerageGuidBuySaleAttrName].Value;
                    var xmlDateTime = nodeList.Attributes[ShareObject.BrokerageDateAttrName].Value;
                    var xmlSalePart = Convert.ToBoolean(nodeList.Attributes[ShareObject.BrokerageSalePartAttrName].Value);
                    var xmlBuyPart = Convert.ToBoolean(nodeList.Attributes[ShareObject.BrokerageBuyPartAttrName].Value);

                    if (xmlGuid == guid &&
                        xmlDateTime == dateTime &&
                        (xmlBuyPart == isBuyPart || xmlSalePart == isSalePart)
                    )
                    {
                        var decProvision =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageProvisionAttrName].Value);
                        var decBrokerFee =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageBrokerFeeAttrName].Value);
                        var decTraderPlaceFee =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageTraderPlaceFeeAttrName].Value);
                        var decReduction =
                            Convert.ToDecimal(nodeList.Attributes[ShareObject.BrokerageReductionAttrName].Value);

                        Helper.CalcBrokerageValues(decProvision, decBrokerFee, decTraderPlaceFee, decReduction,
                            out var decBrokerage, out var decBrokerageWithReduction);

                        return decBrokerage;
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        #endregion Get brokerage of a share object by date

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
