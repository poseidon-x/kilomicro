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

namespace coreERP.gl.v
{
    public partial class post : corePage
    {
        public override string URL
        {
            get { return "~/gl/pc/post.aspx"; }
        }

        private void Edit(string batchNo)
        {
        }

        List<jnl_batch_stg> journalEntries;
        core_dbEntities ent; 
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
                    RenderTree();
                    var profile = ent.comp_prof.FirstOrDefault();
                    if(profile != null && profile.currency_id != null)
                        Session["currency_id"] = profile.currency_id;
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

        private void RenderTree()
        {
            try
            {
                using (var gent = new coreGLEntities())
                {
                    this.RadTreeView1.Nodes.Clear();
                    RadTreeNode rootNode = new RadTreeNode("Voucher", "__root__");
                    rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                    rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    this.RadTreeView1.Nodes.Add(rootNode);
                    var years = (from jnl in gent.v_dtl
                                 from batch in gent.v_head
                                 where jnl.v_head.v_head_id == batch.v_head_id
                                    && batch.posted==false
                                 orderby jnl.tx_date.Year descending
                                 select jnl.tx_date.Year).Distinct();
                    foreach (var year in years)
                    {
                        RadTreeNode node = new RadTreeNode(year.ToString(), "y:" + year.ToString());
                        node.Visible = true;
                        node.ImageUrl = "~/images/tree/folder_open.jpg";
                        node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        node.ToolTip = "Expand to see PC transactions for year: " + year.ToString();
                        node.Checkable = true;
                        rootNode.Nodes.Add(node);
                        var months = (from jnl in gent.v_dtl
                                      from batch in gent.v_head
                                      where jnl.v_head.v_head_id == batch.v_head_id
                                          && jnl.tx_date.Year == year
                                            && batch.posted == false
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
                            childnode.ToolTip = "Expand to see PC transactions for month: " + year.ToString() +
                                ", " + monthName;
                            childnode.Checkable = true;
                            node.Nodes.Add(childnode);
                            var days = (from jnl in gent.v_dtl
                                        from batch in gent.v_head
                                        where jnl.v_head.v_head_id == batch.v_head_id
                                            && jnl.tx_date.Year == year
                                            && jnl.tx_date.Month == month
                                            && batch.posted == false
                                        orderby jnl.tx_date.Day descending
                                        select jnl.tx_date.Day).Distinct();
                            foreach (var day in days)
                            {
                                RadTreeNode gchildnode = new RadTreeNode(day.ToString(), "d:" + year.ToString() +
                                "," + month.ToString() + day.ToString());
                                gchildnode.Visible = true;
                                gchildnode.ImageUrl = "~/images/tree/folder_open.jpg";
                                gchildnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                                gchildnode.ToolTip = "Expand to see PC transactions for date: " +
                                    (new DateTime(year, month, day)).ToString("dd-MMM-yyyy");
                                gchildnode.Checkable = true;
                                childnode.Nodes.Add(gchildnode);
                                var batches = (from jnl in gent.v_dtl
                                               from batch in gent.v_head
                                               where jnl.v_head.v_head_id == batch.v_head_id
                                                   && jnl.tx_date.Year == year
                                                   && jnl.tx_date.Month == month
                                                   && jnl.tx_date.Day == day
                                                    && batch.posted == false
                                               orderby batch.batch_no
                                               select batch).Distinct();
                                foreach (var b in batches)
                                {
                                    if (b.AllowedOU(Context.User.Identity.Name))
                                    {
                                        RadTreeNode gchildnode2 = new RadTreeNode(b.batch_no, "b:" + b.batch_no + ":u");
                                        gchildnode2.Visible = true;
                                        gchildnode2.ImageUrl = "~/images/editNew.jpg";
                                        gchildnode2.ExpandedImageUrl = "~/images/editNew.jpg";

                                        gchildnode2.ToolTip = "Click to see PC transactions for batch no.: " + b.batch_no;
                                        //gchildnode2.NavigateUrl = "~/gl/journal/local.aspx?op=edit&batchno=" +
                                        //    batchNo; 
                                        gchildnode2.PostBack = true;
                                        gchildnode2.PostBack = true;
                                        gchildnode2.Checkable = true;
                                        gchildnode.Nodes.Add(gchildnode2);
                                    }
                                }
                            }
                        }
                    }
                    rootNode.Expanded = true;
                }
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
            errorMsg += "<br /> Error Detail: " + (ex.InnerException??ex).Message;
            errorMsg += "Please correct and continue or cancel.";
            divError.Style["visibility"] = "visible";
            spanError.InnerHtml = errorMsg;
        }

        public string AccountName(int acct_id)
        {
            var item = (from m in ent.accts
                        where m.acct_id == acct_id
                        select new
                        {
                            m.acc_num,
                            m.acc_name
                        }).FirstOrDefault();
            return item == null ? "" : item.acc_num + " - " + item.acc_name;
        }

        public string CostCenterName(object cost_center_id)
        {
            int? id = (int?)cost_center_id;
            var item = (from m in ent.vw_gl_ou
                        where m.ou_id == id
                        select new
                        {
                            m.ou_name1
                        }).FirstOrDefault();
            return item == null ? "" : item.ou_name1;
        }

        public string CurrencyName(int currency_id)
        { 
                var item = (from m in ent.currencies
                            where m.currency_id == currency_id
                            select new
                            {
                                m.major_name,
                                m.major_symbol
                            }).FirstOrDefault();
                return item == null ? "" : item.major_name;
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
                    case "New":
                        
                        break;  
                    case "Delete":
                        var itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "b")
                        {
                            string  batchNo = e.Node.Value.Split(':')[1];
                            using (var gent = new coreGLEntities())
                            {
                                var b = gent.v_head.FirstOrDefault(p => p.batch_no == batchNo);
                                if (b != null)
                                {
                                    gent.v_head.Remove(b);
                                    gent.SaveChanges();
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
                        }  
                        break;
                    case "Edit":
                        itemType = clickedNode.Value.Split(':')[0];
                        if (itemType == "b")
                        { 
                               
                        } 
                        break;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> processed = new List<string>();
                using (var gent = new coreGLEntities())
                {
                    journalEntries = new List<jnl_batch_stg>();
                    var cnt = 0;
                    var existingJB = gent.jnl_batch_stg.Where(p => p.creator == User.Identity.Name);
                    var existingJ = gent.jnl_stg.Where(p => p.creator == User.Identity.Name);
                    foreach (var i in existingJ)
                    {
                        gent.jnl_stg.Remove(i);
                    }
                    foreach (var i in existingJB)
                    {
                        gent.jnl_batch_stg.Remove(i);
                    }
                    foreach (var node in RadTreeView1.CheckedNodes)
                    {
                        if (node.Value.Split(':')[0] == "b")
                        {
                            var batchNo = node.Value.Split(':')[1];
                            var tb = gent.v_head.FirstOrDefault(p => p.batch_no == batchNo);
                            if (tb != null && processed.Contains(batchNo) == false)
                            {
                                cnt--;
                                var jb = new jnl_batch_stg
                                {
                                    batch_no = tb.batch_no,
                                    creation_date = DateTime.Now,
                                    creator = User.Identity.Name,
                                    is_adj = false,
                                    multi_currency = ent.comp_prof.FirstOrDefault().currency_id == tb.currency_id ? false : true,
                                    posted = false,
                                    source = "VC"
                                };
                                gent.jnl_batch_stg.Add(jb);
                                var js = gent.v_dtl.Where(p => p.v_head.v_head_id == tb.v_head_id).ToList<v_dtl>();
                                var retained = 0.0;
                                var total = 0.0;
                                var ft = (from a in gent.v_ftr
                                          where a.v_head_id == tb.v_head_id
                                          select a.tot_amount).ToList();
                                var tt = (from a in gent.v_dtl
                                          where a.v_head_id == tb.v_head_id
                                          select a.amount).ToList();
                                if (ft.Count > 0) retained = ft.Sum();
                                if (tt.Count > 0) total = tt.Sum() - retained - tb.with_amt;

                                var description = (from j in gent.v_dtl
                                                   where j.v_head_id==tb.v_head_id
                                                   select j.description).FirstOrDefault();
                                var tx_date = (from j in gent.v_dtl
                                               where j.v_head_id == tb.v_head_id
                                               select j.tx_date).FirstOrDefault();
                                var ref_no = (from j in gent.v_dtl
                                              where j.v_head_id == tb.v_head_id
                                              select j.ref_no).FirstOrDefault();
                                var cost_center_id = (from j in gent.v_dtl
                                                      where j.v_head_id == tb.v_head_id
                                                      select j.gl_ou_id).FirstOrDefault();

                                foreach (v_dtl j in js)
                                {
                                    var item = new jnl_stg
                                    { 
                                        acct_id=j.acct_id,
                                        dbt_amt = (tb.v_type != "C") ? j.amount * tb.rate : 0,
                                        crdt_amt = (tb.v_type == "C") ? j.amount * tb.rate : 0,
                                        description = j.description,
                                        creation_date = DateTime.Now,
                                        creator = User.Identity.Name,
                                        currency_id = tb.currency_id,
                                        frgn_dbt_amt = j.amount,
                                        cost_center_id = j.gl_ou_id ,
                                        rate = tb.rate,
                                        recipient = tb.recipient,
                                        ref_no = j.ref_no,
                                        tx_date = j.tx_date,
                                        jnl_batch_stg = jb,
                                        check_no=tb.check_no
                                    };
                                    gent.jnl_stg.Add(item);
                                }

                                var fs = gent.v_ftr.Where(p => p.v_head.v_head_id == tb.v_head_id).ToList<v_ftr>();
                                foreach (v_ftr f in fs)
                                {
                                    var item = new jnl_stg
                                    {
                                        acct_id = f.acct_id,
                                        crdt_amt = (tb.v_type != "C") ? f.tot_amount * tb.rate : 0,
                                        dbt_amt = (tb.v_type == "C") ? f.tot_amount * tb.rate : 0,
                                        description = f.description,
                                        creation_date = DateTime.Now,
                                        creator = User.Identity.Name,
                                        currency_id = tb.currency_id,
                                        frgn_dbt_amt = f.tot_amount,
                                        cost_center_id = cost_center_id,
                                        rate = tb.rate,
                                        recipient = tb.recipient,
                                        ref_no = ref_no,
                                        tx_date = tx_date,
                                        jnl_batch_stg = jb,
                                        check_no = tb.check_no
                                    };
                                    gent.jnl_stg.Add(item);
                                }

                                var item2 = new jnl_stg
                                {
                                    acct_id = tb.bank_acct_id,
                                    crdt_amt = (tb.v_type != "C") ? total * tb.rate : 0,
                                    dbt_amt = (tb.v_type == "C") ? total * tb.rate : 0,
                                    description = description,
                                    creation_date = DateTime.Now,
                                    creator = User.Identity.Name,
                                    currency_id = tb.currency_id,
                                    frgn_dbt_amt = total,
                                    rate = tb.rate,
                                    recipient = tb.recipient,
                                    ref_no = ref_no,
                                    cost_center_id = cost_center_id,
                                    tx_date = tx_date,
                                    jnl_batch_stg = jb
                                };
                                gent.jnl_stg.Add(item2);
 
                                if (tb.is_withheld == true && tb.with_amt > 0
                                    && tb.with_acct_id != null && tb.with_acct_id.Value>0)
                                {
                                    item2 = new jnl_stg
                                    {
                                        acct_id = tb.with_acct_id.Value,
                                        crdt_amt = (tb.v_type != "C") ? tb.with_amt * tb.rate : 0,
                                        dbt_amt = (tb.v_type == "C") ? tb.with_amt * tb.rate : 0,
                                        description = "Withholding Tax " + description,
                                        creation_date = DateTime.Now,
                                        creator = User.Identity.Name,
                                        currency_id = tb.currency_id,
                                        frgn_dbt_amt = tb.with_amt,
                                        rate = tb.rate,
                                        recipient = tb.recipient,
                                        ref_no = ref_no,
                                        cost_center_id = cost_center_id,
                                        tx_date = tx_date,
                                        jnl_batch_stg = jb
                                    };
                                    gent.jnl_stg.Add(item2);
                                }

                                journalEntries.Add(jb);
                                btnPost.Enabled = true;
                                processed.Add(batchNo);
                            }
                        }
                    }
                    gent.SaveChanges();
                    //journalEntries = gent.jnl_batch_stg.Where(p => p.creator == User.Identity.Name);
                    Session["journalEntries"] = journalEntries;
                    EntityDataSource1.WhereParameters[0].DefaultValue = User.Identity.Name;
                    //EntityDataSource2.WhereParameters[0].DefaultValue = User.Identity.Name;
                    RadGrid1.DataBind();
                    
                    pnlEditJournal.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            try
            {
                using (var gent = new coreGLEntities())
                {
                    journalEntries = Session["journalEntries"] as List<jnl_batch_stg>;
                    if (journalEntries != null)
                    {
                        foreach (var b in journalEntries)
                        { 
                            var sb = gent.jnl_batch_stg.FirstOrDefault(p => p.batch_no == b.batch_no);
                            if(sb.sum_crdt_amt()!=sb.sum_dbt_amt())throw new ApplicationException("Some batches do not balance");
                            var jb = sb.ToJournal();
                            ent.jnl_batch.Add(jb);
                            var js = gent.jnl_stg.Where(p => p.jnl_batch_id == sb.jnl_batch_id);
                            foreach (var j in js)
                            {
                                var nj = j.ToJournal(ent);
                                nj.jnl_batch = jb;
                                ent.jnl.Add(nj);
                            }
                            var tb = gent.v_head.FirstOrDefault(p => p.batch_no == b.batch_no);
                            tb.posted = true;
                        }
                        var existingJB = gent.jnl_batch_stg.Where(p => p.creator == User.Identity.Name);
                        var existingJ = gent.jnl_stg.Where(p => p.creator == User.Identity.Name);
                        foreach (var i in existingJ)
                        {
                            gent.jnl_stg.Remove(i);
                        }
                        foreach (var i in existingJB)
                        {
                            gent.jnl_batch_stg.Remove(i);
                        }
                       
                        ent.SaveChanges();
                        gent.SaveChanges();
                        RenderTree();
                        Session["journalEntries"] = null;
                        btnPost.Enabled = false;
                        EntityDataSource1.WhereParameters[0].DefaultValue = User.Identity.Name;
                        //EntityDataSource2.WhereParameters[0].DefaultValue = User.Identity.Name;
                        RadGrid1.DataBind();

                        pnlEditJournal.Visible = false;
                    }
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
                addButton.Visible = false;
                LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                lnkButton.Visible = false;
            }
        } 

    }
}
