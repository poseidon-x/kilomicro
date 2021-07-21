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
    public partial class status : corePage
    {
        ReportDocument rpt;

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

        public override string URL
        {
            get { return "~/ln/reports/status.aspx"; }
        }
                coreLoansEntities le = new coreLoansEntities();
                string categoryID = "";
                protected void Page_Load(object sender, EventArgs e)
                {
                    categoryID = Request.Params["catID"];
                    if (categoryID == null) categoryID = "";
                    if (!IsPostBack)
                    {
                        Session["endDate"] = null;
                        dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                            1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                        dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                            1, 0, 0, 0));

                        cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                        foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                        {
                            cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                        }
                    }
                }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, clientID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, null);
            } 
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                DateTime startDate = (DateTime)Session["startDate"];
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(startDate, endDate, clientID);
                }
                else
                {
                    Bind(startDate, endDate, null);
                }
            }
        }

        private void Bind(DateTime startDate, DateTime endDate, int? clientID)
        {
            rpt = new coreReports.ln.rptLoans2();
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                && (branchID == null || p.branchID == branchID)).ToList();
            if (clientID == null)
            {
                var res = (new reportEntities()).vwLoans3.Where(p => p.disbursementDate>= startDate && p.disbursementDate <= endDate
                    ).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                var res2 = (new reportEntities()).vwDeposits.ToList();
                res2.Add(new vwDeposit { clientID=0});
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource(res2);
            }
            else
            {
                var res = (new reportEntities()).vwLoans3.Where(p => p.disbursementDate >= startDate && p.disbursementDate <= endDate && p.clientID == clientID).ToList();

                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                var res2 = (new reportEntities()).vwDeposits.Where(p => p.clientID == clientID).ToList();
                res2.Add(new vwDeposit { clientID = 0 });
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource(res2);
            }
            rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Loans Report as at " + endDate.ToString("dd-MMM-yyyy"));
            var branchName = "";
            if (cboBranch.Text != "")
            {
                branchName = "BRANCH: " + cboBranch.Text;
            }
            try
            {
                rpt.SetParameterValue("branch", branchName);
            }
            catch (Exception) { }
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
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
