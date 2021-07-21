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
    public partial class expired : corePage
    {
        ReportDocument rpt;
        string categoryID = "";
        public override string URL
        {
            get { return "~/ln/reports/expired.aspx"; }
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
        protected void Page_Load(object sender, EventArgs e)
        {
                categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                if (categoryID == null) categoryID = "";
                dtpEndDate.SelectedDate = DateTime.Now.Date;
                dtpStartDate.SelectedDate = new DateTime(2000, 1, 1);
                Session["endDate"] = null;

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames + " - " + cl.staffNo, 
                        cl.staffID.ToString()));
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
            int? staffID = null;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            var type = 2;
            Session["resExpiredLoans"] = null;
            if(opt1.Checked==true)
            {
                type = 1;
            }
            if (cboStaff.SelectedValue != "")
            {
                staffID = int.Parse(cboStaff.SelectedValue);
                Session["staffID"] = staffID;
            }

            Session["type"] = type;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID, dtpEndDate.SelectedDate.Value, type, staffID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null, dtpEndDate.SelectedDate.Value, type, staffID);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? staffID = null;
            if (Session["staffID"] != null)
            {
                staffID = int.Parse(Session["staffID"].ToString()); 
            }
            if (Session["endDate"] != null)
            {
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID, (DateTime)Session["endDate"], (int)Session["type"], staffID);
                }
                else
                {
                    Bind(null, (DateTime)Session["endDate"], (int)Session["type"], staffID);
                }
            }
        }

        private void Bind(int? clientID, DateTime endDate,int type, int? staffID)
        {
            var le = new coreLoansEntities();
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p =>  (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.ln3.rptExpiredLoan();

            var rent = new coreReports.reportEntities(); ;
            if (Session["resExpiredLoans"] != null)
            {
                var res = Session["resExpiredLoans"] as List<coreReports.getLoanBalanceReport_Result>;
                rpt.SetDataSource(res);
            }
            else
            {
                if (clientID == null && staffID == null)
                {
                    var res = (rent.getLoanBalanceReport(endDate).Where(
                        p => p.date >= dtpStartDate.SelectedDate).ToList());
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
                    Session["resExpiredLoans"] = res;
                }
                else if (clientID == null && staffID != null)
                {
                    var res = (rent.getLoanBalanceReport(endDate).Where(
                        p => p.date >= dtpStartDate.SelectedDate).Where(p => p.staffID == staffID).ToList());
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
                    Session["resExpiredLoans"] = res;
                }
                else if (clientID != null && staffID == null)
                {
                    var res = (rent.getLoanBalanceReport(endDate).Where(
                        p => p.date >= dtpStartDate.SelectedDate).Where(p => p.clientID == clientID).ToList());
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
                    Session["resExpiredLoans"] = res;
                }
                else
                {
                    var res = (rent.getLoanBalanceReport(endDate).Where(
                        p => p.date >= dtpStartDate.SelectedDate).Where(p => p.clientID == clientID
                        && p.staffID == staffID)
                        .ToList());
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
                    Session["resExpiredLoans"] = res;
                }
            }
            rpt.Subreports[0].SetDataSource(rent.vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Expired Loans at " + endDate.ToString("dd-MMM-yyyy"));
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
            if (staffID != null)
            {
                rpt.SetParameterValue("staff", "Assigned to: " + cboStaff.SelectedItem.Text);
            }
            else
            {
                rpt.SetParameterValue("staff", "");
            }
            rpt.SetParameterValue("date", dtpEndDate.SelectedDate.Value);
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
                foreach (var cl in le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                    && (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

    }
}
