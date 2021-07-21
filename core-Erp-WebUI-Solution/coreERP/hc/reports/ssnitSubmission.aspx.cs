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

namespace coreERP.hc.reports
{
    public partial class ssnitSubmission : corePage
    {
        ReportDocument rpt;
        coreLoansEntities le = new coreLoansEntities();


        public override string URL
        {
            get { return "~/hc/reports/ssnitSubmission.aspx"; }
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

        private String[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["calendarID"] = null;

                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs.Where(p=> p.employmentStatusID==1).ToList())
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames + " - " + cl.staffNo, 
                        cl.staffID.ToString()));
                }
                cboPayCalendar.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.payCalendars.OrderByDescending(p=>p.year).ThenByDescending(p=>p.month).ToList())
                {
                    cboPayCalendar.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.year.ToString() + ", " +
                        months[cl.month-1],
                        cl.payCalendarID.ToString()));
                }

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var br in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(br.branchName.ToString(), br.branchID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (cboPayCalendar.SelectedValue != "")
            {
                var calendarID = int.Parse(cboPayCalendar.SelectedValue);
                Session["calendarID"] = calendarID;
                if (cboClient.SelectedValue != "" && cboBranch.SelectedValue != "")
                {
                    var staffID = int.Parse(cboClient.SelectedValue);
                    var branchID = int.Parse(cboBranch.SelectedValue);

                    Session["staffID"] = staffID;
                    Bind(staffID, branchID, calendarID);
                }
                else if (cboClient.SelectedValue != "")
                {
                    var staffID = int.Parse(cboClient.SelectedValue);
                    Session["staffID"] = staffID;
                    Bind(staffID,null, calendarID);
                }
                else
                {
                    Session["staffID"] = null;
                    Bind(null, null, calendarID);
                }
                Session["run"] = true;

            }
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["calendarID"] != null)
            {
                var calendarID = (int)Session["calendarID"];
                if (Session["staffID"] != null)
                {
                    if (Session["staffID"] != null)
                    {
                        var staffID = (int)Session["staffID"];
                        Bind(staffID, null,calendarID);
                    }
                    else
                    {
                        Bind(null,null, calendarID);
                    }
                }
                else if (Session["run"] != null)
                {
                    Bind(null, null,calendarID);
                }
            }
        }

        private void Bind(int? staffID, int? branId, int calendarID)
        {
            rpt = new coreReports.hc.rptSSNITPensionSubmission();
            var rent = new reportEntities();
            List<int> staffInBranch = null;
            if (staffID == null && (cboBranch.SelectedValue != null && !String.IsNullOrEmpty(cboBranch.SelectedValue)
                && !String.IsNullOrWhiteSpace(cboBranch.SelectedValue)))
            {
                var brnId = int.Parse(cboBranch.SelectedValue);
                var st = le.staffs.Where(p => p.branchID == brnId).ToList();
                staffInBranch = le.staffs.Where(p => p.branchID == brnId).Select(p => p.staffID).ToList();
                var res = rent.vwPensions.Where(p=>p.payCalendarID==calendarID
                    && staffInBranch.Contains(p.staffID)).OrderBy(p => p.staffName).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }  
                rpt.Database.Tables[0].SetDataSource(res);
                rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList()); 
            }
            else if (staffID == null )
            {
                var res = rent.vwPensions.Where(p => p.payCalendarID == calendarID
                    ).OrderBy(p => p.staffName).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.Database.Tables[0].SetDataSource(res);
                rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
            }
             
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
    }
}
