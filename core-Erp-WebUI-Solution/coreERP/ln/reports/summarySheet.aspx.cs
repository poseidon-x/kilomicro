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

namespace coreERP.ln.reports
{
    public partial class summarySheet : corePage
    {
        ReportDocument rpt;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/reports/summarySheet.aspx"; }
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

        string categoryID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLoansEntities();
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            { 
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (cboLoan.SelectedValue != "")
            {
                var loanID = int.Parse(cboLoan.SelectedValue);
                Session["loanID"] = loanID;
                Bind(loanID);
            }
            else
            {
                Session["loanID"] = null; 
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["loanID"] != null)
            {
                if (Session["loanID"] != null)
                {
                    var loanID = (int)Session["loanID"];
                    Bind(loanID);
                } 
            }
        }

        private void Bind(int? loanID)
        {
            le = new coreLoansEntities();
            rpt = new coreReports.ln.rptSummarySheet();
            if (loanID != null)
            {
                var res = (new coreReports.reportEntities()).vwSummarySheets.Where(p => p.loanID == loanID).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                 

            } 
            this.rvw.ReportSource = rpt; 
        }
         

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cboLoan.Text = "";
            cboLoan.Items.Clear();
            cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                foreach (var cl in (new coreReports.reportEntities()).vwSummarySheets.Where(p => p.clientID == clientID && p.amountApproved>1).ToList())
                {
                    cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.finalApprovalDate.ToString("dd-MMM-yyyy") + " - " + cl.amountApproved.ToString("#,###.#0")
                        , cl.loanID.ToString()));
                }
            }
            Session["loanID"] = null;
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
