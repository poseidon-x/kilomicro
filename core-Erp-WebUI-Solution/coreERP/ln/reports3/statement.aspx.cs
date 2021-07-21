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
    public partial class statement : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports3/statement.aspx"; }
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
                        dtpStartDate.SelectedDate = DateTime.Now;
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
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID);
                }
                else
                {
                    Bind( null);
                }
            }
        }

        private void Bind(int? clientID)
        {
            var categoryId = Request.Params["catID"];
            if (categoryId == null) categoryId = "";
            rpt = new coreReports.ln3.rptLoanActualStmt2();

            if (clientID == null)
            {
                return;
            }
            else
            {
                using (var rent = (new reportEntities()))
                {
                    rent.Database.CommandTimeout = 120000;
                    var endDate = dtpStartDate.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1);
                
                    var res = rent.vwLoanActualSchedules.Where(p => p.clientID == clientID)
                        .Where(p => p.date <= endDate)
                        .OrderBy(p => p.clientName)
                        .ThenByDescending(p => p.disbursementDate)
                        .ThenBy(p => p.date)
                        .ToList();
                    if (res.Count == 0)
                    {
                        HtmlHelper.MessageBox(
                            "Report cannot be displaid because no data was found for the provided criteria",
                            "coreERP©: No Data", IconType.deny);
                        return;
                    }

                    rpt.Database.Tables[0].SetDataSource(res);
                    rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
                }
                this.rvw.ReportSource = rpt;
            }
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

    }
}
