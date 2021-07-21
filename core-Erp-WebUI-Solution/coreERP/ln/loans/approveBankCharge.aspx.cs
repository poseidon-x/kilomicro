using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class approveBankCharge : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            { 
                dtDate.SelectedDate = DateTime.Now.Date;
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (txtAmount.Value != null && txtAmount.Value > 0
                && dtDate.SelectedDate != null && cboLoan.SelectedValue != "")
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                le = new coreLoansEntities();
                coreLogic.jnl_batch jb = null;
                var pro = ent.comp_prof.FirstOrDefault();

                var loanId = int.Parse(cboLoan.SelectedValue);
                var loan = le.loans.First(p => p.loanID == loanId);
                var pen = new loanPenalty
                {
                    proposedAmount = 0,
                    penaltyBalance = txtAmount.Value.Value,
                    penaltyDate = dtDate.SelectedDate.Value,
                    creation_date = DateTime.Now,
                    creator = User.Identity.Name, 
                    loanID = loan.loanID,
                    penaltyFee = txtAmount.Value.Value,
                    penaltyTypeId = 2
                };
                le.loanPenalties.Add(pen);
                pen.loan = loan;

                jb = journalextensions.Post("LN", loan.loanType.accountsReceivableAccountID,
                    loan.loanType.unearnedInterestAccountID, pen.penaltyFee,
                    "Bank Charges for Returned Cheques for Loan - " + loan.client.surName + "," + loan.client.otherNames,
                    pro.currency_id.Value, pen.penaltyDate, loan.loanNo, ent, User.Identity.Name,
                    loan.client.branchID);
                if (jb != null)
                {
                    ent.jnl_batch.Add(jb);
                }

                using (var tr1 = ent.Database.BeginTransaction())
                {
                    using (var tr2 = le.Database.BeginTransaction())
                    {
                        try
                        {
                            le.SaveChanges();
                            ent.SaveChanges();

                            tr2.Commit();
                            tr1.Commit();
                        }
                        catch (Exception x)
                        {
                            tr2.Rollback();
                            tr1.Rollback();

                            HtmlHelper.MessageBox("Error Applying Charges",
                                "coreERP©: Failed", IconType.deny);
                            return;
                        }
                    }
                }
                HtmlHelper.MessageBox2("Bank Charges applied successfully!",
                    ResolveUrl("/ln/loans/approveBankCharge.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }
         
        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
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

        private void OnChange()
        {
            int? id = null;
            cboLoan.Items.Clear();
            if (cboClient.SelectedValue != "")
            {
                id = int.Parse(cboClient.SelectedValue);

                cboLoan.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.loans
                    .Where(p => p.clientID == id)
                    .OrderBy(p => p.disbursementDate))
                {
                    cboLoan.Items.Add(new RadComboBoxItem(cl.loanNo + " (" + cl.amountDisbursed.ToString("#,###.#0"),
                        cl.loanID.ToString()));
                }
            } 
        }
    }
}