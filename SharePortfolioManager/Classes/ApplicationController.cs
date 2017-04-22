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

using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;

namespace SharePortfolioManager.Classes
{
    public class ApplicationController : WindowsFormsApplicationBase
    {
        #region Variables

        private Form mainForm;

        #endregion Variables

        #region Methods

        /// <summary>
        /// This is the constructor
        /// </summary>
        /// <param name="form">Reference of the MainForm</param>
        public ApplicationController(Form form)
        {
            // We keep a reference to the MainForm 
            // to run and also use it when we need to bring to the front
            mainForm = form;
            this.IsSingleInstance = true;
            this.StartupNextInstance += ThisStartupNextInstance;
        }

        /// <summary>
        /// This event handler is called when a new MainForm instance is created
        /// </summary>
        /// <param name="sender">ApplicationController</param>
        /// <param name="e">StartupNextInstanceEventArgs </param>
        void ThisStartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            // Check if the MainFrom is minimized so restore the window
            if (mainForm.WindowState == FormWindowState.Minimized)
            {
                //Here we bring application to the front
                e.BringToForeground = true;
                mainForm.ShowInTaskbar = true;
                mainForm.Show();
                mainForm.WindowState = FormWindowState.Normal;
            }
        }

        protected override void OnCreateMainForm()
        {
            this.MainForm = mainForm;
        }

        #endregion Methods
    }
}