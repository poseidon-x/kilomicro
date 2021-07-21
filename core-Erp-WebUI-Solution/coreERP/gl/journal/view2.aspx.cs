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

namespace coreERP.gl.journal
{
    public partial class view2 : corePage
    {
        public override string URL
        {
            get { return "~/gl/journal/view2.aspx"; }
        }

        private void AddNew()
        {
            batch = new jnl_batch_tmp();
            batch.batch_no = "";
            batch.posted = false;
            batch.source = "J/E";
            ent.jnl_batch_tmp.Add(batch);
            Session["batch"] = batch;
            Session["tx_count"] = 0;
            dtTransactionDate.SelectedDate = DateTime.Now;
            txtBatchNumber.Text = "UNASSIGNED";
        }

        private void Edit(string batchNo)
        {
            try
            {
                batch = ent.jnl_batch_tmp.First(p => p.batch_no == batchNo);
                Session["batch"] = batch;
                Session["tx_count"] = ent.jnl_tmp.Count(p => p.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id);
                dtTransactionDate.SelectedDate = ent.jnl_tmp.First(p => p.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id).tx_date;
                txtBatchNumber.Text = batch.batch_no;
            }
            catch (Exception x)
            {
                postedBatch = ent.jnl_batch.First(p => p.batch_no == batchNo);
                Session["batchPosted"] = postedBatch;
               // postedBatch.jnl.Load();
                Session["tx_count"] = postedBatch.jnl.Count();
                dtTransactionDate.SelectedDate = postedBatch.jnl.First().tx_date;
                txtBatchNumber.Text = postedBatch.batch_no;
            }
            this.pnlEditJournal.Visible = true;
        }

        core_dbEntities ent;
        jnl_batch_tmp batch;
        jnl_batch postedBatch;

        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
                ent = new core_dbEntities();
                coreReports.reportEntities re = new coreReports.reportEntities();
                divError.Style["visibility"] = "hidden";
                RadTreeView1.NodeClick += new RadTreeViewEventHandler(RadTreeView1_NodeClick);
                if (!Page.IsPostBack)
                {
                    dtTransactionDate.SelectedDate = DateTime.Now.Date.AddDays(-1);
                    dtTransactionDate2.SelectedDate = DateTime.Now;
                } 
                else{
                    if (Session["lstBatch"] != null)
                    {
                       // RadGrid1.DataSource = Session["lstBatch"]; 
                    }
                }
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
        }

