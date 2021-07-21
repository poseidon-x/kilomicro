using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.saving
{
    public partial class withCalc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();

            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                Clear();
                cboSavings.Items.Clear();
                int clientID = int.Parse(cboClient.SelectedValue);
                coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
                var client = le.clients
                            .First(p => p.clientID == clientID);
                foreach (var sav in client.savings.OrderBy(p => p.maturityDate))
                {
                    cboSavings.Items.Add(new RadComboBoxItem(sav.savingNo + " | " + sav.maturityDate.Value.ToString("dd-MMM-yyyy")
                         + " | " + (sav.principalBalance + sav.interestBalance).ToString("#,##0.#0"),
                        sav.savingID.ToString()));
                }
            }
        }

        protected void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSavings.SelectedValue != "" && txtTakeHomeAmount.Value.Value > 0)
                {
                    var savId = int.Parse(cboSavings.SelectedValue);
                    coreLogic.IChargesHelper chMgr = new coreLogic.ChargesHelper();
                    coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
                    var calc = chMgr.ComputeCharges(savId, txtTakeHomeAmount.Value.Value,DateTime.Today);
                    var sav = le.savings.First(p => p.savingID == savId);

                    txtAvailIntBal.Value = sav.availableInterestBalance;
                    txtAvailPrincBal.Value = sav.availablePrincipalBalance;
                    txtCurPrincBal.Value = sav.principalBalance;
                    txtGrossWith.Value = calc.grossWithdrawalWamount;
                    txtIntWIth.Value = calc.interestWithdrawal;
                    txtNetWith.Value = calc.netWithdrawalAmount;
                    txtPrincWith.Value = calc.principalWithdrawal;
                    txtTotalCharges.Value = calc.totalCharges;
                    dtpMatDate.SelectedDate = sav.maturityDate;

                }
            }
            catch(ApplicationException ax)//Validation Error
            {
                HtmlHelper.MessageBox(ax.Message);
            }
        }

        private void Clear()
        {

            txtAvailIntBal.Value = null;
            txtAvailPrincBal.Value = null; 
            txtCurPrincBal.Value = null;
            txtGrossWith.Value = null; 
            txtIntWIth.Value = null;
            txtNetWith.Value = null; 
            txtPrincWith.Value = null;
            txtTotalCharges.Value = null;
            dtpMatDate.SelectedDate = null;

        }

        protected void btnEmptyAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSavings.SelectedValue != "")
                {
                    var savId = int.Parse(cboSavings.SelectedValue);
                    coreLogic.IChargesHelper chMgr = new coreLogic.ChargesHelper();
                    coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
                    var calc = chMgr.EmptyAccount(savId);
                    var sav = le.savings.First(p => p.savingID == savId);

                    txtAvailIntBal.Value = sav.availableInterestBalance;
                    txtAvailPrincBal.Value = sav.availablePrincipalBalance;
                    txtCurPrincBal.Value = sav.principalBalance;
                    txtGrossWith.Value = calc.grossWithdrawalWamount;
                    txtIntWIth.Value = calc.interestWithdrawal;
                    txtNetWith.Value = calc.netWithdrawalAmount;
                    txtPrincWith.Value = calc.principalWithdrawal;
                    txtTotalCharges.Value = calc.totalCharges;
                    dtpMatDate.SelectedDate = sav.maturityDate;

                }
            }
            catch (ApplicationException ax)//Validation Error
            {
                HtmlHelper.MessageBox(ax.Message);
            }
        }
    }
}