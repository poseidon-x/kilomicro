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
    public partial class regularSusuAccountDetails : corePage
    {

        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/regularSusu/regularSusuReports/regularSusuAccountDetails.aspx"; }
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
                dtDate.SelectedDate = DateTime.Now;
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        { 
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null);
            } 
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        { 
            if (cboClient.SelectedValue != "")
            {
                var clientID = (int.Parse(cboClient.SelectedValue));
                Bind(clientID);
            }
            else if (ViewState["clicked"] == "1")
            {
                Bind(null);
            } 
        }

        private void Bind(int? clientID)
        {
            var le = new coreLoansEntities();
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.rs.rptRegularSusuAccountDetails();
            var rent = new coreReports.reportEntities();
            rent.Database.CommandTimeout = 10000;
            var res = rent.getRegularSusuAccountStatus(dtDate.SelectedDate, null)
                .Where(p=> clientID==null || p.clientID==clientID).OrderBy(p => p.clientName)
                .ThenBy(p=> p.regularSusuAccountNo).ToList();
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
            var res2 = rent.getRegularSusuAccountSchedule(dtDate.SelectedDate, dtDate.SelectedDate, null)
               .Where(p => (clientID == null || p.clientID == clientID) && (p.plannedContributionDate<= dtDate.SelectedDate))
               .OrderBy(p => p.clientName)
               .ThenBy(p => p.regularSusuAccountNo).ToList();
            for (int i = res2.Count - 1; i >= 0; i--)
            {
                if (cl.FirstOrDefault(p => p.clientID == res2[i].clientID) == null)
                {
                    res2.Remove(res2[i]);
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
            rpt.Database.Tables[2].SetDataSource(res);
            rpt.Database.Tables[0].SetDataSource(res3);
            rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
            rpt.Subreports[0].SetDataSource(res2);
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
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p =>  (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

    }
}
