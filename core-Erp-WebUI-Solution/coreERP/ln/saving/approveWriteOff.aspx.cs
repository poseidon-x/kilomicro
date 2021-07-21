using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.saving
{
    public partial class approveWriteOff : System.Web.UI.Page
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
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            if (txtAmount.Value != null && dtInterestDate1.SelectedDate != null && cboAmount.SelectedValue != "")
            {

                int id = int.Parse(cboAmount.SelectedValue);
                var saving = le.savings.FirstOrDefault(p => p.savingID == id);
                if (saving != null)
                {
                    //saving.clientReference.Load();
                    //saving.savingTypeReference.Load();
                    if (txtAmount.Value.Value <= saving.principalBalance + saving.interestBalance)
                    {
                        var amt = txtAmount.Value.Value;
                        var amt2 = (txtAmount.Value.Value <= saving.interestBalance) ? txtAmount.Value.Value : saving.interestBalance;
                        amt -= amt2;
                        saving.interestAccumulated -= amt2;
                        saving.interestBalance -= amt2;
                        saving.interestExpected -= amt2;
                        saving.amountInvested -= amt2;
                        saving.principalBalance -= amt; 
                        saving.amountInvested -= amt;
                    }                    
                    else if (txtAmount.Value.Value <= saving.principalBalance)
                    {
                        saving.principalBalance -= txtAmount.Value.Value;
                        saving.amountInvested -= txtAmount.Value.Value;
                    } 
                    else
                    {
                        HtmlHelper.MessageBox("Not enough balance to write off suggested amount on this account.",
                            "coreERP: Failed", IconType.deny);
                        return;
                    }
                    var jb = journalextensions.Post("LN",
                        saving.savingType.accountsPayableAccountID.Value,
                        saving.savingType.interestExpenseAccountID.Value,
                        txtAmount.Value.Value,
                        "Amount written off on Savings Account - " + saving.client.surName + "," + saving.client.otherNames,
                        pro.currency_id.Value, dtInterestDate1.SelectedDate.Value, saving.savingNo, ent, User.Identity.Name,
                            saving.client.branchID);

                    ent.jnl_batch.Add(jb);

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Write-off on Savings Account Saved Successfully!", ResolveUrl("~/ln/saving/approveInterest.aspx"), "coreERP©: Successful", IconType.ok);
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

                    var savings = le.savings.Where(p => p.client.clientID == id && (p.principalBalance > 1 || p.interestBalance > 0));
                    cboAmount.Items.Add(new RadComboBoxItem("", ""));
                    foreach (var r in savings)
                    {
                        cboAmount.Items.Add(new RadComboBoxItem(r.amountInvested.ToString("#,##0.#0")
                            + " - " + r.maturityDate.Value.ToString("dd-MMM-yyyy"), r.savingID.ToString()));
                    }
                }
            }
        }
        
    }
}