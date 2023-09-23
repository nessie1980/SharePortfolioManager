//MIT License
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
//#define DEBUG_SHARE_ADD_VIEW

using LanguageHandler;
using Logging;
using Parser;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.ParserRegex;
using SharePortfolioManager.Classes.ShareObjects;
using SharePortfolioManager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharePortfolioManager.Classes.Configurations;

namespace SharePortfolioManager.ShareAddForm.View
{
    /// <summary>
    /// Error codes of the ShareAdd
    /// </summary>
    public enum ShareAddErrorCode
    {
        AddSuccessful,
        AddFailed,
        WknEmpty,
        WknExists,
        IsinEmpty,
        IsinExists,
        NameEmpty,
        NameExists,
        StockMarketLaunchDateNotModified,
        DetailsWebSiteEmpty,
        DetailsWebSiteWrongFormat,
        DetailsWebSiteExists,
        MarketValuesWebSiteEmpty,
        MarketWebSiteWrongFormat,
        MarketWebSiteExists,
        DailyValuesWebSiteEmpty,
        DailyValuesWebSiteWrongFormat,
        DailyValuesWebSiteExists,
        DepotNumberEmpty,
        OrderNumberEmpty,
        OrderNumberExists,
        VolumeEmpty,
        VolumeWrongFormat,
        VolumeWrongValue,
        SharePriceEmpty,
        SharePriceWrongFormat,
        SharePriceWrongValue,
        ProvisionWrongFormat,
        ProvisionWrongValue,
        BrokerFeeWrongFormat,
        BrokerFeeWrongValue,
        TraderPlaceFeeWrongFormat,
        TraderPlaceFeeWrongValue,
        ReductionWrongFormat,
        ReductionWrongValue,
        BrokerageEmpty,
        BrokerageWrongFormat,
        BrokerageWrongValue,
        DocumentBrowseFailed,
        DocumentDirectoryDoesNotExists,
        DocumentFileDoesNotExists,
        WebSiteRegexNotFound
    };

    /// <summary>
    /// Error codes for the document parsing
    /// </summary>
    public enum ParsingErrorCode
    {
#pragma warning disable 1591
        ParsingFailed = -6,
        ParsingDocumentNotImplemented = -5,
        ParsingDocumentTypeIdentifierFailed = -4,
        ParsingBankIdentifierFailed = -3,
        ParsingDocumentFailed = -2,
        ParsingParsingDocumentError = -1,
        ParsingStarted = 0,
        ParsingIdentifierValuesFound = 1
#pragma warning restore 1591
    }

    /// <inheritdoc />
    /// <summary>
    /// Interface of the ShareAdd view
    /// </summary>
    public interface IViewShareAdd : INotifyPropertyChanged
    {
        event EventHandler ShareAddEventHandler;
        event EventHandler FormatInputValuesEventHandler;
        event EventHandler DocumentBrowseEventHandler;

        ShareAddErrorCode ErrorCode { get; set; }

        ShareObjectMarketValue ShareObjectMarketValue { get; set; }
        List<ShareObjectMarketValue> ShareObjectListMarketValue { get; set; }
        ShareObjectFinalValue ShareObjectFinalValue { get; set; }
        List<ShareObjectFinalValue> ShareObjectListFinalValue { get; set; }
        List<Image> ImageListPrevDayPerformance { get; }
        List<Image> ImageListCompletePerformance { get; }
        List<WebSiteRegex> WebSiteRegexList { get; }

        Logger Logger { get; }
        Language Language { get; }
        string LanguageName { get; }

        string Wkn { get; set; }
        string Isin { get; set; }
        string ShareName { get; set; }
        string StockMarketLaunchDate { get; set; }
        string Date { get; set; }
        string Time { get; set; }
        string DepotNumber { get; set; }
        string OrderNumber { get; set; }
        string Volume { get; set; }
        string SharePrice { get; set; }
        string BuyValue { get; set; }
        string Provision { get; set; }
        string BrokerFee { get; set; }
        string TraderPlaceFee { get; set; }
        string Reduction { get; set; }
        string Brokerage { get; set; }
        string BuyValueBrokerageReduction { get; set; }
        string DetailsWebSite { get; set; }
        string MarketValuesWebSite { get; set; }
        ShareObject.ParsingTypes MarketValuesParsingOption { get; set; }
        string DailyValuesWebSite { get; set; }
        ShareObject.ParsingTypes DailyValuesParsingOption { get; set; }
        CultureInfo CultureInfo { get; }
        int DividendPayoutInterval { get; set; }
        ShareObject.ShareTypes ShareType { get; set; }
        string Document { get; set; }

        DialogResult ShowDialog();
        void AddFinish();

        void DocumentBrowseFinish();
    }

    internal partial class ViewShareAdd : Form, IViewShareAdd
    {
        #region Parsing Fields

        /// <summary>
        /// Flag if a parsing start is allows ( document browse / drag and drop )
        /// </summary>
        private bool _parsingStartAllow;

        /// <summary>
        /// Counter for the check bank type configurations
        /// </summary>
        private int _bankCounter;

        /// <summary>
        /// Flag if the bank identifier of the given document could be found in the document configurations
        /// </summary>
        private bool _bankIdentifierFound;

        /// <summary>
        /// Flag if the buy identifier type of the given document could be found in the document configurations
        /// </summary>
        private bool _buyIdentifierFound;

        /// <summary>
        /// Flag if the document type is not implemented yet
        /// </summary>
        private bool _documentTypNotImplemented;

        /// <summary>
        /// Flag if the document values parsing is running
        /// </summary>
        private bool _documentValuesRunning;

        /// <summary>
        /// Flag if the parsing was successful
        /// </summary>
        private bool _parsingResult;

        /// <summary>
        /// Flag if the parsing with the Parser.dll is done
        /// </summary>
        private bool _parsingThreadFinished;

        /// <summary>
        /// BackGroundWorker for the document parsing
        /// </summary>
        private readonly BackgroundWorker _parsingBackgroundWorker = new BackgroundWorker();

        #endregion Parsing Fields

        #region Properties

        public FrmMain ParentWindow { get; set; }

        public Logger Logger { get; internal set; }

        public Language Language { get; internal set; }

        public string LanguageName { get; internal set; }

        public string ParsingFileName { get; set; }

        public bool StopFomClosingFlag { get; set; }

        #region Parsing

        public DocumentParsingConfiguration.DocumentTypes DocumentType { get; internal set; }

        public Parser.Parser DocumentTypeParser;

        public string ParsingText { get; internal set; }

        public Dictionary<string, List<string>> DictionaryParsingResult;

        #endregion Parsing

        #endregion Properties

        #region IViewMember

        public ShareAddErrorCode ErrorCode { get; set; }

        public ShareObjectMarketValue ShareObjectMarketValue
        {
            get => ParentWindow.ShareObjectMarketValue;
            set => ParentWindow.ShareObjectMarketValue = value;
        }

