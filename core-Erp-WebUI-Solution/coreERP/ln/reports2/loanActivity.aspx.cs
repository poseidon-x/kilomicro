﻿using System;
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
    public partial class loanActivity : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports2/loanActivity.aspx"; }
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
                Session["endDate"] = null;
                var categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, 1,
                    1, 0, 0, 0));
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["resAct"] = null;
            Session["resAct2"] = null;
            Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value);
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                DateTime startDate = (DateTime)Session["startDate"];  
                Bind(startDate, endDate); 
            }
        }

        private void Bind(DateTime startDate, DateTime endDate)
        {
            var le = new coreLoansEntities(); 
            rpt = new coreReports.ln2.rptLoanActivity4();
            if (Session["resAct"] != null && Session["resAct2"] != null && Session["resAct3"] != null)
            {
                var res = Session["resAct"] as List<getLoanActivity_Result>;
                var res2 = Session["resAct2"] as List<getLoanActivity2_Result>;
                var res3 = Session["resAct3"] as List<getLoanActivity3_Result>;
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource(res2);
                rpt.Subreports[1].SetDataSource(res3);
            }
            else
            { 
                var res = (new reportEntities()).getLoanActivity(startDate, endDate).ToList();

                var res2 = (new reportEntities()).getLoanActivity2(startDate, endDate).ToList();
                var res3 = (new reportEntities()).getLoanActivity3(startDate, endDate).ToList();
                Session["resAct"] = res;
                Session["resAct2"] = res2;
                Session["resAct3"] = res3;
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource(res2);
                rpt.Subreports[1].SetDataSource(res3); 
            }
            rpt.Subreports[2].SetDataSource((new reportEntities()).vwCompProfs.ToList()); 
            rpt.SetParameterValue("title", "Payroll Loan Activity Report b/n " +startDate.ToString("dd-MMM-yyyy") +" and " +endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
    }
}
