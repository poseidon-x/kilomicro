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

namespace coreERP.gl.pc
{
    public partial class pc : corePage
    {
        public override string URL
        {
            get { return "~/gl/pc/pc.aspx"; }
        }

        private void AddNew()
        {
            batch = new pc_head
            {
                batch_no="",
                creation_date=DateTime.Now,
                creator=Context.User.Identity.Name,
                currency_id=0,
                pc_acct_id = 0,
                posted=false,
                rate=1,
                recipient="",
                recipient_id=0
            };
            var profile = ent.comp_prof.FirstOrDefault();
            if (profile != null && profile.currency_id != null)
                Session["currency_id"] = profile.currency_id;
            gent.pc_head.Add(batch);
            Session["batch"] = batch;
            Session["tx_count"] = 0;
            dtTransactionDate.SelectedDate = DateTime.Now;
            txtBatchNumber.Text = "UNASSIGNED";
            txtCreator.Text = Context.User.Identity.Name;
            txtBatchState.Text = "UNPOSTED";
            txtRecipient.Text = batch.recipient;
            SetBalance();
            SetRate();
            cboAcc.Enabled = true;
            txtRate.Enabled = true;
            txtRecipient.Enabled = true;
            cboCur.Enabled = true;
            dtTransactionDate.Enabled = true;
            PopulateAccounts();
            try
            {
                this.EntityDataSource1.WhereParameters["pc_head_id"].DefaultValue = batch.pc_head_id.ToString();
            }
            catch (Exception ex)
            {
                this.EntityDataSource1.WhereParameters["pc_head_id"].DefaultValue = "0";
            }
            this.RadGrid1.Visible = true;
            this.RadGrid2.Visible = false; 
        }

