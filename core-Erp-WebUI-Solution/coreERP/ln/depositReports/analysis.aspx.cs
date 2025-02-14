﻿using System;
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
using Telerik.Web.UI;
using coreERP.code;

namespace coreERP.ln.depositReports
{
    public partial class analysis : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/depositReports/analysis.aspx"; }
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
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = DateTime.Now.Date;

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? staffID = null;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            var type = 2;
            Session["resDepositBalances"] = null;
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
            rpt = new coreReports.dp.rptDepositAnalysis();
            var rent = new reportEntities();

            if (Session["resDepositBalances"] != null)
            {
                var res = Session["resDepositBalances"] as List<coreReports.getDepositBalanceReport_Result>;
                rpt.SetDataSource(res);
            }
            else
            {
                var categoryID = Request.Params["catID"];
                int? branchID = null;
                if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
                if (categoryID == null) categoryID = "";
                var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                    && (branchID == null || p.branchID == branchID)).ToList();
                if (clientID == null )
                {
                    var res = (rent.getDepositBalanceReport(endDate).ToList());
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
                    Session["resDepositBalances"] = res;
                } 
                else if (clientID != null)
                {
                    var res = (rent.getDepositBalanceReport(endDate).Where(p => p.clientID == clientID).ToList());
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
                    Session["resDepositBalances"] = res;
                }
            }
            rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Deposit Analysis Report as at " + endDate.ToString("dd-MMM-yyyy"));
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
