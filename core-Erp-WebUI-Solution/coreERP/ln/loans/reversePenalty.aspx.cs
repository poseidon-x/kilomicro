using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreReports;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class reversePenalty : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            
        }

        protected void btnReverse_Click(object sender, EventArgs e)
        {
            if (txtReversalAmount.Value.Value != null && txtReversalAmount.Value.Value > 0&&
                cboPenalty.SelectedValue != "")
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                coreLogic.jnl_batch jb = null;
                var pro = ent.comp_prof.FirstOrDefault();

                var clientId = int.Parse(cboClient.SelectedValue);
                var loanId = int.Parse(cboLoan.SelectedValue);
                var penaltyId = int.Parse(cboPenalty.SelectedValue);
                var amount = txtReversalAmount.Value.Value;

                if (clientId > 0 && loanId > 0 && penaltyId > 0 && amount > 0)
                {
                    var loan = le.loans.FirstOrDefault(p => p.loanID == loanId);
                    if (loan != null && loan.closed == false)
                    {
                        if (pro.comp_name.ToLower().Contains("eclipse") || pro.comp_name.ToLower().Contains("demo"))
                        {
                            var penalty = le.loanPenalties.FirstOrDefault(p => p.loanPenaltyID == penaltyId);
                            if (amount < penalty.penaltyFee)
                            {
                                reversepenaltyAmount(penalty, amount);
                            }
                            else if (amount == penalty.penaltyFee)
                            {
                                le.loanPenalties.Remove(penalty);
                            }
                            else if (amount > penalty.penaltyFee)
                            {
                                HtmlHelper.MessageBox("Amount greater than penalty fee");
                            }

                            jb = journalextensions.Post("LN", loan.loanType.interestIncomeAccountID,
                                loan.loanType.accountsReceivableAccountID, amount,
                                "Penalty Reversal for Loan - " + loan.client.surName + "," + loan.client.otherNames,
                                pro.currency_id.Value, DateTime.Now, loan.loanNo, ent, User.Identity.Name,
                                loan.client.branchID);
                            if (jb != null)
                            {
                                ent.jnl_batch.Add(jb);
                            }

                        }
                    }
                }

                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Penalty Reversed successfully!", ResolveUrl("/ln/loans/reversePenalty.aspx"),
                    "coreERP©: Successful", IconType.ok);
            }
        }


        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cboLoan.Items.Clear();
            if (cboClient.SelectedValue != "")
            {
                coreReports.reportEntities rent = new reportEntities();
                var clientID = int.Parse(cboClient.SelectedValue);
                cboLoan.Items.Clear();
                cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var ln in le.loans.Where(p => p.clientID == clientID && p.closed == false).OrderByDescending(p => p.loanID).ToList())
                {
                    cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem( ln.loanNo + " : " 
                        + ln.amountDisbursed.ToString("#,###.#0"),ln.loanID.ToString()));
                }
            }
        }

        protected void cboLoan_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cboPenalty.Items.Clear();
            if (cboLoan.SelectedValue != "")
            {
                coreReports.reportEntities rent = new reportEntities();
                var loanId = int.Parse(cboLoan.SelectedValue);
                cboPenalty.Items.Clear();
                cboPenalty.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var pn in le.loanPenalties.Where(p => p.loanID == loanId && p.penaltyBalance > 0)
                    .OrderByDescending(p => p.penaltyDate).ToList())
                {
                    cboPenalty.Items.Add(new Telerik.Web.UI.RadComboBoxItem(
                        pn.penaltyDate.ToString("dd-MMM-yyyy")
                        + " : " + pn.penaltyFee.ToString("#,###.#0"), pn.loanPenaltyID.ToString()));
                }
            }
        }

        protected void cboPenalty_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            txtReversalAmount.Value = null;
            if (cboPenalty.SelectedValue != "")
            {
                coreReports.reportEntities rent = new reportEntities();
                var penaltyId = int.Parse(cboPenalty.SelectedValue);
                var pn = le.loanPenalties.FirstOrDefault(p => p.loanPenaltyID == penaltyId).penaltyFee;
                
                txtReversalAmount.MaxValue = (double)pn;
                
            }
        }

        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 1
                        || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 5).Where(p =>
                     (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void reversepenaltyAmount(loanPenalty pen, double amount)
        {
            pen.penaltyFee -= amount;
            pen.penaltyBalance -= amount;
        }



    }
}