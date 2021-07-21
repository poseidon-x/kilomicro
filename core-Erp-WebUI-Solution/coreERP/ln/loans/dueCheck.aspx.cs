using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.loans
{
    public partial class dueCheck : System.Web.UI.Page
    { 
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                cboCheckType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.checkTypes)
                {
                    cboCheckType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.checkTypeName,
                        r.checkTypeID.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    var id = int.Parse(Request.Params["id"]);
                    var lc = le.loanChecks.FirstOrDefault(p => p.loanCheckID == id);
                    if (lc != null)
                    {
                        //lc.clientReference.Load();
                        //lc.client.loans.Load();

                        cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                        foreach (var r in lc.client.loans)
                        {
                            if (r.balance > 1)
                            {
                                cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanNo + " - " +
                                r.disbursementDate.Value.ToString("dd-MMM-yyyy") + " - " + r.amountDisbursed.ToString("#,###.#0"),
                                    r.loanID.ToString()));
                            }
                        }
                        if (lc.loanID != null)
                        {
                            cboLoan.SelectedValue = lc.loanID.ToString();
                        }
                        lblCheckNumber.Text = lc.checkNumber;
                        //lblClientID.Text = lc.loan.client.accountNumber;
                        //lblClientName.Text = lc.loan.client.surName + ", " + lc.loan.client.otherNames;
                        cboBank.SelectedValue = lc.bankID.ToString();
                        dtDate.SelectedDate = lc.checkDate;
                        if (lc.checkTypeID != null) cboCheckType.SelectedValue = lc.checkTypeID.ToString();
                    }
                }
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (Request.Params["id"] != null && cboLoan.SelectedValue!="")
            {
                
                    var id = int.Parse(Request.Params["id"]);
                    var loanID = int.Parse(cboLoan.SelectedValue);
                    var lc = le.loanChecks.FirstOrDefault(p => p.loanCheckID == id);
                    var ln = le.loans.FirstOrDefault(p => p.loanID == loanID);
                    if (lc != null && ln != null)
                    {
                        //ln.loanTypeReference.Load();
                        //ln.clientReference.Load();
                        lc.bankID = int.Parse(cboBank.SelectedValue);
                        lc.cashDate = dtDate.SelectedDate.Value;
                        lc.loanID = loanID;
                        rpmtMgr.ApplyCheck(le, id, User.Identity.Name, lc.bankID.Value, lc.cashDate.Value, ln);
                        HtmlHelper.MessageBox2("Check Applied Successfully!", ResolveUrl("~/ln/loans/dueChecks.aspx"), "coreERP©: Successful", IconType.ok);
                    }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            if (Request.Params["id"] != null)
            {
                    var id = int.Parse(Request.Params["id"]);
                    var lc = le.loanChecks.FirstOrDefault(p => p.loanCheckID == id);
                    if (lc != null)
                    {
                        le.loanChecks.Remove(lc);
                        le.SaveChanges();
                        HtmlHelper.MessageBox2("Check Cancelled Successfully!", ResolveUrl("~/ln/loans/dueChecks.aspx"), "coreERP©: Successful", IconType.ok);
                    }
            }
        }
    }
}