        public List<ShareObjectMarketValue> ShareObjectListMarketValue
        {
            get => ParentWindow.ShareObjectListMarketValue;
            set => ParentWindow.ShareObjectListMarketValue = value;
        }

        public ShareObjectFinalValue ShareObjectFinalValue
        {
            get => ParentWindow.ShareObjectFinalValue;
            set => ParentWindow.ShareObjectFinalValue = value;
        }

        public List<ShareObjectFinalValue> ShareObjectListFinalValue
        {
            get => ParentWindow.ShareObjectListFinalValue;
            set => ParentWindow.ShareObjectListFinalValue = value;
        }

        public List<Image> ImageListPrevDayPerformance => ParentWindow.ImageListPrevDayPerformance;

        public List<Image> ImageListCompletePerformance => ParentWindow.ImageListCompletePerformance;

        public List<WebSiteRegex> WebSiteRegexList { get; set; }

        #region Input values

        public string Wkn
        {
            get => txtBoxWkn.Text;
            set
            {
                if (txtBoxWkn.Text == value)
                    return;
                txtBoxWkn.Text = value;
            }
        }

        public string Isin
        {
            get => txtBoxIsin.Text;
            set
            {
                if (txtBoxIsin.Text == value)
                    return;
                txtBoxIsin.Text = value;
            }
        }

        public string ShareName
        {
            get => txtBoxName.Text;
            set
            {
                if (txtBoxName.Text == value)
                    return;
                txtBoxName.Text = value;
            }
        }

        public string StockMarketLaunchDate
        {
            get => dateTimePickerStockMarketLaunch.Text;
            set
            {
                if (dateTimePickerStockMarketLaunch.Text == value)
                    return;
                dateTimePickerStockMarketLaunch.Text = value;
            }
        }

        public ShareObject.ShareTypes ShareType
        {
            get => (ShareObject.ShareTypes)cbxShareType.SelectedIndex;
            set
            {
                if ((ShareObject.ShareTypes) cbxShareType.SelectedIndex == value)
                    return;
                cbxShareType.SelectedIndex = (int)value;
            }
        }

        public int DividendPayoutInterval
        {
            get => cbxDividendPayoutInterval.SelectedIndex;
            set
            {
                if (cbxDividendPayoutInterval.SelectedIndex == value)
                    return;
                cbxDividendPayoutInterval.SelectedIndex = value;
            }
        }

        public CultureInfo CultureInfo
        {
            get
            {
                var cultureName = cbxCultureInfo.SelectedItem.ToString();
                return Helper.GetCultureByName(cultureName);
            }
        }

        public string DetailsWebSite
        {
            get => txtBoxDetailsWebSite.Text;
            set
            {
                if (txtBoxDetailsWebSite.Text == value)
                    return;
                txtBoxDetailsWebSite.Text = value;
            }
        }

        public string MarketValuesWebSite
        {
            get => txtBoxMarketWebSite.Text;
            set
            {
                if (txtBoxMarketWebSite.Text == value)
                    return;
                txtBoxMarketWebSite.Text = value;
            }
        }

        public ShareObject.ParsingTypes MarketValuesParsingOption
        {
            get => (ShareObject.ParsingTypes)cbxMarketWebSiteParsingType.SelectedIndex;
            set
            {
                if ((ShareObject.ParsingTypes)cbxMarketWebSiteParsingType.SelectedIndex == value)
                    return;
                cbxMarketWebSiteParsingType.SelectedIndex = (int)value;
            }
        }

        public string DailyValuesWebSite
        {
            get => txtBoxDailyValuesWebSite.Text;
            set
            {
                if (txtBoxDailyValuesWebSite.Text == value)
                    return;
                txtBoxDailyValuesWebSite.Text = value;
            }
        }

        public ShareObject.ParsingTypes DailyValuesParsingOption
        {
            get => (ShareObject.ParsingTypes)cbxDailyValuesWebSiteParsingType.SelectedIndex;
            set
            {
                if ((ShareObject.ParsingTypes)cbxDailyValuesWebSiteParsingType.SelectedIndex == value)
                    return;
                cbxDailyValuesWebSiteParsingType.SelectedIndex = (int)value;
            }
        }

        public string Date
        {
            get => dateTimePickerDate.Text;
            set
            {
                if (dateTimePickerDate.Text == value)
                    return;
                dateTimePickerDate.Text = value;
            }
        }

        public string Time
        {
            get => dateTimePickerTime.Text;
            set
            {
                if (dateTimePickerTime.Text == value)
                    return;
                dateTimePickerTime.Text = value;
            }
        }

        public string DepotNumber
        {
            get => cbxDepotNumber.SelectedIndex > 0 ? cbxDepotNumber.SelectedItem.ToString() : @"-";
            set
            {
                var index = cbxDepotNumber.FindString(value);
                if (index > -1)
                    cbxDepotNumber.SelectedIndex = index;
            }
        }

        public string OrderNumber
        {
            get => txtBoxOrderNumber.Text;
            set
            {
                if (txtBoxOrderNumber.Text == value)
                    return;
                txtBoxOrderNumber.Text = value;
            }
        }

        public string Volume
        {
            get => txtBoxVolume.Text;
            set
            {
                if (txtBoxVolume.Text == value)
                    return;
                txtBoxVolume.Text = value;
            }
        }

        public string SharePrice
        {
            get => txtBoxSharePrice.Text;
            set
            {
                if (txtBoxSharePrice.Text == value)
                    return;
                txtBoxSharePrice.Text = value;
            }
        }

        public string Provision
        {
            get => txtBoxProvision.Text;
            set
            {
                if (txtBoxProvision.Text == value)
                    return;
                txtBoxProvision.Text = value;
            }
        }

        public string BrokerFee
        {
            get => txtBoxBrokerFee.Text;
            set
            {
                if (txtBoxBrokerFee.Text == value)
                    return;
                txtBoxBrokerFee.Text = value;
            }
        }

        public string TraderPlaceFee
        {
            get => txtBoxTraderPlaceFee.Text;
            set
            {
                if (txtBoxTraderPlaceFee.Text == value)
                    return;
                txtBoxTraderPlaceFee.Text = value;
            }
        }

        public string Reduction
        {
            get => txtBoxReduction.Text;
            set
            {
                if (txtBoxReduction.Text == value)
                    return;
                txtBoxReduction.Text = value;
            }
        }

        public string Brokerage
        {
            get => txtBoxBrokerage.Text;
            set
            {
                if (txtBoxBrokerage.Text == value)
                    return;
                txtBoxBrokerage.Text = value;
            }
        }

        public string BuyValue
        {
            get => txtBoxBuyValue.Text;
            set
            {
                if (txtBoxBuyValue.Text == value)
                    return;
                txtBoxBuyValue.Text = value;
            }
        }

        public string BuyValueBrokerageReduction
        {
            get => txtBoxBuyValueBrokerageReduction.Text;
            set
            {
                if (txtBoxBuyValueBrokerageReduction.Text == value)
                    return;
                txtBoxBuyValueBrokerageReduction.Text = value;
            }
        }

