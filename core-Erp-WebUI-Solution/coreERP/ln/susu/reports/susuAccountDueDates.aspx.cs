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

namespace coreERP.ln.susu.reports
{
    public partial class susuAccountDueDates : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/susu/reports/susuAccountDueDates.aspx"; }
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

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
                cboPosition.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuPositions.OrderBy(p => p.susuPositionName).ToList())
                {
                    cboPosition.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuPositionName, r.susuPositionNo.ToString()));
                }
                cboGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuGroups.OrderBy(p => p.susuGroupName).ToList())
                {
                    cboGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGroupName, r.susuGroupNo.ToString()));
                }
                cboGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuGrades.OrderBy(p => p.susuGradeName).ToList())
                {
                    cboGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGradeName, r.susuGradeNo.ToString()));
                }
                dtStartDate.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
                dtEndDate.SelectedDate = dtStartDate.SelectedDate.Value.AddYears(1).AddSeconds(-1);
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Bind(null);
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            Bind(null);
        }

        private void Bind(int? clientID)
        {
            var le = new coreLoansEntities();
            var categoryID = Request.Params["catID"];
            int? branchID = null; 
            int? gradeId = null;
            int? positionId = null;
            int? groupId = null;
            if (cboGrade.SelectedValue != "")
            {
                gradeId = int.Parse(cboGrade.SelectedValue);
            }
            if (cboPosition.SelectedValue != "")
            {
                positionId = int.Parse(cboPosition.SelectedValue);
            }
            if (cboGroup.SelectedValue != "")
            {
                groupId = int.Parse(cboGroup.SelectedValue);
            }
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.ss.rptSusuAccountsDuePerMonth();
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 10000;
            var res = rent.getSusuAccountStatus(dtEndDate.SelectedDate, null)
                .Where(p=> p.entitledDate>= dtStartDate.SelectedDate && p.entitledDate <= dtEndDate.SelectedDate)
                .Where(p => clientID == null || p.clientID == clientID)
                .Where(p => groupId == null || groupId == p.susuGroupNo)
                .Where(p => positionId == null || positionId == p.susuPositionNo)
                .Where(p => gradeId == null || gradeId == p.susuGradeNo)
                .OrderBy(p => p.entitledDate)
                .ThenBy(p => p.clientName)
                .ThenBy(p=> p.susuAccountNo).ToList();
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
            var res3 = rent.vwClients.OrderBy(p => p.clientName).ToList();
            for (int i = res3.Count - 1; i >= 0; i--)
            {
                if (cl.FirstOrDefault(p => p.clientID == res3[i].clientID) == null)
                {
                    res3.Remove(res3[i]);
                }
            }
            rpt.Database.Tables[0].SetDataSource(res);
            rpt.Database.Tables[1].SetDataSource(res3);
            rpt.Database.Tables[2].SetDataSource(rent.vwCompProfs.ToList()); 
            var branchName = "";
            if (cboBranch.Text != "")
            {
                branchName = "BRANCH: " + cboBranch.Text;
            }
            try
            {
                rpt.SetParameterValue("branchName", branchName);
            }
            catch (Exception) { }
            this.rvw.ReportSource = rpt;
        } 
    }
}
