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

namespace coreERP.ln.reports2
{
    public partial class schedule : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports2/schedule.aspx"; }
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
                var categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
                coreLoansEntities le = new coreLoansEntities();
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => (categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5)))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " - " + cl.accountNumber, cl.clientID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (cboLoan.SelectedValue != "")
            {
                var loanID = int.Parse(cboLoan.SelectedValue);
                Session["loanID"] = loanID;
                Bind(loanID);
            }
            else
            {
                Session["loanID"] = null;
                Bind(null);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["loanID"] != null)
            {
                if (Session["loanID"] != null)
                {
                    var loanID = (int)Session["loanID"];
                    Bind(loanID);
                }
                else
                {
                    Bind( null);
                }
            }
        }

        private void Bind(int? loanID)
        {
            var categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = ""; 
            rpt = new coreReports.ln2.rptSchedule(); 
            if (loanID != null)
            {
                var res = (new reportEntities()).vwSchedules.Where(p=>p.loanID==loanID).OrderBy(p => p.repaymentDate).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                this.rvw.ReportSource = rpt;
            } 
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                coreLoansEntities le = new coreLoansEntities();
                var clientID = int.Parse(cboClient.SelectedValue);
                cboLoan.Items.Clear();
                cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.loans.Where(p=> p.clientID==clientID).ToList())
                {
                    cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.loanNo + " - " + cl.amountDisbursed.ToString("#,###.#0"), 
                        cl.loanID.ToString()));
                }
            }
        }
    }
}
