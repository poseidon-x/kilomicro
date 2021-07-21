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
    public partial class undisbursed : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports/undisbursed.aspx"; }
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

                coreLoansEntities le = new coreLoansEntities();
                string categoryID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
                categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, 1,
                    1, 0, 0, 0)); 
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames + " - " + cl.staffNo, cl.staffID.ToString()));
                }

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? clientID = null;
            int? staffID = null;
            if (cboClient.SelectedValue != "")
            {
                clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
            }
            else
            {
                Session["ClientID"] = null;
            }
            if (cboStaff.SelectedValue != "")
            {
                staffID = int.Parse(cboStaff.SelectedValue);
                Session["staffID"] = staffID;
            }
            else
            {
                Session["staffID"] = null;
            } 
            Bind(clientID, staffID, dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value);
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? clientID = null;
            int? staffID = null;
            if (Session["ClientID"] != null)
            {
                clientID = (int)Session["ClientID"];
            }
            if (Session["staffID"] != null)
            {
                staffID = (int)Session["staffID"];
            } 
            Bind(clientID, staffID, dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value);
        }

        private void Bind(int? clientID, int? staffID, DateTime startDate, DateTime endDate)
        {
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                && (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.ln.rptUndisbursed();
            if (clientID == null && staffID== null)
            {
                var res = (new reportEntities()).vwUndisbureds.Where(p => p.applicationDate >= startDate && p.applicationDate <= endDate).Where(p =>
                    (categoryID != "5" && !p.categoryName.Contains("Payroll")) || (categoryID == "5" && p.categoryName.Contains("Payroll"))
                    ).OrderBy(p => p.applicationDate).ToList();
                var res2 = (new reportEntities()).vwLoanStatus.Where(p => p.date >= startDate && p.date <= endDate).ToList();
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
                rpt.SetDataSource(res);
                rpt.Subreports[1].SetDataSource(res2);
            }
            else if (clientID != null && staffID == null)
            {
                var res = (new reportEntities()).vwUndisbureds.Where(p => p.applicationDate >= startDate && p.applicationDate <= endDate && clientID == p.clientID).Where(p =>
                    (categoryID != "5" && !p.categoryName.Contains("Payroll")) || (categoryID == "5" && p.categoryName.Contains("Payroll"))
                    ).OrderBy(p => p.applicationDate).ToList();
                var res2 = (new reportEntities()).vwLoanStatus.Where(p => p.date >= startDate && p.date <= endDate && clientID == p.clientID).ToList();
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
                rpt.SetDataSource(res);
                rpt.Subreports[1].SetDataSource(res2);
            }
            else if (clientID == null && staffID != null)
            {
                var res = (new reportEntities()).vwUndisbureds.Where(p => p.applicationDate >= startDate && p.applicationDate <= endDate && staffID == p.staffID).Where(p =>
                    (categoryID != "5" && !p.categoryName.Contains("Payroll")) || (categoryID == "5" && p.categoryName.Contains("Payroll"))
                    ).OrderBy(p => p.applicationDate).ToList();
                var res2 = (new reportEntities()).vwLoanStatus.Where(p => p.date >= startDate && p.date <= endDate && staffID == p.staffID).ToList();
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
                rpt.SetDataSource(res);
                rpt.Subreports[1].SetDataSource(res2);
            }
            else
            {
                var res = (new reportEntities()).vwUndisbureds.Where(p => p.applicationDate >= startDate && p.applicationDate <= endDate && staffID == p.staffID && clientID == p.clientID).Where(p =>
                    (categoryID != "5" && !p.categoryName.Contains("Payroll")) || (categoryID == "5" && p.categoryName.Contains("Payroll"))
                    ).OrderBy(p => p.applicationDate).ToList();
                var res2 = (new reportEntities()).vwLoanStatus.Where(p => p.date >= startDate && p.date <= endDate && staffID == p.staffID && clientID == p.clientID).OrderBy(p => p.date).ToList();
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
                rpt.SetDataSource(res);
                rpt.Subreports[1].SetDataSource(res2);
            }
            rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Undisbursed Loans");
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
            if (cboStaff.SelectedValue != "")
            {
                rpt.SetParameterValue("staffTitle", "Assigned to: " + cboStaff.SelectedItem.Text);
            }
            else
            {
                rpt.SetParameterValue("staffTitle", "");
            }
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
