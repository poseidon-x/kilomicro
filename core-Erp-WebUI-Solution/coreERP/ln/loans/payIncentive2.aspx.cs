using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic.Models;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class payIncentive2 : System.Web.UI.Page
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
                foreach (var cl in ent.bank_accts.OrderBy(p => p.bank_acct_desc).ToList())
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
                    var txtComm = item.FindControl("txtComm") as Telerik.Web.UI.RadNumericTextBox;
                    var txtWith = item.FindControl("txtWith") as Telerik.Web.UI.RadNumericTextBox;
                    var chkSelected = item.FindControl("chkSelected") as CheckBox; 
                    if (lblID != null && chkSelected != null && chkSelected.Checked == true && dtDate.SelectedDate != null)
                    {
                        int id = int.Parse(lblID.Text);
                        var inc = le.loanIncentives.FirstOrDefault(p => p.loanIncentiveID == id && p.commPaid==false);
                        if (inc != null)
                        {
                            //inc.loanReference.Load();
                            //inc.loan.loanTypeReference.Load();
                            //inc.loan.clientReference.Load();
                            //inc.agentReference.Load();
                             
                            var crAccNo = ent.accts.FirstOrDefault(p=>p.acc_num=="1002").acct_id;

                            if (cboPaymentMode.SelectedValue != "1") {
                                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                                if (ba != null)
                                {
                                    //ba.acctsReference.Load();
                                    if (ba.accts != null)
                                    {
                                        crAccNo = ba.accts.acct_id;
                                    }
                                }
                            }
                            List<JournalTransactionLine> lines = new List<JournalTransactionLine>();
                            lines.Add(new JournalTransactionLine
                            {
                                accountId = inc.loan.loanType.apCommissionAccountID,
                                description = "Payroll Loan Commission Paid to Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                    + " (" + inc.agent.agentNo + ")",
                                debit = inc.commissionAmount-inc.withHoldingAmount,
                                credit = 0,
                                refNo = inc.loan.loanNo
                            });
                            lines.Add(new JournalTransactionLine
                            {
                                accountId = crAccNo,
                                description = "Payroll Loan Commission Paid to Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                    + " (" + inc.agent.agentNo + ")",
                                debit = 0,
                                credit = inc.commissionAmount - inc.withHoldingAmount,
                                refNo = inc.loan.loanNo
                            });
                            JournalExtensions ext = new JournalExtensions();
                            jb = ext.PostFullBatch("LN", lines, pro.currency_id.Value, dtDate.SelectedDate.Value, ent,
                                User.Identity.Name,
                                inc.loan.client.branchID);
                              
                            inc.commPaid = true;
                            inc.commPaidDate = DateTime.Now;
                            if (jb != null)
                            {
                                ent.jnl_batch.Add(jb);
                            }
                        }
                    }
                    
                        le.SaveChanges();
                        ent.SaveChanges();
                    

                }
                HtmlHelper.MessageBox2("Payroll Loan Commissions Paid successfully!", ResolveUrl("/ln/loans/postIncentive.aspx"), "coreERP©: Successful", IconType.ok);
                
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

            var incs = le.loanIncentives.Where(p => p.commPosted == true && p.commPaid==false && (id == -1 || p.agentID == id)
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