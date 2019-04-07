﻿//MIT License
//
//Copyright(c) 2019 nessie1980(nessie1980 @gmx.de)
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

using System;
using Parser;

namespace SharePortfolioManager.Classes.ParserRegex
{
    [Serializable]
    public class DocumentRegex
    {
        #region Properties

        public string DocumentType { get; internal set; }

        public string TypeIdentifier { get; internal set; }

        public string DocumentEncodingType { get; internal set; }

        public RegExList DocumentRegexList { get; internal set; }

        #endregion Properties

        #region Methods

        public DocumentRegex()
        { }

        public DocumentRegex(string documentType, string typeIdentifier, string documentEncodingType, RegExList documentRegexList)
        {
            DocumentType = documentType;
            TypeIdentifier = typeIdentifier;
            DocumentEncodingType = documentEncodingType;
            DocumentRegexList = documentRegexList;
        }

        #endregion Methods
    }
}
