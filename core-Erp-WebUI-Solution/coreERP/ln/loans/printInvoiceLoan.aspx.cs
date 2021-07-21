using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using coreReports;
using coreLogic;
using coreERP.code;



namespace coreERP.ln.reports
{
    public partial class printInvoiceLoan : corePage
    {
        public override string URL
        {
            get { return "~/ln/reports/printInvoiceLoan.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var lent = new coreLoansEntities();
                var rent = new reportEntities();
                if (Request.Params["id"] != null)
                {
                    var id = int.Parse(Request.Params["id"]);
                    var il = lent.invoiceLoans.FirstOrDefault(p => p.invoiceLoanID == id);
                    if (il != null)
                    {
                        //il.clientReference.Load();
                        ReportDocument rpt = new coreReports.ln.rptInvoiceLoanDisb();
                        rpt.Subreports[0].SetDataSource(rent.vwCompProfs.ToList());
                        
                        rpt.SetParameterValue("companyName", Settings.companyName);
                        rpt.SetParameterValue("amount", il.proposedAmount);
                        rpt.SetParameterValue("invoiceDate", il.invoiceDate);
                        rpt.SetParameterValue("invoiceDescription", il.invoiceDescription);
                        rpt.SetParameterValue("clientName", il.client.surName + ", " + il.client.otherNames
                            + " (" + il.client.accountNumber + ")");
                        rpt.SetParameterValue("with", il.withHoldingTax);
                        rpt.SetParameterValue("invoiceAmount", il.invoiceAmount);
                        this.rvw.ReportSource = rpt;
                    }
                }
                else if (Request.Params["ilm"] != null)
                {
                    var id = int.Parse(Request.Params["ilm"]);
                    var res = rent.vwInvoiceLoans.Where(p => p.invoiceLoanMasterID == id).OrderBy(p => p.invoiceDescription).ToList();
                    ReportDocument rpt = new coreReports.ln.rptInvoiceLoanMaster();
                    rpt.SetDataSource(res);
                    rpt.Subreports[0].SetDataSource(rent.vwCompProfs.ToList());
                    this.rvw.ReportSource = rpt;
                }
            }
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            var lent = new coreLoansEntities();
            var rent = new reportEntities();
            if (Request.Params["id"] != null)
            {
                var id = int.Parse(Request.Params["id"]);
                var il = lent.invoiceLoans.FirstOrDefault(p => p.invoiceLoanID == id);
                if (il != null)
                {
                    //il.clientReference.Load();
                    ReportDocument rpt = new coreReports.ln.rptInvoiceLoanDisb();
                    rpt.Subreports[0].SetDataSource(rent.vwCompProfs.ToList());

                    rpt.SetParameterValue("companyName", Settings.companyName);
                    rpt.SetParameterValue("amount", il.proposedAmount);
                    rpt.SetParameterValue("invoiceDate", il.invoiceDate);
                    rpt.SetParameterValue("invoiceDescription", il.invoiceDescription);
                    rpt.SetParameterValue("clientName", il.client.surName + ", " + il.client.otherNames
                        + " (" + il.client.accountNumber + ")");
                    rpt.SetParameterValue("with", il.withHoldingTax);
                    rpt.SetParameterValue("invoiceAmount", il.invoiceAmount);
                    this.rvw.ReportSource = rpt;
                }
            }
            else if (Request.Params["ilm"] != null)
            {
                var id = int.Parse(Request.Params["ilm"]);
                var res = rent.vwInvoiceLoans.Where(p => p.invoiceLoanMasterID == id).OrderBy(p => p.invoiceDescription).ToList();
                ReportDocument rpt = new coreReports.ln.rptInvoiceLoanMaster();
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource(rent.vwCompProfs.ToList());
                this.rvw.ReportSource = rpt;
            }
        }

    }
}
