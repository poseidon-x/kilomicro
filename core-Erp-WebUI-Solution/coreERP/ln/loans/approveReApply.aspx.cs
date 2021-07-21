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
    public partial class approveReApply : System.Web.UI.Page
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
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch jb = null;
            var pro = ent.comp_prof.FirstOrDefault();
            var rent = new coreReports.reportEntities();
            IRepaymentsManager rpmtMgr = new RepaymentsManager();

            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox;
                var chkSelected = item.FindControl("chkSelected") as CheckBox; 
                if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked==true && 
                    cboLoan.SelectedValue!="")
                {
                    int id = int.Parse(lblID.Text);
                    int loanID = int.Parse(cboLoan.SelectedValue);
                    var ln = le.loans.FirstOrDefault(p => p.loanID == loanID);
                    var ln2 = le.loans.FirstOrDefault(p => p.loanID == id);
                    var pen = (
                        from s in rent.vwLoanStatements
                        group s by new
                        {
                            s.clientID,
                            s.agentName,
                            s.clientName,
                            s.loanNo,
                            s.loanID,
                            s.accountNumber
                        }
                        into grp
                        where grp.Key.loanID == id
                        select new
                        {
                            grp.Key.clientID,
                            grp.Key.clientName,
                            grp.Key.agentName,
                            grp.Key.loanNo,
                            grp.Key.loanID,
                            grp.Key.accountNumber,
                            Balance = grp.Sum(p => p.Dr - p.Cr)
                        }
                ).Where(p => p.Balance <= 10).FirstOrDefault();
                    if (pen != null && pen.Balance<=txtProposedAmount.Value)
                    {
                        rpmtMgr.ApplyNegativeBalanceToLoan(ln, le, ent, txtProposedAmount.Value.Value, 
                            dtDate.SelectedDate.Value,
                            User.Identity.Name, ln2);
                    }
                }
            }
             
            le.SaveChanges();
            ent.SaveChanges();
            HtmlHelper.MessageBox2("Negative Balance applied successfully!", ResolveUrl("/ln/loans/approveReApply.aspx"), "coreERP©: Successful", IconType.ok);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }


        private void OnChange()
        {
            int? id = null;
            cboLoan.Items.Clear();
            if (cboClient.SelectedValue != "")
            {
                id = int.Parse(cboClient.SelectedValue);

                cboLoan.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.loans.Where(p => p.clientID == id).OrderBy(p => p.disbursementDate))
                {
                    cboLoan.Items.Add(new RadComboBoxItem(cl.loanNo + " (" + cl.amountDisbursed.ToString("#,###.#0"),
                        cl.loanID.ToString()));
                }
            }

            var rent = new coreReports.reportEntities();
            var src = (
                    from s in rent.vwLoanStatements
                    group s by new
                    {
                        s.clientID,
                        s.agentName,
                        s.clientName,
                        s.loanNo,
                        s.loanID,
                        s.accountNumber
                    }
                        into grp
                        where grp.Key.clientID == id
                        select new
                        {
                            grp.Key.clientID,
                            grp.Key.clientName,
                            grp.Key.agentName,
                            grp.Key.loanNo,
                            grp.Key.loanID,
                            grp.Key.accountNumber,
                            Balance = grp.Sum(p => p.Dr - p.Cr)
                        }
                ).Where(p => p.Balance <= -10).ToList();
            rpPenalty.DataSource = src;
            rpPenalty.DataBind();
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

    }
}