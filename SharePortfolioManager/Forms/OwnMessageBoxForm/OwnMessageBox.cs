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
//#define DEBUG_OWN_MESSAGE_BOX

using System;
using System.Drawing;
using System.Windows.Forms;
using SharePortfolioManager.Properties;

namespace SharePortfolioManager.OwnMessageBoxForm
{
    /// <summary>
    /// State levels for the logging (e.g. Info)
    /// </summary>
    public enum EOwnMessageBoxInfoType
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        InputFileName = 3
    }

    public partial class OwnMessageBox : Form
    {
        #region Variables

        private readonly string _caption;

        private readonly string _message;

        private readonly string _okButtonText;

        private readonly string _cancelButtonText;

        private readonly EOwnMessageBoxInfoType _boxType;

        private readonly bool _flagInputBox;

        private TextBox _txtBoxInputString;

        #endregion Variables

        #region Properties

        public string InputString { get; internal set; }

        #endregion Properties

        #region Methods

        public OwnMessageBox(string strCaption, string strMessage, string strOk, string strCancel, EOwnMessageBoxInfoType eType,
            bool bInputBox = false)
        {
            InitializeComponent();

            _caption = strCaption;
            _message = strMessage;
            _okButtonText = strOk;
            _cancelButtonText = strCancel;
            _boxType = eType;
            _flagInputBox = bInputBox;
        }

        private void OnBtnOk_Click(object sender, EventArgs e)
        {
            // Set input string to the public property
            if (_flagInputBox)
                InputString = _txtBoxInputString.Text;

            // Check if an input string is given if an input string must be given
            if ((!_flagInputBox || _txtBoxInputString.Text == "") && _flagInputBox)
            {
                _txtBoxInputString.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OwnMessageBox_Load(object sender, EventArgs e)
        {
            Text = _caption;
            btnOk.Text = _okButtonText;
            btnCancel.Text = _cancelButtonText;

            switch (_boxType)
            {
                case EOwnMessageBoxInfoType.Info:
                    Icon = Resources.info;
                    break;
                case EOwnMessageBoxInfoType.Warning:
                    Icon = Resources.warning;
                    break;
                case EOwnMessageBoxInfoType.Error:
                    Icon = Resources.error;
                    break;
                case EOwnMessageBoxInfoType.InputFileName:
                    Icon = Resources.input;
                    break;
                default:
                    Icon = Resources.info;
                    break;
            }

            if (_flagInputBox)
            {
                _txtBoxInputString = new TextBox
                {
                    Multiline = false,
                    Name = "InputBox",
                    TabIndex = 0,
                    TabStop = true
                };
            }
        }

        private void OwnMessageBox_Paint(object sender, PaintEventArgs e)
        {
            // Start point of the message
            var startPoint = new Point(10, 10);

            // Set TextFormatFlags to no padding so strings are drawn together.
            const TextFormatFlags flags = TextFormatFlags.WordBreak;

            // Max size of the message text
            var proposedSize = new Size(Width - 20, int.MaxValue);

            // Measure the size which is necessary for the message
            var size = TextRenderer.MeasureText(e.Graphics, _message, Font, proposedSize, flags);

            // Rectangle for text drawing 
            var rect = new Rectangle(startPoint, size);

            // Draw text in the rectangle
            TextRenderer.DrawText(e.Graphics, _message, Font, rect, Color.Black, flags);

            // Check if a input box should be shown
            if (_flagInputBox)
            {
                _txtBoxInputString.Location = new Point(rect.Location.X, rect.Location.Y + rect.Height + 10);

                Size = new Size(Width, size.Height + btnOk.Height + 75 + 33);

                _txtBoxInputString.Size = new Size(Width - 40, 23);
            }
            else
                Size = new Size(Width, size.Height + btnOk.Height + 75);

            // Resize the form
        }

        private void OwnMessageBox_Shown(object sender, EventArgs e)
        {
            // Add text box
            Controls.Add(_txtBoxInputString);

            // Set focus to text box
            if (_flagInputBox)
                _txtBoxInputString.Focus();
        }

        #endregion Methods
    }
}
