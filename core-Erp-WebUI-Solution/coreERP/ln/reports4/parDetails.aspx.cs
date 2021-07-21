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

namespace coreERP.ln.reports4
{
    public partial class parDetails : corePage
    {
        ReportDocument rpt;
        string categoryID = "";
        public override string URL
        {
            get { return "~/ln/reports4/parDetails.aspx"; }
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
                categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                Session["filter"] = "1";
                if (categoryID == null) categoryID = "";
                dtpEndDate.SelectedDate = DateTime.Now.Date; 
                Session["endDate"] = null;

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames + " - " + cl.staffNo, 
                        cl.staffID.ToString()));
                }

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }

                cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanTypes.OrderBy(p => p.loanTypeName).ToList())
                {
                    cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanTypeName, r.loanTypeID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? staffID = null;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            var type = 2;
            Session["resParDetails"] = null; 
            if (cboStaff.SelectedValue != "")
            {
                staffID = int.Parse(cboStaff.SelectedValue);
                Session["staffID"] = staffID;
            }

            Session["type"] = type;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID, dtpEndDate.SelectedDate.Value, type, staffID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null, dtpEndDate.SelectedDate.Value, type, staffID);
            } 
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? staffID = null;
            if (Session["staffID"] != null)
            {
                staffID = int.Parse(Session["staffID"].ToString()); 
            }
            if (Session["endDate"] != null)
            {
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID, (DateTime)Session["endDate"], (int)Session["type"], staffID);
                }
                else
                {
                    Bind(null, (DateTime)Session["endDate"], (int)Session["type"], staffID);
                }
            }
        }

        private void Bind(int? clientID, DateTime endDate,int type, int? staffID)
        {
            Session["endDate"] = endDate;
            var le = new coreLoansEntities();  
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            int? loanTypeID = null;
            if (cboLoanType.SelectedValue != "") loanTypeID = int.Parse(cboLoanType.SelectedValue);
            if (Session["filter"] == "1")
            {
                rpt = new coreReports.ln4.rptPARDetail();
            }
            else
            {
                rpt = new coreReports.ln4.rptPARSummary();
            }
            
            var rent = new reportEntities();
            if (Session["resParDetails"] != null)
            {
                var res = Session["resParDetails"] as List<getPARnPIRData_Result>;
                rpt.SetDataSource(res);
            }
            else
            {
                var res = rent.getPARnPIRData(clientID, staffID, branchID, loanTypeID, endDate)
                    .Where(p=> p.totalPrincipalOwed>0)
                    .ToList<coreReports.getPARnPIRData_Result>()
                    ;
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                } 
                rpt.SetDataSource(res);
                Session["resParDetails"] = res;
            }
            rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList<coreReports.vwCompProf>()); 
            if (staffID != null)
            {
                rpt.SetParameterValue("staff", "<b>Relationship Officer</b>: " + cboStaff.SelectedItem.Text);
            }
            else
            {
                rpt.SetParameterValue("staff", "");
            }
            rpt.SetParameterValue("endDate", endDate);
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                    && (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void rdbDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDetail.Checked == true)
            {
                Session["filter"] = "1";
            }
        }

        protected void rdbSummary_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSummary.Checked == true)
            {
                Session["filter"] = "2";
            }
        }

    }
}
