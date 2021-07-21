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
using Telerik.Web.UI;

namespace coreERP.ln.savingReports
{
    public partial class statementSelf : corePage
    {
        ReportDocument rpt;
        private string catID = "";

        public override string URL
        {
            get { return "~/ln/savingReports/statementSelf.aspx"; }
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
                var staff = le.staffs.FirstOrDefault(p => p.userName == User.Identity.Name);
                if (staff != null)
                {
                    var ssa = le.staffSavings.FirstOrDefault(p => p.staffID == staff.staffID);
                    if (ssa != null)
                    {
                        Session["SavingID"] = ssa.savingID;
                        Bind(ssa.savingID);
                    }
                }
            }
        }
        
        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["SavingID"] != null)
            {
                var savingID = (int)Session["SavingID"];
                Bind(savingID); 
            } 
        }

        private void Bind(int? savingID)
        {
            rpt = new coreReports.sv.rptSavingStatement();
            var rent = (new reportEntities());
            var res = rent.vwSavingStatements
                .Where(p => p.loanID == savingID)
                .OrderBy(p => p.loanID)
                .ThenBy(p => p.date)
                .ToList();
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);

            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName); 
            this.rvw.ReportSource = rpt;
        }
    }
}