        public void RadTreeView1_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                var keys = e.Node.Value.Split(':');
                if (keys[0] == "b")
                {
                    Edit(keys[1]);
                }
            }
            catch (Exception x)
            {
                ManageException(x);
            }
        }

        private void RenderTree(List<jnl> listJnl)
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("G/L Journal Entries", "__root__"); 
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg"; 
                this.RadTreeView1.Nodes.Add(rootNode);
                var years = (from jnl in listJnl
                             orderby jnl.tx_date.Year descending 
                             select jnl.tx_date.Year).Distinct();
                foreach (var year in years)
                {
                    RadTreeNode node = new RadTreeNode(year.ToString(), "y:" + year.ToString());
                    node.Visible = true;
                    node.ImageUrl = "~/images/tree/folder_open.jpg";
                    node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg"; 
                    node.ToolTip = "Expand to see transactions for year: " + year.ToString();
                    node.Checkable = true;
                    rootNode.Nodes.Add(node);
                    var months = (from jnl in listJnl
                                 where jnl.tx_date.Year == year
                                  orderby jnl.tx_date.Month descending 
                                  select jnl.tx_date.Month).Distinct();
                    foreach (var month in months)
                    {
                        var monthName = MonthName(month);
                        RadTreeNode childnode = new RadTreeNode(monthName, "m:" + year.ToString() +
                            "," + month.ToString());
                        childnode.Visible = true;
                        childnode.ImageUrl = "~/images/tree/folder_open.jpg";
                        childnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        childnode.ToolTip = "Expand to see transactions for month: " + year.ToString() + 
                            ", " + monthName;
                        childnode.Checkable = true;
                        node.Nodes.Add(childnode);
                        var days = (from jnl in listJnl
                                      where jnl.tx_date.Year == year
                                        && jnl.tx_date.Month == month
                                      orderby jnl.tx_date.Day descending 
                                      select jnl.tx_date.Day).Distinct();
                        foreach (var day in days)
                        {
                            RadTreeNode gchildnode = new RadTreeNode(day.ToString(), "d:" + year.ToString() +
                            "," + month.ToString() + day.ToString());
                            gchildnode.Visible = true;
                            gchildnode.ImageUrl = "~/images/tree/folder_open.jpg";
                            gchildnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                            gchildnode.ToolTip = "Expand to see transactions for date: " + 
                                (new DateTime(year,month,day)).ToString("dd-MMM-yyyy");
                            gchildnode.Checkable = true;
                            childnode.Nodes.Add(gchildnode);
                            var batches = (from jnl in listJnl 
                                        where   jnl.tx_date.Year == year
                                            && jnl.tx_date.Month == month
                                            && jnl.tx_date.Day == day
                                        orderby jnl.ref_no
                                           select jnl.ref_no).Distinct();
                            foreach (var b in batches)
                            { 
                                    RadTreeNode gchildnode2 = new RadTreeNode(b, "b:" + b);
                                    gchildnode2.Visible = true; 
                                    gchildnode2.ImageUrl = "~/images/edit.jpg";
                                    gchildnode2.ExpandedImageUrl = "~/images/edit.jpg"; 
                                    gchildnode2.ToolTip = "Click to see transactions for batch no.: " + b;
                                    //gchildnode2.NavigateUrl = "~/gl/journal/local.aspx?op=edit&batchno=" +
                                    //    batchNo; 
                                    gchildnode2.PostBack = true;
                                    gchildnode2.Checkable = true;
                                    gchildnode.Nodes.Add(gchildnode2); 
                            }
                        }
                    }
                }
                rootNode.Expanded = true;

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
            var item = (from c in ent.jnl_tmp
                        from m in ent.accts
                        where c.accts.acct_id == m.acct_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.acc_num,
                            m.acc_name
                        }).FirstOrDefault();
            if (item == null)
            {
                var item2 = (from c in ent.jnl
                        from m in ent.accts
                        where c.accts.acct_id == m.acct_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.acc_num,
                            m.acc_name
                        }).FirstOrDefault();
                return item2 == null ? "" : item2.acc_num + " - " + item2.acc_name;
            }
            else
            {
                return item == null ? "" : item.acc_num + " - " + item.acc_name;
            }
        }

        public string CostCenterName(int jnl_id)
        {
            var item = (from c in ent.jnl_tmp
                        from m in ent.vw_gl_ou
                        where c.gl_ou.ou_id == m.ou_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.ou_name1
                        }).FirstOrDefault();
            if (item == null)
            {
                var item2 = (from c in ent.jnl_tmp
                            from m in ent.vw_gl_ou
                            where c.gl_ou.ou_id == m.ou_id
                                && c.jnl_id == jnl_id
                            select new
                            {
                                m.ou_name1
                            }).FirstOrDefault();
                return item2 == null ? "" : item2.ou_name1;
            }
            else
            {
                return item == null ? "" : item.ou_name1;
            }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var account_id = int.Parse(Session["acct_id"].ToString());
                accts acc = ent.accts.First<accts>(p => p.acct_id == account_id);
                jnl_tmp jnl = new jnl_tmp();
                jnl.description = newVals["description"].ToString();
                jnl.dbt_amt = double.Parse(newVals["dbt_amt"].ToString());
                jnl.crdt_amt = double.Parse(newVals["crdt_amt"].ToString());
                if(newVals["ref_no"]!=null && newVals["ref_no"].ToString() != "")
                    jnl.ref_no = newVals["ref_no"].ToString(); 
                jnl.accts = acc;
                if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                     && Session["gl_ou_id"].ToString() != "0")
                {
                    var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                    gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                    jnl.gl_ou = ou;
                }
                var currency_id = (int)Session["currency_id"];
                jnl.currencies = ent.currencies.Where(p => p.currency_id == currency_id).FirstOrDefault();
                jnl.tx_date = dtTransactionDate.SelectedDate.Value;
                jnl.creation_date = DateTime.Now;
                jnl.creator = User.Identity.Name;
                jnl.jnl_batch_tmp = batch.jnl_batch_id>0? ent.jnl_batch_tmp.First(p=>p.jnl_batch_id==batch.jnl_batch_id):batch;
                ent.jnl_tmp.Add(jnl);
                if (batch.batch_no == null || batch.batch_no == "")
                    this.txtBatchNumber.Text = coreExtensions.NextGLBatchNumber();
                batch.batch_no = this.txtBatchNumber.Text;
                batch.creation_date = DateTime.Now;
                batch.creator = User.Identity.Name;
                ent.SaveChanges(); //ent.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins,batch);
                Session["tx_count"] = (int)Session["tx_count"] + 1;
                batch = ent.jnl_batch_tmp.First(p => p.jnl_batch_id == batch.jnl_batch_id);
                Session["batch"] = batch; 
                Session["acct_id"] = 0;
                if ((int)Session["tx_count"] % 2 == 1)
                {
                    var freshVals = new System.Collections.Specialized.ListDictionary(); 
                    freshVals["ref_no"] = jnl.ref_no;
                    freshVals["acct_id"] = 0;
                    freshVals["tx_date"] = jnl.tx_date;
                    freshVals["description"] = jnl.description;
                    freshVals["currency_id"] = jnl.currencies.currency_id;
                    freshVals["rate"] = jnl.rate;
                    freshVals["dbt_amt"] = jnl.crdt_amt;
                    freshVals["crdt_amt"] = jnl.dbt_amt;
                    freshVals["creator"] = User.Identity.Name;
                    freshVals["creation_date"] = DateTime.Now;
                    RadGrid1.MasterTableView.InsertItem(freshVals);
                }
                else
                {
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                } 
            }
            catch (Exception ex) { ManageException(ex); }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var jnl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["jnl_id"];
                var jnl = (from p in ent.jnl_tmp where p.jnl_id == jnl_id select p).First<jnl_tmp>();

                var account_id = int.Parse(Session["acct_id"].ToString());
                accts acc = ent.accts.First<accts>(p => p.acct_id == account_id);

                jnl.description = newVals["description"].ToString();
                jnl.dbt_amt = double.Parse(newVals["dbt_amt"].ToString());
                jnl.crdt_amt = double.Parse(newVals["crdt_amt"].ToString());

                if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != "")
                {
                    var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                    gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                    jnl.gl_ou = ou;
                }
                
                if (newVals["ref_no"] != null && newVals["ref_no"].ToString() != "")
                    jnl.ref_no = newVals["ref_no"].ToString(); 
                jnl.accts = acc; 
                jnl.tx_date = dtTransactionDate.SelectedDate.Value;
                jnl.modification_date = DateTime.Now;
                jnl.last_modifier = User.Identity.Name;
                ent.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
                Session["acct_id"] = 0;
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_ItemInserted(object source, Telerik.Web.UI.GridInsertedEventArgs e)
        {
        }

        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                e.Canceled = true; 
            }
            else if (e.CommandName == "Edit")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                var jnl_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("jnl_id").ToString());
                Session["currency_id"] = (from c in ent.jnl_tmp
                                            from u in ent.currencies
                                            where c.currencies.currency_id == u.currency_id
                                                && c.jnl_id == jnl_id
                                            select new { u.currency_id }).FirstOrDefault().currency_id;
                Session["acct_id"] = (from c in ent.jnl_tmp
                                            from u in ent.accts
                                        where c.accts.acct_id == u.acct_id
                                                && c.jnl_id == jnl_id
                                        select new { u.acct_id }).FirstOrDefault().acct_id;
                var ou = (from c in ent.jnl_tmp
                                        from u in ent.vw_gl_ou
                                        where c.gl_ou.ou_id == u.ou_id
                                                && c.jnl_id == jnl_id
                                        select new { u.ou_id }).FirstOrDefault();
                if (ou != null) Session["gl_ou_id"]=ou.ou_id;
            }
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
                addButton.Visible = false;
                LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                lnkButton.Visible = false;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var startDate = dtTransactionDate.SelectedDate.Value;
            var endDate = dtTransactionDate2.SelectedDate.Value;
            var batchNo = txtBatchNumber.Text;
            var refNo = txtRefNo.Text;

            var lstJnl = ent.jnl.Where(p => p.tx_date >= startDate && p.tx_date <= endDate &&
                (batchNo == "" || p.jnl_batch.batch_no == batchNo) &&
                (refNo == "" || p.ref_no == refNo)).OrderBy(p => p.tx_date).ToList();
            var lstBatch = new List<jnl_batch>();
            foreach (var j in lstJnl)
            {
                //j.jnl_batchReference.Load();
                if (lstBatch.Contains(j.jnl_batch) == false)
                {
                    lstBatch.Add(j.jnl_batch);
                }
            }
            RenderTree(lstJnl);
            RadGrid1.DataSource = lstBatch;
            Session["lstBatch"] = lstBatch;
            RadGrid1.DataBind();
        }

        protected void RadGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem; 
            int jnlBatchID = int.Parse(dataItem.KeyValues.Split(':')[1].Replace("\"","").Replace("{","").Replace("}","")); 
            if (Session["lstBatch"] != null)
            {
                var lstBatch = Session["lstBatch"] as List<jnl_batch>;
                if (lstBatch != null)
                {
                    var batch = lstBatch.FirstOrDefault(p => p.jnl_batch_id == jnlBatchID);
                    if (batch != null)
                    {
                        e.DetailTableView.DataSource = batch.jnl;
                    }
                }
            }
        }
         
    }
}
