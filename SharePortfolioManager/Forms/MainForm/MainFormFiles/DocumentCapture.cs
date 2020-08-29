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
//#define DEBUG_MAIN_FORM_DOCUMENT_CAPTURE

using SharePortfolioManager.Classes;
using SharePortfolioManager.OwnMessageBoxForm;
using System;
using System.IO;
using System.Windows.Forms;
using SharePortfolioManager.DocumentCaptureParsing;

namespace SharePortfolioManager
{
    public partial class FrmMain
    {
        /// <summary>
        /// This function shows the Drop sign
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// This function allows to sets via Drag and Drop a document for this buy
        /// </summary>
        /// <param name="sender">Text box</param>
        /// <param name="e">EventArgs</param>
        private void OnTxtBoxDocument_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length <= 0 || files.Length > 1) return;

                txtBoxDocumentCapture.Text = files[0];

                // Check if the document is a PDF
                var extenstion = Path.GetExtension(txtBoxDocumentCapture.Text);

                if (string.Compare(extenstion, ".PDF", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var frmDocumentCapture = new FrmDocumentCaptureParsing(this, txtBoxDocumentCapture.Text, Logger, Language, LanguageName);
                    frmDocumentCapture.ShowDialog();

                    txtBoxDocumentCapture.Text = @"";
                }
                else
                {
                    var strCaption = Language.GetLanguageTextListByXPath(@"/MessageBoxForm/Captions/*", LanguageName)[
                        (int)EOwnMessageBoxInfoType.Info];
                    var strMessage = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Content/DocumentCaptureOnlyPdf",
                        LanguageName);
                    var strOk = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Ok",
                        LanguageName);
                    var strCancel = Language.GetLanguageTextByXPath(@"/MessageBoxForm/Buttons/Cancel",
                        LanguageName);

                    // Show message
                    var msg = new OwnMessageBox(strCaption, strMessage, strOk, strCancel);
                    msg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Helper.ShowExceptionMessage(ex);
            }
        }
    }
}