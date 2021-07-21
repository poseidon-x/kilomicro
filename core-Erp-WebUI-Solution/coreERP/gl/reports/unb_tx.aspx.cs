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
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.gl.reports
{
    public partial class unb_tx : corePage
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
            get { return "~/gl/reports/unb_tx.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1, 23, 59, 59).AddDays(-1);
                dtpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1, 0, 0, 0).AddYears(-50);
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Bind();
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                Bind();
            }
        }

        private void Bind()
        {
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (Session["endDate"] != null)
            {
                endDate = (DateTime)Session["endDate"];
            }
            if (Session["startDate"] != null)
            {
                startDate = (DateTime)Session["startDate"];
            }
            rpt = new coreReports.gl.fin_stmt.rptUnbTx();
            var ent = new reportEntities();
            ent.Database.CommandTimeout = 3000;
            var res = ent.get_unb_tx(startDate, endDate).ToList<vw_unb_tx>();
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Unbalanced Transactions");
            this.rvw.ReportSource = rpt;
        }

    }
}
