using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.cashier
{
    public partial class reverse : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            cboClient.Items.Add(new RadComboBoxItem("", ""));
            foreach (var cl in le.clients.OrderBy(p => p.surName))
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                        ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
            }
        }


        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            List<coreLogic.loan> loans = new List<coreLogic.loan>(); 
            if (cboClient.SelectedValue != "")
            {
                int id = int.Parse(cboClient.SelectedValue);
                var cl = le.clients.FirstOrDefault(p => p.clientID == id);
                if (cl != null)
                {
                    //cl.loans.Load();
                    cboLoan.SelectedValue = "";
                    cboLoan.Items.Clear();
                    cboLoan.Items.Add(new RadComboBoxItem("", ""));
                    loans = cl.loans.Where(p => p.disbursementDate != null).ToList();
                    foreach (var ln in loans)
                    {
                        cboLoan.Items.Add(new RadComboBoxItem(ln.disbursementDate.Value.ToString("dd-MMM-yyyy")+ "(" +
                            ln.amountRequested.ToString("#,###.##") + ")", ln.loanID.ToString()));
                    }
                }
            }             
        }

        protected void cboLoan_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            List<coreLogic.loanRepayment> lrs = new List<coreLogic.loanRepayment>();
            List<coreLogic.loanTranch> lts = new List<coreLogic.loanTranch>();
            List<coreLogic.loanPenalty> lai = new List<coreLogic.loanPenalty>();
            List<coreLogic.loanFee> lpf = new List<coreLogic.loanFee>();
            List<coreLogic.loanInsurance> lins = new List<coreLogic.loanInsurance>();
            if (cboLoan.SelectedValue != "")
            {
                int id = int.Parse(cboLoan.SelectedValue);
                var ln = le.loans.FirstOrDefault(p => p.loanID == id);
                if (ln != null)
                {
                    //ln.clientReference.Load();
                    //ln.loanRepayments.Load();
                    lrs = ln.loanRepayments.OrderByDescending(p=>p.repaymentDate).ToList();
                    //ln.loanTranches.Load();
                    lts = ln.loanTranches.OrderByDescending(p => p.disbursementDate).ToList();
                    foreach (var rp in lrs)
                    {
                        //rp.repaymentTypeReference.Load();
                    }
                    foreach (var lt in lts)
                    {
                        //lt.modeOfPaymentReference.Load();
                    }
                    //ln.loanFees.Load();
                    lpf = ln.loanFees.ToList();
                    //ln.loanPenalties.Load();
                    lai = ln.loanPenalties.ToList();
                    lins = ln.loanInsurances.ToList();
                }
            }
            grid.DataSource = lrs;
            grid.DataBind();
            grid2.DataSource = lts;
            grid2.DataBind();
            grid3.DataSource = lai;
            grid3.DataBind();
            grid4.DataSource = lpf;
            grid4.DataBind();
            grid5.DataSource = lins;
            grid5.DataBind();

        }

    }
}