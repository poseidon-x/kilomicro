using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.deposit
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
                var deposit = le.deposits.FirstOrDefault(p => p.depositID == id);
                //deposit.depositTypeReference.Load();
                if (deposit != null)
                {
                    //deposit.clientReference.Load();
                    int ctID = int.Parse(cboChargeType.SelectedValue);
                    var dc = new coreLogic.depositCharge
                    {
                        amount = txtAmount.Value.Value,
                        approvedBy = User.Identity.Name,
                        chargeDate = dtInterestDate1.SelectedDate.Value,
                        creationDate = DateTime.Now,
                        depositID = id,
                        memo = txtMemo.Text,
                        chargeTypeID = ctID
                    };
                    deposit.depositCharges.Add(dc);
                    if (txtAmount.Value.Value <= deposit.principalBalance)
                    {
                        deposit.principalBalance -= txtAmount.Value.Value;
                    }
                    else if (txtAmount.Value.Value <= deposit.principalBalance + deposit.interestBalance)
                    {
                        var amt = txtAmount.Value.Value;
                        amt -= deposit.principalBalance;
                        deposit.principalBalance = 0;
                        deposit.interestBalance -= amt; ;
                    }
                    else
                    {
                        HtmlHelper.MessageBox("Not enough balance to deduct suggested charges on this account.",
                            "coreERP: Failed", IconType.deny);
                        return;
                    }
                    var jb = journalextensions.Post("LN",
                        deposit.depositType.accountsPayableAccountID.Value,
                        deposit.depositType.chargesIncomeAccountID.Value,
                        txtAmount.Value.Value,
                        "Charges approved on Deposit Account - " + deposit.client.surName + "," + deposit.client.otherNames,
                        pro.currency_id.Value, dtInterestDate1.SelectedDate.Value, deposit.depositNo, ent, User.Identity.Name,
                            deposit.client.branchID);

                    ent.jnl_batch.Add(jb);

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Charges on Deposit Account Saved Successfully!", ResolveUrl("~/ln/deposit/approveInterest.aspx"), "coreERP©: Successful", IconType.ok);
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

                    var deposits = le.deposits.Where(p => p.client.clientID == id && (p.principalBalance > 1 || p.interestBalance > 0));
                    cboAmount.Items.Add(new RadComboBoxItem("", ""));
                    foreach (var r in deposits)
                    {
                        cboAmount.Items.Add(new RadComboBoxItem(r.amountInvested.ToString("#,##0.#0")
                            + " - " + r.maturityDate.Value.ToString("dd-MMM-yyyy"), r.depositID.ToString()));
                    }
                }
            }
        }
        
    }
}