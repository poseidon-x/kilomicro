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
    public partial class reverseProvision : System.Web.UI.Page
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
 
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
 
        protected void btnInit_Click(object sender, EventArgs e)
        { 
            if (cboYear.SelectedValue != "" && cboMonth.SelectedValue != "")
            {
                int year = int.Parse(cboYear.SelectedValue);
                int month = int.Parse(cboMonth.SelectedValue);
                var date = new DateTime(year, month, 1).AddMonths(1).AddSeconds(-1);

                var ps = le.loanProvisions.FirstOrDefault(p => p.provisionDate == date && p.posted == true);
                if (ps == null)
                {
                    HtmlHelper.MessageBox("The selected month has not been posted and cannot be reversed.", "coreERP: Not Posted", IconType.deny);
                    btnPost.Enabled = false;
                    rpPenalty.DataSource = null;
                    rpPenalty.DataBind();
                    return;
                }
                else if (ps.reversed == true)
                {
                    HtmlHelper.MessageBox("The selected month has already been reversed.", "coreERP: Already Reversed", IconType.deny);
                    btnPost.Enabled = false;
                    rpPenalty.DataSource = null;
                    rpPenalty.DataBind();
                    return;
                }
                else if(date <= DateTime.Now)
                {
                    var incs = le.loanProvisions.Where(p => p.provisionDate == date && p.reversed == false).OrderBy(p => p.daysDue).ToList();
                    foreach (var inc in incs)
                    {
                        /*if (inc.EntityState != System.Data.Entity.EntityState.Deleted)
                        {
                            //inc.loanReference.Load();
                            //inc.loan.clientReference.Load();
                            //inc.loan.loanTypeReference.Load();
                        }*/
                    }
                    rpPenalty.DataSource = incs.OrderBy(p => p.loan.client.surName).ThenBy(p => p.loan.client.otherNames).ThenBy(p => p.loan.loanNo);
                    rpPenalty.DataBind();
                    btnPost.Enabled = true; 
                }
                else
                {
                    btnPost.Enabled = false; 
                }
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
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
                    var inc = le.loanProvisions.FirstOrDefault(p => p.loanProvisionID == id && p.reversed == false);
                    if (inc != null)
                    {
                        //inc.loanReference.Load();
                        //inc.loan.loanTypeReference.Load();
                        //inc.loan.clientReference.Load();

                        if (jb == null)
                        {
                            jb = journalextensions.Post("LN",
                                inc.loan.loanType.provisionsAccountID, inc.loan.loanType.provisionExpenseAccountID,
                                inc.provisionAmount,
                                "Loan Provisions Reversal for Subsequent Month- " + inc.loan.client.surName + ", " + inc.loan.client.otherNames
                                + " (" + inc.loan.client.accountNumber + ")",
                                pro.currency_id.Value, inc.provisionDate.AddDays(1).Date, inc.loan.loanNo, ent, User.Identity.Name,
                                 inc.loan.client.branchID);
                        }
                        else
                        {
                            var jb2 = journalextensions.Post("LN",
                                inc.loan.loanType.provisionsAccountID, inc.loan.loanType.provisionExpenseAccountID,
                                inc.provisionAmount,
                                "Loan Provisions Reversal for Subsequent Month- " + inc.loan.client.surName + ", " + inc.loan.client.otherNames
                                + " (" + inc.loan.client.accountNumber + ")",
                                pro.currency_id.Value, inc.provisionDate.AddDays(1).Date, inc.loan.loanNo, ent, User.Identity.Name,
                                 inc.loan.client.branchID);
                            var list2 = jb2.jnl.ToList();
                            jb.jnl.Add(list2[0]);
                            jb.jnl.Add(list2[1]);
                        }

                        inc.reversed = true;

                    }
                }
            }
            if (jb != null)
            {
                ent.jnl_batch.Add(jb);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Loan Provisions Reversed successfully!", ResolveUrl("/ln/loans/reverseProvision.aspx"), "coreERP©: Successful", IconType.ok);
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
    }
}