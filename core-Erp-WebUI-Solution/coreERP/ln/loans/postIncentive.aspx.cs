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
    public partial class postIncentive : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
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
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch jb = null;
            var pro = ent.comp_prof.FirstOrDefault();

            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox; 
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                var dtDate = item.FindControl("dtDate") as RadDatePicker;
                if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked == true && dtDate != null)
                {
                    int id = int.Parse(lblID.Text);
                    var inc = le.loanIncentives.FirstOrDefault(p => p.loanIncentiveID == id);
                    if (inc != null)
                    {
                        //inc.loanReference.Load();
                        //inc.loan.loanTypeReference.Load();
                        //inc.loan.clientReference.Load();
                        //inc.agentReference.Load();

                        inc.approved = true;
                        inc.incetiveDate = dtDate.SelectedDate.Value;
                        inc.incentiveAmount = txtProposedAmount.Value.Value; 

                        if (jb == null)
                        {
                            jb = journalextensions.Post("LN",
                                inc.loan.loanType.incentiveAccountID, inc.loan.loanType.apIncentiveAccountID, inc.incentiveAmount,
                                "Payroll Loan Incentive Posted for Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                + " (" + inc.agent.agentNo + ")",
                                pro.currency_id.Value, inc.incetiveDate.Value, inc.loan.loanNo, ent, User.Identity.Name,
                                inc.loan.client.branchID);
                        }
                        else
                        {
                            var jb2 = journalextensions.Post("LN",
                                inc.loan.loanType.incentiveAccountID, inc.loan.loanType.apIncentiveAccountID, inc.incentiveAmount,
                                "Payroll Loan Incentive Posted for Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                + " (" + inc.agent.agentNo + ")",
                                pro.currency_id.Value, inc.incetiveDate.Value, inc.loan.loanNo, ent, User.Identity.Name,
                                inc.loan.client.branchID);
                            var list2 = jb2.jnl.ToList();
                            jb.jnl.Add(list2[0]);
                            jb.jnl.Add(list2[1]);
                        } 

                        inc.posted = true;
                        inc.postedDate = DateTime.Now;
                        
                    }
                }
            }
            if (jb != null)
            {
                ent.jnl_batch.Add(jb);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Payroll Loan Incentives Approved successfully!", ResolveUrl("/ln/loans/postIncentive.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var id = -1;
            if (cboAgent.SelectedValue != "")
            {
                id = int.Parse(cboAgent.SelectedValue);
                CalculateIncentive(id);
            }
            var incs = le.loanIncentives.Where(p => p.posted == false && p.agentID == id).ToList();
            foreach (var pen in incs)
            {
                //pen.loanReference.Load();
                //pen.loan.clientReference.Load();
            }
            rpPenalty.DataSource = incs;
            rpPenalty.DataBind();
        }


        public void CalculateIncentive(int? agentID)
        {
            try
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var lns = new List<coreLogic.loan>();
                if (agentID == null)
                {
                    lns = le.loans.Where(p => p.amountDisbursed > 0 && p.agentID != null
                        && p.disbursementDate >= dtpStartDate.SelectedDate && p.disbursementDate <= dtpEndDate.SelectedDate).ToList();
                }
                else
                {
                    lns = le.loans.Where(p => p.amountDisbursed > 0 && p.agentID == agentID
                        && p.disbursementDate >= dtpStartDate.SelectedDate && p.disbursementDate <= dtpEndDate.SelectedDate).ToList();
                }
                foreach (var ln in lns)
                {
                    //ln.loanTypeReference.Load();
                    //ln.loanIncentives.Load();
                    var inc = ln.loanIncentives.FirstOrDefault(p => p.posted == true);
                    if (inc == null)
                    {
                        var incentive = 0.0; 
                        foreach (var i in le.incentiveStructures.ToList())
                        {
                            if (ln.amountDisbursed-ln.processingFee >= i.lowerLimit && ln.amountDisbursed-ln.processingFee <= i.upperLimit)
                            {
                                incentive = i.incentiveAmount; 
                            }
                        }
                        if (incentive > 0)
                        {
                            inc = ln.loanIncentives.FirstOrDefault();
                            if (inc != null)
                            {
                                inc.incentiveAmount = incentive;
                            }
                            else
                            {
                                inc = new coreLogic.loanIncentive
                                {
                                    incentiveAmount = incentive,
                                    incetiveDate = ln.disbursementDate.Value,
                                    creation_date = DateTime.Now,
                                    creator = "SYSTEM",
                                    loanAmount = ln.amountDisbursed - ln.processingFee,
                                    agentID = ln.agentID.Value,
                                    posted = false,
                                    approved = false,
                                    commPosted = false,
                                    commPaid = false,
                                    paid = false,
                                    commissionAmount = 0,
                                    withHoldingAmount = 0
                                };
                                ln.loanIncentives.Add(inc);
                            }
                        }
                    }
                    le.SaveChanges();
                }
            }
            catch (Exception x)
            {
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var id = -1;
            if (cboAgent.SelectedValue != "")
            {
                id = int.Parse(cboAgent.SelectedValue);
                CalculateIncentive(id);
            }
            else
            {
                CalculateIncentive(null);
            }
            var incs = le.loanIncentives.Where(p => p.posted == false && (id == -1 || p.agentID == id)
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