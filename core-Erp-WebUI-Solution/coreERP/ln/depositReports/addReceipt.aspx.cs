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

namespace coreERP.ln.depositReports
{
    public partial class addReceipt : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/depositReports/addReceipt.aspx"; }
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
            if (!IsPostBack)
            {
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["loanID"] = id;
                    Bind(id);
                }
                else
                {
                    var categoryID = Request.Params["catID"];
                    if (categoryID == null) categoryID = "";
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
            rpt = new coreReports.dp.rptAddReceipt(); 
            if (loanID != null)
            {
                var res = (new reportEntities()).vwDepositAdditionals.Where(p=>p.depositAdditionalID==loanID).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.Database.Tables[0].SetDataSource(res);
                rpt.Database.Tables[1].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                this.rvw.ReportSource = rpt;
            } 
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cboLoan.Items.Clear();
            if (cboClient.SelectedValue != "")
            { 
                coreReports.reportEntities rent = new reportEntities();
                var clientId = int.Parse(cboClient.SelectedValue);
                cboLoan.Items.Clear();
                cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in rent.vwDepositAdditionals.Where(p => p.clientID == clientId).OrderByDescending(p => p.depositDate).ToList())
                {
                    cboLoan.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.depositDate.ToString("dd-MMM-yyyy")
                        + " : " + cl.depositAmount.ToString("#,###.#0"), 
                        cl.depositAdditionalID.ToString()));
                }
            }
        }

        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 1
                        || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 5).Where(p =>
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
