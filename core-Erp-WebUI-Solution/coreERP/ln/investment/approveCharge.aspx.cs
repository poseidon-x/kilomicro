using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.investment
{
    public partial class approveCharge : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 1 || p.clientTypeID==2).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }

                cboChargeType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.chargeTypes.OrderBy(p=> p.chargeTypeName))
                {
                    cboChargeType.Items.Add(new RadComboBoxItem(cl.chargeTypeName, cl.chargeTypeID.ToString()));
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            if (txtAmount.Value != null && dtInterestDate1.SelectedDate != null && cboAmount.SelectedValue != ""
                && cboChargeType.SelectedValue!="")
            {

                int id = int.Parse(cboAmount.SelectedValue);
                var investment = le.investments.FirstOrDefault(p => p.investmentID == id);
                //investment.investmentTypeReference.Load();
                if (investment != null)
                {
                    //investment.clientReference.Load();
                    int ctID = int.Parse(cboChargeType.SelectedValue);
                    var dc = new coreLogic.investmentCharge
                    {
                        amount = txtAmount.Value.Value,
                        approvedBy = User.Identity.Name,
                        chargeDate = dtInterestDate1.SelectedDate.Value,
                        creationDate = DateTime.Now,
                        investmentID = id,
                        memo = txtMemo.Text,
                        chargeTypeID = ctID
                    };
                    if (txtAmount.Value.Value <= investment.principalBalance)
                    {
                        investment.principalBalance -= txtAmount.Value.Value;
                    }
                    else if (txtAmount.Value.Value <= investment.principalBalance + investment.interestBalance)
                    {
                        var amt = txtAmount.Value.Value;
                        amt -= investment.principalBalance;
                        investment.principalBalance = 0;
                        investment.interestBalance -= amt; ;
                    }
                    else
                    {
                        HtmlHelper.MessageBox("Not enough balance to deduct suggested charges on this account.",
                            "coreERP: Failed", IconType.deny);
                        return;
                    }
                    var jb = journalextensions.Post("LN",
                        investment.investmentType.chargesIncomeAccountID.Value,
                        investment.investmentType.accountsPayableAccountID.Value,
                        txtAmount.Value.Value,
                        "Charges approved on Investment Account - " + investment.client.surName + "," + investment.client.otherNames,
                        pro.currency_id.Value, dtInterestDate1.SelectedDate.Value, investment.investmentNo, ent, User.Identity.Name,
                            investment.client.branchID);

                    ent.jnl_batch.Add(jb);

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Charges on Investment Account Saved Successfully!", ResolveUrl("~/ln/investment/approveInterest.aspx"), "coreERP©: Successful", IconType.ok);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void dtInterestDate1_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if (cboClient.SelectedValue != "")
            {
                var id = -1;
                if (cboClient.SelectedValue != "")
                {
                    id = int.Parse(cboClient.SelectedValue);

                    var investments = le.investments.Where(p => p.client.clientID == id && (p.principalBalance > 1 || p.interestBalance > 0));
                    cboAmount.Items.Add(new RadComboBoxItem("", ""));
                    foreach (var r in investments)
                    {
                        cboAmount.Items.Add(new RadComboBoxItem(r.amountInvested.ToString("#,##0.#0")
                            + " - " + r.maturityDate.Value.ToString("dd-MMM-yyyy"), r.investmentID.ToString()));
                    }
                }
            }
        }
        
    }
}