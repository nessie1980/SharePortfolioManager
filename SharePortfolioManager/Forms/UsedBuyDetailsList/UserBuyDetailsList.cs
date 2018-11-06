using System;
using System.Windows.Forms;

namespace SharePortfolioManager
{
    public partial class UsedBuyDetailsList : Form
    {
        #region Variables

        private readonly string _message;

        #endregion Variables

        public UsedBuyDetailsList(string strCaption, string strGrpBoxCaption, string strMessage, string strOk)
        {
            InitializeComponent();

            _message = strMessage;

            Text = strCaption;
            grpBoxUsedBuyDetails.Text = strGrpBoxCaption;
            btnOk.Text = strOk;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void UsedBuyDetailsList_Shown(object sender, EventArgs e)
        {
            rchTxtBoxUsedBuyDetails.Text = _message;
        }
    }
}
