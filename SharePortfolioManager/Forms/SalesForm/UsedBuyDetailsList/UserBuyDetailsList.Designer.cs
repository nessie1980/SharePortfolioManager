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

namespace SharePortfolioManager.SalesForm.UsedBuyDetailsList
{
    partial class UsedBuyDetailsList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tblLayPnlOnwMessageBox = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.rchTxtBoxUsedBuyDetails = new System.Windows.Forms.RichTextBox();
            this.grpBoxUsedBuyDetails = new System.Windows.Forms.GroupBox();
            this.tblLayPnlOnwMessageBox.SuspendLayout();
            this.grpBoxUsedBuyDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblLayPnlOnwMessageBox
            // 
            this.tblLayPnlOnwMessageBox.ColumnCount = 3;
            this.tblLayPnlOnwMessageBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOnwMessageBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlOnwMessageBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tblLayPnlOnwMessageBox.Controls.Add(this.btnOk, 2, 1);
            this.tblLayPnlOnwMessageBox.Controls.Add(this.rchTxtBoxUsedBuyDetails, 0, 0);
            this.tblLayPnlOnwMessageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayPnlOnwMessageBox.Location = new System.Drawing.Point(3, 18);
            this.tblLayPnlOnwMessageBox.Name = "tblLayPnlOnwMessageBox";
            this.tblLayPnlOnwMessageBox.RowCount = 1;
            this.tblLayPnlOnwMessageBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayPnlOnwMessageBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tblLayPnlOnwMessageBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayPnlOnwMessageBox.Size = new System.Drawing.Size(598, 195);
            this.tblLayPnlOnwMessageBox.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(418, 162);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(180, 33);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "_Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // rchTxtBoxUsedBuyDetails
            // 
            this.rchTxtBoxUsedBuyDetails.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tblLayPnlOnwMessageBox.SetColumnSpan(this.rchTxtBoxUsedBuyDetails, 3);
            this.rchTxtBoxUsedBuyDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rchTxtBoxUsedBuyDetails.Location = new System.Drawing.Point(3, 3);
            this.rchTxtBoxUsedBuyDetails.Name = "rchTxtBoxUsedBuyDetails";
            this.rchTxtBoxUsedBuyDetails.ReadOnly = true;
            this.rchTxtBoxUsedBuyDetails.Size = new System.Drawing.Size(592, 156);
            this.rchTxtBoxUsedBuyDetails.TabIndex = 5;
            this.rchTxtBoxUsedBuyDetails.Text = "";
            // 
            // grpBoxUsedBuyDetails
            // 
            this.grpBoxUsedBuyDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxUsedBuyDetails.Controls.Add(this.tblLayPnlOnwMessageBox);
            this.grpBoxUsedBuyDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpBoxUsedBuyDetails.Location = new System.Drawing.Point(5, 5);
            this.grpBoxUsedBuyDetails.Name = "grpBoxUsedBuyDetails";
            this.grpBoxUsedBuyDetails.Size = new System.Drawing.Size(604, 216);
            this.grpBoxUsedBuyDetails.TabIndex = 5;
            this.grpBoxUsedBuyDetails.TabStop = false;
            this.grpBoxUsedBuyDetails.Text = "_UsedBuyDetails";
            // 
            // UsedBuyDetailsList
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(614, 224);
            this.Controls.Add(this.grpBoxUsedBuyDetails);
            this.Font = new System.Drawing.Font("Consolas", 9F);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(630, 263);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(630, 263);
            this.Name = "UsedBuyDetailsList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_UsedBuyDetailsListCaption";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.UsedBuyDetailsList_Shown);
            this.tblLayPnlOnwMessageBox.ResumeLayout(false);
            this.grpBoxUsedBuyDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tblLayPnlOnwMessageBox;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.RichTextBox rchTxtBoxUsedBuyDetails;
        private System.Windows.Forms.GroupBox grpBoxUsedBuyDetails;
    }
}