using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data;
using coreReports;
using Telerik.Web.UI;
using System.Collections; 

namespace coreERP.gl.accounts
{
    public partial class open : corePage
    {
        public override string URL
        {
            get { return "~/gl/accounts/open.aspx"; }
        }

        core_dbEntities ent;
        jnl_batch_tmp batch;
        private List<vw_acc_bals> results;

        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
                if (!IsPostBack)
                {
                    divError.Style["visibility"] = "hidden";
                    PopulatePeriods();
                }
            }
            catch (Exception x){
                ManageException(x);
            }
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
            divError.Style["color"] = "red";
            spanError.InnerHtml = errorMsg;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
                ent = new core_dbEntities();
            ent.Database.CommandTimeout = 3000;

            var date = DateTime.Parse(cboPeriod.SelectedValue);
                var period = int.Parse(cboPeriod.Text);

                ent.open_period(period, date);
                ent.SaveChanges();
                PopulatePeriods();
                divError.Style["visibility"] = "visible";
                divError.InnerHtml = "Period " + period.ToString() + " re-opened successfully.";
                divError.Style["color"] = "green";
        }

        protected void PopulatePeriods()
        {
            try
            {
                core_dbEntities ent = new core_dbEntities();
                cboPeriod.DataSource = ent.acct_period.ToList();
                cboPeriod.DataBind();
                cboPeriod.Items.Insert(0, new RadComboBoxItem("Select Closed Period", null));
            }
            catch (Exception ex) { }
        }

    }
}
