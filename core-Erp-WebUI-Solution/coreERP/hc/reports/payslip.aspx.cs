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
    public partial class payslip : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/hc/reports/payslip.aspx"; }
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
                coreLoansEntities le = new coreLoansEntities();
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
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (cboPayCalendar.SelectedValue != "")
            {
                var calendarID = int.Parse(cboPayCalendar.SelectedValue);
                Session["calendarID"] = calendarID;
                if (cboClient.SelectedValue != "")
                {
                    var staffID = int.Parse(cboClient.SelectedValue);
                    Session["staffID"] = staffID;
                    Bind(staffID, calendarID);
                }
                else
                {
                    Session["staffID"] = null;
                    Bind(null, calendarID);
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
                        Bind(staffID, calendarID);
                    }
                    else
                    {
                        Bind(null, calendarID);
                    }
                }
                else if (Session["run"] != null)
                {
                    Bind(null, calendarID);
                }
            }
        }

        private void Bind(int? staffID, int calendarID)
        {
            rpt = new coreReports.hc.rptPayslip();
            var rent = new reportEntities(); 
            if (staffID == null)
            {
                var res = rent.vwPaymasters.Where(p=>p.payCalendarID==calendarID
                    ).OrderBy(p => p.staffName).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                } 
                var res2 = rent.vwPaymasterEarnings.Where(p => p.payCalendarID == calendarID
                     ).ToList();
                var res3 = rent.vwPaymasterDeductions.Where(p => p.payCalendarID == calendarID
                    ).ToList();
                var res4 = rent.vwPaymasterEmployerPensions.Where(p => p.payCalendarID == calendarID
                    ).ToList();
                if (res4.Count == 0)
                {
                    res4.Add(new vwPaymasterEmployerPension
                    {
                        amount = 0,
                        bankAccountNo = "",
                        bankBranchName = "",
                        bankName = "",
                        basicSalary = 0,
                        description = "",
                        DOB = DateTime.MinValue,
                        EarningType = "",
                        isPosted = false,
                        isProcessed = false,
                        jobTitleID = 1,
                        jobTitleName = "",
                        month = 1,
                        netSalary = 0,
                        payCalendarID = 1,
                        payMasterID = 1,
                        ssn = "",
                        staffCategoryID = 1,
                        staffCategoryName = "",
                        staffID = 1,
                        staffName = "",
                        staffNo = "",
                        typeID = 1,
                        year = 1
                    });
                }
                rpt.Database.Tables[0].SetDataSource(res);
                rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
                rpt.Subreports[0].SetDataSource(res3);
                rpt.Subreports[1].SetDataSource(res2);
                rpt.Subreports[2].SetDataSource(res4);
            }
            else
            {
                var res = rent.vwPaymasters.Where(p => p.payCalendarID == calendarID
                    && p.staffID==staffID).OrderBy(p => p.staffName).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                var res2 = rent.vwPaymasterEarnings.Where(p => p.payCalendarID == calendarID
                    && p.staffID == staffID).ToList();
                var res3 = rent.vwPaymasterDeductions.Where(p => p.payCalendarID == calendarID
                    && p.staffID == staffID).ToList();
                var res4 = rent.vwPaymasterEmployerPensions.Where(p => p.payCalendarID == calendarID
                    && p.staffID == staffID).ToList();
                if (res4.Count == 0)
                {
                    res4.Add(new vwPaymasterEmployerPension
                    {
                        amount = 0,
                        bankAccountNo = "",
                        bankBranchName = "",
                        bankName = "",
                        basicSalary = 0,
                        description = "",
                        DOB = DateTime.MinValue,
                        EarningType = "",
                        isPosted = false,
                        isProcessed = false,
                        jobTitleID = 1,
                        jobTitleName = "",
                        month = 1,
                        netSalary = 0,
                        payCalendarID = 1,
                        payMasterID = 1,
                        ssn = "",
                        staffCategoryID = 1,
                        staffCategoryName = "",
                        staffID = 1,
                        staffName = "",
                        staffNo = "",
                        typeID = 1,
                        year = 1
                    });
                }
                rpt.Database.Tables[0].SetDataSource(res);
                rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
                rpt.Subreports[0].SetDataSource(res3);
                rpt.Subreports[1].SetDataSource(res2);
                rpt.Subreports[2].SetDataSource(res4);
            } 
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
    }
}
