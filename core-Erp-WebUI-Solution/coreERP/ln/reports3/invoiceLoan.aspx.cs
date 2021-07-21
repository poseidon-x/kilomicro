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
    public partial class invoiceLoan : corePage
    {
        ReportDocument rpt;
        string categoryID = "";
        public override string URL
        {
            get { return "~/ln/reports/invoiceLoan.aspx"; }
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
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                if (categoryID == null) categoryID = "";
                Session["endDate"] = null;

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames + " - " + cl.staffNo, 
                        cl.staffID.ToString()));
                }

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }

                cboSupplier.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.suppliers.OrderBy(p => p.supplierName).ToList())
                {
                    cboSupplier.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.supplierName, r.supplierID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? staffID = null;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            var type = 2;
            Session["resInvoiceLoans"] = null;
            if(opt1.Checked==true)
            {
                type = 1;
            }
            if (cboStaff.SelectedValue != "")
            {
                staffID = int.Parse(cboStaff.SelectedValue);
                Session["staffID"] = staffID;
            }

            Session["type"] = type;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID, dtpEndDate.SelectedDate.Value);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null, dtpEndDate.SelectedDate.Value);
            }
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? staffID = null;
            if (Session["staffID"] != null)
            {
                staffID = int.Parse(Session["staffID"].ToString()); 
            }
            if (Session["endDate"] != null)
            {
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID, (DateTime)Session["endDate"]);
                }
                else
                {
                    Bind(null, dtpEndDate.SelectedDate.Value);
                }
            }
        }

        private void Bind(int? clientId, DateTime endDate)
        {
            var le = new coreLoansEntities();
            var categoryId = Request.Params["catID"];
            int? branchId = null;
            if (cboBranch.SelectedValue != "") branchId = int.Parse(cboBranch.SelectedValue);
            if (categoryId == null) categoryId = "";
            var cl = le.clients.Where(p =>  (branchId == null || p.branchID == branchId)).ToList();
            rpt = new coreReports.ln3.rptInvoiceLoan();

            var rent = new coreReports.reportEntities(); ;
            if (Session["resInvoiceLoans"] != null)
            {
                var res = Session["resInvoiceLoans"] as List<vwInvoiceLoanMaster>;
                rpt.SetDataSource(res);
            }
            else
            {
                int? supplierId = null;
                int? staffId = null;
                if (cboSupplier.SelectedValue != "") supplierId = int.Parse(cboSupplier.SelectedValue);

                var res = rent.vwInvoiceLoanMasters.Where(
                    p => ((p.disbursementDate >= dtpStartDate.SelectedDate) && (p.disbursementDate <= dtpEndDate.SelectedDate))
                    && (supplierId == null || p.supplierID == supplierId)
                    && (clientId == null || p.clientID == clientId)
                    && (opt1.Checked == true || p.amountPaid > 0)
                    && (chkOutstanding.Checked == false || p.amountDisbursed > p.amountPaid + 30)
                    ).ToList();
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
                rpt.Database.Tables[0].SetDataSource(res);
                Session["resInvoiceLoans"] = res;
            }
            rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList()); 
            rpt.SetParameterValue("reportTitle", "Invoice Loans b/n " + dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy") 
                + " and " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
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
            //if (staffID != null)
            //{
            //    rpt.SetParameterValue("staff", "Assigned to: " + cboStaff.SelectedItem.Text);
            //}
            //else
            //{
            //    rpt.SetParameterValue("staff", "");
            //} 
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
