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
using Telerik.Web.UI;
using coreERP.code;

namespace coreERP.ln.depositReports
{
    public partial class deposits : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/depositReports/deposits.aspx"; }
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
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                ViewState["filter"] = 1;
                dtpEndDate.SelectedDate = DateTime.Today.Date;
                dtpStartDate.SelectedDate = DateTime.Today.Date;

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
                Session["type"] = 1;
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? staffID = null;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            Session["startDate"] = dtpStartDate.SelectedDate.Value;
            Session["resDepositBalances"] = null;

            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID, dtpStartDate.SelectedDate.Value,dtpEndDate.SelectedDate.Value);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null, dtpStartDate.SelectedDate.Value,dtpEndDate.SelectedDate.Value);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? staffID = null; 
            if (Session["endDate"] != null)
            {
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID, (DateTime)Session["startDate"],(DateTime)Session["endDate"]);
                }
                else
                {
                    Bind(null, (DateTime)Session["startDate"],(DateTime)Session["endDate"]);
                }
            }
        }

        private void Bind(int? clientID, DateTime startDate, DateTime endDate)
        {
            int type = (int) Session["type"];
            if (type == 1)
            {
                rpt = new coreReports.dp.rptDeposit2();
            }
            else
            {
                rpt = new coreReports.dp.rptDeposit();
            }
            if (Session["resDepositBalances"] != null)
            {
                var res = Session["resDepositBalances"] as List<coreReports.getDepositBalanceReport_Result>;
                rpt.SetDataSource(res);
            }
            else
            {
                var categoryID = Request.Params["catID"];
                int? staffID = null;
                int? branchID = null;
                if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
                if (categoryID == null) categoryID = "";
                var cl = le.clients.Where(p => (branchID == null || p.branchID == branchID)).ToList();
                if (cboStaff.SelectedValue != "")
                {
                    staffID = int.Parse(cboStaff.SelectedValue);
                }

                var rent = new reportEntities();
                rent.Database.CommandTimeout = 60000;

                var res = (rent
                .getDepositBalanceReport(endDate)
                    .Where(p => clientID == null || p.clientID == clientID)
                    .Where(p => staffID == null || p.staffID == staffID)
                    .Where(p => p.firstDepositDate >= startDate)
                    .ToList());
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox(
                        "Report cannot be displaid because no data was found for the provided criteria",
                        "coreERP©: No Data", IconType.deny);
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
                Session["resDepositBalances"] = res;
            }
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Deposit Balance Report as at " + endDate.ToString("dd-MMM-yyyy"));
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
            if (cboStaff.SelectedValue != "" && cboStaff.SelectedValue!= null)
            {
                rpt.SetParameterValue("staff", "Assigned to: "  + cboStaff.SelectedItem.Text);
            }
            else
            {
                rpt.SetParameterValue("staff", "");
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
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 1
                        || p.clientTypeID == 2 || p.clientTypeID == 5).Where(p =>
                     (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void opt1_CheckedChanged(object sender, EventArgs e)
        {
            Session["type"] = 1;
            btnRun_Click(btnRun, EventArgs.Empty);
        }

        protected void opt2_CheckedChanged(object sender, EventArgs e)
        {
            Session["type"] = 2;
            btnRun_Click(btnRun, EventArgs.Empty);
        }
    }
}
