//MIT License
//
//Copyright(c) 2017 - 2021 nessie1980(nessie1980 @gmx.de)
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
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    internal static class Program
    {
        /// <summary>
        /// Main entry point of the application
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                // Check if the necessary DLLs exists
                if (!File.Exists(@"LanguageHandler.dll"))
                    throw new FileNotFoundException(@"LanguageHandler.dll does not exist!");

                if (!File.Exists(@"Logger.dll"))
                    throw new FileNotFoundException(@"Logger.dll does not exist!");

                if (!File.Exists(@"Parser.dll"))
                    throw new FileNotFoundException(@"Parser.dll does not exist!");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Create a controller and pass an instance of your application from the MainForm
                var controller = new Classes.ApplicationController(new FrmMain());

                //Run application
                controller.Run(Environment.GetCommandLineArgs());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK);
            }
        }
    }
}
