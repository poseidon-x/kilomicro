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
    public partial class payIncentive : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                dtpEndDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddSeconds(-1);
                dtpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                cboAgent.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.agents.OrderBy(p => p.surName))
                {
                    cboAgent.Items.Add(new RadComboBoxItem(cl.surName +
                    ", " + cl.otherNames + " (" + cl.agentNo + ")", cl.agentID.ToString()));
                }

                cboBank.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in ent.bank_accts.OrderBy(p=>p.bank_acct_desc).ToList())
                {
                    cboBank.Items.Add(new RadComboBoxItem(cl.bank_acct_desc + " (" + cl.bank_acct_num + ")", cl.bank_acct_id.ToString()));
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch jb = null;
            var pro = ent.comp_prof.FirstOrDefault();
            if (((cboPaymentMode.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentMode.SelectedValue == "1")))
            {
                int? bankID = null;
                if (cboBank.SelectedValue != null && cboBank.SelectedValue != "")
                    bankID = int.Parse(cboBank.SelectedValue);
               
                foreach (RepeaterItem item in rpPenalty.Items)
                {
                    var lblID = item.FindControl("lblID") as Label;
                    var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox; 
                    var chkSelected = item.FindControl("chkSelected") as CheckBox;
                    var dtDate = item.FindControl("dtDate") as RadDatePicker;
                    if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked == true && dtDate != null)
                    {
                        int id = int.Parse(lblID.Text);
                        var inc = le.loanIncentives.FirstOrDefault(p => p.loanIncentiveID == id && p.paid==false);
                        if (inc != null)
                        {
                            //inc.loanReference.Load();
                            //inc.loan.loanTypeReference.Load();
                            //inc.loan.clientReference.Load();
                            //inc.agentReference.Load();

                            var crAccNo = ent.accts.FirstOrDefault(p => p.acc_num == "1002").acct_id;

                            if (cboPaymentMode.SelectedValue != "1") {
                                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                                if (ba != null)
                                {
                                    ////ba.acctsReference.Load();
                                    if (ba.accts != null)
                                    {
                                        crAccNo = ba.accts.acct_id;
                                    }
                                }
                            }

                            if (jb == null)
                            {
                                jb = journalextensions.Post("LN",
                                    inc.loan.loanType.apIncentiveAccountID, crAccNo, inc.incentiveAmount,
                                    "Payroll Loan Incentive Paid to Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                    + " (" + inc.agent.agentNo + ")",
                                    pro.currency_id.Value, dtDate.SelectedDate.Value, inc.loan.loanNo, ent, User.Identity.Name, inc.loan.client.branchID);
                            }
                            else
                            {
                                var jb2 = journalextensions.Post("LN",
                                    inc.loan.loanType.apIncentiveAccountID, crAccNo, inc.incentiveAmount,
                                    "Payroll Loan Incentive Paid to Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                    + " (" + inc.agent.agentNo + ")",
                                    pro.currency_id.Value, dtDate.SelectedDate.Value, inc.loan.loanNo, ent, User.Identity.Name, inc.loan.client.branchID);
                                var list2 = jb2.jnl.ToList();
                                jb.jnl.Add(list2[0]);
                                jb.jnl.Add(list2[1]);
                            }                              
                            inc.paid = true;
                            inc.paidDate = DateTime.Now;
                        }
                    }
                }
                if (jb != null)
                {
                    ent.jnl_batch.Add(jb);
                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Payroll Loan Incentives Paid successfully!", ResolveUrl("/ln/loans/postIncentive.aspx"), "coreERP©: Successful", IconType.ok);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var id = -1;
            if (cboAgent.SelectedValue != "")
            {
                id = int.Parse(cboAgent.SelectedValue);
            }
            var incs = le.loanIncentives.Where(p => p.posted == true && p.paid == false && (id == -1 || p.agentID == id)
                && p.loan.disbursementDate >= dtpStartDate.SelectedDate && p.loan.disbursementDate <= dtpEndDate.SelectedDate).ToList();
            foreach (var pen in incs)
            {
                //pen.loanReference.Load();
                //pen.loan.clientReference.Load();
                //pen.agentReference.Load();
            }
            rpPenalty.DataSource = incs;
            rpPenalty.DataBind();
        }
    }
}