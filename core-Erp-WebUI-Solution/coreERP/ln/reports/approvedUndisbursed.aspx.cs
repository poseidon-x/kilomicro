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
using coreERP.code;

namespace coreERP.ln.reports
{
    public partial class approvedUndisbursed : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports/approvedUndisbursed.aspx"; }
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
            var categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            { 
                coreLoansEntities le = new coreLoansEntities();
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p=> (categoryID!="5" && p.categoryID!=5)||(categoryID=="5" && p.categoryID==5)))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " - " + cl.accountNumber, cl.clientID.ToString()));
                }

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
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
            if (Session["ClientID"] != null)
            {
                var clientID = (int)Session["ClientID"];
                Bind(clientID);
            }
            else
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
            var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                && (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.ln.rptApprovedUndisbursed();
            if (clientID == null)
            {
                var res = (new reportEntities()).vwApprovedUndisbureds.OrderBy(p => p.applicationDate).ToList();
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
                var res = (new reportEntities()).vwApprovedUndisbureds.Where(p => p.clientID == clientID).OrderBy(p => p.applicationDate).ToList();
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
            rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Approved But Undisbursed Loans");
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
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
    }
}
