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
using System.Data;

namespace coreERP.ln.reports
{
    public partial class transaction : System.Web.UI.Page
    {
        ReportDocument rpt;
        coreLoansEntities le = new coreLoansEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var branches = le.branches.ToList();

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Select A Branch", ""));
                foreach (var branch in branches)
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(branch.branchName + " (" + branch.branchCode + ")",
                        branch.branchID.ToString()));
                }
            }
            
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (cboBranch.SelectedValue != "")
            {
                int branchID = int.Parse(cboBranch.SelectedValue);
                DateTime transDate = dpkTransDate.SelectedDate.Value;

                Session["BranchID"] = branchID;
                Session["TransDate"] = transDate;
                Session["resDailyTrans"] = null;

                requestBind(branchID, transDate);
            }
        }

        private void requestBind(int branch, DateTime date)
        {
            reportEntities rent = new reportEntities();
            if (branch > 0 && date !=  null)
            {
                rpt = new coreReports.eod.rptBranchDailyTransaction();
            }

            if (branch > 0 && date != null)
            {
                List<coreReports.getDailyTransactionReportByBranch_Result> res = null;

                if (Session["resDailyTrans"] != null)
                {
                    res = Session["resDailyTrans"] as List<coreReports.getDailyTransactionReportByBranch_Result>;
                }
                else
                {
                    res = rent.getDailyTransactionReportByBranch(branch, date).ToList();
                }

                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displayed because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }

                rpt.SetDataSource(res);

                foreach (ReportDocument sr in rpt.Subreports)
                {
                    if (sr.Name == "rptProfile.rpt")
                    {
                        sr.SetDataSource(rent.vwCompProfs.ToList());
                        break;
                    }
                }
            }

            var transBranch = le.branches.FirstOrDefault(p => p.branchID == branch);

            rpt.SetParameterValue("branch", transBranch.branchName);
            rpt.SetParameterValue("branchCode", transBranch.branchCode);
            rpt.SetParameterValue("date", date);
            rpt.SetParameterValue("reportTitle", "END OF DAY TRANSACTION REPORT");

            this.rvw.ReportSource = rpt;
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["BranchID"] != null && Session["TransDate"] != null)
            {
                var branchID = (int)Session["BranchID"];
                var transDate = (DateTime)Session["TransDate"];
                requestBind(branchID, transDate);
            }
        }
    }
}