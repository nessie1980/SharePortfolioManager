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

using WebParser;

namespace SharePortfolioManager
{
    public class WebSiteRegex
    {
        #region Variables

        /// <summary>
        /// Stores the website link of the share
        /// </summary>
        private string _webSiteName;

        /// <summary>
        /// Stores the encoding of the website content
        /// </summary>
        private string _webSiteEncodingType;

        /// <summary>
        /// Stores the RegEx list for the website
        /// </summary>
        private RegExList _webSiteRegexList;

        #endregion Variables

        #region Properties

        public string WebSiteName
        {
            get { return _webSiteName; }
            set { _webSiteName = value; }
        }

        public string WebSiteEncodingType
        {
            get { return _webSiteEncodingType; }
            set { _webSiteEncodingType = value; }
        }

        public RegExList WebSiteRegexList
        {
            get { return _webSiteRegexList; }
            set { _webSiteRegexList = value; }
        }

        #endregion Properties

        #region Methods

        public WebSiteRegex()
        { }

        public WebSiteRegex(string webSiteName, string webSiteEncodingType, RegExList webSiteRegexList)
        {
            _webSiteName = webSiteName;
            _webSiteEncodingType = webSiteEncodingType;
            WebSiteRegexList = webSiteRegexList;
        }

        #endregion Methods
    }
}
