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
using coreReports;

namespace coreERP.ln.analysis
{
    public partial class cashier : corePage
    {
        private List<string> batches;
        List<vwCashierDetail> tx;

        public override string URL
        {
            get { return "~/ln/analysis/cashier.aspx"; }
        }
         
        core_dbEntities ent;
        jnl_batch batch;
        reportEntities le;
        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
                ent = new core_dbEntities();
                le = new reportEntities();
                coreReports.reportEntities re = new coreReports.reportEntities();
                divError.Style["visibility"] = "hidden"; 
                if (!Page.IsPostBack)
                {
                    Session["selectedBatches"] = null;
                    Session["tx"] = null;
                    RenderTree();
                    var profile = ent.comp_prof.FirstOrDefault();
                    if(profile != null && profile.currency_id != null)
                        Session["currency_id"] = profile.currency_id;
                }
                else if (Session["batch"] != null)
                {
                    batch = Session["batch"] as jnl_batch;
                }
                LoadBatches();
                divProc.Style["visibility"] = "hidden";
            }
            catch (Exception x){
                ManageException(x);
            }
        }

        public void RadTreeView1_NodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                if (e.Node.Checked == true) e.Node.CheckChildNodes();
                else e.Node.UncheckChildNodes();
            }
            catch (Exception x)
            {
                ManageException(x);
            }
            btnPreview_Click(btnPreview, EventArgs.Empty);
        }
 
        private void RenderTree()
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                 
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
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
         
        public string AccountName(int jnl_id)
        {
            var item = (from c in ent.jnl
                        from m in ent.accts
                        where c.accts.acct_id == m.acct_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.acc_num,
                            m.acc_name
                        }).FirstOrDefault();
            return item==null?"":item.acc_num + " - " + item.acc_name;
        }

        public string CostCenterName(int jnl_id)
        {
            var item = (from c in ent.jnl
                        from m in ent.vw_gl_ou
                        where c.gl_ou.ou_id == m.ou_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.ou_name1
                        }).FirstOrDefault();
            return item == null ? "" : item.ou_name1;
        }
          
        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                var accs = (from a in ent.accts
                            from c in ent.currencies
                            where a.currencies.currency_id == c.currency_id
                            select new
                            {
                                a.acct_id,
                                a.acc_num,
                                a.acc_name,
                                c.major_name,
                                c.major_symbol
                            });
                cboAcc.Items.Clear();
                var profile = ent.comp_prof.FirstOrDefault();
                var numb4 = profile != null ? profile.num_b4_name : false;
                foreach (var cur in accs)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = numb4?cur.acc_num + " - " + cur.acc_name
                         : cur.acc_name + " - " + cur.acc_num;;
                    item.Value = cur.acct_id.ToString();

                    item.Attributes.Add("acc_num", cur.acc_num);
                    item.Attributes.Add("acc_name", cur.acc_name );
                    item.Attributes.Add("acc_cur", cur.major_name);

                    cboAcc.Items.Add(item);

                } 
                if (Session["acct_id"] != null && Session["acct_id"].ToString() != "0")
                    cboAcc.SelectedValue = Session["acct_id"].ToString();
                else
                    cboAcc.Text = " ";
            }
            catch (Exception ex) { }
        }

        protected void PopulateCostCenters(Telerik.Web.UI.RadComboBox cboCC)
        {
            try
            {
                var cc = (from a in ent.vw_gl_ou
                            select a);
                cboCC.Items.Clear();
                foreach (var cur in cc)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.ou_name1;
                    item.Value = cur.ou_id.ToString();

                    item.Attributes.Add("ou_name", cur.ou_name1);

                    cboCC.Items.Add(item);

                } 
                if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != "0")
                    cboCC.SelectedValue = Session["gl_ou_id"].ToString();
                else{
                    cboCC.Text = " ";
                }
            }
            catch (Exception ex) { }
        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
            {
                var item = e.Item as GridEditableItem;
                var rcb = item["acct_id"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateAccounts(rcb);
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAcc_SelectedIndexChanged);
                    rcb.DataBind();
                }
                rcb = item["gl_ou_id"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateCostCenters(rcb);
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboCC_SelectedIndexChanged);
                }
            }
        }

        void cboAcc_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["acct_id"] = e.Value;
        }

        void cboCC_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["gl_ou_id"] = e.Value;
        }

        protected void RadTreeView1_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            try
            {
                RadTreeNode clickedNode = e.Node;
                core_dbEntities ent = new core_dbEntities();

                switch (e.MenuItem.Value)
                {
                    case "Delete":
                        var itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "b")
                        {
                            string batchNo = e.Node.Value.Split(':')[1];
                            var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == batchNo);
                            if (b != null)
                            {
                                for (int i = b.jnl.Count() - 1; i >= 0; i--)
                                {
                                    b.jnl.Remove(b.jnl.ToList()[i]);
                                }
                                ent.jnl_batch.Remove(b);
                                ent.SaveChanges();
                            }
                            clickedNode.ExpandParentNodes();
                            var key = clickedNode.ParentNode.Value;
                            clickedNode.Remove();
                            RenderTree();
                            try
                            {
                                RadTreeView1.FindNodeByValue(key).ExpandChildNodes();
                            }
                            catch (Exception) { }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
         
        public void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                Button addButton = e.Item.FindControl("AddNewRecordButton") as Button;
                if (addButton != null)
                {
                    addButton.Visible = false;
                    LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                    if (lnkButton != null)
                    {
                        lnkButton.Visible = false;
                    }
                }
            }
            else if (e.Item is GridGroupFooterItem)
            {
                try
                {
                    //get the footer cell:
                    var descCell = (e.Item as GridGroupFooterItem)["naration"]; 
                    //find the group header for the current group:
                    var hdrs = RadGrid1.MasterTableView.GetItems(GridItemType.GroupHeader)
                        .Where(i => i.GroupIndex == e.Item.GroupIndex).ToList();
                    var headerItem = ((hdrs.Count) > 0 ? hdrs[0] : null) as GridGroupHeaderItem;
                     
                    string groupHeaderText = ((hdrs.Count) > 0 ? (hdrs[0] as GridGroupHeaderItem).DataCell.Text : "");
                    groupHeaderText = groupHeaderText + " total ";
                    //add group level to the text in the footer:
                    descCell.Text = groupHeaderText;
                }
                catch (Exception) { }
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                batches.Clear();
                tx.Clear();

                var js = le.vwCashierDetails.Where(p =>
                    (txtClientID.Text == "" || p.accountNumber.Contains(this.txtClientID.Text))
                    && (txtRefNo.Text == "" || p.loanNo.Contains(txtRefNo.Text))
                    && (dtpEndDate.SelectedDate == null || p.txDate <= dtpEndDate.SelectedDate)
                    && (dtStartDate.SelectedDate == null || p.txDate >= dtStartDate.SelectedDate)
                    && (txtDesc.Text == "" || p.naration.Contains(txtDesc.Text))
                    && (txtCreator.Text == "" || p.full_name.Contains(txtCreator.Text))
                    );
                foreach (var j in js)
                { 
                    tx.Add(j);
                }
                Session["selectedBatches"] = batches;
                Session["tx"] = tx;
                if (tx.Count > 0)
                {
                    pnlEditJournal.Visible = true;
                }
                else
                {
                    pnlEditJournal.Visible = false;
                }
                RadGrid1.DataSource = tx;
                RadGrid1.DataBind();
                RenderTree();
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                ManageException(ex);
            }
        }

        private void LoadBatches()
        {
            if (Session["selectedBatches"] != null)
            {
                batches = Session["selectedBatches"] as List<string>;
            }
            else
            {
                batches = new List<string>();
            }
            var parameter = "";
            for (int i = 0; i < batches.Count; i++)
            {
                if (i > 0)
                {
                    parameter = parameter + ",'" + batches[i] + "'";
                }
                else
                {
                    parameter = "'" + batches[i] + "'";
                }
            }
            if (Session["tx"] != null)
            {
                tx = Session["tx"] as List<vwCashierDetail>;
            }
            else
            {
                tx = new List<vwCashierDetail>();
                Session["tx"] = tx;
            }
        }

        protected void RadGrid1_Load(object sender, EventArgs e)
        {
            RadGrid1.DataSource = tx;
        }
         
    }
}
