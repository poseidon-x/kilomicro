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
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.ln.reports
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
            get { return "~/ln/reports/tx_by_acc.aspx"; }
        }

                coreLogic.coreLoansEntities le = new coreLoansEntities();
                string categoryID = "";
        protected void Page_Load(object sender, EventArgs e)
                {
                    categoryID = Request.Params["catID"];
                    if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                rbDetailed.Checked = true;
                dtpEndDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1, 23, 59, 59).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1, 0, 0, 0);
                PopulateAccounts();
                PopulateCostCenters();
                PopulateCurrencies(); 

                cboLoanType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var c in le.loanTypes.OrderBy(p => p.loanTypeName).ToList())
                {
                    cboLoanType.Items.Add(new RadComboBoxItem(c.loanTypeName,
                        c.loanTypeID.ToString()));
                }

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
            }
            if (Session["selectedID"] == rbDetailed.ID)
            {
                rbDetailed.Checked = true;
            }
            else if (Session["selectedID"] == rbGrouped.ID)
            {
                rbGrouped.Checked = true;
            }
            else if (Session["selectedID"] == rbFiltered.ID)
            {
                rbFiltered.Checked = true;
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1);
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["frgn"] = chkForeign.Checked;
            Session["gl_ou_id"] = (cboCC.SelectedValue != null && cboCC.SelectedValue != "") ? cboCC.SelectedValue : null;
            Session["acct_id"] = (cboAcc.SelectedValue != null && cboAcc.SelectedValue != "") ? cboAcc.SelectedValue : null;
            Session["currency_id"] = (cboCur.SelectedValue != null && cboCur.SelectedValue!="") ? cboCur.SelectedValue : null;
            Session["batchNo"] = (txtBatchNo.Text.Trim() != "") ? txtBatchNo.Text : null;
            Session["refNo"] = (txtRefNo.Text.Trim() != "") ? txtRefNo.Text : null;
            Session["clientID"] = (cboClient.SelectedValue != null && cboClient.SelectedValue != "") ? cboClient.SelectedValue : null;
            Session["loanTypeID"] = (cboLoanType.SelectedValue != null && cboLoanType.SelectedValue != "") ? cboLoanType.SelectedValue : null;
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
            int? clientID = null;
            int? loanTypeID = null;
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
            if (Session["clientID"] != null)
            {
                clientID = int.Parse(Session["clientID"].ToString());
            }
            if (Session["loanTypeID"] != null)
            {
                loanTypeID = int.Parse(Session["loanTypeID"].ToString());
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
            if (rbGrouped.Checked == true)
            {
                rpt = new coreReports.ln.rptTxByAcct();
            }
            else if (rbDetailed.Checked == true)
            {
                rpt = new coreReports.ln.rptTxByAcct2();
            }
            else if (rbFiltered.Checked == true)
            {
                rpt = new coreReports.ln.rptTxByAcct3();
            }
            var re = (new reportEntities());
            //re.CommandTimeout = 999999;
            List<vwLoanTx> res;
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => (branchID == null || p.branchID == branchID)).ToList();
            if (Session["res_tx"] == null)
            {
                res = re.vwLoanTxes.Where(p=>
                        (p.tx_date >= startDate && p.tx_date<= endDate)
                        && (acct_id == null || p.acct_id == acct_id)
                        && (refNo == null || p.ref_no == refNo)
                        && (batchNo == null || p.batch_no == batchNo)
                        && (loanTypeID == null || p.loanTypeID == loanTypeID)
                        && (clientID == null || p.clientID == clientID)
                    ).OrderBy(p => p.tx_date).ToList();
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                Session["res_tx"] = res;
            }
            else
                res = Session["res_tx"] as List<vwLoanTx>;
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Loan G/L Transactions By Account");
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

        protected void cboCur_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            PopulateCurrencies();
        }

        protected void cboAcc_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            PopulateAccounts();
        }

        protected void cboCC_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            PopulateCostCenters();
        }

        protected void rbDetailed_CheckedChanged(object sender, EventArgs e)
        {
            Session["selectedID"] = (sender as RadioButton).ID;
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
