using coreLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class postProvision : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            {
                cboYear.Items.Add(new RadComboBoxItem("", ""));
                for (int i = 0; i < 20; i++ )
                {
                    var year = 2010 + i;
                    if (year <= DateTime.Now.Year)
                    {
                        cboYear.Items.Add(new RadComboBoxItem(year.ToString(), year.ToString()));
                    }
                }

                cboMonth.Items.Add(new RadComboBoxItem("", ""));
                for (int i = 0; i < 12; i++)
                {
                    var month = new DateTime(2010,1,1).AddMonths(i);
                    cboMonth.Items.Add(new RadComboBoxItem(month.ToString("MMMM"), month.ToString("MM")));
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch jb = null;
            var pro = ent.comp_prof.FirstOrDefault();

            var proClses = le.provisionClasses.ToList();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtAmt") as Telerik.Web.UI.RadNumericTextBox;
                var txtDays = item.FindControl("txtDays") as Telerik.Web.UI.RadNumericTextBox;
                var txtSec = item.FindControl("txtSec") as Telerik.Web.UI.RadNumericTextBox;  
                if (lblID != null && txtProposedAmount != null && txtDays != null && txtSec != null)
                {
                    int id = int.Parse(lblID.Text);
                    var inc = le.loanProvisions.FirstOrDefault(p => p.loanProvisionID == id);
                    if (inc != null)
                    { 
                        inc.proposedAmount = txtProposedAmount.Value.Value;
                        inc.provisionAmount = inc.proposedAmount;
                        inc.securityValue = txtSec.Value.Value;
                        inc.daysDue = (int)txtDays.Value.Value;
                        inc.edited = true;

                        coreLogic.provisionClass proCls = null;
                        foreach (var cls in proClses)
                        {
                            if (cls.maxDays >= inc.daysDue && cls.minDays <= inc.daysDue)
                            {
                                proCls = cls;
                                break;
                            }
                        }
                    }
                }
            } 
            le.SaveChanges();
            HtmlHelper.MessageBox2("Loan Provision Saved successfully!", ResolveUrl("/ln/loans/postProvision.aspx"), "coreERP©: Successful", IconType.ok);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }


        protected void btnInit_Click(object sender, EventArgs e)
        { 
            le=new  coreLoansEntities();
            if (cboYear.SelectedValue != "" && cboMonth.SelectedValue != "")
            {
                int year = int.Parse(cboYear.SelectedValue);
                int month = int.Parse(cboMonth.SelectedValue);
                var date = new DateTime(year, month, 1).AddMonths(1).AddSeconds(-1);

                if (date <= DateTime.Now && le.loanProvisions.Any(p => p.provisionDate == date))
                {
                    coreReports.reportEntities rent = new coreReports.reportEntities();
                    var proClses = le.provisionClasses.ToList();
                    var vl2s = rent.GetvwLoans2(date).ToList();
                    foreach (var vl2 in vl2s)
                    {
                        if (vl2.principalBalance + vl2.interestBalance > 1)
                        {
                            coreLogic.provisionClass proCls = null;
                            foreach (var cls in proClses)
                            {
                                if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                                {
                                    proCls = cls;
                                    break;
                                }
                            }
                            if (proCls != null)
                            {
                                var provision = vl2.principalBalance*proCls.provisionPercent/100.0;
                                var inc = le.loanProvisions.FirstOrDefault(p => p.provisionDate == date
                                                                                && p.loanID == vl2.loanID);
                                if (inc == null)
                                {

                                    inc = new coreLogic.loanProvision
                                    {
                                        daysDue = vl2.daysDue,
                                        interestBalance = vl2.interestBalance,
                                        loanID = vl2.loanID,
                                        posted = false,
                                        principalBalance = vl2.principalBalance,
                                        proposedAmount = provision,
                                        provisionAmount = 0,
                                        provisionClassID = proCls.provisionClassID,
                                        provisionDate = date,
                                        securityValue = vl2.collateralValue,
                                        typeOfSecurity = vl2.collateralType,
                                        edited = false
                                    };
                                    le.loanProvisions.Add(inc);
                                }
                                else if (!inc.edited)
                                {
                                    inc.interestBalance = vl2.interestBalance;
                                    inc.principalBalance = vl2.principalBalance;
                                    inc.provisionClassID = proCls.provisionClassID;
                                    inc.securityValue = vl2.collateralValue;
                                    inc.typeOfSecurity = vl2.collateralType;
                                    inc.proposedAmount = provision;
                                    inc.daysDue = vl2.daysDue;
                                }
                            }
                        }
                    }
                    le.SaveChanges();

                }
                else
                {
                    coreReports.reportEntities rent = new coreReports.reportEntities();
                    var proClses = le.provisionClasses.ToList();
                    var vl2s = rent.GetvwLoans2(date).ToList();
                    foreach (var vl2 in vl2s)
                    {
                        if (vl2.principalBalance + vl2.interestBalance > 1)
                        {
                            coreLogic.provisionClass proCls = null;
                            foreach (var cls in proClses)
                            {
                                if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                                {
                                    proCls = cls;
                                    break;
                                }
                            }
                            if (proCls != null )
                            {
                                var provision = vl2.principalBalance * proCls.provisionPercent / 100.0;
                                var inc =le.loanProvisions.FirstOrDefault(p=> p.provisionDate==date 
                                && p.loanID==vl2.loanID
                                        );

                                if (inc == null)
                                {
                                    inc = new coreLogic.loanProvision
                                    {
                                        daysDue = vl2.daysDue,
                                        interestBalance = vl2.interestBalance,
                                        loanID = vl2.loanID,
                                        posted = false,
                                        principalBalance = vl2.principalBalance,
                                        proposedAmount = provision,
                                        provisionAmount = 0,
                                        provisionClassID = proCls.provisionClassID,
                                        provisionDate = date,
                                        securityValue = vl2.collateralValue,
                                        typeOfSecurity = vl2.collateralType,
                                        edited = false
                                    };
                                    le.loanProvisions.Add(inc);
                                }
                                else if (!inc.edited)
                                {
                                    inc.interestBalance = vl2.interestBalance;
                                    inc.proposedAmount = vl2.provisionAmount;
                                    inc.principalBalance = vl2.principalBalance;
                                    inc.daysDue = vl2.daysDue;
                                    inc.provisionClassID = proCls.provisionClassID;
                                    inc.securityValue = vl2.collateralValue;
                                    inc.typeOfSecurity = vl2.collateralType;
                                    inc.posted = false;
                                }
                            }
                        }
                    }
                    le.SaveChanges();
                }
                if (!le.loanProvisions.Any(p => p.provisionDate == date && p.posted == false))
                {
                    btnPost.Enabled = false;
                    btnOK.Enabled = false;
                }
                else
                {
                    btnPost.Enabled = true;
                    btnOK.Enabled = true;
                }
                var incs = le.loanProvisions
                    .Include(p=> p.loan)
                    .Include(p => p.loan.client)
                    .Where(p => p.provisionDate == date && !p.posted ).OrderBy(p => p.daysDue)
                    .ThenBy(p => p.loan.client.surName).ThenBy(p => p.loan.client.otherNames).ThenBy(p => p.loan.loanNo)
                    //.Take(50)
                    .ToList();
                
                rpPenalty.DataSource = incs;
                rpPenalty.DataBind();
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch jb = null;
            var pro = ent.comp_prof.FirstOrDefault();
            List<int> loanIds = new List<int>();

            var proClses = le.provisionClasses.ToList();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtAmt") as Telerik.Web.UI.RadNumericTextBox;
                var txtDays = item.FindControl("txtDays") as Telerik.Web.UI.RadNumericTextBox;
                var txtSec = item.FindControl("txtSec") as Telerik.Web.UI.RadNumericTextBox;
                if (lblID != null && txtProposedAmount != null && txtDays != null && txtSec != null)
                {
                    int id = int.Parse(lblID.Text);
                    var inc = le.loanProvisions.FirstOrDefault(p => p.loanProvisionID == id);
                    loanIds.Add(inc.loanID);
                    if (inc != null && !inc.posted && !inc.reversed)
                    {
                        inc.proposedAmount = txtProposedAmount.Value.Value;
                        inc.provisionAmount = inc.proposedAmount;
                        inc.securityValue = txtSec.Value.Value;
                        inc.daysDue = (int) txtDays.Value.Value;

                        coreLogic.provisionClass proCls = null;
                        foreach (var cls in proClses)
                        {
                            if (cls.maxDays >= inc.daysDue && cls.minDays <= inc.daysDue)
                            {
                                proCls = cls;
                                break;
                            }
                        }

                        var debJnl =
                            ent.jnl.FirstOrDefault(p => p.ref_no == inc.loan.loanNo && p.tx_date == inc.provisionDate
                                                        && p.description.StartsWith("Loan Provisions - ") &&
                                                        p.dbt_amt > 0);
                        var crdJnl =
                            ent.jnl.FirstOrDefault(p => p.ref_no == inc.loan.loanNo && p.tx_date == inc.provisionDate
                                                        && p.description.StartsWith("Loan Provisions - ") &&
                                                        p.crdt_amt > 0);
                        if (debJnl != null && crdJnl != null)
                        {
                            debJnl.dbt_amt = inc.provisionAmount;
                            crdJnl.crdt_amt = inc.provisionAmount;
                        }
                        else
                        {
                            if (jb == null)
                            {
                                jb = journalextensions.Post("LN",
                                    inc.loan.loanType.provisionExpenseAccountID, inc.loan.loanType.provisionsAccountID,
                                    inc.provisionAmount,
                                    "Loan Provisions - " + inc.loan.client.surName + ", " + inc.loan.client.otherNames
                                    + " (" + inc.loan.client.accountNumber + ")",
                                    pro.currency_id.Value, inc.provisionDate, inc.loan.loanNo, ent, User.Identity.Name,
                                    inc.loan.client.branchID);
                            }
                            else
                            {
                                var jb2 = journalextensions.Post("LN",
                                    inc.loan.loanType.provisionExpenseAccountID,
                                    inc.loan.loanType.provisionsAccountID, inc.provisionAmount,
                                    "Loan Provisions - " + inc.loan.client.surName + ", " + inc.loan.client.otherNames
                                    + " (" + inc.loan.client.accountNumber + ")",
                                    pro.currency_id.Value, inc.provisionDate, inc.loan.loanNo, ent, User.Identity.Name,
                                    inc.loan.client.branchID);
                                var list2 = jb2.jnl.ToList();
                                jb.jnl.Add(list2[0]);
                                jb.jnl.Add(list2[1]);
                            }
                        }
                        inc.posted = true;
                    }
                }
            }
            int year = int.Parse(cboYear.SelectedValue);
            int month = int.Parse(cboMonth.SelectedValue);
            var date = new DateTime(year, month, 1).AddMonths(1).AddSeconds(-1);
            date = date.AddMonths(-1);
            date = (new DateTime(date.Year, date.Month, 1)).AddMonths(1).AddSeconds(-1);
            var incs =
                le.loanProvisions.Where(p => p.provisionDate == date && p.posted && !p.reversed)// && loanIds.Contains(p.loanID))
                    //.Take(50)
                    .ToList();

            foreach (var inc in incs)
            {
                var debJnl =
                    ent.jnl.FirstOrDefault(p => p.ref_no == inc.loan.loanNo && p.tx_date == inc.provisionDate
                                                && p.description.StartsWith("Loan Provisions - ") && p.dbt_amt > 0);
                var crdJnl =
                    ent.jnl.FirstOrDefault(p => p.ref_no == inc.loan.loanNo && p.tx_date == inc.provisionDate
                                                &&
                                                p.description.StartsWith(
                                                    "Loan Provisions Reversal for Subsequent Month- ") && p.crdt_amt > 0);
                if (debJnl != null && crdJnl != null)
                {
                    debJnl.dbt_amt = inc.provisionAmount;
                    crdJnl.crdt_amt = inc.provisionAmount;
                }
                else
                {
                    if (jb == null)
                    {
                        jb = journalextensions.Post("LN",
                            inc.loan.loanType.provisionsAccountID, inc.loan.loanType.provisionExpenseAccountID,
                            inc.provisionAmount,
                            "Loan Provisions Reversal for Subsequent Month- " + inc.loan.client.surName + ", " +
                            inc.loan.client.otherNames
                            + " (" + inc.loan.client.accountNumber + ")",
                            pro.currency_id.Value, inc.provisionDate.AddDays(1).Date, inc.loan.loanNo, ent,
                            User.Identity.Name,
                            inc.loan.client.branchID);
                    }
                    else
                    {
                        var jb2 = journalextensions.Post("LN",
                            inc.loan.loanType.provisionsAccountID, inc.loan.loanType.provisionExpenseAccountID,
                            inc.provisionAmount,
                            "Loan Provisions Reversal for Subsequent Month- " + inc.loan.client.surName + ", " +
                            inc.loan.client.otherNames
                            + " (" + inc.loan.client.accountNumber + ")",
                            pro.currency_id.Value, inc.provisionDate.AddDays(1).Date, inc.loan.loanNo, ent,
                            User.Identity.Name,
                            inc.loan.client.branchID);
                        var list2 = jb2.jnl.ToList();
                        jb.jnl.Add(list2[0]);
                        jb.jnl.Add(list2[1]);
                    }

                    inc.reversed = true;
                }

                if (jb != null)
                {
                    ent.jnl_batch.Add(jb);
                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Loan Provisions Posted successfully!",
                        ResolveUrl("/ln/loans/postProvision.aspx"),
                        "coreERP©: Successful", IconType.ok);
                }
            }
        }

        protected void btnReInit_Click(object sender, EventArgs e)
        {
            if (cboYear.SelectedValue != "" && cboMonth.SelectedValue != "")
            {
                int year = int.Parse(cboYear.SelectedValue);
                int month = int.Parse(cboMonth.SelectedValue);
                var date = new DateTime(year, month, 1).AddMonths(1).AddSeconds(-1);

                if (date <= DateTime.Now )
                {
                    var lps = le.loanProvisions.Where(p => p.provisionDate == date).ToList();
                    foreach (var lp in lps)
                    {
                        le.loanProvisions.Remove(lp);
                    }
                    le.SaveChanges();
                    btnInit_Click(sender, e);
                }
            }
        }

        protected void cboMonth_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnInit_Click(sender, e);
        }

        protected void cboYear_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnInit_Click(sender, e);
        }

        public string GetClientName(object ln)
        {
            var l = ln as coreLogic.loan;
            if (l == null)
            {
                return "";
            }
            else if (l.client == null)
            {
                return "";
            }
            return l.client.surName + ", " + l.client.otherNames;
        }

        protected void txtAmt_OnTextChanged(object sender, EventArgs e)
        {
            
        }
    }
}