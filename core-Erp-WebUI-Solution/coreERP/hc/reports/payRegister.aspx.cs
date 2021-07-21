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
    public partial class payRegister : corePage
    {
        ReportDocument rpt;
        coreLoansEntities le = new coreLoansEntities();

        public override string URL
        {
            get { return "~/hc/reports/payRegister.aspx"; }
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
                cboCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffCategories.OrderBy(p=>p.staffCategoryName).ToList())
                {
                    cboCategory.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.staffCategoryName,
                        cl.staffCategoryID.ToString()));
                }
                cboGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.levels.OrderBy(p => p.levelName).ToList())
                {
                    cboGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.levelName,
                        cl.levelID.ToString()));
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
                int? levelID = null;
                int? categoryID = null;
                if (cboCategory.SelectedValue != "")
                {
                    categoryID = int.Parse(cboCategory.SelectedValue);
                }
                if (cboGrade.SelectedValue != "")
                {
                    levelID = int.Parse(cboGrade.SelectedValue);
                }
                Session["categoryID"] = categoryID;
                Session["levelID"] = levelID;
                if (cboClient.SelectedValue != "")
                {
                    var staffID = int.Parse(cboClient.SelectedValue);
                    Session["staffID"] = staffID;
                    Bind(staffID, calendarID, levelID, categoryID);
                }
                else
                {
                    Session["staffID"] = null;
                    Bind(null, calendarID, levelID, categoryID);
                }
                Session["run"] = true;
            } 
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["calendarID"] != null)
            {
                var calendarID = (int)Session["calendarID"];
                int? levelID = null;
                int? categoryID = null;
                if (Session["categoryID"] != null)
                {
                    categoryID = (int)Session["categoryID"];
                }
                if (Session["levelID"] != null)
                {
                    levelID = (int)Session["levelID"];
                }
                if (Session["staffID"] != null)
                {
                    //if (Session["staffID"] != null)
                    //{
                    //    var staffID = (int)Session["staffID"];
                    //    Bind(staffID, calendarID, levelID, categoryID);
                    //}
                    //else
                    //{

                        Bind(null, calendarID, levelID, categoryID);
                    //}
                }
                else if (Session["run"] != null)
                {
                    Bind(null, calendarID, levelID,categoryID);
                }
            }
        }

        private void Bind(int? staffID, int calendarID, int? levelID, int? categoryID)
        {
            rpt = new coreReports.hc.rptPayRegister();
            var rent = new reportEntities();
            List<int> staffInBranch =  null;
            if (cboBranch.SelectedValue != null && !String.IsNullOrEmpty(cboBranch.SelectedValue) 
                && !String.IsNullOrWhiteSpace(cboBranch.SelectedValue))
            {
                var branId = int.Parse(cboBranch.SelectedValue);
                staffInBranch = le.staffs.Where(p => p.branchID == branId).Select(p => p.staffID).ToList();
            }
            else
            {
                staffInBranch = le.staffs.Select(p => p.staffID).ToList();
            }

            if (staffInBranch.Any())
            {
                var res = rent.vwPaymasters.Where(p => p.payCalendarID == calendarID && staffInBranch.Contains(p.staffID)
                && (staffID == null || p.staffID == staffID)
                && (levelID == null || p.levelID == levelID)
                && (categoryID == null || p.staffCategoryID == categoryID)).OrderBy(p => p.staffName).ToList();

                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.Database.Tables[0].SetDataSource(res);
                rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
                this.rvw.ReportSource = rpt;
            }
            else
            {
                var res = rent.vwPaymasters.Where(p => p.payCalendarID == calendarID
                && (staffID == null || p.staffID == staffID)
                && (levelID == null || p.levelID == levelID)
                && (categoryID == null || p.staffCategoryID == categoryID)).OrderBy(p => p.staffName).ToList();

                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.Database.Tables[0].SetDataSource(res);
                rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
                this.rvw.ReportSource = rpt;
            }

            
        } 
    }
}
