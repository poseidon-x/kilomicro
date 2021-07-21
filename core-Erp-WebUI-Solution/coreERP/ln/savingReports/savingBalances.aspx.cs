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

namespace coreERP.ln.savingReports
{
    public partial class savingBalances : corePage
    {
        ReportDocument rpt;
                coreLoansEntities le = new coreLoansEntities();
        public override string URL
        {
            get { return "~/ln/savingReports/savingBalances.aspx"; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = DateTime.Now.Date;

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames + " - " + cl.staffNo,
                        cl.staffID.ToString()));
                }

            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? staffID = null;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            var type = 2;
            Session["resSavingsBalances"] = null;
            if(opt1.Checked==true)
            {
                type = 1;
            } 

            Session["type"] = type;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID, dtpEndDate.SelectedDate.Value, type);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null, dtpEndDate.SelectedDate.Value, type);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? staffID = null; 
            if (Session["endDate"] != null)
            {
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID, (DateTime)Session["endDate"], (int)Session["type"]);
                }
                else
                {
                    Bind(null, (DateTime)Session["endDate"], (int)Session["type"]);
                }
            }
        }

        private void Bind(int? clientID, DateTime endDate,int type)
        {
            if (type == 1)
            {
                rpt = new coreReports.sv.rptGrpSavingBalance();
            }
            else
            {
                rpt = new coreReports.sv.rptSavingBalance();
            }
            if (Session["resSavingsBalances"] != null)
            {
                var res = Session["resSavingsBalances"] as List<getSavingBalanceReport_Result>;
                if (res == null)
                {
                    res = new List<getSavingBalanceReport_Result>();
                }
                rpt.SetDataSource(res);
            }
            else
            {
                var categoryID = Request.Params["catID"];
                int? staffID = null;
                int? branchID = null;
                if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
                if (cboStaff.SelectedValue != "") staffID = int.Parse(cboStaff.SelectedValue);

                if (categoryID == null) categoryID = "";
                var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                    && (branchID == null || p.branchID == branchID)).ToList();
                rvw.Visible = true;

                if (clientID == null )
                {
                    var res = (new reportEntities().getSavingBalanceReport(endDate).Where(p => staffID == null || p.staffID == staffID).ToList());
                    if (res.Count == 0)
                    {
                        HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                        rvw.Visible = false;
                        return;
                    }
                    for (int i = res.Count - 1; i >= 0; i--)
                    {
                        if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                        {
                            res.Remove(res[i]);
                        }
                    }
                    rpt.SetDataSource(res); 
                    Session["resSavingsBalances"] = res;
                } 
                else if (clientID != null)
                {
                    var res = (new reportEntities().getSavingBalanceReport(endDate).Where(p => p.clientID == clientID).Where(p => staffID == null || p.staffID == staffID).ToList());
                    if (res.Count == 0)
                    {
                        HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                        rvw.Visible = false;
                        return;
                    }
                    for (int i = res.Count - 1; i >= 0; i--)
                    {
                        if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                        {
                            res.Remove(res[i]);
                        }
                    }
                    rpt.SetDataSource(res);
                    Session["resSavingsBalances"] = res;
                }
            }
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Savings Balance Report as at " + endDate.ToString("dd-MMM-yyyy"));
            var branchName = "";
            if (cboBranch.Text != "")
            {
                branchName = "BRANCH: " + cboBranch.Text;
            }
            try
            {
                rpt.SetParameterValue("branch", branchName);
            }
            catch (Exception) { }
            if (cboStaff.SelectedValue != "" && cboStaff.SelectedValue != null)
            {
                rpt.SetParameterValue("staff", "Assigned to: " + cboStaff.SelectedItem.Text);
            }
            else
            {
                rpt.SetParameterValue("staff", "");
            }
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
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 1
                        || p.clientTypeID == 2 || p.clientTypeID == 5).Where(p =>
                     (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }
         
    }
}
