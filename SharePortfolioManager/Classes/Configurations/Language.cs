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
//#define DEBUG_MAIN_FORM_LANGUAGE

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using LanguageHandler;
using SharePortfolioManager.Classes.ShareObjects;

#if DEBUG_LANGUAGE
using SharePortfolioManager.InvalidLanguageKeysForm;
#endif

namespace SharePortfolioManager.Classes.Configurations
{
    public static class LanguageConfiguration
    {
        #region Error codes

        // Error codes of the WebSiteConfiguration class
        public enum ELanguageErrorCode
        {
            ConfigurationLoadSuccessful = 0,
            FileDoesNotExit = -1,
            ConfigurationXmlError = -2,
            ConfigurationLoadFailed = -3
        };

        #endregion Error codes

        #region Properties

        /// <summary>
        /// Flag if the configuration load was successful
        /// </summary>
        public static bool InitFlag { internal set; get; }

        /// <summary>
        /// Error code of the language configuration load
        /// </summary>
        public static ELanguageErrorCode ErrorCode { internal set; get; }

        /// <summary>
        /// Last exception of the language configuration load
        /// </summary>
        public static Exception LastException { internal set; get; }

        /// <summary>
        /// XML file with the language configuration
        /// </summary>
        public const string FileName = @"Settings\Language.xml";

        /// <summary>
        /// Language configuration instance
        /// </summary>
        public static Language Language;

        public static List<string> LoggerStateList { internal set; get; }

        public static List<string> LoggerComponentNamesList { internal set; get; }

        #endregion Properties

        #region Load language

        /// <summary>
        /// This function loads the language and sets the language values
        /// to the controls (e.g. labels, buttons and so on).
        /// 
        /// REMARK: in DEBUG mode it is check if all language keys are set in the Language.XML file
        /// </summary>
        public static bool LoadLanguage()
        {
            try
            {
                // Check if the language configuration file exists
                if (!File.Exists(FileName))
                {
                    ErrorCode = ELanguageErrorCode.FileDoesNotExit;

                    return false;
                }

                // Load language XML file
                Language = new Language(FileName);

                // Check if the language file has been loaded
                if (Language.InitFlag)
                {
                    // ONLY in DEBUG mode
                    // Check if an language key is not defined in the Language.XML file and then create a
                    // a dialog with the undefined language keys
#if DEBUG_LANGUAGE
                    var strProjectPath =
                        Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
                    Language.CheckLanguageKeysOfProject(strProjectPath);
                    Language.CheckLanguageKeysOfXml(strProjectPath);

                    if (Language.InvalidLanguageKeysOfProject.Count != 0 ||
                        Language.InvalidLanguageKeysOfXml.Count != 0)
                    {
                        var strInvalidKeys = @"";

                        if (Language.InvalidLanguageKeysOfProject.Count != 0)
                        {
                            var counter = 0;

                            strInvalidKeys =
                                Language.InvalidLanguageKeysOfProject.Count +
                                " invalid language keys in the project files.\n";

                            foreach (var invalidKeyProject in Language.InvalidLanguageKeysOfProject)
                            {
                                if (counter < Language.InvalidLanguageKeysOfProject.Count -1)
                                {
                                    strInvalidKeys += invalidKeyProject + Environment.NewLine;
                                }
                                else
                                {
                                    strInvalidKeys += invalidKeyProject;
                                }

                                counter++;
                            }

                            if (Language.InvalidLanguageKeysOfXml.Count != 0)
                            {
                                strInvalidKeys += Environment.NewLine;
                                strInvalidKeys += Environment.NewLine;
                            }
                        }

                        if (Language.InvalidLanguageKeysOfXml.Count != 0)
                        {
                            var counter = 0;

                            strInvalidKeys +=
                                Language.InvalidLanguageKeysOfXml.Count + " unused XML language keys in file \"" +
                                FileName + Environment.NewLine;
                            foreach (var invalidKeyXml in Language.InvalidLanguageKeysOfXml)
                            {
                                if (counter < Language.InvalidLanguageKeysOfXml.Count -1)
                                {
                                    strInvalidKeys += invalidKeyXml + Environment.NewLine;
                                }
                                else
                                {
                                    strInvalidKeys += invalidKeyXml;
                                }

                                counter++;
                            }
                        }

                        var invalidLanguageKeysDlg = new FrmInvalidLanguageKeys();
                        invalidLanguageKeysDlg.Text += @" - (Project path: " + strProjectPath + @")";
                        invalidLanguageKeysDlg.SetText(strInvalidKeys);
                        invalidLanguageKeysDlg.ShowDialog();
                    }
#endif

                    #region Load logger language

                    // Add state names
                    if (LoggerStateList == null)
                    {
                        LoggerStateList = new List<string>();
                    }
                    else
                    {
                        LoggerStateList.Clear();
                    }

                    LoggerStateList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Start", SettingsConfiguration.LanguageName));
                    LoggerStateList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Info", SettingsConfiguration.LanguageName));
                    LoggerStateList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Warning", SettingsConfiguration.LanguageName));
                    LoggerStateList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/Error", SettingsConfiguration.LanguageName));
                    LoggerStateList.Add(Language.GetLanguageTextByXPath(@"/Logger/States/FatalError", SettingsConfiguration.LanguageName));

                    // Add component names
                    if (LoggerComponentNamesList == null)
                    {
                        LoggerComponentNamesList = new List<string>();
                    }
                    else
                    {
                        LoggerComponentNamesList.Clear();
                    }

                    LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Application",
                        SettingsConfiguration.LanguageName));
                    LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Parser",
                        SettingsConfiguration.LanguageName));
                    LoggerComponentNamesList.Add(
                        Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/LanguageHandler", SettingsConfiguration.LanguageName));
                    LoggerComponentNamesList.Add(Language.GetLanguageTextByXPath(@"/Logger/ComponentNames/Logger",
                        SettingsConfiguration.LanguageName));

                    #endregion Load logger language

                    #region Set share object unit and percentage unit

                    ShareObject.PercentageUnit = Language.GetLanguageTextByXPath(@"/PercentageUnit", SettingsConfiguration.LanguageName);
                    ShareObject.PieceUnit = Language.GetLanguageTextByXPath(@"/PieceUnit", SettingsConfiguration.LanguageName);

                    #endregion Set share object unit and percentage unit

                    // Set initialization flag
                    InitFlag = true;

                    ErrorCode = ELanguageErrorCode.ConfigurationLoadSuccessful;

                    return InitFlag;
                }

                // Set initialization flag
                InitFlag = false;

                ErrorCode = ELanguageErrorCode.ConfigurationLoadFailed;

                return InitFlag;
            }
            catch (XmlException ex)
            {
                LastException = ex;

                ErrorCode = ELanguageErrorCode.ConfigurationXmlError;

                // Set initialization flag
                InitFlag = false;

                return InitFlag;
            }

            catch (Exception ex)
            {
                LastException = ex;

                ErrorCode = ELanguageErrorCode.ConfigurationLoadFailed;

                // Set initialization flag
                InitFlag = false;

                return InitFlag;
            }
        }

        #endregion Load language
    }
}