        private void Edit(string batchNo)
        {
            try
            {
                batch = gent.pc_head.First(p => p.batch_no == batchNo);
                Session["batch"] = batch;
                Session["tx_count"] = gent.pc_dtl.Count(p => p.pc_head.pc_head_id == batch.pc_head_id);
                dtTransactionDate.SelectedDate = gent.pc_dtl.First(p => p.pc_head.pc_head_id == batch.pc_head_id).tx_date;
                txtBatchNumber.Text = batch.batch_no;
                txtCreator.Text = batch.creator;
                txtBatchState.Text = "UNPOSTED";
                txtRecipient.Text = batch.recipient;
                Session["currency_id"] = batch.currency_id;
                Session["pc_acct_id"] = batch.pc_acct_id;
                cboAcc.SelectedValue = batch.pc_acct_id.ToString();
                cboCur.SelectedValue=batch.currency_id.ToString();
                txtRate.Value = batch.rate;
                txtRate.Value=batch.rate;
                SetBalance();
                SetRate();
                cboAcc.Enabled = true;
                txtRate.Enabled = true;
                txtRecipient.Enabled = true;
                cboCur.Enabled = true;
                dtTransactionDate.Enabled = true;
                PopulateAccounts();
                try
                {
                    this.EntityDataSource1.WhereParameters["pc_head_id"].DefaultValue = batch.pc_head_id.ToString();
                }
                catch (Exception ex)
                {
                    this.EntityDataSource1.WhereParameters["pc_head_id"].DefaultValue = "0";
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
            batch = gent.pc_head.First(p => p.batch_no == batchNo);
            Session["batch"] = batch;
            dtTransactionDate.SelectedDate = batch.creation_date;
            txtBatchNumber.Text = batch.batch_no;
            txtCreator.Text = batch.creator;
            txtBatchState.Text = "POSTED"; 
            txtRecipient.Text = batch.recipient;
            Session["currency_id"] = batch.currency_id;
            Session["pc_acct_id"] = batch.pc_acct_id;
            cboAcc.SelectedValue = batch.pc_acct_id.ToString();
            cboCur.SelectedValue = batch.currency_id.ToString();
            txtRate.Value = batch.rate;
            txtRate.Value = batch.rate;
            SetBalance();
            SetRate();
            cboAcc.Enabled = false;
            txtRate.Enabled = false;
            txtRecipient.Enabled = false;
            cboCur.Enabled = false;
            dtTransactionDate.Enabled = false;
            PopulateAccounts();
            try
            {
                this.EntityDataSource2.WhereParameters["pc_head_id"].DefaultValue = batch.pc_head_id.ToString();
            }
            catch (Exception ex)
            {
                this.EntityDataSource2.WhereParameters["pc_head_id"].DefaultValue = "0";
            }
            this.RadGrid1.Visible = false;
            this.RadGrid2.Visible = true;
        }

        core_dbEntities ent;
        coreGLEntities gent;
        pc_head batch; 
        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ent = new core_dbEntities();
                gent = new coreGLEntities();
                divError.Style["visibility"] = "hidden";
                RadTreeView1.NodeClick += new RadTreeViewEventHandler(RadTreeView1_NodeClick);
                if (!Page.IsPostBack)
                {
                    var profile = ent.comp_prof.FirstOrDefault();
                    if (profile != null && profile.currency_id != null)
                        Session["currency_id"] = profile.currency_id;
                    var da = (from p in ent.def_accts
                              where p.code == "PC"
                                  select p.accts.acct_id).FirstOrDefault();
                    if (da != null)
                        Session["pc_acct_id"] = da;
                    PopulateCurrencies(); 
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
                }
                else if (Session["batch"] != null)
                {
                    batch = Session["batch"] as pc_head;
                }
                try
                {
                    this.EntityDataSource1.WhereParameters["pc_head_id"].DefaultValue = batch.pc_head_id.ToString();
                    this.EntityDataSource2.WhereParameters["pc_head_id"].DefaultValue = batch.pc_head_id.ToString();
                    //this.EntityDataSource2.WhereParameters["batch_id"].DefaultValue = batchPost.jnl_batch_id.ToString();
                }
                catch (Exception ex)
                {
                    this.EntityDataSource1.WhereParameters["pc_head_id"].DefaultValue = "0";
                    this.EntityDataSource2.WhereParameters["pc_head_id"].DefaultValue = "0";
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
                    string batchNo = keys[1];
                    using (var gent = new coreGLEntities())
                    {
                        var b = gent.pc_head.FirstOrDefault(p => p.batch_no == batchNo);
                        if (b != null)
                        {
                            if (b.posted == false)
                            {
                                Edit(batchNo);
                            }
                            else
                            {
                                EditPosted(batchNo);
                            }
                        }
                    }
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
                    RadTreeNode rootNode = new RadTreeNode("Petty Cash", "__root__");
                    rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                    rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    this.RadTreeView1.Nodes.Add(rootNode);
                    var years = (from jnl in gent.pc_dtl
                                 from batch in gent.pc_head
                                 where jnl.pc_head.pc_head_id == batch.pc_head_id
                                 select new { jnl.tx_date.Year }).Distinct().OrderByDescending(p => p.Year);
                    foreach (var year in years)
                    {
                        RadTreeNode node = new RadTreeNode(year.Year.ToString(), "y:" + year.Year.ToString());
                        node.Visible = true;
                        node.ImageUrl = "~/images/tree/folder_open.jpg";
                        node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        node.ToolTip = "Expand to see PC transactions for year: " + year.ToString();
                        rootNode.Nodes.Add(node);
                        var months = (from jnl in gent.pc_dtl
                                      from batch in gent.pc_head
                                      where jnl.pc_head.pc_head_id == batch.pc_head_id
                                          && jnl.tx_date.Year == year.Year
                                      select new { jnl.tx_date.Month }).Distinct().OrderByDescending(p=>p.Month);
                        foreach (var month in months)
                        {
                            var monthName = MonthName(month.Month);
                            RadTreeNode childnode = new RadTreeNode(monthName, "m:" + year.Year.ToString() +
                                "," + month.Month.ToString());
                            childnode.Visible = true;
                            childnode.ImageUrl = "~/images/tree/folder_open.jpg";
                            childnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                            childnode.ToolTip = "Expand to see PC transactions for month: " + year.ToString() +
                                ", " + monthName;
                            childnode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                            node.Nodes.Add(childnode);  
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

        private void RenderTreeMonth(RadTreeNode childnode)
        {
            try
            {
                using (var gent = new coreGLEntities())
                {
                    var split = childnode.Value.Split(':');
                    var split2 = split[1].Split(',');
                    var year = int.Parse(split2[0]);
                    var month = int.Parse(split2[1]);

                    var days = (from jnl in gent.pc_dtl
                                from batch in gent.pc_head
                                where jnl.pc_head.pc_head_id == batch.pc_head_id
                                    && jnl.tx_date.Year == year
                                    && jnl.tx_date.Month == month
                                select new { jnl.tx_date.Day }).Distinct().OrderByDescending(p => p.Day);
                    foreach (var day in days)
                    {
                        RadTreeNode gchildnode = new RadTreeNode(day.Day.ToString(), "d:" + year.ToString() +
                        month.ToString().PadLeft(2, '0') + day.Day.ToString().PadLeft(2, '0'));
                        gchildnode.Visible = true;
                        gchildnode.ImageUrl = "~/images/tree/folder_open.jpg";
                        gchildnode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                        gchildnode.ToolTip = "Expand to see PC transactions for date: " +
                            (new DateTime(year, month, day.Day)).ToString("dd-MMM-yyyy");
                        gchildnode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                        childnode.Nodes.Add(gchildnode);

                    }
                } 
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void RenderTreeDay(RadTreeNode gchildnode)
        {
            try
            {
                using (var gent = new coreGLEntities())
                {
                    var prof = ent.comp_prof.SingleOrDefault();
                    var enforceOUSec = (prof == null) ? false : prof.enf_ou_sec;
                    var startDate = DateTime.ParseExact(gchildnode.Value.Split(':')[1], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    var endDate = startDate.AddDays(1).AddSeconds(-1);
                    var batches = (from jnl in gent.pc_dtl
                                   from batch in gent.pc_head
                                   where jnl.pc_head.pc_head_id == batch.pc_head_id
                                       && jnl.tx_date >= startDate
                                       && jnl.tx_date <= endDate
                                   select new { batch }).Distinct().OrderBy(p => p.batch.batch_no);
                    foreach (var b in batches)
                    {
                        if (enforceOUSec == false || b.batch.AllowedOU(Context.User.Identity.Name))
                        {
                            RadTreeNode gchildnode2 = new RadTreeNode(b.batch.batch_no, "b:" + b.batch.batch_no + ":u");
                            gchildnode2.Visible = true;
                            gchildnode2.ImageUrl = "~/images/editNew.jpg";
                            gchildnode2.ExpandedImageUrl = "~/images/editNew.jpg";

                            gchildnode2.ToolTip = "Click to see PC transactions for batch no.: " + b.batch.batch_no;
                            //gchildnode2.NavigateUrl = "~/gl/journal/local.aspx?op=edit&batchno=" +
                            //    batchNo; 
                            gchildnode2.PostBack = true;
                            gchildnode.Nodes.Add(gchildnode2);
                        }
                    }
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
            this.RadGrid1.ExportSettings.FileName = "coreERP_Petty_Cash";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_Petty_Cash";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_Petty_Cash";
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

        public string AccountName(int pc_dtl_id)
        {
            var j = gent.pc_dtl.FirstOrDefault(p => p.pc_dtl_id == pc_dtl_id);
            if (j != null)
            {
                var item = (from m in ent.accts
                            where m.acct_id == j.acct_id
                            select new
                            {
                                m.acc_num,
                                m.acc_name
                            }).FirstOrDefault();
                return item == null ? "" : item.acc_num + " - " + item.acc_name;
            }
            return "";
        }

        public string CurrencyName(int jnl_id)
        {
            return "";
        }

        public string CostCenterName(int pc_dtl_id)
        {
            var j = gent.pc_dtl.FirstOrDefault(p => p.pc_dtl_id == pc_dtl_id);
            if (j != null)
            {
                var item = (from m in ent.vw_gl_ou
                            where m.ou_id == j.gl_ou_id
                            select new
                            {
                                m.ou_name1
                            }).FirstOrDefault();
                return item == null ? "" : item.ou_name1;
            }
            return "";
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
                    var jnl = new pc_dtl();
                    jnl.description = newVals["description"].ToString();
                    if (newVals["check_no"] != null && newVals["check_no"].ToString() != "")
                        jnl.check_no = newVals["check_no"].ToString();
                    else
                        jnl.check_no = "";
                    jnl.amount = double.Parse(newVals["amount"].ToString());
                    if (newVals["ref_no"] != null && newVals["ref_no"].ToString() != "")
                        jnl.ref_no = newVals["ref_no"].ToString();
                    jnl.acct_id = acc.acct_id;
                    if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                         && Session["gl_ou_id"].ToString() != "0")
                    {
                        var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                        gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                        jnl.gl_ou_id = ou.ou_id;
                    }
                    jnl.tx_date = txDate;
                    jnl.creation_date = DateTime.Now;
                    jnl.creator = User.Identity.Name;
                    jnl.pc_head = batch.pc_head_id > 0 ? gent.pc_head.First(p => p.pc_head_id == batch.pc_head_id) : batch;
                    gent.pc_dtl.Add(jnl);
                    if (batch.batch_no == null || batch.batch_no == "")
                        this.txtBatchNumber.Text = coreExtensions.NextGLBatchNumber();
                    batch.batch_no = this.txtBatchNumber.Text;
                    batch.creation_date = DateTime.Now;
                    batch.creator = User.Identity.Name;
                    batch.recipient = txtRecipient.Text;
                    if (Session["pc_acct_id"] != null && Session["pc_acct_id"] !="" && Session["pc_acct_id"] !="0")
                    {
                        batch.pc_acct_id = int.Parse(Session["pc_acct_id"].ToString());
                    }
                    else
                    {
                        batch.pc_acct_id = int.Parse(cboAcc.SelectedValue);
                    }
                    batch.currency_id = int.Parse(cboCur.SelectedValue);

                    gent.SaveChanges(); //gent.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, batch);
                    Session["tx_count"] = (int)Session["tx_count"] + 1;
                    batch = gent.pc_head.First(p => p.pc_head_id == batch.pc_head_id);
                    Session["batch"] = batch;
                    this.EntityDataSource1.WhereParameters["pc_head_id"].DefaultValue = batch.pc_head_id.ToString();
                    Session["acct_id"] = 0;
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
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
                    var pc_dtl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pc_dtl_id"];
                    var jnl = (from p in gent.pc_dtl where p.pc_dtl_id == pc_dtl_id select p).First<pc_dtl>();

                    var account_id = int.Parse(Session["acct_id"].ToString());
                    accts acc = ent.accts.First<accts>(p => p.acct_id == account_id);
                     
                    jnl.description = newVals["description"].ToString();
                    jnl.amount = double.Parse(newVals["amount"].ToString()); 

                    if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                        && Session["gl_ou_id"].ToString() != "")
                    {
                        var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                        gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                        jnl.gl_ou_id = ou.ou_id;
                    }

                    if (newVals["ref_no"] != null && newVals["ref_no"].ToString() != "")
                        jnl.ref_no = newVals["ref_no"].ToString();
                    if (newVals["check_no"] != null && newVals["check_no"].ToString() != "")
                        jnl.check_no = newVals["check_no"].ToString();
                    else
                        jnl.check_no = "";
                    jnl.acct_id = acc.acct_id;
                    jnl.tx_date = (DateTime)newVals["tx_date"];
                    jnl.modification_date = DateTime.Now;
                    jnl.last_modifier = User.Identity.Name;
                    batch = gent.pc_head.FirstOrDefault(p => p.pc_head_id == batch.pc_head_id);
                    Session["batch"] = batch;
                    batch.last_modifier = User.Identity.Name;
                    batch.modification_date = DateTime.Now;
                    batch.currency_id = int.Parse(cboCur.SelectedValue);
                    batch.rate = txtRate.Value.Value;

                    if (Session["pc_acct_id"] != null && Session["pc_acct_id"] != "" && Session["pc_acct_id"] != "0")
                    {
                        batch.pc_acct_id = int.Parse(Session["pc_acct_id"].ToString());
                    }
                    else
                    {
                        batch.pc_acct_id = int.Parse(cboAcc.SelectedValue);
                    }

                    batch.recipient = txtRecipient.Text;
                    gent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                    Session["acct_id"] = 0; 
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
                newVals["pc_dtl_id"] = 0;
                newVals["pc_head_id"] = 0;
                newVals["ref_no"] = string.Empty;
                newVals["check_no"] = string.Empty;
                newVals["acct_id"] = 0; 
                newVals["gl_ou_id"] = 0;
                Session["gl_ou_id"] = 0;
                newVals["tx_date"] = DateTime.Now;
                newVals["description"] = string.Empty;
                newVals["amount"] = 0.0; 
                newVals["creator"] = User.Identity.Name;
                newVals["creation_date"] = DateTime.Now;
                e.Item.OwnerTableView.InsertItem(newVals);
            }
            else if (e.CommandName == "Edit")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                var pc_dtl_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("pc_dtl_id").ToString());
                Session["acct_id"] = (from c in gent.pc_dtl
                                        where c.pc_dtl_id == pc_dtl_id
                                        select new { c.acct_id }).FirstOrDefault().acct_id;
                var ou = (from c in gent.pc_dtl
                          where c.pc_dtl_id == pc_dtl_id
                                        select new { c.gl_ou_id }).FirstOrDefault();
                if (ou != null) Session["gl_ou_id"] = ou.gl_ou_id;
                DateTime? txDate = (from c in gent.pc_dtl
                                    where c.pc_dtl_id == pc_dtl_id
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

        protected void PopulateAccounts()
        {
            try
            {
                using (core_dbEntities ent = new core_dbEntities())
                {
                    int? id = null;
                    if (Session["pc_acct_id"] != null && Session["pc_acct_id"]!="0")
                    {
                        id = int.Parse(Session["pc_acct_id"].ToString());
                    }
                    var accs = (from a in ent.accts
                                from c in ent.currencies
                                where a.currencies.currency_id == c.currency_id
                                    && (id == null || a.acct_id == id)
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
                    if (Session["pc_acct_id"] == null || Session["pc_acct_id"].ToString().Trim() == "0")
                    {
                        cboAcc.Items.Insert(0, new RadComboBoxItem("Petty Cash Account", null));
                    }
                    else
                    {
                        cboAcc.SelectedValue = Session["pc_acct_id"].ToString();
                        SetBalance();
                    }
                }
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
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAccI_SelectedIndexChanged);
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

        public void RadGrid2_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
            {
                var item = e.Item as GridEditableItem;
                var rcb = item["acct_id"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
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
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboCur_SelectedIndexChanged);
                }
                var di = item["tx_date"].Controls[1] as RadDatePicker;
                if (di != null)
                {
                    di.SelectedDate = (DateTime?)Session["tx_date"];
                }
            }
        }

        void cboAccI_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["acct_id"] = e.Value;   
        }

        void cboCC_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["gl_ou_id"] = e.Value;
        }
         
        protected void cboCur_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Session["currency_id"] = e.Value;
                SetRate();
                if (Session["batch"] != null)
                {
                    batch = Session["batch"] as pc_head;
                    if (batch != null)
                    {
                        using (var gent = new coreGLEntities())
                        {
                            var b = gent.pc_head.FirstOrDefault(p => p.batch_no == batch.batch_no);
                            if (b != null)
                            {
                                b.currency_id = int.Parse(e.Value);
                                b.rate = txtRate.Value.Value;
                                gent.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        protected void cboAcc_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (e.Value != "" && e.Value != null && e.Value != "0")
            {
                Session["pc_acct_id"] = e.Value;
                SetBalance();
            }
        }

        private void SetRate()
        {
            int currencyID = -1;
            if (Session["currency_id"] != null && int.TryParse(Session["currency_id"].ToString(), out currencyID) == true)
            {
                if (currencyID > 0)
                {
                    core_dbEntities ent = new core_dbEntities();
                    var cur = ent.currencies.FirstOrDefault(p => p.currency_id == currencyID);
                    if (cur != null)
                    {
                        txtRate.Value = cur.current_buy_rate;
                    }
                }
            }
        }

        private void SetBalance()
        {
            int accID = -1;
            if (Session["pc_acct_id"] != null && int.TryParse(Session["pc_acct_id"].ToString(), out accID) == true)
            {
                if (accID > 0)
                {
                    using (var ent = new core_dbEntities())
                    {
                        var cur = ent.get_acc_bal(accID,DateTime.Now,null).ToList();
                        if (cur != null && cur.Count()>0)
                        {
                            txtBalance.Value = cur[0];
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
                using (var ent = new coreGLEntities())
                {

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
                                string batchNo = e.Node.Value.Split(':')[1];
                                var b = ent.pc_head.FirstOrDefault(p => p.batch_no == batchNo);
                                if (b != null && b.posted == false)
                                {
                                    ent.pc_head.Remove(b);
                                    ent.SaveChanges();
                                }
                                else
                                {
                                    ManageException(new ApplicationException("Cannot delete posted Petty Cash"));
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
                                var b = ent.pc_head.FirstOrDefault(p => p.batch_no == batchNo);
                                if (b != null)
                                {
                                    if (b.posted == false)
                                    {
                                        Edit(batchNo);
                                    }
                                    else
                                    {
                                        EditPosted(batchNo);
                                    }
                                }
                            }
                            break;
                    }
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

        protected void PopulateCurrencies()
        {
            try
            {
                using (core_dbEntities ent = new core_dbEntities())
                {
                    cboCur.DataSource = ent.currencies.ToList();
                    cboCur.DataBind();
                    if (Session["currency_id"] == null || Session["currency_id"].ToString().Trim() == "0")
                    {
                        cboCur.Items.Insert(0, new RadComboBoxItem("Transaction Currency", null));
                    }
                    else
                    {
                        cboCur.SelectedValue = Session["currency_id"].ToString();
                        SetRate();
                    }
                }
            }
            catch (Exception ex) { }
        }
         
        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                var accs = (from a in ent.accts
                            from c in ent.currencies
                            from h in ent.acct_heads
                            where a.currencies.currency_id == c.currency_id
                                && a.acct_heads.acct_head_id == h.acct_head_id
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

                    item.Text = numb4 ? cur.acc_num + " - " + cur.acc_name
                         : cur.acc_name + " - " + cur.acc_num; ;
                    item.Value = cur.acct_id.ToString();

                    item.Attributes.Add("acc_num", cur.acc_num);
                    item.Attributes.Add("acc_name", cur.acc_name);
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

        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var pc_dtl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pc_dtl_id"];
            var pc = gent.pc_dtl.FirstOrDefault(p => p.pc_dtl_id == pc_dtl_id);
            if (pc != null)
            {
                gent.pc_dtl.Remove(pc);
                gent.SaveChanges();
            }
        }

        protected void RadGrid2_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var pc_dtl_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["pc_dtl_id"];
            var pc = gent.pc_dtl.FirstOrDefault(p => p.pc_dtl_id == pc_dtl_id);
            if (pc != null)
            {
                gent.pc_dtl.Remove(pc);
                gent.SaveChanges();
            }
        }

        protected void RadTreeView1_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("m"))
            {
                RenderTreeMonth(e.Node);
            }
            else if (e.Node.Value.StartsWith("d"))
            {
                RenderTreeDay(e.Node);
            }
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
