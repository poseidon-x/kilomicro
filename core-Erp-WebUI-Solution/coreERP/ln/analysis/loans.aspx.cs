using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.ln.analysis
{
    public partial class loans : corePage
    {
        private List<string> batches;
        List<jnl> tx;

        public override string URL
        {
            get { return "~/ln/analysis/loans.aspx"; }
        }
         
        core_dbEntities ent;
        jnl_batch batch;
        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        {  
                ent = new core_dbEntities(); 
                divError.Style["visibility"] = "hidden"; 
                if (!Page.IsPostBack)
                {
                    coreReports.reportEntities rent = new coreReports.reportEntities();
                    string staffID=null;
                    if (User.IsInRole("admin")==false){
                        var staff = (new coreLoansEntities()).staffs.FirstOrDefault(p=> p.userName.Trim()==User.Identity.Name.Trim());
                        if(staff==null)staffID="-100";
                        else staffID=staff.staffNo;
                    }
                    //var lns = rent.vwLoansByStaffs.Where(p => (staffID == null || p.Expr1 == staffID) && (p.totalBalance > 5)).ToList();
                    //RadGrid1.DataSource = lns;
                    //RadGrid1.DataBind();
                } 
                divProc.Style["visibility"] = "hidden"; 
        }
         
        private string MonthName(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "February";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;
                default:
                    monthName = "";
                    break;
            }

            return monthName;
        }
         
        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_journal_entries";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_journal_entries";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_journal_entries";
            this.RadGrid1.ExportSettings.Pdf.Title = "Currencies Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        private string toPlural(string item)
        {
            var rtr = item;
            var lastDigit = item.Substring(item.Length - 1, 1);
            var lastDigit2 = item.Substring(item.Length - 2, 2);
            if (lastDigit == "s" || lastDigit2 == "es") { }
            else if (lastDigit == "y" || lastDigit == "i")
            {
                rtr = item.Substring(0, item.Length - 1);
            }
            else if (lastDigit2 == "ch" || lastDigit2 == "sh")
            {
                rtr = item + "es";
            }
            else
            {
                rtr = item + "s";
            }
            return rtr;
        }

        private void ManageException(Exception ex)
        {
            string errorMsg = "There was an error processing your request:";
            if (ex is System.Data.Entity.Core.UpdateException)
            {
                if (ex.InnerException.Message.Contains("uk_acct_cat_name") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_code"))
                {
                    errorMsg += "<br />The Main Account Head you are trying to create already exist.";
                }
                if (ex.InnerException.Message.Contains("uk_acct_cat_max_acct_num") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_min_acct_num"))
                {
                    errorMsg += "<br />The Account number range specified overlaps another account head.";
                }
            }
            errorMsg += "Please correct and continue or cancel.";
            divError.Style["visibility"] = "visible";
            spanError.InnerHtml = errorMsg;
        }
          
        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
            {
            }
        }
           
        public void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            { 
            }
            else if (e.Item is GridGroupFooterItem)
            {
                try
                { 
                }
                catch (Exception) { }
            }
        }
         
        protected void RadGrid1_Load(object sender, EventArgs e)
        { 
            coreReports.reportEntities rent = new coreReports.reportEntities();
            string staffID = null;
            if (User.IsInRole("admin") == false)
            {
                var staff = (new coreLoansEntities()).staffs.FirstOrDefault(p => p.userName.Trim() == User.Identity.Name.Trim());
                if (staff == null) staffID = "-100";
                else staffID = staff.staffNo;
            }
            //var lns = rent.vwLoansByStaffs.Where(p => (staffID == null || p.Expr1 == staffID) && ( p.totalBalance>5)).ToList();
            //RadGrid1.DataSource = lns; 
        }
         
    }
}
