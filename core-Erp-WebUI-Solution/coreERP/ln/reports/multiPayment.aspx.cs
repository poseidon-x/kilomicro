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
    public partial class multiPayment : corePage
    {
        ReportDocument rpt;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/reports/cashier.aspx"; }
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
            le = new coreLoansEntities();
            if (!IsPostBack)
            { 
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (cboRepayment.SelectedValue != "")
            {
                var clientID = int.Parse(cboRepayment.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["ClientID"] != null)
            { 
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID);
                }
                else
                {
                    Bind( null);
                }
            }
        }

        private void Bind(int? rpID)
        {
            var clientName = "";
            le = new coreLoansEntities();
            rpt = new coreReports.ln.rptMultiPmtRem();
            if (rpID != null)
            {
                var mrc = le.multiPaymentClients.FirstOrDefault(p => p.multiPaymentClientID == rpID);
                //mrc.cashierReceiptReference.Load();
                 
                //mrc.multiPayments.Load();
                List<coreReports.vwLoanBalance> list = new List<vwLoanBalance>();
                foreach (var mr in mrc.multiPayments)
                {
                    list.Add(new vwLoanBalance
                    {
                        amountPaid=(int)mr.amountPaid,
                        description=mr.description,
                        disbursementDate=mr.disbursementDate,
                        interestOutstanding=mr.interestOutstanding,
                        principalOutstanding=mr.principalOutstanding,
                        invoiceNo=mr.invoiceNo,
                        loanID=mr.loanID,
                        loanNo=mr.loanNo,
                        processingFee=mr.processingFee,
                        totalOutstanding=mr.principalOutstanding+mr.interestOutstanding
                    });
                    if (clientName == "")
                    {
                        var loan = le.loans.FirstOrDefault(p => p.loanID == mr.loanID);
                        if (loan != null && loan.client != null)
                        {
                            var cl = loan.client;
                            clientName = (cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                                ", " + cl.otherNames);
                        }
                    }
                } 
                if (list.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(list);
                rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());

                rpt.SetParameterValue("companyName", Settings.companyName);
                rpt.SetParameterValue("invoiceDate", mrc.invoiceDate);
                rpt.SetParameterValue("clientName", ((mrc==null || mrc.clientName==null || mrc.clientName.Trim()=="")?
                    clientName
                    :mrc.clientName));
                rpt.SetParameterValue("amount", mrc.amount);

            }
            rpt.SetParameterValue("companyName", Settings.companyName);
            //rpt.SetParameterValue("reportTitle", "Loans Report as at " + endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cboRepayment.Items.Clear();
            cboRepayment.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                foreach (var cl in le.multiPaymentClients.Where(p => p.cashierReceipt.clientID == clientID).ToList())
                {
                    cboRepayment.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.invoiceDate.ToString("dd-MMM-yyyy") + " - " + cl.amount.ToString("#,###.#0")
                        , cl.multiPaymentClientID.ToString()));
                }
            }
        }
        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p =>
                     (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

    }
}
