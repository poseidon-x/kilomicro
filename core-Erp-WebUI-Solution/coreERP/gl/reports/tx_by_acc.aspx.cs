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
using Telerik.Web.UI;
using coreLogic;
using coreERP.code;

namespace coreERP.gl.reports
{
    public partial class tx_by_acc : corePage
    {
        ReportDocument rpt;

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

        public override string URL
        {
            get { return "~/gl/reports/tx_by_acc.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1, 23, 59, 59).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1, 0, 0, 0);
                //PopulateAccounts();
                PopulateCurrencies();
                PopulateCostCenters();
            }
        }

        protected void PopulateCostCenters()
        {
            try
            {
                core_dbEntities ent = new core_dbEntities();
                var cc = (from a in ent.vw_gl_ou
                          select a);
                cboCC.DataSource = cc.ToList();
                cboCC.DataBind();
                cboCC.Items.Insert(0, new RadComboBoxItem("Cost Center", null));
            }
            catch (Exception ex) { }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["frgn"] = chkForeign.Checked;
            Session["gl_ou_id"] = (cboCC.SelectedValue != null && cboCC.SelectedValue != "") ? cboCC.SelectedValue : null;
            Session["acct_id"] = (cboAcc.SelectedValue != null && cboAcc.SelectedValue != "") ? cboAcc.SelectedValue : null;
            Session["currency_id"] = (cboCur.SelectedValue != null && cboCur.SelectedValue!="") ? cboCur.SelectedValue : null;
            Session["batchNo"] = (txtBatchNo.Text.Trim() != "") ? txtBatchNo.Text : null;
            Session["refNo"] = (txtRefNo.Text.Trim() != "") ? txtRefNo.Text : null;
            Session["res_tx"] = null;
            Bind();
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                Bind();
            }
        }

        private void Bind()
        {
            DateTime? startDate = null;
            DateTime? endDate = null;
            bool? foreign = null;
            string batchNo = null;
            string refNo = null;
            int? currency_id = null;
            int? acct_id = null;
            int? gl_ou_id = null;
            if (Session["endDate"] != null)
            {
                endDate = (DateTime)Session["endDate"];
            }
            if (Session["startDate"] != null)
            {
                startDate = (DateTime)Session["startDate"];
            }
            if (Session["frgn"] != null)
            {
                foreign = (bool)Session["frgn"];
            }
            if (Session["batch_no"] != null)
            {
                batchNo = Session["batch_no"].ToString();
            }
            if (Session["acct_id"] != null)
            {
                acct_id = int.Parse(Session["acct_id"].ToString());
            }
            if (Session["currency_id"] != null)
            {
                currency_id = int.Parse(Session["currency_id"].ToString());
            }
            if (Session["gl_ou_id"] != null)
            {
                gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
            } 
            if (Session["batchNo"] != null)
            {
                batchNo =  Session["batchNo"].ToString();
            }
            if (Session["refNo"] != null)
            {
                refNo = Session["refNo"].ToString();
            }
            rpt = new coreReports.gl.fin_stmt.rptTxByAcct();
            var re = (new reportEntities());
            re.Database.CommandTimeout = 300000;
            //re.CommandTimeout = 999999;
            List<vw_tx_by_acct> res;
            if (Session["res_tx"] == null)
            {
                res = re.get_tx_by_acct(
                   acct_id, startDate, endDate, null, foreign, 7, batchNo, currency_id, refNo, gl_ou_id).ToList<vw_tx_by_acct>();
                Session["res_tx"] = res;
            }
            else
                res = Session["res_tx"] as List<vw_tx_by_acct>;
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Transactions By Account");
            this.rvw.ReportSource = rpt;
        }

        protected void PopulateAccounts()
        {
            try
            {
                core_dbEntities ent = new core_dbEntities();
                var accs = (from a in ent.accts
                            from c in ent.currencies
                            where a.currencies.currency_id == c.currency_id
                            orderby a.acc_num
                            select new
                            {
                                a.acct_id,
                                a.acc_num,
                                a.acc_name,
                                c.major_name,
                                c.major_symbol
                            }).ToList();
                cboAcc.DataSource = accs;
                cboAcc.DataBind();
                cboAcc.Items.Insert(0, new RadComboBoxItem("Transaction Account", null));
            }
            catch (Exception ex) { }
        }

        protected void PopulateCurrencies()
        {
            try
            {
                core_dbEntities ent = new core_dbEntities();
                cboCur.DataSource=ent.currencies.ToList();
                cboCur.DataBind();
                cboCur.Items.Insert(0, new RadComboBoxItem("Transaction Currency", null));
            }
            catch (Exception ex) { }
        }


        protected void cboCC_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            PopulateCostCenters();
        }
                
        protected void cboCur_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            PopulateCurrencies();
        }

        protected void cboAcc_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            PopulateAccounts();

        }
        protected void cboGLAcc_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                var cbo = sender as RadComboBox;
                if (e.Text.Trim().Length > 2 && cbo != null)
                {
                    using (core_dbEntities ent = new core_dbEntities())
                    {
                        var accs = (from a in ent.vw_accounts
                                    from c in ent.currencies
                                    where (a.acc_name.Contains(e.Text) || a.cat_name.Contains(e.Text) || a.head_name1.Contains(e.Text)
                                        || a.head_name2.Contains(e.Text) || a.head_name3.Contains(e.Text))
                                        && (a.currency_id == c.currency_id)
                                    select new
                                    {
                                        a.acct_id,
                                        a.acc_num,
                                        a.acc_name,
                                        major_name = c.major_name,
                                        a.fullname
                                    }).ToList();
                        cbo.DataSource = accs;
                        cbo.DataBind();
                        cbo.DataTextField = "fullname";
                        cbo.DataValueField = "acct_id";
                    }
                }
            }
            catch (Exception ex) { }
        } 

    }
}
