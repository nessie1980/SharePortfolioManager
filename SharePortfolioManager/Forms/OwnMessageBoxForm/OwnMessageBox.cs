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

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class OwnMessageBox : Form
    {
        #region Variables

        string _caption;

        string _message;

        string _okButtonText;

        string _cancelButtonText;

        bool _flagInputBox;

        TextBox _txtBoxInputString;

        string _strInputString;

        #endregion Variables

        #region Properties

        public string InputString
        {
            get { return _strInputString; }
            internal set { _strInputString = value; }
        }

        #endregion Properties

        #region Methods

        public OwnMessageBox(string strCaption, string strMessage, string strOk, string strCancel, bool bInputBox = false )
        {
            InitializeComponent();

            _caption = strCaption;
            _message = strMessage;
            _okButtonText = strOk;
            _cancelButtonText = strCancel;
            _flagInputBox = bInputBox;
        }

        private void OnBtnOk_Click(object sender, EventArgs e)
        {
            // Set input string to the public property
            if (_flagInputBox)
                InputString = _txtBoxInputString.Text;

            // Check if an input string is given if an input string must be given
            if ((_flagInputBox == true && _txtBoxInputString.Text != "")
                ||
                _flagInputBox == false
                )
            {
                DialogResult = DialogResult.OK;
                Close();
            }
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

            if (_flagInputBox)
            {
                _txtBoxInputString = new TextBox()
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
            int iStartButton = 0;
            // Start point of the message
            Point startPoint = new Point(10, 10);

            // Set TextFormatFlags to no padding so strings are drawn together.
            TextFormatFlags flags = TextFormatFlags.WordBreak;

            // Max size of the message text
            Size proposedSize = new Size(this.Width - 20, int.MaxValue);

            // Measure the size which is necessary for the message
            Size size = TextRenderer.MeasureText(e.Graphics, _message, this.Font, proposedSize, flags);

            // Rectangle for text drawing 
            Rectangle rect = new Rectangle(startPoint, size);

            // Draw text in the rectangle
            TextRenderer.DrawText(e.Graphics, _message, this.Font, rect, Color.Black, flags);

            // Check if a input box should be shown
            if (_flagInputBox)
            {
                _txtBoxInputString.Location = new Point(rect.Location.X, rect.Location.Y + rect.Height + 10);

                this.Size = new Size(this.Width, size.Height + btnOk.Height + 75 + 33);

                _txtBoxInputString.Size = new Size(this.Width - 40, 23);
            }
            else
                this.Size = new Size(this.Width, size.Height + btnOk.Height + 75);

            this.Controls.Add(_txtBoxInputString);

            // Resize the form
            iStartButton = btnOk.Location.Y;
        }

        private void OwnMessageBox_Shown(object sender, EventArgs e)
        {
            if (_flagInputBox)
                _txtBoxInputString.Focus();
        }

        #endregion Methods
    }
}