        public string Document
        {
            get => txtBoxDocument.Text;
            set
            {
                if (txtBoxDocument.Text == value)
                    return;
                txtBoxDocument.Text = value;
            }
        }

        #endregion Input values

        public void AddFinish()
        {
            // Set messages
            string strMessage;
            var clrMessage = Color.Black;
            var stateLevel = FrmMain.EStateLevels.Info;

            StopFomClosingFlag = true;

            // Enable controls
            Enabled = true;

            switch (ErrorCode)
            {
                case ShareAddErrorCode.AddSuccessful:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/StateMessages/AddSuccess", SettingsConfiguration.LanguageName);

                    StopFomClosingFlag = false;

                    // Check if a direct document parsing is done
                    if (ParsingFileName != null)
                        Close();

                    break;
                }
                case ShareAddErrorCode.AddFailed:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/AddFailed", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxVolume.Focus();
                    break;
                }
                case ShareAddErrorCode.WknEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WKNEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxWkn.Focus();
                    break;
                }
                case ShareAddErrorCode.WknExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WKNExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxWkn.Focus();
                    break;
                }
                case ShareAddErrorCode.IsinEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ISINEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxIsin.Focus();
                    break;
                }
                case ShareAddErrorCode.IsinExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ISINExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxIsin.Focus();
                    break;
                }
                case ShareAddErrorCode.NameEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/NameEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxName.Focus();
                    break;
                }
                case ShareAddErrorCode.NameExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/NameExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxName.Focus();
                    break;
                }
                case ShareAddErrorCode.StockMarketLaunchDateNotModified:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/StockMarketLaunchDateNotModified", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    dateTimePickerStockMarketLaunch.Focus();
                    break;
                }
                case ShareAddErrorCode.DepotNumberEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DepotNumberEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    cbxDepotNumber.Focus();
                    break;
                }
                case ShareAddErrorCode.OrderNumberEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/OrderNumberEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxOrderNumber.Focus();
                    break;
                }
                case ShareAddErrorCode.OrderNumberExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/OrderNumberExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxOrderNumber.Focus();
                    break;
                }
                case ShareAddErrorCode.VolumeEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxVolume.Focus();
                    break;
                }
                case ShareAddErrorCode.VolumeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxVolume.Focus();
                    break;
                }
                case ShareAddErrorCode.VolumeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/VolumeWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxVolume.Focus();
                    break;
                }
                case ShareAddErrorCode.SharePriceEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxSharePrice.Focus();
                    break;
                }
                case ShareAddErrorCode.SharePriceWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxSharePrice.Focus();
                    break;
                }
                case ShareAddErrorCode.SharePriceWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/SharePriceWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxSharePrice.Focus();
                    break;
                }
                case ShareAddErrorCode.ProvisionWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ProvisionWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxProvision.Focus();
                    break;
                }
                case ShareAddErrorCode.ProvisionWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ProvisionWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxProvision.Focus();
                    break;
                }
                case ShareAddErrorCode.BrokerFeeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/BrokerFeeWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxBrokerFee.Focus();
                    break;
                }
                case ShareAddErrorCode.BrokerFeeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/BrokerFeeWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxBrokerFee.Focus();
                    break;
                }
                case ShareAddErrorCode.TraderPlaceFeeWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/TraderPlaceFeeWrongFormat",
                            LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxTraderPlaceFee.Focus();
                    break;
                }
                case ShareAddErrorCode.TraderPlaceFeeWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/TraderPlaceFeeWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxTraderPlaceFee.Focus();
                    break;
                }
                case ShareAddErrorCode.ReductionWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ReductionWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxReduction.Focus();
                    break;
                }
                case ShareAddErrorCode.ReductionWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ReductionWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxReduction.Focus();
                    break;
                }
                case ShareAddErrorCode.BrokerageEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/BrokerageEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxBrokerage.Focus();
                    break;
                }
                case ShareAddErrorCode.BrokerageWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/BrokerageWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxBrokerage.Focus();
                    break;
                }
                case ShareAddErrorCode.BrokerageWrongValue:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/BrokerageWrongValue", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxBrokerage.Focus();
                    break;
                }

                case ShareAddErrorCode.DetailsWebSiteEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DetailsWebSiteEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxDetailsWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.DetailsWebSiteWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DetailsWebSiteWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxDetailsWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.DetailsWebSiteExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DetailsWebSiteExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxDetailsWebSite.Focus();
                    break;
                }


                case ShareAddErrorCode.MarketValuesWebSiteEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/MarketValuesWebSiteEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxMarketWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.MarketWebSiteWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/MarketValuesWebSiteWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxMarketWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.MarketWebSiteExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/MarketValuesWebSiteExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxMarketWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.DailyValuesWebSiteEmpty:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DailyValuesWebSiteEmpty", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxDailyValuesWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.DailyValuesWebSiteWrongFormat:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DailyValuesWebSiteWrongFormat", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxDailyValuesWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.DailyValuesWebSiteExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DailyValuesWebSiteExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    txtBoxDailyValuesWebSite.Focus();
                    break;
                }
                case ShareAddErrorCode.DocumentDirectoryDoesNotExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DocumentDirectoryDoesNotExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    break;
                }
                case ShareAddErrorCode.DocumentFileDoesNotExists:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/DocumentFileDoesNotExists", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    break;
                }
                case ShareAddErrorCode.WebSiteRegexNotFound:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/WebSiteRegexNotFound", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    break;
                }
                default:
                {
                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/UnknownState", SettingsConfiguration.LanguageName);
                    clrMessage = Color.Red;
                    stateLevel = FrmMain.EStateLevels.Error;
                    break;
                }
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageaAddShare,
               strMessage,
               Language,
               LanguageName,
               clrMessage,
               Logger,
               (int)stateLevel,
               (int)FrmMain.EComponentLevels.Application);
        }

        public void DocumentBrowseFinish()
        {
            // Set messages
            var strMessage = string.Empty;
            var clrMessage = Color.Black;
            const FrmMain.EStateLevels stateLevel = FrmMain.EStateLevels.Info;

            switch (ErrorCode)
            {
                case ShareAddErrorCode.DocumentBrowseFailed:
                {
                    txtBoxDocument.Text = @"-";

                    strMessage =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/ChoseDocumentFailed",
                            LanguageName);
                } break;
                default:
                {
                    if (_parsingBackgroundWorker.IsBusy)
                    {
                        _parsingBackgroundWorker.CancelAsync();
                    }
                    else
                    {
                        ResetValues();
                        _parsingBackgroundWorker.RunWorkerAsync();
                    }

                    Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);
                } break;
            }

            Helper.AddStatusMessage(toolStripStatusLabelMessageaAddShare,
                strMessage,
                Language,
                LanguageName,
                clrMessage,
                Logger,
                (int)stateLevel,
                (int)FrmMain.EComponentLevels.Application);
        }

        #endregion IViewMember

        #region Event Members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler FormatInputValuesEventHandler;
        public event EventHandler ShareAddEventHandler;
        public event EventHandler DocumentBrowseEventHandler;

        #endregion Event Members

        #region Form

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewShareAdd(FrmMain parentWindow, Logger logger, Language language, string strLanguage, List<WebSiteRegex> webSiteRegexList, string strParsingFileName = null)
        {
            InitializeComponent();

            ParentWindow = parentWindow;
            Logger = logger;
            Language = language;
            LanguageName = strLanguage;
            WebSiteRegexList = webSiteRegexList;
            ParsingFileName = strParsingFileName;
            StopFomClosingFlag = false;

            #region Parsing backgroundworker

            _parsingBackgroundWorker.WorkerReportsProgress = true;
            _parsingBackgroundWorker.WorkerSupportsCancellation = true;

            // Check if the PDF converter is installed so initialize the background worker for the parsing
            if (Helper.PdfParserInstalled())
            {
                _parsingBackgroundWorker.DoWork += DocumentParsing;
                _parsingBackgroundWorker.ProgressChanged += OnDocumentParsingProgressChanged;
                _parsingBackgroundWorker.RunWorkerCompleted += OnDocumentParsingRunWorkerCompleted;
            }

            #endregion Parsing backgroundworker
        }

        /// <summary>
        /// This function is called when the form is closing
        /// Here we check if the form really should be closed
        /// </summary>
        /// <param name="sender">Form</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void FrmShareAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the form should be closed
            if (StopFomClosingFlag)
            {
                // Stop closing the form
                e.Cancel = true;
            }
            else
            {
                // Cleanup web browser
                Helper.WebBrowserPdf.CleanUp(webBrowser1);
            }

            // Reset closing flag
            StopFomClosingFlag = false;
        }

        /// <summary>
        /// This function creates the share add form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmShareAdd_Load(object sender, EventArgs e)
        {
            try
            {
                #region Language configuration

                Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Caption", SettingsConfiguration.LanguageName);
                grpBoxGeneral.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Caption", SettingsConfiguration.LanguageName);
                grpBoxDocumentPreview.Text =
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxDocumentPreview/Caption", SettingsConfiguration.LanguageName);

                lblWkn.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/WKN", SettingsConfiguration.LanguageName);
                lblIsin.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/ISIN", SettingsConfiguration.LanguageName);
                lblName.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Name", SettingsConfiguration.LanguageName);
                lblStockMarketLaunchDate.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/StockMarketLaunchDate", SettingsConfiguration.LanguageName);

                lblShareType.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/ShareType", SettingsConfiguration.LanguageName);
                // Add share type values
                Language.GetLanguageTextListByXPath(@"/ComboBoxItemsShareType/*", SettingsConfiguration.LanguageName).ForEach(item => cbxShareType.Items.Add(item));

                lblDividendPayoutInterval.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/PayoutInterval", SettingsConfiguration.LanguageName);
                // Add dividend payout interval values
                Language.GetLanguageTextListByXPath(@"/ComboBoxItemsPayout/*", SettingsConfiguration.LanguageName).ForEach(item => cbxDividendPayoutInterval.Items.Add(item));

                lblCultureInfo.Text =
                    Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/CultureInfo", SettingsConfiguration.LanguageName);

                lblDetailsWebSite.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/DetailsWebSite", SettingsConfiguration.LanguageName);
                lblMarketWebSite.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/MarketValuesWebSite", SettingsConfiguration.LanguageName);
                // Add parsing options values
                Language.GetLanguageTextListByXPath(@"/ComboBoxItemsParsingType/*", SettingsConfiguration.LanguageName).ForEach(item => cbxMarketWebSiteParsingType.Items.Add(item));
                lblMarketWebSiteApiKey.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/MarketValuesWebSiteApiKey", SettingsConfiguration.LanguageName);

                lblDailyValuesWebSite.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/DailyValuesWebSite", SettingsConfiguration.LanguageName);
                // Add parsing options values
                Language.GetLanguageTextListByXPath(@"/ComboBoxItemsParsingType/*", SettingsConfiguration.LanguageName).ForEach(item => cbxDailyValuesWebSiteParsingType.Items.Add(item));
                lblDailyValuesWebSiteApiKey.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/DailyValuesWebSiteApiKey", SettingsConfiguration.LanguageName);

                lblDepotNumber.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/DepotNumber", SettingsConfiguration.LanguageName);
                lblOrderNumber.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/OrderNumber", SettingsConfiguration.LanguageName);
                lblDate.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Date", SettingsConfiguration.LanguageName);
                lblVolume.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Volume", SettingsConfiguration.LanguageName);
                lblVolumeUnit.Text = ShareObject.PieceUnit;
                lblSharePrice.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/SharePrice", SettingsConfiguration.LanguageName);
                lblSharePriceUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblBuyValue.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/MarketValue", SettingsConfiguration.LanguageName);
                lblBuyValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblProvision.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Provision", SettingsConfiguration.LanguageName);
                lblProvisionValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblBrokerFee.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/BrokerFee", SettingsConfiguration.LanguageName);
                lblBrokerFeeValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblTraderPlaceFee.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/TraderPlaceFee", SettingsConfiguration.LanguageName);
                lblTraderPlaceFeeValueUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblReduction.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Reduction", SettingsConfiguration.LanguageName);
                lblReductionUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblBrokerage.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Brokerage", SettingsConfiguration.LanguageName);
                lblBrokerageUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;
                lblBuyValueBrokerageReduction.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/FinalValue", SettingsConfiguration.LanguageName);
                lblBuyValueBrokerageReductionUnit.Text = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID).CurrencySymbol;

                lblDocument.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/GrpBoxGeneral/Labels/Document", SettingsConfiguration.LanguageName);

                btnSave.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Save", SettingsConfiguration.LanguageName);
                btnCancel.Text = Language.GetLanguageTextByXPath(@"/AddFormShare/Buttons/Cancel", SettingsConfiguration.LanguageName);

                #endregion Language configuration

                #region Get culture info

                var listCultures = new List<string>();
                foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    listCultures.Add($"{ci.Name}");
                }

                listCultures.Sort();

                foreach (var value in listCultures)
                {
                    if (value != "")
                        cbxCultureInfo.Items.Add(value);
                }

                var cultureInfo = CultureInfo.CurrentCulture;

                cbxCultureInfo.SelectedIndex = cbxCultureInfo.FindStringExact(cultureInfo.Name);

                #endregion Get culture info

                #region Get depot numbers and the corresponding banks

                var listBankRegex = DocumentParsingConfiguration.BankRegexList;

                cbxDepotNumber.Items.Add(@"-");
                foreach (var bankRegex in listBankRegex)
                {
                    cbxDepotNumber.Items.Add(bankRegex.BankIdentifier + @" - " + bankRegex.BankName);
                }

                #endregion Get depot numbers and the corresponding banks

                // Load button images
                btnSave.Image = Resources.button_save_24;
                btnCancel.Image = Resources.button_reset_24;

                cbxDividendPayoutInterval.SelectedIndex = 0;
                cbxShareType.SelectedIndex = 0;

                cbxMarketWebSiteParsingType.SelectedIndex = 0;
                cbxDailyValuesWebSiteParsingType.SelectedIndex = 0;

                dateTimePickerStockMarketLaunch.MinDate = DateTime.MinValue;
                dateTimePickerStockMarketLaunch.Value = dateTimePickerStockMarketLaunch.MinDate;
                dateTimePickerDate.Value = DateTime.Now;
                dateTimePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageaAddShare,
                    Language.GetLanguageTextByXPath(@"/MainForm/Errors/AddShowFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);
            }
        }

        /// <summary>
        /// This function set the focus on the WKN number edit box
        /// when the form is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmShareAdd_Shown(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);

            // If a parsing file name is given the form directly starts with the document parsing
            if (ParsingFileName != null)
            {
                _parsingStartAllow = true;
                txtBoxDocument.Text = ParsingFileName;
            }

            txtBoxWkn.Focus();
        }

        #endregion Form

        #region Button

        /// <summary>
        /// This function opens a file dialog and the user
        /// can chose a file which documents the buy of the share
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnShareDocumentBrowse_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelMessageAddShareDocumentParsing.Text = string.Empty;
            toolStripStatusLabelMessageaAddShare.Text = string.Empty;

            DocumentBrowseEventHandler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This function stores the values to the share object
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls
                Enabled = false;

                // Reset parsing status message
                toolStripStatusLabelMessageAddShareDocumentParsing.Text = string.Empty;

                ShareAddEventHandler?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                StopFomClosingFlag = true;

                Helper.AddStatusMessage(toolStripStatusLabelMessageaAddShare,
                    Language.GetLanguageTextByXPath(@"/AddFormShare/Errors/AddSaveFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName,
                    Color.DarkRed, Logger, (int) FrmMain.EStateLevels.FatalError,
                    (int) FrmMain.EComponentLevels.Application,
                    ex);

                // Enable controls
                Enabled = true;
            }
        }

        /// <summary>
        /// This function close the form
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            StopFomClosingFlag = false;
            Close();
        }

        #endregion Button

        #region Date / Time

        private void DatePickerDate_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
        }

        private void DatePickerTime_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
        }

        #endregion Date / Time

        #region TextBoxes

        private void OnTxtBoxWkn_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wkn"));
        }

        private void OnTxtBoxIsin_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Isin"));
        }

        private void OnTxtBoxName_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
        }

        private void OnDateTimePickerStockMarketLaunch_ValueChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StockMarketLaunchDate"));
        }

        private void OnTxtBoxDetailsWebSite_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DetailsWebSite"));
        }

        private void OnTxtBoxMarketValuesWebSite_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MarketValuesWebSite"));
        }

        private void OnTxtBoxDailyValuesWebSite_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DailyValuesWebSite"));
        }

        private void OnTxtBoxDailyValuesWebSite_Leave(object sender, EventArgs e)
        {
            txtBoxDailyValuesWebSite.Text = Helper.RegexReplaceStartDateAndInterval(txtBoxDailyValuesWebSite.Text, DailyValuesParsingOption);
        }

        private void OntTxtBoxOrderNumber_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OrderNumber"));
        }

        private void OnTxtBoxVolume_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Volume"));
        }

        private void OnTxtBoxVolume_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxSharePrice_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SharePrice"));
        }

        private void OnTxtBoxSharePrice_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxProvision_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Provision"));
        }

        private void OnTxtBoxProvision_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxBrokerFee_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BrokerFee"));
        }

        private void OnTxtBoxBrokerFee_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxTraderPlaceFee_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TraderPlaceFee"));
        }

        private void OnTxtBoxTraderPlaceFee_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxReduction_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reduction"));
        }

        private void OnTxtBoxReduction_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxBrokerage_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Brokerage"));
        }

        private void OnTxtBoxBrokerage_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxDocument_TextChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Document"));

            if (_parsingStartAllow)
            {
                if (_parsingBackgroundWorker.IsBusy)
                {
                    _parsingBackgroundWorker.CancelAsync();
                }
                else
                {
                    ResetValues();
                    _parsingBackgroundWorker.RunWorkerAsync();
                }
            }

            _parsingStartAllow = false;
        }

        private void OnTxtBoxDocument_Leave(object sender, EventArgs e)
        {
            FormatInputValuesEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                _parsingStartAllow = true;

                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length <= 0 || files.Length > 1) return;

                txtBoxDocument.Text = files[0];

                // Check if the document is a PDF
                var extenstion = Path.GetExtension(txtBoxDocument.Text);

                if (string.Compare(extenstion, ".PDF", StringComparison.OrdinalIgnoreCase) == 0 && _parsingStartAllow)
                {
                    if (_parsingBackgroundWorker.IsBusy)
                    {
                        _parsingBackgroundWorker.CancelAsync();
                    }
                    else
                    {
                        ResetValues();
                        _parsingBackgroundWorker.RunWorkerAsync();
                    }
                }

                _parsingStartAllow = false;

                //webBrowser1.Navigate(temp.DocumentAsStr);
                Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);
            }
            catch (Exception ex)
            {
                Helper.AddStatusMessage(toolStripStatusLabelMessageAddShareDocumentParsing,
                    Language.GetLanguageTextByXPath(@"/AddEditFormBuy/ParsingErrors/ParsingFailed", SettingsConfiguration.LanguageName),
                    Language, SettingsConfiguration.LanguageName, Color.DarkRed, Logger,
                    (int)FrmMain.EStateLevels.FatalError, (int)FrmMain.EComponentLevels.Application,
                    ex);

                toolStripProgressBarAddShareDocumentParsing.Visible = false;
                grpBoxGeneral.Enabled = true;

                _parsingStartAllow = false;
                _parsingThreadFinished = true;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);

                if (DocumentTypeParser != null)
                    DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
            }
        }

        #endregion TextBoxes

        #region ComboBoxes

        private void OnCboBoxCultureInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CultureInfo"));
        }

        private void OnCbxDividendPayoutInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DividendPayoutInterval"));
        }

        private void OnCbxDepotNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepotNumber"));
        }

        private void OnCbxShareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShareType"));
        }

        private void OnCbxMarketValuesParsingOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MarketValuesParsingOption"));

            if (SettingsConfiguration.ApiKeysDictionary.ContainsKey(((ComboBox)sender).SelectedItem.ToString()))
            {
                lblMarketWebSiteApiKeyValue.Text =
                    SettingsConfiguration.ApiKeysDictionary[((ComboBox)sender).SelectedItem.ToString()];

            }
            else
            {
                lblMarketWebSiteApiKeyValue.Text = string.Empty;
            }
        }

        private void OnCbxDailyValuesParsingOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DailyValuesParsingOption"));

            if (SettingsConfiguration.ApiKeysDictionary.ContainsKey(((ComboBox)sender).SelectedItem.ToString()))
            {
                lblDailyWebSiteApiKeyValue.Text =
                    SettingsConfiguration.ApiKeysDictionary[((ComboBox)sender).SelectedItem.ToString()];

            }
            else
            {
                lblDailyWebSiteApiKeyValue.Text = string.Empty;
            }

            if (((ComboBox)sender).SelectedIndex != 0)
            {
                txtBoxDailyValuesWebSite.Text = Helper.RegexReplaceStartDateAndInterval(txtBoxDailyValuesWebSite.Text, DailyValuesParsingOption);
            }
        }

        #endregion ComboBoxes

        #region Parsing

        private void DocumentParsing(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Reset parsing variables
                _parsingResult = true;
                _parsingThreadFinished = false;
                _bankCounter = 0;
                _bankIdentifierFound = false;
                _buyIdentifierFound = false;
                _documentTypNotImplemented = false;
                _documentValuesRunning = false;

                ParsingText = string.Empty;

                _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingStarted);

                DocumentType = DocumentParsingConfiguration.DocumentTypes.BuyDocument;
                DocumentTypeParser = null;
                DictionaryParsingResult = null;

                Helper.RunProcess(Helper.PdfToTextApplication,
                    $"-simple \"{txtBoxDocument.Text}\" {Helper.ParsingDocumentFileName}");

                ParsingText = File.ReadAllText(Helper.ParsingDocumentFileName, Encoding.Default);

                // Start document parsing
                DocumentTypeParsing();

                while (!_parsingThreadFinished)
                {
                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException)
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingDocumentFailed);
            }
            catch (Exception)
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingDocumentFailed);
            }
        }

        /// <summary>
        /// This function starts the document parsing process
        /// which checks if the document typ is correct
        /// </summary>
        private void DocumentTypeParsing()
        {
            try
            {
                if (DocumentParsingConfiguration.InitFlag)
                {
                    if (DocumentTypeParser == null)
                        DocumentTypeParser = new Parser.Parser();

                    DocumentTypeParser.ParsingValues = new ParsingValues(ParsingText,
                        DocumentParsingConfiguration.BankRegexList[_bankCounter].BankEncodingType,
                        DocumentParsingConfiguration.BankRegexList[_bankCounter].BankRegexList
                    );
                    
                    // Check if the Parser is in idle mode
                    if (DocumentTypeParser != null && DocumentTypeParser.ParserInfoState.State == DataTypes.ParserState.Idle)
                    {
                        DocumentTypeParser.OnParserUpdate += DocumentTypeParser_UpdateGUI;

                        // Reset flags
                        _bankIdentifierFound = false;
                        _buyIdentifierFound = false;

                        // Start document parsing
                        DocumentTypeParser.StartParsing();
                    }
                    else
                    {
                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                        _parsingThreadFinished = true;
                        _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingFailed);
                    }
                }
                else
                {
                    _parsingThreadFinished = true;
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingParsingDocumentError);
                }
            }
            catch (Exception)
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingFailed);
            }
        }

        /// <summary>
        /// This event handler updates the progress while checking the document type
        /// </summary>
        /// <param name="sender">BackGroundWorker</param>
        /// <param name="e">ProgressChangedEventArgs</param>
        private void DocumentTypeParser_UpdateGUI(object sender, DataTypes.OnParserUpdateEventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => DocumentTypeParser_UpdateGUI(sender, e)));
                }
                else
                {
                    try
                    {
                        switch (e.ParserInfoState.LastErrorCode)
                        {
                            case DataTypes.ParserErrorCodes.Finished:
                            {
                                //if (e.ParserInfoState.SearchResult != null)
                                //{
                                //    foreach (var result in e.ParserInfoState.SearchResult)
                                //    {
                                //        Console.Write(@"{0}:", result.Key);
                                //        if (result.Value != null && result.Value.Count > 0)
                                //            Console.WriteLine(@"{0}", result.Value[0]);
                                //        else
                                //            Console.WriteLine(@"-");
                                //    }
                                //}
                                break;
                            }
                            case DataTypes.ParserErrorCodes.SearchFinished:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.SearchRunning:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.SearchStarted:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ContentLoadFinished:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ContentLoadStarted:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.Started:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.Starting:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.NoError:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.StartFailed:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.BusyFailed:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.InvalidWebSiteGiven:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.NoRegexListGiven:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.NoWebContentLoaded:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.JsonError:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ParsingFailed:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.CancelThread:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.FileExceptionOccurred:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.JsonExceptionOccurred:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.WebExceptionOccurred:
                            {
                                break;
                            }
                            case DataTypes.ParserErrorCodes.ExceptionOccurred:
                            {
                                break;
                            }
                        }

                        if (DocumentTypeParser.ParserErrorCode > 0)
                            Thread.Sleep(100);

                        // Check if a error occurred or the process has been finished
                        if (e.ParserInfoState.LastErrorCode < 0 ||
                            e.ParserInfoState.LastErrorCode == DataTypes.ParserErrorCodes.Finished)
                        {
                            if (e.ParserInfoState.LastErrorCode < 0)
                            {
                                // Set fail message
                                _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingFailed);
                            }
                            else
                            {
                                //Check if the correct bank identifier and document identifier may have been found so search for the document values
                                if (e.ParserInfoState.SearchResult != null &&
                                    (
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName) ||
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BuyIdentifierTagName)
                                    )
                                )
                                {
                                    // Check if the bank identifier has been found and is the right one
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName)
                                        &&
                                        DocumentParsingConfiguration
                                            .BankRegexList[_bankCounter].BankIdentifier ==
                                        e.ParserInfoState.SearchResult[
                                            DocumentParsingConfiguration.BankIdentifierTagName][0]
                                    )
                                        _bankIdentifierFound = true;
                                    // Check if the buy identifier has been found
                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BuyIdentifierTagName))
                                        _buyIdentifierFound = true;

                                    if (e.ParserInfoState.SearchResult != null &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BankIdentifierTagName) &&
                                        e.ParserInfoState.SearchResult.ContainsKey(DocumentParsingConfiguration
                                            .BuyIdentifierTagName))
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingIdentifierValuesFound);

                                        _documentValuesRunning = true;

                                        // Get the correct parsing options for the given document type
                                        switch (DocumentType)
                                        {
                                            case DocumentParsingConfiguration.DocumentTypes.BuyDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeBuy]
                                                        .DocumentRegexList
                                                );
                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            case DocumentParsingConfiguration.DocumentTypes.SaleDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeSale]
                                                        .DocumentRegexList
                                                );
                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            case DocumentParsingConfiguration.DocumentTypes.DividendDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeDividend]
                                                        .DocumentRegexList
                                                );
                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            case DocumentParsingConfiguration.DocumentTypes.BrokerageDocument:
                                            {
                                                var parsingValues = DocumentTypeParser.ParsingValues;
                                                DocumentTypeParser.ParsingValues = new ParsingValues(
                                                    parsingValues.ParsingText,
                                                    parsingValues.EncodingType,
                                                    DocumentParsingConfiguration
                                                        .BankRegexList[_bankCounter]
                                                        .DictionaryDocumentRegex[
                                                            DocumentParsingConfiguration.DocumentTypeBrokerage]
                                                        .DocumentRegexList
                                                );
                                                DocumentTypeParser.StartParsing();
                                            }
                                                break;
                                            default:
                                            {
                                                _documentTypNotImplemented = true;
                                            }
                                                break;
                                        }
                                    }
                                }

                                _bankCounter++;

                                // Check if another bank configuration should be checked
                                if (_bankCounter < DocumentParsingConfiguration.BankRegexList.Count &&
                                    _bankIdentifierFound == false)
                                {
                                    var parsingValues = DocumentTypeParser.ParsingValues;
                                    DocumentTypeParser.ParsingValues = new ParsingValues(
                                        parsingValues.ParsingText,
                                        parsingValues.EncodingType,
                                        DocumentParsingConfiguration
                                            .BankRegexList[_bankCounter].BankRegexList
                                    );
                                    DocumentTypeParser.StartParsing();
                                }
                                else
                                {
                                    if (_bankIdentifierFound == false)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingBankIdentifierFailed);

                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else if (_buyIdentifierFound == false)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingDocumentTypeIdentifierFailed);

                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else if (_documentTypNotImplemented)
                                    {
                                        _parsingBackgroundWorker.ReportProgress(
                                            (int) ParsingErrorCode.ParsingDocumentNotImplemented);

                                        DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                        _parsingThreadFinished = true;
                                    }
                                    else
                                    {
                                        if (_documentValuesRunning)
                                        {
                                            if (DocumentTypeParser.ParsingResult != null)
                                            {
                                                DictionaryParsingResult =
                                                    new Dictionary<string, List<string>>(DocumentTypeParser
                                                        .ParsingResult);

                                                DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                                                _parsingThreadFinished = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Show exception is not allowed here (THREAD)
                        // Only do progress report progress
                        _parsingThreadFinished = true;
                        _parsingStartAllow = false;

                        if (_parsingBackgroundWorker.WorkerReportsProgress)
                            _parsingBackgroundWorker.ReportProgress((int) ParsingErrorCode.ParsingDocumentFailed);

                        if (DocumentTypeParser != null)
                            DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
                    }
                }
            }
            catch (Exception)
            {
                // Show exception is not allowed here (THREAD)
                // Only do progress report progress
                _parsingThreadFinished = true;
                _parsingStartAllow = false;

                if (_parsingBackgroundWorker.WorkerReportsProgress)
                    _parsingBackgroundWorker.ReportProgress((int)ParsingErrorCode.ParsingDocumentFailed);

                if (DocumentTypeParser != null)
                    DocumentTypeParser.OnParserUpdate -= DocumentTypeParser_UpdateGUI;
            }
        }

        private void OnDocumentParsingProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case (int)ParsingErrorCode.ParsingStarted:
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingStateMessages/ParsingStarted",
                            LanguageName);

                    toolStripProgressBarAddShareDocumentParsing.Visible = true;
                    grpBoxGeneral.Enabled = false;
                    break;
                }
                case (int)ParsingErrorCode.ParsingParsingDocumentError:
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingErrors/ParsingParsingDocumentError", SettingsConfiguration.LanguageName);

                    toolStripProgressBarAddShareDocumentParsing.Visible = false;
                    grpBoxGeneral.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingFailed:
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingErrors/ParsingFailed", SettingsConfiguration.LanguageName);

                    toolStripProgressBarAddShareDocumentParsing.Visible = false;
                    grpBoxGeneral.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingDocumentNotImplemented:
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingErrors/ParsingDocumentNotImplemented",
                            LanguageName);

                    toolStripProgressBarAddShareDocumentParsing.Visible = false;
                    grpBoxGeneral.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingBankIdentifierFailed:
                {
                    //picBoxDepotNumberParseState.Image = Resources.search_failed_24;

                        toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingErrors/ParsingBankIdentifierFailed",
                            LanguageName);

                    toolStripProgressBarAddShareDocumentParsing.Visible = false;
                    grpBoxGeneral.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingDocumentTypeIdentifierFailed:
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(
                            @"/AddFormShare/ParsingErrors/ParsingDocumentTypeIdentifierFailed", SettingsConfiguration.LanguageName);

                    toolStripProgressBarAddShareDocumentParsing.Visible = false;
                    grpBoxGeneral.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingDocumentFailed:
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingErrors/ParsingDocumentFailed",
                            LanguageName);

                    toolStripProgressBarAddShareDocumentParsing.Visible = false;
                    grpBoxGeneral.Enabled = true;
                    break;
                }
                case (int)ParsingErrorCode.ParsingIdentifierValuesFound:
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingErrors/ParsingIdentifierValuesFound",
                            LanguageName);
                    break;
                }
            }
        }

        private void OnDocumentParsingRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Delete old parsing text file
            if (File.Exists(Helper.ParsingDocumentFileName))
                File.Delete(Helper.ParsingDocumentFileName);

            if (DictionaryParsingResult != null)
            {
                #region Check which values are found

                foreach (var resultEntry in DictionaryParsingResult)
                {
                    if (resultEntry.Value.Count <= 0) continue;

                    switch (resultEntry.Key)
                    {
                        case DocumentParsingConfiguration.DocumentTypeBuyWkn:
                        {
                            picBoxWknParseState.Image = Resources.search_ok_24;
                            txtBoxWkn.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyIsin:
                        {
                            picBoxIsinParseState.Image = Resources.search_ok_24;
                            txtBoxIsin.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyDepotNumber:
                        {
                            picBoxDepotNumberParseState.Image = Resources.search_ok_24;

                            cbxDepotNumber.SelectedIndex = cbxDepotNumber.FindString(DocumentParsingConfiguration.BankRegexList[_bankCounter - 2].BankIdentifier);
                        } break;
                        case DocumentParsingConfiguration.DocumentTypeBuyOrderNumber:
                        {
                            picBoxOrderNumberParseState.Image = Resources.search_ok_24;
                            txtBoxOrderNumber.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyName:
                        {
                            picBoxNameParseState.Image = Resources.search_ok_24;
                            txtBoxName.Text =
                                Helper.RemoveDoubleWhiteSpaces(resultEntry.Value[0].Trim());
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyDate:
                        {
                            picBoxDateParseState.Image = Resources.search_ok_24;
                            dateTimePickerDate.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyTime:
                        {
                            picBoxTimeParseState.Image = Resources.search_ok_24;
                            dateTimePickerTime.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyVolume:
                        {
                            picBoxVolumeParseState.Image = Resources.search_ok_24;
                            txtBoxVolume.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyPrice:
                        {
                            picBoxPriceParseState.Image = Resources.search_ok_24;
                            txtBoxSharePrice.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyProvision:
                        {
                            picBoxProvisionParseState.Image = Resources.search_ok_24;
                            txtBoxProvision.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyBrokerFee:
                        {
                            picBoxBrokerFeeParseState.Image = Resources.search_ok_24;
                            txtBoxBrokerFee.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee:
                        {
                            picBoxTraderPlaceFeeParseState.Image = Resources.search_ok_24;
                            txtBoxTraderPlaceFee.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                        case DocumentParsingConfiguration.DocumentTypeBuyReduction:
                        {
                            picBoxReductionParseState.Image = Resources.search_ok_24;
                            txtBoxReduction.Text = resultEntry.Value[0].Trim();
                            break;
                        }
                    }
                }

                #endregion Check which values are found

                #region Not found values

                // Which values are not found
                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyWkn) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyWkn ) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyWkn].Count == 0
                )
                {
                    picBoxWknParseState.Image = Resources.search_failed_24;
                    txtBoxWkn.Text = string.Empty;
                    _parsingResult = false;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyIsin) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyIsin) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyIsin].Count == 0
                   )
                {
                    picBoxIsinParseState.Image = Resources.search_failed_24;
                    txtBoxIsin.Text = string.Empty;
                    _parsingResult = false;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyName) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyName) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyName].Count == 0
                )
                {
                    picBoxNameParseState.Image = Resources.search_failed_24;
                    txtBoxName.Text = string.Empty;
                    _parsingResult = false;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyDate) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyDate) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyDate].Count == 0
                )
                {
                    picBoxDateParseState.Image = Resources.search_failed_24;
                    dateTimePickerDate.Value = DateTime.Now;
                    dateTimePickerTime.Value =
                        new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    _parsingResult = false;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTime) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTime) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyTime].Count == 0
                )
                {
                    dateTimePickerTime.Value =
                        new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyOrderNumber) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyOrderNumber) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyOrderNumber].Count == 0
                )
                {
                    picBoxOrderNumberParseState.Image = Resources.search_failed_24;
                    txtBoxOrderNumber.Text = string.Empty;
                    _parsingResult = false;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyVolume) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyVolume) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyVolume].Count == 0
                )
                {
                    picBoxVolumeParseState.Image = Resources.search_failed_24;
                    txtBoxVolume.Text = string.Empty;
                    _parsingResult = false;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyPrice) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyPrice) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyPrice].Count == 0
                )
                {
                    picBoxPriceParseState.Image = Resources.search_failed_24;
                    txtBoxSharePrice.Text = string.Empty;
                    _parsingResult = false;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyProvision) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyProvision) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyProvision].Count == 0
                )
                {
                    picBoxProvisionParseState.Image = Resources.search_info_24;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyBrokerFee) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyBrokerFee) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyBrokerFee].Count == 0
                )
                {
                    picBoxBrokerFeeParseState.Image = Resources.search_info_24;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyTraderPlaceFee].Count == 0
                )
                {
                    picBoxTraderPlaceFeeParseState.Image = Resources.search_info_24;
                }

                if (!DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyReduction) ||
                    DictionaryParsingResult.ContainsKey(DocumentParsingConfiguration.DocumentTypeBuyReduction) &&
                    DictionaryParsingResult[DocumentParsingConfiguration.DocumentTypeBuyReduction].Count == 0
                )
                {
                    picBoxReductionParseState.Image = Resources.search_info_24;
                }

                #endregion Not found values

                if (!_parsingResult)
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Red;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingErrors/ParsingFailed", SettingsConfiguration.LanguageName);
                }
                else
                {
                    toolStripStatusLabelMessageAddShareDocumentParsing.ForeColor = Color.Black;
                    toolStripStatusLabelMessageAddShareDocumentParsing.Text =
                        Language.GetLanguageTextByXPath(@"/AddFormShare/ParsingStateMessages/ParsingDocumentSuccessful",
                            LanguageName);
                }
            }

            toolStripProgressBarAddShareDocumentParsing.Visible = false;
            grpBoxGeneral.Enabled = true;
        }

        private void ResetValues()
        {
            // Reset state pictures
            picBoxWknParseState.Image = Resources.empty_arrow;
            picBoxIsinParseState.Image = Resources.empty_arrow;
            picBoxNameParseState.Image = Resources.empty_arrow;
            picBoxDepotNumberParseState.Image = Resources.empty_arrow;
            picBoxOrderNumberParseState.Image = Resources.empty_arrow;
            picBoxDateParseState.Image = Resources.empty_arrow;
            picBoxTimeParseState.Image = Resources.empty_arrow;
            picBoxVolumeParseState.Image = Resources.empty_arrow;
            picBoxPriceParseState.Image = Resources.empty_arrow;
            picBoxProvisionParseState.Image = Resources.empty_arrow;
            picBoxBrokerFeeParseState.Image = Resources.empty_arrow;
            picBoxTraderPlaceFeeParseState.Image = Resources.empty_arrow;
            picBoxReductionParseState.Image = Resources.empty_arrow;

            // Reset textboxes
            txtBoxWkn.Text = string.Empty;
            txtBoxIsin.Text = string.Empty;
            txtBoxName.Text = string.Empty;
            dateTimePickerStockMarketLaunch.MinDate = DateTime.MinValue;
            dateTimePickerStockMarketLaunch.Value = dateTimePickerStockMarketLaunch.MinDate;
            cbxShareType.SelectedIndex = 0;
            cbxDividendPayoutInterval.SelectedIndex = 0;
            dateTimePickerDate.Value = DateTime.Now;
            dateTimePickerTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            txtBoxDetailsWebSite.Text = string.Empty;
            txtBoxMarketWebSite.Text = string.Empty;
            cbxMarketWebSiteParsingType.SelectedIndex = 0;
            lblMarketWebSiteApiKey.Text = string.Empty;
            txtBoxDailyValuesWebSite.Text = string.Empty;
            cbxDailyValuesWebSiteParsingType.SelectedIndex = 0;
            lblDailyValuesWebSiteApiKey.Text = string.Empty;
            cbxDepotNumber.SelectedIndex = 0;
            txtBoxOrderNumber.Text = string.Empty;
            txtBoxVolume.Text = string.Empty;
            txtBoxSharePrice.Text = string.Empty;
            txtBoxProvision.Text = string.Empty;
            txtBoxBrokerFee.Text = string.Empty;
            txtBoxTraderPlaceFee.Text = string.Empty;
            txtBoxReduction.Text = string.Empty;

            // Reset status strip
            toolStripStatusLabelMessageaAddShare.Text = string.Empty;
            toolStripStatusLabelMessageAddShareDocumentParsing.Text = string.Empty;
            toolStripProgressBarAddShareDocumentParsing.Visible = false;

            // Reset document web browser
            Helper.WebBrowserPdf.Reload(webBrowser1, txtBoxDocument.Text);
        }

        #endregion Parsing
    }
}
