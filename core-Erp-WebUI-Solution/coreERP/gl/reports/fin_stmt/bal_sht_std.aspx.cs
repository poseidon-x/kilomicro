﻿using System;
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
using Telerik.Web.UI;

namespace coreERP.gl.reports.fin_stmt
{
    public partial class bal_sht_std : corePage
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
            get { return "~/gl/reports/fin_stmt/bal_sht_std.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);

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
            Session["sum"] = chkSum.Checked;
            Session["resb"] = null;
            Bind(dtpEndDate.SelectedDate.Value, chkNoTx.Checked, chkSum.Checked);
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                Bind(endDate, (bool)Session["notx"], (bool)Session["sum"]);
            }
        }

        private void Bind(DateTime endDate, bool noTx, bool sum)
        {
            int? ccID = null;
            if (cboCC.SelectedValue != "")
            {
                ccID = int.Parse(cboCC.SelectedValue);
            }
            List<vw_acc_bals> res;
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 300000;
            if (sum == false)
            {
                rpt = new coreReports.gl.fin_stmt_2.rptBS();
                if (Session["resb"] != null)
                {
                    res = Session["resb"] as List<vw_acc_bals>;
                }
                else
                {
                    res = rent.get_acc_bals(dtpStartDate.SelectedDate, endDate, noTx,null, ccID).ToList<vw_acc_bals>();
                    Session["resb"] = res;
                }
            }
            else
            {
                rpt = new coreReports.gl.fin_stmt_2.rptBS();
                if (Session["resb"] != null)
                {
                    res = Session["resb"] as List<vw_acc_bals>;
                }
                else
                {
                    res = rent.get_bal_sht_sum(dtpStartDate.SelectedDate, endDate, noTx, sum,ccID).ToList<vw_acc_bals>();
                    Session["resb"] = res;
                }
            }
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource(rent.vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "G/L Movement Report b/n " + dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                + " & " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }

    }
}