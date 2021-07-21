using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreERP.code;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using coreReports;
using coreLogic;

namespace coreERP.ln.reports
{
    public partial class receipt : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports/receipt.aspx"; }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rpt != null)
            {
                try
                {
                    rpt.Dispose();
                    rpt.Close();
                    rpt = null;
                }
                catch (Exception) { }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["cashierReceipt"] != null)
                {
                    var loanRepayment = Session["cashierReceipt"] as cashierReceipt;
                    if (loanRepayment != null && loanRepayment.loan != null && loanRepayment.loan.client != null)
                    {
                        var le = new coreLogic.coreLoansEntities();
                        rpt = new coreReports.ln.rptReceipt();
                        rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                        rpt.Subreports[1].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                        
                        rpt.SetParameterValue("companyName", Settings.companyName);
                        rpt.SetParameterValue("amount", loanRepayment.amount);
                        rpt.SetParameterValue("receiptDate", loanRepayment.txDate);
                        rpt.SetParameterValue("receiptNo", loanRepayment.loan.loanNo + "/" + loanRepayment.cashierReceiptID.ToString());
                        rpt.SetParameterValue("clientName", loanRepayment.loan.client.surName + ", " + loanRepayment.loan.client.otherNames
                            + " (" + loanRepayment.loan.client.accountNumber + ")");
                        rpt.SetParameterValue("cashier", loanRepayment.cashiersTill.userName);
                        var mp = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == loanRepayment.paymentModeID);
                        if (mp != null)
                        {
                            rpt.SetParameterValue("modeOfPayment", mp.modeOfPaymentName);
                        }
                        else
                        {
                            rpt.SetParameterValue("modeOfPayment", "");
                        }
                        this.rvw.ReportSource = rpt;
                    }
                }
            }
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["cashierReceipt"] != null)
            {
                var loanRepayment = Session["cashierReceipt"] as cashierReceipt;
                if (loanRepayment != null && loanRepayment.loan != null && loanRepayment.loan.client != null)
                {
                    var le = new coreLogic.coreLoansEntities();
                    rpt = new coreReports.ln.rptReceipt();
                    rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                    rpt.Subreports[1].SetDataSource((new reportEntities()).vwCompProfs.ToList());

                    rpt.SetParameterValue("companyName", Settings.companyName);
                    rpt.SetParameterValue("amount", loanRepayment.amount);
                    rpt.SetParameterValue("receiptDate", loanRepayment.txDate);
                    rpt.SetParameterValue("receiptNo", loanRepayment.loan.loanNo + "/" + loanRepayment.cashierReceiptID.ToString());
                    rpt.SetParameterValue("clientName", loanRepayment.loan.client.surName + ", " + loanRepayment.loan.client.otherNames
                        + " (" + loanRepayment.loan.client.accountNumber + ")");
                    rpt.SetParameterValue("cashier", loanRepayment.cashiersTill.userName);
                    var mp = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == loanRepayment.paymentModeID);
                    if (mp != null)
                    {
                        rpt.SetParameterValue("modeOfPayment", mp.modeOfPaymentName);
                    }
                    else
                    {
                        rpt.SetParameterValue("modeOfPayment", "");
                    }
                    this.rvw.ReportSource = rpt;
                }
            }
        }

    }
}
