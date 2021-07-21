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
using Telerik.Web.UI;

namespace coreERP.ln.savingReports
{
    public partial class statement : corePage
    {
        ReportDocument rpt;
        private string catID = "";

        public override string URL
        {
            get { return "~/ln/savingReports/statement.aspx"; }
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
                 
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        { 
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                
                if (User.IsInRole("admin") == false)
                {
                    var accs = le.savings.Where(p => p.clientID == clientID).ToList();
                    foreach (var acc in accs)
                    {
                        var ssa = le.staffSavings.FirstOrDefault(p => p.savingID == acc.savingID);
                        if (ssa != null)
                        {
                            return;
                        }
                    }
                }

                Session["ClientID"] = clientID;
                Bind(clientID);
            }
            else
            {
                return;
                //Session["ClientID"] = null;
                //Bind(null);
            }
            Session["run"] = true; 
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["ClientID"] != null)
            {
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];

                    if (User.IsInRole("admin") == false)
                    {
                        var accs = le.savings.Where(p => p.clientID == clientID).ToList();
                        foreach (var acc in accs)
                        {
                            var ssa = le.staffSavings.FirstOrDefault(p => p.savingID == acc.savingID);
                            if (ssa != null)
                            {
                                return;
                            }
                        }
                    }

                    Bind(clientID);
                }
                else
                {
                    return;
                    //Bind(null);
                }
            }
            else if (Session["run"]!=null)
            {
                return;
                //Bind(null);
            }
        }

        private void Bind(int? clientID)
        {
            List<vwSavingStatement> res = new List<vwSavingStatement>();
            rpt = new coreReports.sv.rptSavingStatement();
            if (clientID == null)
            {
                res = (new reportEntities()).vwSavingStatements.OrderBy(p => p.loanID).OrderBy(p => p.date).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
            }
            else
            {
                res = (new reportEntities()).vwSavingStatements.Where(p => p.clientID == clientID).OrderBy(p => p.date).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
            }
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
             
            //rpt.SetParameterValue("reportTitle", "ACCOUNT STATEMENT ("+ +")"));
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
                        || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6 || p.clientTypeID == 7).Where(p =>
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
