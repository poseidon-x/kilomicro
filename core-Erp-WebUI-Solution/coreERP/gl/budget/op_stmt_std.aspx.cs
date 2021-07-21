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
using Telerik.Web.UI;
using coreERP.code;

namespace coreERP.gl.budget
{
    public partial class op_stmt_std : corePage
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
            get { return "~/gl/reports/op_stmt_std.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);

                PopulateCostCenters();
            }
        }

        protected void PopulateCostCenters()
        {
            try
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                var cc = (from a in ent.vw_gl_ou
                          select a);
                cboCC.DataSource = cc.ToList();
                cboCC.DataBind();
                cboCC.Items.Insert(0, new RadComboBoxItem("Cost Center", null));
            }
            catch (Exception ex) { }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["notx"] = chkNoTx.Checked;
            Bind(dtpEndDate.SelectedDate.Value, chkNoTx.Checked);
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                Bind(endDate, (bool)Session["notx"]);
            }
        }

        private void Bind(DateTime endDate, bool noTx)
        {
            int? ccID = null;
            if (cboCC.SelectedValue != "")
            {
                ccID = int.Parse(cboCC.SelectedValue);
            }
            rpt = new coreReports.gl.budget.rptOS();
            var res = (new reportEntities()).get_op_stmt(DateTime.Now.AddYears(-50), endDate, noTx, ccID).ToList<vw_op_stmt>();
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Income Statement v/s Budget as at " +endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }
    }
}
