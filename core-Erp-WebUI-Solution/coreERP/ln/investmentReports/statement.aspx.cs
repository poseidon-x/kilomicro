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
using coreERP.code;

namespace coreERP.ln.investmentReports
{
    public partial class statement : corePage
    {
        ReportDocument rpt;
                coreLoansEntities le = new coreLoansEntities();
        public override string URL
        {
            get { return "~/ln/investmentReports/statement.aspx"; }
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
            Session["run"] = true;
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
                    Bind(null);
                }
            }
            else if (Session["run"]!=null)
            {
                Bind(null);
            }
        }

        private void Bind(int? clientID)
        {
            rpt = new coreReports.iv.rptInvestmentStatement();
            if (clientID == null)
            {
                var res = (new reportEntities()).vwInvestmentStatements.OrderBy(p => p.loanID).OrderBy(p => p.date).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
            }
            else
            {
                var res = (new reportEntities()).vwInvestmentStatements.Where(p => p.clientID == clientID).OrderBy(p => p.date).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
            }
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            //rpt.SetParameterValue("reportTitle", "Loans Report as at " + endDate.ToString("dd-MMM-yyyy"));
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
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 3).Where(p =>
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