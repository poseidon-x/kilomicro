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
    public partial class foreign : corePage
    {
        public override string URL
        {
            get { return "~/gl/journal/foreign.aspx"; }
        }

        private void AddNew()
        {
            batch = new jnl_batch_tmp();
            batch.batch_no = "";
            batch.posted = false;
            batch.source = "J/E";
            batch.multi_currency = true;
            ent.jnl_batch_tmp.Add(batch);
            Session["batch"] = batch;
            Session["tx_count"] = 0;
            dtTransactionDate.SelectedDate = DateTime.Now;
            txtBatchNumber.Text = "UNASSIGNED";
            txtCreator.Text = Context.User.Identity.Name;
            txtBatchState.Text = "UNPOSTED";
            try
            {
                this.EntityDataSource1.WhereParameters["batch_id"].DefaultValue = batch.jnl_batch_id.ToString();
            }
            catch (Exception ex)
            {
                this.EntityDataSource1.WhereParameters["batch_id"].DefaultValue = "0";
            }
            this.RadGrid1.Visible = true;
            this.RadGrid2.Visible = false; 
        }

        private void Edit(string batchNo)
        {
            try
            {
                batch = ent.jnl_batch_tmp.First(p => p.batch_no == batchNo);
                if (batch.posted == true && ent.comp_prof.FirstOrDefault().edit_posted_jnl == false)
                {
                    EditPosted(batchNo);
                    return;
                }
                Session["batch"] = batch;
                Session["tx_count"] = ent.jnl_tmp.Count(p => p.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id);
                dtTransactionDate.SelectedDate = ent.jnl_tmp.First(p => p.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id).tx_date;
                txtBatchNumber.Text = batch.batch_no;
                txtCreator.Text = batch.creator;
                txtBatchState.Text = "UNPOSTED";
                try
                {
                    this.EntityDataSource1.WhereParameters["batch_id"].DefaultValue = batch.jnl_batch_id.ToString();
                }
                catch (Exception ex)
                {
                    this.EntityDataSource1.WhereParameters["batch_id"].DefaultValue = "0";
                }
                this.RadGrid1.Visible = true;
                this.RadGrid2.Visible = false;
            }
            catch (InvalidOperationException ix)
            {
                EditPosted(batchNo);
            }
        }

        private void EditPosted(string batchNo)
        {
            batchPost = ent.jnl_batch.First(p => p.batch_no == batchNo);
            Session["batchPost"] = batch;
            Session["tx_count"] = ent.jnl.Count(p => p.jnl_batch.jnl_batch_id == batchPost.jnl_batch_id);
            dtTransactionDate.SelectedDate = ent.jnl.First(p => p.jnl_batch.jnl_batch_id == batchPost.jnl_batch_id).tx_date;
            txtBatchNumber.Text = batchPost.batch_no;
            txtCreator.Text = batchPost.creator;
            txtBatchState.Text = "POSTED";
            try
            {
                this.EntityDataSource2.WhereParameters["batch_id"].DefaultValue = batchPost.jnl_batch_id.ToString();
            }
            catch (Exception ex)
            {
                this.EntityDataSource2.WhereParameters["batch_id"].DefaultValue = "0";
            }
            this.RadGrid1.Visible = false;
            this.RadGrid2.Visible = true;
        }

        core_dbEntities ent;
        jnl_batch_tmp batch;
        jnl_batch batchPost;
        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ent = new core_dbEntities();
                divError.Style["visibility"] = "hidden";
                RadTreeView1.NodeClick += new RadTreeViewEventHandler(RadTreeView1_NodeClick);
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["op"] == null || Request.QueryString["op"] == "" ||
                         Request.QueryString["op"] == "new")
                    {
                        AddNew();
                    }
                    else if (Request.QueryString["op"] == "edit")
                    {
                        if (Request.QueryString["posted"] == "true")
                        {
                            EditPosted(Request.QueryString["batchno"]);
                        }
                        else
                        {
                            Edit(Request.QueryString["batchno"]);
                        }
                    }
                    RenderTree();
                    var profile = ent.comp_prof.FirstOrDefault();
                    if(profile != null && profile.currency_id != null)
                        Session["currency_id"] = profile.currency_id;
                }
                else if (Session["batch"] != null)
                {
                    batch = Session["batch"] as jnl_batch_tmp;
                }
                else if (Session["batchPost"] != null)
                {
                    batchPost = Session["batchPost"] as jnl_batch;
                }
                try
                {
                    this.EntityDataSource1.WhereParameters["batch_id"].DefaultValue = batch.jnl_batch_id.ToString();
                    //this.EntityDataSource2.WhereParameters["batch_id"].DefaultValue = batchPost.jnl_batch_id.ToString();
                }
                catch (Exception ex)
                {
                    this.EntityDataSource1.WhereParameters["batch_id"].DefaultValue = "0";
                }
                divProc.Style["visibility"] = "hidden";
            }
            catch (Exception x){
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

        private void RenderTree()
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("G/L Journal Entries", "__root__"); 
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg"; 
                this.RadTreeView1.Nodes.Add(rootNode);
                var years = (from jnl in ent.jnl_tmp
                             from batch in ent.jnl_batch_tmp
                             where jnl.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id
                                 && batch.multi_currency == true
                             orderby jnl.tx_date.Year descending 
                             select jnl.tx_date.Year).Distinct();
                foreach (var year in years)
                {
                    RadTreeNode node = new RadTreeNode(year.ToString(), "y:" + year.ToString());
                    node.Visible = true;
                    node.ImageUrl = "~/images/tree/folder_open.jpg";
                    node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg"; 
                    node.ToolTip = "Expand to see transactions for year: " + year.ToString();
                    rootNode.Nodes.Add(node);
                    var months = (from jnl in ent.jnl_tmp
                                  from batch in ent.jnl_batch_tmp
                                  where jnl.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id
                                      && batch.multi_currency == true
                                      && jnl.tx_date.Year == year
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
                        node.Nodes.Add(childnode);
                        var days = (from jnl in ent.jnl_tmp
                                    from batch in ent.jnl_batch_tmp
                                    where jnl.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id
                                        && batch.multi_currency == true
                                        && jnl.tx_date.Year == year
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
                            childnode.Nodes.Add(gchildnode);
                            var batches = (from jnl in ent.jnl_tmp
                                      from batch in ent.jnl_batch_tmp
                                        where jnl.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id 
                                            && batch.multi_currency==true 
                                            && jnl.tx_date.Year == year
                                            && jnl.tx_date.Month == month
                                            && jnl.tx_date.Day == day
                                        orderby batch.batch_no 
                                        select batch).Distinct();
                            foreach (var b in batches)
                            {
                                if (b.AllowedOU(Context.User.Identity.Name))
                                {
                                    RadTreeNode gchildnode2 = new RadTreeNode(b.batch_no, "b:" + b.batch_no + ":u");
                                    gchildnode2.Visible = true;
                                    if (b.is_valid())
                                    {
                                        gchildnode2.ImageUrl = "~/images/editNew.jpg";
                                        gchildnode2.ExpandedImageUrl = "~/images/editNew.jpg";
                                    }
                                    else
                                    {
                                        gchildnode2.ImageUrl = "~/images/deleteNew.jpg";
                                        gchildnode2.ExpandedImageUrl = "~/images/deleteNew.jpg";
                                    }
                                    gchildnode2.ToolTip = "Click to see transactions for batch no.: " + b.batch_no;
                                    //gchildnode2.NavigateUrl = "~/gl/journal/local.aspx?op=edit&batchno=" +
                                    //    batchNo; 
                                    gchildnode2.PostBack = true;
                                    gchildnode.Nodes.Add(gchildnode2);
                                    if (batch != null && b.batch_no == batch.batch_no)
                                    {
                                        gchildnode.Expanded = true;
                                        childnode.Expanded = true;
                                        node.Expanded = true;
                                    }
                                }
                            }
                        }
                    }
                }
                years = (from j in ent.jnl
                         from batch in ent.jnl_batch
                         where j.jnl_batch.jnl_batch_id == batch.jnl_batch_id
                             && batch.multi_currency == true
                         orderby j.tx_date.Year descending
                         select j.tx_date.Year).Distinct();
                foreach (var year in years)
                {
                    RadTreeNode node = rootNode.Nodes.FindNode(p=>p.Value=="y:" + year.ToString());
                    if (node == null)
                    {
                        node = new RadTreeNode(year.ToString(), "y:" + year.ToString());
                        node.Visible = true;
                        node.ImageUrl = "~/images/tree/folder_open.jpg";
                        node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        node.ToolTip = "Expand to see transactions for year: " + year.ToString();
                        rootNode.Nodes.Add(node);
                    }
                    var months = (from j in ent.jnl
                                  from batch in ent.jnl_batch
                                  where j.jnl_batch.jnl_batch_id == batch.jnl_batch_id
                                      && batch.multi_currency == true
                                      && j.tx_date.Year == year
                                  orderby j.tx_date.Month descending
                                  select j.tx_date.Month).Distinct();
                    foreach (var month in months)
                    {
                        var monthName = MonthName(month);
                        RadTreeNode childnode = node.Nodes.FindNode(p => p.Value == "m:" + year.ToString() +
                            "," + month.ToString());
                        if (childnode == null)
                        {
                            childnode = new RadTreeNode(monthName, "m:" + year.ToString() +
                                "," + month.ToString());
                            childnode.Visible = true;
                            childnode.ImageUrl = "~/images/tree/folder_open.jpg";
                            childnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                            childnode.ToolTip = "Expand to see transactions for month: " + year.ToString() +
                                ", " + monthName;
                            node.Nodes.Add(childnode);
                        }
                        var days = (from j in ent.jnl
                                    from batch in ent.jnl_batch
                                    where j.jnl_batch.jnl_batch_id == batch.jnl_batch_id
                                        && batch.multi_currency == true
                                        && j.tx_date.Year == year
                                      && j.tx_date.Month == month
                                    orderby j.tx_date.Day descending
                                    select j.tx_date.Day).Distinct();
                        foreach (var day in days)
                        {
                            RadTreeNode gchildnode = childnode.Nodes.FindNode(p => p.Value == "d:" + year.ToString() +
                            "," + month.ToString() + day.ToString());
                            if (gchildnode == null)
                            {
                                gchildnode = new RadTreeNode(day.ToString(), "d:" + year.ToString() +
                                    "," + month.ToString() + day.ToString());
                                gchildnode.Visible = true;
                                gchildnode.ImageUrl = "~/images/tree/folder_open.jpg";
                                gchildnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                                gchildnode.ToolTip = "Expand to see transactions for date: " +
                                    (new DateTime(year, month, day)).ToString("dd-MMM-yyyy");
                                childnode.Nodes.Add(gchildnode);
                            }
                            var batches = (from j in ent.jnl
                                           from batch in ent.jnl_batch
                                           where j.jnl_batch.jnl_batch_id == batch.jnl_batch_id
                                                && batch.multi_currency==true
                                               && j.tx_date.Year == year
                                               && j.tx_date.Month == month
                                               && j.tx_date.Day == day
                                           orderby batch.batch_no
                                           select batch).Distinct();
                            foreach (var b in batches)
                            {
                                if (b.AllowedOU(Context.User.Identity.Name))
                                {
                                    RadTreeNode gchildnode2 = gchildnode.Nodes.FindNode(p => p.Value == "b:" + b.batch_no);
                                    if (gchildnode2 == null)
                                    {
                                        gchildnode2 = new RadTreeNode(b.batch_no, "b:" + b.batch_no + ":p");
                                        gchildnode2.Visible = true;
                                        if (b.is_valid())
                                        {
                                            gchildnode2.ImageUrl = "~/images/edit.jpg";
                                            gchildnode2.ExpandedImageUrl = "~/images/edit.jpg";
                                        }
                                        else
                                        {
                                            gchildnode2.ImageUrl = "~/images/delete.jpg";
                                            gchildnode2.ExpandedImageUrl = "~/images/delete.jpg";
                                        }
                                        gchildnode2.ToolTip = "Click to see transactions for batch no.: " + b.batch_no;
                                        //gchildnode2.NavigateUrl = "~/gl/journal/local.aspx?op=edit&batchno=" +
                                        //    batchNo; 
                                        gchildnode2.PostBack = true;
                                        gchildnode.Nodes.Add(gchildnode2);
                                        if (batch != null && b.batch_no == batch.batch_no)
                                        {
                                            gchildnode.Expanded = true;
                                            childnode.Expanded = true;
                                            node.Expanded = true;
                                        }
                                    }
                                }
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
                item = (from c in ent.jnl
                        from m in ent.accts
                        where c.accts.acct_id == m.acct_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.acc_num,
                            m.acc_name
                        }).FirstOrDefault();
            }
            return item==null?"":item.acc_num + " - " + item.acc_name;
        }

        public string CurrencyName(int jnl_id)
        {
            var item = (from c in ent.jnl_tmp
                        from m in ent.currencies
                        where c.currencies.currency_id == m.currency_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.major_name,
                            m.major_symbol
                        }).FirstOrDefault();
            if (item == null)
            {
                item = (from c in ent.jnl
                        from m in ent.currencies
                        where c.currencies.currency_id == m.currency_id
                            && c.jnl_id == jnl_id
                        select new
                        {
                            m.major_name,
                            m.major_symbol
                        }).FirstOrDefault();
            }
            return item == null ? "" : item.major_name;
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
            return item == null ? "" : item.ou_name1;
        }

        public string AccountName2(int jnl_id)
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
            return item == null ? "" : item.acc_num + " - " + item.acc_name;
        }

        public string CostCenterName2(int jnl_id)
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

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var txDate = (DateTime)newVals["tx_date"];
                if (txDate.CanClose())
                {
                    var account_id = int.Parse(Session["acct_id"].ToString());
                    accts acc = ent.accts.First<accts>(p => p.acct_id == account_id);
                    var currency_id = int.Parse(Session["currency_id"].ToString());
                    currencies cur = ent.currencies.First<currencies>(p => p.currency_id == currency_id);
                    jnl_tmp jnl = new jnl_tmp();
                    jnl.description = newVals["description"].ToString();
                    jnl.frgn_dbt_amt = double.Parse(newVals["frgn_dbt_amt"].ToString());
                    jnl.frgn_crdt_amt = double.Parse(newVals["frgn_crdt_amt"].ToString());
                    jnl.rate = double.Parse(newVals["rate"].ToString());
                    jnl.dbt_amt = jnl.frgn_dbt_amt * jnl.rate;
                    jnl.crdt_amt = jnl.frgn_crdt_amt * jnl.rate;
                    jnl.currencies = cur;
                    if (newVals["ref_no"] != null && newVals["ref_no"].ToString() != "")
                        jnl.ref_no = newVals["ref_no"].ToString();
                    jnl.accts = acc;
                    if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                         && Session["gl_ou_id"].ToString() != "0")
                    {
                        var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                        gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                        jnl.gl_ou = ou;
                    }
                    jnl.tx_date = txDate;
                    jnl.creation_date = DateTime.Now;
                    jnl.creator = User.Identity.Name;
                    jnl.jnl_batch_tmp = batch.jnl_batch_id > 0 ? ent.jnl_batch_tmp.First(p => p.jnl_batch_id == batch.jnl_batch_id) : batch;
                    ent.jnl_tmp.Add(jnl);
                    if (batch.batch_no == null || batch.batch_no == "")
                        this.txtBatchNumber.Text = coreExtensions.NextGLBatchNumber();
                    batch.batch_no = this.txtBatchNumber.Text;
                    batch.creation_date = DateTime.Now;
                    batch.creator = User.Identity.Name;
                    ent.SaveChanges(); //ent.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, batch);
                    Session["tx_count"] = (int)Session["tx_count"] + 1;
                    batch = ent.jnl_batch_tmp.First(p => p.jnl_batch_id == batch.jnl_batch_id);
                    Session["batch"] = batch;
                    this.EntityDataSource1.WhereParameters["batch_id"].DefaultValue = batch.jnl_batch_id.ToString();
                    Session["acct_id"] = 0;
                    Session["currency_id"] = 0;
                    if ((int)Session["tx_count"] % 2 == 1)
                    {
                        var freshVals = new System.Collections.Specialized.ListDictionary();
                        freshVals["ref_no"] = jnl.ref_no;
                        freshVals["acct_id"] = 0;
                        freshVals["tx_date"] = jnl.tx_date;
                        freshVals["description"] = jnl.description;
                        freshVals["currency_id"] = jnl.currencies.currency_id;
                        freshVals["rate"] = 0.0;
                        freshVals["dbt_amt"] = 0.0;
                        freshVals["crdt_amt"] = 0.0;
                        freshVals["frgn_dbt_amt"] = 0.0;
                        freshVals["frgn_crdt_amt"] = 0.0;
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
                    RenderTree();
                }
                else
                {
                    divError.Style["visibility"] = "visible";
                    spanError.InnerHtml = "The selected date belongs to a closed period. Please change.";
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
                var txDate = (DateTime)newVals["tx_date"];
                if (txDate.CanClose())
                {
                    var jnl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["jnl_id"];
                    var jnl = (from p in ent.jnl_tmp where p.jnl_id == jnl_id select p).First<jnl_tmp>();

                    var account_id = int.Parse(Session["acct_id"].ToString());
                    accts acc = ent.accts.First<accts>(p => p.acct_id == account_id);

                    var currency_id = int.Parse(Session["currency_id"].ToString());
                    currencies cur = ent.currencies.First<currencies>(p => p.currency_id == currency_id);

                    jnl.description = newVals["description"].ToString();
                    jnl.frgn_dbt_amt = double.Parse(newVals["frgn_dbt_amt"].ToString());
                    jnl.frgn_crdt_amt = double.Parse(newVals["frgn_crdt_amt"].ToString());
                    jnl.rate = double.Parse(newVals["rate"].ToString());
                    jnl.dbt_amt = jnl.frgn_dbt_amt * jnl.rate;
                    jnl.crdt_amt = jnl.frgn_crdt_amt * jnl.rate;
                    jnl.currencies = cur;

                    if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                        && Session["gl_ou_id"].ToString() != "")
                    {
                        var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                        gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                        jnl.gl_ou = ou;
                    }

                    if (newVals["ref_no"] != null && newVals["ref_no"].ToString() != "")
                        jnl.ref_no = newVals["ref_no"].ToString();
                    jnl.accts = acc;
                    jnl.tx_date = (DateTime)newVals["tx_date"];
                    jnl.modification_date = DateTime.Now;
                    jnl.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                    Session["acct_id"] = 0;
                    Session["currency_id"] = 0;
                }
                else
                {
                    divError.Style["visibility"] = "visible";
                    spanError.InnerHtml = "The selected date belongs to a closed period. Please change.";
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid2_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var txDate = (DateTime)newVals["tx_date"];
                if (txDate.CanClose())
                {
                    var jnl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["jnl_id"];
                    var jnl = (from p in ent.jnl where p.jnl_id == jnl_id select p).First<jnl>();

                    var account_id = int.Parse(Session["acct_id"].ToString());
                    accts acc = ent.accts.First<accts>(p => p.acct_id == account_id);

                    var currency_id = int.Parse(Session["currency_id"].ToString());
                    currencies cur = ent.currencies.First<currencies>(p => p.currency_id == currency_id);

                    jnl.description = newVals["description"].ToString();
                    jnl.frgn_dbt_amt = double.Parse(newVals["frgn_dbt_amt"].ToString());
                    jnl.frgn_crdt_amt = double.Parse(newVals["frgn_crdt_amt"].ToString());
                    jnl.rate = double.Parse(newVals["rate"].ToString());
                    jnl.dbt_amt = jnl.frgn_dbt_amt * jnl.rate;
                    jnl.crdt_amt = jnl.frgn_crdt_amt * jnl.rate;
                    jnl.currencies = cur;

                    if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                        && Session["gl_ou_id"].ToString() != "0")
                    {
                        var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                        gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                        jnl.gl_ou = ou;
                    }

                    if (newVals["ref_no"] != null && newVals["ref_no"].ToString() != "")
                        jnl.ref_no = newVals["ref_no"].ToString();
                    jnl.accts = acc;
                    jnl.tx_date = (DateTime)newVals["tx_date"];
                    jnl.modification_date = DateTime.Now;
                    jnl.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid2.EditIndexes.Clear();
                    RadGrid2.MasterTableView.IsItemInserted = false;
                    RadGrid2.MasterTableView.Rebind();
                    Session["acct_id"] = 0;
                    Session["currency_id"] = 0;
                }
                else
                {
                    divError.Style["visibility"] = "visible";
                    spanError.InnerHtml = "The selected date belongs to a closed period. Please change.";
                }
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
                var newVals = new System.Collections.Specialized.ListDictionary();
                newVals["jnl_batch_id"] = 0;
                newVals["jnl_id"] = 0;
                newVals["ref_no"] = string.Empty;
                newVals["acct_id"] = 0;
                Session["acct_id"] = 0;
                newVals["gl_ou_id"] = 0;
                Session["gl_ou_id"] = 0;
                if (Session["tx_date"] != null)
                    newVals["tx_date"] = (DateTime)Session["tx_date"];
                else
                    newVals["tx_date"] = DateTime.Now;
                newVals["description"] = string.Empty;
                newVals["currency_id"] = 0;
                newVals["rate"] = 0.0; 
                newVals["dbt_amt"] = 0.0;
                newVals["crdt_amt"] = 0.0;
                newVals["frgn_dbt_amt"] = 0.0;
                newVals["frgn_crdt_amt"] = 0.0; 
                newVals["creator"] = User.Identity.Name;
                newVals["creation_date"] = DateTime.Now;
                e.Item.OwnerTableView.InsertItem(newVals);
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
                Session["currency_id"] = (from c in ent.jnl_tmp
                                        from u in ent.currencies
                                        where c.currencies.currency_id== u.currency_id
                                                && c.jnl_id == jnl_id
                                        select new { u.currency_id }).FirstOrDefault().currency_id;
                var ou = (from c in ent.jnl_tmp
                                        from u in ent.vw_gl_ou
                                        where c.gl_ou.ou_id == u.ou_id
                                                && c.jnl_id == jnl_id
                                        select new { u.ou_id }).FirstOrDefault();
                if (ou != null) Session["gl_ou_id"] = ou.ou_id;
                DateTime? txDate = (from c in ent.jnl_tmp
                                    where c.jnl_id == jnl_id
                                    select c.tx_date).FirstOrDefault();
                Session["tx_date"] = txDate;
            }
        }

        protected void RadGrid2_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                var jnl_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("jnl_id").ToString());
                Session["currency_id"] = (from c in ent.jnl
                                            from u in ent.currencies
                                            where c.currencies.currency_id == u.currency_id
                                                && c.jnl_id == jnl_id
                                            select new { u.currency_id }).FirstOrDefault().currency_id;
                Session["acct_id"] = (from c in ent.jnl
                                        from u in ent.accts
                                        where c.accts.acct_id == u.acct_id
                                                && c.jnl_id == jnl_id
                                        select new { u.acct_id }).FirstOrDefault().acct_id;
                Session["currency_id"] = (from c in ent.jnl
                                            from u in ent.currencies
                                            where c.currencies.currency_id == u.currency_id
                                                    && c.jnl_id == jnl_id
                                            select new { u.currency_id }).FirstOrDefault().currency_id;
                var ou = (from c in ent.jnl
                          from u in ent.vw_gl_ou
                          where c.gl_ou.ou_id == u.ou_id
                                  && c.jnl_id == jnl_id
                          select new { u.ou_id }).FirstOrDefault();
                if (ou != null) Session["gl_ou_id"] = ou.ou_id;
                DateTime? txDate = (from c in ent.jnl
                                    where c.jnl_id == jnl_id
                                    select c.tx_date).FirstOrDefault();
                Session["tx_date"] = txDate;
            }
        }
          
        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                var accs = (from a in ent.accts
                            from c in ent.currencies
                            from h in ent.acct_heads
                            where a.currencies.currency_id == c.currency_id
                                && a.acct_heads.acct_head_id==h.acct_head_id
                            select new
                            {
                                a.acct_id,
                                a.acc_num,
                                a.acc_name,
                                c.major_name,
                                c.major_symbol,
                                h.head_name
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
                    item.Attributes.Add("head_name", cur.head_name);

                    cboAcc.Items.Add(item);

                } 
                if (Session["acct_id"] != null && Session["acct_id"].ToString() != "0")
                    cboAcc.SelectedValue = Session["acct_id"].ToString();
                else
                    cboAcc.Text = " ";
            }
            catch (Exception ex) { }
        }

        protected void PopulateCurrencies(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                cboAcc.Items.Clear();
                foreach (var cur in ent.currencies)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.major_name;
                    item.Value = cur.currency_id.ToString();

                    item.Attributes.Add("major_symbol", cur.major_symbol);
                    item.Attributes.Add("major_name", cur.major_name);
                    item.Attributes.Add("current_buy_rate", cur.current_buy_rate.ToString("#,###.#0"));

                    cboAcc.Items.Add(item);

                }
                if (Session["currency_id"] != null && Session["currency_id"].ToString() != "0")
                    cboAcc.SelectedValue = Session["currency_id"].ToString();
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
                rcb = item["currency_id"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateCurrencies(rcb);
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboCur_SelectedIndexChanged);
                }
                var di = item["tx_date"].Controls[1] as RadDatePicker;
                if (di != null)
                {
                    di.SelectedDate = (DateTime?)Session["tx_date"];
                }
            }
        }

        public void RadGrid2_ItemCreated(object sender, GridItemEventArgs e)
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
                rcb = item["currency_id"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateCurrencies(rcb);
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboCur_SelectedIndexChanged);
                }
                var di = item["tx_date"].Controls[1] as RadDatePicker;
                if (di != null)
                {
                    di.SelectedDate = (DateTime?)Session["tx_date"];
                }
            }
        }

        void cboAcc_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["acct_id"] = e.Value; 
            int accID = -1;
            if (Session["acct_id"] != null && int.TryParse(Session["acct_id"].ToString(), out accID) == true)
            {
                if (accID > 0)
                {
                    core_dbEntities ent = new core_dbEntities();
                    var cur = (from c in ent.currencies
                                   from a in ent.accts
                               where c.currency_id==a.currencies.currency_id
                                    && a.acct_id==accID
                                   select c).FirstOrDefault()
;
                    if (cur != null)
                    {
                        var items = RadGrid1.MasterTableView.GetItems(GridItemType.EditItem);
                        if (items.Length == 1)
                        {
                            var cboCur = items[0].FindControl("cboCur") as Telerik.Web.UI.RadComboBox;
                            if (cboCur != null)
                            {
                                cboCur.SelectedValue = cur.currency_id.ToString();
                                cboCur.Text = cur.major_name;
                                Session["currency_id"] = cur.currency_id;
                                //PopulateCurrencies(cboCur);
                            }
                            var txtRate = items[0].FindControl("txtRate") as Telerik.Web.UI.RadNumericTextBox;
                            if (txtRate != null)
                            {
                                txtRate.Value = cur.current_buy_rate;
                            }
                        }
                    }
                }
            }
        }

        void cboCC_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["gl_ou_id"] = e.Value;
        }

        void cboCur_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["currency_id"] = e.Value;
            int currencyID =-1;
            if (Session["currency_id"] != null && int.TryParse(Session["currency_id"].ToString(), out currencyID) == true)
            {
                if (currencyID > 0)
                {
                    core_dbEntities ent = new core_dbEntities();
                    var cur = ent.currencies.FirstOrDefault(p => p.currency_id == currencyID);
                    if (cur != null)
                    {
                        var items = RadGrid1.MasterTableView.GetItems(GridItemType.EditItem);
                        if (items.Length == 1)
                        {
                            var txtRate = items[0].FindControl("txtRate") as Telerik.Web.UI.RadNumericTextBox;
                            if (txtRate != null)
                            {
                                txtRate.Value = cur.current_buy_rate;
                            }
                        }
                    }
                }
            }
        }

        protected void RadTreeView1_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            try
            {
                RadTreeNode clickedNode = e.Node;
                core_dbEntities ent = new core_dbEntities();

                switch (e.MenuItem.Value)
                {
                    case "New":
                        var itemType = clickedNode.Value.Split(':')[0];
                            AddNew();
                            break;  
                    case "Delete":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "b")
                        {
                            string  batchNo = e.Node.Value.Split(':')[1];
                            var b = ent.jnl_batch_tmp.FirstOrDefault(p => p.batch_no == batchNo);
                            if (b != null)
                            {
                                ent.jnl_batch_tmp.Remove(b);
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
                            AddNew();
                        }  
                        break;
                    case "Edit":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "b")
                        {
                            string batchNo = e.Node.Value.Split(':')[1];
                            var b = ent.jnl_batch_tmp.FirstOrDefault(p => p.batch_no == batchNo);
                            if (b != null)
                            {
                                Edit(batchNo);
                            } 
                        } 
                        break;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        public void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                Button addButton = e.Item.FindControl("AddNewRecordButton") as Button;
                addButton.Visible = false;
                LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                lnkButton.Visible = false;
            }
        }

        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtTransactionDate.SelectedDate.Value.CanClose())
            {
                Session["tx_date"] = dtTransactionDate.SelectedDate;
                divError.Style["visibility"] = "hidden";
                spanError.InnerHtml = "";
            }
            else
            {
                divError.Style["visibility"] = "visible";
                spanError.InnerHtml = "The selected date belongs to a closed period. Please change.";
            }
        }

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var jnl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["jnl_id"];
            var jnl = ent.jnl_tmp.FirstOrDefault(p => p.jnl_id == jnl_id);
            if (jnl != null)
            {
                ent.jnl_tmp.Remove(jnl);
                ent.SaveChanges();
            }
        }

        protected void RadGrid2_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var jnl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["jnl_id"];
            var jnl = ent.jnl.FirstOrDefault(p => p.jnl_id == jnl_id);
            if (jnl != null)
            {
                ent.jnl.Remove(jnl);
                ent.SaveChanges();
            }
        } 

    }
}
