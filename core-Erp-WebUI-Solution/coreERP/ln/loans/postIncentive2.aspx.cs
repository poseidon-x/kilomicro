using coreLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic.Models;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class postIncentive2 : System.Web.UI.Page
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
                var txtComm = item.FindControl("txtComm") as Telerik.Web.UI.RadNumericTextBox;
                var txtWith = item.FindControl("txtWith") as Telerik.Web.UI.RadNumericTextBox;
                var txtNet = item.FindControl("txtNet") as Telerik.Web.UI.RadNumericTextBox;
                var chkSelected = item.FindControl("chkSelected") as CheckBox; 
                if (lblID != null && chkSelected != null && chkSelected.Checked == true && dtDate.SelectedDate != null)
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
                        inc.commissionAmount = txtComm.Value.Value;
                        inc.withHoldingAmount = txtWith.Value.Value;
                        inc.netCommission = txtNet.Value.Value;

                        var amountToPost = inc.commissionAmount;
                        List<JournalTransactionLine> lines = new List<JournalTransactionLine>();
                        lines.Add(new JournalTransactionLine
                        {
                            accountId = inc.loan.loanType.commissionAccountID,
                            description = "Payroll Loan Commission Posted for Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                + " (" + inc.agent.agentNo + ")",
                            debit = inc.commissionAmount,
                            credit = 0,
                            refNo = inc.loan.loanNo
                        });
                        lines.Add(new JournalTransactionLine
                        {
                            accountId = inc.loan.loanType.apCommissionAccountID,
                            description = "Payroll Loan Commission Posted for Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                + " (" + inc.agent.agentNo + ")",
                            debit = 0,
                            credit = inc.commissionAmount - inc.withHoldingAmount,
                            refNo = inc.loan.loanNo
                        });
                        lines.Add(new JournalTransactionLine
                        {
                            accountId = inc.loan.loanType.withHoldingAccountID,
                            description = "Payroll Loan Commission Posted for Agent - " + inc.agent.surName + ", " + inc.agent.otherNames
                                + " (" + inc.agent.agentNo + ")",
                            debit = 0,
                            credit = inc.withHoldingAmount,
                            refNo = inc.loan.loanNo
                        });
                        JournalExtensions ext = new JournalExtensions();
                        jb = ext.PostFullBatch("LN", lines, pro.currency_id.Value, dtDate.SelectedDate.Value, ent,
                            User.Identity.Name,
                            inc.loan.client.branchID);


                        inc.commPosted = true;
                        inc.commPostedDate = DateTime.Now;

                        if (jb != null)
                        {
                            ent.jnl_batch.Add(jb); 
                        }

                    }
                }
            }
            ent.SaveChanges();
            le.SaveChanges();
            HtmlHelper.MessageBox2("Payroll Loan Commissions Approved successfully!", ResolveUrl("/ln/loans/postIncentive.aspx"), "coreERP©: Successful", IconType.ok);
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void ComputeTotals()
        {
            var netAmount = 0.0;
            var netComm = 0.0;
            var comm = 0.0;
            var with = 0.0;
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtDisb = item.FindControl("txtDisb") as Telerik.Web.UI.RadNumericTextBox;
                var txtComm = item.FindControl("txtComm") as Telerik.Web.UI.RadNumericTextBox;
                var txtWith = item.FindControl("txtWith") as Telerik.Web.UI.RadNumericTextBox;
                var txtNet = item.FindControl("txtNet") as Telerik.Web.UI.RadNumericTextBox;
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                var dtDate = item.FindControl("dtDate") as RadDatePicker;
                if (lblID != null && dtDate != null)
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
                        inc.commissionAmount = txtComm.Value.Value;
                        inc.withHoldingAmount = txtWith.Value.Value;
                        inc.netCommission = txtNet.Value.Value;
                    }

                    netAmount += txtDisb.Value.Value;
                    netComm += txtNet.Value.Value;
                    comm += txtComm.Value.Value;
                    with += txtWith.Value.Value;
                }
            }
             
            lblNetAmount.Text = netAmount.ToString("#,###.#0");
            lblCommission.Text = comm.ToString("#,###.#0");
            lblWithholding.Text = with.ToString("#,###.#0");
            lblNetCommission.Text = netComm.ToString("#,###.#0");  
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
                    var inc=ln.loanIncentives.FirstOrDefault(p=>p.commPosted==true);
                    if (inc == null)
                    { 
                        var commission = 0.0;
                        var withHolding = 0.0;
                        var netCommission = 0.0;
                        foreach (var i in le.incentiveStructures.ToList())
                        {
                            if (ln.amountDisbursed >= i.lowerLimit && ln.amountDisbursed <= i.upperLimit)
                            { 
                                commission = (ln.amountDisbursed-ln.processingFee) * i.commissionRate / 100.0;
                                withHolding = commission * i.withHoldingRate / 100.0;
                                netCommission = Math.Floor(commission - withHolding);
                            }
                        }
                        if (netCommission > 0)
                        {
                            inc = ln.loanIncentives.FirstOrDefault();
                            if (inc != null)
                            {
                                inc.commissionAmount = commission;
                                inc.withHoldingAmount = withHolding;
                                inc.netCommission = netCommission;
                            }
                            else
                            {
                                inc = new coreLogic.loanIncentive
                                    {
                                        incentiveAmount = 0,
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
                                        commissionAmount = commission,
                                        withHoldingAmount = withHolding,
                                        netCommission = netCommission
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
            var incs = le.loanIncentives.Where(p => p.commPosted == false && (id==-1 || p.agentID == id)
                && p.loan.disbursementDate >= dtpStartDate.SelectedDate && p.loan.disbursementDate<=dtpEndDate.SelectedDate).ToList();
            foreach (var pen in incs)
            {
                //pen.loanReference.Load();
                //pen.loan.clientReference.Load();
                //pen.agentReference.Load();
            }
            rpPenalty.DataSource = incs;
            rpPenalty.DataBind();
            ComputeTotals();
        }

        protected void btnReCalc_Click(object sender, EventArgs e)
        {
            ComputeTotals();
        }
    }
}