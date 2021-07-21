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
    public partial class susuAccountStages : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/susu/reports/susuAccountStages.aspx"; }
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
                dtEndDate.SelectedDate = DateTime.Now;
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
            double stageID = double.Parse(cboStage.SelectedValue);
            int statusID = int.Parse(cboStatus.SelectedValue); 
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
            var le = new coreLoansEntities();
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p =>  (branchID == null || p.branchID == branchID)).ToList();
            if (stageID == 2)
            {
                rpt = new coreReports.ss.rptSusuAccountStages();
            }
            else
            {
                rpt = new coreReports.ss.rptSusuAccountStages1A();
            }
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 10000;
            var res = rent.getSusuAccountStatus(dtEndDate.SelectedDate, null)
                .Where(p => /*p.applicationDate >= dtStartDate.SelectedDate &&*/ p.applicationDate <= dtEndDate.SelectedDate)
                .Where(p => clientID == null || p.clientID == clientID).OrderBy(p => p.clientName)
                .Where(p => stageID == 0
                    || (stageID == 1 && (p.statusID == 1 || p.statusID == 2 || p.statusID == 4))
                    || (stageID == 1.5 && (p.statusID == 3))
                    || (stageID == 2 && (p.statusID == 6 || p.statusID == 7)))
                .Where(p=> (statusID==0)
                    || (statusID == 1 && (p.statusID == 3 || p.statusID == 7))
                    || (statusID == 2 && (p.statusID == 1))
                    || (statusID == 3 && (p.statusID == 6))
                    || (statusID == 4 && (p.statusID == 7))
                    || (statusID == 5 && (p.statusID == 6))
                    || (statusID == 6 && (p.statusID == 8))
                )
                .Where(p=> groupId==null || groupId == p.susuGroupNo)
                .Where(p=> positionId==null || positionId == p.susuPositionNo)
                .Where(p=> gradeId==null || gradeId == p.susuGradeNo)
                .OrderBy(p=> p.startDate)
                .ThenBy(p => p.clientName)
                .ThenBy(p => p.susuAccountNo).ToList();
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
            if (stageID == 0)
            {
                rpt.SetParameterValue("reportTitle", "Susu Contributions Performance");
            }
            else if (stageID == 1)
            {
                rpt.SetParameterValue("reportTitle", "Susu Contributions Performance (Stage 1A)");
            }
            else if (stageID == 1.5)
            {
                rpt.SetParameterValue("reportTitle", "Susu Contributions Performance (Stage 1B)");
            }
            else if (stageID == 2)
            {
                rpt.SetParameterValue("reportTitle", "Susu Contributions Performance (Stage 2)");
            }
            if (stageID == 0)
            {
                rpt.SetParameterValue("color", "1");
            }
            else
            {
                rpt.SetParameterValue("color", "");
            }
            rpt.SetParameterValue("date", "As Of "  + dtEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        } 
    }
}
