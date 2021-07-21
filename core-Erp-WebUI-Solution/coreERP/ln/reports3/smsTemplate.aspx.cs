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

namespace coreERP.ln.reports3
{
    public partial class smsTemplate : corePage
    {
        private String[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports3/smsTemplate.aspx"; }
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
                        Session["endDate"] = null;
                        dtpEndDate.SelectedDate = DateTime.Now.AddMonths(-1);

                        cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                        foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                        {
                            cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                        }
                    }
                }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            var endDate = dtpEndDate.SelectedDate.Value;
            Session["endDate"] = endDate;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(endDate, clientID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(endDate, null);
            } 
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                var endDate = (DateTime)Session["endDate"];
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(endDate, clientID);
                }
                else
                {
                    Bind(endDate, null);
                }
            }
        }

        private void Bind(DateTime? endDate, int? clientID)
        {
            var le = new coreLoansEntities();
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                && (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.ln3.rptSMSTemplate();
            var rent=new reportEntities();
            int monthID = int.Parse(endDate.Value.Year.ToString()+ endDate.Value.Month.ToString().PadLeft(2,'0'));
            if (clientID == null)
            {
                var res = rent.vwPLSMSDatas.Where(p => p.fileMonth == monthID).OrderBy(p => p.clientName).ToList();
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
                rpt.SetDataSource(res); 
            }
            else
            {
                var res = rent.vwPLSMSDatas.Where(p => p.fileMonth == monthID && p.clientID == clientID).OrderBy(p => p.clientName).ToList();
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
                rpt.SetDataSource(res); 
            } 
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
            rpt.SetParameterValue("reportTitle", "Payday Loans Deductions for " + months[endDate.Value.Month - 1] + ", " + endDate.Value.Year.ToString());
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

    }
}
