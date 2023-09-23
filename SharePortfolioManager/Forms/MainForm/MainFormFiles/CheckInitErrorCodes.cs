using System;
using System.Drawing;
using System.Windows.Forms;
using Logging;
using SharePortfolioManager.Classes;
using SharePortfolioManager.Classes.Configurations;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        private void CheckInitErrorCodes()
        {
            #region Check configuration error codes

            // Check website configuration read
            if (SettingsConfiguration.ErrorCode != SettingsConfiguration.ESettingsErrorCode.ConfigurationLoadSuccessful ||
                WebSiteConfiguration.ErrorCode != WebSiteConfiguration.EWebSiteErrorCode.ConfigurationLoadSuccessful ||
                LanguageConfiguration.ErrorCode != LanguageConfiguration.ELanguageErrorCode.ConfigurationLoadSuccessful ||
                DocumentParsingConfiguration.ErrorCode != DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationLoadSuccessful ||
                Logger.InitState != Logger.EInitState.Initialized
                )
            {
                // Enable controls
                EnableDisableControlNames.Clear();
                EnableDisableControlNames.Add("menuStrip1");
                Helper.EnableDisableControls(true, this, EnableDisableControlNames);

                // Disable controls
                EnableDisableControlNames.Clear();
                EnableDisableControlNames.Add("newToolStripMenuItem");
                EnableDisableControlNames.Add("openToolStripMenuItem");
                EnableDisableControlNames.Add("saveAsToolStripMenuItem");
                EnableDisableControlNames.Add("settingsToolStripMenuItem");
                EnableDisableControlNames.Add("apiKeysToolStripMenuItem");
                Helper.EnableDisableControls(false, menuStrip1, EnableDisableControlNames);

                // Disable controls
                EnableDisableControlNames.Clear();
                EnableDisableControlNames.Add("grpBoxSharePortfolio");
                EnableDisableControlNames.Add("grpBoxShareDetails");
                EnableDisableControlNames.Add("grpBoxStatusMessage");
                EnableDisableControlNames.Add("grpBoxUpdateState");
                EnableDisableControlNames.Add("grpBoxDocumentCapture");

                EnableDisableControlNames.Add("btnRefreshAll");
                EnableDisableControlNames.Add("btnRefresh");
                EnableDisableControlNames.Add("btnAdd");
                EnableDisableControlNames.Add("btnEdit");
                EnableDisableControlNames.Add("btnDelete");
                Helper.EnableDisableControls(false, tblLayPnlShareOverviews, EnableDisableControlNames);

                SettingsConfiguration.PortfolioName = @"";

                pgbLoadingPortfolio.Value = pgbLoadingPortfolio.Minimum;
                tblLayPnlLoadingPortfolio.Visible = false;
            }

            #region Language error code check 

            // Check language configuration error code
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (LanguageConfiguration.ErrorCode)
            {
                case LanguageConfiguration.ELanguageErrorCode.FileDoesNotExit:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            "Language configuration file '"
                            + LanguageConfiguration.FileName +
                            "' does not exists.",
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError,
                            (int)EComponentLevels.Application);
                    }
                    break;
                case LanguageConfiguration.ELanguageErrorCode.ConfigurationXmlError:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            "A XML syntax error exists in the language configuration file '"
                            + LanguageConfiguration.FileName +
                            "'!",
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int)EStateLevels.FatalError,
                            (int)EComponentLevels.Application,
                            SettingsConfiguration.LastException);
                    }
                    break;
                case LanguageConfiguration.ELanguageErrorCode.ConfigurationLoadFailed:
                    {
                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            "Could not load language configuration file '"
                            + LanguageConfiguration.FileName +
                            "'!",
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                            (int) EComponentLevels.Application,
                            SettingsConfiguration.LastException);
                    }
                    break;
                case LanguageConfiguration.ELanguageErrorCode.ConfigurationLoadSuccessful:
                    {
                        #region Add language menu items for the available languages

                        // Get possible languages and add them to the menu
                        var strLanguages = LanguageConfiguration.Language.GetAvailableLanguages();

                        // Get settings menu item
                        var tmiSettings =
                            (ToolStripMenuItem)menuStrip1.Items["settingsToolStripMenuItem"];
                        // Get language menu item
                        var tmiLanguage =
                            (ToolStripMenuItem)tmiSettings.DropDownItems["languageToolStripMenuItem"];

                        // Add available to the menu
                        foreach (var strLanguage in strLanguages)
                        {
                            var tmiLanguageAdd = new ToolStripMenuItem(strLanguage, null, OnLanguageClick,
                                $"languageToolStripMenuItem{strLanguage}");

                            switch (strLanguage)
                            {
                                case "German":
                                    tmiLanguageAdd.Image = Properties.Resources.menu_flag_german_24;
                                    break;
                                case "English":
                                    tmiLanguageAdd.Image = Properties.Resources.menu_flag_usa_24;
                                    break;
                            }

                            tmiLanguageAdd.ImageScaling = ToolStripItemImageScaling.None;

                            if (strLanguage == SettingsConfiguration.LanguageName)
                                tmiLanguageAdd.Checked = true;

                            tmiLanguage.DropDownItems.Add(tmiLanguageAdd);
                        }

                        #endregion Add language menu items for the available languages

                        Helper.AddStatusMessage(rchTxtBoxStateMessage,
                            LanguageConfiguration.Language.GetLanguageTextByXPath(
                                @"/MainForm/StatusMessages/LoadingLanguageConfigurationSuccessful",
                                SettingsConfiguration.LanguageName),
                            LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                            Color.Green, Logger, (int)EStateLevels.Info, (int)EComponentLevels.Application);
                    }
                    break;
                default:
                    {
                        throw (new NotImplementedException());
                    }
            }

            #endregion Language error code check 

            // Check if the language could be loaded
            if (LanguageConfiguration.ErrorCode !=
                LanguageConfiguration.ELanguageErrorCode.ConfigurationLoadSuccessful) return;

            #region Settings error code check

            // Check settings configuration error code
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (SettingsConfiguration.ErrorCode)
            {
                case SettingsConfiguration.ESettingsErrorCode.FileDoesNotExit:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/SettingsConfigurationErrors/FileDoesNotExists_1",
                            SettingsConfiguration.LanguageName)
                        + SettingsConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/SettingsConfigurationErrors/FileDoesNotExists_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application);
                }
                    break;
                case SettingsConfiguration.ESettingsErrorCode.ConfigurationXmlError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/SettingsConfigurationErrors/XMLSyntaxFailure_1",
                            SettingsConfiguration.LanguageName)
                        + SettingsConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/SettingsConfigurationErrors/XMLSyntaxFailure_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        SettingsConfiguration.LastException);
                }
                    break;
                case SettingsConfiguration.ESettingsErrorCode.ConfigurationLoadFailed:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/SettingsConfigurationErrors/CouldNotLoadFile_1",
                            SettingsConfiguration.LanguageName)
                        + SettingsConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/SettingsConfigurationErrors/CouldNotLoadFile_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        SettingsConfiguration.LastException);
                }
                    break;
                case SettingsConfiguration.ESettingsErrorCode.ConfigurationLoadSuccessful:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/StatusMessages/LoadingSettingsConfigurationSuccessful",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.Green, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Application);
                }
                    break;
                default:
                {
                    throw (new NotImplementedException());
                }
            }

            #endregion Settings error code check

            #region Website configuration error code check

            // Check website configuration error code
            switch (WebSiteConfiguration.ErrorCode)
            {
                case WebSiteConfiguration.EWebSiteErrorCode.FileDoesNotExit:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/FileDoesNotExists_1",
                            SettingsConfiguration.LanguageName)
                        + WebSiteConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/FileDoesNotExists_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application);
                }
                    break;
                case WebSiteConfiguration.EWebSiteErrorCode.ConfigurationEmpty:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/ConfigurationListEmpty",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.Warning,
                        (int) EComponentLevels.Application,
                        WebSiteConfiguration.LastException);
                }
                    break;
                case WebSiteConfiguration.EWebSiteErrorCode.ConfigurationXmlError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/XMLSyntaxFailure_1",
                            SettingsConfiguration.LanguageName)
                        + WebSiteConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/XMLSyntaxFailure_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        WebSiteConfiguration.LastException);
                }
                    break;
                case WebSiteConfiguration.EWebSiteErrorCode.ConfigurationAttributeError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/ConfigurationAttributeError_1",
                            SettingsConfiguration.LanguageName)
                        + WebSiteConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/ConfigurationAttributeError_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        WebSiteConfiguration.LastException);
                }
                    break;
                case WebSiteConfiguration.EWebSiteErrorCode.ConfigurationLoadFailed:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/CouldNotLoadFile_1",
                            SettingsConfiguration.LanguageName)
                        + WebSiteConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/WebSiteConfigurationErrors/CouldNotLoadFile_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        WebSiteConfiguration.LastException);
                }
                    break;
                case WebSiteConfiguration.EWebSiteErrorCode.ConfigurationLoadSuccessful:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/StatusMessages/LoadingWebSiteConfigurationSuccessful",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.Green, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Application);
                }
                    break;
                default:
                {
                    throw (new NotImplementedException());
                }
            }

            #endregion WebSite configuration error code check

            #region Document parsing configuration error code check

            // Check website configuration error code
            switch (DocumentParsingConfiguration.ErrorCode)
            {
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationFileDoesNotExist:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/FileDoesNotExists_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/FileDoesNotExists_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationEmpty:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationListEmpty",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.Warning,
                        (int) EComponentLevels.Application,
                        DocumentParsingConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationBankAttributesError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationBankAttributeError_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationBankAttributeError_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        DocumentParsingConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationBankElementsError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationBankElementsError_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationBankElementsError_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError,
                        (int)EComponentLevels.Application,
                        DocumentParsingConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationDocumentElementsError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationDocumentElementsError_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationDocumentElementsError_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError,
                        (int)EComponentLevels.Application,
                        DocumentParsingConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationDocumentElementAttributeError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationDocumentElementAttributeError_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationDocumentElementAttributeError_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError,
                        (int)EComponentLevels.Application,
                        DocumentParsingConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationIdentifierAttributeError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationIdentifierAttributeError_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/ConfigurationIdentifierAttributeError_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int)EStateLevels.FatalError,
                        (int)EComponentLevels.Application,
                        DocumentParsingConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationXmlError:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/XMLSyntaxFailure_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/XMLSyntaxFailure_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        WebSiteConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationLoadFailed:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/CouldNotLoadFile_1",
                            SettingsConfiguration.LanguageName)
                        + DocumentParsingConfiguration.FileName
                        + LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/DocumentParsingConfigurationErrors/CouldNotLoadFile_2",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.DarkRed, Logger, (int) EStateLevels.FatalError,
                        (int) EComponentLevels.Application,
                        DocumentParsingConfiguration.LastException);
                }
                    break;
                case DocumentParsingConfiguration.EDocumentParsingErrorCode.ConfigurationLoadSuccessful:
                {
                    Helper.AddStatusMessage(rchTxtBoxStateMessage,
                        LanguageConfiguration.Language.GetLanguageTextByXPath(
                            @"/MainForm/StatusMessages/LoadingDocumentParsingConfigurationSuccessful",
                            SettingsConfiguration.LanguageName),
                        LanguageConfiguration.Language, SettingsConfiguration.LanguageName,
                        Color.Green, Logger, (int) EStateLevels.Info, (int) EComponentLevels.Application);
                }
                    break;
                default:
                {
                    throw (new NotImplementedException());
                }
            }

            #endregion Document parsing configuration error code check

            #endregion Check configuration error 

            #region Check if PDF converter is installed

            grpBoxDocumentCapture.Enabled = Helper.PdfParserInstalled();

            #endregion Check if PDF converter is installed
        }

    }
}
