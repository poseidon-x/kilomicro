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

namespace coreERP.ln.bogreports
{
    public partial class topnloans : corePage
    {
        ReportDocument rpt;
        coreLoansEntities le = new coreLoansEntities();
        public override string URL
        {
            get { return "~/ln/bogreports/topnloans.aspx"; }
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
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddDays(-1); 
                coreLoansEntities le = new coreLoansEntities(); 
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = DateTime.Now.AddYears(-10);
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(DateTime.Now.AddYears(-10), dtpEndDate.SelectedDate.Value, clientID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(DateTime.Now.AddYears(-10), dtpEndDate.SelectedDate.Value, null);
            } 
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                DateTime startDate = (DateTime)Session["startDate"];
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(startDate, endDate, clientID);
                }
                else
                {
                    Bind(startDate, endDate, null);
                }
            }
        }

        private void Bind(DateTime startDate, DateTime endDate, int? clientID)
        {
            rpt = new coreReports.ln.bog.rptTopNExposures();
            if (clientID == null)
            {
                var res = (new reportEntities()).GetvwLoans23(endDate, (int)txtNum.Value.Value).OrderByDescending(
                    p=>p.principalBalance).Take((int)txtNum.Value.Value).ToList();
                rpt.SetDataSource(res);
            }
            else
            {
                var res = (new reportEntities()).GetvwLoans23(
                    endDate,(int)txtNum.Value.Value).Where(
                    p => p.clientID == clientID.Value).ToList();
                rpt.SetDataSource(res);
            }
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "ADVANCES – "+txtNum.Value.Value.ToString("#,###")+" LARGEST EXPOSURES");
            rpt.SetParameterValue("period", "AS OF " + endDate.ToString("dd-MMM-yyyy"));
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
                foreach (var cl in le.clients.Where(p => 
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
