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

namespace coreERP.fa.reports
{
    public partial class assetDepreciation : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/fa/reports/assetDepreciation.aspx"; }
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
                coreLoansEntities le = new coreLoansEntities();
                core_dbEntities ent = new core_dbEntities();
                cboCat.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                dtStartDate.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
                dtEndDate.SelectedDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                foreach (var cl in le.assetCategories)
                {
                    cboCat.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.assetCategoryName , cl.assetCategoryID.ToString()));
                }
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames, cl.staffID.ToString()));
                }
                cboOU.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in ent.vw_ou)
                {
                    cboOU.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.ou_name, cl.ou_id.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? catID = null;
            int? staffID = null;
            int? ouID = null;

            if (cboCat.SelectedValue != "")
            {
                catID = int.Parse(cboCat.SelectedValue);
                Session["catID"] = catID; 
            }

            if (cboOU.SelectedValue != "")
            {
                ouID = int.Parse(cboOU.SelectedValue);
                Session["ouID"] = catID;
            }

            if (cboStaff.SelectedValue != "")
            {
                staffID = int.Parse(cboStaff.SelectedValue);
                Session["staffID"] = catID;
            }
            Bind(catID, ouID, staffID);
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? catID = null;
            int? staffID = null;
            int? ouID = null;

            if (Session["catID"] != null)
            {
                catID = int.Parse(Session["catID"].ToString()); 
            }

            if (Session["ouID"] != null)
            {
                ouID = int.Parse(Session["ouID"].ToString());
            }

            if (Session["staffID"] != null)
            {
                staffID = int.Parse(Session["staffID"].ToString());
            }
            Bind(catID, ouID, staffID);
        }

        private void Bind(int? catID,int? ouID, int? staffID)
        {
            if(chkCat.Checked==true)
                rpt = new coreReports.fa.rptAssetDepreciation();
            else
                rpt = new coreReports.fa.rptAssetDepreciationOU();
            var res = (new reportEntities()).getAssetDepreciation(dtStartDate.SelectedDate,dtEndDate.SelectedDate).Where(p => (catID == null || p.assetCategoryID==catID)
                && (ouID == null || p.ouID == ouID)
                && (staffID == null || p.staffid == staffID)
                ).OrderBy(p => p.assetDescription).ToList();
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Fixed Assets Register");
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
    }
}
